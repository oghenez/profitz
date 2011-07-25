using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace Profit.Server
{
    public class StockCardRepository
    {
        public static void Update(OdbcCommand cmd, StockCard sc)
        {
 
        }
        public static void Save(OdbcCommand cmd, StockCard sc)
        {

        }
      
        public static StockCard FindStockCard(OdbcCommand cmd, long partId, long locationId, long periodId)
        {
            cmd.CommandText = String.Format("select * from table_stockcard where part_id = {0} and warehouse_id = {1} and period_id = {2}", partId, locationId, periodId);
            OdbcDataReader r =  cmd.ExecuteReader();
            StockCard sc = StockCard.TransformReader(r);
            r.Close();
            return sc;
        }
        public static StockCard FindStockCard(OdbcCommand cmd, long periodId)
        {
            cmd.CommandText = String.Format("select * from table_stockcard where period_id = {0}", periodId);
            OdbcDataReader r = cmd.ExecuteReader();
            StockCard sc = StockCard.TransformReader(r);
            r.Close();
            return sc;
        }
    }
}
