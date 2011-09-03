using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class VendorBalanceEntryRepository
    {
        public static IList FindVendorBalanceEntryInfoPart(MySql.Data.MySqlClient.MySqlCommand cmd)
        {
            //string hql = String.Format("SELECT new PartDTO(p.Id, p.Code, p.Name) FROM {0} p", GetSubjectType().ToString());
           // IList result = HibernateTemplate.Find(hql);
            return null; ;
        }
        public static void Delete(MySql.Data.MySqlClient.MySqlCommand cmd, VendorBalanceEntry sc)
        {
            cmd.CommandText = VendorBalanceEntry.DeleteSQL(sc.ID);
            cmd.ExecuteNonQuery();
        }
        public static VendorBalanceEntry FindVendorBalanceEntryByEventItem(MySql.Data.MySqlClient.MySqlCommand cmd, int itemID, VendorBalanceEntryType scetype)
        {
            cmd.CommandText = VendorBalanceEntry.FindByEventJournalItem(itemID, scetype);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            VendorBalanceEntry res = VendorBalanceEntry.TransformReader(r);
            r.Close();
            return res;
        }
        public static void Save(MySql.Data.MySqlClient.MySqlCommand cmd, VendorBalanceEntry sce)
        {
            if (sce.ID == 0)
            {
                cmd.CommandText = sce.GetInsertSQL();
                cmd.ExecuteNonQuery();
                cmd.CommandText = VendorBalanceEntry.SelectMaxIDSQL();
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
