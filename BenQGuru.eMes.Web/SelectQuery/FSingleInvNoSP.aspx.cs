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
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Web.UserControl;

namespace BenQGuru.eMES.Web.SelectQuery
{
    /// <summary>
    /// FSingleItemSP ��ժҪ˵����
    /// </summary>
    public partial class FSingleInvNoSP : BaseSingleSelectorPageNew
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
        }

        #endregion

        #region WebGrid
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedInvNo", "Ԥ������", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnSelectedContact", "��ϵ��", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnSelectedStorage", "��λ", null);
            this.gridUnSelectedHelper.AddDefaultColumn(true, false);
            this.gridUnSelectedHelper.ApplyLanguage(this.languageComponent1);
            this.gridUnSelected.Columns["Selector_UnselectedCode"].Hidden = true;
            this.gridUnSelected.Columns["Selector_UnSelectedDesc"].Hidden = true;
        }


        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();

            row["Selector_UnselectedCode"] = ((InvoicesDetail)obj).InvNo +","+ ((InvoicesDetail)obj).StorageCode;
            row["Selector_UnselectedInvNo"] = ((InvoicesDetail)obj).InvNo;
            row["Selector_UnSelectedContact"] = ((InvoicesDetail)obj).ReceiverUser;
            row["Selector_UnSelectedStorage"] = ((InvoicesDetail)obj).StorageCode;
            return row;
        }


        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUNSelectInvNo(
            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtYLInvNoQuery.Text)),
            FormatHelper.CleanString(this.txtContactQuery.Text),
            FormatHelper.CleanString(this.txtStorageQuery.Text),
           new string[0],
            inclusive,
            exclusive);
        }


        protected override int GetUnSelectedRowCount()
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUNSelectInvNoCount(
        FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtYLInvNoQuery.Text)),
        FormatHelper.CleanString(this.txtContactQuery.Text),
        FormatHelper.CleanString(this.txtStorageQuery.Text),
        new string[0]);

        }


        #endregion

    }
}
