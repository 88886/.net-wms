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
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Domain.Warehouse;
using Infragistics.WebUI.UltraWebGrid;

namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FInvBusiness2FormulaQuery : BaseMPage
    {
        protected System.Web.UI.WebControls.Label lblOperationSelectTitle;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private InventoryFacade facade = null;

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

                this.txtBusinessCodeQuery.Text = this.GetRequestParam("businessCode");

                if (facade == null)
                {
                    facade = new InventoryFacade(this.DataProvider);
                }
                InvBusiness business = (InvBusiness)facade.GetInvBusiness(this.GetRequestParam("businessCode"), GlobalVariables.CurrentOrganizations.First().OrganizationID);
                this.txtBusinessDescriptionQuery.Text = business.BusinessDescription;

            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        #endregion

        #region NotStable
        protected override void InitWebGrid()
        {
            this.gridHelper.AddColumn("FormulaCode", "�������", null);
            this.gridHelper.AddColumn("FormulaDescription", "��������", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);


            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.AddDefaultColumn(true, false);
            base.InitWebGrid();

            this.gridHelper.RequestData();
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (facade == null)
            {
                facade = new InventoryFacade(this.DataProvider);
            }
            this.facade.DeleteInvBusiness2Formula((InvBusiness2Formula[])domainObjects.ToArray(typeof(InvBusiness2Formula)));
        }

        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new InventoryFacade(this.DataProvider);
            }
            return this.facade.QueryInvBusinessFormulaCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtBusinessCodeQuery.Text)));
        }

        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
            return new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
                new object[]{"false",
								((InvFormula)obj).FormulaCode.ToString(),
                                ((InvFormula)obj).FormulaDesc.ToString(),
                                //((InvFormula)obj).MaintainUser.ToString(),
                                   ((InvFormula)obj).GetDisplayText("MaintainUser"),
                                FormatHelper.ToDateString(((InvFormula)obj).MaintainDate)
							});
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new InventoryFacade(this.DataProvider);
            }
            return facade.QueryInvBusinessFormulaQuery(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtBusinessCodeQuery.Text)),
                inclusive, exclusive);
        }

        protected void cmdSelect_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(
                    this.MakeRedirectUrl("./FInvBusiness2Formula.aspx",
                                            new string[] { "businessCode" },
                                            new string[] { this.txtBusinessCodeQuery.Text.Trim() }));
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("./FInvBusinessMP.aspx"));
        }

        protected override object GetEditObject(UltraGridRow row)
        {
            if (facade == null)
            {
                facade = new InventoryFacade(this.DataProvider);
            }
            object obj = this.facade.GetInvBusiness2Formula(this.txtBusinessCodeQuery.Text.Trim(), row.Cells[1].Text.Trim(), GlobalVariables.CurrentOrganizations.First().OrganizationID);

            if (obj != null)
            {
                return (InvBusiness2Formula)obj;
            }

            return null;
        }

        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((InvFormula)obj).FormulaCode.ToString(),
                                ((InvFormula)obj).FormulaDesc.ToString(),
                                //((InvFormula)obj).MaintainUser.ToString(),
                                ((InvFormula)obj).GetDisplayText("MaintainUser"),
                                FormatHelper.ToDateString(((InvFormula)obj).MaintainDate)};
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"FormulaCode",
                                    "FormulaDescription",
                                    "MaintainUser",	
                                    "MaintainDate"};
        }

        #endregion
    }
}
