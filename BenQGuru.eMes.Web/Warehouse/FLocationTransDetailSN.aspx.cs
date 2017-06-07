using System;
using System.Data;

using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FLocationTransDetailSN : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private WarehouseFacade _WarehouseFacade = null;

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
            // 
            // languageComponent1
            // 
            this.languageComponent1.Language = "CHS";
            this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
            this.languageComponent1.RuntimePage = null;
            this.languageComponent1.RuntimeUserControl = null;
            this.languageComponent1.UserControlName = "";

        }
        #endregion

        #region Init

        protected void Page_Load(object sender, System.EventArgs e)
        {
            InitHander();
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                this.txtLocationNoQuery.Text = Request.QueryString["LOCATIONNO"];
                this.txtLocationNoQuery.Enabled = false;
                InitWebGrid();
                this.cmdQuery_Click(null, null);
                this.RequestData();
            }
        }

        #region Ĭ�ϲ�ѯ
        private void RequestData()
        {

            this.pagerToolBar.PageSize = this.pagerSizeSelector.PageSize;
            this.gridHelper.GridBind(PageGridBunding.Page, this.pagerSizeSelector.PageSize);

            this.pagerToolBar.RowCount = GetRowCount();
            this.pagerToolBar.InitPager();
        }
        private void InitHander()
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
            this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadDataSource);
            this.gridHelper.GetRowCountHandle = new GetRowCountDelegateNew(this.GetRowCount);
            this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridRow);

            this.buttonHelper = new ButtonHelper(this);

            this.pagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);


            #region Exporter
            this.excelExporter = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
            this.excelExporter.Page = this;
            this.excelExporter.LanguageComponent = this.languageComponent1;
            this.excelExporter.LoadExportDataHandle = new LoadExportDataDelegate(LoadDataSource);
            this.excelExporter.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatExportRecord);
            this.excelExporter.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetColumnHeaderText);
            #endregion

        }
        private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
        {
            this.gridHelper.GridBind(this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize);
        }

        #endregion

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("LocStorTransNo", "��λ�ƶ�����", null);
            this.gridHelper.AddColumn("DQMcode", "�������ϱ���", null);
            this.gridHelper.AddColumn("Mdesc", "��������", null);
            this.gridHelper.AddColumn("RequireQty", "��������", null);
            this.gridHelper.AddColumn("SN", "SN", null);

            this.gridHelper.AddDefaultColumn(false, false);
            
            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();

            row["LocStorTransNo"] = ((StorloctransDetailEX)obj).Transno;
            row["DQMcode"] = ((StorloctransDetailEX)obj).DqmCode;
            row["Mdesc"] = ((StorloctransDetailEX)obj).MDesc;
            row["RequireQty"] = ((StorloctransDetailEX)obj).Qty;
            row["SN"] = ((StorloctransDetailEX)obj).Sn;

            return row;

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }

            return this._WarehouseFacade.QueryStorloctransDetailSN(
           FormatHelper.CleanString(this.txtLocationNoQuery.Text),
           Request.QueryString["FCARTON"],
            Request.QueryString["TCARTON"],
           inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            return this._WarehouseFacade.QueryStorloctransDetailSNCount(
                  FormatHelper.CleanString(this.txtLocationNoQuery.Text),
                  Request.QueryString["FCARTON"],
                  Request.QueryString["TCARTON"]
                  );
        }

        #endregion
        
        #region Button

        //����
        protected void cmdReturn_ServerClick(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
            {
                Response.Redirect(this.MakeRedirectUrl(Request.QueryString["Page"], new string[] { "LOCATIONNO", "MCODE" }, new string[] { this.txtLocationNoQuery.Text, Request.QueryString["MCODE"] }));
            }
            else
            {
                Response.Redirect(this.MakeRedirectUrl("FLocationTransDetailCarton.aspx", new string[] { "LOCATIONNO", "MCODE" }, new string[] { this.txtLocationNoQuery.Text, Request.QueryString["MCODE"] }));
            }
        }

        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                                ((StorloctransDetailEX)obj).Transno,
                                ((StorloctransDetailEX)obj).DqmCode,
                                 ((StorloctransDetailEX)obj).MDesc,
                                 ((StorloctransDetailEX)obj).Qty.ToString("G0"),
                                ((StorloctransDetailEX)obj).Sn
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[]
                {
                                    "LocationNo",
                                    "DQMcode",
                                    "Mdesc",
                                    "RequireQty",
                                    "SN"                                   
                                   
                };
        }

        #endregion
    }
}
