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
        public DateTime TRANSACTION_DATE;
        public DateTime NOTICE_DATE = DateTime.Now;
        public StockCardEntryType STOCK_CARD_ENTRY_TYPE = StockCardEntryType.PurchaseOrder;
        public IList EVENT_ITEMS = new ArrayList();
        public Employee EMPLOYEE;
        public string NOTES = string.Empty;
        public bool POSTED;
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

        public virtual string GetByIDSQL(int ID)
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
    }
}
