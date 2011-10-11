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
        public double BALANCE = 0;
        public double BOOKED = 0;
        public double BACK_ORDER = 0;
        public IList STOCK_CARD_ENTRIES = new ArrayList();
        double BALANCE_AVAILABLE = 0;
        double BOOK_AVAILABLE = 0;
        double BACK_ORDER_AVAILABLE = 0;
        double BALANCE_AVAILABLE_RECALCULATE = 0;
        double BOOK_AVAILABLE_RECALCULATE = 0;
        double BACK_ORDER_AVAILABLE_RECALCULATE = 0;

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
            recalculateAvailable();
            switch (item.STOCK_CARD_ENTRY_TYPE)
            {
                case StockCardEntryType.OpeningStock:
                    BALANCE += qty;
                    BALANCE_AVAILABLE_RECALCULATE += qty;
                    break;
                case StockCardEntryType.StockTaking:
                    BALANCE += qty;
                    BALANCE_AVAILABLE_RECALCULATE += qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.PurchaseOrder:
                    BACK_ORDER += qty;
                    BACK_ORDER_AVAILABLE_RECALCULATE += qty;
                    break;
                case StockCardEntryType.GoodReceiveNote:
                    BALANCE += qty;
                    BALANCE_AVAILABLE_RECALCULATE += qty;
                    BACK_ORDER -= qty;
                    BACK_ORDER_AVAILABLE_RECALCULATE -= qty;
                    assertNotMinusBackOrderStock();
                    break;
                case StockCardEntryType.SalesOrder:
                    BOOKED += qty;
                    BOOK_AVAILABLE_RECALCULATE += qty;
                    break;
                case StockCardEntryType.DeliveryOrder:
                    BOOKED -= qty;
                    BOOK_AVAILABLE_RECALCULATE -= qty;
                    assertNotMinusBookedStock();
                    BALANCE -= qty;
                    BALANCE_AVAILABLE_RECALCULATE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.PurchaseReturn:
                    BALANCE -= qty;
                    BALANCE_AVAILABLE_RECALCULATE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.SalesReturn:
                    BALANCE += qty;
                    BALANCE_AVAILABLE_RECALCULATE += qty;
                    break;
                case StockCardEntryType.SupplierInvoice:
                    SupplierInvoiceItem sit = (SupplierInvoiceItem)item;
                    if (sit.GRN_ITEM != null)
                    {
                        if (sit.GRN_ITEM.ID == 0)
                        {
                            BALANCE += qty;
                            BALANCE_AVAILABLE_RECALCULATE += qty;
                        }
                    }
                    break;
                case StockCardEntryType.CustomerInvoice:
                    CustomerInvoiceItem cit = (CustomerInvoiceItem)item;
                    if (cit.DO_ITEM != null)
                    {
                        if (cit.DO_ITEM.ID == 0)
                        {
                            BALANCE -= qty;
                            BALANCE_AVAILABLE_RECALCULATE -= qty;
                            assertNotMinusBalanceStock();
                        }
                    }
                    break;
                case StockCardEntryType.POS:
                    BALANCE -= qty;
                    BALANCE_AVAILABLE_RECALCULATE -= qty;
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
            recalculateAvailable();
            switch (item.STOCK_CARD_ENTRY_TYPE)
            {
                case StockCardEntryType.OpeningStock:
                    BALANCE -= qty;
                    BALANCE_AVAILABLE_RECALCULATE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.StockTaking:
                    BALANCE -= qty;
                    BALANCE_AVAILABLE_RECALCULATE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.PurchaseOrder:
                    BACK_ORDER -= qty;
                    BACK_ORDER_AVAILABLE_RECALCULATE -= qty;
                    assertNotMinusBackOrderStock();
                    break;
                case StockCardEntryType.GoodReceiveNote:
                    BALANCE -= qty;
                    BALANCE_AVAILABLE_RECALCULATE -= qty;
                    BACK_ORDER += qty;
                    BACK_ORDER_AVAILABLE_RECALCULATE += qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.SalesOrder:
                    BOOKED -= qty;
                    BOOK_AVAILABLE_RECALCULATE -= qty;
                    assertNotMinusBookedStock();
                    break;
                case StockCardEntryType.DeliveryOrder:
                    BOOKED += qty;
                    BOOK_AVAILABLE_RECALCULATE += qty;
                    BALANCE += qty;
                    BALANCE_AVAILABLE_RECALCULATE += qty;
                    break;
                case StockCardEntryType.PurchaseReturn:
                    BALANCE += qty;
                    BALANCE_AVAILABLE_RECALCULATE += qty;
                    break;
                case StockCardEntryType.SalesReturn:
                    BALANCE -= qty;
                    BALANCE_AVAILABLE_RECALCULATE -= qty;
                    assertNotMinusBalanceStock();
                    break;
                case StockCardEntryType.SupplierInvoice:
                    SupplierInvoiceItem sit = (SupplierInvoiceItem)item;
                    if (sit.GRN_ITEM != null)
                    {
                        if (sit.GRN_ITEM.ID == 0)
                        {
                            BALANCE -= qty;
                            BALANCE_AVAILABLE_RECALCULATE -= qty;
                            assertNotMinusBalanceStock();
                        }
                    }
                    break;
                    
                case StockCardEntryType.CustomerInvoice:
                     CustomerInvoiceItem cit = (CustomerInvoiceItem)item;
                     if (cit.DO_ITEM != null)
                     {
                         if (cit.DO_ITEM.ID == 0)
                         {
                             BALANCE += qty;
                             BALANCE_AVAILABLE_RECALCULATE += qty;
                         }
                     }
                    break;
                case StockCardEntryType.POS:
                    BALANCE += qty;
                    BALANCE_AVAILABLE_RECALCULATE += qty;
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
            if (BALANCE_AVAILABLE_RECALCULATE < 0)
                throw new Exception(@"
            Stok barang (" + PART.NAME + ") kurang di Gudang (" + WAREHOUSE.NAME + @"),
            Stok yang ada : " + BALANCE_AVAILABLE + " " + PART.UNIT.CODE);

            //Stock Card (" + PART.NAME + "): Minus Balance Stock Stock On Warehouse (" + WAREHOUSE.NAME + "). \r\n Available Balance : " + BALANCE_AVAILABLE + " " + PART.UNIT.CODE);
                //throw new Exception(" Stock Card (" + PART.NAME + "): Minus Balance Stock Stock On Warehouse (" + WAREHOUSE.NAME + "). \r\n Available Balance : " + BALANCE_AVAILABLE + " " + PART.UNIT.CODE);
        }
        private void assertNotMinusBackOrderStock()
        {
            if (BACK_ORDER_AVAILABLE_RECALCULATE < 0)
                throw new Exception(@"
            Stok Order barang (" + PART.NAME + ") kurang di Gudang (" + WAREHOUSE.NAME + @"),
            Stok Order yang ada : " + BACK_ORDER_AVAILABLE + " " + PART.UNIT.CODE);
               // throw new Exception(" Stock Card (" + PART.NAME + "): Minus BackOder Stock On Warehouse (" + WAREHOUSE.NAME + "). Available BackOrder : " + BACK_ORDER_AVAILABLE + " " + PART.UNIT.CODE);
        }
        private void assertNotMinusBookedStock()
        {
            if (BOOK_AVAILABLE_RECALCULATE < 0)
                throw new Exception(@"
            Stok Booking barang (" + PART.NAME + ") kurang di Gudang (" + WAREHOUSE.NAME + @"),
            Stok Booking yang ada : " + BOOK_AVAILABLE + " " + PART.UNIT.CODE);
              //  throw new Exception(" Stock Card (" + PART.NAME + "): Minus Booked Stock Stock On Warehouse (" + WAREHOUSE.NAME + "). Available Booked : " + BOOK_AVAILABLE + " " + PART.UNIT.CODE);
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
        public static StockCard TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            StockCard stockcard = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                stockcard = new StockCard();
                stockcard.ID = Convert.ToInt32(aReader[0]);
                stockcard.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                stockcard.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                stockcard.PERIOD = new Period(Convert.ToInt32(aReader["period_id"]));
                stockcard.BALANCE =  Convert.ToDouble(aReader["sc_balance"]);
                stockcard.BACK_ORDER =  Convert.ToDouble(aReader["sc_backorder"]);
                stockcard.BOOKED = Convert.ToDouble(aReader["sc_booked"]);
                stockcard.BALANCE_AVAILABLE = stockcard.BALANCE;
                stockcard.BACK_ORDER_AVAILABLE = stockcard.BACK_ORDER;
                stockcard.BOOK_AVAILABLE = stockcard.BOOKED;
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
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
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
                stockcard.BALANCE_AVAILABLE = stockcard.BALANCE;
                stockcard.BACK_ORDER_AVAILABLE = stockcard.BACK_ORDER;
                stockcard.BOOK_AVAILABLE = stockcard.BOOKED;
                result.Add(stockcard);
            }
            return result;
        }
        public static IList TransforReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
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
                stockcard.BALANCE_AVAILABLE = stockcard.BALANCE;
                stockcard.BACK_ORDER_AVAILABLE = stockcard.BACK_ORDER;
                stockcard.BOOK_AVAILABLE = stockcard.BOOKED;
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
        public static string FindByPartPeriod(int part, int period)
        {
            return String.Format("SELECT * from table_stockcard where period_id = {0} and part_id = {1}",period,part);
        }

        private void recalculateAvailable()
        {
            BALANCE_AVAILABLE = 0;
            BOOK_AVAILABLE = 0;
            BACK_ORDER_AVAILABLE = 0;
            if (STOCK_CARD_ENTRIES.Count == 0) return;
            foreach (StockCardEntry item in STOCK_CARD_ENTRIES)
            {
                switch (item.STOCK_CARD_ENTRY_TYPE)
                {
                    case StockCardEntryType.OpeningStock:
                        BALANCE_AVAILABLE += item.AMOUNT;
                        break;
                    case StockCardEntryType.StockTaking:
                        BALANCE_AVAILABLE += item.AMOUNT;
                        assertNotMinusBalanceStock();
                        break;
                    case StockCardEntryType.PurchaseOrder:
                        BACK_ORDER_AVAILABLE += item.AMOUNT;
                        break;
                    case StockCardEntryType.GoodReceiveNote:
                        BALANCE_AVAILABLE += item.AMOUNT;
                        BACK_ORDER_AVAILABLE -= item.AMOUNT;
                        break;
                    case StockCardEntryType.SalesOrder:
                        BOOK_AVAILABLE += item.AMOUNT;
                        break;
                    case StockCardEntryType.DeliveryOrder:
                        BOOK_AVAILABLE -= item.AMOUNT;
                        BALANCE_AVAILABLE -= item.AMOUNT;
                        break;
                    case StockCardEntryType.PurchaseReturn:
                        BALANCE_AVAILABLE -= item.AMOUNT;
                        break;
                    case StockCardEntryType.SalesReturn:
                        BALANCE_AVAILABLE += item.AMOUNT;
                        break;
                    case StockCardEntryType.SupplierInvoice:
                        SupplierInvoiceItem sit = (SupplierInvoiceItem)item.EVENT_ITEM;
                        if (sit.GRN_ITEM != null)
                        {
                            if (sit.GRN_ITEM.ID == 0)
                                BALANCE += item.AMOUNT;
                        }
                        break;
                    case StockCardEntryType.CustomerInvoice:
                        CustomerInvoiceItem cit = (CustomerInvoiceItem)item.EVENT_ITEM;
                        if (cit.DO_ITEM != null)
                        {
                            if (cit.DO_ITEM.ID == 0)
                            {
                                BALANCE -= item.AMOUNT;
                            }
                        }
                        break;
                    case StockCardEntryType.POS:
                        BALANCE -= item.AMOUNT;
                        break;
                    default:
                        break;
                }
            }
            BALANCE_AVAILABLE_RECALCULATE = BALANCE_AVAILABLE;
            BOOK_AVAILABLE_RECALCULATE = BOOK_AVAILABLE;
            BACK_ORDER_AVAILABLE_RECALCULATE = BACK_ORDER_AVAILABLE;
        }
        #region IEntity Members


        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
