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
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Client.Service;
using UserControl;
using BenQGuru.eMES.OQC;

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FNormalShip ��ժҪ˵����
	/// </summary>
	public class FNormalShip : System.Windows.Forms.Form
	{
		#region ������������

		private const string NormalShip = "����Ʒ����";
		private const string UnNormalShip = "������Ʒ����";

		private DataTable dtFIFO = new DataTable();

		protected System.Windows.Forms.Panel panel1;
		protected UserControl.UCButton ucBtnExit;

		protected DataTable _tmpTable = new DataTable();
		protected System.ComponentModel.Container components = null;

		protected InventoryFacade _facade = null;

		protected IDomainDataProvider _domainDataProvider;
		protected UserControl.UCButton ucBtnComplete;
		protected System.Windows.Forms.GroupBox groupBox1;
		protected Infragistics.Win.UltraWinEditors.UltraOptionSet ultraOsType;
		protected UserControl.UCButton btnDeleteRCard;
		protected UserControl.UCButton btnDeleteDetail;
		private System.Windows.Forms.GroupBox groupBox2;
		protected UserControl.UCLabelEdit txtSumNum;
		//protected DataTable _tblRCard = new DataTable();
		private string _shipNo;
		private UserControl.UCButton btnAdd;
		private string _currseq;
		protected UserControl.UCButton btnUnComplete;
		protected UserControl.UCLabelEdit txtCartonNum;
		private System.Windows.Forms.TextBox ucLETicketNo;
		private System.Windows.Forms.Label lblShipNo;
		private System.Windows.Forms.TextBox txtPartner;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox ucLEInput;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton rbDelete;
		private System.Windows.Forms.RadioButton rbAdd;
		private System.Windows.Forms.Label lblCheck;
		private System.Windows.Forms.Label label3;
		protected UserControl.UCLabelCombox txtShipType;
		private System.Windows.Forms.GroupBox grpFIFO;
		protected Infragistics.Win.UltraWinGrid.UltraGrid ultraGridFIFO;
		protected System.Windows.Forms.GroupBox gbxDetail;
		protected Infragistics.Win.UltraWinGrid.UltraGrid ultraGridContent;
		protected System.Windows.Forms.Panel panel4;
		protected UserControl.UCButton btnCheck;
		protected UserControl.UCLabelEdit txtDay;
		private System.Windows.Forms.CheckBox cbFIFO;
		protected UserControl.UCButton btnOutputFIFO;

		FInfoForm _infoForm = ApplicationRun.GetInfoForm();
		#endregion

		#region ϵͳ���֣����캯����ϵͳ����
		public FNormalShip()
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
		
	
		public BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider DataProvider
		{
			get
			{
				return (BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider;
			}
		}

		protected void SucessMessage(string msg)
		{
			_infoForm.Add(new UserControl.Message(UserControl.MessageType.Success,msg));
		}

		protected void ErrorMessage(string msg)
		{			
			_infoForm.Add(new UserControl.Message(UserControl.MessageType.Error,msg));
			BenQGuru.eMES.Web.Helper.SoundPlayer.PlayErrorMusic();
		}

		private void FNormalShip_Closed(object sender, System.EventArgs e)
		{
			if (this.DataProvider!=null)
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection();  
			}
		}

		private void CloseConnection()
		{
			if (this._domainDataProvider!=null)
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this._domainDataProvider).PersistBroker.CloseConnection(); 
		}

		private void FNormalShip_Load(object sender, System.EventArgs e)
		{
			_infoForm.Add("");
			//_inputType.CurrentAction = InputType.ActionType.Partner;

			_domainDataProvider =ApplicationService.Current().DataProvider;

			this._facade = new InventoryFacade( this.DataProvider );
			UserControl.UIStyleBuilder.FormUI(this);
			ucLETicketNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			ucLEInput.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;

			#region ��ʼ��Grid
			UserControl.UIStyleBuilder.GridUI(this.ultraGridContent);

			_tmpTable.Columns.Clear();
			_tmpTable.Columns.Add("*",typeof(bool));
			_tmpTable.Columns.Add("NO",typeof(string));
			_tmpTable.Columns.Add("SEQ",typeof(int));
			_tmpTable.Columns.Add( "CustomerCode", typeof( string ));
			_tmpTable.Columns.Add( "CustomerNmae", typeof( string ));
			_tmpTable.Columns.Add( "ShipDate", typeof( string ));
			_tmpTable.Columns.Add( "ModelCode", typeof( string ));
			_tmpTable.Columns.Add( "ItemCode", typeof( string ));
			_tmpTable.Columns.Add( "ItemDesc", typeof( string ));
			_tmpTable.Columns.Add( "PlanQty", typeof( string ) );
			_tmpTable.Columns.Add( "ActQty", typeof( string ) );
			_tmpTable.Columns.Add( "status", typeof( string ) );
			BindGrid();

			InitialFIFOGrid();
//			UserControl.UIStyleBuilder.GridUI(this.gridRCard);
//			this._tblRCard.Columns.Clear();
//			this._tblRCard.Columns.Add("RCard",typeof(string));
//			this._tblRCard.Columns.Add("CollectType",typeof(string));
//			this._tblRCard.Columns.Add("SEQ",typeof(string));

			//System.Data.DataView dv = new System.Data.DataView(this._tblRCard,"SEQ=" + 0,string.Empty,System.Data.DataViewRowState.CurrentRows);
			//BindRCard(dv);

			#endregion

			#region ������������
			//Ĭ��ѡ��Ϊ��ά����
			ultraOsType.Items.Clear();
			ultraOsType.CheckedItem =  ultraOsType.Items.Add(CollectionType.Carton,CollectionType.Carton.ToString());
			//this.ultraOptionSetOQCExameOpion.Items.Add(OQCFacade.OQC_ExameObject_Carton,"Carton");
			ultraOsType.Items.Add(CollectionType.PCS,CollectionType.PCS.ToString());
			#endregion

			txtShipType.ComboBoxData.Items.Add(NormalShip);
			txtShipType.ComboBoxData.Items.Add(UnNormalShip);

			txtShipType.SelectedIndex = 0;

            this.ucLETicketNo.Focus();
			this.ucLETicketNo.BackColor = Color.GreenYellow;
		}

		private void InitialFIFOGrid()
		{
			dtFIFO.Columns.Clear();
		
			dtFIFO.Columns.Add( "CartonCode", typeof( string )).ReadOnly = true;
			dtFIFO.Columns.Add( "Rcard", typeof( string )).ReadOnly = true;
			dtFIFO.Columns.Add( "ModelCode", typeof( string )).ReadOnly = true;
			dtFIFO.Columns.Add( "ItemCode", typeof( string )).ReadOnly = true;
			dtFIFO.Columns.Add( "RecDate", typeof( string )).ReadOnly = true;
			dtFIFO.Columns.Add( "RecTime", typeof( string )).ReadOnly = true;
			
		}

		private void gridRCard_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
//			UltraWinGridHelper gridHelper = new UltraWinGridHelper(this.gridRCard);
//			gridHelper.AddCommonColumn("RCard","���к�");
//			gridHelper.AddCommonColumn("CollectType","CollectType");
//			gridHelper.AddCommonColumn("SEQ","���");
		}	

		private void ultraGridContent_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			UltraWinGridHelper gridHelper = new UltraWinGridHelper(this.ultraGridContent);
			gridHelper.AddCommonColumn("*","*");
			gridHelper.AddCommonColumn("NO","NO");
			gridHelper.AddCommonColumn("SEQ","���");
			gridHelper.AddCommonColumn("CustomerCode","�ͻ�����");
			gridHelper.AddCommonColumn("CustomerNmae","�ͻ�����");
			gridHelper.AddCommonColumn("ShipDate","��������");
			gridHelper.AddCommonColumn("ModelCode","��Ʒ��");
			gridHelper.AddCommonColumn("ItemCode","��Ʒ����");
			gridHelper.AddCommonColumn("ItemDesc","��Ʒ����");
			gridHelper.AddCommonColumn("PlanQty","�ƻ�����");
			gridHelper.AddCommonColumn("ActQty","ʵ������");
			gridHelper.AddCommonColumn("status","status");

			this.ultraGridContent.DisplayLayout.Bands[0].Columns["*"].Width = 20;
			this.ultraGridContent.DisplayLayout.Bands[0].Columns["SEQ"].Width = 40;
		}

		

		private void BindRCard(System.Data.DataView dv)
		{
//			this.gridRCard.DataSource = null;
//			this.gridRCard.DataSource = dv;
//			
//			this.gridRCard.DisplayLayout.Bands[0].Columns["CollectType"].Hidden = true;
//			this.gridRCard.DisplayLayout.Bands[0].Columns["SEQ"].Hidden = true;
		}

		private void BindGrid()
		{
			ultraGridContent.DataSource = null;
			DataView dv = _tmpTable.DefaultView;
			dv.Sort = "ModelCode,ItemCode";
			ultraGridContent.DataSource = dv;
			ultraGridContent.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
			ultraGridContent.DisplayLayout.Bands[0].Columns["NO"].Hidden = true;
			//ultraGridContent.DisplayLayout.Bands[0].Columns["SEQ"].Hidden = true;
			ultraGridContent.DisplayLayout.Bands[0].Columns["status"].Hidden = true;
			for(int i=1;i<this.ultraGridContent.DisplayLayout.Bands[0].Columns.Count;i++)
			{
				this.ultraGridContent.DisplayLayout.Bands[0].Columns[i].CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly;
			}
		}
		#endregion 

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FNormalShip));
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUnComplete = new UserControl.UCButton();
            this.ucBtnExit = new UserControl.UCButton();
            this.ucBtnComplete = new UserControl.UCButton();
            this.btnDeleteDetail = new UserControl.UCButton();
            this.btnAdd = new UserControl.UCButton();
            this.btnDeleteRCard = new UserControl.UCButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraOsType = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOutputFIFO = new UserControl.UCButton();
            this.btnCheck = new UserControl.UCButton();
            this.txtShipType = new UserControl.UCLabelCombox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCheck = new System.Windows.Forms.Label();
            this.cbFIFO = new System.Windows.Forms.CheckBox();
            this.rbDelete = new System.Windows.Forms.RadioButton();
            this.rbAdd = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.ucLEInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPartner = new System.Windows.Forms.TextBox();
            this.lblShipNo = new System.Windows.Forms.Label();
            this.ucLETicketNo = new System.Windows.Forms.TextBox();
            this.txtCartonNum = new UserControl.UCLabelEdit();
            this.txtSumNum = new UserControl.UCLabelEdit();
            this.txtDay = new UserControl.UCLabelEdit();
            this.grpFIFO = new System.Windows.Forms.GroupBox();
            this.ultraGridFIFO = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.gbxDetail = new System.Windows.Forms.GroupBox();
            this.ultraGridContent = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOsType)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.grpFIFO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridFIFO)).BeginInit();
            this.gbxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridContent)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnUnComplete);
            this.panel1.Controls.Add(this.ucBtnExit);
            this.panel1.Controls.Add(this.ucBtnComplete);
            this.panel1.Controls.Add(this.btnDeleteDetail);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnDeleteRCard);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 582);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(762, 38);
            this.panel1.TabIndex = 3;
            // 
            // btnUnComplete
            // 
            this.btnUnComplete.BackColor = System.Drawing.SystemColors.Control;
            this.btnUnComplete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUnComplete.BackgroundImage")));
            this.btnUnComplete.ButtonType = UserControl.ButtonTypes.None;
            this.btnUnComplete.Caption = "ȡ�����";
            this.btnUnComplete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUnComplete.Location = new System.Drawing.Point(417, 7);
            this.btnUnComplete.Name = "btnUnComplete";
            this.btnUnComplete.Size = new System.Drawing.Size(88, 22);
            this.btnUnComplete.TabIndex = 3;
            this.btnUnComplete.Click += new System.EventHandler(this.btnUnComplete_Click);
            // 
            // ucBtnExit
            // 
            this.ucBtnExit.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnExit.BackgroundImage")));
            this.ucBtnExit.ButtonType = UserControl.ButtonTypes.Exit;
            this.ucBtnExit.Caption = "�˳�";
            this.ucBtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExit.Location = new System.Drawing.Point(512, 7);
            this.ucBtnExit.Name = "ucBtnExit";
            this.ucBtnExit.Size = new System.Drawing.Size(88, 22);
            this.ucBtnExit.TabIndex = 2;
            // 
            // ucBtnComplete
            // 
            this.ucBtnComplete.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnComplete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnComplete.BackgroundImage")));
            this.ucBtnComplete.ButtonType = UserControl.ButtonTypes.None;
            this.ucBtnComplete.Caption = "��ɳ�����";
            this.ucBtnComplete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnComplete.Location = new System.Drawing.Point(323, 7);
            this.ucBtnComplete.Name = "ucBtnComplete";
            this.ucBtnComplete.Size = new System.Drawing.Size(88, 22);
            this.ucBtnComplete.TabIndex = 1;
            this.ucBtnComplete.Click += new System.EventHandler(this.ucBtnComplete_Click);
            // 
            // btnDeleteDetail
            // 
            this.btnDeleteDetail.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteDetail.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteDetail.BackgroundImage")));
            this.btnDeleteDetail.ButtonType = UserControl.ButtonTypes.None;
            this.btnDeleteDetail.Caption = "ɾ����ϸ";
            this.btnDeleteDetail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteDetail.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeleteDetail.Location = new System.Drawing.Point(229, 7);
            this.btnDeleteDetail.Name = "btnDeleteDetail";
            this.btnDeleteDetail.Size = new System.Drawing.Size(88, 22);
            this.btnDeleteDetail.TabIndex = 6;
            this.btnDeleteDetail.Click += new System.EventHandler(this.btnDeleteDetail_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.ButtonType = UserControl.ButtonTypes.Add;
            this.btnAdd.Caption = "����";
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Location = new System.Drawing.Point(15, 7);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 22);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDeleteRCard
            // 
            this.btnDeleteRCard.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteRCard.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteRCard.BackgroundImage")));
            this.btnDeleteRCard.ButtonType = UserControl.ButtonTypes.None;
            this.btnDeleteRCard.Caption = "ɾ��";
            this.btnDeleteRCard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteRCard.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeleteRCard.Location = new System.Drawing.Point(109, 7);
            this.btnDeleteRCard.Name = "btnDeleteRCard";
            this.btnDeleteRCard.Size = new System.Drawing.Size(88, 22);
            this.btnDeleteRCard.TabIndex = 5;
            this.btnDeleteRCard.Visible = false;
            this.btnDeleteRCard.Click += new System.EventHandler(this.btnDeleteRCard_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ultraOsType);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(762, 37);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "�ɼ���ʽ";
            // 
            // ultraOsType
            // 
            this.ultraOsType.BackColor = System.Drawing.SystemColors.Control;
            this.ultraOsType.BackColorInternal = System.Drawing.SystemColors.Control;
            this.ultraOsType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraOsType.ImageTransparentColor = System.Drawing.Color.Gainsboro;
            valueListItem1.DisplayText = "Carton  ";
            valueListItem2.DisplayText = "PCS";
            this.ultraOsType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.ultraOsType.Location = new System.Drawing.Point(3, 16);
            this.ultraOsType.Name = "ultraOsType";
            this.ultraOsType.Size = new System.Drawing.Size(756, 18);
            this.ultraOsType.TabIndex = 0;
            this.ultraOsType.TabStop = false;
            this.ultraOsType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOutputFIFO);
            this.groupBox2.Controls.Add(this.btnCheck);
            this.groupBox2.Controls.Add(this.txtShipType);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblCheck);
            this.groupBox2.Controls.Add(this.cbFIFO);
            this.groupBox2.Controls.Add(this.rbDelete);
            this.groupBox2.Controls.Add(this.rbAdd);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.ucLEInput);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtPartner);
            this.groupBox2.Controls.Add(this.lblShipNo);
            this.groupBox2.Controls.Add(this.ucLETicketNo);
            this.groupBox2.Controls.Add(this.txtCartonNum);
            this.groupBox2.Controls.Add(this.txtSumNum);
            this.groupBox2.Controls.Add(this.txtDay);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 37);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(762, 104);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // btnOutputFIFO
            // 
            this.btnOutputFIFO.BackColor = System.Drawing.SystemColors.Control;
            this.btnOutputFIFO.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOutputFIFO.BackgroundImage")));
            this.btnOutputFIFO.ButtonType = UserControl.ButtonTypes.None;
            this.btnOutputFIFO.Caption = "�Ƚ��ȳ�����";
            this.btnOutputFIFO.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOutputFIFO.Location = new System.Drawing.Point(496, 72);
            this.btnOutputFIFO.Name = "btnOutputFIFO";
            this.btnOutputFIFO.Size = new System.Drawing.Size(88, 22);
            this.btnOutputFIFO.TabIndex = 33;
            this.btnOutputFIFO.Click += new System.EventHandler(this.btnOutputFIFO_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.BackColor = System.Drawing.SystemColors.Control;
            this.btnCheck.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCheck.BackgroundImage")));
            this.btnCheck.ButtonType = UserControl.ButtonTypes.None;
            this.btnCheck.Caption = "���";
            this.btnCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheck.Location = new System.Drawing.Point(230, 72);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(88, 22);
            this.btnCheck.TabIndex = 32;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // txtShipType
            // 
            this.txtShipType.AllowEditOnlyChecked = true;
            this.txtShipType.Caption = "�� �� Ʒ";
            this.txtShipType.Checked = false;
            this.txtShipType.Location = new System.Drawing.Point(230, 43);
            this.txtShipType.Name = "txtShipType";
            this.txtShipType.SelectedIndex = -1;
            this.txtShipType.ShowCheckBox = false;
            this.txtShipType.Size = new System.Drawing.Size(162, 19);
            this.txtShipType.TabIndex = 31;
            this.txtShipType.WidthType = UserControl.WidthTypes.Normal;
            this.txtShipType.XAlign = 281;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(509, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 21);
            this.label3.TabIndex = 22;
            this.label3.Text = "��ǰ��������";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCheck
            // 
            this.lblCheck.Location = new System.Drawing.Point(425, 44);
            this.lblCheck.Name = "lblCheck";
            this.lblCheck.Size = new System.Drawing.Size(36, 21);
            this.lblCheck.TabIndex = 20;
            this.lblCheck.Text = "���";
            this.lblCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbFIFO
            // 
            this.cbFIFO.Checked = true;
            this.cbFIFO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFIFO.Location = new System.Drawing.Point(466, 14);
            this.cbFIFO.Name = "cbFIFO";
            this.cbFIFO.Size = new System.Drawing.Size(76, 22);
            this.cbFIFO.TabIndex = 19;
            this.cbFIFO.Text = "�Ƚ��ȳ�";
            this.cbFIFO.CheckedChanged += new System.EventHandler(this.cbNormalRCard_CheckedChanged);
            // 
            // rbDelete
            // 
            this.rbDelete.Location = new System.Drawing.Point(377, 74);
            this.rbDelete.Name = "rbDelete";
            this.rbDelete.Size = new System.Drawing.Size(55, 23);
            this.rbDelete.TabIndex = 18;
            this.rbDelete.Text = "ɾ��";
            // 
            // rbAdd
            // 
            this.rbAdd.Checked = true;
            this.rbAdd.Location = new System.Drawing.Point(328, 74);
            this.rbAdd.Name = "rbAdd";
            this.rbAdd.Size = new System.Drawing.Size(49, 23);
            this.rbAdd.TabIndex = 17;
            this.rbAdd.TabStop = true;
            this.rbAdd.Text = "����";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 22);
            this.label2.TabIndex = 16;
            this.label2.Text = "�ɼ�����";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucLEInput
            // 
            this.ucLEInput.Location = new System.Drawing.Point(68, 74);
            this.ucLEInput.Name = "ucLEInput";
            this.ucLEInput.Size = new System.Drawing.Size(147, 20);
            this.ucLEInput.TabIndex = 2;
            this.ucLEInput.Leave += new System.EventHandler(this.ucLEInput_Leave);
            this.ucLEInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucLEInput_TxtboxKeyPress);
            this.ucLEInput.Enter += new System.EventHandler(this.ucLEInput_Enter);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 21);
            this.label1.TabIndex = 14;
            this.label1.Text = "�ͻ�����";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPartner
            // 
            this.txtPartner.Location = new System.Drawing.Point(68, 43);
            this.txtPartner.Name = "txtPartner";
            this.txtPartner.Size = new System.Drawing.Size(147, 20);
            this.txtPartner.TabIndex = 1;
            this.txtPartner.Leave += new System.EventHandler(this.txtPartner_Leave);
            this.txtPartner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPartner_TxtboxKeyPress);
            this.txtPartner.Enter += new System.EventHandler(this.txtPartner_Enter);
            // 
            // lblShipNo
            // 
            this.lblShipNo.Location = new System.Drawing.Point(7, 15);
            this.lblShipNo.Name = "lblShipNo";
            this.lblShipNo.Size = new System.Drawing.Size(57, 21);
            this.lblShipNo.TabIndex = 12;
            this.lblShipNo.Text = "��������";
            this.lblShipNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucLETicketNo
            // 
            this.ucLETicketNo.BackColor = System.Drawing.Color.GreenYellow;
            this.ucLETicketNo.Location = new System.Drawing.Point(68, 15);
            this.ucLETicketNo.Name = "ucLETicketNo";
            this.ucLETicketNo.Size = new System.Drawing.Size(147, 20);
            this.ucLETicketNo.TabIndex = 0;
            this.ucLETicketNo.Leave += new System.EventHandler(this.ucLETicketNo_Leave);
            this.ucLETicketNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucLETicketNo_TxtboxKeyPress);
            this.ucLETicketNo.Enter += new System.EventHandler(this.ucLETicketNo_Enter);
            // 
            // txtCartonNum
            // 
            this.txtCartonNum.AllowEditOnlyChecked = true;
            this.txtCartonNum.Caption = "Carton��";
            this.txtCartonNum.Checked = false;
            this.txtCartonNum.EditType = UserControl.EditTypes.String;
            this.txtCartonNum.Location = new System.Drawing.Point(349, 13);
            this.txtCartonNum.MaxLength = 40;
            this.txtCartonNum.Multiline = false;
            this.txtCartonNum.Name = "txtCartonNum";
            this.txtCartonNum.PasswordChar = '\0';
            this.txtCartonNum.ReadOnly = true;
            this.txtCartonNum.ShowCheckBox = false;
            this.txtCartonNum.Size = new System.Drawing.Size(93, 22);
            this.txtCartonNum.TabIndex = 10;
            this.txtCartonNum.TabNext = false;
            this.txtCartonNum.TabStop = false;
            this.txtCartonNum.Value = "0";
            this.txtCartonNum.WidthType = UserControl.WidthTypes.Tiny;
            this.txtCartonNum.XAlign = 400;
            // 
            // txtSumNum
            // 
            this.txtSumNum.AllowEditOnlyChecked = true;
            this.txtSumNum.Caption = "�ϼ�����";
            this.txtSumNum.Checked = false;
            this.txtSumNum.EditType = UserControl.EditTypes.String;
            this.txtSumNum.Location = new System.Drawing.Point(230, 15);
            this.txtSumNum.MaxLength = 40;
            this.txtSumNum.Multiline = false;
            this.txtSumNum.Name = "txtSumNum";
            this.txtSumNum.PasswordChar = '\0';
            this.txtSumNum.ReadOnly = true;
            this.txtSumNum.ShowCheckBox = false;
            this.txtSumNum.Size = new System.Drawing.Size(93, 22);
            this.txtSumNum.TabIndex = 3;
            this.txtSumNum.TabNext = false;
            this.txtSumNum.TabStop = false;
            this.txtSumNum.Value = "0";
            this.txtSumNum.WidthType = UserControl.WidthTypes.Tiny;
            this.txtSumNum.XAlign = 281;
            // 
            // txtDay
            // 
            this.txtDay.AllowEditOnlyChecked = true;
            this.txtDay.Caption = "";
            this.txtDay.Checked = false;
            this.txtDay.EditType = UserControl.EditTypes.Integer;
            this.txtDay.Location = new System.Drawing.Point(455, 45);
            this.txtDay.MaxLength = 40;
            this.txtDay.Multiline = false;
            this.txtDay.Name = "txtDay";
            this.txtDay.PasswordChar = '\0';
            this.txtDay.ReadOnly = false;
            this.txtDay.ShowCheckBox = false;
            this.txtDay.Size = new System.Drawing.Size(49, 22);
            this.txtDay.TabIndex = 10;
            this.txtDay.TabNext = false;
            this.txtDay.TabStop = false;
            this.txtDay.Value = "1";
            this.txtDay.WidthType = UserControl.WidthTypes.Tiny;
            this.txtDay.XAlign = 462;
            // 
            // grpFIFO
            // 
            this.grpFIFO.Controls.Add(this.ultraGridFIFO);
            this.grpFIFO.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpFIFO.Location = new System.Drawing.Point(13, 7);
            this.grpFIFO.Name = "grpFIFO";
            this.grpFIFO.Size = new System.Drawing.Size(554, 75);
            this.grpFIFO.TabIndex = 8;
            this.grpFIFO.TabStop = false;
            this.grpFIFO.Text = "�Ƚ��ȳ����";
            this.grpFIFO.Visible = false;
            this.grpFIFO.Resize += new System.EventHandler(this.grpFIFO_Resize);
            // 
            // ultraGridFIFO
            // 
            this.ultraGridFIFO.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraGridFIFO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridFIFO.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ultraGridFIFO.Location = new System.Drawing.Point(3, 17);
            this.ultraGridFIFO.Name = "ultraGridFIFO";
            this.ultraGridFIFO.Size = new System.Drawing.Size(548, 55);
            this.ultraGridFIFO.TabIndex = 7;
            this.ultraGridFIFO.TabStop = false;
            this.ultraGridFIFO.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridFIFO_InitializeLayout);
            // 
            // gbxDetail
            // 
            this.gbxDetail.Controls.Add(this.ultraGridContent);
            this.gbxDetail.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbxDetail.Location = new System.Drawing.Point(0, 82);
            this.gbxDetail.Name = "gbxDetail";
            this.gbxDetail.Size = new System.Drawing.Size(587, 223);
            this.gbxDetail.TabIndex = 7;
            this.gbxDetail.TabStop = false;
            this.gbxDetail.Text = "������ϸ";
            // 
            // ultraGridContent
            // 
            this.ultraGridContent.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraGridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridContent.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ultraGridContent.Location = new System.Drawing.Point(3, 17);
            this.ultraGridContent.Name = "ultraGridContent";
            this.ultraGridContent.Size = new System.Drawing.Size(581, 203);
            this.ultraGridContent.TabIndex = 6;
            this.ultraGridContent.TabStop = false;
            this.ultraGridContent.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridContent_InitializeLayout);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.grpFIFO);
            this.panel4.Controls.Add(this.gbxDetail);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel4.Location = new System.Drawing.Point(0, 141);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(762, 441);
            this.panel4.TabIndex = 8;
            // 
            // FNormalShip
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(762, 620);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "FNormalShip";
            this.Text = "���۳����ɼ�";
            this.Load += new System.EventHandler(this.FNormalShip_Load);
            this.SizeChanged += new System.EventHandler(this.FNormalShip_SizeChanged);
            this.Closed += new System.EventHandler(this.FNormalShip_Closed);
            this.Activated += new System.EventHandler(this.FNormalShip_Activated);
            this.Resize += new System.EventHandler(this.FNormalShip_Resize);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraOsType)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpFIFO.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridFIFO)).EndInit();
            this.gbxDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridContent)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		#region ������Ӳ���
		/// <summary>
		/// �������гʻ����Ƕ�ά�������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddData()
		{
			try
			{
				//������ŷ����仯������load����
				if(this._shipNo != this.ucLETicketNo.Text.Trim())
				{
					this.LoadData();
				}
				//����������
				if(!InputValid())
					return ;
				
				//��¼�뾭���̴���ʱ��
				//if(_inputType.CurrentAction == InputType.ActionType.Partner)
				//{
				//��龭���̴���
				//					if(_facade.QueryInvShipPartnerCount(this.ucLETicketNo.InnerTextBox.Text.Trim(),
				//						this.ucLEInput.InnerTextBox.Text.Trim()) <= 0)
				//					{
				//						this.ErrorMessage("��������û�а������������");
				//						this.ucLEInput.TextFocus(false, true);
				//						return;
				//					}
				//
				//					//_inputType.PartnerCode = this.ucLEInput.InnerTextBox.Text.Trim();
				//					//this.ucLEInput.Caption = _inputType.GetInputCaption(this.ultraOsType.CheckedIndex);
				//
				//					//binding OrderNO.
				//					this.ucLCOrderNO.Clear();
				//					this.ucLCOrderNO.AddItem( "", "" );
				//					OrderFacade orderFacade = new OrderFacade( _domainDataProvider );
				//					string[] orders = orderFacade.QueryOrderByPartner(this.ucLEInput.InnerTextBox.Text.Trim());
				//					if( orders!=null && orders.Length>0 )
				//					{
				//						for( int i=0; i<orders.Length; i++)
				//						{
				//							this.ucLCOrderNO.AddItem( orders[i], orders[i] );
				//						}
				//					}
					
				//return;
				//}

				bool isAdd = false;
				if ( this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.Planate.ToString() )
				{
					#region ��ά�������
					string[] idList = null;
					try
					{
						BarCodeParse barParser = new BarCodeParse(this._domainDataProvider);
						idList = barParser.GetIDList( this.ucLEInput.Text.Trim() );
						//string model = barParser.GetModelCode( this.ucLEInput.Value.Trim() );
					}
					catch(System.Exception ex)
					{
						this.ErrorMessage(ex.Message);
						return;
					}
			
					if ( idList == null || idList.Length == 0)
					{
						this.ErrorMessage("$CS_RCard_List_Is_Empty");
						return;
					}

					this.DataProvider.BeginTransaction();

					//�����������ӣ����ж��Ƿ��Ѿ�����
					foreach ( string id in idList )
					{
						AddShipRCard(id,CollectionType.Planate,string.Empty);
						isAdd = true;
					}


					#endregion
				}
				else if ( this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.Carton.ToString() )
				{
					#region Carton
					BenQGuru.eMES.DataCollect.DataCollectFacade dc = new BenQGuru.eMES.DataCollect.DataCollectFacade(this.DataProvider);

					object[] idList = dc.GetSimulationFromCarton(this.ucLEInput.Text.Trim().ToUpper());
					if ( idList == null || idList.Length == 0)
					{
						this.ErrorMessage("$CS_RCard_List_Is_Empty");
						return;
					}

					this.DataProvider.BeginTransaction();

					//�����������ӣ����ж��Ƿ��Ѿ�����
					foreach (BenQGuru.eMES.Domain.DataCollect.Simulation sim in idList )
					{
						AddShipRCard(sim.RunningCard,CollectionType.Carton,this.ucLEInput.Text.Trim().ToUpper());
						isAdd = true;
					}
					#endregion
				}
				else if( this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.PCS.ToString() )
				{
					this.DataProvider.BeginTransaction();

					#region ���к����
					string id = this.ucLEInput.Text.Trim().ToUpper();
					id = id.Substring(0, Math.Min(40,id.Length));
						
					BenQGuru.eMES.DataCollect.DataCollectFacade dc = new BenQGuru.eMES.DataCollect.DataCollectFacade(this.DataProvider);
					BenQGuru.eMES.Domain.DataCollect.Simulation sim = dc.GetSimulation(id) as BenQGuru.eMES.Domain.DataCollect.Simulation;

					string cartoncode = string.Empty;
					if(sim != null)
						cartoncode = sim.CartonCode;

					AddShipRCard(id,CollectionType.PCS,cartoncode);
					isAdd = true;
					#endregion
				}
				if(isAdd)
				{
					if(this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.Carton.ToString())
					{
						this.txtCartonNum.Value = (int.Parse(this.txtCartonNum.Value)+1).ToString();
					}
					this.SucessMessage("$CS_Add_Success");	
				}
				this.DataProvider.CommitTransaction();

				//				if(cbFIFO.Checked)
				//				{
				btnCheck_Click(null,null);
				//				}
				//this.ucLEInput.Caption = _inputType.GetInputCaption(this.ultraOsType.CheckedIndex);
                ucLEInput.Focus();
			}
			catch( Exception ex )
			{
				this.DataProvider.RollbackTransaction();
				this.ErrorMessage( ex.Message );
				this.LoadData();
                this.ucLEInput.Focus();
			}
			finally
			{
				this.ucLEInput.Text = "";
				this.CloseConnection();
			}
		}
		private void ucLEInput_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == '\r' )
			{
				if(this.rbDelete.Checked)
				{
					btnDeleteRCard_Click(null,null);

				}
				else if(rbAdd.Checked)
				{
					AddData();	
				}

				Application.DoEvents();
                ucLEInput.Focus();
				ucLEInput.SelectAll();

			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			AddData();
		}

		//�������������Ч��
		private bool InputValid()
		{
			if ( this.ucLETicketNo.Text.Trim() == string.Empty )
			{
				this.ErrorMessage("$Error_CS_Input_StockIn_TicketNo");
                this.ucLETicketNo.Focus();
				return false;
			}	
			if(txtPartner.Text.Trim() == string.Empty)
			{
				this.ErrorMessage("$Error_CS_Input_Partner_Code");
                this.ucLETicketNo.Focus();
				return false;
			}
			if ( this.ucLEInput.Text.Trim() == string.Empty )
			{
				return false;
			}

			return true;
		}

		private void AddShipRCard(string rcard,CollectionType c_type,string cartoncode)
		{
			//��ѯ���к��Ƿ���ڣ���״̬�Ƿ���ȷ
			object[] objsRCard = _facade.QueryInvRCard(rcard);

			if(objsRCard == null || objsRCard.Length <= 0)
				throw new Exception(rcard + " $Rcard_No_Received");//���кŻ�û�����

			bool isStatusRight = false;
			InvRCard inv = null;
			foreach(object obj in objsRCard)
			{
				InvRCard testinv = obj as InvRCard;

				if(testinv.ShipNO == this._shipNo)
					throw new Exception(rcard + " $Rcard_Exist_In_this_Ship");//���к��Ѿ������ڴ˳�������

				if(testinv != null && testinv.RCardStatus == RCardStatus.Received)
				{
					isStatusRight = true;
					inv = testinv; //�ҵ�һ����Ч������¼
				}
			}

			if(!isStatusRight)
			{
				throw new Exception(rcard + " $Rcard_Shipped"); //���к��Ѿ�����
			}
			
			#region old code			
			//			//���³���������
			//			object[] objs = _facade.QueryInvShip(this.ucLETicketNo.InnerTextBox.Text.Trim(),
			//												inv.ItemCode,_inputType.PartnerCode);
			//			InvShip ship = null;
			//			if(objs != null && objs.Length > 0)
			//			{
			//				ship = objs[0] as InvShip;
			//				if(ship.ShipStatus != ShipStatus.Shipping)
			//					throw new NoAllowAddException();
			//
			//				ship.ActQty += 1;
			//				if(ship.ActQty > ship.PlanQty)
			//					throw new GreaterException();
			//
			//				this._facade.UpdateInvShip(ship);
			//			}
			//			else //������������
			//			{
			//				throw new Exception(rcard + "���кŵĲ�Ʒ����ͳ������в���");
			//			}

			#endregion		

			#region Darfon ����Ҫ���� lucky wei
			//			//���¶�����ϸ
			//			OrderFacade orderFacade = new OrderFacade( _domainDataProvider );
			//			Object orderdetail = orderFacade.GetOrderDetail(this.txtPartner.Value, inv.ItemCode, this.ucLCOrderNO.SelectedItemText );
			//			if( orderdetail!=null )
			//			{
			//				string orderSql = string.Empty;
			//				( orderdetail as OrderDetail ).ActQTY +=1;
			//				if( ( orderdetail as OrderDetail ).ActQTY >= ( orderdetail as OrderDetail ).PlanQTY )
			//				{
			//					//throw new Exception(string.Format("����{0}�����������ƻ���",this.ucLCOrderNO.SelectedItemText));
			//					orderSql = 
			//						string.Format(" update tblorderdetail set actqty=actqty + 1, actdate={3} where ordernumber='{0}' and itemcode='{1}' and partnercode='{2}' ",
			//						this.ucLCOrderNO.SelectedItemText, inv.ItemCode, this.txtPartner.Value,FormatHelper.TODateInt( DateTime.Now ));
			//					this.DataProvider.CustomExecute(new SQLCondition(orderSql));
			//				}
			//
			//				else
			//				{
			//					orderSql = 
			//					 string.Format(" update tblorderdetail set actqty=actqty + 1 where ordernumber='{0}' and itemcode='{1}' and partnercode='{2}' ",
			//					 this.ucLCOrderNO.SelectedItemText, inv.ItemCode, this.txtPartner.Value);
			//					this.DataProvider.CustomExecute(new SQLCondition(orderSql));
			//				}
			//
			//				if( orderFacade.CheckOrderIsCompleted(this.ucLCOrderNO.SelectedItemText) )
			//				{
			//					Object order = orderFacade.GetOrder( this.ucLCOrderNO.SelectedItemText );
			// 					(order as Order).ActShipDate = FormatHelper.TODateInt( DateTime.Now );
			//					(order as Order).ActShipMonth = DateTime.Now.Month;
			//					(order as Order).ActShipWeek = FormatHelper.GetRecentWeekOfYear();
			//					(order as Order).OrderStatus = OrderStatus.Completed ;
			//                    orderFacade.UpdateOrder( order as Order );
			//				}
			//			}
			//			else
			//			{
			//				throw new Exception(rcard + "���кŵĲ�Ʒ����Ͷ����в���");
			//			}

			#endregion

			//���³���������
			InvShip ship = GetShip(this._shipNo,
				inv.ItemCode,this.txtPartner.Text.Trim());

			if(ship != null)
			{
				if(ship.ShipStatus != ShipStatus.Shipping)
					throw new NoAllowAddException();

				ship.ActQty += 1;
				if(ship.ActQty > ship.PlanQty)
					throw new GreaterException();

				this._facade.IncShipQty(ship.ShipNo,ship.ShipSeq.ToString());
			}
			else //������������
			{
				throw new Exception(rcard + " $Receive_NoExist_Or_Partner_NoExist");//�����������ڻ��߳������в���������ͻ�
			}

			bool isNormal = false;

			if(txtShipType.ComboBoxData.Text == NormalShip)
			{
				isNormal = true;
			}

			if(isNormal)
			{
				if(inv.INVRardType != INVRardType.Normal)
					throw new Exception("$ReceiveRcard_Must_Be_Normal"); //��Ʒ���кŵ�������ͱ������������Ʒ
			}
			else
			{
				if(inv.INVRardType != INVRardType.Unnormal)
					throw new Exception("$ReceiveRcard_Must_Be_UnNormal");//��Ʒ���кŵ�������ͱ������쳣���Ʒ
			}

			// Added By Karron Qiu,2006-8-9
			//Laws lu,2006/08/28 �޸�
			if(cbFIFO.Checked == true)
			{
				ShipChecker checker = new ShipChecker(this.DataProvider);
				if( this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.Carton.ToString() )
				{
					checker.Check(inv,this.txtDay.Value.Trim(),isNormal,false);
				}
				else if( this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.PCS.ToString() )
				{
					checker.Check(inv,this.txtDay.Value.Trim(),isNormal,true);
				}
			}
			//End

			//�������������㣬�������к�
			//Laws Lu,2006/11/13 uniform system collect date
			DBDateTime dbDateTime;
			
			dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
			

			inv.ShipDate = dbDateTime.DBDate;
			inv.ShipTime = dbDateTime.DBTime;
			inv.ShipUser = ApplicationService.Current().UserCode;
			inv.ShipNO =  ship.ShipNo;
			inv.ShipSeq = ship.ShipSeq;
			inv.RCardStatus = RCardStatus.Shipped;
			inv.ShipCollectType = c_type.ToString();
			inv.CartonCode = cartoncode;
			_facade.UpdateInvRCard(inv);

			//			string sql2 = string.Format("update tblinvrcard set shipdate={0},shiptime={1},shipuser='{2}',shipno='{3}',shipseq={4},Rcardstatus='{5}',ShipCollectType='{6}' where recno='{7}' and rcard='{8}'",
			//									inv.ShipDate,
			//									inv.ShipTime,
			//									inv.ShipUser,
			//									inv.ShipNO,
			//				                    inv.ShipSeq,
			//									inv.RCardStatus,
			//									inv.ShipCollectType,
			//									inv.RecNO,
			//									inv.RunningCard
			//								  );
			//
			//			int c = this.DataProvider.CustomExecuteWithReturn(new SQLCondition(sql2));
			//			if(c < 1) 
			//				throw new Exception(rcard+" $Ship_Rcard_No_Exist");//������кŲ����ڣ������Ѿ���ɾ��
			//			else if(c>1)
			//				throw new Exception("$Ship_DB_Update_Error"); //���ݿ����ʧ��

			this._currseq = ship.ShipSeq.ToString();

			//���½�����ʾ����
			UpdateFormData(ship,inv,FormAction.Add);

			for(int i = 0 ; i < dtFIFO.Rows.Count ;i ++)
			{
				if(dtFIFO.Rows[i]["Rcard"].ToString().Trim() == inv.RunningCard)
				{
					dtFIFO.Rows.Remove(dtFIFO.Rows[i]);
					dtFIFO.AcceptChanges();

					//					if(dtFIFO.Rows.Count == 0)
					//					{
					//						grpFIFO.Visible = false;
					//
					//						SetPosition();
					//					}
					//existFIFO = true;
					break;
				}

			}
			if(dtFIFO.Rows.Count == 0)
			{
				grpFIFO.Visible = true;
				SetPosition();
			}

			//this.AddRCardGridRow(inv);
		}
		
		private InvShip GetShip(string no,string item,string partner)
		{
			DataRow[] drList = this._tmpTable.Select(string.Format("NO='{0}' and ItemCode='{1}' and CustomerCode='{2}'",no,item,partner));
			if(drList != null && drList.Length > 0)
			{
				InvShip ship = new InvShip();
				ship.ShipNo = drList[0]["NO"].ToString();
				ship.ShipSeq = int.Parse(drList[0]["SEQ"].ToString());
				ship.PlanQty = int.Parse(drList[0]["PlanQty"].ToString());
				ship.ActQty = int.Parse(drList[0]["ActQty"].ToString());
				ship.ShipStatus = drList[0]["status"].ToString();
				ship.ItemCode = drList[0]["ItemCode"].ToString();

				return ship;
			}
			else
				return null;
		}
		#endregion

		#region ������ʾ����
		
		private void LoadData()
		{	
			if(this.ucLETicketNo.Text.Trim() == string.Empty)
				return;

			this._shipNo = this.ucLETicketNo.Text.Trim();

			txtSumNum.InnerTextBox.Text = "0";
			int num = 0;
			this._tmpTable.Rows.Clear();
			BindGrid();
			//this._tblRCard.Rows.Clear();

			//��ѯ���ⵥ
			InvShip ship = null;
			object[] objs = _facade.QueryInvShip(this.ucLETicketNo.Text.Trim());
			if(objs != null && objs.Length > 0)
			{
				foreach(object obj in objs)
				{
					ship = obj as InvShip;
					if(ship != null)
					{
						this.AddGridRow(ship);
						num += ship.ActQty;
					}
				}
			}
			else
			{
				this.ErrorMessage("$ShipNo_Error");// ���������������
			}
			txtSumNum.InnerTextBox.Text = num.ToString();
			
			this.txtCartonNum.Value = _facade.GetCartonShipCount(this.ucLETicketNo.Text.Trim().ToUpper()).ToString();

			if(_currseq != "0")
				this.ActivateRow(_currseq);
		}

		/// <summary>
		/// �����µ���ⵥ��ѯ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucLETicketNo_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == '\r' )
			{
				//_inputType.CurrentAction = InputType.ActionType.RCard;
				//this.ucLEInput.Caption = _inputType.GetInputCaption(this.ultraOsType.CheckedIndex);
				LoadData();

                this.txtPartner.Focus();
			}

		}
	
		
		private void UpdateFormData(InvShip ship,InvRCard inv,string action)
		{
			if(ship == null) 
			{
				this.LoadData();
				return;
			}
	
			int n = 0;
			DataRow[] drList = this._tmpTable.Select(string.Format("NO='{0}' and SEQ={1}",ship.ShipNo,ship.ShipSeq));
			if(drList != null && drList.Length > 0)
			{
				DataRow dr = drList[0];
				n = ship.ActQty - int.Parse(dr["ActQty"].ToString());
				dr = drList[0];
				if(ship != null && ship.ActQty > 0)
					dr["ActQty"] = ship.ActQty;
				else
					dr["ActQty"] = 0;

				if(dr != null)
				{
					if(!ActivateRow(dr["SEQ"].ToString())) //�����ǰ��û�б����¼���򽫵�ǰ���кżӽ���������
					{
//						if(inv != null)
//						{
//							if(action == FormAction.Add)
//								this.lbxRCard.Items.Add(new UIShipRCard(inv.RunningCard,inv.ShipCollectType));
//							else
//							{
//								int i = this.lbxRCard.Items.IndexOf(new UIShipRCard(inv.RunningCard,inv.ShipCollectType));
//								if(i>=0)
//									this.lbxRCard.Items.RemoveAt(i);
//							}
//						}
//						else
//						{
//							this.ultraGridContent_AfterRowActivate(null,null);
//						}
					}
				}
			}
			this.txtSumNum.InnerTextBox.Text = (int.Parse(this.txtSumNum.InnerTextBox.Text)  + n).ToString();

		}

		private void DeleteFormData(InvShip ship)
		{
			if(ship == null) 
			{
				this.LoadData();
				return;
			}
	
			int n = 0;
			DataRow[] drList = this._tmpTable.Select(string.Format("NO='{0}' and SEQ={1}",ship.ShipNo,ship.ShipSeq));
			if(drList != null && drList.Length > 0)
			{
				DataRow dr = drList[0];
				n = 0 - int.Parse(dr["ActQty"].ToString());
				dr = drList[0];
				
				dr["ActQty"] = 0;

				if(dr != null)
				{
					//if(this.IsActiveRow(dr["SEQ"].ToString())) //��ɾ�������Ǽ�����У������ListBox
					//{
					//	this.lbxRCard.Items.Clear();
					//}
				}
			}
			this.txtSumNum.InnerTextBox.Text = (int.Parse(this.txtSumNum.InnerTextBox.Text)  + n).ToString();

		}

		class FormAction
		{
			public static string  Add = "Add";
			public static string Del = "Del";
		}
		//�����ǰ�в���Active���У������¼���,��������¼���򷵻�true
		private bool ActivateRow(string seq)
		{
			bool ret = false;
//			foreach(Infragistics.Win.UltraWinGrid.UltraGridRow row in this.ultraGridContent.Rows)
			for ( int iGridRowLoopIndex = 0; iGridRowLoopIndex < this.ultraGridContent.Rows.Count; iGridRowLoopIndex++)
			{
				Infragistics.Win.UltraWinGrid.UltraGridRow row = this.ultraGridContent.Rows[iGridRowLoopIndex];
				if(row.Cells["SEQ"].Text == seq)
				{
					if(!row.Activated)
					{
						row.Activated = true;
						ret = true;
					}
				}
				else
				{
					row.Activated = false;
					row.Selected = false;
				}
			}

			return ret;
		}

		private bool IsActiveRow(string seq)
		{
			bool ret = false;
//			foreach(Infragistics.Win.UltraWinGrid.UltraGridRow row in this.ultraGridContent.Rows)
			for ( int iGridRowLoopIndex = 0; iGridRowLoopIndex < this.ultraGridContent.Rows.Count; iGridRowLoopIndex++)
			{
				Infragistics.Win.UltraWinGrid.UltraGridRow row = this.ultraGridContent.Rows[iGridRowLoopIndex];
				if(row.Cells["SEQ"].Text == seq)
				{
					if(row.Activated)
					{
						return true;
					}
				}
			}

			return ret;
		}
		private DataRow AddGridRow(InvShip ship)
		{
			DataRow dr = _tmpTable.Rows.Add(new object[]
											{
												false,
												ship.ShipNo,
												ship.ShipSeq,
												ship.PartnerCode,
												ship.PartnerDesc,
												FormatHelper.ToDateString(ship.ShipDate),
												ship.ModelCode,
												ship.ItemCode,
												ship.ItemDesc,
												ship.PlanQty,
												ship.ActQty,
												ship.ShipStatus
											}
								);
			return dr;
		}
	

		private void ultraGridContent_AfterRowActivate(object sender, System.EventArgs e)
		{
//			Infragistics.Win.UltraWinGrid.UltraGridRow row = this.ultraGridContent.DisplayLayout.ActiveRow;
//			if(row != null)
//			{
//				string shipNo = row.Cells["NO"].Text;
//				int shipSeq = int.Parse(row.Cells["SEQ"].Text);
//
//				System.Data.DataView dv = new System.Data.DataView(this._tblRCard,"SEQ=" + shipSeq.ToString(),string.Empty,System.Data.DataViewRowState.CurrentRows);
//
//				if(dv.Count == 0)
//				{
//					object[] objsRCard = _facade.QueryInvRCard3(shipNo,shipSeq);
//					if(objsRCard != null)
//					{
//						foreach(object obj in objsRCard)
//						{
//							InvRCard inv = obj as InvRCard;
//							if(inv != null)
//							{
//								//this.lbxRCard.Items.Add(new UIShipRCard(inv.RunningCard,inv.ShipCollectType));
//								this.AddRCardGridRow(inv);
//							}	
//						}
//					}
//
//					dv = new System.Data.DataView(this._tblRCard,"SEQ=" + shipSeq.ToString(),string.Empty,System.Data.DataViewRowState.CurrentRows);
//				}
//
//				this.BindRCard(dv);
//			}
		}

		//private DataRow AddRCardGridRow(InvRCard inv)
		//{
//			DataRow[] drList = this._tblRCard.Select(string.Format("RCard='{0}'",inv.RunningCard));
//			if(drList == null || drList.Length ==0)
//			{
//			DataRow dr = this._tblRCard.Rows.Add(new object[]
//											{
//												inv.RunningCard,
//												inv.ShipCollectType,
//												inv.ShipSeq
//											}
//				);
//			return dr;
//			}
//			else
//				return drList[0];
		//}
		#endregion

		#region ����ɾ������
		
		//ɾ��һ�����к� or Carton
		private void btnDeleteRCard_Click(object sender, System.EventArgs e)
		{
			if ( this.ucLETicketNo.Text.Trim() == string.Empty )
			{
				this.ErrorMessage("$Error_CS_Input_StockIn_TicketNo");
                this.ucLETicketNo.Focus();
				return;
			}	
			if ( this.ucLEInput.Text.Trim() == string.Empty )
			{
				return;
			}

			string[] idList = null;
			if ( this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.Planate.ToString())
			{
				#region ��ά�������
				
				#endregion
			}
			else if(this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.Carton.ToString())
			{
				#region Carton
				BenQGuru.eMES.DataCollect.DataCollectFacade dc = new BenQGuru.eMES.DataCollect.DataCollectFacade(this.DataProvider);
				object[] objidList = dc.GetSimulationFromCarton(this.ucLEInput.Text.Trim().ToUpper());
				if ( objidList == null || objidList.Length == 0)
				{
					this.ErrorMessage("$CS_RCard_List_Is_Empty");
					return;
				}
				else
				{
					idList = new string[objidList.Length];
					int i = 0;
					foreach (BenQGuru.eMES.Domain.DataCollect.Simulation sim in objidList )
					{
						idList[i++] = sim.RunningCard;
					}
				}
				#endregion
			}
			else if(this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.PCS.ToString() )
			{
				idList = new string[]{this.ucLEInput.Text.Trim().ToUpper()};
			}
			try
			{
				this.DataProvider.BeginTransaction();

				InvRCard inv = null;
				InvShip ship = null;
				string shipNo = this.ucLETicketNo.Text.ToUpper().Trim();

				bool isDelete = false;
				foreach(string rcard in idList)
				{
					//�������к�
					object[] objs =_facade.QueryInvRCard4(shipNo,rcard);
					if(objs != null && objs.Length > 0)
					{
						inv = objs[0] as InvRCard;
						if(inv != null)
						{
							//�жϳ�������״̬
							object[] objShips = _facade.QueryInvShip(shipNo,inv.ShipSeq,1,int.MaxValue);
							if(objShips != null && objShips.Length > 0)
							{
								ship = objShips[0] as InvShip;
								if(ship != null)
								{
									if(ship.ShipStatus != ShipStatus.Shipping)
										throw new NoAllowDeleteException();

									ship.ActQty -= 1;
									if(ship.ActQty < 0)
										ship.ActQty = 0;

					
									_facade.UpdateInvShip(ship);

							
								}
							}

							if(inv.ShipCollectType != this.ultraOsType.CheckedItem.DataValue.ToString())
								throw new Exception("$INV_SHIP_COLLECT_TYPE_ERROR");

							//�������к�
							inv.ShipDate = 0;
							inv.ShipTime = 0;
							inv.ShipUser = string.Empty;
							inv.ShipNO = string.Empty;
							inv.ShipSeq = 0;
							inv.RCardStatus = RCardStatus.Received;
							inv.ShipCollectType = string.Empty;

							_facade.UpdateInvRCard(inv);

							this.UpdateFormData(ship,inv,FormAction.Del);

							isDelete = true;
						}
					}
				}

				this.ultraGridContent_AfterRowActivate(null,null);

				if(isDelete)
				{
					if(this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.Carton.ToString())
					{
						this.txtCartonNum.Value = (int.Parse(this.txtCartonNum.Value)-1).ToString();
					}
					this.SucessMessage("$CS_Delete_Success");
				}
				this.ucLEInput.Text = string.Empty;
				this.DataProvider.CommitTransaction();

				//				if(cbFIFO.Checked)
				//				{
				btnCheck_Click(sender,e);
				//				}
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				this.ErrorMessage(ex.Message);
				this.LoadData();
			}
			finally
			{
				this.ucLEInput.Text = string.Empty;
				this.CloseConnection();
			}
		}


		//ɾ��һ����ϸ
		private void btnDeleteDetail_Click(object sender, System.EventArgs e)
		{
			bool isDeleted = false;
			try
			{
				this.DataProvider.BeginTransaction();
//				foreach(Infragistics.Win.UltraWinGrid.UltraGridRow row in this.ultraGridContent.Rows)
				for ( int iGridRowLoopIndex = 0; iGridRowLoopIndex < this.ultraGridContent.Rows.Count; iGridRowLoopIndex++)
				{
					Infragistics.Win.UltraWinGrid.UltraGridRow row = this.ultraGridContent.Rows[iGridRowLoopIndex];
					if(row.Cells["*"].Text.ToLower() == "true")
					{
						string shipNo = row.Cells["NO"].Text;
						int seq = int.Parse(row.Cells["SEQ"].Text);
						this._currseq = "0";

			
						object[] objShips = _facade.QueryInvShip(shipNo,seq,1,int.MaxValue);
						if(objShips != null && objShips.Length > 0)
						{
							InvShip ship = objShips[0] as InvShip;
							if(ship != null)
							{
								//�жϳ�������״̬
								if(ship.ShipStatus != ShipStatus.Shipping)
									throw new NoAllowDeleteException();
								
								object[] objs =_facade.QueryInvRCard3(shipNo,seq);
								if(objs != null && objs.Length > 0)
								{
									foreach(object obj in objs)
									{
										InvRCard inv = obj as InvRCard;
										if(inv != null)
										{
											inv.ShipDate = 0;
											inv.ShipTime = 0;
											inv.ShipUser = string.Empty;
											inv.ShipNO = string.Empty;
											inv.ShipSeq = 0;
											inv.RCardStatus = RCardStatus.Received;
											inv.ShipCollectType = string.Empty;
											_facade.UpdateInvRCard(inv);
										}
									}
								}

								//ɾ��������
								ship.ActQty = 0;
								_facade.UpdateInvShip(ship);

								//���½�����ʾ
								DeleteFormData(ship);

//								DataRow[] drList = this._tblRCard.Select(string.Format("SEQ={0}",ship.ShipSeq));
//								if(drList != null)
//								{
//									foreach(DataRow dr in drList)
//									{
//										dr.Delete();
//									}
//								}
							}	
						}
						isDeleted = true;
					}

				}
				this.DataProvider.CommitTransaction();
				if(isDeleted)
				{
					this.ultraGridContent_AfterRowActivate(null,null);
					this.txtCartonNum.Value = _facade.GetCartonShipCount(this.ucLETicketNo.Text.Trim().ToUpper()).ToString();
					this.SucessMessage("$CS_Delete_Success");
				}
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				this.ErrorMessage(ex.Message);
				this.LoadData();
			}
			finally
			{
				this.CloseConnection();
			}
		}
		
		#endregion

		#region ִ����ɳ����Ķ���
		private void ucBtnComplete_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.DataProvider.BeginTransaction();

				_facade.CompleteInvShip(this.ucLETicketNo.Text.Trim(),
					ApplicationService.Current().UserCode);	

				this.DataProvider.CommitTransaction();

				foreach(DataRow dr in this._tmpTable.Rows)
				{
					dr["status"] = ShipStatus.Shipped;
				}

				this.SucessMessage("$Ship_Completed");
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				this.ErrorMessage(ex.Message);
				this.LoadData();
			}
			finally
			{
				this.CloseConnection();
			}
		}

		//ȡ�����
		private void btnUnComplete_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.DataProvider.BeginTransaction();

				_facade.UnCompleteInvShip(this.ucLETicketNo.Text.Trim(),
					ApplicationService.Current().UserCode);	

				this.DataProvider.CommitTransaction();

				foreach(DataRow dr in this._tmpTable.Rows)
				{
					dr["status"] = ShipStatus.Shipping;
				}

				this.SucessMessage("$Ship_UnCompleted"); //ȡ����ɳɹ�
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				this.ErrorMessage(ex.Message);
				this.LoadData();
			}
			finally
			{
				this.CloseConnection();
			}
		}
		#endregion		

		private void txtPartner_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == '\r')
			{
				if(_facade.QueryInvShipPartnerCount(this.ucLETicketNo.Text.Trim(),
					this.txtPartner.Text.Trim()) <= 0)
				{
					this.ErrorMessage("$Error_CS_Ship_Partner_No_Exist");
                    this.txtPartner.Focus();
					return;
				}

                this.ucLEInput.Focus();
				//_inputType.PartnerCode = this.ucLEInput.InnerTextBox.Text.Trim();
				//this.ucLEInput.Caption = _inputType.GetInputCaption(this.ultraOsType.CheckedIndex);

				//binding OrderNO.
//				this.ucLCOrderNO.Clear();
//				this.ucLCOrderNO.AddItem( "", "" );
//				OrderFacade orderFacade = new OrderFacade( _domainDataProvider );
//				string[] orders = orderFacade.QueryOrderByPartner(this.txtPartner.Value.Trim());
//				if( orders!=null && orders.Length>0 )
//				{
//					for( int i=0; i<orders.Length; i++)
//					{
//						this.ucLCOrderNO.AddItem( orders[i], orders[i] );
//					}
//				}
			}
		}

		private void cbNormalRCard_CheckedChanged(object sender, System.EventArgs e)
		{
			txtDay.Enabled = cbFIFO.Checked;
		}

		private void grpFIFO_Resize(object sender, System.EventArgs e)
		{
		
		}

		private void FNormalShip_Resize(object sender, System.EventArgs e)
		{
			SetPosition();
		}

		private void SetPosition()
		{
			grpFIFO.Left = 0;
			grpFIFO.Top = 0;//panel4.Top ;
			grpFIFO.Width = panel4.Width;

			grpFIFO.Height = Convert.ToInt32(Decimal.Truncate(Convert.ToDecimal(panel4.Height * 0.2)));

			if(grpFIFO.Visible == true)
			{
				gbxDetail.Top = grpFIFO.Height;
				gbxDetail.Height = panel4.Height - grpFIFO.Height;
			}
			else
			{
				gbxDetail.Top = 0;
				gbxDetail.Height = panel4.Height;
			}

			gbxDetail.Width = panel4.Width;
		}

		private void FNormalShip_SizeChanged(object sender, System.EventArgs e)
		{
			SetPosition();
		}

		#region Laws Lu,2006/08/25	FIFO

		private object[] LoadFIFODataSource()
		{
			object[] objs = null;

			int d = Web.Helper.FormatHelper.TODateInt(DateTime.Today.AddDays(-Convert.ToInt32(txtDay.Value)));

			object[] objItems = _facade.QueryInvShip(this.ucLETicketNo.Text.Trim());

			ArrayList alItems = new ArrayList();
			if(objItems != null && objItems.Length > 0)
			{
				foreach(InvShip invShip in objItems)
				{
					if(!alItems.Contains(invShip.ItemCode))
					{
						alItems.Add(invShip.ItemCode);
					}
				}
			}
			string itemCodes = String.Join("','",(string[])alItems.ToArray(typeof(string)));

			//			if( this.ultraOsType.CheckedItem.DataValue.ToString() == CollectionType.Carton.ToString() )
			//			{
			objs = (new InventoryFacade(DataProvider)).QueryFIFOInvRCardByItem(itemCodes/*,cbxItemCode.ComboBoxData.Text.ToUpper().Trim()*/,d);
			//			}
			//			else
			//			{
			//				objs = (new InventoryFacade(DataProvider)).QueryFIFOInvRCardByRcard(itemCodes/*,cbxItemCode.ComboBoxData.Text.ToUpper().Trim()*/,d);
			//			}
			return objs;
		}

		private void FillFIFOGrid(object[] objs)
		{
			dtFIFO.Clear();
			if(objs != null)
			{
				foreach(object obj in objs)
				{
					if(obj != null)
					{
						InvRCard invRcard = obj as InvRCard;


						dtFIFO.Rows.Add(new object[]{
														invRcard.CartonCode
														,invRcard.RunningCard
														,invRcard.MOCode
														,invRcard.ItemCode
														,invRcard.ReceiveDate
														,invRcard.ReceiveTime
													});
					}
				}

				dtFIFO.AcceptChanges();

				ultraGridFIFO.DataSource = dtFIFO;

				if(dtFIFO.Rows.Count > 0)
				{
					grpFIFO.Visible = true;
					SetPosition();
				}
			}
		}


		private void btnCheck_Click(object sender, System.EventArgs e)
		{
			if(cbFIFO.Checked == true)
			{
				//			if(rbAdd.Checked == true)
				//			{
				FillFIFOGrid(LoadFIFODataSource());

				//			}
			}
			else
			{
				dtFIFO.Clear();
				dtFIFO.AcceptChanges();
				grpFIFO.Visible = false;
				SetPosition();
			}
		}

		private void ultraGridFIFO_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			UltraWinGridHelper gridHelper = new UltraWinGridHelper(this.ultraGridFIFO);
			gridHelper.AddCommonColumn("CartonCode","Carton���");
			gridHelper.AddCommonColumn("Rcard","��Ʒ���к�");
			gridHelper.AddCommonColumn("ModelCode","��Ʒ��");
			gridHelper.AddCommonColumn("ItemCode","��Ʒ����");
			gridHelper.AddCommonColumn("RecDate","�������");
			gridHelper.AddCommonColumn("RecTime","���ʱ��");
		}

		#endregion

		private void btnOutputFIFO_Click(object sender, System.EventArgs e)
		{
			FileDialog fd = new SaveFileDialog();
			fd.AddExtension = true;
			//fd.
			fd.Filter = "CSV�����ļ�|*.CSV";

			DialogResult diaResult =  fd.ShowDialog();

			string fileName = fd.FileName;

			if(diaResult == DialogResult.OK &&  fileName != String.Empty)
			{
				System.IO.FileStream fs =  System.IO.File.Create(fileName);
				try
				{
					string colName = String.Empty ;

					foreach(DataColumn dc in dtFIFO.Columns)
					{
						colName += dc.ColumnName + ",";
					}

					colName = colName.Substring(0,colName.Length - 1);
					colName += "\r\n";

					byte[] btCol = System.Text.Encoding.Default.GetBytes(colName);
					fs.Write(btCol,0,btCol.Length);

					foreach(DataRow dr in dtFIFO.Rows)
					{
						string rowData = String.Empty ;

						rowData = dr["CartonCode"].ToString() + ","
							+ dr["Rcard"].ToString() + ","
							+ dr["ModelCode"].ToString() + ","
							+ dr["ItemCode"].ToString() + ","
							+ dr["RecDate"].ToString() + ","
							+ dr["RecTime"].ToString();
						rowData = rowData + "\r\n";
				
						byte[] bt = System.Text.Encoding.Default.GetBytes(rowData);
						fs.Write(bt,0,bt.Length);
					}
				}
				catch(Exception ex)
				{
					Log.Error(ex.Message);
				}
				finally
				{
					fs.Flush();
					fs.Close();
				}
			}
			
			
		}
		//Laws Lu,2006/12/25 ������뱳��ɫ��Ϊǳ�̣��Ƴ��ָ�����
		private void ucLETicketNo_Enter(object sender, System.EventArgs e)
		{
			ucLETicketNo.BackColor =  Color.GreenYellow;
		}

		private void txtPartner_Enter(object sender, System.EventArgs e)
		{
			txtPartner.BackColor =  Color.GreenYellow;
		}

		private void ucLEInput_Enter(object sender, System.EventArgs e)
		{
			ucLEInput.BackColor =  Color.GreenYellow;
		}

		private void ucLETicketNo_Leave(object sender, System.EventArgs e)
		{
			ucLETicketNo.BackColor =  Color.White;
		}

		private void txtPartner_Leave(object sender, System.EventArgs e)
		{
			txtPartner.BackColor =  Color.White;
		}

		private void ucLEInput_Leave(object sender, System.EventArgs e)
		{
			ucLEInput.BackColor =  Color.White;
		}

		private void FNormalShip_Activated(object sender, System.EventArgs e)
		{
            ucLETicketNo.Focus();
			ucLETicketNo.BackColor = Color.GreenYellow;
		}

		#region �ڲ��ඨ��
//		class InputType
//		{
//			public enum ActionType{Partner,RCard};
//
//			public ActionType CurrentAction = ActionType.Partner;
//
//			public string GetInputCaption(int rd)
//			{
//				if(CurrentAction == ActionType.Partner)
//				{
//					CurrentAction = ActionType.RCard;
//					return "�������ά������Ʒ���к�";
//				}
//				else
//				{
//					CurrentAction = ActionType.Partner;
//					return "�����뾭���̴���";
//				}
//			}
//
//			public string PartnerCode = string.Empty;
//			public string ItemCode = string.Empty;
//		}

		#endregion

	}

	public class ShipChecker
	{

//		7.1	�����ѡ�ˡ��������Ʒ������ѡ������ά����������������Ĳɼ�����Carton����Ʒ���кţ�
//		��Ӧ��ÿ����Ʒ�������������Ʒ����δ���������У�������ͬ��Ʒ����Ĳ�Ʒ���кŵ�������ڲ�����
//		����ǰ���ڿ۳��趨��������֮ǰ��������ɼ��Ĳ�Ʒ����������ڡ���ǰ���ڿ۳��趨����������
//		��Ҫ����Ƿ���ڱ�֮����������ͬ��Ʒ�����δ�������ϡ����磬��ǰ����2006/7/20,�趨������5�죬
//		���赱ǰ�ɼ��İ�װ�����������2006/7/17,��ʱ���ϵͳ�д���2006/7/15��ǰ����ͬ��Ʒ��δ������������ϣ�
//		������ɼ������Ʒ����ʱ��ʾ�û����ⷿ�л����ڸ���������ڵ���ͬ��Ʒ�����Ȳɼ��ǲ��ֲ�Ʒ����
//		�����ǰ�ɼ��İ�װ�����������2006/7/13,��ʱ���ϵͳ�д���2006/7/13��ǰ����ͬ��Ʒ��δ������������ϣ�
//		����Ȼ������ɼ����䣬����ʾ�û��ɼ��������Ĳ�Ʒ��
//		7.2	�����������ʱ��Ҫ�ȶ�ÿһ��Ʒ���кŵ����ʱ�䣬����ɼ���ʽ�ǰ�װ��ţ�������񲻰��������Ʒ��
//		7.3	ֻ��Ҫ��鵽������ڣ�����Ҫ���Ǿ�������ʱ�䣨ʱ���룩
//		7.4	���δ��ѡ���������Ʒ������ѡ��趨�����������ı�����Ч���û��������롰�쳣���Ʒ����������Ҫ����������


		public ShipChecker(BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider _DataProvider) 
		{
			DataProvider = _DataProvider;

			this.facade = new InventoryFacade(this.DataProvider);
		}

		private BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider DataProvider;
		private InventoryFacade facade = null;

		public void Check(InvRCard testinv ,string Day,bool NormalChecked,bool PCS)
		{
			if(NormalChecked)
			{
				if(testinv.INVRardType != INVRardType.Normal)
					throw new Exception("$ReceiveRcard_Must_Be_Normal"); //��Ʒ���кŵ�������ͱ������������Ʒ
			}
			else
			{
				if(testinv.INVRardType != INVRardType.Unnormal)
					throw new Exception("$ReceiveRcard_Must_Be_UnNormal");//��Ʒ���кŵ�������ͱ������쳣���Ʒ
			}


			if(!IsNumber(Day))
				throw new Exception("$Day_Must_Be_Number");//��������������


			double day = double.Parse(Day);

			int d = Web.Helper.FormatHelper.TODateInt(DateTime.Today.AddDays(-day));

			if(d > testinv.ReceiveDate)
				d = testinv.ReceiveDate;

			int count = 0;
			if(PCS)
			{
				count = facade.QueryInvRCardCount(testinv.RunningCard,testinv.ItemCode,d);
			}
			else
			{
				count = facade.QueryInvRCardByCartonCount(testinv.CartonCode,testinv.ItemCode,d);
			}

			if(count > 0)
				throw new Exception("$Pls_Shipp_Earlier_ReceiveRcard"); //�ⷿ�л����ڸ���������ڵ���ͬ��Ʒ�����Ȳɼ��ǲ��ֲ�Ʒ
		}

		private bool IsNumber(string number)
		{
			try
			{
				int.Parse(number);

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
