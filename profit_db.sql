-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.0.83-community-nt


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema profit_db
--

CREATE DATABASE IF NOT EXISTS profit_db;
USE profit_db;

--
-- Definition of table `table_bank`
--

DROP TABLE IF EXISTS `table_bank`;
CREATE TABLE `table_bank` (
  `bank_id` int(10) unsigned NOT NULL auto_increment,
  `bank_code` varchar(45) NOT NULL,
  `bank_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`bank_id`),
  UNIQUE KEY `Index_2` (`bank_code`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_bank`
--

/*!40000 ALTER TABLE `table_bank` DISABLE KEYS */;
INSERT INTO `table_bank` (`bank_id`,`bank_code`,`bank_name`) VALUES 
 (2,'BCA','BCA cabang Jodoh'),
 (3,'BTN','Bank Tabungan Negara Tiban'),
 (4,'DNM','Danamon Bank'),
 (5,'MGA','Bank Mega'),
 (6,'INDX','Bank Index Cabang Jodoh'),
 (8,'BRI','BRI cabang batam');
/*!40000 ALTER TABLE `table_bank` ENABLE KEYS */;


--
-- Definition of table `table_currency`
--

DROP TABLE IF EXISTS `table_currency`;
CREATE TABLE `table_currency` (
  `ccy_id` int(10) unsigned NOT NULL auto_increment,
  `ccy_code` varchar(45) NOT NULL,
  `ccy_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`ccy_id`),
  UNIQUE KEY `Index_2` (`ccy_code`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_currency`
--

/*!40000 ALTER TABLE `table_currency` DISABLE KEYS */;
INSERT INTO `table_currency` (`ccy_id`,`ccy_code`,`ccy_name`) VALUES 
 (1,'IDR','Indonesian Rupiah'),
 (2,'SGD','Singapore Dollar'),
 (3,'USD','US Dollar');
/*!40000 ALTER TABLE `table_currency` ENABLE KEYS */;


--
-- Definition of table `table_customer`
--

DROP TABLE IF EXISTS `table_customer`;
CREATE TABLE `table_customer` (
  `cus_id` int(10) unsigned NOT NULL auto_increment,
  `cus_code` varchar(45) NOT NULL,
  `cus_name` varchar(45) NOT NULL,
  `cus_active` tinyint(1) NOT NULL,
  `cus_address` text NOT NULL,
  `cus_contact` varchar(45) NOT NULL,
  `cus_creditlimit` double NOT NULL,
  `ccy_id` int(10) unsigned NOT NULL,
  `cuscat_id` int(10) unsigned NOT NULL,
  `cus_email` varchar(45) NOT NULL,
  `emp_id` int(10) unsigned NOT NULL,
  `cus_fax` varchar(45) NOT NULL,
  `cus_phone` varchar(45) NOT NULL,
  `pricecat_id` int(10) unsigned NOT NULL,
  `tax_id` int(10) unsigned NOT NULL,
  `cus_taxno` varchar(45) NOT NULL,
  `top_id` int(10) unsigned NOT NULL,
  `cus_website` varchar(45) NOT NULL,
  `cus_zipcode` varchar(45) NOT NULL,
  PRIMARY KEY  (`cus_id`),
  UNIQUE KEY `Index_2` (`cus_code`),
  KEY `FK_table_customer_1` (`ccy_id`),
  KEY `FK_table_customer_2` (`cuscat_id`),
  KEY `FK_table_customer_3` (`emp_id`),
  KEY `FK_table_customer_4` (`pricecat_id`),
  KEY `FK_table_customer_5` (`tax_id`),
  KEY `FK_table_customer_6` (`top_id`),
  CONSTRAINT `FK_table_customer_1` FOREIGN KEY (`ccy_id`) REFERENCES `table_currency` (`ccy_id`),
  CONSTRAINT `FK_table_customer_2` FOREIGN KEY (`cuscat_id`) REFERENCES `table_customercategory` (`cuscat_id`),
  CONSTRAINT `FK_table_customer_3` FOREIGN KEY (`emp_id`) REFERENCES `table_employee` (`emp_id`),
  CONSTRAINT `FK_table_customer_4` FOREIGN KEY (`pricecat_id`) REFERENCES `table_pricecategory` (`pricecat_id`),
  CONSTRAINT `FK_table_customer_5` FOREIGN KEY (`tax_id`) REFERENCES `table_tax` (`tax_id`),
  CONSTRAINT `FK_table_customer_6` FOREIGN KEY (`top_id`) REFERENCES `table_termofpayment` (`top_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_customer`
--

/*!40000 ALTER TABLE `table_customer` DISABLE KEYS */;
INSERT INTO `table_customer` (`cus_id`,`cus_code`,`cus_name`,`cus_active`,`cus_address`,`cus_contact`,`cus_creditlimit`,`ccy_id`,`cuscat_id`,`cus_email`,`emp_id`,`cus_fax`,`cus_phone`,`pricecat_id`,`tax_id`,`cus_taxno`,`top_id`,`cus_website`,`cus_zipcode`) VALUES 
 (1,'CUS001','PT. MASUK ANGIN',0,'Jl. Imam bonjol BATAM ','2',15000000,2,1,'4',1,'6','3',1,1,'7',1,'5','1'),
 (2,'DDW','Drydocks Word Pertama',1,'Tanjung uncang','Mark',500000,3,4,'dfsf.com',1,'43244','321321',3,2,'1132145',2,'fewfe.com','12345'),
 (3,'POS','POS Customer',0,'','',0,1,1,'',2,'','',1,2,'',2,'','');
/*!40000 ALTER TABLE `table_customer` ENABLE KEYS */;


--
-- Definition of table `table_customercategory`
--

DROP TABLE IF EXISTS `table_customercategory`;
CREATE TABLE `table_customercategory` (
  `cuscat_id` int(10) unsigned NOT NULL auto_increment,
  `cuscat_code` varchar(45) NOT NULL,
  `cuscat_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`cuscat_id`),
  UNIQUE KEY `Index_2` (`cuscat_code`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_customercategory`
--

/*!40000 ALTER TABLE `table_customercategory` DISABLE KEYS */;
INSERT INTO `table_customercategory` (`cuscat_id`,`cuscat_code`,`cuscat_name`) VALUES 
 (1,'CUS001','MINI MARKET'),
 (2,'CUS002','SUPER MARKET'),
 (3,'CUS003','HYPER MARKET'),
 (4,'CUS004','DEPARTMENT STORE');
/*!40000 ALTER TABLE `table_customercategory` ENABLE KEYS */;


--
-- Definition of table `table_division`
--

DROP TABLE IF EXISTS `table_division`;
CREATE TABLE `table_division` (
  `div_id` int(10) unsigned NOT NULL auto_increment,
  `div_code` varchar(45) NOT NULL,
  `div_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`div_id`),
  UNIQUE KEY `Index_2` (`div_code`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_division`
--

/*!40000 ALTER TABLE `table_division` DISABLE KEYS */;
INSERT INTO `table_division` (`div_id`,`div_code`,`div_name`) VALUES 
 (1,'DIV001','Information Technology'),
 (6,'COM','COMMERCIAL');
/*!40000 ALTER TABLE `table_division` ENABLE KEYS */;


--
-- Definition of table `table_documenttype`
--

DROP TABLE IF EXISTS `table_documenttype`;
CREATE TABLE `table_documenttype` (
  `doctype_id` int(10) unsigned NOT NULL auto_increment,
  `doctype_code` varchar(45) NOT NULL,
  `doctype_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`doctype_id`),
  UNIQUE KEY `Index_2` (`doctype_code`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_documenttype`
--

/*!40000 ALTER TABLE `table_documenttype` DISABLE KEYS */;
INSERT INTO `table_documenttype` (`doctype_id`,`doctype_code`,`doctype_name`) VALUES 
 (1,'DOC001','Document 001'),
 (2,'DOC002','DOCUMENT 002');
/*!40000 ALTER TABLE `table_documenttype` ENABLE KEYS */;


--
-- Definition of table `table_employee`
--

DROP TABLE IF EXISTS `table_employee`;
CREATE TABLE `table_employee` (
  `emp_id` int(10) unsigned NOT NULL auto_increment,
  `emp_code` varchar(45) NOT NULL,
  `emp_name` varchar(45) NOT NULL,
  `emp_salesman` tinyint(1) NOT NULL,
  `emp_storeman` tinyint(1) NOT NULL,
  `emp_purchaser` tinyint(1) NOT NULL,
  PRIMARY KEY  (`emp_id`),
  UNIQUE KEY `Index_2` (`emp_code`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_employee`
--

/*!40000 ALTER TABLE `table_employee` DISABLE KEYS */;
INSERT INTO `table_employee` (`emp_id`,`emp_code`,`emp_name`,`emp_salesman`,`emp_storeman`,`emp_purchaser`) VALUES 
 (1,'DWP02825','Dodo',1,0,0),
 (2,'DWP00533','RADY',1,1,1),
 (6,'DWP00165','SEBASTIAN',0,0,1);
/*!40000 ALTER TABLE `table_employee` ENABLE KEYS */;


--
-- Definition of table `table_exchangerate`
--

DROP TABLE IF EXISTS `table_exchangerate`;
CREATE TABLE `table_exchangerate` (
  `excrate_id` int(10) unsigned NOT NULL auto_increment,
  `excrate_code` varchar(45) NOT NULL,
  `excrate_start` datetime NOT NULL,
  `excrate_end` datetime NOT NULL,
  `excrate_rate` double NOT NULL,
  `ccy_id` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`excrate_id`),
  UNIQUE KEY `Index_2` (`excrate_code`),
  KEY `FK_table_exchangerate_1` (`ccy_id`),
  CONSTRAINT `FK_table_exchangerate_1` FOREIGN KEY (`ccy_id`) REFERENCES `table_currency` (`ccy_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_exchangerate`
--

/*!40000 ALTER TABLE `table_exchangerate` DISABLE KEYS */;
INSERT INTO `table_exchangerate` (`excrate_id`,`excrate_code`,`excrate_start`,`excrate_end`,`excrate_rate`,`ccy_id`) VALUES 
 (1,'BASE','2011-07-19 00:00:00','2011-07-30 00:00:00',1,1),
 (2,'USD','2011-07-19 00:00:00','2011-07-23 00:00:00',9000.15,3),
 (3,'SGD','2011-07-19 00:00:00','2011-08-19 00:00:00',7000,2);
/*!40000 ALTER TABLE `table_exchangerate` ENABLE KEYS */;


--
-- Definition of table `table_partcategory`
--

DROP TABLE IF EXISTS `table_partcategory`;
CREATE TABLE `table_partcategory` (
  `prtcat_id` int(10) unsigned NOT NULL auto_increment,
  `prtcat_code` varchar(45) NOT NULL,
  `prtcat_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`prtcat_id`),
  UNIQUE KEY `Index_2` (`prtcat_code`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_partcategory`
--

/*!40000 ALTER TABLE `table_partcategory` DISABLE KEYS */;
INSERT INTO `table_partcategory` (`prtcat_id`,`prtcat_code`,`prtcat_name`) VALUES 
 (1,'BSR','BESAR'),
 (2,'KCL','KECILXXX');
/*!40000 ALTER TABLE `table_partcategory` ENABLE KEYS */;


--
-- Definition of table `table_partgroup`
--

DROP TABLE IF EXISTS `table_partgroup`;
CREATE TABLE `table_partgroup` (
  `prtgroup_id` int(10) unsigned NOT NULL auto_increment,
  `prtgroup_code` varchar(45) NOT NULL,
  `prtgroup_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`prtgroup_id`),
  UNIQUE KEY `Index_2` (`prtgroup_code`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_partgroup`
--

/*!40000 ALTER TABLE `table_partgroup` DISABLE KEYS */;
INSERT INTO `table_partgroup` (`prtgroup_id`,`prtgroup_code`,`prtgroup_name`) VALUES 
 (1,'SNK','SNACK'),
 (2,'DRK','SOFT DRINK E');
/*!40000 ALTER TABLE `table_partgroup` ENABLE KEYS */;


--
-- Definition of table `table_pricecategory`
--

DROP TABLE IF EXISTS `table_pricecategory`;
CREATE TABLE `table_pricecategory` (
  `pricecat_id` int(10) unsigned NOT NULL auto_increment,
  `pricecat_code` varchar(45) NOT NULL,
  `pricecat_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`pricecat_id`),
  UNIQUE KEY `Index_2` (`pricecat_code`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_pricecategory`
--

/*!40000 ALTER TABLE `table_pricecategory` DISABLE KEYS */;
INSERT INTO `table_pricecategory` (`pricecat_id`,`pricecat_code`,`pricecat_name`) VALUES 
 (1,'NRM','NORMAL'),
 (2,'DISC10','DISCOUNT 10 %'),
 (3,'DISC20','DISCOUNT 20 %');
/*!40000 ALTER TABLE `table_pricecategory` ENABLE KEYS */;


--
-- Definition of table `table_supplier`
--

DROP TABLE IF EXISTS `table_supplier`;
CREATE TABLE `table_supplier` (
  `sup_id` int(10) unsigned NOT NULL auto_increment,
  `sup_code` varchar(45) NOT NULL,
  `sup_name` varchar(45) NOT NULL,
  `sup_active` tinyint(1) NOT NULL,
  `sup_address` text NOT NULL,
  `sup_contact` varchar(45) NOT NULL,
  `sup_creditlimit` double NOT NULL,
  `ccy_id` int(10) unsigned NOT NULL,
  `supcat_id` int(10) unsigned NOT NULL,
  `sup_email` varchar(45) NOT NULL,
  `emp_id` int(10) unsigned NOT NULL,
  `sup_fax` varchar(45) NOT NULL,
  `sup_phone` varchar(45) NOT NULL,
  `pricecat_id` int(10) unsigned NOT NULL,
  `tax_id` int(10) unsigned NOT NULL,
  `sup_taxno` varchar(45) NOT NULL,
  `top_id` int(10) unsigned NOT NULL,
  `sup_website` varchar(45) NOT NULL,
  `sup_zipcode` varchar(45) NOT NULL,
  PRIMARY KEY  (`sup_id`),
  UNIQUE KEY `Index_2` (`sup_code`),
  KEY `FK_table_supplier_1` (`ccy_id`),
  KEY `FK_table_supplier_2` (`supcat_id`),
  KEY `FK_table_supplier_3` (`emp_id`),
  KEY `FK_table_supplier_4` (`pricecat_id`),
  KEY `FK_table_supplier_5` (`tax_id`),
  KEY `FK_table_supplier_6` (`top_id`),
  CONSTRAINT `FK_table_supplier_1` FOREIGN KEY (`ccy_id`) REFERENCES `table_currency` (`ccy_id`),
  CONSTRAINT `FK_table_supplier_2` FOREIGN KEY (`supcat_id`) REFERENCES `table_suppliercategory` (`supcat_id`),
  CONSTRAINT `FK_table_supplier_3` FOREIGN KEY (`emp_id`) REFERENCES `table_employee` (`emp_id`),
  CONSTRAINT `FK_table_supplier_4` FOREIGN KEY (`pricecat_id`) REFERENCES `table_pricecategory` (`pricecat_id`),
  CONSTRAINT `FK_table_supplier_5` FOREIGN KEY (`tax_id`) REFERENCES `table_tax` (`tax_id`),
  CONSTRAINT `FK_table_supplier_6` FOREIGN KEY (`top_id`) REFERENCES `table_termofpayment` (`top_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_supplier`
--

/*!40000 ALTER TABLE `table_supplier` DISABLE KEYS */;
INSERT INTO `table_supplier` (`sup_id`,`sup_code`,`sup_name`,`sup_active`,`sup_address`,`sup_contact`,`sup_creditlimit`,`ccy_id`,`supcat_id`,`sup_email`,`emp_id`,`sup_fax`,`sup_phone`,`pricecat_id`,`tax_id`,`sup_taxno`,`top_id`,`sup_website`,`sup_zipcode`) VALUES 
 (1,'DT','DUTA COMPUTER',1,'1','3',1000,2,2,'5',2,'7','4',2,2,'8',2,'6','2'),
 (2,'OMC','OMEGA COMPUTER',0,'Sungai Panas BATAM','Ku CIAN',0,1,1,'omega@yahoo.com',6,'r4343','44324',1,1,'ffdsfsdf',1,'www.omega.com','33213');
/*!40000 ALTER TABLE `table_supplier` ENABLE KEYS */;


--
-- Definition of table `table_suppliercategory`
--

DROP TABLE IF EXISTS `table_suppliercategory`;
CREATE TABLE `table_suppliercategory` (
  `supcat_id` int(10) unsigned NOT NULL auto_increment,
  `supcat_code` varchar(45) NOT NULL,
  `supcat_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`supcat_id`),
  UNIQUE KEY `Index_2` (`supcat_code`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_suppliercategory`
--

/*!40000 ALTER TABLE `table_suppliercategory` DISABLE KEYS */;
INSERT INTO `table_suppliercategory` (`supcat_id`,`supcat_code`,`supcat_name`) VALUES 
 (1,'SUP001','MINI MARKET'),
 (2,'SUP002','SUPER MARKET'),
 (3,'SUP003','PT.');
/*!40000 ALTER TABLE `table_suppliercategory` ENABLE KEYS */;


--
-- Definition of table `table_tax`
--

DROP TABLE IF EXISTS `table_tax`;
CREATE TABLE `table_tax` (
  `tax_id` int(10) unsigned NOT NULL auto_increment,
  `tax_code` varchar(45) NOT NULL,
  `tax_name` varchar(45) NOT NULL,
  `tax_rate` double NOT NULL,
  PRIMARY KEY  (`tax_id`),
  UNIQUE KEY `Index_2` (`tax_code`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_tax`
--

/*!40000 ALTER TABLE `table_tax` DISABLE KEYS */;
INSERT INTO `table_tax` (`tax_id`,`tax_code`,`tax_name`,`tax_rate`) VALUES 
 (1,'PPH','Pajak Penghasilan',10),
 (2,'PPN','Pajak Pasar',15.16);
/*!40000 ALTER TABLE `table_tax` ENABLE KEYS */;


--
-- Definition of table `table_termofpayment`
--

DROP TABLE IF EXISTS `table_termofpayment`;
CREATE TABLE `table_termofpayment` (
  `top_id` int(10) unsigned NOT NULL auto_increment,
  `top_code` varchar(45) NOT NULL,
  `top_name` varchar(45) NOT NULL,
  `top_days` int(10) unsigned NOT NULL,
  PRIMARY KEY  (`top_id`),
  KEY `Index_2` (`top_code`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_termofpayment`
--

/*!40000 ALTER TABLE `table_termofpayment` DISABLE KEYS */;
INSERT INTO `table_termofpayment` (`top_id`,`top_code`,`top_name`,`top_days`) VALUES 
 (1,'COD','Cash On Delivery',0),
 (2,'30DYS','Thirty Days Payment c',30),
 (3,'60DYS','Sixty Days XX',60);
/*!40000 ALTER TABLE `table_termofpayment` ENABLE KEYS */;


--
-- Definition of table `table_unit`
--

DROP TABLE IF EXISTS `table_unit`;
CREATE TABLE `table_unit` (
  `unit_id` int(10) unsigned NOT NULL auto_increment,
  `unit_code` varchar(45) NOT NULL,
  `unit_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`unit_id`),
  KEY `Index_2` (`unit_code`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_unit`
--

/*!40000 ALTER TABLE `table_unit` DISABLE KEYS */;
INSERT INTO `table_unit` (`unit_id`,`unit_code`,`unit_name`) VALUES 
 (1,'PCS','PIECES'),
 (2,'KG','Kilogram'),
 (3,'MTR','Meter');
/*!40000 ALTER TABLE `table_unit` ENABLE KEYS */;


--
-- Definition of table `table_warehouse`
--

DROP TABLE IF EXISTS `table_warehouse`;
CREATE TABLE `table_warehouse` (
  `warehouse_id` int(10) unsigned NOT NULL auto_increment,
  `warehouse_code` varchar(45) NOT NULL,
  `warehouse_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`warehouse_id`),
  UNIQUE KEY `Index_2` (`warehouse_code`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_warehouse`
--

/*!40000 ALTER TABLE `table_warehouse` DISABLE KEYS */;
INSERT INTO `table_warehouse` (`warehouse_id`,`warehouse_code`,`warehouse_name`) VALUES 
 (1,'DEF','DEFAULT STORE'),
 (2,'STR001','GUDANG A'),
 (3,'STR002','GUDANG B-CD');
/*!40000 ALTER TABLE `table_warehouse` ENABLE KEYS */;




/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
