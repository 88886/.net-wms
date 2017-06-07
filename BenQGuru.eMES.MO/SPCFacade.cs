using System;
using System.Runtime.Remoting;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.SPC;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.MOModel
{
	/// <summary>
	/// SPCFacade ��ժҪ˵����
	/// �ļ���:		SPCFacade.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		ER/Studio Basic Macro Code Generation  Created by ****
	/// ��������:	2006-8-14 9:14:21
	/// �޸���:
	/// �޸�����:
	/// �� ��:	
	/// �� ��:	
	/// </summary>
	public class SPCFacade
	{
		private IDomainDataProvider _domainDataProvider = null;
		private FacadeHelper _helper = null;

		public SPCFacade(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
			this._helper = new FacadeHelper( DataProvider );
		}

		public IDomainDataProvider DataProvider
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

		#region SPCConfig
		/// <summary>
		/// 
		/// </summary>
		public SPCConfig CreateNewSPCConfig()
		{
			return new SPCConfig();
		}

		public void AddSPCConfig( SPCConfig sPCConfig)
		{
			this._helper.AddDomainObject( sPCConfig );
		}

		public void UpdateSPCConfig(SPCConfig sPCConfig)
		{
			this._helper.UpdateDomainObject( sPCConfig );
		}

		public void DeleteSPCConfig(SPCConfig sPCConfig)
		{
			this._helper.DeleteDomainObject( sPCConfig );
		}

		public void DeleteSPCConfig(SPCConfig[] sPCConfig)
		{
			this._helper.DeleteDomainObject( sPCConfig );
		}

		public object GetSPCConfig( string paramName )
		{
			return this.DataProvider.CustomSearch(typeof(SPCConfig), new object[]{ paramName });
		}

		/// <summary>
		/// ** ��������:	��ѯSPCConfig��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="paramName">ParamName��ģ����ѯ</param>
		/// <returns> SPCConfig���ܼ�¼��</returns>
		public int QuerySPCConfigCount( string paramName)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSPCCONFIG where 1=1 and ParamName like '{0}%' " , paramName)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSPCConfig
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="paramName">ParamName��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SPCConfig����</returns>
		public object[] QuerySPCConfig( string paramName, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(SPCConfig), new PagerCondition(string.Format("select {0} from TBLSPCCONFIG where 1=1 and ParamName like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCConfig)) , paramName), "ParamName", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�SPCConfig
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>SPCConfig���ܼ�¼��</returns>
		public object[] GetAllSPCConfig()
		{
			return this.DataProvider.CustomQuery(typeof(SPCConfig), new SQLCondition(string.Format("select {0} from TBLSPCCONFIG order by ParamName", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCConfig)))));
		}


		#endregion

		#region SPCDataStore
		/// <summary>
		/// 
		/// </summary>
		public SPCDataStore CreateNewSPCDataStore()
		{
			return new SPCDataStore();
		}

		public void AddSPCDataStore( SPCDataStore SPCDataStore)
		{
			this._helper.AddDomainObject( SPCDataStore );
		}

		public void UpdateSPCDataStore(SPCDataStore SPCDataStore)
		{
			this._helper.UpdateDomainObject( SPCDataStore );
		}

		public void DeleteSPCDataStore(SPCDataStore SPCDataStore)
		{
			this._helper.DeleteDomainObject( SPCDataStore );
		}

		public void DeleteSPCDataStore(SPCDataStore[] SPCDataStore)
		{
			this._helper.DeleteDomainObject( SPCDataStore );
		}

		public object GetSPCDataStore( string iD )
		{
			return this.DataProvider.CustomSearch(typeof(SPCDataStore), new object[]{ iD });
		}

		/// <summary>
		/// ** ��������:	��ѯSPCDataStore��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="iD">ID��ģ����ѯ</param>
		/// <returns> SPCDataStore���ܼ�¼��</returns>
		public int QuerySPCDataStoreCount( string iD)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSPCDataStore where 1=1 and ID like '{0}%' " , iD)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSPCDataStore
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="iD">ID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SPCDataStore����</returns>
		public object[] QuerySPCDataStore( string iD, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(SPCDataStore), new PagerCondition(string.Format("select {0} from TBLSPCDataStore where 1=1 and ID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCDataStore)) , iD), "ID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	��ѯSPCDataStore��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="iD">ID��ģ����ѯ</param>
		/// <returns> SPCDataStore���ܼ�¼��</returns>
		public int QuerySPCDataStoreCount( string itemCodes, string objectCode)
		{
			string condition = string.Empty;
			if( itemCodes!=null && itemCodes.Length>0 )
			{
				condition += string.Format( " and itemcode in ({0}) ", FormatHelper.ProcessQueryValues( itemCodes ) );
			}

			if( objectCode!=null && objectCode.Length>0 )
			{
				condition += string.Format( " and objectCode like '{0}%' ", objectCode );
			}
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSPCDataStore where 1=1 {0} " , condition)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSPCDataStore
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="iD">ID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SPCDataStore����</returns>
		public object[] QuerySPCDataStore( string itemCodes, string objectCode, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if( itemCodes!=null && itemCodes.Length>0 )
			{
				condition += string.Format( " and itemcode in ({0}) ", FormatHelper.ProcessQueryValues( itemCodes ) );
			}

			if( objectCode!=null && objectCode.Length>0 )
			{
				condition += string.Format( " and objectCode like '{0}%' ", objectCode );
			}
			return this.DataProvider.CustomQuery(typeof(SPCDataStore), new PagerCondition(string.Format("select {0} from TBLSPCDataStore where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCDataStore)) , condition), "itemcode", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�SPCDataStore
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>SPCDataStore���ܼ�¼��</returns>
		public object[] GetAllSPCDataStore()
		{
			return this.DataProvider.CustomQuery(typeof(SPCDataStore), new SQLCondition(string.Format("select {0} from TBLSPCDataStore order by ID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCDataStore)))));
		}


		#endregion

        #region SPCDeterRule
        /// <summary>
        /// 
        /// </summary>
        public SPCDeterRule CreateNewSPCDeterRule()
        {
            return new SPCDeterRule();
        }

        public void AddSPCDeterRule(SPCDeterRule sPCDeterRule)
        {
            this._helper.AddDomainObject(sPCDeterRule);
        }

        public void UpdateSPCDeterRule(SPCDeterRule sPCDeterRule)
        {
            this._helper.UpdateDomainObject(sPCDeterRule);
        }

        public void DeleteSPCDeterRule(SPCDeterRule sPCDeterRule)
        {
            this._helper.DeleteDomainObject(sPCDeterRule);
        }

        public void DeleteSPCDeterRule(SPCDeterRule[] sPCDeterRule)
        {
            this._helper.DeleteDomainObject(sPCDeterRule);
        }

        public object GetSPCDeterRule(string ruleCode)
        {
            return this.DataProvider.CustomSearch(typeof(SPCDeterRule), new object[] { ruleCode });
        }

        /// <summary>
        /// ** ��������:	��ѯSPCDeterRule��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2007-8-20 15:07:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="ruleCode">RuleCode��ģ����ѯ</param>
        /// <returns> SPCDeterRule���ܼ�¼��</returns>
        public int QuerySPCDeterRuleCount(string ruleCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSPCDETERRULE where 1=1 and RULECODE like '{0}%' ", ruleCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSPCDeterRule
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2007-8-20 15:07:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="ruleCode">RuleCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SPCDeterRule����</returns>
        public object[] QuerySPCDeterRule(string ruleCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SPCDeterRule), new PagerCondition(string.Format("select {0} from TBLSPCDETERRULE where 1=1 and RULECODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCDeterRule)), ruleCode), "RULECODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SPCDeterRule
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2007-8-20 15:07:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SPCDeterRule���ܼ�¼��</returns>
        public object[] GetAllSPCDeterRule()
        {
            return this.DataProvider.CustomQuery(typeof(SPCDeterRule), new SQLCondition(string.Format("select {0} from TBLSPCDETERRULE order by RULECODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCDeterRule)))));
        }
        public object[] GetSPCDeterRuleEnabled()
        {
            string strSql = "select * from TBLSPCDETERRULE where Enabled='" + FormatHelper.BooleanToString(true) + "' order by rulecode";
            return this.DataProvider.CustomQuery(typeof(SPCDeterRule), new SQLCondition(strSql));
        }


        #endregion

		#region SPCItemSpec
		/// <summary>
		/// 
		/// </summary>
		public SPCItemSpec CreateNewSPCItemSpec()
		{
			return new SPCItemSpec();
		}

		public void AddSPCItemSpec( SPCItemSpec sPCItemSpec)
		{
			this._helper.AddDomainObject( sPCItemSpec );
		}

		public void UpdateSPCItemSpec(SPCItemSpec sPCItemSpec)
		{
			this._helper.UpdateDomainObject( sPCItemSpec );
		}

		public void DeleteSPCItemSpec(SPCItemSpec sPCItemSpec)
		{
			this._helper.DeleteDomainObject( sPCItemSpec );
		}

		public void DeleteSPCItemSpec(SPCItemSpec[] sPCItemSpec)
		{
			this._helper.DeleteDomainObject( sPCItemSpec );
		}

		public object GetSPCItemSpec( string itemCode, decimal groupSeq, string objectCode )
		{
			return this.DataProvider.CustomSearch(typeof(SPCItemSpec), new object[]{ itemCode, groupSeq, objectCode });
		}

		/// <summary>
		/// ** ��������:	��ѯSPCItemSpec��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-15 18:06:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="groupSeq">GroupSeq��ģ����ѯ</param>
		/// <param name="objectCode">ObjectCode��ģ����ѯ</param>
		/// <returns> SPCItemSpec���ܼ�¼��</returns>
		public int QuerySPCItemSpecCount( string itemCodes, decimal groupSeq, string objectCode)
		{
			string condition = string.Empty;
			if( itemCodes!=null && itemCodes.Length>0 )
			{
				condition += string.Format( " and itemcode in ({0}) ", FormatHelper.ProcessQueryValues( itemCodes ) );
			}
			
			condition += string.Format( " and GroupSeq = {0} ", groupSeq );
			
			if( objectCode!=null && objectCode.Length>0 )
			{
				condition += string.Format( " and objectCode = '{0}' ", objectCode );
			}
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSPCITEMSPEC where 1=1 {0} " , condition)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSPCItemSpec
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-15 18:06:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="groupSeq">GroupSeq��ģ����ѯ</param>
		/// <param name="objectCode">ObjectCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SPCItemSpec����</returns>
		public object[] QuerySPCItemSpec( string itemCodes, decimal groupSeq, string objectCode, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if( itemCodes!=null && itemCodes.Length>0 )
			{
				condition += string.Format( " and itemcode in ({0}) ", FormatHelper.ProcessQueryValues( itemCodes ) );
			}
			
			condition += string.Format( " and GroupSeq = {0} ", groupSeq );
			
			if( objectCode!=null && objectCode.Length>0 )
			{
				condition += string.Format( " and objectCode = '{0}' ", objectCode );
			}
			return this.DataProvider.CustomQuery(typeof(SPCItemSpec), new PagerCondition(string.Format("select {0} from TBLSPCITEMSPEC where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCItemSpec)) , condition), "ItemCode,GroupSeq,ObjectCode", inclusive, exclusive));
		}

		public int QuerySPCItemSpecCount( string itemCodes, string objectCode)
		{
			string condition = string.Empty;
			if( itemCodes!=null && itemCodes.Length>0 )
			{
				condition += string.Format( " and itemcode in ({0}) ", FormatHelper.ProcessQueryValues( itemCodes ) );
			}
			
			if( objectCode!=null && objectCode.Length>0 )
			{
				condition += string.Format( " and objectCode = '{0}' ", objectCode );
			}
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSPCITEMSPEC where 1=1 {0} " , condition)));
		}

		
		public object[] QuerySPCItemSpec( string itemCodes, string objectCode, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if( itemCodes!=null && itemCodes.Length>0 )
			{
				condition += string.Format( " and itemcode in ({0}) ", FormatHelper.ProcessQueryValues( itemCodes ) );
			}
			
			if( objectCode!=null && objectCode.Length>0 )
			{
				condition += string.Format( " and objectCode = '{0}' ", objectCode );
			}
			return this.DataProvider.CustomQuery(typeof(SPCItemSpec), new PagerCondition(string.Format("select {0} from TBLSPCITEMSPEC where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCItemSpec)) , condition), "ItemCode,GroupSeq,ObjectCode", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�SPCItemSpec
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-15 18:06:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>SPCItemSpec���ܼ�¼��</returns>
		public object[] GetAllSPCItemSpec()
		{
			return this.DataProvider.CustomQuery(typeof(SPCItemSpec), new SQLCondition(string.Format("select {0} from TBLSPCITEMSPEC order by ItemCode,GroupSeq,ObjectCode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCItemSpec)))));
		}


		#endregion

		#region SPCObject
		/// <summary>
		/// 
		/// </summary>
		public SPCObject CreateNewSPCObject()
		{
			return new SPCObject();
		}

		public void AddSPCObject( SPCObject sPCObject)
		{
			this._helper.AddDomainObject( sPCObject );
		}

		public void UpdateSPCObject(SPCObject sPCObject)
		{
			this._helper.UpdateDomainObject( sPCObject );
		}

		public void DeleteSPCObject(SPCObject sPCObject)
		{
			this._helper.DeleteDomainObject( sPCObject, 
 								new ICheck[]{ new DeleteAssociateCheck( sPCObject,
														this.DataProvider, 
														new Type[]{
																typeof(SPCItemSpec),
																typeof(SPCDataStore)	})} );
		}

		public void DeleteSPCObject(SPCObject[] sPCObject)
		{
			this._helper.DeleteDomainObject( sPCObject, 
 								new ICheck[]{ new DeleteAssociateCheck( sPCObject,
														this.DataProvider, 
														new Type[]{
																typeof(SPCItemSpec),
																typeof(SPCDataStore)	})} );
		}

		public object GetSPCObject( string objectCode )
		{
			return this.DataProvider.CustomSearch(typeof(SPCObject), new object[]{ objectCode });
		}

		/// <summary>
		/// ** ��������:	��ѯSPCObject��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="objectCode">ObjectCode��ģ����ѯ</param>
		/// <returns> SPCObject���ܼ�¼��</returns>
		public int QuerySPCObjectCount( string objectCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSPCOBJECT where 1=1 and ObjectCode like '{0}%' " , objectCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSPCObject
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="objectCode">ObjectCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SPCObject����</returns>
		public object[] QuerySPCObject( string objectCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(SPCObject), new PagerCondition(string.Format("select {0} from TBLSPCOBJECT where 1=1 and ObjectCode like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCObject)) , objectCode), "ObjectCode", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	��ѯSPCObject��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="objectCode">ObjectCode��ģ����ѯ</param>
		/// <returns> SPCObject���ܼ�¼��</returns>
		public int QuerySPCObjectCount( string objectCode, string objectName)
		{
			string condition = string.Empty;
			if( objectCode!=null && objectCode.Length>0 )
			{
				condition += string.Format( " and ObjectCode like '{0}%' ", objectCode );
			}

			if( objectName!=null && objectName.Length>0 )
			{
				condition += string.Format( " and ObjectName like '{0}%' ", objectName );
			}
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSPCOBJECT where 1=1 {0} " , condition)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSPCObject
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="objectCode">ObjectCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SPCObject����</returns>
		public object[] QuerySPCObject( string objectCode, string objectName, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if( objectCode!=null && objectCode.Length>0 )
			{
				condition += string.Format( " and ObjectCode like '{0}%' ", objectCode );
			}

			if( objectName!=null && objectName.Length>0 )
			{
				condition += string.Format( " and ObjectName like '{0}%' ", objectName );
			}
			return this.DataProvider.CustomQuery(typeof(SPCObject), new PagerCondition(string.Format("select {0} from TBLSPCOBJECT where 1=1 {1}", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCObject)) , condition), "ObjectCode", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�SPCObject
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-8-14 9:14:21
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>SPCObject���ܼ�¼��</returns>
		public object[] GetAllSPCObject()
		{
			return this.DataProvider.CustomQuery(typeof(SPCObject), new SQLCondition(string.Format("select {0} from TBLSPCOBJECT order by ObjectCode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCObject)))));
		}


		#endregion

		#region SPCObjectStore
		/// <summary>
		/// 
		/// </summary>
		public SPCObjectStore CreateNewSPCObjectStore()
		{
			return new SPCObjectStore();
		}

		public void AddSPCObjectStore( SPCObjectStore sPCObjectStore)
		{
			this._helper.AddDomainObject( sPCObjectStore );
		}

		public void UpdateSPCObjectStore(SPCObjectStore sPCObjectStore)
		{
			this._helper.UpdateDomainObject( sPCObjectStore );
		}

		public void DeleteSPCObjectStore(SPCObjectStore sPCObjectStore)
		{
			this._helper.DeleteDomainObject( sPCObjectStore );
		}

		public void DeleteSPCObjectStore(SPCObjectStore[] sPCObjectStore)
		{
			this._helper.DeleteDomainObject( sPCObjectStore );
		}

        public object GetSPCObjectStore(string PK)
        {
            return this.DataProvider.CustomSearch(typeof(SPCObjectStore), new object[] { PK });
        }

        public object[] GetSPCObjectStore(string objectCode, decimal groupSeq)
        {
            string sql = "select {0} from TBLSPCOBJECTSTORE where ObjectCode = '" + objectCode + "' and GroupSEQ = " + groupSeq + " order by GROUPSEQ,ObjectCode";
            object[] objects = this.DataProvider.CustomQuery(typeof(SPCObjectStore), new SQLCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCObjectStore)))));

            return objects;
        }



        public object[] GetSPCObjectStore(string objectCode, decimal groupSeq, string ckGroup)
        {
            string sql = "select {0} from TBLSPCOBJECTSTORE where ObjectCode = '" + objectCode + "' and GroupSEQ = " + groupSeq + " and CKGroup = '" + ckGroup + "' order by GROUPSEQ,ObjectCode";
            object[] objects = this.DataProvider.CustomQuery(typeof(SPCObjectStore), new SQLCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCObjectStore)))));

            return objects;
        }

        public object GetSPCObjectStore(string objectCode, decimal groupSeq, string ckGroup, string ckItemCode)
        {
            string sql = "select {0} from TBLSPCOBJECTSTORE where ObjectCode = '" + objectCode + "' and GroupSEQ = " + groupSeq + " and CKGroup = '" + ckGroup + "' and CKItemCode = '" + ckItemCode + "' order by GROUPSEQ,ObjectCode";
            object[] objects = this.DataProvider.CustomQuery(typeof(SPCObjectStore), new SQLCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCObjectStore)))));
            if (objects != null)
            {
                return objects[0];
            }
            return null;

        }

       

		/// <summary>
		/// ** ��������:	��ѯSPCObjectStore��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-1-12 13:52:01
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="objectCode">ObjectCode��ģ����ѯ</param>
		/// <returns> SPCObjectStore���ܼ�¼��</returns>
		public int QuerySPCObjectStoreCount( string objectCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSPCOBJECTSTORE where 1=1 and ObjectCode like '{0}%' " , objectCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSPCObjectStore
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-1-12 13:52:01
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="groupSeq">GroupSeq��ģ����ѯ</param>
		/// <param name="objectCode">ObjectCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SPCObjectStore����</returns>
		public object[] QuerySPCObjectStore( string objectCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(SPCObjectStore), new PagerCondition(string.Format("select {0} from TBLSPCOBJECTSTORE where 1=1 and ObjectCode like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCObjectStore)) , objectCode), "ObjectCode,GROUPSEQ", inclusive, exclusive));
		}

        public object[] QuerySPCObjectStoreByObjectCode(string objectCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SPCObjectStore), new PagerCondition(string.Format("select {0} from TBLSPCOBJECTSTORE where 1=1 and ObjectCode = '{1}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCObjectStore)), objectCode), "ObjectCode,GROUPSEQ", inclusive, exclusive));
        }

		/// <summary>
		/// ** ��������:	������е�SPCObjectStore
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-1-12 13:52:01
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>SPCObjectStore���ܼ�¼��</returns>
		public object[] GetAllSPCObjectStore()
		{
			return this.DataProvider.CustomQuery(typeof(SPCObjectStore), new SQLCondition(string.Format("select {0} from TBLSPCOBJECTSTORE order by GROUPSEQ,ObjectCode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SPCObjectStore)))));
		}


		#endregion

	}
}

