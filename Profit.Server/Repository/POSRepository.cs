using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class POSRepository : TransactionRepository
    {
        CustomerInvoiceJournalRepository r_sij;
        ReceiptRepository r_receipt;

        public POSRepository()
            : base() 
        {
            r_sij = new CustomerInvoiceJournalRepository(m_command);
            r_receipt = new ReceiptRepository(m_command);
        }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (EventItem item in events.EVENT_ITEMS)
            {
                SetStockCard(item, p);
            }
            POS si = (POS)events;
            
            CustomerInvoiceJournal sij = new CustomerInvoiceJournal();
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
            sij.VENDOR = si.CUSTOMER;
            sij.POS_INVOICE = si;
            sij.NET_AMOUNT = si.NET_TOTAL;
            sij.EMPLOYEE = si.EMPLOYEE;
            sij.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerInvoice;

            CustomerInvoiceJournalItem siji = new CustomerInvoiceJournalItem();
            siji.AMOUNT = si.NET_TOTAL;
            siji.CURRENCY = si.CURRENCY;
            siji.EVENT_JOURNAL = sij;
            siji.VENDOR = si.CUSTOMER;
            siji.INVOICE_NO = si.CODE;
            siji.INVOICE_DATE = si.TRANSACTION_DATE;
            siji.TOP = si.TOP;
            siji.EMPLOYEE = si.EMPLOYEE;
            siji.DUE_DATE = si.DUE_DATE;
            siji.OUTSTANDING_AMOUNT = si.NET_TOTAL;
            siji.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerInvoice;

            sij.EVENT_JOURNAL_ITEMS.Add(siji);
            r_sij.SaveNoTransaction(sij);
            r_sij.ConfirmNoTransaction(sij.ID);

            Receipt py = new Receipt();
            py.CURRENCY = sij.CURRENCY;
            py.NET_AMOUNT = sij.NET_AMOUNT;
            py.NOTES = "Auto generate from POS Transaction";
            py.NOTICE_DATE = sij.NOTICE_DATE;
            py.OTHER_EXPENSE = sij.OTHER_EXPENSE;
            py.SUBTOTAL_AMOUNT = sij.SUBTOTAL_AMOUNT;
            py.TRANSACTION_DATE = sij.TRANSACTION_DATE;
            py.VENDOR = sij.VENDOR;
            py.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Receipt;
            py.EMPLOYEE = sij.EMPLOYEE;

            ReceiptItem pyi = new ReceiptItem();
            pyi.AMOUNT = siji.AMOUNT;
            pyi.CURRENCY = py.CURRENCY;
            pyi.DUE_DATE = py.TRANSACTION_DATE;
            pyi.EMPLOYEE = py.EMPLOYEE;
            pyi.EVENT_JOURNAL = sij;
            pyi.INVOICE_DATE = sij.TRANSACTION_DATE;
            pyi.INVOICE_NO = sij.CODE;
            pyi.NOTES = "Autogenerate Payment from POS transaction";
            pyi.PAYMENT_TYPE = ReceiptType.Cash;
            pyi.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE = VendorBalanceEntryType.Receipt;
            pyi.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Receipt;
            pyi.VENDOR_BALANCE_TYPE = VendorBalanceType.Customer;
            pyi.CUSTOMER_INVOICE_JOURNAL_ITEM = siji;
            pyi.VENDOR = sij.VENDOR;
            py.EVENT_JOURNAL_ITEMS.Add(pyi);

            r_receipt.SaveNoTransaction(sij);
            r_receipt.ConfirmNoTransaction(sij.ID);

        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (EventItem item in events.EVENT_ITEMS)
            {
                SetStockCard(item, p);
            }
            CustomerInvoiceJournal sij = (CustomerInvoiceJournal)r_sij.FindPeriodCIJId(events.ID);
            if (sij == null) throw new Exception("Customer Invoice Journal is missing");

            ReceiptItem rec = r_receipt.FindReceiptItemByCIJ(sij.ID);
            if (rec == null) throw new Exception("Receipt is missing");

            r_receipt.ReviseNoTransaction(rec.EVENT_JOURNAL.ID);
            r_receipt.DeleteNoTransaction(sij);

            r_sij.ReviseNoTransaction(sij.ID);
            r_sij.DeleteNoTransaction(sij);


        }

        protected override void doSave(Event e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "POS");
                Event codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "POS", e.CODE, lastCode, lastDate, trDate, trCount == 0);
                POS stk = (POS)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = POS.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (POSItem item in stk.EVENT_ITEMS)
                {
                    item.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(m_command, item.PART.ID);
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = POSItem.SelectMaxIDSQL();
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
                POS e = (POS)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (POSItem sti in e.EVENT_ITEMS)
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
                        m_command.CommandText = POSItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = POSItem.DeleteUpdate(e.ID, e.EVENT_ITEMS);
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
            POS st = (POS)e;
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = POSItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = POS.DeleteSQL(st.ID);
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
            m_command.CommandText = POS.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = POS.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            POS st = POS.TransformReader(r);
            r.Close();
            m_command.CommandText = POSItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = POSItem.TransformReaderList(r);
            r.Close();
            foreach (POSItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                if(sti.DO_ITEM.ID>0)
                    sti.DO_ITEM = DeliveryOrderRepository.FindDeliveryOrderItem(m_command, sti.DO_ITEM.ID);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(Event e, bool posted)
        {
            m_command.CommandText = POS.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }

        public static POSItem FindPOSItem(MySql.Data.MySqlClient.MySqlCommand cmd, int poiID)
        {
            cmd.CommandText = POSItem.GetByIDSQL(poiID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            POSItem result = POSItem.TransformReader(r);
            r.Close();
            result.EVENT = SalesOrderRepository.GetHeaderOnly(cmd, result.EVENT.ID);
            result.EVENT.EVENT_ITEMS.Add(result);
            return result;
        }
        public static POS GetHeaderOnly(MySql.Data.MySqlClient.MySqlCommand cmd , int poID)
        {
            cmd.CommandText = POS.GetByIDSQL(poID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            POS st = POS.TransformReader(r);
            r.Close();
            return st;
        }
        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = POS.GetSearch(find);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList rest = POS.TransformReaderList(r);
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
                m_command.CommandText = POS.SelectCountByCode(code);
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
            m_command.CommandText = POS.FindLastCodeAndTransactionDate(codesample);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            Event e = POS.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = POS.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "POS");
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

            m_command.CommandText = POSItem.GetSearchByPartAndPONo(find,supplierID, pois, trDate);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            IList result = POSItem.TransformReaderList(r);
            r.Close();
            foreach (POSItem t in result)
            {
                m_command.CommandText = POS.GetByIDSQL(t.EVENT.ID);
                r = m_command.ExecuteReader();
                t.EVENT = POS.TransformReader(r);
                r.Close();
                m_command.CommandText = Part.GetByIDSQLStatic(t.PART.ID);
                r = m_command.ExecuteReader();
                t.PART = Part.GetPart(r);
                r.Close();
                m_command.CommandText = Unit.GetByIDSQLstatic(t.UNIT.ID);
                r = m_command.ExecuteReader();
                t.UNIT = Unit.GetUnit(r);
                r.Close();
                m_command.CommandText = TermOfPayment.GetByIDSQLStatic(((POS)t.EVENT).TOP.ID);
                r = m_command.ExecuteReader();
                ((POS)t.EVENT).TOP = TermOfPayment.GetTOP(r);
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
            m_command.CommandText = POSItem.GetTheLatestSOPrice(supID, partID, unitID);
            object r = m_command.ExecuteScalar();
            if (r == null) return 0d;
            double result = Convert.ToDouble(r);
            return result;
        }

    }
}
