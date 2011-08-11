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
        public const string CUSTOMER_CATEGORY_REPOSITORY = "CustomerCategoryRepository";
        public const string SUPPLIER_CATEGORY_REPOSITORY = "SupplierCategoryRepository";
        public const string PRICE_CATEGORY_REPOSITORY = "PriceCategoryRepository";
        public const string TAX_REPOSITORY = "TaxRepository";
        public const string PART_GROUP_REPOSITORY = "PartGroupRepository";
        public const string WAREHOUSE_REPOSITORY = "WarehouseRepository";
        public const string PART_CATEGORY_REPOSITORY = "PartCategoryRepository";
        public const string DOC_TYPE_REPOSITORY = "DocTypeRepository";
        public const string EXCHANGE_RATE_REPOSITORY = "ExchangeRateRepository";
        public const string CUSTOMER_REPOSITORY = "CustomerRepository";
        public const string SUPPLIER_REPOSITORY = "SupplierRepository";
        public const string YEAR_REPOSITORY = "YearRepository";
        public const string PART_REPOSITORY = "PartRepository";
        public const string STOCKTAKING_REPOSITORY = "StockTakingRepository";
        public const string USER_REPOSITORY = "UserRepository";
        public const string GENERAL_SETUP_REPOSITORY = "GeneralSetupRepository";
        public const string PURCHASEORDER_REPOSITORY = "PurchaseOrderRepository";
        public const string PERIOD_REPOSITORY = "PeriodRepository";
        public const string USER_SETTING_REPOSITORY = "UserSettingRepository";
        public const string GOODRECEIVENOTE_REPOSITORY = "GoodReceiveNoteRepository";



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
            Repository bankRepository = new Repository(new Bank());
            Repository ccyRepository = new Repository(new Currency());
            Repository divRepository = new Repository(new Division());
            Repository empRepository = new EmployeeRepository(new Employee());
            Repository topRepository = new Repository(new TermOfPayment());
            Repository unitRepository = new Repository(new Unit());
            Repository cuscatRepository = new Repository(new CustomerCategory());
            Repository supcatRepository = new Repository(new SupplierCategory());
            Repository pricecatRepository = new Repository(new PriceCategory());
            Repository taxRepository = new Repository(new Tax());
            Repository prtGroupRepository = new Repository(new PartGroup());
            Repository warehouseRepository = new Repository(new Warehouse());
            Repository prtCategoryRepository = new Repository(new PartCategory());
            Repository docTypeRepository = new Repository(new DocumentType());
            Repository excRateRepository = new Repository(new ExchangeRate());
            Repository customerRepository = new Repository(new Customer());
            Repository supplierRepository = new Repository(new Supplier());
            Repository yearRepository = new YearRepository();
            Repository partRepository = new PartRepository(new Part());
            StockTakingRepository stocktakingRepository = new StockTakingRepository();
            UserRepository userRepository = new UserRepository(new User());
            GeneralSetupRepository generalsetupRepository = new GeneralSetupRepository();
            PurchaseOrderRepository poRepository = new PurchaseOrderRepository();
            PeriodRepository periodRepository = new PeriodRepository();
            UserSettingsRepository usRepository = new UserSettingsRepository();
            GoodReceiveNoteRepository grnRepository = new GoodReceiveNoteRepository();


            m_listService.Add(BANK_REPOSITORY, bankRepository);
            m_listService.Add(CURRENCY_REPOSITORY, ccyRepository);
            m_listService.Add(DIVISION_REPOSITORY, divRepository);
            m_listService.Add(EMPLOYEE_REPOSITORY, empRepository);
            m_listService.Add(TOP_REPOSITORY, topRepository);
            m_listService.Add(UNIT_REPOSITORY, unitRepository);
            m_listService.Add(CUSTOMER_CATEGORY_REPOSITORY, cuscatRepository);
            m_listService.Add(SUPPLIER_CATEGORY_REPOSITORY, supcatRepository);
            m_listService.Add(PRICE_CATEGORY_REPOSITORY, pricecatRepository);
            m_listService.Add(TAX_REPOSITORY, taxRepository);
            m_listService.Add(PART_GROUP_REPOSITORY, prtGroupRepository);
            m_listService.Add(WAREHOUSE_REPOSITORY, warehouseRepository);
            m_listService.Add(PART_CATEGORY_REPOSITORY, prtCategoryRepository);
            m_listService.Add(DOC_TYPE_REPOSITORY, docTypeRepository);
            m_listService.Add(EXCHANGE_RATE_REPOSITORY, excRateRepository);
            m_listService.Add(CUSTOMER_REPOSITORY, customerRepository);
            m_listService.Add(SUPPLIER_REPOSITORY, supplierRepository);
            m_listService.Add(YEAR_REPOSITORY, yearRepository);
            m_listService.Add(PART_REPOSITORY, partRepository);
            m_listService.Add(STOCKTAKING_REPOSITORY, stocktakingRepository);
            m_listService.Add(USER_REPOSITORY, userRepository);
            m_listService.Add(GENERAL_SETUP_REPOSITORY, generalsetupRepository);
            m_listService.Add(PURCHASEORDER_REPOSITORY, poRepository);
            m_listService.Add(PERIOD_REPOSITORY, periodRepository);
            m_listService.Add(USER_SETTING_REPOSITORY, usRepository);
            m_listService.Add(GOODRECEIVENOTE_REPOSITORY, grnRepository);

        }
        public Repository GetRepository(string name)
        {
            return (Repository)m_listService[name];
        }
        public TransactionRepository GetTransactionRepository(string name)
        {
            return (TransactionRepository)m_listService[name];
        }
        public UserSettingsRepository UserSetting()
        {
            return (UserSettingsRepository)m_listService[USER_SETTING_REPOSITORY];
        }
    }
}
