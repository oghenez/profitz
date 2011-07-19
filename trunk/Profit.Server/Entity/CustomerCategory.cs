using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class CustomerCategory : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public CustomerCategory()
        {
        }
        public CustomerCategory(int id)
        {
            ID = id;
        }
        public CustomerCategory(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(OdbcDataReader aReader)
        {
            CustomerCategory customercategory = null;
            while (aReader.Read())
            {
                customercategory = new CustomerCategory();
                customercategory.ID = Convert.ToInt32(aReader[0]);
                customercategory.CODE = aReader[1].ToString();
                customercategory.NAME = aReader[2].ToString();
            }
            return customercategory;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_customercategory 
                (cuscat_code,cuscat_name) 
                VALUES ('{0}','{1}')",
                CODE, NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_customercategory where cuscat_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_customercategory set 
                cuscat_code = '{0}', 
                cuscat_name='{1}'
                where cuscat_id = {2}",
                CODE, NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_customercategory where cuscat_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_customercategory where cuscat_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_customercategory where cuscat_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_customercategory where cuscat_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_customercategory");
        }
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                CustomerCategory customercategory = new CustomerCategory();
                customercategory.ID = Convert.ToInt32(aReader[0]);
                customercategory.CODE = aReader[1].ToString();
                customercategory.NAME = aReader[2].ToString();
                result.Add(customercategory);
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
            return String.Format("SELECT max(cuscat_id) from table_customercategory");
        }
    }
}
