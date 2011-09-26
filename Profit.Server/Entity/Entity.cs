using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public abstract class Entity
    {
        public string MODIFIED_BY = "";
        public DateTime MODIFIED_DATE = DateTime.Now;
        public string MODIFIED_COMPUTER_NAME = "";
    }
}
