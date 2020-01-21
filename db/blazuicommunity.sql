/*
Navicat MySQL Data Transfer

Source Server         : mysql_localhost
Source Server Version : 80018
Source Host           : localhost:3306
Source Database       : blazuicommunity

Target Server Type    : MYSQL
Target Server Version : 80018
File Encoding         : 65001

Date: 2020-01-21 22:50:47
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for follow
-- ----------------------------
DROP TABLE IF EXISTS `follow`;
CREATE TABLE `follow` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TopicId` int(11) NOT NULL,
  `Status` int(11) DEFAULT '0',
  `FollowTime` datetime NOT NULL,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Reference_7` (`TopicId`),
  KEY `FK_Reference_6` (`UserId`),
  CONSTRAINT `follow_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `follow_ibfk_2` FOREIGN KEY (`TopicId`) REFERENCES `topic` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of follow
-- ----------------------------

-- ----------------------------
-- Table structure for point
-- ----------------------------
DROP TABLE IF EXISTS `point`;
CREATE TABLE `point` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Access` int(11) DEFAULT NULL,
  `Score` int(11) DEFAULT NULL,
  `Description` varchar(100) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Reference_5` (`UserId`),
  CONSTRAINT `point_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT
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
  `Content` text NOT NULL,
  `PublishTime` datetime NOT NULL,
  `ModifyTime` datetime DEFAULT NULL,
  `UserId` int(11) NOT NULL,
  `TopicId` int(11) NOT NULL,
  `Status` int(11) DEFAULT NULL,
  `Favor` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Reference_4` (`TopicId`),
  CONSTRAINT `reply_ibfk_1` FOREIGN KEY (`TopicId`) REFERENCES `topic` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of reply
-- ----------------------------
INSERT INTO `reply` VALUES ('1', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('2', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('3', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('4', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('5', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('6', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('7', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('8', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('9', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('10', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('11', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('12', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('13', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('14', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('15', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('16', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('17', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('18', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('19', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('20', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('21', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('22', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');
INSERT INTO `reply` VALUES ('23', '121', '2020-01-20 12:17:48', '2020-01-20 12:17:51', '1', '1', '0', '123');

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of syslog
-- ----------------------------
INSERT INTO `syslog` VALUES ('1', '321333333333333', '13131', '异常，，，，', '1', '2020-01-14 21:31:35.274893', '1', '2020-01-14 21:31:35.275329', '1');

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
  `OAuthLogin` int(11) NOT NULL,
  `OAuthName` varchar(50) DEFAULT NULL,
  `OAuthId` varchar(50) NOT NULL,
  `NickName` varchar(50) DEFAULT NULL,
  `Photo` varchar(200) DEFAULT NULL,
  `UserId` int(11) NOT NULL,
  `HomePage` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Reference_2` (`UserId`),
  CONSTRAINT `thirdaccount_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT
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
  `Title` varchar(200) NOT NULL,
  `Content` text NOT NULL,
  `PublishTime` datetime NOT NULL,
  `ModifyTime` datetime DEFAULT NULL,
  `UserId` int(11) NOT NULL,
  `Status` int(11) DEFAULT NULL,
  `Hot` int(11) DEFAULT NULL,
  `Top` int(11) DEFAULT NULL,
  `Good` int(11) DEFAULT NULL,
  `TopicType` int(11) DEFAULT NULL,
  `ReplyCount` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Reference_8` (`UserId`),
  CONSTRAINT `topic_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of topic
-- ----------------------------
INSERT INTO `topic` VALUES ('1', '12', '1312', '2020-01-19 21:40:53', '2020-01-19 21:40:57', '1', '1', '121', '1', '1', '1', '11');

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Account` varchar(20) NOT NULL,
  `Cypher` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `NickName` varchar(30) DEFAULT NULL,
  `Mobile` varchar(15) NOT NULL,
  `Avatar` varchar(200) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Sex` int(11) DEFAULT NULL,
  `Signature` varchar(200) DEFAULT NULL,
  `RegisterDate` datetime NOT NULL,
  `Status` int(11) DEFAULT NULL,
  `Level` int(11) DEFAULT NULL,
  `Points` int(11) DEFAULT NULL,
  `LastLoginDate` datetime NOT NULL,
  `LastLoginType` int(11) NOT NULL,
  `LastLoginAddr` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=64 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('1', 'abcde', 'e10adc3949ba59abbe56e057f20f883e', 'vicky', '', 'http://www.baidu.com/image/ssa', '212718@qq.com', '0', ' stay hangur stay foolish', '0001-01-01 00:00:00', '0', '1', '100', '0001-01-01 00:00:00', '0', '2131');
INSERT INTO `user` VALUES ('2', 'abcdefg123', 'e10adc3949ba59abbe56e057f20f883e', 'vicky', '12131321321312', 'http://www.baidu.com/image/ssa', '212718@qq.com', '0', ' stay hangur stay foolish', '2020-01-16 17:18:20', '0', '1', '100', '2020-01-16 17:18:20', '0', '2131');
INSERT INTO `user` VALUES ('33', '123120', 'e10adc3949ba59abbe56e057f20f883e', '3213123120', '1231231', '123123120', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('34', '123121', 'e10adc3949ba59abbe56e057f20f883e', '3213123121', '1231231', '123123121', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('35', '123122', 'e10adc3949ba59abbe56e057f20f883e', '3213123122', '1231231', '123123122', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('36', '123123', 'e10adc3949ba59abbe56e057f20f883e', '3213123123', '1231231', '123123123', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('37', '123124', 'e10adc3949ba59abbe56e057f20f883e', '3213123124', '1231231', '123123124', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('38', '123125', 'e10adc3949ba59abbe56e057f20f883e', '3213123125', '1231231', '123123125', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('39', '123126', 'e10adc3949ba59abbe56e057f20f883e', '3213123126', '1231231', '123123126', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('40', '123127', 'e10adc3949ba59abbe56e057f20f883e', '3213123127', '1231231', '123123127', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('41', '123128', 'e10adc3949ba59abbe56e057f20f883e', '3213123128', '1231231', '123123128', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('42', '123129', 'e10adc3949ba59abbe56e057f20f883e', '3213123129', '1231231', '123123129', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('43', '1231210', 'e10adc3949ba59abbe56e057f20f883e', '32131231210', '1231231', '1231231210', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('44', '1231211', 'e10adc3949ba59abbe56e057f20f883e', '32131231211', '1231231', '1231231211', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('45', '1231212', 'e10adc3949ba59abbe56e057f20f883e', '32131231212', '1231231', '1231231212', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('46', '1231213', 'e10adc3949ba59abbe56e057f20f883e', '32131231213', '1231231', '1231231213', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('47', '1231214', 'e10adc3949ba59abbe56e057f20f883e', '32131231214', '1231231', '1231231214', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('48', '1231215', 'e10adc3949ba59abbe56e057f20f883e', '32131231215', '1231231', '1231231215', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('49', '1231216', 'e10adc3949ba59abbe56e057f20f883e', '32131231216', '1231231', '1231231216', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('50', '1231217', 'e10adc3949ba59abbe56e057f20f883e', '32131231217', '1231231', '1231231217', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('51', '1231218', 'e10adc3949ba59abbe56e057f20f883e', '32131231218', '1231231', '1231231218', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');
INSERT INTO `user` VALUES ('52', '1231219', 'e10adc3949ba59abbe56e057f20f883e', '32131231219', '1231231', '1231231219', 'eeee', '1', '21312', '2020-01-18 20:58:01', '0', '1', '1', '2020-01-18 20:58:01', '0', '1312312');

-- ----------------------------
-- Table structure for useraddress
-- ----------------------------
DROP TABLE IF EXISTS `useraddress`;
CREATE TABLE `useraddress` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Country` varchar(50) DEFAULT NULL,
  `Province` varchar(100) DEFAULT NULL,
  `City` varchar(100) DEFAULT NULL,
  `District` char(10) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Reference_1` (`UserId`),
  CONSTRAINT `useraddress_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of useraddress
-- ----------------------------

-- ----------------------------
-- Table structure for userrealverification
-- ----------------------------
DROP TABLE IF EXISTS `userrealverification`;
CREATE TABLE `userrealverification` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdentityNo` varchar(20) NOT NULL,
  `UserName` varchar(50) NOT NULL,
  `PhotoFront` varchar(200) NOT NULL,
  `PhotoBehind` varchar(200) NOT NULL,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Reference_3` (`UserId`),
  CONSTRAINT `userrealverification_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of userrealverification
-- ----------------------------
