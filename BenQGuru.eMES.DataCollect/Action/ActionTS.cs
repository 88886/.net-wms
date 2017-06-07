using System;

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.TS;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.TS;
using BenQGuru.eMES.Web.Helper;

using UserControl;

namespace BenQGuru.eMES.DataCollect.Action
{
    /// <summary>
    /// ���������ɼ�
    /// </summary>
    public class ActionTS : IActionWithStatus
    {
        private IDomainDataProvider _domainDataProvider = null;

        //		public ActionTS()
        //		{
        //			//
        //			// TODO: �ڴ˴���ӹ��캯���߼�
        //			//
        //		}

        public ActionTS(IDomainDataProvider domainDataProvider)
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

        #region Laws Lu,2005/10/28,ע��
        //
        ////		/// <summary>
        ////		/// ����TS�Ƿ���������д����SIMULATION
        ////		/// </summary>
        ////		/// <param name="iD"></param>
        ////		/// <param name="actionType"></param>
        ////		/// <param name="resourceCode"></param>
        ////		/// <param name="userCode"></param>
        ////		/// <param name="product"></param>
        ////		/// <returns></returns>
        //		public Messages CheckID(ActionEventArgs actionEventArgs)
        //		{
        //			Messages messages=new Messages();
        //			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"CheckID");
        //			dataCollectDebug.WhenFunctionIn(messages);
        //			try
        //			{
        //				MOFacade moFacade=new MOFacade(this.DataProvider);				 
        //				BaseModelFacade dataModel=new BaseModelFacade(this.DataProvider);
        //				DataCollectFacade dataCollectFacade=new DataCollectFacade(this.DataProvider);
        //				//IDataCollectModule dataCollect=new DataCollectOnLine();
        //				
        //				#region ��鹤��
        //				MO mo=(MO)moFacade.GetMO(actionEventArgs.ProductInfo.LastSimulation.MOCode);
        //				//����״̬���
        //				if (!dataCollectFacade.CheckMO(mo))
        //				{
        //					throw new Exception("$MOStatus_Error"+mo.MOStatus);
        //				}		
        //				#endregion
        //				#region ���ID״̬
        //				//����TS��ID״̬���� TODO
        //				throw new Exception("$CS_IDisNG");
        //				#endregion
        //				#region ���;��
        //				//Operation op=new Operation();
        //				ItemRoute2OP op=new ItemRoute2OP();
        //				//���ݻ���OP��� TODO
        //				//op=(ItemRoute2OP)moFacade.GetOperation("");
        ////				if (dataModel.GetOperation2Resource(op.OPCode,resourceCode)==null)
        ////				{
        ////					throw new Exception("$CS_Route_Failed $CS_Param_OPCode"+op.OPCode);						
        ////				}
        //				#endregion
        //				#region ���ACTION
        //				if (dataCollectFacade.CheckAction(actionEventArgs.ProductInfo,op,actionEventArgs.ActionType))
        //				{
        //				}
        //				#endregion
        //				#region ��д��SIMULATION
        //				messages.AddMessages( dataCollectFacade.WriteSimulation(actionEventArgs.RunningCard, actionEventArgs.ActionType, actionEventArgs.ResourceCode, actionEventArgs.UserCode, actionEventArgs.ProductInfo));
        //				if (messages.IsSuccess())
        //				{
        //					//�޸�
        //					actionEventArgs.ProductInfo.NowSimulation.RouteCode =	actionEventArgs.ProductInfo.LastSimulation.RouteCode;
        //					actionEventArgs.ProductInfo.NowSimulation.OPCode =		op.OPCode;				
        //					actionEventArgs.ProductInfo.NowSimulation.ResourceCode=	actionEventArgs.ResourceCode;	
        //					
        //					actionEventArgs.ProductInfo.NowSimulation.ActionList =";"+ actionEventArgs.ActionType +";";				
        //				}
        //				#endregion
        //			}
        //			catch (Exception e)
        //			{
        //				messages.Add(new Message(e));
        //			}
        //			dataCollectDebug.WhenFunctionOut(messages);
        //			return messages;
        //		}
        //		public Messages CheckIDIn(string iD,string actionType,string resourceCode,string userCode, ProductInfo product,object[] datas1,object[] datas2)
        //		{
        //			Messages messages=new Messages();
        //			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"GetIDInfo");
        //			dataCollectDebug.WhenFunctionIn(messages);
        //			try
        //			{
        //				DataCollectFacade dataCollect=new DataCollectFacade(this.DataProvider);
        //				messages.AddMessages( dataCollect.CheckID(iD,actionType,resourceCode,userCode, product));
        //			
        //			}
        //			catch (Exception e)
        //			{
        //				messages.Add(new Message(e));
        //			}
        //			dataCollectDebug.WhenFunctionOut(messages);
        //			return messages;
        //		}
        #endregion

        /// <summary>
        /// ����Ʒ�ɼ�
        /// </summary>
        /// <param name="domainDataProvider"></param>
        /// <param name="iD"></param>
        /// <param name="actionType"></param>
        /// <param name="resourceCode"></param>
        /// <param name="userCode"></param>
        /// <param name="product"></param>
        /// <param name="datas1"></param>
        /// <param name="datas2"></param>
        /// <returns></returns>
        public Messages Execute(ActionEventArgs actionEventArgs)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            Messages msgAutoGoMO = new Messages();		// Added by Icyer 2007/03/09
            try
            {
                // Added by Icyer 2007/03/09
                // ����Զ���������
                ActionGoToMO actionGoToMO = new ActionGoToMO(this.DataProvider);
                msgAutoGoMO = actionGoToMO.AutoGoMO(actionEventArgs);
                // Added end

                //���ErrorCode,ErrorGroup�Ƿ���ȷ TODO
                //���CS��ȷ��ErrorCode,ErrorGroup����ȷ�ģ��˴��߼�����ȥ��


                // Added by Icyer 2006/12/03
                // �Զ���Undo
                messages.AddMessages((new ActionUndoNG(this.DataProvider)).UndoNG(actionEventArgs));
                // Added end
                ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
                //��дSIMULATION ��鹤����ID��;�̡�����
                messages.AddMessages(dataCollect.CheckID(actionEventArgs));
                if (messages.IsSuccess())
                {
                    //����SIMULATION ������Ϣ
                    actionEventArgs.ProductInfo.NowSimulation.ProductStatus = ProductStatus.NG;
                    actionEventArgs.ProductInfo.NowSimulation.NGTimes = actionEventArgs.ProductInfo.NowSimulation.NGTimes + 1;
                    if (actionEventArgs.CurrentMO != null)
                    {
                        actionEventArgs.ProductInfo.NowSimulation.RMABillCode = actionEventArgs.CurrentMO.RMABillCode;
                    }
                    else
                    {
                        MO mo = (new MOFacade(DataProvider)).GetMO(actionEventArgs.ProductInfo.NowSimulation.MOCode) as MO;

                        actionEventArgs.CurrentMO = mo;
                        actionEventArgs.ProductInfo.NowSimulation.RMABillCode = mo.RMABillCode;

                    }

                    messages.AddMessages(dataCollect.Execute(actionEventArgs));
                    if (messages.IsSuccess())
                    {

                        //Laws Lu,2005/12/19,����	��ȡErrorGroup2ErrorCode
                        if (actionEventArgs.ActionType == ActionType.DataCollectAction_NG)
                        {
                            actionEventArgs.ProductInfo.ECG2ErrCodes = ParseECG2Errs(((TSActionEventArgs)actionEventArgs).ErrorCodes, actionEventArgs.ActionType);
                        }
                        else
                        {
                            actionEventArgs.ProductInfo.ECG2ErrCodes = ParseECG2Errs(((TSActionEventArgs)actionEventArgs).ErrorInfor, actionEventArgs.ActionType);
                        }

                        //��д���Ա��� TODO
                        if (actionEventArgs.ActionType == ActionType.DataCollectAction_SMTNG)
                            messages.AddMessages(dataCollect.CollectErrorInfor(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorInfor, ((TSActionEventArgs)actionEventArgs).Memo));
                        else
                            messages.AddMessages(dataCollect.CollectErrorInformation(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorCodes, null, ((TSActionEventArgs)actionEventArgs).Memo));

                        //ReportHelper reportCollect = new ReportHelper(this.DataProvider);
                        //messages.AddMessages(reportCollect.ReportLineQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo));
                        //messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo));
                        //if (actionEventArgs.ActionType == ActionType.DataCollectAction_NG)
                        //    messages.AddMessages(reportCollect.ReportResECMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorCodes));
                        //else
                        //    messages.AddMessages(reportCollect.ReportResECMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorInfor));

                        //if (actionEventArgs.ActionType == ActionType.DataCollectAction_NG)
                        //    messages.AddMessages(reportCollect.ReportLineECOQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorCodes));
                        //else
                        //    messages.AddMessages(reportCollect.ReportLineECOQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorInfor));

                        // Added By Hi1/Venus.Feng on 20080711 for Hisense Version : Auto Set RCard Reflow Route and OP by ErrorCode
                        if (messages.IsSuccess() == true)
                        {
                            messages.AddMessages(this.SetRCardReflowByErrorCode(actionEventArgs));
                        }
                    }

                    if (messages.IsSuccess())
                    {
                        DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
                        dataCollectFacade.TryToDeleteRCardFromLot(actionEventArgs.RunningCard);
                    }
                }
            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            //return messages;
            if (msgAutoGoMO.Count() < 1 || (msgAutoGoMO.IsSuccess() == true && messages.IsSuccess() == false))
                return messages;
            else
            {
                msgAutoGoMO.IgnoreError();
                msgAutoGoMO.AddMessages(messages);
                return msgAutoGoMO;
            }
        }
        //Laws Lu,2006/01/10,��ȡ���������顢��������
        public static string ParseECG2Errs(object[] errorInfo, string actType)
        {
            string strReturn = String.Empty;
            if (errorInfo == null)
            {
                return strReturn;
            }
            for (int i = 0; i < errorInfo.Length; i++)
            {
                if (actType == ActionType.DataCollectAction_SMTNG)
                {
                    TSErrorCode2Location tsLoc = ((TSErrorCode2Location)errorInfo[i]);
                    //Laws Lu,2006/06/06
                    string eg2er = tsLoc.ErrorCodeGroup + ":" + tsLoc.ErrorCode + ";";
                    if (strReturn.IndexOf(eg2er) < 0)
                    {
                        strReturn += tsLoc.ErrorCodeGroup + ":" + tsLoc.ErrorCode + ";";
                    }
                }
                if (actType == ActionType.DataCollectAction_NG || actType == ActionType.DataCollectAction_OutLineNG)
                {
                    ErrorCodeGroup2ErrorCode tsLoc = ((ErrorCodeGroup2ErrorCode)errorInfo[i]);
                    strReturn += tsLoc.ErrorCodeGroup + ":" + tsLoc.ErrorCode + ";";
                }
            }

            return strReturn;
        }
        /// <summary>
        /// ����Ʒ�ɼ�
        /// </summary>
        /// <param name="domainDataProvider"></param>
        /// <param name="iD"></param>
        /// <param name="actionType"></param>
        /// <param name="resourceCode"></param>
        /// <param name="userCode"></param>
        /// <param name="product"></param>
        /// <param name="datas1"></param>
        /// <param name="datas2"></param>
        /// <returns></returns>
        public Messages Execute(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            Messages msgAutoGoMO = new Messages();		// Added by Icyer 2007/03/09
            try
            {
                // Added by Icyer 2007/03/09
                // ����Զ���������
                ActionGoToMO actionGoToMO = new ActionGoToMO(this.DataProvider);
                msgAutoGoMO = actionGoToMO.AutoGoMO(actionEventArgs);
                // Added end

                //���ErrorCode,ErrorGroup�Ƿ���ȷ TODO
                //���CS��ȷ��ErrorCode,ErrorGroup����ȷ�ģ��˴��߼�����ȥ��


                // Added by Icyer 2006/12/03
                // �Զ���Undo
                messages.AddMessages((new ActionUndoNG(this.DataProvider)).UndoNG(actionEventArgs));
                // Added end
                ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
                //��дSIMULATION ��鹤����ID��;�̡�����
                messages.AddMessages(dataCollect.CheckID(actionEventArgs, actionCheckStatus));
                if (messages.IsSuccess())
                {
                    //����SIMULATION ������Ϣ
                    actionEventArgs.ProductInfo.NowSimulation.ProductStatus = ProductStatus.NG;
                    actionEventArgs.ProductInfo.NowSimulation.NGTimes = actionEventArgs.ProductInfo.NowSimulation.NGTimes + 1;
                    if (actionEventArgs.CurrentMO != null)
                    {
                        actionEventArgs.ProductInfo.NowSimulation.RMABillCode = actionEventArgs.CurrentMO.RMABillCode;
                    }
                    else
                    {
                        MO mo = (new MOFacade(DataProvider)).GetMO(actionEventArgs.ProductInfo.NowSimulation.MOCode) as MO;

                        actionEventArgs.CurrentMO = mo;
                        actionEventArgs.ProductInfo.NowSimulation.RMABillCode = mo.RMABillCode;

                    }

                    if (actionCheckStatus.NeedUpdateSimulation)
                    {
                        messages.AddMessages(dataCollect.Execute(actionEventArgs));
                    }
                    else
                    {
                        messages.AddMessages(dataCollect.Execute(actionEventArgs, actionCheckStatus));
                    }


                    if (messages.IsSuccess())
                    {
                        if (actionCheckStatus.NeedFillReport == true)
                        {
                            //Laws Lu,2005/12/19,����	��ȡErrorGroup2ErrorCode
                            if (actionEventArgs.ActionType == ActionType.DataCollectAction_NG)
                            {
                                actionEventArgs.ProductInfo.ECG2ErrCodes = ParseECG2Errs(((TSActionEventArgs)actionEventArgs).ErrorCodes, actionEventArgs.ActionType);
                            }
                            else
                            {
                                actionEventArgs.ProductInfo.ECG2ErrCodes = ParseECG2Errs(((TSActionEventArgs)actionEventArgs).ErrorInfor, actionEventArgs.ActionType);
                            }

                            //ReportHelper reportCollect = new ReportHelper(this.DataProvider);
                            //messages.AddMessages(reportCollect.ReportLineQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo));
                            //messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo));
                            //if (actionEventArgs.ActionType == ActionType.DataCollectAction_NG)
                            //    messages.AddMessages(reportCollect.ReportResECMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorCodes));
                            //else
                            //    messages.AddMessages(reportCollect.ReportResECMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorInfor));

                            ////							if (actionEventArgs.ActionType==ActionType.DataCollectAction_NG)
                            ////								messages.AddMessages(reportCollect.ReportResECMaster(this.DataProvider,actionEventArgs.ActionType,actionEventArgs.ProductInfo,((TSActionEventArgs)actionEventArgs).ErrorCodes));
                            ////							else
                            ////								messages.AddMessages(reportCollect.ReportResECMaster(this.DataProvider,actionEventArgs.ActionType,actionEventArgs.ProductInfo,((TSActionEventArgs)actionEventArgs).ErrorInfor));
                            //if (actionEventArgs.ActionType == ActionType.DataCollectAction_NG)
                            //    messages.AddMessages(reportCollect.ReportLineECOQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorCodes));
                            //else
                            //    messages.AddMessages(reportCollect.ReportLineECOQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorInfor));

                        }


                        //��д���Ա��� TODO
                        if (actionEventArgs.ActionType == ActionType.DataCollectAction_SMTNG)
                            messages.AddMessages(dataCollect.CollectErrorInfor(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorInfor, ((TSActionEventArgs)actionEventArgs).Memo));
                        else
                            messages.AddMessages(dataCollect.CollectErrorInformation(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, ((TSActionEventArgs)actionEventArgs).ErrorCodes, null, ((TSActionEventArgs)actionEventArgs).Memo));

                        // Added By Hi1/Venus.Feng on 20080711 for Hisense Version : Auto Set RCard Reflow Route and OP by ErrorCode
                        if (messages.IsSuccess() == true)
                        {
                            messages.AddMessages(this.SetRCardReflowByErrorCode(actionEventArgs));
                        }
                    }

                    if (messages.IsSuccess())
                    {
                        DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
                        dataCollectFacade.TryToDeleteRCardFromLot(actionEventArgs.RunningCard);
                    }
                }
            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            //return messages;
            if (msgAutoGoMO.Count() < 1 || (msgAutoGoMO.IsSuccess() == true && messages.IsSuccess() == false))
                return messages;
            else
            {
                msgAutoGoMO.IgnoreError();
                msgAutoGoMO.AddMessages(messages);
                return msgAutoGoMO;
            }
        }

        private Messages SetRCardReflowByErrorCode(ActionEventArgs actionEventArgs)
        {
            Messages messages = new Messages();
            string routeCode = "";
            string opCode = "";

            System.Collections.Generic.List<string> listErrorCode = new System.Collections.Generic.List<string>();

            if (((TSActionEventArgs)actionEventArgs).ErrorInfor != null)
            {
                TSErrorCode2Location[] error = new TSErrorCode2Location[((TSActionEventArgs)actionEventArgs).ErrorInfor.Length];
                ((TSActionEventArgs)actionEventArgs).ErrorInfor.CopyTo(error, 0);
                for (int i = 0; i < error.Length; i++)
                {
                    if (error[i].ErrorCode != "" && listErrorCode.Contains(error[i].ErrorCode) == false)
                    {
                        listErrorCode.Add(error[i].ErrorCode);
                    }
                }
            }
            else if (((TSActionEventArgs)actionEventArgs).ErrorCodes != null)
            {
                ErrorCodeGroup2ErrorCode[] error = new ErrorCodeGroup2ErrorCode[((TSActionEventArgs)actionEventArgs).ErrorCodes.Length];
                ((TSActionEventArgs)actionEventArgs).ErrorCodes.CopyTo(error, 0);
                for (int i = 0; i < error.Length; i++)
                {
                    if (error[i].ErrorCode != "" && listErrorCode.Contains(error[i].ErrorCode) == false)
                    {
                        listErrorCode.Add(error[i].ErrorCode);
                    }
                }
            }
            if (listErrorCode.Count == 0)
                return messages;

            // ��ѯ��������Ͳ�Ʒ��Ӧ�ķ���;��
            TSModel.TSModelFacade tsmodelFacade = new BenQGuru.eMES.TSModel.TSModelFacade(this.DataProvider);
            object objErrorCode2OPRework = tsmodelFacade.GetErrorCode2OPRework(actionEventArgs.ProductInfo.NowSimulation.OPCode,
                listErrorCode[0], GlobalVariables.CurrentOrganizations.First().OrganizationID);

            object tempOPRework;
            ErrorCode2OPRework tempErrorCode2OPRework;
            if (objErrorCode2OPRework == null)
            {
                for (int i = 1; i < listErrorCode.Count; i++)
                {
                    tempOPRework = tsmodelFacade.GetErrorCode2OPRework(actionEventArgs.ProductInfo.NowSimulation.OPCode,
                           listErrorCode[i], GlobalVariables.CurrentOrganizations.First().OrganizationID);
                    if (tempOPRework != null)
                    {
                        messages.Add(new Message(MessageType.Error, "$Error_ErrorCodeHaveMoreThanOneRoute"));
                        return messages;
                    }
                }

                return messages;
            }
            else
            {
                ErrorCode2OPRework firstErrorCode2OPRework = objErrorCode2OPRework as ErrorCode2OPRework;
                for (int i = 1; i < listErrorCode.Count; i++)
                {
                    tempOPRework = tsmodelFacade.GetErrorCode2OPRework(actionEventArgs.ProductInfo.NowSimulation.OPCode,
                           listErrorCode[i], GlobalVariables.CurrentOrganizations.First().OrganizationID);

                    if (tempOPRework == null)
                    {
                        messages.Add(new Message(MessageType.Error, "$Error_ErrorCodeHaveMoreThanOneRoute"));
                        return messages;
                    }
                    else
                    {
                        tempErrorCode2OPRework = tempOPRework as ErrorCode2OPRework;
                        if (tempErrorCode2OPRework.RouteCode != firstErrorCode2OPRework.RouteCode
                            || tempErrorCode2OPRework.ToOPCode != firstErrorCode2OPRework.ToOPCode)
                        {
                            messages.Add(new Message(MessageType.Error, "$Error_ErrorCodeHaveMoreThanOneRoute"));
                            return messages;
                        }
                    }
                }

                opCode = firstErrorCode2OPRework.ToOPCode;
                routeCode = firstErrorCode2OPRework.RouteCode;
                if (string.Compare(opCode, "TS", true) == 0)
                {
                    return messages;
                }
            }

            if (routeCode.Trim().Length == 0)  // ���;�̵��ڿգ��򽫵�ǰ���кŵ�;����Ϊ����;�� 
            {
                routeCode = actionEventArgs.ProductInfo.NowSimulation.RouteCode;
            }

            // ����Ʒ��;�̵Ķ�Ӧ
            ItemFacade itemFacade = new ItemFacade(this.DataProvider);
            ItemRoute2OP op = (ItemRoute2OP)itemFacade.GetItemRoute2Operation(actionEventArgs.ProductInfo.NowSimulation.ItemCode, routeCode, opCode);
            if (op == null)
            {
                messages.Add(new Message(MessageType.Error, "$Error_ReworkRouteNotBelongToItem [" + routeCode + "]"));
                return messages;
            }

            // ��TS��״̬�ĳ�ά����
            TSFacade tsFacade = new TSFacade(this.DataProvider);
            object obj = tsFacade.GetCardLastTSRecord(actionEventArgs.RunningCard);
            if (obj != null)
            {
                Domain.TS.TS ts = (Domain.TS.TS)obj;
                ts.ConfirmResourceCode = actionEventArgs.ResourceCode;
                ts.ConfirmUser = actionEventArgs.UserCode;
                ts.ConfirmDate = actionEventArgs.ProductInfo.NowSimulation.MaintainDate;
                ts.ConfirmTime = actionEventArgs.ProductInfo.NowSimulation.MaintainTime;
                ts.TSStatus = TSStatus.TSStatus_TS;
                tsFacade.UpdateTS(ts);
            }
            else
                return messages;

            // ��ʼ���û���
            ActionFactory actionFactory = new ActionFactory(this.DataProvider);
            IAction actionTSComplete = actionFactory.CreateAction(ActionType.DataCollectAction_TSComplete);
            TSActionEventArgs tsactionEventArgs = new TSActionEventArgs(
                ActionType.DataCollectAction_TSComplete,
                actionEventArgs.RunningCard,
                actionEventArgs.UserCode,
                actionEventArgs.ResourceCode,
                TSStatus.TSStatus_Reflow,
                actionEventArgs.ProductInfo.NowSimulation.MOCode,
                actionEventArgs.ProductInfo.NowSimulation.ItemCode,
                routeCode,
                op.OPCode,
                actionEventArgs.UserCode,
                null);
            tsactionEventArgs.RouteCode = routeCode;
            tsactionEventArgs.IgnoreResourceInOPTS = true;

            messages.AddMessages(actionTSComplete.Execute(tsactionEventArgs));

            return messages;
        }
    }
}
