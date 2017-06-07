using System;

namespace UserControl
{
	/// <summary>
	/// IWatchPanel ��ժҪ˵����
	/// </summary>
	public interface IWatchPanel
	{
		void HideHeader();
		void HideBottom();
		int HeaderHeight{get;}
		int BottomHeight{get;}
	}

	public interface ICalculateWatchPanel : IWatchPanel
	{
		void SetCalculateStrategy(ICalculateStrategy Stategy);
		void CalculateActual();
	}

	public class TimePeriodForWatchPanel
	{
		public string TPCode;

		public string TPHeader;

		//		public TimePeriodForWatchPanel(string TPCode,string TPHeader)
		//		{
		//			this.TPCode = TPCode;
		//			this.TPHeader = TPHeader;
		//		}
	}

	public interface ICalculateStrategy
	{
		decimal Calculate(decimal Input,decimal Defects,decimal FPYGoal);
	}
}
