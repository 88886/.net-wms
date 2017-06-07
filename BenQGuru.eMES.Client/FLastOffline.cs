using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.DataCollect.Action;
using BenQGuru.eMES.Client.Service;
using BenQGuru.eMES.Web.Helper;
using UserControl;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.AlertModel;

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FLastOffline ��ժҪ˵����
	/// </summary>
	public class FLastOffline : System.Windows.Forms.Form
	{
		private UserControl.UCMessage ucMessage;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private string _moCode = string.Empty;
		private System.Windows.Forms.Label lblOnlineInfo;
		private UserControl.UCLabelEdit ucRCard;
		private UserControl.UCLabelEdit ucSSName;
		private BenQGuru.eMES.AlertModel.FirstOnlineFacade _facade = null;
		//private string _itemcode;
		private UserControl.UCLabelCombox cbxEndTime;
		private UserControl.UCButton btnOffline;
		private UserControl.UCLabelEdit txtBeginTime;

		private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		public FLastOffline()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FLastOffline));
			this.ucMessage = new UserControl.UCMessage();
			this.lblOnlineInfo = new System.Windows.Forms.Label();
			this.ucRCard = new UserControl.UCLabelEdit();
			this.ucSSName = new UserControl.UCLabelEdit();
			this.btnOffline = new UserControl.UCButton();
			this.cbxEndTime = new UserControl.UCLabelCombox();
			this.txtBeginTime = new UserControl.UCLabelEdit();
			this.SuspendLayout();
			// 
			// ucMessage
			// 
			this.ucMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ucMessage.BackColor = System.Drawing.Color.Gainsboro;
			this.ucMessage.ButtonVisible = false;
			this.ucMessage.Enabled = false;
			this.ucMessage.Location = new System.Drawing.Point(64, 72);
			this.ucMessage.Name = "ucMessage";
			this.ucMessage.Size = new System.Drawing.Size(640, 376);
			this.ucMessage.TabIndex = 0;
			// 
			// lblOnlineInfo
			// 
			this.lblOnlineInfo.Location = new System.Drawing.Point(8, 72);
			this.lblOnlineInfo.Name = "lblOnlineInfo";
			this.lblOnlineInfo.Size = new System.Drawing.Size(56, 16);
			this.lblOnlineInfo.TabIndex = 9;
			this.lblOnlineInfo.Text = "������Ϣ";
			// 
			// ucRCard
			// 
			this.ucRCard.AllowEditOnlyChecked = true;
			this.ucRCard.Caption = "ĩ����ǩ";
			this.ucRCard.Checked = false;
			this.ucRCard.EditType = UserControl.EditTypes.String;
			this.ucRCard.Location = new System.Drawing.Point(2, 8);
			this.ucRCard.MaxLength = 400;
			this.ucRCard.Multiline = false;
			this.ucRCard.Name = "ucRCard";
			this.ucRCard.PasswordChar = '\0';
			this.ucRCard.ReadOnly = false;
			this.ucRCard.ShowCheckBox = false;
			this.ucRCard.Size = new System.Drawing.Size(262, 23);
			this.ucRCard.TabIndex = 0;
			this.ucRCard.TabNext = false;
			this.ucRCard.Value = "";
			this.ucRCard.WidthType = UserControl.WidthTypes.Long;
			this.ucRCard.XAlign = 64;
			this.ucRCard.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucRCard_TxtboxKeyPress);
			// 
			// ucSSName
			// 
			this.ucSSName.AllowEditOnlyChecked = true;
			this.ucSSName.Caption = "������";
			this.ucSSName.Checked = false;
			this.ucSSName.EditType = UserControl.EditTypes.String;
			this.ucSSName.Enabled = false;
			this.ucSSName.Location = new System.Drawing.Point(272, 8);
			this.ucSSName.MaxLength = 40;
			this.ucSSName.Multiline = false;
			this.ucSSName.Name = "ucSSName";
			this.ucSSName.PasswordChar = '\0';
			this.ucSSName.ReadOnly = false;
			this.ucSSName.ShowCheckBox = false;
			this.ucSSName.Size = new System.Drawing.Size(256, 24);
			this.ucSSName.TabIndex = 11;
			this.ucSSName.TabNext = false;
			this.ucSSName.Value = "";
			this.ucSSName.WidthType = UserControl.WidthTypes.Long;
			this.ucSSName.XAlign = 328;
			// 
			// btnOffline
			// 
			this.btnOffline.BackColor = System.Drawing.SystemColors.Control;
			this.btnOffline.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOffline.BackgroundImage")));
			this.btnOffline.ButtonType = UserControl.ButtonTypes.None;
			this.btnOffline.Caption = "ȷ������";
			this.btnOffline.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnOffline.Location = new System.Drawing.Point(520, 40);
			this.btnOffline.Name = "btnOffline";
			this.btnOffline.Size = new System.Drawing.Size(88, 22);
			this.btnOffline.TabIndex = 16;
			this.btnOffline.Click += new System.EventHandler(this.btnOffline_Click);
			// 
			// cbxEndTime
			// 
			this.cbxEndTime.AllowEditOnlyChecked = true;
			this.cbxEndTime.Caption = "�°�ʱ��";
			this.cbxEndTime.Checked = false;
			this.cbxEndTime.Location = new System.Drawing.Point(5, 40);
			this.cbxEndTime.Name = "cbxEndTime";
			this.cbxEndTime.SelectedIndex = -1;
			this.cbxEndTime.ShowCheckBox = false;
			this.cbxEndTime.Size = new System.Drawing.Size(195, 21);
			this.cbxEndTime.TabIndex = 17;
			this.cbxEndTime.WidthType = UserControl.WidthTypes.Normal;
			this.cbxEndTime.XAlign = 67;
			this.cbxEndTime.SelectedIndexChanged += new System.EventHandler(this.cbxEndTime_SelectedIndexChanged);
			// 
			// txtBeginTime
			// 
			this.txtBeginTime.AllowEditOnlyChecked = true;
			this.txtBeginTime.Caption = "�ϰ�ʱ��";
			this.txtBeginTime.Checked = false;
			this.txtBeginTime.EditType = UserControl.EditTypes.String;
			this.txtBeginTime.Enabled = false;
			this.txtBeginTime.Location = new System.Drawing.Point(269, 40);
			this.txtBeginTime.MaxLength = 400;
			this.txtBeginTime.Multiline = false;
			this.txtBeginTime.Name = "txtBeginTime";
			this.txtBeginTime.PasswordChar = '\0';
			this.txtBeginTime.ReadOnly = false;
			this.txtBeginTime.ShowCheckBox = false;
			this.txtBeginTime.Size = new System.Drawing.Size(195, 23);
			this.txtBeginTime.TabIndex = 18;
			this.txtBeginTime.TabNext = false;
			this.txtBeginTime.Value = "";
			this.txtBeginTime.WidthType = UserControl.WidthTypes.Normal;
			this.txtBeginTime.XAlign = 331;
			// 
			// FLastOffline
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(712, 453);
			this.Controls.Add(this.txtBeginTime);
			this.Controls.Add(this.cbxEndTime);
			this.Controls.Add(this.btnOffline);
			this.Controls.Add(this.ucSSName);
			this.Controls.Add(this.ucRCard);
			this.Controls.Add(this.lblOnlineInfo);
			this.Controls.Add(this.ucMessage);
			this.Name = "FLastOffline";
			this.Text = "ĩ������";
			this.Load += new System.EventHandler(this.FLastOffline_Load);
			this.Closed += new System.EventHandler(this.FLastOffline_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		private string _firstMsg;

		private void FLastOffline_Closed(object sender, System.EventArgs e)
		{
			CloseConnection(); 
		}

		private void CloseConnection()
		{
			if (this.DataProvider!=null)
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection(); 
		}

		private void FLastOffline_Load(object sender, System.EventArgs e)
		{
			_firstMsg = UserControl.MutiLanguages.ParserMessage("$Error_FirstOn_Time");//{0}{1}�����׼�����ʱ����:{2};
			this.ucRCard.TextFocus(false, true);
			Messages msg = new Messages();

			_facade = new FirstOnlineFacade(this.DataProvider);

			//ȡ����
			BenQGuru.eMES.BaseSetting.BaseModelFacade _baseFacade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider);
			Resource res = (Resource)_baseFacade.GetResource(ApplicationService.Current().ResourceCode);
			if(res != null && res.StepSequenceCode != null)
			{
				this.ucSSName.Value = res.StepSequenceCode;
			}
			else
			{
				msg.Add(new UserControl.Message(MessageType.Error,this.ucSSName.Caption + "$Error_Input_Empty")); 
				ApplicationRun.GetInfoForm().Add(msg);
				return;
			}

			this.ucRCard.InnerTextBox.Multiline = false;
		}


		private void ucRCard_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar=='\r')
			{
				FillEndTime();	
			}
		}

		//����°�ʱ���б�
		private void FillEndTime()
		{
			#region ����������
			if(this.ucSSName.Value == string.Empty)
			{
				Messages msg = new Messages();
				msg.Add(new UserControl.Message(MessageType.Error,this.ucSSName.Caption + " $Error_Input_Empty"));
				ApplicationRun.GetInfoForm().Add(msg);
				this.ucRCard.TextFocus(false, true);
				return;
			}
			if(this.ucRCard.Value == string.Empty)
			{
				Messages msg = new Messages();
				msg.Add(new UserControl.Message(MessageType.Error,this.ucRCard.Caption + " $Error_Input_Empty"));
				ApplicationRun.GetInfoForm().Add(msg);
				this.ucRCard.TextFocus(false, true);
				return;
			}
			#endregion
			
			#region �жϲ�Ʒ���к��Ƿ����
			BenQGuru.eMES.DataCollect.DataCollectFacade dcFacade = new DataCollectFacade(this.DataProvider);
			BenQGuru.eMES.Domain.DataCollect.Simulation sim = dcFacade.GetSimulation(ucRCard.InnerTextBox.Text.Trim()) as BenQGuru.eMES.Domain.DataCollect.Simulation;
			
			string itemcode;
			if(sim == null)
			{
				ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error,"$Error_First_No_Rcard"));
				ucRCard.TextFocus(false, true);
				return;
			}
			else
			{
				itemcode = sim.ItemCode;
			}
			#endregion

			#region Build�°�ʱ�������б�
			this.cbxEndTime.ComboBoxData.Items.Clear();
			object[] objs = this._facade.QueryLast(this.ucSSName.Value,itemcode);
			if(objs != null && objs.Length > 0)
			{
				foreach(BenQGuru.eMES.Domain.Alert.FirstOnline last in objs)
				{
					if(last != null)
					{
						this.cbxEndTime.ComboBoxData.Items.Add(new UILastOnline(last));
					}
				}
			}
			else
			{
				ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error,"������ĩ������"));
				ucRCard.TextFocus(false, true);
				return;
			}

			if(this.cbxEndTime.ComboBoxData.Items.Count > 0)
				this.cbxEndTime.ComboBoxData.SelectedIndex = 0;
			#endregion

		}

		private void btnOffline_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(this.cbxEndTime.ComboBoxData.SelectedIndex == -1)
				{
					ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error,"��ѡ���°�ʱ��"));
					ucRCard.TextFocus(false, true);
					return;
				}

				Messages msg = new Messages();
				UILastOnline ul = this.cbxEndTime.ComboBoxData.SelectedItem as UILastOnline;

				BenQGuru.eMES.Domain.Alert.FirstOnline first_on = ul._first;
				if(first_on == null)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"�������׼���/����"));
					ApplicationRun.GetInfoForm().Add(msg);
					return;
				}
	
				if(first_on.LastType == LineActionType.OFF)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"�Ѿ�����ĩ��������"));
					ApplicationRun.GetInfoForm().Add(msg);
					return;
				}

				if(first_on.LastType != LineActionType.ON)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"������ĩ������"));
					ApplicationRun.GetInfoForm().Add(msg);
					return;
				}

				try
				{
					//2006/11/17,Laws Lu add get DateTime from db Server
					DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

					DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);

					first_on.LastOffRCard = this.ucRCard.Value.Trim();
					first_on.LastType = LineActionType.OFF;
					first_on.LastOffTime = FormatHelper.TOTimeInt(dtNow);
					
					this.DataProvider.BeginTransaction();

					this.DataProvider.Update(first_on);

					ucMessage.Add("�°�ʱ��"+this.cbxEndTime.ComboBoxData.SelectedItem.ToString()+"," +
									"����" + first_on.SSCode + "," +
									"��Ʒ����" + first_on.ItemCode + "," +
									"ĩ������ʱ��" + FormatHelper.ToTimeString(first_on.LastOffTime)
								);
					
					this.DataProvider.CommitTransaction();
					msg.Add(new UserControl.Message(MessageType.Success,"ĩ�����߳ɹ�")); //
					ClearForm();
					ApplicationRun.GetInfoForm().Add(msg);
				}
				catch(System.Exception ex)
				{
					this.DataProvider.RollbackTransaction();
					msg.Add(new UserControl.Message(MessageType.Error,ex.Message)); 
					ApplicationRun.GetInfoForm().Add(msg);
				}
			}
			finally
			{
				this.Cursor = System.Windows.Forms.Cursors.Arrow;
				this.ucRCard.Value = string.Empty;
				this.ucRCard.TextFocus(false, true);
				CloseConnection();
			}
		}

		//�����û�ѡ����°�ʱ�䣬�����ϰ�ʱ�乩ѡ��
		private void cbxEndTime_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UILastOnline ul = this.cbxEndTime.ComboBoxData.SelectedItem as UILastOnline;
			this.txtBeginTime.Value = FormatHelper.ToTimeString(ul._first.ShiftTime);
		}

		private void ClearForm()
		{
			this.cbxEndTime.ComboBoxData.Items.Clear();
		}

	}
}
