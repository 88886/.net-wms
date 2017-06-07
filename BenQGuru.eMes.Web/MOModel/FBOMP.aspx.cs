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
	/// MBOM ��ժҪ˵����
	/// </summary>
    public partial class FBOMP : BaseMPageMinus
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;


		//private GridHelper gridHelper = null;
		private ButtonHelper buttonHelper = null;
		private ItemFacade _itemFacade;// = FacadeFactory.CreateItemFacade();
		private SBOMFacade _sbomFacade ;//= FacadeFactory.CreateSBOMFacade();
		protected BenQGuru.eMES.Web.Helper.ExcelExporter excelExporter;
	

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
			this.excelExporter = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
			// 
			// languageComponent1
			// 
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";
			// 
			// excelExporter
			// 
			this.excelExporter.FileExtension = "xls";
			this.excelExporter.LanguageComponent = this.languageComponent1;
			this.excelExporter.Page = this;
			this.excelExporter.RowSplit = "\r\n";

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

		protected void btnQuery_ServerClick(object sender, System.EventArgs e)
		{
			RequestData();
		}

		private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
		{
			this.gridHelper.GridBind( this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize );
		}

	
		protected void cmdAdd_ServerClick(object sender, System.EventArgs e)
		{
			Response.Redirect(this.MakeRedirectUrl("FBOMDownloadMP.aspx"));
		}

		private void cmdDelete_ServerClick(object sender, System.EventArgs e)
		{
			ArrayList array = this.gridHelper.GetCheckedRows();
			if( array.Count > 0 )
			{
				if(_sbomFacade==null){_sbomFacade = new FacadeFactory(base.DataProvider).CreateSBOMFacade();}
				string[] itemCodes = new string[array.Count];

				for(int i=0; i<array.Count;i++)
				{
                    itemCodes[i] = ((GridRecord)array[i]).Items.FindItemByKey("ItemCode").Value.ToString();
				}
			
				_sbomFacade.DeleteSBOM(itemCodes);

				this.RequestData();
				this.buttonHelper.PageActionStatusHandle( PageActionType.Delete );
			}
		}

        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            if (commandName == "SBOM")
            {
                Response.Redirect(this.MakeRedirectUrl("FBOMItemMP.aspx", new string[] { "itemcode" }, new string[] { row.Items.FindItemByKey("ItemCode").Value.ToString() }));
            }
            if (commandName == "OPBOM")
            {
                Response.Redirect(this.MakeRedirectUrl("FOPBOMOperationListMP.aspx", new string[] { "itemcode" }, new string[] { row.Items.FindItemByKey("ItemCode").Value.ToString() }));
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

			this.excelExporter.LoadExportDataHandle = new LoadExportDataDelegate(LoadDataSource);
			this.excelExporter.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatExportRecord);
			this.excelExporter.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetColumnHeaderText);

		}

		private object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_itemFacade==null){_itemFacade = new FacadeFactory(base.DataProvider).CreateItemFacade();}
			//return this._itemFacade.QueryHasSBOMItem(FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text)) ,string.Empty,string.Empty,string.Empty, inclusive,exclusive);
			return this._itemFacade.QueryHasSBOMItem(FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text)) ,this.drpItemTypeQuery.SelectedValue,string.Empty,string.Empty,this.txtItemNameQuery.Text, inclusive,exclusive);
		}

		private int GetRowCount()
		{
			if(_itemFacade==null){_itemFacade = new FacadeFactory(base.DataProvider).CreateItemFacade();}
			//return this._itemFacade.QueryHasSBOMItemCount(FormatHelper.CleanString(this.txtItemCodeQuery.Text),string.Empty,string.Empty,string.Empty);
			return this._itemFacade.QueryHasSBOMItemCount(FormatHelper.CleanString(this.txtItemCodeQuery.Text),this.drpItemTypeQuery.SelectedValue,string.Empty,string.Empty,this.txtItemNameQuery.Text);
		}

		private void RequestData()
		{
			// 2005-04-06
			this.pagerToolBar.PageSize = this.pagerSizeSelector.PageSize;
			this.gridHelper.GridBind(PageGridBunding.Page, this.pagerSizeSelector.PageSize);

			this.pagerToolBar.RowCount = GetRowCount();
			this.pagerToolBar.InitPager();
		}

		protected override void InitWebGrid()
		{
            base.InitWebGrid();
			this.gridHelper.AddColumn( "ItemCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "ItemName",		 "��Ʒ����",		null);
			this.gridHelper.AddColumn( "ItemType", "��Ʒ���",	null);
			this.gridHelper.AddColumn( "ItemDescription",  "��Ʒ����",	null);
			this.gridHelper.AddLinkColumn( "SBOM",		 "��Ʒ��׼BOM",		null);
			this.gridHelper.AddColumn( "SBOMMUSER",  "��׼BOMά����Ա",	null);
			this.gridHelper.AddColumn( "SBOMMDATE",  "��׼BOMά������",	null);
			//this.gridHelper.AddLinkColumn( "ItemRoute",		 "��Ʒ����;��",		null);
			this.gridHelper.AddLinkColumn( "OPBOM",		 "��Ʒ����BOM",		null);
			this.gridHelper.AddColumn( "OPBOMMUSER",  "����BOMά����Ա",	null);
			this.gridHelper.AddColumn( "OPBOMMDATE",  "����BOMά������",	null);

            this.gridHelper.AddColumn("OrganizationID", "��֯���", null);
            this.gridWebGrid.Columns.FromKey("OrganizationID").Hidden = true;

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
            DataRow row=this.DtSource.NewRow();
            row["ItemCode"]=((ITEM2BOM)obj).ItemCode.ToString();
            row["ItemName"]=((ITEM2BOM)obj).ItemName.ToString();
            row["ItemType"]=this.languageComponent1.GetString(((ITEM2BOM)obj).ItemType.ToString());
            row["ItemDescription"]=((ITEM2BOM)obj).ItemDescription.ToString();
            row["SBOM"]="";
            row["SBOMMUSER"]=((ITEM2BOM)obj).GetDisplayText("SBOMMaintainUser");
            row["SBOMMDATE"]=((((ITEM2BOM)obj).SBOMMaintainDate==0) ? string.Empty:FormatHelper.ToDateString(((ITEM2BOM)obj).SBOMMaintainDate));
            row["OPBOM"] = "";
            row["OPBOMMUSER"]=((ITEM2BOM)obj).GetDisplayText("OPBOMMaintainUser");
            row["OPBOMMDATE"]=((((ITEM2BOM)obj).OPBOMMaintainDate==0) ? string.Empty:FormatHelper.ToDateString(((ITEM2BOM)obj).OPBOMMaintainDate));
            row["OrganizationID"]=((ITEM2BOM)obj).OrganizationID.ToString();
            return row;
		}
		#endregion


		private string[] FormatExportRecord( object obj )
		{
			return new string[]{((ITEM2BOM)obj).ItemCode.ToString(),
								   ((ITEM2BOM)obj).ItemName.ToString(),
								   this.languageComponent1.GetString(((ITEM2BOM)obj).ItemType.ToString()),
								   ((ITEM2BOM)obj).ItemDescription.ToString(),
								   ((ITEM2BOM)obj).GetDisplayText("SBOMMaintainUser"),
								   (((ITEM2BOM)obj).SBOMMaintainDate==0) ? string.Empty:FormatHelper.ToDateString(((ITEM2BOM)obj).SBOMMaintainDate),
								   ((ITEM2BOM)obj).GetDisplayText("OPBOMMaintainUser"),
								   (((ITEM2BOM)obj).OPBOMMaintainDate==0) ? string.Empty:FormatHelper.ToDateString(((ITEM2BOM)obj).OPBOMMaintainDate)};
		}

		private string[] GetColumnHeaderText()
		{
			return new string[] {	"ItemCode",
									"ItemName",
									"ItemType",	
									"ItemDescription",
									"SBOMMUSER",
									"SBOMMDATE",
									"OPBOMMUSER",
									"OPBOMMDATE",
                                    "OrganizationID"
								};
		}

		private object[] LoadDataSource()
		{
			return this.LoadDataSource(
				1, int.MaxValue );
		}

	
		protected void cmdGridExport_ServerClick(object sender, System.EventArgs e)
		{
			this.excelExporter.Export();
		}
	}
}
