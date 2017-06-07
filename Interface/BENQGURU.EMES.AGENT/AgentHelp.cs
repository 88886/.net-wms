using System;
using UserControl;

namespace BenQGuru.eMES.Agent
{
	/// <summary>
	/// AgentHelp ��ժҪ˵����
	/// </summary>
	public class AgentHelp
	{
		public AgentHelp()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		public static string GetErrorMessage(Messages messages)
		{
			for (int i =0 ;i<messages.Count();i++)
			{
				Message message= messages.Objects(i);
				if (message.Type ==MessageType.Error)
				{
					if (message.Body==string.Empty)
						return MutiLanguages.ParserMessage(message.Exception.Message);
					else
						return MutiLanguages.ParserMessage(message.Body);
				}
				
			}
			return MutiLanguages.ParserMessage("$CS_System_unKnowError");
		}

		public static string getResCode(string code)
		{
			//��Դ�������������ǰ���ģ�����
			if(AgentInfo.Module == "AOI")
			{
				return "AOI"+code;
			}
			else if(AgentInfo.Module == "ICT")
			{
				return "ICT"+code;
			}

			return code;
		}
		
	}
}
