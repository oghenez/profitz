using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Profit.Server
{
    public class CustomerRepository :Repository
    {
        public CustomerRepository()
            : base(new Customer())
        { }
        public IList GetAllActive()
        {
            OpenConnection();
            MySql.Data.MySqlClient.MySqlDataReader r;
            m_cmd.CommandText = Customer.GetAllActiveSQL();
            r  = m_cmd.ExecuteReader();
            IList result = Customer.GetAllTransform(r);
            r.Close();
            return result;
        }
        public IList GetAllTransactions(int supID)
        {
            ArrayList result = new ArrayList();
            OpenConnection();
            MySql.Data.MySqlClient.MySqlDataReader r;

            m_cmd.CommandText = SalesOrder.GetByCustomerSQL(supID);
            r = m_cmd.ExecuteReader();
            IList po = SalesOrder.TransformReaderList(r);
            r.Close();
            foreach (Event e in po)
            {
                result.Add(e);
            }

            m_cmd.CommandText = DeliveryOrder.GetByCustomerSQL(supID);
            r = m_cmd.ExecuteReader();
            IList grn = DeliveryOrder.TransformReaderList(r);
            r.Close();
            foreach (Event e in grn)
            {
                result.Add(e);
            }

            m_cmd.CommandText = CustomerInvoice.GetByCustomerSQL(supID);
            r = m_cmd.ExecuteReader();
            IList si = CustomerInvoice.TransformReaderList(r);
            r.Close();
            foreach (Event e in si)
            {
                result.Add(e);
            }

            m_cmd.CommandText = POS.GetByCustomerSQL(supID);
            r = m_cmd.ExecuteReader();
            IList pos = POS.TransformReaderList(r);
            r.Close();
            foreach (Event e in pos)
            {
                result.Add(e);
            }

            m_cmd.CommandText = SalesReturn.GetByCustomerSQL(supID);
            r = m_cmd.ExecuteReader();
            IList pr = SalesReturn.TransformReaderList(r);
            r.Close();
            foreach (Event e in pr)
            {
                result.Add(e);
            }

            m_cmd.CommandText = ARCreditNote.GetByCustomerSQL(supID);
            r = m_cmd.ExecuteReader();
            IList apdn = ARCreditNote.TransformReaderList(r);
            r.Close();
            foreach (EventJournal e in apdn)
            {
                result.Add(e);
            }

            m_cmd.CommandText = Receipt.GetByCustomerSQL(supID);
            r = m_cmd.ExecuteReader();
            IList py = Receipt.TransformReaderList(r);
            r.Close();
            foreach (EventJournal e in py)
            {
                result.Add(e);
            }

            m_cmd.CommandText = CustomerOutStandingInvoice.GetByCustomerSQL(supID);
            r = m_cmd.ExecuteReader();
            IList soi = CustomerOutStandingInvoice.TransformReaderList(r);
            r.Close();
            foreach (EventJournal e in soi)
            {
                result.Add(e);
            }

            result.Sort(new EventDateComparer());
            return result;
        }
        public IList GetAllTransactions(int supID, DateTime startDate, DateTime enDate, bool allStatus, bool status)
        {
            ArrayList result = new ArrayList();
            OpenConnection();
            MySql.Data.MySqlClient.MySqlDataReader r;

            m_cmd.CommandText = SalesOrder.GetByCustomerSQL(startDate, enDate, supID, allStatus, status);
            r = m_cmd.ExecuteReader();
            IList po = SalesOrder.TransformReaderList(r);
            r.Close();
            foreach (Event e in po)
            {
                result.Add(e);
            }

            m_cmd.CommandText = DeliveryOrder.GetByCustomerSQL(startDate, enDate, supID, allStatus, status);
            r = m_cmd.ExecuteReader();
            IList grn = DeliveryOrder.TransformReaderList(r);
            r.Close();
            foreach (Event e in grn)
            {
                result.Add(e);
            }

            m_cmd.CommandText = CustomerInvoice.GetByCustomerSQL(startDate, enDate, supID, allStatus, status);
            r = m_cmd.ExecuteReader();
            IList si = CustomerInvoice.TransformReaderList(r);
            r.Close();
            foreach (Event e in si)
            {
                result.Add(e);
            }

            m_cmd.CommandText = POS.GetByCustomerSQL(startDate, enDate, supID, allStatus, status);
            r = m_cmd.ExecuteReader();
            IList pos = POS.TransformReaderList(r);
            r.Close();
            foreach (Event e in pos)
            {
                result.Add(e);
            }

            m_cmd.CommandText = SalesReturn.GetByCustomerSQL(startDate, enDate, supID, allStatus, status);
            r = m_cmd.ExecuteReader();
            IList pr = SalesReturn.TransformReaderList(r);
            r.Close();
            foreach (Event e in pr)
            {
                result.Add(e);
            }

            m_cmd.CommandText = ARCreditNote.GetByCustomerSQL(startDate, enDate, supID, allStatus, status);
            r = m_cmd.ExecuteReader();
            IList apdn = ARCreditNote.TransformReaderList(r);
            r.Close();
            foreach (EventJournal e in apdn)
            {
                result.Add(e);
            }

            m_cmd.CommandText = Receipt.GetByCustomerSQL(startDate, enDate, supID, allStatus, status);
            r = m_cmd.ExecuteReader();
            IList py = Receipt.TransformReaderList(r);
            r.Close();
            foreach (EventJournal e in py)
            {
                result.Add(e);
            }

            m_cmd.CommandText = CustomerOutStandingInvoice.GetByCustomerSQL(startDate, enDate, supID, allStatus, status);
            r = m_cmd.ExecuteReader();
            IList soi = CustomerOutStandingInvoice.TransformReaderList(r);
            r.Close();
            foreach (EventJournal e in soi)
            {
                result.Add(e);
            }

            result.Sort(new EventDateComparer());
            return result;
        }
        public IList GetVendorBalanceEntry(int supID)
        {
            ArrayList result = new ArrayList();
            OpenConnection();
            MySql.Data.MySqlClient.MySqlDataReader r;

            m_cmd.CommandText = VendorBalanceEntry.FindByVendorBalanceByCustomer(supID, VendorBalanceType.Customer);
            r= m_cmd.ExecuteReader();
            IList vbe = VendorBalanceEntry.TransformReaderList(r);
            r.Close();

            foreach (VendorBalanceEntry v in vbe)
            {
                result.Add(v);
            }
            result.Sort(new VendorBalanceEntryDateComparer());
            return result;
        }
        public IList GetVendorBalances(int supID)
        {
            ArrayList result = new ArrayList();
            OpenConnection();
            MySql.Data.MySqlClient.MySqlDataReader r;

            m_cmd.CommandText = VendorBalance.FindByVendorBalanceByCustomer(supID);
            r = m_cmd.ExecuteReader();
            IList vbe = VendorBalance.TransformReaderList(r);
            r.Close();
            foreach (VendorBalance v in vbe)
            {
                result.Add(v);
            }
            result.Sort(new VendorBalanceComparer());
            return result;
        }
        private class VendorBalanceComparer : IComparer
        {

            #region IComparer Members

            public int Compare(object x, object y)
            {
                VendorBalance X = (VendorBalance)x;
                VendorBalance Y = (VendorBalance)y;
                return new CaseInsensitiveComparer().Compare(X.PERIOD.ID, Y.PERIOD.ID);
            }

            #endregion
        }
        private class VendorBalanceEntryDateComparer : IComparer
        {

            #region IComparer Members

            public int Compare(object x, object y)
            {
                VendorBalanceEntry X = (VendorBalanceEntry)x;
                VendorBalanceEntry Y = (VendorBalanceEntry)y;
                return DateTime.Compare(X.TRANSACTION_DATE, Y.TRANSACTION_DATE);
            }

            #endregion
        }
        private class EventDateComparer : IComparer
        {

            #region IComparer Members

            public int Compare(object x, object y)
            {
                DateTime xdate = DateTime.Today;
                DateTime ydate = DateTime.Today;
                if (x is Event)
                {
                    Event ev = (Event)x;
                    xdate = ev.TRANSACTION_DATE;
                }
                if (x is EventJournal)
                {
                    EventJournal ev = (EventJournal)x;
                    xdate = ev.TRANSACTION_DATE;
                }
                if (y is Event)
                {
                    Event ev = (Event)y;
                    ydate = ev.TRANSACTION_DATE;
                }
                if (y is EventJournal)
                {
                    EventJournal ev = (EventJournal)y;
                    ydate = ev.TRANSACTION_DATE;
                }
                return DateTime.Compare(xdate, ydate);
            }

            #endregion
        }
    }
}
