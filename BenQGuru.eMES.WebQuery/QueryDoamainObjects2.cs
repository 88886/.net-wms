using System;
using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.WebQuery
{

    #region ��Ʒ׷��

    /// <summary>
    /// ��Ʒ׷��
    /// </summary>
    [Serializable, TableMap("TBLSIMULATIONREPORT", "RCARD,MOCODE")]
    public class ItemTracing : DomainObject
    {
        /// <summary>
        /// ���к�
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string  RCard;

		/// <summary>
		/// ���к�
		/// </summary>
		[FieldMapAttribute("TCARD", typeof(string), 40, true)]
		public string  TCard;

        /// <summary>
        /// ���к�
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(string), 40, true)]
        public decimal  RCardSeq;

        /// <summary>
        /// ��Ʒ
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string  ItemCode;

        /// <summary>
        /// ��Ʒ״̬
        /// </summary>
        [FieldMapAttribute("STATUS", typeof(string), 40, true)]
        public string  ItemStatus;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string  MOCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string  ModelCode;

        /// <summary>
        /// ���ڹ���
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string  OPCode;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("OPTYPE", typeof(string), 40, true)]
        public string  OPType;
    
        /// <summary>
        /// ����;��
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string  RouteCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string  SegmentCode;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string  LineCode;

        /// <summary>
        /// ��Դ
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string  ResCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int  MaintainDate;

        /// <summary>
        /// ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int  MaintainTime;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string  MaintainUser;

		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("LACTION", typeof(string), 40, true)]
		public string  LastAction;
    }

    public class ItemTracingQuery : ItemTracing
    {
        [FieldMapAttribute("MMODELCODE", typeof(string), 40, true)]
        public string MaterialModelCode;

        [FieldMapAttribute("BIGSSCODE", typeof(string), 40, true)]
        public string BigStepSequenceCode;
    }

    #endregion

    #region ��������

    /// <summary>
    /// ��������
    /// </summary>
    public class ProductionProcess : DomainObject
    {
        /// <summary>
        /// ���к�
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string  RCard;

        /// <summary>
        /// ���к�˳��
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(string), 40, true)]
        public decimal  RCardSequence;


        /// <summary>
        /// ����;��
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string  RouteCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string  OPCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string  MOCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string  ModelCode;

        /// <summary>
        /// ��Ʒ
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string  ItemCode;

        /// <summary>
        /// ��Ʒ״̬
        /// </summary>
        [FieldMapAttribute("ACTIONRESULT", typeof(string), 40, true)]
        public string  ItemStatus;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("OPTYPE", typeof(string), 40, true)]
        public string  OPType;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string  SegmentCode;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string  LineCode;

        /// <summary>
        /// ��Դ
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string  ResCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int  MaintainDate;

        /// <summary>
        /// ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int  MaintainTime;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string  MaintainUser;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("ACTION", typeof(string), 40, true)]
		public string  Action;


    }



    #endregion

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    public class OPResult : DomainObject
    {
        /// <summary>
        /// ���к�
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string  RCard;

        /// <summary>
        /// ���к�˳��
        /// </summary>
        [FieldMapAttribute("RCARDSEQ", typeof(string), 40, true)]
        public decimal  RCardSequence;


        /// <summary>
        /// ����;��
        /// </summary>
        [FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
        public string  RouteCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("OPCODE", typeof(string), 40, true)]
        public string  OPCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MOCODE", typeof(string), 40, true)]
        public string  MOCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
        public string  ModelCode;

        /// <summary>
        /// ��Ʒ
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string  ItemCode;

        /// <summary>
        /// ��Ʒ״̬
        /// </summary>
        [FieldMapAttribute("ACTIONRESULT", typeof(string), 40, true)]
        public string  ItemStatus;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("OPTYPE", typeof(string), 40, true)]
        public string  OPType;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
        public string  SegmentCode;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("SSCODE", typeof(string), 40, true)]
        public string  LineCode;

        /// <summary>
        /// ��Դ
        /// </summary>
        [FieldMapAttribute("RESCODE", typeof(string), 40, true)]
        public string  ResCode;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int  MaintainDate;

        /// <summary>
        /// ʱ��
        /// </summary>
        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int  MaintainTime;

        /// <summary>
        /// ������
        /// </summary>
        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        public string  MaintainUser;
		
		/// <summary>
		/// CARTON��
		/// </summary>
		[FieldMapAttribute("CARTONCODE", typeof(string), 40, false)]
		public string  CartonCode;

		/// <summary>
		/// ShelfPK
		/// </summary>
		[FieldMapAttribute("SHELFNO", typeof(string), 40, false)]
		public string  ShelfPK;

    }

    #endregion

    #region �������Ϻ�

    /// <summary>
    /// �������Ϻ�
    /// </summary>
    public class LotItemInfo : DomainObject
    {
        /// <summary>
        /// �������Ϻ�
        /// </summary>
        [FieldMapAttribute("MCARD", typeof(string), 40, true)]
        public string MCard ;
    
        /// <summary>
        /// �����Ϻ�
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode ;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, true)]
        public string Customer ;

        /// <summary>
        /// �����Ϻ�
        /// </summary>
        [FieldMapAttribute("VENDORITEMCODE", typeof(string), 40, true)]
        public string CustomerItem ;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, true)]
        public string LotNO ;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("DATECODE", typeof(string), 40, true)]
        public string DateCode ;

        /// <summary>
        /// ���
        /// </summary>
        [FieldMapAttribute("VERSION", typeof(string), 40, true)]
        public string Version ;


        [FieldMapAttribute("PCBA", typeof(string), 40, true)]
        public string PCBA ;


        [FieldMapAttribute("BIOS", typeof(string), 40, true)]
        public string BIOS ;

        [FieldMapAttribute("TRYITEMCODE", typeof(string), 40, true)]
        public string TryItemCode ;

    }


    #endregion

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    public class RunningItemInfo : DomainObject
    {
        /// <summary>
        /// ԭʼ���к�
        /// </summary>
        [FieldMapAttribute("MCARD", typeof(string), 40, true)]
        public string MCARD ;
    
        /// <summary>
        /// �����Ϻ�
        /// </summary>
        [FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
        public string ItemCode ;

        /// <summary>
        /// ����
        /// </summary>
        [FieldMapAttribute("VENDORCODE", typeof(string), 40, true)]
        public string Customer ;

        /// <summary>
        /// �����Ϻ�
        /// </summary>
        [FieldMapAttribute("VENDORITEMCODE", typeof(string), 40, true)]
        public string CustomerItem ;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("LOTNO", typeof(string), 40, true)]
        public string LotNO ;

        /// <summary>
        /// ��������
        /// </summary>
        [FieldMapAttribute("DATECODE", typeof(string), 40, true)]
        public string DateCode ;

        /// <summary>
        /// ���
        /// </summary>
        [FieldMapAttribute("VERSION", typeof(string), 40, true)]
        public string Version ;


        [FieldMapAttribute("PCBA", typeof(string), 40, true)]
        public string PCBA ;


        [FieldMapAttribute("BIOS", typeof(string), 40, true)]
        public string BIOS ;

        [FieldMapAttribute("TRACINGABLE", typeof(bool), 40, true)]
        public bool CanTracing ;

    }
    #endregion

    #region ���к�ת��
    /// <summary>
    /// ���к�ת��
    /// </summary>
    public class SNInfo : DomainObject
    {
        /// <summary>
        /// ԭʼ���к�
        /// </summary>
        [FieldMapAttribute("SCARD", typeof(string), 40, true)]
        public string SourceCard ;
    
        /// <summary>
        /// �����к�
        /// </summary>
        [FieldMapAttribute("TCARD", typeof(string), 40, true)]
        public string TranslateCard ;

        /// <summary>
        /// �����к�
        /// </summary>
        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard ;



    }

    #endregion

    #region ��װ

    public class PackingInfo : DomainObject
    {
        /// <summary>
        /// ԭʼ���к�
        /// </summary>
        [FieldMapAttribute("SCARD", typeof(string), 40, true)]
        public string SourceCard ;
    
        /// <summary>
        /// �����к�
        /// </summary>
        [FieldMapAttribute("TCARD", typeof(string), 40, true)]
        public string TranslateCard ;


    }


    #endregion

	#region ά�޹�����
	/// <summary>
	/// ά�޹�����
	/// </summary>
	public class TSOPResult : DomainObject
	{
		/// <summary>
		/// ���к�
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, true)]
		public string  RCard;

		/// <summary>
		/// ���к�˳��
		/// </summary>
		[FieldMapAttribute("RCARDSEQ", typeof(string), 40, true)]
		public decimal  RCardSequence;


		/// <summary>
		/// ����;��
		/// </summary>
		[FieldMapAttribute("FRMROUTECODE", typeof(string), 40, true)]
		public string  RouteCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("COPCODE", typeof(string), 40, true)]
		public string  OPCode;

		/// <summary>
		/// ��Ʒ״̬
		/// </summary>
		[FieldMapAttribute("TSSTATUS", typeof(string), 40, true)]
		public string  ItemStatus;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OPTYPE", typeof(string), 40, true)]
		public string  OPType;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string  SegmentCode;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string  LineCode;

		/// <summary>
		/// ��Դ
		/// </summary>
		[FieldMapAttribute("CRESCODE", typeof(string), 40, true)]
		public string  ResCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// ʱ��
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]
		public string  MaintainUser;


	}

	#endregion

	#region ά����Ϣ
	/// <summary>
	/// ά����Ϣ
	/// </summary>
	public class TSInfo : DomainObject
	{
		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("ECGCODE", typeof(string), 40, true)]
		public string  ErrorCodeGroup;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("ECODE", typeof(string), 40, true)]
		public string  ErrorCode;


		/// <summary>
		/// ����ԭ��
		/// </summary>
		[FieldMapAttribute("ECSCODE", typeof(string), 40, true)]
		public string  ErrorCauseCode;

		/// <summary>
		/// ����λ��
		/// </summary>
		[FieldMapAttribute("ELOC", typeof(string), 40, true)]
		public string  ErrorLocation;

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("EPART", typeof(string), 40, true)]
		public string  ErrorPart;

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("SOLCODE", typeof(string), 40, true)]
		public string  SolutionCode;

		/// <summary>
		/// ���α�
		/// </summary>
		[FieldMapAttribute("DUTYCODE", typeof(string), 40, true)]
		public string  DutyCode;

		/// <summary>
		/// ����˵��
		/// </summary>
		[FieldMapAttribute("SOLMEMO", typeof(string), 40, true)]
		public string  SolutionMemo;

		/// <summary>
		/// ά�޹�
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// ά������
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(string), 40, true)]
		public int  MaintainDate;

		/// <summary>
		/// ά��ʱ��
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(string), 40, true)]
		public int  MaintainTime;
	}

	#endregion

	#region ά����Ϣ
	/// <summary>
	/// ������Ϣ
	/// </summary>
	public class HLInfo : DomainObject
	{

		/// <summary>
		/// �������Ϻ�
		/// </summary>
		[FieldMapAttribute("MITEMCODE", typeof(string), 40, true)]
		public string  MItemCode;

		/// <summary>
		/// ���������к�
		/// </summary>
		[FieldMapAttribute("MCARD", typeof(string), 40, true)]
		public string  MCard;

		/// <summary>
		/// ԭ�����Ϻ�
		/// </summary>
		[FieldMapAttribute("SITEMCODE", typeof(string), 40, false)]
		public string  SourceItemCode;

		/// <summary>
		/// ԭ�������к�
		/// </summary>
		[FieldMapAttribute("MSCARD", typeof(string), 40, false)]
		public string  MSourceCard;

		/// <summary>
		/// ����λ��
		/// </summary>
		[FieldMapAttribute("LOC", typeof(string), 40, true)]
		public string  Location;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 40, true)]
		public string  LotNO;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("VENDORCODE", typeof(string), 40, true)]
		public string  VendorCode;

		/// <summary>
		/// �����Ϻ�
		/// </summary>
		[FieldMapAttribute("VENDORITEMCODE", typeof(string), 40, true)]
		public string  VendorItemCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("DATECODE", typeof(string), 40, true)]
		public string  DateCode;

		/// <summary>
		/// ��Ʒ�汾
		/// </summary>
		[FieldMapAttribute("REVERSION", typeof(string), 40, true)]
		public string  Version;

		/// <summary>
		/// PCBA�汾
		/// </summary>
		[FieldMapAttribute("PCBA", typeof(string), 40, true)]
		public string  PCBA;

		/// <summary>
		/// BIOS�汾
		/// </summary>
		[FieldMapAttribute("BIOS", typeof(string), 40, true)]
		public string  BIOS;

		/// <summary>
		/// ����˵��
		/// </summary>
		[FieldMapAttribute("MEMO", typeof(string), 40, true)]
		public string  Memo;
	}

	#endregion

	#region OQC ���

	public class OQCLRR : DomainObject
	{
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string  ModelCode;

		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ItemCode;

		/// <summary>
		/// �������������
		/// </summary>
		[FieldMapAttribute("LOTTOTALCOUNT", typeof(decimal), 15, true)]
		public decimal  LotTotalCount;

		/// <summary>
		/// ���˵�������
		/// </summary>
		[FieldMapAttribute("LOTNGCOUNT", typeof(decimal), 15, true)]
		public decimal  LotNGCount;

		[FieldMapAttribute("DATEGROUP", typeof(int), 15, true)]
		public int DateGroup;

		[FieldMapAttribute("LRR", typeof(decimal), 15, true)]
		public decimal  LRR
		{
			get
			{
				if(LotTotalCount == 0)
				{
					return 0;
				}
				return Math.Round(LotNGCount/LotTotalCount,4);
			}
		}

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("LOTSAMPLECOUNT", typeof(decimal), 15, true)]
		public decimal  LotSampleCount;

		/// <summary>
		/// ������������
		/// </summary>
		[FieldMapAttribute("LOTSAMPLENGCOUNT", typeof(decimal), 15, true)]
		public decimal  LotSampleNGCount;

		[FieldMapAttribute("DPPM", typeof(decimal), 15, true)]
		public decimal  DPPM
		{
			get
			{
				if(LotSampleCount == 0)
				{
					return 0;
				}
				return Convert.ToInt32((LotSampleNGCount/LotSampleCount) * 1000000);
			}
		}

		/// <summary>
		/// �ͼ�����
		/// </summary>
		[FieldMapAttribute("LOTSIZE", typeof(decimal), 15, true)]
		public decimal  LotSize;
		
	}

	public class OQCSDR : DomainObject
	{
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string  ModelCode;

		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ItemCode;

		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// ������Ĳ�Ʒ����
		/// </summary>
		[FieldMapAttribute("SAMPLECOUNT", typeof(decimal), 15, true)]
		public decimal  SampleCount;

		/// <summary>
		/// ������Ĳ�����Ʒ����
		/// </summary>
		[FieldMapAttribute("SAMPLENGCOUNT", typeof(decimal), 15, true)]
		public decimal  SampleNGCount;

		[FieldMapAttribute("DATEGROUP", typeof(int), 15, true)]
		public int DateGroup;

		[FieldMapAttribute("LRR", typeof(decimal), 15, true)]
		public decimal  SDR
		{
			get
			{
				if(SampleCount == 0)
				{
					return 0;
				}
				return Math.Round((SampleNGCount/SampleCount)*1000000,0);
			}
		}
		
	}

	public class OQCErrorCode : DomainObject
	{
		[FieldMapAttribute("ECODE", typeof(string), 40, true)]
		public string  ErrorCode;

		[FieldMapAttribute("ECDESC", typeof(string), 40, true)]
		public string  ErrorCodeDesc;

		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ItemCode;

		[FieldMapAttribute("ErrorCodeCardQty", typeof(decimal), 15, true)]
		public decimal  ErrorCodeCardQty;

		[FieldMapAttribute("ItemCardQty", typeof(decimal), 15, true)]
		public decimal  ItemCardQty;
		
	}
     

	#endregion
}
