using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PurchaseOrder : Event
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
        public AgainstStatus AGAINST_GRN_STATUS = AgainstStatus.Open;
        public Supplier SUPPLIER = null;

        public PurchaseOrder()
            : base()
        { }
        public PurchaseOrder(int id)
            : base()
        {
            ID = id;
        }
        public void UpdateAgainstGRNStatusPO()
        {
            bool allClosed = true;
            for (int i = 0; i < EVENT_ITEMS.Count; i++)
            {
                PurchaseOrderItem poi = EVENT_ITEMS[i] as PurchaseOrderItem;
                if (poi.AGAINST_GRN_STATUS == AgainstStatus.Close) continue;
                allClosed = false;
                break;
            }
            AGAINST_GRN_STATUS = allClosed ? AgainstStatus.Close : AgainstStatus.Outstanding;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_purchaseorder 
                (   po_date,
                    po_noticedate,
                    po_scentrytype,
                    emp_id,
                    po_notes,
                    po_posted,
                    po_eventstatus,
                    div_id,
                    top_id,
                    po_duedate,
                    ccy_id,
                    po_subtotal,
                    po_discpercent,
                    po_discafteramount,
                    po_discamount,
                    tax_id,
                    po_taxafteramount,
                    po_otherexpense,
                    po_nettotal,
                    po_againsgrnstatus,
                    po_code,
                    sup_id,
                    po_docno,
                    po_docdate
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},'{9}',{10},
                        {11},{12},{13},{14},{15},{16},{17},{18},'{19}','{20}',{21},'{22}','{23}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.PurchaseOrder.ToString(),
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
                AGAINST_GRN_STATUS.ToString(),
                CODE,
                SUPPLIER == null ? 0 : SUPPLIER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT)
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_purchaseorder set 
                    po_date = '{0}',
                    po_noticedate = '{1}',
                    po_scentrytype = '{2}',
                    emp_id = {3},
                    po_notes = '{4}',
                    po_posted = {5},
                    po_eventstatus = '{6}',
                    div_id = {7},
                    top_id = {8},
                    po_duedate = '{9}',
                    ccy_id = {10},
                    po_subtotal = {11},
                    po_discpercent = {12},
                    po_discafteramount = {13},
                    po_discamount = {14},
                    tax_id = {15},
                    po_taxafteramount = {16},
                    po_otherexpense = {17},
                    po_nettotal = {18},
                    po_againsgrnstatus = '{19}',
                    po_code = '{20}',
                    sup_id = {21},
                    po_docno = '{22}',
                    po_docdate ='{23}'
                where po_id = {24}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.PurchaseOrder.ToString(),
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
                AGAINST_GRN_STATUS.ToString(),
                CODE,
                SUPPLIER == null ? 0 : SUPPLIER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT),
                ID);
        }
        public static PurchaseOrder TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            PurchaseOrder transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new PurchaseOrder();
                transaction.ID = Convert.ToInt32(aReader["po_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["po_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["po_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["po_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["po_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["po_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["po_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["po_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["po_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["po_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["po_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["po_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["po_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["po_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["po_nettotal"]);
                transaction.AGAINST_GRN_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["po_againsgrnstatus"].ToString());
                transaction.CODE = aReader["po_code"].ToString();
                transaction.SUPPLIER = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.DOCUMENT_NO = aReader["po_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["po_docdate"]);
                transaction.VENDOR = transaction.SUPPLIER;
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                PurchaseOrder transaction = new PurchaseOrder();
                transaction.ID = Convert.ToInt32(aReader["po_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["po_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["po_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["po_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["po_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["po_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["po_eventstatus"].ToString());
                transaction.DIVISION = new Division(Convert.ToInt32(aReader["div_id"]));
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DUE_DATE = Convert.ToDateTime(aReader["po_duedate"]);
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.SUB_TOTAL = Convert.ToDouble(aReader["po_subtotal"]);
                transaction.DISC_PERCENT = Convert.ToDouble(aReader["po_discpercent"]);
                transaction.DISC_AFTER_AMOUNT = Convert.ToDouble(aReader["po_discafteramount"]);
                transaction.DISC_AMOUNT = Convert.ToDouble(aReader["po_discamount"]);
                transaction.TAX = new Tax(Convert.ToInt32(aReader["tax_id"]));
                transaction.TAX_AFTER_AMOUNT = Convert.ToDouble(aReader["po_taxafteramount"]);
                transaction.OTHER_EXPENSE = Convert.ToDouble(aReader["po_otherexpense"]);
                transaction.NET_TOTAL = Convert.ToDouble(aReader["po_nettotal"]);
                transaction.AGAINST_GRN_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["po_againsgrnstatus"].ToString());
                transaction.CODE = aReader["po_code"].ToString();
                transaction.SUPPLIER = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.DOCUMENT_NO = aReader["po_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["po_docdate"]);
                transaction.VENDOR = transaction.SUPPLIER;
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(po_id) from table_purchaseorder");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_purchaseorder where po_id ={0}", id);
        }
        public static string GetBySupplierSQL(int id)
        {
            return String.Format("SELECT * from table_purchaseorder where sup_id ={0}", id);
        }
        public static string GetBySupplierSQL(DateTime startDate, DateTime endDate, int supid, 
            bool allStatus, bool status)
        {
            return String.Format(@"SELECT * from table_purchaseorder where 
            po_date between '{0}' and '{1}' {2} {3}",
                startDate.ToString(Utils.DATE_FORMAT), endDate.ToString(Utils.DATE_FORMAT),
                supid == 0 ? "" : " and sup_id = " + supid,
                allStatus ? "" : " and po_posted = " + status);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_purchaseorder where po_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT po_eventstatus from table_purchaseorder where po_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_purchaseorder set 
                    po_posted= {0},
                    po_eventstatus= '{1}'
                where po_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"update table_purchaseorder set 
                    po_againsgrnstatus = '{0}'
                where po_id = {1}",
                          AGAINST_GRN_STATUS.ToString(),
                           ID);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_purchaseorder p where p.po_code like '%{0}%' ORDER BY p.po_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_purchaseorder p";
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_purchaseorder where po_code ='{0}'", code);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_purchaseorder p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                INNER JOIN table_supplier s on s.sup_id = p.sup_id
                where concat(p.po_code, e.emp_code, e.emp_name, s.sup_code, s.sup_name)
                like '%{0}%'", find);
        }
    }
}
