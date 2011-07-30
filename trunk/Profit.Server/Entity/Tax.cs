using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Tax : IEntity
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
        public IEntity Get(OdbcDataReader aReader)
        {
            Tax tax = null;
            while (aReader.Read())
            {
                tax = new Tax();
                tax.ID = Convert.ToInt32(aReader[0]);
                tax.CODE = aReader[1].ToString();
                tax.NAME = aReader[2].ToString();
                tax.RATE = Convert.ToDouble(aReader[3]);
            }
            return tax;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_tax 
                (tax_code,tax_name,tax_rate) 
                VALUES ('{0}','{1}',{2})",
                CODE, NAME, RATE);
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
                tax_rate={2}
                where tax_id = {3}",
                CODE, NAME, RATE, ID);
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
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Tax tax = new Tax();
                tax.ID = Convert.ToInt32(aReader[0]);
                tax.CODE = aReader[1].ToString();
                tax.NAME = aReader[2].ToString();
                tax.RATE = Convert.ToDouble(aReader[3]);
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
