using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SupplierInvoiceJournalRepository : JournalRepository
    {
        public SupplierInvoiceJournalRepository() : base() { }
        public SupplierInvoiceJournalRepository(OdbcCommand cmd) : base() 
        {
            m_command = cmd;
        }
        protected override void doConfirm(EventJournal events, Period p)
        {
            foreach (SupplierInvoiceJournalItem item in events.EVENT_JOURNAL_ITEMS)
            {
                SetVendorBalance(item, p);
            }
        }
        protected override void doRevise(EventJournal events, Period p)
        {
            foreach (SupplierInvoiceJournalItem item in events.EVENT_JOURNAL_ITEMS)
            {
                SetVendorBalance(item, p);
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
            OdbcTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;

                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "SupplierInvoiceJournal");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "SupplierInvoiceJournal", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                SupplierInvoiceJournal stk = (SupplierInvoiceJournal)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = SupplierInvoiceJournal.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (SupplierInvoiceJournalItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = SupplierInvoiceJournalItem.SelectMaxIDSQL();
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

        protected override void doSaveNoTransaction(EventJournal e)
        {
            try
            {
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "SupplierInvoiceJournal");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "SupplierInvoiceJournal", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                SupplierInvoiceJournal stk = (SupplierInvoiceJournal)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = SupplierInvoiceJournal.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (SupplierInvoiceJournalItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = SupplierInvoiceJournalItem.SelectMaxIDSQL();
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
        protected override void doUpdate(EventJournal en)
        {
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                SupplierInvoiceJournal e = (SupplierInvoiceJournal)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (SupplierInvoiceJournalItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = SupplierInvoiceJournalItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = SupplierInvoiceJournalItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
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
            SupplierInvoiceJournal st = (SupplierInvoiceJournal)e;
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = SupplierInvoiceJournalItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = SupplierInvoiceJournal.DeleteSQL(st.ID);
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
            m_command.CommandText = SupplierInvoiceJournal.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override EventJournal doGet(int ID)
        {
            m_command.CommandText = SupplierInvoiceJournal.GetByIDSQL(ID);
            OdbcDataReader r = m_command.ExecuteReader();
            SupplierInvoiceJournal st = SupplierInvoiceJournal.TransformReader(r);
            r.Close();
            m_command.CommandText = SupplierInvoiceJournalItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = SupplierInvoiceJournalItem.TransformReaderList(r);
            r.Close();
            foreach (SupplierInvoiceJournalItem sti in stis)
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
            m_command.CommandText = SupplierInvoiceJournal.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        //public static SupplierInvoiceJournalItem FindGRNItem(OdbcCommand cmd, int grnIID)
        //{
        //    cmd.CommandText = SupplierInvoiceJournalItem.FindByGrnItemIDSQL(grnIID);
        //    OdbcDataReader r = cmd.ExecuteReader();
        //    SupplierInvoiceJournalItem res = SupplierInvoiceJournalItem.TransformReader(r);
        //    r.Close();
        //    cmd.CommandText = SupplierInvoiceJournal.GetByIDSQL(res.EVENT.ID);
        //    r = cmd.ExecuteReader();
        //    res.EVENT = SupplierInvoiceJournal.TransformReader(r);
        //    r.Close();
        //    return res;
        //}

        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = SupplierInvoiceJournal.GetSearch(find);
                OdbcDataReader r = m_command.ExecuteReader();
                IList rest = SupplierInvoiceJournal.TransformReaderList(r);
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
                m_command.CommandText = SupplierInvoiceJournal.SelectCountByCode(code);
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
            m_command.CommandText = SupplierInvoiceJournal.FindLastCodeAndTransactionDate(codesample);
            OdbcDataReader r = m_command.ExecuteReader();
            EventJournal e = SupplierInvoiceJournal.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = SupplierInvoiceJournal.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "SupplierInvoiceJournal");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }


        protected override void doUpdateNoTransaction(EventJournal en)
        {
            try
            {
                SupplierInvoiceJournal e = (SupplierInvoiceJournal)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (SupplierInvoiceJournalItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = SupplierInvoiceJournalItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = SupplierInvoiceJournalItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        protected override void doDeleteNoTransaction(EventJournal e)
        {
            SupplierInvoiceJournal st = (SupplierInvoiceJournal)e;
            try
            {
                if (getEventStatus(st.ID) == EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = SupplierInvoiceJournalItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = SupplierInvoiceJournal.DeleteSQL(st.ID);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public SupplierInvoiceJournal FindPeriodSIJId(int SIId)
        {
            string sql = SupplierInvoiceJournal.FindPeriodSIJId(SIId);
            m_command.CommandText = sql;
            OdbcDataReader r = m_command.ExecuteReader();
            SupplierInvoiceJournal rest = SupplierInvoiceJournal.TransformReader(r);
            r.Close();
            return rest;
        }
        public static void UpdateAgainstStatus(OdbcCommand cmd, EventJournal e, ISupplierInvoiceJournalItem ei)
        {
            SupplierInvoiceJournal po = (SupplierInvoiceJournal)e;
            SupplierInvoiceJournalItem poi = (SupplierInvoiceJournalItem)ei;
            cmd.CommandText = poi.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
            cmd.CommandText = po.UpdateAgainstStatus();
            cmd.ExecuteNonQuery();
        }
        internal static SupplierInvoiceJournalItem FindSIJournalItemlistForPayment(OdbcCommand cmd, int supinvItemID)
        {
            cmd.CommandText = SupplierInvoiceJournalItem.GetByIDSQL(supinvItemID);
            OdbcDataReader r = cmd.ExecuteReader();
            SupplierInvoiceJournalItem result = SupplierInvoiceJournalItem.TransformReader(r);
            r.Close();
            cmd.CommandText = SupplierInvoiceJournal.GetByIDSQL(result.EVENT_JOURNAL.ID);
            r = cmd.ExecuteReader();
            result.EVENT_JOURNAL = SupplierInvoiceJournal.TransformReader(r);
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
        public IList FindSIJournalItemlistForPayment(string find, int supplier, DateTime trdate, IList notIn)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (int i in notIn)
            {
                poisSB.Append(i.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIn.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            m_command.CommandText = SupplierInvoiceJournalItem.GetSearchForPayment(find, supplier, pois, trdate);
            OdbcDataReader r = m_command.ExecuteReader();
            IList result = SupplierInvoiceJournalItem.TransformReaderList(r);
            r.Close();
            foreach (SupplierInvoiceJournalItem t in result)
            {
                m_command.CommandText = SupplierInvoiceJournal.GetByIDSQL(t.EVENT_JOURNAL.ID);
                r = m_command.ExecuteReader();
                t.EVENT_JOURNAL = SupplierInvoiceJournal.TransformReader(r);
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
        public double GetOutstanding(int sijiID)
        {
            m_command.CommandText = SupplierInvoiceJournalItem.GetByOutstandingSQL(sijiID);
            double d = Convert.ToDouble(m_command.ExecuteScalar());
            return d;
        }
        public double GetPaid(int sijiID)
        {
            m_command.CommandText = SupplierInvoiceJournalItem.GetByPaidSQL(sijiID);
            double d = Convert.ToDouble(m_command.ExecuteScalar());
            return d;
        }
    }
}
