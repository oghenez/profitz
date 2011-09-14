using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class POSItem : EventItem
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
        public DeliveryOrderItem DO_ITEM = null;

        public POSItem() : base() { }
        public POSItem(int ID) : base(ID) { }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_positem 
                (   
                    pos_id,
                    part_id,
                    warehouse_id,
                    posi_amount,
                    sce_id,
                    posi_scentrytype,
                    sc_id,
                    unit_id,
                    posi_price,
                    posi_discpercent,
                    posi_discamount,
                    posi_totaldisc,
                    posi_subtotal,
                    posi_notes,
                    posi_disca,
                    posi_discb,
                    posi_discc,
                    posi_discabc,
                    doi_id
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},{8},{9},{10},{11},{12},'{13}',{14},
                    {15},{16},'{17}',{18})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.POS.ToString(),
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
                DO_ITEM==null?0:DO_ITEM.ID
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_positem set 
                    pos_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    posi_amount = {3},
                    sce_id = {4},
                    posi_scentrytype = '{5}',
                    sc_id = {6},
                    unit_id = {7},
                    posi_price = {8},
                    posi_discpercent = {9},
                    posi_discamount = {10},
                    posi_totaldisc = {11},
                    posi_subtotal = {12},
                    posi_notes = '{13}',
                    posi_disca = {14},
                    posi_discb = {15},
                    posi_discc = {16},
                    posi_discabc = '{17}',
                    doi_id = {18}
                where posi_id = {19}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.POS.ToString(),
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
                DO_ITEM==null?0:DO_ITEM.ID,
                ID);
        }
        public static POSItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            POSItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new POSItem();
                transaction.ID = Convert.ToInt32(aReader["posi_id"]);
                transaction.EVENT = new POS(Convert.ToInt32(aReader["pos_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["posi_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["posi_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["posi_price"]));
                transaction.DISC_PERCENT = Convert.ToDouble(Convert.ToInt32(aReader["posi_discpercent"]));
                transaction.DISC_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["posi_discamount"]));
                transaction.TOTAL_DISCOUNT = Convert.ToDouble(Convert.ToInt32(aReader["posi_totaldisc"]));
                transaction.SUBTOTAL = Convert.ToDouble(Convert.ToInt32(aReader["posi_subtotal"]));
                transaction.NOTES = aReader["posi_notes"].ToString();
                transaction.DISC_A = Convert.ToDouble(Convert.ToInt32(aReader["posi_disca"]));
                transaction.DISC_B = Convert.ToDouble(Convert.ToInt32(aReader["posi_discb"]));
                transaction.DISC_C = Convert.ToDouble(Convert.ToInt32(aReader["posi_discc"]));
                transaction.DISC_ABC = aReader["posi_discabc"].ToString();
                transaction.DO_ITEM = new DeliveryOrderItem(Convert.ToInt32(aReader["doi_id"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                POSItem transaction = new POSItem();
                transaction.ID = Convert.ToInt32(aReader["posi_id"]);
                transaction.EVENT = new POS(Convert.ToInt32(aReader["pos_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["posi_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["posi_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["posi_price"]));
                transaction.DISC_PERCENT = Convert.ToDouble(Convert.ToInt32(aReader["posi_discpercent"]));
                transaction.DISC_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["posi_discamount"]));
                transaction.TOTAL_DISCOUNT = Convert.ToDouble(Convert.ToInt32(aReader["posi_totaldisc"]));
                transaction.SUBTOTAL = Convert.ToDouble(Convert.ToInt32(aReader["posi_subtotal"]));
                transaction.NOTES = aReader["posi_notes"].ToString();
                transaction.DISC_A = Convert.ToDouble(Convert.ToInt32(aReader["posi_disca"]));
                transaction.DISC_B = Convert.ToDouble(Convert.ToInt32(aReader["posi_discb"]));
                transaction.DISC_C = Convert.ToDouble(Convert.ToInt32(aReader["posi_discc"]));
                transaction.DISC_ABC = aReader["posi_discabc"].ToString();
                transaction.DO_ITEM = new DeliveryOrderItem(Convert.ToInt32(aReader["doi_id"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(posi_id) from table_positem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_positem where pos_id = {0}", id);
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_positem where posi_id = {0}", id);
        }
        public static string GetByPartIDSQL(int id)
        {
            return String.Format("SELECT * from table_positem where part_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (POSItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_positem where pos_id = {0} and posi_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_positem where posi_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_positem where pos_id = {0}", id);
        }
        public string UpdateAgainstStatus()
        {
            return "";
        }
        public static string GetSearchByPartAndPONo(string find, int CustomerID, string poi, DateTime trdate)
        {
            return "";
//            return String.Format(@"SELECT t.*
//                FROM table_positem t
//                INNER JOIN table_pos p on p.pos_id = t.pos_id
//                INNER JOIN table_part pt on pt.part_id = t.part_id
//                where t.posi_outstandingamounttodo > 0
//                and concat(pt.part_code, pt.part_name, p.pos_code) like '%{0}%' and p.cus_id = {1}  
//                and p.pos_posted = true
//                and p.pos_date <= '{2}'
//               {3}", find, CustomerID, trdate.ToString(Utils.DATE_FORMAT), poi!=""?" and t.posi_id not in ("+poi+")":"");
        }
        public override bool Equals(object obj)
        {
            POSItem e = (POSItem)obj;
            if (e == null) return false;
            return e.ID == this.ID;
        }

        internal static string GetTheLatestSOPrice(int cusID, int partID, int unitID)
        {
            return String.Format(@"SELECT t.posi_price
                FROM table_positem t
                INNER JOIN table_pos p on p.pos_id = t.pos_id
                where p.cus_id = {0} and t.part_id = {1} and t.unit_id = {2}
                order by p.pos_date desc
                ", cusID, partID, unitID);
        }
        internal static string GetDOUseByPOS()
        {
            return "select distinct(t.doi_id) FROM table_positem t where t.doi_id != 0";
        }
        internal static string GetDOUseByPOS(int grnID)
        {
            return string.Format("select * FROM table_positem t where t.doi_id = {0}", grnID);
        }
        internal static string GetDOUseByPOS(IList grnids)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (DeliveryOrderItem i in grnids)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = grnids.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return string.Format("Select Count(*) from table_positem where doi_id in ({0})", pois);
        }
    }
}
