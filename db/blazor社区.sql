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
   Access               int comment '0��ǩ����1��������2��������3:��������4������',
   Score                int comment '����',
   Description          varchar(100) comment '˵��',
   UserId               int comment '�û�Id',
   primary key (Id)
);

/*==============================================================*/
/* Table: Reply                                                 */
/*==============================================================*/
create table Reply
(
   Id                   int not null auto_increment,
   Content              text not null comment '����',
   PublishTime          datetime not null comment '����ʱ��',
   ModifyTime           datetime comment '�޸�ʱ��',
   UserId               int not null comment '������Id',
   TopicId              int not null comment '������Id',
   Status               int comment '0��������-1����ɾ��',
   Favor                int comment '������',
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
   Title                varchar(200) not null comment '����',
   Content              text not null comment '����',
   PublishTime          datetime not null comment '����ʱ��',
   ModifyTime           datetime comment '�޸�ʱ��',
   UserId               int not null comment '������Id',
   Status               int comment '0��������-1���ѽ���',
   Hot                  int comment '����',
   Top                  int comment '�Ƿ��ö�',
   Good                 int comment '�Ƿ񾫻�',
   TopicType            int comment '0�����ʣ�1������2�����ۣ�3�����飬4������',
   ReplyCount           int comment '�ظ�������',
   primary key (Id)
);

/*==============================================================*/
/* Table: TopicId                                               */
/*==============================================================*/
create table TopicId
(
   Id                   int not null,
   TopicId              int not null comment '������Id',
   Status               int comment '0��������-1����ȡ����ע',
   FollowTime           datetime not null comment '��עʱ��',
   UserId               int not null comment '�û�Id',
   primary key (Id)
);

alter table TopicId comment '��ע��Id';

/*==============================================================*/
/* Table: User                                                  */
/*==============================================================*/
create table User
(
   Id                   int not null auto_increment,
   Account              varchar(20) not null comment '�˺�',
   Cypher               varchar(20) not null comment '����',
   NickName             varchar(30) comment '�ǳ�',
   Mobile               varchar(15) not null comment '�ֻ���',
   Avatar               varchar(200) comment 'ͷ��',
   Email                varchar(50) comment '����',
   Sex                  int comment '�Ա�',
   Signature            varchar(200) comment 'ǩ��',
   RegisterDate         datetime not null comment 'ע������',
   Status               int comment '0��������-1����ɾ��/����',
   Level                int comment '�ȼ�',
   Points               int comment '����',
   LastLoginDate        datetime not null comment '����¼ʱ��',
   LastLoginType        int not null comment '����¼��ʽ',
   LastLoginAddr        varchar(100) not null comment '����¼����',
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
   IdentityNo           varchar(20) not null comment '���֤',
   UserName             varchar(50) not null comment '����',
   PhotoFront           varchar(200) not null comment '���֤����',
   PhotoBehind          varchar(200) not null comment '���֤����',
   UserId               int not null comment '�û�Id',
   primary key (Id)
);

alter table UserRealVerification comment '�û�ʵ����֤';

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

