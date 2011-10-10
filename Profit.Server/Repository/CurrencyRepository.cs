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
            }
            if (r == null)
                throw new Exception(string.Format("Rate Currency [{0}] belum di setup", ccy.CODE));
            aReader.Close();
            return r.RATE_TO_BASE * amount;
        }
        public double ConvertToBaseCurrency(Currency ccy, double amount, DateTime date)
        {
            OpenConnection();
            return CurrencyRepository.ConvertToBaseCurrency(m_cmd, ccy, amount, date);
        }
    }
}
