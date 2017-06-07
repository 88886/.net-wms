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
	/// FTSChangedItemQP ��ժҪ˵����
	/// </summary>
	public partial class FTSChangedItemQP2 : BaseRQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
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
						this.SourceResourceTime =  FormatHelper.TOTimeInt(tsTime);
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
			this._gridHelper.GridHelper.AddColumn( "MItemCode1", "�������Ϻ�",	null);
			this._gridHelper.GridHelper.AddColumn( "MCard1", "���������к�",	null);
			this._gridHelper.GridHelper.AddColumn( "SItemCode1", "ԭ�����Ϻ�",	null);
			this._gridHelper.GridHelper.AddColumn( "MSCard1", "ԭ�������к�",	null);
			this._gridHelper.GridHelper.AddColumn( "Location1", "���λ��",	null);
			this._gridHelper.GridHelper.AddColumn( "LotNO2", "����",	null);
			this._gridHelper.GridHelper.AddColumn( "VendorCode",		"����",	null);
			this._gridHelper.GridHelper.AddColumn("VendorItemCode",			"�����Ϻ�",null);
			this._gridHelper.GridHelper.AddColumn("DateCode",			"��������",null);		
			this._gridHelper.GridHelper.AddColumn( "Reversion",		"��Ʒ�汾",	null);
			this._gridHelper.GridHelper.AddColumn( "PCBA",		"PCBA�汾",	null);
			this._gridHelper.GridHelper.AddColumn( "BIOS",		"BIOS�汾",	null);
			this._gridHelper.GridHelper.AddColumn( "MEMO",		"����˵��",	null);

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );
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
					facadeFactory.CreateQueryTSChangedPartsFacade().QueryTSChangedParts(						
					"",
					"",
					FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper(),
					FormatHelper.CleanString(this.txtSnQuery.Text).ToUpper(),
					"",
					FormatHelper.CleanString(this.txtRepaireResourceQuery.Text).ToUpper(),
					"",
					( e as WebQueryEventArgs ).StartRow,
					( e as WebQueryEventArgs ).EndRow);

				( e as WebQueryEventArgs ).RowCount = 
					facadeFactory.CreateQueryTSChangedPartsFacade().QueryTSChangedPartsCount(						
					"",
					"",
					FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper(),
					FormatHelper.CleanString(this.txtSnQuery.Text).ToUpper(),
					"",
					FormatHelper.CleanString(this.txtRepaireResourceQuery.Text).ToUpper(),
					"");
			}
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				TSItem obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as TSItem;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.MItemCode,
													  obj.MCard,
													  obj.SourceItemCode ,
													  obj.MSourceCard ,
													  obj.Location,
													  obj.LotNO,
													  obj.VendorCode,
													  obj.VendorItemCode,
													  obj.DateCode,													  
													  obj.Reversion,
													  obj.PCBA,
													  obj.BIOS,
													  obj.MEMO
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				TSItem obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as TSItem;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.SourceItemCode,
									obj.MSourceCard,
									obj.SourceItemCode ,
									obj.MSourceCard ,
									obj.Location,
									obj.LotNO,
									obj.VendorCode,
									obj.VendorItemCode,
									obj.DateCode,													  
									obj.Reversion,
									obj.PCBA,
									obj.BIOS,
									obj.MEMO
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"MItemCode1",
								"MCard1",
								"SItemCode1",
								"MSCard1",
								"Location1",
								"LotNO2",
								"VendorCode",
								"VendorItemCode",
								"DateCode",
								"Reversion",
								"PCBA",
								"BIOS",
								"MEMO"
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
