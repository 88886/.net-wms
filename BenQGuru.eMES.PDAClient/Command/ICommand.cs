using System;

namespace BenQGuru.eMES.PDAClient.Command
{
	/// <summary>
	/// ICommand ��ժҪ˵����
	/// </summary>
	public interface ICommand
	{
		void Execute();
		void Update();
	}
}
