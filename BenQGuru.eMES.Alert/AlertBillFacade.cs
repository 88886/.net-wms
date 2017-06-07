using System;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;  
using BenQGuru.eMES.Domain.Alert;
using BenQGuru.eMES.Domain;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.AlertModel
{
	/// <summary>
	/// AlertBill ��ժҪ˵����
	/// </summary>
	public class AlertBillFacade
	{
		private IDomainDataProvider _domainDataProvider = null;
		private FacadeHelper _helper = null;

		public AlertBillFacade()
		{
			this._helper = new FacadeHelper( DataProvider );
		}

		public AlertBillFacade(IDomainDataProvider domainDataProvider)
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

		#region AlertBill
		/// <summary>
		/// 
		/// </summary>
		public AlertBill CreateNewAlertBill()
		{
			return new AlertBill();
		}

		private int GetNextAlertBillID()
		{
			return this._domainDataProvider.GetCount(new SQLCondition("select SEQALERTBILL.NEXTVAL from dual"));
		}
		public void AddAlertBill( AlertBill alertBill)
		{
			//���BillId
			alertBill.BillId = this.GetNextAlertBillID();

			this._helper.AddDomainObject( alertBill );
		}

		public void UpdateAlertBill(AlertBill alertBill)
		{
			this._helper.UpdateDomainObject( alertBill );
		}

		public void DeleteAlertBill(AlertBill alertBill)
		{
			this._helper.DeleteDomainObject( alertBill, 
				new ICheck[]{ new DeleteAssociateCheck( alertBill,
								this.DataProvider, 
								new Type[]{
											  typeof(AlertNotifier)	})} );
		}

		public void DeleteAlertBill(AlertBill[] alertBill)
		{
			this._helper.DeleteDomainObject( alertBill, 
				new ICheck[]{ new DeleteAssociateCheck( alertBill,
								this.DataProvider, 
								new Type[]{
											  typeof(AlertNotifier)	})} );
		}

		public object GetAlertBill(int billId)
		{
			return this.DataProvider.CustomSearch(typeof(AlertBill), new object[]{billId});
		}

		/// <summary>
		/// ** ��������:	��ѯAlertBill��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="alertType">AlertType��ģ����ѯ</param>
		/// <param name="alertItem">AlertItem��ģ����ѯ</param>
		/// <returns> AlertBill���ܼ�¼��</returns>
		public int QueryAlertBillCount( string itemCode, string alertType, string alertItem)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLALERTBILL where 1=1 and ITEMCODE like '{0}%'  and ALERTTYPE like '{1}%'  and ALERTITEM like '{2}%' " , itemCode, alertType, alertItem)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯAlertBill
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="alertType">AlertType��ģ����ѯ</param>
		/// <param name="alertItem">AlertItem��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> AlertBill����</returns>
		public object[] QueryAlertBill( string itemCode, string alertType, string alertItem, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(AlertBill), new PagerCondition(string.Format("select {0} from TBLALERTBILL where 1=1 and ITEMCODE like '{1}%'  and ALERTTYPE like '{2}%'  and ALERTITEM like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertBill)) , itemCode, alertType, alertItem), "ITEMCODE,ALERTTYPE,ALERTITEM", inclusive, exclusive));
		}

		public object[] QueryAlertBillExact( string itemCode, string alertType, string alertItem, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(AlertBill), new PagerCondition(string.Format("select {0} from TBLALERTBILL where 1=1 and ITEMCODE = '{1}'  and ALERTTYPE = '{2}'  and ALERTITEM ='{3}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertBill)) , itemCode, alertType, alertItem), "ITEMCODE,ALERTTYPE,ALERTITEM", inclusive, exclusive));
		}
		/// <summary>
		/// ��AlertType ����Ч���ڽ��в�ѯ
		/// </summary>
		public object[] QueryAlertBill(string alertType,int validdate )
		{
			return this.DataProvider.CustomQuery(typeof(AlertBill), new SQLCondition(string.Format("select {0} from TBLALERTBILL where 1=1 and ALERTTYPE = '{1}'  and validdate<={2} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertBill)) ,alertType,validdate)));
		}
		/// <summary>
		/// ** ��������:	��ҳ��ѯAlertBill
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="alertType">AlertType��ģ����ѯ</param>
		/// <param name="alertItem">AlertItem��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> AlertBill����</returns>
		public object[] QueryAlertBill2( string itemCode, string alertType, string alertItem, int inclusive, int exclusive )
		{
			string sql = "select {0} from TBLALERTBILL where 1=1";
			if(alertType != "*" && alertType != string.Empty)
				sql = sql + " and ALERTTYPE='" + alertType + "'";
			if(alertItem != "*" && alertItem != string.Empty)
				sql = sql + " and ALERTITEM='" + alertItem + "'";

			string[] itemcodes = itemCode.Split(',');
			if(itemcodes.Length == 1)
			{
				if(itemCode != string.Empty)
					sql = sql + " and ITEMCODE like '" + itemCode +"%'";
			}
			else if(itemcodes.Length > 1)
			{
				string itemcodelist = BenQGuru.eMES.Web.Helper.FormatHelper.ProcessQueryValues(itemcodes);
				sql = sql + " and ITEMCODE in(" + itemcodelist + ")";
			}
			
			return this.DataProvider.CustomQuery(typeof(AlertBill), new PagerCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertBill)) ), "mdate desc,mtime desc", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�AlertBill
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>AlertBill���ܼ�¼��</returns>
		public object[] GetAllAlertBill()
		{
			return this.DataProvider.CustomQuery(typeof(AlertBill), new SQLCondition(string.Format("select {0} from TBLALERTBILL order by ITEMCODE,ALERTTYPE,ALERTITEM", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertBill)))));
		}


		#endregion

		#region AlertResBill ��Դ����Ԥ��
		/// <summary>
		/// 
		/// </summary>
		public AlertResBill CreateNewAlertResBill()
		{
			return new AlertResBill();
		}

		public void AddAlertResBill( AlertResBill alertBill)
		{
			alertBill.BillId = this.GetNextAlertBillID(); //Ԥ���趨Id

			this._helper.AddDomainObject( alertBill );
		}

		public void UpdateAlertResBill(AlertResBill alertBill)
		{
			this._helper.UpdateDomainObject( alertBill );
		}

		public void DeleteAlertResBill(AlertResBill alertBill)
		{
			this._helper.DeleteDomainObject( alertBill);
		}

		public void DeleteAlertResBill(AlertResBill[] alertBill)
		{
			this._helper.DeleteDomainObject( alertBill, 
				new ICheck[]{ new DeleteAssociateCheck( alertBill,
								this.DataProvider, 
								new Type[]{
											  typeof(AlertNotifier)	})} );
		}

		public object GetAlertResBill(int billId)
		{
			return this.DataProvider.CustomSearch(typeof(AlertResBill), new object[]{billId});
		}

		/// <summary>
		/// ** ��������:	��ѯAlertBill��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="alertType">AlertType��ģ����ѯ</param>
		/// <param name="alertItem">AlertItem��ģ����ѯ</param>
		/// <returns> AlertBill���ܼ�¼��</returns>
		public int QueryAlertResBillCount( string itemCode, string alertType, string alertItem ,string resCode,string ecgcode2ecode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLALERTRESBILL where 1=1 and ITEMCODE like '{0}%'  and ALERTTYPE like '{1}%'  and ALERTITEM like '{2}%' " , itemCode, alertType, alertItem)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯAlertBill
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="alertType">AlertType��ģ����ѯ</param>
		/// <param name="alertItem">AlertItem��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> AlertBill����</returns>
		public object[] QueryAlertResBill( string itemCode, string alertType, string alertItem ,string resCode,string ecgcode2ecode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(AlertResBill), new PagerCondition(string.Format("select {0} from TBLALERTRESBILL where 1=1 and ITEMCODE like '{1}%'  and ALERTTYPE like '{2}%'  and ALERTITEM like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertResBill)) , itemCode, alertType, alertItem), "ITEMCODE,ALERTTYPE,ALERTITEM", inclusive, exclusive));
		}

		public object[] QueryAlertResBill( string alertType, string alertItem )
		{
			return this.DataProvider.CustomQuery(typeof(AlertResBill), new SQLCondition(string.Format("select {0} from TBLALERTRESBILL where 1=1  and ALERTTYPE = '{1}'  and ALERTITEM = '{2}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertResBill)) ,  alertType, alertItem)));
		}

		/// <summary>
		/// ��AlertType ����Ч���ڽ��в�ѯ
		/// </summary>
		public object[] QueryAlertResBill(string alertType,int validdate )
		{
			return this.DataProvider.CustomQuery(typeof(AlertBill), new SQLCondition(string.Format("select {0} from TBLALERTBILL where 1=1 and ALERTTYPE = '{1}'  and validdate<={2} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertResBill)) ,alertType,validdate)));
		}
		/// <summary>
		/// ** ��������:	��ҳ��ѯAlertBill
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="alertType">AlertType��ģ����ѯ</param>
		/// <param name="alertItem">AlertItem��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> AlertBill����</returns>
		public object[] QueryAlertResBill2( string itemCode, string alertType, string alertItem ,string resCode,string ecgcode2ecode, int inclusive, int exclusive )
		{
			string sql = "select {0} from TBLALERTRESBILL where 1=1";
			if(alertType != "*" && alertType != string.Empty)
				sql = sql + " and ALERTTYPE='" + alertType + "'";
			if(alertItem != "*" && alertItem != string.Empty)
				sql = sql + " and ALERTITEM='" + alertItem + "'";

			string[] itemcodes = itemCode.Split(',');
			if(itemcodes.Length == 1)
			{
				if(itemCode != string.Empty)
					sql = sql + " and ITEMCODE like '" + itemCode +"%'";
			}
			else if(itemcodes.Length > 1)
			{
				string itemcodelist = BenQGuru.eMES.Web.Helper.FormatHelper.ProcessQueryValues(itemcodes);
				sql = sql + " and ITEMCODE in(" + itemcodelist + ")";
			}
			
			return this.DataProvider.CustomQuery(typeof(AlertResBill), new PagerCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertResBill)) ), "mdate desc,mtime desc", inclusive, exclusive));
		}

		#endregion

		#region AlertNotifier
		/// <summary>
		/// 
		/// </summary>
		public AlertNotifier CreateNewAlertNotifier()
		{
			return new AlertNotifier();
		}

		public void AddAlertNotifier( AlertNotifier alertNotifier)
		{
			this._helper.AddDomainObject( alertNotifier );
		}

		public void UpdateAlertNotifier(AlertNotifier alertNotifier)
		{
			this._helper.UpdateDomainObject( alertNotifier );
		}

		public void DeleteAlertNotifier(AlertNotifier alertNotifier)
		{
			this._helper.DeleteDomainObject( alertNotifier );
		}

		public void DeleteAlertNotifier(AlertNotifier[] alertNotifier)
		{
			this._helper.DeleteDomainObject( alertNotifier );
		}

		public object GetAlertNotifier( string userCode,int billId)
		{
			return this.DataProvider.CustomSearch(typeof(AlertNotifier), new object[]{ userCode,billId});
		}

		/// <summary>
		/// ** ��������:	��ѯAlertNotifier��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="userCode">UserCode��ģ����ѯ</param>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="alertType">AlertType��ģ����ѯ</param>
		/// <param name="alertItem">AlertItem��ģ����ѯ</param>
		/// <returns> AlertNotifier���ܼ�¼��</returns>
		public int QueryAlertNotifierCount( string userCode, string itemCode, string alertType, string alertItem)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLALERTNOTIFIER where 1=1 and USERCODE like '{0}%'  and ITEMCODE like '{1}%'  and ALERTTYPE like '{2}%'  and ALERTITEM like '{3}%' " , userCode, itemCode, alertType, alertItem)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯAlertNotifier
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="userCode">UserCode��ģ����ѯ</param>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="alertType">AlertType��ģ����ѯ</param>
		/// <param name="alertItem">AlertItem��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> AlertNotifier����</returns>
		public object[] QueryAlertNotifier( string userCode, string itemCode, string alertType, string alertItem, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(AlertNotifier), new PagerCondition(string.Format("select {0} from TBLALERTNOTIFIER where 1=1 and USERCODE like '{1}%'  and ITEMCODE like '{2}%'  and ALERTTYPE like '{3}%'  and ALERTITEM like '{4}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertNotifier)) , userCode, itemCode, alertType, alertItem), "USERCODE,ITEMCODE,ALERTTYPE,ALERTITEM", inclusive, exclusive));
		}

		public object[] QueryAlertNotifier(int billId)
		{
			return this.DataProvider.CustomQuery(typeof(AlertNotifier), new SQLCondition(
																						string.Format("select {0} from TBLALERTNOTIFIER where 1=1 and billId={1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertNotifier)) ,billId)
																						)
												);
		}
		/// <summary>
		/// ** ��������:	������е�AlertNotifier
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-9-5 14:02:07
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>AlertNotifier���ܼ�¼��</returns>
		public object[] GetAllAlertNotifier()
		{
			return this.DataProvider.CustomQuery(typeof(AlertNotifier), new SQLCondition(string.Format("select {0} from TBLALERTNOTIFIER order by USERCODE,ITEMCODE,ALERTTYPE,ALERTITEM", DomainObjectUtility.GetDomainObjectFieldsString(typeof(AlertNotifier)))));
		}


		#endregion
	}

	public class AlertType_Old
	{
		public const string NG = "NG";
		public const string PPM = "PPM";
		public const string DirectPass = "DirectPass";
		public const string CPK = "CPK";
		public const string First = "First";
		public const string ResourceNG = "ResourceNG";				//��Դ������
		public const string Manual = "Manual";
	}

	public class AlertItem_Old
	{
		public const string Item = "Item";
		public const string Model = "Model";
		public const string SS = "SS";
		public const string Segment = "Segment";
		public const string Resource = "Resource";
	}

    public class Operator_Old
	{
		public const string BW = "BW";
		public const string LE = "LE";
		public const string GE = "GE";
	}

    public class AlertLevel_Old
	{
		public const string Important = "Important";
		public const string Primary   = "Primary";
		public const string Severity  = "Severity";
	}

    public class AlertStatus_Old
	{
		public const string Unhandled = "Unhandled";
		public const string Closed = "Closed";
		public const string Observing = "Observing";
		public const string Handling = "Handling";
	}
}
