using System;
using System.Collections.Generic;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for Warehouse
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2005-7-28 16:35:54
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.Warehouse
{
    #region Factory
    /// <summary>
    /// ����
    /// </summary>
    [Serializable, TableMap("TBLFACTORY", "FACCODE")]
    public class Factory : DomainObject
    {
        public Factory()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
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
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FactoryCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("FACDESC", typeof(string), 100, false)]
        public string FactoryDescription;

        /// <summary>
        /// ��֯���
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, true)]
        public int OrganizationID;
    }
    #endregion

    #region MOStock
    /// <summary>
    /// ��������ͳ��
    /// </summary>
    [Serializable, TableMap("TBLMOSTOCK", "ITEMCODE,MOCODE")]
    public class MOStock : DomainObject
    {
        public MOStock()
        {
        }

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
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ���Ϻ�
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// ������δ���ʵ�����
        /// </summary>
        [FieldMapAttribute("ISSUEQTY", typeof(decimal), 10, true)]
        public decimal IssueQty;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("SCRAPQTY", typeof(decimal), 10, true)]
        public decimal ScrapQty;

        /// <summary>
        /// ӯ�� (��¼������ά������)
        /// </summary>
        [FieldMapAttribute("GAINLOSE", typeof(decimal), 10, true)]
        public decimal GainLose;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// �ƻ�����
        /// </summary>
        [FieldMapAttribute("RECQTY", typeof(decimal), 10, true)]
        public decimal ReceiptQty;

        /// <summary>
        /// ��Ʒ��������
        /// </summary>
        [FieldMapAttribute("RETURNQTY", typeof(decimal), 10, true)]
        public decimal ReturnQty;

        /// <summary>
        /// ����Ʒ��������
        /// </summary>
        [FieldMapAttribute("RETURNSCRAPQTY", typeof(decimal), 10, true)]
        public decimal ReturnScrapQty;


        /// <summary>
        /// �������ϲɼ������ϵ�����
        /// </summary>
        public decimal MOLoadingQty;

        /// <summary>
        /// ά�޴˹������������ϸ����ϵ�����
        /// </summary>
        public decimal TSLoadingQty;

        /// <summary>
        /// ������δ��������
        /// </summary>
        public decimal TSUnCompletedQty;

        /// <summary>
        /// ����״̬ �����жϹ�������ʺͱ����ʼ��㹫ʽ��
        /// </summary>
        public string MOStatus;

    }
    #endregion

    #region TransactionType
    /// <summary>
    /// ��������
    /// </summary>
    [Serializable, TableMap("TBLTRANSTYPE", "TRANSTYPECODE")]
    public class TransactionType : DomainObject
    {
        public TransactionType()
        {
        }

        public class TransactionTypeMoStock
        {
            /// <summary>
            /// �������ʹ���
            /// </summary>
            public string TransactionTypeCode;
            /// <summary>
            /// �ۼƵ���attribute
            /// </summary>
            public string AttributeName;
            /// <summary>
            /// ��������(add, sub)
            /// </summary>
            public string Operation;
            /// <summary>
            /// Ŀ��ֿ� (���ڼ���������ά������)
            /// </summary>
            public string ToWarehouse;
        }
        public static TransactionTypeMoStock[] TRANSACTIONTYPE_MOSTOCK = null;
        public static System.Collections.Hashtable TRANSACTION_MAPPING = null;

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
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("TRANSTYPEESC", typeof(string), 100, false)]
        public string TransactionTypeDescription;

        /// <summary>
        /// ���ʹ���
        /// </summary>
        [FieldMapAttribute("TRANSTYPECODE", typeof(string), 40, true)]
        public string TransactionTypeCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("TRANSTYPENAME", typeof(string), 40, false)]
        public string TransactionTypeName;

        /// <summary>
        /// �Ƿ񹤵��ܿ�
        /// </summary>
        [FieldMapAttribute("ISBYMO", typeof(string), 1, true)]
        public string IsByMOControl;

        /// <summary>
        /// ǰ׺
        /// </summary>
        [FieldMapAttribute("TRANSPREFIX", typeof(string), 40, false)]
        public string TransactionPrefix;

        /// <summary>
        /// �Ƿ��ʼ���ͣ���ʼ���Ͳ�����ɾ��
        /// </summary>
        [FieldMapAttribute("ISINIT", typeof(string), 1, true)]
        public string IsInit;

    }
    #endregion

    #region Warehouse

    /// <summary>
    /// �ֿ�
    /// </summary>
    [Serializable, TableMap("TBLWAREHOURSE", "WHCODE,FACCODE")]
    public class Warehouse : DomainObject
    {
        public Warehouse()
        {
        }

        /// <summary>
        /// �ֿ����Ͳ�������
        /// </summary>
        public static string WarehouseTypeGroup = "WAREHOUSETYPE";

        /// <summary>
        /// ��ʼ״̬
        /// </summary>
        public const string WarehouseStatus_Initialize = "WarehouseStatus_Initialize";
        /// <summary>
        /// ����״̬
        /// </summary>
        public const string WarehouseStatus_Normal = "WarehouseStatus_Normal";
        /// <summary>
        /// �̵�״̬
        /// </summary>
        public const string WarehouseStatus_Cycle = "WarehouseStatus_Cycle";
        /// <summary>
        /// �ر�״̬
        /// </summary>
        public const string WarehouseStatus_Closed = "WarehouseStatus_Closed";

        /// <summary>
        /// �ֿ����
        /// </summary>
        [FieldMapAttribute("WHCODE", typeof(string), 40, true)]
        public string WarehouseCode;

        /// <summary>
        /// �ֿ�����
        /// </summary>
        [FieldMapAttribute("WHDESC", typeof(string), 100, false)]
        public string WarehouseDescription;

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
        /// �ֿ�����
        /// ����/��Ʒ������/����������/���Ʒ��
        /// </summary>
        [FieldMapAttribute("WHTYPE", typeof(string), 40, true)]
        public string WarehouseType;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        //		/// <summary>
        //		/// ���δ���
        //		/// </summary>
        //		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        //		public string  SegmentCode;

        /// <summary>
        /// ��ע
        /// </summary>
        [FieldMapAttribute("MEMO", typeof(string), 100, false)]
        public string MEMO;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FactoryCode;

        /// <summary>
        /// ����̵�����
        /// </summary>
        [FieldMapAttribute("LCYCLEDATE", typeof(int), 8, true)]
        public int LastCycleCountDate;

        /// <summary>
        /// ����̵�ʱ��
        /// </summary>
        [FieldMapAttribute("LCYCLETIME", typeof(int), 6, true)]
        public int LastCycleCountTime;

        /// <summary>
        /// �ⷿ״̬ 
        /// ��ʼ��/����/�̵�/�ر�
        /// </summary>
        [FieldMapAttribute("WHSTATUS", typeof(string), 40, true)]
        public string WarehouseStatus;

        /// <summary>
        /// ����̵����
        /// </summary>
        [FieldMapAttribute("LCYCLECODE", typeof(string), 40, false)]
        public string LastCycleCountCode;

        /// <summary>
        /// ������ʹ�ô���
        /// </summary>
        [FieldMapAttribute("USECOUNT", typeof(decimal), 10, true)]
        public decimal UseCount;

        /// <summary>
        /// �Ƿ�ܿ�
        /// </summary>
        [FieldMapAttribute("ISCTRL", typeof(string), 1, true)]
        public string IsControl;

    }
    #endregion

    #region Warehouse2StepSequence
    /// <summary>
    /// ������ֿ��Ӧ
    /// </summary>
    //[Serializable, TableMap("TBLWH2SSCODE", "SSCODE,WHCODE,SEGCODE,FACCODE")]
    [Serializable, TableMap("TBLWH2SSCODE", "SSCODE,WHCODE,FACCODE")]
    public class Warehouse2StepSequence : DomainObject
    {
        public Warehouse2StepSequence()
        {
        }

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
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ���ߴ���
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string StepSequenceCode;

        /// <summary>
        /// �ֿ����
        /// </summary>
        [FieldMapAttribute("WHCODE", typeof(string), 40, true)]
        public string WarehouseCode;

        /*
        /// <summary>
        /// ���δ���
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string  SegmentCode;
        */

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FactoryCode;

    }
    #endregion

    #region WarehouseCycleCount
    /// <summary>
    /// �ֿ��̵�����
    /// </summary>
    [Serializable, TableMap("TBLWHCYCLE", "CYCLECODE")]
    public class WarehouseCycleCount : DomainObject
    {
        public WarehouseCycleCount()
        {
        }

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
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ȷ����
        /// </summary>
        [FieldMapAttribute("CFMUSER", typeof(string), 40, false)]
        public string ConfirmUser;

        /// <summary>
        /// ȷ������
        /// </summary>
        [FieldMapAttribute("CFMDATE", typeof(int), 8, false)]
        public int ConfirmDate;

        /// <summary>
        /// ȷ��ʱ��
        /// </summary>
        [FieldMapAttribute("CFMTIME", typeof(int), 6, true)]
        public int ConfirmTime;

        /// <summary>
        /// �̵����
        /// </summary>
        [FieldMapAttribute("CYCLECODE", typeof(string), 40, true)]
        public string CycleCountCode;

        /// <summary>
        /// �̵�����
        /// ��/��/��/�����̵�
        /// 
        /// </summary>
        [FieldMapAttribute("CYCLETYPE", typeof(string), 40, true)]
        public string CycleCountType;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("SHIFTCODE", typeof(string), 40, false)]
        public string ShiftCode;

        /// <summary>
        /// �ֿ����
        /// </summary>
        [FieldMapAttribute("WHCODE", typeof(string), 40, true)]
        public string WarehouseCode;


        //		/// <summary>
        //		/// ���δ���
        //		/// </summary>
        //		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        //		public string  SegmentCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FactoryCode;

    }
    #endregion

    #region WarehouseCycleCountDetail
    /// <summary>
    /// �̵���ϸ
    /// </summary>
    [Serializable, TableMap("TBLWHCYLCEDETAIL", "ITEMCODE,CYCLECODE")]
    public class WarehouseCycleCountDetail : DomainObject
    {
        public WarehouseCycleCountDetail()
        {
        }

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
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ���ϴ���
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// �ڳ��� (�ⷿ��ɢ����)
        /// </summary>
        [FieldMapAttribute("QTY", typeof(decimal), 10, true)]
        public decimal Qty;

        /// <summary>
        /// �������� 
        /// </summary>
        [FieldMapAttribute("LINEQTY", typeof(decimal), 10, true)]
        public decimal LineQty;

        /// <summary>
        /// ������ = �ⷿ��ɢ���� �� ��������
        /// </summary>
        [FieldMapAttribute("WAREHOUSE2LINEQTY", typeof(decimal), 10, true)]
        public decimal Warehouse2LineQty;

        /// <summary>
        /// �̵���
        /// </summary>
        [FieldMapAttribute("PHQTY", typeof(decimal), 10, true)]
        public decimal PhysicalQty;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("ADJQTY", typeof(decimal), 10, true)]
        public decimal AdjustQty;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("ADJUSER", typeof(string), 40, false)]
        public string AdjustUser;

        /// <summary>
        /// ȷ����
        /// </summary>
        [FieldMapAttribute("CFMUSER", typeof(string), 40, false)]
        public string ConfirmUser;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("ADJDATE", typeof(int), 8, false)]
        public int AdjustDate;

        /// <summary>
        /// ȷ������
        /// </summary>
        [FieldMapAttribute("CFMDATE", typeof(int), 8, false)]
        public int ConfirmDate;

        /// <summary>
        /// ȷ��ʱ��
        /// </summary>
        [FieldMapAttribute("CFMTIME", typeof(int), 6, true)]
        public int ConfirmTime;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [FieldMapAttribute("ADJTIME", typeof(int), 6, true)]
        public int AdjustTime;

        /// <summary>
        /// �̵����
        /// </summary>
        [FieldMapAttribute("CYCLECODE", typeof(string), 40, true)]
        public string CycleCountCode;

        /// <summary>
        /// �ֿ����
        /// </summary>
        [FieldMapAttribute("WHCODE", typeof(string), 40, true)]
        public string WarehouseCode;

        //		/// <summary>
        //		/// ���δ���
        //		/// </summary>
        //		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        //		public string  SegmentCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FactoryCode;
    }
    #endregion

    #region WarehouseItem
    /// <summary>
    /// �ֿ���������
    /// </summary>
    [Serializable, TableMap("TBLWHITEM", "ITEMCODE")]
    public class WarehouseItem : DomainObject
    {
        public WarehouseItem()
        {
        }

        /// <summary>
        /// ������λ��������
        /// </summary>
        public static string WarehouseItemUOMGroup = "WAREHOUSEITEMUOM";

        public static string WarehouseItemControlType_Lot = "WarehouseItemControlType_Lot";
        public static string WarehouseItemControlType_Single = "WarehouseItemControlType_Single";

        /// <summary>
        /// �Ϻ�[ItemCode]
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

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
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// ��Ʒ����[ItemName]
        /// </summary>
        [FieldMapAttribute("ITEMNAME", typeof(string), 100, false)]
        public string ItemName;

        /// <summary>
        /// ������λ[ItemUOM]
        /// </summary>
        [FieldMapAttribute("ITEMUOM", typeof(string), 40, true)]
        public string ItemUOM;

        /// <summary>
        /// �ܿط�ʽ[ItemControl]
        /// </summary>
        [FieldMapAttribute("ITEMCONTROL", typeof(string), 40, false)]
        public string ItemControlType;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

    }
    #endregion

    #region WarehouseStock
    /// <summary>
    /// �ֿ�����
    /// </summary>
    //[Serializable, TableMap("TBLWHSTOCK", "ITEMCODE,WHCODE,SEGCODE,FACCODE")]
    [Serializable, TableMap("TBLWHSTOCK", "ITEMCODE,WHCODE,FACCODE")]
    public class WarehouseStock : DomainObject
    {
        public WarehouseStock()
        {
        }

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
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ���ϴ���
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// �ڳ�����
        /// </summary>
        [FieldMapAttribute("OPENQTY", typeof(decimal), 10, true)]
        public decimal OpenQty;

        /// <summary>
        /// �ֿ����
        /// </summary>
        [FieldMapAttribute("WHCODE", typeof(string), 40, true)]
        public string WarehouseCode;

        //		/// <summary>
        //		/// ���δ���
        //		/// </summary>
        //		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        //		public string  SegmentCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FactoryCode;

    }
    #endregion

    #region WarehouseTicket
    /// <summary>
    /// ���׵�����
    /// </summary>
    [Serializable, TableMap("TBLWHTKT", "TKTNO")]
    public class WarehouseTicket : DomainObject
    {
        public WarehouseTicket()
        {
        }

        public enum TransactionStatusEnum
        {
            Pending,
            Transaction,
            Closed,
            Deleted
        }

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
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ���׵���
        /// </summary>
        [FieldMapAttribute("TKTNO", typeof(string), 40, true)]
        public string TicketNo;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("TRANSTYPECODE", typeof(string), 40, true)]
        public string TransactionTypeCode;

        /// <summary>
        /// ״̬
        /// Open/Close
        /// </summary>
        [FieldMapAttribute("TRANSSTATUS", typeof(string), 40, true)]
        public string TransactionStatus;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("TRANSUSER", typeof(string), 40, false)]
        public string TransactionUser;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("TransactionDate", typeof(int), 8, false)]
        public int TransactionDate;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [FieldMapAttribute("TransactionTime", typeof(int), 6, false)]
        public int TransactionTime;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("TicketUser", typeof(string), 40, true)]
        public string TicketUser;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("TicketDate", typeof(int), 8, true)]
        public int TicketDate;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [FieldMapAttribute("TicketTime", typeof(int), 6, true)]
        public int TicketTime;

        /// <summary>
        /// ��Դ����
        /// </summary>
        [FieldMapAttribute("STCKNO", typeof(string), 40, false)]
        public string SourceTicketNo;

        /// <summary>
        /// Ŀ��ֿ�
        /// </summary>
        [FieldMapAttribute("TOWHCODE", typeof(string), 40, true)]
        public string TOWarehouseCode;

        //		/// <summary>
        //		/// Ŀ�깤��
        //		/// </summary>
        //		[FieldMapAttribute("TOSEGCODE", typeof(string), 40, true)]
        //		public string  TOSegmentCode;

        /// <summary>
        /// �ֿ����
        /// </summary>
        [FieldMapAttribute("WHCODE", typeof(string), 40, false)]
        public string WarehouseCode;

        //		/// <summary>
        //		/// ���δ���
        //		/// </summary>
        //		[FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
        //		public string  SegmentCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FactoryCode;

        /// <summary>
        /// Ŀ�깤��
        /// </summary>
        [FieldMapAttribute("TOFACCODE", typeof(string), 40, true)]
        public string TOFactoryCode;

        /// <summary>
        /// �ο�����
        /// </summary>
        [FieldMapAttribute("REFCODE", typeof(string), 40, true)]
        public string ReferenceCode;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

    }
    #endregion

    #region WarehouseTicketSeq
    /// <summary>
    /// �������к�
    /// </summary>
    [Serializable, TableMap("TBLTICKETSEQ", "NEXTSEQ")]
    public class WarehouseTicketSeq : DomainObject
    {
        public WarehouseTicketSeq()
        {
        }
        /// <summary>
        /// ���׵���
        /// </summary>
        [FieldMapAttribute("NEXTSEQ", typeof(int), 10, true)]
        public int NextSeq;

    }
    #endregion

    #region WarehouseTicketDetail
    /// <summary>
    /// ���׵���ϸ
    /// </summary>
    [Serializable, TableMap("TBLWHTKTDETAIL", "SEQ,TKTNO")]
    public class WarehouseTicketDetail : DomainObject
    {
        public WarehouseTicketDetail()
        {
        }

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
        [FieldMapAttribute("EAttribute1", typeof(string), 40, false)]
        public string EAttribute1;

        /// <summary>
        /// ���к�
        /// </summary>
        [FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
        public decimal Sequence;

        /// <summary>
        /// ���׵���
        /// </summary>
        [FieldMapAttribute("TKTNO", typeof(string), 40, true)]
        public string TicketNo;

        /// <summary>
        /// ���ϴ���
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("QTY", typeof(decimal), 15, false)]
        public decimal Qty;

        /// <summary>
        /// ʵ������
        /// </summary>
        [FieldMapAttribute("ACTQTY", typeof(decimal), 15, false)]
        public decimal ActualQty;

        /// <summary>
        /// ����״̬
        /// Open/Close
        /// </summary>
        [FieldMapAttribute("TRANSSTATUS", typeof(string), 40, false)]
        public string TransactionStatus;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("TRANSUSER", typeof(string), 40, false)]
        public string TransactionUser;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("TransactionDate", typeof(int), 8, false)]
        public int TransactionDate;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [FieldMapAttribute("TransactionTime", typeof(int), 6, false)]
        public int TransactionTime;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("TicketUser", typeof(string), 40, false)]
        public string TicketUser;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("TicketDate", typeof(int), 8, false)]
        public int TicketDate;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [FieldMapAttribute("TicketTime", typeof(int), 6, false)]
        public int TicketTime;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string MOCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("ITEMNANE", typeof(string), 40, false)]
        public string ItemName;

        /// <summary>
        /// ��Դ�ֿ������
        /// </summary>
        [FieldMapAttribute("FRMWHQTY", typeof(decimal), 15, false)]
        public decimal FromWarehouseQty;

        /// <summary>
        /// Ŀ��ֿ������
        /// </summary>
        [FieldMapAttribute("TOWHQTY", typeof(decimal), 15, false)]
        public decimal ToWarehouseQty;

    }
    #endregion

    #region WarehouseTicketQueryItem
    /// <summary>
    /// ���׵�����
    /// </summary>
    [Serializable, TableMap("TBLWHTKT", "TKTNO")]
    public class WarehouseTicketQueryItem : DomainObject
    {
        public WarehouseTicketQueryItem()
        {
        }

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("TRANSTYPECODE", typeof(string), 40, true)]
        public string TransactionTypeCode;

        /// <summary>
        /// Ŀ��ֿ�
        /// </summary>
        [FieldMapAttribute("TOWHCODE", typeof(string), 40, true)]
        public string TOWarehouseCode;

        //		/// <summary>
        //		/// Ŀ�깤��
        //		/// </summary>
        //		[FieldMapAttribute("TOSEGCODE", typeof(string), 40, true)]
        //		public string  TOSegmentCode;

        /// <summary>
        /// �ֿ����
        /// </summary>
        [FieldMapAttribute("WHCODE", typeof(string), 40, false)]
        public string WarehouseCode;

        //		/// <summary>
        //		/// ���δ���
        //		/// </summary>
        //		[FieldMapAttribute("SEGCODE", typeof(string), 40, false)]
        //		public string  SegmentCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FactoryCode;

        /// <summary>
        /// Ŀ�깤��
        /// </summary>
        [FieldMapAttribute("TOFACCODE", typeof(string), 40, true)]
        public string TOFactoryCode;

        /// <summary>
        /// ���ϱ��
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("QTY", typeof(decimal), 40, true)]
        public decimal Qty;

    }
    #endregion

    #region SAPStorageInfo

    [Serializable, TableMap("TBLSAPSTORAGEINFO", "ITEMCODE,ORGID,STORAGEID,ITEMGRADE")]
    public class SAPStorageInfo : DomainObject
    {
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        [FieldMapAttribute("DIVISION", typeof(string), 40, true)]
        public string Division;

        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

        [FieldMapAttribute("STORAGEID", typeof(string), 40, true)]
        public string StorageID;

        [FieldMapAttribute("STORAGENAME", typeof(string), 100, true)]
        public string StorageName;

        [FieldMapAttribute("ITEMGRADE", typeof(string), 40, false)]
        public string ItemGrade;

        [FieldMapAttribute("CLABSQty", typeof(decimal), 13, true)]
        public decimal CLABSQty;

        [FieldMapAttribute("CINSMQty", typeof(decimal), 13, true)]
        public decimal CINSMQty;

        [FieldMapAttribute("CSPEMQty", typeof(decimal), 13, true)]
        public decimal CSPEMQty;

        [FieldMapAttribute("CUMLQty", typeof(decimal), 13, true)]
        public decimal CUMLQty;

        [FieldMapAttribute("ITEMDESC", typeof(string), 100, true)]
        public string ItemDescription;

        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string ModelCode;

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
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;
    }
    #endregion

    #region SAPStorageQuery

    /// <summary>
    ///	SAPStorageQuery
    /// </summary>
    [Serializable, TableMap("TBLSAPSTORAGEQUERY", "SERIAL")]
    public class SAPStorageQuery : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public SAPStorageQuery()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>	
        [FieldMapAttribute("SERIAL", typeof(int), 8, false)]
        public int Serial;

        ///<summary>
        ///ItemCode
        ///</summary>	
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        ///<summary>
        ///OrganizationID
        ///</summary>	
        [FieldMapAttribute("ORGID", typeof(string), 500, false)]
        public string OrganizationID;

        ///<summary>
        ///StorageID
        ///</summary>	
        [FieldMapAttribute("STORAGEID", typeof(string), 500, false)]
        public string StorageID;

        ///<summary>
        ///Flag
        ///</summary>	
        [FieldMapAttribute("FLAG", typeof(string), 10, false)]
        public string Flag;

        ///<summary>
        ///TransactionCode
        ///</summary>	
        [FieldMapAttribute("TRANSACTIONCODE", typeof(string), 100, true)]
        public string TransactionCode;

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

    #region Storage-- �����Ϣ add by jinger 2016-01-18
    /// <summary>
    /// TBLSTORAGE-- �����Ϣ 
    /// </summary>
    [Serializable, TableMap("TBLSTORAGE", "STORAGECODE,ORGID")]
    public class Storage : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Storage()
        {
        }
        ///<summary>
        ///��֯ID
        ///</summary>
        [FieldMapAttribute("ORGID", typeof(int), 22, false)]
        public int OrgID;

        ///<summary>
        ///SAP��λ����
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        ///<summary>
        ///SAP��λ����
        ///</summary>
        [FieldMapAttribute("STORAGENAME", typeof(string), 100, true)]
        public string StorageName;

        ///<summary>
        ///Ԥ���ֶ�3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string Eattribute3;

        ///<summary>
        ///Ԥ���ֶ�2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string Eattribute2;

        ///<summary>
        ///��λ��Դ(MES, SAP)
        ///</summary>
        [FieldMapAttribute("SOURCEFLAG", typeof(string), 3, false)]
        public string SourceFlag;

        ///<summary>
        ///�����λ��ʶ(Y:�����λ; N:�������λ)
        ///</summary>
        [FieldMapAttribute("VIRTUALFLAG", typeof(string), 1, false)]
        public string VirtualFlag;

        ///<summary>
        ///��ϵ��4
        ///</summary>
        [FieldMapAttribute("CONTACTUSER4", typeof(string), 40, true)]
        public string ContactUser4;

        ///<summary>
        ///��ϵ��3
        ///</summary>
        [FieldMapAttribute("CONTACTUSER3", typeof(string), 40, true)]
        public string ContactUser3;

        ///<summary>
        ///��ϵ��2
        ///</summary>
        [FieldMapAttribute("CONTACTUSER2", typeof(string), 40, true)]
        public string ContactUser2;

        ///<summary>
        ///��ϵ��1
        ///</summary>
        [FieldMapAttribute("CONTACTUSER1", typeof(string), 40, true)]
        public string ContactUser1;

        ///<summary>
        ///��ַ4
        ///</summary>
        [FieldMapAttribute("ADDRESS4", typeof(string), 200, true)]
        public string Address4;

        ///<summary>
        ///��ַ3
        ///</summary>
        [FieldMapAttribute("ADDRESS3", typeof(string), 200, true)]
        public string Address3;

        ///<summary>
        ///��ַ2
        ///</summary>
        [FieldMapAttribute("ADDRESS2", typeof(string), 200, true)]
        public string Address2;

        ///<summary>
        ///��ַ1
        ///</summary>
        [FieldMapAttribute("ADDRESS1", typeof(string), 200, true)]
        public string Address1;

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("SPROPERTY", typeof(string), 40, true)]
        public string SProperty;

        ///<summary>
        ///Ԥ���ֶ�1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 100, true)]
        public string Eattribute1;

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, true)]
        public int MaintainTime;

        ///<summary>
        ///ά������ 
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, true)]
        public int MaintainDate;

        /// <summary>
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string MaintainUser;
    }
    #endregion

    #region Location--��λ��Ϣ add by jinger 2016-01-18
    /// <summary>
    /// TBLLOCATION--��λ��Ϣ
    /// </summary>
    [Serializable, TableMap("TBLLOCATION", "LOCATIONCODE,ORGID")]
    public class Location : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Location()
        {
        }

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά������ 
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///Ԥ���ֶ�3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string Eattribute3;

        ///<summary>
        ///Ԥ���ֶ�2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string Eattribute2;

        ///<summary>
        ///Ԥ���ֶ�1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;

        ///<summary>
        ///��֯ID
        ///</summary>
        [FieldMapAttribute("ORGID", typeof(int), 22, false)]
        public int OrgID;

        ///<summary>
        ///��λ����
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        ///<summary>
        ///��λ����
        ///</summary>
        [FieldMapAttribute("LOCATIONNAME", typeof(string), 100, true)]
        public string LocationName;

        ///<summary>
        ///��λ����
        ///</summary>
        [FieldMapAttribute("LOCATIONCODE", typeof(string), 40, false)]
        public string LocationCode;

    }

    [Serializable]
    public class LocationWithStorageName : Location
    {
        [FieldMapAttribute("STORAGENAME", typeof(string), 100, false)]
        public string StorageName;
    }
    #endregion

    #region SpecStorageInfo-- �������Ͽ����Ϣ  add by jinger 2016-01-29
    /// <summary>
    /// TBLSPECSTORAGEINFO-- �������Ͽ����Ϣ 
    /// </summary>
    [Serializable, TableMap("TBLSPECSTORAGEINFO", "STORAGECODE,MCODE,LOCATIONCODE")]
    public class SpecStorageInfo : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public SpecStorageInfo()
        {
        }

        #region add by sam
        ///<summary>
        ///Muom
        ///</summary>
        [FieldMapAttribute("MUOM", typeof(string), 40, true)]
        public string Muom;

        #endregion
        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά������
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///����ʱ��
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///������
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("STORAGEQTY", typeof(int), 22, false)]
        public int StorageQty;

        ///<summary>
        ///��λ����
        ///</summary>
        [FieldMapAttribute("LOCATIONCODE", typeof(string), 40, false)]
        public string LocationCode;

        ///<summary>
        ///��λ
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        ///<summary>
        ///�������ϱ���
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 22, false)]
        public string DQMCode;

        ///<summary>
        ///SAP���Ϻ�
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

    }
    #endregion

    #region Storagedetail-- �����ϸ��Ϣ add by jinger 2016-01-18
    /// <summary>
    /// TBLSTORAGEDETAIL-- �����ϸ��Ϣ 
    /// </summary>
    [Serializable, TableMap("TBLSTORAGEDETAIL", "CARTONNO")]
    public class StorageDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public StorageDetail()
        {
        }

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά������
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///����ʱ��
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///������
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///��ⷵ��������
        ///</summary>
        [FieldMapAttribute("REWORKAPPLYUSER", typeof(string), 40, true)]
        public string ReworkApplyUser;

        ///<summary>
        ///����
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FacCode;

        ///<summary>
        ///���һ�����ʱ��
        ///</summary>
        [FieldMapAttribute("LASTSTORAGEAGEDATE", typeof(int), 22, true)]
        public int LastStorageAgeDate;

        ///<summary>
        ///��һ�����ʱ��
        ///</summary>
        [FieldMapAttribute("STORAGEAGEDATE", typeof(int), 22, true)]
        public int StorageAgeDate;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("PRODUCTION_DATE", typeof(int), 22, true)]
        public int ProductionDate;

        ///<summary>
        ///�������κ�
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string Lotno;

        ///<summary>
        ///��Ӧ�����κ�
        ///</summary>
        [FieldMapAttribute("SUPPLIER_LOTNO", typeof(string), 40, true)]
        public string SupplierLotNo;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("FREEZEQTY", typeof(int), 22, false)]
        public int FreezeQty;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("AVAILABLEQTY", typeof(int), 22, false)]
        public int AvailableQty;

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("STORAGEQTY", typeof(int), 22, false)]
        public int StorageQty;

        ///<summary>
        ///��λ
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string MDesc;

        ///<summary>
        ///�������Ϻ�
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCode;

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string CartonNo;

        ///<summary>
        ///��λ����
        ///</summary>
        [FieldMapAttribute("LOCATIONCODE", typeof(string), 40, false)]
        public string LocationCode;

        ///<summary>
        ///��λ
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        ///<summary>
        ///SAP���Ϻ�
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///��Ч������ʱ��
        ///</summary>
        [FieldMapAttribute("ValidStartDate", typeof(int), 22, false)]
        public int ValidStartDate;


    }

    /// <summary>
    /// �����ϸ��չʵ��
    /// </summary>
    [Serializable]
    public class StorageDetailExt : StorageDetail
    {
        /// <summary>
        /// ��λ����
        /// </summary>
        [FieldMapAttribute("STORAGENAME", typeof(string), 100, false)]
        public string StorageName;

        ///<summary>
        ///��λ����
        ///</summary>
        [FieldMapAttribute("LOCATIONNAME", typeof(string), 100, true)]
        public string LocationName;

        ///<summary>
        ///SN����
        ///</summary>
        [FieldMapAttribute("SN", typeof(string), 40, false)]
        public string SN;

        ///<summary>
        ///Validity
        ///</summary>
        [FieldMapAttribute("VALIDITY", typeof(int), 22, true)]
        public int Validity;
    }
    #endregion

    #region Storagedetailsn-- �����ϸSN��Ϣ add by jinger 2016-01-18
    /// <summary>
    /// TBLSTORAGEDETAILSN-- �����ϸSN��Ϣ 
    /// </summary>
    [Serializable, TableMap("TBLSTORAGEDETAILSN", "SN")]
    public class StorageDetailSN : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public StorageDetailSN()
        {
        }

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά������ 
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///����ʱ��
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///������
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///�����
        ///</summary>
        [FieldMapAttribute("PICKBLOCK", typeof(string), 40, false)]
        public string PickBlock;

        ///<summary>
        ///SN����
        ///</summary>
        [FieldMapAttribute("SN", typeof(string), 40, false)]
        public string SN;

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string CartonNo;

    }
    #endregion

    #region Specinout-- �������ϳ���ⵥ add by jinger 2016-01-22
    /// <summary>
    /// TBLSPECINOUT-- �������ϳ���ⵥ 
    /// </summary>
    [Serializable, TableMap("TBLSPECINOUT", "SERIAL")]
    public class SpecInOut : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public SpecInOut()
        {
        }

        #region add by sam
        ///<summary>
        ///Muom
        ///</summary>
        [FieldMapAttribute("MUOM", typeof(string), 40, true)]
        public string Muom;

        #endregion

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά������ 
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("QTY", typeof(int), 22, true)]
        public int Qty;

        ///<summary>
        ///��λ����
        ///</summary>
        [FieldMapAttribute("LOCATIONCODE", typeof(string), 40, false)]
        public string LocationCode;

        ///<summary>
        ///��λ
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        ///<summary>
        ///���ϱ���
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///�������ϱ���
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCode;

        ///<summary>
        ///I=��⣬O=����
        ///</summary>
        [FieldMapAttribute("MOVETYPE", typeof(string), 1, false)]
        public string MoveType;

        [FieldMapAttribute("InOutDesc", typeof(string), 400, false)]
        public string InOutDesc;

        ///<summary>
        ///������
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

    }

    /// <summary>
    /// ���뵥��չʵ��
    /// </summary>
    [Serializable]
    public class SpecInOutWithMaterial : BenQGuru.eMES.Domain.MOModel.Material
    {
        /// <summary>
        /// ��λ����
        /// </summary>
        [FieldMapAttribute("STORAGENAME", typeof(string), 100, false)]
        public string StorageName;

        ///<summary>
        ///��λ����
        ///</summary>
        [FieldMapAttribute("LOCATIONNAME", typeof(string), 100, true)]
        public string LocationName;

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("QTY", typeof(int), 22, true)]
        public int Qty;
        [FieldMapAttribute("InOutDesc", typeof(string), 22, true)]
        public string InOutDesc;


    }
    #endregion

    //#region Invoices-- SAP���ݱ�  add by jinger 2016-01-25
    ///// <summary>
    ///// TBLINVOICES-- SAP���ݱ� 
    ///// </summary>
    //[Serializable, TableMap("TBLINVOICES", "INVNO")]
    //public class Invoices : BenQGuru.eMES.Common.Domain.DomainObject
    //{
    //    public Invoices()
    //    {
    //    }

    //    ///<summary>
    //    ///Ԥ���ֶ�3
    //    ///</summary>
    //    [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
    //    public string Eattribute3;

    //    ///<summary>
    //    ///Ԥ���ֶ�2
    //    ///</summary>
    //    [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
    //    public string Eattribute2;

    //    ///<summary>
    //    ///Ԥ���ֶ�1
    //    ///</summary>
    //    [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
    //    public string Eattribute1;

    //    ///<summary>
    //    ///ά��ʱ��
    //    ///</summary>
    //    [FieldMapAttribute("MTIME", typeof(int), 22, false)]
    //    public int MaintainTime;

    //    ///<summary>
    //    ///ά������
    //    ///</summary>
    //    [FieldMapAttribute("MDATE", typeof(int), 22, false)]
    //    public int MaintainDate;

    //    ///<summary>
    //    ///ά����
    //    ///</summary>
    //    [FieldMapAttribute("MUSER", typeof(string), 40, false)]
    //    public string MaintainUser;

    //    ///<summary>
    //    ///����ʱ��
    //    ///</summary>
    //    [FieldMapAttribute("CTIME", typeof(int), 22, false)]
    //    public int CTime;

    //    ///<summary>
    //    ///��������
    //    ///</summary>
    //    [FieldMapAttribute("CDATE", typeof(int), 22, false)]
    //    public int CDate;

    //    ///<summary>
    //    ///������
    //    ///</summary>
    //    [FieldMapAttribute("CUSER", typeof(string), 40, false)]
    //    public string CUser;

    //    ///<summary>
    //    ///�̵�ƾ֤��
    //    ///</summary>
    //    [FieldMapAttribute("INVENTORYNO", typeof(string), 16, true)]
    //    public string InventoryNo;

    //    ///<summary>
    //    ///ƾ֤����
    //    ///</summary>
    //    [FieldMapAttribute("VOUCHERDATE", typeof(int), 22, true)]
    //    public int VoucherDate;

    //    ///<summary>
    //    ///��������
    //    ///</summary>
    //    [FieldMapAttribute("APPLYDATE", typeof(int), 22, true)]
    //    public int ApplyDate;

    //    ///<summary>
    //    ///��/���ʶ
    //    ///</summary>
    //    [FieldMapAttribute("MOVETYPE", typeof(string), 3, true)]
    //    public string MoveType;

    //    ///<summary>
    //    ///�ɱ�����
    //    ///</summary>
    //    [FieldMapAttribute("CC", typeof(string), 10, true)]
    //    public string Cc;

    //    ///<summary>
    //    ///��ⷵ��������
    //    ///</summary>
    //    [FieldMapAttribute("REWORKAPPLYUSER", typeof(string), 12, true)]
    //    public string ReworkApplyUser;

    //    ///<summary>
    //    ///������Ϣ
    //    ///</summary>
    //    [FieldMapAttribute("LOGISTICS", typeof(string), 220, true)]
    //    public string Logistics;

    //    ///<summary>
    //    ///OA��ˮ��
    //    ///</summary>
    //    [FieldMapAttribute("OANO", typeof(string), 12, true)]
    //    public string OANO;

    //    ///<summary>
    //    ///�������ͣ�UB=��������ZC=ת����ZJCR=��ⷵ���룻ZJCC=��ⷵ������ZBLR=����Ʒ��⣻ZCAR=Claim��⣩(UB)
    //    ///</summary>
    //    [FieldMapAttribute("UBTYPE", typeof(string), 4, true)]
    //    public string UbType;

    //    ///<summary>
    //    ///�Ƿ�Ӳ������������ʶFrom SAP SO field��(Y��N)(DN)
    //    ///</summary>
    //    [FieldMapAttribute("GFFLAG", typeof(string), 40, true)]
    //    public string GfFlag;

    //    ///<summary>
    //    ///A=��pending��B=ȡ��cancel��C=����release(DN)
    //    ///</summary>
    //    [FieldMapAttribute("DNBATCHSTATUS", typeof(string), 1, true)]
    //    public string DNBatchStatus;

    //    ///<summary>
    //    ///�������� LF=������RE=�˻�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("DNTYPE", typeof(string), 4, true)]
    //    public string DNType;

    //    ///<summary>
    //    ///�˻���ʶ(Return PO��ʶ)(X=�˻�PO����=����PO)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("RETURNFLAG", typeof(string), 1, true)]
    //    public string ReturnFlag;

    //    ///<summary>
    //    ///PO��ע5--our reference(PO)
    //    ///</summary>
    //    [FieldMapAttribute("REMARK5", typeof(string), 12, true)]
    //    public string Remark5;

    //    ///<summary>
    //    ///PO��ע4--your reference(PO)
    //    ///</summary>
    //    [FieldMapAttribute("REMARK4", typeof(string), 12, true)]
    //    public string Remark4;

    //    ///<summary>
    //    ///PO��ע3--header remarks(PO)
    //    ///</summary>
    //    [FieldMapAttribute("REMARK3", typeof(string), 50, true)]
    //    public string Remark3;

    //    ///<summary>
    //    ///PO��ע2--header note(PO)
    //    ///</summary>
    //    [FieldMapAttribute("REMARK2", typeof(string), 50, true)]
    //    public string Remark2;

    //    ///<summary>
    //    ///PO��ע1--header text(PO)
    //    ///</summary>
    //    [FieldMapAttribute("REMARK1", typeof(string), 50, true)]
    //    public string Remark1;

    //    ///<summary>
    //    ///�ɹ���������purchase group��(PO)
    //    ///</summary>
    //    [FieldMapAttribute("PURCHASEGROUP", typeof(string), 18, true)]
    //    public string PurchaseGroup;

    //    ///<summary>
    //    ///�ɹ���(purchase group)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("PURCHUGCODE", typeof(string), 3, true)]
    //    public string PurchugCode;

    //    ///<summary>
    //    ///�ɹ���֯��purchase org.��(PO)
    //    ///</summary>
    //    [FieldMapAttribute("PURCHORGCODE", typeof(string), 4, true)]
    //    public string PurchorgCode;

    //    ///<summary>
    //    ///����Ա�绰(PO)
    //    ///</summary>
    //    [FieldMapAttribute("BUYERPHONE", typeof(string), 30, true)]
    //    public string BuyerPhone;

    //    ///<summary>
    //    ///��������ʱ�䣨ʱ���룩(PO)
    //    ///</summary>
    //    [FieldMapAttribute("POUPDATETIME", typeof(int), 22, true)]
    //    public int PoupDateTime;

    //    ///<summary>
    //    ///�����������ڣ������գ�(PO)
    //    ///</summary>
    //    [FieldMapAttribute("POUPDATEDATE", typeof(int), 22, true)]
    //    public int PoupDateDate;

    //    ///<summary>
    //    ///����������Ա(PO)
    //    ///</summary>
    //    [FieldMapAttribute("CREATEUSER", typeof(string), 12, true)]
    //    public string CreateUser;

    //    ///<summary>
    //    ///�����������ڣ������գ�(PO)
    //    ///</summary>
    //    [FieldMapAttribute("POCREATEDATE", typeof(int), 22, true)]
    //    public int PocreateDate;

    //    ///<summary>
    //    ///��������(NB)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("ORDERTYPE", typeof(string), 4, true)]
    //    public string OrderType;

    //    ///<summary>
    //    ///��Ӧ�̴���(PO)
    //    ///</summary>
    //    [FieldMapAttribute("VENDORCODE", typeof(string), 40, true)]
    //    public string VendorCode;

    //    ///<summary>
    //    ///��˾����(ȡTD28)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("COMPANYCODE", typeof(string), 4, true)]
    //    public string CompanyCode;

    //    ///<summary>
    //    ///����״̬(G=Release,��G=Pending)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("ORDERSTATUS", typeof(string), 1, true)]
    //    public string OrderStatus;

    //    ///<summary>
    //    ///�Ƿ�ɴ������ָ��(Y:�ɴ�����N�����ɴ���)
    //    ///</summary>
    //    [FieldMapAttribute("ASNAVAILABLE", typeof(string), 40, true)]
    //    public string ASNAvailable;

    //    ///<summary>
    //    ///SAP�������״̬(N:δ��ɣ�Y�����)
    //    ///</summary>
    //    [FieldMapAttribute("FINISHFLAG", typeof(string), 40, true)]
    //    public string FinishFlag;

    //    ///<summary>
    //    ///SAP��������
    //    ///</summary>
    //    [FieldMapAttribute("INVTYPE", typeof(string), 40, false)]
    //    public string InvType;

    //    ///<summary>
    //    ///SAP����״̬
    //    ///</summary>
    //    [FieldMapAttribute("INVSTATUS", typeof(string), 1, true)]
    //    public string InvStatus;

    //    ///<summary>
    //    ///SAP���ݺ�(PO)
    //    ///</summary>
    //    [FieldMapAttribute("INVNO", typeof(string), 40, false)]
    //    public string InvNo;

    //}
    //#endregion
    /// <summary>
    /// SAP������չʵ��
    /// </summary>
    [Serializable]
    public class InvoicesExt : Invoices
    {
        /// <summary>
        /// ��Ӧ������
        /// </summary>
        [FieldMapAttribute("VENDORNAME", typeof(string), 100, false)]
        public string VendorName;

    }

    #region Invoices   //Amy �޸�  ��ṹ�仯  @20160317
    /// <summary>
    /// TBLINVOICES
    /// </summary>
    [Serializable, TableMap("TBLINVOICES", "INVNO")]
    public class Invoices : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Invoices()
        {
        }

        [FieldMapAttribute("NOTOUTCHECKFLAG", typeof(string), 1, true)]
        public string NotOutCheckFlag;

        #region add by sam
        [FieldMapAttribute("SAPCUSER", typeof(string), 40, false)]
        public string SAPCuser;

        ///<summary>
        ///ͬ��ʱ��
        ///</summary>
        [FieldMapAttribute("SAPCTIME", typeof(int), 22, false)]
        public int SAPCTime;

        ///<summary>
        ///ͬ������
        ///</summary>
        [FieldMapAttribute("SAPCDATE", typeof(int), 22, false)]
        public int SAPCDate;

        #endregion
        ///<summary>
        ///DnmTime
        ///</summary>
        [FieldMapAttribute("DNMTIME", typeof(int), 22, true)]
        public int DnMTime;

        ///<summary>
        ///Cusbatchno
        ///</summary>
        [FieldMapAttribute("CUSBATCHNO", typeof(string), 240, true)]
        public string CusBatchNo;
        ///<summary>
        ///fromfaccode
        ///</summary>
        [FieldMapAttribute("fromfaccode", typeof(string), 4, true)]
        public string fromfaccode;
        ///<summary>
        ///Shippinglocation
        ///</summary>
        [FieldMapAttribute("SHIPPINGLOCATION", typeof(string), 528, true)]
        public string ShippingLocation;

        ///<summary>
        ///PlangiDate
        ///</summary>
        [FieldMapAttribute("PLANGIDATE", typeof(int), 22, true)]
        public int PlangiDate;

        ///<summary>
        ///Gfcontractno
        ///</summary>
        [FieldMapAttribute("GFCONTRACTNO", typeof(string), 48, true)]
        public string GFContractNo;

        ///<summary>
        ///Invno
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, false)]
        public string InvNo;

        ///<summary>
        ///InvStatus
        ///</summary>
        [FieldMapAttribute("INVSTATUS", typeof(string), 40, true)]
        public string InvStatus;

        ///<summary>
        ///InvType
        ///</summary>
        [FieldMapAttribute("INVTYPE", typeof(string), 40, false)]
        public string InvType;

        ///<summary>
        ///Finishflag
        ///</summary>
        [FieldMapAttribute("FINISHFLAG", typeof(string), 40, false)]
        public string FinishFlag;

        ///<summary>
        ///Asnavailable
        ///</summary>
        [FieldMapAttribute("ASNAVAILABLE", typeof(string), 40, true)]
        public string AsnAvailable;

        ///<summary>
        ///OrderStatus
        ///</summary>
        [FieldMapAttribute("ORDERSTATUS", typeof(string), 1, true)]
        public string OrderStatus;

        ///<summary>
        ///CompanyCode
        ///</summary>
        [FieldMapAttribute("COMPANYCODE", typeof(string), 4, true)]
        public string CompanyCode;

        ///<summary>
        ///VEndorCode
        ///</summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, true)]
        public string VendorCode;

        ///<summary>
        ///OrderType
        ///</summary>
        [FieldMapAttribute("ORDERTYPE", typeof(string), 4, true)]
        public string OrderType;

        ///<summary>
        ///PocreateDate
        ///</summary>
        [FieldMapAttribute("POCREATEDATE", typeof(int), 22, true)]
        public int PoCreateDate;

        ///<summary>
        ///CreateUser
        ///</summary>
        [FieldMapAttribute("CREATEUSER", typeof(string), 12, true)]
        public string CreateUser;

        ///<summary>
        ///PoupDatedate
        ///</summary>
        [FieldMapAttribute("POUPDATEDATE", typeof(int), 22, true)]
        public int PoupDateDate;

        ///<summary>
        ///PoupDateTime
        ///</summary>
        [FieldMapAttribute("POUPDATETIME", typeof(int), 22, true)]
        public int PoupDateTime;

        ///<summary>
        ///Buyerphone
        ///</summary>
        [FieldMapAttribute("BUYERPHONE", typeof(string), 30, true)]
        public string BuyerPhone;

        ///<summary>
        ///PurchorgCode
        ///</summary>
        [FieldMapAttribute("PURCHORGCODE", typeof(string), 4, true)]
        public string PurChorgCode;

        ///<summary>
        ///PurchugCode
        ///</summary>
        [FieldMapAttribute("PURCHUGCODE", typeof(string), 3, true)]
        public string PurChugCode;

        ///<summary>
        ///Purchasegroup
        ///</summary>
        [FieldMapAttribute("PURCHASEGROUP", typeof(string), 72, true)]
        public string PurchaseGroup;

        ///<summary>
        ///Remark1
        ///</summary>
        [FieldMapAttribute("REMARK1", typeof(string), 200, true)]
        public string Remark1;

        ///<summary>
        ///Remark2
        ///</summary>
        [FieldMapAttribute("REMARK2", typeof(string), 200, true)]
        public string Remark2;

        ///<summary>
        ///Remark3
        ///</summary>
        [FieldMapAttribute("REMARK3", typeof(string), 200, true)]
        public string Remark3;

        ///<summary>
        ///Remark4
        ///</summary>
        [FieldMapAttribute("REMARK4", typeof(string), 48, true)]
        public string Remark4;

        ///<summary>
        ///Remark5
        ///</summary>
        [FieldMapAttribute("REMARK5", typeof(string), 48, true)]
        public string Remark5;

        ///<summary>
        ///Returnflag
        ///</summary>
        [FieldMapAttribute("RETURNFLAG", typeof(string), 1, true)]
        public string ReturnFlag;

        /////<summary>
        /////DnType
        /////</summary>
        //[FieldMapAttribute("DNTYPE", typeof(string), 4, true)]
        //public string DnType;

        ///<summary>
        ///DnbatchStatus
        ///</summary>
        [FieldMapAttribute("DNBATCHSTATUS", typeof(string), 1, true)]
        public string DnBatchStatus;

        ///<summary>
        ///Gfflag
        ///</summary>
        [FieldMapAttribute("GFFLAG", typeof(string), 40, true)]
        public string GfFlag;

        ///<summary>
        ///UbType
        ///</summary>
        [FieldMapAttribute("UBTYPE", typeof(string), 4, true)]
        public string UbType;

        ///<summary>
        ///Oano
        ///</summary>
        [FieldMapAttribute("OANO", typeof(string), 12, true)]
        public string OaMo;

        ///<summary>
        ///Logistics
        ///</summary>
        [FieldMapAttribute("LOGISTICS", typeof(string), 220, true)]
        public string Logistics;

        ///<summary>
        ///ReworkapplyUser
        ///</summary>
        [FieldMapAttribute("REWORKAPPLYUSER", typeof(string), 12, true)]
        public string ReworkApplyUser;

        ///<summary>
        ///Cc
        ///</summary>
        [FieldMapAttribute("CC", typeof(string), 10, true)]
        public string Cc;

        ///<summary>
        ///MoveType
        ///</summary>
        [FieldMapAttribute("MOVETYPE", typeof(string), 3, true)]
        public string MoveType;

        ///<summary>
        ///ApplyDate
        ///</summary>
        [FieldMapAttribute("APPLYDATE", typeof(int), 22, true)]
        public int ApplyDate;

        ///<summary>
        ///VoucherDate
        ///</summary>
        [FieldMapAttribute("VOUCHERDATE", typeof(int), 22, true)]
        public int VoucherDate;

        ///<summary>
        ///Inventoryno
        ///</summary>
        [FieldMapAttribute("INVENTORYNO", typeof(string), 16, true)]
        public string InventoryNo;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;



        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;

        ///<summary>
        ///Eattribute2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string Eattribute2;

        ///<summary>
        ///Eattribute3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string Eattribute3;

        ///<summary>
        ///Shiptoparty
        ///</summary>
        [FieldMapAttribute("SHIPTOPARTY", typeof(string), 40, true)]
        public string ShipToParty;

        ///<summary>
        ///Orderreason
        ///</summary>
        [FieldMapAttribute("ORDERREASON", typeof(string), 12, true)]
        public string OrderReason;

        ///<summary>
        ///Postway
        ///</summary>
        [FieldMapAttribute("POSTWAY", typeof(string), 40, true)]
        public string PostWay;

        ///<summary>
        ///Pickcondition
        ///</summary>
        [FieldMapAttribute("PICKCONDITION", typeof(string), 32, true)]
        public string PickCondition;

        ///<summary>
        ///Orderno
        ///</summary>
        [FieldMapAttribute("ORDERNO", typeof(string), 140, true)]
        public string OrderNo;

        ///<summary>
        ///Cusorderno
        ///</summary>
        [FieldMapAttribute("CUSORDERNO", typeof(string), 40, true)]
        public string CusOrderNo;

        ///<summary>
        ///CusordernoType
        ///</summary>
        [FieldMapAttribute("CUSORDERNOTYPE", typeof(string), 16, true)]
        public string CusOrderNoType;

        ///<summary>
        ///DnmUser
        ///</summary>
        [FieldMapAttribute("DNMUSER", typeof(string), 48, true)]
        public string DnMUser;

        ///<summary>
        ///DnmDate
        ///</summary>
        [FieldMapAttribute("DNMDATE", typeof(int), 22, true)]
        public int DnMDate;

        [FieldMapAttribute("DNBATCHNO", typeof(string), 20, true)]
        public string DNBATCHNO;

    }
    #endregion

    #region SAPInvoices add by sam

    /// <summary>
    /// SAPInvoicesQuery 
    /// </summary>
    public class SAPInvoicesQuery : SAPInvoices
    {
        public SAPInvoicesQuery()
        {
        }

        [FieldMapAttribute("DNNO", typeof(string), 40, true)]
        public string DNNO;

        ///<summary>
        ///INVLINE
        ///</summary>
        [FieldMapAttribute("INVLINE", typeof(int), 22, false)]
        public int INVLINE;

        ///<summary>
        ///INVLINESTATUS
        ///</summary>
        [FieldMapAttribute("INVLINESTATUS", typeof(string), 40, true)]
        public string INVLINESTATUS;

        /////<summary>
        /////FROMFACCODE
        /////</summary>
        //[FieldMapAttribute("FROMFACCODE", typeof(string), 10, true)]
        //public string FROMFACCODE;

        ///<summary>
        ///FROMSTORAGECODE
        ///</summary>
        [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 12, true)]
        public string FROMSTORAGECODE;

        ///<summary>
        ///FACCODE
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 4, true)]
        public string FACCODE;

        ///<summary>
        ///STORAGECODE
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string STORAGECODE;

        ///<summary>
        ///MCODE
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 18, false)]
        public string MCODE;

        ///<summary>
        ///DQMCODE
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCODE;

        ///<summary>
        ///MENSHORTDESC
        ///</summary>
        [FieldMapAttribute("MENSHORTDESC", typeof(string), 160, true)]
        public string MENSHORTDESC;

        ///<summary>
        ///MLONGDESC
        ///</summary>
        [FieldMapAttribute("MLONGDESC", typeof(string), 880, true)]
        public string MLONGDESC;

        ///<summary>
        ///PLANQTY
        ///</summary>
        [FieldMapAttribute("PLANQTY", typeof(decimal), 22, false)]
        public decimal PLANQTY;

        ///<summary>
        ///ACTQTY
        ///</summary>
        [FieldMapAttribute("ACTQTY", typeof(decimal), 22, true)]
        public decimal ACTQTY;

        ///<summary>
        ///OUTQTY
        ///</summary>
        [FieldMapAttribute("OUTQTY", typeof(decimal), 22, true)]
        public decimal OUTQTY;

        ///<summary>
        ///PLANDATE
        ///</summary>
        [FieldMapAttribute("PLANDATE", typeof(int), 22, true)]
        public int PLANDATE;

        ///<summary>
        ///UNIT
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 3, true)]
        public string UNIT;

        ///<summary>
        ///SHIPADDR
        ///</summary>
        [FieldMapAttribute("SHIPADDR", typeof(string), 240, true)]
        public string SHIPADDR;

        ///<summary>
        ///STATUS
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string STATUS;

        ///<summary>
        ///DETAILREMARK
        ///</summary>
        [FieldMapAttribute("DETAILREMARK", typeof(string), 220, true)]
        public string DETAILREMARK;

        ///<summary>
        ///VENDORMCODE
        ///</summary>
        [FieldMapAttribute("VENDORMCODE", typeof(string), 140, true)]
        public string VENDORMCODE;

        ///<summary>
        ///PRNO
        ///</summary>
        [FieldMapAttribute("PRNO", typeof(string), 10, true)]
        public string PRNO;



        ///<summary>
        ///ACCOUNTASSIGNMENT
        ///</summary>
        [FieldMapAttribute("ACCOUNTASSIGNMENT", typeof(string), 1, true)]
        public string ACCOUNTASSIGNMENT;

        ///<summary>
        ///ITEMCATEGORY
        ///</summary>
        [FieldMapAttribute("ITEMCATEGORY", typeof(string), 1, true)]
        public string ITEMCATEGORY;

        ///<summary>
        ///INCOTERMS1
        ///</summary>
        [FieldMapAttribute("INCOTERMS1", typeof(string), 3, true)]
        public string INCOTERMS1;

        ///<summary>
        ///INCOTERMS2
        ///</summary>
        [FieldMapAttribute("INCOTERMS2", typeof(string), 28, true)]
        public string INCOTERMS2;

        ///<summary>
        ///SO
        ///</summary>
        [FieldMapAttribute("SO", typeof(string), 10, true)]
        public string SO;

        ///<summary>
        ///SOITEMNO
        ///</summary>
        [FieldMapAttribute("SOITEMNO", typeof(string), 6, true)]
        public string SOITEMNO;

        ///<summary>
        ///SOWBSNO
        ///</summary>
        [FieldMapAttribute("SOWBSNO", typeof(string), 8, true)]
        public string SOWBSNO;

        ///<summary>
        ///CCNO
        ///</summary>
        [FieldMapAttribute("CCNO", typeof(string), 10, true)]
        public string CCNO;

        /////<summary>
        /////CUSBATCHNO
        /////</summary>
        //[FieldMapAttribute("CUSBATCHNO", typeof(string), 60, true)]
        //public string CUSBATCHNO;

        ///<summary>
        ///HIGNLEVELITEM
        ///</summary>
        [FieldMapAttribute("HIGNLEVELITEM", typeof(int), 22, true)]
        public int HIGNLEVELITEM;

        ///<summary>
        ///MOVEMENTTYPE
        ///</summary>
        [FieldMapAttribute("MOVEMENTTYPE", typeof(string), 4, true)]
        public string MOVEMENTTYPE;

        ///<summary>
        ///CUSMCODE
        ///</summary>
        [FieldMapAttribute("CUSMCODE", typeof(string), 40, true)]
        public string CUSMCODE;

        ///<summary>
        ///CUSITEMSPEC
        ///</summary>
        [FieldMapAttribute("CUSITEMSPEC", typeof(string), 40, true)]
        public string CUSITEMSPEC;

        ///<summary>
        ///CUSITEMDESC
        ///</summary>
        [FieldMapAttribute("CUSITEMDESC", typeof(string), 40, true)]
        public string CUSITEMDESC;

        ///<summary>
        ///VENDERMCODE
        ///</summary>
        [FieldMapAttribute("VENDERMCODE", typeof(string), 40, true)]
        public string VENDERMCODE;

        ///<summary>
        ///GFHWMCODE
        ///</summary>
        [FieldMapAttribute("GFHWMCODE", typeof(string), 12, true)]
        public string GFHWMCODE;

        ///<summary>
        ///GFPACKINGSEQ
        ///</summary>
        [FieldMapAttribute("GFPACKINGSEQ", typeof(string), 6, true)]
        public string GFPACKINGSEQ;

        ///<summary>
        ///GFHWDESC
        ///</summary>
        [FieldMapAttribute("GFHWDESC", typeof(string), 40, true)]
        public string GFHWDESC;

        ///<summary>
        ///HWCODEQTY
        ///</summary>
        [FieldMapAttribute("HWCODEQTY", typeof(string), 40, true)]
        public string HWCODEQTY;

        ///<summary>
        ///HWCODEUNIT
        ///</summary>
        [FieldMapAttribute("HWCODEUNIT", typeof(string), 40, true)]
        public string HWCODEUNIT;

        ///<summary>
        ///HWTYPEINFO
        ///</summary>
        [FieldMapAttribute("HWTYPEINFO", typeof(string), 40, true)]
        public string HWTYPEINFO;

        ///<summary>
        ///PACKINGWAY
        ///</summary>
        [FieldMapAttribute("PACKINGWAY", typeof(string), 40, true)]
        public string PACKINGWAY;

        ///<summary>
        ///PACKINGNO
        ///</summary>
        [FieldMapAttribute("PACKINGNO", typeof(string), 40, true)]
        public string PACKINGNO;

        ///<summary>
        ///PACKINGSPEC
        ///</summary>
        [FieldMapAttribute("PACKINGSPEC", typeof(string), 40, true)]
        public string PACKINGSPEC;

        ///<summary>
        ///PACKINGWAYNO
        ///</summary>
        [FieldMapAttribute("PACKINGWAYNO", typeof(string), 40, true)]
        public string PACKINGWAYNO;

        ///<summary>
        ///DQSMCODE
        ///</summary>
        [FieldMapAttribute("DQSMCODE", typeof(string), 40, true)]
        public string DQSMCODE;

        ///<summary>
        ///RECEIVERADDR
        ///</summary>
        [FieldMapAttribute("RECEIVERADDR", typeof(string), 240, true)]
        public string RECEIVERADDR;

        ///<summary>
        ///RECEIVERUSER
        ///</summary>
        [FieldMapAttribute("RECEIVERUSER", typeof(string), 50, true)]
        public string RECEIVERUSER;

        ///<summary>
        ///CUSTMCODE
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 35, true)]
        public string CUSTMCODE;

        ///<summary>
        ///RECEIVEMCODE
        ///</summary>
        [FieldMapAttribute("RECEIVEMCODE", typeof(string), 35, true)]
        public string RECEIVEMCODE;

        ///<summary>
        ///DEMANDARRIVALDATE
        ///</summary>
        [FieldMapAttribute("DEMANDARRIVALDATE", typeof(int), 22, true)]
        public int DEMANDARRIVALDATE;

        ///<summary>
        ///NEEDDATE
        ///</summary>
        [FieldMapAttribute("NEEDDATE", typeof(int), 22, true)]
        public int NEEDDATE;

        ///<summary>
        ///TYPE
        ///</summary>
        [FieldMapAttribute("TYPE", typeof(string), 3, true)]
        public string TYPE;


    }
    #endregion

    #region SAPInvoices add by sam

    /// <summary>
    /// TBLINVOICES 
    /// </summary>
    [Serializable, TableMap("TBLINVOICES", "INVNO")]
    public class SAPInvoices : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public SAPInvoices()
        {
        }


        [FieldMapAttribute("NOTOUTCHECKFLAG", typeof(string), 1, true)]
        public string NOTOUTCHECKFLAG;

        [FieldMapAttribute("VENDORNAME", typeof(string), 100, false)]
        public string VENDORNAME;

        ///<summary>
        ///DNMTIME
        ///</summary>
        [FieldMapAttribute("DNMTIME", typeof(int), 22, true)]
        public int DNMTIME;

        ///<summary>
        ///CUSBATCHNO
        ///</summary>
        [FieldMapAttribute("CUSBATCHNO", typeof(string), 240, true)]
        public string CUSBATCHNO;

        ///<summary>
        ///FROMFACCODE
        ///</summary>
        [FieldMapAttribute("FROMFACCODE", typeof(string), 4, true)]
        public string FROMFACCODE;

        ///<summary>
        ///SHIPPINGLOCATION
        ///</summary>
        [FieldMapAttribute("SHIPPINGLOCATION", typeof(string), 528, true)]
        public string SHIPPINGLOCATION;

        ///<summary>
        ///PLANGIDATE
        ///</summary>
        [FieldMapAttribute("PLANGIDATE", typeof(int), 22, true)]
        public int PLANGIDATE;

        ///<summary>
        ///GFCONTRACTNO
        ///</summary>
        [FieldMapAttribute("GFCONTRACTNO", typeof(string), 48, true)]
        public string GFCONTRACTNO;

        ///<summary>
        ///INVNO
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, false)]
        public string INVNO;

        ///<summary>
        ///INVSTATUS
        ///</summary>
        [FieldMapAttribute("INVSTATUS", typeof(string), 40, true)]
        public string INVSTATUS;

        ///<summary>
        ///INVTYPE
        ///</summary>
        [FieldMapAttribute("INVTYPE", typeof(string), 40, false)]
        public string INVTYPE;

        ///<summary>
        ///FINISHFLAG
        ///</summary>
        [FieldMapAttribute("FINISHFLAG", typeof(string), 40, false)]
        public string FINISHFLAG;

        ///<summary>
        ///ASNAVAILABLE
        ///</summary>
        [FieldMapAttribute("ASNAVAILABLE", typeof(string), 40, true)]
        public string ASNAVAILABLE;

        ///<summary>
        ///ORDERSTATUS
        ///</summary>
        [FieldMapAttribute("ORDERSTATUS", typeof(string), 1, true)]
        public string ORDERSTATUS;

        ///<summary>
        ///COMPANYCODE
        ///</summary>
        [FieldMapAttribute("COMPANYCODE", typeof(string), 4, true)]
        public string COMPANYCODE;

        ///<summary>
        ///VENDORCODE
        ///</summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, true)]
        public string VENDORCODE;

        ///<summary>
        ///ORDERTYPE
        ///</summary>
        [FieldMapAttribute("ORDERTYPE", typeof(string), 4, true)]
        public string ORDERTYPE;

        ///<summary>
        ///POCREATEDATE
        ///</summary>
        [FieldMapAttribute("POCREATEDATE", typeof(int), 22, true)]
        public int POCREATEDATE;

        ///<summary>
        ///CREATEUSER
        ///</summary>
        [FieldMapAttribute("CREATEUSER", typeof(string), 12, true)]
        public string CREATEUSER;

        ///<summary>
        ///POUPDATEDATE
        ///</summary>
        [FieldMapAttribute("POUPDATEDATE", typeof(int), 22, true)]
        public int POUPDATEDATE;

        ///<summary>
        ///POUPDATETIME
        ///</summary>
        [FieldMapAttribute("POUPDATETIME", typeof(int), 22, true)]
        public int POUPDATETIME;

        ///<summary>
        ///BUYERPHONE
        ///</summary>
        [FieldMapAttribute("BUYERPHONE", typeof(string), 30, true)]
        public string BUYERPHONE;

        ///<summary>
        ///PURCHORGCODE
        ///</summary>
        [FieldMapAttribute("PURCHORGCODE", typeof(string), 4, true)]
        public string PURCHORGCODE;

        ///<summary>
        ///PURCHUGCODE
        ///</summary>
        [FieldMapAttribute("PURCHUGCODE", typeof(string), 3, true)]
        public string PURCHUGCODE;

        ///<summary>
        ///PURCHASEGROUP
        ///</summary>
        [FieldMapAttribute("PURCHASEGROUP", typeof(string), 72, true)]
        public string PURCHASEGROUP;

        ///<summary>
        ///REMARK1
        ///</summary>
        [FieldMapAttribute("REMARK1", typeof(string), 200, true)]
        public string REMARK1;

        ///<summary>
        ///REMARK2
        ///</summary>
        [FieldMapAttribute("REMARK2", typeof(string), 200, true)]
        public string REMARK2;

        ///<summary>
        ///REMARK3
        ///</summary>
        [FieldMapAttribute("REMARK3", typeof(string), 200, true)]
        public string REMARK3;

        ///<summary>
        ///REMARK4
        ///</summary>
        [FieldMapAttribute("REMARK4", typeof(string), 48, true)]
        public string REMARK4;

        ///<summary>
        ///REMARK5
        ///</summary>
        [FieldMapAttribute("REMARK5", typeof(string), 48, true)]
        public string REMARK5;

        ///<summary>
        ///RETURNFLAG
        ///</summary>
        [FieldMapAttribute("RETURNFLAG", typeof(string), 1, true)]
        public string RETURNFLAG;

        /////<summary>
        /////DNTYPE
        /////</summary>
        //[FieldMapAttribute("DNTYPE", typeof(string), 4, true)]
        //public string DNTYPE;

        ///<summary>
        ///DNBATCHSTATUS
        ///</summary>
        [FieldMapAttribute("DNBATCHSTATUS", typeof(string), 1, true)]
        public string DNBATCHSTATUS;

        ///<summary>
        ///GFFLAG
        ///</summary>
        [FieldMapAttribute("GFFLAG", typeof(string), 40, true)]
        public string GFFLAG;

        ///<summary>
        ///UBTYPE
        ///</summary>
        [FieldMapAttribute("UBTYPE", typeof(string), 4, true)]
        public string UBTYPE;

        ///<summary>
        ///OANO
        ///</summary>
        [FieldMapAttribute("OANO", typeof(string), 12, true)]
        public string OANO;

        ///<summary>
        ///LOGISTICS
        ///</summary>
        [FieldMapAttribute("LOGISTICS", typeof(string), 220, true)]
        public string LOGISTICS;

        ///<summary>
        ///REWORKAPPLYUSER
        ///</summary>
        [FieldMapAttribute("REWORKAPPLYUSER", typeof(string), 12, true)]
        public string REWORKAPPLYUSER;

        ///<summary>
        ///CC
        ///</summary>
        [FieldMapAttribute("CC", typeof(string), 10, true)]
        public string CC;

        ///<summary>
        ///MOVETYPE
        ///</summary>
        [FieldMapAttribute("MOVETYPE", typeof(string), 3, true)]
        public string MOVETYPE;

        ///<summary>
        ///APPLYDATE
        ///</summary>
        [FieldMapAttribute("APPLYDATE", typeof(int), 22, true)]
        public int APPLYDATE;

        ///<summary>
        ///VOUCHERDATE
        ///</summary>
        [FieldMapAttribute("VOUCHERDATE", typeof(int), 22, true)]
        public int VOUCHERDATE;

        ///<summary>
        ///INVENTORYNO
        ///</summary>
        [FieldMapAttribute("INVENTORYNO", typeof(string), 16, true)]
        public string INVENTORYNO;

        ///<summary>
        ///CUSER
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUSER;

        ///<summary>
        ///CDATE
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDATE;

        ///<summary>
        ///CTIME
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTIME;

        ///<summary>
        ///MAINTAINUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;

        ///<summary>
        ///MAINTAINDATE
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MDATE;

        ///<summary>
        ///MAINTAINTIME
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MTIME;

        ///<summary>
        ///EATTRIBUTE1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string EATTRIBUTE1;

        ///<summary>
        ///EATTRIBUTE2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string EATTRIBUTE2;

        ///<summary>
        ///EATTRIBUTE3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string EATTRIBUTE3;

        ///<summary>
        ///SHIPTOPARTY
        ///</summary>
        [FieldMapAttribute("SHIPTOPARTY", typeof(string), 40, true)]
        public string SHIPTOPARTY;

        ///<summary>
        ///ORDERREASON
        ///</summary>
        [FieldMapAttribute("ORDERREASON", typeof(string), 12, true)]
        public string ORDERREASON;

        ///<summary>
        ///POSTWAY
        ///</summary>
        [FieldMapAttribute("POSTWAY", typeof(string), 40, true)]
        public string POSTWAY;

        ///<summary>
        ///PICKCONDITION
        ///</summary>
        [FieldMapAttribute("PICKCONDITION", typeof(string), 32, true)]
        public string PICKCONDITION;

        ///<summary>
        ///ORDERNO
        ///</summary>
        [FieldMapAttribute("ORDERNO", typeof(string), 140, true)]
        public string ORDERNO;

        ///<summary>
        ///CUSORDERNO
        ///</summary>
        [FieldMapAttribute("CUSORDERNO", typeof(string), 40, true)]
        public string CUSORDERNO;

        ///<summary>
        ///CUSORDERNOTYPE
        ///</summary>
        [FieldMapAttribute("CUSORDERNOTYPE", typeof(string), 16, true)]
        public string CUSORDERNOTYPE;

        ///<summary>
        ///DNMUSER
        ///</summary>
        [FieldMapAttribute("DNMUSER", typeof(string), 48, true)]
        public string DNMUSER;

        ///<summary>
        ///DNMDATE
        ///</summary>
        [FieldMapAttribute("DNMDATE", typeof(int), 22, true)]
        public int DNMDATE;

        [FieldMapAttribute("DNBATCHNO", typeof(string), 20, true)]
        public string DNBATCHNO;
    }
    #endregion

    #region SAPInvoicesDetail

    /// <summary>
    /// TBLINVOICESDETAIL-- SAP������ϸ�� add by jinger 2016-01-25
    /// </summary>
    [Serializable, TableMap("TBLINVOICESDETAIL", "INVNO,INVLINE")]
    public class SAPInvoicesDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public SAPInvoicesDetail()
        {
        }

        //add by sam
        //[FieldMapAttribute("LINEREMARK", typeof(string), 160, true)]
        //public string LINEREMARK;

        ///<summary>
        ///SAP��������
        ///</summary>
        [FieldMapAttribute("INVTYPE", typeof(string), 40, false)]
        public string INVTYPE;

        ///<summary>
        ///MAINTAINDATE
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MDATE;

        ///<summary>
        ///MAINTAINTIME
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MTIME;

        ///<summary>
        ///EATTRIBUTE1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string EATTRIBUTE1;

        ///<summary>
        ///EATTRIBUTE2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string EATTRIBUTE2;

        ///<summary>
        ///EATTRIBUTE3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string EATTRIBUTE3;

        ///<summary>
        ///INVNO
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, false)]
        public string INVNO;

        ///<summary>
        ///INVLINE
        ///</summary>
        [FieldMapAttribute("INVLINE", typeof(int), 22, false)]
        public int INVLINE;

        ///<summary>
        ///INVLINESTATUS
        ///</summary>
        [FieldMapAttribute("INVLINESTATUS", typeof(string), 40, true)]
        public string INVLINESTATUS;

        /////<summary>
        /////FROMFACCODE
        /////</summary>
        //[FieldMapAttribute("FROMFACCODE", typeof(string), 10, true)]
        //public string FROMFACCODE;

        ///<summary>
        ///FROMSTORAGECODE
        ///</summary>
        [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 12, true)]
        public string FROMSTORAGECODE;

        ///<summary>
        ///FACCODE
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 4, true)]
        public string FACCODE;

        ///<summary>
        ///STORAGECODE
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string STORAGECODE;

        ///<summary>
        ///MCODE
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 18, false)]
        public string MCODE;

        ///<summary>
        ///DQMCODE
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCODE;

        ///<summary>
        ///MENSHORTDESC
        ///</summary>
        [FieldMapAttribute("MENSHORTDESC", typeof(string), 160, true)]
        public string MENSHORTDESC;

        ///<summary>
        ///MLONGDESC
        ///</summary>
        [FieldMapAttribute("MLONGDESC", typeof(string), 880, true)]
        public string MLONGDESC;

        ///<summary>
        ///PLANQTY
        ///</summary>
        [FieldMapAttribute("PLANQTY", typeof(decimal), 22, false)]
        public decimal PLANQTY;

        ///<summary>
        ///ACTQTY
        ///</summary>
        [FieldMapAttribute("ACTQTY", typeof(decimal), 22, true)]
        public decimal ACTQTY;

        ///<summary>
        ///OUTQTY
        ///</summary>
        [FieldMapAttribute("OUTQTY", typeof(decimal), 22, true)]
        public decimal OUTQTY;

        ///<summary>
        ///PLANDATE
        ///</summary>
        [FieldMapAttribute("PLANDATE", typeof(int), 22, true)]
        public int PLANDATE;

        ///<summary>
        ///UNIT
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 3, true)]
        public string UNIT;

        ///<summary>
        ///SHIPADDR
        ///</summary>
        [FieldMapAttribute("SHIPADDR", typeof(string), 240, true)]
        public string SHIPADDR;

        ///<summary>
        ///STATUS
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string STATUS;

        ///<summary>
        ///DETAILREMARK
        ///</summary>
        [FieldMapAttribute("DETAILREMARK", typeof(string), 220, true)]
        public string DETAILREMARK;

        ///<summary>
        ///VENDORMCODE
        ///</summary>
        [FieldMapAttribute("VENDORMCODE", typeof(string), 140, true)]
        public string VENDORMCODE;

        ///<summary>
        ///PRNO
        ///</summary>
        [FieldMapAttribute("PRNO", typeof(string), 10, true)]
        public string PRNO;

        ///<summary>
        ///RETURNFLAG
        ///</summary>
        [FieldMapAttribute("RETURNFLAG", typeof(string), 1, true)]
        public string RETURNFLAG;

        ///<summary>
        ///ACCOUNTASSIGNMENT
        ///</summary>
        [FieldMapAttribute("ACCOUNTASSIGNMENT", typeof(string), 1, true)]
        public string ACCOUNTASSIGNMENT;

        ///<summary>
        ///ITEMCATEGORY
        ///</summary>
        [FieldMapAttribute("ITEMCATEGORY", typeof(string), 1, true)]
        public string ITEMCATEGORY;

        ///<summary>
        ///INCOTERMS1
        ///</summary>
        [FieldMapAttribute("INCOTERMS1", typeof(string), 3, true)]
        public string INCOTERMS1;

        ///<summary>
        ///INCOTERMS2
        ///</summary>
        [FieldMapAttribute("INCOTERMS2", typeof(string), 28, true)]
        public string INCOTERMS2;

        ///<summary>
        ///SO
        ///</summary>
        [FieldMapAttribute("SO", typeof(string), 10, true)]
        public string SO;

        ///<summary>
        ///SOITEMNO
        ///</summary>
        [FieldMapAttribute("SOITEMNO", typeof(string), 6, true)]
        public string SOITEMNO;

        ///<summary>
        ///SOWBSNO
        ///</summary>
        [FieldMapAttribute("SOWBSNO", typeof(string), 8, true)]
        public string SOWBSNO;

        ///<summary>
        ///CCNO
        ///</summary>
        [FieldMapAttribute("CCNO", typeof(string), 10, true)]
        public string CCNO;

        /////<summary>
        /////CUSBATCHNO
        /////</summary>
        //[FieldMapAttribute("CUSBATCHNO", typeof(string), 60, true)]
        //public string CUSBATCHNO;

        ///<summary>
        ///HIGNLEVELITEM
        ///</summary>
        [FieldMapAttribute("HIGNLEVELITEM", typeof(int), 22, true)]
        public int HIGNLEVELITEM;

        ///<summary>
        ///MOVEMENTTYPE
        ///</summary>
        [FieldMapAttribute("MOVEMENTTYPE", typeof(string), 4, true)]
        public string MOVEMENTTYPE;

        ///<summary>
        ///CUSMCODE
        ///</summary>
        [FieldMapAttribute("CUSMCODE", typeof(string), 40, true)]
        public string CUSMCODE;

        ///<summary>
        ///CUSITEMSPEC
        ///</summary>
        [FieldMapAttribute("CUSITEMSPEC", typeof(string), 40, true)]
        public string CUSITEMSPEC;

        ///<summary>
        ///CUSITEMDESC
        ///</summary>
        [FieldMapAttribute("CUSITEMDESC", typeof(string), 40, true)]
        public string CUSITEMDESC;

        ///<summary>
        ///VENDERMCODE
        ///</summary>
        [FieldMapAttribute("VENDERMCODE", typeof(string), 40, true)]
        public string VENDERMCODE;

        ///<summary>
        ///GFHWMCODE
        ///</summary>
        [FieldMapAttribute("GFHWMCODE", typeof(string), 12, true)]
        public string GFHWMCODE;

        ///<summary>
        ///GFPACKINGSEQ
        ///</summary>
        [FieldMapAttribute("GFPACKINGSEQ", typeof(string), 6, true)]
        public string GFPACKINGSEQ;

        ///<summary>
        ///GFHWDESC
        ///</summary>
        [FieldMapAttribute("GFHWDESC", typeof(string), 40, true)]
        public string GFHWDESC;

        ///<summary>
        ///HWCODEQTY
        ///</summary>
        [FieldMapAttribute("HWCODEQTY", typeof(string), 40, true)]
        public string HWCODEQTY;

        ///<summary>
        ///HWCODEUNIT
        ///</summary>
        [FieldMapAttribute("HWCODEUNIT", typeof(string), 40, true)]
        public string HWCODEUNIT;

        ///<summary>
        ///HWTYPEINFO
        ///</summary>
        [FieldMapAttribute("HWTYPEINFO", typeof(string), 40, true)]
        public string HWTYPEINFO;

        ///<summary>
        ///PACKINGWAY
        ///</summary>
        [FieldMapAttribute("PACKINGWAY", typeof(string), 40, true)]
        public string PACKINGWAY;

        ///<summary>
        ///PACKINGNO
        ///</summary>
        [FieldMapAttribute("PACKINGNO", typeof(string), 40, true)]
        public string PACKINGNO;

        ///<summary>
        ///PACKINGSPEC
        ///</summary>
        [FieldMapAttribute("PACKINGSPEC", typeof(string), 40, true)]
        public string PACKINGSPEC;

        ///<summary>
        ///PACKINGWAYNO
        ///</summary>
        [FieldMapAttribute("PACKINGWAYNO", typeof(string), 40, true)]
        public string PACKINGWAYNO;

        ///<summary>
        ///DQSMCODE
        ///</summary>
        [FieldMapAttribute("DQSMCODE", typeof(string), 40, true)]
        public string DQSMCODE;

        ///<summary>
        ///RECEIVERADDR
        ///</summary>
        [FieldMapAttribute("RECEIVERADDR", typeof(string), 240, true)]
        public string RECEIVERADDR;

        ///<summary>
        ///RECEIVERUSER
        ///</summary>
        [FieldMapAttribute("RECEIVERUSER", typeof(string), 50, true)]
        public string RECEIVERUSER;

        ///<summary>
        ///CUSTMCODE
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 35, true)]
        public string CUSTMCODE;

        ///<summary>
        ///RECEIVEMCODE
        ///</summary>
        [FieldMapAttribute("RECEIVEMCODE", typeof(string), 35, true)]
        public string RECEIVEMCODE;

        ///<summary>
        ///DEMANDARRIVALDATE
        ///</summary>
        [FieldMapAttribute("DEMANDARRIVALDATE", typeof(int), 22, true)]
        public int DEMANDARRIVALDATE;

        ///<summary>
        ///NEEDDATE
        ///</summary>
        [FieldMapAttribute("NEEDDATE", typeof(int), 22, true)]
        public int NEEDDATE;

        ///<summary>
        ///TYPE
        ///</summary>
        [FieldMapAttribute("TYPE", typeof(string), 3, true)]
        public string TYPE;

        ///<summary>
        ///CUSER
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUSER;

        ///<summary>
        ///CDATE
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDATE;

        ///<summary>
        ///CTIME
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTIME;

        ///<summary>
        ///MAINTAINUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;

    }
    #endregion

    //#region Invoicesdetail-- SAP������ϸ�� add by jinger 2016-01-25
    ///// <summary>
    ///// TBLINVOICESDETAIL-- SAP������ϸ�� add by jinger 2016-01-25
    ///// </summary>
    //[Serializable, TableMap("TBLINVOICESDETAIL", "INVNO,INVLINE")]
    //public class InvoicesDetail : BenQGuru.eMES.Common.Domain.DomainObject
    //{
    //    public InvoicesDetail()
    //    {
    //    }

    //    ///<summary>
    //    ///Ԥ���ֶ�3
    //    ///</summary>
    //    [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
    //    public string Eattribute3;

    //    ///<summary>
    //    ///Ԥ���ֶ�2
    //    ///</summary>
    //    [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
    //    public string Eattribute2;

    //    ///<summary>
    //    ///Ԥ���ֶ�1
    //    ///</summary>
    //    [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
    //    public string Eattribute1;

    //    ///<summary>
    //    ///ά��ʱ��
    //    ///</summary>
    //    [FieldMapAttribute("MTIME", typeof(int), 22, false)]
    //    public int MaintainTime;

    //    ///<summary>
    //    ///ά������
    //    ///</summary>
    //    [FieldMapAttribute("MDATE", typeof(int), 22, false)]
    //    public int MaintainDate;

    //    ///<summary>
    //    ///ά����
    //    ///</summary>
    //    [FieldMapAttribute("MUSER", typeof(string), 40, false)]
    //    public string MaintainUser;

    //    ///<summary>
    //    ///����ʱ��
    //    ///</summary>
    //    [FieldMapAttribute("CTIME", typeof(int), 22, false)]
    //    public int CTime;

    //    ///<summary>
    //    ///��������
    //    ///</summary>
    //    [FieldMapAttribute("CDATE", typeof(int), 22, false)]
    //    public int CDate;

    //    ///<summary>
    //    ///������
    //    ///</summary>
    //    [FieldMapAttribute("CUSER", typeof(string), 40, false)]
    //    public string CUser;

    //    ///<summary>
    //    ///�̿�/��ӯ��ʶ��702/701��(SC)
    //    ///</summary>
    //    [FieldMapAttribute("TYPE", typeof(string), 3, true)]
    //    public string Type;

    //    ///<summary>
    //    ///ϣ���������ڣ��������ڣ�(RS)
    //    ///</summary>
    //    [FieldMapAttribute("NEEDDATE", typeof(int), 22, true)]
    //    public int NeedDate;

    //    ///<summary>
    //    ///Ҫ�󵽻�ʱ��(UB)
    //    ///</summary>
    //    [FieldMapAttribute("DEMANDARRIVALDATE", typeof(int), 22, true)]
    //    public int DemandArrivalDate;

    //    ///<summary>
    //    ///�ջ������Ϻţ�Item text��(UB)
    //    ///</summary>
    //    [FieldMapAttribute("RECEIVEMCODE", typeof(string), 35, true)]
    //    public string ReceiveMCode;

    //    ///<summary>
    //    ///��Ϊ���Ϻţ�44118421��(UB,RS)
    //    ///</summary>
    //    [FieldMapAttribute("CUSTMCODE", typeof(string), 35, true)]
    //    public string CustmCode;

    //    ///<summary>
    //    ///�ջ���ϵ�˼���ϵ��ʽ(UB,RS)
    //    ///</summary>
    //    [FieldMapAttribute("RECEIVERUSER", typeof(string), 50, true)]
    //    public string ReceiverUser;

    //    ///<summary>
    //    ///����λ��ַ,�ջ�/������ַ(UB,RS)
    //    ///</summary>
    //    [FieldMapAttribute("RECEIVERADDR", typeof(string), 240, true)]
    //    public string ReceiverAddr;

    //    ///<summary>
    //    ///����S����{���Ϻ�}��SAP ��Ҫ����ȡ���߼��������ӱ�ʶλ���ֹ����SO(DN)
    //    ///</summary>
    //    [FieldMapAttribute("DQSMCODE", typeof(string), 40, true)]
    //    public string DQSMCode;

    //    ///<summary>
    //    ///��װ�䷽ʽȡ�� ��SAP�Խ���ȡ����������У�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("PACKINGWAYNO", typeof(string), 40, true)]
    //    public string PackingWayNo;

    //    ///<summary>
    //    ///��װ���� ��SAP�Խ���ȡ����������У�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("PACKINGSPEC", typeof(string), 40, true)]
    //    public string PackingSpec;

    //    ///<summary>
    //    ///��װ���� ��SAP�Խ���ȡ����������У�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("PACKINGNO", typeof(string), 40, true)]
    //    public string PackingNo;

    //    ///<summary>
    //    ///��װ��ʽ ��SAP�Խ���ȡ����������У�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("PACKINGWAY", typeof(string), 40, true)]
    //    public string PackingWay;

    //    ///<summary>
    //    ///��Ϊ�ͺű�ǩ��Ϣ ��SAP�Խ���ȡ����������У�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("HWTYPEINFO", typeof(string), 40, true)]
    //    public string HWTypeInfo;

    //    ///<summary>
    //    ///��Ϊ����������λ(DN)
    //    ///</summary>
    //    [FieldMapAttribute("HWCODEUNIT", typeof(string), 40, true)]
    //    public string HWCodeUnit;

    //    ///<summary>
    //    ///��Ϊ�������� ��SAP��������ģ�������У�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("HWCODEQTY", typeof(string), 40, true)]
    //    public string HWCodeQty;

    //    ///<summary>
    //    ///�����Ϊ����(DN)
    //    ///</summary>
    //    [FieldMapAttribute("GFHWDESC", typeof(string), 40, true)]
    //    public string GFhwDesc;

    //    ///<summary>
    //    ///�����װ��� Purchase order item(SAP���������)(DN)
    //    ///</summary>
    //    [FieldMapAttribute("GFPACKINGSEQ", typeof(string), 6, true)]
    //    public string GFPackingSeq;

    //    ///<summary>
    //    ///�����Ϊ���� item your reference (SAP)(DN)
    //    ///</summary>
    //    [FieldMapAttribute("GFHWMCODE", typeof(string), 12, true)]
    //    public string GFhwMCode;

    //    ///<summary>
    //    ///��Ӧ�����ϱ���_verdor material number (SAP��ȡ���߼��ο�download����) (DN)
    //    ///</summary>
    //    [FieldMapAttribute("VENDERMCODE", typeof(string), 40, true)]
    //    public string VenderMCode;

    //    ///<summary>
    //    ///�ͻ�����������SAP) from SO(DN)
    //    ///</summary>
    //    [FieldMapAttribute("CUSITEMDESC", typeof(string), 40, true)]
    //    public string CusItemDesc;

    //    ///<summary>
    //    ///�ͻ������ͺţ�SAP) from SO(DN)
    //    ///</summary>
    //    [FieldMapAttribute("CUSITEMSPEC", typeof(string), 40, true)]
    //    public string CusItemspec;

    //    ///<summary>
    //    ///�ͻ����ϱ��루SAP) from SO(DN)
    //    ///</summary>
    //    [FieldMapAttribute("CUSMCODE", typeof(string), 40, true)]
    //    public string CusMCode;

    //    ///<summary>
    //    ///�ƶ�����(DN)
    //    ///</summary>
    //    [FieldMapAttribute("MOVEMENTTYPE", typeof(string), 4, true)]
    //    public string MovementType;

    //    ///<summary>
    //    ///�ϲ�����Ŀ��(Hign Level Item)(DN)
    //    ///</summary>
    //    [FieldMapAttribute("HIGNLEVELITEM", typeof(int), 22, true)]
    //    public int HignLevelItem;

    //    ///<summary>
    //    ///�����ͬ�� Header your reference (SAP���������)(DN)
    //    ///</summary>
    //    [FieldMapAttribute("GFCONTRACTNO", typeof(string), 12, true)]
    //    public string GfContractNo;

    //    ///<summary>
    //    ///�ƻ��������� Planed GI date (SAP)(DN)
    //    ///</summary>
    //    [FieldMapAttribute("PLANGIDATE", typeof(string), 40, true)]
    //    public string PlanGIDate;

    //    ///<summary>
    //    ///�ջ���ַ shipping  location ��SAP) (DN)
    //    ///</summary>
    //    [FieldMapAttribute("SHIPPINGLOCATION", typeof(string), 40, true)]
    //    public string ShippingLocation;

    //    ///<summary>
    //    ///�ͻ����κ�(SAP)\��Ŀ����(DN)
    //    ///</summary>
    //    [FieldMapAttribute("CUSBATCHNO", typeof(string), 60, true)]
    //    public string CusBatchNo;

    //    ///<summary>
    //    ///�������ͣ�SO/PO)��SAP_SO�������ͣ�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("CUSORDERNOTYPE", typeof(string), 2, true)]
    //    public string CusOrderNoType;

    //    ///<summary>
    //    ///�����ţ�SO/PO)��SAP SO�ţ�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("CUSORDERNO", typeof(string), 10, true)]
    //    public string CusOrderNo;

    //    ///<summary>
    //    ///��ͬ��(SAP_SO PO�ţ�(DN)
    //    ///</summary>
    //    [FieldMapAttribute("ORDERNO", typeof(string), 20, true)]
    //    public string OrderNo;

    //    ///<summary>
    //    ///����������SAP)(B B��S)(DN)
    //    ///</summary>
    //    [FieldMapAttribute("PICKCONDITION", typeof(string), 40, true)]
    //    public string PickCondition;

    //    ///<summary>
    //    ///������ʽ ��SAP)(����ֱ�������ֽ����������Ӽ�����)(DN)
    //    ///</summary>
    //    [FieldMapAttribute("POSTWAY", typeof(string), 40, true)]
    //    public string PostWay;

    //    ///<summary>
    //    ///����ԭ�� Order reason (SAP) (DN)
    //    ///</summary>
    //    [FieldMapAttribute("ORDERREASON", typeof(string), 40, true)]
    //    public string OrderReason;

    //    ///<summary>
    //    ///�ʹ﷽(DN)
    //    ///</summary>
    //    [FieldMapAttribute("SHIPTOPARTY", typeof(string), 10, true)]
    //    public string ShipToParty;

    //    ///<summary>
    //    ///����Ŀ��(DN)
    //    ///</summary>
    //    [FieldMapAttribute("DNLINE", typeof(int), 22, true)]
    //    public int DNLine;

    //    ///<summary>
    //    ///��������(DN)
    //    ///</summary>
    //    [FieldMapAttribute("DNNO", typeof(string), 10, true)]
    //    public string DNNo;

    //    ///<summary>
    //    ///�ɱ�����(CC��)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("CCNO", typeof(string), 10, true)]
    //    public string CcNo;

    //    ///<summary>
    //    ///SO WBS��(PO)
    //    ///</summary>
    //    [FieldMapAttribute("SOWBSNO", typeof(string), 8, true)]
    //    public string SOWBSNo;

    //    ///<summary>
    //    ///���۶�������Ŀ��(SO item��)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("SOITEMNO", typeof(string), 6, true)]
    //    public string SOItemNo;

    //    ///<summary>
    //    ///���۶�����(SO��)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("SO", typeof(string), 10, true)]
    //    public string SO;

    //    ///<summary>
    //    ///����ó����������(incoterms 2)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("INCOTERMS2", typeof(string), 28, true)]
    //    public string Incoterms2;

    //    ///<summary>
    //    ///����ó������(incoterms 1)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("INCOTERMS1", typeof(string), 3, true)]
    //    public string Incoterms1;

    //    ///<summary>
    //    ///����Ŀ���item category��(PO)
    //    ///</summary>
    //    [FieldMapAttribute("ITEMCATEGORY", typeof(string), 1, true)]
    //    public string ItemCategory;

    //    ///<summary>
    //    ///��Ŀ���䣨account assignment��(PO)
    //    ///</summary>
    //    [FieldMapAttribute("ACCOUNTASSIGNMENT", typeof(string), 1, true)]
    //    public string AccountAssignment;

    //    ///<summary>
    //    ///�˻���ʶ(Return PO��ʶ)(X=�˻�PO����=����PO)(PO)
    //    ///</summary>
    //    [FieldMapAttribute("RETURNFLAG", typeof(string), 1, true)]
    //    public string ReturnFlag;

    //    ///<summary>
    //    ///�ɹ������(PR��)(PO,RS)
    //    ///</summary>
    //    [FieldMapAttribute("PRNO", typeof(string), 10, true)]
    //    public string PrNo;

    //    ///<summary>
    //    ///��Ӧ�����ϱ���(PO)
    //    ///</summary>
    //    [FieldMapAttribute("VENDORMCODE", typeof(string), 35, true)]
    //    public string VendorMCode;

    //    ///<summary>
    //    ///����Ŀ��ע��item text��(PO)
    //    ///</summary>
    //    [FieldMapAttribute("DETAILREMARK", typeof(string), 220, true)]
    //    public string DetailRemark;

    //    ///<summary>
    //    ///����Ŀ״̬(PO,UB,RS)
    //    ///</summary>
    //    [FieldMapAttribute("STATUS", typeof(string), 1, true)]
    //    public string Status;

    //    ///<summary>
    //    ///�ͻ���ַ������Ŀ��(PO)
    //    ///</summary>
    //    [FieldMapAttribute("SHIPADDR", typeof(string), 240, true)]
    //    public string ShipAddr;

    //    ///<summary>
    //    ///���ϵ�λ(PO,DN)
    //    ///</summary>
    //    [FieldMapAttribute("UNIT", typeof(string), 3, true)]
    //    public string Unit;

    //    ///<summary>
    //    ///�������ڣ������գ�(PO)
    //    ///</summary>
    //    [FieldMapAttribute("PLANDATE", typeof(int), 22, true)]
    //    public int PlanDate;


    //    ///<summary>
    //    ///�ѳ�������
    //    ///</summary>
    //    [FieldMapAttribute("OUTQTY", typeof(decimal), 25, true)]
    //    public decimal OutQty;

    //    ///<summary>
    //    ///���������
    //    ///</summary>
    //    [FieldMapAttribute("ACTQTY", typeof(decimal), 25, true)]
    //    public decimal ActQty;

    //    ///<summary>
    //    ///��������(PO,DN)
    //    ///</summary>
    //    [FieldMapAttribute("PLANQTY", typeof(decimal), 25, false)]
    //    public decimal PlanQty;

    //    ///<summary>
    //    ///���ϳ�����(PO)
    //    ///</summary>
    //    [FieldMapAttribute("MLONGDESC", typeof(string), 220, true)]
    //    public string MLongDesc;

    //    ///<summary>
    //    ///���϶�����(PO)
    //    ///</summary>
    //    [FieldMapAttribute("MENSHORTDESC", typeof(string), 40, true)]
    //    public string MENShortDesc;

    //    ///<summary>
    //    ///�������Ϻ�
    //    ///</summary>
    //    [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
    //    public string DQMCode;

    //    ///<summary>
    //    ///���ϱ���(PO,DN)
    //    ///</summary>
    //    [FieldMapAttribute("MCODE", typeof(string), 18, false)]
    //    public string MCode;

    //    ///<summary>
    //    ///��λ(PO,DN,UB)
    //    ///</summary>
    //    [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
    //    public string StorageCode;

    //    ///<summary>
    //    ///��������(Plant)(PO,DN,UB)
    //    ///</summary>
    //    [FieldMapAttribute("FACCODE", typeof(string), 4, true)]
    //    public string FacCode;

    //    ///<summary>
    //    ///�����λ( item requisitioner)(UB)
    //    ///</summary>
    //    [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 12, true)]
    //    public string FromStorageCode;

    //    ///<summary>
    //    ///��������(10Y2)(UB)
    //    ///</summary>
    //    [FieldMapAttribute("FROMFACCODE", typeof(string), 10, true)]
    //    public string FromFacCode;

    //    ///<summary>
    //    ///SAP���ݺ���״̬
    //    ///</summary>
    //    [FieldMapAttribute("INVLINESTATUS", typeof(string), 1, true)]
    //    public string InvLineStatus;

    //    ///<summary>
    //    ///SAP���ݺ�����Ŀ��
    //    ///</summary>
    //    [FieldMapAttribute("INVLINE", typeof(int), 22, false)]
    //    public int InvLine;

    //    ///<summary>
    //    ///SAP���ݺ�
    //    ///</summary>
    //    [FieldMapAttribute("INVNO", typeof(string), 40, false)]
    //    public string InvNo;

    //}
    //#endregion

    /// <summary>
    /// SAP������ϸ��չʵ��
    /// </summary>
    [Serializable]
    public class InvoicesDetailExt : InvoicesDetail
    {
        ///<summary>
        ///SAP��������
        ///</summary>
        [FieldMapAttribute("INVTYPE", typeof(string), 40, false)]
        public string InvType;

        #region add by sam
        ///<summary>
        ///ORDERNO
        ///</summary>
        [FieldMapAttribute("ORDERNO", typeof(string), 140, true)]
        public string ORDERNO;

        ///<summary>
        ///CUSORDERNO
        ///</summary>
        [FieldMapAttribute("CUSORDERNO", typeof(string), 40, true)]
        public string CUSORDERNO;

        ///<summary>
        ///CUSORDERNOTYPE
        ///</summary>
        [FieldMapAttribute("CUSORDERNOTYPE", typeof(string), 16, true)]
        public string CUSORDERNOTYPE;

        ///<summary>
        ///CUSBATCHNO
        ///</summary>
        [FieldMapAttribute("CUSBATCHNO", typeof(string), 240, true)]
        public string CUSBATCHNO;

        ///<summary>
        ///SHIPPINGLOCATION
        ///</summary>
        [FieldMapAttribute("SHIPPINGLOCATION", typeof(string), 528, true)]
        public string SHIPPINGLOCATION;

        ///<summary>
        ///GFCONTRACTNO
        ///</summary>
        [FieldMapAttribute("GFCONTRACTNO", typeof(string), 48, true)]
        public string GFCONTRACTNO;


        ///<summary>
        ///ORDERREASON
        ///</summary>
        [FieldMapAttribute("ORDERREASON", typeof(string), 12, true)]
        public string ORDERREASON;

        ///<summary>
        ///POSTWAY
        ///</summary>
        [FieldMapAttribute("POSTWAY", typeof(string), 40, true)]
        public string POSTWAY;

        ///<summary>
        ///PICKCONDITION
        ///</summary>
        [FieldMapAttribute("PICKCONDITION", typeof(string), 32, true)]
        public string PICKCONDITION;

        ///<summary>
        ///GFFLAG
        ///</summary>
        [FieldMapAttribute("GFFLAG", typeof(string), 40, true)]
        public string GFFLAG;

        ///<summary>
        ///INVSTATUS
        ///</summary>
        [FieldMapAttribute("INVSTATUS", typeof(string), 40, true)]
        public string INVSTATUS;
        #endregion
    }


    [Serializable]
    public class InvoicesDetailExt1 : BenQGuru.eMES.Common.Domain.DomainObject
    {
        //add by sam �б�ע
        //[FieldMapAttribute("LINEREMARK", typeof(string), 160, true)]
        //public string LineRemark;

        ///<summary>
        ///SAP��������
        ///</summary>
        [FieldMapAttribute("INVTYPE", typeof(string), 40, false)]
        public string InvType;

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
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;

        ///<summary>
        ///Eattribute2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string Eattribute2;

        ///<summary>
        ///Eattribute3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string Eattribute3;

        ///<summary>
        ///Invno
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, false)]
        public string InvNo;

        ///<summary>
        ///Invline
        ///</summary>
        [FieldMapAttribute("INVLINE", typeof(int), 22, false)]
        public int InvLine;

        ///<summary>
        ///InvlineStatus
        ///</summary>
        [FieldMapAttribute("INVLINESTATUS", typeof(string), 40, true)]
        public string InvLineStatus;

        /////<summary>
        /////FromfacCode
        /////</summary>
        //[FieldMapAttribute("FROMFACCODE", typeof(string), 10, true)]
        //public string FromFacCode;

        ///<summary>
        ///FromstorageCode
        ///</summary>
        [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 12, true)]
        public string FromStorageCode;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 4, true)]
        public string FacCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 18, false)]
        public string MCode;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCode;

        ///<summary>
        ///MenshortDesc
        ///</summary>
        [FieldMapAttribute("MENSHORTDESC", typeof(string), 160, true)]
        public string MenshortDesc;

        ///<summary>
        ///MlongDesc
        ///</summary>
        [FieldMapAttribute("MLONGDESC", typeof(string), 880, true)]
        public string MLongDesc;

        ///<summary>
        ///PlanQty
        ///</summary>
        [FieldMapAttribute("PLANQTY", typeof(decimal), 22, false)]
        public decimal PlanQty;

        ///<summary>
        ///ActQty
        ///</summary>
        [FieldMapAttribute("ACTQTY", typeof(decimal), 22, true)]
        public decimal ActQty;

        ///<summary>
        ///OutQty
        ///</summary>
        [FieldMapAttribute("OUTQTY", typeof(decimal), 22, true)]
        public decimal OutQty;

        ///<summary>
        ///PlanDate
        ///</summary>
        [FieldMapAttribute("PLANDATE", typeof(int), 22, true)]
        public int PlanDate;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 3, true)]
        public string Unit;

        ///<summary>
        ///Shipaddr
        ///</summary>
        [FieldMapAttribute("SHIPADDR", typeof(string), 240, true)]
        public string ShipAddr;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string Status;

        ///<summary>
        ///Detailremark
        ///</summary>
        [FieldMapAttribute("DETAILREMARK", typeof(string), 220, true)]
        public string DetailRemark;

        ///<summary>
        ///VEndormCode
        ///</summary>
        [FieldMapAttribute("VENDORMCODE", typeof(string), 140, true)]
        public string VEndorMCode;

        ///<summary>
        ///Prno
        ///</summary>
        [FieldMapAttribute("PRNO", typeof(string), 10, true)]
        public string PrNo;

        ///<summary>
        ///Returnflag
        ///</summary>
        [FieldMapAttribute("RETURNFLAG", typeof(string), 1, true)]
        public string ReturnFlag;

        ///<summary>
        ///Accountassignment
        ///</summary>
        [FieldMapAttribute("ACCOUNTASSIGNMENT", typeof(string), 1, true)]
        public string AccountAssignMent;

        ///<summary>
        ///Itemcategory
        ///</summary>
        [FieldMapAttribute("ITEMCATEGORY", typeof(string), 1, true)]
        public string ItemCategory;

        ///<summary>
        ///Incoterms1
        ///</summary>
        [FieldMapAttribute("INCOTERMS1", typeof(string), 3, true)]
        public string Incoterms1;

        ///<summary>
        ///Incoterms2
        ///</summary>
        [FieldMapAttribute("INCOTERMS2", typeof(string), 28, true)]
        public string Incoterms2;

        ///<summary>
        ///So
        ///</summary>
        [FieldMapAttribute("SO", typeof(string), 10, true)]
        public string So;

        ///<summary>
        ///Soitemno
        ///</summary>
        [FieldMapAttribute("SOITEMNO", typeof(string), 6, true)]
        public string SoItemNo;

        ///<summary>
        ///Sowbsno
        ///</summary>
        [FieldMapAttribute("SOWBSNO", typeof(string), 8, true)]
        public string SowBSNo;

        ///<summary>
        ///Ccno
        ///</summary>
        [FieldMapAttribute("CCNO", typeof(string), 10, true)]
        public string CcNo;

        /////<summary>
        /////Cusbatchno
        /////</summary>
        //[FieldMapAttribute("CUSBATCHNO", typeof(string), 60, true)]
        //public string CusBatchNo;

        ///<summary>
        ///Hignlevelitem
        ///</summary>
        [FieldMapAttribute("HIGNLEVELITEM", typeof(int), 22, true)]
        public int HignLevelItem;

        ///<summary>
        ///MovementType
        ///</summary>
        [FieldMapAttribute("MOVEMENTTYPE", typeof(string), 4, true)]
        public string MovementType;

        ///<summary>
        ///CusmCode
        ///</summary>
        [FieldMapAttribute("CUSMCODE", typeof(string), 40, true)]
        public string CusMCode;

        ///<summary>
        ///Cusitemspec
        ///</summary>
        [FieldMapAttribute("CUSITEMSPEC", typeof(string), 40, true)]
        public string CusitemSpec;

        ///<summary>
        ///CusitemDesc
        ///</summary>
        [FieldMapAttribute("CUSITEMDESC", typeof(string), 40, true)]
        public string CusitemDesc;

        ///<summary>
        ///VEndermCode
        ///</summary>
        [FieldMapAttribute("VENDERMCODE", typeof(string), 40, true)]
        public string VEnderMCode;

        ///<summary>
        ///GfhwmCode
        ///</summary>
        [FieldMapAttribute("GFHWMCODE", typeof(string), 12, true)]
        public string GfhwmCode;

        ///<summary>
        ///Gfpackingseq
        ///</summary>
        [FieldMapAttribute("GFPACKINGSEQ", typeof(string), 6, true)]
        public string GfPackingSeq;

        ///<summary>
        ///GfhwDesc
        ///</summary>
        [FieldMapAttribute("GFHWDESC", typeof(string), 40, true)]
        public string GfhwDesc;

        ///<summary>
        ///HwCodeQty
        ///</summary>
        [FieldMapAttribute("HWCODEQTY", typeof(string), 40, true)]
        public string HwCodeQty;

        ///<summary>
        ///HwCodeunit
        ///</summary>
        [FieldMapAttribute("HWCODEUNIT", typeof(string), 40, true)]
        public string HwCodeUnit;

        ///<summary>
        ///HwTypeinfo
        ///</summary>
        [FieldMapAttribute("HWTYPEINFO", typeof(string), 40, true)]
        public string HwTypeInfo;

        ///<summary>
        ///Packingway
        ///</summary>
        [FieldMapAttribute("PACKINGWAY", typeof(string), 40, true)]
        public string PackingWay;

        ///<summary>
        ///Packingno
        ///</summary>
        [FieldMapAttribute("PACKINGNO", typeof(string), 40, true)]
        public string PackingNo;

        ///<summary>
        ///Packingspec
        ///</summary>
        [FieldMapAttribute("PACKINGSPEC", typeof(string), 40, true)]
        public string PackingSpec;

        ///<summary>
        ///Packingwayno
        ///</summary>
        [FieldMapAttribute("PACKINGWAYNO", typeof(string), 40, true)]
        public string PackingWayNo;

        ///<summary>
        ///DqsmCode
        ///</summary>
        [FieldMapAttribute("DQSMCODE", typeof(string), 40, true)]
        public string DqSmCode;

        ///<summary>
        ///Receiveraddr
        ///</summary>
        [FieldMapAttribute("RECEIVERADDR", typeof(string), 240, true)]
        public string ReceiverAddr;

        ///<summary>
        ///ReceiverUser
        ///</summary>
        [FieldMapAttribute("RECEIVERUSER", typeof(string), 50, true)]
        public string ReceiverUser;

        ///<summary>
        ///CustmCode
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 35, true)]
        public string CustmCode;

        ///<summary>
        ///ReceivemCode
        ///</summary>
        [FieldMapAttribute("RECEIVEMCODE", typeof(string), 35, true)]
        public string ReceiveMCode;

        ///<summary>
        ///DemandarrivalDate
        ///</summary>
        [FieldMapAttribute("DEMANDARRIVALDATE", typeof(int), 22, true)]
        public int DemandArrivalDate;

        ///<summary>
        ///NeedDate
        ///</summary>
        [FieldMapAttribute("NEEDDATE", typeof(int), 22, true)]
        public int NeedDate;

        ///<summary>
        ///Type
        ///</summary>
        [FieldMapAttribute("TYPE", typeof(string), 3, true)]
        public string Type;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

        ///<summary>
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }


    #region Invoicesdetail   //��ṹ�仯��Amy�޸�  @20160317
    /// <summary>
    /// TBLINVOICESDETAIL
    /// </summary>
    [Serializable, TableMap("TBLINVOICESDETAIL", "INVNO,INVLINE")]
    public class InvoicesDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public InvoicesDetail()
        {
        }

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
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;

        ///<summary>
        ///Eattribute2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string Eattribute2;

        ///<summary>
        ///Eattribute3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string Eattribute3;

        ///<summary>
        ///Invno
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, false)]
        public string InvNo;

        ///<summary>
        ///Invline
        ///</summary>
        [FieldMapAttribute("INVLINE", typeof(int), 22, false)]
        public int InvLine;

        ///<summary>
        ///InvlineStatus
        ///</summary>
        [FieldMapAttribute("INVLINESTATUS", typeof(string), 40, true)]
        public string InvLineStatus;

        /////<summary>
        /////FromfacCode
        /////</summary>
        //[FieldMapAttribute("FROMFACCODE", typeof(string), 10, true)]
        //public string FromFacCode;

        ///<summary>
        ///FromstorageCode
        ///</summary>
        [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 12, true)]
        public string FromStorageCode;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 4, true)]
        public string FacCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 18, false)]
        public string MCode;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCode;

        ///<summary>
        ///MenshortDesc
        ///</summary>
        [FieldMapAttribute("MENSHORTDESC", typeof(string), 160, true)]
        public string MenshortDesc;

        ///<summary>
        ///MlongDesc
        ///</summary>
        [FieldMapAttribute("MLONGDESC", typeof(string), 880, true)]
        public string MLongDesc;

        ///<summary>
        ///PlanQty
        ///</summary>
        [FieldMapAttribute("PLANQTY", typeof(decimal), 22, false)]
        public decimal PlanQty;

        ///<summary>
        ///ActQty
        ///</summary>
        [FieldMapAttribute("ACTQTY", typeof(decimal), 22, true)]
        public decimal ActQty;

        ///<summary>
        ///OutQty
        ///</summary>
        [FieldMapAttribute("OUTQTY", typeof(decimal), 22, true)]
        public decimal OutQty;

        ///<summary>
        ///PlanDate
        ///</summary>
        [FieldMapAttribute("PLANDATE", typeof(int), 22, true)]
        public int PlanDate;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 3, true)]
        public string Unit;

        ///<summary>
        ///Shipaddr
        ///</summary>
        [FieldMapAttribute("SHIPADDR", typeof(string), 240, true)]
        public string ShipAddr;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string Status;

        ///<summary>
        ///Detailremark
        ///</summary>
        [FieldMapAttribute("DETAILREMARK", typeof(string), 220, true)]
        public string DetailRemark;

        ///<summary>
        ///VEndormCode
        ///</summary>
        [FieldMapAttribute("VENDORMCODE", typeof(string), 140, true)]
        public string VEndorMCode;

        ///<summary>
        ///Prno
        ///</summary>
        [FieldMapAttribute("PRNO", typeof(string), 10, true)]
        public string PrNo;

        ///<summary>
        ///Returnflag
        ///</summary>
        [FieldMapAttribute("RETURNFLAG", typeof(string), 1, true)]
        public string ReturnFlag;

        ///<summary>
        ///Accountassignment
        ///</summary>
        [FieldMapAttribute("ACCOUNTASSIGNMENT", typeof(string), 1, true)]
        public string AccountAssignMent;

        ///<summary>
        ///Itemcategory
        ///</summary>
        [FieldMapAttribute("ITEMCATEGORY", typeof(string), 1, true)]
        public string ItemCategory;

        ///<summary>
        ///Incoterms1
        ///</summary>
        [FieldMapAttribute("INCOTERMS1", typeof(string), 3, true)]
        public string Incoterms1;

        ///<summary>
        ///Incoterms2
        ///</summary>
        [FieldMapAttribute("INCOTERMS2", typeof(string), 28, true)]
        public string Incoterms2;

        ///<summary>
        ///So
        ///</summary>
        [FieldMapAttribute("SO", typeof(string), 10, true)]
        public string So;

        ///<summary>
        ///Soitemno
        ///</summary>
        [FieldMapAttribute("SOITEMNO", typeof(string), 6, true)]
        public string SoItemNo;

        ///<summary>
        ///Sowbsno
        ///</summary>
        [FieldMapAttribute("SOWBSNO", typeof(string), 8, true)]
        public string SowBSNo;

        ///<summary>
        ///Ccno
        ///</summary>
        [FieldMapAttribute("CCNO", typeof(string), 10, true)]
        public string CcNo;

        /////<summary>
        /////Cusbatchno
        /////</summary>
        //[FieldMapAttribute("CUSBATCHNO", typeof(string), 60, true)]
        //public string CusBatchNo;

        ///<summary>
        ///Hignlevelitem
        ///</summary>
        [FieldMapAttribute("HIGNLEVELITEM", typeof(int), 22, true)]
        public int HignLevelItem;

        ///<summary>
        ///MovementType
        ///</summary>
        [FieldMapAttribute("MOVEMENTTYPE", typeof(string), 4, true)]
        public string MovementType;

        ///<summary>
        ///CusmCode
        ///</summary>
        [FieldMapAttribute("CUSMCODE", typeof(string), 40, true)]
        public string CusMCode;

        ///<summary>
        ///Cusitemspec
        ///</summary>
        [FieldMapAttribute("CUSITEMSPEC", typeof(string), 40, true)]
        public string CusitemSpec;

        ///<summary>
        ///CusitemDesc
        ///</summary>
        [FieldMapAttribute("CUSITEMDESC", typeof(string), 40, true)]
        public string CusitemDesc;

        ///<summary>
        ///VEndermCode
        ///</summary>
        [FieldMapAttribute("VENDERMCODE", typeof(string), 40, true)]
        public string VEnderMCode;

        ///<summary>
        ///GfhwmCode
        ///</summary>
        [FieldMapAttribute("GFHWMCODE", typeof(string), 12, true)]
        public string GfhwmCode;

        ///<summary>
        ///Gfpackingseq
        ///</summary>
        [FieldMapAttribute("GFPACKINGSEQ", typeof(string), 6, true)]
        public string GfPackingSeq;

        ///<summary>
        ///GfhwDesc
        ///</summary>
        [FieldMapAttribute("GFHWDESC", typeof(string), 40, true)]
        public string GfhwDesc;

        ///<summary>
        ///HwCodeQty
        ///</summary>
        [FieldMapAttribute("HWCODEQTY", typeof(string), 40, true)]
        public string HwCodeQty;

        ///<summary>
        ///HwCodeunit
        ///</summary>
        [FieldMapAttribute("HWCODEUNIT", typeof(string), 40, true)]
        public string HwCodeUnit;

        ///<summary>
        ///HwTypeinfo
        ///</summary>
        [FieldMapAttribute("HWTYPEINFO", typeof(string), 40, true)]
        public string HwTypeInfo;

        ///<summary>
        ///Packingway
        ///</summary>
        [FieldMapAttribute("PACKINGWAY", typeof(string), 40, true)]
        public string PackingWay;

        ///<summary>
        ///Packingno
        ///</summary>
        [FieldMapAttribute("PACKINGNO", typeof(string), 40, true)]
        public string PackingNo;

        ///<summary>
        ///Packingspec
        ///</summary>
        [FieldMapAttribute("PACKINGSPEC", typeof(string), 40, true)]
        public string PackingSpec;

        ///<summary>
        ///Packingwayno
        ///</summary>
        [FieldMapAttribute("PACKINGWAYNO", typeof(string), 40, true)]
        public string PackingWayNo;

        ///<summary>
        ///DqsmCode
        ///</summary>
        [FieldMapAttribute("DQSMCODE", typeof(string), 40, true)]
        public string DqSmCode;

        ///<summary>
        ///Receiveraddr
        ///</summary>
        [FieldMapAttribute("RECEIVERADDR", typeof(string), 240, true)]
        public string ReceiverAddr;

        ///<summary>
        ///ReceiverUser
        ///</summary>
        [FieldMapAttribute("RECEIVERUSER", typeof(string), 50, true)]
        public string ReceiverUser;

        ///<summary>
        ///CustmCode
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 35, true)]
        public string CustmCode;

        ///<summary>
        ///ReceivemCode
        ///</summary>
        [FieldMapAttribute("RECEIVEMCODE", typeof(string), 35, true)]
        public string ReceiveMCode;

        ///<summary>
        ///DemandarrivalDate
        ///</summary>
        [FieldMapAttribute("DEMANDARRIVALDATE", typeof(int), 22, true)]
        public int DemandArrivalDate;

        ///<summary>
        ///NeedDate
        ///</summary>
        [FieldMapAttribute("NEEDDATE", typeof(int), 22, true)]
        public int NeedDate;

        ///<summary>
        ///Type
        ///</summary>
        [FieldMapAttribute("TYPE", typeof(string), 3, true)]
        public string Type;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

        ///<summary>
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }
    #endregion

    #region InvoicesDetailQuery--

    /// <summary>
    /// TBLINVOICESDETAIL-- SAP������ϸ�� add by jinger 2016-01-25
    /// </summary>
    public class InvoicesDetailQuery : InvoicesDetail
    {
        public InvoicesDetailQuery()
        {
        }

        ///<summary>
        ///���ϳ�����(PO)
        ///</summary>
        [FieldMapAttribute("MCHLONGDESC", typeof(string), 220, true)]
        public string MChLongDesc;

    }
    #endregion

    #region PickQuery
    public class PickQuery : Pick
    {
        public PickQuery()
        {
        }

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

    }
    #endregion

    #region Pick-- ���������ͷ add by jinger 2016-01-27
    /// <summary>
    /// TBLPICK-- ���������ͷ 
    /// </summary>
    [Serializable, TableMap("TBLPICK", "PICKNO")]
    public class Pick : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Pick()
        {
        }

        [FieldMapAttribute("EXPRESSDELEGATE", typeof(string), 40, false)]
        public string ExpressDelegate;

        [FieldMapAttribute("VENDERCODE", typeof(string), 40, false)]
        public string VenderCode;

        ///<summary>
        ///���ָ���
        ///</summary>
        [FieldMapAttribute("StNo", typeof(string), 200, false)]
        public string StNo;

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά������ 
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///����ʱ��
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///������
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///��ע
        ///</summary>
        [FieldMapAttribute("REMARK1", typeof(string), 200, true)]
        public string Remark1;

        ///<summary>
        ///��ⷵ��������
        ///</summary>
        [FieldMapAttribute("REWORKAPPLYUSER", typeof(string), 40, true)]
        public string ReworkapplyUser;

        ///<summary>
        ///�ջ���ַ
        ///</summary>
        [FieldMapAttribute("RECEIVERADDR", typeof(string), 100, true)]
        public string Receiveraddr;

        ///<summary>
        ///�ջ���
        ///</summary>
        [FieldMapAttribute("RECEIVERUSER", typeof(string), 40, true)]
        public string ReceiverUser;

        ///<summary>
        ///����ʱ��
        ///</summary>
        [FieldMapAttribute("DELIVERY_TIME", typeof(int), 22, true)]
        public int DeliveryTime;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("DELIVERY_DATE", typeof(int), 22, true)]
        public int DeliveryDate;

        ///<summary>
        ///�䵥���ʱ��
        ///</summary>
        [FieldMapAttribute("PACKING_LIST_TIME", typeof(int), 22, true)]
        public int PackingListTime;

        ///<summary>
        ///�䵥�������
        ///</summary>
        [FieldMapAttribute("PACKING_LIST_DATE", typeof(int), 22, true)]
        public int PackingListDate;

        ///<summary>
        ///OQC���ʱ��
        ///</summary>
        [FieldMapAttribute("OQC_TIME", typeof(int), 22, true)]
        public int OQCTime;

        ///<summary>
        ///OQC�������
        ///</summary>
        [FieldMapAttribute("OQC_DATE", typeof(int), 22, true)]
        public int OQCDate;

        ///<summary>
        ///�������ʱ��
        ///</summary>
        [FieldMapAttribute("FINISH_TIME", typeof(int), 22, true)]
        public int FinishTime;

        ///<summary>
        ///�����������
        ///</summary>
        [FieldMapAttribute("FINISH_DATE", typeof(int), 22, true)]
        public int FinishDate;

        ///<summary>
        ///�ͻ���ͷ���ʱ��
        ///</summary>
        [FieldMapAttribute("SHIPPING_MARK_TIME", typeof(int), 22, true)]
        public int ShippingMarkTime;

        ///<summary>
        ///�ͻ���ͷ�������
        ///</summary>
        [FieldMapAttribute("SHIPPING_MARK_DATE", typeof(int), 22, true)]
        public int ShippingMarkDate;

        ///<summary>
        ///�ͻ���ͷ�����
        ///</summary>
        [FieldMapAttribute("SHIPPING_MARK_USER", typeof(string), 40, true)]
        public string ShippingMarkUser;

        ///<summary>
        ///�·�ʱ��
        ///</summary>
        [FieldMapAttribute("DOWN_TIME", typeof(int), 22, true)]
        public int DownTime;

        ///<summary>
        ///�·�����
        ///</summary>
        [FieldMapAttribute("DOWN_DATE", typeof(int), 22, true)]
        public int DownDate;

        ///<summary>
        ///�·���
        ///</summary>
        [FieldMapAttribute("DOWN_USER", typeof(string), 40, true)]
        public string DownUser;

        ///<summary>
        ///OA��ˮ�ţ�head Your Reference��
        ///</summary>
        [FieldMapAttribute("OANO", typeof(string), 40, true)]
        public string OANo;

        ///<summary>
        ///�������������ʱ��
        ///</summary>
        [FieldMapAttribute("CREATE_PICK_TIME", typeof(int), 22, true)]
        public int CreatePickTime;

        ///<summary>
        ///�����������������
        ///</summary>
        [FieldMapAttribute("CREATE_PICK_DATE", typeof(int), 22, true)]
        public int CreatePickDate;

        ///<summary>
        ///�����ʶ��From SAP SO field��(DN)
        ///</summary>
        [FieldMapAttribute("GFFLAG", typeof(string), 40, true)]
        public string GFFlag;

        ///<summary>
        ///�ͻ����κţ�SAP ��Ŀ����)(DN)
        ///</summary>
        [FieldMapAttribute("CUSBATCHNO", typeof(string), 40, true)]
        public string CusBatchNo;

        ///<summary>
        ///�ƻ��������� Planed GI date (SAP)(DN)
        ///</summary>
        [FieldMapAttribute("PLANGIDATE", typeof(string), 40, true)]
        public string PlanGIDate;

        ///<summary>
        ///�ƻ���������
        ///</summary>
        [FieldMapAttribute("PLAN_DATE", typeof(int), 22, true)]
        public int PlanDate;

        ///<summary>
        ///�����λ
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        ///<summary>
        ///���⹤��
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FacCode;

        ///<summary>
        ///����λ
        ///</summary>
        [FieldMapAttribute("INSTORAGECODE", typeof(string), 40, true)]
        public string InStorageCode;

        ///<summary>
        ///��⹤��
        ///</summary>
        [FieldMapAttribute("INFACCODE", typeof(string), 40, true)]
        public string InFacCode;

        ///<summary>
        ///PO��
        ///</summary>
        [FieldMapAttribute("PONO", typeof(string), 40, true)]
        public string PONo;

        ///<summary>
        ///���
        ///</summary>
        [FieldMapAttribute("SERIAL_NUMBER", typeof(Int64), 22, true)]
        public Int64 SerialNumber;

        ///<summary>
        ///SAP���ݺ�
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, true)]
        public string InvNo;

        ///<summary>
        ///�ɱ�����
        ///</summary>
        [FieldMapAttribute("CC", typeof(string), 40, true)]
        public string CC;

        ///<summary>
        ///״̬:��ʼ�������·���������ɣ���װ��ɣ�OQCУ����ɣ��䵥��ɣ��ѳ��⣬ȡ��
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///��������������(��ӦSAP��������)
        ///</summary>
        [FieldMapAttribute("PICKTYPE", typeof(string), 40, false)]
        public string PickType;

        ///<summary>
        ///����������
        ///</summary>
        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string PickNo;

    }
    #endregion



    #region ViewField--ѡ���ֶ� add by jinger 2016-01-05
    /// <summary>
    /// ViewField--ѡ���ֶ�
    /// </summary>
    [Serializable, TableMap("TBLVIEWFILED", "USERCODE,SEQ,TABLENAME")]
    public class ViewField : DomainObject
    {
        public ViewField()
        {
        }

        /// <summary>
        /// ��¼�û�����
        /// </summary>
        [FieldMapAttribute("USERCODE", typeof(string), 40, true)]
        public string UserCode;

        /// <summary>
        /// ˳���
        /// </summary>
        [FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
        public decimal Sequence;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("TABLENAME", typeof(string), 40, true)]
        public string TableName;

        /// <summary>
        /// �ֶ�����
        /// </summary>
        [FieldMapAttribute("FIELDNAME", typeof(string), 40, false)]
        public string FieldName;

        /// <summary>
        /// �ֶ�����
        /// </summary>
        [FieldMapAttribute("DESCRIPTION", typeof(string), 200, false)]
        public string Description;

        /// <summary>
        /// �Ƿ�Ĭ��
        /// </summary>
        [FieldMapAttribute("ISDEFAULT", typeof(string), 40, false)]
        public string IsDefault;

    }
    #endregion

    #region Storageinfo-- �����Ϣ  add by jinger 2016-01-27
    ///// <summary>
    ///// TBLSTORAGEINFO-- �����Ϣ 
    ///// </summary>
    //[Serializable, TableMap("TBLSTORAGEINFO", "STORAGECODE,MCODE")]
    //public class StorageInfo : BenQGuru.eMES.Common.Domain.DomainObject
    //{
    //    public StorageInfo()
    //    {
    //    }

    //    ///<summary>
    //    ///ά��ʱ��
    //    ///</summary>
    //    [FieldMapAttribute("MTIME", typeof(int), 22, false)]
    //    public int MaintainTime;

    //    ///<summary>
    //    ///ά������
    //    ///</summary>
    //    [FieldMapAttribute("MDATE", typeof(int), 22, false)]
    //    public int MaintainDate;

    //    ///<summary>
    //    ///ά����
    //    ///</summary>
    //    [FieldMapAttribute("MUSER", typeof(string), 40, false)]
    //    public string MaintainUser;

    //    ///<summary>
    //    ///����
    //    ///</summary>
    //    [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
    //    public string FacCode;

    //    ///<summary>
    //    ///�������
    //    ///</summary>
    //    [FieldMapAttribute("STORAGEQTY", typeof(int), 22, false)]
    //    public int StorageQty;

    //    ///<summary>
    //    ///��λ
    //    ///</summary>
    //    [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
    //    public string StorageCode;

    //    ///<summary>
    //    ///SAP���Ϻ�
    //    ///</summary>
    //    [FieldMapAttribute("MCODE", typeof(string), 40, false)]
    //    public string MCode;

    //}
    #endregion

    #region ASN-- ASN����  add by jinger 2016-01-28
    /// <summary>
    /// TBLASN-- ASN���� 
    /// </summary>
    [Serializable, TableMap("TBLASN", "STNO")]
    public class ASN : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public ASN()
        {
        }

        ///<summary>
        ///�����ò���������
        ///</summary>
        [FieldMapAttribute("INITGIVEINQTY", typeof(int), 22, true)]
        public int InitGiveInQty;

        ///<summary>
        ///�����������
        ///</summary>
        [FieldMapAttribute("INITRECEIVEQTY", typeof(int), 22, true)]
        public int InitReceiveQty;

        ///<summary>
        ///�������ԭ��
        ///</summary>
        [FieldMapAttribute("INITRECEIVEDESC", typeof(string), 200, true)]
        public string InitReceiveDesc;

        ///<summary>
        ///�����������
        ///</summary>
        [FieldMapAttribute("INITREJECTQTY", typeof(int), 22, true)]
        public int InitRejectQty;

        ///<summary>
        ///��ע
        ///</summary>
        [FieldMapAttribute("REMARK1", typeof(string), 200, true)]
        public string Remark1;

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά������ 
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///����ʱ��
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///������
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///��ⷵ��������
        ///</summary>
        [FieldMapAttribute("REWORKAPPLYUSER", typeof(string), 40, true)]
        public string ReWorkApplyUser;

        ///<summary>
        ///��������
        ///</summary>
        //[FieldMapAttribute("PROVIDE_DATE", typeof(int), 22, true)]
        //public int ProvideDate;

        [FieldMapAttribute("PROVIDE_DATE", typeof(string), 40, true)]
        public string ProvideDate;

        ///<summary>
        ///װ�䵥��
        ///</summary>
        [FieldMapAttribute("PACKINGLISTNO", typeof(string), 40, true)]
        public string PackingListNo;

        ///<summary>
        ///Ԥ�Ƶ�������
        ///</summary>
        [FieldMapAttribute("PREDICT_DATE", typeof(int), 22, true)]
        public int PreictDate;

        ///<summary>
        ///����������
        ///</summary>
        [FieldMapAttribute("PICKNO", typeof(string), 40, true)]
        public string PickNo;

        ///<summary>
        ///���������벻��Ʒ��(Y:�벻��Ʒ��)
        ///</summary>
        [FieldMapAttribute("REJECTS_FLAG", typeof(string), 1, true)]
        public string RejectsFlag;

        ///<summary>
        ///��Ӧ��ֱ��(Y:ֱ��)
        ///</summary>
        [FieldMapAttribute("DIRECT_FLAG", typeof(string), 1, true)]
        public string DirectFlag;

        ///<summary>
        ///��������(Y:����)
        ///</summary>
        [FieldMapAttribute("EXIGENCY_FLAG", typeof(string), 1, true)]
        public string ExigencyFlag;

        ///<summary>
        ///���
        ///</summary>
        [FieldMapAttribute("VOLUME", typeof(string), 40, true)]
        public string Volume;

        ///<summary>
        ///ë��
        ///</summary>
        [FieldMapAttribute("GROSS_WEIGHT", typeof(decimal), 22, true)]
        public decimal GrossWeight;

        ///<summary>
        ///�����λ
        ///</summary>
        [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 40, true)]
        public string FromStorageCode;

        ///<summary>
        ///���⹤��
        ///</summary>
        [FieldMapAttribute("FROMFACCODE", typeof(string), 40, true)]
        public string FromFacCode;

        ///<summary>
        ///����λ
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        ///<summary>
        ///��⹤��
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FacCode;

        ///<summary>
        ///OA���뵥��
        ///</summary>
        [FieldMapAttribute("OANO", typeof(string), 40, true)]
        public string OANo;

        ///<summary>
        ///ASN״̬:��ʼ�����·������գ�������ɣ����ѿ�
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///��Ӧ�̴���
        ///</summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, true)]
        public string VendorCode;

        ///<summary>
        ///SAP���ݺ�
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, true)]
        public string InvNo;

        ///<summary>
        ///ASN��������(��ӦSAP��������)
        ///</summary>
        [FieldMapAttribute("STTYPE", typeof(string), 40, false)]
        public string StType;

        ///<summary>
        ///ASN����
        ///</summary>
        [FieldMapAttribute("STNO", typeof(string), 40, false)]
        public string StNo;

    }
    #endregion

    #region ASNDetail-- ASN��ϸ��  add by jinger 2016-01-28
    /// <summary>
    /// TBLASNDETAIL-- ASN��ϸ�� 
    /// </summary>
    [Serializable, TableMap("TBLASNDETAIL", "STLINE,STNO")]
    public class ASNDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public ASNDetail()
        {
        }

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά������ 
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///����ʱ��
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///������
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///��ע
        ///</summary>
        [FieldMapAttribute("REMARK1", typeof(string), 200, true)]
        public string Remark1;

        ///<summary>
        ///�������˵��(վ����������)
        ///</summary>
        [FieldMapAttribute("INITRECEIVEDESC", typeof(string), 200, true)]
        public string InitReceiveDesc;

        ///<summary>
        ///�������״̬(Receive:���գ�Reject:���գ�Givein:�ò�����)
        ///</summary>
        [FieldMapAttribute("INITRECEIVESTATUS", typeof(string), 40, true)]
        public string InitReceiveStatus;

        ///<summary>
        ///��һ�����ʱ��
        ///</summary>
        [FieldMapAttribute("STORAGEAGEDATE", typeof(int), 22, true)]
        public int StorageAgeDate;

        ///<summary>
        ///�������κ�
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string LotNo;

        ///<summary>
        ///��Ӧ�����κ�
        ///</summary>
        [FieldMapAttribute("SUPPLIER_LOTNO", typeof(string), 40, true)]
        public string SupplierLotNo;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("PRODUCTION_DATE", typeof(int), 22, true)]
        public int ProductionDate;

        ///<summary>
        ///��λ
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///����ͨ������
        ///</summary>
        [FieldMapAttribute("QCPASSQTY", typeof(int), 22, true)]
        public int QcPassQty;

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("ACTQTY", typeof(int), 22, true)]
        public int ActQty;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("RECEIVEQTY", typeof(int), 22, true)]
        public int ReceiveQty;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("QTY", typeof(int), 22, false)]
        public int Qty;

        ///<summary>
        ///��Ӧ�����ϱ�������
        ///</summary>
        [FieldMapAttribute("VENDORMCODEDESC", typeof(string), 200, true)]
        public string VendorMCodeDesc;

        ///<summary>
        ///��Ӧ�����ϱ���
        ///</summary>
        [FieldMapAttribute("VENDORMCODE", typeof(string), 35, true)]
        public string VendorMCode;

        ///<summary>
        ///�ջ������Ϻ�
        ///</summary>
        [FieldMapAttribute("RECEIVEMCODE", typeof(string), 40, true)]
        public string ReceiveMCode;

        ///<summary>
        ///��������
        ///</summary>
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string MDesc;

        ///<summary>
        ///�������Ϻ�
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCode;

        ///<summary>
        ///SAP���Ϻ�
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///��Ϊ���Ϻ�
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 40, true)]
        public string CustMCode;

        ///<summary>
        ///���
        ///</summary>
        [FieldMapAttribute("CARTONSEQ", typeof(string), 40, true)]
        public string CartonSeq;

        ///<summary>
        ///�����
        ///</summary>
        [FieldMapAttribute("CARTONBIGSEQ", typeof(string), 40, true)]
        public string CartonBigSeq;

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, true)]
        public string CartonNo;

        ///<summary>
        ///ASN��״̬:Release:��ʼ����WaitReceive:���ջ���ReceiveClose:������ɣ�IQCClose:IQC��ɣ�OnLocation:�ϼܣ�Close:��⣻Cancel:ȡ��
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///ASN������Ŀ
        ///</summary>
        [FieldMapAttribute("STLINE", typeof(string), 40, false)]
        public string StLine;

        ///<summary>
        ///ASN����
        ///</summary>
        [FieldMapAttribute("STNO", typeof(string), 40, false)]
        public string StNo;
        ///<summary>
        ///
        ///</summary>
        [FieldMapAttribute("Remark2", typeof(string), 200, false)]
        public string Remark2;

        [FieldMapAttribute("INITGIVEINDESC", typeof(string), 40, false)]
        public string InitGIVEINDESC;

    }
    #endregion



    #region InvDoc-- �����ļ���  add by jinger 2016-01-31
    /// <summary>
    /// TBLINVDOC-- �����ļ��� 
    /// </summary>
    [Serializable, TableMap("TBLINVDOC", "DOCSERIAL")]
    public class InvDoc : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public InvDoc()
        {
        }

        ///<summary>
        ///ά��ʱ��
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MaintainTime;

        ///<summary>
        ///ά������
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        ///<summary>
        ///ά����
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///�������ļ�����
        ///</summary>
        [FieldMapAttribute("SERVERFILENAME", typeof(string), 200, false)]
        public string ServerFileName;

        ///<summary>
        ///�ϴ�����
        ///</summary>
        [FieldMapAttribute("UPFILEDATE", typeof(int), 22, false)]
        public int UpfileDate;

        ///<summary>
        ///�ϴ���
        ///</summary>
        [FieldMapAttribute("UPUSER", typeof(string), 40, false)]
        public string UpUser;

        ///<summary>
        ///��ע
        ///</summary>
        [FieldMapAttribute("REMARK1", typeof(string), 2000, true)]
        public string Remark1;

        ///<summary>
        ///�ļ���С
        ///</summary>
        [FieldMapAttribute("DOCSIZE", typeof(int), 22, true)]
        public int DocSize;

        ///<summary>
        ///�ļ�����(�ļ���չ��)
        ///</summary>
        [FieldMapAttribute("DOCTYPE", typeof(string), 20, true)]
        public string DocType;

        ///<summary>
        ///�ļ�����
        ///</summary>
        [FieldMapAttribute("DOCNAME", typeof(string), 200, false)]
        public string DocName;

        ///<summary>
        ///�����ļ�����:DirectSign:��Ӧ��ֱ��ǩ���ļ���InitReject:��������ļ���InitGivein:�����ò������ļ���
        ///</summary>
        [FieldMapAttribute("INVDOCTYPE", typeof(string), 40, false)]
        public string InvDocType;

        ///<summary>
        ///���ݺ�
        ///</summary>
        [FieldMapAttribute("INVDOCNO", typeof(string), 40, false)]
        public string InvDocNo;

        ///<summary>
        ///�ļ����
        ///</summary>
        [FieldMapAttribute("DOCSERIAL", typeof(int), 22, false)]
        public int DocSerial;

        ///<summary>
        ///DIRNAME
        ///</summary>
        [FieldMapAttribute("DIRNAME", typeof(string), 40, true)]
        public string Dirname;
        [FieldMapAttribute("PICKNO", typeof(string), 40, true)]
        public string PickNo;
    }
    #endregion

    #region Stack

    [Serializable, TableMap("TBLSTACK", "STACKCODE")]
    public class SStack : DomainObject
    {
        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string StackCode;

        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        [FieldMapAttribute("STACKDESC", typeof(string), 100, false)]
        public string StackDesc;

        [FieldMapAttribute("CAPACITY", typeof(Int32), 10, false)]
        public Int32 Capacity;

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
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string MaintainUser;

        [FieldMapAttribute("ISONEITEM", typeof(string), 40, true)]
        public string IsOneItem;

    }

    [Serializable]
    public class SStackWithStorageName : SStack
    {
        [FieldMapAttribute("STORAGENAME", typeof(string), 100, false)]
        public string StorageName;
    }

    [Serializable, TableMap("TBLSTACK", "STACKCODE")]
    public class StackNew : DomainObject
    {
        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string StackCode;

        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        [FieldMapAttribute("STACKDESC", typeof(string), 100, false)]
        public string StackDesc;

        [FieldMapAttribute("CAPACITY", typeof(Int32), 10, false)]
        public Int32 Capacity;

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
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }

    #endregion

    #region StackToRcard
    [Serializable, TableMap("TBLSTACK2RCARD", "SERIALNO,CARTONCODE")]
    public class StackToRcard : DomainObject
    {
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string StackCode;

        ///<summary>
        ///OQCLOT
        ///</summary>
        [FieldMapAttribute("OQCLOT", typeof(string), 40, true)]
        public string Oqclot;

        ///<summary>
        ///CARTONCODE
        ///</summary>
        [FieldMapAttribute("CARTONCODE", typeof(string), 100, false)]
        public string Cartoncode;

        [FieldMapAttribute("SERIALNO", typeof(string), 40, false)]
        public string SerialNo;

        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        [FieldMapAttribute("BUSINESSREASON", typeof(string), 40, false)]
        public string BusinessReason;

        [FieldMapAttribute("COMPANY", typeof(string), 100, false)]
        public string Company;

        //[FieldMapAttribute("ITEMGRADE", typeof(string), 40, false)]
        //public string ItemGrade;

        [FieldMapAttribute("INDATE", typeof(int), 8, false)]
        public int InDate;

        [FieldMapAttribute("INTIME", typeof(int), 6, false)]
        public int InTime;

        [FieldMapAttribute("INUSER", typeof(string), 40, false)]
        public string InUser;

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
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        [FieldMapAttribute("TRANSINSERIAL", typeof(int), 40, false)]
        public int TransInSerial;

    }
    #endregion

    #region RcardToStackPallet

    [Serializable]
    public class RcardToStackPallet : StackToRcard
    {
        [FieldMapAttribute("PALLETCODE", typeof(string), 40, false)]
        public string PalletCode;

        [FieldMapAttribute("ITEMDESC", typeof(string), 100, false)]
        public string ItemDescription;

        //[FieldMapAttribute("CARTONCODE", typeof(string), 40, false)]
        //public string CartonCode;


        [FieldMapAttribute("MOCODE", typeof(string), 40, false)]
        public string MOCode;
    }

    #endregion

    #region InvBusiness
    [Serializable, TableMap("TBLINVBUSINESS", "BUSINESSCODE,ORGID")]
    public class InvBusiness : DomainObject
    {
        [FieldMapAttribute("BUSINESSCODE", typeof(string), 40, false)]
        public string BusinessCode;

        [FieldMapAttribute("BUSINESSDESC", typeof(string), 100, false)]
        public string BusinessDescription;

        [FieldMapAttribute("BUSINESSTYPE", typeof(string), 40, false)]
        public string BusinessType;

        [FieldMapAttribute("BUSINESSREASON", typeof(string), 40, false)]
        public string BusinessReason;

        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

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
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string MaintainUser;

        /// <summary>
        /// �Ƿ��Ƚ��ȳ�[ISFIFO]
        /// </summary>
        [FieldMapAttribute("ISFIFO", typeof(string), 40, false)]
        public string ISFIFO;

    }
    #endregion

    #region InvInTransaction
    [Serializable, TableMap("TBLINVINTRANSACTION", "serial")]
    public class InvInTransaction : DomainObject
    {
        [FieldMapAttribute("TRANSCODE", typeof(string), 40, true)]
        public string TransCode;

        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string Rcard;

        [FieldMapAttribute("CARTONCODE", typeof(string), 100, true)]
        public string CartonCode;

        [FieldMapAttribute("PALLETCODE", typeof(string), 40, true)]
        public string PalletCode;

        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        [FieldMapAttribute("BUSINESSCODE", typeof(string), 40, false)]
        public string BusinessCode;

        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string StackCode;

        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        //[FieldMapAttribute("ITEMGRADE", typeof(string), 40, false)]
        //public string ItemGrade;

        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string SSCode;

        [FieldMapAttribute("COMPANY", typeof(string), 100, false)]
        public string Company;

        [FieldMapAttribute("BUSINESSREASON", typeof(string), 40, false)]
        public string BusinessReason;

        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

        [FieldMapAttribute("SERIAL", typeof(int), 10, false)]
        public int Serial;

        [FieldMapAttribute("DELIVERUSER", typeof(string), 40, true)]
        public string DeliverUser;

        [FieldMapAttribute("MEMO", typeof(string), 100, true)]
        public string Memo;

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
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        [FieldMapAttribute("relateddocument", typeof(string), 100, true)]
        public string RelatedDocument;

    }

    #endregion

    #region InvOutTransaction
    [Serializable, TableMap("TBLINVOUTTRANSACTION", "serial")]
    public class InvOutTransaction : DomainObject
    {
        [FieldMapAttribute("TRANSCODE", typeof(string), 40, true)]
        public string TransCode;

        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string Rcard;

        [FieldMapAttribute("CARTONCODE", typeof(string), 100, true)]
        public string CartonCode;

        [FieldMapAttribute("PALLETCODE", typeof(string), 40, true)]
        public string PalletCode;

        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        [FieldMapAttribute("BUSINESSCODE", typeof(string), 40, false)]
        public string BusinessCode;

        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string StackCode;

        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        //[FieldMapAttribute("ITEMGRADE", typeof(string), 40, false)]
        //public string ItemGrade;


        [FieldMapAttribute("COMPANY", typeof(string), 100, false)]
        public string Company;

        [FieldMapAttribute("BUSINESSREASON", typeof(string), 40, false)]
        public string BusinessReason;

        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

        [FieldMapAttribute("SERIAL", typeof(int), 10, false)]
        public int Serial;

        [FieldMapAttribute("MEMO", typeof(string), 100, true)]
        public string Memo;

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
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        [FieldMapAttribute("DNLINE", typeof(string), 6, false)]
        public string DNLine;

        [FieldMapAttribute("TRANSINSERIAL", typeof(int), 40, false)]
        public int TransInSerial;
    }

    #endregion

    #region InvFormula

    [Serializable, TableMap("TBLINVFORMULA", "FORMULACODE")]
    public class InvFormula : DomainObject
    {

        [FieldMapAttribute("FORMULACODE", typeof(string), 40, false)]
        public string FormulaCode;

        [FieldMapAttribute("FORMULADESC", typeof(string), 100, true)]
        public string FormulaDesc;


        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;


        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string MaintainUser;

    }

    #endregion

    #region InvPeriod

    [Serializable, TableMap("TBLINVPERIOD", "INVPERIODCODE,PEIODGROUP")]
    public class InvPeriod : DomainObject
    {

        [FieldMapAttribute("INVPERIODCODE", typeof(string), 40, false)]
        public string InvPeriodCode;

        [FieldMapAttribute("PEIODGROUP", typeof(string), 40, false)]
        public string PeiodGroup;

        [FieldMapAttribute("DATEFROM", typeof(int), 8, true)]
        public int DateFrom;


        [FieldMapAttribute("DATETO", typeof(int), 8, true)]
        public int DateTo;

        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;


        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string MaintainUser;
    }

    #endregion

    #region InventoryPeriodStandard

    /// <summary>
    ///	InventoryPeriodStandard
    /// </summary>
    [Serializable, TableMap("TBLINVPERIODSTD", "INVTYPE, PERIODGROUP, INVPERIODCODE")]
    public class InventoryPeriodStandard : DomainObject
    {
        public InventoryPeriodStandard()
        {
        }

        ///<summary>
        ///InventoryType
        ///</summary>	
        [FieldMapAttribute("INVTYPE", typeof(string), 40, false)]
        public string InventoryType;

        ///<summary>
        ///PeriodGROUP
        ///</summary>	
        [FieldMapAttribute("PERIODGROUP", typeof(string), 40, false)]
        public string PeriodGroup;

        ///<summary>
        ///InventoryPeriodCode
        ///</summary>	
        [FieldMapAttribute("INVPERIODCODE", typeof(string), 40, false)]
        public string InventoryPeriodCode;

        ///<summary>
        ///PercentageStandard
        ///</summary>	
        [FieldMapAttribute("PERCENTAGESTD", typeof(decimal), 8, false)]
        public decimal PercentageStandard;

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


    }

    #endregion

    #region ProductInvPeriod

    /// <summary>
    ///	ProductInvPeriod
    /// </summary>
    [Serializable, TableMap("", "")]
    public class ProductInvPeriod : DomainObject
    {
        public ProductInvPeriod()
        {
        }

        [FieldMapAttribute("INVTYPE", typeof(string), 40, true)]
        public string InventoryType;

        [FieldMapAttribute("INVPERIODCODE", typeof(string), 40, true)]
        public string InventoryPeriodCode;

        [FieldMapAttribute("DATEFROM", typeof(int), 22, true)]
        public int DateFrom;

        [FieldMapAttribute("DATETO", typeof(int), 22, true)]
        public int DateTo;

        [FieldMapAttribute("PERCENTAGESTD", typeof(decimal), 8, true)]
        public decimal PercentageStandard;

        [FieldMapAttribute("PRODUCTCOUNT", typeof(int), 22, true)]
        public int ProductCount;

        public bool IsForTitle = false;
        public List<ProductInvPeriod> DataToGrid = new List<ProductInvPeriod>();
    }

    #endregion

    #region StorageAttribute

    [Serializable, TableMap(" (SELECT paramcode, paramdesc FROM tblsysparam WHERE paramgroupcode = 'STORAGEATTRIBUTE' ) storageattributeparam ", "")]
    public class StorageAttributeParam : DomainObject
    {
        public StorageAttributeParam()
        {
        }

        [FieldMapAttribute("PARAMCODE", typeof(string), 40, true)]
        public string ParamCode;

        [FieldMapAttribute("PARAMDESC", typeof(string), 40, true)]
        public string ParamDesc;
    }

    #endregion

    #region InvBusiness2Formula

    [Serializable, TableMap("TBLINVBUSINESS2FORMULA", "BUSINESSCODE,FORMULACODE,ORGID")]
    public class InvBusiness2Formula : DomainObject
    {

        [FieldMapAttribute("BUSINESSCODE", typeof(string), 40, false)]
        public string BusinessCode;


        [FieldMapAttribute("FORMULACODE", typeof(string), 40, false)]
        public string FormulaCode;


        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;


        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

    }

    #endregion

    #region Pause

    [Serializable, TableMap("TBLPAUSE", "PAUSECODE")]
    public class Pause : DomainObject
    {

        [FieldMapAttribute("PAUSECODE", typeof(string), 40, false)]
        public string PauseCode;

        [FieldMapAttribute("PAUSEREASON", typeof(string), 200, true)]
        public string PauseReason;

        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        [FieldMapAttribute("CANCELREASON", typeof(string), 200, true)]
        public string CancelReason;

        [FieldMapAttribute("PDATE", typeof(int), 8, false)]
        public int PDate;


        [FieldMapAttribute("PTIME", typeof(int), 6, false)]
        public int PTime;

        [FieldMapAttribute("PUSER", typeof(string), 40, false)]
        public string PUser;

        [FieldMapAttribute("CDATE", typeof(int), 8, true)]
        public int CancelDate;


        [FieldMapAttribute("CTIME", typeof(int), 6, true)]
        public int CancelTime;

        [FieldMapAttribute("CUSER", typeof(string), 40, true)]
        public string CancelUser;

        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;


        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }

    #endregion

    #region DNTempOut

    [Serializable, TableMap("TBLDNTEMPOUT", "STACKCODE,ITEMCODE,DNNO,DNLINE")]
    public class DNTempOut : DomainObject
    {

        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string StackCode;

        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        [FieldMapAttribute("DNNO", typeof(string), 40, false)]
        public string DNNO;

        [FieldMapAttribute("DNLINE", typeof(string), 6, false)]
        public string DNLine;

        [FieldMapAttribute("TEMPQTY", typeof(int), 10, false)]
        public int TempQty;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;
    }

    #endregion

    #region Pause2Rcard

    [Serializable, TableMap("TBLPAUSE2RCARD", "SERIALNO,PAUSECODE")]
    public class Pause2Rcard : DomainObject
    {
        [FieldMapAttribute("PAUSECODE", typeof(string), 40, false)]
        public string PauseCode;

        [FieldMapAttribute("SERIALNO", typeof(string), 40, false)]
        public string SerialNo;

        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        [FieldMapAttribute("CANCELSEQ", typeof(string), 40, true)]
        public string CancelSeq;

        [FieldMapAttribute("BOM", typeof(string), 40, true)]
        public string BOM;

        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;


        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        [FieldMapAttribute("CANCELREASON", typeof(string), 200, true)]
        public string CancelReason;

        [FieldMapAttribute("PDATE", typeof(int), 8, false)]
        public int PDate;

        [FieldMapAttribute("PTIME", typeof(int), 6, false)]
        public int PTime;

        [FieldMapAttribute("PUSER", typeof(string), 40, false)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string PUser;

        [FieldMapAttribute("CDATE", typeof(int), 8, true)]
        public int CancelDate;


        [FieldMapAttribute("CTIME", typeof(int), 6, true)]
        public int CancelTime;

        [FieldMapAttribute("CUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string CancelUser;

        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;


        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }

    #endregion

    #region PauseQuery

    [Serializable]
    public class PauseQuery : Pause2Rcard
    {
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLSTORAGE", "STORAGECODE", "STORAGENAME")]
        public string StorageCode;

        [FieldMapAttribute("STACKCODE", typeof(string), 100, true)]
        public string StackCode;

        [FieldMapAttribute("PALLETCODE", typeof(string), 40, true)]
        public string PalletCode;

        [FieldMapAttribute("RCARDCOUNT", typeof(decimal), 8, true)]
        public decimal RcardCount;

        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCode;

        [FieldMapAttribute("MDESC", typeof(string), 40, true)]
        public string MDesc;

        [FieldMapAttribute("CARTONCODE", typeof(string), 40, true)]
        public string CartonCode;

        [FieldMapAttribute("PAUSEQTY", typeof(string), 40, true)]
        public string PauseQty;

        [FieldMapAttribute("CANCELQTY", typeof(string), 40, true)]
        public string CancelQty;

        [FieldMapAttribute("MMODELCODE", typeof(string), 40, true)]
        public string MModelCode;

        [FieldMapAttribute("PAUSEREASON", typeof(string), 200, true)]
        public string PauseReason;
    }

    #endregion

    #region PauseSetting

    [Serializable]
    public class PauseSetting : DomainObject
    {
        [FieldMapAttribute("itemcode", typeof(string), 40, false)]
        public string ItemCode;

        [FieldMapAttribute("mdesc", typeof(string), 100, true)]
        public string ItemDescription;

        [FieldMapAttribute("mobom", typeof(string), 40, true)]
        public string BOM;

        [FieldMapAttribute("BIGSSCODE", typeof(string), 40, true)]
        public string BigSSCode;

        [FieldMapAttribute("qty", typeof(decimal), 0, true)]
        public decimal qty;

        [FieldMapAttribute("palletcode", typeof(string), 40, true)]
        public string PalletCode;

        [FieldMapAttribute("rcard", typeof(string), 40, true)]
        public string Rcard;

        [FieldMapAttribute("mocode", typeof(string), 40, true)]
        public string MOCode;


        [FieldMapAttribute("finisheddate", typeof(Int32), 8, true)]
        public Int32 FinishedDate;

        [FieldMapAttribute("indate", typeof(Int32), 8, true)]
        public Int32 InInvDate;
    }

    #endregion

    #region CancelPauseQuery

    [Serializable]
    public class CancelPauseQuery : DomainObject
    {
        [FieldMapAttribute("storagecode", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLSTORAGE", "STORAGECODE", "STORAGENAME")]
        public string StorageCode;

        [FieldMapAttribute("stackcode", typeof(string), 40, true)]
        public string StackCode;

        [FieldMapAttribute("itemcode", typeof(string), 40, false)]
        public string ItemCode;

        [FieldMapAttribute("mdesc", typeof(string), 100, true)]
        public string ItemDescription;

        [FieldMapAttribute("status", typeof(string), 40, false)]
        public string Status;

        [FieldMapAttribute("pausecode", typeof(string), 40, false)]
        public string PauseCode;

        [FieldMapAttribute("pauseqty", typeof(decimal), 0, false)]
        public decimal PauseQty;

        [FieldMapAttribute("cancelqty", typeof(decimal), 0, false)]
        public decimal CancelQty;
    }

    #endregion

    #region StackMessage

    [Serializable]
    public class StackMessage : DomainObject
    {
        [FieldMapAttribute("stackcode", typeof(string), 40, true)]
        public string StackCode;

        [FieldMapAttribute("STACKQTYMESSAGE", typeof(string), 40, true)]
        public string StackQtyMessage;
    }

    #endregion

    #region DNTempOutMessage

    [Serializable]
    public class DNTempOutMessage : StackToRcard
    {

        [FieldMapAttribute("STORAGENAME", typeof(string), 100, true)]
        public string StorageName;

        [FieldMapAttribute("MNAME", typeof(string), 100, true)]
        public string ItemDescription;

        [FieldMapAttribute("MMODELCODE", typeof(string), 40, true)]
        public string MModelCode;

        [FieldMapAttribute("INVQTY", typeof(int), 10, true)]
        public int INVQTY;

        [FieldMapAttribute("COMQTY", typeof(int), 10, true)]
        public int COMQTY;

        [FieldMapAttribute("SAPQTY", typeof(int), 10, true)]
        public int SAPQTY;

        [FieldMapAttribute("TEMPQTY", typeof(int), 10, true)]
        public int TEMPQTY;
    }

    #endregion

    #region MaterialBusiness(���ϳ��������)
    [Serializable, TableMap("TBLMATERIALBUSINESS", "BUSINESSCODE")]
    public class MaterialBusiness : DomainObject
    {
        /// <summary>
        /// ҵ�����ʹ���
        /// </summary>
        [FieldMapAttribute("BusinessCode", typeof(string), 40, false)]
        public string BusinessCode;

        /// <summary>
        /// ҵ����������
        /// </summary>
        [FieldMapAttribute("BusinessDesc", typeof(string), 100, false)]
        public string BusinessDesc;

        /// <summary>
        /// ҵ������
        /// </summary>
        [FieldMapAttribute("BusinessType", typeof(string), 40, false)]
        public string BusinessType;

        /// <summary>
        /// SAP ҵ�����
        /// </summary>
        [FieldMapAttribute("SAPCODE", typeof(string), 40, false)]
        public string SAPCODE;

        /// <summary>
        /// ��֯ID
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

        /// <summary>
        /// FIFO���
        /// </summary>
        [FieldMapAttribute("ISFIFO", typeof(string), 40, false)]
        public string ISFIFO;

        /// <summary>
        /// ά������
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        /// <summary>
        /// ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// ά����
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;
    }
    #endregion

    #region InvIntransSum
    [Serializable, TableMap("TBLINVINTRANSSUM", "")]
    public class InvIntransSum : DomainObject
    {
        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MaintainDate;

        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string StackCode;

        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode;

        //[FieldMapAttribute("ITEMGRADE", typeof(string), 40, false)]
        //public string ItemGrade;

        [FieldMapAttribute("INQTY", typeof(int), 22, false)]
        public int InQty;
    }

    #endregion

    #region MaterialFull �������ײ�ѯ����
    public class MaterialFull : DomainObject
    {
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MoCode;
        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCode;
        [FieldMapAttribute("PLANTYPE", typeof(string), 8, true)]
        public string PlanType;
        [FieldMapAttribute("LOSTQTY", typeof(decimal), 22, true)]
        public decimal LostQty;
        [FieldMapAttribute("SUMQTY", typeof(decimal), 22, true)]
        public decimal SumQty;
        [FieldMapAttribute("QTY", typeof(decimal), 22, true)]
        public decimal Qty;
        [FieldMapAttribute("SHORTQTY", typeof(decimal), 22, true)]
        public decimal ShortQty;
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string Mdesc;
        [FieldMapAttribute("MODESC", typeof(string), 200, true)]
        public string Modesc;

        ///<summary>
        ///QUERYSEQ
        ///</summary>
        [FieldMapAttribute("QUERYSEQ", typeof(int), 22, false)]
        public int Queryseq;

        ///<summary>
        ///PLANDATE
        ///</summary>
        [FieldMapAttribute("PLANDATE", typeof(int), 22, false)]
        public int Plandate;

        ///<summary>
        ///ITEMCODE
        ///</summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string Itemcode;

        ///<summary>
        ///ORGID
        ///</summary>
        [FieldMapAttribute("ORGID", typeof(int), 22, false)]
        public int Orgid;

        ///<summary>
        ///PLANQTY
        ///</summary>
        [FieldMapAttribute("PLANQTY", typeof(int), 22, false)]
        public int Planqty;

        ///<summary>
        ///MUSER
        ///</summary>
        ///
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

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
    }
    #endregion

    #region TBLMSDLevel
    [Serializable, TableMap("TBLMSDLevel", "MHumidityLevel")]
    public class MSDLevel : DomainObject
    {
        /// <summary>
        /// ʪ���ȼ�
        /// </summary>
        [FieldMapAttribute("MHUMIDITYLEVEL", typeof(string), 40, false)]
        public string MHumidityLevel;

        /// <summary>
        /// ʪ���ȼ�����
        /// </summary>
        [FieldMapAttribute("MHUMIDITYLEVELDESC", typeof(string), 100, true)]
        public string MHumidityLevelDesc;

        /// <summary>
        ///��Ч��������
        /// </summary>
        [FieldMapAttribute("FLOORLIFE", typeof(int), 10, false)]
        public int FloorLife;

        /// <summary>
        ///��������С����ʱ��
        /// </summary>
        [FieldMapAttribute("DRYINGTIME", typeof(int), 10, false)]
        public int DryingTime;

        /// <summary>
        ///��¶ʱ��
        /// </summary>
        [FieldMapAttribute("INDRYINGTIME", typeof(int), 10, false)]
        public int INDryingTime;

        /// <summary>
        /// ά������
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        /// <summary>
        /// ά����
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;
    }
    #endregion

    #region Customer
    /// <summary>
    /// Customer
    /// </summary>
    [Serializable, TableMap("TBLCUSTOMER", "CUSTOMERCODE")]
    public class Customer : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Customer()
        {
        }

        ///<summary>
        ///CUSTOMERCODE
        ///</summary>
        [FieldMapAttribute("CUSTOMERCODE", typeof(string), 40, false)]
        public string CustomerCode;

        ///<summary>
        ///CUSTOMERNAME
        ///</summary>
        [FieldMapAttribute("CUSTOMERNAME", typeof(string), 100, false)]
        public string CustomerName;

        ///<summary>
        ///EATTRIBUTE3
        ///</summary>
        [FieldMapAttribute("ADDRESS", typeof(string), 100, true)]
        public string ADDRESS;

        ///<summary>
        ///EATTRIBUTE3
        ///</summary>
        [FieldMapAttribute("TEL", typeof(string), 16, true)]
        public string TEL;

        ///<summary>
        ///EATTRIBUTE3
        ///</summary>
        [FieldMapAttribute("FLAG", typeof(string), 2, true)]
        public string FLAG;


        ///<summary>
        ///EATTRIBUTE1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;
        ///<summary>
        ///EATTRIBUTE2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string Eattribute2;
        ///<summary>
        ///EATTRIBUTE3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string Eattribute3;

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
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string Muser;

    }
    #endregion

    #region TBLMaterialMSL
    [Serializable, TableMap("TBLMaterialMSL", "MCODE,ORGID")]
    public class MaterialMSL : DomainObject
    {
        /// <summary>
        /// ���ϴ���
        /// </summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        /// <summary>
        /// ��֯ID
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

        /// <summary>
        /// ʪ���ȼ�
        /// </summary>
        [FieldMapAttribute("MHUMIDITYLEVEL", typeof(string), 40, false)]
        public string MHumidityLevel;

        ///// <summary>
        /////��������С����ʱ��
        ///// </summary>
        //[FieldMapAttribute("DRYINGTIME", typeof(int), 10, false)]
        //public int DryingTime;

        ///// <summary>
        /////��Ⱪ¶�ڿ�����δʹ�������������ʱ��(��¶ʱ��)
        ///// </summary>
        //[FieldMapAttribute("INDRYINGTIME", typeof(int), 10, false)]
        //public int InDryingTime;

        /// <summary>
        /// ά������
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        /// <summary>
        /// ά����
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;
    }
    #endregion

    #region STORAGEINFO jingerע�� 20160127

    /// <summary>
    /// TBLSTORAGEINFO
    /// </summary>
    [Serializable, TableMap("TBLSTORAGEINFO", "STORAGEID,MCODE")]
    public class StorageInfo : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public StorageInfo()
        {
        }

        ///<summary>
        ///MCODE
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string Mcode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("StorageCode", typeof(string), 40, false)]
        public string StorageCode;

        /////<summary>
        /////STACKCODE
        /////</summary>
        //[FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        //public string Stackcode;

        ///<summary>
        ///STORAGEQTY
        ///</summary>
        [FieldMapAttribute("STORAGEQTY", typeof(decimal), 22, false)]
        public decimal Storageqty;

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
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string Muser;

        /////<summary>
        /////Organization ID
        /////</summary>	
        //[FieldMapAttribute("ORGID", typeof(int), 8, false)]
        //public int OrganizationID;

    }
    #endregion

    #region STORAGELOTINFO
    /// <summary>
    /// TBLSTORAGELOTINFO
    /// </summary>
    [Serializable, TableMap("TBLSTORAGELOTINFO", "LOTNO,STORAGEID,STACKCODE,MCODE")]
    public class StorageLotInfo : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public StorageLotInfo()
        {
        }

        ///<summary>
        ///LOTNO
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 100, false)]
        public string Lotno;

        ///<summary>
        ///STORAGEID
        ///</summary>
        [FieldMapAttribute("STORAGEID", typeof(string), 40, false)]
        public string Storageid;

        ///<summary>
        ///STACKCODE
        ///</summary>
        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string Stackcode;

        ///<summary>
        ///MCODE
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string Mcode;

        ///<summary>
        ///LOTQTY
        ///</summary>
        [FieldMapAttribute("LOTQTY", typeof(decimal), 28, false)]
        public decimal Lotqty;

        ///<summary>
        ///RECEIVEDATE
        ///</summary>
        [FieldMapAttribute("RECEIVEDATE", typeof(int), 22, false)]
        public int Receivedate;

        ///<summary>
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
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

    }
    #endregion

    #region ITEMTRANS
    /// <summary>
    /// TBLITEMTRANS
    /// </summary>
    [Serializable, TableMap("TBLITEMTRANS", "SERIAL")]
    public class ItemTrans : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public ItemTrans()
        {
        }

        ///<summary>
        ///SERIAL
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 38, false)]
        public int Serial;

        ///<summary>
        ///TRANSNO
        ///</summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 50, true)]
        public string Transno;

        ///<summary>
        ///TRANSLINE
        ///</summary>
        [FieldMapAttribute("TRANSLINE", typeof(int), 22, true)]
        public int Transline;

        ///<summary>
        ///ITEMCODE
        ///</summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string Itemcode;

        ///<summary>
        ///FRMSTORAGEID
        ///</summary>
        [FieldMapAttribute("FRMSTORAGEID", typeof(string), 40, true)]
        public string Frmstorageid;

        ///<summary>
        ///FRMSTACKCODE
        ///</summary>
        [FieldMapAttribute("FRMSTACKCODE", typeof(string), 40, true)]
        public string Frmstackcode;

        ///<summary>
        ///TOSTORAGEID
        ///</summary>
        [FieldMapAttribute("TOSTORAGEID", typeof(string), 40, true)]
        public string Tostorageid;

        ///<summary>
        ///TOSTACKCODE
        ///</summary>
        [FieldMapAttribute("TOSTACKCODE", typeof(string), 40, true)]
        public string Tostackcode;

        ///<summary>
        ///TRANSQTY
        ///</summary>
        [FieldMapAttribute("TRANSQTY", typeof(decimal), 22, false)]
        public decimal Transqty;

        ///<summary>
        ///MEMO
        ///</summary>
        [FieldMapAttribute("MEMO", typeof(string), 1000, true)]
        public string Memo;

        ///<summary>
        ///TRANSTYPE
        ///</summary>
        [FieldMapAttribute("TRANSTYPE", typeof(string), 40, false)]
        public string Transtype;

        ///<summary>
        ///BUSINESSCODE
        ///</summary>
        [FieldMapAttribute("BUSINESSCODE", typeof(string), 40, false)]
        public string Businesscode;

        ///<summary>
        ///ORGID
        ///</summary>
        [FieldMapAttribute("ORGID", typeof(int), 22, false)]
        public int Orgid;

        ///<summary>
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
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

    }
    #endregion

    #region ITEMTRANSLOT
    /// <summary>
    /// TBLITEMTRANSLOT
    /// </summary>
    [Serializable, TableMap("TBLITEMTRANSLOT", "LOTNO,TBLITEMTRANS_SERIAL")]
    public class ItemTransLot : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public ItemTransLot()
        {
        }

        ///<summary>
        ///TBLITEMTRANS_SERIAL
        ///</summary>
        [FieldMapAttribute("TBLITEMTRANS_SERIAL", typeof(int), 22, false)]
        public int Tblitemtrans_serial;

        ///<summary>
        ///LOTNO
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 100, false)]
        public string Lotno;

        ///<summary>
        ///ITEMCODE
        ///</summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string Itemcode;

        ///<summary>
        ///TRANSQTY
        ///</summary>
        [FieldMapAttribute("TRANSQTY", typeof(decimal), 28, false)]
        public decimal Transqty;

        ///<summary>
        ///MEMO
        ///</summary>
        [FieldMapAttribute("MEMO", typeof(string), 1000, true)]
        public string Memo;

        ///<summary>
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
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

    }
    #endregion

    #region ITEMTRANSLOTDETAIL
    /// <summary>
    /// TBLITEMTRANSLOTDETAIL
    /// </summary>
    [Serializable, TableMap("TBLITEMTRANSLOTDETAIL", "TBLITEMTRANS_SERIAL,LOTNO,SERIALNO")]
    public class ItemTransLotDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public ItemTransLotDetail()
        {
        }

        ///<summary>
        ///TBLITEMTRANS_SERIAL
        ///</summary>
        [FieldMapAttribute("TBLITEMTRANS_SERIAL", typeof(int), 22, false)]
        public int Tblitemtrans_serial;

        ///<summary>
        ///LOTNO
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 100, false)]
        public string Lotno;

        ///<summary>
        ///ITEMCODE
        ///</summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string Itemcode;

        ///<summary>
        ///SERIALNO
        ///</summary>
        [FieldMapAttribute("SERIALNO", typeof(string), 40, false)]
        public string Serialno;

        ///<summary>
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
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

    }
    #endregion

    #region ITEMTRANSLOTFORTRANS
    public class ItemTransLotForTrans : ItemTransLot
    {
        public ItemTransLotForTrans()
        {
        }
        ///<summary>
        ///FRMSTORAGEQTY
        ///</summary>
        [FieldMapAttribute("FRMSTORAGEQTY", typeof(decimal), 28, true)]
        public decimal FrmStorageQty;

        ///<summary>
        ///SERIAL
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 38, false)]
        public int Serial;

        ///<summary>
        ///TRANSNO
        ///</summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 40, true)]
        public string Transno;

        ///<summary>
        ///STACKCODE
        ///</summary>
        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string Stackcode;

        ///<summary>
        ///TRANSLINE
        ///</summary>
        [FieldMapAttribute("TRANSLINE", typeof(int), 22, true)]
        public int Transline;
    }
    #endregion

    #region ITEMTRANSLOTDETAILFORTRANS
    public class ItemTransLotDetailForTrans : ItemTransLotDetail
    {
        public ItemTransLotDetailForTrans()
        {
        }
        ///<summary>
        ///STACKCODE
        ///</summary>
        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string Stackcode;
    }
    #endregion

    [Serializable]
    public class MaterialMSLExc : MaterialMSL
    {
        [FieldMapAttribute("MaterialName", typeof(string), 40, true)]
        public string MaterialName;

        [FieldMapAttribute("MaterialDes", typeof(string), 40, true)]
        public string MaterialDes;

        [FieldMapAttribute("MHUMIDITYLEVELDESC", typeof(string), 40, true)]
        public string MHumidityLevelDesc;

        [FieldMapAttribute("FLOORLIFE", typeof(int), 10, false)]
        public int FloorLife;

        [FieldMapAttribute("DRYINGTIME", typeof(int), 10, false)]
        public int DryingTime;

        [FieldMapAttribute("INDRYINGTIME", typeof(int), 10, false)]
        public int InDryingTime;
    }


    #region TBLITEMLot
    [Serializable, TableMap("TBLITEMLot", "LotNO")]
    public class ITEMLot : DomainObject
    {
        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("LOTNO", typeof(string), 100, false)]
        public string LotNO;

        /// <summary>
        /// ���ϱ��
        /// </summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        /// <summary>
        /// ��֯ID
        /// </summary>
        [FieldMapAttribute("ORGID", typeof(int), 8, false)]
        public int OrgID;

        /// <summary>
        /// ���׵���
        /// </summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 50, true)]
        public string TransNO;

        /// <summary>
        /// ���׵��к�
        /// </summary>
        [FieldMapAttribute("TRANSLINE", typeof(int), 8, true)]
        public int TransLine;

        /// <summary>
        /// �������ϴ���
        /// </summary>
        [FieldMapAttribute("VENDORITEMCODE", typeof(string), 100, true)]
        public string VendorItemCode;

        /// <summary>
        /// ���̴���
        /// </summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 100, true)]
        public string VendorCode;

        /// <summary>
        /// ��Ӧ����������
        /// </summary>
        [FieldMapAttribute("VENDERLOTNO", typeof(string), 40, true)]
        public string VenderLotNO;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("DATECODE", typeof(int), 8, false)]
        public int DateCode;

        /// <summary>
        /// ԭʼ��������
        /// </summary>
        [FieldMapAttribute("LOTQTY", typeof(int), 13, false)]
        public int LotQty;

        /// <summary>
        /// �Ƿ���Ч
        /// </summary>
        [FieldMapAttribute("ACTIVE", typeof(string), 1, false)]
        public string Active;

        /// <summary>
        /// ʧЧ����
        /// </summary>
        [FieldMapAttribute("EXDATE", typeof(int), 8, false)]
        public int Exdate;

        /// <summary>
        /// ��ӡ����
        /// </summary>
        [FieldMapAttribute("PRINTTIMES", typeof(int), 6, false)]
        public int PrintTimes;

        /// <summary>
        /// ����ӡ��
        /// </summary>
        [FieldMapAttribute("LASTPRINTUSER", typeof(string), 40, false)]
        public string lastPrintUSER;

        /// <summary>
        /// ����ӡ����
        /// </summary>
        [FieldMapAttribute("LASTPRINTDATE", typeof(int), 8, false)]
        public int lastPrintDate;

        /// <summary>
        /// ����ӡʱ��
        /// </summary>
        [FieldMapAttribute("LASTPRINTTIME", typeof(int), 6, false)]
        public int lastPrintTime;

        /// <summary>
        /// ά������
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        /// <summary>
        /// ά����
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;
    }
    #endregion

    #region ITEMLOT
    /// <summary>
    /// TBLITEMLOT
    /// </summary>
    [Serializable, TableMap("TBLITEMLOT", "LOTNO,MCODE")]
    public class ItemLot : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public ItemLot()
        {
        }

        ///<summary>
        ///LOTNO
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 100, false)]
        public string Lotno;

        ///<summary>
        ///MCODE
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string Mcode;

        ///<summary>
        ///ORGID
        ///</summary>
        [FieldMapAttribute("ORGID", typeof(int), 22, false)]
        public int Orgid;

        ///<summary>
        ///TRANSNO
        ///</summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 50, true)]
        public string Transno;

        ///<summary>
        ///TRANSLINE
        ///</summary>
        [FieldMapAttribute("TRANSLINE", typeof(int), 22, true)]
        public int Transline;

        ///<summary>
        ///VENDORITEMCODE
        ///</summary>
        [FieldMapAttribute("VENDORITEMCODE", typeof(string), 100, true)]
        public string Vendoritemcode;

        ///<summary>
        ///VENDORCODE
        ///</summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 100, true)]
        public string Vendorcode;

        ///<summary>
        ///VENDERLOTNO
        ///</summary>
        [FieldMapAttribute("VENDERLOTNO", typeof(string), 40, true)]
        public string Venderlotno;

        ///<summary>
        ///DATECODE
        ///</summary>
        [FieldMapAttribute("DATECODE", typeof(int), 8, true)]
        public int Datecode;

        ///<summary>
        ///LOTQTY
        ///</summary>
        [FieldMapAttribute("LOTQTY", typeof(decimal), 28, false)]
        public decimal Lotqty;

        ///<summary>
        ///ACTIVE
        ///</summary>
        [FieldMapAttribute("ACTIVE", typeof(string), 1, false)]
        public string Active;

        ///<summary>
        ///EXDATE
        ///</summary>
        [FieldMapAttribute("EXDATE", typeof(int), 8, false)]
        public int Exdate;

        ///<summary>
        ///PRINTTIMES
        ///</summary>
        [FieldMapAttribute("PRINTTIMES", typeof(int), 6, false)]
        public int Printtimes;

        ///<summary>
        ///LASTPRINTUSER
        ///</summary>
        [FieldMapAttribute("LASTPRINTUSER", typeof(string), 40, false)]
        public string Lastprintuser;

        ///<summary>
        ///LASTPRINTDATE
        ///</summary>
        [FieldMapAttribute("LASTPRINTDATE", typeof(int), 8, false)]
        public int Lastprintdate;

        ///<summary>
        ///LASTPRINTTIME
        ///</summary>
        [FieldMapAttribute("LASTPRINTTIME", typeof(int), 6, false)]
        public int Lastprinttime;

        /// <summary>
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// ���ά��ʱ��[LastMaintainTime]
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// ���ά������[LastMaintainDate]
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

    }
    #endregion

    #region ItemLotForQuery
    /// <summary>
    /// ItemLotForQuery
    /// </summary>

    public class ItemLotForQuery : ItemLot
    {
        public ItemLotForQuery()
        {
        }


        ///<summary>
        ///VENDORNAME
        ///</summary>
        [FieldMapAttribute("VENDORNAME", typeof(string), 100, false)]
        public string VendorName;


        ///<summary>
        ///Material Description
        ///</summary>	
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string MaterialDescription;

        ///<summary>
        ///Material Name
        ///</summary>	
        [FieldMapAttribute("MNAME", typeof(string), 40, true)]
        public string MaterialName;

        ///<summary>
        ///StorageID
        ///</summary>	
        [FieldMapAttribute("STORAGEID", typeof(string), 40, false)]
        public string StorageID;


        ///<summary>
        ///STACKCODE
        ///</summary>	
        [FieldMapAttribute("STACKCODE", typeof(string), 40, false)]
        public string StackCode;

    }
    #endregion

    #region TBLITEMLOTFORTRANS
    /// <summary>
    /// TBLITEMLOTFORTRANS
    /// </summary> 
    public class ItemLotForTrans : ItemLot
    {
        public ItemLotForTrans()
        {
        }
        ///<summary>
        ///FRMSTORAGEQTY
        ///</summary>
        [FieldMapAttribute("FRMSTORAGEQTY", typeof(decimal), 28, true)]
        public decimal FrmStorageQty;

        ///<summary>
        ///STACKCODE
        ///</summary>
        [FieldMapAttribute("STACKCODE", typeof(string), 40, true)]
        public string Stackcode;

    }
    #endregion

    #region ITEMLOTDETAIL
    /// <summary>
    /// TBLITEMLOTDETAIL
    /// </summary>
    [Serializable, TableMap("TBLITEMLOTDETAIL", "SERIALNO,MCODE")]
    public class ItemLotDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public ItemLotDetail()
        {
        }

        ///<summary>
        ///SERIALNO
        ///</summary>
        [FieldMapAttribute("SERIALNO", typeof(string), 40, false)]
        public string Serialno;

        ///<summary>
        ///LOTNO
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string Lotno;

        ///<summary>
        ///MCODE
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string Mcode;

        ///<summary>
        ///STORAGEID
        ///</summary>
        [FieldMapAttribute("STORAGEID", typeof(string), 40, true)]
        public string Storageid;

        ///<summary>
        ///STACKCODE
        ///</summary>
        [FieldMapAttribute("STACKCODE", typeof(string), 40, true)]
        public string Stackcode;

        ///<summary>
        ///SERIALSTATUS
        ///</summary>
        [FieldMapAttribute("SERIALSTATUS", typeof(string), 40, false)]
        public string Serialstatus;

        ///<summary>
        ///PRINTTIMES
        ///</summary>
        [FieldMapAttribute("PRINTTIMES", typeof(int), 22, false)]
        public int Printtimes;

        ///<summary>
        ///LASTPRINTUSER
        ///</summary>
        [FieldMapAttribute("LASTPRINTUSER", typeof(string), 40, false)]
        public string Lastprintuser;

        ///<summary>
        ///LASTPRINTDATE
        ///</summary>
        [FieldMapAttribute("LASTPRINTDATE", typeof(int), 8, false)]
        public int Lastprintdate;

        ///<summary>
        ///LASTPRINTTIME
        ///</summary>
        [FieldMapAttribute("LASTPRINTTIME", typeof(int), 6, false)]
        public int Lastprinttime;

        /// <summary>
        /// ���ά���û�[LastMaintainUser]
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        /// <summary>
        /// ���ά��ʱ��[LastMaintainTime]
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

        /// <summary>
        /// ���ά������[LastMaintainDate]
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;


    }
    #endregion

    #region TBLITEMLOTDETAILFORTRANS
    /// <summary>
    /// TBLITEMLOTDETAILFORTRANS
    /// </summary> 
    public class ItemLotDetailForTrans : ItemLotDetail
    {
        public ItemLotDetailForTrans()
        {
        }
        ///<summary>
        ///FRMSTORAGEQTY
        ///</summary>
        [FieldMapAttribute("FRMSTORAGEQTY", typeof(decimal), 28, true)]
        public decimal FrmStorageQty;

        ///<summary>
        ///LOTQTY
        ///</summary>
        [FieldMapAttribute("LOTQTY", typeof(decimal), 28, false)]
        public decimal Lotqty;
    }
    #endregion

    #region LOTCHANGELOG
    /// <summary>
    /// TBLLOTCHANGELOG
    /// </summary>
    [Serializable, TableMap("TBLLOTCHANGELOG", "SERIAL")]
    public class LotChangeLog : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public LotChangeLog()
        {
        }

        ///<summary>
        ///SERIAL
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///NEWLOTNO
        ///</summary>
        [FieldMapAttribute("NEWLOTNO", typeof(string), 100, false)]
        public string Newlotno;

        ///<summary>
        ///NEWLOTQTY
        ///</summary>
        [FieldMapAttribute("NEWLOTQTY", typeof(decimal), 22, false)]
        public decimal Newlotqty;

        ///<summary>
        ///OLDLOTNO
        ///</summary>
        [FieldMapAttribute("OLDLOTNO", typeof(string), 100, false)]
        public string Oldlotno;

        ///<summary>
        ///OLDLOTQTY
        ///</summary>
        [FieldMapAttribute("OLDLOTQTY", typeof(decimal), 22, false)]
        public decimal Oldlotqty;

        ///<summary>
        ///CHGTYPE
        ///</summary>
        [FieldMapAttribute("CHGTYPE", typeof(string), 40, false)]
        public string Chgtype;

        ///<summary>
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
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

    }
    #endregion

    #region TBLMSDLOT
    [Serializable, TableMap("TBLMSDLOT", "LotNo")]
    public class MSDLOT : DomainObject
    {
        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string LotNo;

        /// <summary>
        /// MSD״̬
        /// </summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        /// <summary>
        ///��Ч����������Сʱ��
        /// </summary>
        [FieldMapAttribute("FLOORlIFE", typeof(decimal), 15, false)]
        public decimal Floorlife;

        /// <summary>
        ///ʣ�೵��������Сʱ��
        /// </summary>
        [FieldMapAttribute("OVERFLOORlIFE", typeof(decimal), 15, false)]
        public decimal OverFloorlife;

        /// <summary>
        /// ά������
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        /// <summary>
        /// ά����
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;
    }
    #endregion

    #region TBLSERIALBOOK
    /// <summary>
    /// �������������к�
    /// </summary>
    [Serializable, TableMap("TBLSERIALBOOK", "SNPREFIX")]
    public class SERIALBOOK : DomainObject
    {
        public SERIALBOOK()
        {
        }

        /// <summary>
        /// ���к�ǰ׺
        /// </summary>
        [FieldMapAttribute("SNPREFIX", typeof(string), 40, false)]
        public string SNPrefix;

        ///// <summary>
        ///// ����
        ///// </summary>
        //[FieldMapAttribute("DATECODE", typeof(string), 40, false)]
        //public string DateCode;

        /// <summary>
        /// ���к����Serial����
        /// </summary>
        [FieldMapAttribute("MAXSERIAL", typeof(string), 40, false)]
        public string MaxSerial;

        /// <summary>
        /// ά����
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MUser;

        /// <summary>
        /// ά������
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MDate;

        /// <summary>
        /// ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MTime;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("SERIALTYPE", typeof(string), 40, true)]
        public string SerialType;

    }
    #endregion


    #region InvTransfer
    /// <summary>
    /// TBLInvTransfer
    /// </summary>
    [Serializable, TableMap("TBLINVTRANSFER", "TRANSFERNO")]
    public class InvTransfer : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public InvTransfer()
        {
        }

        ///<summary>
        ///TRANSFERNO
        ///</summary>
        [FieldMapAttribute("TRANSFERNO", typeof(string), 40, false)]
        public string TransferNO;

        ///<summary>
        ///FRMSTORAGEID
        ///</summary>
        [FieldMapAttribute("FRMSTORAGEID", typeof(string), 40, false)]
        public string FromStorageID;

        ///<summary>
        ///TOSTORAGEID
        ///</summary>
        [FieldMapAttribute("TOSTORAGEID", typeof(string), 40, true)]
        public string ToStorageID;

        ///<summary>
        ///TRANSFERSTATUS
        ///</summary>
        [FieldMapAttribute("TRANSFERSTATUS", typeof(string), 40, false)]
        public string TransferStatus;

        ///<summary>
        ///RECTYPE
        ///</summary>
        [FieldMapAttribute("RECTYPE", typeof(string), 40, false)]
        public string Rectype;

        ///<summary>
        ///MEMO
        ///</summary>
        [FieldMapAttribute("MEMO", typeof(string), 2000, true)]
        public string Memo;


        ///<summary>
        ///CREATEDATE
        ///</summary>
        [FieldMapAttribute("CREATEDATE", typeof(int), 22, true)]
        public int CreateDate;

        ///<summary>
        ///CREATETIME
        ///</summary>
        [FieldMapAttribute("CREATETIME", typeof(int), 22, true)]
        public int CreateTime;

        ///<summary>
        ///CREATEUSER
        ///</summary>
        [FieldMapAttribute("CREATEUSER", typeof(string), 40, true)]

        public string CreateUser;

        ///<summary>
        ///ORGID
        ///</summary>
        [FieldMapAttribute("ORGID", typeof(int), 22, false)]
        public int OrgID;

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
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string Muser;

    }
    #endregion

    #region InvTransferDetail
    /// <summary>
    /// TBLInvTransferDetail
    /// </summary>
    [Serializable, TableMap("TBLINVTRANSFERDETAIL", "TRANSFERNO,TRANSFERLINE")]
    public class InvTransferDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public InvTransferDetail()
        {
        }

        ///<summary>
        ///TRANSFERNO
        ///</summary>
        [FieldMapAttribute("TRANSFERNO", typeof(string), 40, false)]
        public string TransferNO;

        ///<summary>
        ///TRANSFERLINE
        ///</summary>
        [FieldMapAttribute("TRANSFERLINE", typeof(int), 22, false)]
        public int TransferLine;

        ///<summary>
        ///ORDERNO
        ///</summary>
        [FieldMapAttribute("ORDERNO", typeof(string), 40, true)]
        public string OrderNO;

        ///<summary>
        ///ORDERLINE
        ///</summary>
        [FieldMapAttribute("ORDERLINE", typeof(int), 22, true)]
        public int OrderLine;

        ///<summary>
        ///TRANSFERSTATUS
        ///</summary>
        [FieldMapAttribute("TRANSFERSTATUS", typeof(string), 40, false)]
        public string TransferStatus;

        ///<summary>
        ///MEMO
        ///</summary>
        [FieldMapAttribute("MEMO", typeof(string), 2000, true)]
        public string Memo;

        ///<summary>
        ///ITEMCODE
        ///</summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, false)]
        public string ItemCode;

        ///<summary>
        ///MOCODE
        ///</summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string MOCode;

        ///<summary>
        ///PLANQTY
        ///</summary>
        [FieldMapAttribute("PLANQTY", typeof(decimal), 28, false)]
        public decimal Planqty;

        ///<summary>
        ///ACTQTY
        ///</summary>
        [FieldMapAttribute("ACTQTY", typeof(decimal), 28, true)]
        public decimal Actqty;

        ///<summary>
        ///CUSTOMERCODE
        ///</summary>
        [FieldMapAttribute("CUSTOMERCODE", typeof(string), 40, true)]
        public string CustomerCode;

        ///<summary>
        ///CUSTOMERNAME
        ///</summary>
        [FieldMapAttribute("CUSTOMERNAME", typeof(string), 100, true)]
        public string CustomerName;

        ///<summary>
        ///TRANSFERDATE
        ///</summary>
        [FieldMapAttribute("TRANSFERDATE", typeof(int), 22, true)]
        public int TransferDate;

        ///<summary>
        ///TRANSFERTIME
        ///</summary>
        [FieldMapAttribute("TRANSFERTIME", typeof(int), 22, true)]
        public int TransferTime;

        ///<summary>
        ///TRANSFERUSER
        ///</summary>
        [FieldMapAttribute("TRANSFERUSER", typeof(string), 40, true)]
        public string TransferUser;

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
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string Muser;

    }
    #endregion

    #region INVTRANSFERMERGE
    /// <summary>
    /// TBLINVTRANSFERMERGE
    /// </summary>
    [Serializable, TableMap("TBLINVTRANSFERMERGE", "SERIAL")]
    public class InvTransferMerge : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public InvTransferMerge()
        {
        }

        ///<summary>
        ///SERIAL
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///TRANSFERNO
        ///</summary>
        [FieldMapAttribute("TRANSFERNO", typeof(string), 40, false)]
        public string Transferno;

        ///<summary>
        ///FRMTRANSFERNO
        ///</summary>
        [FieldMapAttribute("FRMTRANSFERNO", typeof(string), 40, false)]
        public string Frmtransferno;

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
        ///MUSER
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string Muser;

    }
    #endregion

    #region InvTransferDetailForQuery
    public class InvTransferDetailForQuey : InvTransferDetail
    {
        public InvTransferDetailForQuey()
        {
        }
        ///<summary>
        ///Material Description
        ///</summary>	
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string MaterialDescription;

        ///<summary>
        ///TransQTY
        ///</summary>	
        [FieldMapAttribute("TRANSQTY", typeof(decimal), 28, true)]
        public decimal TransQTY;

        ///<summary>
        ///TOSTORAGEID
        ///</summary>
        [FieldMapAttribute("TOSTORAGEID", typeof(string), 40, false)]
        public string ToStorageID;

        ///<summary>
        ///FRMSTORAGEID
        ///</summary>
        [FieldMapAttribute("FRMSTORAGEID", typeof(string), 40, false)]
        public string FromStorageID;

        ///<summary>
        ///STACKCODE
        ///</summary>
        [FieldMapAttribute("STACKCODE", typeof(string), 40, true)]
        public string StackCODE;

        ///<summary>
        ///TOSTORAGEQTY
        ///</summary>
        [FieldMapAttribute("TOSTORAGEQTY", typeof(decimal), 28, true)]
        public decimal ToStorageQty;

        ///<summary>
        ///FRMSTORAGEQTY
        ///</summary>
        [FieldMapAttribute("FRMSTORAGEQTY", typeof(decimal), 28, true)]
        public decimal FrmStorageQty;

        ///<summary>
        ///MCONTROLTYPE
        ///</summary>
        [FieldMap("MCONTROLTYPE", typeof(string), 40, false)]
        public string MaterialControlType;

        //edit by kathy @20140626 ��ѯ�����Ͽ������-�ѱ�������
        ///<summary>
        ///TOSTORAGEQTY��������Դ�������-�ѱ�������
        ///</summary>
        [FieldMapAttribute("frmlotqty", typeof(decimal), 28, true)]
        public decimal FrmLotQty;

        ///<summary>
        ///FRMSTORAGEQTY��������Ŀ�Ŀ������-�ѱ�������
        ///</summary>
        [FieldMapAttribute("tolotqty", typeof(decimal), 28, true)]
        public decimal ToLotQty;
    }
    #endregion

    [Serializable]
    public class MSDLOTLExc : MSDLOT
    {
        [FieldMapAttribute("MNAME", typeof(string), 40, true)]
        public string MNAME;

        [FieldMapAttribute("MDESC", typeof(string), 40, true)]
        public string MDESC;

        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCODE;
    }

    #region MSDWIP
    [Serializable, TableMap("TBLMSDWIP", "SERIAL")]
    public class MSDWIP : DomainObject
    {
        /// <summary>
        ///�Զ����ӣ�trigger��
        /// </summary>
        [FieldMapAttribute("SERIAL", typeof(int), 38, false)]
        public int serial;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string LotNo;

        /// <summary>
        /// MSD״̬
        /// </summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        /// <summary>
        /// ά������
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, false)]
        public int MaintainDate;

        /// <summary>
        /// ά��ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, false)]
        public int MaintainTime;

        /// <summary>
        /// ά����
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;
    }
    #endregion

    #region Sapcloseperiod
    /// <summary>
    /// TBLSAPCLOSEPERIOD
    /// </summary>
    [Serializable, TableMap("TBLSAPCLOSEPERIOD", "SERIAL")]
    public class Sapcloseperiod : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Sapcloseperiod()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///StartDate
        ///</summary>
        [FieldMapAttribute("STARTDATE", typeof(int), 22, false)]
        public int StartDate;

        ///<summary>
        ///StartTime
        ///</summary>
        [FieldMapAttribute("STARTTIME", typeof(int), 22, false)]
        public int StartTime;

        ///<summary>
        ///EndDate
        ///</summary>
        [FieldMapAttribute("ENDDATE", typeof(int), 22, false)]
        public int EndDate;

        ///<summary>
        ///EndTime
        ///</summary>
        [FieldMapAttribute("ENDTIME", typeof(int), 22, false)]
        public int EndTime;

        ///<summary>
        ///Orgid
        ///</summary>
        [FieldMapAttribute("ORGID", typeof(int), 22, false)]
        public int Orgid;

        ///<summary>
        ///Eattribute1
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE1", typeof(string), 40, true)]
        public string Eattribute1;

        ///<summary>
        ///Eattribute2
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE2", typeof(string), 40, true)]
        public string Eattribute2;

        ///<summary>
        ///Eattribute3
        ///</summary>
        [FieldMapAttribute("EATTRIBUTE3", typeof(string), 40, true)]
        public string Eattribute3;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

        ///<summary>
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, true)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, true)]
        public int MaintainTime;

    }
    #endregion

    //#region Asn  Amy Add
    ///// <summary>
    ///// TBLASN
    ///// </summary>

    //[Serializable, TableMap("TBLASN", "STNO")]
    //public class Asn : BenQGuru.eMES.Common.Domain.DomainObject
    //{
    //    public Asn()
    //    {
    //    }

    //    ///<summary>
    //    ///InitrejectQty
    //    ///</summary>
    //    [FieldMapAttribute("INITREJECTQTY", typeof(int), 22, true)]
    //    public int InitrejectQty;

    //    ///<summary>
    //    ///InitreceiveDesc
    //    ///</summary>
    //    [FieldMapAttribute("INITRECEIVEDESC", typeof(string), 200, true)]
    //    public string InitreceiveDesc;

    //    ///<summary>
    //    ///InitreceiveQty
    //    ///</summary>
    //    [FieldMapAttribute("INITRECEIVEQTY", typeof(int), 22, true)]
    //    public int InitreceiveQty;

    //    ///<summary>
    //    ///InitgiveinQty
    //    ///</summary>
    //    [FieldMapAttribute("INITGIVEINQTY", typeof(int), 22, true)]
    //    public int InitgiveinQty;

    //    ///<summary>
    //    ///Stno
    //    ///</summary>
    //    [FieldMapAttribute("STNO", typeof(string), 40, false)]
    //    public string Stno;

    //    ///<summary>
    //    ///StType
    //    ///</summary>
    //    [FieldMapAttribute("STTYPE", typeof(string), 40, false)]
    //    public string StType;

    //    ///<summary>
    //    ///Invno
    //    ///</summary>
    //    [FieldMapAttribute("INVNO", typeof(string), 40, true)]
    //    public string Invno;

    //    ///<summary>
    //    ///VEndorCode
    //    ///</summary>
    //    [FieldMapAttribute("VENDORCODE", typeof(string), 40, true)]
    //    public string VEndorCode;

    //    ///<summary>
    //    ///Status
    //    ///</summary>
    //    [FieldMapAttribute("STATUS", typeof(string), 40, false)]
    //    public string Status;

    //    ///<summary>
    //    ///Oano
    //    ///</summary>
    //    [FieldMapAttribute("OANO", typeof(string), 40, true)]
    //    public string Oano;

    //    ///<summary>
    //    ///FacCode
    //    ///</summary>
    //    [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
    //    public string FacCode;

    //    ///<summary>
    //    ///StorageCode
    //    ///</summary>
    //    [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
    //    public string StorageCode;

    //    ///<summary>
    //    ///FromfacCode
    //    ///</summary>
    //    [FieldMapAttribute("FROMFACCODE", typeof(string), 40, true)]
    //    public string FromfacCode;

    //    ///<summary>
    //    ///FromstorageCode
    //    ///</summary>
    //    [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 40, true)]
    //    public string FromstorageCode;

    //    ///<summary>
    //    ///Gross_weight
    //    ///</summary>
    //    [FieldMapAttribute("GROSS_WEIGHT", typeof(decimal), 22, true)]
    //    public decimal Gross_weight;

    //    ///<summary>
    //    ///Volume
    //    ///</summary>
    //    [FieldMapAttribute("VOLUME", typeof(string), 40, true)]
    //    public string Volume;

    //    ///<summary>
    //    ///Exigency_flag
    //    ///</summary>
    //    [FieldMapAttribute("EXIGENCY_FLAG", typeof(string), 1, true)]
    //    public string Exigency_flag;

    //    ///<summary>
    //    ///Direct_flag
    //    ///</summary>
    //    [FieldMapAttribute("DIRECT_FLAG", typeof(string), 1, true)]
    //    public string Direct_flag;

    //    ///<summary>
    //    ///Rejects_flag
    //    ///</summary>
    //    [FieldMapAttribute("REJECTS_FLAG", typeof(string), 1, true)]
    //    public string Rejects_flag;

    //    ///<summary>
    //    ///Pickno
    //    ///</summary>
    //    [FieldMapAttribute("PICKNO", typeof(string), 40, true)]
    //    public string Pickno;

    //    ///<summary>
    //    ///Predict_Date
    //    ///</summary>
    //    [FieldMapAttribute("PREDICT_DATE", typeof(int), 22, true)]
    //    public int Predict_Date;

    //    ///<summary>
    //    ///Packinglistno
    //    ///</summary>
    //    [FieldMapAttribute("PACKINGLISTNO", typeof(string), 40, true)]
    //    public string Packinglistno;

    //    ///<summary>
    //    ///Provide_Date
    //    ///</summary>
    //    [FieldMapAttribute("PROVIDE_DATE", typeof(int), 22, true)]
    //    public int Provide_Date;

    //    ///<summary>
    //    ///ReworkapplyUser
    //    ///</summary>
    //    [FieldMapAttribute("REWORKAPPLYUSER", typeof(string), 40, true)]
    //    public string ReworkapplyUser;

    //    ///<summary>
    //    ///CUser
    //    ///</summary>
    //    [FieldMapAttribute("CUSER", typeof(string), 40, false)]
    //    public string CUser;

    //    ///<summary>
    //    ///CDate
    //    ///</summary>
    //    [FieldMapAttribute("CDATE", typeof(int), 22, false)]
    //    public int CDate;

    //    ///<summary>
    //    ///CTime
    //    ///</summary>
    //    [FieldMapAttribute("CTIME", typeof(int), 22, false)]
    //    public int CTime;

    //    ///<summary>
    //    ///MaintainDate
    //    ///</summary>
    //    [FieldMapAttribute("MDATE", typeof(int), 22, false)]
    //    public int MaintainDate;

    //    ///<summary>
    //    ///MaintainTime
    //    ///</summary>
    //    [FieldMapAttribute("MTIME", typeof(int), 22, false)]
    //    public int MaintainTime;

    //    ///<summary>
    //    ///MaintainUser
    //    ///</summary>
    //    [FieldMapAttribute("MUSER", typeof(string), 40, false)]
    //    public string MaintainUser;

    //    ///<summary>
    //    ///Remark1
    //    ///</summary>
    //    [FieldMapAttribute("REMARK1", typeof(string), 200, true)]
    //    public string Remark1;

    //}


    //#endregion
    #region Asn
    /// <summary>
    /// TBLASN
    /// </summary>
    [Serializable, TableMap("TBLASN", "STNO")]
    public class Asn : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Asn()
        {
        }

        ///<summary>
        ///InitrejectQty
        ///</summary>
        [FieldMapAttribute("INITREJECTQTY", typeof(int), 22, true)]
        public int InitrejectQty;

        ///<summary>
        ///InitreceiveDesc
        ///</summary>
        [FieldMapAttribute("INITRECEIVEDESC", typeof(string), 200, true)]
        public string InitreceiveDesc;

        ///<summary>
        ///InitreceiveQty
        ///</summary>
        [FieldMapAttribute("INITRECEIVEQTY", typeof(int), 22, true)]
        public int InitreceiveQty;

        ///<summary>
        ///InitgiveinQty
        ///</summary>
        [FieldMapAttribute("INITGIVEINQTY", typeof(int), 22, true)]
        public int InitgiveinQty;

        ///<summary>
        ///Stno
        ///</summary>
        [FieldMapAttribute("STNO", typeof(string), 40, false)]
        public string Stno;

        ///<summary>
        ///StType
        ///</summary>
        [FieldMapAttribute("STTYPE", typeof(string), 40, false)]
        public string StType;

        ///<summary>
        ///Invno
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, true)]
        public string Invno;

        ///<summary>
        ///VEndorCode
        ///</summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, true)]
        public string VEndorCode;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///Oano
        ///</summary>
        [FieldMapAttribute("OANO", typeof(string), 40, true)]
        public string Oano;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FacCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        ///<summary>
        ///FromfacCode
        ///</summary>
        [FieldMapAttribute("FROMFACCODE", typeof(string), 40, true)]
        public string FromfacCode;

        ///<summary>
        ///FromstorageCode
        ///</summary>
        [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 40, true)]
        public string FromstorageCode;

        ///<summary>
        ///Gross_weight
        ///</summary>
        [FieldMapAttribute("GROSS_WEIGHT", typeof(decimal), 22, true)]
        public decimal Gross_weight;

        ///<summary>
        ///Volume
        ///</summary>
        [FieldMapAttribute("VOLUME", typeof(string), 40, true)]
        public string Volume;

        ///<summary>
        ///Exigency_flag
        ///</summary>
        [FieldMapAttribute("EXIGENCY_FLAG", typeof(string), 1, true)]
        public string Exigency_flag;

        ///<summary>
        ///Direct_flag
        ///</summary>
        [FieldMapAttribute("DIRECT_FLAG", typeof(string), 1, true)]
        public string Direct_flag;

        ///<summary>
        ///Rejects_flag
        ///</summary>
        [FieldMapAttribute("REJECTS_FLAG", typeof(string), 1, true)]
        public string Rejects_flag;

        ///<summary>
        ///Pickno
        ///</summary>
        [FieldMapAttribute("PICKNO", typeof(string), 40, true)]
        public string Pickno;

        ///<summary>
        ///Predict_Date
        ///</summary>
        [FieldMapAttribute("PREDICT_DATE", typeof(int), 22, true)]
        public int Predict_Date;

        ///<summary>
        ///Packinglistno
        ///</summary>
        [FieldMapAttribute("PACKINGLISTNO", typeof(string), 40, true)]
        public string Packinglistno;

        ///<summary>
        ///Provide_Date
        ///</summary>
        //[FieldMapAttribute("PROVIDE_DATE", typeof(int), 22, true)]
        //public int Provide_Date;

        [FieldMapAttribute("PROVIDE_DATE", typeof(string), 40, true)]
        public string Provide_Date;

        ///<summary>
        ///ReworkapplyUser
        ///</summary>
        [FieldMapAttribute("REWORKAPPLYUSER", typeof(string), 40, true)]
        public string ReworkapplyUser;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        ///<summary>
        ///Remark1
        ///</summary>
        [FieldMapAttribute("REMARK1", typeof(string), 200, true)]
        public string Remark1;

    }
    #endregion

    #region Asndetail   Amy add
    /// <summary>
    /// TBLASNDETAIL
    /// </summary>
    [Serializable, TableMap("TBLASNDETAIL", "STLINE,STNO")]
    public class Asndetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Asndetail()
        {
        }






        ///<summary>
        ///InitreceiveStatus
        ///</summary>
        [FieldMapAttribute("INITRECEIVESTATUS", typeof(string), 40, true)]
        public string InitreceiveStatus;

        ///<summary>
        ///InitreceiveDesc
        ///</summary>
        [FieldMapAttribute("INITRECEIVEDESC", typeof(string), 200, true)]
        public string InitreceiveDesc;

        ///<summary>
        ///VEndormCodeDesc
        ///</summary>
        [FieldMapAttribute("VENDORMCODEDESC", typeof(string), 100, true)]
        public string VEndormCodeDesc;

        ///<summary>
        ///VEndormCode
        ///</summary>
        [FieldMapAttribute("VENDORMCODE", typeof(string), 40, true)]
        public string VEndormCode;

        ///<summary>
        ///Stno
        ///</summary>
        [FieldMapAttribute("STNO", typeof(string), 40, false)]
        public string Stno;

        ///<summary>
        ///Stline
        ///</summary>
        [FieldMapAttribute("STLINE", typeof(string), 40, false)]
        public string Stline;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, true)]
        public string Cartonno;

        ///<summary>
        ///Cartonbigseq
        ///</summary>
        [FieldMapAttribute("CARTONBIGSEQ", typeof(string), 40, true)]
        public string Cartonbigseq;

        ///<summary>
        ///Cartonseq
        ///</summary>
        [FieldMapAttribute("CARTONSEQ", typeof(string), 40, true)]
        public string Cartonseq;

        ///<summary>
        ///CustmCode
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 40, true)]
        public string CustmCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DqmCode;

        ///<summary>
        ///MDesc
        ///</summary>
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string MDesc;

        ///<summary>
        ///ReceivemCode
        ///</summary>
        [FieldMapAttribute("RECEIVEMCODE", typeof(string), 40, true)]
        public string ReceivemCode;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(int), 22, false)]
        public int Qty;

        ///<summary>
        ///ReceiveQty
        ///</summary>
        [FieldMapAttribute("RECEIVEQTY", typeof(int), 22, true)]
        public int ReceiveQty;

        ///<summary>
        ///ActQty
        ///</summary>
        [FieldMapAttribute("ACTQTY", typeof(int), 22, true)]
        public int ActQty;

        ///<summary>
        ///QcpassQty
        ///</summary>
        [FieldMapAttribute("QCPASSQTY", typeof(int), 22, true)]
        public int QcpassQty;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Production_Date
        ///</summary>
        [FieldMapAttribute("PRODUCTION_DATE", typeof(int), 22, true)]
        public int Production_Date;

        ///<summary>
        ///Supplier_lotno
        ///</summary>
        [FieldMapAttribute("SUPPLIER_LOTNO", typeof(string), 40, true)]
        public string Supplier_lotno;

        ///<summary>
        ///Lotno
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string Lotno;


        ///<summary>
        ///StorageageDate
        ///</summary>
        [FieldMapAttribute("STORAGEAGEDATE", typeof(int), 22, true)]
        public int StorageageDate;

        ///<summary>
        ///Remark1
        ///</summary>
        [FieldMapAttribute("REMARK1", typeof(string), 200, true)]
        public string Remark1;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

        [FieldMapAttribute("LocationCode1", typeof(string), 22, false)]
        public string LocationCode1;

        [FieldMapAttribute("Remark2", typeof(string), 200, false)]
        public string Remark2;

        [FieldMapAttribute("INITGIVEINDESC", typeof(string), 40, false)]
        public string InitGIVEINDESC;

    }
    #endregion
    #region  Amy add


    /// <summary>
    /// TBLASNDETAILEX
    /// </summary>
    public class AsndetailEX : Asndetail
    {
        public AsndetailEX()
        {
        }

        ///<summary>
        ///�������
        ///</summary>
        [FieldMapAttribute("MControlType", typeof(string), 40, true)]
        public string MControlType;


        //add by bela 20160222
        ///<summary>
        ///StType
        ///</summary>
        [FieldMapAttribute("STTYPE", typeof(string), 40, false)]
        public string StType;

        ///<summary>
        ///Invno
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, true)]
        public string Invno;
    }

    #endregion
    #region  Amy add

    public class invoicedetailEX : InvoicesDetail
    {
        public invoicedetailEX()
        {
        }

        ///<summary>
        ///stno
        ///</summary>
        [FieldMapAttribute("stno", typeof(string), 40, true)]
        public string stno;

        ///<summary>
        ///stline
        ///</summary>
        [FieldMapAttribute("stline", typeof(string), 40, true)]
        public string stline;

        ///<summary>
        ///EQTY
        ///</summary>
        [FieldMapAttribute("EQTY", typeof(Decimal), 40, true)]
        public Decimal EQTY;
        ///<summary>
        ///INQTY
        ///</summary>
        [FieldMapAttribute("INQTY", typeof(Decimal), 40, true)]
        public Decimal INQTY;

    }

    #endregion

    #region Asndetailitem  Amy add
    /// <summary>
    /// TBLASNDETAILITEM
    /// </summary>
    [Serializable, TableMap("TBLASNDETAILITEM", "INVNO,STLINE,INVLINE,STNO")]
    public class Asndetailitem : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Asndetailitem()
        {
        }

        ///<summary>
        ///Stno
        ///</summary>
        [FieldMapAttribute("STNO", typeof(string), 40, false)]
        public string Stno;

        ///<summary>
        ///Stline
        ///</summary>
        [FieldMapAttribute("STLINE", typeof(string), 40, false)]
        public string Stline;

        ///<summary>
        ///Invno
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, false)]
        public string Invno;

        ///<summary>
        ///Invline
        ///</summary>
        [FieldMapAttribute("INVLINE", typeof(string), 40, false)]
        public string Invline;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DqmCode;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///ReceiveQty
        ///</summary>
        [FieldMapAttribute("RECEIVEQTY", typeof(decimal), 22, true)]
        public decimal ReceiveQty;

        /////<summary>
        /////RejectReceiveQTY
        /////</summary>
        //[FieldMapAttribute("RejectReceiveQTY", typeof(decimal), 22, true)]
        //public decimal RejectReceiveQTY;

        ///<summary>
        ///ActQty
        ///</summary>
        [FieldMapAttribute("ACTQTY", typeof(decimal), 22, true)]
        public decimal ActQty;

        ///<summary>
        ///QcpassQty
        ///</summary>
        [FieldMapAttribute("QCPASSQTY", typeof(decimal), 22, true)]
        public decimal QcpassQty;

        /////<summary>
        /////RejectQCPASSQTY
        /////</summary>
        //[FieldMapAttribute("RejectQCPASSQTY", typeof(decimal), 22, true)]
        //public decimal RejectQCPASSQTY;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }
    #endregion

    #region Asndetailsn  Amy add
    /// <summary>
    /// TBLASNDETAILSN
    /// </summary>
    [Serializable, TableMap("TBLASNDETAILSN", "SN,STNO")]
    public class Asndetailsn : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Asndetailsn()
        {
        }

        ///<summary>
        ///Stno
        ///</summary>
        [FieldMapAttribute("STNO", typeof(string), 40, false)]
        public string Stno;

        ///<summary>
        ///Stline
        ///</summary>
        [FieldMapAttribute("STLINE", typeof(string), 40, false)]
        public string Stline;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, true)]
        public string Cartonno;

        ///<summary>
        ///Sn
        ///</summary>
        [FieldMapAttribute("SN", typeof(string), 40, false)]
        public string Sn;

        ///<summary>
        ///SN IQC״̬(Y:�ϸ�N:���ϸ�)
        ///</summary>
        [FieldMapAttribute("QCSTATUS", typeof(string), 40, false)]
        public string QcStatus;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }
    #endregion

    #region Pickdetailmaterialsn   Amy add
    /// <summary>
    /// TBLPICKDETAILMATERIALSN
    /// </summary>
    [Serializable, TableMap("TBLPICKDETAILMATERIALSN", "PICKNO,SN")]
    public class Pickdetailmaterialsn : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Pickdetailmaterialsn()
        {
        }

        ///<summary>
        ///Pickno
        ///</summary>
        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string Pickno;

        ///<summary>
        ///Pickline
        ///</summary>
        [FieldMapAttribute("PICKLINE", typeof(string), 6, false)]
        public string Pickline;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string Cartonno;

        ///<summary>
        ///Sn
        ///</summary>
        [FieldMapAttribute("SN", typeof(string), 40, false)]
        public string Sn;

        ///<summary>
        ///QcStatus
        ///</summary>
        [FieldMapAttribute("QCSTATUS", typeof(string), 40, true)]
        public string QcStatus;

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

    }
    #endregion

    #region Pickdetailmaterial
    /// <summary>
    /// TBLPICKDETAILMATERIAL
    /// </summary>
    [Serializable, TableMap("TBLPICKDETAILMATERIAL", "PICKNO,CARTONNO")]
    public class Pickdetailmaterial : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Pickdetailmaterial()
        {
        }

        ///<summary>
        ///PQty
        ///</summary>
        [FieldMapAttribute("PQTY", typeof(decimal), 22, true)]
        public decimal PQty;

        ///<summary>
        ///Pickno
        ///</summary>
        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string Pickno;

        ///<summary>
        ///Pickline
        ///</summary>
        [FieldMapAttribute("PICKLINE", typeof(string), 6, false)]
        public string Pickline;

        ///<summary>
        ///CustmCode
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 40, true)]
        public string CustmCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DqmCode;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///LocationCode
        ///</summary>
        [FieldMapAttribute("LOCATIONCODE", typeof(string), 40, false)]
        public string LocationCode;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string Cartonno;

        ///<summary>
        ///Production_Date
        ///</summary>
        [FieldMapAttribute("PRODUCTION_DATE", typeof(int), 22, true)]
        public int Production_Date;

        ///<summary>
        ///Supplier_lotno
        ///</summary>
        [FieldMapAttribute("SUPPLIER_LOTNO", typeof(string), 40, true)]
        public string Supplier_lotno;

        ///<summary>
        ///Lotno
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string Lotno;

        ///<summary>
        ///StorageageDate
        ///</summary>
        [FieldMapAttribute("STORAGEAGEDATE", typeof(int), 22, true)]
        public int StorageageDate;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }
    #endregion
    #region Storagedetaillog // amy add @20160312
    /// <summary>
    /// TBLSTORAGEDETAILLOG
    /// </summary>
    [Serializable, TableMap("TBLSTORAGEDETAILLOG", "SERIAL")]
    public class Storagedetaillog : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Storagedetaillog()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Serialno
        ///</summary>
        [FieldMapAttribute("SERIALNO", typeof(string), 40, false)]
        public string Serialno;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

        ///<summary>
        ///LocationCode
        ///</summary>
        [FieldMapAttribute("LOCATIONCODE", typeof(string), 40, false)]
        public string LocationCode;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string Cartonno;

        ///<summary>
        ///Type
        ///</summary>
        [FieldMapAttribute("TYPE", typeof(string), 40, false)]
        public string Type;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DqmCode;

        ///<summary>
        ///MDesc
        ///</summary>
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string MDesc;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///StorageQty
        ///</summary>
        [FieldMapAttribute("STORAGEQTY", typeof(decimal), 22, false)]
        public decimal StorageQty;

        ///<summary>
        ///AvailableQty
        ///</summary>
        [FieldMapAttribute("AVAILABLEQTY", typeof(decimal), 22, false)]
        public decimal AvailableQty;

        ///<summary>
        ///FreezeQty
        ///</summary>
        [FieldMapAttribute("FREEZEQTY", typeof(decimal), 22, false)]
        public decimal FreezeQty;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, true)]
        public decimal Qty;

        ///<summary>
        ///Supplier_lotno
        ///</summary>
        [FieldMapAttribute("SUPPLIER_LOTNO", typeof(string), 40, true)]
        public string Supplier_lotno;

        ///<summary>
        ///Lotno
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string Lotno;

        ///<summary>
        ///Production_Date
        ///</summary>
        [FieldMapAttribute("PRODUCTION_DATE", typeof(int), 22, true)]
        public int Production_Date;

        ///<summary>
        ///StorageageDate
        ///</summary>
        [FieldMapAttribute("STORAGEAGEDATE", typeof(int), 22, true)]
        public int StorageageDate;

        ///<summary>
        ///LaststorageageDate
        ///</summary>
        [FieldMapAttribute("LASTSTORAGEAGEDATE", typeof(int), 22, true)]
        public int LaststorageageDate;

        ///<summary>
        ///ValidStartDate
        ///</summary>
        [FieldMapAttribute("VALIDSTARTDATE", typeof(int), 22, true)]
        public int ValidStartDate;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FacCode;

        ///<summary>
        ///ReworkapplyUser
        ///</summary>
        [FieldMapAttribute("REWORKAPPLYUSER", typeof(string), 40, true)]
        public string ReworkapplyUser;

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

    }
    #endregion

    #region

    public class Asndetailexp : Asndetail
    {
        public Asndetailexp()
        {
        }

        ///<summary>
        ///LocationCode
        ///</summary>
        [FieldMapAttribute("LocationCode", typeof(string), 40, false)]
        public string LocationCode;
        ///<summary>
        ///ReLocationCode   --�Ƽ���λ
        ///</summary>
        [FieldMapAttribute("ReLocationCode", typeof(string), 40, false)]
        public string ReLocationCode;
        ///<summary>
        ///mcontroltype   --�ܿ�����
        ///</summary>
        [FieldMapAttribute("MControlType", typeof(string), 40, false)]
        public string MControlType;
    }


    #endregion
    #region Invinouttrans
    /// <summary>
    /// TBLINVINOUTTRANS
    /// </summary>
    [Serializable, TableMap("TBLINVINOUTTRANS", "SERIAL")]
    public class InvInOutTrans : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public InvInOutTrans()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Transno
        ///</summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 40, false)]
        public string TransNO;

        ///<summary>
        ///TransType
        ///</summary>
        [FieldMapAttribute("TRANSTYPE", typeof(string), 40, false)]
        public string TransType;

        ///<summary>
        ///InvType
        ///</summary>
        [FieldMapAttribute("INVTYPE", typeof(string), 40, false)]
        public string InvType;

        ///<summary>
        ///Invno
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, true)]
        public string InvNO;

        ///<summary>
        ///ProcessType
        ///</summary>
        [FieldMapAttribute("ProcessType", typeof(string), 40, true)]
        public string ProcessType;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, true)]
        public string CartonNO;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCode;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, true)]
        public string DqMCode;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, true)]
        public decimal Qty;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FacCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///FromfacCode
        ///</summary>
        [FieldMapAttribute("FROMFACCODE", typeof(string), 40, true)]
        public string FromFacCode;

        ///<summary>
        ///FromstorageCode
        ///</summary>
        [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 40, true)]
        public string FromStorageCode;

        ///<summary>
        ///Production_Date
        ///</summary>
        [FieldMapAttribute("PRODUCTION_DATE", typeof(int), 22, true)]
        public int ProductionDate;

        ///<summary>
        ///Supplier_lotno
        ///</summary>
        [FieldMapAttribute("SUPPLIER_LOTNO", typeof(string), 40, true)]
        public string SupplierLotNo;

        ///<summary>
        ///Lotno
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, true)]
        public string LotNo;

        ///<summary>
        ///StorageageDate
        ///</summary>
        [FieldMapAttribute("STORAGEAGEDATE", typeof(int), 22, true)]
        public int StorageAgeDate;

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

    }
    #endregion

    #region Pickdetail  Amy Add by @20160224
    /// <summary>
    /// TBLPICKDETAIL
    /// </summary>
    [Serializable, TableMap("TBLPICKDETAIL", "PICKNO,PICKLINE")]
    public class PickDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public PickDetail()
        {
        }

        [FieldMapAttribute("DETAILREMARK", typeof(string), 880, true)]
        public string Detailremark;

        ///<summary>
        ///Invline
        ///</summary>
        [FieldMapAttribute("INVLINE", typeof(int), 22, true)]
        public int InvLine;

        ///<summary>
        ///Pickno
        ///</summary>
        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string PickNo;

        ///<summary>
        ///Pickline
        ///</summary>
        [FieldMapAttribute("PICKLINE", typeof(string), 6, false)]
        public string PickLine;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///CustmCode
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 40, true)]
        public string CustMCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCode;

        ///<summary>
        ///MDesc
        ///</summary>
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string MDesc;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal QTY;

        ///<summary>
        ///OweQty
        ///</summary>
        [FieldMapAttribute("OWEQTY", typeof(decimal), 22, true)]
        public decimal OweQTY;

        ///<summary>
        ///SQty
        ///</summary>
        [FieldMapAttribute("SQTY", typeof(decimal), 22, true)]
        public decimal SQTY;

        ///<summary>
        ///PQty
        ///</summary>
        [FieldMapAttribute("PQTY", typeof(decimal), 22, true)]
        public decimal PQTY;

        ///<summary>
        ///OutQty
        ///</summary>
        [FieldMapAttribute("OUTQTY", typeof(decimal), 22, true)]
        public decimal OutQTY;

        ///<summary>
        ///QcpassQty
        ///</summary>
        [FieldMapAttribute("QCPASSQTY", typeof(decimal), 22, true)]
        public decimal QCPassQTY;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Cusorderno
        ///</summary>
        [FieldMapAttribute("CUSORDERNO", typeof(string), 10, true)]
        public string CusOrderNo;

        ///<summary>
        ///CusordernoType
        ///</summary>
        [FieldMapAttribute("CUSORDERNOTYPE", typeof(string), 2, true)]
        public string CusOrderNoType;

        ///<summary>
        ///CusitemCode
        ///</summary>
        [FieldMapAttribute("CUSITEMCODE", typeof(string), 40, true)]
        public string CusItemCode;

        ///<summary>
        ///Cusitemspec
        ///</summary>
        [FieldMapAttribute("CUSITEMSPEC", typeof(string), 40, true)]
        public string CusItemSpec;

        ///<summary>
        ///CusitemDesc
        ///</summary>
        [FieldMapAttribute("CUSITEMDESC", typeof(string), 40, true)]
        public string CusItemDesc;

        ///<summary>
        ///VEnderitemCode
        ///</summary>
        [FieldMapAttribute("VENDERITEMCODE", typeof(string), 40, true)]
        public string VEnderItemCode;

        ///<summary>
        ///Gfcontractno
        ///</summary>
        [FieldMapAttribute("GFCONTRACTNO", typeof(string), 40, true)]
        public string GFContractNo;

        ///<summary>
        ///GfhwitemCode
        ///</summary>
        [FieldMapAttribute("GFHWITEMCODE", typeof(string), 40, true)]
        public string GFHWItemCode;

        ///<summary>
        ///Gfpackingseq
        ///</summary>
        [FieldMapAttribute("GFPACKINGSEQ", typeof(string), 40, true)]
        public string GFPackingSeq;

        ///<summary>
        ///Postway
        ///</summary>
        [FieldMapAttribute("POSTWAY", typeof(string), 40, true)]
        public string PostWay;

        ///<summary>
        ///Pickcondition
        ///</summary>
        [FieldMapAttribute("PICKCONDITION", typeof(string), 40, true)]
        public string PickCondition;

        ///<summary>
        ///HwCodeQty
        ///</summary>
        [FieldMapAttribute("HWCODEQTY", typeof(string), 40, true)]
        public string HWCodeQTY;

        ///<summary>
        ///HwTypeinfo
        ///</summary>
        [FieldMapAttribute("HWTYPEINFO", typeof(string), 40, true)]
        public string HWTypeInfo;

        ///<summary>
        ///Packingway
        ///</summary>
        [FieldMapAttribute("PACKINGWAY", typeof(string), 200, true)]
        public string PackingWay;

        ///<summary>
        ///Packingno
        ///</summary>
        [FieldMapAttribute("PACKINGNO", typeof(string), 20, true)]
        public string PackingNo;

        ///<summary>
        ///Packingspec
        ///</summary>
        [FieldMapAttribute("PACKINGSPEC", typeof(string), 50, true)]
        public string PackingSpec;

        ///<summary>
        ///Packingwayno
        ///</summary>
        [FieldMapAttribute("PACKINGWAYNO", typeof(string), 220, true)]
        public string PackingWayNo;

        ///<summary>
        ///DqsitemCode
        ///</summary>
        [FieldMapAttribute("DQSITEMCODE", typeof(string), 40, true)]
        public string DQSItemCode;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }
    #endregion

    #region  Amy Add by @20160304

    public class PickDetailEx : PickDetail
    {
        public PickDetailEx()
        {
        }

        ///<summary>
        ///sumBox  ����
        ///</summary>
        [FieldMapAttribute("sumBox", typeof(string), 40, false)]
        public string sumBox;

        ///<summary>
        ///sumBox  ����
        ///</summary>
        [FieldMapAttribute("Invno", typeof(string), 40, false)]
        public string InvNo;

        [FieldMapAttribute("SAPCOUNT", typeof(int), 40, false)]
        public int SapCount;


    }
    public class PickDetailMaterialSNEx : Pickdetailmaterialsn
    {
        public PickDetailMaterialSNEx()
        {
        }

        ///<summary>
        ///DQMCODE  �������Ϻ�
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCODE;
        ///<summary>
        ///CusMCode  ��Ϊ���Ϻ�
        ///</summary>
        [FieldMapAttribute("CusMCode", typeof(string), 40, false)]
        public string CusMCode;
    }

    #endregion

    #region Serialbook
    /// <summary>
    /// amy x lv  OQC�����ͼ���
    /// </summary>
    [Serializable, TableMap("TBLSERIALBOOK", "SNPREFIX")]
    public class Serialbook : DomainObject
    {
        public Serialbook()
        {
        }

        [FieldMapAttribute("SNPREFIX", typeof(string), 40, false)]
        public string SNprefix;

        [FieldMapAttribute("MAXSERIAL", typeof(string), 40, false)]
        public string MAXSerial;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUser;

        [FieldMapAttribute("MDATE", typeof(int), 10, false)]
        public int MDate;

        [FieldMapAttribute("MTIME", typeof(int), 10, false)]
        public int MTime;

        [FieldMapAttribute("SERIALTYPE", typeof(string), 40, false)]
        public int SerialType;
    }
    #endregion

    #region Barcode add by sam 2016��2��26��
    /// <summary>
    /// TBLBARCODE
    /// </summary>
    [Serializable, TableMap("TBLBARCODE", "BARCODE")]
    public class BarCode : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public BarCode()
        {
        }

        ///<summary>
        ///Barcode
        ///</summary>
        [FieldMapAttribute("BARCODE", typeof(string), 50, false)]
        public string BarCodeNo;

        ///<summary>
        ///Type
        ///</summary>
        [FieldMapAttribute("TYPE", typeof(string), 40, false)]
        public string Type;

        ///<summary>
        ///Mcode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCode;

        ///<summary>
        ///Encode
        ///</summary>
        [FieldMapAttribute("ENCODE", typeof(string), 40, true)]
        public string EnCode;

        ///<summary>
        ///Spanyear
        ///</summary>
        [FieldMapAttribute("SPANYEAR", typeof(string), 22, false)]
        public string SpanYear;

        ///<summary>
        ///Spandate
        ///</summary>
        [FieldMapAttribute("SPANDATE", typeof(int), 22, false)]
        public int SpanDate;

        ///<summary>
        ///Serialno
        ///</summary>
        [FieldMapAttribute("SERIALNO", typeof(int), 22, false)]
        public int SerialNo;

        ///<summary>
        ///Printtimes
        ///</summary>
        [FieldMapAttribute("PRINTTIMES", typeof(int), 22, true)]
        public int PrintTimes;

        ///<summary>
        ///Cuser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///Cdate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///Ctime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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

    }
    #endregion

    #region add by sam 2016��3��7��
    public class PickDetailQuery : PickDetail
    {
        public PickDetailQuery()
        {
        }

        ///<summary>
        ///
        ///</summary>
        [FieldMapAttribute("OrderNo", typeof(string), 40, false)]
        public string OrderNo;

        ///<summary>
        ///�����λ
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, false)]
        public string StorageCode;

    }

    public class PickdetailmaterialQuery : Pickdetailmaterial
    {
        public PickdetailmaterialQuery()
        {
        }



        ///<summary>
        ///CusitemCode
        ///</summary>
        [FieldMapAttribute("CUSITEMCODE", typeof(string), 40, true)]
        public string CusItemCode;

        ///<summary>
        ///GfhwitemCode
        ///</summary>
        [FieldMapAttribute("GFHWITEMCODE", typeof(string), 40, true)]
        public string GfhwItemCode;

        ///<summary>
        ///SN
        ///</summary>
        [FieldMapAttribute("SN", typeof(string), 40, true)]
        public string Sn;


    }

    #endregion

    #region Picktopo add by sam ������ϵ
    /// <summary>
    /// TBLPICKTOPO
    /// </summary>
    [Serializable, TableMap("TBLPICKTOPO", "PICKNO,PICKLINE")]
    public class PickToPo : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public PickToPo()
        {
        }

        ///<summary>
        ///Pono
        ///</summary>
        [FieldMapAttribute("PONO", typeof(string), 40, false)]
        public string PoNo;

        ///<summary>
        ///Poline
        ///</summary>
        [FieldMapAttribute("POLINE", typeof(string), 6, false)]
        public string PoLine;

        ///<summary>
        ///Pickno
        ///</summary>
        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string PickNo;

        ///<summary>
        ///Pickline
        ///</summary>
        [FieldMapAttribute("PICKLINE", typeof(string), 6, false)]
        public string PickLine;

        ///<summary>
        ///Mcode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCode;

        ///<summary>
        ///Dqmcode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, true)]
        public string DQMCode;

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
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }
    #endregion



    #region Storloctrans
    /// <summary>
    /// TBLSTORLOCTRANS
    /// </summary>
    [Serializable, TableMap("TBLSTORLOCTRANS", "TRANSNO")]
    public class Storloctrans : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Storloctrans()
        {
        }

        ///<summary>
        ///Transno
        ///</summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 40, false)]
        public string Transno;

        ///<summary>
        ///TransType
        ///</summary>
        [FieldMapAttribute("TRANSTYPE", typeof(string), 40, false)]
        public string TransType;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///Invno
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, false)]
        public string Invno;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///FromstorageCode
        ///</summary>
        [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 40, true)]
        public string FromstorageCode;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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

    }
    #endregion

    #region StorloctransDetail
    /// <summary>
    /// TBLSTORLOCTRANSDETAIL
    /// </summary>
    [Serializable, TableMap("TBLSTORLOCTRANSDETAIL", "TRANSNO,MCODE")]
    public class StorloctransDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public StorloctransDetail()
        {
        }

        ///<summary>
        ///Transno
        ///</summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 40, false)]
        public string Transno;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string Status;

        ///<summary>
        ///CustmCode
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 40, true)]
        public string CustmCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DqmCode;

        ///<summary>
        ///MDesc
        ///</summary>
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string MDesc;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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

    }

    public class StorloctransDetailEX : StorloctransDetail
    {
        ///<summary>
        ///TRANSQTY
        ///</summary>
        [FieldMapAttribute("TRANSQTY", typeof(decimal), 22, false)]
        public decimal TransQty;

        ///<summary>
        ///Sn
        ///</summary>
        [FieldMapAttribute("SN", typeof(string), 40, false)]
        public string Sn;
    }
    #endregion

    #region StorloctransDetailCarton
    /// <summary>
    /// TBLSTORLOCTRANSDETAILCARTON
    /// </summary>
    [Serializable, TableMap("TBLSTORLOCTRANSDETAILCARTON", "TRANSNO,FROMCARTONNO,CARTONNO")]
    public class StorloctransDetailCarton : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public StorloctransDetailCarton()
        {
        }

        ///<summary>
        ///Transno
        ///</summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 40, false)]
        public string Transno;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///DqmCode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DqmCode;

        ///<summary>
        ///MDesc
        ///</summary>
        [FieldMapAttribute("MDESC", typeof(string), 200, true)]
        public string MDesc;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Supplier_lotno
        ///</summary>
        [FieldMapAttribute("SUPPLIER_LOTNO", typeof(string), 40, true)]
        public string Supplier_lotno;

        ///<summary>
        ///Lotno
        ///</summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, false)]
        public string Lotno;

        ///<summary>
        ///Production_Date
        ///</summary>
        [FieldMapAttribute("PRODUCTION_DATE", typeof(int), 22, true)]
        public int Production_Date;

        ///<summary>
        ///StorageageDate
        ///</summary>
        [FieldMapAttribute("STORAGEAGEDATE", typeof(int), 22, true)]
        public int StorageageDate;

        ///<summary>
        ///LaststorageageDate
        ///</summary>
        [FieldMapAttribute("LASTSTORAGEAGEDATE", typeof(int), 22, true)]
        public int LaststorageageDate;

        ///<summary>
        ///ValidStartDate
        ///</summary>
        [FieldMapAttribute("VALIDSTARTDATE", typeof(int), 22, true)]
        public int ValidStartDate;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FacCode;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///LocationCode
        ///</summary>
        [FieldMapAttribute("LOCATIONCODE", typeof(string), 40, false)]
        public string LocationCode;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string Cartonno;

        ///<summary>
        ///FromlocationCode
        ///</summary>
        [FieldMapAttribute("FROMLOCATIONCODE", typeof(string), 40, false)]
        public string FromlocationCode;

        ///<summary>
        ///Fromcartonno
        ///</summary>
        [FieldMapAttribute("FROMCARTONNO", typeof(string), 40, false)]
        public string Fromcartonno;

        ///<summary>
        ///CUser
        ///</summary>
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUser;

        ///<summary>
        ///CDate
        ///</summary>
        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDate;

        ///<summary>
        ///CTime
        ///</summary>
        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTime;

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

    }
    #endregion





    #region ת����ҵ add by Chris.H.Wang @20160315
    [Serializable]
    public class StorLocTransOperations : StorloctransDetailCarton
    {
        ///<summary>
        ///HasReMoveQty
        ///</summary>
        [FieldMapAttribute("HasReMoveQty", typeof(decimal), 22, false)]
        public decimal HasReMoveQty;

        ///<summary>
        ///FromstorageCode
        ///</summary>
        [FieldMapAttribute("FROMSTORAGECODE", typeof(string), 40, true)]
        public string FromstorageCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("INVNO", typeof(string), 40, true)]
        public string InvNo;

    }
    #endregion

    #region StorloctransDetailSN
    /// <summary>
    /// TBLSTORLOCTRANSDETAILSN
    /// </summary>
    [Serializable, TableMap("TBLSTORLOCTRANSDETAILSN", "TRANSNO,SN")]
    public class StorloctransDetailSN : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public StorloctransDetailSN()
        {
        }

        ///<summary>
        ///Transno
        ///</summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 40, false)]
        public string Transno;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string Cartonno;

        ///<summary>
        ///Fromcartonno
        ///</summary>
        [FieldMapAttribute("FROMCARTONNO", typeof(string), 40, false)]
        public string Fromcartonno;

        ///<summary>
        ///Sn
        ///</summary>
        [FieldMapAttribute("SN", typeof(string), 40, false)]
        public string Sn;

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

    }
    #endregion

    //[Serializable, TableMap("TBLOQC", "OQCNO")]
    //public class OQC : BenQGuru.eMES.Common.Domain.DomainObject
    //{
    //    [FieldMapAttribute("OQCNO", typeof(string), 50, false)]
    //    public string OQCNO;
    //    [FieldMapAttribute("OQCTYPE", typeof(string), 40, false)]
    //    public string OQCTYPE;
    //    [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
    //    public string PICKNO;
    //    [FieldMapAttribute("CARINVNO", typeof(string), 40, false)]
    //    public string CARINVNO;
    //    [FieldMapAttribute("STATUS", typeof(string), 40, false)]
    //    public string STATUS;
    //    [FieldMapAttribute("APPDATE", typeof(int), 40, false)]
    //    public int APPDATE;
    //    [FieldMapAttribute("APPTIME", typeof(int), 40, false)]
    //    public int APPTIME;
    //    [FieldMapAttribute("INSPDATE", typeof(int), 40, false)]
    //    public int INSPDATE;
    //    [FieldMapAttribute("INSPTIME", typeof(int), 40, false)]
    //    public int INSPTIME;
    //    [FieldMapAttribute("QCSTATUS", typeof(string), 40, false)]
    //    public string QCSTATUS;
    //    [FieldMapAttribute("CUSER", typeof(string), 40, false)]
    //    public string CUSER;



    //    [FieldMapAttribute("CDATE", typeof(int), 40, false)]
    //    public int CDATE;

    //    [FieldMapAttribute("CTIME", typeof(int), 40, false)]
    //    public int CTIME;
    //    [FieldMapAttribute("MDATE", typeof(string), 40, false)]
    //    public int MDATE;
    //    [FieldMapAttribute("MTIME", typeof(string), 40, false)]
    //    public int MTIME;

    //    [FieldMapAttribute("MUSER", typeof(string), 40, false)]
    //    public string MUSER;
    //}


    #region PickDetailMaterialSn
    /// <summary>
    /// TBLPICKDETAILMATERIALSN
    /// </summary>
    [Serializable, TableMap("TBLPICKDETAILMATERIALSN", "PICKNO,SN")]
    public class PickDetailMaterialSn : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public PickDetailMaterialSn()
        {
        }

        ///<summary>
        ///Pickno
        ///</summary>
        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string PickNo;

        ///<summary>
        ///Pickline
        ///</summary>
        [FieldMapAttribute("PICKLINE", typeof(string), 6, false)]
        public string PickLine;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string CartonNo;

        ///<summary>
        ///Sn
        ///</summary>
        [FieldMapAttribute("SN", typeof(string), 40, false)]
        public string Sn;

        ///<summary>
        ///Qcstatus
        ///</summary>
        [FieldMapAttribute("QCSTATUS", typeof(string), 40, true)]
        public string QcStatus;

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

    }
    #endregion



    [Serializable, TableMap("TBLCARTONINVOICES", "CARINVNO")]
    public class CARTONINVOICES : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public CARTONINVOICES()
        {
        }

        ///<summary>
        ///Transno
        ///</summary>
        [FieldMapAttribute("CARINVNO", typeof(string), 40, false)]
        public string CARINVNO;

        ///<summary>
        ///Cartonno
        ///</summary>
        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string PICKNO;

        ///<summary>
        ///Fromcartonno
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string STATUS;

        ///<summary>
        ///Sn
        ///</summary>
        [FieldMapAttribute("SO", typeof(string), 40, false)]
        public string SO;

        ///<summary>
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("PONO", typeof(string), 40, false)]
        public string PONO;

        ///<summary>
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("PROJECTNAME", typeof(string), 40, false)]
        public string PROJECTNAME;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("FDATE", typeof(int), 22, false)]
        public int FDATE;


        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("FTIME", typeof(int), 22, false)]
        public int FTIME;


        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("ODATE", typeof(int), 22, false)]
        public int ODATE;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("OTIME", typeof(int), 22, false)]
        public int OTIME;


        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("SHIPPING_MARK_USER", typeof(string), 22, false)]
        public string SHIPPING_MARK_USER;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("SHIPPING_MARK_DATE", typeof(int), 22, false)]
        public int SHIPPING_MARK_DATE;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("SHIPPING_MARK_TIME", typeof(int), 22, false)]
        public int SHIPPING_MARK_TIME;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("GROSS_WEIGHT", typeof(double), 22, false)]
        public double GROSS_WEIGHT;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("VOLUME", typeof(string), 40, false)]
        public string VOLUME;


        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUSER;


        [FieldMapAttribute("CDATE", typeof(int), 22, false)]
        public int CDATE;

        [FieldMapAttribute("CTIME", typeof(int), 22, false)]
        public int CTIME;

        [FieldMapAttribute("MDATE", typeof(int), 22, false)]
        public int MDATE;

        [FieldMapAttribute("MTIME", typeof(int), 22, false)]
        public int MTIME;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;
    }


    [Serializable, TableMap("TBLCARTONINVDETAILSN", "CARINVNO, SN")]
    public class CARTONINVDETAILSN : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public CARTONINVDETAILSN()
        {
        }


        [FieldMapAttribute("CARINVNO", typeof(string), 40, false)]
        public string CARINVNO;


        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string PICKNO;

        [FieldMapAttribute("PICKLINE", typeof(string), 40, false)]
        public string PICKLINE;


        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string CARTONNO;


        [FieldMapAttribute("SN", typeof(string), 40, false)]
        public string SN;


        [FieldMapAttribute("MTIME", typeof(int), 40, false)]
        public int MTIME;
        [FieldMapAttribute("MDATE", typeof(int), 40, false)]
        public int MDATE;
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;
    }




    [Serializable, TableMap("TBLCARTONINVDETAIL", "CARINVNO,CARTONNO")]
    public class CartonInvDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        [FieldMapAttribute("CARINVNO", typeof(string), 40, false)]
        public string CARINVNO;

        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string PICKNO;

        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string STATUS;

        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string CARTONNO;

        [FieldMapAttribute("PACKMCODE", typeof(string), 40, false)]
        public string PACKMCODE;

        [FieldMapAttribute("PACKQTY", typeof(double), 40, false)]
        public double PACKQTY;

        [FieldMapAttribute("QCSTATUS", typeof(string), 40, false)]
        public string QCSTATUS;

        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUSER;

        [FieldMapAttribute("CDATE", typeof(int), 40, false)]
        public int CDATE;

        [FieldMapAttribute("CTIME", typeof(int), 40, false)]
        public int CTIME;

        [FieldMapAttribute("MDATE", typeof(int), 40, false)]
        public int MDATE;


        [FieldMapAttribute("MTIME", typeof(int), 40, false)]
        public int MTIME;


        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;
    }

    [Serializable, TableMap("TBLCartonInvDetailMaterial", "CARINVNO, CARTONNO, DQMCODE")]
    public class CartonInvDetailMaterial : BenQGuru.eMES.Common.Domain.DomainObject
    {
        [FieldMapAttribute("CARINVNO", typeof(string), 40, false)]
        public string CARINVNO;

        [FieldMapAttribute("PICKNO", typeof(string), 40, false)]
        public string PICKNO;
        [FieldMapAttribute("PICKLINE", typeof(string), 40, false)]
        public string PICKLINE;
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string CARTONNO;
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCODE;
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCODE;
        [FieldMapAttribute("DQSMCODE", typeof(string), 40, false)]
        public string DQSMCODE;
        [FieldMapAttribute("QTY", typeof(decimal), 40, false)]
        public decimal QTY;
        [FieldMapAttribute("UNIT", typeof(string), 40, false)]
        public string UNIT;
        [FieldMapAttribute("GFHWITEMCODE", typeof(string), 40, false)]
        public string GFHWITEMCODE;
        [FieldMapAttribute("GFPACKINGSEQ", typeof(string), 40, false)]
        public string GFPACKINGSEQ;
        [FieldMapAttribute("GFFLAG", typeof(string), 40, false)]
        public string GFFLAG;
        [FieldMapAttribute("QCSTATUS", typeof(string), 40, false)]
        public string QCSTATUS;
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUSER;
        [FieldMapAttribute("CDATE", typeof(int), 40, false)]
        public int CDATE;
        [FieldMapAttribute("CTIME", typeof(int), 40, false)]
        public int CTIME;
        [FieldMapAttribute("MDATE", typeof(string), 40, false)]
        public int MDATE;
        [FieldMapAttribute("MTIME", typeof(string), 40, false)]
        public int MTIME;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;
    }





    [Serializable, TableMap("TBLStockCheck", "CheckNo")]
    public class StockCheck : BenQGuru.eMES.Common.Domain.DomainObject
    {
        [FieldMapAttribute("CheckNo", typeof(string), 50, false)]
        public string CheckNo;
        [FieldMapAttribute("CheckType", typeof(string), 40, false)]
        public string CheckType;
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FACCODE;
        [FieldMapAttribute("StorageCode", typeof(string), 40, false)]
        public string StorageCode;

        [FieldMapAttribute("REMARK1", typeof(string), 200, false)]
        public string REMARK1;
        [FieldMapAttribute("STATUS", typeof(string), 40, false)]
        public string STATUS;


        [FieldMapAttribute("SDATE", typeof(int), 40, false)]
        public int SDATE;
        [FieldMapAttribute("EDATE", typeof(int), 40, false)]
        public int EDATE;
        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUSER;
        [FieldMapAttribute("CDATE", typeof(int), 40, false)]
        public int CDATE;

        [FieldMapAttribute("CTIME", typeof(int), 40, false)]
        public int CTIME;

        [FieldMapAttribute("MDATE", typeof(string), 40, false)]
        public int MDATE;
        [FieldMapAttribute("MTIME", typeof(string), 40, false)]
        public int MTIME;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;
    }

    [Serializable, TableMap("TBLStockCheckDetail", "CHECKNO, DQMCODE, STORAGECODE, LOCATIONCODE, CARTONNO")]
    public class StockCheckDetail : BenQGuru.eMES.Common.Domain.DomainObject
    {
        [FieldMapAttribute("CheckNo", typeof(string), 50, false)]
        public string CheckNo;
        [FieldMapAttribute("StorageCode", typeof(string), 40, false)]
        public string StorageCode;
        [FieldMapAttribute("LocationCode", typeof(string), 40, false)]
        public string LocationCode;
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string CARTONNO;

        [FieldMapAttribute("DQMCODE", typeof(string), 200, false)]
        public string DQMCODE;
        [FieldMapAttribute("MDESC", typeof(string), 200, false)]
        public string MDESC;
        [FieldMapAttribute("UNIT", typeof(string), 200, false)]
        public string UNIT;

        [FieldMapAttribute("STORAGEQTY", typeof(decimal), 40, false)]
        public decimal STORAGEQTY;
        [FieldMapAttribute("CheckQty", typeof(int), 40, false)]
        public decimal CheckQty;

        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUSER;
        [FieldMapAttribute("CDATE", typeof(int), 40, false)]
        public int CDATE;

        [FieldMapAttribute("CTIME", typeof(int), 40, false)]
        public int CTIME;

        [FieldMapAttribute("MDATE", typeof(string), 40, false)]
        public int MDATE;
        [FieldMapAttribute("MTIME", typeof(string), 40, false)]
        public int MTIME;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;


        [FieldMapAttribute("DIFFDESC", typeof(string), 40, false)]
        public string DIFFDESC;
    }

    [Serializable, TableMap("TBLStockCheckDetailCarton", "CHECKNO, CARTONNO, DQMCODE, STORAGECODE, LOCATIONCODE")]
    public class StockCheckDetailCarton : BenQGuru.eMES.Common.Domain.DomainObject
    {
        [FieldMapAttribute("CheckNo", typeof(string), 50, false)]
        public string CheckNo;
        [FieldMapAttribute("StorageCode", typeof(string), 40, false)]
        public string StorageCode;
        [FieldMapAttribute("LocationCode", typeof(string), 40, false)]
        public string LocationCode;
        [FieldMapAttribute("CARTONNO", typeof(string), 40, false)]
        public string CARTONNO;
        [FieldMapAttribute("DQMCODE", typeof(string), 40, false)]
        public string DQMCODE;
        [FieldMapAttribute("STORAGEQTY", typeof(decimal), 200, false)]
        public decimal STORAGEQTY;

        //add by sam
        [FieldMapAttribute("SLocationCode", typeof(string), 40, false)]
        public string SLocationCode;
        [FieldMapAttribute("SCARTONNO", typeof(string), 40, false)]
        public string SCARTONNO;

        [FieldMapAttribute("DIFFDESC", typeof(string), 200, false)]
        public string DiffDesc;

        [FieldMapAttribute("CheckQty", typeof(decimal), 40, false)]
        public decimal CheckQty;

        [FieldMapAttribute("CUSER", typeof(string), 40, false)]
        public string CUSER;
        [FieldMapAttribute("CDATE", typeof(int), 40, false)]
        public int CDATE;

        [FieldMapAttribute("CTIME", typeof(int), 40, false)]
        public int CTIME;

        [FieldMapAttribute("MDATE", typeof(string), 40, false)]
        public int MDATE;
        [FieldMapAttribute("MTIME", typeof(string), 40, false)]
        public int MTIME;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;
    }

    #region  Amy Ǩ�� @20160312

    public class PickInfo : DomainObject
    {
        [FieldMapAttribute("CARINVNO", typeof(string), 40, true)]
        public string CARINVNO;

        [FieldMapAttribute("PICKNO", typeof(string), 40, true)]
        public string PICKNO;

        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string STATUS;

        [FieldMapAttribute("PICKTYPE", typeof(string), 40, true)]
        public string PICKTYPE;

        [FieldMapAttribute("INVNO", typeof(string), 40, true)]
        public string INVNO;

        [FieldMapAttribute("ORDERNO", typeof(string), 40, true)]
        public string ORDERNO;

        [FieldMapAttribute("StorageCode", typeof(string), 40, true)]
        public string StorageCode;
        [FieldMapAttribute("ReceiverUser", typeof(string), 40, true)]
        public string ReceiverUser;

        [FieldMapAttribute("ReceiverAddr", typeof(string), 40, true)]
        public string ReceiverAddr;

        [FieldMapAttribute("Plan_Date", typeof(int), 22, true)]
        public int Plan_Date;

        [FieldMapAttribute("PLANGIDATE", typeof(string), 40, true)]
        public string PLANGIDATE;
        [FieldMapAttribute("GFCONTRACTNO", typeof(string), 40, true)]
        public string GFCONTRACTNO;

        [FieldMapAttribute("GFFLAG", typeof(string), 40, true)]
        public string GFFLAG;


        [FieldMapAttribute("OANO", typeof(string), 40, true)]
        public string OANO;

        [FieldMapAttribute("Packing_List_Date", typeof(int), 22, true)]
        public int Packing_List_Date;


        [FieldMapAttribute("Packing_List_Time", typeof(int), 22, true)]
        public int Packing_List_Time;

        [FieldMapAttribute("Shipping_Mark_Date", typeof(int), 22, true)]
        public int Shipping_Mark_Date;


        [FieldMapAttribute("Shipping_Mark_Time", typeof(int), 22, true)]
        public int Shipping_Mark_Time;
        [FieldMapAttribute("GROSS_WEIGHT", typeof(double), 32, true)]
        public double GROSS_WEIGHT;
        [FieldMapAttribute("VOLUME", typeof(string), 40, true)]
        public string VOLUME;
        [FieldMapAttribute("CARTONNO", typeof(string), 40, true)]
        public string CARTONNO;
        [FieldMapAttribute("GFHWITEMCODE", typeof(string), 40, true)]
        public string GFHWITEMCODE;
        [FieldMapAttribute("GFPACKINGSEQ", typeof(string), 40, true)]
        public string GFPACKINGSEQ;
        [FieldMapAttribute("PICKLINE", typeof(string), 40, true)]
        public string PICKLINE;

        [FieldMapAttribute("SN", typeof(string), 40, true)]
        public string SN;

        [FieldMapAttribute("P22", typeof(string), 40, true)]
        public string P22;
    }
    #endregion
    #region Log

    #region Rslog
    /// <summary>
    /// TBLRSLOG
    /// </summary>
    [Serializable, TableMap("TBLRSLOG", "SERIAL")]
    public class Rslog : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Rslog()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///DocumentDate
        ///</summary>
        [FieldMapAttribute("DOCUMENTDATE", typeof(int), 22, false)]
        public int DocumentDate;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FacCode;

        ///<summary>
        ///Inoutflag
        ///</summary>
        [FieldMapAttribute("INOUTFLAG", typeof(string), 40, false)]
        public string Inoutflag;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCode;

        ///<summary>
        ///Mestransno
        ///</summary>
        [FieldMapAttribute("MESTRANSNO", typeof(string), 40, false)]
        public string Mestransno;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///Rsline
        ///</summary>
        [FieldMapAttribute("RSLINE", typeof(int), 22, false)]
        public int Rsline;

        ///<summary>
        ///Rsno
        ///</summary>
        [FieldMapAttribute("RSNO", typeof(string), 40, false)]
        public string Rsno;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string Sapreturn;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string Sapmaterialinvoice;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 40, true)]
        public string Message;

        ///<summary>
        ///tblrslog
        ///</summary>
        [FieldMapAttribute("IsPBack", typeof(string), 40, true)]
        public string IsPBack;

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

    }
    #endregion

    #region Dnlog_in
    /// <summary>
    /// TBLDNLOG_IN
    /// </summary>
    [Serializable, TableMap("TBLDNLOG_IN", "SERIAL")]
    public class Dnlog_in : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Dnlog_in()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Batchno
        ///</summary>
        [FieldMapAttribute("BATCHNO", typeof(string), 50, false)]
        public string Batchno;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///Rsline
        ///</summary>
        [FieldMapAttribute("DNLINE", typeof(int), 22, false)]
        public int DNline;

        ///<summary>
        ///Rsno
        ///</summary>
        [FieldMapAttribute("DNNO", typeof(string), 40, false)]
        public string DNno;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string Sapmaterialinvoice;

        ///<summary>
        ///Ispback
        ///</summary>
        [FieldMapAttribute("ISPBACK", typeof(string), 40, true)]
        public string Ispback;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string Sapreturn;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 200, true)]
        public string Message;

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

    }
    #endregion

    #region Wwpolog
    /// <summary>
    /// TBLWWPOLOG
    /// </summary>
    [Serializable, TableMap("TBLWWPOLOG", "SERIAL")]
    public class Wwpolog : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Wwpolog()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Pono
        ///</summary>
        [FieldMapAttribute("PONO", typeof(string), 40, false)]
        public string Pono;

        ///<summary>
        ///Poline
        ///</summary>
        [FieldMapAttribute("POLINE", typeof(int), 22, false)]
        public int Poline;

        ///<summary>
        ///VEndorCode
        ///</summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, false)]
        public string VEndorCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///Inoutflag
        ///</summary>
        [FieldMapAttribute("INOUTFLAG", typeof(string), 40, true)]
        public string Inoutflag;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, true)]
        public decimal Qty;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FacCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///Mestransno
        ///</summary>
        [FieldMapAttribute("MESTRANSNO", typeof(string), 40, true)]
        public string Mestransno;

        ///<summary>
        ///MestransDate
        ///</summary>
        [FieldMapAttribute("MESTRANSDATE", typeof(int), 22, true)]
        public int MestransDate;

        ///<summary>
        ///Remark
        ///</summary>
        [FieldMapAttribute("REMARK", typeof(string), 40, true)]
        public string Remark;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string Sapmaterialinvoice;

        ///<summary>
        ///Ispback
        ///</summary>
        [FieldMapAttribute("ISPBACK", typeof(string), 40, true)]
        public string Ispback;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string Sapreturn;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 200, true)]
        public string Message;

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

    }
    #endregion

    #region Ublog
    /// <summary>
    /// TBLUBLOG
    /// </summary>
    [Serializable, TableMap("TBLUBLOG", "SERIAL")]
    public class Ublog : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Ublog()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Ubno
        ///</summary>
        [FieldMapAttribute("UBNO", typeof(string), 40, false)]
        public string Ubno;

        ///<summary>
        ///Ubline
        ///</summary>
        [FieldMapAttribute("UBLINE", typeof(int), 22, false)]
        public int Ubline;

        ///<summary>
        ///ContactUser
        ///</summary>
        [FieldMapAttribute("CONTACTUSER", typeof(string), 40, false)]
        public string ContactUser;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///Inoutflag
        ///</summary>
        [FieldMapAttribute("INOUTFLAG", typeof(string), 40, true)]
        public string Inoutflag;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, true)]
        public decimal Qty;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FacCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///Mestransno
        ///</summary>
        [FieldMapAttribute("MESTRANSNO", typeof(string), 40, true)]
        public string Mestransno;

        ///<summary>
        ///DocumentDate
        ///</summary>
        [FieldMapAttribute("DOCUMENTDATE", typeof(string), 40, true)]
        public string DocumentDate;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string Sapmaterialinvoice;

        ///<summary>
        ///Ispback
        ///</summary>
        [FieldMapAttribute("ISPBACK", typeof(string), 40, true)]
        public string Ispback;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string Sapreturn;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 200, true)]
        public string Message;

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

    }
    #endregion

    #region DNLOG
    [Serializable, TableMap("DNLOG", "")]
    public class DNLOG : DomainObject
    {

        [FieldMapAttribute("DNLINE", typeof(int), 40, true)]
        public int DNLINE;


        [FieldMapAttribute("DNNO", typeof(string), 40, true)]
        public string DNNO;


        [FieldMapAttribute("Qty", typeof(decimal), 40, false)]
        public decimal Qty;

        [FieldMapAttribute("Unit", typeof(string), 40, false)]
        public string Unit;
        [FieldMapAttribute("RESULT", typeof(string), 40, false)]
        public string RESULT;
        [FieldMapAttribute("ISALL", typeof(string), 40, false)]
        public string ISALL;

        [FieldMapAttribute("DNBATCHNO", typeof(string), 40, false)]
        public string DNBATCHNO;

        [FieldMapAttribute("MESSAGE", typeof(string), 60, false)]
        public string MESSAGE;
        [FieldMapAttribute("MDATE", typeof(string), 40, false)]
        public int MDATE;
        [FieldMapAttribute("MTIME", typeof(string), 40, false)]
        public int MTIME;

        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MUSER;
    }
    #endregion

    #region Polog
    /// <summary>
    /// TBLPOLOG
    /// </summary>
    [Serializable, TableMap("TBLPOLOG", "")]
    public class PoLog : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public PoLog()
        {
        }

        [FieldMapAttribute("Serial", typeof(int), 22, false)]
        public int Serial;


        [FieldMapAttribute("ZNUMBER", typeof(string), 22, false)]
        public string ZNUMBER;

        ///<summary>
        ///Pono
        ///</summary>
        [FieldMapAttribute("PONO", typeof(string), 40, false)]
        public string PONO;

        ///<summary>
        ///Poline
        ///</summary>
        [FieldMapAttribute("POLINE", typeof(string), 40, false)]
        public string PoLine;

        ///<summary>
        ///Faccode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FacCode;

        ///<summary>
        ///Serialno
        ///</summary>
        [FieldMapAttribute("SERIALNO", typeof(string), 40, false)]
        public string SerialNO;

        ///<summary>
        ///Mcode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string Status;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string SAPMaterialInvoice;

        ///<summary>
        ///Operator
        ///</summary>
        [FieldMapAttribute("OPERATOR", typeof(string), 40, true)]
        public string Operator;

        ///<summary>
        ///Vendorinvoice
        ///</summary>
        [FieldMapAttribute("VENDORINVOICE", typeof(string), 40, true)]
        public string VendorInvoice;

        ///<summary>
        ///Storagecode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///Remark
        ///</summary>
        [FieldMapAttribute("REMARK", typeof(string), 40, true)]
        public string Remark;

        ///<summary>
        ///Invoicedate
        ///</summary>
        [FieldMapAttribute("INVOICEDATE", typeof(int), 22, false)]
        public int InvoiceDate;

        ///<summary>
        ///Ispback
        ///</summary>
        [FieldMapAttribute("ISPBACK", typeof(string), 40, true)]
        public string IsPBack;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string SapReturn;

        ///<summary>
        ///Saptimestamp
        ///</summary>
        [FieldMapAttribute("SapDateStamp", typeof(int), 22, false)]
        public int SapDateStamp;

        ///<summary>
        ///Saptimestamp
        ///</summary>
        [FieldMapAttribute("SAPTIMESTAMP", typeof(int), 22, false)]
        public int SapTimeStamp;


        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("Message", typeof(string), 200, true)]
        public string Message;


    }
    #endregion

    #endregion


    #region Log2Sap

    #region Rs2Sap
    /// <summary>
    /// TBLRSLOG
    /// </summary>
    [Serializable, TableMap("TBLRS2SAP", "SERIAL")]
    public class Rs2Sap : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Rs2Sap()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///DocumentDate
        ///</summary>
        [FieldMapAttribute("DOCUMENTDATE", typeof(int), 22, false)]
        public int DocumentDate;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, false)]
        public string FacCode;

        ///<summary>
        ///Inoutflag
        ///</summary>
        [FieldMapAttribute("INOUTFLAG", typeof(string), 40, false)]
        public string InOutFlag;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCode;

        ///<summary>
        ///Mestransno
        ///</summary>
        [FieldMapAttribute("MESTRANSNO", typeof(string), 40, false)]
        public string MesTransNO;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///Rsline
        ///</summary>
        [FieldMapAttribute("RSLINE", typeof(int), 22, false)]
        public int RSLine;

        ///<summary>
        ///Rsno
        ///</summary>
        [FieldMapAttribute("RSNO", typeof(string), 40, false)]
        public string RSNO;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string SapReturn;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string Sapmaterialinvoice;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 40, true)]
        public string Message;

        ///<summary>
        ///tblrslog
        ///</summary>
        [FieldMapAttribute("IsPBack", typeof(string), 40, true)]
        public string IsPBack;

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

    }
    #endregion

    #region Dn_in2Sap
    /// <summary>
    /// TBLDNLOG_IN
    /// </summary>
    [Serializable, TableMap("TBLDN_IN2SAP", "SERIAL")]
    public class Dn_in2Sap : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Dn_in2Sap()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Batchno
        ///</summary>
        [FieldMapAttribute("BATCHNO", typeof(string), 50, false)]
        public string BatchNO;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///Rsline
        ///</summary>
        [FieldMapAttribute("DNLINE", typeof(int), 22, false)]
        public int DNLine;

        ///<summary>
        ///Rsno
        ///</summary>
        [FieldMapAttribute("DNNO", typeof(string), 40, false)]
        public string DNno;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string Sapmaterialinvoice;

        ///<summary>
        ///Ispback
        ///</summary>
        [FieldMapAttribute("ISPBACK", typeof(string), 40, true)]
        public string Ispback;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string Sapreturn;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 200, true)]
        public string Message;

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

    }
    #endregion

    #region Wwpo2Sap
    /// <summary>
    /// TBLWWPOLOG
    /// </summary>
    [Serializable, TableMap("TBLWWPO2SAP", "SERIAL")]
    public class Wwpo2Sap : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Wwpo2Sap()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Pono
        ///</summary>
        [FieldMapAttribute("PONO", typeof(string), 40, false)]
        public string PONO;

        ///<summary>
        ///Poline
        ///</summary>
        [FieldMapAttribute("POLINE", typeof(int), 22, false)]
        public int POLine;

        ///<summary>
        ///VEndorCode
        ///</summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, false)]
        public string VendorCode;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///Inoutflag
        ///</summary>
        [FieldMapAttribute("INOUTFLAG", typeof(string), 40, true)]
        public string InOutFlag;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, true)]
        public decimal Qty;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FacCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///Mestransno
        ///</summary>
        [FieldMapAttribute("MESTRANSNO", typeof(string), 40, true)]
        public string MesTransNO;

        ///<summary>
        ///MestransDate
        ///</summary>
        [FieldMapAttribute("MESTRANSDATE", typeof(int), 22, true)]
        public int MesTransDate;

        ///<summary>
        ///Remark
        ///</summary>
        [FieldMapAttribute("REMARK", typeof(string), 40, true)]
        public string Remark;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string Sapmaterialinvoice;

        ///<summary>
        ///Ispback
        ///</summary>
        [FieldMapAttribute("ISPBACK", typeof(string), 40, true)]
        public string Ispback;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string SapReturn;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 200, true)]
        public string Message;

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

    }
    #endregion

    #region Ub2Sap
    /// <summary>
    /// TBLUBLOG
    /// </summary>
    [Serializable, TableMap("TBLUB2SAP", "SERIAL")]
    public class Ub2Sap : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Ub2Sap()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Ubno
        ///</summary>
        [FieldMapAttribute("UBNO", typeof(string), 40, false)]
        public string UBNO;

        ///<summary>
        ///Ubline
        ///</summary>
        [FieldMapAttribute("UBLINE", typeof(int), 22, false)]
        public int UBLine;

        ///<summary>
        ///ContactUser
        ///</summary>
        [FieldMapAttribute("CONTACTUSER", typeof(string), 40, false)]
        public string ContactUser;

        ///<summary>
        ///MCode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///Inoutflag
        ///</summary>
        [FieldMapAttribute("INOUTFLAG", typeof(string), 40, true)]
        public string InOutFlag;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, true)]
        public decimal Qty;

        ///<summary>
        ///FacCode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FacCode;

        ///<summary>
        ///StorageCode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///Mestransno
        ///</summary>
        [FieldMapAttribute("MESTRANSNO", typeof(string), 40, true)]
        public string MesTransNO;

        ///<summary>
        ///DocumentDate
        ///</summary>
        [FieldMapAttribute("DOCUMENTDATE", typeof(int), 40, true)]
        public int DocumentDate;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string Sapmaterialinvoice;

        ///<summary>
        ///Ispback
        ///</summary>
        [FieldMapAttribute("ISPBACK", typeof(string), 40, true)]
        public string Ispback;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string SapReturn;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 200, true)]
        public string Message;

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

    }
    #endregion

    #region DN2Sap
    [Serializable, TableMap("TBLDN2SAP", "SERIAL")]
    public class DN2Sap : DomainObject
    {
        public DN2Sap()
        {
        }


        [FieldMapAttribute("Serial", typeof(int), 22, false)]
        public int Serial;

        [FieldMapAttribute("DNLINE", typeof(int), 40, true)]
        public int DNLine;


        [FieldMapAttribute("DNNO", typeof(string), 40, true)]
        public string DNNO;


        [FieldMapAttribute("Qty", typeof(decimal), 40, false)]
        public decimal Qty;

        [FieldMapAttribute("Unit", typeof(string), 40, false)]
        public string Unit;
        [FieldMapAttribute("RESULT", typeof(string), 40, false)]
        public string Result;
        [FieldMapAttribute("ISALL", typeof(string), 40, false)]
        public string IsAll;

        [FieldMapAttribute("DNBATCHNO", typeof(string), 40, false)]
        public string BatchNO;

        [FieldMapAttribute("MESSAGE", typeof(string), 60, false)]
        public string Message;


        [FieldMapAttribute("TRANSTYPE", typeof(string), 60, false)]
        public string TRANSTYPE;

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
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;
    }
    #endregion

    #region Po2Sap
    /// <summary>
    /// TBLPOLOG
    /// </summary>
    [Serializable, TableMap("TBLPO2SAP", "SERIAL")]
    public class Po2Sap : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Po2Sap()
        {
        }

        [FieldMapAttribute("Serial", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Pono
        ///</summary>
        [FieldMapAttribute("PONO", typeof(string), 40, false)]
        public string PONO;

        ///<summary>
        ///Poline
        ///</summary>
        [FieldMapAttribute("POLINE", typeof(int), 40, false)]
        public int POLine;

        ///<summary>
        ///Faccode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FacCode;

        ///<summary>
        ///Serialno
        ///</summary>
        [FieldMapAttribute("SERIALNO", typeof(string), 40, false)]
        public string SerialNO;

        ///<summary>
        ///Mcode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, false)]
        public decimal Qty;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Status
        ///</summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string Status;

        ///<summary>
        ///Sapmaterialinvoice
        ///</summary>
        [FieldMapAttribute("SAPMATERIALINVOICE", typeof(string), 40, true)]
        public string SAPMaterialInvoice;

        ///<summary>
        ///Operator
        ///</summary>
        [FieldMapAttribute("OPERATOR", typeof(string), 40, true)]
        public string Operator;

        ///<summary>
        ///Vendorinvoice
        ///</summary>
        [FieldMapAttribute("VENDORINVOICE", typeof(string), 40, true)]
        public string VendorInvoice;

        ///<summary>
        ///Storagecode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///Remark
        ///</summary>
        [FieldMapAttribute("REMARK", typeof(string), 40, true)]
        public string Remark;

        ///<summary>
        ///Invoicedate
        ///</summary>
        [FieldMapAttribute("INVOICEDATE", typeof(int), 22, false)]
        public int InvoiceDate;

        ///<summary>
        ///Ispback
        ///</summary>
        [FieldMapAttribute("ISPBACK", typeof(string), 40, true)]
        public string IsPBack;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string SapReturn;

        ///<summary>
        ///Saptimestamp
        ///</summary>
        [FieldMapAttribute("SapDateStamp", typeof(int), 22, false)]
        public int SapDateStamp;

        ///<summary>
        ///Saptimestamp
        ///</summary>
        [FieldMapAttribute("SAPTIMESTAMP", typeof(int), 22, false)]
        public int SapTimeStamp;

        [FieldMap("STNO", typeof(String), 40, true)]
        public string STNO;

       
        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("Message", typeof(string), 200, true)]
        public string Message;
        [FieldMapAttribute("ZNUMBER", typeof(string), 200, true)]
        public string ZNUMBER;

    }
    #endregion

    #region StockScrap2Sap
    /// <summary>
    /// TBLSTOCKSCRAP2SAP
    /// </summary>
    [Serializable, TableMap("TBLSTOCKSCRAP2SAP", "SERIAL")]
    public class StockScrap2Sap : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public StockScrap2Sap()
        {
        }

        ///<summary>
        ///Lineno
        ///</summary>
        [FieldMapAttribute("LINENO", typeof(int), 22, false)]
        public int LineNO;

        ///<summary>
        ///Messcrapno
        ///</summary>
        [FieldMapAttribute("MESSCRAPNO", typeof(string), 40, true)]
        public string MESScrapNO;

        ///<summary>
        ///Mcode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCode;

        ///<summary>
        ///Scrapcode
        ///</summary>
        [FieldMapAttribute("SCRAPCODE", typeof(string), 40, true)]
        public string ScrapCode;

        ///<summary>
        ///Faccode
        ///</summary>
        [FieldMapAttribute("FACCODE", typeof(string), 40, true)]
        public string FacCode;

        ///<summary>
        ///Storagecode
        ///</summary>
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string StorageCode;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, true)]
        public decimal Qty;


        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Cc
        ///</summary>
        [FieldMapAttribute("CC", typeof(string), 40, true)]
        public string CC;

        ///<summary>
        ///Remark
        ///</summary>
        [FieldMapAttribute("REMARK", typeof(string), 40, true)]
        public string Remark;

        ///<summary>
        ///Documentdate
        ///</summary>
        [FieldMapAttribute("DOCUMENTDATE", typeof(int), 22, false)]
        public int DocumentDate;

        ///<summary>
        ///Operator
        ///</summary>
        [FieldMapAttribute("OPERATOR", typeof(string), 40, true)]
        public string Operator;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 40, true)]
        public string Message;

        ///<summary>
        ///Sapreturn
        ///</summary>
        [FieldMapAttribute("SAPRETURN", typeof(string), 40, true)]
        public string SapReturn;

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
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, false)]
        public string MaintainUser;

    }
    #endregion


    #endregion



    #region Storloctrans add by sam 2016��3��25��

    /// <summary>
    /// TBLSTORLOCTRANS
    /// </summary>
    public class LocStorTransMsg : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public LocStorTransMsg()
        {
        }
        ///<summary>
        ///Transno
        ///</summary>
        [FieldMapAttribute("TRANSNO", typeof(string), 40, false)]
        public string Transno;

        ///<summary>
        ///Transno
        ///</summary>
        [FieldMapAttribute("Msg", typeof(string), 40, false)]
        public string Msg;

    }
    #endregion

    #region ��װ��ҵ add by Chris.H.Wang @20160305
    [Serializable]
    public class PackagingOperations : CartonInvDetailMaterial
    {
        ///<summary>
        ///��Ϊ���Ϻ�
        ///</summary>
        [FieldMapAttribute("CUSTMCODE", typeof(string), 40, true)]
        public string CustMCode;

        ///<summary>
        ///�Ѽ������
        ///</summary>
        [FieldMapAttribute("SQTY", typeof(decimal), 22, true)]
        public decimal SQTY;

        ///<summary>
        ///SN����
        ///</summary>
        [FieldMapAttribute("SN", typeof(string), 40, false)]
        public string SN;
    }


    #endregion

    [Serializable]
    public class MSDWIPExc : MSDWIP
    {
        [FieldMapAttribute("MNAME", typeof(string), 40, true)]
        public string MNAME;

        [FieldMapAttribute("MDESC", typeof(string), 40, true)]
        public string MDESC;

        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCODE;
    }



    #region wwpo add by sam 2016��4��18��
    #region Wwpo
    /// <summary>
    /// TBLWWPO
    /// </summary>
    [Serializable, TableMap("TBLWWPO", "SERIAL")]
    public class MesWWPO : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public MesWWPO()
        {
        }

        ///<summary>
        ///Serial
        ///</summary>
        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Pono
        ///</summary>
        [FieldMapAttribute("PONO", typeof(string), 40, false)]
        public string PONO;

        ///<summary>
        ///Poline
        ///</summary>
        [FieldMapAttribute("POLINE", typeof(int), 22, false)]
        public int POLine;

        ///<summary>
        ///Subline
        ///</summary>
        [FieldMapAttribute("SUBLINE", typeof(int), 22, false)]
        public int SubLine;

        ///<summary>
        ///Mcode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 40, false)]
        public string MCode;

        ///<summary>
        ///Hwmcode
        ///</summary>
        [FieldMapAttribute("HWMCODE", typeof(string), 40, true)]
        public string HWMCode;

        ///<summary>
        ///Qty
        ///</summary>
        [FieldMapAttribute("QTY", typeof(decimal), 22, true)]
        public decimal Qty;

        ///<summary>
        ///Unit
        ///</summary>
        [FieldMapAttribute("UNIT", typeof(string), 40, true)]
        public string Unit;

        ///<summary>
        ///Sapresult
        ///</summary>
        [FieldMapAttribute("SAPRESULT", typeof(string), 40, true)]
        public string SapResult;

        ///<summary>
        ///Message
        ///</summary>
        [FieldMapAttribute("MESSAGE", typeof(string), 200, true)]
        public string Message;

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

    }
    #endregion

    [Serializable]
    public class MesWWPOExc : MesWWPO
    {
        [FieldMapAttribute("MCHLONGDESC", typeof(string), 200, true)]
        public string MChLongDesc;

        [FieldMapAttribute("DQMCODE", typeof(string), 40, true)]
        public string DQMCode;

        [FieldMapAttribute("CPMDESC", typeof(string), 200, true)]
        public string CPMDesc;

        [FieldMapAttribute("CPDQMCODE", typeof(string), 40, true)]
        public string CPDQMCode;
    }

    #endregion

    #region Storagesap2mes
    /// <summary>
    /// TBLSTORAGESAP2MES
    /// </summary>
    [Serializable, TableMap("TBLSTORAGESAP2MES", "SERIAL")]
    public class Storagesap2mes : BenQGuru.eMES.Common.Domain.DomainObject
    {
        public Storagesap2mes()
        {
        }

        [FieldMapAttribute("SERIAL", typeof(int), 22, false)]
        public int Serial;

        ///<summary>
        ///Mcode
        ///</summary>
        [FieldMapAttribute("MCODE", typeof(string), 18, true)]
        public string Mcode;

        ///<summary>
        ///Dqmcode
        ///</summary>
        [FieldMapAttribute("DQMCODE", typeof(string), 40, true)]
        public string Dqmcode;

        ///<summary>
        ///Messtoragecode
        ///</summary>
        [FieldMapAttribute("MESSTORAGECODE", typeof(string), 40, true)]
        public string Messtoragecode;

        ///<summary>
        ///Sapstoragecode
        ///</summary>
        [FieldMapAttribute("SAPSTORAGECODE", typeof(string), 40, true)]
        public string Sapstoragecode;

        ///<summary>
        ///Mesqty
        ///</summary>
        [FieldMapAttribute("MESQTY", typeof(decimal), 22, true)]
        public decimal Mesqty;

        ///<summary>
        ///Sapqty
        ///</summary>
        [FieldMapAttribute("SAPQTY", typeof(decimal), 22, true)]
        public decimal Sapqty;

        ///<summary>
        ///Disqty
        ///</summary>
        [FieldMapAttribute("DISQTY", typeof(decimal), 22, true)]
        public decimal Disqty;

        ///<summary>
        ///MaintainUser
        ///</summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MaintainUser;

        ///<summary>
        ///MaintainDate
        ///</summary>
        [FieldMapAttribute("MDATE", typeof(int), 22, true)]
        public int MaintainDate;

        ///<summary>
        ///MaintainTime
        ///</summary>
        [FieldMapAttribute("MTIME", typeof(int), 22, true)]
        public int MaintainTime;

    }

    [Serializable, TableMap("TBLSPECSTORAGEINFO", "MCODE, STORAGECODE, LOCATIONCODE")]
    public class SPECSTORAGEINFO : BenQGuru.eMES.Common.Domain.DomainObject
    {
        [FieldMapAttribute("MCODE", typeof(string), 40, true)]
        public string MCODE;


        [FieldMapAttribute("DQMCODE", typeof(string), 40, true)]
        public string DQMCODE;
        [FieldMapAttribute("STORAGECODE", typeof(string), 40, true)]
        public string STORAGECODE;

        [FieldMapAttribute("LOCATIONCODE", typeof(string), 40, true)]
        public string LOCATIONCODE;

        [FieldMapAttribute("STORAGEQTY", typeof(decimal), 40, true)]
        public decimal STORAGEQTY;

        [FieldMapAttribute("CUSER", typeof(string), 40, true)]
        public string CUSER;

        [FieldMapAttribute("CDATE", typeof(int), 40, true)]
        public int CDATE;

        [FieldMapAttribute("CTIME", typeof(int), 40, true)]
        public int CTIME;

        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string MUSER;

        [FieldMapAttribute("MDATE", typeof(int), 40, true)]
        public int MDATE;

        [FieldMapAttribute("MTIME", typeof(int), 40, true)]
        public int MTIME;


    }



    #endregion

    #region DocForQuery
    public class InDocForQuery : InvDoc
    {
        public InDocForQuery()
        {
        }

        //[FieldMapAttribute("DIRNAME", typeof(string), 40, true)]
        //public string DirName;
    }
    #endregion


}

