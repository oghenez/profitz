using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class CustomerOutStandingInvoiceItem : EventJournalItem, ICustomerInvoiceJournalItem
    {
        public AgainstStatus AGAINST_RECEIPT_STATUS = AgainstStatus.Open;
        public double OUTSTANDING_AMOUNT = 0;
        public double PAID_AMOUNT = 0;

        public CustomerOutStandingInvoiceItem() : base()
        {
            VENDOR_BALANCE_TYPE = VendorBalanceType.Customer;
        }
        public CustomerOutStandingInvoiceItem(int id)
            : this()
        {
            ID = id;
        }
        public void SetOSAgainstReceiptItem(IReceipt pyi)
        {
            double qtyAmount = pyi.GET_AMOUNT;
            if (qtyAmount <= 0) return;
            if (AGAINST_RECEIPT_STATUS == AgainstStatus.Close)
                throw new Exception("Invoice Item Allready Close :" + this.INVOICE_NO);
            if (qtyAmount > OUTSTANDING_AMOUNT)
                throw new Exception("Receipt Item Amount exceed SIJ Outstanding Item Amount :" + this.INVOICE_NO);
            OUTSTANDING_AMOUNT = OUTSTANDING_AMOUNT - qtyAmount;
            PAID_AMOUNT = PAID_AMOUNT + qtyAmount;
            if (isValidToClose())
                AGAINST_RECEIPT_STATUS = AgainstStatus.Close;
            else
                AGAINST_RECEIPT_STATUS = AgainstStatus.Outstanding;
            ((CustomerOutStandingInvoice)EVENT_JOURNAL).UpdateAgainstReceiptStatusSIJ();
        }
        public void UnSetOSAgainstReceiptItem(IReceipt grni)
        {
            double qtyAmount = grni.GET_AMOUNT;
            if (qtyAmount > this.AMOUNT || OUTSTANDING_AMOUNT + qtyAmount > this.AMOUNT)
                throw new Exception("Receipt Item revise Amount exceed SIJ Item Amount :" + this.INVOICE_NO);
            OUTSTANDING_AMOUNT = OUTSTANDING_AMOUNT + qtyAmount;
            PAID_AMOUNT = PAID_AMOUNT - qtyAmount;
            if (OUTSTANDING_AMOUNT > 0)
                AGAINST_RECEIPT_STATUS = AgainstStatus.Outstanding;
            ((CustomerOutStandingInvoice)EVENT_JOURNAL).UpdateAgainstReceiptStatusSIJ();
        }
        private bool isValidToClose()
        {
            bool validA = OUTSTANDING_AMOUNT == 0;
            bool validB = PAID_AMOUNT == AMOUNT;
            return validA && validB;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_customeroutstandinginvoiceitem 
                (   
                    costi_id,
                    cus_id,
                    ccy_id,
                    costii_amount,
                    vbe_id,
                    vb_id,
                    costii_entrytype,
                    costii_invoicedate,
                    costii_invoiceno,
                    costii_duedate,
                    emp_id,
                    costii_discount,
                    costii_amountbeforediscount,
                    top_id,
                    costii_description,
                    costii_notes,
                    costii_againstpaymentstatus,
                    costii_outstandingamount,
                    costii_receiptamount  
                ) 
                VALUES ({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}','{9}',{10},{11},{12},{13},'{14}','{15}'
                        ,'{16}',{17},{18})",
               EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY==null?0:VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE==null?0:VENDOR_BALANCE.ID,
               VendorBalanceEntryType.CustomerOutStandingInvoice.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP.ID,
               DESCRIPTION,
               NOTES,
                AGAINST_RECEIPT_STATUS.ToString(),
               AMOUNT,
               0
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_customeroutstandinginvoiceitem set 
                    costi_id = {0},
                    cus_id = {1},
                    ccy_id = {2},
                    costii_amount = {3},
                    vbe_id = {4},
                    vb_id = {5},
                    costii_entrytype = '{6}',
                    costii_invoicedate = '{7}',
                    costii_invoiceno = '{8}',
                    costii_duedate = '{9}',
                    emp_id = {10},
                    costii_discount = {11},
                    costii_amountbeforediscount = {12},
                    top_id = {13},
                    costii_description = '{14}',
                    costii_notes  = '{15}',
                    costii_againstpaymentstatus = '{16}',
                    costii_outstandingamount = {17},
                    costii_receiptamount  = {18}
                    where costii_id = {19}",
                 EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY == null ? 0 : VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
               VendorBalanceEntryType.CustomerOutStandingInvoice.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP.ID,
               DESCRIPTION,
               NOTES,
                AGAINST_RECEIPT_STATUS.ToString(),
               //OUTSTANDING_AMOUNT,
               AMOUNT,
               0,
                ID);
        }
        public static CustomerOutStandingInvoiceItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            CustomerOutStandingInvoiceItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new CustomerOutStandingInvoiceItem();
                transaction.ID = Convert.ToInt32(aReader["costii_id"]);
                transaction.EVENT_JOURNAL = new CustomerOutStandingInvoice(Convert.ToInt32(aReader["costi_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["costii_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
               //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerOutStandingInvoice;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["costii_invoicedate"]);
                transaction.INVOICE_NO = aReader["costii_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["costii_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["costii_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["costii_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["costii_description"].ToString();
                transaction.NOTES = aReader["costii_notes"].ToString();
                transaction.AGAINST_RECEIPT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["costii_againstpaymentstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT = Convert.ToDouble(aReader["costii_outstandingamount"]);
                transaction.PAID_AMOUNT = Convert.ToDouble(aReader["costii_receiptamount"]);
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                CustomerOutStandingInvoiceItem transaction = new CustomerOutStandingInvoiceItem();
                transaction.ID = Convert.ToInt32(aReader["costii_id"]);
                transaction.EVENT_JOURNAL = new CustomerOutStandingInvoice(Convert.ToInt32(aReader["costi_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["costii_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.CustomerOutStandingInvoice;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["costii_invoicedate"]);
                transaction.INVOICE_NO = aReader["costii_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["costii_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["costii_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["costii_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["costii_description"].ToString();
                transaction.NOTES = aReader["costii_notes"].ToString();
                transaction.AGAINST_RECEIPT_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["costii_againstpaymentstatus"].ToString());
                transaction.OUTSTANDING_AMOUNT = Convert.ToDouble(aReader["costii_outstandingamount"]);
                transaction.PAID_AMOUNT = Convert.ToDouble(aReader["costii_receiptamount"]);
                result.Add(transaction);
            }
            return result;
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_customeroutstandinginvoiceitem where costii_id = {0}", id);
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(costii_id) from table_customeroutstandinginvoiceitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_customeroutstandinginvoiceitem where costi_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (CustomerOutStandingInvoiceItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_customeroutstandinginvoiceitem where costi_id = {0} and costii_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_customeroutstandinginvoiceitem where costii_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_customeroutstandinginvoiceitem where costi_id = {0}", id);
        }
        //public static string FindByGrnItemIDSQL(int id)
        //{
        //    return String.Format("SELECT * from table_customeroutstandinginvoiceitem where grni_id = {0}", id);
        //}
        public static string GetSearchForReceipt(string find, int ccyID, int supplierID, string poi, DateTime trdate)
        {
            return String.Format(@"SELECT t.*
                FROM table_customeroutstandinginvoiceitem t
                INNER JOIN table_customeroutstandinginvoice p on p.costi_id = t.costi_id
                where t.costii_outstandingamount > 0
                and p.ccy_id = {4}
                and p.costi_code like '%{0}%' and p.cus_id = {1}  
                and p.costi_posted = true
                and p.costi_date <= '{2}'
               {3}", find, supplierID, trdate.ToString(Utils.DATE_FORMAT), poi != "" ? " and t.costii_id not in (" + poi + ")" : "",
                   ccyID);
        }

        public static string GetByOutstandingSQL(int id)
        {
            return String.Format("SELECT costii_outstandingamount from table_customeroutstandinginvoiceitem where costii_id = {0}", id);
        }
        public static string GetByReceiptSQL(int id)
        {
            return String.Format("SELECT costii_receiptamount from table_customeroutstandinginvoiceitem where costii_id = {0}", id);
        }

        public string UpdateAgainstStatus()
        {
            return String.Format(@"Update table_customeroutstandinginvoiceitem set 
                    costii_againstpaymentstatus = '{0}',
                    costii_outstandingamount = {1},
                    costii_receiptamount = {2}
                    where costii_id = {3}", AGAINST_RECEIPT_STATUS.ToString(),
                                       OUTSTANDING_AMOUNT,
                                       PAID_AMOUNT,
                                       ID);
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
    }
}
