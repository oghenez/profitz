using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Receipt : EventJournal
    {
        public Receipt()
        { }
        public Receipt(int id)
            : base()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_receipt 
                (  
                    rec_code,
                    rec_date,
                    cus_id,
                    ccy_id,
                    entry_type,
                    rec_notes,
                    rec_posted,
                    rec_eventstatus,
                    rec_subtotalamount,
                    rec_discpercent,
                    rec_amountafterdiscpercent,
                    rec_discamount,
                    rec_amountafterdiscamount,
                    rec_otherexpense,
                    rec_netamount,
                    emp_id, 
                    modified_by, 
                    modified_date, 
                    modified_computer
                ) 
                VALUES ('{0}','{1}',{2},{3},'{4}','{5}',{6},'{7}',{8},{9},{10},{11},{12},{13},{14},{15},'{16}','{17}','{18}')",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.Receipt.ToString(),
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
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_receipt set 
                   rec_code = '{0}',
                    rec_date= '{1}',
                    cus_id = {2},
                    ccy_id= {3},
                    entry_type= '{4}',
                    rec_notes= '{5}',
                    rec_posted= {6},
                    rec_eventstatus= '{7}',
                    rec_subtotalamount= {8},
                    rec_discpercent= {9},
                    rec_amountafterdiscpercent= {10},
                    rec_discamount= {11},
                    rec_amountafterdiscamount= {12},
                    rec_otherexpense= {13},
                    rec_netamount= {14},
                    emp_id= {15},
                modified_by='{16}', 
                modified_date='{17}',
                modified_computer='{18}'
                where rec_id = {19}",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.Receipt.ToString(),
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
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public static Receipt TransformReader(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            Receipt tr = null;
            if (r.HasRows)
            {
                r.Read();
                tr = new Receipt();
                tr.ID = Convert.ToInt32(r["rec_id"]);
                tr.CODE = r["rec_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["rec_date"]);
                tr.VENDOR = new Customer(Convert.ToInt32(r["cus_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Receipt;
                tr.NOTES = r["rec_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["rec_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["rec_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["rec_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["rec_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["rec_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["rec_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["rec_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["rec_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["rec_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
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
                Receipt tr = new Receipt();
                tr.ID = Convert.ToInt32(r["rec_id"]);
                tr.CODE = r["rec_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["rec_date"]);
                tr.VENDOR = new Customer(Convert.ToInt32(r["cus_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Receipt;
                tr.NOTES = r["rec_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["rec_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["rec_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["rec_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["rec_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["rec_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["rec_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["rec_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["rec_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["rec_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.MODIFIED_BY = r["modified_by"].ToString();
                tr.MODIFIED_DATE = Convert.ToDateTime(r["modified_date"].ToString());
                tr.MODIFIED_COMPUTER_NAME = r["modified_computer"].ToString();
                result.Add(tr);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(rec_id) from table_receipt");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_receipt where rec_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_receipt where rec_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT rec_eventstatus from table_receipt where rec_id ={0}", id);
        }
        public static string GetByCustomerSQL(int id)
        {
            return String.Format("SELECT * from table_receipt where cus_id ={0}", id);
        }
        public static string GetByCustomerSQL(DateTime startDate, DateTime endDate, int supid,
      bool allStatus, bool status)
        {
            return String.Format(@"SELECT * from table_receipt where 
            rec_date between '{0}' and '{1}' {2} {3}",
                startDate.ToString(Utils.DATE_FORMAT), endDate.ToString(Utils.DATE_FORMAT),
                supid == 0 ? "" : " and cus_id = " + supid,
                allStatus ? "" : " and rec_posted = " + status);
        }
        public static string GetUpdateStatusSQL(EventJournal e)
        {
            return String.Format(@"update table_receipt set 
                    rec_posted= {0},
                    rec_eventstatus= '{1}'
                where rec_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_receipt p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.rec_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_receipt where rec_code ='{0}'", code);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_receipt p where p.rec_code like '%{0}%' ORDER BY p.rec_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_receipt p";
        }
        public static string FindReceiptId(int id)
        {
            return String.Format(@"select * from table_receipt p where p.ci_id = {0}", id);
        }
    }
}
