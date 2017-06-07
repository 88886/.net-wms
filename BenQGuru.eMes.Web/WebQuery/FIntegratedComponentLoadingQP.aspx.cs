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
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.DataCollect;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.WebQuery
{
    /// <summary>
    /// FIntegratedComponentLoadingQP ��ժҪ˵����
    /// </summary>
    public partial class FIntegratedComponentLoadingQP : BaseQPageNew
    {

        protected ExcelExporter excelExporter = null;
        protected WebQueryHelperNew _helper = null;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        //protected GridHelper gridHelper = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, DtSource);

            this._helper = new WebQueryHelperNew(this.cmdQuery, this.cmdGridExport, this.gridWebGrid, this.pagerSizeSelector, this.pagerToolBar, this.languageComponent1, this.DtSource);
            this._helper.LoadGridDataSource += new EventHandler(_helper_LoadGridDataSource);
            this._helper.DomainObjectToGridRow += new EventHandler(_helper_DomainObjectToGridRow);
            this._helper.DomainObjectToExportRow += new EventHandler(_helper_DomainObjectToExportRow);
            this._helper.GetExportHeadText += new EventHandler(_helper_GetExportHeadText);

            FormatHelper.SetSNRangeValue(txtStartSnQuery, txtEndSnQuery);

            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this._initialWebGrid();
            }
        }

        private void _initialWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("MOCode", "����", null);
            this.gridHelper.AddColumn("ItemCode", "��Ʒ����", null);
            this.gridHelper.AddColumn("SN", "��Ʒ���к�", null);
            this.gridHelper.AddColumn("ComponentLoadingOPCode", "���Ϲ�λ", null);
            this.gridHelper.AddColumn("ComponentLoadingStepSequenceCode", "�����߱�", null);
            this.gridHelper.AddColumn("ComponentLoadingResourceCode", "������Դ", null);
            this.gridHelper.AddColumn("PackedNo", "��С��װ����", null);
            this.gridHelper.AddColumn("EmployeeNo", "Ա������", null);
            this.gridHelper.AddColumn("ComponentLoadingDate", "��������", null);
            this.gridHelper.AddColumn("ComponentLoadingTime", "����ʱ��", null);
            this.gridHelper.AddLinkColumn("ComponentLoadingDetails", "������ϸ", null);

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

        private bool _checkRequireFields()
        {
            PageCheckManager manager = new PageCheckManager();
            manager.Add(new LengthCheck(this.lblMOIDQuery, this.txtConditionMo.TextBox, System.Int32.MaxValue, true));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            return true;
        }

        private void _helper_LoadGridDataSource(object sender, EventArgs e)
        {
            //�����к�ת��ΪSourceCode
            DataCollectFacade dataCollectfacade = new DataCollectFacade(this.DataProvider);
            //�������кŵ��������Ҫ����һ�´���
            string startRCard = FormatHelper.CleanString(this.txtStartSnQuery.Text.Trim().ToUpper());
            string endRCard = FormatHelper.CleanString(this.txtEndSnQuery.Text.Trim().ToUpper());
            //ת����SourceCard
            string startSourceCard = dataCollectfacade.GetSourceCard(startRCard, string.Empty);
            string endSourceCard = dataCollectfacade.GetSourceCard(endRCard, string.Empty);
            //end

            FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
            (e as WebQueryEventArgsNew).GridDataSource =
                facadeFactory.CreateQueryComponentLoadingFacade().QueryIntegratedLoading(
                FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
                FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
                FormatHelper.CleanString(this.txtMCard.Text).ToUpper(),
                FormatHelper.CleanString(startSourceCard).ToUpper(),
                FormatHelper.CleanString(endSourceCard).ToUpper(),
                (e as WebQueryEventArgsNew).StartRow,
                (e as WebQueryEventArgsNew).EndRow);

            (e as WebQueryEventArgsNew).RowCount =
                facadeFactory.CreateQueryComponentLoadingFacade().QueryIntegratedLoadingCount(
                FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
                FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
                FormatHelper.CleanString(this.txtMCard.Text).ToUpper(),
                FormatHelper.CleanString(startSourceCard).ToUpper(),
                FormatHelper.CleanString(endSourceCard).ToUpper());

        }

        private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToGridRowEventArgsNew).DomainObject != null)
            {

                QDOIntegrated obj = (e as DomainObjectToGridRowEventArgsNew).DomainObject as QDOIntegrated;
                DataRow row = DtSource.NewRow();
                row["MOCode"] = obj.MoCode;
                row["ItemCode"] = obj.ItemCode;
                row["SN"] = obj.SN;
                row["ComponentLoadingOPCode"] = obj.OperationCode;
                row["ComponentLoadingStepSequenceCode"] = obj.StepSequenceCode;
                row["ComponentLoadingResourceCode"] = obj.ResourceCode;
                row["PackedNo"] = obj.INNO;
                row["EmployeeNo"] = obj.MaintainUser;
                row["ComponentLoadingDate"] = FormatHelper.ToDateString(obj.MaintainDate);
                row["ComponentLoadingTime"] = FormatHelper.ToTimeString(obj.MaintainTime);
                (e as DomainObjectToGridRowEventArgsNew).GridRow = row;

            }
        }

        private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToExportRowEventArgsNew).DomainObject != null)
            {
                QDOIntegrated obj = (e as DomainObjectToExportRowEventArgsNew).DomainObject as QDOIntegrated;
                (e as DomainObjectToExportRowEventArgsNew).ExportRow =
                    new string[]{
                        obj.MoCode,
                        obj.ItemCode,
                        obj.SN,
                        obj.OperationCode,
                        obj.StepSequenceCode,
                        obj.ResourceCode,
                        obj.INNO,
                        obj.MaintainUser,
                        FormatHelper.ToDateString(obj.MaintainDate),
                        FormatHelper.ToTimeString(obj.MaintainTime)
                    };
            }
        }

        private void _helper_GetExportHeadText(object sender, EventArgs e)
        {
            (e as ExportHeadEventArgsNew).Heads =
                new string[]{
                    "MOCode",
                    "ItemCode",
                    "SN",
                    "ComponentLoadingOPCode",
                    "ComponentLoadingStepSequenceCode",
                    "ComponentLoadingResourceCode",
                    "PackedNo",
                    "EmployeeNo",
                    "ComponentLoadingDate",
                    "ComponentLoadingTime"
                };
        }

        protected override void Grid_ClickCell(GridRecord row, string command)
        {
            if (command == "ComponentLoadingDetails")
            {
                this.Response.Redirect(
                                   this.MakeRedirectUrl(
                                   "FIntegratedDetailsQP.aspx",
                                   new string[]{
                        "PackedNo",
                        "RETURNPAGEURL"
                    },
                                   new string[]{
                        row.Items.FindItemByKey("PackedNo").Text,
                        "FIntegratedComponentLoadingQP.aspx"
                    })
                               );
            }
        }


    }
}
