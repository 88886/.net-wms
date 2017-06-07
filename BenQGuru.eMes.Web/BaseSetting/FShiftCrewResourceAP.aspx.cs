using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using Infragistics.Web.UI.GridControls;

namespace BenQGuru.eMES.Web.BaseSetting
{
    public partial class FShiftCrewResourceAP : BaseAPageNew
    {
        protected System.Web.UI.WebControls.Label lblOperationSelectTitle;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        private ShiftModel facade = null;
        private BaseModelFacade _facade = null;
        protected ExcelExporter excelExporter = null;

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

        #region Stable
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this.txtCrewCodeQuery.Text = this.GetRequestParam("crewCode");
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
            this.gridHelper.AddColumn("UnAssResourceCode", "��Դ����", null);
            this.gridHelper.AddColumn("ResourceDescription", "��Դ����", null);
            this.gridHelper.AddColumn("StepSequenceCode", "��������", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);


            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.AddDefaultColumn(true, false);
            base.InitWebGrid();

            // this.gridHelper.RequestData();
        }

        protected override void AddDomainObject(ArrayList domainObject)
        {
            if (facade == null)
            {
                facade = new ShiftModel(this.DataProvider);
            }
            facade.AddShiftCrewResource((Resource[])domainObject.ToArray(typeof(Resource)), this.txtCrewCodeQuery.Text);
        }

        protected override object GetEditObject(GridRecord row)
        {
            if (_facade == null)
            {
                _facade = new BaseModelFacade(base.DataProvider);
            }
            object obj = _facade.GetResource(row.Items.FindItemByKey("UnAssResourceCode").Text.ToString());

            if (obj != null)
            {
                return (Resource)obj;
            }

            return null;
        }


        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new ShiftModel(this.DataProvider);
            }
            return this.facade.QueryShiftCrew2NewResourceCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtResourceCodeQuery.Text)));
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = DtSource.NewRow();
            row["UnAssResourceCode"] = ((Resource)obj).ResourceCode.ToString();
            row["ResourceDescription"] = ((Resource)obj).ResourceDescription.ToString();
            row["StepSequenceCode"] = ((Resource)obj).StepSequenceCode.ToString();
            row["MaintainUser"] = ((Resource)obj).MaintainUser;
            row["MaintainDate"] = FormatHelper.ToDateString(((Resource)obj).MaintainDate);
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new ShiftModel(this.DataProvider);
            }
            return facade.QueryShiftCrew2NewResource(
                    FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtResourceCodeQuery.Text)),
                    inclusive, exclusive);
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(
                this.MakeRedirectUrl(@"./FShiftCrewResourceSP.aspx",
                                    new string[] { "crewCode" },
                                    new string[] { this.txtCrewCodeQuery.Text }));
        }

        #endregion

    }
}
