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
        public void Import(string s)
        {
            OpenConnection();
            OdbcCommand aCommand = new OdbcCommand();
            aCommand.Connection= m_connection;
            OdbcTransaction trc = m_connection.BeginTransaction();
            aCommand.Transaction = trc;
            try
            {
                string[] p = s.Split(',');
                aCommand.CommandText = Part.GetByCodeSQLStatic(p[0]);
                OdbcDataReader re = aCommand.ExecuteReader();
                Part fpart = Part.GetPart(re);
                re.Close();
                if (fpart != null) { trc.Rollback(); return; }

                Unit u = GetUnit(aCommand, p[3], p[4]);
                PartGroup pg = GetPartGroup(aCommand, p[5], p[6]);
                Currency ccy = GetCurrency(aCommand, p[7], p[8]);
                PartCategory pc = GetPartCategory(aCommand, p[9], "KECIL");
                Part part = new Part();
                part.CODE = p[0];
                part.NAME = p[1];
                part.BARCODE = p[2];
                part.UNIT = u;
                part.PART_GROUP = pg;
                part.CURRENCY = ccy;
                part.PART_CATEGORY = pc;
                aCommand.CommandText = part.GetInsertSQL();
                aCommand.ExecuteNonQuery();
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }

        private Unit GetUnit(OdbcCommand aCommand, string code, string name)
        {
            //OdbcCommand aCommand = new OdbcCommand();
            aCommand.Connection = m_connection;
            aCommand.CommandText = Unit.GetByCodeSQLStatic(code.Trim());
            OdbcDataReader r = aCommand.ExecuteReader();
            Unit u = Unit.GetUnit(r);
            r.Close();
            if (u == null)
            {
                u = new Unit();
                u.CODE = code;
                u.NAME = name;
                aCommand.CommandText = u.GetInsertSQL();
                aCommand.ExecuteNonQuery();
                aCommand.CommandText = u.GetMaximumIDSQL();
                u.ID = Convert.ToInt32(aCommand.ExecuteScalar());
            }
            return u;
        }
        private PartGroup GetPartGroup(OdbcCommand aCommand, string code, string name)
        {
            //OdbcCommand aCommand = new OdbcCommand();
            aCommand.Connection = m_connection;
            aCommand.CommandText = PartGroup.GetByCodeSQLStatic(code.Trim());
            OdbcDataReader r = aCommand.ExecuteReader();
            PartGroup u = PartGroup.GetPartGroup(r);
            r.Close();
            if (u == null)
            {
                u = new PartGroup();
                u.CODE = code;
                u.NAME = name;
                aCommand.CommandText = u.GetInsertSQL();
                aCommand.ExecuteNonQuery();
                aCommand.CommandText = u.GetMaximumIDSQL();
                u.ID = Convert.ToInt32(aCommand.ExecuteScalar());
            }
            return u;
        }
        private Currency GetCurrency(OdbcCommand aCommand, string code, string name)
        {
            //OdbcCommand aCommand = new OdbcCommand();
            aCommand.Connection = m_connection;
            aCommand.CommandText = Currency.GetByCodeSQLStatic(code.Trim());
            OdbcDataReader r = aCommand.ExecuteReader();
            Currency u = Currency.GetCurrency(r);
            r.Close();
            if (u == null)
            {
                u = new Currency();
                u.CODE = code;
                u.NAME = name;
                aCommand.CommandText = u.GetInsertSQL();
                aCommand.ExecuteNonQuery();
                aCommand.CommandText = u.GetMaximumIDSQL();
                u.ID = Convert.ToInt32(aCommand.ExecuteScalar());
            }
            return u;
        }
        private PartCategory GetPartCategory(OdbcCommand aCommand, string code, string name)
        {
            //OdbcCommand aCommand = new OdbcCommand();
            aCommand.Connection = m_connection;
            aCommand.CommandText = PartCategory.GetByCodeSQLStatic(code.Trim());
            OdbcDataReader r = aCommand.ExecuteReader();
            PartCategory u = PartCategory.GetPartCategory(r);
            r.Close();
            if (u == null)
            {
                u = new PartCategory();
                u.CODE = code;
                u.NAME = name;
                aCommand.CommandText = u.GetInsertSQL();
                aCommand.ExecuteNonQuery();
                aCommand.CommandText = u.GetMaximumIDSQL();
                u.ID = Convert.ToInt32(aCommand.ExecuteScalar());
            }
            return u;
        }
        public IList Search(string search)
        {
            try
            {
                OpenConnection();
                OdbcCommand aCommand = new OdbcCommand(Part.GetSearchSQL(search), m_connection);
                OdbcDataReader aReader = aCommand.ExecuteReader();
                IList a = Part.GetAllStatic(aReader);
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
