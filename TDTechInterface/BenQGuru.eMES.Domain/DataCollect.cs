using System;
using BenQGuru.eMES.Common.Domain;
using System.Collections;

/// <summary>
/// ** ��������:	DomainObject for DataCollect
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2005-05-20 9:39:01
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.DataCollect
{

    #region Simulation
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLSIMULATION", "RCARD,MOCODE")]
    public class Simulation : DomainObject
    {
        public Simulation()
        {
        }

        /// <summary>
        /// ���̿���
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// ��ǰ;��
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
        public string RouteCode;

        /// <summary>
        /// �Ϻ�
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// 0 no Complete
        /// 1 completed
        /// </summary>
        [FieldMapAttribute("ISCOM", typeof(string), 40, true)]
        public string IsComplete;

        /// <summary>
        /// CARTON��
        /// </summary>
        [FieldMapAttribute("CARTONCODE", typeof(string), 40, false)]
        public string CartonCode;

        /// <summary>
        /// ջ���
        /// </summary>
        [FieldMapAttribute("PALLETCODE", typeof(string), 40, false)]
        public string PalletCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ��ǰ��Դ
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResourceCode;

        /// <summary>
        /// ��ǰ����
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OPCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// ǰһ����
        /// </summary>
        [FieldMapAttribute("TCARD", typeof(string), 40, true)]
        public string TranslateCard;

        /// <summary>
        /// ���̿�������
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
        public decimal RunningCardSequence;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("LACTION", typeof(string), 40, true)]
        public string LastAction;

        /// <summary>
        /// ǰһ��������
        /// </summary>
        [FieldMapAttribute("TCARDSEQ", typeof(decimal), 10, true)]
        public decimal TranslateCardSequence;

        /// <summary>
        /// Good/NG/Reject/OutLine
        /// </summary>
        [FieldMapAttribute("PRODUCTSTATUS", typeof(string), 40, true)]
        public string ProductStatus;

        /// <summary>
        /// ����NG�Ĵ���
        /// </summary>
        [FieldMapAttribute("NGTimes", typeof(decimal), 10, true)]
        public decimal NGTimes;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute2", typeof(string), 40, false)]
        public string EAttribute2;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string LOTNO;

        /// <summary>
        /// ��Դ����  �������⹤��
        /// </summary>
        [FieldMapAttribute("FROMOP", typeof(string), 40, false)]
        public string FromOP;

        /// <summary>
        /// ��Դ;��  �������⹤��
        /// </summary>
        [FieldMapAttribute("FROMROUTE", typeof(string), 40, false)]
        public string FromRoute;

        /// <summary>
        /// �������   Ĭ��Ϊ1
        /// </summary>
        [FieldMapAttribute("IDMERGERULE", typeof(decimal), 10, true)]
        public decimal IDMergeRule;

        /// <summary>
        /// ����
        /// Good;NG;
        /// </summary>
        [FieldMapAttribute("ACTIONLIST", typeof(string), 100, false)]
        public string ActionList;

        /// <summary>
        /// ��һ������
        /// </summary>
        [FieldMapAttribute("SCARD", typeof(string), 40, true)]
        public string SourceCard;

        /// <summary>
        /// ��һ����������
        /// </summary>
        [FieldMapAttribute("SCARDSEQ", typeof(decimal), 10, true)]
        public decimal SourceCardSequence;

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

        /// <summary>
        /// �Ƿ�ͣ��
        /// </summary>
        [FieldMapAttribute("IsHold", typeof(int), 10, true)]
        public int IsHold;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("SHELFNO", typeof(string), 40, false)]
        public string ShelfNO;

        /// <summary>
        /// RMA����
        /// </summary>
        [FieldMapAttribute("RMABILLCODE", typeof(string), 40, false)]
        public string RMABillCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;
    }
    #endregion

    #region OnWIP
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLONWIP", "RCARD,RCARDSEQ,MOCODE")]
    public class OnWIP : DomainObject
    {
        public OnWIP()
        {
        }

        /// <summary>
        /// ManufactoryID
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

        /// <summary>
        /// �ռ�ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// �ռ�����
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// ������Ա
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// Default : A
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
        public decimal RunningCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("TCARD", typeof(string), 40, true)]
        public string TranslateCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SCARD", typeof(string), 40, true)]
        public string SourceCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ACTION", typeof(string), 40, true)]
        public string Action;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ACTIONRESULT", typeof(string), 40, true)]
        public string ActionResult;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SHIFTDAY", typeof(int), 8, true)]
        public int ShiftDay;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("TCARDSEQ", typeof(decimal), 10, true)]
        public decimal TranslateCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SCARDSEQ", typeof(decimal), 10, true)]
        public decimal SourceCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("NGTIMES", typeof(decimal), 10, true)]
        public decimal NGTimes;

        /// <summary>
        /// �Ϻ�
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// ʱ��δ���
        /// </summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        /// <summary>
        /// ��δ���
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        /// <summary>
        /// ��Դ����
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResourceCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OPCode;

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, false)]
        public string StepSequenceCode;

        /// <summary>
        /// ���δ���
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
        public string SegmentCode;

        /// <summary>
        /// ����;�̴���
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
        public string RouteCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("SHELFNO", typeof(string), 40, false)]
        public string ShelfNO;

        /// <summary>
        /// RMA����
        /// </summary>
        [FieldMapAttribute("RMABILLCODE", typeof(string), 40, false)]
        public string RMABillCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;

        public override string ToString()
        {
            System.Text.StringBuilder Debugstr = new System.Text.StringBuilder();
            Debugstr.Append(this.RunningCard + SimulationConst.spaceMark);
            Debugstr.Append(this.MaintainTime + SimulationConst.spaceMark);
            Debugstr.Append(this.MaintainDate + SimulationConst.spaceMark);
            Debugstr.Append(this.MaintainUser + SimulationConst.spaceMark);
            Debugstr.Append(this.RunningCardSequence + SimulationConst.spaceMark);
            Debugstr.Append(this.ModelCode + SimulationConst.spaceMark);
            Debugstr.Append(this.MOCode + SimulationConst.spaceMark);
            Debugstr.Append(this.TranslateCard + SimulationConst.spaceMark);
            Debugstr.Append(this.SourceCard + SimulationConst.spaceMark);
            Debugstr.Append(this.Action + SimulationConst.spaceMark);
            Debugstr.Append(this.ActionResult + SimulationConst.spaceMark);
            Debugstr.Append(this.EAttribute1 + SimulationConst.spaceMark);
            Debugstr.Append(this.ShiftDay + SimulationConst.spaceMark);
            Debugstr.Append(this.TranslateCardSequence + SimulationConst.spaceMark);
            Debugstr.Append(this.SourceCardSequence + SimulationConst.spaceMark);
            Debugstr.Append(this.NGTimes + SimulationConst.spaceMark);
            Debugstr.Append(this.ItemCode + SimulationConst.spaceMark);
            Debugstr.Append(this.TimePeriodCode + SimulationConst.spaceMark);
            Debugstr.Append(this.ShiftCode + SimulationConst.spaceMark);
            Debugstr.Append(this.ShiftTypeCode + SimulationConst.spaceMark);
            Debugstr.Append(this.ResourceCode + SimulationConst.spaceMark);
            Debugstr.Append(this.OPCode + SimulationConst.spaceMark);
            Debugstr.Append(this.StepSequenceCode + SimulationConst.spaceMark);
            Debugstr.Append(this.SegmentCode + SimulationConst.spaceMark);
            Debugstr.Append(this.RouteCode + SimulationConst.spaceMark);
            return Debugstr.ToString();
        }
    }
    #endregion

    #region SimulationReport
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLSIMULATIONREPORT", "RCARD,MOCODE")]
    public class SimulationReport : DomainObject
    {
        public SimulationReport()
        {
        }

        /// <summary>
        /// ManufactoryID
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

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

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
        public string RouteCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
        public string SegmentCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, false)]
        public string StepSequenceCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OPCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResourceCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// 0 no Complete
        /// 1 completed
        /// </summary>
        [FieldMapAttribute("ISCOM", typeof(string), 40, true)]
        public string IsComplete;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("CARTONCODE", typeof(string), 40, false)]
        public string CartonCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("PALLETCODE", typeof(string), 40, false)]
        public string PalletCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("TCARD", typeof(string), 40, true)]
        public string TranslateCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SCARD", typeof(string), 40, true)]
        public string SourceCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
        public decimal RunningCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("LACTION", typeof(string), 40, true)]
        public string LastAction;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SHIFTDAY", typeof(int), 8, true)]
        public int ShiftDay;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("TCARDSEQ", typeof(decimal), 10, true)]
        public decimal TranslateCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SCARDSEQ", typeof(decimal), 10, true)]
        public decimal SourceCardSequence;

        /// <summary>
        /// Break/Normal
        /// </summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string Status;

        /// <summary>
        /// ����NG�Ĵ���
        /// </summary>
        [FieldMapAttribute("NGTimes", typeof(decimal), 10, true)]
        public decimal NGTimes;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute2", typeof(string), 40, false)]
        public string EAttribute2;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string LOTNO;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("IDMERGERULE", typeof(decimal), 10, true)]
        public decimal IDMergeRule;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        /// <summary>
        /// ��δ���
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        /// <summary>
        /// ʱ��δ���
        /// </summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("SHELFNO", typeof(string), 40, false)]
        public string ShelfNO;

        /// <summary>
        /// RMA����
        /// </summary>
        [FieldMapAttribute("RMABILLCODE", typeof(string), 40, false)]
        public string RMABillCode;

        /// <summary>
        /// �Ƿ�Ϊ���õ�KeyPart��Ϊ1ʱΪ���ã�����Ϊ����
        /// </summary>
        [FieldMapAttribute("ISLOADEDPART", typeof(string), 40, false)]
        public string IsLoadedPart;

        /// <summary>
        /// �����к���ΪKeyPart���ϵ������к�
        /// </summary>
        [FieldMapAttribute("LOADEDRCARD", typeof(string), 40, false)]
        public string LoadedRCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;

        public override string ToString()
        {
            System.Text.StringBuilder Debugstr = new System.Text.StringBuilder();
            Debugstr.Append(this.RunningCard + SimulationConst.spaceMark);
            Debugstr.Append(this.MaintainUser + SimulationConst.spaceMark);
            Debugstr.Append(this.MaintainDate + SimulationConst.spaceMark);
            Debugstr.Append(this.MaintainTime + SimulationConst.spaceMark);
            Debugstr.Append(this.MOCode + SimulationConst.spaceMark);
            Debugstr.Append(this.RouteCode + SimulationConst.spaceMark);
            Debugstr.Append(this.SegmentCode + SimulationConst.spaceMark);
            Debugstr.Append(this.StepSequenceCode + SimulationConst.spaceMark);
            Debugstr.Append(this.OPCode + SimulationConst.spaceMark);
            Debugstr.Append(this.ResourceCode + SimulationConst.spaceMark);
            Debugstr.Append(this.ItemCode + SimulationConst.spaceMark);
            Debugstr.Append(this.IsComplete + SimulationConst.spaceMark);
            Debugstr.Append(this.CartonCode + SimulationConst.spaceMark);
            Debugstr.Append(this.PalletCode + SimulationConst.spaceMark);
            Debugstr.Append(this.EAttribute1 + SimulationConst.spaceMark);
            Debugstr.Append(this.ModelCode + SimulationConst.spaceMark);
            Debugstr.Append(this.TranslateCard + SimulationConst.spaceMark);
            Debugstr.Append(this.SourceCard + SimulationConst.spaceMark);
            Debugstr.Append(this.RunningCardSequence + SimulationConst.spaceMark);
            Debugstr.Append(this.LastAction + SimulationConst.spaceMark);
            Debugstr.Append(this.ShiftDay + SimulationConst.spaceMark);
            Debugstr.Append(this.TranslateCardSequence + SimulationConst.spaceMark);
            Debugstr.Append(this.SourceCardSequence + SimulationConst.spaceMark);
            Debugstr.Append(this.Status + SimulationConst.spaceMark);
            Debugstr.Append(this.NGTimes + SimulationConst.spaceMark);
            Debugstr.Append(this.EAttribute2 + SimulationConst.spaceMark);
            Debugstr.Append(this.LOTNO + SimulationConst.spaceMark);
            Debugstr.Append(this.IDMergeRule + SimulationConst.spaceMark);
            Debugstr.Append(this.ShiftTypeCode + SimulationConst.spaceMark);
            Debugstr.Append(this.ShiftCode + SimulationConst.spaceMark);
            Debugstr.Append(this.TimePeriodCode + SimulationConst.spaceMark);
            return Debugstr.ToString();
        }
    }
    #endregion

    #region OnWIPCardTransfer
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLONWIPCARDTRANS", "RCARD,MOCODE,RCARDSEQ")]
    public class OnWIPCardTransfer : DomainObject
    {
        public OnWIPCardTransfer()
        {
        }

        /// <summary>
        /// ManufactoryID
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

        /// <summary>
        /// �ռ�ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// �ռ�����
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// ������Ա
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// Default : A
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
        public decimal RunningCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("TCARD", typeof(string), 40, true)]
        public string TranslateCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SCARD", typeof(string), 40, true)]
        public string SourceCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("IDMERGETYPE", typeof(string), 40, true)]
        public string IDMergeType;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// �Ϻ�
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// ��Դ����
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResourceCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OPCode;

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, false)]
        public string StepSequenceCode;

        /// <summary>
        /// ���δ���
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
        public string SegmnetCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("TCARDSEQ", typeof(decimal), 10, true)]
        public decimal TranslateCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SCARDSEQ", typeof(decimal), 10, true)]
        public decimal SourceCardSequence;

        /// <summary>
        /// ����;�̴���
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
        public string RouteCode;

        /// <summary>
        /// ʱ��δ���
        /// </summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        /// <summary>
        /// ��δ���
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;

    }
    #endregion

    #region OnWIPECN
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLONWIPECN", "RCARD,RCARDSEQ,MOCODE,ECNNO")]
    public class OnWIPECN : DomainObject
    {
        public OnWIPECN()
        {
        }

        /// <summary>
        /// MNID
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

        /// <summary>
        /// ���
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
        public decimal RunningCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// ��Ʒ�Ϻ�
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ʱ��δ���
        /// </summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        /// <summary>
        /// ��δ���
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        /// <summary>
        /// ��Դ����
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string ResourceCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string OPCode;

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string StepSequenceCode;

        /// <summary>
        /// ���δ���
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string SegmnetCode;

        /// <summary>
        /// ����;�̴���
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string RouteCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ECNNO", typeof(string), 40, true)]
        public string ECNNO;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;

    }
    #endregion

    #region OnWIPSoftVersion
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLONWIPSOFTVER", "RCARD,RCARDSEQ,MOCODE,SOFTVER")]
    public class OnWIPSoftVersion : DomainObject
    {
        public OnWIPSoftVersion()
        {
        }

        /// <summary>
        /// MNID
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

        /// <summary>
        /// ���
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
        public decimal RunningCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// ��Ʒ�Ϻ�
        /// </summary>
        [FieldMapAttribute("SOFTVER", typeof(string), 40, true)]
        public string SoftwareVersion;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SOFTNAME", typeof(string), 40, false)]
        public string SoftwareName;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ʱ��δ���
        /// </summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        /// <summary>
        /// ��δ���
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        /// <summary>
        /// ��Դ����
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string ResourceCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string OPCode;

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string StepSequenceCode;

        /// <summary>
        /// ���δ���
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string SegmnetCode;

        /// <summary>
        /// ����;�̴���
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string RouteCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;

    }

    //��ѯ����
    public class OnWIPSoftVersionError : VersionError
    {
        public OnWIPSoftVersionError()
        {
        }

        /// <summary>
        /// ���
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
        public decimal RunningCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SOFTNAME", typeof(string), 40, false)]
        public string SoftwareName;

        /// <summary>
        /// ʱ��δ���
        /// </summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        /// <summary>
        /// ��δ���
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        /// <summary>
        /// ��Դ����
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string ResourceCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string OPCode;

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string StepSequenceCode;

        /// <summary>
        /// ���δ���
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string SegmnetCode;

        /// <summary>
        /// ����;�̴���
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string RouteCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

    }
    #endregion

    #region OnWIPTRY
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLONWIPTRY", "RCARD,RCARDSEQ,MOCODE,TRYNO")]
    public class OnWIPTRY : DomainObject
    {
        public OnWIPTRY()
        {
        }

        /// <summary>
        /// MNID
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

        /// <summary>
        /// ���
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
        public decimal RunningCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// ��Ʒ�Ϻ�
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ʱ��δ���
        /// </summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        /// <summary>
        /// ��δ���
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        /// <summary>
        /// ��Դ����
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string ResourceCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string OPCode;

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string StepSequenceCode;

        /// <summary>
        /// ���δ���
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string SegmnetCode;

        /// <summary>
        /// ����;�̴���
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string RouteCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("TRYNO", typeof(string), 40, true)]
        public string TRYNO;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;

    }
    #endregion

    #region OnWIPItem
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLONWIPITEM", "RCARD,RCARDSEQ,MOCODE,MSEQ")]
    public class OnWIPItem : DomainObject
    {
        public OnWIPItem()
        {
        }

        /// <summary>
        /// MNID
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

        /// <summary>
        /// Running Card 0
        /// Integration NO  1
        /// </summary>
        [FieldMapAttribute("MCARDTYPE", typeof(string), 40, true)]
        public string MCardType;

        /// <summary>
        /// ���
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
        public decimal RunningCardSequence;

        /// <summary>
        /// INNO/keparts
        /// </summary>
        [FieldMapAttribute("MCARD", typeof(string), 40, false)]
        public string MCARD;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// ��Ʒ�Ϻ�
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MITEMCODE", typeof(string), 40, false)]
        public string MItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ʱ��δ���
        /// </summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        /// <summary>
        /// ��δ���
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        /// <summary>
        /// ��Դ����
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string ResourceCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string OPCode;

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string StepSequenceCode;

        /// <summary>
        /// ���δ���
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string SegmentCode;

        /// <summary>
        /// ����;�̴���
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string RouteCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("TRANSSTATUS", typeof(string), 40, true)]
        public string TransactionStatus;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string LotNO;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("PCBA", typeof(string), 40, false)]
        public string PCBA;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("BIOS", typeof(string), 40, false)]
        public string BIOS;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("VERSION", typeof(string), 40, false)]
        public string Version;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("VENDORITEMCODE", typeof(string), 40, false)]
        public string VendorItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, false)]
        public string VendorCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("DATECODE", typeof(string), 40, false)]
        public string DateCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MSEQ", typeof(decimal), 10, true)]
        public decimal MSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("Qty", typeof(decimal), 10, true)]
        public decimal Qty;

        //		/// <summary>
        //		/// ��������
        //		/// </summary>
        //		[FieldMapAttribute("", typeof(decimal), 10, true)]
        //		public decimal  DropQty;

        /// <summary>
        /// 0,����
        /// 1,����
        /// </summary>
        [FieldMapAttribute("ActionType", typeof(int), 10, false)]
        public int ActionType;

        /// <summary>
        /// ����OP
        /// </summary>
        [FieldMapAttribute("DropOP", typeof(string), 40, false)]
        public string DropOP;

        /// <summary>
        /// �����û�
        /// </summary>
        [FieldMapAttribute("DropUser", typeof(string), 40, false)]
        public string DropUser;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("DropDate", typeof(int), 8, false)]
        public int DropDate;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [FieldMapAttribute("DropTime", typeof(int), 6, false)]
        public int DropTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MOSEQ", typeof(decimal), 10, true)]
        public decimal MOSeq;
    }
    #endregion

    #region VersionCollect
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLVersionCollect", "Rcard")]
    public class VersionCollect : DomainObject
    {
        public VersionCollect()
        {
        }

        /// <summary>
        /// ���̿���
        /// </summary>
        [FieldMapAttribute("Rcard", typeof(string), 40, true)]
        public string Rcard;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("Mocode", typeof(string), 40, false)]
        public string Mocode;

        /// <summary>
        /// �汾��Ϣ
        /// </summary>
        [FieldMapAttribute("VersionInfo", typeof(string), 100, true)]
        public string VersionInfo;

        /// <summary>
        /// �û�
        /// </summary>
        [FieldMapAttribute("MUser", typeof(string), 40, true)]
        public string MUser;

        /// <summary>
        /// ά������
        /// </summary>
        [FieldMapAttribute("MDate", typeof(int), 8, true)]
        public int MDate;

        /// <summary>
        /// ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTime", typeof(int), 6, true)]
        public int MTime;

        /// <summary>
        /// ��ע
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
        public string EAttribute1;

    }
    #endregion

    #region VersionError
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLVersionError", "PKID,Rcard,Mocode")]
    public class VersionError : DomainObject
    {
        public VersionError()
        {
        }

        /// <summary>
        /// ���̿���
        /// </summary>
        [FieldMapAttribute("PKID", typeof(string), 40, true)]
        public string PKID;
        /// <summary>
        /// ���̿���
        /// </summary>
        [FieldMapAttribute("Rcard", typeof(string), 40, true)]
        public string Rcard;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("Mocode", typeof(string), 40, true)]
        public string Mocode;

        /// <summary>
        /// �汾��Ϣ
        /// </summary>
        [FieldMapAttribute("VersionInfo", typeof(string), 100, true)]
        public string VersionInfo;

        /// <summary>
        /// �û�
        /// </summary>
        [FieldMapAttribute("MUser", typeof(string), 40, true)]
        public string MUser;

        /// <summary>
        /// ά������
        /// </summary>
        [FieldMapAttribute("MDate", typeof(int), 8, true)]
        public int MDate;

        /// <summary>
        /// ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTime", typeof(int), 6, true)]
        public int MTime;

        /// <summary>
        /// ��ע
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
        public string EAttribute1;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MoVersionInfo", typeof(string), 100, true)]
        public string MoVersionInfo;

    }
    #endregion

    #region ConfigInfo
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLConfigInfo", "RCARD,CheckItemCode,CatergoryCode")]
    public class ConfigInfo : DomainObject
    {
        public ConfigInfo()
        {
        }

        /// <summary>
        /// MNID
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("CheckItemCode", typeof(string), 40, true)]
        public string CheckItemCode;

        /// <summary>
        /// ��Ʒ�Ϻ�
        /// </summary>
        [FieldMapAttribute("CatergoryCode", typeof(string), 40, true)]
        public string CatergoryCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// �����ֵ
        /// </summary>
        [FieldMapAttribute("CheckItemVlaue", typeof(string), 10, false)]
        public string CheckItemVlaue;

    }
    #endregion

    #region OnWipConfigCollect
    /// <summary>
    /// 
    /// </summary>
    [Serializable, TableMap("TBLONWIPCFGCOLLECT", "PKID")]
    public class OnWipConfigCollect : DomainObject
    {
        public OnWipConfigCollect()
        {
        }

        /// <summary>
        /// MNID
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, false)]
        public string RunningCard;

        /// <summary>
        /// ���
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, false)]
        public decimal RunningCardSequence;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("CheckItemCode", typeof(string), 40, false)]
        public string CheckItemCode;

        /// <summary>
        /// ��Ʒ�Ϻ�
        /// </summary>
        [FieldMapAttribute("CatergoryCode", typeof(string), 40, false)]
        public string CatergoryCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, true)]
        public string EAttribute1;

        /// <summary>
        /// ʱ��δ���
        /// </summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        /// <summary>
        /// ��δ���
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        /// <summary>
        /// ���ƴ���
        /// </summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        /// <summary>
        /// ��Դ����
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string ResourceCode;

        /// <summary>
        /// �������
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string OPCode;

        /// <summary>
        /// ���������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string StepSequenceCode;

        /// <summary>
        /// ���δ���
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string SegmnetCode;

        /// <summary>
        /// ����;�̴���
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
        public string RouteCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ParentCode", typeof(string), 40, false)]
        public string ParentCode;

        /// <summary>
        /// �����ֵ
        /// </summary>
        [FieldMapAttribute("CheckItemVlaue", typeof(string), 40, false)]
        public string CheckItemVlaue;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MoCode", typeof(string), 40, false)]
        public string MoCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("PKID", typeof(string), 40, true)]
        public string PKID;

        /// <summary>
        /// ʵ�ʲɼ�ֵ
        /// </summary>
        [FieldMapAttribute("ActValue", typeof(string), 40, true)]
        public string ActValue;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("ItemConfig", typeof(string), 40, true)]
        public string ItemConfig;

    }
    #endregion

    #region RMARCARD
    /// <summary>
    /// RMA������Ϣ
    /// </summary>
    [Serializable, TableMap("TBLRMARCARD", "RMAPKID")]
    public class RMARCARD : DomainObject
    {
        public RMARCARD()
        {
        }

        /// <summary>
        /// ��Ʒ���к�
        /// </summary>
        [FieldMapAttribute("RMAPKID", typeof(string), 40, true)]
        public string RMAPKID;
        /// <summary>
        /// ��Ʒ���к�
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, false)]
        public string RCARD;

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ITEMCODE;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("REWORKMOCODE", typeof(string), 40, false)]
        public string REWORKMOCODE;

        /// <summary>
        /// RMA���ͣ�REWORK/TS
        /// </summary>
        [FieldMapAttribute("RMATYPE", typeof(string), 40, true)]
        public string RMATYPE;

        /// <summary>
        /// RMA����
        /// </summary>
        [FieldMapAttribute("RMABILLNO", typeof(string), 40, true)]
        public string RMABILLNO;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 100, false)]
        public string EATTRIBUTE1;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MDATE;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MTIME;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SSCOE", typeof(string), 40, false)]
        public string SSCOE;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string RESCODE;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
        public string SEGCODE;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SHIFTDAY", typeof(int), 8, false)]
        public int SHIFTDAY;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
        public string ROUTECODE;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OPCODE;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, false)]
        public string MODELCODE;

    }
    #endregion

    #region WorkingError

    /// <summary>
    ///	WorkingError
    /// </summary>
    [Serializable, TableMap("TBLWORKINGERROR", "RESCODE,CUSER,CDATE,CTIME")]
    public class WorkingError : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public WorkingError()
        {
        }

        ///<summary>
        ///SegmentCode
        ///</summary>	
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string SegmentCode;

        ///<summary>
        ///StepSequenceCode
        ///</summary>	
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string StepSequenceCode;

        ///<summary>
        ///ResourceCode
        ///</summary>	
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResourceCode;

        ///<summary>
        ///InputContent
        ///</summary>	
        [FieldMapAttribute("INPUTCONTENT", typeof(string), 200, true)]
        public string InputContent;

        ///<summary>
        ///Function
        ///</summary>	
        [FieldMapAttribute("FUNCTION", typeof(string), 200, true)]
        public string Function;

        ///<summary>
        ///FunctionType
        ///</summary>	
        [FieldMapAttribute("FUNCTIONTYPE", typeof(string), 40, false)]
        public string FunctionType;

        ///<summary>
        ///ErrorMessage
        ///</summary>	
        [FieldMapAttribute("ERRORMSG", typeof(string), 500, true)]
        public string ErrorMessage;

        ///<summary>
        ///ErrorMessageCode
        ///</summary>	
        [FieldMapAttribute("ERRORMSGCODE", typeof(string), 200, true)]
        public string ErrorMessageCode;

        ///<summary>
        ///ShiftTypeCode
        ///</summary>	
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, true)]
        public string ShiftTypeCode;

        ///<summary>
        ///ShiftCode
        ///</summary>	
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
        public string ShiftCode;

        ///<summary>
        ///TimePeriodCode
        ///</summary>	
        [FieldMapAttribute("TPCODE", typeof(string), 40, true)]
        public string TimePeriodCode;

        ///<summary>
        ///ShiftDay
        ///</summary>	
        [FieldMapAttribute("SHIFTDAY", typeof(int), 8, true)]
        public int ShiftDay;

        ///<summary>
        ///CreateUser
        ///</summary>	
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CreateUser;

        ///<summary>
        ///CreateDate
        ///</summary>	
        [FieldMapAttribute("CDATE", typeof(int), 8, false)]
        public int CreateDate;

        ///<summary>
        ///CreateTime
        ///</summary>	
        [FieldMapAttribute("CTIME", typeof(int), 6, false)]
        public int CreateTime;

        ///<summary>
        ///Status
        ///</summary>	
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///MaintainUser
        ///</summary>	
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>	
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>	
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        ///<summary>
        ///EAttribute1
        ///</summary>	
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string EAttribute1;

    }

    #endregion

    #region SimulationConst

    public class SimulationConst
    {
        public const string spaceMark = "\t";
        public static bool Debug = true;
    }

    #endregion

    #region Down

    /// <summary>
    ///	Down
    /// </summary>
    [Serializable, TableMap("TBLDOWN", "DOWNCODE,RCARD")]
    public class Down : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Down()
        {
        }

        ///<summary>
        ///DOWNCODE
        ///</summary>	
        [FieldMapAttribute("DOWNCODE", typeof(string), 40, false)]
        public string DownCode;

        ///<summary>
        ///RCard
        ///</summary>	
        [FieldMapAttribute("RCARD", typeof(string), 40, false)]
        public string RCard;

        ///<summary>
        ///MOCode
        ///</summary>	
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string MOCode;

        ///<summary>
        ///ModelCode
        ///</summary>	
        [FieldMapAttribute("MODELCODE", typeof(string), 40, false)]
        public string ModelCode;

        ///<summary>
        ///ItemCode
        ///</summary>	
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        ///<summary>
        ///SSCODE
        ///</summary>	
        [FieldMapAttribute("SSCODE", typeof(string), 40, false)]
        public string SSCode;

        ///<summary>
        ///RESCODE
        ///</summary>	
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResCode;


        ///<summary>
        ///DOWNSTATUS
        ///</summary>	
        [FieldMapAttribute("DOWNSTATUS", typeof(string), 40, false)]
        public string DownStatus;

        ///<summary>
        ///DOWNREASON
        ///</summary>	
        [FieldMapAttribute("DOWNREASON", typeof(string), 100, true)]
        public string DownReason;

        ///<summary>
        ///DOWNDATE
        ///</summary>	
        [FieldMapAttribute("DOWNDATE", typeof(int), 8, true)]
        public int DownDate;

        ///<summary>
        ///DOWNSHIFTDATE
        ///</summary>	
        [FieldMapAttribute("DOWNSHIFTDATE", typeof(int), 8, true)]
        public int DownShiftDate;

        ///<summary>
        ///DOWNTIME
        ///</summary>	
        [FieldMapAttribute("DOWNTIME", typeof(int), 6, true)]
        public int DownTime;

        ///<summary>
        ///DOWNBY
        ///</summary>	
        [FieldMapAttribute("DOWNBY", typeof(string), 40, true)]
        public string DownBy;

        ///<summary>
        ///UPREASON
        ///</summary>	
        [FieldMapAttribute("UPREASON", typeof(string), 100, true)]
        public string UPReason;

        ///<summary>
        ///UPDATE_
        ///</summary>	
        [FieldMapAttribute("UPDAY", typeof(int), 8, true)]
        public int UPDATE_;

        ///<summary>
        ///UPTIME
        ///</summary>	
        [FieldMapAttribute("UPTIME", typeof(int), 6, true)]
        public int UPTIME;

        ///<summary>
        ///UPBY
        ///</summary>	
        [FieldMapAttribute("UPBY", typeof(string), 40, true)]
        public string UPBY;

        ///<summary>
        ///MaintainUser
        ///</summary>	
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>	
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>	
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        ///<summary>
        ///EAttribute1
        ///</summary>	
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string EAttribute1;

        ///<summary>
        ///ORGID
        ///</summary>	
        [FieldMapAttribute("ORGID", typeof(int), 8, true)]
        public int ORGID;

    }

    [Serializable]
    public class DownWithRCardInfo : Down
    {
        [FieldMapAttribute("DOWNQTY", typeof(int), 8, true)]
        public int DownQuantity;

        [FieldMapAttribute("ITEMDESC", typeof(string), 100, true)]
        public string ItemDescription;
    }
    #endregion

    #region MACInfo

    /// <summary>
    /// MACInfo
    /// </summary>
    [Serializable, TableMap("TBLMACINFO", "MOCODE, RCARD")]
    public class MACInfo : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public MACInfo()
        {
        }

        ///<summary>
        ///MOCode
        ///</summary>	
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string MOCode;

        ///<summary>
        ///RCard
        ///</summary>	
        [FieldMapAttribute("RCARD", typeof(string), 40, false)]
        public string RCard;

        ///<summary>
        ///RCardSeq
        ///</summary>	
        [FieldMapAttribute("RCARDSEQ", typeof(int), 10, false)]
        public int RCardSeq;

        ///<summary>
        ///MACID
        ///</summary>	
        [FieldMapAttribute("MACID", typeof(string), 100, false)]
        public string MACID;

        ///<summary>
        ///MACAddress
        ///</summary>	
        [FieldMapAttribute("MACADDRESS", typeof(string), 100, false)]
        public string MACAddress;

        ///<summary>
        ///RouteCode
        ///</summary>	
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
        public string RouteCode;

        ///<summary>
        ///OPCode
        ///</summary>	
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OPCode;

        ///<summary>
        ///SegmentCode
        ///</summary>	
        [FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
        public string SegmentCode;

        ///<summary>
        ///SSCode
        ///</summary>	
        [FieldMapAttribute("SSCODE", typeof(string), 40, false)]
        public string SSCode;

        ///<summary>
        ///ResourceCode
        ///</summary>	
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResourceCode;

        ///<summary>
        ///MaintainUser
        ///</summary>	
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>	
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>	
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

    }

    #endregion


    #region IDInfo

    /// <summary>
    /// MACInfo
    /// </summary>
    [Serializable, TableMap("TBLIDINFO", "MOCODE, RCARD")]
    public class IDInfo : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public IDInfo()
        {
        }

        ///<summary>
        ///MOCode
        ///</summary>	
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string MOCode;

        ///<summary>
        ///RCard
        ///</summary>	
        [FieldMapAttribute("RCARD", typeof(string), 40, false)]
        public string RCard;

        ///<summary>
        ///RCardSeq
        ///</summary>	
        [FieldMapAttribute("RCARDSEQ", typeof(int), 10, false)]
        public int RCardSeq;

        ///<summary>
        ///ID1
        ///</summary>	
        [FieldMapAttribute("ID1", typeof(string), 100, true)]
        public string ID1;

        ///<summary>
        ///ID1
        ///</summary>	
        [FieldMapAttribute("ID2", typeof(string), 100, true)]
        public string ID2;

        ///<summary>
        ///ID1
        ///</summary>	
        [FieldMapAttribute("ID3", typeof(string), 100, true)]
        public string ID3;

        ///<summary>
        ///RouteCode
        ///</summary>	
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, false)]
        public string RouteCode;

        ///<summary>
        ///OPCode
        ///</summary>	
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OPCode;

        ///<summary>
        ///SegmentCode
        ///</summary>	
        [FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
        public string SegmentCode;

        ///<summary>
        ///SSCode
        ///</summary>	
        [FieldMapAttribute("SSCODE", typeof(string), 40, false)]
        public string SSCode;

        ///<summary>
        ///ResourceCode
        ///</summary>	
        [FieldMapAttribute("RESCODE", typeof(string), 40, false)]
        public string ResourceCode;

        ///<summary>
        ///MaintainUser
        ///</summary>	
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>	
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>	
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

    }

    #endregion

    #region SPLITBOARD
    /// <summary>
    /// TBLSPLITBOARD
    /// </summary>
    [Serializable, TableMap("TBLSPLITBOARD", "MOCODE,RCARD")]
    public class SplitBoard : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public SplitBoard()
        {
        }

        ///<summary>
        ///RCARD
        ///</summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, false)]
        public string Rcard;

        ///<summary>
        ///MOCODE
        ///</summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string Mocode;

        ///<summary>
        ///SEQ
        ///</summary>
        [FieldMapAttribute("SEQ", typeof(decimal), 22, false)]
        public decimal Seq;

        ///<summary>
        ///SCARD
        ///</summary>
        [FieldMapAttribute("SCARD", typeof(string), 40, false)]
        public string Scard;

        ///<summary>
        ///MODELCODE
        ///</summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, false)]
        public string Modelcode;

        ///<summary>
        ///ITEMCODE
        ///</summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string Itemcode;

        ///<summary>
        ///ROUTECODE
        ///</summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string Routecode;

        ///<summary>
        ///OPCODE
        ///</summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string Opcode;

        ///<summary>
        ///SEGCODE
        ///</summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string Segcode;

        ///<summary>
        ///SSCODE
        ///</summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string Sscode;

        ///<summary>
        ///RESCODE
        ///</summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string Rescode;

        ///<summary>
        ///SHIFTTYPECODE
        ///</summary>
        [FieldMapAttribute("SHIFTTYPECODE", typeof(string), 40, false)]
        public string Shifttypecode;

        ///<summary>
        ///SHIFTCODE
        ///</summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, false)]
        public string Shiftcode;

        ///<summary>
        ///TPCODE
        ///</summary>
        [FieldMapAttribute("TPCODE", typeof(string), 40, false)]
        public string Tpcode;

        ///<summary>
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 22, false)]
        public string Muser;

        ///<summary>
        ///MDATE
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int Mdate;

        ///<summary>
        ///MTIME
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int Mtime;

        ///<summary>
        ///EATTRIBUTE1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;



    }
    #endregion

    #region Distolinehead
    /// <summary>
    /// TBLDISTOLINEHEAD
    /// </summary>
    [Serializable, TableMap("TBLDISTOLINEHEAD", "MOCODE,MCODE")]
    public class DisToLineHead : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public DisToLineHead()
        {
        }

        ///<summary>
        ///MoCode
        ///</summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string MoCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///MName
        ///</summary>
        [FieldMapAttribute("MNAME", typeof(string), 200, true)]
        public string MName;

        ///<summary>
        ///MplanQty
        ///</summary>
        [FieldMapAttribute("MPLANQTY", typeof(decimal), 22, false)]
        public decimal MplanQty;

        ///<summary>
        ///MdisQty
        ///</summary>
        [FieldMapAttribute("MDISQTY", typeof(decimal), 22, false)]
        public decimal MdisQty;

        ///<summary>
        ///ItemCode
        ///</summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        ///<summary>
        ///MoplanQty
        ///</summary>
        [FieldMapAttribute("MOPLANQTY", typeof(decimal), 22, false)]
        public decimal MoplanQty;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///PEndingTime
        ///</summary>
        [FieldMapAttribute("PENDINGTIME", typeof(decimal), 22, true)]
        public decimal PEndingTime;

        ///<summary>
        ///Orgid
        ///</summary>
        [FieldMapAttribute("ORGID", typeof(int), 22, false)]
        public int Orgid;

        ///<summary>
        ///Mobom
        ///</summary>
        [FieldMapAttribute("MOBOM", typeof(string), 40, false)]
        public string Mobom;

        ///<summary>
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///Eattribute1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 200, true)]
        public string Eattribute1;

    }
    #endregion

    #region DisToLineDetail
    /// <summary>
    /// TBLDISTOLINEDETAIL
    /// </summary>
    [Serializable, TableMap("TBLDISTOLINEDETAIL", "OPCODE,MOCODE,SSCODE,MCODE")]
    public class DisToLineDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public DisToLineDetail()
        {
        }

        ///<summary>
        ///MoCode
        ///</summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string MoCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///SsCode
        ///</summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, false)]
        public string SsCode;

        ///<summary>
        ///OpCode
        ///</summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OpCode;

        ///<summary>
        ///SegCode
        ///</summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string SegCode;

        ///<summary>
        ///MssdisQty
        ///</summary>
        [FieldMapAttribute("MSSDISQTY", typeof(decimal), 22, false)]
        public decimal MssdisQty;

        ///<summary>
        ///MssleftQty
        ///</summary>
        [FieldMapAttribute("MSSLEFTQTY", typeof(decimal), 22, false)]
        public decimal MssleftQty;

        ///<summary>
        ///MssleftTime
        ///</summary>
        [FieldMapAttribute("MSSLEFTTIME", typeof(decimal), 22, false)]
        public decimal MssleftTime;

        ///<summary>
        ///MQty
        ///</summary>
        [FieldMapAttribute("MQTY", typeof(decimal), 22, false)]
        public decimal MQty;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///Eattribute
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE", typeof(string), 200, true)]
        public string Eattribute;

    }
    #endregion

    #region DisToLineList
    /// <summary>
    /// TBLDISTOLINELIST
    /// </summary>
    [Serializable, TableMap("TBLDISTOLINELIST", "SERIAL")]
    public class DisToLineList : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public DisToLineList()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///MoCode
        ///</summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string MoCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///SsCode
        ///</summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, false)]
        public string SsCode;

        ///<summary>
        ///OpCode
        ///</summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, false)]
        public string OpCode;

        ///<summary>
        ///SegCode
        ///</summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string SegCode;

        ///<summary>
        ///MssdisQty
        ///</summary>
        [FieldMapAttribute("MSSDISQTY", typeof(decimal), 22, false)]
        public decimal MssdisQty;

        ///<summary>
        ///MssleftQty
        ///</summary>
        [FieldMapAttribute("MSSLEFTQTY", typeof(decimal), 22, false)]
        public decimal MssleftQty;

        ///<summary>
        ///MssleftTime
        ///</summary>
        [FieldMapAttribute("MSSLEFTTIME", typeof(decimal), 22, false)]
        public decimal MssleftTime;

        ///<summary>
        ///MQty
        ///</summary>
        [FieldMapAttribute("MQTY", typeof(decimal), 22, false)]
        public decimal MQty;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///Delflag
        ///</summary>
        [FieldMapAttribute("DELFLAG", typeof(string), 8, false)]
        public string Delflag;

        ///<summary>
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///Eattribute
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE", typeof(string), 200, true)]
        public string Eattribute;

    }
    #endregion


}