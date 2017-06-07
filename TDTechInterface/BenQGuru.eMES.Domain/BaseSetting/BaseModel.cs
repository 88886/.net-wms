using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for BaseModel
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2005-04-29 10:36:04
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.BaseSetting
{

	#region Operation
	/// <summary>
	/// �����б�
	/// </summary>
	[Serializable, TableMap("TBLOP", "OPCODE")]
	public class Operation : DomainObject
	{
		public Operation()
		{
		}
 
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OPDESC", typeof(string), 100, false)]
		public string  OPDescription;

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
		/// �����ռ���ʽ A�����Զ��ռ���Auto����M�����ֶ��ռ���Manual��
		/// </summary>
		[FieldMapAttribute("OPCOLLECTION", typeof(string), 40, true)]
		public string  OPCollectionType;

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string  OPCode;

		/// <summary>
		/// 1- �� 0- ��
		/// �Ƿ�Route Check
		/// �Ƿ�����Check
		/// �Ƿ���ҪMNID���
		/// �Ƿ�MO����
		/// �Ƿ�MO����
		/// �Ƿ��������ϼ�
		/// �Ƿ�SPCͳ��
		/// �Ƿ�����������
		/// �Ƿ����ж�
		/// </summary>
		[FieldMapAttribute("OPCONTROL", typeof(string), 40, true)]
		public string  OPControl;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

	}
	#endregion

	#region Operation2Resource
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLOP2RES", "OPCODE,RESCODE")]
	public class Operation2Resource : DomainObject
	{
		public Operation2Resource()
		{
		}
 
		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string  OPCode;

		/// <summary>
		/// ��Դ����
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string  ResourceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

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
		[FieldMapAttribute("RESSEQ", typeof(decimal), 10, true)]
		public decimal  ResourceSequence;

	}
	#endregion

	#region Resource
	/// <summary>
	/// ��Դ
	/// </summary>
	[Serializable, TableMap("TBLRES", "RESCODE")]
	public class Resource : DomainObject
	{
		public Resource()
		{
		}
 
		/// <summary>
		/// ��Դ����
		/// </summary>
		[FieldMapAttribute("RESDESC", typeof(string), 100, false)]
		public string  ResourceDescription;

		/// <summary>
		/// ��Դ����ʶ
		/// ����ϵͳ�����Ķ��壺
		/// 1. ϵͳ�������: RESOURCETYPE
		/// 
		/// Tools, Machine, Operator
		/// </summary>
		[FieldMapAttribute("RESTYPE", typeof(string), 40, true)]
		public string  ResourceType;

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
		/// ��Դ����
		/// �䶨�������û�ϵͳ�����Ķ���: 
		/// 1. ϵͳ��������: RESGROUP
		/// </summary>
		[FieldMapAttribute("RESGROUP", typeof(string), 40, false)]
		public string  ResourceGroup;

		/// <summary>
		/// ��Դ����
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string  ResourceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ���������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLSS", "SSCODE", "SSDESC")]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string  SegmentCode;

		/// <summary>
		/// ���ƴ���
		/// </summary>
		[FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
		public string  ShiftTypeCode;

        //Added by HI1/Venus.Feng on 20080624 for Hisense Version : Add Organization
        /// <summary>
        /// Organization ID
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrganizationID;
        //End

        [FieldMapAttribute("REWORKROUTECODE", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLROUTE", "ROUTECODE", "ROUTEDESC")]
        public string ReworkRouteCode;

        [FieldMapAttribute("REWORKCODE", typeof(string), 40, true)]
        public string ReworkCode;

        [FieldMapAttribute("DCTCODE", typeof(string), 40, true)]
        public string DctCode;

        [FieldMapAttribute("CREWCODE", typeof(string), 40, true)]
        public string CrewCode;
	}
	#endregion

    #region ResourceReworkLog
    /// <summary>
    /// ����
    /// </summary>
    [Serializable, TableMap("TBLRESREWRKLOG", "SEQ,RESCODE")]
    public class ResourceReworkLog : DomainObject
    {
        public ResourceReworkLog()
        {
        }

        /// <summary>
        /// ���ά������[LastMaintainDate]
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// ���ά��ʱ��[LastMaintainTime]
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
        public decimal Sequence;

        /// <summary>
        /// ��Ч����
        /// </summary>
        [FieldMapAttribute("REWORKROUTECODE", typeof(string), 40, false)]
        public string ReworkRouteCode;

        /// <summary>
        /// ʧЧ����
        /// </summary>
        [FieldMapAttribute("OLDREWORKROUTECODE", typeof(string), 40, false)]
        public string OldReworkRouteCode;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string ResourceCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

    }
    #endregion

    #region Resource2ReworkSheet

    /// <summary>
    ///	Resource2ReworkSheet
    /// </summary>
    [Serializable, TableMap("TBLRES2REWORKSHEET", "RESCODE,REWORKCODE")]
    public class Resource2ReworkSheet : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Resource2ReworkSheet()
        {
        }

        ///<summary>
        ///ResourceCode
        ///</summary>	
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResourceCode;

        ///<summary>
        ///ReworkCode
        ///</summary>	
        [FieldMapAttribute("REWORKCODE", typeof(string), 40, false)]
        public string ReworkCode;

        ///<summary>
        ///ItemCode
        ///</summary>	
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        ///<summary>
        ///LotNo
        ///</summary>	
        [FieldMapAttribute("LOTNO", typeof(string), 40, true)]
        public string LotNo;

        ///<summary>
        ///MaintainUser
        ///</summary>	
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>	
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>	
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

    }

    #endregion

	#region Route
	/// <summary>
	/// ����;��
	/// </summary>
	[Serializable, TableMap("TBLROUTE", "ROUTECODE")]
	public class Route : DomainObject
	{
		public Route()
		{
		}
 
		/// <summary>
		/// ����;������
		/// </summary>
		[FieldMapAttribute("ROUTEDESC", typeof(string), 100, false)]
		public string  RouteDescription;

		/// <summary>
		/// ����;�����: 
		/// ����ϵͳ�����Ķ���:
		/// 1. ϵͳ��������:  ROUTETYPE
		/// Rework, Scrap, Normal, Refllow
		/// </summary>
		[FieldMapAttribute("ROUTETYPE", typeof(string), 40, true)]
		public string  RouteType;

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
		/// ��Чʱ��
		/// </summary>
		[FieldMapAttribute("EFFDATE", typeof(int), 8, true)]
		public int  EffectiveDate;

		/// <summary>
		/// ʧЧʱ��
		/// </summary>
		[FieldMapAttribute("IVLDATE", typeof(int), 8, true)]
		public int  InvalidDate;

		/// <summary>
		/// ����;�̴���
		/// </summary>
		[FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
		public string  RouteCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;


		[FieldMapAttribute("ENABLED", typeof(string), 1, false)]
		public string  Enabled;
	}
	#endregion

	#region Route2Operation
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLROUTE2OP", "ROUTECODE,OPCODE")]
	public class Route2Operation : DomainObject
	{
		public Route2Operation()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

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
		/// ����;�̴���
		/// </summary>
		[FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
		public string  RouteCode;

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string  OPCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPSEQ", typeof(decimal), 10, true)]
		public decimal  OPSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPCONTROL", typeof(string), 40, true)]
		public string  OPControl;

	}
	#endregion

	#region Segment
	/// <summary>
	/// ����
	/// </summary>
	[Serializable, TableMap("TBLSEG", "SEGCODE")]
	public class Segment : DomainObject
	{
		public Segment()
		{
		}
 
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("SEGDESC", typeof(string), 100, false)]
		public string  SegmentDescription;

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
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string  SegmentCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEGSEQ", typeof(decimal), 10, true)]
		public decimal  SegmentSequence;

		/// <summary>
		/// ���ƴ���
		/// </summary>
		[FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLSHIFTTYPE", "SHIFTTYPECODE", "SHIFTTYPEDESC")]
		public string  ShiftTypeCode;

        /// <summary>
        /// ��֯���
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, true)]
        public int OrganizationID;

        /// <summary>
        /// Factory Code
        /// </summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLFACTORY", "FACCODE", "FACDESC")]
        public string FactoryCode;
	}
	#endregion

	#region StepSequence
	/// <summary>
	/// ������
	/// </summary>
	[Serializable, TableMap("TBLSS", "SSCODE")]
	public class StepSequence : DomainObject
	{
		public StepSequence()
		{
		}
 
		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("SSDESC", typeof(string), 100, false)]
		public string  StepSequenceDescription;

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
		/// ���������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "", "", "SSDESC")]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLSEG", "SEGCODE", "SEGDESC")]
		public string  SegmentCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSSEQ", typeof(decimal), 10, true)]
		public decimal  StepSequenceOrder;

        /// <summary>
        /// ��֯���
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, true)]
        public int OrganizationID;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, false)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLSHIFTTYPE", "SHIFTTYPECODE", "SHIFTTYPEDESC")]
        public string ShiftTypeCode;

        /// <summary>
        /// Step Sequence Type
        /// </summary>
        [FieldMapAttribute("SSTYPE", typeof(string), 40, false)]
        public string StepSequenceType;

        [FieldMapAttribute("BIGSSCODE", typeof(string), 40, false)]
        public string BigStepSequenceCode;

        [FieldMapAttribute("SAVEINSTOCK", typeof(string), 40, false)]
        public string SaveInStock;
	}
	#endregion

    #region BigStepSequence (Big Line)

    [Serializable, TableMap("(SELECT paramalias AS bigsscode , paramdesc AS bigssdesc FROM tblsysparam WHERE paramgroupcode = 'BIGLINEGROUP') tblbisss ", "BIGSSCODE")]
    public class BigSS : DomainObject
    {
        public BigSS()
        {
        }

        [FieldMapAttribute("BIGSSCODE", typeof(string), 40, false)]
        public string BigSSCode;

        [FieldMapAttribute("BIGSSDESC", typeof(string), 100, true)]
        public string BigSSDescription; 
    }

    #endregion

    #region DCT
    /// <summary>
    /// DCT
    /// </summary>
    [Serializable, TableMap("TBLDCT", "DCTCODE")]
    public class Dct : DomainObject
    {
        public Dct()
        {
        }

        /// <summary>
        /// DCTָ��
        /// </summary>
        [FieldMapAttribute("DCTCODE", typeof(string), 40, false)]
        public string DctCode;

        /// <summary>
        /// DCT����
        /// </summary>
        [FieldMapAttribute("DCTDESC", typeof(string), 40, true)]
        public string Dctdesc;

        /// <summary>
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string MaintainUser;

        /// <summary>
        /// ���ά������[LastMaintainDate]
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// ���ά��ʱ��[LastMaintainTime]
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;        

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, true)]
        public string EAttribute1;

    }
    #endregion

    #region SoftWareVersion
    /// <summary>
    /// SoftWareVersion
    /// </summary>
    [Serializable, TableMap("TBLSOFTVER", "VERSIONCODE")]
    public class SoftWareVersion : DomainObject
    {
        public SoftWareVersion()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("VERSIONCODE", typeof(string), 40, false)]
        public string VersionCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string Status;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EFFDATE", typeof(int), 8, false)]
        public int EffectiveDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("INVDATE", typeof(int), 8, false)]
        public int InvalidDate;

        /// <summary>
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string MaintainUser;

        /// <summary>
        /// ���ά������[LastMaintainDate]
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// ���ά��ʱ��[LastMaintainTime]
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, true)]
        public string EAttribute1;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrganizationID;

    }
    #endregion

    #region RCardChange
    /// <summary>
    /// RCardChange
    /// </summary>
    [Serializable, TableMap("TBLRCARDCHANGE", "")]
    public class RCardChange : DomainObject
    {
        public RCardChange()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("RCARDFROM", typeof(string), 40, false)]
        public string RCardFrom;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("RCARDTO", typeof(string), 40, false)]
        public string RCardTo;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("REASON", typeof(string), 200, false)]
        public string Reason;

        /// <summary>
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        /// <summary>
        /// ���ά������[LastMaintainDate]
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// ���ά��ʱ��[LastMaintainTime]
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, true)]
        public string EAttribute1;

    }
    #endregion

    #region MESEntityList

    /// <summary>
    ///	MESEntityList
    /// </summary>
    [Serializable, TableMap("TBLMESENTITYLIST", "SERIAL")]
    public class MESEntityList : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public MESEntityList()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>	
        [FieldMapAttribute("SERIAL", typeof(int), 38, false)]
        public int Serial;

        ///<summary>
        ///BigSSCode
        ///</summary>	
        [FieldMapAttribute("BIGSSCODE", typeof(string), 40, false)]
        public string BigSSCode;

        ///<summary>
        ///ModelCode
        ///</summary>	
        [FieldMapAttribute("MODELCODE", typeof(string), 40, false)]
        public string ModelCode;

        ///<summary>
        ///OPCode
        ///</summary>	
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OPCode;

        ///<summary>
        ///SegmentCode
        ///</summary>	
        [FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
        public string SegmentCode;

        ///<summary>
        ///StepSequenceCode
        ///</summary>	
        [FieldMapAttribute("SSCODE", typeof(string), 40, false)]
        public string StepSequenceCode;

        ///<summary>
        ///ResourceCode
        ///</summary>	
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResourceCode;

        ///<summary>
        ///ShiftTypeCode
        ///</summary>	
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, false)]
        public string ShiftTypeCode;

        ///<summary>
        ///ShiftCode
        ///</summary>	
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, false)]
        public string ShiftCode;

        ///<summary>
        ///TPCode
        ///</summary>	
        [FieldMapAttribute("TPCODE", typeof(string), 40, false)]
        public string TPCode;

        ///<summary>
        ///FactoryCode
        ///</summary>	
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FactoryCode;

        ///<summary>
        ///OrganizationID
        ///</summary>	
        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrganizationID;

        ///<summary>
        ///EAttribute1
        ///</summary>	
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string EAttribute1;

    }

    #endregion
}

