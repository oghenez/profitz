using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class OpeningStockItem : EventItem
    {
        public double PRICE = 0;
        public double TOTAL_AMOUNT = 0;

        public OpeningStockItem() : base() { }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_openingstockitem 
                (   
                    opst_id,
                    part_id,
                    warehouse_id,
                    opsti_amount,
                    sce_id,
                    opst_scentrytype,
                    sc_id,
                    unit_id,
                    opsti_price,
                    opsti_totalamount
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},{8},{9})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.OpeningStock.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                PRICE,
                TOTAL_AMOUNT
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_openingstockitem set 
                    opst_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    opsti_amount = {3},
                    sce_id = {4},
                    opst_scentrytype = '{5}',
                    sc_id = {6},
                    unit_id = {7},
                    opsti_price = {8},
                    opsti_totalamount = {9}
                where opsti_id = {10}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                 StockCardEntryType.OpeningStock.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                PRICE,
                TOTAL_AMOUNT,
                ID);
        }
        public static OpeningStockItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            OpeningStockItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new OpeningStockItem();
                transaction.ID = Convert.ToInt32(aReader["opsti_id"]);
                transaction.EVENT = new OpeningStock(Convert.ToInt32(aReader["opst_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["opsti_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["opst_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["opsti_price"]));
                transaction.TOTAL_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["opsti_totalamount"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                OpeningStockItem transaction = new OpeningStockItem();
                transaction.ID = Convert.ToInt32(aReader["opsti_id"]);
                transaction.EVENT = new OpeningStock(Convert.ToInt32(aReader["opst_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["opsti_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["opst_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["opsti_price"]));
                transaction.TOTAL_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["opsti_totalamount"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(opsti_id) from table_openingstockitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_openingstockitem where opst_id = {0}", id);
        }
        public static string GetByPartIDSQL(int id)
        {
            return String.Format("SELECT * from table_openingstockitem where part_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (OpeningStockItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_openingstockitem where opst_id = {0} and opsti_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_openingstockitem where opsti_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_openingstockitem where opst_id = {0}", id);
        }
    }
}
