using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class Event : IEntity
    {
        public int ID = 0;
        public string CODE = string.Empty;
        public DateTime TRANSACTION_DATE;
        public DateTime NOTICE_DATE = DateTime.Now;
        public StockCardEntryType STOCK_CARD_ENTRY_TYPE = StockCardEntryType.PurchaseOrder;
        public IList EVENT_ITEMS = new ArrayList();
        public Employee EMPLOYEE;
        public string NOTES = string.Empty;
        public bool POSTED;
        public string DOCUMENT_NO = string.Empty;
        public DateTime DOCUMENT_DATE = DateTime.Today;
        //bool m_byTransaction = true;

        public EventStatus EVENT_STATUS = EventStatus.Entry;
        public IList DELETED_STOCK_CARD_ENTRY = new ArrayList();
        public Event()
        { }

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

        public virtual string GetDeleteSQL()
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

        public virtual string GetAllSQL()
        {
            throw new NotImplementedException();
        }

        public virtual string GetMaximumIDSQL()
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

        public virtual IEntity Get(OdbcDataReader aReader)
        {
            throw new NotImplementedException();
        }

        public virtual IList GetAll(System.Data.Odbc.OdbcDataReader aReader)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override bool Equals(object obj)
        {
            IEntity e = (IEntity)obj;
            if (e == null) return false;
            return e.GetID() == ID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void ProcessConfirm()
        {
            assertNotConfirmed();
            foreach (EventItem eventItem in EVENT_ITEMS)
            {
                eventItem.ProcessConfirm();
            }
            this.EVENT_STATUS = EventStatus.Confirm;
            this.POSTED = true;
        }
        private void assertNotConfirmed()
        {
            if (this.EVENT_STATUS.Equals(EventStatus.Confirm))
                throw new Exception("Event allready Confirmed");
        }
        public void ProcessRevised()
        {
            assertNotRevised();
            DELETED_STOCK_CARD_ENTRY.Clear();
            foreach (EventItem eventItem in EVENT_ITEMS)
            {
                eventItem.ProcessRevise();
                DELETED_STOCK_CARD_ENTRY.Add(eventItem.STOCK_CARD_ENTRY);
                eventItem.STOCK_CARD_ENTRY = null;
            }
            this.EVENT_STATUS = EventStatus.Entry;
            this.POSTED = false;
        }
        private void assertNotRevised()
        {
            if (this.EVENT_STATUS.Equals(EventStatus.Entry))
                throw new Exception("Event allready Revised");
        }

        #region IEntity Members


        public string GetConcatSearch(string find)
        {
            return "";
        }

        #endregion
    }
}
