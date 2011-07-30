using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class EventItem : IEntity
    {
        public int ID = 0;
        public Event EVENT;
        public Part PART;
        public Warehouse WAREHOUSE;
        public double QYTAMOUNT;
        public StockCardEntry STOCK_CARD_ENTRY;
        public StockCardEntryType STOCK_CARD_ENTRY_TYPE;
        public StockCard STOCK_CARD;
        public Unit UNIT;

        public bool UPDATED = false; //for update

        public EventItem()
        { }
        public EventItem(int id)
        { ID = id; }
        public double GetAmountInSmallestUnit()
        {
            if (this.UNIT.ID == PART.UNIT.ID) return QYTAMOUNT;
            double conversion = PART.GetUnitConversion(UNIT.ID).ORIGINAL_QTY / PART.GetUnitConversion(UNIT.ID).CONVERSION_QTY;
            return conversion * QYTAMOUNT;
        }

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

        public virtual IList GetAll(OdbcDataReader aReader)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            EventItem e = (EventItem)obj;
            if (e == null) return false;
            return e.ID == ID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public void ProcessConfirm()
        {
            asserNotNullStockCard();
            STOCK_CARD.ProcessConfirm(this);
        }
        private void asserNotNullStockCard()
        {
            if (STOCK_CARD == null)
                throw new Exception("Stock Card is not injected");
        }
        public void ProcessRevise()
        {
            asserNotNullStockCard();
            STOCK_CARD.ProcessRevise(this);
        }

        #region IEntity Members


        public string GetConcatSearch(string find)
        {
            return "";
        }

        #endregion
    }
}
