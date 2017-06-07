using System;
using System.Collections;
using System.Runtime.Remoting;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.ATE;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.Domain.TS;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.TSModel;

namespace BenQGuru.eMES.DataCollect
{
	/// <summary>
	/// ATEFacade ��ժҪ˵����
	/// �ļ���:		ATEFacade.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
	/// ��������:	2006-5-22 10:29:35
	/// �޸���:
	/// �޸�����:
	/// �� ��:	
	/// �� ��:	
	/// </summary>
	public class ATEFacade
	{
		private IDomainDataProvider _domainDataProvider = null;
		private FacadeHelper _helper = null;

		public ATEFacade(IDomainDataProvider domainDataProvider)
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

		#region ATETestInfo
		/// <summary>
		/// 
		/// </summary>
		public ATETestInfo CreateNewATETestInfo()
		{
			return new ATETestInfo();
		}

		public void AddATETestInfo( ATETestInfo aTETestInfo)
		{
			this._helper.AddDomainObject( aTETestInfo );
		}

		public void UpdateATETestInfo(ATETestInfo aTETestInfo)
		{
			this._helper.UpdateDomainObject( aTETestInfo );
		}

		public void DeleteATETestInfo(ATETestInfo aTETestInfo)
		{
			this._helper.DeleteDomainObject( aTETestInfo );
		}

		public void DeleteATETestInfo(ATETestInfo[] aTETestInfo)
		{
			this._helper.DeleteDomainObject( aTETestInfo );
		}

		public object GetATETestInfo( string pKID )
		{
			return this.DataProvider.CustomSearch(typeof(ATETestInfo), new object[]{ pKID });
		}

		/// <summary>
		/// ** ��������:	��ѯATETestInfo��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-22 10:29:36
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="pKID">PKID��ģ����ѯ</param>
		/// <returns> ATETestInfo���ܼ�¼��</returns>
		public int QueryATETestInfoCount( string pKID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLATETESTINFO where 1=1 and PKID like '{0}%' " , pKID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯATETestInfo
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-22 10:29:36
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="pKID">PKID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ATETestInfo����</returns>
		public object[] QueryATETestInfo( string pKID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ATETestInfo), new PagerCondition(string.Format("select {0} from TBLATETESTINFO where 1=1 and PKID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ATETestInfo)) , pKID), "PKID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ATETestInfo
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-22 10:29:36
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ATETestInfo���ܼ�¼��</returns>
		public object[] GetAllATETestInfo()
		{
			return this.DataProvider.CustomQuery(typeof(ATETestInfo), new SQLCondition(string.Format("select {0} from TBLATETESTINFO order by PKID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ATETestInfo)))));
		}


		public object[] GetATETestInfoByRCard(string rcard, string rescode)
		{
			return this.DataProvider.CustomQuery(typeof(ATETestInfo), 
				new SQLParamCondition(string.Format("select {0} from TBLATETESTINFO where rcard=$RCARD and rescode=$RESCODE", 
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(ATETestInfo))),
				new SQLParameter[]{ 
									  new SQLParameter("RCARD", typeof(string), rcard),
									  new SQLParameter("RESCODE", typeof(string), rescode)
								  }));
		}

		public object[] GetErrorInfo( ATETestInfo ateTestInfo, string modelcode )
		{
			/* Fail Code,to be confirmed,
			 * ��ʽΪFailCode��KeyName֮��ķָ�����^��KeyName֮��ķָ�����#��ÿ��FailCode֮��ķָ�����*��
			 * ���磺FailCode^KeyName#KeyName*FailCode^KeyName#KeyName */
			if( ateTestInfo.FailCode==null || ateTestInfo.FailCode.Trim().Length == 0 )
			{
				return null;
			}
  
			string[] failGroup = ateTestInfo.FailCode.Split('*');

			int count=0;
			for (int i=0 ; i<failGroup.Length; i++)
			{
				count+= failGroup[i].Split('^')[1].Split('#').Length;
			}

			object[] objs = new object[count];
			int k=0;
			for (int i=0 ; i<failGroup.Length; i++)
			{
				string errorCode = failGroup[i].Split('^')[0];
				string[] errorLoc = failGroup[i].Split('^')[1].Split('#');

				for( int j=0; j<errorLoc.Length; j++)
				{
					TSErrorCode2Location tsinfo = new TSErrorCode2Location();
					tsinfo.ErrorCode = errorCode;

					TSModelFacade tsmodelFacade = new TSModelFacade(this.DataProvider);
					object[] objecgs = tsmodelFacade.QueryECG2ECByECAndModelCode(new string[]{ errorCode  },modelcode);
				
					tsinfo.ErrorCodeGroup = (objecgs[0] as ErrorCodeGroup2ErrorCode).ErrorCodeGroup ; 

					tsinfo.ErrorLocation = errorLoc[j];
					tsinfo.AB = ItemLocationSide.ItemLocationSide_AB;
					objs[k] = tsinfo;
					k++;
				}
			}
			return objs;
		}
		#endregion

	}
}

