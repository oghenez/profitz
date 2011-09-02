using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class SupplierInvoiceJournalItem : EventJournalItem, ISupplierInvoiceJournalItem
    {
        public AgainstStatus AGAINST_PAYMENT_STATUS = AgainstStatus.Open;
        public double OUTSTANDING_AMOUNT = 0;
        public double PAID_AMOUNT = 0;

        public SupplierInvoiceJournalItem()
            : base()
        {
            VENDOR_BALANCE_TYPE = VendorBalanceType.Supplier;
        }
        public SupplierInvoiceJournalItem(int id)
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
            ((SupplierInvoiceJournal)EVENT_JOURNAL).UpdateAgainstPaymentStatusSIJ();
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
            ((SupplierInvoiceJournal)EVENT_JOURNAL).UpdateAgainstPaymentStatusSIJ();
        }
        private bool isValidToClose()
        {
            bool validA = OUTSTANDING_AMOUNT == 0;
            bool validB = PAID_AMOUNT == AMOUNT;
            return validA && validB;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_supplierinvoicejournalitem 
                (   
                    sij_id,
                    sup_id,
                    ccy_id,
                    siji_amount,
                    vbe_id,
                    vb_id,
                    siji_entrytype,
                    siji_invoicedate,
                    siji_invoiceno,
                    siji_duedate,
                    emp_id,
                    siji_discount,
                    siji_amountbeforediscount,
                    top_id,
                    siji_description,
                    siji_notes,
                    siji_againstpaymentstatus,
                    siji_outstandingamount,
                    siji_paidamount 
                ) 
                VALUES ({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}','{9}',{10},{11},{12},{13},'{14}','{15}','{16}',{17},{18})",
               EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY == null ? 0 : VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
               VendorBalanceEntryType.SupplierInvoice.ToString(),
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
            return String.Format(@"update table_supplierinvoicejournalitem set 
                    sij_id = {0},
                    sup_id = {1},
                    ccy_id = {2},
                    siji_amount = {3},
                    vbe_id = {4},
                    vb_id = {5},
                    siji_entrytype = '{6}',
                    siji_invoicedate = '{7}',
                    siji_invoiceno = '{8}',
                    siji_duedate = '{9}',
                    emp_id = {10},
                    siji_discount = {11},
                    siji_amountbeforediscount = {12},
                    top_id = {13},
                    siji_description = '{14}',
                    siji_notes  = '{15}',
                    siji_againstpaymentstatus = '{16}',
                    siji_outstandingamount = {17},
                    siji_paidamount  = {18}
                    where siji_id = {16}",
                 EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY == null ? 0 : VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
               VendorBalanceEntryType.SupplierInvoice.ToString(),
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
        public static SupplierInvoiceJournalItem TransformReader(OdbcDataReader aReader)
        {
            SupplierInvoiceJournalItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new SupplierInvoiceJournalItem();
                transaction.ID = Convert.ToInt32(aReader["siji_id"]);
                transaction.EVENT_JOURNAL = new SupplierInvoiceJournal(Convert.ToInt32(aReader["sij_id"]));
                transaction.VENDOR = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["siji_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierInvoice;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["siji_invoicedate"]);
                transaction.INVOICE_NO = aReader["siji_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["siji_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["siji_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["siji_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["siji_description"].ToString();
                transaction.NOTES = aReader["siji_notes"].ToString();
                transaction.AGAINST_PAYMENT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["siji_againstpaymentstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT = Convert.ToDouble(aReader["siji_outstandingamount"]);
                transaction.PAID_AMOUNT = Convert.ToDouble(aReader["siji_paidamount"]);

            }
            return transaction;
        }
        public static IList TransformReaderList(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                SupplierInvoiceJournalItem transaction = new SupplierInvoiceJournalItem();
                transaction.ID = Convert.ToInt32(aReader["siji_id"]);
                transaction.EVENT_JOURNAL = new SupplierInvoiceJournal(Convert.ToInt32(aReader["sij_id"]));
                transaction.VENDOR = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["siji_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierOutStandingInvoice;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["siji_invoicedate"]);
                transaction.INVOICE_NO = aReader["siji_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["siji_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["siji_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["siji_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["siji_description"].ToString();
                transaction.NOTES = aReader["siji_notes"].ToString();
                transaction.AGAINST_PAYMENT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["siji_againstpaymentstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT = Convert.ToDouble(aReader["siji_outstandingamount"]);
                transaction.PAID_AMOUNT = Convert.ToDouble(aReader["siji_paidamount"]);
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(siji_id) from table_supplierinvoicejournalitem");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_supplierinvoicejournalitem where siji_id = {0}", id);
        }

        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_supplierinvoicejournalitem where sij_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (SupplierInvoiceJournalItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_supplierinvoicejournalitem where sij_id = {0} and siji_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_supplierinvoicejournalitem where siji_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_supplierinvoicejournalitem where sij_id = {0}", id);
        }
        //public static string FindByGrnItemIDSQL(int id)
        //{
        //    return String.Format("SELECT * from table_supplierinvoicejournalitem where grni_id = {0}", id);
        //}
        public string UpdateAgainstStatus()
        {
            return String.Format(@"Update table_supplierinvoicejournalitem set 
                    siji_againstpaymentstatus = '{0}',
                    siji_outstandingamount = {1},
                    siji_paidamount = {2}
                    where siji_id = {3}", AGAINST_PAYMENT_STATUS.ToString(),
                                       OUTSTANDING_AMOUNT,
                                       PAID_AMOUNT,
                                       ID);
        }
        public static string GetSearchForPayment(string find,int ccyID, int supplierID, string poi, DateTime trdate)
        {
            return String.Format(@"SELECT t.*
                FROM table_supplierinvoicejournalitem t
                INNER JOIN table_supplierinvoicejournal p on p.sij_id = t.sij_id
                where t.siji_outstandingamount > 0
                and p.ccy_id = {4}
                and p.sij_code like '%{0}%' and p.sup_id = {1}  
                and p.sij_posted = true
                and p.sij_date <= '{2}'
               {3}", find, supplierID, trdate.ToString(Utils.DATE_FORMAT), poi != "" ? " and t.siji_id not in (" + poi + ")" : "", ccyID);
        }
        #region ISupplierInvoiceJournalItem Members

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
            return String.Format("SELECT siji_outstandingamount from table_supplierinvoicejournalitem where siji_id = {0}", id);
        }
        public static string GetByPaidSQL(int id)
        {
            return String.Format("SELECT siji_paidamount from table_supplierinvoicejournalitem where siji_id = {0}", id);
        }
    }
}