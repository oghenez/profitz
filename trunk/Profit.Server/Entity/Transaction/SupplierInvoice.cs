using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SupplierInvoice : Event
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
        public Supplier SUPPLIER = null;

        public SupplierInvoice()
            : base()
        { }
        public SupplierInvoice(int id)
            : base()
        {
            ID = id;
        }
       
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_supplierinvoice 
                (   si_date,
                    si_noticedate,
                    si_scentrytype,
                    emp_id,
                    si_notes,
                    si_posted,
                    si_eventstatus,
                    div_id,
                    top_id,
                    si_duedate,
                    ccy_id,
                    si_subtotal,
                    si_discpercent,
                    si_discafteramount,
                    si_discamount,
                    tax_id,
                    si_taxafteramount,
                    si_otherexpense,
                    si_nettotal,
                    si_code,
                    sup_id,
                    si_docno,
                    si_docdate
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},'{9}',{10},
                        {11},{12},{13},{14},{15},{16},{17},{18},'{19}',{20},'{21}','{22}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.SupplierInvoice.ToString(),
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
                SUPPLIER == null ? 0 : SUPPLIER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT)
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_supplierinvoice set 
                    si_date = '{0}',
                    si_noticedate = '{1}',
                    si_scentrytype = '{2}',
                    emp_id = {3},
                    si_notes = '{4}',
                    si_posted = {5},
                    si_eventstatus = '{6}',
                    div_id = {7},
                    top_id = {8},
                    si_duedate = '{9}',
                    ccy_id = {10},
                    si_subtotal = {11},
                    si_discpercent = {12},
                    si_discafteramount = {13},
                    si_discamount = {14},
                    tax_id = {15},
                    si_taxafteramount = {16},
                    si_otherexpense = {17},
                    si_nettotal = {18},
                    si_code = '{19}',
                    sup_id = {20},
                    si_docno= '{21}',
                    si_docdate= '{22}'
                where si_id = {23}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.SupplierInvoice.ToString(),
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
                SUPPLIER == null ? 0 : SUPPLIER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT),
                ID);
        }
        public static SupplierInvoice TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            SupplierInvoice transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new SupplierInvoice();
                transaction.ID = Convert.ToInt32(aReader["si_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["si_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["si_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["si_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["si_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["si_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["si_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["si_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["si_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["si_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["si_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["si_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["si_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["si_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["si_nettotal"]);
                transaction.CODE = aReader["si_code"].ToString();
                transaction.SUPPLIER = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.DOCUMENT_NO = aReader["si_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["si_docdate"]);
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                SupplierInvoice transaction = new SupplierInvoice();
                transaction.ID = Convert.ToInt32(aReader["si_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["si_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["si_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["si_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["si_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["si_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["si_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["si_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["si_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["si_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["si_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["si_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["si_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["si_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["si_nettotal"]);
                transaction.CODE = aReader["si_code"].ToString();
                transaction.SUPPLIER = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.DOCUMENT_NO = aReader["si_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["si_docdate"]);
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(si_id) from table_supplierinvoice");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_supplierinvoice where si_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_supplierinvoice where si_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT si_eventstatus from table_supplierinvoice where si_id ={0}", id);
        }
        public static string GetBySupplierSQL(int id)
        {
            return String.Format("SELECT * from table_supplierinvoice where sup_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_supplierinvoice set 
                    si_posted= {0},
                    si_eventstatus= '{1}'
                where si_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public string UpdateAgainstStatus()
        {
            return ""; 
//            String.Format(@"update table_supplierinvoice set 
//                    si_againsgrnstatus = '{0}'
//                where si_id = {1}",
//                           AGAINST_GRN_STATUS.ToString(),
//                            ID);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_supplierinvoice p where p.si_code like '%{0}%' ORDER BY p.si_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_supplierinvoice p";
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_supplierinvoice where si_code ='{0}'", code);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_supplierinvoice p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.si_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
    }
}
