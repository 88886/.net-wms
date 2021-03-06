--Add column 
ALTER TABLE TBLERPMO ADD SERIAL NUMBER(22) NOT NULL;
ALTER TABLE TBLERPMO ADD MORemark VARCHAR2(500) DEFAULT ' ' NOT NULL;

-- Create sequence 
create sequence TBLERPMO_ID_S
minvalue 1
maxvalue 999999999999999999999999999
start with 141
increment by 1
cache 20;

-- Create TRIGGER 
CREATE OR REPLACE TRIGGER TBLERPMO_BRI1
  BEFORE INSERT ON TBLERPMO
  REFERENCING NEW AS NEW OLD AS OLD
  FOR EACH ROW
BEGIN
  SELECT TBLERPMO_ID_S.NEXTVAL INTO :NEW.SERIAL FROM DUAL;
END;


-- 删除来的主键 
 alter table TBLERPMO DROP PRIMARY KEY   ;
 
-- 设置 SERIAL 为主键
 alter table TBLERPMO
  add constraint TBLERPMO_PK primary key (SERIAL);  

-- 添加索引
create index INDEX_ITEMCODE on TBLERPMO (ITEMCODE);
create index INDEX_MOCODE_MOVER on TBLERPMO (MOCODE,MOVER);

-- Add column  
ALTER TABLE TBLMO ADD MORemark VARCHAR2(500)DEFAULT ' '  NOT NULL ;

insert into tblMOViewField (USERCODE, SEQ, FIELDNAME, DESCRIPTION, ISDEFAULT)
values ('MO_FIELD_LIST_SYSTEM_DEFAULT', 46, 'MORemark', 'Mo_MORemark', '');


-- create table TBLMOSTOCK
DROP TABLE TBLMOSTOCK;
 CREATE table TBLMOSTOCK 
(
  MOCODE VARCHAR2(40) NOT NULL,
  MOVER  VARCHAR2(40) NOT NULL,
  OPBOMVER  VARCHAR2(40) NOT NULL,
  ITEMCODE  VARCHAR2(40) NOT NULL,
  MITEMCODE VARCHAR2(40) NOT NULL,
  SOURCEWH VARCHAR2(40) NOT NULL,
  PLANQTY NUMBER(22,5) NOT NULL
);

-- 设置 SERIAL 为主键
 alter table TBLMOSTOCK
  add constraint TBLMOSTOCK_PK primary key (MOCODE,MOVER,ITEMCODE,MITEMCODE);  


DROP TABLE TBLERPMINNO ;
-- Create table
create table TBLERPMOSTOCK
(
  SERIAL    NUMBER(10) not null,
  MOCODE    VARCHAR2(40) not null,
  MOVER     VARCHAR2(40) not null,
  OPBOMVER  VARCHAR2(40) not null,
  ITEMCODE  VARCHAR2(40) not null,
  MITEMCODE VARCHAR2(40) not null,
  FLAG      VARCHAR2(1) not null,
  SOURCEWH  VARCHAR2(40) default ' ' not null,
  PLANQTY   NUMBER(22,5) default 0 not null
);
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLERPMOSTOCK
  add constraint TBLERPMOSTOCK_PK primary key (SERIAL);
-- Create/Recreate indexes 
create index INDEX_ERPMOSTOCK on TBLERPMOSTOCK (MOCODE, MOVER, MITEMCODE);


-- Create sequence 
create sequence TBLERPMOSTOCK_ID_S
minvalue 1
maxvalue 999999999999999999999999999
start with 141
increment by 1
cache 20;

-- Create TRIGGER 
CREATE OR REPLACE TRIGGER TBLERPMOSTOCK_BRI1
  BEFORE INSERT ON TBLERPMOSTOCK
  REFERENCING NEW AS NEW OLD AS OLD
  FOR EACH ROW
BEGIN
  SELECT TBLERPMOSTOCK_ID_S.NEXTVAL INTO :NEW.SERIAL FROM DUAL;
END;




ALTER TABLE TBLERPVENDOR DROP PRIMARY KEY;

CREATE INDEX VENDOR_INDEX_VENDORCODE ON TBLERPVENDOR(VENDORCODE);

ALTER TABLE TBLERPVENDOR MODIFY VENDORNAME VARCHAR2(100);

ALTER TABLE TBLVENDOR MODIFY VENDORNAME VARCHAR2(100);

ALTER TABLE TBLERPSBOM DROP PRIMARY KEY;

CREATE INDEX ERPSBOM_INDEX_ITEMCODE ON TBLERPSBOM(ITEMCODE);

CREATE INDEX ERPSBOM_INDEX_SBITEMCODE ON TBLERPSBOM(SBITEMCODE);

CREATE INDEX ERPSBOM_INDEX_SBOMVER ON TBLERPSBOM(SBOMVER);


TRUNCATE TABLE TBLERPSBOM;

ALTER TABLE TBLERPSBOM ADD SERIAL NUMBER(10) PRIMARY KEY;

 CREATE SEQUENCE SEQUENCE_ERPSBOM_SERIAL  
 INCREMENT BY 1     
     START WITH 1      
     NOMAXVALUE      
     NOCYCLE         
    CACHE 10; 
 
 CREATE TRIGGER TRIGGER_ERPSBOM_SERIAL
   BEFORE INSERT ON TBLERPSBOM
   FOR EACH ROW
 BEGIN
   SELECT SEQUENCE_ERPSBOM_SERIAL.NEXTVAL INTO :NEW.SERIAL FROM DUAL;
 END;


ALTER TABLE TBLERPSBOM DROP COLUMN SBITEMNAME;

ALTER TABLE TBLERPSBOM DROP COLUMN SBITEMDESC;

ALTER TABLE TBLERPSBOM DROP COLUMN ITEMDESC;



ALTER TABLE TBLERPITEM DROP PRIMARY KEY;

CREATE INDEX ITEM_INDEX_ITEMCODE ON TBLERPITEM(ITEMCODE);

ALTER TABLE TBLERPITEM MODIFY ITEMTYPE DEFAULT 'itemtype_finishedproduct';


ALTER TABLE TBLERPCUSTOMER DROP PRIMARY KEY;

CREATE INDEX CUSTOMER_INDEX_CUSTOMERCODE ON TBLERPCUSTOMER(CUSTOMERCODE);

ALTER TABLE TBLERPCUSTOMER MODIFY CUSTOMERNAME VARCHAR2(100);

ALTER TABLE TBLCUSTOMER MODIFY CUSTOMERNAME VARCHAR2(100);



ALTER TABLE TBLERPMATERIAL DROP PRIMARY KEY;

CREATE INDEX MATERIAL_INDEX_MCODE ON TBLERPMATERIAL(MCODE);

CREATE INDEX MATERIAL_INDEX_MNAME ON TBLERPMATERIAL(MNAME);



ALTER TABLE TBLERPMOBOM DROP PRIMARY KEY;


CREATE INDEX MOBOM_INDEX_MOCODE ON TBLERPMOBOM(MOCODE);

CREATE INDEX MOBOM_INDEX_MOVER ON TBLERPMOBOM(MOVER);

CREATE INDEX MOBOM_INDEX_ITEMCODE ON TBLERPMOBOM(ITEMCODE);

CREATE INDEX MOBOM_INDEX_MOBITEMCODE ON TBLERPMOBOM(MOBITEMCODE);

TRUNCATE TABLE TBLERPMOBOM;



ALTER TABLE TBLERPMOBOM ADD SERIAL NUMBER(10) PRIMARY KEY;

 CREATE SEQUENCE SEQUENCE_MOBOM_SERIAL  
 INCREMENT BY 1     
     START WITH 1      
     NOMAXVALUE      
     NOCYCLE         
    CACHE 10; 
 
 CREATE TRIGGER TRIGGER_MOBOM_SERIAL
   BEFORE INSERT ON TBLERPMOBOM
   FOR EACH ROW
 BEGIN
   SELECT SEQUENCE_MOBOM_SERIAL.NEXTVAL INTO :NEW.SERIAL FROM DUAL;
 END;


--出库单信息
create table TBLERPDN
(
  DNNO           VARCHAR2(40) not null,
  DNLINE         VARCHAR2(40) not null,
  SALESORDER     VARCHAR2(40),
  SALESORDERLINE NUMBER(22),
  BUSINESSCODE   VARCHAR2(40),
  SHIPTOCOMPANY  VARCHAR2(40) not null,
  SHIPTOPARTY    VARCHAR2(40) not null,
  DNFROM         VARCHAR2(40),
  DEPT           VARCHAR2(100),
  MEMO           VARCHAR2(200),
  ITEMCODE       VARCHAR2(40) not null,
  FRMSTORAGE     VARCHAR2(40),
  PLANQUANTITY   NUMBER(22) not null,
  REALQUANTITY   NUMBER(22),
  UNIT           VARCHAR2(3) not null,
  FLAG           VARCHAR2(1) not null,
  SERIAL         NUMBER(22) not null
);

ALTER TABLE TBLERPDN DROP PRIMARY KEY;

CREATE INDEX DN_INDEX_DNNO_DNLINE ON TBLERPDN(DNNO,DNLine);

CREATE INDEX DN_INDEX_SALESORDER_LINE ON TBLERPDN(SalesOrder,SalesOrderLine);

TRUNCATE TABLE TBLERPDN;


ALTER TABLE TBLERPDN ADD SERIAL NUMBER(10) PRIMARY KEY;


 CREATE SEQUENCE SEQUENCE_DN_SERIAL  
 INCREMENT BY 1     
     START WITH 1      
     NOMAXVALUE      
     NOCYCLE         
    CACHE 10; 
 
 CREATE TRIGGER TRIGGER_DN_SERIAL
   BEFORE INSERT ON TBLERPDN
   FOR EACH ROW
 BEGIN
   SELECT SEQUENCE_DN_SERIAL.NEXTVAL INTO :NEW.SERIAL FROM DUAL;
 END;


alter table tbldn drop column status;

alter table tbldn add CUSORDERLINE number(10);

alter table tbldn add ShipToCompany varchar2(40);

--以下是关于库存移转单表
-- Create table TBLERPINVTRANSFER
create table TBLERPINVTRANSFER
(
  SERIAL       NUMBER(22) not null,
  TRANSFERNO   VARCHAR2(40) not null,
  TRANSFERLINE NUMBER(22) not null,
  FRMSTORAGEID VARCHAR2(40) not null,
  TOSTORAGEID  VARCHAR2(40),
  RECTYPE      VARCHAR2(40) not null,
  ORDERNO      VARCHAR2(40),
  ORDERLINE    NUMBER(22),
  ITEMCODE     VARCHAR2(40) not null,
  PLANQTY      NUMBER(22) not null,
  CUSTOMERCODE VARCHAR2(40),
  MOCODE       VARCHAR2(40),
  MEMO         VARCHAR2(2000),
  FLAG         VARCHAR2(1) not null
);
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLERPINVTRANSFER
  add constraint PK_TBLERPINVTRANSFER primary key (SERIAL);
-- Create/Recreate indexes 
create index INDEX_TBLERPINVTRANSFER_OR on TBLERPINVTRANSFER (ORDERNO, ORDERLINE);
create index INDEX_TBLERPINVTRANSFER_TR on TBLERPINVTRANSFER (TRANSFERNO, TRANSFERLINE);
-- Create sequence 
create sequence SEQ_TBLERPINVTRANSFER_SERIAL
minvalue 1
maxvalue 999999999999999999999999999
start with 1
increment by 1
cache 20;
-- Create TRIGGER
CREATE OR REPLACE TRIGGER tr_tblerpinvtransfer_insert
  BEFORE INSERT ON tblerpinvtransfer
  FOR EACH ROW
  when (new.serial is null)
BEGIN
  SELECT seq_tblerpinvtransfer_serial.nextval INTO :NEW.serial FROM DUAL;
END;
/
--库存移转单表结束

--以下是产品别表以及产品别和产品关系表
-- Create table TBLERPMODEL
create table TBLERPMODEL
(
  MODELCODE VARCHAR2(40) not null,
  MODELDESC VARCHAR2(100),
  FLAG      VARCHAR2(1) not null
);
-- Create/Recreate indexes 
create index INDEX_TBLERPMODEL_MODELCODE on TBLERPMODEL (MODELCODE);
/
-- Create table TBLERPMODEL2ITEM
create table TBLERPMODEL2ITEM
(
  MODELCODE VARCHAR2(40) not null,
  ITEMCODE  VARCHAR2(40) not null,
  FLAG      VARCHAR2(1) not null
);
-- Create/Recreate indexes 
create index INDEX_ERPMODEL2ITEM_ITEMCODE on TBLERPMODEL2ITEM (ITEMCODE);
/
--产品别相关结束

--工单备料单修改主键
alter table TBLMOSTOCK drop constraint TBLMOSTOCK_PK;
alter table TBLMOSTOCK add constraint TBLMOSTOCK_PK primary key(MOCODE, MOVER, ITEMCODE, MITEMCODE, OPBOMVER);
/
--修改工单备料单主键结束

--出库单信息表中未设置不允许为NULL值，我直接修改了本脚本的创建该表的语句

--材料入库单
create table TBLERPINVRECEIPT
(
  SERIAL       NUMBER(22) not null,
  RECEIPTNO    VARCHAR2(40) not null,
  RECEIPTLINE  NUMBER(22) not null,
  PURCHASENO   VARCHAR2(40),
  PURCHASELINE NUMBER(22),
  STORAGEID    VARCHAR2(40) not null,
  VENDORCODE   VARCHAR2(40) not null,
  RECTYPE      VARCHAR2(40) not null,
  ITEMCODE     VARCHAR2(40) not null,
  PLANQTY      NUMBER(22) not null,
  VENDERLOTNO  VARCHAR2(40),
  MOCODE       VARCHAR2(40),
  MEMO         VARCHAR2(2000),
  FLAG         VARCHAR2(1) not null
);
alter table TBLERPINVRECEIPT
  add constraint PK_TBLERPINVRECEIPT primary key (SERIAL);
create index INDEX_TBLERPINVRECEIPT_PU on TBLERPINVRECEIPT (PURCHASENO, PURCHASELINE);
create index INDEX_TBLERPINVRECEIPT_RE on TBLERPINVRECEIPT (RECEIPTNO, RECEIPTLINE);

--工单BOM信息
drop index MOBOM_INDEX_ITEMCODE;
drop index MOBOM_INDEX_MOBITEMCODE;
drop index MOBOM_INDEX_MOCODE;
drop index MOBOM_INDEX_MOVER;
create index INDEX_TBLERPMOBOM on TBLERPMOBOM (MOCODE, MOVER,ITEMCODE, MOBITEMCODE);
alter table TBLERPMOBOM
  add constraint PK_TBLERPMOBOM primary key (SERIAL);
alter table TBLERPMOBOM modify serial number(22);

--工单
alter table TBLERPMO
  add constraint PK_TBLERPMO primary key (SERIAL);

--标准BOM
drop index ERPSBOM_INDEX_ITEMCODE;
drop index ERPSBOM_INDEX_SBITEMCODE;
drop index ERPSBOM_INDEX_SBOMVER;
create index INDEX_TBLERPSBOM on TBLERPSBOM (ITEMCODE, SBITEMCODE, SBOMVER);
alter table TBLERPSBOM
  add constraint PK_TBLERPSBOM primary key (SERIAL);
alter table TBLERPSBOM modify serial number(22);

--MES系统里的标准BOM
alter table TBLSBOM modify location varchar2(500);

--修改MES工单备料的主键
alter table TBLMOSTOCK drop constraint TBLMOSTOCK;
alter table TBLMOSTOCK
  add constraint TBLMOSTOCK_PK primary key (MOCODE, MOVER, ITEMCODE, MITEMCODE, OPBOMVER);

--修改湿敏时调整的列
alter table TBLMSDLEVEL add INDRYINGTIME NUMBER(10) not null;
alter table TBLMaterialMSL drop column INDRYINGTIME;
