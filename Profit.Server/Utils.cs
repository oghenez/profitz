using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace Profit.Server
{
    public class Utils
    {
        public static void GetListCode(StringCollection list, IList dtos)
        {
            foreach (IEntity cod in dtos)
            {
                list.Add(cod.ToString());
            }
        }
        public static IEntity FindEntityInList(string value, IList list)
        {
            foreach (IEntity cod in list)
            {
                if (cod.ToString() == value)
                    return cod;
            }
            return null;
        }
    }
}
