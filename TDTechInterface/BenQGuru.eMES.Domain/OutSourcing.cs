using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for OutSourcing
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2006-7-21 10:29:02
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.OutSourcing
{

	#region OutMO
	/// <summary>
	/// �����������
	/// </summary>
	[Serializable, TableMap("TBLOUTMO", "RndID")]
	public class OutMO : DomainObject
	{
		public OutMO()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RndID", typeof(string), 40, true)]
		public string  RndID;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// �Ϻ�
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
		public string  ItemCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("QTY", typeof(decimal), 15, true)]
		public decimal  Qty;

		/// <summary>
		/// �����
		/// </summary>
		[FieldMapAttribute("OUTFACTORY", typeof(string), 40, false)]
		public string  OutFactory;

		/// <summary>
		/// �깤����
		/// </summary>
		[FieldMapAttribute("COMPLETEDATE", typeof(int), 8, true)]
		public int  CompleteDate;

		/// <summary>
		/// ��ʼ���к�
		/// </summary>
		[FieldMapAttribute("STARTSN", typeof(string), 40, false)]
		public string  StartSN;

		/// <summary>
		/// �������к�
		/// </summary>
		[FieldMapAttribute("ENDSN", typeof(string), 40, false)]
		public string  EndSN;

		/// <summary>
		/// ��ע
		/// </summary>
		[FieldMapAttribute("MEMO", typeof(string), 100, false)]
		public string  Memo;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("IMPORTUSER", typeof(string), 40, false)]
		public string  ImportUser;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("IMPORTDATE", typeof(int), 8, true)]
		public int  ImportDate;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("IMPORTTIME", typeof(int), 6, true)]
		public int  ImportTime;

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
		/// ���ͣ�LOT��PCS
		/// </summary>
		[FieldMapAttribute("TYPE", typeof(string), 40, false)]
		public string  Type;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PLANQTY", typeof(decimal), 15, true)]
		public decimal  PlanQty;

	}
	#endregion

	#region OutWIP
	/// <summary>
	/// �����������
	/// </summary>
	[Serializable, TableMap("TBLOUTWIP", "RndID")]
	public class OutWIP : DomainObject
	{
		public OutWIP()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RndID", typeof(string), 40, true)]
		public string  RndID;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// �������к�
		/// </summary>
		[FieldMapAttribute("ENDSN", typeof(string), 40, false)]
		public string  EndSN;

		/// <summary>
		/// ��ע
		/// </summary>
		[FieldMapAttribute("MEMO", typeof(string), 100, false)]
		public string  Memo;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("IMPORTUSER", typeof(string), 40, false)]
		public string  ImportUser;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("IMPORTDATE", typeof(int), 8, true)]
		public int  ImportDate;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("IMPORTTIME", typeof(int), 6, true)]
		public int  ImportTime;

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
		/// ���ͣ�LOT��PCS
		/// </summary>
		[FieldMapAttribute("TYPE", typeof(string), 40, false)]
		public string  Type;

		/// <summary>
		/// ��ʼ���к�
		/// </summary>
		[FieldMapAttribute("STARTSN", typeof(string), 40, false)]
		public string  StartSN;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, false)]
		public string  OPCode;

		/// <summary>
		/// ��Ʒ״̬��GOOD/NG
		/// </summary>
		[FieldMapAttribute("PRODUCTSTATUS", typeof(string), 40, false)]
		public string  ProductStatus;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("ERRORDESC", typeof(string), 100, false)]
		public string  ErrorDescription;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("QTY", typeof(decimal), 10, true)]
		public decimal  Qty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, false)]
		public string  StepSequenceCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SHIFTDATE", typeof(string), 40, false)]
		public string  ShiftDate;

	}
	#endregion

	#region OutWIPMaterial
	/// <summary>
	/// �����������
	/// </summary>
	[Serializable, TableMap("TBLOUTWIPMATERIAL", "RndID")]
	public class OutWIPMaterial : DomainObject
	{
		public OutWIPMaterial()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RndID", typeof(string), 40, true)]
		public string  RndID;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// �������к�
		/// </summary>
		[FieldMapAttribute("ENDSN", typeof(string), 40, false)]
		public string  EndSN;

		/// <summary>
		/// ��ע
		/// </summary>
		[FieldMapAttribute("MEMO", typeof(string), 100, false)]
		public string  Memo;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("IMPORTUSER", typeof(string), 40, false)]
		public string  ImportUser;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("IMPORTDATE", typeof(int), 8, true)]
		public int  ImportDate;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[FieldMapAttribute("IMPORTTIME", typeof(int), 6, true)]
		public int  ImportTime;

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
		/// ���ͣ�LOT��PCS
		/// </summary>
		[FieldMapAttribute("TYPE", typeof(string), 40, false)]
		public string  Type;

		/// <summary>
		/// ��ʼ���к�
		/// </summary>
		[FieldMapAttribute("STARTSN", typeof(string), 40, false)]
		public string  StartSN;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, false)]
		public string  MaterialCode;

		/// <summary>
		/// Date Code
		/// </summary>
		[FieldMapAttribute("DATECODE", typeof(string), 40, false)]
		public string  DateCode;

		/// <summary>
		/// ��Ӧ��
		/// </summary>
		[FieldMapAttribute("SUPPLIER", typeof(string), 40, false)]
		public string  Supplier;

	}
	#endregion

	#region OutMaterial
	/// <summary>
	/// ���������������
	/// </summary>
	[Serializable, TableMap("TBLOUTMATERIAL", "REELNO")]
	public class OutMaterial : DomainObject
	{
		public OutMaterial()
		{
		}
 
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// ���ϴ���
		/// </summary>
		[FieldMapAttribute("MATERIALCODE", typeof(string), 40, true)]
		public string  MaterialCode;

		/// <summary>
		/// �Ͼ���
		/// </summary>
		[FieldMapAttribute("REELNO", typeof(string), 40, true)]
		public string  ReelNo;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("QTY", typeof(decimal), 15, false)]
		public decimal  Qty;

		/// <summary>
		/// �Ƿ��ز�
		/// </summary>
		[FieldMapAttribute("ISSPECIAL", typeof(string), 1, false)]
		public string  IsSpecial;

		/// <summary>
		/// �Ƿ��������
		/// </summary>
		[FieldMapAttribute("ISOUTMATERIAL", typeof(string), 1, false)]
		public string  IsOutMaterial;

		/// <summary>
		/// ά���û�
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// ά������
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(decimal), 8, false)]
		public decimal  MaintainDate;
	}
	#endregion

}

