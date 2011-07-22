using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public enum PeriodStatus
    {
        Open,
        Current,
        Close
    }
    public enum CostMethod
    {
        MovingAverage,
        FIFO
    }
}
