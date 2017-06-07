using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for DataLink ��������
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2006-05-18 9:29:18
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.DataLink
{

	#region FT
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLFT", "RCard,TestSeq")]
	public class FT : DomainObject
	{
		public FT()
		{
		}
 
		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		[FieldMapAttribute("RCard", typeof(string), 40, true)]
		public string  RCard;

		/// <summary>
		/// ������ţ���Զ�β������õ��ֶΣ�
		/// </summary>
		[FieldMapAttribute("TestSeq", typeof(int), 10, true)]
		public int  TestSeq;

		/// <summary>
		/// ��Ʒ
		/// </summary>
		[FieldMapAttribute("Itemcode", typeof(string), 40, true)]
		public string  Itemcode;

		/// <summary>
		/// ��Դ(�豸)
		/// </summary>
		[FieldMapAttribute("Rescode", typeof(string), 40, true)]
		public string  Rescode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("LineCode", typeof(string), 40, true)]
		public string  LineCode;

		/// <summary>
		/// �ƾ�
		/// </summary>
		[FieldMapAttribute("Machinetool", typeof(string), 40, true)]
		public string  Machinetool;
		

		/// <summary>
		/// �ɼ���Ա
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// �ɼ�����
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// �ɼ�ʱ��
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;


		/// <summary>
		/// ���Խ��
		/// </summary>
		[FieldMapAttribute("TestResult", typeof(string), 1, true)]
		public string  TestResult;

	}
	#endregion

	#region FTDetail
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("TBLFTDetail", "RCard,TestSeq,TGroup")]
	public class FTDetail : DomainObject
	{
		public FTDetail()
		{
		}
 
		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		[FieldMapAttribute("RCard", typeof(string), 40, true)]
		public string  RCard;

		/// <summary>
		/// ������ţ���Զ�β������õ��ֶΣ�
		/// </summary>
		[FieldMapAttribute("TestSeq", typeof(int), 10, true)]
		public int  TestSeq;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("TGroup", typeof(int), 10, true)]
		public int  TGroup;

		/// <summary>
		/// Ƶ��(����)
		/// </summary>
		[FieldMapAttribute("FreqUpSpec", typeof(decimal), 15, true)]
		public decimal  FreqUpSpec;

		/// <summary>
		/// Ƶ��(����)
		/// </summary>
		[FieldMapAttribute("FreqLowSpec", typeof(decimal), 15, true)]
		public decimal  FreqLowSpec;

		/// <summary>
		/// Ƶ��(����ֵ)
		/// </summary>
		[FieldMapAttribute("Freq", typeof(decimal), 15, true)]
		public decimal  Freq;

		#region Duty_RT

		/// <summary>
		/// DUTY_RATO(����)
		/// </summary>
		[FieldMapAttribute("DutyUpSpec", typeof(decimal), 15, false)]
		public decimal  DutyUpSpec;

		/// <summary>
		/// DUTY_RATO(����)
		/// </summary>
		[FieldMapAttribute("DutyLowSpec", typeof(decimal), 15, false)]
		public decimal  DutyLowSpec;

		/// <summary>
		/// DUTY_RATO(����ֵ)
		/// </summary>
		[FieldMapAttribute("Duty_Rt", typeof(decimal), 15, false)]
		public decimal  Duty_Rt;
		#endregion

		#region Burst_MD
		/// <summary>
		/// BURST_MD(����)
		/// </summary>
		[FieldMapAttribute("BurstUpSpec", typeof(decimal), 15, false)]
		public decimal  BurstUpSpec;

		/// <summary>
		/// BURST_MD(����)
		/// </summary>
		[FieldMapAttribute("BurstLowSpec", typeof(decimal), 15, false)]
		public decimal  BurstLowSpec;

		/// <summary>
		/// BURST_MD(����ֵ)
		/// </summary>
		[FieldMapAttribute("Burst_Md", typeof(decimal), 15, false)]
		public decimal  Burst_Md;
		#endregion


		/// <summary>
		/// ����(����)
		/// </summary>
		[FieldMapAttribute("ACUpSpec", typeof(decimal), 15, true)]
		public decimal  ACUpSpec;

		/// <summary>
		/// ����(����)
		/// </summary>
		[FieldMapAttribute("ACLowSpec", typeof(decimal), 15, true)]
		public decimal  ACLowSpec;

		/// <summary>
		/// ����1(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC1", typeof(decimal), 15, true)]
		public decimal  AC1;

		/// <summary>
		/// ����2(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC2", typeof(decimal), 15, true)]
		public decimal  AC2;

		/// <summary>
		/// ����3(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC3", typeof(decimal), 15, true)]
		public decimal  AC3;

		/// <summary>
		/// ����4(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC4", typeof(decimal), 15, true)]
		public decimal  AC4;

		/// <summary>
		/// ����5(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC5", typeof(decimal), 15, true)]
		public decimal  AC5;

		/// <summary>
		/// ����6(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC6", typeof(decimal), 15, true)]
		public decimal  AC6;

		/// <summary>
		/// ����7(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC7", typeof(decimal), 15, true)]
		public decimal  AC7;

		/// <summary>
		/// ����8(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC8", typeof(decimal), 15, true)]
		public decimal  AC8;

		/// <summary>
		/// ����9(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC9", typeof(decimal), 15, true)]
		public decimal  AC9;

		/// <summary>
		/// ����9(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC10", typeof(decimal), 15, true)]
		public decimal  AC10;

		/// <summary>
		/// ����11(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC11", typeof(decimal), 15, true)]
		public decimal  AC11;

		/// <summary>
		/// ����12(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC12", typeof(decimal), 15, true)]
		public decimal  AC12;

		/// <summary>
		/// ����13(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC13", typeof(decimal), 15, true)]
		public decimal  AC13;

		/// <summary>
		/// ����14(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC14", typeof(decimal), 15, true)]
		public decimal  AC14;

		/// <summary>
		/// ����15(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC15", typeof(decimal), 15, true)]
		public decimal  AC15;

		/// <summary>
		/// ����16(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC16", typeof(decimal), 15, true)]
		public decimal  AC16;

		/// <summary>
		/// ����17(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC17", typeof(decimal), 15, false)]
		public decimal  AC17;

		/// <summary>
		/// ����18(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC18", typeof(decimal), 15, false)]
		public decimal  AC18;

		/// <summary>
		/// ����19(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC19", typeof(decimal), 15, false)]
		public decimal  AC19;

		/// <summary>
		/// ����20(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC20", typeof(decimal), 15, false)]
		public decimal  AC20;

		/// <summary>
		/// ����21(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC21", typeof(decimal), 15, false)]
		public decimal  AC21;

		/// <summary>
		/// ����22(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC22", typeof(decimal), 15, false)]
		public decimal  AC22;

		/// <summary>
		/// ����23(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC23", typeof(decimal), 15, false)]
		public decimal  AC23;

		/// <summary>
		/// ����24(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC24", typeof(decimal), 15, false)]
		public decimal  AC24;
		
		/// <summary>
		/// ����25(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC25", typeof(decimal), 15, false)]
		public decimal  AC25;

		/// <summary>
		/// ����26(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC26", typeof(decimal), 15, false)]
		public decimal  AC26;

		/// <summary>
		/// ����27(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC27", typeof(decimal), 15, false)]
		public decimal  AC27;

		/// <summary>
		/// ����28(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC28", typeof(decimal), 15, false)]
		public decimal  AC28;

		/// <summary>
		/// ����29(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC29", typeof(decimal), 15, false)]
		public decimal  AC29;

		/// <summary>
		/// ����30(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC30", typeof(decimal), 15, false)]
		public decimal  AC30;

		/// <summary>
		/// ����31(����ֵ)
		/// </summary>
		[FieldMapAttribute("AC31", typeof(decimal), 15, false)]
		public decimal  AC31;
	}
	#endregion

}

