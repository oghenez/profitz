using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class ProcessTransactionRepository : Repository
    {
        OdbcCommand m_command;
        public ProcessTransactionRepository(): base(null)
        {
            m_command = new OdbcCommand("", m_connection);
        }
        public int GetTotalTransactionCount()
        {
            return 10;
//            m_command.CommandText = String.Format(@"select p.po_code from table_purchaseorder p where p.po_posted = false and p.po_date between '{0}' and '{1}'
//                    union
//                    select g.grn_code from table_goodreceivenote g where g.grn_posted = false and g.grn_date between '{0}' and '{1}'
//                    union
//                    select s.stk_code from table_stocktaking s where s.stk_posted = false and s.stk_date between '{0}' and '{1}'
//                    union
//                    select si.si_code from table_supplierinvoice si where si.si_posted = false and si.si_date between '{0}' and '{1}'
//                    union
//                    select pr.prn_code from table_purchasereturn pr where pr.prn_posted = false and pr.prn_date between '{0}' and '{1}'
//                    union
//                    select sij.sij_code from table_supplierinvoicejournal sij where sij.sij_posted = false and sij.sij_date between '{0}' and '{1}'
//                    union
//                    select so.sosti_code from table_supplieroutstandinginvoice so where so.sosti_posted = false and so.sosti_date between '{0}' and '{1}'
//                    union
//                    select ap.apdn_code from table_apdebitnote ap where ap.apdn_posted = false and ap.apdn_date between '{0}' and '{1}'
//                    union
//                    select py.pay_code from table_payment py where py.pay_posted = false and py.pay_date between '{0}' and '{1}'",
//                        start.ToString(Utils.DATE_FORMAT),
//                        end.ToString(Utils.DATE_FORMAT));
//            OdbcDataReader r = m_command.ExecuteReader();
//            IList result = new ArrayList();
//            if (r.HasRows)
//            {
//                result.Add(r[0].ToString());
//            }
//            r.Close();
//            return result;
 
        }
        private IList GetAllCodeListOfNotPostedEvent(DateTime start, DateTime end)
        {
            m_command.CommandText = String.Format(@"select p.po_code from table_purchaseorder p where p.po_posted = false and p.po_date between '{0}' and '{1}'
                    union
                    select g.grn_code from table_goodreceivenote g where g.grn_posted = false and g.grn_date between '{0}' and '{1}'
                    union
                    select s.stk_code from table_stocktaking s where s.stk_posted = false and s.stk_date between '{0}' and '{1}'
                    union
                    select si.si_code from table_supplierinvoice si where si.si_posted = false and si.si_date between '{0}' and '{1}'
                    union
                    select pr.prn_code from table_purchasereturn pr where pr.prn_posted = false and pr.prn_date between '{0}' and '{1}'
                    union
                    select sij.sij_code from table_supplierinvoicejournal sij where sij.sij_posted = false and sij.sij_date between '{0}' and '{1}'
                    union
                    select so.sosti_code from table_supplieroutstandinginvoice so where so.sosti_posted = false and so.sosti_date between '{0}' and '{1}'
                    union
                    select ap.apdn_code from table_apdebitnote ap where ap.apdn_posted = false and ap.apdn_date between '{0}' and '{1}'
                    union
                    select py.pay_code from table_payment py where py.pay_posted = false and py.pay_date between '{0}' and '{1}'", 
                     start.ToString(Utils.DATE_FORMAT),
                     end.ToString(Utils.DATE_FORMAT));
            OdbcDataReader r = m_command.ExecuteReader();
            IList result = new ArrayList();
            if (r.HasRows)
            {
                while (r.Read())
                {
                    result.Add(r[0].ToString());
                }
            }
            r.Close();
            return result;
        }
        private Period GetNextPeriod(Period crnt)
        {
            if (crnt == null)
                throw new Exception("Period Not Found");
            IList periods = PeriodRepository.LoadAll(m_command);
            ArrayList periodss = (ArrayList)periods;
            periodss.Sort(new PeriodComparer());
            int crntdx = periods.IndexOf(crnt);
            int next = crntdx - 1;
            if (next > periods.Count)
                return null;
            return periods[next] as Period;
        }
        public Period GetPrevPeriod(Period crnt)
        {
            if (crnt == null)
                throw new Exception("Period Not Found");
            IList periods = PeriodRepository.LoadAll(m_command);
            ArrayList periodss = (ArrayList)periods;
            periodss.Sort(new PeriodComparer());
            int crntdx = periods.IndexOf(crnt);
            int prev = crntdx + 1;
            if (prev < 0)
                return null;
            if (prev > (periodss.Count - 1))
                return null;
            return periods[prev] as Period;
        }
        public void ProcessTransaction(int currentPeriodId)
        {
            OpenConnection();
            OdbcTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;
                Period crntPeriod = PeriodRepository.FindPeriod(m_command, currentPeriodId) as Period;
                if (crntPeriod == null)
                    throw new Exception("Current Period Not Found!");
                if (crntPeriod.PERIOD_STATUS != PeriodStatus.Current)
                    throw new Exception("Period is not in Active Month!");
                GeneralSetup gs = GeneralSetupRepository.GetGeneralSetup(m_command);
                if (gs.START_ENTRY_PERIOD == null)
                    throw new Exception("Start Entry Month Not Found!");

                IList invTrs = this.GetAllCodeListOfNotPostedEvent(crntPeriod.START_DATE, crntPeriod.END_DATA);

                string invCodes = string.Empty;

                if (invTrs.Count > 0)
                {
                    foreach (string code in invTrs)
                    {
                        invCodes += code + "\r\n";
                    }
                }
                if (invTrs.Count > 0)
                    throw new Exception("Please Post Transaction : \r\n" + invCodes);

                Period nextPeriod = this.GetNextPeriod(crntPeriod) as Period;
                if (nextPeriod == null)
                    throw new Exception("Next Period Not Define!");

                IList stockcards = StockCardRepository.FindStockCardByPeriod(m_command, crntPeriod.ID);
                IList newSCards = new ArrayList();
                for (int i = 0; i < stockcards.Count; i++)
                {
                    StockCard sCard = stockcards[i] as StockCard;
                    newSCards.Add(sCard.Create(nextPeriod));
                }
                foreach (StockCard sc in newSCards)
                {
                    StockCardRepository.SaveHeader(m_command, sc);
                }

                IList vbalances = VendorBalanceRepository.FindVendorBalanceByPeriod(m_command, crntPeriod.ID);
                IList newvbalances = new ArrayList();
                for (int i = 0; i < vbalances.Count; i++)
                {
                    VendorBalance vb = vbalances[i] as VendorBalance;
                    newvbalances.Add(vb.Create(nextPeriod));
                }
                foreach (VendorBalance sc in newvbalances)
                {
                    VendorBalanceRepository.SaveHeader(m_command, sc);
                }
                nextPeriod.PERIOD_STATUS = PeriodStatus.Current;
                nextPeriod.START_DATE = DateTime.Now;
                PeriodRepository.UpdatePeriod(m_command, nextPeriod);

                crntPeriod.PERIOD_STATUS = PeriodStatus.Close;
                crntPeriod.CLOSED_DATE = DateTime.Now;
                PeriodRepository.UpdatePeriod(m_command, crntPeriod);
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }

        public void RollBackTransaction(int currentPeriodId)
        {
             OdbcTransaction trc = m_connection.BeginTransaction();
             try
             {
                 m_command.Transaction = trc;
                 Period crntPeriod = PeriodRepository.FindPeriod(m_command, currentPeriodId) as Period;
                 if (crntPeriod == null)
                     throw new Exception("Current Period Not Found!");
                 if (crntPeriod.PERIOD_STATUS != PeriodStatus.Current)
                     throw new Exception("Period is not in Active Month!");
                 GeneralSetup gs = GeneralSetupRepository.GetGeneralSetup(m_command);
                 if (gs.START_ENTRY_PERIOD == null)
                     throw new Exception("Start Entry Month Not Found!");
                 IList invTrs = this.GetAllCodeListOfNotPostedEvent(crntPeriod.START_DATE, crntPeriod.END_DATA);
                 string invCodes = string.Empty;
                 if (invTrs.Count > 0)
                 {
                     foreach (string code in invTrs)
                     {
                         invCodes += code + "\r\n";
                     }
                 }

                 if (invTrs.Count > 0)
                     throw new Exception("Please Unpost Transaction : \r\n" + invCodes );
                 Period prevPeriod = this.GetPrevPeriod(crntPeriod) as Period;
                 if (prevPeriod == null)
                     throw new Exception("Previous Period Not Define!");
                 IList stockcards = StockCardRepository.FindStockCardByPeriod(m_command, crntPeriod.ID);
                 IList vbalances = VendorBalanceRepository.FindVendorBalanceByPeriod(m_command, crntPeriod.ID);

                 foreach (StockCard sc in stockcards)
                 {
                     StockCardRepository.DeleteHeader(m_command, sc);
                 }
                 foreach (VendorBalance sc in vbalances)
                 {
                     VendorBalanceRepository.DeleteHeader(m_command, sc);
                 }
                 prevPeriod.PERIOD_STATUS = PeriodStatus.Current;
                 PeriodRepository.UpdatePeriod(m_command, prevPeriod);

                 crntPeriod.PERIOD_STATUS = PeriodStatus.Open;
                 PeriodRepository.UpdatePeriod(m_command, crntPeriod);
                 trc.Commit();
             }
             catch (Exception x)
             {
                 trc.Rollback();
                 throw x;
             }

        }
        public class PeriodComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                Period dtoX = (Period)x;
                Period dtoY = (Period)y;
                int res = Comparer.DefaultInvariant.Compare(dtoY.ID, dtoX.ID);
                return res;
            }

            #endregion
        }
    }
}
