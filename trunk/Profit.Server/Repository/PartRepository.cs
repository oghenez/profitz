using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Data;

namespace Profit.Server
{
    public class PartRepository : Repository
    {
        string m_pictureFolder = System.Windows.Forms.Application.StartupPath + @"\picture\";
        public PartRepository(IEntity e)
            : base(e)
        { }
        public override void Save(IEntity en)
        {
            OpenConnection();
            MySql.Data.MySqlClient.MySqlTransaction trans = m_connection.BeginTransaction();
            MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand();
            aCommand.Connection = m_connection;
            aCommand.Transaction = trans;
            Part e = (Part)en;
            try
            {
                aCommand.CommandText = e.GetInsertSQL();
                this.SavePicture(e.PICTURE, e.CODE);
                aCommand.ExecuteNonQuery();
                aCommand.CommandText = e.GetMaximumIDSQL();
                e.ID = Convert.ToInt32(aCommand.ExecuteScalar());
                //add Base Conversion--------------------
                UnitConversion unit = new UnitConversion();
                unit.BARCODE = e.BARCODE;
                unit.CONVERSION_QTY = 1;
                unit.CONVERSION_UNIT = e.UNIT;
                unit.COST_PRICE = e.COST_PRICE;
                unit.ORIGINAL_QTY = 1;
                unit.PART = e;
                unit.SELL_PRICE = e.SELL_PRICE;
                e.UNIT_CONVERSION_LIST.Add(unit);
                //---------------------------------------
                foreach (UnitConversion uc in e.UNIT_CONVERSION_LIST)
                {
                    aCommand.CommandText = uc.GetInsertSQL();
                    aCommand.ExecuteNonQuery();
                    aCommand.CommandText = uc.GetMaximumIDSQL();
                    uc.ID = Convert.ToInt32(aCommand.ExecuteScalar());
                }
                trans.Commit();
            }
            catch (Exception x)
            {
                trans.Rollback();
                en.SetID(0);
                foreach (UnitConversion uc in e.UNIT_CONVERSION_LIST)
                {
                    uc.ID = 0;
                }
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
                Part e = (Part)en;
                aCommand.CommandText = e.GetUpdateSQL();
                this.SavePicture(e.PICTURE, e.CODE);
                aCommand.ExecuteNonQuery();
                //Update base unit---------------------------
                aCommand.CommandText = UnitConversion.GetByPartAndUnitConIDSQL(e.ID, e.UNIT.ID);
                MySql.Data.MySqlClient.MySqlDataReader r = aCommand.ExecuteReader();
                UnitConversion uc = UnitConversion.GetUnitConversion(r);
                r.Close();
                if (uc == null)
                    uc = new UnitConversion();
                uc.BARCODE = e.BARCODE;
                uc.CONVERSION_QTY = 1;
                uc.CONVERSION_UNIT = e.UNIT;
                uc.COST_PRICE = e.COST_PRICE;
                uc.ORIGINAL_QTY = 1;
                uc.PART = e;
                uc.SELL_PRICE = e.SELL_PRICE;
                e.UNIT_CONVERSION_LIST.Add(uc);
                //------------------------------
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
                aCommand.CommandText = UnitConversion.DeleteUpdate(e.ID, e.UNIT_CONVERSION_LIST);
                aCommand.ExecuteNonQuery();

                //IList luc = UnitConversion.GetAllStatic(r);
                //r.Close();
                //foreach (UnitConversion chk in luc)
                //{
                //    chk.UPDATED = e.UNIT_CONVERSION_LIST.Contains(chk);
                //}
                //foreach (UnitConversion chk in luc)
                //{
                //    if (!chk.UPDATED)
                //    {
                //        aCommand.CommandText = chk.GetDeleteSQL();
                //        aCommand.ExecuteNonQuery();
                //    }
                //}

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
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(UnitConversion.GetAllByPartSQL(partID), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = UnitConversion.GetAllStatic(aReader);
                //if(a.Contains(
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
            MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand();
            aCommand.Connection= m_connection;
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            aCommand.Transaction = trc;
            try
            {
                string[] p = s.Split(',');
                aCommand.CommandText = Part.GetByCodeSQLStatic(p[0]);
                MySql.Data.MySqlClient.MySqlDataReader re = aCommand.ExecuteReader();
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
        private Unit GetUnit(MySql.Data.MySqlClient.MySqlCommand aCommand, string code, string name)
        {
            //MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand();
            aCommand.Connection = m_connection;
            aCommand.CommandText = Unit.GetByCodeSQLStatic(code.Trim());
            MySql.Data.MySqlClient.MySqlDataReader r = aCommand.ExecuteReader();
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
        private PartGroup GetPartGroup(MySql.Data.MySqlClient.MySqlCommand aCommand, string code, string name)
        {
            //MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand();
            aCommand.Connection = m_connection;
            aCommand.CommandText = PartGroup.GetByCodeSQLStatic(code.Trim());
            MySql.Data.MySqlClient.MySqlDataReader r = aCommand.ExecuteReader();
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
        private Currency GetCurrency(MySql.Data.MySqlClient.MySqlCommand aCommand, string code, string name)
        {
            //MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand();
            aCommand.Connection = m_connection;
            aCommand.CommandText = Currency.GetByCodeSQLStatic(code.Trim());
            MySql.Data.MySqlClient.MySqlDataReader r = aCommand.ExecuteReader();
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
        private PartCategory GetPartCategory(MySql.Data.MySqlClient.MySqlCommand aCommand, string code, string name)
        {
            //MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand();
            aCommand.Connection = m_connection;
            aCommand.CommandText = PartCategory.GetByCodeSQLStatic(code.Trim());
            MySql.Data.MySqlClient.MySqlDataReader r = aCommand.ExecuteReader();
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
        public IList SearchActivePart(string search)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(Part.GetSearchSQL(search), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                //DataSet ds = new DataSet();
                //MySql.Data.MySqlClient.MySqlDataAdapter dc = new MySql.Data.MySqlClient.MySqlDataAdapter(Part.GetSearchSQL(search), m_connection);
                //dc.Fill(ds);
                IList a = Part.GetAllStatic(aReader);
                aReader.Close();
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
        public IList GetAllUnit(int partID, int unitID)
        {
            try
            {
                OpenConnection();
                MySql.Data.MySqlClient.MySqlCommand aCommand = new MySql.Data.MySqlClient.MySqlCommand(UnitConversion.GetAllByPartSQL(partID), m_connection);
                MySql.Data.MySqlClient.MySqlDataReader aReader = aCommand.ExecuteReader();
                IList a = UnitConversion.GetAllStatic(aReader);
                aReader.Close();
                IList result = new ArrayList();
                foreach (UnitConversion uc in a)
                {
                    aCommand.CommandText = Unit.GetByIDSQLstatic(uc.CONVERSION_UNIT.ID);
                    aReader = aCommand.ExecuteReader();
                    Unit u = Unit.GetUnit(aReader);
                    aReader.Close();
                    result.Add(u);
                }
                if (!result.Contains(new Unit(unitID)))
                {
                    aCommand.CommandText = Unit.GetByIDSQLstatic(unitID);
                    aReader = aCommand.ExecuteReader();
                    Unit up = Unit.GetUnit(aReader);
                    aReader.Close();
                    result.Add(up);
                }
                return result;
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

        //For transaction
        public static Part GetByID(MySql.Data.MySqlClient.MySqlCommand cmd, int id)
        {
            cmd.CommandText = Part.GetByIDSQLStatic(id);
            MySql.Data.MySqlClient.MySqlDataReader r =  cmd.ExecuteReader();
            Part result = Part.GetPart(r);
            r.Close();
            result.UNIT_CONVERSION_LIST = PartRepository.GetUnitConversionsStatic(cmd, id);
            return result;
        }
        public static IList GetUnitConversionsStatic(MySql.Data.MySqlClient.MySqlCommand cmd, int partID)
        {
            cmd.CommandText = UnitConversion.GetAllByPartSQL(partID);
            MySql.Data.MySqlClient.MySqlDataReader aReader = cmd.ExecuteReader();
            IList a = UnitConversion.GetAllStatic(aReader);
            aReader.Close();
            return a;
        }
        public StockCardInfo GetStockCardInfo(int partID)
        {
            OpenConnection();
            StockCardInfo result = new StockCardInfo();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
            cmd.Connection = m_connection;
            Period p = PeriodRepository.FindCurrentPeriod(cmd);
            cmd.CommandText = StockCard.FindByPartPeriod(partID, p.ID);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            IList stockcards = StockCard.TransforReaderList(r);
            r.Close();
            foreach (StockCard sc in stockcards)
            {
                result.BACKORDER += sc.BACK_ORDER;
                result.BALANCE += sc.BALANCE;
                result.BOOKED += sc.BOOKED;
            }
            return result;
        }
        public void SavePicture(byte[] image, string name)
        {
            try
            {
                if (!Directory.Exists(m_pictureFolder))
                    Directory.CreateDirectory(m_pictureFolder);
                ////if (File.Exists(m_pictureFolder + name + ".JPEG"))
                ////{
                ////    File.Delete(m_pictureFolder + name + ".JPEG");
                ////}
                //StreamWriter w = new StreamWriter(
                //image.Save(m_pictureFolder + name + ".JPEG", System.Drawing.Imaging.ImageFormat.Jpeg);
                //byte[] fileBArray = new byte[(int)file.length()];
                //StreamWriter fis = new StreamWriter(m_pictureFolder + name + ".JPEG");
                //fis.Write(image);
                File.WriteAllBytes(m_pictureFolder + name + ".JPEG", image);

                //FileOutputStream fos = new FileOutputStream("C:\\abc.jpg");
                //fos.write(fileBArray);
                //fis.Close();
            }
            catch(Exception x)
            {}
        }
        public byte[] GetImage(string name)
        {
            byte[] m = null;
            try
            {
                if (File.Exists(m_pictureFolder + name + ".JPEG"))
                {
                   // FileStream r = new FileStream(m_pictureFolder + name + ".JPEG", FileMode.Open, FileAccess.Read);
                   // m = r.Read(
                   // r.Close();
                    
                    FileStream r = new FileStream(m_pictureFolder + name + ".JPEG", FileMode.Open, FileAccess.Read);
                    int length = (int)r.Length;         // get file length
                    m = new byte[length];               // create buffer
                    int count;                            // actual number of bytes read
                    int sum = 0;                          // total number of bytes read

                    // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = r.Read(m, sum, length - sum)) > 0)
                        sum += count;  // sum is a buffer offset for next reading
                    r.Close();
                }
                return m;// Image.FromFile(m_pictureFolder + name + ".JPEG");
            }
            catch (Exception x)
            {
                return m;
            }
        }
        //-------------
        //public void UpdatePart()
        //{
        //    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
        //    cmd.Connection = m_connection;
        //    m_connection.Open();
        //    cmd.CommandText = m_entity.GetAllSQL();
            
        //    MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
        //    IList result = new ArrayList();
        //    while (r.Read())
        //    {
        //        PartUnit pu = new PartUnit();
        //        pu.PART = Convert.ToInt32(r["part_id"]);
        //        pu.UNIT = Convert.ToInt32(r["unit_id"]);
        //        result.Add(pu);
        //    }
        //    r.Close();
        //    foreach (PartUnit e in result)
        //    {
        //        cmd.CommandText = UnitConversion.GetByPartAndUnitConIDSQL(e.PART, e.UNIT);
        //        r = cmd.ExecuteReader();
        //        UnitConversion uc = UnitConversion.GetUnitConversion(r);
        //        r.Close();
        //        if (uc == null)
        //        {
        //            uc = new UnitConversion();
        //            uc.BARCODE = e.BARCODE;
        //            uc.CONVERSION_QTY = 1;
        //            uc.CONVERSION_UNIT = e.UNIT;
        //            uc.COST_PRICE = e.COST_PRICE;
        //            uc.ORIGINAL_QTY = 1;
        //            uc.PART = e;
        //            uc.SELL_PRICE = e.SELL_PRICE;
        //            cmd.CommandText = uc.GetInsertSQL();
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
        //private class PartUnit
        //{
        //    public int PART, UNIT;
            
        //}
    }
}
