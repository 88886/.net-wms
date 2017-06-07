using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for SystemSetting
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2005-04-29 10:36:31
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.BaseSetting
{

	#region Menu �˵�
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLMENU", "MENUCODE")]
	public class Menu : DomainObject
	{
		public Menu()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MENUCODE", typeof(string), 40, true)]
		public string  MenuCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MENUDESC", typeof(string), 100, false)]
		public string  MenuDescription;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDLCODE", typeof(string), 40, false)]
		public string  ModuleCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MENUSEQ", typeof(decimal), 10, true)]
		public decimal  MenuSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PMENUCODE", typeof(string), 40, false)]
		public string  ParentMenuCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MENUTYPE", typeof(string), 40, true)]
		public string  MenuType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("VISIBILITY", typeof(string), 40, true)]
		public string  Visibility;

	}
	#endregion

	#region Module ����ģ��
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLMDL", "MDLCODE")]
	public class Module : DomainObject
	{
		public Module()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDLCODE", typeof(string), 40, true)]
		public string  ModuleCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDLDESC", typeof(string), 100, false)]
		public string  ModuleDescription;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDLSEQ", typeof(decimal), 10, true)]
		public decimal  ModuleSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDLHFNAME", typeof(string), 100, false)]
		public string  ModuleHelpFileName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ISACTIVE", typeof(string), 1, true)]
		public string  IsActive;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ISSYS", typeof(string), 1, true)]
		public string  IsSystem;

		/// <summary>
		/// C/S  B/S
		/// </summary>
		[FieldMapAttribute("MDLTYPE", typeof(string), 40, true)]
		public string  ModuleType;

		/// <summary>
		/// Alpha/Beta/Release
		/// </summary>
		[FieldMapAttribute("MDLSTATUS", typeof(string), 40, true)]
		public string  ModuleStatus;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FORMURL", typeof(string), 100, false)]
		public string  FormUrl;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDLVER", typeof(string), 40, false)]
		public string  ModuleVersion;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PMDLCODE", typeof(string), 40, false)]
		public string  ParentModuleCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ISRESTRAIN", typeof(string), 1, false)]
		public string  IsRestrain;

	}
	#endregion

	#region Parameter ����
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSYSPARAM", "PARAMCODE,PARAMGROUPCODE")]
	public class Parameter : DomainObject
	{
		public Parameter()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARAMCODE", typeof(string), 40, true)]
		public string  ParameterCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARAMDESC", typeof(string), 100, false)]
		public string  ParameterDescription;

		/// <summary>
		/// 1 -  ʹ����
		/// 0 -  ����
		/// 
		/// </summary>
		[FieldMapAttribute("ISACTIVE", typeof(string), 1, true)]
		public string  IsActive;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 1  ϵͳ
		/// 0  �û�
		/// </summary>
		[FieldMapAttribute("ISSYS", typeof(string), 1, true)]
		public string  IsSystem;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARAMALIAS", typeof(string), 40, false)]
		public string  ParameterAlias;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARAMVALUE", typeof(string), 100, false)]
		public string  ParameterValue;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARAMGROUPCODE", typeof(string), 40, true)]
		public string  ParameterGroupCode;

		/// <summary>
		/// sequence ����˳��
		/// </summary>
		[FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
		public string  ParameterSequence;

		/// <summary>
		/// ������������
		/// </summary>
		[FieldMapAttribute("PARENTPARAM", typeof(string), 40, false)]
		public string  ParentParameterCode;

	}
	#endregion

	#region ParameterGroup ������
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSYSPARAMGROUP", "PARAMGROUPCODE")]
	public class ParameterGroup : DomainObject
	{
		public ParameterGroup()
		{
		}
 
		/// <summary>
		/// Static/System Parameter/ User Parameter
		/// </summary>
		[FieldMapAttribute("PARAMGROUPTYPE", typeof(string), 40, true)]
		public string  ParameterGroupType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARAMGROUPDESC", typeof(string), 100, false)]
		public string  ParameterGroupDescription;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ISSYS", typeof(string), 1, true)]
		public string  IsSystem;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARAMGROUPCODE", typeof(string), 40, true)]
		public string  ParameterGroupCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

	}
	#endregion

	#region User �û�
	/// <summary>
	/// ϵͳ�û�
	/// </summary>
	[Serializable, TableMap("TBLUSER", "USERCODE")]
	public class User : DomainObject
	{
		public User()
		{
		}
 
		/// <summary>
		/// �û���
		/// </summary>
		[FieldMapAttribute("USERNAME", typeof(string), 40, false)]
		public string  UserName;

		/// <summary>
		/// �û�����
		/// </summary>
		[FieldMapAttribute("USERPWD", typeof(string), 40, true)]
		public string  UserPassword;

		/// <summary>
		/// �绰����
		/// </summary>
		[FieldMapAttribute("USERTEL", typeof(string), 40, false)]
		public string  UserTelephone;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("USEREMAIL", typeof(string), 100, false)]
		public string  UserEmail;

		/// <summary>
		/// ���ά������[LastMaintainDate]
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// ���ά��ʱ��[LastMaintainTime]
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ���ά���û�[LastMaintainUser]
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]

		public string  MaintainUser;

		/// <summary>
		/// �û����ڵĲ���
		/// �����û�ϵͳ�����Ķ���, ϵͳ�������� USERDEPART
		/// </summary>
		[FieldMapAttribute("USERDEPART", typeof(string), 40, false)]
		public string  UserDepartment;

		/// <summary>
		/// �û�����
		/// </summary>
		[FieldMapAttribute("USERCODE", typeof(string), 40, true)]
		public string  UserCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		[FieldMapAttribute("USERSTAT", typeof(string), 40, true)]
		public string  UserStatus;
	}
	#endregion

	#region UserGroup �û���
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLUSERGROUP", "USERGROUPCODE")]
	public class UserGroup : DomainObject
	{
		public UserGroup()
		{
		}
 
		/// <summary>
		/// �û�������
		/// </summary>
		[FieldMapAttribute("USERGROUPDESC", typeof(string), 100, false)]
		public string  UserGroupDescription;

		/// <summary>
		/// ���ά���û�[LastMaintainUser]
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
		public string  MaintainUser;

		/// <summary>
		/// ���ά������[LastMaintainDate]
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// ���ά��ʱ��[LastMaintainTime]
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// �û������
		/// </summary>
		[FieldMapAttribute("USERGROUPCODE", typeof(string), 40, true)]
		public string  UserGroupCode;

		/// <summary>
		/// �û������
		/// ����ϵͳ�����Ķ���, ϵͳ�������� USERGROUPTYPE
		/// 1. Administrator
		/// 2. Guest
		/// </summary>
		[FieldMapAttribute("USERGROUPTYPE", typeof(string), 40, true)]
		public string  UserGroupType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

	}
	#endregion

	#region UserGroup2Module
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLUSERGROUP2MODULE", "MDLCODE,USERGROUPCODE")]
	public class UserGroup2Module : DomainObject
	{
		public UserGroup2Module()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDLCODE", typeof(string), 40, true)]
		public string  ModuleCode;

		/// <summary>
		/// �û������
		/// </summary>
		[FieldMapAttribute("USERGROUPCODE", typeof(string), 40, true)]
		public string  UserGroupCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("VIEWVALUE", typeof(string), 40, false)]
		public string  ViewValue;

	}
	#endregion

	#region UserGroup2Resource
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLUSERGROUP2RES", "RESCODE,USERGROUPCODE")]
	public class UserGroup2Resource : DomainObject
	{
		public UserGroup2Resource()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string  ResourceCode;

		/// <summary>
		/// �û������
		/// </summary>
		[FieldMapAttribute("USERGROUPCODE", typeof(string), 40, true)]
		public string  UserGroupCode;

	}
	#endregion

	#region UserGroup2User
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLUSERGROUP2USER", "USERCODE,USERGROUPCODE")]
	public class UserGroup2User : DomainObject
	{
		public UserGroup2User()
		{
		}
 
		/// <summary>
		/// ���ά���û�[LastMaintainUser]
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// ���ά������[LastMaintainDate]
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// ���ά��ʱ��[LastMaintainTime]
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// �û�����
		/// </summary>
		[FieldMapAttribute("USERCODE", typeof(string), 40, true)]
		public string  UserCode;

		/// <summary>
		/// �û������
		/// </summary>
		[FieldMapAttribute("USERGROUPCODE", typeof(string), 40, true)]
		public string  UserGroupCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

	}
	#endregion

    #region UserGroup2Vendor-- ��Ӧ�������û��� add by jinger 2016-01-25
    /// <summary>
    /// TBLUSERGROUP2VENDOR-- ��Ӧ�������û��� 
    /// </summary>
    [Serializable, TableMap("TBLUSERGROUP2VENDOR", "USERGROUPCODE,VENDORCODE")]
    public class UserGroup2Vendor : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public UserGroup2Vendor()
        {
        }

        ///<summary>
        ///�����ֶ�
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;

        ///<summary>
        ///ά������ 
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///�û������
        ///</summary>
        [FieldMapAttribute("USERGROUPCODE", typeof(string), 40, false)]
        public string UserGroupCode;

        ///<summary>
        ///��Ӧ�̴���
        ///</summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, false)]
        public string VendorCode;

    }
    #endregion

	#region SystemError
	/// <summary>
	/// ϵͳ�����¼
	/// </summary>
	[Serializable, TableMap("TBLSYSERROR", "SYSERRCODE")]
	public class SystemError : DomainObject
	{
		public SystemError()
		{
		}
 
		/// <summary>
		/// ϵͳ�������
		/// </summary>
		[FieldMapAttribute("SYSERRCODE", typeof(string), 40, true)]
		public string  SystemErrorCode;

		/// <summary>
		/// ������Ϣ
		/// </summary>
		[FieldMapAttribute("ERRMSG", typeof(string), 100, true)]
		public string  ErrorMessage;

		/// <summary>
		/// �ڲ�������Ϣ
		/// </summary>
		[FieldMapAttribute("INNERERRMSG", typeof(string), 100, false)]
		public string  InnerErrorMessage;

		/// <summary>
		/// �ύ�û�
		/// </summary>
		[FieldMapAttribute("SENDUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
		public string  SendUser;

		/// <summary>
		/// �ύ����
		/// </summary>
		[FieldMapAttribute("SENDDATE", typeof(int), 8, true)]
		public int  SendDate;

		/// <summary>
		/// �ύʱ��
		/// </summary>
		[FieldMapAttribute("SENDTIME", typeof(int), 6, true)]
		public int  SendTime;

		/// <summary>
		/// �Ƿ��ѽ��
		/// </summary>
		[FieldMapAttribute("ISRES", typeof(string), 1, true)]
		public string  IsResolved;

		/// <summary>
		/// �����ע
		/// </summary>
		[FieldMapAttribute("RESNOTES", typeof(string), 100, false)]
		public string  ResolveNotes;

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("RESDATE", typeof(int), 8, false)]
		public int  ResolveDate;

		/// <summary>
		/// ���ʱ��
		/// </summary>
		[FieldMapAttribute("RESTIME", typeof(int), 6, false)]
		public int  ResolveTime;

		/// <summary>
		/// ά���û�
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
		public string  MaintainUser;

		/// <summary>
		/// ά������
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// ά��ʱ��
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ����û�
		/// </summary>
		[FieldMapAttribute("RESUSER", typeof(string), 40, false)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
		public string  ResolveUser;

		/// <summary>
		/// ��������ģ��
		/// </summary>
		[FieldMapAttribute("TRGMDLCODE", typeof(string), 40, false)]
		public string  TriggerModuleCode;

		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("TRIGACTION", typeof(string), 40, false)]
		public string  TriggerAction;

	}
	#endregion

	#region USERGROUP2ITEM
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("USERGROUP2ITEM", "USERGROUPCODE,ITEMCODE")]
	public class USERGROUP2ITEM : DomainObject
	{
		public USERGROUP2ITEM()
		{
		}
 
		/// <summary>
		/// �û������
		/// </summary>
		[FieldMapAttribute("USERGROUPCODE", typeof(string), 40, true)]
		public string  USERGROUPCODE;

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ITEMCODE;

		/// <summary>
		/// ά����Ա
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MUSER;

		/// <summary>
		/// ά������
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MDATE;

		/// <summary>
		/// ά��ʱ��
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MTIME;

		/// <summary>
		/// �Ƿ���Ч
		/// </summary>
		[FieldMapAttribute("ISAVAILABLE", typeof(decimal), 10, true)]
		public decimal  ISAVAILABLE;

		/// <summary>
		/// ��ע
		/// </summary>
		[FieldMapAttribute("EATTRIBUTE1", typeof(string), 100, false)]
		public string  EATTRIBUTE1;

	}
	#endregion

    #region BSHomeSetting

    /// <summary>
    ///	BSHomeSetting
    /// </summary>
    [Serializable, TableMap("TBLBSHOMESETTING", "REPORTSEQ")]
    public class BSHomeSetting : DomainObject
    {
        public BSHomeSetting()
        {
        }

        ///<summary>
        ///ReportSeq
        ///</summary>	
        [FieldMapAttribute("REPORTSEQ", typeof(int), 38, false)]
        public int ReportSeq;

        ///<summary>
        ///ModuleCode
        ///</summary>	
        [FieldMapAttribute("MDLCODE", typeof(string), 40, false)]
        public string ModuleCode;

        ///<summary>
        ///ChartType
        ///</summary>	
        [FieldMapAttribute("CHARTTYPE", typeof(string), 40, false)]
        public string ChartType;

        ///<summary>
        ///MaintainUser
        ///</summary>	
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>	
        [FieldMapAttribute("MDATE", typeof(int), 38, false)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>	
        [FieldMapAttribute("MTIME", typeof(int), 38, false)]
        public int MaintainTime;

    }

    #endregion

    #region BSHomeSettingDetail

    /// <summary>
    ///	BSHomeSettingDetail
    /// </summary>
    [Serializable, TableMap("TBLBSHOMESETTINGDETAIL", "REPORTSEQ, PARAMNAME")]
    public class BSHomeSettingDetail : DomainObject
    {
        public BSHomeSettingDetail()
        {
        }

        ///<summary>
        ///ReportSeq
        ///</summary>	
        [FieldMapAttribute("REPORTSEQ", typeof(int), 38, false)]
        public int ReportSeq;

        ///<summary>
        ///ParameterName
        ///</summary>	
        [FieldMapAttribute("PARAMNAME", typeof(string), 100, false)]
        public string ParameterName;

        ///<summary>
        ///ParameterValue
        ///</summary>	
        [FieldMapAttribute("PARAMVALUE", typeof(string), 2000, false)]
        public string ParameterValue;

        ///<summary>
        ///MaintainUser
        ///</summary>	
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>	
        [FieldMapAttribute("MDATE", typeof(int), 38, false)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>	
        [FieldMapAttribute("MTIME", typeof(int), 38, false)]
        public int MaintainTime;

    }

    #endregion
}

