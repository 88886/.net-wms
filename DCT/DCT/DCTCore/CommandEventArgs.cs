using System;
using System.Collections;

namespace BenQGuru.eMES.Common.DCT.Core
{
	/// <author>Laws Lu</author>
	/// <since>2006/04/12</since>
	/// <version>1.0.0</version>
	public class CommandEventArgs: System.EventArgs 
	{
		public CommandEventArgs(string msg)
		{
			this.message = msg;
		}

        static CommandEventArgs()
        {
            try
            {
                AutoReportInterval = (byte)int.Parse(System.Configuration.ConfigurationSettings.AppSettings["AutoReportInterval"]);
            }
            catch { }

            try
            {
                DeadInterval = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["DeadInterval"]);
            }
            catch { }
        }

		protected string message;

		public string Message
		{
			get
			{
				return message;
			}
			set
			{
				message = value;
			}
		}

        public static byte AutoReportInterval = 5;//joe 20070903 �Զ����������Ƿ�live��ʱ��������λ��

        public static int DeadInterval = 7;//joe 20070903 ����10��û�б�����豸������Ϊ�Ѿ��Ͽ���

    }

	/// <author>Laws Lu</author>
	/// <since>2006/04/12</since>
	/// <version>1.0.0</version>
	public enum DCTCommand
	{
		Reset,
		ClearText,
        ClearGraphic,
        ClearUserInput,
		SpeakerOn,
		SpeakerOff,
		ScrollUp,
		ScrollDown,
		AutoReportingOn,
		AutoReportingOff,
        HostReportSetting,
        HostReportPackage        
	}

	/// <author>Laws Lu</author>
	/// <since>2006/04/14</since>
	/// <version>1.0.0</version>
	public abstract class DCTCommandList
	{
		public Hashtable dctCommandList 
			= new Hashtable();


		public DCTCommandList()
		{
			InitialCommand();
		}

		public virtual void InitialCommand()
		{
//			dctCommandList.Add(DCTCommand.Reset,(new byte[]{27,0}));
//			dctCommandList.Add(DCTCommand.ClearText,(new byte[]{27,96}));
//			dctCommandList.Add(DCTCommand.SpeakerOff,(new byte[]{27,11,0}));
//			dctCommandList.Add(DCTCommand.SpeakerOn,(new byte[]{27,11,1}));
//			dctCommandList.Add(DCTCommand.ScrollUp,(new byte[]{27,103,255}));
//			dctCommandList.Add(DCTCommand.ScrollDown,(new byte[]{27,103,1}));
		}


		public byte[] GetCommand(DCTCommand commandName)
		{
			return (dctCommandList[commandName] as byte[]);
		}

	
	}
}
