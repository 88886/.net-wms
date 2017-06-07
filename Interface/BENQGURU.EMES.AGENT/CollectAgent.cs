using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;


namespace BenQGuru.eMES.Agent
{
	/// <summary>
	/// CollectAgent ��ժҪ˵����
	/// </summary>
	public class CollectAgent : System.Windows.Forms.Form
	{
		private bool allowClose = false;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox MsgBox;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuItem1;
		private IDomainDataProvider _domainDataProvider = DomainDataProviderManager.DomainDataProvider();

		public CollectAgent()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//

			LoadConfigXML();
			InitMsg();
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CollectAgent));
			this.MsgBox = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnClear = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// MsgBox
			// 
			this.MsgBox.Location = new System.Drawing.Point(24, 40);
			this.MsgBox.Name = "MsgBox";
			this.MsgBox.Size = new System.Drawing.Size(408, 224);
			this.MsgBox.TabIndex = 0;
			this.MsgBox.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.TabIndex = 1;
			this.label1.Text = "��ǰ�ɼ���Ϣ";
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(136, 280);
			this.btnClear.Name = "btnClear";
			this.btnClear.TabIndex = 2;
			this.btnClear.Text = "���";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenu = this.contextMenu1;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "BenQGuru Collect Agent";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "�˳�";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// CollectAgent
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(456, 318);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.MsgBox);
			this.KeyPreview = true;
			this.Name = "CollectAgent";
			this.Text = "BenQGuruCollectAgent";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.CollectAgent_Closing);
			this.Activated += new System.EventHandler(this.CollectAgent_Activated);
			this.ResumeLayout(false);

		}
		#endregion

//		/// <summary>
//		/// Ӧ�ó��������ڵ㡣
//		/// </summary>
//		[STAThread]
//		static void Main() 
//		{
//			//ֻ����һ��agent
//			System.Threading.Mutex appSingleton = new System.Threading.Mutex(false,"Agent");
//			if(appSingleton.WaitOne(0,false))
//			{
//				Application.Run(new CollectAgent());
//			}
//			else
//			{
//				MessageBox.Show("�Ѿ���һ��AgentӦ�ó��������С�\n���ȷ���˳�","Agent ���о���");
//			}
//			appSingleton.Close();
//		}

		#region ��ʱ���¼�

		//��ʱ��ִ��
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			AgentRun();
			//testMsg();
		}

		#endregion

		#region ��ʼ��Message

		private void InitMsg()
		{
			if(AgentInfo.Module != null && AgentInfo.Module != string.Empty)
			{
				UserControl.Messages msgs = new UserControl.Messages();		
			
				msgs.Add( new UserControl.Message(UserControl.MessageType.Success,string.Format("MES {0} Agent ����",AgentInfo.Module)) );
				msgs.Add( new UserControl.Message(UserControl.MessageType.Success,string.Format("MES {0} Agent ���Ŀ¼Ϊ {1}",AgentInfo.Module,AgentInfo.DirectoryPath)) );
				msgs.Add( new UserControl.Message(" "));

				ApplicationRun.GetInfoForm().Add(msgs);
			}
		}

		#endregion

		#region ������

		private void AgentRun()
		{
			CollectExcute collect = new CollectExcute(_domainDataProvider);

			try
			{
				if(AgentInfo.RunStatus	== "TRUE")
				{
					string[] fileList = this.GetFiles(AgentInfo.DirectoryPath);
					if(fileList!=null && fileList.Length>0)
					{
						foreach(string filePath in fileList)
						{
							UserControl.Messages collectMessage =  collect.Excute(AgentInfo.Module,filePath,AgentInfo.FileEncoding);
							ApplicationRun.GetInfoForm().Add(collectMessage);	//�ɼ���Ϣ��ʾ
							
							DealFile(filePath);									//�ļ�����
						}
						
					}
				}
			}
			catch
			{}
		}

		#endregion

		#region ��ȡ���ò���

		private void LoadConfigXML()
		{
			try
			{
				//��ȡ��ǰAgent������
				//��ȡ����ļ���·��
				//��ȡ�ļ�Parser
				//��ȡ�ļ�����
				string strFile = Application.StartupPath + "\\WatchFile.xml" ;
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
				doc.Load(strFile);

				System.Xml.XmlNode node = doc.SelectSingleNode(string.Format("//WatchFile"));
				if (node != null)
				{
					System.Xml.XmlNodeList selectNodes = node.SelectNodes("WatchFileInfo");
					if(selectNodes!=null && selectNodes.Count>0)
						foreach(System.Xml.XmlNode _childnode in selectNodes)
						{
							AgentInfo.RunStatus		= _childnode.Attributes["RunStatus"].Value.Trim().ToUpper();	//�Ƿ�����
							AgentInfo.Module		= _childnode.Attributes["Module"].Value.Trim().ToUpper();		//ģ��
							AgentInfo.DirectoryPath	= _childnode.Attributes["DirectoryPath"].Value.Trim();			//�������ļ�·��
							try
							{
								int pinterval = int.Parse(_childnode.Attributes["Interval"].Value.Trim());
								AgentInfo.Interval		= pinterval;			//ѭ��ִ�еļ��,����(Ĭ��ֵΪ1000)
							}
							catch
							{
							}
							AgentInfo.FileEncoding	= _childnode.Attributes["FileEncoding"].Value.Trim();		//�ļ��ı����ʽ

							// Added by Icyer 2006/08/03
							if (_childnode.Attributes["SMTLoadItem"] != null)
							{
								AgentInfo.SMTLoadItem = (_childnode.Attributes["SMTLoadItem"].Value.Trim().ToUpper() == "TRUE");
							}

							// Added by Icyer 2006/08/03
							if (_childnode.Attributes["AllowGoToMO"] != null)
							{
								AgentInfo.AllowGoToMO = (_childnode.Attributes["AllowGoToMO"].Value.Trim().ToUpper() == "TRUE");
							}
							// Added end

							if(AgentInfo.RunStatus	== "TRUE")
							{
								//��ȡ���е�module
								break;
							}
						}
				}
				node = null;
			}
			catch
			{
				ApplicationRun.GetInfoForm().Add(new UserControl.Message(UserControl.MessageType.Error,"δ�ҵ� WatchFile.xml �����ļ�"));
			}
		}
	
		#endregion

		#region ����ָ��Ŀ¼����ȡ��Ҫ������ļ�

		/// <summary>
		/// ����ָ��Ŀ¼����ȡ��Ҫ������ļ�
		/// </summary>
		/// <param name="directoryPath"> ���Ŀ¼ </param>
		/// <returns>�����ļ���ַ����</returns>
		private string[] GetFiles(string directoryPath)
		{
			string[] fileList = Directory.GetFileSystemEntries(directoryPath);	//�ļ�Ŀ¼
			return fileList;
		}

		#endregion

		#region ת���Ѿ�������ļ�

		private void DealFile(string filePath)
		{
			try
			{
				//Laws Lu,2006/12/15 �������þ����Ƿ���б����ļ��Ĳ���
				if(System.Configuration.ConfigurationSettings.AppSettings["NeedBackup"] != null &&
					System.Configuration.ConfigurationSettings.AppSettings["NeedBackup"].Trim() != String.Empty &&
					System.Configuration.ConfigurationSettings.AppSettings["NeedBackup"].Trim() == "1")
				{
					if(!System.IO.Directory.Exists(AgentInfo.BackDirectoryPath))
					{
						Directory.CreateDirectory(AgentInfo.BackDirectoryPath);
					}

					if(AgentInfo.BackDirectoryPath != null && AgentInfo.BackDirectoryPath != string.Empty)
					{
						string bakFilePath = AgentInfo.BackDirectoryPath + @"\" + Path.GetFileName(filePath); 
						File.Copy(filePath,bakFilePath,true);
					}
				}

				File.Delete(filePath);
			}
			catch(Exception ex)
			{
				Log.Error(ex.Message);
			}
		}

		#endregion

		#region ���Message

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			this.MsgBox.Clear();
		}

		#endregion

		#region notifyIcon�¼�
		private void notifyIcon1_DoubleClick(object sender, System.EventArgs e)
		{
//			if(this.WindowState == FormWindowState.Minimized)
//			{
			//Laws Lu,2006/12/21 ����˫��ͼ��������ʾ����
				this.Show();
				InitMsg();
				this.Hide();
				this.WindowState = FormWindowState.Normal;
				this.Opacity = 80;
//			}
		}
		#endregion

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			allowClose = true;
			this.Close();
		}

		private void CollectAgent_Activated(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void CollectAgent_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
			if(_domainDataProvider != null)
			{
				((SQLDomainDataProvider)_domainDataProvider).PersistBroker.CloseConnection();
			}
		}
	}
}
