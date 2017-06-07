using System;
using System.Runtime.Remoting;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.SolderPaste;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.SMT
{
	/// <summary>
	/// SolderPasteFacade ��ժҪ˵����
	/// �ļ���:		SolderPasteFacade.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		ER/Studio Basic Macro Code Generation  Created by ****
	/// ��������:	2006-7-17 10:24:42
	/// �޸���:
	/// �޸�����:
	/// �� ��:	
	/// �� ��:	
	/// </summary>
	public class SolderPasteFacade:MarshalByRefObject
	{
		private IDomainDataProvider _domainDataProvider = null;
		private FacadeHelper _helper = null;

		public SolderPasteFacade(IDomainDataProvider domainDataProvider)
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

		#region SolderPaste
		/// <summary>
		/// 
		/// </summary>
		public SolderPaste CreateNewSolderPaste()
		{
			return new SolderPaste();
		}

		public void AddSolderPaste( SolderPaste solderPaste)
		{
			this._helper.AddDomainObject( solderPaste );
		}

		public void UpdateSolderPaste(SolderPaste solderPaste)
		{
			this._helper.UpdateDomainObject( solderPaste );
		}

		public void DeleteSolderPaste(SolderPaste solderPaste)
		{
			//Laws Lu,2006/09/07 �޸�	�Ѿ�ʹ�ò�����ɾ��
			bool bAllowDel = true;
				object objSPPro = this.GetSOLDERPASTEPROBySPPID(solderPaste.SolderPasteID);

			if(objSPPro != null)
			{
				bAllowDel = false;
			}

			if(bAllowDel)
			{
				this._helper.DeleteDomainObject( solderPaste );
			}
			else
			{
				throw new Exception("$CS_SOLDERPASTE_ALREADY_USING");
			}
		}

		public void DeleteSolderPaste(SolderPaste[] solderPaste)
		{
			//Laws Lu,2006/09/07 �޸�	�Ѿ�ʹ�ò�����ɾ��
			bool bAllowDel = true;
			foreach(SolderPaste sp in solderPaste)
			{
				object objSPPro = this.GetSOLDERPASTEPROBySPPID(sp.SolderPasteID);

				if(objSPPro != null)
				{
					bAllowDel = false;
					break;
				}
			}
			if(bAllowDel)
			{
				this._helper.DeleteDomainObject( solderPaste );
			}
			else
			{
				throw new Exception("$CS_SOLDERPASTE_ALREADY_USING");
			}
		}

		public object GetSolderPaste( string solderPasteID )
		{
			return this.DataProvider.CustomSearch(typeof(SolderPaste), new object[]{ solderPasteID });
		}

		public object GetUnUseSolderPaste( string solderPasteID )
		{
			object[] objs =  DataProvider.CustomQuery(typeof(SolderPaste),
				new SQLCondition(
				String.Format("SELECT {0}  FROM tblsolderpaste WHERE SPID = '{1}' and used = 0 " 
				+ "AND STATUS IN ('" + Web.Helper.SolderPasteStatus.Normal 
				+ "','" + Web.Helper.SolderPasteStatus.Reflow + "')"
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste))
				,solderPasteID)));

			if(objs != null && objs.Length > 0)
			{
				return objs[0];
			}
			else
			{
				return null;
			}
			
		}

		/// <summary>
		/// ** ��������:	��ѯSolderPaste��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-17 15:45:18
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solderPasteID">SolderPasteID��ģ����ѯ</param>
		/// <returns> SolderPaste���ܼ�¼��</returns>
		public int QuerySolderPasteCount( string solderPasteID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSOLDERPASTE where 1=1 and SPID like '{0}%' " , solderPasteID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSolderPaste
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-17 15:45:18
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="solderPasteID">SolderPasteID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SolderPaste����</returns>
		public object[] QuerySolderPaste( string solderPasteID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(SolderPaste), new PagerCondition(string.Format("select {0} from TBLSOLDERPASTE where 1=1 and SPID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste)) , solderPasteID), "SPID", inclusive, exclusive));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="solderPasteID"></param>
		/// <returns></returns>
		public int QuerySolderPasteCount( string solderPasteID, string partNO, string lotNO, string spstatus, int startdate, int endate )
		{
			string condition = string.Empty;
			if(solderPasteID!=null && solderPasteID.Length>0)
			{
				condition += string.Format(" and SPID like '{0}%'  ", solderPasteID);
			}
			if(partNO!=null && partNO.Length>0)
			{
				condition += string.Format(" and partno like '{0}%'  ", partNO);
			}
			if(lotNO!=null && lotNO.Length>0)
			{
				condition += string.Format(" and lotNO like '{0}%'  ", lotNO);
			}
			if(spstatus!=null && spstatus.Length>0)
			{
				condition += string.Format(" and status = '{0}'  ", spstatus);
			}
			condition += FormatHelper.GetDateRangeSql("prodate", startdate, endate);
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSOLDERPASTE where 1=1 {0}" , condition)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="solderPasteID"></param>
		/// <param name="inclusive"></param>
		/// <param name="exclusive"></param>
		/// <returns></returns>
		public object[] QuerySolderPaste( string solderPasteID, string partNO, string lotNO, string spstatus, int startdate, int endate, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if(solderPasteID!=null && solderPasteID.Length>0)
			{
				condition += string.Format(" and SPID like '{0}%'  ", solderPasteID);
			}
			if(partNO!=null && partNO.Length>0)
			{
				condition += string.Format(" and partno like '{0}%'  ", partNO);
			}
			if(lotNO!=null && lotNO.Length>0)
			{
				condition += string.Format(" and lotNO like '{0}%'  ", lotNO);
			}
			if(spstatus!=null && spstatus.Length>0)
			{
				condition += string.Format(" and status = '{0}'  ", spstatus);
			}

			condition += FormatHelper.GetDateRangeSql("prodate", startdate, endate);
			return this.DataProvider.CustomQuery(typeof(SolderPaste), new PagerCondition(string.Format("select {0} from TBLSOLDERPASTE where 1=1 {1}", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste)) , condition), "SPID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�SolderPaste
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-17 15:45:18
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>SolderPaste���ܼ�¼��</returns>
		public object[] GetAllSolderPaste()
		{
			return this.DataProvider.CustomQuery(typeof(SolderPaste), new SQLCondition(string.Format("select {0} from TBLSOLDERPASTE order by SPID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste)))));
		}


		#endregion

		#region SolderPaste2Item
		/// <summary>
		/// 
		/// </summary>
		public SolderPaste2Item CreateNewSolderPaste2Item()
		{
			return new SolderPaste2Item();
		}

		public void AddSolderPaste2ItemWithTransaction(SolderPaste2Item[] solderPaste2Item)
		{
			this._helper.AddDomainObject( solderPaste2Item );
		}

		public void AddSolderPaste2Item( SolderPaste2Item solderPaste2Item)
		{
			this._helper.AddDomainObject( solderPaste2Item );
		}

		public void UpdateSolderPaste2Item(SolderPaste2Item solderPaste2Item)
		{
			this._helper.UpdateDomainObject( solderPaste2Item );
		}

		public void DeleteSolderPaste2Item(SolderPaste2Item solderPaste2Item)
		{
			this._helper.DeleteDomainObject( solderPaste2Item );
		}

		public void DeleteSolderPaste2Item(SolderPaste2Item[] solderPaste2Item)
		{
			this._helper.DeleteDomainObject( solderPaste2Item );
		}

		public object GetSolderPaste2Item( string itemCode )
		{
			return this.DataProvider.CustomSearch(typeof(SolderPaste2Item), new object[]{ itemCode });
		}

		/// <summary>
		/// ** ��������:	��ѯSolderPaste2Item��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-18 10:40:41
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <returns> SolderPaste2Item���ܼ�¼��</returns>
		public int QuerySolderPaste2ItemCount( string itemCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSOLDERPASTE2ITEM where 1=1 and ITEMCODE like '{0}%' " , itemCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSolderPaste2Item
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-18 10:40:41
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SolderPaste2Item����</returns>
		public object[] QuerySolderPaste2Item( string itemCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(SolderPaste2Item), new PagerCondition(string.Format("select {0} from TBLSOLDERPASTE2ITEM where 1=1 and ITEMCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste2Item)) , itemCode), "SPTYPE,ITEMCODE", inclusive, exclusive));
		}

		public int QuerySolderPaste2ItemCount( string itemCode, string spType)
		{
			string condition = string.Empty;
			if( itemCode!=null && itemCode.Length>0 )
			{
				condition += string.Format(" and ITEMCODE like '{0}%' ", itemCode);
			}

			if( spType!=null && spType.Length>0 )
			{
				condition += string.Format(" and sptype = '{0}' ", spType);
			}
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSOLDERPASTE2ITEM where 1=1 {0} " , condition)));
		}

		public object[] QuerySolderPaste2Item( string itemCode, string spType, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if( itemCode!=null && itemCode.Length>0 )
			{
				condition += string.Format(" and ITEMCODE like '{0}%' ", itemCode);
			}

			if( spType!=null && spType.Length>0 )
			{
				condition += string.Format(" and sptype = '{0}' ", spType);
			}
			return this.DataProvider.CustomQuery(typeof(SolderPaste2Item), new PagerCondition(string.Format("select {0} from TBLSOLDERPASTE2ITEM where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste2Item)) , condition), "ITEMCODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�SolderPaste2Item
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-18 10:40:41
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>SolderPaste2Item���ܼ�¼��</returns>
		public object[] GetAllSolderPaste2Item()
		{
			return this.DataProvider.CustomQuery(typeof(SolderPaste2Item), new SQLCondition(string.Format("select {0} from TBLSOLDERPASTE2ITEM order by ITEMCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste2Item)))));
		}


		#endregion

		#region SolderPasteControl
		/// <summary>
		/// 
		/// </summary>
		public SolderPasteControl CreateNewSolderPasteControl()
		{
			return new SolderPasteControl();
		}

		public void AddSolderPasteControl( SolderPasteControl solderPasteControl)
		{
			this._helper.AddDomainObject( solderPasteControl );
		}

		public void UpdateSolderPasteControl(SolderPasteControl solderPasteControl)
		{
			this._helper.UpdateDomainObject( solderPasteControl );
		}

		public void DeleteSolderPasteControl(SolderPasteControl solderPasteControl)
		{
			this._helper.DeleteDomainObject( solderPasteControl, 
				new ICheck[]{ new DeleteAssociateCheck( solderPasteControl,
								this.DataProvider, 
								new Type[]{
											  typeof(SolderPaste)	})} );
		}

		public void DeleteSolderPasteControl(SolderPasteControl[] solderPasteControl)
		{
			this._helper.DeleteDomainObject( solderPasteControl, 
				new ICheck[]{ new DeleteAssociateCheck( solderPasteControl,
								this.DataProvider, 
								new Type[]{
											  typeof(SolderPaste)	})} );
		}

		public object GetSolderPasteControl( string partNO )
		{
			return this.DataProvider.CustomSearch(typeof(SolderPasteControl), new object[]{ partNO });
		}

		/// <summary>
		/// ** ��������:	��ѯSolderPasteControl��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-17 15:45:18
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="partNO">PartNO��ģ����ѯ</param>
		/// <returns> SolderPasteControl���ܼ�¼��</returns>
		public int QuerySolderPasteControlCount( string partNO)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSOLDERPASTECONTROL where 1=1 and PARTNO like '{0}%' " , partNO)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSolderPasteControl
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-17 15:45:18
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="partNO">PartNO��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SolderPasteControl����</returns>
		public object[] QuerySolderPasteControl( string partNO, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(SolderPasteControl), new PagerCondition(string.Format("select {0} from TBLSOLDERPASTECONTROL where 1=1 and PARTNO like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPasteControl)) , partNO), "PARTNO", inclusive, exclusive));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="partNO"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public int QuerySolderPasteControlCount( string partNO, string type)
		{
			string condition = string.Empty;
			if( type!=null && type.Length>0 )
			{
				condition += string.Format(" and type = '{0}' ", type);
			}
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSOLDERPASTECONTROL where 1=1 and PARTNO like '{0}%' {1} " , partNO, condition)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="partNO"></param>
		/// <param name="type"></param>
		/// <param name="inclusive"></param>
		/// <param name="exclusive"></param>
		/// <returns></returns>
		public object[] QuerySolderPasteControl( string partNO, string type, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if( type!=null && type.Length>0 )
			{
				condition += string.Format(" and type = '{0}' ", type);
			}
			return this.DataProvider.CustomQuery(typeof(SolderPasteControl), new PagerCondition(string.Format("select {0} from TBLSOLDERPASTECONTROL where 1=1 and PARTNO like '{1}%' {2} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPasteControl)) , partNO, condition), "PARTNO", inclusive, exclusive));
		} 

		/// <summary>
		/// ** ��������:	������е�SolderPasteControl
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-17 15:45:18
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>SolderPasteControl���ܼ�¼��</returns>
		public object[] GetAllSolderPasteControl()
		{
			return this.DataProvider.CustomQuery(typeof(SolderPasteControl), new SQLCondition(string.Format("select {0} from TBLSOLDERPASTECONTROL order by PARTNO", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPasteControl)))));
		}


		#endregion

		#region SOLDERPASTEPRO
		/// <summary>
		/// �������
		/// </summary>
		public SOLDERPASTEPRO CreateNewSOLDERPASTEPRO()
		{
			return new SOLDERPASTEPRO();
		}

		public void AddSOLDERPASTEPRO( SOLDERPASTEPRO sOLDERPASTEPRO)
		{
			this._helper.AddDomainObject( sOLDERPASTEPRO );
		}

		public void UpdateSOLDERPASTEPRO(SOLDERPASTEPRO sOLDERPASTEPRO)
		{
			this._helper.UpdateDomainObject( sOLDERPASTEPRO );
		}

		public void DeleteSOLDERPASTEPRO(SOLDERPASTEPRO sOLDERPASTEPRO)
		{
			this._helper.DeleteDomainObject( sOLDERPASTEPRO );
		}

		public void DeleteSOLDERPASTEPRO(SOLDERPASTEPRO[] sOLDERPASTEPRO)
		{
			this._helper.DeleteDomainObject( sOLDERPASTEPRO );
		}

		public object GetSOLDERPASTEPRO( string sPRPKID )
		{
			return this.DataProvider.CustomSearch(typeof(SOLDERPASTEPRO), new object[]{ sPRPKID });
		}

		public object GetSOLDERPASTEPROBySPPID( string SPPID )
		{
			object[] objs =  DataProvider.CustomQuery(typeof(SOLDERPASTEPRO),
				new SQLCondition(
				String.Format("select {0} from TBLSOLDERPASTEPRO where SOLDERPASTEID ='" + SPPID + "' order by mdate desc,mtime desc,sequence desc"
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SOLDERPASTEPRO)))));

			if(objs != null && objs.Length > 0)
			{
				return objs[0];
			}
			else
			{
				return null;
			}
		}

//		public object GetSPP( string SPPID ,string moCode,string ssCode)
//		{
//			object[] objs =  DataProvider.CustomQuery(typeof(SOLDERPASTEPRO),
//				new SQLCondition(
//				String.Format("select {0} from TBLSOLDERPASTEPRO where SOLDERPASTEID ='" + SPPID + "' "
//				+ "AND mocode = '" + moCode + "'"
//				+ "AND linecode = '" + ssCode + "'"
//				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SOLDERPASTEPRO)))));
//
//			if(objs != null && objs.Length > 0)
//			{
//				return objs[0];
//			}
//			else
//			{
//				return null;
//			}
//		}

//		public void TransMOSPP(string moCode , string ssCode,SOLDERPASTEPRO spp)
//		{
//			DataProvider.CustomExecute(	new SQLCondition(
//				"UPDATE  TBLSOLDERPASTEPRO  set "
//				+ "mocode = '" + spp.MOCODE + "' "
//				+ ",linecode = '" + spp.LINECODE + "' "
//				+ ",MUSER = '" + spp.MUSER + "'"
//				+ ",MDATE = " + spp.MDATE.ToString()
//				+ ",MTIME = " + spp.MTIME.ToString()
//				+ "WHERE SPPID = '" + spp.SOLDERPASTEID + "'"
//				+ "AND MOCODE = '" + moCode + "'"
//			    + "AND LINECODE = '" + ssCode +  "'"));
//
//		}

		public object GetSPP(string SPPID, string moCode , string ssCode)
		{
			object[] objs =  DataProvider.CustomQuery(typeof(SOLDERPASTEPRO),
				new SQLCondition(
				String.Format("SELECT {0} FROM TBLSOLDERPASTEPRO WHERE "
				+ "SOLDERPASTEID = '" + SPPID + "' "
				+ "AND mocode = '" + moCode + "' "
				+ "AND linecode = '" + ssCode + "' "
				+ "AND STATUS in ('" + Web.Helper.SolderPasteStatus.Normal + "','" 
				+ Web.Helper.SolderPasteStatus.Agitate + "','"
				+ Web.Helper.SolderPasteStatus.Return + "','"
				+ Web.Helper.SolderPasteStatus.Unveil + "')"
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SOLDERPASTEPRO)))));

			if(objs != null && objs.Length > 0)
			{
				return objs[0];
			}
			else
			{
				return null;
			}
		}

		public object GetSPPForUnavial(string SPPID, string moCode , string ssCode)
		{
			object[] objs =  DataProvider.CustomQuery(typeof(SOLDERPASTEPRO),
				new SQLCondition(
				String.Format("SELECT {0} FROM TBLSOLDERPASTEPRO WHERE "
				+ "SOLDERPASTEID = '" + SPPID + "' "
				+ "AND mocode = '" + moCode + "' "
				+ "AND linecode = '" + ssCode + "' "
				+ "AND STATUS not in ('" + Web.Helper.SolderPasteStatus.UsedUp + "','" 
				+ Web.Helper.SolderPasteStatus.scrap + "')"
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SOLDERPASTEPRO)))));

			if(objs != null && objs.Length > 0)
			{
				return objs[0];
			}
			else
			{
				return null;
			}
		}

		public object GetFirstInSPPByItem( string itemCode )
		{
			object[] objs =  DataProvider.CustomQuery(typeof(SolderPaste),
				new SQLCondition(
				String.Format("SELECT {0} FROM (SELECT {1}  FROM tblsolderpaste WHERE partno " 
				+ "IN (SELECT partno FROM tblsolderpastecontrol WHERE \"TYPE\" " 
				+ "IN (SELECT sptype FROM tblsolderpaste2item WHERE itemcode = '" + itemCode + "')) "
				+ "AND STATUS IN ('" + Web.Helper.SolderPasteStatus.Normal 
				+ "','" + Web.Helper.SolderPasteStatus.Reflow + "') order by PRODATE ) WHERE ROWNUM = 1"
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste))
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste)))));

			if(objs != null && objs.Length > 0)
			{
				return objs[0];
			}
			else
			{
				return null;
			}
		}

		public object[] GetReflowInSPPByItem( string itemCode )
		{
			object[] objs =  DataProvider.CustomQuery(typeof(SolderPaste),
				new SQLCondition(
				String.Format("SELECT {0}  FROM tblsolderpaste WHERE partno " 
				+ "IN (SELECT partno FROM tblsolderpastecontrol WHERE \"TYPE\" " 
				+ "IN (SELECT sptype FROM tblsolderpaste2item WHERE itemcode = '" + itemCode + "')) "
				+ "AND STATUS IN ('" + Web.Helper.SolderPasteStatus.Reflow + "')"
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste)))));

			return objs;
		}

		public object[] GetFirstInSPPByItem( string itemCode , string spProDate ,string spID)
		{
			object[] objs =  DataProvider.CustomQuery(typeof(SolderPaste),
				new SQLCondition(
				String.Format("SELECT {0} FROM tblsolderpaste WHERE partno " 
				+ "IN (SELECT partno FROM tblsolderpastecontrol WHERE \"TYPE\" " 
				+ "IN (SELECT sptype FROM tblsolderpaste2item WHERE itemcode = '" + itemCode + "')) "
				+ "AND STATUS IN ('" + Web.Helper.SolderPasteStatus.Normal 
				+ "','" + Web.Helper.SolderPasteStatus.Reflow + "') and PRODATE <= " + spProDate 
				+ " AND SPID NOT IN ('" + spID + "')"
				+ " AND SPID NOT IN (SELECT SOLDERPASTEID from tblsolderpastepro where "
				+ " STATUS in ('" 
				+ Web.Helper.SolderPasteStatus.Agitate + "','"
				+ Web.Helper.SolderPasteStatus.Restrain + "','"
				+ Web.Helper.SolderPasteStatus.Return + "','"
				+ Web.Helper.SolderPasteStatus.Unveil + "'))"
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SolderPaste)))));

			return objs;
		}

		public object[] QueryOnWorkSPP( string moCode , string ssCode)
		{
			string sql = "SELECT {0} FROM TBLSOLDERPASTEPRO WHERE 1=1 ";
			if(moCode != null && moCode != String.Empty)
			{
				sql += " AND mocode = '" + moCode + "'";
			}
			if(ssCode != null && ssCode != String.Empty)
			{
				sql	+= " AND linecode = '" + ssCode + "'";
			}

			sql += " AND STATUS in ('" + Web.Helper.SolderPasteStatus.Normal + "','" 
				+ Web.Helper.SolderPasteStatus.Agitate + "','"
				+ Web.Helper.SolderPasteStatus.Restrain + "','"
				+ Web.Helper.SolderPasteStatus.Return + "','"
				+ Web.Helper.SolderPasteStatus.Unveil + "')";
			//Laws Lu,2006/10/08,Power0202 2006-9-8 �� �������ʱ��û�а�ʱ���Ⱥ��Զ����򣬲�����ʹ�� 

			sql += "order by RETURNDATE ,RETURNTIME ";
			object[] objs =  DataProvider.CustomQuery(typeof(SOLDERPASTEPRO),
				new SQLCondition(
				String.Format(sql
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SOLDERPASTEPRO)))));

			return objs;
			//			if(objs != null && objs.Length > 0)
			//			{
			//				return objs[0];
			//			}
			//			else
			//			{
			//				return null;
			//			}
		}

		public object[] QueryOnWorkSPP( string moCode , string ssCode,string spid)
		{
			string sql = "SELECT {0} FROM TBLSOLDERPASTEPRO WHERE 1=1 ";
			if(moCode != null && moCode != String.Empty)
			{
				sql += " AND mocode = '" + moCode + "'";
			}
			if(ssCode != null && ssCode != String.Empty)
			{
				sql	+= " AND linecode = '" + ssCode + "'";
			}
			if(spid != null && spid != String.Empty)
			{
				sql	+= " AND SOLDERPASTEID like '" + spid + "%'";
			}

			sql += " AND STATUS in ('" + Web.Helper.SolderPasteStatus.Normal + "','" 
				+ Web.Helper.SolderPasteStatus.Agitate + "','"
				+ Web.Helper.SolderPasteStatus.Restrain + "','"
				+ Web.Helper.SolderPasteStatus.Return + "','"
				+ Web.Helper.SolderPasteStatus.Unveil + "')";
			//Laws Lu,2006/10/08,Power0202 2006-9-8 �� �������ʱ��û�а�ʱ���Ⱥ��Զ����򣬲�����ʹ�� 

			sql += "order by RETURNDATE ,RETURNTIME ";
			object[] objs =  DataProvider.CustomQuery(typeof(SOLDERPASTEPRO),
				new SQLCondition(
				String.Format(sql
				,DomainObjectUtility.GetDomainObjectFieldsString(typeof(SOLDERPASTEPRO)))));

			return objs;
			//			if(objs != null && objs.Length > 0)
			//			{
			//				return objs[0];
			//			}
			//			else
			//			{
			//				return null;
			//			}
		}

		/// <summary>
		/// ** ��������:	��ѯSOLDERPASTEPRO��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-7-17 14:40:27
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="sPRPKID">SPRPKID��ģ����ѯ</param>
		/// <returns> SOLDERPASTEPRO���ܼ�¼��</returns>
		public int QuerySOLDERPASTEPROCount( string sPRPKID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSOLDERPASTEPRO where 1=1 and SPRPKID like '{0}%' " , sPRPKID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯSOLDERPASTEPRO
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-7-17 14:40:27
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="sPRPKID">SPRPKID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> SOLDERPASTEPRO����</returns>
		public object[] QuerySOLDERPASTEPRO( string sPRPKID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(SOLDERPASTEPRO), new PagerCondition(string.Format("select {0} from TBLSOLDERPASTEPRO where 1=1 and SPRPKID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SOLDERPASTEPRO)) , sPRPKID), "SPRPKID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�SOLDERPASTEPRO
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2006-7-17 14:40:27
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>SOLDERPASTEPRO���ܼ�¼��</returns>
		public object[] GetAllSOLDERPASTEPRO()
		{
			return this.DataProvider.CustomQuery(typeof(SOLDERPASTEPRO), new SQLCondition(string.Format("select {0} from TBLSOLDERPASTEPRO order by SPRPKID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SOLDERPASTEPRO)))));
		}


		#endregion

	}
}

