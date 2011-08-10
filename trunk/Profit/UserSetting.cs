using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Profit.Server;

namespace Profit
{
    public class UserSetting
    {
        public static void SaveSetting(DataGridView dg, int userid, string formname)
        {
            UserSettingsRepository r_setting = RepositoryFactory.GetInstance().UserSetting();
            foreach (DataGridViewColumn c in dg.Columns)
            {
                r_setting.SaveSettings(userid, formname + c.Name + "Width", typeof(int), c.Width.ToString());
                r_setting.SaveSettings(userid, formname + c.Name + "Visible", typeof(bool), c.Visible.ToString());
            }
        }
        public static void LoadSetting(DataGridView dg, int userid, string formname)
        {
            UserSettingsRepository r_setting = RepositoryFactory.GetInstance().UserSetting();
            foreach (DataGridViewColumn c in dg.Columns)
            {
                c.Visible = r_setting.GetBoolValue(userid, formname + c.Name + "Visible");
                c.Width = r_setting.GetIntValue(userid, formname + c.Name + "Width", c.Width);
            }
        }
        public static void SaveSetting(string uniqname, string value, int userid, string formname, Type type)
        {
            UserSettingsRepository r_setting = RepositoryFactory.GetInstance().UserSetting();
            r_setting.SaveSettings(userid, formname + uniqname, type, value);
        }
        public static string GetStringValue(string uniqname, int userid, string formname, string defaultVal)
        {
            UserSettingsRepository r_setting = RepositoryFactory.GetInstance().UserSetting();
            return r_setting.GetStringValue(userid, formname + uniqname, defaultVal);
        }
    }
}
