using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Currency : IEntity
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
        public IEntity Get(OdbcDataReader aReader)
        {
            Currency currency = null;
            while (aReader.Read())
            {
                currency = new Currency();
                currency.ID = Convert.ToInt32(aReader[0]);
                currency.CODE = aReader[1].ToString();
                currency.NAME = aReader[2].ToString();
            }
            return currency;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_currency 
                (ccy_code,ccy_name) 
                VALUES ('{0}','{1}')",
                CODE, NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_currency where ccy_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_currency set 
                ccy_code = '{0}', 
                ccy_name='{1}'
                where ccy_id = {2}",
                CODE, NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_currency where ccy_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
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
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Currency currency = new Currency();
                currency.ID = Convert.ToInt32(aReader[0]);
                currency.CODE = aReader[1].ToString();
                currency.NAME = aReader[2].ToString();
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
