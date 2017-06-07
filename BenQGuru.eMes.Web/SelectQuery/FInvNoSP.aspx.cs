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

using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Warehouse;

namespace BenQGuru.eMES.Web.SelectQuery
{
    public partial class FInvNoSP : BaseSelectorPageNew
    {
        private BenQGuru.eMES.SelectQuery.SPFacade facade;

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
            this.gridSelectedHelper.AddColumn("Selector_SelectedInvNo", "Ԥ������", null);
            this.gridSelectedHelper.AddColumn("Selector_SelectedContact", "��ϵ��", null);
            this.gridSelectedHelper.AddColumn("Selector_SelectedStorage", "��λ", null);
            this.gridSelectedHelper.AddDefaultColumn(true, false);
            this.gridSelectedHelper.ApplyLanguage(this.languageComponent1);

            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedInvNo", "Ԥ������", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnSelectedContact", "��ϵ��", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnSelectedStorage", "��λ", null);
            this.gridUnSelectedHelper.AddDefaultColumn(true, false);
            this.gridUnSelectedHelper.ApplyLanguage(this.languageComponent1);

           
          
        }
        protected override DataRow GetSelectedGridRow(object obj)
        {
            DataRow row = DtSourceSelected.NewRow();
            row["Selector_SelectedInvNo"] = ((InvoicesDetail)obj).InvNo;
            row["Selector_SelectedContact"] = ((InvoicesDetail)obj).ReceiverUser;
            row["Selector_SelectedStorage"] = ((InvoicesDetail)obj).FromStorageCode;
            return row;
        }

        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedInvNo"] = ((InvoicesDetail)obj).InvNo;
            row["Selector_UnSelectedContact"] = ((InvoicesDetail)obj).ReceiverUser;
            row["Selector_UnSelectedStorage"] = ((InvoicesDetail)obj).FromStorageCode;
            return row;
        }

        protected override object[] LoadSelectedDataSource(int inclusive, int exclusive)
        {
            if (facade == null) 
            { 
                facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); 
            }

            return this.facade.QuerySelectedInvNo(this.GetSelectedCodes());

        }

        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {
            if (facade == null) 
            { 
                facade = new FacadeFactory(base.DataProvider).CreateSPFacade();
            }

            return this.facade.QueryUNSelectInvNo(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtYLInvNoQuery.Text)),
                FormatHelper.CleanString(this.txtContactQuery.Text),
                FormatHelper.CleanString(this.txtStorageQuery.Text), 
                this.GetSelectedCodes(), 
                inclusive, 
                exclusive);
        }


        protected override int GetUnSelectedRowCount()
        {
            if (facade == null) 
            { 
                facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); 
            }

            return this.facade.QueryUNSelectInvNoCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtYLInvNoQuery.Text)),
                FormatHelper.CleanString(this.txtContactQuery.Text),
                FormatHelper.CleanString(this.txtStorageQuery.Text),
                this.GetSelectedCodes());
        }

        #endregion    
    
    }
}
