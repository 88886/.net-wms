using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Client.Service;
using BenQGuru.eMES.MOModel;
using System.Collections;
using BenQGuru.eMES.Domain.MOModel;
using UserControl;
using Infragistics.Win.UltraWinGrid;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.Client
{
    public partial class FRecNoSelect : BaseForm
    {
        private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;

        private DataTable m_dtProduct;

        public event ParentChildRelateEventHandler<ParentChildRelateEventArgs<Hashtable>> ProductInfoEvent;

        public void OnProductInfoEvent(object sender, ParentChildRelateEventArgs<Hashtable> e)
        {
            if (this.ProductInfoEvent != null)
            {
                ProductInfoEvent(sender, e);
            }
        }

        private IDomainDataProvider DataProvider
        {
            get
            {
                return _domainDataProvider;
            }
        }

        public FRecNoSelect()
        {
            InitializeComponent();
        }

        private void gridProduct_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // 自适应列宽
            e.Layout.AutoFitColumns = false;
            e.Layout.Override.AllowColSizing = AllowColSizing.None;

            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;

            // 设置Grid的Split窗口个数，建议设置为1--不允许Split
            e.Layout.MaxColScrollRegions = 1;
            e.Layout.MaxRowScrollRegions = 1;

            // 排序
            e.Layout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;

            e.Layout.Bands[0].Columns["ReceiptNO"].Width = 250;
            //e.Layout.Bands[0].Columns["itemdesc"].Width = 370;

            // 栏位名称
            e.Layout.Bands[0].Columns["ReceiptNO"].Header.Caption = "入库单号";
            //e.Layout.Bands[0].Columns["itemdesc"].Header.Caption = "库单描述";

            // 设置栏位是否允许编辑，及栏位的显示形式
            e.Layout.Bands[0].Columns["ReceiptNO"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            //e.Layout.Bands[0].Columns["itemdesc"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
        }

        private void FProductSelect_Load(object sender, EventArgs e)
        {
            UserControl.UIStyleBuilder.FormUI(this);
            UserControl.UIStyleBuilder.GridUI(this.gridProduct);

            InitialDataTable();
            DoQuery("");

            //this.InitGridLanguage(gridProduct);
            //this.InitPageLanguage();
        }

        private void InitialDataTable()
        {
            this.m_dtProduct = new DataTable();
            this.m_dtProduct.Columns.Add("ReceiptNO");
            //this.m_dtProduct.Columns.Add("itemdesc");
            this.gridProduct.DataSource = this.m_dtProduct;
        }

        private void DoQuery(string receiptNo)
        {
            //if (itemCode.Length==0)
            //{
            //    //Message:请至少填入一个查询条件
            //    ApplicationRun.GetInfoForm().Add(
            //        new UserControl.Message(MessageType.Error, "$CS_INPUT_QUERY_CONDITION"));
            //    return;
            //}

            IQC.IQCFacade _IQCFacade = new IQC.IQCFacade(this.DataProvider);
            object[] itemList = _IQCFacade.QueryINVReceipt(receiptNo, 0, int.MaxValue);

            this.m_dtProduct.Clear();

            if (itemList != null)
            {
                
                foreach (Domain.IQC.InvReceipt item in itemList)
                {
                    DataRow dr = this.m_dtProduct.NewRow();
                    dr["ReceiptNO"] = item.Receiptno;
                    //dr["itemdesc"] = item.ItemDescription;
                    this.m_dtProduct.Rows.Add(dr);
                }
            }
            
        }

        private void txtItemCode_TxtboxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                //if (this.txtItemCode.Value.Trim().Length == 0)
                //{
                //    return;
                //}
                
                this.DoQuery(FormatHelper.CleanString(this.txtInvRecNo.Value.Trim()));
            }
        }


        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (this.gridProduct.Rows.Count ==0)
            {
                //表格中没有数据
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_GRID_NO_RECORD"));
                return;
            }

            if (this.gridProduct.ActiveRow != null)
            {
                Hashtable ht = new Hashtable();
                ht.Add("ReceiptNO", this.gridProduct.ActiveRow.Cells["ReceiptNO"].Value.ToString());
                //ht.Add("itemdesc", this.gridProduct.ActiveRow.Cells["itemdesc"].Value.ToString());
                ParentChildRelateEventArgs<Hashtable> args = new ParentChildRelateEventArgs<Hashtable>(ht);
                this.OnProductInfoEvent(sender, args);
                this.Close();
            }
            else
            {
                //请选择一笔数据
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_GRID_SELECT_ONE_RECORD"));
            }
        }

        private void gridProduct_DoubleClick(object sender, EventArgs e)
        {
            cmdOK_Click(sender, e);
        }



    }
}