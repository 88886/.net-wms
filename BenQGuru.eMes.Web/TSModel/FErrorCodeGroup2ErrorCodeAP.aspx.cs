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

namespace BenQGuru.eMES.Web.TSModel
{
    /// <summary>
    /// FOperation2ResourceSP ��ժҪ˵����
    /// </summary>
    public partial class FErrorCodeGroup2ErrorCodeAP : BaseAPageNew
    {
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        protected System.Web.UI.WebControls.Label lblErrorGroupCode;

        private TSModelFacade _facade;//= TSModelFacadeFactory.CreateTSModelFacade();

        #region Stable
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
            }
            this.txtErrorCodeGroupCodeQuery.Text = this.GetRequestParam("ErrorCodeGroup");
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        #endregion

        #region Not Stable
        protected override void InitWebGrid()
        {
            this.gridHelper.AddColumn("UnAssErrorCode", "��������", null);
            this.gridHelper.AddColumn("ErrorDescription", "������������", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            base.InitWebGrid();
        }

        protected override void AddDomainObject(ArrayList domainObject)
        {
            if (_facade == null) { _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade(); }
            _facade.AddErrorCodeGroup2ErrorCode((ErrorCodeGroup2ErrorCode[])domainObject.ToArray(typeof(ErrorCodeGroup2ErrorCode)));
        }

        protected override int GetRowCount()
        {
            if (_facade == null) { _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade(); }
            return this._facade.GetUnselectedErrorCodeByErrorCodeGroupCodeCount(FormatHelper.PKCapitalFormat(this.txtErrorCodeGroupCodeQuery.Text.Trim()),
                FormatHelper.PKCapitalFormat(this.txtErrorCodeCodeQuery.Text.Trim()));
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = DtSource.NewRow();
            row["UnAssErrorCode"] = FormatHelper.PKCapitalFormat(((ErrorCodeA)obj).ErrorCode.ToString());
            row["ErrorDescription"] = ((ErrorCodeA)obj).ErrorDescription.ToString();
            row["MaintainUser"] = ((ErrorCodeA)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((ErrorCodeA)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((ErrorCodeA)obj).MaintainTime);
            return row;
        }

        protected override object GetEditObject(GridRecord row)
        {
            if (_facade == null) { _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade(); }
            ErrorCodeGroup2ErrorCode relation = _facade.CreateNewErrorCodeGroup2ErrorCode();
            relation.ErrorCodeGroup = this.txtErrorCodeGroupCodeQuery.Text.Trim();
            relation.ErrorCode = row.Items.FindItemByKey("UnAssErrorCode").Text;
            relation.MaintainUser = this.GetUserCode();

            return relation;
        }


        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null) { _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade(); }
            return _facade.GetUnselectedErrorCodeByErrorCodeGroupCode(
                FormatHelper.PKCapitalFormat(this.txtErrorCodeGroupCodeQuery.Text.Trim()),
                FormatHelper.PKCapitalFormat(this.txtErrorCodeCodeQuery.Text.Trim()),
                inclusive, exclusive);
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("./FErrorCodeGroup2ErrorCodeSP.aspx", new string[] { "ErrorCodeGroup" }, new string[] { this.GetRequestParam("ErrorCodeGroup") }));
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

    }
}
