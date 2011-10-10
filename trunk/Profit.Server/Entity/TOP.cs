using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class TermOfPayment : Entity, IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public int DAYS = 0;

        public TermOfPayment()
        {
        }
        public TermOfPayment(int id)
        {
            ID = id;
        }
        public TermOfPayment(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            TermOfPayment top = null;
            while (aReader.Read())
            {
                top = new TermOfPayment();
                top.ID = Convert.ToInt32(aReader[0]);
                top.CODE = aReader[1].ToString();
                top.NAME = aReader[2].ToString();
                top.DAYS = Convert.ToInt16(aReader[3]);
                top.MODIFIED_BY = aReader["modified_by"].ToString();
                top.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                top.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return top;
        }
        public static TermOfPayment GetTOP(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            TermOfPayment top = null;
            while (aReader.Read())
            {
                top = new TermOfPayment();
                top.ID = Convert.ToInt32(aReader[0]);
                top.CODE = aReader[1].ToString();
                top.NAME = aReader[2].ToString();
                top.DAYS = Convert.ToInt16(aReader[3]);
                top.MODIFIED_BY = aReader["modified_by"].ToString();
                top.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                top.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return top;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_termofpayment 
                (top_code,top_name,top_days, modified_by, modified_date, modified_computer) 
                VALUES ('{0}','{1}',{2},'{3}','{4}','{5}')",
                CODE, NAME, DAYS, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_termofpayment where top_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_termofpayment set 
                top_code = '{0}', 
                top_name='{1}',
                top_days={2},
                modified_by='{3}', 
                modified_date='{4}',
                modified_computer='{5}'
                where top_id = {6}",
                CODE, NAME, DAYS, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_termofpayment where top_id = {0}", ID);
        }
        public static string GetByIDSQLStatic(int ID)
        {
            return String.Format("select * from table_termofpayment where top_id = {0}", ID);
        }
        public static string GetByCodeStaticSQL(string code)
        {
            return String.Format("select * from table_termofpayment where top_code = '{0}'", code);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_termofpayment where top_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_termofpayment where top_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_termofpayment where top_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_termofpayment");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_termofpayment p where concat(p.top_code, p.top_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                TermOfPayment top = new TermOfPayment();
                top.ID = Convert.ToInt32(aReader[0]);
                top.CODE = aReader[1].ToString();
                top.NAME = aReader[2].ToString();
                top.DAYS = Convert.ToInt16(aReader[3]);
                top.MODIFIED_BY = aReader["modified_by"].ToString();
                top.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                top.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(top);
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
            return String.Format("SELECT max(top_id) from table_termofpayment");
        }
    }
}
