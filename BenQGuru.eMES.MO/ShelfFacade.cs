using System;
using System.Collections;
using System.Runtime.Remoting;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Web.Helper;
//using BenQGuru.eMES.WebQuery;
using BenQGuru.eMES.Domain.BaseSetting;

namespace BenQGuru.eMES.MOModel
{
	#region ��Ʒ׷��

	/// <summary>
	/// ��Ʒ׷��
	/// </summary>
	[Serializable, TableMap("TBLSIMULATIONREPORT", "RCARD,MOCODE")]
	public class ItemTracingForShelf : DomainObject
	{
		/// <summary>
		/// ���к�
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, true)]
		public string  RCard;

		/// <summary>
		/// ���к�
		/// </summary>
		[FieldMapAttribute("TCARD", typeof(string), 40, true)]
		public string  TCard;

		/// <summary>
		/// ���к�
		/// </summary>
		[FieldMapAttribute("RCARDSEQ", typeof(string), 40, true)]
		public decimal  RCardSeq;

		/// <summary>
		/// ��Ʒ
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ItemCode;

		/// <summary>
		/// ��Ʒ״̬
		/// </summary>
		[FieldMapAttribute("STATUS", typeof(string), 40, true)]
		public string  ItemStatus;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string  ModelCode;

		/// <summary>
		/// ���ڹ���
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string  OPCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OPTYPE", typeof(string), 40, true)]
		public string  OPType;
    
		/// <summary>
		/// ����;��
		/// </summary>
		[FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
		public string  RouteCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("SEGCODE", typeof(string), 40, true)]
		public string  SegmentCode;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("SSCODE", typeof(string), 40, true)]
		public string  LineCode;

		/// <summary>
		/// ��Դ
		/// </summary>
		[FieldMapAttribute("RESCODE", typeof(string), 40, true)]
		public string  ResCode;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// ʱ��
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, true)]
		public string  MaintainUser;

		/// <summary>
		/// ����������
		/// </summary>
		[FieldMapAttribute("LACTION", typeof(string), 40, true)]
		public string  LastAction;
	}

	#endregion

	public class BurnOutShelf
	{
		public string ShelfNO;
		public TimeSpan TimeSpan;
		public string memo;

	}

	/// <summary>
	/// ATEFacade ��ժҪ˵����
	/// �ļ���:		ATEFacade.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
	/// ��������:	2006-5-24 15:09:22
	/// �޸���:
	/// �޸�����:
	/// �� ��:	
	/// �� ��:	
	/// </summary>
	public class ShelfFacade:MarshalByRefObject
	{
		

		private IDomainDataProvider _domainDataProvider = null;
		private FacadeHelper _helper = null;

		public ShelfFacade(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
			this._helper = new FacadeHelper( DataProvider );
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}


		public ShelfFacade()
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

		#region Shelf
		/// <summary>
		/// 
		/// </summary>
		public Shelf CreateNewShelf()
		{
			return new Shelf();
		}

		public void AddShelf( Shelf shelf)
		{
			this._helper.AddDomainObject( shelf );
		}

		public void UpdateShelf(Shelf shelf)
		{
			this._helper.UpdateDomainObject( shelf );
		}

		public void UpdateShelfMemo(Shelf shelf)
		{
			DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

			DataProvider.CustomExecute(new SQLCondition("update tblshelf set "
				+ "memo ='" + shelf.Memo + "'"
				+ ",ItemCode ='" + shelf.ITEMCODE + "'"
				+ ",MUSER ='" + shelf.MaintainUser + "'"
				+ ",MDATE =" + dbDateTime.DBDate
				+ ",MTIME =" +  dbDateTime.DBTime
				+ " where shelfno='" + shelf.ShelfNO + "'"));
			//this._helper.UpdateDomainObject( shelf );
		}

		public void DeleteShelf(Shelf shelf)
		{
			this._helper.DeleteDomainObject( shelf );
		}

		public void DeleteShelf(Shelf[] shelf)
		{
			this._helper.DeleteDomainObject( shelf );
		}

		public object GetShelf( string shelfNO )
		{
			return this.DataProvider.CustomSearch(typeof(Shelf), new object[]{ shelfNO });
		}

		public object GetShelfAndLockit( string shelfNO )
		{
			object[] objs = this.DataProvider.CustomQuery(typeof(Shelf)
				, new SQLCondition(String.Format("SELECT {0} FROM TBLSHELF WHERE SHELFNO ='{1}' FOR UPDATE NOWAIT",DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shelf)),shelfNO)));

			if(objs == null || (objs != null && objs.Length == 0))
			{
				return null;
			}

			return objs[0];
		}

		/// <summary>
		/// ** ��������:	��ѯShelf��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// 		/// ** �� ��:		2006-5-24 15:09:22
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="shelfNO">ShelfNO��ģ����ѯ</param>
		/// <returns> Shelf���ܼ�¼��</returns>
		public int QueryShelfCount( string shelfNO)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSHELF where 1=1 and SHELFNO like '{0}%' " , shelfNO)));
		}

		/// <summary>
		/// ** ��������:	��ѯShelf��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// 		/// ** �� ��:		2006-5-24 15:09:22
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="shelfNO">ShelfNO��ģ����ѯ</param>
		/// <returns> Shelf���ܼ�¼��</returns>
		public int QueryShelfCount( string shelfNO,string itemCode)
		{
			string itemCondition = String.Empty;

			if(itemCode != null && itemCode != String.Empty)
			{
				itemCondition = " AND ItemCode like '" + itemCode + "%' ";
			}

			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSHELF where 1=1 and SHELFNO like '{0}%' " + itemCondition , shelfNO)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯShelf
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-24 15:09:22
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="shelfNO">ShelfNO��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> Shelf����</returns>
		public object[] QueryShelf( string shelfNO, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(Shelf), new PagerCondition(string.Format("select {0} from TBLSHELF where 1=1 and SHELFNO like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shelf)) , shelfNO), "SHELFNO", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯShelf
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-24 15:09:22
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="shelfNO">ShelfNO��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> Shelf����</returns>
		public object[] QueryShelf( string shelfNO,string itemCode, int inclusive, int exclusive )
		{
			string itemCondition = String.Empty;

			if(itemCode != null && itemCode != String.Empty)
			{
				itemCondition = " AND ItemCode like '" + itemCode + "%' ";
			}
			return this.DataProvider.CustomQuery(typeof(Shelf), new PagerCondition(string.Format("select {0} from TBLSHELF where 1=1 and SHELFNO like '{1}%' " + itemCondition , DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shelf)) , shelfNO), "SHELFNO", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�Shelf
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-24 15:09:22
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>Shelf���ܼ�¼��</returns>
		public object[] GetAllShelf()
		{
			return this.DataProvider.CustomQuery(typeof(Shelf), new SQLCondition(string.Format("select {0} from TBLSHELF order by SHELFNO", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shelf)))));
		}

		#endregion

		#region BurnInOutVolumn
		/// <summary>
		/// 
		/// </summary>
		public BurnInOutVolumn CreateNewBurnInOutVolumn()
		{
			return new BurnInOutVolumn();
		}

		public void AddBurnInOutVolumn( BurnInOutVolumn burnInOutVolumn)
		{
			this._helper.AddDomainObject( burnInOutVolumn );
		}

		public void UpdateBurnInOutVolumn(BurnInOutVolumn burnInOutVolumn)
		{
			this._helper.UpdateDomainObject( burnInOutVolumn );
		}

		public void DeleteBurnInOutVolumn(BurnInOutVolumn burnInOutVolumn)
		{
			this._helper.DeleteDomainObject( burnInOutVolumn );
		}

		public void DeleteBurnInOutVolumn(BurnInOutVolumn[] burnInOutVolumn)
		{
			this._helper.DeleteDomainObject( burnInOutVolumn );
		}

		public object GetBurnInOutVolumn( string pKID )
		{
			return this.DataProvider.CustomSearch(typeof(BurnInOutVolumn), new object[]{ pKID });
		}

		/// <summary>
		/// ** ��������:	��ѯBurnInOutVolumn��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-25 13:50:23
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="pKID">PKID��ģ����ѯ</param>
		/// <returns> BurnInOutVolumn���ܼ�¼��</returns>
		public int QueryBurnInOutVolumnCount( string pKID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLBURNVOLUMN where 1=1 and PKID like '{0}%' " , pKID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯBurnInOutVolumn
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-25 13:50:23
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="pKID">PKID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> BurnInOutVolumn����</returns>
		public object[] QueryBurnInOutVolumn( string pKID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(BurnInOutVolumn), new PagerCondition(string.Format("select {0} from TBLBURNVOLUMN where 1=1 and PKID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(BurnInOutVolumn)) , pKID), "PKID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�BurnInOutVolumn
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-25 13:50:23
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>BurnInOutVolumn���ܼ�¼��</returns>
		public object[] GetAllBurnInOutVolumn()
		{
			return this.DataProvider.CustomQuery(typeof(BurnInOutVolumn), new SQLCondition(string.Format("select {0} from TBLBURNVOLUMN order by PKID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(BurnInOutVolumn)))));
		}

		public void UpdateBurnInOutVolumnBySqlAdd(string muser)
		{
			//Laws Lu,2006/11/13 uniform system collect date
			DBDateTime dbDateTime;
						
			dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

			string sql = string.Format("update TBLBURNVOLUMN set used = used+1,muser = '{0}',mdate={1},mtime={2} where PKID='{3}'",
				muser, 
				dbDateTime.DBDate,
				dbDateTime.DBTime,
				Guid.Empty.ToString());
			this.DataProvider.CustomExecute( new SQLCondition(sql) );
		}

		public void UpdateBurnInOutVolumnBySqlMinus(string muser)
		{
			//Laws Lu,2006/11/13 uniform system collect date
			DBDateTime dbDateTime;
						
			dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

			string sql = string.Format("update TBLBURNVOLUMN set used = used-1,muser = '{0}',mdate={1},mtime={2} where PKID='{3}'",
				muser, 
				dbDateTime.DBDate,
				dbDateTime.DBTime,
				Guid.Empty.ToString());
			this.DataProvider.CustomExecute( new SQLCondition(sql) );
		}
		#endregion

		#region ShelfActionList
		/// <summary>
		/// 
		/// </summary>
		public ShelfActionList CreateNewShelfActionList()
		{
			return new ShelfActionList();
		}

		public void AddShelfActionList( ShelfActionList shelfActionList)
		{
			this._helper.AddDomainObject( shelfActionList );
		}

		public void UpdateShelfActionList(ShelfActionList shelfActionList)
		{
			this._helper.UpdateDomainObject( shelfActionList );
		}

		public void DeleteShelfActionList(ShelfActionList shelfActionList)
		{
			this._helper.DeleteDomainObject( shelfActionList );
		}

		public void DeleteShelfActionList(ShelfActionList[] shelfActionList)
		{
			this._helper.DeleteDomainObject( shelfActionList );
		}

		public object GetShelfActionList( string pKID )
		{
			return this.DataProvider.CustomSearch(typeof(ShelfActionList), new object[]{ pKID });
		}

		public object GetShelfActionListByShelfNo( string shelfnos,string itemCodes,string moCodes,
			string startSn,string endSn,int BurnInBeginDate,int BurnInEndDate,int BurnOutBeginDate,int BurnOutEndDate,
			int inclusive, int exclusive)
		{
			string join = " LEFT ";

			string shelfnoCondition = "";
			if( shelfnos != "" && shelfnos != null )
			{
				shelfnoCondition = string.Format(
					@" and TBLSHELFACTIONLIST.shelfno in ({0})",FormatHelper.ProcessQueryValues( shelfnos ) );
			}

			string itemCondition = "";
			if( itemCodes != "" && itemCodes != null )
			{
				itemCondition = string.Format(
					@" and itemcode in ({0})",FormatHelper.ProcessQueryValues( itemCodes ) );

				join = " INNER ";
			}

			string moCondition = "";
			if( moCodes != "" && moCodes != null )
			{
				moCondition = string.Format(
					@" and mocode in ({0})",FormatHelper.ProcessQueryValues( moCodes ) );

				join = " INNER ";
			}

			string SnCondition = string.Empty;
			SnCondition = FormatHelper.GetRCardRangeSql("rcard",startSn.ToUpper(),endSn.ToUpper());

			if(SnCondition != "")
			{
				join = " INNER ";
			}

			string dateCondition  = string.Empty;
			if(BurnInBeginDate != DefaultDateTime.DefaultToInt)
			{
				dateCondition = FormatHelper.GetDateRangeSql("BIDATE",BurnInBeginDate,BurnInEndDate);
			}
			else if(BurnOutBeginDate != DefaultDateTime.DefaultToInt)
			{
				dateCondition = FormatHelper.GetDateRangeSql("BODATE",BurnOutBeginDate,BurnOutEndDate);
			}

			string sql = string.Format(" select TBLSHELFACTIONLIST.* ,RESCODE,SSCODE from TBLSHELFACTIONLIST {5} join ( "+
								" select distinct shelfno,RESCODE,SSCODE  from tblonwip where   shelfno in"+
								" (select pkid from TBLSHELFACTIONLIST where 1=1 {0}  {4}  ) {1} {2} {3} "+
							" ) b on TBLSHELFACTIONLIST.pkid=b.shelfno where 1=1 {0} {4} ",
				shelfnoCondition,itemCondition,moCondition,SnCondition,dateCondition ,join);

//			string sql = string.Format("select distinct TBLSHELFACTIONLIST.*,TBLONWIP.RESCODE,TBLONWIP.SSCODE from TBLSHELFACTIONLIST "+
//							" INNER JOIN TBLONWIP ON TBLSHELFACTIONLIST.shelfno = pkid where 1=1 {0} {1} {2} {3} {4} ", 
//				shelfnoCondition,itemCondition,moCondition,SnCondition,dateCondition);

			object[] objs = this.DataProvider.CustomQuery(
				typeof(ShelfActionListForQuery),
				new PagerCondition(sql,"TBLSHELFACTIONLIST.shelfno, TBLSHELFACTIONLIST.mdate*1000000+TBLSHELFACTIONLIST.mtime desc",inclusive, exclusive));
				//new PagerCondition(sql,"TBLSHELFACTIONLIST.shelfno, TBLSHELFACTIONLIST.mdate*1000000+TBLSHELFACTIONLIST.mtime desc",inclusive, exclusive));

			if(objs == null)
			{
				return new object[]{};
			}

			return objs;
		}

		public int QueryShelfActionListCountByShelfNo( string shelfnos,string itemCodes,string moCodes,
			string startSn,string endSn,int BurnInBeginDate,int BurnInEndDate,int BurnOutBeginDate,int BurnOutEndDate)
		{
			string join = " LEFT ";
			string shelfnoCondition = "";
			if( shelfnos != "" && shelfnos != null )
			{
				shelfnoCondition = string.Format(
					@" and TBLSHELFACTIONLIST.shelfno in ({0})",FormatHelper.ProcessQueryValues( shelfnos ) );
			}

			string itemCondition = "";
			if( itemCodes != "" && itemCodes != null )
			{
				itemCondition = string.Format(
					@" and itemcode in ({0})",FormatHelper.ProcessQueryValues( itemCodes ) );

				join = " INNER ";
			}

			string moCondition = "";
			if( moCodes != "" && moCodes != null )
			{
				moCondition = string.Format(
					@" and mocode in ({0})",FormatHelper.ProcessQueryValues( moCodes ) );

				join = " INNER ";
			}

			string SnCondition = string.Empty;
			SnCondition = FormatHelper.GetRCardRangeSql("rcard",startSn.ToUpper(),endSn.ToUpper());

			if(SnCondition != "")
			{
				join = " INNER ";
			}

			string dateCondition  = string.Empty;
			if(BurnInBeginDate != DefaultDateTime.DefaultToInt)
			{
				dateCondition = FormatHelper.GetDateRangeSql("BIDATE",BurnInBeginDate,BurnInEndDate);
			}
			else if(BurnOutBeginDate != DefaultDateTime.DefaultToInt)
			{
				dateCondition = FormatHelper.GetDateRangeSql("BODATE",BurnOutBeginDate,BurnOutEndDate);
			}

			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSHELFACTIONLIST  {5} join ( "+
								" select distinct shelfno,RESCODE,SSCODE  from tblonwip where   shelfno in"+
								" (select pkid from TBLSHELFACTIONLIST where 1=1 {0}  {4}  ) {1} {2} {3} "+
							" ) b on TBLSHELFACTIONLIST.pkid=b.shelfno where 1=1 {0} {4} ", 
				shelfnoCondition,itemCondition,moCondition,SnCondition,dateCondition,join)));
		}

		public object GetShelfActionList( string shelfno, string status )
		{
			object[] objs = this.DataProvider.CustomQuery(
				typeof(ShelfActionList), 
				new SQLCondition(string.Format("select {0} from TBLSHELFACTIONLIST where shelfno = '{1}' and status = '{2}' order by BIDATE DESC,BITIME DESC", 
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(ShelfActionList)) , shelfno, status)));
			if(objs!=null)
			{
				return objs[0];
			}

			return null;
		}

		public int ShelfIsUsed( string shelfno)
		{
			int iCount = 0;
			try
			{
				iCount = this.DataProvider.GetCount(
					new SQLCondition(string.Format("select count(*) from TBLSHELFACTIONLIST where shelfno = '{0}' ",shelfno)));
			}
			catch{}

			return iCount;
		}

		/// <summary>
		/// ** ��������:	��ѯShelfActionList��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-29 11:32:52
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="pKID">PKID��ģ����ѯ</param>
		/// <returns> ShelfActionList���ܼ�¼��</returns>
		public int QueryShelfActionListCount( string pKID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSHELFACTIONLIST where 1=1 and PKID like '{0}%' " , pKID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯShelfActionList
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-29 11:32:52
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="pKID">PKID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ShelfActionList����</returns>
		public object[] QueryShelfActionList( string pKID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ShelfActionList), new PagerCondition(string.Format("select {0} from TBLSHELFACTIONLIST where 1=1 and PKID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ShelfActionList)) , pKID), "PKID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ShelfActionList
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-5-29 11:32:52
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ShelfActionList���ܼ�¼��</returns>
		public object[] GetAllShelfActionList()
		{
			return this.DataProvider.CustomQuery(typeof(ShelfActionList), new SQLCondition(string.Format("select {0} from TBLSHELFACTIONLIST order by PKID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ShelfActionList)))));
		}

		/// <summary>
		/// �ѹ��ڵ�
		/// </summary>
		/// <returns></returns>
		public Hashtable GetExpiredShelf()
		{
			object[] objs = 
				this.DataProvider.CustomQuery(
				typeof(ShelfActionList), 
				new SQLCondition(string.Format("select {0} from TBLSHELFACTIONLIST where status ='{1}' ORDER BY SHELFNO", 
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(ShelfActionList)),
				ShelfStatus.BurnIn)));

			object[] objShelfs = 
				this.DataProvider.CustomQuery(
				typeof(Shelf), 
				new SQLCondition(string.Format("select {0} from TBLSHELF where status ='{1}'", 
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shelf)),
				ShelfStatus.BurnIn)));

			if(objs==null) return null;

			Hashtable ht = new Hashtable();

			//Laws Lu,2006/11/13 uniform system collect date
			DBDateTime dbDateTime;
						
			dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

			DateTime workDateTime = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);

			for( int i=0; i<objs.Length; i++ )
			{
				ShelfActionList shelfActionList = objs[i] as ShelfActionList;

				string date = shelfActionList.BurnInDate.ToString();
				int year = Convert.ToInt32( date.Substring(0,4) );
				int month = Convert.ToInt32( date.Substring(4,2) );
				int day = Convert.ToInt32( date.Substring(6,2) );
				string time = shelfActionList.BurnInTime.ToString().PadLeft(6,'0');
				int hour = Convert.ToInt32( time.Substring(0,2) );
				int minute = Convert.ToInt32( time.Substring(2,2) );
				int second = Convert.ToInt32( time.Substring(4,2) );

				DateTime beginDt = new DateTime(year, month, day, hour, minute, second);

				double tphours = Convert.ToDouble(shelfActionList.BurnInTimePeriod);
				/* shelfActionList.BurnInTimePeriod�޸�Ϊֱ�Ӽ�¼ʱ��
				switch( shelfActionList.BurnInTimePeriod )
				{
					case BurnInTP.A:
						tphours = 2;
						break;
					case BurnInTP.B:
						tphours = 4;
						break;
					case BurnInTP.C:
						tphours = 24;
						break;
					default:
						tphours = 0;
						break;
				}
				*/
				
				DateTime endDt = beginDt.AddHours( tphours );

				
				if( DateTime.Compare( workDateTime, endDt )>=0 )
				{
					if(!ht.ContainsKey(shelfActionList.ShelfNO))
					{
							BurnOutShelf sf = new BurnOutShelf();
							sf.ShelfNO = shelfActionList.ShelfNO;

							foreach(object objShelf in objShelfs)
							{
								Shelf sfMain = (Shelf)objShelf;

								if(sfMain.ShelfNO == sf.ShelfNO)
								{
									sf.memo = sfMain.Memo;
									break;
								}
							}

							ht.Add( shelfActionList.ShelfNO, sf);
					}
				}

			}
			return ht;
		}

		/// <summary>
		/// δ���ڵ�
		/// </summary>
		/// <returns></returns>
		public Hashtable GetNotExpiredShelf()
		{
			object[] objs = 
				this.DataProvider.CustomQuery(
				typeof(ShelfActionList), 
				new SQLCondition(string.Format("select {0} from TBLSHELFACTIONLIST where status ='{1}' ORDER BY BIDATE,BITIME", 
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(ShelfActionList)),
				ShelfStatus.BurnIn)));

			object[] objShelfs = 
				this.DataProvider.CustomQuery(
				typeof(Shelf), 
				new SQLCondition(string.Format("select {0} from TBLSHELF where status ='{1}'", 
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shelf)),
				ShelfStatus.BurnIn)));

			if(objs==null) return null;

			Hashtable ht = new Hashtable();

			//Laws Lu,2006/11/13 uniform system collect date
			DBDateTime dbDateTime;
						
			dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

			DateTime workDateTime = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);

			for( int i=0; i<objs.Length; i++ )
			{
				ShelfActionList shelfActionList = objs[i] as ShelfActionList;

				string date = shelfActionList.BurnInDate.ToString();
				int year = Convert.ToInt32( date.Substring(0,4) );
				int month = Convert.ToInt32( date.Substring(4,2) );
				int day = Convert.ToInt32( date.Substring(6,2) );
				string time = shelfActionList.BurnInTime.ToString().PadLeft(6,'0');
				int hour = Convert.ToInt32( time.Substring(0,2) );
				int minute = Convert.ToInt32( time.Substring(2,2) );
				int second = Convert.ToInt32( time.Substring(4,2) );

				DateTime beginDt = new DateTime(year, month, day, hour, minute, second);

				double tphours = Convert.ToDouble(shelfActionList.BurnInTimePeriod);
				/* shelfActionList.BurnInTimePeriod�޸�Ϊֱ�Ӽ�¼ʱ��
				switch( shelfActionList.BurnInTimePeriod )
				{
					case BurnInTP.A:
						tphours = 2;
						break;
					case BurnInTP.B:
						tphours = 4;
						break;
					case BurnInTP.C:
						tphours = 24;
						break;
					default:
						tphours = 0;
						break;
				}
				*/

				DateTime endDt = beginDt.AddHours( tphours );

				if( DateTime.Compare( workDateTime , endDt )<0 )
				{
					TimeSpan span = endDt - workDateTime;
					if(!ht.ContainsKey(shelfActionList.ShelfNO))
					{
						BurnOutShelf sf = new BurnOutShelf();
						sf.ShelfNO = shelfActionList.ShelfNO;
						sf.TimeSpan = span;
						foreach(object objShelf in objShelfs)
						{
							Shelf sfMain = (Shelf)objShelf;

							if(sfMain.ShelfNO == sf.ShelfNO)
							{
								sf.memo = sfMain.Memo;
								break;
							}
						}

						ht.Add( shelfActionList.ShelfNO, sf );
					}
				}

			}
			return ht;
		}

		
	
		#endregion

		public string[] GetShelf2RCard( string shelfpk)
		{
			string sql = string.Format(
				@"select {0}
				from tblsimulation
				where shelfno in (select pkid
									from tblshelfactionlist
									where pkid = $PKID )",
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(Simulation)));
			object[] objs = this.DataProvider.CustomQuery( 
				typeof(Simulation), 
				new SQLParamCondition(sql, new SQLParameter[]{ new SQLParameter("PKID",typeof(string), shelfpk) }));

			string[] rcards = null;
			if( objs!=null )
			{
				rcards = new string[objs.Length];	
				for(int i=0; i<objs.Length; i++)
				{
					rcards[i] = (objs[i] as Simulation).RunningCard;
				}
			}

            return rcards;
		}

		public object[] QueryShelf2RCard( string shelfnos,string itemCodes,string moCodes,
			string startSn,string endSn,int BurnInBeginDate,int BurnInEndDate,int BurnOutBeginDate,int BurnOutEndDate, 
			int inclusive, int exclusive )
		{
			#region ����
			/*
			string shelfnoCondition = "";
			if( shelfnos != "" && shelfnos != null )
			{
				shelfnoCondition = string.Format(
					@" and TBLSHELFACTIONLIST.shelfno in ({0})",FormatHelper.ProcessQueryValues( shelfnos ) );
			}

			
			string itemCondition = "";
			if( itemCodes != "" && itemCodes != null )
			{
				itemCondition = string.Format(
					@" and itemcode in ({0})",FormatHelper.ProcessQueryValues( itemCodes ) );
			}

			string moCondition = "";
			if( moCodes != "" && moCodes != null )
			{
				moCondition = string.Format(
					@" and mocode in ({0})",FormatHelper.ProcessQueryValues( moCodes ) );
			}

			string SnCondition = string.Empty;
			SnCondition = FormatHelper.GetRCardRangeSql("rcard",startSn.ToUpper(),endSn.ToUpper());

			string dateCondition  = string.Empty;
			if(BurnInBeginDate != DefaultDateTime.DefaultToInt)
			{
				dateCondition = FormatHelper.GetDateRangeSql("BIDATE",BurnInBeginDate,BurnInEndDate);
			}
			else if(BurnOutBeginDate != DefaultDateTime.DefaultToInt)
			{
				dateCondition = FormatHelper.GetDateRangeSql("BODATE",BurnOutBeginDate,BurnOutEndDate);
			}

			string sql = string.Format(" select  RCARD,MOCODE,ITEMCODE,ACTIONRESULT,TBLSHELFACTIONLIST.SHELFNO EATTRIBUTE1   "+
				" from tblonwip ,TBLSHELFACTIONLIST  where   tblonwip.shelfno = TBLSHELFACTIONLIST.pkid "+
				"  {0} {1} {2} {3} {4}  ",
//					,DomainObjectUtility.GetDomainObjectFieldsString(typeof(OnWIP)),
					shelfnoCondition,itemCondition,moCondition,SnCondition,dateCondition);


			object[] objs = this.DataProvider.CustomQuery(
				typeof(OnWIP),
				new PagerCondition(sql,"rcard",inclusive, exclusive));
			

			if(objs == null)
			{
				return new object[]{};
			}

			return objs;

*/
			#endregion


			string sql = string.Format("select TBLSIMULATIONREPORT.modelcode, TBLSIMULATIONREPORT.mocode, TBLSIMULATIONREPORT.status, "+
						" TBLSIMULATIONREPORT.itemcode,  TBLSIMULATIONREPORT.rcard,TBLSHELFACTIONLIST.SHELFNO as muser  "+
						" from tblsimulationreport,TBLSHELFACTIONLIST, tblonwip where "+
						" tblonwip.shelfno = TBLSHELFACTIONLIST.pkid and tblonwip.rcard= TBLSIMULATIONREPORT.rcard "+
						" and tblonwip.mocode = TBLSIMULATIONREPORT.mocode ",
						DomainObjectUtility.GetDomainObjectFieldsString(typeof(ItemTracingForShelf)) ) ;

			string SnCondition = string.Empty;
			SnCondition = FormatHelper.GetRCardRangeSql("tblsimulationreport.rcard",startSn.ToUpper(),endSn.ToUpper());
			sql += SnCondition;

			if( shelfnos != "" && shelfnos != null )
			{
				sql += string.Format(
					@" and TBLSHELFACTIONLIST.shelfno in ({0})",FormatHelper.ProcessQueryValues( shelfnos ) );
			}

			
			string itemCondition = "";
			if( itemCodes != "" && itemCodes != null )
			{
				sql += string.Format(
					@" and tblsimulationreport.itemcode in ({0})",FormatHelper.ProcessQueryValues( itemCodes ) );
			}

			string moCondition = "";
			if( moCodes != "" && moCodes != null )
			{
				sql += string.Format(
					@" and tblsimulationreport.mocode in ({0})",FormatHelper.ProcessQueryValues( moCodes ) );
			}

			string dateCondition  = string.Empty;
			if(BurnInBeginDate != DefaultDateTime.DefaultToInt)
			{
				sql += FormatHelper.GetDateRangeSql("TBLSHELFACTIONLIST.BIDATE",BurnInBeginDate,BurnInEndDate);
			}
			else if(BurnOutBeginDate != DefaultDateTime.DefaultToInt)
			{
				sql += FormatHelper.GetDateRangeSql("TBLSHELFACTIONLIST.BODATE",BurnOutBeginDate,BurnOutEndDate);
			}

			object[] objs = this.DataProvider.CustomQuery( typeof(ItemTracingForShelf) , new PagerCondition( sql , "tblsimulationreport.rcard",inclusive,exclusive)) ;

			if(objs == null)
			{
				return null ;
			}

			BenQGuru.eMES.BaseSetting.BaseModelFacade bmFacade = new BenQGuru.eMES.BaseSetting.BaseModelFacade( this.DataProvider ) ;


			for(int i=0;i<objs.Length;i++)
			{
				ItemTracingForShelf objIT = objs[i] as ItemTracingForShelf;
				//added by jessie lee for CS0041,2005/10/11,P4.10
				//����ǰ���Ѿ��õ���rcard

				//added by jessie lee, 2005/12/6, for CS179
				/*��Ʒ׷�ݹ���
						ά�޵��������ڲ�Ʒ׷�ݵ���Ϣ�У��������Ʒ״̬�Ĳ���Ʒ״̬��ϸ��Ϊ�����ޡ����ޡ�ά���С����ϡ���⡢ά����ɡ������в���Ʒ�Ĳ�Ʒ״̬����������7��״̬�����档

						�����ǲ�Ʒ״̬������������
						��Ʒ״̬��
						1. ��Ʒ��ֻҪ�ɼ�Ϊ��Ʒ����Ʒ��״̬��Ϊ��Ʒ������Ʒ/����Ʒ�ɼ�Ϊ��Ʒ����ͨ���ɼ�Ϊ��Ʒ�Լ�����Ĭ��Ϊ��Ʒ�Ĳɼ�
						2. ���ޣ��Ѳɼ�Ϊ����������Ʒ/����Ʒ�ɼ�Ϊ����Ʒ���췢��Ϊ����Ʒ�������õ��Ĳ���,����δ������ȷ�ϵ�״̬
						3. ���ޣ��ѱ�����ȷ�ϣ���δ����ά�޵�״̬
						4. ά���У�����ά�޵�״̬
						5. ���ϣ���Ʒ��ָ��Ϊ����
						6. ��⣺��Ʒ�����
						7. ά����ɣ���Ʒ�ѱ�ά����ɣ���δ�ص���������
						8. ����Ʒ���ѱ��������˵Ĳ�Ʒ�������е����в�Ʒ��״̬��������Ʒ״̬
					*/
				//				string tsSQL = string.Format("select {0} from tblts where rcard = '{1}' and rcardseq = {2} and mocode = '{3}' and tsstatus <> '{4}'",
				//					DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS)),
				//					objIT.RCard,
				//					objIT.RCardSeq,
				//					objIT.MOCode,
				//					TSStatus.TSStatus_New);

				string tsSQL = string.Format(@"select scrapcause, frmtime, frmdate, frmuser, shifttypecode, shiftcode,
								tpcode, scardseq, frmmemo, tcardseq, tffullpath, shiftday, refrescode,
								transstatus, refopcode, copcode, refroutecode, crescode, refmocode,
								cardtype, rrcard, frminputtype, tsmemo, tstime, confirmtime, tsuser,
								confirmdate, tsdate, confirmuser, tsstatus, scard, tsid, tcard,
								tstimes, eattribute1, rcardseq, tsrescode, modelcode, frmrescode,
								itemcode, frmopcode, mtime, frmsscode, mdate, frmsegcode, 
								CASE tsstatus
									WHEN 'tsstatus_ts'
										THEN (SELECT muser
												FROM tbltserrorcause
												WHERE tbltserrorcause.tsid = a.tsid AND ROWNUM = 1)
									WHEN 'tsstatus_reflow'
										THEN a.tsuser
									WHEN 'tsstatus_complete'
										THEN a.tsuser
									WHEN 'tsstatus_confirm'
										THEN a.confirmuser
									WHEN 'tsstatus_split'
										THEN a.tsuser
									WHEN 'tsstatus_scrap'
										THEN a.tsuser
									ELSE muser
								END AS muser,
								frmroutecode, rcard, mocode from tblts a
								where rcard = '{0}' and rcardseq = {1} and mocode = '{2}' and tsstatus <> '{3}'",
					objIT.RCard,
					objIT.RCardSeq,
					objIT.MOCode,
					TSStatus.TSStatus_New);

				Object[] tsObjs = this.DataProvider.CustomQuery( 
					typeof(BenQGuru.eMES.Domain.TS.TS),
					new PagerCondition(tsSQL,inclusive,exclusive));

				if(tsObjs!=null)
				{
					BenQGuru.eMES.Domain.TS.TS tsObj = tsObjs[0] as BenQGuru.eMES.Domain.TS.TS;
					objIT.ResCode = tsObj.ConfirmResourceCode;
					objIT.LastAction = "TS";
					objIT.MaintainDate = tsObj.MaintainDate ;
					objIT.MaintainTime = tsObj.MaintainTime ;

					//��¼���滻
					//������tblts ��confirmuser
					//ά����ɺ�ά����tblerrorcause ��muser
					//���ͱ�����tblts��tblts ��tsuser
					objIT.MaintainUser = tsObj.MaintainUser ;

					objIT.OPCode = tsObj.ConfirmOPCode ;
					objIT.OPType = "TS" ;
					objIT.RouteCode = string.Empty ;
					objIT.LineCode = string.Empty ;
					objIT.SegmentCode = string.Empty ;
					objIT.ItemStatus = tsObj.TSStatus;
					continue;
				}
				//added end

				if( string.Compare( objIT.ItemStatus,ProductStatus.NG,true)==0)
				{
					objIT.ItemStatus = TSStatus.TSStatus_New;
				}

				object op = bmFacade.GetOperation( objIT.OPCode) ;
				if(op== null)
				{
					objIT.OPType = string.Empty ;
				}
				else
				{
                
					if( ((Operation)op).OPControl[(int)BenQGuru.eMES.BaseSetting.OperationList.OutsideRoute] == '1' )
					{
						objIT.OPType = ((Operation)op).OPControl ;
					}
					else
					{
						object ir2o = this.GetItemRoute2Operation(
							objIT.ItemCode,
							objIT.RouteCode,
							objIT.OPCode
							) ;

						if( ir2o == null )
						{
							objIT.OPType = string.Empty ;
						}
						else
						{
							objIT.OPType = ((Operation)op).OPControl ;
						}
					}
				}
			}
            
			return objs ;
		}

		/// <summary>
		/// form ItemFacade
		/// </summary>
		/// <param name="itemCode"></param>
		/// <param name="routeCode"></param>
		/// <param name="opCode"></param>
		/// <returns></returns>
		public object GetItemRoute2Operation(string itemCode,string routeCode,string opCode)
		{
			string selectSql = "select "+DomainObjectUtility.GetDomainObjectFieldsString(typeof(ItemRoute2OP))+" from tblitemroute2op where itemcode=$itemcode and routecode=$routecode and opcode =$opcode" + GlobalVariables.CurrentOrganizations.GetSQLCondition();
			object[] objs = this.DataProvider.CustomQuery(typeof(ItemRoute2OP),new SQLParamCondition(selectSql,new SQLParameter[] { new SQLParameter("itemcode",typeof(string),itemCode),new SQLParameter("routecode",typeof(string),routeCode),
																																	  new SQLParameter("opcode",typeof(string), opCode)}));
			if(objs == null)
			{
				ExceptionManager.Raise(this.GetType().BaseType,"$Error_ItemRoute_NotExisted");
			}
			return objs[0];
		}

		public object[] QueryShelf2RCard( string shelfpk, int inclusive, int exclusive )
		{

			/*
			string sql = string.Format(
				@"select {0}
				from tblonwip
				where shelfno in (select pkid
									from tblshelfactionlist
									where pkid = $PKID )",
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(OnWIP)));
			return this.DataProvider.CustomQuery( 
				typeof(OnWIP), 
				new PagerParamCondition(
				sql, 
				inclusive, 
				exclusive, 
				new SQLParameter[]{ new SQLParameter("PKID",typeof(string), shelfpk) }
				) );
*/

			string sql = string.Format("select TBLSIMULATIONREPORT.modelcode, TBLSIMULATIONREPORT.mocode, TBLSIMULATIONREPORT.status, "+
				" TBLSIMULATIONREPORT.itemcode ,TBLSIMULATIONREPORT.rcard,TBLSHELFACTIONLIST.SHELFNO as muser  "+
				" from tblsimulationreport,TBLSHELFACTIONLIST, tblonwip where "+
				" tblonwip.shelfno = TBLSHELFACTIONLIST.pkid and tblonwip.rcard= TBLSIMULATIONREPORT.rcard "+
				" and tblonwip.mocode = TBLSIMULATIONREPORT.mocode",
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(ItemTracingForShelf)) ) ;

//			string SnCondition = string.Empty;
//			SnCondition = FormatHelper.GetRCardRangeSql("tblsimulationreport.rcard",startSn.ToUpper(),endSn.ToUpper());
//			sql += SnCondition;
			
			sql += string.Format(" and TBLSHELFACTIONLIST.pkid ='{0}'",shelfpk);

			object[] objs = this.DataProvider.CustomQuery( typeof(ItemTracingForShelf) , new PagerCondition( sql , "tblsimulationreport.rcard",inclusive,exclusive)) ;

			if(objs == null)
			{
				return null ;
			}

			BenQGuru.eMES.BaseSetting.BaseModelFacade bmFacade = new BenQGuru.eMES.BaseSetting.BaseModelFacade( this.DataProvider ) ;


			for(int i=0;i<objs.Length;i++)
			{
				ItemTracingForShelf objIT = objs[i] as ItemTracingForShelf;
				//added by jessie lee for CS0041,2005/10/11,P4.10
				//����ǰ���Ѿ��õ���rcard

				//added by jessie lee, 2005/12/6, for CS179
				/*��Ʒ׷�ݹ���
						ά�޵��������ڲ�Ʒ׷�ݵ���Ϣ�У��������Ʒ״̬�Ĳ���Ʒ״̬��ϸ��Ϊ�����ޡ����ޡ�ά���С����ϡ���⡢ά����ɡ������в���Ʒ�Ĳ�Ʒ״̬����������7��״̬�����档

						�����ǲ�Ʒ״̬������������
						��Ʒ״̬��
						1. ��Ʒ��ֻҪ�ɼ�Ϊ��Ʒ����Ʒ��״̬��Ϊ��Ʒ������Ʒ/����Ʒ�ɼ�Ϊ��Ʒ����ͨ���ɼ�Ϊ��Ʒ�Լ�����Ĭ��Ϊ��Ʒ�Ĳɼ�
						2. ���ޣ��Ѳɼ�Ϊ����������Ʒ/����Ʒ�ɼ�Ϊ����Ʒ���췢��Ϊ����Ʒ�������õ��Ĳ���,����δ������ȷ�ϵ�״̬
						3. ���ޣ��ѱ�����ȷ�ϣ���δ����ά�޵�״̬
						4. ά���У�����ά�޵�״̬
						5. ���ϣ���Ʒ��ָ��Ϊ����
						6. ��⣺��Ʒ�����
						7. ά����ɣ���Ʒ�ѱ�ά����ɣ���δ�ص���������
						8. ����Ʒ���ѱ��������˵Ĳ�Ʒ�������е����в�Ʒ��״̬��������Ʒ״̬
					*/
				//				string tsSQL = string.Format("select {0} from tblts where rcard = '{1}' and rcardseq = {2} and mocode = '{3}' and tsstatus <> '{4}'",
				//					DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS)),
				//					objIT.RCard,
				//					objIT.RCardSeq,
				//					objIT.MOCode,
				//					TSStatus.TSStatus_New);

				string tsSQL = string.Format(@"select scrapcause, frmtime, frmdate, frmuser, shifttypecode, shiftcode,
								tpcode, scardseq, frmmemo, tcardseq, tffullpath, shiftday, refrescode,
								transstatus, refopcode, copcode, refroutecode, crescode, refmocode,
								cardtype, rrcard, frminputtype, tsmemo, tstime, confirmtime, tsuser,
								confirmdate, tsdate, confirmuser, tsstatus, scard, tsid, tcard,
								tstimes, eattribute1, rcardseq, tsrescode, modelcode, frmrescode,
								itemcode, frmopcode, mtime, frmsscode, mdate, frmsegcode, 
								CASE tsstatus
									WHEN 'tsstatus_ts'
										THEN (SELECT muser
												FROM tbltserrorcause
												WHERE tbltserrorcause.tsid = a.tsid AND ROWNUM = 1)
									WHEN 'tsstatus_reflow'
										THEN a.tsuser
									WHEN 'tsstatus_complete'
										THEN a.tsuser
									WHEN 'tsstatus_confirm'
										THEN a.confirmuser
									WHEN 'tsstatus_split'
										THEN a.tsuser
									WHEN 'tsstatus_scrap'
										THEN a.tsuser
									ELSE muser
								END AS muser,
								frmroutecode, rcard, mocode from tblts a
								where rcard = '{0}' and rcardseq = {1} and mocode = '{2}' and tsstatus <> '{3}'",
					objIT.RCard,
					objIT.RCardSeq,
					objIT.MOCode,
					TSStatus.TSStatus_New);

				Object[] tsObjs = this.DataProvider.CustomQuery( 
					typeof(BenQGuru.eMES.Domain.TS.TS),
					new PagerCondition(tsSQL,inclusive,exclusive));

				if(tsObjs!=null)
				{
					BenQGuru.eMES.Domain.TS.TS tsObj = tsObjs[0] as BenQGuru.eMES.Domain.TS.TS;
					objIT.ResCode = tsObj.ConfirmResourceCode;
					objIT.LastAction = "TS";
					objIT.MaintainDate = tsObj.MaintainDate ;
					objIT.MaintainTime = tsObj.MaintainTime ;

					//��¼���滻
					//������tblts ��confirmuser
					//ά����ɺ�ά����tblerrorcause ��muser
					//���ͱ�����tblts��tblts ��tsuser
					objIT.MaintainUser = tsObj.MaintainUser ;

					objIT.OPCode = tsObj.ConfirmOPCode ;
					objIT.OPType = "TS" ;
					objIT.RouteCode = string.Empty ;
					objIT.LineCode = string.Empty ;
					objIT.SegmentCode = string.Empty ;
					objIT.ItemStatus = tsObj.TSStatus;
					continue;
				}
				//added end

				if( string.Compare( objIT.ItemStatus,ProductStatus.NG,true)==0)
				{
					objIT.ItemStatus = TSStatus.TSStatus_New;
				}

				object op = bmFacade.GetOperation( objIT.OPCode) ;
				if(op== null)
				{
					objIT.OPType = string.Empty ;
				}
				else
				{
                
					if( ((Operation)op).OPControl[(int)BenQGuru.eMES.BaseSetting.OperationList.OutsideRoute] == '1' )
					{
						objIT.OPType = ((Operation)op).OPControl ;
					}
					else
					{
						object ir2o = this.GetItemRoute2Operation(
							objIT.ItemCode,
							objIT.RouteCode,
							objIT.OPCode
							) ;

						if( ir2o == null )
						{
							objIT.OPType = string.Empty ;
						}
						else
						{
							objIT.OPType = ((Operation)op).OPControl ;
						}
					}
				}
			}
            
			return objs ;
		}

		public int QueryShelf2RCardCount( string shelfpk)
		{
			string sql = string.Format(
				@"select count(*)
				from tblonwip
				where shelfno in (select pkid
									from tblshelfactionlist
									where pkid = $PKID )");
				return this.DataProvider.GetCount( new SQLParamCondition(sql, 
					new SQLParameter[]{ new SQLParameter("PKID",typeof(string), shelfpk) })) ;
		}

	}
}

