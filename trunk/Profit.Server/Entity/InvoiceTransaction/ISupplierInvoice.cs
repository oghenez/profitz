using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public interface ISupplierInvoiceJournalItem : IEntity
    {
        EventJournal GET_EVENT_JOURNAL { get; set; }
        void SetOSAgainstPaymentItem(IPayment pyi);
        void UnSetOSAgainstPaymentItem(IPayment pyi);
    }
    public interface ICustomerInvoiceJournalItem : IEntity
    {
        EventJournal GET_EVENT_JOURNAL { get; set; }
        void SetOSAgainstReceiptItem(IReceipt pyi);
        void UnSetOSAgainstReceiptItem(IReceipt pyi);
    }
}
