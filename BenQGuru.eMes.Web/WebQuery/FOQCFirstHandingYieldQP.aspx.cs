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
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FOQCFirstHandingYieldQP ��ժҪ˵����
	/// </summary>
	public partial class FOQCFirstHandingYieldQP : BaseQPage
	{
		protected System.Web.UI.WebControls.Label lblModelCodeQuery;

		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected ExcelExporter excelExporter = null;
		private System.ComponentModel.IContainer components;

		protected WebQueryHelper _helper = null;
		protected GridHelper _gridHelper = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelper(this.gridWebGrid);

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				this.dateStartDateQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.dateEndDateQuery.Text = this.dateStartDateQuery.Text;
			}

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);
			this._helper.GridCellClick +=new EventHandler(_helper_GridCellClick);
		}

		private void _initialWebGrid()
		{
			this._gridHelper.AddColumn("ModelCode","��Ʒ�����",null);
			this._gridHelper.AddColumn("ItemCode","��Ʒ����",null);
			this._gridHelper.AddColumn("FirstHandingAmount","һ���ύ����",null);
			this._gridHelper.AddColumn("FirstHandingYieldAmount","һ���ϸ���",null);
			this._gridHelper.AddColumn("FirstHandingYieldPercent","һ�ν���ϸ���",null);
			this._gridHelper.AddLinkColumn("FirstHandingDetail",	"һ���ύ��ϸ",null);
			this.gridWebGrid.Columns.FromKey("FirstHandingDetail").Width=new Unit("10%");

			//������
			this._gridHelper.ApplyLanguage( this.languageComponent1 );
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

		}
		#endregion

		private bool _checkRequireFields()
		{			
			PageCheckManager manager = new PageCheckManager();
			manager.Add( new DateRangeCheck(this.lblStartDateQuery,this.dateStartDateQuery.Text,this.lblEndDateQuery,this.dateEndDateQuery.Text,true));

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return true;
			}	
			return true;
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			if( this._checkRequireFields() )
			{
				FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
				( e as WebQueryEventArgs ).GridDataSource = 
					facadeFactory.CreateQueryFacade1().QueryFirstHandingYield(
					"",
					this.txtItem.Text,
					FormatHelper.TODateInt(this.dateStartDateQuery.Text),
					FormatHelper.TODateInt(this.dateEndDateQuery.Text),
					( e as WebQueryEventArgs ).StartRow,
					( e as WebQueryEventArgs ).EndRow);

				( e as WebQueryEventArgs ).RowCount = 
					facadeFactory.CreateQueryFacade1().QueryFirstHandingYieldCount(
					"",
					this.txtItem.Text,
					FormatHelper.TODateInt(this.dateStartDateQuery.Text),
					FormatHelper.TODateInt(this.dateEndDateQuery.Text) );
			}
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				OQCFirstHandingYield obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as OQCFirstHandingYield;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.ModelCode,
													  obj.ItemCode,
													  obj.FirstHandingAmount,
													  obj.FirstHandingYieldAmount,
													  System.Decimal.Round((obj.FirstHandingYieldPercent * 100),2) + "%",
													  ""
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				OQCFirstHandingYield obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as OQCFirstHandingYield;				
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.ModelCode,
									obj.ItemCode,
									obj.FirstHandingAmount.ToString(),
									obj.FirstHandingYieldAmount.ToString(),
									System.Decimal.Round((obj.FirstHandingYieldPercent * 100),2) + "%"
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"ModelCode",
								"ItemCode",
								"FirstHandingAmount",
								"FirstHandingYieldAmount",
								"FirstHandingYieldPercent"
							};
		}

		#region GridCellButtnClick

		private void _helper_GridCellClick(object sender, EventArgs e)
		{
			if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "FirstHandingDetail".ToUpper() )
			{			
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FOQCFirstHandingYieldDetailQP.aspx",
					new string[]{
									"MODELCODE",
									"ITEMCODE",
									"STARTDATE",
									"ENDDATE",
									"REFEREDURL",
								},
					new string[]{
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("ModelCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("ItemCode").Text,
									this.dateStartDateQuery.Text,
									this.dateEndDateQuery.Text,
									"FOQCFirstHandingYieldQP.aspx",
								})
					);
			}
		}

		#endregion
	}
}

