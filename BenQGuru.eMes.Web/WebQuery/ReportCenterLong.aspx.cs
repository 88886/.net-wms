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
	/// ReportCenterLong ��ժҪ˵����
	/// </summary>
	public partial class ReportCenterLong : BaseRQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		protected GridHelperForRPT _gridHelper = null;

		protected int today = FormatHelper.TODateInt(System.DateTime.Now);
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelperForRPT(this.gridLong);

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
			this.gridLong.Columns.Clear();

			this._gridHelper.GridHelper.AddColumn("Ts_SN","���к�",null);
			this._gridHelper.GridHelper.AddColumn("Ts_ConDate","����ά��վ����",null);
			this._gridHelper.GridHelper.AddColumn("Ts_Days","ά������",null);

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );

			string selected = "";
			GridItemStyle blueBack = new GridItemStyle(true);
			blueBack.BackColor = Color.SkyBlue;
			if( this.gridLong.Columns.FromKey( selected ) != null )
			{
                this.gridLong.Columns.FromKey(selected).HeaderStyle.BackColor = Color.SkyBlue;
			}						
		}

		private object[] _loadDataSource()
		{
			return new FacadeFactory(base.DataProvider).CreateQueryFacade1().QueryRPTCenterLong(today);
		}

		private void _processDataDourceToGrid(object[] source)
		{
			this._initialGrid();

			this.gridLong.Rows.Clear();

			if( source != null )
			{
				foreach(RPTCenterLong real in source)
				{
					UltraGridRow gridRow = null;
					object[] objs = new object[this.gridLong.Columns.Count];
					gridRow = new UltraGridRow( objs );
					this.gridLong.Rows.Add( gridRow );
					gridRow.Cells.FromKey("Ts_SN").Text = real.Ts_SN;
					gridRow.Cells.FromKey("Ts_ConDate").Text = real.Ts_ConfirmDate.ToString("####/##/##");
					gridRow.Cells.FromKey("Ts_Days").Text = real.Ts_Days.ToString();
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
				for(int col=1;col < this.gridLong.Columns.Count-1;col++)
				{			
					for(int row=0;row<this.gridLong.Rows.Count-1;row++)
					{
//						this.gridLong.Rows[row].Cells[col].Style = style;
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
	}
}