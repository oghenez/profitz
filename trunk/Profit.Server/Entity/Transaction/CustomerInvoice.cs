using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class CustomerInvoice : Event
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
        public Customer CUSTOMER = null;

        public CustomerInvoice()
            : base()
        { }
        public CustomerInvoice(int id)
            : base()
        {
            ID = id;
        }
       
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_customerinvoice 
                (   ci_date,
                    ci_noticedate,
                    ci_scentrytype,
                    emp_id,
                    ci_notes,
                    ci_posted,
                    ci_eventstatus,
                    div_id,
                    top_id,
                    ci_duedate,
                    ccy_id,
                    ci_subtotal,
                    ci_discpercent,
                    ci_discafteramount,
                    ci_discamount,
                    tax_id,
                    ci_taxafteramount,
                    ci_otherexpense,
                    ci_nettotal,
                    ci_code,
                    cus_id,
                    ci_docno,
                    ci_docdate
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},'{9}',{10},
                        {11},{12},{13},{14},{15},{16},{17},{18},'{19}',{20},'{21}','{22}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.CustomerInvoice.ToString(),
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
                CODE,
                CUSTOMER == null ? 0 : CUSTOMER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT)
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_customerinvoice set 
                    ci_date = '{0}',
                    ci_noticedate = '{1}',
                    ci_scentrytype = '{2}',
                    emp_id = {3},
                    ci_notes = '{4}',
                    ci_posted = {5},
                    ci_eventstatus = '{6}',
                    div_id = {7},
                    top_id = {8},
                    ci_duedate = '{9}',
                    ccy_id = {10},
                    ci_subtotal = {11},
                    ci_discpercent = {12},
                    ci_discafteramount = {13},
                    ci_discamount = {14},
                    tax_id = {15},
                    ci_taxafteramount = {16},
                    ci_otherexpense = {17},
                    ci_nettotal = {18},
                    ci_code = '{19}',
                    cus_id = {20},
                    ci_docno= '{21}',
                    ci_docdate= '{22}'
                where ci_id = {23}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.CustomerInvoice.ToString(),
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
                CODE,
                CUSTOMER == null ? 0 : CUSTOMER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT),
                ID);
        }
        public static CustomerInvoice TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            CustomerInvoice transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new CustomerInvoice();
                transaction.ID = Convert.ToInt32(aReader["ci_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["ci_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["ci_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["ci_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["ci_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["ci_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["ci_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["ci_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["ci_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["ci_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["ci_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["ci_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["ci_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["ci_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["ci_nettotal"]);
                transaction.CODE = aReader["ci_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.DOCUMENT_NO = aReader["ci_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["ci_docdate"]);
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                CustomerInvoice transaction = new CustomerInvoice();
                transaction.ID = Convert.ToInt32(aReader["ci_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["ci_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["ci_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["ci_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["ci_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["ci_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["ci_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["ci_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["ci_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["ci_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["ci_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["ci_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["ci_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["ci_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["ci_nettotal"]);
                transaction.CODE = aReader["ci_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.DOCUMENT_NO = aReader["ci_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["ci_docdate"]);
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(ci_id) from table_customerinvoice");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_customerinvoice where ci_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_customerinvoice where ci_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT ci_eventstatus from table_customerinvoice where ci_id ={0}", id);
        }
        public static string GetBySupplierSQL(int id)
        {
            return String.Format("SELECT * from table_customerinvoice where cus_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_customerinvoice set 
                    ci_posted= {0},
                    ci_eventstatus= '{1}'
                where ci_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public string UpdateAgainstStatus()
        {
            return ""; 
//            String.Format(@"update table_customerinvoice set 
//                    ci_againsgrnstatus = '{0}'
//                where ci_id = {1}",
//                           AGAINST_GRN_STATUS.ToString(),
//                            ID);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_customerinvoice p where p.ci_code like '%{0}%' ORDER BY p.ci_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_customerinvoice p";
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_customerinvoice where ci_code ='{0}'", code);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_customerinvoice p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.ci_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
    }
}
