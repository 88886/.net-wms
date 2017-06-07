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
	public class FInvRecCheck : System.Windows.Forms.Form
	{
		protected IDomainDataProvider _domainDataProvider;
		private FInfoForm _infoForm;
		private string[] _idList;
		private System.Windows.Forms.GroupBox grbRec;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtRecNo;
		private System.Windows.Forms.TextBox txtRecCarton;
		private System.Windows.Forms.TextBox txtRecRCard;
		private UserControl.UCButton btnRecOK;
		protected InventoryFacade _facade = null;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}
		private void CloseConnection()
		{
			if (this._domainDataProvider!=null)
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this._domainDataProvider).PersistBroker.CloseConnection(); 
		}

		public FInvRecCheck()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();
			UserControl.UIStyleBuilder.FormUI(this);	
			_infoForm = ApplicationRun.GetInfoForm();

			_domainDataProvider =ApplicationService.Current().DataProvider;
			this._facade = new InventoryFacade( this.DataProvider );
			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FInvRecCheck));
            this.grbRec = new System.Windows.Forms.GroupBox();
            this.btnRecOK = new UserControl.UCButton();
            this.txtRecRCard = new System.Windows.Forms.TextBox();
            this.txtRecCarton = new System.Windows.Forms.TextBox();
            this.txtRecNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grbRec.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbRec
            // 
            this.grbRec.Controls.Add(this.btnRecOK);
            this.grbRec.Controls.Add(this.txtRecRCard);
            this.grbRec.Controls.Add(this.txtRecCarton);
            this.grbRec.Controls.Add(this.txtRecNo);
            this.grbRec.Controls.Add(this.label3);
            this.grbRec.Controls.Add(this.label2);
            this.grbRec.Controls.Add(this.label1);
            this.grbRec.Dock = System.Windows.Forms.DockStyle.Top;
            this.grbRec.Location = new System.Drawing.Point(0, 0);
            this.grbRec.Name = "grbRec";
            this.grbRec.Size = new System.Drawing.Size(646, 93);
            this.grbRec.TabIndex = 0;
            this.grbRec.TabStop = false;
            this.grbRec.Text = "�����֤";
            // 
            // btnRecOK
            // 
            this.btnRecOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRecOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnRecOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRecOK.BackgroundImage")));
            this.btnRecOK.ButtonType = UserControl.ButtonTypes.None;
            this.btnRecOK.Caption = "��֤���";
            this.btnRecOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecOK.Location = new System.Drawing.Point(358, 63);
            this.btnRecOK.Name = "btnRecOK";
            this.btnRecOK.Size = new System.Drawing.Size(88, 22);
            this.btnRecOK.TabIndex = 3;
            this.btnRecOK.Click += new System.EventHandler(this.btnRecOK_Click);
            // 
            // txtRecRCard
            // 
            this.txtRecRCard.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRecRCard.Location = new System.Drawing.Point(65, 65);
            this.txtRecRCard.Name = "txtRecRCard";
            this.txtRecRCard.Size = new System.Drawing.Size(150, 20);
            this.txtRecRCard.TabIndex = 2;
            this.txtRecRCard.Leave += new System.EventHandler(this.txtRecRCard_Leave);
            this.txtRecRCard.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRecRCard_KeyUp);
            this.txtRecRCard.Enter += new System.EventHandler(this.txtRecRCard_Enter);
            // 
            // txtRecCarton
            // 
            this.txtRecCarton.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRecCarton.Location = new System.Drawing.Point(301, 32);
            this.txtRecCarton.Name = "txtRecCarton";
            this.txtRecCarton.Size = new System.Drawing.Size(158, 20);
            this.txtRecCarton.TabIndex = 1;
            this.txtRecCarton.Leave += new System.EventHandler(this.txtRecCarton_Leave);
            this.txtRecCarton.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRecRCard_KeyUp);
            this.txtRecCarton.Enter += new System.EventHandler(this.txtRecCarton_Enter);
            // 
            // txtRecNo
            // 
            this.txtRecNo.BackColor = System.Drawing.Color.GreenYellow;
            this.txtRecNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRecNo.Location = new System.Drawing.Point(65, 32);
            this.txtRecNo.Name = "txtRecNo";
            this.txtRecNo.Size = new System.Drawing.Size(152, 20);
            this.txtRecNo.TabIndex = 0;
            this.txtRecNo.Leave += new System.EventHandler(this.txtRecNo_Leave);
            this.txtRecNo.Enter += new System.EventHandler(this.txtRecNo_Enter);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "���к�";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(245, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Carton��";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "��ⵥ��";
            // 
            // FInvRecCheck
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(646, 322);
            this.Controls.Add(this.grbRec);
            this.Location = new System.Drawing.Point(400, 400);
            this.Name = "FInvRecCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "���������֤";
            this.Load += new System.EventHandler(this.FInvPlanate_Load);
            this.Closed += new System.EventHandler(this.FInvPlanate_Closed);
            this.Activated += new System.EventHandler(this.FInvRecCheck_Activated);
            this.grbRec.ResumeLayout(false);
            this.grbRec.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion


		private void FInvPlanate_Load(object sender, System.EventArgs e)
		{
			_domainDataProvider =ApplicationService.Current().DataProvider;

		}

		private void FInvPlanate_Closed(object sender, System.EventArgs e)
		{
			if (this._domainDataProvider!=null)
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this._domainDataProvider).PersistBroker.CloseConnection();  
			}
		}

		private void btnRecOK_Click(object sender, System.EventArgs e)
		{
			#region ����������
			if(this.txtRecNo.Text.Trim() == string.Empty)
			{
				this.ErrorMessage("$Error_CS_Input_StockIn_TicketNo");
                this.txtRecNo.Focus();
				return;
			}

			if(this.txtRecCarton.Text.Trim() == string.Empty && this.txtRecRCard.Text.Trim() == string.Empty)
			{
				this.ErrorMessage("$CS_PLEASE_INPUT_CARTONNO $CS_OR $CS_PleaseInputID");
                this.txtRecCarton.Focus();
				return;
			}
			#endregion

			bool RecCarton = false;
			bool RecRCard = false;
			if(this.txtRecCarton.Text.Trim() != string.Empty)
			{
				RecCarton = true;
			}
			else if(this.txtRecRCard.Text.Trim() != string.Empty)
			{
				RecRCard = true;
			}

			if(this._facade.CheckRecInv(this.txtRecNo.Text,this.txtRecCarton.Text,this.txtRecRCard.Text))
			{
				this.SucessMessage("$Inv_Carton_Rcard_Exists");
			}
			else
			{
				this.ErrorMessage("$Inv_Carton_Rcard__Not_Exists");
			}

			this.txtRecCarton.Text = string.Empty;
			this.txtRecRCard.Text = string.Empty;

			if(RecCarton)
			{
                this.txtRecCarton.Focus();
			}
			else
			{
                this.txtRecRCard.Focus();
			}
		}

		
		protected void ErrorMessage(string msg)
		{			
			_infoForm.Add(new UserControl.Message(UserControl.MessageType.Error,msg));
			BenQGuru.eMES.Web.Helper.SoundPlayer.PlayErrorMusic();
		}

		protected void SucessMessage(string msg)
		{
			_infoForm.Add(new UserControl.Message(UserControl.MessageType.Success,msg));
		}

		private void txtRecRCard_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				btnRecOK_Click(null,null);
			}
		}
		//Laws Lu,2006/12/25 ������뱳��ɫ��Ϊǳ�̣��Ƴ��ָ�����
		private void txtRecNo_Enter(object sender, System.EventArgs e)
		{
			txtRecNo.BackColor = Color.GreenYellow;
		}

		private void txtRecCarton_Enter(object sender, System.EventArgs e)
		{
			txtRecCarton.BackColor = Color.GreenYellow;
		}

		private void txtRecRCard_Enter(object sender, System.EventArgs e)
		{
			txtRecRCard.BackColor = Color.GreenYellow;
		}

		private void txtRecNo_Leave(object sender, System.EventArgs e)
		{
			txtRecNo.BackColor = Color.White;
		}

		private void txtRecCarton_Leave(object sender, System.EventArgs e)
		{
			txtRecCarton.BackColor = Color.White;
		}

		private void txtRecRCard_Leave(object sender, System.EventArgs e)
		{
			txtRecRCard.BackColor = Color.White;
		}

		private void FInvRecCheck_Activated(object sender, System.EventArgs e)
		{
            txtRecNo.Focus();
			txtRecNo.BackColor = Color.GreenYellow;
		}
	}
}
