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
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FTSRecordQP ��ժҪ˵����
	/// </summary>
	public partial class FPackingDetailsQP : BaseQPageNew
	{
        //protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        //private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelperNew _helper = null;

		//protected GridHelperNew gridHelper = null;
		private QueryFacade2 _facade = null ;// FacadeFactory.CreateQueryFacade2() ;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.gridHelper = new GridHelperNew(this.gridWebGrid,this.DtSource);
			this.txtCartonNoQuery.Text = this.GetRequestParam("cartonno");
			this.txtCartonCollectedQuery.Text = this.GetRequestParam("collected");
			this.txtCartonMemoQuery.Text = this.GetRequestParam("memo");
			
			this.gridHelper = new GridHelperNew(this.gridWebGrid,this.DtSource);
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this._initialWebGrid();
            }
			this._helper = new WebQueryHelperNew( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1,this.DtSource );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);
			this._helper.Query(null);
            //this._initialWebGrid();
			
		}

		private void _initialWebGrid()
		{
            base.InitWebGrid();
			this.gridHelper.AddColumn("RCard","��Ʒ���к�",null) ;
			this.gridHelper.AddColumn( "ItemCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "MOCode", "��������",	null);

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
			return true;
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
			if(_facade==null)
			{
				_facade = new FacadeFactory(base.DataProvider).CreateQueryFacade2() ;
			}
			( e as WebQueryEventArgsNew ).GridDataSource = this._facade.QueryPackingInfoRCard1(
				this.txtCartonNoQuery.Text.Trim().ToUpper(),
				( e as WebQueryEventArgsNew ).StartRow,
				( e as WebQueryEventArgsNew ).EndRow);
			( e as WebQueryEventArgsNew ).RowCount = this._facade.QueryPackingInfoRCardCount1(
				this.txtCartonNoQuery.Text.Trim().ToUpper());
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			BenQGuru.eMES.Domain.DataCollect.SimulationReport obj = ( e as DomainObjectToGridRowEventArgsNew ).DomainObject as BenQGuru.eMES.Domain.DataCollect.SimulationReport;
            DataRow row = this.DtSource.NewRow();
            row["RCard"] = obj.RunningCard;
            row["ItemCode"] = obj.ItemCode;
            row["MOCode"] = obj.MOCode;
            (e as DomainObjectToGridRowEventArgsNew).GridRow = row;
                //new UltraGridRow( new object[]{
                //                                  obj.RunningCard,
                //                                  obj.ItemCode,
                //                                  obj.MOCode 
                //                              }
                //);
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
            BenQGuru.eMES.Domain.DataCollect.SimulationReport obj = (e as DomainObjectToExportRowEventArgsNew).DomainObject as BenQGuru.eMES.Domain.DataCollect.SimulationReport;
			( e as DomainObjectToExportRowEventArgsNew ).ExportRow = 
				new string[]{
								obj.RunningCard,
								obj.ItemCode,
								obj.MOCode 
							};
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgsNew ).Heads = 
				new string[]{
								"RCard",
								"ItemCode",
								"MOCode"
							};

		}

		protected void cmdReturn_ServerClick(object sender, EventArgs e)
		{
			this.Response.Redirect("FPackingQP.aspx");
		}
	}
}
