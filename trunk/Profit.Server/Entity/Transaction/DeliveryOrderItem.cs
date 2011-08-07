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
            double qtyAmount = doi.QYTAMOUNT;
            if (qtyAmount <= 0) return;
            if (AGAINST_SR_STATUS == AgainstStatus.Close)
                throw new Exception("DO Item Allready Close :" + this.PART.NAME);
            if (qtyAmount > OUTSTANDING_AMOUNT_TO_SR)
                throw new Exception("SR Item Amount exceed Outstanding DO Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_SR = OUTSTANDING_AMOUNT_TO_SR - qtyAmount;
            RETURNED_AMOUNT = RETURNED_AMOUNT + qtyAmount;
            if (isValidToClose())
                AGAINST_SR_STATUS = AgainstStatus.Close;
            else
                AGAINST_SR_STATUS = AgainstStatus.Outstanding;
            ((GoodReceiveNote)EVENT).UpdateAgainstPRStatusGRN();
        }
        public void UnSetOSAgainstSRItem(SalesReturnItem doi)
        {
            double qtyAmount = doi.QYTAMOUNT;
            if (qtyAmount > this.QYTAMOUNT || OUTSTANDING_AMOUNT_TO_SR + qtyAmount > this.QYTAMOUNT)
                throw new Exception("SR Item revise Amount exceed DO Item Amount :" + this.PART.NAME);
            OUTSTANDING_AMOUNT_TO_SR = OUTSTANDING_AMOUNT_TO_SR + qtyAmount;
            RETURNED_AMOUNT = RETURNED_AMOUNT - qtyAmount;
            if (RETURNED_AMOUNT > 0)
                AGAINST_SR_STATUS = AgainstStatus.Outstanding;
            else
                AGAINST_SR_STATUS = AgainstStatus.Open;
            ((GoodReceiveNote)EVENT).UpdateAgainstPRStatusGRN();
        }
        private bool isValidToClose()
        {
            bool validA = OUTSTANDING_AMOUNT_TO_SR == 0;
            bool validB = RETURNED_AMOUNT == QYTAMOUNT;
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
                QYTAMOUNT, //OUTSTANDING_AMOUNT_TO_SR,
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
                OUTSTANDING_AMOUNT_TO_SR,
                RETURNED_AMOUNT,
                ID);
        }
        public static DeliveryOrderItem TransformReader(OdbcDataReader aReader)
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
        public static IList TransformReaderList(OdbcDataReader aReader)
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
                    where soi_id = {3}", AGAINST_SR_STATUS.ToString(),
                                       OUTSTANDING_AMOUNT_TO_SR,
                                       RETURNED_AMOUNT,
                                       ID);
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_deliveryorderitem where doi_id = {0}", id);
        }
    }
}
