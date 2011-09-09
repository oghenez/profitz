using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class ReceiptItem : EventJournalItem, IReceipt
    {
        public ReceiptType PAYMENT_TYPE = ReceiptType.Cash;
        public VendorBalanceEntryType VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE = VendorBalanceEntryType.CustomerInvoice;
        public ICustomerInvoiceJournalItem CUSTOMER_INVOICE_JOURNAL_ITEM;
        public ARCreditNote AR_CREDIT_NOTE = null;
        public Bank BANK = new Bank();
        //public 

        public ReceiptItem() : base()
        {
            VENDOR_BALANCE_TYPE = VendorBalanceType.Customer;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_receiptitem 
                (   
                    rec_id,
                    cus_id,
                    ccy_id,
                    reci_amount,
                    vbe_id,
                    vb_id,
                    reci_entrytype,
                    reci_invoicedate,
                    reci_invoiceno,
                    reci_duedate,
                    emp_id,
                    reci_discount,
                    reci_amountbeforediscount,
                    top_id,
                    reci_description,
                    reci_notes,
                    inv_id,
                    inv_type,
                    bank_id,
                    reci_receipttype,
                    arcr_id
                ) 
                VALUES ({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}','{9}',{10},{11},{12},{13},
                '{14}','{15}',{16},'{17}',{18},'{19}',{20})",
               EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY==null?0:VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE==null?0:VENDOR_BALANCE.ID,
               VendorBalanceEntryType.Receipt.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP==null?0:TOP.ID,
               DESCRIPTION,
               NOTES,
               CUSTOMER_INVOICE_JOURNAL_ITEM.GetID(),
               VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE.ToString(),
               BANK==null?0:BANK.ID,
               PAYMENT_TYPE.ToString(),
                 AR_CREDIT_NOTE==null?0:AR_CREDIT_NOTE.ID
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_receiptitem set 
                    rec_id = {0},
                    cus_id = {1},
                    ccy_id = {2},
                    reci_amount = {3},
                    vbe_id = {4},
                    vb_id = {5},
                    reci_entrytype = '{6}',
                    reci_invoicedate = '{7}',
                    reci_invoiceno = '{8}',
                    reci_duedate = '{9}',
                    emp_id = {10},
                    reci_discount = {11},
                    reci_amountbeforediscount = {12},
                    top_id = {13},
                    reci_description = '{14}',
                    reci_notes  = '{15}',
                    inv_id = {16},
                    inv_type = '{17}',
                    bank_id = {18},
                    reci_receipttype = '{19}',
                    arcr_id = {20}
                    where reci_id = {21}",
                 EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY == null ? 0 : VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
               VendorBalanceEntryType.Receipt.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP == null ? 0 : TOP.ID,
               DESCRIPTION,
               NOTES,CUSTOMER_INVOICE_JOURNAL_ITEM.GetID(),
               VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE.ToString(),
               BANK==null?0:BANK.ID,
               PAYMENT_TYPE.ToString(), 
                AR_CREDIT_NOTE==null?0:AR_CREDIT_NOTE.ID,
                ID);
        }
        public static ReceiptItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            ReceiptItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new ReceiptItem();
                transaction.ID = Convert.ToInt32(aReader["reci_id"]);
                transaction.EVENT_JOURNAL = new Receipt(Convert.ToInt32(aReader["rec_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["reci_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
               //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Receipt;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["reci_invoicedate"]);
                transaction.INVOICE_NO = aReader["reci_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["reci_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["reci_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["reci_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["reci_description"].ToString();
                transaction.NOTES = aReader["reci_notes"].ToString();
                transaction.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE = (VendorBalanceEntryType)Enum.Parse(typeof(VendorBalanceEntryType), aReader["inv_type"].ToString());
                if (transaction.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE == VendorBalanceEntryType.CustomerInvoice)
                    transaction.CUSTOMER_INVOICE_JOURNAL_ITEM = new CustomerInvoiceJournalItem(Convert.ToInt32(aReader["inv_id"]));
                else
                    transaction.CUSTOMER_INVOICE_JOURNAL_ITEM = new CustomerOutStandingInvoiceItem(Convert.ToInt32(aReader["inv_id"]));
                transaction.BANK = new Bank(Convert.ToInt32(aReader["bank_id"]));
                transaction.PAYMENT_TYPE = (ReceiptType)Enum.Parse(typeof(ReceiptType), aReader["reci_receipttype"].ToString());
                transaction.AR_CREDIT_NOTE = new ARCreditNote(Convert.ToInt32(aReader["arcr_id"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                ReceiptItem transaction = new ReceiptItem();
                transaction.ID = Convert.ToInt32(aReader["reci_id"]);
                transaction.EVENT_JOURNAL = new Receipt(Convert.ToInt32(aReader["rec_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["reci_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Receipt;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["reci_invoicedate"]);
                transaction.INVOICE_NO = aReader["reci_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["reci_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["reci_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["reci_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["reci_description"].ToString();
                transaction.NOTES = aReader["reci_notes"].ToString();
                transaction.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE = (VendorBalanceEntryType)Enum.Parse(typeof(VendorBalanceEntryType), aReader["inv_type"].ToString());
                if (transaction.VENDOR_BALANCE_CUSTOMER_INVOICE_TYPE == VendorBalanceEntryType.CustomerInvoice)
                    transaction.CUSTOMER_INVOICE_JOURNAL_ITEM = new CustomerInvoiceJournalItem(Convert.ToInt32(aReader["inv_id"]));
                else
                    transaction.CUSTOMER_INVOICE_JOURNAL_ITEM = new CustomerOutStandingInvoiceItem(Convert.ToInt32(aReader["inv_id"]));
                transaction.BANK = new Bank(Convert.ToInt32(aReader["bank_id"]));
                transaction.PAYMENT_TYPE = (ReceiptType)Enum.Parse(typeof(ReceiptType), aReader["reci_receipttype"].ToString());
                transaction.AR_CREDIT_NOTE = new ARCreditNote(Convert.ToInt32(aReader["arcr_id"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(reci_id) from table_receiptitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_receiptitem where rec_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (ReceiptItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_receiptitem where rec_id = {0} and reci_id not in ({1})", id, pois);
        }
        public static string GetNotInTypeARCR(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (ReceiptItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Select * from table_receiptitem where rec_id = {0} and reci_receipttype ='{2}' and reci_id not in ({1})", id, pois, ReceiptType.ARCreditNote.ToString());
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_receiptitem where reci_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_receiptitem where rec_id = {0}", id);
        }
        public static string GetCustomerInvoiceBySOIID(int soiid, VendorBalanceEntryType invtype)
        {
            return String.Format("select * from table_receiptitem where inv_id = {0} and inv_type = '{1}'", soiid, invtype.ToString());
        }
        public static string GetReceiptItemByARCR(int apdnID)
        {
            return String.Format("select * from table_receiptitem where arcr_id = {0}", apdnID);
        }
        //public static string FindByGrnItemIDSQL(int id)
        //{
        //    return String.Format("SELECT * from table_receiptitem where grni_id = {0}", id);
        //}
//        public static string GetUpdateAPDNSQL(int payitmid, int apdnID)
//        {
//            return String.Format(@"update table_receiptitem set 
//                    arcr_id = {0}
//                    where reci_id = {1}",
//                                        apdnID,payitmid);
//        }
        #region IReceipt Members

        public double GET_AMOUNT
        {
            get
            {
                return AMOUNT;
            }
        }

        #endregion
    }
}
