using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profit.Server;

namespace Profit.Test
{
    public class GoodReceiveNoteTest
    {
        PurchaseOrderRepository m_poRep = new PurchaseOrderRepository();
        GoodReceiveNoteRepository m_grnRep = new GoodReceiveNoteRepository();

        public GoodReceiveNoteTest()
        {
            //TestCreate();
            //GoodReceiveNote grn = (GoodReceiveNote)m_grnRep.Get(3);
            //m_grnRep.Confirm(3);
            //m_grnRep.Revise(3);
        }
        private void TestCreate()
        {
            PurchaseOrder po = (PurchaseOrder)m_poRep.Get(5);
            GoodReceiveNote grn = new GoodReceiveNote();
            grn.CODE = "GRN001";
            grn.EMPLOYEE = new Employee(1);
            grn.EVENT_STATUS = EventStatus.Entry;
            grn.NOTES = "TEST IN GRN";
            grn.NOTICE_DATE = DateTime.Today;
            grn.STOCK_CARD_ENTRY_TYPE = StockCardEntryType.GoodReceiveNote;
            grn.TRANSACTION_DATE = DateTime.Today;

            GoodReceiveNoteItem grni1 = new GoodReceiveNoteItem();
            grni1.EVENT = grn;
            grni1.NOTES = "TEST GRN ITEM 1";
            grni1.PART = new Part(9070);
            grni1.PO_ITEM = (PurchaseOrderItem)po.EVENT_ITEMS[0];
            grni1.QYTAMOUNT = 13000;
            grni1.RETURNED_AMOUNT = 0;
            grni1.STOCK_CARD_ENTRY_TYPE = StockCardEntryType.GoodReceiveNote;
            grni1.UNIT = new Unit(1);
            grni1.WAREHOUSE = new Warehouse(1);

            GoodReceiveNoteItem grni2 = new GoodReceiveNoteItem();
            grni2.EVENT = grn;
            grni2.NOTES = "TEST GRN ITEM 1";
            grni2.PART = new Part(9071);
            grni2.PO_ITEM = (PurchaseOrderItem)po.EVENT_ITEMS[1];
            grni2.QYTAMOUNT = 1254;
            grni2.RETURNED_AMOUNT = 0;
            grni2.STOCK_CARD_ENTRY_TYPE = StockCardEntryType.GoodReceiveNote;
            grni2.UNIT = new Unit(1);
            grni2.WAREHOUSE = new Warehouse(1);

            grn.EVENT_ITEMS.Add(grni1);
            grn.EVENT_ITEMS.Add(grni2);

            m_grnRep.Save(grn);
            
        }
    }
}
  

