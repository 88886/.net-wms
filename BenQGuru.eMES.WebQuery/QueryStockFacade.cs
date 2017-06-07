using System;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Common.Helper;  
using BenQGuru.eMES.Common.DomainDataProvider;   
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Material;

namespace BenQGuru.eMES.WebQuery
{
	/// <summary>
	/// QueryStockFacade ��ժҪ˵����
	/// </summary>
	public class QueryStockFacade
	{
		private  IDomainDataProvider _domainDataProvider= null;

		public QueryStockFacade( IDomainDataProvider domainDataProvider )
		{
			this._domainDataProvider = domainDataProvider;
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

		#region Stock In
		/// <summary>
		/// ** ��������:	�����ϸ��ѯ,��ҳ
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-07-27
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="startTicketNo">��ʼ��ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="endTicketNo">������ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="startInDate">��ʼ���ʱ��,Ϊ0������ѯ����</param>
		/// <param name="endInDate">�������ʱ��,Ϊ0������ѯ����</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns></returns>
		public object[] QueryMaterialStockIn( 
			string startTicketNo, string endTicketNo, 
			int startInDate, int endInDate, 
			int inclusive, int exclusive )
		{
			string ticketCondition = string.Empty;

			if ( startTicketNo != null && startTicketNo != string.Empty )
			{
				ticketCondition += string.Format(" and TKTNO >= '{0}'", startTicketNo);
			}
			if ( endTicketNo != null && endTicketNo != string.Empty )
			{
				ticketCondition += string.Format(" and TKTNO <= '{0}'", endTicketNo);
			}

			string dateCondition = string.Empty;

			if ( startInDate != 0 )
			{
				dateCondition += string.Format(" and MDATE >= {0}", startInDate);
			}
			if ( endInDate != 0 )
			{
				dateCondition += string.Format(" and MDATE <= {0}", endInDate);
			}
			
			return this.DataProvider.CustomQuery(
				typeof(QStockInDetail),	
				new PagerCondition( 
				string.Format( @"select {0}, b.QTY from( select * from TBLMSTOCKIN where 1=1 {1} {2}) TBLMSTOCKIN, 
									( select TKTNO, count(RCARD) as QTY from TBLMSTOCKIN where 1=1 {1} group by TKTNO ) b
									where TBLMSTOCKIN.TKTNO = b.TKTNO", 
				DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(MaterialStockIn)),
				ticketCondition, 
				dateCondition ), 
				"TBLMSTOCKIN.TKTNO", 
				inclusive, 
				exclusive ) );
		}


		/// <summary>
		/// Added by Jessie Lee,2005/9/5
		/// </summary>
		/// <param name="startTicketNo">��ʼ��ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="endTicketNo">������ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="startInDate">��ʼ���ʱ��,Ϊ0������ѯ����</param>
		/// <param name="endInDate">�������ʱ��,Ϊ0������ѯ����</param>
		/// <param name="includeDelStatus">�Ƿ����ɾ��״̬</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns></returns>
		public object[] QueryMaterialStockIn( 
			string startTicketNo, 
			string endTicketNo, 
			int startInDate, 
			int endInDate,
			bool includeDelStatus,
			int inclusive, 
			int exclusive )
		{
			string ticketCondition = string.Empty;

			if ( startTicketNo != null && startTicketNo != string.Empty )
			{
				ticketCondition += string.Format(" and TKTNO >= '{0}'", startTicketNo);
			}
			if ( endTicketNo != null && endTicketNo != string.Empty )
			{
				ticketCondition += string.Format(" and TKTNO <= '{0}'", endTicketNo);
			}

			string dateCondition = string.Empty;

			if ( startInDate != 0 )
			{
				dateCondition += string.Format(" and MDATE >= {0}", startInDate);
			}
			if ( endInDate != 0 )
			{
				dateCondition += string.Format(" and MDATE <= {0}", endInDate);
			}

			//����ɾ��״̬������ʾ����״̬
			if(includeDelStatus)
			{
				return this.DataProvider.CustomQuery(
					typeof(QStockInDetail),	
					new PagerCondition( 
					string.Format( @"select {0}, b.QTY from( select * from TBLMSTOCKIN where 1=1 {1} {2}) TBLMSTOCKIN, 
									( select TKTNO, count(RCARD) as QTY from TBLMSTOCKIN where 1=1 {1} group by TKTNO ) b
									where TBLMSTOCKIN.TKTNO = b.TKTNO", 
					DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(MaterialStockIn)),
					ticketCondition, 
					dateCondition ), 
					"TBLMSTOCKIN.TKTNO", 
					inclusive, 
					exclusive ) );
			}

			//������ɾ��״̬
			return this.DataProvider.CustomQuery(
				typeof(QStockInDetail),	
				new PagerCondition( 
				string.Format( @"select {0}, b.QTY from( select * from TBLMSTOCKIN where status<>'{3}' {1} {2}) TBLMSTOCKIN, 
									( select TKTNO, count(RCARD) as QTY from TBLMSTOCKIN where 1=1 {1} group by TKTNO ) b
									where TBLMSTOCKIN.TKTNO = b.TKTNO", 
				DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(MaterialStockIn)),
				ticketCondition, 
				dateCondition,
				StockStatus.Deleted), 
				"TBLMSTOCKIN.TKTNO", 
				inclusive, 
				exclusive ) );
		}
		/// <summary>
		/// ** ��������:	�����ϸ��ѯ,��������
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-08-01
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="startTicketNo">��ʼ��ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="endTicketNo">������ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="startInDate">��ʼ���ʱ��,Ϊ0������ѯ����</param>
		/// <param name="endInDate">�������ʱ��,Ϊ0������ѯ����</param>
		/// <returns></returns>
		public int QueryMaterialStockInCount( 
			string startTicketNo, string endTicketNo, 
			int startInDate, int endInDate)
		{			
			string condition = string.Empty;

			if ( startTicketNo != null && startTicketNo != string.Empty )
			{
				condition += string.Format(" and TKTNO >= '{0}'", startTicketNo);
			}
			if ( endTicketNo != null && endTicketNo != string.Empty )
			{
				condition += string.Format(" and TKTNO <= '{0}'", endTicketNo);
			}

			if ( startInDate != 0 )
			{
				condition += string.Format(" and MDATE >= {0}", startInDate);
			}
			if ( endInDate != 0 )
			{
				condition += string.Format(" and MDATE <= {0}", endInDate);
			}
			
			return this.DataProvider.GetCount( new SQLCondition( 
				string.Format( @"select count(*) from TBLMSTOCKIN where 1=1 {0}",condition )));
		}

		/// <summary>
		/// Added by Jessie Lee,2005/9/5
		/// </summary>
		/// <param name="startTicketNo">��ʼ��ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="endTicketNo">������ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="startInDate">��ʼ���ʱ��,Ϊ0������ѯ����</param>
		/// <param name="endInDate">�������ʱ��,Ϊ0������ѯ����</param>
		/// <param name="includeDelStatus">�Ƿ����ɾ��״̬</param>
		/// <returns></returns>
		public int QueryMaterialStockInCount( 
			string startTicketNo, string endTicketNo, 
			int startInDate, int endInDate,bool includeDelStatus)
		{			
			string condition = string.Empty;

			if ( startTicketNo != null && startTicketNo != string.Empty )
			{
				condition += string.Format(" and TKTNO >= '{0}'", startTicketNo);
			}
			if ( endTicketNo != null && endTicketNo != string.Empty )
			{
				condition += string.Format(" and TKTNO <= '{0}'", endTicketNo);
			}

			if ( startInDate != 0 )
			{
				condition += string.Format(" and MDATE >= {0}", startInDate);
			}
			if ( endInDate != 0 )
			{
				condition += string.Format(" and MDATE <= {0}", endInDate);
			}
			
			if(includeDelStatus)
			{
				return this.DataProvider.GetCount( new SQLCondition( 
					string.Format( @"select count(*) from TBLMSTOCKIN where 1=1 {0}",condition )));
			}

			return this.DataProvider.GetCount( new SQLCondition( 
				string.Format( @"select count(*) from TBLMSTOCKIN where status<>'{1}' {0}",condition,StockStatus.Deleted )));	
		}
		#endregion

		#region Stock Out
		/// <summary>
		/// ** ��������:	��������ϸ��ѯ,��ҳ
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-08-01
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="startTicketNo">��ʼ��ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="endTicketNo">������ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="startRunningCard">��ʼ��Ʒ���к�,Ϊ�ղ�����ѯ����</param>
		/// <param name="endRunningCard">������Ʒ���к�,Ϊ�ղ�����ѯ����</param>
		/// <param name="modelCode">���ִ��룬��','�ָ���ַ�������ȷ��ѯ</param>
		/// <param name="dealer">�����̣�ģ����ѯ</param>
		/// <param name="startOutDate">��ʼ����ʱ��,Ϊ0������ѯ����</param>
		/// <param name="endOutDate">��������ʱ��,Ϊ0������ѯ����</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns></returns>
		public object[] QueryMaterialStockOut( 
			string startTicketNo, string endTicketNo, 
			string startRunningCard, string endRunningCard,
			string modelCode, string dealer,
			int startOutDate, int endOutDate, 
			int inclusive, int exclusive )
		{
			string condition = string.Empty;

			if ( startTicketNo != null && startTicketNo != string.Empty )
			{
				condition += string.Format(" and tblmstockout.TKTNO >= '{0}'", startTicketNo);
			}
			if ( endTicketNo != null && endTicketNo != string.Empty )
			{
				condition += string.Format(" and tblmstockout.TKTNO <= '{0}'", endTicketNo);
			}

			if ( startRunningCard != null && startRunningCard != string.Empty )
			{
				condition += string.Format(" and tblmstockoutdetail.rcard >= '{0}'", startRunningCard);
			}
			if ( endRunningCard != null && endRunningCard != string.Empty )
			{
				condition += string.Format(" and tblmstockoutdetail.rcard <= '{0}'", endRunningCard);
			}

			if ( modelCode != null && modelCode != string.Empty )
			{
				condition += string.Format(" and tblmstockout.modelcode in ({0})", FormatHelper.ProcessQueryValues(modelCode));
			}
			if ( dealer != null && dealer != string.Empty )
			{
				condition += string.Format(" and upper(tblmstockout.dealer) like '{0}%'", dealer.ToUpper());
			}

			if ( startOutDate != 0 )
			{
				condition += string.Format(" and tblmstockout.outdate >= {0}", startOutDate);
			}
			if ( endOutDate != 0 )
			{
				condition += string.Format(" and tblmstockout.outdate <= {0}", endOutDate);
			}
			
			return this.DataProvider.CustomQuery(
				typeof(QStockOutDetail),	
				new PagerCondition( 
				string.Format( @"select {0},tblmstockoutdetail.rcard from tblmstockout,tblmstockoutdetail 
												where tblmstockout.tktno=tblmstockoutdetail.tktno and tblmstockout.seq=tblmstockoutdetail.seq {1}", 
				DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(MaterialStockOut)),
				condition), 
				"tblmstockout.TKTNO,tblmstockout.modelcode,tblmstockout.dealer,tblmstockout.outdate", 
				inclusive, 
				exclusive ) );
		}

		
		/// <summary>
		/// Added by jessie lee,2005/9/5
		/// </summary>
		/// <param name="startTicketNo">��ʼ��ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="endTicketNo">������ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="startRunningCard">��ʼ��Ʒ���к�,Ϊ�ղ�����ѯ����</param>
		/// <param name="endRunningCard">������Ʒ���к�,Ϊ�ղ�����ѯ����</param>
		/// <param name="modelCode">���ִ��룬��','�ָ���ַ�������ȷ��ѯ</param>
		/// <param name="dealer">�����̣�ģ����ѯ</param>
		/// <param name="startOutDate">��ʼ����ʱ��,Ϊ0������ѯ����</param>
		/// <param name="endOutDate">��������ʱ��,Ϊ0������ѯ����</param>
		/// <param name="includeDelStatus">�Ƿ����ɾ��״̬</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns></returns>
		public object[] QueryMaterialStockOut( 
			string startTicketNo, string endTicketNo, 
			string startRunningCard, string endRunningCard,
			string modelCode, string dealer,
			int startOutDate, int endOutDate,
			bool includeDelStatus,
			int inclusive, int exclusive )
		{
			string condition = string.Empty;

			if ( startTicketNo != null && startTicketNo != string.Empty )
			{
				condition += string.Format(" and tblmstockout.TKTNO >= '{0}'", startTicketNo);
			}
			if ( endTicketNo != null && endTicketNo != string.Empty )
			{
				condition += string.Format(" and tblmstockout.TKTNO <= '{0}'", endTicketNo);
			}

			if ( startRunningCard != null && startRunningCard != string.Empty )
			{
				condition += string.Format(" and tblmstockoutdetail.rcard >= '{0}'", startRunningCard);
			}
			if ( endRunningCard != null && endRunningCard != string.Empty )
			{
				condition += string.Format(" and tblmstockoutdetail.rcard <= '{0}'", endRunningCard);
			}

			if ( modelCode != null && modelCode != string.Empty )
			{
				condition += string.Format(" and tblmstockout.modelcode in ({0})", FormatHelper.ProcessQueryValues(modelCode));
			}
			if ( dealer != null && dealer != string.Empty )
			{
				condition += string.Format(" and upper(tblmstockout.dealer) like '{0}%'", dealer.ToUpper());
			}

			if ( startOutDate != 0 )
			{
				condition += string.Format(" and tblmstockout.outdate >= {0}", startOutDate);
			}
			if ( endOutDate != 0 )
			{
				condition += string.Format(" and tblmstockout.outdate <= {0}", endOutDate);
			}
			//����ɾ��״̬����������״̬
			if(includeDelStatus)
			{
				return this.DataProvider.CustomQuery(
					typeof(QStockOutDetail),	
					new PagerCondition( 
					string.Format( @"select {0},tblmstockoutdetail.rcard from tblmstockout,tblmstockoutdetail 
												where tblmstockout.tktno=tblmstockoutdetail.tktno and tblmstockout.seq=tblmstockoutdetail.seq {1}", 
					DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(MaterialStockOut)),
					condition), 
					"tblmstockout.TKTNO,tblmstockout.modelcode,tblmstockout.dealer,tblmstockout.outdate", 
					inclusive, 
					exclusive ) );
			}

			return this.DataProvider.CustomQuery(
				typeof(QStockOutDetail),	
				new PagerCondition( 
				string.Format( @"select {0},tblmstockoutdetail.rcard from tblmstockout,tblmstockoutdetail 
												where tblmstockout.status<>'{2}' and tblmstockout.tktno=tblmstockoutdetail.tktno and tblmstockout.seq=tblmstockoutdetail.seq {1}", 
				DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(MaterialStockOut)),
				condition,
				StockStatus.Deleted), 
				"tblmstockout.TKTNO,tblmstockout.modelcode,tblmstockout.dealer,tblmstockout.outdate", 
				inclusive, 
				exclusive ) );

		}
		/// <summary>
		/// ** ��������:	��������ϸ��ѯ,��������
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-08-01
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="startTicketNo">��ʼ��ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="endTicketNo">������ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="startRunningCard">��ʼ��Ʒ���к�,Ϊ�ղ�����ѯ����</param>
		/// <param name="endRunningCard">������Ʒ���к�,Ϊ�ղ�����ѯ����</param>
		/// <param name="modelCode">���ִ��룬��','�ָ���ַ�������ȷ��ѯ</param>
		/// <param name="dealer">�����̣�ģ����ѯ</param>
		/// <param name="startOutDate">��ʼ����ʱ��,Ϊ0������ѯ����</param>
		/// <param name="endOutDate">��������ʱ��,Ϊ0������ѯ����</param>
		/// <returns></returns>
		public int QueryMaterialStockOutCount( 
			string startTicketNo, string endTicketNo, 
			string startRunningCard, string endRunningCard,
			string modelCode, string dealer,
			int startOutDate, int endOutDate)
		{			
			string condition = string.Empty;

			if ( startTicketNo != null && startTicketNo != string.Empty )
			{
				condition += string.Format(" and tblmstockout.TKTNO >= '{0}'", startTicketNo);
			}
			if ( endTicketNo != null && endTicketNo != string.Empty )
			{
				condition += string.Format(" and tblmstockout.TKTNO <= '{0}'", endTicketNo);
			}

			if ( startRunningCard != null && startRunningCard != string.Empty )
			{
				condition += string.Format(" and tblmstockoutdetail.rcard >= '{0}'", startRunningCard);
			}
			if ( endRunningCard != null && endRunningCard != string.Empty )
			{
				condition += string.Format(" and tblmstockoutdetail.rcard <= '{0}'", endRunningCard);
			}

			if ( modelCode != null && modelCode != string.Empty )
			{
				condition += string.Format(" and tblmstockout.modelcode in ({0})", FormatHelper.ProcessQueryValues(modelCode));
			}
			if ( dealer != null && dealer != string.Empty )
			{
				condition += string.Format(" and upper(tblmstockout.dealer) like '{0}%'", dealer.ToUpper());
			}

			if ( startOutDate != 0 )
			{
				condition += string.Format(" and tblmstockout.outdate >= {0}", startOutDate);
			}
			if ( endOutDate != 0 )
			{
				condition += string.Format(" and tblmstockout.outdate <= {0}", endOutDate);
			}
			
			return this.DataProvider.GetCount(
				new SQLCondition( 
				string.Format( @"select count(*) from tblmstockout,tblmstockoutdetail 
												where tblmstockout.tktno=tblmstockoutdetail.tktno and tblmstockout.seq=tblmstockoutdetail.seq {0}", 
				condition)) );
		}
	
		/// <summary>
		/// Added by jessie lee,2005/9/5
		/// </summary>
		/// <param name="startTicketNo">��ʼ��ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="endTicketNo">������ⵥ��,Ϊ�ղ�����ѯ����</param>
		/// <param name="startRunningCard">��ʼ��Ʒ���к�,Ϊ�ղ�����ѯ����</param>
		/// <param name="endRunningCard">������Ʒ���к�,Ϊ�ղ�����ѯ����</param>
		/// <param name="modelCode">���ִ��룬��','�ָ���ַ�������ȷ��ѯ</param>
		/// <param name="dealer">�����̣�ģ����ѯ</param>
		/// <param name="startOutDate">��ʼ����ʱ��,Ϊ0������ѯ����</param>
		/// <param name="endOutDate">��������ʱ��,Ϊ0������ѯ����</param>
		/// <param name="includeDelStatus">�Ƿ����ɾ��״̬</param>
		/// <returns></returns>
		public int QueryMaterialStockOutCount( 
			string startTicketNo, string endTicketNo, 
			string startRunningCard, string endRunningCard,
			string modelCode, string dealer,
			int startOutDate, int endOutDate,
			bool includeDelStatus )
		{			
			string condition = string.Empty;

			if ( startTicketNo != null && startTicketNo != string.Empty )
			{
				condition += string.Format(" and tblmstockout.TKTNO >= '{0}'", startTicketNo);
			}
			if ( endTicketNo != null && endTicketNo != string.Empty )
			{
				condition += string.Format(" and tblmstockout.TKTNO <= '{0}'", endTicketNo);
			}

			if ( startRunningCard != null && startRunningCard != string.Empty )
			{
				condition += string.Format(" and tblmstockoutdetail.rcard >= '{0}'", startRunningCard);
			}
			if ( endRunningCard != null && endRunningCard != string.Empty )
			{
				condition += string.Format(" and tblmstockoutdetail.rcard <= '{0}'", endRunningCard);
			}

			if ( modelCode != null && modelCode != string.Empty )
			{
				condition += string.Format(" and tblmstockout.modelcode in ({0})", FormatHelper.ProcessQueryValues(modelCode));
			}
			if ( dealer != null && dealer != string.Empty )
			{
				condition += string.Format(" and upper(tblmstockout.dealer) like '{0}%'", dealer.ToUpper());
			}

			if ( startOutDate != 0 )
			{
				condition += string.Format(" and tblmstockout.outdate >= {0}", startOutDate);
			}
			if ( endOutDate != 0 )
			{
				condition += string.Format(" and tblmstockout.outdate <= {0}", endOutDate);
			}
			
			if (includeDelStatus)
			{
				return this.DataProvider.GetCount(
					new SQLCondition( 
					string.Format( @"select count(*) from tblmstockout,tblmstockoutdetail 
												where tblmstockout.tktno=tblmstockoutdetail.tktno and tblmstockout.seq=tblmstockoutdetail.seq {0}", 
					condition)) );
			}

			return this.DataProvider.GetCount(
				new SQLCondition( 
				string.Format( @"select count(*) from tblmstockout,tblmstockoutdetail 
												where tblmstockout.status<>'{0}' and tblmstockout.tktno=tblmstockoutdetail.tktno and tblmstockout.seq=tblmstockoutdetail.seq {1}",
				StockStatus.Deleted, 
				condition)) );
		}
	
		#endregion

		#region Stock In/Out Contrast
		/// <summary>
		/// ** ��������:	��������ϸ�ȶ�,��ҳ
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-08-02
		/// *************************************
		/// ** �� ��:       jessie lee         
		/// ** �� ��:		2005/9/5
		/// ** �޸�����:	�ų�״̬Ϊɾ����������
		/// </summary>
		/// <param name="status">״̬�������ѳ�0������δ��1��δ���ѳ�2</param>
		/// <param name="startDate">��ʼʱ��,Ϊ0������ѯ����</param>
		/// <param name="endDate">����ʱ��,Ϊ0������ѯ����</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns></returns>
		public object[] QueryStockContrast( 
			int status, int startDate, int endDate, 
			int inclusive, int exclusive )
		{
			string condition = string.Empty;
			string sql = string.Empty; 

			if ( startDate != 0 )
			{
				condition += string.Format(" and a.mdate >= {0}", startDate);
			}
			if ( endDate != 0 )
			{
				condition += string.Format(" and a.mdate <= {0}", endDate);
			}
			
			// �����ѳ����ڲ�ѯ�����趨��ʱ�������������ʱ���������һ��Ϊ���ݣ�
			// Ȼ��ȥѰ�������ʱ��֮���г�����¼�����ϣ���ʱ����ʱ�����ѯ������ʱ������û�й�ϵ��
			if ( status == 0 )
			{
				sql = string.Format( 
@"select * from 
    (select a.rcard, a.tktno as INTKTNO, b.tktno as OUTTKTNO, 
    a.mdate as INDATE, b.mdate as OUTDATE, a.muser as inuser, b.muser as outuser,
    row_number() over(partition by a.rcard order by a.mdate,a.mtime) rnum 
    from tblmstockin a, tblmstockoutdetail b 
    where  a.status<>'{0}' and a.rcard=b.rcard 
    {1}
    and (a.mdate<b.mdate or (a.mdate=b.mdate and a.mtime<=b.mtime)) )
where rnum = 1",StockStatus.Deleted ,condition);
			}

			// ����δ�����ڲ�ѯ�����趨��ʱ�������������ʱ���������һ��Ϊ���ݣ�
			// Ȼ��ȥѰ�������ʱ��֮��û�г�����¼�����ϣ���ʱ����ʱ�����ѯ������ʱ������û�й�ϵ��
			if ( status == 1 )
			{ 
				sql = string.Format(
@"select * from 
    (select rcard, tktno as INTKTNO, mdate as INDATE, muser as inuser,
    row_number() over(partition by rcard order by mdate,mtime) rnum 
    from tblmstockin a
    where  a.status<>'{0}' and rcard not in ( select distinct b.rcard from tblmstockin a, tblmstockoutdetail b where 
    a.rcard=b.rcard and (a.mdate<b.mdate or (a.mdate=b.mdate and a.mtime<=b.mtime)) {1})
	{1})
where rnum = 1" ,StockStatus.Deleted ,condition);
			}
			
			// δ���ѳ����ڲ�ѯ�����趨��ʱ���������Գ���ʱ���������һ��Ϊ���ݣ�
			// Ȼ��ȥѰ�������ʱ��֮ǰû������¼�����ϣ���ʱ���ʱ�����ѯ������ʱ������û�й�ϵ��
			if ( status == 2 )
			{
				sql = string.Format(
@"select * from 
    (select rcard, tktno as OUTTKTNO, mdate as OUTDATE, muser as OUTUSER,
    row_number() over(partition by rcard order by mdate,mtime) rnum 
    from tblmstockoutdetail a
    where rcard not in ( select distinct a.rcard from tblmstockin a, tblmstockoutdetail b where a.status<>'{0}' and 
    a.rcard=b.rcard and (a.mdate<b.mdate or (a.mdate=b.mdate and a.mtime<=b.mtime)) {1} ) 
	{1})
where rnum = 1" ,StockStatus.Deleted ,condition);
			}

			return this.DataProvider.CustomQuery(
				typeof(QStockContrast),	
				new PagerCondition(sql, "rcard", inclusive, exclusive, true ) );
		}

		/// <summary>
		/// ** ��������:	��������ϸ�ȶ�,��������
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-08-02
		/// ** �� ��:		jessie lee	
		/// ** �� ��:		2005/9/5
		/// </summary>
		/// <param name="status">״̬�������ѳ�0������δ��1��δ���ѳ�2</param>
		/// <param name="startDate">��ʼʱ��,Ϊ0������ѯ����</param>
		/// <param name="endDate">����ʱ��,Ϊ0������ѯ����</param>
		/// <returns></returns>
		public int QueryStockContrastCount( int status, int startDate, int endDate)
		{			
			string condition = string.Empty;
			string sql = string.Empty; 

			if ( startDate != 0 )
			{
				condition += string.Format(" and a.mdate >= {0}", startDate);
			}
			if ( endDate != 0 )
			{
				condition += string.Format(" and a.mdate <= {0}", endDate);
			}

			if ( status == 0 )
			{
				sql = string.Format( 
@"select count(*) from 
    (select row_number() over(partition by a.rcard order by a.mdate,a.mtime) rnum 
    from tblmstockin a, tblmstockoutdetail b 
    where  a.status<>'{0}' and a.rcard=b.rcard 
    {1}
    and (a.mdate<b.mdate or (a.mdate=b.mdate and a.mtime<=b.mtime)) )
where rnum = 1" ,StockStatus.Deleted ,condition);
			}
			
			if ( status == 1 )
			{ 
				sql = string.Format(
@"select count(*) from 
    (select row_number() over(partition by rcard order by mdate,mtime) rnum 
    from tblmstockin a
    where   a.status<>'{0}' and  rcard not in ( select distinct b.rcard from tblmstockin a, tblmstockoutdetail b where 
    a.rcard=b.rcard and (a.mdate<b.mdate or (a.mdate=b.mdate and a.mtime<=b.mtime)) {1} )
    {1})
where rnum = 1" ,StockStatus.Deleted , condition);
			}
			
			if ( status == 2 )
			{
				sql = string.Format(
@"select count(*) from 
    (select row_number() over(partition by rcard order by mdate,mtime) rnum 
    from tblmstockoutdetail a
    where rcard not in ( select distinct a.rcard from tblmstockin a, tblmstockoutdetail b where  a.status<>'{0}' and  
    a.rcard=b.rcard and (a.mdate<b.mdate or (a.mdate=b.mdate and a.mtime<=b.mtime)) {1} )
    {1})
where rnum = 1" ,StockStatus.Deleted ,condition );
			}
			
			return this.DataProvider.GetCount( new SQLCondition(sql) );
		}
		#endregion
	}
}
