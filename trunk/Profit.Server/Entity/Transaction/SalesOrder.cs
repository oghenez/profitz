using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SalesOrder : Event
    {
        public Division DIVISION = null;
        public TermOfPayment TOP = null;
        public DateTime DUE_DATE = DateTime.Today;
        public Currency CURRENCY = null;
        public double SUB_TOTAL = 0;
        public double DISC_PERCENT = 0;
        public double DISC_AFTER_AMOUNT = 0;
        public double DISC_AMOUNT = 0;
        public Tax TAX = null;
        public double TAX_AFTER_AMOUNT = 0;
        public double OTHER_EXPENSE = 0;
        public double NET_TOTAL = 0;
        public AgainstStatus AGAINST_DO_STATUS = AgainstStatus.Open;
        public Customer CUSTOMER = null;

        public SalesOrder()
            : base()
        { }
        public SalesOrder(int id)
            : base()
        {
            ID = id;
        }
        public void UpdateAgainstDOStatusSO()
        {
            bool allClosed = true;
            for (int i = 0; i < EVENT_ITEMS.Count; i++)
            {
                SalesOrderItem poi = EVENT_ITEMS[i] as SalesOrderItem;
                if (poi.AGAINST_DO_STATUS == AgainstStatus.Close) continue;
                allClosed = false;
                break;
            }
            AGAINST_DO_STATUS = allClosed ? AgainstStatus.Close : AgainstStatus.Outstanding;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_salesorder 
                (   so_date,
                    so_noticedate,
                    so_scentrytype,
                    emp_id,
                    so_notes,
                    so_posted,
                    so_eventstatus,
                    div_id,
                    top_id,
                    so_duedate,
                    ccy_id,
                    so_subtotal,
                    so_discpercent,
                    so_discafteramount,
                    so_discamount,
                    tax_id,
                    so_taxafteramount,
                    so_otherexpense,
                    so_nettotal,
                    so_againsdostatus,
                    so_code,
                    cus_id,
                    so_docno,
                    so_docdate
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},'{9}',{10},
                        {11},{12},{13},{14},{15},{16},{17},{18},'{19}','{20}',{21},'{22}','{23}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.SalesOrder.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                DIVISION == null ? 0 : DIVISION.ID,
                TOP == null ? 0 : TOP.ID,
                DUE_DATE.ToString(Utils.DATE_FORMAT),
                CURRENCY == null ? 0 : CURRENCY.ID,
                SUB_TOTAL,
                DISC_PERCENT,
                DISC_AFTER_AMOUNT,
                DISC_AMOUNT,
                TAX == null ? 0 : TAX.ID,
                TAX_AFTER_AMOUNT,
                OTHER_EXPENSE,
                NET_TOTAL,
                AGAINST_DO_STATUS.ToString(),
                CODE,
                CUSTOMER == null ? 0 : CUSTOMER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT)
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_salesorder set 
                    so_date = {0},
                    so_noticedate = '{1}',
                    so_scentrytype = '{2}',
                    emp_id = {3},
                    so_notes = '{4}',
                    so_posted = {5},
                    so_eventstatus = '{6}',
                    div_id = {7},
                    top_id = {8},
                    so_duedate = '{9}',
                    ccy_id = {10},
                    so_subtotal = {11},
                    so_discpercent = {12},
                    so_discafteramount = {13},
                    so_discamount = {14},
                    tax_id = {15},
                    so_taxafteramount = {16},
                    so_otherexpense = {17},
                    so_nettotal = {18},
                    so_againsdostatus = '{19}',
                    so_code = '{20}',
                    cus_id = {21},
                    so_docno = '{22}',
                    so_docdate ='{23}'
                where so_id = {24}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.SalesOrder.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                DIVISION == null ? 0 : DIVISION.ID,
                TOP == null ? 0 : TOP.ID,
                DUE_DATE.ToString(Utils.DATE_FORMAT),
                CURRENCY == null ? 0 : CURRENCY.ID,
                SUB_TOTAL,
                DISC_PERCENT,
                DISC_AFTER_AMOUNT,
                DISC_AMOUNT,
                TAX == null ? 0 : TAX.ID,
                TAX_AFTER_AMOUNT,
                OTHER_EXPENSE,
                NET_TOTAL,
                AGAINST_DO_STATUS.ToString(),
                CODE,
                CUSTOMER == null ? 0 : CUSTOMER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT),
                ID);
        }
        public static SalesOrder TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            SalesOrder transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new SalesOrder();
                transaction.ID = Convert.ToInt32(aReader["so_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["so_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["so_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["so_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["so_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["so_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["so_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["so_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["so_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["so_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["so_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["so_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["so_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["so_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["so_nettotal"]);
                transaction.AGAINST_DO_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["so_againsdostatus"].ToString());
                transaction.CODE = aReader["so_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.DOCUMENT_NO = aReader["so_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["so_docdate"]);
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                SalesOrder transaction = new SalesOrder();
                transaction.ID = Convert.ToInt32(aReader["so_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["so_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["so_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["so_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["so_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["so_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["so_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["so_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["so_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["so_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["so_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["so_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["so_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["so_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["so_nettotal"]);
                transaction.AGAINST_DO_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["so_againsdostatus"].ToString());
                transaction.CODE = aReader["so_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.DOCUMENT_NO = aReader["so_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["so_docdate"]);
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(so_id) from table_salesorder");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_salesorder where so_id ={0}", id);
        }
        public static string GetBySupplierSQL(int id)
        {
            return String.Format("SELECT * from table_salesorder where cus_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_salesorder where so_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT so_eventstatus from table_salesorder where so_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_salesorder set 
                    so_posted= {0},
                    so_eventstatus= '{1}'
                where so_id = {2}",
                 e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"update table_salesorder set 
                    so_againsdostatus = '{0}'
                where so_id = {1}",
                          AGAINST_DO_STATUS.ToString(),
                           ID);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_salesorder p where p.so_code like '%{0}%' ORDER BY p.so_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_salesorder p";
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_salesorder where so_code ='{0}'", code);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_salesorder p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                INNER JOIN table_customer s on s.cus_id = p.cus_id
                where concat(p.so_code, e.emp_code, e.emp_name, s.cus_code, s.cus_name)
                like '%{0}%'", find);
        }
    }
}
