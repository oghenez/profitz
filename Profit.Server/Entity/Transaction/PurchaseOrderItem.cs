using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class PurchaseOrderItem : EventItem
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
        public AgainstStatus AGAINST_GRN_STATUS = AgainstStatus.Open;
        public double OUTSTANDING_AMOUNT_TO_GRN = 0;
        public double RECEIVED_AMOUNT = 0;

        public PurchaseOrderItem() : base() { }
        public PurchaseOrderItem(int ID) : base(ID) { }
        public void SetOSAgainstGRNItem(GoodReceiveNoteItem grni)
        {
            double qtyAmount = grni.GetAmountInSmallestUnit();//grni.QYTAMOUNT;
            if (qtyAmount <= 0) return;
            if (AGAINST_GRN_STATUS == AgainstStatus.Close)
                throw new Exception("PO Item Allready Close :" + this.PART.NAME);
            if (qtyAmount > OUTSTANDING_AMOUNT_TO_GRN)
                throw new Exception("GRN Item Amount exceed PO Outstanding Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_GRN = OUTSTANDING_AMOUNT_TO_GRN - qtyAmount;
            RECEIVED_AMOUNT = RECEIVED_AMOUNT + qtyAmount;
            if (isValidToClose())
                AGAINST_GRN_STATUS = AgainstStatus.Close;
            else
                AGAINST_GRN_STATUS = AgainstStatus.Outstanding;
            ((PurchaseOrder)EVENT).UpdateAgainstGRNStatusPO();
        }
        public void UnSetOSAgainstGRNItem(GoodReceiveNoteItem grni)
        {
            double qtyAmount = grni.GetAmountInSmallestUnit();//= grni.QYTAMOUNT;
            if (qtyAmount > this.GetAmountInSmallestUnit() || OUTSTANDING_AMOUNT_TO_GRN + qtyAmount > this.GetAmountInSmallestUnit())
                throw new Exception("GRN Item revise Amount exceed PO Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_GRN = OUTSTANDING_AMOUNT_TO_GRN + qtyAmount;
            RECEIVED_AMOUNT = RECEIVED_AMOUNT - qtyAmount;
           // if (OUTSTANDING_AMOUNT_TO_GRN > 0)
            if (RECEIVED_AMOUNT > 0)
                AGAINST_GRN_STATUS = AgainstStatus.Outstanding;
            else
                AGAINST_GRN_STATUS = AgainstStatus.Open;
            ((PurchaseOrder)EVENT).UpdateAgainstGRNStatusPO();
        }
        private bool isValidToClose()
        {
            bool validA = OUTSTANDING_AMOUNT_TO_GRN == 0;
            bool validB = RECEIVED_AMOUNT == this.GetAmountInSmallestUnit();//QYTAMOUNT;
            return validA && validB;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_purchaseorderitem 
                (   
                    po_id,
                    part_id,
                    warehouse_id,
                    poi_amount,
                    sce_id,
                    poi_scentrytype,
                    sc_id,
                    unit_id,
                    poi_price,
                    poi_discpercent,
                    poi_discamount,
                    poi_totaldisc,
                    poi_subtotal,
                    poi_notes,
                    poi_disca,
                    poi_discb,
                    poi_discc,
                    poi_discabc,
                    poi_againstgrnstatus,
                    poi_outstandingamounttogrn,
                    poi_receivedamount
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},{8},{9},{10},{11},{12},'{13}',{14},
                    {15},{16},'{17}','{18}',{19},{20})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.PurchaseOrder.ToString(),
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
                AGAINST_GRN_STATUS.ToString(),
                 GetAmountInSmallestUnit(),//OUTSTANDING_AMOUNT_TO_GRN,
                0//RECEIVED_AMOUNT
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_purchaseorderitem set 
                    po_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    poi_amount = {3},
                    sce_id = {4},
                    poi_scentrytype = '{5}',
                    sc_id = {6},
                    unit_id = {7},
                    poi_price = {8},
                    poi_discpercent = {9},
                    poi_discamount = {10},
                    poi_totaldisc = {11},
                    poi_subtotal = {12},
                    poi_notes = '{13}',
                    poi_disca = {14},
                    poi_discb = {15},
                    poi_discc = {16},
                    poi_discabc = '{17}',
                    poi_againstgrnstatus = '{18}',
                    poi_outstandingamounttogrn = {19},
                    poi_receivedamount = {20}
                where poi_id = {21}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.PurchaseOrder.ToString(),
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
                AGAINST_GRN_STATUS.ToString(),
                GetAmountInSmallestUnit(),
                RECEIVED_AMOUNT,
                ID);
        }
        public static PurchaseOrderItem TransformReader(OdbcDataReader aReader)
        {
            PurchaseOrderItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new PurchaseOrderItem();
                transaction.ID = Convert.ToInt32(aReader["poi_id"]);
                transaction.EVENT = new PurchaseOrder(Convert.ToInt32(aReader["po_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["poi_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["poi_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["poi_price"]));
                transaction.DISC_PERCENT = Convert.ToDouble(Convert.ToInt32(aReader["poi_discpercent"]));
                transaction.DISC_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["poi_discamount"]));
                transaction.TOTAL_DISCOUNT = Convert.ToDouble(Convert.ToInt32(aReader["poi_totaldisc"]));
                transaction.SUBTOTAL = Convert.ToDouble(Convert.ToInt32(aReader["poi_subtotal"]));
                transaction.NOTES = aReader["poi_notes"].ToString();
                transaction.DISC_A = Convert.ToDouble(Convert.ToInt32(aReader["poi_disca"]));
                transaction.DISC_B = Convert.ToDouble(Convert.ToInt32(aReader["poi_discb"]));
                transaction.DISC_C = Convert.ToDouble(Convert.ToInt32(aReader["poi_discc"]));
                transaction.DISC_ABC = aReader["poi_discabc"].ToString();
                transaction.AGAINST_GRN_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["poi_againstgrnstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT_TO_GRN = Convert.ToDouble(Convert.ToInt32(aReader["poi_outstandingamounttogrn"]));
                transaction.RECEIVED_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["poi_receivedamount"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                PurchaseOrderItem transaction = new PurchaseOrderItem();
                transaction.ID = Convert.ToInt32(aReader["poi_id"]);
                transaction.EVENT = new PurchaseOrder(Convert.ToInt32(aReader["po_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["poi_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["poi_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["poi_price"]));
                transaction.DISC_PERCENT = Convert.ToDouble(Convert.ToInt32(aReader["poi_discpercent"]));
                transaction.DISC_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["poi_discamount"]));
                transaction.TOTAL_DISCOUNT = Convert.ToDouble(Convert.ToInt32(aReader["poi_totaldisc"]));
                transaction.SUBTOTAL = Convert.ToDouble(Convert.ToInt32(aReader["poi_subtotal"]));
                transaction.NOTES = aReader["poi_notes"].ToString();
                transaction.DISC_A = Convert.ToDouble(Convert.ToInt32(aReader["poi_disca"]));
                transaction.DISC_B = Convert.ToDouble(Convert.ToInt32(aReader["poi_discb"]));
                transaction.DISC_C = Convert.ToDouble(Convert.ToInt32(aReader["poi_discc"]));
                transaction.DISC_ABC = aReader["poi_discabc"].ToString();
                transaction.AGAINST_GRN_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["poi_againstgrnstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT_TO_GRN = Convert.ToDouble(Convert.ToInt32(aReader["poi_outstandingamounttogrn"]));
                transaction.RECEIVED_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["poi_receivedamount"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(poi_id) from table_purchaseorderitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_purchaseorderitem where po_id = {0}", id);
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_purchaseorderitem where poi_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (PurchaseOrderItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_purchaseorderitem where po_id = {0} and poi_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_purchaseorderitem where poi_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_purchaseorderitem where po_id = {0}", id);
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"Update table_purchaseorderitem set 
                    poi_againstgrnstatus = '{0}',
                    poi_outstandingamounttogrn = {1},
                    poi_receivedamount = {2}
                    where poi_id = {3}", AGAINST_GRN_STATUS.ToString(),
                                       OUTSTANDING_AMOUNT_TO_GRN,
                                       RECEIVED_AMOUNT,
                                       ID); 
        }
        public static string GetSearchByPartAndPONo(string find, int supplierID ,string poi)
        {
            return String.Format(@"SELECT t.*
                FROM table_purchaseorderitem t
                INNER JOIN table_purchaseorder p on p.po_id = t.po_id
                INNER JOIN table_part pt on pt.part_id = t.part_id
                where t.poi_outstandingamounttogrn > 0
                and concat(pt.part_code, pt.part_name, p.po_code) like '%{0}%' and p.sup_id = {1}  
                and p.po_posted = true
               {2}", find, supplierID, poi!=""?" and t.poi_id not in ("+poi+")":"");
        }
        public override bool Equals(object obj)
        {
            PurchaseOrderItem e = (PurchaseOrderItem)obj;
            if (e == null) return false;
            return e.ID == this.ID;
        }
    }
}
