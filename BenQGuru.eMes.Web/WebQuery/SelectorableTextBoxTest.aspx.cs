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

using BenQGuru.eMES.Web.SelectQuery ;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// Test ��ժҪ˵����
	/// </summary>
	public class SelectorableTextBoxTest : System.Web.UI.Page
	{
        protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtOP;
        protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox4MO txtMO;
        protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtItem;
        protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtStepSequence;
        protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtSegment;
        protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtModel;
        protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtResource;
        protected System.Web.UI.WebControls.Button Button1;
        protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox4SS Selectabletextbox1;
        protected SelectableTextBox txtCode;
    
		private void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��


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
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion

        private void Button1_Click(object sender, System.EventArgs e)
        {
            Response.Write( this.txtOP.Text) ;
        }
	}
}
