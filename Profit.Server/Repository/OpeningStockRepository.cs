using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class OpeningStockRepository : TransactionRepository
    {
        public OpeningStockRepository() : base() { }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (EventItem item in events.EVENT_ITEMS)
            {
                SetStockCard(item, p);
                assertValidStockCardForOpening(item);
            }
        }
        public void ConfirmNoTransaction(int id, MySql.Data.MySqlClient.MySqlCommand command)
        {
            m_command = command;
            try
            {
                Event events = this.Get(id);
                if (events.POSTED) //.EVENT_STATUS == EventStatus.Confirm)
                    throw new Exception("Status is already Posted/Confirm");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                doConfirm(events, p);
                events.ProcessConfirm();
                this.UpdateStatus(events, true);
                updateStockCards(events.EVENT_ITEMS);
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        private void assertValidStockCardForOpening(EventItem item)
        {
            if (item.STOCK_CARD.BALANCE > 0)
            {
                throw new Exception("There is stock for this Item [" + item.PART.NAME + "], please unpost related transaction");
            }
        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (EventItem item in events.EVENT_ITEMS)
            {
                if (events.NOTES == "AUTO" + p.START_DATE.ToString(Utils.DATE_FORMAT_SHORT))
                    throw new Exception("Transaksi ini otomatis, tidak bisa di unpost");
                SetStockCard(item, p);
            }
        }
        public void ReviseNoTransaction(int id, MySql.Data.MySqlClient.MySqlCommand command)
        {
            m_command = command;
            try
            {
                Event events = this.Get(id);
                if (events.EVENT_STATUS == EventStatus.Entry)
                    throw new Exception("Status is already Unposted/Entry");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                foreach (EventItem item in events.EVENT_ITEMS)
                {
                    SetStockCard(item, p);
                }
                events.ProcessRevised();
                this.UpdateStatus(events, false);
                deleteStockCardEntry(events.DELETED_STOCK_CARD_ENTRY);
                updateStockCards(events.EVENT_ITEMS);
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        protected override void doSave(Event e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;                
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "OpeningStock");
                Event codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "OpeningStock", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                
                OpeningStock stk = (OpeningStock)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = OpeningStock.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (OpeningStockItem item in stk.EVENT_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = OpeningStockItem.SelectMaxIDSQL();
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
        public void SaveNoTransaction(Event e, MySql.Data.MySqlClient.MySqlCommand command)
        {
            try
            {
                m_command = command;
                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "OpeningStock");
                Event codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "OpeningStock", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                OpeningStock stk = (OpeningStock)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = OpeningStock.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (OpeningStockItem item in stk.EVENT_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = OpeningStockItem.SelectMaxIDSQL();
                    item.ID = Convert.ToInt32(m_command.ExecuteScalar());
                }
            }
            catch (Exception x)
            {
                e.ID = 0;
                foreach (EventItem item in e.EVENT_ITEMS)
                {
                    item.ID = 0;
                }
                throw x;
            }
        }
        protected override void doUpdate(Event en)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                OpeningStock e = (OpeningStock)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (OpeningStockItem sti in e.EVENT_ITEMS)
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
                        m_command.CommandText = OpeningStockItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = OpeningStockItem.DeleteUpdate(e.ID, e.EVENT_ITEMS);
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
            OpeningStock st = (OpeningStock)e;//this.Get(e.ID);
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = OpeningStockItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = OpeningStock.DeleteSQL(st.ID);
                m_command.ExecuteNonQuery();
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        public void DeleteNoTransaction(Event e, MySql.Data.MySqlClient.MySqlCommand command)
        {
            OpeningStock st = (OpeningStock)e;//this.Get(e.ID);
            m_command = command;
            try
            {
                if (getEventStatus(st.ID) == EventStatus.Confirm)
                    throw new Exception("Revise before delete");
                m_command.CommandText = OpeningStockItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = OpeningStock.DeleteSQL(st.ID);
                m_command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        private EventStatus getEventStatus(int id)
        {
            m_command.CommandText = OpeningStock.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = OpeningStock.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            OpeningStock st = OpeningStock.TransformReader(r);
            r.Close();
            m_command.CommandText = OpeningStockItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = OpeningStockItem.TransformReaderList(r);
            r.Close();
            foreach (OpeningStockItem sti in stis)
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
            m_command.CommandText = OpeningStock.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = OpeningStock.GetSearch(find);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList rest = OpeningStock.TransformReaderList(r);
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
                m_command.CommandText = OpeningStock.SelectCountByCode(code);
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
            m_command.CommandText = OpeningStock.FindLastCodeAndTransactionDate(codesample);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            Event e = OpeningStock.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = OpeningStock.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "OpeningStock");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }
        public static OpeningStock GetHeaderOnly(MySql.Data.MySqlClient.MySqlCommand cmd, int poID)
        {
            cmd.CommandText = OpeningStock.GetByIDSQL(poID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            OpeningStock st = OpeningStock.TransformReader(r);
            r.Close();
            return st;
        }
        public OpeningStock GetOpeningStockByNotes(string note)
        {
            m_command.CommandText = OpeningStock.GetByNotesSQL(note);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            OpeningStock st = OpeningStock.TransformReader(r);
            r.Close();
            m_command.CommandText = OpeningStockItem.GetByEventIDSQL(st.ID);
            r = m_command.ExecuteReader();
            IList stis = OpeningStockItem.TransformReaderList(r);
            r.Close();
            foreach (OpeningStockItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
    }
}
