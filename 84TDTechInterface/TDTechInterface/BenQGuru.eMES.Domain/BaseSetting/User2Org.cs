using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for User2Org
/// ** �� ��:		Created by Scott Gy
/// ** �� ��:		2008-06-25 10:36:31
/// ** �� ��:
/// ** �� ��:
/// </summary>

namespace BenQGuru.eMES.Domain.BaseSetting
{
    #region User2Org
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLUSER2ORG", "USERCODE,ORGID")]
    public class User2Org : DomainObject
    {
        public User2Org()
        {
        }

        /// <summary>
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        /// <summary>
        /// ���ά������[LastMaintainDate]
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// ���ά��ʱ��[LastMaintainTime]
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        /// <summary>
        /// �û�����
        /// </summary>
        [FieldMapAttribute("USERCODE", typeof(string), 40, false)]
        public string UserCode;

        /// <summary>
        /// ��֯���
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrganizationID;

        /// <summary>
        /// �Ƿ�ΪĬ����֯
        /// </summary>
        [FieldMapAttribute("DEFAULTORG", typeof(int), 1, false)]
        public int IsDefaultOrg;

    }
    #endregion
}
