insert into tblsysparamgroup (PARAMGROUPCODE, PARAMGROUPTYPE, PARAMGROUPDESC, MUSER, MDATE, MTIME, ISSYS, EATTRIBUTE1)
values ('SMT_MATERIAL', 'SMT_Material', 'SMT_Material', 'ADMIN', 20101116, 164606, '0', '');
 
 
//抛料率
insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('SMT_AUTO_MATERIAL', 'SMT_MATERIAL', '0.5', '抛料率', '', 'ADMIN', 20101116, 164735, '0', '0', '1', '');
 
//需自动扣料工序
insert into tblsysparam (PARAMCODE, PARAMGROUPCODE, PARAMALIAS, PARAMDESC, PARAMVALUE, MUSER, MDATE, MTIME, ISACTIVE, ISSYS, EATTRIBUTE1, PARENTPARAM)
values ('MANUALTEST', 'SMT_MATERIAL', 'MANUALTEST', '测试工序', '', 'ADMIN', 20101117, 141509, '0', '0', '2', '');


//
alter table  TBLSAPMATERIALTRANS modify FRMStorageID  VARCHAR2(40) ;

alter table  TBLSAPMATERIALTRANS modify TOStorageID  VARCHAR2(40) ;


//
alter table tblmaterial modify(mname VARCHAR2(200)); 

