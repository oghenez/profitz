using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class ARCreditNoteRepository : JournalRepository
    {
        public ARCreditNoteRepository() : base() { }

        protected override void doConfirm(EventJournal events, Period p)
        {
            foreach (ARCreditNoteItem item in events.EVENT_JOURNAL_ITEMS)
            {
                SetVendorBalance(item, p);
                item.ProcessPosted();
                updateVendorBalances(item.VENDOR_BALANCE);
                saveVendorBalanceEntry(item.VENDOR_BALANCE_ENTRY);
            }
        }
        protected override void doRevise(EventJournal events, Period p)
        {
            foreach (ARCreditNoteItem item in events.EVENT_JOURNAL_ITEMS)
            {
                IList paid = ReceiptRepository.FindReceiptUsingARCR(m_command, item.EVENT_JOURNAL.ID);
               // if (((ARCreditNote)events).USED_FOR_PAYMENT)
                if(paid.Count>0)
                    throw new Exception("ARCR["+item.EVENT_JOURNAL.CODE+"] has been used by Receipt ["+((ReceiptItem)paid[0]).EVENT_JOURNAL.CODE+"], please delete receipt.");
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
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "ARCreditNote");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "ARCreditNote", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                ARCreditNote stk = (ARCreditNote)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = ARCreditNote.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (ARCreditNoteItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = ARCreditNoteItem.SelectMaxIDSQL();
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
                ARCreditNote e = (ARCreditNote)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (ARCreditNoteItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = ARCreditNoteItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = ARCreditNoteItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
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
            ARCreditNote st = (ARCreditNote)e;
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = ARCreditNoteItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = ARCreditNote.DeleteSQL(st.ID);
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
            m_command.CommandText = ARCreditNote.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override EventJournal doGet(int ID)
        {
            m_command.CommandText = ARCreditNote.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            ARCreditNote st = ARCreditNote.TransformReader(r);
            r.Close();
            m_command.CommandText = ARCreditNoteItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = ARCreditNoteItem.TransformReaderList(r);
            r.Close();
            foreach (ARCreditNoteItem sti in stis)
            {
                sti.EVENT_JOURNAL = st;
                //sti.VENDOR = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.VENDOR_BALANCE_ENTRY = VendorBalanceEntryRepository.FindVendorBalanceEntryByEventItem(m_command, sti.ID, sti.VENDOR_BALANCE_ENTRY_TYPE);
                //sti.GRN_ITEM = GoodReceiveNoteRepository.FindGoodReceiveNoteItem(m_command, sti.GRN_ITEM.ID);
                //sti.GRN_ITEM.PART = PartRepository.GetByID(m_command, sti.GRN_ITEM.PART.ID);
                if (sti.SALES_RETURN != null)
                {
                    if (sti.SALES_RETURN.ID > 0)
                    {
                        sti.SALES_RETURN = SalesReturnRepository.GetSalesReturnForCreditNote(m_command, sti.SALES_RETURN);
                    }
                }
                st.EVENT_JOURNAL_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(EventJournal e, bool posted)
        {
            m_command.CommandText = ARCreditNote.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        //public static ARCreditNoteItem FindGRNItem(MySql.Data.MySqlClient.MySqlCommand cmd, int grnIID)
        //{
        //    cmd.CommandText = ARCreditNoteItem.FindByGrnItemIDSQL(grnIID);
        //    MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
        //    ARCreditNoteItem res = ARCreditNoteItem.TransformReader(r);
        //    r.Close();
        //    cmd.CommandText = ARCreditNote.GetByIDSQL(res.EVENT.ID);
        //    r = cmd.ExecuteReader();
        //    res.EVENT = ARCreditNote.TransformReader(r);
        //    r.Close();
        //    return res;
        //}

        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = ARCreditNote.GetSearch(find);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList rest = ARCreditNote.TransformReaderList(r);
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
                m_command.CommandText = ARCreditNote.SelectCountByCode(code);
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
            m_command.CommandText = ARCreditNote.FindLastCodeAndTransactionDate(codesample);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            EventJournal e = ARCreditNote.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = ARCreditNote.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "ARCreditNote");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }

        protected override void doSaveNoTransaction(EventJournal e)
        {
            try
            {
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "ARCreditNote");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "ARCreditNote", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                ARCreditNote stk = (ARCreditNote)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = ARCreditNote.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (ARCreditNoteItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = ARCreditNoteItem.SelectMaxIDSQL();
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
                ARCreditNote e = (ARCreditNote)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (ARCreditNoteItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = ARCreditNoteItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = ARCreditNoteItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        protected override void doDeleteNoTransaction(EventJournal e)
        {
            ARCreditNote st = (ARCreditNote)e;
            try
            {
                if (getEventStatus(st.ID) == EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = ARCreditNoteItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = ARCreditNote.DeleteSQL(st.ID);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public IList FindAPDNForReceipt(int supID, DateTime trdate, string find, IList notInID)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (int i in notInID)
            {
                poisSB.Append(i.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notInID.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";

            m_command.CommandText = ARCreditNote.GetForReceipt(supID, trdate, find, pois);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            IList reuslt = ARCreditNote.TransformReaderList(r);
            r.Close();
            return reuslt;
        }
        public static  ARCreditNote FindARCRForReceipt(MySql.Data.MySqlClient.MySqlCommand m_command, int apdnID)
        {
            m_command.CommandText = ARCreditNote.GetByIDSQL(apdnID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            ARCreditNote reuslt = ARCreditNote.TransformReader(r);
            r.Close();
            return reuslt;
        }
        internal static IList FindARCRBySalesReturn(MySql.Data.MySqlClient.MySqlCommand cmd, int prID)
        {
            cmd.CommandText = ARCreditNoteItem.GetARCRItemBySRID(prID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            IList result = ARCreditNoteItem.TransformReaderList(r);
            r.Close();
            foreach (ARCreditNoteItem i in result)
            {
                cmd.CommandText = ARCreditNote.GetByIDSQL(i.EVENT_JOURNAL.ID);
                r = cmd.ExecuteReader();
                i.EVENT_JOURNAL = ARCreditNote.TransformReader(r);
                r.Close();
            }
            return result;
        }
    }
}
