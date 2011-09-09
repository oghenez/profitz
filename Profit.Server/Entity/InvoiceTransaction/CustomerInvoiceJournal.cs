using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class CustomerInvoiceJournal : EventJournal
    {
        public CustomerInvoice CUSTOMER_INVOICE;
        public AgainstStatus AGAINST_RECEIPT_STATUS = AgainstStatus.Open;

        public CustomerInvoiceJournal()
        { }
        public CustomerInvoiceJournal(int id)
            : base()
        {
            ID = id;
        }
        public void UpdateAgainstReceiptStatusSIJ()
        {
            bool allClosed = true;
            for (int i = 0; i < EVENT_JOURNAL_ITEMS.Count; i++)
            {
                CustomerInvoiceJournalItem poi = EVENT_JOURNAL_ITEMS[i] as CustomerInvoiceJournalItem;
                if (poi.AGAINST_RECEIPT_STATUS == AgainstStatus.Close) continue;
                allClosed = false;
                break;
            }
            AGAINST_RECEIPT_STATUS = allClosed ? AgainstStatus.Close : AgainstStatus.Outstanding;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_customerinvoicejournal 
                (  
                    cij_code,
                    cij_date,
                    cus_id,
                    ccy_id,
                    entry_type,
                    cij_notes,
                    cij_posted,
                    cij_eventstatus,
                    cij_subtotalamount,
                    cij_discpercent,
                    cij_amountafterdiscpercent,
                    cij_discamount,
                    cij_amountafterdiscamount,
                    cij_otherexpense,
                    cij_netamount,
                    emp_id,
                    ci_id,
                    cij_againstreceiptstatus
                ) 
                VALUES ('{0}','{1}',{2},{3},'{4}','{5}',{6},'{7}',{8},{9},{10},{11},{12},{13},{14},{15},{16},'{17}')",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.CustomerInvoice.ToString(),
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
                CUSTOMER_INVOICE == null ? 0 : CUSTOMER_INVOICE.ID,
                AGAINST_RECEIPT_STATUS.ToString()
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_customerinvoicejournal set 
                   cij_code = '{0}',
                    cij_date= '{1}',
                    cus_id = {2},
                    ccy_id= {3},
                    entry_type= '{4}',
                    cij_notes= '{5}',
                    cij_posted= {6},
                    cij_eventstatus= '{7}',
                    cij_subtotalamount= {8},
                    cij_discpercent= {9},
                    cij_amountafterdiscpercent= {10},
                    cij_discamount= {11},
                    cij_amountafterdiscamount= {12},
                    cij_otherexpense= {13},
                    cij_netamount= {14},
                    emp_id = {15},
                     ci_id = {16},
                    cij_againstreceiptstatus = '{17}'
                where cij_id = {18}",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.CustomerInvoice.ToString(),
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
                CUSTOMER_INVOICE == null ? 0 : CUSTOMER_INVOICE.ID,
                AGAINST_RECEIPT_STATUS.ToString(),
                ID);
        }
        public static CustomerInvoiceJournal TransformReader(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            CustomerInvoiceJournal tr = null;
            if (r.HasRows)
            {
                r.Read();
                tr = new CustomerInvoiceJournal();
                tr.ID = Convert.ToInt32(r["cij_id"]);
                tr.CODE = r["cij_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["cij_date"]);
                tr.VENDOR = new Customer(Convert.ToInt32(r["cus_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerInvoice;
                tr.NOTES = r["cij_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["cij_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["cij_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["cij_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["cij_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["cij_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["cij_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["cij_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["cij_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["cij_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.CUSTOMER_INVOICE = new CustomerInvoice(Convert.ToInt32(r["ci_id"]));
                tr.AGAINST_RECEIPT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), r["cij_againstreceiptstatus"].ToString());
            }
            return tr;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            IList result = new ArrayList();
            while (r.Read())
            {
                CustomerInvoiceJournal tr = new CustomerInvoiceJournal();
                tr.ID = Convert.ToInt32(r["cij_id"]);
                tr.CODE = r["cij_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["cij_date"]);
                tr.VENDOR = new Customer(Convert.ToInt32(r["cus_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerInvoice;
                tr.NOTES = r["cij_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["cij_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["cij_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["cij_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["cij_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["cij_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["cij_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["cij_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["cij_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["cij_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.CUSTOMER_INVOICE = new CustomerInvoice(Convert.ToInt32(r["ci_id"]));
                tr.AGAINST_RECEIPT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), r["cij_againstreceiptstatus"].ToString());
                result.Add(tr);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(cij_id) from table_customerinvoicejournal");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_customerinvoicejournal where cij_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_customerinvoicejournal where cij_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT cij_eventstatus from table_customerinvoicejournal where cij_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(EventJournal e)
        {
            return String.Format(@"update table_customerinvoicejournal set 
                    cij_posted= {0},
                    cij_eventstatus= '{1}'
                where cij_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_customerinvoicejournal p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.cij_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_customerinvoicejournal where cij_code ='{0}'", code);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_customerinvoicejournal p where p.cij_code like '%{0}%' ORDER BY p.cij_id DESC", code);
        }
        public static string FindPeriodCIJId(int id)
        {
            return String.Format(@"select * from table_customerinvoicejournal p where p.ci_id = {0}", id);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_customerinvoicejournal p";
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"update table_customerinvoicejournal set 
                    cij_againstreceiptstatus = '{0}'
                where ci_id = {1}",
                          AGAINST_RECEIPT_STATUS.ToString(),
                           ID);
        }
    }
}
