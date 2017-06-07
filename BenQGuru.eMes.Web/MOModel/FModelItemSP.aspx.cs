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
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
#endregion


namespace BenQGuru.eMES.Web.MOModel
{
	/// <summary>
	/// FModelItemSP ��ժҪ˵����
	/// </summary>
	public partial class FModelItemSP : BasePage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		private GridHelper gridHelper = null;
		private ButtonHelper buttonHelper = null;
		//private ItemFacade _itemFacade = FacadeFactory.CreateItemFacade();
		private ModelFacade _modelFacade;// = new FacadeFactory(base.DataProvider).CreateModelFacade();
	
	

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
				InitParameters();
				this.InitWebGrid();
			}
		}

	

		public string ModelCode
		{
			get
			{
				return (string) this.ViewState["modelcode"];
			}
		}
		protected void cmdAdd_ServerClick(object sender, System.EventArgs e)
		{		
			if(_modelFacade==null){_modelFacade = new FacadeFactory(base.DataProvider).CreateModelFacade();}
			ArrayList array = this.gridHelper.GetCheckedRows();
			if( array.Count > 0 )
			{
				ArrayList items = new ArrayList( array.Count );
			
				foreach (UltraGridRow row in array)
				{
					object item = this.GetEditObject(row);
					if( item != null )
					{
						items.Add( (Model2Item)item );
					}
				}

				this._modelFacade.AssignItemsToModel((Model2Item[])items.ToArray( typeof(Model2Item) ) );

				this.RequestData();
			}
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("FModelItemEP.aspx", new string[] {"modelcode"},new string[] {ModelCode}));
		}

		protected void chbSelectAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( this.chbSelectAll.Checked )
			{
				this.gridHelper.CheckAllRows( CheckStatus.Checked );
			}
			else
			{
				this.gridHelper.CheckAllRows( CheckStatus.Unchecked );
			}
		}

		protected void cmdQuery_ServerClick(object sender, System.EventArgs e)
		{
			this.RequestData();
		}
		#endregion

		#region private method

		private void InitHander()
		{
			this.gridHelper = new GridHelper(this.gridWebGrid);
			this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadDataSource);
			this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridRow);

			this.buttonHelper = new ButtonHelper(this);

			this.pagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);
		}

		private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
		{
			this.gridHelper.GridBind( this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize );
		}

		private void InitParameters()
		{
			if(this.Request.Params["modelcode"] == null)
			{
				ExceptionManager.Raise(this.GetType().BaseType,"$Error_RequestUrlParameter_Lost");
			}
			else
			{
				//sammer kong 20050411
				this.ViewState["modelcode"] = this.Request.Params["modelcode"] ;
			}
		}
		
		private void InitWebGrid()
		{
			this.gridHelper.AddColumn( "ItemCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "ItemName",		 "��Ʒ����",		null);
			this.gridHelper.AddColumn( "ItemType", "��Ʒ���",	null);
			//this.gridHelper.AddColumn( "Model",		 "��Ʒ�����",	null);
			this.gridHelper.AddColumn( "UOM",  "������λ",	null);
//			this.gridHelper.AddColumn( "ItemVersion",		 "��Ʒ�汾",	null);
//			this.gridHelper.AddColumn( "ItemControlType",		 "�ܿط�ʽ",	null);
//			this.gridHelper.AddColumn( "ItemUser",	 "������Ա",	null);
//			this.gridHelper.AddColumn( "ItemDate",    "��������",		null);
			this.gridHelper.AddColumn( "Description",    "��Ʒ����",		null);
			this.gridHelper.AddColumn("MUSER","ά����Ա",null);
			this.gridHelper.AddColumn("MDATE","ά������",null);
			this.gridHelper.AddDefaultColumn( true, false );
			//this.gridHelper.ApplyDefaultStyle();

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		protected Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{"false",
								((Item)obj).ItemCode.ToString(),
								((Item)obj).ItemName.ToString(),
								this.languageComponent1.GetString( ((Item)obj).ItemType.ToString()),
								//((Item)obj).ModelCode.ToString(),
								((Item)obj).ItemUOM.ToString(),
//								((Item)obj).ItemVersion.ToString(),
//								((Item)obj).ItemControlType.ToString(),
//								((Item)obj).ItemUser.ToString(),
//								FormatHelper.ToDateString(((Item)obj).ItemDate),
								((Item)obj).ItemDescription.ToString(),
								((Item)obj).MaintainUser.ToString(),
								FormatHelper.ToDateString(((Item)obj).MaintainDate)
							});
		}

		private object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
		{	
			if(_modelFacade==null){_modelFacade = new FacadeFactory(base.DataProvider).CreateModelFacade();}
			Model2Item model2Item = this._modelFacade.CreateModel2Item();
			model2Item.ModelCode = this.ModelCode;
			model2Item.ItemCode = row.Cells[1].Text;
			model2Item.MaintainUser = this.GetUserCode();
            model2Item.OrganizationID = GlobalVariables.CurrentOrganizations.First().OrganizationID;
			return model2Item;
		}

		private object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_modelFacade==null){_modelFacade = new FacadeFactory(base.DataProvider).CreateModelFacade();}
			return this._modelFacade.GetUnSelectedItems(
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(ModelCode)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)),FormatHelper.CleanString(this.txtItemNameQuery.Text),FormatHelper.CleanString(this.drpItemTypeQuery.SelectedValue),
				inclusive, exclusive );
		}

		private int GetRowCount()
		{
			if(_modelFacade==null){_modelFacade = new FacadeFactory(base.DataProvider).CreateModelFacade();}
			return this._modelFacade.GetUnSelectedItemsCounts(
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(ModelCode)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)),FormatHelper.CleanString(this.txtItemNameQuery.Text),FormatHelper.CleanString(this.drpItemTypeQuery.SelectedValue)
				);
		}

		private void RequestData()
		{
			this.pagerToolBar.PageSize = this.pagerSizeSelector.PageSize;
			this.gridHelper.GridBind(PageGridBunding.Page, this.pagerSizeSelector.PageSize);
			this.pagerToolBar.RowCount = GetRowCount();
			this.pagerToolBar.InitPager();
		}


		#endregion

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
