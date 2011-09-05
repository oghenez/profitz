using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class GoodReceiveNoteItem : EventItem
    {
        public string NOTES = string.Empty;
        public PurchaseOrderItem PO_ITEM;
        public AgainstStatus AGAINST_PR_STATUS = AgainstStatus.Open;
        public double OUTSTANDING_AMOUNT_TO_PR = 0;
        public double RETURNED_AMOUNT = 0;

        public GoodReceiveNoteItem() : base() { }
        public GoodReceiveNoteItem(int Id) : base(Id) { }
        public void SetOSAgainstPRItem(PurchaseReturnItem doi)
        {
            double qtyAmount = doi.GetAmountInSmallestUnit();// QYTAMOUNT;
            if (qtyAmount <= 0) return;
            if (AGAINST_PR_STATUS == AgainstStatus.Close)
                throw new Exception("GRN Item Allready Close :" + this.PART.NAME);
            if (qtyAmount > OUTSTANDING_AMOUNT_TO_PR)
                throw new Exception("PR Item Amount exceed Outstanding GRN Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_PR = OUTSTANDING_AMOUNT_TO_PR - qtyAmount;
            RETURNED_AMOUNT = RETURNED_AMOUNT + qtyAmount;
            if (isValidToClose())
                AGAINST_PR_STATUS = AgainstStatus.Close;
            else
                AGAINST_PR_STATUS = AgainstStatus.Outstanding;
            ((GoodReceiveNote)EVENT).UpdateAgainstPRStatusGRN();
        }
        public void UnSetOSAgainstPRItem(PurchaseReturnItem doi)
        {
            double qtyAmount = doi.GetAmountInSmallestUnit();  //QYTAMOUNT;
            if (qtyAmount > this.GetAmountInSmallestUnit() || OUTSTANDING_AMOUNT_TO_PR + qtyAmount > this.GetAmountInSmallestUnit())
                throw new Exception("PR Item revise Amount exceed GRN Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_PR = OUTSTANDING_AMOUNT_TO_PR + qtyAmount;
            RETURNED_AMOUNT = RETURNED_AMOUNT - qtyAmount;
            if (RETURNED_AMOUNT > 0)
                AGAINST_PR_STATUS = AgainstStatus.Outstanding;
            else
                AGAINST_PR_STATUS = AgainstStatus.Open;
            ((GoodReceiveNote)EVENT).UpdateAgainstPRStatusGRN();
        }
        private bool isValidToClose()
        {
            bool validA = OUTSTANDING_AMOUNT_TO_PR == 0;
            bool validB = RETURNED_AMOUNT == GetAmountInSmallestUnit();
            return validA && validB;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_goodreceivenoteitem 
                (   
                    grn_id,
                    part_id,
                    warehouse_id,
                    grni_amount,
                    sce_id,
                    grn_scentrytype,
                    sc_id,
                    unit_id,
                    grni_notes,
                    poi_id,
                    grni_againstprstatus,
                    grni_outstandingamtpr,
                    grni_returnedamount
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},'{8}',{9},'{10}',{11},{12})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.GoodReceiveNote.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                NOTES,
                PO_ITEM==null?0:PO_ITEM.ID,
                AGAINST_PR_STATUS.ToString(),
                GetAmountInSmallestUnit(), //OUTSTANDING_AMOUNT_TO_PR,
                0//RETURNED_AMOUNT
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_goodreceivenoteitem set 
                    grn_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    grni_amount = {3},
                    sce_id = {4},
                    grn_scentrytype = '{5}',
                    sc_id = {6},
                    unit_id = {7},
                    grni_notes = '{8}',
                    poi_id = {9},
                    grni_againstprstatus = '{10}',
                    grni_outstandingamtpr = {11},
                    grni_returnedamount = {12}
                where grni_id = {13}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.GoodReceiveNote.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                NOTES,
                PO_ITEM==null?0:PO_ITEM.ID,
                AGAINST_PR_STATUS.ToString(),
                GetAmountInSmallestUnit(),//OUTSTANDING_AMOUNT_TO_PR,
                RETURNED_AMOUNT,
                ID);
        }
        public static GoodReceiveNoteItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            GoodReceiveNoteItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new GoodReceiveNoteItem();
                transaction.ID = Convert.ToInt32(aReader["grni_id"]);
                transaction.EVENT = new GoodReceiveNote(Convert.ToInt32(aReader["grn_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["grni_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["grn_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.NOTES = aReader["grni_notes"].ToString();
                transaction.PO_ITEM = new PurchaseOrderItem(Convert.ToInt32(aReader["poi_id"]));
                transaction.AGAINST_PR_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["grni_againstprstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT_TO_PR = Convert.ToDouble(Convert.ToInt32(aReader["grni_outstandingamtpr"]));
                transaction.RETURNED_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["grni_returnedamount"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                GoodReceiveNoteItem transaction = new GoodReceiveNoteItem();
                transaction.ID = Convert.ToInt32(aReader["grni_id"]);
                transaction.EVENT = new GoodReceiveNote(Convert.ToInt32(aReader["grn_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["grni_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["grn_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.NOTES = aReader["grni_notes"].ToString();
                transaction.PO_ITEM = new PurchaseOrderItem(Convert.ToInt32(aReader["poi_id"]));
                transaction.AGAINST_PR_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["grni_againstprstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT_TO_PR = Convert.ToDouble(Convert.ToInt32(aReader["grni_outstandingamtpr"]));
                transaction.RETURNED_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["grni_returnedamount"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(grni_id) from table_goodreceivenoteitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_goodreceivenoteitem where grn_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (GoodReceiveNoteItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_goodreceivenoteitem where grn_id = {0} and grni_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_goodreceivenoteitem where grni_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_goodreceivenoteitem where grn_id = {0}", id);
        }
        public static string FindByPOItemIDSQL(int id)
        {
            return String.Format("SELECT * from table_goodreceivenoteitem where poi_id = {0}", id);
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"Update table_goodreceivenoteitem set 
                    grni_againstprstatus = '{0}',
                    grni_outstandingamtpr = {1},
                    grni_returnedamount = {2}
                    where grni_id = {3}", AGAINST_PR_STATUS.ToString(),
                                       OUTSTANDING_AMOUNT_TO_PR,
                                       RETURNED_AMOUNT,
                                       ID);
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_goodreceivenoteitem where grni_id = {0}", id);
        }
        public static string GetByPartIDSQL(int id)
        {
            return String.Format("SELECT * from table_goodreceivenoteitem where part_id = {0}", id);
        }
        public static string GetSearchByPartAndGRNNo(string find, int supplierID, string poi, DateTime trdate)
        {
            return String.Format(@"SELECT t.*
                FROM table_goodreceivenoteitem t
                INNER JOIN table_goodreceivenote p on p.grn_id = t.grn_id
                INNER JOIN table_part pt on pt.part_id = t.part_id
                where t.grni_outstandingamtpr > 0
                and concat(pt.part_code, pt.part_name, p.grn_code) like '%{0}%' and p.sup_id = {1}  
                and p.grn_posted = true
                and p.grn_date <= '{2}'
               {3}", find, supplierID, trdate.ToString(Utils.DATE_FORMAT), poi != "" ? " and t.grni_id not in (" + poi + ")" : "");
        }
        public static string GetGRNItemBySuppDate(int supID, DateTime date, string notInGRNItem)
        {
            return String.Format(@"SELECT t.*
                FROM table_goodreceivenoteitem t
                INNER JOIN table_goodreceivenote p on p.grn_id = t.grn_id
                INNER JOIN table_part pt on pt.part_id = t.part_id
                where t.grni_outstandingamtpr > 0
                and p.sup_id = {0}  
                and p.grn_posted = true
                and p.grn_date <= '{1}'
                {2}", 
                    supID, 
                    date.ToString(Utils.DATE_FORMAT),
                    notInGRNItem != "" ? " and t.grni_id not in (" + notInGRNItem + ")" : "");
        }
        public override bool Equals(object obj)
        {
            GoodReceiveNoteItem e = (GoodReceiveNoteItem)obj;
            if (e == null) return false;
            return e.ID == this.ID;
        }
        public static string GetOutstandingReturnSQL(int id)
        {
            return String.Format("SELECT grni_outstandingamtpr from table_goodreceivenoteitem where grni_id = {0}", id);
        }
        public static string GetReturnSQL(int id)
        {
            return String.Format("SELECT grni_returnedamount from table_goodreceivenoteitem where grni_id = {0}", id);
        }
    }
}
