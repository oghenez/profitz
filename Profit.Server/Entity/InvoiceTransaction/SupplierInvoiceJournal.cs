using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class SupplierInvoiceJournal : EventJournal
    {
        public SupplierInvoice SUPPLIER_INVOICE;
        public AgainstStatus AGAINST_PAYMENT_STATUS = AgainstStatus.Open;

        public SupplierInvoiceJournal()
        { }
        public SupplierInvoiceJournal(int id)
            : base()
        {
            ID = id;
        }
        public void UpdateAgainstPaymentStatusSIJ()
        {
            bool allClosed = true;
            for (int i = 0; i < EVENT_JOURNAL_ITEMS.Count; i++)
            {
                SupplierInvoiceJournalItem poi = EVENT_JOURNAL_ITEMS[i] as SupplierInvoiceJournalItem;
                if (poi.AGAINST_PAYMENT_STATUS == AgainstStatus.Close) continue;
                allClosed = false;
                break;
            }
            AGAINST_PAYMENT_STATUS = allClosed ? AgainstStatus.Close : AgainstStatus.Outstanding;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_supplierinvoicejournal 
                (  
                    sij_code,
                    sij_date,
                    sup_id,
                    ccy_id,
                    entry_type,
                    sij_notes,
                    sij_posted,
                    sij_eventstatus,
                    sij_subtotalamount,
                    sij_discpercent,
                    sij_amountafterdiscpercent,
                    sij_discamount,
                    sij_amountafterdiscamount,
                    sij_otherexpense,
                    sij_netamount,
                    emp_id,
                    si_id,
                    sij_againstpaymentstatus, 
                    modified_by, 
                    modified_date, 
                    modified_computer
                ) 
                VALUES ('{0}','{1}',{2},{3},'{4}','{5}',{6},'{7}',{8},{9},{10},{11},{12},{13},{14},{15},{16},'{17}','{18}','{19}','{20}')",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.SupplierInvoice.ToString(),
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                SUBTOTAL_AMOUNT,
                DISC_PERCENT,
                AMOUNT_AFTER_DISC_PERCENT,
                DISC_AMOUNT,
                AMOUNT_AFTER_DISC_AMOUNT,
                OTHER_EXPENSE,
                NET_AMOUNT,
                EMPLOYEE.ID,
                SUPPLIER_INVOICE == null ? 0 : SUPPLIER_INVOICE.ID,
                AGAINST_PAYMENT_STATUS.ToString(),
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_supplierinvoicejournal set 
                   sij_code = '{0}',
                    sij_date= '{1}',
                    sup_id = {2},
                    ccy_id= {3},
                    entry_type= '{4}',
                    sij_notes= '{5}',
                    sij_posted= {6},
                    sij_eventstatus= '{7}',
                    sij_subtotalamount= {8},
                    sij_discpercent= {9},
                    sij_amountafterdiscpercent= {10},
                    sij_discamount= {11},
                    sij_amountafterdiscamount= {12},
                    sij_otherexpense= {13},
                    sij_netamount= {14},
                    emp_id = {15},
                     si_id = {16},
                    sij_againstpaymentstatus = '{17}',
                modified_by='{18}', 
                modified_date='{19}',
                modified_computer='{20}'
                where sij_id = {21}",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.SupplierInvoice.ToString(),
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                SUBTOTAL_AMOUNT,
                DISC_PERCENT,
                AMOUNT_AFTER_DISC_PERCENT,
                DISC_AMOUNT,
                AMOUNT_AFTER_DISC_AMOUNT,
                OTHER_EXPENSE,
                NET_AMOUNT,
                EMPLOYEE.ID,
                SUPPLIER_INVOICE == null ? 0 : SUPPLIER_INVOICE.ID,
                AGAINST_PAYMENT_STATUS.ToString(),
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public static SupplierInvoiceJournal TransformReader(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            SupplierInvoiceJournal tr = null;
            if (r.HasRows)
            {
                r.Read();
                tr = new SupplierInvoiceJournal();
                tr.ID = Convert.ToInt32(r["sij_id"]);
                tr.CODE = r["sij_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["sij_date"]);
                tr.VENDOR = new Supplier(Convert.ToInt32(r["sup_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierInvoice;
                tr.NOTES = r["sij_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["sij_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["sij_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["sij_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["sij_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["sij_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["sij_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["sij_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["sij_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["sij_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.SUPPLIER_INVOICE = new SupplierInvoice(Convert.ToInt32(r["si_id"]));
                tr.AGAINST_PAYMENT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), r["sij_againstpaymentstatus"].ToString());
                tr.MODIFIED_BY = r["modified_by"].ToString();
                tr.MODIFIED_DATE = Convert.ToDateTime(r["modified_date"].ToString());
                tr.MODIFIED_COMPUTER_NAME = r["modified_computer"].ToString();
            }
            return tr;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            IList result = new ArrayList();
            while (r.Read())
            {
                SupplierInvoiceJournal tr = new SupplierInvoiceJournal();
                tr.ID = Convert.ToInt32(r["sij_id"]);
                tr.CODE = r["sij_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["sij_date"]);
                tr.VENDOR = new Supplier(Convert.ToInt32(r["sup_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierInvoice;
                tr.NOTES = r["sij_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["sij_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["sij_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["sij_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["sij_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["sij_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["sij_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["sij_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["sij_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["sij_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.SUPPLIER_INVOICE = new SupplierInvoice(Convert.ToInt32(r["si_id"]));
                tr.AGAINST_PAYMENT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), r["sij_againstpaymentstatus"].ToString());
                tr.MODIFIED_BY = r["modified_by"].ToString();
                tr.MODIFIED_DATE = Convert.ToDateTime(r["modified_date"].ToString());
                tr.MODIFIED_COMPUTER_NAME = r["modified_computer"].ToString();
                result.Add(tr);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(sij_id) from table_supplierinvoicejournal");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_supplierinvoicejournal where sij_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_supplierinvoicejournal where sij_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT sij_eventstatus from table_supplierinvoicejournal where sij_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(EventJournal e)
        {
            return String.Format(@"update table_supplierinvoicejournal set 
                    sij_posted= {0},
                    sij_eventstatus= '{1}'
                where sij_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_supplierinvoicejournal p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.sij_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_supplierinvoicejournal where sij_code ='{0}'", code);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_supplierinvoicejournal p where p.sij_code like '%{0}%' ORDER BY p.sij_id DESC", code);
        }
        public static string FindPeriodSIJId(int id)
        {
            return String.Format(@"select * from table_supplierinvoicejournal p where p.si_id = {0}", id);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_supplierinvoicejournal p";
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"update table_supplierinvoicejournal set 
                    sij_againstpaymentstatus = '{0}'
                where si_id = {1}",
                          AGAINST_PAYMENT_STATUS.ToString(),
                           ID);
        }
    }
}
