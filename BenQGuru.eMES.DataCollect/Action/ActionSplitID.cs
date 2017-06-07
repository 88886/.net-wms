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
	public class ActionSplit:IAction
	{
		
		private IDomainDataProvider _domainDataProvider = null;

//		public ActionSplit()
//		{	
//		}

		public ActionSplit(IDomainDataProvider domainDataProvider)
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
		/// ���к�ת���ɼ���ֻ֧�ְַ壬��֧�ֺϰ�
		/// </summary>
		/// <param name="domainDataProvider"></param>
		/// <param name="iD"></param>
		/// <param name="actionType"></param>
		/// <param name="resourceCode"></param>
		/// <param name="userCode"></param>
		/// <param name="product"></param>
		/// <param name="datas1">ת�����ID��</param>
		/// <param name="datas2">NULL</param>
		/// <returns></returns>
		public Messages  Execute(ActionEventArgs actionEventArgs)
		{				
			// Added by Icyer 2006/10/08
			if (((SplitIDActionEventArgs)actionEventArgs).IsUndo == true)
			{
				return this.UndoExecute((SplitIDActionEventArgs)actionEventArgs);
			}
			// Added end

			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				if (((SplitIDActionEventArgs)actionEventArgs).SplitedIDs == null || ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs.Length == 0)
				{
					throw new Exception("$CS_System_Params_Losted");
				}

				ActionOnLineHelper helper = new ActionOnLineHelper(this.DataProvider);

				//��дSIMULATION ��鹤����ID��;�̡�����
				messages.AddMessages( helper.CheckID(actionEventArgs));
				if (messages.IsSuccess())
				{				
					//
					actionEventArgs.ProductInfo.NowSimulation.IDMergeRule = 1; //actionEventArgs.ProductInfo.NowSimulation.IDMergeRule/((SplitIDActionEventArgs)actionEventArgs).SplitedIDs.Length;
					actionEventArgs.ProductInfo.NowSimulation.TranslateCard = actionEventArgs.ProductInfo.LastSimulation.RunningCard;
					actionEventArgs.ProductInfo.NowSimulation.TranslateCardSequence = actionEventArgs.ProductInfo.LastSimulation.RunningCardSequence;
					actionEventArgs.ProductInfo.NowSimulation.NGTimes = actionEventArgs.ProductInfo.LastSimulation.NGTimes;

					DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);

					for (int i=0;i<((SplitIDActionEventArgs)actionEventArgs).SplitedIDs.Length ;i++)
					{
						//�޸�SIMULATION
						//Laws Lu,2005/08/15,����	�깤�߼���������Check��ͨ��������£����е�RunningCardӦ����GOOD״̬
						//��ʱ���������⹤��
						if (actionEventArgs.ProductInfo.NowSimulation.RouteCode != "" && dataCollectFacade.OPIsMORouteLastOP(
							actionEventArgs.ProductInfo.NowSimulation.MOCode
							,actionEventArgs.ProductInfo.NowSimulation.RouteCode
							,actionEventArgs.ProductInfo.NowSimulation.OPCode))
						{
							actionEventArgs.ProductInfo.NowSimulation.IsComplete = "1";
							actionEventArgs.ProductInfo.NowSimulation.EAttribute1 = "GOOD";
						}
						//End Laws Lu
						actionEventArgs.ProductInfo.NowSimulation.RunningCard = ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs[i].ToString();
						/* added by jessie lee,����� ���к�ת�� */
						if( string.Compare( (actionEventArgs as SplitIDActionEventArgs).IDMergeType,IDMergeType.IDMERGETYPE_IDMERGE,true)==0 )
						{
							/* ת����ͬһ�Ź��� */
							if ( (actionEventArgs as SplitIDActionEventArgs).UpdateSimulation )
							{
								actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence
									= (actionEventArgs as SplitIDActionEventArgs).ExistIMEISeq + 10 ;
							}
							else
							{
								actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence = ActionOnLineHelper.StartSeq ;
							}
						}
						else
						{
							/* ���� ���к�ת�� */
							actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence = ActionOnLineHelper.StartSeq ;
						}
                        actionEventArgs.ProductInfo.NowSimulation.MOSeq = actionEventArgs.ProductInfo.LastSimulation.MOSeq;     // Added by Icyer 2007/07/03
						
						messages.AddMessages( helper.Execute(actionEventArgs));

						if (messages.IsSuccess())
						{
							//��дIDת������
							OnWIPCardTransfer transf = dataCollectFacade.CreateNewOnWIPCardTransfer();

							transf.RunningCard			= ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs[i].ToString().Trim().ToUpper();
							transf.RunningCardSequence	= actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence;
							transf.IDMergeType			= ((SplitIDActionEventArgs)actionEventArgs).IDMergeType;
							transf.TranslateCard		= actionEventArgs.ProductInfo.LastSimulation.RunningCard;
							transf.TranslateCardSequence = actionEventArgs.ProductInfo.LastSimulation.RunningCardSequence;
							transf.SourceCard			= actionEventArgs.ProductInfo.NowSimulation.SourceCard;
							transf.ModelCode			= actionEventArgs.ProductInfo.NowSimulation.ModelCode;
							transf.MOCode				= actionEventArgs.ProductInfo.NowSimulation.MOCode;
							transf.ItemCode				= actionEventArgs.ProductInfo.NowSimulation.ItemCode;
							transf.ResourceCode			= actionEventArgs.ResourceCode;
							transf.OPCode				= actionEventArgs.ProductInfo.NowSimulation.OPCode;
							transf.SourceCardSequence	= actionEventArgs.ProductInfo.NowSimulation.SourceCardSequence;
							transf.RouteCode			= actionEventArgs.ProductInfo.NowSimulation.RouteCode;
							transf.StepSequenceCode		= actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode;
							transf.SegmnetCode			= actionEventArgs.ProductInfo.NowSimulationReport.SegmentCode;
							transf.TimePeriodCode		= actionEventArgs.ProductInfo.NowSimulationReport.TimePeriodCode;
							transf.ShiftCode			= actionEventArgs.ProductInfo.NowSimulationReport.ShiftCode;
							transf.ShiftTypeCode		= actionEventArgs.ProductInfo.NowSimulationReport.ShiftTypeCode;
							transf.MaintainUser			= actionEventArgs.UserCode;
                            transf.MOSeq = actionEventArgs.ProductInfo.NowSimulationReport.MOSeq;

							dataCollectFacade.AddOnWIPCardTransfer( transf );

							// ��ID��ӵ�MO��Χ����
							MORunningCardFacade cardFacade = new MORunningCardFacade(this.DataProvider);
							MORunningCard card = cardFacade.CreateNewMORunningCard();
				
							card.MOCode				= actionEventArgs.ProductInfo.NowSimulation.MOCode;
							card.MORunningCardStart = ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs[i].ToString().Trim().ToUpper();
							card.MORunningCardEnd	= card.MORunningCardStart;
							card.MaintainUser		= actionEventArgs.UserCode;
                            card.MOSeq = actionEventArgs.ProductInfo.NowSimulation.MOSeq;
				
							cardFacade.AddMORunningCard( card );
							//AMOI  MARK  START  20050806 ���Ӱ���Դͳ�Ʋ������깤ͳ��
							#region ��дͳ�Ʊ��� ����Դͳ��
							// Added by Icyer 2006/06/05
                            //if (actionEventArgs.NeedUpdateReport == true)
                            //{
                            //    ReportHelper reportCollect= new ReportHelper(this.DataProvider);
                            //    messages.AddMessages(reportCollect.ReportLineQuanMaster(this.DataProvider,
                            //        actionEventArgs.ActionType,actionEventArgs.ProductInfo));							
                            //    messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider
                            //        ,actionEventArgs.ActionType,actionEventArgs.ProductInfo));
                            //}
							#endregion
							//AMOI  MARK  END
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
		
		// Added by Icyer 2006/11/08
		/// <summary>
		/// ��Undo
		/// </summary>
		public Messages  UndoExecute(SplitIDActionEventArgs actionEventArgs)
		{				

			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				if (((SplitIDActionEventArgs)actionEventArgs).SplitedIDs == null || ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs.Length == 0)
				{
					throw new Exception("$CS_System_Params_Losted");
				}

				DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
				object[] oldSimulation = dataCollectFacade.GetSimulationFromTCard(actionEventArgs.RunningCard);
				if (oldSimulation == null || oldSimulation.Length == 0)
				{
					throw new Exception("$CS_System_Params_Losted");
				}
				if (oldSimulation.Length != actionEventArgs.SplitedIDs.Length)
				{
					throw new Exception("$CS_System_Params_Losted");
				}
				
				ActionOnLineHelper helper = new ActionOnLineHelper(this.DataProvider);
				MORunningCardFacade cardFacade = new MORunningCardFacade(this.DataProvider);
				// ���β���ÿ�����кŸ���
				for (int i = 0; i < actionEventArgs.SplitedIDs.Length; i++)
				{
					// ����Simulation
					Simulation s = (Simulation)oldSimulation[i];
					string oldRCard = s.RunningCard;
					decimal oldRCardSeq = s.RunningCardSequence;
					string strNewRCard = actionEventArgs.SplitedIDs[i].ToString();
					string strSql = "UPDATE tblSimulation SET RCard='" + strNewRCard + "' WHERE RCard='" + oldRCard + "' AND RCardSeq=" + oldRCardSeq.ToString();
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					// ����SimulationReport
					strSql = "UPDATE tblSimulationReport SET RCard='" + strNewRCard + "' WHERE RCard='" + oldRCard + "' AND RCardSeq=" + oldRCardSeq.ToString();
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					// ����OnWIP
					strSql = "UPDATE tblOnWIP SET RCard='" + strNewRCard + "' WHERE RCard='" + oldRCard + "' AND RCardSeq=" + oldRCardSeq.ToString();
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					// ����OnWIPTrans
					strSql = "UPDATE tblOnWIPCardTrans SET RCard='" + strNewRCard + "' WHERE RCard='" + oldRCard + "' AND RCardSeq=" + oldRCardSeq.ToString();
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					// ����MOCard
					strSql = "UPDATE tblmorcard SET MORCardStart='" + strNewRCard + "',MORCardEnd='" + strNewRCard + "' WHERE MORCardStart='" + oldRCard + "' AND MOCode='" + s.MOCode + "' ";
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					// �����ø���
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
	
	}
}
