-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.7.11 - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for elitegst
DROP DATABASE IF EXISTS `elitegst`;
CREATE DATABASE IF NOT EXISTS `elitegst` /*!40100 DEFAULT CHARACTER SET utf32 */;
USE `elitegst`;

-- Dumping structure for view elitegst.fabricinvoicetotal
DROP VIEW IF EXISTS `fabricinvoicetotal`;
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `fabricinvoicetotal` (
	`Id` INT(11) NOT NULL,
	`Quantity` DECIMAL(33,2) NULL,
	`Subtotal` DECIMAL(42,2) NULL,
	`CGST` DECIMAL(32,2) NULL,
	`SGST` DECIMAL(32,2) NULL,
	`IGST` DECIMAL(32,2) NULL
) ENGINE=MyISAM;

-- Dumping structure for table elitegst.invoicefabricproducts
DROP TABLE IF EXISTS `invoicefabricproducts`;
CREATE TABLE IF NOT EXISTS `invoicefabricproducts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `InvoiceId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `Bales` int(11) NOT NULL DEFAULT '1',
  `Pieces` int(11) NOT NULL DEFAULT '1',
  `Meters` decimal(10,2) NOT NULL DEFAULT '0.00',
  `FoldingLoss` decimal(10,2) NOT NULL DEFAULT '0.00',
  `FoldingLossRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Rate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `CGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `SGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `CGST` decimal(10,2) NOT NULL DEFAULT '0.00',
  `SGST` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IGST` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`Id`),
  KEY `invoicefabricproducts_ibfk_1` (`InvoiceId`),
  KEY `invoicefabricproducts_ibfk_2` (`ProductId`),
  CONSTRAINT `invoicefabricproducts_ibfk_1` FOREIGN KEY (`InvoiceId`) REFERENCES `invoices` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `invoicefabricproducts_ibfk_2` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf32 ROW_FORMAT=DYNAMIC;

-- Dumping data for table elitegst.invoicefabricproducts: ~0 rows (approximately)
/*!40000 ALTER TABLE `invoicefabricproducts` DISABLE KEYS */;
/*!40000 ALTER TABLE `invoicefabricproducts` ENABLE KEYS */;

-- Dumping structure for table elitegst.invoiceproducts
DROP TABLE IF EXISTS `invoiceproducts`;
CREATE TABLE IF NOT EXISTS `invoiceproducts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `InvoiceId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `Quantity` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Packs` int(11) NOT NULL DEFAULT '1',
  `Rate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Discount` decimal(10,2) NOT NULL DEFAULT '0.00',
  `CGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `SGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `CGST` decimal(10,2) NOT NULL DEFAULT '0.00',
  `SGST` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IGST` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`Id`),
  KEY `FK_invoiceproducts_invoices` (`InvoiceId`),
  KEY `FK_invoiceproducts_products` (`ProductId`),
  CONSTRAINT `FK_invoiceproducts_invoices` FOREIGN KEY (`InvoiceId`) REFERENCES `invoices` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_invoiceproducts_products` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf32 ROW_FORMAT=DYNAMIC;

-- Dumping data for table elitegst.invoiceproducts: ~1 rows (approximately)
/*!40000 ALTER TABLE `invoiceproducts` DISABLE KEYS */;
/*!40000 ALTER TABLE `invoiceproducts` ENABLE KEYS */;

-- Dumping structure for table elitegst.invoices
DROP TABLE IF EXISTS `invoices`;
CREATE TABLE IF NOT EXISTS `invoices` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `InvoiceStringId` varchar(50) NOT NULL,
  `InvoiceType` tinyint(4) NOT NULL,
  `InvoiceDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `BillingId` int(11) NOT NULL,
  `ShippingId` int(11) NOT NULL,
  `TransportMode` varchar(20) NOT NULL,
  `VehicleNumber` varchar(20) DEFAULT NULL,
  `Remarks` varchar(500) DEFAULT NULL,
  `LoadingCharges` decimal(10,2) NOT NULL DEFAULT '0.00',
  `OtherCharges` decimal(10,2) NOT NULL DEFAULT '0.00',
  `RoundingOff` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IsCancelled` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id`),
  KEY `FK_invoices_parties` (`BillingId`),
  KEY `FK_invoices_parties_shipping` (`ShippingId`),
  CONSTRAINT `FK_invoices_parties` FOREIGN KEY (`BillingId`) REFERENCES `parties` (`Id`) ON UPDATE CASCADE,
  CONSTRAINT `FK_invoices_parties_shipping` FOREIGN KEY (`ShippingId`) REFERENCES `parties` (`Id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf32;

-- Dumping data for table elitegst.invoices: ~0 rows (approximately)
/*!40000 ALTER TABLE `invoices` DISABLE KEYS */;
/*!40000 ALTER TABLE `invoices` ENABLE KEYS */;

-- Dumping structure for view elitegst.invoicetotal
DROP VIEW IF EXISTS `invoicetotal`;
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `invoicetotal` (
	`Id` INT(11) NOT NULL,
	`Quantity` DECIMAL(32,2) NULL,
	`Subtotal` DECIMAL(41,2) NULL,
	`CGST` DECIMAL(32,2) NULL,
	`SGST` DECIMAL(32,2) NULL,
	`IGST` DECIMAL(32,2) NULL,
	`Discount` DECIMAL(32,2) NULL
) ENGINE=MyISAM;

-- Dumping structure for table elitegst.options
DROP TABLE IF EXISTS `options`;
CREATE TABLE IF NOT EXISTS `options` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DefaultCGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `DefaultSGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `DefaultIGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `DefaultDiscountRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `DefaultFoldingLossRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `DefaultInvoiceRemarks` varchar(500) DEFAULT NULL,
  `DefaultPurchaseOrderRemarks` varchar(500) DEFAULT NULL,
  `DefaultFabricInvoiceRemarks` varchar(500) DEFAULT NULL,
  `BankAccName` varchar(100) DEFAULT NULL,
  `BankAccNo` varchar(50) DEFAULT NULL,
  `BankBranch` varchar(100) DEFAULT NULL,
  `BankIFSC` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf32;

-- Dumping data for table elitegst.options: ~0 rows (approximately)
/*!40000 ALTER TABLE `options` DISABLE KEYS */;
INSERT INTO `options` (`Id`, `DefaultCGSTRate`, `DefaultSGSTRate`, `DefaultIGSTRate`, `DefaultDiscountRate`, `DefaultFoldingLossRate`, `DefaultInvoiceRemarks`, `DefaultPurchaseOrderRemarks`, `DefaultFabricInvoiceRemarks`, `BankAccName`, `BankAccNo`, `BankBranch`, `BankIFSC`) VALUES
	(1, 2.50, 2.50, 0.00, 0.00, 3.00, 'Declaration\r\nWe declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.', NULL, NULL, 'THE ELITE SOLUTIONS', '1234567890', 'CANARA BANK', 'CNRB0003453');
/*!40000 ALTER TABLE `options` ENABLE KEYS */;

-- Dumping structure for table elitegst.parties
DROP TABLE IF EXISTS `parties`;
CREATE TABLE IF NOT EXISTS `parties` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PartyType` tinyint(4) NOT NULL,
  `CompanyName` varchar(100) NOT NULL,
  `Address` varchar(150) DEFAULT NULL,
  `City` varchar(50) DEFAULT NULL,
  `State` varchar(50) DEFAULT NULL,
  `Code` varchar(10) DEFAULT NULL,
  `Phone` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `GSTIN` varchar(30) DEFAULT NULL,
  `IsActive` tinyint(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf32;

-- Dumping data for table elitegst.parties: ~1 rows (approximately)
/*!40000 ALTER TABLE `parties` DISABLE KEYS */;
INSERT INTO `parties` (`Id`, `PartyType`, `CompanyName`, `Address`, `City`, `State`, `Code`, `Phone`, `Email`, `GSTIN`, `IsActive`) VALUES
	(1, 1, 'The Elite Solutions', '2/113, Kodangipalayam', 'Palladam', 'Tamil Nadu', '33', '9500442332', 'info@theelitesoltuions.com', 'BQ4HKYXGXVVD6J89FMYBHH7QM', 1);
/*!40000 ALTER TABLE `parties` ENABLE KEYS */;

-- Dumping structure for table elitegst.products
DROP TABLE IF EXISTS `products`;
CREATE TABLE IF NOT EXISTS `products` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ProductDescription` varchar(150) NOT NULL,
  `HSN` varchar(30) DEFAULT NULL,
  `UoM` varchar(30) NOT NULL,
  `Rate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IsActive` tinyint(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32;

-- Dumping data for table elitegst.products: ~1 rows (approximately)
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
/*!40000 ALTER TABLE `products` ENABLE KEYS */;

-- Dumping structure for table elitegst.purchaseorderproducts
DROP TABLE IF EXISTS `purchaseorderproducts`;
CREATE TABLE IF NOT EXISTS `purchaseorderproducts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PurchaseOrderId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `Quantity` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Rate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Discount` decimal(10,2) NOT NULL DEFAULT '0.00',
  `CGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `SGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IGSTRate` decimal(10,2) NOT NULL DEFAULT '0.00',
  `CGST` decimal(10,2) NOT NULL DEFAULT '0.00',
  `SGST` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IGST` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`Id`),
  KEY `FK_purchaseorderproducts_purchaseorders` (`PurchaseOrderId`),
  KEY `FK_purchaseorderproducts_products` (`ProductId`),
  CONSTRAINT `FK_purchaseorderproducts_products` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON UPDATE CASCADE,
  CONSTRAINT `FK_purchaseorderproducts_purchaseorders` FOREIGN KEY (`PurchaseOrderId`) REFERENCES `purchaseorders` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf32;

-- Dumping data for table elitegst.purchaseorderproducts: ~0 rows (approximately)
/*!40000 ALTER TABLE `purchaseorderproducts` DISABLE KEYS */;
/*!40000 ALTER TABLE `purchaseorderproducts` ENABLE KEYS */;

-- Dumping structure for table elitegst.purchaseorders
DROP TABLE IF EXISTS `purchaseorders`;
CREATE TABLE IF NOT EXISTS `purchaseorders` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PurchaseOrderStringId` varchar(50) NOT NULL,
  `PurchaseOrderDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `BillingId` int(11) NOT NULL,
  `ShippingId` int(11) NOT NULL,
  `Remarks` varchar(500) DEFAULT NULL,
  `IsCancelled` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id`),
  KEY `FK_purchaseorders_parties` (`BillingId`),
  KEY `FK_purchaseorders_parties_shipping` (`ShippingId`),
  CONSTRAINT `FK_purchaseorders_parties` FOREIGN KEY (`BillingId`) REFERENCES `parties` (`Id`) ON UPDATE CASCADE,
  CONSTRAINT `FK_purchaseorders_parties_shipping` FOREIGN KEY (`ShippingId`) REFERENCES `parties` (`Id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf32;

-- Dumping data for table elitegst.purchaseorders: ~0 rows (approximately)
/*!40000 ALTER TABLE `purchaseorders` DISABLE KEYS */;
/*!40000 ALTER TABLE `purchaseorders` ENABLE KEYS */;

-- Dumping structure for view elitegst.purchaseordertotal
DROP VIEW IF EXISTS `purchaseordertotal`;
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `purchaseordertotal` (
	`Id` INT(11) NOT NULL,
	`Quantity` DECIMAL(32,2) NULL,
	`Subtotal` DECIMAL(41,2) NULL,
	`CGST` DECIMAL(32,2) NULL,
	`SGST` DECIMAL(32,2) NULL,
	`IGST` DECIMAL(32,2) NULL
) ENGINE=MyISAM;

-- Dumping structure for view elitegst.fabricinvoicetotal
DROP VIEW IF EXISTS `fabricinvoicetotal`;
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `fabricinvoicetotal`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `fabricinvoicetotal` AS SELECT ipt.InvoiceId AS Id, ROUND(SUM(ipt.Meters) - SUM(ipt.FoldingLoss), 2) AS Quantity, ROUND(SUM(ipt.Subtotal), 2) AS Subtotal, ROUND(SUM(ipt.CGST), 2) AS CGST, ROUND(SUM(ipt.SGST), 2) AS SGST, ROUND(SUM(ipt.IGST), 2) AS IGST
FROM (
	SELECT ip.InvoiceId, ip.ProductId, ip.Meters, ip.FoldingLoss, ip.Rate, (ip.Meters - ip.FoldingLoss) * ip.Rate AS Subtotal, ip.CGST, ip.SGST, ip.IGST
	FROM invoicefabricproducts ip
	WHERE 1
	) ipt
WHERE 1
GROUP BY ipt.InvoiceId ;

-- Dumping structure for view elitegst.invoicetotal
DROP VIEW IF EXISTS `invoicetotal`;
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `invoicetotal`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `invoicetotal` AS SELECT ipt.InvoiceId AS Id, ROUND(SUM(ipt.Quantity), 2) AS Quantity, ROUND(SUM(ipt.Subtotal), 2) AS Subtotal, ROUND(SUM(ipt.CGST), 2) AS CGST, ROUND(SUM(ipt.SGST), 2) AS SGST, ROUND(SUM(ipt.IGST), 2) AS IGST, ROUND(SUM(ipt.Discount), 2) AS Discount
FROM (
	SELECT ip.InvoiceId, ip.ProductId, ip.Quantity, ip.Rate, ip.Quantity * ip.Rate AS Subtotal, ip.CGST, ip.SGST, ip.IGST, ip.Discount
	FROM invoiceproducts ip
	WHERE 1
	) ipt
WHERE 1
GROUP BY ipt.InvoiceId ;

-- Dumping structure for view elitegst.purchaseordertotal
DROP VIEW IF EXISTS `purchaseordertotal`;
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `purchaseordertotal`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `purchaseordertotal` AS SELECT ipt.PurchaseOrderId AS Id, SUM(ipt.Quantity) AS Quantity, ROUND(SUM(ipt.Subtotal), 2) AS Subtotal, ROUND(SUM(ipt.CGST), 2) AS CGST, ROUND(SUM(ipt.SGST), 2) AS SGST, ROUND(SUM(ipt.IGST), 2) AS IGST
FROM (
	SELECT ip.PurchaseOrderId, ip.ProductId, ip.Quantity, ip.Rate, ip.Quantity * ip.Rate AS Subtotal, ip.CGST, ip.SGST, ip.IGST
	FROM purchaseorderproducts ip
	WHERE 1
	) ipt
WHERE 1
GROUP BY ipt.PurchaseOrderId ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
