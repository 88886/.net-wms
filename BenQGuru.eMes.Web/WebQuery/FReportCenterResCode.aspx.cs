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
	/// FReportCenterResCode ��ժҪ˵����
	/// </summary>
	public partial class FReportCenterResCode : BaseRQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
	
		protected GridHelperForRPT _gridHelper = null;

		protected int today = FormatHelper.TODateInt(System.DateTime.Now);
		protected string segmentCode = "";
		protected string stepSequenceCode = "";
		protected string moCode = "";
		protected string itemCode = "";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelperForRPT(this.gridResCode);
			this.gridResCode.DisplayLayout.CellClickActionDefault = CellClickAction.NotSet;
			segmentCode = this.GetRequestParam("SegmentCode");
			stepSequenceCode = this.GetRequestParam("StepSequenceCode");
			moCode = this.GetRequestParam("MoCode");
			itemCode = this.GetRequestParam("ItemCode");

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
			this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
		#endregion

		private void _initialGrid()
		{
			this.gridResCode.Columns.Clear();

			this._gridHelper.GridHelper.AddColumn("ResCode","��Դ",null);
			this._gridHelper.GridHelper.AddColumn("DayQuantity","���ղ���",null);
			this._gridHelper.GridHelper.AddColumn("AccumulateQTY","�ۼƲ���",null);

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );

			string selected = "";
			GridItemStyle blueBack = new GridItemStyle(true);
			blueBack.BackColor = Color.SkyBlue;
			if( this.gridResCode.Columns.FromKey( selected ) != null )
			{
                this.gridResCode.Columns.FromKey(selected).HeaderStyle.BackColor = Color.SkyBlue;
			}
		}

		private object[] _loadDataSource()
		{
			return new FacadeFactory(base.DataProvider).CreateQueryFacade1().QueryRPTCenterResCode(today,segmentCode,stepSequenceCode,moCode,itemCode);
		}

		private void _processDataDourceToGrid(object[] source)
		{
			this._initialGrid();

			this.gridResCode.Rows.Clear();

			if( source != null )
			{
				foreach(RPTCenterResCode real in source)
				{
					UltraGridRow gridRow = null;
					object[] objs = new object[this.gridResCode.Columns.Count];
					gridRow = new UltraGridRow( objs );
					this.gridResCode.Rows.Add( gridRow );
					gridRow.Cells.FromKey("ResCode").Text = real.ResCode;
					gridRow.Cells.FromKey("DayQuantity").Text = real.DayQuantity.ToString();
					gridRow.Cells.FromKey("AccumulateQTY").Text = real.ActQTY.ToString();
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
				for(int row=0;row<this.gridResCode.Rows.Count;row++)
				{
//					this.gridMocode.Rows[row].Cells[4].Style = style;
				}
			}
			catch
			{
			}
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(
				this.MakeRedirectUrl(
				"FReportCenterMocode.aspx",
				new string[]{"SegmentCode","StepSequenceCode"},
				new string[]{segmentCode,stepSequenceCode})
				);
		}
	}
}
