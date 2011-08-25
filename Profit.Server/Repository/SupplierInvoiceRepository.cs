using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SupplierInvoiceRepository : TransactionRepository
    {
        SupplierInvoiceJournalRepository r_sij;
        public SupplierInvoiceRepository() : base() 
        {
            r_sij = new SupplierInvoiceJournalRepository(m_command);
        }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (EventItem item in events.EVENT_ITEMS)
            {
                SetStockCard(item, p);
            }
            SupplierInvoice si = (SupplierInvoice)events;
            SupplierInvoiceJournal sij = new SupplierInvoiceJournal();
           // sij.LastUpdate = DateTime.Now;
           // sij.ByTransaction = true;
            sij.CODE = si.CODE;
            //sij.ComputerName = si.ComputerName;
            sij.CURRENCY = si.CURRENCY;
            sij.EVENT_STATUS = EventStatus.Entry;
            sij.NOTES = si.NOTES;
            sij.NOTICE_DATE = si.NOTICE_DATE;
            sij.TRANSACTION_DATE = si.TRANSACTION_DATE;
           // sij.UserName = si.UserName;
            sij.VENDOR = si.SUPPLIER;
            sij.SUPPLIER_INVOICE = si;
            sij.NET_AMOUNT = si.NET_TOTAL;
            sij.EMPLOYEE = si.EMPLOYEE;

            SupplierInvoiceJournalItem siji = new SupplierInvoiceJournalItem();
            siji.AMOUNT = si.NET_TOTAL;
            siji.CURRENCY = si.CURRENCY;
            siji.EVENT_JOURNAL = sij;
            siji.VENDOR = si.SUPPLIER;
            siji.INVOICE_NO = si.CODE;
            siji.INVOICE_DATE = si.TRANSACTION_DATE;
            siji.TOP = si.TOP;
            siji.EMPLOYEE = si.EMPLOYEE;
            siji.DUE_DATE = si.DUE_DATE;
            siji.OUTSTANDING_AMOUNT = si.NET_TOTAL;

            sij.EVENT_JOURNAL_ITEMS.Add(siji);
            r_sij.SaveNoTransaction(sij);
            r_sij.ConfirmNoTransaction(sij.ID);
        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (EventItem item in events.EVENT_ITEMS)
            {
                SetStockCard(item, p);
            }
            SupplierInvoiceJournal sij = (SupplierInvoiceJournal)r_sij.FindPeriodSIJId(events.ID);
            if (sij == null) throw new Exception("Supplier Invoice Journal is missing");
            r_sij.ReviseNoTransaction(sij.ID);
            r_sij.DeleteNoTransaction(sij);
        }

        protected override void doSave(Event e)
        {
            OdbcTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "SupplierInvoice");
                Event codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "SupplierInvoice", e.CODE, lastCode, lastDate, trDate, trCount == 0);
                SupplierInvoice stk = (SupplierInvoice)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = SupplierInvoice.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (SupplierInvoiceItem item in stk.EVENT_ITEMS)
                {
                    item.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(m_command, item.PART.ID);
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = SupplierInvoiceItem.SelectMaxIDSQL();
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
                SupplierInvoice e = (SupplierInvoice)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (SupplierInvoiceItem sti in e.EVENT_ITEMS)
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
                        m_command.CommandText = SupplierInvoiceItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = SupplierInvoiceItem.DeleteUpdate(e.ID, e.EVENT_ITEMS);
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
            SupplierInvoice st = (SupplierInvoice)e;
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = SupplierInvoiceItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = SupplierInvoice.DeleteSQL(st.ID);
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
            m_command.CommandText = SupplierInvoice.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = SupplierInvoice.GetByIDSQL(ID);
            OdbcDataReader r = m_command.ExecuteReader();
            SupplierInvoice st = SupplierInvoice.TransformReader(r);
            r.Close();
            m_command.CommandText = SupplierInvoiceItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = SupplierInvoiceItem.TransformReaderList(r);
            r.Close();
            foreach (SupplierInvoiceItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                if(sti.GRN_ITEM.ID>0)
                    sti.GRN_ITEM = GoodReceiveNoteRepository.FindGoodReceiveNoteItem(m_command, sti.GRN_ITEM.ID);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(Event e, bool posted)
        {
            m_command.CommandText = SupplierInvoice.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }

        public static SupplierInvoiceItem FindSupplierInvoiceItem(OdbcCommand cmd, int poiID)
        {
            cmd.CommandText = SupplierInvoiceItem.GetByIDSQL(poiID);
            OdbcDataReader r = cmd.ExecuteReader();
            SupplierInvoiceItem result = SupplierInvoiceItem.TransformReader(r);
            r.Close();
            result.EVENT = PurchaseOrderRepository.GetHeaderOnly(cmd, result.EVENT.ID);
            result.EVENT.EVENT_ITEMS.Add(result);
            return result;
        }
        public static SupplierInvoice GetHeaderOnly(OdbcCommand cmd , int poID)
        {
            cmd.CommandText = SupplierInvoice.GetByIDSQL(poID);
            OdbcDataReader r = cmd.ExecuteReader();
            SupplierInvoice st = SupplierInvoice.TransformReader(r);
            r.Close();
            return st;
        }
        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = SupplierInvoice.GetSearch(find);
                OdbcDataReader r = m_command.ExecuteReader();
                IList rest = SupplierInvoice.TransformReaderList(r);
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
                m_command.CommandText = SupplierInvoice.SelectCountByCode(code);
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
            m_command.CommandText = SupplierInvoice.FindLastCodeAndTransactionDate(codesample);
            OdbcDataReader r = m_command.ExecuteReader();
            Event e = SupplierInvoice.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = SupplierInvoice.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "SupplierInvoice");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }
        public IList FindSIbyPartAndPONo(string find, IList exceptPOI, int supplierID, DateTime trDate)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach(int i in exceptPOI)
            {
                poisSB.Append(i.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = exceptPOI.Count>0?pois.Substring(0, pois.Length - 1):"";

            m_command.CommandText = SupplierInvoiceItem.GetSearchByPartAndPONo(find,supplierID, pois, trDate);
            OdbcDataReader r = m_command.ExecuteReader();
            IList result = SupplierInvoiceItem.TransformReaderList(r);
            r.Close();
            foreach (SupplierInvoiceItem t in result)
            {
                m_command.CommandText = SupplierInvoice.GetByIDSQL(t.EVENT.ID);
                r = m_command.ExecuteReader();
                t.EVENT = SupplierInvoice.TransformReader(r);
                r.Close();
                m_command.CommandText = Part.GetByIDSQLStatic(t.PART.ID);
                r = m_command.ExecuteReader();
                t.PART = Part.GetPart(r);
                r.Close();
                m_command.CommandText = Unit.GetByIDSQLstatic(t.UNIT.ID);
                r = m_command.ExecuteReader();
                t.UNIT = Unit.GetUnit(r);
                r.Close();
                m_command.CommandText = TermOfPayment.GetByIDSQLStatic(((SupplierInvoice)t.EVENT).TOP.ID);
                r = m_command.ExecuteReader();
                ((SupplierInvoice)t.EVENT).TOP = TermOfPayment.GetTOP(r);
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
        public double GetTheLatestSIPrice(int supID, int partID, int unitID)
        {
            m_command.CommandText = SupplierInvoiceItem.GetTheLatestPOPrice(supID, partID, unitID);
            object r = m_command.ExecuteScalar();
            if (r == null) return 0d;
            double result = Convert.ToDouble(r);
            return result;
        }

    }
}
