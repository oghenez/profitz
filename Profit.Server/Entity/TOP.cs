using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class TermOfPayment : IEntity
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
        public IEntity Get(OdbcDataReader aReader)
        {
            TermOfPayment top = null;
            while (aReader.Read())
            {
                top = new TermOfPayment();
                top.ID = Convert.ToInt32(aReader[0]);
                top.CODE = aReader[1].ToString();
                top.NAME = aReader[2].ToString();
                top.DAYS = Convert.ToInt16(aReader[3]);
            }
            return top;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_termofpayment 
                (top_code,top_name,top_days) 
                VALUES ('{0}','{1}',{2})",
                CODE, NAME, DAYS);
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
                top_days={2}
                where top_id = {3}",
                CODE, NAME, DAYS, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_termofpayment where top_id = {0}", ID);
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
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                TermOfPayment top = new TermOfPayment();
                top.ID = Convert.ToInt32(aReader[0]);
                top.CODE = aReader[1].ToString();
                top.NAME = aReader[2].ToString();
                top.DAYS = Convert.ToInt16(aReader[3]);
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
