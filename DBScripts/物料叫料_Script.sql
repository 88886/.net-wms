--ģ�鼰�˵�
insert into tblmdl (MDLCODE, PMDLCODE, MDLVER, MDLTYPE, MDLSTATUS, MDLDESC, MDLSEQ, MDLHFNAME, MUSER, MDATE, MTIME, ISSYS, ISACTIVE, FORMURL, EATTRIBUTE1, ISRESTRAIN)
values ('DISTOLINE', 'CS_DATACOLLECT', '1.0', 'C/S', 'Alpha', '��������', 146, null, 'ADMIN', 20140827, 160333, '1', '1', 'BenQGuru.eMES.Client.FDisToLine', null, '0');
/
insert into tblmenu (MENUCODE, MDLCODE, PMENUCODE, MENUDESC, MENUSEQ, MENUTYPE, MUSER, MDATE, MTIME, EATTRIBUTE1, VISIBILITY)
values ('DISTOLINE', 'DISTOLINE', 'CS_DATACOLLECT', '��������ά��', 146, 'C/S', 'ADMIN', 20140827, 160438, null, '0');
/

--��ṹ
-- Create table
create table TBLDISTOLINEHEAD
(
  mocode      VARCHAR2(40) not null,
  mcode       VARCHAR2(40) not null,
  mname       VARCHAR2(200),
  mplanqty    NUMBER(10,2) not null,
  mdisqty     NUMBER(10,2) default (0) not null,
  itemcode    VARCHAR2(40) not null,
  moplanqty   NUMBER(10,2) not null,
  status      VARCHAR2(40) not null,
  pendingtime NUMBER(10,2) default (0),
  orgid       NUMBER(10) not null,
  mobom       VARCHAR2(40) not null,
  muser       VARCHAR2(40) not null,
  mdate       NUMBER(8) not null,
  mtime       NUMBER(6) not null,
  eattribute1 VARCHAR2(200)
)
/
-- Add comments to the table 
comment on table TBLDISTOLINEHEAD
  is '��������������';
-- Add comments to the columns 
comment on column TBLDISTOLINEHEAD.mocode
  is '��������';
comment on column TBLDISTOLINEHEAD.mcode
  is '���ϴ���';
comment on column TBLDISTOLINEHEAD.mname
  is '��������';
comment on column TBLDISTOLINEHEAD.mplanqty
  is '�ƻ���������';
comment on column TBLDISTOLINEHEAD.mdisqty
  is '�ѷ���������';
comment on column TBLDISTOLINEHEAD.itemcode
  is '��Ʒ�Ϻ�';
comment on column TBLDISTOLINEHEAD.moplanqty
  is '�ƻ�������';
comment on column TBLDISTOLINEHEAD.status
  is '��������״̬��Initial-��ʼ����Distributing-�����У�Pending-��ͣ���ͣ�Finish-�������';
comment on column TBLDISTOLINEHEAD.pendingtime
  is '��ͣʱ�����֣�';
comment on column TBLDISTOLINEHEAD.orgid
  is '��֯ID';
comment on column TBLDISTOLINEHEAD.mobom
  is '����BOM�汾';
comment on column TBLDISTOLINEHEAD.muser
  is 'ά����';
comment on column TBLDISTOLINEHEAD.mdate
  is 'ά������';
comment on column TBLDISTOLINEHEAD.mtime
  is 'ά��ʱ��';
comment on column TBLDISTOLINEHEAD.eattribute1
  is '�����ֶ�';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLDISTOLINEHEAD
  add constraint TBLDISTOLINEHEAD_PK primary key (MOCODE, MCODE)
 /
 
 -- Create table
create table TBLDISTOLINEDETAIL
(
  mocode      VARCHAR2(40) not null,
  mcode       VARCHAR2(40) not null,
  sscode      VARCHAR2(40) not null,
  opcode      VARCHAR2(40) not null,
  segcode     VARCHAR2(40),
  mssdisqty   NUMBER(10,2) not null,
  mssleftqty  NUMBER(10,2) default (0) not null,
  msslefttime NUMBER(10,2) default (0) not null,
  mqty        NUMBER(10,2) default (0) not null,
  status      VARCHAR2(40) not null,
  muser       VARCHAR2(40) not null,
  mdate       NUMBER(8) not null,
  mtime       NUMBER(6) not null,
  eattribute  VARCHAR2(200)
)
/
-- Add comments to the table 
comment on table TBLDISTOLINEDETAIL
  is '������������ϸ��';
-- Add comments to the columns 
comment on column TBLDISTOLINEDETAIL.mocode
  is '��������';
comment on column TBLDISTOLINEDETAIL.mcode
  is '���ϴ���';
comment on column TBLDISTOLINEDETAIL.sscode
  is '���ߴ���';
comment on column TBLDISTOLINEDETAIL.opcode
  is '�������';
comment on column TBLDISTOLINEDETAIL.segcode
  is '�������';
comment on column TBLDISTOLINEDETAIL.mssdisqty
  is '�ѷ�����';
comment on column TBLDISTOLINEDETAIL.mssleftqty
  is '����ʣ������';
comment on column TBLDISTOLINEDETAIL.msslefttime
  is 'ʣ��ʹ��ʱ��';
comment on column TBLDISTOLINEDETAIL.mqty
  is '���η�������';
comment on column TBLDISTOLINEDETAIL.status
  is '����״̬��Normal-������WaitDis-�����ͣ�ERDis-�������ͣ�ShortDis-ȱ���У�Finish-�������';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLDISTOLINEDETAIL
  add constraint TBLDISTOLINEDETAIL_PK primary key (MOCODE, MCODE, SSCODE, OPCODE)
 /
 
 -- Create table
create table TBLDISTOLINELIST
(
  serial      NUMBER not null,
  mocode      VARCHAR2(40) not null,
  mcode       VARCHAR2(40) not null,
  sscode      VARCHAR2(40) not null,
  opcode      VARCHAR2(40) default ('1') not null,
  segcode     VARCHAR2(40),
  mssdisqty   NUMBER(10,2) not null,
  mssleftqty  NUMBER(10,2) default (0) not null,
  msslefttime NUMBER(10,2) default (0) not null,
  mqty        NUMBER(10,2) default (0) not null,
  status      VARCHAR2(40) not null,
  delflag     VARCHAR2(8) default ('N') not null,
  muser       VARCHAR2(40) not null,
  mdate       NUMBER(8) not null,
  mtime       NUMBER(6) not null,
  eattribute  VARCHAR2(200)
)
/
-- Add comments to the table 
comment on table TBLDISTOLINELIST
  is '����������List��';
-- Add comments to the columns 
comment on column TBLDISTOLINELIST.serial
  is '��������';
comment on column TBLDISTOLINELIST.mocode
  is '��������';
comment on column TBLDISTOLINELIST.mcode
  is '���ϴ���';
comment on column TBLDISTOLINELIST.sscode
  is '���ߴ���';
comment on column TBLDISTOLINELIST.opcode
  is '�������';
comment on column TBLDISTOLINELIST.segcode
  is '�������';
comment on column TBLDISTOLINELIST.mssdisqty
  is '�ѷ�����';
comment on column TBLDISTOLINELIST.mssleftqty
  is '����ʣ������';
comment on column TBLDISTOLINELIST.msslefttime
  is 'ʣ��ʹ��ʱ��';
comment on column TBLDISTOLINELIST.mqty
  is '���η�������';
comment on column TBLDISTOLINELIST.status
  is '����״̬';
comment on column TBLDISTOLINELIST.delflag
  is 'ɾ����־ ɾ��ʱΪY';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TBLDISTOLINELIST
  add constraint TBLDISTOLINELIST_PK primary key (SERIAL)
 /
 
 -- Create sequence 
create sequence SEQ_TBLDISTOLINELIST_SERIAL
minvalue 1
maxvalue 999999999999999999999
start with 121
increment by 1
cache 20;
/

CREATE OR REPLACE TRIGGER tri_tblDisToLineList_serial
 BEFORE
  INSERT
 ON tblDisToLineList
REFERENCING NEW AS NEW OLD AS OLD
 FOR EACH ROW
BEGIN
   SELECT SEQ_tblDisToLineList_SERIAL.NEXTVAL INTO :NEW.serial
   FROM dual;
END;
/

--����ά��
insert into tblsysparamgroup (PARAMGROUPCODE, PARAMGROUPTYPE, PARAMGROUPDESC, MUSER, MDATE, MTIME, ISSYS, EATTRIBUTE1)
values ('ALERTMATERIALDISGROUP', 'AlertMaterialDisGroup', '��������Ԥ��', 'ADMIN', 20140818, 90421, '0', null);
/

insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('ALERTDISNORMAL', 'ALERTMATERIALDISGROUP', '30', '������Ԥ��ʱ��', null, 'ADMIN', 20140818, 91006, '0', '0', '3', null);
/
insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('ALERTDISER', 'ALERTMATERIALDISGROUP', '15', '��������Ԥ��ʱ��', null, 'ADMIN', 20140818, 91013, '0', '0', '4', null);
/
insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('ALERTDISFLAG', 'ALERTMATERIALDISGROUP', 'Y', '�Ƿ�����Ԥ����Y/N��', null, 'ADMIN', 20140818, 90714, '0', '0', '1', null);
/
insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('ALERTDISOP', 'ALERTMATERIALDISGROUP', 'JARVIS_PM', '�ǹܿ��Ͽ��Ϲ���', null, 'ADMIN', 20140827, 141116, '0', '0', '2', null);
/

----Package
CREATE OR REPLACE PACKAGE pkg_MOTODISTOLINEHEAD IS
  PROCEDURE MOToDisToLineHead;

END pkg_MOTODISTOLINEHEAD;

--Package Body
CREATE OR REPLACE PACKAGE BODY pkg_MOToDisToLineHead IS

  V_SQL      VARCHAR2(5000) := '';
  v_Cur_Date NUMBER;
  v_Cur_Time NUMBER;

  --��¼joblog
  procedure RecordJobResult(p_JobID         IN VARCHAR2,
                            p_StartDateTime IN DATE,
                            p_EndDateTime   IN DATE,
                            p_ProcessCount  IN NUMBER,
                            p_Result        IN VARCHAR2,
                            p_ErrorMsg      IN VARCHAR2) is
    v_UsedSeconds NUMBER;
  begin
    v_UsedSeconds := (p_EndDateTime - p_StartDateTime) * 24 * 60 * 60;
  
    INSERT INTO tbljoblog
      (jobid,
       startdatetime,
       enddatetime,
       usedtime,
       processcount,
       result,
       errormsg)
    VALUES
      (p_JobID,
       p_StartDateTime,
       p_EndDateTime,
       v_UsedSeconds,
       p_ProcessCount,
       p_Result,
       p_ErrorMsg);
    commit;
    return;
  exception
    when others then
      return;
    
  end RecordJobResult;

  --�����·���ɺ�����ͨ��job��ʱ�ܵ���������ϱ���
  PROCEDURE MOToDisToLineHead IS
  
    V_MOCODE                 VARCHAR2(40);
    I_PROCESSCOUNT           NUMBER := 0;
    V_STARTDATETIME          DATE;
    SELECTMOForInsert_CURSOR Sys_Refcursor;
    MCODEForInsert_CURSOR    Sys_Refcursor;
    SELECTMOForUpdate_CURSOR Sys_Refcursor;
    MCODEForUpdate_CURSOR    Sys_Refcursor;
    v_JobID                  VARCHAR2(40) := 'JOB_MOToDisToLineHead';
    v_ErrorID                VARCHAR2(100);
    V_MCODE                  VARCHAR2(40);
    V_MNAME                  VARCHAR2(400);
    V_MPLANQTY               NUMBER(22);
    V_MDISQTY                NUMBER := 0;
    V_ITEMCODE               VARCHAR2(40);
    V_MOPLANQTY              NUMBER(22);
    V_PENDINGTIME            NUMBER := 0;
    V_ORGID                  NUMBER(22);
    V_MOBOM                  VARCHAR2(40);
  
  BEGIN
  
    SELECT sysdate INTO V_STARTDATETIME FROM dual;
  
    v_Cur_Date := TO_NUMBER(TO_CHAR(sysdate, 'yyyyMMdd'));
    v_Cur_Time := TO_NUMBER(TO_CHAR(sysdate, 'hhmmss'));
  
    --begin�����߼�
    --��tblmoץȡ����״̬Ϊmostatus_release��mostatus_open���Ҵ���tblDisToLineHead��״̬ΪInitial�Ĺ�����Ϣ
    --���ڸ���tblDisToLineHead
    V_SQL := ' ';
    V_SQL := V_SQL || ' select mocode from tblmo ';
    V_SQL := V_SQL ||
             ' where mostatus in(''mostatus_release'',''mostatus_open'') ';
    V_SQL := V_SQL ||
             ' and mocode in (select mocode from tblDisToLineHead where status=''Initial'') ';
    OPEN SELECTMOForUpdate_CURSOR FOR V_SQL;
    LOOP
      FETCH SELECTMOForUpdate_CURSOR
        INTO V_MOCODE;
      EXIT WHEN SELECTMOForUpdate_CURSOR%NOTFOUND;
    
      --���ڵ��ʹ�������ץȡtblmobom�иù����µ����ϴ���
      V_SQL := ' select mobitemcode from tblmo inner join tblmobom on tblmo.mocode=tblmobom.mocode where tblmo.mocode=''' ||
               V_MOCODE || '''';
      OPEN MCODEForUpdate_CURSOR FOR V_SQL;
      LOOP
        FETCH MCODEForUpdate_CURSOR
          INTO V_MCODE;
        EXIT WHEN MCODEForUpdate_CURSOR%NOTFOUND;
      
        I_PROCESSCOUNT := I_ProcessCount + 1;
      
        --����ά��������bom���й������ϣ�ץ�����������������ڸ���
        --����MOBOM����������*MO�мƻ�������=����������
        select t1.moplanqty * t2.mobitemqty as MPLANQTY
          into V_MPLANQTY
          from tblmo t1
         inner join (select tblmo.mocode, mobitemcode, mobitemqty
                       from tblmo
                      inner join tblmobom
                         on tblmo.mocode = tblmobom.mocode
                      inner join tblmaterial tml
                         on mobitemcode = tml.mcode
                      where tblmo.mocode = V_MOCODE
                        and mobitemcode = V_MCODE) t2
            on t1.mocode = t2.mocode;
      
        update tblDisToLineHead
           set mplanqty = V_MPLANQTY
         where mocode = V_MOCODE
           and mcode = V_MCODE;
      
      end loop;
      close MCODEForUpdate_CURSOR;
    end loop;
    close SELECTMOForUpdate_CURSOR;
    commit;
    --end�����߼�
  
    --begin�����߼�
    --tblmo��ץȡ����״̬Ϊmostatus_release��mostatus_open���Ҳ���tblDisToLineHead���еĹ�����
    --����������tblDisToLineHead
    V_SQL := ' ';
    V_SQL := V_SQL || ' select mocode from tblmo ';
    V_SQL := V_SQL ||
             ' where mostatus in(''mostatus_release'',''mostatus_open'')';
    V_SQL := V_SQL ||
             ' and mocode not in (select mocode from tblDisToLineHead) ';
    OPEN SELECTMOForInsert_CURSOR FOR V_SQL;
    LOOP
      FETCH SELECTMOForInsert_CURSOR
        INTO V_MOCODE;
      EXIT WHEN SELECTMOForInsert_CURSOR%NOTFOUND;
    
      --���ڵ��ʹ�������ץȡtblmobom�иù����µ����ϴ���
      V_SQL := ' select mobitemcode from tblmo inner join tblmobom on tblmo.mocode=tblmobom.mocode where tblmo.mocode=''' ||
               V_MOCODE || '''';
      OPEN MCODEForInsert_CURSOR FOR V_SQL;
      LOOP
        FETCH MCODEForInsert_CURSOR
          INTO V_MCODE;
        EXIT WHEN MCODEForInsert_CURSOR%NOTFOUND;
      
        I_PROCESSCOUNT := I_ProcessCount + 1;
      
        --����ά��������bom���й������ϣ�ץ����Ҫ�����ݣ���������
        select t1.moplanqty,
               t1.itemcode,
               t1.orgid,
               t1.mobom,
               t2.mobitemcode as mcode,
               t2.mname,
               t1.moplanqty * t2.mobitemqty as MPLANQTY
          into V_MOPLANQTY,
               V_ITEMCODE,
               V_ORGID,
               V_MOBOM,
               V_MCODE,
               V_MNAME,
               V_MPLANQTY
          from tblmo t1
         inner join (select tblmo.mocode, mobitemcode, mobitemqty, mname
                       from tblmo
                      inner join tblmobom
                         on tblmo.mocode = tblmobom.mocode
                      inner join tblmaterial tml
                         on mobitemcode = tml.mcode
                      where tblmo.mocode = V_MOCODE
                        and mobitemcode = V_MCODE) t2
            on t1.mocode = t2.mocode;
        --����
        --����MOBOM����������*MO�мƻ�������=����������������ϢInsert��tblDisToLineHead��״̬Ϊ��ʼ��
        insert into tblDisToLineHead
          (Mocode,
           Mcode,
           Mname,
           Mplanqty,
           Mdisqty,
           Itemcode,
           Moplanqty,
           Status,
           Pendingtime,
           Orgid,
           Mobom,
           Muser,
           Mdate,
           Mtime)
        values
          (V_MOCODE,
           V_MCODE,
           V_MNAME,
           V_MPLANQTY,
           V_MDISQTY,
           V_ITEMCODE,
           V_MOPLANQTY,
           'Initial',
           V_PENDINGTIME,
           V_ORGID,
           V_MOBOM,
           'JOB',
           v_Cur_Date,
           v_Cur_Time);
      end loop;
      close MCODEForInsert_CURSOR;
    end loop;
    close SELECTMOForInsert_CURSOR;
    commit;
    --end�����߼�
  
    RecordJobResult(v_JobID,
                    v_StartDateTime,
                    sysdate,
                    I_ProcessCount,
                    'OK',
                    '');
  exception
    when others then
      v_ErrorID := substrb(sqlerrm, 1, 100);
      rollback;
      RecordJobResult(v_JobID,
                      v_StartDateTime,
                      sysdate,
                      I_ProcessCount,
                      'FAIL',
                      v_ErrorID);
      commit;
    
  END MOToDisToLineHead;
END pkg_MOToDisToLineHead;

--scheduler
BEGIN
dbms_scheduler.create_job(job_name => 'JOB_MOTODISTOLINEHEAD',
       job_type => 'PLSQL_BLOCK',
       job_action => 'DECLARE BEGIN PKG_MOTODISTOLINEHEAD.MOTODISTOLINEHEAD; END;', 
       start_date => sysdate,
       repeat_interval => 'FREQ=MINUTELY; INTERVAL=60', 
       comments => '�������ݽ���������ϱ�'); 
END;