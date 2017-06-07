using System;
using System.Runtime.Remoting;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.TS;

namespace BenQGuru.eMES.TSModel
{
	/// <summary>
	/// TSModelFacade ��ժҪ˵����
	/// �ļ���:		TSModelFacade.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
	/// ��������:	2005-03-24 13:28:05
	/// �޸���:
	/// �޸�����:
	/// �� ��:	
	/// �� ��:	
	/// </summary>
	public class TSModelFacade:MarshalByRefObject
	{
		private IDomainDataProvider _domainDataProvider = null;
		private FacadeHelper _helper = null;

		public override object InitializeLifetimeService()
		{
			return null;
		}

		public TSModelFacade(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
			this._helper = new FacadeHelper( DataProvider );
		}

		public TSModelFacade()
		{
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

		#region Duty
		/// <summary>
		/// 
		/// </summary>
		public Duty CreateNewDuty()
		{
			return new Duty();
		}

		public void AddDuty( Duty duty)
		{
			this._helper.AddDomainObject( duty );
		}

		public void UpdateDuty(Duty duty)
		{
			this._helper.UpdateDomainObject( duty );
		}

		public void DeleteDuty(Duty duty)
		{
			this._helper.DeleteDomainObject( duty );
		}

		public void DeleteDuty(Duty[] duty)
		{
			this._helper.DeleteDomainObject( duty );
		}

		public object GetDuty( string dutyCode )
		{
			return this.DataProvider.CustomSearch(typeof(Duty), new object[]{ dutyCode });
		}

		/// <summary>
		/// ** ��������:	��ѯDuty��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dutyCode">DutyCode��ģ����ѯ</param>
		/// <returns> Duty���ܼ�¼��</returns>
		public int QueryDutyCount( string dutyCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLDUTY where 1=1 and DUTYCODE like '{0}%' " , FormatHelper.PKCapitalFormat(dutyCode))));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯDuty
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dutyCode">DutyCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> Duty����</returns>
		public object[] QueryDuty( string dutyCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(Duty), new PagerCondition(string.Format("select {0} from TBLDUTY where 1=1 and DUTYCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Duty)) , FormatHelper.PKCapitalFormat(dutyCode)), "DUTYCODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�Duty
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>Duty���ܼ�¼��</returns>
		public object[] GetAllDuty()
		{
			return this.DataProvider.CustomQuery(typeof(Duty), new SQLCondition(string.Format("select {0} from TBLDUTY order by DUTYCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Duty)))));
		}


		#endregion

		#region ErrorCause
		/// <summary>
		/// 
		/// </summary>
		public ErrorCause CreateNewErrorCause()
		{
			return new ErrorCause();
		}

		public void AddErrorCause( ErrorCause errorCause)
		{
			this._helper.AddDomainObject( errorCause );
		}

		public void UpdateErrorCause(ErrorCause errorCause)
		{
			this._helper.UpdateDomainObject( errorCause );
		}

		public void DeleteErrorCause(ErrorCause errorCause)
		{
			this._helper.DeleteDomainObject( errorCause,
				new ICheck[]{ new DeleteAssociateCheck( errorCause,
								this.DataProvider, 
								new Type[]{
											  typeof(Model2ErrorCause)	})} );
		}

		public void DeleteErrorCause(ErrorCause[] errorCause)
		{
			this._helper.DeleteDomainObject( errorCause, 
				new ICheck[]{ new DeleteAssociateCheck( errorCause,
								this.DataProvider, 
								new Type[]{
											  typeof(Model2ErrorCause)	})} );
		}

		public object GetErrorCause( string errorCauseCode )
		{
			return this.DataProvider.CustomSearch(typeof(ErrorCause), new object[]{ errorCauseCode });
		}

		/// <summary>
		/// ** ��������:	��ѯErrorCause��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCauseCode">ErrorCauseCode��ģ����ѯ</param>
		/// <returns> ErrorCause���ܼ�¼��</returns>
		public int QueryErrorCauseCount( string errorCauseCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECS where 1=1 and ECSCODE like '{0}%' " , FormatHelper.PKCapitalFormat(errorCauseCode))));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯErrorCause
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCauseCode">ErrorCauseCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ErrorCause����</returns>
		public object[] QueryErrorCause( string errorCauseCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCause), new PagerCondition(string.Format("select {0} from TBLECS where 1=1 and ECSCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCause)) , FormatHelper.PKCapitalFormat(errorCauseCode)), "ECSCODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ErrorCause
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ErrorCause���ܼ�¼��</returns>
		public object[] GetAllErrorCause()
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCause), new SQLCondition(string.Format("select {0} from TBLECS order by ECSCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCause)))));
		}


		#endregion

		#region ErrorCauseGroup
		/// <summary>
		/// 
		/// </summary>
		public ErrorCauseGroup CreateNewErrorCauseGroup()
		{
			return new ErrorCauseGroup();
		}

		public void AddErrorCauseGroup( ErrorCauseGroup errorCauseGroup)
		{
			this._helper.AddDomainObject( errorCauseGroup );
		}

		public void UpdateErrorCauseGroup(ErrorCauseGroup errorCauseGroup)
		{
			this._helper.UpdateDomainObject( errorCauseGroup );
		}

		public void DeleteErrorCauseGroup(ErrorCauseGroup errorCauseGroup)
		{
			this._helper.DeleteDomainObject( errorCauseGroup, 
				new ICheck[]{ new DeleteAssociateCheck( errorCauseGroup,
								this.DataProvider, 
								new Type[]{
											  typeof(ErrorCauseGroup2ErrorCause),
											  typeof(Model2ErrorCauseGroup),
												typeof(ItemRouteOp2ErrorCauseGroup)})} );
		}

		public void DeleteErrorCauseGroup(ErrorCauseGroup[] errorCauseGroup)
		{
			this._helper.DeleteDomainObject( errorCauseGroup, 
				new ICheck[]{ new DeleteAssociateCheck( errorCauseGroup,
								this.DataProvider, 
								new Type[]{
											  typeof(ErrorCauseGroup2ErrorCause),
											  typeof(Model2ErrorCauseGroup),
											  typeof(ItemRouteOp2ErrorCauseGroup)	})} );
		}

		public object GetErrorCauseGroup( string errorCauseGroup )
		{
			return this.DataProvider.CustomSearch(typeof(ErrorCauseGroup), new object[]{ errorCauseGroup });
		}

		/// <summary>
		/// ** ��������:	��ѯErrorCauseGroup��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-26 9:23:31
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCauseGroup">ErrorCauseGroup��ģ����ѯ</param>
		/// <returns> ErrorCauseGroup���ܼ�¼��</returns>
		public int QueryErrorCauseGroupCount( string errorCauseGroup)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECSG where 1=1 and ECSGCODE like '{0}%' " , errorCauseGroup)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯErrorCauseGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-26 9:23:31
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCauseGroup">ErrorCauseGroup��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ErrorCauseGroup����</returns>
		public object[] QueryErrorCauseGroup( string errorCauseGroup, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCauseGroup), new PagerCondition(string.Format("select {0} from TBLECSG where 1=1 and ECSGCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCauseGroup)) , errorCauseGroup), "ECSGCODE", inclusive, exclusive));
		}

		//ȡ�û����µ����в���ԭ����
        public object[] QueryModelErrorCauseGroup(string modelcode, string errCauseGroupCondition, int inclusive, int exclusive)
        {
            string condition = string.Empty;
            if (errCauseGroupCondition.Trim().Length > 0)
                condition = " AND (UPPER(ecsgcode) LIKE UPPER('%" + errCauseGroupCondition + "%') OR UPPER(ecsgdesc) LIKE UPPER('%" + errCauseGroupCondition + "%')) ";

            return this.DataProvider.CustomQuery(typeof(ErrorCauseGroup), new PagerCondition(string.Format("select {0} from TBLECSG where 1=1 and ECSGCODE in(select ECSGCODE from tblmodel2ecsg where modelcode='{1}') " + condition , DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCauseGroup)), modelcode), "ECSGCODE", inclusive, exclusive));
        }

		public object[] QueryModelErrorCauseGroup( string modelcode, int inclusive, int exclusive )
		{
            return QueryModelErrorCauseGroup(modelcode, string.Empty, inclusive, exclusive);
		}


		/// <summary>
		/// ** ��������:	������е�ErrorCauseGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-26 9:23:31
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ErrorCauseGroup���ܼ�¼��</returns>
		public object[] GetAllErrorCauseGroup()
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCauseGroup), new SQLCondition(string.Format("select {0} from TBLECSG order by ECSGCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCauseGroup)))));
		}


		#endregion

		#region ErrorCauseGroup2ErrorCause
		/// <summary>
		/// 
		/// </summary>
		public ErrorCauseGroup2ErrorCause CreateNewErrorCauseGroup2ErrorCause()
		{
			return new ErrorCauseGroup2ErrorCause();
		}

		public void AddErrorCauseGroup2ErrorCause( ErrorCauseGroup2ErrorCause errorCauseGroup2ErrorCause)
		{
			this._helper.AddDomainObject( errorCauseGroup2ErrorCause );
		}

		public void AddErrorCauseGroup2ErrorCause( ErrorCauseGroup2ErrorCause[] errorCauseGroup2ErrorCause)
		{
			this._helper.AddDomainObject( errorCauseGroup2ErrorCause );
		}

		public void UpdateErrorCauseGroup2ErrorCause(ErrorCauseGroup2ErrorCause errorCauseGroup2ErrorCause)
		{
			this._helper.UpdateDomainObject( errorCauseGroup2ErrorCause );
		}

		public void DeleteErrorCauseGroup2ErrorCause(ErrorCauseGroup2ErrorCause errorCauseGroup2ErrorCause)
		{
			this._helper.DeleteDomainObject( errorCauseGroup2ErrorCause );
		}

		public void DeleteErrorCauseGroup2ErrorCause(ErrorCauseGroup2ErrorCause[] errorCauseGroup2ErrorCause)
		{
			this._helper.DeleteDomainObject( errorCauseGroup2ErrorCause );
		}

		public object GetErrorCauseGroup2ErrorCause( string errorCauseGroup, string errorCause )
		{
			return this.DataProvider.CustomSearch(typeof(ErrorCauseGroup2ErrorCause), new object[]{ errorCauseGroup, errorCause });
		}

		/// <summary>
		/// ** ��������:	��ѯErrorCauseGroup2ErrorCause��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-26 9:23:31
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCauseGroup">ErrorCauseGroup��ģ����ѯ</param>
		/// <param name="errorCause">ErrorCause��ģ����ѯ</param>
		/// <returns> ErrorCauseGroup2ErrorCause���ܼ�¼��</returns>
		public int QueryErrorCauseGroup2ErrorCauseCount( string errorCauseGroup, string errorCause)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECSG2ECS where 1=1 and ECSGCODE like '{0}%'  and ECSODE like '{1}%' " , errorCauseGroup, errorCause)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯErrorCauseGroup2ErrorCause
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-26 9:23:31
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCauseGroup">ErrorCauseGroup��ģ����ѯ</param>
		/// <param name="errorCause">ErrorCause��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ErrorCauseGroup2ErrorCause����</returns>
		public object[] QueryErrorCauseGroup2ErrorCause( string errorCauseGroup, string errorCause, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCauseGroup2ErrorCause), new PagerCondition(string.Format("select {0} from TBLECSG2ECS where 1=1 and ECSGCODE like '{1}%'  and ECSODE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCauseGroup2ErrorCause)) , errorCauseGroup, errorCause), "ECSGCODE,ECSODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ErrorCauseGroup2ErrorCause
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-26 9:23:31
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ErrorCauseGroup2ErrorCause���ܼ�¼��</returns>
		public object[] GetAllErrorCauseGroup2ErrorCause()
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCauseGroup2ErrorCause), new SQLCondition(string.Format("select {0} from TBLECSG2ECS order by ECSGCODE,ECSODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCauseGroup2ErrorCause)))));
		}

		
		/// <summary>
		/// ��ǰError Cause Group �Ѿ�ѡ��� Error Cause
		/// </summary>
		/// <param name="errorCodeGroupCode"></param>
		/// <param name="errorCode"></param>
		/// <returns></returns>
		public int GetSelectedErrorCauseByErrorCauseGroupCodeCount(string errorCauseGroupCode, string errorCause)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECSG2ECS where ECSGCODE ='{0}' and ECSCODE like '{1}%'", errorCauseGroupCode, FormatHelper.PKCapitalFormat(errorCause))));
		}

		
		public object[] GetSelectedErrorCauseByErrorCauseGroupCode(string errorCauseGroupCode, string errorCause, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCause), 
				new PagerCondition(string.Format("select {0} from TBLECS where ECSCODE in ( select ECSCODE from TBLECSG2ECS where ECSGCODE ='{1}') and ECSCODE like '{2}%' order by ECSCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCause)), errorCauseGroupCode, FormatHelper.PKCapitalFormat(errorCause)), "ECSCODE", inclusive, exclusive));
		}

		
		public object[] GetSelectedErrorCauseByErrorCauseGroupCode(string errorCauseGroupCode)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCause), 
				new SQLCondition(string.Format("select {0} from TBLECS where ECSCODE in ( select ECSCODE from TBLECSG2ECS where ECSGCODE ='{1}') order by ECSCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCause)),errorCauseGroupCode)));
		}

		/// <summary>
		/// ��ѯ������ErrorCauseGroup��Error Cause
		/// </summary>
		/// <param name="errorCodeGroupCode"></param>
		/// <param name="errorCode"></param>
		/// <returns></returns>
		public int GetUnselectedErrorCauseByErrorCauseGroupCodeCount(string errorCauseGroupCode, string errorCause)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECS where ECSCODE not in ( select ECSCODE from TBLECSG2ECS where ECSGCODE ='{0}') and ECSCODE like '{1}%'", errorCauseGroupCode, FormatHelper.PKCapitalFormat(errorCause))));
		}

		public object[]  GetUnselectedErrorCauseByErrorCauseGroupCode(string errorCauseGroupCode, string errorCause, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCause), 
				new PagerCondition(string.Format("select {0} from TBLECS where ECSCODE not in ( select ECSCODE from TBLECSG2ECS where ECSGCODE ='{1}') and ECSCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCause)), errorCauseGroupCode, FormatHelper.PKCapitalFormat(errorCause)), "ECSCODE", inclusive, exclusive));
		}

		#endregion

		#region ErrorCodeA
		/// <summary>
		/// 
		/// </summary>
		public ErrorCodeA CreateNewErrorCodeA()
		{
			return new ErrorCodeA();
		}

		public void AddErrorCode( ErrorCodeA errorCodeA)
		{
			this._helper.AddDomainObject( errorCodeA );
		}

		public void UpdateErrorCode(ErrorCodeA errorCodeA)
		{
			this._helper.UpdateDomainObject( errorCodeA );
		}

		public void DeleteErrorCode(ErrorCodeA errorCodeA)
		{
			this._helper.DeleteDomainObject( errorCodeA, 
				new ICheck[]{ new DeleteAssociateCheck( errorCodeA,
								this.DataProvider, 
								new Type[]{
											  typeof(ErrorCodeGroup2ErrorCode)	})} );
		}

		public void DeleteErrorCode(ErrorCodeA[] errorCodeA)
		{
			this._helper.DeleteDomainObject( errorCodeA, 
				new ICheck[]{ new DeleteAssociateCheck( errorCodeA,
								this.DataProvider, 
								new Type[]{
											  typeof(ErrorCodeGroup2ErrorCode)	})} );
		}

		public object GetErrorCode( string errorCode )
		{
			return this.DataProvider.CustomSearch(typeof(ErrorCodeA), new object[]{ errorCode });
		}

		/// <summary>
		/// ** ��������:	��ѯErrorCodeA��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCode">ErrorCode��ģ����ѯ</param>
		/// <returns> ErrorCodeA���ܼ�¼��</returns>
		public int QueryErrorCodeCount( string errorCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLEC where 1=1 and ECODE like '{0}%' " , FormatHelper.PKCapitalFormat(errorCode))));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯErrorCodeA
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCode">ErrorCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ErrorCodeA����</returns>
		public object[] QueryErrorCode( string errorCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeA), new PagerCondition(string.Format("select {0} from TBLEC where 1=1 and ECODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeA)) , FormatHelper.PKCapitalFormat(errorCode)), "ECODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ErrorCodeA
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ErrorCodeA���ܼ�¼��</returns>
		public object[] GetAllErrorCode()
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeA), new SQLCondition(string.Format("select {0} from TBLEC order by ECODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeA)))));
		}

		public object[] GetErrorCodeByGroup( string errorCodeGroupCode )
		{
			string sql = string.Format(@"SELECT {0} 
											FROM tblec
											WHERE ecode IN (
													SELECT tblecg2ec.ecode
														FROM tblecg2ec
													WHERE tblecg2ec.ecgcode = '{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeA)),errorCodeGroupCode);
			return this.DataProvider.CustomSearch(typeof(ErrorCodeA), new SQLCondition(sql));
		}


		#endregion

        #region ErrorCodeItem2Route
        public ErrorCodeItem2Route CreateNewErrorCodeItem2Route()
        {
            return new ErrorCodeItem2Route();
        }

        public void AddErrorCodeItem2Route(ErrorCodeItem2Route errorCodeItem2Route)
        {
            this._helper.AddDomainObject(errorCodeItem2Route);
        }

        public void UpdateErrorCodeItem2Route(ErrorCodeItem2Route errorCodeItem2Route)
        {
            this._helper.UpdateDomainObject(errorCodeItem2Route);
        }

        public void DeleteErrorCodeItem2Route(ErrorCodeItem2Route errorCodeItem2Route)
        {
            this._helper.DeleteDomainObject(errorCodeItem2Route);
        }

        public void DeleteErrorCodeItem2Route(ErrorCodeItem2Route[] errorCodeItem2Routes)
        {
            this._helper.DeleteDomainObject(errorCodeItem2Routes);
        }

        public object GetErrorCodeItem2Route(string errorCode, string itemCode, int orgID)
        {
            return this._domainDataProvider.CustomSearch(typeof(ErrorCodeItem2Route), new object[] { errorCode, itemCode, orgID });
        }

        public object[] QueryErrorCodeItem2Route(string errorCode, string itemCode, int orgID, int inclusive, int exclusive)
        {
            string sql = this.GetQueryErrorCodeItem2RouteSQL(errorCode, itemCode, orgID, false);
            return this._domainDataProvider.CustomQuery(typeof(ErrorCodeItem2Route),
                new PagerCondition(sql, "itemcode", inclusive, exclusive));
        }

        public int QueryErrorCodeItem2RouteCount(string errorCode, string itemCode, int orgID)
        {
            string sql = this.GetQueryErrorCodeItem2RouteSQL(errorCode, itemCode, orgID, true);
            return this._domainDataProvider.GetCount(new SQLCondition(sql));
        }

        private string GetQueryErrorCodeItem2RouteSQL(string errorCode, string itemCode, int orgID, bool isForCount)
        {
            string sql = "";
            if (isForCount)
            {
                sql += "SELECT COUNT(*) FROM tblecitem2route ";
            }
            else
            {
                sql += "SELECT " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeItem2Route)) + " FROM tblecitem2route ";
            }

            sql += "WHERE 1=1";
            sql += "  AND eccode='" + errorCode + "'";
            sql += "  AND orgid=" + orgID;
            if (itemCode.Trim().Length > 0)
            {
                sql += "  AND itemcode LIKE '%" + itemCode + "%'";
            }

            return sql;
        } 
        #endregion

		#region ErrorCodeGroupA
		/// <summary>
		/// 
		/// </summary>
		public ErrorCodeGroupA CreateNewErrorCodeGroup()
		{
			return new ErrorCodeGroupA();
		}

		public void AddErrorCodeGroup( ErrorCodeGroupA errorCodeGroupA)
		{
			this._helper.AddDomainObject( errorCodeGroupA );
		}

		public void UpdateErrorCodeGroup(ErrorCodeGroupA errorCodeGroupA)
		{
			this._helper.UpdateDomainObject( errorCodeGroupA );
		}

		public void DeleteErrorCodeGroup(ErrorCodeGroupA errorCodeGroupA)
		{
			this._helper.DeleteDomainObject( errorCodeGroupA, 
				new ICheck[]{ new DeleteAssociateCheck( errorCodeGroupA,
								this.DataProvider, 
								new Type[]{
											  typeof(ErrorCodeGroup2ErrorCode),
											  typeof(Model2ErrorCodeGroup)	})} );
		}

		public void DeleteErrorCodeGroup(ErrorCodeGroupA[] errorCodeGroupA)
		{
			this._helper.DeleteDomainObject( errorCodeGroupA, 
				new ICheck[]{ new DeleteAssociateCheck( errorCodeGroupA,
								this.DataProvider, 
								new Type[]{
											  typeof(ErrorCodeGroup2ErrorCode),
											  typeof(Model2ErrorCodeGroup)	})} );
		}

		public object GetErrorCodeGroup( string errorCodeGroupCode )
		{
			return this.DataProvider.CustomSearch(typeof(ErrorCodeGroupA), new object[]{ errorCodeGroupCode });
		}

		/// <summary>
		/// ** ��������:	��ѯErrorCodeGroupA��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup��ģ����ѯ</param>
		/// <returns> ErrorCodeGroupA���ܼ�¼��</returns>
		public int QueryErrorCodeGroupCount( string errorCodeGroupCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECG where 1=1 and ECGCODE like '{0}%' " , FormatHelper.PKCapitalFormat(errorCodeGroupCode))));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯErrorCodeGroupA
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ErrorCodeGroupA����</returns>
		public object[] QueryErrorCodeGroup( string errorCodeGroupCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA), new PagerCondition(string.Format("select {0} from TBLECG where 1=1 and ECGCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)) , FormatHelper.PKCapitalFormat(errorCodeGroupCode)), "ECGCODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ErrorCodeGroupA
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ErrorCodeGroupA���ܼ�¼��</returns>
		public object[] GetAllErrorCodeGroup()
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA), new SQLCondition(string.Format("select {0} from TBLECG order by ECGCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)))));
		}

		//ͨ����Ʒ��ȡErrorCodeGroup
		public object[] GetErrorCodeGroupByItemCode(string itemCode)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA), new SQLCondition(string.Format("select {0} from TBLECG where ECGCODE in(select TBLMODEL2ECG.ECGCODE from TBLMODEL2ECG,TBLMODEL2ITEM where TBLMODEL2ECG.MODELCODE=TBLMODEL2ITEM.MODELCODE and TBLMODEL2ITEM.ITEMCODE = '{1}' " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ") ORDER BY ecgdesc,ecgcode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)),itemCode)));
		}

		//ͨ�����ֻ�ȡErrorCodeGroup
		public object[] GetErrorCodeGroupByModel(string modelCode)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA),new SQLCondition(string.Format(" SELECT {0} FROM tblecg WHERE ecgcode IN ( SELECT tblmodel2ecg.ecgcode FROM tblmodel2ecg WHERE tblmodel2ecg.modelcode = '{1}')  ",DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)),modelCode)));
		}


		/// <summary>
		/// ** ��������:	�ж�һ��ErrorCode�Ƿ������һ��ErrorCodeGroup��
		/// ** �� ��:		vizo
		/// ** �� ��:		2005-06-10 11:32:15
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="errorGroupCode"></param>
		/// <returns>boolֵ</returns>
		public bool IsErrorCodeInGroup(string errorCode,string errorGroupCode)
		{
			object[] objs= GetSelectedErrorCodeByErrorCodeGroupCode( errorGroupCode , errorCode , 1 , int.MaxValue ) ;
			if( objs == null )
			{
				return false ;
			}

			foreach(ErrorCodeA obj in objs)
			{
				if(obj.ErrorCode == errorCode)
				{
					return true ;
				}
			}
			return false ;
		}
		#endregion

		#region ErrorCodeGroup2ErrorCode
		/// <summary>
		/// 
		/// </summary>
		public ErrorCodeGroup2ErrorCode CreateNewErrorCodeGroup2ErrorCode()
		{
			return new ErrorCodeGroup2ErrorCode();
		}

		public void AddErrorCodeGroup2ErrorCode( ErrorCodeGroup2ErrorCode errorCodeGroup2ErrorCode)
		{
			this._helper.AddDomainObject( errorCodeGroup2ErrorCode );
		}

		public void UpdateErrorCodeGroup2ErrorCode(ErrorCodeGroup2ErrorCode errorCodeGroup2ErrorCode)
		{
			this._helper.UpdateDomainObject( errorCodeGroup2ErrorCode );
		}

		public void DeleteErrorCodeGroup2ErrorCode(ErrorCodeGroup2ErrorCode errorCodeGroup2ErrorCode)
		{
			this._helper.DeleteDomainObject( errorCodeGroup2ErrorCode );
		}

		public void DeleteErrorCodeGroup2ErrorCode(ErrorCodeGroup2ErrorCode[] errorCodeGroup2ErrorCode)
		{
			this._helper.DeleteDomainObject( errorCodeGroup2ErrorCode );
		}

		public object GetErrorCodeGroup2ErrorCode( string errorCodeGroupCode, string errorCode )
		{
			return this.DataProvider.CustomSearch(typeof(ErrorCodeGroup2ErrorCode), new object[]{ errorCodeGroupCode, errorCode });
		}

		/// <summary>
		/// ** ��������:	��ѯErrorCodeGroup2ErrorCode��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup��ģ����ѯ</param>
		/// <param name="errorCode">ErrorCode��ģ����ѯ</param>
		/// <returns> ErrorCodeGroup2ErrorCode���ܼ�¼��</returns>
		public int QueryErrorCodeGroup2ErrorCodeCount( string errorCodeGroupCode, string errorCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECG2EC where 1=1 and ECGCODE like '{0}%'  and ECODE like '{1}%' " , FormatHelper.PKCapitalFormat(errorCodeGroupCode), FormatHelper.PKCapitalFormat(errorCode))));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯErrorCodeGroup2ErrorCode
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup��ģ����ѯ</param>
		/// <param name="errorCode">ErrorCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ErrorCodeGroup2ErrorCode����</returns>
		public object[] QueryErrorCodeGroup2ErrorCode( string errorCodeGroupCode, string errorCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroup2ErrorCode), new PagerCondition(string.Format("select {0} from TBLECG2EC where 1=1 and ECGCODE like '{1}%'  and ECODE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroup2ErrorCode)) , FormatHelper.PKCapitalFormat(errorCodeGroupCode), FormatHelper.PKCapitalFormat(errorCode)), "ECGCODE,ECODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ErrorCodeGroup2ErrorCode
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ErrorCodeGroup2ErrorCode���ܼ�¼��</returns>
		public object[] GetAllErrorCodeGroup2ErrorCode()
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroup2ErrorCode), new SQLCondition(string.Format("select {0} from TBLECG2EC order by ECGCODE,ECODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroup2ErrorCode)))));
		}

		public object GetErrorCodeGroup2ErrorCodeByecCode(string ecCode)
		{
			object[] objs = this.DataProvider.CustomQuery(typeof(ErrorCodeGroup2ErrorCode), new SQLCondition(string.Format("select {0} from TBLECG2EC  where ecode = '{1}'"
				, DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroup2ErrorCode))
				, ecCode)));

			if(objs != null && objs.Length > 0)
			{
				return objs[0];
			}
			else
			{
				return null;
			}
		}

		public void AddErrorCodeGroup2ErrorCode( ErrorCodeGroup2ErrorCode[] errorCodeGroup2ErrorCodes )
		{
			this._helper.AddDomainObject( errorCodeGroup2ErrorCodes );
		}

		#region ErrorCodeA --> ErrorCodeGroupA
		/// <summary>
		/// ** ��������:	��ErrorCode���ErrorCodeGroupA
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCode">ErrorCode,��ȷ��ѯ</param>
		/// <returns>ErrorCodeGroupA����</returns>
		public object[] GetErrorCodeGroupByErrorCodeCode(string errorCode)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA), new SQLCondition(string.Format("select {0} from TBLECG where ECGCODE in ( select ECGCODE from TBLECG2EC where ECODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)), errorCode)));
		}

		/// <summary>
		/// ** ��������:	��ErrorCode�������ErrorCodeA��ErrorCodeGroupA������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCode">ErrorCode,��ȷ��ѯ</param>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,ģ����ѯ</param>
		/// <returns>ErrorCodeGroupA������</returns>
		public int GetSelectedErrorCodeGroupByErrorCodeCodeCount(string errorCode, string errorCodeGroupCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECG2EC where ECODE ='{0}' and ECGCODE like '{1}%'", errorCode, FormatHelper.PKCapitalFormat(errorCodeGroupCode))));
		}

		/// <summary>
		/// ** ��������:	��ErrorCode�������ErrorCodeA��ErrorCodeGroupA����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCode">ErrorCode,��ȷ��ѯ</param>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>ErrorCodeGroupA����</returns>
		public object[] GetSelectedErrorCodeGroupByErrorCodeCode(string errorCode, string errorCodeGroupCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA), 
				new PagerCondition(string.Format("select {0} from TBLECG where ECGCODE in ( select ECGCODE from TBLECG2EC where ECODE ='{1}') and ECGCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)), errorCode, FormatHelper.PKCapitalFormat(errorCodeGroupCode)), "ECGCODE", inclusive, exclusive));
		}
		
		/// <summary>
		/// ** ��������:	��ErrorCode��ò�����ErrorCodeA��ErrorCodeGroupA������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCode">ErrorCode,��ȷ��ѯ</param>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,ģ����ѯ</param>
		/// <returns>ErrorCodeGroupA������</returns>
		public int GetUnselectedErrorCodeGroupByErrorCodeCodeCount(string errorCode, string errorCodeGroupCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECG where ECGCODE not in ( select ECGCODE from TBLECG2EC where ECODE ='{0}') and ECGCODE like '{1}%'", errorCode, FormatHelper.PKCapitalFormat(errorCodeGroupCode))));
		}

		/// <summary>
		/// ** ��������:	��ErrorCode��ò�����ErrorCodeA��ErrorCodeGroupA����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCode">ErrorCode,��ȷ��ѯ</param>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>ErrorCodeGroupA����</returns>
		public object[] GetUnselectedErrorCodeGroupByErrorCodeCode(string errorCode, string errorCodeGroupCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA), 
				new PagerCondition(string.Format("select {0} from TBLECG where ECGCODE not in ( select ECGCODE from TBLECG2EC where ECODE ='{1}') and ECGCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)), errorCode, FormatHelper.PKCapitalFormat(errorCodeGroupCode)), "ECGCODE", inclusive, exclusive));
		}
		#endregion 

		#region ErrorCodeGroupA --> ErrorCodeA
		/// <summary>
		/// ** ��������:	��ErrorCodeGroup���ErrorCodeA
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <returns>ErrorCodeA����</returns>
		public object[] GetErrorCodeByErrorCodeGroupCode(string errorCodeGroupCode)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeA), new SQLCondition(string.Format("select {0} from TBLEC where ECODE in ( select ECODE from TBLECG2EC where ECGCODE='{1}') ORDER BY ecdesc,ecode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeA)), errorCodeGroupCode)));
		}

		/// <summary>
		/// ** ��������:	��ErrorCodeGroup�������ErrorCodeGroupA��ErrorCodeA������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <param name="errorCode">ErrorCode,ģ����ѯ</param>
		/// <returns>ErrorCodeA������</returns>
		public int GetSelectedErrorCodeByErrorCodeGroupCodeCount(string errorCodeGroupCode, string errorCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECG2EC where ECGCODE ='{0}' and ECODE like '{1}%'", errorCodeGroupCode, FormatHelper.PKCapitalFormat(errorCode))));
		}

		/// <summary>
		/// ** ��������:	��ErrorCodeGroup�������ErrorCodeGroupA��ErrorCodeA����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <param name="errorCode">ErrorCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>ErrorCodeA����</returns>
		public object[] GetSelectedErrorCodeByErrorCodeGroupCode(string errorCodeGroupCode, string errorCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeA), 
				new PagerCondition(string.Format("select /*+ leading(TBLEC) */  {0} from TBLEC where ECODE in ( select ECODE from TBLECG2EC where ECGCODE ='{1}') and ECODE like '{2}%' order by ECODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeA)), errorCodeGroupCode, FormatHelper.PKCapitalFormat(errorCode)), "ECODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	��ErrorCodeGroup�������ErrorCodeGroupA��ErrorCodeA����ҳ
		/// ** �� ��:		Laws Lu
		/// ** �� ��:		2005-09-08
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <param name="errorCode">ErrorCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>ErrorCodeA����</returns>
		public object[] GetSelectedErrorCodeByErrorCodeGroupCode(string errorCodeGroupCode)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeA), 
				new SQLCondition(string.Format("select /*+ leading(TBLEC) */  {0} from TBLEC where ECODE in ( select ECODE from TBLECG2EC where ECGCODE ='{1}') order by ECODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeA)),errorCodeGroupCode)));
		}
		
		/// <summary>
		/// ** ��������:	��ErrorCodeGroup��ò�����ErrorCodeGroupA��ErrorCodeA������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <param name="errorCode">ErrorCode,ģ����ѯ</param>
		/// <returns>ErrorCodeA������</returns>
		public int GetUnselectedErrorCodeByErrorCodeGroupCodeCount(string errorCodeGroupCode, string errorCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLEC where ECODE not in ( select ECODE from TBLECG2EC where ECGCODE ='{0}') and ECODE like '{1}%'", errorCodeGroupCode, FormatHelper.PKCapitalFormat(errorCode))));
		}

		/// <summary>
		/// ** ��������:	��ErrorCodeGroup��ò�����ErrorCodeGroupA��ErrorCodeA����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <param name="errorCode">ErrorCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>ErrorCodeA����</returns>
		public object[] GetUnselectedErrorCodeByErrorCodeGroupCode(string errorCodeGroupCode, string errorCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeA), 
				new PagerCondition(string.Format("select {0} from TBLEC where ECODE not in ( select ECODE from TBLECG2EC where ECGCODE ='{1}') and ECODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeA)), errorCodeGroupCode, FormatHelper.PKCapitalFormat(errorCode)), "ECODE", inclusive, exclusive));
		}
		#endregion 


		#endregion

		#region Model2ErrorCause
		/// <summary>
		/// 
		/// </summary>
		public Model2ErrorCause CreateNewModel2ErrorCause()
		{
			return new Model2ErrorCause();
		}

		public void AddModel2ErrorCause( Model2ErrorCause model2ErrorCause)
		{
			this._helper.AddDomainObject( model2ErrorCause );
		}

		public void UpdateModel2ErrorCause(Model2ErrorCause model2ErrorCause)
		{
			this._helper.UpdateDomainObject( model2ErrorCause );
		}

		public void DeleteModel2ErrorCause(Model2ErrorCause model2ErrorCause)
		{
			this._helper.DeleteDomainObject( model2ErrorCause );
		}

		public void DeleteModel2ErrorCause(Model2ErrorCause[] model2ErrorCause)
		{
			this._helper.DeleteDomainObject( model2ErrorCause );
		}

		public object GetModel2ErrorCause( string errorCauseCode, string modelCode )
		{
			return this.DataProvider.CustomSearch(typeof(Model2ErrorCause), new object[]{ errorCauseCode, modelCode });
		}

		/// <summary>
		/// ** ��������:	��ѯModel2ErrorCause��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-04-04 9:27:59
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCauseCode">ErrorCauseCode��ģ����ѯ</param>
		/// <param name="modelCode">ModelCode��ģ����ѯ</param>
		/// <returns> Model2ErrorCause���ܼ�¼��</returns>
		public int QueryModel2ErrorCauseCount( string errorCauseCode, string modelCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2ECS where 1=1 and ECSCODE like '{0}%'  and MODELCODE like '{1}%' " , FormatHelper.PKCapitalFormat(errorCauseCode), FormatHelper.PKCapitalFormat(modelCode))));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯModel2ErrorCause
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-04-04 9:27:59
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCauseCode">ErrorCauseCode��ģ����ѯ</param>
		/// <param name="modelCode">ModelCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> Model2ErrorCause����</returns>
		public object[] QueryModel2ErrorCause( string errorCauseCode, string modelCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(Model2ErrorCause), new PagerCondition(string.Format("select {0} from TBLMODEL2ECS where 1=1 and ECSCODE like '{1}%'  and MODELCODE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model2ErrorCause)) , FormatHelper.PKCapitalFormat(errorCauseCode), FormatHelper.PKCapitalFormat(modelCode)), "ECSCODE,MODELCODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�Model2ErrorCause
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-04-04 9:27:59
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>Model2ErrorCause���ܼ�¼��</returns>
		public object[] GetAllModel2ErrorCause()
		{
			return this.DataProvider.CustomQuery(typeof(Model2ErrorCause), new SQLCondition(string.Format("select {0} from TBLMODEL2ECS order by ECSCODE,MODELCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model2ErrorCause)))));
		}

		public void AddModel2ErrorCause( Model2ErrorCause[] model2ErrorCauses )
		{
			this._helper.AddDomainObject( model2ErrorCauses );
		}

		#region Model --> ErrorCause
		/// <summary>
		/// ** ��������:	��ModelCode���ErrorCause
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-04-04 9:27:59
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <returns>ErrorCause����</returns>
		public object[] GetErrorCauseByModelCode(string modelCode)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCause), new SQLCondition(string.Format("select {0} from TBLECS where ECSCODE in ( select ECSCODE from TBLMODEL2ECS where MODELCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCause)), modelCode)));
		}

		/// <summary>
		/// ** ��������:	��ModelCode�������Model��ErrorCause������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-04-04 9:27:59
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="errorCauseCode">ErrorCauseCode,ģ����ѯ</param>
		/// <returns>ErrorCause������</returns>
		public int GetSelectedErrorCauseByModelCodeCount(string modelCode, string errorCauseCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2ECS where MODELCODE ='{0}' and ECSCODE like '{1}%'", modelCode, FormatHelper.PKCapitalFormat(errorCauseCode))));
		}

		/// <summary>
		/// ** ��������:	��ModelCode�������Model��ErrorCause����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-04-04 9:27:59
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="errorCauseCode">ErrorCauseCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>ErrorCause����</returns>
		public object[] GetSelectedErrorCauseByModelCode(string modelCode, string errorCauseCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCause), 
				new PagerCondition(string.Format("select {0} from TBLECS where ECSCODE in ( select ECSCODE from TBLMODEL2ECS where MODELCODE ='{1}') and ECSCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCause)), modelCode, FormatHelper.PKCapitalFormat(errorCauseCode)), "ECSCODE", inclusive, exclusive));
		}
		
		/// <summary>
		/// ** ��������:	��ModelCode��ò�����Model��ErrorCause������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-04-04 9:27:59
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="errorCauseCode">ErrorCauseCode,ģ����ѯ</param>
		/// <returns>ErrorCause������</returns>
		public int GetUnselectedErrorCauseByModelCodeCount(string modelCode, string errorCauseCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECS where ECSCODE not in ( select ECSCODE from TBLMODEL2ECS where MODELCODE ='{0}') and ECSCODE like '{1}%'", modelCode, FormatHelper.PKCapitalFormat(errorCauseCode))));
		}

		/// <summary>
		/// ** ��������:	��ModelCode��ò�����Model��ErrorCause����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-04-04 9:27:59
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="errorCauseCode">ErrorCauseCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>ErrorCause����</returns>
		public object[] GetUnselectedErrorCauseByModelCode(string modelCode, string errorCauseCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCause), 
				new PagerCondition(string.Format("select {0} from TBLECS where ECSCODE not in ( select ECSCODE from TBLMODEL2ECS where MODELCODE ='{1}') and ECSCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCause)), modelCode, FormatHelper.PKCapitalFormat(errorCauseCode)), "ECSCODE", inclusive, exclusive));
		}
		#endregion 

		#endregion

		#region Model2ErrorCauseGroup
		/// <summary>
		/// 
		/// </summary>
		public Model2ErrorCauseGroup CreateNewModel2ErrorCauseGroup()
		{
			return new Model2ErrorCauseGroup();
		}

		public void AddModel2ErrorCauseGroup( Model2ErrorCauseGroup model2ErrorCauseGroup)
		{
			this._helper.AddDomainObject( model2ErrorCauseGroup );
		}

		public void AddModel2ErrorCauseGroup( Model2ErrorCauseGroup[] model2ErrorCauseGroup)
		{
			this._helper.AddDomainObject( model2ErrorCauseGroup );
		}

		public void UpdateModel2ErrorCauseGroup(Model2ErrorCauseGroup model2ErrorCauseGroup)
		{
			this._helper.UpdateDomainObject( model2ErrorCauseGroup );
		}

		public void DeleteModel2ErrorCauseGroup(Model2ErrorCauseGroup model2ErrorCauseGroup)
		{
			this._helper.DeleteDomainObject( model2ErrorCauseGroup );
		}

		public void DeleteModel2ErrorCauseGroup(Model2ErrorCauseGroup[] model2ErrorCauseGroup)
		{
			this._helper.DeleteDomainObject( model2ErrorCauseGroup );
		}

		public object GetModel2ErrorCauseGroup( string modelCode, string errorCauseGroupCode )
		{
			return this.DataProvider.CustomSearch(typeof(Model2ErrorCauseGroup), new object[]{ modelCode, errorCauseGroupCode });
		}

		/// <summary>
		/// ** ��������:	��ѯModel2ErrorCauseGroup��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-26 13:34:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode��ģ����ѯ</param>
		/// <param name="errorCauseGroupCode">ErrorCauseGroupCode��ģ����ѯ</param>
		/// <returns> Model2ErrorCauseGroup���ܼ�¼��</returns>
		public int QueryModel2ErrorCauseGroupCount( string modelCode, string errorCauseGroupCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2ECSG where 1=1 and MODELCODE like '{0}%'  and ECSGCODE like '{1}%' " , modelCode, errorCauseGroupCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯModel2ErrorCauseGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-26 13:34:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode��ģ����ѯ</param>
		/// <param name="errorCauseGroupCode">ErrorCauseGroupCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> Model2ErrorCauseGroup����</returns>
		public object[] QueryModel2ErrorCauseGroup( string modelCode, string errorCauseGroupCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(Model2ErrorCauseGroup), new PagerCondition(string.Format("select {0} from TBLMODEL2ECSG where 1=1 and MODELCODE like '{1}%'  and ECSGCODE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model2ErrorCauseGroup)) , modelCode, errorCauseGroupCode), "MODELCODE,ECSGCODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�Model2ErrorCauseGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-26 13:34:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>Model2ErrorCauseGroup���ܼ�¼��</returns>
		public object[] GetAllModel2ErrorCauseGroup()
		{
			return this.DataProvider.CustomQuery(typeof(Model2ErrorCauseGroup), new SQLCondition(string.Format("select {0} from TBLMODEL2ECSG order by MODELCODE,ECSGCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model2ErrorCauseGroup)))));
		}

		/// <summary>
		/// �Ѿ�ѡ���Cause Group
		/// </summary>
		/// <param name="modelCode"></param>
		/// <param name="errorCauseCode"></param>
		/// <returns></returns>
		public int GetSelectedErrorCauseGroupByModelCodeCount(string modelCode, string errorCauseGroupCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2ECSG where MODELCODE ='{0}' and ECSGCODE like '{1}%'", modelCode, FormatHelper.PKCapitalFormat(errorCauseGroupCode))));
		}

		
		public object[] GetSelectedErrorCauseGroupByModelCode(string modelCode, string errorCauseCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCauseGroup), 
				new PagerCondition(string.Format("select {0} from TBLECSG where ECSGCODE in ( select ECSGCODE from TBLMODEL2ECSG where MODELCODE ='{1}') and ECSGCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCauseGroup)), modelCode, FormatHelper.PKCapitalFormat(errorCauseCode)), "ECSGCODE", inclusive, exclusive));
		}
		
		/// <summary>
		/// δѡ���Cause Group
		/// </summary>
		public int GetUnselectedErrorCauseGroupByModelCodeCount(string modelCode, string errorCauseGroupCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECSG where ECSGCODE not in ( select ECSGCODE from TBLMODEL2ECSG where MODELCODE ='{0}') and ECSGCODE like '{1}%'", modelCode, FormatHelper.PKCapitalFormat(errorCauseGroupCode))));
		}

		
		public object[] GetUnselectedErrorCauseGroupByModelCode(string modelCode, string errorCauseGroupCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCauseGroup), 
				new PagerCondition(string.Format("select {0} from TBLECSG where ECSGCODE not in ( select ECSGCODE from TBLMODEL2ECSG where MODELCODE ='{1}') and ECSGCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCauseGroup)), modelCode, FormatHelper.PKCapitalFormat(errorCauseGroupCode)), "ECSGCODE", inclusive, exclusive));
		}
		#endregion

		#region ItemRouteOp2ErrorCauseGroup
		/// <summary>
		/// 
		/// </summary>
		public ItemRouteOp2ErrorCauseGroup CreateNewItemRouteOp2ErrorCauseGroup()
		{
			return new ItemRouteOp2ErrorCauseGroup();
		}

		public void AddItemRouteOp2ErrorCauseGroup( ItemRouteOp2ErrorCauseGroup itemRouteOp2ErrorCauseGroup)
		{
			//�˲�Ʒ����;�����������ԭ����ֻ����һ��
			int c = this.DataProvider.GetCount(
											new SQLCondition(
															string.Format("select count(*) from TBLITEMROUTEOP2ECSG where 1=1 and itemcode='{0}' and routecode='{1}' and ecsgcode='{2}'" 
																		,itemRouteOp2ErrorCauseGroup.ItemCode
																		,itemRouteOp2ErrorCauseGroup.RouteCode
																		,itemRouteOp2ErrorCauseGroup.ErrorCauseGroupCode)
															)
											);
			if(c > 0)
				ExceptionManager.Raise(this.GetType(),"$Error_Item_Route_ErrorCauseGroup_Exist");

			c = 0;
			c = this.DataProvider.GetCount(	new SQLCondition(
												string.Format("select count(*) from TBLITEMROUTEOP2ECSG where 1=1 and opid='{0}'" 
																,itemRouteOp2ErrorCauseGroup.OpID
																)
															)
												);
			if(c > 0)
				ExceptionManager.Raise(this.GetType(),"$Error_Unique_ErrorCauseGroup_Op");

			this._helper.AddDomainObject( itemRouteOp2ErrorCauseGroup );
		}

		public void UpdateItemRouteOp2ErrorCauseGroup(ItemRouteOp2ErrorCauseGroup itemRouteOp2ErrorCauseGroup)
		{
			this._helper.UpdateDomainObject( itemRouteOp2ErrorCauseGroup );
		}

		public void DeleteItemRouteOp2ErrorCauseGroup(ItemRouteOp2ErrorCauseGroup itemRouteOp2ErrorCauseGroup)
		{
			this._helper.DeleteDomainObject( itemRouteOp2ErrorCauseGroup );
		}

		public void DeleteItemRouteOp2ErrorCauseGroup(ItemRouteOp2ErrorCauseGroup[] itemRouteOp2ErrorCauseGroup)
		{
			this._helper.DeleteDomainObject( itemRouteOp2ErrorCauseGroup );
		}

		public object GetItemRouteOp2ErrorCauseGroup( string opID, string errorCauseGroupCode )
		{
			return this.DataProvider.CustomSearch(typeof(ItemRouteOp2ErrorCauseGroup), new object[]{ opID, errorCauseGroupCode });
		}

		/// <summary>
		/// ** ��������:	��ѯItemRouteOp2ErrorCauseGroup��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-27 15:43:19
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="opID">OpID��ģ����ѯ</param>
		/// <param name="errorCauseGroupCode">ErrorCauseGroupCode��ģ����ѯ</param>
		/// <returns> ItemRouteOp2ErrorCauseGroup���ܼ�¼��</returns>
		public int QueryItemRouteOp2ErrorCauseGroupCount( string opID, string errorCauseGroupCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLITEMROUTEOP2ECSG where 1=1 and OpID like '{0}%'  and ECSGCode like '{1}%' " , opID, errorCauseGroupCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯItemRouteOp2ErrorCauseGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-27 15:43:19
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="opID">OpID��ģ����ѯ</param>
		/// <param name="errorCauseGroupCode">ErrorCauseGroupCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ItemRouteOp2ErrorCauseGroup����</returns>
		public object[] QueryItemRouteOp2ErrorCauseGroup( string opID, string errorCauseGroupCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ItemRouteOp2ErrorCauseGroup), new PagerCondition(string.Format("select {0} from TBLITEMROUTEOP2ECSG where 1=1 and OpID like '{1}%'  and ECSGCode like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ItemRouteOp2ErrorCauseGroup)) , opID, errorCauseGroupCode), "OpID,ECSGCode", inclusive, exclusive));
		}

		public object[] QueryItemRouteOp2ErrorCauseGroup( string itemcode,string route, string errorCauseGroupCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ItemRouteOp2ErrorCauseGroup), new PagerCondition(string.Format("select {0} from TBLITEMROUTEOP2ECSG where 1=1 and itemcode like '{1}' and routecode like '{2}' and ECSGCode like '{3}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ItemRouteOp2ErrorCauseGroup)) , itemcode,route, errorCauseGroupCode), "itemcode,routecode,ECSGCode", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ItemRouteOp2ErrorCauseGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-27 15:43:19
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ItemRouteOp2ErrorCauseGroup���ܼ�¼��</returns>
		public object[] GetAllItemRouteOp2ErrorCauseGroup()
		{
			return this.DataProvider.CustomQuery(typeof(ItemRouteOp2ErrorCauseGroup), new SQLCondition(string.Format("select {0} from TBLITEMROUTEOP2ECSG order by OpID,ECSGCode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ItemRouteOp2ErrorCauseGroup)))));
		}
		#endregion

		#region Model2ErrorCodeGroup
		/// <summary>
		/// 
		/// </summary>
		public Model2ErrorCodeGroup CreateNewModel2ErrorCodeGroup()
		{
			return new Model2ErrorCodeGroup();
		}

		public void AddModel2ErrorCodeGroup( Model2ErrorCodeGroup model2ErrorCodeGroup)
		{
			this._helper.AddDomainObject( model2ErrorCodeGroup );
		}

		public void UpdateModel2ErrorCodeGroup(Model2ErrorCodeGroup model2ErrorCodeGroup)
		{
			this._helper.UpdateDomainObject( model2ErrorCodeGroup );
		}

		public void DeleteModel2ErrorCodeGroup(Model2ErrorCodeGroup model2ErrorCodeGroup)
		{
			this._helper.DeleteDomainObject( model2ErrorCodeGroup );
		}

		public void DeleteModel2ErrorCodeGroup(Model2ErrorCodeGroup[] model2ErrorCodeGroup)
		{
			this._helper.DeleteDomainObject( model2ErrorCodeGroup );
		}

		public object GetModel2ErrorCodeGroup( string errorCodeGroupCode, string modelCode )
		{
			return this.DataProvider.CustomSearch(typeof(Model2ErrorCodeGroup), new object[]{ errorCodeGroupCode, modelCode });
		}

		/// <summary>
		/// ** ��������:	��ѯModel2ErrorCodeGroup��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup��ģ����ѯ</param>
		/// <param name="modelCode">ModelCode��ģ����ѯ</param>
		/// <returns> Model2ErrorCodeGroup���ܼ�¼��</returns>
		public int QueryModel2ErrorCodeGroupCount( string errorCodeGroupCode, string modelCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2ECG where 1=1 and ECGCODE like '{0}%'  and MODELCODE like '{1}%' " , FormatHelper.PKCapitalFormat(errorCodeGroupCode), FormatHelper.PKCapitalFormat(modelCode))));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯModel2ErrorCodeGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup��ģ����ѯ</param>
		/// <param name="modelCode">ModelCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> Model2ErrorCodeGroup����</returns>
		public object[] QueryModel2ErrorCodeGroup( string errorCodeGroupCode, string modelCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(Model2ErrorCodeGroup), new PagerCondition(string.Format("select {0} from TBLMODEL2ECG where 1=1 and ECGCODE like '{1}%'  and MODELCODE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model2ErrorCodeGroup)) , FormatHelper.PKCapitalFormat(errorCodeGroupCode), FormatHelper.PKCapitalFormat(modelCode)), "ECGCODE,MODELCODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�Model2ErrorCodeGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>Model2ErrorCodeGroup���ܼ�¼��</returns>
		public object[] GetAllModel2ErrorCodeGroup()
		{
			return this.DataProvider.CustomQuery(typeof(Model2ErrorCodeGroup), new SQLCondition(string.Format("select {0} from TBLMODEL2ECG order by ECGCODE,MODELCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model2ErrorCodeGroup)))));
		}

		public void AddModel2ErrorCodeGroup( Model2ErrorCodeGroup[] model2ErrorCodeGroups )
		{
			this._helper.AddDomainObject( model2ErrorCodeGroups );
		}

		#region Model --> ErrorCodeGroupA
		/// <summary>
		/// ** ��������:	��ModelCode���ErrorCodeGroupA
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <returns>ErrorCodeGroupA����</returns>
		public object[] GetErrorCodeGroupByModelCode(string modelCode)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA), new SQLCondition(string.Format("select {0} from TBLECG where ECGCODE in ( select ECGCODE from TBLMODEL2ECG where MODELCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)), modelCode)));
		}

		/// <summary>
		/// ** ��������:	��ModelCode�������Model��ErrorCodeGroupA������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,ģ����ѯ</param>
		/// <returns>ErrorCodeGroupA������</returns>
		public int GetSelectedErrorCodeGroupByModelCodeCount(string modelCode, string errorCodeGroupCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2ECG where MODELCODE ='{0}' and ECGCODE like '{1}%'", modelCode, FormatHelper.PKCapitalFormat(errorCodeGroupCode))));
		}

		/// <summary>
		/// ** ��������:	��ModelCode�������Model��ErrorCodeGroupA����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>ErrorCodeGroupA����</returns>
		public object[] GetSelectedErrorCodeGroupByModelCode(string modelCode, string errorCodeGroupCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA), 
				new PagerCondition(string.Format("select {0} from TBLECG where ECGCODE in ( select ECGCODE from TBLMODEL2ECG where MODELCODE ='{1}') and ECGCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)), modelCode, FormatHelper.PKCapitalFormat(errorCodeGroupCode)), "ECGCODE", inclusive, exclusive));
		}
		
		/// <summary>
		/// ** ��������:	��ModelCode��ò�����Model��ErrorCodeGroupA������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,ģ����ѯ</param>
		/// <returns>ErrorCodeGroupA������</returns>
		public int GetUnselectedErrorCodeGroupByModelCodeCount(string modelCode, string errorCodeGroupCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLECG where ECGCODE not in ( select ECGCODE from TBLMODEL2ECG where MODELCODE ='{0}') and ECGCODE like '{1}%'", modelCode, FormatHelper.PKCapitalFormat(errorCodeGroupCode))));
		}

		/// <summary>
		/// ** ��������:	��ModelCode��ò�����Model��ErrorCodeGroupA����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>ErrorCodeGroupA����</returns>
		public object[] GetUnselectedErrorCodeGroupByModelCode(string modelCode, string errorCodeGroupCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(ErrorCodeGroupA), 
				new PagerCondition(string.Format("select {0} from TBLECG where ECGCODE not in ( select ECGCODE from TBLMODEL2ECG where MODELCODE ='{1}') and ECGCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroupA)), modelCode, FormatHelper.PKCapitalFormat(errorCodeGroupCode)), "ECGCODE", inclusive, exclusive));
		}
		#endregion 

		#region ErrorCodeGroupA --> Model
		/// <summary>
		/// ** ��������:	��ErrorCodeGroup���Model
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <returns>Model����</returns>
		public object[] GetModelByErrorCodeGroupCode(string errorCodeGroupCode)
		{
            return this.DataProvider.CustomQuery(typeof(Model), new SQLCondition(string.Format("select {0} from TBLMODEL where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and MODELCODE in ( select MODELCODE from TBLMODEL2ECG where ECGCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model)), errorCodeGroupCode)));
		}

		/// <summary>
		/// ** ��������:	��ErrorCodeGroup�������ErrorCodeGroupA��Model������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <param name="modelCode">ModelCode,ģ����ѯ</param>
		/// <returns>Model������</returns>
		public int GetSelectedModelByErrorCodeGroupCodeCount(string errorCodeGroupCode, string modelCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2ECG where ECGCODE ='{0}' and MODELCODE like '{1}%'", errorCodeGroupCode, FormatHelper.PKCapitalFormat(modelCode))));
		}

		/// <summary>
		/// ** ��������:	��ErrorCodeGroup�������ErrorCodeGroupA��Model����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <param name="modelCode">ModelCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>Model����</returns>
		public object[] GetSelectedModelByErrorCodeGroupCode(string errorCodeGroupCode, string modelCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(Model),
                new PagerCondition(string.Format("select {0} from TBLMODEL where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and MODELCODE in ( select MODELCODE from TBLMODEL2ECG where ECGCODE ='{1}') and MODELCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model)), errorCodeGroupCode, FormatHelper.PKCapitalFormat(modelCode)), "MODELCODE", inclusive, exclusive));
		}
		
		/// <summary>
		/// ** ��������:	��ErrorCodeGroup��ò�����ErrorCodeGroupA��Model������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <param name="modelCode">ModelCode,ģ����ѯ</param>
		/// <returns>Model������</returns>
		public int GetUnselectedModelByErrorCodeGroupCodeCount(string errorCodeGroupCode, string modelCode)
		{
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and MODELCODE not in ( select MODELCODE from TBLMODEL2ECG where ECGCODE ='{0}') and MODELCODE like '{1}%'", errorCodeGroupCode, FormatHelper.PKCapitalFormat(modelCode))));
		}

		/// <summary>
		/// ** ��������:	��ErrorCodeGroup��ò�����ErrorCodeGroupA��Model����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="errorCodeGroupCode">ErrorCodeGroup,��ȷ��ѯ</param>
		/// <param name="modelCode">ModelCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>Model����</returns>
		public object[] GetUnselectedModelByErrorCodeGroupCode(string errorCodeGroupCode, string modelCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(Model),
                new PagerCondition(string.Format("select {0} from TBLMODEL where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and MODELCODE not in ( select MODELCODE from TBLMODEL2ECG where ECGCODE ='{1}') and MODELCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model)), errorCodeGroupCode, FormatHelper.PKCapitalFormat(modelCode)), "MODELCODE", inclusive, exclusive));
		}
		#endregion 


		#endregion

		#region Model2Solution
		/// <summary>
		/// 
		/// </summary>
		public Model2Solution CreateNewModel2Solution()
		{
			return new Model2Solution();
		}

		public void AddModel2Solution( Model2Solution model2Solution)
		{
			this._helper.AddDomainObject( model2Solution );
		}

		public void UpdateModel2Solution(Model2Solution model2Solution)
		{
			this._helper.UpdateDomainObject( model2Solution );
		}

		public void DeleteModel2Solution(Model2Solution model2Solution)
		{
			this._helper.DeleteDomainObject( model2Solution );
		}

		public void DeleteModel2Solution(Model2Solution[] model2Solution)
		{
			this._helper.DeleteDomainObject( model2Solution );
		}

		public object GetModel2Solution( string solutionCode, string modelCode )
		{
			return this.DataProvider.CustomSearch(typeof(Model2Solution), new object[]{ solutionCode, modelCode });
		}

		/// <summary>
		/// ** ��������:	��ѯModel2Solution��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solutionCode">SolutionCode��ģ����ѯ</param>
		/// <param name="modelCode">ModelCode��ģ����ѯ</param>
		/// <returns> Model2Solution���ܼ�¼��</returns>
		public int QueryModel2SolutionCount( string solutionCode, string modelCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2SOLUTION where 1=1 and SOLCODE like '{0}%'  and MODELCODE like '{1}%' " , FormatHelper.PKCapitalFormat(solutionCode), FormatHelper.PKCapitalFormat(modelCode))));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯModel2Solution
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solutionCode">SolutionCode��ģ����ѯ</param>
		/// <param name="modelCode">ModelCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> Model2Solution����</returns>
		public object[] QueryModel2Solution( string solutionCode, string modelCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(Model2Solution), new PagerCondition(string.Format("select {0} from TBLMODEL2SOLUTION where 1=1 and SOLCODE like '{1}%'  and MODELCODE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model2Solution)) , FormatHelper.PKCapitalFormat(solutionCode), FormatHelper.PKCapitalFormat(modelCode)), "SOLCODE,MODELCODE", inclusive, exclusive));
		}

        public object[] QueryModel2SolutionNew(string solutionCode, string modelCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(Model2Solution), new PagerCondition(string.Format("select {0} from TBLMODEL2SOLUTION where 1=1 and SOLCODE like '{1}%'  and MODELCODE = '{2}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model2Solution)), FormatHelper.PKCapitalFormat(solutionCode), FormatHelper.PKCapitalFormat(modelCode)), "SOLCODE,MODELCODE", inclusive, exclusive));
        }

		/// <summary>
		/// ** ��������:	������е�Model2Solution
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>Model2Solution���ܼ�¼��</returns>
		public object[] GetAllModel2Solution()
		{
			return this.DataProvider.CustomQuery(typeof(Model2Solution), new SQLCondition(string.Format("select {0} from TBLMODEL2SOLUTION order by SOLCODE,MODELCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model2Solution)))));
		}

		public void AddModel2Solution( Model2Solution[] model2Solutions )
		{
			this._helper.AddDomainObject( model2Solutions );
		}

		#region Model --> Solution
		/// <summary>
		/// ** ��������:	��ModelCode���Solution
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <returns>Solution����</returns>
		public object[] GetSolutionByModelCode(string modelCode)
		{
			return this.DataProvider.CustomQuery(typeof(Solution), new SQLCondition(string.Format("select {0} from TBLSOLUTION where SOLCODE in ( select SOLCODE from TBLMODEL2SOLUTION where MODELCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Solution)), modelCode)));
		}

		/// <summary>
		/// ** ��������:	��ModelCode�������Model��Solution������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="solutionCode">SolutionCode,ģ����ѯ</param>
		/// <returns>Solution������</returns>
		public int GetSelectedSolutionByModelCodeCount(string modelCode, string solutionCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2SOLUTION where MODELCODE ='{0}' and SOLCODE like '{1}%'", modelCode, FormatHelper.PKCapitalFormat(solutionCode))));
		}

		/// <summary>
		/// ** ��������:	��ModelCode�������Model��Solution����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="solutionCode">SolutionCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>Solution����</returns>
		public object[] GetSelectedSolutionByModelCode(string modelCode, string solutionCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(Solution), 
				new PagerCondition(string.Format("select {0} from TBLSOLUTION where SOLCODE in ( select SOLCODE from TBLMODEL2SOLUTION where MODELCODE ='{1}') and SOLCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Solution)), modelCode, FormatHelper.PKCapitalFormat(solutionCode)), "SOLCODE", inclusive, exclusive));
		}
		
		/// <summary>
		/// ** ��������:	��ModelCode��ò�����Model��Solution������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="solutionCode">SolutionCode,ģ����ѯ</param>
		/// <returns>Solution������</returns>
		public int GetUnselectedSolutionByModelCodeCount(string modelCode, string solutionCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSOLUTION where SOLCODE not in ( select SOLCODE from TBLMODEL2SOLUTION where MODELCODE ='{0}') and SOLCODE like '{1}%'", modelCode, FormatHelper.PKCapitalFormat(solutionCode))));
		}

		/// <summary>
		/// ** ��������:	��ModelCode��ò�����Model��Solution����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="modelCode">ModelCode,��ȷ��ѯ</param>
		/// <param name="solutionCode">SolutionCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>Solution����</returns>
		public object[] GetUnselectedSolutionByModelCode(string modelCode, string solutionCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(Solution), 
				new PagerCondition(string.Format("select {0} from TBLSOLUTION where SOLCODE not in ( select SOLCODE from TBLMODEL2SOLUTION where MODELCODE ='{1}') and SOLCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Solution)), modelCode, FormatHelper.PKCapitalFormat(solutionCode)), "SOLCODE", inclusive, exclusive));
		}
		#endregion 

		#region Solution --> Model
		/// <summary>
		/// ** ��������:	��SolutionCode���Model
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solutionCode">SolutionCode,��ȷ��ѯ</param>
		/// <returns>Model����</returns>
		public object[] GetModelBySolutionCode(string solutionCode)
		{
            return this.DataProvider.CustomQuery(typeof(Model), new SQLCondition(string.Format("select {0} from TBLMODEL where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and MODELCODE in ( select MODELCODE from TBLMODEL2SOLUTION where SOLCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model)), solutionCode)));
		}

		/// <summary>
		/// ** ��������:	��SolutionCode�������Solution��Model������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solutionCode">SolutionCode,��ȷ��ѯ</param>
		/// <param name="modelCode">ModelCode,ģ����ѯ</param>
		/// <returns>Model������</returns>
		public int GetSelectedModelBySolutionCodeCount(string solutionCode, string modelCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL2SOLUTION where SOLCODE ='{0}' and MODELCODE like '{1}%'", solutionCode, FormatHelper.PKCapitalFormat(modelCode))));
		}

		/// <summary>
		/// ** ��������:	��SolutionCode�������Solution��Model����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solutionCode">SolutionCode,��ȷ��ѯ</param>
		/// <param name="modelCode">ModelCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>Model����</returns>
		public object[] GetSelectedModelBySolutionCode(string solutionCode, string modelCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(Model),
                new PagerCondition(string.Format("select {0} from TBLMODEL where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and MODELCODE in ( select MODELCODE from TBLMODEL2SOLUTION where SOLCODE ='{1}') and MODELCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model)), solutionCode, FormatHelper.PKCapitalFormat(modelCode)), "MODELCODE", inclusive, exclusive));
		}
		
		/// <summary>
		/// ** ��������:	��SolutionCode��ò�����Solution��Model������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solutionCode">SolutionCode,��ȷ��ѯ</param>
		/// <param name="modelCode">ModelCode,ģ����ѯ</param>
		/// <returns>Model������</returns>
		public int GetUnselectedModelBySolutionCodeCount(string solutionCode, string modelCode)
		{
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMODEL where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and MODELCODE not in ( select MODELCODE from TBLMODEL2SOLUTION where SOLCODE ='{0}') and MODELCODE like '{1}%'", solutionCode, FormatHelper.PKCapitalFormat(modelCode))));
		}

		/// <summary>
		/// ** ��������:	��SolutionCode��ò�����Solution��Model����ҳ
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solutionCode">SolutionCode,��ȷ��ѯ</param>
		/// <param name="modelCode">ModelCode,ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns>Model����</returns>
		public object[] GetUnselectedModelBySolutionCode(string solutionCode, string modelCode, int inclusive, int exclusive)
		{
			return this.DataProvider.CustomQuery(typeof(Model),
                new PagerCondition(string.Format("select {0} from TBLMODEL where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and MODELCODE not in ( select MODELCODE from TBLMODEL2SOLUTION where SOLCODE ='{1}') and MODELCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Model)), solutionCode, FormatHelper.PKCapitalFormat(modelCode)), "MODELCODE", inclusive, exclusive));
		}
		#endregion 


		#endregion

		#region Solution
		/// <summary>
		/// 
		/// </summary>
		public Solution CreateNewSolution()
		{
			return new Solution();
		}

		public void AddSolution( Solution solution)
		{
			this._helper.AddDomainObject( solution );
		}

		public void UpdateSolution(Solution solution)
		{
			this._helper.UpdateDomainObject( solution );
		}

		public void DeleteSolution(Solution solution)
		{
			this._helper.DeleteDomainObject( solution, 
				new ICheck[]{ new DeleteAssociateCheck( solution,
								this.DataProvider, 
								new Type[]{
											  typeof(Model2Solution)	})} );
		}

		public void DeleteSolution(Solution[] solution)
		{
			this._helper.DeleteDomainObject( solution, 
				new ICheck[]{ new DeleteAssociateCheck( solution,
								this.DataProvider, 
								new Type[]{
											  typeof(Model2Solution)	})} );
		}

		public object GetSolution( string solutionCode )
		{
			return this.DataProvider.CustomSearch(typeof(Solution), new object[]{ solutionCode });
		}

		/// <summary>
		/// ** ��������:	��ѯSolution��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solutionCode">SolutionCode��ģ����ѯ</param>
		/// <returns> Solution���ܼ�¼��</returns>
		public int QuerySolutionCount( string solutionCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSOLUTION where 1=1 and SOLCODE like '{0}%' " , FormatHelper.PKCapitalFormat(solutionCode))));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSolution
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solutionCode">SolutionCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> Solution����</returns>
		public object[] QuerySolution( string solutionCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(Solution), new PagerCondition(string.Format("select {0} from TBLSOLUTION where 1=1 and SOLCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Solution)) , FormatHelper.PKCapitalFormat(solutionCode)), "SOLCODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�Solution
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-24 13:28:05
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>Solution���ܼ�¼��</returns>
		public object[] GetAllSolution()
		{
			return this.DataProvider.CustomQuery(typeof(Solution), new SQLCondition(string.Format("select {0} from TBLSOLUTION order by SOLCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Solution)))));
		}


		#endregion

		#region Help Method
		public bool CheckDutyCodeIsUsed(Duty[] dutys)
		{
			string[] dutyCodes = new string[dutys.Length];

			for(int i=0; i<dutys.Length; i++)
			{
				dutyCodes[i] = dutys[i].DutyCode ;
			}
			
			int count = this.DataProvider.GetCount(new SQLCondition(string.Format("select count( dutycode ) from tbltserrorcause where dutycode in ({0})",
				FormatHelper.ProcessQueryValues(dutyCodes))));
			if(count>0)
			{
				return true;
			}
			return false;
		}

		public bool CheckErrorCodeGroup2ErrorCodeIsUsed(ErrorCodeGroup2ErrorCode[] ecg2ec)
		{
			string errorCode2Group = string.Empty;
			for(int i=0; i<ecg2ec.Length; i++)
			{
				errorCode2Group+="('"+ecg2ec[i].ErrorCodeGroup+"','"+ecg2ec[i].ErrorCode+"'),";
			}
			string sql = string.Format("select count( * ) from tbloqclotcard2errorcode where (errorcodegroup,ecode) in ({0})",errorCode2Group.TrimEnd(','));
			int count = this.DataProvider.GetCount(new SQLCondition(sql));
			if(count>0)
			{
				return true;
			}
			return false;
		}
		#endregion

        #region TSSmartConfig
        /// <summary>
        /// 
        /// </summary>
        public TSSmartConfig CreateNewTSSmartConfig()
        {
            return new TSSmartConfig();
        }

        public void AddTSSmartConfig(TSSmartConfig tSSmartConfig)
        {
            if (tSSmartConfig.Sequence == 0)
            {
                string strSql = "select max(seq) seq from tbltssmartcfg";
                object[] objsTmp = this.DataProvider.CustomQuery(typeof(TSSmartConfig), new SQLCondition(strSql));
                if (objsTmp != null && objsTmp.Length > 0)
                    tSSmartConfig.Sequence = ((TSSmartConfig)objsTmp[0]).Sequence + 1;
                else
                    tSSmartConfig.Sequence = 1;
            }
            this._helper.AddDomainObject(tSSmartConfig);
        }

        public void UpdateTSSmartConfig(TSSmartConfig tSSmartConfig)
        {
            this._helper.UpdateDomainObject(tSSmartConfig);
        }

        public void DeleteTSSmartConfig(TSSmartConfig tSSmartConfig)
        {
            this._helper.DeleteDomainObject(tSSmartConfig);
        }

        public void DeleteTSSmartConfig(TSSmartConfig[] tSSmartConfig)
        {
            this._helper.DeleteDomainObject(tSSmartConfig);
        }

        public object GetTSSmartConfig(decimal sequence)
        {
            return this.DataProvider.CustomSearch(typeof(TSSmartConfig), new object[] { sequence });
        }

        /// <summary>
        /// ** ��������:	��ѯTSSmartConfig��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2007-4-23 17:03:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <returns> TSSmartConfig���ܼ�¼��</returns>
        public int QueryTSSmartConfigCount(decimal sequence)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLTSSMARTCFG where 1=1 and SEQ like '{0}%' ", sequence)));
        }
        public int QueryTSSmartConfigCount(string errorCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLTSSMARTCFG where 1=1 and ecode like '{0}%' ", errorCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯTSSmartConfig
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2007-4-23 17:03:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> TSSmartConfig����</returns>
        public object[] QueryTSSmartConfig(decimal sequence, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(TSSmartConfig), new PagerCondition(string.Format("select {0} from TBLTSSMARTCFG where 1=1 and SEQ like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSSmartConfig)), sequence), "SEQ", inclusive, exclusive));
        }
        public object[] QueryTSSmartConfig(string errorCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(TSSmartConfig), new PagerCondition(string.Format("select {0} from TBLTSSMARTCFG where 1=1 and ecode like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSSmartConfig)), errorCode), "SEQ", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�TSSmartConfig
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2007-4-23 17:03:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>TSSmartConfig���ܼ�¼��</returns>
        public object[] GetAllTSSmartConfig()
        {
            return this.DataProvider.CustomQuery(typeof(TSSmartConfig), new SQLCondition(string.Format("select {0} from TBLTSSMARTCFG order by SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSSmartConfig)))));
        }

        #endregion

        #region ErrorCode2OPRework
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode2OPRework CreateNewErrorCode2OPRework()
        {
            return new ErrorCode2OPRework();
        }

        public void AddErrorCode2OPRework(ErrorCode2OPRework[] errorCode2OPRework)
        {
            this._helper.AddDomainObject(errorCode2OPRework);
        }

        public void UpdateErrorCode2OPRework(ErrorCode2OPRework errorCode2OPRework)
        {
            this._helper.UpdateDomainObject(errorCode2OPRework);
        }

        public void DeleteErrorCode2OPRework(ErrorCode2OPRework[] errorCode2OPRework)
        {
            this._helper.DeleteDomainObject(errorCode2OPRework);
        }

        public object GetErrorCode2OPRework(string  oPCode, string errorCode, int orgId)
        {
            return this.DataProvider.CustomSearch(typeof(ErrorCode2OPRework), new object[] { oPCode, errorCode, orgId });
        }

        /// <summary>
        /// ** ��������:	��ѯErrorCode2OPRework��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Roger xue
        /// ** �� ��:		2008-09-17
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCauseCode">ErrorCode2OPRework��ģ����ѯ</param>
        /// <returns> ErrorCode2OPRework���ܼ�¼��</returns>
        public int QueryErrorCode2OPReworkCount(string OPCode, string errorCodeDesc)
        {
            string sql = "SELECT COUNT(*) FROM tblec2oprework t1, tblec t2 WHERE t1.ecode = t2.ecode";
            if (OPCode != null && OPCode.Length != 0)
            {
                sql = string.Format("{0} AND t1.OPCode = '{1}'", sql, OPCode);
            }

            if (errorCodeDesc != null && errorCodeDesc.Length != 0)
            {
                sql = string.Format("{0} AND t2.ecdesc LIKE '%{1}%'", sql, errorCodeDesc);
            }
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯErrorCode2OPRework
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Roger xue
        /// ** �� ��:		2008-09-17
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCauseCode">ErrorCode2OPRework��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ErrorCode2OPRework����</returns>
        public object[] QueryErrorCode2OPRework(string OPCode, string errorCodeDesc, int inclusive, int exclusive)
        {
            string sql = "SELECT t1.OPCode AS OPCode, t1.ecode AS ErrorCode, t2.ecdesc AS ErrorCodeDesc, t1.routecode AS RouteCode, t1.toopcode AS ToOPCode FROM tblec2oprework t1, tblec t2 WHERE t1.ecode = t2.ecode";
            if (OPCode != null && OPCode.Length != 0)
            {
                sql = string.Format("{0} AND t1.OPCode = '{1}'", sql, OPCode);
            }

            if (errorCodeDesc != null && errorCodeDesc.Length != 0)
            {
                sql = string.Format("{0} AND t2.ecdesc LIKE '%{1}%'", sql, errorCodeDesc);
            }

            return this.DataProvider.CustomQuery(typeof(ErrorCode2OPReworkNew), new PagerCondition(sql, "OPCode", inclusive, exclusive));
        }

        #endregion

		/// <summary>
		/// added by jessie lee for DCT,2006/5/11
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="modelCode"></param>
		/// <returns></returns>
		public object[] QueryECG2ECByECAndModelCode( string[] errorCode, string modelCode )
		{
			string sql = string.Format
				(@"select {0}
				from tblecg2ec
				where ecgcode in (select ecgcode from tblmodel2ecg where modelcode = '{1}')
				and ecode in ( {2} ) ",DomainObjectUtility.GetDomainObjectFieldsString( typeof(ErrorCodeGroup2ErrorCode) ),
				modelCode,
				FormatHelper.ProcessQueryValues( errorCode ));

			return this.DataProvider.CustomQuery( typeof(ErrorCodeGroup2ErrorCode),
				new SQLCondition(sql) );
		}

        public object[] GetErrorCodeByErrorGroupList(string errorGroupCodeList)
        {
            errorGroupCodeList = errorGroupCodeList.Replace(",","','");

            string sql = "";
            sql += "SELECT DISTINCT a.ecode AS ECODE, a.ecdesc AS ECDESC, b.ecgcode AS ECGCODE";
            sql += "           FROM tblec a, tblecg2ec b";
            sql += "          WHERE a.ecode = b.ecode";
            sql += "            AND b.ecgcode IN ('" + errorGroupCodeList + "')";

            return this.DataProvider.CustomQuery(typeof(ErrorGrou2ErrorCode4OQC), new SQLCondition(sql));
        }
        #region Add by sandy on 20140528
        /// <summary>
        /// ���ݲ��������ȡ���������顢���������Ӧ��ϵ��Ϣ
        /// </summary>
        /// <param name="ecode"></param>
        /// <returns></returns>
        public object[] GetECG2ECByECode(string ecode)
        {
            object[] ecg2EC = this.DataProvider.CustomQuery(typeof(ErrorCodeGroup2ErrorCode),
                new SQLParamCondition(
                string.Format("select {0} from tblecg2ec where ECODE = $ECODE",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(ErrorCodeGroup2ErrorCode))),
                new SQLParameter[] { new SQLParameter("ECODE", typeof(string), ecode) }));

            if (ecg2EC == null || ecg2EC.Length == 0)
                return null;
            else
                return ecg2EC;
        }
        #endregion
    }

}

