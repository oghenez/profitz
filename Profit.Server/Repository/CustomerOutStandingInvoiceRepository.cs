﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class CustomerOutStandingInvoiceRepository : JournalRepository
    {
        public CustomerOutStandingInvoiceRepository() : base() { }

        protected override void doConfirm(EventJournal events, Period p)
        {
            foreach (CustomerOutStandingInvoiceItem item in events.EVENT_JOURNAL_ITEMS)
            {
                SetVendorBalance(item, p);
                item.ProcessPosted();
                updateVendorBalances(item.VENDOR_BALANCE);
                saveVendorBalanceEntry(item.VENDOR_BALANCE_ENTRY);
            }
        }
        protected override void doRevise(EventJournal events, Period p)
        {
            foreach (CustomerOutStandingInvoiceItem item in events.EVENT_JOURNAL_ITEMS)
            {
                IList used = ReceiptRepository.FindPaidCustomerOutstanding(m_command, item.ID);
                if (used.Count > 0)
                    //if (item.PAID_AMOUNT > 0)
                    throw new Exception("This Invoice [" + item.INVOICE_NO + "] is paid ["+((ReceiptItem)used[0]).EVENT_JOURNAL.CODE+"], please delete receipt first.");
                SetVendorBalance(item, p);
                item.ProcessUnPosted();
                updateVendorBalances(item.VENDOR_BALANCE);
                deleteVendorBalanceEntry(item.VENDOR_BALANCE_ENTRY);
            }
        }
        //private void assertConfirmedPO(EventJournal p)
        //{
        //    if (p.EVENT_STATUS.Equals(EventStatus.Entry))
        //        throw new Exception("PO not confirmed :" + p.CODE);
        //}
        //private void assertValidDate(EventJournal po, EventJournal grn)
        //{
        //    if (grn.TRANSACTION_DATE < po.TRANSACTION_DATE)
        //        throw new Exception("GRN Date can not less than PO Date :" + po.CODE + " [ " + po.TRANSACTION_DATE.ToString("dd-MMM-yyyy") + " ] ");
        //}
        protected override void doSave(EventJournal e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;

                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "CustomerOutStandingInvoice");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "CustomerOutStandingInvoice", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                CustomerOutStandingInvoice stk = (CustomerOutStandingInvoice)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = CustomerOutStandingInvoice.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (CustomerOutStandingInvoiceItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = CustomerOutStandingInvoiceItem.SelectMaxIDSQL();
                    item.ID = Convert.ToInt32(m_command.ExecuteScalar());
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
                CustomerOutStandingInvoice e = (CustomerOutStandingInvoice)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (CustomerOutStandingInvoiceItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = CustomerOutStandingInvoiceItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = CustomerOutStandingInvoiceItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
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
            CustomerOutStandingInvoice st = (CustomerOutStandingInvoice)e;
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = CustomerOutStandingInvoiceItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = CustomerOutStandingInvoice.DeleteSQL(st.ID);
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
            m_command.CommandText = CustomerOutStandingInvoice.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override EventJournal doGet(int ID)
        {
            m_command.CommandText = CustomerOutStandingInvoice.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            CustomerOutStandingInvoice st = CustomerOutStandingInvoice.TransformReader(r);
            r.Close();
            m_command.CommandText = CustomerOutStandingInvoiceItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = CustomerOutStandingInvoiceItem.TransformReaderList(r);
            r.Close();
            foreach (CustomerOutStandingInvoiceItem sti in stis)
            {
                sti.EVENT_JOURNAL = st;
                //sti.VENDOR = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.VENDOR_BALANCE_ENTRY = VendorBalanceEntryRepository.FindVendorBalanceEntryByEventItem(m_command, sti.ID, sti.VENDOR_BALANCE_ENTRY_TYPE);
                //sti.GRN_ITEM = GoodReceiveNoteRepository.FindGoodReceiveNoteItem(m_command, sti.GRN_ITEM.ID);
                //sti.GRN_ITEM.PART = PartRepository.GetByID(m_command, sti.GRN_ITEM.PART.ID);
                st.EVENT_JOURNAL_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(EventJournal e, bool posted)
        {
            m_command.CommandText = CustomerOutStandingInvoice.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        //public static CustomerOutStandingInvoiceItem FindGRNItem(MySql.Data.MySqlClient.MySqlCommand cmd, int grnIID)
        //{
        //    cmd.CommandText = CustomerOutStandingInvoiceItem.FindByGrnItemIDSQL(grnIID);
        //    MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
        //    CustomerOutStandingInvoiceItem res = CustomerOutStandingInvoiceItem.TransformReader(r);
        //    r.Close();
        //    cmd.CommandText = CustomerOutStandingInvoice.GetByIDSQL(res.EVENT.ID);
        //    r = cmd.ExecuteReader();
        //    res.EVENT = CustomerOutStandingInvoice.TransformReader(r);
        //    r.Close();
        //    return res;
        //}

        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = CustomerOutStandingInvoice.GetSearch(find);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList rest = CustomerOutStandingInvoice.TransformReaderList(r);
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
                m_command.CommandText = CustomerOutStandingInvoice.SelectCountByCode(code);
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
            m_command.CommandText = CustomerOutStandingInvoice.FindLastCodeAndTransactionDate(codesample);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            EventJournal e = CustomerOutStandingInvoice.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = CustomerOutStandingInvoice.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "CustomerOutStandingInvoice");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }

        protected override void doSaveNoTransaction(EventJournal e)
        {
            try
            {
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "CustomerOutStandingInvoice");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "CustomerOutStandingInvoice", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                CustomerOutStandingInvoice stk = (CustomerOutStandingInvoice)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = CustomerOutStandingInvoice.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (CustomerOutStandingInvoiceItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = CustomerOutStandingInvoiceItem.SelectMaxIDSQL();
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
                CustomerOutStandingInvoice e = (CustomerOutStandingInvoice)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (CustomerOutStandingInvoiceItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = CustomerOutStandingInvoiceItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = CustomerOutStandingInvoiceItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        protected override void doDeleteNoTransaction(EventJournal e)
        {
            CustomerOutStandingInvoice st = (CustomerOutStandingInvoice)e;
            try
            {
                if (getEventStatus(st.ID) == EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = CustomerOutStandingInvoiceItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = CustomerOutStandingInvoice.DeleteSQL(st.ID);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public IList FindCOIJournalItemlistForReceipt(string find, int ccyID, int customer, DateTime trdate, IList notIn)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (int i in notIn)
            {
                poisSB.Append(i.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIn.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            m_command.CommandText = CustomerOutStandingInvoiceItem.GetSearchForReceipt(find, ccyID, customer, pois, trdate);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            IList result = CustomerOutStandingInvoiceItem.TransformReaderList(r);
            r.Close();
            foreach (CustomerOutStandingInvoiceItem t in result)
            {
                m_command.CommandText = CustomerOutStandingInvoice.GetByIDSQL(t.EVENT_JOURNAL.ID);
                r = m_command.ExecuteReader();
                t.EVENT_JOURNAL = CustomerOutStandingInvoice.TransformReader(r);
                r.Close();

                m_command.CommandText = Currency.GetByIDSQLStatic(t.CURRENCY.ID);
                r = m_command.ExecuteReader();
                t.CURRENCY = Currency.GetCurrency(r);
                r.Close();

                m_command.CommandText = TermOfPayment.GetByIDSQLStatic(t.TOP.ID);
                r = m_command.ExecuteReader();
                t.TOP = TermOfPayment.GetTOP(r);
                r.Close();

                m_command.CommandText = Employee.GetByIDSQLStatic(t.EMPLOYEE.ID);
                r = m_command.ExecuteReader();
                t.EMPLOYEE = Employee.GetEmployee(r);
                r.Close();
            }
            return result;
        }
        internal static CustomerOutStandingInvoiceItem FindCOIItemlistForReceipt(MySql.Data.MySqlClient.MySqlCommand cmd, int supinvItemID)
        {
            cmd.CommandText = CustomerOutStandingInvoiceItem.GetByIDSQL(supinvItemID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            CustomerOutStandingInvoiceItem result = CustomerOutStandingInvoiceItem.TransformReader(r);
            r.Close();
            cmd.CommandText = CustomerOutStandingInvoice.GetByIDSQL(result.EVENT_JOURNAL.ID);
            r = cmd.ExecuteReader();
            result.EVENT_JOURNAL = CustomerOutStandingInvoice.TransformReader(r);
            r.Close();

            cmd.CommandText = Currency.GetByIDSQLStatic(result.CURRENCY.ID);
            r = cmd.ExecuteReader();
            result.CURRENCY = Currency.GetCurrency(r);
            r.Close();

            cmd.CommandText = TermOfPayment.GetByIDSQLStatic(result.TOP.ID);
            r = cmd.ExecuteReader();
            result.TOP = TermOfPayment.GetTOP(r);
            r.Close();

            cmd.CommandText = Employee.GetByIDSQLStatic(result.EMPLOYEE.ID);
            r = cmd.ExecuteReader();
            result.EMPLOYEE = Employee.GetEmployee(r);
            r.Close();


            return result;
        }
        public double GetOutstanding(int sijiID)
        {
            m_command.CommandText = CustomerOutStandingInvoiceItem.GetByOutstandingSQL(sijiID);
            double d = Convert.ToDouble(m_command.ExecuteScalar());
            return d;
        }
        public double GetPaid(int sijiID)
        {
            m_command.CommandText = CustomerOutStandingInvoiceItem.GetByReceiptSQL(sijiID);
            double d = Convert.ToDouble(m_command.ExecuteScalar());
            return d;
        }
        public static void UpdateAgainstStatus(MySql.Data.MySqlClient.MySqlCommand cmd, EventJournal e, ICustomerInvoiceJournalItem ei)
        {
            CustomerOutStandingInvoice po = (CustomerOutStandingInvoice)e;
            CustomerOutStandingInvoiceItem poi = (CustomerOutStandingInvoiceItem)ei;
            cmd.CommandText = poi.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
            cmd.CommandText = po.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
        }
    }
}
