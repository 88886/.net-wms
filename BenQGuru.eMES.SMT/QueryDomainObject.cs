using System;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 

namespace  BenQGuru.eMES.SMT
{
	/// <summary>
	/// StationBOM ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Simone Xu
	/// ��������:	2005/06/20
	/// �޸���:
	/// �޸�����:
	/// �� ��:		����վ��ʵ��,���ڱȶ�,���ݿ��޴˱�
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class StationBOM : DomainObject
	{
		/// ��������[MOCODE]
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MOCode;

		/// <summary>
		/// ��̨����[RESCODE]
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string ResourceCode;

		/// <summary>
		/// վλ���[StationCode]
		/// </summary>
		[FieldMapAttribute("StationCode", typeof(string), 40, true)]
		public string  StationCode;

		/// <summary>
		/// Feeder������[FEEDERCODE]
		/// </summary>
		[FieldMapAttribute("FEEDERCODE", typeof(string), 40, true)]
		public string  FeederCode;

		/// <summary>
		/// ����������[OBITEMCODE]
		/// </summary>
		[FieldMapAttribute("OBITEMCODE", typeof(string), 40, true)]
		public string  OBItemCode;


		/// <summary>
		/// �ȶԽ��[CompareResult]
		/// </summary>
		[FieldMapAttribute("CompareResult", typeof(string), 40, true)]
		public string  CompareResult;
	}

	/// <summary>
	/// MOItemBOM ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Simone Xu
	/// ��������:	2005/06/20
	/// �޸���:
	/// �޸�����:
	/// �� ��:		���빤�������嵥ʵ��,���ڱȶ�,���ݿ��޴˱�
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class MOItemBOM : DomainObject
	{

		/// ��������[MOCODE]
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MOCode;

		/// <summary>
		/// ��Ʒ����[ITEMCODE]
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ItemCode;

		/// <summary>
		/// �ӽ��Ϻ�[ITEMCODE]
		/// </summary>
		[FieldMapAttribute("OBITEMCODE", typeof(string), 40, true)]
		public string  OBItemCode;

		/// <summary>
		/// �ӽ�������[OBITEMNAME]
		/// </summary>
		[FieldMapAttribute("OBITEMNAME", typeof(string), 40, true)]
		public string  OBItemName;

		/// <summary>
		/// ��������[ITEMCODE]
		/// </summary>
		[FieldMapAttribute("OBITEMQTY", typeof(string), 40, true)]
		public string  OBItemQTY;

		/// <summary>
		/// ������λ[OBITEMUNIT]
		/// </summary>
		[FieldMapAttribute("OBITEMUNIT", typeof(string), 40, true)]
		public string  OBItemUnit;

		/// <summary>
		/// �ȶԽ��[CompareResult]
		/// </summary>
		[FieldMapAttribute("CompareResult", typeof(string), 40, true)]
		public string  CompareResult;
	}

	/// <summary>
	/// ���������Ϣ ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Simone Xu
	/// ��������:	2005/06/20
	/// �޸���:
	/// �޸�����:
	/// �� ��:		����վ�������������Ϣ
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class ErrorMessage : DomainObject
	{
		/// ������λ[MOCODE]
		/// </summary>
		[FieldMapAttribute("Errorolumn", typeof(string), 40, true)]
		public string Errorolumn;

		/// <summary>
		/// �������[ITEMCODE]
		/// </summary>
		[FieldMapAttribute("ErrorCode", typeof(string), 40, true)]
		public string  ErrorCode;

		/// <summary>
		/// ����ԭ��[ITEMCODE]
		/// </summary>
		[FieldMapAttribute("ErrorReason", typeof(string), 40, true)]
		public string  ErrorReason;

	}

	/// <summary>
	/// ��������
	/// </summary>
	[Serializable]
	public class SMTRptLineQtyMO : DomainObject
	{
		[FieldMapAttribute("ProductCode", typeof(string), 40, true)]
		public string ProductCode;
		
		[FieldMapAttribute("MOCode", typeof(string), 40, true)]
		public string MOCode;
		
		[FieldMapAttribute("PlanQty", typeof(decimal), 40, true)]
		public decimal PlanQty;
		
		[FieldMapAttribute("PlanManHour", typeof(decimal), 40, true)]
		public decimal PlanManHour;
		
		[FieldMapAttribute("CurrentQty", typeof(decimal), 40, true)]
		public decimal CurrentQty;
		
		[FieldMapAttribute("ActualManHour", typeof(decimal), 40, true)]
		public decimal ActualManHour;
				
		[FieldMapAttribute("ActualQty", typeof(decimal), 40, true)]
		public decimal ActualQty;
		
		[FieldMapAttribute("DifferenceQty", typeof(decimal), 40, true)]
		public decimal DifferenceQty;
		
		[FieldMapAttribute("MOComPassRate", typeof(decimal), 40, true)]
		public decimal MOComPassRate;

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

	}

	/// <summary>
	/// ����ʱ�β���
	/// </summary>
	[Serializable]
	public class SMTRptLineQtyTimePeriod : DomainObject
	{
		[FieldMapAttribute("ProductCode", typeof(string), 40, true)]
		public string ProductCode;
		
		[FieldMapAttribute("MOCode", typeof(string), 40, true)]
		public string MOCode;
		
		[FieldMapAttribute("ShiftDay", typeof(decimal), 40, true)]
		public decimal ShiftDay;
		
		[FieldMapAttribute("TPCode", typeof(string), 40, true)]
		public string TPCode;
		
		[FieldMapAttribute("TPDesc", typeof(string), 40, true)]
		public string TPDescription;
		
		[FieldMapAttribute("CurrentQty", typeof(decimal), 40, true)]
		public decimal CurrentQty;
		
		[FieldMapAttribute("ActualQty", typeof(decimal), 40, true)]
		public decimal ActualQty;
		
		[FieldMapAttribute("DifferenceQty", typeof(decimal), 40, true)]
		public decimal DifferenceQty;
		
		[FieldMapAttribute("TPComPassRate", typeof(decimal), 40, true)]
		public decimal TPComPassRate;
		
		[FieldMapAttribute("DIFFREASON", typeof(string), 100, true)]
		public string MaintainReason;

		[FieldMapAttribute("DIFFMUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		[FieldMapAttribute("DIFFMDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		[FieldMapAttribute("DIFFMTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		[FieldMapAttribute("DAY", typeof(int), 8, true)]
		public int  Day;

		[FieldMapAttribute("TPBTIME", typeof(int), 6, true)]
		public int  TPBeginTime;

		[FieldMapAttribute("TPETIME", typeof(int), 6, true)]
		public int  TPEndTime;

	}

	/// <summary>
	/// ������������
	/// </summary>
	[Serializable]
	public class SMTRptMOMaterial : DomainObject
	{
		[FieldMapAttribute("ProductCode", typeof(string), 40, true)]
		public string ProductCode;
		
		[FieldMapAttribute("MOCode", typeof(string), 40, true)]
		public string MOCode;
		
		[FieldMapAttribute("SSCode", typeof(string), 40, true)]
		public string StepSequenceCode;
		
		[FieldMapAttribute("MaterialCode", typeof(string), 40, true)]
		public string MaterialCode;
		
		[FieldMapAttribute("MachineCode", typeof(string), 40, true)]
		public string MachineCode;
		
		[FieldMapAttribute("MachineStationCode", typeof(string), 40, true)]
		public string MachineStationCode;
		
		[FieldMapAttribute("LogicalUsedQty", typeof(decimal), 40, true)]
		public decimal LogicalUsedQty;
		
		[FieldMapAttribute("ActualUsedQty", typeof(decimal), 40, true)]
		public decimal ActualUsedQty;
		
		[FieldMapAttribute("MachineDiscardRate", typeof(decimal), 40, true)]
		public decimal MachineDiscardRate;
		
		[FieldMapAttribute("MachineDiscardQty", typeof(decimal), 100, true)]
		public decimal MachineDiscardQty;

		[FieldMapAttribute("ManualDiscardRate", typeof(decimal), 40, false)]
		public decimal  ManualDiscardRate;

		[FieldMapAttribute("ManualDiscardQty", typeof(decimal), 8, true)]
		public decimal  ManualDiscardQty;
	}
	
}
