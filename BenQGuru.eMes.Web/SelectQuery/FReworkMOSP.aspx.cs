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

namespace BenQGuru.eMES.Web.SelectQuery
{
	/// <summary>
	/// Selector ��ժҪ˵����
	/// </summary>
	public partial class FReworkMOSP : BaseSelectorPageNew
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
			factory_load();
		}

		#endregion

		#region WebGrid

		protected override void InitWebGrid()
		{
			//this.gridSelected.Columns.Clear();
			//this.gridUnselected.Columns.Clear();
            base.InitWebGrid();
            base.InitWebGrid2();
			this.gridSelectedHelper.AddColumn( "Selector_SelectedCode", "��ѡ��ķ�������",	null);
			this.gridSelectedHelper.AddColumn( "Selector_SelectedMOCode", "��������",	null);
			this.gridSelectedHelper.AddColumn( "Selector_SelectedItemCode", "��Ʒ����",	null);
			this.gridSelectedHelper.AddDefaultColumn(true,false) ;
			this.gridSelectedHelper.ApplyLanguage( this.languageComponent1 );

			this.gridUnSelectedHelper.AddColumn( "Selector_UnselectedCode", "δѡ��ķ�������",	null);
			this.gridUnSelectedHelper.AddColumn( "Selector_UnSelectedMOCode", "��������",	null);
			this.gridUnSelectedHelper.AddColumn( "Selector_UnSelectedItemCode", "��Ʒ����",	null);
			this.gridUnSelectedHelper.AddDefaultColumn(true,false) ;
			this.gridUnSelectedHelper.ApplyLanguage( this.languageComponent1 );
		}

		protected override DataRow GetSelectedGridRow(object obj)
		{
            DataRow row = DtSourceSelected.NewRow();
            row["Selector_SelectedCode"] = ((BenQGuru.eMES.SelectQuery.SelectReworkSheet)obj).ReworkCode;
            row["Selector_SelectedMOCode"] = ((BenQGuru.eMES.SelectQuery.SelectReworkSheet)obj).MOCode;
            row["Selector_SelectedItemCode"] = ((BenQGuru.eMES.SelectQuery.SelectReworkSheet)obj).ItemName;
            return row;
		}

		protected override DataRow GetUnSelectedGridRow(object obj)
		{
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((BenQGuru.eMES.SelectQuery.SelectReworkSheet)obj).ReworkCode ;
            row["Selector_UnSelectedMOCode"] = ((BenQGuru.eMES.SelectQuery.SelectReworkSheet)obj).MOCode ;
            row["Selector_UnSelectedItemCode"] = ((BenQGuru.eMES.SelectQuery.SelectReworkSheet)obj).ItemName;
            return row;
		}

		protected override object[] LoadSelectedDataSource(int inclusive, int exclusive)
		{
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}
			return this.facade.QuerySelectedReworkSheet( this.GetSelectedCodes() ) ;
		}

		protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
		{
			
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}

			return this.facade.QueryUnSelectedReworkSheet( 
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtModelCodeQuery.Text) ),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text) ),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtMOCodeQuery.Text) ), 
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtReworkSheetQuery.Text) ), 
				this.GetSelectedCodes(),
				inclusive, exclusive ) ;
		}


		protected override int GetUnSelectedRowCount()
		{
			
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}

			return this.facade.QueryUnSelectedReworkSheetCount( 
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtModelCodeQuery.Text) ),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text) ),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtMOCodeQuery.Text) ), 
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtReworkSheetQuery.Text) ), 
				this.GetSelectedCodes());
		}
        
		#endregion

		private void factory_load()
		{
			if(!Page.IsPostBack)
			{
				WarehouseFacade whFacade = new WarehouseFacade(base.DataProvider);
				object[]  factorys  = whFacade.GetAllFactory();
				if( factorys != null )
				{
					foreach( BenQGuru.eMES.Domain.Warehouse.Factory _factory in factorys)
					{
						this.drpFactory.Items.Add( _factory.FactoryCode ) ;
					}
				}
				new DropDownListBuilder( this.drpFactory ).AddAllItem( this.languageComponent1 ) ;
			}
		}

	}
}
