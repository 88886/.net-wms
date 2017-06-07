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
using BenQGuru.eMES.Web.UserControl;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FTSInfoQP ��ժҪ˵����
	/// </summary>
	public partial class FTSInfoQP2 : BaseRQPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputButton Submit2;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		public BenQGuru.eMES.Web.UserControl.eMESDate dateStartDateQuery;
		public BenQGuru.eMES.Web.UserControl.eMESDate dateEndDateQuery;
		public BenQGuru.eMES.Web.UserControl.UCNumericUpDown upDown;
		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
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

				RadioButtonListBuilder builder = new RadioButtonListBuilder(
					new TSInfoSummaryTarget(),this.rblSummaryTargetQuery,this.languageComponent1);
				
				builder.Build();

				this._initialWebGrid();

				this.txtErrorCodeGroup.Enabled = false;
				this.txtErrorCodeGroup.Text = string.Empty;

				this.upDown.Value = 5;

				this.dateStartDateQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.dateEndDateQuery.Text = this.dateStartDateQuery.Text;
			}

			RadioButtonListBuilder.FormatListControlStyle( this.rblSummaryTargetQuery,80 );
		}

		private void _initialWebGrid()
		{
			this.gridWebGrid.Columns.Clear();

			if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorCause)
			{
				this._gridHelper.GridHelper.AddColumn(TSInfoSummaryTarget.ErrorCause,"����ԭ��",null);
				this._gridHelper.GridHelper.AddColumn("ErrorCauseDescription","����ԭ������",null);
			}
			else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorCodeGroup)
			{
				this._gridHelper.GridHelper.AddColumn(TSInfoSummaryTarget.ErrorCodeGroup,"����������",null);
				this._gridHelper.GridHelper.AddColumn("ErrorCodeGroupDescription","��������������",null);
			}
			else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorLocation)
			{
				this._gridHelper.GridHelper.AddColumn(TSInfoSummaryTarget.ErrorLocation,"����λ��",null);
			}
			else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.Duty)
			{
				this._gridHelper.GridHelper.AddColumn(TSInfoSummaryTarget.Duty,"���α�",null);
				this._gridHelper.GridHelper.AddColumn("ErrorDutyDescription","���α�����",null);
			}
			else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorCode)
			{
				this._gridHelper.GridHelper.AddColumn(TSInfoSummaryTarget.ErrorCodeGroup,"����������",null);
				this._gridHelper.GridHelper.AddColumn("ErrorCodeGroupDescription","��������������",null);
				this._gridHelper.GridHelper.AddColumn(TSInfoSummaryTarget.ErrorCode,	"��������",null);
				this._gridHelper.GridHelper.AddColumn("ErrorCodeDescription","������������",null);
			}

			this._gridHelper.GridHelper.AddColumn("TsInfoQuantity",				"����",null);
			this._gridHelper.GridHelper.AddColumn("Percent",				"�ٷֱ�",null);
			this._gridHelper.GridHelper.AddLinkColumn("List",	"��ϸ��Ϣ",null);

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
			PageCheckManager manager = new PageCheckManager();
			manager.Add(new DateRangeCheck(this.lblStartDateQuery,this.dateStartDateQuery.Text,this.lblEndDateQuery,this.dateEndDateQuery.Text,true));

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return false;
			}	
			return true;
		}


		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			this._initialWebGrid();
			if( this._checkRequireFields() )
			{
				FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
				object[] dataSource = 
					facadeFactory.CreateQueryTSInfoFacade().QueryTSInfo(
					FormatHelper.CleanString(this.txtErrorCodeGroup.Text).ToUpper(),
					FormatHelper.CleanString(this.txtErrorCode.Text).ToUpper(),
					FormatHelper.CleanString(this.txtErrorCause.Text).ToUpper(),
					FormatHelper.CleanString(this.txtErrorLocation.Text).ToUpper(),
					FormatHelper.CleanString(this.txtErrorDuty.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionModel.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
					FormatHelper.CleanString(this.txtFromResource.Text).ToUpper(),
					FormatHelper.TODateInt(this.dateStartDateQuery.Text),
					FormatHelper.TODateInt(this.dateEndDateQuery.Text),
					this.rblSummaryTargetQuery.SelectedValue,
					FormatHelper.CleanString(this.txtLotNo.Text).ToUpper(),
					this.upDown.Value,
					( e as WebQueryEventArgs ).StartRow,
					( e as WebQueryEventArgs ).EndRow);

				( e as WebQueryEventArgs ).GridDataSource = dataSource;

				( e as WebQueryEventArgs ).RowCount = 
					facadeFactory.CreateQueryTSInfoFacade().QueryTSInfoCount(
					FormatHelper.CleanString(this.txtErrorCodeGroup.Text).ToUpper(),
					FormatHelper.CleanString(this.txtErrorCode.Text).ToUpper(),
					FormatHelper.CleanString(this.txtErrorCause.Text).ToUpper(),
					FormatHelper.CleanString(this.txtErrorLocation.Text).ToUpper(),
					FormatHelper.CleanString(this.txtErrorDuty.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionModel.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
					FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
					FormatHelper.CleanString(this.txtFromResource.Text).ToUpper(),
					FormatHelper.TODateInt(this.dateStartDateQuery.Text),
					FormatHelper.TODateInt(this.dateEndDateQuery.Text),
					this.rblSummaryTargetQuery.SelectedValue,
					FormatHelper.CleanString(this.txtLotNo.Text).ToUpper(),
					this.upDown.Value);

				this._processOWC( dataSource );
			}
		}


		private void _processOWC(object[] dataSource)
		{
			this.OWCChartSpace1.ClearCharts();

			if( dataSource != null )
			{
				dataSource = this.AddOtherInfo(dataSource);
				string[] categories = new string[ dataSource.Length ];
				object[] ColumnClusteredValues = new object[ dataSource.Length ];	//��״ͼvalues
				object[] ParetoValues = new object[ dataSource.Length ];		//����ͼvalues

				for(int i = 0;i<dataSource.Length;i++)
				{
					if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorCause)
					{
						categories[i] = (dataSource[i] as QDOTSInfo).ErrorCause;
					}
					else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorCodeGroup)
					{
						categories[i] = (dataSource[i] as QDOTSInfo).ErrorCodeGroup;
					}
					else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorLocation)
					{
						categories[i] = (dataSource[i] as QDOTSInfo).ErrorLocation;
					}
					else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.Duty)
					{
						categories[i] = (dataSource[i] as QDOTSInfo).Duty;
					}
					else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorCode)
					{
						categories[i] = (dataSource[i] as QDOTSInfo).ErrorCode;
					}
					
					ColumnClusteredValues[i] = (dataSource[i] as QDOTSInfo).Quantity;
					ParetoValues[i] = this.getParetoValue(dataSource,i);
				}

				this.OWCChartSpace1.ChartCombinationType = OWCChartCombinationType.OWCCombinationPareto;		//���ö�ͼ��ϻ�ͼ��ʽΪPareto ����ͼ
				this.OWCChartSpace1.AddChart("����", categories, ColumnClusteredValues );						//Ĭ�������״ͼ
				this.OWCChartSpace1.AddChart("�ٷֱ�", categories, ParetoValues ,OWCChartType.LineMarkers);		//����ͼ
			
				int majorUnit = GetMahorUnit(((QDOTSInfo)dataSource[0]).AllQuantity);			//������Y����С��Ԫ�̶�,����С��5��ʱ�����С����Ԫ.
				if(majorUnit > 0)
				{
					this.OWCChartSpace1.ChartLeftMajorUnit = majorUnit;
				}
				this.OWCChartSpace1.ChartLeftMaximum = ((QDOTSInfo)dataSource[0]).AllQuantity;								//������Y�����̶�
				this.OWCChartSpace1.Display = true;
			}
		}

		//��ȡ��С��Ԫ�̶�
		private int GetMahorUnit(int quantity)
		{
			if(quantity < 10 ) 
				return 1;

			return 0;
		}

		private int GetMahorUnit2(int quantity)
		{
			if(quantity < 10 ) return 1;
			if(quantity < 100 ) return 10;
			int length = quantity.ToString().Length;

			int ff = Convert.ToInt32(quantity.ToString().Substring(0,1));
			if(ff < 2)
			{
				return 1*Convert.ToInt32(Math.Pow(10,length-2));
			}
			else if(ff >5)
			{
				return 5*Convert.ToInt32(Math.Pow(10,length-2));
			}
			else
			{
				return 2*Convert.ToInt32(Math.Pow(10,length-2));
			}
		}

		//�������ͳ����λ
		private object[] AddOtherInfo(object[] dataSource)
		{
			if(dataSource !=null && dataSource.Length >0)
			{
				QDOTSInfo otherQDOTSInfo = new QDOTSInfo();
				string OtherCode = "����";
				otherQDOTSInfo.ErrorCodeGroup = OtherCode;
				otherQDOTSInfo.ErrorCode = OtherCode;
				otherQDOTSInfo.ErrorCause = OtherCode;
				otherQDOTSInfo.ErrorLocation = OtherCode;
				otherQDOTSInfo.Duty = OtherCode;
				otherQDOTSInfo.AllQuantity = ((QDOTSInfo)dataSource[0]).AllQuantity;
				otherQDOTSInfo.Quantity = otherQDOTSInfo.AllQuantity - (int)this.getMaxNum(dataSource);
				decimal percent =  ((decimal)otherQDOTSInfo.Quantity)/((decimal)otherQDOTSInfo.AllQuantity);
				otherQDOTSInfo.Percent = percent;
				
				object[] newDataSource = new object[dataSource.Length + 1];
				for(int i=0;i<dataSource.Length;i++)
				{
					newDataSource[i] = dataSource[i];
				}
				newDataSource[dataSource.Length] = otherQDOTSInfo;

				return newDataSource;
			}
			return dataSource;
		}

		private decimal getParetoValue(object[] dataSource,int count)
		{
			//����ͼ��value ���ۼ�ֵ,���ݴ�����ۼ���,ͳ���ۼ�ֵ
			decimal returnValue = 0;

			for(int i=0;i<=count;i++)
			{
				returnValue += (dataSource[i] as QDOTSInfo).Percent;
			}
			return returnValue;
		}
		private double getMaxNum(object[] dataSource)
		{
			//ͳ�ƴ�������Դ�����ֵ,�����������Y������ֵ
			double returnValue = 0;

			for(int i=0;i<=dataSource.Length-1;i++)
			{
				returnValue += (dataSource[i] as QDOTSInfo).Quantity;
			}
			return returnValue;
		}


		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				QDOTSInfo obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as QDOTSInfo;

				ArrayList objList = new ArrayList();
				if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorCode.ToUpper() )
				{
					objList.Add( obj.ErrorCodeGroup );
					objList.Add( obj.ErrorCodeGroupDesc );
					objList.Add( obj.ErrorCode );
					objList.Add( obj.ErrorCodeDesc );
				}
				else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorCodeGroup.ToUpper() )
				{
					objList.Add( obj.ErrorCodeGroup );
					objList.Add( obj.ErrorCodeGroupDesc );
				}
				else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorCause.ToUpper() )
				{
					objList.Add( obj.ErrorCause );
					objList.Add( obj.ErrorCauseDesc );
				}
				else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorLocation.ToUpper() )
				{
					objList.Add( obj.ErrorLocation );
				}
				else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.Duty.ToUpper() )
				{
					objList.Add( obj.Duty );
					objList.Add( obj.DutyDesc );
				}

				objList.Add( obj.Quantity );
				objList.Add( System.Decimal.Round(obj.Percent * 100,2)  + "%" );

				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( objList.ToArray() );
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				//modified by jessie lee for CS0096, 2005/10/10
				QDOTSInfo obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as QDOTSInfo;

				ArrayList objList = new ArrayList();
				if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorCode.ToUpper() )
				{
					objList.Add( obj.ErrorCodeGroup );
					objList.Add( obj.ErrorCodeGroupDesc );
					objList.Add( obj.ErrorCode );
					objList.Add( obj.ErrorCodeDesc );
				}
				else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorCodeGroup.ToUpper() )
				{
					objList.Add( obj.ErrorCodeGroup );
					objList.Add( obj.ErrorCodeGroupDesc );
				}
				else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorCause.ToUpper() )
				{
					objList.Add( obj.ErrorCause );
					objList.Add( obj.ErrorCauseDesc );
				}
				else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorLocation.ToUpper() )
				{
					objList.Add( obj.ErrorLocation );
				}
				else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.Duty.ToUpper() )
				{
					objList.Add( obj.Duty );
					objList.Add( obj.DutyDesc );
				}

				objList.Add( obj.Quantity.ToString() );
				objList.Add( obj.Percent.ToString(".##%") );

				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
									(string[])objList.ToArray(typeof(string));
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			ArrayList objList = new ArrayList();
			if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorCode.ToUpper() )
			{
				objList.Add( "ErrorCodeGroup" );
				objList.Add( "ErrorCodeGroupDescription" );
				objList.Add( "ErrorCode" );
				objList.Add( "ErrorCodeDescription" );
			}
			else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorCodeGroup.ToUpper() )
			{
				objList.Add( "ErrorCodeGroup" );
				objList.Add( "ErrorCodeGroupDescription" );
			}
			else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorCause.ToUpper() )
			{
				objList.Add( "ErrorCause" );
				objList.Add( "ErrorCauseDescription" );
			}
			else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.ErrorLocation.ToUpper() )
			{
				objList.Add( "ErrorLocation" );
			}
			else if( this.rblSummaryTargetQuery.SelectedValue.ToUpper() == TSInfoSummaryTarget.Duty.ToUpper() )
			{
				objList.Add( "Duty" );
				objList.Add( "ErrorDutyDescription" );
			}
			objList.Add( "TsInfoQuantity" );
			objList.Add( "Percent" );

			( e as ExportHeadEventArgs ).Heads = (string[])objList.ToArray(typeof(string))
							;
		}	
	
		private void _helper_GridCellClick(object sender, EventArgs e)
		{			
			if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "List".ToUpper() )
			{
				string summaryObject = (e as GridCellClickEventArgs).Cell.Row.Cells.FromKey(this.rblSummaryTargetQuery.SelectedValue).Text;
				string summaryObjectDesc = string.Empty;
				string summaryObject1 = string.Empty;
				string summaryObject1Desc = string.Empty;
				if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorCause)
				{
					summaryObjectDesc = (e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("ErrorCauseDescription").Text;
				}
				else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorCodeGroup)
				{
					summaryObjectDesc = (e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("ErrorCodeGroupDescription").Text;
				}
				else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.Duty)
				{
					summaryObjectDesc = (e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("ErrorDutyDescription").Text;
				}
				else if( this.rblSummaryTargetQuery.SelectedValue == TSInfoSummaryTarget.ErrorCode)
				{
					summaryObjectDesc = (e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("ErrorCodeDescription").Text;
					summaryObject1 = (e as GridCellClickEventArgs).Cell.Row.Cells.FromKey(TSInfoSummaryTarget.ErrorCodeGroup).Text;
					summaryObject1Desc = (e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("ErrorCodeGroupDescription").Text;
				}


				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FTSInfoListQP2.aspx",
					new string[]{
									"12_ModelCode",
									"12_ItemCode",
									"12_MoCode",
									"12_FrmResCodes",
									"12_StartDate",
									"12_EndDate",
									"12_ECG",
									"12_EC",
									"12_ECS",
									"12_LOC",
									"12_DUTY",
									"12_SummaryTarget",
									"12_SummaryObject",
									"12_SummaryObjectDesc",
									"12_SummaryObject1",
									"12_SummaryObject1Desc",
									"12_LotNo",
									"BackUrl"
								},
					new string[]{
									FormatHelper.CleanString(this.txtConditionModel.Text),	
									FormatHelper.CleanString(this.txtConditionItem.Text),	
									FormatHelper.CleanString(this.txtConditionMo.Text),	
									FormatHelper.CleanString(this.txtFromResource.Text),	
									this.dateStartDateQuery.Text,
									this.dateEndDateQuery.Text,
									FormatHelper.CleanString(this.txtErrorCodeGroup.Text),
									FormatHelper.CleanString(this.txtErrorCode.Text),
									FormatHelper.CleanString(this.txtErrorCause.Text),
									FormatHelper.CleanString(this.txtErrorLocation.Text),
									FormatHelper.CleanString(this.txtErrorDuty.Text),
									this.rblSummaryTargetQuery.SelectedValue,
									summaryObject,
									summaryObjectDesc,
									summaryObject1,
									summaryObject1Desc,
									this.txtLotNo.Text.Trim().ToUpper(),
									"FTSInfoQP2.aspx"
								})
					);
			}
		}

		//����ѡ���ͳ�ƶ���Ĳ�ͬ,��ʾ�Ĳ�ѯ����Ҳ����ͬ
		protected void rblSummaryTargetQuery_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.txtErrorCodeGroup.Enabled = true;
			this.txtErrorCode.Enabled = true;
			this.txtErrorCause.Enabled = true;
			this.txtErrorLocation.Enabled = true;
			this.txtErrorDuty.Enabled = true;

			if(this.rblSummaryTargetQuery.SelectedIndex == 0)
			{
				this.txtErrorCodeGroup.Enabled = false;
				this.txtErrorCodeGroup.Text = string.Empty;
			}
			else if(this.rblSummaryTargetQuery.SelectedIndex == 1)
			{
				this.txtErrorCode.Enabled = false;
				this.txtErrorCode.Text = string.Empty;
			}
			else if(this.rblSummaryTargetQuery.SelectedIndex == 2)
			{
				this.txtErrorCause.Enabled = false;
				this.txtErrorCause.Text = string.Empty;
			}
			else if(this.rblSummaryTargetQuery.SelectedIndex == 3)
			{
				this.txtErrorLocation.Enabled = false;
				this.txtErrorLocation.Text = string.Empty;
			}
			else if(this.rblSummaryTargetQuery.SelectedIndex == 4)
			{
				this.txtErrorDuty.Enabled = false;
				this.txtErrorDuty.Text = string.Empty;
			}

		}		
	}
}
