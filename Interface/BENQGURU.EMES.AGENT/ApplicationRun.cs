using System;
using System.Windows.Forms;

namespace BenQGuru.eMES.Agent
{
	/// <summary>
	/// ApplicationRun ��ժҪ˵����
	/// </summary>
	public class ApplicationRun
	{
		[System.Runtime.InteropServices.DllImport("User32.dll")]
		private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
		[System.Runtime.InteropServices.DllImport("User32.dll")]
		private static extern bool SetForegroundWindow(System.IntPtr hWnd);

		public ApplicationRun()
		{
		}
		/// <summary>
		/// Ӧ�ó��������ڵ㡣
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.ThreadException +=  new System.Threading.ThreadExceptionEventHandler(ApplicationRun.otherException); 
			Application.ThreadExit += new System.EventHandler(ApplicationRun.Exit);

			
			System.Threading.Mutex appSingleton = new System.Threading.Mutex(false,"Agent");
			if(appSingleton.WaitOne(0,false))
			{
				Application.Run(new CollectAgent());
			}
			else
			{
				MessageBox.Show("�Ѿ���һ��AgentӦ�ó��������С�\n���ȷ���˳�","Agent ���о���");
			}
			appSingleton.Close();
			
		}

		public static System.Diagnostics.Process RunningInstance() 
		{ 
			//			System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess(); 
			//			System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName (current.ProcessName); 
			//			//������ͬ���ƵĽ��� 
			//			foreach (System.Diagnostics.Process process in processes) 
			//			{ 
			//				//���Ե�ǰ���� 
			//				if (process.Id != current.Id) 
			//				{ 
			//					//string a = process.MainModule.FileName;
			//					//ȷ����ͬ���̵ĳ�������λ���Ƿ�һ��. 
			//					if(process.MainModule.FileName == current.MainModule.FileName)
			//					{
			////					if (System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName) 
			////					{ 
			//						//Return the other process instance. 
			//						return process; 
			//					}
			//				} 
			//			} 
			//No other instance was found, return null. 
			return null; 
		}

		private static void HandleRunningInstance(System.Diagnostics.Process instance)
		{
			//MessageBox.Show("��Ӧ��ϵͳ�Ѿ������У�","��ʾ��Ϣ",MessageBoxButtons.OK,MessageBoxIcon.Information);
			ShowWindowAsync(instance.MainWindowHandle,3); //����api�����������ʾ����
			SetForegroundWindow(instance.MainWindowHandle); //�����ڷ�����ǰ�ˡ�
		}
		


		private static void otherException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			if(e.Exception.Source.Trim() != "Infragistics.Win.UltraWinGrid.v3.2")
			{
				ApplicationRun.GetInfoForm().Add("$CS_System_Error:"+e.Exception.Message);

				UserControl.FileLog.FileLogOut("Client.log",e.Exception.Message);
			}
		}

		private static void Exit(object sender, System.EventArgs e)
		{
		}

		private static FInfoForm infoForm=null;
		public static FInfoForm GetInfoForm()
		{   
			if (infoForm==null)
			{

				infoForm = new FInfoForm();
				infoForm.Height = 200;
				infoForm.Dock = DockStyle.Fill;

				infoForm.Show();
			}
			else
			{
				if (!infoForm.Visible) 
				{
					infoForm=new FInfoForm();
					infoForm.Show();
				}
			}
			return infoForm;
		}
	}
}
