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

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
using Infragistics.WebUI.UltraWebGrid;

namespace BenQGuru.eMES.Web.WarehouseWeb
{
	/// <summary>
	/// FFactory ��ժҪ˵����
	/// </summary>
	public partial class FQueryStockMP : BaseMPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected BenQGuru.eMES.Web.Helper.ExcelExporter excelExporter;
	
		private ButtonHelper buttonHelper = null;
		private WarehouseFacade _facade ;//= new WarehouseFacade();


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

		
		#region Init
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				InitDropDownList();
			}
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}
		#endregion

		#region WebGrid
		protected override void InitWebGrid()
		{
			this.gridHelper.AddColumn( "FactoryCode", "��������",	null);
			//this.gridHelper.AddColumn( "SegmentCode", "���δ���",	null);
			this.gridHelper.AddColumn( "WarehouseCode", "�ֿ�����",	null);
			this.gridHelper.AddColumn( "WarehouseItemCode", "���ϴ���",	null);
			this.gridHelper.AddColumn( "WarehouseItemName", "��������",	null);
			this.gridHelper.AddColumn( "WarehouseQty", "��ɢ����",	null);
			this.gridHelper.AddLinkColumn( "WarehouseItemQtyDt", "�ⷿ������ϸ",	null);

			this.gridHelper.AddDefaultColumn( false, false );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			WarehouseStock stock = (WarehouseStock)obj;
			Infragistics.WebUI.UltraWebGrid.UltraGridRow row = 
				new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
				new object[]{
								stock.FactoryCode,
								//stock.SegmentCode,
								stock.WarehouseCode,
								stock.ItemCode,
								GetItemName(stock.ItemCode),
								Math.Round(stock.OpenQty, 2),
								""
								});
			stock = null;
			return row;
		}
		private Hashtable htItems = new Hashtable();
		private string GetItemName(string itemCode)
		{
			try
			{
				return htItems[itemCode].ToString();
			}
			catch
			{
				return "";
			}
		}


		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			object[] objs = this._facade.QueryWarehouseStock( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)),FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpWarehouseCodeQuery.Text)),/*this.drpSegmentCodeQuery.SelectedValue,*/this.drpFactoryCodeQuery.SelectedValue,
				inclusive, exclusive );
			if (objs != null && objs.Length > 0)
			{
				//��ȡ��������
				object[] objitem = this._facade.GetAllWarehouseItem();
				if (objitem != null)
				{
					for (int i = 0; i < objitem.Length; i++)
					{
						WarehouseItem item = (WarehouseItem)objitem[i];
						htItems.Add(item.ItemCode, item.ItemName);
						item = null;
					}
				}
				objitem = null;
			}
			return objs;
		}

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			return this._facade.QueryWarehouseStockCount(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)),FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpWarehouseCodeQuery.Text)), /*this.drpSegmentCodeQuery.SelectedValue,*/ this.drpFactoryCodeQuery.SelectedValue);
		}

		#endregion

		#region ���ݳ�ʼ��

		private void InitDropDownList()
		{
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			DropDownListBuilder builder = new DropDownListBuilder(this.drpFactoryCodeQuery);
			builder.HandleGetObjectList = new GetObjectListDelegate(this._facade.GetAllFactory);
			builder.Build("FactoryCode","FactoryCode");
			this.drpFactoryCodeQuery.Items.Insert(0, new ListItem("", ""));

			BenQGuru.eMES.BaseSetting.BaseModelFacade bmFacade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(base.DataProvider);
//			builder = new DropDownListBuilder(this.drpSegmentCodeQuery);
//			builder.HandleGetObjectList = new GetObjectListDelegate(bmFacade.GetAllSegment);
//			builder.Build("SegmentCode","SegmentCode");
//			this.drpSegmentCodeQuery.Items.Insert(0, new ListItem("", ""));

			bmFacade = null;
			builder = null;
		}

		//ѡ����Դ���������Σ���ѯ���вֿ�
		/*
		private void FillWarehouseFrom(object sender, System.EventArgs e)
		{
			this.drpWarehouseCodeQuery.Items.Clear();
			if (this.drpFactoryCodeQuery.SelectedValue == string.Empty)
			{
				this.drpSegmentCodeQuery.SelectedValue = string.Empty;
			}
			else
				this.FillWarehouse(this.drpFactoryCodeQuery, this.drpSegmentCodeQuery, this.drpWarehouseCodeQuery);
		}
		private void FillWarehouse(DropDownList drpFactory, DropDownList drpSeg, DropDownList drp)
		{
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			string factoryCode = drpFactory.SelectedValue;
			string segCode = drpSeg.SelectedValue;
			object[] objs = this._facade.GetWarehouseByFactorySeg(segCode, factoryCode, true);
			drp.Items.Clear();
			drp.Items.Add("");
			if (objs != null)
			{
				for (int i = 0; i < objs.Length; i++)
				{
					Warehouse wh = (Warehouse)objs[i];
					drp.Items.Add(new ListItem(wh.WarehouseCode, wh.WarehouseCode));
					wh = null;
				}
			}
			objs = null;
		}
		*/
		#endregion


		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			WarehouseStock stock = (WarehouseStock)obj;
			string[] strArr = 
				new string[]{	stock.FactoryCode,
								//stock.SegmentCode,
								stock.WarehouseCode,
								stock.ItemCode,
								GetItemName(stock.ItemCode),
								stock.OpenQty.ToString()
								};
			stock = null;
			return strArr;
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	
									"FactoryCode",
									//"SegmentCode",
									"WarehouseCode",
									"WarehouseItemCode",
									"WarehouseItemName",
									"WarehouseQty"
									};
		}
		#endregion

		protected override void Grid_ClickCellButton(object sender, CellEventArgs e)
		{
			if( string.Compare( e.Cell.Column.Key,"WarehouseItemQtyDt",true)==0 )
			{
				string url = this.MakeRedirectUrl("FWHCountDetail.aspx",
					new string[]{"FactoryCode","WarehouseCode","WarehouseItemCode"},
					new string[]{
									e.Cell.Row.Cells.FromKey("FactoryCode").Text,
									e.Cell.Row.Cells.FromKey("WarehouseCode").Text,
									e.Cell.Row.Cells.FromKey("WarehouseItemCode").Text,
								});
				Response.Redirect( url, false );
			}
		}

	
	}
}
