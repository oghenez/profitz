using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class DocumentType : Entity, IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public DocumentType()
        {
        }
        public DocumentType(int id)
        {
            ID = id;
        }
        public DocumentType(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            DocumentType docType = null;
            while (aReader.Read())
            {
                docType = new DocumentType();
                docType.ID = Convert.ToInt32(aReader[0]);
                docType.CODE = aReader[1].ToString();
                docType.NAME = aReader[2].ToString();
                docType.MODIFIED_BY = aReader["modified_by"].ToString();
                docType.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                docType.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return docType;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_documenttype 
                (doctype_code,doctype_name, modified_by, modified_date, modified_computer) 
                 VALUES ('{0}','{1}', '{2}', '{3}', '{4}')",
                CODE, NAME, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_documenttype where doctype_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_documenttype set 
                doctype_code = '{0}', 
                doctype_name='{1}',
                modified_by='{2}', 
                modified_date='{3}',
                modified_computer='{4}'
                where doctype_id = {5}",
                CODE, NAME, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME, ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_documenttype where doctype_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_documenttype where doctype_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_documenttype where doctype_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_documenttype where doctype_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_documenttype");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_documenttype p where concat(p.doctype_code, p.doctype_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                DocumentType docType = new DocumentType();
                docType.ID = Convert.ToInt32(aReader[0]);
                docType.CODE = aReader[1].ToString();
                docType.NAME = aReader[2].ToString();
                docType.MODIFIED_BY = aReader["modified_by"].ToString();
                docType.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                docType.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(docType);
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
            return CODE;
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
            return String.Format("SELECT max(doctype_id) from table_documenttype");
        }
    }
}
