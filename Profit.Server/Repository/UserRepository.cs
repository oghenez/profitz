using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class UserRepository : Repository
    {
        public UserRepository(IEntity e)
            : base(e)
        {
        }
        public override void Save(IEntity p)
        {
            OpenConnection();
            MySql.Data.MySqlClient.MySqlTransaction tr = m_connection.BeginTransaction();
            User e = (User)p;
            try
            {
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetInsertSQL(), m_connection);
                aCommand.Transaction = tr;
                aCommand.ExecuteNonQuery();
                aCommand.CommandText = e.GetMaximumIDSQL();
                e.ID = Convert.ToInt32(aCommand.ExecuteScalar());
                foreach (string kys in e.FORM_ACCESS_LIST.Keys)
                {
                    aCommand.CommandText = e.FORM_ACCESS_LIST[kys].GetInsertSQL();
                    aCommand.ExecuteNonQuery();
                    aCommand.CommandText = e.FORM_ACCESS_LIST[kys].GetMaximumIDSQL();
                    e.FORM_ACCESS_LIST[kys].ID = Convert.ToInt32(aCommand.ExecuteScalar());
                }
                tr.Commit();
            }
            catch (Exception x)
            {
                tr.Rollback();
                e.ID = 0;
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public override void Update(IEntity en)
        {
            OpenConnection();
            MySql.Data.MySqlClient.MySqlTransaction trans = m_connection.BeginTransaction();
            MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand();
            aCommand.Connection = m_connection;
            aCommand.Transaction = trans;
            try
            {
                User e = (User)en;
                aCommand.CommandText = e.GetUpdateSQL();
                aCommand.ExecuteNonQuery();

                foreach (string kys in e.FORM_ACCESS_LIST.Keys)
                {
                    if (e.FORM_ACCESS_LIST[kys].ID > 0)
                    {
                        aCommand.CommandText = e.FORM_ACCESS_LIST[kys].GetUpdateSQL();
                        aCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        aCommand.CommandText = e.FORM_ACCESS_LIST[kys].GetInsertSQL();
                        aCommand.ExecuteNonQuery();
                        aCommand.CommandText = e.FORM_ACCESS_LIST[kys].GetMaximumIDSQL();
                        e.FORM_ACCESS_LIST[kys].ID = Convert.ToInt32(aCommand.ExecuteScalar());
                    }
                }
                aCommand.CommandText = FormAccess.GetAllByUserSQL(e.ID);
                MySql.Data.MySqlClient.MySqlDataReader r = aCommand.ExecuteReader();
                IList luc = FormAccess.GetAllStatic(r);
                r.Close();
                foreach (FormAccess chk in luc)
                {
                    chk.UPDATED = e.FORM_ACCESS_LIST.Contains(new KeyValuePair<string,FormAccess>(chk.CODE, chk));
                }
                foreach (FormAccess chk in luc)
                {
                    if (!chk.UPDATED)
                    {
                        aCommand.CommandText = chk.GetDeleteSQL();
                        aCommand.ExecuteNonQuery();
                    }
                }
                trans.Commit();
            }
            catch (Exception x)
            {
                trans.Rollback();
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public override void Delete(IEntity e)
        {
            OpenConnection();
            MySql.Data.MySqlClient.MySqlTransaction transac = m_connection.BeginTransaction();
            try
            {
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand();
                aCommand.Connection = m_connection;
                aCommand.Transaction = transac;
                User y = (User)e;
                foreach (string kys in y.FORM_ACCESS_LIST.Keys)
                {
                    aCommand.CommandText = y.FORM_ACCESS_LIST[kys].GetDeleteSQL();
                    aCommand.ExecuteNonQuery();
                }
                aCommand.CommandText = y.GetDeleteSQL();
                aCommand.ExecuteNonQuery();
                transac.Commit();
            }
            catch (Exception x)
            {
                transac.Rollback();
                m_connection.Close();
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public User GetFormAccessByUserID(User userID)
        {
            try
            {
                OpenConnection();
                FormAccess p = new FormAccess();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(FormAccess.GetAllByUserSQL(userID.ID), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = p.GetAll(aReader);
                foreach (FormAccess fa in a)
                {
                    userID.FORM_ACCESS_LIST.Add(fa.CODE, fa);
                }
                return userID;
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
        public User GetUser(string code, string password)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(User.GetByExactCodeSQL(code));
                cmd.Connection = m_connection;
                MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
                User u = User.TransformReader(r);
                r.Close();
                if(u==null)
                    return null;
                if (!u.ACTIVE)
                {
                    return null;
                }
                if (u.PASSWORD == password)
                {
                    FormAccess p = new FormAccess();
                    MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(FormAccess.GetAllByUserSQL(u.ID), m_connection);
                    MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                    IList a = p.GetAll(aReader);
                    foreach (FormAccess fa in a)
                    {
                        u.FORM_ACCESS_LIST.Add(fa.CODE, fa);
                    }
                    return u;
                }
                else
                    return null;
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
        public User getUser(string code)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(User.GetByExactCodeSQL(code));
                cmd.Connection = m_connection;
                MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
                User u = User.TransformReader(r);
                r.Close();
                if (u == null)
                    return null;
                FormAccess p = new FormAccess();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(FormAccess.GetAllByUserSQL(u.ID), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = p.GetAll(aReader);
                aReader.Close();
                foreach (FormAccess fa in a)
                {
                    u.FORM_ACCESS_LIST.Add(fa.CODE, fa);
                }
                return u;
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
    }
}
