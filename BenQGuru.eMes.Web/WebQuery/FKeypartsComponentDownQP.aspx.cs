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
using BenQGuru.eMES.Domain.DataCollect;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FKeypartsComponentLoadingQP ��ժҪ˵����
	/// </summary>
	public partial class FKeypartsComponentDownQP : BaseQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected GridHelper _gridHelper = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelper(this.gridWebGrid);

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);				
			this._helper.GridCellClick +=new EventHandler(_helper_GridCellClick);
			FormatHelper.SetSNRangeValue(txtStartSnQuery,txtEndSnQuery);
			FormatHelper.SetSNRangeValue(txtStartKeypartsQuery,txtEndKeypartsQuery);

			if(!this.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();
			}
		}

		private void _initialWebGrid()
		{
			this._gridHelper.AddColumn("MOCode",				"����",null);
			this._gridHelper.AddColumn("ItemCode",			"��Ʒ����",null);			
			this._gridHelper.AddColumn("SN",			"��Ʒ���к�",null);
			this._gridHelper.AddColumn("ComponentLoadingOPCode1",			"���Ϲ�λ",null);
			this._gridHelper.AddColumn("ComponentLoadingStepSequenceCode1",			"�����߱�",null);			
			this._gridHelper.AddColumn("ComponentLoadingResourceCode1",				"������Դ",null);
			this._gridHelper.AddColumn("Keyparts",			"�ؼ��Ϻ�",null);
			this._gridHelper.AddColumn("EmployeeNo",				"Ա������",null);
			this._gridHelper.AddColumn( "ComponentLoadingDate1",		"��������",	null);
			this._gridHelper.AddColumn( "ComponentLoadingTime1",		"����ʱ��",	null);
			this._gridHelper.AddColumn( "INNO",		"KeyPats���к�",	null);
			this._gridHelper.AddLinkColumn("KeypartsDetails","Keyparts��ϸ",null);

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
			manager.Add( new LengthCheck(this.lblMOIDQuery,this.txtConditionMo.TextBox,System.Int32.MaxValue,true) );

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return false;
			}	
			return true;
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
			( e as WebQueryEventArgs ).GridDataSource = 
				facadeFactory.CreateQueryComponentLoadingFacade().QueryDownKeyparts(
				FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
				FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
				FormatHelper.CleanString(this.txtStartSnQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtEndSnQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtStartKeypartsQuery.Text.ToUpper()),
				FormatHelper.CleanString(this.txtEndKeypartsQuery.Text.ToUpper()),
				( e as WebQueryEventArgs ).StartRow,
				( e as WebQueryEventArgs ).EndRow);

			( e as WebQueryEventArgs ).RowCount = 
				facadeFactory.CreateQueryComponentLoadingFacade().QueryDownKeypartsCount(
				FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
				FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
				FormatHelper.CleanString(this.txtStartSnQuery.Text.ToUpper()),
				FormatHelper.CleanString(this.txtEndSnQuery.Text.ToUpper()),
				FormatHelper.CleanString(this.txtStartKeypartsQuery.Text.ToUpper()),
				FormatHelper.CleanString(this.txtEndKeypartsQuery.Text.ToUpper()));
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				QDOKeyparts obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as QDOKeyparts;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.MoCode,
													  obj.ItemCode,
													  obj.SN,
													  obj.OperationCode,													  
													  obj.StepSequenceCode,
													  obj.ResourceCode,
													  obj.MItemCode,
													  obj.MaintainUser,
													  FormatHelper.ToDateString(obj.MaintainDate),
													  FormatHelper.ToTimeString(obj.MaintainTime),
													  obj.INNO,
													  ""
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				QDOKeyparts obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as QDOKeyparts;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.MoCode,
									obj.ItemCode,
									obj.SN,
									obj.OperationCode,													  
									obj.StepSequenceCode,
									obj.ResourceCode,
									obj.MItemCode,
									obj.MaintainUser,
									FormatHelper.ToDateString(obj.MaintainDate),
									FormatHelper.ToTimeString(obj.MaintainTime),
									obj.INNO
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"MOCode",
								"ItemCode",
								"SN",
								"ComponentLoadingOPCode",
								"ComponentLoadingStepSequenceCode",
								"ComponentLoadingResourceCode",
								"Keyparts",
								"EmployeeNo",
								"ComponentLoadingDate",
								"ComponentLoadingTime",
								"INNO"
							};
		}		

		private void _helper_GridCellClick(object sender, EventArgs e)
		{
			if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "KeypartsDetails".ToUpper() )
			{
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FKeypartsDetailsQP.aspx",
					new string[]{
									"INNO",
									"RETURNPAGEURL"
								},
					new string[]{
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("INNO").Text,
									"FKeypartsComponentDownQP.aspx"
								})
					);
			}
		}			
	}
}
