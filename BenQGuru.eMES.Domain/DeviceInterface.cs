using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for DeviceInterface
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2006-5-30 09:13:33 ����
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.DeviceInterface
{

	#region PreTestValue
	/// <summary>
	/// 
	/// </summary>
	[Serializable, TableMap("tblPreTestValue", "ID")]
	public class PreTestValue : DomainObject
	{
		public PreTestValue()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RCard", typeof(string), 40, true)]
		public string  RCard;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCode", typeof(string), 40, false)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("Value", typeof(decimal), 15, true)]
		public decimal  Value;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MaxValue", typeof(decimal), 15, true)]
		public decimal  MaxValue;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MinValue", typeof(decimal), 15, true)]
		public decimal  MinValue;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ID", typeof(string), 40, true)]
		public string  ID;


		//****************************
		//�����ֶ����ݿ�����ʱû�м�¼ Simone
		//****************************

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("ItemCode", typeof(string), 40, false)]
		public string  ItemCode;

		/// <summary>
		/// ���ߴ���
		/// </summary>
		[FieldMapAttribute("SSCode", typeof(string), 40, false)]
		public string  SSCode;

		/// <summary>
		/// ��Դ����
		/// </summary>
		[FieldMapAttribute("ResCode", typeof(string), 40, false)]
		public string  ResCode;

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
		[FieldMapAttribute("TestResult", typeof(string), 10, true)]
		public string  TestResult;

	}
	#endregion

}

