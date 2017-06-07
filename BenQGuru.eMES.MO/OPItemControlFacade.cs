#region System
using System;
using System.Runtime.Remoting;  
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Common.Helper;  
using BenQGuru.eMES.Common.DomainDataProvider;   
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Web.Helper;
#endregion
   
/// OPItemControlFacade ��ժҪ˵����
/// �ļ���:
/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
/// ������:Crystal Chu
/// ��������:2005/03/22
/// �޸���:crystal chu
/// �޸�����:
/// �� ��: ��OPBOMItemControl���õĲ�������
///        crystal chu 2005/04/20 �ڶ�OPItem���޸ĺ����ӣ�ɾ����ʱ�����Ӧ��OPBOM�Ƿ���ʹ����
///           
/// �� ��:	
/// </summary>    
namespace BenQGuru.eMES.MOModel
{
	public class OPItemControlFacade:MarshalByRefObject
	{
		//private static readonly log4net.ILog _log = BenQGuru.eMES.Common.Log.GetLogger(typeof(MOFacade));
		private  IDomainDataProvider _domainDataProvider= null;
		private  FacadeHelper _helper					= null;

		public override object InitializeLifetimeService()
		{
			return null;
		}

		public OPItemControlFacade()
		{
			this._helper = new FacadeHelper(DataProvider);
		}

		public OPItemControlFacade(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
			this._helper = new FacadeHelper(DataProvider);
		}

		protected IDomainDataProvider DataProvider
		{
			get
			{
				if (_domainDataProvider == null)
				{
					_domainDataProvider = DomainDataProviderManager.DomainDataProvider(); 
				}
				return _domainDataProvider;
			}	
		}


		public OPItemControl CreateNewOPItemControl()
		{
			return new OPItemControl();
		}

		/// <summary>
		/// ���opitemcontrol,�������ж���ͬ�Ŀ�����Ϣ�Ƿ��Ѿ�����ӣ����û����ӣ���
		/// ���е�sequenceȡһ�������Ϣ�������sequence��1
		/// ** nunit
		/// </summary>
		/// <param name="opItemControl"></param>
		public void AddOPItemControl(OPItemControl opItemControl)
		{
			if(opItemControl == null)
			{
				ExceptionManager.Raise(this.GetType(),"$Error_Null_Paramter");
				//				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),String.Format(ErrorCenter.ERROR_ARGUMENT_NULL,"opItemControl")));
			}

			//add by crystal chu 2005/04/20 OPBOM�Ƿ���ʹ����
			OPBOMFacade _opBOMFacade = new OPBOMFacade(this.DataProvider);
			_opBOMFacade.OPBOMChangedCheck(opItemControl.ItemCode, opItemControl.OPBOMCode,opItemControl.OPBOMVersion,GlobalVariables.CurrentOrganizations.First().OrganizationID);

			object[] objs = QueryOPBOMItemControl(opItemControl.ItemCode,opItemControl.OPBOMItemCode,opItemControl.OPBOMCode,opItemControl.OPBOMVersion,opItemControl.OPID,
				opItemControl.DateCodeStart,opItemControl.DateCodeEnd,opItemControl.VendorCode,opItemControl.VendorItemCode,opItemControl.ItemVersion,
				opItemControl.BIOSVersion,opItemControl.PCBAVersion,opItemControl.CardStart,opItemControl.CardEnd);
			if(objs != null)
			{
				ExceptionManager.Raise(this.GetType(),"$Error_OPItemControl_Exist");
				//				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPItemControlFacade),String.Format(ErrorCenter.ERROR_ADDOPITEMCONTROL, opItemControl.OPBOMItemCode)));
			}
			objs = QueryOPBOMItemControl(opItemControl.ItemCode,opItemControl.OPBOMItemCode,opItemControl.OPBOMCode,opItemControl.OPBOMVersion,opItemControl.OPID,string.Empty,
				string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty);
			int sequence = 0;
			if(objs != null)
			{
				sequence = System.Int32.Parse(((OPItemControl)objs[objs.Length-1]).Sequence.ToString())+1;
			}
			try
			{
				//Laws Lu,2006/11/13 uniform system collect date
				DBDateTime dbDateTime;
						
				dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

				opItemControl.MaintainDate = dbDateTime.DBDate;
				opItemControl.MaintainTime = dbDateTime.DBTime;

				opItemControl.Sequence = sequence;
				this.DataProvider.Insert(opItemControl); 
			}
			catch(Exception ex)
			{
				ExceptionManager.Raise(this.GetType(),"$Error_AddOPItemControl",String.Format("$OPBOMItemcode='{0}'",opItemControl.OPBOMItemCode),ex);
				//				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPItemControl), String.Format(ErrorCenter.ERROR_ADDOPITEMCONTROL, opItemControl.OPBOMItemCode)), ex);
			}
		}

		/// <summary>
		/// ** nunit
		/// </summary>
		/// <param name="opItemControl"></param>
		public void UpdateItemControl(OPItemControl opItemControl)
		{
			//add by crystal chu 2005/04/20 OPBOM�Ƿ���ʹ����
			OPBOMFacade _opBOMFacade = new OPBOMFacade(this.DataProvider);
			_opBOMFacade.OPBOMChangedCheck(opItemControl.ItemCode,opItemControl.OPBOMCode,opItemControl.OPBOMVersion,GlobalVariables.CurrentOrganizations.First().OrganizationID);
			this._helper.UpdateDomainObject(opItemControl);
		}

		/// <summary>
		/// �жϸ�bom�Ƿ��Ѿ�������ʹ�ã����ʹ�����Ӧopbom������Ʒ�Ŀ��������򲻿��Ա�ɾ��
		/// ** nunit
		/// </summary>
		/// <param name="opItemControl"></param>
		public void DeleteItemControl(OPItemControl opItemControl)
		{
			if(opItemControl == null)
			{
				ExceptionManager.Raise(this.GetType(),"$Error_Null_Paramter");
				//				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),String.Format(ErrorCenter.ERROR_ARGUMENT_NULL,"opItemControl")));
			}

			//add by crystal chu 2005/04/20 OPBOM�Ƿ���ʹ����
			OPBOMFacade _opBOMFacade = new OPBOMFacade(this.DataProvider);
			_opBOMFacade.OPBOMChangedCheck(opItemControl.ItemCode, opItemControl.OPBOMCode,opItemControl.OPBOMVersion,GlobalVariables.CurrentOrganizations.First().OrganizationID);

			try
			{
				this.DataProvider.Delete(opItemControl); 
			}
			catch(Exception ex)
			{
				ExceptionManager.Raise(this.GetType(),"$Error_DeleteItemControl",String.Format("[$OPBOMItemcode='{0}']",opItemControl.OPBOMItemCode),ex);
				//				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPItemControl), string.Format(ErrorCenter.ERROR_DELETEITEMCONTROL,opItemControl.OPBOMItemCode)), ex);
			}
		}



		/// <summary>
		/// ɾ��itemcontrol��Ⱥ�����������
		/// </summary>
		/// <param name="opItemControls"></param>
		public void DeleteItemControl(OPItemControl[] opItemControls)
		{
			try
			{
				this.DataProvider.BeginTransaction();
				for(int i=0;i<opItemControls.Length;i++)
				{
					DeleteItemControl(opItemControls[i]);
				}
				this.DataProvider.CommitTransaction();
			}
			catch(Exception ex)
			{
				//_log.Error(ex.Message,ex);
				this.DataProvider.RollbackTransaction();
				ExceptionManager.Raise(this.GetType(),"$Error_DeleteItemControl",ex);
				//				throw ex;
			}
		}




		/// <summary>
		/// 
		/// �жϸÿ�����Ϣ�Ƿ��Ѿ�����
		/// </summary>
		/// <param name="itemCode"></param>
		/// <param name="BOMItemCode"></param>
		/// <param name="OPBOMCode"></param>
		/// <param name="OPBOMVersion"></param>
		/// <returns></returns>
		public bool IsOPBOMItemControlExist(string itemCode,string BOMItemCode,string OPBOMCode,string OPBOMVersion)
		{
			string selectSQL = "select count(*) from tblopitemcontrol where 1=1 {0} order by seq";
			string tmpString = string.Empty;
			if((itemCode != string.Empty)&&(itemCode.Trim() != string.Empty))
			{
				tmpString += " and itemcode ='"+itemCode.Trim()+"'";
			}
			if((BOMItemCode != string.Empty)&&(BOMItemCode.Trim() != string.Empty))
			{
				tmpString += " and obitemcode ='"+BOMItemCode.Trim()+"'";
			}
			if((OPBOMCode != string.Empty)&&(OPBOMCode.Trim() != string.Empty))
			{
				tmpString += " and OBCode='"+OPBOMCode.Trim()+"'";
			}
			if((OPBOMVersion != string.Empty)&&(OPBOMVersion.Trim() != string.Empty))
			{
				tmpString += " and OPBOMVER ='"+OPBOMVersion+"'";
			}
			int iCount = this.DataProvider.GetCount(new SQLCondition(String.Format(selectSQL,tmpString)));
			if(iCount>0)
			{
				return true;
			}
			return false;
		}


		public object[] QueryOPBOMItemControl(string itemCode,string BOMItemCode,string OPBOMCode,string OPBOMVersion,string OPID,string DateCodeStart,string DateCodeEnd,string VendorCode,string VendorItemCode,string ItemVersion,string BIOSVersion,string PCBAVersion,string cardStart,string cardEnd)
		{
			string selectSQL = "select "+ DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPItemControl))+" from tblopitemcontrol where 1=1 {0} order by seq";
			string tmpString = string.Empty;
			if((itemCode != string.Empty)&&(itemCode.Trim() != string.Empty))
			{
				tmpString += " and itemcode ='"+itemCode.Trim()+"'";
			}
			if((BOMItemCode != string.Empty)&&(BOMItemCode.Trim() != string.Empty))
			{
				tmpString += " and obitemcode ='"+BOMItemCode.Trim()+"'";
			}
			if((OPBOMCode != string.Empty)&&(OPBOMCode.Trim() != string.Empty))
			{
				tmpString += " and OBCode='"+OPBOMCode.Trim()+"'";
			}
			if((OPBOMVersion != string.Empty)&&(OPBOMVersion.Trim() != string.Empty))
			{
				tmpString += " and OPBOMVER ='"+OPBOMVersion+"'";
			}
			if((OPID != string.Empty)&&(OPID.Trim() != string.Empty))
			{
				tmpString += " and opid ='"+OPID.Trim()+"'";
			}
			if((DateCodeStart != string.Empty)&&(DateCodeStart.Trim() != string.Empty))
			{
				tmpString +=" and dcstart='"+DateCodeStart.Trim()+"'";
			}
			if((DateCodeEnd != string.Empty)&&(DateCodeEnd.Trim()!=string.Empty))
			{
				tmpString += " and dcend='"+DateCodeEnd.Trim()+"'";
			}
			if((VendorCode != string.Empty)&&(VendorCode.Trim() != string.Empty))
			{
				tmpString += " and vcode='"+VendorCode+"'";
			}
			if((VendorItemCode != string.Empty)&&(VendorItemCode.Trim() != string.Empty))
			{
				tmpString += " and vitemcode='"+VendorItemCode+"'";
			}
			if((ItemVersion != string.Empty)&&(ItemVersion.Trim() != string.Empty))
			{
				tmpString += " and itemver='"+ItemVersion.Trim()+"'";
			}
			if((BIOSVersion != string.Empty)&&(BIOSVersion.Trim() != string.Empty))
			{
				tmpString +=" and biosver='"+BIOSVersion.Trim()+"'";
			}
			if((PCBAVersion != string.Empty)&&(PCBAVersion.Trim() != string.Empty))
			{
				tmpString += " and pcbaver='"+PCBAVersion+"'";
			}
			if((cardStart != string.Empty)&&(cardStart.Trim() != string.Empty))
			{
				tmpString += " and cardstart='"+cardStart.Trim()+"'";
			}
			if((cardEnd != string.Empty)&&(cardEnd.Trim() != string.Empty))
			{
				tmpString += " and cardend='"+cardEnd.Trim()+"'";
			}
			return this.DataProvider.CustomQuery(typeof(OPItemControl),new SQLCondition(String.Format(selectSQL,tmpString)));
		}


		public object[] GetOPBOMItemControl(string itemCode,string OPID,string BOMItemCode,string OPBOMCode,string OPBOmVersion,int inclusive,int exclusive)
		{
			if((itemCode == string.Empty)||(OPID == string.Empty)||(BOMItemCode==string.Empty)||(OPBOMCode==string.Empty)||(OPBOmVersion==string.Empty))
			{
				ExceptionManager.Raise(this.GetType(),"$Error_Null_Paramter");
				//				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPItemControlFacade),String.Format(ErrorCenter.ERROR_ARGUMENT_NULL,"itemCode or OPID or BOMItemCode or OPBOMCode or OPBOmVersion")));
			}
			string selectSql = "select "+DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPItemControl))+" from tblopitemcontrol where itemcode='{0}'  "+
				" and opid='{1}' and obitemcode='{2}'  and OBCode='{3}'  and opbomver='{4}' order by seq";
			object[] objs = new object[5];
			objs[0]=itemCode;
			objs[1]=OPID;
			objs[2]=BOMItemCode;
			objs[3]=OPBOMCode;
			objs[4]=OPBOmVersion;
			return this.DataProvider.CustomQuery(typeof(OPItemControl),new PagerCondition(String.Format(selectSql,objs),inclusive,exclusive));
		}

		public int GetOPBOMItemControlCounts(string itemCode,string OPID,string BOMItemCode,string OPBOMCode,string OPBOmVersion)
		{
			if((itemCode == string.Empty)||(OPID == string.Empty)||(BOMItemCode==string.Empty)||(OPBOMCode==string.Empty)||(OPBOmVersion==string.Empty))
			{
				ExceptionManager.Raise(this.GetType(),"$Error_Null_Paramter");
				//				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPItemControlFacade),String.Format(ErrorCenter.ERROR_ARGUMENT_NULL,"itemCode")));
			}
			string selectSql = "select count(*) from tblopitemcontrol where itemcode='{0}'  "+
				" and opid='{1}' and obitemcode='{2}'  and OBCode='{3}'  and opbomver='{4}' order by seq";
			object[] objs = new object[5];
			objs[0]=itemCode;
			objs[1]=OPID;
			objs[2]=BOMItemCode;
			objs[3]=OPBOMCode;
			objs[4]=OPBOmVersion;
			return this.DataProvider.GetCount(new SQLCondition(String.Format(selectSql,objs)));
		}

		public object GetOPBOMItemControl(string itemCode,string OPID,string BOMItemCode,string OPBOMCode,string OPBOMVersion,int sequence)
		{
			return this.DataProvider.CustomSearch(typeof(OPItemControl),new object[]{sequence,itemCode,OPBOMCode,OPBOMVersion,OPID,BOMItemCode});
		}
	}
		
}