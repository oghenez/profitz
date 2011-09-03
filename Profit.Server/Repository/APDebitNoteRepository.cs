using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class APDebitNoteRepository : JournalRepository
    {
        public APDebitNoteRepository() : base() { }

        protected override void doConfirm(EventJournal events, Period p)
        {
            foreach (APDebitNoteItem item in events.EVENT_JOURNAL_ITEMS)
            {
                SetVendorBalance(item, p);
                item.ProcessPosted();
                updateVendorBalances(item.VENDOR_BALANCE);
                saveVendorBalanceEntry(item.VENDOR_BALANCE_ENTRY);
            }
        }
        protected override void doRevise(EventJournal events, Period p)
        {
            foreach (APDebitNoteItem item in events.EVENT_JOURNAL_ITEMS)
            {
                if (((APDebitNote)events).USED_FOR_PAYMENT)
                    throw new Exception("APDN has been used by Payment, please delete payment.");
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
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "APDebitNote");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "APDebitNote", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                APDebitNote stk = (APDebitNote)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = APDebitNote.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (APDebitNoteItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = APDebitNoteItem.SelectMaxIDSQL();
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
                APDebitNote e = (APDebitNote)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (APDebitNoteItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = APDebitNoteItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = APDebitNoteItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
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
            APDebitNote st = (APDebitNote)e;
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = APDebitNoteItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = APDebitNote.DeleteSQL(st.ID);
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
            m_command.CommandText = APDebitNote.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override EventJournal doGet(int ID)
        {
            m_command.CommandText = APDebitNote.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            APDebitNote st = APDebitNote.TransformReader(r);
            r.Close();
            m_command.CommandText = APDebitNoteItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = APDebitNoteItem.TransformReaderList(r);
            r.Close();
            foreach (APDebitNoteItem sti in stis)
            {
                sti.EVENT_JOURNAL = st;
                //sti.VENDOR = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.VENDOR_BALANCE_ENTRY = VendorBalanceEntryRepository.FindVendorBalanceEntryByEventItem(m_command, sti.ID, sti.VENDOR_BALANCE_ENTRY_TYPE);
                //sti.GRN_ITEM = GoodReceiveNoteRepository.FindGoodReceiveNoteItem(m_command, sti.GRN_ITEM.ID);
                //sti.GRN_ITEM.PART = PartRepository.GetByID(m_command, sti.GRN_ITEM.PART.ID);
                if (sti.PURCHASE_RETURN != null)
                {
                    if (sti.PURCHASE_RETURN.ID > 0)
                    {
                        sti.PURCHASE_RETURN = PurchaseReturnRepository.GetPurchaseReturnForDebitNote(m_command, sti.PURCHASE_RETURN);
                    }
                }
                st.EVENT_JOURNAL_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(EventJournal e, bool posted)
        {
            m_command.CommandText = APDebitNote.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        //public static APDebitNoteItem FindGRNItem(MySql.Data.MySqlClient.MySqlCommand cmd, int grnIID)
        //{
        //    cmd.CommandText = APDebitNoteItem.FindByGrnItemIDSQL(grnIID);
        //    MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
        //    APDebitNoteItem res = APDebitNoteItem.TransformReader(r);
        //    r.Close();
        //    cmd.CommandText = APDebitNote.GetByIDSQL(res.EVENT.ID);
        //    r = cmd.ExecuteReader();
        //    res.EVENT = APDebitNote.TransformReader(r);
        //    r.Close();
        //    return res;
        //}

        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = APDebitNote.GetSearch(find);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList rest = APDebitNote.TransformReaderList(r);
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
                m_command.CommandText = APDebitNote.SelectCountByCode(code);
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
            m_command.CommandText = APDebitNote.FindLastCodeAndTransactionDate(codesample);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            EventJournal e = APDebitNote.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = APDebitNote.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "APDebitNote");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }

        protected override void doSaveNoTransaction(EventJournal e)
        {
            try
            {
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "APDebitNote");
                EventJournal codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "APDebitNote", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                APDebitNote stk = (APDebitNote)e;
                m_command.CommandText = stk.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = APDebitNote.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (APDebitNoteItem item in stk.EVENT_JOURNAL_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = APDebitNoteItem.SelectMaxIDSQL();
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
                APDebitNote e = (APDebitNote)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (APDebitNoteItem sti in e.EVENT_JOURNAL_ITEMS)
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
                        m_command.CommandText = APDebitNoteItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = APDebitNoteItem.DeleteUpdate(e.ID, e.EVENT_JOURNAL_ITEMS);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        protected override void doDeleteNoTransaction(EventJournal e)
        {
            APDebitNote st = (APDebitNote)e;
            try
            {
                if (getEventStatus(st.ID) == EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = APDebitNoteItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = APDebitNote.DeleteSQL(st.ID);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public IList FindAPDNForPayment(int supID, DateTime trdate, string find)
        {
            m_command.CommandText = APDebitNote.GetForPayment(supID, trdate, find);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            IList reuslt = APDebitNote.TransformReaderList(r);
            r.Close();
            return reuslt;
        }
        public static  APDebitNote FindAPDNForPayment(MySql.Data.MySqlClient.MySqlCommand m_command, int apdnID)
        {
            m_command.CommandText = APDebitNote.GetByIDSQL(apdnID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            APDebitNote reuslt = APDebitNote.TransformReader(r);
            r.Close();
            return reuslt;
        }
    }
}
