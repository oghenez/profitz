using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class PeriodRepository :Repository
    {
        public PeriodRepository()
            : base(new Period())
        { }
        public Period FindCurrentPeriod()
        {
            OpenConnection();
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = m_connection;
            string hql = String.Format("select * from table_period p where p.period_status ='{0}'", PeriodStatus.Current.ToString());
            cmd.CommandText = hql;
            OdbcDataReader r = cmd.ExecuteReader();
            IList result = Period.TransformReaderList(r);
            r.Close();
            if (result.Count == 0) return null;
            return result[0] as Period;
        }
        public static Period FindPeriodByDate(OdbcCommand cmd, DateTime date)
        {
            string hql = String.Format("select * from table_period p where p.period_start <= '{0}' and p.period_end >= '{0}'", date.ToString(Utils.DATE_FORMAT));
//            OdbcParameter st = new OdbcParameter(":date", date);
            cmd.CommandText = hql;
         //   cmd.Parameters.Add(st);
            OdbcDataReader r = cmd.ExecuteReader();
            IList result = Period.TransformReaderList(r);
            r.Close();
            if (result.Count == 0) return null;
            return result[0] as Period;
        }
        public static Period FindCurrentPeriod(OdbcCommand cmd)
        {
            string hql = String.Format("select * from table_period p where p.period_status ='{0}'", PeriodStatus.Current.ToString() );
            cmd.CommandText = hql;
            OdbcDataReader r = cmd.ExecuteReader();
            IList result = Period.TransformReaderList(r);
            r.Close();
            if (result.Count == 0) return null;
            return result[0] as Period;
        }
        public static Period FindPeriod(OdbcCommand cmd, int id)
        {
            string hql = String.Format("select * from table_period p where p.period_id ='{0}'", id);
            cmd.CommandText = hql;
            OdbcDataReader r = cmd.ExecuteReader();
            IList result = Period.TransformReaderList(r);
            r.Close();
            if (result.Count == 0) return null;
            return result[0] as Period;
        }
        public static Period FindNextPeriod(OdbcCommand cmd)
        {
            Period crnt = PeriodRepository.FindCurrentPeriod(cmd);
            cmd.CommandText = "select * from table_period";
            OdbcDataReader r = cmd.ExecuteReader();
            IList periods = Period.TransformReaderList(r);
            r.Close();
            int crntdx = periods.IndexOf(crnt);
            int next = crntdx + 1;
            if (next > periods.Count)
                return null;
            return periods[next] as Period;
        }
        public Period FindPrevPeriod(OdbcCommand cmd)
        {
            Period crnt = PeriodRepository.FindCurrentPeriod(cmd);
            cmd.CommandText = "select * from table_period";
            OdbcDataReader r = cmd.ExecuteReader();
            IList periods = Period.TransformReaderList(r);
            r.Close();
            int crntdx = periods.IndexOf(crnt);
            int prev = crntdx - 1;
            if (prev < 0)
                return null;
            return periods[prev] as Period;
        }
        public int GetStockCardEntryCount(OdbcCommand cmd)
        {
            string hql = "SELECT COUNT(*) FROM table_stockcardentry";
            cmd.CommandText = hql;
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }
    }
}
