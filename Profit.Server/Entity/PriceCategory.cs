using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PriceCategory : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public PriceCategory()
        {
        }
        public PriceCategory(int id)
        {
            ID = id;
        }
        public PriceCategory(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(OdbcDataReader aReader)
        {
            PriceCategory pricecategory = null;
            while (aReader.Read())
            {
                pricecategory = new PriceCategory();
                pricecategory.ID = Convert.ToInt32(aReader[0]);
                pricecategory.CODE = aReader[1].ToString();
                pricecategory.NAME = aReader[2].ToString();
            }
            return pricecategory;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_pricecategory 
                (pricecat_code,pricecat_name) 
                VALUES ('{0}','{1}')",
                CODE, NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_pricecategory where pricecat_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_pricecategory set 
                pricecat_code = '{0}', 
                pricecat_name='{1}'
                where pricecat_id = {2}",
                CODE, NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_pricecategory where pricecat_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_pricecategory where pricecat_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_pricecategory where pricecat_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_pricecategory where pricecat_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_pricecategory");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_pricecategory p where concat(p.pricecat_code, p.pricecat_name) like '%{0}%'", find);
        }
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                PriceCategory pricecategory = new PriceCategory();
                pricecategory.ID = Convert.ToInt32(aReader[0]);
                pricecategory.CODE = aReader[1].ToString();
                pricecategory.NAME = aReader[2].ToString();
                result.Add(pricecategory);
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
            return String.Format("SELECT max(pricecat_id) from table_pricecategory");
        }
    }
}
