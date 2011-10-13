using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Profit.Server
{
    public class StockReportRepository : Repository
    {
        PartRepository r_partRep = new PartRepository(); 
        public StockReportRepository():base(null)
        {
            OpenConnection();
        }
        public IList GetAllPartForReport()
        {
            m_cmd.CommandText = Part.GetAllSQLStatic();
            MySql.Data.MySqlClient.MySqlDataReader rdr = m_cmd.ExecuteReader();
            IList result = Part.GetAllStaticForReport(rdr);
            rdr.Close();
            return result;
        }
        public IList GetAllPartGroupForReport()
        {
            m_cmd.CommandText = PartGroup.GetAllSQLStatic();
            MySql.Data.MySqlClient.MySqlDataReader rdr = m_cmd.ExecuteReader();
            IList result = PartGroup.GetAllStaticForReport(rdr);
            rdr.Close();
            return result;
        }
        public DataSet GetStockReport(bool allPart, string partStart, string partEnd, bool allGroup, 
            string groupStart, string groupEnd, DateTime asOfDate )
        {
            DataSet ds = new DataSet();
            m_cmd.CommandText = Part.GetStockReport(allPart, partStart, partEnd, allGroup, groupStart, groupEnd);
            MySql.Data.MySqlClient.MySqlDataReader rdr = m_cmd.ExecuteReader();
            IList result = Part.GetAllStaticForReport(rdr);
            rdr.Close();
            IList stockcards = new ArrayList();
            foreach (Part p in result)
            {
                StockCard sc = new StockCard(p, null, null);
                m_cmd.CommandText = StockCardEntry.FindByStockCardEntryByPart(p.ID, new DateTime(2000,1,1),asOfDate );
                rdr = m_cmd.ExecuteReader();
                sc.STOCK_CARD_ENTRIES = StockCardEntry.TransformReaderList(rdr);
                rdr.Close();
                foreach (StockCardEntry e in sc.STOCK_CARD_ENTRIES)
                {
                    if (e.STOCK_CARD_ENTRY_TYPE == StockCardEntryType.SupplierInvoice)
                    {
                        m_cmd.CommandText = SupplierInvoiceItem.GetByIDSQL(e.EVENT_ITEM.ID);
                        rdr = m_cmd.ExecuteReader();
                        e.EVENT_ITEM = SupplierInvoiceItem.TransformReader(rdr);
                        rdr.Close();
                    }
                    if (e.STOCK_CARD_ENTRY_TYPE == StockCardEntryType.CustomerInvoice)
                    {
                        m_cmd.CommandText = CustomerInvoiceItem.GetByIDSQL(e.EVENT_ITEM.ID);
                        rdr = m_cmd.ExecuteReader();
                        e.EVENT_ITEM = CustomerInvoiceItem.TransformReader(rdr);
                        rdr.Close();
                    }
                }
                sc.recalculateAvailable();
                stockcards.Add(sc);
            }
            return ds;
        }

    }
}
