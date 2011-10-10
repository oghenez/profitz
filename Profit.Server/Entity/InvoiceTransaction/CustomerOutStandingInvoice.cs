using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class CustomerOutStandingInvoice : EventJournal
    {
        public AgainstStatus AGAINST_RECEIPT_STATUS = AgainstStatus.Open;

        public CustomerOutStandingInvoice()
        { }
        public CustomerOutStandingInvoice(int id)
            : base()
        {
            ID = id;
        }
        public void UpdateAgainstReceiptStatusSIJ()
        {
            bool allClosed = true;
            for (int i = 0; i < EVENT_JOURNAL_ITEMS.Count; i++)
            {
                CustomerOutStandingInvoiceItem poi = EVENT_JOURNAL_ITEMS[i] as CustomerOutStandingInvoiceItem;
                if (poi.AGAINST_RECEIPT_STATUS == AgainstStatus.Close) continue;
                allClosed = false;
                break;
            }
            AGAINST_RECEIPT_STATUS = allClosed ? AgainstStatus.Close : AgainstStatus.Outstanding;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_customeroutstandinginvoice 
                (  
                    costi_code,
                    costi_date,
                    cus_id,
                    ccy_id,
                    entry_type,
                    costi_notes,
                    costi_posted,
                    costi_eventstatus,
                    costi_subtotalamount,
                    costi_discpercent,
                    costi_amountafterdiscpercent,
                    costi_discamount,
                    costi_amountafterdiscamount,
                    costi_otherexpense,
                    costi_netamount,
                    emp_id,
                    costi_againstreceiptstatus, 
                    modified_by, 
                    modified_date, 
                    modified_computer
                ) 
                VALUES ('{0}','{1}',{2},{3},'{4}','{5}',{6},'{7}',{8},{9},{10},{11},{12},{13},{14},{15},'{16}','{17}','{18}','{19}')",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.CustomerOutStandingInvoice.ToString(),
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
                AGAINST_RECEIPT_STATUS.ToString(),
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_customeroutstandinginvoice set 
                   costi_code = '{0}',
                    costi_date= '{1}',
                    cus_id = {2},
                    ccy_id= {3},
                    entry_type= '{4}',
                    costi_notes= '{5}',
                    costi_posted= {6},
                    costi_eventstatus= '{7}',
                    costi_subtotalamount= {8},
                    costi_discpercent= {9},
                    costi_amountafterdiscpercent= {10},
                    costi_discamount= {11},
                    costi_amountafterdiscamount= {12},
                    costi_otherexpense= {13},
                    costi_netamount= {14},
                    emp_id= {15},
                    costi_againstreceiptstatus = '{16}',
                modified_by='{17}', 
                modified_date='{18}',
                modified_computer='{19}'
                where costi_id = {20}",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.CustomerOutStandingInvoice.ToString(),
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
                AGAINST_RECEIPT_STATUS.ToString(),
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public static CustomerOutStandingInvoice TransformReader(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            CustomerOutStandingInvoice tr = null;
            if (r.HasRows)
            {
                r.Read();
                tr = new CustomerOutStandingInvoice();
                tr.ID = Convert.ToInt32(r["costi_id"]);
                tr.CODE = r["costi_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["costi_date"]);
                tr.VENDOR = new Customer(Convert.ToInt32(r["cus_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerOutStandingInvoice;
                tr.NOTES = r["costi_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["costi_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["costi_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["costi_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["costi_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["costi_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["costi_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["costi_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["costi_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["costi_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.AGAINST_RECEIPT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), r["costi_againstreceiptstatus"].ToString());
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
                CustomerOutStandingInvoice tr = new CustomerOutStandingInvoice();
                tr.ID = Convert.ToInt32(r["costi_id"]);
                tr.CODE = r["costi_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["costi_date"]);
                tr.VENDOR = new Customer(Convert.ToInt32(r["cus_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerOutStandingInvoice;
                tr.NOTES = r["costi_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["costi_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["costi_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["costi_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["costi_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["costi_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["costi_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["costi_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["costi_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["costi_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.AGAINST_RECEIPT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), r["costi_againstreceiptstatus"].ToString());
                tr.MODIFIED_BY = r["modified_by"].ToString();
                tr.MODIFIED_DATE = Convert.ToDateTime(r["modified_date"].ToString());
                tr.MODIFIED_COMPUTER_NAME = r["modified_computer"].ToString();
                result.Add(tr);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(costi_id) from table_customeroutstandinginvoice");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_customeroutstandinginvoice where costi_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_customeroutstandinginvoice where costi_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT costi_eventstatus from table_customeroutstandinginvoice where costi_id ={0}", id);
        }
        public static string GetByCustomerSQL(int id)
        {
            return String.Format("SELECT * from table_customeroutstandinginvoice where cus_id ={0}", id);
        }
        public static string GetByCustomerSQL(DateTime startDate, DateTime endDate, int supid,
     bool allStatus, bool status)
        {
            return String.Format(@"SELECT * from table_customeroutstandinginvoice where 
            costi_date between '{0}' and '{1}' {2} {3}",
                startDate.ToString(Utils.DATE_FORMAT), endDate.ToString(Utils.DATE_FORMAT_SHORT_END),
                supid == 0 ? "" : " and cus_id = " + supid,
                allStatus ? "" : " and costi_posted = " + status);
        }
        public static string GetUpdateStatusSQL(EventJournal e)
        {
            return String.Format(@"update table_customeroutstandinginvoice set 
                    costi_posted= {0},
                    costi_eventstatus= '{1}'
                where costi_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_customeroutstandinginvoice p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                INNER JOIN table_customer s on s.cus_id = p.cus_id
                where concat(p.costi_code, e.emp_code, e.emp_name, s.cus_code, s.cus_name)
                like '%{0}%'", find);
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_customeroutstandinginvoice where costi_code ='{0}'", code);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_customeroutstandinginvoice p where p.costi_code like '%{0}%' ORDER BY p.costi_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_customeroutstandinginvoice p";
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"update table_customeroutstandinginvoice set 
                    costi_againstreceiptstatus = '{0}'
                where costi_id = {1}",
                          AGAINST_RECEIPT_STATUS.ToString(),
                           ID);
        }
    }
}
