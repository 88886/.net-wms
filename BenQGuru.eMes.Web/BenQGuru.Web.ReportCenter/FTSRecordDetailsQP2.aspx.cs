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
	/// FTSRecordDetailsQP ��ժҪ˵����
	/// </summary>
	public partial class FTSRecordDetailsQP2 : BaseRQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected System.Web.UI.WebControls.TextBox txtRepaireOperationQuery;
		protected GridHelperForRPT _gridHelper = null;

		#region ViewState
		private int SourceResourceDate
		{
			get
			{
				if( this.ViewState["SourceResourceDate"] != null )
				{
					try
					{
						return System.Int32.Parse(this.ViewState["SourceResourceDate"].ToString());
					}
					catch
					{
						return 0;
					}
				}
				else
				{
					return 0;
				}
			}
			set
			{
				this.ViewState["SourceResourceDate"] = value;
			}
		}

		private int SourceResourceTime
		{
			get
			{
				if( this.ViewState["SourceResourceTime"] != null )
				{
					try
					{
						return System.Int32.Parse(this.ViewState["SourceResourceTime"].ToString());
					}
					catch
					{
						return 0;
					}
				}
				else
				{
					return 0;
				}
			}
			set
			{
				this.ViewState["SourceResourceTime"] = value;
			}
		}

		private string RunningCardSeq
		{
			get
			{
				if( this.ViewState["RunningCardSeq"] != null )
				{
					try
					{
						return this.ViewState["RunningCardSeq"].ToString();
					}
					catch
					{
						return "0";
					}
				}
				else
				{
					return "0";
				}
			}
			set
			{
				this.ViewState["RunningCardSeq"] = value;
			}
		}
		#endregion
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._initialParamter();			

			this._gridHelper = new GridHelperForRPT(this.gridWebGrid);

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

		private void _initialParamter()
		{
			this.txtModelQuery.Text = this.GetRequestParam("ModelCode");
			this.txtItemQuery.Text = this.GetRequestParam("ItemCode");
			this.txtMoQuery.Text = this.GetRequestParam("MoCode");
			this.txtSnQuery.Text = this.GetRequestParam("RunningCard");
			this.txtTsStateQuery.Text = this.GetRequestParam("TSState");
			this.txtRepaireResourceQuery.Text = this.GetRequestParam("TSResourceCode");
			this.txtScrapCauseQuery.Text = this.GetRequestParam("ScrapCause");

			this.RunningCardSeq = this.GetRequestParam("RunningCardSeq");

			if( this.GetRequestParam("TSDate") != null )
			{
				string tsDate = this.GetRequestParam("TSDate");

				try
				{					
					this.SourceResourceDate = FormatHelper.TODateInt(tsDate);
				}
				catch
				{
					this.SourceResourceDate = 0;
				}

				if( this.GetRequestParam("TSTime") != null )
				{
					string tsTime = this.GetRequestParam("TSTime");

					try
					{
						this.SourceResourceTime = FormatHelper.TOTimeInt(tsTime);
					}
					catch
					{
						this.SourceResourceTime = 0;
					}
				}
			}
		}

		private void _initialWebGrid()
		{
			this._gridHelper.GridHelper.AddColumn("ErrorCodeGroup",	"����������",null);
			this._gridHelper.GridHelper.AddColumn("ErrorCode",			"��������",null);
			this._gridHelper.GridHelper.AddColumn("ErrorCause",			"����ԭ��",null);
			this._gridHelper.GridHelper.AddColumn( "ErrorLocation",		"����λ��",	null);
			this._gridHelper.GridHelper.AddColumn( "ErrorPart",		"����Ԫ��",	null);
			this._gridHelper.GridHelper.AddColumn("Solution",			"�������",null);
			this._gridHelper.GridHelper.AddColumn("Duty",			"���α�",null);		
			this._gridHelper.GridHelper.AddColumn( "Memo",		"����˵��",	null);
			this._gridHelper.GridHelper.AddColumn( "TsOperator",		"ά�޹�",	null);
			this._gridHelper.GridHelper.AddColumn( "MaintainDate",		"ά������",	null);
			this._gridHelper.GridHelper.AddColumn( "MaintainTime",		"ά��ʱ��",	null);

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );

			this.gridWebGrid.Columns.FromKey("ErrorCodeGroup").MergeCells = true;
			this.gridWebGrid.Columns.FromKey("ErrorCode").MergeCells = true;
			this.gridWebGrid.Columns.FromKey("ErrorCause").MergeCells = true;
			this.gridWebGrid.Columns.FromKey("Solution").MergeCells = true;
			this.gridWebGrid.Columns.FromKey("Duty").MergeCells = true;
			this.gridWebGrid.Columns.FromKey("Memo").MergeCells = true;
			this.gridWebGrid.Columns.FromKey("TsOperator").MergeCells = true;
			this.gridWebGrid.Columns.FromKey("MaintainDate").MergeCells = true;
			this.gridWebGrid.Columns.FromKey("MaintainTime").MergeCells = true;
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
//			PageCheckManager manager = new PageCheckManager();
//			manager.Add( new LengthCheck(this.lblModelQuery,this.txtModelQuery,System.Int32.MaxValue,true) );
//			manager.Add( new LengthCheck(this.lblItemQuery,this.txtItemQuery,System.Int32.MaxValue,true) );
//			manager.Add( new LengthCheck(this.lblMoQuery,this.txtMoQuery,System.Int32.MaxValue,true) );
//			manager.Add( new LengthCheck(this.lblSnQuery,this.txtSnQuery,System.Int32.MaxValue,true) );
//			manager.Add( new LengthCheck(this.lblTSStateQuery,this.txtTsStateQuery,System.Int32.MaxValue,true) );
//			manager.Add( new LengthCheck(this.lblRepaireOperationQuery,this.txtRepaireResourceQuery,System.Int32.MaxValue,true) );
//
//			if( !manager.Check() )
//			{
//				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
//				return true;
//			}	
			return true;
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			if( this._checkRequireFields() )
			{	
				FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
				( e as WebQueryEventArgs ).GridDataSource = 
					facadeFactory.CreateQueryTSDetailsFacade().QueryTSDetails(						
					FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper(),
					FormatHelper.CleanString(this.txtSnQuery.Text).ToUpper(),
					int.Parse(this.RunningCardSeq),
					( e as WebQueryEventArgs ).StartRow,
					( e as WebQueryEventArgs ).EndRow);

				( e as WebQueryEventArgs ).RowCount = 
					facadeFactory.CreateQueryTSDetailsFacade().QueryTSDetailsCount(	
					FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper(),
					FormatHelper.CleanString(this.txtSnQuery.Text).ToUpper(),
					int.Parse(this.RunningCardSeq));
				
			}
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				QDOTSDetails1 obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as QDOTSDetails1;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.ErrorCodeGroupDescription,
													  obj.ErrorCodeDescription,
													  obj.ErrorCauseDescription,
													  obj.ErrorLocation,
													  obj.ErrorParts,
													  obj.SolutionDescription,
													  obj.DutyDescription,													  
													  obj.Memo,
													  obj.TSOperator,
													  FormatHelper.ToDateString(obj.TSDate),
													  FormatHelper.ToTimeString(obj.TSTime)
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				QDOTSDetails1 obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as QDOTSDetails1;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.ErrorCodeGroupDescription,
									obj.ErrorCodeDescription,
									obj.ErrorCauseDescription,
									obj.ErrorLocation,
									obj.ErrorParts,
									obj.SolutionDescription,
									obj.DutyDescription,													  
									obj.Memo,
									obj.TSOperator,
									FormatHelper.ToDateString(obj.TSDate),
									FormatHelper.ToTimeString(obj.TSTime)
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"ErrorCodeGroup",
								"ErrorCode",
								"ErrorCause",
								"ErrorLocation",
								"ErrorParts",
								"Solution",
								"Duty",
								"Memo",
								"TsOperator",
								"MaintainDate",
								"MaintainTime"
							};
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			ArrayList keys = new ArrayList();
			ArrayList values = new ArrayList();

			for(int i =0;i<this.Request.QueryString.AllKeys.Length;i++)
			{
				if( this.Request.QueryString.AllKeys.GetValue(i).ToString().StartsWith("12_") )
				{
					keys.Add( this.Request.QueryString.AllKeys.GetValue(i).ToString() );
					values.Add( this.Request.QueryString[this.Request.QueryString.AllKeys.GetValue(i).ToString()] );
				}
			}

			this.Response.Redirect(
				this.MakeRedirectUrl(
				this.GetRequestParam("BackUrl"),(string[])keys.ToArray(typeof(string)),(string[])values.ToArray(typeof(string))));
		}		
	}
}
