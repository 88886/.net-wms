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
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Web.UserControl;

namespace BenQGuru.eMES.Web.SelectQuery
{
    /// <summary>
    /// FSingleItemSP ��ժҪ˵����
    /// </summary>
    public partial class FSingleInvNoMaterialSP : BaseSingleSelectorPageNew
    {

        private BenQGuru.eMES.SelectQuery.SPFacade facade;//= FacadeFactory.CreateSPFacade() ;
     

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
            txtYLInvNoQuery.Text = "";
            if (Request.QueryString["PickNo"] != null)
            {
                string pickno = Request.QueryString["PickNo"];
                InventoryFacade inventoryFacade = new InventoryFacade(base.DataProvider);
                Pick pick = (Pick)inventoryFacade.GetPick(pickno);
                if (pick != null)
                {
                    txtYLInvNoQuery.Text = pick.InvNo;
                    txtStorageCodeEidt.Text = pick.StorageCode;
                }
            }
            txtStorageCodeEidt.Visible = false;
      
          
        }

        #endregion

        #region WebGrid
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedDQMCode", "�������ϱ���", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedCustmCode", " ��Ӧ�����ϱ���", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedDQMCodeQty", "��������", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnSelectedMdes", "��������", null);
            this.gridUnSelectedHelper.AddDefaultColumn(true, false);
            this.gridUnSelectedHelper.ApplyLanguage(this.languageComponent1);
            //this.gridUnSelected.Columns["Selector_UnselectedCode"].Hidden = true;
            this.gridUnSelected.Columns["Selector_UnSelectedDesc"].Hidden = true;
        }


        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((InvoicesDetailQuery)obj).InvLine;
            row["Selector_UnselectedDQMCode"] = ((InvoicesDetailQuery)obj).DQMCode;
            row["Selector_UnselectedCustmCode"] = ((InvoicesDetailQuery)obj).CustmCode;
            row["Selector_UnselectedDQMCodeQty"] = ((InvoicesDetailQuery)obj).PlanQty.ToString("G0");
            row["Selector_UnSelectedMdes"] = ((InvoicesDetailQuery)obj).MChLongDesc;
            return row;
        }


        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUNSelectInvNoMaterial(
            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtYLInvNoQuery.Text)),
            FormatHelper.CleanString(this.txtDQMoCodeQuery.Text),
            FormatHelper.CleanString(this.txtStorageCodeEidt.Text),
           new string[0],
            inclusive,
            exclusive);
        }


        protected override int GetUnSelectedRowCount()
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUNSelectInvNoMaterialCount(
        FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtYLInvNoQuery.Text)),
       FormatHelper.CleanString(this.txtDQMoCodeQuery.Text),
       FormatHelper.CleanString(this.txtStorageCodeEidt.Text),
 
        new string[0]);

        }


        #endregion

    }
}
