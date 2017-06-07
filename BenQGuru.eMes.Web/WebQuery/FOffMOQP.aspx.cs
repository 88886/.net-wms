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
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.TS;


namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FOffMOQP ��ժҪ˵����
	/// </summary>
	public partial class FOffMOQP : BaseQPage
	{

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected GridHelper _gridHelper = null;
		protected System.Web.UI.WebControls.Label lblStartSNQuery;
		protected System.Web.UI.WebControls.Label lblEndSNQuery;
		protected System.Web.UI.WebControls.Label lblSoftwareNameQuery;
		protected System.Web.UI.WebControls.TextBox txtSoftwareNameQuery;
		protected System.Web.UI.WebControls.Label lblSoftwareVersionQuery;
		protected System.Web.UI.WebControls.Label lblStepSequenceConditionQuery;
		protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtConditionStepSequence;

		protected System.Web.UI.WebControls.TextBox txtSoftwareVersionQuery;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelper(this.gridWebGrid);

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				txtBeginDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );
				txtEndDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );
			}

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);				
			this._helper.GridCellClick +=new EventHandler(_helper_GridCellClick);

			FormatHelper.SetSNRangeValue(txtStartSnQuery,txtEndSnQuery);
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

		private void _initialWebGrid()
		{
			//��Ʒ���к�
			//����
			//��Ʒ����
			//�ɼ���λ
			//�ɼ��߱�
			//�ɼ���Դ
			//Ա������
			//�ɼ�����
			//�ɼ�ʱ��

			this._gridHelper.AddColumn("RCARD",				"��Ʒ���к�",null);
			this._gridHelper.AddColumn("MOCODE",				"����",null);
			this._gridHelper.AddColumn("ITEMCODE",				"��Ʒ����",null);
			this._gridHelper.AddColumn( "OPCode2",		"�ɼ���λ",	null);	
			this._gridHelper.AddColumn("SSCode2","�ɼ��߱�",null);
			this._gridHelper.AddColumn("Resource2","�ɼ���Դ",null);
			this._gridHelper.AddColumn("MaintainUser",				"Ա������",null);
			this._gridHelper.AddColumn( "MaintainDate2",		"�ɼ�����",	null);
			this._gridHelper.AddColumn( "MaintainTime2",		"�ɼ�ʱ��",	null);

			//������
			this._gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
			PageCheckManager manager = new PageCheckManager();
			manager.Add(new DateRangeCheck(this.lblStartDateQuery,this.txtBeginDate.Text,this.lblEndDateQuery,this.txtEndDate.Text,true));

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return;
			}

			if((sender is System.Web.UI.HtmlControls.HtmlInputButton ) && ((System.Web.UI.HtmlControls.HtmlInputControl)(((System.Web.UI.HtmlControls.HtmlInputButton)(sender)))).Name == "cmdGridExport")
			{
				//TODO ForSimone
				this.ExportQueryEvent(sender,e);
			}
			else
			{
				this.QueryEvent(sender,e);
			}
		}

		#region ��ѯ�¼�

		private void QueryEvent(object sender, EventArgs e)
		{
			int BeginDate = FormatHelper.TODateInt(this.txtBeginDate.Text);
			int EndDate = FormatHelper.TODateInt(this.txtEndDate.Text);

			BenQGuru.eMES.TS.TSFacade tsfacade = new BenQGuru.eMES.TS.TSFacade(base.DataProvider);
			( e as WebQueryEventArgs ).GridDataSource = 
				tsfacade.QuerySimulationReport(
				FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
				FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
				FormatHelper.CleanString(this.txtStartSnQuery.Text),
				FormatHelper.CleanString(this.txtEndSnQuery.Text),
				BeginDate,
				EndDate,
				( e as WebQueryEventArgs ).StartRow,
				( e as WebQueryEventArgs ).EndRow);

			( e as WebQueryEventArgs ).RowCount = 
				tsfacade.QuerySimulationReportCount(
				FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
				FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
				FormatHelper.CleanString(this.txtStartSnQuery.Text),
				FormatHelper.CleanString(this.txtEndSnQuery.Text),
				BeginDate,
				EndDate);
		}

		//�����¼�
		private void ExportQueryEvent(object sender, EventArgs e)
		{
			this.QueryEvent(sender,e);
		}

		#endregion


		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{


			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				SimulationReport obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as SimulationReport;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.RunningCard,
													  obj.MOCode,
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
				SimulationReport obj = (SimulationReport)( (DomainObjectToExportRowEventArgs)e ).DomainObject;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.RunningCard,
									obj.MOCode,
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
								"RunningCard",
								"MOCODE",
								"ITEMCODE",
								"OPCode",
								"SSCode",
								"Resource",
								"MaintainUser",
								"MaintainDate",
								"MaintainTime"
							};
		}	

		private void _helper_GridCellClick(object sender, EventArgs e)
		{
			if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "CHECKITEMLIST".ToUpper() )
			{
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FOQCCardCheckList.aspx",
					new string[]{
									"LotNo",
									"LotSeq",
									"ItemCode",
									"MoCode",
									"RunningCard",
									"RunningCardSeq",
									"BackUrl"
								},
					new string[]{
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("LOTNO").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("LOTNOSEQ").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("ITEMCODE").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("MOCODE").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARD").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARDSEQ").Text,
									"FOQCLotSampleQP.aspx"
								})
					);

			}
			else if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "ERRORCODE".ToUpper() )
			{
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FOQCSampleNGDetailQP.aspx",
					new string[]{
									"LotNo",
									"MoCode",
									"RunningCard",
									"RunningCardSeq",
									"BackUrl"
								},
					new string[]{
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("LOTNO").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("MOCODE").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARD").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARDSEQ").Text,
									"FOQCLotSampleQP.aspx"
								})
					);
			}
		}			
	}
}