using System;
using BenQGuru.eMES.Common.DCT.ATop.GW28;

namespace DCTSimulateServer
{
	/// <summary>
	/// Simulate ��ժҪ˵����
	/// </summary>
	class SimulateServer
	{
		static int iCount = 55962;
		/// <summary>
		/// Ӧ�ó��������ڵ㡣
		/// </summary>
		static void Main(string[] args)
		{
			for(int i = 0;i < 200;i ++)
			{
				System.Threading.ThreadStart ts = new System.Threading.ThreadStart(RunThread);
				System.Threading.Thread cc = new System.Threading.Thread(ts);

				iCount = iCount + 1;

				cc.Start();
			}
		}

		public static void RunThread()
		{
			GW28DCTDriver driver = new GW28DCTDriver();
			
			//GW28Client client = new GW28Client("10.89.58.160",iCount);
//			driver = new GW28DCTDriver();
			//
			//				client.ClientAddress = "10.89.58.161";
			//				client.ClientID = 1;
			//				client.ClientPort  = 55962;
			//				client.ClientDesc  = "����";

			driver.DCTListen(null);
		}

	}
}
