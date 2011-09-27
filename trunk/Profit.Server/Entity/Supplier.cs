using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Supplier : Vendor , IEntity
    {
        public double CREDIT_LIMIT = 0;
        public PriceCategory PRICE_CATEGORY = null;
        public SupplierCategory SUPPLIER_CATEGORY = null;
        public bool ACTIVE = true;

        public Supplier()
        {
        }
        public Supplier(int id)
        {
            ID = id;
        }
        public Supplier(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Supplier supplier = null;
            while (aReader.Read())
            {
                supplier = new Supplier();
                supplier.ID = Convert.ToInt32(aReader[0]);
                supplier.CODE = aReader[1].ToString();
                supplier.NAME = aReader[2].ToString();
                supplier.ACTIVE = Convert.ToBoolean(aReader[3]);
                supplier.ADDRESS = aReader[4].ToString();
                supplier.CONTACT = aReader[5].ToString();
                supplier.CREDIT_LIMIT = Convert.ToDouble(aReader[6]);
                supplier.CURRENCY = new Currency(Convert.ToInt32(aReader[7]));
                supplier.SUPPLIER_CATEGORY = new SupplierCategory(Convert.ToInt32(aReader[8]));
                supplier.EMAIL = aReader[9].ToString();
                supplier.EMPLOYEE = new Employee(Convert.ToInt32(aReader[10]));
                supplier.FAX = aReader[11].ToString();
                supplier.PHONE = aReader[12].ToString();
                supplier.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(aReader[13]));
                supplier.TAX = new Tax(Convert.ToInt32(aReader[14]));
                supplier.TAX_NO = aReader[15].ToString();
                supplier.TERM_OF_PAYMENT = new TermOfPayment(Convert.ToInt32(aReader[16]));
                supplier.WEBSITE = aReader[17].ToString();
                supplier.ZIPCODE = aReader[18].ToString();
                supplier.MODIFIED_BY = aReader["modified_by"].ToString();
                supplier.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                supplier.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return supplier;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_supplier 
                (sup_code,
                sup_name,
                sup_active,
                sup_address, 
                sup_contact,    
                sup_creditlimit,
                ccy_id,
                supcat_id,
                sup_email,
                emp_id,
                sup_fax,
                sup_phone,
                pricecat_id,
                tax_id,
                sup_taxno,
                top_id,
                sup_website,
                sup_zipcode, modified_by, modified_date, modified_computer
                ) 
                VALUES (
                '{0}',
                '{1}',
                {2},
                '{3}',
                '{4}',
                '{5}',
                '{6}',
                '{7}',
                '{8}',
                '{9}',
                '{10}',
                '{11}',
                '{12}',
                '{13}',
                '{14}',
                '{15}',
                '{16}',
                '{17}',
                '{18}',
                '{19}',
                '{20}'
                )",
                CODE, 
                NAME,
                ACTIVE,
                ADDRESS,
                CONTACT,
                CREDIT_LIMIT,
                CURRENCY.ID,
                SUPPLIER_CATEGORY.ID,
                EMAIL,
                EMPLOYEE.ID,
                FAX,
                PHONE,
                PRICE_CATEGORY.ID,
                TAX.ID,
                TAX_NO,
                TERM_OF_PAYMENT.ID,
                WEBSITE,
                ZIPCODE, MODIFIED_BY, DateTime.Now.ToString(Utils.DATE_FORMAT), MODIFIED_COMPUTER_NAME);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_supplier where sup_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_supplier set 
                sup_code = '{0}', 
                sup_name ='{1}',
                sup_active={2},
                sup_address='{3}', 
                sup_contact='{4}',    
                sup_creditlimit={5},
                ccy_id={6},
                supcat_id={7},
                sup_email='{8}',
                emp_id={9},
                sup_fax='{10}',
                sup_phone='{11}',
                pricecat_id={12},
                tax_id={13},
                sup_taxno='{14}',
                top_id={15},
                sup_website='{16}',
                sup_zipcode='{17}',
                modified_by='{18}', 
                modified_date='{19}',
                modified_computer='{20}'
                where sup_id = {21}",
                CODE, 
                NAME, 
                ACTIVE,
                ADDRESS,
                CONTACT,
                CREDIT_LIMIT,
                CURRENCY.ID,
                SUPPLIER_CATEGORY.ID,
                EMAIL,
                EMPLOYEE.ID,
                FAX,
                PHONE,
                PRICE_CATEGORY.ID,
                TAX.ID,
                TAX_NO,
                TERM_OF_PAYMENT.ID,
                WEBSITE,
                ZIPCODE, 
                MODIFIED_BY, 
                DateTime.Now.ToString(Utils.DATE_FORMAT), 
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_supplier where sup_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_supplier where sup_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_supplier where sup_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_supplier where sup_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_supplier");
        }
        public static string GetAllActiveSQL()
        {
            return String.Format("select * from table_supplier where sup_active = true");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_supplier p where concat(p.sup_code, p.sup_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Supplier supplier = new Supplier();
                supplier.ID = Convert.ToInt32(aReader[0]);
                supplier.CODE = aReader[1].ToString();
                supplier.NAME = aReader[2].ToString();
                supplier.ACTIVE = Convert.ToBoolean(aReader[3]);
                supplier.ADDRESS = aReader[4].ToString();
                supplier.CONTACT = aReader[5].ToString();
                supplier.CREDIT_LIMIT = Convert.ToDouble(aReader[6]);
                supplier.CURRENCY = new Currency(Convert.ToInt32(aReader[7]));
                supplier.SUPPLIER_CATEGORY = new SupplierCategory(Convert.ToInt32(aReader[8]));
                supplier.EMAIL = aReader[9].ToString();
                supplier.EMPLOYEE = new Employee(Convert.ToInt32(aReader[10]));
                supplier.FAX = aReader[11].ToString();
                supplier.PHONE = aReader[12].ToString();
                supplier.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(aReader[13]));
                supplier.TAX = new Tax(Convert.ToInt32(aReader[14]));
                supplier.TAX_NO = aReader[15].ToString();
                supplier.TERM_OF_PAYMENT = new TermOfPayment(Convert.ToInt32(aReader[16]));
                supplier.WEBSITE = aReader[17].ToString();
                supplier.ZIPCODE = aReader[18].ToString();
                supplier.MODIFIED_BY = aReader["modified_by"].ToString();
                supplier.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                supplier.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(supplier);
            }
            return result;
        }
        public static IList GetAllTransform(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Supplier supplier = new Supplier();
                supplier.ID = Convert.ToInt32(aReader[0]);
                supplier.CODE = aReader[1].ToString();
                supplier.NAME = aReader[2].ToString();
                supplier.ACTIVE = Convert.ToBoolean(aReader[3]);
                supplier.ADDRESS = aReader[4].ToString();
                supplier.CONTACT = aReader[5].ToString();
                supplier.CREDIT_LIMIT = Convert.ToDouble(aReader[6]);
                supplier.CURRENCY = new Currency(Convert.ToInt32(aReader[7]));
                supplier.SUPPLIER_CATEGORY = new SupplierCategory(Convert.ToInt32(aReader[8]));
                supplier.EMAIL = aReader[9].ToString();
                supplier.EMPLOYEE = new Employee(Convert.ToInt32(aReader[10]));
                supplier.FAX = aReader[11].ToString();
                supplier.PHONE = aReader[12].ToString();
                supplier.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(aReader[13]));
                supplier.TAX = new Tax(Convert.ToInt32(aReader[14]));
                supplier.TAX_NO = aReader[15].ToString();
                supplier.TERM_OF_PAYMENT = new TermOfPayment(Convert.ToInt32(aReader[16]));
                supplier.WEBSITE = aReader[17].ToString();
                supplier.ZIPCODE = aReader[18].ToString();
                supplier.MODIFIED_BY = aReader["modified_by"].ToString();
                supplier.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                supplier.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(supplier);
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
            return String.Format("SELECT max(sup_id) from table_supplier");
        }
    }
}
