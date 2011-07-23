using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
