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
using BenQGuru.eMES.WebQuery;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FReportCenterWeekMocode 的摘要说明。
	/// </summary>
	public partial class FReportCenterWeekMocode : BaseRQPage
	{
	
		protected int today = FormatHelper.TODateInt(System.DateTime.Now);
		protected string segmentCode = "";
		protected string stepSequenceCode = "";
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			segmentCode = this.GetRequestParam("SegmentCode");
			stepSequenceCode = this.GetRequestParam("StepSequenceCode");

			if( !this.IsPostBack )
			{
				// 初始化页面语言
				this.InitPageLanguage(this.languageComponent1, false);

				this.OWCChartSpace1.Display = false;
				this._processOWC(this._loadDataSource());
			}
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
			this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
			// 
			// languageComponent1
			// 
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
		#endregion

		private object[] _loadDataSource()
		{
			return new FacadeFactory(base.DataProvider).CreateQueryFacade1().QueryRPTCenterWeekMocode(today,segmentCode,stepSequenceCode);
		}
		
		private void _processOWC(object[] dataSource)
		{
			this.OWCChartSpace1.ClearCharts();

			if( dataSource != null )
			{
				string[] categories = new string[ dataSource.Length ];
				object[] values = new object[dataSource.Length];

				for(int i = 0;i<dataSource.Length;i++)
				{
					categories[i] = (dataSource[i] as RPTCenterWeekMocode).ShiftDay.ToString();
					values[i] = (dataSource[i] as RPTCenterWeekMocode).DayQuantity;
				}

				this.OWCChartSpace1.AddChart(stepSequenceCode,categories,values,OWCChartType.LineMarkers);
				this.OWCChartSpace1.Display = true;
			}
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(
				this.MakeRedirectUrl(
				"ReportCenterLine.aspx",
				new string[]{"SegmentCode"},
				new string[]{segmentCode})
				);
		}
	}
}