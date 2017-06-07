using System;
using System.Collections.Generic;
using System.Text;

using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.Domain.BaseSetting
{
    #region User Ex
    /// <summary>
    /// User ����
    /// </summary>
    public class UserEx : User
    {
        public UserEx()
        {
        }

        /// <summary>
        /// User2Org�б�
        /// </summary>
        public User2Org[] user2OrgList;

        /// <summary>
        /// Ĭ����֯Desc
        /// </summary>
        [FieldMapAttribute("ORGDESC", typeof(string), 40, false)]
        public string DefaultOrgDesc;
    }
    #endregion
}
