using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace Profit.Server
{
    public class UserSettingsRepository
    {
        protected OdbcConnection m_connection = new OdbcConnection("Driver={MySQL ODBC 5.1 Driver};server=localhost;database=profit_db;uid=root;pwd=1234");
        OdbcCommand m_cmd = new OdbcCommand();

        public UserSettingsRepository()
        {
            m_cmd.Connection = m_connection;
        }
        public void SaveSettings(int userid, string name, Type type, string value)
        {
            if(m_connection.State== System.Data.ConnectionState.Closed)m_connection.Open();

            if (isExist(userid, name, type))
            {
                string update = String.Format(@"update table_usersettings set 
                us_value = '{0}'
                where user_id = {1}
                        and us_name = '{2}' and us_type = '{3}' ",
                value, userid, name, type.ToString());
                m_cmd.CommandText = update;
                m_cmd.ExecuteNonQuery();
            }
            else
            {
                string insert = String.Format(@"insert into table_usersettings
                (user_id,us_name,us_value,us_type) 
                VALUES ('{0}','{1}','{2}','{3}')", userid, name, value, type.ToString());
                m_cmd.CommandText = insert;
                m_cmd.ExecuteNonQuery();
            }
        }
        private bool isExist(int userid, string name, Type type)
        {
            string get = String.Format(@"select count(*) from table_usersettings where user_id = {0}
                        and us_name = '{1}' and us_type = '{2}' ", userid, name, type.ToString());
            m_cmd.CommandText = get;
            int count = Convert.ToInt32(m_cmd.ExecuteScalar());
            return count > 0;
        }
        public bool GetBoolValue(int userid, string name)
        {
            if (m_connection.State == System.Data.ConnectionState.Closed) m_connection.Open();
            string get = String.Format(@"select us_value from table_usersettings where user_id = {0}
                        and us_name = '{1}' and us_type = '{2}' ", userid, name, typeof(bool).ToString());
            m_cmd.CommandText = get;
            object t = m_cmd.ExecuteScalar();
            if (t == null) return true;
            return Convert.ToBoolean(t);
        }
        public int GetIntValue(int userid, string name, int def)
        {
            if (m_connection.State == System.Data.ConnectionState.Closed) m_connection.Open();
            string get = String.Format(@"select us_value from table_usersettings where user_id = {0}
                        and us_name = '{1}' and us_type = '{2}' ", userid, name, typeof(int).ToString());
            m_cmd.CommandText = get;
            object t = m_cmd.ExecuteScalar();
            if (t == null) return def;
            return Convert.ToInt32(t);
        }
        public string GetStringValue(int userid, string name, string def)
        {
            if (m_connection.State == System.Data.ConnectionState.Closed) m_connection.Open();
            string get = String.Format(@"select us_value from table_usersettings where user_id = {0}
                        and us_name = '{1}' and us_type = '{2}' ", userid, name, typeof(String).ToString());
            m_cmd.CommandText = get;
            object t = m_cmd.ExecuteScalar();
            if (t == null) return def;
            return t.ToString();
        }
    }
}
