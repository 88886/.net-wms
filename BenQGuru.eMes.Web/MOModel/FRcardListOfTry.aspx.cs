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
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.MOModel;
using Infragistics.Web.UI.GridControls;

namespace BenQGuru.eMES.Web.MOModel
{
    public partial class FRcardListOfTry : BaseMPageNew
    {
        protected System.Web.UI.WebControls.Label lblOperationSelectTitle;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private TryFacade facade = null;

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

                if (Page.Request["tryCode"] != null && Page.Request["tryCode"] != string.Empty)
                {
                    this.txtTryCodeQuery.Text = Page.Request["tryCode"];
                }

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
            this.gridHelper.AddColumn("RunningCard", "���к�", null);
            this.gridHelper.AddColumn("CartonCode", "���", null);
            this.gridHelper.AddColumn("PalletCode", "ջ���", null);
            this.gridHelper.AddColumn("ItemCode", "��Ʒ", null);
            this.gridHelper.AddColumn("ItemDescription", "��Ʒ����", null);

            this.gridHelper.AddColumn("OPCode", "�������", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridHelper.AddDefaultColumn(false, false);
            ((BoundDataField)this.gridWebGrid.Columns.FromKey("RunningCard")).HtmlEncode = false;
            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.RequestData();
        }

        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new TryFacade(this.DataProvider);
            }
            return this.facade.QueryTry2RcardByTryCodeCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtTryCodeQuery.Text)));
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = DtSource.NewRow();
            row["RunningCard"] = GetRCardLink(((Try2RcardNew)obj).RCard.ToString());
            row["CartonCode"] = ((Try2RcardNew)obj).CartonCode.ToString();
            row["PalletCode"] = ((Try2RcardNew)obj).PalletCode.ToString();
            row["ItemCode"] = ((Try2RcardNew)obj).ItemCode.ToString();
            row["ItemDescription"] = ((Try2RcardNew)obj).ItemDescription.ToString();
            row["OPCode"] = ((Try2RcardNew)obj).OPCode.ToString();
            row["MaintainUser"] = ((Try2RcardNew)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((Try2RcardNew)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((Try2RcardNew)obj).MaintainTime);
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new TryFacade(this.DataProvider);
            }
            return facade.QueryTry2RcardByTryCode(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtTryCodeQuery.Text)),
                inclusive, exclusive);
        }

        #endregion


        #region Export
        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                                ((Try2RcardNew)obj).RCard.ToString(),
                                ((Try2RcardNew)obj).CartonCode.ToString(),
                                ((Try2RcardNew)obj).PalletCode.ToString(),
                                ((Try2RcardNew)obj).ItemCode.ToString(),
                                ((Try2RcardNew)obj).ItemDescription.ToString(),
                                ((Try2RcardNew)obj).OPCode.ToString(),
                                ((Try2RcardNew)obj).GetDisplayText("MaintainUser"),
								FormatHelper.ToDateString(((Try2RcardNew)obj).MaintainDate),							
								FormatHelper.ToTimeString(((Try2RcardNew)obj).MaintainTime)
            };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"RunningCard",
									"CartonCode",	
									"PalletCode",
                                    "ItemCode",
                                    "ItemDescription",
                                    "OPCode",
									"MaintainUser",	
									"MaintainDate",
                                    "MaintainTime",                        
                                };
        }

        #endregion

        private string GetRCardLink(string rcard)
        {
            return string.Format("<a href='../WebQuery/FItemTracingQP.aspx?RCARDFROM={0}&RCARDTO={1}'>{2}</a>", rcard, rcard, rcard);
        }

        protected void cmdCancel_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("FTryMP.aspx"));
        }
    }
}
