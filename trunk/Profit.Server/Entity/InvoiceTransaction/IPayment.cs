using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public interface IPayment
    {
        double GET_AMOUNT { get; }
    }
    public interface IReceipt
    {
        double GET_AMOUNT { get; }
    }
}
