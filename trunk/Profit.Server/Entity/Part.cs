using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;
using System.Data;

namespace Profit.Server
{
    public class Part : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public string BARCODE = "";
        public PartGroup PART_GROUP = new PartGroup();
        public Unit UNIT = new Unit();
        public Currency CURRENCY = new Currency();
        public CostMethod COST_METHOD = CostMethod.MovingAverage;
        public PartCategory PART_CATEGORY = new PartCategory();
        public PriceCategory PRICE_CATEGORY = null;
        public double MINIMUM_STOCK = 0;
        public double MAXIMUM_STOCK = 0;
        public double COST_PRICE = 0;
        public double SELL_PRICE = 0;
        public double CURRENT_STOCK = 0;
        public bool TAXABLE = false;
        public Tax TAX = null;
        public bool ACTIVE = true;
        public IList UNIT_CONVERSION_LIST = new ArrayList();
        public IDictionary SELLING_PRICE_INFO_LIST = new Hashtable();
        public byte[] PICTURE;
        public string PICTURE_NAME = "";

        public Unit UNIT_BY_SEARCH = null;// for searching unit conversion
        public double SELL_PRICE_BY_SEARCH = 0;
        public double COST_PRICE_BY_SEARCH = 0;
        public double NEW_SELL_PRICE = 0;//update mark up down
        public Part()
        {
        }
        public Part(int id)
        {
            ID = id;
        }
        public Part(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            Part part = null;
            while (r.Read())
            {
                part = new Part();
                part.ID = Convert.ToInt32(r[0]);
                part.CODE = r["part_code"].ToString();
                part.NAME = r["part_name"].ToString();
                part.ACTIVE = Convert.ToBoolean(r["part_active"]);
                part.BARCODE = r["part_barcode"].ToString();
                part.COST_METHOD = (CostMethod)Enum.Parse(typeof(CostMethod),r["part_costmethod"].ToString());
                part.COST_PRICE = Convert.ToDouble(r["part_costprice"]);
                part.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                part.CURRENT_STOCK = Convert.ToDouble(r["part_currentstock"]);
                part.MAXIMUM_STOCK = Convert.ToDouble(r["part_maximumstock"]);
                part.MINIMUM_STOCK = Convert.ToDouble(r["part_minimumstock"]);
                part.PART_CATEGORY = new PartCategory(Convert.ToInt32(r["prtcat_id"]));
                part.PART_GROUP = new PartGroup(Convert.ToInt32(r["prtgroup_id"]));
                part.SELL_PRICE = Convert.ToDouble(r["part_sellprice"]);
                part.TAXABLE = Convert.ToBoolean(r["part_taxable"]);
                part.UNIT = new Unit(Convert.ToInt32(r["unit_id"]));
                part.PICTURE_NAME = r["part_picture"].ToString();
                part.TAX = new Tax(Convert.ToInt32(r["tax_id"]));
                part.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(r["pricecat_id"]));
             }
            return part;
        }
        public static Part GetPart(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            Part part = null;
            while (r.Read())
            {
                part = new Part();
                part.ID = Convert.ToInt32(r[0]);
                part.CODE = r["part_code"].ToString();
                part.NAME = r["part_name"].ToString();
                part.ACTIVE = Convert.ToBoolean(r["part_active"]);
                part.BARCODE = r["part_barcode"].ToString();
                part.COST_METHOD = (CostMethod)Enum.Parse(typeof(CostMethod), r["part_costmethod"].ToString());
                part.COST_PRICE = Convert.ToDouble(r["part_costprice"]);
                part.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                part.CURRENT_STOCK = Convert.ToDouble(r["part_currentstock"]);
                part.MAXIMUM_STOCK = Convert.ToDouble(r["part_maximumstock"]);
                part.MINIMUM_STOCK = Convert.ToDouble(r["part_minimumstock"]);
                part.PART_CATEGORY = new PartCategory(Convert.ToInt32(r["prtcat_id"]));
                part.PART_GROUP = new PartGroup(Convert.ToInt32(r["prtgroup_id"]));
                part.SELL_PRICE = Convert.ToDouble(r["part_sellprice"]);
                part.TAXABLE = Convert.ToBoolean(r["part_taxable"]);
                part.UNIT = new Unit(Convert.ToInt32(r["unit_id"]));
                part.PICTURE_NAME = r["part_picture"].ToString();
                part.TAX = new Tax(Convert.ToInt32(r["tax_id"]));
                part.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(r["pricecat_id"]));
            }
            return part;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_part 
                (   part_code, 
                    part_name,
                    part_active,
                    part_barcode,
                    part_costmethod,
                    part_costprice,
                    ccy_id,
                    part_currentstock,
                    part_maximumstock,
                    part_minimumstock,
                    prtcat_id,
                    prtgroup_id,
                    part_sellprice,
                    part_taxable,
                    unit_id,
                    part_picture,
                    tax_id,
                    pricecat_id) 
                VALUES (
                    '{0}',
                    '{1}',
                    {2},
                    '{3}',
                    '{4}',
                    '{5}',
                    '{6}',
                    '{7}',
                    '{8}',
                    '{9}',
                    '{10}',
                    '{11}',
                    '{12}',
                    {13},
                    '{14}',
                    '{15}',
                    {16},
                    {17}
                    )",
                CODE, 
                NAME,
                ACTIVE,
                BARCODE,
                COST_METHOD.ToString(),
                COST_PRICE,
                CURRENCY.ID,
                CURRENT_STOCK,
                MAXIMUM_STOCK,
                MINIMUM_STOCK,
                PART_CATEGORY.ID,
                PART_GROUP.ID,
                SELL_PRICE,
                TAXABLE,
                UNIT.ID,
                CODE,
                TAX==null?0:TAX.ID,
                PRICE_CATEGORY==null?0:PRICE_CATEGORY.ID
                );
        }
        public string GetDeleteSQL()
        {
            return "delete from table_part where part_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_part set 
                part_code = '{0}', 
                part_name='{1}',
                part_active={2},
                part_barcode='{3}',
                part_costmethod='{4}',
                part_costprice='{5}',
                ccy_id='{6}',
                part_currentstock='{7}',
                part_maximumstock='{8}',
                part_minimumstock='{9}',
                prtcat_id='{10}',
                prtgroup_id='{11}',
                part_sellprice='{12}',
                part_taxable={13},
                unit_id='{14}',
                part_picture = '{15}',
                tax_id = {16},
                pricecat_id = {17}
                where part_id = {18}",
                CODE, NAME, ACTIVE,
                BARCODE,
                COST_METHOD.ToString(),
                COST_PRICE,
                CURRENCY.ID,
                CURRENT_STOCK,
                MAXIMUM_STOCK,
                MINIMUM_STOCK,
                PART_CATEGORY.ID,
                PART_GROUP.ID,
                SELL_PRICE,
                TAXABLE,
                UNIT.ID,
                CODE,
                TAX==null?0:TAX.ID,
                PRICE_CATEGORY==null?0:PRICE_CATEGORY.ID,
                ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_part where part_id = {0}", ID);
        }
        public static string GetByIDSQLStatic(int ID)
        {
            return String.Format("select * from table_part where part_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_part where part_code = '{0}'", code);
        }
        public static string GetByCodeSQLStatic(string code)
        {
            return String.Format("select * from table_part where part_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_part where part_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_part where part_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_part");
        }
        public static string GetSearchSQL(string search, bool active)
        {
           // return String.Format(@"select * from table_part p where part_active = {0} 
           // and concat(p.part_code, p.part_name, p.part_barcode) like '%{1}%'", true, search);
            return String.Format(@"select distinct(p.part_id),p.* from table_part p, table_unitconversion v where {0}
             concat(p.part_code, p.part_name, p.part_barcode, v.unitconv_barcode) like '%{1}%'
            and v.part_id = p.part_id", active ? "p.part_active = true and" : "", search);
        }
        public static string GetSearchBarcodeOnlySQL(string search, bool active)
        {
            // return String.Format(@"select * from table_part p where part_active = {0} 
            // and concat(p.part_code, p.part_name, p.part_barcode) like '%{1}%'", true, search);
            return String.Format(@"select distinct(p.part_id),p.* from table_part p, table_unitconversion v where {0}
             v.unitconv_barcode like '%{1}%'
            and v.part_id = p.part_id", active ? "p.part_active = true and" : "", search);
        }

        public string GetConcatSearch(string find)
        {
            return String.Format(@"select * from table_part p where concat(p.part_code, p.part_name, p.part_barcode) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            IList result = new ArrayList();
            while (r.Read())
            {
                Part part = new Part();
                part.ID = Convert.ToInt32(r[0]);
                part.CODE = r["part_code"].ToString();
                part.NAME = r["part_name"].ToString();
                part.ACTIVE = Convert.ToBoolean(r["part_active"]);
                part.BARCODE = r["part_barcode"].ToString();
                part.COST_METHOD = (CostMethod)Enum.Parse(typeof(CostMethod), r["part_costmethod"].ToString());
                part.COST_PRICE = Convert.ToDouble(r["part_costprice"]);
                part.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                part.CURRENT_STOCK = Convert.ToDouble(r["part_currentstock"]);
                part.MAXIMUM_STOCK = Convert.ToDouble(r["part_maximumstock"]);
                part.MINIMUM_STOCK = Convert.ToDouble(r["part_minimumstock"]);
                part.PART_CATEGORY = new PartCategory(Convert.ToInt32(r["prtcat_id"]));
                part.PART_GROUP = new PartGroup(Convert.ToInt32(r["prtgroup_id"]));
                part.SELL_PRICE = Convert.ToDouble(r["part_sellprice"]);
                part.TAXABLE = Convert.ToBoolean(r["part_taxable"]);
                part.UNIT = new Unit(Convert.ToInt32(r["unit_id"]));
                part.PICTURE_NAME = r["part_picture"].ToString();
                part.TAX = new Tax(Convert.ToInt32(r["tax_id"]));
                part.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(r["pricecat_id"]));
                result.Add(part);
            }
            return result;
        }
        public static IList GetAllStatic(MySql.Data.MySqlClient.MySqlDataReader r)
        {
            IList result = new ArrayList();
            while (r.Read())
            {
                Part part = new Part();
                part.ID = Convert.ToInt32(r[0]);
                part.CODE = r["part_code"].ToString();
                part.NAME = r["part_name"].ToString();
                part.ACTIVE = Convert.ToBoolean(r["part_active"]);
                part.BARCODE = r["part_barcode"].ToString();
                part.COST_METHOD = (CostMethod)Enum.Parse(typeof(CostMethod), r["part_costmethod"].ToString());
                part.COST_PRICE = Convert.ToDouble(r["part_costprice"]);
                part.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                part.CURRENT_STOCK = Convert.ToDouble(r["part_currentstock"]);
                part.MAXIMUM_STOCK = Convert.ToDouble(r["part_maximumstock"]);
                part.MINIMUM_STOCK = Convert.ToDouble(r["part_minimumstock"]);
                part.PART_CATEGORY = new PartCategory(Convert.ToInt32(r["prtcat_id"]));
                part.PART_GROUP = new PartGroup(Convert.ToInt32(r["prtgroup_id"]));
                part.SELL_PRICE = Convert.ToDouble(r["part_sellprice"]);
                part.TAXABLE = Convert.ToBoolean(r["part_taxable"]);
                part.UNIT = new Unit(Convert.ToInt32(r["unit_id"]));
                part.PICTURE_NAME = r["part_picture"].ToString();
                part.TAX = new Tax(Convert.ToInt32(r["tax_id"]));
                part.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(r["pricecat_id"]));
                part.UNIT_BY_SEARCH = part.UNIT;//pos control
                part.SELL_PRICE_BY_SEARCH = part.SELL_PRICE;//pos control
                result.Add(part);
            }
            return result;
        }
        public static IList GetAllStatic(DataSet ds)
        {
            IList result = new ArrayList();
            DataTable dt = ds.Tables[0];
            foreach (DataRow r in dt.Rows)
            {
                Part part = new Part();
                part.ID = Convert.ToInt32(r[0]);
                part.CODE = r["part_code"].ToString();
                part.NAME = r["part_name"].ToString();
                part.ACTIVE = Convert.ToBoolean(r["part_active"]);
                part.BARCODE = r["part_barcode"].ToString();
                part.COST_METHOD = (CostMethod)Enum.Parse(typeof(CostMethod), r["part_costmethod"].ToString());
                part.COST_PRICE = Convert.ToDouble(r["part_costprice"]);
                part.CURRENCY = new Currency(Convert.ToInt32(r["ccy_id"]));
                part.CURRENT_STOCK = Convert.ToDouble(r["part_currentstock"]);
                part.MAXIMUM_STOCK = Convert.ToDouble(r["part_maximumstock"]);
                part.MINIMUM_STOCK = Convert.ToDouble(r["part_minimumstock"]);
                part.PART_CATEGORY = new PartCategory(Convert.ToInt32(r["prtcat_id"]));
                part.PART_GROUP = new PartGroup(Convert.ToInt32(r["prtgroup_id"]));
                part.SELL_PRICE = Convert.ToDouble(r["part_sellprice"]);
                part.TAXABLE = Convert.ToBoolean(r["part_taxable"]);
                part.UNIT = new Unit(Convert.ToInt32(r["unit_id"]));
                part.PICTURE_NAME = r["part_picture"].ToString();
                part.TAX = new Tax(Convert.ToInt32(r["tax_id"]));
                part.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(r["pricecat_id"]));
                result.Add(part);
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
            return String.Format("SELECT max(part_id) from table_part");
        }
        public UnitConversion GetUnitConversion(long unitId)
        {
            foreach (UnitConversion u in UNIT_CONVERSION_LIST)
            {
                if (u.CONVERSION_UNIT.ID == unitId)
                    return u;
            }
            return null;
        }
        public static string FindPartBy(bool prtGroup, int prtGroupID, bool prtCat, int prtCatID, bool part, int partID)
        {
            string sql =
                String.Format(@"select * from table_part where part_active = True {0} {1} {2}",
                prtGroup ? " and prtgroup_id = " + prtGroupID : "", prtCat ? " and prtcat_id =" + prtCatID : "", part ? " and part_id =" + partID : "");
            return  sql;
        }

        internal string updateSellingPrice()
        {
            return "update table_part set part_sellprice = " + this.NEW_SELL_PRICE;
        }
    }
}
