using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class DeliveryOrder : Event
    {
        public AgainstStatus AGAINST_SR_STATUS = AgainstStatus.Open;
        public Customer CUSTOMER = null;

        public DeliveryOrder()
            : base()
        { }
        public DeliveryOrder(int id)
            : base()
        {
            ID = id;
        }
        public void UpdateAgainstSRStatusDO()
        {
            bool allClosed = true;
            for (int i = 0; i < EVENT_ITEMS.Count; i++)
            {
                DeliveryOrderItem soi = EVENT_ITEMS[i] as DeliveryOrderItem;
                if (soi.AGAINST_SR_STATUS == AgainstStatus.Close) continue;
                allClosed = false;
                break;
            }
            AGAINST_SR_STATUS = allClosed ? AgainstStatus.Close : AgainstStatus.Outstanding;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_deliveryorder 
                (   do_date,
                    do_noticedate,
                    do_scentrytype,
                    emp_id,
                    do_notes,
                    do_posted,
                    do_eventstatus,
                    do_againstprstatus,
                    do_code,
                    cus_id,
                    do_docno,
                    do_docdate, 
                    modified_by, 
                    modified_date, 
                    modified_computer
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}','{7}','{8}',{9},'{10}','{11}','{12}','{13}','{14}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.DeliveryOrder.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                AGAINST_SR_STATUS.ToString(),
                CODE,
                CUSTOMER == null ? 0 : CUSTOMER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT),
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_deliveryorder set 
                    do_date = '{0}',
                    do_noticedate= '{1}',
                    do_scentrytype= '{2}',
                    emp_id= {3},
                    do_notes= '{4}',
                    do_posted= {5},
                    do_eventstatus= '{6}',
                    do_againstprstatus= '{7}',
                    do_code = '{8}',
                    cus_id = {9},
                    do_docno = '{10}',
                    do_docdate = '{11}',
                    modified_by='{12}', 
                    modified_date='{13}',
                    modified_computer='{14}'
                    where do_id = {15}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.DeliveryOrder.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                AGAINST_SR_STATUS.ToString(),
                CODE,
                CUSTOMER == null ? 0 : CUSTOMER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT), 
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public static DeliveryOrder TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            DeliveryOrder transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new DeliveryOrder();
                transaction.ID = Convert.ToInt32(aReader["do_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["do_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["do_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["do_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["do_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["do_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["do_eventstatus"].ToString());
                transaction.AGAINST_SR_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["do_againstprstatus"].ToString());
                transaction.CODE = aReader["do_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.DOCUMENT_NO = aReader["do_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["do_docdate"]);
                transaction.VENDOR = transaction.CUSTOMER;
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
                DeliveryOrder transaction = new DeliveryOrder();
                transaction.ID = Convert.ToInt32(aReader["do_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["do_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["do_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["do_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["do_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["do_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["do_eventstatus"].ToString());
                transaction.AGAINST_SR_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["do_againstprstatus"].ToString());
                transaction.CODE = aReader["do_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.DOCUMENT_NO = aReader["do_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["do_docdate"]);
                transaction.VENDOR = transaction.CUSTOMER;
                transaction.MODIFIED_BY = aReader["modified_by"].ToString();
                transaction.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                transaction.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(do_id) from table_deliveryorder");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_deliveryorder where do_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_deliveryorder where do_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT do_eventstatus from table_deliveryorder where do_id ={0}", id);
        }
        public static string GetByCustomerSQL(int id)
        {
            return String.Format("SELECT * from table_deliveryorder where cus_id ={0}", id);
        }
        public static string GetByCustomerSQL(DateTime startDate, DateTime endDate, int supid,
         bool allStatus, bool status)
        {
            return String.Format(@"SELECT * from table_deliveryorder where 
            do_date between '{0}' and '{1}' {2} {3}",
                startDate.ToString(Utils.DATE_FORMAT), endDate.ToString(Utils.DATE_FORMAT),
                supid == 0 ? "" : " and cus_id = " + supid,
                allStatus ? "" : " and do_posted = " + status);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_deliveryorder set 
                    do_posted= {0},
                    do_eventstatus= '{1}'
                where do_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"update table_deliveryorder set 
                    do_againstprstatus = '{0}'
                where do_id = {1}",
                          AGAINST_SR_STATUS.ToString(),
                           ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_deliveryorder p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                INNER JOIN table_customer s on s.cus_id = p.cus_id
                where concat(p.do_code, e.emp_code, e.emp_name, s.cus_code, s.cus_name)
                like '%{0}%'", find);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_deliveryorder p where p.do_code like '%{0}%' ORDER BY p.do_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_deliveryorder p";
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_deliveryorder where do_code ='{0}'", code);
        }
    }
}
