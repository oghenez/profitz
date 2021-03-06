﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Division : Entity, IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public Division()
        {
        }
        public Division(int id)
        {
            ID = id;
        }
        public Division(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Division division = null;
            while (aReader.Read())
            {
                division = new Division();
                division.ID = Convert.ToInt32(aReader[0]);
                division.CODE = aReader[1].ToString();
                division.NAME = aReader[2].ToString();
                division.MODIFIED_BY = aReader["modified_by"].ToString();
                division.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                division.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return division;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_division 
                (div_code,div_name, modified_by, modified_date, modified_computer) 
                VALUES ('{0}','{1}', '{2}', '{3}', '{4}')",
                CODE, NAME, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_division where div_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_division set 
                div_code = '{0}', 
                div_name='{1}',
                modified_by='{2}', 
                modified_date='{3}',
                modified_computer='{4}'
                where div_id = {5}",
                CODE, NAME, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_division where div_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_division where div_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_division where div_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_division where div_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_division");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_division p where concat(p.div_code, p.div_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Division division = new Division();
                division.ID = Convert.ToInt32(aReader[0]);
                division.CODE = aReader[1].ToString();
                division.NAME = aReader[2].ToString();
                division.MODIFIED_BY = aReader["modified_by"].ToString();
                division.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                division.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(division);
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
            return String.Format("SELECT max(div_id) from table_division");
        }
    }
}
