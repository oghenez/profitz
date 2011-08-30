using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PaymentRepository : JournalRepository
    {
        public PaymentRepository() : base() { }

        protected override void doConfirm(EventJournal events, Period p)
        {
            foreach (PaymentItem item in events.EVENT_JOURNAL_ITEMS)
            {
                assertConfirmedSIJ(item.SUPPLIER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL);
                assertValidDate(item.SUPPLIER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL, events);
                item.CURRENCY = events.CURRENCY;
                item.VENDOR = events.VENDOR;
                SetVendorBalance(item, p);
                item.ProcessPosted();
                updateVendorBalances(item.VENDOR_BALANCE);
                saveVendorBalanceEntry(item.VENDOR_BALANCE_ENTRY);
                item.SUPPLIER_INVOICE_JOURNAL_ITEM.SetOSAgainstPaymentItem(item);
                if (item.SUPPLIER_INVOICE_JOURNAL_ITEM is SupplierInvoiceJournalItem)
                {
                    SupplierInvoiceJournalRepository.UpdateAgainstStatus(m_command, item.SUPPLIER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                        item.SUPPLIER_INVOICE_JOURNAL_ITEM);
                }
                if (item.SUPPLIER_INVOICE_JOURNAL_ITEM is SupplierOutStandingInvoiceItem)
                {
                    SupplierOutStandingInvoiceRepository.UpdateAgainstStatus(m_command, item.SUPPLIER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                        item.SUPPLIER_INVOICE_JOURNAL_ITEM);
                }
            }
        }
        protected override void doRevise(EventJournal events, Period p)
        {
            foreach (PaymentItem item in events.EVENT_JOURNAL_ITEMS)
            {
                item.CURRENCY = events.CURRENCY;
                item.VENDOR = events.VENDOR;
                SetVendorBalance(item, p);
                item.ProcessUnPosted();
                updateVendorBalances(item.VENDOR_BALANCE);
                deleteVendorBalanceEntry(item.VENDOR_BALANCE_ENTRY);
                item.SUPPLIER_INVOICE_JOURNAL_ITEM.UnSetOSAgainstPaymentItem(item);
                if (item.SUPPLIER_INVOICE_JOURNAL_ITEM is SupplierInvoiceJournalItem)
                {
                    SupplierInvoiceJournalRepository.UpdateAgainstStatus(m_command, item.SUPPLIER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                        item.SUPPLIER_INVOICE_JOURNAL_ITEM);
                }
                if (item.SUPPLIER_INVOICE_JOURNAL_ITEM is SupplierOutStandingInvoiceItem)
                {
                    SupplierOutStandingInvoiceRepository.UpdateAgainstStatus(m_command, item.SUPPLIER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                        item.SUPPLIER_INVOICE_JOURNAL_ITEM);
                }
                if (item.PAYMENT_TYPE == PaymentType.APDebitNote)
                {
        
                }
            }
        }
        private void assertConfirmedSIJ(EventJournal p)
        {
            if (p.EVENT_STATUS.Equals(EventStatus.Entry))
                throw new Exception("SIJ not confirmed :" + p.CODE);
        }
        private void assertValidDate(EventJournal sij, EventJournal py)
        {
            if (py.TRANSACTION_DATE < sij.TRANSACTION_DATE)
                throw new Exception("Payment Date can not less than Invoice Date :" + sij.CODE + " [ " + sij.TRANSACTION_DATE.ToString("dd-MMM-yyyy") + " ] ");
        }

        protected override void doSave(EventJournal e)
        {
            OdbcTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;

                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "Payment");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "Payment", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                Payment stk = (Payment)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = Payment.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (PaymentItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    item.VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE = item.SUPPLIER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL.VENDOR_BALANCE_ENTRY_TYPE;
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = PaymentItem.SelectMaxIDSQL();
                    item.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    if (item.PAYMENT_TYPE == PaymentType.APDebitNote)
                    {
                        m_command.CommandText = APDebitNote.UpdateUsedForPayment(item.AP_DEBIT_NOTE.ID, true);
                        m_command.ExecuteNonQuery();
                    }
                }
                trc.Commit();
            }
            catch (Exception x)
            {
                e.ID = 0;
                foreach (EventJournalItem item in e.EVENT_JOURNAL_ITEMS)
                {
                    item.ID = 0;
                }
                trc.Rollback();
                throw x;
            }
        }
        protected override void doUpdate(EventJournal en)
        {
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                Payment e = (Payment)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (PaymentItem sti in e.EVENT_JOURNAL_ITEMS)
                {
                    sti.VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE = sti.SUPPLIER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL.VENDOR_BALANCE_ENTRY_TYPE;
                    if (sti.ID > 0)
                    {
                        m_command.CommandText = sti.GetUpdateSQL();
                        m_command.ExecuteNonQuery();
                    }
                    else
                    {
                        m_command.CommandText = sti.GetInsertSQL();
                        m_command.ExecuteNonQuery();
                        m_command.CommandText = PaymentItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                    if (sti.PAYMENT_TYPE == PaymentType.APDebitNote)
                    {
                        m_command.CommandText = APDebitNote.UpdateUsedForPayment(sti.AP_DEBIT_NOTE.ID, true);
                        m_command.ExecuteNonQuery();
                    }
                }
                m_command.CommandText = PaymentItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
                m_command.ExecuteNonQuery();
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        protected override void doDelete(EventJournal e)
        {
            Payment st = (Payment)e;
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                foreach (PaymentItem sti in e.EVENT_JOURNAL_ITEMS)
                {
                    if (sti.PAYMENT_TYPE == PaymentType.APDebitNote)
                    {
                        m_command.CommandText = APDebitNote.UpdateUsedForPayment(sti.AP_DEBIT_NOTE.ID, false);
                        m_command.ExecuteNonQuery();
                    }
                }
                m_command.CommandText = PaymentItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = Payment.DeleteSQL(st.ID);
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
            m_command.CommandText = Payment.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override EventJournal doGet(int ID)
        {
            m_command.CommandText = Payment.GetByIDSQL(ID);
            OdbcDataReader r = m_command.ExecuteReader();
            Payment st = Payment.TransformReader(r);
            r.Close();
            m_command.CommandText = PaymentItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = PaymentItem.TransformReaderList(r);
            r.Close();
            foreach (PaymentItem sti in stis)
            {
                sti.EVENT_JOURNAL = st;
                //sti.VENDOR = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.VENDOR_BALANCE_ENTRY = VendorBalanceEntryRepository.FindVendorBalanceEntryByEventItem(m_command, sti.ID, sti.VENDOR_BALANCE_ENTRY_TYPE);
                //sti.GRN_ITEM = GoodReceiveNoteRepository.FindGoodReceiveNoteItem(m_command, sti.GRN_ITEM.ID);
                //sti.GRN_ITEM.PART = PartRepository.GetByID(m_command, sti.GRN_ITEM.PART.ID);
                if (sti.VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE == VendorBalanceEntryType.SupplierInvoice)
                {
                    if(sti.SUPPLIER_INVOICE_JOURNAL_ITEM!=null)
                        sti.SUPPLIER_INVOICE_JOURNAL_ITEM = SupplierInvoiceJournalRepository.FindSIJournalItemlistForPayment(m_command, sti.SUPPLIER_INVOICE_JOURNAL_ITEM.GetID());
                }
                if (sti.VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE == VendorBalanceEntryType.SupplierOutStandingInvoice)
                {
                    if (sti.SUPPLIER_INVOICE_JOURNAL_ITEM != null)
                        sti.SUPPLIER_INVOICE_JOURNAL_ITEM = SupplierOutStandingInvoiceRepository.FindSOIItemlistForPayment(m_command, sti.SUPPLIER_INVOICE_JOURNAL_ITEM.GetID());
                }
                if (sti.PAYMENT_TYPE == PaymentType.Bank)
                    sti.BANK = getBank(sti.BANK.ID);
                if (sti.PAYMENT_TYPE == PaymentType.APDebitNote)
                    sti.AP_DEBIT_NOTE = APDebitNoteRepository.FindAPDNForPayment(m_command, sti.AP_DEBIT_NOTE.ID);
                st.EVENT_JOURNAL_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(EventJournal e, bool posted)
        {
            m_command.CommandText = Payment.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        //public static PaymentItem FindGRNItem(OdbcCommand cmd, int grnIID)
        //{
        //    cmd.CommandText = PaymentItem.FindByGrnItemIDSQL(grnIID);
        //    OdbcDataReader r = cmd.ExecuteReader();
        //    PaymentItem res = PaymentItem.TransformReader(r);
        //    r.Close();
        //    cmd.CommandText = Payment.GetByIDSQL(res.EVENT.ID);
        //    r = cmd.ExecuteReader();
        //    res.EVENT = Payment.TransformReader(r);
        //    r.Close();
        //    return res;
        //}

        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = Payment.GetSearch(find);
                OdbcDataReader r = m_command.ExecuteReader();
                IList rest = Payment.TransformReaderList(r);
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
                m_command.CommandText = Payment.SelectCountByCode(code);
                int t = Convert.ToInt32(m_command.ExecuteScalar());
                return t > 0;
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public override EventJournal FindLastCodeAndTransactionDate(string codesample)
        {
            m_command.CommandText = Payment.FindLastCodeAndTransactionDate(codesample);
            OdbcDataReader r = m_command.ExecuteReader();
            EventJournal e = Payment.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = Payment.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "Payment");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }

        protected override void doSaveNoTransaction(EventJournal e)
        {
            try
            {
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "Payment");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "Payment", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                Payment stk = (Payment)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = Payment.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (PaymentItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = PaymentItem.SelectMaxIDSQL();
                    item.ID = Convert.ToInt32(m_command.ExecuteScalar());
                }
            }
            catch (Exception x)
            {
                e.ID = 0;
                foreach (EventJournalItem item in e.EVENT_JOURNAL_ITEMS)
                {
                    item.ID = 0;
                }
                throw x;
            }
        }

        protected override void doUpdateNoTransaction(EventJournal en)
        {
            try
            {
                Payment e = (Payment)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (PaymentItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = PaymentItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = PaymentItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        protected override void doDeleteNoTransaction(EventJournal e)
        {
            Payment st = (Payment)e;
            try
            {
                if (getEventStatus(st.ID) == EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = PaymentItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = Payment.DeleteSQL(st.ID);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        private Bank getBank(int ID)
        {
            m_command.CommandText = Bank.GetByIDSQLStatic(ID);
            OdbcDataReader r = m_command.ExecuteReader();
            Bank b = Bank.GetBank(r);
            r.Close();
            return b;
        }
    }
}
