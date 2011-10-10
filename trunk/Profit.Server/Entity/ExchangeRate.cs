using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class ExchangeRate : Entity, IEntity
    {
        string DATE_FORMAT = "yyyy/MM/dd";
        public int ID = 0;
        public string CODE = "B001";
        public Currency CURRENCY = new Currency();
        public DateTime START_DATE = DateTime.Today;
        public DateTime END_DATE = DateTime.Today;
        public double RATE_TO_BASE = 0d;

        public ExchangeRate()
        {
        }
        public ExchangeRate(int id)
        {
            ID = id;
        }
        public ExchangeRate(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            ExchangeRate excrate = null;
            while (aReader.Read())
            {
                excrate = new ExchangeRate();
                excrate.ID = Convert.ToInt32(aReader[0]);
                excrate.CODE = aReader[1].ToString();
                excrate.START_DATE = Convert.ToDateTime(aReader[2]);
                excrate.END_DATE = Convert.ToDateTime(aReader[3]);
                excrate.RATE_TO_BASE = Convert.ToDouble(aReader[4]);
                excrate.CURRENCY = new Currency(Convert.ToInt32(aReader[5]));
                excrate.MODIFIED_BY = aReader["modified_by"].ToString();
                excrate.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                excrate.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return excrate;
        }
        public static ExchangeRate GetExchangeRate(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            ExchangeRate excrate = null;
            while (aReader.Read())
            {
                excrate = new ExchangeRate();
                excrate.ID = Convert.ToInt32(aReader[0]);
                excrate.CODE = aReader[1].ToString();
                excrate.START_DATE = Convert.ToDateTime(aReader[2]);
                excrate.END_DATE = Convert.ToDateTime(aReader[3]);
                excrate.RATE_TO_BASE = Convert.ToDouble(aReader[4]);
                excrate.CURRENCY = new Currency(Convert.ToInt32(aReader[5]));
                excrate.MODIFIED_BY = aReader["modified_by"].ToString();
                excrate.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                excrate.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return excrate;
        }
        public string GetInsertSQL()
        {
            if (CURRENCY.ID == 0) throw new Exception("Please select Currency");
            return String.Format(@"insert into table_exchangerate 
                (excrate_code,excrate_start,excrate_end,excrate_rate,ccy_id, modified_by, modified_date, modified_computer) 
                VALUES ('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}')",
                CODE, 
                START_DATE.ToString(DATE_FORMAT),
                END_DATE.ToString(DATE_FORMAT), 
                RATE_TO_BASE,
                CURRENCY.ID, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_exchangerate where excrate_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            if (CURRENCY.ID == 0) throw new Exception("Please select Currency");
            return String.Format(@"update table_exchangerate set 
                excrate_code = '{0}',
                excrate_start= '{1}',
                excrate_end= '{2}',
                excrate_rate= {3},
                ccy_id = {4},
                modified_by='{5}', 
                modified_date='{6}',
                modified_computer='{7}'
                where excrate_id = {8}",
                CODE, 
                START_DATE.ToString(DATE_FORMAT),
                END_DATE.ToString(DATE_FORMAT), 
                RATE_TO_BASE, 
                CURRENCY.ID, MODIFIED_BY , DateTime.Now.ToString(Utils.DATE_FORMAT),MODIFIED_COMPUTER_NAME,
                ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_exchangerate where excrate_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_exchangerate where excrate_code = '{0}'", code);
        }
        public static string GetBaseCCySQL()
        {
            return String.Format("select * from table_exchangerate where excrate_rate = 1");
        }
        public static string GetByCcyAndDateSQL(int ccyID, DateTime date)
        {
            return String.Format(@"select * from table_exchangerate where '{0}' between excrate_start 
            and excrate_end and ccy_id = {1}", date.ToString(Utils.DATE_FORMAT_SHORT), ccyID);
        }
        public static string GetByCcyDesc(int ccyID)
        {
            return String.Format(@"select * from table_exchangerate where ccy_id = {0} order by 
                excrate_end desc", ccyID);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_exchangerate where excrate_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_exchangerate where excrate_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_exchangerate");
        }
        public string GetConcatSearch(string find)
        {
            return "";
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                ExchangeRate excrate = new ExchangeRate();
                excrate.ID = Convert.ToInt32(aReader[0]);
                excrate.CODE = aReader[1].ToString();
                excrate.START_DATE = Convert.ToDateTime(aReader[2]);
                excrate.END_DATE = Convert.ToDateTime(aReader[3]);
                excrate.RATE_TO_BASE = Convert.ToDouble(aReader[4]);
                excrate.CURRENCY = new Currency(Convert.ToInt32(aReader[5]));
                excrate.MODIFIED_BY = aReader["modified_by"].ToString();
                excrate.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                excrate.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(excrate);
            }
            return result;
        }
        public static IList GetAllStatic(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                ExchangeRate excrate = new ExchangeRate();
                excrate.ID = Convert.ToInt32(aReader[0]);
                excrate.CODE = aReader[1].ToString();
                excrate.START_DATE = Convert.ToDateTime(aReader[2]);
                excrate.END_DATE = Convert.ToDateTime(aReader[3]);
                excrate.RATE_TO_BASE = Convert.ToDouble(aReader[4]);
                excrate.CURRENCY = new Currency(Convert.ToInt32(aReader[5]));
                excrate.MODIFIED_BY = aReader["modified_by"].ToString();
                excrate.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                excrate.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(excrate);
            }
            return result;
        }
        public int GetID()
        {
            return ID;
        }
        public void SetID(int id)
        {
            ID = id;
        }
        public override string ToString()
        {
            return CODE;
        }
        public string GetCode()
        {
            return CODE;
        }
        public void SetCode(string code)
        {
            CODE = code;
        }
        public string GetMaximumIDSQL()
        {
            return String.Format("SELECT max(excrate_id) from table_exchangerate");
        }
    }
}
