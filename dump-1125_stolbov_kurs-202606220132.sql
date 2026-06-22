-- MySQL dump 10.13  Distrib 5.5.62, for Win64 (AMD64)
--
-- Host: localhost    Database: 1125_stolbov_kurs
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `client` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `lastname` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `passportSeries` varchar(4) COLLATE utf8mb4_general_ci NOT NULL,
  `passportNumber` varchar(6) COLLATE utf8mb4_general_ci NOT NULL,
  `phone` varchar(11) COLLATE utf8mb4_general_ci NOT NULL,
  `email` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,'Семён','Лобанов','2626','234554','88005553535','lobanov1989@gmail.com'),(2,'Фил','Ричардс','4508','778899','89112223344','phil_rich@usa.com'),(3,'Глеб','Романенко','4612','112233','89997776655','gleb_boss@mail.ru'),(4,'Борис','Левин','4510','654321','89001112233','levin_intellect@yandex.ru'),(5,'Варвара','Черноус','2814','987654','89223334455','varya_ch@gmail.com'),(7,'Кирил','Огузок','6767','424242','89993456767','superkiril@gmail.com'),(8,'Сергей','Иванов','4502','123456','79991112233','ivanov_kuhnya@mail.ru'),(9,'Надежда','Петрова','4613','654321','79112223344','komendant_obshaga@yandex.ru'),(10,'Дмитрий','Кузнецов','5014','789012','79003334455','kuznets_cook@gmail.com'),(11,'Елена','Смирнова','4005','210987','79554445566','elena_shef@mail.ru'),(12,'Михаил','Попов','6011','345678','79225556677','miha_student3@rambler.ru'),(13,'Артем','Васильев','1215','876543','79336667788','artem_kitchen@gmail.com'),(14,'Анна','Соколова','4516','456123','79777778899','anya_obshaga4@mail.ru'),(15,'Игорь','Михайлов','5002','321654','79668889900','igor_sushist@yandex.ru'),(16,'Ольга','Новикова','4608','987456','79889990011','olga_povar@mail.ru'),(17,'Данила','Федоров','2019','159263','79051113355','danila_dostavka@gmail.com'),(18,'Никита','Морозов','4512','753951','79162224466','moroz_obshaga@rambler.ru'),(19,'Мария','Волкова','4017','951357','79253335577','masha_konditer@mail.ru'),(20,'Алексей','Лебедев','5015','486259','79374446688','lebed_plov@yandex.ru'),(21,'Татьяна','Семенова','4611','269584','79995557799','tanya_starosta@mail.ru'),(22,'Владимир','Егоров','6018','842615','79036668800','vovan_kuhnya@gmail.com'),(23,'Владислав','Носиков','4646','667588','89145567874','vladnosik2001@gmail.com');
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rent`
--

DROP TABLE IF EXISTS `rent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rent` (
  `roomNumber` int NOT NULL,
  `clientId` int NOT NULL,
  `startDate` datetime NOT NULL,
  `endDate` datetime NOT NULL,
  `id` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`),
  KEY `rents_rooms_FK` (`roomNumber`),
  KEY `rents_clients_FK` (`clientId`),
  CONSTRAINT `rents_clients_FK` FOREIGN KEY (`clientId`) REFERENCES `client` (`id`),
  CONSTRAINT `rents_rooms_FK` FOREIGN KEY (`roomNumber`) REFERENCES `room` (`number`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rent`
--

LOCK TABLES `rent` WRITE;
/*!40000 ALTER TABLE `rent` DISABLE KEYS */;
INSERT INTO `rent` VALUES (201,1,'2026-05-07 00:00:00','2026-05-13 00:00:00',24),(302,13,'2026-04-28 00:00:00','2026-04-29 00:00:00',25),(101,23,'2026-05-17 00:00:00','2026-05-20 00:00:00',26),(301,4,'2026-05-22 00:00:00','2026-05-25 00:00:00',27),(101,4,'2026-05-05 00:00:00','2026-05-07 00:00:00',28),(102,23,'2026-04-29 00:00:00','2026-05-02 00:00:00',29),(302,14,'2026-05-30 00:00:00','2026-06-04 00:00:00',30),(301,2,'2026-05-24 00:00:00','2026-05-28 00:00:00',31),(201,15,'2026-05-30 00:00:00','2026-06-04 00:00:00',32),(101,4,'2026-05-20 00:00:00','2026-05-22 00:00:00',33),(102,5,'2026-05-04 00:00:00','2026-05-05 00:00:00',34),(102,17,'2026-04-26 00:00:00','2026-04-29 00:00:00',35),(101,2,'2026-05-12 00:00:00','2026-05-13 00:00:00',36),(302,12,'2026-05-09 00:00:00','2026-05-16 00:00:00',37),(201,23,'2026-04-26 00:00:00','2026-05-01 00:00:00',38),(301,11,'2026-06-05 00:00:00','2026-06-10 00:00:00',39),(201,18,'2026-04-26 00:00:00','2026-05-03 00:00:00',40),(201,14,'2026-06-03 00:00:00','2026-06-10 00:00:00',41),(202,17,'2026-06-14 00:00:00','2026-06-15 00:00:00',42),(201,22,'2026-05-26 00:00:00','2026-06-02 00:00:00',43),(102,5,'2026-04-24 00:00:00','2026-05-01 00:00:00',44),(201,7,'2026-05-08 00:00:00','2026-05-14 00:00:00',45),(101,2,'2026-06-06 00:00:00','2026-06-10 00:00:00',46),(102,20,'2026-05-23 00:00:00','2026-05-24 00:00:00',47),(102,3,'2026-06-21 00:00:00','2026-06-24 00:00:00',48),(201,2,'2026-06-11 00:00:00','2026-06-13 00:00:00',49),(102,9,'2026-06-12 00:00:00','2026-06-14 00:00:00',50),(102,22,'2026-05-29 00:00:00','2026-06-04 00:00:00',51),(302,9,'2026-04-26 00:00:00','2026-04-29 00:00:00',52),(202,21,'2026-05-29 00:00:00','2026-06-03 00:00:00',53);
/*!40000 ALTER TABLE `rent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reservation`
--

DROP TABLE IF EXISTS `reservation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `reservation` (
  `rommNumber` int NOT NULL,
  `clientId` int NOT NULL,
  `reservationDate` datetime NOT NULL,
  `id` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`),
  KEY `reservation_rooms_FK` (`rommNumber`),
  KEY `reservation_clients_FK` (`clientId`),
  CONSTRAINT `reservation_clients_FK` FOREIGN KEY (`clientId`) REFERENCES `client` (`id`),
  CONSTRAINT `reservation_rooms_FK` FOREIGN KEY (`rommNumber`) REFERENCES `room` (`number`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reservation`
--

LOCK TABLES `reservation` WRITE;
/*!40000 ALTER TABLE `reservation` DISABLE KEYS */;
INSERT INTO `reservation` VALUES (102,1,'2026-06-24 00:00:00',5),(301,1,'2026-06-23 00:00:00',6),(301,3,'2026-07-06 00:00:00',7),(201,3,'2026-06-24 00:00:00',8),(102,7,'2026-06-29 00:00:00',9);
/*!40000 ALTER TABLE `reservation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `room`
--

DROP TABLE IF EXISTS `room`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room` (
  `number` int NOT NULL,
  `area` int NOT NULL,
  `isFree` tinyint(1) NOT NULL,
  `classId` int DEFAULT NULL,
  `pricePerN` int NOT NULL,
  PRIMARY KEY (`number`),
  KEY `room_roomClass_FK` (`classId`),
  CONSTRAINT `room_roomClass_FK` FOREIGN KEY (`classId`) REFERENCES `roomclass` (`classId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `room`
--

LOCK TABLES `room` WRITE;
/*!40000 ALTER TABLE `room` DISABLE KEYS */;
INSERT INTO `room` VALUES (101,18,1,1,5000),(102,20,1,2,10500),(201,25,1,2,12000),(202,30,0,3,20000),(301,45,1,4,35000),(302,50,1,4,40000);
/*!40000 ALTER TABLE `room` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roomclass`
--

DROP TABLE IF EXISTS `roomclass`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `roomclass` (
  `classId` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`classId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roomclass`
--

LOCK TABLES `roomclass` WRITE;
/*!40000 ALTER TABLE `roomclass` DISABLE KEYS */;
INSERT INTO `roomclass` VALUES (1,'Эконом'),(2,'Стандарт'),(3,'Комфорт'),(4,'Люкс');
/*!40000 ALTER TABLE `roomclass` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database '1125_stolbov_kurs'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-06-22  1:32:22
