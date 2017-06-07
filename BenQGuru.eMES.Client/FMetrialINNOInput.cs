using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Client.Service;

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FMetrialINNOInput ��ժҪ˵����
	/// </summary>
	public class FMetrialINNOInput : System.Windows.Forms.Form
	{
		private UserControl.UCLabelEdit ucLEINNO;
		private UserControl.UCButton ucBtnOK;
		private System.Windows.Forms.Label lblMessage;
		private UserControl.UCButton ucBtnCancel;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FMetrialINNOInput()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
			UserControl.UIStyleBuilder.FormUI(this);
		}

		public string INNO
		{
			get
			{
				return this.ucLEINNO.Value.Trim().ToUpper();
			}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMetrialINNOInput));
            this.ucLEINNO = new UserControl.UCLabelEdit();
            this.ucBtnOK = new UserControl.UCButton();
            this.lblMessage = new System.Windows.Forms.Label();
            this.ucBtnCancel = new UserControl.UCButton();
            this.SuspendLayout();
            // 
            // ucLEINNO
            // 
            this.ucLEINNO.AllowEditOnlyChecked = true;
            this.ucLEINNO.Caption = "INNO";
            this.ucLEINNO.Checked = false;
            this.ucLEINNO.EditType = UserControl.EditTypes.String;
            this.ucLEINNO.Location = new System.Drawing.Point(28, 40);
            this.ucLEINNO.MaxLength = 40;
            this.ucLEINNO.Multiline = false;
            this.ucLEINNO.Name = "ucLEINNO";
            this.ucLEINNO.PasswordChar = '\0';
            this.ucLEINNO.ReadOnly = false;
            this.ucLEINNO.ShowCheckBox = false;
            this.ucLEINNO.Size = new System.Drawing.Size(237, 21);
            this.ucLEINNO.TabIndex = 1;
            this.ucLEINNO.TabNext = true;
            this.ucLEINNO.Value = "";
            this.ucLEINNO.WidthType = UserControl.WidthTypes.Long;
            this.ucLEINNO.XAlign = 65;
            // 
            // ucBtnOK
            // 
            this.ucBtnOK.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnOK.BackgroundImage")));
            this.ucBtnOK.ButtonType = UserControl.ButtonTypes.None;
            this.ucBtnOK.Caption = "ȷ��";
            this.ucBtnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnOK.Location = new System.Drawing.Point(40, 72);
            this.ucBtnOK.Name = "ucBtnOK";
            this.ucBtnOK.Size = new System.Drawing.Size(88, 22);
            this.ucBtnOK.TabIndex = 2;
            this.ucBtnOK.Click += new System.EventHandler(this.ucBtnOK_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(26, 16);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(240, 16);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "�������µļ������Ϻ�";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucBtnCancel
            // 
            this.ucBtnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnCancel.BackgroundImage")));
            this.ucBtnCancel.ButtonType = UserControl.ButtonTypes.None;
            this.ucBtnCancel.Caption = "ȡ��";
            this.ucBtnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnCancel.Location = new System.Drawing.Point(160, 72);
            this.ucBtnCancel.Name = "ucBtnCancel";
            this.ucBtnCancel.Size = new System.Drawing.Size(88, 22);
            this.ucBtnCancel.TabIndex = 3;
            this.ucBtnCancel.Click += new System.EventHandler(this.ucBtnCancel_Click);
            // 
            // FMetrialINNOInput
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(292, 101);
            this.Controls.Add(this.ucBtnCancel);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.ucBtnOK);
            this.Controls.Add(this.ucLEINNO);
            this.MaximizeBox = false;
            this.Name = "FMetrialINNOInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "������INNO";
            this.Closed += new System.EventHandler(this.FMetrialINNOInput_Closed);
            this.ResumeLayout(false);

		}
		#endregion

		protected void ShowMessage(string message)
		{
			ApplicationRun.GetInfoForm().Add( message );
		}

		protected void ShowMessage(UserControl.Message message)
		{
			ApplicationRun.GetInfoForm().Add( message );
		}

		private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		private void ucBtnOK_Click(object sender, System.EventArgs e)
		{
			if ( this.ucLEINNO.Value.Trim() == string.Empty )
			{
				this.ShowMessage("$CS_Please_Input_INNO");//������INNO
			}
			else if ( new MaterialFacade(this.DataProvider).IsINNOExist( this.ucLEINNO.Value.Trim().ToUpper() ) )
			{
				this.ShowMessage(new UserControl.Message(UserControl.MessageType.Error,"$Error_CS_INNO_Exist"));//INNO�Ѵ��ڣ�����������
			}
			else
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void ucBtnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();		
		}

		private void FMetrialINNOInput_Closed(object sender, System.EventArgs e)
		{
			if (this.DataProvider!=null)
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection();  
		}
	}
}
