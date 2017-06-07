using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

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

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FStockInput ��ժҪ˵����
	/// </summary>
	public class FStockInput : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox grpQuery;
		private Infragistics.Win.UltraWinEditors.UltraOptionSet ultraOsType;
		private UserControl.UCLabelEdit ucLEInput;
		private System.Windows.Forms.Panel panel1;
		private UserControl.UCButton ucBtnRemove;
		private UserControl.UCButton ucBtnExit;
		private UserControl.UCButton ucBtnSave;
		private System.Windows.Forms.Panel panel2;
		private UserControl.UCButton ucBtnDelete;
		private UserControl.UCLabelEdit ucLETicketNo;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Data.DataColumn dataColumn8;
		private System.Data.DataColumn dataColumn11;
		private System.Data.DataColumn dataColumn12;
		private System.Data.DataColumn dataColumn1;
		private System.Data.DataSet dsStockIn;
		private System.Data.DataTable dtStockIn;
		private UserControl.UCLabelCombox ucLCModel;
		private UserControl.UCLabelEdit ucLEMOCode;
		private System.Data.DataColumn dataColumn2;

		private DataTable _tmpTable = new DataTable();
		UltraWinGridHelper ultraWinGridHelper;
		private System.Windows.Forms.Panel panel3;
		private UserControl.UCLabelEdit txtSumNum;

		public string DeleteInfor;
		private UserControl.UCLabelEdit txtMEMO;

		private Hashtable sum = new Hashtable();
		private System.Data.DataColumn dataColumn3;
		private System.Data.DataColumn dataColumn4;
		private System.Windows.Forms.Panel panel4;
		private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridContent;
		
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FStockInput()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
			UserControl.UIStyleBuilder.FormUI(this);	
			UserControl.UIStyleBuilder.UltraOptionSetUI(this.ultraOsType);
		
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
		
		private MaterialStockFacade _facade = null;

		private IDomainDataProvider _domainDataProvider =ApplicationService.Current().DataProvider;
		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		protected void ShowMessage(string message)
		{
			ApplicationRun.GetInfoForm().Add( message );
		}

		protected void ShowMessage(Messages messages)
		{			
			ApplicationRun.GetInfoForm().Add(messages);
		}

		protected void ShowMessage(UserControl.Message message)
		{			
			ApplicationRun.GetInfoForm().Add(message);
		}


		protected void ShowMessage(Exception e)
		{			
			ApplicationRun.GetInfoForm().Add(new UserControl.Message(e));
		}

		private enum CollectionType
		{
			Planate, PCS
		}

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FStockInput));
            this.grpQuery = new System.Windows.Forms.GroupBox();
            this.txtMEMO = new UserControl.UCLabelEdit();
            this.ucLEMOCode = new UserControl.UCLabelEdit();
            this.ucLCModel = new UserControl.UCLabelCombox();
            this.ucLEInput = new UserControl.UCLabelEdit();
            this.ultraOsType = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucBtnRemove = new UserControl.UCButton();
            this.ucBtnExit = new UserControl.UCButton();
            this.ucBtnSave = new UserControl.UCButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ucBtnDelete = new UserControl.UCButton();
            this.ucLETicketNo = new UserControl.UCLabelEdit();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ultraGridContent = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtSumNum = new UserControl.UCLabelEdit();
            this.dsStockIn = new System.Data.DataSet();
            this.dtStockIn = new System.Data.DataTable();
            this.dataColumn8 = new System.Data.DataColumn();
            this.dataColumn11 = new System.Data.DataColumn();
            this.dataColumn12 = new System.Data.DataColumn();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.grpQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOsType)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridContent)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsStockIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStockIn)).BeginInit();
            this.SuspendLayout();
            // 
            // grpQuery
            // 
            this.grpQuery.Controls.Add(this.txtMEMO);
            this.grpQuery.Controls.Add(this.ucLEMOCode);
            this.grpQuery.Controls.Add(this.ucLCModel);
            this.grpQuery.Controls.Add(this.ucLEInput);
            this.grpQuery.Controls.Add(this.ultraOsType);
            this.grpQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpQuery.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpQuery.Location = new System.Drawing.Point(0, 37);
            this.grpQuery.Name = "grpQuery";
            this.grpQuery.Size = new System.Drawing.Size(552, 149);
            this.grpQuery.TabIndex = 1;
            this.grpQuery.TabStop = false;
            this.grpQuery.Text = "�ɼ���ʽ";
            // 
            // txtMEMO
            // 
            this.txtMEMO.AllowEditOnlyChecked = true;
            this.txtMEMO.Caption = "��ע";
            this.txtMEMO.Checked = false;
            this.txtMEMO.EditType = UserControl.EditTypes.String;
            this.txtMEMO.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMEMO.Location = new System.Drawing.Point(20, 82);
            this.txtMEMO.MaxLength = 80;
            this.txtMEMO.Multiline = true;
            this.txtMEMO.Name = "txtMEMO";
            this.txtMEMO.PasswordChar = '\0';
            this.txtMEMO.ReadOnly = false;
            this.txtMEMO.ShowCheckBox = false;
            this.txtMEMO.Size = new System.Drawing.Size(197, 56);
            this.txtMEMO.TabIndex = 7;
            this.txtMEMO.TabNext = true;
            this.txtMEMO.Value = "";
            this.txtMEMO.WidthType = UserControl.WidthTypes.Long;
            this.txtMEMO.XAlign = 51;
            // 
            // ucLEMOCode
            // 
            this.ucLEMOCode.AllowEditOnlyChecked = true;
            this.ucLEMOCode.Caption = "����";
            this.ucLEMOCode.Checked = false;
            this.ucLEMOCode.EditType = UserControl.EditTypes.String;
            this.ucLEMOCode.Location = new System.Drawing.Point(365, 15);
            this.ucLEMOCode.MaxLength = 40;
            this.ucLEMOCode.Multiline = false;
            this.ucLEMOCode.Name = "ucLEMOCode";
            this.ucLEMOCode.PasswordChar = '\0';
            this.ucLEMOCode.ReadOnly = false;
            this.ucLEMOCode.ShowCheckBox = false;
            this.ucLEMOCode.Size = new System.Drawing.Size(142, 22);
            this.ucLEMOCode.TabIndex = 3;
            this.ucLEMOCode.TabNext = false;
            this.ucLEMOCode.Value = "";
            this.ucLEMOCode.WidthType = UserControl.WidthTypes.Normal;
            this.ucLEMOCode.XAlign = 396;
            // 
            // ucLCModel
            // 
            this.ucLCModel.AllowEditOnlyChecked = true;
            this.ucLCModel.Caption = "��Ʒ��";
            this.ucLCModel.Checked = false;
            this.ucLCModel.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucLCModel.Location = new System.Drawing.Point(142, 15);
            this.ucLCModel.Name = "ucLCModel";
            this.ucLCModel.SelectedIndex = -1;
            this.ucLCModel.ShowCheckBox = false;
            this.ucLCModel.Size = new System.Drawing.Size(207, 19);
            this.ucLCModel.TabIndex = 2;
            this.ucLCModel.WidthType = UserControl.WidthTypes.Long;
            this.ucLCModel.XAlign = 183;
            // 
            // ucLEInput
            // 
            this.ucLEInput.AllowEditOnlyChecked = true;
            this.ucLEInput.Caption = "�����";
            this.ucLEInput.Checked = false;
            this.ucLEInput.EditType = UserControl.EditTypes.String;
            this.ucLEInput.Location = new System.Drawing.Point(14, 45);
            this.ucLEInput.MaxLength = 1000;
            this.ucLEInput.Multiline = false;
            this.ucLEInput.Name = "ucLEInput";
            this.ucLEInput.PasswordChar = '\0';
            this.ucLEInput.ReadOnly = false;
            this.ucLEInput.ShowCheckBox = false;
            this.ucLEInput.Size = new System.Drawing.Size(374, 22);
            this.ucLEInput.TabIndex = 1;
            this.ucLEInput.TabNext = false;
            this.ucLEInput.Value = "";
            this.ucLEInput.WidthType = UserControl.WidthTypes.TooLong;
            this.ucLEInput.XAlign = 55;
            this.ucLEInput.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucLEInput_TxtboxKeyPress);
            // 
            // ultraOsType
            // 
            this.ultraOsType.BackColor = System.Drawing.SystemColors.Control;
            this.ultraOsType.BackColorInternal = System.Drawing.SystemColors.Control;
            this.ultraOsType.ImageTransparentColor = System.Drawing.Color.Gainsboro;
            valueListItem1.DisplayText = "��ά����     ";
            valueListItem2.DisplayText = "PCS";
            this.ultraOsType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.ultraOsType.Location = new System.Drawing.Point(7, 15);
            this.ultraOsType.Name = "ultraOsType";
            this.ultraOsType.Size = new System.Drawing.Size(129, 15);
            this.ultraOsType.TabIndex = 0;
            this.ultraOsType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ucBtnRemove);
            this.panel1.Controls.Add(this.ucBtnExit);
            this.panel1.Controls.Add(this.ucBtnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 449);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(552, 37);
            this.panel1.TabIndex = 3;
            // 
            // ucBtnRemove
            // 
            this.ucBtnRemove.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnRemove.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnRemove.BackgroundImage")));
            this.ucBtnRemove.ButtonType = UserControl.ButtonTypes.Move;
            this.ucBtnRemove.Caption = "�Ƴ�";
            this.ucBtnRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnRemove.Location = new System.Drawing.Point(87, 8);
            this.ucBtnRemove.Name = "ucBtnRemove";
            this.ucBtnRemove.Size = new System.Drawing.Size(88, 22);
            this.ucBtnRemove.TabIndex = 0;
            this.ucBtnRemove.Click += new System.EventHandler(this.ucBtnRemove_Click);
            // 
            // ucBtnExit
            // 
            this.ucBtnExit.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnExit.BackgroundImage")));
            this.ucBtnExit.ButtonType = UserControl.ButtonTypes.Exit;
            this.ucBtnExit.Caption = "�˳�";
            this.ucBtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExit.Location = new System.Drawing.Point(300, 8);
            this.ucBtnExit.Name = "ucBtnExit";
            this.ucBtnExit.Size = new System.Drawing.Size(88, 22);
            this.ucBtnExit.TabIndex = 2;
            // 
            // ucBtnSave
            // 
            this.ucBtnSave.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnSave.BackgroundImage")));
            this.ucBtnSave.ButtonType = UserControl.ButtonTypes.Save;
            this.ucBtnSave.Caption = "����";
            this.ucBtnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnSave.Location = new System.Drawing.Point(193, 8);
            this.ucBtnSave.Name = "ucBtnSave";
            this.ucBtnSave.Size = new System.Drawing.Size(88, 22);
            this.ucBtnSave.TabIndex = 1;
            this.ucBtnSave.Click += new System.EventHandler(this.ucBtnSave_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ucBtnDelete);
            this.panel2.Controls.Add(this.ucLETicketNo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(552, 37);
            this.panel2.TabIndex = 0;
            // 
            // ucBtnDelete
            // 
            this.ucBtnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnDelete.BackgroundImage")));
            this.ucBtnDelete.ButtonType = UserControl.ButtonTypes.None;
            this.ucBtnDelete.Caption = "ɾ����ⵥ";
            this.ucBtnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnDelete.Location = new System.Drawing.Point(300, 7);
            this.ucBtnDelete.Name = "ucBtnDelete";
            this.ucBtnDelete.Size = new System.Drawing.Size(88, 22);
            this.ucBtnDelete.TabIndex = 1;
            this.ucBtnDelete.Click += new System.EventHandler(this.ucBtnDelete_Click);
            // 
            // ucLETicketNo
            // 
            this.ucLETicketNo.AllowEditOnlyChecked = true;
            this.ucLETicketNo.Caption = "��ⵥ��";
            this.ucLETicketNo.Checked = false;
            this.ucLETicketNo.EditType = UserControl.EditTypes.String;
            this.ucLETicketNo.Location = new System.Drawing.Point(14, 6);
            this.ucLETicketNo.MaxLength = 40;
            this.ucLETicketNo.Multiline = false;
            this.ucLETicketNo.Name = "ucLETicketNo";
            this.ucLETicketNo.PasswordChar = '\0';
            this.ucLETicketNo.ReadOnly = false;
            this.ucLETicketNo.ShowCheckBox = false;
            this.ucLETicketNo.Size = new System.Drawing.Size(218, 23);
            this.ucLETicketNo.TabIndex = 0;
            this.ucLETicketNo.TabNext = false;
            this.ucLETicketNo.Value = "";
            this.ucLETicketNo.WidthType = UserControl.WidthTypes.Long;
            this.ucLETicketNo.XAlign = 65;
            this.ucLETicketNo.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucLETicketNo_TxtboxKeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 186);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(552, 263);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "�����ϸ";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ultraGridContent);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 17);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(546, 205);
            this.panel4.TabIndex = 5;
            // 
            // ultraGridContent
            // 
            this.ultraGridContent.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraGridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridContent.Location = new System.Drawing.Point(0, 0);
            this.ultraGridContent.Name = "ultraGridContent";
            this.ultraGridContent.Size = new System.Drawing.Size(546, 205);
            this.ultraGridContent.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtSumNum);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 222);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(546, 38);
            this.panel3.TabIndex = 4;
            // 
            // txtSumNum
            // 
            this.txtSumNum.AllowEditOnlyChecked = true;
            this.txtSumNum.Caption = "�����ܼ�";
            this.txtSumNum.Checked = false;
            this.txtSumNum.EditType = UserControl.EditTypes.String;
            this.txtSumNum.Location = new System.Drawing.Point(314, 7);
            this.txtSumNum.MaxLength = 40;
            this.txtSumNum.Multiline = false;
            this.txtSumNum.Name = "txtSumNum";
            this.txtSumNum.PasswordChar = '\0';
            this.txtSumNum.ReadOnly = true;
            this.txtSumNum.ShowCheckBox = false;
            this.txtSumNum.Size = new System.Drawing.Size(134, 23);
            this.txtSumNum.TabIndex = 1;
            this.txtSumNum.TabNext = false;
            this.txtSumNum.Value = "0";
            this.txtSumNum.WidthType = UserControl.WidthTypes.Small;
            this.txtSumNum.XAlign = 365;
            // 
            // dsStockIn
            // 
            this.dsStockIn.DataSetName = "dsStockIn";
            this.dsStockIn.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.dsStockIn.Tables.AddRange(new System.Data.DataTable[] {
            this.dtStockIn});
            // 
            // dtStockIn
            // 
            this.dtStockIn.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn8,
            this.dataColumn11,
            this.dataColumn12,
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4});
            this.dtStockIn.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.UniqueConstraint("Constraint1", new string[] {
                        "��Ʒ���к�"}, true)});
            this.dtStockIn.PrimaryKey = new System.Data.DataColumn[] {
        this.dataColumn1};
            this.dtStockIn.TableName = "dtStockIn";
            // 
            // dataColumn8
            // 
            this.dataColumn8.Caption = "��Ʒ��";
            this.dataColumn8.ColumnName = "��Ʒ��";
            // 
            // dataColumn11
            // 
            this.dataColumn11.ColumnName = "����";
            this.dataColumn11.DataType = typeof(short);
            // 
            // dataColumn12
            // 
            this.dataColumn12.AllowDBNull = false;
            this.dataColumn12.Caption = "����";
            this.dataColumn12.ColumnName = "����";
            // 
            // dataColumn1
            // 
            this.dataColumn1.AllowDBNull = false;
            this.dataColumn1.Caption = "��Ʒ���к�";
            this.dataColumn1.ColumnName = "��Ʒ���к�";
            // 
            // dataColumn2
            // 
            this.dataColumn2.Caption = "CollectType";
            this.dataColumn2.ColumnName = "CollectType";
            // 
            // dataColumn3
            // 
            this.dataColumn3.Caption = "MODEL";
            this.dataColumn3.ColumnName = "MODEL";
            // 
            // dataColumn4
            // 
            this.dataColumn4.Caption = "MO";
            this.dataColumn4.ColumnName = "MO";
            // 
            // FStockInput
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(552, 486);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpQuery);
            this.Controls.Add(this.panel2);
            this.Name = "FStockInput";
            this.Text = "���ɼ�";
            this.Load += new System.EventHandler(this.FStockInput_Load);
            this.Closed += new System.EventHandler(this.FStockInput_Closed);
            this.grpQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraOsType)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridContent)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dsStockIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStockIn)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void FStockInput_Load(object sender, System.EventArgs e)
		{
			this._facade = new MaterialStockFacade( this.DataProvider );
			UserControl.UIStyleBuilder.FormUI(this);
			UserControl.UIStyleBuilder.GridUI(this.ultraGridContent);

			InitializeTempDatable();

			DataView dv = _tmpTable.DefaultView;

			dv.Sort = "MODEL,MO";

			ultraGridContent.DataSource = dv;

			ultraGridContent.DisplayLayout.Bands[0].Columns["MODEL"].Hidden = true;
			ultraGridContent.DisplayLayout.Bands[0].Columns["MO"].Hidden = true;

			#region ������������
			//Ĭ��ѡ��Ϊ��ά����
			ultraOsType.Items.Clear();
			ultraOsType.CheckedItem =  ultraOsType.Items.Add(OQCFacade.OQC_ExameObject_PlanarCode,"��ά����");
			//this.ultraOptionSetOQCExameOpion.Items.Add(OQCFacade.OQC_ExameObject_Carton,"Carton");
			ultraOsType.Items.Add(OQCFacade.OQC_ExameObject_PCS,"PCS");
			#endregion

			#region Laws Lu,2005/08/27,���� ��ʼ����Ʒ��
			this.ucLCModel.Clear();
			this.ucLCModel.AddItem("", "");

			object[] objs = new ModelFacade( this.DataProvider ).GetAllModels();

			if ( objs == null )
			{
				return;
			}

			foreach (BenQGuru.eMES.Domain.MOModel.Model model in objs )
			{
				this.ucLCModel.AddItem( model.ModelCode, model.ModelCode );
			}

			this.ucLCModel.SelectedIndex = 0;
			#endregion

			this.ucLETicketNo.TextFocus(false, true);
		}

		private void ucLETicketNo_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == '\r' )
			{
				if ( this._facade.IsStockInTicketExist(this.ucLETicketNo.Value.Trim().ToUpper()) )
				{
					this.ShowMessage(new UserControl.Message(MessageType.Error,"$Error_CS_Stock_In_Ticket_Exist"));	// ��ⵥ���Ѵ���
					this.ucLETicketNo.TextFocus(false, true);
				}
				else
				{
					this.ucLEInput.TextFocus(false, true);
				}
			}
		}

		private void ucLEInput_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == '\r' )
			{	
				if ( this.ucLETicketNo.Value.Trim() == string.Empty )
				{
					this.ShowMessage(new UserControl.Message(MessageType.Error,"$Error_CS_Input_StockIn_TicketNo"));

					this.ucLETicketNo.TextFocus(false, true);

					return;
				}	

				if ( this.ucLEInput.Value.Trim() == string.Empty )
				{
					return;
				}

				try
				{		
					Messages msg = new Messages();
					//��ά����
					if ( this.ultraOsType.CheckedIndex == (int)CollectionType.Planate )
					{
						msg.AddMessages(CollectPlanate());
					}
					//PCS
					if ( this.ultraOsType.CheckedIndex == (int)CollectionType.PCS )
					{
						msg.AddMessages(CollectPCS());
					}

					ultraGridContent.DisplayLayout.Bands[0].Columns["MODEL"].Hidden = true;
					ultraGridContent.DisplayLayout.Bands[0].Columns["MO"].Hidden = true;

					this.ShowMessage(msg);
					ucLEInput.TextFocus(false, true);
				}
				catch( Exception ex )
				{
					this.ShowMessage( ex.Message );
					this.ucLEInput.TextFocus(false, true);

					return;
				}
				finally
				{
					this.ucLEInput.Value = "";
				}
			}
		}

		private void ucBtnDelete_Click(object sender, System.EventArgs e)
		{
			#region ɾ����ⵥ
			// ��ⵥ��δ��			
			if ( this.ucLETicketNo.Value.Trim() == string.Empty )
			{
				this.ShowMessage(new UserControl.Message(MessageType.Error,"$Error_CS_Input_StockIn_TicketNo"));
				this.ucLETicketNo.TextFocus(false, true);
				return;
			}	

			
			if ( !this._facade.IsStockInTicketIncludeDeletedExist(this.ucLETicketNo.Value.Trim().ToUpper()) )
			{
				this.ShowMessage(new UserControl.Message(MessageType.Error,"$Error_CS_Stock_In_Ticket_Not_Exist"));	// ��ⵥ�Ų�����
				this.ucLETicketNo.TextFocus(false, true);
				return;
			}

			FStockDelInfo confirm = new FStockDelInfo();
			// ȷ��ɾ����ⵥ
			if (confirm.ShowDialog(this) == DialogResult.OK )
			{
			
				this._domainDataProvider.BeginTransaction();
				try
				{
					object[] objStockIns = this._facade.QueryMaterialStockIn(this.ucLETicketNo.Value.Trim());

					bool result = false;

					//Laws Lu,2005/09/05,	�޸���ⵥ״̬Ϊ��ɾ��
					if(objStockIns != null && objStockIns.Length > 0)
					{
						for(int i = 0 ;i < objStockIns.Length ; i++)
						{
							MaterialStockIn mso = (MaterialStockIn)objStockIns[i];
							if(mso.Status != StockStatus.Deleted)
							{
								mso.DelUser = ApplicationService.Current().UserCode;
								mso.DelDate = BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(DateTime.Now);
								mso.DelTime = BenQGuru.eMES.Web.Helper.FormatHelper.TOTimeInt(DateTime.Now);
								mso.Status = BenQGuru.eMES.Material.StockStatus.Deleted;
								mso.DelMemo = this.DeleteInfor;

								this._facade.UpdateMaterialStockIn(mso);

								result = true;
							}
						}
					}

					this._domainDataProvider.CommitTransaction();

					//���ͳ������֣�ɾ����������ɾ����ⵥʱ����Ҫ������������ⵥ�Ƿ��Ѿ���ɾ��������Ѿ�����ɾ��״̬��ֱ����ʾ�û�����������������ⵥ�����Ѿ�ɾ��
					if(!result)
					{
						this.ShowMessage(new UserControl.Message(MessageType.Error,"$Error_CS_Stock_In_Ticket_Has_Been_Deleted"));	
						this.ucLETicketNo.TextFocus(false, true);
						return;
					}
				
					// ɾ����ⵥ
					//this._facade.UpdateMaterialStockIn( this.ucLETicketNo.Value.Trim() );
				}
				catch(Exception ex)
				{
					this._domainDataProvider.RollbackTransaction();
					this.ShowMessage( ex);
					this.ucLETicketNo.TextFocus(false, true);
					return;
				}
				// ɾ���ɹ�
				this.ShowMessage(new UserControl.Message(MessageType.Success,"$CS_Delete_Success"));		// ɾ���ɹ�
			}
			
			this.ucLETicketNo.Value = string.Empty;
			#endregion
		}

		private void ucBtnRemove_Click(object sender, System.EventArgs e)
		{
			try
			{	
				Messages msg = new Messages();
				//��ά����
				if ( this.ultraOsType.CheckedIndex == (int)CollectionType.Planate )
				{
					msg.AddMessages(RemovePlanate());
				}
				//PCS
				//Laws Lu,2005/09/07,�޸�	Check�ɼ����ͺ��б��еĲɼ�����			
				if ( this.ultraOsType.CheckedIndex == (int)CollectionType.PCS )
				{
					if( ultraGridContent.Selected.Rows.Count > 0)
					{
						Infragistics.Win.UltraWinGrid.UltraGridRow ultraDR = ultraGridContent.Selected.Rows[0];
				
						DataRow[] drs = dtStockIn.Select("��Ʒ���к�='" +ultraDR.Cells["��Ʒ���к�"].Text.ToUpper().Trim()+"'");
						if(drs.Length > 0)
						{
							if(drs[0]["CollectType"].ToString().ToUpper().Trim() != StockCollectionType.PCS)
							{
								msg.Add(new UserControl.Message(MessageType.Error,"$CS_COLLECTTYPE_NOT_MATCH"));
							}
						}
					
						if(msg.IsSuccess())
						{
							msg.AddMessages(RemovePCS());
						}
					}
					else
					{
						msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_Select_ID_To_Delete"));
					}
				}

				if(!msg.IsSuccess())
				{
					this.ShowMessage(msg);
				}
			}			
			catch( Exception ex )
			{
				this.ShowMessage( ex );
			}
		}

		private void ucBtnSave_Click(object sender, System.EventArgs e)
		{		
			#region ������ⵥ
			if ( this.ucLETicketNo.Value.Trim() == string.Empty )
			{
				this.ShowMessage(new UserControl.Message(MessageType.Error,"$Error_CS_Input_StockIn_TicketNo"));
				this.ucLETicketNo.TextFocus(false, true);
				return;
			}	

			//Laws Lu,2005/08/27
			if ( this.dtStockIn.Rows.Count == 0 )
			{
				this.ShowMessage(new UserControl.Message(MessageType.Error,"$CS_RCard_List_Is_Empty"));

				return;
			}

			if ( this._facade.IsStockInTicketExist(this.ucLETicketNo.Value.Trim().ToUpper()) )
			{
				this.ShowMessage(new UserControl.Message(MessageType.Error,"$Error_CS_Stock_In_Ticket_Exist"));	// ��ⵥ���Ѵ���
				this.ucLETicketNo.TextFocus(false, true);
				return;
			}

			this.DataProvider.BeginTransaction();

			//TODO:Laws Lu,2005/08/27
			try
			{				
				this._facade.AddMaterialStockIn( 
					this.ucLETicketNo.Value.Trim()
					,dtStockIn
					,ApplicationService.Current().UserCode
					,txtMEMO.Value);	
					
				this.DataProvider.CommitTransaction();
				//Laws Lu,2005/09/05,	�������
				//Laws Lu,2005/09/06,	���Memo
				txtSumNum.Value = "0";
				ucLCModel.SelectedIndex = -1;
				ucLEMOCode.Value = String.Empty;
				txtMEMO.Value = String.Empty;
			}
			catch( Exception exp )
			{
				this.DataProvider.RollbackTransaction();
				throw exp;
			}

			this.ShowMessage(new UserControl.Message(MessageType.Success,"$CS_Save_Success"));		// ����ɹ� ��ⵥ��

			//TODO:Laws Lu,2005/08/27

			sum.Clear();
			dtStockIn.Clear();
			_tmpTable.Clear();
			_tmpTable.AcceptChanges();
			dtStockIn.AcceptChanges();
//			this.lstRCardList.Items.Clear();
			this.ucLETicketNo.Value = string.Empty;
			this.ucLETicketNo.TextFocus(false, true);

			#endregion
		}

		private bool findItemFromList( string runningCard,out int rowNumber)
		{
			bool bReturn = false;
			rowNumber = -1;
			//TODO:Laws Lu,2005/08/27
			//�жϲ�Ʒ���к��Ƿ����
			if(dtStockIn.Rows.Count > 0)
			{
				for(int iRowNum = 0; iRowNum< dtStockIn.Rows.Count;iRowNum++)
				{
					if ( dtStockIn.Rows[iRowNum]["��Ʒ���к�"].ToString().Trim() == runningCard)
					{
						rowNumber = iRowNum;
						bReturn = true;
						break;
					}
				}
			}

			return bReturn;

		}

		private Messages checkRCardExist( string runningCard,out System.Data.DataRow foundRow,out bool found)
		{
			foundRow = null;
			Messages msg = new Messages();
			int iFoundRowNum ;
			found = findItemFromList(runningCard,out iFoundRowNum);

			if (found)
			{
				foundRow = dtStockIn.Rows[iFoundRowNum];
			}
			else if( iFoundRowNum != -1 && false == found)
			{
				msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_ID_Exist_In_StockIn_Ticket $RCard=" + runningCard));	// ���к��Ѵ���
			}

			return msg;
		
		}

		private void FStockInput_Closed(object sender, System.EventArgs e)
		{
			if (this.DataProvider!=null)
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection();  
			}

		
		}

		private Messages CollectPlanate()
		{
			#region ��ά����ɼ�
			Messages msg = new Messages();
			
			// ������ά����
			BarCodeParse barParser = new BarCodeParse(this._domainDataProvider);
			string[] idList = barParser.GetIDList( this.ucLEInput.Value.Trim() );
			string model = barParser.GetModelCode( this.ucLEInput.Value.Trim() );

			if(ucLCModel.ComboBoxData.SelectedIndex >= 0)
			{
				if (model.Trim().ToUpper() 
					!= ucLCModel.SelectedItemText.Trim().ToUpper())
				{
					msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_Planate_Model_Code_Not_Match"));

                    ucLCModel.Focus();

					return msg;
				}
			}
			else
			{
				msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_Planate_Model_Code_Not_Match"));

                ucLCModel.Focus();

				return msg;
			}
				
			if ( idList == null )
			{
				msg.Add(new UserControl.Message(MessageType.Error,"$CS_RCard_List_Is_Empty"));
				return msg;
			}

			DataRow dr = null;
			//Laws Lu,2005/08/27
			// ������к��Ƿ�����List�д���
			ArrayList ar = new ArrayList();
			foreach ( string id in idList )
			{
				bool found;
				msg.AddMessages(checkRCardExist( id,out dr,out found));
			
				if(found)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_ID_Exist_In_StockIn_Ticket $RCard=" + id));	// ���к��Ѵ���
					return msg;
				}

				if(dr == null && !found)
				{
					dr = dtStockIn.NewRow();
				}

				dr["��Ʒ���к�"] = id.ToUpper().Trim();
				dr["��Ʒ��"] = ucLCModel.SelectedItemText.Trim().ToUpper();
				//dr["����"] = ApplicationService.Current().UserCode;
				dr["����"] = ucLEMOCode.Value.Trim();
				dr["CollectType"] = StockCollectionType.Planate;

				dr["MODEL"] = ucLCModel.SelectedItemText.Trim().ToUpper();;
				dr["MO"] = ucLEMOCode.Value.Trim();;

				if(!dtStockIn.Rows.Contains(id.ToUpper().Trim()))
				{
					//Laws Lu,2005/09/05,�޸�	������Ʒ�� + ����	ͳ��
					InsertRow(id,dr);
				}
				else
				{
					ar.Add(id.ToUpper().Trim());
				}
			}

			foreach(object obj in ar)
			{
				msg.Add(new UserControl.Message("$CS_Param_ID " + obj.ToString().ToUpper() +" $CS_IDRepeatCollect"));
			}

			return msg;
			#endregion
		}


		private Messages CollectPCS()
		{
			#region ���кŲɼ�
			Messages msg = new Messages();

			string id = this.ucLEInput.Value.Trim().ToUpper();
			id = id.Substring(0, Math.Min(40,id.Length));

//			if(ucLCModel.ComboBoxData.SelectedIndex < 0)
//			{
//				//msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_Planate_Model_Code_Not_Match"));
//
//				ucLCModel.TextFocus(false, true);
//
//				return msg;
//			}

			// ������к��Ƿ�����List�д���
			DataRow dr = null;
			bool found;
			msg.AddMessages(checkRCardExist( id,out dr,out found));
			
			if(found)
			{
				msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_ID_Exist_In_StockIn_Ticket $RCard=" + id));	// ���к��Ѵ���
				return msg;
			}
			if(dr == null && !found)
			{
				dr = dtStockIn.NewRow();
			}

			dr["��Ʒ���к�"] = id.ToUpper();
			
			dr["��Ʒ��"] = ucLCModel.SelectedItemValue;

			//dr["����"] = ApplicationService.Current().UserCode;
			dr["����"] = ucLEMOCode.Value.Trim();

			dr["CollectType"] = StockCollectionType.PCS;

			dr["MODEL"] = ucLCModel.SelectedItemText.Trim().ToUpper();;
			dr["MO"] = ucLEMOCode.Value.Trim();;
			
			msg.AddMessages(InsertRow(id,dr));

			return msg;
			#endregion
		}

		private Messages InsertRow(string id,DataRow dr)
		{
			Messages msg = new Messages();
			//Laws Lu,2005/08/27
			if(!dtStockIn.Rows.Contains(id.ToUpper().Trim()))
			{
				#region ���ɼ�
				//Laws Lu,2005/09/05,�޸�	������Ʒ�� + ����	ͳ��
				DataRow[] drs = dtStockIn.Select("��Ʒ��='" + dr["��Ʒ��"].ToString().Trim() 
					+ "' and ����='" + dr["����"].ToString().Trim() + "'");
				if(drs == null || (drs != null && drs.Length < 1))
				{
					_tmpTable.Rows.Add(new object[]{
													   ucLCModel.SelectedItemText.Trim().ToUpper()
													   ,1
													   ,ucLEMOCode.Value.Trim()
													   ,id.ToUpper().Trim()
													   ,ucLCModel.SelectedItemText.Trim().ToUpper()
													   ,ucLEMOCode.Value.Trim()});

					sum.Add(dr["��Ʒ��"].ToString().Trim()+dr["����"].ToString().Trim(),1);

					dtStockIn.Rows.Add(dr);

					_tmpTable.AcceptChanges();
					dtStockIn.AcceptChanges();
				}
				else
				{
					DataRow[] drtmps = _tmpTable.Select("��Ʒ��='" + dr["��Ʒ��"].ToString().Trim() 
						+ "' and ����='" + dr["����"].ToString().Trim() + "'");

					if(drtmps == null || drtmps.Length < 1)
					{
						sum.Add(dr["��Ʒ��"].ToString().Trim()+dr["����"].ToString().Trim(),1);

						_tmpTable.Rows.Add(new object[]{
														   ucLCModel.SelectedItemText.Trim().ToUpper()
														   ,1
														   ,ucLEMOCode.Value.Trim()
														   ,id.ToUpper().Trim()
														   ,ucLCModel.SelectedItemText.Trim().ToUpper()
														   ,ucLEMOCode.Value.Trim()});

						dtStockIn.Rows.Add(dr);

						_tmpTable.AcceptChanges();
						dtStockIn.AcceptChanges();
					}
					else
					{
						foreach(DataRow drOld in drtmps)
						{
							if(drOld["����"] != null 
								&& drOld["MODEL"].ToString().ToUpper() == dr["��Ʒ��"].ToString().ToUpper() 
								&& drOld["MO"].ToString().ToUpper() == dr["����"].ToString().ToUpper())
							{
								if(sum.ContainsKey(dr["��Ʒ��"].ToString().Trim()+dr["����"].ToString().Trim()))
								{
									sum[dr["��Ʒ��"].ToString().Trim()+dr["����"].ToString().Trim()] = Convert.ToInt32(sum[dr["��Ʒ��"].ToString().Trim()+dr["����"].ToString().Trim()]) + 1;

									drOld["����"] = sum[dr["��Ʒ��"].ToString().Trim()+dr["����"].ToString().Trim()];

									_tmpTable.Rows.Add(new object[]{
																	   null
																	   ,null
																	   ,null
																	   ,id.ToUpper().Trim()
																	   ,ucLCModel.SelectedItemText.Trim().ToUpper()
																	   ,ucLEMOCode.Value.Trim()});

									dtStockIn.Rows.Add(dr);

									_tmpTable.AcceptChanges();
									dtStockIn.AcceptChanges();
								}
								else
								{
									sum.Add(dr["��Ʒ��"].ToString().Trim()+dr["����"].ToString().Trim(),1);

									_tmpTable.Rows.Add(new object[]{
																	   ucLCModel.SelectedItemText.Trim().ToUpper()
																	   ,1
																	   ,ucLEMOCode.Value.Trim()
																	   ,id.ToUpper().Trim()
																	   ,ucLCModel.SelectedItemText.Trim().ToUpper()
																	   ,ucLEMOCode.Value.Trim()});

									dtStockIn.Rows.Add(dr);

									_tmpTable.AcceptChanges();
									dtStockIn.AcceptChanges();
								}
							}
						}
					}
					
				}
				//End Laws Lu

				int iSum = txtSumNum.Value.Trim()==String.Empty?0:Convert.ToInt32(txtSumNum.Value.Trim());
				txtSumNum.Value = Convert.ToString(iSum + 1);
				#endregion	
			}
			else
			{
				msg.Add(new UserControl.Message("$CS_Param_ID " + id.ToUpper() +" $CS_IDRepeatCollect"));
			}

			return msg;
		}

		public string PlanateNum;

		private Messages RemovePlanate()
		{
			#region �Ƴ���ά����
			Messages msg = new Messages();
			if ( this.ultraOsType.CheckedIndex == (int)CollectionType.Planate)
			{	
				FStockRemove form = new FStockRemove(this);
				form.Text = this.Text;
						
				if ( form.ShowDialog() == DialogResult.OK )
				{
					BarCodeParse barParser = new BarCodeParse(this._domainDataProvider);

					string[] idList = barParser.GetIDList( PlanateNum );
					string Model = barParser.GetModelCode( PlanateNum );

					if(ucLCModel.ComboBoxData.SelectedIndex >= 0)
					{
						if (Model.Trim().ToUpper() 
							!= ucLCModel.SelectedItemText.Trim().ToUpper())
						{
							msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_Planate_Model_Code_Not_Match"));

                            ucLCModel.Focus();
						}
					}
					else
					{
						msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_Planate_Model_Code_Not_Match"));

                        ucLCModel.Focus();
					}
				
					if ( idList == null )
					{
						msg.Add(new UserControl.Message(MessageType.Error,"$CS_RCard_List_Is_Empty"));
						return msg;
					}

					// ������к��Ƿ�����List�д���
					DataRow dr = null;
					ArrayList ar = new ArrayList();

					foreach ( string id in idList )
					{
						bool found;
						msg.AddMessages(checkRCardExist( id,out dr,out found));
			
						if(dr != null)
						{
							//Laws Lu,2005/08/27
							//Laws Lu,2005/09/07,�޸�	�����Ƴ�ͬʱ��С��
							string model = String.Empty;
							string mo = String.Empty;
							int iSubSum = 0;
							bool bNeed = false;
							if(dtStockIn.Rows.Contains(id.ToUpper().Trim()))
							{

								foreach(DataRow tempDR in _tmpTable.Select("��Ʒ���к�='" 
									+ dr["��Ʒ���к�"].ToString().ToUpper().Trim()
									+ "'"))
								{

									if(tempDR["����"].ToString() != String.Empty)
									{
										bNeed = true;
								
										iSubSum = Convert.ToInt32(tempDR["����"].ToString().ToUpper().Trim()) - 1;
									}

									model = tempDR["MODEL"].ToString();
									mo = tempDR["MO"].ToString();

									_tmpTable.Rows.Remove(tempDR);

								}

								//Laws Lu,2005/09/07,����	���С��
//								sum[dr["��Ʒ��"].ToString().ToUpper().Trim()+dr["����"].ToString().ToUpper().Trim()]
//									= Convert.ToInt32(sum[dr["��Ʒ��"].ToString().ToUpper().Trim()+dr["����"].ToString().ToUpper().Trim()]) - 1;
//								if( Convert.ToInt32(sum[dr["��Ʒ��"].ToString().ToUpper().Trim()+dr["����"].ToString().ToUpper().Trim()]) == 0)
//								{
//									sum.Remove(dr["��Ʒ��"].ToString().ToUpper().Trim()+dr["����"].ToString().ToUpper().Trim());
//								}

								dtStockIn.Rows.Remove(dr);

								DataView dv = (DataView)ultraGridContent.DataSource;
								if(bNeed == true)
								{

									DataRow[] drs = _tmpTable.Select("MODEL='" 
										+ model + "' and MO='"
										+ mo +"'");
						
									if(drs.Length > 0)
									{
										drs[0]["��Ʒ��"] = model;
										drs[0]["����"] = mo;
										drs[0]["����"] = Convert.ToInt32(sum[model+mo]) - 1;

										sum[model+mo] = drs[0]["����"];

										if(Convert.ToInt32(sum[model+mo]) == 0)
										{
											sum.Remove(model+mo);
										}
									}
									else
									{
										sum.Remove(model+mo);
									}

									//						if(dv.Table.Rows.Count > 0)
									//						{
									//							dv.Table.Rows[iRowIndex + 1]["��Ʒ��"] = model;
									//							dv.Table.Rows[iRowIndex + 1]["����"] = mo;
									//							dv.Table.Rows[iRowIndex + 1]["����"] = iSubSum;
									//						}

								}
								else
								{
									DataRow[] drs = _tmpTable.Select("MODEL='" 
										+ model + "' and MO='"
										+ mo +"' and ����<>''");
						
									if(drs.Length > 0)
									{
										drs[0]["��Ʒ��"] = model;
										drs[0]["����"] = mo;
										drs[0]["����"] = Convert.ToInt32(drs[0]["����"]) - 1;

										sum[model+mo] = drs[0]["����"];

										if(Convert.ToInt32(sum[model+mo]) == 0)
										{
											sum.Remove(model+mo);
										}
									}
									else
									{
										sum.Remove(model+mo);
									}
						
								}

								int iSum = txtSumNum.Value.Trim()==String.Empty?0:Convert.ToInt32(txtSumNum.Value.Trim());

								if(iSum != 0)
								{
									txtSumNum.Value = Convert.ToString(iSum - 1);
								}
								
									
								_tmpTable.AcceptChanges();
								dtStockIn.AcceptChanges();
							}
//							else
//							{
//								ar.Add(id.ToUpper().Trim());
//							}
						}
					}

//					foreach(object obj in ar)
//					{
//						msg.Add(new UserControl.Message("$CS_Param_ID " + obj.ToString().ToUpper() +" $CS_IDRepeatCollect"));
//					}
				}
			}

			return msg;
			#endregion
		}

		private Messages RemovePCS()
		{
			#region �Ƴ����к�
			Messages msg = new Messages();
			if ( this.ultraOsType.CheckedIndex == (int)CollectionType.PCS )
			{
				//Laws Lu,2005/08/27
				if (dtStockIn.Rows.Count < 1 || ultraGridContent.Selected.Rows.Count < 1)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_Select_ID_To_Delete"));
				}
				else
				{
					//Laws Lu,2005/09/07,�޸�	�����Ƴ�ͬʱ��С��
					string model = String.Empty;
					string mo = String.Empty;
					int iSubSum = 0;
					bool bNeed = false;
//					foreach(Infragistics.Win.UltraWinGrid.UltraGridRow dr in ultraGridContent.Selected.Rows)
					for ( int iGridRowLoopIndex = 0; iGridRowLoopIndex < ultraGridContent.Selected.Rows.Count; iGridRowLoopIndex++)
					{
						Infragistics.Win.UltraWinGrid.UltraGridRow dr = ultraGridContent.Selected.Rows[iGridRowLoopIndex];
						foreach(DataRow drDT in dtStockIn.Select("��Ʒ���к�='" 
							+ dr.Cells["��Ʒ���к�"].Text.ToString().ToUpper().Trim()
							+ "'"))
						{

							if(dr.Cells["����"].Text.ToString() != String.Empty)
							{
								bNeed = true;
								
								iSubSum = Convert.ToInt32(dr.Cells["����"].Text.ToString().ToUpper().Trim()) - 1;
							}

							model = drDT["MODEL"].ToString();
							mo = drDT["MO"].ToString();

							dtStockIn.Rows.Remove(drDT);

						}

					}

					ultraGridContent.DeleteSelectedRows(false);

					_tmpTable.AcceptChanges();
					dtStockIn.AcceptChanges();
					
					DataView dv = (DataView)ultraGridContent.DataSource;
					if(bNeed == true)
					{

						DataRow[] drs = _tmpTable.Select("MODEL='" 
							+ model + "' and MO='"
							+ mo +"'");
						
						if(drs.Length > 0)
						{
							drs[0]["��Ʒ��"] = model;
							drs[0]["����"] = mo;
							drs[0]["����"] = Convert.ToInt32(sum[model+mo]) - 1;

							sum[model+mo] = drs[0]["����"];

							if(Convert.ToInt32(sum[model+mo]) == 0)
							{
								sum.Remove(model+mo);
							}
						}
						else
						{
							sum.Remove(model+mo);
						}

//						if(dv.Table.Rows.Count > 0)
//						{
//							dv.Table.Rows[iRowIndex + 1]["��Ʒ��"] = model;
//							dv.Table.Rows[iRowIndex + 1]["����"] = mo;
//							dv.Table.Rows[iRowIndex + 1]["����"] = iSubSum;
//						}

					}
					else
					{
						DataRow[] drs = _tmpTable.Select("MODEL='" 
							+ model + "' and MO='"
							+ mo +"' and ����<>''");
						
						if(drs.Length > 0)
						{
							drs[0]["��Ʒ��"] = model;
							drs[0]["����"] = mo;
							drs[0]["����"] = Convert.ToInt32(drs[0]["����"]) - 1;

							sum[model+mo] = drs[0]["����"];

							if(Convert.ToInt32(sum[model+mo]) == 0)
							{
								sum.Remove(model+mo);
							}
						}
						else
						{
							sum.Remove(model+mo);
						}
						
					}

					int iSum = txtSumNum.Value.Trim()==String.Empty?0:Convert.ToInt32(txtSumNum.Value.Trim());

					if(iSum != 0)
					{
						txtSumNum.Value = Convert.ToString(iSum - 1);
					}
					
				}

				
			}

			return msg;
			#endregion
		}

		#region ������ʾ���
		private void InitializeTempDatable()
		{
			_tmpTable.Columns.Clear();

			_tmpTable.Columns.Add( "��Ʒ��", typeof( string ));
			_tmpTable.Columns.Add( "����", typeof( string ) );
			_tmpTable.Columns.Add( "����", typeof( string ) );
			_tmpTable.Columns.Add( "��Ʒ���к�", typeof( string )).Unique = true;
			_tmpTable.Columns.Add( "MODEL", typeof( string ) );
			_tmpTable.Columns.Add( "MO", typeof( string ) );

		}

		private void ultraGridMain_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			ultraWinGridHelper = new UltraWinGridHelper(ultraGridContent);

			ultraWinGridHelper.AddCommonColumn("��Ʒ��","��Ʒ��");
			ultraWinGridHelper.AddCommonColumn("����","����");
			ultraWinGridHelper.AddCommonColumn("����","����");
			ultraWinGridHelper.AddCommonColumn("��Ʒ���к�","��Ʒ���к�");

		}

		#endregion
	}
}
