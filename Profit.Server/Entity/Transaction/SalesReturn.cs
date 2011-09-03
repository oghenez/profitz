using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SalesReturn : Event
    {
        public Customer CUSTOMER = null;
        public SalesReturn()
            : base()
        { }
        public SalesReturn(int id)
            : base()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_salesreturn 
                (   srn_date,
                    srn_noticedate,
                    srn_scentrytype,
                    emp_id,
                    srn_notes,
                    srn_posted,
                    srn_eventstatus,
                    srn_code,
                    cus_id
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}','{7}',{8})",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.SalesReturn.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                CODE,
                CUSTOMER == null ? 0 : CUSTOMER.ID
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_salesreturn set 
                    srn_date = '{0}',
                    srn_noticedate= '{1}',
                    srn_scentrytype= '{2}',
                    emp_id= {3},
                    srn_notes= '{4}',
                    srn_posted= {5},
                    srn_eventstatus= '{6}',
                    srn_code = '{7}',
                    cus_id = {8}
                where srn_id = {9}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.SalesReturn.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                CODE,
                CUSTOMER == null ? 0 : CUSTOMER.ID,
                ID);
        }
        public static SalesReturn TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            SalesReturn transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new SalesReturn();
                transaction.ID = Convert.ToInt32(aReader["srn_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["srn_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["srn_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["srn_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["srn_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["srn_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["srn_eventstatus"].ToString());
                transaction.CODE = aReader["srn_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                SalesReturn transaction = new SalesReturn();
                transaction.ID = Convert.ToInt32(aReader["srn_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["srn_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["srn_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["srn_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["srn_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["srn_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["srn_eventstatus"].ToString());
                transaction.CODE = aReader["srn_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(srn_id) from table_salesreturn");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_salesreturn where srn_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_salesreturn where srn_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT srn_eventstatus from table_salesreturn where srn_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_salesreturn set 
                    srn_posted= {0},
                    srn_eventstatus= '{1}'
                where srn_id = {2}",
                 e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
    }
}
