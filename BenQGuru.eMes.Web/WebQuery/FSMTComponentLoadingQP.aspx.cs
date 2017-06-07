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
using BenQGuru.eMES.Domain.SMT;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FSMTComponentLoadingQP ��ժҪ˵����
	/// </summary>
	public partial class FSMTComponentLoadingQP : BaseQPage
	{

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected GridHelper _gridHelper = null;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
			
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelper(this.gridWebGrid);

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);			

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
				this._initialWebGrid();
			}	
		}

		private void _initialWebGrid()
		{
			this._gridHelper.AddColumn("MOCode",				"����",null);
			this._gridHelper.AddColumn("ResourceCode",			"��Դ",null);
			this._gridHelper.AddColumn("StationCode",			"վλ",null);
			this._gridHelper.AddColumn("FeederCode",			"Feeder",null);
			this._gridHelper.AddColumn("OPBOMItemCode",			"���Ϻ�",null);
			this._gridHelper.AddColumn("LotNO",				"��������",null);
			this._gridHelper.AddColumn("DateCode",			"��������",null);
			this._gridHelper.AddColumn("VendorCode",		"��Ӧ��",null);
			this._gridHelper.AddColumn("VenderItemCode",	"��Ӧ���Ϻ�",null);
			this._gridHelper.AddColumn("Version",		"���ϰ汾",null);			
			this._gridHelper.AddColumn( "BIOS",			"BIOS",	null);
			this._gridHelper.AddColumn( "PCBA",			"PCBA",	null);
			this._gridHelper.AddColumn( "MaintainDate",		"ά������",	null);
			this._gridHelper.AddColumn( "MaintainTime",		"ά��ʱ��",	null);	

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
				facadeFactory.CreateQuerySMTComponentLoadingFacade().QuerySMTComponentLoading(
				FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
				FormatHelper.CleanString(this.txtConditionResource.Text).ToUpper(),
				( e as WebQueryEventArgs ).StartRow,
				( e as WebQueryEventArgs ).EndRow);

			( e as WebQueryEventArgs ).RowCount = 
				facadeFactory.CreateQuerySMTComponentLoadingFacade().QuerySMTComponentLoadingCount(
				FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
				FormatHelper.CleanString(this.txtConditionResource.Text).ToUpper());
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				SMTResourceBOM obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as SMTResourceBOM;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.MOCode,
													  obj.ResourceCode,
													  obj.StationCode,
													  obj.FeederCode,													  
													  obj.OPBOMItemCode,
													  obj.LotNO,
													  obj.DateCode,
													  obj.VendorCode,
													  obj.VenderItemCode,
													  obj.Version,
													  obj.BIOS,
													  obj.PCBA,
													  FormatHelper.ToDateString(obj.MaintainDate),
													  FormatHelper.ToTimeString(obj.MaintainTime)
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				SMTResourceBOM obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as SMTResourceBOM;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.MOCode,
									obj.ResourceCode,
									obj.StationCode,
									obj.FeederCode,													  
									obj.OPBOMItemCode,
									obj.LotNO,
									obj.DateCode,
									obj.VendorCode,
									obj.VenderItemCode,
									obj.Version,
									obj.BIOS,
									obj.PCBA,
									FormatHelper.ToDateString(obj.MaintainDate),
									FormatHelper.ToTimeString(obj.MaintainTime)
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"MOCode",
								"ResourceCode",
								"StationCode",
								"FeederCode",
								"OPBOMItemCode",
								"LotNO",
								"DateCode",
								"VendorCode",
								"VenderItemCode",
								"Version",
								"BIOS",
								"PCBA",
								"MaintainDate",
								"MaintainTime"
							};
		}		
	}
}
