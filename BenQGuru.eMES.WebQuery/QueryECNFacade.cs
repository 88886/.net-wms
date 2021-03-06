using System;
using System.Collections;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Common.Helper;  
using BenQGuru.eMES.Common.DomainDataProvider;   
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.DataCollect;

using BenQGuru.eMES.Material;

namespace BenQGuru.eMES.WebQuery
{
	/// <summary>
	/// QueryECNFacade 的摘要说明。
	/// </summary>
	public class QueryECNFacade
	{
		private  IDomainDataProvider _domainDataProvider= null;

		public QueryECNFacade( IDomainDataProvider domainDataProvider )
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

		public object[] QueryECN(
			string itemCodes,string moCodes,string modelCodes,
			string startSn,string endSn,string ecn,
			int inclusive,int exclusive)
		{
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

			string modelCondition = "";
			if( modelCodes != "" && modelCodes != null )
			{
				modelCondition = string.Format(
					@" and modelcode in ({0})",FormatHelper.ProcessQueryValues( modelCodes ) );
			}

			string SnCondition = string.Empty;
			CastDownHelper castDownHelper = new CastDownHelper(this.DataProvider);
			if( string.Compare( startSn,endSn,true )==0  && startSn!=string.Empty  )
			{
				ArrayList array = new ArrayList();
				array.Add(startSn);
				castDownHelper.GetAllRCard(ref array,startSn);

				string[] rCards = (string[])array.ToArray(typeof(System.String));
				
				SnCondition = string.Format(" and rcard in ({0}) ",FormatHelper.ProcessQueryValues(rCards));
			}
			else if( string.Compare( startSn,endSn,true )!=0 )
			{
				ArrayList array = new ArrayList();
				castDownHelper.BuildProcessRcardCondition(ref array,startSn.ToUpper(),endSn.ToUpper());
				string rcardCondition = string.Empty ;
				if(array.Count>0)
				{
					for(int i=0; i<array.Count; i++)
					{
						if(i<array.Count-1)
						{
							rcardCondition += array[i].ToString() + " union ";
						}
						else
						{
							rcardCondition += array[i].ToString();
						}
					}
				}
				SnCondition = "and ( 1=1 " + FormatHelper.GetRCardRangeSql("rcard",startSn.ToUpper(),endSn.ToUpper());

				if( rcardCondition!=string.Empty )
				{
					SnCondition += string.Format(" or rcard in ({0}) ",rcardCondition);
				}
				SnCondition += " ) ";
			}

			string ecnCondition = "";
			if( ecn != "" && ecn != null )
			{
				ecnCondition = string.Format(
					@" and ECNNO like '{0}%'",ecn.ToUpper());
			}

			PagerCondition pagerCondition = new PagerCondition(string.Format(
				@"select distinct {0} from TBLONWIPECN where 1=1 {1}{2}{3}{4}{5}",
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(OnWIPECN)),
				itemCondition,moCondition,modelCondition,SnCondition,ecnCondition),
				"mocode,itemcode,opcode",
				inclusive,exclusive,true);
#if DEBUG
			Log.Info(pagerCondition.SQLText);
#endif
			object[] objs = this.DataProvider.CustomQuery(
				typeof(QDOECNList),pagerCondition);

			if( objs != null )
			{
				foreach(QDOECNList obj in objs)
				{
					object[] sn = this.DataProvider.CustomQuery(typeof(Simulation),
						new SQLCondition(
						string.Format(@"select ProductStatus from TBLSIMULATION where RCARD = '{0}'",obj.RunningCard)));
					if( sn != null )
					{
						object[] sn2 = this.DataProvider.CustomQuery(typeof(Simulation),
							new SQLCondition(
							string.Format(
								@"select tblts.tsstatus as ProductStatus from TBLSIMULATION,tblts 
								where TBLSIMULATION.RCARD = tblts.rcard
								and tblsimulation.rcardseq = tblts.rcardseq
								and tblsimulation.mocode = tblts.mocode  
								and TBLSIMULATION.rcard = '{0}' ",obj.RunningCard)));
						if(sn2!=null)
						{
							obj.SNState = (sn2[0] as Simulation).ProductStatus;
						}
						else
						{
							obj.SNState = (sn[0] as Simulation).ProductStatus;
						}
					}
					else
					{
						obj.SNState = ProductStatus.OffLine;
					}

					object[] onWip = this.DataProvider.CustomQuery(typeof(OnWIP),
						new SQLCondition(String.Format(
							@"select opcode from tblonwip where rcard ='{0}'     
							and (rcardseq,mocode) in 
							(select rcardseq,mocode
									from (select rcardseq, mocode
											from tblonwip
											where rcard = '{0}'
											order by mdate * 1000000 + mtime desc)
									where rownum = 1)",obj.RunningCard)));

					if(onWip != null)
					{
						obj.CollectionOPCODE =obj.OPCode;		//采集工位
						obj.OPCode = ((OnWIP)onWip[0]).OPCode ;	//当前工位 
					}
				}
			}
			return objs ;
		}

		public int QueryECNCount(
			string itemCodes,string moCodes,string modelCodes,
			string startSn,string endSn,string ecn)
		{
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

			string modelCondition = "";
			if( modelCodes != "" && modelCodes != null )
			{
				modelCondition = string.Format(
					@" and modelcode in ({0})",FormatHelper.ProcessQueryValues( modelCodes ) );
			}

			string SnCondition = string.Empty;
			CastDownHelper castDownHelper = new CastDownHelper(this.DataProvider);
			if( string.Compare( startSn,endSn,true )==0  && startSn!=string.Empty  )
			{
				ArrayList array = new ArrayList();
				array.Add(startSn);
				castDownHelper.GetAllRCard(ref array,startSn);

				string[] rCards = (string[])array.ToArray(typeof(System.String));
				
				SnCondition = string.Format(" and rcard in ({0}) ",FormatHelper.ProcessQueryValues(rCards));
			}
			else if( string.Compare( startSn,endSn,true )!=0 )
			{
				ArrayList array = new ArrayList();
				castDownHelper.BuildProcessRcardCondition(ref array,startSn.ToUpper(),endSn.ToUpper());
				string rcardCondition = string.Empty ;
				if(array.Count>0)
				{
					for(int i=0; i<array.Count; i++)
					{
						if(i<array.Count-1)
						{
							rcardCondition += array[i].ToString() + " union ";
						}
						else
						{
							rcardCondition += array[i].ToString();
						}
					}
				}
				SnCondition = "and ( 1=1 " + FormatHelper.GetRCardRangeSql("rcard",startSn.ToUpper(),endSn.ToUpper());

				if( rcardCondition!=string.Empty )
				{
					SnCondition += string.Format(" or rcard in ({0}) ",rcardCondition);
				}
				SnCondition += " ) ";
			}

			string ecnCondition = "";
			if( ecn != "" && ecn != null )
			{
				ecnCondition = string.Format(
					@" and ECNNO like '{0}%'",ecn.ToUpper());
			}

#if DEBUG
			Log.Info(
				new SQLCondition(
				string.Format(
				@"select {0} from TBLONWIPECN where 1=1 {1}{2}{3}{4}{5}",
				"count(mocode)",
				itemCondition,moCondition,modelCondition,SnCondition,ecnCondition)).SQLText);
			
#endif
			return this.DataProvider.GetCount(
				new SQLCondition(
				string.Format(
				@"select {0} from TBLONWIPECN where 1=1 {1}{2}{3}{4}{5}",
				"count(mocode)",
				itemCondition,moCondition,modelCondition,SnCondition,ecnCondition)));
		}

		public object[] QueryECNDetails(
			string ecn,
			int inclusive,int exclusive)
		{
#if DEBUG
			Log.Info(
				new PagerCondition(
				string.Format(
				@"select distinct {0} from TBLONWIPECN where ECNNO = '{1}'",
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(QDOECNList)),
				ecn.ToUpper()),
				"RCARD,OPCODE,SSCODE,RESCODE",
				inclusive,exclusive,true).SQLText);
#endif
			object[] objs = this.DataProvider.CustomQuery(
				typeof(QDOECNList),
				new PagerCondition(
				string.Format(
				@"select distinct {0} from TBLONWIPECN where ECNNO = '{1}'",
				DomainObjectUtility.GetDomainObjectFieldsString(typeof(QDOECNList)),
				ecn.ToUpper()),
				"RCARD,OPCODE,SSCODE,RESCODE",
				inclusive,exclusive,true));

			if( objs != null )
			{
				foreach(QDOECNList obj in objs)
				{
					object[] sn = this.DataProvider.CustomQuery(typeof(Simulation),
						new SQLCondition(
						string.Format(@"select ProductStatus from TBLSIMULATION where RCARD = '{0}'",obj.RunningCard)));
					if( sn != null )
					{
						obj.SNState = (sn[0] as Simulation).ProductStatus;
					}
					else
					{
						obj.SNState = ProductStatus.OffLine;
					}
				}
			}

			return objs;
		}

		public int QueryECNDetailsCount(
			string ecn)
		{
#if DEBUG
			Log.Info(
				new SQLCondition(
				string.Format(
				@"select count(*) from (select distinct {0} from TBLONWIPECN where ECNNO = '{1}')",
				"RCARD,OPCODE,SSCODE,RESCODE",
				ecn.ToUpper())).SQLText);
#endif
			return this.DataProvider.GetCount(
				new SQLCondition(
				string.Format(
				@"select count(*) from (select distinct {0} from TBLONWIPECN where ECNNO = '{1}')",
				"RCARD,OPCODE,SSCODE,RESCODE",
				ecn.ToUpper())));
		}
	}
}
