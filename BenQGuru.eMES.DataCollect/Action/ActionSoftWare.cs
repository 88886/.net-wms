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
	/// ���������ɼ�
	/// </summary>
	public class ActionSoftWare:IActionWithStatus
	{
		
		private IDomainDataProvider _domainDataProvider = null;

//		public ActionSoftWare()
//		{	
//		}

		public ActionSoftWare(IDomainDataProvider domainDataProvider)
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

		/// <summary>
		/// ��Ʒ�ɼ�
		/// </summary>
		/// <param name="domainDataProvider"></param>
		/// <param name="iD"></param>
		/// <param name="actionType"></param>
		/// <param name="resourceCode"></param>
		/// <param name="userCode"></param>
		/// <param name="product"></param>
		/// <param name="datas1">����</param>
		/// <param name="datas2">NULL</param>
		/// <returns></returns>
		public Messages  Execute(ActionEventArgs actionEventArgs)
		{				
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				DataCollectFacade facade = new DataCollectFacade(this.DataProvider);
				ActionOnLineHelper helper = new ActionOnLineHelper(this.DataProvider);
				
				if (actionEventArgs.ProductInfo == null)
				{
					// ���LastSimulation
					messages.AddMessages( helper.GetIDInfo(actionEventArgs.RunningCard) );

					if ( messages.IsSuccess() )
					{
						actionEventArgs.ProductInfo = (ProductInfo)messages.GetData().Values[0];
					}
				}	

				if ( actionEventArgs.ProductInfo == null )
				{				
					throw new Exception("$NoProductInfo");
				}

				if ( actionEventArgs.ProductInfo.LastSimulation == null)
				{
					throw new Exception("$NoSimulationInfo");
				}
		
				if ( messages.IsSuccess() )
				{
					facade.CheckMO(actionEventArgs.ProductInfo.LastSimulation.MOCode,actionEventArgs.ProductInfo);
					facade.GetRouteOPOnline(actionEventArgs.RunningCard, actionEventArgs.ActionType, actionEventArgs.ResourceCode, actionEventArgs.UserCode, actionEventArgs.ProductInfo ); 
					
					DBDateTime dbDateTime;
						
					dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

					actionEventArgs.ProductInfo.NowSimulation.MaintainUser = actionEventArgs.UserCode;

					actionEventArgs.ProductInfo.NowSimulation.MaintainDate = dbDateTime.DBDate ;
					actionEventArgs.ProductInfo.NowSimulation.MaintainTime = dbDateTime.DBTime ;

					// ��дNowSimulationReport
					actionEventArgs.ProductInfo.NowSimulationReport = helper.FillSimulationReport( actionEventArgs.ProductInfo );

					// ��дSoftwareVersion����
					this.WriteSoftwareInfo( (SoftwareActionEventArgs)actionEventArgs, facade );
				}
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}

		// Added by Icyer 2005/10/28
		public Messages  Execute(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
		{				
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				DataCollectFacade facade = new DataCollectFacade(this.DataProvider);
				ActionOnLineHelper helper = new ActionOnLineHelper(this.DataProvider);
				
				if (actionEventArgs.ProductInfo == null)
				{
					actionEventArgs.ProductInfo = actionCheckStatus.ProductInfo;
					if (actionEventArgs.ProductInfo == null)
					{
						// ���LastSimulation
						messages.AddMessages( helper.GetIDInfo(actionEventArgs.RunningCard) );
						if ( messages.IsSuccess() )
						{
							actionEventArgs.ProductInfo = (ProductInfo)messages.GetData().Values[0];
							actionCheckStatus.ProductInfo = (ProductInfo)messages.GetData().Values[0];
						}
					}
				}	

				if ( actionEventArgs.ProductInfo == null )
				{				
					throw new Exception("$NoProductInfo");
				}

				if ( actionEventArgs.ProductInfo.LastSimulation == null)
				{
					throw new Exception("$NoSimulationInfo");
				}
		
				if ( messages.IsSuccess() )
				{
					//���û�м�������
					if (actionCheckStatus.CheckedMO == false)
					{
						facade.CheckMO(actionEventArgs.ProductInfo.LastSimulation.MOCode,actionEventArgs.ProductInfo);
					}
					facade.GetRouteOPOnline(actionEventArgs.RunningCard, actionEventArgs.ActionType, actionEventArgs.ResourceCode, actionEventArgs.UserCode, actionEventArgs.ProductInfo, actionCheckStatus ); 
					//Laws Lu,2006/11/13 add uniform collect date time
					DBDateTime dbDateTime;
						
					dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

					actionEventArgs.ProductInfo.NowSimulation.MaintainUser = actionEventArgs.UserCode;

					actionEventArgs.ProductInfo.NowSimulation.MaintainDate = dbDateTime.DBDate ;
					actionEventArgs.ProductInfo.NowSimulation.MaintainTime = dbDateTime.DBTime ;

					actionEventArgs.ProductInfo.NowSimulationReport = helper.FillSimulationReport( actionEventArgs.ProductInfo );

					// ��дSoftwareVersion����
					this.WriteSoftwareInfo( (SoftwareActionEventArgs)actionEventArgs, facade, actionCheckStatus );

					//��Action�����б�
					actionCheckStatus.ActionList.Add(actionEventArgs);
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

		private void WriteSoftwareInfo( SoftwareActionEventArgs actionEventArgs, DataCollectFacade facade)
		{
			WriteSoftwareInfo(actionEventArgs, facade, null);
		}
		private void WriteSoftwareInfo( SoftwareActionEventArgs actionEventArgs, DataCollectFacade facade, ActionCheckStatus actionCheckStatus )
		{
			SimulationReport report = actionEventArgs.ProductInfo.NowSimulationReport;

			OnWIPSoftVersion onWIPSoftVersion = facade.CreateNewOnWIPSoftVersion();

			onWIPSoftVersion.SoftwareVersion	= actionEventArgs.SoftwareVersion;	
			onWIPSoftVersion.SoftwareName		= actionEventArgs.SoftwareName;
			onWIPSoftVersion.RunningCard		= report.RunningCard;
			onWIPSoftVersion.RunningCardSequence = report.RunningCardSequence;
			onWIPSoftVersion.MOCode				= report.MOCode;
			onWIPSoftVersion.ItemCode			= report.ItemCode;
			onWIPSoftVersion.ResourceCode		= report.ResourceCode;
			onWIPSoftVersion.OPCode				= report.OPCode;
			onWIPSoftVersion.RouteCode			= report.RouteCode;
			onWIPSoftVersion.ModelCode			= report.ModelCode;
			onWIPSoftVersion.MaintainDate		= report.MaintainDate;
			onWIPSoftVersion.MaintainTime		= report.MaintainTime;
			onWIPSoftVersion.MaintainUser		= report.MaintainUser;
			onWIPSoftVersion.ShiftTypeCode		= report.ShiftTypeCode;
			onWIPSoftVersion.StepSequenceCode	= report.StepSequenceCode;
			onWIPSoftVersion.SegmnetCode		= report.SegmentCode;	
			onWIPSoftVersion.ShiftCode			= report.ShiftCode;
			onWIPSoftVersion.TimePeriodCode		= report.TimePeriodCode;
            onWIPSoftVersion.MOSeq = report.MOSeq;  // Added by Icyer 2007/07/02

			if (actionCheckStatus == null || actionCheckStatus.NeedUpdateSimulation == true)
			{
				facade.AddOnWIPSoftVersion( onWIPSoftVersion );
			}
			else
			{
				actionEventArgs.OnWIP.Add(onWIPSoftVersion);
			}
		}
	}
}
