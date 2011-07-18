-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.0.27-community-nt


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

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
 (8,'BRI','BRI cabang batam'),
 (10,'ytru','rtyurtyu'),
 (11,'1234','1234');
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `table_currency`
--

/*!40000 ALTER TABLE `table_currency` DISABLE KEYS */;
INSERT INTO `table_currency` (`ccy_id`,`ccy_code`,`ccy_name`) VALUES 
 (1,'IDR','Indonesian Rupiah'),
 (2,'SGD','Singapore Dollar'),
 (3,'USD','US Dollar');
/*!40000 ALTER TABLE `table_currency` ENABLE KEYS */;




/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
