---------�豸����------------

-- Create table TBLEquipmentType
create table TBLEquipmentType
(
  EQPTYPE   NVARCHAR2(40) not null,
  EQPTYPEDESC   NVARCHAR2(100),
  EATTRIBUTE1  NVARCHAR2(100),
  MUSER         NVARCHAR2(40),
  MDATE         NUMBER(8),
  MTIME         NUMBER(6)
)
/
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLEquipmentType  add constraint TBLEquipmentType_PK primary key (EQPTYPE)
/

---------�豸ά������------------

-- Create table TBLEquipmentTSType
create table TBLEquipmentTSType
(
  EQPTSTYPE   NVARCHAR2(40) not null,
  EQPTSTYPEDESC   NVARCHAR2(100),
  EATTRIBUTE1  NVARCHAR2(100),
  MUSER         NVARCHAR2(40),
  MDATE         NUMBER(8),
  MTIME         NUMBER(6)
)
/
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLEquipmentTSType  add constraint TBLEquipmentTSType_PK primary key (EQPTSTYPE)

/

---------�豸�嵥------------
-- Create table TBLEQUIPMENT

create table TBLEQUIPMENT
(
  EQPID       VARCHAR2(40) not null,
  EQPNAME       VARCHAR2(100),
  EQPDESC     VARCHAR2(100),  
  MODEL       VARCHAR2(40),
  TYPE        VARCHAR2(40),
  EQPTYPE     VARCHAR2(40),  
  EQPCOMPANY  VARCHAR2(100),
  CONTACT     VARCHAR2(40),
  TELPHONE    VARCHAR2(40),
  EATTRIBUTE1 VARCHAR2(100),
  EATTRIBUTE2 VARCHAR2(100),
  EATTRIBUTE3 VARCHAR2(100),
  EQPSTATUS   VARCHAR2(40),
  MUSER       VARCHAR2(40),
  MDATE       NUMBER(22) not null,
  MTIME       NUMBER(22) not null
)

-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLEQUIPMENT
  add constraint TBLEQUIPMENT_PK primary key (EQPID)
/



---------�豸״̬�����־------------
-- Create table TBLEQPLOG

create table TBLEQPLOG
(
  Serial     NUMBER(22) not null,
  EQPID  VARCHAR2(40) not null,
  EQPSTATUS    VARCHAR2(40),
  MEMO  VARCHAR2(400),
  MUSER      VARCHAR2(40) not null,
  MDATE      NUMBER(8) not null,
  MTIME      NUMBER(6) not null
)
/
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLEQPLOG
  add constraint TBLEQPLOG_PK primary key (SERIAL)
/
  
CREATE SEQUENCE TBLEQPLOG_id_s
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 20;
/

CREATE OR REPLACE TRIGGER TBLEQPLOG_BRI1
  BEFORE INSERT ON TBLEQPLOG
  REFERENCING NEW AS NEW OLD AS OLD
  FOR EACH ROW
BEGIN
  SELECT TBLEQPLOG_id_s.NEXTVAL INTO :NEW.serial FROM DUAL;
END;

/

---------�豸ά����־------------
-- Create table TBLEQPTSLOG
create table TBLEQPTSLOG
(
  Serial     NUMBER(22) not null,
  EQPID  VARCHAR2(40) not null,
  FINDUSER VARCHAR2(40) not null,
  FINDMDATE NUMBER(8) not null,
  FINDMTIME NUMBER(6) not null,
  TSINFO	VARCHAR2(400),	
  REASON    VARCHAR2(400),
  solution    VARCHAR2(400),
  Result    VARCHAR2(400),
  TSType    VARCHAR2(40),
  STATUS	VARCHAR2(40) not null,
  Duration	NUMBER(8) not null,
  MEMO  VARCHAR2(400),
  MUSER      VARCHAR2(40) not null,
  MDATE      NUMBER(8) not null,
  MTIME      NUMBER(6) not null
)
/
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLEQPTSLOG
  add constraint TBLEQPTSLOG_PK primary key (Serial)
/


CREATE SEQUENCE TBLEQPTSLOG_id_s
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 20
;
/
CREATE OR REPLACE TRIGGER TBLEQPTSLOG_BRI1
  BEFORE INSERT ON TBLEQPTSLOG
  REFERENCING NEW AS NEW OLD AS OLD
  FOR EACH ROW
BEGIN
  SELECT TBLEQPTSLOG_id_s.NEXTVAL INTO :NEW.serial FROM DUAL;
END
; 
/

---------�豸�����ƻ�------------

-- Create table TBLEQPMaintenance
create table TBLEQPMaintenance
(
  EQPID     VARCHAR2(40) not null,
  MaintainTYPE    VARCHAR2(40) not null, 
  MaintainITEM    VARCHAR2(400) not null,
  CYCLETYPE       VARCHAR2(40) not null,
  Frequency     NUMBER(6) not null,
  MUSER       VARCHAR2(40) not null,
  MDATE       NUMBER(8) not null,
  MTIME       NUMBER(6) not null
)
/

alter table TBLEQPMaintenance
  add primary key (EQPID, MaintainITEM,MaintainTYPE)
/
  
  
  ---------TBLEQPMaintainLog------------

-- Create table TBLEQPMaintainLog

 create table TBLEQPMaintainLog
(
  SERIAL   NUMBER(22) not null,
  EQPID     VARCHAR2(40) not null,
  MaintainTYPE    VARCHAR2(400) not null,   
  MaintainITEM    VARCHAR2(400) not null,
  MEMO       VARCHAR2(400),
  RESULT	VARCHAR2(40) not null,  
  MUSER       VARCHAR2(40) not null,
  MDATE       NUMBER(8) not null,
  MTIME       NUMBER(6) not null
)
/

alter table TBLEQPMaintainLog
  add constraint TBLEQPMaintainLog_PK primary key (SERIAL)
  /
  
  

  CREATE SEQUENCE TBLEQPMaintainLog_id_s
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 20;
/

CREATE OR REPLACE TRIGGER TBLEQPMaintainLog_BRI1
  BEFORE INSERT ON TBLEQPMaintainLog
  REFERENCING NEW AS NEW OLD AS OLD
  FOR EACH ROW
BEGIN
  SELECT TBLEQPMaintainLog_id_s.NEXTVAL INTO :NEW.serial FROM DUAL;
END;

/


  ---------�豸Ч�ʻ�������ά��------------
-- Create table TBLEQPOEE 	
create table TBLEQPOEE
(
  EQPID  VARCHAR2(40) not null,
  WORKTIME    NUMBER(8) not null,
  SSCODE    VARCHAR2(40) not null,
  OPCODE    VARCHAR2(40) not null,
  MUSER      VARCHAR2(40) not null,
  MDATE      NUMBER(8) not null,
  MTIME      NUMBER(6) not null
)
/
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLEQPOEE
  add constraint TBLEQPOEE_PK primary key (EQPID)
/

  ---------�豸ʹ�����ά��------------
-- Create table TBLEQPUSEINFO  
create table TBLEQPUSEINFO
(
  EQPID  VARCHAR2(40) not null,
  USEDATE    NUMBER(8) not null,
  ONTIME    NUMBER(6) not null,
  OFFTIME    NUMBER(6) not null,
  RUNuration    NUMBER(8) not null,
  STOPDuration    NUMBER(8) not null,
  MUSER      VARCHAR2(40) not null,
  MDATE      NUMBER(8) not null,
  MTIME      NUMBER(6) not null
)
/
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLEQPUSEINFO
  add constraint TBLEQPUSEINFO_PK primary key (EQPID,USEDATE)
  
/

--------------------------------�豸�˵�------------------------------

  ---------�豸����------------
insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQUIPMENT', 'SMT_MANAGER', '', 'B/S', 'Alpha', '�豸����', 41, '', 'ADMIN', 20100907, 105005, '1', '1', '', '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQUIPMENT', 'EQUIPMENT', 'SMT_MANAGER', '�豸����', 41, 'B/S', 'ADMIN', 20100831, 194135, '', '0');

  ---------�豸ά��------------
  
insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQUIPMENTTYPEMP', 'EQUIPMENT', '', 'B/S', 'Alpha', '�豸����ά��', 1, '', 'ADMIN', 20100809, 123250, '1', '1', 'BaseSetting/FEquimentTypeMP.aspx', '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQUIPMENTTYPEMP', 'EQUIPMENTTYPEMP', 'EQUIPMENT', '�豸����ά��', 1, 'B/S', 'ADMIN', 20100809, 124257, '', '0');

  
insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQUIPMENTTSTYPEMP', 'EQUIPMENT', '', 'B/S', 'Alpha', '�豸ά������ά��', 3, '', 'ADMIN', 20100809, 123250, '1', '1', 'BaseSetting/FEquimentTsTypeMP.aspx', '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQUIPMENTTSTYPEMP', 'EQUIPMENTTSTYPEMP', 'EQUIPMENT', '�豸ά������ά��', 3, 'B/S', 'ADMIN', 20100809, 124257, '', '0');

  
insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQUIPMENTMP', 'EQUIPMENT', '', 'B/S', 'Alpha', '�豸ά��', 2, '', 'ADMIN', 20100809, 123250, '1', '1', 'BaseSetting/FEquimentMP.aspx', '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQUIPMENTMP', 'EQUIPMENTMP', 'EQUIPMENT', '�豸ά��', 2, 'B/S', 'ADMIN', 20100809, 124257, '', '0');



insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQOLOG', 'EQUIPMENT', '', 'B/S', 'Alpha', '�豸����', 9, '', 'ADMIN', 20110308, 230905, '1', '1', 'BaseSetting/FEQPLOG.aspx', '', '0');

insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQOTSLOG', 'EQUIPMENT', '', 'B/S', 'Alpha', 'ά�޼�¼', 10, '', 'ADMIN', 20110308, 230905, '1', '1', 'BaseSetting/FEQPTSLOG.aspx', '', '0');


--�豸����
insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQPMAINTENANCE', 'EQUIPMENT', '', 'B/S', 'Alpha', '�豸����', 14, '', 'ADMIN', 20110315, 151717, '1', '1', '', '', '0');

insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQPMAINTAINLOG', 'EQPMAINTENANCE', '', 'B/S', 'Alpha', '������־ά��', 16, '', 'ADMIN', 20110315, 151831, '1', '1', 'BaseSetting/FEQPMaintainLog.aspx', '', '0');

insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQPMAINTAINLOGADD', 'EQPMAINTENANCE', '', 'B/S', 'Alpha', '������־����', 17, '', 'ADMIN', 20110315, 151844, '1', '1', 'BaseSetting/FEQPMaintainLogAdd.aspx', '', '0');

insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQPMAINTENANCEPLAN', 'EQPMAINTENANCE', '', 'B/S', 'Alpha', '�豸�����ƻ�ά��', 15, '', 'ADMIN', 20110315, 151806, '1', '1', 'BaseSetting/FEQPMaintenance.aspx', '', '0');


insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQPMAINTENANCE', 'EQPMAINTENANCE', 'EQUIPMENT', '�豸����', 10, 'B/S', 'ADMIN', 20110315, 155824, '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQPMAINTAINLOG', 'EQPMAINTAINLOG', 'EQPMAINTENANCE', '������־ά��', 2, 'B/S', 'ADMIN', 20110315, 152245, '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQPMAINTENANCEPLAN', 'EQPMAINTENANCEPLAN', 'EQPMAINTENANCE', '�豸�����ƻ�ά��', 1, 'B/S', 'ADMIN', 20110315, 152156, '', '0');


 -- �豸Ч�ʷ���  2011 03 14
insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQUIPMENTOEE', 'EQUIPMENT', '', 'B/S', 'Alpha', '�豸Ч�ʷ���', 42, '', 'ADMIN', 20110311, 130326, '1', '1', '', '', '0');

insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQPOEE', 'EQUIPMENTOEE', '', 'B/S', 'Alpha', '�豸Ч�ʻ�����Ϣά��', 1, '', 'ADMIN', 20110311, 130454, '1', '1', 'BaseSetting/FEQPOEE.aspx', '', '0');

insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQPUSEINFO', 'EQUIPMENTOEE', '', 'B/S', 'Alpha', '�豸ʹ�����ά��', 2, '', 'ADMIN', 20110311, 130603, '1', '1', 'BaseSetting/FEQPUseInfo.aspx', '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQUIPMENTOEE', 'EQUIPMENTOEE', 'EQUIPMENT', '�豸Ч�ʷ���', 42, 'B/S', 'ADMIN', 20110311, 131209, '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQPOEE', 'EQPOEE', 'EQUIPMENTOEE', '�豸Ч�ʻ�����Ϣά��', 1, 'B/S', 'ADMIN', 20110311, 131247, '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('EQPUSEINFO', 'EQPUSEINFO', 'EQUIPMENTOEE', '�豸ʹ�����', 2, 'B/S', 'ADMIN', 20110311, 131335, '', '0');



insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQPMAINTAINAUTOREMIND', 'CS_SMT', '', 'C/S', 'Alpha', '�豸�����Զ�����', 50, '', 'ADMIN', 20110623, 102742, '1', '1', 'BenQGuru.eMES.Client.FEQPMaintainAutoRemind', '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('CS_EQPMAINTAINAUTOREMIND', 'EQPMAINTAINAUTOREMIND', 'CS_SMT', '�豸�����Զ�����', 50, 'C/S', 'ADMIN', 20110623, 102953, '', '0');



insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('EQPEFFICIENCYQUERY', 'CS_SMT', '', 'C/S', 'Alpha', '�豸Ч�ʷ���', 55, '', 'ADMIN', 20110623, 102742, '1', '1', 'BenQGuru.eMES.Client.FEQPEFficiencyQuery', '', '0');

insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('CS_EQPEFFICIENCYQUERY', 'EQPEFFICIENCYQUERY', 'CS_SMT', '�豸Ч�ʷ���', 55, 'C/S', 'ADMIN', 20110623, 102953, '', '0');





------------------ϵͳ����ά��----------

------------------�豸װ̬------------
insert into tblsysparamgroup (PARAMGROUPCODE, PARAMGROUPTYPE, PARAMGROUPDESC, MUSER, MDATE, MTIME, ISSYS, EATTRIBUTE1)
values ('EQPSTATUS', 'EQPSTATUS', '�豸װ̬', 'ADMIN', 20110308, 212555, '0', '');

insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('USEING', 'EQPSTATUS', 'USEING', '����', '', 'ADMIN', 20110309, 191040, '0', '0', '1', '');

insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('PRODUCTION', 'EQPSTATUS', 'PRODUCTION', 'Ͷ��', '', 'ADMIN', 20110308, 212714, '0', '0', '1', '');

insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('REINFORCE', 'EQPSTATUS', 'REINFORCE', 'ת��', '', 'ADMIN', 20110308, 212714, '0', '0', '1', '');

insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('UNUSE', 'EQPSTATUS', 'UNUSE', '����', '', 'ADMIN', 20110308, 212714, '0', '0', '1', '');

insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('SCRAP', 'EQPSTATUS', 'SCRAP', '����', '', 'ADMIN', 20110308, 212714, '0', '0', '1', '');




--1.��ɾ������ 
alter table TBLEQPMAINTENANCE drop constraint SYS_C00484596;


---2.��������

alter table TBLEQPMAINTENANCE
     add constraint TBLEQPMAINTENANCE_PK primary key (EQPID,MAINTAINTYPE,MAINTAINITEM);
