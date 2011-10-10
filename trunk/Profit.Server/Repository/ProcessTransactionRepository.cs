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
        MySql.Data.MySqlClient.MySqlCommand m_command;
        OpeningStockRepository r_openingStock = null;

        public ProcessTransactionRepository(): base(null)
        {
            m_command = new MySql.Data.MySqlClient.MySqlCommand("", m_connection);
            r_openingStock = new OpeningStockRepository();
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
//            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
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
                    select py.pay_code from table_payment py where py.pay_posted = false and py.pay_date between '{0}' and '{1}'
                    union
                    select p.so_code from table_salesorder p where p.so_posted = false and p.so_date between '{0}' and '{1}'
                    union
                    select g.do_code from table_deliveryorder g where g.do_posted = false and g.do_date between '{0}' and '{1}'
                    union
                    select ci.ci_code from table_customerinvoice ci where ci.ci_posted = false and ci.ci_date between '{0}' and '{1}'
                    union
                    select pr.srn_code from table_salesreturn pr where pr.srn_posted = false and pr.srn_date between '{0}' and '{1}'
                    union
                    select sij.cij_code from table_customerinvoicejournal sij where sij.cij_posted = false and sij.cij_date between '{0}' and '{1}'
                    union
                    select so.costi_code from table_customeroutstandinginvoice so where so.costi_posted = false and so.costi_date between '{0}' and '{1}'
                    union
                    select ap.arcr_code from table_arcreditnote ap where ap.arcr_posted = false and ap.arcr_date between '{0}' and '{1}'
                    union
                    select py.rec_code from table_receipt py where py.rec_posted = false and py.rec_date between '{0}' and '{1}'
                    union
                    select pss.pos_code from table_pos pss where pss.pos_posted = false and pss.pos_date between '{0}' and '{1}'
", 
                     start.ToString(Utils.DATE_FORMAT),
                     end.ToString(Utils.DATE_FORMAT_SHORT_END));
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
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
        private IList GetAllCodeListOfPostedEvent(DateTime start, DateTime end)
        {
            m_command.CommandText = String.Format(@"select p.po_code from table_purchaseorder p where p.po_posted = true and p.po_date between '{0}' and '{1}'
                    union
                    select g.grn_code from table_goodreceivenote g where g.grn_posted = true and g.grn_date between '{0}' and '{1}'
                    union
                    select s.stk_code from table_stocktaking s where s.stk_posted = true and s.stk_date between '{0}' and '{1}'
                    union
                    select si.si_code from table_supplierinvoice si where si.si_posted = true and si.si_date between '{0}' and '{1}'
                    union
                    select pr.prn_code from table_purchasereturn pr where pr.prn_posted = true and pr.prn_date between '{0}' and '{1}'
                    union
                    select sij.sij_code from table_supplierinvoicejournal sij where sij.sij_posted = true and sij.sij_date between '{0}' and '{1}'
                    union
                    select so.sosti_code from table_supplieroutstandinginvoice so where so.sosti_posted = true and so.sosti_date between '{0}' and '{1}'
                    union
                    select ap.apdn_code from table_apdebitnote ap where ap.apdn_posted = true and ap.apdn_date between '{0}' and '{1}'
                    union
                    select py.pay_code from table_payment py where py.pay_posted = true and py.pay_date between '{0}' and '{1}'
                    union
                    select p.so_code from table_salesorder p where p.so_posted = true and p.so_date between '{0}' and '{1}'
                    union
                    select g.do_code from table_deliveryorder g where g.do_posted = true and g.do_date between '{0}' and '{1}'
                    union
                    select ci.ci_code from table_customerinvoice ci where ci.ci_posted = true and ci.ci_date between '{0}' and '{1}'
                    union
                    select pr.srn_code from table_salesreturn pr where pr.srn_posted = true and pr.srn_date between '{0}' and '{1}'
                    union
                    select sij.cij_code from table_customerinvoicejournal sij where sij.cij_posted = true and sij.cij_date between '{0}' and '{1}'
                    union
                    select so.costi_code from table_customeroutstandinginvoice so where so.costi_posted = true and so.costi_date between '{0}' and '{1}'
                    union
                    select ap.arcr_code from table_arcreditnote ap where ap.arcr_posted = true and ap.arcr_date between '{0}' and '{1}'
                    union
                    select py.rec_code from table_receipt py where py.rec_posted = true and py.rec_date between '{0}' and '{1}'
                    union
                    select pss.pos_code from table_pos pss where pss.pos_posted = true and pss.pos_date between '{0}' and '{1}'
",
                     start.ToString(Utils.DATE_FORMAT),
                     end.ToString(Utils.DATE_FORMAT_SHORT_END));
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
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
        public void ProcessTransaction(int currentPeriodId, Employee emp)
        {
            OpenConnection();
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
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

                nextPeriod.PERIOD_STATUS = PeriodStatus.Current;
                nextPeriod.CLOSED_DATE = DateTime.Now;
                PeriodRepository.UpdatePeriod(m_command, nextPeriod);

                crntPeriod.PERIOD_STATUS = PeriodStatus.Close;
                crntPeriod.CLOSED_DATE = DateTime.Now;

                PeriodRepository.UpdatePeriod(m_command, crntPeriod);

                IList newSCards = new ArrayList();

                OpeningStock ops = new OpeningStock();
                ops.TRANSACTION_DATE = nextPeriod.START_DATE;
                ops.CURRENCY = CurrencyRepository.GetBaseCurrency(m_command);
                ops.DOCUMENT_DATE = DateTime.Today;
                ops.EMPLOYEE = emp;
                ops.MODIFIED_BY = emp.NAME;
                ops.MODIFIED_DATE = DateTime.Today;
                ops.NOTES = "AUTO" + nextPeriod.START_DATE.ToString(Utils.DATE_FORMAT_SHORT);
                ops.NOTICE_DATE = DateTime.Today;
                ops.WAREHOUSE = getCommonStore();
                double total = 0;

                for (int i = 0; i < stockcards.Count; i++)
                {
                    StockCard sCard = stockcards[i] as StockCard;
                    if (sCard.BALANCE == 0) continue;
                    OpeningStockItem opi = new OpeningStockItem();
                    opi.EVENT = ops;
                    opi.PART = sCard.PART;
                    opi.PRICE = PartRepository.GetLatestPriceMovementItemPeriod(m_command, sCard.PART.ID,
                        crntPeriod.START_DATE, crntPeriod.END_DATA);
                    opi.QYTAMOUNT = sCard.BALANCE;
                    opi.TOTAL_AMOUNT = opi.PRICE * sCard.BALANCE;
                    Part p = PartRepository.GetByID(m_command, sCard.PART.ID);
                    opi.UNIT = p.UNIT;
                    opi.WAREHOUSE = sCard.WAREHOUSE;
                    ops.EVENT_ITEMS.Add(opi);
                    total+=opi.TOTAL_AMOUNT;
                    // newSCards.Add(sCard.Create(nextPeriod));
                }
                ops.AMOUNT = total;
                r_openingStock.SaveNoTransaction(ops, m_command);
                r_openingStock.ConfirmNoTransaction(ops.ID, m_command);
                //foreach (StockCard sc in newSCards)
                //{
                   // StockCardRepository.SaveHeader(m_command, sc);
               // }

                IList vbalances = VendorBalanceRepository.FindVendorBalanceByPeriod(m_command, crntPeriod.ID);
                IList newvbalances = new ArrayList();
                for (int i = 0; i < vbalances.Count; i++)
                {
                    VendorBalance vb = vbalances[i] as VendorBalance;
                    newvbalances.Add(vb.Create(nextPeriod));
                    //if (vb.VENDOR_BALANCE_TYPE == VendorBalanceType.Customer)
                    //{
                    //    CustomerOutStandingInvoice cosi = new CustomerOutStandingInvoice();
                    //    cosi.CURRENCY = vb.CURRENCY;
                    //    cosi.TRANSACTION_DATE = nextPeriod.START_DATE;
                    //    cosi.EMPLOYEE = emp;
                    //    cosi.MODIFIED_BY = emp.NAME;
                    //    cosi.MODIFIED_COMPUTER_NAME = Environment.MachineName;
                    //    cosi.MODIFIED_DATE = DateTime.Now;
                    //    cosi.NOTES = "AUTO";
                    //    cosi.NOTICE_DATE = DateTime.Now;
                    //    cosi.VENDOR = vb.VENDOR;
                    //    CustomerOutStandingInvoiceItem cosii = new CustomerOutStandingInvoiceItem();
                    //    cosii.EVENT_JOURNAL = cosi;
                    //    cosii.INVOICE_NO = "AUTO_OPENING_BALANCE";
                    //    cosii.INVOICE_DATE = DateTime.Today;
                    //    cosii.TOP = getCommonTOP();
                    //    cosii.DUE_DATE = DateTime.Today;
                    //    cosii.EMPLOYEE = emp;
                    //    cosii.AMOUNT = vb.BALANCE;
                    //    cosii.
                    //}
                }
                foreach (VendorBalance sc in newvbalances)
                {
                    VendorBalanceRepository.SaveHeader(m_command, sc);
                }
               
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
            OpenConnection();
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
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

                 OpeningStock p = r_openingStock.GetOpeningStockByNotes("AUTO" + crntPeriod.START_DATE.ToString(Utils.DATE_FORMAT_SHORT));
                 r_openingStock.ReviseNoTransaction(p.ID, m_command);
                 r_openingStock.DeleteNoTransaction(p, m_command);

                 IList invTrs = GetAllCodeListOfPostedEvent(crntPeriod.START_DATE, crntPeriod.END_DATA);
                 //this.GetAllCodeListOfNotPostedEvent(crntPeriod.START_DATE, crntPeriod.END_DATA);
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
        private Warehouse getCommonStore()
        {
            m_command.CommandText = Warehouse.GetByCodeSQLStatic("CMN");
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            Warehouse re = Warehouse.GetWarehouse(r);
            r.Close();
            return re;
        }
        private TermOfPayment getCommonTOP()
        {
            m_command.CommandText = TermOfPayment.GetByCodeStaticSQL("COD");
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            TermOfPayment re = TermOfPayment.GetTOP(r);
            r.Close();
            return re;
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
