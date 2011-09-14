using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class VendorBalance : IEntity
    {
        public int ID = 0;
        public VendorBalanceType VENDOR_BALANCE_TYPE = VendorBalanceType.Supplier;
        public Period PERIOD;
        public Vendor VENDOR;
        public Currency CURRENCY;
        public double BALANCE = 0;
        public IList VENDOR_BALANCE_ENTRIES = new ArrayList();

        public VendorBalance() : base() { }
        public VendorBalance(int id) { ID = id; }
        public VendorBalance(Period period, Vendor vendor, Currency currency, VendorBalanceType type)
        {
            PERIOD = period;
            VENDOR = vendor;
            CURRENCY = currency;
            VENDOR_BALANCE_TYPE = type;
        }
        public void ProcessPosting(EventJournalItem eventItem)
        {
            assertSameCurrency(eventItem.CURRENCY);
            assertPeriodIsCurrent();
            assertInPeriodRange(eventItem.EVENT_JOURNAL.TRANSACTION_DATE);
            double qty = eventItem.AMOUNT;
            switch (eventItem.VENDOR_BALANCE_ENTRY_TYPE)
            {
                case VendorBalanceEntryType.SupplierOutStandingInvoice:
                    BALANCE += qty;
                    break;
                case VendorBalanceEntryType.CustomerOutStandingInvoice:
                    BALANCE += qty;
                    break;
                case VendorBalanceEntryType.SupplierInvoice:
                    BALANCE += qty;
                    break;
                case VendorBalanceEntryType.CustomerInvoice:
                    BALANCE += qty;
                    break;
                case VendorBalanceEntryType.Payment:
                    BALANCE -= qty;
                    assertNotMinusBalanceAmount();
                    break;
                case VendorBalanceEntryType.Receipt:
                    BALANCE -= qty;
                    assertNotMinusBalanceAmount();
                    break;
                default:
                    break;
            }
            VendorBalanceEntry sce = new VendorBalanceEntry(this, eventItem);
            VENDOR_BALANCE_ENTRIES.Add(sce);
        }
        public void ProcessUnPosting(EventJournalItem eventItem)
        {
            assertSameCurrency(eventItem.CURRENCY);
            assertPeriodIsCurrent();
            assertInPeriodRange(eventItem.EVENT_JOURNAL.TRANSACTION_DATE);
            double qty = eventItem.AMOUNT;
            switch (eventItem.VENDOR_BALANCE_ENTRY_TYPE)
            {
                case VendorBalanceEntryType.SupplierOutStandingInvoice:
                    BALANCE -= qty;
                    assertNotMinusBalanceAmount();
                    break;
                case VendorBalanceEntryType.CustomerOutStandingInvoice:
                    BALANCE -= qty;
                    assertNotMinusBalanceAmount();
                    break;
                case VendorBalanceEntryType.SupplierInvoice:
                    assertNotMinusBalanceAmount();
                    BALANCE -= qty;
                    break;
                case VendorBalanceEntryType.CustomerInvoice:
                    assertNotMinusBalanceAmount();
                    BALANCE -= qty;
                    break;
                case VendorBalanceEntryType.Payment:
                    BALANCE += qty;
                    break;
                case VendorBalanceEntryType.Receipt:
                    BALANCE += qty;
                    break;
                default:
                    break;
            }
        }
        private void assertNotMinusBalanceAmount()
        {
            if (BALANCE < 0)
                throw new Exception(" Vendor Balance (" + VENDOR.NAME + "): Minus Balance Amount On Currency (" + CURRENCY.CODE + "). Available Balance : " + BALANCE + " " + CURRENCY.CODE);
        }
        private void assertInPeriodRange(DateTime date)
        {
            if (!PERIOD.IsInRange(date))
                throw new Exception(" Vendor Balance : Transaction Date in Not in Period ");
        }
        private void assertPeriodIsCurrent()
        {
            if (PERIOD.PERIOD_STATUS != PeriodStatus.Current)
                throw new Exception(" Vendor Balance : Period Status Not Current ");
        }
        private void assertSameCurrency(Currency currency)
        {
            bool same = this.CURRENCY.ID == currency.ID;
            if (!same)
                throw new Exception(" Vendor Balance : not same Currency ");
        }
        public static VendorBalance CreateVendorBalance(EventJournalItem item, Period period)
        {//
            return new VendorBalance(period, item.VENDOR, item.CURRENCY, item.VENDOR_BALANCE_TYPE);
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_vendorbalance
                (
                    vb_vendorbalancetype,
                    period_id,
                    vendor_id,
                    ccy_id,
                    vb_balance
                ) 
                VALUES ('{0}',{1},{2},{3},{4})",
               VENDOR_BALANCE_TYPE.ToString(), 
               PERIOD.ID,
               VENDOR.ID,
               CURRENCY.ID,
               BALANCE
               );
        }
        public string GetDeleteSQL()
        {
            return "delete from table_vendorbalance where vb_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_vendorbalance set 
                vb_vendorbalancetype = '{0}',
                period_id = {1},
                vendor_id = {2},
                ccy_id = {3},
                vb_balance = {4}
                where vb_id = {5}",
                VENDOR_BALANCE_TYPE.ToString(), 
                PERIOD.ID, 
                VENDOR.ID, 
                CURRENCY.ID, 
                BALANCE, 
                ID);
        }
        public static string GetByIDSQLStatic(int ID)
        {
            return String.Format("select * from table_vendorbalance where vb_id = {0}", ID);
        }
        public static string GetAllSQLStatic()
        {
            return String.Format("select * from table_vendorbalance");
        }
        public static string FindByVendorBalanceBySupplier(int supID)
        {
            return String.Format("select * from table_vendorbalance where vendor_id ={0} and vb_vendorbalancetype ='{1}'", supID, VendorBalanceType.Supplier.ToString());
        }
        public static string FindByVendorBalanceByCustomer(int supID)
        {
            return String.Format("select * from table_vendorbalance where vendor_id ={0} and vb_vendorbalancetype ='{1}'", supID, VendorBalanceType.Customer.ToString());
        }
        public static VendorBalance TransformReader(MySql.Data.MySqlClient.MySqlDataReader a)
        {
            VendorBalance vb = null;
            if (a.HasRows)
            {
                a.Read();
                vb = new VendorBalance();
                vb.ID = Convert.ToInt32(a["vb_id"]);
                vb.VENDOR_BALANCE_TYPE = (VendorBalanceType)Enum.Parse(typeof(VendorBalanceType), a["vb_vendorbalancetype"].ToString());
                vb.PERIOD = new Period(Convert.ToInt32(a["period_id"]));
                vb.VENDOR = vb.VENDOR_BALANCE_TYPE == VendorBalanceType.Supplier ?
                    (Vendor)new Supplier(Convert.ToInt32(a["vendor_id"])) : (Vendor)new Customer(Convert.ToInt32(a["vendor_id"]));
                vb.BALANCE = Convert.ToDouble(a["vb_balance"]);
                vb.CURRENCY = new Currency(Convert.ToInt32(a["ccy_id"]));
            }
            return vb;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader a)
        {
            IList result = new ArrayList();
            while (a.Read())
            {
                VendorBalance vb = new VendorBalance();
                vb.ID = Convert.ToInt32(a["vb_id"]);
                vb.VENDOR_BALANCE_TYPE = (VendorBalanceType)Enum.Parse(typeof(VendorBalanceType), a["vb_vendorbalancetype"].ToString());
                vb.PERIOD = new Period(Convert.ToInt32(a["period_id"]));
                vb.VENDOR = vb.VENDOR_BALANCE_TYPE == VendorBalanceType.Supplier ?
                    (Vendor)new Supplier(Convert.ToInt32(a["vendor_id"])) : (Vendor)new Customer(Convert.ToInt32(a["vendor_id"]));
                vb.BALANCE = Convert.ToDouble(a["vb_balance"]);
                vb.CURRENCY = new Currency(Convert.ToInt32(a["ccy_id"]));
                result.Add(vb);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(vb_id) from table_vendorbalance");
        }
        public static string FindByVendorPeriod(int vendor, int period, VendorBalanceType t)
        {
            return String.Format("SELECT * from table_vendorbalance where period_id = {0} and vendor_id = {1} and vb_vendorbalancetype ='{2}'", 
                period, vendor,t.ToString());
        }

        #region IEntity Members

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public void SetID(int id)
        {
            throw new NotImplementedException();
        }

        public string GetCode()
        {
            throw new NotImplementedException();
        }

        public void SetCode(string code)
        {
            throw new NotImplementedException();
        }

        public string GetByCodeSQL(string code)
        {
            throw new NotImplementedException();
        }

        public string GetMaximumIDSQL()
        {
            throw new NotImplementedException();
        }

        public string GetByCodeLikeSQL(string text)
        {
            throw new NotImplementedException();
        }

        public string GetByNameLikeSQL(string text)
        {
            throw new NotImplementedException();
        }

        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            throw new NotImplementedException();
        }

        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            throw new NotImplementedException();
        }

        public string GetConcatSearch(string find)
        {
            throw new NotImplementedException();
        }
        public VendorBalance Create(Period newPeriod)
        {
            VendorBalance sCard = new VendorBalance(newPeriod, VENDOR, CURRENCY, VENDOR_BALANCE_TYPE);
            sCard.BALANCE = BALANCE;
            return sCard;
        }
        #endregion

        #region IEntity Members


        public string GetAllSQL()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEntity Members


        public string GetByIDSQL(int ID)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
