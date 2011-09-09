using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class ARCreditNoteItem : EventJournalItem
    {
        public SalesReturn SALES_RETURN = null;
        public ARCreditNoteItem() : base()
        {
            VENDOR_BALANCE_TYPE = VendorBalanceType.Customer;
        }
        public ARCreditNoteItem(int id)
            : this()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_arcreditnoteitem 
                (   
                    arcr_id,
                    cus_id,
                    ccy_id,
                    arcri_amount,
                    vbe_id,
                    vb_id,
                    arcri_entrytype,
                    arcri_invoicedate,
                    arcri_invoiceno,
                    arcri_duedate,
                    emp_id,
                    arcri_discount,
                    arcri_amountbeforediscount,
                    top_id,
                    arcri_description,
                    arcri_notes,
                    srn_id
                ) 
                VALUES ({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}','{9}',{10},{11},{12},{13},'{14}','{15}',{16})",
               EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY==null?0:VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE==null?0:VENDOR_BALANCE.ID,
               VendorBalanceEntryType.ARCreditNote.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP==null?0:TOP.ID,
               DESCRIPTION,
               NOTES,
               SALES_RETURN==null?0:SALES_RETURN.ID
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_arcreditnoteitem set 
                    arcr_id = {0},
                    cus_id = {1},
                    ccy_id = {2},
                    arcri_amount = {3},
                    vbe_id = {4},
                    vb_id = {5},
                    arcri_entrytype = '{6}',
                    arcri_invoicedate = '{7}',
                    arcri_invoiceno = '{8}',
                    arcri_duedate = '{9}',
                    emp_id = {10},
                    arcri_discount = {11},
                    arcri_amountbeforediscount = {12},
                    top_id = {13},
                    arcri_description = '{14}',
                    arcri_notes  = '{15}',
                    srn_id = {16}
                    where arcri_id = {17}",
                 EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY == null ? 0 : VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
               VendorBalanceEntryType.ARCreditNote.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP == null ? 0 : TOP.ID,
               DESCRIPTION,
               NOTES,
               SALES_RETURN==null?0:SALES_RETURN.ID,
                ID);
        }
        public static ARCreditNoteItem TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            ARCreditNoteItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new ARCreditNoteItem();
                transaction.ID = Convert.ToInt32(aReader["arcri_id"]);
                transaction.EVENT_JOURNAL = new ARCreditNote(Convert.ToInt32(aReader["arcr_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["arcri_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
               //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.ARCreditNote;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["arcri_invoicedate"]);
                transaction.INVOICE_NO = aReader["arcri_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["arcri_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["arcri_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["arcri_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["arcri_description"].ToString();
                transaction.NOTES = aReader["arcri_notes"].ToString();
                transaction.SALES_RETURN = new  SalesReturn(Convert.ToInt32(aReader["srn_id"]));
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                ARCreditNoteItem transaction = new ARCreditNoteItem();
                transaction.ID = Convert.ToInt32(aReader["arcri_id"]);
                transaction.EVENT_JOURNAL = new ARCreditNote(Convert.ToInt32(aReader["arcr_id"]));
                transaction.VENDOR = new Customer(Convert.ToInt32(aReader["cus_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["arcri_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.ARCreditNote;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["arcri_invoicedate"]);
                transaction.INVOICE_NO = aReader["arcri_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["arcri_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["arcri_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["arcri_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["arcri_description"].ToString();
                transaction.NOTES = aReader["arcri_notes"].ToString();
                transaction.SALES_RETURN = new SalesReturn(Convert.ToInt32(aReader["srn_id"]));
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(arcri_id) from table_arcreditnoteitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_arcreditnoteitem where arcr_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (ARCreditNoteItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_arcreditnoteitem where arcr_id = {0} and arcri_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_arcreditnoteitem where arcri_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_arcreditnoteitem where arcr_id = {0}", id);
        }
        //public static string FindByGrnItemIDSQL(int id)
        //{
        //    return String.Format("SELECT * from table_arcreditnoteitem where grni_id = {0}", id);
        //}
        internal static string GetSRUsedByARCR()
        {
            return "select distinct(t.srn_id) FROM table_arcreditnoteitem t where t.srn_id != 0";
        }
        internal static string GetSRUsedByARCR(int purchaseReturn)
        {
            return string.Format("Select Count(*) from table_arcreditnoteitem where srn_id = ({0})", purchaseReturn);
        }
        public static string GetARCRItemBySRID(int prnID)
        {
            return String.Format("select * from table_arcreditnoteitem where srn_id = {0}", prnID);
        }
    }
}
