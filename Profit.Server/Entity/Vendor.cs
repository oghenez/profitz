using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public abstract class Vendor 
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

    }
}
