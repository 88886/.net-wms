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

using BenQGuru.eMES.Domain.Performance;
using Infragistics.Web.UI.GridControls;

namespace BenQGuru.eMES.Web.SelectQuery
{
    public partial class FsingleExceptionCodeSP : BaseSingleSelectorPageNew
    {
        private BenQGuru.eMES.SelectQuery.SPFacade facade;
        protected global::System.Web.UI.WebControls.TextBox DateQuery;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

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
        protected void Page_Load(object sender, EventArgs e)
        {
            this.gridUnSelectedHelper = new GridHelperNew(this.gridUnSelected, DtSourceUnSelected);
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                this.InitControls();
               
            }
        }

        protected void InitControls()
        {
            if (this.GetRequestParam("Date") != null)
            {
                this.DateQuery.Text = this.GetRequestParam("Date").ToString();
            }

            if (this.GetRequestParam("ItemCode") != null)
            {
                this.txtItemCodeQuery.Text = this.GetRequestParam("ItemCode").ToString();
            }

            if (this.GetRequestParam("ShiftCode") != null)
            {
                this.txtShiftCodeQuery.Text = this.GetRequestParam("ShiftCode").ToString();
            }

            if (this.GetRequestParam("SSCode") != null)
            {
                this.txtSSQuery.Text = this.GetRequestParam("SSCode").ToString();
            }

            if (this.GetRequestParam("ExceptionCode") != null)
            {
                this.txtExceptionCodeQuery.Text = this.GetRequestParam("ExceptionCode").ToString();
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
            base.InitWebGrid();
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedCode", "ʵ���쳣�¼�", null);
            this.gridUnSelectedHelper.AddColumn("sscode", "���ߴ���", null);
            this.gridUnSelectedHelper.AddColumn("Date", "����", null);
            this.gridUnSelectedHelper.AddColumn("ShiftCode", "��δ���", null);
            this.gridUnSelectedHelper.AddColumn("MaterialCode", "���ϴ���", null);
            this.gridUnSelectedHelper.AddColumn("StartDateTime", "��ʼʱ��", null);
            this.gridUnSelectedHelper.AddColumn("EndDateTime", "����ʱ��", null);
            this.gridUnSelectedHelper.AddColumn("ExceptionCode", "�쳣�¼�����", null);
            this.gridUnSelectedHelper.AddColumn("ExceptionDESC", "����", null);
            this.gridUnSelectedHelper.AddColumn("Memo", "��ע", null);
            this.gridUnSelectedHelper.AddColumn("ComfirmMEMO", "ȷ�ϱ�ע", null);
            this.gridUnSelectedHelper.AddDefaultColumn(true, false);
            //������
            this.gridUnSelectedHelper.ApplyLanguage(this.languageComponent1);
            this.gridUnSelectedHelper.RequestData();

            //((BoundDataField)this.gridUnSelected.Columns["Selector_UnselectedCode"]).Hidden = true;
            ((BoundDataField)this.gridUnSelected.Columns["Selector_UnSelectedDesc"]).Hidden = true;
        }

        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["GUID"] = Guid.NewGuid().ToString();
            row["Selector_UnselectedCode"] = ((ExceptionEventWithDescription)obj).Serial.ToString();
            row["sscode"] = ((ExceptionEventWithDescription)obj).SSCode;
            row["Date"] = FormatHelper.ToDateString(((ExceptionEventWithDescription)obj).ShiftDate);
            row["ShiftCode"] = ((ExceptionEventWithDescription)obj).ShiftCode;
            row["MaterialCode"] = ((ExceptionEventWithDescription)obj).ItemCode;
            row["StartDateTime"] = FormatHelper.ToTimeString(((ExceptionEventWithDescription)obj).BeginTime);
            row["EndDateTime"] = FormatHelper.ToTimeString(((ExceptionEventWithDescription)obj).EndTime);
            row["ExceptionCode"] = ((ExceptionEventWithDescription)obj).ExceptionCode;
            row["ExceptionDESC"] = ((ExceptionEventWithDescription)obj).Description;
            row["Memo"] = ((ExceptionEventWithDescription)obj).Memo;
            row["ComfirmMEMO"] = ((ExceptionEventWithDescription)obj).ComfirmMemo;
            return row;
        }


        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUnSelectedExceptionCode(FormatHelper.TODateInt(this.DateQuery.Text),
                                                            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)),
                                                            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtShiftCodeQuery.Text)),
                                                            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSSQuery.Text)),
                                                            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtExceptionCodeQuery.Text)),
                                                            new string[0],
                                                            inclusive, exclusive);
        }


        protected override int GetUnSelectedRowCount()
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUnSelectedExceptionCodeCount(FormatHelper.TODateInt(this.DateQuery.Text),
                                                                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)),
                                                                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtShiftCodeQuery.Text)),
                                                                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSSQuery.Text)),
                                                                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtExceptionCodeQuery.Text)),
                                                                new string[0]);
        }

        #endregion

        #region Button
        //ȷ��
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            //ֻ��ѡһ��
            string exceptionSerial = "";
            int count = 0;
            if (Request.QueryString["Action"] != null && Request.QueryString["Action"].ToString() == "Multi")
            {
                for (int i = 0; i < this.gridUnSelected.Rows.Count; i++)
                {
                    if (this.gridUnSelected.Rows[i].Items.FindItemByKey(gridUnSelectedHelper.CheckColumnKey).ToString().ToUpper() == "TRUE")
                    {
                        if (exceptionSerial == "")
                        {
                            exceptionSerial = this.gridUnSelected.Rows[i].Items.FindItemByKey("Selector_UnselectedCode").Value.ToString();
                        }
                        else
                        {
                            exceptionSerial += "," + this.gridUnSelected.Rows[i].Items.FindItemByKey("Selector_UnselectedCode").Value.ToString();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.gridUnSelected.Rows.Count; i++)
                {
                    if (this.gridUnSelected.Rows[i].Items.FindItemByKey(gridUnSelectedHelper.CheckColumnKey).Value.ToString().ToUpper() == "TRUE")
                    {
                        if (exceptionSerial == "")
                        {
                            exceptionSerial = this.gridUnSelected.Rows[i].Items.FindItemByKey("Selector_UnselectedCode").Value.ToString();
                        }
                        count++;
                    }
                }
                if (count > 1)
                {
                    WebInfoPublish.PublishInfo(this, "$CS_GRID_SELECT_ONE_RECORD", this.languageComponent1);
                    return;
                }
            }

            //this.ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue='" + exceptionSerial + "';window.close();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "window.returnValue='" + exceptionSerial + "';window.close();", true);
        }

        #endregion
    }
}
