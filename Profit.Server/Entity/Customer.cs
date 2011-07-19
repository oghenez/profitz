using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Customer : IEntity
    {
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public string ADDRESS = "";
        public string ZIPCODE = "";
        public string CONTACT = "";
        public string PHONE = "";
        public string FAX = "";
        public string EMAIL = "";
        public string WEBSITE = "";
        public TermOfPayment TERM_OF_PAYMENT = null;
        public Employee EMPLOYEE = null;
        public Tax TAX = null;
        public Currency CURRENCY = null;
        public string TAX_NO = "";
        public double CREDIT_LIMIT = 0;
        public PriceCategory PRICE_CATEGORY = null;
        public CustomerCategory CUSTOMER_CATEGORY = null;
        public bool ACTIVE = true;

        public Customer()
        {
        }
        public Customer(int id)
        {
            ID = id;
        }
        public Customer(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(OdbcDataReader aReader)
        {
            Customer customer = null;
            while (aReader.Read())
            {
                customer = new Customer();
                customer.ID = Convert.ToInt32(aReader[0]);
                customer.CODE = aReader[1].ToString();
                customer.NAME = aReader[2].ToString();
                customer.ACTIVE = Convert.ToBoolean(aReader[3]);
                customer.ADDRESS = aReader[4].ToString();
                customer.CONTACT = aReader[5].ToString();
                customer.CREDIT_LIMIT = Convert.ToDouble(aReader[6]);
                customer.CURRENCY = new Currency(Convert.ToInt32(aReader[7]));
                customer.CUSTOMER_CATEGORY = new CustomerCategory(Convert.ToInt32(aReader[8]));
                customer.EMAIL = aReader[9].ToString();
                customer.EMPLOYEE = new Employee(Convert.ToInt32(aReader[10]));
                customer.FAX = aReader[11].ToString();
                customer.PHONE = aReader[12].ToString();
                customer.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(aReader[13]));
                customer.TAX = new Tax(Convert.ToInt32(aReader[14]));
                customer.TAX_NO = aReader[15].ToString();
                customer.TERM_OF_PAYMENT = new TermOfPayment(Convert.ToInt32(aReader[16]));
                customer.WEBSITE = aReader[17].ToString();
                customer.ZIPCODE = aReader[18].ToString();
            }
            return customer;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_customer 
                (cus_code,
                cus_name,
                cus_active,
                cus_address, 
                cus_contact,    
                cus_creditlimit,
                ccy_id,
                cuscat_id,
                cus_email,
                emp_id,
                cus_fax,
                cus_phone,
                pricecat_id,
                tax_id,
                cus_taxno,
                top_id,
                cus_website,
                cus_zipcode
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
                '{17}'
                )",
                CODE, 
                NAME,
                ACTIVE,
                ADDRESS,
                CONTACT,
                CREDIT_LIMIT,
                CURRENCY.ID,
                CUSTOMER_CATEGORY.ID,
                EMAIL,
                EMPLOYEE.ID,
                FAX,
                PHONE,
                PRICE_CATEGORY.ID,
                TAX.ID,
                TAX_NO,
                TERM_OF_PAYMENT.ID,
                WEBSITE,
                ZIPCODE);
        }
        public string GetDeleteSQL()
        {
            return "delete from table_customer where cus_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_customer set 
                cus_code = '{0}', 
                cus_name ='{1}',
                cus_active={2},
                cus_address='{3}', 
                cus_contact='{4}',    
                cus_creditlimit={5},
                ccy_id={6},
                cuscat_id={7},
                cus_email='{8}',
                emp_id={9},
                cus_fax='{10}',
                cus_phone='{11}',
                pricecat_id={12},
                tax_id={13},
                cus_taxno='{14}',
                top_id={15},
                cus_website='{16}',
                cus_zipcode='{17}'
                where cus_id = {18}",
                CODE, 
                NAME, 
                ACTIVE,
                ADDRESS,
                CONTACT,
                CREDIT_LIMIT,
                CURRENCY.ID,
                CUSTOMER_CATEGORY.ID,
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
                ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_customer where cus_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_customer where cus_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_customer where cus_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_customer where cus_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_customer");
        }
        public IList GetAll(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Customer customer = new Customer();
                customer.ID = Convert.ToInt32(aReader[0]);
                customer.CODE = aReader[1].ToString();
                customer.NAME = aReader[2].ToString();
                customer.ACTIVE = Convert.ToBoolean(aReader[3]);
                customer.ADDRESS = aReader[4].ToString();
                customer.CONTACT = aReader[5].ToString();
                customer.CREDIT_LIMIT = Convert.ToDouble(aReader[6]);
                customer.CURRENCY = new Currency(Convert.ToInt32(aReader[7]));
                customer.CUSTOMER_CATEGORY = new CustomerCategory(Convert.ToInt32(aReader[8]));
                customer.EMAIL = aReader[9].ToString();
                customer.EMPLOYEE = new Employee(Convert.ToInt32(aReader[10]));
                customer.FAX = aReader[11].ToString();
                customer.PHONE = aReader[12].ToString();
                customer.PRICE_CATEGORY = new PriceCategory(Convert.ToInt32(aReader[13]));
                customer.TAX = new Tax(Convert.ToInt32(aReader[14]));
                customer.TAX_NO = aReader[15].ToString();
                customer.TERM_OF_PAYMENT = new TermOfPayment(Convert.ToInt32(aReader[16]));
                customer.WEBSITE = aReader[17].ToString();
                customer.ZIPCODE = aReader[18].ToString();
                result.Add(customer);
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
            return String.Format("SELECT max(cus_id) from table_customer");
        }
    }
}
