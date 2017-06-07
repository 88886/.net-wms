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
	/// SAPDataGeter ��ȡSAP����
	/// </summary>
	public class SAPDataGeter
	{

		private  IDomainDataProvider _domainDataProvider= null;
		private  FacadeHelper _helper					= null;

		public SAPDataGeter(IDomainDataProvider domainDataProvider)
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
					//TODO �˴�Ӧ������SAP�����ݿ�
					_domainDataProvider = DomainDataProviderManager.DomainDataProvider(DBName.SAP);
				}
				return _domainDataProvider;
			}	
		}

		public object[] GetSAPData(DataName _dataName)
		{
			if(_dataName == DataName.SAPMO)
			{
				//����
				return this.GetSAPMO();
			
			}
			else if(_dataName == DataName.SAPMOBom)
			{
				//����BOM
				return this.GetSAPMOBom();
			}
			else if(_dataName == DataName.SAPBom)
			{
				//BOM
				return this.GetSAPBOM();
			}
			return new object[]{};
		}

		public object[] GetSAPData(DataName _dataName,int inclusive,int exclusive)
		{
			if(_dataName == DataName.SAPMO)
			{
				//����
				return this.GetSAPMO(inclusive,exclusive);
			
			}
			else if(_dataName == DataName.SAPMOBom)
			{
				//����BOM
				return this.GetSAPMOBom(inclusive,exclusive);
			}
			else if(_dataName == DataName.SAPBom)
			{
				//BOM
				return this.GetSAPBOM(inclusive,exclusive);
			}
			return new object[]{};
		}

		#region ��ȡҪ�����SAP����

		/// <summary>
		/// SAP����
		/// </summary>
		/// <returns></returns>
		private object[] GetSAPMO()
		{
			string condition = this.GetLastupdateCondition(JobName.MO);	//��ѯ����
			string sql = string.Format("select {0} from WO where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SAPMO)), condition) ;
			//Test
			//string sql = string.Format("select * from ( select {0} from WO where 1=1 {1} ) where rownum < 3  ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SAPMO)), condition) ;
			object[] returnObjs = this.DataProvider.CustomQuery(
				typeof(SAPMO),
				new SQLCondition(sql));
			return returnObjs;
		}	
	
		/// <summary>
		/// SAP����BOM
		/// </summary>
		/// <returns></returns>
		private object[] GetSAPMOBom()
		{
			string condition = this.GetLastupdateCondition(JobName.MOBom);	//��ѯ����
			string sql = string.Format("select {0} from WODETAIL where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SAPMOBom)), condition) ;
			//Test
			//string sql = string.Format("select * from ( select {0} from WODETAIL where 1=1 {1} ) where rownum < 3  ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SAPMOBom)), condition) ;
			object[] returnObjs = this.DataProvider.CustomQuery(
				typeof(SAPMOBom),
				new SQLCondition(sql));
			return returnObjs;
		}		

		/// <summary>
		/// SAP BOM
		/// </summary>
		/// <returns></returns>
		private object[] GetSAPBOM()
		{
			string condition = this.GetLastupdateCondition(JobName.BOM);	//��ѯ����
			string sql = string.Format("select {0} from BOM where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SAPBOM)), condition) ;
			//Test
			//string sql = string.Format("select * from ( select {0} from BOM where 1=1 {1} ) where rownum < 3 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SAPBOM)), condition) ;
			object[] returnObjs = this.DataProvider.CustomQuery(
				typeof(SAPBOM),
				new SQLCondition(sql));
			return returnObjs;
		}		
		
		#endregion

		#region ��ȡҪ�����SAP���� ���� ��ҳ����

		/// <summary>
		/// SAP����
		/// </summary>
		/// <returns></returns>
		private object[] GetSAPMO(int inclusive,int exclusive)
		{
			string condition = this.GetLastupdateCondition(JobName.MO);	//��ѯ����
			string sql = string.Format("select {0} from WO where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SAPMO)), condition) ;
			object[] returnObjs = this.DataProvider.CustomQuery(
				typeof(SAPMO),
				 new PagerCondition(sql,inclusive,exclusive));
			return returnObjs;
		}	
	
		/// <summary>
		/// SAP ����bom
		/// </summary>
		/// <returns></returns>
		private object[] GetSAPMOBom(int inclusive,int exclusive)
		{
			//wodetailû��������ʱ�� ,��˸������¸��¹�������ȡ
			#region ���ϲ�ѯ,��Ч��������ʱ����

//			string condition = this.GetLastupdateCondition(JobName.MOBom);	//��ѯ����
//			string mocodeCondition = string.Format("AND wodetail.AUFNR in ( select AUFNR from WO where 1=1 {0} )",  condition) ;
//			string sql = string.Format(@"  SELECT wodetail.zpri, wodetail.alpgr, wodetail.enmng, wodetail.bdmng,
//												wodetail.gmein, wodetail.zflag, wodetail.matnr, wodetail.vornr,
//												wodetail.aufnr, wodetail.pwerk, wo.fmatnr,item.maktx
//											FROM item, wodetail,wo
//											WHERE 1=1
//											and item.matnr = wodetail.matnr 
//											and item.werks = wodetail.pwerk 
//											and wodetail.aufnr = wo.aufnr {0} ", mocodeCondition) ;
//			object[] returnObjs = this.DataProvider.CustomQuery(
//				typeof(SAPMOBom),
//				 new PagerCondition(sql,inclusive,exclusive));
//			return returnObjs;

			#endregion

			string condition = this.GetLastupdateCondition(JobName.MOBom);	//��ѯ����
			string mocodeCondition = string.Format("AND wodetail.AUFNR in ( select AUFNR from WO where 1=1 {0} )",  condition) ;
			string sql = string.Format(@"  SELECT wodetail.zpri, wodetail.alpgr, wodetail.enmng, wodetail.bdmng,
												wodetail.gmein, wodetail.zflag, wodetail.matnr, wodetail.vornr,
												wodetail.aufnr, wodetail.pwerk, wo.fmatnr
											FROM wodetail,wo
											WHERE 1=1
											and wodetail.aufnr = wo.aufnr {0} ", mocodeCondition) ;
			object[] returnObjs = this.DataProvider.CustomQuery(
				typeof(SAPMOBom),
				new PagerCondition(sql,inclusive,exclusive));

			if(returnObjs == null)return null;
			#region ӳ����������
			ArrayList matnrs = new ArrayList();
			foreach(SAPMOBom _sapmobom in returnObjs)
			{
				matnrs.Add(_sapmobom.MOBOMItemCode);
			}

			Hashtable itemMatnrsHT = this.GetSAPItemHT(matnrs);

			foreach(SAPMOBom _sapmobom in returnObjs)
			{
				string htKey = _sapmobom.MOBOMItemCode;
				if(itemMatnrsHT.Contains(htKey))
				{
					_sapmobom.MOBOMItemName = (string)itemMatnrsHT[htKey];
				}
			}

			#endregion

			#region ӳ�乤���ƻ�����

			ArrayList mocodes = new ArrayList();
			Hashtable mocodesHT = new Hashtable();
			foreach(SAPMOBom _sapmobom in returnObjs)
			{
				if(!mocodesHT.Contains(_sapmobom.MOCode))
				{
					mocodes.Add(_sapmobom.MOCode);
					mocodesHT.Add(_sapmobom.MOCode,_sapmobom.MOCode);
				}
			}

			Hashtable MOPlanQtyHT = this.GetSAPMOPlanQty(mocodes);

			foreach(SAPMOBom _sapmobom in returnObjs)
			{
				string htKey = _sapmobom.MOCode;
				if(MOPlanQtyHT.Contains(htKey))
				{
					_sapmobom.MOPlanQty = (decimal)MOPlanQtyHT[htKey];
				}
			}

			#endregion

			return returnObjs;
		}		

		/// <summary>
		/// SAP BOM
		/// </summary>
		/// <returns></returns>
		private object[] GetSAPBOM(int inclusive,int exclusive)
		{
			#region ���ϲ�ѯ,��Ч��������ʱ����

//			string condition = this.GetLastupdateCondition(JobName.BOM);	//��ѯ����
//			string sql = string.Format(@" select bom.* ,item.maktx from item,bom 
//											WHERE 1=1
//											and item.matnr = bom.matnr 
//											and item.werks = bom.pwerk {0} ",  condition) ;
//			object[] returnObjs = this.DataProvider.CustomQuery(
//				typeof(SAPBOM),
//				 new PagerCondition(sql,inclusive,exclusive));
//			return returnObjs;

			#endregion

			string condition = this.GetLastupdateCondition(JobName.BOM);	//��ѯ����
			string sql = string.Format(@" select bom.* from bom 
											WHERE 1=1 {0} ",  condition) ;
			object[] returnObjs = this.DataProvider.CustomQuery(
				typeof(SAPBOM),
				new PagerCondition(sql,inclusive,exclusive));

			if(returnObjs == null)return null;

			#region ӳ����������
			ArrayList matnrs = new ArrayList();
			foreach(SAPBOM _sapbom in returnObjs)
			{
				matnrs.Add(_sapbom.SBOMItemCode);
			}

			Hashtable itemMatnrsHT = this.GetSAPItemHT(matnrs);

			foreach(SAPBOM _sapbom in returnObjs)
			{
				string htKey = _sapbom.SBOMItemCode;
				if(itemMatnrsHT.Contains(htKey))
				{
					_sapbom.SOBOMItemName = (string)itemMatnrsHT[htKey];
				}
			}

			#endregion

			return returnObjs;

		}		



		/// <summary>
		/// ��ȡsap��������
		/// </summary>
		/// <param name="matnrs"></param>
		/// <returns></returns>
		private Hashtable GetSAPItemHT(ArrayList matnrs)
		{
			//��ȡsap��Ʒ
			//ӳ��sap��Ʒ�������Ʒ����
			string condition = string.Empty;
			if(matnrs != null && matnrs.Count > 0)
			{
				string itemMatnrs = "('" + String.Join("','",(string[])matnrs.ToArray(typeof(string))) + "')";
				condition = string.Format(" and  item.matnr in {0}",itemMatnrs);
			}
			else{return null;}

			string sql = string.Format(" select distinct item.matnr, item.maktx from item where 1=1 {0}",condition);

			object[] returnObjs = this.DataProvider.CustomQuery(
				typeof(SAPItem),new SQLCondition(sql));
			

			Hashtable returnHT = new Hashtable();
			if(returnObjs != null )
			{
				foreach(SAPItem _sapItem in returnObjs)
				{
					if(!returnHT.Contains(_sapItem.MATNR))
					returnHT.Add(_sapItem.MATNR,_sapItem.MAKTX);
				}
			}

			return returnHT;
		}

		/// <summary>
		/// ��ȡ�����ƻ��ƻ�����
		/// </summary>
		/// <returns></returns>
		private Hashtable GetSAPMOPlanQty(ArrayList mocodes)
		{
			string condition = string.Empty;
			if(mocodes != null && mocodes.Count > 0)
			{
				string mocodeContition = "('" + String.Join("','",(string[])mocodes.ToArray(typeof(string))) + "')";
				condition = string.Format(" and  WO.AUFNR in {0}",mocodeContition);
			}
			else{return null;}

			string sql = string.Format(" select distinct WO.AUFNR, WO.PSMNG from WO where 1=1 {0}",condition);

			object[] returnObjs = this.DataProvider.CustomQuery(
				typeof(SAPMO),new SQLCondition(sql));
			
			Hashtable returnHT = new Hashtable();
			if(returnObjs != null )
			{
				foreach(SAPMO _sapmo in returnObjs)
				{
					try
					{
						if(!returnHT.Contains(_sapmo.MOCode))
							returnHT.Add(_sapmo.MOCode,Convert.ToDecimal(_sapmo.MOPlanQty));
					}
					catch
					{
						continue;
					}
				}
			}

			return returnHT;
		}

		#endregion

		#region ��ȡҪ�����SAP��������

		public int GetImportCount(DataName _dataName)
		{
			if(_dataName == DataName.SAPMO)
			{
				//����
				return this.GetSAPMOCount();
			
			}
			else if(_dataName == DataName.SAPMOBom)
			{
				//����BOM
				return this.GetSAPMOBOMCount();
			}
			else if(_dataName == DataName.SAPBom)
			{
				//BOM
				return this.GetSAPBOMCount();
			}
			return 0;
		}

		/// <summary>
		/// SAP����
		/// </summary>
		/// <returns></returns>
		private int GetSAPMOCount()
		{
			string condition = this.GetLastupdateCondition(JobName.MO);	//��ѯ����
			string sql = string.Format("select COUNT(AUFNR) from WO where 1=1 {0} ",  condition) ;
			return this.DataProvider.GetCount(new SQLCondition(sql));
		}	
	
		/// <summary>
		/// SAP����bom
		/// </summary>
		/// <returns></returns>
		private int GetSAPMOBOMCount()
		{
			//wodetailû��������ʱ�� ,��˸������¸��¹�������ȡ
			string condition = this.GetLastupdateCondition(JobName.MOBom);	//��ѯ����
			string mocodeCondition = string.Format("AND AUFNR in ( select AUFNR from WO where 1=1 {0} )",  condition) ;
			string sql = string.Format("select COUNT(MATNR) from WODETAIL where 1=1 {0} ",  mocodeCondition) ;
			return this.DataProvider.GetCount(new SQLCondition(sql));
		}		

		/// <summary>
		/// SAP BOM
		/// </summary>
		/// <returns></returns>
		private int GetSAPBOMCount()
		{
			string condition = this.GetLastupdateCondition(JobName.BOM);	//��ѯ����
			string sql = string.Format("select COUNT(FMATNR) from BOM where 1=1 {0} ",  condition) ;
			return this.DataProvider.GetCount(new SQLCondition(sql));
		}		
		
		#endregion

		#region JobLog����

		#region ���� MES JobLog

		//����ɹ���joglog
		public void AddSuccessJobLog(JobName jobName,DateTime beginTime,DateTime endTime)
		{
			JobLog _jobLog = this.CreateSuccessJobLog(jobName,beginTime,endTime);
			this._helper.AddDomainObject(_jobLog);
		}

		//����ʧ�ܵ�joglog
		public void AddFailedJobLog(JobName jobName,DateTime beginTime,DateTime endTime)
		{
			JobLog _jobLog = this.CreateFailedJobLog(jobName,beginTime,endTime);
			this._helper.AddDomainObject(_jobLog);
		}

		#endregion

		#region	���� MES JobLog

		public JobLog CreateSuccessJobLog(JobName jobName,DateTime beginTime,DateTime endTime)
		{
			JobLog _jobLog = new JobLog();
			_jobLog.JobClass = this.GetMESJobClass();//JobClass.MES.ToString();	//JobClassӦ�ô������ļ��л�ȡ
			
			_jobLog.Resultcode = 1;
			_jobLog.Resultdesc = "Success";
			_jobLog.StartTime = beginTime;
			_jobLog.EndTime = endTime;

			if(jobName == JobName.MO)
			{
				_jobLog.JobName = JobName.MO.ToString();
			}
			else if(jobName == JobName.BOM)
			{
				_jobLog.JobName = JobName.BOM.ToString();
			}
			else if(jobName == JobName.MOBom)
			{
				_jobLog.JobName = JobName.MOBom.ToString();
			}

			return _jobLog;
		}

		public JobLog CreateFailedJobLog(JobName jobName,DateTime beginTime,DateTime endTime)
		{
			JobLog _jobLog = new JobLog();
			_jobLog.JobClass = this.GetMESJobClass();//JobClass.MES.ToString();	//JobClassӦ�ô������ļ��л�ȡ
			
			_jobLog.Resultcode = 1;
			_jobLog.Resultdesc = "Failed";
			_jobLog.StartTime = beginTime;
			_jobLog.EndTime = endTime;

			if(jobName == JobName.MO)
			{
				_jobLog.JobName = JobName.MO.ToString();
			}
			else if(jobName == JobName.BOM)
			{
				_jobLog.JobName = JobName.BOM.ToString();
			}
			else if(jobName == JobName.MOBom)
			{
				_jobLog.JobName = JobName.MOBom.ToString();
			}

			return _jobLog;
		}

		#endregion

		#region ����JobLog��ȡ��Ӧ��ѯ����

		private string GetLastupdateCondition(JobName jobName)
		{
			string lastUpdateCondition = string.Empty;
			object[] logs = this.GetJobLog(jobName);
			if(logs!=null && logs.Length >0)
			{
				lastUpdateCondition = " AND LASTUPDATE > to_date('{0}','YYYY-MM-DD HH24:MI:SS') ";
				JobLog lastJobLog = (JobLog)logs[0];
				lastUpdateCondition = string.Format(lastUpdateCondition,lastJobLog.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));

			}
			return lastUpdateCondition;
		}

		#endregion

		#region	����JobName��ȡ��Ӧ�����һ��JobLog

		//����JobName��ȡ��Ӧ�����һ��JobLog
		private object[] GetJobLog(JobName jobName)
		{
			string sql = string.Empty;
			if(jobName == JobName.MO)
			{
				sql = this.GetSapMOJobLogSql();
			}
			else if(jobName == JobName.MOBom)
			{
				sql = this.GetSapWodetailLogSql();
			}
			else if(jobName == JobName.BOM)
			{
				sql = this.GetSapBomJobLogSql();
			}
			
			object[] returnObjs = this.DataProvider.CustomQuery(
				typeof(JobLog),
				new SQLCondition(sql));
			return returnObjs;
		}	


		#endregion

		#region ��ȡMES��Ӧ�����һ��JobLog

		//��ȡ���SAPMO �� JobLog
		public string GetSapMOJobLogSql()
		{
			string jobClass = this.GetMESJobClass();//JobClass.MES.ToString();	//JobClassӦ�ô������ļ��л�ȡ
			string jobName = JobName.MO.ToString();
			string endtimeCondition = string.Format(" select max(endtime) as endtime from joblog where jobclass='{0}' AND jobname='{1}' and RESULTDESC = 'Success' ",jobClass,jobName);
			string sql = string.Format(@"select {3} from  joblog where 1=1 AND endtime = ({2})
						AND jobclass='{0}' AND jobname='{1}'",jobClass,jobName,endtimeCondition,DomainObjectUtility.GetDomainObjectFieldsString(typeof(JobLog)));

			return sql;
		}

		//��ȡ���SAP wodetail�� JobLog ��Ӧmobom
		public string GetSapWodetailLogSql()
		{
			string jobClass = this.GetMESJobClass();//JobClass.MES.ToString();	//JobClassӦ�ô������ļ��л�ȡ
			string jobName = JobName.MOBom.ToString();
			string endtimeCondition = string.Format(" select max(endtime) as endtime from joblog where jobclass='{0}' AND jobname='{1}' and RESULTDESC = 'Success' ",jobClass,jobName);
			string sql = string.Format(@"select {3} from  joblog where 1=1 AND endtime = ({2})
						AND jobclass='{0}' AND jobname='{1}'",jobClass,jobName,endtimeCondition,DomainObjectUtility.GetDomainObjectFieldsString(typeof(JobLog)));

			return sql;
		}

		//��ȡ���SAPBom�� JobLog
		public string GetSapBomJobLogSql()
		{
			string jobClass = this.GetMESJobClass();//JobClass.MES.ToString();	//JobClassӦ�ô������ļ��л�ȡ
			string jobName = JobName.BOM.ToString();
			string endtimeCondition = string.Format(" select max(endtime) as endtime from joblog where jobclass='{0}' AND jobname='{1}' and RESULTDESC = 'Success' ",jobClass,jobName);
			string sql = string.Format(@"select {3} from  joblog where 1=1 AND endtime = ({2})
						AND jobclass='{0}' AND jobname='{1}'",jobClass,jobName,endtimeCondition,DomainObjectUtility.GetDomainObjectFieldsString(typeof(JobLog)));

			return sql;
		}

		#endregion

		#region ��ȡ�����ļ��е�JobClass,���û������,Ĭ����MES

		private string GetMESJobClass()
		{
			string strJobClassName = JobClass.MESMOBILE.ToString();//Ĭ����MES�ֻ� ����
			try
			{
				//JobClassӦ�ô������ļ��л�ȡ
				strJobClassName = BenQGuru.eMES.Common.Config.ConfigSection.Current.DomainSetting.MESJobClassName;
			}
			catch
			{
				strJobClassName = JobClass.MESMOBILE.ToString();			
			}
			return strJobClassName;
		}

		#endregion

		#endregion
	}
}
