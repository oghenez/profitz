using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;

namespace Profit.Server
{
    public class EmployeeRepository : Repository
    {
        public EmployeeRepository(IEntity e)
            : base(e)
        {
        }
        public IList GetAllPurchaser()
        {
            try
            {
                OpenConnection();
                Employee emp = (Employee)m_entity;
                OdbcCommand aCommand = new OdbcCommand(emp.GetAllPurchaser(), m_connection);
                OdbcDataReader aReader = aCommand.ExecuteReader();
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
        public IList GetAllStoreman()
        {
            try
            {
                OpenConnection();
                Employee emp = (Employee)m_entity;
                OdbcCommand aCommand = new OdbcCommand(emp.GetAllStoreman(), m_connection);
                OdbcDataReader aReader = aCommand.ExecuteReader();
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
        public IList GetAllSalesman()
        {
            try
            {
                OpenConnection();
                Employee emp = (Employee)m_entity;
                OdbcCommand aCommand = new OdbcCommand(emp.GetAllSalesman(), m_connection);
                OdbcDataReader aReader = aCommand.ExecuteReader();
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
    }
}
