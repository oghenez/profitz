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
        public static IList FindStockCardEntryInfoPart(OdbcCommand cmd)
        {
            //string hql = String.Format("SELECT new PartDTO(p.Id, p.Code, p.Name) FROM {0} p", GetSubjectType().ToString());
           // IList result = HibernateTemplate.Find(hql);
            return null; ;
        }
        public static void Delete(OdbcCommand cmd, StockCardEntry sc)
        {

        }
    }
}
