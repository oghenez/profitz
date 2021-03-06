﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class StockTakingItems : EventItem
    {
        public double PRICE = 0;
        public double TOTAL_AMOUNT = 0;

        public StockTakingItems() : base() { }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_stocktakingitem 
                (   
                    stk_id,
                    part_id,
                    warehouse_id,
                    stki_amount,
                    sce_id,
                    stk_scentrytype,
                    sc_id,
                    unit_id,
                    stki_price,
                    stki_totalamount
                ) 
                VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},{8},{9})",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                StockCardEntryType.StockTaking.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                PRICE,
                TOTAL_AMOUNT
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_stocktakingitem set 
                    stk_id = {0},
                    part_id = {1},
                    warehouse_id = {2},
                    stki_amount = {3},
                    sce_id = {4},
                    stk_scentrytype = '{5}',
                    sc_id = {6},
                    unit_id = {7},
                    stki_price = {8},
                    stki_totalamount = {9}
                where stki_id = {10}",
                EVENT.ID,
                PART.ID,
                WAREHOUSE.ID,
                QYTAMOUNT,
                STOCK_CARD_ENTRY == null ? 0 : STOCK_CARD_ENTRY.ID,
                 StockCardEntryType.StockTaking.ToString(),
                STOCK_CARD == null ? 0 : STOCK_CARD.ID,
                UNIT.ID,
                PRICE,
                TOTAL_AMOUNT,
                ID);
        }
        public static StockTakingItems TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            StockTakingItems transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new StockTakingItems();
                transaction.ID = Convert.ToInt32(aReader["stki_id"]);
                transaction.EVENT = new StockTaking(Convert.ToInt32(aReader["stk_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["stki_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["stk_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.PRICE = Convert.ToDouble(Convert.ToInt32(aReader["stki_price"]));
                transaction.TOTAL_AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["stki_totalamount"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                StockTakingItems transaction = new StockTakingItems();
                transaction.ID = Convert.ToInt32(aReader["stki_id"]);
                transaction.EVENT = new StockTaking(Convert.ToInt32(aReader["stk_id"]));
                transaction.PART = new Part(Convert.ToInt32(aReader["part_id"]));
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.QYTAMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["stki_amount"]));
                transaction.STOCK_CARD_ENTRY = new StockCardEntry(Convert.ToInt32(aReader["sce_id"]));
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["stk_scentrytype"].ToString());
                transaction.STOCK_CARD = new StockCard(Convert.ToInt32(aReader["sc_id"]));
                transaction.UNIT = new Unit(Convert.ToInt32(aReader["unit_id"]));//         
                transaction.PRICE = Convert.ToDouble(Convert.ToDouble(aReader["stki_price"]));
                transaction.TOTAL_AMOUNT = Convert.ToDouble(Convert.ToDouble(aReader["stki_totalamount"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(stki_id) from table_stocktakingitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_stocktakingitem where stk_id = {0}", id);
        }
        public static string GetByPartIDSQL(int id)
        {
            return String.Format("SELECT * from table_stocktakingitem where part_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (StockTakingItems i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_stocktakingitem where stk_id = {0} and stki_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_stocktakingitem where stki_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_stocktakingitem where stk_id = {0}", id);
        }
        public static string GetByPartIDOrderByDateSQL(int id)
        {
            return String.Format(@"SELECT i.* from table_stocktaking h, 
            table_stocktakingitem i 
            where 
            h.stk_id = i.stk_id 
            and i.part_id = {0}
            and h.stk_posted = True
            order by h.stk_date asc",
             id);
        }
        public static string GetByPartIDOrderByDateRangeSQL(int id, DateTime start, DateTime end)
        {
            return String.Format(@"SELECT i.* from table_stocktaking h, 
            table_stocktakingitem i 
            where 
            h.stk_id = i.stk_id 
            and i.part_id = {0}
            and h.stk_posted = True
            and h.stk_date between '{1}' and '{2}'
            order by h.stk_date asc",
             id, start.ToString(Utils.DATE_FORMAT_SHORT), end.ToString(Utils.DATE_FORMAT_SHORT_END));
        }
    }
}
