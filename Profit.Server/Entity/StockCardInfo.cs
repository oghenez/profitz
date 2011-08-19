﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public class StockCardInfo
    {
        public double BALANCE = 0;
        public double BOOKED = 0;
        public double BACKORDER = 0;
        public StockCardInfo() { }
        public StockCardInfo(double balance, double booked, double backorder)
        {
            BALANCE = balance;
            BOOKED = booked;
            BACKORDER = backorder;
        }
    }
}