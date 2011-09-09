using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SalesReturnRepository : TransactionRepository
    {
        public SalesReturnRepository() : base() { }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (SalesReturnItem item in events.EVENT_ITEMS)
            {
                DeliveryOrder po = (DeliveryOrder)item.DO_ITEM.EVENT;
                assertConfirmedPO(po);  
                assertValidDate(po, item.EVENT);
                SetStockCard(item, p);
                item.DO_ITEM.SetOSAgainstSRItem(item);
                DeliveryOrderRepository.UpdateAgainstStatus(m_command, po, item.DO_ITEM);
            }
        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (SalesReturnItem item in events.EVENT_ITEMS)
            {
                assertUsedByARCR(item);
                DeliveryOrder po = (DeliveryOrder)item.DO_ITEM.EVENT;
                SetStockCard(item, p);
                item.DO_ITEM.UnSetOSAgainstSRItem(item);
                DeliveryOrderRepository.UpdateAgainstStatus(m_command, po, item.DO_ITEM);
            }
        }
        private void assertUsedByARCR(SalesReturnItem item)
        {
            IList used = ARCreditNoteRepository.FindARCRBySalesReturn(m_command, item.EVENT.ID);
            if (used.Count > 0)
                throw new Exception("This Sales Return [" + item.EVENT.CODE + "] is used by [" + ((ARCreditNoteItem)used[0]).EVENT_JOURNAL.CODE + "], please delete APDN first.");
        }
        private void assertConfirmedPO(Event p)
        {
            if (p.EVENT_STATUS.Equals(EventStatus.Entry))
                throw new Exception("SR not confirmed :" + p.CODE);
        }
        private void assertValidDate(Event po, Event grn)
        {
            if (grn.TRANSACTION_DATE < po.TRANSACTION_DATE)
                throw new Exception("SR Date can not less than DO Date :" + po.CODE + " [ " + po.TRANSACTION_DATE.ToString("dd-MMM-yyyy") + " ] ");
        }
        protected override void doSave(Event e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;

                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "SalesReturn");
                Event codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "SalesReturn", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                SalesReturn stk = (SalesReturn)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = SalesReturn.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (SalesReturnItem item in stk.EVENT_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = SalesReturnItem.SelectMaxIDSQL();
                    item.ID = Convert.ToInt32(m_command.ExecuteScalar());
                }
                trc.Commit();
            }
            catch (Exception x)
            {
                e.ID = 0;
                foreach (EventItem item in e.EVENT_ITEMS)
                {
                    item.ID = 0;
                }
                trc.Rollback();
                throw x;
            }
        }
        protected override void doUpdate(Event en)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                SalesReturn e = (SalesReturn)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (SalesReturnItem sti in e.EVENT_ITEMS)
                {
                    if (sti.ID > 0)
                    {
                        m_command.CommandText = sti.GetUpdateSQL();
                        m_command.ExecuteNonQuery();
                    }
                    else
                    {
                        m_command.CommandText = sti.GetInsertSQL();
                        m_command.ExecuteNonQuery();
                        m_command.CommandText = SalesReturnItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = SalesReturnItem.DeleteUpdate(e.ID, e.EVENT_ITEMS);
                m_command.ExecuteNonQuery();
                //m_command.CommandText = SalesReturnItem.GetByEventIDSQL(e.ID);
                //MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                //IList luc = SalesReturnItem.TransformReaderList(r);
                //r.Close();
                //foreach (SalesReturnItem chk in luc)
                //{
                //    chk.UPDATED = e.EVENT_ITEMS.Contains(chk);
                //}
                //foreach (SalesReturnItem chk in luc)
                //{
                //    if (!chk.UPDATED)
                //    {
                //        m_command.CommandText = SalesReturnItem.DeleteSQL(chk.ID);
                //        m_command.ExecuteNonQuery();
                //    }
                //}
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        protected override void doDelete(Event e)
        {
            SalesReturn st = (SalesReturn)e;//this.Get(e.ID);
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");

                m_command.CommandText = ARCreditNoteItem.GetSRUsedByARCR(st.ID);
                int count = Convert.ToInt32(m_command.ExecuteScalar());
                if (count > 0)
                    throw new Exception("Can not delete this Sales Return, this Sales Return used by ARCR");

                m_command.CommandText = SalesReturnItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = SalesReturn.DeleteSQL(st.ID);
                m_command.ExecuteNonQuery();
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        private EventStatus getEventStatus(int id)
        {
            m_command.CommandText = SalesReturn.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = SalesReturn.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            SalesReturn st = SalesReturn.TransformReader(r);
            r.Close();
            m_command.CommandText = SalesReturnItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = SalesReturnItem.TransformReaderList(r);
            r.Close();
            foreach (SalesReturnItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                sti.DO_ITEM = DeliveryOrderRepository.FindDeliveryOrderItem(m_command, sti.DO_ITEM.ID);
                sti.DO_ITEM.PART = PartRepository.GetByID(m_command, sti.DO_ITEM.PART.ID);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(Event e, bool posted)
        {
            m_command.CommandText = SalesReturn.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        public static SalesReturnItem FindDOItem(MySql.Data.MySqlClient.MySqlCommand cmd, int grnIID)
        {
            cmd.CommandText = SalesReturnItem.FindByDOItemIDSQL(grnIID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            SalesReturnItem res = SalesReturnItem.TransformReader(r);
            r.Close();
            if (res == null) return null;
            cmd.CommandText = SalesReturn.GetByIDSQL(res.EVENT.ID);
            r = cmd.ExecuteReader();
            res.EVENT = SalesReturn.TransformReader(r);
            r.Close();
            return res;
        }
        public static SalesReturn GetHeaderOnly(MySql.Data.MySqlClient.MySqlCommand cmd, int prnID)
        {
            cmd.CommandText = SalesReturn.GetByIDSQL(prnID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            SalesReturn st = SalesReturn.TransformReader(r);
            r.Close();
            return st;
        }
        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = SalesReturn.GetSearch(find);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList rest = SalesReturn.TransformReaderList(r);
                r.Close();
                return rest;
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        protected override bool doIsCodeExist(string code)
        {
            try
            {
                m_command.CommandText = SalesReturn.SelectCountByCode(code);
                int t = Convert.ToInt32(m_command.ExecuteScalar());
                return t > 0;
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public override Event FindLastCodeAndTransactionDate(string codesample)
        {
            m_command.CommandText = SalesReturn.FindLastCodeAndTransactionDate(codesample);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            Event e = SalesReturn.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = SalesReturn.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "SalesReturn");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }
        public IList FindSRForARCreditNote(string find, int supID, DateTime trdate, IList added)
        {
            m_command.CommandText = ARCreditNoteItem.GetSRUsedByARCR();
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            if (r.HasRows)
            {
                while (r.Read())
                {
                    int id = Convert.ToInt32(r[0]);
                    if (!added.Contains(id))
                        added.Add(id);
                }
            }
            r.Close();
            StringBuilder poisSB = new StringBuilder();
            foreach (int i in added)
            {
                poisSB.Append(i.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = added.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";

            m_command.CommandText = SalesReturn.GetSearchSRNoForARCR(find, supID, pois, trdate);
            r = m_command.ExecuteReader();
            IList result = SalesReturn.TransformReaderList(r);
            r.Close();
            foreach (SalesReturn p in result)
            {
                m_command.CommandText = SalesReturnItem.GetByEventIDSQL(p.ID);
                r = m_command.ExecuteReader();
                p.EVENT_ITEMS = SalesReturnItem.TransformReaderList(r);
                r.Close();

                foreach (SalesReturnItem t in p.EVENT_ITEMS)
                {

                    if ((t.DO_ITEM == null) && (t.DO_ITEM.ID == 0)) continue;


                    m_command.CommandText = DeliveryOrderItem.GetByIDSQL(t.DO_ITEM.ID);
                    r = m_command.ExecuteReader();
                    t.DO_ITEM = DeliveryOrderItem.TransformReader(r);
                    r.Close();

                    if ((t.DO_ITEM.SO_ITEM == null)) continue;
                    if (t.DO_ITEM.SO_ITEM.ID == 0) continue;

                    m_command.CommandText = SalesOrderItem.GetByIDSQL(t.DO_ITEM.SO_ITEM.ID);
                    r = m_command.ExecuteReader();
                    t.DO_ITEM.SO_ITEM = SalesOrderItem.TransformReader(r);
                    r.Close();

                    t.DO_ITEM.PART = PartRepository.GetByID(m_command, t.DO_ITEM.PART.ID);
                    t.DO_ITEM.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(m_command, t.DO_ITEM.SO_ITEM.PART.ID);
                    t.PART = t.DO_ITEM.SO_ITEM.PART = t.DO_ITEM.PART;

                    double subamount = (t.DO_ITEM.SO_ITEM.SUBTOTAL / t.DO_ITEM.SO_ITEM.GetAmountInSmallestUnit()) * t.GetAmountInSmallestUnit();
                    p.TOTAL_AMOUNT_FROM_SO += subamount;

                    t.DO_ITEM.SO_ITEM.EVENT = SalesOrderRepository.GetHeaderOnly(m_command, t.DO_ITEM.SO_ITEM.EVENT.ID);
                    p.CURRENCY = ((SalesOrder)t.DO_ITEM.SO_ITEM.EVENT).CURRENCY;
                }
            }
            return result;
        }

        public static SalesReturn GetSalesReturnForCreditNote(MySql.Data.MySqlClient.MySqlCommand cmd, SalesReturn p)
        {
            cmd.CommandText = SalesReturnItem.GetByEventIDSQL(p.ID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            p.EVENT_ITEMS = SalesReturnItem.TransformReaderList(r);
            r.Close();

            foreach (SalesReturnItem t in p.EVENT_ITEMS)
            {

                if ((t.DO_ITEM == null) && (t.DO_ITEM.ID == 0)) continue;


                cmd.CommandText = DeliveryOrderItem.GetByIDSQL(t.DO_ITEM.ID);
                r = cmd.ExecuteReader();
                t.DO_ITEM = DeliveryOrderItem.TransformReader(r);
                r.Close();

                if ((t.DO_ITEM.SO_ITEM == null)) continue;
                if (t.DO_ITEM.SO_ITEM.ID == 0) continue;

                cmd.CommandText = SalesOrderItem.GetByIDSQL(t.DO_ITEM.SO_ITEM.ID);
                r = cmd.ExecuteReader();
                t.DO_ITEM.SO_ITEM = SalesOrderItem.TransformReader(r);
                r.Close();

                t.DO_ITEM.PART = PartRepository.GetByID(cmd, t.DO_ITEM.PART.ID);
                t.DO_ITEM.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(cmd, t.DO_ITEM.SO_ITEM.PART.ID);
                t.PART = t.DO_ITEM.SO_ITEM.PART = t.DO_ITEM.PART;

                double subamount = (t.DO_ITEM.SO_ITEM.SUBTOTAL / t.DO_ITEM.SO_ITEM.GetAmountInSmallestUnit()) * t.GetAmountInSmallestUnit();
                p.TOTAL_AMOUNT_FROM_SO += subamount;

                t.DO_ITEM.SO_ITEM.EVENT = SalesOrderRepository.GetHeaderOnly(cmd, t.DO_ITEM.SO_ITEM.EVENT.ID);
                p.CURRENCY = ((SalesOrder)t.DO_ITEM.SO_ITEM.EVENT).CURRENCY;
            }
            return p;
        }
    }
}
