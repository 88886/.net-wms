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
using BenQGuru.eMES.OQC;
using BenQGuru.eMES.Domain.OQC;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.OQC
{
    /// <summary>
    /// FOQCCheckGroup2List ��ժҪ˵����
    /// </summary>
    public partial class FOQCCheckGroup2List : BaseMPageNew
    {
        protected System.Web.UI.WebControls.Label lblOperationSelectTitle;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private OQCFacade facade = null;

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
            //this.pagerSizeSelector.Readonly = true;

            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this.txtCheckGroupQuery.Text = this.GetRequestParam("checkGroup");
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
            base.InitWebGrid();
            this.gridHelper.AddColumn("AssCheckItemCode", "�ѹ���������Ŀ����", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);


            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            this.gridHelper.AddDefaultColumn(true, false);

            this.gridHelper.RequestData();
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (facade == null)
            {
                facade = new OQCFacade(this.DataProvider);
            }
            this.facade.DeleteOQCCheckGroup2List((OQCCheckGroup2List[])domainObjects.ToArray(typeof(OQCCheckGroup2List)));
        }

        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new OQCFacade(this.DataProvider);
            }
            return this.facade.QueryOQCCheckGroup2ListCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCheckGroupQuery.Text)));
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["AssCheckItemCode"] = ((OQCCheckGroup2List)obj).CheckItemCode.ToString();
            row["MaintainUser"] = ((OQCCheckGroup2List)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((OQCCheckGroup2List)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((OQCCheckGroup2List)obj).MaintainTime);
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new OQCFacade(this.DataProvider);
            }
            return facade.QueryOQCCheckGroup2List(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCheckGroupQuery.Text)),
                inclusive, exclusive);
        }

        protected void cmdSelect_ServerClick(object sender, System.EventArgs e)
        {
            //this.Response.Redirect(
            //        this.MakeRedirectUrl("./FOQCCheckItemByCheckGroup.aspx",
            //                                new string[] { "checkGroup" },
            //                                new string[] { this.txtCheckGroupQuery.Text.Trim() }));
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("./FOQCCheckGroupMP.aspx"));
        }

        protected override object GetEditObject(GridRecord row)
        {
            if (facade == null)
            {
                facade = new OQCFacade(this.DataProvider);
            }
            object obj = this.facade.GetOQCCheckGroup2List(this.txtCheckGroupQuery.Text.Trim(), row.Items.FindItemByKey("AssCheckItemCode").Value.ToString());

            if (obj != null)
            {
                return (OQCCheckGroup2List)obj;
            }

            return null;
        }

        #endregion


        #region Export
        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((OQCCheckGroup2List)obj).CheckItemCode.ToString(),
								   ((OQCCheckGroup2List)obj).GetDisplayText("MaintainUser"),
								   FormatHelper.ToDateString(((OQCCheckGroup2List)obj).MaintainDate),
                                   FormatHelper.ToDateString(((OQCCheckGroup2List)obj).MaintainTime)};
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"AssCheckItemCode",
									"MaintainUser",	
									"MaintainDate",
                                    "MaintainTime"};
        }

        #endregion

        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            this.gridHelper.RequestData();
        }
    }
}
