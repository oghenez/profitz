using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class APDebitNote : EventJournal
    {
        public bool USED_FOR_PAYMENT = false;
        public APDebitNote()
        { }
        public APDebitNote(int id)
            : base()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_apdebitnote 
                (  
                    apdn_code,
                    apdn_date,
                    sup_id,
                    ccy_id,
                    entry_type,
                    apdn_notes,
                    apdn_posted,
                    apdn_eventstatus,
                    apdn_subtotalamount,
                    apdn_discpercent,
                    apdn_amountafterdiscpercent,
                    apdn_discamount,
                    apdn_amountafterdiscamount,
                    apdn_otherexpense,
                    apdn_netamount,
                    emp_id,
                    apdn_usedforpayment, 
                    modified_by, 
                    modified_date, 
                    modified_computer
                ) 
                VALUES ('{0}','{1}',{2},{3},'{4}','{5}',{6},'{7}',{8},{9},{10},{11},{12},{13},{14},{15},{16}
                ,'{17}','{18}','{19}')",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.APDebitNote.ToString(),
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
                USED_FOR_PAYMENT,
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_apdebitnote set 
                   apdn_code = '{0}',
                    apdn_date= '{1}',
                    sup_id = {2},
                    ccy_id= {3},
                    entry_type= '{4}',
                    apdn_notes= '{5}',
                    apdn_posted= {6},
                    apdn_eventstatus= '{7}',
                    apdn_subtotalamount= {8},
                    apdn_discpercent= {9},
                    apdn_amountafterdiscpercent= {10},
                    apdn_discamount= {11},
                    apdn_amountafterdiscamount= {12},
                    apdn_otherexpense= {13},
                    apdn_netamount= {14},
                    emp_id= {15},
                    apdn_usedforpayment = {16},
                modified_by='{17}', 
                modified_date='{18}',
                modified_computer='{19}'
                where apdn_id = {20}",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.APDebitNote.ToString(),
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
                USED_FOR_PAYMENT,
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public static APDebitNote TransformReader(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            APDebitNote tr = null;
            if (r.HasRows)
            {
                r.Read();
                tr = new APDebitNote();
                tr.ID = Convert.ToInt32(r["apdn_id"]);
                tr.CODE = r["apdn_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["apdn_date"]);
                tr.VENDOR = new Supplier(Convert.ToInt32(r["sup_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.APDebitNote;
                tr.NOTES = r["apdn_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["apdn_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["apdn_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["apdn_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["apdn_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["apdn_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["apdn_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["apdn_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["apdn_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["apdn_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.USED_FOR_PAYMENT = Convert.ToBoolean(r["apdn_usedforpayment"]);
                tr.MODIFIED_BY = r["modified_by"].ToString();
                tr.MODIFIED_DATE = Convert.ToDateTime(r["modified_date"].ToString());
                tr.MODIFIED_COMPUTER_NAME = r["modified_computer"].ToString();
            }
            return tr;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            IList result = new ArrayList();
            if (r == null) return result;
            while (r.Read())
            {
                APDebitNote tr = new APDebitNote();
                tr.ID = Convert.ToInt32(r["apdn_id"]);
                tr.CODE = r["apdn_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["apdn_date"]);
                tr.VENDOR = new Supplier(Convert.ToInt32(r["sup_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.APDebitNote;
                tr.NOTES = r["apdn_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["apdn_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["apdn_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["apdn_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["apdn_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["apdn_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["apdn_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["apdn_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["apdn_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["apdn_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.USED_FOR_PAYMENT = Convert.ToBoolean(r["apdn_usedforpayment"]);
                tr.MODIFIED_BY = r["modified_by"].ToString();
                tr.MODIFIED_DATE = Convert.ToDateTime(r["modified_date"].ToString());
                tr.MODIFIED_COMPUTER_NAME = r["modified_computer"].ToString();
                result.Add(tr);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(apdn_id) from table_apdebitnote");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_apdebitnote where apdn_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_apdebitnote where apdn_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT apdn_eventstatus from table_apdebitnote where apdn_id ={0}", id);
        }
        public static string GetBySupplierSQL(int id)
        {
            return String.Format("SELECT * from table_apdebitnote where sup_id ={0}", id);
        }
        public static string GetBySupplierSQL(DateTime startDate, DateTime endDate, int supid,
            bool allStatus, bool status)
        {
            return String.Format(@"SELECT * from table_apdebitnote where 
            apdn_date between '{0}' and '{1}' {2} {3}",
                startDate.ToString(Utils.DATE_FORMAT), endDate.ToString(Utils.DATE_FORMAT_SHORT_END),
                supid == 0 ? "" : " and sup_id = " + supid,
                allStatus ? "" : " and apdn_posted = " + status);
        }
        public static string GetUpdateStatusSQL(EventJournal e)
        {
            return String.Format(@"update table_apdebitnote set 
                    apdn_posted= {0},
                    apdn_eventstatus= '{1}'
                where apdn_id = {2}",
                e.POSTED,
                e.POSTED?EventStatus.Confirm: EventStatus.Entry,
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_apdebitnote p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.apdn_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
        public static string GetForPayment(int supID, DateTime dt, string find, string notin)
        {
            return String.Format(@"select * from table_apdebitnote p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where 
                p.sup_id = {0} and
                p.apdn_date <= '{1}'  and         
                p.apdn_posted = true and
                p.apdn_usedforpayment = false and
                concat(p.apdn_code, e.emp_code, e.emp_name) like '%{2}%' {3}
                ",
                 supID, dt.ToString(Utils.DATE_FORMAT), find, notin == "" ? "" : "and apdn_id not in ("+notin+")");
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_apdebitnote where apdn_code ='{0}'", code);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_apdebitnote p where p.apdn_code like '%{0}%' ORDER BY p.apdn_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_apdebitnote p";
        }
        public static string UpdateUsedForPayment(int apdniID, bool use)
        {
            return String.Format(@"update table_apdebitnote set 
                    apdn_usedforpayment = {0}
                where apdn_id = {1}",
                                    use,apdniID
                          );
        }
    }
}
