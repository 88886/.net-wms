using System;

namespace BenQGuru.eMES.Client.Command
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
