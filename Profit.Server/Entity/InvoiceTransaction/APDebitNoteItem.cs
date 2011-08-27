using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class APDebitNoteItem : EventJournalItem
    {
        public PurchaseReturn PURCHASE_RETURN = null;
        public APDebitNoteItem() : base()
        {
            VENDOR_BALANCE_TYPE = VendorBalanceType.Supplier;
        }
        public APDebitNoteItem(int id)
            : this()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_apdebitnoteitem 
                (   
                    apdn_id,
                    sup_id,
                    ccy_id,
                    apdni_amount,
                    vbe_id,
                    vb_id,
                    apdni_entrytype,
                    apdni_invoicedate,
                    apdni_invoiceno,
                    apdni_duedate,
                    emp_id,
                    apdni_discount,
                    apdni_amountbeforediscount,
                    top_id,
                    apdni_description,
                    apdni_notes,
                    prn_id
                ) 
                VALUES ({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}','{9}',{10},{11},{12},{13},'{14}','{15}',{16})",
               EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY==null?0:VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE==null?0:VENDOR_BALANCE.ID,
               VendorBalanceEntryType.APDebitNote.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP==null?0:TOP.ID,
               DESCRIPTION,
               NOTES,
               PURCHASE_RETURN==null?0:PURCHASE_RETURN.ID
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_apdebitnoteitem set 
                    apdn_id = {0},
                    sup_id = {1},
                    ccy_id = {2},
                    apdni_amount = {3},
                    vbe_id = {4},
                    vb_id = {5},
                    apdni_entrytype = '{6}',
                    apdni_invoicedate = '{7}',
                    apdni_invoiceno = '{8}',
                    apdni_duedate = '{9}',
                    emp_id = {10},
                    apdni_discount = {11},
                    apdni_amountbeforediscount = {12},
                    top_id = {13},
                    apdni_description = '{14}',
                    apdni_notes  = '{15}',
                    prn_id = {16}
                    where apdni_id = {17}",
                 EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY == null ? 0 : VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
               VendorBalanceEntryType.APDebitNote.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP == null ? 0 : TOP.ID,
               DESCRIPTION,
               NOTES,
               PURCHASE_RETURN==null?0:PURCHASE_RETURN.ID,
                ID);
        }
        public static APDebitNoteItem TransformReader(OdbcDataReader aReader)
        {
            APDebitNoteItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new APDebitNoteItem();
                transaction.ID = Convert.ToInt32(aReader["apdni_id"]);
                transaction.EVENT_JOURNAL = new APDebitNote(Convert.ToInt32(aReader["apdn_id"]));
                transaction.VENDOR = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["apdni_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
               //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.APDebitNote;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["apdni_invoicedate"]);
                transaction.INVOICE_NO = aReader["apdni_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["apdni_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["apdni_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["apdni_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["apdni_description"].ToString();
                transaction.NOTES = aReader["apdni_notes"].ToString();
                transaction.PURCHASE_RETURN = new  PurchaseReturn(Convert.ToInt32(aReader["prn_id"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                APDebitNoteItem transaction = new APDebitNoteItem();
                transaction.ID = Convert.ToInt32(aReader["apdni_id"]);
                transaction.EVENT_JOURNAL = new APDebitNote(Convert.ToInt32(aReader["apdn_id"]));
                transaction.VENDOR = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["apdni_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.APDebitNote;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["apdni_invoicedate"]);
                transaction.INVOICE_NO = aReader["apdni_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["apdni_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["apdni_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["apdni_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["apdni_description"].ToString();
                transaction.NOTES = aReader["apdni_notes"].ToString();
                transaction.PURCHASE_RETURN = new PurchaseReturn(Convert.ToInt32(aReader["prn_id"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(apdni_id) from table_apdebitnoteitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_apdebitnoteitem where apdn_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (APDebitNoteItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_apdebitnoteitem where apdn_id = {0} and apdni_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_apdebitnoteitem where apdni_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_apdebitnoteitem where apdn_id = {0}", id);
        }
        //public static string FindByGrnItemIDSQL(int id)
        //{
        //    return String.Format("SELECT * from table_apdebitnoteitem where grni_id = {0}", id);
        //}
        internal static string GetPRUsedByAPDN()
        {
            return "select distinct(t.prn_id) FROM table_apdebitnoteitem t where t.prn_id != 0";
        }
        internal static string GetPRUsedByAPDN(int purchaseReturn)
        {
            return string.Format("Select Count(*) from table_apdebitnoteitem where prn_id = ({0})", purchaseReturn);
        }
    }
}
