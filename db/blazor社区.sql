/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2019/12/30 13:28:57                          */
/*==============================================================*/


drop table if exists Point;

drop table if exists Reply;

drop table if exists ThirdAccount;

drop table if exists Topic;

drop table if exists TopicId;

drop table if exists User;

drop table if exists UserAddress;

drop table if exists UserRealVerification;

/*==============================================================*/
/* Table: Point                                                 */
/*==============================================================*/
create table Point
(
   Id                   int not null auto_increment,
   Access               int comment '0：签到，1：发帖，2：回帖，3:精华帖，4：其他',
   Score                int comment '分数',
   Description          varchar(100) comment '说明',
   UserId               int comment '用户Id',
   primary key (Id)
);

/*==============================================================*/
/* Table: Reply                                                 */
/*==============================================================*/
create table Reply
(
   Id                   int not null auto_increment,
   Content              text not null comment '内容',
   PublishTime          datetime not null comment '发表时间',
   ModifyTime           datetime comment '修改时间',
   UserId               int not null comment '发帖人Id',
   TopicId              int not null comment '主题帖Id',
   Status               int comment '0：正常，-1，已删除',
   Favor                int comment '赞数量',
   primary key (Id)
);

/*==============================================================*/
/* Table: ThirdAccount                                          */
/*==============================================================*/
create table ThirdAccount
(
   Id                   int not null,
   OAuthLogin           int not null,
   OAuthName            varchar(50),
   OAuthId              varchar(50) not null,
   NickName             varchar(50),
   Photo                varchar(200),
   UserId               int not null,
   HomePage             varchar(200),
   primary key (Id)
);

/*==============================================================*/
/* Table: Topic                                                 */
/*==============================================================*/
create table Topic
(
   Id                   int not null,
   Title                varchar(200) not null comment '标题',
   Content              text not null comment '内容',
   PublishTime          datetime not null comment '发表时间',
   ModifyTime           datetime comment '修改时间',
   UserId               int not null comment '发帖人Id',
   Status               int comment '0：正常，-1：已结帖',
   Hot                  int comment '人气',
   Top                  int comment '是否置顶',
   Good                 int comment '是否精华',
   TopicType            int comment '0：提问，1：分享，2：讨论，3：建议，4：公告',
   ReplyCount           int comment '回复帖数量',
   primary key (Id)
);

/*==============================================================*/
/* Table: TopicId                                               */
/*==============================================================*/
create table TopicId
(
   Id                   int not null,
   TopicId              int not null comment '主题帖Id',
   Status               int comment '0：正常，-1：已取消关注',
   FollowTime           datetime not null comment '关注时间',
   UserId               int not null comment '用户Id',
   primary key (Id)
);

alter table TopicId comment '关注帖Id';

/*==============================================================*/
/* Table: User                                                  */
/*==============================================================*/
create table User
(
   Id                   int not null auto_increment,
   Account              varchar(20) not null comment '账号',
   Cypher               varchar(20) not null comment '密码',
   NickName             varchar(30) comment '昵称',
   Mobile               varchar(15) not null comment '手机号',
   Avatar               varchar(200) comment '头像',
   Email                varchar(50) comment '邮箱',
   Sex                  int comment '性别',
   Signature            varchar(200) comment '签名',
   RegisterDate         datetime not null comment '注册日期',
   Status               int comment '0：正常，-1：已删除/冻结',
   Level                int comment '等级',
   Points               int comment '积分',
   LastLoginDate        datetime not null comment '最后登录时间',
   LastLoginType        int not null comment '最后登录方式',
   LastLoginAddr        varchar(100) not null comment '最后登录地区',
   primary key (Id)
);

/*==============================================================*/
/* Table: UserAddress                                           */
/*==============================================================*/
create table UserAddress
(
   Id                   int not null,
   Country              varchar(50),
   Province             varchar(100),
   City                 varchar(100),
   District             char(10),
   UserId               int,
   primary key (Id)
);

/*==============================================================*/
/* Table: UserRealVerification                                  */
/*==============================================================*/
create table UserRealVerification
(
   Id                   int not null,
   IdentityNo           varchar(20) not null comment '身份证',
   UserName             varchar(50) not null comment '姓名',
   PhotoFront           varchar(200) not null comment '身份证正面',
   PhotoBehind          varchar(200) not null comment '身份证背面',
   UserId               int not null comment '用户Id',
   primary key (Id)
);

alter table UserRealVerification comment '用户实名认证';

alter table Point add constraint FK_Reference_5 foreign key (UserId)
      references User (Id) on delete restrict on update restrict;

alter table Reply add constraint FK_Reference_4 foreign key (TopicId)
      references Topic (Id) on delete restrict on update restrict;

alter table ThirdAccount add constraint FK_Reference_2 foreign key (UserId)
      references User (Id) on delete restrict on update restrict;

alter table Topic add constraint FK_Reference_8 foreign key (UserId)
      references User (Id) on delete restrict on update restrict;

alter table TopicId add constraint FK_Reference_6 foreign key (UserId)
      references User (Id) on delete restrict on update restrict;

alter table TopicId add constraint FK_Reference_7 foreign key (TopicId)
      references Topic (Id) on delete restrict on update restrict;

alter table UserAddress add constraint FK_Reference_1 foreign key (UserId)
      references User (Id) on delete restrict on update restrict;

alter table UserRealVerification add constraint FK_Reference_3 foreign key (UserId)
      references User (Id) on delete restrict on update restrict;

