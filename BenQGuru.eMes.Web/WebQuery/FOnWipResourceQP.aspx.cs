using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.WebQuery
{
    /// <summary>
    /// FOnWipResourceQP ��ժҪ˵����
    /// </summary>
    public partial class FOnWipResourceQP : BaseQPageNew
    {

        protected WebQueryHelperNew _helper = null;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        //protected GridHelper gridHelper = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, DtSource);
            this._helper = new WebQueryHelperNew(null, this.cmdGridExport, this.gridWebGrid, this.pagerSizeSelector, this.pagerToolBar, this.languageComponent1, DtSource);
            this._helper.LoadGridDataSource += new EventHandler(_helper_LoadGridDataSource);
            this._helper.DomainObjectToGridRow += new EventHandler(_helper_DomainObjectToGridRow);
            this._helper.DomainObjectToExportRow += new EventHandler(_helper_DomainObjectToExportRow);
            this._helper.GetExportHeadText += new EventHandler(_helper_GetExportHeadText);
            //this._helper.GridCellClick +=new EventHandler(_helper_GridCellClick);

            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                /* added by jessie lee, 2005/12/30,
                 * �����Ʒֲ�ҳ���жϹ����Ƿ�ΪFQC���� */
                bool isFQC = false;
                if (string.Compare(this.GetRequestParam("OperationCode"), "TS", true) != 0)
                {
                    FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
                    isFQC = facadeFactory.CreateQueryFacade3().IsOpFQC(
                        this.GetRequestParam("OperationCode"),
                        this.GetRequestParam("ItemCode"),
                        this.GetRequestParam("MoCode"),
                        FormatHelper.TODateInt(this.GetRequestParam("STARTDATE")),
                        FormatHelper.TODateInt(this.GetRequestParam("ENDDATE")));
                }
                this.ViewState["IsFQC"] = isFQC;

                this._initialWebGrid();

                this.txtItemCodeQuery.Text = this.GetRequestParam("ItemCode");
                this.txtMoCodeQuery.Text = this.GetRequestParam("MoCode");
                this.txtOperationCodeQuery.Text = this.GetRequestParam("OperationCode");
                this.txtStartDate.Value = this.GetRequestParam("STARTDATE");
                this.txtEndDate.Value = this.GetRequestParam("ENDDATE");

                this._helper.Query(sender);

            }
        }

        private void _initialWebGrid()
        {
            base.InitWebGrid();
            if (string.Compare(this.GetRequestParam("OperationCode").ToString(), "TS", true) != 0)
            {
                this.gridHelper.AddColumn("SegmentCode", "����", null);
                this.gridHelper.AddColumn("ShiftDay", "����", null);
                this.gridHelper.AddColumn("ShiftCode", "���", null);
                this.gridHelper.AddColumn("StepSequenceCode", "������", null);
                this.gridHelper.AddColumn("ResourceCode", "��Դ", null);
                this.gridHelper.AddColumn("MoCode", "����", null);
                this.gridHelper.AddColumn("OnWipGoodQuantityOnResource", "������Ʒ����", null);
                this.gridHelper.AddLinkColumn("OnWipGoodDistributing", "������Ʒ��ϸ", null);
                this.gridHelper.AddColumn("OnWipNGQuantityOnResource", "���Ʋ���Ʒ����", null);
                this.gridHelper.AddLinkColumn("OnWipNGDistributing", "���Ʋ���Ʒ��ϸ", null);

                if (bool.Parse(this.ViewState["IsFQC"].ToString()))
                {
                    this.gridHelper.AddColumn("OQCNGWaitForRework", "���˴���������", null);
                    this.gridHelper.AddLinkColumn("OQCNGWaitForReworkDistributing", "���˴�����������ϸ", null);
                    //this.gridWebGrid.Bands[0].Columns.FromKey("OQCNGWaitForReworkDistributing").Width = new Unit(60);
                }

                //this.gridWebGrid.Bands[0].Columns.FromKey("OnWipGoodDistributing").Width = new Unit(60);
                //this.gridWebGrid.Bands[0].Columns.FromKey("OnWipNGDistributing").Width = new Unit(60);
            }
            else
            {
                /* ά�� */
                this.gridHelper.AddColumn("SegmentCode", "����", null);
                this.gridHelper.AddColumn("ShiftDay", "����", null);
                this.gridHelper.AddColumn("ShiftCode", "���", null);
                this.gridHelper.AddColumn("StepSequenceCode", "������", null);
                this.gridHelper.AddColumn("ResourceCode", "��Դ", null);
                this.gridHelper.AddColumn("MoCode", "����", null);

                this.gridHelper.AddColumn("TSConfirmQty", "��������", null);
                this.gridHelper.AddLinkColumn("TSConfirmQtyDistributing", "����������ϸ", null);
                //this.gridWebGrid.Bands[0].Columns.FromKey("TSConfirmQtyDistributing").Width = new Unit(60);

                this.gridHelper.AddColumn("TSQty", "ά��������", null);
                this.gridHelper.AddLinkColumn("TSQtyDistributing", "ά����������ϸ", null);
                //this.gridWebGrid.Bands[0].Columns.FromKey("TSQtyDistributing").Width = new Unit(60);

                this.gridHelper.AddColumn("TSReflowQty", "����������", null);
                this.gridHelper.AddLinkColumn("TSReflowQtyDistributing", "����������", null);
                //this.gridWebGrid.Bands[0].Columns.FromKey("TSReflowQtyDistributing").Width = new Unit(60);

            }

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

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
            this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
            this.languageComponent1.RuntimePage = null;
            this.languageComponent1.RuntimeUserControl = null;
            this.languageComponent1.UserControlName = "";

        }
        #endregion

        private void _helper_LoadGridDataSource(object sender, EventArgs e)
        {
            FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
            (e as WebQueryEventArgsNew).GridDataSource = facadeFactory.CreateQueryFacade3().QueryOnWipInfoOnResource(
                this.txtItemCodeQuery.Text,
                this.txtMoCodeQuery.Text,
                this.txtOperationCodeQuery.Text,
                FormatHelper.TODateInt(this.txtStartDate.Value),
                FormatHelper.TODateInt(this.txtEndDate.Value),
                (e as WebQueryEventArgsNew).StartRow,
                (e as WebQueryEventArgsNew).EndRow);

            (e as WebQueryEventArgsNew).RowCount =
                facadeFactory.CreateQueryFacade3().QueryOnWipInfoOnResourceCount(
                this.txtItemCodeQuery.Text,
                this.txtMoCodeQuery.Text,
                this.txtOperationCodeQuery.Text,
                FormatHelper.TODateInt(this.txtStartDate.Value),
                FormatHelper.TODateInt(this.txtEndDate.Value));
        }

        private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToGridRowEventArgsNew).DomainObject != null)
            {
                if (string.Compare(this.GetRequestParam("OperationCode").ToString(), "TS", true) != 0)
                {
                    if (bool.Parse(this.ViewState["IsFQC"].ToString()))
                    {
                        /* FQC */
                        OnWipInfoOnResource obj = (e as DomainObjectToGridRowEventArgsNew).DomainObject as OnWipInfoOnResource;
                        DataRow row = DtSource.NewRow();
                        row["SegmentCode"] = obj.SegmentCode;
                        row["ShiftDay"] = FormatHelper.ToDateString(obj.ShiftDay);
                        row["ShiftCode"] = obj.ShiftCode;
                        row["StepSequenceCode"] = obj.StepSequenceCode;
                        row["ResourceCode"] = obj.ResourceCode;
                        row["MoCode"] = obj.MoCode;
                        row["OnWipGoodQuantityOnResource"] = obj.OnWipGoodQuantityOnResource;
                        row["OnWipGoodDistributing"] = "";
                        row["OnWipNGQuantityOnResource"] = obj.OnWipNGQuantityOnResource;
                        row["OnWipNGDistributing"] = "";
                        row["OQCNGWaitForRework"] = obj.NGForReworksQTY;
                        row["OQCNGWaitForReworkDistributing"] = "";
                        (e as DomainObjectToGridRowEventArgsNew).GridRow = row;
                    }
                    else
                    {
                        /* ��ͨ */
                        OnWipInfoOnResource obj = (e as DomainObjectToGridRowEventArgsNew).DomainObject as OnWipInfoOnResource;
                        DataRow row = DtSource.NewRow();
                        row["SegmentCode"] = obj.SegmentCode;
                        row["ShiftDay"] = FormatHelper.ToDateString(obj.ShiftDay);
                        row["ShiftCode"] = obj.ShiftCode;
                        row["StepSequenceCode"] = obj.StepSequenceCode;
                        row["ResourceCode"] = obj.ResourceCode;
                        row["MoCode"] = obj.MoCode;

                        row["OnWipGoodQuantityOnResource"] = obj.OnWipGoodQuantityOnResource;
                        row["OnWipGoodDistributing"] = "";
                        row["OnWipNGQuantityOnResource"] = obj.OnWipNGQuantityOnResource;
                        row["OnWipNGDistributing"] = "";

                        (e as DomainObjectToGridRowEventArgsNew).GridRow = row;
                          
                    }
                }
                else
                {
                    /* ά�� */
                    OnWipInfoOnResource obj = (e as DomainObjectToGridRowEventArgsNew).DomainObject as OnWipInfoOnResource;
                    DataRow row = DtSource.NewRow();
                    row["SegmentCode"] = obj.SegmentCode;
                    row["ShiftDay"] = FormatHelper.ToDateString(obj.ShiftDay);
                    row["ShiftCode"] = obj.ShiftCode;
                    row["StepSequenceCode"] = obj.StepSequenceCode;
                    row["ResourceCode"] = obj.ResourceCode;
                    row["MoCode"] = obj.MoCode;
                    row["TSConfirmQty"] = obj.TSConfirmQty;
                    row["TSConfirmQtyDistributing"] = "";
                    row["TSQty"] = obj.TSQty;
                    row["TSQtyDistributing"] = "";
                    row["TSReflowQty"] = obj.TSReflowQty;
                    row["TSReflowQtyDistributing"] = "";

                    (e as DomainObjectToGridRowEventArgsNew).GridRow = row;
                }
            }
        }

        private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToExportRowEventArgsNew).DomainObject != null)
            {
                if (string.Compare(this.GetRequestParam("OperationCode").ToString(), "TS", true) != 0)
                {
                    if (bool.Parse(this.ViewState["IsFQC"].ToString()))
                    {
                        /* FQC */
                        OnWipInfoOnResource obj = (e as DomainObjectToExportRowEventArgsNew).DomainObject as OnWipInfoOnResource;
                        (e as DomainObjectToExportRowEventArgsNew).ExportRow =
                            new string[]{
											this.txtItemCodeQuery.Text,
											this.txtOperationCodeQuery.Text,
											obj.SegmentCode,
											FormatHelper.ToDateString( obj.ShiftDay ),
											obj.ShiftCode,
											obj.StepSequenceCode,
											obj.ResourceCode,
											obj.MoCode,
											obj.OnWipGoodQuantityOnResource.ToString(),
											obj.OnWipNGQuantityOnResource.ToString(),
											obj.NGForReworksQTY.ToString()
										};
                    }
                    else
                    {
                        /* ��ͨ */
                        OnWipInfoOnResource obj = (e as DomainObjectToExportRowEventArgsNew).DomainObject as OnWipInfoOnResource;
                        (e as DomainObjectToExportRowEventArgsNew).ExportRow =
                            new string[]{
											this.txtItemCodeQuery.Text,
											this.txtOperationCodeQuery.Text,
											obj.SegmentCode,
											FormatHelper.ToDateString( obj.ShiftDay ),
											obj.ShiftCode,
											obj.StepSequenceCode,
											obj.ResourceCode,
											obj.MoCode,
											obj.OnWipGoodQuantityOnResource.ToString(),
											obj.OnWipNGQuantityOnResource.ToString()
										};
                    }
                }
                else
                {
                    /* ά�� */
                    OnWipInfoOnResource obj = (e as DomainObjectToExportRowEventArgsNew).DomainObject as OnWipInfoOnResource;
                    (e as DomainObjectToExportRowEventArgsNew).ExportRow =
                        new string[]{
										this.txtItemCodeQuery.Text,
										this.txtOperationCodeQuery.Text,
										obj.SegmentCode,
										FormatHelper.ToDateString( obj.ShiftDay ),
										obj.ShiftCode,
										obj.StepSequenceCode,
										obj.ResourceCode,
										obj.MoCode,
										obj.TSConfirmQty.ToString(),
										obj.TSQty.ToString(),
										obj.TSReflowQty.ToString()
									};
                }
            }
        }

        private void _helper_GetExportHeadText(object sender, EventArgs e)
        {
            if (string.Compare(this.GetRequestParam("OperationCode").ToString(), "TS", true) != 0)
            {
                if (bool.Parse(this.ViewState["IsFQC"].ToString()))
                {
                    /* FQC */
                    (e as ExportHeadEventArgsNew).Heads =
                        new string[]{
										"ItemCode",
										"OperationCode",
										"SegmentCode",
										"ShiftDay",
										"ShiftCode",
										"StepSequenceCode",
										"ResourceCode",
										"MOCode",
										"OnWipGoodQuantityOnResource",
										"OnWipNGQuantityOnResource",
										"OQCNGWaitForRework"
									};
                }
                else
                {
                    /* Common */
                    (e as ExportHeadEventArgsNew).Heads =
                        new string[]{
										"ItemCode",
										"OperationCode",
										"SegmentCode",
										"ShiftDay",
										"ShiftCode",
										"StepSequenceCode",
										"ResourceCode",
										"MOCode",
										"OnWipGoodQuantityOnResource",
										"OnWipNGQuantityOnResource"
									};
                }
            }
            else
            {
                /* TS */
                (e as ExportHeadEventArgsNew).Heads =
                    new string[]{
									"ItemCode",
									"OperationCode",
									"SegmentCode",
									"ShiftDay",
									"ShiftCode",
									"StepSequenceCode",
									"ResourceCode",
									"MOCode",
									"TSConfirmQty",
									"TSQty",
									"TSReflowQty"
								};
            }
        }

        protected override void Grid_ClickCell(GridRecord row, string command)
        {

            if (command == "OnWipGoodDistributing")/* ������Ʒ��ϸ */
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FOnWipDistributingQP.aspx",
                    new string[]{
									"Status",
									"ItemCode",
									"ShiftDay",
									"ShiftCode",
									"OperationCode",
									"ResourceCode",
									"MoCode",
									"StartDate",
									"EndDate" ,
									"MoCodes" ,
									"IsFQC"
								},
                    new string[]{
									"GOOD",
									this.txtItemCodeQuery.Text,
									row.Items.FindItemByKey("ShiftDay").Text,
									row.Items.FindItemByKey("ShiftCode").Text,
									this.txtOperationCodeQuery.Text,
									row.Items.FindItemByKey("ResourceCode").Text,
									row.Items.FindItemByKey("MoCode").Text,
									this.txtStartDate.Value,
									this.txtEndDate.Value,
									this.txtMoCodeQuery.Text,
									this.ViewState["IsFQC"].ToString()
									
				})
                    );
            }
            else if (command == "OnWipNGDistributing")/* ���Ʋ���Ʒ��ϸ */
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FOnWipDistributingQP.aspx",
                    new string[]{
									"Status",
									"ItemCode",
									"ShiftDay",
									"ShiftCode",
									"OperationCode",
									"ResourceCode",
									"MoCode",
									"StartDate",
									"EndDate" ,
									"MoCodes",
									"IsFQC"
								},
                    new string[]{
									"NG",
									this.txtItemCodeQuery.Text,
									row.Items.FindItemByKey("ShiftDay").Text,
									row.Items.FindItemByKey("ShiftCode").Text,
									this.txtOperationCodeQuery.Text,
									row.Items.FindItemByKey("ResourceCode").Text,
									row.Items.FindItemByKey("MoCode").Text,
									this.txtStartDate.Value,
									this.txtEndDate.Value,
									this.txtMoCodeQuery.Text,
									this.ViewState["IsFQC"].ToString()
								})
                    );
            }
            else if (command == "OQCNGWaitForReworkDistributing") /* Reject */
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FOnWipDistributingQP.aspx",
                    new string[]{
									"Status",
									"ItemCode",
									"ShiftDay",
									"ShiftCode",
									"OperationCode",
									"ResourceCode",
									"MoCode",
									"StartDate",
									"EndDate" ,
									"MoCodes" ,
									"IsFQC"
								},
                    new string[]{
									"REJECT",
									this.txtItemCodeQuery.Text,
									row.Items.FindItemByKey("ShiftDay").Text,
									row.Items.FindItemByKey("ShiftCode").Text,
									this.txtOperationCodeQuery.Text,
									row.Items.FindItemByKey("ResourceCode").Text,
									row.Items.FindItemByKey("MoCode").Text,
									this.txtStartDate.Value,
									this.txtEndDate.Value,
									this.txtMoCodeQuery.Text,
									this.ViewState["IsFQC"].ToString()
								})
                    );
            }
            else if (command == "TSConfirmQtyDistributing") /* TS Confirm */
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FOnWipDistributingQP.aspx",
                    new string[]{
									"Status",
									"ItemCode",
									"ShiftDay",
									"ShiftCode",
									"OperationCode",
									"ResourceCode",
									"MoCode",
									"StartDate",
									"EndDate" ,
									"MoCodes" ,
									"IsFQC"
								},
                    new string[]{
									TSStatus.TSStatus_Confirm,
									this.txtItemCodeQuery.Text,
									row.Items.FindItemByKey("ShiftDay").Text,
									row.Items.FindItemByKey("ShiftCode").Text,
									this.txtOperationCodeQuery.Text,
									row.Items.FindItemByKey("ResourceCode").Text,
									row.Items.FindItemByKey("MoCode").Text,
									this.txtStartDate.Value,
									this.txtEndDate.Value,
									this.txtMoCodeQuery.Text,
									this.ViewState["IsFQC"].ToString()
									
								})
                    );
            }
            else if (command == "TSQtyDistributing")/* TS */
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FOnWipDistributingQP.aspx",
                    new string[]{
									"Status",
									"ItemCode",
									"ShiftDay",
									"ShiftCode",
									"OperationCode",
									"ResourceCode",
									"MoCode",
									"StartDate",
									"EndDate" ,
									"MoCodes",
									"IsFQC"
								},
                    new string[]{
									TSStatus.TSStatus_TS,
									this.txtItemCodeQuery.Text,
									row.Items.FindItemByKey("ShiftDay").Text,
									row.Items.FindItemByKey("ShiftCode").Text,
									this.txtOperationCodeQuery.Text,
								    row.Items.FindItemByKey("ResourceCode").Text,
									row.Items.FindItemByKey("MoCode").Text,
									this.txtStartDate.Value,
									this.txtEndDate.Value,
									this.txtMoCodeQuery.Text,
									this.ViewState["IsFQC"].ToString()
								})
                    );
            }
            else if (command == "TSReflowQtyDistributing")/* TS Reflow */
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FOnWipDistributingQP.aspx",
                    new string[]{
									"Status",
									"ItemCode",
									"ShiftDay",
									"ShiftCode",
									"OperationCode",
									"ResourceCode",
									"MoCode",
									"StartDate",
									"EndDate" ,
									"MoCodes" ,
									"IsFQC"
								},
                    new string[]{
									TSStatus.TSStatus_Reflow,
									this.txtItemCodeQuery.Text,
									row.Items.FindItemByKey("ShiftDay").Text,
									row.Items.FindItemByKey("ShiftCode").Text,
									this.txtOperationCodeQuery.Text,
									row.Items.FindItemByKey("ResourceCode").Text,
									row.Items.FindItemByKey("MoCode").Text,
									this.txtStartDate.Value,
									this.txtEndDate.Value,
									this.txtMoCodeQuery.Text,
									this.ViewState["IsFQC"].ToString()
								})
                    );
            }
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("FOnWipQP.aspx"));
        }
    }
}
