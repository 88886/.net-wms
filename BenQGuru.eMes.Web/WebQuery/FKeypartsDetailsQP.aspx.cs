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
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Domain.DataCollect;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FKeypartsDetailsQP ��ժҪ˵����
	/// </summary>
	public partial class FKeypartsDetailsQP : BaseQPageNew
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelperNew _helper = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//this.pagerSizeSelector.Readonly = true;

            this.txtKeypartsQuery.Text = this.GetRequestParam("Keyparts");

			gridHelper = new GridHelperNew(this.gridWebGrid,DtSource);

			this._helper = new WebQueryHelperNew( null,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 ,DtSource);
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
				this.displayLable();
			}
		}

		private void displayLable()
		{
			string referedURL = this.GetRequestParam("RETURNPAGEURL") ;
			if( referedURL == "FKeypartsComponentLoadingQP.aspx")
			{
                this.lblTitle.Text = this.languageComponent1.GetString("$PageControl_KeypartsTitle");
			}
			else if(referedURL == "FKeypartsComponentDownQP.aspx")
			{
                this.lblTitle.Text = this.languageComponent1.GetString("$PageControl_KeypartsCut");
			}

		}

		private void _initialWebGrid()
		{
            base.InitWebGrid();
			this.gridHelper.AddColumn("MItemCode",				"�Ϻ�",null);
			this.gridHelper.AddColumn("VendorCode",			"���̴���",null);
			this.gridHelper.AddColumn("VendorItemCode",				"�����Ϻ�",null);
			this.gridHelper.AddColumn("LotNO",			"����",null);
			this.gridHelper.AddColumn("DateCode",			"��������",null);
			this.gridHelper.AddColumn("Version",			"�汾",null);
			this.gridHelper.AddColumn("PCBA",				"PCBA",null);
			this.gridHelper.AddColumn("BIOS",			"BIOS",null);
			this.gridHelper.AddColumn("MaintainUser",			"�ɼ���Ա",null);
			this.gridHelper.AddColumn( "MaintainDate",		"ά������",	null);
			this.gridHelper.AddColumn( "MaintainTime",		"ά��ʱ��",	null);		

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );			
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
			manager.Add( new LengthCheck(this.lblKeyparts,this.txtKeypartsQuery,System.Int32.MaxValue,true) );

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
				( e as WebQueryEventArgsNew ).GridDataSource = 
					facadeFactory.CreateQueryComponentLoadingFacade().QueryKeypartsListDetails(						
					FormatHelper.CleanString(this.txtKeypartsQuery.Text).ToUpper(),
					( e as WebQueryEventArgsNew ).StartRow,
					( e as WebQueryEventArgsNew ).EndRow);

				( e as WebQueryEventArgsNew ).RowCount = 
					facadeFactory.CreateQueryComponentLoadingFacade().QueryKeypartsListDetailsCount(						
					FormatHelper.CleanString(this.txtKeypartsQuery.Text).ToUpper());
			}
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgsNew ).DomainObject != null )
			{
				OnWIPItem obj = ( e as DomainObjectToGridRowEventArgsNew ).DomainObject as OnWIPItem;
                DataRow row = DtSource.NewRow();
                row["MItemCode"] = obj.MItemCode;
                row["VendorCode"] = obj.VendorCode;
                row["VendorItemCode"] = obj.VendorItemCode;
                row["LotNO"] = obj.LotNO;
                row["DateCode"] = obj.DateCode;
                row["Version"] = obj.Version;
                row["PCBA"] = obj.PCBA;
                row["BIOS"] = obj.BIOS;
                row["MaintainUser"] = obj.MaintainUser;
                row["MaintainDate"] = FormatHelper.ToDateString(obj.MaintainDate);
                row["MaintainTime"] = FormatHelper.ToTimeString(obj.MaintainTime);
                (e as DomainObjectToGridRowEventArgsNew).GridRow = row;
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgsNew ).DomainObject != null )
			{
				OnWIPItem obj = ( e as DomainObjectToExportRowEventArgsNew ).DomainObject as OnWIPItem;
				( e as DomainObjectToExportRowEventArgsNew ).ExportRow = 
					new string[]{
									obj.MItemCode,
									obj.VendorCode,
									obj.VendorItemCode,
									obj.LotNO,
									obj.DateCode,
									obj.Version,
									obj.PCBA,
									obj.BIOS,
									obj.MaintainUser,
									FormatHelper.ToDateString(obj.MaintainDate),
									FormatHelper.ToTimeString(obj.MaintainTime)
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgsNew ).Heads = 
				new string[]{
								"MItemCode",
								"VendorCode",
								"VendorItemCode",
								"LotNO",
								"DateCode",
								"Version",
								"PCBA",
								"BIOS",
								"EmployeeNo",
								"ComponentLoadingDate",
								"ComponentLoadingTime"
							};
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			string referedURL = this.GetRequestParam("RETURNPAGEURL") ;
			if( referedURL == string.Empty)
			{
				referedURL = "FKeypartsComponentLoadingQP.aspx" ;
			}
			else
			{
				referedURL = System.Web.HttpUtility.UrlDecode(referedURL) ;
			}
			Response.Redirect( referedURL ) ;
		}		
	}
}
