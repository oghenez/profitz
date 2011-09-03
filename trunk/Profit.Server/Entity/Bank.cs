using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Bank : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public Bank()
        {
        }
        public Bank(int id)
        {
            ID = id;
        }
        public Bank(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Bank bank = null;
            while (aReader.Read())
            {
                bank = new Bank();
                bank.ID = Convert.ToInt32(aReader[0]);
                bank.CODE = aReader[1].ToString();
                bank.NAME = aReader[2].ToString();
            }
            return bank;
        }
        public static Bank GetBank(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Bank bank = null;
            while (aReader.Read())
            {
                bank = new Bank();
                bank.ID = Convert.ToInt32(aReader[0]);
                bank.CODE = aReader[1].ToString();
                bank.NAME = aReader[2].ToString();
            }
            return bank;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_bank 
                (bank_code,bank_name) 
                VALUES ('{0}','{1}')",
                CODE, NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_bank where bank_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_bank set 
                bank_code = '{0}', 
                bank_name='{1}'
                where bank_id = {2}",
                CODE, NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_bank where bank_id = {0}", ID);
        }
        public static string GetByIDSQLStatic(int ID)
        {
            return String.Format("select * from table_bank where bank_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_bank where bank_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_bank where bank_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_bank where bank_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_bank");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_bank p where concat(p.bank_code, p.bank_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Bank bank = new Bank();
                bank.ID = Convert.ToInt32(aReader[0]);
                bank.CODE = aReader[1].ToString();
                bank.NAME = aReader[2].ToString();
                result.Add(bank);
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
            return String.Format("SELECT max(bank_id) from table_bank");
        }
    }
}
