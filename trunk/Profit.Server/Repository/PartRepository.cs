using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PartRepository : Repository
    {
        public PartRepository(IEntity e)
            : base(e)
        { }
        public override void Save(IEntity en)
        {
            OpenConnection();
            OdbcTransaction trans = m_connection.BeginTransaction();
            OdbcCommand aCommand = new OdbcCommand();
            aCommand.Connection = m_connection;
            aCommand.Transaction = trans;
            try
            {
                Part e = (Part)en;
                aCommand.CommandText = e.GetInsertSQL();
                aCommand.ExecuteNonQuery();
                aCommand.CommandText = e.GetMaximumIDSQL();
                e.ID = Convert.ToInt32(aCommand.ExecuteScalar());
                foreach (UnitConversion uc in e.UNIT_CONVERSION_LIST)
                {
                    aCommand.CommandText = uc.GetInsertSQL();
                    aCommand.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (Exception x)
            {
                trans.Rollback();
                en.SetID(0);
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
            OdbcTransaction trans = m_connection.BeginTransaction();
            OdbcCommand aCommand = new OdbcCommand();
            aCommand.Connection = m_connection;
            aCommand.Transaction = trans;
            try
            {
                Part e = (Part)en;
                aCommand.CommandText = e.GetUpdateSQL();
                aCommand.ExecuteNonQuery();

                foreach (UnitConversion ucp in e.UNIT_CONVERSION_LIST)
                {
                    if (ucp.ID > 0)
                    {
                        aCommand.CommandText = ucp.GetUpdateSQL();
                        aCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        aCommand.CommandText = ucp.GetInsertSQL();
                        aCommand.ExecuteNonQuery();
                        aCommand.CommandText = ucp.GetMaximumIDSQL();
                        ucp.ID = Convert.ToInt32(aCommand.ExecuteScalar());
                    }
                }
                aCommand.CommandText = UnitConversion.GetAllByPartSQL(e.ID);
                OdbcDataReader r = aCommand.ExecuteReader();
                IList luc = UnitConversion.GetAllStatic(r);
                r.Close();
                foreach (UnitConversion chk in luc)
                {
                    chk.UPDATED = e.UNIT_CONVERSION_LIST.Contains(chk);
                }
                foreach (UnitConversion chk in luc)
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
        public virtual IList GetUnitConversions(int partID)
        {
            try
            {
                OpenConnection();
                OdbcCommand aCommand = new OdbcCommand(UnitConversion.GetAllByPartSQL(partID), m_connection);
                OdbcDataReader aReader = aCommand.ExecuteReader();
                IList a = UnitConversion.GetAllStatic(aReader);
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
