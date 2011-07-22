using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

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
                foreach (object key in e.UNIT_CONVERSION_LIST.Keys)
                {
                    UnitConversion uc = (UnitConversion)e.UNIT_CONVERSION_LIST[key];
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
    }
}
