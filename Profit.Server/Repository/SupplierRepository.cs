using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Profit.Server
{
    public class SupplierRepository :Repository
    {
        public SupplierRepository():base(new Supplier())
        { }

        public IList GetAllTransactions(int supID)
        {
            ArrayList result = new ArrayList();
            OpenConnection();
            MySql.Data.MySqlClient.MySqlDataReader r;

            m_cmd.CommandText = PurchaseOrder.GetBySupplierSQL(supID);
            r = m_cmd.ExecuteReader();
            IList po = PurchaseOrder.TransformReaderList(r);
            r.Close();
            foreach (Event e in po)
            {
                result.Add(e);
            }

            m_cmd.CommandText = GoodReceiveNote.GetBySupplierSQL(supID);
            r = m_cmd.ExecuteReader();
            IList grn = GoodReceiveNote.TransformReaderList(r);
            r.Close();
            foreach (Event e in grn)
            {
                result.Add(e);
            }

            m_cmd.CommandText = SupplierInvoice.GetBySupplierSQL(supID);
            r = m_cmd.ExecuteReader();
            IList si = SupplierInvoice.TransformReaderList(r);
            r.Close();
            foreach (Event e in si)
            {
                result.Add(e);
            }

            m_cmd.CommandText = PurchaseReturn.GetBySupplierSQL(supID);
            r = m_cmd.ExecuteReader();
            IList pr = PurchaseReturn.TransformReaderList(r);
            r.Close();
            foreach (Event e in pr)
            {
                result.Add(e);
            }

            m_cmd.CommandText = APDebitNote.GetBySupplierSQL(supID);
            r = m_cmd.ExecuteReader();
            IList apdn = APDebitNote.TransformReaderList(r);
            r.Close();
            foreach (EventJournal e in apdn)
            {
                result.Add(e);
            }

            m_cmd.CommandText = Payment.GetBySupplierSQL(supID);
            r = m_cmd.ExecuteReader();
            IList py = Payment.TransformReaderList(r);
            r.Close();
            foreach (EventJournal e in py)
            {
                result.Add(e);
            }
            //result.Sort(new Eve
            return result;
        }
        private class EventDateComparer : IComparer
        {

            #region IComparer Members

            public int Compare(object x, object y)
            {
                Event X = (Event)x;
                Event Y = (Event)y;
                return DateTime.Compare(X.TRANSACTION_DATE, Y.TRANSACTION_DATE);
            }

            #endregion
        }
    }
}
