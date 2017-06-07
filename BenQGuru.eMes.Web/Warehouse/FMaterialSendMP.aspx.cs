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
using BenQGuru.eMES.Material;
using BenQGuru.eMES.SAPDataTransfer;
using BenQGuru.eMES.SAPDataTransferInterface;
using System.Collections.Generic;

namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FMaterialSendMP : BaseMPage
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
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        protected void drpStatus_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                this.drpStatusQuery.Items.Add(new ListItem("", ""));
                this.drpStatusQuery.Items.Add(new ListItem(FlagStatus.FlagStatus_MES, FlagStatus.FlagStatus_MES));
                this.drpStatusQuery.Items.Add(new ListItem(FlagStatus.FlagStatus_POST, FlagStatus.FlagStatus_POST));
                this.drpStatusQuery.Items.Add(new ListItem(FlagStatus.FlagStatus_SAP, FlagStatus.FlagStatus_SAP));
            }
        }

        #region WebGrid

        protected override void InitWebGrid()
        {
            this.gridHelper.AddColumn("MaterialLot", "��������", null);
            this.gridHelper.AddColumn("PostSeq", "���к�", null);
            this.gridHelper.AddColumn("MaterialCode", "���ϴ���", null);
            this.gridHelper.AddColumn("MaterialDescription", "��������", null);
            this.gridHelper.AddColumn("VendorCode", "��Ӧ��", null);
            this.gridHelper.AddColumn("MoCode", "����", null);
            this.gridHelper.AddColumn("TransQTY", "��������", null);
            this.gridHelper.AddColumn("ReceiveMemo", "̧ͷ�ı�", null);
            this.gridHelper.AddColumn("AccountDate", "��������", null);
            this.gridHelper.AddColumn("VoucherDate", "ƾ֤����", null);
            this.gridHelper.AddColumn("OrgID", "��������", null);
            this.gridHelper.AddColumn("FRMStorageID", "�������", null);
            this.gridHelper.AddColumn("TOStorageID", "�ջ����", null);
            this.gridHelper.AddColumn("Unit", "��λ", null);
            this.gridHelper.AddColumn("Status", "״̬", null);
            this.gridHelper.AddColumn("ToItemCode", "ת���Ϻ�", null);
            this.gridHelper.AddColumn("SAPCode", "SAPCode", null);
            this.gridHelper.AddColumn("ErrorMessage", "������Ϣ", null);
            this.gridHelper.AddLinkColumn("SyncStatus", "״̬ͬ��", null);


            this.gridWebGrid.Columns.FromKey("MaterialLot").Width = new Unit(153, UnitType.Pixel);
            this.gridWebGrid.Columns.FromKey("TransQTY").Width = new Unit(62, UnitType.Pixel);
            this.gridWebGrid.Columns.FromKey("ReceiveMemo").Width = new Unit(62, UnitType.Pixel);
            this.gridWebGrid.Columns.FromKey("Unit").Width = new Unit(35, UnitType.Pixel);
            this.gridWebGrid.Columns.FromKey("SyncStatus").Width = new Unit(62, UnitType.Pixel);
            this.gridWebGrid.Columns.FromKey("Status").Width = new Unit(38, UnitType.Pixel);
            this.gridWebGrid.Columns.FromKey("AccountDate").Width = new Unit(65, UnitType.Pixel);
            this.gridWebGrid.Columns.FromKey("VoucherDate").Width = new Unit(65, UnitType.Pixel);
            this.gridWebGrid.Columns.FromKey("MaterialDescription").Width = new Unit(70, UnitType.Pixel);

            this.gridWebGrid.Columns.FromKey("TransQTY").DataType = "System.Int32";
            this.gridWebGrid.Columns.FromKey("PostSeq").DataType = "System.Int32";

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override void cmdQuery_Click(object sender, EventArgs e)
        {
            PageCheckManager manager = new PageCheckManager();

            manager.Add(new DateRangeCheck(this.lblVoucherDateFrom, this.dateVoucherDateFrom.Text, this.dateVoucherDateTo.Text, false));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return;
            }

            base.cmdQuery_Click(sender, e);
        }

        protected override int GetRowCount()
        {
            if (_facade == null) { _facade = new InventoryFacade(base.DataProvider); }

            return this._facade.QuerySendMaterialCount(

                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMaterialCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtVerdorCodeQuery.Text)),
                FormatHelper.TODateInt(this.dateVoucherDateFrom.Text),
                FormatHelper.TODateInt(this.dateVoucherDateTo.Text),
                FormatHelper.CleanString(this.drpStatusQuery.SelectedValue));

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null) { _facade = new InventoryFacade(base.DataProvider); }
            return this._facade.QuerySendMaterial(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMaterialCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtVerdorCodeQuery.Text)),
                FormatHelper.TODateInt(this.dateVoucherDateFrom.Text),
                FormatHelper.TODateInt(this.dateVoucherDateTo.Text),
                FormatHelper.CleanString(this.drpStatusQuery.SelectedValue),
                inclusive, exclusive);
        }

        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
            SAPMaterialTransDesc item = obj as SAPMaterialTransDesc;

            return new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
                new object[]{
                                item.MaterialLotNo,
                                item.PostSeq,
                                item.ItemCode,
                                item.MaterialDescription, 
                                item.VendorCode,
                                item.MoCode,
                                item.TransQTY,
                                item.ReceiveMemo,
                                FormatHelper.ToDateString(item.AccountDate),
                                FormatHelper.ToDateString(item.VoucherDate),
                                item.OrganizationID.ToString(),
                                item.FRMStorageID,
                                item.TOStorageID,
                                item.Unit,
                                item.Flag,
                                item.ToItemCode,
                                item.SAPCode,
                                string.Compare(item.Flag, "SAP", true) == 0 ? "" : item.ErrorMessage,
                                ""                            
                });
        }

        protected override void Grid_ClickCellButton(object sender, Infragistics.WebUI.UltraWebGrid.CellEventArgs e)
        {
            string userCode = this.GetUserCode();

            if (this.gridHelper.IsClickColumn("SyncStatus", e))
            {
                if (string.Compare(e.Cell.Row.Cells.FromKey("Status").Value.ToString(), FlagStatus.FlagStatus_MES, true) != 0
                    && string.Compare(e.Cell.Row.Cells.FromKey("Status").Value.ToString(), FlagStatus.FlagStatus_POST, true) != 0)
                {
                    WebInfoPublish.Publish(this, "$Error_OnlyMESAndPOSTStatusCanDo", this.languageComponent1);
                }
                else
                {
                    if (_facade == null) { _facade = new InventoryFacade(base.DataProvider); }
                    this._facade.ManualSyncMaterialSendFlag(
                        e.Cell.Row.Cells.FromKey("MaterialLot").Value.ToString(),
                        int.Parse(e.Cell.Row.Cells.FromKey("PostSeq").Value.ToString()),
                        userCode);

                    this.cmdQuery_Click(null, null);
                }
            }
        }

        #endregion

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"MaterialLotNo",
                                    "PostSeq",
                                    "MaterialCode",	
                                    "MaterialDescription",
                                    "VendorCode",
                                    "MoCode",
                                    "TransQTY",
                                    "ReceiveMemo",
                                    "AccountDate",
                                    "VoucherDate",	
                                    "OrgID",
                                    "FRMStorageID",
                                    "TOStorageID",
                                    "Unit",
                                    "Status",	
                                    "ToItemCode",
                                    "SAPCode",
                                    "ErrorMessage"
                                   };


        }

        protected override string[] FormatExportRecord(object obj)
        {
            SAPMaterialTransDesc item = obj as SAPMaterialTransDesc;

            return new string[]{
                               item.MaterialLotNo,
                                item.PostSeq.ToString(),
                                item.ItemCode,
                                item.MaterialDescription, 
                                item.VendorCode,
                                item.MoCode,
                                item.TransQTY.ToString(),
                                item.ReceiveMemo,
                                FormatHelper.ToDateString(item.AccountDate),
                                FormatHelper.ToDateString(item.VoucherDate),
                                item.OrganizationID.ToString(),
                                item.FRMStorageID,
                                item.TOStorageID,
                                item.Unit,
                                item.Flag,
                                item.ToItemCode,
                                item.SAPCode,
                                item.ErrorMessage
                               
                };



        }
    }
}
