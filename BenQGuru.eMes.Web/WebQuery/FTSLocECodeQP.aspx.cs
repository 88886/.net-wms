using System;
using System.Collections;
using System.Collections.Generic;
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
using BenQGuru.eMES.Material;


namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FTSLocECodeQP ��ժҪ˵����
	/// </summary>
	public partial class FTSLocECodeQP : BaseQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		protected WebQueryHelper _helper = null;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox Selectabletextbox1;

		GridHelper _gridHelper = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{            
			this.txtItemCodeQuery.Target = this.MakeRedirectUrl("FItemSP.aspx");
			this.txtMoCodeQuery.Target = this.MakeRedirectUrl("FMOSP.aspx");

			if( !this.IsPostBack )
			{ 		
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.dateStartDateQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.dateEndDateQuery.Text = this.dateStartDateQuery.Text;

				RadioButtonListBuilder builder3 = new RadioButtonListBuilder(
					new VisibleStyle(),this.rblVisibleStyle,this.languageComponent1);			
				builder3.Build();              
                this.columnChart.Visible = false;
                this.gridWebGrid.Visible = false;

                if (this.Request.Params["Width"] != null)
                {
                    ViewState["Width"] = this.Request.Params["Width"];
                }

                if (this.Request.Params["Height"] != null)
                {
                    ViewState["Height"] = this.Request.Params["Height"];
                }
               
			}
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

		protected void cmdQuery_ServerClick(object sender, EventArgs e)
		{
			if (_checkRequireFields() == false)
				return;
			_helper_LoadGridDataSource(sender, e);
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
			if(!_checkRequireFields())return;

			this.ViewState["ItemCode"] = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text));
			this.ViewState["MoCode"] = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMoCodeQuery.Text));

			FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
			object[] dataSource = facadeFactory.CreateQueryTSInfoFacade().QueryTSLocECode(
				this.ViewState["ItemCode"].ToString(),
				this.ViewState["MoCode"].ToString(),
				FormatHelper.TODateInt(this.dateStartDateQuery.Text),
				FormatHelper.TODateInt(this.dateEndDateQuery.Text),
				1,
				int.MaxValue);

			//chart
            List<string> fixedColumnList = new List<string>();
            fixedColumnList.Add("ErrorLocation");       
            
            if (dataSource == null || dataSource.Length <= 0)
            {
                this.gridWebGrid.Visible = false;
                this.columnChart.Visible = false;
                return;
            }
            NewReportDomainObject[] newReportDomainObject =  new NewReportDomainObject[dataSource.Length];
            for (int i = 0; i < dataSource.Length; i++)
            {
                NewReportDomainObject domainObject = new NewReportDomainObject();
                domainObject.ErrorCause = ((QDOTSInfo)dataSource[i]).ErrorCauseDesc;
                domainObject.ErrorLocation = ((QDOTSInfo)dataSource[i]).ErrorLocation;
                domainObject.Quantity = ((QDOTSInfo)dataSource[i]).Quantity;
                newReportDomainObject[i] = domainObject;
            }            

			if( this.rblVisibleStyle.SelectedValue.ToUpper() == VisibleStyle.Pivot.ToUpper() )
			{               
                this.columnChart.Visible = false;
               
                List<ReportGridDim3Property> dim3PropertyList = new List<ReportGridDim3Property>();

                dim3PropertyList.Add(new ReportGridDim3Property("Quantity", "0", "SUM", "SUM", false));                

                ReportGridHelper reportGridHelper = new ReportGridHelper(this.DataProvider, this.languageComponent1, this.gridWebGrid);
                reportGridHelper.DataSource = newReportDomainObject;
                reportGridHelper.Dim1PropertyList = fixedColumnList;               
                reportGridHelper.Dim3PropertyList = dim3PropertyList;
                reportGridHelper.HasDim3PropertyNameRowColumn = true;
                reportGridHelper.ShowGrid();

                this.gridWebGrid.Columns[2].Header.Caption = languageComponent1.GetString("ErrorCauseDesc");
                this.gridWebGrid.Visible = true;
			}
			if( this.rblVisibleStyle.SelectedValue.ToUpper() == VisibleStyle.Chart.ToUpper() )
			{               
                this.gridWebGrid.Visible = false;

                //������ҳ����Ĵ�С
                if (ViewState["Width"] != null)
                {
                    columnChart.Width = int.Parse(ViewState["Width"].ToString());
                }

                if (ViewState["Height"] != null)
                {
                    columnChart.Height = int.Parse(ViewState["Height"].ToString());
                }
                //end

                DataTable dataTableColumn = new DataTable();
                dataTableColumn.Columns.Add("Names", typeof(System.String));
                dataTableColumn.Columns.Add(" ", typeof(System.Int32));
                for (int i = 0; i < newReportDomainObject.Length; i++)
                {
                    dataTableColumn.Rows.Add(new object[] { newReportDomainObject[i].ErrorLocation, newReportDomainObject[i].Quantity });
                }
                this.columnChart.DataType = true;
                this.columnChart.YLabelFormatString = "<DATA_VALUE:0.##>";
                this.columnChart.ChartTextFormatString = "<DATA_VALUE:0.##>";
                this.columnChart.ColumnDataSource = dataTableColumn;
                this.columnChart.DataBindTable();
                this.columnChart.Visible = true;                
            }
		}

		/// <summary>
		/// ������
		/// </summary>
		/// <returns></returns>
		private bool _checkRequireFields()
		{			
			PageCheckManager manager = new PageCheckManager();
			manager.Add( new DateRangeCheck(this.lblStartDateQuery,this.dateStartDateQuery.Text,this.lblEndDateQuery,this.dateEndDateQuery.Text,true));

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return true;
			}	
			return true;
		}

		private void dFactoryCode_Load(object sender, EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				WarehouseFacade whFacade = new WarehouseFacade(base.DataProvider);
				object[]  factorys  = whFacade.GetAllFactory();
				if( factorys != null )
				{
					foreach( BenQGuru.eMES.Domain.Warehouse.Factory _factory in factorys)
					{
						this.dFactoryCode.Items.Add( _factory.FactoryCode ) ;
					}
					new DropDownListBuilder( this.dFactoryCode ).AddAllItem( this.languageComponent1 ) ;
				}
			}
		}       

	}
}


