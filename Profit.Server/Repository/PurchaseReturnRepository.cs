using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PurchaseReturnRepository : TransactionRepository
    {
        public PurchaseReturnRepository() : base() { }

        protected override void doConfirm(Event events, Period p)
        {
            foreach (PurchaseReturnItem item in events.EVENT_ITEMS)
            {
                GoodReceiveNote po = (GoodReceiveNote)item.GRN_ITEM.EVENT;
                assertConfirmedPO(po);
                assertValidDate(po, item.EVENT);
                SetStockCard(item, p);
                item.GRN_ITEM.SetOSAgainstPRItem(item);
                GoodReceiveNoteRepository.UpdateAgainstStatus(m_command, po, item.GRN_ITEM);
            }
        }
        protected override void doRevise(Event events, Period p)
        {
            foreach (PurchaseReturnItem item in events.EVENT_ITEMS)
            {
                assertUsedByAPDN(item);
                GoodReceiveNote po = (GoodReceiveNote)item.GRN_ITEM.EVENT;
                SetStockCard(item, p);
                item.GRN_ITEM.UnSetOSAgainstPRItem(item);
                GoodReceiveNoteRepository.UpdateAgainstStatus(m_command, po, item.GRN_ITEM);
            }
        }

        private void assertUsedByAPDN(PurchaseReturnItem item)
        {
            IList used = APDebitNoteRepository.FindAPDNByPurchaseReturn(m_command, item.EVENT.ID);
            if (used.Count > 0)
                throw new Exception("This Purchase Return [" + item.EVENT.CODE + "] is used by [" + ((APDebitNoteItem)used[0]).EVENT_JOURNAL.CODE + "], please delete APDN first.");
        }
        private void assertConfirmedPO(Event p)
        {
            if (p.EVENT_STATUS.Equals(EventStatus.Entry))
                throw new Exception("PR not confirmed :" + p.CODE);
        }
        private void assertValidDate(Event po, Event grn)
        {
            if (grn.TRANSACTION_DATE < po.TRANSACTION_DATE)
                throw new Exception("PR Date can not less than PO Date :" + po.CODE + " [ " + po.TRANSACTION_DATE.ToString("dd-MMM-yyyy") + " ] ");
        }
        protected override void doSave(Event e)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            try
            {
                m_command.Transaction = trc;

                DateTime trDate = DateTime.Today;
                string codesample = AutoNumberSetupRepository.GetCodeSampleByDomainName(m_command, "PurchaseReturn");
                Event codeDate = FindLastCodeAndTransactionDate(codesample);
                string lastCode = codeDate == null ? string.Empty : codeDate.CODE;
                DateTime lastDate = codeDate == null ? trDate : codeDate.TRANSACTION_DATE;
                int trCount = RecordCount();
                e.CODE = AutoNumberSetupRepository.GetAutoNumberByDomainName(m_command, "PurchaseReturn", e.CODE, lastCode, lastDate, trDate, trCount == 0);

                PurchaseReturn stk = (PurchaseReturn)e;
                m_command.CommandText = e.GetInsertSQL();
                m_command.ExecuteNonQuery();
                m_command.CommandText = PurchaseReturn.SelectMaxIDSQL();
                stk.ID = Convert.ToInt32(m_command.ExecuteScalar());
                foreach (PurchaseReturnItem item in stk.EVENT_ITEMS)
                {
                    m_command.CommandText = item.GetInsertSQL();
                    m_command.ExecuteNonQuery();
                    m_command.CommandText = PurchaseReturnItem.SelectMaxIDSQL();
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
                PurchaseReturn e = (PurchaseReturn)en;
                m_command.CommandText = e.GetUpdateSQL();
                m_command.ExecuteNonQuery();

                foreach (PurchaseReturnItem sti in e.EVENT_ITEMS)
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
                        m_command.CommandText = PurchaseReturnItem.SelectMaxIDSQL();
                        sti.ID = Convert.ToInt32(m_command.ExecuteScalar());
                    }
                }
                m_command.CommandText = PurchaseReturnItem.DeleteUpdate(e.ID, e.EVENT_ITEMS);
                m_command.ExecuteNonQuery();
                //m_command.CommandText = PurchaseReturnItem.GetByEventIDSQL(e.ID);
                //MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                //IList luc = PurchaseReturnItem.TransformReaderList(r);
                //r.Close();
                //foreach (PurchaseReturnItem chk in luc)
                //{
                //    chk.UPDATED = e.EVENT_ITEMS.Contains(chk);
                //}
                //foreach (PurchaseReturnItem chk in luc)
                //{
                //    if (!chk.UPDATED)
                //    {
                //        m_command.CommandText = PurchaseReturnItem.DeleteSQL(chk.ID);
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
            PurchaseReturn st = (PurchaseReturn)e;//this.Get(e.ID);
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                if (getEventStatus(st.ID)== EventStatus.Confirm)
                    throw new Exception("Revise before delete");

                m_command.CommandText = APDebitNoteItem.GetPRUsedByAPDN(st.ID);
                int count = Convert.ToInt32(m_command.ExecuteScalar());
                if (count > 0)
                    throw new Exception("Can not delete this Purchase Return, this Purchase Return used by APDN");

                m_command.CommandText = PurchaseReturnItem.DeleteAllByEventSQL(st.ID);
                m_command.ExecuteNonQuery();
                m_command.CommandText = PurchaseReturn.DeleteSQL(st.ID);
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
            m_command.CommandText = PurchaseReturn.GetEventStatus(id);
            object b = m_command.ExecuteScalar();
            EventStatus m = (EventStatus)Enum.Parse(typeof(EventStatus), b.ToString());
            return m;
        }
        protected override Event doGet(int ID)
        {
            m_command.CommandText = PurchaseReturn.GetByIDSQL(ID);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            PurchaseReturn st = PurchaseReturn.TransformReader(r);
            r.Close();
            m_command.CommandText = PurchaseReturnItem.GetByEventIDSQL(ID);
            r = m_command.ExecuteReader();
            IList stis = PurchaseReturnItem.TransformReaderList(r);
            r.Close();
            foreach (PurchaseReturnItem sti in stis)
            {
                sti.EVENT = st;
                sti.PART = PartRepository.GetByID(m_command, sti.PART.ID);
                sti.STOCK_CARD_ENTRY = StockCardEntryRepository.FindStockCardEntryByEventItem(m_command, sti.ID, sti.STOCK_CARD_ENTRY_TYPE);
                sti.GRN_ITEM = GoodReceiveNoteRepository.FindGoodReceiveNoteItem(m_command, sti.GRN_ITEM.ID);
                sti.GRN_ITEM.PART = PartRepository.GetByID(m_command, sti.GRN_ITEM.PART.ID);
                st.EVENT_ITEMS.Add(sti);
            }
            return st;
        }
        protected override void doUpdateStatus(Event e, bool posted)
        {
            m_command.CommandText = PurchaseReturn.GetUpdateStatusSQL(e);
            m_command.ExecuteNonQuery();
        }
        public static PurchaseReturnItem FindGRNItem(MySql.Data.MySqlClient.MySqlCommand cmd, int grnIID)
        {
            cmd.CommandText = PurchaseReturnItem.FindByGrnItemIDSQL(grnIID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            PurchaseReturnItem res = PurchaseReturnItem.TransformReader(r);
            r.Close();
            if (res == null) return null;
            cmd.CommandText = PurchaseReturn.GetByIDSQL(res.EVENT.ID);
            r = cmd.ExecuteReader();
            res.EVENT = PurchaseReturn.TransformReader(r);
            r.Close();
            return res;
        }
        public static PurchaseReturn GetHeaderOnly(MySql.Data.MySqlClient.MySqlCommand cmd, int prnID)
        {
            cmd.CommandText = PurchaseReturn.GetByIDSQL(prnID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            PurchaseReturn st = PurchaseReturn.TransformReader(r);
            r.Close();
            return st;
        }
        protected override IList doSearch(string find)
        {
            try
            {
                m_command.CommandText = PurchaseReturn.GetSearch(find);
                MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
                IList rest = PurchaseReturn.TransformReaderList(r);
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
                m_command.CommandText = PurchaseReturn.SelectCountByCode(code);
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
            m_command.CommandText = PurchaseReturn.FindLastCodeAndTransactionDate(codesample);
            MySql.Data.MySqlClient.MySqlDataReader r = m_command.ExecuteReader();
            Event e = PurchaseReturn.TransformReader(r);
            r.Close();
            return e;
        }
        public int RecordCount()
        {
            m_command.CommandText = PurchaseReturn.RecordCount();
            int result = Convert.ToInt32(m_command.ExecuteScalar());
            return result;
        }
        public bool IsAutoNumber()
        {
            AutoNumberSetup autonumber = AutoNumberSetupRepository.GetAutoNumberSetup(m_command, "PurchaseReturn");
            return autonumber.AUTONUMBER_SETUP_TYPE == AutoNumberSetupType.Auto;
        }
        public IList FindPRForAPDebitNote(string find, int supID,DateTime trdate,IList added)
        {
            m_command.CommandText = APDebitNoteItem.GetPRUsedByAPDN();
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

            m_command.CommandText = PurchaseReturn.GetSearchPRNoForAPDN(find, supID, pois, trdate);
            r = m_command.ExecuteReader();
            IList result = PurchaseReturn.TransformReaderList(r);
            r.Close();
            foreach (PurchaseReturn p in result)
            {
                m_command.CommandText = PurchaseReturnItem.GetByEventIDSQL(p.ID);
                r = m_command.ExecuteReader();
                p.EVENT_ITEMS = PurchaseReturnItem.TransformReaderList(r);
                r.Close();

                foreach (PurchaseReturnItem t in p.EVENT_ITEMS)
                {

                    if ((t.GRN_ITEM == null) && (t.GRN_ITEM.ID == 0)) continue;


                    m_command.CommandText = GoodReceiveNoteItem.GetByIDSQL(t.GRN_ITEM.ID);
                    r = m_command.ExecuteReader();
                    t.GRN_ITEM = GoodReceiveNoteItem.TransformReader(r);
                    r.Close();

                    if ((t.GRN_ITEM.PO_ITEM == null)) continue;
                    if (t.GRN_ITEM.PO_ITEM.ID == 0) continue;

                    m_command.CommandText = PurchaseOrderItem.GetByIDSQL(t.GRN_ITEM.PO_ITEM.ID);
                    r = m_command.ExecuteReader();
                    t.GRN_ITEM.PO_ITEM = PurchaseOrderItem.TransformReader(r);
                    r.Close();

                    t.GRN_ITEM.PART = PartRepository.GetByID(m_command, t.GRN_ITEM.PART.ID);
                    t.GRN_ITEM.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(m_command, t.GRN_ITEM.PO_ITEM.PART.ID);
                    t.PART = t.GRN_ITEM.PO_ITEM.PART = t.GRN_ITEM.PART;

                    double subamount = (t.GRN_ITEM.PO_ITEM.SUBTOTAL / t.GRN_ITEM.PO_ITEM.GetAmountInSmallestUnit()) * t.GetAmountInSmallestUnit();
                    p.TOTAL_AMOUNT_FROM_PO += subamount;

                    t.GRN_ITEM.PO_ITEM.EVENT = PurchaseOrderRepository.GetHeaderOnly(m_command, t.GRN_ITEM.PO_ITEM.EVENT.ID);
                    p.CURRENCY = ((PurchaseOrder)t.GRN_ITEM.PO_ITEM.EVENT).CURRENCY;
                }
            }
            return result;
        }

        public static PurchaseReturn GetPurchaseReturnForDebitNote(MySql.Data.MySqlClient.MySqlCommand cmd, PurchaseReturn p)
        {
            cmd.CommandText = PurchaseReturnItem.GetByEventIDSQL(p.ID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            p.EVENT_ITEMS = PurchaseReturnItem.TransformReaderList(r);
            r.Close();

            foreach (PurchaseReturnItem t in p.EVENT_ITEMS)
            {

                if ((t.GRN_ITEM == null) && (t.GRN_ITEM.ID == 0)) continue;


                cmd.CommandText = GoodReceiveNoteItem.GetByIDSQL(t.GRN_ITEM.ID);
                r = cmd.ExecuteReader();
                t.GRN_ITEM = GoodReceiveNoteItem.TransformReader(r);
                r.Close();

                if ((t.GRN_ITEM.PO_ITEM == null)) continue;
                if (t.GRN_ITEM.PO_ITEM.ID == 0) continue;

                cmd.CommandText = PurchaseOrderItem.GetByIDSQL(t.GRN_ITEM.PO_ITEM.ID);
                r = cmd.ExecuteReader();
                t.GRN_ITEM.PO_ITEM = PurchaseOrderItem.TransformReader(r);
                r.Close();

                t.GRN_ITEM.PART = PartRepository.GetByID(cmd, t.GRN_ITEM.PART.ID);
                t.GRN_ITEM.PART.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(cmd, t.GRN_ITEM.PO_ITEM.PART.ID);
                t.PART = t.GRN_ITEM.PO_ITEM.PART = t.GRN_ITEM.PART;

                double subamount = (t.GRN_ITEM.PO_ITEM.SUBTOTAL / t.GRN_ITEM.PO_ITEM.GetAmountInSmallestUnit()) * t.GetAmountInSmallestUnit();
                p.TOTAL_AMOUNT_FROM_PO += subamount;

                t.GRN_ITEM.PO_ITEM.EVENT = PurchaseOrderRepository.GetHeaderOnly(cmd, t.GRN_ITEM.PO_ITEM.EVENT.ID);
                p.CURRENCY = ((PurchaseOrder)t.GRN_ITEM.PO_ITEM.EVENT).CURRENCY;
            }
            return p;
        }
    }
}
