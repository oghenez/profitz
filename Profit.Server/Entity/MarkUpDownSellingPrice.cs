using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public class MarkUpDownSellingPrice
    {
        public MarkUpDownSellingPriceType MARK_TYPE = MarkUpDownSellingPriceType.BottomPrice;
        public MarkUpDownSellingPriceBaseOn MARK_BASE_ON = MarkUpDownSellingPriceBaseOn.CostPrice;
        public MarkUpDownSellingPriceMarkType MARK_MARK_TYPE = MarkUpDownSellingPriceMarkType.Percentage;
        public Currency CURRENCY = null;
        public PartGroup PART_GROUP = null;
        public PartCategory PART_CATEGORY = null;
        public Part PART = null;
        public PriceCategory PRICE_CATEGORY = null;
        public RoundType ROUND_TYPE = RoundType.Up;
        public double ROUNDING = 2;
        public double VALUE = 0;
        public double PERCENTAGE = 0;
        //private DateTime m_lastUpdate;
        //private string m_userName = string.Empty;
        //private string m_computerName = string.Empty;
    }
}
