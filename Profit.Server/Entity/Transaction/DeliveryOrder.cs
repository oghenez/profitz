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
                    do_docdate
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}','{7}','{8}',{9},'{10}','{11}')",
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
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT)
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
                    do_docdate = '{11}'
                where do_id = {12}",
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
                where po_id = {1}",
                          AGAINST_SR_STATUS.ToString(),
                           ID);
        }
    }
}
