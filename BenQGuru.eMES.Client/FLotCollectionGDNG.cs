using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using BenQGuru.eMES.LotDataCollect;
using BenQGuru.eMES.Domain.LotDataCollect;
using BenQGuru.eMES.Web.Helper;
using UserControl;
using BenQGuru.eMES.TSModel;
using BenQGuru.eMES.Client.Service;
using BenQGuru.eMES.LotDataCollect.Action;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using BenQGuru.eMES.Domain.TS;


namespace BenQGuru.eMES.Client
{
    /// <summary>
    /// FCollectionGDNG ��ժҪ˵����
    /// Laws Lu,2005/08/10,����ҳ���߼�
    /// Laws Lu,2005/08/16,�޸�	Lucky������
    /// ��������˵��ֻ��������������Ѿ����������������кű��ٴι�����������������Ĺ��������Ƿ�����ȷ�ģ���
    /// �����Ӧ�ù������������к�û�гɹ�����������������ʲôԭ�򣩡�
    /// �ڵ�һ������¿��Լ��������߼���û�����⣻�ڵڶ�����������޷������ģ�������������û�У�
    ///	ϸ�ڵ��߼�������һ�°ɣ��������˵�ĵ�һ�������Ŀǰ���߼�����ȫ����ģ�
    ///	����ǵڶ����������ֻ��Ҫ��֤�������������ɹ����������߼�ȫ��ֹͣ��
    ///	��ʱֻ��Ҫ�����û����ò�Ʒ���к�û�й�������
    /// </summary>
    public class FLotCollectionGDNG : BaseForm
    {
        private const string ng_collect = ActionType.DataCollectAction_NG;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox GroupItemInfo;
        private System.Windows.Forms.Panel panel1;
        public UserControl.UCLabelEdit txtLotCode;
        private System.ComponentModel.IContainer components;
        //private ActionOnLineHelper dataCollect = null;
        private UserControl.UCButton btnExit;
        private UserControl.UCLabelEdit txtMO;
        private UserControl.UCLabelEdit txtItem;
        private System.Windows.Forms.RadioButton rdoGood;
        private System.Windows.Forms.RadioButton rdoNG;
        private UserControl.UCLabelEdit txtMem;
        private UserControl.UCLabelEdit txtGOMO;
        private ProductInfo product;
        private UserControl.UCLabelEdit txtLotLetter;
        private UserControl.UCLabelEdit txtLotLen;
        //Laws Lu,2005/08/16,����	���洦����Ϣ
        private Messages globeMSG = new Messages();
        private double iNG = 0;
        public UCLabelEdit txtGoodQty;
        private UCLabelEdit txtItemDesc;
        private CheckBox checkBoxAutoSaveErrorCode;
        private string _FunctionName = string.Empty;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridMain;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private DataCollectFacade _DataCollectFacade = null;
        private BaseModelFacade dataModel = null;

        private DataTable m_LotDT;
        private DataSet m_LotSet;

        private DataTable m_ErrorCodeDT;
        private DataSet m_ErrorCodeSet;

        private ActionCheckStatus actionCheckStatus = new ActionCheckStatus();
        private UCButton btnSave;
        private Domain.BaseSetting.Resource Resource;
        public UCLabelEdit txtNGQty;
        private CheckBox cbxGoodQty;
        private CheckBox cbxNGQty;
        private UCLabelEdit txtErrorCode;
        private UltraGrid ultraGridErrorCode;
        private bool isCollect;

        public FLotCollectionGDNG()
        {
            //
            // Windows ���������֧���������
            //
            InitializeComponent();

            //
            // TODO: �� InitializeComponent ���ú�����κι��캯������
            //
            UserControl.UIStyleBuilder.FormUI(this);
            product = new ProductInfo();
            txtMem.AutoChange();
        }

        private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
        public IDomainDataProvider DataProvider
        {
            get
            {
                return _domainDataProvider;
            }
        }

        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
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

        #region Windows ������������ɵĴ���
        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLotCollectionGDNG));
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.panelButton = new System.Windows.Forms.Panel();
            this.cbxGoodQty = new System.Windows.Forms.CheckBox();
            this.cbxNGQty = new System.Windows.Forms.CheckBox();
            this.txtNGQty = new UserControl.UCLabelEdit();
            this.txtGoodQty = new UserControl.UCLabelEdit();
            this.txtMem = new UserControl.UCLabelEdit();
            this.txtLotCode = new UserControl.UCLabelEdit();
            this.rdoNG = new System.Windows.Forms.RadioButton();
            this.rdoGood = new System.Windows.Forms.RadioButton();
            this.btnSave = new UserControl.UCButton();
            this.btnExit = new UserControl.UCButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLotLetter = new UserControl.UCLabelEdit();
            this.txtLotLen = new UserControl.UCLabelEdit();
            this.txtGOMO = new UserControl.UCLabelEdit();
            this.GroupItemInfo = new System.Windows.Forms.GroupBox();
            this.txtItemDesc = new UserControl.UCLabelEdit();
            this.txtMO = new UserControl.UCLabelEdit();
            this.txtItem = new UserControl.UCLabelEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ultraGridErrorCode = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.txtErrorCode = new UserControl.UCLabelEdit();
            this.checkBoxAutoSaveErrorCode = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ultraGridMain = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.panelButton.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.GroupItemInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridErrorCode)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel6.TabIndex = 0;
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.cbxGoodQty);
            this.panelButton.Controls.Add(this.cbxNGQty);
            this.panelButton.Controls.Add(this.txtNGQty);
            this.panelButton.Controls.Add(this.txtGoodQty);
            this.panelButton.Controls.Add(this.txtMem);
            this.panelButton.Controls.Add(this.txtLotCode);
            this.panelButton.Controls.Add(this.rdoNG);
            this.panelButton.Controls.Add(this.rdoGood);
            this.panelButton.Controls.Add(this.btnSave);
            this.panelButton.Controls.Add(this.btnExit);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 493);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(923, 98);
            this.panelButton.TabIndex = 155;
            // 
            // cbxGoodQty
            // 
            this.cbxGoodQty.AutoSize = true;
            this.cbxGoodQty.Location = new System.Drawing.Point(248, 70);
            this.cbxGoodQty.Name = "cbxGoodQty";
            this.cbxGoodQty.Size = new System.Drawing.Size(15, 14);
            this.cbxGoodQty.TabIndex = 8;
            this.cbxGoodQty.UseVisualStyleBackColor = true;
            this.cbxGoodQty.Click += new System.EventHandler(this.cbxGoodQty_Click);
            // 
            // cbxNGQty
            // 
            this.cbxNGQty.AutoSize = true;
            this.cbxNGQty.Checked = true;
            this.cbxNGQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxNGQty.Location = new System.Drawing.Point(248, 40);
            this.cbxNGQty.Name = "cbxNGQty";
            this.cbxNGQty.Size = new System.Drawing.Size(15, 14);
            this.cbxNGQty.TabIndex = 8;
            this.cbxNGQty.UseVisualStyleBackColor = true;
            this.cbxNGQty.Click += new System.EventHandler(this.cbxNGQty_Click);
            // 
            // txtNGQty
            // 
            this.txtNGQty.AllowEditOnlyChecked = true;
            this.txtNGQty.AutoSelectAll = false;
            this.txtNGQty.AutoUpper = true;
            this.txtNGQty.Caption = "��������";
            this.txtNGQty.Checked = false;
            this.txtNGQty.EditType = UserControl.EditTypes.Integer;
            this.txtNGQty.Location = new System.Drawing.Point(265, 36);
            this.txtNGQty.MaxLength = 40;
            this.txtNGQty.Multiline = false;
            this.txtNGQty.Name = "txtNGQty";
            this.txtNGQty.PasswordChar = '\0';
            this.txtNGQty.ReadOnly = false;
            this.txtNGQty.ShowCheckBox = false;
            this.txtNGQty.Size = new System.Drawing.Size(194, 24);
            this.txtNGQty.TabIndex = 7;
            this.txtNGQty.TabNext = false;
            this.txtNGQty.Value = "";
            this.txtNGQty.WidthType = UserControl.WidthTypes.Normal;
            this.txtNGQty.XAlign = 326;
            this.txtNGQty.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNGQty_KeyPress);
            // 
            // txtGoodQty
            // 
            this.txtGoodQty.AllowEditOnlyChecked = true;
            this.txtGoodQty.AutoSelectAll = false;
            this.txtGoodQty.AutoUpper = true;
            this.txtGoodQty.Caption = "��Ʒ����";
            this.txtGoodQty.Checked = false;
            this.txtGoodQty.EditType = UserControl.EditTypes.Integer;
            this.txtGoodQty.Location = new System.Drawing.Point(265, 66);
            this.txtGoodQty.MaxLength = 40;
            this.txtGoodQty.Multiline = false;
            this.txtGoodQty.Name = "txtGoodQty";
            this.txtGoodQty.PasswordChar = '\0';
            this.txtGoodQty.ReadOnly = false;
            this.txtGoodQty.ShowCheckBox = false;
            this.txtGoodQty.Size = new System.Drawing.Size(194, 24);
            this.txtGoodQty.TabIndex = 7;
            this.txtGoodQty.TabNext = false;
            this.txtGoodQty.Value = "";
            this.txtGoodQty.WidthType = UserControl.WidthTypes.Normal;
            this.txtGoodQty.XAlign = 326;
            this.txtGoodQty.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGoodQty_KeyPress);
            // 
            // txtMem
            // 
            this.txtMem.AllowEditOnlyChecked = true;
            this.txtMem.AutoSelectAll = false;
            this.txtMem.AutoUpper = true;
            this.txtMem.Caption = "��ע";
            this.txtMem.Checked = false;
            this.txtMem.EditType = UserControl.EditTypes.String;
            this.txtMem.Location = new System.Drawing.Point(545, 6);
            this.txtMem.MaxLength = 80;
            this.txtMem.Multiline = true;
            this.txtMem.Name = "txtMem";
            this.txtMem.PasswordChar = '\0';
            this.txtMem.ReadOnly = false;
            this.txtMem.ShowCheckBox = false;
            this.txtMem.Size = new System.Drawing.Size(237, 51);
            this.txtMem.TabIndex = 3;
            this.txtMem.TabNext = true;
            this.txtMem.Value = "";
            this.txtMem.WidthType = UserControl.WidthTypes.Long;
            this.txtMem.XAlign = 582;
            // 
            // txtLotCode
            // 
            this.txtLotCode.AllowEditOnlyChecked = true;
            this.txtLotCode.AutoSelectAll = false;
            this.txtLotCode.AutoUpper = true;
            this.txtLotCode.Caption = "������������";
            this.txtLotCode.Checked = false;
            this.txtLotCode.EditType = UserControl.EditTypes.String;
            this.txtLotCode.Location = new System.Drawing.Point(240, 6);
            this.txtLotCode.MaxLength = 40;
            this.txtLotCode.Multiline = false;
            this.txtLotCode.Name = "txtLotCode";
            this.txtLotCode.PasswordChar = '\0';
            this.txtLotCode.ReadOnly = false;
            this.txtLotCode.ShowCheckBox = false;
            this.txtLotCode.Size = new System.Drawing.Size(285, 24);
            this.txtLotCode.TabIndex = 2;
            this.txtLotCode.TabNext = false;
            this.txtLotCode.Value = "";
            this.txtLotCode.WidthType = UserControl.WidthTypes.Long;
            this.txtLotCode.XAlign = 325;
            this.txtLotCode.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLotCode_TxtboxKeyPress);
            // 
            // rdoNG
            // 
            this.rdoNG.Font = new System.Drawing.Font("����", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rdoNG.ForeColor = System.Drawing.Color.Red;
            this.rdoNG.Location = new System.Drawing.Point(94, 6);
            this.rdoNG.Name = "rdoNG";
            this.rdoNG.Size = new System.Drawing.Size(96, 24);
            this.rdoNG.TabIndex = 6;
            this.rdoNG.Tag = "1";
            this.rdoNG.Text = "����Ʒ";
            this.rdoNG.CheckedChanged += new System.EventHandler(this.rdoNG_CheckedChanged);
            this.rdoNG.Click += new System.EventHandler(this.rdoNG_Click);
            // 
            // rdoGood
            // 
            this.rdoGood.Checked = true;
            this.rdoGood.Font = new System.Drawing.Font("����", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rdoGood.ForeColor = System.Drawing.Color.Blue;
            this.rdoGood.Location = new System.Drawing.Point(9, 6);
            this.rdoGood.Name = "rdoGood";
            this.rdoGood.Size = new System.Drawing.Size(79, 24);
            this.rdoGood.TabIndex = 5;
            this.rdoGood.TabStop = true;
            this.rdoGood.Tag = "1";
            this.rdoGood.Text = "��Ʒ";
            this.rdoGood.CheckedChanged += new System.EventHandler(this.rdoGood_CheckedChanged);
            this.rdoGood.Click += new System.EventHandler(this.rdoGood_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.ButtonType = UserControl.ButtonTypes.Save;
            this.btnSave.Caption = "����";
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(585, 66);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 22);
            this.btnSave.TabIndex = 5;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.ButtonType = UserControl.ButtonTypes.Exit;
            this.btnExit.Caption = "�˳�";
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Location = new System.Drawing.Point(693, 66);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(88, 22);
            this.btnExit.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtLotLetter);
            this.groupBox2.Controls.Add(this.txtLotLen);
            this.groupBox2.Controls.Add(this.txtGOMO);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(923, 50);
            this.groupBox2.TabIndex = 157;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "�����⹤λָ��";
            // 
            // txtLotLetter
            // 
            this.txtLotLetter.AllowEditOnlyChecked = true;
            this.txtLotLetter.AutoSelectAll = false;
            this.txtLotLetter.AutoUpper = true;
            this.txtLotLetter.Caption = "��Ʒ���к����ַ���";
            this.txtLotLetter.Checked = false;
            this.txtLotLetter.EditType = UserControl.EditTypes.String;
            this.txtLotLetter.Enabled = false;
            this.txtLotLetter.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLotLetter.Location = new System.Drawing.Point(520, 12);
            this.txtLotLetter.MaxLength = 40;
            this.txtLotLetter.Multiline = false;
            this.txtLotLetter.Name = "txtLotLetter";
            this.txtLotLetter.PasswordChar = '\0';
            this.txtLotLetter.ReadOnly = false;
            this.txtLotLetter.ShowCheckBox = true;
            this.txtLotLetter.Size = new System.Drawing.Size(270, 24);
            this.txtLotLetter.TabIndex = 28;
            this.txtLotLetter.TabNext = false;
            this.txtLotLetter.Value = "";
            this.txtLotLetter.WidthType = UserControl.WidthTypes.Normal;
            this.txtLotLetter.XAlign = 657;
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
            this.txtLotLen.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLotLen.Location = new System.Drawing.Point(259, 12);
            this.txtLotLen.MaxLength = 40;
            this.txtLotLen.Multiline = false;
            this.txtLotLen.Name = "txtLotLen";
            this.txtLotLen.PasswordChar = '\0';
            this.txtLotLen.ReadOnly = false;
            this.txtLotLen.ShowCheckBox = true;
            this.txtLotLen.Size = new System.Drawing.Size(246, 24);
            this.txtLotLen.TabIndex = 27;
            this.txtLotLen.TabNext = false;
            this.txtLotLen.Value = "";
            this.txtLotLen.WidthType = UserControl.WidthTypes.Normal;
            this.txtLotLen.XAlign = 372;
            // 
            // txtGOMO
            // 
            this.txtGOMO.AllowEditOnlyChecked = true;
            this.txtGOMO.AutoSelectAll = false;
            this.txtGOMO.AutoUpper = true;
            this.txtGOMO.Caption = "�趨��������";
            this.txtGOMO.Checked = false;
            this.txtGOMO.EditType = UserControl.EditTypes.String;
            this.txtGOMO.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGOMO.Location = new System.Drawing.Point(8, 12);
            this.txtGOMO.MaxLength = 40;
            this.txtGOMO.Multiline = false;
            this.txtGOMO.Name = "txtGOMO";
            this.txtGOMO.PasswordChar = '\0';
            this.txtGOMO.ReadOnly = false;
            this.txtGOMO.ShowCheckBox = true;
            this.txtGOMO.Size = new System.Drawing.Size(234, 24);
            this.txtGOMO.TabIndex = 1;
            this.txtGOMO.TabNext = true;
            this.txtGOMO.Value = "";
            this.txtGOMO.WidthType = UserControl.WidthTypes.Normal;
            this.txtGOMO.XAlign = 109;
            this.txtGOMO.CheckBoxCheckedChanged += new System.EventHandler(this.txtGOMO_CheckBoxCheckedChanged);
            this.txtGOMO.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGOMO_TxtboxKeyPress);
            // 
            // GroupItemInfo
            // 
            this.GroupItemInfo.Controls.Add(this.txtItemDesc);
            this.GroupItemInfo.Controls.Add(this.txtMO);
            this.GroupItemInfo.Controls.Add(this.txtItem);
            this.GroupItemInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.GroupItemInfo.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GroupItemInfo.Location = new System.Drawing.Point(0, 50);
            this.GroupItemInfo.Name = "GroupItemInfo";
            this.GroupItemInfo.Size = new System.Drawing.Size(923, 47);
            this.GroupItemInfo.TabIndex = 158;
            this.GroupItemInfo.TabStop = false;
            this.GroupItemInfo.Text = "��Ʒ��Ϣ";
            // 
            // txtItemDesc
            // 
            this.txtItemDesc.AllowEditOnlyChecked = true;
            this.txtItemDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txtItemDesc.AutoSelectAll = false;
            this.txtItemDesc.AutoUpper = true;
            this.txtItemDesc.Caption = "��Ʒ����";
            this.txtItemDesc.Checked = false;
            this.txtItemDesc.EditType = UserControl.EditTypes.String;
            this.txtItemDesc.Enabled = false;
            this.txtItemDesc.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtItemDesc.Location = new System.Drawing.Point(311, 14);
            this.txtItemDesc.MaxLength = 40;
            this.txtItemDesc.Multiline = false;
            this.txtItemDesc.Name = "txtItemDesc";
            this.txtItemDesc.PasswordChar = '\0';
            this.txtItemDesc.ReadOnly = true;
            this.txtItemDesc.ShowCheckBox = false;
            this.txtItemDesc.Size = new System.Drawing.Size(194, 24);
            this.txtItemDesc.TabIndex = 3;
            this.txtItemDesc.TabNext = true;
            this.txtItemDesc.Value = "";
            this.txtItemDesc.WidthType = UserControl.WidthTypes.Normal;
            this.txtItemDesc.XAlign = 372;
            // 
            // txtMO
            // 
            this.txtMO.AllowEditOnlyChecked = true;
            this.txtMO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txtMO.AutoSelectAll = false;
            this.txtMO.AutoUpper = true;
            this.txtMO.Caption = "����";
            this.txtMO.Checked = false;
            this.txtMO.EditType = UserControl.EditTypes.String;
            this.txtMO.Enabled = false;
            this.txtMO.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMO.Location = new System.Drawing.Point(620, 14);
            this.txtMO.MaxLength = 40;
            this.txtMO.Multiline = false;
            this.txtMO.Name = "txtMO";
            this.txtMO.PasswordChar = '\0';
            this.txtMO.ReadOnly = true;
            this.txtMO.ShowCheckBox = false;
            this.txtMO.Size = new System.Drawing.Size(170, 24);
            this.txtMO.TabIndex = 3;
            this.txtMO.TabNext = true;
            this.txtMO.Value = "";
            this.txtMO.WidthType = UserControl.WidthTypes.Normal;
            this.txtMO.XAlign = 657;
            // 
            // txtItem
            // 
            this.txtItem.AllowEditOnlyChecked = true;
            this.txtItem.AutoSelectAll = false;
            this.txtItem.AutoUpper = true;
            this.txtItem.Caption = "��Ʒ";
            this.txtItem.Checked = false;
            this.txtItem.EditType = UserControl.EditTypes.String;
            this.txtItem.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtItem.Location = new System.Drawing.Point(72, 14);
            this.txtItem.MaxLength = 40;
            this.txtItem.Multiline = false;
            this.txtItem.Name = "txtItem";
            this.txtItem.PasswordChar = '\0';
            this.txtItem.ReadOnly = true;
            this.txtItem.ShowCheckBox = false;
            this.txtItem.Size = new System.Drawing.Size(170, 24);
            this.txtItem.TabIndex = 0;
            this.txtItem.TabNext = true;
            this.txtItem.Value = "";
            this.txtItem.WidthType = UserControl.WidthTypes.Normal;
            this.txtItem.XAlign = 109;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 97);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(923, 396);
            this.panel1.TabIndex = 159;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ultraGridErrorCode);
            this.groupBox4.Controls.Add(this.txtErrorCode);
            this.groupBox4.Controls.Add(this.checkBoxAutoSaveErrorCode);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Right;

            this.groupBox4.Location = new System.Drawing.Point(561, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(362, 396);
            this.groupBox4.TabIndex = 159;
            this.groupBox4.TabStop = false;
            // 
            // ultraGridErrorCode
            // 
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridErrorCode.DisplayLayout.Appearance = appearance27;
            this.ultraGridErrorCode.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridErrorCode.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance28.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance28.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridErrorCode.DisplayLayout.GroupByBox.Appearance = appearance28;
            appearance29.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridErrorCode.DisplayLayout.GroupByBox.BandLabelAppearance = appearance29;
            this.ultraGridErrorCode.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance30.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance30.BackColor2 = System.Drawing.SystemColors.Control;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance30.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridErrorCode.DisplayLayout.GroupByBox.PromptAppearance = appearance30;
            this.ultraGridErrorCode.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridErrorCode.DisplayLayout.MaxRowScrollRegions = 1;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            appearance31.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridErrorCode.DisplayLayout.Override.ActiveCellAppearance = appearance31;
            appearance32.BackColor = System.Drawing.SystemColors.Highlight;
            appearance32.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridErrorCode.DisplayLayout.Override.ActiveRowAppearance = appearance32;
            this.ultraGridErrorCode.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridErrorCode.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance33.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridErrorCode.DisplayLayout.Override.CardAreaAppearance = appearance33;
            appearance34.BorderColor = System.Drawing.Color.Silver;
            appearance34.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridErrorCode.DisplayLayout.Override.CellAppearance = appearance34;
            this.ultraGridErrorCode.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraGridErrorCode.DisplayLayout.Override.CellPadding = 0;
            appearance35.BackColor = System.Drawing.SystemColors.Control;
            appearance35.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance35.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance35.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridErrorCode.DisplayLayout.Override.GroupByRowAppearance = appearance35;
            appearance36.TextHAlignAsString = "Left";
            this.ultraGridErrorCode.DisplayLayout.Override.HeaderAppearance = appearance36;
            this.ultraGridErrorCode.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridErrorCode.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridErrorCode.DisplayLayout.Override.RowAppearance = appearance37;
            this.ultraGridErrorCode.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridErrorCode.DisplayLayout.Override.TemplateAddRowAppearance = appearance38;
            this.ultraGridErrorCode.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridErrorCode.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridErrorCode.Location = new System.Drawing.Point(6, 50);
            this.ultraGridErrorCode.Name = "ultraGridErrorCode";
            this.ultraGridErrorCode.Size = new System.Drawing.Size(366, 343);
            this.ultraGridErrorCode.TabIndex = 159;
            this.ultraGridErrorCode.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridErrorCode_InitializeLayout);
            // 
            // txtErrorCode
            // 
            this.txtErrorCode.AllowEditOnlyChecked = true;
            this.txtErrorCode.AutoSelectAll = false;
            this.txtErrorCode.AutoUpper = true;
            this.txtErrorCode.Caption = "��������";
            this.txtErrorCode.Checked = false;
            this.txtErrorCode.EditType = UserControl.EditTypes.String;
            this.txtErrorCode.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtErrorCode.Location = new System.Drawing.Point(22, 20);
            this.txtErrorCode.MaxLength = 40;
            this.txtErrorCode.Multiline = false;
            this.txtErrorCode.Name = "txtErrorCode";
            this.txtErrorCode.PasswordChar = '\0';
            this.txtErrorCode.ReadOnly = false;
            this.txtErrorCode.ShowCheckBox = false;
            this.txtErrorCode.Size = new System.Drawing.Size(194, 24);
            this.txtErrorCode.TabIndex = 3;
            this.txtErrorCode.TabNext = true;
            this.txtErrorCode.Value = "";
            this.txtErrorCode.WidthType = UserControl.WidthTypes.Normal;
            this.txtErrorCode.XAlign = 83;
            this.txtErrorCode.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtErrorCode_TxtboxKeyPress);
            // 
            // checkBoxAutoSaveErrorCode
            // 
            this.checkBoxAutoSaveErrorCode.BackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxAutoSaveErrorCode.Location = new System.Drawing.Point(233, 21);
            this.checkBoxAutoSaveErrorCode.Name = "checkBoxAutoSaveErrorCode";
            this.checkBoxAutoSaveErrorCode.Size = new System.Drawing.Size(120, 16);
            this.checkBoxAutoSaveErrorCode.TabIndex = 2;
            this.checkBoxAutoSaveErrorCode.Text = "�Զ�������������";
            this.checkBoxAutoSaveErrorCode.UseVisualStyleBackColor = false;
            this.checkBoxAutoSaveErrorCode.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.ultraGridMain);
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(558, 396);
            this.groupBox3.TabIndex = 159;
            this.groupBox3.TabStop = false;
            // 
            // ultraGridMain
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridMain.DisplayLayout.Appearance = appearance13;
            this.ultraGridMain.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridMain.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMain.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMain.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.ultraGridMain.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMain.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.ultraGridMain.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridMain.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridMain.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridMain.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.ultraGridMain.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridMain.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridMain.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridMain.DisplayLayout.Override.CellAppearance = appearance20;
            this.ultraGridMain.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraGridMain.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMain.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.ultraGridMain.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.ultraGridMain.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridMain.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridMain.DisplayLayout.Override.RowAppearance = appearance23;
            this.ultraGridMain.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridMain.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.ultraGridMain.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridMain.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridMain.Location = new System.Drawing.Point(3, 17);
            this.ultraGridMain.Name = "ultraGridMain";
            this.ultraGridMain.Size = new System.Drawing.Size(552, 376);
            this.ultraGridMain.TabIndex = 158;
            this.ultraGridMain.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridMain_InitializeLayout);
            // 
            // FLotCollectionGDNG
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(923, 591);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.GroupItemInfo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panelButton);
            this.KeyPreview = true;
            this.Name = "FLotCollectionGDNG";
            this.Text = "��Ʒ/����Ʒ�ɼ�";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.FCollectionGDNG_Activated);
            this.Deactivate += new System.EventHandler(this.FCollectionGDNG_Deactivated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FCollectionGDNG_FormClosed);
            this.Load += new System.EventHandler(this.FCollectionGDNG_Load);
            this.panelButton.ResumeLayout(false);
            this.panelButton.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.GroupItemInfo.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridErrorCode)).EndInit();
            this.groupBox3.ResumeLayout(false);
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
            //this.m_LotDT.Columns.Add("MoCode", typeof(string));
            this.m_LotDT.Columns.Add("LotQty", typeof(string));
            this.m_LotDT.Columns.Add("GoodQty", typeof(string));
            this.m_LotDT.Columns.Add("NGQty", typeof(string));
            //this.m_LotDT.Columns.Add("ItemCode", typeof(string));
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
            //e.Layout.Bands[0].Columns["MoCode"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["LotQty"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["GoodQty"].Header.Caption = "��Ʒ����";
            e.Layout.Bands[0].Columns["NGQty"].Header.Caption = "��������";
            //e.Layout.Bands[0].Columns["ItemCode"].Header.Caption = "��Ʒ����";
            e.Layout.Bands[0].Columns["ProductStatus"].Header.Caption = "��Ʒ����״̬";
            e.Layout.Bands[0].Columns["CollectStatus"].Header.Caption = "�ɼ�״̬";
            e.Layout.Bands[0].Columns["MUser"].Header.Caption = "�ɼ���Ա";
            e.Layout.Bands[0].Columns["BeginDate"].Header.Caption = "��ʼ����";
            e.Layout.Bands[0].Columns["BeginTime"].Header.Caption = "��ʼʱ��";
            e.Layout.Bands[0].Columns["EndDate"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["EndTime"].Header.Caption = "����ʱ��";

            e.Layout.Bands[0].Columns["LotCode"].Width = 100;
            //e.Layout.Bands[0].Columns["MoCode"].Width = 100;
            e.Layout.Bands[0].Columns["LotQty"].Width = 60;
            e.Layout.Bands[0].Columns["GoodQty"].Width = 60;
            e.Layout.Bands[0].Columns["NGQty"].Width = 60;
            //e.Layout.Bands[0].Columns["ItemCode"].Width = 100;
            e.Layout.Bands[0].Columns["ProductStatus"].Width = 80;
            e.Layout.Bands[0].Columns["CollectStatus"].Width = 80;
            e.Layout.Bands[0].Columns["MUser"].Width = 80;
            e.Layout.Bands[0].Columns["BeginDate"].Width = 80;
            e.Layout.Bands[0].Columns["BeginTime"].Width = 80;
            e.Layout.Bands[0].Columns["EndDate"].Width = 80;
            e.Layout.Bands[0].Columns["EndTime"].Width = 80;

            // ������λ�Ƿ�����༭������λ����ʾ��ʽ
            e.Layout.Bands[0].Columns["LotCode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            //e.Layout.Bands[0].Columns["MoCode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["LotQty"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["GoodQty"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["NGQty"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            //e.Layout.Bands[0].Columns["ItemCode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
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
                        //rowGroup["MoCode"] = item.MOCode;
                        rowGroup["LotQty"] = item.LotQty;
                        rowGroup["GoodQty"] = item.GoodQty;
                        rowGroup["NGQty"] = item.NGQty;
                        //rowGroup["ItemCode"] = item.ItemCode;
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

        #region �������� 
        //add by kathy @20130829
        private void InitializeErrorCodeGrid()
        {
            this.m_ErrorCodeSet = new DataSet();
            this.m_ErrorCodeDT = new DataTable("ErrorCodeDT");
            this.m_ErrorCodeDT.Columns.Add("ECGCode", typeof(string));
            this.m_ErrorCodeDT.Columns.Add("ECode", typeof(string));
            this.m_ErrorCodeDT.Columns.Add("ECGDesc", typeof(string));
            this.m_ErrorCodeDT.Columns.Add("ErrorQty", typeof(string));
            this.m_ErrorCodeSet.Tables.Add(m_ErrorCodeDT);
            this.m_ErrorCodeSet.AcceptChanges();
        }

        private void ultraGridErrorCode_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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
            e.Layout.Bands[0].ScrollTipField = "ErrorCode";

            // �����п��������
            e.Layout.Bands[0].Columns["ECGCode"].Header.Caption = "����������";
            e.Layout.Bands[0].Columns["ECode"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["ECGDesc"].Header.Caption = "��������������";
            e.Layout.Bands[0].Columns["ErrorQty"].Header.Caption = "����";

            e.Layout.Bands[0].Columns["ECGCode"].Width = 98;
            e.Layout.Bands[0].Columns["ECode"].Width = 98;
            e.Layout.Bands[0].Columns["ECGDesc"].Width = 98;
            e.Layout.Bands[0].Columns["ErrorQty"].Width = 40;

            // ������λ�Ƿ�����༭������λ����ʾ��ʽ
            e.Layout.Bands[0].Columns["ECGCode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["ECode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["ECGDesc"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["ErrorQty"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;

            this.InitGridLanguage(ultraGridErrorCode);
        }

        //���ز�������
        private void LoadErrorCode(string itemCode)
        {
            InitializeErrorCodeGrid();

            if (_DataCollectFacade == null)
            {
                _DataCollectFacade = new DataCollectFacade(this.DataProvider);
            }
            try
            {
                this.ClearErrorCodeList();
                object[] errorCodeGroups = _DataCollectFacade.GetTSErrorCode(itemCode);
                DataRow rowGroup;
                if (errorCodeGroups != null)
                {
                    foreach (ErrorCodeForLot errorCode in errorCodeGroups)
                    {
                        rowGroup = this.m_ErrorCodeSet.Tables["ErrorCodeDT"].NewRow();
                        rowGroup["ECGCode"] = errorCode.ECGCode;
                        rowGroup["ECode"] = errorCode.ECode;
                        rowGroup["ECGDesc"] = errorCode.ECGDesc;
                        rowGroup["ErrorQty"] = 0;
                        this.m_ErrorCodeSet.Tables["ErrorCodeDT"].Rows.Add(rowGroup);
                    }

                }
                this.m_ErrorCodeSet.Tables["ErrorCodeDT"].AcceptChanges();
                this.m_ErrorCodeSet.AcceptChanges();
                this.ultraGridErrorCode.DataSource = this.m_ErrorCodeSet;
                this.ultraGridErrorCode.UpdateData();
            }
            catch (Exception ex) { }
        }

        private void ClearErrorCodeList()
        {
            if (this.m_ErrorCodeSet == null)
            {
                return;
            }
            this.m_ErrorCodeSet.Tables["ErrorCodeDT"].Rows.Clear();
            this.m_ErrorCodeSet.Tables["ErrorCodeDT"].AcceptChanges();
            this.m_ErrorCodeSet.AcceptChanges();
        }
        #endregion

        /// <summary>
        /// ��ò�Ʒ��Ϣ
        /// Laws Lu,2005/08/02,�޸�
        /// </summary>
        /// <returns></returns>
        private Messages GetProduct()
        {
            Messages productmessages = new Messages();
            ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);

            //Add By Bernard @ 2010-11-03
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
            string lotCode = this.txtLotCode.Value.Trim().ToUpper();
            //end

            productmessages.AddMessages(dataCollect.GetIDInfo(lotCode.Trim().ToUpper()));
            if (productmessages.IsSuccess())
            {
                product = (ProductInfo)productmessages.GetData().Values[0];
            }
            else
            {
                product = new ProductInfo();
            }

            dataCollect = null;
            return productmessages;
        }

        /// <summary>
        /// ���ݲ�Ʒ��Ϣ���������ֿؼ���״̬
        /// </summary>
        /// <returns></returns>
        private Messages CheckProduct()
        {
            Messages messages = new Messages();
            try
            {
                messages.AddMessages(GetProduct());

            }
            catch (Exception e)
            {
                messages.Add(new UserControl.Message(e));

            }
            return messages;
        }

        private Hashtable listActionCheckStatus = new Hashtable();


        //�ж��Ƿ��Ѿ��ɼ�
        private bool IsCollectByRes()
        {
            LotSimulation simulation = _DataCollectFacade.GetLotSimulation(this.txtLotCode.Value.Trim().ToUpper()) as LotSimulation;
            if (simulation != null)
            {
                object objOP = dataModel.GetOperationByResource(ApplicationService.Current().ResourceCode);

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

        public void txtLotCode_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //txtRunningCard.Value = txtRunningCard.Value.Trim();
            if (e.KeyChar == '\r')
            {
                if (txtLotCode.Value.Trim() == string.Empty)
                {
                    //Laws Lu,2005/08/10,����	��û�������Ʒ���к�ʱ��չ������Ϻ�
                    if (!this.txtGOMO.Checked)
                    {
                        txtMO.Value = String.Empty;
                        txtItem.Value = String.Empty;
                        txtItemDesc.Value = String.Empty;
                        this.txtGoodQty.Value = String.Empty;
                    }
                    //End Laws Lu

                    ApplicationRun.GetInfoForm().AddEx("$CS_PleaseInputSimLot");

                    //�������Ƶ���Ʒ���к������
                    txtLotCode.TextFocus(false, true);
                    return;
                }
                else
                {
                    //Add By Bernard @ 2010-11-03
                    string lotCode = this.txtLotCode.Value.Trim().ToUpper();
                    //end

                    // Added by Hi1/venus.Feng on 20080822 for Hisense Version
                    if (this.txtGOMO.Checked && this.txtGOMO.Value.Trim().Length == 0)
                    {
                        ApplicationRun.GetInfoForm().AddEx("$CS_CMPleaseInputMO");
                        this.txtGOMO.Checked = true;
                        this.txtGOMO.TextFocus(false, true);
                        return;
                    }

                    if (this.txtGOMO.Checked && this.txtGOMO.InnerTextBox.Enabled)
                    {
                        ApplicationRun.GetInfoForm().AddEx("$CS_PleasePressEnterOnGOMO");
                        this.txtGOMO.Checked = true;
                        this.txtGOMO.TextFocus(false, true);
                        return;
                    }
                    // End Added

                    if (txtLotCode.Value.Trim().ToUpper() == ng_collect)
                    {
                        rdoNG.Checked = true;
                        txtLotCode.TextFocus(false, true);
                        return;
                    }

                    //add by hiro.chen 08/11/18 TocheckIsDown
                    Messages msg = new Messages();
                    //msg.AddMessages(dataCollectFacade.CheckISDown(lotCode.Trim().ToUpper()));
                    //if (!msg.IsSuccess())
                    //{
                    //    ApplicationRun.GetInfoForm().AddEx(this._FunctionName, this.txtLotCode.Caption + ":" + this.txtLotCode.Value, msg, true);
                    //    return;
                    //}
                    //end 

                    //���ϲ��ܷ��� 
                    //msg.AddMessages(dataCollectFacade.CheckReworkRcardIsScarp(sourceCard.Trim().ToUpper(), ApplicationService.Current().ResourceCode));
                    //if (!msg.IsSuccess())
                    //{
                    //    ApplicationRun.GetInfoForm().AddEx(this._FunctionName, this.txtLotCode.Caption + ":" + this.txtLotCode.Value, msg, true);
                    //    txtLotCode.TextFocus(false, true);
                    //    return;
                    //}
                    //end


                    //Laws Lu,2005/10/19,����	������������
                    //Laws Lu,2006/12/25 �޸�	����Open/Close�Ĵ���
                    ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.AutoCloseConnection = false;
                    ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();

                    //�Ƿ��Ѿ��ɼ�
                    isCollect = IsCollectByRes();

                    //Laws Lu,2005/08/16,�޸�	��msg����globeMSG
                    globeMSG = CheckProduct();

                    if (Resource == null)
                    {
                        Resource = (Domain.BaseSetting.Resource)dataModel.GetResource(ApplicationService.Current().ResourceCode);
                    }
                    actionCheckStatus = new ActionCheckStatus();
                    actionCheckStatus.ProductInfo = product;

                    if (actionCheckStatus.ProductInfo != null)
                    {
                        actionCheckStatus.ProductInfo.Resource = Resource;
                    }

                    string strMoCode = String.Empty;
                    if (product != null && product.LastSimulation != null)
                    {
                        strMoCode = product.LastSimulation.MOCode;
                    }
                    if (strMoCode != String.Empty)
                    {
                        if (listActionCheckStatus.ContainsKey(strMoCode))
                        {
                            actionCheckStatus = (ActionCheckStatus)listActionCheckStatus[strMoCode];
                            actionCheckStatus.ProductInfo = product;
                            actionCheckStatus.ActionList = new ArrayList();
                        }
                        else
                        {
                            listActionCheckStatus.Add(strMoCode, actionCheckStatus);
                        }
                    }
                    //Amoi,Laws Lu,2005/08/02,�޸�
                    if (txtGOMO.Checked == true)
                    {
                        if (!isCollect)
                        {
                            globeMSG.AddMessages(RunGOMO(actionCheckStatus, txtLotCode.Value.Trim().ToUpper()));

                            if (!globeMSG.IsSuccess())
                            {
                                listActionCheckStatus.Clear();
                            }
                        }
                    }
                    else
                    {
                        if (product != null && product.LastSimulation != null)
                        {
                            //this.txtGoodQty.Value = product.LastSimulation.GoodQty.ToString();
                            this.txtMO.Value = product.LastSimulation.MOCode;
                            this.txtItem.Value = product.LastSimulation.ItemCode;
                            this.txtItemDesc.Value = this.GetItemDescription(product.LastSimulation.ItemCode, GlobalVariables.CurrentOrganizations.First().OrganizationID);
                        }
                        else
                        {
                            this.txtGoodQty.Value = "";
                            this.txtItem.Value = "";
                            this.txtMO.Value = "";
                            this.txtItemDesc.Value = "";
                        }
                    }
                    //EndAmoi

                    //Amoi,Laws Lu,2005/08/02,���� �����ƷRadioBox��ѡ����ֱ�ӱ���
                    if (globeMSG.IsSuccess() && rdoGood.Checked == true)
                    {
                        if (!isCollect)
                        {
                            btnSave_Click(sender, e);
                        }
                        else
                        {
                            this.btnSave.Enabled = true;
                            this.btnSave.Focus();
                        }
                    }
                    else if (globeMSG.IsSuccess() && rdoNG.Checked == true)
                    {
                        //û�п�ʼ�ɼ�������ʾ�û���Ĭ�ϲɼ���Ʒ
                        if (!isCollect)
                        {
                            globeMSG.Add(new UserControl.Message(MessageType.Error, "$CS_Lot_Must_Collect_GoodBegin"));
                            txtLotCode.Value = "";
                        }
                        else
                        {
                            // Modified By Hi1/Venus.Feng on 20080821 for Hisense Version : support auto generate errorcode online
                            if (this.checkBoxAutoSaveErrorCode.Checked)
                            {
                                if (this.SetErrorCodeListForDefaultSetting())
                                {
                                    btnSave_Click(sender, e);
                                }
                            }
                            else
                            {
                                if (SetErrorCodeList())
                                {
                                    //ucErrorCodeSelectNew.Focus();
                                    this.txtErrorCode.Focus();

                                    if (ultraGridErrorCode.Rows.Count < 1)
                                    //if (ucErrorCodeSelectNew.m_ErrorGroup.Rows.Count < 1)
                                    {
                                        globeMSG.Add(new UserControl.Message(MessageType.Error, "$CS_MUST_MAINTEN_ERRGROUP"));
                                        txtLotCode.Value = "";
                                    }

                                    this.checkBoxAutoSaveErrorCode.Enabled = false;
                                    btnSave.Enabled = true;
                                }
                            }
                            // End Modified
                        }
                    }

                }

                //Laws Lu,2005/10/19,����	������������
                //Laws Lu,2006/12/25 �޸�	����Open/Close�Ĵ���
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.AutoCloseConnection = true;


                //�������Ƶ���Ʒ���к������
                ApplicationRun.GetInfoForm().AddEx(this._FunctionName, this.txtLotCode.Caption + ": " + this.txtLotCode.Value, globeMSG, true);

                //Application.DoEvents();
                txtLotCode.TextFocus(false, true);

                if (rdoNG.Checked == true && globeMSG.IsSuccess())
                {
                    // Modified By Hi1/Venus.Feng on 20080821 for Hisense Version 
                    if (this.checkBoxAutoSaveErrorCode.Checked)
                    {
                        this.InitErrorSelector();
                        this.checkBoxAutoSaveErrorCode.Checked = true;
                        //this.ucErrorCodeSelectNew.Enabled = false;
                        this.txtErrorCode.Enabled = false;
                        this.checkBoxAutoSaveErrorCode.Enabled = false;
                        this.rdoGood.Checked = true;
                    }
                    else
                    {
                        this.checkBoxAutoSaveErrorCode.Enabled = false;
                        //this.ucErrorCodeSelectNew.Enabled = true;
                        //this.ucErrorCodeSelectNew.ucLabelEditErrorCode.Value = String.Empty;
                        //this.ucErrorCodeSelectNew.ucLabelEditErrorCode.Focus();
                        this.txtErrorCode.Enabled = true;
                        this.txtErrorCode.Value = string.Empty;
                        this.txtErrorCode.Focus();
                    }
                    // ENd Added
                }

                globeMSG.ClearMessages();
            }
            else
            {
                this.btnSave.Enabled = false;
            }
        }

        private string GetItemDescription(string itemCode, int orgID)
        {
            ItemFacade facade = new ItemFacade(this.DataProvider);
            Item item = facade.GetItem(itemCode, orgID) as Item;
            if (item == null)
            {
                return itemCode;
            }
            else
            {
                return (item as Item).ItemDescription;
            }
        }

        // Added By Hi1/Venus.Feng on 20080821 for Hisense Version : Support Auto generate errorcode 
        private bool SetErrorCodeListForDefaultSetting()
        {
            TSModelFacade tsFacade = new TSModelFacade(this.DataProvider);

            //Add By Bernrd @ 2010-11-03
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
            string lotCode = this.txtLotCode.Value.Trim().ToUpper();
            //end

            string strItem = String.Empty;

            // Added by Icyer 2007/03/09		��Ϊ���ж��Ƿ���Ҫ�����������Ӵ������Ĺ�����ȡ��Ʒ���룻��������������Ŵ�Simulationȡ
            ActionGoToMO actionGoMO = new ActionGoToMO(this.DataProvider);
            Messages msgMo = actionGoMO.GetItemCodeFromGoMoRCard(ApplicationService.Current().ResourceCode, lotCode.Trim().ToUpper());
            if (msgMo.IsSuccess() == false)		// ����д��󣬱�ʾ��Ҫ�������������ǽ����������ѯ������������
            {
                globeMSG.AddMessages(msgMo);
                if (!this.txtGOMO.Checked)
                {
                    txtMO.Value = String.Empty;
                    txtItem.Value = String.Empty;
                    txtItemDesc.Value = "";
                }
                return false;
            }
            else	// ���سɹ����������������Ҫ�����������ҷ�����ȷ�Ĺ�����Ϣ������Ҫ��������
            {
                UserControl.Message msgMoData = msgMo.GetData();
                if (msgMoData != null)		// ��DATA���ݣ���ʾ��Ҫ��������
                {
                    MO mo = (MO)msgMoData.Values[0];
                    if (mo != null)
                    {
                        strItem = mo.ItemCode;
                    }
                }
                else		// ���û��DATA���ݣ���ʾ����Ҫ�����������������ǰ�Ĵ��룺�����к��Ҳ�Ʒ
                {
                    GetProduct();
                    if (product.LastSimulation != null)
                    {
                        strItem = product.LastSimulation.ItemCode;
                    }
                }
            }
            if (strItem == string.Empty)	// �������û�еõ���Ʒ���룬�򱨴�
            {
                globeMSG.Add(new UserControl.Message(MessageType.Error, "$NoSimulation"));
                if (!this.txtGOMO.Checked)
                {
                    txtMO.Value = String.Empty;
                    txtItem.Value = String.Empty;
                    txtItemDesc.Value = "";
                }
                return false;
            }
            // Added end

            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            object parameter = systemSettingFacade.GetParameter("DEFAULTERRORCODE", "NGCOLLECTDEFAULTERRORCODE");
            if (parameter == null)
            {
                globeMSG.Add(new UserControl.Message(MessageType.Error, "$Error_NoDefaultErrorCode"));
                return false;
            }

            //modified by kathy @20130830
            //���ݲ�Ʒ������ز���������Ϣ
            LoadErrorCode(strItem);

            //Parameter errorCodeParameter = parameter as Parameter;
            //string errorCode = errorCodeParameter.ParameterAlias;
            //object[] ecgObjects = tsFacade.GetErrorCodeGroupByErrorCodeCode(errorCode);
            //if (ecgObjects == null || ecgObjects.Length == 0)
            //{
            //    globeMSG.Add(new UserControl.Message(MessageType.Error, "$Error_ErrorCodeNoErrorGroup"));
            //    return false;
            //}
            //ucErrorCodeSelectNew.SetGridSelected(errorCode, "");
            return true;
        }
        // End Added

        //Laws Lu,2005/08/25,�޸�	����ʧ�����RunningCard�����
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            this.txtLotCode.TextFocus(false, true);
            Messages messages = new Messages();
            ActionOnLineHelper onLine = new ActionOnLineHelper(this.DataProvider);

            string lotCode = this.txtLotCode.Value.Trim().ToUpper();
            string OutputMessages = string.Empty;

            DataProvider.BeginTransaction();
            try
            {
                #region Laws Lu,���水ť�����߼�
                if (txtGOMO.Checked)
                {
                    if (rdoGood.Checked)
                    {
                        messages = GetProduct();
                        if (messages.IsSuccess() && !isCollect)
                        {
                            messages.AddMessages(RunGood(actionCheckStatus));
                        }
                    }
                    else if (rdoNG.Checked)
                    {
                        messages = CheckErrorCodes();

                        if (messages.IsSuccess())
                        {
                            messages = GetProduct();
                        }
                        BaseModelFacade baseModelFacade = new BaseModelFacade(this.DataProvider);
                        object objOP = baseModelFacade.GetOperationByResource(ApplicationService.Current().ResourceCode);

                        if (messages.IsSuccess() && isCollect)
                        {
                            messages.AddMessages(RunNG(actionCheckStatus));
                        }
                    }
                }
                else
                {
                    if (rdoGood.Checked)
                    {
                        //add by hiro 2008/12/04
                        if (messages.IsSuccess() && !isCollect)
                        {
                            messages.AddMessages(RunGood(actionCheckStatus));
                        }
                    }
                    else if (rdoNG.Checked)
                    {
                        messages = CheckErrorCodes();

                        if (messages.IsSuccess() && isCollect)
                        {
                            messages.AddMessages(RunNG(actionCheckStatus));
                        }
                    }
                }
                #endregion

                if (messages.IsSuccess())
                {
                    if (isCollect)
                    {
                        //����TBLLotSimulation,TblLotsimulationReport,TblLotOnWip , tbllotOnWipItem

                        DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

                        LotSimulation nowSimulation = _DataCollectFacade.GetLotSimulation(lotCode) as LotSimulation;
                        nowSimulation.CollectStatus = CollectStatus.CollectStatus_END;
                        nowSimulation.EndDate = dbDateTime.DBDate;
                        nowSimulation.EndTime = dbDateTime.DBTime;
                        nowSimulation.MaintainUser = ApplicationService.Current().UserCode;

                        if (cbxGoodQty.Checked)
                        {
                            if (this.txtGoodQty.Value.Trim() == string.Empty)
                            {
                                throw new Exception("$Error_GoodQty_IS_Empty");
                            }
                            if (Convert.ToDecimal(this.txtGoodQty.Value.Trim()) > nowSimulation.LotQty)
                            {
                                throw new Exception("$Error_GoodQty_IS_Bigger");
                            }
                            if (Convert.ToDecimal(this.txtGoodQty.Value.Trim()) <= 0)
                            {
                                throw new Exception("$Error_GoodQty_IS_Small");
                            }

                            nowSimulation.GoodQty = Convert.ToDecimal(this.txtGoodQty.Value.Trim());
                            nowSimulation.NGQty = nowSimulation.LotQty - nowSimulation.GoodQty;
                        }
                        else if (cbxNGQty.Checked)
                        {
                            if (this.txtNGQty.Value.Trim() == string.Empty)
                            {
                                throw new Exception("$Error_NGQty_IS_Empty");
                            }
                            if (Convert.ToDecimal(this.txtNGQty.Value.Trim()) > nowSimulation.LotQty)
                            {
                                throw new Exception("$Error_NGQty_IS_Bigger");
                            }
                            if (Convert.ToDecimal(this.txtNGQty.Value.Trim()) < 0)
                            {
                                throw new Exception("$Error_NGQty_IS_Small");
                            }

                            nowSimulation.NGQty = Convert.ToDecimal(this.txtNGQty.Value.Trim());
                            nowSimulation.GoodQty = nowSimulation.LotQty - nowSimulation.NGQty;
                        }

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
                            else if (nowSimulationReport.EndTime < period.TimePeriodBeginTime)
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
                        nowSimulationReport.IsComplete = nowSimulation.IsComplete;
                        nowSimulationReport.GoodQty = nowSimulation.GoodQty;
                        nowSimulationReport.NGQty = nowSimulation.NGQty;
                        nowSimulationReport.EndTimePeriodCode = period.TimePeriodCode;
                        nowSimulationReport.EndShiftCode = period.ShiftCode;
                        nowSimulationReport.CollectStatus = CollectStatus.CollectStatus_END;
                        nowSimulationReport.MaintainUser = ApplicationService.Current().UserCode;
                        _DataCollectFacade.UpdateLotSimulationReport(nowSimulationReport);

                        object[] objs = _DataCollectFacade.QueryLotOnWip(nowSimulation.MOCode, nowSimulation.LotCode, nowSimulation.OPCode);
                        if (objs != null)
                        {
                            foreach (LotOnWip onwip in objs)
                            {
                                onwip.GoodQty = nowSimulation.GoodQty;
                                onwip.NGQty = nowSimulation.NGQty;
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

                        if (this.rdoGood.Checked)
                        {
                            if (nowSimulation.IsComplete == "1")
                            {
                                MOFacade moFacade = new MOFacade(this.DataProvider);
                                moFacade.UpdateMOACTQTY(nowSimulation.MOCode);
                            }

                            messages.Add(new UserControl.Message(MessageType.Success, string.Format("$CS_GOODSUCCESS,$CS_Parameter_Lot: {0}", txtLotCode.Value.Trim())));
                            this.btnSave.Enabled = false;
                        }
                    }

                    DataProvider.CommitTransaction();

                    //�����Ѳɼ�����������
                    object obj = _DataCollectFacade.GetLotSimulation(lotCode.ToUpper());
                    if (obj != null)
                    {
                        LoadLotSimulationList((obj as LotSimulation).MOCode, (obj as LotSimulation).ResCode);
                    }
                }
                else
                {
                    txtLotCode.Value = String.Empty;
                    DataProvider.RollbackTransaction();
                }

            }
            catch (Exception exp)
            {
                DataProvider.RollbackTransaction();
                messages.Add(new UserControl.Message(exp));
                txtLotCode.Value = String.Empty;
                //globeMSG.AddMessages(messages);
            }
            finally
            {
                globeMSG.AddMessages(messages);

                Messages newMsg = new Messages();
                string exception = globeMSG.OutPut();
                exception = exception.Replace(lotCode, this.txtLotCode.Value);
                string type = string.Empty;
                if (globeMSG.functionList().IndexOf(":") > 0)
                {
                    type = globeMSG.functionList().Substring(0, globeMSG.functionList().IndexOf(":"));
                }
                newMsg.Add(new UserControl.Message(MessageType.Error, exception));
                if (type == MessageType.Error.ToString())
                {
                    ApplicationRun.GetInfoForm().AddEx(this._FunctionName, this.txtLotCode.Caption + ": " + this.txtLotCode.Value, newMsg, true);
                }
                else
                {
                    ApplicationRun.GetInfoForm().AddEx(this._FunctionName, this.txtLotCode.Caption + ": " + this.txtLotCode.Value, globeMSG, true);
                }

                globeMSG.ClearMessages();

                //��������ҳ��ؼ�״̬
                if (messages.IsSuccess())
                {
                    ClearFormMessages();
                }
                //Laws Lu,2005/10/19,����	������������
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }

            if (!messages.IsSuccess() && rdoGood.Checked == true)//Amoi,Laws Lu,2005/08/02,����	ʧ��ʱSave��ť״̬ΪFalse
            {
                txtLotCode.Value = String.Empty;
                btnSave.Enabled = false;
            }
            else if (!messages.IsSuccess() && rdoNG.Checked == true)//Amoi,Laws Lu,2005/08/10,����	ʧ��ʱSave��ť״̬ΪTrue
            {
                // Modified By Hi1/Venus.Feng on 20080821 for Hisense Version
                if (this.checkBoxAutoSaveErrorCode.Checked)
                {
                    this.InitErrorSelector();
                    txtLotCode.TextFocus(false, true);
                }
                else
                {
                    txtLotCode.Value = String.Empty;
                    txtLotCode.TextFocus(false, true);
                    this.checkBoxAutoSaveErrorCode.Enabled = true;
                    btnSave.Enabled = true;
                }
                // ENd Added
            }

            if (messages.IsSuccess() && rdoNG.Checked == true)
            {
                this.InitErrorSelector();
                this.checkBoxAutoSaveErrorCode.Enabled = true;
                btnSave.Enabled = false;
                txtLotCode.TextFocus(false, true);
                this.txtNGQty.Value = "0";
                this.txtGoodQty.Value = "";
            }
        }

        //���������ɼ�
        private Messages RunGOMO(ActionCheckStatus actionCheckStatus, string lotCode)
        {
            Messages messages = new Messages();
            ActionOnLineHelper onLine = new ActionOnLineHelper(this.DataProvider);

            //Laws Lu,2005/09/14,����	��������Ϊ��
            if (txtGOMO.Checked == true && txtGOMO.Value.Trim() != String.Empty)
            {
                actionCheckStatus.ProductInfo = product;

                //����Ʒ���кŸ�ʽ
                bool lenCheckBool = true;
                //��Ʒ���кų��ȼ��
                if (txtLotLen.Checked && txtLotLen.Value.Trim() != string.Empty)
                {
                    int len = 0;
                    try
                    {
                        len = int.Parse(txtLotLen.Value.Trim());
                        if (txtLotCode.Value.Trim().Length != len)
                        {
                            lenCheckBool = false;
                            messages.Add(new UserControl.Message(MessageType.Error, "$CS_LOT_NO_LEN_CHECK_FAIL $CS_Parameter_Lot:" + txtLotCode.Value.Trim()));
                        }
                    }
                    catch
                    {
                        messages.Add(new UserControl.Message(MessageType.Error, "$CS_LOT_NO_LEN_CHECK_FAIL $CS_Parameter_Lot:" + txtLotCode.Value.Trim()));
                    }
                }

                //��Ʒ���к����ַ������
                if (txtLotLetter.Checked && txtLotLetter.Value.Trim() != string.Empty)
                {
                    int index = -1;
                    if (txtLotLetter.Value.Trim().Length <= lotCode.Trim().Length)
                    {
                        index = lotCode.IndexOf(txtLotLetter.Value.Trim());
                    }
                    if (index == -1)
                    {
                        lenCheckBool = false;
                        messages.Add(new UserControl.Message(MessageType.Error, "$CS_LOT_NO_FCHAR_CHECK_FAIL $CS_Parameter_Lot:" + lotCode.Trim()));
                    }
                }

                //add by hiro 08/11/05 ������к�����Ϊ��ĸ,���ֺͿո�   

                //if (!_DataCollectFacade.CheckInputRcard(this.txtItem.Value.Trim().ToString().ToUpper(), lotCode.Trim().ToString().ToUpper()))
                //{
                //    messages.Add(new UserControl.Message(MessageType.Error, "$CS_SNContent_CheckWrong $CS_Param_RunSeq:" + lotCode.Trim().ToString()));
                //}
                //end by hiro

                //Laws Lu,��������
                GoToMOActionEventArgs args = new GoToMOActionEventArgs(ActionType.DataCollectAction_GoMO,
                    lotCode.Trim(),
                    ApplicationService.Current().UserCode,
                    ApplicationService.Current().ResourceCode, product, txtGOMO.Value);

                //Laws Lu,ִ�й����ɼ����ռ�������Ϣ
                if (messages.IsSuccess())
                {
                    messages.AddMessages(onLine.Action(args, actionCheckStatus));
                }
            }

            if (messages.IsSuccess())
            {
                messages.Add(new UserControl.Message(MessageType.Success, "$CS_GOMOSUCCESS $CS_Parameter_Lot:" + lotCode.Trim().ToString()));
                txtLotCode.TextFocus(false, true);
            }

            return messages;
        }

        /// <summary>
        /// GOODָ��ɼ�
        /// </summary>
        /// <returns></returns>
        private Messages RunGood(ActionCheckStatus actionCheckStatus)
        {
            Messages messages = new Messages();
            //Add By Bernard @ 2010-11-03
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
            string lotCode = this.txtLotCode.Value.Trim().ToUpper();
            if (product.LastSimulation != null)
            {
                this.txtGoodQty.Value = product.LastSimulation.GoodQty.ToString();
            }
            else
            {
                this.txtGoodQty.Value = "0";
            }
            //end
            IAction dataCollectModule = new ActionFactory(this.DataProvider).CreateAction(ActionType.DataCollectAction_GOOD);
            messages.AddMessages(((IActionWithStatus)dataCollectModule).Execute(new ActionEventArgs(ActionType.DataCollectAction_GOOD, lotCode.Trim(),
                ApplicationService.Current().UserCode,
                ApplicationService.Current().ResourceCode, product), actionCheckStatus));

            if (messages.IsSuccess())
            {
                messages.Add(new UserControl.Message(MessageType.Success, string.Format("$CS_GOOD_BEGIN,$CS_Parameter_Lot: {0}", txtLotCode.Value.Trim())));
            }
            return messages;
        }

        /// <summary>
        /// NGָ��ɼ�
        /// </summary>
        /// <returns></returns>
        private Messages RunNG(ActionCheckStatus actionCheckStatus)
        {
            Messages messages = new Messages();

            //Add By Bernard @ 2010-11-03
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
            string lotCode = this.txtLotCode.Value.Trim().ToUpper();
            //end
            //actionCheckStatus.currentGoodQty = Convert.ToInt32(this.txtGoodQty.Value.Trim());
            if (cbxGoodQty.Checked)
            {
                if (this.txtGoodQty.Value.Trim() == string.Empty)
                {
                    throw new Exception("$Error_GoodQty_IS_Empty");
                }
                if (Convert.ToDecimal(this.txtGoodQty.Value.Trim()) > product.LastSimulation.LotQty)
                {
                    throw new Exception("$Error_GoodQty_IS_Bigger");
                }
                if (Convert.ToDecimal(this.txtGoodQty.Value.Trim()) <= 0)
                {
                    throw new Exception("$Error_GoodQty_IS_Small");
                }

                actionCheckStatus.currentGoodQty = Convert.ToDecimal(this.txtGoodQty.Value.Trim());

            }
            else if (cbxNGQty.Checked)
            {
                if (this.txtNGQty.Value.Trim() == string.Empty)
                {
                    throw new Exception("$Error_NGQty_IS_Empty");
                }
                if (Convert.ToDecimal(this.txtNGQty.Value.Trim()) > product.LastSimulation.LotQty)
                {
                    throw new Exception("$Error_NGQty_IS_Bigger");
                }
                if (Convert.ToDecimal(this.txtNGQty.Value.Trim()) <= 0)
                {
                    throw new Exception("$Error_NGQty_IS_Small");
                }

                actionCheckStatus.currentGoodQty = product.LastSimulation.LotQty - Convert.ToDecimal(this.txtNGQty.Value.Trim());
            }

            object[] ErrorCodes = GetSelectedErrorCodes();//ȡ���������飫��������

            IAction dataCollectModule = new ActionFactory(this.DataProvider).CreateAction(ActionType.DataCollectAction_NG);
            messages.AddMessages(((IActionWithStatus)dataCollectModule).Execute(
                new TSActionEventArgs(ActionType.DataCollectAction_NG,
                lotCode,
                ApplicationService.Current().UserCode,
                ApplicationService.Current().ResourceCode,
                product,
                ErrorCodes,
                txtMem.Value), actionCheckStatus));



            if (messages.IsSuccess())
            {
                messages.Add(new UserControl.Message(MessageType.Success, string.Format("$CS_NGSUCCESS,$CS_Parameter_Lot: {0}", txtLotCode.Value.Trim())));
            }
            return messages;
        }

        private void rdoGood_Click(object sender, System.EventArgs e)
        {
            this.InitErrorSelector();
            this.checkBoxAutoSaveErrorCode.Enabled = false;
        }

        private void rdoNG_Click(object sender, System.EventArgs e)
        {
            this.InitErrorSelector();
            this.checkBoxAutoSaveErrorCode.Enabled = true;

        }

        /// <summary>
        /// ����ɹ�������������ݲ���ʼ���ؼ�״̬
        /// Amoi,Laws Lu,2005/08/02
        /// </summary>
        private void ClearFormMessages()
        {
            txtMem.Value = string.Empty;

            txtErrorCode.Value = string.Empty;
            txtErrorCode.Enabled = false;
            ClearErrorCodeList();
            //ucErrorCodeSelectNew.ClearErrorList();
            InitialRunningCard();
        }

        /// <summary>
        /// ��ʼ��RunningCard��״̬
        /// Amoi,Laws Lu,2005/08/02,����
        /// </summary>
        private void InitialRunningCard()
        {
            txtLotCode.Value = String.Empty;
            txtLotCode.TextFocus(false, true);
        }

        private void FCollectionGDNG_Load(object sender, System.EventArgs e)
        {
            this._FunctionName = this.Text;
            this.txtNGQty.Value = "0";


            InitialRunningCard();
            this.checkBoxAutoSaveErrorCode.Checked = false;
            this.checkBoxAutoSaveErrorCode.Enabled = false;
            //this.ucErrorCodeSelectNew.Enabled = false;

            txtErrorCode.Value = string.Empty;
            txtErrorCode.Enabled = false;
            this.ultraGridErrorCode.Enabled = true;

            _DataCollectFacade = new DataCollectFacade(this.DataProvider);
            dataModel = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider);

            //this.InitPageLanguage();

            //this.InitUCErrorCodeSelectNewLanguage(ucErrorCodeSelectNew);
            //this.InitGridLanguage(ucErrorCodeSelectNew.ultraGridErrorList);

        }

        /// <summary>
        /// �����ǰ���ã����������ò��������б��ֵ
        /// Amoi,Laws Lu,2005/08/02,����
        /// </summary>
        private bool SetErrorCodeList()
        {
            TSModelFacade tsFacade = new TSModelFacade(this.DataProvider);

            //Add By Bernard @ 2010-11-03
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
            string lotCode = txtLotCode.Value.Trim().ToUpper();
            //end

            //Amoi,Laws Lu,2005/08/06,�޸�
            string strItem = String.Empty;

            // Added by Icyer 2007/03/09		��Ϊ���ж��Ƿ���Ҫ�����������Ӵ������Ĺ�����ȡ��Ʒ���룻��������������Ŵ�Simulationȡ
            ActionGoToMO actionGoMO = new ActionGoToMO(this.DataProvider);
            Messages msgMo = actionGoMO.GetItemCodeFromGoMoRCard(ApplicationService.Current().ResourceCode, lotCode.Trim().ToUpper());
            if (msgMo.IsSuccess() == false)		// ����д��󣬱�ʾ��Ҫ�������������ǽ����������ѯ������������
            {
                globeMSG.AddMessages(msgMo);
                //txtItem.Value = String.Empty;
                //txtMO.Value = String.Empty;
                return false;
            }
            else	// ���سɹ����������������Ҫ�����������ҷ�����ȷ�Ĺ�����Ϣ������Ҫ��������
            {
                UserControl.Message msgMoData = msgMo.GetData();
                if (msgMoData != null)		// ��DATA���ݣ���ʾ��Ҫ��������
                {
                    MO mo = (MO)msgMoData.Values[0];
                    if (mo != null)
                        strItem = mo.ItemCode;
                }
                else		// ���û��DATA���ݣ���ʾ����Ҫ�����������������ǰ�Ĵ��룺�����к��Ҳ�Ʒ
                {
                    GetProduct();
                    if (product.LastSimulation != null)
                    {
                        strItem = product.LastSimulation.ItemCode;
                    }
                }
            }
            if (strItem == string.Empty)	// �������û�еõ���Ʒ���룬�򱨴�
            {
                globeMSG.Add(new UserControl.Message(MessageType.Error, "$NoSimulation"));
                return false;
            }
            // Added end

            //modified by kathy @20130830
            //���ݲ�Ʒ������ز���������Ϣ
            LoadErrorCode(strItem);


            //�����ݵ��ؼ�
            //ucErrorCodeSelectNew.LoadErrorList(strItem, this.DataProvider);
            return true;

        }

        private void InitErrorSelector()
        {
            ClearErrorCodeList();
            //ucErrorCodeSelectNew.ClearErrorList();
        }

        private Messages CheckErrorCodes()
        {
            Messages megs = new Messages();
            //modified by kathy @20130830
            //���û��ѡ�������룬�򱨴�
            int totalErrorQty = 0;
            foreach (UltraGridRow row in ultraGridErrorCode.Rows)
            {
                totalErrorQty += Int32.Parse(row.Cells["ErrorQty"].Value.ToString());
                if (totalErrorQty > 0)
                    return megs;
            }
            if (totalErrorQty == 0)
                megs.Add(new UserControl.Message(MessageType.Error, "$DCT_PLEASE_INPUT_ErrorCode"));//�����벻������

            //if (ucErrorCodeSelectNew.GetSelectedErrorCodeList() == null || ucErrorCodeSelectNew.GetSelectedErrorCodeList().Length == 0)
            //megs.Add(new UserControl.Message(MessageType.Error, "$CS_Please_Select_ErrorCode"));
            return megs;
        }

        private object[] GetSelectedErrorCodes()
        {
            //modified by kathy @20130830
            //��ò������롢��������
            ArrayList tsErrorCodes = new ArrayList();
            foreach (UltraGridRow row in ultraGridErrorCode.Rows)
            {
                if (row.Cells["ErrorQty"].Value.ToString().Trim() != "0")
                {
                    TSErrorCode tsErrorCode = new TSErrorCode();
                    tsErrorCode.ErrorCode = row.Cells["ECode"].Value.ToString();
                    tsErrorCode.ErrorCodeGroup = row.Cells["ECGCode"].Value.ToString();
                    tsErrorCode.ErrorQty = Int32.Parse(row.Cells["ErrorQty"].Value.ToString());
                    tsErrorCodes.Add(tsErrorCode);
                }
            }
            object[] result = tsErrorCodes.ToArray();
            return result;

            //object[] result = this.ucErrorCodeSelectNew.GetSelectedErrorCodeList();
            //return result;
        }

        private void FCollectionGDNG_Activated(object sender, System.EventArgs e)
        {
            txtLotCode.TextFocus(false, true);
        }

        private void FCollectionGDNG_Deactivated(object sender, System.EventArgs e)
        {
            ApplicationRun.GetQtyForm().Hide();
        }

        private void rdoGood_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rdoGood.Checked)
            {
                txtLotCode.TextFocus(false, true);
            }
        }

        private void rdoNG_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rdoNG.Checked)
            {
                txtLotCode.TextFocus(false, true);
            }
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

        /// <summary>
        /// added by jessie lee,�ж��Ƿ������һ������
        /// </summary>
        /// <param name="moCode"></param>
        /// <param name="routeCode"></param>
        /// <param name="opCode"></param>
        /// <returns></returns>
        private bool IsLastOP(string moCode, string routeCode, string opCode)
        {
            if (routeCode == string.Empty)
                return false;
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);

            return dataCollectFacade.OPIsMORouteLastOP(moCode, routeCode, opCode);
        }

        private void txtGOMO_CheckBoxCheckedChanged(object sender, System.EventArgs e)
        {
            if (txtGOMO.Checked == false)
            {
                this.txtLotLen.Value = String.Empty;
                this.txtLotLetter.Value = String.Empty;
                this.txtLotLetter.Checked = false;
                this.txtLotLen.Checked = false;
                this.txtLotLen.Enabled = false;
                this.txtLotLetter.Enabled = false;
            }
            if (txtGOMO.Checked == true)
            {
                this.txtLotLen.Value = String.Empty;
                this.txtLotLetter.Value = String.Empty;
                this.txtLotLetter.Checked = false;
                this.txtLotLen.Checked = false;
                this.txtLotLen.Enabled = true;
                this.txtLotLetter.Enabled = true;
            }

            //this.txtItem.Value = "";
            //this.txtMO.Value = "";
            //this.txtItemDesc.Value = "";
        }

        private void txtGOMO_TxtboxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                // Added By Hi1/Venus.Feng on 20080822 for Hisense Version : Add loading item and item description
                if (this.txtGOMO.Value.Trim().Length == 0)
                {
                    ApplicationRun.GetInfoForm().AddEx("$CS_CMPleaseInputMO");
                    this.txtItem.Value = "";
                    this.txtMO.Value = "";
                    this.txtItemDesc.Value = "";
                    this.txtLotLetter.Value = "";
                    this.txtLotLetter.Checked = false;
                    this.txtGOMO.Checked = true;
                    this.txtGOMO.TextFocus(false, true);
                    return;
                }

                string moCode = this.txtGOMO.Value.Trim().ToUpper();
                MOFacade mofacade = new MOFacade(this.DataProvider);

                object moObj = mofacade.GetMO(moCode);
                if (moObj == null)
                {
                    // mo not exist
                    ApplicationRun.GetInfoForm().AddEx("$CS_MO_NOT_EXIST");
                    this.txtItem.Value = "";
                    this.txtMO.Value = "";
                    this.txtItemDesc.Value = "";
                    this.txtLotLetter.Value = "";
                    this.txtLotLetter.Checked = false;
                    this.txtGOMO.Checked = true;
                    this.txtGOMO.TextFocus(false, true);
                    return;
                }

                //�����Ѳɼ�����������
                LoadLotSimulationList(this.txtGOMO.Value.Trim().ToString(), ApplicationService.Current().ResourceCode);

                MO mo = moObj as MO;
                this.txtMO.Value = mo.MOCode;

                ItemFacade itemFacade = new ItemFacade(this.DataProvider);
                object itemObject = itemFacade.GetItem(mo.ItemCode, mo.OrganizationID);
                if (itemObject == null)
                {
                    // Item not exist
                    ApplicationRun.GetInfoForm().AddEx("$Error_ItemCode_NotExist $Domain_ItemCode:" + mo.ItemCode);
                    this.txtItem.Value = "";
                    this.txtMO.Value = "";
                    this.txtItemDesc.Value = "";
                    this.txtLotLetter.Value = "";
                    this.txtLotLetter.Checked = false;
                    this.txtGOMO.Checked = true;
                    this.txtGOMO.TextFocus(false, true);
                    return;
                }

                Item item = itemObject as Item;

                ItemLotFacade itemLotFacade = new ItemLotFacade(this._domainDataProvider);
                object item2Lotcheck = itemLotFacade.GetItem2LotCheck(item.ItemCode);
                if (item2Lotcheck == null)
                {
                    // Item2LOTCheck not exist
                    ApplicationRun.GetInfoForm().AddEx("$Error_NoItemSNCheckInfo $Domain_ItemCode:" + mo.ItemCode);
                    this.txtItem.Value = "";
                    this.txtMO.Value = "";
                    this.txtItemDesc.Value = "";
                    this.txtLotLetter.Value = "";
                    this.txtLotLetter.Checked = false;
                    this.txtGOMO.Checked = true;
                    this.txtGOMO.TextFocus(false, true);
                    return;
                }

                Item2LotCheck item2LOTcheck = item2Lotcheck as Item2LotCheck;
                this.txtItem.Value = item.ItemCode;
                this.txtItemDesc.Value = item.ItemDescription;

                SystemSettingFacade ssf = new SystemSettingFacade(this.DataProvider);

                object para = ssf.GetParameter("PRODUCTCODECONTROLSTATUS", "PRODUCTCODECONTROLSTATUS");

                if (item2LOTcheck.SNPrefix.Length != 0)
                {
                    this.txtLotLetter.Checked = true;
                    this.txtLotLetter.Value = item2LOTcheck.SNPrefix;
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

                if (item2LOTcheck.SNLength != 0)
                {
                    this.txtLotLen.Checked = true;
                    this.txtLotLen.Value = item2LOTcheck.SNLength.ToString();
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

                // end added
                this.txtGOMO.InnerTextBox.Enabled = false;
                txtLotCode.TextFocus(false, true);

            }
        }

        private void checkBoxAutoSaveErrorCode_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FCollectionGDNG_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_domainDataProvider != null)
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.CloseConnection();
            }

            ApplicationRun.GetQtyForm().Hide();
        }

        private void txtNGQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                txtLotCode.TextFocus(false, true);
            }
        }

        private void txtGoodQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                txtLotCode.TextFocus(false, true);
            }
        }

        private void cbxNGQty_Click(object sender, EventArgs e)
        {
            txtNGQty.InnerTextBox.Enabled = true;
            txtNGQty.Value = "0";
            txtGoodQty.InnerTextBox.Enabled = false;
            txtNGQty.TextFocus(false, true);
            cbxNGQty.Checked = true;
            cbxGoodQty.Checked = false;
        }

        private void cbxGoodQty_Click(object sender, EventArgs e)
        {
            txtGoodQty.InnerTextBox.Enabled = true;
            txtGoodQty.Value = "";
            txtNGQty.InnerTextBox.Enabled = false;
            txtGoodQty.TextFocus(false, true);
            cbxGoodQty.Checked = true;
            cbxNGQty.Checked = false;
        }

        private void txtErrorCode_TxtboxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                string errorCode = txtErrorCode.Value.Trim();
                int count = 0;
                foreach(UltraGridRow row in ultraGridErrorCode.Rows)
                {
                    if (row.Cells["ErrorQty"].Value.ToString().Trim() == string.Empty)
                    {
                        row.Cells["ErrorQty"].Value = 0;
                    }

                    if (errorCode == row.Cells["ECode"].Value.ToString())
                    {
                        row.Cells["ErrorQty"].Value = Int32.Parse(row.Cells["ErrorQty"].Value.ToString()) + 1;
                        count++;
                    }
                }
                if (count == 0)
                {
                    ApplicationRun.GetInfoForm().AddEx("$CS_Please_Enter_Related_ErrorCode");//�������벻�ڷ�Χ��
                }
            }
            txtErrorCode.Focus();
        }
    }
}
