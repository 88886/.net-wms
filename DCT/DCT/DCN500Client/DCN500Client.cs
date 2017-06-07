using System;

using System.Net;
using System.Net.Sockets;
using BenQGuru.eMES.Common.DCT.Core;

namespace BenQGuru.eMES.Common.DCT.ATop.DCN500
{
	/// <summary>
	/// DCN500Client ��ժҪ˵����
	/// </summary>
	public class DCN500Client:IDCTClient
	{
		private const string IN = "  IN  ";
		private const string OUT = "  OUT  ";

		private BaseDCTAction cachedAction = null;

		#region ����
		//ip��ַ
		protected string client_ip = String.Empty;
		//�˿�
		protected int client_port = 55962;
		//����
		protected string client_description = String.Empty;
		//ID
		protected int client_id;
		//״̬
		protected DCN500ClientStatus client_status;
		//��ǰ���ܵ��ַ���
		protected string client_recieve;
		//��Դ����
		protected string client_rescode = String.Empty;
		//��½�û�
		protected string client_loginuser = String.Empty;
		//��½�û�
		protected string client_password = String.Empty;

		//ͨѶ�õ�Socket
		protected Socket socket;
		//�Ƿ��½
		protected bool client_certificate;
		//DB�����ṩ����
		protected object db_connection = null;
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
				return client_certificate;
			}
			set
			{
				client_certificate = value;
				
			}
		}
		/// <summary>
		/// ��½�û�
		/// </summary>
		public string LoginedUser
		{
			get
			{
				return client_loginuser;
			}
			set
			{
				client_loginuser = value;
			}
		}
		/// <summary>
		/// ��½�û�
		/// </summary>
		public string LoginedPassword
		{
			get
			{
				return client_password;
			}
			set
			{
				client_password = value;
			}
		}
		/// <summary>
		/// ��Դ����
		/// </summary>
		public string ResourceCode
		{
			get
			{
				return client_rescode;
			}
			set
			{
				client_rescode = value;
				if(AfterLogin != null)
				{
					AfterLogin(this,new CommandEventArgs(value));
				}
			}
		}

        private string _SegmentCode = string.Empty;
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

        private string _StepSequenceCode = string.Empty;
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

        private string _ShiftTypeCode = string.Empty;
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
				return client_recieve;
			}
			set
			{
				client_recieve = value;
			}
		}
		/// <output>����</output>
		public DCTType Type
		{
			get
			{
				return DCTType.DCN500;
			}
		}
		/// <output>IP��ַ</output>
		public string ClientAddress
		{
			get
			{
				return client_ip;
			}
			set
			{
				client_ip = value;
			}
		}

		/// <output>�˿�</output>
		public int ClientPort
		{
			get
			{
				return client_port;
			}
			set
			{
				client_port = value;
			}
		}

		/// <output>ID</output>
		public int ClientID
		{
			get
			{
				return client_id;
			}
			set
			{
				client_id = value;
			}
		}

		/// <output>����</output>
		public string ClientDesc
		{
			get
			{
				return client_description;
			}
			set
			{
				client_description = value;
			}
		}

		/// <output>״̬</output>
		public DCN500ClientStatus ClientStatus
		{
			get
			{
				if(this.socket.Connected == false)
				{
					client_status = DCN500ClientStatus.Closed;
				}
				return client_status;
			}
			set
			{
				client_status = value;
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
				return cachedAction ;
			}
		}

		#endregion

		public DCN500Client(string ip,int port)
		{
			try
			{
				if(OnSendData == null)
				{
					OnSendData += new EventCommandHandler(DCN500Client_OnSendData);
				}
				client_ip = ip;
				client_port = port;

				InitialClient();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public string DCN500Client_OnSendData(object sender,CommandEventArgs e)
		{
			return null;
		}

        public string SendMessage(string msg, int lineno)
        {
            return string.Empty;
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
				if(socket == null)
				{
					InitialClient();
				}
				if(socket.Poll(200,SelectMode.SelectError)  || socket.Connected  == false)
				{
					IPAddress ipAdd = IPAddress.Parse(client_ip);
					IPEndPoint ipEnd = new IPEndPoint(ipAdd,client_port);

					socket.Connect(ipEnd);

					this.SendCommand(DCTCommand.ClearText);

					this.client_status = DCN500ClientStatus.Connecting;

					this.SendCommand(DCTCommand.SpeakerOff);//�ص�beeper
					this.SendCommand(DCTCommand.AutoReportingOff);//�ص��Զ�����
				}
				
				//this.SendCommand(DCTCommand.ClearText);//����
				
			}
			catch(Exception ex)
			{
				if(OnError != null)
				{
					OnError(this,new CommandEventArgs(ex.Message));
				}
				socket = null;
				InitialClient();
				//throw ex;
			}
		}
	

		public void Close()
		{
			if(socket != null)
			{
				socket.Shutdown(SocketShutdown.Both);
				socket.Close();
			}
		}

		public string SendCommand(DCTCommand command)
		{
			string strReturn = String.Empty;

			if(socket == null || socket.Connected == false)
			{
				Open();
			}

			DCN500CommandList comList = new DCN500CommandList();

			socket.Send(comList.GetCommand(command));

			if(OnSendData != null)
			{
				OnSendData(this
					,new CommandEventArgs(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + OUT + command.ToString()));
			}

			return strReturn;
		}

		public string SendMessage(string msg)
		{
			string strReturn = String.Empty;

			byte[] btOriginal = System.Text.Encoding.Default.GetBytes(msg);
			byte[] btFinal = (new DCN500MessageParser()).WrapMessage(btOriginal);

			if(socket == null || socket.Connected == false)
			{
				Open();
			}
			//Laws Lu,2006/06/20	���DCT�豸����
			//socket.Send(new DCN500CommandList().GetCommand(DCTCommand.ClearText));

			socket.Send(btFinal);

			//socket.Send(new DCN500CommandList().GetCommand(DCTCommand.ScrollDown));
			
			if(OnSendData != null)
			{
				OnSendData(this
					,new CommandEventArgs(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + OUT + msg));
			}

			btOriginal = null;
			btFinal = null;

			return strReturn;
		}

		public string RecieveData()
		{
			string strReturn = String.Empty;

			byte[] btRecieve = new byte[512];
			//string strRecieve = String.Empty;
			if(socket == null || socket.Connected == false)
			{
				Open();
			}
			socket.Receive(btRecieve);

			strReturn = System.Text.Encoding.Default.GetString(
				(new DCN500MessageParser()).RetrieveData(btRecieve));
			
			client_recieve = strReturn.ToUpper().Trim();

			if(OnSendData != null)
			{
				OnSendData(this
					,new CommandEventArgs(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + IN + strReturn));
			}
			btRecieve = null;

			return strReturn.ToUpper().Trim();

		}

		/// <summary>
		/// ��ʼ��Client����
		/// </summary>
		protected void InitialClient()
		{
			if(socket == null)
			{
				socket = new Socket(
					AddressFamily.InterNetwork
					,SocketType.Stream
					,ProtocolType.IP);

				socket.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.SendTimeout,50);
			}
		}

		public override string ToString()
		{
			return this.RecievedData;
		}

		//2006/06/02 add ,loop ask for client status
		//2006/06/16 modify,confirm status use ping command
		//2006/07/01 modify,@ darfon
		public bool IsAlive()
		{
			bool bResult = true;
			try
			{
				
				if( !socket.Connected || DCN500ClientStatus.Connecting != ClientStatus)
				{
					bResult = false;
				}
				else
				{
//					if(socket.Poll(-1,SelectMode.SelectRead))
//					{
//						this.RecieveData();
//					}
					if(socket.Poll(-1,SelectMode.SelectWrite)/* || socket.Poll(-1,SelectMode.SelectError) */)
					{
						//bResult = false;
						this.SendCommand(DCTCommand.AutoReportingOn);
					}
				}
				//this.socket.Poll(200,SelectMode.SelectWrite);
//				if(!PingClass.PingHost(client_ip) || !socket.Connected || (DCN500ClientStatus.Connecting != ClientStatus))
//				{
//					bResult = false;
//					//Laws Lu,2006/06/20 change facility status
//					ClientStatus = DCN500ClientStatus.TCPConnectError;
//				}
			}
			catch(Exception ex)
			{
				bResult = false;

				//Laws Lu,2006/06/20 change facility status
				ClientStatus = DCN500ClientStatus.TCPConnectError;

				Log.Error(ex.Message);

				InitialClient();
				
			}
				
			//System.Diagnostics.Debug.Assert(bResult,"OK");
			
			return bResult;
		}

		#endregion

        public void ClearRecievedData()
        {
            this.RecievedData = "";
        }

		public event EventCommandHandler OnSendData;

		public event EventCommandHandler OnError;

		public event EventCommandHandler AfterLogin;
		
	}

	/// <author>Laws Lu</author>
	/// <since>2006/04/12</since>
	/// <version>1.0.0</version>
	public enum DCN500ClientStatus
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
