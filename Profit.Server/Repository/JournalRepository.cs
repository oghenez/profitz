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
        protected OdbcConnection m_connection = new OdbcConnection("Driver={MySQL ODBC 5.1 Driver};server=localhost;database=profit_db;uid=root;pwd=1234");
        protected OdbcCommand m_command = new OdbcCommand();
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
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                EventJournal events = this.Get(id);
                if (events.POSTED) 
                    throw new Exception("Status is already Posted/Confirm");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                doConfirm(events, p);
                events.ProcessPosting();
                this.UpdateStatus(events, true);
                updateVendorBalances(events.EVENT_JOURNAL_ITEMS);
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        public void Revise(int id)
        {
            OdbcTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                EventJournal events = this.Get(id);
                if (events.EVENT_STATUS == EventStatus.Entry)
                    throw new Exception("Status is already Unposted/Entry");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                doRevise(events, p);
                events.ProcessUnPosted();
                this.UpdateStatus(events, false);
                deleteVendorBalanceEntry(events.DELETED_VENDORBALANCEENTRY);
                updateVendorBalances(events.EVENT_JOURNAL_ITEMS);
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
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
        abstract protected void doUpdate(EventJournal e);
        abstract protected void doDelete(EventJournal e);
        abstract protected EventJournal doGet(int ID);
        abstract protected void doUpdateStatus(EventJournal e, bool p);

        public void Save(EventJournal e)
        {
            doSave(e);
        }
        public EventJournal Get(int ID)
        {
            return doGet(ID);
        }
        public void Update(EventJournal e)
        {
            doUpdate(e);
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
