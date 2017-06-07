using System;
using System.Collections;
using UserControl;
using BenQGuru.eMES.Common.DCT.Core;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.TSModel;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.DataCollect.Action;

namespace BenQGuru.eMES.Common.DCT.Action
{
	/// <summary>
	/// ActionCollectSMTNG ��ժҪ˵����
	/// </summary>
	public class ActionCollectSMTNG:BaseDCTAction
	{
		private Hashtable errorCodesHT = null;

        //û��Error Location�����������ָ��
        public static string NoLoc = "NOLOC";

        public ActionCollectSMTNG()
		{
            this.InitMessage = (new ActionHelper()).GetActionDesc(this);
            this.OutMesssage = new Message(MessageType.Normal, "$DCT_NG_Please_Input_NG_SN");
            this.LastPrompMesssage = new Message(MessageType.Normal, "$DCT_NG_Please_Input_NG_SN");
		}

		private ProductInfo currentProductInfo = null;
		private MO moWillGo = null;
		public override Messages PreAction(object act)
		{
			DataCollect.Action.ActionEventArgs args;
			if(currentProductInfo == null)
			{
                bool finCancel = false;
				bool bCancel = false;
				Messages msgChk = new Messages();
				if (act.ToString().ToUpper().Trim() == BaseDCTDriver.FINERROR && this.NextAction == null)
				{
                    finCancel = true;
				}
				else
				{
					args = new BenQGuru.eMES.DataCollect.Action.ActionEventArgs();
					args.RunningCard = act.ToString().ToUpper().Trim();
					msgChk = this.CheckProduct(args.RunningCard, act);
					if (msgChk.IsSuccess() == false)
					{
						msgChk.Add(new UserControl.Message(MessageType.Normal,"$DCT_NG_Please_Input_NG_SN"));
						bCancel = true;
					}
					else
					{
						this.ObjectState = args;
					}
				}
                if (finCancel == true)
                {
                    base.Action(act);
                    return msgChk;
                }
				if (bCancel == true)
				{
                    this.Status = ActionStatus.PrepareData;
                    this.FlowDirect = FlowDirect.WaitingInput;
					return msgChk;
				}
			}
			
			base.PreAction (act);

			Messages msg = new Messages();

            if(_currInputStep.StepType == InputStepType.ErrorCode)
			    msg.Add(new UserControl.Message(MessageType.Normal,"$DCT_PLEASE_INPUT_ErrorCode"));
			
			return  msg;
		}

        
		public override Messages Action(object act)
		{
			Messages msg = new Messages();
			BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider = null;

			if(act == null)
			{
				return msg;
			}

            IDCTClient client = act as IDCTClient;

			if (currentProductInfo == null)
				return msg;
			
			DataCollect.Action.ActionEventArgs args;
			if(ObjectState == null)
			{
				args = new BenQGuru.eMES.DataCollect.Action.ActionEventArgs();
			}
			else
			{
				args = ObjectState as DataCollect.Action.ActionEventArgs;
			}

			string data = act.ToString().ToUpper().Trim();//Errorcode
			//Laws Lu,2006/06/03	���	��ȡ��������
			if((act as IDCTClient).DBConnection != null)
			{
				domainProvider = (act as IDCTClient).DBConnection as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
			}
			else
			{
				domainProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider() 
					as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
				(act as IDCTClient).DBConnection = domainProvider;
			}


			if( string.Compare(data,BaseDCTDriver.FINERROR,true) != 0 )//�����ı�־
			{
				msg = CheckData(data,domainProvider);

				if(msg.IsSuccess())
				{
                    if (_currInputStep.StepType == InputStepType.ErrorCode)  //һ��������ErrorCode �� Error Location���������
                    {
                        if (errorCodesHT == null)
                        {
                            errorCodesHT = new Hashtable();
                        }

                        string key = _currInputStep.ErrorCode + "&" + _currInputStep.ErrorLoc;

                        bool bExist = false;
                        if (!errorCodesHT.ContainsKey(key))
                        {
                            errorCodesHT.Add(key, key);
                        }
                        else
                        {
                            bExist = true;
                        }

                                   
                        if (bExist == false)
                        {
                            //msg.Add(new UserControl.Message(MessageType.Succes,data));	// Removed by Icyer 2007/01/09	�����ظ����
                        }
                        else
                        {
                            msg.Add(new UserControl.Message(MessageType.Success, "$ErrorCodeCollected"));
                        }
                        this.Status = ActionStatus.PrepareData; 
                        return msg;
                    }
                    else
                    {                        
                        this.Status = ActionStatus.PrepareData;
                        return msg;
                    }
				}
				else	
				{					
					this.Status = ActionStatus.PrepareData;
					return msg;
				}
			}

			if(msg.IsSuccess())
			{
				//������к�
				
				ActionOnLineHelper _helper = new ActionOnLineHelper(domainProvider); 
				
				msg =  _helper.GetIDInfo( args.RunningCard ) ;

				if( msg.IsSuccess() )
				{
					ProductInfo product= (ProductInfo)msg.GetData().Values[0]; 
					
					TSModelFacade tsmodelFacade = new TSModelFacade( domainProvider );

					if(errorCodesHT == null)
					{
						msg.Add(new UserControl.Message(UserControl.MessageType.Error,"$ErrorCode_Not_Exist"));
					}

					if(msg.IsSuccess())
                    {
                        #region �õ��û������ Error Code��Error Location

                        BenQGuru.eMES.Domain.TS.TSErrorCode2Location[] errorInfo = new BenQGuru.eMES.Domain.TS.TSErrorCode2Location[errorCodesHT.Count];
                        TSModel.TSModelFacade tsFacade = new TSModelFacade(domainProvider);
                        int i = 0;
                        foreach (DictionaryEntry dic in errorCodesHT)
                        {
                            string[] keys = dic.Key.ToString().Split('&');
                            if (keys.Length > 1)
                            {
                                //û��Location
                                if (keys[1].Contains(NoLoc))
                                {
                                    object[] objs = tsFacade.GetErrorCodeGroupByErrorCodeCode(keys[0]);
                                    errorInfo[i] = new BenQGuru.eMES.Domain.TS.TSErrorCode2Location();
                                    if (objs != null && objs.Length > 0)
                                    {
                                        ErrorCodeGroupA errorgroup = (ErrorCodeGroupA)objs[0];
                                        errorInfo[i].ErrorCode = keys[0];
                                        errorInfo[i].ErrorCodeGroup = errorgroup.ErrorCodeGroup;
                                        errorInfo[i].ErrorLocation = string.Empty;
                                        errorInfo[i].AB = ItemLocationSide.ItemLocationSide_AB;
                                    }     
                                }
                                else //��location
                                {
                                    object[] objs = tsFacade.GetErrorCodeGroupByErrorCodeCode(keys[0]);
                                    errorInfo[i] = new BenQGuru.eMES.Domain.TS.TSErrorCode2Location();
                                    if (objs != null && objs.Length > 0)
                                    {
                                        ErrorCodeGroupA errorgroup = (ErrorCodeGroupA)objs[0];
                                        errorInfo[i].ErrorCode = keys[0];
                                        errorInfo[i].ErrorCodeGroup = errorgroup.ErrorCodeGroup;
                                        errorInfo[i].ErrorLocation = keys[1];
                                        errorInfo[i].AB = ItemLocationSide.ItemLocationSide_AB;
                                    }  
                                }
                            }
                            else //û��Location
                            {
                                object[] objs = tsFacade.GetErrorCodeGroupByErrorCodeCode(keys[0]);
                                errorInfo[i] = new BenQGuru.eMES.Domain.TS.TSErrorCode2Location();
                                if (objs != null && objs.Length > 0)
                                {
                                    ErrorCodeGroupA errorgroup = (ErrorCodeGroupA)objs[0];
                                    errorInfo[i].ErrorCode = keys[0];
                                    errorInfo[i].ErrorCodeGroup = errorgroup.ErrorCodeGroup;
                                    errorInfo[i].ErrorLocation = string.Empty;
                                    errorInfo[i].AB = ItemLocationSide.ItemLocationSide_AB;
                                } 
                            }

                            i++;
                        }

                        #endregion

                        /*
                        string[] errorcs = new string[errorCodesHT.Count];
						int i=0;
						foreach( DictionaryEntry dic in errorCodesHT )
						{
							errorcs[i] = dic.Key.ToString();
							i++;
						}
                         * */
						/*	Removed by Icyer 2007/03/15		�����Զ���������
						//Laws Lu,2006/06/22 modify fix the bug that system alert object not set an instance when the product.LastSimulation is null
						if(product.LastSimulation == null)
						{
							msg.Add(new UserControl.Message(UserControl.MessageType.Error,"$NoSimulation"));
						}
						*/
                        /*
						object[] errorcodes = null;
						if(msg.IsSuccess())
						{
							//errorcodes = tsmodelFacade.QueryECG2ECByECAndModelCode(errorcs, product.LastSimulation.ModelCode);
							string strModelCode = this.GetModelCodeFromProduct(product, this.moWillGo, domainProvider);
							if (strModelCode != "")
							{
								errorcodes = tsmodelFacade.QueryECG2ECByECAndModelCode(errorcs, strModelCode);
							}

							if( errorcodes==null || errorcodes.Length==0 )
							{
								msg.Add(new UserControl.Message(UserControl.MessageType.Error,"$ErrorCode_Not_BelongTo_ModelCode"));
								base.Action (act);
								ActionCollectSMTNG actionCollectNG = this;
								actionCollectNG.IsTopAction = false;
								this.NextAction = actionCollectNG;
								this.Status = ActionStatus.PrepareData;
								return msg;
							}
						}
					  */
						if(msg.IsSuccess())
						{
							IAction dataCollectModule 
								= new BenQGuru.eMES.DataCollect.Action.ActionFactory( domainProvider ).CreateAction(ActionType.DataCollectAction_NG);

							//((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)domainProvider).PersistBroker.OpenConnection();
							domainProvider.BeginTransaction();
							try
							{
								//IDCTClient client = act as IDCTClient;
                                ActionCheckStatus actionCheckStatus = new ActionCheckStatus();

								msg.AddMessages( ((IActionWithStatus)dataCollectModule).Execute(
									new TSActionEventArgs(ActionType.DataCollectAction_SMTNG,
									args.RunningCard,
									client.LoginedUser,
									client.ResourceCode,
									product,
                                    errorInfo,
									""),actionCheckStatus));

								if (msg.IsSuccess())
								{
									domainProvider.CommitTransaction();
                                    msg.Add(new UserControl.Message(MessageType.Success, string.Format("$CS_NGSUCCESS")));
								
								}
								else
								{
									domainProvider.RollbackTransaction();
								}
							}
							catch(Exception ex)
							{
								domainProvider.RollbackTransaction();

								msg.Add(new UserControl.Message(ex));
							}
							finally
							{
							
								((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)domainProvider).PersistBroker.CloseConnection();
							}
                        }
                        
                    }
				}
			}

            //if((act as IDCTClient).CachedAction is ActionCollectSMTNG)
            //{
            //    if(this.Status == BenQGuru.eMES.Common.DCT.Core.ActionStatus.Working)
            //    {
            //        this.ObjectState = null;
            //        if(this.errorCodesHT != null)
            //        {
            //            this.errorCodesHT.Clear();
            //        }
            //    }
            //    this.Status = BenQGuru.eMES.Common.DCT.Core.ActionStatus.PrepareData;
            //    (act as IDCTClient).CachedAction = this;
            //}
            if (this.errorCodesHT == null)
            {
                msg.ClearMessages();
            }
            if (msg.IsSuccess() || this.errorCodesHT == null)
            {
                base.Action(act);
            }
            else
            {
                msg.Add(new UserControl.Message(MessageType.Normal, "$DCT_NG_Please_Input_NG_SN"));
                this.Status = ActionStatus.PrepareData;
                this.FlowDirect = FlowDirect.WaitingInput;
            }
			currentProductInfo = null;
			moWillGo = null;
            this.ObjectState = null;
            if (this.errorCodesHT != null)
            {
                this.errorCodesHT.Clear();
            }
			return msg;
		}


		public override Messages AftAction(object act)
		{
			base.AftAction (act);

			return null;
		}

		#region Check Data
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data">ErrorCode</param>
		/// <returns></returns>
		public Messages CheckData(string data,Common.DomainDataProvider.SQLDomainDataProvider domainProvider)
		{
			Messages msg = new Messages();
			if ( data == string.Empty )
			{
				msg.Add(new UserControl.Message(UserControl.MessageType.Error,"$DCT_PLEASE_INPUT_ErrorCode"));
			}
			else
			{
                if (this._currInputStep.StepType == InputStepType.ErrorCode)
                {
                    TSModelFacade tsmodelFacade = new TSModelFacade(domainProvider);
                    object obj = tsmodelFacade.GetErrorCode(data);

                    if (obj == null)
                    {
                        msg.Add(new UserControl.Message(UserControl.MessageType.Error, "$ErrorCode_Not_Exist"));
                        return msg;
                    }

                    /* ����Ҫ��鲻�����������ڲ�Ʒ��
                    if (currentProductInfo != null)
                    {
                        //object[] errorcodes = tsmodelFacade.QueryECG2ECByECAndModelCode(new string[]{data}, currentProductInfo.LastSimulation.ModelCode);
                        object[] errorcodes = null;
                        string strModelCode = GetModelCodeFromProduct(currentProductInfo, this.moWillGo, domainProvider);
                        if (strModelCode != "")
                        {
                            errorcodes = tsmodelFacade.QueryECG2ECByECAndModelCode(new string[] { data }, strModelCode);
                        }

                        if (errorcodes == null || errorcodes.Length == 0)
                        {
                            msg.Add(new UserControl.Message(UserControl.MessageType.Error, "$ErrorCode_Not_BelongTo_ModelCode"));
                            return msg;
                        }
                    }
                    */
                    this._currInputStep.ErrorCode = data;
                    this._currInputStep.StepType = InputStepType.ErrorLoc;
                    msg.Add(new UserControl.Message(UserControl.MessageType.Normal, "$CS_Please_Input_Location"));
                }
                else
                {
                    this._currInputStep.ErrorLoc = data;
                    this._currInputStep.StepType = InputStepType.ErrorCode;
                    //msg.Add(new UserControl.Message(UserControl.MessageType.Normal, "$DCT_PLEASE_INPUT_ErrorCode"));
                }
			}

			return msg;
		}

		public Messages CheckProduct(string rcard, object act)
		{
			currentProductInfo = null;
			moWillGo = null;
			Messages msg = new Messages();
			BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider = null;
			if((act as IDCTClient).DBConnection != null)
			{
				domainProvider = (act as IDCTClient).DBConnection as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
			}
			else
			{
				domainProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider() 
					as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
				(act as IDCTClient).DBConnection = domainProvider;
			}
			ActionOnLineHelper _helper = new ActionOnLineHelper(domainProvider); 
			msg = _helper.GetIDInfo( rcard) ;
			if( msg.IsSuccess() )
			{
				IDCTClient client = (IDCTClient)act;
				ProductInfo product= (ProductInfo)msg.GetData().Values[0]; 
				bool bNeedCheckMO = false;
				if (product == null || product.LastSimulation == null)
				{
					/*	��Ҫ�ټ���Ƿ��������
					msg.Add(new UserControl.Message(UserControl.MessageType.Error,"$NoSimulation"));
					return msg;
					*/
					bNeedCheckMO = true;
				}
				else
				{
					// ����깤�����ҵ�ǰ��Դ������Simulation�ĵ�ǰ��������Ҫ����������
					if (product.LastSimulation.IsComplete == "1")
					{
						BenQGuru.eMES.BaseSetting.BaseModelFacade dataModel = new BenQGuru.eMES.BaseSetting.BaseModelFacade(domainProvider);
						if (dataModel.GetOperation2Resource(product.LastSimulation.OPCode, client.ResourceCode) == null)
						{
							bNeedCheckMO = true;
						}
					}
				}
				Messages msgChkErr = new Messages();
				if (bNeedCheckMO == true)
				{
					ActionGoToMO actionGoMO = new ActionGoToMO(domainProvider);
					Messages msgMo = actionGoMO.GetItemCodeFromGoMoRCard(client.ResourceCode, rcard);
					if (msgMo.IsSuccess() == false)		// ����д��󣬱�ʾ��Ҫ�������������ǽ����������ѯ������������
					{
						msgChkErr.AddMessages(msgMo);
					}
					else	// ���سɹ����������������Ҫ�����������ҷ�����ȷ�Ĺ�����Ϣ������Ҫ��������
					{
						UserControl.Message msgMoData = msgMo.GetData();
						if (msgMoData != null && msgMoData.Values.Length > 0)		// ��DATA���ݣ���ʾ��Ҫ��������
						{
							this.moWillGo = (MO)msgMoData.Values[0];
						}
						else		// ���û��DATA���ݣ���ʾ����Ҫ�����������������ǰ�Ĵ��룺���LastSimulationΪ�գ���û�����к�
						{
							if (product.LastSimulation == null )
							{
								msgChkErr.Add(new UserControl.Message(UserControl.MessageType.Error,"$NoSimulation"));
							}
						}
					}
				}
				if (msgChkErr.IsSuccess() == false)
				{
					return msgChkErr;
				}
				
				if (product.LastSimulation != null)		// ֻ�������кŴ��ڵ�����²ż��;��
				{
					msg = _helper.CheckID(new TSActionEventArgs(ActionType.DataCollectAction_NG,
						rcard,
						client.LoginedUser,
						client.ResourceCode,
						product,
						new object[]{},
						null,
						""));
				}
				if (product.LastSimulation == null || msg.IsSuccess() == true)
				{
					currentProductInfo = product;
				}
				else if (product.LastSimulation.LastAction == ActionType.DataCollectAction_GOOD ||
						product.LastSimulation.LastAction == ActionType.DataCollectAction_NG ||
						product.LastSimulation.LastAction == ActionType.DataCollectAction_OutLineGood ||
						product.LastSimulation.LastAction == ActionType.DataCollectAction_OutLineNG)	// �������վ�ظ��ɼ�
				{
					BenQGuru.eMES.BaseSetting.BaseModelFacade dataModel = new BenQGuru.eMES.BaseSetting.BaseModelFacade(domainProvider);
					if (dataModel.GetOperation2Resource(product.LastSimulation.OPCode, (act as IDCTClient).ResourceCode) != null)
					{
						msg.ClearMessages();
						currentProductInfo = product;
					}
					
				}
			}
			return msg;
		}

		/// <summary>
		/// ��ѯ��Ʒ����룺���mo��Ϊnull����ͨ�������Ų�ѯ��Ʒ�𣬷���ͨ��product�е����кŲ�ѯ
		/// </summary>
		/// <returns></returns>
		private string GetModelCodeFromProduct(ProductInfo product, MO mo, BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider)
		{
			string strModelCode = string.Empty;
			if (mo != null)
			{
				BenQGuru.eMES.MOModel.ModelFacade modelFacade = new ModelFacade(domainProvider);
				object[] objsModel = modelFacade.GetModel2ItemByItemCode(mo.ItemCode);
				if (objsModel != null && objsModel.Length > 0)
				{
					Model2Item modelItem = (Model2Item)objsModel[0];
					strModelCode = modelItem.ModelCode;
				}
			}
			else if (product.LastSimulation != null)
				strModelCode = product.LastSimulation.ModelCode;
			return strModelCode;
		}
		#endregion

        #region �������̿���
        class InputStep
        {
            public string ErrorCode = string.Empty;
            public string ErrorLoc = string.Empty;
            public string StepType = InputStepType.ErrorCode;
        }

        private InputStep _currInputStep = new InputStep();

        class InputStepType
        {
            public static string ErrorCode = "ErrorCode";
            public static string ErrorLoc = "ErrorLoc";
        }
        #endregion
    }
}
