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
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.TSModel;

using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.TSModel
{
    /// <summary>
    /// FOperation2ResourceSP ��ժҪ˵����
    /// </summary>
    public partial class FModel2ErrorCodeGroupSP : BaseMPageNew
    {

        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        protected System.Web.UI.WebControls.Label lblErrorGroupCode;

        private TSModelFacade _facade;//= TSModelFacadeFactory.CreateTSModelFacade();

        #region Stable
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //this.pagerSizeSelector.Readonly = true;

            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                this.txtModelCodeQuery.Text = this.GetRequestParam("Modelcode");

            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        #endregion

        #region Not Stable
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("AssErrorCodeGroupA", "�ѹ�������������", null);
            this.gridHelper.AddColumn("ErrorCodeGroupDescription", "��������������", null);
            this.gridHelper.AddColumn("RouteCode", ";�̴���", null);
            this.gridHelper.AddColumn("OPCode", "�������", null);
            this.gridHelper.AddColumn("ResourecCode", "��Դ����", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridWebGrid.Columns.FromKey("RouteCode").Hidden = true;
            this.gridWebGrid.Columns.FromKey("OPCode").Hidden = true;
            this.gridWebGrid.Columns.FromKey("ResourecCode").Hidden = true;

            this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
            this.gridHelper.AddDefaultColumn(true, false);

            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.RequestData();

        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (_facade == null) { _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade(); }
            _facade.DeleteModel2ErrorCodeGroup((Model2ErrorCodeGroup[])domainObjects.ToArray(typeof(Model2ErrorCodeGroup)));
        }

        protected override int GetRowCount()
        {
            if (_facade == null) { _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade(); }
            return this._facade.GetSelectedErrorCodeGroupByModelCodeCount(
                FormatHelper.PKCapitalFormat(this.txtModelCodeQuery.Text.Trim()),
                FormatHelper.PKCapitalFormat(this.txtErrorCodeGroupCodeQuery.Text.Trim())
                );
        }

        protected override DataRow GetGridRow(object obj)
        {

            DataRow row = this.DtSource.NewRow();
            row["AssErrorCodeGroupA"] = ((ErrorCodeGroupA)obj).ErrorCodeGroup.ToString();
            row["ErrorCodeGroupDescription"] = ((ErrorCodeGroupA)obj).ErrorCodeGroupDescription.ToString();
            row["RouteCode"] = ((ErrorCodeGroupA)obj).RouteCode.ToString();
            row["OPCode"] = ((ErrorCodeGroupA)obj).OPCode.ToString();
            row["ResourecCode"] = ((ErrorCodeGroupA)obj).ResourceCode.ToString();
            row["MaintainUser"] = ((ErrorCodeGroupA)obj).MaintainUser.ToString();
            row["MaintainDate"] = FormatHelper.ToDateString(((ErrorCodeGroupA)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((ErrorCodeGroupA)obj).MaintainTime);
            return row;
        }

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((ErrorCodeGroupA)obj).ErrorCodeGroup.ToString(),
								   ((ErrorCodeGroupA)obj).ErrorCodeGroupDescription.ToString(),
								   ((ErrorCodeGroupA)obj).MaintainUser.ToString(),
								   FormatHelper.ToDateString(((ErrorCodeGroupA)obj).MaintainDate)};
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"AssErrorCodeGroupA",
									"ErrorCodeGroupDescription",
									"MaintainUser",
									"MaintainDate"};
        }

        protected override object GetEditObject(GridRecord row)
        {
            if (_facade == null) { _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade(); }
            Model2ErrorCodeGroup relation = _facade.CreateNewModel2ErrorCodeGroup();
            relation.ModelCode = this.txtModelCodeQuery.Text.Trim();
            relation.ErrorCodeGroup = row.Items.FindItemByKey("AssErrorCodeGroupA").Text;
            relation.RouteCode = row.Items.FindItemByKey("RouteCode").Text.Trim();
            relation.OPCode = row.Items.FindItemByKey("OPCode").Text.Trim();
            relation.MaintainUser = this.GetUserCode();

            return relation;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null) { _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade(); }
            return this._facade.GetSelectedErrorCodeGroupByModelCode(
                FormatHelper.PKCapitalFormat(this.txtModelCodeQuery.Text.Trim()),
                FormatHelper.PKCapitalFormat(this.txtErrorCodeGroupCodeQuery.Text.Trim()),
                inclusive, exclusive);
        }

        //protected void cmdSelect_ServerClick(object sender, System.EventArgs e)
        //{
        //    this.gridHelper.RequestData();
        //    this.Response.Redirect(this.MakeRedirectUrl("../tsmodel/FModel2ErrorCodeGroupSP.aspx", new string[] { "modelcode" }, new string[] { this.txtModelCodeQuery.Text.Trim().ToUpper() }));
        //}

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("../momodel/FModelMP.aspx"));
        }

        #endregion

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


        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            this.gridHelper.RequestData();
        }


    }
}
