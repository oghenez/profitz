using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class OpeningStock : Event
    {
        public Warehouse WAREHOUSE;
        public double AMOUNT = 0;
        public Currency CURRENCY;

        public OpeningStock()
            : base()
        { }
        public OpeningStock(int id)
            : base()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_openingstock 
                (   opst_date,
                    opst_noticedate,
                    opst_scentrytype,
                    emp_id,
                    opst_notes,
                    opst_posted,
                    opst_eventstatus,
                    warehouse_id,
                    opst_amount,
                    ccy_id,
                    opst_code, 
                    modified_by, 
                    modified_date, 
                    modified_computer
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},{9},'{10}','{11}','{12}','{13}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.OpeningStock.ToString(),//STOCK_CARD_ENTRY_TYPE.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                WAREHOUSE.ID,
                AMOUNT,
                CURRENCY.ID,
                CODE,
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_openingstock set 
                    opst_date = '{0}',
                    opst_noticedate= '{1}',
                    opst_scentrytype= '{2}',
                    emp_id= {3},
                    opst_notes= '{4}',
                    opst_posted= {5},
                    opst_eventstatus= '{6}',
                    warehouse_id= {7},
                    opst_amount= {8},
                    ccy_id= {9},
                    opst_code = '{10}',
                modified_by='{11}', 
                modified_date='{12}',
                modified_computer='{13}'
                where opst_id = {14}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                 StockCardEntryType.OpeningStock.ToString(), //STOCK_CARD_ENTRY_TYPE.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                WAREHOUSE.ID,
                AMOUNT,
                CURRENCY.ID,
                CODE,
                 MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public static OpeningStock TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            OpeningStock transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new OpeningStock();
                transaction.ID = Convert.ToInt32(aReader["opst_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["opst_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["opst_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["opst_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["opst_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["opst_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["opst_eventstatus"].ToString());
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["opst_amount"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.CODE = aReader["opst_code"].ToString();
                transaction.MODIFIED_BY = aReader["modified_by"].ToString();
                transaction.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                transaction.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                OpeningStock transaction = new OpeningStock();
                transaction.ID = Convert.ToInt32(aReader["opst_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["opst_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["opst_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["opst_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["opst_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["opst_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["opst_eventstatus"].ToString());
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["opst_amount"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.CODE = aReader["opst_code"].ToString();
                transaction.MODIFIED_BY = aReader["modified_by"].ToString();
                transaction.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                transaction.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(opst_id) from table_openingstock");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_openingstock where opst_id ={0}", id);
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_openingstock where opst_code ='{0}'", code);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_openingstock where opst_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT opst_eventstatus from table_openingstock where opst_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_openingstock set 
                    opst_posted= {0},
                    opst_eventstatus= '{1}'
                where opst_id = {2}",
                 e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_openingstock p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                INNER JOIN table_warehouse w on w.warehouse_id = p.warehouse_id
                where concat(p.opst_code, e.emp_code, e.emp_name,
                w.warehouse_code,w.warehouse_name)
                like '%{0}%'", find);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_openingstock p where p.opst_code like '%{0}%' ORDER BY p.opst_id DESC", code);
        }

        public static string RecordCount()
        {
            return @"select Count(*) from table_openingstock p";
        }
    }
}
