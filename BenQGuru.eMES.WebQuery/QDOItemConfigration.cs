using System;
using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.WebQuery
{
	/// <summary>
	/// QDOItemConfigration ��ժҪ˵����
	/// </summary>
	public class QDOItemConfigration : DomainObject 
	{
		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, false)]
		public string  RunningCard;

		/// <summary>
		/// ���
		/// </summary>
		[FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, false)]
		public decimal  RunningCardSequence;

		/// <summary>
		/// Ա������
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// �ɼ�ʱ��
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// �ɼ�����
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("CheckItemCode", typeof(string), 40, false)]
		public string  CheckItemCode;

		/// <summary>
		/// ���ô���
		/// </summary>
		[FieldMapAttribute("CatergoryCode", typeof(string), 40, false)]
		public string  CatergoryCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, true)]
		public string  EAttribute1;

		/// <summary>
		/// ʱ��δ���
		/// </summary>
		[FieldMapAttribute("TPCODE", typeof(string), 40, true)]
		public string  TimePeriodCode;

		/// <summary>
		/// ��δ���
		/// </summary>
		[FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
		public string  ShiftCode;

		/// <summary>
		/// ���ƴ���
		/// </summary>
		[FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
		public string  ShiftTypeCode;

		/// <summary>
		/// �ɼ���Դ
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string  ResourceCode;

		/// <summary>
		/// �ɼ���λ
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string  OPCode;

		/// <summary>
		/// �ɼ��߱�
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string  StepSequenceCode;

		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string  SegmnetCode;

		/// <summary>
		/// ����;�̴���
		/// </summary>
		[FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
		public string  RouteCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string  ModelCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ItemCode;

		/// <summary>
		/// ��׼ֵ
		/// </summary>
		[FieldMapAttribute("CheckItemVlaue", typeof(string), 40, false)]
		public string  CheckItemVlaue;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MoCode", typeof(string), 40, false)]
		public string  MoCode;

		/// <summary>
		/// ʵ��ֵ
		/// </summary>
		[FieldMapAttribute("ActValue", typeof(string), 40, true)]
		public string  ActValue;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("ItemConfig", typeof(string), 40, true)]
		public string  ItemConfig;

	}
}
