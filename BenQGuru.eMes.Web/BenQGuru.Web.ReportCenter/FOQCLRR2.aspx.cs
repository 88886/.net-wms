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
using BenQGuru.eMES.Domain.OQC;
using BenQGuru.eMES.OQC;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FOQCLRR ��ժҪ˵����
	/// </summary>
	public partial class FOQCLRR2: BaseRQPage
	{
		protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtConditionMo;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected GridHelperForRPT _gridHelper = null;
		protected System.Web.UI.WebControls.Label lblStartSNQuery;
		protected System.Web.UI.WebControls.TextBox txtStartSNQuery;
		protected System.Web.UI.WebControls.Label lblEndSNQuery;
		protected System.Web.UI.WebControls.TextBox txtEndSNQuery;
		protected System.Web.UI.WebControls.Label lblSoftwareNameQuery;
		protected System.Web.UI.WebControls.TextBox txtSoftwareNameQuery;
		protected System.Web.UI.WebControls.Label lblSoftwareVersionQuery;
		protected System.Web.UI.WebControls.Label lblStepSequenceConditionQuery;
		protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtConditionStepSequence;
		protected System.Web.UI.WebControls.TextBox txtSoftwareVersionQuery;

		protected BenQGuru.eMES.Web.UserControl.eMESDate txtOQCBeginDate;
		protected BenQGuru.eMES.Web.UserControl.eMESTime txtOQCBeginTime;
		protected BenQGuru.eMES.Web.UserControl.eMESDate txtOQCEndDate;
		protected BenQGuru.eMES.Web.UserControl.eMESTime txtOQCEndTime;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.OWCPivotTable1.LanguageComponent = this.languageComponent1;

			this._gridHelper = new GridHelperForRPT(this.gridWebGrid);

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				txtOQCBeginDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );
				txtOQCEndDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );

				this.txtOQCBeginTime.Text = FormatHelper.ToTimeString( 0 ) ;
				this.txtOQCEndTime.Text = FormatHelper.ToTimeString(235959);
			}

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);				
			this._helper.GridCellClick +=new EventHandler(_helper_GridCellClick);
			
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
			//OQC����lot �� ,B----OQC���鲻��lot ��,LRR
			//this._gridHelper.AddColumn("ITEMCODE",			"��Ʒ����",null);
			if(this.drpDateGroup.SelectedValue == "MDATE")
			{
				this._gridHelper.GridHelper.AddColumn("DATEGROUP",			"��",null);
			}
			else if(this.drpDateGroup.SelectedValue == "WEEK")
			{
				this._gridHelper.GridHelper.AddColumn("DATEGROUP",			"��",null);
			}
			else if(this.drpDateGroup.SelectedValue == "MONTH")
			{
				this._gridHelper.GridHelper.AddColumn("DATEGROUP",			"��",null);
			}
			else
			{
				this._gridHelper.GridHelper.AddColumn("DATEGROUP",			"����",null);
			}
			this._gridHelper.GridHelper.AddColumn("MODELCODE",			"��Ʒ�����",null);
			this._gridHelper.GridHelper.AddColumn("LOTTOTALCOUNT",			"���LOT����",null);
			this._gridHelper.GridHelper.AddColumn("LOTNGCOUNT",		"����LOT����",null);
			this._gridHelper.GridHelper.AddColumn("LOTSIZE",		"�ͼ�����",null);
			this._gridHelper.GridHelper.AddColumn("LOTSAMPLECOUNT",		"��������",null);
			this._gridHelper.GridHelper.AddColumn("LOTSAMPLENGCOUNT",		"��������",null);
			this._gridHelper.GridHelper.AddColumn("LRR",				"LRR",null);
			this._gridHelper.GridHelper.AddColumn("DPPM",				"DPPM",null);

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
			PageCheckManager manager = new PageCheckManager();

			manager.Add( new DateRangeCheck(this.lblOQCBegindate, this.txtOQCBeginDate.Text, this.txtOQCEndDate.Text, false) );

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return;
			}

			this.QueryEvent(sender,e);

		}

		#region ��ѯ�¼�

		private void QueryEvent(object sender, EventArgs e)
		{
			int OQCBeginDate = FormatHelper.TODateInt(this.txtOQCBeginDate.Text);
			int OQCEndDate = FormatHelper.TODateInt(this.txtOQCEndDate.Text);

			int OQCBeginTime = FormatHelper.TOTimeInt(this.txtOQCBeginTime.Text);
			int OQCEndTime = FormatHelper.TOTimeInt(this.txtOQCEndTime.Text);

			BenQGuru.eMES.WebQuery.QueryFacade2 qfacade = new BenQGuru.eMES.WebQuery.QueryFacade2(base.DataProvider);

			object[] dataSource = qfacade.QueryOQCLRR(
				FormatHelper.CleanString(this.txtConditionModel.Text).ToUpper(),
				FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
				FormatHelper.CleanString(this.drpDateGroup.SelectedValue),
				OQCBeginDate,OQCBeginTime,
				OQCEndDate,OQCEndTime,
				this.drpType.SelectedValue.Trim().ToUpper());

			( e as WebQueryEventArgs ).GridDataSource = dataSource;
				

			( e as WebQueryEventArgs ).RowCount = 0;
			if(( e as WebQueryEventArgs ).GridDataSource != null)
			{
				( e as WebQueryEventArgs ).RowCount =  ( e as WebQueryEventArgs ).GridDataSource.Length;
			}

			this._processOWC( dataSource );

			// ����
			decimal iLotTotalCount = 0, iLotRejectCount = 0, iLotSampleCount = 0, iLotSampleNGCount = 0;
			if (dataSource != null)
			{
				for (int i = 0; i < dataSource.Length; i++)
				{
					OQCLRR item = (OQCLRR)dataSource[i];
					iLotTotalCount += item.LotTotalCount;
					iLotRejectCount += item.LotNGCount;
					iLotSampleCount += item.LotSampleCount;
					iLotSampleNGCount += item.LotSampleNGCount;
				}
			}
			this.lblLotTotalCountValue.Text = iLotTotalCount.ToString();
			this.lblLotRejectCountValue.Text = iLotRejectCount.ToString();
			this.lblLotSampleCountValue.Text = iLotSampleCount.ToString();
			this.lblLotSampleNGCountValue.Text = iLotSampleNGCount.ToString();
			if (iLotTotalCount != 0)
				this.lblLRRValue.Text = Math.Round(iLotRejectCount / iLotTotalCount * 100, 2).ToString() + "%";
			else
				this.lblLRRValue.Text = "0%";
			if (iLotSampleCount != 0)
				this.lblDPPMValue.Text = Convert.ToInt32(iLotSampleNGCount / iLotSampleCount * 1000000).ToString();
			else
				this.lblDPPMValue.Text = "0";
		}

		//�����¼�
		private void ExportQueryEvent(object sender, EventArgs e)
		{
			this.QueryEvent(sender,e);
		}

		private void _processOWC(object[] dataSource)
		{
			//this.OWCChartSpace1.ClearCharts();

			this.OWCPivotTable1.ClearFieldSet();
			if( dataSource != null )
			{
				this.OWCPivotTable1.SetDataSource( 
					dataSource, new string[]{"DateGroup","ItemCode","LRR","ModelCode","LotTotalCount","LotNGCount"});

				this.OWCPivotTable1.AddRowFieldSet("DateGroup", false);
				this.OWCPivotTable1.AddColumnFieldSet("ModelCode",false);
				this.OWCPivotTable1.AddTotalField(
					"LRR",
					"LRR",
					PivotTotalFunctionType.Sum);

				this.OWCChartSpace1.DataSource = this.OWCPivotTable1.PivotTableName;
				this.OWCChartSpace1.ChartType = OWCChartType.LineMarkers;
				
				this.OWCPivotTable1.Display =  false ;
				this.OWCChartSpace1.Display = true;

				#region aa
				/*
				Hashtable itemHT = new Hashtable();
				for( int i=0; i<dataSource.Length; i++ )
				{
					string itemCode = (dataSource[i] as OQCLRR).ItemCode.ToString();
					if(!itemHT.ContainsKey(itemCode))
					{
						itemHT.Add( itemCode, 1 );
					}
					else
					{
						int count = int.Parse(itemHT[itemCode].ToString())+1;
						itemHT[itemCode] = count;
					}
				}

				if(itemHT!=null && itemHT.Count>0)
				{
					foreach( DictionaryEntry dic in itemHT)
					{
						int count = int.Parse( dic.Value.ToString() ); 
						string[] categories = new string[ count ];
						object[] values = new object[ count ];

						int k=0;
						
						ArrayList dataArray = new ArrayList( dataSource );
						dataArray.Reverse();

						for( int j=dataArray.Count-1; j>=0; j-- )
						{
							string timeType = "";
							switch( this.drpDateGroup.SelectedValue )
							{
								case "MDATE":
									timeType = FormatHelper.ToDateString((dataArray[j] as OQCLRR).DateGroup);
									break;
								case "WEEK":
									timeType = (dataArray[j] as OQCLRR).DateGroup.ToString()+languageComponent1.GetString(RMATimeKide.Week);
									break;
								case "MONTH":
									timeType = (dataArray[j] as OQCLRR).DateGroup.ToString()+languageComponent1.GetString(RMATimeKide.Month);
									break;
								default:
									timeType = "";
									break;
							}

							categories[k] = timeType;

							if( string.Compare( (dataArray[j] as OQCLRR).ItemCode.ToString(), dic.Key.ToString(), true )==0 )
							{
								values[k] = (dataArray[j] as OQCLRR).LRR;
								dataArray.RemoveAt(j);
								if((k+1)>=count)break;
								k++;
							}
						}
						
						this.OWCChartSpace1.AddChart(dic.Key.ToString(), categories, values, OWCChartType.LineMarkers );						
					}

					this.OWCChartSpace1.ChartType = OWCChartType.LineMarkers;this.OWCChartSpace1.
					this.OWCChartSpace1.ChartLeftMaximum = 1;
					this.OWCChartSpace1.Display = true;

                }
				*/	
				#endregion
			}
		}


		#endregion


		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				OQCLRR obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as OQCLRR;

				string LRR = "";
				if(obj.LRR == 0)
				{
					LRR = "0%";
				}
				else
				{
					LRR = obj.LRR.ToString( "##.##%");
				}

				string DPPM = "";
				if (obj.DPPM == 0)
					DPPM = "0";
				else
					DPPM = obj.DPPM.ToString();

				string dataGroup = "";
				if(this.drpDateGroup.SelectedValue == "MDATE")
				{
					dataGroup = FormatHelper.ToDateString( obj.DateGroup );
				}
				else if(this.drpDateGroup.SelectedValue == "WEEK")
				{
					dataGroup = obj.DateGroup.ToString().Insert(4, "W");
				}
				else if(this.drpDateGroup.SelectedValue == "MONTH")
				{
					dataGroup = obj.DateGroup.ToString().Insert(4, "M");
				}
				else
				{
					dataGroup = FormatHelper.ToDateString( obj.DateGroup );
				}

				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
												     dataGroup,
												     obj.ModelCode,
													 obj.LotTotalCount.ToString(),
													 obj.LotNGCount.ToString(),
													  obj.LotSize.ToString(),
													  obj.LotSampleCount.ToString(),
													  obj.LotSampleNGCount.ToString(),
													 LRR,
													 DPPM
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				OQCLRR obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as OQCLRR;

				string LRR = "";
				if(obj.LRR == 0)
				{
					LRR = "0%";
				}
				else
				{
					LRR = obj.LRR.ToString("##.##%");
				}

				string DPPM = "";
				if (obj.DPPM == 0)
					DPPM = "0";
				else
					DPPM = obj.DPPM.ToString();

				string dataGroup = "";
				if(this.drpDateGroup.SelectedValue == "MDATE")
				{
					dataGroup = FormatHelper.ToDateString( obj.DateGroup );
				}
				else if(this.drpDateGroup.SelectedValue == "WEEK")
				{
					dataGroup = obj.DateGroup.ToString().Insert(4, "W");
				}
				else if(this.drpDateGroup.SelectedValue == "MONTH")
				{
					dataGroup = obj.DateGroup.ToString().Insert(4, "M");
				}
				else
				{
					dataGroup = FormatHelper.ToDateString( obj.DateGroup );
				}

				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									dataGroup,
									obj.ModelCode,
									obj.LotTotalCount.ToString(),
									obj.LotNGCount.ToString(),
									obj.LotSize.ToString(),
									obj.LotSampleCount.ToString(),
									obj.LotSampleNGCount.ToString(),
									LRR,
									DPPM
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			if(this.drpDateGroup.SelectedValue == "MDATE")
			{
				( e as ExportHeadEventArgs ).Heads = 
					new string[]{
									"��",
									"MODELCODE",
									"���LOT����",
									"����LOT����",
									"�ͼ�����",
									"��������",
									"��������",
									"LRR",
									"DPPM"
								};
			}
			else if(this.drpDateGroup.SelectedValue == "WEEK")
			{
				( e as ExportHeadEventArgs ).Heads = 
					new string[]{
									"��",
									"MODELCODE",
									"���LOT����",
									"����LOT����",
									"�ͼ�����",
									"��������",
									"��������",
									"LRR",
									"DPPM"
								};
			}
			else if(this.drpDateGroup.SelectedValue == "MONTH")
			{
				( e as ExportHeadEventArgs ).Heads = 
					new string[]{
									"��",
									"MODELCODE",
									"���LOT����",
									"����LOT����",
									"�ͼ�����",
									"��������",
									"��������",
									"LRR",
									"DPPM"
								};
			}
			else
			{
				( e as ExportHeadEventArgs ).Heads = 
					new string[]{
									"����",
									"MODELCODE",
									"���LOT����",
									"����LOT����",
									"�ͼ�����",
									"��������",
									"��������",
									"LRR",
									"DPPM"
								};
			}
			
		}	

		private void _helper_GridCellClick(object sender, EventArgs e)
		{
			if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "CHECKITEMLIST".ToUpper() )
			{
//				decimal rcardseq = Convert.ToDecimal((e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARDSEQ").Text);
//				this.Response.Redirect( 
//					this.MakeRedirectUrl(
//					"FOQCCardCheckList.aspx",
//					new string[]{
//									"LotNo",
//									"LotSeq",
//									"ItemCode",
//									"MoCode",
//									"RunningCard",
//									"RunningCardSeq",
//									"BackUrl"
//								},
//					new string[]{
//									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("LOTNO").Text,
//									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("LOTNOSEQ").Text,
//									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("ITEMCODE").Text,
//									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("MOCODE").Text,
//									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARD").Text,
//									rcardseq.ToString(),
//									"FOQCLotSampleQP.aspx"
//								})
//					);

			}
			else if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "ERRORCODE".ToUpper() )
			{
//				this.Response.Redirect( 
//					this.MakeRedirectUrl(
//					"FOQCSampleNGDetailQP.aspx",
//					new string[]{
//									"LotNo",
//									"MoCode",
//									"RunningCard",
//									"RunningCardSeq",
//									"BackUrl"
//								},
//					new string[]{
//									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("LOTNO").Text,
//									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("MOCODE").Text,
//									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARD").Text,
//									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARDSEQ").Text,
//									"FOQCLotSampleQP.aspx"
//								})
//					);
			}
		}			
	}
}
