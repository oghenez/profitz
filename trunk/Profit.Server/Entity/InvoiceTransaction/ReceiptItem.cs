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
        public VendorBalanceEntryType VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE = VendorBalanceEntryType.CustomerInvoice;
        public ICustomerInvoiceJournalItem SUPPLIER_INVOICE_JOURNAL_ITEM;
        public APDebitNote AP_DEBIT_NOTE = null;
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
                    pay_id,
                    cus_id,
                    ccy_id,
                    payi_amount,
                    vbe_id,
                    vb_id,
                    payi_entrytype,
                    payi_invoicedate,
                    payi_invoiceno,
                    payi_duedate,
                    emp_id,
                    payi_discount,
                    payi_amountbeforediscount,
                    top_id,
                    payi_description,
                    payi_notes,
                    inv_id,
                    inv_type,
                    bank_id,
                    payi_paymenttype,
                    apdn_id
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
               SUPPLIER_INVOICE_JOURNAL_ITEM.GetID(),
               VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE.ToString(),
               BANK==null?0:BANK.ID,
               PAYMENT_TYPE.ToString(),
                 AP_DEBIT_NOTE==null?0:AP_DEBIT_NOTE.ID
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_receiptitem set 
                    pay_id = {0},
                    cus_id = {1},
                    ccy_id = {2},
                    payi_amount = {3},
                    vbe_id = {4},
                    vb_id = {5},
                    payi_entrytype = '{6}',
                    payi_invoicedate = '{7}',
                    payi_invoiceno = '{8}',
                    payi_duedate = '{9}',
                    emp_id = {10},
                    payi_discount = {11},
                    payi_amountbeforediscount = {12},
                    top_id = {13},
                    payi_description = '{14}',
                    payi_notes  = '{15}',
                    inv_id = {16},
                    inv_type = '{17}',
                    bank_id = {18},
                    payi_paymenttype = '{19}',
                    apdn_id = {20}
                    where payi_id = {21}",
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
               NOTES,SUPPLIER_INVOICE_JOURNAL_ITEM.GetID(),
               VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE.ToString(),
               BANK==null?0:BANK.ID,
               PAYMENT_TYPE.ToString(), 
                AP_DEBIT_NOTE==null?0:AP_DEBIT_NOTE.ID,
                ID);
        }
        public static ReceiptItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            ReceiptItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new ReceiptItem();
                transaction.ID = Convert.ToInt32(aReader["payi_id"]);
                transaction.EVENT_JOURNAL = new Receipt(Convert.ToInt32(aReader["pay_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["payi_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
               //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Receipt;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["payi_invoicedate"]);
                transaction.INVOICE_NO = aReader["payi_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["payi_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["payi_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["payi_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["payi_description"].ToString();
                transaction.NOTES = aReader["payi_notes"].ToString();
                transaction.VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE = (VendorBalanceEntryType)Enum.Parse(typeof(VendorBalanceEntryType), aReader["inv_type"].ToString());
                if (transaction.VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE == VendorBalanceEntryType.CustomerInvoice)
                    transaction.SUPPLIER_INVOICE_JOURNAL_ITEM = new CustomerInvoiceJournalItem(Convert.ToInt32(aReader["inv_id"]));
                else
                    transaction.SUPPLIER_INVOICE_JOURNAL_ITEM = new CustomerOutStandingInvoiceItem(Convert.ToInt32(aReader["inv_id"]));
                transaction.BANK = new Bank(Convert.ToInt32(aReader["bank_id"]));
                transaction.PAYMENT_TYPE = (ReceiptType)Enum.Parse(typeof(ReceiptType), aReader["payi_paymenttype"].ToString());
                transaction.AP_DEBIT_NOTE = new APDebitNote(Convert.ToInt32(aReader["apdn_id"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                ReceiptItem transaction = new ReceiptItem();
                transaction.ID = Convert.ToInt32(aReader["payi_id"]);
                transaction.EVENT_JOURNAL = new Receipt(Convert.ToInt32(aReader["pay_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["payi_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.Receipt;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["payi_invoicedate"]);
                transaction.INVOICE_NO = aReader["payi_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["payi_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["payi_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["payi_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["payi_description"].ToString();
                transaction.NOTES = aReader["payi_notes"].ToString();
                transaction.VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE = (VendorBalanceEntryType)Enum.Parse(typeof(VendorBalanceEntryType), aReader["inv_type"].ToString());
                if (transaction.VENDOR_BALANCE_SUPPLIER_INVOICE_TYPE == VendorBalanceEntryType.CustomerInvoice)
                    transaction.SUPPLIER_INVOICE_JOURNAL_ITEM = new CustomerInvoiceJournalItem(Convert.ToInt32(aReader["inv_id"]));
                else
                    transaction.SUPPLIER_INVOICE_JOURNAL_ITEM = new CustomerOutStandingInvoiceItem(Convert.ToInt32(aReader["inv_id"]));
                transaction.BANK = new Bank(Convert.ToInt32(aReader["bank_id"]));
                transaction.PAYMENT_TYPE = (ReceiptType)Enum.Parse(typeof(ReceiptType), aReader["payi_paymenttype"].ToString());
                transaction.AP_DEBIT_NOTE = new APDebitNote(Convert.ToInt32(aReader["apdn_id"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(payi_id) from table_receiptitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_receiptitem where pay_id = {0}", id);
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
            return String.Format("Delete from table_receiptitem where pay_id = {0} and payi_id not in ({1})", id, pois);
        }
        public static string GetNotInTypeAPDN(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (ReceiptItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Select * from table_receiptitem where pay_id = {0} and payi_paymenttype ='{2}' and payi_id not in ({1})", id, pois, ReceiptType.APDebitNote.ToString());
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_receiptitem where payi_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_receiptitem where pay_id = {0}", id);
        }
        public static string GetCustomerInvoiceBySOIID(int soiid, VendorBalanceEntryType invtype)
        {
            return String.Format("select * from table_receiptitem where inv_id = {0} and inv_type = '{1}'", soiid, invtype.ToString());
        }
        public static string GetReceiptItemByAPDN(int apdnID)
        {
            return String.Format("select * from table_receiptitem where apdn_id = {0}", apdnID);
        }
        //public static string FindByGrnItemIDSQL(int id)
        //{
        //    return String.Format("SELECT * from table_receiptitem where grni_id = {0}", id);
        //}
//        public static string GetUpdateAPDNSQL(int payitmid, int apdnID)
//        {
//            return String.Format(@"update table_receiptitem set 
//                    apdn_id = {0}
//                    where payi_id = {1}",
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
