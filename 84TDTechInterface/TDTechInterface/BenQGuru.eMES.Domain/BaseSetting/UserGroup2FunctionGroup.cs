using System;
using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.Domain.BaseSetting
{
    #region UserGroup2FunctionGroup
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLUSERGROUP2FUNCTIONGROUP", "USERGROUPCODE,FUNCTIONGROUPCODE")]
    public class UserGroup2FunctionGroup : DomainObject
    {
        public UserGroup2FunctionGroup()
        {
        }

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

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("FUNCTIONGROUPCODE", typeof(string), 40, true)]
        public string FunctionGroupCode;

        /// <summary>
        /// �û������
        /// </summary>
        [FieldMapAttribute("USERGROUPCODE", typeof(string), 40, true)]
        public string UserGroupCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("VIEWVALUE", typeof(string), 40, false)]
        public string ViewValue;

    }
    #endregion
}
