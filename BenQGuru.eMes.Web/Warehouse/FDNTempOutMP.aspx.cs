using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BenQGuru.eMES.Web.Helper;

using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using System.Collections.Generic;

using Infragistics.WebUI.UltraWebGrid;

namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FDNTempOutMP : BaseMPage
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        private InventoryFacade _facade = null;

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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                this.InitUI();
                DrpStorageTypeQuery_Init();

            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        private void DrpStorageTypeQuery_Init()
        {
            if (_facade == null)
            {
                _facade = new InventoryFacade(this.DataProvider);
            }

            object[] StorageList = _facade.GetAllStorageCode();

            this.drpStorageTypeQuery.Items.Clear();

            this.drpStorageTypeQuery.Items.Add(new ListItem("", ""));
            for (int i = 0; i < StorageList.Length; i++)
            {
                //this.drpStorageTypeQuery.Items.Add(new ListItem(((Storage)StorageList[i]).StorageName, ((Storage)StorageList[i]).StorageCode));
            }
            this.drpStorageTypeQuery.SelectedIndex = 0;
        }
        #region WebGrid

        protected override void InitWebGrid()
        {
            this.gridHelper.AddColumn("Storage", "���", null);
            this.gridHelper.AddColumn("StorageDesc", "�������", null);
            this.gridHelper.AddColumn("Company", "��˾��", null);
            this.gridHelper.AddColumn("StackCode", "��λ����", null);
            this.gridHelper.AddColumn("ItemCode", "��Ʒ����", null);
            this.gridHelper.AddColumn("ItemDescription", "��Ʒ����", null);
            this.gridHelper.AddColumn("MModelCode", "����", null);
            this.gridHelper.AddColumn("lotqty", "�ڿ�����", null);
            this.gridHelper.AddColumn("Mo_MOActualQty", "���깤����", null);
            this.gridHelper.AddColumn("SAP_REPORTQTY", "SAP��������", null);
            this.gridHelper.AddColumn("TempOutQty", "������", null);
            this.gridHelper.AddColumn("CanDoQty", "��������", null);
            this.gridHelper.AddLinkColumn("TempDN", "�󶨽�����", null);

            this.gridHelper.AddDefaultColumn(false, false);
            this.gridWebGrid.Columns.FromKey("TempDN").Width = 75;
            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override void cmdQuery_Click(object sender, EventArgs e)
        {
            base.cmdQuery_Click(sender, e);
        }

        protected override int GetRowCount()
        {
            if (_facade == null)
            {
                _facade = new InventoryFacade(base.DataProvider);
            }
            return this._facade.GetDNTempOutCount(this.drpStorageTypeQuery.SelectedValue,
                                                  FormatHelper.CleanString(this.txtItemCodeEdit.Text.ToUpper()),
                                                  FormatHelper.CleanString(this.txtMmodelcode.Text.ToUpper()),
                                                  FormatHelper.CleanString(this.txtStackCodeQuery.Text.ToUpper())
                                                        );

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null)
            {
                _facade = new InventoryFacade(base.DataProvider);
            }

            if (this.drpStorageTypeQuery.SelectedValue.ToString().Trim() == string.Empty)
            {
                WebInfoPublish.Publish(this, "$ERROR_Storage_Must_Selected", this.languageComponent1);
                return null;
            }

            return this._facade.QueryDNTempOut(this.drpStorageTypeQuery.SelectedValue,
                                              FormatHelper.CleanString(this.txtItemCodeEdit.Text.ToUpper()),
                                              FormatHelper.CleanString(this.txtMmodelcode.Text.ToUpper()),
                                              FormatHelper.CleanString(this.txtStackCodeQuery.Text.ToUpper()),
                                              inclusive, exclusive);

        }

        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
            return new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
                new object[]{
								((DNTempOutMessage)obj).StorageCode.ToString(),
								((DNTempOutMessage)obj).StorageName.ToString(),
								((DNTempOutMessage)obj).Company.ToString(),
                                ((DNTempOutMessage)obj).StackCode.ToString(),
                                ((DNTempOutMessage)obj).ItemCode.ToString(),
								((DNTempOutMessage)obj).ItemDescription.ToString(),
								((DNTempOutMessage)obj).MModelCode.ToString(),
                                ((DNTempOutMessage)obj).INVQTY.ToString(),
                                ((DNTempOutMessage)obj).COMQTY.ToString(),
                                ((DNTempOutMessage)obj).SAPQTY.ToString(),
                                ((DNTempOutMessage)obj).TEMPQTY.ToString(),
                                (((DNTempOutMessage)obj).SAPQTY-((DNTempOutMessage)obj).TEMPQTY).ToString(),
                                ""          
								});
        }

        protected override void Grid_ClickCell(UltraGridCell cell)
        {
            string userCode = this.GetUserCode();
            base.Grid_ClickCell(cell);
            if (this.gridHelper.IsClickColumn("TempDN", cell))
            {
                Response.Redirect(this.MakeRedirectUrl("FDNTempOutSP.aspx", new string[] { "Storage","Company", "StackCode", "ItemCode", "MModelCode" },
                    new string[] { 
                                cell.Row.Cells[0].ToString(), 
                                cell.Row.Cells[2].ToString(),
                                cell.Row.Cells[3].ToString(), 
                                cell.Row.Cells[4].ToString(),
                                cell.Row.Cells[6].ToString()
                    }));
            }
        }

        #endregion

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"Storage",
									"StorageDesc",	
									"Company",
				                    "StackCode",
									"ItemCode",	
									"ItemDescription",
				                    "MModelCode",
									"lotqty",	
									"Mo_MOActualQty",
                                    "SAP_REPORTQTY" ,
                                     "TempOutQty",
                                    "CanDoQty"
            };
        }

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{ ((DNTempOutMessage)obj).StorageCode.ToString(),
								((DNTempOutMessage)obj).StorageName.ToString(),
								((DNTempOutMessage)obj).Company.ToString(),
                                ((DNTempOutMessage)obj).StackCode.ToString(),
                                ((DNTempOutMessage)obj).ItemCode.ToString(),
								((DNTempOutMessage)obj).ItemDescription.ToString(),
								((DNTempOutMessage)obj).MModelCode.ToString(),
                                ((DNTempOutMessage)obj).INVQTY.ToString(),
                                ((DNTempOutMessage)obj).COMQTY.ToString(),
                                ((DNTempOutMessage)obj).SAPQTY.ToString(),
                                ((DNTempOutMessage)obj).TEMPQTY.ToString(),
                                (((DNTempOutMessage)obj).SAPQTY-((DNTempOutMessage)obj).TEMPQTY).ToString()
                                            
            };
        }
    }
}
