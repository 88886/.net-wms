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
using System.IO;
using System.Text;

using Infragistics.WebUI.UltraWebGrid;

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.WebQuery;
using BenQGuru.eMES.Common.Config;
using BenQGuru.eMES.Material;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FTimeQuantitySumQP ��ժҪ˵����
	/// </summary>
	public partial class FTimeQuantitySumQP2 : BaseRQPage
	{
		protected BenQGuru.eMES.Web.UserControl.eMESDate dateStartDateQuery;
		protected BenQGuru.eMES.Web.UserControl.eMESDate dateEndDateQuery;
		protected BenQGuru.eMES.Web.UserControl.eMESTime dateStartTimeQuery;
		protected BenQGuru.eMES.Web.UserControl.eMESTime dateEndTimeQuery;
		
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Timers.Timer timerRefresh;
		protected BenQGuru.eMES.Web.Helper.RefreshController RefreshController1;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

		protected GridHelperForRPT _gridHelper = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{		
			this._gridHelper = new GridHelperForRPT(this.gridWebGrid);
			this.gridWebGrid.DisplayLayout.CellClickActionDefault = CellClickAction.NotSet;

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.dateStartDateQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.dateEndDateQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.dateStartTimeQuery.Text = FormatHelper.ToTimeString(FormatHelper.TOTimeInt(System.DateTime.Now)) ;
				this.dateEndTimeQuery.Text = FormatHelper.ToTimeString(235959);

				this._initialWebGrid();			
			}	
			
			if( !this.IsPostBack )
			{

				//������ܵ�����ҳ��Ĳ���ֱ��ִ�в�ѯ
				if(this.GetRequestParam("post") != null && this.GetRequestParam("post") != string.Empty)	
				{
					//����
					this.dateStartDateQuery.Text = this.GetRequestParam("shiftday");
					this.dateStartTimeQuery.Text = this.GetRequestParam("shiftday");
					this._doQuery();
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
			this.timerRefresh = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.timerRefresh)).BeginInit();
			// 
			// languageComponent1
			// 
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";
			// 
			// timerRefresh
			// 
			this.timerRefresh.Elapsed += new System.Timers.ElapsedEventHandler(this.timerRefresh_Elapsed);
			((System.ComponentModel.ISupportInitialize)(this.timerRefresh)).EndInit();

		}
		#endregion

		private void _initialWebGrid()
		{
			this.gridWebGrid.Columns.Clear();

			this._gridHelper.GridHelper.AddColumn("StepSequenceCode","����",null);
			this._gridHelper.GridHelper.AddColumn("ResourceCode","��Դ",null);
			this._gridHelper.GridHelper.AddColumn("ActionResult","��������",null);

			string selected = "";
			GridItemStyle blueBack = new GridItemStyle(true);
			blueBack.BackColor = Color.SkyBlue;
			if( this.gridWebGrid.Columns.FromKey( selected ) != null )
			{
                this.gridWebGrid.Columns.FromKey(selected).HeaderStyle.BackColor = Color.SkyBlue;
			}

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );
		}

		private object[] _loadDataSource()
		{
			return new FacadeFactory(base.DataProvider).CreateQueryFacade1().QueryTimeQuantitySum(
				this.V_StepSequenceCode,
				this.V_Rescode,
				FormatHelper.TODateInt(this.V_StartDate),
				FormatHelper.TODateInt(this.V_EndDate),
				FormatHelper.TOTimeInt(this.V_StartTime),
				FormatHelper.TOTimeInt(this.V_EndTime));
		}


		private void _processDataDourceToGrid(object[] source)
		{
			this._initialWebGrid();

			this.gridWebGrid.Rows.Clear();

			if( source != null )
			{
				foreach(TimeQuantitySum real in source)
				{
					UltraGridRow gridRow = null;
					object[] objs = new object[this.gridWebGrid.Columns.Count];
					gridRow = new UltraGridRow( objs );
					this.gridWebGrid.Rows.Add( gridRow );
					gridRow.Cells.FromKey("StepSequenceCode").Text = real.StepSequenceCode;
					gridRow.Cells.FromKey("ResourceCode").Text = real.ResourceCode;
					gridRow.Cells.FromKey("ActionResult").Text = real.ActionResult.ToString();
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
				for(int col=1;col < this.gridWebGrid.Columns.Count-1;col++)
				{			
					for(int row=0;row<this.gridWebGrid.Rows.Count-1;row++)
					{
                        this.gridWebGrid.Rows[row].Cells[col].Style.Cursor = Infragistics.WebUI.Shared.Cursors.Hand;
					}
				}
			}
			catch
			{
			}
		}

		private bool _checkRequireFields()
		{
			PageCheckManager manager = new PageCheckManager();

			manager.Add(new DateRangeCheck(this.lblSDateQuery,this.dateStartDateQuery.Text,this.lblEDateQuery,this.dateEndDateQuery.Text,0,3,false));
			
			if( !manager.Check() || this.dateStartDateQuery.Text == "" || this.dateEndDateQuery.Text == "" )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);

				return false;
			}
			else
			{
				return true;
			}
		}

		private void _doQuery()
		{
			if(this._checkRequireFields())
			{
				this._initialWebGrid();
				this._processDataDourceToGrid( this._loadDataSource() );
			}
		}

		#region ViewState

		private string V_StepSequenceCode
		{
			get
			{	
				return this.txtStepSequence.Text;
			}
			set
			{
				this.ViewState["V_StepSequenceCode"] = value;
			}
		}

		private string V_Rescode
		{
			get
			{	
				return this.txtRescode.Text;
			}
			set
			{
				this.ViewState["V_Rescode"] = value;
			}
		}

		private string V_StartDate
		{
			get
			{	
				return this.dateStartDateQuery.Text;
			}
			set
			{
				this.ViewState["V_StartDate"] = value;
			}
		}

		private string V_StartTime
		{
			get
			{	
				return this.dateStartTimeQuery.Text;
			}
			set
			{
				this.ViewState["V_StartTime"] = value;
			}
		}

		private string V_EndDate
		{
			get
			{	
				return this.dateEndDateQuery.Text;
			}
			set
			{
				this.ViewState["V_EndDate"] = value;
			}
		}

		private string V_EndTime
		{
			get
			{	
				return this.dateEndTimeQuery.Text;
			}
			set
			{
				this.ViewState["V_EndTime"] = value;
			}
		}
		#endregion

		protected void cmdQuery_ServerClick(object sender, System.EventArgs e)
		{	
			this._doQuery();
		}

		private void timerRefresh_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
		}

		private void chkRefreshAuto_CheckedChanged(object sender, System.EventArgs e)
		{
			this._doQuery();
		}

		protected void cmdGridExport2_ServerClick(object sender, EventArgs e)
		{
			this.GridExport(this.gridWebGrid);
		}
	}
}
