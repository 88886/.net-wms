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
    public class ActionConvertCard : IAction
    {
        private IDomainDataProvider _domainDataProvider = null;

        public ActionConvertCard(IDomainDataProvider domainDataProvider)
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

        /// ���к�ת���ɼ�,��֧�ְַ�
        public Messages Execute(ActionEventArgs actionEventArgs)
        {
            // Added by Icyer 2006/10/08
            if (((ConvertCardActionEventArgs)actionEventArgs).IsUndo == true)
            {
                return this.UndoExecute((ConvertCardActionEventArgs)actionEventArgs);
            }
            // Added end

            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                if (((ConvertCardActionEventArgs)actionEventArgs).ConvertToCard.Length <= 0)
                {
                    throw new Exception("$CS_System_Params_Losted");
                }

                ActionOnLineHelper helper = new ActionOnLineHelper(this.DataProvider);


                //��дSIMULATION ��鹤����ID��;�̡�����
                messages.AddMessages(helper.CheckID(actionEventArgs));

                if (messages.IsSuccess())
                {
                    actionEventArgs.ProductInfo.NowSimulation.IDMergeRule = 1; //actionEventArgs.ProductInfo.NowSimulation.IDMergeRule/((SplitIDActionEventArgs)actionEventArgs).SplitedIDs.Length;
                    actionEventArgs.ProductInfo.NowSimulation.TranslateCard = ((ConvertCardActionEventArgs)actionEventArgs).TransFromCard;
                    actionEventArgs.ProductInfo.NowSimulation.TranslateCardSequence = actionEventArgs.ProductInfo.LastSimulation.RunningCardSequence;
                    actionEventArgs.ProductInfo.NowSimulation.NGTimes = actionEventArgs.ProductInfo.LastSimulation.NGTimes;

                    DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);

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
                        //�깤�Զ����
                        dataCollectFacade.AutoInventory(actionEventArgs.ProductInfo.NowSimulation, actionEventArgs.UserCode);
                    }
                    //End Laws Lu
                    actionEventArgs.ProductInfo.NowSimulation.MOSeq = actionEventArgs.ProductInfo.LastSimulation.MOSeq;
                    // hiro.chen 2009.08.28     ��tblsimulation,tblonwip,tblsimulationreport��Rcardת������   
                    actionEventArgs.ProductInfo.LastSimulation.TranslateCard = (actionEventArgs as ConvertCardActionEventArgs).TransFromCard;
                    actionEventArgs.ProductInfo.NowSimulation.TranslateCard = (actionEventArgs as ConvertCardActionEventArgs).TransFromCard;
                    actionEventArgs.ProductInfo.LastSimulation.RunningCard = actionEventArgs.ProductInfo.NowSimulation.SourceCard;
                    actionEventArgs.ProductInfo.NowSimulation.RunningCard = actionEventArgs.ProductInfo.NowSimulation.SourceCard;

                    messages.AddMessages(helper.Execute(actionEventArgs));

                    if (messages.IsSuccess())
                    {
                        //��дIDת������
                        OnWIPCardTransfer transf = dataCollectFacade.CreateNewOnWIPCardTransfer();

                        transf.RunningCard = ((ConvertCardActionEventArgs)actionEventArgs).ConvertToCard.Trim().ToUpper();
                        transf.RunningCardSequence = actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence;
                        transf.IDMergeType = ((ConvertCardActionEventArgs)actionEventArgs).IDMergeType;
                        transf.TranslateCard = ((ConvertCardActionEventArgs)actionEventArgs).TransFromCard;
                        transf.TranslateCardSequence = actionEventArgs.ProductInfo.LastSimulation.RunningCardSequence;
                        transf.SourceCard = actionEventArgs.ProductInfo.NowSimulation.SourceCard;
                        transf.ModelCode = actionEventArgs.ProductInfo.NowSimulation.ModelCode;
                        transf.MOCode = actionEventArgs.ProductInfo.NowSimulation.MOCode;
                        transf.ItemCode = actionEventArgs.ProductInfo.NowSimulation.ItemCode;
                        transf.ResourceCode = actionEventArgs.ResourceCode;
                        transf.OPCode = actionEventArgs.ProductInfo.NowSimulation.OPCode;
                        transf.SourceCardSequence = actionEventArgs.ProductInfo.NowSimulation.SourceCardSequence;
                        transf.RouteCode = actionEventArgs.ProductInfo.NowSimulation.RouteCode;
                        transf.StepSequenceCode = actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode;
                        transf.SegmnetCode = actionEventArgs.ProductInfo.NowSimulationReport.SegmentCode;
                        transf.TimePeriodCode = actionEventArgs.ProductInfo.NowSimulationReport.TimePeriodCode;
                        transf.ShiftCode = actionEventArgs.ProductInfo.NowSimulationReport.ShiftCode;
                        transf.ShiftTypeCode = actionEventArgs.ProductInfo.NowSimulationReport.ShiftTypeCode;
                        transf.MaintainUser = actionEventArgs.UserCode;
                        transf.MOSeq = actionEventArgs.ProductInfo.NowSimulationReport.MOSeq;

                        dataCollectFacade.AddOnWIPCardTransfer(transf);
                        //AMOI  MARK  START  20050806 ���Ӱ���Դͳ�Ʋ������깤ͳ��
                        #region ��дͳ�Ʊ��� ����Դͳ��
                        // Added by Icyer 2006/06/05
                        //if (actionEventArgs.NeedUpdateReport == true)
                        //{
                        //    ReportHelper reportCollect = new ReportHelper(this.DataProvider);
                        //    messages.AddMessages(reportCollect.ReportLineQuanMaster(this.DataProvider,
                        //        actionEventArgs.ActionType, actionEventArgs.ProductInfo));
                        //    messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider
                        //        , actionEventArgs.ActionType, actionEventArgs.ProductInfo));
                        //}
                        #endregion
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

        public Messages UndoExecute(ConvertCardActionEventArgs actionEventArgs)
        {

            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                if (((ConvertCardActionEventArgs)actionEventArgs).ConvertToCard.Length < 0)
                {
                    throw new Exception("$CS_System_Params_Losted");
                }

                DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
                object[] oldSimulation = dataCollectFacade.GetSimulationFromTCard(actionEventArgs.RunningCard);
                if (oldSimulation == null || oldSimulation.Length == 0)
                {
                    throw new Exception("$CS_System_Params_Losted");
                }

                ActionOnLineHelper helper = new ActionOnLineHelper(this.DataProvider);
                MORunningCardFacade cardFacade = new MORunningCardFacade(this.DataProvider);

                // ����Simulation
                Simulation s = (Simulation)oldSimulation[0];
                string oldRCard = s.TranslateCard;
                decimal oldRCardSeq = s.RunningCardSequence;
                string strNewRCard = actionEventArgs.ConvertToCard.ToString();
                string strSql = "";

                // ����OnWIPTrans
                strSql = "UPDATE tblOnWIPCardTrans SET RCard='" + strNewRCard + "' WHERE TCard='" + oldRCard + "'";
                this.DataProvider.CustomExecute(new SQLCondition(strSql));
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
