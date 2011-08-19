using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SupplierOutStandingInvoiceItem : EventJournalItem
    {
        public SupplierOutStandingInvoiceItem() : base()
        {
            VENDOR_BALANCE_TYPE = VendorBalanceType.Supplier;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_supplieroutstandinginvoiceitem 
                (   
                    sosti_id,
                    sup_id,
                    ccy_id,
                    sostii_amount,
                    vbe_id,
                    vb_id,
                    sostii_entrytype,
                    sostii_invoicedate,
                    sostii_invoiceno,
                    sostii_duedate,
                    emp_id,
                    sostii_discount,
                    sostii_amountbeforediscount,
                    top_id,
                    sostii_description,
                    sostii_notes 
                ) 
                VALUES ({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}','{9}',{10},{11},{12},{13},'{14}','{15}')",
               EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY==null?0:VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE==null?0:VENDOR_BALANCE.ID,
               VendorBalanceEntryType.SupplierOutStandingInvoice.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP.ID,
               DESCRIPTION,
               NOTES
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_supplieroutstandinginvoiceitem set 
                    sosti_id = {0},
                    sup_id = {1},
                    ccy_id = {2},
                    sostii_amount = {3},
                    vbe_id = {4},
                    vb_id = {5},
                    sostii_entrytype = '{6}',
                    sostii_invoicedate = '{7}',
                    sostii_invoiceno = '{8}',
                    sostii_duedate = '{9}',
                    emp_id = {10},
                    sostii_discount = {11},
                    sostii_amountbeforediscount = {12},
                    top_id = {13},
                    sostii_description = '{14}',
                    sostii_notes  = '{15}'
                    where sostii_id = {16}",
                 EVENT_JOURNAL.ID,
               VENDOR.ID,
               CURRENCY.ID,
               AMOUNT,
               VENDOR_BALANCE_ENTRY == null ? 0 : VENDOR_BALANCE_ENTRY.ID,
               VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
               VendorBalanceEntryType.SupplierOutStandingInvoice.ToString(),
               INVOICE_DATE.ToString(Utils.DATE_FORMAT),
               INVOICE_NO,
               DUE_DATE.ToString(Utils.DATE_FORMAT),
               EMPLOYEE.ID,
               DISCOUNT,
               AMOUNT_BEFORE_DISCOUNT,
               TOP.ID,
               DESCRIPTION,
               NOTES,
                ID);
        }
        public static SupplierOutStandingInvoiceItem TransformReader(OdbcDataReader aReader)
        {
            SupplierOutStandingInvoiceItem transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new SupplierOutStandingInvoiceItem();
                transaction.ID = Convert.ToInt32(aReader["sostii_id"]);
                transaction.EVENT_JOURNAL = new SupplierOutStandingInvoice(Convert.ToInt32(aReader["sosti_id"]));
                transaction.VENDOR = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["sostii_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
               //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierOutStandingInvoice;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["sostii_invoicedate"]);
                transaction.INVOICE_NO = aReader["sostii_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["sostii_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["sostii_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["sostii_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["sostii_description"].ToString();
                transaction.NOTES = aReader["sostii_notes"].ToString();

            }
            return transaction;
        }
        public static IList TransformReaderList(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                SupplierOutStandingInvoiceItem transaction = new SupplierOutStandingInvoiceItem();
                transaction.ID = Convert.ToInt32(aReader["sostii_id"]);
                transaction.EVENT_JOURNAL = new SupplierOutStandingInvoice(Convert.ToInt32(aReader["sosti_id"]));
                transaction.VENDOR = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.AMOUNT = Convert.ToDouble(aReader["sostii_amount"]);
                //transaction.VENDOR_BALANCE_ENTRY = new VendorBalanceEntry(
                //VENDOR_BALANCE == null ? 0 : VENDOR_BALANCE.ID,
                transaction.VENDOR_BALANCE_ENTRY_TYPE = VendorBalanceEntryType.SupplierOutStandingInvoice;
                transaction.INVOICE_DATE = Convert.ToDateTime(aReader["sostii_invoicedate"]);
                transaction.INVOICE_NO = aReader["sostii_invoiceno"].ToString();
                transaction.DUE_DATE = Convert.ToDateTime(aReader["sostii_duedate"]);
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.DISCOUNT = Convert.ToDouble(aReader["sostii_discount"]);
                transaction.AMOUNT_BEFORE_DISCOUNT = Convert.ToDouble(aReader["sostii_amountbeforediscount"]);
                transaction.TOP = new TermOfPayment(Convert.ToInt32(aReader["top_id"]));
                transaction.DESCRIPTION = aReader["sostii_description"].ToString();
                transaction.NOTES = aReader["sostii_notes"].ToString();
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(sostii_id) from table_supplieroutstandinginvoiceitem");
        }
        public static string GetByEventIDSQL(int id)
        {
            return String.Format("SELECT * from table_supplieroutstandinginvoiceitem where sosti_id = {0}", id);
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (SupplierOutStandingInvoiceItem i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_supplieroutstandinginvoiceitem where sosti_id = {0} and sostii_id not in ({1})", id, pois);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_supplieroutstandinginvoiceitem where sostii_id = {0}", id);
        }
        public static string DeleteAllByEventSQL(int id)
        {
            return String.Format("Delete from table_supplieroutstandinginvoiceitem where sosti_id = {0}", id);
        }
        //public static string FindByGrnItemIDSQL(int id)
        //{
        //    return String.Format("SELECT * from table_supplieroutstandinginvoiceitem where grni_id = {0}", id);
        //}
    }
}
