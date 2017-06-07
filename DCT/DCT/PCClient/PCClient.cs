using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Net;
using System.Net.Sockets;
using System.Text;

using BenQGuru.eMES.Common.DCT.Core;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.Common.DCT.PC
{
    public class PCClient : IDCTClient
    {
        private BaseDCTAction _CachedAction = null;
        private SQLDomainDataProvider _DataProvider = null;
        private DCTFacade _DCTFacade = null;

        #region �ֶ�

        //ip��ַ
        protected string _ClientAddress = String.Empty;
        //�˿�
        protected int _ClientPort = 54321;
        //����
        protected string _ClientDesc = String.Empty;
        //ID
        protected int _ClientID;
        //״̬
        protected PCClientStatus _ClientStatus;
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
                BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider()
                    as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;

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
        public PCClientStatus ClientStatus
        {
            get
            {
                if (_DataProvider == null)
                {
                    _ClientStatus = PCClientStatus.Closed;
                }
                else
                {
                    _ClientStatus = PCClientStatus.Connecting;
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
                _CachedAction = value;
            }
            get
            {
                return _CachedAction;
            }
        }

        public SQLDomainDataProvider DataProvider
        {
            set
            {
                _DataProvider = value;
            }
            get
            {
                return _DataProvider;
            }
        }
        #endregion

        private string _LocalAddress = String.Empty;
        private int _LocalPort = 54321;

        public PCClient(string ip, int port)
        {
            _ClientAddress = ip;
            _ClientPort = port;

            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            _LocalAddress = addressList[0].ToString();
            _LocalPort = port;
        }

        public event EventCommandHandler OnSendData;

        public event EventCommandHandler OnError;

        public event EventCommandHandler AfterLogin;

        #region ����

        public void Open()
        {
            try
            {
                if (_DataProvider == null)
                {
                    _DataProvider = (SQLDomainDataProvider)DomainDataProviderManager.DomainDataProvider();
                }

                if (_DCTFacade == null)
                {
                    _DCTFacade = new DCTFacade(_DataProvider);

                    _DCTFacade.ClearAllDCTMessage();

                    this.SendCommand(DCTCommand.ClearText);
                    this.SendCommand(DCTCommand.SpeakerOff);
                    this.SendCommand(DCTCommand.AutoReportingOff);
                    this.SendCommand(DCTCommand.AutoReportingOn);
                    this.SendCommand(DCTCommand.HostReportSetting);
                    this.SendCommand(DCTCommand.ClearText);
                    this.SendCommand(DCTCommand.ClearGraphic);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Close()
        {
            _DCTFacade = null;
            _DataProvider = null;
        }

        public string SendCommand(DCTCommand command)
        {
            Open();

            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

            DCTMessage dctMessage = _DCTFacade.CreateNewDCTMessage();
            dctMessage.FromAddress = _LocalAddress;
            dctMessage.FromPort = _LocalPort;
            dctMessage.ToAddress = _ClientAddress;
            dctMessage.ToPort = _ClientPort;
            dctMessage.Direction = DCTMessageDirection.ServerToClient;
            dctMessage.MessageType = DCTMessageType.Command;
            dctMessage.MessageContent = command.ToString();
            dctMessage.Status = DCTMessageStatus.New;
            dctMessage.MaintainUser = "DCTControlPanel";
            dctMessage.MaintainDate = dbDateTime.DBDate;
            dctMessage.MaintainTime = dbDateTime.DBTime;
            this._DCTFacade.AddDCTMessage(dctMessage);

            return command.ToString();
        }

        public string SendMessage(string msg, int line)
        {
            return SendMessage(msg);
        }

        public string SendMessage(string msg)
        {
            Open();

            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

            DCTMessage dctMessage = _DCTFacade.CreateNewDCTMessage();
            dctMessage.FromAddress = _LocalAddress;
            dctMessage.FromPort = _LocalPort;
            dctMessage.ToAddress = _ClientAddress;
            dctMessage.ToPort = _ClientPort;
            dctMessage.Direction = DCTMessageDirection.ServerToClient;
            dctMessage.MessageType = DCTMessageType.Message;
            dctMessage.MessageContent = msg;
            dctMessage.Status = DCTMessageStatus.New;
            dctMessage.MaintainUser = "DCTControlPanel";
            dctMessage.MaintainDate = dbDateTime.DBDate;
            dctMessage.MaintainTime = dbDateTime.DBTime;
            _DCTFacade.AddDCTMessage(dctMessage);

            return msg;
        }

        public DCTMessage RecieveData()
        {
            object[] list = _DCTFacade.QueryNewDCTMessage(DCTMessageDirection.ClientToServer, _LocalAddress,_LocalPort, 1);
            if (list == null)
            {
                _RecievedData = string.Empty;
                return null;
            }
            else
            {
                DCTMessage dctMessage= (DCTMessage)list[0];
                _RecievedData = dctMessage.MessageContent;
                dctMessage.Status = DCTMessageStatus.Dealed;
                _DCTFacade.UpdateDCTMessage(dctMessage);
                return dctMessage;
            }
        }

        public override string ToString()
        {
            return _RecievedData;
        }

        public bool IsAlive()
        {
            _IsAlive = true;
            return _IsAlive;
        }

        public void ClearRecievedData()
        {
            _RecievedData = string.Empty;
        }

        public void ClearScreen()
        {
            SendCommand(DCTCommand.ClearText);
            SendCommand(DCTCommand.ClearUserInput);
        }

        #endregion
    }
}
