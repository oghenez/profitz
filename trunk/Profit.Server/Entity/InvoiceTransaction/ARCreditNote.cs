using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class ARCreditNote : EventJournal
    {
        public bool USED_FOR_RECEIPT = false;
        public ARCreditNote()
        { }
        public ARCreditNote(int id)
            : base()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_arcreditnote 
                (  
                    arcr_code,
                    arcr_date,
                    cus_id,
                    ccy_id,
                    entry_type,
                    arcr_notes,
                    arcr_posted,
                    arcr_eventstatus,
                    arcr_subtotalamount,
                    arcr_discpercent,
                    arcr_amountafterdiscpercent,
                    arcr_discamount,
                    arcr_amountafterdiscamount,
                    arcr_otherexpense,
                    arcr_netamount,
                    emp_id,
                    arcr_usedforreceipt
                ) 
                VALUES ('{0}','{1}',{2},{3},'{4}','{5}',{6},'{7}',{8},{9},{10},{11},{12},{13},{14},{15},{16})",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.ARCreditNote.ToString(),
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
                USED_FOR_RECEIPT
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_arcreditnote set 
                   arcr_code = '{0}',
                    arcr_date= '{1}',
                    cus_id = {2},
                    ccy_id= {3},
                    entry_type= '{4}',
                    arcr_notes= '{5}',
                    arcr_posted= {6},
                    arcr_eventstatus= '{7}',
                    arcr_subtotalamount= {8},
                    arcr_discpercent= {9},
                    arcr_amountafterdiscpercent= {10},
                    arcr_discamount= {11},
                    arcr_amountafterdiscamount= {12},
                    arcr_otherexpense= {13},
                    arcr_netamount= {14},
                    emp_id= {15},
                    arcr_usedforreceipt = {16}
                where arcr_id = {17}",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.ARCreditNote.ToString(),
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
                USED_FOR_RECEIPT,
                ID);
        }
        public static ARCreditNote TransformReader(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            ARCreditNote tr = null;
            if (r.HasRows)
            {
                r.Read();
                tr = new ARCreditNote();
                tr.ID = Convert.ToInt32(r["arcr_id"]);
                tr.CODE = r["arcr_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["arcr_date"]);
                tr.VENDOR = new Customer(Convert.ToInt32(r["cus_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.ARCreditNote;
                tr.NOTES = r["arcr_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["arcr_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["arcr_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["arcr_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["arcr_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["arcr_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["arcr_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["arcr_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["arcr_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["arcr_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.USED_FOR_RECEIPT = Convert.ToBoolean(r["arcr_usedforreceipt"]);
            }
            return tr;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            IList result = new ArrayList();
            if (r == null) return result;
            while (r.Read())
            {
                ARCreditNote tr = new ARCreditNote();
                tr.ID = Convert.ToInt32(r["arcr_id"]);
                tr.CODE = r["arcr_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["arcr_date"]);
                tr.VENDOR = new Customer(Convert.ToInt32(r["cus_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.ARCreditNote;
                tr.NOTES = r["arcr_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["arcr_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["arcr_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["arcr_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["arcr_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["arcr_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["arcr_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["arcr_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["arcr_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["arcr_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.USED_FOR_RECEIPT = Convert.ToBoolean(r["arcr_usedforreceipt"]);
                result.Add(tr);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(arcr_id) from table_arcreditnote");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_arcreditnote where arcr_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_arcreditnote where arcr_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT arcr_eventstatus from table_arcreditnote where arcr_id ={0}", id);
        }
        public static string GetByCustomerSQL(int id)
        {
            return String.Format("SELECT * from table_arcreditnote where cus_id ={0}", id);
        }
        public static string GetByCustomerSQL(DateTime startDate, DateTime endDate, int supid,
       bool allStatus, bool status)
        {
            return String.Format(@"SELECT * from table_arcreditnote where 
            arcr_date between '{0}' and '{1}' {2} {3}",
                startDate.ToString(Utils.DATE_FORMAT), endDate.ToString(Utils.DATE_FORMAT),
                supid == 0 ? "" : " and cus_id = " + supid,
                allStatus ? "" : " and arcr_posted = " + status);
        }
        public static string GetUpdateStatusSQL(EventJournal e)
        {
            return String.Format(@"update table_arcreditnote set 
                    arcr_posted= {0},
                    arcr_eventstatus= '{1}'
                where arcr_id = {2}",
                e.POSTED,
                e.POSTED?EventStatus.Confirm: EventStatus.Entry,
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_arcreditnote p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.arcr_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
        public static string GetForReceipt(int supID, DateTime dt, string find, string notin)
        {
            return String.Format(@"select * from table_arcreditnote p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where 
                p.cus_id = {0} and
                p.arcr_date <= '{1}'  and         
                p.arcr_posted = true and
                p.arcr_usedforreceipt = false and
                concat(p.arcr_code, e.emp_code, e.emp_name) like '%{2}%' {3}
                ",
                 supID, dt.ToString(Utils.DATE_FORMAT), find, notin == "" ? "" : "and arcr_id not in ("+notin+")");
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_arcreditnote where arcr_code ='{0}'", code);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_arcreditnote p where p.arcr_code like '%{0}%' ORDER BY p.arcr_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_arcreditnote p";
        }
        public static string UpdateUsedForReceipt(int apdniID, bool use)
        {
            return String.Format(@"update table_arcreditnote set 
                    arcr_usedforreceipt = {0}
                where arcr_id = {1}",
                                    use,apdniID
                          );
        }
    }
}
