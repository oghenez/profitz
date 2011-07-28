using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class StockCardEntry
    {
        
        public int ID=0;
        public StockCard STOCK_CARD;
        public StockCardEntryType STOCK_CARD_ENTRY_TYPE;
        public DateTime TRANSACTION_DATE;
        public Unit UNIT;
        public double AMOUNT;
        public EventItem EVENT_ITEM;

        public bool UPDATED = false;

        public StockCardEntry()
        { }
        public StockCardEntry(int id)
        {
            ID = id;
        }
        public StockCardEntry(StockCard stockCard, EventItem item)
        {
            STOCK_CARD = stockCard;
            STOCK_CARD_ENTRY_TYPE = item.STOCK_CARD_ENTRY_TYPE;
            TRANSACTION_DATE = item.EVENT.TRANSACTION_DATE;
            AMOUNT = item.GetAmountInSmallestUnit();
            UNIT = item.PART.UNIT;
            item.STOCK_CARD_ENTRY = this;
            EVENT_ITEM = item;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_stockcardentry
                (   
                    sc_id,
                    sce_stockcardentrytype,
                    sce_date,
                    unit_id,
                    sce_amount,
                    eventitem_id
                ) 
                VALUES ({0},'{1}','{2}',{3},{4},{5})",
                STOCK_CARD.ID,
                STOCK_CARD_ENTRY_TYPE.ToString(),
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                UNIT.ID,
                AMOUNT,
                EVENT_ITEM.ID
                );
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_stockcardentry set 
                    sc_id = {0},
                    sce_stockcardentrytype= '{1}',
                    sce_date= '{2}',
                    unit_id= {3},
                    sce_amount= {4},
                    eventitem_id= {5}
                where sce_id = {6}",
                STOCK_CARD.ID,
                STOCK_CARD_ENTRY_TYPE.ToString(),
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                UNIT.ID,
                AMOUNT,
                EVENT_ITEM.ID,
                ID);
        }
        public static StockCardEntry TransformReader(OdbcDataReader aReader)
        {
            StockCardEntry tr = null;
            while (aReader.Read())
            {
                tr = new StockCardEntry();
                tr.ID = Convert.ToInt32(aReader["sce_id"]);
                tr.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                tr.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["sce_stockcardentrytype"].ToString());
                tr.TRANSACTION_DATE = Convert.ToDateTime(aReader["sce_date"]);
                tr.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                tr.AMOUNT = Convert.ToDouble(aReader["sce_amount"]);
                tr.EVENT_ITEM = new EventItem(Convert.ToInt32(aReader["eventitem_id"]));
            }
            return tr;
        }
        public static IList TransformReaderList(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                StockCardEntry tr = new StockCardEntry();
                tr.ID = Convert.ToInt32(aReader["sce_id"]);
                tr.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                tr.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["sce_stockcardentrytype"].ToString());
                tr.TRANSACTION_DATE = Convert.ToDateTime(aReader["sce_date"]);
                tr.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                tr.AMOUNT = Convert.ToDouble(aReader["sce_amount"]);
                tr.EVENT_ITEM = new EventItem(Convert.ToInt32(aReader["eventitem_id"]));
                result.Add(tr);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(sce_id) from table_stockcardentry");
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_stockcardentry where sce_id ={0}", id);
        }
        public static string FindByEventItem(int id, StockCardEntryType type)
        {
            return String.Format("Select * from table_stockcardentry where eventitem_id ={0} and sce_stockcardentrytype = '{1}'", id, type.ToString());
        }
        public static string FindByStockCard(int id)
        {
            return String.Format("Select * from table_stockcardentry where sc_id ={0}", id);
        }
        public override bool Equals(object obj)
        {
            StockCardEntry sce = (StockCardEntry)obj;
            if (sce == null) return false;
            return sce.ID == ID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
