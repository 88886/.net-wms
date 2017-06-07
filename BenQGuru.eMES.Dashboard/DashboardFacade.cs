using System;

using UserControl;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Common.MutiLanguage; 
using BenQGuru.eMES.Common.Helper;  
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper; 

namespace BenQGuru.eMES.Dashboard
{
	/// <summary>
	/// DashboardFactory ��ժҪ˵����
	/// </summary>
	public class DashboardFacade
	{
		public const string  SOCR = "SOCR";

		public const string ByMonth = "MONTH";
		public const string ByDay = "DAY";
		public const string ByWeek = "WEEK";
		//public static string  SOCR = "SOCR";

		private  IDomainDataProvider _domainDataProvider = null;
		public IDomainDataProvider DataProvider
		{
			get
			{
				//				if (_domainDataProvider == null)
				//				{
				//					_domainDataProvider = DomainDataProviderManager.DomainDataProvider(); 
				//				}

				return _domainDataProvider;
			}	
		}

		public DashboardFacade(IDomainDataProvider dataProvider)
		{
			_domainDataProvider = dataProvider;
		}


		#region ������ʱ�ʿ���
		public string getSOCR(
			string modelCode
			,string itemCode
			,string orderCode
			,string frmDate
			,string toDate
			,string frmMonth
			,string toMonth
			,string frmWeek
			,string toWeek
			,string statisticlatitude)
		{
			string strReturn = String.Empty;

			#region Contract Total SQL String

			string sqlCondition = String.Empty;

			string sqlMain = @"SELECT   b.*,
         CASE 
             WHEN a.planshipdate >= a.actshipdate AND a.actshipdate IS NOT NULL 
                THEN 'OnTimeShip' 
             WHEN a.planshipdate < a.actshipdate AND a.actshipdate IS NOT NULL  
                THEN 'NotOnTimeShip' 
             WHEN a.actshipdate IS NULL 
                THEN 'NotOnTimeShip' 
          END AS shiptype,
         a.planshipdate, a.actshipdate,
         CASE
            WHEN a.planshipdate = 99991231
               THEN '0'
            WHEN LENGTH (a.planshipdate) < 8
               THEN '0'
            ELSE TO_CHAR (TO_DATE (a.planshipdate, 'yyyyMMdd'),
                          'ww'
                         )
         END AS planshipweek,
         CASE
            WHEN a.planshipdate = 99991231
               THEN '0'
            WHEN LENGTH (a.planshipdate) < 8
               THEN '0'
            ELSE TO_CHAR (TO_DATE (a.planshipdate, 'yyyyMMdd'),
                          'MM'
                         )
         END AS planshipmonth
    FROM tblorder a, tblorderdetail b
   WHERE a.ordernumber = b.ordernumber {0}
ORDER BY a.planshipdate";

			//			string sqlMain = @"SELECT   shiptype, COUNT (shiptype) AS ordercount,
			//         COUNT (*) AS totalordercount, SUM (actqty) AS shipqty,
			//         SUM (planqty) AS totoalqty,
			//         ROUND (COUNT (shiptype) / COUNT (*), 2) * 100 AS PERCENT
			//    FROM (SELECT (CASE
			//                     WHEN a.planshipdate >= b.actdate
			//                          AND b.actdate IS NOT NULL
			//                        THEN " + MutiLanguages.ParserMessage("OnTimeShip") + @"      /*�ƻ����ڴ��ڵ���ʵ��������ڣ���ʱ����*/
			//                     WHEN a.planshipdate < b.actdate AND b.actdate IS NOT NULL
			//                        THEN " + MutiLanguages.ParserMessage("NotOnTimeShip") + @"            /*�ƻ�����С��������ڣ�δ��ʱ����*/
			//                     WHEN b.actdate IS NULL
			//                        THEN "+ MutiLanguages.ParserMessage("NotOnTimeShip") + @"            /*�ƻ�����С��������ڣ�δ��ʱ����*/
			//                  END
			//                 ) AS shiptype,
			//                 a.planshipdate, b.*
			//            FROM tblorder a, tblorderdetail b
			//           WHERE a.ordernumber = b.ordernumber {0}) s
			//GROUP BY shiptype";

			//����
			if(modelCode != null && modelCode != String.Empty)
			{
				sqlCondition += " and itemCode in (select itemcode from tblmodel2item where modelcode='" + modelCode + "'" + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ")";
			}
			//��Ʒ
			if(itemCode != null && itemCode != String.Empty)
			{
				sqlCondition += " and itemCode = '" + itemCode + "'";
			}
			//������
			if(orderCode != null && orderCode != String.Empty)
			{
				sqlCondition += " and  OrderNumber = '" + orderCode + "'";
			}
			//��ʼ���ڣ��ƻ��������ڣ�
			if(frmDate != null && frmDate != String.Empty)
			{
				sqlCondition += " and A.PlanShipDate >= " + frmDate;
			}
			//�������ڣ��ƻ��������ڣ�
			if(toDate != null && toDate != String.Empty)
			{
				sqlCondition += " and A.PlanShipDate <= " + toDate;
			}
			//��ʼʵ�ʳ����·�
			if(frmMonth != null && frmMonth != String.Empty)
			{
				sqlCondition += " and A.ActShipMonth >= " + frmMonth;
			}
			//����ʵ�ʳ����·�
			if(toMonth != null && toMonth != String.Empty)
			{
				sqlCondition += " and A.ActShipMonth <= " + toMonth ;
			}
			//��ʼʵ�ʳ�������
			if(frmWeek != null && frmWeek != String.Empty)
			{
				sqlCondition += " and A.ActShipWeek <= " + frmWeek ;
			}
			//��ʼʵ�ʳ�������
			if(toWeek != null && toWeek != String.Empty)
			{
				sqlCondition += " and A.ActShipWeek <= " + toWeek;
			}

			string sql = String.Format(sqlMain,sqlCondition);
			#endregion

			object[] objs = DataProvider.CustomQuery(typeof(SOCRTOTALObject)
				,new SQLCondition(sql));

			System.Collections.Hashtable htTotal = new System.Collections.Hashtable();

			if(objs != null && objs.Length > 0)
			{
				foreach(SOCRTOTALObject obj in objs)
				{
					//����Total Object
					if(htTotal.ContainsKey(obj.ItemType))
					{
						#region �Ѿ�����Key=obj.ItemType��TotalObject����
						TotalObject to = (TotalObject)htTotal[obj.ItemType];

						to.ItemType = obj.ItemType;
						
						to.OrderCount = (double)objs.Length;

						#region ����DateShip

						if(to.DailyShips == null)
						{
							to.DailyShips = new System.Collections.Hashtable();

							DailyShip ds = new DailyShip();
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.ActShipDate = obj.PlanShipDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.ActShipDate = obj.PlanShipWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.ActShipDate = obj.PlanShipMonth;
									break;
								}
							}
							ds.OrderCount = 1;	

							//							if(obj.ItemType == TotalObject.OnTimeShip)
							//							{
								
							to.ShippedOrderCount = to.ShippedOrderCount  + 1;
							//							}
							//����ShipDetails
							if(ds.ShipDetails == null)
							{
								ds.ShipDetails = new System.Collections.ArrayList();
							}

							ds.ShipDetails.Add(obj);

							to.DailyShips.Add(ds.ActShipDate + obj.ItemType,ds);
						}
						else
						{
							string byObject = String.Empty;
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									byObject = obj.PlanShipDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									byObject = obj.PlanShipWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									byObject = obj.PlanShipMonth;
									break;
								}
							}

							if(to.DailyShips.ContainsKey(byObject + obj.ItemType))
							{
								DailyShip ds = (DailyShip)to.DailyShips[byObject + obj.ItemType];
								//Laws Lu,2006/04/26 ������ͬγ��ͳ��
								switch(statisticlatitude)
								{
									case DashboardFacade.ByDay:
									{
										ds.ActShipDate = obj.PlanShipDate;
										break;
									}
									case DashboardFacade.ByWeek:
									{
										ds.ActShipDate = obj.PlanShipWeek;
										break;
									}
									case DashboardFacade.ByMonth:
									{
										ds.ActShipDate = obj.PlanShipMonth;
										break;
									}
								}
								ds.OrderCount = ds.OrderCount  + 1;

								//								if(obj.ItemType == TotalObject.OnTimeShip)
								//								{
								to.ShippedOrderCount = to.ShippedOrderCount  + 1;
								//								}
								//����ShipDetails
								if(ds.ShipDetails == null)
								{
									ds.ShipDetails = new System.Collections.ArrayList();
								}

								ds.ShipDetails.Add(obj);

								to.DailyShips[ds.ActShipDate + obj.ItemType] = ds;
							}
							else
							{
								DailyShip ds = new DailyShip();//(DailyShip)to.DailyShips[ds.ActShipDate + obj.ItemType];

								ds.OrderCount = 1;
								//Laws Lu,2006/04/26 ������ͬγ��ͳ��
								switch(statisticlatitude)
								{
									case DashboardFacade.ByDay:
									{
										ds.ActShipDate = obj.PlanShipDate;
										break;
									}
									case DashboardFacade.ByWeek:
									{
										ds.ActShipDate = obj.PlanShipWeek;
										break;
									}
									case DashboardFacade.ByMonth:
									{
										ds.ActShipDate = obj.PlanShipMonth;
										break;
									}
								}
								//								if(to.ItemType == TotalObject.OnTimeShip)
								//								{
								to.ShippedOrderCount = to.ShippedOrderCount  + 1;
								//								}
								//����ShipDetails
								if(ds.ShipDetails == null)
								{
									ds.ShipDetails = new System.Collections.ArrayList();
								}

								ds.ShipDetails.Add(obj);

								to.DailyShips.Add(ds.ActShipDate + obj.ItemType,ds);
							}
						}

						#endregion

						to.Scale = System.Math.Round(to.ShippedOrderCount/to.OrderCount,2) * 100;

						htTotal[obj.ItemType] = to;
						#endregion
					}
					else
					{
						#region ������ʱ�Ĵ���
						TotalObject to = new TotalObject();
					
						to.ItemType = obj.ItemType;
						to.OrderCount = (double)objs.Length;
						
						#region ����DateShip

						if(to.DailyShips == null)
						{
							to.DailyShips = new System.Collections.Hashtable();

							DailyShip ds = new DailyShip();
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.ActShipDate = obj.PlanShipDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.ActShipDate = obj.PlanShipWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.ActShipDate = obj.PlanShipMonth;
									break;
								}
							}
							ds.OrderCount = 1;	

							//							if(obj.ItemType == TotalObject.OnTimeShip)
							//							{
							to.ShippedOrderCount = to.ShippedOrderCount  + 1;
							//							}
							//����ShipDetails
							if(ds.ShipDetails == null)
							{
								ds.ShipDetails = new System.Collections.ArrayList();
							}

							ds.ShipDetails.Add(obj);

							to.DailyShips.Add(ds.ActShipDate + obj.ItemType,ds);
						}
						else
						{
							string byObject = String.Empty;
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									byObject = obj.PlanShipDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									byObject = obj.PlanShipWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									byObject = obj.PlanShipMonth;
									break;
								}
							}


							DailyShip ds = (DailyShip)to.DailyShips[byObject];
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.ActShipDate = obj.PlanShipDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.ActShipDate = obj.PlanShipWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.ActShipDate = obj.PlanShipMonth;
									break;
								}
							}
							ds.OrderCount = ds.OrderCount  + 1;

							//							if(to.ItemType == TotalObject.OnTimeShip)
							//							{
							to.ShippedOrderCount = to.ShippedOrderCount  + 1;
							//							}
							//����ShipDetails
							if(ds.ShipDetails == null)
							{
								ds.ShipDetails = new System.Collections.ArrayList();
							}

							ds.ShipDetails.Add(obj);

							to.DailyShips.Add(ds.ActShipDate + obj.ItemType,ds);
						}

						#endregion

						to.Scale = System.Math.Round(to.ShippedOrderCount/to.OrderCount,2) * 100;

						htTotal.Add(obj.ItemType,to);
						#endregion
					}
				}
			}

			SOCRXmlBuilder xml = new SOCRXmlBuilder();
			xml.BeginBuildRoot();
			//д�����
			foreach(TotalObject to in htTotal.Values)
			{
				xml.BeginBuildItem(to.ItemType,to.ShippedOrderCount.ToString(),to.OrderCount.ToString(),to.Scale.ToString());
				//д��������
				foreach(DailyShip ds in to.DailyShips.Values)
				{
					xml.BeginBuildDateShip(ds.ActShipDate,ds.OrderCount.ToString());
					//д��������
					foreach(SOCRTOTALObject obj in ds.ShipDetails)
					{
						xml.BuildDateShipDetail(obj.ItemCode,obj.OrderNO,obj.PartnerCode,obj.PlanShipDate,obj.ActDate,obj.ActQty);
					}
					//Flex��bug�����������޷���ʾ
					if(ds.ShipDetails.Count == 1)
					{
						xml.BuildDateShipDetail(String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
					}

					xml.EndBuildDateShip();
				}
				//Flex��bug�����������޷���ʾ
				if(to.DailyShips.Values.Count == 1)
				{
					xml.BeginBuildDateShip(String.Empty,String.Empty);
					xml.EndBuildDateShip();
				}

				xml.EndBuildItem();
			}
			//Flex��bug�����������޷���ʾ
			if(htTotal.Values.Count == 1)
			{	
				xml.BeginBuildItem(String.Empty,String.Empty,String.Empty,String.Empty);
				xml.EndBuildItem();
			}

			xml.EndBuildRoot();

			strReturn = xml.XmlContent.ToString();

			return strReturn;
		}
		#endregion

		#region ������ʱ�����
		public string getMPOCR(
			string modelCode
			,string itemCode
			,string moCode
			,string frmDate
			,string toDate
			,string frmMonth
			,string toMonth
			,string frmWeek
			,string toWeek
			,string statisticlatitude)
		{
			string strReturn = String.Empty;

			#region Contract Total SQL String

			string sqlCondition = String.Empty;

			string sqlMain = @"/* Formatted on 2006/04/27 16:01 (Formatter Plus v4.8.6) */
SELECT *
  FROM (SELECT b.*,
               CASE
                  WHEN b.moplanenddate = 99991231
                     THEN '0'
                  WHEN LENGTH (b.moplanenddate) < 8
                     THEN '0'
                  ELSE TO_CHAR (TO_DATE (b.moplanenddate, 'yyyyMMdd'),
                                'ww'
                               )
               END AS planendweek,
               CASE
                  WHEN b.moplanenddate = 99991231
                     THEN '0'
                  WHEN LENGTH (b.moplanenddate) < 8
                     THEN '0'
                  ELSE TO_CHAR (TO_DATE (b.moplanenddate, 'yyyyMMdd'),
                                'MM'
                               )
               END AS planendmonth,
               CASE
                  WHEN b.moplanenddate >= b.moactenddate
                     THEN 'OnTimeProduct'
                  ELSE 'NotOnTimeProduct'
               END AS moprodcutstatus
          FROM tblmo b) a
 WHERE a.planendweek <> 0 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " {0} order by moplanenddate";

			//����
			if(modelCode != null && modelCode != String.Empty)
			{
				sqlCondition += " and itemCode in (select itemcode from tblmodel2item where modelcode='" + modelCode + "'" + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ")";
			}
			//��Ʒ
			if(itemCode != null && itemCode != String.Empty)
			{
				sqlCondition += " and itemCode = '" + itemCode + "'";
			}
			//������
			if(moCode != null && moCode != String.Empty)
			{
				sqlCondition += " and  mocode = '" + moCode + "'";
			}
			//��ʼ���ڣ��ƻ��������ڣ�
			if(frmDate != null && frmDate != String.Empty)
			{
				sqlCondition += " and A.MoPlanEndDate >= " + frmDate;
			}
			//�������ڣ��ƻ��������ڣ�
			if(toDate != null && toDate != String.Empty)
			{
				sqlCondition += " and A.MoPlanEndDate <= " + toDate;
			}
			//��ʼʵ�ʳ����·�
			if(frmMonth != null && frmMonth != String.Empty)
			{
				sqlCondition += " and A.ActEndMonth >= " + frmMonth;
			}
			//����ʵ�ʳ����·�
			if(toMonth != null && toMonth != String.Empty)
			{
				sqlCondition += " and A.ActEndMonth <= " + toMonth ;
			}
			//��ʼʵ�ʳ�������
			if(frmWeek != null && frmWeek != String.Empty)
			{
				sqlCondition += " and A.ActEndWeek <= " + frmWeek ;
			}
			//��ʼʵ�ʳ�������
			if(toWeek != null && toWeek != String.Empty)
			{
				sqlCondition += " and A.ActEndWeek <= " + toWeek;
			}

			string sql = String.Format(sqlMain,sqlCondition);
			#endregion

			object[] objs = DataProvider.CustomQuery(typeof(MPOCRObject)
				,new SQLCondition(sql));

			System.Collections.Hashtable htTotal = new System.Collections.Hashtable();

			if(objs != null && objs.Length > 0)
			{
				foreach(MPOCRObject obj in objs)
				{
					//����Total Object
					if(htTotal.ContainsKey(obj.MoProductStatus))
					{
						#region �Ѿ�����Key=obj.MoProductStatus��MoObject����
						MoObject to = (MoObject)htTotal[obj.MoProductStatus];

						to.MoProductStatus = obj.MoProductStatus;
						
						to.TotalMoCount = (double)objs.Length;

						#region ����DailyMo

						if(to.DailyMos == null)
						{
							to.DailyMos = new System.Collections.SortedList();

							DailyMo ds = new DailyMo();
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.CountScale = obj.PlanEndDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.CountScale = obj.PlanEndWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.CountScale = obj.PlanEndMonth;
									break;
								}
							}
							ds.MoCount = 1;	

							//							if(obj.MoProductStatus == MoObject.OnTimeShip)
							//							{
								
							to.MoCount = to.MoCount  + 1;
							//							}
							//����ShipDetails
							if(ds.MoDetails == null)
							{
								ds.MoDetails = new System.Collections.ArrayList();
							}

							ds.MoDetails.Add(obj);

							to.DailyMos.Add(ds.CountScale + obj.MoProductStatus,ds);
						}
						else
						{
							string byObject = String.Empty;
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									byObject = obj.PlanEndDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									byObject = obj.PlanEndWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									byObject = obj.PlanEndMonth;
									break;
								}
							}

							if(to.DailyMos.ContainsKey(byObject + obj.MoProductStatus))
							{
								DailyMo ds = (DailyMo)to.DailyMos[byObject + obj.MoProductStatus];
								//Laws Lu,2006/04/26 ������ͬγ��ͳ��
								switch(statisticlatitude)
								{
									case DashboardFacade.ByDay:
									{
										ds.CountScale = obj.PlanEndDate;
										break;
									}
									case DashboardFacade.ByWeek:
									{
										ds.CountScale = obj.PlanEndWeek;
										break;
									}
									case DashboardFacade.ByMonth:
									{
										ds.CountScale = obj.PlanEndMonth;
										break;
									}
								}
								ds.MoCount = ds.MoCount  + 1;

								//								if(obj.MoProductStatus == MoObject.OnTimeShip)
								//								{
								to.MoCount = to.MoCount  + 1;
								//								}
								//����ShipDetails
								if(ds.MoDetails == null)
								{
									ds.MoDetails = new System.Collections.ArrayList();
								}

								ds.MoDetails.Add(obj);

								to.DailyMos[ds.CountScale + obj.MoProductStatus] = ds;
							}
							else
							{
								DailyMo ds = new DailyMo();//(DailyMo)to.DailyMos[ds.CountScale + obj.MoProductStatus];

								ds.MoCount = 1;
								//Laws Lu,2006/04/26 ������ͬγ��ͳ��
								switch(statisticlatitude)
								{
									case DashboardFacade.ByDay:
									{
										ds.CountScale = obj.PlanEndDate;
										break;
									}
									case DashboardFacade.ByWeek:
									{
										ds.CountScale = obj.PlanEndWeek;
										break;
									}
									case DashboardFacade.ByMonth:
									{
										ds.CountScale = obj.PlanEndMonth;
										break;
									}
								}
								//								if(to.MoProductStatus == MoObject.OnTimeShip)
								//								{
								to.MoCount = to.MoCount  + 1;
								//								}
								//����ShipDetails
								if(ds.MoDetails == null)
								{
									ds.MoDetails = new System.Collections.ArrayList();
								}

								ds.MoDetails.Add(obj);

								to.DailyMos.Add(ds.CountScale + obj.MoProductStatus,ds);
							}
						}

						#endregion

						to.Scale = System.Math.Round(to.MoCount/to.TotalMoCount,2) * 100;

						htTotal[obj.MoProductStatus] = to;
						#endregion
					}
					else
					{
						#region ������ʱ�Ĵ���
						MoObject to = new MoObject();
					
						to.MoProductStatus = obj.MoProductStatus;
						to.TotalMoCount = (double)objs.Length;
						
						#region ����DailyMo

						if(to.DailyMos == null)
						{
							to.DailyMos = new System.Collections.SortedList();

							DailyMo ds = new DailyMo();
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.CountScale = obj.PlanEndDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.CountScale = obj.PlanEndWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.CountScale = obj.PlanEndMonth;
									break;
								}
							}
							ds.MoCount = 1;	

							//							if(obj.MoProductStatus == MoObject.OnTimeShip)
							//							{
							to.MoCount = to.MoCount  + 1;
							//							}
							//����ShipDetails
							if(ds.MoDetails == null)
							{
								ds.MoDetails = new System.Collections.ArrayList();
							}

							ds.MoDetails.Add(obj);

							to.DailyMos.Add(ds.CountScale + obj.MoProductStatus,ds);
						}
						else
						{
							string byObject = String.Empty;
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									byObject = obj.PlanEndDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									byObject = obj.PlanEndWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									byObject = obj.PlanEndMonth;
									break;
								}
							}


							DailyMo ds = (DailyMo)to.DailyMos[byObject];
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.CountScale = obj.PlanEndDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.CountScale = obj.PlanEndWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.CountScale = obj.PlanEndMonth;
									break;
								}
							}
							ds.MoCount = ds.MoCount  + 1;

							//							if(to.MoProductStatus == MoObject.OnTimeShip)
							//							{
							to.MoCount = to.MoCount  + 1;
							//							}
							//����ShipDetails
							if(ds.MoDetails == null)
							{
								ds.MoDetails = new System.Collections.ArrayList();
							}

							ds.MoDetails.Add(obj);

							to.DailyMos.Add(ds.CountScale + obj.MoProductStatus,ds);
						}

						#endregion

						to.Scale = System.Math.Round(to.MoCount/to.TotalMoCount,2) * 100;

						htTotal.Add(obj.MoProductStatus,to);
						#endregion
					}
				}
			}

			MPOCRXmlBuilder xml = new MPOCRXmlBuilder();
			xml.BeginBuildRoot();
			//д�����
			foreach(MoObject to in htTotal.Values)
			{
				xml.BeginBuildItem(to.MoProductStatus,to.MoCount.ToString(),to.TotalMoCount.ToString(),to.Scale.ToString());
				//д��������
				foreach(DailyMo ds in to.DailyMos.Values)
				{
					xml.BeginBuildDateMo(ds.CountScale,ds.MoCount.ToString());
					//д��������
					foreach(MPOCRObject obj in ds.MoDetails)
					{
						xml.BuildDateMoDetail(obj.ItemCode,obj.MoCode,obj.PlanEndDate,obj.ActEndDate,obj.OutPutQty);
					}
					//Flex��bug�����������޷���ʾ
					if(ds.MoDetails.Count == 1)
					{
						xml.BuildDateMoDetail(String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
					}
					xml.EndBuildDateMo();
				}
				//Flex��bug�����������޷���ʾ
				if(to.DailyMos.Values.Count == 1)
				{
					xml.BeginBuildDateMo(String.Empty,String.Empty);
					xml.EndBuildDateMo();
				}
				xml.EndBuildItem();
			}
			if(htTotal.Values.Count == 1)
			{	
				xml.BeginBuildItem(String.Empty,String.Empty,String.Empty,String.Empty);
				xml.EndBuildItem();
			}
			xml.EndBuildRoot();

			strReturn = xml.XmlContent.ToString();

			return strReturn;
		}
		#endregion

		#region MTTR����
		//TODO:��δ���
		public string getMTTR(
			string modelCode
			,string itemCode
			,string resCode
			,string frmDate
			,string toDate
			,string frmMonth
			,string toMonth
			,string frmWeek
			,string toWeek
			,string statisticlatitude
			,string filterfield)
		{
			string strReturn = String.Empty;

			#region Contract Total SQL String

			string sqlCondition = String.Empty;


			string sqlMain = @"SELECT   a.*,(case when a.confirmdate>0 then to_char(to_date(a.confirmdate,'yyyyMMdd'),'ww') else '0' end) as confirmweek,(case when a.confirmdate>0 then to_char(to_date(a.confirmdate,'yyyyMMdd'),'MM') else '0' end) as confirmmonth
    FROM (SELECT
                   round((CASE
                       WHEN confirmdate <= tsdate AND tstime IS NOT NULL AND confirmtime IS NOT NULL AND tsdate - confirmdate < 1
                          THEN   TO_DATE (to_char(tstime,'000000'), 'HH24MISS') - TO_DATE (to_char(confirmtime,'000000'), 'HH24MISS')
                       WHEN confirmdate <= tsdate AND tsdate - confirmdate >= 1 AND tstime IS NOT NULL AND confirmtime IS NOT NULL
                          THEN   TO_DATE (TO_CHAR (tsdate) || TO_CHAR (tstime,'000000'),
                                          'yyyyMMddHH24MISS'
                                         )
                               - TO_DATE (   TO_CHAR (confirmdate)
                                          || TO_CHAR (confirmtime,'000000'),
                                          'yyyyMMddHH24MISS'
                                         )
                       ELSE 0
                    END
                   )
                 * 24
                 * 60,0) AS tsspantime,
                 modelcode,itemcode,tsrescode,rcard,confirmdate,tsdate
            FROM tblts b
           WHERE tsstatus IN ('tsstatus_reflow', 'tsstatus_complete')) a
ORDER BY a.confirmdate";

			//����
			if(modelCode != null && modelCode != String.Empty)
			{
				sqlCondition += " and itemCode in (select itemcode from tblmodel2item where modelcode='" + modelCode + "'" + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ")";
			}
			//��Ʒ
			if(itemCode != null && itemCode != String.Empty)
			{
				sqlCondition += " and itemCode = '" + itemCode + "'";
			}
			//������
			if(resCode != null && resCode != String.Empty)
			{
				sqlCondition += " and  tsrescode = '" + resCode + "'";
			}
			//��ʼ���ڣ��ƻ��������ڣ�
			if(frmDate != null && frmDate != String.Empty)
			{
				sqlCondition += " and A.confirmdate >= " + frmDate;
			}
			//�������ڣ��ƻ��������ڣ�
			if(toDate != null && toDate != String.Empty)
			{
				sqlCondition += " and A.confirmdate <= " + toDate;
			}
			//��ʼʵ�ʳ����·�
			if(frmMonth != null && frmMonth != String.Empty)
			{
				sqlCondition += " and A.confirmmonth >= " + frmMonth;
			}
			//����ʵ�ʳ����·�
			if(toMonth != null && toMonth != String.Empty)
			{
				sqlCondition += " and A.confirmmonth <= " + toMonth ;
			}
			//��ʼʵ�ʳ�������
			if(frmWeek != null && frmWeek != String.Empty)
			{
				sqlCondition += " and A.confirmweek <= " + frmWeek ;
			}
			//��ʼʵ�ʳ�������
			if(toWeek != null && toWeek != String.Empty)
			{
				sqlCondition += " and A.confirmweek <= " + toWeek;
			}

			string sql = String.Format(sqlMain,sqlCondition);
			#endregion

			object[] objs = DataProvider.CustomQuery(typeof(MTTRQueryObject)
				,new SQLCondition(sql));

			System.Collections.Hashtable htTotal = new System.Collections.Hashtable();

			if(objs != null && objs.Length > 0)
			{

				double mttr = 0;
				foreach(MTTRQueryObject obj in objs)
				{
					string objGroupType = String.Empty;

					switch(filterfield)
					{
						case "MODELCODE":
						{
							objGroupType = obj.ModelCode;
							break;
						}
						case "ITEMCODE":
						{
							objGroupType = obj.ItemCode;
							break;
						}
						case "RESCODE":
						{
							objGroupType = obj.ResourceCode;
							break;
						}
					}
					//����Total Object
					if(htTotal.ContainsKey(objGroupType))
					{
						#region �Ѿ�����Key=objGroupType��MTTRObject����
						MTTRObject mto = (MTTRObject)htTotal[objGroupType];

						mto.FieldCode = objGroupType;
						
						mto.TSQty = mto.TSQty + 1;

						//mto.MTTR

						#region ����SubMTTR Details

						if(mto.DailyMTTRs == null)
						{
							mto.DailyMTTRs = new System.Collections.SortedList();

							SubMTTR ds = new SubMTTR();
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.Date = obj.ConfrimDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.Date = obj.ConfrimWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.Date = obj.ConfrimMonth;
									break;
								}
							}
							ds.FieldCode = mto.FieldCode;
							ds.TSQty = 1;	

							ds.TTR = ds.TTR + Convert.ToDouble(obj.TTR);

							//							if(objGroupType == MTTRObject.OnTimeShip)
							//							{
								
							//mto.ShippedOrderCount = mto.ShippedOrderCount  + 1;
							//							}
							//����TTRDetails
							if(ds.TTRDetails == null)
							{
								ds.TTRDetails = new System.Collections.ArrayList();
							}

							ds.TTRDetails.Add(obj);

							ds.MTTR = System.Math.Round(ds.TTR / ds.TSQty,0);

							mto.DailyMTTRs.Add(ds.Date + objGroupType,ds);
						}
						else
						{
							string byObject = String.Empty;
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									byObject = obj.ConfrimDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									byObject = obj.ConfrimWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									byObject = obj.ConfrimMonth;
									break;
								}
							}

							if(mto.DailyMTTRs.ContainsKey(byObject + objGroupType))
							{
								SubMTTR ds = (SubMTTR)mto.DailyMTTRs[byObject + objGroupType];
								//Laws Lu,2006/04/26 ������ͬγ��ͳ��
								switch(statisticlatitude)
								{
									case DashboardFacade.ByDay:
									{
										ds.Date = obj.ConfrimDate;
										break;
									}
									case DashboardFacade.ByWeek:
									{
										ds.Date = obj.ConfrimWeek;
										break;
									}
									case DashboardFacade.ByMonth:
									{
										ds.Date = obj.ConfrimMonth;
										break;
									}
								}
								ds.FieldCode = mto.FieldCode;
								ds.TSQty = ds.TSQty  + 1;
								ds.TTR = ds.TTR + Convert.ToDouble(obj.TTR);

								//								if(objGroupType == MTTRObject.OnTimeShip)
								//								{
								//mto.ShippedOrderCount = mto.ShippedOrderCount  + 1;
								//								}
								//����TTRDetails
								if(ds.TTRDetails == null)
								{
									ds.TTRDetails = new System.Collections.ArrayList();
								}

								ds.TTRDetails.Add(obj);

								ds.MTTR = System.Math.Round(ds.TTR / ds.TSQty,0);

								mto.DailyMTTRs[ds.Date + objGroupType] = ds;
							}
							else
							{
								SubMTTR ds = new SubMTTR();//(SubMTTR)mto.DailyMTTRs[ds.Date + objGroupType];
								ds.FieldCode = mto.FieldCode;
								ds.TSQty = 1;
								ds.TTR = ds.TTR + Convert.ToDouble(obj.TTR);
								//Laws Lu,2006/04/26 ������ͬγ��ͳ��
								switch(statisticlatitude)
								{
									case DashboardFacade.ByDay:
									{
										ds.Date = obj.ConfrimDate;
										break;
									}
									case DashboardFacade.ByWeek:
									{
										ds.Date = obj.ConfrimWeek;
										break;
									}
									case DashboardFacade.ByMonth:
									{
										ds.Date = obj.ConfrimMonth;
										break;
									}
								}
								//								if(mto.FieldCode == MTTRObject.OnTimeShip)
								//								{
								ds.MTTR = System.Math.Round(ds.TTR / ds.TSQty,0);
								//								}
								//����TTRDetails
								if(ds.TTRDetails == null)
								{
									ds.TTRDetails = new System.Collections.ArrayList();
								}

								ds.TTRDetails.Add(obj);

								mto.DailyMTTRs.Add(ds.Date + objGroupType,ds);
							}
						}

						#endregion
						
						mto.TTR = Convert.ToDouble(obj.TTR) + mto.TTR;

						mto.MTTR = System.Math.Round(Convert.ToDouble(mto.TTR)/mto.TSQty,0);

						mttr = mttr + mto.MTTR;

						mto.TotalMTTR = System.Math.Round(mttr/objs.Length,0);

						htTotal[objGroupType] = mto;
						#endregion
					}
					else
					{
						#region ������ʱ�Ĵ���
						MTTRObject mto = new MTTRObject();
					
						mto.FieldCode = objGroupType;
						
						mto.TSQty = mto.TSQty + 1;
						
						#region ����SubMTTR Details

						if(mto.DailyMTTRs == null)
						{
							mto.DailyMTTRs = new System.Collections.SortedList();

							SubMTTR ds = new SubMTTR();
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.Date = obj.ConfrimDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.Date = obj.ConfrimWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.Date = obj.ConfrimMonth;
									break;
								}
							}
							ds.FieldCode = mto.FieldCode;
							ds.TSQty = 1;	
							ds.TTR = ds.TTR +Convert.ToDouble(obj.TTR);

							//							if(objGroupType == MTTRObject.OnTimeShip)
							//							{
							ds.MTTR = System.Math.Round(ds.TTR / ds.TSQty,0);
							//							}
							//����TTRDetails
							if(ds.TTRDetails == null)
							{
								ds.TTRDetails = new System.Collections.ArrayList();
							}

							ds.TTRDetails.Add(obj);

							mto.DailyMTTRs.Add(ds.Date + objGroupType,ds);
						}
						else
						{
							string byObject = String.Empty;
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									byObject = obj.ConfrimDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									byObject = obj.ConfrimWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									byObject = obj.ConfrimMonth;
									break;
								}
							}


							SubMTTR ds = (SubMTTR)mto.DailyMTTRs[byObject];
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.Date = obj.ConfrimDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.Date = obj.ConfrimWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.Date = obj.ConfrimMonth;
									break;
								}
							}
							ds.FieldCode = mto.FieldCode;
							ds.TSQty = ds.TSQty  + 1;
							ds.TTR = ds.TTR +Convert.ToDouble(obj.TTR);

							//							if(mto.FieldCode == MTTRObject.OnTimeShip)
							//							{
							ds.MTTR = System.Math.Round(ds.TTR / ds.TSQty,0);
							//							}
							//����TTRDetails
							if(ds.TTRDetails == null)
							{
								ds.TTRDetails = new System.Collections.ArrayList();
							}

							ds.TTRDetails.Add(obj);

							mto.DailyMTTRs.Add(ds.Date + objGroupType,ds);
						}

						#endregion

						mto.TTR = Convert.ToDouble(obj.TTR) + mto.TTR;

						mto.MTTR = System.Math.Round(Convert.ToDouble(mto.TTR)/mto.TSQty,0);

						mttr = mttr + mto.MTTR;

						mto.TotalMTTR =  System.Math.Round(mttr/objs.Length,0);

						htTotal.Add(objGroupType,mto);
						#endregion
					}
				}
			}

			MTTRXmlBuilder xml = new MTTRXmlBuilder();
			xml.BeginBuildRoot();
			//д�����
			foreach(MTTRObject to in htTotal.Values)
			{
				xml.BeginBuildItem(to.FieldCode,to.MTTR.ToString(),to.TotalMTTR.ToString(),to.TSQty.ToString());
				//д��������
				foreach(SubMTTR ds in to.DailyMTTRs.Values)
				{
					xml.BeginBuildDateMTTR(ds.Date,ds.FieldCode,ds.TSQty.ToString(),ds.MTTR.ToString());
					//д��������
					foreach(MTTRQueryObject obj in ds.TTRDetails)
					{
						xml.BuildDateMTTRDetail(obj.ItemCode,obj.ModelCode,obj.ResourceCode,obj.SN,obj.ConfrimDate,obj.CompleteDate,obj.TTR);
					}
					//Flex��bug�����������޷���ʾ
					if(ds.TTRDetails.Count == 1)
					{
						xml.BuildDateMTTRDetail(String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
					}

					xml.EndBuildDateMTTR();
				}
				//Flex��bug�����������޷���ʾ
				if(to.DailyMTTRs.Values.Count == 1)
				{
					xml.BeginBuildDateMTTR(String.Empty,String.Empty,String.Empty,String.Empty);
					xml.EndBuildDateMTTR();
				}

				xml.EndBuildItem();
			}
			//Flex��bug�����������޷���ʾ
			if(htTotal.Values.Count == 1)
			{	
				xml.BeginBuildItem(String.Empty,String.Empty,String.Empty,String.Empty);
				xml.EndBuildItem();
			}

			xml.EndBuildRoot();

			strReturn = xml.XmlContent.ToString();

			return strReturn;
		}
		#endregion

		#region MTBF ƽ���޹���ʱ��

		public string getMTBF(
			string modelCode
			,string itemCode
			,string ssCode
			,string frmDate
			,string toDate
			,string frmMonth
			,string toMonth
			,string frmWeek
			,string toWeek
			,string statisticlatitude
			,string filterfield)
		{
			string strReturn = String.Empty;


			#region Contract Total SQL String

			string sqlCondition = String.Empty;


			string sqlMain = @"SELECT d.*, (SELECT MAX (mtime)
										FROM tblonwip c
										WHERE c.shiftday = d.shiftday) AS endtime
							FROM (SELECT rcard, modelcode, itemcode, sscode, shiftday,
										TO_CHAR (TO_DATE (shiftday, 'yyyyMMdd'), 'ww') AS week,
										TO_CHAR (TO_DATE (shiftday, 'yyyyMMdd'), 'MM') AS MONTH,
										(SELECT MIN (mtime)
											FROM tblonwip b
											WHERE b.shiftday = a.shiftday) AS begintime
									FROM tblonwip a
									WHERE action = 'NG' {0} ) d";

			//����
			if(modelCode != null && modelCode != String.Empty)
			{
				sqlCondition += " and itemCode in (select itemcode from tblmodel2item where modelcode='" + modelCode + "'" + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ")";
			}
			//��Ʒ
			if(itemCode != null && itemCode != String.Empty)
			{
				sqlCondition += " and itemCode = '" + itemCode + "'";
			}
			//���ߴ���
			if(ssCode != null && ssCode != String.Empty)
			{
				sqlCondition += " and  sscode = '" + ssCode + "'";
			}
			//��ʼ���ڣ��ƻ��������ڣ�
			if(frmDate != null && frmDate != String.Empty)
			{
				sqlCondition += " and A.shiftday >= " + frmDate;
			}
			//�������ڣ��ƻ��������ڣ�
			if(toDate != null && toDate != String.Empty)
			{
				sqlCondition += " and A.shiftday <= " + toDate;
			}
			

			string sql = String.Format(sqlMain,sqlCondition);
			#endregion

			object[] objs = DataProvider.CustomQuery(typeof(MTBFQueryObject)
				,new SQLCondition(sql));

			System.Collections.Hashtable htTotal = new System.Collections.Hashtable();
			double totalMTBF=0;
			if(objs != null && objs.Length > 0)
			{

				double ngQty = 0;
				double totalManuTime = 0;
				foreach(MTBFQueryObject obj in objs)
				{
					ngQty += 1;
					string objGroupType = String.Empty;

					switch(filterfield)
					{
						case "MODELCODE":
						{
							objGroupType = obj.ModelCode;
							break;
						}
						case "ITEMCODE":
						{
							objGroupType = obj.ItemCode;
							break;
						}
						case "SSCODE":
						{
							objGroupType = obj.SSCode;
							break;
						}
					}
					//����Total Object
					if(htTotal.ContainsKey(objGroupType))
					{
						#region �Ѿ�����Key=objGroupType��MTBFObject����
						MTBFObject mto = (MTBFObject)htTotal[objGroupType];
						
						mto.FieldCode = objGroupType;						
						mto.NGQty += 1;					//��������
						
						#region ����SubMTBF Details
						
						if(mto.DailyMTBFs != null)
						{
							string byObject = String.Empty;
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									byObject = obj.NGDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									byObject = obj.NGWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									byObject = obj.NGMonth;
									break;
								}
							}
						
							string htKey = byObject + objGroupType + obj.NGDate;
							
							if(mto.DailyMTBFs.ContainsKey(byObject + objGroupType) )
							{
								if(!mto.HTDayData.ContainsKey(htKey))
								{
									mto.ManufactureTime += obj.ManufactureTime;	//����ʱ��	
									totalManuTime += obj.ManufactureTime;

									SubMTBF ds = (SubMTBF)mto.DailyMTBFs[byObject + objGroupType];
									//Laws Lu,2006/04/26 ������ͬγ��ͳ��
									switch(statisticlatitude)
									{
										case DashboardFacade.ByDay:
										{
											ds.Date = obj.NGDate;
											break;
										}
										case DashboardFacade.ByWeek:
										{
											ds.Date = obj.NGWeek;
											break;
										}
										case DashboardFacade.ByMonth:
										{
											ds.Date = obj.NGMonth;
											break;
										}
									}
									ds.FieldCode = mto.FieldCode;
									ds.NGQty = ds.NGQty  + 1;
	
									ds.ManufactureTime += obj.ManufactureTime;
									//ds.MTBF;�ڶ������Զ�����
														
									//����MTBFDetails
									if(ds.MTBFDetails == null)
									{
										ds.MTBFDetails = new System.Collections.ArrayList();
										ds.HTDetailDayData = new System.Collections.Hashtable();
									}
						
									if(!ds.HTDetailDayData.Contains(htKey))
									{
										ds.MTBFDetails.Add(obj);
										ds.HTDetailDayData.Add(htKey,htKey);//��¼�Ѿ�ͳ�Ƶ���ϸ����
									}
						
									mto.DailyMTBFs[ds.Date + objGroupType] = ds;
									mto.HTDayData.Add(htKey,htKey); //��¼�Ѿ�ͳ�Ƶ���ϸ����
								}
								else
								{
									//��ϸ������ʱ���Ѿ�ͳ�ƹ��ˣ������Ӧ��MTBFDetails�м���.
									SubMTBF ds = (SubMTBF)mto.DailyMTBFs[byObject + objGroupType];
									ds.NGQty += 1;
									ds.MTBFDetails.Add(obj);
									mto.DailyMTBFs[ds.Date + objGroupType] = ds;
								}
							}
							else
							{
								SubMTBF ds = new SubMTBF();
								//Laws Lu,2006/04/26 ������ͬγ��ͳ��
								switch(statisticlatitude)
								{
									case DashboardFacade.ByDay:
									{
										ds.Date = obj.NGDate;
										break;
									}
									case DashboardFacade.ByWeek:
									{
										ds.Date = obj.NGWeek;
										break;
									}
									case DashboardFacade.ByMonth:
									{
										ds.Date = obj.NGMonth;
										break;
									}
								}
								ds.FieldCode = mto.FieldCode;
								ds.NGQty = 1;	
								ds.ManufactureTime += obj.ManufactureTime;
								//ds.MTBF;�ڶ������Զ�����
							
								//����MTBFDetails
								if(ds.MTBFDetails == null)
								{
									ds.MTBFDetails = new System.Collections.ArrayList();
									ds.HTDetailDayData = new System.Collections.Hashtable();
								}
							
								ds.MTBFDetails.Add(obj);
								ds.HTDetailDayData.Add(htKey,htKey);//��¼�Ѿ�ͳ�Ƶ���ϸ����

								mto.DailyMTBFs.Add(ds.Date + objGroupType,ds);
								mto.HTDayData.Add(htKey,htKey); //��¼�Ѿ�ͳ�Ƶ���ϸ����
							}
						}
						
						#endregion
						
						htTotal[objGroupType] = mto;
						#endregion
					}
					else
					{
						#region ������ʱ�Ĵ���
						MTBFObject mto = new MTBFObject();
					
						mto.FieldCode = objGroupType;						
						mto.NGQty = mto.NGQty + 1;					//��������
						mto.ManufactureTime += obj.ManufactureTime;	//����ʱ��
						totalManuTime += obj.ManufactureTime;
						
						#region ����SubMTBF Details

						if(mto.DailyMTBFs == null)
						{
							mto.DailyMTBFs = new System.Collections.SortedList();
							mto.HTDayData = new System.Collections.Hashtable();

							SubMTBF ds = new SubMTBF();
							//Laws Lu,2006/04/26 ������ͬγ��ͳ��
							switch(statisticlatitude)
							{
								case DashboardFacade.ByDay:
								{
									ds.Date = obj.NGDate;
									break;
								}
								case DashboardFacade.ByWeek:
								{
									ds.Date = obj.NGWeek;
									break;
								}
								case DashboardFacade.ByMonth:
								{
									ds.Date = obj.NGMonth;
									break;
								}
							}
							ds.FieldCode = mto.FieldCode;
							ds.NGQty = 1;	
							ds.ManufactureTime += obj.ManufactureTime;
							//ds.MTBF;�ڶ������Զ�����
							
							//����MTBFDetails
							if(ds.MTBFDetails == null)
							{
								ds.MTBFDetails = new System.Collections.ArrayList();
								ds.HTDetailDayData = new System.Collections.Hashtable();
							}
							string htKey = ds.Date + objGroupType + obj.NGDate;
							
							ds.MTBFDetails.Add(obj);
							ds.HTDetailDayData.Add(htKey,htKey);//��¼�Ѿ�ͳ�Ƶ���ϸ����

							mto.DailyMTBFs.Add(ds.Date + objGroupType,ds);
							mto.HTDayData.Add(htKey,htKey); //��¼�Ѿ�ͳ�Ƶ���ϸ����
						}

						#endregion


						htTotal.Add(objGroupType,mto);
						#endregion
					}
				}
				totalMTBF = System.Math.Round(totalManuTime/ngQty,2);
			}

			MTBFXmlBuilder xml = new MTBFXmlBuilder();
			xml.BeginBuildRoot();
			//д�����
			foreach(MTBFObject to in htTotal.Values)
			{
				xml.BeginBuildItem(to.FieldCode,to.MTBF.ToString(),totalMTBF.ToString(),to.NGQty.ToString());
				//д��������
				foreach(SubMTBF ds in to.DailyMTBFs.Values)
				{
					xml.BeginBuildDateMTBF(ds.Date,ds.FieldCode,ds.NGQty.ToString(),ds.MTBF.ToString());
					//д��������
					foreach(MTBFQueryObject obj in ds.MTBFDetails)
					{
						xml.BuildDateMTBFDetail(obj.ItemCode,obj.ModelCode,obj.SSCode,obj.SN,obj.NGDate,obj.begintime,obj.endtime,obj.ManufactureTime.ToString());
					}
					//Flex��bug�����������޷���ʾ
					if(ds.MTBFDetails.Count == 1)
					{
						xml.BuildDateMTBFDetail(String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
					}

					xml.EndBuildDateMTBF();
				}
				//Flex��bug�����������޷���ʾ
				if(to.DailyMTBFs.Values.Count == 1)
				{
					xml.BeginBuildDateMTBF(String.Empty,String.Empty,String.Empty,String.Empty);
					xml.EndBuildDateMTBF();
				}

				xml.EndBuildItem();
			}
			//Flex��bug�����������޷���ʾ
			if(htTotal.Values.Count == 1)
			{	
				xml.BeginBuildItem(String.Empty,String.Empty,String.Empty,String.Empty);
				xml.EndBuildItem();
			}

			xml.EndBuildRoot();

			strReturn = xml.XmlContent.ToString();

			return strReturn;
		}

		private string getManufacrureTime(string begintime,string endtime)
		{
			return string.Empty;
		}

		#endregion
	}

}
