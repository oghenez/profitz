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
-- Definition of table `table_division`
--

DROP TABLE IF EXISTS `table_division`;
CREATE TABLE `table_division` (
  `div_id` int(10) unsigned NOT NULL auto_increment,
  `div_code` varchar(45) NOT NULL,
  `div_name` varchar(45) NOT NULL,
  PRIMARY KEY  (`div_id`),
  UNIQUE KEY `Index_2` (`div_code`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_division`
--

/*!40000 ALTER TABLE `table_division` DISABLE KEYS */;
INSERT INTO `table_division` (`div_id`,`div_code`,`div_name`) VALUES 
 (1,'DIV001','Information Technology');
/*!40000 ALTER TABLE `table_division` ENABLE KEYS */;


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
 (1,'DWP02825','Dodo',1,1,0),
 (2,'DWP00533','RADY',0,1,1),
 (6,'DWP00165','SEBASTIAN',1,1,1);
/*!40000 ALTER TABLE `table_employee` ENABLE KEYS */;


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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_termofpayment`
--

/*!40000 ALTER TABLE `table_termofpayment` DISABLE KEYS */;
INSERT INTO `table_termofpayment` (`top_id`,`top_code`,`top_name`,`top_days`) VALUES 
 (1,'COD','Cash On Delivery',0),
 (2,'30DYS','Thirty Days Payment',30);
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




/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
