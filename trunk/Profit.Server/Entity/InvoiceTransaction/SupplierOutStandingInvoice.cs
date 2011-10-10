using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SupplierOutStandingInvoice : EventJournal
    {
        public AgainstStatus AGAINST_PAYMENT_STATUS = AgainstStatus.Open;

        public SupplierOutStandingInvoice()
        { }
        public SupplierOutStandingInvoice(int id)
            : base()
        {
            ID = id;
        }
        public void UpdateAgainstPaymentStatusSIJ()
        {
            bool allClosed = true;
            for (int i = 0; i < EVENT_JOURNAL_ITEMS.Count; i++)
            {
                SupplierOutStandingInvoiceItem poi = EVENT_JOURNAL_ITEMS[i] as SupplierOutStandingInvoiceItem;
                if (poi.AGAINST_PAYMENT_STATUS == AgainstStatus.Close) continue;
                allClosed = false;
                break;
            }
            AGAINST_PAYMENT_STATUS = allClosed ? AgainstStatus.Close : AgainstStatus.Outstanding;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_supplieroutstandinginvoice 
                (  
                    sosti_code,
                    sosti_date,
                    sup_id,
                    ccy_id,
                    entry_type,
                    sosti_notes,
                    sosti_posted,
                    sosti_eventstatus,
                    sosti_subtotalamount,
                    sosti_discpercent,
                    sosti_amountafterdiscpercent,
                    sosti_discamount,
                    sosti_amountafterdiscamount,
                    sosti_otherexpense,
                    sosti_netamount,
                    emp_id,
                    sosti_againstpaymentstatus, 
                    modified_by, 
                    modified_date, 
                    modified_computer
                ) 
                VALUES ('{0}','{1}',{2},{3},'{4}','{5}',{6},'{7}',{8},{9},{10},{11},{12},{13},{14},{15},'{16}','{17}','{18}','{19}')",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.SupplierOutStandingInvoice.ToString(),
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
                AGAINST_PAYMENT_STATUS.ToString(),
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_supplieroutstandinginvoice set 
                   sosti_code = '{0}',
                    sosti_date= '{1}',
                    sup_id = {2},
                    ccy_id= {3},
                    entry_type= '{4}',
                    sosti_notes= '{5}',
                    sosti_posted= {6},
                    sosti_eventstatus= '{7}',
                    sosti_subtotalamount= {8},
                    sosti_discpercent= {9},
                    sosti_amountafterdiscpercent= {10},
                    sosti_discamount= {11},
                    sosti_amountafterdiscamount= {12},
                    sosti_otherexpense= {13},
                    sosti_netamount= {14},
                    emp_id= {15},
                    sosti_againstpaymentstatus = '{16}',
                modified_by='{17}', 
                modified_date='{18}',
                modified_computer='{19}'
                where sosti_id = {20}",
                CODE,
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                VENDOR.ID,
                CURRENCY.ID,
                VendorBalanceEntryType.SupplierOutStandingInvoice.ToString(),
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
                AGAINST_PAYMENT_STATUS.ToString(),
                MODIFIED_BY,
                DateTime.Now.ToString(Utils.DATE_FORMAT),
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public static SupplierOutStandingInvoice TransformReader(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            SupplierOutStandingInvoice tr = null;
            if (r.HasRows)
            {
                r.Read();
                tr = new SupplierOutStandingInvoice();
                tr.ID = Convert.ToInt32(r["sosti_id"]);
                tr.CODE = r["sosti_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["sosti_date"]);
                tr.VENDOR = new Supplier(Convert.ToInt32(r["sup_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierOutStandingInvoice;
                tr.NOTES = r["sosti_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["sosti_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["sosti_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["sosti_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["sosti_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["sosti_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["sosti_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["sosti_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["sosti_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["sosti_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.AGAINST_PAYMENT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), r["sosti_againstpaymentstatus"].ToString());
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
                SupplierOutStandingInvoice tr = new SupplierOutStandingInvoice();
                tr.ID = Convert.ToInt32(r["sosti_id"]);
                tr.CODE = r["sosti_code"].ToString();
                tr.TRANSACTION_DATE = Convert.ToDateTime(r["sosti_date"]);
                tr.VENDOR = new Supplier(Convert.ToInt32(r["sup_id"]));
                tr.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierOutStandingInvoice;
                tr.NOTES = r["sosti_notes"].ToString();
                tr.POSTED = Convert.ToBoolean(r["sosti_posted"]);
                tr.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), r["sosti_eventstatus"].ToString());
                tr.SUBTOTAL_AMOUNT = Convert.ToDouble(r["sosti_subtotalamount"]);
                tr.DISC_PERCENT = Convert.ToDouble(r["sosti_discpercent"]);
                tr.AMOUNT_AFTER_DISC_PERCENT = Convert.ToDouble(r["sosti_amountafterdiscpercent"]);
                tr.DISC_AMOUNT = Convert.ToDouble(r["sosti_discamount"]);
                tr.AMOUNT_AFTER_DISC_AMOUNT = Convert.ToDouble(r["sosti_amountafterdiscamount"]);
                tr.OTHER_EXPENSE = Convert.ToDouble(r["sosti_otherexpense"]);
                tr.NET_AMOUNT = Convert.ToDouble(r["sosti_netamount"]);
                tr.EMPLOYEE = new Employee(Convert.ToInt32(r["emp_id"]));
                tr.AGAINST_PAYMENT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), r["sosti_againstpaymentstatus"].ToString());
                tr.MODIFIED_BY = r["modified_by"].ToString();
                tr.MODIFIED_DATE = Convert.ToDateTime(r["modified_date"].ToString());
                tr.MODIFIED_COMPUTER_NAME = r["modified_computer"].ToString();
                result.Add(tr);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(sosti_id) from table_supplieroutstandinginvoice");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_supplieroutstandinginvoice where sosti_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_supplieroutstandinginvoice where sosti_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT sosti_eventstatus from table_supplieroutstandinginvoice where sosti_id ={0}", id);
        }
        public static string GetBySupplierSQL(int id)
        {
            return String.Format("SELECT * from table_supplieroutstandinginvoice where sup_id ={0}", id);
        }
        public static string GetBySupplierSQL(DateTime startDate, DateTime endDate, int supid,
           bool allStatus, bool status)
        {
            return String.Format(@"SELECT * from table_supplieroutstandinginvoice where 
            sosti_date between '{0}' and '{1}' {2} {3}",
                startDate.ToString(Utils.DATE_FORMAT), endDate.ToString(Utils.DATE_FORMAT_SHORT_END),
                supid == 0 ? "" : " and sup_id = " + supid,
                allStatus ? "" : " and sosti_posted = " + status);
        }
        public static string GetUpdateStatusSQL(EventJournal e)
        {
            return String.Format(@"update table_supplieroutstandinginvoice set 
                    sosti_posted= {0},
                    sosti_eventstatus= '{1}'
                where sosti_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_supplieroutstandinginvoice p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                INNER JOIN table_supplier s on s.sup_id = p.sup_id
                where concat(p.sosti_code, e.emp_code, e.emp_name, s.sup_code, s.sup_name)
                like '%{0}%'", find);
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_supplieroutstandinginvoice where sosti_code ='{0}'", code);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_supplieroutstandinginvoice p where p.sosti_code like '%{0}%' ORDER BY p.sosti_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_supplieroutstandinginvoice p";
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"update table_supplieroutstandinginvoice set 
                    sosti_againstpaymentstatus = '{0}'
                where sosti_id = {1}",
                          AGAINST_PAYMENT_STATUS.ToString(),
                           ID);
        }
    }
}
