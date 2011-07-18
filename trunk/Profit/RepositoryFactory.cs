using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Odbc;
using Profit.Server;

namespace Profit
{
    public class RepositoryFactory
    {
        Hashtable m_listService = new Hashtable();
        static volatile RepositoryFactory m_instance;
        public const string BANK_REPOSITORY = "BankRepository";
        public const string CURRENCY_REPOSITORY = "CurrencyRepository";
        public const string DIVISION_REPOSITORY = "DivisionRepository";
        public const string EMPLOYEE_REPOSITORY = "EmployeeRepository";
        public const string TOP_REPOSITORY = "TOPRepository";
        public const string UNIT_REPOSITORY = "UnitRepository";

        public static RepositoryFactory GetInstance()
        {
            if (m_instance == null)
            {
                m_instance = new RepositoryFactory();

            }
            return m_instance;
        }
        public RepositoryFactory()
        {
            Repository bankRepository = new Repository();
            Repository ccyRepository = new Repository();
            Repository divRepository = new Repository();
            Repository empRepository = new Repository();
            Repository topRepository = new Repository();
            Repository unitRepository = new Repository();

            m_listService.Add(BANK_REPOSITORY, bankRepository);
            m_listService.Add(CURRENCY_REPOSITORY, ccyRepository);
            m_listService.Add(DIVISION_REPOSITORY, divRepository);
            m_listService.Add(EMPLOYEE_REPOSITORY, empRepository);
            m_listService.Add(TOP_REPOSITORY, topRepository);
            m_listService.Add(UNIT_REPOSITORY, topRepository);
        }
        public Repository GetRepository(string name)
        {
            return (Repository)m_listService[name];
        }
    }
}
