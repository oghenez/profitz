﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class SupplierCategory : Entity, IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public SupplierCategory()
        {
        }
        public SupplierCategory(int id)
        {
            ID = id;
        }
        public SupplierCategory(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            SupplierCategory suppliercategory = null;
            while (aReader.Read())
            {
                suppliercategory = new SupplierCategory();
                suppliercategory.ID = Convert.ToInt32(aReader[0]);
                suppliercategory.CODE = aReader[1].ToString();
                suppliercategory.NAME = aReader[2].ToString();
                suppliercategory.MODIFIED_BY = aReader["modified_by"].ToString();
                suppliercategory.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                suppliercategory.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return suppliercategory;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_suppliercategory 
                (supcat_code,supcat_name, modified_by, modified_date, modified_computer) 
                VALUES ('{0}','{1}', '{2}', '{3}', '{4}')",
                CODE, NAME, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_suppliercategory where supcat_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_suppliercategory set 
                supcat_code = '{0}', 
                supcat_name='{1}',
                modified_by='{2}', 
                modified_date='{3}',
                modified_computer='{4}'
                where supcat_id = {5}",
                CODE, NAME, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_suppliercategory where supcat_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_suppliercategory where supcat_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_suppliercategory where supcat_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_suppliercategory where supcat_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_suppliercategory");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_suppliercategory p where concat(p.supcat_code, p.supcat_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                SupplierCategory suppliercategory = new SupplierCategory();
                suppliercategory.ID = Convert.ToInt32(aReader[0]);
                suppliercategory.CODE = aReader[1].ToString();
                suppliercategory.NAME = aReader[2].ToString();
                suppliercategory.MODIFIED_BY = aReader["modified_by"].ToString();
                suppliercategory.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                suppliercategory.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(suppliercategory);
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
            return String.Format("SELECT max(supcat_id) from table_suppliercategory");
        }
    }
}
