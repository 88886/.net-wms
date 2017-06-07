using System;
using System.Collections;
using System.Runtime.Remoting;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.ArmorPlate;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.SMT
{
	/// <summary>
	/// ArmorPlateFacade ��ժҪ˵����
	/// �ļ���:		ArmorPlateFacade.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		ER/Studio Basic Macro Code Generation  Created by ****
	/// ��������:	2006-7-25 11:37:06
	/// �޸���:
	/// �޸�����:
	/// �� ��:	
	/// �� ��:	
	/// </summary>
	public class ArmorPlateFacade
	{
		private IDomainDataProvider _domainDataProvider = null;
		private FacadeHelper _helper = null;
		
		public ArmorPlateFacade(IDomainDataProvider domainDataProvider)
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

		#region ArmorPlate
		/// <summary>
		/// 
		/// </summary>
		public ArmorPlate CreateNewArmorPlate()
		{
			return new ArmorPlate();
		}

		public void AddArmorPlate( ArmorPlate armorPlate)
		{
			this.DataProvider.BeginTransaction();
			try
			{
				this._helper.AddDomainObject( armorPlate );
				if( armorPlate.Items!=null && armorPlate.Items.Length>0 )
				{
					for(int i=0; i<armorPlate.Items.Length; i++)
					{
						this._helper.AddDomainObject( armorPlate.Items[i] );
					}
				}

				this.DataProvider.CommitTransaction();
			}
			catch( Exception ex )
			{
				this.DataProvider.RollbackTransaction();

				ExceptionManager.Raise(armorPlate.GetType(), "$Error_Add_Domain_Object", ex);
			}
		}

		public void UpdateArmorPlate(ArmorPlate armorPlate)
		{
			if(armorPlate.Items!=null)
			{
				object[] oldItems = this.GetArmorPlate2Items( armorPlate.ArmorPlateID );
				ArrayList array = new ArrayList(oldItems);
				this._helper.DeleteDomainObject( (ArmorPlate2Item[])array.ToArray(typeof(ArmorPlate2Item)) );
			}

			this.DataProvider.BeginTransaction();
			try
			{
				this._helper.UpdateDomainObject( armorPlate );
				if( armorPlate.Items!=null && armorPlate.Items.Length>0 )
				{
					for(int i=0; i<armorPlate.Items.Length; i++)
					{
						this._helper.AddDomainObject( armorPlate.Items[i], false);
					}
				}

				this.DataProvider.CommitTransaction();
			}
			catch( Exception ex )
			{
				this.DataProvider.RollbackTransaction();

				ExceptionManager.Raise(armorPlate.GetType(), "$Error_Update_Domain_Object", ex);
			}
		}

		public void DeleteArmorPlate(ArmorPlate armorPlate)
		{
			this.DataProvider.BeginTransaction();
			try
			{
				object[] oldItems = this.GetArmorPlate2Items( armorPlate.ArmorPlateID );

				if( oldItems!=null && oldItems.Length>0 )
				{
					for(int i=0; i<oldItems.Length; i++)
					{
						this._helper.DeleteDomainObject( oldItems[i] as ArmorPlate2Item );
					}
				}

				this._helper.DeleteDomainObject( armorPlate, 
					new ICheck[]{ new DeleteAssociateCheck( armorPlate,
									this.DataProvider, 
									new Type[]{
												  typeof(ArmorPlateVersionChangeList),
												  typeof(ArmorPlateStatusChangeList),
												  typeof(ArmorPlateContol)	})} );

				this.DataProvider.CommitTransaction();
			}
			catch( Exception ex )
			{
				this.DataProvider.RollbackTransaction();

				ExceptionManager.Raise(armorPlate.GetType(), "$Error_Delete_Domain_Object", ex);
			}
		}

		public void DeleteArmorPlate(ArmorPlate[] armorPlate)
		{
			this.DataProvider.BeginTransaction();
			try
			{
				for(int i=0; i<armorPlate.Length; i++)
				{
					object[] oldItems = this.GetArmorPlate2Items( armorPlate[i].ArmorPlateID );

					if( oldItems!=null && oldItems.Length>0 )
					{
						for(int j=0; j<oldItems.Length; j++)
						{
							this._helper.DeleteDomainObject( oldItems[j] as ArmorPlate2Item);
						}
					}

					this._helper.DeleteDomainObject( armorPlate[i], 
						new ICheck[]{ new DeleteAssociateCheck( armorPlate,
										this.DataProvider, 
										new Type[]{
													  typeof(ArmorPlateVersionChangeList),
													  typeof(ArmorPlateStatusChangeList),
													  typeof(ArmorPlateContol)	})} );
				}

				this.DataProvider.CommitTransaction();
			}
			catch( Exception ex )
			{
				this.DataProvider.RollbackTransaction();

				ExceptionManager.Raise(armorPlate[0].GetType(), "$Error_Delete_Domain_Object", ex);
			}


		}

		public object GetArmorPlate( string armorPlateID )
		{
			object obj = this.DataProvider.CustomSearch(typeof(ArmorPlate), new object[]{ armorPlateID });
			if(obj!=null)
			{
				ArmorPlate ap = obj as ArmorPlate ;
				object[] items = this.GetArmorPlate2Items( ap.ArmorPlateID );
				if(items!=null && items.Length>0)
				{
					foreach( ArmorPlate2Item item in items )
					{
						ap.WithItems += item.ItemCode + ",";
					}
					ap.WithItems = ap.WithItems.TrimEnd(',');
				}

			}
			return obj;
		}

		/// <summary>
		/// ** ��������:	��ѯArmorPlate��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="armorPlateID">ArmorPlateID��ģ����ѯ</param>
		/// <returns> ArmorPlate���ܼ�¼��</returns>
		public int QueryArmorPlateCount( string armorPlateID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBlARMORPLATE where 1=1 and APID like '{0}%' " , armorPlateID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯArmorPlate
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="armorPlateID">ArmorPlateID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ArmorPlate����</returns>
		public object[] QueryArmorPlate( string armorPlateID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlate), new PagerCondition(string.Format("select {0} from TBlARMORPLATE where 1=1 and APID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlate)) , armorPlateID), "APID", inclusive, exclusive));
		}

		public int QueryArmorPlateCount( string armorPlateID, string basePlateCode, string itemcode )
		{
			string condition = string.Empty;
			if( armorPlateID!=null && armorPlateID.Length>0 )
			{
				condition += string.Format(" and APID like '{0}%' ", armorPlateID );
			}

			if( basePlateCode!=null && basePlateCode.Length>0 )
			{
				condition += string.Format(" and BPCODE like '{0}%' ", basePlateCode );
			}

			if( itemcode!=null && itemcode.Length>0 )
			{
				condition += string.Format(" and APID in ( select APID from TBLAP2ITEM where ITEMCODE like '{0}%' ) ", itemcode );
			}

			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBlARMORPLATE where 1=1 {0}" , condition)));
		}

		public object[] QueryArmorPlate( string armorPlateID, string basePlateCode, string itemcode, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if( armorPlateID!=null && armorPlateID.Length>0 )
			{
				condition += string.Format(" and APID like '{0}%' ", armorPlateID );
			}

			if( basePlateCode!=null && basePlateCode.Length>0 )
			{
				condition += string.Format(" and BPCODE like '{0}%' ", basePlateCode );
			}

			if( itemcode!=null && itemcode.Length>0 )
			{
				condition += string.Format(" and APID in ( select APID from TBLAP2ITEM where ITEMCODE like '{0}%' ) ", itemcode );
			}

			object[] objs = this.DataProvider.CustomQuery(typeof(ArmorPlate), new PagerCondition(string.Format("select {0} from TBlARMORPLATE where 1=1 {1}", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlate)) , condition), "APID", inclusive, exclusive));
			
			if(objs!=null && objs.Length>0)
			{
				for( int i=0; i<objs.Length; i++ )
				{
					ArmorPlate ap = objs[i] as ArmorPlate ;

					object[] items = this.GetArmorPlate2Items( ap.ArmorPlateID );
					if(items!=null && items.Length>0)
					{
						foreach( ArmorPlate2Item item in items )
						{
							ap.WithItems += item.ItemCode + ",";
						}
						ap.WithItems = ap.WithItems.TrimEnd(',');
					}
				}
			}

			return objs;
		}

		/// <summary>
		/// ** ��������:	������е�ArmorPlate
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ArmorPlate���ܼ�¼��</returns>
		public object[] GetAllArmorPlate()
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlate), new SQLCondition(string.Format("select {0} from TBlARMORPLATE order by APID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlate)))));
		}


		#endregion

		#region ArmorPlate2Item
		/// <summary>
		/// 
		/// </summary>
		public ArmorPlate2Item CreateNewArmorPlate2Item()
		{
			return new ArmorPlate2Item();
		}

		public void AddArmorPlate2Item( ArmorPlate2Item armorPlate2Item)
		{
			this._helper.AddDomainObject( armorPlate2Item );
		}

		public void UpdateArmorPlate2Item(ArmorPlate2Item armorPlate2Item)
		{
			this._helper.UpdateDomainObject( armorPlate2Item );
		}

		public void DeleteArmorPlate2Item(ArmorPlate2Item armorPlate2Item)
		{
			this._helper.DeleteDomainObject( armorPlate2Item );
		}

		public void DeleteArmorPlate2Item(ArmorPlate2Item[] armorPlate2Item)
		{
			this._helper.DeleteDomainObject( armorPlate2Item );
		}

		public object GetArmorPlate2Item( string itemCode, string armorPlateID )
		{
			return this.DataProvider.CustomSearch(typeof(ArmorPlate2Item), new object[]{ itemCode, armorPlateID });
		}

		public object[] GetArmorPlate2Items( string armorPlateID )
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlate2Item), 
				new SQLCondition(string.Format("select {0} from TBLAP2ITEM where 1=1 and APID = '{1}' order by ItemCode ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlate2Item)) , armorPlateID)));
		}

		/// <summary>
		/// ** ��������:	��ѯArmorPlate2Item��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="armorPlateID">ArmorPlateID��ģ����ѯ</param>
		/// <returns> ArmorPlate2Item���ܼ�¼��</returns>
		public int QueryArmorPlate2ItemCount( string itemCode, string armorPlateID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLAP2ITEM where 1=1 and ITEMCODE like '{0}%'  and APID like '{1}%' " , itemCode, armorPlateID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯArmorPlate2Item
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="itemCode">ItemCode��ģ����ѯ</param>
		/// <param name="armorPlateID">ArmorPlateID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ArmorPlate2Item����</returns>
		public object[] QueryArmorPlate2Item( string itemCode, string armorPlateID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlate2Item), new PagerCondition(string.Format("select {0} from TBLAP2ITEM where 1=1 and ITEMCODE like '{1}%'  and APID like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlate2Item)) , itemCode, armorPlateID), "ITEMCODE,APID", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ArmorPlate2Item
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ArmorPlate2Item���ܼ�¼��</returns>
		public object[] GetAllArmorPlate2Item()
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlate2Item), new SQLCondition(string.Format("select {0} from TBLAP2ITEM order by ITEMCODE,APID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlate2Item)))));
		}


		#endregion

		#region ArmorPlateContol
		/// <summary>
		/// 
		/// </summary>
		public ArmorPlateContol CreateNewArmorPlateContol()
		{
			return new ArmorPlateContol();
		}

		public void AddArmorPlateContol( ArmorPlateContol armorPlateContol)
		{
			this._helper.AddDomainObject( armorPlateContol );
		}

		public void UpdateArmorPlateContol(ArmorPlateContol armorPlateContol)
		{
			this._helper.UpdateDomainObject( armorPlateContol );
		}

		public void DeleteArmorPlateContol(ArmorPlateContol armorPlateContol)
		{
			this._helper.DeleteDomainObject( armorPlateContol );
		}

		public void DeleteArmorPlateContol(ArmorPlateContol[] armorPlateContol)
		{
			this._helper.DeleteDomainObject( armorPlateContol );
		}

		public object GetArmorPlateContol( string oID )
		{
			return this.DataProvider.CustomSearch(typeof(ArmorPlateContol), new object[]{ oID });
		}

		public object GetArmorPlateInUseContol( string apid )
		{
			object[] objs = this.DataProvider.CustomQuery(
				typeof(ArmorPlateContol), 
				new SQLCondition(
				string.Format("select {0} from TBlARMORPLATECONTROL where apid='{1}' and status = '{2}' ", 
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateContol)), apid, FormatHelper.TRUE_STRING)));
			if(objs!=null && objs.Length>0)
			{
				return objs[0];
			}

			return null;
		}

		/// <summary>
		/// ** ��������:	��ѯArmorPlateContol��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="oID">OID��ģ����ѯ</param>
		/// <returns> ArmorPlateContol���ܼ�¼��</returns>
		public int QueryArmorPlateContolCount( string oID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBlARMORPLATECONTROL where 1=1 and OID like '{0}%' " , oID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯArmorPlateContol
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="oID">OID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ArmorPlateContol����</returns>
		public object[] QueryArmorPlateContol( string oID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlateContol), new PagerCondition(string.Format("select {0} from TBlARMORPLATECONTROL where 1=1 and OID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateContol)) , oID), "OID", inclusive, exclusive));
		}

		public object[] QueryArmorPlateContol( string mocode, string sscode )
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlateContol), new SQLCondition(string.Format("select {0} from TBlARMORPLATECONTROL where Status='{1}' and USEDMOCODE='{2}' and USEDSSCODE='{3}' order by apid", 
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateContol)) , 
				FormatHelper.TRUE_STRING,
				mocode, sscode)));
		}

		public int QueryArmorPlateContolCount( string apid, string bpcode, string mocodes, string sscode)
		{
			string condition = string.Empty;
			if( apid!=null && apid.Length>0 )
			{
				condition += string.Format( " and APID like '{0}%' ", apid );
			}

			if( bpcode!=null && bpcode.Length>0 )
			{
				condition += string.Format( " and BPCODE like '{0}%' ", bpcode );
			}

			if( mocodes!=null && mocodes.Length>0 )
			{
				condition += string.Format( " and USEDMOCODE in ({0}) ", FormatHelper.ProcessQueryValues( mocodes ) );
			}

			if( sscode!=null && sscode.Length>0 )
			{
				condition += string.Format( " and USEDSSCODE like '{0}%' ", sscode );
			}
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBlARMORPLATECONTROL where 1=1 {0} " , condition)));
		}

		public int QueryArmorPlateContolCount( string apid, string bpcode, string mocodes, string sscode,string beginDate,string endDate)
		{
			string condition = string.Empty;
			if( apid!=null && apid.Length>0 )
			{
				condition += string.Format( " and APID like '{0}%' ", apid );
			}

			if( bpcode!=null && bpcode.Length>0 )
			{
				condition += string.Format( " and BPCODE like '{0}%' ", bpcode );
			}

			if( mocodes!=null && mocodes.Length>0 )
			{
				condition += string.Format( " and USEDMOCODE in ({0}) ", FormatHelper.ProcessQueryValues( mocodes ) );
			}

			if( sscode!=null && sscode.Length>0 )
			{
				condition += string.Format( " and USEDSSCODE like '{0}%' ", sscode );
			}
            //if(beginDate != null && beginDate != String.Empty
            //    && endDate != null &&��endDate != String.Empty)
            //{
            //    condition += FormatHelper.GetDateRangeSql("UDATE",int.Parse(beginDate),int.Parse(endDate));
            //}
            if (beginDate != null && beginDate != String.Empty && beginDate != "0")
            {
                condition += " and UDATE >= " + beginDate;
            }
            if (endDate != null && endDate != String.Empty && endDate != "0")
            {
                condition += " and UDATE <= " + endDate;
            }

			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBlARMORPLATECONTROL where 1=1 {0} " , condition)));
		}

		public object[] QueryArmorPlateContol(  string apid, string bpcode, string mocodes, string sscode,string beginDate,string endDate, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if( apid!=null && apid.Length>0 )
			{
				condition += string.Format( " and APID like '{0}%' ", apid );
			}

			if( bpcode!=null && bpcode.Length>0 )
			{
				condition += string.Format( " and BPCODE like '{0}%' ", bpcode );
			}

			if( mocodes!=null && mocodes.Length>0 )
			{
				condition += string.Format( " and USEDMOCODE in ({0}) ", FormatHelper.ProcessQueryValues( mocodes ) );
			}

			if( sscode!=null && sscode.Length>0 )
			{
				condition += string.Format( " and USEDSSCODE like '{0}%' ", sscode );
			}

            //if(beginDate != null && beginDate != String.Empty
            //    && endDate != null &&��endDate != String.Empty)
            //{
            //    condition += FormatHelper.GetDateRangeSql("UDATE",int.Parse(beginDate),int.Parse(endDate));
            //}
            if (beginDate != null && beginDate != String.Empty && beginDate != "0")
            {
                condition += " and UDATE >= " + beginDate;
            }
            if (endDate != null && endDate != String.Empty && endDate != "0")
            {
                condition += " and UDATE <= " + endDate;
            }

			return this.DataProvider.CustomQuery(typeof(ArmorPlateContol), new PagerCondition(string.Format("select {0} from TBlARMORPLATECONTROL where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateContol)) , condition), "apid,mdate,mtime", inclusive, exclusive));
		}
	
		public object[] QueryArmorPlateContol(  string apid, string bpcode, string mocodes, string sscode, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if( apid!=null && apid.Length>0 )
			{
				condition += string.Format( " and APID like '{0}%' ", apid );
			}

			if( bpcode!=null && bpcode.Length>0 )
			{
				condition += string.Format( " and BPCODE like '{0}%' ", bpcode );
			}

			if( mocodes!=null && mocodes.Length>0 )
			{
				condition += string.Format( " and USEDMOCODE in ({0}) ", FormatHelper.ProcessQueryValues( mocodes ) );
			}

			if( sscode!=null && sscode.Length>0 )
			{
				condition += string.Format( " and USEDSSCODE like '{0}%' ", sscode );
			}

			return this.DataProvider.CustomQuery(typeof(ArmorPlateContol), new PagerCondition(string.Format("select {0} from TBlARMORPLATECONTROL where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateContol)) , condition), "apid,mdate,mtime", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ArmorPlateContol
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ArmorPlateContol���ܼ�¼��</returns>
		public object[] GetAllArmorPlateContol()
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlateContol), new SQLCondition(string.Format("select {0} from TBlARMORPLATECONTROL order by OID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateContol)))));
		}


		#endregion

		#region ArmorPlateStatusChangeList
		/// <summary>
		/// 
		/// </summary>
		public ArmorPlateStatusChangeList CreateNewArmorPlateStatusChangeList()
		{
			return new ArmorPlateStatusChangeList();
		}

		public void AddArmorPlateStatusChangeList( ArmorPlateStatusChangeList armorPlateStatusChangeList)
		{
			this._helper.AddDomainObject( armorPlateStatusChangeList );
		}

		public void UpdateArmorPlateStatusChangeList(ArmorPlateStatusChangeList armorPlateStatusChangeList)
		{
			this._helper.UpdateDomainObject( armorPlateStatusChangeList );
		}

		public void DeleteArmorPlateStatusChangeList(ArmorPlateStatusChangeList armorPlateStatusChangeList)
		{
			this._helper.DeleteDomainObject( armorPlateStatusChangeList );
		}

		public void DeleteArmorPlateStatusChangeList(ArmorPlateStatusChangeList[] armorPlateStatusChangeList)
		{
			this._helper.DeleteDomainObject( armorPlateStatusChangeList );
		}

		public object GetArmorPlateStatusChangeList( string oID )
		{
			return this.DataProvider.CustomSearch(typeof(ArmorPlateStatusChangeList), new object[]{ oID });
		}

		/// <summary>
		/// ** ��������:	��ѯArmorPlateStatusChangeList��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="oID">OID��ģ����ѯ</param>
		/// <returns> ArmorPlateStatusChangeList���ܼ�¼��</returns>
		public int QueryArmorPlateStatusChangeListCount( string oID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLAPSTATUSLIST where 1=1 and OID like '{0}%' " , oID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯArmorPlateStatusChangeList
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="oID">OID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ArmorPlateStatusChangeList����</returns>
		public object[] QueryArmorPlateStatusChangeList( string oID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlateStatusChangeList), new PagerCondition(string.Format("select {0} from TBLAPSTATUSLIST where 1=1 and OID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateStatusChangeList)) , oID), "OID", inclusive, exclusive));
		}

		public int QueryArmorPlateStatusChangeListCount( string armorPlateID, string basePlateCode)
		{
			string condition = string.Empty;
			if(armorPlateID!=null && armorPlateID.Length>0)
			{
				condition += string.Format( " and APID like '{0}%' ",armorPlateID );
			}
			if(basePlateCode!=null && basePlateCode.Length>0)
			{
				condition += string.Format( " and BPCODE like '{0}%' ",basePlateCode );
			}
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLAPSTATUSLIST where 1=1 {0} " , condition)));
		}

		public object[] QueryArmorPlateStatusChangeList( string armorPlateID, string basePlateCode, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if(armorPlateID!=null && armorPlateID.Length>0)
			{
				condition += string.Format( " and APID like '{0}%' ",armorPlateID );
			}
			if(basePlateCode!=null && basePlateCode.Length>0)
			{
				condition += string.Format( " and BPCODE like '{0}%' ",basePlateCode );
			}
			return this.DataProvider.CustomQuery(typeof(ArmorPlateStatusChangeList), new PagerCondition(string.Format("select {0} from TBLAPSTATUSLIST where 1=1 {1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateStatusChangeList)) , condition), "APID, mdate, mtime", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ArmorPlateStatusChangeList
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ArmorPlateStatusChangeList���ܼ�¼��</returns>
		public object[] GetAllArmorPlateStatusChangeList()
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlateStatusChangeList), new SQLCondition(string.Format("select {0} from TBLAPSTATUSLIST order by OID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateStatusChangeList)))));
		}


		#endregion

		#region ArmorPlateVersionChangeList
		/// <summary>
		/// 
		/// </summary>
		public ArmorPlateVersionChangeList CreateNewArmorPlateVersionChangeList()
		{
			return new ArmorPlateVersionChangeList();
		}

		public void AddArmorPlateVersionChangeList( ArmorPlateVersionChangeList armorPlateVersionChangeList)
		{
			this._helper.AddDomainObject( armorPlateVersionChangeList );
		}

		public void UpdateArmorPlateVersionChangeList(ArmorPlateVersionChangeList armorPlateVersionChangeList)
		{
			this._helper.UpdateDomainObject( armorPlateVersionChangeList );
		}

		public void DeleteArmorPlateVersionChangeList(ArmorPlateVersionChangeList armorPlateVersionChangeList)
		{
			this._helper.DeleteDomainObject( armorPlateVersionChangeList );
		}

		public void DeleteArmorPlateVersionChangeList(ArmorPlateVersionChangeList[] armorPlateVersionChangeList)
		{
			this._helper.DeleteDomainObject( armorPlateVersionChangeList );
		}

		public object GetArmorPlateVersionChangeList( string oID )
		{
			return this.DataProvider.CustomSearch(typeof(ArmorPlateVersionChangeList), new object[]{ oID });
		}

		/// <summary>
		/// ** ��������:	��ѯArmorPlateVersionChangeList��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="oID">OID��ģ����ѯ</param>
		/// <returns> ArmorPlateVersionChangeList���ܼ�¼��</returns>
		public int QueryArmorPlateVersionChangeListCount( string oID)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLAPVCHANGELIST where 1=1 and OID like '{0}%' " , oID)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯArmorPlateVersionChangeList
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="oID">OID��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ArmorPlateVersionChangeList����</returns>
		public object[] QueryArmorPlateVersionChangeList( string oID, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlateVersionChangeList), new PagerCondition(string.Format("select {0} from TBLAPVCHANGELIST where 1=1 and OID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateVersionChangeList)) , oID), "OID", inclusive, exclusive));
		}

		public int QueryArmorPlateVersionChangeListCount( string armorPlateID, string basePlateCode)
		{
			string condition = string.Empty;
			if(armorPlateID!=null && armorPlateID.Length>0)
			{
				condition += string.Format( " and APID like '{0}%' ",armorPlateID );
			}
			if(basePlateCode!=null && basePlateCode.Length>0)
			{
				condition += string.Format( " and BPCODE like '{0}%' ",basePlateCode );
			}
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLAPVCHANGELIST where 1=1 {0} " , condition)));
		}

		public object[] QueryArmorPlateVersionChangeList( string armorPlateID, string basePlateCode, int inclusive, int exclusive )
		{
			string condition = string.Empty;
			if(armorPlateID!=null && armorPlateID.Length>0)
			{
				condition += string.Format( " and APID like '{0}%' ",armorPlateID );
			}
			if(basePlateCode!=null && basePlateCode.Length>0)
			{
				condition += string.Format( " and BPCODE like '{0}%' ",basePlateCode );
			}
			return this.DataProvider.CustomQuery(typeof(ArmorPlateVersionChangeList), 
				new PagerCondition(string.Format("select {0} from TBLAPVCHANGELIST where 1=1 {1} ", 
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateVersionChangeList)) , condition), 
				"APID, mdate, mtime", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ArmorPlateVersionChangeList
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by ****
		/// ** �� ��:		2006-7-25 11:37:06
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ArmorPlateVersionChangeList���ܼ�¼��</returns>
		public object[] GetAllArmorPlateVersionChangeList()
		{
			return this.DataProvider.CustomQuery(typeof(ArmorPlateVersionChangeList), new SQLCondition(string.Format("select {0} from TBLAPVCHANGELIST order by OID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ArmorPlateVersionChangeList)))));
		}


		#endregion

	}
}

