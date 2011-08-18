using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Profit.Server
{
    public abstract class EventJournal : IEntity
    {
        public int ID = 0;
        public string CODE = "---";
        public DateTime TRANSACTION_DATE;
        public DateTime NOTICE_DATE = DateTime.Now;
        public Vendor VENDOR;
        public Currency CURRENCY;
        public VendorBalanceEntryType VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierOutStandingInvoice;
        public string NOTES = string.Empty;
        public bool POSTED;
        public EventStatus EVENT_STATUS = EventStatus.Entry;
        public double SUBTOTAL_AMOUNT = 0;
        public double DISC_PERCENT = 0;
        public double AMOUNT_AFTER_DISC_PERCENT = 0;
        public double DISC_AMOUNT = 0;
        public double AMOUNT_AFTER_DISC_AMOUNT = 0;
        public double OTHER_EXPENSE = 0;
        public double NET_AMOUNT = 0;
        public Employee EMPLOYEE = null;
        IList m_deletedVendorBalanceEntry = new ArrayList();
        public IList m_eventJournalItems = new ArrayList();

        #region IEntity Members

        public int GetID()
        {
            return ID;
        }

        public void SetID(int id)
        {
            ID = id;
        }

        public string GetCode()
        {
            return CODE;
        }

        public void SetCode(string code)
        {
            CODE = code;
        }

        public virtual string GetInsertSQL()
        {
            throw new NotImplementedException();
        }

        public string GetDeleteSQL()
        {
            throw new NotImplementedException();
        }

        public virtual string GetUpdateSQL()
        {
            throw new NotImplementedException();
        }

        public string GetByIDSQL(int ID)
        {
            throw new NotImplementedException();
        }

        public string GetByCodeSQL(string code)
        {
            throw new NotImplementedException();
        }

        public string GetAllSQL()
        {
            throw new NotImplementedException();
        }

        public string GetMaximumIDSQL()
        {
            throw new NotImplementedException();
        }

        public string GetByCodeLikeSQL(string text)
        {
            throw new NotImplementedException();
        }

        public string GetByNameLikeSQL(string text)
        {
            throw new NotImplementedException();
        }

        public IEntity Get(System.Data.Odbc.OdbcDataReader aReader)
        {
            throw new NotImplementedException();
        }

        public IList GetAll(System.Data.Odbc.OdbcDataReader aReader)
        {
            throw new NotImplementedException();
        }

        public string GetConcatSearch(string find)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void ProcessPosting()
        {
            assertNotPosted();
            foreach (EventJournalItem eventItem in m_eventJournalItems)
            {
                eventItem.ProcessPosted();
            }
            this.EVENT_STATUS = EventStatus.Confirm;
            this.POSTED = true;
        }
        public void ProcessUnPosted()
        {
            assertNotUnPosted();
            foreach (EventJournalItem eventItem in m_eventJournalItems)
            {
                eventItem.ProcessUnPosted();
                m_deletedVendorBalanceEntry.Add(eventItem.VENDOR_BALANCE_ENTRY);//test
                eventItem.VENDOR_BALANCE_ENTRY = null;//test
            }
            this.EVENT_STATUS = EventStatus.Entry;
            this.POSTED = false;
        }
        public IList GetDeletedVendorBalanceEntry()
        {
            return m_deletedVendorBalanceEntry;
        }
        private void assertNotPosted()
        {
            if (this.EVENT_STATUS.Equals(EventStatus.Confirm))
                throw new Exception("Event allready Confirmed");
        }
        private void assertNotUnPosted()
        {
            if (this.EVENT_STATUS.Equals(EventStatus.Entry))
                throw new Exception("Event allready Revised");
        }
    }
}
