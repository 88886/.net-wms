using System;
using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.Domain.BaseSetting
{
    #region FunctionGroup
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLFUNCTIONGROUP", "FUNCTIONGROUPCODE")]
    public class FunctionGroup : DomainObject
    {
        public FunctionGroup()
        {
        }

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("FUNCTIONGROUPCODE", typeof(string), 40, true)]
        public string FunctionGroupCode;

        /// <summary>
        /// ����������
        /// </summary>
        [FieldMapAttribute("FUNCTIONGROUPDESC", typeof(string), 40, false)]
        public string FunctionGroupDescription;

        /// <summary>
        /// ���ά���û�
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string MaintainUser;

        /// <summary>
        /// ���ά������
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// ���ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;
    }
    #endregion
}
