using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SalesReturnRepository : TransactionRepository
    {
        public SalesReturnRepository() : base() { }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (SalesReturnItem item in events.EVENT_ITEMS)
            {
                DeliveryOrder po = (DeliveryOrder)item.DO_ITEM.EVENT;
                assertConfirmedPO(po);
                assertValidDate(po, item.EVENT);
                SetStockCard(item, p);
                item.DO_ITEM.SetOSAgainstSRItem(item);
                DeliveryOrderRepository.UpdateAgainstStatus(m_command, po, item.DO_ITEM);
            }
        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (SalesReturnItem item in events.EVENT_ITEMS)
            {
                DeliveryOrder po = (DeliveryOrder)item.DO_ITEM.EVENT;
                SetStockCard(item, p);
                item.DO_ITEM.UnSetOSAgainstSRItem(item);
                DeliveryOrderRepository.UpdateAgainstStatus(m_command, po, item.DO_ITEM);
            }
        }
        private void assertConfirmedPO(Event p)
        {
            if (p.EVENT_STATUS.Equals(EventStatus.Entry))
                throw new Exception("SO not confirmed :" + p.CODE);
        }
        private void assertValidDate(Event po, Event grn)
        {
            if (grn.TRANSACTION_DATE < po.TRANSACTION_DATE)
                throw new Exception("SR Date can not less than DO Date :" + po.CODE + " [ " + po.TRANSACTION_DATE.ToString("dd-MMM-yyyy") + " ] ");
        }
        protected override void doSave(Event e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;
                SalesReturn stk = (SalesReturn)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = SalesReturn.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (SalesReturnItem item in stk.EVENT_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = SalesReturnItem.SelectMaxIDSQL();
                    item.ID = Convert.ToInt32(m_command.ExecuteScalar());
                }
                trc.Commit();
            }
            catch (Exception x)
            {
                e.ID = 0;
                foreach (EventItem item in e.EVENT_ITEMS)
                {
                    item.ID = 0;
                }
                trc.Rollback();
                throw x;
            }
        }
        protected override void doUpdate(Event en)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                SalesReturn e = (SalesReturn)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (SalesReturnItem sti in e.EVENT_ITEMS)
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
                        m_command.CommandText = SalesReturnItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = SalesReturnItem.DeleteUpdate(e.ID, e.EVENT_ITEMS);
                m_command.ExecuteNonQuery();
                //m_command.CommandText = SalesReturnItem.GetByEventIDSQL(e.ID);
                //MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                //IList luc = SalesReturnItem.TransformReaderList(r);
                //r.Close();
                //foreach (SalesReturnItem chk in luc)
                //{
                //    chk.UPDATED = e.EVENT_ITEMS.Contains(chk);
                //}
                //foreach (SalesReturnItem chk in luc)
                //{
                //    if (!chk.UPDATED)
                //    {
                //        m_command.CommandText = SalesReturnItem.DeleteSQL(chk.ID);
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
            SalesReturn st = (SalesReturn)e;//this.Get(e.ID);
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = SalesReturnItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = SalesReturn.DeleteSQL(st.ID);
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
            m_command.CommandText = SalesReturn.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = SalesReturn.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            SalesReturn st = SalesReturn.TransformReader(r);
            r.Close();
            m_command.CommandText = SalesReturnItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = SalesReturnItem.TransformReaderList(r);
            r.Close();
            foreach (SalesReturnItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                sti.DO_ITEM = DeliveryOrderRepository.FindDeliveryOrderItem(m_command, sti.DO_ITEM.ID);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(Event e, bool posted)
        {
            m_command.CommandText = SalesReturn.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        public static SalesReturnItem FindDOItem(MySql.Data.MySqlClient.MySqlCommand cmd, int grnIID)
        {
            cmd.CommandText = SalesReturnItem.FindByDOItemIDSQL(grnIID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            SalesReturnItem res = SalesReturnItem.TransformReader(r);
            r.Close();
            return res;
        }

        protected override IList doSearch(string find)
        {
            throw new NotImplementedException();
        }

        protected override bool doIsCodeExist(string code)
        {
            throw new NotImplementedException();
        }
    }
}
