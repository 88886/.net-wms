using System;
using UserControl;

namespace BenQGuru.eMES.Agent
{
	/// <summary>
	/// IAgentCollect ��ժҪ˵����
	/// </summary>
	public interface IAgentAction
	{
		//ִ�вɼ�
		UserControl.Messages CollectExcute(string filePath,string encoding);
		
		//Good�ɼ�
		UserControl.Messages GoodCollect(object[] parserObjs);
		
		//NG�ɼ�
		UserControl.Messages NGCollect(object[] parserObjs);
	}
}
