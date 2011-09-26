using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Currency : Entity, IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public Currency()
        {
        }
        public Currency(int id)
        {
            ID = id;
        }
        public Currency(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Currency currency = null;
            while (aReader.Read())
            {
                currency = new Currency();
                currency.ID = Convert.ToInt32(aReader[0]);
                currency.CODE = aReader[1].ToString();
                currency.NAME = aReader[2].ToString();
                currency.MODIFIED_BY = aReader["modified_by"].ToString();
                currency.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                currency.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return currency;
        }
        public static Currency GetCurrency(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Currency currency = null;
            while (aReader.Read())
            {
                currency = new Currency();
                currency.ID = Convert.ToInt32(aReader[0]);
                currency.CODE = aReader[1].ToString();
                currency.NAME = aReader[2].ToString();
                currency.MODIFIED_BY = aReader["modified_by"].ToString();
                currency.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                currency.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return currency;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_currency 
                (ccy_code,ccy_name) 
                VALUES ('{0}','{1}', '{2}', '{3}', '{4}')",
                CODE, NAME, MODIFIED_BY, MODIFIED_DATE.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_currency where ccy_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_currency set 
                ccy_code = '{0}', 
                ccy_name='{1}',
                modified_by='{2}', 
                modified_date='{3}',
                modified_computer='{4}'
                where ccy_id = {5}",
                CODE, NAME, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_currency where ccy_id = {0}", ID);
        }
        public static string GetByIDSQLStatic(int ID)
        {
            return String.Format("select * from table_currency where ccy_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_currency where ccy_code = '{0}'", code);
        }
        public static string GetByCodeSQLStatic(string code)
        {
            return String.Format("select * from table_currency where ccy_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_currency where ccy_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_currency where ccy_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_currency");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_currency p where concat(p.ccy_code, p.ccy_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Currency currency = new Currency();
                currency.ID = Convert.ToInt32(aReader[0]);
                currency.CODE = aReader[1].ToString();
                currency.NAME = aReader[2].ToString();
                currency.MODIFIED_BY = aReader["modified_by"].ToString();
                currency.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                currency.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(currency);
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
            return String.Format("SELECT max(ccy_id) from table_currency");
        }
    }
}
