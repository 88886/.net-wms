using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

using BenQGuru.eMES.Common.DCT.Core;

namespace BenQGuru.eMES.Common.DCT.ATop.DT4000
{
    /// <summary>
    /// DT4000Client ��ժҪ˵����
    /// </summary>
    public class DT4000Client : IDCTClient
    {
        private const string IN = "  IN  ";
        private const string OUT = "  OUT  ";

        private BaseDCTAction cachedAction = null;

        #region ����

        //ip��ַ
        protected string _ClientAddress = String.Empty;
        //�˿�
        protected int _ClientPort = 55962;
        //����
        protected string _ClientDesc = String.Empty;
        //ID
        protected int _ClientID;
        //״̬
        protected DT4000ClientStatus _ClientStatus;
        //��ǰ���ܵ��ַ���
        protected string _RecievedData;
        //��Դ����
        protected string _ResourceCode = String.Empty;
        protected string _SegmentCode = String.Empty;
        protected string _StepSequenceCode = String.Empty;
        protected string _ShiftTypeCode = String.Empty;
        //��½�û�
        protected string _LoginUser = String.Empty;
        //��½�û�
        protected string _Password = String.Empty;
        //ͨѶ�õ�Socket
        protected Socket socket;
        //�Ƿ��½
        protected bool _Authorized;
        //DB�����ṩ����
        protected object db_connection = null;

        //�豸���һ�α����Ƿ�Live��ʱ��
        private DateTime _LastReportTime = DateTime.Now;
        //�Ƿ���
        private bool _IsAlive = true;

        #endregion

        #region ����

        /// <summary>
        /// DB ��������
        /// </summary>
        public object DBConnection
        {
            set
            {
                db_connection = value;
            }
            get
            {
                return db_connection;
            }
        }
        /// <summary>
        /// �Ƿ�Ϊ�Ѿ���½
        /// </summary>
        public bool Authorized
        {
            get
            {
                return _Authorized;
            }
            set
            {
                _Authorized = value;

            }
        }
        /// <summary>
        /// ��½�û�
        /// </summary>
        public string LoginedUser
        {
            get
            {
                //���ص�ǰ�ϸڵ���Ա�б�,�����Ա�б��ǿյ�,
                //�򷵻ص�ǰ��¼����Ա
                //BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider()
                //    as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;

                string operlist = string.Empty;
                //BenQGuru.eMES.HourCounting.HourCountingFacade facade = new BenQGuru.eMES.HourCounting.HourCountingFacade(domainProvider);
                //object[] objs = facade.GetHourCountingOperaterWorkOnByResource(this.ResourceCode);
                //if (objs != null && objs.Length > 0)
                //{
                //    foreach (BenQGuru.eMES.Domain.HourCounting.HourCountingOperaterWorkOn wo in objs)
                //    {
                //        operlist = operlist + wo.OperaterNo + ",";
                //    }

                //    if (operlist.Length > 0)
                //    {
                //        operlist = operlist.Substring(0, operlist.Length - 1);
                //    }
                //}

                if (operlist != string.Empty)
                    return operlist;
                else
                    return _LoginUser;
            }
            set
            {
                _LoginUser = value;
            }
        }
        /// <summary>
        /// ��½�û�
        /// </summary>
        public string LoginedPassword
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }
        /// <summary>
        /// ��Դ����
        /// </summary>
        public string ResourceCode
        {
            get
            {
                return _ResourceCode;
            }
            set
            {
                _ResourceCode = value;
                if (AfterLogin != null)
                {
                    AfterLogin(this, new CommandEventArgs(value));
                }
            }
        }
        public string SegmentCode
        {
            get
            {
                return _SegmentCode;
            }
            set
            {
                _SegmentCode = value;
                if (AfterLogin != null)
                {
                    AfterLogin(this, new CommandEventArgs(value));
                }
            }
        }
        public string StepSequenceCode
        {
            get
            {
                return _StepSequenceCode;
            }
            set
            {
                _StepSequenceCode = value;
                if (AfterLogin != null)
                {
                    AfterLogin(this, new CommandEventArgs(value));
                }
            }
        }
        public string ShiftTypeCode
        {
            get
            {
                return _ShiftTypeCode;
            }
            set
            {
                _ShiftTypeCode = value;
                if (AfterLogin != null)
                {
                    AfterLogin(this, new CommandEventArgs(value));
                }
            }
        }
        /// <summary>
        /// ���ܵ�����
        /// </summary>
        public string RecievedData
        {
            get
            {
                return _RecievedData;
            }
            set
            {
                _RecievedData = value;
            }
        }
        /// <output>����</output>
        public DCTType Type
        {
            get
            {
                return DCTType.DT4000;
            }
        }
        /// <output>IP��ַ</output>
        public string ClientAddress
        {
            get
            {
                return _ClientAddress;
            }
            set
            {
                _ClientAddress = value;
            }
        }

        /// <output>�˿�</output>
        public int ClientPort
        {
            get
            {
                return _ClientPort;
            }
            set
            {
                _ClientPort = value;
            }
        }

        /// <output>ID</output>
        public int ClientID
        {
            get
            {
                return _ClientID;
            }
            set
            {
                _ClientID = value;
            }
        }

        /// <output>����</output>
        public string ClientDesc
        {
            get
            {
                return _ClientDesc;
            }
            set
            {
                _ClientDesc = value;
            }
        }

        /// <output>״̬</output>
        public DT4000ClientStatus ClientStatus
        {
            get
            {
                if (this.socket.Connected == false)
                {
                    _ClientStatus = DT4000ClientStatus.Closed;
                }
                return _ClientStatus;
            }
            set
            {
                _ClientStatus = value;
            }
        }

        public BaseDCTAction CachedAction
        {
            set
            {
                cachedAction = value;
            }
            get
            {
                return cachedAction;
            }
        }
        #endregion

        public DT4000Client(string ip, int port)
        {
            try
            {
                /*
                if(OnSendData == null)
                {
                    OnSendData += new EventCommandHandler(DT4000Client_OnSendData);
                }
                */
                _ClientAddress = ip;
                _ClientPort = port;

                InitialClient();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DT4000Client_OnSendData(object sender, CommandEventArgs e)
        {
            return null;
        }

        #region ����

        public int Reset()
        {
            return 0;
        }

        public int ChangeStatus()
        {
            return 0;
        }

        public int ChangeIP()
        {
            return 0;
        }

        public int ChangePort()
        {
            return 0;
        }

        public void Open()
        {
            try
            {
                _LastReportTime = DateTime.Now;

                if (socket == null)
                {
                    InitialClient();
                }

                if (socket.Connected == false || socket.Poll(200, SelectMode.SelectError))
                {
                    IPAddress ipAdd = IPAddress.Parse(_ClientAddress);
                    IPEndPoint ipEnd = new IPEndPoint(ipAdd, _ClientPort);

                    socket.Connect(ipEnd);

                    this.SendCommand(DCTCommand.ClearText);

                    this._ClientStatus = DT4000ClientStatus.Connecting;

                    this.SendCommand(DCTCommand.SpeakerOff);//�ص�beeper
                    this.SendCommand(DCTCommand.AutoReportingOff);//�ص��Զ�����
                    this.SendCommand(DCTCommand.AutoReportingOn);//�Զ������Ƿ�live
                    this.SendCommand(DCTCommand.HostReportSetting);
                    this.SendCommand(DCTCommand.ClearText);
                    this.SendCommand(DCTCommand.ClearGraphic);
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new CommandEventArgs(ex.Message));
                }
                socket = null;
                InitialClient();
                //throw ex;
            }
        }


        public void Close()
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            //Add by vivan.sun 2011-6-3
            if (this.DBConnection != null)
            {
                BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider = this.DBConnection as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
                domainProvider.PersistBroker.CloseConnection();
            }
            //end add
        }

        public string SendCommand(DCTCommand command)
        {
            string strReturn = String.Empty;

            if (socket == null || socket.Connected == false)
            {
                Open();
            }

            DT4000CommandList comList = new DT4000CommandList();

            socket.Send(comList.GetCommand(command));

            /*
            if(OnSendData != null)
            {
                OnSendData(this
                    ,new CommandEventArgs(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + OUT + command.ToString()));
            }
            */

            return strReturn;
        }

        public string SendMessage(string msg, int line)
        {
            string strReturn = String.Empty;

            //btSetCursor
            byte[] btSetCursor = new byte[DT4000MessageParser.MessageSetCursor1.Length];
            DT4000MessageParser.MessageSetCursor1.CopyTo(btSetCursor, 0);
            if (line < 1)
                btSetCursor[11] = 1;
            else if (line > 4)
                btSetCursor[11] = 4;
            else
                btSetCursor[11] = (byte)line;

            //btBackToOutput          
            byte[] btBackToOutput = new byte[DT4000MessageParser.MessagePrefix.Length];
            DT4000MessageParser.MessagePrefix.CopyTo(btBackToOutput, 0);

            //btOriginal
            byte[] btOriginal = System.Text.Encoding.Default.GetBytes(msg);
            if (System.Configuration.ConfigurationSettings.AppSettings["Language"] == "CHS")	// ��������
            {
                btOriginal = System.Text.Encoding.GetEncoding("GB2312").GetBytes(msg);
            }
            else if (System.Configuration.ConfigurationSettings.AppSettings["Language"] == "CHT")	// ��������
            {
                btOriginal = System.Text.Encoding.GetEncoding("BIG5").GetBytes(msg);
            }

            byte[] btOriginal2 = new byte[_DCTEnLineLen - 1];

            for (int i = 0; i < _DCTEnLineLen - 1; i++)
            {
                if (i < btOriginal.Length)
                    btOriginal2[i] = btOriginal[i];
                else
                    btOriginal2[i] = 32;
            }

            byte[] btFinal = (new DT4000MessageParser()).WrapMessageNew(btOriginal2);


            //Open
            if (socket == null || socket.Connected == false)
            {
                Open();
            }

            //���ص��������
            socket.Send(btBackToOutput);

            //���ù��λ��
            socket.Send(btSetCursor);

            //�����Ϣ
            //socket.Send(DT4000MessageParser.MessageClearText);
            //socket.Send(btSetCursor);
            socket.Send(btFinal);

            //���ù��λ�õ���һ��
            socket.Send(new byte[] { 0x0c, 0x00, 0x40, 0x00, 0x00, 0x00, 0x65, 0x00, 0x00, 0x01, 0x00, 0x01 });

            //����û��������
            socket.Send(new DT4000CommandList().GetCommand(DCTCommand.ClearUserInput));

            if (OnSendData != null)
            {
                OnSendData(this
                    , new CommandEventArgs(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + OUT + msg));
            }

            btOriginal = null;
            btFinal = null;

            return strReturn;
        }

        int _DCTEnLineLen = 30;
        bool _Cut = false;
        byte[] _OldMessageByteArray = null;

        public string SendMessage(string msg)
		{
			string strReturn = String.Empty;

            if (msg == null || msg.Trim().Length <= 0)
                return strReturn;

            //��ȡ���뷽ʽ
            System.Text.Encoding currEncoding = System.Text.Encoding.Default;
            if (System.Configuration.ConfigurationSettings.AppSettings["Language"] == "CHS")	// ��������
            {
                currEncoding = System.Text.Encoding.GetEncoding("GB2312");
            }
            else if (System.Configuration.ConfigurationSettings.AppSettings["Language"] == "CHT")	// ��������
            {
                currEncoding = System.Text.Encoding.GetEncoding("BIG5");
            }

            //������Ϣ����
            string sendTemp = string.Empty;
            ArrayList msgList = new ArrayList();

            while (currEncoding.GetByteCount(msg) > _DCTEnLineLen)
            {
                for (int i = 1; i < currEncoding.GetByteCount(msg); i++)
                {
                    if (currEncoding.GetByteCount(msg.Substring(0, i)) > _DCTEnLineLen)
                    {
                        sendTemp = msg.Substring(0, i - 1);
                        msg = msg.Substring(i - 1);
                        break;
                    }
                }

                if (!_Cut)
                    msgList.Add(sendTemp);
            }

            //�������һ��
            if (msg.Length > 0)
            {
                if (currEncoding.GetByteCount(msg) < _DCTEnLineLen)
                {
                    msgList.Add(msg);
                }
                else
                {
                    for (int i = 1; i < currEncoding.GetByteCount(msg); i++)
                    {
                        if (currEncoding.GetByteCount(msg.Substring(0, i)) >= _DCTEnLineLen)
                        {
                            sendTemp = msg.Substring(0, i - 1);
                            msg = msg.Substring(i - 1);
                            break;
                        }
                    }
                    if (!_Cut)
                        msgList.Add(sendTemp);

                    if (msg.Length > 0)
                        msgList.Add(msg);
                }
            }

            //ÿ��ĩβ����ո�
            ArrayList lineByteList = new ArrayList();
            int byteCount = 0;
            for (int i = 0; i < msgList.Count; i++)
            {
                byte[] btOriginal = new byte[_DCTEnLineLen];
                for (int j = 0; j < btOriginal.Length; j++)
                    btOriginal[j] = 32;

                currEncoding.GetBytes((string)msgList[i]).CopyTo(btOriginal, 0);

                lineByteList.Add(btOriginal);
                byteCount += btOriginal.Length;
            }

            //������Ļ��ʾ��Ҫ���ֽ�����
            if (_OldMessageByteArray == null)
            {
                _OldMessageByteArray = new byte[3 * _DCTEnLineLen];
                for (int i = 0; i < _OldMessageByteArray.Length; i++)
                    _OldMessageByteArray[i] = 32;
            }

            byte[] newMessageByteArray = new byte[_OldMessageByteArray.Length + byteCount];
            for (int i = 0; i < newMessageByteArray.Length; i++)
                newMessageByteArray[i] = 32;
            _OldMessageByteArray.CopyTo(newMessageByteArray, 0);

            byteCount = 0;
            for (int i = 0; i < lineByteList.Count; i++)
            {
                ((byte[])lineByteList[i]).CopyTo(newMessageByteArray, _OldMessageByteArray.Length + byteCount);
                byteCount += ((byte[])lineByteList[i]).Length;
            }

            //���¼�¼��ʾ��Ϣ���ֽ�����
            if (newMessageByteArray.Length <= 3 * _DCTEnLineLen)
                _OldMessageByteArray = new byte[newMessageByteArray.Length];
            else
                _OldMessageByteArray = new byte[3 * _DCTEnLineLen];

            for (int i = 0; i < _OldMessageByteArray.Length; i++)
            {
                _OldMessageByteArray[i] = newMessageByteArray[newMessageByteArray.Length - _OldMessageByteArray.Length + i];
            }

            //��÷��͸�DCT������
            byte[] btFinal = (new DT4000MessageParser()).WrapMessageNew(_OldMessageByteArray);


            //��ʾ
            if (socket == null || socket.Connected == false)
            {
                Open();
            }
            socket.Send(DT4000MessageParser.MessageClearText);
            socket.Send(DT4000MessageParser.MessageSetCursor2);
            socket.Send(btFinal);
            socket.Send(DT4000MessageParser.MessageSetCursor1);
            socket.Send(new DT4000CommandList().GetCommand(DCTCommand.ClearUserInput));

            if (OnSendData != null)
            {
                OnSendData(this
                    , new CommandEventArgs(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + OUT + msg));
            }

            btFinal = null;

            return strReturn;
		}

        public string RecieveData()
        {
            string strReturn = String.Empty;

            byte[] btRecieve = new byte[512];
            //string strRecieve = String.Empty;
            if (socket == null || socket.Connected == false)
            {
                Open();
            }
            socket.Receive(btRecieve);

            if (btRecieve.Length >= 7)
            {
                //�ж��ǲ����Զ�reportָ��
                if (btRecieve[0] == (byte)0x8
                    &&
                    btRecieve[1] == (byte)0x0
                    &&
                    btRecieve[2] == (byte)0x40
                    &&
                    btRecieve[3] == (byte)0x0
                    &&
                    btRecieve[4] == (byte)0x0
                    &&
                    btRecieve[5] == (byte)0x0
                    &&
                    btRecieve[6] == (byte)0x11
                    )
                {
                    this._LastReportTime = DateTime.Now;
                    this.SendCommand(DCTCommand.HostReportPackage);
                    return string.Empty;
                }
            }
            strReturn = System.Text.Encoding.Default.GetString(
                (new DT4000MessageParser()).RetrieveData(btRecieve));

            _RecievedData = strReturn.ToUpper().Trim();

            if (OnSendData != null)
            {
                OnSendData(this
                    , new CommandEventArgs(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + IN + strReturn));
            }
            btRecieve = null;

            return strReturn.ToUpper().Trim();

        }

        /// <summary>
        /// ��ʼ��Client����
        /// </summary>
        protected void InitialClient()
        {
            if (socket == null)
            {
                socket = new Socket(
                    AddressFamily.InterNetwork
                    , SocketType.Stream
                    , ProtocolType.IP);

                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 500);
            }
        }

        public override string ToString()
        {
            return this.RecievedData;
        }

        public bool IsAlive()
        {
            _IsAlive = true;

            if (_LastReportTime.AddSeconds(CommandEventArgs.DeadInterval) < DateTime.Now)
            {
                _IsAlive = false;
            }
            else
            {
                _IsAlive = true;
            }

            return _IsAlive;
        }

        public void ClearRecievedData()
        {
            this.RecievedData = "";
        }

        public void ClearScreen()
        {
            this.SendCommand(DCTCommand.ClearText);
            this.SendCommand(DCTCommand.ClearUserInput);
            this._OldMessageByteArray = null;
        }

        #endregion

        public event EventCommandHandler OnSendData;

        public event EventCommandHandler OnError;

        public event EventCommandHandler AfterLogin;

    }

    /// <author>Laws Lu</author>
    /// <since>2006/04/12</since>
    /// <version>1.0.0</version>
    public enum DT4000ClientStatus
    {
        TCPCreateError = -1,
        ClientNotExist = -2,
        Closed = 0,
        WaitingForConnect = 6,
        Connecting = 7,
        TimeOut = 8,
        TCPConnectError = 9
    }
}
