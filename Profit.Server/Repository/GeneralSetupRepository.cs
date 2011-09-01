using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class GeneralSetupRepository : Repository
    {
        public GeneralSetupRepository()
            : base(new GeneralSetup())
        {
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
                GeneralSetup e = (GeneralSetup)en;
                aCommand.CommandText = e.GetUpdateSQL();
                aCommand.ExecuteNonQuery();

                foreach (string kys in e.AUTONUMBER_LIST.Keys)
                {
                    aCommand.CommandText = e.AUTONUMBER_LIST[kys].GetUpdateSQL();
                    aCommand.ExecuteNonQuery();
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
        public new IEntity GetById(IEntity en)
        {
            try
            {
                OpenConnection();
                GeneralSetup e = (GeneralSetup)en;
                OdbcCommand aCommand = new OdbcCommand(e.GetByIDSQL(e.GetID()), m_connection);
                OdbcDataReader aReader = aCommand.ExecuteReader();
                GeneralSetup a = (GeneralSetup)e.Get(aReader);
                aReader.Close();
                a.START_ENTRY_PERIOD = PeriodRepository.FindPeriod(aCommand, a.START_ENTRY_PERIOD.ID);
                aCommand.CommandText = AutoNumberSetup.GetAllSQLStatic();
                aReader = aCommand.ExecuteReader();
                IList lst = AutoNumberSetup.GetAllStatic(aReader);
                aReader.Close();
                foreach (AutoNumberSetup s in lst)
                {
                    a.AUTONUMBER_LIST.Add(s.FORM_CODE, s);
                }

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
        internal static GeneralSetup GetGeneralSetup(OdbcCommand cmd)
        {
            GeneralSetup e = new GeneralSetup();
            cmd.CommandText = GeneralSetup.GetAllSQLStatic();
            OdbcDataReader aReader = cmd.ExecuteReader();
            GeneralSetup a = (GeneralSetup)e.Get(aReader);
            aReader.Close();
            cmd.CommandText = AutoNumberSetup.GetAllSQLStatic();
            aReader = cmd.ExecuteReader();
            IList lst = AutoNumberSetup.GetAllStatic(aReader);
            aReader.Close();
            foreach (AutoNumberSetup s in lst)
            {
                a.AUTONUMBER_LIST.Add(s.FORM_CODE, s);
            }

            return a;

        }
        public override IList GetAll()
        {
            try
            {
                OpenConnection();
                OdbcCommand aCommand = new OdbcCommand(m_entity.GetAllSQL(), m_connection);
                OdbcDataReader aReader = aCommand.ExecuteReader();
                IList a = m_entity.GetAll(aReader);
                aReader.Close();
                foreach (GeneralSetup s in a)
                {
                    s.START_ENTRY_PERIOD = PeriodRepository.FindPeriod(aCommand, s.START_ENTRY_PERIOD.ID);
                }
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
