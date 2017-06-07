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
using BenQGuru.eMES.Rework;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.Rework
{
    public partial class FReworkRangeSP_New : BaseMPageNew
    {
        protected System.Web.UI.WebControls.Label lblOperationSelectTitle;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private ReworkFacade facade = null;

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

                this.txtReworkSheetCode.Text = this.GetRequestParam("ReworkSheetCode");

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
            this.gridHelper.AddColumn("LotList", "��������", null);
            this.gridHelper.AddColumn("MOCode", "������������", null);
            this.gridHelper.AddColumn("ItemCode", "��Ʒ����", null);
            this.gridHelper.AddColumn("MModelCode", "����", null);
            this.gridHelper.AddColumn("ReworkDate", "��������", null);
            this.gridHelper.AddColumn("ReworkTime", "����ʱ��", null);
            this.gridHelper.AddColumn("ReworkUser", "��Ա", null);

            this.gridHelper.AddDefaultColumn(false, false);

            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridWebGrid.Columns.FromKey("MModelCode").Hidden = true;
            ((BoundDataField)this.gridHelper.Grid.Columns.FromKey("RunningCard")).HtmlEncode = false;
            ((BoundDataField)this.gridHelper.Grid.Columns.FromKey("LotList")).HtmlEncode = false;
            this.gridHelper.RequestData();
        }

        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new ReworkFacade(base.DataProvider);
            }
            return this.facade.GetReworkRangeQueryCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtReworkSheetCode.Text)));
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["RunningCard"]=GetRCardLink(((ReworkRangeQuery)obj).RunningCard.ToString());
                row["LotList"]=GetLotNoLink(((ReworkRangeQuery)obj).LotList.ToString());
                row["MOCode"]=((ReworkRangeQuery)obj).MoCode.ToString();
                row["ItemCode"]=((ReworkRangeQuery)obj).ItemCode.ToString();
                row["MModelCode"]=((ReworkRangeQuery)obj).MModelCode.ToString();
                row["ReworkDate"]=FormatHelper.ToDateString(((ReworkRangeQuery)obj).MaintainDate);
                row["ReworkTime"]=FormatHelper.ToTimeString(((ReworkRangeQuery)obj).MaintainTime);
                row["ReworkUser"] = ((ReworkRangeQuery)obj).GetDisplayText("MaintainUser");
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new ReworkFacade(base.DataProvider);
            }

            return this.facade.GetReworkRangeQuery(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtReworkSheetCode.Text)),
                inclusive, exclusive);
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("./FReworkSheetMP.aspx"));
        }

        //protected override object GetEditObject(UltraGridRow row)
        //{
        //    if (facade == null)
        //    {
        //        facade = new ReworkFacade(this.DataProvider);
        //    }
        //    object obj = this.facade.GetOQCCheckGroup2List(this.txtCheckGroupQuery.Text.Trim(), row.Cells[1].Text.Trim());

        //    if (obj != null)
        //    {
        //        return (OQCCheckGroup2List)obj;
        //    }

        //    return null;
        //}

        #endregion

        #region Export
        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                                ((ReworkRangeQuery)obj).RunningCard.ToString(),
                                ((ReworkRangeQuery)obj).LotList.ToString(),
                                ((ReworkRangeQuery)obj).MoCode.ToString(),
                                ((ReworkRangeQuery)obj).ItemCode.ToString(),
								FormatHelper.ToDateString(((ReworkRangeQuery)obj).MaintainDate),							
								FormatHelper.ToTimeString(((ReworkRangeQuery)obj).MaintainTime),
                                ((ReworkRangeQuery)obj).MaintainUser};
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"RunningCard",
									"LotList",	
									"MoCode",
                                    "ItemCode",
									"ReworkDate",	
									"ReworkTime",
                                    "ReworkUser"};
        }

        #endregion

        #region Link
        private string GetRCardLink(string no)
        {
            string url = string.Format("../WebQuery/FOQCLotSampleQP.aspx?reworkrcard={0}", this.Server.UrlEncode(no));
            return string.Format("<a href=" + url + ">{0}</a>", no);
        }
        private string GetLotNoLink(string no)
        {
            if (no == string.Empty)
            {
                return " ";
            }
            else
            {
                string url = string.Format("../WebQuery/FOQCSampleNGDetailQP.aspx?LotNo={0}&BackUrl=../Rework/FReworkRangeSP_New.aspx?ReworkSheetCode={1}", this.Server.UrlEncode(no), this.Server.UrlEncode(this.GetRequestParam("ReworkSheetCode")));
                return string.Format("<a href=" + url + ">{0}</a>", no);
            }
        }

        #endregion

    }
}
