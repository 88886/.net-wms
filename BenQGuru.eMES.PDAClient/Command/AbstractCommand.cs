using System;

namespace BenQGuru.eMES.PDAClient.Command
{
	/// <summary>
	/// AbstractCommand ��ժҪ˵����
	/// </summary>
	public abstract class AbstractCommand:ICommand
	{
		#region ICommand ��Ա

		public virtual void Execute()
		{
			// TODO:  ��� AbstractCommand.Execute ʵ��
		}

		public virtual void Hint()
		{
			// TODO:  ��� AbstractCommand.Hint ʵ��
		}

		public virtual void Update()
		{
			// TODO:  ��� AbstractCommand.Update ʵ��
		}

		#endregion
	}
}
