using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class GeneralSetup : IEntity
    {
        public int ID = 0;
        public string COMPANY_NAME = string.Empty;
        public string ADDRESS = string.Empty;
        public string PHONE = string.Empty;
        public string FAX = string.Empty;
        public string TAX_NO = string.Empty;
        public DateTime REG_DATE = DateTime.Today;
        public string EMAIL = string.Empty;
        public string WEBSITE = string.Empty;
        public Period START_ENTRY_PERIOD = null;
        public IDictionary<string, AutoNumberSetup> AUTONUMBER_LIST = new Dictionary<string, AutoNumberSetup>();

        public GeneralSetup()
        {
        }
        public GeneralSetup(int id)
        {
            ID = id;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            GeneralSetup generalsetup = null;
            while (aReader.Read())
            {
                generalsetup = new GeneralSetup();
                generalsetup.ID = Convert.ToInt32(aReader[0]);
                generalsetup.COMPANY_NAME = aReader[1].ToString();
                generalsetup.ADDRESS = aReader[2].ToString();
                generalsetup.PHONE = aReader[3].ToString();
                generalsetup.FAX = aReader[4].ToString();
                generalsetup.TAX_NO = aReader[5].ToString();
                generalsetup.REG_DATE = Convert.ToDateTime(aReader[6]);
                generalsetup.EMAIL = aReader[7].ToString();
                generalsetup.WEBSITE = aReader[8].ToString();
                generalsetup.START_ENTRY_PERIOD = new Period(Convert.ToInt32(aReader[9]));
            }
            return generalsetup;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_generalsetup 
                (
                gst_companyname,
                gst_address,
                gst_phone,
                gst_fax,
                gst_taxno,
                gst_regdate,
                gst_email,
                gst_website,
                gst_startentryperiod    
                ) 
                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8})",
                                         COMPANY_NAME,
                                         ADDRESS,
                                         PHONE,
                                         FAX,
                                         TAX_NO,
                                         REG_DATE.ToString(Utils.DATE_FORMAT),
                                         EMAIL,
                                         WEBSITE,
                                         START_ENTRY_PERIOD.ID
                );
        }
        public string GetDeleteSQL()
        {
            return "delete from table_generalsetup where gst_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_generalsetup set 
                gst_companyname ='{0}',
                gst_address ='{1}',
                gst_phone ='{2}',
                gst_fax ='{3}',
                gst_taxno ='{4}',
                gst_regdate ='{5}',
                gst_email ='{6}',
                gst_website  ='{7}',
                gst_startentryperiod = {8}
                where gst_id = {9}",
                COMPANY_NAME,
                 ADDRESS,
                 PHONE,
                 FAX,
                 TAX_NO,
                 REG_DATE.ToString(Utils.DATE_FORMAT),
                 EMAIL,
                 WEBSITE,
                 START_ENTRY_PERIOD.ID,
                 ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_generalsetup where gst_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return "";// String.Format("select * from table_generalsetup where bank_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return "";//String.Format("select * from table_generalsetup where bank_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return "";//String.Format("select * from table_generalsetup where bank_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return "";//String.Format("select * from table_generalsetup");
        }
        public static string GetAllSQLStatic()
        {
            return String.Format("select * from table_generalsetup");
        }
        public string GetConcatSearch(string find)
        {
            return "";//String.Format(@"SELECT * FROM table_generalsetup p where concat(p.bank_code, p.bank_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                GeneralSetup generalsetup = new GeneralSetup();
                generalsetup.ID = Convert.ToInt32(aReader[0]);
                generalsetup.COMPANY_NAME = aReader[1].ToString();
                generalsetup.ADDRESS = aReader[2].ToString();
                generalsetup.PHONE = aReader[3].ToString();
                generalsetup.FAX = aReader[4].ToString();
                generalsetup.TAX_NO = aReader[5].ToString();
                generalsetup.REG_DATE = Convert.ToDateTime(aReader[6]);
                generalsetup.EMAIL = aReader[7].ToString();
                generalsetup.WEBSITE = aReader[8].ToString();
                generalsetup.START_ENTRY_PERIOD = new Period(Convert.ToInt32(aReader[9]));
                result.Add(generalsetup);
            }
            return result;
        }
        public int GetID()
        {
            return ID;
        }
        public void SetID(int id)
        {
            ID = id;
        }
        public override string ToString()
        {
            return COMPANY_NAME;
        }
        public string GetCode()
        {
            return COMPANY_NAME;
        }
        public void SetCode(string code)
        {
            COMPANY_NAME = code;
        }
        public string GetMaximumIDSQL()
        {
            return String.Format("SELECT max(gst_id) from table_generalsetup");
        }
    }
}
