using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PurchaseOrderRepository : TransactionRepository
    {
        public PurchaseOrderRepository() : base() { }

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
                assertUsedPOItemByGRNItem(item);
                SetStockCard(item, p);
            }
        }
        private void assertUsedPOItemByGRNItem(EventItem item)
        {
            GoodReceiveNoteItem grnItm = GoodReceiveNoteRepository.FindPOItem(m_command, item.ID);
            if (grnItm != null)
                throw new Exception("PO Item allready used by GRN item, delete GRN first : " + grnItm.EVENT.CODE);
        }
        protected override void doSave(Event e)
        {
            OdbcTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "PurchaseOrder");
                Event codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "PurchaseOrder", e.CODE, lastCode, lastDate, trDate, trCount == 0);
                PurchaseOrder stk = (PurchaseOrder)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = PurchaseOrder.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (PurchaseOrderItem item in stk.EVENT_ITEMS)
                {
                    item.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(m_command, item.PART.ID);
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = PurchaseOrderItem.SelectMaxIDSQL();
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
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                PurchaseOrder e = (PurchaseOrder)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (PurchaseOrderItem sti in e.EVENT_ITEMS)
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
                        m_command.CommandText = PurchaseOrderItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = PurchaseOrderItem.DeleteUpdate(e.ID, e.EVENT_ITEMS);
                m_command.ExecuteNonQuery();
                //m_command.CommandText = PurchaseOrderItem.GetByEventIDSQL(e.ID);
                //OdbcDataReader r = m_command.ExecuteReader();
                //IList luc = PurchaseOrderItem.TransformReaderList(r);
                //r.Close();
                //foreach (PurchaseOrderItem chk in luc)
                //{
                //    chk.UPDATED = e.EVENT_ITEMS.Contains(chk);
                //}
                //foreach (PurchaseOrderItem chk in luc)
                //{
                //    if (!chk.UPDATED)
                //    {
                //        m_command.CommandText = PurchaseOrderItem.DeleteSQL(chk.ID);
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
            PurchaseOrder st = (PurchaseOrder)e;//this.Get(e.ID);
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = PurchaseOrderItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = PurchaseOrder.DeleteSQL(st.ID);
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
            m_command.CommandText = PurchaseOrder.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = PurchaseOrder.GetByIDSQL(ID);
            OdbcDataReader r = m_command.ExecuteReader();
            PurchaseOrder st = PurchaseOrder.TransformReader(r);
            r.Close();
            m_command.CommandText = PurchaseOrderItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = PurchaseOrderItem.TransformReaderList(r);
            r.Close();
            foreach (PurchaseOrderItem sti in stis)
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
            m_command.CommandText = PurchaseOrder.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        public static void UpdateAgainstStatus(OdbcCommand cmd, PurchaseOrder po, PurchaseOrderItem poi)
        {
            cmd.CommandText = poi.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
            cmd.CommandText = po.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
        }
        public static PurchaseOrderItem FindPurchaseOrderItem(OdbcCommand cmd, int poiID)
        {
            cmd.CommandText = PurchaseOrderItem.GetByIDSQL(poiID);
            OdbcDataReader r = cmd.ExecuteReader();
            PurchaseOrderItem result = PurchaseOrderItem.TransformReader(r);
            r.Close();
            result.EVENT = PurchaseOrderRepository.GetHeaderOnly(cmd, result.EVENT.ID);
            result.EVENT.EVENT_ITEMS.Add(result);
            return result;
        }
        public static PurchaseOrder GetHeaderOnly(OdbcCommand cmd , int poID)
        {
            cmd.CommandText = PurchaseOrder.GetByIDSQL(poID);
            OdbcDataReader r = cmd.ExecuteReader();
            PurchaseOrder st = PurchaseOrder.TransformReader(r);
            r.Close();
            return st;
        }
        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = PurchaseOrder.GetSearch(find);
                OdbcDataReader r = m_command.ExecuteReader();
                IList rest = PurchaseOrder.TransformReaderList(r);
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
                m_command.CommandText = PurchaseOrder.SelectCountByCode(code);
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
            m_command.CommandText = PurchaseOrder.FindLastCodeAndTransactionDate(codesample);
            OdbcDataReader r = m_command.ExecuteReader();
            Event e = PurchaseOrder.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = PurchaseOrder.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "PurchaseOrder");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }
        public IList FindPObyPartAndPONo(string find, IList exceptPOI, int supplierID, DateTime trDate)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach(int i in exceptPOI)
            {
                poisSB.Append(i.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = exceptPOI.Count>0?pois.Substring(0, pois.Length - 1):"";

            m_command.CommandText = PurchaseOrderItem.GetSearchByPartAndPONo(find,supplierID, pois, trDate);
            OdbcDataReader r = m_command.ExecuteReader();
            IList result = PurchaseOrderItem.TransformReaderList(r);
            r.Close();
            foreach (PurchaseOrderItem t in result)
            {
                m_command.CommandText = PurchaseOrder.GetByIDSQL(t.EVENT.ID);
                r = m_command.ExecuteReader();
                t.EVENT = PurchaseOrder.TransformReader(r);
                r.Close();
                m_command.CommandText = Part.GetByIDSQLStatic(t.PART.ID);
                r = m_command.ExecuteReader();
                t.PART = Part.GetPart(r);
                r.Close();
                m_command.CommandText = Unit.GetByIDSQLstatic(t.UNIT.ID);
                r = m_command.ExecuteReader();
                t.UNIT = Unit.GetUnit(r);
                r.Close();
                m_command.CommandText = TermOfPayment.GetByIDSQLStatic(((PurchaseOrder)t.EVENT).TOP.ID);
                r = m_command.ExecuteReader();
                ((PurchaseOrder)t.EVENT).TOP = TermOfPayment.GetTOP(r);
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
        public double GetTheLatestPOPrice(int supID, int partID, int unitID)
        {
            m_command.CommandText = PurchaseOrderItem.GetTheLatestPOPrice(supID, partID, unitID);
            object r = m_command.ExecuteScalar();
            if (r == null) return 0d;
            double result = Convert.ToDouble(r);
            return result;
        }
        public PurchaseOrderItem FindPurchaseOrderItem(int poiID)
        {
            m_command.CommandText = PurchaseOrderItem.GetByIDSQL(poiID);
            OdbcDataReader r = m_command.ExecuteReader();
            PurchaseOrderItem result = PurchaseOrderItem.TransformReader(r);
            r.Close();
            result.EVENT = PurchaseOrderRepository.GetHeaderOnly(m_command, result.EVENT.ID);
            result.EVENT.EVENT_ITEMS.Add(result);
            return result;
        }
    }
}
