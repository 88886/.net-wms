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
	/// FFirstOnline ��ժҪ˵����
	/// </summary>
	public class FFirstOnline : System.Windows.Forms.Form
	{
		private UserControl.UCButton ucBtnExit;
		private UserControl.UCButton ucBtnOK;
		private UserControl.UCMessage ucMessage;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private string _moCode = string.Empty;

		private UserControl.UCButton ucButton1;
		private System.Windows.Forms.Label lblOnlineInfo;
		private UserControl.UCLabelEdit ucRCard;
		private UserControl.UCLabelEdit ucSSName;
		private BenQGuru.eMES.AlertModel.FirstOnlineFacade _facade = null;
		private UserControl.UCLabelCombox cbxShiftTime;
		private System.Windows.Forms.DateTimePicker dtpBegin;
		private System.Windows.Forms.Label label1;

		private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		public FFirstOnline()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FFirstOnline));
			this.ucBtnExit = new UserControl.UCButton();
			this.ucBtnOK = new UserControl.UCButton();
			this.ucMessage = new UserControl.UCMessage();
			this.ucButton1 = new UserControl.UCButton();
			this.lblOnlineInfo = new System.Windows.Forms.Label();
			this.ucRCard = new UserControl.UCLabelEdit();
			this.ucSSName = new UserControl.UCLabelEdit();
			this.cbxShiftTime = new UserControl.UCLabelCombox();
			this.dtpBegin = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// ucBtnExit
			// 
			this.ucBtnExit.BackColor = System.Drawing.SystemColors.Control;
			this.ucBtnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnExit.BackgroundImage")));
			this.ucBtnExit.ButtonType = UserControl.ButtonTypes.Exit;
			this.ucBtnExit.Caption = "�˳�";
			this.ucBtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ucBtnExit.Location = new System.Drawing.Point(442, 52);
			this.ucBtnExit.Name = "ucBtnExit";
			this.ucBtnExit.Size = new System.Drawing.Size(88, 22);
			this.ucBtnExit.TabIndex = 2;
			// 
			// ucBtnOK
			// 
			this.ucBtnOK.BackColor = System.Drawing.SystemColors.Control;
			this.ucBtnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnOK.BackgroundImage")));
			this.ucBtnOK.ButtonType = UserControl.ButtonTypes.Confirm;
			this.ucBtnOK.Caption = "ȷ��";
			this.ucBtnOK.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ucBtnOK.Location = new System.Drawing.Point(336, 52);
			this.ucBtnOK.Name = "ucBtnOK";
			this.ucBtnOK.Size = new System.Drawing.Size(88, 22);
			this.ucBtnOK.TabIndex = 1;
			this.ucBtnOK.Click += new System.EventHandler(this.ucBtnOK_Click);
			// 
			// ucMessage
			// 
			this.ucMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ucMessage.BackColor = System.Drawing.Color.Gainsboro;
			this.ucMessage.ButtonVisible = false;
			this.ucMessage.Enabled = false;
			this.ucMessage.Location = new System.Drawing.Point(77, 95);
			this.ucMessage.Name = "ucMessage";
			this.ucMessage.Size = new System.Drawing.Size(473, 215);
			this.ucMessage.TabIndex = 0;
			// 
			// ucButton1
			// 
			this.ucButton1.BackColor = System.Drawing.SystemColors.Control;
			this.ucButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucButton1.BackgroundImage")));
			this.ucButton1.ButtonType = UserControl.ButtonTypes.None;
			this.ucButton1.Caption = "��������";
			this.ucButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ucButton1.Location = new System.Drawing.Point(464, 17);
			this.ucButton1.Name = "ucButton1";
			this.ucButton1.Size = new System.Drawing.Size(88, 22);
			this.ucButton1.TabIndex = 8;
			this.ucButton1.Visible = false;
			this.ucButton1.Click += new System.EventHandler(this.ucButton1_Click);
			// 
			// lblOnlineInfo
			// 
			this.lblOnlineInfo.Location = new System.Drawing.Point(10, 95);
			this.lblOnlineInfo.Name = "lblOnlineInfo";
			this.lblOnlineInfo.Size = new System.Drawing.Size(67, 25);
			this.lblOnlineInfo.TabIndex = 9;
			this.lblOnlineInfo.Text = "������Ϣ";
			// 
			// ucRCard
			// 
			this.ucRCard.AllowEditOnlyChecked = true;
			this.ucRCard.Caption = "�׼���ǩ";
			this.ucRCard.Checked = false;
			this.ucRCard.EditType = UserControl.EditTypes.String;
			this.ucRCard.Location = new System.Drawing.Point(22, 52);
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
			this.ucRCard.XAlign = 84;
			this.ucRCard.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucRCard_TxtboxKeyPress);
			// 
			// ucSSName
			// 
			this.ucSSName.AllowEditOnlyChecked = true;
			this.ucSSName.Caption = "������";
			this.ucSSName.Checked = false;
			this.ucSSName.EditType = UserControl.EditTypes.String;
			this.ucSSName.Enabled = false;
			this.ucSSName.Location = new System.Drawing.Point(34, 16);
			this.ucSSName.MaxLength = 40;
			this.ucSSName.Multiline = false;
			this.ucSSName.Name = "ucSSName";
			this.ucSSName.PasswordChar = '\0';
			this.ucSSName.ReadOnly = false;
			this.ucSSName.ShowCheckBox = false;
			this.ucSSName.Size = new System.Drawing.Size(250, 24);
			this.ucSSName.TabIndex = 11;
			this.ucSSName.TabNext = false;
			this.ucSSName.Value = "";
			this.ucSSName.WidthType = UserControl.WidthTypes.Long;
			this.ucSSName.XAlign = 84;
			// 
			// cbxShiftTime
			// 
			this.cbxShiftTime.AllowEditOnlyChecked = true;
			this.cbxShiftTime.Caption = "�ϰ�ʱ��";
			this.cbxShiftTime.Checked = false;
			this.cbxShiftTime.Location = new System.Drawing.Point(24, 152);
			this.cbxShiftTime.Name = "cbxShiftTime";
			this.cbxShiftTime.SelectedIndex = -1;
			this.cbxShiftTime.ShowCheckBox = false;
			this.cbxShiftTime.Size = new System.Drawing.Size(195, 21);
			this.cbxShiftTime.TabIndex = 13;
			this.cbxShiftTime.Visible = false;
			this.cbxShiftTime.WidthType = UserControl.WidthTypes.Normal;
			this.cbxShiftTime.XAlign = 86;
			// 
			// dtpBegin
			// 
			this.dtpBegin.CustomFormat = "H:mm:ss";
			this.dtpBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpBegin.Location = new System.Drawing.Point(352, 16);
			this.dtpBegin.Name = "dtpBegin";
			this.dtpBegin.ShowUpDown = true;
			this.dtpBegin.Size = new System.Drawing.Size(80, 21);
			this.dtpBegin.TabIndex = 14;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(288, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 15;
			this.label1.Text = "�ϰ�ʱ��";
			// 
			// FFirstOnline
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(556, 315);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dtpBegin);
			this.Controls.Add(this.cbxShiftTime);
			this.Controls.Add(this.ucSSName);
			this.Controls.Add(this.ucRCard);
			this.Controls.Add(this.lblOnlineInfo);
			this.Controls.Add(this.ucButton1);
			this.Controls.Add(this.ucMessage);
			this.Controls.Add(this.ucBtnOK);
			this.Controls.Add(this.ucBtnExit);
			this.Name = "FFirstOnline";
			this.Text = "�׼�����";
			this.Load += new System.EventHandler(this.FFirstOnline_Load);
			this.Closed += new System.EventHandler(this.FFirstOnline_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		private string _firstMsg;

		private void FFirstOnline_Closed(object sender, System.EventArgs e)
		{
			CloseConnection(); 
		}

		private void CloseConnection()
		{
			if (this.DataProvider!=null)
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection(); 
		}

		private void FFirstOnline_Load(object sender, System.EventArgs e)
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
			
			int i = this._facade.BindShiftTime(this.cbxShiftTime.ComboBoxData.Items,res);
			if( i >= 0)
			{
				this.cbxShiftTime.ComboBoxData.SelectedIndex = i;
				this.dtpBegin.Text = this.cbxShiftTime.ComboBoxData.SelectedItem.ToString();
			}

			this.ucRCard.InnerTextBox.Multiline = false;

			DoSSCodeChange();
		}


		private void DoAction()
		{
			try
			{
				//2006/11/17,Laws Lu add get DateTime from db Server
				DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

				DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);

				this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
				Messages msg = new Messages();

				//ȡ�õ�ǰʱ�����ڵİ��
				int i = _facade.GetCurrShiftIndex(FormatHelper.TOTimeInt(this.dtpBegin.Text),this.cbxShiftTime.ComboBoxData.Items);
				if(i != -1)
				{
					this.cbxShiftTime.ComboBoxData.SelectedIndex = i;
				}
				else
				{
					msg.Add(new UserControl.Message(MessageType.Error,"ѡ���ʱ��û�а��")); //��ѡ����
					ApplicationRun.GetInfoForm().Add(msg);
					return;	
				}
				UIShift shift = this.cbxShiftTime.ComboBoxData.SelectedItem as UIShift;
				if(shift == null)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"$��β�����")); //��ѡ����
					ApplicationRun.GetInfoForm().Add(msg);
					return;
				}

				BenQGuru.eMES.DataCollect.DataCollectFacade dcFacade = new DataCollectFacade(this.DataProvider);
				BenQGuru.eMES.Domain.DataCollect.Simulation sim = dcFacade.GetSimulation(ucRCard.InnerTextBox.Text.Trim()) as BenQGuru.eMES.Domain.DataCollect.Simulation;
			
				string itemcode;
				if(sim == null)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"$Error_First_No_Rcard")); //�����ϲ����������Ʒ���к�
					ApplicationRun.GetInfoForm().Add(msg);
					ucRCard.TextFocus(false, true);
					return;
				}
				else
				{
					itemcode = sim.ItemCode;
				}
				
				//�ж��Ƿ��Ѿ���������
				DateTime dtShiftDay = dtNow;
				
				/////���ǿ���İ�Σ������Ǵ��ڵڶ���,����ǰһ����Ϊ��������
				if(shift.IsOverDay == FormatHelper.TRUE_STRING)
				{
					if(FormatHelper.TOTimeInt(dtShiftDay) < shift.EndTime)
						dtShiftDay = dtShiftDay.AddDays(-1);
				}

				#region //�ж�ʱ����򣺰����ʼʱ��<=�ϰ�ʱ��<=�Σϣ�<=�����ֹʱ��
//				int shiftBegin = shift.BeginTime; //�����ʼʱ��
//
//				int begin = FormatHelper.TOTimeInt(this.dtpBegin.Text); //�ϰ�ʱ��
//				if(shift.IsOverDay == FormatHelper.TRUE_STRING && begin <shiftBegin)
//					begin += 1000000;
//
//				int iNow = FormatHelper.TOTimeInt(dtNow);//�Σϣ�
//				if(shift.IsOverDay == FormatHelper.TRUE_STRING && iNow < shiftBegin)
//					iNow += 1000000;
//
//				//�°�ʱ��
//				int shiftEnd = shift.EndTime;
//				if(shift.IsOverDay == FormatHelper.TRUE_STRING)
//					shiftEnd += 1000000; 
//
//				if(!
//					(shiftBegin<=begin
//					&&
//					begin<=iNow
//					&&
//					iNow<=shiftEnd)
//					)
//				{
//					if(MessageBox.Show("�ϰ�ʱ�����������󣨹���Ϊ�������ʼʱ��<=�ϰ�ʱ��<=����ʱ��<=�����ֹʱ�䣩���Ƿ������",
//															"ȷ��Ҫ����",
//															MessageBoxButtons.YesNo)==DialogResult.No)
//						return;
//				}
				#endregion

				object obj = _facade.GetFirstOnline(this.ucSSName.Value,FormatHelper.TODateInt(dtShiftDay),itemcode,shift.ShiftCode,FormatHelper.TOTimeInt(this.dtpBegin.Text));
				BenQGuru.eMES.Domain.Alert.FirstOnline first = obj as BenQGuru.eMES.Domain.Alert.FirstOnline;

				if(first != null)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"��������ϰ�ʱ�䣬�˲����ϴ˲�Ʒ�Ѿ���һ�������׼�"));//"$Error_FirstOn_Repeat")); //���������Σ��˲����ϴ˲�Ʒ�Ѿ���һ�������׼�
					ApplicationRun.GetInfoForm().Add(msg);
					return;
				}
				try
				{
					this.DataProvider.BeginTransaction();

					first = _facade.CreateNewFirstOnline();
					first.ItemCode = itemcode;
					first.ModelCode = sim.ModelCode;
					first.SSCode = this.ucSSName.Value;
					first.RunningCard = this.ucRCard.Value;
					first.OfflineRuningCard = string.Empty;
					first.ActionType = LineActionType.ON;
					first.MaintainDate = FormatHelper.TODateInt(dtShiftDay); ////���ǿ���İ�Σ������Ǵ��ڵڶ���,����ǰһ����Ϊ��������
					first.MaintianUser = ApplicationService.Current().UserCode;
					first.OffLineTime = 0;
					first.OnLineTime = FormatHelper.TOTimeInt(dtNow);
					first.ShiftCode = shift.ShiftCode;
					first.ShiftTime = FormatHelper.TOTimeInt(this.dtpBegin.Text);
					first.IsOverDay = shift.IsOverDay;

					this.DataProvider.Insert(first);
			
					ucMessage.Add(new UserControl.Message("�ϰ�ʱ��"+this.dtpBegin.Text+"," +
						String.Format(_firstMsg,
						dtShiftDay.ToLongDateString(),
						first.SSCode,
						first.ItemCode,
						FormatHelper.ToTimeString(first.OnLineTime)
						)
						)
						);

					this.DataProvider.CommitTransaction();
					msg.Add(new UserControl.Message(MessageType.Success,"$Error_FirstOn_Sucess")); //�׼����߳ɹ�
					ApplicationRun.GetInfoForm().Add(msg);
				}
				catch(System.Exception ex)
				{
					this.DataProvider.RollbackTransaction();
					msg.Add(new UserControl.Message(MessageType.Success,ex.Message));
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
		

		private void ucRCard_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar=='\r')
			{
				ucBtnOK_Click(null,null);	
			}
		}

		private void ucBtnOK_Click(object sender, System.EventArgs e)
		{
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
			DoAction();	
			this.ucRCard.TextFocus(false, true);
		}

		private void ucButton1_Click(object sender, System.EventArgs e)
		{
			FSSChoose ss = new FSSChoose();
			if(ss.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.ucSSName.Value = ss.GetSSCode();
				DoSSCodeChange();
			}
		}

		//���ߴ��뷢���ı�ʱִ��,��һ��load�����ѡ�����ʱ����
		private void DoSSCodeChange()
		{
//			_facade = new FirstOnlineFacade(this.DataProvider);
//
//			//�ж��Ƿ��Ѿ���������
//			object obj = _facade.GetFirstOnline(this.ucSSName.Value,FormatHelper.TODateInt(DateTime.Now),LineActionType.ON);
//			BenQGuru.eMES.Domain.Alert.FirstOnline first = obj as BenQGuru.eMES.Domain.Alert.FirstOnline;
//
//			if(first != null)
//			{
//				this.ucRCard.Enabled = false;
//				this.ucRCard.Value = first.RunningCard;
//				ucMessage.Add(new UserControl.Message(
//					String.Format(_firstMsg,
//					(DateTime.Parse((FormatHelper.ToDateString(first.MaintainDate)))).ToLongDateString(),
//					first.SSCode,
//					FormatHelper.ToTimeString(first.OnLineTime)
//					)
//					)
//					);
//			
//			}
//			else
//			{
//				this.ucRCard.Enabled = true;
//				this.ucRCard.Value = string.Empty;
//
//			}
		}
	}
}
