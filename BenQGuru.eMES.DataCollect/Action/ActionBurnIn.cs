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

namespace BenQGuru.eMES.DataCollect.Action
{
	/// <summary>
	/// ActionBurnIn ��ժҪ˵����
	/// </summary>
	public class ActionBurnIn:IActionWithStatus
	{
		private IDomainDataProvider _domainDataProvider = null;
		public ActionBurnIn( IDomainDataProvider domainDataProvider )
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
                       //�깤�Զ����
                        dataCollectFacade.AutoInventory(actionEventArgs.ProductInfo.NowSimulation, actionEventArgs.UserCode);
					}
					//End Laws Lu

					actionEventArgs.ProductInfo.NowSimulation.ShelfNO = actionEventArgs.ShelfNO;

					messages.AddMessages( dataCollect.Execute(actionEventArgs));
                    //if (messages.IsSuccess())
                    //{
                    //    //��д���Ա��� TODO
                    //    ReportHelper reportCollect= new ReportHelper(this.DataProvider);
                    //    messages.AddMessages(reportCollect.ReportLineQuanMaster(this.DataProvider,actionEventArgs.ActionType,actionEventArgs.ProductInfo));
                    //    messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider,actionEventArgs.ActionType,actionEventArgs.ProductInfo));

                    //}
				}				
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}

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
                        //�깤�Զ����
                        dataCollectFacade.AutoInventory(actionEventArgs.ProductInfo.NowSimulation, actionEventArgs.UserCode);
					}

					actionEventArgs.ProductInfo.NowSimulation.ShelfNO = actionEventArgs.ShelfNO;

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
