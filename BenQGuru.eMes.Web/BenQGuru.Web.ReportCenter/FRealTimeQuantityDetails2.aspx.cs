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
using Infragistics.WebUI.UltraWebGrid;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FRealTimeQuantityDetails ��ժҪ˵����
	/// </summary>
	public partial class FRealTimeQuantityDetails2 : BaseRQPage
	{
		protected BenQGuru.eMES.Web.Helper.RefreshController RefreshController1;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdQuery;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		public BenQGuru.eMES.Web.UserControl.eMESDate eMESDate1;

		private GridHelperForRPT _gridHelper = null;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent2;
		private WebQueryHelper _helper = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelperForRPT( this.gridWebGrid );
			this._helper = new WebQueryHelper(null,null,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			//this.pagerSizeSelector.Readonly = true;
			this.eMESDate1.Enable = "false";
			

			if( !this.IsPostBack )
			{
				this.eMESDate1.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				
				this._initialQueryCondtion();

				this._initialWebGrid();

				this._helper.Query( sender );
			}
		}

		private void _initialQueryCondtion()
		{
			this.txtSegmentQuery.Text = this.GetRequestParam("segmentcode");
			this.eMESDate1.Text = this.GetRequestParam("shiftday");
			this.txtShiftCodeQuery.Text = this.GetRequestParam("shiftcode");
			this.txtStepSeqenceCodeQuery.Text = this.GetRequestParam("stepsequencecode");
			this.txtModelCodeQuery.Text = this.GetRequestParam("modelcode");
			this.txtItemCodeQuery.Text = this.GetRequestParam("itemcode");
			this.txtMoCodeQuery.Text = this.GetRequestParam("mocode");
			this.txtTimePeriodQuery.Text = this.GetRequestParam("tpcode");
			this.txtTimePeriodDetail.Text = this.GetRequestParam("tpcodedetail");
			this.ViewState["IncludeMidOutput"] = bool.Parse(this.GetRequestParam("IncludeMidOutput"));
		}

		private void _initialWebGrid()
		{
			this._gridHelper.GridHelper.AddColumn("MoCode","��������",null);
			this._gridHelper.GridHelper.AddColumn("MoMemo","������ע",null);
			this._gridHelper.GridHelper.AddColumn("ModelCode","��Ʒ�����",null);
			this._gridHelper.GridHelper.AddColumn("ItemCode","��Ʒ����",null);
			this._gridHelper.GridHelper.AddColumn("ItemName","��Ʒ����",null);
			this._gridHelper.GridHelper.AddColumn("InputQuantity","Ͷ������",null);
			this._gridHelper.GridHelper.AddColumn("OutputQuantity","��������",null);
			this._gridHelper.GridHelper.Grid.Bands[0].ColFootersVisible =ShowMarginInfo.Yes ;
			this._gridHelper.GridHelper.Grid.Columns[4].FooterText = "����" ;
			this._gridHelper.GridHelper.Grid.Columns[5].FooterTotal = SummaryInfo.Sum ;
			this._gridHelper.GridHelper.Grid.Columns[6].FooterTotal = SummaryInfo.Sum ;
			this._gridHelper.GridHelper.Grid.Bands[0].FooterStyle.BackColor = this._gridHelper.GridHelper.Grid.Bands[0].HeaderStyle.BackColor ;
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
			this.languageComponent2 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
			// 
			// languageComponent1
			// 
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";
			// 
			// languageComponent2
			// 
			this.languageComponent2.Language = "CHS";
			this.languageComponent2.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent2.RuntimePage = null;
			this.languageComponent2.RuntimeUserControl = null;
			this.languageComponent2.UserControlName = "";

		}
		#endregion

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
			FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
			( e as WebQueryEventArgs ).GridDataSource = 
				facadeFactory.CreateQueryFacade1().QueryRealTimeDetails(
				this.txtSegmentQuery.Text,
				FormatHelper.TODateInt(this.eMESDate1.Text),
				this.txtShiftCodeQuery.Text,
				this.txtStepSeqenceCodeQuery.Text,
				this.txtModelCodeQuery.Text,
				this.txtItemCodeQuery.Text,
				this.txtMoCodeQuery.Text,
				this.txtTimePeriodQuery.Text,
				(bool)this.ViewState["IncludeMidOutput"],
				( e as WebQueryEventArgs ).StartRow,
				( e as WebQueryEventArgs ).EndRow);

			( e as WebQueryEventArgs ).RowCount = 
				facadeFactory.CreateQueryFacade1().QueryRealTimeDetailsCount(
				this.txtSegmentQuery.Text,
				FormatHelper.TODateInt(this.eMESDate1.Text),
				this.txtShiftCodeQuery.Text,
				this.txtStepSeqenceCodeQuery.Text,
				this.txtModelCodeQuery.Text,
				this.txtItemCodeQuery.Text,
				this.txtMoCodeQuery.Text,
				this.txtTimePeriodQuery.Text,
				(bool)this.ViewState["IncludeMidOutput"]);
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				RealTimeDetails obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as RealTimeDetails;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.MoCode,
													  obj.MoMemo,
													  obj.ModelCode,
													  obj.ItemCode,
													  obj.ItemName,
													  obj.InputQuantity,
													  obj.OutputQuantity
												  }
					);
			}
		}
	}
}
