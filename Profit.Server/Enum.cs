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
        POS//, //11
        //StockTransferSubstraction,
        //StockTransferAddition
    }
    public enum StockCardEntryTypeSupplier
    {
        ALL,
        PurchaseOrder, 
        GoodReceiveNote, 
        PurchaseReturn, 
        SupplierInvoice,
        Payment,
        APDebitNote,
        SupplierOutStandingInvoice
    }
    public enum StockCardEntryTypeCustomer
    {
        ALL,
        SalesOrder,
        DeliveryOrder,
        SalesReturn,
        CustomerInvoice,
        Receipt,
        ARCreditNote,
        CustomerOutStandingInvoice,
        POS
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
        Unexpected,
        OpeningStock,
        Transfer
    }
    public enum AgainstStatus
    {
        Open,//0
        Outstanding,//1
        Close//2
    }
    public enum InitialAutoNumberSetup
    {
        None,
        Monthly,
        Yearly
    }
    public enum AutoNumberSetupType
    {
        Manual,
        Auto
    }
    public enum VendorBalanceEntryType
    {
        SupplierOutStandingInvoice,
        CustomerOutStandingInvoice,
        SupplierInvoice,
        CustomerInvoice,
        Payment,
        Receipt,
        APCreditNote,
        APDebitNote,
        ARCreditNote,
        ARDebitNote,
        POS
    }
    public enum VendorBalanceType
    {
        Supplier,
        Customer,
    }
    public enum PaymentType
    {
        Cash,
        Bank,
        //CreditCard,
       // DebitCard,
        APDebitNote
    }

    public enum ReceiptType
    {
        Cash,
        Bank,
        ARCreditNote
    }

    public enum MarkUpDownSellingPriceType
    {
        BottomPrice,
        SellingPrice,
        CostPriceRecalculate,
        CostPriceWithoutRecalculate
    }
    public enum MarkUpDownSellingPriceBaseOn
    {
        CostPrice,
        SellPrice,
    }
    public enum MarkUpDownSellingPriceMarkType
    {
        Percentage,
        Value
    }
    public enum RoundType
    {
        Up,
        Down
    }
}
