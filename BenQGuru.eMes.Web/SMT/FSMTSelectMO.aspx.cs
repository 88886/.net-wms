#region system
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
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.SMT;
using BenQGuru.eMES.Domain.SMT;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
#endregion

namespace BenQGuru.eMES.Web.SMT
{
	/// <summary>
	/// FSMTSelectMO ��ժҪ˵����
	/// </summary>
	public class FSMTSelectMO  : BasePage
	{
		protected Infragistics.WebUI.UltraWebGrid.UltraWebGrid gridWebGrid;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdAdd;
		protected System.Web.UI.WebControls.Label lblRouteCodeQuery;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdQuery;
		protected BenQGuru.eMES.Web.Helper.PagerSizeSelector pagerSizeSelector;
		protected BenQGuru.eMES.Web.Helper.PagerToolBar pagerToolBar;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdReturn;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidreturnParams;
		private GridHelper gridHelper = null;
		private ButtonHelper buttonHelper = null;
		protected System.Web.UI.WebControls.Label lblItemCodeQuery;
		protected System.Web.UI.WebControls.TextBox txtItemCodeQuery;
		protected System.Web.UI.WebControls.Label lblTitles;

		//private BenQGuru.eMES.SMT.SMTFacade _facade ;//= new SMTFacadeFactory().Create();
		protected System.Web.UI.WebControls.Label lblMoStatusQuery;
		protected System.Web.UI.WebControls.DropDownList drpMoStatusQuery;
		protected System.Web.UI.WebControls.TextBox txtMoCodeQuery;
		private BenQGuru.eMES.MOModel.MOFacade _modelFacade ;//= new SMTFacadeFactory(base.DataProvider).CreateMOFacade();
		
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			
			InitHanders();
			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
				// ��ʼ������UI
				this.InitUI();
				this.InitWebGrid();
				if (Request.QueryString["mocode"]!=null)
				{
					this.txtMoCodeQuery.Text = Request.QueryString["mocode"].ToString();
					this.cmdQuery_ServerClick(null,null);
				}
			}
		}
		
		private void InitHanders()
		{
			this.gridHelper = new GridHelper(this.gridWebGrid);
			this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadDataSource);
			this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridRow);

			this.buttonHelper = new ButtonHelper(this);

			this.pagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);

		}

		protected Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{"false",
								((MO)obj).Factory,
								((MO)obj).MOCode,
								((MO)obj).MOType,
								((MO)obj).ItemCode,
								((MO)obj).MOPlanQty.ToString(),
								((MO)obj).MOInputQty.ToString(),
								((MO)obj).MOActualQty.ToString(),
								((MO)obj).MOScrapQty.ToString(),
								this.languageComponent1.GetString(((MO)obj).MOStatus)
								});
		}


		private void InitWebGrid()
		{
			this.gridHelper.AddColumn( "Factory", "����",	null);
			this.gridHelper.AddColumn( "MOCode", "��������",	null);
			this.gridHelper.AddColumn( "MOType", "��������",	null);
			this.gridHelper.AddColumn( "ItemCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "MOPlanQTY", "�ƻ�����",	null);
			this.gridHelper.AddColumn( "MOInputQty", "��Ͷ������",	null);
			this.gridHelper.AddColumn( "MOActualQty", "���깤����",	null);
			this.gridHelper.AddColumn( "MOScrapQty", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "MOStatus", "����״̬",	null);

			this.gridHelper.AddDefaultColumn( true, false );
			this.gridHelper.AddLinkColumn("MO2RouteEdit","ѡ��;��",null) ;
			this.gridHelper.Grid.Columns.FromKey("MO2RouteEdit").Hidden = true ;

			this.gridHelper.Grid.Columns.FromKey("Factory").Hidden = true ;
			this.gridHelper.Grid.Columns.FromKey("MOInputQty").Hidden = true ;
			this.gridHelper.Grid.Columns.FromKey("MOActualQty").Hidden = true ;
			this.gridHelper.Grid.Columns.FromKey("MOScrapQty").Hidden = true ;

			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
		{
			this.gridHelper.GridBind( this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize );
		}

		private object[] LoadDataSource()
		{
			return this.LoadDataSource( 1, int.MaxValue );
		}

		private object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_modelFacade==null){_modelFacade = new SMTFacadeFactory(base.DataProvider).CreateMOFacade();}
//			return this._modelFacade.QueryMOIllegibility(
//				FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtMoCodeQuery.Text )),
//				FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtItemCodeQuery.Text )),
//				string.Empty,
//				FormatHelper.CleanString( this.drpMoStatusQuery.SelectedValue ),
//				string.Empty,string.Empty,
//				inclusive, exclusive );
			return this._modelFacade.QueryMOIllegibility(
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtMoCodeQuery.Text )),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtItemCodeQuery.Text )),
				string.Empty,
				this.getSelectMOStatus(),
				string.Empty,string.Empty,
				inclusive, exclusive );
		}

		private string[] getSelectMOStatus()
		{
			if(this.drpMoStatusQuery.SelectedValue == string.Empty)
			{
				return GetMOStatusList();
			}
			else
			{
				return new string[]{this.drpMoStatusQuery.SelectedValue};
			}
		}
		private int GetRowCount()
		{
			if(_modelFacade==null){_modelFacade = new SMTFacadeFactory(base.DataProvider).CreateMOFacade();}
//			return this._modelFacade.QueryMOIllegibilityCount(
//				FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtMoCodeQuery.Text )),
//				FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtItemCodeQuery.Text )),
//				string.Empty,
//				FormatHelper.CleanString( this.drpMoStatusQuery.SelectedValue ),
//				string.Empty,string.Empty);

			return this._modelFacade.QueryMOIllegibilityCount(
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtMoCodeQuery.Text )),
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtItemCodeQuery.Text )),
				string.Empty,
				this.getSelectMOStatus(),
				string.Empty,string.Empty);
		}

		private string[] FormatExportRecord( object obj )
		{
			return new string[]{ ((Route)obj).RouteCode.ToString(),
								   ((Route)obj).RouteDescription.ToString(),
								   ((Route)obj).MaintainUser.ToString(),
								   FormatHelper.ToDateString(((Route)obj).MaintainDate)};
		}

		private string[] GetColumnHeaderText()
		{
			return new string[] {	"RouteCode",
									"RouteDescription",
									"MaintainUser","MaintainDate"
								};
		}

		private void RequestData()
		{
			this.pagerToolBar.PageSize = this.pagerSizeSelector.PageSize;
			this.gridHelper.GridBind(PageGridBunding.Page, this.pagerSizeSelector.PageSize);
			this.pagerToolBar.RowCount = GetRowCount();
			this.pagerToolBar.InitPager();
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
			this.drpMoStatusQuery.Load += new System.EventHandler(this.drpMoStatusQuery_Load);
			this.cmdQuery.ServerClick += new System.EventHandler(this.cmdQuery_ServerClick);
			this.cmdAdd.ServerClick += new System.EventHandler(this.cmdAdd_ServerClick);
			this.cmdReturn.ServerClick += new System.EventHandler(this.cmdReturn_ServerClick);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void cmdQuery_ServerClick(object sender, System.EventArgs e)
		{
			this.RequestData();
		}

		private void cmdAdd_ServerClick(object sender, System.EventArgs e)
		{
			if(this.gridWebGrid.DisplayLayout.ActiveRow!=null)
			{
				string moCode = this.gridWebGrid.DisplayLayout.ActiveRow.Cells.FromKey("MOCode").Text;
				//ѡ�񹤵�,ֻ�ܵ�ѡ
				this.hidreturnParams.Value =moCode;
				this.ExecuteClientFunction("ReturnValue","");
			}
			else{this.Alert("��ѡ�񹤵�");}
			
		}


		private object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
		{	
			//��ȡGrid�е�һ������
			return null;
		}

		private void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			//���� , Ӧ�õ��ÿͻ��˽ű�����
			this.ExecuteClientFunction("Close","");
		}
		private void drpMoStatusQuery_Load(object sender, System.EventArgs e)
		{
			if(!this.IsPostBack)
			{
				DropDownListBuilder _builder = new DropDownListBuilder(this.drpMoStatusQuery);
				this.drpMoStatusQuery.Items.Add(new ListItem("",""));
				string[] moStatuses = this.GetMOStatusList();
				foreach(string item in moStatuses)
				{
					this.drpMoStatusQuery.Items.Add(new ListItem(this.languageComponent1.GetString(item), item));
				}
				
			}
		}

		
		//��ȡ������ɸѡ������release��open
		private string[] GetMOStatusList()
		{
			return new string[]{MOManufactureStatus.MOSTATUS_INITIAL, MOManufactureStatus.MOSTATUS_RELEASE, MOManufactureStatus.MOSTATUS_OPEN, MOManufactureStatus.MOSTATUS_PENDING};

		}

		
		#region ˽�з���

		/// <summary>
		/// ִ�пͻ��˵ĺ���
		/// </summary>
		/// <param name="FunctionName">������</param>
		/// <param name="FunctionParam">����</param>
		/// <param name="Page">��ǰҳ�������</param>
		public  void ExecuteClientFunction(string FunctionName,string FunctionParam)
		{
			try
			{
				string _msg = string.Empty;
				if(FunctionParam != string.Empty)
					_msg = string.Format("<script language='JavaScript'>  {0}('{1}');</script>",FunctionName,FunctionParam);
				else
					_msg = string.Format("<script language='JavaScript'>  {0}();</script>",FunctionName);

				//��Keyֵ��Ϊ�����,��ֹ�ű��ظ�
				Page.RegisterStartupScript(Guid.NewGuid().ToString(),_msg);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private void Alert(string msg)
		{
			msg = msg.Replace("'","");
			msg = msg.Replace("\r","");
			msg = msg.Replace("\n","");
			string _msg = string.Format("<script language='JavaScript'>  alert('{0}');</script>",msg);
			Page.RegisterStartupScript("",_msg);
		}

		#endregion



	}
}
