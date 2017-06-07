using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using BenQGuru.eMES.Common.DCT.Core;

namespace BenQGuru.eMES.Common.DCT
{
	/// <summary>
	/// frmDCTServer ��ժҪ˵����
	/// </summary>
	public class frmDCTMonitor : System.Windows.Forms.Form
	{
		private IDCTClient _client = null;
		private System.Windows.Forms.RichTextBox txtMonitor;

		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmDCTMonitor()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		public frmDCTMonitor(IDCTClient client)
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			_client = client;
			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
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
			this.txtMonitor = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// txtMonitor
			// 
			this.txtMonitor.BackColor = System.Drawing.SystemColors.ControlText;
			this.txtMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtMonitor.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
			this.txtMonitor.ForeColor = System.Drawing.SystemColors.Info;
			this.txtMonitor.Location = new System.Drawing.Point(0, 0);
			this.txtMonitor.Name = "txtMonitor";
			this.txtMonitor.ReadOnly = true;
			this.txtMonitor.Size = new System.Drawing.Size(562, 463);
			this.txtMonitor.TabIndex = 0;
			this.txtMonitor.Text = "";
			// 
			// frmDCTMonitor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(562, 463);
			this.Controls.Add(this.txtMonitor);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmDCTMonitor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DCT Client Data Monitor";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmDCTMonitor_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		public string SendData(object sender,CommandEventArgs e)
		{
			this.txtMonitor.AppendText(e.Message + "\r\n");
			return e.Message;
		}

		private void frmDCTMonitor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(_client != null)
			{
				_client.OnSendData -= new EventCommandHandler(SendData);
			}
		}
	}
}
