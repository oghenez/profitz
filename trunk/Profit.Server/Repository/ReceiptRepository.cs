﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class ReceiptRepository : JournalRepository
    {
        public ReceiptRepository() : base() { }
        public ReceiptRepository(MySql.Data.MySqlClient.MySqlCommand cmd)
            : base() 
        {
            m_command = cmd;
        }

        protected override void doConfirm(EventJournal events, Period p)
        {
            foreach (ReceiptItem item in events.EVENT_JOURNAL_ITEMS)
            {
                assertConfirmedSIJ(item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL);
                assertValidDate(item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL, events);
                item.CURRENCY = events.CURRENCY;
                item.VENDOR = events.VENDOR;
                SetVendorBalance(item, p);
                item.ProcessPosted();
                updateVendorBalances(item.VENDOR_BALANCE);
                saveVendorBalanceEntry(item.VENDOR_BALANCE_ENTRY);
                item.CUSTOMER_INVOICE_JOURNAL_ITEM.SetOSAgainstReceiptItem(item);
                if (item.CUSTOMER_INVOICE_JOURNAL_ITEM is CustomerInvoiceJournalItem)
                {
                    CustomerInvoiceJournalRepository.UpdateAgainstStatus(m_command, item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                        item.CUSTOMER_INVOICE_JOURNAL_ITEM);
                }
                if (item.CUSTOMER_INVOICE_JOURNAL_ITEM is CustomerOutStandingInvoiceItem)
                {
                    CustomerOutStandingInvoiceRepository.UpdateAgainstStatus(m_command, item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                        item.CUSTOMER_INVOICE_JOURNAL_ITEM);
                }
                // if (item.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                //{
        
                //}
            }
        }
        protected override void doRevise(EventJournal events, Period p)
        {
            foreach (ReceiptItem item in events.EVENT_JOURNAL_ITEMS)
            {
                
                item.CURRENCY = events.CURRENCY;
                item.VENDOR = events.VENDOR;
                SetVendorBalance(item, p);
                item.ProcessUnPosted();
                updateVendorBalances(item.VENDOR_BALANCE);
                deleteVendorBalanceEntry(item.VENDOR_BALANCE_ENTRY);
                item.CUSTOMER_INVOICE_JOURNAL_ITEM.UnSetOSAgainstReceiptItem(item);
                if (item.CUSTOMER_INVOICE_JOURNAL_ITEM is CustomerInvoiceJournalItem)
                {
                    CustomerInvoiceJournalRepository.UpdateAgainstStatus(m_command, item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                        item.CUSTOMER_INVOICE_JOURNAL_ITEM);
                    CustomerInvoiceJournal cij = (CustomerInvoiceJournal)((CustomerInvoiceJournalItem)item.CUSTOMER_INVOICE_JOURNAL_ITEM).EVENT_JOURNAL;
                    if (cij.POS_INVOICE.ID > 0)
                    {
                        throw new Exception("POS Receipt can not revise, please revise POS");
                    }
                }
                if (item.CUSTOMER_INVOICE_JOURNAL_ITEM is CustomerOutStandingInvoiceItem)
                {
                    CustomerOutStandingInvoiceRepository.UpdateAgainstStatus(m_command, item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                        item.CUSTOMER_INVOICE_JOURNAL_ITEM);
                }
                //if (item.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                //{
        
                //}
            }
        }
        public void ReviseForPOSNoTransaction(int id)
        {
            try
            {
                EventJournal events = this.Get(id);
                if (events.EVENT_STATUS == EventStatus.Entry)
                    throw new Exception("Status is already Unposted/Entry");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                foreach (ReceiptItem item in events.EVENT_JOURNAL_ITEMS)
                {

                    item.CURRENCY = events.CURRENCY;
                    item.VENDOR = events.VENDOR;
                    SetVendorBalance(item, p);
                    item.ProcessUnPosted();
                    updateVendorBalances(item.VENDOR_BALANCE);
                    deleteVendorBalanceEntry(item.VENDOR_BALANCE_ENTRY);
                    item.CUSTOMER_INVOICE_JOURNAL_ITEM.UnSetOSAgainstReceiptItem(item);
                    if (item.CUSTOMER_INVOICE_JOURNAL_ITEM is CustomerInvoiceJournalItem)
                    {
                        CustomerInvoiceJournalRepository.UpdateAgainstStatus(m_command, item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                            item.CUSTOMER_INVOICE_JOURNAL_ITEM);
                        CustomerInvoiceJournal cij = (CustomerInvoiceJournal)((CustomerInvoiceJournalItem)item.CUSTOMER_INVOICE_JOURNAL_ITEM).EVENT_JOURNAL;
                    }
                    //if (item.CUSTOMER_INVOICE_JOURNAL_ITEM is CustomerOutStandingInvoiceItem)
                    //{
                    //    CustomerOutStandingInvoiceRepository.UpdateAgainstStatus(m_command, item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL,
                    //        item.CUSTOMER_INVOICE_JOURNAL_ITEM);
                    //}
                }
                events.ProcessUnPosted();
                this.UpdateStatus(events, false);
            }
            catch (Exception x)
            {
                throw x;
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
                throw new Exception("Receipt Date can not less than Invoice Date :" + sij.CODE + " [ " + sij.TRANSACTION_DATE.ToString("dd-MMM-yyyy") + " ] ");
        }

        protected override void doSave(EventJournal e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;

                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "Receipt");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "Receipt", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                Receipt stk = (Receipt)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = Receipt.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (ReceiptItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    item.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE = item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL.VENDOR_BALANCE_ENTRY_TYPE;
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = ReceiptItem.SelectMaxIDSQL();
                    item.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    if (item.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                    {
                        m_command.CommandText = ARCreditNote.UpdateUsedForReceipt(item.AR_CREDIT_NOTE.ID, true);
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
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                Receipt e = (Receipt)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (ReceiptItem sti in e.EVENT_JOURNAL_ITEMS)
                {
                    sti.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE = sti.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL.VENDOR_BALANCE_ENTRY_TYPE;
                    if (sti.ID > 0)
                    {
                        m_command.CommandText = sti.GetUpdateSQL();
                        m_command.ExecuteNonQuery();
                    }
                    else
                    {
                        m_command.CommandText = sti.GetInsertSQL();
                        m_command.ExecuteNonQuery();
                        m_command.CommandText = ReceiptItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                    if (sti.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                    {
                        m_command.CommandText = ARCreditNote.UpdateUsedForReceipt(sti.AR_CREDIT_NOTE.ID, true);
                        m_command.ExecuteNonQuery();
                    }
                }
                m_command.CommandText = ReceiptItem.GetNotInTypeARCR(e.ID, e.EVENT_JOURNAL_ITEMS);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList pymnts = ReceiptItem.TransformReaderList(r);
                r.Close();
                foreach (ReceiptItem itm in pymnts)
                {
                     m_command.CommandText = ARCreditNote.UpdateUsedForReceipt(itm.AR_CREDIT_NOTE.ID, false);
                     m_command.ExecuteNonQuery();
                }
               

                m_command.CommandText = ReceiptItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
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
            Receipt st = (Receipt)e;
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                foreach (ReceiptItem sti in e.EVENT_JOURNAL_ITEMS)
                {
                    if (sti.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                    {
                        m_command.CommandText = ARCreditNote.UpdateUsedForReceipt(sti.AR_CREDIT_NOTE.ID, false);
                        m_command.ExecuteNonQuery();
                    }
                }
                m_command.CommandText = ReceiptItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = Receipt.DeleteSQL(st.ID);
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
            m_command.CommandText = Receipt.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override EventJournal doGet(int ID)
        {
            m_command.CommandText = Receipt.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            Receipt st = Receipt.TransformReader(r);
            r.Close();
            if (st == null) return null;
            m_command.CommandText = ReceiptItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = ReceiptItem.TransformReaderList(r);
            r.Close();
            foreach (ReceiptItem sti in stis)
            {
                sti.EVENT_JOURNAL = st;
                //sti.VENDOR = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.VENDOR_BALANCE_ENTRY = VendorBalanceEntryRepository.FindVendorBalanceEntryByEventItem(m_command, sti.ID, sti.VENDOR_BALANCE_ENTRY_TYPE);
                //sti.GRN_ITEM = GoodReceiveNoteRepository.FindGoodReceiveNoteItem(m_command, sti.GRN_ITEM.ID);
                //sti.GRN_ITEM.PART = PartRepository.GetByID(m_command, sti.GRN_ITEM.PART.ID);
                if (sti.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE == VendorBalanceEntryType.CustomerInvoice)
                {
                    if(sti.CUSTOMER_INVOICE_JOURNAL_ITEM!=null)
                        sti.CUSTOMER_INVOICE_JOURNAL_ITEM = CustomerInvoiceJournalRepository.FindCIJournalItemlistForReceipt(m_command, sti.CUSTOMER_INVOICE_JOURNAL_ITEM.GetID());
                }
                if (sti.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE == VendorBalanceEntryType.CustomerOutStandingInvoice)
                {
                    if (sti.CUSTOMER_INVOICE_JOURNAL_ITEM != null)
                        sti.CUSTOMER_INVOICE_JOURNAL_ITEM = CustomerOutStandingInvoiceRepository.FindCOIItemlistForReceipt(m_command, sti.CUSTOMER_INVOICE_JOURNAL_ITEM.GetID());
                }
                if (sti.PAYMENT_TYPE == ReceiptType.Bank)
                    sti.BANK = getBank(sti.BANK.ID);
                if (sti.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                    sti.AR_CREDIT_NOTE = ARCreditNoteRepository.FindARCRForReceipt(m_command, sti.AR_CREDIT_NOTE.ID);
                st.EVENT_JOURNAL_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(EventJournal e, bool posted)
        {
            m_command.CommandText = Receipt.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        //public static ReceiptItem FindGRNItem(MySql.Data.MySqlClient.MySqlCommand cmd, int grnIID)
        //{
        //    cmd.CommandText = ReceiptItem.FindByGrnItemIDSQL(grnIID);
        //    MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
        //    ReceiptItem res = ReceiptItem.TransformReader(r);
        //    r.Close();
        //    cmd.CommandText = Receipt.GetByIDSQL(res.EVENT.ID);
        //    r = cmd.ExecuteReader();
        //    res.EVENT = Receipt.TransformReader(r);
        //    r.Close();
        //    return res;
        //}

        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = Receipt.GetSearch(find);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList rest = Receipt.TransformReaderList(r);
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
                m_command.CommandText = Receipt.SelectCountByCode(code);
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
            m_command.CommandText = Receipt.FindLastCodeAndTransactionDate(codesample);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            EventJournal e = Receipt.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = Receipt.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "Receipt");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }

        protected override void doSaveNoTransaction(EventJournal e)
        {
            try
            {
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "Receipt");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "Receipt", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                Receipt stk = (Receipt)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = Receipt.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (ReceiptItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    item.EVENT_JOURNAL = stk;
                    item.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE = item.CUSTOMER_INVOICE_JOURNAL_ITEM.GET_EVENT_JOURNAL.VENDOR_BALANCE_ENTRY_TYPE;
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = ReceiptItem.SelectMaxIDSQL();
                    item.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    if (item.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                    {
                        m_command.CommandText = ARCreditNote.UpdateUsedForReceipt(item.AR_CREDIT_NOTE.ID, true);
                        m_command.ExecuteNonQuery();
                    }
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
                Receipt e = (Receipt)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (ReceiptItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = ReceiptItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = ReceiptItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        protected override void doDeleteNoTransaction(EventJournal e)
        {
            Receipt st = (Receipt)this.Get(e.ID);
            try
            {
                
                if (getEventStatus(st.ID) == EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                foreach (ReceiptItem sti in e.EVENT_JOURNAL_ITEMS)
                {
                    if (sti.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                    {
                        m_command.CommandText = ARCreditNote.UpdateUsedForReceipt(sti.AR_CREDIT_NOTE.ID, false);
                        m_command.ExecuteNonQuery();
                    }
                }
                m_command.CommandText = ReceiptItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = Receipt.DeleteSQL(st.ID);
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
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            Bank b = Bank.GetBank(r);
            r.Close();
            return b;
        }
        internal static IList FindPaidCustomerOutstanding(MySql.Data.MySqlClient.MySqlCommand cmd, int soiID)
        {
            cmd.CommandText = ReceiptItem.GetCustomerInvoiceBySOIID(soiID, VendorBalanceEntryType.CustomerOutStandingInvoice);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            IList result = ReceiptItem.TransformReaderList(r);
            r.Close();
            foreach (ReceiptItem i in result)
            {
                cmd.CommandText = Receipt.GetByIDSQL(i.EVENT_JOURNAL.ID);
                 r = cmd.ExecuteReader();
                 i.EVENT_JOURNAL = Receipt.TransformReader(r);
                 r.Close();
            }
            return result;
        }
        internal static IList FindPaidSupplierInvoice(MySql.Data.MySqlClient.MySqlCommand cmd, int siID)
        {
            cmd.CommandText = ReceiptItem.GetCustomerInvoiceBySOIID(siID, VendorBalanceEntryType.SupplierInvoice);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            IList result = ReceiptItem.TransformReaderList(r);
            r.Close();
            foreach (ReceiptItem i in result)
            {
                cmd.CommandText = Receipt.GetByIDSQL(i.EVENT_JOURNAL.ID);
                r = cmd.ExecuteReader();
                i.EVENT_JOURNAL = Receipt.TransformReader(r);
                r.Close();
            }
            return result;
        }
        internal static IList FindPaidCustomerInvoice(MySql.Data.MySqlClient.MySqlCommand cmd, int siID)
        {
            cmd.CommandText = ReceiptItem.GetCustomerInvoiceBySOIID(siID, VendorBalanceEntryType.CustomerInvoice);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            IList result = ReceiptItem.TransformReaderList(r);
            r.Close();
            foreach (ReceiptItem i in result)
            {
                cmd.CommandText = Receipt.GetByIDSQL(i.EVENT_JOURNAL.ID);
                r = cmd.ExecuteReader();
                i.EVENT_JOURNAL = Receipt.TransformReader(r);
                r.Close();
            }
            return result;
        }
        internal static IList FindReceiptUsingARCR(MySql.Data.MySqlClient.MySqlCommand cmd, int apDNID)
        {
            cmd.CommandText = ReceiptItem.GetReceiptItemByARCR(apDNID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            IList result = ReceiptItem.TransformReaderList(r);
            r.Close();
            foreach (ReceiptItem i in result)
            {
                cmd.CommandText = Receipt.GetByIDSQL(i.EVENT_JOURNAL.ID);
                r = cmd.ExecuteReader();
                i.EVENT_JOURNAL = Receipt.TransformReader(r);
                r.Close();
            }
            return result;
        }
        public ReceiptItem FindReceiptItemByCIJ(int CIId)
        {
            string sql = ReceiptItem.GetCustomerInvoiceBySOIID(CIId, VendorBalanceEntryType.CustomerInvoice);
            m_command.CommandText = sql;
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            ReceiptItem rest = ReceiptItem.TransformReader(r);
            r.Close();
            return rest;
        }
    }
}
