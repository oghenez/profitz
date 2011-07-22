using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PartSellingPriceInformation : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public Part PART;
        public PriceCategory PRICE_CATEGORY;
        public Currency CURRENCY;
        public Unit UNIT;
        public double SELLING_PRICE = 0;
        public double COST_PRICE = 0;
        public double BOTTOM_PRICE = 0;
        public double DISCOUNT_AMT = 0;
        public double DISCOUNT_PERCENT = 0;

        public PartSellingPriceInformation()
        {
        }
        public PartSellingPriceInformation(int id)
        {
            ID = id;
        }
        public PartSellingPriceInformation(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(OdbcDataReader aReader)
        {
            PartSellingPriceInformation bank = null;
            while (aReader.Read())
            {
                bank = new PartSellingPriceInformation();
                bank.ID = Convert.ToInt32(aReader[0]);
                bank.CODE = aReader[1].ToString();
                bank.NAME = aReader[2].ToString();
            }
            return bank;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_partsellingpriceinformation 
                (prtsellprc_code,prtsellprc_name) 
                VALUES ('{0}','{1}')",
                CODE, NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_partsellingpriceinformation where prtsellprc_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_partsellingpriceinformation set 
                prtsellprc_code = '{0}', 
                prtsellprc_name='{1}'
                where prtsellprc_id = {2}",
                CODE, NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_partsellingpriceinformation where prtsellprc_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_partsellingpriceinformation where prtsellprc_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_partsellingpriceinformation where prtsellprc_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_partsellingpriceinformation where prtsellprc_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_partsellingpriceinformation");
        }
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                PartSellingPriceInformation bank = new PartSellingPriceInformation();
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
            return String.Format("SELECT max(prtsellprc_id) from table_partsellingpriceinformation");
        }
    }
}
