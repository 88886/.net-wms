using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Client.Service;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.LotDataCollect.Action;
using BenQGuru.eMES.LotDataCollect;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.LotDataCollect;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.TS;
using BenQGuru.eMES.TSModel;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

using UserControl;


namespace BenQGuru.eMES.Client
{
    public class FLotCollectionMetrial :  BaseForm
    {
        #region �ؼ�
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.GroupBox GroupItemInfo;
        private System.Windows.Forms.GroupBox groupBoxInput;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label lblItemDesc;
        private System.Windows.Forms.Label labelItemCodeDesc;

        private UserControl.UCMessage ucMessageInfo;
        private UserControl.UCLabelEdit edtMO;
        private UserControl.UCLabelEdit txtLotLetter;
        private UserControl.UCLabelEdit txtLotLen;
        private UserControl.UCLabelEdit txtItemCode;
        private UserControl.UCButton btnExit;
        public UserControl.UCLabelEdit edtInput;
        #endregion

        #region ȫ�ֱ���

        private const string opCollectAutoCollectLotPart = "0";
        private const string opCollectNeedInputLotPart = "1";
        private string _FunctionName = string.Empty;
        private string _lotCode = string.Empty;//�����(Դ)�������� Add By Bernard @ 2010-11-02

        private DataTable m_LotDT;
        private DataSet m_LotSet;

        private Hashtable listOpBomKeyParts = new Hashtable();
        private ProductInfo productInfo = null;
        private Hashtable listActionCheckStatus = new Hashtable();
        private Hashtable listEndActionCheckStatus = new Hashtable();
        private Resource Resource;

        private MaterialFacade _MaterialFacade = null;
        private DataCollectFacade _DataCollectFacade = null;
        MOFacade moFacade = null;
        BenQGuru.eMES.BaseSetting.BaseModelFacade baseModelFacade = null;
        MOLotFacade _MOLotFacade = null;
        private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
        public IDomainDataProvider DataProvider
        {
            get
            {
                return _domainDataProvider;
            }
        }

        #region ���̿���

        private int _FlowControl = -1;

        private bool _NeedInputLotPart = false;
        private int _CollectedCount = 0;

        private bool _CancelChanged = false;
        private bool _IgnoreChangedEvent = false;

        #endregion

        #region �м�����

        private string _inno;

        //private string ID;
        private int _CollectIDCount;
        private bool _HaveCollectMaterial = false;

        private object[] _OPBOMDetailList;
        private ArrayList _OPBOMDetailArrayList = new ArrayList();

        private object[] _PreparedLotPartList;
        private ArrayList _PreparedLotPartArrayList = new ArrayList();

        private object[] _objBomDetailNotFilter;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridMain;


        ArrayList _InputPartList = new ArrayList();

        #endregion

        #endregion

        #region Form��ʼ���Զ�����

        public FLotCollectionMetrial()
        {
            InitializeComponent();

            //ץȡ���趨AutoMaterialOP
            baseModelFacade = new BaseModelFacade(this.DataProvider);
            moFacade = new MOFacade(this.DataProvider);
            Operation2Resource op2Res = (Operation2Resource)baseModelFacade.GetOperationByResource(ApplicationService.Current().ResourceCode);
            if (op2Res != null)
            {
                string opCode = op2Res.OPCode.Trim().ToUpper();
                Domain.BaseSetting.Parameter autoMaterialOP = (Domain.BaseSetting.Parameter)(new SystemSettingFacade(this.DataProvider)).GetParameter(opCode, "AUTOMATERIAL");
            }

            UserControl.UIStyleBuilder.FormUI(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLotCollectionMetrial));
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            this.groupBoxInput = new System.Windows.Forms.GroupBox();
            this.edtInput = new UserControl.UCLabelEdit();
            this.btnExit = new UserControl.UCButton();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.GroupItemInfo = new System.Windows.Forms.GroupBox();
            this.txtItemCode = new UserControl.UCLabelEdit();
            this.lblItemDesc = new System.Windows.Forms.Label();
            this.labelItemCodeDesc = new System.Windows.Forms.Label();
            this.txtLotLetter = new UserControl.UCLabelEdit();
            this.txtLotLen = new UserControl.UCLabelEdit();
            this.edtMO = new UserControl.UCLabelEdit();
            this.ultraGridMain = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ucMessageInfo = new UserControl.UCMessage();
            this.groupBoxInput.SuspendLayout();
            this.groupBoxInfo.SuspendLayout();
            this.GroupItemInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridMain)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxInput
            // 
            this.groupBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInput.Controls.Add(this.edtInput);
            this.groupBoxInput.Controls.Add(this.btnExit);
            this.groupBoxInput.Location = new System.Drawing.Point(0, 534);
            this.groupBoxInput.Name = "groupBoxInput";
            this.groupBoxInput.Size = new System.Drawing.Size(980, 45);
            this.groupBoxInput.TabIndex = 153;
            this.groupBoxInput.TabStop = false;
            // 
            // edtInput
            // 
            this.edtInput.AllowEditOnlyChecked = false;
            this.edtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.edtInput.AutoSelectAll = false;
            this.edtInput.AutoUpper = true;
            this.edtInput.Caption = "�����";
            this.edtInput.Checked = false;
            this.edtInput.EditType = UserControl.EditTypes.String;
            this.edtInput.Location = new System.Drawing.Point(17, 13);
            this.edtInput.MaxLength = 40;
            this.edtInput.Multiline = false;
            this.edtInput.Name = "edtInput";
            this.edtInput.PasswordChar = '\0';
            this.edtInput.ReadOnly = false;
            this.edtInput.ShowCheckBox = false;
            this.edtInput.Size = new System.Drawing.Size(249, 24);
            this.edtInput.TabIndex = 9;
            this.edtInput.TabNext = false;
            this.edtInput.Value = "";
            this.edtInput.WidthType = UserControl.WidthTypes.Long;
            this.edtInput.XAlign = 66;
            this.edtInput.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edtInput_TxtboxKeyPress);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.ButtonType = UserControl.ButtonTypes.Exit;
            this.btnExit.Caption = "�˳�";
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Location = new System.Drawing.Point(324, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(88, 22);
            this.btnExit.TabIndex = 10;
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInfo.Controls.Add(this.GroupItemInfo);
            this.groupBoxInfo.Controls.Add(this.txtLotLetter);
            this.groupBoxInfo.Controls.Add(this.txtLotLen);
            this.groupBoxInfo.Controls.Add(this.edtMO);
            this.groupBoxInfo.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxInfo.Location = new System.Drawing.Point(0, 5);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(980, 93);
            this.groupBoxInfo.TabIndex = 3;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "������Ϣָ��";
            // 
            // GroupItemInfo
            // 
            this.GroupItemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupItemInfo.Controls.Add(this.txtItemCode);
            this.GroupItemInfo.Controls.Add(this.lblItemDesc);
            this.GroupItemInfo.Controls.Add(this.labelItemCodeDesc);
            this.GroupItemInfo.Location = new System.Drawing.Point(6, 46);
            this.GroupItemInfo.Name = "GroupItemInfo";
            this.GroupItemInfo.Size = new System.Drawing.Size(980, 44);
            this.GroupItemInfo.TabIndex = 217;
            this.GroupItemInfo.TabStop = false;
            this.GroupItemInfo.Text = "��Ʒ��Ϣ";
            // 
            // txtItemCode
            // 
            this.txtItemCode.AllowEditOnlyChecked = true;
            this.txtItemCode.AutoSelectAll = false;
            this.txtItemCode.AutoUpper = true;
            this.txtItemCode.BackColor = System.Drawing.Color.Transparent;
            this.txtItemCode.Caption = "��Ʒ";
            this.txtItemCode.Checked = false;
            this.txtItemCode.EditType = UserControl.EditTypes.String;
            this.txtItemCode.Enabled = false;
            this.txtItemCode.Location = new System.Drawing.Point(81, 14);
            this.txtItemCode.MaxLength = 40;
            this.txtItemCode.Multiline = false;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.PasswordChar = '\0';
            this.txtItemCode.ReadOnly = false;
            this.txtItemCode.ShowCheckBox = false;
            this.txtItemCode.Size = new System.Drawing.Size(170, 24);
            this.txtItemCode.TabIndex = 216;
            this.txtItemCode.TabNext = true;
            this.txtItemCode.TabStop = false;
            this.txtItemCode.Value = "";
            this.txtItemCode.WidthType = UserControl.WidthTypes.Normal;
            this.txtItemCode.XAlign = 118;
            // 
            // lblItemDesc
            // 
            this.lblItemDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemDesc.Location = new System.Drawing.Point(290, 10);
            this.lblItemDesc.Name = "lblItemDesc";
            this.lblItemDesc.Size = new System.Drawing.Size(74, 24);
            this.lblItemDesc.TabIndex = 215;
            this.lblItemDesc.Text = "��Ʒ����";
            this.lblItemDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelItemCodeDesc
            // 
            this.labelItemCodeDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelItemCodeDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelItemCodeDesc.Location = new System.Drawing.Point(370, 10);
            this.labelItemCodeDesc.Name = "labelItemCodeDesc";
            this.labelItemCodeDesc.Size = new System.Drawing.Size(395, 24);
            this.labelItemCodeDesc.TabIndex = 214;
            this.labelItemCodeDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLotLetter
            // 
            this.txtLotLetter.AllowEditOnlyChecked = true;
            this.txtLotLetter.AutoSelectAll = false;
            this.txtLotLetter.AutoUpper = true;
            this.txtLotLetter.Caption = "��Ʒ���к����ַ�";
            this.txtLotLetter.Checked = false;
            this.txtLotLetter.EditType = UserControl.EditTypes.String;
            this.txtLotLetter.Enabled = false;
            this.txtLotLetter.Location = new System.Drawing.Point(513, 16);
            this.txtLotLetter.MaxLength = 40;
            this.txtLotLetter.Multiline = false;
            this.txtLotLetter.Name = "txtLotLetter";
            this.txtLotLetter.PasswordChar = '\0';
            this.txtLotLetter.ReadOnly = false;
            this.txtLotLetter.ShowCheckBox = true;
            this.txtLotLetter.Size = new System.Drawing.Size(258, 24);
            this.txtLotLetter.TabIndex = 26;
            this.txtLotLetter.TabNext = false;
            this.txtLotLetter.Value = "";
            this.txtLotLetter.WidthType = UserControl.WidthTypes.Normal;
            this.txtLotLetter.XAlign = 638;
            // 
            // txtLotLen
            // 
            this.txtLotLen.AllowEditOnlyChecked = true;
            this.txtLotLen.AutoSelectAll = false;
            this.txtLotLen.AutoUpper = true;
            this.txtLotLen.Caption = "��Ʒ���кų���";
            this.txtLotLen.Checked = false;
            this.txtLotLen.EditType = UserControl.EditTypes.Integer;
            this.txtLotLen.Enabled = false;
            this.txtLotLen.Location = new System.Drawing.Point(257, 16);
            this.txtLotLen.MaxLength = 40;
            this.txtLotLen.Multiline = false;
            this.txtLotLen.Name = "txtLotLen";
            this.txtLotLen.PasswordChar = '\0';
            this.txtLotLen.ReadOnly = false;
            this.txtLotLen.ShowCheckBox = true;
            this.txtLotLen.Size = new System.Drawing.Size(246, 24);
            this.txtLotLen.TabIndex = 25;
            this.txtLotLen.TabNext = false;
            this.txtLotLen.Value = "";
            this.txtLotLen.WidthType = UserControl.WidthTypes.Normal;
            this.txtLotLen.XAlign = 370;
            // 
            // edtMO
            // 
            this.edtMO.AllowEditOnlyChecked = true;
            this.edtMO.AutoSelectAll = false;
            this.edtMO.AutoUpper = true;
            this.edtMO.Caption = "�趨��������";
            this.edtMO.Checked = false;
            this.edtMO.EditType = UserControl.EditTypes.String;
            this.edtMO.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.edtMO.Location = new System.Drawing.Point(17, 16);
            this.edtMO.MaxLength = 40;
            this.edtMO.Multiline = false;
            this.edtMO.Name = "edtMO";
            this.edtMO.PasswordChar = '\0';
            this.edtMO.ReadOnly = false;
            this.edtMO.ShowCheckBox = true;
            this.edtMO.Size = new System.Drawing.Size(234, 24);
            this.edtMO.TabIndex = 3;
            this.edtMO.TabNext = true;
            this.edtMO.Value = "";
            this.edtMO.WidthType = UserControl.WidthTypes.Normal;
            this.edtMO.XAlign = 118;
            this.edtMO.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.INNO_TxtboxKeyPress);
            this.edtMO.CheckBoxCheckedChanged += new System.EventHandler(this.edtMO_CheckBoxCheckedChanged);
            // 
            // ultraGridMain
            // 
            this.ultraGridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridMain.DisplayLayout.Appearance = appearance27;
            this.ultraGridMain.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridMain.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance28.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance28.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMain.DisplayLayout.GroupByBox.Appearance = appearance28;
            appearance29.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMain.DisplayLayout.GroupByBox.BandLabelAppearance = appearance29;
            this.ultraGridMain.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance30.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance30.BackColor2 = System.Drawing.SystemColors.Control;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance30.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMain.DisplayLayout.GroupByBox.PromptAppearance = appearance30;
            this.ultraGridMain.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridMain.DisplayLayout.MaxRowScrollRegions = 1;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            appearance31.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridMain.DisplayLayout.Override.ActiveCellAppearance = appearance31;
            appearance32.BackColor = System.Drawing.SystemColors.Highlight;
            appearance32.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridMain.DisplayLayout.Override.ActiveRowAppearance = appearance32;
            this.ultraGridMain.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridMain.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance33.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridMain.DisplayLayout.Override.CardAreaAppearance = appearance33;
            appearance34.BorderColor = System.Drawing.Color.Silver;
            appearance34.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridMain.DisplayLayout.Override.CellAppearance = appearance34;
            this.ultraGridMain.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraGridMain.DisplayLayout.Override.CellPadding = 0;
            appearance35.BackColor = System.Drawing.SystemColors.Control;
            appearance35.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance35.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance35.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMain.DisplayLayout.Override.GroupByRowAppearance = appearance35;
            appearance36.TextHAlignAsString = "Left";
            this.ultraGridMain.DisplayLayout.Override.HeaderAppearance = appearance36;
            this.ultraGridMain.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridMain.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridMain.DisplayLayout.Override.RowAppearance = appearance37;
            this.ultraGridMain.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridMain.DisplayLayout.Override.TemplateAddRowAppearance = appearance38;
            this.ultraGridMain.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridMain.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridMain.Location = new System.Drawing.Point(0, 111);
            this.ultraGridMain.Name = "ultraGridMain";
            this.ultraGridMain.Size = new System.Drawing.Size(980, 190);
            this.ultraGridMain.TabIndex = 157;
            this.ultraGridMain.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridMain_InitializeLayout);
            // 
            // ucMessageInfo
            // 
            this.ucMessageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucMessageInfo.BackColor = System.Drawing.Color.Gainsboro;
            this.ucMessageInfo.ButtonVisible = false;
            this.ucMessageInfo.Location = new System.Drawing.Point(0, 307);
            this.ucMessageInfo.Name = "ucMessageInfo";
            this.ucMessageInfo.Size = new System.Drawing.Size(980, 233);
            this.ucMessageInfo.TabIndex = 156;
            this.ucMessageInfo.TabStop = false;
            this.ucMessageInfo.WorkingErrorAdded += new UserControl.WorkingErrorAddedEventHandler(this.ucMessageInfo_WorkingErrorAdded);
            // 
            // FLotCollectionMetrial
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(980, 582);
            this.Controls.Add(this.ultraGridMain);
            this.Controls.Add(this.groupBoxInfo);
            this.Controls.Add(this.ucMessageInfo);
            this.Controls.Add(this.groupBoxInput);
            this.Name = "FLotCollectionMetrial";
            this.Text = "���ϲɼ�";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Deactivate += new System.EventHandler(this.FCollectionMetrial_Deactivate);
            this.Load += new System.EventHandler(this.FCollectionMetrial_Load);
            this.Activated += new System.EventHandler(this.FCollectionMetrial_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FCollectionMetrial_FormClosed);
            this.groupBoxInput.ResumeLayout(false);
            this.groupBoxInfo.ResumeLayout(false);
            this.GroupItemInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        #region �����������Grid()

        private void InitializeMainGrid()
        {
            this.m_LotSet = new DataSet();

            this.m_LotDT = new DataTable("LotDT");
            this.m_LotDT.Columns.Add("LotCode", typeof(string));
            this.m_LotDT.Columns.Add("MoCode", typeof(string));
            this.m_LotDT.Columns.Add("LotQty", typeof(string));
            this.m_LotDT.Columns.Add("GoodQty", typeof(string));
            this.m_LotDT.Columns.Add("NGQty", typeof(string));
            this.m_LotDT.Columns.Add("ItemCode", typeof(string));
            this.m_LotDT.Columns.Add("ProductStatus", typeof(string));
            this.m_LotDT.Columns.Add("CollectStatus", typeof(string));
            this.m_LotDT.Columns.Add("MUser", typeof(string));
            this.m_LotDT.Columns.Add("BeginDate", typeof(string));
            this.m_LotDT.Columns.Add("BeginTime", typeof(string));
            this.m_LotDT.Columns.Add("EndDate", typeof(string));
            this.m_LotDT.Columns.Add("EndTime", typeof(string));
            this.m_LotSet.Tables.Add(m_LotDT);
            this.m_LotSet.AcceptChanges();
            //this.ultraGridMain.DataSource = this.m_LotSet;
        }

        private void ultraGridMain_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // ����Ӧ�п�
            e.Layout.AutoFitColumns = false;
            e.Layout.Override.AllowColSizing = AllowColSizing.Free;
            e.Layout.ScrollBounds = ScrollBounds.ScrollToFill;

            //e.Layout.MaxBandDepth = 1;
            // �Զ��ж��Ƿ���ʾǰ���+��-��
            e.Layout.Override.ExpansionIndicator = ShowExpansionIndicator.CheckOnDisplay;

            // ����Grid��Split���ڸ�������������Ϊ1--������Split
            e.Layout.MaxColScrollRegions = 1;
            e.Layout.MaxRowScrollRegions = 1;

            // ������
            e.Layout.UseFixedHeaders = true;
            e.Layout.Override.FixedHeaderAppearance.BackColor = Color.LightYellow;
            e.Layout.Override.FixedHeaderAppearance.ForeColor = Color.Blue;
            e.Layout.Override.FixedCellAppearance.BackColor = Color.LightYellow;
            e.Layout.Override.FixedCellAppearance.ForeColor = Color.Blue;
            e.Layout.Override.FixedHeaderIndicator = FixedHeaderIndicator.None;

            // ����
            e.Layout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;

            // ������ɾ��
            e.Layout.Override.AllowDelete = DefaultableBoolean.False;

            // ������ʾ
            e.Layout.Bands[0].ScrollTipField = "LotCode";

            // �����п��������
            e.Layout.Bands[0].Columns["LotCode"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["MoCode"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["LotQty"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["GoodQty"].Header.Caption = "��Ʒ����";
            e.Layout.Bands[0].Columns["NGQty"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["ItemCode"].Header.Caption = "��Ʒ����";
            e.Layout.Bands[0].Columns["ProductStatus"].Header.Caption = "��Ʒ����״̬";
            e.Layout.Bands[0].Columns["CollectStatus"].Header.Caption = "�ɼ�״̬";
            e.Layout.Bands[0].Columns["MUser"].Header.Caption = "�ɼ���Ա";
            e.Layout.Bands[0].Columns["BeginDate"].Header.Caption = "��ʼ����";
            e.Layout.Bands[0].Columns["BeginTime"].Header.Caption = "��ʼʱ��";
            e.Layout.Bands[0].Columns["EndDate"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["EndTime"].Header.Caption = "����ʱ��";

            e.Layout.Bands[0].Columns["LotCode"].Width = 100;
            e.Layout.Bands[0].Columns["MoCode"].Width = 100;
            e.Layout.Bands[0].Columns["LotQty"].Width = 60;
            e.Layout.Bands[0].Columns["GoodQty"].Width = 60;
            e.Layout.Bands[0].Columns["NGQty"].Width = 60;
            e.Layout.Bands[0].Columns["ItemCode"].Width = 100;
            e.Layout.Bands[0].Columns["ProductStatus"].Width = 100;
            e.Layout.Bands[0].Columns["CollectStatus"].Width = 100;
            e.Layout.Bands[0].Columns["MUser"].Width = 100;
            e.Layout.Bands[0].Columns["BeginDate"].Width = 100;
            e.Layout.Bands[0].Columns["BeginTime"].Width = 100;
            e.Layout.Bands[0].Columns["EndDate"].Width = 100;
            e.Layout.Bands[0].Columns["EndTime"].Width = 100;

            // ������λ�Ƿ�����༭������λ����ʾ��ʽ
            e.Layout.Bands[0].Columns["LotCode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["MoCode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["LotQty"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["GoodQty"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["NGQty"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["ItemCode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["ProductStatus"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["CollectStatus"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["MUser"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["BeginDate"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["BeginTime"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["EndDate"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["EndTime"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            // ����ɸѡ
            //e.Layout.Bands[0].Columns["LotCode"].AllowRowFiltering = DefaultableBoolean.True;
            //e.Layout.Bands[0].Columns["LotCode"].SortIndicator = SortIndicator.Ascending;

            this.InitGridLanguage(ultraGridMain);
        }

        private void LoadLotSimulationList(string moCode, string resCode)
        {
            InitializeMainGrid();

            if (_DataCollectFacade == null)
            {
                _DataCollectFacade = new DataCollectFacade(this.DataProvider);
            }
            try
            {
                this.ClearLotSimulationList();

                object[] lotGroups = _DataCollectFacade.GetOnlineLotSimulation(moCode, resCode);
                DataRow rowGroup;
                if (lotGroups != null)
                {
                    foreach (LotSimulation item in lotGroups)
                    {
                        rowGroup = this.m_LotSet.Tables["LotDT"].NewRow();
                        rowGroup["LotCode"] = item.LotCode;
                        rowGroup["MoCode"] = item.MOCode;
                        rowGroup["LotQty"] = item.LotQty;
                        rowGroup["GoodQty"] = item.GoodQty;
                        rowGroup["NGQty"] = item.NGQty;
                        rowGroup["ItemCode"] = item.ItemCode;
                        rowGroup["ProductStatus"] = MutiLanguages.ParserString(item.ProductStatus);
                        rowGroup["CollectStatus"] = MutiLanguages.ParserString(item.CollectStatus);
                        rowGroup["MUser"] = item.MaintainUser;
                        rowGroup["BeginDate"] = FormatHelper.ToDateString(item.BeginDate);
                        rowGroup["BeginTime"] = FormatHelper.ToTimeString(item.BeginTime);
                        rowGroup["EndDate"] = FormatHelper.ToDateString(item.EndDate);
                        rowGroup["EndTime"] = FormatHelper.ToTimeString(item.EndTime);

                        this.m_LotSet.Tables["LotDT"].Rows.Add(rowGroup);
                    }

                }

                this.m_LotSet.Tables["LotDT"].AcceptChanges();
                this.m_LotSet.AcceptChanges();
                this.ultraGridMain.DataSource = this.m_LotSet;
                this.ultraGridMain.UpdateData();
            }
            catch (Exception ex)
            {
            }
        }

        private void ClearLotSimulationList()
        {
            if (this.m_LotSet == null)
            {
                return;
            }
            this.m_LotSet.Tables["LotDT"].Rows.Clear();
            this.m_LotSet.Tables["LotDT"].AcceptChanges();

            this.m_LotSet.AcceptChanges();
        }
        #endregion

        #region �¼�

        private void FCollectionMetrial_Activated(object sender, System.EventArgs e)
        {
            edtInput.TextFocus(false, true);
        }

        private void FCollectionMetrial_Deactivate(object sender, System.EventArgs e)
        {
            ApplicationRun.GetQtyForm().Hide();
        }

        private void FCollectionMetrial_Load(object sender, System.EventArgs e)
        {
            this._FunctionName = this.Text;

            _MaterialFacade = new MaterialFacade(this.DataProvider);
            _DataCollectFacade = new DataCollectFacade(this.DataProvider);
            _MOLotFacade = new MOLotFacade(this.DataProvider);

            this.InitPageLanguage();

        }

        private void FCollectionMetrial_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DataProvider != null)
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection();

            ApplicationRun.GetQtyForm().Hide();
        }

        private void INNO_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                ClearVariables();

                if (this.edtMO.Checked)
                {
                    MOFacade moFacade = new MOFacade(this._domainDataProvider);
                    object objmo = moFacade.GetMO(this.edtMO.Value.Trim().ToString());
                    if (objmo != null)
                    {
                        //�����Ѳɼ�����������
                        LoadLotSimulationList(this.edtMO.Value.Trim().ToString(), ApplicationService.Current().ResourceCode);

                        this.txtItemCode.Value = ((MO)objmo).ItemCode.Trim().ToString();
                        ItemFacade itemFacede = new ItemFacade(this._domainDataProvider);
                        object objitem = itemFacede.GetItem(((MO)objmo).ItemCode.Trim().ToString(), GlobalVariables.CurrentOrganizations.First().OrganizationID);

                        if (objitem == null)
                        {
                            ucMessageInfo.AddEx(this._FunctionName, "$CS_MO: " + this.edtMO.Value, new UserControl.Message(MessageType.Error, "$Error_ItemCode_NotExist $Domain_ItemCode=" + ((MO)objmo).ItemCode.Trim().ToString()), true);
                            this.edtMO.TextFocus(false, true);
                            return;
                        }

                        Item item = objitem as Item;
                        this.labelItemCodeDesc.Text = item.ItemDescription;
                        ItemLotFacade itemLotFacade = new ItemLotFacade(this._domainDataProvider);
                        object item2Lotcheck = itemLotFacade.GetItem2LotCheck(item.ItemCode);
                        if (item2Lotcheck == null)
                        {
                            // Item2SNCheck not exist
                            ucMessageInfo.AddEx(this._FunctionName, "$CS_MO: " + this.edtMO.Value, new UserControl.Message(MessageType.Error, "$Error_NoItemSNCheckInfo $Domain_ItemCode=" + item.ItemCode), true);
                            this.txtLotLetter.Value = "";
                            this.txtLotLetter.Checked = false;
                            this.txtLotLen.Checked = false;
                            this.txtLotLen.Value = "";
                            this.edtMO.TextFocus(false, true);
                            return;
                        }

                        Item2LotCheck item2LOTCheck = item2Lotcheck as Item2LotCheck;
                        SystemSettingFacade ssf = new SystemSettingFacade(this.DataProvider);
                        object para = ssf.GetParameter("PRODUCTCODECONTROLSTATUS", "PRODUCTCODECONTROLSTATUS");
                        if (item2LOTCheck.SNPrefix.Length != 0)
                        {
                            this.txtLotLetter.Checked = true;
                            this.txtLotLetter.Value = item2LOTCheck.SNPrefix;
                            if (para != null)
                            {
                                if (string.Compare(((Parameter)para).ParameterAlias, "1", true) == 0)
                                {
                                    this.txtLotLetter.Enabled = false;
                                }
                                else
                                {
                                    this.txtLotLetter.Enabled = true;
                                }
                            }
                            else
                            {
                                this.txtLotLetter.Enabled = true;
                            }
                        }
                        else
                        {
                            this.txtLotLetter.Enabled = true;
                        }

                        if (item2LOTCheck.SNLength != 0)
                        {
                            this.txtLotLen.Checked = true;
                            this.txtLotLen.Value = item2LOTCheck.SNLength.ToString();
                            if (para != null)
                            {
                                if (string.Compare(((Parameter)para).ParameterAlias, "1", true) == 0)
                                {
                                    this.txtLotLen.Enabled = false;
                                }
                                else
                                {
                                    this.txtLotLen.Enabled = true;
                                }
                            }
                            else
                            {
                                this.txtLotLen.Enabled = true;
                            }
                        }
                        else
                        {
                            this.txtLotLen.Enabled = true;
                        }
                    }
                    else
                    {
                        ucMessageInfo.AddEx(this._FunctionName, "$CS_MO: " + this.edtMO.Value, new UserControl.Message(MessageType.Error, "$CS_MO_NOT_EXIST"), true);
                        this.edtMO.TextFocus(false, true);
                        return;
                    }

                    this.edtMO.InnerTextBox.Enabled = false;
                    # region ��ʼ��ʾ��Ϣ
                    ucMessageInfo.AddEx(">>$CS_PleaseInputSimLot");
                    this.edtInput.TextFocus(false, true);
                    #endregion

                }
            }
        }

        private void edtMO_CheckBoxCheckedChanged(object sender, System.EventArgs e)
        {
            if (edtMO.Checked == false)
            {
                this.txtLotLen.Value = String.Empty;
                this.txtLotLetter.Value = String.Empty;
                this.labelItemCodeDesc.Text = string.Empty;
                this.txtItemCode.Value = string.Empty;

                this.txtLotLen.Checked = false;
                this.txtLotLetter.Checked = false;
                this.txtLotLen.Enabled = false;
                this.txtLotLetter.Enabled = false;
            }
            if (edtMO.Checked == true)
            {
                this.txtLotLen.Enabled = true;
                this.txtLotLetter.Enabled = true;
            }
        }

        private void ucMessageInfo_WorkingErrorAdded(object sender, WorkingErrorAddedEventArgs e)
        {
            CSHelper.ucMessageWorkingErrorAdded(e, this.DataProvider);
        }

        public void edtInput_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (this.edtInput.Value.Trim() == string.Empty)
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$Error_Date_Empty", new UserControl.Message(MessageType.Normal, "$Error_Date_Empty"), true);
                    this.edtInput.TextFocus(false, true);
                    return;
                }
                if (this.edtMO.Checked == true && this.edtMO.InnerTextBox.Enabled)
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_MO: " + this.edtMO.Value, new UserControl.Message(MessageType.Normal, "$CS_PleasePressEnterOnGOMO"), true);
                    this.edtMO.TextFocus(false, true);
                }
                else
                {
                    ucMessageInfo.AddWithoutEnter("<<");
                    ucMessageInfo.AddBoldText(edtInput.Value.Trim());
                    Collect();

                }
            }
        }

        #endregion


        #region �ж����������Ƿ�����tblmo2lotlink

        private bool ISFromMo2LotLink(string mocode, string lotCode)
        {
            object obj = _MOLotFacade.GetMO2LotLink(lotCode, mocode);
            if (obj == null)
            {
                ucMessageInfo.AddEx(this._FunctionName, "$Error_Date_Empty", new UserControl.Message(MessageType.Normal, "$Error_Date_Empty"), true);
                return false;
            }
            else
            {
                ucMessageInfo.AddEx(this._FunctionName, "$Error_Date_Empty", new UserControl.Message(MessageType.Normal, "$Error_Date_Empty"), true);
                return true;
            }

        }

        #endregion

        #region ������
        private bool GetOPBOMMINNO()
        {
            Messages msgMo = new Messages();

            try
            {
                //Ϊ��������               
                ((SQLDomainDataProvider)this.DataProvider).PersistBroker.AutoCloseConnection = false;
                ((SQLDomainDataProvider)this.DataProvider).PersistBroker.OpenConnection();

                string strMoCode = edtMO.Value.Trim().ToUpper();
                ProductInfo product = null;

                if (strMoCode == "")
                {
                    #region �����������
                    // Added by Icyer 2007/03/16		��Ϊ���ж��Ƿ���Ҫ�����������Ӵ������Ĺ�����ȡ��Ʒ���룻��������������Ŵ�Simulationȡ
                    ActionGoToMO actionGoMO = new ActionGoToMO(this.DataProvider);
                    msgMo = actionGoMO.GetItemCodeFromGoMoRCard(ApplicationService.Current().ResourceCode, _lotCode);
                    if (msgMo.IsSuccess() == false)		// ����д��󣬱�ʾ��Ҫ�������������ǽ����������ѯ������������
                    {
                        ucMessageInfo.AddEx(this._FunctionName, "$CS_RCARD: " + this.edtInput.Value, msgMo, true);
                        return false;
                    }
                    else	// ���سɹ����������������Ҫ�����������ҷ�����ȷ�Ĺ�����Ϣ������Ҫ��������
                    {
                        UserControl.Message msgMoData = msgMo.GetData();
                        if (msgMoData != null)		// ��DATA���ݣ���ʾ��Ҫ��������
                        {
                            MO mo = (MO)msgMoData.Values[0];
                            if (mo != null)
                                strMoCode = mo.MOCode;
                        }
                        else		// ���û��DATA���ݣ���ʾ����Ҫ�����������������ǰ�Ĵ��룺�����к��Ҳ�Ʒ
                        {
                            ActionOnLineHelper onLine = new ActionOnLineHelper(this.DataProvider);
                            Messages messages1 = onLine.GetIDInfo(_lotCode);
                            if (messages1.IsSuccess())
                            {
                                product = (ProductInfo)messages1.GetData().Values[0];
                                productInfo = product;
                                if (product.LastSimulation != null)
                                {
                                    strMoCode = product.LastSimulation.MOCode;
                                    product.NowSimulation = product.LastSimulation;
                                    product.NowSimulation.ResCode = ApplicationService.Current().ResourceCode;
                                    product.NowSimulation.OPCode = baseModelFacade.GetOperationByResource(ApplicationService.Current().ResourceCode).OPCode;
                                }
                                else if (productInfo.LastSimulation == null)
                                {
                                    ucMessageInfo.AddEx(this._FunctionName, "$CS_RCARD: " + this.edtInput.Value, new UserControl.Message(MessageType.Error, "$NoSimulation"), true);

                                    return false;
                                }
                            }
                        }
                    }
                    // Added end
                    #endregion
                }
                else
                {
                    ActionOnLineHelper onLine = new ActionOnLineHelper(this.DataProvider);
                    #region ����;�̣�Ȼ����OPBOM

                    MO mo;

                    Messages messages1 = onLine.GetIDInfoByMoCodeAndId(strMoCode, _lotCode);
                    if (messages1.IsSuccess())
                    {
                        product = (ProductInfo)messages1.GetData().Values[0];
                        if (edtMO.Checked)
                        {
                            ActionGoToMO goToMO = new ActionGoToMO(this.DataProvider);
                            //AMOI  MARK  START  20050803  �ظ�������ͬһ����ʱ���������
                            GoToMOActionEventArgs MOActionEventArgs = new GoToMOActionEventArgs(ActionType.DataCollectAction_GoMO, _lotCode, ApplicationService.Current().UserCode,
                                ApplicationService.Current().ResourceCode, product, edtMO.Value.Trim());
                            messages1 = goToMO.CheckIn(MOActionEventArgs);
                            if (!MOActionEventArgs.PassCheck)
                            {
                                messages1 = onLine.CheckID(new CINNOActionEventArgs(ActionType.DataCollectAction_CollectINNO, _lotCode, ApplicationService.Current().UserCode,
                                    ApplicationService.Current().ResourceCode, product));
                            }
                            //AMOI  MARK  END
                        }
                        else
                        {
                            // Added by Icyer 2007/03/16	���������������������������飬��������������;�̼��
                            if (msgMo.GetData() != null)	// ��Ҫ�����������������������
                            {
                                UserControl.Message msgMoData = msgMo.GetData();
                                mo = (MO)msgMoData.Values[0];
                                ActionGoToMO goToMO = new ActionGoToMO(this.DataProvider);
                                GoToMOActionEventArgs MOActionEventArgs = new GoToMOActionEventArgs(ActionType.DataCollectAction_GoMO, _lotCode, ApplicationService.Current().UserCode,
                                    ApplicationService.Current().ResourceCode, product, mo.MOCode);
                                messages1 = goToMO.CheckIn(MOActionEventArgs);
                                if (!MOActionEventArgs.PassCheck)
                                {
                                    messages1 = onLine.CheckID(new CINNOActionEventArgs(ActionType.DataCollectAction_CollectINNO, _lotCode, ApplicationService.Current().UserCode,
                                        ApplicationService.Current().ResourceCode, product));
                                }
                            }
                            else	// ����Ҫ����������������к�;��
                            {
                                messages1 = onLine.CheckID(new CINNOActionEventArgs(ActionType.DataCollectAction_CollectINNO, _lotCode, ApplicationService.Current().UserCode,
                                    ApplicationService.Current().ResourceCode, product));
                            }
                            if (messages1.IsSuccess() == false)
                            {
                                ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, messages1, true);
                                return false;
                            }
                            // Added end
                        }
                        if (messages1.IsSuccess())
                        {
                            //product=(ProductInfo)messages1.GetData().Values[0];
                            mo = (MO)moFacade.GetMO(product.NowSimulation.MOCode);
                        }
                        else
                        {
                            //Add By Bernard @ 2010-11-02
                            Messages messages2 = new Messages();
                            string exception = messages1.OutPut();
                            exception = exception.Replace(_lotCode.Trim().ToUpper(), this.edtInput.Value.Trim().ToUpper());
                            messages2.Add(new UserControl.Message(MessageType.Error, exception));
                            //end

                            ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, messages1, true);
                            return false;
                        }
                    }
                    #endregion
                }

                #region �ҵ���������Ϣ
                MO moNew = (MO)moFacade.GetMO(strMoCode);
                OPBOMFacade opBOMFacade = new OPBOMFacade(this._domainDataProvider);
                MaterialFacade materialFacade = new MaterialFacade(this._domainDataProvider);

                _OPBOMDetailList = opBOMFacade.QueryOPBOMDetail(product.NowSimulation.ItemCode, string.Empty, string.Empty, moNew.BOMVersion,
                    product.NowSimulation.RouteCode, product.NowSimulation.OPCode, (int)MaterialType.CollectMaterial,
                    int.MinValue, int.MaxValue, moNew.OrganizationID, true);



                if (_OPBOMDetailList == null || _OPBOMDetailList.Length <= 0)
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(MessageType.Error, "$CS_OPBOM_NotFound"), true);
                    ClearVariables();
                    return false;
                }

                _objBomDetailNotFilter = _OPBOMDetailList;
                //���˱�ѡ��
                _OPBOMDetailList = FilterOPBOMDetail(_OPBOMDetailList);

                if (_OPBOMDetailList == null || _OPBOMDetailList.Length <= 0)
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(MessageType.Error, "$CS_OPBOM_NotFound"), true);
                    ClearVariables();
                    return false;
                }

                //��ȡ���ع���������
                //ע�⣺���tblminno�е������Ƿ��ܶ�Ӧ��tblopbomdetail��
                object[] tempMinNo = materialFacade.QueryMINNO_New(strMoCode, product.NowSimulation.RouteCode,
                    product.NowSimulation.OPCode, ApplicationService.Current().ResourceCode, moNew.BOMVersion);
                if (tempMinNo == null)
                {
                    _PreparedLotPartList = null;
                }
                else
                {
                    ArrayList minnoList = new ArrayList();
                    foreach (MINNO minno in tempMinNo)
                    {
                        bool found = false;

                        if (_OPBOMDetailList != null)
                        {
                            foreach (OPBOMDetail opBOMDetail in _objBomDetailNotFilter)
                            {
                                if (minno.MSourceItemCode == opBOMDetail.OPBOMSourceItemCode
                                    && opBOMDetail.OPBOMItemControlType == BOMItemControlType.ITEM_CONTROL_LOT
                                    && minno.MItemCode == opBOMDetail.OPBOMItemCode)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found)
                            minnoList.Add(minno);
                    }
                    _PreparedLotPartList = minnoList.ToArray();
                }

                if (_OPBOMDetailList != null && _OPBOMDetailList.Length > 0)
                {
                    this.GetOPBOMDetailCount();
                }

                int _OPBOMDetailLotPartCount = -1;

                if (_PreparedLotPartList != null && _PreparedLotPartList.Length > 0)
                {
                    _OPBOMDetailLotPartCount = _PreparedLotPartList.Length;
                }
                else
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(MessageType.Error, "$CS_NOOPBomInfo "), true);
                    ClearVariables();
                    return false;
                }

                if (_OPBOMDetailList.Length > _OPBOMDetailLotPartCount)
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(MessageType.Error, "$CS_LotControl_notFull"), true);
                    ClearVariables();
                    return false;
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(ex), true);
                return false;
            }
            finally
            {
                ((SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection();
                ((SQLDomainDataProvider)this.DataProvider).PersistBroker.AutoCloseConnection = true;
            }
        }
        #endregion

        //��ȡArrayList��Count
        private void GetOPBOMDetailCount()
        {
            if (_OPBOMDetailList != null)
            {
                for (int i = 0; i < _OPBOMDetailList.Length; i++)
                {
                    if (((OPBOMDetail)_OPBOMDetailList[i]).OPBOMItemControlType == "item_control_lot")
                    {
                        _OPBOMDetailArrayList.Add(_OPBOMDetailList[i]);
                    }
                    else
                    {
                        int number = Convert.ToInt32(((OPBOMDetail)_OPBOMDetailList[i]).OPBOMItemQty);
                        for (int j = 0; j < number; j++)
                        {
                            _OPBOMDetailArrayList.Add(_OPBOMDetailList[i]);
                        }
                    }
                }

            }


            if (_PreparedLotPartList != null)
            {
                for (int i = 0; i < _PreparedLotPartList.Length; i++)
                {
                    int number = Convert.ToInt32(((MINNO)_PreparedLotPartList[i]).Qty);
                    for (int j = 0; j < number; j++)
                    {
                        _PreparedLotPartArrayList.Add(_PreparedLotPartList[i]);
                    }
                }
            }
        }

        //�ɼ�
        private void Collect()
        {
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);

            UserControl.Messages messages = new UserControl.Messages();

            string data = Web.Helper.FormatHelper.CleanString(edtInput.Value.ToUpper().Trim());

            try
            {
                #region �������������

                _lotCode = this.edtInput.Value.Trim();
                #region ����������

                if (this.edtMO.Checked && this.edtMO.InnerTextBox.Enabled)
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_MO: " + this.edtMO.Value, new UserControl.Message(MessageType.Error, ">>$CS_PleasePressEnterOnGOMO"), true);
                    this.edtMO.Checked = true;
                    this.edtMO.TextFocus(false, true);
                    return;
                }

                //����Ʒ���кŸ�ʽ
                bool lenCheckBool = true;
                //��Ʒ���кų��ȼ��
                if (txtLotLen.Checked && txtLotLen.Value.Trim() != string.Empty)
                {
                    int len = 0;
                    try
                    {
                        len = int.Parse(txtLotLen.Value.Trim());
                        if (data.Trim().Length != len)
                        {
                            lenCheckBool = false;
                            ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(MessageType.Error, "$CS_LOT_NO_LEN_CHECK_FAIL"), true);
                            ucMessageInfo.AddEx(">>$CS_PleaseInputSimLot");
                            edtInput.TextFocus(false, true);
                            return;
                        }
                    }
                    catch
                    {
                        edtInput.TextFocus(false, true);
                        return;
                    }
                }

                //��Ʒ���к����ַ����
                if (txtLotLetter.Checked && txtLotLetter.Value.Trim() != string.Empty)
                {
                    // Changed by Icyer 2006/11/13
                    int index = -1;
                    if (txtLotLetter.Value.Trim().ToUpper().Length <= data.Length)
                    {
                        index = data.IndexOf(txtLotLetter.Value.Trim().ToUpper());
                    }
                    // Changed end
                    if (index != 0)
                    {
                        lenCheckBool = false;
                        ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(MessageType.Error, "$CS_LOT_NO_FCHAR_CHECK_FAIL"), true);
                        ucMessageInfo.AddEx(">>$CS_PleaseInputSimLot");
                        edtInput.TextFocus(false, true);
                        return;
                    }
                }


                if (!this.SNConttentCheck(this.txtItemCode.Value.Trim().ToString(), this.edtInput.Value.Trim().ToString()))
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(MessageType.Error, "$CS_SNContent_CheckWrong $CS_Param_RunSeq:" + this.edtInput.Value.Trim().ToString()), true);
                    ucMessageInfo.AddEx(">>$CS_PleaseInputSimLot");
                    edtInput.TextFocus(false, true);
                    return;
                }


                #endregion

                if (!GetOPBOMMINNO())
                {
                    edtInput.TextFocus(true, true);
                    return;
                }
                //�ж��Ƿ�Ӧ���ɼ�
                if (IsCollectByRes())
                {
                    SaveEnd();
                }
                else if (AutoCollectAllLotPart())
                {
                    Save();
                    return;
                }
                else
                {
                    ClearVariables();
                    return;
                }

                #endregion
            }
            catch (Exception e)
            {
                messages.Add(new UserControl.Message(e));

                ClearVariables();
            }

            ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, messages, true);

            edtInput.TextFocus(true, true);
        }

        //�ж��Ƿ��Ѿ��ɼ�
        private bool IsCollectByRes()
        {
            LotSimulation simulation = _DataCollectFacade.GetLotSimulation(this.edtInput.Value.Trim().ToUpper()) as LotSimulation;
            if (simulation != null)
            {
                object objOP = baseModelFacade.GetOperationByResource(ApplicationService.Current().ResourceCode);

                if (simulation.OPCode == (objOP as Operation2Resource).OPCode)
                {
                    if (simulation.CollectStatus == CollectStatus.CollectStatus_BEGIN)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        //�Զ��ɼ�����Lot����
        private bool AutoCollectAllLotPart()
        {
            if (_PreparedLotPartList != null)
            {
                for (int i = 0; i < _PreparedLotPartList.Length; i++)
                {
                    try
                    {

                        string moCode = ((MINNO)_PreparedLotPartList[i]).MOCode;
                        string mItemCode = ((MINNO)_PreparedLotPartList[i]).MItemCode;
                        string barcode = ((MINNO)_PreparedLotPartList[i]).MItemPackedNo;

                        object opBomDetailNew = _objBomDetailNotFilter[0];
                        if (_objBomDetailNotFilter != null && _objBomDetailNotFilter.Length > 0)
                        {
                            for (int j = 0; j < _objBomDetailNotFilter.Length; j++)
                            {
                                if (((OPBOMDetail)_objBomDetailNotFilter[j]).OPBOMItemCode == mItemCode
                                    && ((OPBOMDetail)_objBomDetailNotFilter[j]).OPBOMSourceItemCode == ((MINNO)_PreparedLotPartList[i]).MSourceItemCode)
                                {
                                    opBomDetailNew = _objBomDetailNotFilter[j];
                                    break;
                                }
                            }
                        }

                        if (!CollectPart((OPBOMDetail)opBomDetailNew, barcode))
                        {
                            return false;
                        }

                    }
                    catch (Exception e)
                    {
                        ucMessageInfo.AddEx(this._FunctionName, "$CS_RCARD: " + this.edtInput.Value, new UserControl.Message(e), true);
                    }
                }
            }

            return true;
        }

        //�ɼ��ֹ����������
        private bool CollectPart(OPBOMDetail opBOMDetail, string materialSerialNo)
        {
            bool returnValue = false;
            Messages msg = new Messages();

            try
            {
                //��ȡMOCode
                string moCode = this.edtMO.Value.ToString().Trim();
                if (moCode.Length <= 0)
                {
                    LotSimulation sim = (LotSimulation)_DataCollectFacade.GetLotSimulation(_lotCode);
                    if (sim != null)
                    {
                        moCode = sim.MOCode;
                    }
                }

                //�������MINNO
                MINNO newMINNO = _MaterialFacade.CreateNewMINNO();
                msg = _DataCollectFacade.GetMINNOByBarcode(opBOMDetail, materialSerialNo, moCode, _InputPartList, true, false, out newMINNO);

                if (msg.IsSuccess())
                {
                    if (newMINNO != null)
                    {
                        _InputPartList.Add((object)newMINNO);
                    }

                    ++_CollectedCount;
                    returnValue = true;
                }
                else
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_MCARD: " + materialSerialNo, msg, true);
                }

            }
            catch (Exception e)
            {
                ucMessageInfo.AddEx(this._FunctionName, "$CS_MCARD: " + materialSerialNo, new UserControl.Message(e), true);
            }

            return returnValue;
        }

        private void Save()
        {
            //add by hiro 2008/12/04 check try planqty >actualqty
            //end 
            ActionOnLineHelper onLine = new ActionOnLineHelper(this.DataProvider);
            //Laws Lu,2005/10/19,����	������������
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.AutoCloseConnection = false;
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.BeginTransaction();
            // Added by Icyer 2005/10/28
            if (Resource == null)
            {
                BenQGuru.eMES.BaseSetting.BaseModelFacade dataModel = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider);
                Resource = (Domain.BaseSetting.Resource)dataModel.GetResource(ApplicationService.Current().ResourceCode);
            }
            // Added end
            try
            {
                ExtendSimulation lastSimulation = null;
                Messages _Messages = new Messages();
                ActionCheckStatus actionCheckStatus = new ActionCheckStatus();
                string strMoCode = edtMO.Value.Trim().ToUpper();
                ProductInfo currentProduct = null;
                if (strMoCode == "")
                {
                    // Added by Icyer 2007/03/17		��Ϊ���ж��Ƿ���Ҫ�����������Ӵ������Ĺ�����ȡ��Ʒ���룻��������������Ŵ�Simulationȡ
                    ActionGoToMO actionGoMO = new ActionGoToMO(this.DataProvider);
                    Messages msgMo = actionGoMO.GetItemCodeFromGoMoRCard(ApplicationService.Current().ResourceCode, _lotCode);
                    if (msgMo.IsSuccess() == false)		// ����д��󣬱�ʾ��Ҫ�������������ǽ����������ѯ������������
                    {
                    }
                    else	// ���سɹ����������������Ҫ�����������ҷ�����ȷ�Ĺ�����Ϣ������Ҫ��������
                    {
                        UserControl.Message msgMoData = msgMo.GetData();
                        if (msgMoData != null)		// ��DATA���ݣ���ʾ��Ҫ��������
                        {
                            MO mo = (MO)msgMoData.Values[0];
                            strMoCode = mo.MOCode;
                        }
                        else		// ���û��DATA���ݣ���ʾ����Ҫ�����������������ǰ�Ĵ��룺�����к��Ҳ�Ʒ
                        {
                            Messages messages1 = new Messages();
                            messages1 = onLine.GetIDInfo(_lotCode);
                            if (messages1.IsSuccess())
                            {
                                currentProduct = (ProductInfo)messages1.GetData().Values[0];
                                if (currentProduct.LastSimulation != null)
                                {
                                    strMoCode = currentProduct.LastSimulation.MOCode;
                                }
                                lastSimulation = currentProduct.LastSimulation;
                                currentProduct.Resource = Resource;
                            }
                        }
                    }
                    // Added end
                }
                if (strMoCode != "")
                {
                    if (listActionCheckStatus.ContainsKey(strMoCode))
                    {
                        actionCheckStatus = (ActionCheckStatus)listActionCheckStatus[strMoCode];
                        actionCheckStatus.ProductInfo = currentProduct;
                        actionCheckStatus.ActionList = new ArrayList();
                    }
                    else
                    {
                        actionCheckStatus.NeedUpdateSimulation = false;
                        actionCheckStatus.NeedFillReport = false;
                        listActionCheckStatus.Add(strMoCode, actionCheckStatus);
                    }
                }
                #region ��������
                if (edtMO.Checked)
                {
                    // Changed by Icyer 2005/10/18
                    Messages messages1 = new Messages();
                    if (actionCheckStatus.ProductInfo == null)
                    {
                        messages1 = onLine.GetIDInfo(_lotCode);
                        actionCheckStatus.ProductInfo = (ProductInfo)messages1.GetData().Values[0];
                        actionCheckStatus.ProductInfo.Resource = Resource;
                        lastSimulation = actionCheckStatus.ProductInfo.LastSimulation;
                        _Messages.AddMessages(messages1);
                    }
                    // Changed end
                    if (_Messages.IsSuccess())
                    {
                        // Changed by Icyer 2005/10/18
                        ProductInfo product = actionCheckStatus.ProductInfo;
                        // Changed end
                        messages1.AddMessages(onLine.ActionWithTransaction(new GoToMOActionEventArgs(ActionType.DataCollectAction_GoMO, _lotCode,
                            ApplicationService.Current().UserCode, ApplicationService.Current().ResourceCode,
                            product, edtMO.Value.Trim()), actionCheckStatus));
                    }
                    if (messages1.IsSuccess())
                        messages1.Add(new UserControl.Message(MessageType.Success, "$CS_GOMO_CollectSuccess"));
                    else
                    {
                        ClearVariables();
                    }
                    _Messages.AddMessages(messages1);
                }

                BenQGuru.eMES.Material.WarehouseFacade wfacade = null;
                if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                {
                    wfacade = new WarehouseFacade(this.DataProvider);
                }

                //karron qiu
                //���������ϲɼ��������Ʒ/����Ʒ�������Ӽ������ܣ����û��ɹ��ɼ�һ����Ʒ���кź�
                //��������һ��ͬʱ�ü������������û����㡣ÿ�δ򿪽���ʱ�����������������㣬ϵͳ����Ҫ���������ֵ
                //bool flag = true;//��ʾ�Ƿ�ɼ��ɹ�
                if (true)
                {
                    Messages messages1 = new Messages();
                    if (actionCheckStatus.ProductInfo == null)
                    {
                        messages1 = onLine.GetIDInfo(_lotCode);
                        actionCheckStatus.ProductInfo = (ProductInfo)messages1.GetData().Values[0];
                        actionCheckStatus.ProductInfo.Resource = Resource;
                        lastSimulation = actionCheckStatus.ProductInfo.LastSimulation;
                        _Messages.AddMessages(messages1);
                    }
                    else	//����һ��Action��NowSimulation����Ϊ��Action��LastSimulation
                    {
                        if (actionCheckStatus.ActionList.Count > 0)
                        {
                            actionCheckStatus.ProductInfo = new ProductInfo();
                            actionCheckStatus.ProductInfo.NowSimulation = new LotSimulation();
                            actionCheckStatus.ProductInfo.Resource = Resource;
                            actionCheckStatus.ProductInfo.LastSimulation =
                                new ExtendSimulation(((ActionEventArgs)actionCheckStatus.ActionList[actionCheckStatus.ActionList.Count - 1]).ProductInfo.NowSimulation);

                        }
                    }

                    // Changed end
                    if (_Messages.IsSuccess())
                    {
                        if (messages1.IsSuccess())
                        {
                            // Changed by Icyer 2005/10/18
                            ProductInfo product = actionCheckStatus.ProductInfo;
                            // Changed end
                            object[] objBomDetailLot = new object[_InputPartList.Count];
                            _InputPartList.CopyTo(objBomDetailLot);
                            if (objBomDetailLot != null)
                            {
                                messages1.AddMessages(onLine.ActionWithTransaction(new CINNOActionEventArgs(ActionType.DataCollectAction_CollectINNO, _lotCode,
                                    ApplicationService.Current().UserCode, ApplicationService.Current().ResourceCode,
                                    product, "", wfacade
                                    ), actionCheckStatus, objBomDetailLot));

                            }
                        }
                    }
                    if (messages1.IsSuccess())
                        messages1.Add(new UserControl.Message(MessageType.Success, "$CS_LotCollectBegin"));

                    _Messages.AddMessages(messages1);

                }
                //���ʹ��Material���֣���ִ�л����SQL
                if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                {
                    if (wfacade != null)
                        wfacade.ExecCacheSQL();
                }
                //AMOI  MARK  START  20050803 ���OP�趨���в��Թ��������������,Ϊ��������;�̣�ֱ����GOOD�ɼ����������OP�趨���������
                {
                    Messages messages2 = new Messages();
                    if (actionCheckStatus.ProductInfo == null)
                    {
                        messages2 = onLine.GetIDInfo(_lotCode);
                        actionCheckStatus.ProductInfo = (ProductInfo)messages2.GetData().Values[0];
                        actionCheckStatus.ProductInfo.Resource = Resource;
                        lastSimulation = actionCheckStatus.ProductInfo.LastSimulation;
                        _Messages.AddMessages(messages2);
                    }
                    else	//����һ��Action��NowSimulation����Ϊ��Action��LastSimulation
                    {
                        if (actionCheckStatus.ActionList.Count > 0)
                        {
                            actionCheckStatus.ProductInfo = new ProductInfo();
                            actionCheckStatus.ProductInfo.NowSimulation = new LotSimulation();
                            actionCheckStatus.ProductInfo.Resource = Resource;
                            actionCheckStatus.ProductInfo.LastSimulation =
                                new ExtendSimulation(((ActionEventArgs)actionCheckStatus.ActionList[actionCheckStatus.ActionList.Count - 1]).ProductInfo.NowSimulation);
                        }
                    }
                    // Changed end
                    if (_Messages.IsSuccess())
                    {
                        // Changed by Icyer 2005/10/18
                        ProductInfo product = actionCheckStatus.ProductInfo;
                        // Changed end

                        messages2.AddMessages(onLine.ActionWithTransaction(new ActionEventArgs(ActionType.DataCollectAction_GOOD, _lotCode,
                            ApplicationService.Current().UserCode, ApplicationService.Current().ResourceCode,
                            product), actionCheckStatus));
                    }
                    if (_Messages.IsSuccess())
                    {
                        // Added by Icyer 2005/10/31
                        // ����Wip & Simulation
                        ActionEventArgs actionEventArgs;
                        actionEventArgs = (ActionEventArgs)actionCheckStatus.ActionList[actionCheckStatus.ActionList.Count - 1];
                        ExtendSimulation oldLastSimulation = actionEventArgs.ProductInfo.LastSimulation;
                        actionEventArgs.ProductInfo.LastSimulation = lastSimulation;
                        _Messages.AddMessages(onLine.Execute(actionEventArgs, actionCheckStatus, true, false));
                        actionEventArgs.ProductInfo.LastSimulation = oldLastSimulation;

                        //ReportHelper reportCollect = new ReportHelper(this.DataProvider);
                        for (int i = 0; i < actionCheckStatus.ActionList.Count; i++)
                        {
                            actionEventArgs = (ActionEventArgs)actionCheckStatus.ActionList[i];
                            //����WIP
                            if (actionEventArgs.OnWIP != null)
                            {
                                for (int iwip = 0; iwip < actionEventArgs.OnWIP.Count; iwip++)
                                {
                                    if (actionEventArgs.OnWIP[iwip] is LotOnWip)
                                    {
                                        _DataCollectFacade.AddLotOnWip((LotOnWip)actionEventArgs.OnWIP[iwip]);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bool f = false;
                        for (int i = 0; i < messages2.Count(); i++)
                        {
                            UserControl.Message message3 = messages2.Objects(i);
                            if (message3.Type == MessageType.Error)
                            {
                                if (message3.Body.IndexOf("$CS_OP_Not_TestOP") < 0)
                                {
                                    if (message3.Exception != null)
                                        if (message3.Exception.Message.IndexOf("$CS_OP_Not_TestOP") >= 0)
                                            continue;
                                    f = true;
                                }
                            }
                        }
                        _Messages.AddMessages(messages2);
                    }
                }
                //AMOI  MARK  END
                #endregion

                if (_Messages.IsSuccess())
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, _Messages, true);
                    ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CommitTransaction();
                }
                else
                {
                    UserControl.Messages message = new UserControl.Messages();
                    for (int i = 0; i < _Messages.Count(); i++)
                    {
                        if (_Messages.Objects(i).Type == MessageType.Error)
                        {
                            message.Add(_Messages.Objects(i));
                        }
                    }

                    ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, message, true);
                    ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.RollbackTransaction();
                }
            }
            catch (Exception e)
            {
                ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(e), true);
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.RollbackTransaction();
            }
            finally
            {
                //Laws Lu,2007/01/05,����	������������
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.AutoCloseConnection = true;
            }

            //������ʾ���βɼ�������
            object obj = _DataCollectFacade.GetLotSimulation(this.edtInput.Value.Trim().ToUpper());
            if (obj != null)
            {
                LoadLotSimulationList((obj as LotSimulation).MOCode, ApplicationService.Current().ResourceCode);
            }

            ClearVariables();

            ucMessageInfo.AddEx(">>$CS_PleaseInputSimLot");
            this.edtInput.TextFocus(false, true);
        }

        private void SaveEnd()
        {
            ActionOnLineHelper onLine = new ActionOnLineHelper(this.DataProvider);
            //DataProvider.BeginTransaction();
            //Laws Lu,2005/10/19,����	������������
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.AutoCloseConnection = false;
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.BeginTransaction();
            // Added by Icyer 2005/10/28           
            if (Resource == null)
            {
                Resource = (Domain.BaseSetting.Resource)baseModelFacade.GetResource(ApplicationService.Current().ResourceCode);
            }
            // Added end
            try
            {
                ExtendSimulation lastSimulation = null;
                Messages _Messages = new Messages();
                string strMoCode = edtMO.Value.Trim().ToUpper();
                ProductInfo currentProduct = null;

                // Added by Icyer 2007/03/17		��Ϊ���ж��Ƿ���Ҫ�����������Ӵ������Ĺ�����ȡ��Ʒ���룻��������������Ŵ�Simulationȡ
                ActionGoToMO actionGoMO = new ActionGoToMO(this.DataProvider);
                Messages msgMo = actionGoMO.GetItemCodeFromGoMoRCard(ApplicationService.Current().ResourceCode, this.edtInput.Value.Trim().ToUpper());
                if (msgMo.IsSuccess() == false)		// ����д��󣬱�ʾ��Ҫ�������������ǽ����������ѯ������������
                {
                }
                else	// ���سɹ�������Ҫ��������
                {
                    DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

                    //�������ǰ�Ĵ��룺�����������Ҳ�Ʒ
                    Messages messages1 = new Messages();
                    messages1 = onLine.GetIDInfo(this.edtInput.Value.Trim().ToUpper());

                    if (messages1.IsSuccess())
                    {
                        currentProduct = (ProductInfo)messages1.GetData().Values[0];
                        currentProduct.WorkDateTime = dbDateTime;
                        if (currentProduct.LastSimulation != null)
                        {
                            strMoCode = currentProduct.LastSimulation.MOCode;
                            currentProduct.NowSimulation = currentProduct.LastSimulation;

                        }

                        currentProduct.Resource = Resource;
                        object objOP = baseModelFacade.GetOperationByResource(ApplicationService.Current().ResourceCode);

                        if (currentProduct.NowSimulation.OPCode != (objOP as Operation2Resource).OPCode && currentProduct.NowSimulation.CollectStatus == CollectStatus.CollectStatus_BEGIN)
                        {
                            messages1.Add(new UserControl.Message(MessageType.Error, "$CS_LotSimulation_ISCollect"));
                            _Messages.AddMessages(messages1);
                        }
                    }

                }

                //���ʹ��Material���֣���ִ�л����SQL
                BenQGuru.eMES.Material.WarehouseFacade wfacade = null;
                if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                {
                    wfacade = new WarehouseFacade(this.DataProvider);
                    wfacade.ExecCacheSQL();
                }

                //AMOI  MARK  START  20050803 ���OP�趨���в��Թ��������������,Ϊ��������;�̣�ֱ����GOOD�ɼ����������OP�趨���������
                {
                    Messages messages2 = new Messages();
                    // Changed end

                    if (_Messages.IsSuccess())
                    {
                        //����TBLLotSimulation,TblLotsimulationReport,TblLotOnWip , tbllotOnWipItem

                        DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                        LotSimulation nowSimulation = currentProduct.LastSimulation;
                        nowSimulation.CollectStatus = CollectStatus.CollectStatus_END;
                        nowSimulation.EndDate = dbDateTime.DBDate;
                        nowSimulation.EndTime = dbDateTime.DBTime;
                        nowSimulation.MaintainUser = ApplicationService.Current().UserCode;
                        _DataCollectFacade.IsComp(nowSimulation);
                        _DataCollectFacade.UpdateLotSimulation(nowSimulation);

                        LotSimulationReport nowSimulationReport = _DataCollectFacade.GetLastLotSimulationReport(nowSimulation.LotCode);//onLine.FillLotSimulationReport(currentProduct);
                        nowSimulationReport.EndDate = dbDateTime.DBDate;
                        nowSimulationReport.EndTime = dbDateTime.DBTime;

                        ShiftModelFacade shiftModel = new ShiftModelFacade(this.DataProvider);
                        TimePeriod period = (TimePeriod)shiftModel.GetTimePeriod(nowSimulationReport.ShiftTypeCode, nowSimulationReport.EndTime);
                        if (period == null)
                        {
                            throw new Exception("$OutOfPerid");
                        }

                        DateTime dtWorkDateTime = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);
                        if (period.IsOverDate == FormatHelper.TRUE_STRING)
                        {
                            if (period.TimePeriodBeginTime < period.TimePeriodEndTime)
                            {
                                nowSimulationReport.EndShiftDay = FormatHelper.TODateInt(dtWorkDateTime.AddDays(-1));
                            }
                            else if (productInfo.NowSimulation.EndTime < period.TimePeriodBeginTime)
                            {
                                nowSimulationReport.EndShiftDay = FormatHelper.TODateInt(dtWorkDateTime.AddDays(-1));
                            }
                            else
                            {
                                nowSimulationReport.EndShiftDay = nowSimulationReport.EndDate;
                            }
                        }
                        else
                        {
                            nowSimulationReport.EndShiftDay = FormatHelper.TODateInt(dtWorkDateTime);
                        }
                        nowSimulationReport.EndTimePeriodCode = period.TimePeriodCode;
                        nowSimulationReport.EndShiftCode = period.ShiftCode;
                        nowSimulationReport.CollectStatus = CollectStatus.CollectStatus_END;
                        nowSimulationReport.MaintainUser = ApplicationService.Current().UserCode;
                        nowSimulationReport.IsComplete = nowSimulation.IsComplete;
                        nowSimulationReport.EAttribute1 = nowSimulation.EAttribute1;
                        _DataCollectFacade.UpdateLotSimulationReport(nowSimulationReport);


                        object[] objs = _DataCollectFacade.QueryLotOnWip(nowSimulation.MOCode, nowSimulation.LotCode, nowSimulation.OPCode);
                        if (objs != null)
                        {
                            foreach (LotOnWip onwip in objs)
                            {
                                onwip.EndShiftCode = period.ShiftCode;
                                onwip.EndShiftDay = nowSimulationReport.EndShiftDay;
                                onwip.EndTimePeriodCode = period.TimePeriodCode;
                                onwip.CollectStatus = CollectStatus.CollectStatus_END;
                                onwip.EndDate = dbDateTime.DBDate;
                                onwip.EndTime = dbDateTime.DBTime;
                                onwip.MaintainUser = ApplicationService.Current().UserCode;
                                _DataCollectFacade.UpdateLotOnWip(onwip);
                            }
                        }

                        object[] objOnwipItems = _DataCollectFacade.QueryLotOnWIPItem(nowSimulation.LotCode, nowSimulation.MOCode, nowSimulation.OPCode);
                        if (objOnwipItems != null && objOnwipItems.Length > 0)
                        {
                            foreach (LotOnWipItem onwipItem in objOnwipItems)
                            {
                                (onwipItem as LotOnWipItem).EndShiftCode = period.ShiftCode;
                                (onwipItem as LotOnWipItem).EndTimePeriodCode = period.TimePeriodCode;
                                (onwipItem as LotOnWipItem).CollectStatus = CollectStatus.CollectStatus_END;
                                (onwipItem as LotOnWipItem).EndDate = dbDateTime.DBDate;
                                (onwipItem as LotOnWipItem).EndTime = dbDateTime.DBTime;
                                (onwipItem as LotOnWipItem).MaintainUser = ApplicationService.Current().UserCode;
                                _DataCollectFacade.UpdateLotOnWIPItem((onwipItem as LotOnWipItem));
                            }

                        }

                        if (nowSimulation.IsComplete == "1")
                        {

                            MOFacade moFacade = new MOFacade(this.DataProvider);
                            moFacade.UpdateMOACTQTY(nowSimulation.MOCode);

                        }

                        messages2.Add(new UserControl.Message(MessageType.Success, "$CS_LotCollectEnd"));
                        messages2.Add(new UserControl.Message(MessageType.Success, "$CS_GOOD_CollectSuccess"));
                        _Messages.AddMessages(messages2);
                    }
                    else
                    {
                        bool f = false;
                        for (int i = 0; i < messages2.Count(); i++)
                        {
                            UserControl.Message message3 = messages2.Objects(i);
                            if (message3.Type == MessageType.Error)
                            {
                                if (message3.Body.IndexOf("$CS_OP_Not_TestOP") < 0)
                                {
                                    if (message3.Exception != null)
                                        if (message3.Exception.Message.IndexOf("$CS_OP_Not_TestOP") >= 0)
                                            continue;
                                    f = true;
                                }
                            }
                        }
                        _Messages.AddMessages(messages2);
                    }
                }
                //AMOI  MARK  END

                if (_Messages.IsSuccess())
                {
                    ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, _Messages, true);
                    ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CommitTransaction();
                }
                else
                {
                    UserControl.Messages message = new UserControl.Messages();
                    for (int i = 0; i < _Messages.Count(); i++)
                    {
                        if (_Messages.Objects(i).Type == MessageType.Error)
                        {
                            message.Add(_Messages.Objects(i));
                        }
                    }

                    ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, message, true);
                    ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.RollbackTransaction();
                }
            }
            catch (Exception e)
            {
                //DataProvider.RollbackTransaction();
                ucMessageInfo.AddEx(this._FunctionName, "$CS_LotNo: " + this.edtInput.Value, new UserControl.Message(e), true);
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.RollbackTransaction();
            }
            finally
            {
                //Laws Lu,2005/10/19,����	������������
                //				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
                //Laws Lu,2007/01/05,����	������������
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.AutoCloseConnection = true;
            }

            //������ʾ���βɼ�������
            object obj = _DataCollectFacade.GetLotSimulation(this.edtInput.Value.Trim().ToUpper());
            if (obj != null)
            {
                LoadLotSimulationList((obj as LotSimulation).MOCode, (obj as LotSimulation).ResCode);
            }
            ClearVariables();

            ucMessageInfo.AddEx(">>$CS_PleaseInputSimLot");
            this.edtInput.TextFocus(true, true);
        }

        private void ClearVariables()
        {

            _OPBOMDetailArrayList = new ArrayList();

            _PreparedLotPartArrayList = new ArrayList();

            _CollectedCount = 0;
            _CollectIDCount = 0;
            _HaveCollectMaterial = false;
            _lotCode = string.Empty;

            _InputPartList = new ArrayList();
        }



        private void ShowMessageForPartInput(OPBOMDetail opBOMDetail)
        {

            if (opBOMDetail.OPBOMItemControlType == "item_control_lot")
            {
                ucMessageInfo.AddEx(">>$CS_PleaseInputLot>>$CS_Param_Lot=" + opBOMDetail.OPBOMItemCode);
            }
        }

        private object[] FilterOPBOMDetail(object[] bomDetailList)
        {
            ArrayList filterList = new ArrayList();

            for (int i = 0; i < bomDetailList.Length; i++)
            {
                if (((OPBOMDetail)bomDetailList[i]).OPBOMItemControlType != BOMItemControlType.ITEM_CONTROL_LOT)
                {
                    continue;
                }

                bool found = false;
                for (int j = 0; j < filterList.Count; j++)
                {
                    if (((OPBOMDetail)bomDetailList[i]).OPBOMSourceItemCode == ((OPBOMDetail)filterList[j]).OPBOMSourceItemCode)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    filterList.Add(bomDetailList[i]);
                }
            }

            return filterList.ToArray();
        }

        //������к�����Ϊ��ĸ,���ֺͿո�
        private bool SNConttentCheck(string itemCode, string rCard)
        {
            bool returnValue = true;

            ItemFacade itemFacade = new ItemFacade(this.DataProvider);

            string pattern = @"^([A-Za-z0-9]+[ ]*)*[A-Za-z0-9]+$";
            Regex rex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = rex.Match(rCard);

            object obj = itemFacade.GetItem2SNCheck(itemCode, ItemCheckType.ItemCheckType_SERIAL);
            if (obj != null && ((Item2SNCheck)obj).SNContentCheck == "Y" && !match.Success)
            {
                returnValue = false;
            }

            return returnValue;
        }

        private bool IsNumber(object obj)
        {
            try
            {
                int.Parse(obj.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        private int GetCountToCollect()
        {
            int returnValue = 0;

            if (_OPBOMDetailArrayList != null)
            {
                returnValue = _OPBOMDetailArrayList.Count;
            }

            return returnValue;
        }






    }
}
