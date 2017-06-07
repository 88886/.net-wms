using System;
using UserControl;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.DataCollect;
using System.Collections;

namespace  BenQGuru.eMES.DataCollect.Action
{
	/// <summary>
	/// ���빤���ɼ�
	/// </summary>
	public class ActionOffMo: IActionWithStatus
	{
		private IDomainDataProvider _domainDataProvider = null;

		public ActionOffMo(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
		}

		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		public Messages Execute(ActionEventArgs actionEventArgs)
		{				
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
				//��дSIMULATION ��鹤����ID��;�̡�����
				messages.AddMessages( dataCollect.CheckID(actionEventArgs));
				
				//����Ƿ��깤
				if(actionEventArgs.ProductInfo.LastSimulation.IsComplete == "1")
				{
					messages.Add(new UserControl.Message(MessageType.Error,"$CS_PRODUCT_ALREADY_COMPLETE $CS_Param_OPCode  =" + actionEventArgs.ProductInfo.LastSimulation.OPCode));
				}
				if (messages.IsSuccess())
				{				
					//Laws Lu,2005/08/15,����	�깤�߼���������Check��ͨ��������£����е�RunningCardӦ����GOOD״̬
					//��ʱ���������⹤��
					actionEventArgs.ProductInfo.NowSimulation.IsComplete = "1";
					actionEventArgs.ProductInfo.NowSimulation.EAttribute1 = ProductStatus.OffMo;
					actionEventArgs.ProductInfo.NowSimulation.ProductStatus = ProductStatus.OffMo;
					

					//End Laws Lu
						messages.AddMessages( dataCollect.Execute(actionEventArgs));
					if (messages.IsSuccess())
					{
						//����ά�޼�¼
						if(actionEventArgs.ProductInfo.LastTS != null)
						{
							//2006/02/08	�޸�	�޸�ά�޼�¼״̬
							Domain.TS.TS  ts = actionEventArgs.ProductInfo.LastTS;

							TS.TSFacade tsFAC = new BenQGuru.eMES.TS.TSFacade(DataProvider);

//							if(ts.TSStatus == TSStatus.TSStatus_New)
//							{
//								tsFAC.DeleteTS(ts);
//							}
//							else
//							{
							tsFAC.UpdateTSStatus(ts.TSId,TSStatus.TSStatus_OffMo,actionEventArgs.UserCode);
//							}

//							ts.TSId = FormatHelper.GetUniqueID(actionEventArgs.ProductInfo.NowSimulation.MOCode
//								,ts.RunningCard,ts.RunningCardSequence.ToString());
//
//							ts.MaintainUser = actionEventArgs.UserCode;
//							ts.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
//							ts.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
//
//							ts.TSStatus = TSStatus.TSStatus_OffMo;
//
//							
//							tsFAC.AddTS(ts);
						}
							
						///���Ͽۿ��
						///
						//Laws Lu,2005/10/20,����	ʹ�������ļ�����������ģ���Ƿ�ʹ��
						if(System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
						{
							BenQGuru.eMES.Material.CastDownHelper castHelper = new BenQGuru.eMES.Material.CastDownHelper(DataProvider);
							ArrayList arRcard = new ArrayList();

							castHelper.GetAllRCardByMo(ref arRcard,actionEventArgs.ProductInfo.NowSimulation.RunningCard,actionEventArgs.ProductInfo.NowSimulation.MOCode);

							if(arRcard.Count == 0)
							{
								arRcard.Add(actionEventArgs.RunningCard);
							}

							string runningCards = "('" + String.Join("','",(string[])arRcard.ToArray(typeof(string))) + "')";
							//���ϲ��黹�ⷿ
							BenQGuru.eMES.Material.WarehouseFacade wfacade = new BenQGuru.eMES.Material.WarehouseFacade(this.DataProvider);
							wfacade.DropMaterialStock(runningCards
								, actionEventArgs.ProductInfo.NowSimulation.MOCode,actionEventArgs.ProductInfo.NowSimulation.OPCode);
						}

						//��д���Ա��� TODO
                        //ReportHelper reportCollect= new ReportHelper(this.DataProvider);
                        //messages.AddMessages(reportCollect.ReportLineQuanMaster(this.DataProvider,actionEventArgs.ActionType,actionEventArgs.ProductInfo));
                        //messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider,actionEventArgs.ActionType,actionEventArgs.ProductInfo));
						
						//��д���빤����
						MOFacade moFAC = new MOFacade(DataProvider);

						OffMoCard offCard = new OffMoCard();
						offCard.PK = System.Guid.NewGuid().ToString();
						offCard.MoCode = actionEventArgs.ProductInfo.NowSimulation.MOCode;
						offCard.RCARD = actionEventArgs.RunningCard;
						offCard.MoType = (actionEventArgs as OffMoEventArgs).MOType;
						offCard.MUSER = actionEventArgs.UserCode;
						offCard.MDATE = actionEventArgs.ProductInfo.NowSimulation.MaintainDate;
						offCard.MTIME = actionEventArgs.ProductInfo.NowSimulation.MaintainTime;
						

						moFAC.AddOffMoCard(offCard);
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
		//��ȡ�˲�Ʒ�ڹ����Ϲ�����Ʒ
		public object[] GetMoUsedItem(string runningCard,decimal runningCardSequence,string moCode)
		{
			 string selectSQL =" select distinct mitemcode from tblonwipitem where rcard = '{0}' and mocode = '{1}'" ;

			 return this.DataProvider.CustomQuery(typeof(ONWIPItemQueryObject),new SQLCondition(String.Format(selectSQL,new object[] {runningCard,moCode})));
		}
	
		//Laws Lu,2006/01/04���˷����Ѿ�����ʹ��
		public Messages Execute(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
		{				
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
				//��дSIMULATION ��鹤����ID��;�̡�����
				messages.AddMessages( dataCollect.CheckID(actionEventArgs, actionCheckStatus));
				
				if (messages.IsSuccess())
				{				
					//Laws Lu,2005/08/15,����	�깤�߼���������Check��ͨ��������£����е�RunningCardӦ����GOOD״̬
					//��ʱ���������⹤��
					DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
					if (actionEventArgs.ProductInfo.NowSimulation.RouteCode != "" && dataCollectFacade.OPIsMORouteLastOP(
						actionEventArgs.ProductInfo.NowSimulation.MOCode
						,actionEventArgs.ProductInfo.NowSimulation.RouteCode
						,actionEventArgs.ProductInfo.NowSimulation.OPCode))
					{
						actionEventArgs.ProductInfo.NowSimulation.IsComplete = "1";
						actionEventArgs.ProductInfo.NowSimulation.EAttribute1 = "GOOD";
					}
					//End Laws Lu
					if (actionCheckStatus.NeedUpdateSimulation == true)
					{
						messages.AddMessages( dataCollect.Execute(actionEventArgs));
					}
					else
					{
						messages.AddMessages( dataCollect.Execute(actionEventArgs, actionCheckStatus));
					}
					if (messages.IsSuccess())
					{
						//��д���Ա��� TODO
                        //if (actionCheckStatus.NeedFillReport == true)
                        //{
                        //    ReportHelper reportCollect= new ReportHelper(this.DataProvider);
                        //    messages.AddMessages(reportCollect.ReportLineQuanMaster(this.DataProvider,actionEventArgs.ActionType,actionEventArgs.ProductInfo, actionCheckStatus));
                        //    messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider,actionEventArgs.ActionType,actionEventArgs.ProductInfo, actionCheckStatus));
                        //}

						//��Action�����б�
						actionCheckStatus.ActionList.Add(actionEventArgs);
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
}
