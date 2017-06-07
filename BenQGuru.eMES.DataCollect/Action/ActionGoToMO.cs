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
using BenQGuru.eMES.Domain.Package;
using BenQGuru.eMES.Domain.SMT;
using BenQGuru.eMES.SMT;

namespace BenQGuru.eMES.DataCollect.Action
{
    /// <summary>
    /// ���������ɼ�
    /// ���⣺��վͬʱͶ��ͬһ����ʱ���ᷢ����ͻ
    /// </summary>
    public class ActionGoToMO : IActionWithStatus
    {

        private IDomainDataProvider _domainDataProvider = null;

        //		public ActionGoToMO()
        //		{	
        //		}

        public ActionGoToMO(IDomainDataProvider domainDataProvider)
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
        #region ��鹤�������Ƿ�ΪRMA�������������������кŵõ����δ�᰸��RMA����
        public string GetRMABillCode(string rcard)
        {
            RMAFacade _RMAFacade = new RMAFacade(this.DataProvider);
            object objRMADetial = _RMAFacade.GetRMADetailByRCard(rcard);
            if (objRMADetial != null)
            {
                return (objRMADetial as Domain.RMA.RMADetial).Rmabillcode;
            }
            return "";
        }
        #endregion

        public Messages CheckIn(ActionEventArgs actionEventArgs)
        {
            ((GoToMOActionEventArgs)actionEventArgs).PassCheck = true;

            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "GetIDInfo");
            dataCollectDebug.WhenFunctionIn(messages);

            try
            {
                DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
                MOFacade moFacade = new MOFacade(this.DataProvider);
                BaseModelFacade dataModel = new BaseModelFacade(this.DataProvider);
                ModelFacade mf = new ModelFacade(this.DataProvider);
                SystemSettingFacade systemFacade = new SystemSettingFacade(this.DataProvider);

                //AMOI  MARK  START  20050803 ���Ĭ��Ϊ��ʼ��FOR �ع����к��ظ�
                actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence = ActionOnLineHelper.StartSeq;
                //AMOI  MARK  END

                if (((GoToMOActionEventArgs)actionEventArgs).MOCode == null || ((GoToMOActionEventArgs)actionEventArgs).MOCode.Trim() == string.Empty)
                {
                    throw new Exception("$CS_Sys_GoToMO_Lost_MOParam");
                }


                #region ���;��
                MO mo = null;
                if (actionEventArgs.CurrentMO != null)
                {
                    mo = actionEventArgs.CurrentMO;
                }
                else
                {
                    mo = (MO)moFacade.GetMO(((GoToMOActionEventArgs)actionEventArgs).MOCode);
                    actionEventArgs.CurrentMO = mo;
                }
                if (mo == null)
                {
                    throw new Exception("$CS_MO_Not_Exit");
                }

                MO2Route route = (MO2Route)moFacade.GetMONormalRouteByMOCode(mo.MOCode);
                if (route == null)
                {
                    throw new Exception("$CS_MOnotNormalRoute");
                }

                ItemRoute2OP op = dataCollectFacade.GetMORouteFirstOP(mo.MOCode, route.RouteCode);

                if (dataModel.GetOperation2Resource(op.OPCode, actionEventArgs.ResourceCode) == null)
                {
                    throw new Exception("$CS_Route_Failed_FirstOP $Domain_MO =" + mo.MOCode);
                }
                #endregion

                #region ��鹤��

                //����״̬���
                if (!dataCollectFacade.CheckMO(mo))
                {
                    throw new Exception("$CS_MO_Status_Error $CS_Param_MOStatus=$" + mo.MOStatus);
                }

                /*1.Simulation û�м�¼
                 *	    a. MO Running Card û���ظ�		OK
                 *     b. MO Running Card ���ظ�		Exception
                 * 
                 * 2. Simulation �м�¼
                 *		a.  Simulation ���� �� ��ǰ���� һ��			OK
                 *		b.  Simulation ���� �� ��ǰ���� ��һ�� 
                 *			 b1.  ��ǰ���� ���ع�����				
                 *					b11.  MO Running Card û���ظ�		OK
                 *					b12.  MO Running Card ���ظ�			Exception
                 *			 b2.  ��ǰ���������ع�����						Exception
                 *					
                 */

                Parameter parameter = (Parameter)systemFacade.GetParameter(mo.MOType, BenQGuru.eMES.Web.Helper.MOType.GroupType);

                if (parameter == null)
                {
                    throw new Exception("$CS_MOType_Lost");
                }
                mo.MOType = parameter.ParameterValue;

                // add by andy.xin 
                string rmaBillCode = "";
                if (string.Compare(parameter.ParameterValue, BenQGuru.eMES.Web.Helper.MOType.MOTYPE_RMAREWORKMOTYPE, true) == 0)
                {
                    rmaBillCode = this.GetRMABillCode(actionEventArgs.RunningCard);
                }



                object objOffCard = null;
                //Laws Lu��2005/12/16������	������������������Ĳ�Ʒ���кŲ�����ɼ�
                if (actionEventArgs.ProductInfo.LastSimulation != null)
                {
                    /*	Removed by Icyer 2006/12/27 @ YHI
                    objOffCard = moFacade.GetOffMoCardByRcard(actionEventArgs.ProductInfo.LastSimulation.RunningCard);
                    if(objOffCard != null)
                    {
                        OffMoCard offCard = objOffCard as OffMoCard;

                        if(offCard.MoType == "NORMAL")
                        {
                            throw new Exception("$CS_RCRAD_ALREADY_OFF_MO  $Domain_MO=" + mo.MOCode);
                        }
                    }
                    */

                }

                //Write off by Laws Lu on Darfon Requirement,2006/05/24,submitted by Johnson
                //				if ( mo.MOType == BenQGuru.eMES.Web.Helper.MOType.MOTYPE_NORMALMOTYPE )
                //				{	
                //
                //					#region ��ͨ�������
                //					
                ////					// ������кŸ�ʽ
                ////					if ( ((GoToMOActionEventArgs)actionEventArgs).RunningCard.Length < 7 )
                ////					{
                ////						throw new Exception("$CS_RunningCard_Format_Error");
                ////					}
                ////
                ////					// ��Ʒ���кŵĵڶ����빤���ĵ�һλ��ͬ
                ////					if ( ((GoToMOActionEventArgs)actionEventArgs).RunningCard[1] != ((GoToMOActionEventArgs)actionEventArgs).MOCode[0] )
                ////					{
                ////						throw new Exception("$CS_RunningCard_Format_Error");
                ////					}
                ////
                ////					// ��Ʒ���кŵ�3��7���빤�������5λ��ͬ
                ////					if ( ((GoToMOActionEventArgs)actionEventArgs).RunningCard.Substring(2, 5) != ((GoToMOActionEventArgs)actionEventArgs).MOCode.Substring( ((GoToMOActionEventArgs)actionEventArgs).MOCode.Length-5 < 0 ? 0 : ((GoToMOActionEventArgs)actionEventArgs).MOCode.Length-5 ) )
                ////					{
                ////						throw new Exception("$CS_RunningCard_Format_Error");
                ////					}
                //					
                //
                //					// ID�͹������	Added by Jane Shu	Date:2005/06/03
                //					MORunningCardFacade cardFacade = new MORunningCardFacade(dataCollectFacade.DataProvider);
                //				
                //					if ( actionEventArgs.ProductInfo.LastSimulation == null )	// Simulation��û�м�¼
                //					{		
                //						// �ӹ���ID��Χ���飬��ID�Ƿ�ʹ�ù�
                //						if ( cardFacade.IsRunningCardUsed(actionEventArgs.RunningCard) )
                //						{	
                //							throw new Exception("$CS_ID_Has_One_MO $CS_Param_ID =" + actionEventArgs.RunningCard);
                //						}
                //					}
                //					else														// Simulation���м�¼
                //					{
                //						// Simulation�еĹ�����Ҫ�����Ĺ�����ͬ
                //						// ��������������к�������ݿ����
                //						if ( actionEventArgs.ProductInfo.LastSimulation.MOCode.ToUpper() == mo.MOCode.ToUpper() )
                //						{
                //							((GoToMOActionEventArgs)actionEventArgs).PassCheck = false;
                //							return messages;
                //						}
                //						else
                //						{
                //    							//�ӹ���ID��Χ���飬��ID�Ƿ�ʹ�ù�
                //								if ( cardFacade.IsRunningCardUsed(actionEventArgs.RunningCard))
                //								{
                //									throw new Exception("$CS_ID_Has_One_MO $CS_Param_ID =" + actionEventArgs.RunningCard);
                //								}
                //
                //						}
                //					}
                //					#endregion
                //				}
                //				else
                //				{
                //					//�ж�ID����
                ////					if (actionEventArgs.RunningCard.Length <11)
                ////						throw new Exception("$CS_ID_Length<11");
                ////					if (actionEventArgs.RunningCard.Length >15)
                ////						throw new Exception("$CS_ID_Length>15");
                //End Write off	

                if (actionEventArgs.ProductInfo.LastSimulation != null)
                {
                    //�����Ƿ�һ��
                    //						if (mo.MOCode == actionEventArgs.ProductInfo.LastSimulation.MOCode )
                    //							throw new Exception("$CS_ItemCodeNotSame $CS_Param_ItemCode = "+mo.ItemCode
                    //								+" $CS_Param_ItemCode = "+actionEventArgs.ProductInfo.LastSimulation.ItemCode );
                    //���������͹���������ͬҲ������
                    if ((mo.MOCode == actionEventArgs.ProductInfo.LastSimulation.MOCode)/*	&&
							(actionEventArgs.ProductInfo.LastSimulation.OPCode ==op.OPCode )*/
                                                                                          )
                    {
                        ((GoToMOActionEventArgs)actionEventArgs).PassCheck = false;
                        return messages;
                    }
                    //Laws Lu,2005/10/20,�޸�	Lucky������	CS112
                    //���鷵�����������ɼ�ʱ�����ж�������Ҳ����˵ֻ��û�����Ƽ�¼�Ļ����Ѿ����Ĳ�Ʒ���кŲ��ܹ�����������
                    if (actionEventArgs.ProductInfo.LastSimulation.IsComplete == "0")
                    {
                        throw new Exception("$CS_PRODUCT_STILL_INLINE_NOT_BELONG_MO ,$CS_Param_ID=" + actionEventArgs.RunningCard);
                    }

                    // Added By Hi1/Venus.Feng on 20080809 for Hisense : Add Item Check for complete rcard
                    //if (string.Compare(mo.ItemCode, actionEventArgs.ProductInfo.LastSimulation.ItemCode, true) != 0)
                    //{
                    //    throw new Exception("$Error_RCardItemCodeNotSameWithMO");
                    //}
                    // End Added
                }
                //Laws Lu,2006/08/01,
                /*Ŀǰϵͳ��֧����ͬ��Ʒ���кſ��Զ�ι�����ͬ�����������ǻ���ԭ�����¿����·���������IMEI�����õ�ҵ������Ŀǰ�﷽û����������
������һҵ�����ƣ�Ŀǰ���������߼�����Ҫ��Onwip����Ѱ����RcardSeqence����������Ӱ��ϵͳ���ܡ�
���޸Ĺ��������߼���ȡ����ͬ��Ʒ���кſ��Զ�ι�����ͬ����������
Ϊ�˷�ֹ����������³�����������������ڹ��������߼��н����ϸ��飬��������ȷ��ʾ��Ϣ*/
                //�Ƿ������ʷ������Ϣ
                //					OnWIP onwip = dataCollectFacade.CheckIDIsUsed(actionEventArgs.RunningCard, mo.MOCode);
                //					if (onwip == null)
                //					{									
                //					}
                //					else
                //					{	
                //Laws Lu��2006/07/10
                OnWIPCardTransfer trans = dataCollectFacade.CheckIDIsSNTransfered(actionEventArgs.RunningCard, mo.MOCode);
                if (trans != null)
                //						if(actionEventArgs.ProductInfo.LastSimulation == null || 
                //							(actionEventArgs.ProductInfo.LastSimulation != null 
                //							&& actionEventArgs.ProductInfo.LastSimulation.IsComplete != "1"))
                {
                    /*	Removed by Icyer 2006/12/27 @ YHI
                    throw new Exception("$CS_RCRAD_ALREADY_OFF_MO  $Domain_MO=" + mo.MOCode);
                    */
                }
                //						else
                //						{
                //							actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence = onwip.RunningCardSequence + 10; 
                //						}
                //					}	
                //}

                // Ͷ������� 
                if (mo.IsControlInput == "1")	// �ͻ��ڹ����й�ѡ�ˡ�����Ͷ���������鹤����Ͷ���������򲻼��
                {
                    if (mo.MOPlanQty - mo.MOInputQty + mo.MOScrapQty + mo.MOOffQty - mo.IDMergeRule < 0)
                    {
                        throw new Exception("$CS_MOInputOut $Domain_MO =" + mo.MOCode);
                    }
                }
                #endregion

                #region ���ⷿ״̬
                if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"] != null
                    && System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                {
                    Material.WarehouseFacade wFAC = new BenQGuru.eMES.Material.WarehouseFacade(DataProvider);
                    //����Դ��Ӧ�Ĳ���
                    BenQGuru.eMES.BaseSetting.BaseModelFacade facade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider);
                    object objResource = facade.GetResource(actionEventArgs.ResourceCode);

                    string strSSCode = ((BenQGuru.eMES.Domain.BaseSetting.Resource)objResource).StepSequenceCode;
                    object obj = wFAC.GetWarehouseByMoSS(mo.MOCode, strSSCode);
                    if (obj != null)
                    {
                        Domain.Warehouse.Warehouse wh = obj as Domain.Warehouse.Warehouse;
                        //Laws Lu��2006/02/20���޸�/���蹤�δ���
                        string strStatus = wFAC.GetWarehouseStatus(wh.WarehouseCode, wh.FactoryCode);
                        if (strStatus == Domain.Warehouse.Warehouse.WarehouseStatus_Cycle)
                        {
                            throw new Exception("$CS_LINE_IS_HOLD");
                        }
                    }
                }
                #endregion

                #region ����Ƿ����Զ�����lot��OPά��
                // Added By Hi1/Venus.Feng on 20080730 for Hisense
                string itemCode = mo.ItemCode;
                ItemFacade itemFacade = new ItemFacade(this.DataProvider);
                object item = itemFacade.GetItem(itemCode, mo.OrganizationID);
                if (item == null)
                {
                    throw new Exception("$Error_ItemCode_NotExist $Domain_ItemCode=" + itemCode);
                }
                //if (((Item)item).CheckItemOP == null || ((Item)item).CheckItemOP.Trim().Length == 0)
                //{
                //    throw new Exception("$Error_NoItemGenerateLotOPCode $Domain_ItemCode=" + itemCode);
                //}
                // End Added
                #endregion

                #region ��д��SIMULATION
                //messages.AddMessages( dataCollectFacade.WriteSimulation(id,actionType,resourceCode,userCode,product));
                actionEventArgs.ProductInfo.NowSimulation.RouteCode = route.RouteCode;
                actionEventArgs.ProductInfo.NowSimulation.OPCode = op.OPCode;

                actionEventArgs.ProductInfo.NowSimulation.LastAction = ActionType.DataCollectAction_GoMO;
                actionEventArgs.ProductInfo.NowSimulation.ActionList = ";" + ActionType.DataCollectAction_GoMO + ";";
                actionEventArgs.ProductInfo.NowSimulation.RunningCard = actionEventArgs.RunningCard;
                //AMOI  MARK  START  20050803 �����ǰ���Ѿ�����
                //actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence =	ActionOnLineHelper.StartSeq;
                //AMOI  MARK  END
                actionEventArgs.ProductInfo.NowSimulation.TranslateCard = actionEventArgs.RunningCard;
                actionEventArgs.ProductInfo.NowSimulation.TranslateCardSequence = ActionOnLineHelper.StartSeq;
                actionEventArgs.ProductInfo.NowSimulation.SourceCard = actionEventArgs.RunningCard;
                actionEventArgs.ProductInfo.NowSimulation.SourceCardSequence = ActionOnLineHelper.StartSeq;
                actionEventArgs.ProductInfo.NowSimulation.MOCode = mo.MOCode;
                actionEventArgs.ProductInfo.NowSimulation.ItemCode = mo.ItemCode;
                Model model = mf.GetModelByItemCode(mo.ItemCode);
                if (model == null)
                {
                    throw new Exception("$CS_Model_Lost $CS_Param_ItemCode=" + mo.ItemCode);
                }
                actionEventArgs.ProductInfo.NowSimulation.ModelCode = model.ModelCode;
                //����Ƿ���ڴ�X�����
                SMTFacade smtfacade = new SMTFacade(this.DataProvider);
                object relation = smtfacade.GetSMTRelationQty(actionEventArgs.RunningCard, mo.MOCode);
                if (relation != null)
                {
                    actionEventArgs.ProductInfo.NowSimulation.IDMergeRule = ((Smtrelationqty)relation).Relationqtry;
                }
                else
                {
                    actionEventArgs.ProductInfo.NowSimulation.IDMergeRule = mo.IDMergeRule;
                }
                actionEventArgs.ProductInfo.NowSimulation.IsComplete = ProductComplete.NoComplete;
                actionEventArgs.ProductInfo.NowSimulation.ResourceCode = actionEventArgs.ResourceCode;
                actionEventArgs.ProductInfo.NowSimulation.ProductStatus = ProductStatus.GOOD;
                actionEventArgs.ProductInfo.NowSimulation.FromOP = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.FromRoute = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.CartonCode = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.LOTNO = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.PalletCode = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.NGTimes = ActionOnLineHelper.StartNGTimes;
                actionEventArgs.ProductInfo.NowSimulation.ProductStatus = ProductStatus.GOOD;
                actionEventArgs.ProductInfo.NowSimulation.IsHold = 0;
                actionEventArgs.ProductInfo.NowSimulation.MOSeq = mo.MOSeq;     // Added by Icyer 2007/07/03
                //update by andy.xin rmaBillCode
                actionEventArgs.ProductInfo.NowSimulation.RMABillCode = rmaBillCode; //mo.RMABillCode;

                #endregion
            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }

            dataCollectDebug.WhenFunctionOut(messages);
            return messages;
        }

        // Added by Icyer 2005/10/28
        public Messages CheckIn(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
        {
            ((GoToMOActionEventArgs)actionEventArgs).PassCheck = true;

            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "GetIDInfo(WithCheck)");
            dataCollectDebug.WhenFunctionIn(messages);

            try
            {
                DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
                MOFacade moFacade = new MOFacade(this.DataProvider);
                BaseModelFacade dataModel = new BaseModelFacade(this.DataProvider);
                ModelFacade mf = new ModelFacade(this.DataProvider);
                SystemSettingFacade systemFacade = new SystemSettingFacade(this.DataProvider);

                //AMOI  MARK  START  20050803 ���Ĭ��Ϊ��ʼ��FOR �ع����к��ظ�
                actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence = ActionOnLineHelper.StartSeq;
                //AMOI  MARK  END

                if (((GoToMOActionEventArgs)actionEventArgs).MOCode == null || ((GoToMOActionEventArgs)actionEventArgs).MOCode.Trim() == string.Empty)
                {
                    throw new Exception("$CS_Sys_GoToMO_Lost_MOParam");
                }

                #region ���;��
                //���ActionCheckStatus��û�м���;�̣�����;�̣�����CheckedOP��ΪTrue
                MO mo = actionCheckStatus.MO;
                if (mo == null || mo.IsControlInput == "1")
                {
                    //					MO mo = null;
                    if (actionEventArgs.CurrentMO != null)
                    {
                        mo = actionEventArgs.CurrentMO;
                    }
                    else
                    {
                        mo = (MO)moFacade.GetMO(((GoToMOActionEventArgs)actionEventArgs).MOCode);
                        actionEventArgs.CurrentMO = mo;
                    }
                    //					mo = (MO)moFacade.GetMO(((GoToMOActionEventArgs)actionEventArgs).MOCode);
                    if (mo == null)
                    {
                        throw new Exception("$CS_MO_Not_Exit");
                    }
                    actionCheckStatus.MO = mo;
                }
                MO2Route route = actionCheckStatus.Route;
                if (route == null)
                {
                    route = (MO2Route)moFacade.GetMONormalRouteByMOCode(mo.MOCode);
                    if (route == null)
                    {
                        throw new Exception("$CS_MOnotNormalRoute");
                    }
                    actionCheckStatus.Route = route;
                }
                ItemRoute2OP op = actionCheckStatus.OP;
                if (op == null)
                {
                    op = dataCollectFacade.GetMORouteFirstOP(mo.MOCode, route.RouteCode);
                    actionCheckStatus.OP = op;
                }
                if (actionCheckStatus.CheckedOP == false)
                {
                    if (dataModel.GetOperation2Resource(op.OPCode, actionEventArgs.ResourceCode) == null)
                    {
                        throw new Exception("$CS_Route_Failed_FirstOP $Domain_MO =" + mo.MOCode);
                    }
                    actionCheckStatus.CheckedOP = true;
                }
                #endregion

                #region ��鹤��

                //����״̬���
                //���ActionCheckStatus��û�м�������״̬�����飬����CheckedMO��ΪTrue
                if (actionCheckStatus.CheckedMO == false)
                {
                    if (!dataCollectFacade.CheckMO(mo))
                    {
                        throw new Exception("$CS_MO_Status_Error $CS_Param_MOStatus=$" + mo.MOStatus);
                    }
                    actionCheckStatus.CheckedMO = true;
                }

                /*1.Simulation û�м�¼
                 *	    a. MO Running Card û���ظ�		OK
                 *     b. MO Running Card ���ظ�		Exception
                 * 
                 * 2. Simulation �м�¼
                 *		a.  Simulation ���� �� ��ǰ���� һ��			OK
                 *		b.  Simulation ���� �� ��ǰ���� ��һ�� 
                 *			 b1.  ��ǰ���� ���ع�����				
                 *					b11.  MO Running Card û���ظ�		OK
                 *					b12.  MO Running Card ���ظ�			Exception
                 *			 b2.  ��ǰ���������ع�����						Exception
                 *					
                 */

                //���ActionCheckStatus��û�м�������״̬�����飬����CheckedMO��ΪTrue
                string rmaBillCode = "";
                string strMOTypeParamValue = actionCheckStatus.MOTypeParamValue;
                if (strMOTypeParamValue == string.Empty)
                {
                    Parameter parameter = (Parameter)systemFacade.GetParameter(mo.MOType, BenQGuru.eMES.Web.Helper.MOType.GroupType);

                    if (parameter == null)
                    {
                        throw new Exception("$CS_MOType_Lost");
                    }
                    //mo.MOType = parameter.ParameterValue;
                    strMOTypeParamValue = parameter.ParameterValue;
                    actionCheckStatus.MOTypeParamValue = strMOTypeParamValue;

                    // add by andy.xin 
                    if (string.Compare(parameter.ParameterValue, BenQGuru.eMES.Web.Helper.MOType.MOTYPE_RMAREWORKMOTYPE, true) == 0)
                    {
                        rmaBillCode = this.GetRMABillCode(actionEventArgs.RunningCard);
                    }
                }
                else
                {
                    if (string.Compare(strMOTypeParamValue, BenQGuru.eMES.Web.Helper.MOType.MOTYPE_RMAREWORKMOTYPE, true) == 0)
                    {
                        rmaBillCode = this.GetRMABillCode(actionEventArgs.RunningCard);
                    }
                }



                object objOffCard = null;
                //Laws Lu��2005/12/16������	������������������Ĳ�Ʒ���кŲ�����ɼ�
                if (actionEventArgs.ProductInfo.LastSimulation != null)
                {
                    /*	Removed by Icyer 2006/12/27 @ YHI
                    objOffCard = moFacade.GetOffMoCardByRcard(actionEventArgs.ProductInfo.LastSimulation.RunningCard);
                    if(objOffCard != null)
                    {
                        OffMoCard offCard = objOffCard as OffMoCard;

                        if(offCard.MoType == "NORMAL")
                        {
                            throw new Exception("$CS_RCRAD_ALREADY_OFF_MO  $Domain_MO=" + mo.MOCode);
                        }
                    }
                    */

                }

                //Write off by Laws Lu on Darfon Requirement,2006/05/24,submitted by Johnson
                //if ( mo.MOType == BenQGuru.eMES.Web.Helper.MOType.MOTYPE_NORMALMOTYPE )
                //				if ( strMOTypeParamValue == BenQGuru.eMES.Web.Helper.MOType.MOTYPE_NORMALMOTYPE )
                //				{	
                //					#region ��ͨ�������
                //					
                ////					// ������кŸ�ʽ
                ////					if ( ((GoToMOActionEventArgs)actionEventArgs).RunningCard.Length < 7 )
                ////					{
                ////						throw new Exception("$CS_RunningCard_Format_Error");
                ////					}
                ////
                ////					// ��Ʒ���кŵĵڶ����빤���ĵ�һλ��ͬ
                ////					if ( ((GoToMOActionEventArgs)actionEventArgs).RunningCard[1] != ((GoToMOActionEventArgs)actionEventArgs).MOCode[0] )
                ////					{
                ////						throw new Exception("$CS_RunningCard_Format_Error");
                ////					}
                ////
                ////					// ��Ʒ���кŵ�3��7���빤�������5λ��ͬ
                ////					if ( ((GoToMOActionEventArgs)actionEventArgs).RunningCard.Substring(2, 5) != ((GoToMOActionEventArgs)actionEventArgs).MOCode.Substring( ((GoToMOActionEventArgs)actionEventArgs).MOCode.Length-5 < 0 ? 0 : ((GoToMOActionEventArgs)actionEventArgs).MOCode.Length-5 ) )
                ////					{
                ////						throw new Exception("$CS_RunningCard_Format_Error");
                ////					}
                //					
                //
                //					// ID�͹������	Added by Jane Shu	Date:2005/06/03
                //					MORunningCardFacade cardFacade = new MORunningCardFacade(dataCollectFacade.DataProvider);
                //				
                //					if ( actionEventArgs.ProductInfo.LastSimulation == null )	// Simulation��û�м�¼
                //					{		
                //						// �ӹ���ID��Χ���飬��ID�Ƿ�ʹ�ù�
                //						if ( cardFacade.IsRunningCardUsed(actionEventArgs.RunningCard) )
                //						{	
                //							throw new Exception("$CS_ID_Has_One_MO");
                //						}
                //					}
                //					else														// Simulation���м�¼
                //					{
                //						// Simulation�еĹ�����Ҫ�����Ĺ�����ͬ
                //						// ��������������к�������ݿ����
                //						if ( actionEventArgs.ProductInfo.LastSimulation.MOCode.ToUpper() == mo.MOCode.ToUpper() )
                //						{
                //							((GoToMOActionEventArgs)actionEventArgs).PassCheck = false;
                //							return messages;
                //						}
                //						else
                //						{
                //							//�ӹ���ID��Χ���飬��ID�Ƿ�ʹ�ù�
                //							if ( cardFacade.IsRunningCardUsed(actionEventArgs.RunningCard) )
                //							{
                //								throw new Exception("$CS_ID_Has_One_MO");
                //							}
                //
                //						}
                //					}
                //					#endregion
                //				}
                //				else
                //				{
                //�ж�ID����
                //					if (actionEventArgs.RunningCard.Length <11)
                //						throw new Exception("$CS_ID_Length<11");
                //					if (actionEventArgs.RunningCard.Length >15)
                //						throw new Exception("$CS_ID_Length>15");
                //End Write off
                if (actionEventArgs.ProductInfo.LastSimulation != null)
                {
                    //						//��Ʒ�Ƿ�һ��
                    //						if (mo.ItemCode !=actionEventArgs.ProductInfo.LastSimulation.ItemCode )
                    //							throw new Exception("$CS_ItemCodeNotSame $CS_Param_ItemCode= $"+mo.ItemCode
                    //								+" $CS_Param_ItemCode= $"+actionEventArgs.ProductInfo.LastSimulation.ItemCode );
                    //���������͹���������ͬҲ������
                    if ((mo.MOCode == actionEventArgs.ProductInfo.LastSimulation.MOCode)/*	&&
							(actionEventArgs.ProductInfo.LastSimulation.OPCode ==op.OPCode )*/
                                                                                          )
                    {
                        ((GoToMOActionEventArgs)actionEventArgs).PassCheck = false;
                        return messages;
                    }
                    //Laws Lu,2005/10/20,�޸�	Lucky������	CS112
                    //���鷵�����������ɼ�ʱ�����ж�������Ҳ����˵ֻ��û�����Ƽ�¼�Ļ����Ѿ����Ĳ�Ʒ���кŲ��ܹ�����������
                    if (actionEventArgs.ProductInfo.LastSimulation.IsComplete == "0")
                    {
                        throw new Exception("$CS_PRODUCT_STILL_INLINE_NOT_BELONG_MO ,$CS_Param_ID " + actionEventArgs.RunningCard);
                    }
                }
                //Laws Lu,2006/08/01,
                /*Ŀǰϵͳ��֧����ͬ��Ʒ���кſ��Զ�ι�����ͬ�����������ǻ���ԭ�����¿����·���������IMEI�����õ�ҵ������Ŀǰ�﷽û����������
������һҵ�����ƣ�Ŀǰ���������߼�����Ҫ��Onwip����Ѱ����RcardSeqence����������Ӱ��ϵͳ���ܡ�
���޸Ĺ��������߼���ȡ����ͬ��Ʒ���кſ��Զ�ι�����ͬ����������
Ϊ�˷�ֹ����������³�����������������ڹ��������߼��н����ϸ��飬��������ȷ��ʾ��Ϣ*/
                //					//�Ƿ������ʷ������Ϣ
                //					OnWIP onwip= dataCollectFacade.CheckIDIsUsed(actionEventArgs.RunningCard, mo.MOCode);
                //					if (onwip==null)
                //					{									
                //					}
                //					else
                //					{		
                //Laws Lu��2006/07/10
                OnWIPCardTransfer trans = dataCollectFacade.CheckIDIsSNTransfered(actionEventArgs.RunningCard, mo.MOCode);
                if (trans != null)
                //						if(actionEventArgs.ProductInfo.LastSimulation == null || 
                //							(actionEventArgs.ProductInfo.LastSimulation != null 
                //							&& actionEventArgs.ProductInfo.LastSimulation.IsComplete != "1"))
                {
                    /*	Removed by Icyer 2006/12/27 @ YHI
                    throw new Exception("$CS_RCRAD_ALREADY_OFF_MO  $Domain_MO=" + mo.MOCode);
                    */
                }
                //						else
                //						{
                //							actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence =onwip.RunningCardSequence + 10; 
                //						}
                //					}	
                //				}

                // Ͷ������� 
                if (mo.IsControlInput == "1")	// �ͻ��ڹ����й�ѡ�ˡ�����Ͷ���������鹤����Ͷ���������򲻼��
                {
                    if (mo.MOPlanQty - mo.MOInputQty + mo.MOScrapQty + mo.MOOffQty - mo.IDMergeRule < 0)
                    {
                        throw new Exception("$CS_MOInputOut");
                    }
                }
                #endregion

                #region ���ⷿ״̬
                if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"] != null
                    && System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                {
                    Material.WarehouseFacade wFAC = new BenQGuru.eMES.Material.WarehouseFacade(DataProvider);
                    //����Դ��Ӧ�Ĳ���
                    BenQGuru.eMES.BaseSetting.BaseModelFacade facade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider);
                    object objResource = facade.GetResource(actionEventArgs.ResourceCode);

                    string strSSCode = ((BenQGuru.eMES.Domain.BaseSetting.Resource)objResource).StepSequenceCode;

                    object obj = wFAC.GetWarehouseByMoSS(mo.MOCode, strSSCode);
                    if (obj != null)
                    {
                        Domain.Warehouse.Warehouse wh = obj as Domain.Warehouse.Warehouse;
                        string strStatus = wFAC.GetWarehouseStatus(wh.WarehouseCode, wh.FactoryCode);
                        //Laws Lu��2006/02/20���޸�/���蹤�δ���
                        if (strStatus == Domain.Warehouse.Warehouse.WarehouseStatus_Cycle)
                        {
                            throw new Exception("$CS_LINE_IS_HOLD");
                        }
                    }
                }
                #endregion

                #region ����Ƿ����Զ�����lot��OPά��
                // Added By Hi1/Venus.Feng on 20080730 for Hisense
                string itemCode = mo.ItemCode;
                ItemFacade itemFacade = new ItemFacade(this.DataProvider);
                object item = itemFacade.GetItem(itemCode, mo.OrganizationID);
                if (item == null)
                {
                    throw new Exception("$Error_ItemCode_NotExist $Domain_ItemCode=" + itemCode);
                }
                //if (((Item)item).CheckItemOP == null || ((Item)item).CheckItemOP.Trim().Length == 0)
                //{
                //    throw new Exception("$Error_NoItemGenerateLotOPCode $Domain_ItemCode=" + itemCode);
                //}
                // End Added
                #endregion

                #region ��д��SIMULATION
                //messages.AddMessages( dataCollectFacade.WriteSimulation(id,actionType,resourceCode,userCode,product));
                actionEventArgs.ProductInfo.NowSimulation.RouteCode = route.RouteCode;
                actionEventArgs.ProductInfo.NowSimulation.OPCode = op.OPCode;

                actionEventArgs.ProductInfo.NowSimulation.LastAction = ActionType.DataCollectAction_GoMO;
                actionEventArgs.ProductInfo.NowSimulation.ActionList = ";" + ActionType.DataCollectAction_GoMO + ";";
                actionEventArgs.ProductInfo.NowSimulation.RunningCard = actionEventArgs.RunningCard;
                //AMOI  MARK  START  20050803 �����ǰ���Ѿ�����
                //actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence =	ActionOnLineHelper.StartSeq;
                //AMOI  MARK  END
                actionEventArgs.ProductInfo.NowSimulation.TranslateCard = actionEventArgs.RunningCard;
                actionEventArgs.ProductInfo.NowSimulation.TranslateCardSequence = ActionOnLineHelper.StartSeq;
                actionEventArgs.ProductInfo.NowSimulation.SourceCard = actionEventArgs.RunningCard;
                actionEventArgs.ProductInfo.NowSimulation.SourceCardSequence = ActionOnLineHelper.StartSeq;
                actionEventArgs.ProductInfo.NowSimulation.MOCode = mo.MOCode;
                actionEventArgs.ProductInfo.NowSimulation.ItemCode = mo.ItemCode;

                //update by andy xin rmaBillCode
                actionEventArgs.ProductInfo.NowSimulation.RMABillCode = rmaBillCode;//mo.RMABillCode;
                // Changed by Icyer 2005/10/29
                /*
                Model model=mf.GetModelByItemCode( mo.ItemCode);
                if (model==null)
                {
                    throw new Exception("$CS_Model_Lost $CS_Param_ItemCode="+mo.ItemCode);
                }
                */
                Model model = actionCheckStatus.Model;
                if (model == null)
                {
                    model = mf.GetModelByItemCode(mo.ItemCode);
                    actionCheckStatus.Model = model;
                    if (model == null)
                    {
                        throw new Exception("$CS_Model_Lost $CS_Param_ItemCode=" + mo.ItemCode);
                    }
                }
                // Changed end
                actionEventArgs.ProductInfo.NowSimulation.ModelCode = model.ModelCode;
                //����Ƿ���ڴ�X�����
                SMTFacade smtfacade = new SMTFacade(this.DataProvider);
                object relation = smtfacade.GetSMTRelationQty(actionEventArgs.RunningCard, mo.MOCode);
                if (relation != null)
                {
                    actionEventArgs.ProductInfo.NowSimulation.IDMergeRule = ((Smtrelationqty)relation).Relationqtry;
                }
                else
                {
                    actionEventArgs.ProductInfo.NowSimulation.IDMergeRule = mo.IDMergeRule;
                }
                actionEventArgs.ProductInfo.NowSimulation.IsComplete = ProductComplete.NoComplete;
                actionEventArgs.ProductInfo.NowSimulation.ResourceCode = actionEventArgs.ResourceCode;
                actionEventArgs.ProductInfo.NowSimulation.ProductStatus = ProductStatus.GOOD;
                actionEventArgs.ProductInfo.NowSimulation.FromOP = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.FromRoute = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.CartonCode = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.LOTNO = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.PalletCode = ActionOnLineHelper.StringNull;
                actionEventArgs.ProductInfo.NowSimulation.NGTimes = ActionOnLineHelper.StartNGTimes;
                actionEventArgs.ProductInfo.NowSimulation.IsHold = 0;
                actionEventArgs.ProductInfo.NowSimulation.MOSeq = mo.MOSeq;     // Added by Icyer 2007/07/03
                #endregion
            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }

            dataCollectDebug.WhenFunctionOut(messages);
            return messages;
        }


        public Messages Execute(ActionEventArgs actionEventArgs)
        {
            ((GoToMOActionEventArgs)actionEventArgs).MOCode = ((GoToMOActionEventArgs)actionEventArgs).MOCode.Trim().ToUpper();
            actionEventArgs.RunningCard = actionEventArgs.RunningCard.Trim().ToUpper();

            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);

            if (((GoToMOActionEventArgs)actionEventArgs).MOCode.Trim() == String.Empty)
            {
                messages.Add(new UserControl.Message(MessageType.Error, "$CS_Please_Input_MOCode"));
            }

            if (((GoToMOActionEventArgs)actionEventArgs).RunningCard.Trim() == String.Empty)
            {
                messages.Add(new UserControl.Message(MessageType.Error, "$CS_Please_Input_RunningCard"));
            }
            //add by hiro.chen 08/11/18 checkISDown
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
            messages.AddMessages(dataCollectFacade.CheckISDown(((GoToMOActionEventArgs)actionEventArgs).RunningCard.Trim()));
            //end

            try
            {
                if (messages.IsSuccess())
                {
                    actionEventArgs.ProductInfo.NowSimulation.EAttribute1 = (actionEventArgs as GoToMOActionEventArgs).Memo;
                    //actionEventArgs.ProductInfo.NowSimulation.RMABillCode = (actionEventArgs as GoToMOActionEventArgs).ProductInfo.CurrentMO.MOCode;

                    messages.AddMessages(this.CheckIn(actionEventArgs));
                    // Added by Icyer 2006/12/05
                    TakeDownCarton(actionEventArgs);
                    // Added end

                    //Laws Lu,2006/07/05 add support RMA
                    if (actionEventArgs.CurrentMO != null)
                    {
                        actionEventArgs.ProductInfo.NowSimulation.RMABillCode = actionEventArgs.CurrentMO.RMABillCode;
                    }

                    if (!((GoToMOActionEventArgs)actionEventArgs).PassCheck)
                    {
                        throw new Exception("$CS_ID_Has_Already_Belong_To_This_MO $CS_Param_ID="
                            + actionEventArgs.RunningCard + " $Domain_MO=" + ((GoToMOActionEventArgs)actionEventArgs).MOCode);
                    }

                    if (messages.IsSuccess())
                    {
                        ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
                        //actionEventArgs.ProductInfo.NowSimulation.EAttribute1 = (actionEventArgs as GoToMOActionEventArgs).Memo;

                        // Added By Hi1/Venus Feng on 20081114 for Hisense Version : remove pallet and carton
                        // ����Ͳ�Pallet
                        DataCollectFacade dcf = new DataCollectFacade(this.DataProvider);

                        //messages.AddMessages(dcf.RemoveFromCarton(actionEventArgs.ProductInfo.NowSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode));
                       // if (messages.IsSuccess())
                       // {
                        //    actionEventArgs.ProductInfo.NowSimulation.CartonCode = "";
                        //    messages.ClearMessages();
                        //}
                        
                        if (messages.IsSuccess())
                        {
                            Package.PackageFacade packageFacade = new BenQGuru.eMES.Package.PackageFacade(DataProvider);

                            Pallet2RCard pallet2RCard = packageFacade.GetPallet2RCardByRCard(actionEventArgs.ProductInfo.NowSimulation.RunningCard.ToUpper()) as Pallet2RCard;
                            if (pallet2RCard != null)
                            {
                                Pallet pallet = packageFacade.GetPallet(pallet2RCard.PalletCode) as Pallet;
                                if (pallet != null)
                                {
                                    messages.AddMessages(dcf.RemoveFromPallet(actionEventArgs.ProductInfo.NowSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode, true));
                                    if (messages.IsSuccess())
                                    {
                                        actionEventArgs.ProductInfo.NowSimulation.PalletCode = "";
                                        messages.ClearMessages();
                                    }
                                }
                            }
                        }
                        // End Added

                        messages.AddMessages(dataCollect.Execute(actionEventArgs));

                        if (messages.IsSuccess())
                        {
                            //Laws Lu,2005/10/24,ע��
                            // Added by Jane Shu	Date:2005/06/02
                            //							if ( actionEventArgs.ProductInfo.NowSimulation != null )
                            //							{
                            //this.updateMOQty(actionEventArgs.ProductInfo.NowSimulation.MOCode, actionEventArgs.UserCode);
                            this.updateItem2Route(actionEventArgs.ProductInfo.NowSimulation.ItemCode, actionEventArgs.ProductInfo.NowSimulation.RouteCode, actionEventArgs.UserCode);
                            //							}

                            // ��ID��ӵ�MO��Χ����		Added by Jane Shu	Date:2005/06/03	
                            MORunningCardFacade cardFacade = new MORunningCardFacade(this.DataProvider);
                            MORunningCard card = cardFacade.CreateNewMORunningCard();

                            DBDateTime dbDateTime;
                            //Laws Lu,2006/11/13 uniform system collect date
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

                            card.MOCode = ((GoToMOActionEventArgs)actionEventArgs).MOCode.ToString();
                            card.MORunningCardStart = actionEventArgs.RunningCard;
                            card.MORunningCardEnd = actionEventArgs.RunningCard;
                            card.MaintainUser = actionEventArgs.UserCode;

                            card.MaintainDate = dbDateTime.DBDate;
                            card.MaintainTime = dbDateTime.DBTime;

                            card.EAttribute1 = (actionEventArgs as GoToMOActionEventArgs).Memo;
                            // Added by Icyer 2007/07/02
                            MOFacade moFacade = new MOFacade(this.DataProvider);
                            MO mo = (MO)moFacade.GetMO(card.MOCode);
                            card.MOSeq = mo.MOSeq;
                            // Added end

                            cardFacade.AddMORunningCard(card);


                            //ReportHelper reportCollect = new ReportHelper(this.DataProvider);
                            //messages.AddMessages(reportCollect.ReportLineQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo));
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

        // Added by Icyer 2005/10/28
        //��չһ����ActionCheckStatus�����ķ���
        public Messages Execute(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
        {
            ((GoToMOActionEventArgs)actionEventArgs).MOCode = ((GoToMOActionEventArgs)actionEventArgs).MOCode.Trim().ToUpper();
            actionEventArgs.ProductInfo.NowSimulation.EAttribute1 = (actionEventArgs as GoToMOActionEventArgs).Memo;
            //actionEventArgs.ProductInfo.NowSimulation.RMABillCode = (actionEventArgs as GoToMOActionEventArgs).ProductInfo.CurrentMO.mor;

            actionEventArgs.RunningCard = actionEventArgs.RunningCard.Trim().ToUpper();

            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);

            if (((GoToMOActionEventArgs)actionEventArgs).MOCode.Trim() == String.Empty)
            {
                messages.Add(new UserControl.Message(MessageType.Error, "$CS_Please_Input_MOCode"));
            }

            if (((GoToMOActionEventArgs)actionEventArgs).RunningCard.Trim() == String.Empty)
            {
                messages.Add(new UserControl.Message(MessageType.Error, "$CS_Please_Input_RunningCard"));
            }

            //add by hiro.chen 08/11/18 checkISDown
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
            messages.AddMessages(dataCollectFacade.CheckISDown(((GoToMOActionEventArgs)actionEventArgs).RunningCard.Trim()));
            //end

            try
            {
                if (messages.IsSuccess())
                {
                    //���ú���ActionCheckStatus������CheckIn����
                    messages.AddMessages(this.CheckIn(actionEventArgs, actionCheckStatus));
                    // Added by Icyer 2006/12/05
                    TakeDownCarton(actionEventArgs);
                    // Added end
                    //Laws Lu,2006/07/05 add support RMA
                    if (actionEventArgs.CurrentMO != null)
                    {
                        // Delete by andy RMA����д��
                        //actionEventArgs.ProductInfo.NowSimulation.RMABillCode = actionEventArgs.CurrentMO.RMABillCode;
                    }

                    if (!((GoToMOActionEventArgs)actionEventArgs).PassCheck)
                    {
                        throw new Exception("$CS_ID_Has_Already_Belong_To_This_MO $CS_Param_ID="
                            + actionEventArgs.RunningCard + " $Domain_MO=" + ((GoToMOActionEventArgs)actionEventArgs).MOCode);
                    }

                    if (messages.IsSuccess())
                    {
                        // ����Ͳ�Pallet
                        DataCollectFacade dcf = new DataCollectFacade(this.DataProvider);

                        if (messages.IsSuccess())
                        {
                            Package.PackageFacade packageFacade = new BenQGuru.eMES.Package.PackageFacade(DataProvider);

                            Pallet2RCard pallet2RCard = packageFacade.GetPallet2RCardByRCard(actionEventArgs.ProductInfo.NowSimulation.RunningCard.ToUpper()) as Pallet2RCard;
                            if (pallet2RCard != null)
                            {
                                Pallet pallet = packageFacade.GetPallet(pallet2RCard.PalletCode) as Pallet;
                                if (pallet != null)
                                {
                                    messages.AddMessages(dcf.RemoveFromPallet(actionEventArgs.ProductInfo.NowSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode, true));
                                    if (messages.IsSuccess())
                                    {
                                        actionEventArgs.ProductInfo.NowSimulation.PalletCode = "";
                                        messages.ClearMessages();
                                    }
                                }
                            }
                        }

                       // if (messages.IsSuccess())
                       // {
                       //     messages.AddMessages(dcf.RemoveFromCarton(actionEventArgs.ProductInfo.NowSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode));
                       //     if (messages.IsSuccess())
                       //    {
                       //        actionEventArgs.ProductInfo.NowSimulation.CartonCode = "";
                        //       messages.ClearMessages();
                        //   }

                       // }

                        actionEventArgs.ProductInfo.NowSimulation.EAttribute1 = (actionEventArgs as GoToMOActionEventArgs).Memo;

                        if (actionCheckStatus.NeedUpdateSimulation)
                        {
                            ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
                            messages.AddMessages(dataCollect.Execute(actionEventArgs));
                        }
                        else
                        {
                            ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
                            messages.AddMessages(dataCollect.Execute(actionEventArgs, actionCheckStatus));
                        }

                        if (messages.IsSuccess())
                        {
                            //this.updateMOQty(actionEventArgs.ProductInfo.NowSimulation.MOCode, actionEventArgs.UserCode, actionCheckStatus);
                            //�Ƿ�ִ�й�updateItem2Route����
                            if (actionCheckStatus.IsUpdateRefItem2Route == false)
                            {
                                this.updateItem2Route(actionEventArgs.ProductInfo.NowSimulation.ItemCode, actionEventArgs.ProductInfo.NowSimulation.RouteCode, actionEventArgs.UserCode);
                                actionCheckStatus.IsUpdateRefItem2Route = true;
                            }

                            MORunningCardFacade cardFacade = new MORunningCardFacade(this.DataProvider);
                            MORunningCard card = cardFacade.CreateNewMORunningCard();

                            card.MOCode = ((GoToMOActionEventArgs)actionEventArgs).MOCode.ToString();
                            card.MORunningCardStart = actionEventArgs.RunningCard;
                            card.MORunningCardEnd = actionEventArgs.RunningCard;
                            card.MaintainUser = actionEventArgs.UserCode;

                            DBDateTime dbDateTime;
                            //Laws Lu,2006/11/13 uniform system collect date
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

                            card.MaintainDate = dbDateTime.DBDate;
                            card.MaintainTime = dbDateTime.DBTime;
                            // Added by Icyer 2007/07/02
                            MOFacade moFacade = new MOFacade(this.DataProvider);
                            MO mo = (MO)moFacade.GetMO(card.MOCode);
                            card.MOSeq = mo.MOSeq;
                            // Added end

                            cardFacade.AddMORunningCard(card);


                            //if (messages.IsSuccess())
                            //{
                            //    if (actionCheckStatus.NeedFillReport)
                            //    {
                            //        ReportHelper reportCollect = new ReportHelper(this.DataProvider);
                            //        messages.AddMessages(reportCollect.ReportLineQuanMaster(this.DataProvider, actionEventArgs.ActionType, actionEventArgs.ProductInfo, actionCheckStatus));
                            //    }
                            //}

                            //��Action�����б�
                            actionCheckStatus.ActionList.Add(actionEventArgs);
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
        // Added end

        #region ��������
        //		/// <summary>
        //		/// ����MO��״̬������
        //		/// </summary>
        //		/// <param name="moCode"></param>
        //		/// <param name="userCode"></param>
        //		/// <param name="domainDataProvider"></param>
        //		private void updateMOQty(string moCode, string userCode,ActionEventArgs actionEventArgs)
        //		{
        //			updateMOQty(moCode, userCode,actionEventArgs, null);
        //		}
        //		// Added by Icyer 2005/10/28
        //		// ��չһ����ActionCheckStatus�����ķ���
        //		private void updateMOQty(string moCode, string userCode,ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
        //		{	
        //			MOFacade moFacade=new MOFacade(this.DataProvider);
        //
        //			// Changed by Icyer 2005/10/28
        //			//MO mo=(MO)moFacade.GetMO(moCode);
        //			MO mo = null;
        //			if (actionCheckStatus == null || actionCheckStatus.MO == null || 
        //				actionCheckStatus.MO.MOStatus == MOManufactureStatus.MOSTATUS_RELEASE )
        //			{
        ////				mo=(MO)moFacade.GetMO(moCode);
        //				if(actionEventArgs.CurrentMO != null)
        //				{
        //					mo = actionEventArgs.CurrentMO;
        //				}
        //				else
        //				{
        //					mo=(MO)moFacade.GetMO(moCode);
        //				}
        //
        //				if (actionCheckStatus != null)
        //				{
        //					actionCheckStatus.MO = mo;
        //				}
        //			}
        //			else
        //			{
        //				mo = actionCheckStatus.MO;
        //			}
        //			// Changed end
        //
        //			// ����״̬ΪRelease����дͶ������������״̬��Ϊopen����дʵ�ʿ���ʱ��
        //			if ( mo.MOStatus == MOManufactureStatus.MOSTATUS_RELEASE )
        //			{
        //				this.DataProvider.CustomExecute( new SQLParamCondition(
        //					string.Format("update tblmo set MOSTATUS='{0}', MOINPUTQTY=MOINPUTQTY+$IDMergeRule, MOACTSTARTDATE=$STARTDATE, MUSER=$MUSER, MDATE=$MDATE, MTIME=$MTIME where mocode=$mocode", MOManufactureStatus.MOSTATUS_OPEN),
        //					new SQLParameter[] {
        //										   new SQLParameter("IDMergeRule",	typeof(decimal),	mo.IDMergeRule ),
        //										   new SQLParameter("STARTDATE",	typeof(int),		FormatHelper.TODateInt(DateTime.Now)),
        //										   new SQLParameter("MUSER",		typeof(string),		userCode),
        //										   new SQLParameter("MDATE",		typeof(int),		FormatHelper.TODateInt(DateTime.Now)),
        //										   new SQLParameter("MTIME",		typeof(int),		FormatHelper.TOTimeInt(DateTime.Now)),
        //										   new SQLParameter("mocode",		typeof(string),		moCode)
        //									   }) );
        //			}
        //			else
        //			{
        //				this.DataProvider.CustomExecute( new SQLParamCondition(
        //					"update tblmo set MOINPUTQTY=MOINPUTQTY+$IDMergeRule, MUSER=$MUSER, MDATE=$MDATE, MTIME=$MTIME where mocode=$mocode",
        //					new SQLParameter[] {
        //										   new SQLParameter("IDMergeRule",	typeof(decimal),	mo.IDMergeRule ),
        //										   new SQLParameter("MUSER",		typeof(string),		userCode),
        //										   new SQLParameter("MDATE",		typeof(int),		FormatHelper.TODateInt(DateTime.Now)),
        //										   new SQLParameter("MTIME",		typeof(int),		FormatHelper.TOTimeInt(DateTime.Now)),
        //										   new SQLParameter("mocode",		typeof(string),		moCode)
        //									   }) );
        //			}
        //		}
        //		// Added end

        #endregion

        private void updateItem2Route(string itemCode, string routeCode, string userCode)
        {
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);


            this.DataProvider.CustomExecute(new SQLParamCondition("update tblitem2route set isref='1', MUSER=$MUSER, MDATE=$MDATE, MTIME=$MTIME where itemcode=$itemcode and routecode=$routecode and isref!='1'" + GlobalVariables.CurrentOrganizations.GetSQLCondition(),
                new SQLParameter[] {
									   new SQLParameter("MUSER", typeof(string), userCode),
									   new SQLParameter("MDATE", typeof(int), dbDateTime.DBDate),
									   new SQLParameter("MTIME", typeof(int), dbDateTime.DBTime),
									   new SQLParameter("itemcode", typeof(string), itemCode ),
									   new SQLParameter("routecode", typeof(string), routeCode )
								   }));
        }

        // Added by Icyer 2006/12/05
        //modified by crane.liu 20140228
        private void TakeDownCarton(ActionEventArgs actionEventArgs)
        {
            bool bDo = false;
            ProductInfo product = actionEventArgs.ProductInfo;
            if (System.Configuration.ConfigurationSettings.AppSettings["TakeDownCartonReMORework"] == "1" &&
                product != null &&
                product.NowSimulation != null
                //&&
                //product.LastSimulation != null &&
                //product.NowSimulation.MOCode != product.LastSimulation.MOCode 
                //&&
                //product.LastSimulation.CartonCode != string.Empty
                )
            {
                if (product.LastSimulation != null)
                {
                    if (product.NowSimulation.MOCode == product.LastSimulation.MOCode)
                    {
                        return;
                    }
                }
                MOFacade moFacade = new MOFacade(this.DataProvider);
                MO mo = (MO)moFacade.GetMO(product.NowSimulation.MOCode);
                if (mo == null)
                    return;
                BenQGuru.eMES.BaseSetting.SystemSettingFacade sysFacade = new SystemSettingFacade(this.DataProvider);
                Parameter param = (Parameter)sysFacade.GetParameter(mo.MOType, BenQGuru.eMES.Web.Helper.MOType.GroupType);
                if (param != null &&
                    param.ParameterValue == BenQGuru.eMES.Web.Helper.MOType.MOTYPE_REWORKMOTYPE)
                {

                    //-------Add by DS22 / Crane.Liu 2014-02-28 Start--------------
                    // Description:�ص���Ĳ��䣬�ص���û��simulation
                    DataCollectFacade dcf = new DataCollectFacade(this.DataProvider);
                    dcf.RemoveFromCarton(actionEventArgs.ProductInfo.NowSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode);

                    actionEventArgs.ProductInfo.NowSimulation.CartonCode = "";
                    if (product.LastSimulation != null)
                    {
                        product.LastSimulation.CartonCode = string.Empty;
                    }
                    //-------Add by DS22 / Crane.Liu 2014-02-28 End----------------
                    bDo = true;
                }
            }
            if (bDo == false)
                return;


            //BenQGuru.eMES.Package.PackageFacade packageFacade = new BenQGuru.eMES.Package.PackageFacade(this.DataProvider);

            //string strSql = "UPDATE tblSimulation SET CartonCode='' WHERE RCard='" + product.LastSimulation.RunningCard + "' AND MOCode='" + product.LastSimulation.MOCode + "' ";
            //this.DataProvider.CustomExecute(new SQLCondition(strSql));

            //strSql = "UPDATE tblSimulationReport SET CartonCode='' WHERE RCard='" + product.LastSimulation.RunningCard + "' AND MOCode='" + product.LastSimulation.MOCode + "' ";
            // this.DataProvider.CustomExecute(new SQLCondition(strSql));

            // #region add by andy xin  2012/04/20
            ////ɾ��Carton2RCARD
            //Carton2RCARD oldCarton2RCARD = (Carton2RCARD)packageFacade.GetCarton2RCARD(product.LastSimulation.CartonCode.Trim().ToUpper(), product.LastSimulation.RunningCard.Trim().ToUpper());
            // if (oldCarton2RCARD != null)
            // {
            //     packageFacade.DeleteCarton2RCARD(oldCarton2RCARD);
            //  }
            // //end

            //  packageFacade.SubtractCollected(product.LastSimulation.CartonCode.Trim().ToUpper());

            // CARTONINFO oldCarton = packageFacade.GetCARTONINFO(product.LastSimulation.CartonCode.Trim().ToUpper()) as CARTONINFO;
            // if (oldCarton.COLLECTED == 0)
            // {
            //      packageFacade.DeleteCARTONINFO(oldCarton);
            // }
            // #endregion

            //  packageFacade.SaveRemoveCarton2RCARDLog(product.LastSimulation.CartonCode, product.LastSimulation.RunningCard, actionEventArgs.UserCode);

            //-------Add by DS22 / Crane.Liu 2014-02-28 Start--------------
            // Description:ע�͵�
            //DataCollectFacade dcf = new DataCollectFacade(this.DataProvider);
            //dcf.RemoveFromCarton(actionEventArgs.ProductInfo.NowSimulation.RunningCard.ToUpper(), actionEventArgs.UserCode);

            //actionEventArgs.ProductInfo.NowSimulation.CartonCode = "";
            //product.LastSimulation.CartonCode = string.Empty;
            //-------Add by DS22 / Crane.Liu 2014-02-28 End----------------
        }
        // Added end

        // Added by Icyer 2007/03/09
        // �Զ�����Ƿ��������
        // ��������Ҫ�����ϣ�RunningCard��ResourceCode
        // ������Ϣ��
        //		�������Ҫ�����������򷵻ؿ�Messages
        //		�����Ҫ�����������򷵻ع�������������Messages
        //		�����Ҫ�������������ǹ�������ʧ�ܣ��򷵻�ʧ����Ϣ
        // ������Դ����B/S��"��Դ������������"������
        public Messages AutoGoMO(ActionEventArgs actionEventArgs)
        {
            return AutoGoMO(actionEventArgs, null);
        }
        public Messages AutoGoMO(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
        {
            Messages msg = new Messages();

            // �����ǰ���к�������Ʒ���򲻻�����������
            if (actionEventArgs.ProductInfo != null &&
                actionEventArgs.ProductInfo.LastSimulation != null &&
                actionEventArgs.ProductInfo.LastSimulation.IsComplete != "1")
            {
                return msg;
                /*
                if (actionEventArgs.ProductInfo.LastSimulation.ResourceCode == actionEventArgs.ResourceCode)	// ��Դ��ͬ�����ظ��ɼ�
                {
                    return msg;
                }
                else	// �Ƿ�OP��ͬ
                {
                    BenQGuru.eMES.BaseSetting.BaseModelFacade modelFacade = new BaseModelFacade(this.DataProvider);
                    object objTmpOp = modelFacade.GetOperation2Resource(actionEventArgs.ProductInfo.LastSimulation.OPCode, actionEventArgs.ResourceCode);
                    if (objTmpOp != null)	// OP��ͬ��Ҳ���ظ��ɼ�
                    {
                        return msg;
                    }
                }
                */
            }

            DBDateTime dbDateTime;
            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            string strResCode = actionEventArgs.ResourceCode;
            string strRCard = actionEventArgs.RunningCard;
            // ������Դ����Ƿ���Ҫ��������
            Resource2MO res2mo = GetResource2MOByResource(strResCode, dbDateTime);
            if (res2mo == null)		// ���û�����ã���ʾ���ù���������ֱ�ӷ���
                return msg;
            // ������кŸ�ʽ
            msg.AddMessages(CheckRCardFormatByResource2MO(res2mo, actionEventArgs.RunningCard, actionEventArgs));
            if (msg.IsSuccess() == false)
                return msg;
            // ��������
            string strMOCode = BuildMOCodeByResource2MO(res2mo, actionEventArgs.RunningCard, actionEventArgs, dbDateTime);
            // ִ�й��������Ĳ���
            GoToMOActionEventArgs gomoArg = new GoToMOActionEventArgs(ActionType.DataCollectAction_GoMO,
                actionEventArgs.RunningCard,
                actionEventArgs.UserCode,
                actionEventArgs.ResourceCode,
                actionEventArgs.ProductInfo,
                strMOCode);
            IAction gomoAction = new ActionFactory(this.DataProvider).CreateAction(ActionType.DataCollectAction_GoMO);
            try
            {
                if (actionCheckStatus != null)
                {
                    msg.AddMessages(((IActionWithStatus)gomoAction).Execute(gomoArg, actionCheckStatus));
                }
                else
                {
                    msg.AddMessages(gomoAction.Execute(gomoArg));
                }
            }
            catch (Exception e)
            {
                msg.Add(new Message(e));
            }
            if (msg.IsSuccess() == true)
            {
                msg.Add(new UserControl.Message(MessageType.Success, "$CS_GOMOSUCCESS"));

                if (actionCheckStatus == null || actionCheckStatus.NeedUpdateSimulation == true)
                {
                    ActionOnLineHelper onLine = new ActionOnLineHelper(this.DataProvider);
                    Messages msgProduct = onLine.GetIDInfo(actionEventArgs.RunningCard);
                    actionEventArgs.ProductInfo = (ProductInfo)msgProduct.GetData().Values[0];
                }
                else
                {
                    actionEventArgs.ProductInfo.LastSimulation = new ExtendSimulation(gomoArg.ProductInfo.NowSimulation);
                    actionCheckStatus.ProductInfo.LastSimulation = actionEventArgs.ProductInfo.LastSimulation;
                }
            }

            return msg;
        }
        // ���ݵ�ǰʱ���ѯ��Դ����
        private Resource2MO GetResource2MOByResource(string resourceCode, DBDateTime dbDateTime)
        {
            string strNowTime = dbDateTime.DBDate.ToString() + dbDateTime.DBTime.ToString().PadLeft(6, '0');
            string strSql = "SELECT * FROM TBLRES2MO WHERE RESCODE='" + resourceCode + "' AND STARTDATE * 1000000 + STARTTIME<=" + strNowTime + " AND ENDDATE * 1000000 + ENDTIME>=" + strNowTime + " ORDER BY SEQ DESC ";
            object[] objsSet = this.DataProvider.CustomQuery(typeof(Resource2MO), new SQLCondition(strSql));
            if (objsSet == null || objsSet.Length == 0)
                return null;
            Resource2MO res2mo = (Resource2MO)objsSet[0];
            return res2mo;
        }
        // ���ݹ������ã������кŷ������
        private Messages CheckRCardFormatByResource2MO(Resource2MO res2mo, string runningCard, ActionEventArgs actionEventArgs)
        {
            Messages msg = new Messages();
            // �Ƿ������кŷ���
            if (FormatHelper.StringToBoolean(res2mo.CheckRunningCardFormat) == true)
            {
                // ������ַ���
                if (res2mo.RunningCardPrefix != "")
                {
                    if (runningCard.Length >= res2mo.RunningCardPrefix.Length &&
                        runningCard.StartsWith(res2mo.RunningCardPrefix) == true)
                    { }
                    else
                    {
                        msg.Add(new Message(MessageType.Error, "$CS_Before_Card_FLetter_NotCompare [$CS_ID_Prefix=" + res2mo.RunningCardPrefix + "]"));
                        return msg;
                    }
                }
                // ��鳤��
                if (res2mo.RunningCardLength > 0)
                {
                    if (runningCard.Trim().Length != res2mo.RunningCardLength)
                    {
                        msg.Add(new Message(MessageType.Error, "$CS_Before_Card_Length_FLetter_NotCompare [$CS_ID_Length=" + res2mo.RunningCardLength.ToString() + "]"));
                        return msg;
                    }
                }
            }
            // ��������кŽ����������������кŵĳ���
            if (res2mo.MOGetType == Resource2MOGetType.GetFromRCard)
            {
                int iLen = Convert.ToInt32(res2mo.MOCodeRunningCardStartIndex + res2mo.MOCodeLength);
                if (runningCard.Length < iLen)
                {
                    msg.Add(new Message(MessageType.Error, "$Error_Resource2MO_RCard_Is_Short"));
                    return msg;
                }
            }
            return msg;
        }
        // ���ݹ������ú����кţ����ɹ���
        private string BuildMOCodeByResource2MO(Resource2MO res2mo, string runningCard, ActionEventArgs actionEventArgs, DBDateTime dbDateTime)
        {
            string strMOCode = "";
            if (res2mo.MOGetType == Resource2MOGetType.Static)		// �̶�����
                strMOCode = res2mo.StaticMOCode;
            else if (res2mo.MOGetType == Resource2MOGetType.GetFromRCard)		// �����кŽ�������
            {
                int iStart = Convert.ToInt32(res2mo.MOCodeRunningCardStartIndex - 1);
                int iLen = Convert.ToInt32(res2mo.MOCodeLength);
                if (runningCard.Length > iStart + iLen)
                {
                    strMOCode = runningCard.Substring(iStart, iLen);
                    // Ϊ��������ǰ׺����׺
                    if (res2mo.MOCodePrefix != "")
                        strMOCode = res2mo.MOCodePrefix + strMOCode;
                    if (res2mo.MOCodePostfix != "")
                        strMOCode = strMOCode + res2mo.MOCodePostfix;
                }
            }
            return strMOCode;
        }

        // ����Ҫ�������������к��У���ѯӦ�ù��������Ĺ�����Ϣ
        // ���ڵ�һվ��NGʱ����ѯ���������б�
        public Messages GetItemCodeFromGoMoRCard(string resourceCode, string runningCard)
        {
            Messages msg = new Messages();
            DBDateTime dbDateTime;
            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            string strResCode = resourceCode;
            string strRCard = runningCard;
            // ������Դ����Ƿ���Ҫ��������
            Resource2MO res2mo = GetResource2MOByResource(strResCode, dbDateTime);
            if (res2mo == null)		// ���û�����ã���ʾ���ù���������ֱ�ӷ���
                return msg;
            // ������кŸ�ʽ
            msg.AddMessages(CheckRCardFormatByResource2MO(res2mo, runningCard, null));
            if (msg.IsSuccess() == false)
                return msg;
            // ��������
            string strMOCode = BuildMOCodeByResource2MO(res2mo, runningCard, null, dbDateTime);
            // ��ѯ������Ϣ
            MOFacade moFacade = new MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(strMOCode);
            if (mo == null)
            {
                msg.Add(new Message(MessageType.Error, "$CS_MO_Not_Exit [" + strMOCode + "]"));
                return msg;
            }
            msg.Add(new Message(MessageType.Data, "", new object[] { mo }));
            return msg;
        }
        // Added end

    }
}
