#region system;
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
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.Rework;
using BenQGuru.eMES.Rework;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
#endregion

namespace BenQGuru.eMES.Web.Rework
{
    /// <summary>
    /// FReworkApproveResultMP ��ժҪ˵����
    /// </summary>
    public partial class FReworkApproveResultMP : BaseMPageMinus
    {
        protected System.Web.UI.HtmlControls.HtmlTableCell tdExport;
        protected System.Web.UI.HtmlControls.HtmlInputButton cmdGridExport;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdDelete;
        protected System.Web.UI.HtmlControls.HtmlInputButton cmdDelete;

        //protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        //private BenQGuru.eMES.Web.Helper.GridHelper gridHelper = null;
        private BenQGuru.eMES.Web.Helper.ButtonHelper buttonHelper = null;
        private BenQGuru.eMES.Rework.ReworkFacade _facade;//= ReworkFacadeFactory.Create();

        private string reworkStatus;


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
            //this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
            // 
            // languageComponent1
            // 
            //this.languageComponent1.Language = "CHS";
            //this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
            //this.languageComponent1.RuntimePage = null;
            //this.languageComponent1.RuntimeUserControl = null;
            //this.languageComponent1.UserControlName = "";

        }
        #endregion

        #region Init
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
            try
            {
                object reworkSheet = this._facade.GetReworkSheet(this.GetRequestParam("reworkcode"));
                if (reworkSheet != null)
                {
                    this.reworkStatus = ((ReworkSheet)reworkSheet).Status;
                }
            }
            catch
            {
                ExceptionManager.Raise(this.GetType(), "$Error_RequestUrlParameter_Lost");
            }

            this.InitOnPostBack();

            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this.InitButton();
                this.InitWebGrid();
                InitParamterts();
                InitViewPanel();

            }

        }

        private void InitParamterts()
        {
            if (this.Request.Params["reworkcode"] == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_RequestUrlParameter_Lost");
            }
            this.ReworkCode = Request.Params["reworkcode"];

        }
        private void InitViewPanel()
        {
            if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
            this.txtReworkCodeQuery.Text = ReworkCode;
            this.txtReworkCodeQuery.ReadOnly = true;
            ReworkSheet rs = (ReworkSheet)this._facade.GetReworkSheet(ReworkCode);
            if (rs == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_ReworkCode_Invalid");
            }
            this.txtMOCode.Text = rs.MOCode;
            this.RequestData();
        }

        public string ReworkCode
        {
            get
            {
                return (string)this.ViewState["reworkcode"];
            }
            set
            {
                this.ViewState["reworkcode"] = value;
            }
        }

        private int ReworkSeq
        {
            get
            {
                try
                {
                    return int.Parse(ViewState["reworkSeq"].ToString());
                }
                catch
                {
                    return 0;
                }
            }

            set
            {
                ViewState["reworkSeq"] = value;
            }

        }

        private void InitOnPostBack()
        {
            this.buttonHelper = new ButtonHelper(this);
            this.buttonHelper.SetEditObjectHandle = new SetEditObjectDelegate(this.SetEditObject);
            this.buttonHelper.AfterPageStatusChangeHandle = new PageStatusChangeDelegate(this.buttonHelper_AfterPageStatusChangeHandle);

            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
            this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadDataSource);
            this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridRow);

            this.pagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);

        }

        private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
        {
            this.gridHelper.GridBind(this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize);
        }

        private void InitButton()
        {
            this.buttonHelper.PageActionStatusHandle(PageActionType.Add);
            this.buttonHelper.AddDeleteConfirm();
        }
        protected void drpApproveStatusQuery_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                this.drpApproveStatusQuery.Items.Clear();

                this.drpApproveStatusQuery.Items.Add(
                    new ListItem(this.languageComponent1.GetLanguage(ApproveStatus.APPROVESTATUS_WAITING_STRING).ControlText, ApproveStatus.APPROVESTATUS_WAITING.ToString())
                    );

                this.drpApproveStatusQuery.Items.Add(
                    new ListItem(this.languageComponent1.GetLanguage(ApproveStatus.APPROVESTATUS_PASSED_STRING).ControlText, ApproveStatus.APPROVESTATUS_PASSED.ToString())
                    );

                this.drpApproveStatusQuery.Items.Add(
                    new ListItem(this.languageComponent1.GetLanguage(ApproveStatus.APPROVESTATUS_NOPASSED_STRING).ControlText, ApproveStatus.APPROVESTATUS_NOPASSED.ToString())
                    );

                new DropDownListBuilder(drpApproveStatusQuery).AddAllItem(this.languageComponent1);

            }
        }

        #endregion

        #region WebGrid
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("ReworkPassSequence", "ǩ�˲㼶", null);
            this.gridHelper.AddColumn("PassContent", "ǩ�����", null);
            this.gridHelper.AddColumn("ReworkStatus", "ǩ��״̬", null);
            this.gridHelper.AddColumn("ReworkUserCode", "ǩ����", null);
            this.gridHelper.AddColumn("ReworkDepartment", "ǩ�˲���", null);
            this.gridHelper.AddColumn("MaintainDate", "����", null);
            this.gridHelper.AddColumn("MaintainTime", "ʱ��", null);

            this.gridHelper.AddDefaultColumn(false, false);

            this.gridHelper.ApplyLanguage(this.languageComponent1);


        }

        protected DataRow GetGridRow(object obj)
        {
            DataRow row = DtSource.NewRow();
            row["ReworkPassSequence"] = ((ReworkPassEx)obj).PassSequence.ToString();
            row["PassContent"] = ((ReworkPassEx)obj).PassContent.ToString();
            row["ReworkStatus"] = this.GetPassStatusString(((ReworkPassEx)obj));
            row["ReworkUserCode"] = ((ReworkPassEx)obj).GetDisplayText("UserCode");
            row["ReworkDepartment"] = ((ReworkPassEx)obj).UserDepartment.ToString();
            row["MaintainDate"] = FormatHelper.ToDateString(((ReworkPassEx)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((ReworkPassEx)obj).MaintainTime);
            return row;
        }

        private object[] LoadDataSource(int inclusive, int exclusive)
        {
            // TODO: ʹ������֮����ֶβ�ѯ����ĺ�̨Facade��Query����
            if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
            string approveStatusQuery;

            if (this.reworkStatus == ReworkStatus.REWORKSTATUS_NEW)
            {
                if (this.drpApproveStatusQuery.SelectedValue == string.Empty
                    || this.drpApproveStatusQuery.SelectedValue == ApproveStatus.APPROVESTATUS_WAITING.ToString())
                {
                    approveStatusQuery = string.Empty;
                }
                else
                {
                    approveStatusQuery = "-1";
                }

            }
            else
            {
                approveStatusQuery = this.drpApproveStatusQuery.SelectedValue; ;
            }
            return this._facade.QueryReworkApprover(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtReworkCodeQuery.Text)),
                string.Empty,
                string.Empty,
                FormatHelper.CleanString(approveStatusQuery),
                string.Empty,
                false,
                false,
                inclusive, exclusive);
        }


        private int GetRowCount()
        {
            if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
            // TODO: ʹ������֮����ֶβ�ѯ����ĺ�̨Facade��Query����
            string approveStatusQuery;

            if (this.reworkStatus == ReworkStatus.REWORKSTATUS_NEW)
            {
                if (this.drpApproveStatusQuery.SelectedValue == string.Empty
                    || this.drpApproveStatusQuery.SelectedValue == ApproveStatus.APPROVESTATUS_WAITING.ToString())
                {
                    approveStatusQuery = string.Empty;
                }
                else
                {
                    approveStatusQuery = "-1";
                }

            }
            else
            {
                approveStatusQuery = this.drpApproveStatusQuery.SelectedValue; ;
            }
            return this._facade.QueryReworkApproverCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtReworkCodeQuery.Text)),
                string.Empty,
                string.Empty,
                FormatHelper.CleanString(approveStatusQuery),
                string.Empty,
                false,
                false);
        }

        #endregion

        #region Button
        protected void cmdQuery_ServerClick(object sender, System.EventArgs e)
        {
            this.RequestData();
            this.buttonHelper.PageActionStatusHandle(PageActionType.Query);
        }

        private void RequestData()
        {
            this.pagerToolBar.PageSize = this.pagerSizeSelector.PageSize;
            this.gridHelper.GridBind(PageGridBunding.Page, this.pagerSizeSelector.PageSize);

            this.pagerToolBar.RowCount = GetRowCount();
            this.pagerToolBar.InitPager();
        }

        private void buttonHelper_AfterPageStatusChangeHandle(string pageAction)
        {
            // TODO: ���ɸ��ĵ��ֶ���ֻ��

        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("FReworkApproveMP.aspx");
        }
        #endregion

        #region Object <--> Page


        //private object GetEditObject(GridRecord row)
        //{
        //    if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
        //    object obj = _facade.GetReworkPass(row.Cells[8].Text.ToString(), row.Cells[9].Text.ToString());

        //    if (obj != null)
        //    {
        //        return (ReworkPass)obj;
        //    }

        //    return null;
        //}

        private void SetEditObject(object obj)
        {
        }


        private bool ValidateInput()
        {
            return true;


        }

        #endregion

        private string GetPassStatusString(ReworkPassEx obj)
        {

            if (obj.Status == ApproveStatus.APPROVESTATUS_PASSED)
            {
                return this.languageComponent1.GetString(ApproveStatus.APPROVESTATUS_PASSED_STRING);
            }
            else if (obj.Status == ApproveStatus.APPROVESTATUS_NOPASSED)
            {
                return this.languageComponent1.GetString(ApproveStatus.APPROVESTATUS_NOPASSED_STRING);
            }
            return this.languageComponent1.GetString(ApproveStatus.APPROVESTATUS_WAITING_STRING);
        }


    }
}
