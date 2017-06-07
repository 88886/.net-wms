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
using BenQGuru.eMES.Domain.TS;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FSNNGListQP ��ժҪ˵����
	/// </summary>
	public partial class FSNNGListQP : BaseQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected GridHelper _gridHelper = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//this.pagerSizeSelector.Readonly = true;

			this.txtSNQuery.Text = this.GetRequestParam("RunningCard");

			this._gridHelper = new GridHelper(this.gridWebGrid);

			this._helper = new WebQueryHelper( null,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);				

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();
				this._helper.Query(sender);
			}
		}

		private void _initialWebGrid()
		{
			this._gridHelper.AddColumn("CollectionStepSequenceCode",			"�ɼ��߱�",null);
			this._gridHelper.AddColumn("CollectionResourceCode",				"�ɼ���Դ",null);
			this._gridHelper.AddColumn("ErrorCodeGroup","����������",null);
			this._gridHelper.AddColumn("ErrorCodeGroupDesc","��������������",null);
			this._gridHelper.AddColumn("ErrorCode",		"��������",null);
			this._gridHelper.AddColumn("ErrorCodeDesc","������������",null);
			this._gridHelper.AddColumn( "CollectionDate",		"�ɼ�����",	null);
			this._gridHelper.AddColumn( "CollectionTime",		"�ɼ�ʱ��",	null);		
			this._gridHelper.AddColumn( "FrmMemo",		"��ע",	null);

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
			manager.Add( new LengthCheck(this.lblSN,this.txtSNQuery,System.Int32.MaxValue,true) );

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return false;
			}	
			return true;
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			if( this._checkRequireFields() )
			{
				FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
				( e as WebQueryEventArgs ).GridDataSource = 
					facadeFactory.CreateQuerySNNGFacade().QuerySNNGDetails(	
					"",
					FormatHelper.CleanString(this.txtSNQuery.Text).ToUpper(),
					"",
					"",
					( e as WebQueryEventArgs ).StartRow,
					( e as WebQueryEventArgs ).EndRow);

				( e as WebQueryEventArgs ).RowCount = 
					facadeFactory.CreateQuerySNNGFacade().QuerySNNGDetailsCount(	
					"",
					FormatHelper.CleanString(this.txtSNQuery.Text).ToUpper(),
					"",
					"");
			}
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				QDOSNNGList obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as QDOSNNGList;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.StepSequenceCode,
													  obj.ResourceCode,
													  obj.ErrorCodeGroup,
													  obj.ErrorCodeGroupDesc ,	
													  obj.ErrorCode		,
													  obj.ErrorCodeDesc ,	
													  FormatHelper.ToDateString(obj.MaintainDate),
													  FormatHelper.ToTimeString(obj.MaintainTime),
													  obj.FrmMemo					  
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				QDOSNNGList obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as QDOSNNGList;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.StepSequenceCode,
									obj.ResourceCode,
									obj.ErrorCodeGroup,
									obj.ErrorCodeGroupDesc,
									obj.ErrorCode,
									obj.ErrorCodeDesc,
									FormatHelper.ToDateString(obj.MaintainDate),
									FormatHelper.ToTimeString(obj.MaintainTime),
									obj.FrmMemo
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{	
								"CollectionStepSequenceCode",
								"CollectionResourceCode",
								"ErrorCodeGroup",
								"ErrorCodeGroupDesc",
								"ErrorCode",
								"ErrorCodeDesc",
								"CollectionDate",
								"CollectionTime",
								"IT_MEMO"
							};
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect("FSNNGQP.aspx");
		}		
	}
}
