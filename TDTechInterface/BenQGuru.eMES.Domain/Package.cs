using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for Package
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2006-5-27 11:08:17
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.Package
{

    #region CARTONINFO
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLCARTONINFO", "PKCARTONID")]
    public class CARTONINFO : DomainObject
    {
        public CARTONINFO()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("PKCARTONID", typeof(string), 40, true)]
        public string PKCARTONID;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, true)]
        public string CARTONNO;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("CAPACITY", typeof(decimal), 10, true)]
        public decimal CAPACITY;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("COLLECTED", typeof(decimal), 10, true)]
        public decimal COLLECTED;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MUSER;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(decimal), 10, true)]
        public decimal MDATE;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(decimal), 10, true)]
        public decimal MTIME;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 100, false)]
        public string EATTRIBUTE1;

    }
    #endregion

    public class CartonCollection : CARTONINFO
    {
        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string MOCode;

        /// <summary>
        /// �Ϻ�[ItemCode]
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        /// <summary>
        /// ��Ʒ����[ItemDesc]
        /// </summary>
        [FieldMapAttribute("ITEMDESC", typeof(string), 100, false)]
        public string ItemDescription;

        /// <summary>
        /// ��Ʒ����[ItemName]
        /// </summary>
        [FieldMapAttribute("ITEMNAME", typeof(string), 100, false)]
        public string ItemName;
    }

    #region PKRuleStep
    /// <summary>
    /// ��װ������
    /// </summary>
    [Serializable, TableMap("tblPKRuleStep", "PKRuleCode,Step")]
    public class PKRuleStep : DomainObject
    {
        public PKRuleStep()
        {
        }

        /// <summary>
        /// ��װ�������
        /// </summary>
        [FieldMapAttribute("PKRuleCode", typeof(string), 40, true)]
        public string PKRuleCode;

        /// <summary>
        /// ��װ�������
        /// </summary>
        [FieldMapAttribute("Step", typeof(int), 10, true)]
        public int Step;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("StepCode", typeof(string), 40, true)]
        public string StepCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("StepName", typeof(string), 100, false)]
        public string StepName;

        /// <summary>
        ///  �Ƿ񱣴汾���������
        /// </summary>
        [FieldMapAttribute("IsSaveRCard", typeof(string), 1, false)]
        public string IsSaveRCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUser", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDate", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTime", typeof(int), 6, true)]
        public int MaintainTime;

    }
    #endregion

    #region TBLPACKINGCHK

    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLPACKINGCHK", "RCARD")]
    public class PACKINGCHK : DomainObject
    {
        public PACKINGCHK()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string Rcard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("CHKPRODUCTCODE", typeof(string), 40, true)]
        public string CheckProductCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("CHKACCESSORY", typeof(string), 40, true)]
        public string CheckAccessory;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MUSER;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(decimal), 8, true)]
        public decimal MDATE;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(decimal), 6, true)]
        public decimal MTIME;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string EATTRIBUTE1;

    }
    #endregion


    #region SKDCartonDetail
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLSKDCARTONDETAIL", "MOCODE,MCARD")]
    public class SKDCartonDetail : DomainObject
    {
        public SKDCartonDetail()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string moCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, true)]
        public string CartonNO;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40,true)]
        public string ItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SBITEMCODE", typeof(string), 40, true)]
        public string SBItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MCARD", typeof(string), 40, true)]
        public string MCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

    }
    #endregion

    #region SKDCartonDetailWithCapity

    public class SKDCartonDetailWithCapity : SKDCartonDetail
    {
        [FieldMap("MNAME", typeof(String), 40, true)]
        public string MaterialName;

        [FieldMapAttribute("CARTONQTY", typeof(decimal), 10, true)]
        public decimal cartonQty;

        [FieldMapAttribute("MOQTY", typeof(decimal), 10, true)]
        public decimal moQty;

        [FieldMapAttribute("PLANQTY", typeof(decimal), 10, true)]
        public decimal planQty;
    }

    #endregion
}

