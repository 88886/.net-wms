using System;

using BenQGuru.eMES.Common.DCT.Action;
using BenQGuru.eMES.Common.DCT.Core;

using UserControl;

namespace BenQGuru.eMES.Common.DCT.ATop.DT4000
{
    public class DT4000DCTDriver : BaseDCTDriver, IDCTDriver
    {
        private string _LastCommand = string.Empty;

        public override IDCTClient DCTClient
        {
            get
            {
                return client;
            }
            set
            {
                client = value as DT4000Client;
            }
        }

        public override bool DealSuperCommand(string command)
        {
            bool deal = false;

            if (command == CANCEL)
            {
                Cancel();
                deal = true;
            }

            if (command == EXIT)
            {
                Exit();
                deal = true;
            }

            if (command == CLS)
            {
                Clear();
                deal = true;
            }

            return deal;
        }

        //���DCT Client����
        protected override void Clear()
        {
            if (client == null)
                return;

            (client as DT4000Client).ClearScreen();

            (client as DT4000Client).ClearRecievedData();

            BaseDCTAction action = stack.GetNextAction();
            action.FlowDirect = FlowDirect.WaitingInput;
        }

        //��ֹ��ǰ��Action
        protected override void Cancel()
        {
            if (client == null)
                return;

            if (client.Authorized)
                this.stack.ResetAction();
            else
                this.AddActionIdle(MessageType.Normal);

            (client as DT4000Client).ClearRecievedData();

        }

        //�ǳ�
        protected override void Exit()
        {
            if (client == null)
                return;

            client.Authorized = false;
            client.LoginedUser = string.Empty;
            client.LoginedPassword = string.Empty;
            client.ResourceCode = string.Empty;
            client.CachedAction = null;

            ((DT4000Client)client).ClearRecievedData();

            this.AddActionIdle(MessageType.Normal);

            base.RaiseAfterLogout();
        }

        //����DCT Client
        public override void DCTListen(object obj)
        {
            //��ʼ��
            try
            {
                client.Open();
            }
            catch
            {
            }

            //��ӳ�ʼ��Action
            if (stack.GetNextAction() == null)
                this.AddActionIdle(MessageType.Normal);

            //ѭ�������û�����
            while ((client as DT4000Client).ClientStatus == DT4000ClientStatus.Connecting)
            {
                try
                {
                    client.Open();

                    //������������: CANCEL, LOGIN, EXIT, CLS
                    if (!DealSuperCommand(client.ToString()))
                    {

                        //һ���������ת
                        SwitchCommand();

                        //����������
                        CycleResponse();

                        //��ȡ�û�����
                        CycleRequest();
                    }
                }
                catch (Exception ex)
                {
                    UserControl.FileLog.FileLogOut("DCTControlPanel.log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ex.StackTrace);
                    Log.Error(ex.Message);
                }
            }
        }

        //�����û����󲢷��ؽ��
        public override void CycleResponse()
        {
            if (stack.CurrentDirect == FlowDirect.WaitingOutput)
            {
                try
                {
                    //��ȡ��ǰ��Action
                    BaseDCTAction action = stack.GetNextAction();

                    //��ȡĬ��ACtion
                    if (action == null)
                    {
                        ActionHelper actionHelper = new ActionHelper();
                        BaseDCTAction endNodeAction = actionHelper.GetEndNodeAction(client);
                        stack.ResetActionStack(endNodeAction);

                        if (endNodeAction != null)
                        {
                            this._LastCommand = actionHelper.GetCommandByAction(endNodeAction);
                        }

                        return;
                    }

                    //ȷ����Ҫ��¼��Action�ڵ�¼֮��ſ���ִ��
                    if (action.NeedAuthorized && !client.Authorized)
                    {
                        this.AddActionIdle(MessageType.Error);
                        return;
                    }

                    if (!action.NeedAuthorized || client.Authorized)
                    {
                        // ִ��Action
                        UserControl.Messages msgs = action.Do(client);

                        if (stack.CancelActionOutput)
                        {
                            stack.CancelActionOutput = false;
                        }
                        else
                        {
                            this.SendMessage(client, msgs);
                        }
                    }

                }
                catch (Exception ex)
                {
                    DealExcepion(ex);
                }
            }
        }

        //��DCT Client��ȡ�û�����
        public override void CycleRequest()
        {
            if (stack.CurrentDirect == FlowDirect.WaitingInput)
            {
                try
                {
                    //��ȡDCT����
                    string strRec = (client as DT4000Client).RecieveData();

                    //ȷ���������
                    BaseDCTAction action = stack.GetNextAction();
                    if (strRec != null && strRec.Trim().Length > 0 && strRec.IndexOf("ALL RIGHTS") < 0)
                    {
                        action.FlowDirect = FlowDirect.WaitingOutput;
                    }
                }
                catch (Exception ex)
                {
                    DealExcepion(ex);
                }
            }
        }

        //�����л�
        public void SwitchCommand()
        {
            if (client.ToString() != null && client.ToString().Trim().Length > 0)
            {
                string data = client.ToString().Trim().ToUpper();
                BaseDCTAction action = (new ActionHelper()).GetActionByCommand(data);

                if (action != null)
                {
                    stack.ResetActionStack(action);

                    (client as DT4000Client).ClearRecievedData();

                    this._LastCommand = data;
                }
            }
        }

        private void AddActionIdle(MessageType msgType)
        {
            try
            {
                ActionIdle actionIdle = new ActionIdle();
                actionIdle.OutMesssage = new UserControl.Message(msgType, "$DCT_LOGIN");
                stack.ResetActionStack(actionIdle);
            }
            catch (Exception ex)
            {
                DealExcepion(ex);
            }
        }

        private void DealExcepion(Exception ex)
        {
            UserControl.FileLog.FileLogOut("DCTControlPanel.log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ex.StackTrace);

            UserControl.Messages msgs = new UserControl.Messages();
            msgs.Add(new UserControl.Message(ex));

            if (ex.GetType().FullName != "System.Net.Sockets.SocketException"
                    && ex.GetType().FullName != "System.Threading.ThreadAbortException")
            {
                this.SendMessage(client, msgs);
            }
        }        

        private void SendMessage(object sender, Messages msgs)
        {
            this.stack.SendMessage(sender, msgs, this._LastCommand);
        }
    }
}
