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
    /// FTSRecordDetailsQP ��ժҪ˵����
    /// </summary>
    public partial class FRMATSRecordDetailsQP : BaseQPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        protected ExcelExporter excelExporter = null;
        protected WebQueryHelperNew _helper = null;
        protected System.Web.UI.WebControls.TextBox txtRepaireOperationQuery;
        //protected GridHelper gridHelper = null;

        #region ViewState
        private int SourceResourceDate
        {
            get
            {
                if (this.ViewState["SourceResourceDate"] != null)
                {
                    try
                    {
                        return System.Int32.Parse(this.ViewState["SourceResourceDate"].ToString());
                    }
                    catch
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState["SourceResourceDate"] = value;
            }
        }

        private int SourceResourceTime
        {
            get
            {
                if (this.ViewState["SourceResourceTime"] != null)
                {
                    try
                    {
                        return System.Int32.Parse(this.ViewState["SourceResourceTime"].ToString());
                    }
                    catch
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState["SourceResourceTime"] = value;
            }
        }

        private string RunningCardSeq
        {
            get
            {
                if (this.ViewState["RunningCardSeq"] != null)
                {
                    try
                    {
                        return this.ViewState["RunningCardSeq"].ToString();
                    }
                    catch
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                this.ViewState["RunningCardSeq"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this._initialParamter();

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

        private void _initialParamter()
        {
            this.txtModelQuery.Text = this.GetRequestParam("ModelCode");
            this.txtItemQuery.Text = this.GetRequestParam("ItemCode");
            this.txtMoQuery.Text = this.GetRequestParam("MoCode");
            this.txtSnQuery.Text = this.GetRequestParam("RunningCard");
            this.txtTsStateQuery.Text = this.GetRequestParam("TSState");
            this.txtRepaireResourceQuery.Text = this.GetRequestParam("TSResourceCode");

            this.RunningCardSeq = this.GetRequestParam("RunningCardSeq");

            if (this.GetRequestParam("TSDate") != null)
            {
                string tsDate = this.GetRequestParam("TSDate");

                try
                {
                    this.SourceResourceDate = FormatHelper.TODateInt(tsDate);
                }
                catch
                {
                    this.SourceResourceDate = 0;
                }

                if (this.GetRequestParam("TSTime") != null)
                {
                    string tsTime = this.GetRequestParam("TSTime");

                    try
                    {
                        this.SourceResourceTime = FormatHelper.TOTimeInt(tsTime);
                    }
                    catch
                    {
                        this.SourceResourceTime = 0;
                    }
                }
            }
        }

        private void _initialWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("ErrorCodeGroup", "����������", null);
            this.gridHelper.AddColumn("ErrorCode", "��������", null);
            this.gridHelper.AddColumn("ErrorCause", "����ԭ��", null);
            this.gridHelper.AddColumn("ErrorLocation", "����λ��", null);
            this.gridHelper.AddColumn("ErrorParts", "����Ԫ��", null);
            this.gridHelper.AddColumn("Solution", "�������", null);
            this.gridHelper.AddColumn("Duty", "���α�", null);
            this.gridHelper.AddColumn("Memo", "����˵��", null);
            this.gridHelper.AddColumn("TsOperator", "ά�޹�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            //this.gridWebGrid.Columns.FromKey("ErrorCodeGroup").MergeCells = true;
            //this.gridWebGrid.Columns.FromKey("ErrorCode").MergeCells = true;
            //this.gridWebGrid.Columns.FromKey("ErrorCause").MergeCells = true;
            //this.gridWebGrid.Columns.FromKey("Solution").MergeCells = true;
            //this.gridWebGrid.Columns.FromKey("Duty").MergeCells = true;
            //this.gridWebGrid.Columns.FromKey("Memo").MergeCells = true;
            //this.gridWebGrid.Columns.FromKey("TsOperator").MergeCells = true;
            //this.gridWebGrid.Columns.FromKey("MaintainDate").MergeCells = true;
            //this.gridWebGrid.Columns.FromKey("MaintainTime").MergeCells = true;

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
            //			PageCheckManager manager = new PageCheckManager();
            //			manager.Add( new LengthCheck(this.lblModelQuery,this.txtModelQuery,System.Int32.MaxValue,true) );
            //			manager.Add( new LengthCheck(this.lblItemQuery,this.txtItemQuery,System.Int32.MaxValue,true) );
            //			manager.Add( new LengthCheck(this.lblMoQuery,this.txtMoQuery,System.Int32.MaxValue,true) );
            //			manager.Add( new LengthCheck(this.lblSnQuery,this.txtSnQuery,System.Int32.MaxValue,true) );
            //			manager.Add( new LengthCheck(this.lblTSStateQuery,this.txtTsStateQuery,System.Int32.MaxValue,true) );
            //			manager.Add( new LengthCheck(this.lblRepaireOperationQuery,this.txtRepaireResourceQuery,System.Int32.MaxValue,true) );
            //
            //			if( !manager.Check() )
            //			{
            //				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
            //				return true;
            //			}	
            return true;
        }

        private void _helper_LoadGridDataSource(object sender, EventArgs e)
        {
            if (this._checkRequireFields())
            {
                FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
                (e as WebQueryEventArgsNew).GridDataSource =
                    facadeFactory.CreateQueryTSDetailsFacade().QueryTSDetails(
                    FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper(),
                    FormatHelper.CleanString(this.txtSnQuery.Text).ToUpper(),
                    int.Parse(this.RunningCardSeq),
                    (e as WebQueryEventArgsNew).StartRow,
                    (e as WebQueryEventArgsNew).EndRow);

                (e as WebQueryEventArgsNew).RowCount =
                    facadeFactory.CreateQueryTSDetailsFacade().QueryTSDetailsCount(
                    FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper(),
                    FormatHelper.CleanString(this.txtSnQuery.Text).ToUpper(),
                    int.Parse(this.RunningCardSeq));

            }
        }

        private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToGridRowEventArgsNew).DomainObject != null)
            {
                QDOTSDetails1 obj = (e as DomainObjectToGridRowEventArgsNew).DomainObject as QDOTSDetails1;
                DataRow row = DtSource.NewRow();
                row["ErrorCodeGroup"] = obj.ErrorCodeGroupDescription;
                row["ErrorCode"] = obj.ErrorCodeDescription;
                row["ErrorCause"] = obj.ErrorCauseDescription;
                row["ErrorLocation"] = obj.ErrorLocation;
                row["ErrorParts"] = obj.ErrorParts;
                row["Solution"] = obj.SolutionDescription;
                row["Duty"] = obj.DutyDescription;
                row["Memo"] = obj.Memo;
                row["TsOperator"] = obj.TSOperator;
                row["MaintainDate"] = FormatHelper.ToDateString(obj.TSDate);
                row["MaintainTime"] = FormatHelper.ToTimeString(obj.TSTime);
                (e as DomainObjectToGridRowEventArgsNew).GridRow = row;

            }
        }

        private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToExportRowEventArgsNew).DomainObject != null)
            {
                QDOTSDetails1 obj = (e as DomainObjectToExportRowEventArgsNew).DomainObject as QDOTSDetails1;
                (e as DomainObjectToExportRowEventArgsNew).ExportRow =
                    new string[]{
									obj.ErrorCodeGroupDescription,
									obj.ErrorCodeDescription,
									obj.ErrorCauseDescription,
									obj.ErrorLocation,
									obj.ErrorParts,
									obj.SolutionDescription,
									obj.DutyDescription,													  
									obj.Memo,
									obj.TSOperator,
									FormatHelper.ToDateString(obj.TSDate),
									FormatHelper.ToTimeString(obj.TSTime)
								};
            }
        }

        private void _helper_GetExportHeadText(object sender, EventArgs e)
        {
            (e as ExportHeadEventArgsNew).Heads =
                new string[]{
								"ErrorCodeGroup",
								"ErrorCode",
								"ErrorCause",
								"ErrorLocation",
								"ErrorParts",
								"Solution",
								"Duty",
								"Memo",
								"TsOperator",
								"MaintainDate",
								"MaintainTime"
							};
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            ArrayList keys = new ArrayList();
            ArrayList values = new ArrayList();

            for (int i = 0; i < this.Request.QueryString.AllKeys.Length; i++)
            {
                if (this.Request.QueryString.AllKeys.GetValue(i).ToString().StartsWith("12_"))
                {
                    keys.Add(this.Request.QueryString.AllKeys.GetValue(i).ToString());
                    values.Add(this.Request.QueryString[this.Request.QueryString.AllKeys.GetValue(i).ToString()]);
                }
            }

            this.Response.Redirect(
                this.MakeRedirectUrl(
                this.GetRequestParam("BackUrl"), (string[])keys.ToArray(typeof(string)), (string[])values.ToArray(typeof(string))));
        }
    }
}
