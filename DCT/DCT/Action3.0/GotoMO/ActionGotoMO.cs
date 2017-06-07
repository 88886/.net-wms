using System;
using UserControl;
using BenQGuru.eMES.Common.DCT.Core;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.DataCollect.Action;
using System.Text.RegularExpressions;

namespace BenQGuru.eMES.Common.DCT.Action
{
    /// <summary>
    /// ActionMO ��ժҪ˵����
    /// </summary>
    public class ActionGotoMO : BaseDCTAction
    {
        public ActionGotoMO()
        {
            this.InitMessage = (new ActionHelper()).GetActionDesc(this);
            this.OutMesssage = new Message(MessageType.Normal, "$CS_Please_Input_MOCode");
            this.LastPrompMesssage = new Message(MessageType.Normal, "$CS_Please_Input_MOCode");
        }

        public string _moCode = string.Empty;	// ��������

        public override Messages PreAction(object act)
        {
            Messages msgs = new Messages();

            // ���빤������
            if (_moCode == string.Empty)
            {
                Messages msgMo = CheckMO(act);
                if (msgMo.IsSuccess() == false)
                {
                    ProcessBeforeReturn(this.Status, msgMo);
                    return msgMo;
                }
                _moCode = act.ToString().ToUpper();
            }

            base.PreAction(act);

            this.OutMesssage = new Message(MessageType.Normal, "$CS_Please_Input_RunningCard [" + _moCode + "]");

            msgs.Add(this.OutMesssage);

            ProcessBeforeReturn(this.Status, msgs);
            return msgs;

        }

        // Added by Icyer 2006/12/15
        // ��鹤��
        private Messages CheckMO(object act)
        {
            Messages msgs = new Messages();
            BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider = null;
            if ((act as IDCTClient).DBConnection != null)
            {
                domainProvider = (act as IDCTClient).DBConnection as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
            }
            else
            {
                domainProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider()
                    as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
                (act as IDCTClient).DBConnection = domainProvider;
            }

            MOFacade moFacade = new MOFacade(domainProvider);
            MO mo = (MO)moFacade.GetMO(act.ToString().ToUpper());
            if (mo == null)
            {
                msgs.Add(new UserControl.Message(MessageType.Error, "$CS_MO_NOT_EXIST"));
                ProcessBeforeReturn(this.Status, msgs);
                return msgs;
            }
            if (mo.MOStatus != BenQGuru.eMES.Web.Helper.MOManufactureStatus.MOSTATUS_RELEASE &&
                mo.MOStatus != BenQGuru.eMES.Web.Helper.MOManufactureStatus.MOSTATUS_OPEN)
            {
                msgs.Add(new UserControl.Message(MessageType.Error, "$Error_CS_MO_Should_be_Release_or_Open"));
                ProcessBeforeReturn(this.Status, msgs);
                return msgs;
            }

            MO2Route route = (MO2Route)moFacade.GetMONormalRouteByMOCode(mo.MOCode);
            if (route == null)
            {
                msgs.Add(new UserControl.Message(MessageType.Error, "$CS_MOnotNormalRoute"));
                ProcessBeforeReturn(this.Status, msgs);
                return msgs;
            }

            DataCollectFacade dataCollectFacade = new DataCollectFacade(domainProvider);
            ItemRoute2OP op = dataCollectFacade.GetMORouteFirstOP(mo.MOCode, route.RouteCode);
            BenQGuru.eMES.BaseSetting.BaseModelFacade dataModel = new BenQGuru.eMES.BaseSetting.BaseModelFacade(domainProvider);
            if (dataModel.GetOperation2Resource(op.OPCode, (act as IDCTClient).ResourceCode) == null)
            {
                msgs.Add(new UserControl.Message(MessageType.Error, "$CS_Route_Failed_FirstOP $Domain_MO =" + mo.MOCode));
                ProcessBeforeReturn(this.Status, msgs);
                return msgs;
            }

            ProcessBeforeReturn(this.Status, msgs);

            return msgs;
        }
        // Added end

        public override Messages Action(object act)
        {
            Messages msgs = new Messages();
            BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider = null;

            if (act == null)
            {
                return msgs;
            }

            DataCollect.Action.ActionEventArgs args;
            if (ObjectState == null)
            {
                args = new BenQGuru.eMES.DataCollect.Action.ActionEventArgs();
            }
            else
            {
                args = ObjectState as DataCollect.Action.ActionEventArgs;
            }

            string data = act.ToString().ToUpper().Trim();//��Ʒ���к�
            args.RunningCard = data;
            //Laws Lu,2006/06/03	���	��ȡ��������
            if ((act as IDCTClient).DBConnection != null)
            {
                domainProvider = (act as IDCTClient).DBConnection as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
            }
            else
            {
                domainProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider()
                    as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
                (act as IDCTClient).DBConnection = domainProvider;
            }

            //msg = CheckData(data,domainProvider);		// Removed by Icyer 2006/12/15
            msgs = CheckSN(this._moCode, args.RunningCard, domainProvider);

            //add by hiro 08/11/05
            if (msgs.IsSuccess())
            {
                msgs = this.CheckSNContent(this._moCode, args.RunningCard, domainProvider);
            }
            //end by hiro 

            if (msgs.IsSuccess())
            {

                //������к�
                ActionOnLineHelper _helper = new ActionOnLineHelper(domainProvider);
                //msg.AddMessages(  _helper.GetIDInfo( args.RunningCard ) );
                msgs = _helper.GetIDInfoByMoCodeAndId(_moCode,args.RunningCard);

                if (msgs.IsSuccess())
                {
                    IDCTClient client = act as IDCTClient;

                    ProductInfo product = (ProductInfo)msgs.GetData().Values[0];

                    GoToMOActionEventArgs gotoMOArgs = new GoToMOActionEventArgs(
                        ActionType.DataCollectAction_GoMO,
                        args.RunningCard,
                        client.LoginedUser,
                        client.ResourceCode,
                        product,
                        _moCode);

                    IAction action = new BenQGuru.eMES.DataCollect.Action.ActionFactory(domainProvider).CreateAction(ActionType.DataCollectAction_GoMO);

                    //((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)domainProvider).PersistBroker.OpenConnection();
                    domainProvider.BeginTransaction();
                    try
                    {
                        msgs = ((IActionWithStatus)action).Execute(gotoMOArgs);

                        if (msgs.IsSuccess())
                        {
                            domainProvider.CommitTransaction();
                            msgs.Add(new UserControl.Message(MessageType.Success, string.Format("$CS_GOMO_CollectSuccess")));

                        }
                        else
                        {
                            if (msgs.OutPut().IndexOf("$CS_ID_Has_Already_Belong_To_This_MO") == 0)
                            {
                                msgs.ClearMessages();
                                msgs.Add(new UserControl.Message(UserControl.MessageType.Error, "$CS_ID_Has_Already_Belong_To_This_MO"));
                            }
                            domainProvider.RollbackTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        domainProvider.RollbackTransaction();

                        msgs.Add(new UserControl.Message(ex));
                    }
                    finally
                    {
                        ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)domainProvider).PersistBroker.CloseConnection();
                    }
                }

            }

            if (msgs.IsSuccess())
            {
                base.Action(act);
            }

            this.ObjectState = null;

            ProcessBeforeReturn(this.Status, msgs);

            return msgs;
        }

        public override Messages AftAction(object act)
        {
            base.AftAction(act);

            return null;
        }

        #region Check Data
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">��������</param>
        /// <returns></returns>
        public Messages CheckData(string data, Common.DomainDataProvider.SQLDomainDataProvider domainProvider)
        {
            Messages msg = new Messages();
            if (data == string.Empty)
            {
                msg.Add(new UserControl.Message(UserControl.MessageType.Error, "$Error_MO_Empty"));
            }
            else
            {

                object obj = new MOFacade(domainProvider).GetMO(data);

                if (obj == null)
                {
                    msg.Add(new UserControl.Message(UserControl.MessageType.Error, "$Error_MO_Not_Exist"));
                }
                else
                {

                    if ((((MO)obj).MOStatus != Web.Helper.MOManufactureStatus.MOSTATUS_RELEASE) &&
                        (((MO)obj).MOStatus != Web.Helper.MOManufactureStatus.MOSTATUS_OPEN))
                    {
                        msg.Add(new UserControl.Message(UserControl.MessageType.Error, "$Error_CS_MO_Should_be_Release_or_Open2"));
                    }
                }
            }

            return msg;
        }

        public Messages CheckSN(string moCode, string rcard, Common.DomainDataProvider.SQLDomainDataProvider domainProvider)
        {
            Messages msg = new Messages();
            if (System.Configuration.ConfigurationSettings.AppSettings["CheckRCardRange"] == "1")
            {
                msg.AddMessages(CheckSNRange(moCode, rcard, domainProvider));
            }
            return msg;
        }
        public Messages CheckSNRange(string moCode, string rcard, Common.DomainDataProvider.SQLDomainDataProvider domainProvider)
        {
            Messages msg = new Messages();
            MORunningCardFacade rcardFacade = new MORunningCardFacade(domainProvider);
            if (rcardFacade.CheckRunningCardInRange(moCode, BenQGuru.eMES.Web.Helper.MORunningCardType.BeforeConvert, rcard) == false)
            {
                msg.Add(new Message(MessageType.Error, "$DCT_GOMO_SN_Not_In_Range"));
                return msg;
            }
            return msg;
        }

        //add by hiro 08/11/05 ������к�����Ϊ��ĸ,���ֺͿո�
        public Messages CheckSNContent(string moCode, string rcard, Common.DomainDataProvider.SQLDomainDataProvider domainProvider)
        {
            Messages msg = new Messages();
            MOFacade mofcade = new MOFacade(domainProvider);
            ItemFacade itemfacade = new ItemFacade(domainProvider);
            object objMo=mofcade.GetMO(moCode);
            if (objMo == null)
            {
                return msg;
            }
            string itemCode = ((MO)objMo).ItemCode.ToString();
            object objItemSNcheck = itemfacade.GetItem2SNCheck(itemCode,ItemCheckType.ItemCheckType_SERIAL);

            if (objItemSNcheck==null)
            {
                return msg;
            }
            if (((Item2SNCheck)objItemSNcheck).SNContentCheck==SNContentCheckStatus.SNContentCheckStatus_Need)
            {
                string pattern = @"^([A-Za-z0-9]+[ ]*)*[A-Za-z0-9]+$";
                Regex rex = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = rex.Match(rcard);
                if (!match.Success)
                {
                    msg.Add(new UserControl.Message(MessageType.Error, "$CS_SNContent_CheckWrong $CS_Param_RunSeq:" + rcard.Trim()));
                    return msg;
                }
            }
            return msg;
        }
        //end 
        #endregion










        //melo zheng,2007.8.20,�ж��ַ����Ƿ�Ϊ����
        private bool isInt(string tmp)
        {
            bool b = false;
            try
            {
                int i = Int32.Parse(tmp);
                b = true;
            }
            catch
            {
                b = false;
            }
            return b;
        }

        /*	Removed by Icyer 2007/08/22
        // �ƶ���GOOD�ɼ�
        //melo zheng,2007.8.20,SMT����
        private Messages SMTLoadItem(string rcard, string resourceCode, string userCode,BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider)
        {
            Messages msg = new Messages();
            BenQGuru.eMES.SMT.SMTFacade smtFacade = new BenQGuru.eMES.SMT.SMTFacade(domainProvider);
            msg = smtFacade.LoadMaterialForRCard(rcard, resourceCode, userCode);
            return msg;
        }
        */

        // Added by Icyer 2007/08/22
        public Messages CheckSNFormat(DataCollect.Action.ActionEventArgs args)
        {
            Messages msg = new Messages();
            return msg;
            //melo zheng,2007.8.20,�������кŸ�ʽ
            //��Ʒ���кų��ȼ��:��Ʒ���к�11λ=������7λ+��ˮ��4λ
            int len = 11;
            try
            {
                if (args.RunningCard.Length != len)
                {
                    msg.Add(new UserControl.Message(MessageType.Error, "$CS_Before_Card_Length_FLetter_NotCompare2"));
                    return msg;
                }
                else
                {

                    /*
                                ��Ʒ���к����ַ�����飺
                                ��1λ��G��Ϊ�˱������ַ�Ϊ���������֡�������
                                ��2λ�͵�3λ����2�루2007��Ĺ�������ʾ����������
                                ��4λ����һ�루�ӣ����ã������������£��´������£��ô������£�
                                ��5λ�͵���λ��������
                                ��7λ����ˮ�ţ����ǵ�������ģ��ÿ���·��������������ᳬ������������λ��ʾ���ַ�Χ�Ǵӡ���������������
                                �ڹ����ı��루��������������λ���Ļ����ϣ�������ӣ�λ��ˮ�ţ��ӣ�������������������
                                 */
                    //��1λ��G
                    if (args.RunningCard.Substring(0, 1) != "G")
                    {
                        msg.Add(new UserControl.Message(MessageType.Error, "$CS_Before_Card_FLetter_NotCompare_First2"));
                        return msg;
                    }
                    //��2-3λ����
                    if (!isInt(args.RunningCard.Substring(1, 2)))
                    {
                        msg.Add(new UserControl.Message(MessageType.Error, "$CS_Before_Card_FLetter_NotCompare_First2"));
                        return msg;
                    }
                    //��4λ����:1-9,A,B,C
                    if (isInt(args.RunningCard.Substring(3, 1)))
                    {
                        if (args.RunningCard.Substring(3, 1) == "0")
                        {
                            msg.Add(new UserControl.Message(MessageType.Error, "$CS_Before_Card_FLetter_NotCompare_First2"));
                            return msg;
                        }
                    }
                    else if (args.RunningCard.Substring(3, 1) != "A" && args.RunningCard.Substring(3, 1) != "B"
                        && args.RunningCard.Substring(3, 1) != "C")
                    {
                        msg.Add(new UserControl.Message(MessageType.Error, "$CS_Before_Card_FLetter_NotCompare_First2"));
                        return msg;
                    }
                    //��5-6λ����
                    if (!isInt(args.RunningCard.Substring(4, 2)) || Convert.ToInt16(args.RunningCard.Substring(4, 2)) == 0 ||
                        Convert.ToInt16(args.RunningCard.Substring(4, 2)) > 31)
                    {
                        msg.Add(new UserControl.Message(MessageType.Error, "$CS_Before_Card_FLetter_NotCompare_First2"));
                        return msg;
                    }
                    //��7λ����ˮ��:1-9
                    if (!isInt(args.RunningCard.Substring(6, 1)) || args.RunningCard.Substring(6, 1) == "0")
                    {
                        msg.Add(new UserControl.Message(MessageType.Error, "$CS_Before_Card_FLetter_NotCompare_First2"));
                        return msg;
                    }
                    //��4λ����ˮ��:0001-9999
                    if (!isInt(args.RunningCard.Substring(7, 4)) || args.RunningCard.Substring(7, 4) == "0000")
                    {
                        msg.Add(new UserControl.Message(MessageType.Error, "$CS_Before_Card_FLetter_NotCompare_First2"));
                        return msg;
                    }
                }
            }
            catch
            {
            }
            return msg;
        }

    }
}
