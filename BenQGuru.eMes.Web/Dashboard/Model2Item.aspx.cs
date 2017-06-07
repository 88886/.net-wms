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

using BenQGuru.eMES.Dashboard;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;

namespace BenQGuru.eMES.Web.Dashboard
{
	/// <summary>
	/// WebForm1 ��ժҪ˵����
	/// </summary>
	public partial class Model2Item : Page
	{
		private ModelFacade modelFAC = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				GetModel2Item();
			}
		}

		#region GetModel2Item
		public void GetModel2Item()
		{

			if(modelFAC == null)
			{
				modelFAC = (new FacadeFactory()).CreateModelFacade();;
			}
			string xmlContent = modelFAC.getModel2Item();

			this.Response.Clear();
			this.Response.ClearContent();
			this.Response.ClearHeaders();

			this.Response.Write(xmlContent);
		}
		#endregion

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
		}
		#endregion
	}
}
