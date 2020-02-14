/*
Navicat MySQL Data Transfer

Source Server         : mysql_localhost
Source Server Version : 80018
Source Host           : localhost:3306
Source Database       : blazuicommunity

Target Server Type    : MYSQL
Target Server Version : 80018
File Encoding         : 65001

Date: 2020-02-14 22:54:59
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for address
-- ----------------------------
DROP TABLE IF EXISTS `address`;
CREATE TABLE `address` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Country` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Province` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `City` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `District` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Address_UserId` (`UserId`),
  CONSTRAINT `FK_Address_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of address
-- ----------------------------

-- ----------------------------
-- Table structure for aspnetroleclaims
-- ----------------------------
DROP TABLE IF EXISTS `aspnetroleclaims`;
CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` int(11) NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of aspnetroleclaims
-- ----------------------------

-- ----------------------------
-- Table structure for aspnetroles
-- ----------------------------
DROP TABLE IF EXISTS `aspnetroles`;
CREATE TABLE `aspnetroles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of aspnetroles
-- ----------------------------

-- ----------------------------
-- Table structure for aspnetuserclaims
-- ----------------------------
DROP TABLE IF EXISTS `aspnetuserclaims`;
CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of aspnetuserclaims
-- ----------------------------

-- ----------------------------
-- Table structure for aspnetuserlogins
-- ----------------------------
DROP TABLE IF EXISTS `aspnetuserlogins`;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of aspnetuserlogins
-- ----------------------------

-- ----------------------------
-- Table structure for aspnetuserroles
-- ----------------------------
DROP TABLE IF EXISTS `aspnetuserroles`;
CREATE TABLE `aspnetuserroles` (
  `UserId` int(11) NOT NULL,
  `RoleId` int(11) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of aspnetuserroles
-- ----------------------------

-- ----------------------------
-- Table structure for aspnetusers
-- ----------------------------
DROP TABLE IF EXISTS `aspnetusers`;
CREATE TABLE `aspnetusers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `NickName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Avator` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Sex` int(11) DEFAULT NULL,
  `Signature` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `RegisterDate` datetime(6) NOT NULL,
  `Status` int(11) DEFAULT NULL,
  `Level` int(11) DEFAULT NULL,
  `Points` int(11) DEFAULT NULL,
  `LastLoginDate` datetime(6) NOT NULL,
  `LastLoginType` int(11) NOT NULL,
  `LastLoginAddr` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of aspnetusers
-- ----------------------------
INSERT INTO `aspnetusers` VALUES ('1', 'admin', 'ADMIN', '123456@qq.com', '123456@QQ.COM', '0', 'AQAAAAEAACcQAAAAEDizj3bFcONUaRX3COmRsdomwtmJY+On/Dqpi352ZC8MmyRLkiBAbhcx1Oe9HLgtVA==', 'ECVHJL44HUP2UQEODC3PBIQ2GNY4K2PK', '7e7bfa31-538e-4af5-8a37-c67220540558', '15013720987', '0', '0', null, '1', '0', '最光阴', '/img/header/7b0e264e-4926-4a73-acec-610d06ee3066.png', '0', '蹉跎过，消磨过，最是光阴化浮沫', '2020-02-08 09:29:13.838712', '0', null, null, '2020-02-08 09:29:13.842600', '0', null);
INSERT INTO `aspnetusers` VALUES ('2', 'blazor', 'BLAZOR', '', '', '0', 'AQAAAAEAACcQAAAAEMaCFB07fmlzx47d2oRehfUwCjo44GCaRMUhWK/hJNy7pq1GjXpqcM7CSgnJ6DU2og==', 'AGKCRZ3QCTKS54KVGYXQQCFMRBCY62FD', 'dfdeddc8-093d-49e3-8e94-bc28dee6e7b4', null, '0', '0', null, '1', '0', 'blazor', '/img/header/37b83cb4-0d6a-4a2b-b45b-b9ef0ecf6cc0.jpg', '0', null, '2020-02-08 18:48:13.975617', '0', null, null, '2020-02-08 18:48:13.975901', '0', null);

-- ----------------------------
-- Table structure for aspnetusertokens
-- ----------------------------
DROP TABLE IF EXISTS `aspnetusertokens`;
CREATE TABLE `aspnetusertokens` (
  `UserId` int(11) NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of aspnetusertokens
-- ----------------------------

-- ----------------------------
-- Table structure for follow
-- ----------------------------
DROP TABLE IF EXISTS `follow`;
CREATE TABLE `follow` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TopicId` int(11) NOT NULL,
  `Status` int(11) DEFAULT NULL,
  `FollowTime` datetime(6) NOT NULL,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Follow_TopicId` (`TopicId`),
  KEY `IX_Follow_UserId` (`UserId`),
  CONSTRAINT `FK_Follow_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Follow_Topic_TopicId` FOREIGN KEY (`TopicId`) REFERENCES `topic` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of follow
-- ----------------------------
INSERT INTO `follow` VALUES ('1', '1', '0', '2020-02-11 00:00:00.000000', '1');

-- ----------------------------
-- Table structure for point
-- ----------------------------
DROP TABLE IF EXISTS `point`;
CREATE TABLE `point` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Access` int(11) DEFAULT NULL,
  `Score` int(11) DEFAULT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_point_UserId` (`UserId`),
  CONSTRAINT `FK_point_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of point
-- ----------------------------

-- ----------------------------
-- Table structure for reply
-- ----------------------------
DROP TABLE IF EXISTS `reply`;
CREATE TABLE `reply` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PublishTime` datetime(6) NOT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `UserId` int(11) NOT NULL,
  `TopicId` int(11) NOT NULL,
  `Status` int(11) DEFAULT NULL,
  `Favor` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Reply_TopicId` (`TopicId`),
  CONSTRAINT `FK_Reply_Topic_TopicId` FOREIGN KEY (`TopicId`) REFERENCES `topic` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of reply
-- ----------------------------
INSERT INTO `reply` VALUES ('2', '321312312321', '2020-02-14 19:58:46.868523', '2020-02-14 19:58:46.868393', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('3', '2222121', '2020-02-14 20:02:02.791639', '2020-02-14 20:02:02.791631', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('4', 'ddsadsadsadsadsa', '2020-02-14 20:12:32.743350', '2020-02-14 20:12:32.743342', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('5', 'aadadadas', '2020-02-14 20:20:16.076070', '2020-02-14 20:20:16.076062', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('6', '32132131', '2020-02-14 20:23:05.382652', '2020-02-14 20:23:05.382564', '1', '25', '0', '0');
INSERT INTO `reply` VALUES ('7', '23232213321213', '2020-02-14 20:23:33.874123', '2020-02-14 20:23:33.874114', '1', '25', '0', '0');
INSERT INTO `reply` VALUES ('8', '33333321312', '2020-02-14 20:23:38.962387', '2020-02-14 20:23:38.962380', '1', '25', '0', '0');
INSERT INTO `reply` VALUES ('9', '3331312312321321312', '2020-02-14 20:23:44.145534', '2020-02-14 20:23:44.145527', '1', '25', '0', '0');
INSERT INTO `reply` VALUES ('10', '41421421321312', '2020-02-14 20:23:51.545119', '2020-02-14 20:23:51.545112', '1', '25', '0', '0');
INSERT INTO `reply` VALUES ('11', '3123131321321', '2020-02-14 20:24:04.530778', '2020-02-14 20:24:04.530771', '1', '25', '0', '0');
INSERT INTO `reply` VALUES ('12', '22332121332321321321', '2020-02-14 20:24:26.568677', '2020-02-14 20:24:26.568675', '1', '25', '0', '0');
INSERT INTO `reply` VALUES ('15', 'aaaaa', '2020-02-14 20:35:47.141892', '2020-02-14 20:35:47.141885', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('16', 'adsadadada', '2020-02-14 20:36:16.673595', '2020-02-14 20:36:16.673587', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('17', '11111111', '2020-02-14 20:36:25.092894', '2020-02-14 20:36:25.092887', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('18', 'asddaadaddas', '2020-02-14 20:41:06.231452', '2020-02-14 20:41:06.231448', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('19', '1111', '2020-02-14 20:46:51.090931', '2020-02-14 20:46:51.090923', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('20', '111111111111', '2020-02-14 20:47:10.434640', '2020-02-14 20:47:10.434632', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('21', '222222222222222222222222', '2020-02-14 20:47:58.378288', '2020-02-14 20:47:58.356214', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('22', '11111111111111111', '2020-02-14 21:02:31.140462', '2020-02-14 21:02:31.140450', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('23', 'abcdefg', '2020-02-14 21:05:50.782644', '2020-02-14 21:05:50.782637', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('24', '大萨达DASDADSA', '2020-02-14 22:06:45.564444', '2020-02-14 22:06:45.564351', '1', '35', '0', '0');
INSERT INTO `reply` VALUES ('25', '|   列1   |   列2   |   列3   |   列4   |   列5   |   列6   |\n| :-------| :-------| :-------| :-------| :-------| :-------|\n|         |         |         |         |         |         |', '2020-02-14 22:34:06.213623', '2020-02-14 22:34:06.213535', '1', '36', '0', '0');
INSERT INTO `reply` VALUES ('26', '呵呵呵呵', '2020-02-14 22:34:33.010659', '2020-02-14 22:34:33.010654', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('27', '测试一下会不会跳转到最后一页', '2020-02-14 22:43:40.796838', '2020-02-14 22:43:40.796828', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('28', '测试一下是否会自动跳转到最后一页', '2020-02-14 22:45:18.911951', '2020-02-14 22:45:18.911940', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('29', '测试一下是否会自动跳转到最后一页', '2020-02-14 22:46:19.785225', '2020-02-14 22:46:19.785211', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('30', '成都市大爱仕达', '2020-02-14 22:49:41.976885', '2020-02-14 22:49:41.976861', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('31', '11111', '2020-02-14 22:51:13.370654', '2020-02-14 22:51:13.370642', '1', '7', '0', '0');
INSERT INTO `reply` VALUES ('32', '飒飒的萨达十大', '2020-02-14 22:52:43.544463', '2020-02-14 22:52:43.544455', '1', '7', '0', '0');

-- ----------------------------
-- Table structure for syslog
-- ----------------------------
DROP TABLE IF EXISTS `syslog`;
CREATE TABLE `syslog` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Introduction` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Detail` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `LogType` tinyint(3) unsigned NOT NULL,
  `CreateTime` datetime(6) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `LastModifyTime` datetime(6) DEFAULT NULL,
  `LastModifierId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of syslog
-- ----------------------------

-- ----------------------------
-- Table structure for sysmenu
-- ----------------------------
DROP TABLE IF EXISTS `sysmenu`;
CREATE TABLE `sysmenu` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ParentId` int(11) NOT NULL,
  `Text` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Url` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `MenuLevel` tinyint(3) unsigned NOT NULL,
  `MenuType` tinyint(3) unsigned NOT NULL,
  `MenuIcon` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SourcePath` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Sort` int(11) NOT NULL,
  `Status` tinyint(3) unsigned NOT NULL,
  `CreateTime` datetime(6) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `LastModifyTime` datetime(6) DEFAULT NULL,
  `LastModifierId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sysmenu
-- ----------------------------

-- ----------------------------
-- Table structure for sysrole
-- ----------------------------
DROP TABLE IF EXISTS `sysrole`;
CREATE TABLE `sysrole` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Text` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` tinyint(3) unsigned NOT NULL,
  `CreateTime` datetime(6) NOT NULL,
  `CreateId` int(11) NOT NULL,
  `LastModifyTime` datetime(6) DEFAULT NULL,
  `LastModifierId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sysrole
-- ----------------------------

-- ----------------------------
-- Table structure for sysrolemenumapping
-- ----------------------------
DROP TABLE IF EXISTS `sysrolemenumapping`;
CREATE TABLE `sysrolemenumapping` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SysRoleId` int(11) NOT NULL,
  `SysMenuId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sysrolemenumapping
-- ----------------------------

-- ----------------------------
-- Table structure for sysuser
-- ----------------------------
DROP TABLE IF EXISTS `sysuser`;
CREATE TABLE `sysuser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` tinyint(3) unsigned NOT NULL,
  `Phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Mobile` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Address` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `QQ` bigint(20) DEFAULT NULL,
  `WeChat` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Sex` tinyint(3) unsigned DEFAULT NULL,
  `LastLoginTime` datetime(6) DEFAULT NULL,
  `CreateTime` datetime(6) NOT NULL,
  `CreateId` int(11) NOT NULL,
  `LastModifyTime` datetime(6) DEFAULT NULL,
  `LastModifyId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sysuser
-- ----------------------------

-- ----------------------------
-- Table structure for sysusermenumapping
-- ----------------------------
DROP TABLE IF EXISTS `sysusermenumapping`;
CREATE TABLE `sysusermenumapping` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SysUserId` int(11) NOT NULL,
  `SysMenuId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sysusermenumapping
-- ----------------------------

-- ----------------------------
-- Table structure for sysuserrolemapping
-- ----------------------------
DROP TABLE IF EXISTS `sysuserrolemapping`;
CREATE TABLE `sysuserrolemapping` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SysUserId` int(11) NOT NULL,
  `SysRoleId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sysuserrolemapping
-- ----------------------------

-- ----------------------------
-- Table structure for thirdaccount
-- ----------------------------
DROP TABLE IF EXISTS `thirdaccount`;
CREATE TABLE `thirdaccount` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `OauthType` int(11) NOT NULL,
  `OauthName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `OauthId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `NickName` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Photo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` int(11) NOT NULL,
  `HomePage` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_Thirdaccount_UserId` (`UserId`),
  CONSTRAINT `FK_Thirdaccount_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of thirdaccount
-- ----------------------------

-- ----------------------------
-- Table structure for topic
-- ----------------------------
DROP TABLE IF EXISTS `topic`;
CREATE TABLE `topic` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PublishTime` datetime(6) NOT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `UserId` int(11) NOT NULL,
  `Status` int(11) DEFAULT NULL,
  `Hot` int(11) DEFAULT NULL,
  `Top` int(11) DEFAULT NULL,
  `Good` int(11) DEFAULT NULL,
  `TopicType` int(11) DEFAULT NULL,
  `ReplyCount` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Topic_UserId` (`UserId`),
  CONSTRAINT `FK_Topic_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of topic
-- ----------------------------
INSERT INTO `topic` VALUES ('1', '第一次发帖测试讨论--已结帖', '第一次发帖测试讨论--已结帖', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '1', '0', '0', '0', '1', '11');
INSERT INTO `topic` VALUES ('2', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '0', '2', '4');
INSERT INTO `topic` VALUES ('3', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '1', '2', '2');
INSERT INTO `topic` VALUES ('4', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '1', '1', '3', '5');
INSERT INTO `topic` VALUES ('7', '第二次发帖测试讨论', '第一次发帖测试讨论', '2020-02-13 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '1', '1', '4', '22');
INSERT INTO `topic` VALUES ('8', '第8次发帖测试讨论', '第8次发帖测试讨论', '2020-02-13 15:46:00.000000', '2020-02-13 15:49:41.000000', '1', '0', '0', '1', '1', '0', '6');
INSERT INTO `topic` VALUES ('9', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '0', '2', '11');
INSERT INTO `topic` VALUES ('11', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '0', '2', '4');
INSERT INTO `topic` VALUES ('12', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '1', '1', '2');
INSERT INTO `topic` VALUES ('13', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '1', '1', '1', '5');
INSERT INTO `topic` VALUES ('14', '第二次发帖测试讨论', '第一次发帖测试讨论', '2020-02-13 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '1', '1', '2', '6');
INSERT INTO `topic` VALUES ('15', '第8次发帖测试讨论', '第8次发帖测试讨论', '2020-02-13 15:46:00.000000', '2020-02-13 15:49:41.000000', '1', '0', '0', '1', '1', '3', '6');
INSERT INTO `topic` VALUES ('21', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '0', '2', '11');
INSERT INTO `topic` VALUES ('22', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '0', '2', '4');
INSERT INTO `topic` VALUES ('23', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '1', '3', '2');
INSERT INTO `topic` VALUES ('24', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '1', '1', '0', '5');
INSERT INTO `topic` VALUES ('25', '第二次发帖测试讨论', '第一次发帖测试讨论', '2020-02-13 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '1', '1', '2', '7');
INSERT INTO `topic` VALUES ('26', '第8次发帖测试讨论', '第8次发帖测试讨论', '2020-02-13 15:46:00.000000', '2020-02-13 15:49:41.000000', '1', '0', '0', '1', '1', '2', '6');
INSERT INTO `topic` VALUES ('27', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '0', '1', '11');
INSERT INTO `topic` VALUES ('28', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '0', '2', '4');
INSERT INTO `topic` VALUES ('29', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '1', '0', '2');
INSERT INTO `topic` VALUES ('30', '第一次发帖测试讨论', '第一次发帖测试讨论', '2020-02-10 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '0', '0', '3', '5');
INSERT INTO `topic` VALUES ('31', '第二次发帖测试讨论', '第一次发帖测试讨论', '2020-02-13 15:49:41.115848', '2020-02-10 15:49:41.115739', '1', '0', '0', '1', '1', '3', '6');
INSERT INTO `topic` VALUES ('32', '第8次发帖测试讨论', '第8次发帖测试讨论', '2020-02-13 15:46:00.000000', '2020-02-13 15:49:41.000000', '1', '0', '0', '1', '1', '1', '6');
INSERT INTO `topic` VALUES ('33', '计划周日上线', '计划周日上线', '2020-02-14 11:25:29.644160', '2020-02-14 11:25:29.644153', '1', '0', '0', '0', '0', '4', '0');
INSERT INTO `topic` VALUES ('34', 'MarkDown Demo', '### 主要特性\n\n- 支持“标准”Markdown / CommonMark和Github风格的语法，也可变身为代码编辑器；\n- 支持实时预览、图片（跨域）上传、预格式文本/代码/表格插入、代码折叠和多语言语法高亮等功能；\n- 支持Task lists等Markdown扩展语法；\n- 支持识别和解析HTML标签，并且支持自定义过滤标签解析，具有可靠的安全性和几乎无限的扩展性；\n\n# Editor.md\n\n![](https://pandao.github.io/editor.md/images/logos/editormd-logo-180x180.png)\n\n![](https://img.shields.io/github/stars/pandao/editor.md.svg) ![](https://img.shields.io/github/forks/pandao/editor.md.svg) ![](https://img.shields.io/github/tag/pandao/editor.md.svg) ![](https://img.shields.io/github/release/pandao/editor.md.svg) ![](https://img.shields.io/github/issues/pandao/editor.md.svg) ![](https://img.shields.io/bower/v/editor.md.svg)\n\n# Heading 1\n## Heading 2\n### Heading 3\n#### Heading 4\n##### Heading 5\n###### Heading 6\n# Heading 1 link [Heading link](https://github.com/pandao/editor.md \"Heading link\")\n## Heading 2 link [Heading link](https://github.com/pandao/editor.md \"Heading link\")\n### Heading 3 link [Heading link](https://github.com/pandao/editor.md \"Heading link\")\n#### Heading 4 link [Heading link](https://github.com/pandao/editor.md \"Heading link\") Heading link [Heading link](https://github.com/pandao/editor.md \"Heading link\")\n##### Heading 5 link [Heading link](https://github.com/pandao/editor.md \"Heading link\")\n###### Heading 6 link [Heading link](https://github.com/pandao/editor.md \"Heading link\")\n\n#### 标题（用底线的形式）Heading (underline)\n\nThis is an H1\n=============\n\nThis is an H2\n-------------\n\n### 字符效果和横线等\n\n----\n\n~~删除线~~ <s>删除线（开启识别HTML标签时）</s>\n*斜体字*      _斜体字_\n**粗体**  __粗体__\n***粗斜体*** ___粗斜体___\n\n上标：X<sub>2</sub>，下标：O<sup>2</sup>\n\n**缩写(同HTML的abbr标签)**\n\n> 即更长的单词或短语的缩写形式，前提是开启识别HTML标签时，已默认开启\n\nThe <abbr title=\"Hyper Text Markup Language\">HTML</abbr> specification is maintained by the <abbr title=\"World Wide Web Consortium\">W3C</abbr>.\n\n### 引用 Blockquotes\n\n> 引用文本 Blockquotes\n\n引用的行内混合 Blockquotes\n\n> 引用：如果想要插入空白换行`即<br />标签`，在插入处先键入两个以上的空格然后回车即可，[普通链接](http://localhost/)。\n\n### 锚点与链接 Links\n\n[普通链接](http://localhost/)\n\n[普通链接带标题](http://localhost/ \"普通链接带标题\")\n\n直接链接：<https: //github.com>\n\n[锚点链接][anchor-id]\n\n[anchor-id]: http://www.this-anchor-link.com/\n\nGFM a-tail link @pandao\n\n> @pandao\n\n### 多语言代码高亮 Codes\n\n#### 行内代码 Inline code\n\n执行命令：`npm install marked`\n\n#### 缩进风格\n\n即缩进四个空格，也做为实现类似`<pre>`预格式化文本(Preformatted Text)的功能。\n\n<?php\necho \"Hello world!\";\n?>\n\n预格式化文本：\n\n| First Header  | Second Header |\n| ------------- | ------------- |\n| Content Cell  | Content Cell  |\n| Content Cell  | Content Cell  |\n\n#### JS代码　\n\n```javascript\nfunction test(){\nconsole.log(\"Hello world!\");\n}\n\n(function(){\nvar box = function(){\nreturn box.fn.init();\n};\n\nbox.prototype = box.fn = {\ninit : function(){\n    console.log(\'box.init()\');\n\n    return this;\n},\n\nadd : function(str){\n    alert(\"add\", str);\n\n    return this;\n},\n\nremove : function(str){\n    alert(\"remove\", str);\n\n    return this;\n}\n};\n\nbox.fn.init.prototype = box.fn;\n\nwindow.box =box;\n})();\n\nvar testBox = box();\ntestBox.add(\"jQuery\").remove(\"jQuery\");\n```\n\n#### HTML代码 HTML codes\n\n```html\n<!DOCTYPE html>\n\n<html>\n<head>\n<mate charest=\"utf-8\" />\n\n<title>Hello world!</title>\n</head>\n<body>\n<h1>Hello world!</h1>\n</body>\n</html>\n```\n\n### 图片 Images\n\nImage:\n\n![](https://pandao.github.io/editor.md/examples/images/4.jpg)\n\n> Follow your heart.\n\n![](https://pandao.github.io/editor.md/examples/images/8.jpg)\n\n> 图为：厦门白城沙滩\n\n图片加链接 (Image + Link)：\n\n[![](https://pandao.github.io/editor.md/examples/images/7.jpg)](https://pandao.github.io/editor.md/examples/images/7.jpg \"李健首张专辑《似水流年》封面\")\n\n> 图为：李健首张专辑《似水流年》封面\n\n----\n\n### 列表 Lists\n\n#### 无序列表（减号）Unordered Lists (-)\n\n- 列表一\n- 列表二\n- 列表三\n\n#### 无序列表（星号）Unordered Lists (*)\n\n* 列表一\n* 列表二\n* 列表三\n\n#### 无序列表（加号和嵌套）Unordered Lists (+)\n\n+ 列表一\n+ 列表二\n+ 列表二-1\n+ 列表二-2\n+ 列表二-3\n+ 列表三\n* 列表一\n* 列表二\n* 列表三\n\n#### 有序列表 Ordered Lists (-)\n\n1. 第一行\n2. 第二行\n3. 第三行\n\n#### GFM task list\n\n- [x] GFM task list 1\n- [x] GFM task list 2\n- [ ] GFM task list 3\n- [ ] GFM task list 3-1\n- [ ] GFM task list 3-2\n- [ ] GFM task list 3-3\n- [ ] GFM task list 4\n- [ ] GFM task list 4-1\n- [ ] GFM task list 4-2\n\n----\n\n### 绘制表格 Tables\n\n| 项目        | 价格   |  数量  |\n| --------   | -----:  | :----:  |\n| 计算机      | $1600   |   5     |\n| 手机        |   $12   |   12   |\n| 管线        |    $1    |  234  |\n\nFirst Header  | Second Header\n------------- | -------------\nContent Cell  | Content Cell\nContent Cell  | Content Cell\n\n| First Header  | Second Header |\n| ------------- | ------------- |\n| Content Cell  | Content Cell  |\n| Content Cell  | Content Cell  |\n\n| Function name | Description                    |\n| ------------- | ------------------------------ |\n| `help()`      | Display the help window.       |\n| `destroy()`   | **Destroy your computer!**     |\n\n| Left-Aligned  | Center Aligned  | Right Aligned |\n| :------------ |:---------------:| -----:|\n| col 3 is      | some wordy text | $1600 |\n| col 2 is      | centered        |   $12 |\n| zebra stripes | are neat        |    $1 |\n\n| Item      | Value |\n| --------- | -----:|\n| Computer  | $1600 |\n| Phone     |   $12 |\n| Pipe      |    $1 |\n\n----\n\n#### 特殊符号 HTML Entities Codes\n\n&copy; &  &uml; &trade; &iexcl; &pound;\n&amp; &lt; &gt; &yen; &euro; &reg; &plusmn; &para; &sect; &brvbar; &macr; &laquo; &middot;\n\nX&sup2; Y&sup3; &frac34; &frac14;  &times;  &divide;   &raquo;\n\n18&ordm;C  &quot;  &apos;\n\n#### 反斜杠 Escape\n\n\\*literal asterisks\\*\n\n### End', '2020-02-14 15:48:20.921606', '2020-02-14 15:48:20.921599', '1', '0', '0', '0', '0', '4', '0');
INSERT INTO `topic` VALUES ('35', '基本完成', '基本完成基本完成基本完成基本完成基本完成基本完成基本完成基本完成', '2020-02-14 22:06:36.784067', '2020-02-14 22:06:36.784060', '1', '0', '0', '0', '0', '0', '1');
INSERT INTO `topic` VALUES ('36', 'table', '|   列1   |   列2   |   列3   |   列4   |\n| :-------| :-------| :-------| :-------|\n|   s      |      32   |   3      |    31     |', '2020-02-14 22:33:19.927129', '2020-02-14 22:33:19.927119', '1', '0', '0', '0', '0', '0', '1');

-- ----------------------------
-- Table structure for verify
-- ----------------------------
DROP TABLE IF EXISTS `verify`;
CREATE TABLE `verify` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `VerifyCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ExpireTime` datetime(6) NOT NULL,
  `UserId` int(11) NOT NULL,
  `VerifyType` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of verify
-- ----------------------------
INSERT INTO `verify` VALUES ('1', '1321', '2020-02-09 21:01:28.818229', '1', '0');
INSERT INTO `verify` VALUES ('2', '4603', '2020-02-09 21:27:58.406595', '1', '0');
INSERT INTO `verify` VALUES ('3', '4815', '2020-02-09 21:28:04.132106', '1', '0');
INSERT INTO `verify` VALUES ('4', '8149', '2020-02-09 21:28:05.175559', '1', '0');
INSERT INTO `verify` VALUES ('5', '8198', '2020-02-09 21:28:06.207911', '1', '0');
INSERT INTO `verify` VALUES ('6', '4496', '2020-02-09 21:28:07.047547', '1', '0');
INSERT INTO `verify` VALUES ('7', '6332', '2020-02-09 21:28:07.827114', '1', '0');
INSERT INTO `verify` VALUES ('8', '3163', '2020-02-09 21:28:08.582069', '1', '0');
INSERT INTO `verify` VALUES ('9', '1201', '2020-02-09 21:28:09.420097', '1', '0');
INSERT INTO `verify` VALUES ('10', '0522', '2020-02-09 21:28:10.127164', '1', '0');
INSERT INTO `verify` VALUES ('11', '3623', '2020-02-09 21:28:10.814933', '1', '0');
INSERT INTO `verify` VALUES ('12', '0160', '2020-02-09 21:28:11.436768', '1', '0');
INSERT INTO `verify` VALUES ('13', '0794', '2020-02-09 21:46:39.025010', '1', '1');
INSERT INTO `verify` VALUES ('14', '4426', '2020-02-09 22:04:18.250323', '1', '1');
INSERT INTO `verify` VALUES ('15', '6363', '2020-02-09 22:14:17.902956', '1', '1');
INSERT INTO `verify` VALUES ('16', '3073', '2020-02-09 22:31:36.295893', '1', '2');

-- ----------------------------
-- Table structure for __efmigrationshistory
-- ----------------------------
DROP TABLE IF EXISTS `__efmigrationshistory`;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of __efmigrationshistory
-- ----------------------------
INSERT INTO `__efmigrationshistory` VALUES ('20200208012248_test2', '3.1.1');
