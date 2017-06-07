using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for SolderPaste
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
/// ** �� ��:		2006-7-17 10:24:39
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.SolderPaste
{

	#region SolderPaste
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSOLDERPASTE", "SPID")]
	public class SolderPaste : DomainObject
	{
		public SolderPaste()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SPID", typeof(string), 40, true)]
		public string  SolderPasteID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 40, true)]
		public string  LotNO;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PRODATE", typeof(int), 8, true)]
		public int  ProductionDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EXDATE", typeof(int), 8, true)]
		public int  ExpiringDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("Status", typeof(string), 40, true)]
		public string  Status;

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
		[FieldMapAttribute("eAttribute1", typeof(string), 40, false)]
		public string  eAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARTNO", typeof(string), 40, true)]
		public string  PartNO;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("USED", typeof(string), 1, true)]
		public string  Used;

	}
	#endregion

	#region SolderPaste2Item
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSOLDERPASTE2ITEM", "ITEMCODE")]
	public class SolderPaste2Item : DomainObject
	{
		public SolderPaste2Item()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ItemCode;

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
		[FieldMapAttribute("eAttribute1", typeof(string), 40, false)]
		public string  eAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SPTYPE", typeof(string), 40, true)]
		public string  SolderPasteType;

	}
	#endregion

	#region SolderPasteControl
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSOLDERPASTECONTROL", "PARTNO")]
	public class SolderPasteControl : DomainObject
	{
		public SolderPasteControl()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARTNO", typeof(string), 40, true)]
		public string  PartNO;

		/// <summary>
		/// [���ͣ���Ǧ����Ǧ�����¡�����]
		/// </summary>
		[FieldMapAttribute("TYPE", typeof(string), 40, true)]
		public string  Type;

		/// <summary>
		/// [����ʱ��]
		/// </summary>
		[FieldMapAttribute("OPENTS", typeof(decimal), 15, true)]
		public decimal  OpenTimeSpan;

		/// <summary>
		/// [δ����ʱ��]
		/// </summary>
		[FieldMapAttribute("UNOPENTS", typeof(decimal), 15, true)]
		public decimal  UnOpenTimeSpan;

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
		[FieldMapAttribute("eAttribute1", typeof(string), 40, false)]
		public string  eAttribute1;

		/// <summary>
		/// [����ʱ��]
		/// </summary>
		[FieldMapAttribute("ReturnTimeSpan", typeof(decimal), 15, true)]
		public decimal  ReturnTimeSpan;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("GUARANTEEPERIOD", typeof(int), 10, true)]
		public int  GuaranteePeriod;

	}
	#endregion

	#region SOLDERPASTEPRO
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLSOLDERPASTEPRO", "SPPKID")]
	public class SOLDERPASTEPRO : DomainObject
	{
		public SOLDERPASTEPRO()
		{
		}
 
		/// <summary>
		/// ʹ�ô���
		/// </summary>
		[FieldMapAttribute("SEQUENCE", typeof(int), 6, true)]
		public int  SEQUENCE;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("OPENUSER", typeof(string), 40, false)]
		public string  OPENUSER;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("OPENTIME", typeof(int), 6, false)]
		public int  OPENTIME;

		/// <summary>
		/// �ش���Ա
		/// </summary>
		[FieldMapAttribute("RESAVEUSER", typeof(string), 40, false)]
		public string  RESAVEUSER;

		/// <summary>
		/// �ش�����
		/// </summary>
		[FieldMapAttribute("RESAVEDATE", typeof(int), 8, false)]
		public int  RESAVEDATE;

		/// <summary>
		/// ����ID
		/// </summary>
		[FieldMapAttribute("SOLDERPASTEID", typeof(string), 40, false)]
		public string  SOLDERPASTEID;

		/// <summary>
		/// ��ע
		/// </summary>
		[FieldMapAttribute("EATTRIBUTE1", typeof(string), 100, false)]
		public string  EATTRIBUTE1;

		/// <summary>
		/// ά���û�
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
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
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("UNAVIALUSER", typeof(string), 40, false)]
		public string  UNAVIALUSER;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("UNAVIALDATE", typeof(int), 8, false)]
		public int  UNAVIALDATE;

		/// <summary>
		/// �ش�ʱ��
		/// </summary>
		[FieldMapAttribute("RESAVETIME", typeof(int), 6, false)]
		public int  RESAVETIME;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("UNAVIALTIME", typeof(int), 6, false)]
		public int  UNAVIALTIME;

		/// <summary>
		/// ���ߴ���
		/// </summary>
		[FieldMapAttribute("LINECODE", typeof(string), 40, false)]
		public string  LINECODE;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCODE;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OPENDATE", typeof(int), 8, false)]
		public int  OPENDATE;

		/// <summary>
		/// ״̬��
		/// ����/����ʹ��/����/����/�ش�
		/// </summary>
		[FieldMapAttribute("STATUS", typeof(string), 40, false)]
		public string  STATUS;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("RETURNTIMESPAN", typeof(decimal), 10, false)]
		public decimal  RETURNTIMESPAN;

		/// <summary>
		/// ȡ��δ����ʱ��
		/// </summary>
		[FieldMapAttribute("VEILTIMESPAN", typeof(decimal), 10, false)]
		public decimal  VEILTIMESPAN;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("UNVEILTIMESPAN", typeof(decimal), 10, false)]
		public decimal  UNVEILTIMESPAN;

		/// <summary>
		/// ʵЧ����
		/// </summary>
		[FieldMapAttribute("EXPIREDDATE", typeof(int), 8, true)]
		public int  EXPIREDDATE;

		/// <summary>
		/// ��ע
		/// </summary>
		[FieldMapAttribute("MEMO", typeof(string), 100, false)]
		public string  MEMO;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("AGITAEUSER", typeof(string), 40, false)]
		public string  AGITAEUSER;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("AGITATEDATE", typeof(int), 8, false)]
		public int  AGITATEDATE;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("AGITATETIME", typeof(int), 6, false)]
		public int  AGITATETIME;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("UNVEILUSER", typeof(string), 40, false)]
		public string  UNVEILUSER;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("UNVEILMDATE", typeof(int), 8, false)]
		public int  UNVEILMDATE;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("UNVEILTIME", typeof(int), 6, false)]
		public int  UNVEILTIME;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("SPTYPE", typeof(string), 40, false)]
		public string  SPTYPE;

		/// <summary>
		/// �Ϻ�
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 40, true)]
		public string  LOTNO;

		/// <summary>
		/// ���¼�ʱ
		/// </summary>
		[FieldMapAttribute("RETURNCOUNTTIME", typeof(decimal), 15, false)]
		public decimal  RETURNCOUNTTIME;

		/// <summary>
		/// �����ʱ
		/// </summary>
		[FieldMapAttribute("UNVEILCOUNTTIME", typeof(decimal), 15, false)]
		public decimal  UNVEILCOUNTTIME;

		/// <summary>
		/// δ�����ʱ
		/// </summary>
		[FieldMapAttribute("VEILCOUNTTIME", typeof(decimal), 15, false)]
		public decimal  VEILCOUNTTIME;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("SPPKID", typeof(string), 40, true)]
		public string  SPPKID;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("RETRUNUSER", typeof(string), 40, false)]
		public string  RETRUNUSER;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("RETURNDATE", typeof(int), 8, false)]
		public int  RETURNDATE;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("RETURNTIME", typeof(int), 6, false)]
		public int  RETURNTIME;

	}
	#endregion

}

