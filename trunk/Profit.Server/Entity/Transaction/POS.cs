using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class POS : Event
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

        public double CASH_PAY_AMOUNT = 0;
        public double CHANGE_AMOUNT = 0;
        public PaymentType PAYMENT_TYPE = PaymentType.Cash;
        public string CREDIT_CARD_NO = "";
        public string HOLDER_NAME = "";
        public string ACCOUNT_NO = "";

        public POS()
            : base()
        { }
        public POS(int id)
            : base()
        {
            ID = id;
        }
       
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_pos 
                (   pos_date,
                    pos_noticedate,
                    pos_scentrytype,
                    emp_id,
                    pos_notes,
                    pos_posted,
                    pos_eventstatus,
                    div_id,
                    top_id,
                    pos_duedate,
                    ccy_id,
                    pos_subtotal,
                    pos_discpercent,
                    pos_discafteramount,
                    pos_discamount,
                    tax_id,
                    pos_taxafteramount,
                    pos_otherexpense,
                    pos_nettotal,
                    pos_code,
                    cus_id,
                    pos_docno,
                    pos_docdate,
                    pos_cashpayamount,
                    pos_changepayamount, 
                    modified_by, 
                    modified_date, 
                    modified_computer
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},'{9}',{10},
                        {11},{12},{13},{14},{15},{16},{17},{18},'{19}',{20},'{21}','{22}',{23},{24},'{25}','{26}','{27}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.POS.ToString(),
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
                CASH_PAY_AMOUNT,
                CHANGE_AMOUNT,
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_pos set 
                    pos_date = '{0}',
                    pos_noticedate = '{1}',
                    pos_scentrytype = '{2}',
                    emp_id = {3},
                    pos_notes = '{4}',
                    pos_posted = {5},
                    pos_eventstatus = '{6}',
                    div_id = {7},
                    top_id = {8},
                    pos_duedate = '{9}',
                    ccy_id = {10},
                    pos_subtotal = {11},
                    pos_discpercent = {12},
                    pos_discafteramount = {13},
                    pos_discamount = {14},
                    tax_id = {15},
                    pos_taxafteramount = {16},
                    pos_otherexpense = {17},
                    pos_nettotal = {18},
                    pos_code = '{19}',
                    cus_id = {20},
                    pos_docno= '{21}',
                    pos_docdate= '{22}',
                    pos_cashpayamount = {23},
                    pos_changepayamount = {24},
                modified_by='{25}', 
                modified_date='{26}',
                modified_computer='{27}'
                where pos_id = {28}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.POS.ToString(),
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
                CASH_PAY_AMOUNT,
                CHANGE_AMOUNT,
                 MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public static POS TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            POS transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new POS();
                transaction.ID = Convert.ToInt32(aReader["pos_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["pos_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["pos_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["pos_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["pos_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["pos_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["pos_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["pos_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["pos_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["pos_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["pos_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["pos_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["pos_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["pos_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["pos_nettotal"]);
                transaction.CODE = aReader["pos_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.DOCUMENT_NO = aReader["pos_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["pos_docdate"]);
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
                POS transaction = new POS();
                transaction.ID = Convert.ToInt32(aReader["pos_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["pos_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["pos_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["pos_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["pos_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["pos_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["pos_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["pos_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["pos_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["pos_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["pos_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["pos_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["pos_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["pos_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["pos_nettotal"]);
                transaction.CODE = aReader["pos_code"].ToString();
                transaction.CUSTOMER = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.DOCUMENT_NO = aReader["pos_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["pos_docdate"]);
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
            return String.Format("SELECT max(pos_id) from table_pos");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_pos where pos_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_pos where pos_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT pos_eventstatus from table_pos where pos_id ={0}", id);
        }
        public static string GetByCustomerSQL(int id)
        {
            return String.Format("SELECT * from table_pos where cus_id ={0}", id);
        }
        public static string GetByCustomerSQL(DateTime startDate, DateTime endDate, int supid,
        bool allStatus, bool status)
        {
            return String.Format(@"SELECT * from table_pos where 
            pos_date between '{0}' and '{1}' {2} {3}",
                startDate.ToString(Utils.DATE_FORMAT), endDate.ToString(Utils.DATE_FORMAT_SHORT_END),
                supid == 0 ? "" : " and cus_id = " + supid,
                allStatus ? "" : " and pos_posted = " + status);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_pos set 
                    pos_posted= {0},
                    pos_eventstatus= '{1}'
                where pos_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public string UpdateAgainstStatus()
        {
            return ""; 
//            String.Format(@"update table_pos set 
//                    pos_againsgrnstatus = '{0}'
//                where pos_id = {1}",
//                           AGAINST_GRN_STATUS.ToString(),
//                            ID);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_pos p where p.pos_code like '%{0}%' ORDER BY p.pos_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_pos p";
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_pos where pos_code ='{0}'", code);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_pos p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.pos_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
    }
}
