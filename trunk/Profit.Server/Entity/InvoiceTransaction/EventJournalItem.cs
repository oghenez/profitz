using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public class EventJournalItem : IEntity
    {
        public int ID= 0;
        public EventJournal EVENT_JOURNAL;
        public Vendor VENDOR;
        public Currency CURRENCY;
        public double AMOUNT = 0;
        public VendorBalanceEntry VENDOR_BALANCE_ENTRY;
        public VendorBalance VENDOR_BALANCE;
        public VendorBalanceEntryType VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierOutStandingInvoice;
        public DateTime INVOICE_DATE = DateTime.Today;
        public string INVOICE_NO = string.Empty;
        public DateTime DUE_DATE = DateTime.Today;
        public Employee EMPLOYEE = null;
        public double DISCOUNT = 0;
        public double AMOUNT_BEFORE_DISCOUNT = 0;
        public TermOfPayment TOP = null;
        public string DESCRIPTION = string.Empty;
        public string NOTES = string.Empty;

        public EventJournalItem()
        {
        }
        public EventJournalItem(int id)
        {
            ID = id;
        }
        public void ProcessPosted()
        {
            asserNotNullVendorBalance();
            VENDOR_BALANCE.ProcessPosting(this);
        }
        public void ProcessUnPosted()
        {
            asserNotNullVendorBalance();
            VENDOR_BALANCE.ProcessUnPosting(this);
        }
        private void asserNotNullVendorBalance()
        {
            if (VENDOR_BALANCE == null)
                throw new Exception("Vendor Balance is not injected");
        }

        #region IEntity Members

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public void SetID(int id)
        {
            throw new NotImplementedException();
        }

        public string GetCode()
        {
            throw new NotImplementedException();
        }

        public void SetCode(string code)
        {
            throw new NotImplementedException();
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

        public System.Collections.IList GetAll(System.Data.Odbc.OdbcDataReader aReader)
        {
            throw new NotImplementedException();
        }

        public string GetConcatSearch(string find)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
