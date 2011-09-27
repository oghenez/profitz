using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Tax : Entity,IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public double RATE = 0;

        public Tax()
        {
        }
        public Tax(int id)
        {
            ID = id;
        }
        public Tax(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Tax tax = null;
            while (aReader.Read())
            {
                tax = new Tax();
                tax.ID = Convert.ToInt32(aReader[0]);
                tax.CODE = aReader[1].ToString();
                tax.NAME = aReader[2].ToString();
                tax.RATE = Convert.ToDouble(aReader[3]);
                tax.MODIFIED_BY = aReader["modified_by"].ToString();
                tax.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                tax.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return tax;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_tax 
                (tax_code,tax_name,tax_rate, modified_by, modified_date, modified_computer) 
                VALUES ('{0}','{1}',{2},'{3}','{4}','{5}')",
                CODE, NAME, RATE, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_tax where tax_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_tax set 
                tax_code = '{0}', 
                tax_name='{1}',
                tax_rate={2},
                modified_by='{3}', 
                modified_date='{4}',
                modified_computer='{5}'
                where tax_id = {6}",
                CODE, NAME, RATE, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_tax where tax_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_tax where tax_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_tax where tax_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_tax where tax_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_tax");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_tax p where concat(p.tax_code, p.tax_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Tax tax = new Tax();
                tax.ID = Convert.ToInt32(aReader[0]);
                tax.CODE = aReader[1].ToString();
                tax.NAME = aReader[2].ToString();
                tax.RATE = Convert.ToDouble(aReader[3]);
                tax.MODIFIED_BY = aReader["modified_by"].ToString();
                tax.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                tax.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(tax);
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
            return String.Format("SELECT max(tax_id) from table_tax");
        }
    }
}
