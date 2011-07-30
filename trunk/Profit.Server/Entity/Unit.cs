using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Unit : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public Unit()
        {
        }
        public Unit(int id)
        {
            ID = id;
        }
        public Unit(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(OdbcDataReader aReader)
        {
            Unit unit = null;
            while (aReader.Read())
            {
                unit = new Unit();
                unit.ID = Convert.ToInt32(aReader[0]);
                unit.CODE = aReader[1].ToString();
                unit.NAME = aReader[2].ToString();
            }
            return unit;
        }
        public static Unit GetUnit(OdbcDataReader aReader)
        {
            Unit unit = null;
            while (aReader.Read())
            {
                unit = new Unit();
                unit.ID = Convert.ToInt32(aReader[0]);
                unit.CODE = aReader[1].ToString();
                unit.NAME = aReader[2].ToString();
            }
            return unit;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_unit 
                (unit_code,unit_name) 
                VALUES ('{0}','{1}')",
                CODE, NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_unit where unit_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_unit set 
                unit_code = '{0}', 
                unit_name='{1}'
                where unit_id = {2}",
                CODE, NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_unit where unit_id = {0}", ID);
        }
        public static string GetByIDSQLstatic(int ID)
        {
            return String.Format("select * from table_unit where unit_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_unit where unit_code = '{0}'", code);
        }
        public static string GetByCodeSQLStatic(string code)
        {
            return String.Format("select * from table_unit where unit_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_unit where unit_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_unit where unit_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_unit");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_unit p where concat(p.unit_code, p.unit_name) like '%{0}%'", find);
        }
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Unit unit = new Unit();
                unit.ID = Convert.ToInt32(aReader[0]);
                unit.CODE = aReader[1].ToString();
                unit.NAME = aReader[2].ToString();
                result.Add(unit);
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
            return String.Format("SELECT max(unit_id) from table_unit");
        }
    }
}
