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
using BenQGuru.eMES.Domain.DeviceInterface;
using BenQGuru.eMES.Domain.DataLink;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FPTQuery ��ժҪ˵����
	/// </summary>
	public partial class FPTQuery  : BaseQPage
	{

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected GridHelper _gridHelper = null;
		protected System.Web.UI.WebControls.Label lblStartSNQuery;
		protected System.Web.UI.WebControls.Label lblEndSNQuery;
		protected System.Web.UI.WebControls.Label lblSoftwareNameQuery;
		protected System.Web.UI.WebControls.TextBox txtSoftwareNameQuery;
		protected System.Web.UI.WebControls.Label lblSoftwareVersionQuery;
		protected System.Web.UI.WebControls.Label lblStepSequenceConditionQuery;
		protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtStepSequence;
		protected System.Web.UI.WebControls.Label lblSS;
		protected System.Web.UI.WebControls.DropDownList drpSSQuery;
		protected System.Web.UI.WebControls.Label lblRes;
		protected System.Web.UI.WebControls.TextBox txtResQuery;
		protected System.Web.UI.WebControls.TextBox txtSoftwareVersionQuery;
		protected BenQGuru.eMES.Web.UserControl.eMESDate txtBeginDate;
		protected BenQGuru.eMES.Web.UserControl.eMESDate txtEndDate;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelper(this.gridWebGrid);
			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				//Karron Qiu ���ò�ѯ����Ĭ��Ϊ���죬�����ݿ��м�¼����֮�����ȡ������ע��
				//txtBeginDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );
				//txtEndDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );
			}

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);				

			FormatHelper.SetSNRangeValue(txtStartSnQuery,txtEndSnQuery);
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
			//��Ʒ���кš���Ʒ���롢�������롢���ߡ���Դ�����Ա�׼ֵ,�������ֵ,������Сֵ�����Խ����������Ա,�������ڣ�����ʱ��
			this._gridHelper.AddColumn( "SN",			"��Ʒ���к�",null);
			this._gridHelper.AddColumn( "ItemCode",		"��Ʒ����",null);
			this._gridHelper.AddColumn( "MOCode",		"��������",null);
			this._gridHelper.AddColumn( "SSCode",		"���ߴ���",null);
			this._gridHelper.AddColumn( "ResCode",		"��Դ����",null);
			
			this._gridHelper.AddColumn( "TestStandardValue",	"ʵ�ʲ���ֵ",null);
			this._gridHelper.AddColumn( "TestMaxValue",	"����׼ֵ",null);
			this._gridHelper.AddColumn( "TestMinValue",	"��С��׼ֵ",null);
			this._gridHelper.AddColumn( "TestResult",	"���Խ��",null);

			this._gridHelper.AddColumn( "TestMan",		"������Ա",null);
			this._gridHelper.AddColumn( "TestDate",		"��������",	null);
			this._gridHelper.AddColumn( "TestTime",		"����ʱ��",	null);

			//������
			this._gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
			PageCheckManager manager = new PageCheckManager();
			// Added by Icyer 2006/08/16
			if (this.txtItemQuery.Text.Trim() == string.Empty)
			{
				throw new Exception("$Error_ItemCode_NotCompare");
			}
			// Added end
		
			this.QueryEvent(sender,e);
		}

		#region ��ѯ�¼�

		private void QueryEvent(object sender, EventArgs e)
		{
			int BeginDate = FormatHelper.TODateInt(this.txtBeginDate.Text);
			int EndDate = FormatHelper.TODateInt(this.txtEndDate.Text);

			FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
			( e as WebQueryEventArgs ).GridDataSource = 
				facadeFactory.CreateQueryFacade3().QueryPT(
				FormatHelper.CleanString(this.txtItemQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtStartSnQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtEndSnQuery.Text).ToUpper(),
				BeginDate,
				EndDate,
				( e as WebQueryEventArgs ).StartRow,
				( e as WebQueryEventArgs ).EndRow);

			( e as WebQueryEventArgs ).RowCount =
				facadeFactory.CreateQueryFacade3().QueryPTCount(
				FormatHelper.CleanString(this.txtItemQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtStartSnQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtEndSnQuery.Text).ToUpper(),
				BeginDate,
				EndDate
				);
		}

		#endregion


		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				//��Ʒ���кš���Ʒ���롢�������롢���ߡ���Դ�����Ա�׼ֵ,�������ֵ,������Сֵ�����Խ����������Ա,�������ڣ�����ʱ��
				PreTestValue obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as PreTestValue;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.RCard,
													  obj.ItemCode,
													  obj.MOCode,
													  obj.SSCode,
													  obj.ResCode,
													  obj.Value.ToString(),
													  obj.MaxValue.ToString(),
													  obj.MinValue.ToString(),
													  obj.TestResult,
													  obj.MaintainUser,
													  FormatHelper.ToDateString(obj.MaintainDate),
													  FormatHelper.ToTimeString(obj.MaintainTime)
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{

			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				PreTestValue obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as PreTestValue;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.RCard,
									obj.ItemCode,
									obj.MOCode,
									obj.SSCode,
									obj.ResCode,
									obj.Value.ToString(),
									obj.MaxValue.ToString(),
									obj.MinValue.ToString(),
									obj.TestResult,
									obj.MaintainUser,
									FormatHelper.ToDateString(obj.MaintainDate),
									FormatHelper.ToTimeString(obj.MaintainTime)
								};
			}

		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			//��Ʒ���кš���Ʒ���롢�������롢���ߡ���Դ�����Ա�׼ֵ,�������ֵ,������Сֵ�����Խ����������Ա,�������ڣ�����ʱ��
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"��Ʒ���к�",
								"��Ʒ����",
								"��������",
								"���ߴ���",
								"��Դ����",
								"ʵ�ʲ���ֵ",
								"����׼ֵ",
								"��С��׼ֵ",
								"���Խ��",
								"������Ա",
								"��������",
								"����ʱ��",
			};
				
			
		}
	}
}
