using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Payment : EventJournal
    {
        public Payment()
        { }
        public Payment(int id)
            : base()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_payment 
                (  
                    pay_code,
                    pay_date,
                    sup_id,
                    ccy_id,
                    entry_type,
                    pay_notes,
                    pay_posted,
                    pay_eventstatus,
                    pay_subtotalamount,
                    pay_discpercent,
                    pay_amountafterdiscpercent,
                    pay_discamount,
                    pay_amountafterdiscamount,
                    pay_otherexpense,
                    pay_netamount,
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
                VendorBalanceEntryType.Payment.ToString(),
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
            return String.Format(@"update table_payment set 
                   pay_code = '{0}',
                    pay_date= '{1}',
                    sup_id = {2},
                    ccy_id= {3},
                    entry_type= '{4}',
                    pay_notes= '{5}',
                    pay_posted= {6},
                    pay_eventstatus= '{7}',
                    pay_subtotalamount= {8},
                    pay_discpercent= {9},
                    pay_amountafterdiscpercent= {10},
                    pay_discamount= {11},
                    pay_amountafterdiscamount= {12},
                    pay_otherexpense= {13},
                    pay_netamount= {14},
                    emp_id= {15},
                modified_by='{16}', 
                modified_date='{17}',
                modified_computer='{18}'
                where pay_id = {19}",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.Payment.ToString(),
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
        public static Payment TransformReader(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            Payment tr = null;
            if (r.HasRows)
            {
                r.Read();
                tr = new Payment();
                tr.ID = Convert.ToInt32(r["pay_id"]);
                tr.CODE = r["pay_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["pay_date"]);
                tr.VENDOR = new Supplier(Convert.ToInt32(r["sup_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Payment;
                tr.NOTES = r["pay_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["pay_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["pay_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["pay_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["pay_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["pay_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["pay_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["pay_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["pay_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["pay_netamount"]);
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
                Payment tr = new Payment();
                tr.ID = Convert.ToInt32(r["pay_id"]);
                tr.CODE = r["pay_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["pay_date"]);
                tr.VENDOR = new Supplier(Convert.ToInt32(r["sup_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Payment;
                tr.NOTES = r["pay_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["pay_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["pay_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["pay_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["pay_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["pay_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["pay_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["pay_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["pay_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["pay_netamount"]);
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
            return String.Format("SELECT max(pay_id) from table_payment");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_payment where pay_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_payment where pay_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT pay_eventstatus from table_payment where pay_id ={0}", id);
        }
        public static string GetBySupplierSQL(int id)
        {
            return String.Format("SELECT * from table_payment where sup_id ={0}", id);
        }
        public static string GetBySupplierSQL(DateTime startDate, DateTime endDate, int supid,
           bool allStatus, bool status)
        {
            return String.Format(@"SELECT * from table_payment where 
            pay_date between '{0}' and '{1}' {2} {3}",
                startDate.ToString(Utils.DATE_FORMAT), endDate.ToString(Utils.DATE_FORMAT),
                supid == 0 ? "" : " and sup_id = " + supid,
                allStatus ? "" : " and pay_posted = " + status);
        }
        public static string GetUpdateStatusSQL(EventJournal e)
        {
            return String.Format(@"update table_payment set 
                    pay_posted= {0},
                    pay_eventstatus= '{1}'
                where pay_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_payment p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.pay_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_payment where pay_code ='{0}'", code);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_payment p where p.pay_code like '%{0}%' ORDER BY p.pay_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_payment p";
        }
    }
}
