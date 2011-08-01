using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class GoodReceiveNoteRepository : TransactionRepository
    {
        public GoodReceiveNoteRepository() : base() { }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (GoodReceiveNoteItem item in events.EVENT_ITEMS)
            {
                PurchaseOrder po = (PurchaseOrder)item.PO_ITEM.EVENT;
                assertConfirmedPO(po);
                assertValidDate(po, item.EVENT);
                SetStockCard(item, p);
                item.PO_ITEM.SetOSAgainstGRNItem(item);
                PurchaseOrderRepository.UpdateAgainstStatus(m_command, po, item.PO_ITEM);
            }
        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (GoodReceiveNoteItem item in events.EVENT_ITEMS)
            {
                assertUsedGRNItemByPRItem(item);
                PurchaseOrder po = (PurchaseOrder)item.PO_ITEM.EVENT;
                SetStockCard(item, p);
                item.PO_ITEM.UnSetOSAgainstGRNItem(item);
                PurchaseOrderRepository.UpdateAgainstStatus(m_command, po, item.PO_ITEM);
            }
        }
        private void assertConfirmedPO(Event p)
        {
            if (p.EVENT_STATUS.Equals(EventStatus.Entry))
                throw new Exception("PO not confirmed :" + p.CODE);
        }
        private void assertValidDate(Event po, Event grn)
        {
            if (grn.TRANSACTION_DATE < po.TRANSACTION_DATE)
                throw new Exception("GRN Date can not less than PO Date :" + po.CODE + " [ " + po.TRANSACTION_DATE.ToString("dd-MMM-yyyy") + " ] ");
        }
        private void assertUsedGRNItemByPRItem(EventItem item)
        {
            PurchaseReturnItem doItm = PurchaseReturnRepository.FindGRNItem(m_command, item.ID);
            if (doItm != null)
                throw new Exception("GRN Item allready used by PR item, delete PR first :" + doItm.EVENT.CODE);
        }
        protected override void doSave(Event e)
        {
            OdbcTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;
                GoodReceiveNote stk = (GoodReceiveNote)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = GoodReceiveNote.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (GoodReceiveNoteItem item in stk.EVENT_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = GoodReceiveNoteItem.SelectMaxIDSQL();
                    item.ID = Convert.ToInt32(m_command.ExecuteScalar());
                }
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        protected override void doUpdate(Event en)
        {
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                GoodReceiveNote e = (GoodReceiveNote)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (GoodReceiveNoteItem sti in e.EVENT_ITEMS)
                {
                    if (sti.ID > 0)
                    {
                        m_command.CommandText = sti.GetUpdateSQL();
                        m_command.ExecuteNonQuery();
                    }
                    else
                    {
                        m_command.CommandText = sti.GetInsertSQL();
                        m_command.ExecuteNonQuery();
                        m_command.CommandText = sti.GetMaximumIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = GoodReceiveNoteItem.GetByEventIDSQL(e.ID);
                OdbcDataReader r = m_command.ExecuteReader();
                IList luc = GoodReceiveNoteItem.TransformReaderList(r);
                r.Close();
                foreach (GoodReceiveNoteItem chk in luc)
                {
                    chk.UPDATED = e.EVENT_ITEMS.Contains(chk);
                }
                foreach (GoodReceiveNoteItem chk in luc)
                {
                    if (!chk.UPDATED)
                    {
                        m_command.CommandText = GoodReceiveNoteItem.DeleteSQL(chk.ID);
                        m_command.ExecuteNonQuery();
                    }
                }
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        protected override void doDelete(Event e)
        {
            GoodReceiveNote st = (GoodReceiveNote)e;//this.Get(e.ID);
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = GoodReceiveNoteItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = GoodReceiveNote.DeleteSQL(st.ID);
                m_command.ExecuteNonQuery();
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        private EventStatus getEventStatus(int id)
        {
            m_command.CommandText = GoodReceiveNote.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = GoodReceiveNote.GetByIDSQL(ID);
            OdbcDataReader r = m_command.ExecuteReader();
            GoodReceiveNote st = GoodReceiveNote.TransformReader(r);
            r.Close();
            m_command.CommandText = GoodReceiveNoteItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = GoodReceiveNoteItem.TransformReaderList(r);
            r.Close();
            foreach (GoodReceiveNoteItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                sti.PO_ITEM = PurchaseOrderRepository.FindPurchaseOrderItem(m_command, sti.PO_ITEM.ID);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(Event e, bool posted)
        {
            m_command.CommandText = GoodReceiveNote.GetUpdateStatusSQL(e.ID, posted);
            m_command.ExecuteNonQuery();
        }
        public static GoodReceiveNoteItem FindPOItem(OdbcCommand cmd, int PoIID)
        {
            cmd.CommandText = GoodReceiveNoteItem.FindByPOItemIDSQL(PoIID);
            OdbcDataReader r = cmd.ExecuteReader();
            GoodReceiveNoteItem res = GoodReceiveNoteItem.TransformReader(r);
            r.Close();
            return res;
        }
        public static void UpdateAgainstStatus(OdbcCommand cmd, GoodReceiveNote grn, GoodReceiveNoteItem grni)
        {
            cmd.CommandText = grni.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
            cmd.CommandText = grn.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
        }
        public static GoodReceiveNoteItem FindGoodReceiveNoteItem(OdbcCommand cmd, int grniID)
        {
            cmd.CommandText = GoodReceiveNoteItem.GetByIDSQL(grniID);
            OdbcDataReader r = cmd.ExecuteReader();
            GoodReceiveNoteItem result = GoodReceiveNoteItem.TransformReader(r);
            r.Close();
            result.EVENT = GoodReceiveNoteRepository.GetHeaderOnly(cmd, result.EVENT.ID);
            result.EVENT.EVENT_ITEMS.Add(result);
            return result;
        }
        public static GoodReceiveNote GetHeaderOnly(OdbcCommand cmd, int grnID)
        {
            cmd.CommandText = GoodReceiveNote.GetByIDSQL(grnID);
            OdbcDataReader r = cmd.ExecuteReader();
            GoodReceiveNote st = GoodReceiveNote.TransformReader(r);
            r.Close();
            return st;
        }

        protected override IList doSearch(string find)
        {
            throw new NotImplementedException();
        }
    }
}
