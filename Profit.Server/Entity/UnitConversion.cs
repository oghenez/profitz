﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class UnitConversion : IEntity
    {
        public bool UPDATED = false; // for updating part
        public int ID = 0;
        public string CODE = "-";
        public string NAME = "-";

        public double CONVERSION_QTY = 1;
        public Unit CONVERSION_UNIT;
        public double ORIGINAL_QTY = 1;
        public double COST_PRICE;
        public double SELL_PRICE;
        public string BARCODE = "";
        public Part PART;

        public UnitConversion()
        {
        }
        public UnitConversion(int id)
        {
            ID = id;
        }
        public UnitConversion(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            UnitConversion unitConv = null;
            while (aReader.Read())
            {
                unitConv = new UnitConversion();
                unitConv.ID = Convert.ToInt32(aReader[0]);
                unitConv.CODE = aReader[1].ToString();
                unitConv.NAME = aReader[2].ToString();
                unitConv.CONVERSION_QTY = Convert.ToDouble(aReader[3]);
                unitConv.CONVERSION_UNIT = new Unit(Convert.ToInt32(aReader[4]));
                unitConv.COST_PRICE = Convert.ToDouble(aReader[5]);
                unitConv.SELL_PRICE = Convert.ToDouble(aReader[6]);
                unitConv.PART = new Part(Convert.ToInt32(aReader[7]));
                unitConv.BARCODE = aReader["unitconv_barcode"].ToString();
            }
            return unitConv;
        }
        public static UnitConversion GetUnitConversion(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            UnitConversion unitConv = null;
            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    unitConv = new UnitConversion();
                    unitConv.ID = Convert.ToInt32(aReader[0]);
                    unitConv.CODE = aReader[1].ToString();
                    unitConv.NAME = aReader[2].ToString();
                    unitConv.CONVERSION_QTY = Convert.ToDouble(aReader[3]);
                    unitConv.CONVERSION_UNIT = new Unit(Convert.ToInt32(aReader[4]));
                    unitConv.COST_PRICE = Convert.ToDouble(aReader[5]);
                    unitConv.SELL_PRICE = Convert.ToDouble(aReader[6]);
                    unitConv.PART = new Part(Convert.ToInt32(aReader[7]));
                    unitConv.BARCODE = aReader["unitconv_barcode"].ToString();
                }
            }
            return unitConv;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_unitconversion 
                (
                unitconv_code,
                unitconv_name,
                unitconv_qty,
                unitconv_unit,
                unitconv_costprice,
                unitconv_sellprice,
                part_id,
                unitconv_barcode) 
                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                CODE, 
                NAME,
                CONVERSION_QTY,
                CONVERSION_UNIT.ID,
                COST_PRICE,
                SELL_PRICE,
                PART.ID,
                BARCODE);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_unitconversion where unitconv_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_unitconversion set 
                unitconv_code = '{0}', 
                unitconv_name ='{1}',
                unitconv_qty ='{2}',
                unitconv_unit ='{3}',
                unitconv_costprice ='{4}',
                unitconv_sellprice ='{5}',
                part_id ='{6}',
                unitconv_barcode ='{7}'
                where unitconv_id = {8}",
                CODE, 
                NAME, 
                CONVERSION_QTY,
                CONVERSION_UNIT.ID,
                COST_PRICE,
                SELL_PRICE,
                PART.ID,
                BARCODE,
                ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_unitconversion where unitconv_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_unitconversion where unitconv_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_unitconversion where unitconv_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_unitconversion where unitconv_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_unitconversion");
        }
        public static string DeleteUpdate(int id, IList notIN)
        {
            StringBuilder poisSB = new StringBuilder();
            foreach (UnitConversion i in notIN)
            {
                poisSB.Append(i.ID.ToString());
                poisSB.Append(',');
            }
            string pois = poisSB.ToString();
            pois = notIN.Count > 0 ? pois.Substring(0, pois.Length - 1) : "";
            return String.Format("Delete from table_unitconversion where part_id = {0} and unitconv_id not in ({1})", id, pois);
        }
        public static string GetAllByPartSQL(int partID)
        {
            return String.Format("select * from table_unitconversion where part_id = '{0}'",partID);
        }
        public static string GetAllByBarcodeSQL(string barcode)
        {
            return String.Format("select * from table_unitconversion where unitconv_barcode = '{0}'", barcode);
        }
        public static string GetByPartAndUnitConIDSQL(int partID, int unitConvID)
        {
            return String.Format("select * from table_unitconversion where part_id = {0} and unitconv_unit = {1}", partID, unitConvID);
        }
        public string GetConcatSearch(string find)
        {
            return "";
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                UnitConversion unitConv = new UnitConversion();
                unitConv.ID = Convert.ToInt32(aReader[0]);
                unitConv.CODE = aReader[1].ToString();
                unitConv.NAME = aReader[2].ToString();
                unitConv.CONVERSION_QTY = Convert.ToDouble(aReader[3]);
                unitConv.CONVERSION_UNIT = new Unit(Convert.ToInt32(aReader[4]));
                unitConv.COST_PRICE = Convert.ToDouble(aReader[5]);
                unitConv.SELL_PRICE = Convert.ToDouble(aReader[6]);
                unitConv.PART = new Part(Convert.ToInt32(aReader[7]));
                unitConv.BARCODE = aReader["unitconv_barcode"].ToString();
                result.Add(unitConv);
            }
            return result;
        }
        public static IList GetAllStatic(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                UnitConversion unitConv = new UnitConversion();
                unitConv.ID = Convert.ToInt32(aReader[0]);
                unitConv.CODE = aReader[1].ToString();
                unitConv.NAME = aReader[2].ToString();
                unitConv.CONVERSION_QTY = Convert.ToDouble(aReader[3]);
                unitConv.CONVERSION_UNIT = new Unit(Convert.ToInt32(aReader[4]));
                unitConv.COST_PRICE = Convert.ToDouble(aReader[5]);
                unitConv.SELL_PRICE = Convert.ToDouble(aReader[6]);
                unitConv.PART = new Part(Convert.ToInt32(aReader[7]));
                unitConv.BARCODE = aReader["unitconv_barcode"].ToString();
                result.Add(unitConv);
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
            return String.Format("SELECT max(unitconv_id) from table_unitconversion");
        }
        public override bool Equals(object obj)
        {
            UnitConversion comp = (UnitConversion)obj;
            if (obj == null) return false;
            return comp.ID == ID; ;
        }
    }
}
