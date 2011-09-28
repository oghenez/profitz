using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class User : Entity, IEntity
    {
        Crypto m_crypto = new Crypto();

        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public string PASSWORD = "1234";
        public bool ACTIVE = true;
        public Employee EMPLOYEE = null;
        public IDictionary<string, FormAccess> FORM_ACCESS_LIST= new Dictionary<string, FormAccess>();
        public User()
        {
        }
        public User(int id)
        {
            ID = id;
        }
        public User(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            User user = null;
            while (aReader.Read())
            {
                user = new User();
                user.ID = Convert.ToInt32(aReader[0]);
                user.CODE = aReader[1].ToString();
                user.NAME = aReader[2].ToString();
                user.PASSWORD = m_crypto.Decrypt(aReader[3].ToString());
                user.ACTIVE = Convert.ToBoolean(aReader[4]);
                user.MODIFIED_BY = aReader["modified_by"].ToString();
                user.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                user.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                user.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
            }
            return user;
        }
        public static User TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            User user = null;
            Crypto m_crypto = new Crypto();
            while (aReader.Read())
            {
                user = new User();
                user.ID = Convert.ToInt32(aReader[0]);
                user.CODE = aReader[1].ToString();
                user.NAME = aReader[2].ToString();
                user.PASSWORD = m_crypto.Decrypt(aReader[3].ToString());
                user.ACTIVE = Convert.ToBoolean(aReader[4]);
                user.MODIFIED_BY = aReader["modified_by"].ToString();
                user.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                user.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                user.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));

            }
            return user;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_user 
                (user_code,user_name, user_password, user_active, modified_by, modified_date, modified_computer, emp_id) 
                VALUES ('{0}','{1}','{2}',{3},'{4}','{5}','{6}')",
                CODE, NAME, m_crypto.Encrypt(PASSWORD), 
                ACTIVE, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), 
                MODIFIED_COMPUTER_NAME,
                EMPLOYEE==null?0:EMPLOYEE.ID
                );
        }
        public string GetDeleteSQL()
        {
            return "delete from table_user where user_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_user set 
                user_code = '{0}', 
                user_name='{1}',
                user_password='{2}',
                user_active = {3},
                modified_by='{4}', 
                modified_date='{5}',
                modified_computer='{6}',
                emp_id = {7}
                where user_id = {8}",
                CODE, NAME, m_crypto.Encrypt(PASSWORD), ACTIVE, 
                MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), 
                MODIFIED_COMPUTER_NAME, 
                EMPLOYEE==null?0:EMPLOYEE.ID,
                ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_user where user_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_user where user_code = '{0}'", code);
        }
        public static string GetByExactCodeSQL(string code)
        {
            return String.Format("select * from table_user where user_code COLLATE latin1_general_cs = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_user where user_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_user where user_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_user");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_user p where concat(p.user_code, p.user_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                User user = new User();
                user.ID = Convert.ToInt32(aReader[0]);
                user.CODE = aReader[1].ToString();
                user.NAME = aReader[2].ToString();
                user.PASSWORD = m_crypto.Decrypt(aReader[3].ToString());
                user.ACTIVE = Convert.ToBoolean(aReader[4]);
                user.MODIFIED_BY = aReader["modified_by"].ToString();
                user.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                user.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                user.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));

                result.Add(user);
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
            return String.Format("SELECT max(user_id) from table_user");
        }
    }
}
