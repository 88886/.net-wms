using System;
using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.SAPData
{
	#region ö������

	//��ȡ���ݵĲ���
	public enum DataName
	{
		SAPMO,			//����
		SAPMOBom,		//����Bom
		SAPBom			//BOM
	}

	//JobClass ��־����
	public enum JobClass
	{
		SAP ,		//SAP
		MESMOBILE,			//MESMOBILE		MES�ֻ�
		MESSMT,			    //MESSMT		MESʵװ(SMT)
		MESNOTEBOOK,		//MESNOTEBOOK	MES �ʼǱ�
		MESNOTEBOOKSMT		//MESNOTEBOOK	MES �ʼǱ�SMT
	}

	//JobName ��־����
	public enum JobName
	{
		BOM ,		//BOM
		MO ,		//����
		MOBom 		//����bom
	}

	//JobActionResult ������
	public enum JobActionResult
	{
		ADD ,			//����
		UPDATE ,		//�޸�
		IGNORE			//����
	}

	#endregion

	#region �������ݽӿ�

	//����ʵ��ӿ�,���д�SAP����ȡ�����ݱ���̳��Դ˽ӿ�
	public interface IImport
	{
	
	}

	#endregion

	#region SAP JobLog ����

	/// <summary>
	/// JobLog
	/// </summary>
	[Serializable, TableMap("joblog","JOBCLASS,JOBNAME,STARTTIME,ENDTIME")]
	public class JobLog : DomainObject
	{
		public JobLog()
		{
		}
		
		/// <summary>
		/// JOBCLASS job����
		/// </summary>
		[FieldMapAttribute("JOBCLASS", typeof(string), 15, true)]
		public string  JobClass;

		/// <summary>
		/// JOBNAME job����
		/// </summary>
		[FieldMapAttribute("JOBNAME", typeof(string), 50, true)]
		public string  JobName;

		/// <summary>
		/// STARTTIME ��ʼʱ��
		/// </summary>
		[FieldMapAttribute("STARTTIME", typeof(DateTime), 7, true)]
		public DateTime  StartTime;

		/// <summary>
		/// ENDTIME ����ʱ��
		/// </summary>
		[FieldMapAttribute("ENDTIME", typeof(DateTime), 7, true)]
		public DateTime  EndTime;

		/// <summary>
		/// RESULTCODE 
		/// </summary>
		[FieldMapAttribute("RESULTCODE", typeof(int), 13, true)]
		public int  Resultcode;

		/// <summary>
		/// RESULTDESC
		/// </summary>
		[FieldMapAttribute("RESULTDESC", typeof(string), 2000, true)]
		public string  Resultdesc;
		
	}

	#endregion

	#region SAPMO  SAP����

	/// <summary>
	/// ����
	/// </summary>
	[Serializable, TableMap("WO", "AUFNR")]
	public class SAPMO : DomainObject,IImport
	{
		public SAPMO()
		{
		}
 
		//����	 ����	�����Ϻ�	��������	��������	�ջ�����	������ʼ����	�����������	������λ
		//AUFNR	 WERKS	FMATNR	   AUART	    PSMNG	    WEMNG	    GSTRP	        GLTRP	         GMEIN
		//1000022724	1105	GSM6106SR04	PP01	1,583.00	1,583.00	2005.09.15	2005.09.19	EA

		//�������ݸ�ʽ
		//���������� ,��������,��Ʒ���Ϻ�,�������� ,��������,Ԥ�ƿ�ʼ���� ,Ԥ��������� ,�ͻ�����,�ͻ����� ,��ע

		#region ӳ��MO���ֶ�

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("AUFNR", typeof(string), 12, true)]
		public string  MOCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("WERKS", typeof(string), 4, false)]
		public string  Factory;

		/// <summary>
		/// �����Ϻ�
		/// </summary>
		[FieldMapAttribute("FMATNR", typeof(string), 18, true)]
		public string  ItemCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("AUART", typeof(string), 4, true)]
		public string  MOType;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("PSMNG", typeof(string), 13, true)]
		public string  MOPlanQty;

		/// <summary>
		/// ������ʼ����
		/// </summary>
		[FieldMapAttribute("GSTRP", typeof(DateTime), 8, true)]
		public DateTime  MOPlanStartDate;

		/// <summary>
		/// �����������
		/// </summary>
		[FieldMapAttribute("GLTRP", typeof(DateTime), 8, true)]
		public DateTime  MOPlanEndDate;

		#endregion

		/// <summary>
		/// �ջ�����
		/// </summary>
		[FieldMapAttribute("WEMNG", typeof(string), 13, true)]
		public string  MOOutputQty;

		/// <summary>
		/// ������λ
		/// </summary>
		[FieldMapAttribute("GMEIN", typeof(string), 3, true)]
		public string  ItemUOM;
		
	}


	#endregion

	#region SAPMOBom  SAP ����bom

	/// <summary>
	/// SAP ��Ʒ
	/// </summary>
	[Serializable, TableMap("WODETAIL", "AUFNR")]
	public class SAPMOBom : DomainObject,IImport
	{
		public SAPMOBom()
		{
		}

		#region sap WODETAIL

		//������	���Ϻ���	���Ϲ���	����	��������	��������	�����	���ȼ�	��������	������λ
		//AUFNR		MATNR		PWERK		VORNR	BDMNG		ENMNG		ALPGR	ZPRI	ZFLAG		GMEIN

		#endregion

		#region ӳ��MOBom���ֶ�

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("AUFNR", typeof(string), 12, true)]
		public string  MOCode;

		/// <summary>
		/// �����Ϻ�
		/// </summary>
		[FieldMapAttribute("FMATNR", typeof(string), 18, true)]
		public string  ItemCode;

		/// <summary>
		/// ���Ϻ���
		/// </summary>
		[FieldMapAttribute("MATNR", typeof(string), 18, true)]
		public string  MOBOMItemCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("maktx", typeof(string), 40, true)]
		public string  MOBOMItemName;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("ZFLAG", typeof(string), 1, true)]
		public string  MOBOMItemType;

		/// <summary>
		/// ������λ
		/// </summary>
		[FieldMapAttribute("GMEIN", typeof(string), 6, true)]
		public string  MOBOMItemUOM;

		#endregion

		/// <summary>
		/// ���Ϲ���
		/// </summary>
		[FieldMapAttribute("PWERK", typeof(string), 4, true)]
		public string  Factory;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("VORNR", typeof(string), 4, true)]
		public string  OPCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("BDMNG", typeof(int), 13, true)]
		public int  RequireQty;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("ENMNG", typeof(int), 13, true)]
		public int  DrawQTY;

		/// <summary>
		/// �����
		/// </summary>
		[FieldMapAttribute("ALPGR", typeof(string), 10, true)]
		public string  PItem;

		/// <summary>
		/// ���ȼ�
		/// </summary>
		[FieldMapAttribute("ZPRI", typeof(string), 2, true)]
		public string  PRI;

		/// <summary>
		/// �����ƻ�����
		/// </summary>
		public decimal  MOPlanQty = 0;

		/// <summary>
		/// ��������
		/// </summary>
		public decimal UnitageQty
		{
			get
			{
				if(this.MOPlanQty!=0)
				{
					return Convert.ToDecimal(this.RequireQty/this.MOPlanQty);
				}
				return 0;
			}
		}

	}


	#endregion

	#region SAPBOM  SAP BOM

	/// <summary>
	/// SAP BOM
	/// </summary>
	[Serializable, TableMap("BOM", "AUFNR")]
	public class SAPBOM : DomainObject,IImport
	{
		public SAPBOM()
		{
		}
		

		//��������	�����Ϲ���	�����Ϻ�	�����Ϲ���	��Ч����	ʧЧ����	��λ	��������	�����Ŀ��	 ʹ�ÿ�����	����		����������	��������	ECN	   ���ȼ�
		//FMATNR	FWERKS		MATNR		PWERK		DATUV		DATUB		GMEIN	MENGE		ALPGR		 EWAHR		VORNR		LASTUPDATE		ZFLAG		AENNR  ZPRI

		#region ӳ��BOM���ֶ�

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("FMATNR", typeof(string), 18, true)]
		public string  FItemCode;

		/// <summary>
		/// �����Ϻ�
		/// </summary>
		[FieldMapAttribute("MATNR", typeof(string), 13, true)]
		public string  SBOMItemCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MAKTX", typeof(string), 40, true)]
		public string  SOBOMItemName;

		/// <summary>
		/// ��Ч����
		/// </summary>
		[FieldMapAttribute("DATUV", typeof(DateTime), 7, true)]
		public DateTime  SBOMItemEffectiveDate;

		/// <summary>
		/// ʧЧ����
		/// </summary>
		[FieldMapAttribute("DATUB", typeof(DateTime), 7, true)]
		public DateTime  SBOMItemInvalidDate;

		/// <summary>
		/// ��λ
		/// </summary>
		[FieldMapAttribute("GMEIN", typeof(string), 3, true)]
		public string  ItemUOM;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MENGE", typeof(decimal), 15, true)]
		public decimal  SBOMItemQty;

		/// <summary>
		/// �����Ŀ��
		/// </summary>
		[FieldMapAttribute("ALPGR", typeof(string), 10, true)]
		public string ALPGR = string.Empty;

		/// <summary>
		/// ECN
		/// </summary>
		[FieldMapAttribute("AENNR", typeof(string), 12, true)]
		public string  SBOMItemECN;

		#endregion

		/// <summary>
		/// �����Ϲ���
		/// </summary>
		[FieldMapAttribute("FWERKS", typeof(string), 4, true)]
		public string  FFacatory;

		
		/// <summary>
		/// �����Ϲ���
		/// </summary>
		[FieldMapAttribute("PWERK", typeof(string), 13, true)]
		public string  SFactory;
		

		/// <summary>
		/// ʹ�ÿ�����
		/// </summary>
		[FieldMapAttribute("EWAHR", typeof(string), 13, true)]
		public string  EWAHR;


		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("VORNR", typeof(string), 13, true)]
		public string  OP;

		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("LASTUPDATE", typeof(DateTime), 7, true)]
		public DateTime  LastUpdate;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("ZFLAG", typeof(string), 1, true)]
		public string  ItemType;

		/// <summary>
		/// ���ȼ�
		/// </summary>
		[FieldMapAttribute("ZPRI", typeof(string), 2, true)]
		public string  ZPRI;
		
	}

	#endregion

	#region SAPBOM  SAP Item

	/// <summary>
	/// SAP BOM
	/// </summary>
	[Serializable, TableMap("ITEM", "MATNR,WERKS")]
	public class SAPItem : DomainObject,IImport
	{
		public SAPItem()
		{
		}
		//���Ϲ���	�����Ϻ�	��������
		//WERKS		MATNR		MAKTX	

		/// <summary>
		/// �����Ϻ�
		/// </summary>
		[FieldMapAttribute("MATNR", typeof(string), 18, true)]
		public string  MATNR;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("MAKTX", typeof(string), 40, true)]
		public string  MAKTX;

		/// <summary>
		/// �����Ϲ���
		/// </summary>
		[FieldMapAttribute("WERKS", typeof(string), 4, true)]
		public string  WERKS;
		
	}

	#endregion
	
	
}
