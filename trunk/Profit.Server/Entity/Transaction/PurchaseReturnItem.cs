using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PurchaseReturnItem : EventItem
    {
        public string NOTES = string.Empty;
        public GoodReceiveNoteItem GRN_ITEM;
        public PurchaseReturnItem() : base() { }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_purchasereturnitem 
                (   
                    prn_id,
                    part_id,
                    warehouse_id,
                    prni_amount,
                    sce_id,
                    prn_scentrytype,
                    sc_id,
                    unit_id,
                    prni_notes,
                    grni_id,
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},'{8}',{9})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.PurchaseReturn.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                NOTES,
                GRN_ITEM==null?0:GRN_ITEM.ID
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_purchasereturnitem set 
                    prn_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    prni_amount = {3},
                    sce_id = {4},
                    prn_scentrytype = '{5}',
                    sc_id = {6},
                    unit_id = {7},
                    prni_notes = '{8}',
                    grni_id = {9}
                    where prni_id = {10}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.PurchaseReturn.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                NOTES,
                GRN_ITEM==null?0:GRN_ITEM.ID,
                ID);
        }
        public static PurchaseReturnItem TransformReader(OdbcDataReader aReader)
        {
            PurchaseReturnItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new PurchaseReturnItem();
                transaction.ID = Convert.ToInt32(aReader["prni_id"]);
                transaction.EVENT = new PurchaseReturn(Convert.ToInt32(aReader["prn_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["prni_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["prn_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.NOTES = aReader["prni_notes"].ToString();
                transaction.GRN_ITEM = new GoodReceiveNoteItem(Convert.ToInt32(aReader["grni_id"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                PurchaseReturnItem transaction = new PurchaseReturnItem();
                transaction.ID = Convert.ToInt32(aReader["prni_id"]);
                transaction.EVENT = new PurchaseReturn(Convert.ToInt32(aReader["prn_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["prni_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["prn_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.NOTES = aReader["prni_notes"].ToString();
                transaction.GRN_ITEM = new GoodReceiveNoteItem(Convert.ToInt32(aReader["grni_id"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(prni_id) from table_purchasereturnitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_purchasereturnitem where prn_id = {0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_purchasereturnitem where prni_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_purchasereturnitem where prn_id = {0}", id);
        }
        public static string FindByGrnItemIDSQL(int id)
        {
            return String.Format("SELECT * from table_purchasereturnitem where grni_id = {0}", id);
        }
    }
}
