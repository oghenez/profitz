using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class CustomerInvoiceJournalItem : EventJournalItem, ICustomerInvoiceJournalItem
    {
        public AgainstStatus AGAINST_PAYMENT_STATUS = AgainstStatus.Open;
        public double OUTSTANDING_AMOUNT = 0;
        public double PAID_AMOUNT = 0;

        public CustomerInvoiceJournalItem()
            : base()
        {
            VENDOR_BALANCE_TYPE = VendorBalanceType.Customer;
        }
        public CustomerInvoiceJournalItem(int id)
            : this()
        {
            ID = id;
        }
        public void SetOSAgainstPaymentItem(IPayment pyi)
        {
            double qtyAmount = pyi.GET_AMOUNT;
            if (qtyAmount <= 0) return;
            if (AGAINST_PAYMENT_STATUS == AgainstStatus.Close)
                throw new Exception("Invoice Item Allready Close :" + this.INVOICE_NO);
            if (qtyAmount > OUTSTANDING_AMOUNT)
                throw new Exception("Payment Item Amount exceed SIJ Outstanding Item Amount :" + this.INVOICE_NO);
            OUTSTANDING_AMOUNT = OUTSTANDING_AMOUNT - qtyAmount;
            PAID_AMOUNT = PAID_AMOUNT + qtyAmount;
            if (isValidToClose())
                AGAINST_PAYMENT_STATUS = AgainstStatus.Close;
            else
                AGAINST_PAYMENT_STATUS = AgainstStatus.Outstanding;
            ((CustomerInvoiceJournal)EVENT_JOURNAL).UpdateAgainstPaymentStatusSIJ();
        }
        public void UnSetOSAgainstPaymentItem(IPayment grni)
        {
            double qtyAmount = grni.GET_AMOUNT;
            if (qtyAmount > this.AMOUNT || OUTSTANDING_AMOUNT + qtyAmount > this.AMOUNT)
                throw new Exception("Payment Item revise Amount exceed SIJ Item Amount :" + this.INVOICE_NO);
            OUTSTANDING_AMOUNT = OUTSTANDING_AMOUNT + qtyAmount;
            PAID_AMOUNT = PAID_AMOUNT - qtyAmount;
            if (OUTSTANDING_AMOUNT > 0)
                AGAINST_PAYMENT_STATUS = AgainstStatus.Outstanding;
            ((CustomerInvoiceJournal)EVENT_JOURNAL).UpdateAgainstPaymentStatusSIJ();
        }
        private bool isValidToClose()
        {
            bool validA = OUTSTANDING_AMOUNT == 0;
            bool validB = PAID_AMOUNT == AMOUNT;
            return validA && validB;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_customerinvoicejournalitem 
                (   
                    cij_id,
                    cus_id,
                    ccy_id,
                    ciji_amount,
                    vbe_id,
                    vb_id,
                    ciji_entrytype,
                    ciji_invoicedate,
                    ciji_invoiceno,
                    ciji_duedate,
                    emp_id,
                    ciji_discount,
                    ciji_amountbeforediscount,
                    top_id,
                    ciji_description,
                    ciji_notes,
                    ciji_againstpaymentstatus,
                    ciji_outstandingamount,
                    ciji_paidamount 
                ) 
                VALUES ({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}','{9}',{10},{11},{12},{13},'{14}','{15}','{16}',{17},{18})",
               EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY == null ? 0 : VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
               VendorBalanceEntryType.CustomerInvoice.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP.ID,
               DESCRIPTION,
               NOTES,
               AGAINST_PAYMENT_STATUS.ToString(),
               AMOUNT,
               0
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_customerinvoicejournalitem set 
                    cij_id = {0},
                    cus_id = {1},
                    ccy_id = {2},
                    ciji_amount = {3},
                    vbe_id = {4},
                    vb_id = {5},
                    ciji_entrytype = '{6}',
                    ciji_invoicedate = '{7}',
                    ciji_invoiceno = '{8}',
                    ciji_duedate = '{9}',
                    emp_id = {10},
                    ciji_discount = {11},
                    ciji_amountbeforediscount = {12},
                    top_id = {13},
                    ciji_description = '{14}',
                    ciji_notes  = '{15}',
                    ciji_againstpaymentstatus = '{16}',
                    ciji_outstandingamount = {17},
                    ciji_paidamount  = {18}
                    where ciji_id = {16}",
                 EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY == null ? 0 : VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
               VendorBalanceEntryType.CustomerInvoice.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP.ID,
               DESCRIPTION,
               NOTES,
               AGAINST_PAYMENT_STATUS.ToString(),
               OUTSTANDING_AMOUNT,
               PAID_AMOUNT,
                ID);
        }
        public static CustomerInvoiceJournalItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            CustomerInvoiceJournalItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new CustomerInvoiceJournalItem();
                transaction.ID = Convert.ToInt32(aReader["ciji_id"]);
                transaction.EVENT_JOURNAL = new CustomerInvoiceJournal(Convert.ToInt32(aReader["cij_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["ciji_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerInvoice;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["ciji_invoicedate"]);
                transaction.INVOICE_NO = aReader["ciji_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["ciji_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["ciji_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["ciji_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["ciji_description"].ToString();
                transaction.NOTES = aReader["ciji_notes"].ToString();
                transaction.AGAINST_PAYMENT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["ciji_againstpaymentstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT = Convert.ToDouble(aReader["ciji_outstandingamount"]);
                transaction.PAID_AMOUNT = Convert.ToDouble(aReader["ciji_paidamount"]);

            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                CustomerInvoiceJournalItem transaction = new CustomerInvoiceJournalItem();
                transaction.ID = Convert.ToInt32(aReader["ciji_id"]);
                transaction.EVENT_JOURNAL = new CustomerInvoiceJournal(Convert.ToInt32(aReader["cij_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["ciji_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerInvoice;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["ciji_invoicedate"]);
                transaction.INVOICE_NO = aReader["ciji_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["ciji_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["ciji_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["ciji_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["ciji_description"].ToString();
                transaction.NOTES = aReader["ciji_notes"].ToString();
                transaction.AGAINST_PAYMENT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["ciji_againstpaymentstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT = Convert.ToDouble(aReader["ciji_outstandingamount"]);
                transaction.PAID_AMOUNT = Convert.ToDouble(aReader["ciji_paidamount"]);
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(ciji_id) from table_customerinvoicejournalitem");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_customerinvoicejournalitem where ciji_id = {0}", id);
        }

        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_customerinvoicejournalitem where cij_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (CustomerInvoiceJournalItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_customerinvoicejournalitem where cij_id = {0} and ciji_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_customerinvoicejournalitem where ciji_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_customerinvoicejournalitem where cij_id = {0}", id);
        }
        //public static string FindByGrnItemIDSQL(int id)
        //{
        //    return String.Format("SELECT * from table_customerinvoicejournalitem where grni_id = {0}", id);
        //}
        public string UpdateAgainstStatus()
        {
            return String.Format(@"Update table_customerinvoicejournalitem set 
                    ciji_againstpaymentstatus = '{0}',
                    ciji_outstandingamount = {1},
                    ciji_paidamount = {2}
                    where ciji_id = {3}", AGAINST_PAYMENT_STATUS.ToString(),
                                       OUTSTANDING_AMOUNT,
                                       PAID_AMOUNT,
                                       ID);
        }
        public static string GetSearchForPayment(string find,int ccyID, int customerID, string poi, DateTime trdate)
        {
            return String.Format(@"SELECT t.*
                FROM table_customerinvoicejournalitem t
                INNER JOIN table_customerinvoicejournal p on p.cij_id = t.cij_id
                where t.ciji_outstandingamount > 0
                and p.ccy_id = {4}
                and p.cij_code like '%{0}%' and p.cus_id = {1}  
                and p.cij_posted = true
                and p.cij_date <= '{2}'
               {3}", find, customerID, trdate.ToString(Utils.DATE_FORMAT), poi != "" ? " and t.ciji_id not in (" + poi + ")" : "", ccyID);
        }
        #region ICustomerInvoiceJournalItem Members

        public EventJournal GET_EVENT_JOURNAL
        {
            get
            {
                return EVENT_JOURNAL;
            }
            set
            {
                EVENT_JOURNAL = value;
            }
        }

        #endregion
        public static string GetByOutstandingSQL(int id)
        {
            return String.Format("SELECT ciji_outstandingamount from table_customerinvoicejournalitem where ciji_id = {0}", id);
        }
        public static string GetByPaidSQL(int id)
        {
            return String.Format("SELECT ciji_paidamount from table_customerinvoicejournalitem where ciji_id = {0}", id);
        }
    }
}