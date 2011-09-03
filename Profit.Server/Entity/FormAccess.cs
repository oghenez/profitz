using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class FormAccess : IEntity
    {
   
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public bool SAVE = true;
        public bool DELETE = true;
        public bool VIEW = true;
        public bool POST = true;
        public bool PRINT = true;
        public User USER = null;
        public bool UPDATED = false;
        public FormAccess()
        {
        }
        public FormAccess(int id)
        {
            ID = id;
        }
        public FormAccess(int id, string code, string name)
        {
            ID = id;
            CODE = code;
            NAME = name;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            FormAccess formaccess = null;
            while (aReader.Read())
            {
                formaccess = new FormAccess();
                formaccess.ID = Convert.ToInt32(aReader[0]);
                formaccess.CODE = aReader[1].ToString();
                formaccess.NAME = aReader[2].ToString();
                formaccess.SAVE = Convert.ToBoolean(aReader[3]);
                formaccess.DELETE = Convert.ToBoolean(aReader[4]);
                formaccess.VIEW = Convert.ToBoolean(aReader[5]);
                formaccess.POST = Convert.ToBoolean(aReader[6]);
                formaccess.PRINT = Convert.ToBoolean(aReader[7]);
                formaccess.USER = new User(Convert.ToInt32(aReader[8]));
            }
            return formaccess;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_formaccess 
                (   formaccess_code,
                    formaccess_name,
                    formaccess_save,
                    formaccess_delete,
                    formaccess_view,
                    formaccess_post,
                    formaccess_print,
                    user_id) 
                VALUES ('{0}','{1}',{2},{3},{4},{5},{6},{7})",
                CODE, NAME,SAVE,DELETE,VIEW,POST,PRINT, USER.ID);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_formaccess where formaccess_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_formaccess set 
                formaccess_code = '{0}', 
                formaccess_name='{1}',
                    formaccess_save={2},
                    formaccess_delete={3},
                    formaccess_view={4},
                    formaccess_post={5},
                    formaccess_print={6},
                    user_id={7}
                where formaccess_id = {8}",
                CODE, NAME, SAVE, DELETE, VIEW, POST, PRINT, USER.ID, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_formaccess where formaccess_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_formaccess where formaccess_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_formaccess where formaccess_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_formaccess where formaccess_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_formaccess");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_formaccess p where concat(p.formaccess_code, p.formaccess_name) like '%{0}%'", find);
        }
        public static string GetAllByUserSQL(int userID)
        {
            return String.Format("select * from table_formaccess where user_id={0}",userID);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                FormAccess formaccess = new FormAccess();
                formaccess.ID = Convert.ToInt32(aReader[0]);
                formaccess.CODE = aReader[1].ToString();
                formaccess.NAME = aReader[2].ToString();
                formaccess.SAVE = Convert.ToBoolean(aReader[3]);
                formaccess.DELETE = Convert.ToBoolean(aReader[4]);
                formaccess.VIEW = Convert.ToBoolean(aReader[5]);
                formaccess.POST = Convert.ToBoolean(aReader[6]);
                formaccess.PRINT = Convert.ToBoolean(aReader[7]);
                formaccess.USER = new User(Convert.ToInt32(aReader[8]));
                result.Add(formaccess);
            }
            return result;
        }
        public static  IList GetAllStatic(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                FormAccess formaccess = new FormAccess();
                formaccess.ID = Convert.ToInt32(aReader[0]);
                formaccess.CODE = aReader[1].ToString();
                formaccess.NAME = aReader[2].ToString();
                formaccess.SAVE = Convert.ToBoolean(aReader[3]);
                formaccess.DELETE = Convert.ToBoolean(aReader[4]);
                formaccess.VIEW = Convert.ToBoolean(aReader[5]);
                formaccess.POST = Convert.ToBoolean(aReader[6]);
                formaccess.PRINT = Convert.ToBoolean(aReader[7]);
                formaccess.USER = new User(Convert.ToInt32(aReader[8]));
                result.Add(formaccess);
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
            return NAME;
        }
        public string GetCode()
        {
            return CODE;
        }
        public void SetCode(string code)
        {
            CODE = code;
        }
        public string GetMaximumIDSQL()
        {
            return String.Format("SELECT max(formaccess_id) from table_formaccess");
        }
        public override bool Equals(object obj)
        {
            FormAccess o = (FormAccess)obj;
            if (o == null) return false;
            return o.ID == this.ID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
