using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class DeliveryOrderRepository : TransactionRepository
    {
        public DeliveryOrderRepository() : base() { }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (DeliveryOrderItem item in events.EVENT_ITEMS)
            {
                SalesOrder so = (SalesOrder)item.SO_ITEM.EVENT;
                assertConfirmedSO(so);
                assertValidDate(so, item.EVENT);
                SetStockCard(item, p);
                item.SO_ITEM.SetOSAgainstDOItem(item);
                SalesOrderRepository.UpdateAgainstStatus(m_command, so, item.SO_ITEM);
            }
        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (DeliveryOrderItem item in events.EVENT_ITEMS)
            {
                assertUsedDOItemBySRItem(item);
                SalesOrder so = (SalesOrder)item.SO_ITEM.EVENT;
                SetStockCard(item, p);
                item.SO_ITEM.UnSetOSAgainstDOItem(item);
                SalesOrderRepository.UpdateAgainstStatus(m_command, so, item.SO_ITEM);
            }
        }
        private void assertConfirmedSO(Event p)
        {
            if (p.EVENT_STATUS.Equals(EventStatus.Entry))
                throw new Exception("SO not confirmed :" + p.CODE);
        }
        private void assertValidDate(Event so, Event del)
        {
            if (del.TRANSACTION_DATE < so.TRANSACTION_DATE)
                throw new Exception("DO Date can not less than SO Date :" + so.CODE + " [ " + so.TRANSACTION_DATE.ToString("dd-MMM-yyyy") + " ] ");
        }
        private void assertUsedDOItemBySRItem(EventItem item)
        {
            SalesReturnItem doItm = SalesReturnRepository.FindGRNItem(m_command, item.ID);
            if (doItm != null)
                throw new Exception("DO Item allready used by SR item, delete SR first :" + doItm.EVENT.CODE);
        }
        protected override void doSave(Event e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;
                DeliveryOrder stk = (DeliveryOrder)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = DeliveryOrder.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (DeliveryOrderItem item in stk.EVENT_ITEMS)
                {
                    item.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(m_command, item.PART.ID);
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = DeliveryOrderItem.SelectMaxIDSQL();
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
                DeliveryOrder e = (DeliveryOrder)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (DeliveryOrderItem sti in e.EVENT_ITEMS)
                {
                    sti.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(m_command, sti.PART.ID);
                    if (sti.ID > 0)
                    {
                        m_command.CommandText = sti.GetUpdateSQL();
                        m_command.ExecuteNonQuery();
                    }
                    else
                    {
                        m_command.CommandText = sti.GetInsertSQL();
                        m_command.ExecuteNonQuery();
                        m_command.CommandText = DeliveryOrderItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = DeliveryOrderItem.DeleteUpdate(e.ID,e.EVENT_ITEMS);
                m_command.ExecuteNonQuery();
                //m_command.CommandText = DeliveryOrderItem.GetByEventIDSQL(e.ID);
                //MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                //IList luc = DeliveryOrderItem.TransformReaderList(r);
                //r.Close();
                //foreach (DeliveryOrderItem chk in luc)
                //{
                //    chk.UPDATED = e.EVENT_ITEMS.Contains(chk);
                //}
                //foreach (DeliveryOrderItem chk in luc)
                //{
                //    if (!chk.UPDATED)
                //    {
                //        m_command.CommandText = DeliveryOrderItem.DeleteSQL(chk.ID);
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
            DeliveryOrder st = (DeliveryOrder)e;//this.Get(e.ID);
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = DeliveryOrderItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = DeliveryOrder.DeleteSQL(st.ID);
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
            m_command.CommandText = DeliveryOrder.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = DeliveryOrder.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            DeliveryOrder st = DeliveryOrder.TransformReader(r);
            r.Close();
            m_command.CommandText = DeliveryOrderItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = DeliveryOrderItem.TransformReaderList(r);
            r.Close();
            foreach (DeliveryOrderItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                sti.SO_ITEM = SalesOrderRepository.FindSalesOrderItem(m_command, sti.SO_ITEM.ID);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(Event e, bool posted)
        {
            m_command.CommandText = DeliveryOrder.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        public static DeliveryOrderItem FindDOItem(MySql.Data.MySqlClient.MySqlCommand cmd, int PoIID)
        {
            cmd.CommandText = DeliveryOrderItem.FindBySOItemIDSQL(PoIID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            DeliveryOrderItem res = DeliveryOrderItem.TransformReader(r);
            r.Close();
            return res;
        }
        public static void UpdateAgainstStatus(MySql.Data.MySqlClient.MySqlCommand cmd, DeliveryOrder grn, DeliveryOrderItem grni)
        {
            cmd.CommandText = grni.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
            cmd.CommandText = grn.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
        }
        public static DeliveryOrderItem FindDeliveryOrderItem(MySql.Data.MySqlClient.MySqlCommand cmd, int grniID)
        {
            cmd.CommandText = DeliveryOrderItem.GetByIDSQL(grniID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            DeliveryOrderItem result = DeliveryOrderItem.TransformReader(r);
            r.Close();
            result.EVENT = DeliveryOrderRepository.GetHeaderOnly(cmd, result.EVENT.ID);
            result.EVENT.EVENT_ITEMS.Add(result);
            return result;
        }
        public static DeliveryOrder GetHeaderOnly(MySql.Data.MySqlClient.MySqlCommand cmd, int grnID)
        {
            cmd.CommandText = DeliveryOrder.GetByIDSQL(grnID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            DeliveryOrder st = DeliveryOrder.TransformReader(r);
            r.Close();
            return st;
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
