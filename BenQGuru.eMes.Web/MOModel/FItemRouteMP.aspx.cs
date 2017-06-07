#region system;
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
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
#endregion

namespace BenQGuru.eMES.Web.MOModel
{
	/// <summary>
	/// FItemRouteMP ��ժҪ˵����
	/// </summary>
    public partial class FItemRouteMP : BaseMPageMinus
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		//private GridHelper gridHelper = null;
		private ButtonHelper buttonHelper = null;
		private ItemFacade _itemFacade ;//= FacadeFactory.CreateItemFacade();
	
	

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
			//this.gridWebGrid.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
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

		#region page events
		protected void Page_Load(object sender, System.EventArgs e)
		{
			InitHander();
			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				// ��ʼ������UI
				this.InitUI();
				this.InitButton();
				this.InitWebGrid();
			}
		}

		private void btnQuery_ServerClick(object sender, System.EventArgs e)
		{
			RequestData();
		}

		private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
		{
			this.gridHelper.GridBind( this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize );
		}

		
		
        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            if (commandName == "ItemRoute")
            {
                Response.Redirect(this.MakeRedirectUrl("FItemRouteOperationListMP.aspx", new string[] { "ItemCode" }, new string[] { row.Items.FindItemByKey("ItemCode").Text.Trim() }));
                
            }
            else if (commandName == "DefaultItemRoute")
            {
                Response.Redirect(this.MakeRedirectUrl("FDefaultItemRouteSP.aspx", new string[] { "ItemCode" }, new string[] { row.Items.FindItemByKey("ItemCode").Text.Trim() }));
            }
        }

		#endregion

		#region private method
		private void InitHander()
		{
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
            this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadDataSource);
            this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridRow);

			this.buttonHelper = new ButtonHelper(this);

			this.pagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);

		}

		private object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_itemFacade==null){_itemFacade = new FacadeFactory(base.DataProvider).CreateItemFacade();}
			return this._itemFacade.QueryItemIllegibility(
				FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text)),FormatHelper.CleanString(this.txtItemNameQuery.Text),FormatHelper.CleanString(this.drpItemTypeQuery.SelectedValue),string.Empty,string.Empty,
				inclusive, exclusive );
		}

		private int GetRowCount()
		{
			if(_itemFacade==null){_itemFacade = new FacadeFactory(base.DataProvider).CreateItemFacade();}
			return this._itemFacade.QueryItemIllegibilityCount(FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text)),FormatHelper.CleanString(this.txtItemNameQuery.Text),FormatHelper.CleanString(this.drpItemTypeQuery.SelectedValue),string.Empty,string.Empty);
		}

		private void RequestData()
		{
			// 2005-04-06
			this.pagerToolBar.PageSize = this.pagerSizeSelector.PageSize;
			this.gridHelper.GridBind(PageGridBunding.Page, this.pagerSizeSelector.PageSize);

			this.pagerToolBar.RowCount = GetRowCount();
			this.pagerToolBar.InitPager();
		}

		private void InitWebGrid()
		{
			this.gridHelper.AddColumn( "ItemCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "ItemName",		 "��Ʒ����",		null);
			this.gridHelper.AddColumn( "ItemType", "��Ʒ���",	null);
//			this.gridHelper.AddColumn( "UOM",  "������λ",	null);
			this.gridHelper.AddLinkColumn( "ItemRoute",		 "��Ʒ����;��",		null);
			this.gridHelper.AddLinkColumn( "DefaultItemRoute",		 "��ƷĬ������;��",		null);
			this.gridHelper.AddColumn("MUSER","ά����Ա",null);
			this.gridHelper.AddColumn("MDATE","ά������",null);

            this.gridWebGrid.Columns.FromKey("DefaultItemRoute").Hidden = true;

			this.gridHelper.AddDefaultColumn( false, false );

			//this.gridHelper.ApplyDefaultStyle();

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		public void InitButton()
		{	
			this.buttonHelper.AddDeleteConfirm();
			this.buttonHelper.PageActionStatusHandle( PageActionType.Add );
		}

		protected DataRow GetGridRow(object obj)
		{
            DataRow row = this.DtSource.NewRow();
            row["ItemCode"] = ((Item)obj).ItemCode.ToString();
            row["ItemName"] = ((Item)obj).ItemName.ToString();
            row["ItemType"] = this.languageComponent1.GetString(((Item)obj).ItemType.ToString());
            row["ItemRoute"] = "";
            row["DefaultItemRoute"] = "";
            row["MUSER"] = ((Item)obj).GetDisplayText("MaintainUser");
            row["MDATE"] = FormatHelper.ToDateString(((Item)obj).MaintainDate);
            return row;
		}
		#endregion

		protected void cmdQuery_ServerClick(object sender, System.EventArgs e)
		{
			this.RequestData();
		}

		protected void drpItemTypeQuery_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				this.drpItemTypeQuery.Items.Clear();
				DropDownListBuilder _builder = new DropDownListBuilder(this.drpItemTypeQuery);
				_builder.AddAllItem(languageComponent1);
				this.drpItemTypeQuery.Items.Add(new ListItem( this.languageComponent1.GetString(ItemType.ITEMTYPE_FINISHEDPRODUCT),ItemType.ITEMTYPE_FINISHEDPRODUCT));
				this.drpItemTypeQuery.Items.Add(new ListItem( this.languageComponent1.GetString(ItemType.ITEMTYPE_SEMIMANUFACTURE),ItemType.ITEMTYPE_SEMIMANUFACTURE));
			}
		}

		

	}
}
