using System;

namespace BenQGuru.eMES.Agent
{
	/// <summary>
	/// AgentActionDefault ��ժҪ˵����
	/// </summary>
	public class AgentActionDefault : IAgentAction	
	{
		public AgentActionDefault()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region IAgentAction ��Ա

		public UserControl.Messages CollectExcute(string filePath, string encoding)
		{
			return null;
		}

		public UserControl.Messages GoodCollect(object[] parserObjs)
		{
			return null;
		}

		public UserControl.Messages NGCollect(object[] parserObjs)
		{
			return null;
		}

		#endregion
	}
}
