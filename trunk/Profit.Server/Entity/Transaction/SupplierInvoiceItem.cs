using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class SupplierInvoiceItem : EventItem
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
        public GoodReceiveNoteItem GRN_ITEM = null;

        public SupplierInvoiceItem() : base() { }
        public SupplierInvoiceItem(int ID) : base(ID) { }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_supplierinvoiceitem 
                (   
                    si_id,
                    part_id,
                    warehouse_id,
                    sii_amount,
                    sce_id,
                    sii_scentrytype,
                    sc_id,
                    unit_id,
                    sii_price,
                    sii_discpercent,
                    sii_discamount,
                    sii_totaldisc,
                    sii_subtotal,
                    sii_notes,
                    sii_disca,
                    sii_discb,
                    sii_discc,
                    sii_discabc,
                    grni_id
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},{8},{9},{10},{11},{12},'{13}',{14},
                    {15},{16},'{17}',{18})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.SupplierInvoice.ToString(),
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
                GRN_ITEM==null?0:GRN_ITEM.ID
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_supplierinvoiceitem set 
                    si_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    sii_amount = {3},
                    sce_id = {4},
                    sii_scentrytype = '{5}',
                    sc_id = {6},
                    unit_id = {7},
                    sii_price = {8},
                    sii_discpercent = {9},
                    sii_discamount = {10},
                    sii_totaldisc = {11},
                    sii_subtotal = {12},
                    sii_notes = '{13}',
                    sii_disca = {14},
                    sii_discb = {15},
                    sii_discc = {16},
                    sii_discabc = '{17}',
                    grni_id = {18}
                where sii_id = {19}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.SupplierInvoice.ToString(),
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
                GRN_ITEM==null?0:GRN_ITEM.ID,
                ID);
        }
        public static SupplierInvoiceItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            SupplierInvoiceItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new SupplierInvoiceItem();
                transaction.ID = Convert.ToInt32(aReader["sii_id"]);
                transaction.EVENT = new SupplierInvoice(Convert.ToInt32(aReader["si_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["sii_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["sii_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["sii_price"]));
                transaction.DISC_PERCENT = Convert.ToDouble(Convert.ToInt32(aReader["sii_discpercent"]));
                transaction.DISC_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["sii_discamount"]));
                transaction.TOTAL_DISCOUNT = Convert.ToDouble(Convert.ToInt32(aReader["sii_totaldisc"]));
                transaction.SUBTOTAL = Convert.ToDouble(Convert.ToInt32(aReader["sii_subtotal"]));
                transaction.NOTES = aReader["sii_notes"].ToString();
                transaction.DISC_A = Convert.ToDouble(Convert.ToInt32(aReader["sii_disca"]));
                transaction.DISC_B = Convert.ToDouble(Convert.ToInt32(aReader["sii_discb"]));
                transaction.DISC_C = Convert.ToDouble(Convert.ToInt32(aReader["sii_discc"]));
                transaction.DISC_ABC = aReader["sii_discabc"].ToString();
                transaction.GRN_ITEM = new GoodReceiveNoteItem(Convert.ToInt32(aReader["grni_id"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                SupplierInvoiceItem transaction = new SupplierInvoiceItem();
                transaction.ID = Convert.ToInt32(aReader["sii_id"]);
                transaction.EVENT = new SupplierInvoice(Convert.ToInt32(aReader["si_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["sii_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["sii_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["sii_price"]));
                transaction.DISC_PERCENT = Convert.ToDouble(Convert.ToInt32(aReader["sii_discpercent"]));
                transaction.DISC_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["sii_discamount"]));
                transaction.TOTAL_DISCOUNT = Convert.ToDouble(Convert.ToInt32(aReader["sii_totaldisc"]));
                transaction.SUBTOTAL = Convert.ToDouble(Convert.ToInt32(aReader["sii_subtotal"]));
                transaction.NOTES = aReader["sii_notes"].ToString();
                transaction.DISC_A = Convert.ToDouble(Convert.ToInt32(aReader["sii_disca"]));
                transaction.DISC_B = Convert.ToDouble(Convert.ToInt32(aReader["sii_discb"]));
                transaction.DISC_C = Convert.ToDouble(Convert.ToInt32(aReader["sii_discc"]));
                transaction.DISC_ABC = aReader["sii_discabc"].ToString();
                transaction.GRN_ITEM = new GoodReceiveNoteItem(Convert.ToInt32(aReader["grni_id"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(sii_id) from table_supplierinvoiceitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_supplierinvoiceitem where si_id = {0}", id);
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_supplierinvoiceitem where sii_id = {0}", id);
        }
        public static string GetByPartIDSQL(int id)
        {
            return String.Format("SELECT * from table_supplierinvoiceitem where part_id = {0}", id);
        }
        public static string GetByPartIDOrderByDateSQL(int id)
        {
            return String.Format(@"SELECT i.* from table_supplierinvoice h, 
            table_supplierinvoiceitem i 
            where 
            h.si_id = i.si_id 
            and i.part_id = {0}
            and h.si_posted = True
            order by h.si_date asc",
             id);
        }
        public static string GetByPartIDOrderByDateRangeSQL(int id, DateTime start, DateTime end)
        {
            return String.Format(@"SELECT i.* from table_supplierinvoice h, 
            table_supplierinvoiceitem i 
            where 
            h.si_id = i.si_id 
            and i.part_id = {0}
            and h.si_posted = True
            and h.si_date between '{1}' and '{2}'
            order by h.si_date asc",
             id, start.ToString(Utils.DATE_FORMAT_SHORT), end.ToString(Utils.DATE_FORMAT_SHORT));
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (SupplierInvoiceItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_supplierinvoiceitem where si_id = {0} and sii_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_supplierinvoiceitem where sii_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_supplierinvoiceitem where si_id = {0}", id);
        }
        public string UpdateAgainstStatus()
        {
            return "";
        }
        public static string GetSearchByPartAndPONo(string find, int supplierID, string poi, DateTime trdate)
        {
            return String.Format(@"SELECT t.*
                FROM table_supplierinvoiceitem t
                INNER JOIN table_supplierinvoice p on p.si_id = t.si_id
                INNER JOIN table_part pt on pt.part_id = t.part_id
                where t.sii_outstandingamounttogrn > 0
                and concat(pt.part_code, pt.part_name, p.si_code) like '%{0}%' and p.sup_id = {1}  
                and p.si_posted = true
                and p.si_date <= '{2}'
               {3}", find, supplierID, trdate.ToString(Utils.DATE_FORMAT), poi!=""?" and t.sii_id not in ("+poi+")":"");
        }
        public override bool Equals(object obj)
        {
            SupplierInvoiceItem e = (SupplierInvoiceItem)obj;
            if (e == null) return false;
            return e.ID == this.ID;
        }

        internal static string GetTheLatestPOPrice(int supID, int partID, int unitID)
        {
            return String.Format(@"SELECT t.sii_price
                FROM table_supplierinvoiceitem t
                INNER JOIN table_supplierinvoice p on p.si_id = t.si_id
                where p.sup_id = {0} and t.part_id = {1} and t.unit_id = {2}
                order by p.si_date desc
                ", supID, partID, unitID);
        }
        internal static string GetGRNUseBySupplierInvoice()
        {
            return "select distinct(t.grni_id) FROM table_supplierinvoiceitem t where t.grni_id != 0";
        }
        internal static string GetGRNUseBySupplierInvoice(int grnID)
        {
            return string.Format("select * FROM table_supplierinvoiceitem t where t.grni_id = {0}", grnID);
        }
        internal static string GetGRNUseBySupplierInvoice(IList grnids)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (GoodReceiveNoteItem i in grnids)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = grnids.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return string.Format("Select Count(*) from table_supplierinvoiceitem where grni_id in ({0})", pois);
        }
    }
}
