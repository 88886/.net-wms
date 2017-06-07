#region System
using System;
using System.Text;
using System.Runtime.Remoting;  
using System.Collections;
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Common.Helper;  
using BenQGuru.eMES.Common.DomainDataProvider;   
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Rework;

#endregion
 

namespace BenQGuru.eMES.SAPData
{
	/// <summary>
	/// SAPMaper ����ӳ�� SAP�����ݶ���ӳ�䵽MES�Ķ���
	/// </summary>
	public class SAPMapper
	{
		private static int MOBomSeq=0;
		
		public SAPMapper()
		{
		}


		private bool EffientDeleteMOPrifix  = true;	//true ��ʾʹ��2λ0��ǰ��0��ȡ,Ч�ʸ�; false��ʾ��λǰ��0�Ľ�ȡ,Ч�ʵ�,Ĭ��2λǰ��0��ȡ

		public static object[] MOTypeParams;		//�������Ͳ��� ��Ϊ����������ʾ�Ĺ������Ϳ����Զ���,�����Ҫ��MES���ݿ��л�ȡ.

		/// <summary>
		/// ӳ��SAP ʵ�� �� MES ʵ��
		/// </summary>
		/// <returns></returns>
		public ArrayList MapSAPData(object[] SAPDatas)
		{
			#region �����ж� �����ж�

			if(SAPDatas==null) return new ArrayList();
			if(!(SAPDatas.Length>0)) return new ArrayList();

			if(!(SAPDatas[0] is IImport)) return new ArrayList();;

			if(!(SAPDatas[0] is DomainObject))
			{
				ExceptionManager.Raise(this.GetType(),"$Error_Mapper_Parameter",string.Empty);//�������ʹ���
			}

			#endregion

			ArrayList returnMESObjs = new ArrayList(SAPDatas.Length);	//ӳ�䷵�ص�MES ����

			#region ӳ�乤������
			Type DomainType = SAPDatas[0].GetType();
			if(DomainType == typeof(SAPMO))
			{
				returnMESObjs	= this.MapSAPMO(SAPDatas);			//SAP����
			}
			else if(DomainType == typeof(SAPMOBom))
			{
				returnMESObjs	= this.MapSAPItem(SAPDatas);		//SAP��Ʒ
			}
			else if(DomainType == typeof(SAPBOM))
			{
				returnMESObjs	= this.MapSAPBom(SAPDatas);			//SAPBOM
			}


			#endregion
				
			return returnMESObjs;
		}

		#region ӳ����� ����

		/// <summary>
		/// ӳ��SAP������MES����
		/// </summary>
		/// <returns></returns>
		private ArrayList MapSAPMO(object[] SAPMOObjs)
		{
			#region �����ж� �����ж�

			if(SAPMOObjs==null) return new ArrayList();
			if(!(SAPMOObjs.Length>0)) return new ArrayList();

			if(!(SAPMOObjs[0] is IImport)) return new ArrayList();;

			if(SAPMOObjs[0].GetType() != typeof(SAPMO))
			{
				ExceptionManager.Raise(this.GetType(),"$Error_Mapper_Parameter",string.Empty);//�������ʹ���
			}

			#endregion

			ArrayList mesObjs = new ArrayList(SAPMOObjs.Length);	//ӳ�䷵�ص�MES ����

			#region ӳ��MO���ֶ�
			
			foreach(SAPMO _sapEntity in SAPMOObjs)
			{
				MO _mesEntity = new MO();
				_mesEntity.MOCode			= this.DeletePrifix(_sapEntity.MOCode);//_sapEntity.MOCode;							//������ ��Ҫȥ��ǰ��0
				_mesEntity.Factory			= _sapEntity.Factory;							//����
				_mesEntity.ItemCode			= _sapEntity.ItemCode;							//�����Ϻ�
				_mesEntity.MOType			= _sapEntity.MOType;//this.GetMOType(_sapEntity.MOType);							//��������(��Ҫ��sap����ת����mes����)
				_mesEntity.MOPlanQty		= decimal.Parse(_sapEntity.MOPlanQty);							//��������
				_mesEntity.MOPlanStartDate	= FormatHelper.TODateInt(_sapEntity.MOPlanStartDate);					//������ʼ����
				_mesEntity.MOPlanEndDate	= FormatHelper.TODateInt(_sapEntity.MOPlanEndDate);						//�����������
				
				_mesEntity.MOStatus			= MOManufactureStatus.MOSTATUS_INITIAL;				//״̬Ĭ��Ϊ��ʼ
				_mesEntity.MODownloadDate	= FormatHelper.TODateInt(DateTime.Today);
				_mesEntity.MOUser			= "ADMIN";							//��Ϊϵͳ�Զ�ִ��Ĭ�Ͽ�����ԱΪADMIN	
				_mesEntity.MaintainUser     = "ADMIN";								//��Ϊϵͳ�Զ�ִ��Ĭ��ά����ԱΪADMIN	
				_mesEntity.MaintainDate     = FormatHelper.TODateInt(DateTime.Today);
				_mesEntity.MaintainTime     = FormatHelper.TOTimeInt(DateTime.Now);
				_mesEntity.MOImportDate     = FormatHelper.TODateInt(DateTime.Today);
				_mesEntity.MOImportTime     = FormatHelper.TOTimeInt(DateTime.Now);
				_mesEntity.MOVersion		= "1";
				_mesEntity.IsControlInput	= "0";
				_mesEntity.IsBOMPass		= "0";
				//_mesEntity.MOOffQty         = 0;						//���빤������Ĭ�ϣ�
				_mesEntity.IDMergeRule      = 1;						//�ְ����Ĭ����1
                _mesEntity.OrganizationID = GlobalVariables.CurrentOrganizations.First().OrganizationID;
				//������û��ӳ����ֶ�
				//				_mesEntity.CustomerOrderNO = FormatHelper.CleanString(this.txtCustomerOrderNOEdit.Text, 40);
				//				_mesEntity.CustomerCode = FormatHelper.CleanString(this.txtCustomerCodeEdit.Text, 40);
				//				_mesEntity.MOActualStartDate = (this.txtActualStartDateEdit.Text == "") ? 10101: FormatHelper.TODateInt(this.txtActualStartDateEdit.Text);		//modify by Simone
				//				_mesEntity.MOActualEndDate = (this.txtActualEndDateEdit.Text == "") ? 99991231: FormatHelper.TODateInt(this.txtActualEndDateEdit.Text);
				
				//				_mesEntity.IDMergeRule = System.Int32.Parse(FormatHelper.CleanString(this.txtDenominatorEdit.Text));

				mesObjs.Add(_mesEntity);
			}

			#endregion
				
			return mesObjs;
		}

		//ȥ��SAP ����Code��ǰ��0 (Ŀǰ��2��0)
		private string DeletePrifix(string woCode)
		{
			if(woCode.Length>2)
			{
				//ֻ�������ǰ��0���㷨
				string moCode = woCode;
				if(EffientDeleteMOPrifix)
				{
					moCode = woCode.Substring(2,woCode.Length-2);	//ֻ�������ǰ��0���㷨
				}
				else
				{
					moCode = this.DeletePrifix2(woCode);			//��Զ��ǰ��0���㷨
				}
				return moCode;
			}
			return woCode;
		}

		//��Զ��ǰ��0���㷨,�Ѿ�����ͨ��,��ʱ����,��ΪЧ������
		private string DeletePrifix2(string woCode)
		{
			//��Զ��ǰ��0���㷨
			int notZeroPosition = 0; //����߼���,����0�ĵ�һ���ַ���λ��
			for(int i=0;i<woCode.Length;i++)
			{
				if(woCode[i].CompareTo('0') != 0)
				{
					notZeroPosition = i;
					break;
				}
			}
			string moCode = woCode.Substring(notZeroPosition,woCode.Length-notZeroPosition);
			return moCode;
		}

		#endregion

		#region ӳ����� BOM

		/// <summary>
		/// ӳ��SAP BOM��MES BOM
		/// </summary>
		/// <returns></returns>
		private ArrayList MapSAPBom(object[] SAPBOMObjs)
		{
			#region �����ж� �����ж�

			if(SAPBOMObjs==null) return new ArrayList();
			if(!(SAPBOMObjs.Length>0)) return new ArrayList();

			if(!(SAPBOMObjs[0] is IImport)) return new ArrayList();;

			if(SAPBOMObjs[0].GetType() != typeof(SAPBOM))
			{
				ExceptionManager.Raise(this.GetType(),"$Error_Mapper_Parameter",string.Empty);//�������ʹ���
			}

			#endregion

			ArrayList mesObjs = new ArrayList(SAPBOMObjs.Length);	//ӳ�䷵�ص�MES ����

			#region ӳ��BOM���ֶ�
			//SAPBOM ���� ITEMCODE,Sequence
			int seq = 1;
			string lastALPGR = string.Empty;											
			
			foreach(SAPBOM _sapEntity in SAPBOMObjs)
			{
				SBOM _mesEntity = new SBOM();

                _mesEntity.OrganizationID           = GlobalVariables.CurrentOrganizations.First().OrganizationID;

				_mesEntity.ItemCode					= _sapEntity.FItemCode;					//��Ʒ����
				_mesEntity.SBOMItemECN				= _sapEntity.SBOMItemECN;
				_mesEntity.SBOMItemCode				= _sapEntity.SBOMItemCode;									//�ӽ���
				_mesEntity.SBOMSourceItemCode		= _sapEntity.SBOMItemCode ;									//��ѡ��
				_mesEntity.SBOMItemName				= _sapEntity.SOBOMItemName;										
				_mesEntity.SBOMItemEffectiveDate	= FormatHelper.TODateInt(_sapEntity.SBOMItemEffectiveDate.Date);
				_mesEntity.SBOMItemInvalidDate		= FormatHelper.TODateInt(_sapEntity.SBOMItemInvalidDate.Date);
				_mesEntity.ALPGR					= _sapEntity.ALPGR;											//�����Ŀ��

				_mesEntity.SBOMItemEffectiveTime	= _sapEntity.SBOMItemEffectiveDate.Hour * 10000 + _sapEntity.SBOMItemEffectiveDate.Minute * 100 + _sapEntity.SBOMItemEffectiveDate.Second;
				_mesEntity.SBOMItemInvalidTime		= _sapEntity.SBOMItemInvalidDate.Hour * 10000 + _sapEntity.SBOMItemInvalidDate.Minute * 100 + _sapEntity.SBOMItemInvalidDate.Second;
				
				_mesEntity.SBOMItemControlType		= BOMItemControlType.ITEM_CONTROL_NOCONTROL;	//���������Ĭ���ǲ��ܹܿصģ����û����о�������lot�ܿػ���keyparts�ܿ�
				_mesEntity.SBOMItemStatus			= "0";
				_mesEntity.SBOMItemQty				= _sapEntity.SBOMItemQty;
				if(_sapEntity.ALPGR!=null && _sapEntity.ALPGR != lastALPGR)
				{
					lastALPGR = _sapEntity.ALPGR;	
					seq = 1;
				}
				_mesEntity.Sequence					= seq++;										//Sequence ������Ϊ����
				_mesEntity.SBOMItemUOM				= (_sapEntity.ItemUOM == string.Empty) ? "EA":_sapEntity.ItemUOM;

				_mesEntity.MaintainDate				= FormatHelper.TODateInt(DateTime.Today);
				_mesEntity.MaintainTime				= FormatHelper.TOTimeInt(DateTime.Now);
				_mesEntity.MaintainUser				= "ADMIN";
				_mesEntity.MaintainDate				= FormatHelper.TODateInt(DateTime.Today);
				_mesEntity.MaintainTime				= FormatHelper.TOTimeInt(DateTime.Now);

				//������û��ӳ����ֶ�

				mesObjs.Add(_mesEntity);

				
			}

			#endregion
				
			return mesObjs;
		}

		#endregion

		#region ӳ����� ��������

		
		/// <summary>
		/// ӳ��SAP ����bom��MES����bom
		/// </summary>
		/// <returns></returns>
		private ArrayList MapSAPItem(object[] SAPItemObjs)
		{
			#region �����ж� �����ж�

			if(SAPItemObjs==null) return new ArrayList();
			if(!(SAPItemObjs.Length>0)) return new ArrayList();

			if(!(SAPItemObjs[0] is IImport)) return new ArrayList();;

			if(SAPItemObjs[0].GetType() != typeof(SAPMOBom))
			{
				ExceptionManager.Raise(this.GetType(),"$Error_Mapper_Parameter",string.Empty);//�������ʹ���
			}

			#endregion

			ArrayList mesObjs = new ArrayList(SAPItemObjs.Length);	//ӳ�䷵�ص�MES ����

			#region ӳ��MOBOM���ֶ�
			int seq = 0;
			foreach(SAPMOBom _sapEntity in SAPItemObjs)
			{
				MOBOM _mesEntity = new MOBOM();

				//WODETAIL ���ݽṹ
				//������	���Ϻ���	���Ϲ���	����	��������	��������	�����	���ȼ�	��������	������λ
				//AUFNR		MATNR		PWERK		VORNR	BDMNG		ENMNG		ALPGR	ZPRI	ZFLAG		GMEIN

				_mesEntity.MOCode			= this.DeletePrifix(_sapEntity.MOCode);//_sapEntity.MOCode;							//������ ��Ҫȥ��ǰ��0
				
				_mesEntity.MOBOMItemCode = _sapEntity.MOBOMItemCode;
				_mesEntity.MOBOMItemName = _sapEntity.MOBOMItemName;				
				_mesEntity.MOBOMItemQty = _sapEntity.UnitageQty;					//��������
				_mesEntity.MOBOMSourceItemCode = _sapEntity.PItem;					//�����
				_mesEntity.MOBOMItemUOM = _sapEntity.MOBOMItemUOM;					//������λ
				_mesEntity.OPCode = _sapEntity.OPCode;								//����Code

				_mesEntity.MOBOMItemStatus = "0";									// 1 -  ʹ����  0 -  ����  -1 - ��ɾ��
				_mesEntity.MaintainDate = FormatHelper.TODateInt(DateTime.Today);	//���ά������
				_mesEntity.MaintainTime = FormatHelper.TOTimeInt(DateTime.Today);	//���ά��ʱ��
				_mesEntity.MaintainUser = "ADMIN";									//ά����Ա	Ĭ��ΪADMIN
				_mesEntity.MOBOMItemVersion = "0";									//�ӽ��ϰ汾 Ĭ��Ϊ0	
				_mesEntity.MOBOMItemEffectiveDate = 20000101;						//��Ч����
				_mesEntity.MOBOMItemEffectiveTime = 0;								//��Чʱ��
				_mesEntity.MOBOMItemInvalidDate = 21001229;							//ʧЧ����
				_mesEntity.MOBOMItemInvalidTime = 235959;							//ʧЧʱ��
				_mesEntity.Sequence = ++MOBomSeq;										//���
				++seq;

				_mesEntity.MOBOMItemECN = "";
				_mesEntity.MOBOMItemLocation = "";
				_mesEntity.MOBOMItemDescription = "";								//�ӽ�������
				_mesEntity.MOBOMItemControlType = "";								//��������
				
				_mesEntity.ItemCode = _sapEntity.ItemCode;							//��Ʒ����
				

				//������û��ӳ����ֶ�

				mesObjs.Add(_mesEntity);
			}

			#endregion
				
			return mesObjs;
		}


		#endregion

		#region ��������ӳ�� ��ʱ����

//		private string GetMOType(string WOType)
//		{
//			//�������ͱ���	��������
//			//PP01	��������
//			//PP02	���ƶ���
//			//PP03	���޶���
//			//PP04	��������
//			
//			string moType =BenQGuru.eMES.Web.Helper.MOType.MOTYPE_NORMALMOTYPE;//Ĭ������������
//
//			switch(WOType.Trim())
//			{
//				case "PP01" :						//PP01	��������
//					moType = "NORMAL";//BenQGuru.eMES.Web.Helper.MOType.MOTYPE_NORMALMOTYPE;
//					break;
//				case "PP02" :						//PP02	���ƶ���
//					moType ="NORMAL"; //BenQGuru.eMES.Web.Helper.MOType.MOTYPE_NORMALMOTYPE;				//MES ��û�����ƶ���, ��ʱ����������
//					break;
//				case "PP03" :						//PP03	���޶��� (ÿ�´󷵹�����)
//					moType ="MONTHREWORK"; // BenQGuru.eMES.Web.Helper.MOType.MOTYPE_MONTHREWORKMOTYPE;				//MES ��û�з��޶���, ��ʱ��ÿ�´󷵹�����
//					break;
//				case "PP04" :						//PP04	��������
//					moType ="REWORK"; // BenQGuru.eMES.Web.Helper.MOType.MOTYPE_REWORKMOTYPE;
//					break;
//				default:
//					moType ="NORMAL"; // BenQGuru.eMES.Web.Helper.MOType.MOTYPE_NORMALMOTYPE;
//					break;
//			}
//
//			return moType;
//		}

		#endregion

		#region ��������ӳ�� (δ���)

		private string GetMaterialType(string WOMaterialType)
		{
			//��������	������������
			//FERT	��Ʒ
			//HALB	���Ʒ
			//ROH	ԭ����

			string materialType = "ITEMTYPE_FINISHEDPRODUCT";		//Ĭ���ǳ�Ʒ

			switch(WOMaterialType.Trim())
			{
				case "FERT" :						//FERT	��Ʒ
					materialType = "ITEMTYPE_FINISHEDPRODUCT";
					break;
				case "HALB" :						//HALB	���Ʒ
					materialType = "ITEMTYPE_SEMIMANUFACTURE";				
					break;
				case "ROH" :						//ROH	ԭ����
					materialType = "ITEMTYPE_RAWMATERIAL";				
					break;
			}

			return materialType;
		}

		#endregion

		


		

	}
}
