using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class StockCardRepository : Repository
    {
        public StockCardRepository():base(new StockCard())
        {

        }
        public static void Update(MySql.Data.MySqlClient.MySqlCommand cmd, StockCard sc)
        {
            cmd.CommandText = sc.GetUpdateSQL();
            cmd.ExecuteNonQuery();
            foreach (StockCardEntry sce in sc.STOCK_CARD_ENTRIES)
            {
                if (sce.ID == 0)
                {
                    cmd.CommandText = sce.GetInsertSQL();
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = StockCardEntry.SelectMaxIDSQL();
                    sce.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else 
                {
                    cmd.CommandText = sce.GetUpdateSQL();
                    cmd.ExecuteNonQuery();
                }
            }
            cmd.CommandText = StockCardEntry.FindByStockCard(sc.ID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            IList sces = StockCardEntry.TransformReaderList(r);
            r.Close();
            foreach (StockCardEntry sce in sces)
            {
                sce.UPDATED = sc.STOCK_CARD_ENTRIES.Contains(sce);
            }
            foreach (StockCardEntry sce in sces)
            {
                if (!sce.UPDATED)
                {
                    cmd.CommandText = StockCardEntry.DeleteSQL(sce.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateHeader(MySql.Data.MySqlClient.MySqlCommand cmd, StockCard sc)
        {
            cmd.CommandText = sc.GetUpdateSQL();
            cmd.ExecuteNonQuery();
        }
        public static void Save(MySql.Data.MySqlClient.MySqlCommand cmd, StockCard sc)
        {
            cmd.CommandText = sc.GetInsertSQL();
            cmd.ExecuteNonQuery();
            cmd.CommandText = StockCard.SelectMaxIDSQL();
            sc.ID = Convert.ToInt32(cmd.ExecuteScalar());
            foreach (StockCardEntry sce in sc.STOCK_CARD_ENTRIES)
            {
                cmd.CommandText = sce.GetInsertSQL();
                cmd.ExecuteNonQuery();
                cmd.CommandText = StockCardEntry.SelectMaxIDSQL();
                sce.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public static void SaveHeader(MySql.Data.MySqlClient.MySqlCommand cmd, StockCard sc)
        {
            cmd.CommandText = sc.GetInsertSQL();
            cmd.ExecuteNonQuery();
            cmd.CommandText = StockCard.SelectMaxIDSQL();
            sc.ID = Convert.ToInt32(cmd.ExecuteScalar());
        }
        public static void DeleteHeader(MySql.Data.MySqlClient.MySqlCommand cmd, StockCard sc)
        {
            cmd.CommandText = sc.GetDeleteSQL();
            cmd.ExecuteNonQuery();
        }
        public static StockCard FindStockCard(MySql.Data.MySqlClient.MySqlCommand cmd, long partId, long locationId, long periodId)
        {
            cmd.CommandText = String.Format("select * from table_stockcard where part_id = {0} and warehouse_id = {1} and period_id = {2}", partId, locationId, periodId);
            MySql.Data.MySqlClient.MySqlDataReader r =  cmd.ExecuteReader();
            StockCard sc = StockCard.TransformReader(r);
            r.Close();
            if (sc != null)
            {
                sc.PERIOD = PeriodRepository.FindPeriod(cmd, sc.PERIOD.ID);
                cmd.CommandText = StockCardEntry.FindByStockCard(sc.ID);
                MySql.Data.MySqlClient.MySqlDataReader rx = cmd.ExecuteReader();
                sc.STOCK_CARD_ENTRIES = StockCardEntry.TransformReaderList(rx);
                rx.Close();
            }
            return sc;
        }
        public static StockCard FindStockCardHeader(MySql.Data.MySqlClient.MySqlCommand cmd, long partId, 
            long locationId, Period periodId, DateTime trDate)
        {
            cmd.CommandText = String.Format("select * from table_stockcard where part_id = {0} and warehouse_id = {1} and period_id = {2}", partId, locationId, periodId.ID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            StockCard sc = StockCard.TransformReader(r);
            r.Close();
            if (sc != null)
            {
                sc.PERIOD = PeriodRepository.FindPeriod(cmd, sc.PERIOD.ID);
                sc.WAREHOUSE = StockCardRepository.FindWarehouse(cmd, sc.WAREHOUSE.ID);

                cmd.CommandText = StockCardEntry.FindByStockCard(sc.ID, periodId.START_DATE, trDate);
                MySql.Data.MySqlClient.MySqlDataReader rx = cmd.ExecuteReader();
                sc.STOCK_CARD_ENTRIES = StockCardEntry.TransformReaderList(rx);
                rx.Close();
                foreach (StockCardEntry e in sc.STOCK_CARD_ENTRIES)
                {
                    if(e.STOCK_CARD_ENTRY_TYPE == StockCardEntryType.SupplierInvoice)
                    {
                        cmd.CommandText = SupplierInvoiceItem.GetByIDSQL(e.EVENT_ITEM.ID);
                        rx = cmd.ExecuteReader();
                        e.EVENT_ITEM = SupplierInvoiceItem.TransformReader(rx);
                        rx.Close();
                    }
                    if (e.STOCK_CARD_ENTRY_TYPE == StockCardEntryType.CustomerInvoice)
                    {
                        cmd.CommandText = CustomerInvoiceItem.GetByIDSQL(e.EVENT_ITEM.ID);
                        rx = cmd.ExecuteReader();
                        e.EVENT_ITEM = CustomerInvoiceItem.TransformReader(rx);
                        rx.Close();
                    }
                }
                
            }
            return sc;
        }
        //public static StockCard FindStockCard(MySql.Data.MySqlClient.MySqlCommand cmd, long periodId)
        //{
        //    cmd.CommandText = String.Format("select * from table_stockcard where period_id = {0}", periodId);
        //    MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
        //    StockCard sc = StockCard.TransformReader(r);
        //    r.Close();
        //    return sc;
        //}
        public static IList FindStockCardByPeriod(MySql.Data.MySqlClient.MySqlCommand cmd, long periodId)
        {
            cmd.CommandText = String.Format("select * from table_stockcard where period_id = {0}", periodId);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            IList sc = StockCard.TransforReaderList(r);
            r.Close();
            return sc;
        }
        public static Warehouse FindWarehouse(MySql.Data.MySqlClient.MySqlCommand cmd, int wareHOuseID)
        {
            cmd.CommandText = Warehouse.GetByIDSQLStatic(wareHOuseID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            Warehouse sc = Warehouse.GetWarehouse(r);
            r.Close();
            return sc;
        }
        public static Unit FindUnit(MySql.Data.MySqlClient.MySqlCommand cmd, int unitID)
        {
            cmd.CommandText = Unit.GetByIDSQLstatic(unitID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            Unit sc = Unit.GetUnit(r);
            r.Close();
            return sc;
        }
    }
}
