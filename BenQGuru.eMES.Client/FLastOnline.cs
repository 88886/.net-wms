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
	/// FLastOnline ��ժҪ˵����
	/// </summary>
	public class FLastOnline : System.Windows.Forms.Form
	{
		private UserControl.UCButton ucBtnOK;
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
		private UserControl.UCLabelCombox cbxShiftTime;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DateTimePicker dtpEnd;
		private UserControl.UCLabelCombox cbxBeginTime;
		private string _shiftCode;
		private int _shiftDate;
		private string _itemcode;

		private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		public FLastOnline()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FLastOnline));
			this.ucBtnOK = new UserControl.UCButton();
			this.ucMessage = new UserControl.UCMessage();
			this.lblOnlineInfo = new System.Windows.Forms.Label();
			this.ucRCard = new UserControl.UCLabelEdit();
			this.ucSSName = new UserControl.UCLabelEdit();
			this.cbxShiftTime = new UserControl.UCLabelCombox();
			this.dtpEnd = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.cbxBeginTime = new UserControl.UCLabelCombox();
			this.SuspendLayout();
			// 
			// ucBtnOK
			// 
			this.ucBtnOK.BackColor = System.Drawing.SystemColors.Control;
			this.ucBtnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnOK.BackgroundImage")));
			this.ucBtnOK.ButtonType = UserControl.ButtonTypes.None;
			this.ucBtnOK.Caption = "ȷ������";
			this.ucBtnOK.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ucBtnOK.Location = new System.Drawing.Point(528, 40);
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
			this.ucMessage.Location = new System.Drawing.Point(72, 72);
			this.ucMessage.Name = "ucMessage";
			this.ucMessage.Size = new System.Drawing.Size(632, 376);
			this.ucMessage.TabIndex = 0;
			// 
			// lblOnlineInfo
			// 
			this.lblOnlineInfo.Location = new System.Drawing.Point(8, 80);
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
			this.ucRCard.Location = new System.Drawing.Point(10, 40);
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
			this.ucRCard.XAlign = 72;
			this.ucRCard.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucRCard_TxtboxKeyPress);
			// 
			// ucSSName
			// 
			this.ucSSName.AllowEditOnlyChecked = true;
			this.ucSSName.Caption = "������";
			this.ucSSName.Checked = false;
			this.ucSSName.EditType = UserControl.EditTypes.String;
			this.ucSSName.Enabled = false;
			this.ucSSName.Location = new System.Drawing.Point(288, 8);
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
			this.ucSSName.XAlign = 338;
			// 
			// cbxShiftTime
			// 
			this.cbxShiftTime.AllowEditOnlyChecked = true;
			this.cbxShiftTime.Caption = "���ʱ���б�";
			this.cbxShiftTime.Checked = false;
			this.cbxShiftTime.Location = new System.Drawing.Point(40, 152);
			this.cbxShiftTime.Name = "cbxShiftTime";
			this.cbxShiftTime.SelectedIndex = -1;
			this.cbxShiftTime.ShowCheckBox = false;
			this.cbxShiftTime.Size = new System.Drawing.Size(220, 21);
			this.cbxShiftTime.TabIndex = 13;
			this.cbxShiftTime.Visible = false;
			this.cbxShiftTime.WidthType = UserControl.WidthTypes.Normal;
			this.cbxShiftTime.XAlign = 127;
			// 
			// dtpEnd
			// 
			this.dtpEnd.CustomFormat = "H:mm:ss";
			this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpEnd.Location = new System.Drawing.Point(72, 8);
			this.dtpEnd.Name = "dtpEnd";
			this.dtpEnd.ShowUpDown = true;
			this.dtpEnd.TabIndex = 14;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 15;
			this.label1.Text = "�°�ʱ��";
			// 
			// cbxBeginTime
			// 
			this.cbxBeginTime.AllowEditOnlyChecked = true;
			this.cbxBeginTime.Caption = "�ϰ�ʱ��";
			this.cbxBeginTime.Checked = false;
			this.cbxBeginTime.Location = new System.Drawing.Point(277, 40);
			this.cbxBeginTime.Name = "cbxBeginTime";
			this.cbxBeginTime.SelectedIndex = -1;
			this.cbxBeginTime.ShowCheckBox = false;
			this.cbxBeginTime.Size = new System.Drawing.Size(195, 21);
			this.cbxBeginTime.TabIndex = 16;
			this.cbxBeginTime.WidthType = UserControl.WidthTypes.Normal;
			this.cbxBeginTime.XAlign = 339;
			// 
			// FLastOnline
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(712, 453);
			this.Controls.Add(this.cbxBeginTime);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dtpEnd);
			this.Controls.Add(this.cbxShiftTime);
			this.Controls.Add(this.ucSSName);
			this.Controls.Add(this.ucRCard);
			this.Controls.Add(this.lblOnlineInfo);
			this.Controls.Add(this.ucMessage);
			this.Controls.Add(this.ucBtnOK);
			this.Name = "FLastOnline";
			this.Text = "ĩ������";
			this.Load += new System.EventHandler(this.FLastOnline_Load);
			this.Closed += new System.EventHandler(this.FLastOnline_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		private string _firstMsg;

		private void FLastOnline_Closed(object sender, System.EventArgs e)
		{
			CloseConnection(); 
		}

		private void CloseConnection()
		{
			if (this.DataProvider!=null)
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection(); 
		}

		private void FLastOnline_Load(object sender, System.EventArgs e)
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
				UIShift shift = this.cbxShiftTime.ComboBoxData.SelectedItem as UIShift;
				if(shift != null)
					this.dtpEnd.Text = FormatHelper.ToTimeString(shift.EndTime);
			}

			this.ucRCard.InnerTextBox.Multiline = false;
		}


		private void ucRCard_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar=='\r')
			{
				FillBeginTime();	
			}
		}

		//���ݰ�Σ���Ʒ���룬���ߣ���������������׼����ߺͿ���İ�����������׼����ߣ�����ڡ��ϰ�ʱ�䡱�����б���
		private void FillBeginTime()
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

			#region //ȡ�õ�ǰʱ�����ڵİ��
				
			int i = _facade.GetCurrShiftIndex(FormatHelper.TOTimeInt(this.dtpEnd.Text),this.cbxShiftTime.ComboBoxData.Items);
			if(i != -1)
			{
				this.cbxShiftTime.ComboBoxData.SelectedIndex = i;
			}
			else
			{
				ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error,"ѡ���ʱ��û�а��"));
				return;	
			}
			UIShift shift = this.cbxShiftTime.ComboBoxData.SelectedItem as UIShift;
			if(shift == null)
			{
				ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error,"��β�����"));
				return;
			}
			#endregion

			#region �жϲ�Ʒ���к��Ƿ����
			BenQGuru.eMES.DataCollect.DataCollectFacade dcFacade = new DataCollectFacade(this.DataProvider);
			BenQGuru.eMES.Domain.DataCollect.Simulation sim = dcFacade.GetSimulation(ucRCard.InnerTextBox.Text.Trim()) as BenQGuru.eMES.Domain.DataCollect.Simulation;
			
			if(sim == null)
			{
				ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error,"$Error_First_No_Rcard"));
				ucRCard.TextFocus(false, true);
				return;
			}
			else
			{
				_itemcode = sim.ItemCode;
			}
			#endregion

			//�ж��Ƿ��Ѿ���������
			//2006/11/17,Laws Lu add get DateTime from db Server
			DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

			DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);

			DateTime dtShiftDay = dtNow;
				
			/////���ǿ���İ�Σ������Ǵ��ڵڶ���,����ǰһ����Ϊ��������
			if(shift.IsOverDay == FormatHelper.TRUE_STRING)
			{
				if(FormatHelper.TOTimeInt(dtShiftDay) < shift.BeginTime)
					dtShiftDay = dtShiftDay.AddDays(-1);
			}
			
			this.cbxBeginTime.ComboBoxData.Items.Clear();
			this._shiftCode = shift.ShiftCode;
			this._shiftDate = FormatHelper.TODateInt(dtShiftDay);

			//ȡ�������׼���Ϣ
			object[] objs = _facade.QueryFirst(this.ucSSName.Value,this._shiftDate,_itemcode,this._shiftCode);
			if(objs == null || objs.Length == 0)
			{
				ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error,"�������׼���/����"));
				return;
			}

			foreach(BenQGuru.eMES.Domain.Alert.FirstOnline first in objs)
			{
				if(first != null)
				{
					//������ĩ�����ߺ����ߵ��ų���
					if(first.LastType == LineActionType.ON || first.LastType == LineActionType.OFF)
						continue;

					this.cbxBeginTime.ComboBoxData.Items.Add(FormatHelper.ToTimeString(first.ShiftTime));
				}
			}
			if(this.cbxBeginTime.ComboBoxData.Items.Count > 0)
				this.cbxBeginTime.ComboBoxData.SelectedIndex = 0;
		}

		//ִ��ĩ�����߶���
		private void ucBtnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(	this.cbxBeginTime.ComboBoxData.SelectedIndex == -1)
				{
					ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error,"����ѡ���׼����ϰ�ʱ��"));
					return;
				}

				Messages msg = new Messages();

				object obj_on = _facade.GetFirstOnline(this.ucSSName.Value,this._shiftDate,_itemcode,_shiftCode,FormatHelper.TOTimeInt(this.cbxBeginTime.ComboBoxData.SelectedItem.ToString()));
				BenQGuru.eMES.Domain.Alert.FirstOnline first_on = obj_on as BenQGuru.eMES.Domain.Alert.FirstOnline;
				if(first_on == null)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"�������׼���/����"));
					ApplicationRun.GetInfoForm().Add(msg);
					return;
				}
	
				if(first_on.LastType == LineActionType.ON)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"�Ѿ�����ĩ��������"));
					ApplicationRun.GetInfoForm().Add(msg);
					return;
				}

				if(first_on.LastType == LineActionType.OFF)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"�Ѿ�����ĩ��������"));
					ApplicationRun.GetInfoForm().Add(msg);
					return;
				}

				try
				{
					//2006/11/17,Laws Lu add get DateTime from db Server
					DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

					DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);

					first_on.LastOnRCard = this.ucRCard.Value.Trim();
					first_on.LastType = LineActionType.ON;
					first_on.LastOnTime = FormatHelper.TOTimeInt(dtNow);
					first_on.EndTime = FormatHelper.TOTimeInt(this.dtpEnd.Text);

					this.DataProvider.BeginTransaction();

					this.DataProvider.Update(first_on);

					ucMessage.Add("�°�ʱ��"+this.dtpEnd.Text+"," +
									"����" + first_on.SSCode + "," +
									"��Ʒ����" + first_on.ItemCode + "," +
									"ĩ������ʱ��" + FormatHelper.ToTimeString(first_on.LastOnTime)
								);
					
					this.DataProvider.CommitTransaction();
					msg.Add(new UserControl.Message(MessageType.Success,"ĩ�����߳ɹ�")); 
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
				this.ucRCard.Value = string.Empty;
				this.ucRCard.TextFocus(false, true);
				CloseConnection();
			}
		}

		private void ClearForm()
		{
			this._shiftDate = 0;
			this._itemcode = string.Empty;
			this._shiftCode = string.Empty;
			this.cbxBeginTime.ComboBoxData.Items.Clear();
		}

	}
}
