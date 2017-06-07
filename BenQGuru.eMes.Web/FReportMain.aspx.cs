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

namespace BenQGuru.eMES.Web
{
	/// <summary>
	/// ReportMain ��ժҪ˵����
	/// </summary>
	public partial class FReportMain : BenQGuru.eMES.Web.Helper.BaseRQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected System.Web.UI.HtmlControls.HtmlInputImage Image1;
		private System.ComponentModel.IContainer components;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
				
				this.lblUserName.Text = this.GetUserName();
				this.lblDepartmentName.Text = DepartmentName;
				imgLogout.Attributes["onclick"] = "return LogoutCheck();";

//				OutlookBarBuilder barbuilder = new OutlookBarBuilder();
//				barbuilder.currentPage = this;
//				barbuilder.UserName = this.GetUserName();
//				barbuilder.Build(webOutlookBar, this.languageComponent1,base.DataProvider);
			}
		}

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
			this.languageComponent1.Language = "CHT";
			this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";
			this.imgLogout.ServerClick += new ImageClickEventHandler(imgLogout_ServerClick);

		}
		#endregion

		private void imgLogout_ServerClick(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			SessionHelper sessionHelper = SessionHelper.Current(this.Session); 						

			//sammer kong 20050408 statisical for account of loggin user						
			//			WebStatisical.Instance()["User"].Delete(sessionHelper.UserCode);	
		
			sessionHelper.RemoveAll();
			//

			this.Response.Redirect(this.MakeRedirectUrl(string.Format("{0}FLoginNew.aspx", this.VirtualHostRoot)),false);
		}
	}
}
