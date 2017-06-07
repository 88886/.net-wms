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

namespace BenQGuru.eMES.Web.BaseSetting
{
    /// <summary>
    /// FOperation2ResourceSP ��ժҪ˵����
    /// </summary>
    public partial class FOperation2ResourceAP : BaseAPageNew
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
            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this.txtOperationCodeQuery.Text = this.GetRequestParam("opcode");
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
            this.gridHelper.AddColumn("UnAssResourceCode", "δ������Դ����", null);
            this.gridHelper.AddColumn("ResourceDescription", "��Դ����", null);
            this.gridHelper.AddColumn("ResourceStepSequence", "��������", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);

            this.gridHelper.AddColumn("ResourceType", "��Դ���", null);
            this.gridHelper.AddColumn("ResourceGroup", "��Դ����", null);
            this.gridHelper.AddColumn("ShiftTypeCode", "���ƴ���", null);

            this.gridHelper.AddColumn("SegmentCode", "���δ���", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridWebGrid.Columns.FromKey("ResourceType").Hidden = true;
            this.gridWebGrid.Columns.FromKey("ResourceGroup").Hidden = true;
            this.gridWebGrid.Columns.FromKey("ShiftTypeCode").Hidden = true;
            this.gridWebGrid.Columns.FromKey("SegmentCode").Hidden = true;
            this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;

            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            base.InitWebGrid();
        }

        protected override void AddDomainObject(ArrayList domainObject)
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            facade.AddOperation2Resource((Operation2Resource[])domainObject.ToArray(typeof(Operation2Resource)));
        }

        protected override object GetEditObject(GridRecord row)
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            Operation2Resource relation = facade.CreateNewOperation2Resource();

            relation.OPCode = this.txtOperationCodeQuery.Text.Trim();
            relation.ResourceCode = row.Items.FindItemByKey("UnAssResourceCode").Text;
            relation.MaintainUser = this.GetUserCode();

            return relation;
        }

        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            return this.facade.GetUnselectedResourceByOperationCodeCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtOperationCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtResourceCodeQuery.Text)));
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = DtSource.NewRow();
            row["UnAssResourceCode"] = ((Resource)obj).ResourceCode.ToString();
            row["ResourceDescription"] = ((Resource)obj).ResourceDescription;
            row["ResourceStepSequence"] = ((Resource)obj).GetDisplayText("StepSequenceCode");
            row["MaintainUser"] = ((Resource)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((Resource)obj).MaintainDate);
            row["ResourceType"] = ((Resource)obj).ResourceType.ToString();
            row["ResourceGroup"] = ((Resource)obj).ResourceGroup.ToString();
            row["ShiftTypeCode"] = ((Resource)obj).ShiftTypeCode;
            row["SegmentCode"] = ((Resource)obj).SegmentCode.ToString();
            row["MaintainTime"] = FormatHelper.ToTimeString(((Resource)obj).MaintainTime);
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            return facade.GetUnselectedResourceByOperationCode(
                    FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtOperationCodeQuery.Text)),
                    FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtResourceCodeQuery.Text)),
                    inclusive, exclusive);
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(
                this.MakeRedirectUrl(@"./FOperation2ResourceSP.aspx",
                                    new string[] { "opcode" },
                                    new string[] { this.txtOperationCodeQuery.Text }));
        }
        #endregion

    }
}
