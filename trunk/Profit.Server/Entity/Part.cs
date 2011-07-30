using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

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
        public double MINIMUM_STOCK = 0;
        public double MAXIMUM_STOCK = 0;
        public double COST_PRICE = 0;
        public double SELL_PRICE = 0;
        public double CURRENT_STOCK = 0;
        public bool TAXABLE = false;
        public bool ACTIVE = true;
        public IList UNIT_CONVERSION_LIST = new ArrayList();
        public IDictionary SELLING_PRICE_INFO_LIST = new Hashtable();

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
        public IEntity Get(OdbcDataReader r)
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
            }
            return part;
        }
        public static Part GetPart(OdbcDataReader r)
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
                    unit_id) 
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
                    '{14}'
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
                UNIT.ID
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
                unit_id='{14}'
                where part_id = {15}",
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
                UNIT.ID,ID);
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
        public static string GetSearchSQL(string search)
        {
            return String.Format(@"select * from table_part p where concat(p.part_code, p.part_name, p.part_barcode) like '%{0}%'", search);
        }

        public string GetConcatSearch(string find)
        {
            return String.Format(@"select * from table_part p where concat(p.part_code, p.part_name, p.part_barcode) like '%{0}%'", find);
        }
        public IList GetAll(OdbcDataReader r)
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
                result.Add(part);
            }
            return result;
        }
        public static IList GetAllStatic(OdbcDataReader r)
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

    }
}
