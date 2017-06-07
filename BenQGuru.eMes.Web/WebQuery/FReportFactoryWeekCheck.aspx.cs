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
using BenQGuru.eMES.Report;
using BenQGuru.eMES.Domain.Report;
using BenQGuru.eMES.WebQuery;
using BenQGuru.eMES.Common.Config;
using BenQGuru.eMES.Material;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FReportFactoryWeekCheck ��ժҪ˵����
	/// </summary>
	public partial class FReportFactoryWeekCheck : BaseQPage
	{
		
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected GridHelper _gridHelper = null;

		protected int week = 0;
		protected string lastWeek = "";
		protected string nowWeek = "";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelper(this.gridWebGrid);

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();
				this.OWCChartSpace1.Display = false;
				this._processOWC(this._loadDataSource());
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

		private void _initialWebGrid()
		{
			this.gridWebGrid.Columns.Clear();

			this._gridHelper.AddColumn( "FactoryID", "����",	null);
			this._gridHelper.AddColumn( "LastTotal", "ǰ���ܲ���",	null);
			this._gridHelper.AddColumn( "LastLRR", "ǰ��LRR",	null);
			this._gridHelper.AddColumn( "NowTotal", "�����ܲ���",	null);
			this._gridHelper.AddColumn( "NowLRR", "����LRR",	null);

			//������
			this._gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		private void _processDataDourceToGrid(object[] source)
		{
			this.gridWebGrid.Rows.Clear();

			if( source != null )
			{				
				foreach(RPTFactoryWeekCheck real in source)
				{
					this.gridWebGrid.Rows.Add( this.GetGridRow(real) );
				}
			}

			this._processGridStyle();
		}
		
		private void _processOWC(object[] dataSource)
		{
			this.OWCChartSpace1.ClearCharts();

			if( dataSource != null )
			{
				string[] categories = new string[ dataSource.Length ];
				object[] ClusteredValues = new object[dataSource.Length];	//��״ͼvalues
				object[] ParetoValues = new object[dataSource.Length];		//����ͼvalues

				for(int i = 0;i<dataSource.Length;i++)
				{
					categories[i] = (dataSource[i] as RPTFactoryWeekCheck).FactoryID.ToString();
					ClusteredValues[i] = (dataSource[i] as RPTFactoryWeekCheck).NowTotal;
					ParetoValues[i] = (dataSource[i] as RPTFactoryWeekCheck).NowLRR/100;
				}

				this.OWCChartSpace1.ChartCombinationType = OWCChartCombinationType.OWCCombinationPareto;	//���ö�ͼ��ϻ�ͼ��ʽΪPareto ����ͼ
				this.OWCChartSpace1.AddChart(this.languageComponent1.GetString("NowTotal"),categories,ClusteredValues,OWCChartType.ColumnClustered);
				this.OWCChartSpace1.AddChart(this.languageComponent1.GetString("NowLRR"),categories,ParetoValues,OWCChartType.LineMarkers);
				this.OWCChartSpace1.Display = true;
			}
		}

		private object[] _loadDataSource()
		{
			if( this.txtWeekNoQuery.Text.Trim() != "" && this.txtWeekNoQuery.Text.Substring(0,2).ToUpper() == "WK")
			{
				week = Convert.ToInt16(this.txtWeekNoQuery.Text.Substring(2));
				lastWeek = "WK" + (week-1).ToString();
				nowWeek = "WK" + week.ToString();
				return new FacadeFactory(base.DataProvider).CreateQueryFacade1().QueryRPTFactoryWeekCheck( lastWeek,nowWeek );
			}
			else
			{
				return null;
			}
		}

		private Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{
								((RPTFactoryWeekCheck)obj).FactoryID.ToString(),
								((RPTFactoryWeekCheck)obj).LastTotal.ToString(),
								(((RPTFactoryWeekCheck)obj).LastLRR/100).ToString("##.##%") == "%" ? "0%" : (((RPTFactoryWeekCheck)obj).LastLRR/100).ToString("##.##%"),
								((RPTFactoryWeekCheck)obj).NowTotal.ToString(),
								(((RPTFactoryWeekCheck)obj).NowLRR/100).ToString("##.##%") == "%" ? "0%" : (((RPTFactoryWeekCheck)obj).NowLRR/100).ToString("##.##%")
							});
		}

		private void _processGridStyle()
		{
			try
			{
				GridItemStyle style = new GridItemStyle(true);
				style.Cursor = Infragistics.WebUI.Shared.Cursors.Hand;
				for(int col=1;col < this.gridWebGrid.Columns.Count-1;col++)
				{			
					for(int row=0;row<this.gridWebGrid.Rows.Count-1;row++)
					{
//						this.gridWebGrid.Rows[row].Cells[col].Style = style;
					}
				}
			}
			catch
			{
			}
		}

		private void _doQuery()
		{
			this._processDataDourceToGrid( this._loadDataSource() );
			this._processOWC(this._loadDataSource());
		}

		protected void cmdQuery_ServerClick(object sender, System.EventArgs e)
		{	
			this._doQuery();
		}
	}
}
