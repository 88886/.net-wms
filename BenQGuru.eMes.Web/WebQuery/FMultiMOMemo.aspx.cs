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

using BenQGuru.eMES.Web.Helper ;
using BenQGuru.eMES.WebQuery ;

using Infragistics.WebUI.UltraWebGrid ;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FConfigEx ��ժҪ˵����
	/// </summary>
	public partial class FMultiMOMemo: BaseQPage
	{

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected GridHelper _gridHelper = null;
	
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
			this._gridHelper.AddColumn("SN",				"��Ʒ���к�",null);
			this._gridHelper.AddColumn("Memo",			"��ע",null);		
			this._gridHelper.AddColumn("MOCode",				"����",null);
			this._gridHelper.AddColumn("ItemCode",			"��Ʒ����",null);
			this._gridHelper.AddColumn("CollectionOperationCode",				"�ɼ���λ",null);
			this._gridHelper.AddColumn( "CollectionStepSequenceCode",		"�ɼ��߱�",	null);
			this._gridHelper.AddColumn( "CollectionResourceCode",		"�ɼ���Դ",	null);
			this._gridHelper.AddColumn( "EmployeeNo",		"Ա������",	null);
			this._gridHelper.AddColumn( "CollectionDate",		"�ɼ�����",	null);
			this._gridHelper.AddColumn( "CollectionTime",		"�ɼ�ʱ��",	null);

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

		#region

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			FacadeFactory facadeFactory = new FacadeFactory (base.DataProvider) ;

			( e as WebQueryEventArgs ).GridDataSource = facadeFactory.CreateQueryMultiMOMemoFacade().QueryMultiMOMemo
				( FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtConditionItem.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtConditionMo.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtRCardStart.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtRCardEnd.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtStartMeno.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtEndMeno.Text )),
				( e as WebQueryEventArgs ).StartRow,
				( e as WebQueryEventArgs ).EndRow
				);

			( e as WebQueryEventArgs ).RowCount = facadeFactory.CreateQueryMultiMOMemoFacade().QueryMultiMOMemoCount
				( FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtConditionItem.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtConditionMo.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtRCardStart.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtRCardEnd.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtStartMeno.Text )),
				FormatHelper.CleanString( FormatHelper.PKCapitalFormat( this.txtEndMeno.Text )) ) ;
			
		}

		
		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				QDOMultiMOMemo obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as QDOMultiMOMemo;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.RunningCard,
													  obj.Meno,
													  obj.MoCode,
													  obj.ItemCode,													  
													  obj.OPCode,
													  obj.StepSequenceCode,
													  obj.ResourceCode,
													  obj.MaintainUser,
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
				QDOMultiMOMemo obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as QDOMultiMOMemo;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.RunningCard,
									obj.Meno,
									obj.MoCode,
									obj.ItemCode,													  
									obj.OPCode,
									obj.StepSequenceCode,
									obj.ResourceCode,
									obj.MaintainUser,
									FormatHelper.ToDateString(obj.MaintainDate),
									FormatHelper.ToTimeString(obj.MaintainTime)
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"SN",
								"Memo",
								"MOCode",
								"ItemCode",
								"CollectionOperationCode",
								"CollectionStepSequenceCode",
								"CollectionResourceCode",
								"EmployeeNo",
								"CollectionDate",
								"CollectionTime"
							};
		}	
		#endregion


	}
}
