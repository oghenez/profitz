using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SalesReturnItem : EventItem
    {
        public string NOTES = string.Empty;
        public DeliveryOrderItem DO_ITEM;
        public SalesReturnItem() : base() { }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_salesreturnitem 
                (   
                    srn_id,
                    part_id,
                    warehouse_id,
                    srni_amount,
                    sce_id,
                    srn_scentrytype,
                    sc_id,
                    unit_id,
                    srni_notes,
                    doi_id
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},'{8}',{9})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.SalesReturn.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                NOTES,
                DO_ITEM==null?0:DO_ITEM.ID
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_salesreturnitem set 
                    srn_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    srni_amount = {3},
                    sce_id = {4},
                    srn_scentrytype = '{5}',
                    sc_id = {6},
                    unit_id = {7},
                    srni_notes = '{8}',
                    doi_id = {9}
                    where srni_id = {10}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.SalesReturn.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                NOTES,
                DO_ITEM==null?0:DO_ITEM.ID,
                ID);
        }
        public static SalesReturnItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            SalesReturnItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new SalesReturnItem();
                transaction.ID = Convert.ToInt32(aReader["srni_id"]);
                transaction.EVENT = new SalesReturn(Convert.ToInt32(aReader["srn_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["srni_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["srn_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.NOTES = aReader["srni_notes"].ToString();
                transaction.DO_ITEM = new DeliveryOrderItem(Convert.ToInt32(aReader["doi_id"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                SalesReturnItem transaction = new SalesReturnItem();
                transaction.ID = Convert.ToInt32(aReader["srni_id"]);
                transaction.EVENT = new SalesReturn(Convert.ToInt32(aReader["srn_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["srni_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["srn_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.NOTES = aReader["srni_notes"].ToString();
                transaction.DO_ITEM = new DeliveryOrderItem(Convert.ToInt32(aReader["doi_id"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(srni_id) from table_salesreturnitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_salesreturnitem where srn_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (SalesReturnItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_salesreturnitem where srn_id = {0} and srni_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_salesreturnitem where srni_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_salesreturnitem where srn_id = {0}", id);
        }
        public static string FindByDOItemIDSQL(int id)
        {
            return String.Format("SELECT * from table_salesreturnitem where doi_id = {0}", id);
        }
        public static string GetByPartIDSQL(int id)
        {
            return String.Format("SELECT * from table_salesreturnitem where part_id = {0}", id);
        }
    }
}
