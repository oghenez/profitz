using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PartGroup : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public PartGroup()
        {
        }
        public PartGroup(int id)
        {
            ID = id;
        }
        public PartGroup(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            PartGroup partgroup = null;
            while (aReader.Read())
            {
                partgroup = new PartGroup();
                partgroup.ID = Convert.ToInt32(aReader[0]);
                partgroup.CODE = aReader[1].ToString();
                partgroup.NAME = aReader[2].ToString();
            }
            return partgroup;
        }
        public static PartGroup GetPartGroup(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            PartGroup partgroup = null;
            while (aReader.Read())
            {
                partgroup = new PartGroup();
                partgroup.ID = Convert.ToInt32(aReader[0]);
                partgroup.CODE = aReader[1].ToString();
                partgroup.NAME = aReader[2].ToString();
            }
            return partgroup;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_partgroup 
                (prtgroup_code,prtgroup_name) 
                VALUES ('{0}','{1}')",
                CODE, NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_partgroup where prtgroup_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_partgroup set 
                prtgroup_code = '{0}', 
                prtgroup_name='{1}'
                where prtgroup_id = {2}",
                CODE, NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_partgroup where prtgroup_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_partgroup where prtgroup_code = '{0}'", code);
        }
        public static string GetByCodeSQLStatic(string code)
        {
            return String.Format("select * from table_partgroup where prtgroup_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_partgroup where prtgroup_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_partgroup where prtgroup_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_partgroup");
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                PartGroup partgroup = new PartGroup();
                partgroup.ID = Convert.ToInt32(aReader[0]);
                partgroup.CODE = aReader[1].ToString();
                partgroup.NAME = aReader[2].ToString();
                result.Add(partgroup);
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
            return String.Format("SELECT max(prtgroup_id) from table_partgroup");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_partgroup p where concat(p.prtgroup_code, p.prtgroup_name) like '%{0}%'", find);
        }
    }
}
