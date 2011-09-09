using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class DeliveryOrderItem : EventItem
    {
        public string NOTES = string.Empty;
        public SalesOrderItem SO_ITEM;
        public AgainstStatus AGAINST_SR_STATUS = AgainstStatus.Open;
        public double OUTSTANDING_AMOUNT_TO_SR = 0;
        public double RETURNED_AMOUNT = 0;

        public DeliveryOrderItem() : base() { }
        public DeliveryOrderItem(int Id) : base(Id) { }
        public void SetOSAgainstSRItem(SalesReturnItem doi)
        {
            double qtyAmount = doi.GetAmountInSmallestUnit();
            if (qtyAmount <= 0) return;
            if (AGAINST_SR_STATUS == AgainstStatus.Close)
                throw new Exception("DO Item already fully delivered :" + this.PART.NAME);
            if (qtyAmount > OUTSTANDING_AMOUNT_TO_SR)
                throw new Exception("SR Item Amount exceed Outstanding DO Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_SR = OUTSTANDING_AMOUNT_TO_SR - qtyAmount;
            RETURNED_AMOUNT = RETURNED_AMOUNT + qtyAmount;
            if (isValidToClose())
                AGAINST_SR_STATUS = AgainstStatus.Close;
            else
                AGAINST_SR_STATUS = AgainstStatus.Outstanding;
            ((DeliveryOrder)EVENT).UpdateAgainstSRStatusDO();
        }
        public void UnSetOSAgainstSRItem(SalesReturnItem doi)
        {
            double qtyAmount = doi.GetAmountInSmallestUnit();
            if (qtyAmount > this.GetAmountInSmallestUnit() || OUTSTANDING_AMOUNT_TO_SR + qtyAmount > this.GetAmountInSmallestUnit())
                throw new Exception("SR Item revise Amount exceed DO Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_SR = OUTSTANDING_AMOUNT_TO_SR + qtyAmount;
            RETURNED_AMOUNT = RETURNED_AMOUNT - qtyAmount;
            if (RETURNED_AMOUNT > 0)
                AGAINST_SR_STATUS = AgainstStatus.Outstanding;
            else
                AGAINST_SR_STATUS = AgainstStatus.Open;
            ((DeliveryOrder)EVENT).UpdateAgainstSRStatusDO();
        }
        private bool isValidToClose()
        {
            bool validA = OUTSTANDING_AMOUNT_TO_SR == 0;
            bool validB = RETURNED_AMOUNT == GetAmountInSmallestUnit();
            return validA && validB;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_deliveryorderitem 
                (   
                    do_id,
                    part_id,
                    warehouse_id,
                    doi_amount,
                    sce_id,
                    do_scentrytype,
                    sc_id,
                    unit_id,
                    doi_notes,
                    soi_id,
                    doi_againstprstatus,
                    doi_outstandingamtpr,
                    doi_returnedamount
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},'{8}',{9},'{10}',{11},{12})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.DeliveryOrder.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                NOTES,
                SO_ITEM==null?0:SO_ITEM.ID,
                AGAINST_SR_STATUS.ToString(),
                GetAmountInSmallestUnit(),//QYTAMOUNT, //OUTSTANDING_AMOUNT_TO_SR,
                0//RETURNED_AMOUNT
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_deliveryorderitem set 
                    do_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    doi_amount = {3},
                    sce_id = {4},
                    do_scentrytype = '{5}',
                    sc_id = {6},
                    unit_id = {7},
                    doi_notes = '{8}',
                    soi_id = {9},
                    doi_againstprstatus = '{10}',
                    doi_outstandingamtpr = {11},
                    doi_returnedamount = {12}
                where doi_id = {13}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.DeliveryOrder.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                NOTES,
                SO_ITEM==null?0:SO_ITEM.ID,
                AGAINST_SR_STATUS.ToString(),
                GetAmountInSmallestUnit(),//OUTSTANDING_AMOUNT_TO_SR,
                RETURNED_AMOUNT,
                ID);
        }
        public static DeliveryOrderItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            DeliveryOrderItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new DeliveryOrderItem();
                transaction.ID = Convert.ToInt32(aReader["doi_id"]);
                transaction.EVENT = new DeliveryOrder(Convert.ToInt32(aReader["do_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["doi_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["do_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.NOTES = aReader["doi_notes"].ToString();
                transaction.SO_ITEM = new SalesOrderItem(Convert.ToInt32(aReader["soi_id"]));
                transaction.AGAINST_SR_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["doi_againstprstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT_TO_SR = Convert.ToDouble(Convert.ToInt32(aReader["doi_outstandingamtpr"]));
                transaction.RETURNED_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["doi_returnedamount"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                DeliveryOrderItem transaction = new DeliveryOrderItem();
                transaction.ID = Convert.ToInt32(aReader["doi_id"]);
                transaction.EVENT = new DeliveryOrder(Convert.ToInt32(aReader["do_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["doi_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["do_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.NOTES = aReader["doi_notes"].ToString();
                transaction.SO_ITEM = new SalesOrderItem(Convert.ToInt32(aReader["soi_id"]));
                transaction.AGAINST_SR_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["doi_againstprstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT_TO_SR = Convert.ToDouble(Convert.ToInt32(aReader["doi_outstandingamtpr"]));
                transaction.RETURNED_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["doi_returnedamount"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(doi_id) from table_deliveryorderitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_deliveryorderitem where do_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (DeliveryOrderItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_deliveryorderitem where do_id = {0} and doi_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_deliveryorderitem where doi_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_deliveryorderitem where do_id = {0}", id);
        }
        public static string FindBySOItemIDSQL(int id)
        {
            return String.Format("SELECT * from table_deliveryorderitem where soi_id = {0}", id);
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"Update table_deliveryorderitem set 
                    doi_againstprstatus = '{0}',
                    doi_outstandingamtpr = {1},
                    doi_returnedamount = {2}
                    where doi_id = {3}", AGAINST_SR_STATUS.ToString(),
                                       OUTSTANDING_AMOUNT_TO_SR,
                                       RETURNED_AMOUNT,
                                       ID);
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_deliveryorderitem where doi_id = {0}", id);
        }
        public static string GetByPartIDSQL(int id)
        {
            return String.Format("SELECT * from table_deliveryorderitem where part_id = {0}", id);
        }
        public static string GetSearchByPartAndDONo(string find, int customerID, string poi, DateTime trdate)
        {
            return String.Format(@"SELECT t.*
                FROM table_deliveryorderitem t
                INNER JOIN table_deliveryorder p on p.do_id = t.do_id
                INNER JOIN table_part pt on pt.part_id = t.part_id
                where t.doi_outstandingamtpr > 0
                and concat(pt.part_code, pt.part_name, p.do_code) like '%{0}%' and p.cus_id = {1}  
                and p.do_posted = true
                and p.do_date <= '{2}'
               {3}", find, customerID, trdate.ToString(Utils.DATE_FORMAT), poi != "" ? " and t.doi_id not in (" + poi + ")" : "");
        }
        public static string GetDOItemByCusDate(int supID, DateTime date, string notInGRNItem)
        {
            return String.Format(@"SELECT t.*
                FROM table_deliveryorderitem t
                INNER JOIN table_deliveryorder p on p.do_id = t.do_id
                INNER JOIN table_part pt on pt.part_id = t.part_id
                where t.doi_outstandingamtpr > 0
                and p.cus_id = {0}  
                and p.do_posted = true
                and p.do_date <= '{1}'
                {2}",
                    supID,
                    date.ToString(Utils.DATE_FORMAT),
                    notInGRNItem != "" ? " and t.doi_id not in (" + notInGRNItem + ")" : "");
        }
        public override bool Equals(object obj)
        {
            DeliveryOrderItem e = (DeliveryOrderItem)obj;
            if (e == null) return false;
            return e.ID == this.ID;
        }
        public static string GetOutstandingReturnSQL(int id)
        {
            return String.Format("SELECT doi_outstandingamtpr from table_deliveryorderitem where doi_id = {0}", id);
        }
        public static string GetReturnSQL(int id)
        {
            return String.Format("SELECT doi_returnedamount from table_deliveryorderitem where doi_id = {0}", id);
        }
    }
}
