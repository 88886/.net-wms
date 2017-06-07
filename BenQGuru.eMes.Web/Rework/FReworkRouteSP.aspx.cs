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
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Rework;
using BenQGuru.eMES.Domain.Rework;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.Rework
{
    /// <summary>
    /// FReworkRouteSP ��ժҪ˵����
    /// </summary>
    public partial class FReworkRouteSP : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        private BenQGuru.eMES.Rework.ReworkFacade _facade;//= ReworkFacadeFactory.Create();

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
            if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
            ReworkSheet rs = (ReworkSheet)this._facade.GetReworkSheet(this.GetRequestParam("ReworkSheetCode"));
            if (rs == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_ReworkCode_Invalid");
            }

            if (rs.Status != ReworkStatus.REWORKSTATUS_RELEASE)
            {
                cmdSave.Visible = false;
                cmdCancel.Visible = false;
            }
            else
            {
                cmdSave.Visible = true;
                cmdCancel.Visible = true;
            }


            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this.txtReworkCode.Text = this.GetRequestParam("ReworkSheetCode");
                ReworkSheet obj = (ReworkSheet)this._facade.GetReworkSheet(this.txtReworkCode.Text);
                this.txtItemCode.Text = obj.ItemCode;
                this.txtReworkStatus.Text = this.languageComponent1.GetString(obj.Status);
                this.txtReworkType.Text = this.languageComponent1.GetString(obj.ReworkType);
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        #endregion

        #region WebGrid
        protected override void InitWebGrid()
        {
            // TODO: �����е�˳�򼰱���
            base.InitWebGrid();
            this.gridHelper.AddColumn("ReworkSheetCode", "���󵥺�", null);
            this.gridHelper.AddColumn("RouteCode", ";��", null);
            this.gridHelper.AddColumn("ReworkSheetStatus", "����״̬", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
            this.gridHelper.AddDefaultColumn(false, true);
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.RequestData();
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["ReworkSheetCode"] = ((ReworkSheet)obj).ReworkCode.ToString();
            row["RouteCode"] = ((ReworkSheet)obj).NewRouteCode.ToString();
            row["ReworkSheetStatus"] = this.languageComponent1.GetString(((ReworkSheet)obj).Status);
            row["MaintainUser"] = ((ReworkSheet)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((ReworkSheet)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((ReworkSheet)obj).MaintainTime);
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
            object obj = this._facade.GetReworkSheet(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtReworkCode.Text))
                );
            return new object[1] { obj };
        }


        protected override int GetRowCount()
        {
            return 1;
        }

        #endregion

        #region Object <--> Page
        protected override object GetEditObject(GridRecord row)
        {
            if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
            // TODO: �������е�Index���滻keyIndex
            object obj = _facade.GetReworkSheet(row.Items.FindItemByKey("ReworkSheetCode").Value.ToString());

            if (obj != null)
            {
                return (ReworkSheet)obj;
            }

            return null;
        }

        protected override object GetEditObject()
        {
            if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
            object reworkSheet = this._facade.GetReworkSheet(this.txtReworkCode.Text);
            ((ReworkSheet)reworkSheet).NewRouteCode = this.drpRoute.SelectedValue;

            return reworkSheet;
        }


        protected override void SetEditObject(object obj)
        {
            try
            {
                this.drpRoute.SelectedValue = ((ReworkSheet)obj).NewRouteCode.ToString();
            }
            catch
            {
                this.drpRoute.SelectedIndex = 0;
            }
        }

        #endregion

        #region ���ݳ�ʼ��

        protected void drpRoute_Load(object sender, System.EventArgs e)
        {
            if (!this.IsPostBack)
            {
                object[] objs = new ReworkFacadeFactory(base.DataProvider).CreateItemFacade().QueryItem2Route(this.txtItemCode.Text, string.Empty, GlobalVariables.CurrentOrganizations.First().OrganizationID.ToString());
                this.drpRoute.Items.Add(string.Empty);
                if (objs != null)
                {
                    foreach (BenQGuru.eMES.Domain.MOModel.Item2Route obj in objs)
                    {
                        this.drpRoute.Items.Add(obj.RouteCode);
                    }
                }
            }
        }


        #endregion

        protected override void UpdateDomainObject(object domainObject)
        {
            if (_facade == null) { _facade = new ReworkFacadeFactory(base.DataProvider).Create(); }
            this._facade.UpdateReworkSheet(domainObject as ReworkSheet);
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect(this.MakeRedirectUrl("FReworkSheetMP.aspx"));
        }
    }
}
