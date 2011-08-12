using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PurchaseReturnRepository : TransactionRepository
    {
        public PurchaseReturnRepository() : base() { }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (PurchaseReturnItem item in events.EVENT_ITEMS)
            {
                GoodReceiveNote po = (GoodReceiveNote)item.GRN_ITEM.EVENT;
                assertConfirmedPO(po);
                assertValidDate(po, item.EVENT);
                SetStockCard(item, p);
                item.GRN_ITEM.SetOSAgainstPRItem(item);
                GoodReceiveNoteRepository.UpdateAgainstStatus(m_command, po, item.GRN_ITEM);
            }
        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (PurchaseReturnItem item in events.EVENT_ITEMS)
            {
                GoodReceiveNote po = (GoodReceiveNote)item.GRN_ITEM.EVENT;
                SetStockCard(item, p);
                item.GRN_ITEM.UnSetOSAgainstPRItem(item);
                GoodReceiveNoteRepository.UpdateAgainstStatus(m_command, po, item.GRN_ITEM);
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
        protected override void doSave(Event e)
        {
            OdbcTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;

                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "PurchaseReturn");
                Event codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "PurchaseReturn", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                GoodReceiveNote stk = (GoodReceiveNote)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = GoodReceiveNote.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (PurchaseReturnItem item in stk.EVENT_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = PurchaseReturnItem.SelectMaxIDSQL();
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

                foreach (PurchaseReturnItem sti in e.EVENT_ITEMS)
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
                        m_command.CommandText = PurchaseReturnItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = PurchaseReturnItem.DeleteUpdate(e.ID, e.EVENT_ITEMS);
                m_command.ExecuteNonQuery();
                //m_command.CommandText = PurchaseReturnItem.GetByEventIDSQL(e.ID);
                //OdbcDataReader r = m_command.ExecuteReader();
                //IList luc = PurchaseReturnItem.TransformReaderList(r);
                //r.Close();
                //foreach (PurchaseReturnItem chk in luc)
                //{
                //    chk.UPDATED = e.EVENT_ITEMS.Contains(chk);
                //}
                //foreach (PurchaseReturnItem chk in luc)
                //{
                //    if (!chk.UPDATED)
                //    {
                //        m_command.CommandText = PurchaseReturnItem.DeleteSQL(chk.ID);
                //        m_command.ExecuteNonQuery();
                //    }
                //}
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
                m_command.CommandText = PurchaseReturnItem.DeleteAllByEventSQL(st.ID);
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
            m_command.CommandText = PurchaseReturnItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = PurchaseReturnItem.TransformReaderList(r);
            r.Close();
            foreach (PurchaseReturnItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                sti.GRN_ITEM = GoodReceiveNoteRepository.FindGoodReceiveNoteItem(m_command, sti.GRN_ITEM.ID);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(Event e, bool posted)
        {
            m_command.CommandText = GoodReceiveNote.GetUpdateStatusSQL(e.ID, posted);
            m_command.ExecuteNonQuery();
        }
        public static PurchaseReturnItem FindGRNItem(OdbcCommand cmd, int grnIID)
        {
            cmd.CommandText = PurchaseReturnItem.FindByGrnItemIDSQL(grnIID);
            OdbcDataReader r = cmd.ExecuteReader();
            PurchaseReturnItem res = PurchaseReturnItem.TransformReader(r);
            r.Close();
            return res;
        }

        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = PurchaseReturn.GetSearch(find);
                OdbcDataReader r = m_command.ExecuteReader();
                IList rest = PurchaseReturn.TransformReaderList(r);
                r.Close();
                return rest;
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        protected override bool doIsCodeExist(string code)
        {
            try
            {
                m_command.CommandText = PurchaseReturn.SelectCountByCode(code);
                int t = Convert.ToInt32(m_command.ExecuteScalar());
                return t > 0;
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public override Event FindLastCodeAndTransactionDate(string codesample)
        {
            m_command.CommandText = PurchaseReturn.FindLastCodeAndTransactionDate(codesample);
            OdbcDataReader r = m_command.ExecuteReader();
            Event e = PurchaseReturn.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = PurchaseReturn.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "PurchaseReturn");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }
    }
}
