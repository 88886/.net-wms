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
	/// FQueryMOStockMP ��ժҪ˵����
	/// </summary>
	public partial class FQueryMOStockMP : BaseMPage
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
			this.gridHelper.AddColumn( "MOCode", "������",	null);
			this.gridHelper.AddColumn( "WarehouseItemCode", "���ϴ���",	null);
			this.gridHelper.AddColumn( "WarehouseItemName", "��������",	null);
			this.gridHelper.AddColumn( "ReceiptQty", "������",	null);
			this.gridHelper.AddColumn( "IssueQty", "������",	null);
			this.gridHelper.AddColumn( "ScrapQty", "������",	null);
			this.gridHelper.AddColumn( "ReturnQty", "��Ʒ������",	null);
			this.gridHelper.AddColumn( "ReturnScrapQty", "����Ʒ������",	null);
			this.gridHelper.AddColumn( "RemainQty", "ʣ����",	null);
			this.gridHelper.AddColumn( "NGRateManual", "��Ϊ������",	null);
			this.gridHelper.AddColumn( "NGRateFromItem", "���ϲ�����",	null);
			//this.gridHelper.AddColumn( "MOWasteRate", "���������",	null);
			//this.gridHelper.AddColumn( "MOScrapRate", "����������",	null);
			//this.gridHelper.AddColumn( "WearOffRateTotal", "���������",	null);
			this.gridHelper.AddColumn( "WearOffRateTotal", "���ϲ�����",	null);

			this.gridHelper.AddDefaultColumn( false, false );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			MOStock stock = (MOStock)obj;
			string dNGRateManual, dNGRateFromItem, dWearOffRateTotal;


			string dMOWasteRate = "0%";		//���������
			string dMOScrapRate = "0%";;	//����������

			#region ���������,���������� ����ʾ������

//			decimal wasteRate = 0;
//			decimal scrapRate = 0;
//			if(stock.MOLoadingQty !=0 && (stock.MOLoadingQty + stock.TSLoadingQty) != 0 )
//			if(stock.MOStatus == BenQGuru.eMES.Web.Helper.MOManufactureStatus.MOSTATUS_CLOSE)
//			{
//					wasteRate = (stock.ScrapQty + stock.TSUnCompletedQty) / stock.MOLoadingQty;
//					scrapRate = (stock.ScrapQty + stock.TSUnCompletedQty) / (stock.MOLoadingQty + stock.TSLoadingQty);
//			}
//			else
//			{
//				wasteRate = stock.ScrapQty/ stock.MOLoadingQty;
//				scrapRate = stock.ScrapQty / (stock.MOLoadingQty + stock.TSLoadingQty);
//			}
//			if(wasteRate != 0)
//			{
//				dMOWasteRate = wasteRate.ToString("##.##%");
//			}
//			if(scrapRate != 0)
//			{
//				dMOScrapRate = scrapRate.ToString("##.##%");
//			}

			#endregion

			if (stock.IssueQty != 0)
			{
				dNGRateManual = Math.Round(stock.ScrapQty / stock.IssueQty * 100, 2).ToString() + "%";
				dNGRateFromItem = Math.Round((stock.ReturnScrapQty - stock.ScrapQty) / stock.IssueQty * 100, 2).ToString() + "%";
				dWearOffRateTotal = Math.Round(stock.ReturnScrapQty / stock.IssueQty * 100, 2).ToString() + "%";
			}
			else
			{
				dNGRateManual = dNGRateFromItem = dWearOffRateTotal = "0%";
			}


			Infragistics.WebUI.UltraWebGrid.UltraGridRow row = 
				new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
				new object[]{
								stock.MOCode,
								stock.ItemCode,
								GetItemName(stock.ItemCode),
								Math.Round(stock.ReceiptQty, 2).ToString(),
								Math.Round(stock.IssueQty, 2).ToString(),
								Math.Round(stock.ScrapQty, 2).ToString(),
								Math.Round(stock.ReturnQty, 2).ToString(),
								Math.Round(stock.ReturnScrapQty, 2).ToString(),
								Math.Round(stock.ReceiptQty - stock.IssueQty - stock.ReturnQty - stock.ReturnScrapQty, 2),
								dNGRateManual,
								dNGRateFromItem,
								//dMOWasteRate,
								//dMOScrapRate,
								dWearOffRateTotal
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
			object[] objs = this._facade.GetMOStockInQuery( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)), FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMOCodeQuery.Text)),
				inclusive, exclusive );
			

			#region ��������ʣ�����������������ݼ��� ����ʾ,������

			//this._facade.GetMOByMoStock(objs);						//����״̬����
			//this._facade.GetMOLoadingMaterialCount(objs);				//����������������
			//this._facade.GetTSLoadingMaterialCount(objs);				//��������ά�޲�������
			//this._facade.GetUnCompletedMaterialCount(objs);			//��������δ��������

			#endregion

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
			return this._facade.GetMOStockInQueryCount(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)), FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMOCodeQuery.Text)));
		}

		#endregion


		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			MOStock stock = (MOStock)obj;
			string dNGRateManual, dNGRateFromItem, dWearOffRateTotal;
			if (stock.IssueQty != 0)
			{
				dNGRateManual = Math.Round(stock.ScrapQty / stock.IssueQty * 100, 2).ToString() + "%";
				dNGRateFromItem = Math.Round((stock.ReturnScrapQty - stock.ScrapQty) / stock.IssueQty * 100, 2).ToString() + "%";
				dWearOffRateTotal = Math.Round(stock.ReturnScrapQty / stock.IssueQty * 100, 2).ToString() + "%";
			}
			else
			{
				dNGRateManual = dNGRateFromItem = dWearOffRateTotal = "0%";
			}
			string[] strArr = 
				new string[]{	stock.MOCode,
								stock.ItemCode,
								GetItemName(stock.ItemCode),
								Math.Round(stock.ReceiptQty, 2).ToString(),
								Math.Round(stock.IssueQty, 2).ToString(),
								Math.Round(stock.ScrapQty, 2).ToString(),
								Math.Round(stock.ReturnQty, 2).ToString(),
								Math.Round(stock.ReturnScrapQty, 2).ToString(),
								Math.Round(stock.ReceiptQty - stock.IssueQty - stock.ReturnQty - stock.ReturnScrapQty, 2).ToString(),
								dNGRateManual,
								dNGRateFromItem,
								dWearOffRateTotal
							};
			stock = null;
			return strArr;
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	
									"MOCode",
									"WarehouseItemCode",
									"WarehouseItemName",
									"ReceiptQty",
									"ReceiptScrapQty",
									"IssueQty",
									"ScrapQty",
									"ReturnQty",
									"RemainQty",
									"NGRateManual",
									"NGRateFromItem",
									"WearOffRateTotal"
								};
		}
		#endregion


	
	}
}
