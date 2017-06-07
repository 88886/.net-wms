using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.DataCollect.Action;
using BenQGuru.eMES.Client.Service;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.MOModel;
using UserControl;

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FCollectionIDMerge ��ժҪ˵����
	/// </summary>
	public class FCollectionIDMerge : BaseForm
    {
        #region Controls
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
		private UserControl.UCMessage ucMessage;
		private UserControl.UCButton ucBtnCancel;
		private UserControl.UCButton ucBtnExit;
		private UserControl.UCButton ucBtnOK;
		private UserControl.UCButton ucBtnRecede;
		public UserControl.UCLabelEdit ucLERunningCard;
		/// <summary>
		/// ����������������
		/// </summary>
        private System.ComponentModel.Container components = null;
		private int _currSequence = 0;
		private int _mergeRule = 0;
		private ArrayList _runningCardList = null;
		private ProductInfo _product = null;
		private ActionOnLineHelper _helper = null;
		private string _idMergeType = string.Empty;
		private System.Windows.Forms.TextBox CollectedCount;
		private System.Windows.Forms.Label lblConvertQty;
        private System.Windows.Forms.Splitter splitter1;
		private UserControl.UCLabelEdit checkMO;
		private System.Windows.Forms.CheckBox chkUndo;

        private UCLabelEdit bCardTransLenULE;
        private UCLabelEdit aCardTransLenULE;
        private UCLabelEdit aCardTransLetterULE;
        private UCLabelEdit bCardTransLetterULE;

        #endregion

        private string _tCard = string.Empty;
        private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;

		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		private object[] transedRunningCardByProduct = null;	// Added by Icyer 2006/11/08
		public FCollectionIDMerge()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//			
			UserControl.UIStyleBuilder.FormUI(this);

			this._helper = new ActionOnLineHelper( this.DataProvider );
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCollectionIDMerge));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkUndo = new System.Windows.Forms.CheckBox();
            this.CollectedCount = new System.Windows.Forms.TextBox();
            this.lblConvertQty = new System.Windows.Forms.Label();
            this.ucBtnRecede = new UserControl.UCButton();
            this.ucBtnCancel = new UserControl.UCButton();
            this.ucLERunningCard = new UserControl.UCLabelEdit();
            this.ucBtnExit = new UserControl.UCButton();
            this.ucBtnOK = new UserControl.UCButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.aCardTransLetterULE = new UserControl.UCLabelEdit();
            this.aCardTransLenULE = new UserControl.UCLabelEdit();
            this.bCardTransLenULE = new UserControl.UCLabelEdit();
            this.checkMO = new UserControl.UCLabelEdit();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.bCardTransLetterULE = new UserControl.UCLabelEdit();
            this.ucMessage = new UserControl.UCMessage();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkUndo);
            this.groupBox1.Controls.Add(this.CollectedCount);
            this.groupBox1.Controls.Add(this.lblConvertQty);
            this.groupBox1.Controls.Add(this.ucBtnRecede);
            this.groupBox1.Controls.Add(this.ucBtnCancel);
            this.groupBox1.Controls.Add(this.ucLERunningCard);
            this.groupBox1.Controls.Add(this.ucBtnExit);
            this.groupBox1.Controls.Add(this.ucBtnOK);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 496);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(785, 90);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // chkUndo
            // 
            this.chkUndo.Location = new System.Drawing.Point(277, 19);
            this.chkUndo.Name = "chkUndo";
            this.chkUndo.Size = new System.Drawing.Size(201, 24);
            this.chkUndo.TabIndex = 15;
            this.chkUndo.Text = "����ת�����";
            // 
            // CollectedCount
            // 
            this.CollectedCount.Location = new System.Drawing.Point(553, 21);
            this.CollectedCount.Name = "CollectedCount";
            this.CollectedCount.ReadOnly = true;
            this.CollectedCount.Size = new System.Drawing.Size(61, 21);
            this.CollectedCount.TabIndex = 14;
            this.CollectedCount.Text = "0";
            // 
            // lblConvertQty
            // 
            this.lblConvertQty.Location = new System.Drawing.Point(477, 24);
            this.lblConvertQty.Name = "lblConvertQty";
            this.lblConvertQty.Size = new System.Drawing.Size(72, 15);
            this.lblConvertQty.TabIndex = 13;
            this.lblConvertQty.Text = "ת������";
            // 
            // ucBtnRecede
            // 
            this.ucBtnRecede.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnRecede.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnRecede.BackgroundImage")));
            this.ucBtnRecede.ButtonType = UserControl.ButtonTypes.Change;
            this.ucBtnRecede.Caption = "����";
            this.ucBtnRecede.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnRecede.Location = new System.Drawing.Point(307, 52);
            this.ucBtnRecede.Name = "ucBtnRecede";
            this.ucBtnRecede.Size = new System.Drawing.Size(88, 22);
            this.ucBtnRecede.TabIndex = 3;
            this.ucBtnRecede.Click += new System.EventHandler(this.ucBtnRecede_Click);
            // 
            // ucBtnCancel
            // 
            this.ucBtnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnCancel.BackgroundImage")));
            this.ucBtnCancel.ButtonType = UserControl.ButtonTypes.Cancle;
            this.ucBtnCancel.Caption = "ȡ��";
            this.ucBtnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnCancel.Location = new System.Drawing.Point(200, 52);
            this.ucBtnCancel.Name = "ucBtnCancel";
            this.ucBtnCancel.Size = new System.Drawing.Size(88, 22);
            this.ucBtnCancel.TabIndex = 2;
            this.ucBtnCancel.Click += new System.EventHandler(this.ucBtnCancel_Click);
            // 
            // ucLERunningCard
            // 
            this.ucLERunningCard.AllowEditOnlyChecked = true;
            this.ucLERunningCard.AutoSelectAll = false;
            this.ucLERunningCard.AutoUpper = true;
            this.ucLERunningCard.Caption = "�����";
            this.ucLERunningCard.Checked = false;
            this.ucLERunningCard.EditType = UserControl.EditTypes.String;
            this.ucLERunningCard.Location = new System.Drawing.Point(19, 19);
            this.ucLERunningCard.MaxLength = 40;
            this.ucLERunningCard.Multiline = false;
            this.ucLERunningCard.Name = "ucLERunningCard";
            this.ucLERunningCard.PasswordChar = '\0';
            this.ucLERunningCard.ReadOnly = false;
            this.ucLERunningCard.ShowCheckBox = false;
            this.ucLERunningCard.Size = new System.Drawing.Size(249, 24);
            this.ucLERunningCard.TabIndex = 0;
            this.ucLERunningCard.TabNext = false;
            this.ucLERunningCard.Value = "";
            this.ucLERunningCard.WidthType = UserControl.WidthTypes.Long;
            this.ucLERunningCard.XAlign = 68;
            this.ucLERunningCard.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucLERunningCard_TxtboxKeyPress);
            // 
            // ucBtnExit
            // 
            this.ucBtnExit.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnExit.BackgroundImage")));
            this.ucBtnExit.ButtonType = UserControl.ButtonTypes.Exit;
            this.ucBtnExit.Caption = "�˳�";
            this.ucBtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExit.Location = new System.Drawing.Point(413, 52);
            this.ucBtnExit.Name = "ucBtnExit";
            this.ucBtnExit.Size = new System.Drawing.Size(88, 22);
            this.ucBtnExit.TabIndex = 4;
            // 
            // ucBtnOK
            // 
            this.ucBtnOK.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnOK.BackgroundImage")));
            this.ucBtnOK.ButtonType = UserControl.ButtonTypes.Confirm;
            this.ucBtnOK.Caption = "ȷ��";
            this.ucBtnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnOK.Location = new System.Drawing.Point(93, 52);
            this.ucBtnOK.Name = "ucBtnOK";
            this.ucBtnOK.Size = new System.Drawing.Size(88, 22);
            this.ucBtnOK.TabIndex = 1;
            this.ucBtnOK.Click += new System.EventHandler(this.ucBtnOK_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.aCardTransLetterULE);
            this.panel1.Controls.Add(this.aCardTransLenULE);
            this.panel1.Controls.Add(this.bCardTransLenULE);
            this.panel1.Controls.Add(this.checkMO);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.bCardTransLetterULE);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(785, 104);
            this.panel1.TabIndex = 1;
            // 
            // aCardTransLetterULE
            // 
            this.aCardTransLetterULE.AllowEditOnlyChecked = true;
            this.aCardTransLetterULE.AutoSelectAll = false;
            this.aCardTransLetterULE.AutoUpper = true;
            this.aCardTransLetterULE.Caption = "ת�������к����ַ���";
            this.aCardTransLetterULE.Checked = false;
            this.aCardTransLetterULE.EditType = UserControl.EditTypes.String;
            this.aCardTransLetterULE.Location = new System.Drawing.Point(343, 66);
            this.aCardTransLetterULE.MaxLength = 40;
            this.aCardTransLetterULE.Multiline = false;
            this.aCardTransLetterULE.Name = "aCardTransLetterULE";
            this.aCardTransLetterULE.PasswordChar = '\0';
            this.aCardTransLetterULE.ReadOnly = false;
            this.aCardTransLetterULE.ShowCheckBox = true;
            this.aCardTransLetterULE.Size = new System.Drawing.Size(282, 24);
            this.aCardTransLetterULE.TabIndex = 20;
            this.aCardTransLetterULE.TabNext = false;
            this.aCardTransLetterULE.Value = "";
            this.aCardTransLetterULE.WidthType = UserControl.WidthTypes.Normal;
            this.aCardTransLetterULE.XAlign = 492;
            this.aCardTransLetterULE.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.aCardTransLetterULE_TxtboxKeyPress);
            // 
            // aCardTransLenULE
            // 
            this.aCardTransLenULE.AllowEditOnlyChecked = true;
            this.aCardTransLenULE.AutoSelectAll = false;
            this.aCardTransLenULE.AutoUpper = true;
            this.aCardTransLenULE.Caption = "ת�������кų���";
            this.aCardTransLenULE.Checked = false;
            this.aCardTransLenULE.EditType = UserControl.EditTypes.String;
            this.aCardTransLenULE.Location = new System.Drawing.Point(37, 66);
            this.aCardTransLenULE.MaxLength = 40;
            this.aCardTransLenULE.Multiline = false;
            this.aCardTransLenULE.Name = "aCardTransLenULE";
            this.aCardTransLenULE.PasswordChar = '\0';
            this.aCardTransLenULE.ReadOnly = false;
            this.aCardTransLenULE.ShowCheckBox = true;
            this.aCardTransLenULE.Size = new System.Drawing.Size(258, 24);
            this.aCardTransLenULE.TabIndex = 21;
            this.aCardTransLenULE.TabNext = false;
            this.aCardTransLenULE.Value = "";
            this.aCardTransLenULE.WidthType = UserControl.WidthTypes.Normal;
            this.aCardTransLenULE.XAlign = 162;
            // 
            // bCardTransLenULE
            // 
            this.bCardTransLenULE.AllowEditOnlyChecked = true;
            this.bCardTransLenULE.AutoSelectAll = false;
            this.bCardTransLenULE.AutoUpper = true;
            this.bCardTransLenULE.Caption = "ת��ǰ���кų���";
            this.bCardTransLenULE.Checked = false;
            this.bCardTransLenULE.EditType = UserControl.EditTypes.String;
            this.bCardTransLenULE.Location = new System.Drawing.Point(37, 38);
            this.bCardTransLenULE.MaxLength = 40;
            this.bCardTransLenULE.Multiline = false;
            this.bCardTransLenULE.Name = "bCardTransLenULE";
            this.bCardTransLenULE.PasswordChar = '\0';
            this.bCardTransLenULE.ReadOnly = false;
            this.bCardTransLenULE.ShowCheckBox = true;
            this.bCardTransLenULE.Size = new System.Drawing.Size(258, 24);
            this.bCardTransLenULE.TabIndex = 22;
            this.bCardTransLenULE.TabNext = false;
            this.bCardTransLenULE.Value = "";
            this.bCardTransLenULE.WidthType = UserControl.WidthTypes.Normal;
            this.bCardTransLenULE.XAlign = 162;
            // 
            // checkMO
            // 
            this.checkMO.AllowEditOnlyChecked = true;
            this.checkMO.AutoSelectAll = false;
            this.checkMO.AutoUpper = true;
            this.checkMO.Caption = "�������        ";
            this.checkMO.Checked = false;
            this.checkMO.EditType = UserControl.EditTypes.String;
            this.checkMO.Location = new System.Drawing.Point(37, 10);
            this.checkMO.MaxLength = 40;
            this.checkMO.Multiline = false;
            this.checkMO.Name = "checkMO";
            this.checkMO.PasswordChar = '\0';
            this.checkMO.ReadOnly = false;
            this.checkMO.ShowCheckBox = true;
            this.checkMO.Size = new System.Drawing.Size(258, 24);
            this.checkMO.TabIndex = 15;
            this.checkMO.TabNext = true;
            this.checkMO.Value = "";
            this.checkMO.WidthType = UserControl.WidthTypes.Normal;
            this.checkMO.XAlign = 162;
            this.checkMO.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.checkMO_TxtboxKeyPress);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(2, 104);
            this.splitter1.TabIndex = 13;
            this.splitter1.TabStop = false;
            // 
            // bCardTransLetterULE
            // 
            this.bCardTransLetterULE.AllowEditOnlyChecked = true;
            this.bCardTransLetterULE.AutoSelectAll = false;
            this.bCardTransLetterULE.AutoUpper = true;
            this.bCardTransLetterULE.Caption = "ת��ǰ���к����ַ���";
            this.bCardTransLetterULE.Checked = false;
            this.bCardTransLetterULE.EditType = UserControl.EditTypes.String;
            this.bCardTransLetterULE.Location = new System.Drawing.Point(343, 38);
            this.bCardTransLetterULE.MaxLength = 40;
            this.bCardTransLetterULE.Multiline = false;
            this.bCardTransLetterULE.Name = "bCardTransLetterULE";
            this.bCardTransLetterULE.PasswordChar = '\0';
            this.bCardTransLetterULE.ReadOnly = false;
            this.bCardTransLetterULE.ShowCheckBox = true;
            this.bCardTransLetterULE.Size = new System.Drawing.Size(282, 24);
            this.bCardTransLetterULE.TabIndex = 19;
            this.bCardTransLetterULE.TabNext = false;
            this.bCardTransLetterULE.Value = "";
            this.bCardTransLetterULE.WidthType = UserControl.WidthTypes.Normal;
            this.bCardTransLetterULE.XAlign = 492;
            // 
            // ucMessage
            // 
            this.ucMessage.BackColor = System.Drawing.Color.Gainsboro;
            this.ucMessage.ButtonVisible = false;
            this.ucMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMessage.Location = new System.Drawing.Point(0, 104);
            this.ucMessage.Name = "ucMessage";
            this.ucMessage.Size = new System.Drawing.Size(785, 392);
            this.ucMessage.TabIndex = 2;
            // 
            // FCollectionIDMerge
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(785, 586);
            this.Controls.Add(this.ucMessage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "FCollectionIDMerge";
            this.Text = "���к�ת���ɼ�";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FCollectionIDMerge_Load);
            this.Closed += new System.EventHandler(this.FCollectionIDMerge_Closed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Button Events
		private void ucBtnOK_Click(object sender, System.EventArgs e)
		{
			if ( this.ucLERunningCard.Value.Trim() == string.Empty )
			{
				ucLERunningCard.TextFocus(true, true);
				return;
			}	

			this.ucMessage.Add(string.Format("<< {0}", this.ucLERunningCard.Value.Trim().ToUpper() ));

            //ת������ʼ���к� Add By Bernard @ 2010-10-29
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
            string sourceCard = dataCollectFacade.GetSourceCard(this.ucLERunningCard.Value.Trim().ToUpper(), string.Empty);
            //end

            //Add By Bernard @ 2010-10-29 for Check��Ʒ���к�Hold
            //Simulation objSimulation = dataCollectFacade.GetLastSimulation(sourceCard) as Simulation;
            //if (objSimulation != null)
            //{
            //    if (dataCollectFacade.CheckRcardHold(sourceCard, objSimulation.MOCode.ToUpper().Trim()))
            //    {
            //        this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$RACRD_HOLD $CS_Param_ID=" + sourceCard));
            //        this.ucLERunningCard.Value = "";
            //        ucLERunningCard.TextFocus(true, true);
            //        return;
            //    }
            //}
            //end 
			
	        //Modified By Bernard @ 2010-10-29
			//Messages messages = this._helper.GetIDInfo(this.ucLERunningCard.Value.Trim().ToUpper());
            Messages messages = this._helper.GetIDInfo(sourceCard);

			if ( !messages.IsSuccess() )
			{
				this.ucMessage.Add( messages );
				this.ucLERunningCard.Value = "";

				//Laws Lu,2005/08/11,������������
				ucLERunningCard.TextFocus(true, true);
					
				return;
			}
		
			/* added by jessie lee, 2005/12/10 
			 * �ж� */
			bool isSameMO = false;
			bool updateSim = false ;
			decimal existIMEISeq = 0 ;

			// ����ְ�ǰ��Ʒ���к�
			if ( this._currSequence == 0 )
			{
                //Add By Bernard @ 2010-10-29
                _tCard = this.ucLERunningCard.Value.Trim().ToUpper();

				//Laws Lu,2005/10/19,����	������������
				//Laws Lu,2006/12/25 �޸�	����Open/Close�Ĵ���
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.AutoCloseConnection = true;
				//added by jessie lee, 2005/11/29
				#region �ж�ת��ǰ���к��Ƿ��������
				//���ȼ��
				if( bCardTransLenULE.Checked )
				{
					if( bCardTransLenULE.Value.Trim().Length == 0 )
					{
						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$CS_Before_Card_Transfer_Empty")); 
						ucLERunningCard.TextFocus(true, true);
						return ;
					}

					int len = 0;
					try
					{
						len = int.Parse(bCardTransLenULE.Value.Trim());
					}
					catch
					{
						this.ucMessage.Add( new UserControl.Message(MessageType.Error,"$Error_BeforeCardTransLen_Should_be_Integer"));
						ucLERunningCard.TextFocus(true, true);
						return ;
					}

					if( len != this.ucLERunningCard.Value.Trim().Length )
					{
						this.ucMessage.Add( new UserControl.Message(MessageType.Error,"$Error_BeforeCardTransLen_Not_Correct"));
						ucLERunningCard.TextFocus(true, true);
						return ;
					}
				}

				//���ַ������
				if(bCardTransLetterULE.Checked)
				{
					if( bCardTransLetterULE.Value.Trim().Length == 0 )
					{
						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$CS_Before_Card_Transfer_FLetter_Empty")); 
						ucLERunningCard.TextFocus(true, true);
						return ;
					}

					int index = ucLERunningCard.Value.Trim().IndexOf( bCardTransLetterULE.Value.Trim() );
					if( index != 0 )
					{
						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$CS_Before_Card_Transfer_FLetter_NotCompare")); 
						ucLERunningCard.TextFocus(true, true);
						return ;
					}
				}
				#endregion

				#region ȡ���Զ���ְ���� (Has Comment Out)
                /*Comment Out By Bernard @ 2010-10-29
				if ( this.ucLEIDMergeRule.Checked )
				{
					if ( this.ucLEIDMergeRule.Value.Trim() == string.Empty )
					{
						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$CS_Please_Input_IDMerge_Rule"));		//�������Զ���ְ����

						//Laws Lu,2005/08/11,������������
						ucLERunningCard.TextFocus(true, true);
						return;
					}
				
					int mergeRule = 1;
					
					try
					{
						mergeRule = System.Int32.Parse( this.ucLEIDMergeRule.Value.Trim() );
					}
					catch
					{
						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$Error_CS_IDMerge_Should_be_Integer"));//�ְ��������Ϊ����

						//Laws Lu,2005/08/11,������������
						ucLERunningCard.TextFocus(true, true);
						return;
					}

					if ( mergeRule <= 0 )
					{
						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$Error_CS_IDMerge_Should_Over_Zero"));//�ְ�������������

						//Laws Lu,2005/08/11,������������
						ucLERunningCard.TextFocus(true, true);
						return;
					}

					this._mergeRule = mergeRule;
				}
                 * */
				#endregion

				#region �жϷְ�ǰ���к��Ƿ����
				this._product= (ProductInfo)messages.GetData().Values[0];

                if (this._product.LastSimulation == null)
                {
                    // Added by Icyer 2006/11/08
                    // Undoʱ���޷�ȡ����ת�������к�Simulation����Ҫ��TCARD��ת�������к�
                    bool bNotExist = true;
                    if (this.chkUndo.Checked == true)
                    {
                        DataCollectFacade dcFacade = new DataCollectFacade(this.DataProvider);
                        transedRunningCardByProduct = dcFacade.GetSimulationFromTCard(this.ucLERunningCard.Value.Trim().ToUpper());
                        if (transedRunningCardByProduct != null && transedRunningCardByProduct.Length > 0)
                        {
                            this._product = this._helper.GetIDInfoBySimulation((Simulation)transedRunningCardByProduct[0]);
                            this._product.LastSimulation.RunningCard = this.ucLERunningCard.Value.Trim().ToUpper();
                            this._product.NowSimulation = (Simulation)this._product.LastSimulation;
                            bNotExist = false;
                        }
                    }
                    // Added end
                    if (bNotExist == true)
                    {
                        this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$Error_CS_ID_Not_Exist"));//���кŲ�����
                        this.ucLERunningCard.Value = "";

                        //Laws Lu,2005/08/11,������������
                        ucLERunningCard.TextFocus(true, true);
                        return;
                    }
                }
                else if (this.chkUndo.Checked == true)	// Added by Icyer 2006/11/08
                {
                    this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$IDMerge_Undo_RunningCard_OnLine"));
                    this.ucLERunningCard.Value = "";
                    ucLERunningCard.TextFocus(true, true);
                    return;
                }

				//added by jessie lee, 2006/7/21
				#region ���к�ת�����ӹ�����������

                if (checkMO.Checked
                    && string.Compare(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(checkMO.Value)), this._product.LastSimulation.MOCode, true) != 0)
                {
                    this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$Error_MO_Not_Match"));//���кŲ�����
                    this.ucLERunningCard.Value = "";

                    //Laws Lu,2005/08/11,������������
                    ucLERunningCard.TextFocus(true, true);
                    return;
                }
 
				#endregion

				// Added by Icyer 2006/11/08
				// ֻ�������ɼ�ʱ�ŵ���CheckID���;��
                if (this.chkUndo.Checked == false)
                {
                    messages = new DataCollectFacade(this.DataProvider).CheckID(
                        this._product.LastSimulation.RunningCard,
                        ActionType.DataCollectAction_Split,
                        ApplicationService.Current().ResourceCode,
                        ApplicationService.Current().UserCode,
                        this._product);
                }

				if ( !messages.IsSuccess() )
				{
					this.ucMessage.Add( messages );
					this.ucLERunningCard.Value = "";

					//Laws Lu,2005/08/11,������������
					ucLERunningCard.TextFocus(true, true);
					return;
				}
				#endregion

				#region �ж��Ƿ�ְ幤��ȡϵͳ�ְ����
                object op = new ItemFacade(this.DataProvider).GetItemRoute2Operation(this._product.NowSimulation.ItemCode,
                    this._product.NowSimulation.RouteCode,
                    this._product.NowSimulation.OPCode);

                if (op == null)
                {
                    this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$Error_CS_Current_OP_Not_Exist"));//��ǰ���򲻴���
                    this.ucLERunningCard.Value = "";

                    //Laws Lu,2005/08/11,������������
                    ucLERunningCard.TextFocus(true, true);

                    return;
                }

                if (((ItemRoute2OP)op).OPControl[(int)OperationList.IDTranslation] != '1')
                {
                    this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$CS_OP_Not_SplitOP"));//��ǰ���������ת������
                    this.ucLERunningCard.Value = "";

                    //Laws Lu,2005/08/11,������������
                    ucLERunningCard.TextFocus(true, true);

                    return;
                }

                #region Comment Out By Bernard @ 2010-10-29

                // ȡϵͳ�ְ����
				//if ( ((ItemRoute2OP)op).IDMergeType == IDMergeType.IDMERGETYPE_ROUTER )
				//{
					// Removed by Icyer 2006/11/08
					/*
					//AMOI  MARK  START  20050804 �����ŵ�һλ����ΪD  С����ŵ�һλ����ΪB 
					if  (this.ucLERunningCard.Value[0]!='D')
					{
						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$CS_theFirstOfBIGID_MustBe_D"));
						this.ucLERunningCard.Value = "";	
				
						//Laws Lu,2005/08/11,������������
						ucLERunningCard.TextFocus(true, true);
						return;
					}
					//AMOI  MARK  END
					*/
                
					// ȡ��ǰ����ķְ����
			/* Comment Out		if ( !this.ucLEIDMergeRule.Checked )
					{
						this._mergeRule = (int)((ItemRoute2OP)op).IDMergeRule;
					}
				}
				else
				{
					this._mergeRule = 1;
                }
             *Comment Out */

                #endregion Comment Out
                //Add By Bernard @2010-10-29
                this._mergeRule=1;

                #endregion

                // ȡ���ת������
				this._idMergeType = ((ItemRoute2OP)op).IDMergeType;
				this._runningCardList = new ArrayList( this._mergeRule );
				// Added by Icyer 2006/11/08, �����Undo�����Զ�����ԭת�������к�
				if (this.chkUndo.Checked == true)
				{
					// ������к��Ƿ��ڵ�ǰ����
					if (this._product.LastSimulation.ResourceCode != Service.ApplicationService.Current().ResourceCode)
					{
						object objTmp = (new BaseSetting.BaseModelFacade(this.DataProvider)).GetOperation2Resource(this._product.LastSimulation.OPCode, Service.ApplicationService.Current().ResourceCode);
						if (objTmp == null)
						{
							this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$IDMerge_Undo_CurrentOP_Error"));
							ucLERunningCard.TextFocus(true, true);
							return;
						}
					}
					// ��ʾԭת�������к��б�
					if (transedRunningCardByProduct != null && transedRunningCardByProduct.Length > 0)
					{
						this.ucMessage.AddBoldText("$IDMerge_Undo_Old_RunningCard_List:");
						for (int i = 0; i < transedRunningCardByProduct.Length; i++)
						{
                            //Modified By Bernard @ 2010-10-29
							//this.ucMessage.Add(((Simulation)transedRunningCardByProduct[i]).RunningCard);
                            this.ucMessage.Add(((Simulation)transedRunningCardByProduct[i]).TranslateCard);
						}
					}
				}
				// Added end
			}
			else
			{
				#region ���߼��Ѿ�ͣ��, jessie lee, 2005/11/29

				//AMOI  MARK  START  20050804 �����ŵ�һλ����ΪD  С����ŵ�һλ����ΪB 
//				if (this._idMergeType == IDMergeType.IDMERGETYPE_ROUTER )
//				{
//					if  (this.ucLERunningCard.Value.Trim().ToUpper()[0]!='B')
//					{
//						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$CS_theFirstOfSmallID_MustBe_B"));
//						this.ucLERunningCard.Value = "";					
//						Laws Lu,2005/08/11,������������
//						ucLERunningCard.TextFocus(true, true);
//						return;
//					}
//				}
				//AMOI  MARK  END
				#endregion

				//added by jessie lee, 2005/11/29
				#region �ж�ת�������к��Ƿ��������
				//���ȼ��
				if( aCardTransLenULE.Checked )
				{
                    if (aCardTransLenULE.Value.Trim().Length == 0)
                    {
                        this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$CS_After_Card_Transfer_Empty"));
                        ucLERunningCard.TextFocus(true, true);
                        return;
                    }

					int len = 0;
                    try
                    {
                        len = int.Parse(aCardTransLenULE.Value.Trim());
                    }
                    catch
                    {
                        this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$Error_AfterCardTransLen_Should_be_Integer"));
                        ucLERunningCard.TextFocus(true, true);
                        return;
                    }

					if( len != this.ucLERunningCard.Value.Trim().Length )
					{
						this.ucMessage.Add( new UserControl.Message(MessageType.Error,"$Error_AfterCardTransLen_Not_Correct"));
						ucLERunningCard.TextFocus(true, true);
						return ;
					}
				}

				//���ַ������
				if(aCardTransLetterULE.Checked)
				{
					if( aCardTransLetterULE.Value.Trim().Length == 0 )
					{
						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$CS_After_Card_Transfer_FLetter_Empty")); 
						ucLERunningCard.TextFocus(true, true);
						return ;
					}

					int index = ucLERunningCard.Value.Trim().IndexOf( aCardTransLetterULE.Value.Trim() );
					if( index != 0 )
					{
						this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$CS_After_Card_Transfer_FLetter_NotCompare")); 
						ucLERunningCard.TextFocus(true, true);
						return ;
					}
				}
				#endregion

				// �жϷְ�����к��Ƿ��Ѵ���

                if (((ProductInfo)messages.GetData().Values[0]).LastSimulation != null)
                {
                    /* modified by jessie lee, 2005/12/9, CS188-3
                     * 1,��װ�εĲ�Ʒ�����򱨷Ϻ󣬿����ٴ�Ͷ�뵽��װ�Σ���������EMPƽ̨���ֻ��޷�ʹ���µ�IMEI�ţ�
                     * �ʹ���������IMEI�ŵ����⣬���Ҹ�IMEI�����п���Ͷ�뵽ԭ���Ĺ��������ʸ��ͻ��Ľ�������ǲ�����
                     * ��򱨷Ϻ���ֻ��ٴ�Ͷ���װʱճ���µ�M���루IMEI���ת��ǰ�ĺ��룩���ú�����Թ�����ԭ���Ĺ�
                     * �����¹��������ת��ʱϵͳ����M����ת��������ʹ�ù���IMEI��
                     * 2,RMA�ȿ��߷�����EMPƽ̨�Ĳ�Ʒ��������ʱͬ�����ܴ����ظ�ʹ��IMEI�ŵ����⡣���ʸ����ĵ�һ�ֽ�
                     * ����ʽ�ǿ����������͵Ĺ�������IMEI��Ͷ���µķ�����������ʼ������������µ��������̲���Ҫ��
                     * �����ת�����򣻵ڶ��ַ�ʽ��ճ���µ�M����Ͷ�뵽���������У����ת��ʱ���������IMEI�����ظ�ʹ
                     * ������뱣֤ԭ��ʹ��IMEI�ŵĹ����Ѿ��ص����ͻ�Ҫ����õڶ��ַ����� 
                     * 
                     * 2�пͻ�Ҫ����õڶ��ַ�������ô�ɼ�������rcard��û��simulation��¼�ģ����Բ����������check
                     * */
                    /* �������к�ת�����򣬱���ԭ�����߼� */
                    if (string.Compare(this._idMergeType, IDMergeType.IDMERGETYPE_IDMERGE, true) != 0)
                    {
                        this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$Error_CS_ID_Already_Exist"));//���к��Ѵ���
                        this.ucLERunningCard.Value = "";

                        //Laws Lu,2005/08/11,������������
                        ucLERunningCard.TextFocus(true, true);
                        return;
                    }

                    /* �����к�ת������ 
                     * ת��ǰ��rcard �� ת�����rcard  ����ͬ
                     * ��ͬ�� check IMEI �ظ�ʹ��
                     * */
                    if (string.Compare(this._product.LastSimulation.RunningCard, this.ucLERunningCard.Value.Trim(), true) != 0)
                    {
                        string bMoCode = this._product.LastSimulation.MOCode;
                        string aMoCode = ((ProductInfo)messages.GetData().Values[0]).LastSimulation.MOCode;

                        /* �ж����IMEI���Ƿ񱨷ϻ��߲�� */
                        bool isSpliteOrScrape = CheckIMEISpliteOrScrape(
                            ((ProductInfo)messages.GetData().Values[0]).LastSimulation.RunningCard,
                            ((ProductInfo)messages.GetData().Values[0]).LastSimulation.RunningCardSequence,
                            aMoCode);
                        if (!isSpliteOrScrape)
                        {
                            /* rcard �깤������δ�� */
                            if (((ProductInfo)messages.GetData().Values[0]).LastSimulation.IsComplete != "1"
                                && ((ProductInfo)messages.GetData().Values[0]).LastSimulation.ProductStatus != ProductStatus.OffMo)
                            {
                                this.ucMessage.Add(new UserControl.Message(MessageType.Error, "$Error_CS_ID_Already_Exist"));//���к��Ѵ���
                                this.ucLERunningCard.Value = "";

                                //Laws Lu,2005/08/11,������������
                                ucLERunningCard.TextFocus(true, true);
                                return;
                            }
                        }

                        /* ����ͬһ�Ź��� */
                        if (string.Compare(bMoCode, aMoCode, true) == 0)
                        {
                            isSameMO = true;
                            //existIMEISeq = ((ProductInfo)messages.GetData().Values[0]).LastSimulation.RunningCardSequence ;

                        }
                        else
                        {
                            /* ������ͬ���� */
                            isSameMO = false;

                        }
                        existIMEISeq = ((ProductInfo)messages.GetData().Values[0]).LastSimulation.RunningCardSequence;
                        updateSim = true;

                    }
                    else /* rcard == tcard */
                    {
                        isSameMO = true;
                        existIMEISeq = ((ProductInfo)messages.GetData().Values[0]).LastSimulation.RunningCardSequence;
                    }
                }

				// �жϷְ�����к��Ƿ��ظ�
				if ( this._runningCardList.Contains(this.ucLERunningCard.Value.Trim().ToUpper()) ) 
				{
					this.ucMessage.Add(new UserControl.Message(MessageType.Error,"$Error_CS_Merge_ID_Exist"));//ת�����Ʒ���к��ظ�
					this.ucLERunningCard.Value = "";
					
					//Laws Lu,2005/08/11,������������
					ucLERunningCard.TextFocus(true, true);
					return;
				}

				this._runningCardList.Add( this.ucLERunningCard.Value.Trim().ToUpper() );
			}

			if ( this._currSequence < this._mergeRule )
			{
				this._currSequence++;

                //Comment Out By Bernard @ 2010-10-29
				//this.ucLEIDMergeRule.Enabled = false;
				this.ucMessage.AddBoldText( string.Format(">>$CS_Please_Input_Merge_ID {0}/{1}", this._currSequence.ToString(), this._mergeRule.ToString()));//������ת�����Ʒ���к�
			}
			else if ( this._currSequence == this._mergeRule ) // �ﵽ�ְ����,д�����ݿ�
			{
		
				messages = this.doAction(isSameMO, int.Parse(existIMEISeq.ToString()), updateSim);

				if ( messages.IsSuccess() )	// �ɹ�
				{
					this.ucMessage.Add(new UserControl.Message(MessageType.Success,">>$CS_SplitID_CollectSuccess"));//��Ʒ���к�ת���ɼ��ɹ�
					//added by jessie lee, 2005/11/29,
					#region ��Ӽ�������
					int count = int.Parse(this.CollectedCount.Text)+1;
					this.CollectedCount.Text = count.ToString();
					#endregion
				}
				else						// ʧ��
				{
					this.ucMessage.Add( messages );
				}

				this.initInput();
			}

			this.ucLERunningCard.Value = "";

			//Laws Lu,2005/08/11,������������
			if(!aCardTransLetterULE.Checked)
			{
				ucLERunningCard.TextFocus(true, true);

				//SendKeys.Send("+{TAB}");
			}
			/*	Removed by Icyer 2006/12/11
			else if(this._currSequence == 0 )
			{
				aCardTransLetterULE.TextFocus(true, true);
			}
			*/
			else
			{
				ucLERunningCard.TextFocus(true, true);
			}
		}

		private void ucBtnCancel_Click(object sender, System.EventArgs e)
		{
			this.initInput();
		}

        private void ucBtnRecede_Click(object sender, System.EventArgs e)
        {
            if (this._currSequence > 0)
            {
                if (this._runningCardList.Count > 0)
                {
                    this._runningCardList.RemoveAt(this._runningCardList.Count - 1);
                }
                this._currSequence--;

                if (this._currSequence > 0)
                {
                    this.ucMessage.Add(string.Format(">>$CS_Please_Input_Merge_ID {0}/{1}", this._currSequence.ToString(), this._mergeRule.ToString()));
                }
                else
                {
                    this.ucMessage.Add(">>$CS_Please_Input_ID_To_Merge");//������ת��ǰ��Ʒ���к�
                    //Comment Out By Bernard @ 2010-10-29 
                    //this.ucLEIDMergeRule.Enabled = true;
                }
            }

            //Laws Lu,2005/08/11,������������
            ucLERunningCard.TextFocus(true, true);
        }
		#endregion

		#region Events
		public void ucLERunningCard_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == '\r')
			{
				this.ucBtnOK_Click( sender, null );

				//Laws Lu,2005/08/11,������������,ע��
				//SendKeys.Send("+{TAB}");
			}
		}

		private void FCollectionIDMerge_Load(object sender, System.EventArgs e)
		{
			this.initInput();
            //this.InitPageLanguage();
		}
		#endregion

		#region Function
		private void initInput()
		{
			this.chkUndo.Checked = false;	// Added by Icyer 2006/11/08
			this._currSequence = 0;
            //Commemt Out By Bernard @ 2010-10-29
			//this.ucLEIDMergeRule.Enabled = true;
			this.ucMessage.Add(">>$CS_Please_Input_ID_To_Merge");

			this.ucLERunningCard.TextFocus(false, true);
		}

		private Messages doAction(bool IsSameMO, int ExistIMEISeq, bool UpdateSimulation)
		{
			
			Messages messages = new Messages();

            /* Comment Out By Bernard @ 2010-10-29
			SplitIDActionEventArgs args = new SplitIDActionEventArgs(
				ActionType.DataCollectAction_Split, 
				this._product.LastSimulation.RunningCard, 
				ApplicationService.Current().UserCode,
				ApplicationService.Current().ResourceCode,
				this._product, 
				(object[])this._runningCardList.ToArray(),
				this._idMergeType,
				IsSameMO,
				ExistIMEISeq,
				UpdateSimulation);
             * */

            //Add By Bernard @ 2010-10-29
            ConvertCardActionEventArgs args = new ConvertCardActionEventArgs(
                ActionType.DataCollectAction_Convert,
                _product.LastSimulation.RunningCard,
                _tCard,
               ApplicationService.Current().UserCode,
                 ApplicationService.Current().ResourceCode,
                _product,
                _runningCardList[0].ToString(),
                this._idMergeType,
                IsSameMO,
                ExistIMEISeq,
                true);

			// Added by Icyer 2006/11/08
			if (this.chkUndo.Checked == true)
			{
				args.IsUndo = true;
			}
			// Added end

            //Modified By Bernard @ 2010-10-29
			//IAction action = new ActionFactory(this.DataProvider).CreateAction(ActionType.DataCollectAction_Split);
            IAction action = new ActionFactory(this.DataProvider).CreateAction(ActionType.DataCollectAction_Convert);
	
			//Laws Lu,2005/10/19,����	������������
			((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
			DataProvider.BeginTransaction();
			try
			{
				
				messages.AddMessages(action.Execute(args));	

				if ( messages.IsSuccess() )
				{
					this.DataProvider.CommitTransaction();
					messages.Add( new UserControl.Message(MessageType.Success,"$CS_SplitID_CollectSuccess") );
				}
				else
				{
					this.DataProvider.RollbackTransaction();
				}

				return messages;
			}
			catch(Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				throw ex;
			}
			finally
			{
				//Laws Lu,2005/10/19,����	������������
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
			}
		}
		#endregion

		private void FCollectionIDMerge_Closed(object sender, System.EventArgs e)
		{
			if (this.DataProvider!=null)
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection();  
		}

		private void panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		/// <summary>
		/// ��鵱ǰrcard�Ƿ�����򱨷�
		/// </summary>
		/// <param name="rcard"></param>
		/// <param name="rcardseq"></param>
		/// <param name="mocode"></param>
		/// <returns></returns>
        private bool CheckIMEISpliteOrScrape(string rcard, decimal rcardseq, string mocode)
        {
            string sql = string.Format(" select count(*) from tblts where rcard='{0}' and rcardseq={1} and mocode='{2}' and tsstatus in ('{3}','{4}')",
                rcard, rcardseq, mocode, TSStatus.TSStatus_Scrap, TSStatus.TSStatus_Split);
            int count = this.DataProvider.GetCount(new SQLCondition(sql));
            if (count > 0)
            {
                return true;
            }
            return false;
        }

		private void aCardTransLetterULE_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == '\r')
			{
				ucLERunningCard.TextFocus(true, true);
			}
		}

        //Method Add by Bernard @ 2010-10-29
        private void checkMO_TxtboxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.ucLERunningCard.TextFocus(false, true);
            }
        }

	}
}
