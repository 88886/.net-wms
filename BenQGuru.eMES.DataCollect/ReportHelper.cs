using System;
using System.Data.SqlTypes;
using System.Collections;

using UserControl;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.DataCollect.Action;
using BenQGuru.eMES.Domain.Report;
using BenQGuru.eMES.Report;
using BenQGuru.eMES.Domain.TS;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.TS;
using BenQGuru.eMES.Rework;
using BenQGuru.eMES.MOModel;

namespace BenQGuru.eMES.DataCollect
{
	/// <summary>
	/// ReportHelper ��ժҪ˵����
	/// </summary>
	public class ReportHelper
	{
		private const string OutLineResource = "OUTLINERESOURCE";
		private IDomainDataProvider _domainDataProvider = null;

//		public ReportHelper()
//		{	
//		}

		public ReportHelper(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
		}

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

		public int  WeekOfYear(string Date)
		{
//			int year=0;
			int weekOfYear =0;
//			int dayOfWeek =0;
//			DateTime dt = new DateTime(System.Int32.Parse(Date.Substring(0,4)),System.Int32.Parse(Date.Substring(4,2)),System.Int32.Parse(Date.Substring(6,2)));
//
//			DecodeDateWeek(dt,out year,out weekOfYear,out dayOfWeek);

//			DateTime dtCurrentYear = new DateTime(int.Parse(Date.Substring(0,4)),1,1);
//			DateTime DtNow = new DateTime(int.Parse(Date.Substring(0,4)),int.Parse(Date.Substring(4,2)),int.Parse(Date.Substring(6,2)));
			String sqlWeek = "select TO_CHAR (TO_DATE ('" + Date + "', 'yyyyMMdd'), 'ww') as Week from dual";

			object[] objs = DataProvider.CustomQuery(typeof(WeekObject),new SQLCondition(sqlWeek));

			if(objs != null)
			{
				object obj = objs[0];
				weekOfYear = (obj as WeekObject).Week;

			}
			else
			{
				weekOfYear = 1;
			}

			return weekOfYear;
		}

		private static void DecodeDateWeek(DateTime dt,out int year,out int  weekOfYear,out int dayOfWeek)
		{
			int dayOfYear = dt.DayOfYear;
			int monthOfYear = dt.Month;
			year = dt.Year;
			dayOfWeek = (int)dt.DayOfWeek;
			DateTime startDate = new DateTime(dt.Year,1,1);
			System.DayOfWeek startDayOfWeek = startDate.DayOfWeek;
			System.DayOfWeek endDayOfWeek = dt.DayOfWeek;
			if((startDayOfWeek == DayOfWeek.Friday)||(startDayOfWeek==DayOfWeek.Saturday)||(startDayOfWeek==DayOfWeek.Sunday))
			{
				dayOfYear = dayOfYear - (7- (int)startDayOfWeek);
			}
			else
			{
				dayOfYear = dayOfYear + (int)startDayOfWeek;
			}
			//dayOfYear+startDayOfWeek <7 ��Ӧ��Ϊȥ�����
			if(dayOfYear <0)
			{
				DecodeDateWeek(startDate.AddDays(-1),out year,out weekOfYear,out dayOfWeek);
			}
			weekOfYear = dayOfYear/7;
			if( SqlInt32.Mod(dayOfYear,7) != 0)
			{
				weekOfYear++;
			}
			if(weekOfYear > 52)
			{
				endDayOfWeek = startDayOfWeek;                   
				if(DateTime.IsLeapYear(dt.Year))
				{
					if((endDayOfWeek == DayOfWeek.Sunday)||(endDayOfWeek == DayOfWeek.Monday))
					{
						endDayOfWeek ++;
					}
					if((endDayOfWeek==DayOfWeek.Monday)||(endDayOfWeek==DayOfWeek.Tuesday)||(endDayOfWeek==DayOfWeek.Wednesday))
					{
						year ++;
						weekOfYear = 1;
					}
				}
			}
		}

		private bool IsLastOP(string moCode,string routeCode,string opCode)
		{
			if (routeCode==string.Empty)
				return false;
			DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);

			return dataCollectFacade.OPIsMORouteLastOP(moCode,routeCode,opCode);
		}

		private bool IsMidOutputOP(string itemCode,string routeCode,string opCode,ProductInfo product)
		{
			if (routeCode.Trim()==string.Empty)
			{
				BaseModelFacade itemFacade = new BaseModelFacade(this.DataProvider);

				object obj = itemFacade.GetOperation(opCode);

				if(obj == null)
				{
					//Laws Lu,2006/12/28
					/*Burn In ©ɨʱ���������FT����ʾ���ò�Ʒ�Ѿ��깤���߱��滻�������ʾҪ�޸ģ��Ա���USER֪�����������깤�ˣ�����ʾҪ�ӵ�һվͶ�롣
					�������������ʾ��ʱ�򣬼����ָò�Ʒ�Ѿ��깤���Ȳ�Ҫֱ�ӱ��������ں����ټ�һ����飬������깤���򣬲����������Ͼ�����������Ǻܶ࣬�������ܷ���Ĺ��ǿ����ų�����
					 * */
					if(product.LastSimulation != null && product.LastSimulation.IsComplete == "1")
					{
						throw new Exception("$CS_PRODUCT_ALREADY_COMPLETE $CS_Param_OPCode =" + product.LastSimulation.OPCode);
					}
					else
					{
						throw new Exception("$CS_Route_Failed_GetNotNextOP");
					}
				}

				return FormatHelper.StringToBoolean( ((Operation)obj).OPControl,(int)OperationList.MidistOutput);	
			}
			else
			{
				ItemFacade itemFacade = new ItemFacade(this.DataProvider);
				object obj = null;
				if(product.CurrentItemRoute2OP != null)
				{
					obj  = product.CurrentItemRoute2OP;
				}
				else
				{
					obj =itemFacade.GetItemRoute2Operation(itemCode,routeCode,opCode);
				}
				return FormatHelper.StringToBoolean( ((ItemRoute2OP)obj).OPControl,(int)OperationList.MidistOutput);	
			}
		}

		//Laws Lu,2005/10/26,����	�ж��Ƿ����м�Ͷ�빤��
		private bool IsMidInputOP(string itemCode,string routeCode,string opCode,ProductInfo product)
		{
			//return false;
			if (routeCode.Trim()==string.Empty)
			{
				BaseModelFacade itemFacade = new BaseModelFacade(this.DataProvider);

				object obj = itemFacade.GetOperation(opCode);

				if(obj == null)
				{
					//Laws Lu,2006/12/28
					/*Burn In ©ɨʱ���������FT����ʾ���ò�Ʒ�Ѿ��깤���߱��滻�������ʾҪ�޸ģ��Ա���USER֪�����������깤�ˣ�����ʾҪ�ӵ�һվͶ�롣
					�������������ʾ��ʱ�򣬼����ָò�Ʒ�Ѿ��깤���Ȳ�Ҫֱ�ӱ��������ں����ټ�һ����飬������깤���򣬲����������Ͼ�����������Ǻܶ࣬�������ܷ���Ĺ��ǿ����ų�����
					 * */
					if(product.LastSimulation != null && product.LastSimulation.IsComplete == "1")
					{
						throw new Exception("$CS_PRODUCT_ALREADY_COMPLETE $CS_Param_OPCode =" + product.LastSimulation.OPCode);
					}
					else
					{
						throw new Exception("$CS_Route_Failed_GetNotNextOP");
					}
				}

				return FormatHelper.StringToBoolean( ((Operation)obj).OPControl,(int)OperationList.MidistInput);	
			}
			else
			{
				ItemFacade itemFacade = new ItemFacade(this.DataProvider);

				object obj = null;
				if(product.CurrentItemRoute2OP != null)
				{
					obj  = product.CurrentItemRoute2OP;
				}
				else
				{
					obj =itemFacade.GetItemRoute2Operation(itemCode,routeCode,opCode);
				}
				return FormatHelper.StringToBoolean( ((ItemRoute2OP)obj).OPControl,(int)OperationList.MidistInput);	
			}
		}

		private int IDIsInRejectCount(string runningCard,string line)
		{
			ReworkFacade reworkFacade = new ReworkFacade(this.DataProvider);
			return reworkFacade.GetRejectCount(runningCard,line);
		}

		private int IDIsInRejectCountByMo(string runningCard,string moCode)
		{
			ReworkFacade reworkFacade = new ReworkFacade(this.DataProvider);
			return reworkFacade.GetRejectCountByMO(runningCard,moCode);
		}

		/// <summary>
		/// By Line ͳ�Ʋ�����NG��ֱͨ��
		/// Laws Lu,2005/10/10,�޸�	������
		/// </summary>
		/// <param name="domainDataProvider"></param>
		/// <param name="actionType"></param>
		/// <param name="product"></param>
		/// <returns></returns>
		public Messages ReportLineQuanMaster(IDomainDataProvider domainDataProvider, string actionType,ProductInfo product)
		{
			return ReportLineQuanMaster(domainDataProvider, actionType, product, null);
		}
		// Added by Icyer 2005/10/29
		// ��չһ����ActionCheckStatus�����ķ���
		public Messages ReportLineQuanMaster(IDomainDataProvider domainDataProvider, string actionType,ProductInfo product, ActionCheckStatus actionCheckStatus)
		{
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug("ReportLineQuanMaster");
			dataCollectDebug.WhenFunctionIn(messages);
			ReportFacade reportFacade = new ReportFacade(domainDataProvider);
			ReportRealtimeLineQty reportRealtimeLineQty = new ReportRealtimeLineQty();
			TSFacade tSFacade = new TSFacade(domainDataProvider);
			MOFacade mOFacade = new MOFacade(domainDataProvider);
			//Laws Lu,2005/10/10,����
			int ngtimes = 0;
			int qty = 0;
			int allqty = 0;
			int inputQty = 0;
			int moInputQty = 0;
			int moAllGoodQty = 0;
			try
			{
				//Laws  Lu,2005/10/13,����	QtyFlag����Ϊ����
				string flag = String.Empty;
				bool midOP = false;
				bool midInputOP = false;

				if (actionCheckStatus == null || actionCheckStatus.IsMidOutputOP == string.Empty)
				{
					if(IsMidOutputOP(product.NowSimulationReport.ItemCode,product.NowSimulationReport.RouteCode,product.NowSimulationReport.OPCode,product))
					{
						midOP = true;
					}
					if (actionCheckStatus != null)
					{
						actionCheckStatus.IsMidOutputOP = midOP.ToString();
					}
				}
				else
				{
					midOP = Convert.ToBoolean(actionCheckStatus.IsMidOutputOP);
				}
				if (actionCheckStatus == null || actionCheckStatus.IsMidInputOP == string.Empty)
				{
					//Laws Lu,2005/10/26,����	�м�Ͷ��
					if(IsMidInputOP(product.NowSimulationReport.ItemCode,product.NowSimulationReport.RouteCode,product.NowSimulationReport.OPCode,product))
					{
						midInputOP = true;
					}
					if (actionCheckStatus != null)
					{
						actionCheckStatus.IsMidInputOP = midInputOP.ToString();
					}
				}
				else
				{
					midInputOP = Convert.ToBoolean(actionCheckStatus.IsMidInputOP);
				}

				//Laws Lu,2005/10/18,�޸�	�Ƿ�Ϊ�����
				if (actionCheckStatus == null || actionCheckStatus.IsLastOP == string.Empty)
				{
					if (IsLastOP(product.NowSimulationReport.MOCode,product.NowSimulationReport.RouteCode,product.NowSimulationReport.OPCode))//�Ƿ�Ϊ�����
						flag = "Y";
					else
						flag = "N";
					if (actionCheckStatus != null)
					{
						actionCheckStatus.IsLastOP = flag;
					}
				}
				else
				{
					flag = actionCheckStatus.IsLastOP;
				}

				int iNGStart = 0;

				int iMoNGStart = 0;
				

				#region ��������
				//Laws Lu,2005/11/09,Eric ���м�Ͷ�빤��Ͷ��Ĳ�������Ʒ���ǲ���Ʒ,����ΪͶ�����ļ���.�����ڼ���.
				if(midInputOP)
				{
					inputQty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
				}

				if (product.NowSimulation.ProductStatus == "GOOD" && product.NowSimulation.LastAction != ActionType.DataCollectAction_GoMO)
				{
					//Laws Lu,2005/11/07,����	Ͷ�����ļ���
					//TS�Ĵ���Ϊ0������Ͷ����
					//TS�Ĵ�������0������ϴ��ڴ�OP�ϵ�Action������NG�������ڴ˴�Action��״̬������ô����Ͷ����
					//					if((midInputOP && iNGStart == 0) 
					//						|| (midInputOP && iNGStart > 0 && product.NowSimulation.ProductStatus != product.LastSimulation.ProductStatus
					//						&& product.NowSimulation.OPCode == product.LastSimulation.OPCode))
					//						
					//					{

					if (flag == "Y" || midOP)//�Ƿ�Ϊ�������м���㹤��
					{
						//reportRealtimeLineQty.OuputQty += 1 * product.NowSimulation.IDMergeRule;
						//TS�Ĵ���Ϊ0�����Ӳ�����
						//TS�Ĵ�������0������ϴ��ڴ�OP�ϵ�Action������NG�������ڴ˴�Action��״̬������ô���Ӳ�����
						//						if((iNGStart == 0) 
						//							|| (iNGStart > 0 && product.NowSimulation.ProductStatus != product.LastSimulation.ProductStatus
						//							&& product.LastSimulation.OPCode == product.NowSimulation.OPCode)
						//							|| product.NowSimulation.LastAction == ActionType.DataCollectAction_OQCPass)
						//						{
						if(product.NowSimulation != null)
						{
							iNGStart = tSFacade.QueryTSCountByLine(
								product.NowSimulationReport.RunningCard
								,product.NowSimulationReport.StepSequenceCode
								,Convert.ToInt32(product.NowSimulationReport.RunningCardSequence));

							iMoNGStart = tSFacade.QueryTSCountByMo(
								product.NowSimulation.RunningCard
								,product.NowSimulation.MOCode);
						}

						qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
						//						}
						//Laws Lu,2005/11/05,�޸�	ֱͨ������׼
						if (/*(tSFacade.QueryTSCountByLine(product.NowSimulationReport.RunningCard,product.NowSimulationReport.StepSequenceCode)==0)*/
							iNGStart == 0 && IDIsInRejectCount(product.NowSimulationReport.RunningCard,product.NowSimulationReport.StepSequenceCode)==0) 
						{
							//reportRealtimeLineQty.AllGoodQty += 1 * product.NowSimulation.IDMergeRule;
							allqty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
						}

						//����ֱ̨ͨ��
						#region Eric 2005-11-10  �йز�����Ͷ�����������ʡ�ֱͨ�ʵĲ���˵����
						/*3. ����ֱͨ�ʣ�

						��(���������������򷵹���������Ͷ�뵽�����Ĺ�����δ�����ֹ������Ĳ�Ʒ̨��)/�ù��������������򷵹�����������ɵĲ�Ʒ��Ʒ̨��

 
						�Բ����ж�ֱͨ��Ʒ����ʱ��һ��Ҫ�ֹ������������������A������ֻ���жϸò�Ʒ���к���A������ע���ǹ����������ǲ��ߣ������� �����м���ߣ����ܸò�Ʒ���к�������δ���ֲ���������ǰ��Ĳ���������ֲ��������Ǹ��м���ߵķ�ֱͨƷ���������������Ƿ�����˲��������в�����Ϊ��ֱͨƷ����ʹ�ò�Ʒ���к������������������ֹ����������Ըôε�������Ӱ�쵽ֱ̨ͨ���ļ��㡣

						ֱͨ�ʲ����ڿ�ʱ�ε�����.ֻҪ�������ڹ����������ֹ��������򲻻��¼ΪֱͨƷ��û�в����ͼ�¼ΪֱͨƷ��Ȼ�������ʱ�Ρ�

						�ù��������Ĳ�Ʒһ������ֱͨƷ

						FQC���Ϊ���������˵Ĳ�Ʒһ������ֱͨƷ
						*/
						#endregion
						
						if (flag == "Y" && iMoNGStart == 0 
							&& IDIsInRejectCountByMo(product.NowSimulationReport.RunningCard,product.NowSimulationReport.MOCode)==0
							&& product.NowSimulationReport.NGTimes == 0) 
						{
							moAllGoodQty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
						}
						
					}
				}
				else if (product.NowSimulation.ProductStatus == "NG")
				{
					//reportRealtimeLineQty.NGTimes += 1;
					//Laws Lu,2005/11/07,����	Ͷ�����ļ���
					//�������TS�����Ҳ�Ʒǰһ��״̬�����ڵ�NG״̬����
					//����ǰһ��Action�����ڵ�Action����ͬһ������
					//�ɼ�NG��Ͷ����Ӧ�ü���
					//����ֱ�Ӳɼ�NG����Ͷ������bug
					//Laws Lu,2005/11/09,Eric ���м�Ͷ�빤��Ͷ��Ĳ�������Ʒ���ǲ���Ʒ,����ΪͶ�����ļ���.�����ڼ���.
//					if((midInputOP && iNGStart > 0 && product.NowSimulation.ProductStatus != product.LastSimulation.ProductStatus )
//						&& product.NowSimulation.LastAction != ActionType.DataCollectAction_GoMO
//						&& product.LastSimulation.OPCode == product.NowSimulation.OPCode)

					ngtimes = 1;
					if (product.NowSimulation.OPCode == product.LastSimulation.OPCode
						&& (product.LastSimulation.LastAction == ActionType.DataCollectAction_GOOD 
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_SMTGOOD
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_OutLineGood
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_OQCPass) )
					{
						if (flag == "Y" || midOP)//�Ƿ�Ϊ�������м���㹤��

						{
							//Laws Lu,2005/11/09,�޸�	��ǰվ�й�����,����ǰһ�β���״̬��GOOD�Ż������
							//							if( product.NowSimulation.OPCode == product.LastSimulation.OPCode)
							//							{
							//								if(product.NowSimulation.ProductStatus != product.LastSimulation.ProductStatus)
							//								{
							qty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule);
							//								}
							//							}
							//						
							if(product.NowSimulation != null)
							{
								iNGStart = tSFacade.QueryTSCountByLine(
									product.NowSimulationReport.RunningCard
									,product.NowSimulationReport.StepSequenceCode
									,Convert.ToInt32(product.NowSimulationReport.RunningCardSequence));

								iMoNGStart = tSFacade.QueryTSCountByMo(
									product.NowSimulation.RunningCard
									,product.NowSimulation.MOCode);
							}

							//Laws Lu,2005/11/05,�޸�	ֱͨ������׼
							if (/*(tSFacade.QueryTSCountByLine(product.NowSimulationReport.RunningCard,product.NowSimulationReport.StepSequenceCode)==1)*/
								(IDIsInRejectCount(product.NowSimulationReport.RunningCard,product.NowSimulationReport.StepSequenceCode)==0)
								&& iNGStart == 0)//rejectlist
							{
								//reportRealtimeLineQty.AllGoodQty -= 1 * product.NowSimulation.IDMergeRule;
								allqty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule);
							}

							//����ֱ̨ͨ��
							#region Eric 2005-11-10  �йز�����Ͷ�����������ʡ�ֱͨ�ʵĲ���˵����
/*3. ����ֱͨ�ʣ�

��(���������������򷵹���������Ͷ�뵽�����Ĺ�����δ�����ֹ������Ĳ�Ʒ̨��)/�ù��������������򷵹�����������ɵĲ�Ʒ��Ʒ̨��

 
�Բ����ж�ֱͨ��Ʒ����ʱ��һ��Ҫ�ֹ������������������A������ֻ���жϸò�Ʒ���к���A������ע���ǹ����������ǲ��ߣ������� �����м���ߣ����ܸò�Ʒ���к�������δ���ֲ���������ǰ��Ĳ���������ֲ��������Ǹ��м���ߵķ�ֱͨƷ���������������Ƿ�����˲��������в�����Ϊ��ֱͨƷ����ʹ�ò�Ʒ���к������������������ֹ����������Ըôε�������Ӱ�쵽ֱ̨ͨ���ļ��㡣

ֱͨ�ʲ����ڿ�ʱ�ε�����.ֻҪ�������ڹ����������ֹ��������򲻻��¼ΪֱͨƷ��û�в����ͼ�¼ΪֱͨƷ��Ȼ�������ʱ�Ρ�

�ù��������Ĳ�Ʒһ������ֱͨƷ

FQC���Ϊ���������˵Ĳ�Ʒһ������ֱͨƷ
*/
							#endregion
							
							if (flag == "Y" && iMoNGStart == 0 
								&& IDIsInRejectCountByMo(product.NowSimulationReport.RunningCard,product.NowSimulationReport.MOCode)==0
								&& product.NowSimulationReport.NGTimes == 1) 
							{
								moAllGoodQty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule);
							}
							
							
						}

					}
				}
				else if (product.NowSimulation.ProductStatus == "REJECT")
				{
					if (product.NowSimulation.ResourceCode==product.LastSimulation.ResourceCode
						&& (product.LastSimulation.LastAction == ActionType.DataCollectAction_GOOD 
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_SMTGOOD
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_OutLineGood
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_OQCPass) )
					{
						if (flag == "Y" || midOP)//�Ƿ�Ϊ�������м���㹤��

						{
							//reportRealtimeLineQty.OuputQty -= 1 * product.NowSimulation.IDMergeRule;
							if(product.NowSimulationReport != null)
							{
								iNGStart = tSFacade.QueryTSCountByLine(
									product.NowSimulationReport.RunningCard
									,product.NowSimulationReport.StepSequenceCode
									,Convert.ToInt32(product.NowSimulationReport.RunningCardSequence));

								iMoNGStart = tSFacade.QueryTSCountByMo(
									product.NowSimulation.RunningCard
									,product.NowSimulation.MOCode);
							}

							//Laws Lu,2005/11/05,�޸�	ֱͨ������׼
							qty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule);
							if (/*(tSFacade.QueryTSCountByLine(product.NowSimulationReport.RunningCard,product.NowSimulationReport.StepSequenceCode)==0)*/
								(IDIsInRejectCount(product.NowSimulationReport.RunningCard,product.NowSimulationReport.StepSequenceCode)==1)
								&& iNGStart == 0)//rejectlist
							{
								//reportRealtimeLineQty.AllGoodQty -= 1 * product.NowSimulation.IDMergeRule;
								allqty = Convert.ToInt32( -1 * product.NowSimulation.IDMergeRule);
							}

							//����ֱ̨ͨ��

							#region Eric 2005-11-10  �йز�����Ͷ�����������ʡ�ֱͨ�ʵĲ���˵����
							///<Comment>3. ����ֱͨ�ʣ�
							///��(���������������򷵹���������Ͷ�뵽�����Ĺ�����δ�����ֹ������Ĳ�Ʒ̨��)/�ù��������������򷵹�����������ɵĲ�Ʒ��Ʒ̨��
							///�Բ����ж�ֱͨ��Ʒ����ʱ��һ��Ҫ�ֹ������������������A������ֻ���жϸò�Ʒ���к���A������ע���ǹ����������ǲ��ߣ������� �����м���ߣ����ܸò�Ʒ���к�������δ���ֲ���������ǰ��Ĳ���������ֲ��������Ǹ��м���ߵķ�ֱͨƷ���������������Ƿ�����˲��������в�����Ϊ��ֱͨƷ����ʹ�ò�Ʒ���к������������������ֹ����������Ըôε�������Ӱ�쵽ֱ̨ͨ���ļ��㡣
							///ֱͨ�ʲ����ڿ�ʱ�ε�����.ֻҪ�������ڹ����������ֹ��������򲻻��¼ΪֱͨƷ��û�в����ͼ�¼ΪֱͨƷ��Ȼ�������ʱ�Ρ�
							///�ù��������Ĳ�Ʒһ������ֱͨƷ
							///FQC���Ϊ���������˵Ĳ�Ʒһ������ֱͨƷ
							///</Comment>
							#endregion

							if (flag == "Y" && iMoNGStart == 0 
								&& IDIsInRejectCountByMo(product.NowSimulationReport.RunningCard,product.NowSimulationReport.MOCode)==0
								&& product.NowSimulationReport.NGTimes == 1) 
							{
								moAllGoodQty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule);
							}
						}

					}
				}
				#endregion

				//Laws Lu,2005/10/26,ע��
				//Laws Lu,2005/10/31,����	Ͷ�����ļ���
//				if(midInputOP || product.NowSimulationReport.LastAction == ActionType.DataCollectAction_GoMO)
//				{
//					inputQty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
//				}

				if(product.NowSimulationReport.LastAction == ActionType.DataCollectAction_GoMO)
				{
					moInputQty =  Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
				}

				//Laws Lu,2005/10/10,�޸�	����������
				//Laws Lu,2005/10/28,�޸�	û�����֡��������빤��ʲô����������ʡϵͳ��Դ
				if(ngtimes != 0 || qty != 0 || allqty != 0 || inputQty != 0 || moInputQty != 0 || moAllGoodQty != 0 
					|| actionType == ActionType.DataCollectAction_OffMo/*���빤������*/)
				{
					//Laws Lu,2005/12/26,����	 û������ʲô����������ʡϵͳ��Դ
					if(ngtimes != 0 || qty != 0 || allqty != 0 || inputQty != 0 || moInputQty != 0 || moAllGoodQty != 0)
					{
						#region ��ȡ����ʵ��
						object obj = reportFacade.GetReportRealtimeLineQty(product.NowSimulationReport.MOCode,product.NowSimulationReport.TimePeriodCode,
							product.NowSimulationReport.StepSequenceCode,product.NowSimulationReport.SegmentCode,product.NowSimulationReport.ItemCode,
							product.NowSimulationReport.ShiftCode,product.NowSimulationReport.ModelCode,product.NowSimulationReport.ShiftDay,flag);

						if (obj == null)
						{
							ShiftModelFacade shiftModelFacade = new ShiftModelFacade(domainDataProvider);
							reportRealtimeLineQty.TimePeriodCode = product.NowSimulationReport.TimePeriodCode;
							reportRealtimeLineQty.TimePeriodBeginTime = shiftModelFacade.GetTimePeriodBeginTime(reportRealtimeLineQty.TimePeriodCode);
							reportRealtimeLineQty.TimePeriodEndTime= shiftModelFacade.GetTimePeriodEndTime(reportRealtimeLineQty.TimePeriodCode);

							reportRealtimeLineQty.Day=product.NowSimulationReport.MaintainDate;
				
							reportRealtimeLineQty.ItemCode = product.NowSimulationReport.ItemCode;
							reportRealtimeLineQty.MOCode = product.NowSimulationReport.MOCode;
							reportRealtimeLineQty.ModelCode = product.NowSimulationReport.ModelCode;
					
							reportRealtimeLineQty.ScrapQty =0;
							reportRealtimeLineQty.EAttribute1 = 0;
							reportRealtimeLineQty.SegmentCode = product.NowSimulationReport.SegmentCode;
							reportRealtimeLineQty.ShiftCode = product.NowSimulationReport.ShiftCode;
							reportRealtimeLineQty.ShiftDay =product.NowSimulationReport.ShiftDay;
							reportRealtimeLineQty.StepSequenceCode = product.NowSimulationReport.StepSequenceCode;
						
					
							reportRealtimeLineQty.Week = WeekOfYear(reportRealtimeLineQty.ShiftDay.ToString());
							reportRealtimeLineQty.Month = int.Parse(reportRealtimeLineQty.ShiftDay.ToString().Substring(4,2));
								//int.Parse(reportRealtimeLineQty.ShiftDay.ToString().Substring(4,2));
						}
						else
						{
							reportRealtimeLineQty = (ReportRealtimeLineQty)obj;
						}
						#endregion

						//reportRealtimeLineQty.OuputQty = 0;
						reportRealtimeLineQty.NGTimes =0;
						reportRealtimeLineQty.InputQty = 0;
						reportRealtimeLineQty.AllGoodQty=0;
						reportRealtimeLineQty.EAttribute1 = 0;
						reportRealtimeLineQty.OuputQty = 0;
						reportRealtimeLineQty.MoAllGoodQty = 0;

						//					if(product.NowSimulationReport.LastAction == ActionType.DataCollectAction_GoMO)
						//					{
						//Laws Lu,2005/10/31,����	��ʾΪGOMO��������Ͷ����
						reportRealtimeLineQty.EAttribute1 = moInputQty;
						//					}

						reportRealtimeLineQty.QtyFlag = flag;
						reportRealtimeLineQty.NGTimes = ngtimes;
						reportRealtimeLineQty.InputQty = inputQty;
						reportRealtimeLineQty.AllGoodQty = allqty;
						reportRealtimeLineQty.OuputQty = qty;
						reportRealtimeLineQty.MoAllGoodQty = moAllGoodQty;
						//Laws Lu,2005/11/01,�޸�	ӦDavid��Ҫ�󣬱��������Ͷ��������˳��
						//					if(moInputQty != 0)
						//					{
						//						if(actionCheckStatus != null && actionCheckStatus.MO != null)
						//						{
						//							mOFacade.UpdateMOInPutQty(product.NowSimulation.MOCode,actionCheckStatus.MO,moInputQty);//,reportFacade.GetMOOutPutQty(product.NowSimulationReport.MOCode));
						//						}
						//						else
						//						{
						//							mOFacade.UpdateMOInPutQty(product.NowSimulationReport.MOCode,null,moInputQty);
						//						}
						//					}

#if DEBUG
						BenQGuru.eMES.Common.Log.Info("***************" + "\t RunningCard \t" + product.NowSimulationReport.RunningCard + "\t" +  DateTime.Now.ToString()) ;
#endif

						//					if (flag == "Y" && qty != 0)
						//					{
						//						mOFacade.UpdateMOOutPutQty(product.NowSimulationReport.MOCode,qty);
						//					}

						if (obj == null  )
						{
						
							reportFacade.AddReportRealtimeLineQty(reportRealtimeLineQty);
						}
						else
						{
							reportFacade.ModifyReportRealtimeLineQty(reportRealtimeLineQty,qty,ngtimes,allqty, product.NowSimulationReport.LastAction);
						}

					}

					if(product.NowSimulationReport.LastAction == ActionType.DataCollectAction_OffMo
                        || product.NowSimulationReport.LastAction == ActionType.DataCollectAction_GoMO 
						|| flag == "Y")
					{
						//Laws Lu,2005/12/15,�޸�	������¹�������������
						mOFacade.UpdateMOQty(product.NowSimulationReport.MOCode
							,product.NowSimulationReport.LastAction
							,Convert.ToInt32(1 * product.NowSimulation.IDMergeRule));
					}
				}
				
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}
		// Added end

		/// <summary>
		/// by op ͳ�Ʋ�����NG����
		/// /// Laws Lu,2005/10/10,�޸�	������
		/// </summary>
		/// <param name="domainDataProvider"></param>
		/// <param name="actionType"></param>
		/// <param name="product"></param>
		/// <returns></returns>
		public Messages ReportResQuanMaster(IDomainDataProvider domainDataProvider, string actionType,ProductInfo product)
		{
			return ReportResQuanMaster(domainDataProvider, actionType, product, null);
		}

		
		public void FqcPassQty(Hashtable htProducts,object[] Simulations,OQCPASSEventArgs actionEventArgs)
		{
			ReportFacade reportFacade = new ReportFacade(this.DataProvider);
			TSFacade tSFacade = new TSFacade(this.DataProvider);

			int i = 0;	
			ArrayList rptLineList = new ArrayList();
			//ArrayList rptLineList = new ArrayList();

			foreach (ProductInfo productionInf  in htProducts.Values)
			{
				//ProductInfo productionInf = (ProductInfo)htProducts[(Simulation)htProducts.Keys];
				#region ��ȡ����ʵ��
				string flag = String.Empty;
				if (IsLastOP(productionInf.NowSimulationReport.MOCode,productionInf.NowSimulationReport.RouteCode,productionInf.NowSimulationReport.OPCode))//�Ƿ�Ϊ�����
					flag = "Y";
				else
					flag = "N";

				#region ���߱���
				ReportRealtimeLineQty reportRealtimeLineQty = new ReportRealtimeLineQty();

				object obj = reportFacade.GetReportRealtimeLineQty(productionInf.NowSimulationReport.MOCode,
					productionInf.NowSimulationReport.TimePeriodCode,
					productionInf.NowSimulationReport.StepSequenceCode,
					productionInf.NowSimulationReport.SegmentCode,
					productionInf.NowSimulationReport.ItemCode,
					productionInf.NowSimulationReport.ShiftCode,
					productionInf.NowSimulationReport.ModelCode,
					productionInf.NowSimulationReport.ShiftDay,
					flag);
					
				if(obj == null)
				{
					ShiftModelFacade shiftModelFacade = new ShiftModelFacade(this.DataProvider);
					reportRealtimeLineQty.TimePeriodCode = productionInf.NowSimulationReport.TimePeriodCode;
					reportRealtimeLineQty.TimePeriodBeginTime = shiftModelFacade.GetTimePeriodBeginTime(reportRealtimeLineQty.TimePeriodCode);
					reportRealtimeLineQty.TimePeriodEndTime = shiftModelFacade.GetTimePeriodEndTime(reportRealtimeLineQty.TimePeriodCode);

					reportRealtimeLineQty.Day = productionInf.NowSimulationReport.MaintainDate;
				
					reportRealtimeLineQty.ItemCode = productionInf.NowSimulationReport.ItemCode;
					reportRealtimeLineQty.MOCode = productionInf.NowSimulationReport.MOCode;
					reportRealtimeLineQty.ModelCode = productionInf.NowSimulationReport.ModelCode;
					
					//						reportRealtimeLineQty.ScrapQty = 0;
					//						reportRealtimeLineQty.EAttribute1 = 0;
					reportRealtimeLineQty.SegmentCode = productionInf.NowSimulationReport.SegmentCode;
					reportRealtimeLineQty.ShiftCode = productionInf.NowSimulationReport.ShiftCode;
					reportRealtimeLineQty.ShiftDay = productionInf.NowSimulationReport.ShiftDay;
					reportRealtimeLineQty.StepSequenceCode = productionInf.NowSimulationReport.StepSequenceCode;
											
					reportRealtimeLineQty.Week = WeekOfYear(reportRealtimeLineQty.ShiftDay.ToString());
					reportRealtimeLineQty.Month = int.Parse(reportRealtimeLineQty.ShiftDay.ToString().Substring(4,2));;
					reportRealtimeLineQty.QtyFlag = flag;
				}
				else
				{
					reportRealtimeLineQty = (ReportRealtimeLineQty)obj;
				}


				#region ��������
				
				reportRealtimeLineQty.MoAllGoodQty =Decimal.Zero;
				reportRealtimeLineQty.OuputQty = Decimal.Zero;
				reportRealtimeLineQty.AllGoodQty = Decimal.Zero;
				reportRealtimeLineQty.NGTimes = Decimal.Zero;

				if (productionInf.NowSimulation.ProductStatus == "GOOD")
				{
					
					if (flag == "Y" )
					{
						int iNGStart = 0;

						int iMoNGStart = 0;

						if(productionInf.NowSimulation != null)
						{
							iNGStart = tSFacade.QueryTSCountByLine(
								productionInf.NowSimulationReport.RunningCard
								,productionInf.NowSimulationReport.StepSequenceCode
								,Convert.ToInt32(productionInf.NowSimulationReport.RunningCardSequence));

							iMoNGStart = tSFacade.QueryTSCountByMo(
								productionInf.NowSimulation.RunningCard
								,productionInf.NowSimulation.MOCode);
						}

						reportRealtimeLineQty.OuputQty  = Convert.ToInt32(1 * productionInf.NowSimulation.IDMergeRule);
							
						if (iNGStart == 0 && IDIsInRejectCount(productionInf.NowSimulationReport.RunningCard,productionInf.NowSimulationReport.StepSequenceCode)==0) 
						{
							reportRealtimeLineQty.AllGoodQty  = Convert.ToInt32(1 * productionInf.NowSimulation.IDMergeRule);
						}

						//����ֱ̨ͨ��
						#region Eric 2005-11-10  �йز�����Ͷ�����������ʡ�ֱͨ�ʵĲ���˵����
						/*3. ����ֱͨ�ʣ�

									��(���������������򷵹���������Ͷ�뵽�����Ĺ�����δ�����ֹ������Ĳ�Ʒ̨��)/�ù��������������򷵹�����������ɵĲ�Ʒ��Ʒ̨��

 
									�Բ����ж�ֱͨ��Ʒ����ʱ��һ��Ҫ�ֹ������������������A������ֻ���жϸò�Ʒ���к���A������ע���ǹ����������ǲ��ߣ������� �����м���ߣ����ܸò�Ʒ���к�������δ���ֲ���������ǰ��Ĳ���������ֲ��������Ǹ��м���ߵķ�ֱͨƷ���������������Ƿ�����˲��������в�����Ϊ��ֱͨƷ����ʹ�ò�Ʒ���к������������������ֹ����������Ըôε�������Ӱ�쵽ֱ̨ͨ���ļ��㡣

									ֱͨ�ʲ����ڿ�ʱ�ε�����.ֻҪ�������ڹ����������ֹ��������򲻻��¼ΪֱͨƷ��û�в����ͼ�¼ΪֱͨƷ��Ȼ�������ʱ�Ρ�

									�ù��������Ĳ�Ʒһ������ֱͨƷ

									FQC���Ϊ���������˵Ĳ�Ʒһ������ֱͨƷ
									*/
						#endregion
						
						if (flag == "Y" && iMoNGStart == 0 
							&& IDIsInRejectCountByMo(productionInf.NowSimulationReport.RunningCard,productionInf.NowSimulationReport.MOCode)==0
							&& productionInf.NowSimulationReport.NGTimes == 0) 
						{
							reportRealtimeLineQty.MoAllGoodQty = Convert.ToInt32(1 * productionInf.NowSimulation.IDMergeRule);
						}
						
					}
				}
				#endregion

				rptLineList.Add(reportRealtimeLineQty);

				#endregion

				i ++;							
				#endregion
			}

			//Laws Lu,2005/11/30,����	FQC��PASS�߼��޸�	ӦDavidҪ�󣬽����±���͸��¹������ڱ���ͳ�ƺ����ύ�����ݿ�
			
			Hashtable moQty = new Hashtable();
			string key = String.Empty;

			foreach(ReportRealtimeLineQty rpt in rptLineList)
			{
				key = String.Join(",",new string[]{rpt.MOCode, rpt.TimePeriodCode, rpt.StepSequenceCode, rpt.SegmentCode
													  , rpt.ItemCode, rpt.ShiftCode, rpt.ModelCode, rpt.ShiftDay.ToString(), rpt.QtyFlag});
				
				if(moQty.ContainsKey(key))
				{
					ReportQtyHelper rptQtyHelper2 = (ReportQtyHelper)moQty[key] ;
					
					rptQtyHelper2.AllGoodQty += rpt.AllGoodQty ;
					//rptQtyHelper2.EAttribute1 += rpt.EAttribute1 ;
					//rptQtyHelper2.EAttribute2 += rpt.EAttribute2 ;
					//rptQtyHelper2.EAttribute3 += rpt.EAttribute3 ;
					//rptQtyHelper2.InputQty += rpt.InputQty ;
					rptQtyHelper2.MoAllGoodQty += rpt.MoAllGoodQty ;
					rptQtyHelper2.OuputQty += rpt.OuputQty ;
					//rptQtyHelper2.ScrapQty += rpt.ScrapQty ;

					moQty[key] = rptQtyHelper2;
				}
				else
				{
					ReportQtyHelper rptQtyHelper;

					rptQtyHelper.AllGoodQty = rpt.AllGoodQty ;
					//rptQtyHelper.EAttribute1 = rpt.EAttribute1 ;
					//rptQtyHelper.EAttribute2 = rpt.EAttribute2 ;
					//rptQtyHelper.EAttribute3 = rpt.EAttribute3 ;
					//rptQtyHelper.InputQty = rpt.InputQty ;
					rptQtyHelper.MoAllGoodQty = rpt.MoAllGoodQty ;
					rptQtyHelper.OuputQty = rpt.OuputQty ;
					//rptQtyHelper.ScrapQty = rpt.ScrapQty ;

					moQty.Add(key,rptQtyHelper);
				}
			}

			foreach(string rptKey in moQty.Keys)
			{
				string[] keys = rptKey.Split(new char[]{','});

				if(keys.Length == 9)
				{
					#region ���±������¹���
					ReportRealtimeLineQty reportRealtimeLineQty = new ReportRealtimeLineQty();

					object obj = reportFacade.GetReportRealtimeLineQty(
						keys[0],//MOCode
						keys[1],//TimePeriodCode
						keys[2],//StepSequenceCode
						keys[3],//SegmentCode
						keys[4],//ItemCode
						keys[5],//ShiftCode
						keys[6],//ModelCode
						int.Parse(keys[7]),//ShiftDay
						keys[8]);//QtyFlag

					if (obj == null  )
					{
						reportRealtimeLineQty.TimePeriodCode = keys[1];

						ShiftModelFacade shiftModelFacade = new ShiftModelFacade(this.DataProvider);

						reportRealtimeLineQty.TimePeriodBeginTime = shiftModelFacade.GetTimePeriodBeginTime(reportRealtimeLineQty.TimePeriodCode);
						reportRealtimeLineQty.TimePeriodEndTime=shiftModelFacade.GetTimePeriodEndTime(reportRealtimeLineQty.TimePeriodCode);

						//2006/11/17,Laws Lu add get DateTime from db Server
						DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

						DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);

						reportRealtimeLineQty.Day = FormatHelper.TODateInt(dtNow);
				
						reportRealtimeLineQty.ItemCode =  keys[4];
						reportRealtimeLineQty.MOCode =  keys[0];
						reportRealtimeLineQty.ModelCode =  keys[6];
					
						//reportRealtimeLineQty.ScrapQty = 0;
						//reportRealtimeLineQty.EAttribute1 = 0;
						reportRealtimeLineQty.SegmentCode =  keys[3];
						reportRealtimeLineQty.ShiftCode =  keys[5];
						reportRealtimeLineQty.ShiftDay =  Convert.ToInt32(keys[7]);
						reportRealtimeLineQty.StepSequenceCode =  keys[2];
						
					
						reportRealtimeLineQty.Week = WeekOfYear(reportRealtimeLineQty.ShiftDay.ToString());
						reportRealtimeLineQty.Month = int.Parse(reportRealtimeLineQty.ShiftDay.ToString().Substring(4,2));
					}
					else
					{
						reportRealtimeLineQty = (ReportRealtimeLineQty)obj;
					}

					reportRealtimeLineQty.InputQty = 0;
					reportRealtimeLineQty.EAttribute1 = 0;
					reportRealtimeLineQty.NGTimes = 0;
					//modified by jessie lee, 2006/1/10
					ReportQtyHelper rptQtyHelper3 = (ReportQtyHelper)moQty[rptKey] ;
					//reportRealtimeLineQty.OuputQty = Convert.ToDecimal(moQty[rptKey]);
					reportRealtimeLineQty.OuputQty = rptQtyHelper3.OuputQty ;
					reportRealtimeLineQty.AllGoodQty = rptQtyHelper3.AllGoodQty ;
					reportRealtimeLineQty.MoAllGoodQty = rptQtyHelper3.MoAllGoodQty ;
					reportRealtimeLineQty.QtyFlag = keys[8];

					if(obj ==  null)
					{
						reportFacade.AddReportRealtimeLineQty(reportRealtimeLineQty);
					}
					else
					{
						reportFacade.ModifyReportRealtimeLineQty(reportRealtimeLineQty
							,Convert.ToInt32(reportRealtimeLineQty.OuputQty)
							,Convert.ToInt32(reportRealtimeLineQty.NGTimes)
							,Convert.ToInt32(reportRealtimeLineQty.AllGoodQty)
							,actionEventArgs.ActionType);
					}

					new MOFacade(this.DataProvider).UpdateMOQty(keys[0],actionEventArgs.ActionType,0);
						
					#endregion	
				}
			}
		}
		//Laws Lu,2006/03/20	FQCʱ��¼��Դ�����������
		public Messages UpdateReportResQuanMaster(
			IDomainDataProvider domainDataProvider
			,string flag
			,int qty
			,int ngtimes
			,int iPassQty
			,ReportHistoryOPQty reportHistoryOPQty
			,ProductInfo product)
		{
			DataCollectDebug dataCollectDebug =new DataCollectDebug("ReportResQuanMaster");
			ReportFacade reportFacade = new ReportFacade(domainDataProvider);
			Messages messages = new Messages();

			try
			{
				//AMOI  MARK  END
				//Laws Lu,2005/10/28,�޸�	û������ʲô����������ʡϵͳ��Դ
				if(qty != 0 || ngtimes != 0 || iPassQty != 0 || product.ECG2ErrCodes != String.Empty)
				{
					#region ��ȡ����ʵ��
					object obj = reportFacade.GetReportHistoryOPQty(product.NowSimulationReport.ModelCode,product.NowSimulationReport.ShiftDay,
						product.NowSimulationReport.MOCode,product.NowSimulationReport.TimePeriodCode,product.NowSimulationReport.StepSequenceCode,product.NowSimulationReport.SegmentCode,
						product.NowSimulationReport.ItemCode,product.NowSimulationReport.ShiftCode,product.NowSimulationReport.OPCode,product.NowSimulationReport.ResourceCode,flag);
					if (obj == null)
					{
						reportHistoryOPQty.Day = product.NowSimulationReport.MaintainDate;
						reportHistoryOPQty.ItemCode = product.NowSimulationReport.ItemCode;
						reportHistoryOPQty.MOCode = product.NowSimulationReport.MOCode;
						reportHistoryOPQty.ModelCode = product.NowSimulationReport.ModelCode;
					
						reportHistoryOPQty.QtyFlag = "N";
						reportHistoryOPQty.ShiftCode = product.NowSimulationReport.ShiftCode;
						reportHistoryOPQty.ShiftDay = product.NowSimulationReport.ShiftDay;
						reportHistoryOPQty.StepSequenceCode = product.NowSimulationReport.StepSequenceCode;
						reportHistoryOPQty.TimePeriodCode = product.NowSimulationReport.TimePeriodCode;
						reportHistoryOPQty.AllGoodQty = 0;
						reportHistoryOPQty.ResourceCode = product.NowSimulationReport.ResourceCode;
						reportHistoryOPQty.SegmnetCode = product.NowSimulationReport.SegmentCode;
						reportHistoryOPQty.Week = WeekOfYear(reportHistoryOPQty.ShiftDay.ToString());
						reportHistoryOPQty.OPCode =  product.NowSimulationReport.OPCode;
						reportHistoryOPQty.Month =	int.Parse(reportHistoryOPQty.ShiftDay.ToString().Substring(4,2));//DateTime.Now.Month;
					}
					else
					{
						reportHistoryOPQty = (ReportHistoryOPQty)obj;
					}
					#endregion

					//Laws Lu,2005/09/13,����	������ʱ��
					//2006/11/17,Laws Lu add get DateTime from db Server
					DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

					DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);

					reportHistoryOPQty.LastUpdateTime = FormatHelper.TOTimeInt(dtNow);
					//qty = 0;
					reportHistoryOPQty.OuputQty = qty;
					reportHistoryOPQty.QtyFlag = flag;
					reportHistoryOPQty.NGTimes = ngtimes;
					//Laws Lu,2005/11/07,��Դ��ͨ������
					reportHistoryOPQty.EAttribute2 = iPassQty;
					reportHistoryOPQty.ErrorGroup2Err = product.ECG2ErrCodes;

					if (obj == null)
					{
						//reportHistoryOPQty.QtyFlag = flag;

						reportFacade.AddReportHistoryOPQty(reportHistoryOPQty);
					}
					else
					{
					
						//Laws Lu,2005/10/10,�޸�	����������
						reportFacade.ModifyReportHistoryOPQty(reportHistoryOPQty,qty,ngtimes);
					}
				}
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}

			dataCollectDebug.WhenFunctionOut(messages);

			return messages;
		}
		//Laws Lu,2006/03/20	FQCʱ��¼��Դ�����������
		public Messages SetReportResQuanMaster(IDomainDataProvider domainDataProvider, string actionType,ProductInfo product,int times,int total,int ng)
		{
			Messages messages = new Messages();
			DataCollectDebug dataCollectDebug = new DataCollectDebug("ReportResQuanMaster");
			dataCollectDebug.WhenFunctionIn(messages);
			ReportFacade reportFacade = new ReportFacade(domainDataProvider);
			ReportHistoryOPQty reportHistoryOPQty = new ReportHistoryOPQty();
			//Laws Lu,2005/10/10,����
			int ngtimes = 0;
			int qty = 0;
			int iPassQty = 0;
			try
			{
				//Laws  Lu,2005/10/13,����	QtyFlag����Ϊ����
				string flag = String.Empty;
			
				if (IsLastOP(product.NowSimulationReport.MOCode,product.NowSimulationReport.RouteCode,product.NowSimulationReport.OPCode))//�Ƿ�Ϊ�����
					flag = "Y";
				else
					flag = "N";
			

				//AMOI  MARK  START  20050806 ���Ӱ�װ�����ϡ��ְ� ���ֲ��������в��������� ��ο�SPEC
				//Laws Lu,2005/08/29,�޸�	��Ҫ���Էְ����
				if (product.NowSimulation.LastAction == ActionType.DataCollectAction_OQCLotAddID)
				{
					iPassQty = 1 * total;
					qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule * times);
				}
				else if (product.NowSimulation.LastAction == ActionType.DataCollectAction_OQCLotRemoveID)
				{
					iPassQty = 1 * total;
					qty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule * times);
				}// Added By Hi1/Venus.Feng on 20080821 for Hisense Version
                else if (product.NowSimulation.LastAction == ActionType.DataCollectAction_Carton)
                {
                    iPassQty = 1;
                    qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule * times);
                }// ENd Added
				else if (product.NowSimulation.ProductStatus == "GOOD" )
				{
					if (!CheckNeedCountInthisResoure(product))
					{
						iPassQty = 1 * total;
						qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule * times);
					}					
				}
				else if (product.NowSimulation.ProductStatus == "NG")
				{
					//Laws Lu,2005/11/05,�޸�	����ְ����
					ngtimes = 1 * ng;
					//NG �����ظ��ɼ�,�ɼ�NG֮��ͻᵽTS,�ٴβɼ�NGʱ�ᱨ�������
					if (CheckNeedCountInthisResoure(product))
					{
						if(!(product.LastSimulation.ProductStatus == "GOOD"))
						{
							iPassQty = 1 * total;	
						}

						//�����һ�βɼ�NG,Ȼ�������ɼ�NG��ɸ�����
						if(!(product.LastSimulation.ProductStatus == "NG"))
							qty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule * times);
					}
					else
					{
						iPassQty = 1 * total;
					}
				}
				else
				{
					iPassQty = 1 * total;
				}
				
				messages.AddMessages(UpdateReportResQuanMaster(
					domainDataProvider
					,flag
					,qty
					,ngtimes
					,iPassQty
					,reportHistoryOPQty
					,product));

			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}

		// Added by Icyer 2005/10/29
		// ��չһ����ActionCheckStatus�����ķ���
		public Messages ReportResQuanMaster(IDomainDataProvider domainDataProvider, string actionType,ProductInfo product, ActionCheckStatus actionCheckStatus)
		{
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug("ReportResQuanMaster");
			dataCollectDebug.WhenFunctionIn(messages);
			ReportFacade reportFacade = new ReportFacade(domainDataProvider);
			ReportHistoryOPQty reportHistoryOPQty = new ReportHistoryOPQty();
			//Laws Lu,2005/10/10,����
			int ngtimes = 0;
			int qty = 0;
			int iPassQty = 0;
			try
			{
				//Laws  Lu,2005/10/13,����	QtyFlag����Ϊ����
				string flag = String.Empty;

				// Changed by Icyer 2005/10/28
				/*
				//Laws Lu,2005/10/18,�޸�	�Ƿ�Ϊ�����
				if (IsLastOP(product.NowSimulationReport.MOCode,product.NowSimulationReport.RouteCode,product.NowSimulationReport.OPCode))//�Ƿ�Ϊ�����
					flag = "Y";
				else
					flag = "N";
				*/
				if (actionCheckStatus == null || actionCheckStatus.IsLastOP == string.Empty)
				{
					if (IsLastOP(product.NowSimulationReport.MOCode,product.NowSimulationReport.RouteCode,product.NowSimulationReport.OPCode))//�Ƿ�Ϊ�����
						flag = "Y";
					else
						flag = "N";
					if (actionCheckStatus != null)
					{
						actionCheckStatus.IsLastOP = flag;
					}
				}
				else
				{
					flag = actionCheckStatus.IsLastOP;
				}
				// Changed end

				//AMOI  MARK  START  20050806 ���Ӱ�װ�����ϡ��ְ� ���ֲ��������в��������� ��ο�SPEC
				//Laws Lu,2005/08/29,�޸�	��Ҫ���Էְ����
				if (product.NowSimulation.LastAction == ActionType.DataCollectAction_OQCLotAddID)
				{
					iPassQty = 1;
					//reportHistoryOPQty.OuputQty += 1 * product.NowSimulation.IDMergeRule;
					qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
				}
				else if (product.NowSimulation.LastAction == ActionType.DataCollectAction_OQCLotRemoveID)
				{
					iPassQty = -1;
					//reportHistoryOPQty.OuputQty -= 1 * product.NowSimulation.IDMergeRule;
					qty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule);
                }// Added By Hi1/Venus.Feng on 20080821 for Hisense Version
                else if (product.NowSimulation.LastAction == ActionType.DataCollectAction_Carton)
                {
                    iPassQty = 1;
                    qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
                }// ENd Added
				else if (product.NowSimulation.ProductStatus == "GOOD" )
				{
					if (!CheckNeedCountInthisResoure(product))
					{
						iPassQty = 1;
						//reportHistoryOPQty.OuputQty += 1 * product.NowSimulation.IDMergeRule;
						qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
					}					
				}
				else if (product.NowSimulation.ProductStatus == "NG")
				{
					
					//reportHistoryOPQty.NGTimes += 1 * product.NowSimulation.IDMergeRule;
					//Laws Lu,2005/11/05,�޸�	����ְ����
					ngtimes = 1;
					//NG �����ظ��ɼ�,�ɼ�NG֮��ͻᵽTS,�ٴβɼ�NGʱ�ᱨ�������
					if (CheckNeedCountInthisResoure(product))
					{
						//reportHistoryOPQty.OuputQty -= 1 * product.NowSimulation.IDMergeRule;
						if(!(product.LastSimulation.ProductStatus == "GOOD"))
						{
							iPassQty = 1;	
						}

						//�����һ�βɼ�NG,Ȼ�������ɼ�NG��ɸ�����
						if(!(product.LastSimulation.ProductStatus == "NG"))
							qty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule);
					}
					else
					{
						iPassQty = 1;
					}
				}
				else
				{
					iPassQty = 1;
				}
				
				//AMOI  MARK  END
				//Laws Lu,2005/10/28,�޸�	û������ʲô����������ʡϵͳ��Դ
				if(qty != 0 || ngtimes != 0 || iPassQty != 0 || product.ECG2ErrCodes != String.Empty)
				{
					#region ��ȡ����ʵ��
					object obj = reportFacade.GetReportHistoryOPQty(product.NowSimulationReport.ModelCode,product.NowSimulationReport.ShiftDay,
						product.NowSimulationReport.MOCode,product.NowSimulationReport.TimePeriodCode,product.NowSimulationReport.StepSequenceCode,product.NowSimulationReport.SegmentCode,
						product.NowSimulationReport.ItemCode,product.NowSimulationReport.ShiftCode,product.NowSimulationReport.OPCode,product.NowSimulationReport.ResourceCode,flag);
					if (obj == null)
					{
						reportHistoryOPQty.Day = product.NowSimulationReport.MaintainDate;
						reportHistoryOPQty.ItemCode = product.NowSimulationReport.ItemCode;
						reportHistoryOPQty.MOCode = product.NowSimulationReport.MOCode;
						reportHistoryOPQty.ModelCode = product.NowSimulationReport.ModelCode;
					
						reportHistoryOPQty.QtyFlag = "N";
						reportHistoryOPQty.ShiftCode = product.NowSimulationReport.ShiftCode;
						reportHistoryOPQty.ShiftDay = product.NowSimulationReport.ShiftDay;
						reportHistoryOPQty.StepSequenceCode = product.NowSimulationReport.StepSequenceCode;
						reportHistoryOPQty.TimePeriodCode = product.NowSimulationReport.TimePeriodCode;
						reportHistoryOPQty.AllGoodQty = 0;
						reportHistoryOPQty.ResourceCode = product.NowSimulationReport.ResourceCode;
						reportHistoryOPQty.SegmnetCode = product.NowSimulationReport.SegmentCode;
						reportHistoryOPQty.Week = WeekOfYear(reportHistoryOPQty.ShiftDay.ToString());
						reportHistoryOPQty.OPCode =  product.NowSimulationReport.OPCode;
						reportHistoryOPQty.Month =	int.Parse(reportHistoryOPQty.ShiftDay.ToString().Substring(4,2));//DateTime.Now.Month;
					}
					else
					{
						reportHistoryOPQty = (ReportHistoryOPQty)obj;
					}
					#endregion

					//Laws Lu,2005/09/13,����	������ʱ��
					//2006/11/17,Laws Lu add get DateTime from db Server
					DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

					DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);

					reportHistoryOPQty.LastUpdateTime = FormatHelper.TOTimeInt(dtNow);
					//qty = 0;
					reportHistoryOPQty.OuputQty = qty;
					reportHistoryOPQty.QtyFlag = flag;
					reportHistoryOPQty.NGTimes = ngtimes;
					//Laws Lu,2005/11/07,��Դ��ͨ������
					reportHistoryOPQty.EAttribute2 = iPassQty;
					reportHistoryOPQty.ErrorGroup2Err = product.ECG2ErrCodes;

					#region Add rpt for OutLine Report
					//Laws Lu,2006/06/12 get itemroute 2 op
					if(product.CurrentItemRoute2OP == null &&  product.NowSimulation.RouteCode != String.Empty)
					{
						object objItemRoute2op = (new ItemFacade(DataProvider)).GetItemRoute2Operation(
							product.NowSimulation.ItemCode
							,product.NowSimulation.RouteCode
							,product.NowSimulation.OPCode);
						if(objItemRoute2op != null)
						{
							product.CurrentItemRoute2OP =  objItemRoute2op as ItemRoute2OP;
						}
					}
					if(product.CurrentItemRoute2OP != null 
						&& product.CurrentItemRoute2OP.OptionalOP != null 
						&& product.CurrentItemRoute2OP.OptionalOP != String.Empty)
					{
						//Laws Lu,2006/06/08 count rpt record 
						int iRecordCount = reportFacade.QueryOutLineReportHistoryOPQtyCount(product.NowSimulationReport.ModelCode,product.NowSimulationReport.ShiftDay,
							product.NowSimulationReport.MOCode,product.NowSimulationReport.TimePeriodCode,product.NowSimulationReport.StepSequenceCode,product.NowSimulationReport.SegmentCode,
							product.NowSimulationReport.ItemCode,product.NowSimulationReport.ShiftCode,product.CurrentItemRoute2OP.OptionalOP,OutLineResource/*product.NowSimulationReport.ResourceCode*/);

						if(iRecordCount <= 0)
						{
							ReportHistoryOPQty rptTmp = new ReportHistoryOPQty();
							rptTmp.Day = reportHistoryOPQty.Day;
							rptTmp.ItemCode = reportHistoryOPQty.ItemCode;
							rptTmp.MOCode = reportHistoryOPQty.MOCode;
							rptTmp.ModelCode = reportHistoryOPQty.ModelCode;
					
							rptTmp.QtyFlag = reportHistoryOPQty.QtyFlag;
							rptTmp.ShiftCode = reportHistoryOPQty.ShiftCode;
							rptTmp.ShiftDay = reportHistoryOPQty.ShiftDay;
							rptTmp.StepSequenceCode = reportHistoryOPQty.StepSequenceCode;
							rptTmp.TimePeriodCode = reportHistoryOPQty.TimePeriodCode;
							rptTmp.AllGoodQty = reportHistoryOPQty.AllGoodQty;
							rptTmp.ResourceCode = OutLineResource;
							rptTmp.SegmnetCode = reportHistoryOPQty.SegmnetCode;
							rptTmp.Week = reportHistoryOPQty.Week;
							rptTmp.OPCode = product.CurrentItemRoute2OP.OptionalOP;
							//rptTmp.OPCode =  reportHistoryOPQty.OPCode;
							rptTmp.Month =	reportHistoryOPQty.Month;//DateTime.Now.Month;

							//Laws Lu,2005/09/13,����	������ʱ��
							rptTmp.LastUpdateTime = reportHistoryOPQty.LastUpdateTime;
							//qty = 0;
							rptTmp.OuputQty = reportHistoryOPQty.OuputQty;
							rptTmp.QtyFlag = reportHistoryOPQty.QtyFlag;
							rptTmp.NGTimes = 0;//reportHistoryOPQty.NGTimes;
							//Laws Lu,2005/11/07,��Դ��ͨ������
							rptTmp.EAttribute2 = reportHistoryOPQty.EAttribute2;
							rptTmp.ErrorGroup2Err = reportHistoryOPQty.ErrorGroup2Err;

							reportFacade.AddReportHistoryOPQty(rptTmp);
						}
						else
						{
							object objOutLineRpt = reportFacade.GetOutLineReportHistoryOPQty(product.NowSimulationReport.ModelCode,product.NowSimulationReport.ShiftDay,
								product.NowSimulationReport.MOCode,product.NowSimulationReport.TimePeriodCode,product.NowSimulationReport.StepSequenceCode,product.NowSimulationReport.SegmentCode,
								product.NowSimulationReport.ItemCode,product.NowSimulationReport.ShiftCode,product.CurrentItemRoute2OP.OptionalOP,OutLineResource,/*product.NowSimulationReport.ResourceCode*/flag);

							ReportHistoryOPQty rptTmp = objOutLineRpt as ReportHistoryOPQty;
							//rptTmp.OuputQty
							rptTmp.EAttribute2 = iPassQty;
							rptTmp.ErrorGroup2Err = reportHistoryOPQty.ErrorGroup2Err;
							reportFacade.ModifyReportHistoryOPQty(objOutLineRpt as ReportHistoryOPQty,iPassQty,ngtimes);
						}
					}
					#endregion

					if (obj == null)
					{
						
						//reportHistoryOPQty.QtyFlag = flag;

						reportFacade.AddReportHistoryOPQty(reportHistoryOPQty);
					}
					else
					{
					
						//Laws Lu,2005/10/10,�޸�	����������
						reportFacade.ModifyReportHistoryOPQty(reportHistoryOPQty,qty,ngtimes);
					}
				}
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}
		// Added end

		//Laws Lu,2006/09/13,��Դ����ͳ�Ʊ���
		//��Դͨ��������Դ������Ŀǰ��֧��
		public Messages ReportResECMaster(IDomainDataProvider domainDataProvider, string actionType,ProductInfo product,object[] ErrorCodes)
		{
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug("ReportResECMaster");
			dataCollectDebug.WhenFunctionIn(messages);
			ReportFacade reportFacade = new ReportFacade(domainDataProvider);
			ReportErrorCode2Resource report = new ReportErrorCode2Resource();
			//Laws Lu,2005/10/10,����
			int ngtimes = 0;
			int qty = 0;
			int iPassQty = 0;
			try
			{
				if (product.NowSimulation.LastAction == ActionType.DataCollectAction_OQCLotAddID)
				{
					iPassQty = 1;
					//reportHistoryOPQty.OuputQty += 1 * product.NowSimulation.IDMergeRule;
					qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
				}
				else if (product.NowSimulation.LastAction == ActionType.DataCollectAction_OQCLotRemoveID)
				{
					iPassQty = -1;
					//reportHistoryOPQty.OuputQty -= 1 * product.NowSimulation.IDMergeRule;
					qty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule);
				}// Added By Hi1/Venus.Feng on 20080821 for Hisense Version
                else if (product.NowSimulation.LastAction == ActionType.DataCollectAction_Carton)
                {
                    iPassQty = 1;
                    qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
                }// ENd Added
				else if (product.NowSimulation.ProductStatus == "GOOD" )
				{
					if (!CheckNeedCountInthisResoure(product))
					{
						iPassQty = 1;
						//reportHistoryOPQty.OuputQty += 1 * product.NowSimulation.IDMergeRule;
						qty = Convert.ToInt32(1 * product.NowSimulation.IDMergeRule);
					}					
				}
				else if (product.NowSimulation.ProductStatus == "NG")
				{
					
					//reportHistoryOPQty.NGTimes += 1 * product.NowSimulation.IDMergeRule;
					//Laws Lu,2005/11/05,�޸�	����ְ����
					ngtimes = 1;
					//NG �����ظ��ɼ�,�ɼ�NG֮��ͻᵽTS,�ٴβɼ�NGʱ�ᱨ�������
					if (CheckNeedCountInthisResoure(product))
					{
						//reportHistoryOPQty.OuputQty -= 1 * product.NowSimulation.IDMergeRule;
						if(!(product.LastSimulation.ProductStatus == "GOOD"))
						{
							iPassQty = 1;	
						}

						//�����һ�βɼ�NG,Ȼ�������ɼ�NG��ɸ�����
						if(!(product.LastSimulation.ProductStatus == "NG"))
							qty = Convert.ToInt32(-1 * product.NowSimulation.IDMergeRule);
					}
					else
					{
						iPassQty = 1;
					}
				}
				else
				{
					iPassQty = 1;
				}
				
				//Laws Lu,2005/10/28,�޸�	û������ʲô����������ʡϵͳ��Դ
				if(qty != 0 || ngtimes != 0 || iPassQty != 0 || product.ECG2ErrCodes != String.Empty)
				{
					#region ��ȡ����ʵ��
					
					if (ErrorCodes != null)
					{
						for (int i=0; i<ErrorCodes.Length; i++)
						{
						
							string ecg = String.Empty;
							string ec = String.Empty;
							//object obj;
							if (actionType==ActionType.DataCollectAction_SMTNG)
							{
								ecg = ((TSErrorCode2Location)ErrorCodes[i]).ErrorCodeGroup;
								ec = ((TSErrorCode2Location)ErrorCodes[i]).ErrorCode;
							}
							else
							{
								ecg = ((ErrorCodeGroup2ErrorCode)ErrorCodes[i]).ErrorCodeGroup;
								ec = ((ErrorCodeGroup2ErrorCode)ErrorCodes[i]).ErrorCode;
							}

							object obj = reportFacade.GetReportErrorCode2Resource(
								product.NowSimulationReport.ModelCode
								,product.NowSimulationReport.ItemCode
								,product.NowSimulationReport.MOCode
								,product.NowSimulationReport.ShiftDay
								,product.NowSimulationReport.ShiftCode,
								product.NowSimulationReport.TimePeriodCode
								,product.NowSimulationReport.OPCode
								,product.NowSimulationReport.ResourceCode
								,product.NowSimulationReport.SegmentCode,
								product.NowSimulationReport.StepSequenceCode
								,ecg
								,ec);

							if (obj == null)
							{
								ShiftModelFacade shiftModelFacade = new ShiftModelFacade(domainDataProvider);

								report.Day = product.NowSimulationReport.MaintainDate;
								report.ItemCode = product.NowSimulationReport.ItemCode;
								report.MOCode = product.NowSimulationReport.MOCode;
								report.ModelCode = product.NowSimulationReport.ModelCode;
								report.ErrorCodeGroup = ecg;
								report.ErrorCode  = ec;
					
								report.ShiftCode = product.NowSimulationReport.ShiftCode;
								report.ShiftDay = product.NowSimulationReport.ShiftDay;
								report.SSCode = product.NowSimulationReport.StepSequenceCode;
								report.TPCode = product.NowSimulationReport.TimePeriodCode;
								report.TPBTime = shiftModelFacade.GetTimePeriodBeginTime(report.TPCode);
								report.TPETime= shiftModelFacade.GetTimePeriodEndTime(report.TPCode);
								//report.t
								report.ResourceCode = product.NowSimulationReport.ResourceCode;
								report.SegCode = product.NowSimulationReport.SegmentCode;
								report.Week = WeekOfYear(report.ShiftDay.ToString());
								report.OPCode =  product.NowSimulationReport.OPCode;
								report.Month =	int.Parse(report.ShiftDay.ToString().Substring(4,2));//DateTime.Now.Month;
							}
							else
							{
								report = (ReportErrorCode2Resource)obj;
							}
							#endregion

							report.NGTimes = ngtimes;
				

							if (obj == null)
							{
								reportFacade.AddReportErrorCode2Resource(report);
							}
							else
							{
					
								//Laws Lu,2005/10/10,�޸�	����������
								reportFacade.UpdateReportErrorCode2ResourceQty(report);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}

		//TODO:Laws Lu,2005/10/10,Ӧ����û�п��ǵ��������ع������
		//AMOI  MARK  START  20050806 ����Ƿ��ڵ�ǰվ�Ѿ�����ͳ��
		private bool CheckNeedCountInthisResoure(ProductInfo product)
		{
			if (product.NowSimulation.ResourceCode == product.LastSimulation.ResourceCode
						&& product.NowSimulation.OPCode == product.LastSimulation.OPCode
						&&
						(
				           product.LastSimulation.LastAction == ActionType.DataCollectAction_GOOD 
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_SMTGOOD
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_OutLineGood
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_CollectKeyParts
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_CollectINNO 
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_IDTran 
						|| product.LastSimulation.LastAction == ActionType.DataCollectAction_Split
						 ) 
						)
				return true;
			else
				return false;
		}
		//AMOI  MARK  END

		/// <summary>
		/// By Line ͳ��errorcode�Ĵ���
		/// Laws Lu,2005/10/10,�޸�	������
		/// </summary>
		/// <param name="domainDataProvider"></param>
		/// <param name="actionType"></param>
		/// <param name="product"></param>
		/// <param name="ErrorCodes"></param>
		/// <returns></returns>
		public Messages ReportLineECOQuanMaster(IDomainDataProvider domainDataProvider, string actionType,ProductInfo product,object[] ErrorCodes)
		{
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug("ReportLineECOQuanMaster");
			dataCollectDebug.WhenFunctionIn(messages);
			ReportFacade reportFacade = new ReportFacade(domainDataProvider);
			ReportRealtimeLineErrorCodeQty reportRealtimeLineErrorCodeQty = new ReportRealtimeLineErrorCodeQty();
			//Laws Lu,2005/10/10,����
			int ngtimes = 0;
			try
			{
				if (ErrorCodes != null)
				{
					for (int i=0; i<ErrorCodes.Length; i++)
					{
						
						object obj;
						if (actionType==ActionType.DataCollectAction_SMTNG)
						{
							obj = reportFacade.GetReportRealtimeLineErrorCodeQty(product.NowSimulationReport.ModelCode,product.NowSimulationReport.ShiftDay,
								product.NowSimulationReport.MOCode,product.NowSimulationReport.TimePeriodCode,product.NowSimulationReport.StepSequenceCode,
								product.NowSimulationReport.SegmentCode,product.NowSimulationReport.ItemCode,product.NowSimulationReport.ShiftCode,
								((TSErrorCode2Location)ErrorCodes[i]).ErrorCode,((TSErrorCode2Location)ErrorCodes[i]).ErrorCodeGroup);
						}
						else
						{
							obj = reportFacade.GetReportRealtimeLineErrorCodeQty(product.NowSimulationReport.ModelCode,product.NowSimulationReport.ShiftDay,
								product.NowSimulationReport.MOCode,product.NowSimulationReport.TimePeriodCode,product.NowSimulationReport.StepSequenceCode,
								product.NowSimulationReport.SegmentCode,product.NowSimulationReport.ItemCode,product.NowSimulationReport.ShiftCode,
								((ErrorCodeGroup2ErrorCode)ErrorCodes[i]).ErrorCode,((ErrorCodeGroup2ErrorCode)ErrorCodes[i]).ErrorCodeGroup);
							
						}
						if (obj==null)
						{
							reportRealtimeLineErrorCodeQty.Day = product.NowSimulationReport.MaintainDate;
							if (actionType==ActionType.DataCollectAction_SMTNG)
							{
								reportRealtimeLineErrorCodeQty.ErrorCode = ((TSErrorCode2Location)ErrorCodes[i]).ErrorCode;
								reportRealtimeLineErrorCodeQty.ErrorCodeGroup = ((TSErrorCode2Location)ErrorCodes[i]).ErrorCodeGroup;
							}
							else
							{
								reportRealtimeLineErrorCodeQty.ErrorCode = ((ErrorCodeGroup2ErrorCode)ErrorCodes[i]).ErrorCode;
								reportRealtimeLineErrorCodeQty.ErrorCodeGroup = ((ErrorCodeGroup2ErrorCode)ErrorCodes[i]).ErrorCodeGroup;
								
							}
							reportRealtimeLineErrorCodeQty.ErrorCodeTimes = 0;
							reportRealtimeLineErrorCodeQty.ItemCode = product.NowSimulationReport.ItemCode;
							reportRealtimeLineErrorCodeQty.MOCode = product.NowSimulationReport.MOCode;
							reportRealtimeLineErrorCodeQty.ModelCode = product.NowSimulationReport.ModelCode;
							reportRealtimeLineErrorCodeQty.SegmnetCode = product.NowSimulationReport.SegmentCode;
							reportRealtimeLineErrorCodeQty.ShiftCode = product.NowSimulationReport.ShiftCode;
							reportRealtimeLineErrorCodeQty.StepSequenceCode = product.NowSimulationReport.StepSequenceCode;
							reportRealtimeLineErrorCodeQty.TimePeriodCode = product.NowSimulationReport.TimePeriodCode;
							reportRealtimeLineErrorCodeQty.Day = product.NowSimulationReport.ShiftDay;
							reportRealtimeLineErrorCodeQty.ShiftDay = product.NowSimulationReport.ShiftDay;
						}
						else
						{
							reportRealtimeLineErrorCodeQty = (ReportRealtimeLineErrorCodeQty)obj;
						}
						if (product.NowSimulation.ProductStatus == "NG" || product.NowSimulation.ProductStatus == "REJECT")
						{
							//reportRealtimeLineErrorCodeQty.ErrorCodeTimes += 1;
							ngtimes = 1;
						}
						if(ngtimes != 0)
						{
							reportRealtimeLineErrorCodeQty.ErrorCodeTimes = Convert.ToDecimal(ngtimes);
							if (obj == null)
							{
								reportFacade.AddReportRealtimeLineErrorCodeQty(reportRealtimeLineErrorCodeQty);
							}
							else
							{
								//Laws Lu,2005/10/10,�޸�	������
								reportFacade.ModifyReportRealtimeLineErrorCodeQty(reportRealtimeLineErrorCodeQty,ngtimes);
							}
							
						}
					}
				}
				
				
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}



	}
	/// <summary>
	/// Laws Lu,2006/03/13 �ܵļ���
	/// </summary>
	/// 
	[Serializable]
	public class WeekObject:DomainObject
	{
		[FieldMapAttribute("Week", typeof(int), 2,false)]
		public int  Week;
	}

	/// <summary>
	/// Added by jessie lee, 2005/1/10
	/// </summary>
	/// 
	[Serializable]
	public struct ReportQtyHelper
	{
		public decimal  OuputQty;
		public decimal  AllGoodQty;
		//public decimal  EAttribute1;
		//public decimal  EAttribute2;
		//public decimal  EAttribute3;
		//public decimal  ScrapQty;
		//public decimal  InputQty;
		public decimal  MoAllGoodQty;
		public ReportQtyHelper( 
			decimal  ouputQty,
			decimal  allGoodQty,
			//decimal  eAttribute1,
			//decimal  eAttribute2,
			//decimal  eAttribute3,
			//decimal  scrapQty,
			//decimal  inputQty,
			decimal  moAllGoodQty)
		{
			OuputQty    = ouputQty ;
			AllGoodQty  = allGoodQty ;
			//EAttribute1 = eAttribute1 ;
			//EAttribute2 = eAttribute2 ;
			//EAttribute3 = eAttribute3 ;
			//ScrapQty    = scrapQty ;
			//InputQty    = inputQty ;
			MoAllGoodQty= moAllGoodQty ;
		}		
	}
}
