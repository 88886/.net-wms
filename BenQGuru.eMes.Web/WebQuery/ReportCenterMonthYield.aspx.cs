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
	/// ReportCenterMonthYield ��ժҪ˵����
	/// </summary>
	public partial class ReportCenterMonthYield : BaseRQPage
	{

		protected GridHelperForRPT _gridHelper = null;

		protected int today = FormatHelper.TODateInt(System.DateTime.Now);
		protected string opCode = "";
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelperForRPT(this.gridMonthYield);
			this.gridMonthYield.DisplayLayout.CellClickActionDefault = CellClickAction.NotSet;

			opCode = this.GetRequestParam("OPCode");

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialGrid();
				this._processDataDourceToGrid( this._loadDataSource() );
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
			this.gridMonthYield.Click += new Infragistics.WebUI.UltraWebGrid.ClickEventHandler(this.gridMonthYield_Click);
			// 
			// languageComponent1
			// 
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
		#endregion

		private void _initialGrid()
		{
			this.gridMonthYield.Columns.Clear();

			this._gridHelper.GridHelper.AddColumn("EcgCode","����������",null);
			this._gridHelper.GridHelper.AddColumn("ECode","��������",null);
			this._gridHelper.GridHelper.AddColumn("Ecdesc","��������",null);
			this._gridHelper.GridHelper.AddColumn("Qty","����",null);

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );

			this.gridMonthYield.Columns.FromKey("EcgCode").MergeCells = true;

			string selected = "";
			GridItemStyle blueBack = new GridItemStyle(true);
			blueBack.BackColor = Color.SkyBlue;
			if( this.gridMonthYield.Columns.FromKey( selected ) != null )
			{
                this.gridMonthYield.Columns.FromKey(selected).HeaderStyle.BackColor = Color.SkyBlue;
			}

			this.gridMonthYield.Columns[3].CellStyle.Font.Underline = true;
			this.gridMonthYield.Columns[3].CellStyle.ForeColor = Color.Blue;
		}

		private object[] _loadDataSource()
		{
			return new FacadeFactory(base.DataProvider).CreateQueryFacade1().QueryRPTCenterMonthYield(today,opCode);
		}

		private void _processDataDourceToGrid(object[] source)
		{
			this._initialGrid();

			this.gridMonthYield.Rows.Clear();

			if( source != null )
			{
				foreach(RPTCenterMonthPercent real in source)
				{
					UltraGridRow gridRow = null;
					object[] objs = new object[this.gridMonthYield.Columns.Count];
					gridRow = new UltraGridRow( objs );
					this.gridMonthYield.Rows.Add( gridRow );
					gridRow.Cells.FromKey("ECode").Text = real.ECode;
					gridRow.Cells.FromKey("EcgCode").Text = real.EcgCode;
					gridRow.Cells.FromKey("Ecdesc").Text = real.Ecdesc;
					gridRow.Cells.FromKey("Qty").Text = real.Qty.ToString();
				}
			}

			this._processGridStyle();
		}

		private void _processGridStyle()
		{
			try
			{
				GridItemStyle style = new GridItemStyle(true);
				style.Cursor = Infragistics.WebUI.Shared.Cursors.Hand;
				for(int col=3;col < this.gridMonthYield.Columns.Count;col++)
				{			
					for(int row=0;row<this.gridMonthYield.Rows.Count;row++)
					{
                        this.gridMonthYield.Rows[row].Cells[col].Style.Cursor = Infragistics.WebUI.Shared.Cursors.Hand;				
					}
				}
			}
			catch
			{
			}
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			Response.Redirect("ReportCenter.aspx");
		}

		private void gridMonthYield_Click(object sender, Infragistics.WebUI.UltraWebGrid.ClickEventArgs e)
		{
			if( e.Cell.Column.Key.ToUpper() == "Qty".ToUpper() )
			{
				this.Response.Redirect(
					this.MakeRedirectUrl(
					"FReportCenterMonthProduct.aspx",
					new string[]{"OPCode","ECode"},
					new string[]{opCode,e.Cell.Row.Cells[1].Text})
					);
			}
		}
	}
}
