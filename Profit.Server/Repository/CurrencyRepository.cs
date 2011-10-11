using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Profit.Server
{
    public class CurrencyRepository : Repository
    {
        public CurrencyRepository()
            : base(new Currency())
        { }
        internal static Currency GetBaseCurrency(MySql.Data.MySqlClient.MySqlCommand m_cmd)
        {
            m_cmd.CommandText = ExchangeRate.GetBaseCCySQL();
            MySql.Data.MySqlClient.MySqlDataReader aReader = m_cmd.ExecuteReader();
            IList result = ExchangeRate.GetAllStatic(aReader);
            if (result.Count == 0) throw new Exception("Base Currency belum di setup");
            ExchangeRate t = (ExchangeRate)result[0];
            aReader.Close();
            return t.CURRENCY;
        }
        internal static double ConvertToBaseCurrency(MySql.Data.MySqlClient.MySqlCommand m_cmd, Currency ccy, double amount, DateTime date)
        {
            m_cmd.CommandText = ExchangeRate.GetByCcyAndDateSQL(ccy.ID, date);
            MySql.Data.MySqlClient.MySqlDataReader aReader = m_cmd.ExecuteReader();
            ExchangeRate r = ExchangeRate.GetExchangeRate(aReader);
            aReader.Close();
            if (r == null)
            {
                m_cmd.CommandText = ExchangeRate.GetByCcyDesc(ccy.ID);
                aReader = m_cmd.ExecuteReader();
                r = ExchangeRate.GetExchangeRate(aReader);
                aReader.Close();
            }
            if (r == null)
                throw new Exception(string.Format("Rate Currency [{0}] belum di setup", ccy.CODE));
            
            return r.RATE_TO_BASE * amount;
        }
        internal static double ConvertToCurrency(MySql.Data.MySqlClient.MySqlCommand m_cmd, Currency fromccy, Currency toccy, double amount, DateTime date)
        {
            m_cmd.CommandText = ExchangeRate.GetByCcyAndDateSQL(fromccy.ID, date);
            MySql.Data.MySqlClient.MySqlDataReader aReader = m_cmd.ExecuteReader();
            ExchangeRate rfr = ExchangeRate.GetExchangeRate(aReader);
            aReader.Close();

            m_cmd.CommandText = ExchangeRate.GetByCcyAndDateSQL(toccy.ID, date);
            aReader = m_cmd.ExecuteReader();
            ExchangeRate rto = ExchangeRate.GetExchangeRate(aReader);
            aReader.Close();

            if (rfr == null)
            {
                m_cmd.CommandText = ExchangeRate.GetByCcyDesc(fromccy.ID);
                aReader = m_cmd.ExecuteReader();
                rfr = ExchangeRate.GetExchangeRate(aReader);
                aReader.Close();
            }
            if (rto == null)
            {
                m_cmd.CommandText = ExchangeRate.GetByCcyDesc(toccy.ID);
                aReader = m_cmd.ExecuteReader();
                rto = ExchangeRate.GetExchangeRate(aReader);
                aReader.Close();
            }
            if (rfr == null)
                throw new Exception(string.Format("From Rate Currency [{0}] belum di setup", fromccy.CODE));
            if (rto == null)
                throw new Exception(string.Format("To Rate Currency [{0}] belum di setup", toccy.CODE));

            return (rfr.RATE_TO_BASE / rto.RATE_TO_BASE) * amount;
        }
        public double ConvertToBaseCurrency(Currency ccy, double amount, DateTime date)
        {
            OpenConnection();
            return CurrencyRepository.ConvertToBaseCurrency(m_cmd, ccy, amount, date);
        }
        public double ConvertToCurrency(Currency frccy, Currency toccy, double amount, DateTime date)
        {
            OpenConnection();
            return CurrencyRepository.ConvertToCurrency(m_cmd,frccy, toccy, amount, date);
        }
    }
}
