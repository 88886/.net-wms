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

using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.ReportView;
using BenQGuru.eMES.Domain.ReportView;

namespace BenQGuru.eMES.Web.ReportView
{
	/// <summary>
	/// FRptDesignStep1MP ��ժҪ˵����
	/// </summary>
    public partial class FRptDesignStep1MP : ReportWizardBasePage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		
	
		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
            this.FirstStepPage = "FRptDesignStep1MP.aspx";

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
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (this.IsPostBack == false)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);       

                ReportViewFacade rptFacade = new ReportViewFacade(this.DataProvider);
                object[] objsDS = rptFacade.GetAllRptViewDataSource();
                this.drpDataSource.Items.Clear();
                if (objsDS != null)
                {
                    for (int i = 0; i < objsDS.Length; i++)
                    {
                        RptViewDataSource dataSource = (RptViewDataSource)objsDS[i];
                        this.drpDataSource.Items.Add(new ListItem(dataSource.Name, dataSource.DataSourceID.ToString()));
                    }
                }

                this.lblDesignStep1Title.Text = this.languageComponent1.GetString("$PageControl_DesignStep1Title");
            }
		}

        protected override void DisplayDesignData()
        {
            if (this.designView.DesignMain != null)
            {
                if (this.drpDataSource.Items.FindByValue(this.designView.DesignMain.DataSourceID.ToString()) != null)
                    this.drpDataSource.SelectedValue = this.designView.DesignMain.DataSourceID.ToString();
                this.txtUploadRptName.Text = this.designView.DesignMain.ReportName;
                this.txtUploadRptDesc.Text = this.designView.DesignMain.Description;
            }
        }

        protected override bool ValidateInput()
        {
            if (this.txtUploadRptName.Text.Trim() == "")
            {
                WebInfoPublish.Publish(this, this.lblUploadRptName.Text + " $Error_Input_Empty", this.languageComponent1);
                return false;
            }
            ReportViewFacade rptFacde = new ReportViewFacade(this.DataProvider);
            RptViewDesignMain rptMain = rptFacde.GetRptViewDesignMainByReportName(this.txtUploadRptName.Text.Trim().ToUpper());
            if (rptMain != null)
            {
                bool bError = false;
                if (string.IsNullOrEmpty(this.GetRequestParam("requestid")) == false && rptMain.ReportID != this.GetRequestParam("reportid"))
                {
                    bError = true;
                }
                if (this.designView == null || this.designView.DesignMain == null)
                {
                    bError = true;
                }
                if (this.designView != null && this.designView.DesignMain != null && rptMain.ReportID != this.designView.DesignMain.ReportID)
                {
                    bError = true;
                }
                if (bError == true)
                {
                    WebInfoPublish.Publish(this, "$ReportDesign_ReportName_Exist [" + this.txtUploadRptName.Text + "]", this.languageComponent1);
                    return false;
                }
            }
            return true;
        }

        protected override void RedirectToBack()
        {
            //
        }

        protected override void RedirectToNext()
        {
            if (this.designView.DesignMain != null && this.designView.DesignMain.DisplayType == ReportDisplayType.Chart)
                this.Response.Redirect("FRptDesignChartMP.aspx?displaytype=chart");
            else
                this.Response.Redirect("FRptDesignStep2MP.aspx");
        }

        protected override void UpdateReportDesignView()
        {
            RptViewDesignMain main = new RptViewDesignMain();
            if (this.designView.DesignMain != null)
                main = this.designView.DesignMain;
            main.ReportName = this.txtUploadRptName.Text.Trim().ToUpper();
            main.Description = this.txtUploadRptDesc.Text;
            main.DataSourceID = Convert.ToDecimal(this.drpDataSource.SelectedValue);
            if (string.IsNullOrEmpty(main.DisplayType) == true)
            {
                if (this.GetRequestParam("displaytype") == "gridchart")
                    main.DisplayType = ReportDisplayType.GridChart;
                else if (this.GetRequestParam("displaytype") == "chart")
                    main.DisplayType = ReportDisplayType.Chart;
                else
                    main.DisplayType = ReportDisplayType.Grid;
            }
            this.designView.DesignMain = main;
        }

		#endregion

        


	}
}
