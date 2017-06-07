using System;
using System.Runtime.Remoting;
using System.Collections.Generic;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.MOModel;
namespace BenQGuru.eMES.BaseSetting
{
    /// <summary>
    /// UserManager ��ժҪ˵����
    /// �ļ���:		UserManager.cs
    /// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
    /// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
    /// ��������:	2005-03-22 11:55:39
    /// �޸���:		Jane Shu
    /// �޸�����:	2005-04-05  
    ///					������д��ȥ��upper
    /// �� ��:	
    /// �� ��:	
    /// </summary>
    public class UserFacade : MarshalByRefObject
    {
        private IDomainDataProvider _domainDataProvider = null;
        private FacadeHelper _helper = null;

        public UserFacade(IDomainDataProvider domainDataProvider)
        {
            this._domainDataProvider = domainDataProvider;
            this._helper = new FacadeHelper(this.DataProvider);
        }

        public UserFacade()
        {

        }
        //Laws Lu,max life time to unlimited
        public override object InitializeLifetimeService()
        {
            return null;
        }

        protected IDomainDataProvider DataProvider
        {
            get
            {
                if (_domainDataProvider == null)
                {
                    _domainDataProvider = DomainDataProviderManager.DomainDataProvider();
                }
                return _domainDataProvider;
            }
        }

        #region User
        /// <summary>
        /// ϵͳ�û�
        /// </summary>
        public User CreateNewUser()
        {
            return new User();
        }

        public void AddUser(User user)
        {
            this._helper.AddDomainObject(user);
        }

        public void UpdateUser(User user)
        {
            this._helper.UpdateDomainObject(user);
        }

        public void DeleteUser(User user)
        {
            this._helper.DeleteDomainObject(user,
                new ICheck[]{ new DeleteAssociateCheck( user,
								this.DataProvider, 
								new Type[]{
											  typeof(UserGroup2User)	})});
        }

        public void DeleteUser(User[] user)
        {
            this._helper.DeleteDomainObject(user,
                new ICheck[]{ new DeleteAssociateCheck( user,
								this.DataProvider, 
								new Type[]{
											  typeof(UserGroup2User)	})});
        }

        public object GetUser(string userCode)
        {
            return this.DataProvider.CustomSearch(typeof(User), new object[] { userCode });
        }

        /// <summary>
        /// ** ��������:	��ѯUser��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userCode">UserCode��ģ����ѯ</param>
        /// <returns> User���ܼ�¼��</returns>
        public int QueryUserCount(string userCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLUSER where 1=1 and USERCODE like '{0}%' ", userCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯUser
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userCode">UserCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> User����</returns>
        public object[] QueryUser(string userCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(User), new PagerCondition(string.Format("select {0} from TBLUSER where 1=1 and USERCODE like '{1}%' order by USERCODE ", DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(User)), userCode), inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�User
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>User���ܼ�¼��</returns>
        public object[] GetAllUser()
        {
            return this.DataProvider.CustomQuery(typeof(User), new SQLCondition(string.Format("select {0} from TBLUSER order by USERCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(User)))));
        }
        #endregion

        #region UserEx

        public UserEx CreateNewUserEx()
        {
            return new UserEx();
        }

        public void AddUserEx(UserEx userEx)
        {
            this.DataProvider.BeginTransaction();

            try
            {   //Add User
                User user = CopyUserExToUser(userEx);
                this._helper.AddDomainObject(user);

                //Add User2Org
                AddUser2Org(userEx.user2OrgList);

                this.DataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(typeof(SystemError), "$Error_Update_System_Error", e);
            }
        }

        public void UpdateUserEx(UserEx userEx)
        {
            this.DataProvider.BeginTransaction();

            try
            {
                //Update User
                User user = CopyUserExToUser(userEx);
                this._helper.UpdateDomainObject(user);

                //Update User2Org
                DeleteUser2Org(userEx.UserCode);
                foreach (User2Org user2Org in userEx.user2OrgList)
                {
                    AddUser2Org(user2Org);
                }

                this.DataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(typeof(SystemError), "$Error_Update_System_Error", e);
            }
        }

        public void DeleteUserEx(UserEx userEx)
        {
            this.DataProvider.BeginTransaction();

            try
            {
                //Delete User2Org
                DeleteUser2Org(userEx.UserCode);

                //Delete User
                User user = CopyUserExToUser(userEx);
                this._helper.DeleteDomainObject(user,
                    new ICheck[]{ new DeleteAssociateCheck(user,
                                    this.DataProvider, 
                                    new Type[]{
                                                  typeof(UserGroup2User)	})});

                this.DataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(typeof(SystemError), "$Error_Update_System_Error", e);
            }
        }

        public void DeleteUserEx(UserEx[] userExList)
        {
            this.DataProvider.BeginTransaction();

            try
            {
                //Delete User2Org
                foreach (UserEx userEx in userExList)
                {
                    DeleteUser2Org(userEx.UserCode);
                }

                //Delete User
                foreach (UserEx userEx in userExList)
                {
                    User user = CopyUserExToUser(userEx);
                    this._helper.DeleteDomainObject(user,
                        new ICheck[]{ new DeleteAssociateCheck(user,
                                    this.DataProvider, 
                                    new Type[]{
                                                  typeof(UserGroup2User)	})});
                }

                this.DataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(typeof(SystemError), "$Error_Update_System_Error", e);
            }
        }

        public object GetUserEx(string userCode)
        {
            try
            {
                object userEx = QueryUserEx(userCode, 1, 1)[0];
                object[] objectList = GetUser2OrgList(userCode);
                ((UserEx)userEx).user2OrgList = new User2Org[objectList.Length];
                for (int i = 0; i < objectList.Length; i++)
                {
                    ((UserEx)userEx).user2OrgList[i] = (User2Org)objectList[i];
                }
                return userEx;
            }
            catch
            {
                return null;
            }
        }

        public int QueryUserExCount(string userCode)
        {
            return QueryUserCount(userCode);
        }

        public object[] QueryUserEx(string userCode, int inclusive, int exclusive)
        {
            string selectSQL = string.Format("SELECT {0}, NVL(o.orgdesc, ' ') orgdesc FROM tbluser LEFT OUTER JOIN (SELECT a.usercode, b.orgdesc FROM tbluser2org a, tblorg b WHERE a.orgid = b.orgid AND a.defaultorg = 1) o ON tbluser.usercode = o.usercode WHERE tbluser.usercode LIKE '{1}%' ORDER BY tbluser.usercode ", DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(User)), userCode);
            return this.DataProvider.CustomQuery(typeof(UserEx), new PagerCondition(selectSQL, inclusive, exclusive));
        }

        public object[] GetAllUserEx()
        {
            string selectSQL = string.Format("SELECT {0}, NVL(o.orgdesc, ' ') orgdesc FROM tbluser LEFT OUTER JOIN (SELECT a.usercode, b.orgdesc FROM tbluser2org a, tblorg b WHERE a.orgid = b.orgid AND a.defaultorg = 1) o ON tbluser.usercode = o.usercode ", DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(User)));
            return this.DataProvider.CustomQuery(typeof(UserEx), new SQLCondition(selectSQL));
        }

        private User CopyUserExToUser(UserEx userEx)
        {
            User returnValue = this.CreateNewUser();
            returnValue.MaintainDate = userEx.MaintainDate;
            returnValue.MaintainTime = userEx.MaintainTime;
            returnValue.MaintainUser = userEx.MaintainUser;
            returnValue.UserCode = userEx.UserCode;
            returnValue.UserDepartment = userEx.UserDepartment;
            returnValue.UserEmail = userEx.UserEmail;
            returnValue.UserName = userEx.UserName;
            returnValue.UserPassword = userEx.UserPassword;
            returnValue.UserStatus = userEx.UserStatus;
            returnValue.UserTelephone = userEx.UserTelephone;

            return returnValue;
        }

        #endregion

        #region User2Org

        public User2Org CreateNewUser2Org()
        {
            return new User2Org();
        }

        public void AddUser2Org(User2Org user2Org)
        {
            this._helper.AddDomainObject(user2Org);
        }

        public void AddUser2Org(User2Org[] user2Orgs)
        {
            foreach (User2Org user2Org in user2Orgs)
            {
                this._helper.AddDomainObject(user2Org);
            }
        }

        public void UpdateUser2Org(User2Org user2Org)
        {
            this._helper.UpdateDomainObject(user2Org);
        }

        public void DeleteUser2Org(string userCode)
        {
            string deleteSQL = "DELETE FROM tbluser2org WHERE usercode = '" + userCode.Trim().ToUpper() + "'";
            this.DataProvider.CustomExecute(new SQLCondition(deleteSQL));
        }

        public void DeleteUser2Org(User2Org user2Org)
        {
            this._helper.DeleteDomainObject(user2Org);
        }

        public void DeleteUser2Org(User2Org[] user2Orgs)
        {
            foreach (User2Org user2Org in user2Orgs)
            {
                this._helper.DeleteDomainObject(user2Org);
            }
        }

        public object GetUser2Org(string userCode, int orgID)
        {
            return this.DataProvider.CustomSearch(typeof(User2Org), new object[] { userCode, orgID });
        }

        public object[] GetUser2OrgList(string userCode)
        {
            return this.DataProvider.CustomQuery(typeof(User2Org), new SQLCondition(string.Format("SELECT {0} FROM tbluser2org WHERE usercode = '{1}' ORDER BY orgid ", DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(User2Org)), userCode.Trim().ToUpper())));
        }

        #endregion

        #region UserGroup
        /// <summary>
        /// 
        /// </summary>
        public UserGroup CreateNewUserGroup()
        {
            return new UserGroup();
        }

        public USERGROUP2ITEM CreateNewUSERGROUP2ITEM()
        {
            return new USERGROUP2ITEM();
        }

        public void AddUserGroup(UserGroup userGroup)
        {
            this._helper.AddDomainObject(userGroup);
        }

        public void AddUSERGROUP2ITEM(USERGROUP2ITEM userGroup)
        {
            this._helper.AddDomainObject(userGroup);
        }

        public void UpdateUserGroup(UserGroup userGroup)
        {
            this._helper.UpdateDomainObject(userGroup);
        }

        public void UpdateUSERGROUP2ITEM(USERGROUP2ITEM userGroup)
        {
            this._helper.UpdateDomainObject(userGroup);
        }

        public void DeleteUserGroup(UserGroup userGroup)
        {
            this._helper.DeleteDomainObject(userGroup,
                new ICheck[]{ new DeleteAssociateCheck( userGroup,
								this.DataProvider, 
								new Type[]{
											  typeof(UserGroup2Module),
											  typeof(UserGroup2User)	})});
        }

        public void DeleteUserGroup(UserGroup[] userGroup)
        {
            this._helper.DeleteDomainObject(userGroup,
                new ICheck[]{ new DeleteAssociateCheck( userGroup,
								this.DataProvider, 
								new Type[]{
											  typeof(UserGroup2Module),
											  typeof(UserGroup2User)	})});
        }

        public object GetUserGroup(string userGroupCode)
        {
            return this.DataProvider.CustomSearch(typeof(UserGroup), new object[] { userGroupCode });
        }


        public void DeleteUSERGROUP2ITEM(USERGROUP2ITEM userGroup)
        {
            this._helper.DeleteDomainObject(userGroup,
                new ICheck[] { });
        }

        public void DeleteUSERGROUP2ITEM(USERGROUP2ITEM[] userGroup)
        {
            this._helper.DeleteDomainObject(userGroup,
                new ICheck[] { });
        }

        public object GetUSERGROUP2ITEM(string userGroupCode, string pitemtype)
        {
            return this.DataProvider.CustomSearch(typeof(USERGROUP2ITEM), new object[] { userGroupCode, pitemtype });
        }

        /// <summary>
        /// ** ��������:	��ѯUserGroup��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode��ģ����ѯ</param>
        /// <returns> UserGroup���ܼ�¼��</returns>
        public int QueryUserGroupCount(string userGroupCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLUSERGROUP where 1=1 and USERGROUPCODE like '{0}%' ", userGroupCode)));
        }

        public int CQueryUserGroupCount(string userGroupCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLUSERGROUP where 1=1 and USERGROUPCODE like '{0}%' and USERGROUPTYPE='�ͻ��û���' ", userGroupCode)));
        }

        public int CSQueryUserGroupCount(string userGroupCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from USERGROUP2ITEM where ISAVAILABLE=0  and USERGROUPCODE ='{0}'", userGroupCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯUserGroup
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> UserGroup����</returns>
        public object[] QueryUserGroup(string userGroupCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(UserGroup), new PagerCondition(string.Format("select {0} from TBLUSERGROUP where 1=1 and USERGROUPCODE like '{1}%' order by USERGROUPCODE ", DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(UserGroup)), userGroupCode), inclusive, exclusive));
        }

        public object[] CQueryUserGroup(string userGroupCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(UserGroup), new PagerCondition(string.Format("select {0} from TBLUSERGROUP where 1=1 and USERGROUPCODE like '{1}%' and USERGROUPTYPE='�ͻ��û���' order by USERGROUPCODE ", DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(UserGroup)), userGroupCode), inclusive, exclusive));
        }

        public object[] CSQueryUserGroup(string userGroupCode, int inclusive, int exclusive)
        {
            string strSql = string.Format("select * from USERGROUP2ITEM where ISAVAILABLE=0  and USERGROUPCODE = '{1}' order by USERGROUPCODE ", DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(USERGROUP2ITEM)), userGroupCode);
            return this.DataProvider.CustomQuery(typeof(USERGROUP2ITEM), new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	������е�UserGroup
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>UserGroup���ܼ�¼��</returns>
        public object[] GetAllUserGroup()
        {
            return this.DataProvider.CustomQuery(typeof(UserGroup), new SQLCondition(string.Format("select {0} from TBLUSERGROUP order by USERGROUPCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup)))));
        }

        /// <summary>
        /// ����:��õ�ǰUser������UserGroup
        /// ����:melo zheng
        /// ����:2006.12.26
        /// </summary>
        /// <returns>��ǰUser������UserGroup��</returns>
        public object[] GetAllUserGroup(string userCode)
        {
            return this.DataProvider.CustomQuery(typeof(UserGroup), new SQLCondition(string.Format("select {0} from TBLUSERGROUP where USERGROUPCODE in ( select USERGROUPCODE from TBLUSERGROUP2USER where USERCODE ='" + userCode + "') order by USERGROUPCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup)))));
        }

        #endregion

        #region UserGroup2User
        /// <summary>
        /// 
        /// </summary>
        public UserGroup2User CreateUserGroup2User()
        {
            return new UserGroup2User();
        }

        public void AddUserGroup2User(UserGroup2User userGroup2User)
        {
            this._helper.AddDomainObject(userGroup2User);
        }

        public void UpdateUserGroup2User(UserGroup2User userGroup2User)
        {
            this._helper.UpdateDomainObject(userGroup2User);
        }

        public void DeleteUserGroup2User(UserGroup2User userGroup2User)
        {
            this._helper.DeleteDomainObject(userGroup2User);
        }

        public void DeleteUserGroup2User(UserGroup2User[] userGroup2User)
        {
            this._helper.DeleteDomainObject(userGroup2User);
        }

        public object GetUserGroup2User(string userCode, string userGroupCode)
        {
            return this.DataProvider.CustomSearch(typeof(UserGroup2User), new object[] { userCode, userGroupCode });
        }

        public void AddUserGroup2User(UserGroup2User[] userGroup2Users)
        {
            this._helper.AddDomainObject(userGroup2Users);
        }

        public object[] QueryUserGroup2User(string userGroupCode, string userCode, int inclusive, int exclusive)
        {
            string sql = "SELECT {0} FROM tblusergroup2user WHERE 1 = 1 ";
            if (userGroupCode.Trim().Length > 0)
            {
                sql += string.Format("AND usergroupcode = '{0}'", userGroupCode.Trim().ToUpper());
            }
            if (userCode.Trim().Length > 0)
            {
                sql += string.Format("AND usercode = '{0}'", userCode.Trim().ToUpper());
            }

            sql = string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup2User)));

            return this.DataProvider.CustomQuery(typeof(UserGroup2User), new PagerCondition(sql, inclusive, exclusive));
        }

        public int QueryUserGroup2UserCount(string userGroupCode, string userCode)
        {
            string sql = "SELECT COUNT(*) FROM tblusergroup2user WHERE 1 = 1 ";
            if (userGroupCode.Trim().Length > 0)
            {
                sql += string.Format("AND usergroupcode = '{0}'", userGroupCode.Trim().ToUpper());
            }
            if (userCode.Trim().Length > 0)
            {
                sql += string.Format("AND usercode = '{0}'", userCode.Trim().ToUpper());
            }

            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        #region UserGroup --> User
        /// <summary>
        /// ** ��������:	��UserGroupCode���User
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <returns>User����</returns>
        public object[] GetUserByUserGroupCode(string userGroupCode)
        {
            return this.DataProvider.CustomQuery(typeof(User), new SQLCondition(string.Format("select {0} from TBLUSER where USERCODE in ( select USERCODE from TBLUSERGROUP2USER where USERGROUPCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(User)), userGroupCode)));
        }

        /// <summary>
        /// ** ��������:	��UserGroupCode�������UserGroup��User������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <param name="userCode">UserCode,ģ����ѯ</param>
        /// <returns>User������</returns>
        public int GetSelectedUserByUserGroupCodeCount(string userGroupCode, string userCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLUSERGROUP2USER where USERGROUPCODE ='{0}' and USERCODE like '{1}%'", userGroupCode, userCode)));
        }

        public int GetSelectedUserGroupByUserCodeCount(string userGroupCode, string userCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLUSERGROUP2USER where USERGROUPCODE like'{0}%' and USERCODE = '{1}'", userGroupCode, userCode)));
        }

        /// <summary>
        /// ** ��������:	��UserGroupCode�������UserGroup��User����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <param name="userCode">UserCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>User����</returns>
        public object[] GetSelectedUserByUserGroupCode(string userGroupCode, string userCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(User),
                new PagerCondition(string.Format("select {0} from TBLUSER where USERCODE in ( select USERCODE from TBLUSERGROUP2USER where USERGROUPCODE ='{1}') and USERCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(User)), userGroupCode, userCode), inclusive, exclusive));
        }

        public object[] GetSelectedUserAndOrgIDByUserGroupCode(string userGroupCode, string userCode, int inclusive, int exclusive)
        {
            string sql = string.Empty;
            sql += " SELECT C.*, NVL(d.ORGDESC, ' ') AS ORGDESC FROM ";
            sql += " (select * from TBLUSER where USERCODE in ( select USERCODE from TBLUSERGROUP2USER where USERGROUPCODE ='" + userGroupCode.Trim() + "') and USERCODE like '" + userCode.Trim() + "%') c";
            sql += " LEFT OUTER JOIN (SELECT A.USERCODE, B.ORGDESC ";
            sql += "                 FROM TBLUSER2ORG A, TBLORG B ";
            sql += "          WHERE A.ORGID = B.ORGID ";
            sql += "       AND A.DEFAULTORG = 1) d ON c.USERCODE = d.USERCODE ";

            return this.DataProvider.CustomQuery(typeof(UserEx), new PagerCondition(sql, inclusive, exclusive));
        }

        public object[] GetSelectedUserGroupByUserCode(string userGroupCode, string userCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(UserGroup),
                new PagerCondition(string.Format("select {0} from TBLUSERGROUP where USERGROUPCODE in ( select USERGROUPCODE from TBLUSERGROUP2USER where USERCODE = '{2}') and USERGROUPCODE like '{1}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup)), userGroupCode, userCode), inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	��UserGroupCode��ò�����UserGroup��User������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <param name="userCode">UserCode,ģ����ѯ</param>
        /// <returns>User������</returns>
        public int GetUnselectedUserByUserGroupCodeCount(string userGroupCode, string userCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLUSER where USERCODE not in ( select USERCODE from TBLUSERGROUP2USER where USERGROUPCODE ='{0}') and USERCODE like '{1}%'", userGroupCode, userCode)));
        }

        public int GetUnselectedUserGroupByUserCodeCount(string userGroupCode, string userCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLUSERGROUP where USERGROUPCODE not in ( select USERGROUPCODE from TBLUSERGROUP2USER where USERCODE ='{1}') and USERGROUPCODE like '{0}%'", userGroupCode, userCode)));
        }

        /// <summary>
        /// ** ��������:	��UserGroupCode��ò�����UserGroup��User����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-22 11:55:39
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <param name="userCode">UserCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>User����</returns>
        public object[] GetUnselectedUserByUserGroupCode(string userGroupCode, string userCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(User),
                new PagerCondition(string.Format("select {0} from TBLUSER where USERCODE not in ( select USERCODE from TBLUSERGROUP2USER where USERGROUPCODE ='{1}') and USERCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(User)), userGroupCode, userCode), inclusive, exclusive));
        }


        public object[] GetUnselectedUserAndOrgIDByUserGroupCode(string userGroupCode, string userCode, int inclusive, int exclusive)
        {
            string sql = string.Empty;
            sql += " SELECT C.*, NVL(d.ORGDESC, ' ') AS ORGDESC FROM ";
            sql += " (select * from TBLUSER where USERCODE not in ( select USERCODE from TBLUSERGROUP2USER where USERGROUPCODE ='" + userGroupCode.Trim() + "') and USERCODE like '" + userCode.Trim() + "%') c";
            sql += " LEFT OUTER JOIN (SELECT A.USERCODE, B.ORGDESC ";
            sql += "                 FROM TBLUSER2ORG A, TBLORG B ";
            sql += "          WHERE A.ORGID = B.ORGID ";
            sql += "       AND A.DEFAULTORG = 1) d ON c.USERCODE = d.USERCODE ";

            return this.DataProvider.CustomQuery(typeof(UserEx), new PagerCondition(sql, inclusive, exclusive));
        }

        public object[] GetUnselectedUserGroupByUserCode(string userGroupCode, string userCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(UserGroup),
                new PagerCondition(string.Format("select {0} from TBLUSERGROUP where USERGROUPCODE not in ( select USERGROUPCODE from TBLUSERGROUP2USER where USERCODE ='{2}') and USERGROUPCODE like '{1}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup)), userGroupCode, userCode), inclusive, exclusive));
        }

        #endregion

        // Added By Jane Shu
        public object[] GetUserGroupofUser(string userCode)
        {
            return this.DataProvider.CustomQuery(typeof(UserGroup), new SQLCondition(string.Format("select {0} from TBLUSERGROUP where USERGROUPCODE in ( select USERGROUPCODE from TBLUSERGROUP2USER where USERCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup)), userCode)));
        }

        //add by sam ��λ��������
        public string[] GetUserGroupCodeofUser(string userCode)
        {
            object[] list = this.DataProvider.CustomQuery(typeof(UserGroup), new SQLCondition(string.Format("select {0} from TBLUSERGROUP where USERGROUPCODE in ( select USERGROUPCODE from TBLUSERGROUP2USER where USERCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup)), userCode)));
            if (list != null)
            {
                List<string> userlist = new List<string>();
                foreach (UserGroup userGroup in list)
                {
                    userlist.Add("'" + userGroup.UserGroupCode + "TOSTORAGE" + "'");
                }
                return userlist.ToArray();
            }
            return null;
        }


        public string[] GetUserGroupCodeofUser1(string userCode)
        {
            object[] list = this.DataProvider.CustomQuery(typeof(UserGroup), new SQLCondition(string.Format("select {0} from TBLUSERGROUP where USERGROUPCODE in ( select USERGROUPCODE from TBLUSERGROUP2USER where USERCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup)), userCode)));
            List<string> userlist = new List<string>();
            if (list != null && list.Length > 0)
            {

                foreach (UserGroup userGroup in list)
                {
                    userlist.Add(userGroup.UserGroupCode + "TOSTORAGE");
                }
               
            }
            return userlist.ToArray();
        }
        #endregion

        #region UserGroup2Vendor--��Ӧ�������û��� add by jinger 2016-01-25
        /// <summary>
        /// TBLUSERGROUP2VENDOR
        /// </summary>
        public UserGroup2Vendor CreateNewUserGroup2Vendor()
        {
            return new UserGroup2Vendor();
        }

        public void AddUserGroup2Vendor(UserGroup2Vendor userGroup2Vendor)
        {
            this._helper.AddDomainObject(userGroup2Vendor);
        }

        public void AddUserGroup2Vendor(UserGroup2Vendor[] userGroup2Vendors)
        {
            this._helper.AddDomainObject(userGroup2Vendors);
        }

        public void DeleteUserGroup2Vendor(UserGroup2Vendor userGroup2Vendor)
        {
            this._helper.DeleteDomainObject(userGroup2Vendor);
        }

        public void DeleteUserGroup2Vendor(UserGroup2Vendor[] userGroup2Vendors)
        {
            this._helper.DeleteDomainObject(userGroup2Vendors);
        }

        public void UpdateUserGroup2Vendor(UserGroup2Vendor userGroup2Vendor)
        {
            this._helper.UpdateDomainObject(userGroup2Vendor);
        }

        public object GetUserGroup2Vendor(string UserGroupCode, string VendorCode)
        {
            return this.DataProvider.CustomSearch(typeof(UserGroup2Vendor), new object[] { UserGroupCode, VendorCode });
        }

        //�жϵ�ǰ�û��Ƿ�Ϊ��Ӧ��
        /// <summary>
        /// �жϵ�ǰ�û��Ƿ�Ϊ��Ӧ��
        /// </summary>
        /// <param name="userCode">�û�����</param>
        /// <returns>�ǹ�Ӧ��true;���ǹ�Ӧ��false</returns>
        public bool IsVendor(string userCode)
        {
            string sql = string.Format(@" SELECT {0} FROM TBLUSERGROUP2VENDOR WHERE USERGROUPCODE IN 
                                            (SELECT USERGROUPCODE FROM TBLUSERGROUP2USER WHERE USERCODE = '{1}')",
                                       DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup2Vendor)), userCode);
            object[] objVendors = this.DataProvider.CustomQuery(typeof(UserGroup2Vendor), new SQLCondition(sql));
            if (objVendors != null && objVendors.Length > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// ��ȡ�û��¹�Ӧ�̴���
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public object[] GetVendorCode(string userCode)
        {
            string sql = string.Format(@" SELECT {0} FROM TBLUSERGROUP2VENDOR WHERE USERGROUPCODE IN 
                                            (SELECT USERGROUPCODE FROM TBLUSERGROUP2USER WHERE USERCODE = '{1}')",
                                       DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup2Vendor)), userCode);
            object[] objVendors = this.DataProvider.CustomQuery(typeof(UserGroup2Vendor), new SQLCondition(sql));
            if (objVendors != null && objVendors.Length > 0)
            {
                return objVendors;
            }
            return null;
        }
        //��ȡ�û�������ѡ��Ĺ�Ӧ��
        /// <summary>
        /// ��ȡ�û����µĹ�Ӧ��
        /// </summary>
        /// <param name="userGroupCode">�û������</param>
        /// <param name="vendorCode">��Ӧ�̴���</param>
        /// <param name="inclusive">��ʼ�к�</param>
        /// <param name="exclusive">�����к�</param>
        /// <returns></returns>
        public object[] GetSelectedVendorByUserGroupCode(string userGroupCode, string vendorCode, int inclusive, int exclusive)
        {
            string sql = string.Format(@"SELECT {0} FROM TBLVENDOR 
                                          WHERE VENDORCODE IN (SELECT VENDORCODE FROM  TBLUSERGROUP2VENDOR WHERE USERGROUPCODE='{1}')
                                            AND VENDORCODE LIKE '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Vendor)), userGroupCode, vendorCode);

            return this.DataProvider.CustomQuery(typeof(Vendor), new PagerCondition(sql, inclusive, exclusive));
        }

        //��ȡ�û�������ѡ��Ĺ�Ӧ��������
        /// <summary>
        /// ��ȡ�û����¹�Ӧ��������
        /// </summary>
        /// <param name="userGroupCode">�û������</param>
        /// <param name="vendorCode">��Ӧ�̴���</param>
        /// <returns></returns>
        public int GetSelectedVendorByUserGroupCodeCount(string userGroupCode, string vendorCode)
        {
            string sql = string.Format(@"SELECT COUNT(1) FROM TBLVENDOR 
                                          WHERE VENDORCODE IN (SELECT VENDORCODE FROM  TBLUSERGROUP2VENDOR WHERE USERGROUPCODE='{0}')
                                            AND VENDORCODE LIKE '{1}%'", userGroupCode, vendorCode);

            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        //��ȡ�û�����δѡ��Ĺ�Ӧ��
        /// <summary>
        /// ��ȡ�û����µĹ�Ӧ��
        /// </summary>
        /// <param name="userGroupCode">�û������</param>
        /// <param name="vendorCode">��Ӧ�̴���</param>
        /// <param name="inclusive">��ʼ�к�</param>
        /// <param name="exclusive">�����к�</param>
        /// <returns></returns>
        public object[] GetUnSelectedVendorByUserGroupCode(string userGroupCode, string vendorCode, int inclusive, int exclusive)
        {
            string sql = string.Format(@"SELECT {0} FROM TBLVENDOR 
                                          WHERE VENDORCODE NOT IN (SELECT VENDORCODE FROM  TBLUSERGROUP2VENDOR WHERE USERGROUPCODE='{1}')
                                            AND VENDORCODE LIKE '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Vendor)), userGroupCode, vendorCode);

            return this.DataProvider.CustomQuery(typeof(Vendor), new PagerCondition(sql, inclusive, exclusive));
        }

        //��ȡ�û�����δѡ��Ĺ�Ӧ��������
        /// <summary>
        /// ��ȡ�û����¹�Ӧ��������
        /// </summary>
        /// <param name="userGroupCode">�û������</param>
        /// <param name="vendorCode">��Ӧ�̴���</param>
        /// <returns></returns>
        public int GetUnSelectedVendorByUserGroupCodeCount(string userGroupCode, string vendorCode)
        {
            string sql = string.Format(@"SELECT COUNT(1) FROM TBLVENDOR 
                                          WHERE VENDORCODE NOT IN (SELECT VENDORCODE FROM  TBLUSERGROUP2VENDOR WHERE USERGROUPCODE='{0}')
                                            AND VENDORCODE LIKE '{1}%'", userGroupCode, vendorCode);

            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        #endregion

        #region UserGroup2Resource
        /// <summary>
        /// 
        /// </summary>
        public UserGroup2Resource CreateNewUserGroup2Resource()
        {
            return new UserGroup2Resource();
        }

        public void AddUserGroup2Resource(UserGroup2Resource userGroup2Resource)
        {
            this._helper.AddDomainObject(userGroup2Resource);
        }

        public void UpdateUserGroup2Resource(UserGroup2Resource userGroup2Resource)
        {
            this._helper.UpdateDomainObject(userGroup2Resource);
        }

        public void DeleteUserGroup2Resource(UserGroup2Resource userGroup2Resource)
        {
            this._helper.DeleteDomainObject(userGroup2Resource);
        }

        public void DeleteUserGroup2Resource(UserGroup2Resource[] userGroup2Resource)
        {
            this._helper.DeleteDomainObject(userGroup2Resource);
        }

        public object GetUserGroup2Resource(string userGroupCode, string resourceCode)
        {
            return this.DataProvider.CustomSearch(typeof(UserGroup2Resource), new object[] { userGroupCode, resourceCode });
        }

        /// <summary>
        ///  ���û����������UserGroup��UserGroup2Resource������
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        public int GetUser2ResourceCount(string userCode, string resourceCode)
        {
            //			return this.DataProvider.GetCount( 
            //					new SQLParamCondition("select count(*) from TBLUSERGROUP2RES where USERGROUPCODE in (select USERGROUPCODE from TBLUSERGROUP2USER where USERCODE=$USERCODE) and RESCODE=$RESCODE", 
            //					new SQLParameter[]{ 
            //										new SQLParameter("USERCODE", typeof(string), userCode),
            //										new SQLParameter("RESCODE", typeof(string), resourceCode)}));
            int count = this.DataProvider.GetCount(
                new SQLCondition(
                string.Format("select count(*) from TBLUSERGROUP2USER where USERGROUPCODE='ADMIN' and  USERCODE='{0}'",
                userCode)));

            if (count > 0)
            {
                return count;
            }

            return this.DataProvider.GetCount(
                    new SQLCondition(
                    string.Format("select count(*) from TBLUSERGROUP2RES where USERGROUPCODE in (select USERGROUPCODE from TBLUSERGROUP2USER where USERCODE='{0}') and RESCODE='{1}' ",
                    userCode, resourceCode)));

        }

        /// <summary>
        /// ** ��������:	��ѯUserGroup2Resource��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-30 13:07:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode��ģ����ѯ</param>
        /// <param name="resourceCode">ResourceCode��ģ����ѯ</param>
        /// <returns> UserGroup2Resource���ܼ�¼��</returns>
        public int QueryUserGroup2ResourceCount(string userGroupCode, string resourceCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLUSERGROUP2RES where 1=1 and USERGROUPCODE like '{0}%'  and RESCODE like '{1}%' ", userGroupCode, resourceCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯUserGroup2Resource
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-30 13:07:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode��ģ����ѯ</param>
        /// <param name="resourceCode">ResourceCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> UserGroup2Resource����</returns>
        public object[] QueryUserGroup2Resource(string userGroupCode, string resourceCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(UserGroup2Resource), new PagerCondition(string.Format("select {0} from TBLUSERGROUP2RES where 1=1 and USERGROUPCODE like '{1}%'  and RESCODE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup2Resource)), userGroupCode, resourceCode), "USERGROUPCODE,RESCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�UserGroup2Resource
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-30 13:07:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>UserGroup2Resource���ܼ�¼��</returns>
        public object[] GetAllUserGroup2Resource()
        {
            return this.DataProvider.CustomQuery(typeof(UserGroup2Resource), new SQLCondition(string.Format("select {0} from TBLUSERGROUP2RES order by USERGROUPCODE,RESCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(UserGroup2Resource)))));
        }

        public void AddUserGroup2Resource(UserGroup2Resource[] userGroup2Resources)
        {
            this._helper.AddDomainObject(userGroup2Resources);
        }

        #region UserGroup --> Resource
        /// <summary>
        /// ** ��������:	��UserGroupCode���Resource
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-30 13:07:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <returns>Resource����</returns>
        public object[] GetResourceByUserGroupCode(string userGroupCode)
        {
            // Added by HI1/Venus.Feng on 20080624 for Hisense Version : Add Organization ID
            string sql = "";
            sql += "select {0} from TBLRES where RESCODE in ( select RESCODE from TBLUSERGROUP2RES where USERGROUPCODE='{1}')";
            sql += GlobalVariables.CurrentOrganizations.GetSQLCondition();
            // End Added 
            return this.DataProvider.CustomQuery(typeof(Resource), new SQLCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(Resource)), userGroupCode)));
        }

        /// <summary>
        /// ** ��������:	��UserGroupCode�������UserGroup��Resource������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-30 13:07:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <param name="resourceCode">ResourceCode,ģ����ѯ</param>
        /// <returns>Resource������</returns>
        public int GetSelectedResourceByUserGroupCodeCount(string userGroupCode, string resourceCode)
        {
            // Added by HI1/Venus.Feng on 20080624 for Hisense Version : Add Organization ID
            string sql = "";
            sql += "select count(*) from TBLUSERGROUP2RES where USERGROUPCODE ='{0}' and RESCODE like '{1}%'";
            // End Added

            return this.DataProvider.GetCount(new SQLCondition(string.Format(sql, userGroupCode, resourceCode)));
        }

        /// <summary>
        /// ** ��������:	��UserGroupCode�������UserGroup��Resource����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-30 13:07:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <param name="resourceCode">ResourceCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>Resource����</returns>
        public object[] GetSelectedResourceByUserGroupCode(string userGroupCode, string resourceCode, int inclusive, int exclusive)
        {
            // Added by HI1/Venus.Feng on 20080624 for Hisense Version : Add Organization ID
            string sql = "";
            sql += "select {0} from TBLRES where RESCODE in ( select RESCODE from TBLUSERGROUP2RES where USERGROUPCODE ='{1}') and RESCODE like '{2}%'";
            sql += GlobalVariables.CurrentOrganizations.GetSQLCondition();
            // End Added

            return this.DataProvider.CustomQuery(typeof(Resource),
                new PagerCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(Resource)), userGroupCode, resourceCode), "RESCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	��UserGroupCode��ò�����UserGroup��Resource������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-30 13:07:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <param name="resourceCode">ResourceCode,ģ����ѯ</param>
        /// <returns>Resource������</returns>
        public int GetUnselectedResourceByUserGroupCodeCount(string userGroupCode, string resourceCode)
        {
            // Added by HI1/Venus.Feng on 20080624 for Hisense Version : Add Organization ID
            string sql = "";
            sql += "select count(*) from TBLRES where RESCODE not in ( select RESCODE from TBLUSERGROUP2RES where USERGROUPCODE ='{0}') and RESCODE like '{1}%'";
            sql += GlobalVariables.CurrentOrganizations.GetSQLCondition();
            // End Added
            return this.DataProvider.GetCount(new SQLCondition(string.Format(sql, userGroupCode, resourceCode)));
        }

        /// <summary>
        /// ** ��������:	��UserGroupCode��ò�����UserGroup��Resource����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-30 13:07:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userGroupCode">UserGroupCode,��ȷ��ѯ</param>
        /// <param name="resourceCode">ResourceCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>Resource����</returns>
        public object[] GetUnselectedResourceByUserGroupCode(string userGroupCode, string resourceCode, int inclusive, int exclusive)
        {
            // Added by HI1/Venus.Feng on 20080624 for Hisense Version : Add Organization ID
            string sql = "";
            sql += "select {0} from TBLRES where RESCODE not in ( select RESCODE from TBLUSERGROUP2RES where USERGROUPCODE ='{1}') and RESCODE like '{2}%'";
            sql += GlobalVariables.CurrentOrganizations.GetSQLCondition();
            // End Added

            return this.DataProvider.CustomQuery(typeof(Resource),
                new PagerCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(Resource)), userGroupCode, resourceCode), "RESCODE", inclusive, exclusive));
        }
        #endregion

        #endregion
    }
}
