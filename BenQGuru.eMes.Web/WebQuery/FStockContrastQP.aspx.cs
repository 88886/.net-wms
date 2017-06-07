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

using Infragistics.WebUI.UltraWebGrid;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FStockContrastQP ��ժҪ˵����
	/// </summary>
	public partial class FStockContrastQP : BaseQPage
	{

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
			this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
		#endregion

		protected GridHelper _gridHelper = null;
		protected WebQueryHelper _helper = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelper(this.gridWebGrid);

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);

			if ( !IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();
				this.dateStockDateFromQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(DateTime.Now));
				this.dateStockDateToQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(DateTime.Now));
			}
		}

		private void _initialWebGrid()
		{
			this._gridHelper.AddColumn("StockRunningCard","��Ʒ���к�",null);
			this._gridHelper.AddColumn("StockInTicketNO","��ⵥ��",null);
			this._gridHelper.AddColumn("StockOutTicketNO","��������",null);
			this._gridHelper.AddColumn("StockInDate","�������",null);
			this._gridHelper.AddColumn("StockOutCollectDate","������������",null);
			this._gridHelper.AddColumn("StockInUser","�����Ա",null);
			this._gridHelper.AddColumn("StockOutUser","������Ա",null);

			//������
			this._gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"StockRunningCard",
								"StockInTicketNo",
								"StockOutTicketNo",
								"StockInDate",
								"StockOutCollectDate",
								"StockInUser",
								"StockOutUser"
							};
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
			PageCheckManager manager = new PageCheckManager();
			manager.Add( new DateRangeCheck(this.lblStockDateFromQuery, this.dateStockDateFromQuery.Text, this.dateStockDateToQuery.Text, true) );

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return;
			}	

			FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
			object[] dataSource = facadeFactory.CreateQueryStockFacade().QueryStockContrast(
				this.rdbStockStatusQuery.SelectedIndex,
				FormatHelper.TODateInt(this.dateStockDateFromQuery.Text),
				FormatHelper.TODateInt(this.dateStockDateToQuery.Text),
				( e as WebQueryEventArgs ).StartRow,
				( e as WebQueryEventArgs ).EndRow);

			( e as WebQueryEventArgs ).GridDataSource = dataSource;

			( e as WebQueryEventArgs ).RowCount = 
				facadeFactory.CreateQueryStockFacade().QueryStockContrastCount(
				this.rdbStockStatusQuery.SelectedIndex,
				FormatHelper.TODateInt(this.dateStockDateFromQuery.Text),
				FormatHelper.TODateInt(this.dateStockDateToQuery.Text));
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				QStockContrast obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as QStockContrast;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.RunningCard,
													  obj.StockInTicketNo,
													  obj.StockOutTicketNo,
													  FormatHelper.ToDateString(obj.StockInDate),
													  FormatHelper.ToDateString(obj.StockOutDate),
													  obj.StockInUser,
													  obj.StockOutUser
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				QStockContrast obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as QStockContrast;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.RunningCard,
									obj.StockInTicketNo,
									obj.StockOutTicketNo,
									FormatHelper.ToDateString(obj.StockInDate),
									FormatHelper.ToDateString(obj.StockOutDate),
									obj.StockInUser,
									obj.StockOutUser
								};
			}
		}
	}
}
