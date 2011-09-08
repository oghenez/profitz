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
                assertInvoiceAlreadyGenerated(item);
                SalesOrder so = (SalesOrder)item.SO_ITEM.EVENT;
                SetStockCard(item, p);
                item.SO_ITEM.UnSetOSAgainstDOItem(item);
                SalesOrderRepository.UpdateAgainstStatus(m_command, so, item.SO_ITEM);
            }
        }
        private void assertInvoiceAlreadyGenerated(DeliveryOrderItem item)
        {
            m_command.CommandText = CustomerInvoiceItem.GetDOUseByCustomerInvoice(item.ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            IList invs = CustomerInvoiceItem.TransformReaderList(r);
            r.Close();
            foreach (CustomerInvoiceItem x in invs)
            {
                m_command.CommandText = CustomerInvoice.GetByIDSQL(x.EVENT.ID);
                r = m_command.ExecuteReader();
                x.EVENT = CustomerInvoice.TransformReader(r);
                r.Close();
            }
            if (invs.Count > 0)
                throw new Exception("DO Part [" + item.PART.CODE + "] is used by Customer Invoice [" + ((CustomerInvoiceItem)invs[0]).EVENT.CODE + "], please delete customer invoice first.");
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
            SalesReturnItem doItm = SalesReturnRepository.FindDOItem(m_command, item.ID);
            if (doItm != null)
                throw new Exception("DO Item allready used by SR item, delete SR first :" + doItm.EVENT.CODE);
        }
        protected override void doSave(Event e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;

                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "DeliveryOrder");
                Event codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "DeliveryOrder", e.CODE, lastCode, lastDate, trDate, trCount == 0);

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
        public static void UpdateAgainstStatus(MySql.Data.MySqlClient.MySqlCommand cmd, DeliveryOrder dor, DeliveryOrderItem doi)
        {
            cmd.CommandText = doi.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
            cmd.CommandText = dor.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
        }
        public static DeliveryOrderItem FindDeliveryOrderItem(MySql.Data.MySqlClient.MySqlCommand cmd, int doiID)
        {
            cmd.CommandText = DeliveryOrderItem.GetByIDSQL(doiID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            DeliveryOrderItem result = DeliveryOrderItem.TransformReader(r);
            r.Close();
            result.EVENT = DeliveryOrderRepository.GetHeaderOnly(cmd, result.EVENT.ID);
            result.EVENT.EVENT_ITEMS.Add(result);
            return result;
        }
        public static DeliveryOrder GetHeaderOnly(MySql.Data.MySqlClient.MySqlCommand cmd, int doID)
        {
            cmd.CommandText = DeliveryOrder.GetByIDSQL(doID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            DeliveryOrder st = DeliveryOrder.TransformReader(r);
            r.Close();
            return st;
        }

        //--------------------
        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = DeliveryOrder.GetSearch(find);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList rest = DeliveryOrder.TransformReaderList(r);
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
                m_command.CommandText = DeliveryOrder.SelectCountByCode(code);
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
            m_command.CommandText = DeliveryOrder.FindLastCodeAndTransactionDate(codesample);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            Event e = DeliveryOrder.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = DeliveryOrder.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "DeliveryOrder");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }
        public IList FindSObyPartAndDONo(string find, IList exceptDOI, int customerID, DateTime trDate)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (int i in exceptDOI)
            {
                poisSB.Append(i.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = exceptDOI.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";

            m_command.CommandText = DeliveryOrderItem.GetSearchByPartAndDONo(find, customerID, pois, trDate);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            IList result = DeliveryOrderItem.TransformReaderList(r);
            r.Close();
            foreach (DeliveryOrderItem t in result)
            {
                m_command.CommandText = DeliveryOrder.GetByIDSQL(t.EVENT.ID);
                r = m_command.ExecuteReader();
                t.EVENT = DeliveryOrder.TransformReader(r);
                r.Close();

                m_command.CommandText = Part.GetByIDSQLStatic(t.PART.ID);
                r = m_command.ExecuteReader();
                t.PART = Part.GetPart(r);
                r.Close();

                m_command.CommandText = Unit.GetByIDSQLstatic(t.UNIT.ID);
                r = m_command.ExecuteReader();
                t.UNIT = Unit.GetUnit(r);
                r.Close();

                m_command.CommandText = Warehouse.GetByIDSQLStatic(t.WAREHOUSE.ID);
                r = m_command.ExecuteReader();
                t.WAREHOUSE = Warehouse.GetWarehouse(r);
                r.Close();

                m_command.CommandText = Unit.GetByIDSQLstatic(t.PART.UNIT.ID);
                r = m_command.ExecuteReader();
                t.PART.UNIT = Unit.GetUnit(r);
                r.Close();
            }
            return result;
        }
        public IList FindDOItemlistByCustomerDate(string find, int supID, DateTime trdate, IList doIDS)
        {
            m_command.CommandText = CustomerInvoiceItem.GetDOUseByCustomerInvoice();
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            if (r.HasRows)
            {
                while (r.Read())
                {
                    int id = Convert.ToInt32(r[0]);
                    if (!doIDS.Contains(id))
                        doIDS.Add(id);
                }
            }
            r.Close();
            StringBuilder poisSB = new StringBuilder();
            foreach (int i in doIDS)
            {
                poisSB.Append(i.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = doIDS.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            if (find == "")
                m_command.CommandText = DeliveryOrderItem.GetDOItemByCusDate(supID, trdate, pois);
            else
                m_command.CommandText = DeliveryOrderItem.GetSearchByPartAndDONo(find, supID, pois, trdate);
            r = m_command.ExecuteReader();
            IList result = DeliveryOrderItem.TransformReaderList(r);
            r.Close();
            foreach (DeliveryOrderItem t in result)
            {
                m_command.CommandText = DeliveryOrder.GetByIDSQL(t.EVENT.ID);
                r = m_command.ExecuteReader();
                t.EVENT = DeliveryOrder.TransformReader(r);
                r.Close();

                m_command.CommandText = Part.GetByIDSQLStatic(t.PART.ID);
                r = m_command.ExecuteReader();
                t.PART = Part.GetPart(r);
                r.Close();

                m_command.CommandText = Unit.GetByIDSQLstatic(t.UNIT.ID);
                r = m_command.ExecuteReader();
                t.UNIT = Unit.GetUnit(r);
                r.Close();

                m_command.CommandText = Warehouse.GetByIDSQLStatic(t.WAREHOUSE.ID);
                r = m_command.ExecuteReader();
                t.WAREHOUSE = Warehouse.GetWarehouse(r);
                r.Close();

                m_command.CommandText = Unit.GetByIDSQLstatic(t.PART.UNIT.ID);
                r = m_command.ExecuteReader();
                t.PART.UNIT = Unit.GetUnit(r);
                r.Close();
            }
            return result;
        }
        public double GetOutstandingReturned(int doItem)
        {
            m_command.CommandText = DeliveryOrderItem.GetOutstandingReturnSQL(doItem);
            double result = Convert.ToDouble(m_command.ExecuteScalar());
            return result;
        }
        public double GetReturned(int doItem)
        {
            m_command.CommandText = DeliveryOrderItem.GetReturnSQL(doItem);
            double result = Convert.ToDouble(m_command.ExecuteScalar());
            return result;
        }
    }
}
