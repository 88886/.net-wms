using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for ShiftModel
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2005-04-29 10:36:29
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.BaseSetting
{

	#region Shift
	/// <summary>
	/// ���
	/// </summary>
	[Serializable, TableMap("TBLSHIFT", "SHIFTCODE")]
	public class Shift : DomainObject
	{
		public Shift()
		{
		}
 
		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("SHIFTDESC", typeof(string), 100, false)]
		public string  ShiftDescription;

		/// <summary>
		/// ��ε���ʼʱ��
		/// </summary>
		[FieldMapAttribute("SHIFTBTIME", typeof(int), 6, true)]
		public int  ShiftBeginTime;

		/// <summary>
		/// ��εĽ���ʱ��
		/// </summary>
		[FieldMapAttribute("SHIFTETIME", typeof(int), 6, true)]
		public int  ShiftEndTime;

		/// <summary>
		/// �Ƿ��ǿ�����
		/// 1:��
		/// 0:����
		/// </summary>
		[FieldMapAttribute("ISOVERDAY", typeof(string), 1, true)]
		public string  IsOverDate;

		/// <summary>
		/// ���ϵͳ�û�[LastMaintainUser]
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
		/// ���˳��
		/// </summary>
		[FieldMapAttribute("SHIFTSEQ", typeof(decimal), 10, true)]
		public decimal  ShiftSequence;

		/// <summary>
		/// ��δ���
		/// </summary>
		[FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
		public string  ShiftCode;

		/// <summary>
		/// ���ƴ���
		/// </summary>
		[FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLSHIFTTYPE", "SHIFTTYPECODE", "SHIFTTYPEDESC")]
		public string  ShiftTypeCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

	}
	#endregion

	#region ShiftType
	/// <summary>
	/// ����
	/// </summary>
	[Serializable, TableMap("TBLSHIFTTYPE", "SHIFTTYPECODE")]
	public class ShiftType : DomainObject
	{
		public ShiftType()
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
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
		public string  MaintainUser;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("SHIFTTYPEDESC", typeof(string), 100, false)]
		public string  ShiftTypeDescription;

		/// <summary>
		/// ��Ч����
		/// </summary>
		[FieldMapAttribute("EFFDATE", typeof(decimal), 10, true)]
		public decimal  EffectiveDate;

		/// <summary>
		/// ʧЧ����
		/// </summary>
		[FieldMapAttribute("IVLDATE", typeof(int), 8, true)]
		public int  InvalidDate;

		/// <summary>
		/// ���ƴ���
		/// </summary>
		[FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
		public string  ShiftTypeCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

	}
	#endregion

	#region TimePeriod
	/// <summary>
	/// ʱ���
	/// </summary>
	[Serializable, TableMap("TBLTP", "TPCODE")]
	public class TimePeriod : DomainObject
	{
		public TimePeriod()
		{
		}
 
		/// <summary>
		/// ʱ��εĿ�ʼʱ��
		/// </summary>
		[FieldMapAttribute("TPBTIME", typeof(int), 6, true)]
		public int  TimePeriodBeginTime;

		/// <summary>
		/// ʱ��εĽ���ʱ��
		/// </summary>
		[FieldMapAttribute("TPETIME", typeof(int), 6, true)]
		public int  TimePeriodEndTime;

		/// <summary>
		/// ���ϵͳ�û�[LastMaintainUser]
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
		/// �Ƿ��ǿ�����
		/// 1:��
		/// 0:����
		/// </summary>
		[FieldMapAttribute("ISOVERDATE", typeof(string), 1, true)]
		public string  IsOverDate;

		/// <summary>
		/// ʱ�������
		/// </summary>
		[FieldMapAttribute("TPSEQ", typeof(int), 6, true)]
		public int  TimePeriodSequence;

		/// <summary>
		/// ʱ��δ���
		/// </summary>
		[FieldMapAttribute("TPCODE", typeof(string), 40, true)]
		public string  TimePeriodCode;

		/// <summary>
		/// ʱ�������
		/// </summary>
		[FieldMapAttribute("TPDESC", typeof(string), 100, false)]
		public string  TimePeriodDescription;

		/// <summary>
		/// Normal /Exception
		/// </summary>
		[FieldMapAttribute("TPTYPE", typeof(string), 40, true)]
		public string  TimePeriodType;

		/// <summary>
		/// ��δ���
		/// </summary>
		[FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLSHIFT", "SHIFTCODE", "SHIFTDESC")]
		public string  ShiftCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
		public string  ShiftTypeCode;

	}
	#endregion

    #region ShiftCrew
    /// <summary>
    /// ����
    /// </summary>
    [Serializable, TableMap("TBLCREW", "CREWCODE")]
    public class ShiftCrew : DomainObject
    {
        public ShiftCrew()
        {
        }

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("CREWCODE", typeof(string), 40, false)]
        public string CrewCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("CREWDESC", typeof(string), 100, true)]
        public string CrewDesc;

        /// <summary>
        /// ���ϵͳ�û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

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
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

    }
    #endregion

}

