using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class AutoNumberSetup : IEntity
    {
        public int ID = 0;
        public string ENTITY_NAME = string.Empty;
        public string FORM_CODE = string.Empty;
        public string PREFIX = string.Empty;
        public int START = 0;
        public int DIGIT = 0;
        public InitialAutoNumberSetup INITIAL_AUTO_NUMBER = InitialAutoNumberSetup.None;
        public AutoNumberSetupType AUTONUMBER_SETUP_TYPE = AutoNumberSetupType.Manual;
        public bool IS_TRANSACTION = false;
        public AutoNumberSetup()
        {
        }
        public AutoNumberSetup(int id)
        {
            ID = id;
        }
        public IEntity Get(OdbcDataReader aReader)
        {
            AutoNumberSetup autonum = null;
            while (aReader.Read())
            {
                autonum = new AutoNumberSetup();
                autonum.ID = Convert.ToInt32(aReader[0]);
                autonum.ENTITY_NAME = aReader[1].ToString();
                autonum.FORM_CODE = aReader[2].ToString();
                autonum.PREFIX = aReader[3].ToString();
                autonum.START = Convert.ToInt32(aReader[4]);
                autonum.DIGIT = Convert.ToInt32(aReader[5]);
                autonum.INITIAL_AUTO_NUMBER = (InitialAutoNumberSetup)Enum.Parse(typeof(InitialAutoNumberSetup), aReader[6].ToString());
                autonum.AUTONUMBER_SETUP_TYPE = (AutoNumberSetupType)Enum.Parse(typeof(AutoNumberSetupType), aReader[7].ToString());
                autonum.IS_TRANSACTION = Convert.ToBoolean(aReader[8]);
            }
            return autonum;
        }
        public static AutoNumberSetup GetTransform(OdbcDataReader aReader)
        {
            AutoNumberSetup autonum = null;
            while (aReader.Read())
            {
                autonum = new AutoNumberSetup();
                autonum.ID = Convert.ToInt32(aReader[0]);
                autonum.ENTITY_NAME = aReader[1].ToString();
                autonum.FORM_CODE = aReader[2].ToString();
                autonum.PREFIX = aReader[3].ToString();
                autonum.START = Convert.ToInt32(aReader[4]);
                autonum.DIGIT = Convert.ToInt32(aReader[5]);
                autonum.INITIAL_AUTO_NUMBER = (InitialAutoNumberSetup)Enum.Parse(typeof(InitialAutoNumberSetup), aReader[6].ToString());
                autonum.AUTONUMBER_SETUP_TYPE = (AutoNumberSetupType)Enum.Parse(typeof(AutoNumberSetupType), aReader[7].ToString());
                autonum.IS_TRANSACTION = Convert.ToBoolean(aReader[8]);
            }
            return autonum;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_autonumbersetup 
                (ans_entity,
                    ans_formcode, 
                    ans_prefix, 
                    ans_start,
                    ans_digit,
                    ans_initialno, 
                    ans_type, 
                    ans_istransaction) 
                VALUES ('{0}','{1}','{2}',{3},{4},'{5}','{6}',{7})",
                                          ENTITY_NAME,
                                          FORM_CODE,
                                          PREFIX,
                                          START,
                                          DIGIT,
                                          INITIAL_AUTO_NUMBER.ToString(),
                                          AUTONUMBER_SETUP_TYPE.ToString(),
                                          IS_TRANSACTION
                );
        }
        public string GetDeleteSQL()
        {
            return "delete from table_autonumbersetup where ans_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_autonumbersetup set 
                ans_entity='{0}',
                    ans_formcode='{1}', 
                    ans_prefix='{2}', 
                    ans_start={3},
                    ans_digit={4},
                    ans_initialno='{5}', 
                    ans_type='{6}', 
                    ans_istransaction={7}
                where ans_id = {8}",
                ENTITY_NAME,
                                          FORM_CODE,
                                          PREFIX,
                                          START,
                                          DIGIT,
                                          INITIAL_AUTO_NUMBER.ToString(),
                                          AUTONUMBER_SETUP_TYPE.ToString(),
                                          IS_TRANSACTION, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_autonumbersetup where ans_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_autonumbersetup where bank_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_autonumbersetup where bank_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_autonumbersetup where bank_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_autonumbersetup");
        }
        public static string GetAllSQLStatic()
        {
            return String.Format("select * from table_autonumbersetup");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_autonumbersetup p where concat(p.bank_code, p.bank_name) like '%{0}%'", find);
        }
        public static string FindByDomainName(string domainname)
        {
            return String.Format(@"SELECT * FROM table_autonumbersetup p where p.ans_entity='{0}'", domainname);
        }
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                AutoNumberSetup autonum = new AutoNumberSetup();
                autonum.ID = Convert.ToInt32(aReader[0]);
                autonum.ENTITY_NAME = aReader[1].ToString();
                autonum.FORM_CODE = aReader[2].ToString();
                autonum.PREFIX = aReader[3].ToString();
                autonum.START = Convert.ToInt32(aReader[4]);
                autonum.DIGIT = Convert.ToInt32(aReader[5]);
                autonum.INITIAL_AUTO_NUMBER = (InitialAutoNumberSetup)Enum.Parse(typeof(InitialAutoNumberSetup), aReader[6].ToString());
                autonum.AUTONUMBER_SETUP_TYPE = (AutoNumberSetupType)Enum.Parse(typeof(AutoNumberSetupType), aReader[7].ToString());
                autonum.IS_TRANSACTION = Convert.ToBoolean(aReader[8]);
                result.Add(autonum);
            }
            return result;
        }
        public static IList GetAllStatic(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                AutoNumberSetup autonum = new AutoNumberSetup();
                autonum.ID = Convert.ToInt32(aReader[0]);
                autonum.ENTITY_NAME = aReader[1].ToString();
                autonum.FORM_CODE = aReader[2].ToString();
                autonum.PREFIX = aReader[3].ToString();
                autonum.START = Convert.ToInt32(aReader[4]);
                autonum.DIGIT = Convert.ToInt32(aReader[5]);
                autonum.INITIAL_AUTO_NUMBER = (InitialAutoNumberSetup)Enum.Parse(typeof(InitialAutoNumberSetup), aReader[6].ToString());
                autonum.AUTONUMBER_SETUP_TYPE = (AutoNumberSetupType)Enum.Parse(typeof(AutoNumberSetupType), aReader[7].ToString());
                autonum.IS_TRANSACTION = Convert.ToBoolean(aReader[8]);
                result.Add(autonum);
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
            return ENTITY_NAME;
        }
        public string GetCode()
        {
            return ENTITY_NAME;
        }
        public void SetCode(string code)
        {
            ENTITY_NAME = code;
        }
        public string GetMaximumIDSQL()
        {
            return String.Format("SELECT max(ans_id) from table_autonumbersetup");
        }
    }
}
