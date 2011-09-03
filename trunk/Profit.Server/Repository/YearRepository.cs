using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class YearRepository : Repository
    {
        //Repository m_periodRep = new Repository(new Period());
        public YearRepository()
            : base(new Year())
        {
        }
        public override void Save(IEntity en)
        {
            Year e = (Year)en;
            OpenConnection();
            MySql.Data.MySqlClient.MySqlTransaction transac = m_connection.BeginTransaction();
            try
            {
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(e.GetInsertSQL(), m_connection);
                aCommand.Transaction = transac;
                aCommand.ExecuteNonQuery();
                aCommand.CommandText = e.GetMaximumIDSQL();
                e.ID = Convert.ToInt32(aCommand.ExecuteScalar());
                e.GeneratePeriods();
                foreach (Period p in e.PERIODS)
                {
                    aCommand.CommandText = p.GetInsertSQL();
                    aCommand.ExecuteNonQuery();
                }
                transac.Commit();
            }
            catch (Exception x)
            {
                transac.Rollback();
                throw new Exception(getErrorMessage(x));
            }
            finally
            {
                m_connection.Close();
            }
        }
        public IEntity GetPeriodByYearID(IEntity year)
        {
            try
            {
                OpenConnection();
                Period p = new Period();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(p.GetByPeriodByYearID(year.GetID()), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = p.GetAll(aReader,year);
                ((Year)year).PERIODS = a;
                return year;
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
        public override void Delete(IEntity e)
        {
            OpenConnection();
            MySql.Data.MySqlClient.MySqlTransaction transac = m_connection.BeginTransaction();
            try
            {
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand();
                aCommand.Connection = m_connection;
                aCommand.Transaction = transac;
                Year y = (Year)e;
                foreach (Period p in y.PERIODS)
                {
                    aCommand.CommandText = p.GetDeleteSQL();
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
    }
}
