using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Warehouse : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public Warehouse()
        {
        }
        public Warehouse(int id)
        {
            ID = id;
        }
        public Warehouse(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Warehouse warehouse = null;
            while (aReader.Read())
            {
                warehouse = new Warehouse();
                warehouse.ID = Convert.ToInt32(aReader[0]);
                warehouse.CODE = aReader[1].ToString();
                warehouse.NAME = aReader[2].ToString();
            }
            return warehouse;
        }
        public static Warehouse GetWarehouse(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Warehouse warehouse = null;
            while (aReader.Read())
            {
                warehouse = new Warehouse();
                warehouse.ID = Convert.ToInt32(aReader[0]);
                warehouse.CODE = aReader[1].ToString();
                warehouse.NAME = aReader[2].ToString();
            }
            return warehouse;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_warehouse 
                (warehouse_code,warehouse_name) 
                VALUES ('{0}','{1}')",
                CODE, NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_warehouse where warehouse_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_warehouse set 
                warehouse_code = '{0}', 
                warehouse_name='{1}'
                where warehouse_id = {2}",
                CODE, NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_warehouse where warehouse_id = {0}", ID);
        }
        public static string GetByIDSQLStatic(int ID)
        {
            return String.Format("select * from table_warehouse where warehouse_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_warehouse where warehouse_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_warehouse where warehouse_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_warehouse where warehouse_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_warehouse");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_warehouse p where concat(p.warehouse_code, p.warehouse_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Warehouse warehouse = new Warehouse();
                warehouse.ID = Convert.ToInt32(aReader[0]);
                warehouse.CODE = aReader[1].ToString();
                warehouse.NAME = aReader[2].ToString();
                result.Add(warehouse);
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
            return String.Format("SELECT max(warehouse_id) from table_warehouse");
        }
    }
}
