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
using BenQGuru.eMES.Domain.TS;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.WebQuery
{
    /// <summary>
    /// FSMTNGListQP ��ժҪ˵����
    /// </summary>
    public partial class FSMTNGListQP : BaseQPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        protected ExcelExporter excelExporter = null;
        protected WebQueryHelperNew _helper = null;
        //protected GridHelper gridHelper = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //this.pagerSizeSelector.Readonly = true;

            this.txtSNQuery.Text = this.GetRequestParam("RunningCard");

            this.gridHelper = new GridHelperNew(this.gridWebGrid, DtSource);

            this._helper = new WebQueryHelperNew(null, this.cmdGridExport, this.gridWebGrid, this.pagerSizeSelector, this.pagerToolBar, this.languageComponent1, DtSource);
            this._helper.LoadGridDataSource += new EventHandler(_helper_LoadGridDataSource);
            this._helper.DomainObjectToGridRow += new EventHandler(_helper_DomainObjectToGridRow);
            this._helper.DomainObjectToExportRow += new EventHandler(_helper_DomainObjectToExportRow);
            this._helper.GetExportHeadText += new EventHandler(_helper_GetExportHeadText);

            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this._initialWebGrid();
                this._helper.Query(sender);
            }
        }

        private void _initialWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("ErrorCodeGroup", "����������", null);
            this.gridHelper.AddColumn("ErrorCodeGroupDescription", "��������������", null);
            this.gridHelper.AddColumn("ErrorCode", "��������", null);
            this.gridHelper.AddColumn("ErrorCodeDescription", "������������", null);
            this.gridHelper.AddColumn("ErrorLocation", "����λ��", null);
            this.gridHelper.AddColumn("ErrorSide", "����", null);
            this.gridHelper.AddColumn("CollectionDate", "�ɼ�����", null);
            this.gridHelper.AddColumn("CollectionTime", "�ɼ�ʱ��", null);

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
            manager.Add(new LengthCheck(this.lblRCardQuery, this.txtSNQuery, System.Int32.MaxValue, true));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            return true;
        }

        private void _helper_LoadGridDataSource(object sender, EventArgs e)
        {
            if (this._checkRequireFields())
            {
                FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
                (e as WebQueryEventArgsNew).GridDataSource =
                    facadeFactory.CreateQuerySMTNGFacade().QuerySMTNGDetails(
                    "",
                    FormatHelper.CleanString(this.txtSNQuery.Text).ToUpper(),
                    "",
                    "",
                    (e as WebQueryEventArgsNew).StartRow,
                    (e as WebQueryEventArgsNew).EndRow);

                (e as WebQueryEventArgsNew).RowCount =
                    facadeFactory.CreateQuerySMTNGFacade().QuerySMTNGDetailsCount(
                    "",
                    FormatHelper.CleanString(this.txtSNQuery.Text).ToUpper(),
                    "",
                    "");
            }
        }

        private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToGridRowEventArgsNew).DomainObject != null)
            {
                QSMTNGList obj = (e as DomainObjectToGridRowEventArgsNew).DomainObject as QSMTNGList;
                DataRow row = DtSource.NewRow();
                row["ErrorCodeGroup"] = obj.ErrorCodeGroup;
                row["ErrorCodeGroupDescription"] = obj.ErrorCodeGroupDesc;
                row["ErrorCode"] = obj.ErrorCode;
                row["ErrorCodeDescription"] = obj.ErrorCodeDesc;
                row["ErrorLocation"] = obj.ErrorLocation;
                row["ErrorSide"] = this.languageComponent1.GetString(obj.AB);
                row["CollectionDate"] = FormatHelper.ToDateString(obj.MaintainDate);
                row["CollectionTime"] = FormatHelper.ToTimeString(obj.MaintainTime);
                (e as DomainObjectToGridRowEventArgsNew).GridRow = row;

            }
        }

        private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToExportRowEventArgsNew).DomainObject != null)
            {
                QSMTNGList obj = (e as DomainObjectToExportRowEventArgsNew).DomainObject as QSMTNGList;
                (e as DomainObjectToExportRowEventArgsNew).ExportRow =
                    new string[]{
									obj.ErrorCodeGroup,
									obj.ErrorCodeGroupDesc,
									obj.ErrorCode,
									obj.ErrorCodeDesc,
									this.languageComponent1.GetString(obj.AB),
									FormatHelper.ToDateString(obj.MaintainDate),
									FormatHelper.ToTimeString(obj.MaintainTime)
								};
            }
        }

        private void _helper_GetExportHeadText(object sender, EventArgs e)
        {
            (e as ExportHeadEventArgsNew).Heads =
                new string[]{
								"ErrorCodeGroup",
								"ErrorCodeGroupDescription",
								"ErrorCode",
								"ErrorCodeDescription",
								"ErrorLocation",
								"ErrorSide",
								"CollectionDate",
								"CollectionTime"
							};
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect("FSMTNGQP.aspx");
        }
    }
}

