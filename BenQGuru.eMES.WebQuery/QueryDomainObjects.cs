using System;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.DataCollect;

namespace BenQGuru.eMES.WebQuery
{
	/// <summary>
	/// OQCFirstHandingYield ��ժҪ˵����
	/// </summary>
	public class OQCFirstHandingYield : DomainObject
	{
		/// <summary>
		/// ���ִ���[Model]
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string  ModelCode;

		/// <summary>
		/// �Ϻ�[ItemCode]
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ItemCode;

		/// <summary>
		/// һ���ύ����
		/// </summary>
		[FieldMapAttribute("amount", typeof(decimal), 10, true)]
		public decimal FirstHandingAmount;
 
		/// <summary>
		/// һ���ϸ���
		/// </summary>
		[FieldMapAttribute("yield_amount", typeof(decimal), 10, true)]
		public decimal FirstHandingYieldAmount;
		
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("yield_percent", typeof(decimal), 10, true)]
		public decimal FirstHandingYieldPercent;
	}

	/// <summary>
	/// OQCһ���ϸ�����ϸ
	/// </summary>
	public class OQCFirstHandingYieldDetail : DomainObject
	{
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("LOTNO",typeof(string), 40, true)]
		public string LotNo;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("LOTSIZE",  typeof(decimal), 10, true)]
		public decimal LotSize;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("SSIZE",  typeof(decimal), 10, true)]
		public decimal SSize;

		/// <summary>
		/// ʵ�ʲ�������
		/// </summary>
		[FieldMapAttribute("actcheckcount",  typeof(decimal), 10, true)]
		public decimal ActCheckSize;

		/// <summary>
		/// ��ȫȱ��
		/// </summary>
		[FieldMapAttribute("AGRADETIMES",  typeof(decimal), 10, true)]
		public decimal Agradetimes;

		/// <summary>
		/// ����ȱ��
		/// </summary>
		[FieldMapAttribute("BGGRADETIMES",  typeof(decimal), 10, true)]
		public decimal Bggradetimes;

		/// <summary>
		/// ���ȱ��
		/// </summary>
		[FieldMapAttribute("CGRADETIMES",  typeof(decimal), 10, true)]
		public decimal Cgradetimes;

        [FieldMapAttribute("ZGRADETIMES", typeof(decimal), 10, true)]
        public decimal ZGrageTimes;

		/// <summary>
		/// �ж����
		/// </summary>
		[FieldMapAttribute("LOTSTATUS",typeof(string), 40, true)]
		public string LotStatus;

		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;
		
	}

	public class HistroyQuantitySummary : DomainObject
	{
		/// <summary>
		/// ���ִ���[Model]
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string ModelCode;

		/// <summary>
		/// �Ϻ�[ItemCode]
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MoCode;

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string OperationCode;

		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode;

		/// <summary>
		/// ���������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode;

		/// <summary>
		/// ��Դ����
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string ResourceCode;

		[FieldMapAttribute("MONTH", typeof(string), 8, true)]
		public string Month;

		[FieldMapAttribute("WEEK", typeof(string), 8, true)]
		public string Week;

		[FieldMapAttribute("DAY", typeof(int), 8, true)]
		public int NatureDate;

		[FieldMapAttribute("SHIFTDAY", typeof(int), 8, true)]
		public int ShiftDay;

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
		/// ������
		/// </summary>
		[FieldMapAttribute("OUTPUTQTY", typeof(int), 10, true)]
		public int Quantity;

		/// <summary>
		/// Ͷ����
		/// </summary>
		[FieldMapAttribute("INPUTQTY", typeof(int), 10, true)]
		public int InputQty;
	}

	public class HistoryYieldPercent : DomainObject
	{
		/// <summary>
		/// ���ִ���[Model]
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string ModelCode;

		/// <summary>
		/// �Ϻ�[ItemCode]
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MoCode;

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string OperationCode;

		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode;

		/// <summary>
		/// ���������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode;

		/// <summary>
		/// ��Դ����
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string ResourceCode;

		[FieldMapAttribute("MONTH", typeof(string), 8, true)]
		public string Month;

		[FieldMapAttribute("WEEK", typeof(string), 8, true)]
		public string Week;

		[FieldMapAttribute("DAY", typeof(int), 8, true)]
		public int NatureDate;

		[FieldMapAttribute("SHIFTDAY", typeof(int), 8, true)]
		public int ShiftDay;

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

		[FieldMapAttribute("NGTimes", typeof(int), 10, true)]
		public long NGTimes;

		[FieldMapAttribute("AllTimes", typeof(int), 10, true)]
		public long AllTimes;

		[FieldMapAttribute("OUTPUTQTY", typeof(int), 10, true)]
		public long Quantity = 0;

		[FieldMapAttribute("INPUTQTY", typeof(int), 10, true)]
		public long InputQuantity = 0;

		[FieldMapAttribute("YieldPercent", typeof(decimal), 10, true)]
		public decimal YieldPercent;

		[FieldMapAttribute("ALLGOODQTY", typeof(int), 10, true)]
		public long AllGoodQuantity;

		[FieldMapAttribute("AllGoodYieldPercent", typeof(decimal), 10, true)]
		public decimal AllGoodYieldPercent;

		[FieldMapAttribute("NotYieldPercent", typeof(decimal), 10, true)]
		public decimal NotYieldPercent;

		[FieldMapAttribute("NotYieldLocation", typeof(int), 10, true)]
		public long NotYieldLocation;

		[FieldMapAttribute("totallocation", typeof(int), 10, true)]
		public long TotalLocation;

		[FieldMapAttribute("PPM", typeof(int), 10, true)]
		public long PPM;
	}


	public class OnWipInfoOnOperation : DomainObject
	{		
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string OperationCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("QTY", typeof(decimal), 10, true)]
		public int OnWipQuantityOnOperation;
	}

	public class OnWipInfoOnResource : DomainObject
	{		
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MoCode;

		[FieldMapAttribute("SHIFTCODE", typeof(string), 40, true)]
		public string ShiftCode;

		[FieldMapAttribute("SHIFTDAY", typeof(string), 40, true)]
		public int ShiftDay;
		
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string ResourceCode;

		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode;

		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode;

		/// <summary>
		/// ������Ʒ����
		/// </summary>
		[FieldMapAttribute("GOODQTY", typeof(decimal), 10, true)]
		public int OnWipGoodQuantityOnResource;

		/// <summary>
		/// ���Ʋ���Ʒ����
		/// </summary>
		[FieldMapAttribute("NGQTY", typeof(decimal), 10, true)]
		public int OnWipNGQuantityOnResource;

		/// <summary>
		/// ���˴���������
		/// </summary>
		[FieldMapAttribute("NGForReworksQTY", typeof(decimal), 10, true)]
		public int NGForReworksQTY;

		/* Added by jessie lee, 2005/12/8, 
		 * һ�������ֶΣ�ά�޹���TS��ר��
		 * */
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("TSConfirmQty", typeof(decimal), 10, true)]
		public int TSConfirmQty;

		/// <summary>
		/// ά��������
		/// </summary>
		[FieldMapAttribute("TSQty", typeof(decimal), 10, true)]
		public int TSQty;

		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("TSReflowQty", typeof(decimal), 10, true)]
		public int TSReflowQty;


	}

	public class OnWipInfoDistributing : DomainObject
	{
		/// <summary>
		/// ���к�
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, true)]
		public string RunningCard;
		
		/// <summary>
		///
		/// </summary>
		[FieldMapAttribute("RCARDSEQ", typeof(decimal), 10, true)]
		public decimal RunningCardSequence;

		/// <summary>
		/// ���к�
		/// </summary>
		[FieldMapAttribute("TCARD", typeof(string), 40, true)]
		public string TranslateCard;

		/// <summary>
		/// ��Ʒ״̬
		/// </summary>
		[FieldMapAttribute("STATUS", typeof(string), 40, true)]
		public string ProductStatus;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OPCONTROL", typeof(string), 40, true)]
		public string OPControl;	
	
		/// <summary>
		/// �ְ����
		/// </summary>
		[FieldMapAttribute("IDMERGERULE", typeof(decimal), 10, true)]
		public decimal  IDMergeRule;

		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("LACTION", typeof(string), 40, true)]
		public string  Action;

	}

	public class ComponentLoadingTracking : DomainObject
	{
		[FieldMapAttribute("SN", typeof(string), 40, true)]
		public string SN;

		[FieldMapAttribute("RCARDSEQ", typeof(int), 8, true)]
		public int SNSeq;

		[FieldMapAttribute("MCARD", typeof(string), 40, true)]
		public string MCard;

		[FieldMapAttribute("MSEQ", typeof(int), 10, true)]
		public int  MSequence;

		[FieldMapAttribute("INNO", typeof(string), 40, true)]
		public string INNO;

		[FieldMapAttribute("KEYPARTS", typeof(string), 40, true)]
		public string KeyParts;

		[FieldMapAttribute("MITEMCODE", typeof(string), 40, true)]
		public string MItemCode;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MoCode = "";

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string ModelCode = "";

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode = "";

		[FieldMapAttribute("SNSTATE", typeof(string), 40, true)]
		public string SNState = "";

		[FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
		public string RouteCode = "";

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string OperationCode = "";

		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode = "";

		/// <summary>
		/// ���������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode = "";

		/// <summary>
		/// ��Դ����
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string ResourceCode = "";

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser = "";

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate = 0;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime = 0;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LOTNO", typeof(string), 40, false)]
		public string  LotNO;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PCBA", typeof(string), 40, false)]
		public string  PCBA;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("BIOS", typeof(string), 40, false)]
		public string  BIOS;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("VERSION", typeof(string), 40, false)]
		public string  Version;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("VENDORITEMCODE", typeof(string), 40, false)]
		public string  VendorItemCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("VENDORCODE", typeof(string), 40, false)]
		public string  VendorCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATECODE", typeof(string), 40, false)]
		public string  DateCode;
	}

	public class RealTimeQuantity : DomainObject
	{
		/// <summary>
		/// ���������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode = "";

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode = "";

		/// <summary>
		/// ʱ��δ���
		/// </summary>
		[FieldMapAttribute("TPCODE", typeof(string), 40, true)]
		public string TimePeriodCode = "";

//		[FieldMapAttribute("OUTPUTQTY", typeof(int), 10, true)]
//		public int Quantity = 0; 

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OUTPUTQTY", typeof(decimal), 10, true)]
		public int OutputQuantity = 0; 

		/// <summary>
		/// Ͷ������
		/// </summary>
		[FieldMapAttribute("INPUTQTY", typeof(decimal), 10, true)]
		public int InputQuantity = 0; 

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("SCRAPQTY", typeof(decimal), 10, true)]
		public int ScrapQuantity = 0; 
	}

	//�޸�Ŀ��:tblonwip���ѯ
	//�޸�ʱ��:2006.9.25
	//�޸���:melo zheng
	public class TimeQuantitySum : DomainObject
	{
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode = "";

		/// <summary>
		/// ��Դ
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string ResourceCode = "";

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int MaintainDate = 0;

		/// <summary>
		/// ʱ��
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int MaintainTime = 0;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("ACTIONRESULT", typeof(int), 10, true)]
		public int ActionResult = 0;
	}

	 /// <summary>
	 /// Ͷ���������ʵ��
	 /// </summary>
	public class RealTimeInputOutputQuantity : DomainObject
	{
	
		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MOCode = "";

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string ModelCode = "";

		/// <summary>
		/// ��ƷCode
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode = "";

		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode = "";

		//�����ƻ�����
		[FieldMapAttribute("MOPLANQTY", typeof(decimal), 10, true)]
		public decimal MOPlanqty = 0; 

		//��������Ͷ���� (��ǰ���Ͷ�������)
		[FieldMapAttribute("INPUTQTY", typeof(decimal), 10, true)]
		public decimal MOShiftInputqty = 0; 

		//�����ۼ�Ͷ����
		[FieldMapAttribute("MOINPUTQTY", typeof(decimal), 10, true)]
		public decimal MOInputqty = 0; 

		//������������� (��ǰ��β���������)
		[FieldMapAttribute("OUTPUTQTY", typeof(decimal), 10, true)]
		public decimal MOShiftOutputqty = 0; 

		//�����ۼƲ�����
		[FieldMapAttribute("MOACTQTY", typeof(decimal), 10, true)]
		public decimal MOOutputqty = 0; 

		//�����ۼƲ����
		[FieldMapAttribute("moscrapqty", typeof(decimal), 10, true)]
		public decimal MOScrapqty = 0; 

		/// <summary>
		/// ���빤������
		/// </summary>
		[FieldMapAttribute("OffMoQty", typeof(decimal), 10,false)]
		public decimal  MOOffQty = 0;

	}

	//ʵʱͶ�������ϸ
	public class RealTimeInputQuantity : DomainObject
	{
		/// <summary>
		/// ���������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode = "";

		/// <summary>
		/// ʱ��δ���
		/// </summary>
		[FieldMapAttribute("TPCODE", typeof(string), 40, true)]
		public string TimePeriodCode = "";

		[FieldMapAttribute("INPUTQTY", typeof(int), 10, true)]
		public int Quantity = 0; 
	}

	//ʵʱ����������ϸ
	public class RealTimeOutputQuantity : DomainObject
	{
		/// <summary>
		/// ���������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode = "";

		/// <summary>
		/// ʱ��δ���
		/// </summary>
		[FieldMapAttribute("TPCODE", typeof(string), 40, true)]
		public string TimePeriodCode = "";

		[FieldMapAttribute("OUTPUTQTY", typeof(int), 10, true)]
		public int Quantity = 0; 
	}

	public class RealTimeDetails : DomainObject
	{
		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MoCode = "";

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string ModelCode = "";

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode = "";

//		[FieldMapAttribute("OUTPUTQTY", typeof(int), 10, true)]
//		public int Quantity = 0;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OUTPUTQTY", typeof(decimal), 10, true)]
		public int OutputQuantity = 0; 

		/// <summary>
		/// Ͷ������
		/// </summary>
		[FieldMapAttribute("INPUTQTY", typeof(decimal), 10, true)]
		public int InputQuantity = 0; 

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("SCRAPQTY", typeof(decimal), 10, true)]
		public int ScrapQuantity = 0; 

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("ITEMNAME", typeof(string), 40, true)]
		public string ItemName = "";

		/// <summary>
		/// ������ע
		/// </summary>
		[FieldMapAttribute("MOMEMO", typeof(string), 40, true)]
		public string MoMemo = "";
	}

	public class RealTimeYieldPercent : DomainObject
	{
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode = "";		

		/// <summary>
		/// ���������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode = "";		

		[FieldMapAttribute("ALLGOODQTY", typeof(int), 10, true)]
		public int AllGoodQuantity;

		[FieldMapAttribute("OUTPUTQTY", typeof(int), 10, true)]
		public int Quantity = 0; 

		[FieldMapAttribute("AllGoodYieldPercent", typeof(decimal), 10, true)]
		public decimal AllGoodYieldPercent;
	}

	public class RealTimeDefect : DomainObject
	{
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode = "";
		
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string ResourceCode = "";

		[FieldMapAttribute("ERRORCODE", typeof(string), 40, true)]
		public string ErrorCode = "";

		[FieldMapAttribute("ERRORCODEGROUP", typeof(string), 40, true)]
		public string ErrorCodeGroup = "";

		[FieldMapAttribute("DEFECTQTY", typeof(int), 10, true)]
		public int DefectQuantity = 0;

		//��������������
		[FieldMapAttribute("ECGDESC", typeof(string), 100, true)]
		public string ECGDESC = "";

		//������������
		[FieldMapAttribute("ECDESC", typeof(string), 100, true)]
		public string ECDESC = "";

		[FieldMapAttribute("INPUTQTY", typeof(int), 10, true)]
		public int InputQty = 0;

		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode = "";
	} 

	public class TracedKeyParts : OnWIPItem
	{
		public bool CanTrace = false;

		//��������
		public string MItemName;
	}

	public class TracedMinno : MINNO
	{
		/// <summary>
		/// 0,����
		/// 1,����
		/// </summary>
		public int  ActionType;
	}

	public class MOQueryCode : DomainObject
	{
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MOCode = "";
		
	} 

	public class QDORes2EC: DomainObject
	{
		[FieldMapAttribute("ECGCode", typeof(string), 40, true)]
		public string  ErrorCodeGroup;

		[FieldMapAttribute("ECCode", typeof(string), 40, true)]
		public string  ErrorCode;

		public string  ErrorCodeDesc;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("TPCode", typeof(string), 40, true)]
		public string  TPCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("TPBTime", typeof(int), 6, true)]
		public int  TPBTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("TPETime", typeof(int), 6, true)]
		public int  TPETime;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("NGTimes", typeof(decimal), 10, true)]
		public decimal  NGTimes;

	}

	public class RPTCenterQuantity : DomainObject
	{
		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode;

		/// <summary>
		/// ���ղ���
		/// </summary>
		[FieldMapAttribute("DayQuantity", typeof(int), 40, true)]
		public int DayQuantity;
		
		/// <summary>
		/// �����ۼ�
		/// </summary>
		[FieldMapAttribute("WeekQuantity", typeof(int), 40, true)]
		public int WeekQuantity;
		
		/// <summary>
		/// �����ۼ�
		/// </summary>
		[FieldMapAttribute("MonthQuantity", typeof(int), 40, true)]
		public int MonthQuantity;
	}

	public class RPTCenterYield : DomainObject
	{
		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string OperationCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SHIFTDAY", typeof(int), 8, true)]
		public int ShiftDay;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("NGTIMES", typeof(int), 8, true)]
		public int NGTimes;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EATTRIBUTE2", typeof(int), 8, true)]
		public int Eattribute2;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("DayPercent", typeof(decimal), 40, true)]
		public decimal DayPercent;
		
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("WeekPercent", typeof(decimal), 40, true)]
		public decimal WeekPercent;
		
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MonthPercent", typeof(decimal), 40, true)]
		public decimal MonthPercent;
	}

	public class RPTCenterLRR : DomainObject
	{
		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("DayLRR", typeof(decimal), 40, true)]
		public decimal DayLRR;
		
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("WeekLRR", typeof(decimal), 40, true)]
		public decimal WeekLRR;
		
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MonthLRR", typeof(decimal), 40, true)]
		public decimal MonthLRR;
	}

	public class RPTCenterTPT : DomainObject
	{
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("Mo_MOCode", typeof(string), 40, true)]
		public string Mo_MOCode;

		/// <summary>
		/// ��Ʒ
		/// </summary>
		[FieldMapAttribute("Mo_ItemCode", typeof(string), 40, true)]
		public string Mo_ItemCode;
		
		/// <summary>
		/// Ͷ������
		/// </summary>
		[FieldMapAttribute("Mo_StartDate", typeof(int), 40, true)]
		public int Mo_StartDate;
		
		/// <summary>
		/// Ԥ���������
		/// </summary>
		[FieldMapAttribute("Mo_PlanEndDate", typeof(int), 40, true)]
		public int Mo_PlanEndDate;
		
		/// <summary>
		/// �ص�����
		/// </summary>
		[FieldMapAttribute("Mo_EndDate", typeof(int), 40, true)]
		public int Mo_EndDate;
		
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("Mo_DateNum", typeof(int), 40, true)]
		public int Mo_DateNum;
		
		/// <summary>
		/// ���ƻ�����
		/// </summary>
		[FieldMapAttribute("Mo_OverDateNum", typeof(int), 40, true)]
		public int Mo_OverDateNum;
		
		/// <summary>
		/// ״̬
		/// </summary>
		[FieldMapAttribute("Mo_Estate", typeof(string), 40, true)]
		public string Mo_Estate;
	}

	public class RPTCenterLong : DomainObject
	{
		/// <summary>
		/// ���к�
		/// </summary>
		[FieldMapAttribute("Ts_SN", typeof(string), 40, true)]
		public string Ts_SN;

		/// <summary>
		/// ����ά��վ����
		/// </summary>
		[FieldMapAttribute("Ts_ConfirmDate", typeof(int), 40, true)]
		public int Ts_ConfirmDate;
		
		/// <summary>
		/// ά������
		/// </summary>
		[FieldMapAttribute("Ts_Days", typeof(int), 40, true)]
		public int Ts_Days;
	}

	public class RPTCenterLine : DomainObject
	{
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode;

		/// <summary>
		/// ���ղ���
		/// </summary>
		[FieldMapAttribute("DayQuantity", typeof(int), 40, true)]
		public int DayQuantity;
		
		/// <summary>
		/// �����ۼ�
		/// </summary>
		[FieldMapAttribute("WeekQuantity", typeof(int), 40, true)]
		public int WeekQuantity;
		
		/// <summary>
		/// �����ۼ�
		/// </summary>
		[FieldMapAttribute("MonthQuantity", typeof(int), 40, true)]
		public int MonthQuantity;
	}

	public class RPTCenterWeekQuantity : DomainObject
	{
		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode;

		/// <summary>
		/// ���ղ���
		/// </summary>
		[FieldMapAttribute("DayQuantity", typeof(int), 40, true)]
		public int DayQuantity;
		
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("ShiftDay", typeof(int), 40, true)]
		public int ShiftDay;
	}

	public class RPTCenterMonthQuantity : DomainObject
	{
		/// <summary>
		/// ���δ���
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string SegmentCode;

		/// <summary>
		/// ���ղ���
		/// </summary>
		[FieldMapAttribute("DayQuantity", typeof(int), 40, true)]
		public int DayQuantity;
		
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("ShiftDay", typeof(int), 40, true)]
		public int ShiftDay;
	}

	public class RPTCenterMocode : DomainObject
	{
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string MoCode;

		/// <summary>
		/// ��Ʒ
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode;
		
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("ItemName", typeof(string), 40, true)]
		public string ItemName;
		
		/// <summary>
		/// ���ղ���
		/// </summary>
		[FieldMapAttribute("DayQuantity", typeof(int), 40, true)]
		public int DayQuantity;
		
		/// <summary>
		/// �����ƻ�
		/// </summary>
		[FieldMapAttribute("PlanQTY", typeof(int), 40, true)]
		public int PlanQTY;
		
		/// <summary>
		/// �����ۼ�
		/// </summary>
		[FieldMapAttribute("ActQTY", typeof(int), 40, true)]
		public int ActQTY;
	}

	public class RPTCenterWeekMocode : DomainObject
	{
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode;
		
		/// <summary>
		/// ���ղ���
		/// </summary>
		[FieldMapAttribute("DayQuantity", typeof(int), 40, true)]
		public int DayQuantity;
		
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("ShiftDay", typeof(int), 40, true)]
		public int ShiftDay;
	}

	public class RPTCenterMonthMocode : DomainObject
	{
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode;
		
		/// <summary>
		/// ���ղ���
		/// </summary>
		[FieldMapAttribute("DayQuantity", typeof(int), 40, true)]
		public int DayQuantity;
		
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("ShiftDay", typeof(int), 40, true)]
		public int ShiftDay;
	}

	public class RPTCenterResCode : DomainObject
	{
		/// <summary>
		/// ��Դ
		/// </summary>
		[FieldMapAttribute("ResCode", typeof(string), 40, true)]
		public string ResCode;
		
		/// <summary>
		/// ���ղ���
		/// </summary>
		[FieldMapAttribute("DayQuantity", typeof(int), 40, true)]
		public int DayQuantity;
		
		/// <summary>
		/// �ۼƲ���
		/// </summary>
		[FieldMapAttribute("ActQTY", typeof(int), 40, true)]
		public int ActQTY;
	}

	public class RPTCenterDayYield : DomainObject
	{
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string StepSequenceCode;

		/// <summary>
		/// ��Ʒ
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string ItemCode;

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("ItemName", typeof(string), 40, true)]
		public string ItemName;

		/// <summary>
		/// ��Դ
		/// </summary>
		[FieldMapAttribute("ResCode", typeof(string), 40, true)]
		public string ResCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("DayPercent", typeof(decimal), 40, true)]
		public decimal DayPercent;
	}

	public class RPTCenterWeekYield : DomainObject
	{
		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string OperationCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("DayPercent", typeof(decimal), 40, true)]
		public decimal DayPercent;
		
		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("ShiftDay", typeof(int), 40, true)]
		public int ShiftDay;
	}

	public class RPTCenterMonthYield : DomainObject
	{
		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string OperationCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("DayPercent", typeof(decimal), 40, true)]
		public decimal DayPercent;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("ShiftDay", typeof(int), 40, true)]
		public int ShiftDay;
	}

	public class RPTCenterDayPercent : DomainObject
	{
		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("ECode", typeof(string), 40, true)]
		public string ECode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("EcgCode", typeof(string), 40, true)]
		public string EcgCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("Ecdesc", typeof(string), 40, true)]
		public string Ecdesc;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("Qty", typeof(int), 40, true)]
		public int Qty;
	}

	public class RPTCenterWeekPercent : DomainObject
	{
		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("ECode", typeof(string), 40, true)]
		public string ECode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("EcgCode", typeof(string), 40, true)]
		public string EcgCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("Ecdesc", typeof(string), 40, true)]
		public string Ecdesc;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("Qty", typeof(int), 40, true)]
		public int Qty;
	}

	public class RPTCenterMonthPercent : DomainObject
	{
		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("ECode", typeof(string), 40, true)]
		public string ECode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("EcgCode", typeof(string), 40, true)]
		public string EcgCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("Ecdesc", typeof(string), 40, true)]
		public string Ecdesc;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("Qty", typeof(int), 40, true)]
		public int Qty;
	}

	public class RPTCenterDayProduct : DomainObject
	{
		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		[FieldMapAttribute("RCard", typeof(string), 40, true)]
		public string RCard;

		/// <summary>
		/// ��Ʒ
		/// </summary>
		[FieldMapAttribute("ItemCode", typeof(string), 40, true)]
		public string ItemCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("FrmSSCode", typeof(string), 40, true)]
		public string FrmSSCode;

		/// <summary>
		/// ����������Ա
		/// </summary>
		[FieldMapAttribute("FrmUser", typeof(string), 40, true)]
		public string FrmUser;

		/// <summary>
		/// �������ֹ���
		/// </summary>
		[FieldMapAttribute("FrmOPCode", typeof(string), 40, true)]
		public string FrmOPCode;

		/// <summary>
		/// ����������Դ
		/// </summary>
		[FieldMapAttribute("FrmResCode", typeof(string), 40, true)]
		public string FrmResCode;

		/// <summary>
		/// ������������
		/// </summary>
		[FieldMapAttribute("ShiftDay", typeof(int), 40, true)]
		public int ShiftDay;
	}

	public class RPTCenterWeekProduct : DomainObject
	{
		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		[FieldMapAttribute("RCard", typeof(string), 40, true)]
		public string RCard;

		/// <summary>
		/// ��Ʒ
		/// </summary>
		[FieldMapAttribute("ItemCode", typeof(string), 40, true)]
		public string ItemCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("FrmSSCode", typeof(string), 40, true)]
		public string FrmSSCode;

		/// <summary>
		/// ����������Ա
		/// </summary>
		[FieldMapAttribute("FrmUser", typeof(string), 40, true)]
		public string FrmUser;

		/// <summary>
		/// �������ֹ���
		/// </summary>
		[FieldMapAttribute("FrmOPCode", typeof(string), 40, true)]
		public string FrmOPCode;

		/// <summary>
		/// ����������Դ
		/// </summary>
		[FieldMapAttribute("FrmResCode", typeof(string), 40, true)]
		public string FrmResCode;

		/// <summary>
		/// ������������
		/// </summary>
		[FieldMapAttribute("ShiftDay", typeof(int), 40, true)]
		public int ShiftDay;
	}

	public class RPTCenterMonthProduct : DomainObject
	{
		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		[FieldMapAttribute("RCard", typeof(string), 40, true)]
		public string RCard;

		/// <summary>
		/// ��Ʒ
		/// </summary>
		[FieldMapAttribute("ItemCode", typeof(string), 40, true)]
		public string ItemCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("FrmSSCode", typeof(string), 40, true)]
		public string FrmSSCode;

		/// <summary>
		/// ����������Ա
		/// </summary>
		[FieldMapAttribute("FrmUser", typeof(string), 40, true)]
		public string FrmUser;

		/// <summary>
		/// �������ֹ���
		/// </summary>
		[FieldMapAttribute("FrmOPCode", typeof(string), 40, true)]
		public string FrmOPCode;

		/// <summary>
		/// ����������Դ
		/// </summary>
		[FieldMapAttribute("FrmResCode", typeof(string), 40, true)]
		public string FrmResCode;

		/// <summary>
		/// ������������
		/// </summary>
		[FieldMapAttribute("ShiftDay", typeof(int), 40, true)]
		public int ShiftDay;
	}

	public class RPTFactoryWeekCheck : DomainObject
	{
		/// <summary>
		/// ���к�
		/// </summary>
		[FieldMapAttribute("FactoryID", typeof(string), 40, true)]
		public string FactoryID;

		/// <summary>
		/// �����ܲ���
		/// </summary>
		[FieldMapAttribute("LastTotal", typeof(decimal), 10, true)]
		public decimal LastTotal;
		
		/// <summary>
		/// ����LRR
		/// </summary>
		[FieldMapAttribute("LastLRR", typeof(decimal), 10, true)]
		public decimal LastLRR;

		/// <summary>
		/// �����ܲ���
		/// </summary>
		[FieldMapAttribute("NowTotal", typeof(decimal), 10, true)]
		public decimal NowTotal;
		
		/// <summary>
		/// ����LRR
		/// </summary>
		[FieldMapAttribute("NowLRR", typeof(decimal), 10, true)]
		public decimal NowLRR;
	}

    public class RptAchievingRate : DomainObject
    {
        [FieldMapAttribute("SHIFTDAY", typeof(int), 22, true)]
        public int ShiftDay;

        [FieldMapAttribute("ACHIEVINGQTY", typeof(decimal), 10, true)]
        public decimal AchievingQty;

        [FieldMapAttribute("PLANQTY", typeof(decimal), 10, true)]
        public decimal PlanQty;

        [FieldMapAttribute("ACHIEVINGRATE", typeof(decimal), 10, true)]
        public decimal AchievingRate;
    } 

    public class RptMOCloseRate : DomainObject
    {
        [FieldMapAttribute("YEAR", typeof(int), 22, true)]
        public int Year;

        [FieldMapAttribute("DMONTH", typeof(int), 22, true)]
        public int Month;

        [FieldMapAttribute("DWEEK", typeof(int), 22, true)]
        public int Week;

        [FieldMapAttribute("FIRSTPQTY", typeof(int), 22, true)]
        public int FirstPQty;

        [FieldMapAttribute("OPENQTY", typeof(int), 22, true)]
        public int OpenQty;

        [FieldMapAttribute("CLOSEQTY", typeof(int), 22, true)]
        public int CloseQty;

        [FieldMapAttribute("ENDPQTY", typeof(int), 22, true)]
        public int EndPQty;

        [FieldMapAttribute("CLOSESORATE", typeof(decimal), 10, true)]
        public decimal CloseRate;
    } 
}
