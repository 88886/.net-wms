using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace BenQGuru.eMES.Web.Helper
{

	public class ESmtpMail
	{

		public ESmtpMail()
		{
		}

		~ESmtpMail()
		{
			if (ns != null)
				ns.Close();
			if (tc != null)
				tc.Close();
		}

		/// <summary>
		/// �趨���Դ��룬Ĭ���趨ΪGB2312���粻��Ҫ������Ϊ""
		/// </summary>
		public string Charset="GB2312";

		/// <summary>
		/// �����˵�ַ
		/// </summary>
		public string From="";

		/// <summary>
		/// ����������
		/// </summary>
		public string FromName="";

		/// <summary>
		/// �ظ��ʼ���ַ
		/// </summary>
		public string ReplyTo="";

		/// <summary>
		/// �ʼ�����������
		/// </summary>	
		private string mailserver="";

		/// <summary>
		/// �ʼ���������������֤��Ϣ
		/// ���磺"user:pass@www.server.com:25"��Ҳ��ʡ�Դ�Ҫ��Ϣ����"user:pass@www.server.com"��"www.server.com"
		/// </summary>	
		public string MailDomain
		{
			set
			{
				string maidomain=value.Trim();
				int tempint;

				if(maidomain!="")
				{
					tempint=maidomain.IndexOf("@");
					if(tempint!=-1)
					{
						string up=maidomain.Substring(0,tempint);
						MailServerUserName=up.Substring(0,up.IndexOf(":"));
						MailServerPassWord=up.Substring(up.IndexOf(":")+1,up.Length-up.IndexOf(":")-1);
						maidomain=maidomain.Substring(tempint+1,maidomain.Length-tempint-1);
					}

					tempint=maidomain.IndexOf(":");
					if(tempint!=-1)
					{
						mailserver=maidomain.Substring(0,tempint);
						mailserverport=System.Convert.ToInt32(maidomain.Substring(tempint+1,maidomain.Length-tempint-1));
					}
					else
					{
						mailserver=maidomain;

					}

					
				}

			}
		}

		/// <summary>
		/// �ʼ��������˿ں�
		/// </summary>	
		private int mailserverport=25;

		/// <summary>
		/// �ʼ��������˿ں�
		/// </summary>	
		public int MailDomainPort
		{
			set
			{
				mailserverport=value;
			}
		}


		/// <summary>
		/// �Ƿ���ҪSMTP��֤
		/// </summary>		
		private bool ESmtp=false;

		/// <summary>
		/// SMTP��֤ʱʹ�õ��û���
		/// </summary>
		private string username="";

		/// <summary>
		/// SMTP��֤ʱʹ�õ��û���
		/// </summary>
		public string MailServerUserName
		{
			set
			{
				if(value.Trim()!="")
				{
					username=value.Trim();
					ESmtp=true;
				}
				else
				{
					username="";
					ESmtp=false;
				}
			}
		}

		/// <summary>
		/// SMTP��֤ʱʹ�õ�����
		/// </summary>
		private string password="";

		/// <summary>
		/// SMTP��֤ʱʹ�õ�����
		/// </summary>
		public string MailServerPassWord
		{
			set
			{
				password=value;
			}
		}	

		/// <summary>
		/// �ʼ�����
		/// </summary>		
		public string Subject="";
        
		/// <summary>
		/// �Ƿ�Html�ʼ�
		/// </summary>		
		public bool Html=false;

		/// <summary>
		/// �ʼ�����
		/// </summary>		
		public string Body="";

		/// <summary>
		/// �ռ����б�
		/// </summary>
		private Hashtable Recipient=new Hashtable();
		private Hashtable RecipientName = new Hashtable();

		/// <summary>
		/// �����ռ����б�
		/// </summary>
		private Hashtable RecipientBCC=new Hashtable();
		private Hashtable RecipientBCCName = new Hashtable();

		/// <summary>
		/// �ʼ��������ȼ���������Ϊ"High","Normal","Low"��"1","3","5"
		/// </summary>
		private string priority="Normal";

		/// <summary>
		/// �ʼ��������ȼ���������Ϊ"High","Normal","Low"��"1","3","5"
		/// </summary>
		public string Priority
		{
			set
			{
				switch(value.ToLower())
				{
					case "high":
						priority="High";
						break;

					case "1":
						priority="High";
						break;

					case "normal":
						priority="Normal";
						break;

					case "3":
						priority="Normal";
						break;

					case "low":
						priority="Low";
						break;

					case "5":
						priority="Low";
						break;

					default:
						priority="Normal";
						break;
				}
			}
		}


		/// <summary>
		/// ������Ϣ����
		/// </summary>
		private string errmsg;

		/// <summary>
		/// ������Ϣ����
		/// </summary>		
		public string ErrorMessage
		{
			get
			{
				return errmsg;
			}
		}



		/// <summary>
		/// ������������¼
		/// </summary>
		private string logs="";

		/// <summary>
		/// ������������¼
		/// </summary>
		public string Logs
		{
			get
			{
				return logs;
			}
		}
		

		private string enter="\r\n";
		/// <summary>
		/// TcpClient�����������ӷ�����
		/// </summary>	
		private TcpClient tc;

		/// <summary>
		/// NetworkStream����
		/// </summary>	
		private NetworkStream ns;

		/// <summary>
		/// SMTP��������ϣ��
		/// </summary>
		private Hashtable ErrCodeHT = new Hashtable();

		/// <summary>
		/// SMTP��ȷ�����ϣ��
		/// </summary>
		private Hashtable RightCodeHT = new Hashtable();

		/// <summary>
		/// SMTP��Ӧ�����ϣ��
		/// </summary>
		private void SMTPCodeAdd()
		{
			ErrCodeHT.Add("500","�����ַ����");
			ErrCodeHT.Add("501","������ʽ����");
			ErrCodeHT.Add("502","�����ʵ��");
			ErrCodeHT.Add("503","��������ҪSMTP��֤");
			ErrCodeHT.Add("504","�����������ʵ��");
			ErrCodeHT.Add("421","����δ�������رմ����ŵ�");
			ErrCodeHT.Add("450","Ҫ����ʼ�����δ��ɣ����䲻���ã����磬����æ��");
			ErrCodeHT.Add("550","Ҫ����ʼ�����δ��ɣ����䲻���ã����磬����δ�ҵ����򲻿ɷ��ʣ�");
			ErrCodeHT.Add("451","����Ҫ��Ĳ�������������г���");
			ErrCodeHT.Add("551","�û��Ǳ��أ��볢��<forward-path>");
			ErrCodeHT.Add("452","ϵͳ�洢���㣬Ҫ��Ĳ���δִ��");
			ErrCodeHT.Add("552","�����Ĵ洢���䣬Ҫ��Ĳ���δִ��");
			ErrCodeHT.Add("553","�����������ã�Ҫ��Ĳ���δִ�У����������ʽ����");
			ErrCodeHT.Add("432","��Ҫһ������ת��");
			ErrCodeHT.Add("534","��֤���ƹ��ڼ�");
			ErrCodeHT.Add("538","��ǰ�������֤������Ҫ����");
			ErrCodeHT.Add("454","��ʱ��֤ʧ��");
			ErrCodeHT.Add("530","��Ҫ��֤");

			RightCodeHT.Add("220","�������");
			RightCodeHT.Add("250","Ҫ����ʼ��������");
			RightCodeHT.Add("251","�û��Ǳ��أ���ת����<forward-path>");
			RightCodeHT.Add("354","��ʼ�ʼ����룬��<CRLF>.<CRLF>����");
			RightCodeHT.Add("221","����رմ����ŵ�");
			RightCodeHT.Add("334","��������Ӧ��֤Base64�ַ���");
			RightCodeHT.Add("235","��֤�ɹ�");
		}


		/// <summary>
		/// ���ַ�������ΪBase64�ַ���
		/// </summary>
		/// <param name="estr">Ҫ������ַ���</param>
		private string Base64Encode(string estr)
		{
			byte[] barray;
			barray=Encoding.Default.GetBytes(estr);
			return Convert.ToBase64String(barray);
		}


		/// <summary>
		/// ��Base64�ַ�������Ϊ��ͨ�ַ���
		/// </summary>
		/// <param name="dstr">Ҫ������ַ���</param>
		private string Base64Decode(string dstr)
		{
			byte[] barray;
			barray=Convert.FromBase64String(dstr);
			return Encoding.Default.GetString(barray);
		}

		/// <summary>
		/// �ռ�������
		/// </summary>	
//		public string RecipientName="";


		private int RecipientNum=0;//�ռ�������
		private int RecipientBCCNum=0;//�ܼ��ռ�������

		/// <summary>
		/// ���һ���ռ���
		/// </summary>	
		/// <param name="str">�ռ��˵�ַ</param>
		public bool AddRecipient(string mail)
		{
			return AddRecipient(mail, mail);
		}
		public bool AddRecipient(string name, string mail)
		{
			name = name.Trim();
			mail=mail.Trim();
			if(mail==null||mail==""||mail.IndexOf("@")==-1)
				return true;
			Recipient.Add(RecipientNum,mail);
			RecipientName.Add(RecipientNum, name);
			RecipientNum++;
			return true;
		}

		/// <summary>
		/// ���һ���ܼ��ռ���
		/// </summary>
		/// <param name="str">�ռ��˵�ַ</param>
		public bool AddRecipientBCC(string mail)
		{
			return AddRecipientBCC(mail, mail);
		}
		public bool AddRecipientBCC(string name, string mail)
		{
			name = name.Trim();
			mail=mail.Trim();
			if(mail==null||mail==""||mail.IndexOf("@")==-1)
				return true;
			RecipientBCC.Add(RecipientBCCNum,mail);
			RecipientBCCName.Add(RecipientBCCNum, name);
			RecipientBCCNum++;
			return true;
		}

		/// <summary>
		/// ����SMTP����
		/// </summary>	
		private bool SendCommand(string Command)
		{
			byte[]  WriteBuffer;
			if(Command==null||Command.Trim()=="")
			{
				return true;
			}
			logs+=Command;
			WriteBuffer = Encoding.Default.GetBytes(Command);
			try
			{
				ns.Write(WriteBuffer,0,WriteBuffer.Length);
			}
			catch
			{
				errmsg="�������Ӵ���";
				return false;
			}
			return true;
		}

		/// <summary>
		/// ����SMTP��������Ӧ
		/// </summary>
		private string RecvResponse()
		{
			int StreamSize;
			string ReturnValue = "";
			byte[]  ReadBuffer = new byte[1024] ;
			try
			{
				StreamSize=ns.Read(ReadBuffer,0,ReadBuffer.Length);
			}
			catch
			{
				errmsg="�������Ӵ���";
				return "false";
			}

			if (StreamSize==0)
			{
				return ReturnValue ;
			}
			else
			{
				ReturnValue = Encoding.Default.GetString(ReadBuffer).Substring(0,StreamSize);
				logs+=ReturnValue;
				return  ReturnValue;
			}
		}


		/// <summary>
		/// �����������������һ��������ջ�Ӧ��
		/// </summary>
		/// <param name="Command">һ��Ҫ���͵�����</param>
		/// <param name="errstr">�������Ҫ��������Ϣ</param>
		private bool Dialog(string Command,string errstr)
		{
			if(Command==null||Command.Trim()=="")
			{
				return true;
			}
			if(SendCommand(Command))
			{
				string RR=RecvResponse();
				if(RR=="false")
				{
					return false;
				}
				string RRCode=RR.Substring(0,3);
				if(RightCodeHT[RRCode]!=null)
				{
					return true;
				}
				else
				{
					if(ErrCodeHT[RRCode]!=null)
					{
						errmsg+=(RRCode+ErrCodeHT[RRCode].ToString());
						errmsg+=enter;
					}
					else
					{
						errmsg+=RR;
					}
					errmsg+=errstr;
					return false;
				}
			}
			else
			{
				return false;
			}

		}


		/// <summary>
		/// �����������������һ��������ջ�Ӧ��
		/// </summary>

		private bool Dialog(string[] Command,string errstr)
		{
			for(int i=0;i<Command.Length;i++)
			{
				if(!Dialog(Command[i],""))
				{
					errmsg+=enter;
					errmsg+=errstr;
					return false;
				}
			}

			return true;
		}



		private bool SendEmail()
		{
			//��������
			try
			{
				tc=new TcpClient(mailserver,mailserverport);
			}
			catch(Exception e)
			{
				errmsg=e.ToString();
				return false;
			}

			ns = tc.GetStream();
			SMTPCodeAdd();

			//��֤���������Ƿ���ȷ
			if(RightCodeHT[RecvResponse().Substring(0,3)]==null)
			{
				errmsg="��������ʧ��";
				return false;
			}


			string[] SendBuffer;
			string SendBufferstr;

			//����SMTP��֤
			if(ESmtp)
			{
				SendBuffer=new String[4];
				SendBuffer[0]="EHLO " + mailserver + enter;
				SendBuffer[1]="AUTH LOGIN" + enter;
				SendBuffer[2]=Base64Encode(username) + enter;
				SendBuffer[3]=Base64Encode(password) + enter;
				if(!Dialog(SendBuffer,"SMTP��������֤ʧ�ܣ���˶��û��������롣"))
					return false;
			}
			else
			{
				SendBufferstr="HELO " + mailserver + enter;
				if(!Dialog(SendBufferstr,""))
					return false;
			}

			//
			SendBufferstr="MAIL FROM:<" + From + ">" + enter;
			if(!Dialog(SendBufferstr,"�����˵�ַ���󣬻���Ϊ��"))
				return false;

			//
			SendBuffer=new string[Recipient.Count];
			for(int i=0;i<Recipient.Count;i++)
			{

				SendBuffer[i]="RCPT TO: <" + Recipient[i].ToString() +">" + enter;

			}
			if(!Dialog(SendBuffer,"�ռ��˵�ַ����"))
				return false;

			SendBuffer=new string[RecipientBCC.Count];
			for(int i=0;i<RecipientBCC.Count;i++)
			{

				SendBuffer[i]="RCPT TO:<" + RecipientBCC[i].ToString() +">" + enter;

			}
			if(!Dialog(SendBuffer,"�ܼ��ռ��˵�ַ����"))
				return false;

			SendBufferstr="DATA" + enter;
			if(!Dialog(SendBufferstr,""))
				return false;

			SendBufferstr="From:" + "=?" + Charset.ToUpper() + "?B?" + Base64Encode(FromName) +"?=" + "<" + From +">" +enter;

			if(ReplyTo.Trim()!="")
			{
				SendBufferstr+="Reply-To: " + ReplyTo + enter;
			}

			SendBufferstr+="To:";
			for (int i = 0; i < Recipient.Count; i++)
			{
				SendBufferstr += "=?" + Charset.ToUpper() + "?B?" + Base64Encode(RecipientName[i].ToString()) +"?=" + "<" + Recipient[i].ToString() + ">,";
			}
			
			SendBufferstr+="CC:";
			for(int i=1;i<RecipientBCC.Count;i++)
			{
				SendBufferstr+=RecipientBCC[i].ToString() + "<" + RecipientBCC[i].ToString() +">,";
			}
			SendBufferstr+=enter;


			if(Charset=="")
			{
				SendBufferstr+="Subject:" + Subject + enter;
			}
			else
			{
				SendBufferstr+="Subject:" + "=?" + Charset.ToUpper() + "?B?" + Base64Encode(Subject) +"?=" +enter;
			}

			SendBufferstr+="X-Priority:" + priority + enter;
			SendBufferstr+="X-MSMail-Priority:" + priority + enter;
			SendBufferstr+="Importance:" + priority + enter;
			SendBufferstr+="X-Mailer: BenQGuru.eMES" + enter;
			SendBufferstr+="MIME-Version: 1.0" + enter;

			if(Html)
			{
				SendBufferstr+="Content-Type: text/html;" + enter;
			}
			else
			{
				SendBufferstr+="Content-Type: text/plain;" + enter;
			}

			if(Charset=="")
			{
				SendBufferstr+="	charset=\"iso-8859-1\"" + enter;
			}
			else
			{
				SendBufferstr+="	charset=\"" + Charset.ToLower() + "\"" + enter;
			}

			SendBufferstr+="Content-Transfer-Encoding: base64" + enter;

			SendBufferstr+= enter + enter;
			SendBufferstr+= Base64Encode(Body) + enter;
			SendBufferstr+=enter + "." + enter;

			if(!Dialog(SendBufferstr,"�����ż���Ϣ"))
				return false;


			SendBufferstr="QUIT" + enter;
			if(!Dialog(SendBufferstr,"�Ͽ�����ʱ����"))
				return false;


			ns.Close();
			tc.Close();
			return true;
		}


		/// <summary>
		/// �����ʼ����������в�����ͨ���������á�
		/// </summary>
		public bool Send()
		{
			if(Recipient.Count==0)
			{
				errmsg="�ռ����б���Ϊ��";
				return false;
			}

			if(mailserver.Trim()=="")
			{
				errmsg="����ָ��SMTP������";
				return false;
			}

			return SendEmail();
			
		}


		/// <summary>
		/// �����ʼ�����
		/// </summary>
		/// <param name="smtpserver">smtp��������Ϣ����"username:password@www.smtpserver.com:25"��Ҳ��ȥ�����ִ�Ҫ��Ϣ����"www.smtpserver.com"</param>
		public bool Send(string smtpserver)
		{
			
			MailDomain=smtpserver;
			return Send();
		}


		/// <summary>
		/// �����ʼ�����
		/// </summary>
		/// <param name="smtpserver">smtp��������Ϣ����"username:password@www.smtpserver.com:25"��Ҳ��ȥ�����ִ�Ҫ��Ϣ����"www.smtpserver.com"</param>
		/// <param name="from">������mail��ַ</param>
		/// <param name="fromname">����������</param>
		/// <param name="replyto">�ظ��ʼ���ַ</param>
		/// <param name="to">�ռ��˵�ַ</param>
		/// <param name="toname">�ռ�������</param>
		/// <param name="html">�Ƿ�HTML�ʼ�</param>
		/// <param name="subject">�ʼ�����</param>
		/// <param name="body">�ʼ�����</param>
		public bool Send(string smtpserver,string from,string fromname,string replyto,string to,string toname,bool html,string subject,string body)
		{
			MailDomain=smtpserver;
			From=from;
			FromName=fromname;
			ReplyTo=replyto;
			AddRecipient(toname, to);
			Html=html;
			Subject=subject;
			Body=body;
			return Send();
		}
	}
}
