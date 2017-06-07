#region using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Client.Service;
using UserControl;
using BenQGuru.eMES.OQC;
#endregion

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FInvPlanate ��ժҪ˵����
	/// </summary>
	public class FInvCarton : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox rtxtInput;
		private UserControl.UCButton ucBtnCancel;
		private System.Windows.Forms.Label lblMessage;
		private UserControl.UCButton ucBtnOK;
		protected IDomainDataProvider _domainDataProvider;
		private string[] _idList;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FInvCarton()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		public string[] IDList
		{
			get{ return _idList;}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FInvPlanate));
			this.rtxtInput = new System.Windows.Forms.RichTextBox();
			this.ucBtnCancel = new UserControl.UCButton();
			this.lblMessage = new System.Windows.Forms.Label();
			this.ucBtnOK = new UserControl.UCButton();
			this.SuspendLayout();
			// 
			// rtxtInput
			// 
			this.rtxtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.rtxtInput.Location = new System.Drawing.Point(6, 28);
			this.rtxtInput.Name = "rtxtInput";
			this.rtxtInput.Size = new System.Drawing.Size(316, 82);
			this.rtxtInput.TabIndex = 6;
			this.rtxtInput.Text = "";
			// 
			// ucBtnCancel
			// 
			this.ucBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ucBtnCancel.BackColor = System.Drawing.SystemColors.Control;
			this.ucBtnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnCancel.BackgroundImage")));
			this.ucBtnCancel.ButtonType = UserControl.ButtonTypes.None;
			this.ucBtnCancel.Caption = "ȡ��";
			this.ucBtnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ucBtnCancel.Location = new System.Drawing.Point(128, 118);
			this.ucBtnCancel.Name = "ucBtnCancel";
			this.ucBtnCancel.Size = new System.Drawing.Size(88, 22);
			this.ucBtnCancel.TabIndex = 8;
			this.ucBtnCancel.Click += new System.EventHandler(this.ucBtnCancel_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblMessage.Location = new System.Drawing.Point(8, 6);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(284, 16);
			this.lblMessage.TabIndex = 9;
			this.lblMessage.Text = "��������Ƴ���Carton";
			this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ucBtnOK
			// 
			this.ucBtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ucBtnOK.BackColor = System.Drawing.SystemColors.Control;
			this.ucBtnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnOK.BackgroundImage")));
			this.ucBtnOK.ButtonType = UserControl.ButtonTypes.None;
			this.ucBtnOK.Caption = "ȷ��";
			this.ucBtnOK.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ucBtnOK.Location = new System.Drawing.Point(30, 118);
			this.ucBtnOK.Name = "ucBtnOK";
			this.ucBtnOK.Size = new System.Drawing.Size(88, 22);
			this.ucBtnOK.TabIndex = 7;
			this.ucBtnOK.Click += new System.EventHandler(this.ucBtnOK_Click);
			// 
			// FInvPlanate
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(328, 149);
			this.Controls.Add(this.rtxtInput);
			this.Controls.Add(this.ucBtnCancel);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.ucBtnOK);
			this.Location = new System.Drawing.Point(400, 400);
			this.Name = "FInvPlanate";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Carton����";
			this.Load += new System.EventHandler(this.FInvPlanate_Load);
			this.Closed += new System.EventHandler(this.FInvPlanate_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		protected void ErrorMessage(string msg)
		{			
			ApplicationRun.GetInfoForm().Add(new UserControl.Message(UserControl.MessageType.Error,msg));
		}

		private void ucBtnOK_Click(object sender, System.EventArgs e)
		{
			if ( this.rtxtInput.Text.Trim() == string.Empty )
			{
				this.ErrorMessage(lblMessage.Text);	//��������Ƴ��Ķ�ά����
                this.rtxtInput.Focus();
				return;
			}
			else
			{
				try
				{
//					BarCodeParse barParser = new BarCodeParse(this._domainDataProvider);
//					string[] idList = barParser.GetIDList( this.rtxtInput.Text.Trim());
//			
//					if ( idList == null || idList.Length == 0)
//					{
//						this.ErrorMessage("$CS_RCard_List_Is_Empty");
//						return;
//					}
//					else
//					{
//						this._idList = idList;
//					}
					BenQGuru.eMES.DataCollect.DataCollectFacade dc = new BenQGuru.eMES.DataCollect.DataCollectFacade(this.DataProvider);
					object[] idList = dc.GetSimulationFromCarton(this.rtxtInput.Text.Trim());
					if ( idList == null || idList.Length == 0)
					{
						this.ErrorMessage("$CS_RCard_List_Is_Empty");
						return;
					}
					else
					{
						this._idList = new string[idList.Length];
						int i = 0;
						foreach (BenQGuru.eMES.Domain.DataCollect.Simulation sim in idList )
						{
								_idList[i++] = sim.RunningCard;
						}
					}
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				catch(System.Exception ex)
				{
					this.ErrorMessage(ex.Message);
				}
			}
		}

		private void FInvPlanate_Load(object sender, System.EventArgs e)
		{
			_domainDataProvider =ApplicationService.Current().DataProvider;
			_idList = null;
		}

		private void FInvPlanate_Closed(object sender, System.EventArgs e)
		{
			if (this._domainDataProvider!=null)
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this._domainDataProvider).PersistBroker.CloseConnection();  
			}
		}

		private void ucBtnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}
}
