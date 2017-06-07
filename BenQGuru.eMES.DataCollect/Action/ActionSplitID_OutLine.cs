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
	public class ActionSplitOutLine:IAction
	{
		
		private IDomainDataProvider _domainDataProvider = null;

//		public ActionSplit()
//		{	
//		}

        public ActionSplitOutLine(IDomainDataProvider domainDataProvider)
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
        public Messages Execute(ActionEventArgs actionEventArgs)
        {
            // Added by Icyer 2006/10/08
            if (((SplitIDActionEventArgs)actionEventArgs).IsUndo == true)
            {
                return this.UndoExecute((SplitIDActionEventArgs)actionEventArgs);
            }
            // Added end

            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                if (((SplitIDActionEventArgs)actionEventArgs).SplitedIDs == null || ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs.Length == 0)
                {
                    throw new Exception("$CS_System_Params_Losted");
                }

                ActionOnLineHelper helper = new ActionOnLineHelper(this.DataProvider);
                DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);

                //laura:  GetLastSimulation into NowSimulation                    
                string sourcCard = actionEventArgs.ProductInfo.LastSimulation.RunningCard.ToUpper();

                //GetLastSimulation �ǻ�ȡ��ǰopen�����е�һ�ʣ�GetSimulation �ǻ�ȡ����simulation��mdate �����һ�ʡ�                           
                Simulation objSimulation = dataCollectFacade.GetSimulation(sourcCard) as Simulation;
                if (objSimulation == null)
                {
                    throw new Exception("rcard in simulation not existed!");
                }
                actionEventArgs.ProductInfo.NowSimulation = objSimulation;

                //laura:  GetLastSimulationReport into NowSimulationReport                                                        
                SimulationReport objSimulationReport = dataCollectFacade.GetLastSimulationReport(sourcCard) as SimulationReport;
                if (objSimulationReport == null)
                {
                    throw new Exception("rcard in simulationreport not existed!");
                }
                actionEventArgs.ProductInfo.NowSimulationReport = objSimulationReport;

                //laura:  �����Ҫ������ ProductInfo ��Ϣ
                //to-do... 

                //
                actionEventArgs.ProductInfo.NowSimulation.IDMergeRule = 1; //actionEventArgs.ProductInfo.NowSimulation.IDMergeRule/((SplitIDActionEventArgs)actionEventArgs).SplitedIDs.Length;
                actionEventArgs.ProductInfo.NowSimulation.TranslateCard = actionEventArgs.ProductInfo.LastSimulation.RunningCard;
                actionEventArgs.ProductInfo.NowSimulation.TranslateCardSequence = actionEventArgs.ProductInfo.LastSimulation.RunningCardSequence;
                actionEventArgs.ProductInfo.NowSimulation.NGTimes = actionEventArgs.ProductInfo.LastSimulation.NGTimes;

                for (int i = 0; i < ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs.Length; i++)
                {
                    //�޸�SIMULATION
                    //Laws Lu,2005/08/15,����	�깤�߼���������Check��ͨ��������£����е�RunningCardӦ����GOOD״̬
                    //��ʱ���������⹤��
                    if (actionEventArgs.ProductInfo.NowSimulation.RouteCode != "" && dataCollectFacade.OPIsMORouteLastOP(
                        actionEventArgs.ProductInfo.NowSimulation.MOCode
                        , actionEventArgs.ProductInfo.NowSimulation.RouteCode
                        , actionEventArgs.ProductInfo.NowSimulation.OPCode))
                    {
                        actionEventArgs.ProductInfo.NowSimulation.IsComplete = "1";
                        actionEventArgs.ProductInfo.NowSimulation.EAttribute1 = "GOOD";
                    }
                    //End Laws Lu
                    actionEventArgs.ProductInfo.NowSimulation.RunningCard = ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs[i].ToString();
                    /* added by jessie lee,����� ���к�ת�� */
                    if (string.Compare((actionEventArgs as SplitIDActionEventArgs).IDMergeType, IDMergeType.IDMERGETYPE_IDMERGE, true) == 0)
                    {
                        /* ת����ͬһ�Ź��� */
                        if ((actionEventArgs as SplitIDActionEventArgs).UpdateSimulation)
                        {
                            actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence
                                = (actionEventArgs as SplitIDActionEventArgs).ExistIMEISeq + 10;
                        }
                        else
                        {
                            actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence = ActionOnLineHelper.StartSeq;
                        }
                    }
                    else
                    {
                        /* ���� ���к�ת�� */
                        actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence = ActionOnLineHelper.StartSeq;
                    }
                    actionEventArgs.ProductInfo.NowSimulation.MOSeq = actionEventArgs.ProductInfo.LastSimulation.MOSeq;     // Added by Icyer 2007/07/03

                    //messages.AddMessages(helper.Execute(actionEventArgs));
                    //messages.AddMessages(helper.Execute(actionEventArgs, true,false));  //���⹤�򣬲��� insert tblonwip��

                    if (messages.IsSuccess())
                    {
                        //#region ��ID��ӵ�MO2RCARDLINK��Χ����
                        DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                        //MOFacade _MOFacade = new MOFacade(this.DataProvider);
                        //MO2RCARDLINK mo2cardlink = new MO2RCARDLINK();
                        //mo2cardlink.MOCode = actionEventArgs.ProductInfo.NowSimulation.MOCode;
                        //mo2cardlink.RCard = ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs[i].ToString().Trim().ToUpper();
                        //mo2cardlink.MDate = dbDateTime.DBDate;
                        //mo2cardlink.MUser = actionEventArgs.UserCode;
                        //mo2cardlink.MTime = dbDateTime.DBTime;
                        //mo2cardlink.PrintTimes = 0;
                        //mo2cardlink.LastPrintUSER = "";
                        //mo2cardlink.LastPrintDate = 0;
                        //mo2cardlink.LastPrintTime = 0;
                        //_MOFacade.AddMO2RCardLink(mo2cardlink);
                        //#endregion

                        #region ��ID��ӵ�SplitBoard
                        SplitBoard splitBorad = new SplitBoard();
                        splitBorad.Seq = actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence;
                        splitBorad.Mocode = actionEventArgs.ProductInfo.NowSimulation.MOCode;
                        splitBorad.Rcard = ((SplitIDActionEventArgs)actionEventArgs).SplitedIDs[i].ToString().Trim().ToUpper();
                        splitBorad.Modelcode = actionEventArgs.ProductInfo.NowSimulation.ModelCode;
                        splitBorad.Itemcode = actionEventArgs.ProductInfo.NowSimulation.ItemCode;
                        splitBorad.Opcode = actionEventArgs.ProductInfo.NowSimulation.OPCode;
                        splitBorad.Rescode = actionEventArgs.ResourceCode;
                        splitBorad.Routecode = actionEventArgs.ProductInfo.NowSimulation.RouteCode;
                        splitBorad.Scard = actionEventArgs.ProductInfo.NowSimulation.SourceCard;
                        splitBorad.Segcode = actionEventArgs.ProductInfo.NowSimulationReport.SegmentCode;
                        splitBorad.Shiftcode = actionEventArgs.ProductInfo.NowSimulationReport.ShiftCode;
                        splitBorad.Shifttypecode = actionEventArgs.ProductInfo.NowSimulationReport.ShiftTypeCode;
                        splitBorad.Sscode = actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode;
                        splitBorad.Tpcode = actionEventArgs.ProductInfo.NowSimulationReport.TimePeriodCode;
                        splitBorad.Muser = actionEventArgs.UserCode;
                        splitBorad.Mdate = dbDateTime.DBDate;
                        splitBorad.Mtime = dbDateTime.DBTime;
                        dataCollectFacade.AddSplitBoard(splitBorad);
                        #endregion


                    }
                }
                //}
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
