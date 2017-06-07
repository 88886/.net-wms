using System;

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.DataCollect.Action;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.OQC;
using BenQGuru.eMES.Domain.Package;
using BenQGuru.eMES.Domain.Rework;
using BenQGuru.eMES.Domain.TS;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.OQC;
using BenQGuru.eMES.Rework;
using BenQGuru.eMES.TS;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.SMT;

using UserControl;

namespace BenQGuru.eMES.DataCollect
{
    /// <summary>
    /// DataCollectOnLine ��ժҪ˵����
    /// </summary>
    public class ActionOnLineHelper
    {
        private IDomainDataProvider _domainDataProvider = null;

        public ActionOnLineHelper(IDomainDataProvider domainDataProvider)
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


        public const int StartSeq = 1;
        public const string StringNull = "";
        public const int StartNGTimes = 0;

        //�õ�ID��Simulation ��Ϣ
        public Messages GetIDInfo(string iD)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "GetIDInfo");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                DataCollectFacade dataCollect = new DataCollectFacade(this.DataProvider);
                ProductInfo product = new ProductInfo();
                object obj = dataCollect.GetSimulation(iD.ToUpper());
                if (obj == null)
                {
                    product.LastSimulation = null;

                    //					//Karron Qiu ,2005-11-2,û��������Ϣ��ʱ��,���һ����Ϣ
                    //					messages.Add(new Message(MessageType.Error,"$NoSimulationInfo"));
                }
                else
                {
                    product.LastSimulation = new ExtendSimulation((Simulation)obj);

                    //Laws Lu,2005/12/15	��Ʒͣ���̵�
                    if (Convert.ToInt32(product.LastSimulation.IsHold) == (int)Web.Helper.CycleStatus.Hold)
                    {
                        messages.Add(new Message(MessageType.Error, "$CS_LINE_IS_HOLD"));
                    }
                }
                product.NowSimulation = new Simulation();

                Message message = new Message(MessageType.Data, "", new object[] { product });
                messages.Add(message);

            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            return messages;
        }

        //�õ�ID��Simulation ��Ϣ
        //bighai.wang 2011/02/23 add
        public Messages GetIDInfoSplit(string iD)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "GetIDInfo");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                DataCollectFacade dataCollect = new DataCollectFacade(this.DataProvider);
                ProductInfo product = new ProductInfo();

                //bighai.wang 2011/02/23 
                //����ת��ǰ���кż�����:���һ����Ʒ���кŹ����������,������һ���������깤,�����ٷְ�
                object obj = dataCollect.GetSimulation(iD.ToUpper());

                if (obj != null)
                {
                    //object objRcard = dataCollect.GetSimulationMarge(iD.ToUpper());

                    object[] objTcard = dataCollect.QuerySimulationT(iD.ToUpper());

                    if (objTcard != null)
                    {
                        if (objTcard.Length > 1 && ((Simulation)obj).IsComplete == "1")
                        {
                            messages.Add(new Message(MessageType.Error, "�Ѿ��ֹ���,�����ٷ�"));
                        }
                    }
                }
                //end bighai.wang 2011/02/23

                if (obj == null)
                {
                    product.LastSimulation = null;

                    //					//Karron Qiu ,2005-11-2,û��������Ϣ��ʱ��,���һ����Ϣ
                    //					messages.Add(new Message(MessageType.Error,"$NoSimulationInfo"));
                }
                else
                {
                    product.LastSimulation = new ExtendSimulation((Simulation)obj);

                    //Laws Lu,2005/12/15	��Ʒͣ���̵�
                    if (Convert.ToInt32(product.LastSimulation.IsHold) == (int)Web.Helper.CycleStatus.Hold)
                    {
                        messages.Add(new Message(MessageType.Error, "$CS_LINE_IS_HOLD"));
                    }
                }
                product.NowSimulation = new Simulation();

                Message message = new Message(MessageType.Data, "", new object[] { product });
                messages.Add(message);

            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            return messages;
        }

        public Messages GetIDInfoByMoCodeAndId(string moCode, string iD)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "GetIDInfo");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                DataCollectFacade dataCollect = new DataCollectFacade(this.DataProvider);
                ProductInfo product = new ProductInfo();
                object obj = dataCollect.GetSimulation(moCode, iD.ToUpper());
                if (obj == null)
                {
                    product.LastSimulation = null;

                    //					//Karron Qiu ,2005-11-2,û��������Ϣ��ʱ��,���һ����Ϣ
                    //					messages.Add(new Message(MessageType.Error,"$NoSimulationInfo"));
                }
                else
                {
                    product.LastSimulation = new ExtendSimulation((Simulation)obj);

                    //Laws Lu,2005/12/15	��Ʒͣ���̵�
                    if (Convert.ToInt32(product.LastSimulation.IsHold) == (int)Web.Helper.CycleStatus.Hold)
                    {
                        messages.Add(new Message(MessageType.Error, "$CS_LINE_IS_HOLD"));
                    }
                }
                product.NowSimulation = new Simulation();

                Message message = new Message(MessageType.Data, "", new object[] { product });
                messages.Add(message);

            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            return messages;
        }

        public ProductInfo GetIDInfoBySimulation(Simulation simulation)
        {
            ProductInfo product = new ProductInfo();
            product.LastSimulation = new ExtendSimulation(simulation);
            product.NowSimulation = new Simulation();
            //Laws Lu,2005/12/15	��Ʒͣ���̵�
            if (Convert.ToInt32(product.LastSimulation.IsHold) == (int)Web.Helper.CycleStatus.Hold)
            {
                throw new Exception("$CS_LINE_IS_HOLD");
            }

            return product;

        }

        /// <summary>
        /// �Ƚ�����SIMULATION����;�����Ƿ�����һ��
        /// </summary>
        /// <param name="simulation1"></param>
        /// <param name="simulation2"></param>
        /// <returns></returns>
        public bool CompareSimulationCheck(Simulation simulation1, Simulation simulation2)
        {
            if (simulation1 == null)
                return false;
            if (simulation2 == null)
                return false;
            if ((simulation1.MOCode == simulation2.MOCode)
                && (simulation1.RouteCode == simulation2.RouteCode)
                && (simulation1.OPCode == simulation2.OPCode)
                && (simulation1.ResourceCode == simulation2.ResourceCode)
                && (simulation1.ProductStatus == simulation2.ProductStatus)
                && (simulation1.ActionList == simulation2.ActionList)
                && (simulation1.LastAction == simulation2.LastAction)
                && (simulation1.IsComplete == simulation2.IsComplete)
                )
                return true;
            return false;
        }
        public void CopyProduct(ProductInfo sourceProduct, ProductInfo toProduct)
        {
            ((ExtendSimulation)toProduct.LastSimulation).AdjustProductStatus = ((ExtendSimulation)sourceProduct.LastSimulation).AdjustProductStatus;
            ((ExtendSimulation)toProduct.LastSimulation).NextOPCode = ((ExtendSimulation)sourceProduct.LastSimulation).NextOPCode;
            ((ExtendSimulation)toProduct.LastSimulation).NextRouteCode = ((ExtendSimulation)sourceProduct.LastSimulation).NextRouteCode;
        }
        //Check Route �Ļ�����Ϣ  Check ��ȷ����NewSimulation �������µ�;�� 
        public Messages CheckID(ActionEventArgs actionEventArgs)
        {
            return CheckID(actionEventArgs, null);
        }
        // Added by Icyer 2005/10/28
        // ��չһ����ActionCheckStatus�ķ���
        public Messages CheckID(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "CheckID");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                if (actionEventArgs.ActionType != ActionType.DataCollectAction_GoMO)
                {
                    if (actionEventArgs.ProductInfo.LastSimulation == null)
                    {
                        throw new Exception("$NoSimulationInfo");
                    }
                    else
                    {
                        DataCollectFacade dcFacade = new DataCollectFacade(this.DataProvider);
                        //added by hiro.chen 08/11/18 :�ж��Ƿ����µ�
                        messages.AddMessages(dcFacade.CheckISDown(actionEventArgs.RunningCard));
                        if (!messages.IsSuccess())
                        {
                            return messages;
                        }
                        //end 

                        //����Ͷ��֮ǰ���ж�
                        if (CheckBeforeRework(actionEventArgs))
                        {
                            // Added By Hi1/Venus.Feng on 20080711 for Hisense Version : Add Adjust Rework Route by Resource
                            messages.AddMessages(this.AddReworkRangeByResource(actionEventArgs));
                            if (!messages.IsSuccess())
                            {
                                return messages;
                            }
                            // End Added

                            // Added By Hi1/Venus.Feng on 20080711 for Hisense Version : Add Adjust Rework Route by Resource
                            messages.AddMessages(this.AdjustReworkRouteByResource(actionEventArgs));
                            if (!messages.IsSuccess())
                            {
                                return messages;
                            }
                            // End Added
                        }

                        //						if(actionEventArgs.ProductInfo.LastSimulation.IsComplete == 1)
                        //						{
                        //							messages.AddMessages(new UserControl.Message(MessageType.Error,"$CS_ERROR_PRODUCT_ALREADY_COMPLETE"));
                        //						}
                        messages.AddMessages(dcFacade.CheckID(actionEventArgs.RunningCard, actionEventArgs.ActionType, actionEventArgs.ResourceCode, actionEventArgs.UserCode, actionEventArgs.ProductInfo, actionCheckStatus));
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
        /// ����Resourceά����ReworkSheetCode��RCard���в���Ͳ�Pallet����
        /// ͬʱ����RCard��ReworkSheetCode��Mapping��ϵ
        /// </summary>
        /// <param name="actionEventArgs"></param>
        /// <returns></returns>
        public Messages AddReworkRangeByResource(ActionEventArgs actionEventArgs)
        {
            Messages messages = new Messages();

            ReworkFacade reworkFacade = new ReworkFacade(this.DataProvider);

            try
            {
                if (actionEventArgs.ProductInfo == null || actionEventArgs.ProductInfo.LastSimulation == null)
                    return messages;

                // Get Resource and Resource.ReworkSheetCode
                Resource res = actionEventArgs.ProductInfo.Resource;
                BaseModelFacade modelFacade = new BaseModelFacade(this.DataProvider);
                if (res == null)
                {
                    res = (Resource)modelFacade.GetResource(actionEventArgs.ResourceCode);
                    actionEventArgs.ProductInfo.Resource = res;
                }

                string reworkCode = GetReworkCode(res, actionEventArgs);

                if (reworkCode.Trim().Length > 0)
                {
                    object[] reworkRangeList = reworkFacade.QueryReworkRange(
                        reworkCode.Trim(),
                        actionEventArgs.ProductInfo.LastSimulation.RunningCard);

                    if (reworkRangeList == null)
                    {
                        // ����ReworkRange   
                        DBDateTime currentDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

                        ReworkRange rr = reworkFacade.CreateNewReworkRange();
                        rr.ReworkCode = reworkCode.Trim();
                        rr.RunningCard = actionEventArgs.ProductInfo.LastSimulation.RunningCard;
                        rr.RunningCardSequence = actionEventArgs.ProductInfo.LastSimulation.RunningCardSequence;
                        rr.EAttribute1 = "";
                        rr.MaintainUser = actionEventArgs.UserCode;
                        rr.MaintainDate = currentDateTime.DBDate;
                        rr.MaintainTime = currentDateTime.DBTime;
                        if (reworkFacade.GetReworkRange(rr.ReworkCode, rr.RunningCard, rr.RunningCardSequence) == null)
                        {
                            reworkFacade.AddReworkRange(rr);
                        }

                        // Update Rework Sheet Real Rework Quantity
                        reworkFacade.UpdateReworkSheetRealQty(reworkCode.Trim());

                        // ����Ͳ�Pallet
                        DataCollectFacade dcf = new DataCollectFacade(this.DataProvider);
                        if (actionEventArgs.ProductInfo.LastSimulation.CartonCode != null
                            && actionEventArgs.ProductInfo.LastSimulation.CartonCode.Trim().Length > 0)
                        {
                            messages.AddMessages(dcf.RemoveFromCarton(actionEventArgs.ProductInfo.LastSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode));
                            if (messages.IsSuccess())
                            {
                                actionEventArgs.ProductInfo.LastSimulation.CartonCode = "";
                                messages.ClearMessages();
                            }
                        }

                        if (messages.IsSuccess())
                        {
                            Package.PackageFacade packageFacade = new BenQGuru.eMES.Package.PackageFacade(DataProvider);

                            Pallet2RCard pallet2RCard = packageFacade.GetPallet2RCardByRCard(actionEventArgs.ProductInfo.LastSimulation.RunningCard.ToUpper()) as Pallet2RCard;
                            if (pallet2RCard != null)
                            {
                                Pallet pallet = packageFacade.GetPallet(pallet2RCard.PalletCode) as Pallet;
                                if (pallet != null)
                                {
                                    messages.AddMessages(dcf.RemoveFromPallet(actionEventArgs.ProductInfo.LastSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode, true));
                                    if (messages.IsSuccess())
                                    {
                                        actionEventArgs.ProductInfo.LastSimulation.PalletCode = "";
                                        messages.ClearMessages();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messages.Add(new Message(ex));
            }
            return messages;
        }

        public string GetReworkCode(Resource res, ActionEventArgs actionEventArgs)
        {
            string reworkCode = string.Empty;

            string resCode = res.ResourceCode;
            string runningCard = actionEventArgs.ProductInfo.LastSimulation.RunningCard;
            string itemCode = actionEventArgs.ProductInfo.LastSimulation.ItemCode;
            string moCode = actionEventArgs.ProductInfo.LastSimulation.MOCode;
            string productStatus = actionEventArgs.ProductInfo.LastSimulation.ProductStatus;

            OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
            BaseModelFacade baseModelFacade = new BaseModelFacade(this.DataProvider);
            ReworkFacade reworkFacade = new ReworkFacade(this.DataProvider);

            //�ж�RunningCard�Ƿ�ΪReject״̬
            if (productStatus == ProductStatus.Reject)
            {
                OQCLot lot = (OQCLot)oqcFacade.GetLatestRejectedOQCLot(runningCard);
                if (lot == null)
                {
                    //�Ҳ�����Ӧ��������
                    throw new Exception("$CS_NoRejectedLotFound $CS_Param_ID=" + runningCard);
                }
                else
                {
                    object[] reworkList = baseModelFacade.QueryResource2ReworkSheet(resCode, itemCode, lot.LOTNO);
                    if (reworkList == null)
                    {
                        //����Ʒ�Ҳ�����Ӧ�ķ�������
                        throw new Exception("$CS_NoReworkSheetFoundForReject $CS_Param_ID=" + runningCard);
                    }
                    else
                    {
                        reworkCode = ((Resource2ReworkSheet)reworkList[0]).ReworkCode;
                    }
                }

            }
            else
            {
                object[] reworkList = baseModelFacade.QueryResource2ReworkSheet(resCode, itemCode, string.Empty);
                if (reworkList != null)
                {
                    reworkCode = ((Resource2ReworkSheet)reworkList[0]).ReworkCode;
                }
            }

            if (reworkCode.Trim().Length > 0)
            {
                ReworkSheet reworkSheet = (ReworkSheet)reworkFacade.GetReworkSheet(reworkCode);
                if (reworkSheet == null)
                {
                    throw new Exception("$Error_ReworkSheet_NotExist $Para_ReworkCode=" + reworkCode);
                }

                // �жϷ�������״̬�Ƿ���ȷ
                if (reworkSheet.Status != ReworkStatus.REWORKSTATUS_OPEN)
                {
                    throw new Exception("$Error_ReworkSheetStatus_Invalid, $Para_ReworkCode=" + reworkCode + ", $Para_ReworkSheetStatus=" + reworkSheet.Status);
                }

                // �������󵥵�ItemCode�͵�ǰRcard��ItemCode��һ��
                if (string.Compare(itemCode, reworkSheet.ItemCode, true) != 0)
                {
                    throw new Exception("$Error_ReworkSheetItemCodeNotMapping");
                }
            }

            return reworkCode;
        }

        private bool CheckBeforeRework(ActionEventArgs actionEventArgs)
        {
            string opCode = string.Empty;
            BaseModelFacade baseModelFacade = new BaseModelFacade(this.DataProvider);
            Operation2Resource op2Res = (Operation2Resource)baseModelFacade.GetOperationByResource(actionEventArgs.ResourceCode);
            if (op2Res != null)
            {
                opCode = op2Res.OPCode;
            }

            string lastAction = string.Empty;
            string[] actionList = actionEventArgs.ProductInfo.LastSimulation.ActionList.Split(';');
            if (actionList != null)
            {
                for (int i = 0; i < actionList.Length; i++)
                {
                    if (actionList[i].Trim().Length > 0)
                    {
                        lastAction = actionList[i].Trim();
                    }
                }
            }

            if (string.Compare(lastAction, "GOMO", true) == 0)
            {
                return true;
            }
            else
            {
                return (actionEventArgs.ProductInfo.LastSimulation.OPCode != opCode);
            }
        }

        // Added By Hi1/Venus.Feng on 20080711 for Hisense Version : Add Adjust Rework Route by Resource
        /// <summary>
        /// Adjust Rework Route by Resource Code
        /// </summary>
        /// <param name="actionEventArgs">ActionEventArgs</param>
        /// <returns></returns>
        public Messages AdjustReworkRouteByResource(ActionEventArgs actionEventArgs)
        {
            Messages messages = new Messages();
            try
            {
                if (actionEventArgs.ProductInfo == null || actionEventArgs.ProductInfo.LastSimulation == null)
                    return messages;

                // Get Resource and Resource.ReworkRouteCode
                Resource res = actionEventArgs.ProductInfo.Resource;
                BaseModelFacade modelFacade = new BaseModelFacade(this.DataProvider);
                DataCollectFacade dcf = new DataCollectFacade(this.DataProvider);

                if (res == null)
                {
                    res = (Resource)modelFacade.GetResource(actionEventArgs.ResourceCode);
                    actionEventArgs.ProductInfo.Resource = res;
                }
                if (res.ReworkRouteCode == null || res.ReworkRouteCode == "")
                    return messages;

                // ��鷵��;�����Ʒ�Ĺ���
                ItemFacade itemFacade = new ItemFacade(this.DataProvider);

                // ��鷵��;���Ƿ����ڸò�Ʒ��;���е�һ��
                if (itemFacade.GetItem2Route(actionEventArgs.ProductInfo.LastSimulation.ItemCode, res.ReworkRouteCode, res.OrganizationID.ToString()) == null)
                {
                    throw new Exception("$Error_ReworkRouteNotBelongToItem [" + res.ReworkRouteCode + "]");
                }


                //������к��Ѿ��깤�����ܷ���
                if (actionEventArgs.ProductInfo.LastSimulation.IsComplete == FormatHelper.TRUE_STRING)
                {
                    throw new Exception("$Error_RunningCard_IsCompelete");
                }

                //��ʱ��ӣ�
                if (actionEventArgs.ProductInfo.LastSimulation.ProductStatus == ProductStatus.NG)
                {
                    throw new Exception("$Error_RCardInTSCannotRework");
                }

                // ����Good����������жϵ�ǰItem��;�̺͹����Ƿ����Rework;�̵ĵ�һ���������ǵĻ������κδ���
                ItemRoute2OP op = itemFacade.GetMORouteFirstOperation(actionEventArgs.ProductInfo.LastSimulation.MOCode, res.ReworkRouteCode);
                if (actionEventArgs.ProductInfo.LastSimulation.RouteCode == res.ReworkRouteCode &&
                    actionEventArgs.ProductInfo.LastSimulation.OPCode == op.OPCode &&
                    actionEventArgs.ProductInfo.LastSimulation.ProductStatus == ProductStatus.GOOD)
                {
                    return messages;
                }

                DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

                //actionEventArgs.ProductInfo.NowSimulation = (Simulation)actionEventArgs.ProductInfo.LastSimulation;
                actionEventArgs.ProductInfo.NowSimulation = dcf.CloneSimulation(actionEventArgs.ProductInfo.LastSimulation);

                // �������Ʒ��Reject
                if (actionEventArgs.ProductInfo.LastSimulation.ProductStatus == ProductStatus.GOOD ||
                    actionEventArgs.ProductInfo.LastSimulation.ProductStatus == ProductStatus.Reject)
                {
                    actionEventArgs.ProductInfo.NowSimulation.ProductStatus = ProductStatus.GOOD;
                    actionEventArgs.ProductInfo.NowSimulation.RouteCode = res.ReworkRouteCode;
                    actionEventArgs.ProductInfo.NowSimulation.OPCode = op.OPCode;
                    actionEventArgs.ProductInfo.NowSimulation.ResourceCode = res.ResourceCode;
                    actionEventArgs.ProductInfo.NowSimulation.LastAction = ActionType.DataCollectAction_GoMO;
                    actionEventArgs.ProductInfo.NowSimulation.ActionList = "";
                    actionEventArgs.ProductInfo.NowSimulation.IsComplete = "0";
                    actionEventArgs.ProductInfo.NowSimulation.MaintainUser = actionEventArgs.UserCode;
                    actionEventArgs.ProductInfo.NowSimulation.MaintainDate = dbTime.DBDate;
                    actionEventArgs.ProductInfo.NowSimulation.MaintainTime = dbTime.DBTime;

                    actionEventArgs.ProductInfo.LastSimulation.NextRouteCode = res.ReworkRouteCode;
                    actionEventArgs.ProductInfo.LastSimulation.NextOPCode = op.OPCode;
                }
                else if (actionEventArgs.ProductInfo.LastSimulation.ProductStatus == ProductStatus.NG)
                {
                    // ����ǲ���Ʒ�������TS��״̬�ǻ���
                    TSFacade tsFacade = new TSFacade(this.DataProvider);
                    object objTs = tsFacade.GetCardLastTSRecord(actionEventArgs.ProductInfo.LastSimulation.RunningCard);
                    bool bFlag = false;
                    if (objTs != null)
                    {
                        Domain.TS.TS ts = (Domain.TS.TS)objTs;
                        // ����Ѿ��ǻ���������Ļ���;��
                        if (ts.TSStatus == TSStatus.TSStatus_Reflow)
                        {
                            if (ts.ReflowRouteCode == res.ReworkRouteCode && ts.ReflowOPCode == op.OPCode)
                            {
                                return messages;
                            }
                            if (ts.ReflowRouteCode != res.ReworkRouteCode || ts.ReflowOPCode != op.OPCode)
                            {
                                ts.ReflowRouteCode = res.ReworkRouteCode;
                                ts.ReflowOPCode = op.OPCode;
                                ts.MaintainUser = actionEventArgs.UserCode;
                                tsFacade.UpdateTS(ts);
                                bFlag = true;
                            }
                        }
                        else
                        {
                            // �����New�Ļ���˵���ո�ת�뵽TS�У���ʱӦ���ȸ���Confirm��Ϣ��������������
                            if (ts.TSStatus == TSStatus.TSStatus_New)
                            {
                                ts.ConfirmResourceCode = actionEventArgs.ResourceCode;
                                ts.ConfirmUser = actionEventArgs.UserCode;
                                ts.ConfirmDate = dbTime.DBDate;
                                ts.ConfirmTime = dbTime.DBTime;
                                ts.TSStatus = TSStatus.TSStatus_TS;
                                tsFacade.UpdateTS(ts);
                                bFlag = true;
                            }
                            if (ts.TSStatus == TSStatus.TSStatus_TS)
                            {
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
                                    res.ReworkRouteCode,
                                    op.OPCode,
                                    actionEventArgs.UserCode,
                                    null);
                                tsactionEventArgs.RouteCode = res.ReworkRouteCode;
                                tsactionEventArgs.IgnoreResourceInOPTS = true;
                                messages.AddMessages(actionTSComplete.Execute(tsactionEventArgs));
                                if (messages.IsSuccess() == true)
                                    bFlag = true;
                            }
                        }
                    }
                    if (bFlag == true)
                    {
                        actionEventArgs.ProductInfo.NowSimulation.ActionList = "";
                    }
                    else
                        return messages;
                }
                else
                {
                    return messages;
                }

                actionEventArgs.ProductInfo.NowSimulationReport = this.FillSimulationReport(actionEventArgs.ProductInfo);

                if (messages.IsSuccess() == true)
                {
                    // ����Simulation��SimulationReport
                    this.DataProvider.Update(actionEventArgs.ProductInfo.NowSimulation);
                    this.DataProvider.Update(actionEventArgs.ProductInfo.NowSimulationReport);
                }

                // ����Ͳ�Pallet                
                if (actionEventArgs.ProductInfo.LastSimulation.CartonCode != null
                    && actionEventArgs.ProductInfo.LastSimulation.CartonCode.Trim().Length > 0)
                {
                    messages.AddMessages(dcf.RemoveFromCarton(actionEventArgs.ProductInfo.LastSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode));
                    if (messages.IsSuccess())
                    {
                        actionEventArgs.ProductInfo.LastSimulation.CartonCode = "";
                        messages.ClearMessages();
                    }
                }

                if (messages.IsSuccess())
                {
                    Package.PackageFacade packageFacade = new BenQGuru.eMES.Package.PackageFacade(DataProvider);

                    Pallet2RCard pallet2RCard = packageFacade.GetPallet2RCardByRCard(actionEventArgs.ProductInfo.LastSimulation.RunningCard.ToUpper()) as Pallet2RCard;
                    if (pallet2RCard != null)
                    {
                        Pallet pallet = packageFacade.GetPallet(pallet2RCard.PalletCode) as Pallet;
                        if (pallet != null)
                        {
                            messages.AddMessages(dcf.RemoveFromPallet(actionEventArgs.ProductInfo.LastSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode, true));
                            if (messages.IsSuccess())
                            {
                                actionEventArgs.ProductInfo.LastSimulation.PalletCode = "";
                                messages.ClearMessages();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messages.Add(new Message(ex));
            }
            return messages;
        }
        // End Added

        public Messages CheckIDOutline(ActionEventArgs actionEventArgs)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "CheckID");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                if (actionEventArgs.ActionType != ActionType.DataCollectAction_GoMO)
                {
                    if (actionEventArgs.ProductInfo.LastSimulation == null)
                    {
                        throw new Exception("$NoSimulationInfo");
                    }
                    else
                    {
                        DataCollectFacade dcFacade = new DataCollectFacade(this.DataProvider);
                        messages.AddMessages(dcFacade.CheckIDOutline(actionEventArgs.RunningCard, actionEventArgs.ActionType, actionEventArgs.ResourceCode, ((OutLineActionEventArgs)actionEventArgs).OPCode, actionEventArgs.UserCode, actionEventArgs.ProductInfo));
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

        // ����Simulation ����д
        public Messages Execute(ActionEventArgs actionEventArgs)
        {
            return Execute(actionEventArgs, null);
            #region code not to use
            /*
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				DataCollectFacade dataCollect=new DataCollectFacade(this.DataProvider);
				

				actionEventArgs.ProductInfo.NowSimulation.MaintainUser = actionEventArgs.UserCode;
				actionEventArgs.ProductInfo.NowSimulation.MaintainDate =	FormatHelper.TODateInt(DateTime.Now) ;
				actionEventArgs.ProductInfo.NowSimulation.MaintainTime =	FormatHelper.TOTimeInt(DateTime.Now) ;

				actionEventArgs.ProductInfo.NowSimulationReport =	this.FillSimulationReport(actionEventArgs.ProductInfo);

				// LastSimulation !=null ������� ����Ͷ�� ����
				if (actionEventArgs.ProductInfo.LastSimulation !=null)
				{
					if ((actionEventArgs.ProductInfo.LastSimulation.RunningCard !=	actionEventArgs.ProductInfo.NowSimulation.RunningCard)
						||(actionEventArgs.ProductInfo.LastSimulation.MOCode  !=actionEventArgs.ProductInfo.NowSimulation.MOCode))
					{
						dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);	
						dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);

						dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.LastSimulation);
						dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport );
					}
					else
					{
						dataCollect.UpdateSimulation(actionEventArgs.ProductInfo.NowSimulation);
						dataCollect.UpdateSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport );
					}
				}
				else
				{
					dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);
					dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport );
				}

				dataCollect.AddOnWIP(this.FillOnWip(actionEventArgs.ProductInfo));
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
			*/
            #endregion
        }
        // Added by Icyer 2005/10/31
        // ��չһ����ActionCheckStatus�����ķ���
        public Messages Execute(ActionEventArgs actionEventArgs, Action.ActionCheckStatus actionCheckStatus)
        {
            return Execute(actionEventArgs, actionCheckStatus, false, true);
        }

        public Messages Execute(ActionEventArgs actionEventArgs, Action.ActionCheckStatus actionCheckStatus, bool updateSimulation, bool addWip)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                DataCollectFacade dataCollect = new DataCollectFacade(this.DataProvider);
                //Laws Lu,2006/11/13 uniform system collect date
                DBDateTime dbDateTime;
                //Laws Lu,2006/11/13 uniform system collect date
                if (actionEventArgs.ProductInfo.WorkDateTime != null)
                {
                    dbDateTime = actionEventArgs.ProductInfo.WorkDateTime;
                }
                else
                {
                    dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
                    actionEventArgs.ProductInfo.WorkDateTime = dbDateTime;

                }
                actionEventArgs.ProductInfo.NowSimulation.MaintainDate = dbDateTime.DBDate;
                actionEventArgs.ProductInfo.NowSimulation.MaintainTime = dbDateTime.DBTime;

                actionEventArgs.ProductInfo.NowSimulation.MaintainUser = actionEventArgs.UserCode;

                //				actionEventArgs.ProductInfo.NowSimulation.MaintainDate =	FormatHelper.TODateInt(DateTime.Now) ;
                //				actionEventArgs.ProductInfo.NowSimulation.MaintainTime =	FormatHelper.TOTimeInt(DateTime.Now) ;

                actionEventArgs.ProductInfo.NowSimulationReport = this.FillSimulationReport(actionEventArgs.ProductInfo);

                // ���actionCheckStatus��Ϊ�գ������actionCheckStatus
                if (actionCheckStatus != null)
                {
                    actionCheckStatus.ProductInfo = actionEventArgs.ProductInfo;
                }

                //�����Ҫ����Simulation
                if (actionCheckStatus == null || actionCheckStatus.NeedUpdateSimulation == true || updateSimulation == true)
                {
                    //Ϊ���滻��ȷ����Դ
                    if (actionEventArgs.ActionType == ActionType.DataCollectAction_OQCNG
                        || actionEventArgs.ActionType == ActionType.DataCollectAction_OQCReject)
                    {
                        actionEventArgs.ProductInfo.NowSimulation.ResourceCode = actionEventArgs.ResourceCode;
                        actionEventArgs.ProductInfo.NowSimulationReport.ResourceCode = actionEventArgs.ResourceCode;
                    }

                    // LastSimulation !=null ������� ����Ͷ�� ����
                    if (actionEventArgs.ProductInfo.LastSimulation != null)
                    {
                        if ((actionEventArgs.ProductInfo.LastSimulation.RunningCard != actionEventArgs.ProductInfo.NowSimulation.RunningCard)
                            || (actionEventArgs.ProductInfo.LastSimulation.MOCode != actionEventArgs.ProductInfo.NowSimulation.MOCode))
                        {
                            /* ���к�ת������ */
                            if (actionEventArgs is SplitIDActionEventArgs
                                && string.Compare((actionEventArgs as SplitIDActionEventArgs).IDMergeType, IDMergeType.IDMERGETYPE_IDMERGE, true) == 0)
                            {
                                #region �ְ�
                                SplitIDActionEventArgs splitActionEventArgs = actionEventArgs as SplitIDActionEventArgs;
                                /* splitActionEventArgs.UpdateSimulationΪtrue��˵��ת��������к���SimulationReport���Ǵ��ڵ� */
                                if (splitActionEventArgs.UpdateSimulation)
                                {
                                    if (splitActionEventArgs.IsSameMO)
                                    {
                                        /* ����ͬһ�Ź���
                                         * LastSimulation�ѱ�ת����������ҪDelete;  NowSimulation�Ѿ����ڣ�����update��simulationReportͬsimulation*/
                                        dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);
                                        dataCollect.UpdateSimulation(actionEventArgs.ProductInfo.NowSimulation);

                                        dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.LastSimulation);
                                        dataCollect.UpdateSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                    }
                                    else
                                    {
                                        //dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);	
                                        /* ���ڲ�ͬ�Ĺ�����ɾ��ԭ����Simulation��
                                         * ��Ȼ����ڵ�ǰ���������߱��ϣ��ٻص�ԭ���Ĺ����ɼ����ͻ���� */
                                        dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);
                                        dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.NowSimulation);
                                        dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);
                                        /* ɾ���Լ�������֤���Բ���SimulationReport */
                                        dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.LastSimulation);
                                        dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                        dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                    }
                                }
                                else
                                {
                                    dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);
                                    dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);

                                    dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.LastSimulation);
                                    dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                }

                                #endregion
                            }
                            else
                            {
                                //Laws Lu,2006/03/20 ���Ϻ����¹�����������
                                if (actionEventArgs.ProductInfo.LastSimulation.MOCode != actionEventArgs.ProductInfo.NowSimulation.MOCode)
                                {
                                    dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);

                                    dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                }
                                else
                                {
                                    dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);
                                    dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);

                                    dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.LastSimulation);
                                    dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                }
                            }
                        }
                        else
                        {
                            dataCollect.UpdateSimulation(actionEventArgs.ProductInfo.NowSimulation);
                            dataCollect.UpdateSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                        }
                    }
                    else
                    {
                        dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);
                        dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                    }
                }

                //�����Ҫ����Wip
                SystemSettingFacade systemSettingFacade = null;
                if (actionCheckStatus == null || actionCheckStatus.NeedUpdateSimulation == true)
                {
                    if (actionEventArgs.ActionType == ActionType.DataCollectAction_OQCNG)
                    {
                        dataCollect.AddOnWIP(this.FillOnWip(actionEventArgs.ProductInfo, actionEventArgs));
                    }
                    else
                    {
                        //SMT�Զ�����(SMT����-SMT���Ϲ���ֻ�ǽ�SMT�ϱ��ϣ���û�п��ϣ���Ҫ������վ���п���)
                        systemSettingFacade = new SystemSettingFacade(this.DataProvider);
                        SMTFacade _SMTFacade = new SMTFacade(this.DataProvider);

                        int number = dataCollect.GetRCardInfoCount(actionEventArgs.RunningCard, actionEventArgs.ProductInfo.NowSimulation.MOCode, actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode, actionEventArgs.ProductInfo.NowSimulation.OPCode.ToUpper());

                        //����ֻ��һ��action�н���smt���ϵ��жϣ�һ�βɼ����action�ظ����ϣ�added by Gawain 
                        if (number <= 0 && (actionCheckStatus == null || actionCheckStatus.ActionList.Count == 0))
                        {
                            object OPParameter = systemSettingFacade.GetParameter(actionEventArgs.ProductInfo.NowSimulation.OPCode.ToUpper(), "SMT_MATERIAL");
                            if (OPParameter != null)
                            {
                                //_SMTFacade.SensorCollect(actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode, "1", "1");
                                //modified by Gawain @20130704,for SMT�Զ�����Ӧ����Ҫ���ֹ���
                                _SMTFacade.SensorCollectWithMo(actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode, "1", "1", actionEventArgs.ProductInfo.NowSimulation.MOCode);

                            }
                            //��Ʒ���Ͼ����
                            object COOPParameter = systemSettingFacade.GetParameter(actionEventArgs.ProductInfo.NowSimulation.OPCode.ToUpper(), "SMTBINDOP");
                            if (COOPParameter != null)
                            {
                                SimulationReport simulationreport = actionEventArgs.ProductInfo.NowSimulationReport;
                                SMTFacade smtFacade = new SMTFacade(this.DataProvider);
                                var objs = smtFacade.GetMachineLoadedFeeder(simulationreport.MOCode, actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode);
                                if (objs != null)
                                {
                                    int i = 0;
                                    foreach (BenQGuru.eMES.Domain.SMT.MachineFeeder mf in objs)
                                    {
                                        BenQGuru.eMES.Domain.SMT.Reel reel = smtFacade.GetReel(mf.ReelNo) as BenQGuru.eMES.Domain.SMT.Reel;
                                        OnWIPItem onwipitem = new OnWIPItem();
                                        onwipitem.RunningCard = simulationreport.RunningCard;
                                        onwipitem.RunningCardSequence = simulationreport.RunningCardSequence;
                                        onwipitem.MOCode = simulationreport.MOCode;
                                        onwipitem.MSequence = i;
                                        onwipitem.MCARD = mf.ReelNo;
                                        onwipitem.ModelCode = simulationreport.ModelCode;
                                        onwipitem.ItemCode = simulationreport.ItemCode;
                                        onwipitem.MItemCode = mf.MaterialCode;
                                        onwipitem.MCardType = "1";
                                        onwipitem.Qty = mf.UnitQty;
                                        onwipitem.LotNO = mf.ReelNo;
                                        onwipitem.PCBA = ActionOnLineHelper.StringNull;//��PCBA��
                                        onwipitem.BIOS = ActionOnLineHelper.StringNull;//��BIOS��
                                        onwipitem.Version = ActionOnLineHelper.StringNull;//��Version��
                                        onwipitem.VendorCode = ActionOnLineHelper.StringNull;//��VendorCode
                                        onwipitem.DateCode = reel.DateCode;
                                        onwipitem.RouteCode = simulationreport.RouteCode;
                                        onwipitem.OPCode = simulationreport.OPCode;
                                        onwipitem.SegmentCode = simulationreport.SegmentCode;
                                        onwipitem.StepSequenceCode = simulationreport.StepSequenceCode;
                                        onwipitem.ResourceCode = simulationreport.ResourceCode;
                                        onwipitem.ShiftTypeCode = simulationreport.ShiftTypeCode;
                                        onwipitem.ShiftCode = simulationreport.ShiftCode;
                                        onwipitem.TimePeriodCode = simulationreport.TimePeriodCode;
                                        onwipitem.TransactionStatus = "NO";
                                        onwipitem.MaintainUser = simulationreport.MaintainUser;
                                        onwipitem.MaintainDate = simulationreport.MaintainDate;
                                        onwipitem.MaintainTime = simulationreport.MaintainTime;
                                        onwipitem.ActionType = 0;
                                        onwipitem.MOSeq = simulationreport.MOSeq;
                                        dataCollect.AddOnWIPItem(onwipitem);
                                        i++;
                                    }
                                }
                            }
                        }
                        //end 
                        dataCollect.AddOnWIP(this.FillOnWip(actionEventArgs.ProductInfo));
                    }
                }
                else if (addWip == true)
                {
                    if (actionEventArgs.ActionType == ActionType.DataCollectAction_OQCNG)
                    {
                        dataCollect.AddOnWIP(this.FillOnWip(actionEventArgs.ProductInfo, actionEventArgs));
                    }
                    else
                    {
                        //SMT�Զ�����(SMT����-SMT���Ϲ���ֻ�ǽ�SMT�ϱ��ϣ���û�п��ϣ���Ҫ������վ���п���)
                        systemSettingFacade = new SystemSettingFacade(this.DataProvider);
                        SMTFacade _SMTFacade = new SMTFacade(this.DataProvider);

                        int number = dataCollect.GetRCardInfoCount(actionEventArgs.RunningCard, actionEventArgs.ProductInfo.NowSimulation.MOCode, actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode, actionEventArgs.ProductInfo.NowSimulation.OPCode.ToUpper());

                        //����ֻ��һ��action�н���smt���ϵ��жϣ�һ�βɼ����action�ظ����ϣ�added by Gawain 
                        if (number <= 0 && (actionCheckStatus == null || actionCheckStatus.ActionList.Count == 0))
                        {
                            object OPParameter = systemSettingFacade.GetParameter(actionEventArgs.ProductInfo.NowSimulation.OPCode.ToUpper(), "SMT_MATERIAL");
                            if (OPParameter != null)
                            {
                                //modified by Gawain @20130704,for SMT�Զ�����Ӧ����Ҫ���ֹ���
                                _SMTFacade.SensorCollectWithMo(actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode, "1", "1", actionEventArgs.ProductInfo.NowSimulation.MOCode);
                            }
                            //��Ʒ���Ͼ����
                            object COOPParameter = systemSettingFacade.GetParameter(actionEventArgs.ProductInfo.NowSimulation.OPCode.ToUpper(), "SMTBINDOP");
                            if (COOPParameter != null)
                            {
                                SimulationReport simulationreport = actionEventArgs.ProductInfo.NowSimulationReport;
                                SMTFacade smtFacade = new SMTFacade(this.DataProvider);
                                var objs = smtFacade.GetMachineLoadedFeeder(simulationreport.MOCode, actionEventArgs.ProductInfo.NowSimulationReport.StepSequenceCode);
                                if (objs != null)
                                {
                                    int i = 0;
                                    foreach (BenQGuru.eMES.Domain.SMT.MachineFeeder mf in objs)
                                    {
                                        BenQGuru.eMES.Domain.SMT.Reel reel = smtFacade.GetReel(mf.ReelNo) as BenQGuru.eMES.Domain.SMT.Reel;
                                        OnWIPItem onwipitem = new OnWIPItem();
                                        onwipitem.RunningCard = simulationreport.RunningCard;
                                        onwipitem.RunningCardSequence = simulationreport.RunningCardSequence;
                                        onwipitem.MOCode = simulationreport.MOCode;
                                        onwipitem.MSequence = i;
                                        onwipitem.MCARD = mf.ReelNo;
                                        onwipitem.ModelCode = simulationreport.ModelCode;
                                        onwipitem.ItemCode = simulationreport.ItemCode;
                                        onwipitem.MItemCode = mf.MaterialCode;
                                        onwipitem.MCardType = "1";
                                        onwipitem.Qty = mf.UnitQty;
                                        onwipitem.LotNO = mf.ReelNo;
                                        onwipitem.PCBA = ActionOnLineHelper.StringNull;//��PCBA��
                                        onwipitem.BIOS = ActionOnLineHelper.StringNull;//��BIOS��
                                        onwipitem.Version = ActionOnLineHelper.StringNull;//��Version��
                                        onwipitem.VendorCode = ActionOnLineHelper.StringNull;//��VendorCode
                                        onwipitem.DateCode = reel.DateCode;
                                        onwipitem.RouteCode = simulationreport.RouteCode;
                                        onwipitem.OPCode = simulationreport.OPCode;
                                        onwipitem.SegmentCode = simulationreport.SegmentCode;
                                        onwipitem.StepSequenceCode = simulationreport.StepSequenceCode;
                                        onwipitem.ResourceCode = simulationreport.ResourceCode;
                                        onwipitem.ShiftTypeCode = simulationreport.ShiftTypeCode;
                                        onwipitem.ShiftCode = simulationreport.ShiftCode;
                                        onwipitem.TimePeriodCode = simulationreport.TimePeriodCode;
                                        onwipitem.TransactionStatus = "NO";
                                        onwipitem.MaintainUser = simulationreport.MaintainUser;
                                        onwipitem.MaintainDate = simulationreport.MaintainDate;
                                        onwipitem.MaintainTime = simulationreport.MaintainTime;
                                        onwipitem.ActionType = 0;
                                        onwipitem.MOSeq = simulationreport.MOSeq;
                                        dataCollect.AddOnWIPItem(onwipitem);
                                        i++;
                                    }
                                }
                            }
                            //end
                        }
                        actionEventArgs.OnWIP.Add(this.FillOnWip(actionEventArgs.ProductInfo));
                    }
                }

                #region ���ݲ����趨����ۼ��ǹܿ���
                if (systemSettingFacade == null)
                {
                    systemSettingFacade = new SystemSettingFacade(this.DataProvider);
                }
                //���ϲ����� ��Ӧ��������ALERTDISER��ALERTDISFLAG��ALERTDISNORMAL��ALERTDISOP
                string paramGroup = "AlertMaterialDisGroup";
                string isDisMaterial = string.Empty;
                string alertOPCode = string.Empty;
                decimal normalAlert = 0;
                decimal erAlert = 0;

                object[] paramAlertList = systemSettingFacade.GetParametersByParameterGroup(paramGroup.ToUpper());
                foreach (Parameter param in paramAlertList)
                {
                    decimal tmpTime = 0;
                    if (param.ParameterCode == "ALERTDISFLAG")
                    {
                        isDisMaterial = param.ParameterAlias.ToUpper();
                    }
                    else if (param.ParameterCode == "ALERTDISOP")
                    {
                        alertOPCode = param.ParameterAlias.ToUpper();
                    }
                    else if (param.ParameterCode == "ALERTDISNORMAL")
                    {
                        if (decimal.TryParse(param.ParameterAlias, out tmpTime))
                            normalAlert = tmpTime;
                    }
                    else if (param.ParameterCode == "ALERTDISER")
                    {
                        if (decimal.TryParse(param.ParameterAlias, out tmpTime))
                            erAlert = tmpTime;
                    }
                }

                if (isDisMaterial == "Y")//��Ҫ����
                {
                    SimulationReport simulationreport = actionEventArgs.ProductInfo.NowSimulationReport;
                    MOFacade moFacade = new MOFacade(this.DataProvider);
                    //�жϵ�ǰվ�Ƿ�������õĿ���վ
                    if (alertOPCode == simulationreport.OPCode)
                    {
                        //��ȡ������ϸ��Ϣ
                        object[] disDetailObjs = dataCollect.QueryDisToLineDetailForMType(null,
                                                                         simulationreport.MOCode,
                                                                         simulationreport.StepSequenceCode,
                                                                         "'item_control_nocontrol'");

                        //��ȡ����BOM�����зǿع������ϱ���
                        object[] moBomObjs = moFacade.GetMOBOM(simulationreport.MOCode);
                        if (moBomObjs != null && disDetailObjs != null)
                        {
                            //����DisToLineDetail,�ҳ��͹���bom������Ϻţ������ݹ��������ۼ���
                            foreach (DisToLineDetail disDetail in disDetailObjs)
                            {
                                foreach (MOBOM moBom in moBomObjs)
                                {
                                    if (disDetail.MCode == moBom.MOBOMItemCode)
                                    {
                                        //�ۼ�����
                                        disDetail.MssleftQty = disDetail.MssleftQty - moBom.MOBOMItemQty;
                                        disDetail.MaintainDate = dbDateTime.DBDate;
                                        disDetail.MaintainTime = dbDateTime.DBTime;
                                        disDetail.MaintainUser = simulationreport.MaintainUser;
                                        dataCollect.UpdateDistolinedetail(disDetail);

                                    }
                                }
                            }
                        }
                    }


                }

                #endregion
            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            return messages;
        }

        public Messages Execute(ActionEventArgs actionEventArgs, bool updateSimulation, bool addWip)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                DataCollectFacade dataCollect = new DataCollectFacade(this.DataProvider);
                //Laws Lu,2006/11/13 uniform system collect date
                DBDateTime dbDateTime;
                //Laws Lu,2006/11/13 uniform system collect date
                if (actionEventArgs.ProductInfo.WorkDateTime != null)
                {
                    dbDateTime = actionEventArgs.ProductInfo.WorkDateTime;
                }
                else
                {
                    dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
                    actionEventArgs.ProductInfo.WorkDateTime = dbDateTime;

                }
                actionEventArgs.ProductInfo.NowSimulation.MaintainDate = dbDateTime.DBDate;
                actionEventArgs.ProductInfo.NowSimulation.MaintainTime = dbDateTime.DBTime;

                actionEventArgs.ProductInfo.NowSimulation.MaintainUser = actionEventArgs.UserCode;

                //				actionEventArgs.ProductInfo.NowSimulation.MaintainDate =	FormatHelper.TODateInt(DateTime.Now) ;
                //				actionEventArgs.ProductInfo.NowSimulation.MaintainTime =	FormatHelper.TOTimeInt(DateTime.Now) ;

                actionEventArgs.ProductInfo.NowSimulationReport = this.FillSimulationReport(actionEventArgs.ProductInfo);


                //�����Ҫ����Simulation
                if (updateSimulation == true)
                {
                    //Ϊ���滻��ȷ����Դ
                    if (actionEventArgs.ActionType == ActionType.DataCollectAction_OQCNG
                        || actionEventArgs.ActionType == ActionType.DataCollectAction_OQCReject)
                    {
                        actionEventArgs.ProductInfo.NowSimulation.ResourceCode = actionEventArgs.ResourceCode;
                        actionEventArgs.ProductInfo.NowSimulationReport.ResourceCode = actionEventArgs.ResourceCode;
                    }

                    // LastSimulation !=null ������� ����Ͷ�� ����
                    if (actionEventArgs.ProductInfo.LastSimulation != null)
                    {
                        if ((actionEventArgs.ProductInfo.LastSimulation.RunningCard != actionEventArgs.ProductInfo.NowSimulation.RunningCard)
                            || (actionEventArgs.ProductInfo.LastSimulation.MOCode != actionEventArgs.ProductInfo.NowSimulation.MOCode))
                        {
                            /* ���к�ת������ */
                            if (actionEventArgs is SplitIDActionEventArgs
                                && string.Compare((actionEventArgs as SplitIDActionEventArgs).IDMergeType, IDMergeType.IDMERGETYPE_IDMERGE, true) == 0)
                            {
                                #region �ְ�
                                SplitIDActionEventArgs splitActionEventArgs = actionEventArgs as SplitIDActionEventArgs;
                                /* splitActionEventArgs.UpdateSimulationΪtrue��˵��ת��������к���SimulationReport���Ǵ��ڵ� */
                                if (splitActionEventArgs.UpdateSimulation)
                                {
                                    if (splitActionEventArgs.IsSameMO)
                                    {
                                        /* ����ͬһ�Ź���
                                         * LastSimulation�ѱ�ת����������ҪDelete;  NowSimulation�Ѿ����ڣ�����update��simulationReportͬsimulation*/
                                        dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);
                                        dataCollect.UpdateSimulation(actionEventArgs.ProductInfo.NowSimulation);

                                        dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.LastSimulation);
                                        dataCollect.UpdateSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                    }
                                    else
                                    {
                                        //dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);	
                                        /* ���ڲ�ͬ�Ĺ�����ɾ��ԭ����Simulation��
                                         * ��Ȼ����ڵ�ǰ���������߱��ϣ��ٻص�ԭ���Ĺ����ɼ����ͻ���� */
                                        dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);
                                        dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.NowSimulation);
                                        dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);
                                        /* ɾ���Լ�������֤���Բ���SimulationReport */
                                        dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.LastSimulation);
                                        dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                        dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                    }
                                }
                                else
                                {
                                    dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);
                                    dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                }

                                #endregion
                            }
                            else
                            {
                                //Laws Lu,2006/03/20 ���Ϻ����¹�����������
                                if (actionEventArgs.ProductInfo.LastSimulation.MOCode != actionEventArgs.ProductInfo.NowSimulation.MOCode)
                                {
                                    dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);

                                    dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                }
                                else
                                {
                                    dataCollect.DeleteSimulation(actionEventArgs.ProductInfo.LastSimulation);
                                    dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);

                                    dataCollect.DeleteSimulationReport(actionEventArgs.ProductInfo.LastSimulation);
                                    dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                                }
                            }
                        }
                        else
                        {
                            dataCollect.UpdateSimulation(actionEventArgs.ProductInfo.NowSimulation);
                            dataCollect.UpdateSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                        }
                    }
                    else
                    {
                        dataCollect.AddSimulation(actionEventArgs.ProductInfo.NowSimulation);
                        dataCollect.AddSimulationReport(actionEventArgs.ProductInfo.NowSimulationReport);
                    }
                }
                else if (updateSimulation == false)
                {
                    //Ҫ����simulation��SEQ��ProductStatus
                    object simulation = dataCollect.GetLastSimulation(actionEventArgs.ProductInfo.LastSimulation.RunningCard);
                    ((Simulation)simulation).RunningCardSequence = actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence;
                    ((Simulation)simulation).ProductStatus = actionEventArgs.ProductInfo.NowSimulation.ProductStatus;
                    dataCollect.UpdateSimulation((Simulation)simulation);
                    //add by johnson.shao 20110516 ��Ҫ����simulationReport
                    object simulationreport = dataCollect.GetLastSimulationReport(actionEventArgs.ProductInfo.LastSimulation.RunningCard);
                    ((SimulationReport)simulationreport).RunningCardSequence = actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence;
                    ((SimulationReport)simulationreport).Status = actionEventArgs.ProductInfo.NowSimulation.ProductStatus;
                    dataCollect.UpdateSimulationReport((SimulationReport)simulationreport);
                }

                //�����Ҫ����Wip
                if (addWip == true)
                {
                    dataCollect.AddOnWIP(this.FillOnWip(actionEventArgs.ProductInfo));

                }
                //else if (addWip == true)
                //{
                //    if (actionEventArgs.ActionType == ActionType.DataCollectAction_OQCNG)
                //    {
                //        dataCollect.AddOnWIP(this.FillOnWip(actionEventArgs.ProductInfo, actionEventArgs));
                //    }
                //    else
                //    {
                //        actionEventArgs.OnWIP.Add(this.FillOnWip(actionEventArgs.ProductInfo));
                //    }
                //}
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
        /// ** ��������:	�жϸ�resource code�ڲ�operation TS����Դ�б���
        /// ** �� ��:		crystal chu
        /// ** �� ��:		2005/07/26/
        /// ** �� ��:
        /// ** �� ��:
        /// ** nunit
        /// </summary>
        /// <param name="actionEventArgs"></param>
        /// <returns></returns>
        public Messages CheckResourceInOperationTS(ActionEventArgs actionEventArgs)
        {
            Messages messages = new Messages();
            BaseModelFacade baseModelFacade = new BaseModelFacade(this.DataProvider);
            if (!baseModelFacade.IsResourceInOperationTS(actionEventArgs.ResourceCode))
            {
                messages.Add(new Message(MessageType.Error, "$CSError_Resource_Not_In_OperationTS"));
            }
            return messages;
        }



        /// <summary>
        /// ** ��������:	����Ʒ���к���ά�޼�¼���Ƿ����
        /// ** �� ��:		crystal chu
        /// ** �� ��:		2005/07/26/
        /// ** �� ��:
        /// ** �� ��:
        /// ** nunit
        /// </summary>
        /// <param name="rcard">Ҫ���Ĳ�Ʒ���к�</param>
        /// <returns></returns>
        public Messages CheckIDInTSRecord(string rcard)
        {
            Messages messages = new Messages();
            TSFacade tsFacade = new TSFacade(this.DataProvider);
            if (tsFacade.IsCardInTS(rcard.Trim()))
            {
                messages.Add(new Message(MessageType.Error, "$CSError_Card_Not_In_TS"));
            }
            return messages;
        }



        //��дSimulationReport ID,  ;��,ʱ��,����ģ��,�û���Ϣ ������ ProductInfo.NewSimulation
        public SimulationReport FillSimulationReport(ProductInfo productInfo)
        {
            SimulationReport simulationReport = new SimulationReport();
            simulationReport.RouteCode = productInfo.NowSimulation.RouteCode;
            simulationReport.OPCode = productInfo.NowSimulation.OPCode;
            simulationReport.CartonCode = productInfo.NowSimulation.CartonCode;
            simulationReport.EAttribute1 = productInfo.NowSimulation.EAttribute1;
            simulationReport.EAttribute2 = productInfo.NowSimulation.EAttribute2;
            simulationReport.IDMergeRule = productInfo.NowSimulation.IDMergeRule;
            simulationReport.IsComplete = productInfo.NowSimulation.IsComplete;
            simulationReport.ItemCode = productInfo.NowSimulation.ItemCode;
            simulationReport.LastAction = productInfo.NowSimulation.LastAction;
            simulationReport.LOTNO = productInfo.NowSimulation.LOTNO;
            simulationReport.MaintainDate = productInfo.NowSimulation.MaintainDate;
            simulationReport.MaintainTime = productInfo.NowSimulation.MaintainTime;
            simulationReport.MaintainUser = productInfo.NowSimulation.MaintainUser;
            simulationReport.MOCode = productInfo.NowSimulation.MOCode;
            simulationReport.ModelCode = productInfo.NowSimulation.ModelCode;
            simulationReport.NGTimes = productInfo.NowSimulation.NGTimes;
            simulationReport.PalletCode = productInfo.NowSimulation.PalletCode;
            simulationReport.ResourceCode = productInfo.NowSimulation.ResourceCode;
            simulationReport.RunningCard = productInfo.NowSimulation.RunningCard;
            simulationReport.RunningCardSequence = productInfo.NowSimulation.RunningCardSequence;
            simulationReport.Status = productInfo.NowSimulation.ProductStatus;
            simulationReport.TranslateCard = productInfo.NowSimulation.TranslateCard;
            simulationReport.TranslateCardSequence = productInfo.NowSimulation.TranslateCardSequence;
            simulationReport.SourceCard = productInfo.NowSimulation.SourceCard;
            simulationReport.SourceCardSequence = productInfo.NowSimulation.SourceCardSequence;
            simulationReport.RMABillCode = productInfo.NowSimulation.RMABillCode;

            //added by jessie, 2006-5-29, ��ӳ���
            simulationReport.ShelfNO = productInfo.NowSimulation.ShelfNO;
            //add by Laws,2006/05/31,Carton number
            simulationReport.CartonCode = productInfo.NowSimulation.CartonCode;

            // Changed by Icyer 2005/10/28
            /*
            BaseModelFacade dataModel=new BaseModelFacade(this.DataProvider);
            Resource resource				= (Resource)dataModel.GetResource(productInfo.NowSimulation.ResourceCode);
            */
            Resource resource = productInfo.Resource;
            if (resource == null)
            {
                BaseModelFacade dataModel = new BaseModelFacade(this.DataProvider);
                resource = (Resource)dataModel.GetResource(productInfo.NowSimulation.ResourceCode);
            }
            // Changed end

            simulationReport.SegmentCode = resource.SegmentCode;

            // Changed by Icyer 2005/10/31
            /*
            ShiftModelFacade shiftModel	= new ShiftModelFacade(this.DataProvider);
            TimePeriod  period				= (TimePeriod)shiftModel.GetTimePeriod(resource.ShiftTypeCode, simulationReport.MaintainTime);		
            if (period==null)
            {
                throw new Exception("$OutOfPerid");
            }
            */
            TimePeriod period = productInfo.TimePeriod;
            if (period == null)
            {
                ShiftModelFacade shiftModel = new ShiftModelFacade(this.DataProvider);
                period = (TimePeriod)shiftModel.GetTimePeriod(resource.ShiftTypeCode, simulationReport.MaintainTime);
                if (period == null)
                {
                    throw new Exception("$OutOfPerid");
                }
                productInfo.TimePeriod = period;
            }

            // Modified by Jane Shu		Date:2005-07-26

            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;
            if (productInfo.WorkDateTime == null)
            {
                dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
                productInfo.WorkDateTime = dbDateTime;
            }
            else
            {
                dbDateTime = productInfo.WorkDateTime;
            }

            DateTime dtWorkDateTime = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);
            //			actionEventArgs.ProductInfo.NowSimulation.MaintainDate =	dbDateTime.DBDate ;
            //			actionEventArgs.ProductInfo.NowSimulation.MaintainTime =	dbDateTime.DBTime ;

            if (period.IsOverDate == FormatHelper.TRUE_STRING)
            {
                if (period.TimePeriodBeginTime < period.TimePeriodEndTime)
                {
                    simulationReport.ShiftDay = FormatHelper.TODateInt(dtWorkDateTime.AddDays(-1));//Laws Lu,2006/11/13 uniform system collect date
                }
                else if (productInfo.NowSimulation.MaintainTime < period.TimePeriodBeginTime)
                {
                    simulationReport.ShiftDay = FormatHelper.TODateInt(dtWorkDateTime.AddDays(-1));//Laws Lu,2006/11/13 uniform system collect date
                }
                else
                {
                    simulationReport.ShiftDay = simulationReport.MaintainDate;
                }
            }
            else
            {
                simulationReport.ShiftDay = FormatHelper.TODateInt(dtWorkDateTime);//Laws Lu,2006/11/13 uniform system collect date
            }
            simulationReport.ShiftTypeCode = resource.ShiftTypeCode;
            simulationReport.ShiftCode = period.ShiftCode;
            simulationReport.TimePeriodCode = period.TimePeriodCode;
            simulationReport.StepSequenceCode = resource.StepSequenceCode;
            simulationReport.MOSeq = productInfo.NowSimulation.MOSeq;   // Added by Icyer 2007/07/03
            return simulationReport;
        }

        //��дOnWip  ;��,ʱ��,����ģ��,�û���Ϣ ������ ProductInfo.NewSimulation
        public OnWIP FillOnWip(ProductInfo productInfo)
        {
            return FillOnWip(productInfo, null);
        }

        public OnWIP FillOnWip(ProductInfo productInfo, ActionEventArgs actionEventArgs)
        {
            OnWIP onwip = new OnWIP();
            onwip.Action = productInfo.NowSimulation.LastAction;
            onwip.ActionResult = productInfo.NowSimulation.ProductStatus;
            onwip.ItemCode = productInfo.NowSimulation.ItemCode;
            //Laws Lu,2006/11/13 MDate,MTime from DataBase
            DBDateTime dbDateTime = Web.Helper.FormatHelper.GetNowDBDateTime(DataProvider);
            onwip.MaintainDate = dbDateTime.DBDate;
            onwip.MaintainTime = dbDateTime.DBTime;
            onwip.MaintainUser = productInfo.NowSimulation.MaintainUser;
            onwip.MOCode = productInfo.NowSimulation.MOCode;
            onwip.ModelCode = productInfo.NowSimulation.ModelCode;
            onwip.NGTimes = productInfo.NowSimulation.NGTimes;
            onwip.OPCode = productInfo.NowSimulation.OPCode;

            if (actionEventArgs == null)
            {
                onwip.ResourceCode = productInfo.NowSimulation.ResourceCode;
            }
            else
            {
                onwip.ResourceCode = actionEventArgs.ResourceCode;
            }
            onwip.RouteCode = productInfo.NowSimulation.RouteCode;
            onwip.RunningCard = productInfo.NowSimulation.RunningCard;
            onwip.RunningCardSequence = productInfo.NowSimulation.RunningCardSequence;
            onwip.SegmentCode = productInfo.NowSimulationReport.SegmentCode;
            onwip.ShiftDay = productInfo.NowSimulationReport.ShiftDay;
            onwip.ShiftTypeCode = productInfo.NowSimulationReport.ShiftTypeCode;
            onwip.ShiftCode = productInfo.NowSimulationReport.ShiftCode;
            onwip.TimePeriodCode = productInfo.NowSimulationReport.TimePeriodCode;

            onwip.SourceCard = productInfo.NowSimulation.SourceCard;
            onwip.SourceCardSequence = productInfo.NowSimulation.SourceCardSequence;
            onwip.StepSequenceCode = productInfo.NowSimulationReport.StepSequenceCode;

            onwip.TranslateCard = productInfo.NowSimulation.TranslateCard;
            onwip.TranslateCardSequence = productInfo.NowSimulation.TranslateCardSequence;

            //added by jessie, 2006-5-29, ��ӳ���
            onwip.ShelfNO = productInfo.NowSimulation.ShelfNO;
            //Laws Lu,2006/07/05 support RMA
            onwip.RMABillCode = productInfo.NowSimulation.RMABillCode;
            onwip.EAttribute1 = productInfo.NowSimulation.EAttribute1;

            onwip.MOSeq = productInfo.NowSimulation.MOSeq;  // Added by Icyer 2007/07/02
            return onwip;
        }


        public Messages Action(ActionEventArgs actionEventArgs)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + ".Action." + actionEventArgs.ActionType);
            dataCollectDebug.WhenFunctionIn(messages);
            //IDomainDataProvider domainDataProvider = DomainDataProviderManager.DomainDataProvider();
            this.DataProvider.BeginTransaction();
            try
            {

                IAction dataCollectModule = new ActionFactory(this.DataProvider).CreateAction(actionEventArgs.ActionType);

                messages.AddMessages(dataCollectModule.Execute(actionEventArgs));
                if (messages.IsSuccess())
                    this.DataProvider.CommitTransaction();
                else
                    this.DataProvider.RollbackTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                messages.Add(new Message(e));
            }
            finally
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }

            dataCollectDebug.WhenFunctionOut(messages);
            return messages;
        }

        public Messages ActionWithTransaction(ActionEventArgs actionEventArgs)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + ".Action." + actionEventArgs.ActionType);
            dataCollectDebug.WhenFunctionIn(messages);
            //IDomainDataProvider domainDataProvider = DomainDataProviderManager.DomainDataProvider();
            //this.DataProvider.BeginTransaction();
            try
            {

                IAction dataCollectModule = new ActionFactory(this.DataProvider).CreateAction(actionEventArgs.ActionType);

                messages.AddMessages(dataCollectModule.Execute(actionEventArgs));
                //				if (messages.IsSuccess())
                //					this.DataProvider.CommitTransaction();
                //				else
                //					this.DataProvider.RollbackTransaction();				
            }
            catch (Exception e)
            {
                //this.DataProvider.RollbackTransaction();
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            return messages;
        }

        // Added by Icyer 2005/10/28
        // ��������ĺ���������һ������ActionCheckStatus
        public Messages Action(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + ".Action." + actionEventArgs.ActionType);
            dataCollectDebug.WhenFunctionIn(messages);
            this.DataProvider.BeginTransaction();
            try
            {

                IAction dataCollectModule = new ActionFactory(this.DataProvider).CreateAction(actionEventArgs.ActionType);

                // �����IActionWithStatus���ͣ���ִ�д�ActionCheckStatus�ķ���
                if (dataCollectModule is IActionWithStatus)
                {
                    messages.AddMessages(((IActionWithStatus)dataCollectModule).Execute(actionEventArgs, actionCheckStatus));
                }
                else
                {
                    messages.AddMessages(dataCollectModule.Execute(actionEventArgs));
                }
                if (messages.IsSuccess())
                    this.DataProvider.CommitTransaction();
                else
                    this.DataProvider.RollbackTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                messages.Add(new Message(e));
            }
            finally
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }

            dataCollectDebug.WhenFunctionOut(messages);
            return messages;
        }
        public Messages ActionWithTransaction(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + ".Action." + actionEventArgs.ActionType + "(WithCheck)");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                IAction dataCollectModule = new ActionFactory(this.DataProvider).CreateAction(actionEventArgs.ActionType);

                // �����IActionWithStatus���ͣ���ִ�д�ActionCheckStatus�ķ���
                if (dataCollectModule is IActionWithStatus)
                {
                    messages.AddMessages(((IActionWithStatus)dataCollectModule).Execute(actionEventArgs, actionCheckStatus));
                }
                else
                {
                    messages.AddMessages(dataCollectModule.Execute(actionEventArgs));
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

        //add by hiro 2008/07/23
        public Messages ActionWithTransaction(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus, object[] OPBOMDetail)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + ".Action." + actionEventArgs.ActionType + "(WithCheck)");
            dataCollectDebug.WhenFunctionIn(messages);
            try
            {
                IAction dataCollectModule = new ActionFactory(this.DataProvider).CreateAction(actionEventArgs.ActionType);

                // �����IActionWithStatus���ͣ���ִ�д�ActionCheckStatus�ķ���
                if (dataCollectModule is IActionWithStatus)
                {
                    messages.AddMessages(((IActionWithStatusNew)dataCollectModule).Execute(actionEventArgs, actionCheckStatus, OPBOMDetail));
                }
                else
                {
                    messages.AddMessages(dataCollectModule.Execute(actionEventArgs));
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


        public Messages CollectErrorInformation(IDomainDataProvider domainDataProvider, string actionType, ProductInfo product, object[] datas1, object[] datas2, string memo)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug("CollectErrorInformation");
            dataCollectDebug.WhenFunctionIn(messages);
            TSFacade tsFacade = new TSFacade(domainDataProvider);
            BenQGuru.eMES.Domain.TS.TS tS = new BenQGuru.eMES.Domain.TS.TS();
            TSErrorCode tsErrorCode = new TSErrorCode();
            try
            {
                //Laws Lu,2005/08/17,�޸�
                tS.CardType = CardType.CardType_Product; ;
                //End��Laws Lu
                tS.FormTime = product.NowSimulation.MaintainTime;
                tS.FromDate = product.NowSimulation.MaintainDate;
                //Laws Lu,2005/08/17,�޸�
                //tS.FromInputType = "ONWIP";
                tS.FromInputType = TSFacade.TSSource_OnWIP;
                //End Laws Lu
                //Laws Lu,2005/08/23,�޸� ֻ�ǡ�����ʱ�䡱��������Ա������Ϣû����д��
                tS.FromUser = product.NowSimulation.MaintainUser;
                //End Laws Lu
                tS.FromMemo = memo;
                //Laws Lu,2006/07/05 add support RMA
                tS.RMABillCode = product.NowSimulation.RMABillCode;
                tS.FromOPCode = product.NowSimulation.OPCode;
                tS.FromResourceCode = product.NowSimulation.ResourceCode;
                tS.FromRouteCode = product.NowSimulation.RouteCode;
                tS.FromSegmentCode = product.NowSimulationReport.SegmentCode;
                tS.FromShiftCode = product.NowSimulationReport.ShiftCode;/*Laws Lu,2006/03/11 ���������д����*/
                tS.FromShiftDay = product.NowSimulationReport.ShiftDay;
                tS.FromShiftTypeCode = product.NowSimulationReport.ShiftTypeCode;
                tS.FromStepSequenceCode = product.NowSimulationReport.StepSequenceCode;
                tS.FromTimePeriodCode = product.NowSimulationReport.TimePeriodCode;
                tS.FromUser = product.NowSimulation.MaintainUser;
                tS.ItemCode = product.NowSimulation.ItemCode;
                tS.MaintainDate = product.NowSimulation.MaintainDate;
                tS.MaintainTime = product.NowSimulation.MaintainTime;
                tS.MaintainUser = product.NowSimulation.MaintainUser;
                tS.MOCode = product.NowSimulation.MOCode;
                tS.ModelCode = product.NowSimulation.ModelCode;
                tS.RunningCard = product.NowSimulation.RunningCard;
                tS.RunningCardSequence = product.NowSimulation.RunningCardSequence;
                tS.SourceCard = product.NowSimulation.SourceCard;
                tS.SourceCardSequence = product.NowSimulation.SourceCardSequence;

                tS.FromOutLineRouteCode = product.LastSimulation.RouteCode;
                //Laws Lu��2005/08/17���޸�
                //tS.TransactionStatus = "NO";
                tS.TransactionStatus = TSFacade.TransactionStatus_None;
                //End Laws Lu
                tS.TranslateCard = product.NowSimulation.TranslateCard;
                tS.TranslateCardSequence = product.NowSimulation.TranslateCardSequence;
                //Laws Lu,2005/08/24,�޸�
                //tS.TSId= tS.RunningCard+tS.RunningCardSequence.ToString();
                tS.TSId = FormatHelper.GetUniqueID(product.NowSimulation.MOCode
                    , tS.RunningCard, tS.RunningCardSequence.ToString());
                //End Laws Lu
                //Laws Lu,2005/08/17,����	TS��״̬
                //Laws Lu,2005/08/17,����	TS״̬
                tS.TSStatus = TSStatus.TSStatus_New;
                //Laws Lu,2006/03/11,����	�������޵����ں��·�
                tS.Week = (new ReportHelper(DataProvider)).WeekOfYear(tS.FromShiftDay.ToString());
                tS.Month = int.Parse(tS.FromShiftDay.ToString().Substring(4, 2));

                //End Laws Lu
                tS.TSTimes = product.NowSimulation.NGTimes;
                tS.MOSeq = product.NowSimulation.MOSeq;     // Added by Icyer 2007/07/03



                tsFacade.AddTS(tS);
                //int i=0;
                //if (actionType == ActionType.DataCollectAction_OutLineNG)
                //i=1;
                if (datas1 != null)
                {
                    for (int i = 0; i < datas1.Length; i++)
                    {
                        tsErrorCode.ErrorCode = ((ErrorCodeGroup2ErrorCode)datas1[i]).ErrorCode;
                        tsErrorCode.ErrorCodeGroup = ((ErrorCodeGroup2ErrorCode)datas1[i]).ErrorCodeGroup;
                        tsErrorCode.ItemCode = tS.ItemCode;
                        tsErrorCode.MaintainDate = tS.MaintainDate;
                        tsErrorCode.MaintainTime = tS.MaintainTime;
                        tsErrorCode.MaintainUser = tS.MaintainUser;
                        tsErrorCode.MOCode = tS.MOCode;
                        tsErrorCode.ModelCode = tS.ModelCode;
                        tsErrorCode.RunningCard = tS.RunningCard;
                        tsErrorCode.RunningCardSequence = tS.RunningCardSequence;
                        tsErrorCode.TSId = tS.TSId;
                        tsErrorCode.MOSeq = tS.MOSeq;   // Added by Icyer 2007/7/03
                        tsFacade.AddTSErrorCode(tsErrorCode);
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

        public Messages CollectErrorInfor(IDomainDataProvider domainDataProvider, string actionType, ProductInfo product, object[] errorinfo, string memo)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug("CollectErrorInfor");
            dataCollectDebug.WhenFunctionIn(messages);
            TSFacade tsFacade = new TSFacade(domainDataProvider);
            BenQGuru.eMES.Domain.TS.TS tS = new BenQGuru.eMES.Domain.TS.TS();
            TSErrorCode tsErrorCode = new TSErrorCode();
            TSErrorCode2Location tsErrorCode2Location = new TSErrorCode2Location();
            try
            {
                //Laws Lu,2005/08/17,�޸�
                //tS.CardType = "��Ʒ";
                tS.CardType = CardType.CardType_Product; ;
                //End��Laws Lu
                tS.FormTime = product.NowSimulation.MaintainTime;
                tS.FromDate = product.NowSimulation.MaintainDate;
                //Laws Lu,2005/08/17,�޸�
                //tS.FromInputType = "ONWIP";
                tS.FromInputType = TSFacade.TSSource_OnWIP;
                //End Laws Lu
                //Laws Lu,2005/08/23,�޸� ֻ�ǡ�����ʱ�䡱��������Ա������Ϣû����д��
                tS.FromUser = product.NowSimulation.MaintainUser;
                //End Laws Lu
                tS.FromMemo = memo;
                //Laws Lu,2006/07/05 add support RMA
                tS.RMABillCode = product.NowSimulation.RMABillCode;
                tS.FromOPCode = product.NowSimulation.OPCode;
                tS.FromResourceCode = product.NowSimulation.ResourceCode;
                tS.FromRouteCode = product.NowSimulation.RouteCode;
                tS.FromSegmentCode = product.NowSimulationReport.SegmentCode;
                tS.FromShiftCode = product.NowSimulationReport.ShiftCode; ;/*Laws Lu,2006/03/11 ���������д����*/
                tS.FromShiftDay = product.NowSimulationReport.ShiftDay;
                tS.FromShiftTypeCode = product.NowSimulationReport.ShiftTypeCode;
                tS.FromStepSequenceCode = product.NowSimulationReport.StepSequenceCode;
                tS.FromTimePeriodCode = product.NowSimulationReport.TimePeriodCode;
                tS.FromUser = product.NowSimulation.MaintainUser;
                tS.ItemCode = product.NowSimulation.ItemCode;
                tS.MaintainDate = product.NowSimulation.MaintainDate;
                tS.MaintainTime = product.NowSimulation.MaintainTime;
                tS.MaintainUser = product.NowSimulation.MaintainUser;
                tS.MOCode = product.NowSimulation.MOCode;
                tS.ModelCode = product.NowSimulation.ModelCode;
                tS.RunningCard = product.NowSimulation.RunningCard;
                tS.RunningCardSequence = product.NowSimulation.RunningCardSequence;
                tS.SourceCard = product.NowSimulation.SourceCard;
                tS.SourceCardSequence = product.NowSimulation.SourceCardSequence;
                //Laws Lu��2005/08/17���޸�
                //tS.TransactionStatus = "NO";
                tS.TransactionStatus = TSFacade.TransactionStatus_None;
                //End Laws Lu
                tS.TranslateCard = product.NowSimulation.TranslateCard;
                tS.TranslateCardSequence = product.NowSimulation.TranslateCardSequence;
                //Laws Lu,2005/08/24,�޸�
                //tS.TSId= tS.RunningCard+tS.RunningCardSequence.ToString();
                tS.TSId = FormatHelper.GetUniqueID(product.NowSimulation.MOCode
                    , tS.RunningCard, tS.RunningCardSequence.ToString());
                //End Laws Lu
                //Laws Lu,2005/08/17,����	TS��״̬
                tS.TSStatus = TSStatus.TSStatus_New;
                //Laws Lu,2006/03/11,����	�������޵����ں��·�
                tS.Week = (new ReportHelper(DataProvider)).WeekOfYear(tS.FromShiftDay.ToString());
                tS.Month = int.Parse(tS.FromShiftDay.ToString().Substring(4, 2));

                //End Laws Lu
                tS.TSTimes = product.NowSimulation.NGTimes;
                //Laws Lu,2006/07/06 support RMA
                tS.RMABillCode = product.NowSimulation.RMABillCode;
                //modified by jessie lee ,when the status is new ,there is not tsuser
                //tS.TSUser = product.NowSimulation.MaintainUser;
                tS.MOSeq = product.NowSimulation.MOSeq;     // Added by Icyer 2007/07/03
                tsFacade.AddTS(tS);

                if (errorinfo != null)
                {
                    for (int i = 0; i < errorinfo.Length; i++)
                    {
                        int j = tsFacade.QueryTSErrorCodeCount(((TSErrorCode2Location)errorinfo[i]).ErrorCodeGroup,
                            ((TSErrorCode2Location)errorinfo[i]).ErrorCode, tS.TSId);
                        if (j == 0)
                        {
                            tsErrorCode.ErrorCode = ((TSErrorCode2Location)errorinfo[i]).ErrorCode;
                            tsErrorCode.ErrorCodeGroup = ((TSErrorCode2Location)errorinfo[i]).ErrorCodeGroup;
                            tsErrorCode.ItemCode = tS.ItemCode;
                            tsErrorCode.MaintainDate = tS.MaintainDate;
                            tsErrorCode.MaintainTime = tS.MaintainTime;
                            tsErrorCode.MaintainUser = tS.MaintainUser;
                            tsErrorCode.MOCode = tS.MOCode;
                            tsErrorCode.ModelCode = tS.ModelCode;
                            tsErrorCode.RunningCard = tS.RunningCard;
                            tsErrorCode.RunningCardSequence = tS.RunningCardSequence;
                            tsErrorCode.TSId = tS.TSId;
                            tsErrorCode.MOSeq = tS.MOSeq;   // Added by Icyer 2007/07/03
                            tsFacade.AddTSErrorCode(tsErrorCode);
                        }
                        if (((TSErrorCode2Location)errorinfo[i]).ErrorLocation.Trim() != string.Empty)
                        {
                            tsErrorCode2Location.AB = ((TSErrorCode2Location)errorinfo[i]).AB;
                            tsErrorCode2Location.ErrorLocation = ((TSErrorCode2Location)errorinfo[i]).ErrorLocation;
                            //Laws Lu,2005/09/09,�޸�	bug
                            tsErrorCode2Location.ErrorCode = ((TSErrorCode2Location)errorinfo[i]).ErrorCode;
                            tsErrorCode2Location.ErrorCodeGroup = ((TSErrorCode2Location)errorinfo[i]).ErrorCodeGroup;

                            tsErrorCode2Location.ItemCode = tsErrorCode.ItemCode;
                            tsErrorCode2Location.MaintainDate = tsErrorCode.MaintainDate;
                            tsErrorCode2Location.MaintainTime = tsErrorCode.MaintainTime;
                            tsErrorCode2Location.MaintainUser = tsErrorCode.MaintainUser;
                            tsErrorCode2Location.MEMO = "";
                            tsErrorCode2Location.MOCode = tsErrorCode.MOCode;
                            tsErrorCode2Location.ModelCode = tsErrorCode.ModelCode;
                            tsErrorCode2Location.RunningCard = tsErrorCode.RunningCard;
                            tsErrorCode2Location.RunningCardSequence = tsErrorCode.RunningCardSequence;
                            if (tsErrorCode2Location.ErrorLocation.IndexOf(".") < 0)
                                tsErrorCode2Location.SubErrorLocation = tsErrorCode2Location.ErrorLocation;
                            else
                                tsErrorCode2Location.SubErrorLocation = tsErrorCode2Location.ErrorLocation.Substring(
                                    0, tsErrorCode2Location.ErrorLocation.IndexOf("."));
                            tsErrorCode2Location.TSId = tsErrorCode.TSId;

                            tsErrorCode2Location.ShiftDay = product.NowSimulationReport.ShiftDay;
                            tsErrorCode2Location.MOSeq = tsErrorCode.MOSeq;     // Added by Icyer 2007/07/03
                            tsFacade.AddTSErrorCode2Location(tsErrorCode2Location);
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

        public void CollectErrorInfor(Simulation sim, SimulationReport simReport, string LOTNO)
        {
            TSFacade tsFacade = new TSFacade(_domainDataProvider);
            BenQGuru.eMES.Domain.TS.TS tS = new BenQGuru.eMES.Domain.TS.TS();
            TSErrorCode tsErrorCode = new TSErrorCode();
            //TSErrorCode2Location tsErrorCode2Location = new TSErrorCode2Location();

            tS.CardType = CardType.CardType_Product; ;
            tS.FormTime = sim.MaintainTime;
            tS.FromDate = sim.MaintainDate;
            tS.FromInputType = TSFacade.TSSource_OnWIP;
            tS.FromUser = sim.MaintainUser;
            //tS.FromMemo = memo;
            tS.FromOPCode = sim.OPCode;
            tS.FromResourceCode = sim.ResourceCode;
            tS.FromRouteCode = sim.RouteCode;
            tS.FromSegmentCode = simReport.SegmentCode;
            tS.FromShiftCode = simReport.ShiftCode; ;/*Laws Lu,2006/03/11 ���������д����*/
            tS.FromShiftDay = simReport.ShiftDay;
            tS.FromShiftTypeCode = simReport.ShiftTypeCode;
            tS.FromStepSequenceCode = simReport.StepSequenceCode;
            tS.FromTimePeriodCode = simReport.TimePeriodCode;
            tS.FromUser = sim.MaintainUser;
            tS.ItemCode = sim.ItemCode;
            tS.MaintainDate = sim.MaintainDate;
            tS.MaintainTime = sim.MaintainTime;
            tS.MaintainUser = sim.MaintainUser;
            tS.MOCode = sim.MOCode;
            tS.ModelCode = sim.ModelCode;
            tS.RunningCard = sim.RunningCard;
            tS.RunningCardSequence = sim.RunningCardSequence;
            tS.SourceCard = sim.SourceCard;
            tS.SourceCardSequence = sim.SourceCardSequence;
            tS.TransactionStatus = TSFacade.TransactionStatus_None;
            tS.TranslateCard = sim.TranslateCard;
            tS.TranslateCardSequence = sim.TranslateCardSequence;
            tS.TSId = FormatHelper.GetUniqueID(sim.MOCode
                , tS.RunningCard, tS.RunningCardSequence.ToString());
            tS.TSStatus = TSStatus.TSStatus_New;
            tS.TSTimes = sim.NGTimes;
            //modified by jessie lee ,when the status is new ,there is not tsuser
            //tS.TSUser = sim.MaintainUser;
            tS.MOSeq = sim.MOSeq;       // Added by Icyer 2007/07/03
            tsFacade.AddTS(tS);

            object[] errorinfo = this.DataProvider.CustomQuery(typeof(Domain.OQC.OQCLotCard2ErrorCode), new SQLParamCondition(
                string.Format("select * from tbloqclotcard2errorcode where rcard=$RCARD and rcardseq=$RCARDSEQ and lotno=$LOTNO"),
                new SQLParameter[]
				{
					new SQLParameter("RCARD",typeof(string),sim.RunningCard),
					new SQLParameter("RCARDSEQ",typeof(string),sim.RunningCardSequence),
					new SQLParameter("LOTNO",typeof(string),LOTNO)
				}));
            if (errorinfo != null)
            {
                for (int i = 0; i < errorinfo.Length; i++)
                {
                    int j = tsFacade.QueryTSErrorCodeCount(((Domain.OQC.OQCLotCard2ErrorCode)errorinfo[i]).ErrorCodeGroup,
                        ((Domain.OQC.OQCLotCard2ErrorCode)errorinfo[i]).ErrorCode, tS.TSId);
                    if (j == 0)
                    {
                        tsErrorCode.ErrorCode = ((Domain.OQC.OQCLotCard2ErrorCode)errorinfo[i]).ErrorCode;
                        tsErrorCode.ErrorCodeGroup = ((Domain.OQC.OQCLotCard2ErrorCode)errorinfo[i]).ErrorCodeGroup;
                        tsErrorCode.ItemCode = tS.ItemCode;
                        tsErrorCode.MaintainDate = tS.MaintainDate;
                        tsErrorCode.MaintainTime = tS.MaintainTime;
                        tsErrorCode.MaintainUser = tS.MaintainUser;
                        tsErrorCode.MOCode = tS.MOCode;
                        tsErrorCode.ModelCode = tS.ModelCode;
                        tsErrorCode.RunningCard = tS.RunningCard;
                        tsErrorCode.RunningCardSequence = tS.RunningCardSequence;
                        tsErrorCode.TSId = tS.TSId;
                        tsErrorCode.MOSeq = tS.MOSeq;   // Added by Icyer 2007/07/03
                        tsFacade.AddTSErrorCode(tsErrorCode);
                    }

                }
            }
        }


        /// <summary>
        /// added by jessie lee
        /// </summary>
        /// <param name="rcard"></param>
        /// <returns></returns>
        public bool CheckBelongToLot(string rcard)
        {
            string sql = string.Format(
@"select count(lotno)
  from tbllot
 where lotstatus not in ('{0}', '{1}')
   and lotno in (select lotno from tbllot2card where rcard = '{2}')", OQCLotStatus.OQCLotStatus_Pass, OQCLotStatus.OQCLotStatus_Reject, rcard.ToUpper());
            int count = this.DataProvider.GetCount(new SQLCondition(sql));

            if (count > 0) return true;
            return false;

        }
    }

    public class DataCollectDebug
    {
        //		private string FunctionName;
        //		private DateTime d;
        public DataCollectDebug(string functionName)
        {
            //FunctionName=functionName;
        }
        public void WhenFunctionIn(Messages message)
        {
            //d=DateTime.Now;
            //message.Add(new Message(MessageType.Debug,FunctionName+" IN"));

        }
        public void WhenFunctionOut(Messages message)
        {
            //message.Add(new Message(MessageType.Debug,FunctionName+" OUT"));
            //TimeSpan t= DateTime.Now.Subtract(d);
            //message.Add(new Message(MessageType.Performance,FunctionName+":"+t.TotalMilliseconds.ToString(),new object[]{t.TotalMilliseconds})); 
        }
        public void DebugPoint(Messages message, string point)
        {
            //message.Add(new Message(MessageType.Debug,FunctionName+" "+point));
        }
    }
}
