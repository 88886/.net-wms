using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for ATE
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jessie Lee
/// ** �� ��:		2006-5-22 10:24:05
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.MOModel
{

    #region Item2Label
    /// <summary>
    /// ��Ʒ��Ӧ�ı�ǩģ��
    /// </summary>
    [Serializable, TableMap("tblItem2Label", "ItemCode,LabelCode")]
    public class Item2Label : DomainObject
    {
        public Item2Label()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ItemCode", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ItemDesc", typeof(string), 100, true)]
        public string ItemDesc;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("LabelCode", typeof(string), 40, true)]
        public string LabelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("LabelDesc", typeof(string), 40, true)]
        public string LabelDesc;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
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

    #region LabelRCardPrintLog
    /// <summary>
    /// ��Ʒ���д�ӡLog
    /// </summary>
    [Serializable, TableMap("tblLabelRCardPrintLog", "RCard3")]
    public class LabelRCardPrintLog : DomainObject
    {
        public LabelRCardPrintLog()
        {
        }

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("MoCode", typeof(string), 40, true)]
        public string MoCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("MoSeq", typeof(int), 10, false)]
        public decimal MoSeq;

        /// <summary>
        /// LotNo
        /// </summary>
        [FieldMapAttribute("MoLotNo", typeof(string), 40, false)]
        public string MoLotNo;

        /// <summary>
        /// 3#������ˮ��
        /// </summary>
        [FieldMapAttribute("RCard3Seq", typeof(int), 10, false)]
        public int RCard3Seq;

        /// <summary>
        /// 2#������ˮ��
        /// </summary>
        [FieldMapAttribute("RCard2Seq", typeof(int), 10, true)]
        public int RCard2Seq;

        /// <summary>
        /// 3#����
        /// </summary>
        [FieldMapAttribute("RCard3", typeof(string), 40, true)]
        public string RCard3;

        /// <summary>
        /// 2#����
        /// </summary>
        [FieldMapAttribute("RCard2", typeof(string), 40, true)]
        public string RCard2;

        /// <summary>
        /// ModelCode�����ǲ�Ʒ��
        /// </summary>
        [FieldMapAttribute("ModelCode", typeof(string), 40, true)]
        public string ModelCode;

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

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("PrintDate", typeof(int), 8, true)]
        public int PrintDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("PrintTime", typeof(int), 6, true)]
        public int PrintTime;

        /// <summary>
        /// �꣨2#�������кŹ����е��꣩
        /// </summary>
        [FieldMapAttribute("Year", typeof(string), 40, true)]
        public string Year;

    }
    #endregion

    #region LabelRCardPrintLog_MinMaxSeq
    /// <summary>
    /// ��Ʒ���д�ӡLog
    /// </summary>
    [Serializable, TableMap("tblLabelRCardPrintLog", "RCard3")]
    public class LabelRCardPrintLog_MinMaxSeq : DomainObject
    {
        public LabelRCardPrintLog_MinMaxSeq()
        {
        }
                
        [FieldMapAttribute("MinSeq", typeof(int), 10, true)]
        public int MinSeq;

        [FieldMapAttribute("MaxSeq", typeof(int), 10, true)]
        public int MaxSeq;

    }
    #endregion

    #region LabelRCardSeq2
    /// <summary>
    /// 2#������ˮ����
    /// </summary>
    [Serializable, TableMap("tblLabelRCard2Seq", "Year,ModelCode")]
    public class LabelRCard2Seq : DomainObject
    {
        public LabelRCard2Seq()
        {
        }

        /// <summary>
        /// һ���ַ���07����R���Դ�����
        /// </summary>
        [FieldMapAttribute("Year", typeof(string), 40, true)]
        public string Year;

        /// <summary>
        /// ��Ʒ���е�Model Code,���ǲ�Ʒ��
        /// </summary>
        [FieldMapAttribute("ModelCode", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// ���һ����ˮ��
        /// </summary>
        [FieldMapAttribute("CurrSeq", typeof(int), 10, false)]
        public int CurrSeq;

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

    #region LabelRCard3Seq
    /// <summary>
    /// 3#������ˮ����
    /// </summary>
    [Serializable, TableMap("TblLabelRCardSeq3", "ModelCode,MoLotNo")]
    public class LabelRCard3Seq : DomainObject
    {
        public LabelRCard3Seq()
        {
        }

        /// <summary>
        /// ��Ʒ���е�Model Code
        /// </summary>
        [FieldMapAttribute("ModelCode", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// MO LOT NO
        /// </summary>
        [FieldMapAttribute("MoLotNo", typeof(string), 40, true)]
        public string MoLotNo;

        /// <summary>
        /// ��ǰ��ˮ��
        /// </summary>
        [FieldMapAttribute("CurrSeq", typeof(decimal), 10, false)]
        public decimal CurrSeq;

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

    #region LabelVAPrintLog
    /// <summary>
    /// VA���кŴ�ӡLog
    /// </summary>
    [Serializable, TableMap("tblLabelVAPrintLog", "Seq,MoCode")]
    public class LabelVAPrintLog : DomainObject
    {
        public LabelVAPrintLog()
        {
        }

        /// <summary>
        /// ��ˮ��
        /// </summary>
        [FieldMapAttribute("Seq", typeof(int), 10, true)]
        public decimal Seq;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("MoCode", typeof(string), 40, true)]
        public string MoCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("MoSeq", typeof(int), 10, true)]
        public decimal MoSeq;

        /// <summary>
        /// Model Code
        /// </summary>
        [FieldMapAttribute("ItemModelCode", typeof(string), 40, true)]
        public string ItemModelCode;

        /// <summary>
        /// ��
        /// </summary>
        [FieldMapAttribute("Year", typeof(string), 40, true)]
        public string Year;

        /// <summary>
        /// ��
        /// </summary>
        [FieldMapAttribute("Week", typeof(string), 40, true)]
        public string Week;

        /// <summary>
        /// ��һ�δ�ӡ����
        /// </summary>
        [FieldMapAttribute("PrintDate", typeof(int), 8, true)]
        public int PrintDate;

        /// <summary>
        /// ��һ�δ�ӡʱ��
        /// </summary>
        [FieldMapAttribute("PrintTime", typeof(int), 6, true)]
        public int PrintTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MaintainUser", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MaintainDate", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MaintainTime", typeof(int), 6, true)]
        public int MaintainTime;

    }
    #endregion

    #region LabelVASeq
    /// <summary>
    /// VA���к���ˮ����
    /// </summary>
    [Serializable, TableMap("tblLabelVASeq", "ItemModelCode,Year,Week")]
    public class LabelVASeq : DomainObject
    {
        public LabelVASeq()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ItemModelCode", typeof(string), 40, true)]
        public string ItemModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("Year", typeof(string), 40, true)]
        public string Year;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("Week", typeof(string), 40, true)]
        public string Week;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("CurrSeq", typeof(int), 10, true)]
        public int CurrSeq;

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

    #region LabelDBSeq
    /// <summary>
    /// �Ӱ���ˮ�Ź���
    /// </summary>
    [Serializable, TableMap("tblLabelDBSeq", "MoLotNo,MoCode")]
    public class LabelDBSeq : DomainObject
    {
        public LabelDBSeq()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MoLotNo", typeof(string), 40, true)]
        public string MoLotNo;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MoCode", typeof(string), 40, true)]
        public string MoCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ItemCode", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// �Ӱ幤�����
        /// </summary>
        [FieldMapAttribute("DBMoSeq", typeof(int), 40, true)]
        public int DBMoSeq;

        /// <summary>
        /// �˹�����3��������ˮ�ŵĿ�ʼ
        /// </summary>
        [FieldMapAttribute("StartSeq", typeof(int), 10, true)]
        public int StartSeq;

        /// <summary>
        /// �˹����3����ˮ�ŵĽ���
        /// </summary>
        [FieldMapAttribute("EndSeq", typeof(int), 10, true)]
        public int EndSeq;

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

    #region SS2MaxCartonSeq
    /// <summary>
    /// �Ӱ���ˮ�Ź���
    /// </summary>
    [Serializable, TableMap("TBLSS2MAXCARTONSEQ", "SSCODE")]
    public class SS2MaxCartonSeq : DomainObject
    {
        public SS2MaxCartonSeq()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SSCode", typeof(string), 40, false)]
        public string SSCode;

        /// <summary>
        /// �Ӱ幤�����
        /// </summary>
        [FieldMapAttribute("MaxCartonSeq", typeof(int), 10, false)]
        public int MaxCartonSeq;
        
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

    #region �趨���Լӵ������ļ��У��û��Ժ�����Լ��޸�
    public class LabelSufix
    {
        public static string RCard = Properties.Settings.Default.No3_CheckChar;
        public static string DB = Properties.Settings.Default.DB_CheckChar;
        public static string SMT = Properties.Settings.Default.SMT_CheckChar;
        public static string No5 = Properties.Settings.Default.No5_CheckChar;
    }

    public class LabelSeqLength
    {
        public static int No3 = 4;
        public static int No2 = 6;
        public static int VA = 4;
        public static int Carton = 5;
    }
    #endregion
}

