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
    public enum StockCardEntryType
    {
        PurchaseOrder, //0
        GoodReceiveNote, //1
        SalesOrder, //2
        DeliveryOrder, //3
        OpeningStock,  //4
        StockTaking, //5
        PurchaseReturn, //6
        SalesReturn, //7
        SupplierInvoice, //8
        CustomerInvoice, //9
        Payment, //10
        POS //11
    }
    public enum EventStatus
    {
        Entry,
        Confirm
    }
    public enum StockTakingType
    {
        Adjustment,
        Sample,
        Conversion,
        Unexpected
    }
}
