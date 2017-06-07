using System;
using System.Runtime.Remoting;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.ReportView;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.ReportView
{
	/// <summary>
	/// ReportViewFacade ��ժҪ˵����
	/// �ļ���:		ReportViewFacade.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
	/// ��������:	2007-7-17 9:17:33
	/// �޸���:
	/// �޸�����:
	/// �� ��:	
	/// �� ��:	
	/// </summary>
	public partial class ReportViewFacade
	{
		private IDomainDataProvider _domainDataProvider = null;
		private FacadeHelper _helper = null;

 		public ReportViewFacade()
		{
			this._helper = new FacadeHelper( DataProvider );
		}

		public ReportViewFacade(IDomainDataProvider domainDataProvider)
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

		#region RptViewChartCategory
		/// <summary>
		/// ���ݷ�����λ
		/// </summary>
		public RptViewChartCategory CreateNewRptViewChartCategory()
		{
			return new RptViewChartCategory();
		}

		public void AddRptViewChartCategory( RptViewChartCategory rptViewChartCategory)
		{
			this._helper.AddDomainObject( rptViewChartCategory );
		}

		public void UpdateRptViewChartCategory(RptViewChartCategory rptViewChartCategory)
		{
			this._helper.UpdateDomainObject( rptViewChartCategory );
		}

		public void DeleteRptViewChartCategory(RptViewChartCategory rptViewChartCategory)
		{
			this._helper.DeleteDomainObject( rptViewChartCategory );
		}

		public void DeleteRptViewChartCategory(RptViewChartCategory[] rptViewChartCategory)
		{
			this._helper.DeleteDomainObject( rptViewChartCategory );
		}

		public object GetRptViewChartCategory( string reportID, decimal chartSequence, decimal categorySequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewChartCategory), new object[]{ reportID, chartSequence, categorySequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewChartCategory��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="chartSequence">ChartSequence��ģ����ѯ</param>
		/// <param name="categorySequence">CategorySequence��ģ����ѯ</param>
		/// <returns> RptViewChartCategory���ܼ�¼��</returns>
		public int QueryRptViewChartCategoryCount( string reportID, decimal chartSequence, decimal categorySequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVCHARTCATE where 1=1 and RPTID like '{0}%'  and CHARTSEQ like '{1}%'  and CATESEQ like '{2}%' " , reportID, chartSequence, categorySequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewChartCategory
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="chartSequence">ChartSequence��ģ����ѯ</param>
		/// <param name="categorySequence">CategorySequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewChartCategory����</returns>
		public object[] QueryRptViewChartCategory( string reportID, decimal chartSequence, decimal categorySequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewChartCategory), new PagerCondition(string.Format("select {0} from TBLRPTVCHARTCATE where 1=1 and RPTID like '{1}%'  and CHARTSEQ like '{2}%'  and CATESEQ like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewChartCategory)) , reportID, chartSequence, categorySequence), "RPTID,CHARTSEQ,CATESEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewChartCategory
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewChartCategory���ܼ�¼��</returns>
		public object[] GetAllRptViewChartCategory()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewChartCategory), new SQLCondition(string.Format("select {0} from TBLRPTVCHARTCATE order by RPTID,CHARTSEQ,CATESEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewChartCategory)))));
		}


		#endregion

		#region RptViewChartData
		/// <summary>
		/// ����ͳ����λ
		/// </summary>
		public RptViewChartData CreateNewRptViewChartData()
		{
			return new RptViewChartData();
		}

		public void AddRptViewChartData( RptViewChartData rptViewChartData)
		{
			this._helper.AddDomainObject( rptViewChartData );
		}

		public void UpdateRptViewChartData(RptViewChartData rptViewChartData)
		{
			this._helper.UpdateDomainObject( rptViewChartData );
		}

		public void DeleteRptViewChartData(RptViewChartData rptViewChartData)
		{
			this._helper.DeleteDomainObject( rptViewChartData );
		}

		public void DeleteRptViewChartData(RptViewChartData[] rptViewChartData)
		{
			this._helper.DeleteDomainObject( rptViewChartData );
		}

		public object GetRptViewChartData( string reportID, decimal dataSequence, decimal chartSequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewChartData), new object[]{ reportID, dataSequence, chartSequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewChartData��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="dataSequence">DataSequence��ģ����ѯ</param>
		/// <param name="chartSequence">ChartSequence��ģ����ѯ</param>
		/// <returns> RptViewChartData���ܼ�¼��</returns>
		public int QueryRptViewChartDataCount( string reportID, decimal dataSequence, decimal chartSequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVCHARTDATA where 1=1 and RPTID like '{0}%'  and DATASEQ like '{1}%'  and CHARTSEQ like '{2}%' " , reportID, dataSequence, chartSequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewChartData
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="dataSequence">DataSequence��ģ����ѯ</param>
		/// <param name="chartSequence">ChartSequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewChartData����</returns>
		public object[] QueryRptViewChartData( string reportID, decimal dataSequence, decimal chartSequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewChartData), new PagerCondition(string.Format("select {0} from TBLRPTVCHARTDATA where 1=1 and RPTID like '{1}%'  and DATASEQ like '{2}%'  and CHARTSEQ like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewChartData)) , reportID, dataSequence, chartSequence), "RPTID,DATASEQ,CHARTSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewChartData
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewChartData���ܼ�¼��</returns>
		public object[] GetAllRptViewChartData()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewChartData), new SQLCondition(string.Format("select {0} from TBLRPTVCHARTDATA order by RPTID,DATASEQ,CHARTSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewChartData)))));
		}


		#endregion

		#region RptViewChartMain
		/// <summary>
		/// ������ʾ��λ
		/// </summary>
		public RptViewChartMain CreateNewRptViewChartMain()
		{
			return new RptViewChartMain();
		}

		public void AddRptViewChartMain( RptViewChartMain rptViewChartMain)
		{
			this._helper.AddDomainObject( rptViewChartMain );
		}

		public void UpdateRptViewChartMain(RptViewChartMain rptViewChartMain)
		{
			this._helper.UpdateDomainObject( rptViewChartMain );
		}

		public void DeleteRptViewChartMain(RptViewChartMain rptViewChartMain)
		{
			this._helper.DeleteDomainObject( rptViewChartMain );
		}

		public void DeleteRptViewChartMain(RptViewChartMain[] rptViewChartMain)
		{
			this._helper.DeleteDomainObject( rptViewChartMain );
		}

		public object GetRptViewChartMain( string reportID, decimal chartSequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewChartMain), new object[]{ reportID, chartSequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewChartMain��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="chartSequence">ChartSequence��ģ����ѯ</param>
		/// <returns> RptViewChartMain���ܼ�¼��</returns>
		public int QueryRptViewChartMainCount( string reportID, decimal chartSequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVCHARTMAIN where 1=1 and RPTID like '{0}%'  and CHARTSEQ like '{1}%' " , reportID, chartSequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewChartMain
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="chartSequence">ChartSequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewChartMain����</returns>
		public object[] QueryRptViewChartMain( string reportID, decimal chartSequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewChartMain), new PagerCondition(string.Format("select {0} from TBLRPTVCHARTMAIN where 1=1 and RPTID like '{1}%'  and CHARTSEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewChartMain)) , reportID, chartSequence), "RPTID,CHARTSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewChartMain
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewChartMain���ܼ�¼��</returns>
		public object[] GetAllRptViewChartMain()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewChartMain), new SQLCondition(string.Format("select {0} from TBLRPTVCHARTMAIN order by RPTID,CHARTSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewChartMain)))));
		}


		#endregion

		#region RptViewChartSeries
		/// <summary>
		/// ϵ�з�����λ
		/// </summary>
		public RptViewChartSeries CreateNewRptViewChartSeries()
		{
			return new RptViewChartSeries();
		}

		public void AddRptViewChartSeries( RptViewChartSeries rptViewChartSeries)
		{
			this._helper.AddDomainObject( rptViewChartSeries );
		}

		public void UpdateRptViewChartSeries(RptViewChartSeries rptViewChartSeries)
		{
			this._helper.UpdateDomainObject( rptViewChartSeries );
		}

		public void DeleteRptViewChartSeries(RptViewChartSeries rptViewChartSeries)
		{
			this._helper.DeleteDomainObject( rptViewChartSeries );
		}

		public void DeleteRptViewChartSeries(RptViewChartSeries[] rptViewChartSeries)
		{
			this._helper.DeleteDomainObject( rptViewChartSeries );
		}

		public object GetRptViewChartSeries( string reportID, decimal chartSequence, decimal seriesSequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewChartSeries), new object[]{ reportID, chartSequence, seriesSequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewChartSeries��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="chartSequence">ChartSequence��ģ����ѯ</param>
		/// <param name="seriesSequence">SeriesSequence��ģ����ѯ</param>
		/// <returns> RptViewChartSeries���ܼ�¼��</returns>
		public int QueryRptViewChartSeriesCount( string reportID, decimal chartSequence, decimal seriesSequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVCHARTSER where 1=1 and RPTID like '{0}%'  and CHARTSEQ like '{1}%'  and SERSEQ like '{2}%' " , reportID, chartSequence, seriesSequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewChartSeries
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="chartSequence">ChartSequence��ģ����ѯ</param>
		/// <param name="seriesSequence">SeriesSequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewChartSeries����</returns>
		public object[] QueryRptViewChartSeries( string reportID, decimal chartSequence, decimal seriesSequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewChartSeries), new PagerCondition(string.Format("select {0} from TBLRPTVCHARTSER where 1=1 and RPTID like '{1}%'  and CHARTSEQ like '{2}%'  and SERSEQ like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewChartSeries)) , reportID, chartSequence, seriesSequence), "RPTID,CHARTSEQ,SERSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewChartSeries
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewChartSeries���ܼ�¼��</returns>
		public object[] GetAllRptViewChartSeries()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewChartSeries), new SQLCondition(string.Format("select {0} from TBLRPTVCHARTSER order by RPTID,CHARTSEQ,SERSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewChartSeries)))));
		}


		#endregion

		#region RptViewDataConnect
		/// <summary>
		/// ���ݿ�����
		/// </summary>
		public RptViewDataConnect CreateNewRptViewDataConnect()
		{
			return new RptViewDataConnect();
		}

		public void AddRptViewDataConnect( RptViewDataConnect rptViewDataConnect)
		{
			this._helper.AddDomainObject( rptViewDataConnect );
		}

		public void UpdateRptViewDataConnect(RptViewDataConnect rptViewDataConnect)
		{
			this._helper.UpdateDomainObject( rptViewDataConnect );
		}

		public void DeleteRptViewDataConnect(RptViewDataConnect rptViewDataConnect)
		{
			this._helper.DeleteDomainObject( rptViewDataConnect );
		}

		public void DeleteRptViewDataConnect(RptViewDataConnect[] rptViewDataConnect)
		{
			this._helper.DeleteDomainObject( rptViewDataConnect );
		}

		public object GetRptViewDataConnect( decimal dataConnectID )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewDataConnect), new object[]{ dataConnectID });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewDataConnect��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dataConnectID">DataConnectID��ģ����ѯ</param>
		/// <returns> RptViewDataConnect���ܼ�¼��</returns>
		public int QueryRptViewDataConnectCount( decimal dataConnectID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVCONNECT where 1=1 and DATACONNECTID like '{0}%' " , dataConnectID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewDataConnect
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dataConnectID">DataConnectID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewDataConnect����</returns>
		public object[] QueryRptViewDataConnect( decimal dataConnectID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataConnect), new PagerCondition(string.Format("select {0} from TBLRPTVCONNECT where 1=1 and DATACONNECTID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataConnect)) , dataConnectID), "DATACONNECTID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewDataConnect
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewDataConnect���ܼ�¼��</returns>
		public object[] GetAllRptViewDataConnect()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataConnect), new SQLCondition(string.Format("select {0} from TBLRPTVCONNECT order by DATACONNECTID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataConnect)))));
		}


		#endregion

		#region RptViewDataFormat
		/// <summary>
		/// �ı���ʾ��ʽ
		/// </summary>
		public RptViewDataFormat CreateNewRptViewDataFormat()
		{
			return new RptViewDataFormat();
		}

		public void AddRptViewDataFormat( RptViewDataFormat rptViewDataFormat)
		{
			this._helper.AddDomainObject( rptViewDataFormat );
		}

		public void UpdateRptViewDataFormat(RptViewDataFormat rptViewDataFormat)
		{
			this._helper.UpdateDomainObject( rptViewDataFormat );
		}

		public void DeleteRptViewDataFormat(RptViewDataFormat rptViewDataFormat)
		{
			this._helper.DeleteDomainObject( rptViewDataFormat );
		}

		public void DeleteRptViewDataFormat(RptViewDataFormat[] rptViewDataFormat)
		{
			this._helper.DeleteDomainObject( rptViewDataFormat );
		}

		public object GetRptViewDataFormat( string formatID )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewDataFormat), new object[]{ formatID });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewDataFormat��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="formatID">FormatID��ģ����ѯ</param>
		/// <returns> RptViewDataFormat���ܼ�¼��</returns>
		public int QueryRptViewDataFormatCount( string formatID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVDATAFMT where 1=1 and FORMATID like '{0}%' " , formatID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewDataFormat
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="formatID">FormatID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewDataFormat����</returns>
		public object[] QueryRptViewDataFormat( string formatID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataFormat), new PagerCondition(string.Format("select {0} from TBLRPTVDATAFMT where 1=1 and FORMATID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataFormat)) , formatID), "FORMATID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewDataFormat
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewDataFormat���ܼ�¼��</returns>
		public object[] GetAllRptViewDataFormat()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataFormat), new SQLCondition(string.Format("select {0} from TBLRPTVDATAFMT order by FORMATID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataFormat)))));
		}


		#endregion

		#region RptViewDataSource
		/// <summary>
		/// ���ݲ�ѯ
		/// </summary>
		public RptViewDataSource CreateNewRptViewDataSource()
		{
			return new RptViewDataSource();
		}

		public void AddRptViewDataSource( RptViewDataSource rptViewDataSource)
		{
			this._helper.AddDomainObject( rptViewDataSource );
		}

		public void UpdateRptViewDataSource(RptViewDataSource rptViewDataSource)
		{
			this._helper.UpdateDomainObject( rptViewDataSource );
		}

		public void DeleteRptViewDataSource(RptViewDataSource rptViewDataSource)
		{
			this._helper.DeleteDomainObject( rptViewDataSource );
		}

		public void DeleteRptViewDataSource(RptViewDataSource[] rptViewDataSource)
		{
			this._helper.DeleteDomainObject( rptViewDataSource );
		}

		public object GetRptViewDataSource( decimal dataSourceID )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewDataSource), new object[]{ dataSourceID });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewDataSource��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dataSourceID">DataSourceID��ģ����ѯ</param>
		/// <returns> RptViewDataSource���ܼ�¼��</returns>
		public int QueryRptViewDataSourceCount( decimal dataSourceID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVDATASRC where 1=1 and DATASOURCEID like '{0}%' " , dataSourceID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewDataSource
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dataSourceID">DataSourceID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewDataSource����</returns>
		public object[] QueryRptViewDataSource( decimal dataSourceID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataSource), new PagerCondition(string.Format("select {0} from TBLRPTVDATASRC where 1=1 and DATASOURCEID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataSource)) , dataSourceID), "DATASOURCEID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewDataSource
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewDataSource���ܼ�¼��</returns>
		public object[] GetAllRptViewDataSource()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataSource), new SQLCondition(string.Format("select {0} from TBLRPTVDATASRC order by DATASOURCEID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataSource)))));
		}


		#endregion

		#region RptViewDataSourceColumn
		/// <summary>
		/// ���ݲ�ѯ��λ����
		/// </summary>
		public RptViewDataSourceColumn CreateNewRptViewDataSourceColumn()
		{
			return new RptViewDataSourceColumn();
		}

		public void AddRptViewDataSourceColumn( RptViewDataSourceColumn rptViewDataSourceColumn)
		{
			this._helper.AddDomainObject( rptViewDataSourceColumn );
		}

		public void UpdateRptViewDataSourceColumn(RptViewDataSourceColumn rptViewDataSourceColumn)
		{
			this._helper.UpdateDomainObject( rptViewDataSourceColumn );
		}

		public void DeleteRptViewDataSourceColumn(RptViewDataSourceColumn rptViewDataSourceColumn)
		{
			this._helper.DeleteDomainObject( rptViewDataSourceColumn );
		}

		public void DeleteRptViewDataSourceColumn(RptViewDataSourceColumn[] rptViewDataSourceColumn)
		{
			this._helper.DeleteDomainObject( rptViewDataSourceColumn );
		}

		public object GetRptViewDataSourceColumn( decimal dataSourceID, decimal columnSequence, string columnName )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewDataSourceColumn), new object[]{ dataSourceID, columnSequence, columnName });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewDataSourceColumn��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dataSourceID">DataSourceID��ģ����ѯ</param>
		/// <param name="columnSequence">ColumnSequence��ģ����ѯ</param>
		/// <param name="columnName">ColumnName��ģ����ѯ</param>
		/// <returns> RptViewDataSourceColumn���ܼ�¼��</returns>
		public int QueryRptViewDataSourceColumnCount( decimal dataSourceID, decimal columnSequence, string columnName)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVDATASRCCOLUMN where 1=1 and DATASOURCEID like '{0}%'  and COLUMNSEQ like '{1}%'  and COLUMNNAME like '{2}%' " , dataSourceID, columnSequence, columnName)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewDataSourceColumn
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dataSourceID">DataSourceID��ģ����ѯ</param>
		/// <param name="columnSequence">ColumnSequence��ģ����ѯ</param>
		/// <param name="columnName">ColumnName��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewDataSourceColumn����</returns>
		public object[] QueryRptViewDataSourceColumn( decimal dataSourceID, decimal columnSequence, string columnName, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataSourceColumn), new PagerCondition(string.Format("select {0} from TBLRPTVDATASRCCOLUMN where 1=1 and DATASOURCEID like '{1}%'  and COLUMNSEQ like '{2}%'  and COLUMNNAME like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataSourceColumn)) , dataSourceID, columnSequence, columnName), "DATASOURCEID,COLUMNSEQ,COLUMNNAME", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewDataSourceColumn
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewDataSourceColumn���ܼ�¼��</returns>
		public object[] GetAllRptViewDataSourceColumn()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataSourceColumn), new SQLCondition(string.Format("select {0} from TBLRPTVDATASRCCOLUMN order by DATASOURCEID,COLUMNSEQ,COLUMNNAME", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataSourceColumn)))));
		}


		#endregion

		#region RptViewDataSourceParam
		/// <summary>
		/// ���ݲ�ѯ����(����DLL��Ч)
		/// </summary>
		public RptViewDataSourceParam CreateNewRptViewDataSourceParam()
		{
			return new RptViewDataSourceParam();
		}

		public void AddRptViewDataSourceParam( RptViewDataSourceParam rptViewDataSourceParam)
		{
			this._helper.AddDomainObject( rptViewDataSourceParam );
		}

		public void UpdateRptViewDataSourceParam(RptViewDataSourceParam rptViewDataSourceParam)
		{
			this._helper.UpdateDomainObject( rptViewDataSourceParam );
		}

		public void DeleteRptViewDataSourceParam(RptViewDataSourceParam rptViewDataSourceParam)
		{
			this._helper.DeleteDomainObject( rptViewDataSourceParam );
		}

		public void DeleteRptViewDataSourceParam(RptViewDataSourceParam[] rptViewDataSourceParam)
		{
			this._helper.DeleteDomainObject( rptViewDataSourceParam );
		}

		public object GetRptViewDataSourceParam( decimal dataSourceID, decimal parameterSequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewDataSourceParam), new object[]{ dataSourceID, parameterSequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewDataSourceParam��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dataSourceID">DataSourceID��ģ����ѯ</param>
		/// <param name="parameterSequence">ParameterSequence��ģ����ѯ</param>
		/// <returns> RptViewDataSourceParam���ܼ�¼��</returns>
		public int QueryRptViewDataSourceParamCount( decimal dataSourceID, decimal parameterSequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVDATASRCPARAM where 1=1 and DATASOURCEID like '{0}%'  and PARAMSEQ like '{1}%' " , dataSourceID, parameterSequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewDataSourceParam
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="dataSourceID">DataSourceID��ģ����ѯ</param>
		/// <param name="parameterSequence">ParameterSequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewDataSourceParam����</returns>
		public object[] QueryRptViewDataSourceParam( decimal dataSourceID, decimal parameterSequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataSourceParam), new PagerCondition(string.Format("select {0} from TBLRPTVDATASRCPARAM where 1=1 and DATASOURCEID like '{1}%'  and PARAMSEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataSourceParam)) , dataSourceID, parameterSequence), "DATASOURCEID,PARAMSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewDataSourceParam
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewDataSourceParam���ܼ�¼��</returns>
		public object[] GetAllRptViewDataSourceParam()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDataSourceParam), new SQLCondition(string.Format("select {0} from TBLRPTVDATASRCPARAM order by DATASOURCEID,PARAMSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDataSourceParam)))));
		}


		#endregion

		#region RptViewDesignMain
		/// <summary>
		/// �Զ��屨������
		/// </summary>
		public RptViewDesignMain CreateNewRptViewDesignMain()
		{
			return new RptViewDesignMain();
		}

		public void AddRptViewDesignMain( RptViewDesignMain rptViewDesignMain)
		{
			this._helper.AddDomainObject( rptViewDesignMain );
		}

		public void UpdateRptViewDesignMain(RptViewDesignMain rptViewDesignMain)
		{
			this._helper.UpdateDomainObject( rptViewDesignMain );
		}

		public void DeleteRptViewDesignMain(RptViewDesignMain rptViewDesignMain)
		{
			this._helper.DeleteDomainObject( rptViewDesignMain );
		}

		public void DeleteRptViewDesignMain(RptViewDesignMain[] rptViewDesignMain)
		{
			this._helper.DeleteDomainObject( rptViewDesignMain );
		}

		public object GetRptViewDesignMain( string reportID )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewDesignMain), new object[]{ reportID });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewDesignMain��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <returns> RptViewDesignMain���ܼ�¼��</returns>
		public int QueryRptViewDesignMainCount( string reportID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVDESIGNMAIN where 1=1 and RPTID like '{0}%' " , reportID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewDesignMain
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewDesignMain����</returns>
		public object[] QueryRptViewDesignMain( string reportID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDesignMain), new PagerCondition(string.Format("select {0} from TBLRPTVDESIGNMAIN where 1=1 and RPTID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDesignMain)) , reportID), "RPTID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewDesignMain
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewDesignMain���ܼ�¼��</returns>
		public object[] GetAllRptViewDesignMain()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewDesignMain), new SQLCondition(string.Format("select {0} from TBLRPTVDESIGNMAIN order by RPTID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewDesignMain)))));
		}


		#endregion

		#region RptViewEntry
		/// <summary>
		/// ����ṹά��
		/// </summary>
		public RptViewEntry CreateNewRptViewEntry()
		{
			return new RptViewEntry();
		}

		public void AddRptViewEntry( RptViewEntry rptViewEntry)
		{
			this._helper.AddDomainObject( rptViewEntry );
		}

		public void UpdateRptViewEntry(RptViewEntry rptViewEntry)
		{
			this._helper.UpdateDomainObject( rptViewEntry );
		}

		public void DeleteRptViewEntry(RptViewEntry rptViewEntry)
		{
			this._helper.DeleteDomainObject( rptViewEntry );
		}

		public void DeleteRptViewEntry(RptViewEntry[] rptViewEntry)
		{
			this._helper.DeleteDomainObject( rptViewEntry );
		}

		public object GetRptViewEntry( string entryCode )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewEntry), new object[]{ entryCode });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewEntry��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="entryCode">EntryCode��ģ����ѯ</param>
		/// <returns> RptViewEntry���ܼ�¼��</returns>
		public int QueryRptViewEntryCount( string entryCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVENTRY where 1=1 and ENTRYCODE like '{0}%' " , entryCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewEntry
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="entryCode">EntryCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewEntry����</returns>
		public object[] QueryRptViewEntry( string entryCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewEntry), new PagerCondition(string.Format("select {0} from TBLRPTVENTRY where 1=1 and ENTRYCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewEntry)) , entryCode), "ENTRYCODE", inclusive, exclusive));
		}

        public object[] QueryRptViewEntryForMenu(string entryCode)
        {
            return this.DataProvider.CustomQuery(typeof(RptViewEntry), new SQLCondition(string.Format("SELECT {0} FROM tblrptventry WHERE visible = '{1}' AND entrycode LIKE '{2}%' ORDER BY PENTRYCODE, SEQ ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewEntry)), FormatHelper.TRUE_STRING, entryCode)));
        }

		/// <summary>
		/// ** ��������:	������е�RptViewEntry
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewEntry���ܼ�¼��</returns>
		public object[] GetAllRptViewEntry()
		{
            return this.DataProvider.CustomQuery(typeof(RptViewEntry), new SQLCondition(string.Format("select {0} from TBLRPTVENTRY order by Seq,ENTRYCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewEntry)))));
		}


		#endregion

		#region RptViewExtendText
		/// <summary>
		/// ������ʾ��λ
		/// </summary>
		public RptViewExtendText CreateNewRptViewExtendText()
		{
			return new RptViewExtendText();
		}

		public void AddRptViewExtendText( RptViewExtendText rptViewExtendText)
		{
			this._helper.AddDomainObject( rptViewExtendText );
		}

		public void UpdateRptViewExtendText(RptViewExtendText rptViewExtendText)
		{
			this._helper.UpdateDomainObject( rptViewExtendText );
		}

		public void DeleteRptViewExtendText(RptViewExtendText rptViewExtendText)
		{
			this._helper.DeleteDomainObject( rptViewExtendText );
		}

		public void DeleteRptViewExtendText(RptViewExtendText[] rptViewExtendText)
		{
			this._helper.DeleteDomainObject( rptViewExtendText );
		}

		public object GetRptViewExtendText( string reportID, decimal sequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewExtendText), new object[]{ reportID, sequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewExtendText��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <returns> RptViewExtendText���ܼ�¼��</returns>
		public int QueryRptViewExtendTextCount( string reportID, decimal sequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVEXTTEXT where 1=1 and RPTID like '{0}%'  and SEQ like '{1}%' " , reportID, sequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewExtendText
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewExtendText����</returns>
		public object[] QueryRptViewExtendText( string reportID, decimal sequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewExtendText), new PagerCondition(string.Format("select {0} from TBLRPTVEXTTEXT where 1=1 and RPTID like '{1}%'  and SEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewExtendText)) , reportID, sequence), "RPTID,SEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewExtendText
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewExtendText���ܼ�¼��</returns>
		public object[] GetAllRptViewExtendText()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewExtendText), new SQLCondition(string.Format("select {0} from TBLRPTVEXTTEXT order by RPTID,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewExtendText)))));
		}


		#endregion

		#region RptViewFileParameter
		/// <summary>
		/// �����ļ������б�
		/// </summary>
		public RptViewFileParameter CreateNewRptViewFileParameter()
		{
			return new RptViewFileParameter();
		}

		public void AddRptViewFileParameter( RptViewFileParameter rptViewFileParameter)
		{
			this._helper.AddDomainObject( rptViewFileParameter );
		}

		public void UpdateRptViewFileParameter(RptViewFileParameter rptViewFileParameter)
		{
			this._helper.UpdateDomainObject( rptViewFileParameter );
		}

		public void DeleteRptViewFileParameter(RptViewFileParameter rptViewFileParameter)
		{
			this._helper.DeleteDomainObject( rptViewFileParameter );
		}

		public void DeleteRptViewFileParameter(RptViewFileParameter[] rptViewFileParameter)
		{
			this._helper.DeleteDomainObject( rptViewFileParameter );
		}

		public object GetRptViewFileParameter( string reportID, decimal sequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewFileParameter), new object[]{ reportID, sequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewFileParameter��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <returns> RptViewFileParameter���ܼ�¼��</returns>
		public int QueryRptViewFileParameterCount( string reportID, decimal sequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVFILEPARAM where 1=1 and RPTID like '{0}%'  and SEQ like '{1}%' " , reportID, sequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewFileParameter
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewFileParameter����</returns>
		public object[] QueryRptViewFileParameter( string reportID, decimal sequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewFileParameter), new PagerCondition(string.Format("select {0} from TBLRPTVFILEPARAM where 1=1 and RPTID like '{1}%'  and SEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewFileParameter)) , reportID, sequence), "RPTID,SEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewFileParameter
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewFileParameter���ܼ�¼��</returns>
		public object[] GetAllRptViewFileParameter()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewFileParameter), new SQLCondition(string.Format("select {0} from TBLRPTVFILEPARAM order by RPTID,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewFileParameter)))));
		}


		#endregion

		#region RptViewFilterUI
		/// <summary>
		/// ������λ����
		/// </summary>
		public RptViewFilterUI CreateNewRptViewFilterUI()
		{
			return new RptViewFilterUI();
		}

		public void AddRptViewFilterUI( RptViewFilterUI rptViewFilterUI)
		{
			this._helper.AddDomainObject( rptViewFilterUI );
		}

		public void UpdateRptViewFilterUI(RptViewFilterUI rptViewFilterUI)
		{
			this._helper.UpdateDomainObject( rptViewFilterUI );
		}

		public void DeleteRptViewFilterUI(RptViewFilterUI rptViewFilterUI)
		{
			this._helper.DeleteDomainObject( rptViewFilterUI );
		}

		public void DeleteRptViewFilterUI(RptViewFilterUI[] rptViewFilterUI)
		{
			this._helper.DeleteDomainObject( rptViewFilterUI );
		}

		public object GetRptViewFilterUI( string reportID, decimal sequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewFilterUI), new object[]{ reportID, sequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewFilterUI��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <returns> RptViewFilterUI���ܼ�¼��</returns>
		public int QueryRptViewFilterUICount( string reportID, decimal sequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVFILTERUI where 1=1 and RPTID like '{0}%'  and SEQ like '{1}%' " , reportID, sequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewFilterUI
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewFilterUI����</returns>
		public object[] QueryRptViewFilterUI( string reportID, decimal sequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewFilterUI), new PagerCondition(string.Format("select {0} from TBLRPTVFILTERUI where 1=1 and RPTID like '{1}%'  and SEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewFilterUI)) , reportID, sequence), "RPTID,SEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewFilterUI
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewFilterUI���ܼ�¼��</returns>
		public object[] GetAllRptViewFilterUI()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewFilterUI), new SQLCondition(string.Format("select {0} from TBLRPTVFILTERUI order by RPTID,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewFilterUI)))));
		}


		#endregion

		#region RptViewGridColumn
		/// <summary>
		/// ������ʾ��λ
		/// </summary>
		public RptViewGridColumn CreateNewRptViewGridColumn()
		{
			return new RptViewGridColumn();
		}

		public void AddRptViewGridColumn( RptViewGridColumn rptViewGridColumn)
		{
			this._helper.AddDomainObject( rptViewGridColumn );
		}

		public void UpdateRptViewGridColumn(RptViewGridColumn rptViewGridColumn)
		{
			this._helper.UpdateDomainObject( rptViewGridColumn );
		}

		public void DeleteRptViewGridColumn(RptViewGridColumn rptViewGridColumn)
		{
			this._helper.DeleteDomainObject( rptViewGridColumn );
		}

		public void DeleteRptViewGridColumn(RptViewGridColumn[] rptViewGridColumn)
		{
			this._helper.DeleteDomainObject( rptViewGridColumn );
		}

		public object GetRptViewGridColumn( string reportID, decimal displaySequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewGridColumn), new object[]{ reportID, displaySequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewGridColumn��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="displaySequence">DisplaySequence��ģ����ѯ</param>
		/// <returns> RptViewGridColumn���ܼ�¼��</returns>
		public int QueryRptViewGridColumnCount( string reportID, decimal displaySequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVGRIDCOLUMN where 1=1 and RPTID like '{0}%'  and DISPLAYSEQ like '{1}%' " , reportID, displaySequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewGridColumn
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="displaySequence">DisplaySequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewGridColumn����</returns>
		public object[] QueryRptViewGridColumn( string reportID, decimal displaySequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridColumn), new PagerCondition(string.Format("select {0} from TBLRPTVGRIDCOLUMN where 1=1 and RPTID like '{1}%'  and DISPLAYSEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridColumn)) , reportID, displaySequence), "RPTID,DISPLAYSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewGridColumn
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewGridColumn���ܼ�¼��</returns>
		public object[] GetAllRptViewGridColumn()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridColumn), new SQLCondition(string.Format("select {0} from TBLRPTVGRIDCOLUMN order by RPTID,DISPLAYSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridColumn)))));
		}


		#endregion

		#region RptViewGridDataFormat
		/// <summary>
		/// ������ʾ��λ
		/// </summary>
		public RptViewGridDataFormat CreateNewRptViewGridDataFormat()
		{
			return new RptViewGridDataFormat();
		}

		public void AddRptViewGridDataFormat( RptViewGridDataFormat rptViewGridDataFormat)
		{
			this._helper.AddDomainObject( rptViewGridDataFormat );
		}

		public void UpdateRptViewGridDataFormat(RptViewGridDataFormat rptViewGridDataFormat)
		{
			this._helper.UpdateDomainObject( rptViewGridDataFormat );
		}

		public void DeleteRptViewGridDataFormat(RptViewGridDataFormat rptViewGridDataFormat)
		{
			this._helper.DeleteDomainObject( rptViewGridDataFormat );
		}

		public void DeleteRptViewGridDataFormat(RptViewGridDataFormat[] rptViewGridDataFormat)
		{
			this._helper.DeleteDomainObject( rptViewGridDataFormat );
		}

		public object GetRptViewGridDataFormat( string reportID, string columnName, string styleType, decimal groupSequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewGridDataFormat), new object[]{ reportID, columnName, styleType, groupSequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewGridDataFormat��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="columnName">ColumnName��ģ����ѯ</param>
		/// <param name="styleType">StyleType��ģ����ѯ</param>
		/// <param name="groupSequence">GroupSequence��ģ����ѯ</param>
		/// <returns> RptViewGridDataFormat���ܼ�¼��</returns>
		public int QueryRptViewGridDataFormatCount( string reportID, string columnName, string styleType, decimal groupSequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVGRIDDATAFMT where 1=1 and RPTID like '{0}%'  and COLUMNNAME like '{1}%'  and STYLETYPE like '{2}%'  and GRPSEQ like '{3}%' " , reportID, columnName, styleType, groupSequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewGridDataFormat
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="columnName">ColumnName��ģ����ѯ</param>
		/// <param name="styleType">StyleType��ģ����ѯ</param>
		/// <param name="groupSequence">GroupSequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewGridDataFormat����</returns>
		public object[] QueryRptViewGridDataFormat( string reportID, string columnName, string styleType, decimal groupSequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridDataFormat), new PagerCondition(string.Format("select {0} from TBLRPTVGRIDDATAFMT where 1=1 and RPTID like '{1}%'  and COLUMNNAME like '{2}%'  and STYLETYPE like '{3}%'  and GRPSEQ like '{4}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridDataFormat)) , reportID, columnName, styleType, groupSequence), "RPTID,COLUMNNAME,STYLETYPE,GRPSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewGridDataFormat
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewGridDataFormat���ܼ�¼��</returns>
		public object[] GetAllRptViewGridDataFormat()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridDataFormat), new SQLCondition(string.Format("select {0} from TBLRPTVGRIDDATAFMT order by RPTID,COLUMNNAME,STYLETYPE,GRPSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridDataFormat)))));
		}


		#endregion

		#region RptViewGridDataStyle
		/// <summary>
		/// ������ʾ��λ
		/// </summary>
		public RptViewGridDataStyle CreateNewRptViewGridDataStyle()
		{
			return new RptViewGridDataStyle();
		}

		public void AddRptViewGridDataStyle( RptViewGridDataStyle rptViewGridDataStyle)
		{
			this._helper.AddDomainObject( rptViewGridDataStyle );
		}

		public void UpdateRptViewGridDataStyle(RptViewGridDataStyle rptViewGridDataStyle)
		{
			this._helper.UpdateDomainObject( rptViewGridDataStyle );
		}

		public void DeleteRptViewGridDataStyle(RptViewGridDataStyle rptViewGridDataStyle)
		{
			this._helper.DeleteDomainObject( rptViewGridDataStyle );
		}

		public void DeleteRptViewGridDataStyle(RptViewGridDataStyle[] rptViewGridDataStyle)
		{
			this._helper.DeleteDomainObject( rptViewGridDataStyle );
		}

		public object GetRptViewGridDataStyle( string reportID )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewGridDataStyle), new object[]{ reportID });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewGridDataStyle��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <returns> RptViewGridDataStyle���ܼ�¼��</returns>
		public int QueryRptViewGridDataStyleCount( string reportID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVGRIDDATASTYLE where 1=1 and RPTID like '{0}%' " , reportID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewGridDataStyle
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewGridDataStyle����</returns>
		public object[] QueryRptViewGridDataStyle( string reportID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridDataStyle), new PagerCondition(string.Format("select {0} from TBLRPTVGRIDDATASTYLE where 1=1 and RPTID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridDataStyle)) , reportID), "RPTID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewGridDataStyle
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewGridDataStyle���ܼ�¼��</returns>
		public object[] GetAllRptViewGridDataStyle()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridDataStyle), new SQLCondition(string.Format("select {0} from TBLRPTVGRIDDATASTYLE order by RPTID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridDataStyle)))));
		}


		#endregion

		#region RptViewGridFilter
		/// <summary>
		/// ��������趨
		/// </summary>
		public RptViewGridFilter CreateNewRptViewGridFilter()
		{
			return new RptViewGridFilter();
		}

		public void AddRptViewGridFilter( RptViewGridFilter rptViewGridFilter)
		{
			this._helper.AddDomainObject( rptViewGridFilter );
		}

		public void UpdateRptViewGridFilter(RptViewGridFilter rptViewGridFilter)
		{
			this._helper.UpdateDomainObject( rptViewGridFilter );
		}

		public void DeleteRptViewGridFilter(RptViewGridFilter rptViewGridFilter)
		{
			this._helper.DeleteDomainObject( rptViewGridFilter );
		}

		public void DeleteRptViewGridFilter(RptViewGridFilter[] rptViewGridFilter)
		{
			this._helper.DeleteDomainObject( rptViewGridFilter );
		}

		public object GetRptViewGridFilter( string reportID, decimal filterSequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewGridFilter), new object[]{ reportID, filterSequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewGridFilter��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="filterSequence">FilterSequence��ģ����ѯ</param>
		/// <returns> RptViewGridFilter���ܼ�¼��</returns>
		public int QueryRptViewGridFilterCount( string reportID, decimal filterSequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVGRIDFLT where 1=1 and RPTID like '{0}%'  and FLTSEQ like '{1}%' " , reportID, filterSequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewGridFilter
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="filterSequence">FilterSequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewGridFilter����</returns>
		public object[] QueryRptViewGridFilter( string reportID, decimal filterSequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridFilter), new PagerCondition(string.Format("select {0} from TBLRPTVGRIDFLT where 1=1 and RPTID like '{1}%'  and FLTSEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridFilter)) , reportID, filterSequence), "RPTID,FLTSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewGridFilter
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewGridFilter���ܼ�¼��</returns>
		public object[] GetAllRptViewGridFilter()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridFilter), new SQLCondition(string.Format("select {0} from TBLRPTVGRIDFLT order by RPTID,FLTSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridFilter)))));
		}


		#endregion

		#region RptViewGridGroup
		/// <summary>
		/// ��������趨
		/// </summary>
		public RptViewGridGroup CreateNewRptViewGridGroup()
		{
			return new RptViewGridGroup();
		}

		public void AddRptViewGridGroup( RptViewGridGroup rptViewGridGroup)
		{
			this._helper.AddDomainObject( rptViewGridGroup );
		}

		public void UpdateRptViewGridGroup(RptViewGridGroup rptViewGridGroup)
		{
			this._helper.UpdateDomainObject( rptViewGridGroup );
		}

		public void DeleteRptViewGridGroup(RptViewGridGroup rptViewGridGroup)
		{
			this._helper.DeleteDomainObject( rptViewGridGroup );
		}

		public void DeleteRptViewGridGroup(RptViewGridGroup[] rptViewGridGroup)
		{
			this._helper.DeleteDomainObject( rptViewGridGroup );
		}

		public object GetRptViewGridGroup( string reportID, decimal groupSequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewGridGroup), new object[]{ reportID, groupSequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewGridGroup��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="groupSequence">GroupSequence��ģ����ѯ</param>
		/// <returns> RptViewGridGroup���ܼ�¼��</returns>
		public int QueryRptViewGridGroupCount( string reportID, decimal groupSequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVGRIDGRP where 1=1 and RPTID like '{0}%'  and GRPSEQ like '{1}%' " , reportID, groupSequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewGridGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="groupSequence">GroupSequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewGridGroup����</returns>
		public object[] QueryRptViewGridGroup( string reportID, decimal groupSequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridGroup), new PagerCondition(string.Format("select {0} from TBLRPTVGRIDGRP where 1=1 and RPTID like '{1}%'  and GRPSEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridGroup)) , reportID, groupSequence), "RPTID,GRPSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewGridGroup
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewGridGroup���ܼ�¼��</returns>
		public object[] GetAllRptViewGridGroup()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridGroup), new SQLCondition(string.Format("select {0} from TBLRPTVGRIDGRP order by RPTID,GRPSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridGroup)))));
		}


		#endregion

		#region RptViewGridGroupTotal
		/// <summary>
		/// ��������趨
		/// </summary>
		public RptViewGridGroupTotal CreateNewRptViewGridGroupTotal()
		{
			return new RptViewGridGroupTotal();
		}

		public void AddRptViewGridGroupTotal( RptViewGridGroupTotal rptViewGridGroupTotal)
		{
			this._helper.AddDomainObject( rptViewGridGroupTotal );
		}

		public void UpdateRptViewGridGroupTotal(RptViewGridGroupTotal rptViewGridGroupTotal)
		{
			this._helper.UpdateDomainObject( rptViewGridGroupTotal );
		}

		public void DeleteRptViewGridGroupTotal(RptViewGridGroupTotal rptViewGridGroupTotal)
		{
			this._helper.DeleteDomainObject( rptViewGridGroupTotal );
		}

		public void DeleteRptViewGridGroupTotal(RptViewGridGroupTotal[] rptViewGridGroupTotal)
		{
			this._helper.DeleteDomainObject( rptViewGridGroupTotal );
		}

		public object GetRptViewGridGroupTotal( string reportID, decimal groupSequence, string columnName )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewGridGroupTotal), new object[]{ reportID, groupSequence, columnName });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewGridGroupTotal��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="groupSequence">GroupSequence��ģ����ѯ</param>
		/// <param name="columnName">ColumnName��ģ����ѯ</param>
		/// <returns> RptViewGridGroupTotal���ܼ�¼��</returns>
		public int QueryRptViewGridGroupTotalCount( string reportID, decimal groupSequence, string columnName)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVGRIDGRPTOTAL where 1=1 and RPTID like '{0}%'  and GRPSEQ like '{1}%'  and COLUMNNAME like '{2}%' " , reportID, groupSequence, columnName)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewGridGroupTotal
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="groupSequence">GroupSequence��ģ����ѯ</param>
		/// <param name="columnName">ColumnName��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewGridGroupTotal����</returns>
		public object[] QueryRptViewGridGroupTotal( string reportID, decimal groupSequence, string columnName, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridGroupTotal), new PagerCondition(string.Format("select {0} from TBLRPTVGRIDGRPTOTAL where 1=1 and RPTID like '{1}%'  and GRPSEQ like '{2}%'  and COLUMNNAME like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridGroupTotal)) , reportID, groupSequence, columnName), "RPTID,GRPSEQ,COLUMNNAME", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewGridGroupTotal
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewGridGroupTotal���ܼ�¼��</returns>
		public object[] GetAllRptViewGridGroupTotal()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewGridGroupTotal), new SQLCondition(string.Format("select {0} from TBLRPTVGRIDGRPTOTAL order by RPTID,GRPSEQ,COLUMNNAME", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewGridGroupTotal)))));
		}


		#endregion

		#region RptViewReportSecurity
		/// <summary>
		/// ����Ȩ��
		/// </summary>
		public RptViewReportSecurity CreateNewRptViewReportSecurity()
		{
			return new RptViewReportSecurity();
		}

		public void AddRptViewReportSecurity( RptViewReportSecurity rptViewReportSecurity)
		{
			this._helper.AddDomainObject( rptViewReportSecurity );
		}

		public void UpdateRptViewReportSecurity(RptViewReportSecurity rptViewReportSecurity)
		{
			this._helper.UpdateDomainObject( rptViewReportSecurity );
		}

		public void DeleteRptViewReportSecurity(RptViewReportSecurity rptViewReportSecurity)
		{
			this._helper.DeleteDomainObject( rptViewReportSecurity );
		}

		public void DeleteRptViewReportSecurity(RptViewReportSecurity[] rptViewReportSecurity)
		{
			this._helper.DeleteDomainObject( rptViewReportSecurity );
		}

		public object GetRptViewReportSecurity( string reportID, decimal sequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewReportSecurity), new object[]{ reportID, sequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewReportSecurity��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <returns> RptViewReportSecurity���ܼ�¼��</returns>
		public int QueryRptViewReportSecurityCount( string reportID, decimal sequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVRPTSECURITY where 1=1 and RPTID like '{0}%'  and SEQ like '{1}%' " , reportID, sequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewReportSecurity
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewReportSecurity����</returns>
		public object[] QueryRptViewReportSecurity( string reportID, decimal sequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewReportSecurity), new PagerCondition(string.Format("select {0} from TBLRPTVRPTSECURITY where 1=1 and RPTID like '{1}%'  and SEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewReportSecurity)) , reportID, sequence), "RPTID,SEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewReportSecurity
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewReportSecurity���ܼ�¼��</returns>
		public object[] GetAllRptViewReportSecurity()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewReportSecurity), new SQLCondition(string.Format("select {0} from TBLRPTVRPTSECURITY order by RPTID,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewReportSecurity)))));
		}


		#endregion

		#region RptViewReportStyle
		/// <summary>
		/// ������ʽ
///
		/// </summary>
		public RptViewReportStyle CreateNewRptViewReportStyle()
		{
			return new RptViewReportStyle();
		}

		public void AddRptViewReportStyle( RptViewReportStyle rptViewReportStyle)
		{
			this._helper.AddDomainObject( rptViewReportStyle );
		}

		public void UpdateRptViewReportStyle(RptViewReportStyle rptViewReportStyle)
		{
			this._helper.UpdateDomainObject( rptViewReportStyle );
		}

		public void DeleteRptViewReportStyle(RptViewReportStyle rptViewReportStyle)
		{
			this._helper.DeleteDomainObject( rptViewReportStyle );
		}

		public void DeleteRptViewReportStyle(RptViewReportStyle[] rptViewReportStyle)
		{
			this._helper.DeleteDomainObject( rptViewReportStyle );
		}

		public object GetRptViewReportStyle( decimal styleID )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewReportStyle), new object[]{ styleID });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewReportStyle��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="styleID">StyleID��ģ����ѯ</param>
		/// <returns> RptViewReportStyle���ܼ�¼��</returns>
		public int QueryRptViewReportStyleCount( decimal styleID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVSTYLE where 1=1 and STYLEID like '{0}%' " , styleID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewReportStyle
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="styleID">StyleID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewReportStyle����</returns>
		public object[] QueryRptViewReportStyle( decimal styleID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewReportStyle), new PagerCondition(string.Format("select {0} from TBLRPTVSTYLE where 1=1 and STYLEID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewReportStyle)) , styleID), "STYLEID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewReportStyle
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewReportStyle���ܼ�¼��</returns>
		public object[] GetAllRptViewReportStyle()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewReportStyle), new SQLCondition(string.Format("select {0} from TBLRPTVSTYLE order by STYLEID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewReportStyle)))));
		}


		#endregion

		#region RptViewReportStyleDetail
		/// <summary>
		/// ������ʽ
///
		/// </summary>
		public RptViewReportStyleDetail CreateNewRptViewReportStyleDetail()
		{
			return new RptViewReportStyleDetail();
		}

		public void AddRptViewReportStyleDetail( RptViewReportStyleDetail rptViewReportStyleDetail)
		{
			this._helper.AddDomainObject( rptViewReportStyleDetail );
		}

		public void UpdateRptViewReportStyleDetail(RptViewReportStyleDetail rptViewReportStyleDetail)
		{
			this._helper.UpdateDomainObject( rptViewReportStyleDetail );
		}

		public void DeleteRptViewReportStyleDetail(RptViewReportStyleDetail rptViewReportStyleDetail)
		{
			this._helper.DeleteDomainObject( rptViewReportStyleDetail );
		}

		public void DeleteRptViewReportStyleDetail(RptViewReportStyleDetail[] rptViewReportStyleDetail)
		{
			this._helper.DeleteDomainObject( rptViewReportStyleDetail );
		}

		public object GetRptViewReportStyleDetail( decimal styleID, string styleType )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewReportStyleDetail), new object[]{ styleID, styleType });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewReportStyleDetail��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="styleID">StyleID��ģ����ѯ</param>
		/// <param name="styleType">StyleType��ģ����ѯ</param>
		/// <returns> RptViewReportStyleDetail���ܼ�¼��</returns>
		public int QueryRptViewReportStyleDetailCount( decimal styleID, string styleType)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVSTYLEDTL where 1=1 and STYLEID like '{0}%'  and STYLETYPE like '{1}%' " , styleID, styleType)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewReportStyleDetail
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="styleID">StyleID��ģ����ѯ</param>
		/// <param name="styleType">StyleType��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewReportStyleDetail����</returns>
		public object[] QueryRptViewReportStyleDetail( decimal styleID, string styleType, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewReportStyleDetail), new PagerCondition(string.Format("select {0} from TBLRPTVSTYLEDTL where 1=1 and STYLEID like '{1}%'  and STYLETYPE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewReportStyleDetail)) , styleID, styleType), "STYLEID,STYLETYPE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewReportStyleDetail
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewReportStyleDetail���ܼ�¼��</returns>
		public object[] GetAllRptViewReportStyleDetail()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewReportStyleDetail), new SQLCondition(string.Format("select {0} from TBLRPTVSTYLEDTL order by STYLEID,STYLETYPE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewReportStyleDetail)))));
		}


		#endregion

		#region RptViewUserDefault
		/// <summary>
		/// �û�����
		/// </summary>
		public RptViewUserDefault CreateNewRptViewUserDefault()
		{
			return new RptViewUserDefault();
		}

		public void AddRptViewUserDefault( RptViewUserDefault rptViewUserDefault)
		{
			this._helper.AddDomainObject( rptViewUserDefault );
		}

		public void UpdateRptViewUserDefault(RptViewUserDefault rptViewUserDefault)
		{
			this._helper.UpdateDomainObject( rptViewUserDefault );
		}

		public void DeleteRptViewUserDefault(RptViewUserDefault rptViewUserDefault)
		{
			this._helper.DeleteDomainObject( rptViewUserDefault );
		}

		public void DeleteRptViewUserDefault(RptViewUserDefault[] rptViewUserDefault)
		{
			this._helper.DeleteDomainObject( rptViewUserDefault );
		}

		public object GetRptViewUserDefault( string userCode )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewUserDefault), new object[]{ userCode });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewUserDefault��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="userCode">UserCode��ģ����ѯ</param>
		/// <returns> RptViewUserDefault���ܼ�¼��</returns>
		public int QueryRptViewUserDefaultCount( string userCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVUSERDFT where 1=1 and USERCODE like '{0}%' " , userCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewUserDefault
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="userCode">UserCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewUserDefault����</returns>
		public object[] QueryRptViewUserDefault( string userCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewUserDefault), new PagerCondition(string.Format("select {0} from TBLRPTVUSERDFT where 1=1 and USERCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewUserDefault)) , userCode), "USERCODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewUserDefault
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewUserDefault���ܼ�¼��</returns>
		public object[] GetAllRptViewUserDefault()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewUserDefault), new SQLCondition(string.Format("select {0} from TBLRPTVUSERDFT order by USERCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewUserDefault)))));
		}


		#endregion

		#region RptViewUserSubscription
		/// <summary>
		/// �û�����
		/// </summary>
		public RptViewUserSubscription CreateNewRptViewUserSubscription()
		{
			return new RptViewUserSubscription();
		}

		public void AddRptViewUserSubscription( RptViewUserSubscription rptViewUserSubscription)
		{
			this._helper.AddDomainObject( rptViewUserSubscription );
		}

		public void UpdateRptViewUserSubscription(RptViewUserSubscription rptViewUserSubscription)
		{
			this._helper.UpdateDomainObject( rptViewUserSubscription );
		}

		public void DeleteRptViewUserSubscription(RptViewUserSubscription rptViewUserSubscription)
		{
			this._helper.DeleteDomainObject( rptViewUserSubscription );
		}

		public void DeleteRptViewUserSubscription(RptViewUserSubscription[] rptViewUserSubscription)
		{
			this._helper.DeleteDomainObject( rptViewUserSubscription );
		}

		public object GetRptViewUserSubscription( string userCode, string reportID, decimal sequence )
		{
			return this.DataProvider.CustomSearch(typeof(RptViewUserSubscription), new object[]{ userCode, reportID, sequence });
		}

		/// <summary>
		/// ** ��������:	��ѯRptViewUserSubscription��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="userCode">UserCode��ģ����ѯ</param>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <returns> RptViewUserSubscription���ܼ�¼��</returns>
		public int QueryRptViewUserSubscriptionCount( string userCode, string reportID, decimal sequence)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRPTVUSERSUBSCR where 1=1 and USERCODE like '{0}%'  and RPTID like '{1}%'  and SEQ like '{2}%' " , userCode, reportID, sequence)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯRptViewUserSubscription
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="userCode">UserCode��ģ����ѯ</param>
		/// <param name="reportID">ReportID��ģ����ѯ</param>
		/// <param name="sequence">Sequence��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> RptViewUserSubscription����</returns>
		public object[] QueryRptViewUserSubscription( string userCode, string reportID, decimal sequence, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(RptViewUserSubscription), new PagerCondition(string.Format("select {0} from TBLRPTVUSERSUBSCR where 1=1 and USERCODE like '{1}%'  and RPTID like '{2}%'  and SEQ like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewUserSubscription)) , userCode, reportID, sequence), "USERCODE,RPTID,SEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�RptViewUserSubscription
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2007-7-17 9:17:33
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>RptViewUserSubscription���ܼ�¼��</returns>
		public object[] GetAllRptViewUserSubscription()
		{
			return this.DataProvider.CustomQuery(typeof(RptViewUserSubscription), new SQLCondition(string.Format("select {0} from TBLRPTVUSERSUBSCR order by USERCODE,RPTID,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RptViewUserSubscription)))));
		}


		#endregion

	}
}

