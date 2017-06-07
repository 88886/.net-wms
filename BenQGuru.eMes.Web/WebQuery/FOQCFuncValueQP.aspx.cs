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

using BenQGuru.eMES.Web.Helper ;
using BenQGuru.eMES.Web.UserControl ;
using BenQGuru.eMES.WebQuery ;
using BenQGuru.eMES.Common ;
using BenQGuru.eMES.Domain.OQC;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FOQCFuncValueQP ��ժҪ˵����
	/// </summary>
	public partial class FOQCFuncValueQP : BaseMPage
	{
	
		private System.ComponentModel.IContainer components;
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private QueryOQCFunctionFacade _facade = null ;
		
		protected GridHelper gridSNHelper ;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!this.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				InitWebGridSN() ;

				string rcard = this.GetRequestParam("RCARD") ;
				this.txtSN.Value = rcard;
				this.txtSeq.Value = this.GetRequestParam("RCardSeq");
				this.txtLotNo.Value = this.GetRequestParam("LotNo");
				this.txtLotSeq.Value = this.GetRequestParam("LotSeq");
			}

			this.gridSNHelper = new GridHelper(this.gridSN) ;
			this.gridSNHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadDataSourceSN);
			this.gridSNHelper.GetRowCountHandle = new GetRowCountDelegate(this.GetRowCountSN);
			this.gridSNHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridRowSN);	
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);

			this.gridWebGrid.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
			this.gridSN.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
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


		#region WebGrid
		protected override void InitWebGrid()
		{
			this.gridHelper.AddColumn("RCard","��Ʒ���к�",null) ;
			this.gridHelper.AddColumn( "RCardSeq", "RCardSeq",	null);
			this.gridHelper.AddColumn( "SSCode", "����",	null);
			this.gridHelper.AddColumn( "ResCode", "��Դ",	null);
			this.gridHelper.AddColumn( "DutyRado", "Duty Rado",	null);
			this.gridHelper.AddColumn( "BurstMOFreq", "Burst MOƵ��",	null);
			this.gridHelper.AddColumn( "TestResult", "���Խ��",	null);
			this.gridHelper.AddColumn( "TestDate", "��������",	null);
			this.gridHelper.AddColumn( "TestTime", "����ʱ��",	null);
			this.gridHelper.AddColumn( "TestUser", "������Ա",	null);
			this.gridHelper.AddLinkColumn( "TestData", "��������",	null);

			this.gridHelper.Grid.Columns.FromKey("RCardSeq").Hidden = true ;

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
 
			base.InitWebGrid();

			this.gridHelper.RequestData();

		}
		
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			OQCFuncTestValue testValue = obj as OQCFuncTestValue;

			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{
								testValue.RunningCard,
								testValue.RunningCardSequence,
								testValue.StepSequenceCode,
								testValue.ResourceCode,
								testValue.MinDutyRatoMin.ToString("0.00")+"/"+testValue.MinDutyRatoMax.ToString("0.00")+"/"+testValue.MinDutyRatoValue.ToString("0.00"),
								testValue.BurstMdFreMin.ToString("0.00")+"/"+testValue.BurstMdFreMax.ToString("0.00")+"/"+testValue.BurstMdFreValue.ToString("0.00"),
								this.languageComponent1.GetString( testValue.ProductStatus),
								FormatHelper.ToDateString(testValue.MaintainDate),
								FormatHelper.ToTimeString(testValue.MaintainTime),
								testValue.MaintainUser,
								"",
							}
                
                
				);
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			int seq ;
			try
			{
				seq = int.Parse(this.txtSeq.Value) ;
			}
			catch
			{
				seq = -1 ;
			}

			if(_facade==null)
			{
				_facade = new QueryOQCFunctionFacade(this.DataProvider);
			}
			return this._facade.QueryOQCFuncTestValue(
				this.txtSN.Value ,
				seq,this.txtLotNo.Value,this.txtLotSeq.Value,
				inclusive, exclusive );
		}


		protected override int GetRowCount()
		{
			int seq ;
			try
			{
				seq = int.Parse(this.txtSeq.Value) ;
			}
			catch
			{
				seq = -1 ;
			}

			if(_facade==null)
			{
				_facade = new QueryOQCFunctionFacade(this.DataProvider);
			}
			return this._facade.QueryOQCFuncTestValueCount(
				this.txtSN.Value,
				seq,this.txtLotNo.Value,this.txtLotSeq.Value
				);
		}

		#endregion

		#region SNGrid
		protected void InitWebGridSN()
		{
			this.gridSNHelper.AddColumn("RCard","��Ʒ���к�",null) ;
			this.gridSNHelper.AddColumn( "RCardSeq", "RCardSeq",	null);
			this.gridSNHelper.AddColumn( "SSCode", "����",	null);
			this.gridSNHelper.AddColumn( "ResCode", "��Դ",	null);
			this.gridSNHelper.AddColumn( "TestResult", "���Խ��",	null);
			this.gridSNHelper.AddColumn( "TestDate", "��������",	null);
			this.gridSNHelper.AddColumn( "TestTime", "����ʱ��",	null);
			this.gridSNHelper.AddColumn( "TestUser", "������Ա",	null);
			this.gridSNHelper.AddLinkColumn( "TestData", "��������",	null);

			this.gridSNHelper.Grid.Columns.FromKey("RCardSeq").Hidden = true ;

			this.gridSNHelper.ApplyLanguage( this.languageComponent1 );
 
			this.gridSNHelper.RequestData();

		}
		
		protected Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRowSN(object obj)
		{
			OQCDimention oqc = obj as OQCDimention;

			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{
								oqc.RunningCard,
								oqc.RunningCardSequence,
								oqc.StepSequenceCode,
								oqc.ResourceCode,
								oqc.TestResult,
								FormatHelper.ToDateString(oqc.TestDate),
								FormatHelper.ToTimeString(oqc.TestTime),
								oqc.TestUser,
								"",
							}
                
                
				);	
		}

		protected object[] LoadDataSourceSN(int inclusive, int exclusive)
		{
			int seq ;
			try
			{
				seq = int.Parse(this.txtSeq.Value) ;
			}
			catch
			{
				seq = -1 ;
			}


			if(_facade==null)
			{
				_facade = new QueryOQCFunctionFacade(this.DataProvider);
			}

			return _facade.QueryOQCDimention(this.txtSN.Value,
				seq,this.txtLotNo.Value,this.txtLotSeq.Value,
				inclusive, exclusive);
		}

		protected int GetRowCountSN()
		{
			int seq ;
			try
			{
				seq = int.Parse(this.txtSeq.Value) ;
			}
			catch
			{
				seq = -1 ;
			}

			if(_facade==null)
			{
				_facade = new QueryOQCFunctionFacade(this.DataProvider);
			}
			return this._facade.QueryOQCDimentionCount(
				this.txtSN.Value,
				seq,this.txtLotNo.Value,this.txtLotSeq.Value
				);
		}

		#endregion

		#region Export 	
		protected override string[] FormatExportRecord( object obj )
		{
			OQCFuncTestValue testValue = obj as OQCFuncTestValue;

			return  new string[]{
									testValue.RunningCard,
									//testValue.RunningCardSequence.ToString(),
									testValue.StepSequenceCode,
									testValue.ResourceCode,
									testValue.MinDutyRatoMin.ToString("0.00")+"/"+testValue.MinDutyRatoMax.ToString("0.00")+"/"+testValue.MinDutyRatoValue.ToString("0.00"),
									testValue.BurstMdFreMin.ToString("0.00")+"/"+testValue.BurstMdFreMax.ToString("0.00")+"/"+testValue.BurstMdFreValue.ToString("0.00"),
									this.languageComponent1.GetString( testValue.ProductStatus),
									FormatHelper.ToDateString(testValue.MaintainDate),
									FormatHelper.ToTimeString(testValue.MaintainTime),
									testValue.MaintainUser,
									"",
			}
			;
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {
									"RCard",
									"SSCode",
									"ResCode",
									"DutyRado",
									"BurstMOFreq",
									"TestResult",
									"TestDate",
									"TestTime",
									"TestUser",
								};
		}
		#endregion

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect("FOQCLotSampleQP.aspx",true);
		}

		private void gridWebGrid_ClickCellButton(object sender, Infragistics.WebUI.UltraWebGrid.CellEventArgs e)
		{
			if( e.Cell.Column.Key.ToUpper() == "TESTDATA" )
			{
				string url = this.MakeRedirectUrl(
					"FOQCFuncValueDataQP.aspx",
					new string[]{
									"LotNo",
									"LotSeq",
									"RCard",
									"RCardSeq",
									"BackUrl"
								},
					new string[]{
									this.txtLotNo.Value,
									this.txtLotSeq.Value,
									e.Cell.Row.Cells.FromKey("RCard").Text,
									e.Cell.Row.Cells.FromKey("RCardSeq").Text,
									"FOQCFuncValueQP.aspx"
								});

				this.Response.Redirect(url,true);

			}
		}

	}
}
