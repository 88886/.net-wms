-----------------------------added by leon.li 20130308----mail������ֻ�����״̬-----------------------------------
alter table tblmail
add ISSENDTOPHONE varchar2(1) default 'N' not null;

----------------------------------------���ŷ��ͷ��ز���-----------------------------------------------

alter table tblmail
add PHONESENDRESULT varchar2(40) null;
-----------------------------------------end leon.li-------------------------------------------