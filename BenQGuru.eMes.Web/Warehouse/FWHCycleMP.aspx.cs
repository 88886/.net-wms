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
	public partial class FWHCycleMP : BaseMPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
	
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdDelete;
		//protected System.Web.UI.HtmlControls.HtmlInputHidden txtSegmentCode;
		private WarehouseFacede2 _facade ;//= new WarehouseFacede2();
		private bool isWHCycle = false;
		private bool isExportCycleNow = false;	//�Ƿ����ڵ���


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
			this.txtActualQtyEdit.ReadOnly = true;
			this.cmdSave.Disabled = true;
			if (!this.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				InitDropDownList();
				this.cmdExportBook.Disabled = true;
				this.cmdCloseAccount.Disabled = true;
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
			this.gridWebGrid.Columns.Clear();
			this.gridHelper.AddColumn( "FactoryCode", "��������",	null);
			//this.gridHelper.AddColumn( "SegmentCode", "���δ���",	null);
			this.gridHelper.AddColumn( "WarehouseCode", "�ֿ�����",	null);
			this.gridHelper.AddColumn( "WarehouseStatusLabel", "�ֿ�״̬",	null);
			this.gridHelper.AddColumn( "WarehouseItemCode", "���ϴ���",	null);
			this.gridHelper.AddColumn( "WarehouseItemName", "��������",	null);

			if(isWHCycle)
			{
				this.gridHelper.AddColumn( "WarehouseQty", "��ɢ����",	null);
				this.gridHelper.AddColumn( "LineItemQty", "����Ʒ�������",	null);
				this.gridHelper.AddColumn( "CycleQty", "������",	null);
			}
			else
			{
				this.gridHelper.AddColumn( "WarehouseQty", "��ɢ����",	null);
			}
			
			this.gridHelper.AddColumn( "WarehouseItemActualQty", "ʵ����",	null);
			this.gridHelper.AddColumn( "WarehouseStatus", "�ֿ�״̬",	null);
			this.gridHelper.AddColumn( "WarehouseCycleCode", "�̵����",	null);

			this.gridHelper.AddLinkColumn( "WarehouseItemQtyDt", "�ⷿ������ϸ",	null);

			this.gridHelper.AddDefaultColumn( false, true );
			this.gridWebGrid.Columns.FromKey("WarehouseStatus").Hidden = true;
			this.gridWebGrid.Columns.FromKey("WarehouseCycleCode").Hidden = true;
			this.gridHelper.CheckAllBox.Visible = false;
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			string factoryCode, warehouseCode, itemCode,warehouseQty,lingqty, openQty, actualQty,cycleCode;
			if (obj is WarehouseStock)
			{
				WarehouseStock item = (WarehouseStock)obj;
				factoryCode = item.FactoryCode;
				//segmentCode = item.SegmentCode;
				warehouseCode = item.WarehouseCode;
				itemCode = item.ItemCode;
				openQty = Math.Round(Convert.ToDecimal(item.OpenQty), 2).ToString();
				actualQty = "";
				cycleCode = "";
				warehouseQty = "";
				lingqty = "";
			}
			else
			{
				WarehouseCycleCountDetail item = (WarehouseCycleCountDetail)obj;
				factoryCode = item.FactoryCode;
				//segmentCode = item.SegmentCode;
				warehouseCode = item.WarehouseCode;
				itemCode = item.ItemCode;
				warehouseQty = Math.Round(Convert.ToDecimal(item.Qty), 2).ToString();			//��ɢ����	
				lingqty = Math.Round(Convert.ToDecimal(item.LineQty), 2).ToString();			//����Ʒ�������
				openQty = Math.Round(Convert.ToDecimal(item.Warehouse2LineQty), 2).ToString();	//�������� �� ��ɢ���� �� ����Ʒ�������
				actualQty = Math.Round(Convert.ToDecimal(item.PhysicalQty), 2).ToString();		
				cycleCode = item.CycleCountCode;
			}

			Infragistics.WebUI.UltraWebGrid.UltraGridRow row = new UltraGridRow();
			if(isWHCycle)
			{
				row = 
					new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
					new object[]{
									factoryCode,
									//segmentCode,
									warehouseCode,
									GetWarehouseStatusLabel(obj),
									itemCode,
									GetItemName(itemCode),
									warehouseQty,/*	-------��ɢ����*/
									lingqty,	 /* -------����Ʒ�������*/
									openQty,	 /*	-------�������� �� ��ɢ���� �� ����Ʒ�������*/
									actualQty,
									GetWarehouseStatus(obj),
									cycleCode,
									""});
				
			}
			else
			{
				row = 
					new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
					new object[]{
									factoryCode,
									//segmentCode,
									warehouseCode,
									GetWarehouseStatusLabel(obj),
									itemCode,
									GetItemName(itemCode),
									openQty,/*	-------��ɢ����*/
									actualQty,
									GetWarehouseStatus(obj),
									cycleCode,
									""});
			}
			return row;
		}

		private Hashtable _whstatusMap = null;
		private string GetWarehouseStatusLabel(object obj)
		{
			if (this._whstatusMap == null)
			{
				this._whstatusMap = new Hashtable();
				this._whstatusMap.Add(Warehouse.WarehouseStatus_Normal, this.languageComponent1.GetString(Warehouse.WarehouseStatus_Normal));
				this._whstatusMap.Add(Warehouse.WarehouseStatus_Cycle, this.languageComponent1.GetString(Warehouse.WarehouseStatus_Cycle));
				this._whstatusMap.Add(Warehouse.WarehouseStatus_Closed, this.languageComponent1.GetString(Warehouse.WarehouseStatus_Closed));
				this._whstatusMap.Add(Warehouse.WarehouseStatus_Initialize, this.languageComponent1.GetString(Warehouse.WarehouseStatus_Initialize));
			}
			return this._whstatusMap[GetWarehouseStatus(obj)].ToString();
		}
		private string GetWarehouseStatus(object obj)
		{
			if (obj is WarehouseStock)
				return Warehouse.WarehouseStatus_Normal;
			else
				return Warehouse.WarehouseStatus_Cycle;
		}

		private Hashtable htItems = new Hashtable();
		private string GetItemName(string itemCode)
		{
			try
			{
				//�˴��滻��Ƕ���Ϊȫ�Ƕ���,Ϊ������csv׼��
				return this.ReplaceComma(htItems[itemCode].ToString());
			}
			catch
			{
				return string.Empty;
			}
		}
		//�滻��Ƕ���Ϊȫ�Ƕ���
		private string ReplaceComma(string str)
		{
			return str.Replace(",","��");
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new WarehouseFacede2(base.DataProvider);}
			//�ⷿ�Ƿ��̵�״̬ ��Ĭ����������
			// �˴��жϲ�ѯ�ⷿ�Ƿ��̵�״̬��
			isWHCycle = this._facade.isWareHourseCycle(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpWarehouseCodeQuery.Text)), /*this.drpSegmentCodeQuery.SelectedValue,*/ this.drpFactoryCodeQuery.SelectedValue);
			
			this.InitWebGrid();		//�����Ƿ��̵����³�ʼ��Grid
			object[] objs = new object[]{};
			if(isWHCycle)
			{
				//����ⷿ���̵�״̬ , �̵�����Ʒ
				objs = this._facade.QueryWarehouseStockInCheck3(
					string.Empty, FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpWarehouseCodeQuery.Text)),/*this.drpSegmentCodeQuery.SelectedValue,*/ this.drpFactoryCodeQuery.SelectedValue,
					inclusive, exclusive );
			}
			else
			{
				objs = this._facade.QueryWarehouseStockInCheck( 
					string.Empty, FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpWarehouseCodeQuery.Text)),/*this.drpSegmentCodeQuery.SelectedValue,*/ this.drpFactoryCodeQuery.SelectedValue,
					inclusive, exclusive );
			}
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

			if(isExportCycleNow && isWHCycle)
			{
				RecordCycleResult(objs);
			}

			return objs;
		}

		//����ⷿ���̵�״̬ ���������ڵ����̵�������̵��������ݿ�
		private void RecordCycleResult(object[] objs)
		{
			if(isExportCycleNow && isWHCycle)
			{
				this._facade.ExportUpdateCycleResult(objs);
			}
		}

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new WarehouseFacede2(base.DataProvider);}
				return this._facade.QueryWarehouseStockInCheckCount(string.Empty, FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpWarehouseCodeQuery.Text)),/*this.drpSegmentCodeQuery.SelectedValue,*/this.drpFactoryCodeQuery.SelectedValue);
		}

		#endregion

		#region Button

		protected override void UpdateDomainObject(object domainObject)
		{
			if(_facade==null){_facade = new WarehouseFacede2(base.DataProvider);}
			this._facade.UpdateWarehouseStock( (WarehouseStock)domainObject );
		}
		
		protected override void cmdQuery_Click(object sender, EventArgs e)
		{
			base.cmdQuery_Click (sender, e);

			this.txtFactoryCode.Value = this.drpFactoryCodeQuery.SelectedValue;
			//this.txtSegmentCode.Value = this.drpSegmentCodeQuery.SelectedValue;
			this.txtWarehouseCode.Value = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpWarehouseCodeQuery.Text));

			this.cmdExportBook.Disabled = false;
			this.cmdCloseAccount.Disabled = false;
		}

		protected void cmdClose_ServerClick(object sender, EventArgs e)
		{
			if(_facade==null){_facade = new WarehouseFacede2(base.DataProvider);}
			this._facade.WarehouseClose(this.txtWarehouseCode.Value, /* this.txtSegmentCode.Value,*/ this.txtFactoryCode.Value, this.GetUserCode());

			this.drpFactoryCodeQuery.SelectedValue = this.txtFactoryCode.Value;
			//this.drpSegmentCodeQuery.SelectedValue = this.txtSegmentCode.Value;
			this.drpWarehouseCodeQuery.Text = this.txtWarehouseCode.Value;
			this.cmdQuery_Click(null, null);
		}

		protected void cmdExportCycle_ServerClick(object sender, EventArgs e)
		{
			this.isExportCycleNow = true;	//���ڵ���.

			this.excelExporter.CellSplit = ",";
			this.excelExporter.FileExtension = "csv";
			this.cmdExport_Click(sender, e);
		}
		protected void cmdImport_ServerClick(object sender, EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("../Warehouse/FWHCycleImpSP.aspx"));
		}

		protected void cmdUpdate_ServerClick(object sender, EventArgs e)
		{
			if(_facade==null){_facade = new WarehouseFacede2(base.DataProvider);}
			object obj = this._facade.GetWarehouseCycleCountDetail(this.txtItemCode.Value, this.txtCycleCountCode.Value);
			if (obj == null)
				return;
			WarehouseCycleCountDetail item = (WarehouseCycleCountDetail)obj;
			item.PhysicalQty = decimal.Parse(this.txtActualQtyEdit.Text);
			this._facade.UpdateWarehouseCycleCountDetail(item);
			item = null;
			this.cmdQuery_Click(null, null);
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
			if(_facade==null){_facade = new WarehouseFacede2(base.DataProvider);}
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

		#region ���ݳ�ʼ��

		private void InitDropDownList()
		{
			if(_facade==null){_facade = new WarehouseFacede2(base.DataProvider);}
			BenQGuru.eMES.Common.MutiLanguage.LanguageWord lword  = languageComponent1.GetLanguage("listItemAll");
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
			lword = null;
			builder = null;
		}
		#endregion

		#region Object <--> Page
		protected override object GetEditObject()
		{
			return null;
		}

		protected override object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
		{	
			if(_facade==null){_facade = new WarehouseFacede2(base.DataProvider);}
			if (row.Cells[8].Text.ToString() != Warehouse.WarehouseStatus_Cycle)
				return null;
			object obj = _facade.GetWarehouseCycleCountDetail(row.Cells[4].ToString(), row.Cells[9].ToString());
			
			if (obj != null)
			{
				return (WarehouseCycleCountDetail)obj;
			}

			return null;
		}

		protected override void SetEditObject(object obj)
		{
			if (obj == null)
			{
				this.txtActualQtyEdit.Text = "";

				return;
			}

			WarehouseCycleCountDetail item = (WarehouseCycleCountDetail)obj;
			this.txtActualQtyEdit.Text = item.PhysicalQty.ToString();
			this.txtItemCode.Value = item.ItemCode;
			this.txtCycleCountCode.Value = item.CycleCountCode;
			
			item = null;

			this.txtActualQtyEdit.ReadOnly = false;
			this.cmdSave.Disabled = false;
		}

		
		protected override bool ValidateInput()
		{			
			PageCheckManager manager = new PageCheckManager();
			
			manager.Add( new DecimalCheck(lblActualQtyEdit, txtActualQtyEdit, 0, decimal.MaxValue, true));

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);

				return false;
			}

			return true;
		}

		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			string factoryCode, warehouseCode, itemCode,warehouseQty,lingqty, openQty, actualQty,cycleCode;
			if (obj is WarehouseStock)
			{
				WarehouseStock item = (WarehouseStock)obj;
				factoryCode = item.FactoryCode;
				//segmentCode = item.SegmentCode;
				warehouseCode = item.WarehouseCode;
				itemCode = item.ItemCode;
				openQty = Math.Round(Convert.ToDecimal(item.OpenQty), 2).ToString();
				actualQty = "";
				cycleCode = "";
				warehouseQty = "";
				lingqty = "";
			}
			else
			{
				WarehouseCycleCountDetail item = (WarehouseCycleCountDetail)obj;
				factoryCode = item.FactoryCode;
				//segmentCode = item.SegmentCode;
				warehouseCode = item.WarehouseCode;
				itemCode = item.ItemCode;
				warehouseQty = Math.Round(Convert.ToDecimal(item.Qty), 2).ToString();			//��ɢ����	
				lingqty = Math.Round(Convert.ToDecimal(item.LineQty), 2).ToString();			//����Ʒ�������
				openQty = Math.Round(Convert.ToDecimal(item.Warehouse2LineQty), 2).ToString();	//�������� �� ��ɢ���� �� ����Ʒ�������
				actualQty = Math.Round(Convert.ToDecimal(item.PhysicalQty), 2).ToString();		
				cycleCode = item.CycleCountCode;
			}

			string[] strArr =  new string[]{};
			if(isWHCycle)
			{
				strArr = 
					new string[]{	factoryCode,
									//segmentCode,
									warehouseCode,
									itemCode,
									GetItemName(itemCode),
									warehouseQty,			//��ɢ����
									lingqty,			//����Ʒ�������
									openQty,			//�������� �� ��ɢ���� �� ����Ʒ�������
									""
								};
			}
			else
			{
				strArr = 
					new string[]{	factoryCode,
									//segmentCode,
									warehouseCode,
									itemCode,
									GetItemName(itemCode),
									openQty,
									""
								};
			}
			return strArr;
		}

		protected override string[] GetColumnHeaderText()
		{
			string[] strArr =  new string[]{};
			if(isWHCycle)
			{
				strArr =  new string[] {	
										   "FactoryCode",
										   //"SegmentCode",
										   "WarehouseCode",
										   "WarehouseItemCode",
										   "WarehouseItemName",
										   "WarehouseQty",			//��ɢ����
										   "Lingqty",			//����Ʒ�������
										   "BillOpenQty",		//�������� �� ��ɢ���� �� ����Ʒ�������
										   "CycleActualQty"
									   };
			}
			else
			{
				strArr =  new string[] {	
										"FactoryCode",
										//"SegmentCode",
										"WarehouseCode",
										"WarehouseItemCode",
										"WarehouseItemName",
										"WarehouseQty",
										"CycleActualQty"
									};
			}
			return strArr;
		}

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

		#endregion

	}
}
