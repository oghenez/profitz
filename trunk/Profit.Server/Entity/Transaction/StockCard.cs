using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class StockCard : IEntity
    {
        public int ID = 0;
        public Part PART;
        public Warehouse WAREHOUSE;
        public Period PERIOD;
        double BALANCE = 0;
        double BOOKED = 0;
        double BACK_ORDER = 0;
        public IList STOCK_CARD_ENTRIES = new ArrayList();
        double BALANCE_AVAILABLE = 0;
        double BOOK_AVAILABLE = 0;
        double BACK_ORDER_AVAILABLE = 0;

        public StockCard()
        {
        }
        public StockCard(Part part, Warehouse location, Period period)
        {
            PART = part;
            WAREHOUSE = location;
            PERIOD = period;
        }
        public StockCard(int id)
        {
            ID = id;
        }
        public void ProcessConfirm(EventItem item)
        {
            assertSamePartLocation(item.PART, item.WAREHOUSE);
            assertPeriodIsCurrent();
            assertInPeriodRange(item.EVENT.TRANSACTION_DATE);
            double qty = item.GetAmountInSmallestUnit();
            switch (item.STOCK_CARD_ENTRY_TYPE)
            {
                case StockCardEntryType.OpeningStock:
                    BALANCE += qty;
                    break;
                case StockCardEntryType.StockTaking:
                    BALANCE += qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.PurchaseOrder:
                    BACK_ORDER += qty;
                    break;
                case StockCardEntryType.GoodReceiveNote:
                    BALANCE += qty;
                    BACK_ORDER -= qty;
                    assertNotMinusBackOrderStock();
                    break;
                case StockCardEntryType.SalesOrder:
                    BOOKED += qty;
                    break;
                case StockCardEntryType.DeliveryOrder:
                    BOOKED -= qty;
                    assertNotMinusBookedStock();
                    BALANCE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.PurchaseReturn:
                    BALANCE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.SalesReturn:
                    BALANCE += qty;
                    break;
                case StockCardEntryType.SupplierInvoice:
                    BALANCE += qty;
                    break;
                case StockCardEntryType.CustomerInvoice:
                    BALANCE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                default:
                    break;
            }
            createStockCardEntry(item);
        }
        public void ProcessRevise(EventItem item)
        {
            assertSamePartLocation(item.PART, item.WAREHOUSE);
            assertPeriodIsCurrent();
            assertInPeriodRange(item.EVENT.TRANSACTION_DATE);
            double qty = item.GetAmountInSmallestUnit();
            switch (item.STOCK_CARD_ENTRY_TYPE)
            {
                case StockCardEntryType.OpeningStock:
                    BALANCE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.StockTaking:
                    BALANCE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.PurchaseOrder:
                    BACK_ORDER -= qty;
                    assertNotMinusBackOrderStock();
                    break;
                case StockCardEntryType.GoodReceiveNote:
                    BALANCE -= qty;
                    BACK_ORDER += qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.SalesOrder:
                    BOOKED -= qty;
                    assertNotMinusBookedStock();
                    break;
                case StockCardEntryType.DeliveryOrder:
                    BOOKED += qty;
                    BALANCE += qty;
                    break;
                case StockCardEntryType.PurchaseReturn:
                    BALANCE += qty;
                    break;
                case StockCardEntryType.SalesReturn:
                    BALANCE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.SupplierInvoice:
                    BALANCE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.CustomerInvoice:
                    BALANCE += qty;
                    break;
                default:
                    break;
            }
            //removeStockCardEntry(item);
        }
        private void assertSamePartLocation(Part part, Warehouse location)
        {
            bool sameLocation = this.WAREHOUSE.ID == location.ID;
            bool samePart = this.PART.ID == part.ID;
            if (!(sameLocation && samePart))
                throw new Exception(" Stock Card : not same Part or Location ");
        }
        private void assertPeriodIsCurrent()
        {
            if (PERIOD.PERIOD_STATUS != PeriodStatus.Current)
                throw new Exception(" Stock Card : Period Status Not Current ");
        }
        private void assertInPeriodRange(DateTime date)
        {
            if (!PERIOD.IsInRange(date))
                throw new Exception(" Stock Card : Transaction Date in not in Period ");
        }
        private void assertNotMinusBalanceStock()
        {
            if (BALANCE < 0)
                throw new Exception(" Stock Card (" + PART.NAME + "): Minus Balance Stock Stock On Warehouse (" + WAREHOUSE.NAME + "). Available Balance : " + BALANCE_AVAILABLE + " " + PART.UNIT.CODE);
        }
        private void assertNotMinusBackOrderStock()
        {
            if (BACK_ORDER < 0)
                throw new Exception(" Stock Card (" + PART.NAME + "): Minus BackOder Stock On Warehouse (" + WAREHOUSE.NAME + "). Available BackOrder : " + BACK_ORDER_AVAILABLE + " " + PART.UNIT.CODE);
        }
        private void assertNotMinusBookedStock()
        {
            if (BOOKED < 0)
                throw new Exception(" Stock Card (" + PART.NAME + "): Minus Booked Stock Stock On Warehouse (" + WAREHOUSE.NAME + "). Available Booked : " + BOOK_AVAILABLE + " " + PART.UNIT.CODE);
        }
        private void createStockCardEntry(EventItem item)
        {
            StockCardEntry sce = new StockCardEntry(this, item);
            STOCK_CARD_ENTRIES.Add(sce);
        }
        public StockCard Create(Period newPeriod)
        {
            StockCard sCard = new StockCard(PART, WAREHOUSE, newPeriod);
            sCard.BALANCE = BALANCE;
            sCard.BACK_ORDER = BACK_ORDER;
            sCard.BOOKED = BOOKED;
            return sCard;
        }
        public static StockCard CreateStockCard(EventItem item, Period period)
        {
            return new StockCard(item.PART, item.WAREHOUSE, period);
        }
        public static StockCard TransformReader(OdbcDataReader aReader)
        {
            StockCard stockcard = null;
            while (aReader.Read())
            {
                stockcard = new StockCard();
                stockcard.ID = Convert.ToInt32(aReader[0]);
                stockcard.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                stockcard.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                stockcard.PERIOD = new Period(Convert.ToInt32(aReader["period_id"]));
                stockcard.BALANCE =  Convert.ToDouble(aReader["sc_balance"]);
                stockcard.BACK_ORDER =  Convert.ToDouble(aReader["sc_backorder"]);
                stockcard.BOOKED = Convert.ToDouble(aReader["sc_booked"]);
            }
            return stockcard;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_stockcard 
                (part_id,warehouse_id,period_id,sc_balance,sc_backorder,sc_booked) 
                VALUES ({0},{1},{2},{3},{4},{5})",
                PART.ID, WAREHOUSE.ID, PERIOD.ID, BALANCE, BACK_ORDER, BOOKED);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_stockcard where sc_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_stockcard set 
                part_id = {0},
                warehouse_id = {1},
                period_id = {2},
                sc_balance = {3},
                sc_backorder = {4},
                sc_booked = {5}
                where sc_id = {6}",
                PART.ID, WAREHOUSE.ID, PERIOD.ID, BALANCE, BACK_ORDER, BOOKED, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_stockcard where sc_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return "";//String.Format("select * from table_stockcard where bank_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return "";// String.Format("select * from table_stockcard where bank_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return "";// String.Format("select * from table_stockcard where bank_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_stockcard");
        }
        public string GetConcatSearch(string find)
        {
            return "";
        }
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                StockCard stockcard = new StockCard();
                stockcard.ID = Convert.ToInt32(aReader[0]);
                stockcard.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                stockcard.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                stockcard.PERIOD = new Period(Convert.ToInt32(aReader["period_id"]));
                stockcard.BALANCE = Convert.ToDouble(aReader["sc_balance"]);
                stockcard.BACK_ORDER = Convert.ToDouble(aReader["sc_backorder"]);
                stockcard.BOOKED = Convert.ToDouble(aReader["sc_booked"]);
                result.Add(stockcard);
            }
            return result;
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
            return "";
        }
        public void SetCode(string code)
        {
            
        }
        public string GetMaximumIDSQL()
        {
            return String.Format("SELECT max(sc_id) from table_stockcard");
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(sc_id) from table_stockcard");
        }
        #region IEntity Members


        public IEntity Get(OdbcDataReader aReader)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
