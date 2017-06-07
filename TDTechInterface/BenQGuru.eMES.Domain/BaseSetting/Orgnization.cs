using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	Organization for BaseSetting
/// ** �� ��:		Created by Scott Gu
/// ** �� ��:		2008-06-24 10:36:31
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.BaseSetting
{
    // Added By Hi1/Venus.Feng on 20080624 for Hisense Version : Add Org
    [Serializable, TableMap("TBLORG", "ORGID")]
    public class Organization : DomainObject
    {
        /// <summary>
        /// Organization ID
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, true)]
        public int OrganizationID;

        /// <summary>
        /// Organization Description
        /// </summary>
        [FieldMapAttribute("ORGDESC", typeof(string), 40, true)]
        public string OrganizationDescription;

        /// <summary>
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
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
    }

    // End Added
}

