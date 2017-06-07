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
using BenQGuru.eMES.DataCollect;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FTSRecordQP ��ժҪ˵����
	/// </summary>
	public partial class FPackingQP : BaseQPageNew
	{
        //protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        //private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelperNew _helper = null;

        protected global::System.Web.UI.WebControls.TextBox dateStartDateQuery;
        protected global::System.Web.UI.WebControls.TextBox dateEndDateQuery;

		protected GridHelperNew _gridHelper = null;
		private QueryFacade2 _facade = null ;// FacadeFactory.CreateQueryFacade2() ;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
			this._gridHelper = new GridHelperNew(this.gridWebGrid,this.DtSource);

			if (!this.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				this.dateStartDateQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(DateTime.Today));
				this.dateEndDateQuery.Text = this.dateStartDateQuery.Text;
			}

			this._helper = new WebQueryHelperNew( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1,this.DtSource );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);				
		}

		private void _initialWebGrid()
		{
            base.InitWebGrid();
			this._gridHelper.AddColumn("CartonNo",				"��װ���",null);
			this._gridHelper.AddColumn("Collected",				"�Ѱ�װ����",null);
			this._gridHelper.AddColumn("Capacity",				"����",null);
			this._gridHelper.AddColumn("Memo",			"��ע��Ϣ",null);
			this._gridHelper.AddColumn("MaintainUser",	"��ҵ��Ա",null);
			this._gridHelper.AddColumn("MaintainDate",	"��ҵ����",null);
			this._gridHelper.AddLinkColumn("PackingDetail",			"��Ʒ��ϸ",null);

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
			//this.gridWebGrid.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
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

		private string packingSSCode = string.Empty;
		private string packingResourceCode = string.Empty;
		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
            //�����к�ת��ΪSourceCode
            DataCollectFacade dataCollectfacade = new DataCollectFacade(this.DataProvider);
            //�������кŵ��������Ҫ����һ�´���
            string startRCard = FormatHelper.CleanString(this.txtRCardStartQuery.Text.Trim().ToUpper());
            string endRCard = FormatHelper.CleanString(this.txtRCardEndQuery.Text.Trim().ToUpper());
            //ת����SourceCard
            string startSourceCard = dataCollectfacade.GetSourceCard(startRCard, string.Empty);
            string endSourceCard = dataCollectfacade.GetSourceCard(endRCard, string.Empty);
            //end

			if(_facade==null)
			{
				_facade = new FacadeFactory(base.DataProvider).CreateQueryFacade2() ;
			}
            (e as WebQueryEventArgsNew).GridDataSource = this._facade.QueryPackingInfoDetail(
                this.txtCartonNoQuery.Text.Trim().ToUpper(),
                FormatHelper.TODateInt(this.dateStartDateQuery.Text),
                FormatHelper.TODateInt(this.dateEndDateQuery.Text),
                startSourceCard.ToUpper(),
                endSourceCard.ToUpper(),
                this.txtConditionMo.Text.Trim().ToUpper(),
                this.txtConditionItem.Text.Trim().ToUpper(),
                this.txtCartonMemoQuery.Text.Trim().ToUpper(),
                (e as WebQueryEventArgsNew).StartRow,
                (e as WebQueryEventArgsNew).EndRow);

            (e as WebQueryEventArgsNew).RowCount = this._facade.QueryPackingInfoDetailCount(
                this.txtCartonNoQuery.Text.Trim().ToUpper(),
                FormatHelper.TODateInt(this.dateStartDateQuery.Text),
                FormatHelper.TODateInt(this.dateEndDateQuery.Text),
                startSourceCard.ToUpper(),
                endSourceCard.ToUpper(),
                this.txtConditionMo.Text.Trim().ToUpper(),
                this.txtConditionItem.Text.Trim().ToUpper(),
                this.txtCartonMemoQuery.Text.Trim().ToUpper());
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			BenQGuru.eMES.Domain.Package.CARTONINFO obj = ( e as DomainObjectToGridRowEventArgsNew ).DomainObject as BenQGuru.eMES.Domain.Package.CARTONINFO;

            DataRow row = this.DtSource.NewRow();
            row["CartonNo"] = obj.CARTONNO;
            row["Collected"] = obj.COLLECTED;
            row["Capacity"] = obj.CAPACITY;
            row["Memo"] = obj.EATTRIBUTE1;
            row["MaintainUser"] = obj.MUSER;
            row["MaintainDate"] = FormatHelper.ToDateString(obj.MDATE);


            (e as DomainObjectToGridRowEventArgsNew).GridRow = row;
                //new UltraGridRow( new object[]{
                //                                  obj.CARTONNO,
                //                                  obj.COLLECTED,
                //                                  obj.CAPACITY,
                //                                  obj.EATTRIBUTE1,
                //                                  obj.MUSER,
                //                                  FormatHelper.ToDateString(obj.MDATE)
                //                              }
                //);
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			BenQGuru.eMES.Domain.Package.CARTONINFO obj = ( e as DomainObjectToExportRowEventArgsNew ).DomainObject as BenQGuru.eMES.Domain.Package.CARTONINFO;
			( e as DomainObjectToExportRowEventArgsNew ).ExportRow = 
				new string[]{
								obj.CARTONNO,
								obj.COLLECTED.ToString(),
								obj.CAPACITY.ToString(),
								obj.EATTRIBUTE1,
								obj.MUSER,
								FormatHelper.ToDateString(obj.MDATE)
							};
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgsNew ).Heads = 
				new string[]{
								"CartonNo",
								"Collected",
								"Capacity",
								"Memo",
								"MaintainUser",
								"MaintainDate"
							};

		}

        protected override void Grid_ClickCell(GridRecord row, string command)
		{
            if (command == "PackingDetail")
			{
				string url = this.MakeRedirectUrl("FPackingDetailsQP.aspx",
					new string[]{"cartonno", "collected", "memo"},
                    new string[] { row.Items.FindItemByKey("CartonNo").Text, row.Items.FindItemByKey("Collected").Text, row.Items.FindItemByKey("Memo").Text }
					);
				this.Response.Redirect(url);
			}
		}
	}
}
