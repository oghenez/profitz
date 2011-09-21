using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public abstract class Entity
    {
        public User USER = null;
        public DateTime CREATED_DATE = DateTime.Today;
        public DateTime MODIFIED_DATE = DateTime.Today;
        public string CREATED_COMPUTER_NAME = "";
        public string MODIFIED_COMPUTER_NAME = "";
    }
}
