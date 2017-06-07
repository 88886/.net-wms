using System;
using System.Runtime.Remoting;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.DeviceInterface;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.DeviceInterface
{
	/// <summary>
	/// DeviceInterfaceFacade ��ժҪ˵����
	/// �ļ���:		DeviceInterfaceFacade.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
	/// ��������:	2006-5-30 09:13:52 ����
	/// �޸���:
	/// �޸�����:
	/// �� ��:	
	/// �� ��:	
	/// </summary>
	public class DeviceInterfaceFacade
	{
		private IDomainDataProvider _domainDataProvider = null;
		private FacadeHelper _helper = null;

 		public DeviceInterfaceFacade()
		{
			this._helper = new FacadeHelper( DataProvider );
		}

		public DeviceInterfaceFacade(IDomainDataProvider domainDataProvider)
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

		#region PreTestValue
		/// <summary>
		/// 
		/// </summary>
		public PreTestValue CreateNewPreTestValue()
		{
			return new PreTestValue();
		}

		public void AddPreTestValue( PreTestValue preTestValue)
		{
			this._helper.AddDomainObject( preTestValue );
		}

		public void UpdatePreTestValue(PreTestValue preTestValue)
		{
			this._helper.UpdateDomainObject( preTestValue );
		}

		public void DeletePreTestValue(PreTestValue preTestValue)
		{
			this._helper.DeleteDomainObject( preTestValue );
		}

		public void DeletePreTestValue(PreTestValue[] preTestValue)
		{
			this._helper.DeleteDomainObject( preTestValue );
		}

		public object GetPreTestValue( string iD )
		{
			return this.DataProvider.CustomSearch(typeof(PreTestValue), new object[]{ iD });
		}

		/// <summary>
		/// ** ��������:	��ѯPreTestValue��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-30 09:13:52 ����
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="iD">ID��ģ����ѯ</param>
		/// <returns> PreTestValue���ܼ�¼��</returns>
		public int QueryPreTestValueCount( string iD)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from tblPreTestValue where 1=1 and ID like '{0}%' " , iD)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯPreTestValue
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-30 09:13:52 ����
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="iD">ID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> PreTestValue����</returns>
		public object[] QueryPreTestValue( string iD, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(PreTestValue), new PagerCondition(string.Format("select {0} from tblPreTestValue where 1=1 and ID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(PreTestValue)) , iD), "ID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�PreTestValue
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-30 09:13:52 ����
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>PreTestValue���ܼ�¼��</returns>
		public object[] GetAllPreTestValue()
		{
			return this.DataProvider.CustomQuery(typeof(PreTestValue), new SQLCondition(string.Format("select {0} from tblPreTestValue order by ID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(PreTestValue)))));
		}


		#endregion

	}
}

