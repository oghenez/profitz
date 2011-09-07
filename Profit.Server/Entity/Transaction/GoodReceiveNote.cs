﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class GoodReceiveNote : Event
    {
        public AgainstStatus AGAINST_PR_STATUS = AgainstStatus.Open;
        public Supplier SUPPLIER = null;

        public GoodReceiveNote()
            : base()
        { }
        public GoodReceiveNote(int id)
            : base()
        {
            ID = id;
        }
        public void UpdateAgainstPRStatusGRN()
        {
            bool allClosed = true;
            for (int i = 0; i < EVENT_ITEMS.Count; i++)
            {
                GoodReceiveNoteItem soi = EVENT_ITEMS[i] as GoodReceiveNoteItem;
                if (soi.AGAINST_PR_STATUS == AgainstStatus.Close) continue;
                allClosed = false;
                break;
            }
            AGAINST_PR_STATUS = allClosed ? AgainstStatus.Close : AgainstStatus.Outstanding;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_goodreceivenote 
                (   grn_date,
                    grn_noticedate,
                    grn_scentrytype,
                    emp_id,
                    grn_notes,
                    grn_posted,
                    grn_eventstatus,
                    grn_againstprstatus,
                    grn_code,
                    sup_id,
                    grn_docno,
                    grn_docdate
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}','{7}','{8}',{9},'{10}','{11}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.GoodReceiveNote.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                AGAINST_PR_STATUS.ToString(),
                CODE,
                SUPPLIER == null ? 0 : SUPPLIER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT)
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_goodreceivenote set 
                    grn_date = '{0}',
                    grn_noticedate= '{1}',
                    grn_scentrytype= '{2}',
                    emp_id= {3},
                    grn_notes= '{4}',
                    grn_posted= {5},
                    grn_eventstatus= '{6}',
                    grn_againstprstatus= '{7}',
                    grn_code = '{8}',
                    sup_id = {9},
                    grn_docno = '{10}',
                    grn_docdate = '{11}'
                where grn_id = {12}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.GoodReceiveNote.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                AGAINST_PR_STATUS.ToString(),
                CODE,
                SUPPLIER == null ? 0 : SUPPLIER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT),
                ID);
        }
        public static GoodReceiveNote TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            GoodReceiveNote transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new GoodReceiveNote();
                transaction.ID = Convert.ToInt32(aReader["grn_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["grn_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["grn_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["grn_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["grn_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["grn_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["grn_eventstatus"].ToString());
                transaction.AGAINST_PR_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["grn_againstprstatus"].ToString());
                transaction.CODE = aReader["grn_code"].ToString();
                transaction.SUPPLIER = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.DOCUMENT_NO = aReader["grn_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["grn_docdate"]);
            }
            return transaction;
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                GoodReceiveNote transaction = new GoodReceiveNote();
                transaction.ID = Convert.ToInt32(aReader["grn_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["grn_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["grn_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["grn_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["grn_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["grn_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["grn_eventstatus"].ToString());
                transaction.AGAINST_PR_STATUS = (AgainstStatus)Enum.Parse(typeof(AgainstStatus), aReader["grn_againstprstatus"].ToString());
                transaction.CODE = aReader["grn_code"].ToString();
                transaction.SUPPLIER = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.DOCUMENT_NO = aReader["grn_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["grn_docdate"]);
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(grn_id) from table_goodreceivenote");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_goodreceivenote where grn_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_goodreceivenote where grn_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT grn_eventstatus from table_goodreceivenote where grn_id ={0}", id);
        }
        public static string GetBySupplierSQL(int id)
        {
            return String.Format("SELECT * from table_goodreceivenote where sup_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_goodreceivenote set 
                    grn_posted= {0},
                    grn_eventstatus= '{1}'
                where grn_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public string UpdateAgainstStatus()
        {
            return String.Format(@"update table_goodreceivenote set 
                    grn_againstprstatus = '{0}'
                where grn_id = {1}",
                          AGAINST_PR_STATUS.ToString(),
                           ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_goodreceivenote p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                INNER JOIN table_supplier s on s.sup_id = p.sup_id
                where concat(p.grn_code, e.emp_code, e.emp_name, s.sup_code, s.sup_name)
                like '%{0}%'", find);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_goodreceivenote p where p.grn_code like '%{0}%' ORDER BY p.grn_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_goodreceivenote p";
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_goodreceivenote where grn_code ='{0}'", code);
        }
    }
}
