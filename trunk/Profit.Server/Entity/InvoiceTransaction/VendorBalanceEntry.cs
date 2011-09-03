using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class VendorBalanceEntry
    {
        public int ID = 0;
        public VendorBalance VENDOR_BALANCE;
        public VendorBalanceEntryType VENDOR_BALANCE_ENTRY_TYPE;
        public DateTime TRANSACTION_DATE;
        public Currency CURRENCY;
        public double AMOUNT;
        public EventJournalItem EVENT_JOURNAL_ITEM;
        public bool UPDATED = false;
        public VendorBalanceEntry()
        { }
        public VendorBalanceEntry(VendorBalance vendorBalance, EventJournalItem item)
        {
            VENDOR_BALANCE = vendorBalance;
            VENDOR_BALANCE_ENTRY_TYPE = item.VENDOR_BALANCE_ENTRY_TYPE;
            TRANSACTION_DATE = item.EVENT_JOURNAL.TRANSACTION_DATE;
            CURRENCY = item.CURRENCY;
            item.VENDOR_BALANCE_ENTRY = this;
            EVENT_JOURNAL_ITEM = item;
            AMOUNT = item.AMOUNT;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_vendorbalanceentry
                (   
                    vb_id,
                    vbe_vendorbalanceentrytype,
                    vbe_date,
                    ccy_id,
                    vbe_amount,
                    eventjournalitem_id
                ) 
                VALUES ({0},'{1}','{2}',{3},{4},{5})",
                VENDOR_BALANCE.ID,
                VENDOR_BALANCE_ENTRY_TYPE.ToString(),
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                CURRENCY.ID,
                AMOUNT,
                EVENT_JOURNAL_ITEM.ID
                );
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_vendorbalanceentry set 
                    vb_id = {0},
                    vbe_vendorbalanceentrytype = '{1}',
                    vbe_date = '{2}',
                    ccy_id = {3},
                    vbe_amount = {4},
                    eventjournalitem_id = {5}
                where vbe_id = {6}",
                VENDOR_BALANCE.ID,
                VENDOR_BALANCE_ENTRY_TYPE.ToString(),
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                CURRENCY.ID,
                AMOUNT,
                EVENT_JOURNAL_ITEM.ID,
                ID);
        }
        public static VendorBalanceEntry TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            VendorBalanceEntry tr = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                tr = new VendorBalanceEntry();
                tr.ID = Convert.ToInt32(aReader["vbe_id"]);
                tr.VENDOR_BALANCE = new VendorBalance(Convert.ToInt32(aReader["vb_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = (VendorBalanceEntryType)Enum.Parse(typeof(VendorBalanceEntryType), aReader["vbe_vendorbalanceentrytype"].ToString());
                tr.TRANSACTION_DATE = Convert.ToDateTime(aReader["vbe_date"]);
                tr.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                tr.AMOUNT = Convert.ToDouble(aReader["vbe_amount"]);
                tr.EVENT_JOURNAL_ITEM = new EventJournalItem(Convert.ToInt32(aReader["eventjournalitem_id"]));
            }
            return tr;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                VendorBalanceEntry tr = new VendorBalanceEntry();
                tr.ID = Convert.ToInt32(aReader["vbe_id"]);
                tr.VENDOR_BALANCE = new VendorBalance(Convert.ToInt32(aReader["vb_id"]));
                tr.VENDOR_BALANCE_ENTRY_TYPE = (VendorBalanceEntryType)Enum.Parse(typeof(VendorBalanceEntryType), aReader["vbe_vendorbalanceentrytype"].ToString());
                tr.TRANSACTION_DATE = Convert.ToDateTime(aReader["vbe_date"]);
                tr.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                tr.AMOUNT = Convert.ToDouble(aReader["vbe_amount"]);
                tr.EVENT_JOURNAL_ITEM = new EventJournalItem(Convert.ToInt32(aReader["eventjournalitem_id"]));
                result.Add(tr);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(vbe_id) from table_vendorbalanceentry");
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_vendorbalanceentry where vbe_id ={0}", id);
        }
        public static string FindByEventJournalItem(int id, VendorBalanceEntryType type)
        {
            return String.Format("Select * from table_vendorbalanceentry where eventjournalitem_id ={0} and vbe_vendorbalanceentrytype = '{1}'", id, type.ToString());
        }
        public static string FindByVendorBalance(int id)
        {
            return String.Format("Select * from table_vendorbalanceentry where vb_id ={0}", id);
        }
        public override bool Equals(object obj)
        {
            VendorBalanceEntry sce = (VendorBalanceEntry)obj;
            if (sce == null) return false;
            return sce.ID == ID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
