using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class StockCardEntryRepository
    {
        public static IList FindStockCardEntryInfoPart(MySql.Data.MySqlClient.MySqlCommand cmd)
        {
            //string hql = String.Format("SELECT new PartDTO(p.Id, p.Code, p.Name) FROM {0} p", GetSubjectType().ToString());
           // IList result = HibernateTemplate.Find(hql);
            return null; ;
        }
        public static void Delete(MySql.Data.MySqlClient.MySqlCommand cmd, StockCardEntry sc)
        {
            cmd.CommandText = StockCardEntry.DeleteSQL(sc.ID);
            cmd.ExecuteNonQuery();
        }
        public static StockCardEntry FindStockCardEntryByEventItem(MySql.Data.MySqlClient.MySqlCommand cmd, int itemID, StockCardEntryType scetype)
        {
            cmd.CommandText = StockCardEntry.FindByEventItem(itemID, scetype);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            StockCardEntry res = StockCardEntry.TransformReader(r);
            r.Close();
            return res;
        }
        public static void Save(MySql.Data.MySqlClient.MySqlCommand cmd, StockCardEntry sce)
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
    }
}
