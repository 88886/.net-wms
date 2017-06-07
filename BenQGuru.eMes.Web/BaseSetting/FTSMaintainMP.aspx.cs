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
using Infragistics.WebUI.UltraWebNavigator;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.Web.BASESETTING
{
	/// <summary>
	/// FTSMaintainMP ��ժҪ˵����
	/// </summary>
	public partial class FTSMaintainMP : BasePage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				Node node = null;
                ////duty
                //node = new Node();
                //node.CssClass = "node";
                //node.Text = this.languageComponent1.GetString("ts_duty");
                //node.TargetFrame = "frameTS";
                //node.TargetUrl = this.VirtualHostRoot + "TSMODEL/FDutyMP.aspx";
                //this.treeTS.Nodes.Add( node );

                ////solution
                //node = new Node();
                //node.CssClass = "node";
                //node.Text = this.languageComponent1.GetString("ts_error_solution");
                //node.TargetFrame = "frameTS";
                //node.TargetUrl = "FSolutionMP.aspx";
                //this.treeTS.Nodes.Add( node );

                ////error cause Group
                //node = new Node();
                //node.CssClass = "node";
                //node.Text = this.languageComponent1.GetString("ts_error_cause_group");
                //node.TargetFrame = "frameTS";
                //node.TargetUrl = "FErrorCauseGroupMP.aspx";
                //this.treeTS.Nodes.Add( node );

                ////error cause
                //node = new Node();
                //node.CssClass = "node";
                //node.Text = this.languageComponent1.GetString("ts_error_cause");
                //node.TargetFrame = "frameTS";
                //node.TargetUrl = "FErrorCauseMP.aspx";
                //this.treeTS.Nodes.Add( node );

				//error code group
				node = new Node();
                node.CssClass = "node";
				node.Text = this.languageComponent1.GetString("ts_error_code_group");
				node.TargetFrame = "frameTS";
				node.TargetUrl = "FErrorCodeGroupMP.aspx";
				this.treeTS.Nodes.Add( node );

				//error code
				node = new Node();
                node.CssClass = "node";
				node.Text = this.languageComponent1.GetString("ts_error_code");
				node.TargetFrame = "frameTS";
                node.TargetUrl = "FErrorCodeMP.aspx";
				this.treeTS.Nodes.Add( node );

                //ts smart config
                //node = new Node();
                //node.Text = this.languageComponent1.GetString("TSSMARTCONFIG");
                //node.TargetFrame = "frameTS";
                //node.TargetUrl = "FTSSmartConfigMP.aspx";
                //this.treeTS.Nodes.Add(node);
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
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";
			this.treeTS.NodeChecked += new Infragistics.WebUI.UltraWebNavigator.NodeCheckedEventHandler(this.treeTS_NodeChecked);

		}
		#endregion

		private void treeTS_NodeChecked(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeCheckedEventArgs e)
		{
		
		}
	}
}
