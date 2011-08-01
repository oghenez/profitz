using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class StockTaking : Event
    {
        public Warehouse WAREHOUSE;
        public double AMOUNT = 0;
        public Currency CURRENCY;
        public StockTakingType STOCK_TAKING_TYPE = StockTakingType.Adjustment;

        public StockTaking()
            : base()
        { }
        public StockTaking(int id)
            : base()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_stocktaking 
                (   stk_date,
                    stk_noticedate,
                    stk_scentrytype,
                    emp_id,
                    stk_notes,
                    stk_posted,
                    stk_eventstatus,
                    warehouse_id,
                    stk_amount,
                    ccy_id,
                    stk_stocktakingtype,
                    stk_code
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},{9},'{10}','{11}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.StockTaking.ToString(),//STOCK_CARD_ENTRY_TYPE.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                WAREHOUSE.ID,
                AMOUNT,
                CURRENCY.ID,
                STOCK_TAKING_TYPE.ToString(),
                CODE
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_stocktaking set 
                    stk_date = '{0}',
                    stk_noticedate= '{1}',
                    stk_scentrytype= '{2}',
                    emp_id= {3},
                    stk_notes= '{4}',
                    stk_posted= {5},
                    stk_eventstatus= '{6}',
                    warehouse_id= {7},
                    stk_amount= {8},
                    ccy_id= {9},
                    stk_stocktakingtype = '{10}',
                    stk_code = '{11}'
                where stk_id = {12}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                 StockCardEntryType.StockTaking.ToString(), //STOCK_CARD_ENTRY_TYPE.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                WAREHOUSE.ID,
                AMOUNT,
                CURRENCY.ID,
                STOCK_TAKING_TYPE.ToString(),
                CODE,
                ID);
        }
        public static StockTaking TransformReader(OdbcDataReader aReader)
        {
            StockTaking transaction = null;
            while (aReader.Read())
            {
                transaction = new StockTaking();
                transaction.ID = Convert.ToInt32(aReader["stk_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["stk_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["stk_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["stk_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["stk_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["stk_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["stk_eventstatus"].ToString());
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["stk_amount"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.STOCK_TAKING_TYPE = (StockTakingType)Enum.Parse(typeof(StockTakingType), aReader["stk_stocktakingtype"].ToString());
                transaction.CODE = aReader["stk_code"].ToString();
            }
            return transaction;
        }
        public static IList TransformReaderList(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                StockTaking transaction = new StockTaking();
                transaction.ID = Convert.ToInt32(aReader["stk_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["stk_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["stk_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["stk_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["stk_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["stk_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["stk_eventstatus"].ToString());
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["stk_amount"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.STOCK_TAKING_TYPE = (StockTakingType)Enum.Parse(typeof(StockTakingType), aReader["stk_stocktakingtype"].ToString());
                transaction.CODE = aReader["stk_code"].ToString();
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(stk_id) from table_stocktaking");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_stocktaking where stk_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_stocktaking where stk_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT stk_eventstatus from table_stocktaking where stk_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(int id, bool posted)
        {
            return String.Format(@"update table_stocktaking set 
                    stk_posted= {0},
                    stk_eventstatus= '{1}'
                where stk_id = {2}",
                posted,
                posted ? EventStatus.Confirm : EventStatus.Entry,
                id);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_stocktaking p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                INNER JOIN table_warehouse w on w.warehouse_id = p.warehouse_id
                where concat(p.stk_code, e.emp_code, e.emp_name,
                w.warehouse_code,w.warehouse_name, p.stk_stocktakingtype)
                like '%{0}%'", find);
        }
    }
}
