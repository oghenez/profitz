using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class SalesOrderItem : EventItem
    {
        public double PRICE = 0;
        public double DISC_PERCENT = 0;
        public double DISC_AMOUNT = 0;
        public double TOTAL_DISCOUNT = 0;
        public double SUBTOTAL = 0;
        public string NOTES = string.Empty;
        public double DISC_A = 0;
        public double DISC_B = 0;
        public double DISC_C = 0;
        public string DISC_ABC = string.Empty;
        public AgainstStatus AGAINST_DO_STATUS = AgainstStatus.Open;
        public double OUTSTANDING_AMOUNT_TO_DO = 0;
        public double DELIVERED_AMOUNT = 0;
        public double PRICE_IN_SMALLEST_UNIT = 0;

        public SalesOrderItem() : base() { }
        public SalesOrderItem(int ID) : base(ID) { }
        public void SetOSAgainstDOItem(DeliveryOrderItem grni)
        {
            double qtyAmount = grni.GetAmountInSmallestUnit();//grni.QYTAMOUNT;
            if (qtyAmount <= 0) return;
            if (AGAINST_DO_STATUS == AgainstStatus.Close)
                throw new Exception("SO Item already fully delivered :" + this.PART.NAME);
            if (qtyAmount > OUTSTANDING_AMOUNT_TO_DO)
                throw new Exception("DO Item Amount exceed SO Outstanding Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_DO = OUTSTANDING_AMOUNT_TO_DO - qtyAmount;
            DELIVERED_AMOUNT = DELIVERED_AMOUNT + qtyAmount;
            if (isValidToClose())
                AGAINST_DO_STATUS = AgainstStatus.Close;
            else
                AGAINST_DO_STATUS = AgainstStatus.Outstanding;
            ((SalesOrder)EVENT).UpdateAgainstDOStatusSO();
        }
        public void UnSetOSAgainstDOItem(DeliveryOrderItem grni)
        {
            double qtyAmount = grni.GetAmountInSmallestUnit();//grni.QYTAMOUNT;
            if (qtyAmount > this.GetAmountInSmallestUnit() || OUTSTANDING_AMOUNT_TO_DO + qtyAmount > this.GetAmountInSmallestUnit())
                throw new Exception("DO Item revise Amount exceed SO Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_DO = OUTSTANDING_AMOUNT_TO_DO + qtyAmount;
            DELIVERED_AMOUNT = DELIVERED_AMOUNT - qtyAmount;
           // if (OUTSTANDING_AMOUNT_TO_DO > 0)
            if (DELIVERED_AMOUNT > 0)
                AGAINST_DO_STATUS = AgainstStatus.Outstanding;
            else
                AGAINST_DO_STATUS = AgainstStatus.Open;
            ((SalesOrder)EVENT).UpdateAgainstDOStatusSO();
        }
        private bool isValidToClose()
        {
            bool validA = OUTSTANDING_AMOUNT_TO_DO == 0;
            bool validB = DELIVERED_AMOUNT == GetAmountInSmallestUnit();
            return validA && validB;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_salesorderitem 
                (   
                    so_id,
                    part_id,
                    warehouse_id,
                    soi_amount,
                    sce_id,
                    soi_scentrytype,
                    sc_id,
                    unit_id,
                    soi_price,
                    soi_discpercent,
                    soi_discamount,
                    soi_totaldisc,
                    soi_subtotal,
                    soi_notes,
                    soi_disca,
                    soi_discb,
                    soi_discc,
                    soi_discabc,
                    soi_againstdostatus,
                    soi_outstandingamounttodo,
                    soi_deliveredamount,
                    soi_priceinsmallestunit
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},{8},{9},{10},{11},{12},'{13}',{14},
                    {15},{16},'{17}','{18}',{19},{20},{21})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.SalesOrder.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                PRICE,
                DISC_PERCENT,
                DISC_AMOUNT,
                TOTAL_DISCOUNT,
                SUBTOTAL,
                NOTES,
                DISC_A,
                DISC_B,
                DISC_C,
                DISC_ABC,
                AGAINST_DO_STATUS.ToString(),
                GetAmountInSmallestUnit(),// QYTAMOUNT,//OUTSTANDING_AMOUNT_TO_DO,
                0,//DELIVERED_AMOUNT,
                UNIT.ID==PART.UNIT.ID?PRICE :PRICE / GetAmountInSmallestUnit()
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_salesorderitem set 
                    so_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    soi_amount = {3},
                    sce_id = {4},
                    soi_scentrytype = {5},
                    sc_id = {6},
                    unit_id = {7},
                    soi_price = {8},
                    soi_discpercent = {9},
                    soi_discamount = {10},
                    soi_totaldisc = {11},
                    soi_subtotal = {12},
                    soi_notes = {13},
                    soi_disca = {14},
                    soi_discb = {15},
                    soi_discc = {16},
                    soi_discabc = {17},
                    soi_againstdostatus = {18},
                    soi_outstandingamounttodo = {19},
                    soi_deliveredamount = {20},
                    soi_priceinsmallestunit = {21}
                where soi_id = {22}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.SalesOrder.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                PRICE,
                DISC_PERCENT,
                DISC_AMOUNT,
                TOTAL_DISCOUNT,
                SUBTOTAL,
                NOTES,
                DISC_A,
                DISC_B,
                DISC_C,
                DISC_ABC,
                AGAINST_DO_STATUS.ToString(),
                GetAmountInSmallestUnit(),// OUTSTANDING_AMOUNT_TO_DO,
                DELIVERED_AMOUNT,
                UNIT.ID == PART.UNIT.ID ? PRICE : PRICE / GetAmountInSmallestUnit(),
                ID);
        }
        public static SalesOrderItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            SalesOrderItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new SalesOrderItem();
                transaction.ID = Convert.ToInt32(aReader["soi_id"]);
                transaction.EVENT = new SalesOrder(Convert.ToInt32(aReader["so_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["soi_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["soi_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["soi_price"]));
                transaction.DISC_PERCENT = Convert.ToDouble(Convert.ToInt32(aReader["soi_discpercent"]));
                transaction.DISC_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["soi_discamount"]));
                transaction.TOTAL_DISCOUNT = Convert.ToDouble(Convert.ToInt32(aReader["soi_totaldisc"]));
                transaction.SUBTOTAL = Convert.ToDouble(Convert.ToInt32(aReader["soi_subtotal"]));
                transaction.NOTES = aReader["soi_notes"].ToString();
                transaction.DISC_A = Convert.ToDouble(Convert.ToInt32(aReader["soi_disca"]));
                transaction.DISC_B = Convert.ToDouble(Convert.ToInt32(aReader["soi_discb"]));
                transaction.DISC_C = Convert.ToDouble(Convert.ToInt32(aReader["soi_discc"]));
                transaction.DISC_ABC = aReader["soi_discabc"].ToString();
                transaction.AGAINST_DO_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["soi_againstdostatus"].ToString());
                transaction.OUTSTANDING_AMOUNT_TO_DO = Convert.ToDouble(Convert.ToInt32(aReader["soi_outstandingamounttodo"]));
                transaction.DELIVERED_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["soi_deliveredamount"]));
                transaction.PRICE_IN_SMALLEST_UNIT = Convert.ToDouble(Convert.ToInt32(aReader["soi_priceinsmallestunit"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                SalesOrderItem transaction = new SalesOrderItem();
                transaction.ID = Convert.ToInt32(aReader["soi_id"]);
                transaction.EVENT = new SalesOrder(Convert.ToInt32(aReader["so_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["soi_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["soi_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["soi_price"]));
                transaction.DISC_PERCENT = Convert.ToDouble(Convert.ToInt32(aReader["soi_discpercent"]));
                transaction.DISC_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["soi_discamount"]));
                transaction.TOTAL_DISCOUNT = Convert.ToDouble(Convert.ToInt32(aReader["soi_totaldisc"]));
                transaction.SUBTOTAL = Convert.ToDouble(Convert.ToInt32(aReader["soi_subtotal"]));
                transaction.NOTES = aReader["soi_notes"].ToString();
                transaction.DISC_A = Convert.ToDouble(Convert.ToInt32(aReader["soi_disca"]));
                transaction.DISC_B = Convert.ToDouble(Convert.ToInt32(aReader["soi_discb"]));
                transaction.DISC_C = Convert.ToDouble(Convert.ToInt32(aReader["soi_discc"]));
                transaction.DISC_ABC = aReader["soi_discabc"].ToString();
                transaction.AGAINST_DO_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["soi_againstdostatus"].ToString());
                transaction.OUTSTANDING_AMOUNT_TO_DO = Convert.ToDouble(Convert.ToInt32(aReader["soi_outstandingamounttodo"]));
                transaction.DELIVERED_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["soi_deliveredamount"]));
                transaction.PRICE_IN_SMALLEST_UNIT = Convert.ToDouble(Convert.ToInt32(aReader["soi_priceinsmallestunit"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(soi_id) from table_salesorderitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_salesorderitem where so_id = {0}", id);
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_salesorderitem where soi_id = {0}", id);
        }
        public static string GetByPartIDSQL(int id)
        {
            return String.Format("SELECT * from table_salesorderitem where part_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (SalesOrderItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_salesorderitem where so_id = {0} and soi_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_salesorderitem where soi_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_salesorderitem where so_id = {0}", id);
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"Update table_salesorderitem set 
                    soi_againstdostatus = '{0}',
                    soi_outstandingamounttodo = {1},
                    soi_deliveredamount = {2}
                    where soi_id = {3}", AGAINST_DO_STATUS.ToString(),
                                       OUTSTANDING_AMOUNT_TO_DO,
                                       DELIVERED_AMOUNT,
                                       ID); 
        }
        public static string GetSearchByPartAndSONo(string find, int customerID, string poi, DateTime trdate)
        {
            return String.Format(@"SELECT t.*
                FROM table_salesorderitem t
                INNER JOIN table_salesorder p on p.so_id = t.so_id
                INNER JOIN table_part pt on pt.part_id = t.part_id
                where t.soi_outstandingamounttodo > 0
                and concat(pt.part_code, pt.part_name, p.so_code) like '%{0}%' and p.cus_id = {1}  
                and p.so_posted = true
                and p.so_date <= '{2}'
               {3}", find, customerID, trdate.ToString(Utils.DATE_FORMAT), poi != "" ? " and t.soi_id not in (" + poi + ")" : "");
        }
        public override bool Equals(object obj)
        {
            SalesOrderItem e = (SalesOrderItem)obj;
            if (e == null) return false;
            return e.ID == this.ID;
        }

        internal static string GetTheLatestSOPrice(int cusID, int partID, int unitID)
        {
            return String.Format(@"SELECT t.soi_price
                FROM table_salesorderitem t
                INNER JOIN table_salesorder p on p.so_id = t.so_id
                where p.cus_id = {0} and t.part_id = {1} and t.unit_id = {2}
                order by p.so_date desc
                ", cusID, partID, unitID);
        }
        public static string GetOutstandingDeliveredSQL(int id)
        {
            return String.Format("SELECT soi_outstandingamounttodo from table_salesorderitem where soi_id = {0}", id);
        }
        public static string GetDeliveredSQL(int id)
        {
            return String.Format("SELECT soi_deliveredamount from table_salesorderitem where soi_id = {0}", id);
        }
    }
}
