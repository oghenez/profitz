using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PriceCategory : Entity, IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public double DISCOUNT_PERCENT = 0;
        public double DISCOUNT_AMOUNT = 0; 

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
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            PriceCategory pricecategory = null;
            while (aReader.Read())
            {
                pricecategory = new PriceCategory();
                pricecategory.ID = Convert.ToInt32(aReader[0]);
                pricecategory.CODE = aReader[1].ToString();
                pricecategory.NAME = aReader[2].ToString();
                pricecategory.DISCOUNT_PERCENT = Convert.ToDouble(aReader[3]);
                pricecategory.DISCOUNT_AMOUNT = Convert.ToDouble(aReader[4]);
                pricecategory.MODIFIED_BY = aReader["modified_by"].ToString();
                pricecategory.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                pricecategory.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return pricecategory;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_pricecategory 
                (pricecat_code,pricecat_name,pricecat_disc,pricecat_amount, modified_by, modified_date, modified_computer) 
                VALUES ('{0}','{1}',{2},{3},'{4}','{5}','{6}')",
                CODE, NAME, DISCOUNT_PERCENT, DISCOUNT_AMOUNT, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_pricecategory where pricecat_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_pricecategory set 
                pricecat_code = '{0}', 
                pricecat_name='{1}',
                pricecat_disc = {2},
                pricecat_amount = {3},
                modified_by='{4}', 
                modified_date='{5}',
                modified_computer='{6}'
                where pricecat_id = {7}",
                CODE, NAME, DISCOUNT_PERCENT, DISCOUNT_AMOUNT, 
                MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME, ID);
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
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                PriceCategory pricecategory = new PriceCategory();
                pricecategory.ID = Convert.ToInt32(aReader[0]);
                pricecategory.CODE = aReader[1].ToString();
                pricecategory.NAME = aReader[2].ToString();
                pricecategory.DISCOUNT_PERCENT = Convert.ToDouble(aReader[3]);
                pricecategory.DISCOUNT_AMOUNT = Convert.ToDouble(aReader[4]);
                pricecategory.MODIFIED_BY = aReader["modified_by"].ToString();
                pricecategory.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                pricecategory.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
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
