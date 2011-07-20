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
        public IEntity Get(OdbcDataReader aReader)
        {
            Part part = null;
            while (aReader.Read())
            {
                part = new Part();
                part.ID = Convert.ToInt32(aReader[0]);
                part.CODE = aReader[1].ToString();
                part.NAME = aReader[2].ToString();
            }
            return part;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_part 
                (part_code,part_name) 
                VALUES ('{0}','{1}')",
                CODE, NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_part where part_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_part set 
                part_code = '{0}', 
                part_name='{1}'
                where part_id = {2}",
                CODE, NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_part where part_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
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
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Part part = new Part();
                part.ID = Convert.ToInt32(aReader[0]);
                part.CODE = aReader[1].ToString();
                part.NAME = aReader[2].ToString();
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
    }
}
