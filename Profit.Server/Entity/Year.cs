using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Year : Entity, IEntity
    {
        string DATE_FORMAT = "yyyy/MM/dd";
        public int ID = 0;
        public string CODE = "B001";
        public string NAME = "";
        public DateTime START_DATE;
        public DateTime END_DATE;
        public IList PERIODS = new ArrayList();

        public Year()
        {
        }
        public Year(int id)
        {
            ID = id;
        }
        public Year(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Year year = null;
            while (aReader.Read())
            {
                year = new Year();
                year.ID = Convert.ToInt32(aReader[0]);
                year.CODE = aReader[1].ToString();
                year.NAME = aReader[2].ToString();
                year.START_DATE = Convert.ToDateTime(aReader[3]);
                year.END_DATE = Convert.ToDateTime(aReader[4]);
                year.MODIFIED_BY = aReader["modified_by"].ToString();
                year.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                year.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return year;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_year 
                (
year_code,
year_name,
year_start,
year_end, modified_by, modified_date, modified_computer
) 
                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                CODE, 
                NAME,
                START_DATE.ToString(DATE_FORMAT),
                END_DATE.ToString(DATE_FORMAT), 
                MODIFIED_BY, 
                DateTime.Now.ToString(Utils.DATE_FORMAT), 
                MODIFIED_COMPUTER_NAME
                );
        }
        public string GetDeleteSQL()
        {
            return "delete from table_year where year_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_year set 
                year_code = '{0}', 
                year_name='{1}',
                year_start='{2}',
                year_end='{3}',
                modified_by='{4}', 
                modified_date='{5}',
                modified_computer='{6}'
                where year_id = {7}",
                CODE, 
                NAME, 
                START_DATE.ToString(DATE_FORMAT),
                END_DATE.ToString(DATE_FORMAT), 
                MODIFIED_BY, 
                DateTime.Now.ToString(Utils.DATE_FORMAT), 
                MODIFIED_COMPUTER_NAME,
                ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_year where year_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_year where year_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_year where year_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_year where year_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_year");
        }
        public string GetConcatSearch(string find)
        {
            return String.Format(@"SELECT * FROM table_year p where concat(p.year_code, p.year_name) like '%{0}%'", find);
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Year year = new Year();
                year.ID = Convert.ToInt32(aReader[0]);
                year.CODE = aReader[1].ToString();
                year.NAME = aReader[2].ToString();
                year.START_DATE = Convert.ToDateTime(aReader[3]);
                year.END_DATE = Convert.ToDateTime(aReader[4]);
                year.MODIFIED_BY = aReader["modified_by"].ToString();
                year.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                year.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(year);
            }
            return result;
        }
        public int GetID()
        {
            return ID;
        }
        public void SetID(int id)
        {
            ID = id;
        }
        public override string ToString()
        {
            return CODE;
        }
        public string GetCode()
        {
            return CODE;
        }
        public void SetCode(string code)
        {
            CODE = code;
        }
        public string GetMaximumIDSQL()
        {
            return String.Format("SELECT max(year_id) from table_year");
        }
        public void GeneratePeriods()
        {
            this.PERIODS.Clear();
            DateTime startDate = this.START_DATE;
            for (short seqId = 1; seqId <= 12; seqId++)
            {
                string pCode = this.CODE + seqId.ToString("00");
                Period period = new Period(pCode, PeriodStatus.Open, this, startDate, getEndPeriod(startDate));
                this.PERIODS.Add(period);
                startDate = period.START_DATE.AddMonths(1);
            }
        }
        private DateTime getEndPeriod(DateTime startPeriod)
        {
            return startPeriod.AddMonths(1).AddDays(-1);
        }
    }
}
