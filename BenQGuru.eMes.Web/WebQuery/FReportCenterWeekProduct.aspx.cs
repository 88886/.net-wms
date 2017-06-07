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
	/// FReportCenterWeekProduct ��ժҪ˵����
	/// </summary>
	public partial class FReportCenterWeekProduct : BaseRQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
	
		protected GridHelperForRPT _gridHelper = null;

		protected int today = FormatHelper.TODateInt(System.DateTime.Now);
		protected string opCode = "";
		protected string eCode = "";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelperForRPT(this.gridWeekProduct);

			opCode = this.GetRequestParam("OPCode");
			eCode = this.GetRequestParam("ECode");

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
			this.gridWeekProduct.Columns.Clear();

			this._gridHelper.GridHelper.AddColumn("RCard","��Ʒ���к�",null);
			this._gridHelper.GridHelper.AddColumn("ItemCode","��Ʒ",null);
			this._gridHelper.GridHelper.AddColumn("FrmSSCode","����",null);
			this._gridHelper.GridHelper.AddColumn("NGFrmUser","����������Ա",null);
			this._gridHelper.GridHelper.AddColumn("FrmOPCode","�������ֹ���",null);
			this._gridHelper.GridHelper.AddColumn("FrmResCode","����������Դ",null);
			this._gridHelper.GridHelper.AddColumn("NGShiftDay","������������",null);

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );

			string selected = "";
			GridItemStyle blueBack = new GridItemStyle(true);
			blueBack.BackColor = Color.SkyBlue;
			if( this.gridWeekProduct.Columns.FromKey( selected ) != null )
			{
                this.gridWeekProduct.Columns.FromKey(selected).HeaderStyle.BackColor = Color.SkyBlue;
			}
		}

		private object[] _loadDataSource()
		{
			return new FacadeFactory(base.DataProvider).CreateQueryFacade1().QueryRPTCenterWeekProduct(today,opCode,eCode);
		}

		private void _processDataDourceToGrid(object[] source)
		{
			this._initialGrid();

			this.gridWeekProduct.Rows.Clear();

			if( source != null )
			{
				foreach(RPTCenterWeekProduct real in source)
				{
					UltraGridRow gridRow = null;
					object[] objs = new object[this.gridWeekProduct.Columns.Count];
					gridRow = new UltraGridRow( objs );
					this.gridWeekProduct.Rows.Add( gridRow );
					gridRow.Cells.FromKey("RCard").Text = real.RCard;
					gridRow.Cells.FromKey("ItemCode").Text = real.ItemCode;
					gridRow.Cells.FromKey("FrmSSCode").Text = real.FrmSSCode;
					gridRow.Cells.FromKey("NGFrmUser").Text = real.FrmUser;
					gridRow.Cells.FromKey("FrmOPCode").Text = real.FrmOPCode;
					gridRow.Cells.FromKey("FrmResCode").Text = real.FrmResCode;
					gridRow.Cells.FromKey("NGShiftDay").Text = real.ShiftDay.ToString();
				}
			}
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(
				this.MakeRedirectUrl(
				"ReportCenterWeekYield.aspx",
				new string[]{"OPCode"},
				new string[]{opCode})
				);
		}
	}
}

