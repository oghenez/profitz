using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Employee : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public bool IS_SALESMAN = false;
        public bool IS_STOREMAN = false;
        public bool IS_PURCHASER = false;

        public Employee()
        {
        }
        public Employee(int id)
        {
            ID = id;
        }
        public Employee(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(OdbcDataReader aReader)
        {
            Employee employee = null;
            while (aReader.Read())
            {
                employee = new Employee();
                employee.ID = Convert.ToInt32(aReader[0]);
                employee.CODE = aReader[1].ToString();
                employee.NAME = aReader[2].ToString();
                employee.IS_SALESMAN = Convert.ToBoolean(aReader[3]);
                employee.IS_STOREMAN = Convert.ToBoolean(aReader[4]);
                employee.IS_PURCHASER = Convert.ToBoolean(aReader[5]);
            }
            return employee;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_employee 
                (emp_code,emp_name,emp_salesman,emp_storeman,emp_purchaser) 
                VALUES ('{0}','{1}',{2},{3},{4})",
                CODE, NAME,IS_SALESMAN,IS_STOREMAN,IS_PURCHASER);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_employee where emp_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_employee set 
                emp_code = '{0}', 
                emp_name='{1}',
                emp_salesman={2},
                emp_storeman={3},
                emp_purchaser={4}
                where emp_id = {5}",
                CODE, NAME, IS_SALESMAN, IS_STOREMAN, IS_PURCHASER, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_employee where emp_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_employee where emp_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_employee where emp_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_employee where emp_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_employee");
        }
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Employee employee = new Employee();
                employee.ID = Convert.ToInt32(aReader[0]);
                employee.CODE = aReader[1].ToString();
                employee.NAME = aReader[2].ToString();
                employee.IS_SALESMAN = Convert.ToBoolean(aReader[3]);
                employee.IS_STOREMAN = Convert.ToBoolean(aReader[4]);
                employee.IS_PURCHASER = Convert.ToBoolean(aReader[5]);
                result.Add(employee);
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
            return String.Format("SELECT max(emp_id) from table_employee");
        }
        public string GetAllSalesman()
        {
            return String.Format("select * from table_employee where emp_salesman = {0}", true);
        }
        public string GetAllPurchaser()
        {
            return String.Format("select * from table_employee where emp_purchaser = {0}", true);
        }
        public string GetAllStoreman()
        {
            return String.Format("select * from table_employee where emp_storeman = {0}", true);
        }
    }
}
