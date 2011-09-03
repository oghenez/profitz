using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SalesOrderRepository : TransactionRepository
    {
        public SalesOrderRepository() : base() { }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (EventItem item in events.EVENT_ITEMS)
            {
                SetStockCard(item, p);
            }
        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (EventItem item in events.EVENT_ITEMS)
            {
                assertUsedSOItemByDOItem(item);
                SetStockCard(item, p);
            }
        }
        private void assertUsedSOItemByDOItem(EventItem item)
        {
            DeliveryOrderItem grnItm = DeliveryOrderRepository.FindDOItem(m_command, item.ID);
            if (grnItm != null)
                throw new Exception("SO Item allready used by DO item, delete GRN first : " + grnItm.EVENT.CODE);
        }
        protected override void doSave(Event e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;
                SalesOrder stk = (SalesOrder)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = SalesOrder.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (SalesOrderItem item in stk.EVENT_ITEMS)
                {
                    item.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(m_command, item.PART.ID);
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = SalesOrderItem.SelectMaxIDSQL();
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
                SalesOrder e = (SalesOrder)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (SalesOrderItem sti in e.EVENT_ITEMS)
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
                        m_command.CommandText = SalesOrderItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = SalesOrderItem.DeleteUpdate(e.ID, e.EVENT_ITEMS);
                m_command.ExecuteNonQuery();
                //m_command.CommandText = SalesOrderItem.GetByEventIDSQL(e.ID);
                //MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                //IList luc = SalesOrderItem.TransformReaderList(r);
                //r.Close();
                //foreach (SalesOrderItem chk in luc)
                //{
                //    chk.UPDATED = e.EVENT_ITEMS.Contains(chk);
                //}
                //foreach (SalesOrderItem chk in luc)
                //{
                //    if (!chk.UPDATED)
                //    {
                //        m_command.CommandText = SalesOrderItem.DeleteSQL(chk.ID);
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
            SalesOrder st = (SalesOrder)e;//this.Get(e.ID);
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = SalesOrderItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = SalesOrder.DeleteSQL(st.ID);
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
            m_command.CommandText = SalesOrder.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = SalesOrder.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            SalesOrder st = SalesOrder.TransformReader(r);
            r.Close();
            m_command.CommandText = SalesOrderItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = SalesOrderItem.TransformReaderList(r);
            r.Close();
            foreach (SalesOrderItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(Event e, bool posted)
        {
            m_command.CommandText = SalesOrder.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        public static void UpdateAgainstStatus(MySql.Data.MySqlClient.MySqlCommand cmd, SalesOrder po, SalesOrderItem poi)
        {
            cmd.CommandText = poi.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
            cmd.CommandText = po.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
        }
        public static SalesOrderItem FindSalesOrderItem(MySql.Data.MySqlClient.MySqlCommand cmd, int poiID)
        {
            cmd.CommandText = SalesOrderItem.GetByIDSQL(poiID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            SalesOrderItem result = SalesOrderItem.TransformReader(r);
            r.Close();
            //cmd.CommandText = SalesOrder.GetByIDSQL(result.ID);
            //r = cmd.ExecuteReader();
            result.EVENT = SalesOrderRepository.GetHeaderOnly(cmd, result.EVENT.ID);
            result.EVENT.EVENT_ITEMS.Add(result);
            //r.Close();
            return result;
        }
        public static SalesOrder GetHeaderOnly(MySql.Data.MySqlClient.MySqlCommand cmd , int poID)
        {
            cmd.CommandText = SalesOrder.GetByIDSQL(poID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            SalesOrder st = SalesOrder.TransformReader(r);
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
