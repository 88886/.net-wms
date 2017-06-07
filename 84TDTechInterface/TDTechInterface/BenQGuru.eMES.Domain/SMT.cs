using System;
using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.Domain.SMT
{

	#region Station
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSTATION", "STATIONCODE,RESCODE")]
	public class Station : DomainObject
	{
		public Station()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("STATIONCODE", typeof(string), 40, true)]
		public string  StationCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("STATIONDESC", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

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

	}
	#endregion

	#region SMTResourceBOM
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSMTRESBOM", "ITEMCODE,MOCODE,RESCODE,StationCode,OBITEMCODE")]
	public class SMTResourceBOM : DomainObject
	{
		public SMTResourceBOM()
		{
		}
 
		/// <summary>
		/// ��Ʒ��������
		/// </summary>
		[FieldMapAttribute("DATECODE", typeof(string), 100, false)]
		public string  DateCode;

		/// <summary>
		/// ���̴���
		/// </summary>
		[FieldMapAttribute("VENDORCODE", typeof(string), 100, false)]
		public string  VendorCode;

		/// <summary>
		/// �����Ϻ�
		/// </summary>
		[FieldMapAttribute("VENDERITEMCODE", typeof(string), 100, false)]
		public string  VenderItemCode;

		/// <summary>
		/// ��Ʒ���
		/// </summary>
		[FieldMapAttribute("VERSION", typeof(string), 100, false)]
		public string  Version;

		/// <summary>
		/// BIOS�汾
		/// </summary>
		[FieldMapAttribute("BIOS", typeof(string), 100, false)]
		public string  BIOS;

		/// <summary>
		/// PCBA�汾
		/// </summary>
		[FieldMapAttribute("PCBA", typeof(string), 100, false)]
		public string  PCBA;

		/// <summary>
		/// ���ά���û�[LastMaintainUser]
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// ���ά������[LastMaintainDate]
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ���ά��ʱ��[LastMaintainTime]
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 40, true)]
		public string  LotNO;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OBITEMCODE", typeof(string), 40, false)]
		public string  OPBOMItemCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ItemCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
		public string  RouteCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, false)]
		public string  OPCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string  ResourceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("StationCode", typeof(string), 40, true)]
		public string  StationCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, true)]
		public string  FeederCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPBOMCODE", typeof(string), 40, false)]
		public string  OPBOMCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPBOMVER", typeof(string), 40, false)]
		public string  OPBOMVersion;

	}
	#endregion

	#region CodeObject
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("NULL", "Code")]
	public class CodeObject : DomainObject
	{
		public CodeObject()
		{
		}
 
		[FieldMapAttribute("CODE", typeof(string), 40, false)]
		public string  Code;


	}
	#endregion



	#region Feeder
	/// <summary>
	/// Feeder����
	/// </summary>
	[Serializable, TableMap("tblFEEDER", "FEEDERCODE")]
	public class Feeder : DomainObject
	{
		public Feeder()
		{
		}
 
		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERTYPE", typeof(string), 40, false)]
		public string  FeederType;

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
		public string  MaintainUser;

		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, true)]
		public string  FeederCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// Feeder������
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// ���ʹ�ô���
		/// </summary>
		[FieldMapAttribute("MAXCOUNT", typeof(decimal), 10, true)]
		public decimal  MaxCount;

		/// <summary>
		/// ��ʹ�ô���
		/// </summary>
		[FieldMapAttribute("USEDCOUNT", typeof(decimal), 10, true)]
		public decimal  UsedCount;

		/// <summary>
		/// Ԥ��ʹ�ô���
		/// </summary>
		[FieldMapAttribute("ALERTCOUNT", typeof(decimal), 10, true)]
		public decimal  AlertCount;

		/// <summary>
		/// ״̬
		/// </summary>
		[FieldMapAttribute("STATUS", typeof(string), 40, false)]
		public string  Status;

		/// <summary>
		/// ״̬����ԭ��
		/// </summary>
		[FieldMapAttribute("STATUSCHGREASON", typeof(string), 40, false)]
		public string  StatusChangedReason;

		/// <summary>
		/// ״̬��������
		/// </summary>
		[FieldMapAttribute("STATUSCHGDATE", typeof(int), 8, true)]
		public int  StatusChangedDate;

		/// <summary>
		/// ״̬����ʱ��
		/// </summary>
		[FieldMapAttribute("STATUSCHGTIME", typeof(int), 6, true)]
		public int  StatusChangedTime;

		/// <summary>
		/// �˻�ԭ��
		/// </summary>
		[FieldMapAttribute("RETREASON", typeof(string), 100, false)]
		public string  ReturnReason;

		/// <summary>
		/// �Ƿ���ʹ��
		/// </summary>
		[FieldMapAttribute("USEFLAG", typeof(string), 1, true)]
		public string  UseFlag;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// �ۼ�ʹ�ô���
		/// </summary>
		[FieldMapAttribute("TOTALCOUNT", typeof(decimal), 10, true)]
		public decimal  TotalCount;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CURRUNITQTY", typeof(decimal), 15, true)]
		public decimal  CurrentUnitQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPEUSER", typeof(string), 40, false)]
		public string  OperationUser;

        //Add by Terry 2010-11-03
        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("MAXMDAY", typeof(decimal), 22, true)]
        public decimal MaxMDay;

        /// <summary>
        /// ����Ԥ������
        /// </summary>
        [FieldMapAttribute("ALTERMDAY", typeof(decimal), 22, true)]
        public decimal AlterMDay;

        // <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("MAINTAINDATE", typeof(decimal), 22, false)]
        public decimal TheMaintainDate;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [FieldMapAttribute("MAINTAINTIME", typeof(decimal), 22, false)]
        public decimal TheMaintainTime;

        // <summary>
        /// ������Ա
        /// </summary>
        [FieldMapAttribute("MAINTAINUSER", typeof(string), 40, false)]
        public string TheMaintainUser;

	}
	#endregion

	#region FeederMaintain
	/// <summary>
	/// Feederά����Ϣ
	/// </summary>
	[Serializable, TableMap("TBLFEEDERMAINTAIN", "FEEDERCODE,SEQ")]
	public class FeederMaintain : DomainObject
	{
		public FeederMaintain()
		{
		}
 
		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERTYPE", typeof(string), 40, false)]
		public string  FeederType;

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
		public string  MaintainUser;

		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, true)]
		public string  FeederCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// ״̬
		/// </summary>
		[FieldMapAttribute("STATUS", typeof(string), 40, false)]
		public string  Status;

		/// <summary>
		/// �˻�ԭ��
		/// </summary>
		[FieldMapAttribute("RETREASON", typeof(string), 100, false)]
		public string  ReturnReason;

		/// <summary>
		/// ����ԭ��
		/// </summary>
		[FieldMapAttribute("ANALYSEREASON", typeof(string), 100, false)]
		public string  AnalyseReason;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("OPERMESSAGE", typeof(string), 100, false)]
		public string  OperationMessage;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(string), 10, true)]
		public string  Seq;

		/// <summary>
		/// ά������
		/// </summary>
		[FieldMapAttribute("MAINTAINTYPE", typeof(string), 40, false)]
		public string  MaintainType;

		/// <summary>
		/// ���ϱ��
		/// </summary>
		[FieldMapAttribute("SCRAPFLAG", typeof(string), 1, true)]
		public string  ScrapFlag;

		/// <summary>
		/// ���ʹ�ô���
		/// </summary>
		[FieldMapAttribute("MAXCOUNT", typeof(decimal), 10, true)]
		public decimal  MaxCount;

		/// <summary>
		/// ��ʹ�ô���
		/// </summary>
		[FieldMapAttribute("USEDCOUNT", typeof(decimal), 10, true)]
		public decimal  UsedCount;

		/// <summary>
		/// �ۼ�ʹ�ô���
		/// </summary>
		[FieldMapAttribute("TOTALCOUNT", typeof(decimal), 10, true)]
		public decimal  TotalCount;

		/// <summary>
		/// ԭ״̬
		/// </summary>
		[FieldMapAttribute("OLDSTATUS", typeof(string), 40, false)]
		public string  OldStatus;

	}
	#endregion

	#region FeederSpec
	/// <summary>
	/// Feeder���
	/// </summary>
	[Serializable, TableMap("tblFEEDERSPEC", "FEEDERSPECCODE")]
	public class FeederSpec : DomainObject
	{
		public FeederSpec()
		{
		}
 
		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("NAME", typeof(string), 100, false)]
		public string  Name;

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
		public string  MaintainUser;

		/// <summary>
		/// Feeder������
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, true)]
		public string  FeederSpecCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

	}
	#endregion

	#region FeederStatusLog
	/// <summary>
	/// Feederʹ�ü�¼
	/// </summary>
	[Serializable, TableMap("TBLFEEDERSTATUSLOG", "FEEDERCODE,SEQ")]
	public class FeederStatusLog : DomainObject
	{
		public FeederStatusLog()
		{
		}
 
		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERTYPE", typeof(string), 40, false)]
		public string  FeederType;

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
		public string  MaintainUser;

		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, true)]
		public string  FeederCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// Feeder������
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// ״̬
		/// </summary>
		[FieldMapAttribute("STATUS", typeof(string), 40, false)]
		public string  Status;

		/// <summary>
		/// ״̬����ԭ��
		/// </summary>
		[FieldMapAttribute("STATUSCHGREASON", typeof(string), 40, false)]
		public string  StatusChangedReason;

		/// <summary>
		/// װ�и�������
		/// </summary>
		[FieldMapAttribute("STATUSCHGDATE", typeof(int), 8, true)]
		public int  StatusChangedDate;

		/// <summary>
		/// ״̬����ʱ��
		/// </summary>
		[FieldMapAttribute("STATUSCHGTIME", typeof(int), 6, true)]
		public int  StatusChangedTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
		public decimal  Seq;

		/// <summary>
		/// ����״̬
		/// </summary>
		[FieldMapAttribute("OLDSTATUS", typeof(string), 40, false)]
		public string  OldStatus;

		/// <summary>
		/// ������Ϣ
		/// </summary>
		[FieldMapAttribute("OTHERMESSAGE", typeof(string), 100, false)]
		public string  OtherMessage;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPEUSER", typeof(string), 40, false)]
		public string  OperationUser;

	}
	#endregion

	#region MachineFeeder
	/// <summary>
	/// ��̨���ϼ�¼
	/// </summary>
	[Serializable, TableMap("TBLMACHINEFEEDER", "MOCODE,MACHINECODE,MACHINESTATIONCODE,TBLGRP,SSCODE")]
	public class MachineFeeder : DomainObject
	{
		public MachineFeeder()
		{
		}
 
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
		public string  MaintainUser;

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, false)]
		public string  ProductCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// ��̨����
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, true)]
		public string  MachineCode;

		/// <summary>
		/// վλ����
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, true)]
		public string  MachineStationCode;

		/// <summary>
		/// Feeder���
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, false)]
		public string  FeederCode;

		/// <summary>
		/// �Ͼ���
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, false)]
		public string  ReelNo;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("LOADUSER", typeof(string), 40, false)]
		public string  LoadUser;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("LOADDATE", typeof(int), 8, true)]
		public int  LoadDate;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("LOADTIME", typeof(int), 6, true)]
		public int  LoadTime;

		/// <summary>
		/// �Ͼ����ϴ���
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// ��λ����
		/// </summary>
		[FieldMapAttribute("UNITQTY", typeof(decimal), 15, true)]
		public decimal  UnitQty;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 40, false)]
		public string  LotNo;

		/// <summary>
		/// DateCode
		/// </summary>
		[FieldMapAttribute("DATECODE", typeof(string), 40, false)]
		public string  DateCode;

		/// <summary>
		/// �����
		/// </summary>
		[FieldMapAttribute("CHECKRESULT", typeof(string), 1, true)]
		public string  CheckResult;

		/// <summary>
		/// ʧ��ԭ��
		/// </summary>
		[FieldMapAttribute("FAILREASON", typeof(string), 100, false)]
		public string  FailReason;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("NEXTREELNO", typeof(string), 40, false)]
		public string  NextReelNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPERESCODE", typeof(string), 40, false)]
		public string  OpeResourceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPESSCODE", typeof(string), 40, false)]
		public string  OpeStepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ENABLED", typeof(string), 1, true)]
		public string  Enabled;

		/// <summary>
		/// �Ͼ��Ƿ�ͣ��
		/// </summary>
		[FieldMapAttribute("REELCEASEFLAG", typeof(string), 1, true)]
		public string  ReelCeaseFlag;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("STATIONENABLED", typeof(string), 1, true)]
		public string  StationEnabled;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("TBLGRP", typeof(string), 40, true)]
		public string  TableGroup;

	}
	#endregion

	#region MachineFeederLog
	/// <summary>
	/// ��̨���ϼ�¼
	/// </summary>
	[Serializable, TableMap("TBLMACHINEFEEDERLOG", "LOGNO")]
	public class MachineFeederLog : DomainObject
	{
		public MachineFeederLog()
		{
		}
 
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
		public string  MaintainUser;

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, false)]
		public string  ProductCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// ��̨����
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, false)]
		public string  MachineCode;

		/// <summary>
		/// վλ����
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, false)]
		public string  MachineStationCode;

		/// <summary>
		/// Feeder���
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, false)]
		public string  FeederCode;

		/// <summary>
		/// �Ͼ���
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, false)]
		public string  ReelNo;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("LOADUSER", typeof(string), 40, false)]
		public string  LoadUser;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("LOADDATE", typeof(int), 8, true)]
		public int  LoadDate;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("LOADTIME", typeof(int), 6, true)]
		public int  LoadTime;

		/// <summary>
		/// �Ͼ����ϴ���
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// ��λ����
		/// </summary>
		[FieldMapAttribute("UNITQTY", typeof(decimal), 15, true)]
		public decimal  UnitQty;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 40, false)]
		public string  LotNo;

		/// <summary>
		/// DateCode
		/// </summary>
		[FieldMapAttribute("DATECODE", typeof(string), 40, false)]
		public string  DateCode;

		/// <summary>
		/// �����
		/// </summary>
		[FieldMapAttribute("CHECKRESULT", typeof(string), 1, true)]
		public string  CheckResult;

		/// <summary>
		/// ʧ��ԭ��
		/// </summary>
		[FieldMapAttribute("FAILREASON", typeof(string), 100, false)]
		public string  FailReason;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LOGNO", typeof(decimal), 10, true)]
		public decimal  LogNo;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OPERATIONTYPE", typeof(string), 40, false)]
		public string  OperationType;

		/// <summary>
		/// �Ͼ�ʹ������
		/// </summary>
		[FieldMapAttribute("REELUSEDQTY", typeof(decimal), 15, true)]
		public decimal  ReelUsedQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPERESCODE", typeof(string), 40, false)]
		public string  OpeResourceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPESSCODE", typeof(string), 40, false)]
		public string  OpeStepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("UNLOADUSER", typeof(string), 40, false)]
		public string  UnLoadUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("UNLOADDATE", typeof(int), 8, true)]
		public int  UnLoadDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("UNLOADTIME", typeof(int), 6, true)]
		public int  UnLoadTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("UNLOADTYPE", typeof(string), 40, false)]
		public string  UnLoadType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EXCHGFEEDERCODE", typeof(string), 40, false)]
		public string  ExchangeFeederCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EXCHGREELNO", typeof(string), 40, false)]
		public string  ExchageReelNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// �Ͼ���ϲ�������
		/// </summary>
		[FieldMapAttribute("REELCHKDIFFQTY", typeof(decimal), 15, true)]
		public decimal  ReelCheckDiffQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("STATIONENABLED", typeof(string), 1, true)]
		public string  StationEnabled;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("TBLGRP", typeof(string), 40, false)]
		public string  TableGroup;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;

	}
	#endregion

	#region Reel
	/// <summary>
	/// �Ͼ�����
	/// </summary>
	[Serializable, TableMap("TBLREEL", "REELNO")]
	public class Reel : DomainObject
	{
		public Reel()
		{
		}
 
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("QTY", typeof(decimal), 15, true)]
		public decimal  Qty;

		/// <summary>
		/// ���ϴ���
		/// </summary>
		[FieldMapAttribute("PARTNO", typeof(string), 40, false)]
		public string  PartNo;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 100, false)]
		public string  LotNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATECODE", typeof(string), 40, false)]
		public string  DateCode;

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
		public string  MaintainUser;

		/// <summary>
		/// �Ͼ����
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, true)]
		public string  ReelNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ��ʹ������
		/// </summary>
		[FieldMapAttribute("USEDQTY", typeof(decimal), 15, true)]
		public decimal  UsedQty;

		/// <summary>
		/// �Ƿ�����
		/// </summary>
		[FieldMapAttribute("USEDFLAG", typeof(string), 1, true)]
		public string  UsedFlag;

		/// <summary>
		/// ���õĹ�������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// ���õĲ��ߴ���
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// �Ƿ��ز�
		/// </summary>
		[FieldMapAttribute("ISSPECIAL", typeof(string), 1, true)]
		public string  IsSpecial;

		/// <summary>
		/// ��ע
		/// </summary>
		[FieldMapAttribute("MEMO", typeof(string), 100, false)]
		public string  Memo;

		/// <summary>
		/// ���ϲ���
		/// </summary>
		[FieldMapAttribute("CHECKDIFFQTY", typeof(decimal), 15, true)]
		public decimal  CheckDiffQty;

	}
	#endregion

	#region ReelCheckedLog
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLREELCHKLOG", "CheckID")]
	public class ReelCheckedLog : DomainObject
	{
		public ReelCheckedLog()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CheckID", typeof(decimal), 10, true)]
		public decimal  CheckID;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// �Ͼ���
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, false)]
		public string  ReelNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHECKUSER", typeof(string), 40, false)]
		public string  CheckUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHECKDATE", typeof(int), 8, true)]
		public int  CheckDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHECKTIME", typeof(int), 6, true)]
		public int  CheckTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
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
		[FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// ���ϴ���
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// �Ͼ�������
		/// </summary>
		[FieldMapAttribute("REELQTY", typeof(decimal), 15, true)]
		public decimal  ReelQty;

		/// <summary>
		/// �Ͼ����ʱ��ʣ������
		/// </summary>
		[FieldMapAttribute("REELLEFTQTY", typeof(decimal), 15, true)]
		public decimal  ReelLeftQty;

		/// <summary>
		/// �Ͼ�ʵ������
		/// </summary>
		[FieldMapAttribute("REELACTQTY", typeof(decimal), 15, true)]
		public decimal  ReelActualQty;

		/// <summary>
		/// �Ƿ�������
		/// </summary>
		[FieldMapAttribute("ISCHECKED", typeof(string), 1, true)]
		public string  IsChecked;

		/// <summary>
		/// �Ͼ�����ʱ������
		/// </summary>
		[FieldMapAttribute("REELCURRQTY", typeof(decimal), 15, true)]
		public decimal  ReelCurrentQty;

		/// <summary>
		/// �Ƿ��ز�
		/// </summary>
		[FieldMapAttribute("ISSPECIAL", typeof(string), 1, true)]
		public string  IsSpecial;

		/// <summary>
		/// �زɱ�ע
		/// </summary>
		[FieldMapAttribute("MEMO", typeof(string), 100, false)]
		public string  Memo;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("GETOUTUSER", typeof(string), 40, false)]
		public string  GetOutUser;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("GETOUTDATE", typeof(int), 8, true)]
		public int  GetOutDate;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("GETOUTTIME", typeof(int), 6, true)]
		public int  GetOutTime;

	}
	#endregion

	#region ReelQty
	/// <summary>
	/// Feeder����
	/// </summary>
	[Serializable, TableMap("TBLREELQTY", "REELNO,MOCODE")]
	public class ReelQty : DomainObject
	{
		public ReelQty()
		{
		}
 
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
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// �Ͼ���
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, true)]
		public string  ReelNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// ��̨����
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, false)]
		public string  MachineCode;

		/// <summary>
		/// վλ����
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, false)]
		public string  MachineStationCode;

		/// <summary>
		/// Feeder���
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, false)]
		public string  FeederCode;

		/// <summary>
		/// ԭ������
		/// </summary>
		[FieldMapAttribute("QTY", typeof(decimal), 15, true)]
		public decimal  Qty;

		/// <summary>
		/// ʹ������
		/// </summary>
		[FieldMapAttribute("USEDQTY", typeof(decimal), 15, true)]
		public decimal  UsedQty;

		/// <summary>
		/// �Ѹ�������
		/// </summary>
		[FieldMapAttribute("UPDATEDQTY", typeof(decimal), 15, true)]
		public decimal  UpdatedQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ALERTQTY", typeof(decimal), 15, true)]
		public decimal  AlertQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("UNITQTY", typeof(decimal), 15, true)]
		public decimal  UnitQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MaterialCode", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

	}
	#endregion

	#region ReelValidity
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLREELVALIDITY", "MATERIALPREFIX")]
	public class ReelValidity : DomainObject
	{
		public ReelValidity()
		{
		}
 
		/// <summary>
		/// ���ϴ���ǰ׺
		/// </summary>
		[FieldMapAttribute("MATERIALPREFIX", typeof(string), 40, true)]
		public string  MaterialPrefix;

		/// <summary>
		/// ��Ч��(��)
		/// </summary>
		[FieldMapAttribute("VALIDITYMONTH", typeof(decimal), 15, true)]
		public decimal  ValidityMonth;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
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
		[FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
		public string  EAttribute1;

	}
	#endregion

	#region SMTAlert
	/// <summary>
	/// SMTԤ��
	/// </summary>
	[Serializable, TableMap("TBLSMTALERT", "ALERTSEQ")]
	public class SMTAlert : DomainObject
	{
		public SMTAlert()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ALERTSEQ", typeof(decimal), 10, true)]
		public decimal  AlertSeq;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ALERTTYPE", typeof(string), 40, false)]
		public string  AlertType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, false)]
		public string  ProductCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, false)]
		public string  MachineCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, false)]
		public string  MachineStationCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, false)]
		public string  FeederCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FEEDERMAXCOUNT", typeof(decimal), 10, true)]
		public decimal  FeederMaxCount;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FEEDERALERTCOUNT", typeof(decimal), 10, true)]
		public decimal  FeederAlertCount;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FEEDERUSEDCOUNT", typeof(decimal), 10, true)]
		public decimal  FeederUsedCount;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, false)]
		public string  ReelNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("REELQTY", typeof(decimal), 15, true)]
		public decimal  ReelQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("REELUSEDQTY", typeof(decimal), 15, true)]
		public decimal  ReelUsedQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ALERTDATE", typeof(int), 8, true)]
		public int  AlertDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ALERTTIME", typeof(int), 6, true)]
		public int  AlertTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ALERTSTATUS", typeof(string), 40, false)]
		public string  AlertStatus;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ALERTLEVEL", typeof(string), 40, false)]
		public string  AlertLevel;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MAINTAINUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MAINTAINDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MAINTAINTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

	}
	#endregion

	#region SMTCheckMaterial
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("tblSMTCheckMaterial", "CheckID")]
	public class SMTCheckMaterial : DomainObject
	{
		public SMTCheckMaterial()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CheckID", typeof(decimal), 10, true)]
		public decimal  CheckID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHECKRESULT", typeof(string), 40, false)]
		public string  CheckResult;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHECKUSER", typeof(string), 40, false)]
		public string  CheckUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHECKDATE", typeof(int), 8, true)]
		public int  CheckDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHECKTIME", typeof(int), 6, true)]
		public int  CheckTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
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
		[FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
		public string  ItemCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

	}
	#endregion

	#region SMTCheckMaterialDetail
	/// <summary>
	/// SMT Feeder����
	/// </summary>
	[Serializable, TableMap("tblSMTCheckMaterialDtl", "CheckID,SEQ")]
	public class SMTCheckMaterialDetail : DomainObject
	{
		public SMTCheckMaterialDetail()
		{
		}
 
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, false)]
		public string  ProductCode;

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
		public string  MaintainUser;

		/// <summary>
		/// ��̨����
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, false)]
		public string  MachineCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ��ѡ��
		/// </summary>
		[FieldMapAttribute("SOURCEMATERIALCODE", typeof(string), 40, false)]
		public string  SourceMaterialCode;

		/// <summary>
		/// ���ϴ���
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// Feeder������
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// SMT����
		/// </summary>
		[FieldMapAttribute("SMTQTY", typeof(decimal), 15, true)]
		public decimal  SMTQty;

		/// <summary>
		/// վλ����
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, false)]
		public string  MachineStationCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CheckID", typeof(decimal), 10, true)]
		public decimal  CheckID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
		public decimal  Sequence;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MATERIALNAME", typeof(string), 100, false)]
		public string  MaterialName;

		/// <summary>
		/// ����BOM��λ����
		/// </summary>
		[FieldMapAttribute("BOMQTY", typeof(decimal), 15, true)]
		public decimal  BOMQty;

		/// <summary>
		/// �ȶԽ��
		/// </summary>
		[FieldMapAttribute("CHECKRESULT", typeof(string), 1, true)]
		public string  CheckResult;

		/// <summary>
		/// �ȶ�����
		/// </summary>
		[FieldMapAttribute("CHECKDESC", typeof(string), 100, false)]
		public string  CheckDescription;

		/// <summary>
		/// ��λ
		/// </summary>
		[FieldMapAttribute("BOMUOM", typeof(string), 100, false)]
		public string  BOMUOM;

		/// <summary>
		/// SMT/MOBOM
		/// </summary>
		[FieldMapAttribute("TYPE", typeof(string), 40, false)]
		public string  Type;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

	}
	#endregion

	#region SMTFeederMaterial
	/// <summary>
	/// SMT Feeder����
	/// </summary>
	[Serializable, TableMap("TBLSMTFEEDERMATERIAL", "PRODUCTCODE,MACHINECODE,MATERIALCODE,MACHINESTATIONCODE,SSCODE,TBLGRP")]
	public class SMTFeederMaterial : DomainObject
	{
		public SMTFeederMaterial()
		{
		}
 
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, true)]
		public string  ProductCode;

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
		public string  MaintainUser;

		/// <summary>
		/// ��̨����
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, true)]
		public string  MachineCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

        /// <summary>
        /// add by kathy @20130814 ����ҳ��������λ����¼������Ϣ�Ƿ�Ϸ�
        /// </summary>
        public string EAttribute2;

		/// <summary>
		/// ��ѡ��
		/// </summary>
		[FieldMapAttribute("SOURCEMATERIALCODE", typeof(string), 40, false)]
		public string  SourceMaterialCode;

		/// <summary>
		/// ���ϴ���
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, true)]
		public string  MaterialCode;

		/// <summary>
		/// Feeder������
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("QTY", typeof(decimal), 15, true)]
		public decimal  Qty;

		/// <summary>
		/// վλ����
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, true)]
		public string  MachineStationCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("TBLGRP", typeof(string), 40, true)]
		public string  TableGroup;

	}
	#endregion

	#region SMTFeederMaterialImportLog
	/// <summary>
	/// SMT Feeder����
	/// </summary>
	[Serializable, TableMap("TBLSMTFEEDERMATERIALIMPLOG", "LOGNO,SEQ")]
	public class SMTFeederMaterialImportLog : DomainObject
	{
		public SMTFeederMaterialImportLog()
		{
		}
 
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, false)]
		public string  ProductCode;

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
		public string  MaintainUser;

		/// <summary>
		/// ��̨����
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, false)]
		public string  MachineCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ��ѡ��
		/// </summary>
		[FieldMapAttribute("SOURCEMATERIALCODE", typeof(string), 40, false)]
		public string  SourceMaterialCode;

		/// <summary>
		/// ���ϴ���
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// Feeder������
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("QTY", typeof(decimal), 15, true)]
		public decimal  Qty;

		/// <summary>
		/// վλ����
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, false)]
		public string  MachineStationCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LOGNO", typeof(decimal), 10, true)]
		public decimal  LOGNO;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
		public decimal  Sequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("IMPUSER", typeof(string), 40, false)]
		public string  ImportUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("IMPDATE", typeof(int), 8, true)]
		public int  ImportDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("IMPTIME", typeof(int), 6, true)]
		public int  ImportTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHECKRESULT", typeof(string), 1, true)]
		public string  CheckResult;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHECKDESC", typeof(string), 100, false)]
		public string  CheckDescription;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("TBLGRP", typeof(string), 40, false)]
		public string  TableGroup;

	}
	#endregion

	#region SMTLineControlLog
	/// <summary>
	/// ��̨���ϼ�¼
	/// </summary>
	[Serializable, TableMap("TBLSMTLINECTLLOG", "LOGID")]
	public class SMTLineControlLog : DomainObject
	{
		public SMTLineControlLog()
		{
		}
 
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
		public string  MaintainUser;

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, false)]
		public string  ProductCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// ��̨����
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, false)]
		public string  MachineCode;

		/// <summary>
		/// վλ����
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, false)]
		public string  MachineStationCode;

		/// <summary>
		/// Feeder���
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, false)]
		public string  FeederCode;

		/// <summary>
		/// �Ͼ���
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, false)]
		public string  ReelNo;

		/// <summary>
		/// �Ͼ����ϴ���
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// ��λ����
		/// </summary>
		[FieldMapAttribute("UNITQTY", typeof(decimal), 15, true)]
		public decimal  UnitQty;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("REELQTY", typeof(decimal), 15, true)]
		public decimal  ReelQty;

		/// <summary>
		/// DateCode
		/// </summary>
		[FieldMapAttribute("REELUSEDQTY", typeof(decimal), 15, true)]
		public decimal  ReelUsedQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("NEXTREELNO", typeof(string), 40, false)]
		public string  NextReelNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPERESCODE", typeof(string), 40, false)]
		public string  OpeResourceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPESSCODE", typeof(string), 40, false)]
		public string  OpeStepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// �����Ƿ����
		/// </summary>
		[FieldMapAttribute("ENABLED", typeof(string), 1, true)]
		public string  Enabled;

		/// <summary>
		/// �Ͼ��Ƿ�ͣ��
		/// </summary>
		[FieldMapAttribute("REELCEASEFLAG", typeof(string), 1, true)]
		public string  ReelCeaseFlag;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LOGID", typeof(decimal), 10, true)]
		public decimal  LogID;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OPERATIONTYPE", typeof(string), 40, false)]
		public string  OperationType;

		/// <summary>
		/// ����ԭ��״̬
		/// </summary>
		[FieldMapAttribute("LINESTATUSOLD", typeof(string), 1, true)]
		public string  LineStatusOld;

		/// <summary>
		/// ��������״̬
		/// </summary>
		[FieldMapAttribute("LINESTATUS", typeof(string), 1, true)]
		public string  LineStatus;

		/// <summary>
		/// �Ƿ��޸Ĳ���״̬
		/// </summary>
		[FieldMapAttribute("CHGLINESTATUS", typeof(string), 1, true)]
		public string  ChangeLineStatus;

	}
	#endregion

	#region SMTMachineActiveInno
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSMTMACHINEACTIVEINNO", "MOCODE,SSCODE,MACHINECODE")]
	public class SMTMachineActiveInno : DomainObject
	{
		public SMTMachineActiveInno()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("INNO", typeof(decimal), 10, true)]
		public decimal  INNO;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, true)]
		public string  MachineCode;

	}
	#endregion

	#region SMTMachineDiscard
	/// <summary>
	/// �豸���ϵ���
	/// </summary>
	[Serializable, TableMap("TBLSMTMACHINEDISCARD", "MOCODE,SSCODE,MATERIALCODE,MACHINESTATIONCODE")]
	public class SMTMachineDiscard : DomainObject
	{
		public SMTMachineDiscard()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, true)]
		public string  MaterialCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, true)]
		public string  MachineStationCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PICKUPCOUNT", typeof(decimal), 15, true)]
		public decimal  PickupCount;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("REJECTPARTS", typeof(decimal), 15, true)]
		public decimal  RejectParts;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("NOPICKUP", typeof(decimal), 15, true)]
		public decimal  NoPickup;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ERRORPARTS", typeof(decimal), 15, true)]
		public decimal  ErrorParts;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DISLODGEDPARTS", typeof(decimal), 15, true)]
		public decimal  DislodgedParts;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
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
		[FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
		public string  EAttribute1;

	}
	#endregion

	#region SMTMachineInno
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSMTMACHINEINNO", "INNO,INNOSEQ")]
	public class SMTMachineInno : DomainObject
	{
		public SMTMachineInno()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

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
		public string  MaintainUser;

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, false)]
		public string  ProductCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// ��̨����
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, false)]
		public string  MachineCode;

		/// <summary>
		/// վλ����
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, false)]
		public string  MachineStationCode;

		/// <summary>
		/// Feeder���
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// Feeder����
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, false)]
		public string  FeederCode;

		/// <summary>
		/// �Ͼ���
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, false)]
		public string  ReelNo;

		/// <summary>
		/// �Ͼ����ϴ���
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// ��λ����
		/// </summary>
		[FieldMapAttribute("UNITQTY", typeof(decimal), 15, true)]
		public decimal  UnitQty;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 40, false)]
		public string  LotNo;

		/// <summary>
		/// DateCode
		/// </summary>
		[FieldMapAttribute("DATECODE", typeof(string), 40, false)]
		public string  DateCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("INNO", typeof(decimal), 10, true)]
		public decimal  INNO;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("INNOSEQ", typeof(decimal), 10, true)]
		public decimal  INNOSequence;

	}
	#endregion

	#region SMTRCardInno
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSMTRCARDINNO", "RCARD,RCARDSEQ,INNO")]
	public class SMTRCardInno : DomainObject
	{
		public SMTRCardInno()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, true)]
		public string  RunningCard;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
		public decimal  RunningCardSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("INNO", typeof(decimal), 10, true)]
		public decimal  INNO;

	}
	#endregion

	#region SMTRCardMaterial
	/// <summary>
	/// SMT��RCard����
	/// </summary>
	[Serializable, TableMap("TBLSMTRCARDMATERIAL", "RCARD,RCARDSEQ")]
	public class SMTRCardMaterial : DomainObject
	{
		public SMTRCardMaterial()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, true)]
		public string  RunningCard;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RCARDSEQ", typeof(decimal), 15, true)]
		public decimal  RunningCardSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
		public string  ItemCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, false)]
		public string  ModelCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
		public string  SegmentCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, false)]
		public string  ResourceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
		public string  RouteCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, false)]
		public string  OPCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, false)]
		public string  ShiftTypeCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SHIFTCODE", typeof(string), 40, false)]
		public string  ShiftCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("TPCODE", typeof(string), 40, false)]
		public string  TimePeriodCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PRODUCTSTATUS", typeof(string), 40, false)]
		public string  ProductStatus;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
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
		[FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
		public string  EAttribute1;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;

	}
	#endregion

	#region SMTRCardMaterialDetail
	/// <summary>
	/// SMT��RCard����
	/// </summary>
	[Serializable, TableMap("TBLSMTRCARDMATERIALDTL", "RCARD,RCARDSEQ,REELSEQ")]
	public class SMTRCardMaterialDetail : DomainObject
	{
		public SMTRCardMaterialDetail()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, true)]
		public string  RunningCard;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RCARDSEQ", typeof(decimal), 15, true)]
		public decimal  RunningCardSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
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
		[FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("REELSEQ", typeof(decimal), 10, true)]
		public decimal  ReelSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MACHINECODE", typeof(string), 40, false)]
		public string  MachineCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MACHINESTATIONCODE", typeof(string), 40, false)]
		public string  MachineStationCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FEEDERSPECCODE", typeof(string), 40, false)]
		public string  FeederSpecCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, false)]
		public string  FeederCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, false)]
		public string  ReelNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("UNITQTY", typeof(decimal), 15, true)]
		public decimal  UnitQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 40, false)]
		public string  LotNo;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATECODE", typeof(string), 40, false)]
		public string  DateCode;

	}
	#endregion

	#region SMTSensorQty
	/// <summary>
	/// SMTԤ��
	/// </summary>
	[Serializable, TableMap("TBLSMTSENSORQTY", "PRODUCTCODE,MOCODE,SSCODE,SHIFTDAY,SHIFTTYPECODE,SHIFTCODE,TPCODE")]
	public class SMTSensorQty : DomainObject
	{
		public SMTSensorQty()
		{
		}
 
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, true)]
		public string  ProductCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// �ۼ�����
		/// </summary>
		[FieldMapAttribute("QTY", typeof(decimal), 15, true)]
		public decimal  Qty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MAINTAINUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MAINTAINDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MAINTAINTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ���ߴ���
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string  StepSequenceCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("SHIFTDAY", typeof(int), 8, true)]
		public int  ShiftDay;

		/// <summary>
		/// ���ƴ���
		/// </summary>
		[FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
		public string  ShiftTypeCode;

		/// <summary>
		/// ��δ���
		/// </summary>
		[FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
		public string  ShiftCode;

		/// <summary>
		/// ʱ�δ���
		/// </summary>
		[FieldMapAttribute("TPCODE", typeof(string), 40, true)]
		public string  TPCode;

		/// <summary>
		/// ʱ�ο�ʼ����
		/// </summary>
		[FieldMapAttribute("TPBTIME", typeof(int), 6, true)]
		public int  TPBeginTime;

		/// <summary>
		/// ʱ�ν�������
		/// </summary>
		[FieldMapAttribute("TPETIME", typeof(int), 6, true)]
		public int  TPEndTime;

		/// <summary>
		/// ʱ�δ���
		/// </summary>
		[FieldMapAttribute("TPSEQ", typeof(decimal), 10, true)]
		public decimal  TPSequence;

		/// <summary>
		/// ������ʼ��������
		/// </summary>
		[FieldMapAttribute("MOBDATE", typeof(int), 8, true)]
		public int  MOBeginDate;

		/// <summary>
		/// ������ʼ����ʱ��
		/// </summary>
		[FieldMapAttribute("MOBTIME", typeof(int), 6, true)]
		public int  MOBeginTime;

		/// <summary>
		/// ����������������
		/// </summary>
		[FieldMapAttribute("MOEDATE", typeof(int), 8, true)]
		public int  MOEndDate;

		/// <summary>
		/// ������������ʱ��
		/// </summary>
		[FieldMapAttribute("MOETIME", typeof(int), 6, true)]
		public int  MOEndTime;

		/// <summary>
		/// ��������ԭ��
		/// </summary>
		[FieldMapAttribute("DIFFREASON", typeof(string), 100, false)]
		public string  DifferenceReason;

		/// <summary>
		/// ��������ά����Ա
		/// </summary>
		[FieldMapAttribute("DIFFMUSER", typeof(string), 40, false)]
		public string  DifferenceMaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DIFFMDATE", typeof(int), 8, true)]
		public int  DifferenceMaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DIFFMTIME", typeof(int), 6, true)]
		public int  DifferenceMaintainTime;

	}
	#endregion

	#region SMTTargetQty
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSMTTARGETQTY", "MOCODE,SSCODE,TPCODE")]
	public class SMTTargetQty : DomainObject
	{
		public SMTTargetQty()
		{
		}
 
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// ���ߴ���
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string  SSCode;

		/// <summary>
		/// ʱ�δ���
		/// </summary>
		[FieldMapAttribute("TPCODE", typeof(string), 40, true)]
		public string  TPCode;

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("PRODUCTCODE", typeof(string), 40, false)]
		public string  ProductCode;

		/// <summary>
		/// ���ƴ���
		/// </summary>
		[FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, false)]
		public string  ShiftTypeCode;

		/// <summary>
		/// ��δ���
		/// </summary>
		[FieldMapAttribute("SHIFTCODE", typeof(string), 40, false)]
		public string  ShfitCode;

		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string  SegmentCode;

		/// <summary>
		/// ʱ�ο�ʼʱ��
		/// </summary>
		[FieldMapAttribute("TPBTIME", typeof(int), 6, true)]
		public int  TPBeginTime;

		/// <summary>
		/// ʱ�ν���ʱ��
		/// </summary>
		[FieldMapAttribute("TPETIME", typeof(int), 6, true)]
		public int  TPEndTime;

		/// <summary>
		/// ʱ�δ���
		/// </summary>
		[FieldMapAttribute("TPSEQ", typeof(decimal), 10, true)]
		public decimal  TPSequence;

		/// <summary>
		/// ʱ������
		/// </summary>
		[FieldMapAttribute("TPDESC", typeof(string), 100, false)]
		public string  TPDescription;

		/// <summary>
		/// ʱ��Ŀ�����
		/// </summary>
		[FieldMapAttribute("TPQTY", typeof(decimal), 15, true)]
		public decimal  TPQty;

		/// <summary>
		/// ÿСʱ����
		/// </summary>
		[FieldMapAttribute("QTYPERHOUR", typeof(decimal), 15, true)]
		public decimal  QtyPerHour;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
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
		[FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
		public string  EAttribute1;

	}
	#endregion

    #region Smtrelationqty
    /// <summary>
    /// TBLSMTRELATIONQTY
    /// </summary>
    [Serializable, TableMap("TBLSMTRELATIONQTY", "RCARD,MOCODE")]
    public class Smtrelationqty : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Smtrelationqty()
        {
        }

        ///<summary>
        ///RCARD
        ///</summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, false)]
        public string Rcard;

        ///<summary>
        ///MOCODE
        ///</summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string Mocode;

        ///<summary>
        ///ITEMCODE
        ///</summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string Itemcode;

        ///<summary>
        ///RELATIONQTRY
        ///</summary>
        [FieldMapAttribute("RELATIONQTRY", typeof(int), 22, false)]
        public int Relationqtry;

        ///<summary>
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string Muser;

        ///<summary>
        ///MDATE
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int Mdate;

        ///<summary>
        ///MTIME
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int Mtime;

        ///<summary>
        ///EATTRIBUTE1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;

        ///<summary>
        ///EATTRIBUTE2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string Eattribute2;

        ///<summary>
        ///EATTRIBUTE3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string Eattribute3;

        ///<summary>
        ///MEMO
        ///</summary>
        [FieldMapAttribute("MEMO", typeof(string), 100, true)]
        public string Memo;

    }
    #endregion

    #region Splitboard
    /// <summary>
    /// TBLSPLITBOARD
    /// </summary>
    [Serializable, TableMap("TBLSPLITBOARD", "SEQ,MOCODE,RCARD")]
    public class Splitboard : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Splitboard()
        {
        }

        ///<summary>
        ///RCARD
        ///</summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, false)]
        public string Rcard;

        ///<summary>
        ///MOCODE
        ///</summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string Mocode;

        ///<summary>
        ///SEQ
        ///</summary>
        [FieldMapAttribute("SEQ", typeof(int), 22, false)]
        public int Seq;

        ///<summary>
        ///SCARD
        ///</summary>
        [FieldMapAttribute("SCARD", typeof(string), 40, false)]
        public string Scard;

        ///<summary>
        ///MODELCODE
        ///</summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, false)]
        public string Modelcode;

        ///<summary>
        ///ITEMCODE
        ///</summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string Itemcode;

        ///<summary>
        ///ROUTECODE
        ///</summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string Routecode;

        ///<summary>
        ///OPCODE
        ///</summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string Opcode;

        ///<summary>
        ///SEGCODE
        ///</summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string Segcode;

        ///<summary>
        ///SSCODE
        ///</summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string Sscode;

        ///<summary>
        ///RESCODE
        ///</summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string Rescode;

        ///<summary>
        ///SHIFTTYPECODE
        ///</summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, false)]
        public string Shifttypecode;

        ///<summary>
        ///SHIFTCODE
        ///</summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, false)]
        public string Shiftcode;

        ///<summary>
        ///TPCODE
        ///</summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, false)]
        public string Tpcode;

        ///<summary>
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(int), 22, false)]
        public int Muser;

        ///<summary>
        ///MTIME
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int Mtime;

        ///<summary>
        ///EATTRIBUTE1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;

    }
    #endregion


}

