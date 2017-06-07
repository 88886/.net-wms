using System;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// ApplicationRun ��ժҪ˵����
	/// Laws Lu,2005/08/01,�޸�GetInfoForm����
	/// </summary>
	/// 
	public class ApplicationRun
	{
		[System.Runtime.InteropServices.DllImport("User32.dll")]
		public static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
		[System.Runtime.InteropServices.DllImport("User32.dll")]
		public static extern bool SetForegroundWindow(System.IntPtr hWnd);

		public ApplicationRun()
		{
		}
		public static ApplicationContext applicationContext;
		private static FSplashForm splashForm = new FSplashForm();
		/// <summary>
		/// Ӧ�ó��������ڵ㡣
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.ThreadException +=  new System.Threading.ThreadExceptionEventHandler(ApplicationRun.otherException); 
			Application.ThreadExit += new System.EventHandler(ApplicationRun.Exit);

			applicationContext = new ApplicationContext();      		
			Application.Idle += new EventHandler(OnAppIdle);	
			splashForm.Show();
			Application.Run(applicationContext);
			

			/*
			Application.ThreadException +=  new System.Threading.ThreadExceptionEventHandler(ApplicationRun.otherException); 
			Application.ThreadExit += new System.EventHandler(ApplicationRun.Exit);

			try
			{
				if(System.Configuration.ConfigurationSettings.AppSettings["NTier"] != null
					&& System.Configuration.ConfigurationSettings.AppSettings["NTier"] == "1")
				{
					RemotingConfiguration.Configure("BenQGuru.eMES.Client.exe.config");
				}
			}
			catch(Exception ex)
			{
				string errorInfo = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")//error date & time
					+ "\t" + ex.Message //error message
					+ "\t" + ex.Source	//error object name;
					+ "\r\n" + ex.StackTrace;

				UserControl.FileLog.FileLogOut("Client.log",errorInfo);
			}

			System.Diagnostics.Process pr = RunningInstance();
			if(pr == null)
			{
				Application.Run(new FMain());
			}
			else
			{
				HandleRunningInstance(pr);
			}
							
			
			//Application.Run(new FCollectionOQC());
			//Application.Run(new Form1());
			*/
		}
		
		/// <summary>
		/// ���Խ������ݿ����ӣ��ӿ��¼ʱ���ٶ�
		/// </summary>
		private static void TryConnectDB()
		{
			try
			{
				((BenQGuru.eMES.Common.PersistBroker.SqlPersistBroker)Service.ApplicationService.Current().DataProvider).OpenConnection();
				((BenQGuru.eMES.Common.PersistBroker.SqlPersistBroker)Service.ApplicationService.Current().DataProvider).CloseConnection();
			}
			catch {}
		}
		private static void OnAppIdle(object sender, EventArgs e)
		{
			if(applicationContext.MainForm == null)
			{
				Application.Idle -= new EventHandler(OnAppIdle);
			
				System.Threading.Thread threadDb = new System.Threading.Thread(
					new System.Threading.ThreadStart(TryConnectDB));
				threadDb.Priority = System.Threading.ThreadPriority.BelowNormal;
				threadDb.Start();
				
				try
				{
					if(System.Configuration.ConfigurationSettings.AppSettings["NTier"] != null
						&& System.Configuration.ConfigurationSettings.AppSettings["NTier"] == "1")
					{
						RemotingConfiguration.Configure("BenQGuru.eMES.Client.exe.config");
					}
				}
				catch(Exception ex)
				{
					string errorInfo = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")//error date & time
						+ "\t" + ex.Message //error message
						+ "\t" + ex.Source	//error object name;
						+ "\r\n" + ex.StackTrace;

					UserControl.FileLog.FileLogOut("Client.log",errorInfo);
				}

				System.Diagnostics.Process pr = null;	//RunningInstance();
				if(pr == null)
				{
					UserControl.FileLog.FileLogOut("Client.log", System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "\t" + "New FLogin");
					FLogin loginForm = new FLogin();
					UserControl.FileLog.FileLogOut("Client.log", System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "\t" + "Show FLogin");
					loginForm.Show();
					applicationContext.MainForm = loginForm;
				}
				else
				{
					HandleRunningInstance(pr);
				}
				splashForm.Close();
				splashForm = null;
			}
		}

		public static System.Diagnostics.Process RunningInstance() 
		{ 
			try
			{
				System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess(); 
				System.Diagnostics.Process[] processes =  System.Diagnostics.Process.GetProcesses();
				//			try
				//			{
				//				System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName (current.ProcessName); 
				//			}
				//			catch{}
				//������ͬ���ƵĽ��� 
				foreach (System.Diagnostics.Process process in processes) 
				{ 
					//���Ե�ǰ���� 
					try
					{
						if (process.Id != current.Id) 
						{ 
							//string a = process.MainModule.FileName;
							//ȷ����ͬ���̵ĳ�������λ���Ƿ�һ��.
							string moduleFileName = null;
					
							moduleFileName = process.MainModule.FileName;
					
					
							if(moduleFileName != null && moduleFileName == current.MainModule.FileName)
							{
								//					if (System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName) 
								//					{ 
								//Return the other process instance. 
								return process; 
							}
						}
					}
					catch{}
				} 
			}
			catch{}
			//No other instance was found, return null. 
			return null; 
		}

		public static System.Diagnostics.Process RunningInstance(string processName) 
		{ 
			try
			{
				//System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess(); 
				System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();// (processName); 
				//������ͬ���ƵĽ��� 
				foreach (System.Diagnostics.Process process in processes) 
				{ 
					//���Ե�ǰ���� 
					//				if (process.Id != current.Id) 
					//				{ 
					//string a = process.MainModule.FileName;
					//ȷ����ͬ���̵ĳ�������λ���Ƿ�һ��. 
					string moduleFileName = null;
					try
					{
						moduleFileName = process.MainModule.FileName;
					}
					catch{}
					if(moduleFileName != null && moduleFileName.IndexOf(processName) >= 0)
					{
						//					if (process.MainModule.FileName.Location.Replace("/", "\\") == current.MainModule.FileName) 
						//					{ 
						//Return the other process instance. 
						//			if(processes.Length > 0)
						//			{
						return process; 
					}
					//					}
					//				} 
				} 
			}
			catch{}
			//No other instance was found, return null. 
			return null; 
		}

		private static void HandleRunningInstance(System.Diagnostics.Process instance)
		{
			//MessageBox.Show("��Ӧ��ϵͳ�Ѿ������У�","��ʾ��Ϣ",MessageBoxButtons.OK,MessageBoxIcon.Information);
			try
			{
				ShowWindowAsync(instance.MainWindowHandle,3); //����api�����������ʾ����
				SetForegroundWindow(instance.MainWindowHandle); //�����ڷ�����ǰ�ˡ�
			}
			catch{}
		}
		


		private static void otherException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			if(e.Exception.Source.Trim() != "Infragistics.Win.UltraWinGrid.v3.2"
				/* added by jessie lee, 2006-6-14, �޸ĵ�½ʱ�ı��� */
				&& e.Exception.Source.Trim() != "Infragistics.Win.UltraWinExplorerBar.v3.2")
			{
				ApplicationRun.GetInfoForm().Add("$CS_System_Error:"+e.Exception.Message);

				string errorInfo = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")//error date & time
					+ "\t" + e.Exception.Message //error message
					+ "\t" + e.Exception.Source;	//error object name;

				UserControl.FileLog.FileLogOut("Client.log",errorInfo);
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)
					BenQGuru.eMES.Client.Service.ApplicationService.Current().DataProvider).PersistBroker.CloseConnection();
			}
		}

		private static void Exit(object sender, System.EventArgs e)
		{
			try
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)
					BenQGuru.eMES.Client.Service.ApplicationService.Current().DataProvider).PersistBroker.CloseConnection();
			}
			catch{}
		}

		//Amoi,Laws Lu,2005/08/01,�޸�	��MessageForm�ƶ���ϵͳ�����Panel����ʾ
		private static FInfoForm infoForm=null;
		public static FInfoForm GetInfoForm()
		{   
			if (infoForm==null)
			{
				BenQGuru.eMES.Client.Service.ApplicationService.Current().MianWindows.MessageForm.Controls.Clear();
				BenQGuru.eMES.Client.Service.ApplicationService.Current().MianWindows.MessageForm.Refresh();
				infoForm = new FInfoForm();
				infoForm.TopLevel = false;
				infoForm.Height = BenQGuru.eMES.Client.Service.ApplicationService.Current().MianWindows.MessageForm.Height;
				infoForm.Width = BenQGuru.eMES.Client.Service.ApplicationService.Current().MianWindows.MessageForm.Width;
				infoForm.Dock = DockStyle.Fill;
				BenQGuru.eMES.Client.Service.ApplicationService.Current().MianWindows.MessageForm.Controls.Add(infoForm);

				infoForm.Show();
			}
			else
			{
				//Amoi,Laws Lu,2005/08/01,ע��
//				if (!infoForm.Visible) 
//				{
//					infoForm=new FInfoForm();
//					infoForm.Show();
//				}
				//EndAmoi
				infoForm.Focus();
				infoForm.Show();
			}
			return infoForm;
		}
		//EndAmoi
	}
}
