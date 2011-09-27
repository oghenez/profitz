using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PartCategory : Entity, IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public PartCategory()
        {
        }
        public PartCategory(int id)
        {
            ID = id;
        }
        public PartCategory(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            PartCategory prtCat = null;
            while (aReader.Read())
            {
                prtCat = new PartCategory();
                prtCat.ID = Convert.ToInt32(aReader[0]);
                prtCat.CODE = aReader[1].ToString();
                prtCat.NAME = aReader[2].ToString();
                prtCat.MODIFIED_BY = aReader["modified_by"].ToString();
                prtCat.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                prtCat.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return prtCat;
        }
        public static PartCategory GetPartCategory(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            PartCategory prtCat = null;
            while (aReader.Read())
            {
                prtCat = new PartCategory();
                prtCat.ID = Convert.ToInt32(aReader[0]);
                prtCat.CODE = aReader[1].ToString();
                prtCat.NAME = aReader[2].ToString();
                prtCat.MODIFIED_BY = aReader["modified_by"].ToString();
                prtCat.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                prtCat.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return prtCat;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_partcategory 
                (prtcat_code,prtcat_name, modified_by, modified_date, modified_computer) 
                VALUES ('{0}','{1}', '{2}', '{3}', '{4}')",
                CODE, NAME, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_partcategory where prtcat_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_partcategory set 
                prtcat_code = '{0}', 
                prtcat_name='{1}',
                modified_by='{2}', 
                modified_date='{3}',
                modified_computer='{4}'
                where prtcat_id = {5}",
                CODE, NAME, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_partcategory where prtcat_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_partcategory where prtcat_code = '{0}'", code);
        }
        public static string GetByCodeSQLStatic(string code)
        {
            return String.Format("select * from table_partcategory where prtcat_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_partcategory where prtcat_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_partcategory where prtcat_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_partcategory");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_partcategory p where concat(p.prtcat_code, p.prtcat_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                PartCategory prtCat = new PartCategory();
                prtCat.ID = Convert.ToInt32(aReader[0]);
                prtCat.CODE = aReader[1].ToString();
                prtCat.NAME = aReader[2].ToString();
                prtCat.MODIFIED_BY = aReader["modified_by"].ToString();
                prtCat.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                prtCat.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(prtCat);
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
            return String.Format("SELECT max(prtcat_id) from table_partcategory");
        }
    }
}
