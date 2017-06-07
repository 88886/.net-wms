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
	/// FTSRecordQP ��ժҪ˵����
	/// </summary>
	public partial class FTSRecordQP2 : BaseRQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;

		protected BenQGuru.eMES.Web.UserControl.eMESDate dateStartDateQuery;
		protected BenQGuru.eMES.Web.UserControl.eMESTime dateStartTimeQuery;
		protected BenQGuru.eMES.Web.UserControl.eMESDate dateEndDateQuery;
		protected BenQGuru.eMES.Web.UserControl.eMESTime dateEndTimeQuery;

		protected BenQGuru.eMES.Web.UserControl.eMESDate txtReceiveBeginDate;
		protected BenQGuru.eMES.Web.UserControl.eMESTime txtReceiveBeginTime;
		protected BenQGuru.eMES.Web.UserControl.eMESDate txtReceiveEndDate;
		protected BenQGuru.eMES.Web.UserControl.eMESTime txtReceiveEndTime;

		protected BenQGuru.eMES.Web.UserControl.eMESDate txtTSBeginDate;
		protected BenQGuru.eMES.Web.UserControl.eMESTime txtTSBeginTime;
		protected BenQGuru.eMES.Web.UserControl.eMESDate txtTSEndDate;
		protected BenQGuru.eMES.Web.UserControl.eMESTime txtTSEndTime;

		protected GridHelperForRPT _gridHelper = null;	
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelperForRPT(this.gridWebGrid);

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);				
			this._helper.GridCellClick +=new EventHandler(_helper_GridCellClick);

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				TSStatus tsstatus = new TSStatus();
				tsstatus.Items.Remove(TSStatus.TSStatus_Reflow);		//����������
				new CheckBoxListBuilder(tsstatus,this.chkTSStateList,this.languageComponent1).Build();

				this.dateStartDateQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.dateEndDateQuery.Text = this.dateStartDateQuery.Text;

				this.txtReceiveBeginDate.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.txtReceiveEndDate.Text = this.txtReceiveBeginDate.Text;

				this.txtTSBeginDate.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.txtTSEndDate.Text = this.txtTSBeginDate.Text;

				this.dateStartTimeQuery.Text = FormatHelper.ToTimeString( 0 ) ;
				this.dateEndTimeQuery.Text = FormatHelper.ToTimeString(235959);

				this.txtReceiveBeginTime.Text = FormatHelper.ToTimeString( 0 ) ;
				this.txtReceiveEndTime.Text = FormatHelper.ToTimeString(235959);

				this.txtTSBeginTime.Text = FormatHelper.ToTimeString( 0 ) ;
				this.txtTSEndTime.Text = FormatHelper.ToTimeString(235959);

				this.rdbErrorDate.Attributes["onclick"] = "onRadioCheckChange(this)";
				this.rdbReceivedDate.Attributes["onclick"] = "onRadioCheckChange(this)";
				this.rdbTSDate.Attributes["onclick"] = "onRadioCheckChange(this)";

				this.rdbErrorDate.Checked = true;
			}
			CheckBoxListBuilder.FormatListControlStyle( this.chkTSStateList, 80 );

			FormatHelper.SetSNRangeValue(txtStartSnQuery,txtEndSnQuery);

		}

		private void _initialWebGrid()
		{
			this._gridHelper.GridHelper.AddColumn("Ts_SN",				"���к�",null);
			this._gridHelper.GridHelper.AddColumn("Ts_SNSeq",				"���к�˳��",null);
			this._gridHelper.GridHelper.AddColumn("Ts_Count",				"��N�β���",null);
			this._gridHelper.GridHelper.AddColumn("Ts_TsState",			"ά��״̬",null);
			this._gridHelper.GridHelper.AddLinkColumn("Ts_Details",	"ά����Ϣ",null);
			this._gridHelper.GridHelper.AddLinkColumn("Ts_ChangedItemDetails",	"������Ϣ",null);
			this._gridHelper.GridHelper.AddLinkColumn("Ts_ChangedItemDetailsSMT",	"SMT����",null);
			this._gridHelper.GridHelper.AddColumn("Ts_ModelCode",			"��Ʒ��",null);
			this._gridHelper.GridHelper.AddColumn("Ts_ItemCode",			"��Ʒ",null);
			this._gridHelper.GridHelper.AddColumn("Ts_MoCode",			"����",null);
			this._gridHelper.GridHelper.AddColumn("Ts_NGDate",				"��������",null);
			this._gridHelper.GridHelper.AddColumn("Ts_NGTime",			"����ʱ��",null);
			this._gridHelper.GridHelper.AddColumn("Ts_SourceResource",		"��Դվ",null);	
			this._gridHelper.GridHelper.AddColumn("Ts_FrmUser",		"������",null);	
			this._gridHelper.GridHelper.AddColumn( "Ts_ConfirmDate",			"��������",	null);
			this._gridHelper.GridHelper.AddColumn( "Ts_ConfirmTime",		"����ʱ��",	null);
			this._gridHelper.GridHelper.AddColumn( "Ts_ConfirmResource",		"����վ",	null);	
			this._gridHelper.GridHelper.AddColumn( "Ts_ConfirmUser",		"������",null);
			this._gridHelper.GridHelper.AddColumn( "Ts_SouceResourceDate",			"ά������",	null);
			this._gridHelper.GridHelper.AddColumn( "Ts_SouceResourceTime",		"ά��ʱ��",	null);
			this._gridHelper.GridHelper.AddColumn( "Ts_RepaireResource",		"ά��վ",	null);	
			this._gridHelper.GridHelper.AddColumn("Ts_TSUser",		"ά�޹�",null);	
			this._gridHelper.GridHelper.AddColumn("Ts_DestOpCode",		"ȥ����",null);	
			this._gridHelper.GridHelper.AddColumn("Ts_ScrapCause",		"����ԭ��",null);	

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );
		
			this.gridWebGrid.Columns.FromKey("Ts_SNSeq").Hidden = true;
			this.gridWebGrid.Columns.FromKey("Ts_ScrapCause").Hidden = true;
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
			//manager.Add(new DateRangeCheck(this.lblStartDateQuery,this.dateStartDateQuery.Text,this.lblEndDateQuery,this.dateEndDateQuery.Text,true));
			//manager.Add(new DateRangeCheck(this.lblReceveBegindate,this.txtReceiveBeginDate.Text,this.lblReceveEnddate,this.txtReceiveEndDate.Text,true));
			//manager.Add(new DateRangeCheck(this.lblTSBegindate,this.txtTSBeginDate.Text,this.lblTSEnddate,this.txtTSEndDate.Text,true));
			

			if(this.rdbErrorDate.Checked)
			{
				manager.Add(new DateRangeCheck(this.lblNGStartDateQuery,this.dateStartDateQuery.Text,this.lblNGEndDateQuery,this.dateEndDateQuery.Text,true));
			}
			if(this.rdbReceivedDate.Checked)
			{
				manager.Add(new DateRangeCheck(this.lblReceiveBeginDate,this.txtReceiveBeginDate.Text,this.lblReceiveEndDate,this.txtReceiveEndDate.Text,true));
			}
			if(this.rdbTSDate.Checked)
			{
				manager.Add(new DateRangeCheck(this.lblRepairStartDateQuery,this.txtTSBeginDate.Text,this.lblRepairEndDateQuery,this.txtTSEndDate.Text,true));
			}

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return false;
			}	
			return true;
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			
			if((sender is System.Web.UI.HtmlControls.HtmlInputButton ) && ((System.Web.UI.HtmlControls.HtmlInputControl)(((System.Web.UI.HtmlControls.HtmlInputButton)(sender)))).Name == "cmdQuery")
			{
				//ά�޲�ѯ
				this.QueryEvent(sender,e);
			}
			else
			{
				//������ѯ
				this.ExprotEvent(sender,e);
			}

		}

		#region ��ѯ���ݿ���¼�

		//��ѯ��ť�¼�
		private void QueryEvent(object sender, EventArgs e)
		{
			if( this._checkRequireFields() )
			{	
				int startDate			= DefaultDateTime.DefaultToInt;
				int endDate				= DefaultDateTime.DefaultToInt;
				int receiveBeginDate	= DefaultDateTime.DefaultToInt;
				int receiveEndDate		= DefaultDateTime.DefaultToInt;
				int TSBeginDate			= DefaultDateTime.DefaultToInt;
				int TSEndDate			= DefaultDateTime.DefaultToInt;

				int startTime			= FormatHelper.TOTimeInt(this.dateStartTimeQuery.Text);
				int endTime				= FormatHelper.TOTimeInt(this.dateEndTimeQuery.Text);

				int receiveBeginTime	= FormatHelper.TOTimeInt(this.txtReceiveBeginTime.Text);
				int receiveEndTime		= FormatHelper.TOTimeInt(this.txtReceiveEndTime.Text);

				int TSBeginTime			= FormatHelper.TOTimeInt(this.txtTSBeginTime.Text);
				int TSEndTime			= FormatHelper.TOTimeInt(this.txtTSEndTime.Text);

				if(this.rdbErrorDate.Checked)
				{
					startDate = FormatHelper.TODateInt(this.dateStartDateQuery.Text);
					endDate = FormatHelper.TODateInt(this.dateEndDateQuery.Text);
				}

				if(this.rdbReceivedDate.Checked)
				{
					receiveBeginDate = FormatHelper.TODateInt(this.txtReceiveBeginDate.Text);
					receiveEndDate = FormatHelper.TODateInt(this.txtReceiveEndDate.Text);
				}
				if(this.rdbTSDate.Checked)
				{
					TSBeginDate = FormatHelper.TODateInt(this.txtTSBeginDate.Text);
					TSEndDate = FormatHelper.TODateInt(this.txtTSEndDate.Text);
				}

				FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
				( e as WebQueryEventArgs ).GridDataSource = 
					facadeFactory.CreateQueryTSRecordFacade().QueryTSRecord(
					FormatHelper.CleanString(this.txtConditionModel.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
					FormatHelper.CleanString(this.txtReworkMo.Text).ToUpper(),
					FormatHelper.CleanString(this.txtStartSnQuery.Text).ToUpper(),
					FormatHelper.CleanString(this.txtEndSnQuery.Text).ToUpper(),
					startDate,startTime,
					endDate,endTime,
					receiveBeginDate,receiveBeginTime,
					receiveEndDate,receiveEndTime,
					TSBeginDate,TSBeginTime,
					TSEndDate,TSEndTime,
					CheckBoxListBuilder.GetCheckedList(this.chkTSStateList),
					FormatHelper.CleanString(this.txtConditionResource.Text).ToUpper(),
					FormatHelper.CleanString(this.txtFromResource.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConfirmRes.Text).ToUpper(),
					( e as WebQueryEventArgs ).StartRow,
					( e as WebQueryEventArgs ).EndRow);

				( e as WebQueryEventArgs ).RowCount = 
					facadeFactory.CreateQueryTSRecordFacade().QueryTSRecordCount(
						FormatHelper.CleanString(this.txtConditionModel.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
					FormatHelper.CleanString(this.txtReworkMo.Text).ToUpper(),
					FormatHelper.CleanString(this.txtStartSnQuery.Text).ToUpper(),
					FormatHelper.CleanString(this.txtEndSnQuery.Text).ToUpper(),
					startDate,startTime,
					endDate,endTime,
					receiveBeginDate,receiveBeginTime,
					receiveEndDate,receiveEndTime,
					TSBeginDate,TSBeginTime,
					TSEndDate,TSEndTime,
					CheckBoxListBuilder.GetCheckedList(this.chkTSStateList),
					FormatHelper.CleanString(this.txtConditionResource.Text).ToUpper(),
					FormatHelper.CleanString(this.txtFromResource.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConfirmRes.Text).ToUpper());
			}
		}

		//������ť�¼�
		private void ExprotEvent(object sender, EventArgs e)
		{
			if( this._checkRequireFields() )
			{	
				if(chbRepairDetail.Checked)
				{
					//����ά����ϸ TODO ForSimone
					if( this._checkRequireFields() )
					{	
						int startDate			= DefaultDateTime.DefaultToInt;
						int endDate				= DefaultDateTime.DefaultToInt;
						int receiveBeginDate	= DefaultDateTime.DefaultToInt;
						int receiveEndDate		= DefaultDateTime.DefaultToInt;
						int TSBeginDate			= DefaultDateTime.DefaultToInt;
						int TSEndDate			= DefaultDateTime.DefaultToInt;

						int startTime			= FormatHelper.TOTimeInt(this.dateStartTimeQuery.Text);
						int endTime				= FormatHelper.TOTimeInt(this.dateEndTimeQuery.Text);

						int receiveBeginTime	= FormatHelper.TOTimeInt(this.txtReceiveBeginTime.Text);
						int receiveEndTime		= FormatHelper.TOTimeInt(this.txtReceiveEndTime.Text);

						int TSBeginTime			= FormatHelper.TOTimeInt(this.txtTSBeginTime.Text);
						int TSEndTime			= FormatHelper.TOTimeInt(this.txtTSEndTime.Text);

						
						if(this.rdbErrorDate.Checked)
						{
							startDate = FormatHelper.TODateInt(this.dateStartDateQuery.Text);
							endDate = FormatHelper.TODateInt(this.dateEndDateQuery.Text);
						}

						if(this.rdbReceivedDate.Checked)
						{
							receiveBeginDate = FormatHelper.TODateInt(this.txtReceiveBeginDate.Text);
							receiveEndDate = FormatHelper.TODateInt(this.txtReceiveEndDate.Text);
						}
						if(this.rdbTSDate.Checked)
						{
							TSBeginDate = FormatHelper.TODateInt(this.txtTSBeginDate.Text);
							TSEndDate = FormatHelper.TODateInt(this.txtTSEndDate.Text);
						}

						FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
						( e as WebQueryEventArgs ).GridDataSource = 
							facadeFactory.CreateQueryTSRecordFacade().QueryExportTSRecord(
							FormatHelper.CleanString(this.txtConditionModel.Text).ToUpper(),
							FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
							FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
							FormatHelper.CleanString(this.txtReworkMo.Text).ToUpper(),
							FormatHelper.CleanString(this.txtStartSnQuery.Text).ToUpper(),
							FormatHelper.CleanString(this.txtEndSnQuery.Text).ToUpper(),
							startDate,startTime,
							endDate,endTime,
							receiveBeginDate,receiveBeginTime,
							receiveEndDate,receiveEndTime,
							TSBeginDate,TSBeginTime,
							TSEndDate,TSEndTime,
							CheckBoxListBuilder.GetCheckedList(this.chkTSStateList),
							FormatHelper.CleanString(this.txtConditionResource.Text).ToUpper(),
							FormatHelper.CleanString(this.txtFromResource.Text).ToUpper(),
							FormatHelper.CleanString(this.txtConfirmRes.Text).ToUpper(),
							( e as WebQueryEventArgs ).StartRow,
							( e as WebQueryEventArgs ).EndRow);
						
					}
				}
				else
				{
					this.QueryEvent(sender,e);
				}
			}
		}

		#endregion

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				QDOTSRecord obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as QDOTSRecord;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.SN,
													  obj.RunningCardSequence,
													  obj.NGCount.ToString(),
													  this.languageComponent1.GetString(obj.TsState),
													  "",
													  "",
													  "",
													  obj.ModelCode,
													  obj.ItemCode,													  
													  obj.MoCode,
													  FormatHelper.ToDateString(obj.SourceResourceDate),
													  FormatHelper.ToTimeString(obj.SourceResourceTime),													  
													  obj.SourceResource,
													  obj.FrmUser,
													  FormatHelper.ToDateString(obj.ConfirmDate),
													  FormatHelper.ToTimeString(obj.ConfiemTime),
													  obj.ConfirmResource,
													  obj.ConfirmUser,
													  FormatHelper.ToDateString(obj.RepaireDate),
													  FormatHelper.ToTimeString(obj.RepaireTime),
													  obj.RepaireResource,
													  obj.TSUser,
													  obj.DestOpCode,
													obj.ScrapCause
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				if(chbRepairDetail.Checked)
				{
					ExportQDOTSDetails1 obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as ExportQDOTSDetails1;
					( e as DomainObjectToExportRowEventArgs ).ExportRow = 
						new string[]{
										obj.SN,
										obj.NGCount.ToString(),
										this.languageComponent1.GetString(obj.TsState),
										obj.ModelCode,
										obj.ItemCode,													  
										obj.MoCode,

										FormatHelper.ToDateString(obj.SourceResourceDate),
										FormatHelper.ToTimeString(obj.SourceResourceTime),													  
										obj.SourceResource,
										obj.FrmUser,
										FormatHelper.ToDateString(obj.ConfirmDate),
										FormatHelper.ToTimeString(obj.ConfiemTime),
										obj.ConfirmResource,
										obj.ConfirmUser,

										obj.ErrorCodeGroup,
										obj.ErrorCodeGroupDescription,
										obj.ErrorCode,
										obj.ErrorCodeDescription,
										obj.ErrorCauseCode,
										obj.ErrorCauseDescription,
										obj.ErrorLocation,
										obj.ErrorParts,
										obj.Solution,
										obj.SolutionDescription,
										obj.Duty,
										obj.DutyDescription,

										obj.Memo,
										FormatHelper.ToDateString(obj.RepaireDate),
										FormatHelper.ToTimeString(obj.RepaireTime),
										obj.RepaireResource,
										obj.TSUser,
										obj.DestOpCode
									};
				}
				else
				{
					QDOTSRecord obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as QDOTSRecord;
					( e as DomainObjectToExportRowEventArgs ).ExportRow = 
						new string[]{
										obj.SN,
										obj.NGCount.ToString(),
										this.languageComponent1.GetString(obj.TsState),
										obj.ModelCode,
										obj.ItemCode,													  
										obj.MoCode,
										FormatHelper.ToDateString(obj.SourceResourceDate),
										FormatHelper.ToTimeString(obj.SourceResourceTime),													  
										obj.SourceResource,
										obj.FrmUser,
										FormatHelper.ToDateString(obj.ConfirmDate),
										FormatHelper.ToTimeString(obj.ConfiemTime),
										obj.ConfirmResource,
										obj.ConfirmUser,
										FormatHelper.ToDateString(obj.RepaireDate),
										FormatHelper.ToTimeString(obj.RepaireTime),
										obj.RepaireResource,
										obj.TSUser,
										obj.DestOpCode
									};
				}
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			if(chbRepairDetail.Checked)
			{
				//����ά����ϸ TODO ForSimone
				( e as ExportHeadEventArgs ).Heads = 
					new string[]{
									"Ts_SN",
									"Ts_Count",
									"Ts_TsState",
									"Ts_ModelCode",
									"Ts_ItemCode",
									"Ts_MoCode",

									"Ts_NGDate",
									"Ts_NGTime",
									"Ts_SourceResource",
									"Ts_FrmUser",
									"Ts_ConfirmDate",
									"Ts_ConfirmTime",
									"Ts_ConfirmResource",
									"Ts_ConfirmUser",

									"ErrorCodeGroup",
									"ErrorCodeGroupDescription",
									"ErrorCode",
									"ErrorCodeDescription",
									"ErrorCauseCode",
									"ErrorCauseDescription",
									"ErrorLocation",
									"ErrorParts",
									"Solution",
									"SolutionDescription",
									"Duty",
									"DutyDescription",

									"Memo",
									"Ts_SouceResourceDate",
									"Ts_SouceResourceTime",
									"Ts_RepaireResource",
									"Ts_TSUser",
									"Ts_DestOpCode"
									
								};
			}
			else
			{
				( e as ExportHeadEventArgs ).Heads = 
					new string[]{
									"Ts_SN",
									"Ts_Count",
									"Ts_TsState",
									"Ts_ModelCode",
									"Ts_ItemCode",
									"Ts_MoCode",
									"Ts_NGDate",
									"Ts_NGTime",
									"Ts_SourceResource",
									"Ts_FrmUser",
									"Ts_ConfirmDate",
									"Ts_ConfirmTime",
									"Ts_ConfirmResource",
									"Ts_ConfirmUser",
									"Ts_SouceResourceDate",
									"Ts_SouceResourceTime",
									"Ts_RepaireResource",
									"Ts_TSUser",
									"Ts_DestOpCode"
								};
			}



		}	
	
		private void _helper_GridCellClick(object sender, EventArgs e)
		{			
			if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "Ts_Details".ToUpper() )
			{
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FTSRecordDetailsQP.aspx",
					new string[]{
									"ModelCode",
									"ItemCode",
									"MoCode",
									"RunningCard",
									"RunningCardSeq",
									"TSState",
									"TSResourceCode",
									"TSDate",
									"TSTime",
									"BackUrl",
									"ScrapCause"
								},
					new string[]{
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_ModelCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_ItemCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_MoCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_SN").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_SNSeq").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_TsState").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_RepaireResource").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_NGDate").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_NGTime").Text,
									"FTSRecordQP.aspx",
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_ScrapCause").Text
								})
					);
			}
			else if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "Ts_ChangedItemDetails".ToUpper() )
			{
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FTSChangedItemQP.aspx",
					new string[]{
									"ModelCode",
									"ItemCode",
									"MoCode",
									"RunningCard",
									"TSState",
									"TSResourceCode",
									"TSDate",
									"TSTime",
									"BackUrl"
								},
					new string[]{
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_ModelCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_ItemCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_MoCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_SN").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_TsState").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_RepaireResource").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_NGDate").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_NGTime").Text,
									"FTSRecordQP.aspx"
								})
					);
			}
			else if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "Ts_ChangedItemDetailsSMT".ToUpper() )
			{
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FTSChangedItemSMTQP.aspx",
					new string[]{
									"ModelCode",
									"ItemCode",
									"MoCode",
									"RunningCard",
									"TSState",
									"TSResourceCode",
									"TSDate",
									"TSTime",
									"BackUrl"
								},
					new string[]{
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_ModelCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_ItemCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_MoCode").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_SN").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_TsState").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_RepaireResource").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_NGDate").Text,
									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("Ts_NGTime").Text,
									"FTSRecordQP.aspx"
								})
					);
			}
		}
	}
}
