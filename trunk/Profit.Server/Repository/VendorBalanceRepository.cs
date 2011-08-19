using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class VendorBalanceRepository : Repository
    {
        public VendorBalanceRepository()
            : base(new VendorBalance())
        {

        }
        public static void Update(OdbcCommand cmd, VendorBalance sc)
        {
            cmd.CommandText = sc.GetUpdateSQL();
            cmd.ExecuteNonQuery();
            foreach (VendorBalanceEntry sce in sc.VENDOR_BALANCE_ENTRIES)
            {
                if (sce.ID == 0)
                {
                    cmd.CommandText = sce.GetInsertSQL();
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = VendorBalanceEntry.SelectMaxIDSQL();
                    sce.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else 
                {
                    cmd.CommandText = sce.GetUpdateSQL();
                    cmd.ExecuteNonQuery();
                }
            }
            cmd.CommandText = VendorBalanceEntry.FindByVendorBalance(sc.ID);
            OdbcDataReader r = cmd.ExecuteReader();
            IList sces = VendorBalanceEntry.TransformReaderList(r);
            r.Close();
            foreach (VendorBalanceEntry sce in sces)
            {
                sce.UPDATED = sc.VENDOR_BALANCE_ENTRIES.Contains(sce);
            }
            foreach (VendorBalanceEntry sce in sces)
            {
                if (!sce.UPDATED)
                {
                    cmd.CommandText = VendorBalanceEntry.DeleteSQL(sce.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateHeader(OdbcCommand cmd, VendorBalance sc)
        {
            cmd.CommandText = sc.GetUpdateSQL();
            cmd.ExecuteNonQuery();
        }
        public static void Save(OdbcCommand cmd, VendorBalance sc)
        {
            cmd.CommandText = sc.GetInsertSQL();
            cmd.ExecuteNonQuery();
            cmd.CommandText = VendorBalance.SelectMaxIDSQL();
            sc.ID = Convert.ToInt32(cmd.ExecuteScalar());
            foreach (VendorBalanceEntry sce in sc.VENDOR_BALANCE_ENTRIES)
            {
                cmd.CommandText = sce.GetInsertSQL();
                cmd.ExecuteNonQuery();
                cmd.CommandText = VendorBalanceEntry.SelectMaxIDSQL();
                sce.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public static void SaveHeader(OdbcCommand cmd, VendorBalance sc)
        {
            cmd.CommandText = sc.GetInsertSQL();
            cmd.ExecuteNonQuery();
            cmd.CommandText = VendorBalance.SelectMaxIDSQL();
            sc.ID = Convert.ToInt32(cmd.ExecuteScalar());
        }
        public static VendorBalance FindVendorBalance(OdbcCommand cmd, long vendor, long currency, long periodId, VendorBalanceType type)
        {
            cmd.CommandText = String.Format("select * from table_vendorbalance where vendor_id = {0} and ccy_id = {1} and period_id = {2} and vb_vendorbalancetype = '{3}'", 
                vendor, currency, periodId, type.ToString());
            OdbcDataReader r =  cmd.ExecuteReader();
            VendorBalance sc = VendorBalance.TransformReader(r);
            r.Close();
            if (sc != null)
            {
                sc.PERIOD = PeriodRepository.FindPeriod(cmd, sc.PERIOD.ID);
                cmd.CommandText = VendorBalanceEntry.FindByVendorBalance(sc.ID);
                OdbcDataReader rx = cmd.ExecuteReader();
                sc.VENDOR_BALANCE_ENTRIES = VendorBalanceEntry.TransformReaderList(rx);
                rx.Close();
            }
            return sc;
        }
        public static VendorBalance FindVendorBalanceHeader(OdbcCommand cmd, long vendor, long currency, long periodId, VendorBalanceType type)
        {
            cmd.CommandText = String.Format("select * from table_vendorbalance where vendor_id = {0} and ccy_id = {1} and period_id = {2} and vb_vendorbalancetype = '{3}'",
                vendor, currency, periodId, type.ToString());
            OdbcDataReader r = cmd.ExecuteReader();
            VendorBalance sc = VendorBalance.TransformReader(r);
            r.Close();
            if (sc != null)
            {
                sc.PERIOD = PeriodRepository.FindPeriod(cmd, sc.PERIOD.ID);
            }
            return sc;
        }
        public static VendorBalance FindVendorBalance(OdbcCommand cmd, long periodId)
        {
            cmd.CommandText = String.Format("select * from table_vendorbalance where period_id = {0}", periodId);
            OdbcDataReader r = cmd.ExecuteReader();
            VendorBalance sc = VendorBalance.TransformReader(r);
            r.Close();
            return sc;
        }
    }
}
