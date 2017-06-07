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
using BenQGuru.eMES.Common ;
using BenQGuru.eMES.Web.Helper ;
using BenQGuru.eMES.Web.UserControl ;
using BenQGuru.eMES.SelectQuery;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Domain.MOModel;

namespace BenQGuru.eMES.Web.SelectQuery
{
	/// <summary>
	/// Selector ��ժҪ˵����
	/// </summary>
	public partial class FReMOSP : BaseSelectorPageNew
	{

		private BenQGuru.eMES.SelectQuery.SPFacade facade ;//= FacadeFactory.CreateSPFacade() ;


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

		#region Init
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}
		}

		#endregion

		#region WebGrid

		protected override DataRow GetSelectedGridRow(object obj)
        {
            DataRow row = DtSourceSelected.NewRow();
            row["Selector_SelectedCode"] = ((MO)obj).MOCode;
            row["Selector_SelectedDesc"] = ((MO)obj).MODescription;
            return row;
		}

		protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((MO)obj).MOCode;
            row["Selector_UnSelectedDesc"] = ((MO)obj).MODescription;
            return row;
		}

		protected override object[] LoadSelectedDataSource(int inclusive, int exclusive)
		{
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}
			return this.facade.QuerySelectedReworkMO( this.GetSelectedCodes() ) ;
		}

		protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
		{
			
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}

			return this.facade.QueryUnSelectedReworkMO( 
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtModelCodeQuery.Text) ),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text) ),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtMOCodeQuery.Text) ),
				this.GetSelectedCodes(),
				inclusive, exclusive ) ;
		}


		protected override int GetUnSelectedRowCount()
		{
			
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}

			return this.facade.QueryUnSelectedReworkMOCount( 
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtModelCodeQuery.Text) ),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text) ),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtMOCodeQuery.Text) ),
				this.GetSelectedCodes());
		}
        
		#endregion

	}
}
