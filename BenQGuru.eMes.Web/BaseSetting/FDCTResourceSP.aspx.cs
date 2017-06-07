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
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.BaseSetting
{
    /// <summary>
    /// FDCTResourceSP ��ժҪ˵����
    /// </summary>
    public partial class FDCTResourceSP : BaseMPageNew
    {
        protected System.Web.UI.WebControls.Label lblOperationSelectTitle;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private BaseModelFacade facade = null;//new BaseModelFacadeFactory().Create();	

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

                this.txtDctCommandQuery.Text = this.GetRequestParam("dctCode");
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
            this.gridHelper.AddColumn("AssResourceCode", "�ѹ�����Դ����", null);
            this.gridHelper.AddColumn("ResourceDescription", "��Դ����", null);
            this.gridHelper.AddColumn("ResourceStepSequence", "��������", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.AddDefaultColumn(true, false);

            this.gridHelper.RequestData();

        }

        protected override void UpdateDomainObject(object domainObject)
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            this.facade.UpdateResource((Resource)domainObject);
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            this.facade.UpdateResourceDctCode((Resource[])domainObjects.ToArray(typeof(Resource)));
        }

        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            return this.facade.GetDCT2ResourceCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtResourceCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtDctCommandQuery.Text))
                );
        }

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((Resource)obj).ResourceCode.ToString(),
								   ((Resource)obj).ResourceDescription,
								   ((Resource)obj).StepSequenceCode.ToString(),
								   ((Resource)obj).MaintainUser.ToString(),
								   FormatHelper.ToDateString(((Resource)obj).MaintainDate)};
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"AssResourceCode",
									"ResourceDescription",
									"StepSequenceCode",
									"MaintainUser",	
									"MaintainDate"};
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = DtSource.NewRow();
            row["AssResourceCode"] = ((Resource)obj).ResourceCode.ToString();
            row["ResourceDescription"] = ((Resource)obj).ResourceDescription;
            row["ResourceStepSequence"] = ((Resource)obj).StepSequenceCode.ToString();
            row["MaintainUser"] = ((Resource)obj).MaintainUser.ToString();
            row["MaintainDate"] = FormatHelper.ToDateString(((Resource)obj).MaintainDate);
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            return facade.GetResourceByDCTCode(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtDctCommandQuery.Text)),
                inclusive, exclusive);
        }

        //protected void cmdSelect_ServerClick(object sender, System.EventArgs e)
        //{
        //    this.Response.Redirect(
        //            this.MakeRedirectUrl("./FDCTResourceAP.aspx",
        //                                    new string[] { "dctCode" },
        //                                    new string[] { this.txtDctCommandQuery.Text.Trim() }));
        //}

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("./FDCTCommandMP.aspx"));
        }

        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.txtResourceCodeEdit.Text = "";
                this.txtResourceSequenceEdit.Text = "";

                return;
            }
            this.txtResourceCodeEdit.Text = ((Resource)obj).ResourceCode;
        }

        protected override object GetEditObject()
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            Resource relation = this.facade.CreateNewResource();
            relation.DctCode = FormatHelper.CleanString(this.txtDctCommandQuery.Text.Trim(), 40);
            relation.ResourceCode = FormatHelper.CleanString(this.txtResourceCodeEdit.Text, 40);
            relation.MaintainUser = this.GetUserCode();

            return relation;
        }

        protected override object GetEditObject(GridRecord row)
        {

            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            object obj = this.facade.GetResourceByDctCodeAndRes(row.Items.FindItemByKey("AssResourceCode").Text.Trim(), this.txtDctCommandQuery.Text.Trim());

            if (obj != null)
            {
                return (Resource)obj;
            }

            return null;
        }

        protected override bool ValidateInput()
        {
            //Modify By Leo @  2013-4-9  ҳ���пؼ����ز���Ҫ��֤

            //PageCheckManager manager = new PageCheckManager();

            //manager.Add(new NumberCheck(lblResourceSequenceEdit, txtResourceSequenceEdit, true));

            //if (!manager.Check())
            //{
            //    WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);

            //    return false;
            //}

            return true;
        }
        #endregion

        //ˢ��ҳ��ʹ��
        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            this.gridHelper.RequestData();
        }

    }
}
