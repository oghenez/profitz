using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profit.Server;

namespace Profit.Test
{
    public class PurchaseOrderTest
    {
        PurchaseOrder m_po;
        PurchaseOrderRepository m_rep;

        public PurchaseOrderTest()
        {
            
            m_rep = new PurchaseOrderRepository();

            //TestCreate();
            TestGet();
        }

        private void TestGet()
        {
            TestCreate();
            //m_po = (PurchaseOrder)m_rep.Get(3);
            m_rep.Confirm(m_po.ID);
            //m_rep.Confirm(4);
            //m_po = (PurchaseOrder)m_rep.Get(4);
            //m_rep.Revise(m_po.ID);
            //m_po = (PurchaseOrder)m_rep.Get(4);
            //m_rep.Delete(m_po);
        }

        private void TestCreate()
        {
            m_po = new PurchaseOrder();
            m_po.CODE = "PO00001";
            m_po.CURRENCY = new Currency(1);
            m_po.DISC_AFTER_AMOUNT = 10000;
            m_po.DISC_AMOUNT = 1000;
            m_po.DISC_PERCENT = 10;
            m_po.DIVISION = new Division(1);
            m_po.DUE_DATE = DateTime.Today.AddDays(30);
            m_po.EMPLOYEE = new Employee(1);
            m_po.EVENT_STATUS = EventStatus.Entry;
            m_po.AGAINST_GRN_STATUS = AgainstStatus.Open;
            m_po.NET_TOTAL = 2000000;
            m_po.NOTES = "test test PO";
            m_po.NOTICE_DATE = DateTime.Today;
            m_po.OTHER_EXPENSE = 3000;
            m_po.STOCK_CARD_ENTRY_TYPE = StockCardEntryType.PurchaseOrder;
            m_po.SUB_TOTAL = 3000;
            m_po.TAX = new Tax(1);
            m_po.TAX_AFTER_AMOUNT = 3200;
            m_po.TOP = new TermOfPayment(1);
            m_po.TRANSACTION_DATE = DateTime.Today;

            PurchaseOrderItem poi1 = new PurchaseOrderItem();
            poi1.AGAINST_GRN_STATUS = AgainstStatus.Open;
            poi1.QYTAMOUNT = 13000;
            poi1.DISC_A = 1;
            poi1.DISC_ABC = "1+2+3";
            poi1.DISC_AMOUNT = 200;
            poi1.DISC_B = 2;
            poi1.DISC_C = 3;
            poi1.DISC_PERCENT = 10;
            poi1.EVENT = m_po;
            poi1.NOTES = "TEST PO ITEM 1";
            poi1.OUTSTANDING_AMOUNT_TO_GRN = 0;
            poi1.PART = new Part(9070);
            poi1.PRICE = 3000;
            poi1.RECEIVED_AMOUNT = 0;
            poi1.STOCK_CARD_ENTRY_TYPE = StockCardEntryType.PurchaseOrder;
            poi1.TOTAL_DISCOUNT = 340;
            poi1.UNIT = new Unit(1);
            poi1.WAREHOUSE = new Warehouse(1);
            poi1.SUBTOTAL = 34000;

            PurchaseOrderItem poi2 = new PurchaseOrderItem();
            poi2.AGAINST_GRN_STATUS = AgainstStatus.Open;
            poi2.QYTAMOUNT = 132551;
            poi2.DISC_A = 1;
            poi2.DISC_ABC = "1+2+3";
            poi2.DISC_AMOUNT = 321;
            poi2.DISC_B = 2;
            poi2.DISC_C = 3;
            poi2.DISC_PERCENT = 15;
            poi2.EVENT = m_po;
            poi2.NOTES = "TEST PO ITEM 1";
            poi2.OUTSTANDING_AMOUNT_TO_GRN = 0;
            poi2.PART = new Part(9071);
            poi2.PRICE = 3412;
            poi2.RECEIVED_AMOUNT = 0;
            poi2.STOCK_CARD_ENTRY_TYPE = StockCardEntryType.PurchaseOrder;
            poi2.TOTAL_DISCOUNT = 331;
            poi2.UNIT = new Unit(1);
            poi2.WAREHOUSE = new Warehouse(1);
            poi2.SUBTOTAL = 18000;

            m_po.EVENT_ITEMS.Add(poi1);
            m_po.EVENT_ITEMS.Add(poi2);

            m_rep.Save(m_po);
        }
    }
}
