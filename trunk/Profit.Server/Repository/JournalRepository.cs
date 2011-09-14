using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public abstract class JournalRepository
    {
        protected MySql.Data.MySqlClient.MySqlConnection m_connection = new MySql.Data.MySqlClient.MySqlConnection("server=localhost;database=profit_db;uid=root;pwd=1234");
        
        protected MySql.Data.MySqlClient.MySqlCommand m_command = new MySql.Data.MySqlClient.MySqlCommand();
        abstract protected void doConfirm(EventJournal events, Period p);
        abstract protected void doRevise(EventJournal events, Period p);
        abstract protected IList doSearch(string find);
        abstract protected bool doIsCodeExist(string code);

        //abstract protected void doInitEventRepository();
        //protected EventRepository m_eventRepository;

        public JournalRepository()
        {
            m_connection.Open();
            m_command.Connection = m_connection;
        }
        public void Confirm(int id)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                EventJournal events = this.Get(id);
                if (events.POSTED) 
                    throw new Exception("Status is already Posted/Confirm");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                events.ProcessPosting();
                doConfirm(events, p);
                this.UpdateStatus(events, true);
                //updateVendorBalances(events.EVENT_JOURNAL_ITEMS);
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        public void ConfirmNoTransaction(int id)
        {
            try
            {
                EventJournal events = this.Get(id);
                if (events.POSTED)
                    throw new Exception("Status is already Posted/Confirm");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                events.ProcessPosting();
                doConfirm(events, p);
                this.UpdateStatus(events, true);
                //updateVendorBalances(events.EVENT_JOURNAL_ITEMS);
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public void Revise(int id)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                EventJournal events = this.Get(id);
                if (events == null) throw new Exception("Transaction is deleted");
                if (events.EVENT_STATUS == EventStatus.Entry)
                    throw new Exception("Status is already Unposted/Entry");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                doRevise(events, p);
                events.ProcessUnPosted();
                this.UpdateStatus(events, false);
                //deleteVendorBalanceEntry(events.DELETED_VENDORBALANCEENTRY);
                //updateVendorBalances(events.EVENT_JOURNAL_ITEMS);
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        public void ReviseNoTransaction(int id)
        {
            try
            {
                EventJournal events = this.Get(id);
                if (events.EVENT_STATUS == EventStatus.Entry)
                    throw new Exception("Status is already Unposted/Entry");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                doRevise(events, p);
                events.ProcessUnPosted();
                this.UpdateStatus(events, false);
                //deleteVendorBalanceEntry(events.DELETED_VENDORBALANCEENTRY);
               // updateVendorBalances(events.EVENT_JOURNAL_ITEMS);
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        protected Period AssertValidPeriod(DateTime transactionDate)
        {
            Period transactionPeriod = PeriodRepository.FindPeriodByDate(m_command, transactionDate);
            if (transactionPeriod == null)
                throw new Exception("Period not set");
            if (transactionPeriod.PERIOD_STATUS.Equals(PeriodStatus.Close) || transactionPeriod.PERIOD_STATUS.Equals(PeriodStatus.Open))
                throw new Exception("Transaction is not in Current Period");
            return transactionPeriod;
        }
        protected void updateVendorBalances(IList items)
        {
            foreach (EventJournalItem item in items)
            {
                VendorBalance sc = item.VENDOR_BALANCE;
                if (sc.ID > 0)
                    VendorBalanceRepository.UpdateHeader(m_command, sc);
                else if (sc.ID == 0)
                    VendorBalanceRepository.SaveHeader(m_command, sc);
                if (item.VENDOR_BALANCE_ENTRY != null)
                {
                    VendorBalanceEntryRepository.Save(m_command, item.VENDOR_BALANCE_ENTRY);
                }
            }
        }
        protected void updateVendorBalances(VendorBalance vb)
        {
            if (vb.ID > 0)
                VendorBalanceRepository.UpdateHeader(m_command, vb);
            else if (vb.ID == 0)
                VendorBalanceRepository.SaveHeader(m_command, vb);
        }
        protected void saveVendorBalanceEntry(VendorBalanceEntry vbe)
        {
            if (vbe != null)
            {
                VendorBalanceEntryRepository.Save(m_command, vbe);
            }
        }
        protected void deleteVendorBalanceEntry(VendorBalanceEntry vbe)
        {
            VendorBalanceEntryRepository.Delete(m_command, vbe);
        }
        protected void deleteVendorBalanceEntry(IList sces)
        {
            foreach (VendorBalanceEntry sce in sces)
            {
                VendorBalanceEntryRepository.Delete(m_command, sce);
            }
        }
        protected void SetVendorBalance(EventJournalItem item, Period period)
        {
            VendorBalance sc = VendorBalanceRepository.FindVendorBalanceHeader(m_command, item.VENDOR.ID, item.CURRENCY.ID, period.ID, item.VENDOR_BALANCE_TYPE);
            if (sc == null)
            {
                sc = VendorBalance.CreateVendorBalance(item, period);
            }
            item.VENDOR_BALANCE = sc;
        }

        abstract protected void doSave(EventJournal e);
        abstract protected void doSaveNoTransaction(EventJournal e);
        abstract protected void doUpdate(EventJournal e);
        abstract protected void doUpdateNoTransaction(EventJournal e);
        abstract protected void doDeleteNoTransaction(EventJournal e);
        abstract protected void doDelete(EventJournal e);
        abstract protected EventJournal doGet(int ID);
        abstract protected void doUpdateStatus(EventJournal e, bool p);

        public void Save(EventJournal e)
        {
            doSave(e);
        }
        public void SaveNoTransaction(EventJournal e)
        {
            doSaveNoTransaction(e);
        }
        public EventJournal Get(int ID)
        {
            return doGet(ID);
        }
        public void Update(EventJournal e)
        {
            doUpdate(e);
        }
        public void UpdateNoTransaction(EventJournal e)
        {
            doUpdateNoTransaction(e);
        }
        public void DeleteNoTransaction(EventJournal e)
        {
            doDeleteNoTransaction(e);
        }
        public void Delete(EventJournal e)
        {
            doDelete(e);
        }
        public bool IsCodeExist(string code)
        {
            return doIsCodeExist(code);
        }
        public IList Search(string find)
        {
            return doSearch(find);
        }
        public void UpdateStatus(EventJournal e, bool p)
        {
            doUpdateStatus(e, p);
        }
        public virtual EventJournal FindLastCodeAndTransactionDate(string codesample)
        {
            throw new Exception("FindLastCodeAndTransactionDate is not implemented");
        }
    }
}
