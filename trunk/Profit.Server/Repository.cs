using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;
using System.Data;

namespace Profit.Server
{
    public class Repository
    {
        protected MySql.Data.MySqlClient.MySqlConnection m_connection = new MySql.Data.MySqlClient.MySqlConnection("server=localhost;database=profit_db;uid=root;pwd=1234");
        protected IEntity m_entity = null;
        protected MySql.Data.MySqlClient.MySqlCommand m_cmd = new MySql.Data.MySqlClient.MySqlCommand();

        public Repository(IEntity e)
        {
            m_entity = e;
            m_cmd.Connection = m_connection;
        }
        public void SetConnection(MySql.Data.MySqlClient.MySqlConnection connection)
        {
            m_connection = connection;
        }
        protected void OpenConnection()
        {
            if (m_connection.State == ConnectionState.Closed) m_connection.Open();
        }
        protected void CloseConnection()
        {
            if (m_connection.State == ConnectionState.Open) m_connection.Close();
        }
        public virtual void Save(IEntity e)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetInsertSQL(), m_connection);
                e.SetID(aCommand.ExecuteNonQuery());
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public virtual void Update(IEntity e)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetUpdateSQL(), m_connection);
                aCommand.ExecuteNonQuery();

            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public virtual void Delete(IEntity e)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetDeleteSQL(), m_connection);
                aCommand.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public IEntity GetById(IEntity e)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetByIDSQL(e.GetID()), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IEntity a = e.Get(aReader);
                return a;
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public int GetMaximumID(IEntity e)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetMaximumIDSQL(), m_connection);
                object r = aCommand.ExecuteScalar();
                int aReader = r is System.DBNull ? 0 : Convert.ToInt32(r);
                return aReader;
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public virtual IList GetAll()
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(m_entity.GetAllSQL(), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = m_entity.GetAll(aReader);
                return a;
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public virtual Hashtable GetAllHashtable()
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(m_entity.GetAllSQL(), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = m_entity.GetAll(aReader);
                Hashtable hs = new Hashtable();
                foreach(IEntity e in a)
                    hs.Add(e.ToString(), e);
                return hs;
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public virtual IList GetConcatSearch(string text)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(m_entity.GetConcatSearch(text), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = m_entity.GetAll(aReader);
                return a;
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public DataSet GetAllDataSet()
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlDataAdapter aAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter(m_entity.GetAllSQL(), m_connection);
                DataSet ds = new DataSet("ds");
                aAdapter.Fill(ds);
                return ds;
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public IEntity GetByCode(IEntity e)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetByCodeSQL(e.GetCode()), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IEntity a = e.Get(aReader);
                return a;
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }

        }
        public IList GetByCodeLike(IEntity e, string text)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetByCodeLikeSQL(text), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = e.GetAll(aReader);
                return a;
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }

        }
        public IList GetByNameLike(IEntity e, string text)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetByNameLikeSQL(text), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = e.GetAll(aReader);
                return a;
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }

        }
        public void ExecuteSQL(string sql)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(sql, m_connection);
                aCommand.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        protected string getErrorMessage(Exception x)
        {
            //if (x is OdbcException)
            //{
            //    OdbcException y = (OdbcException)x;
            //    if (y.ErrorCode == -2147467259) return "Record still used by other user";
            //    if (y.ErrorCode == -2146232009) return "Record already used by other record";
            //}
            return x.Message;
        }
    }
}
