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
using System.Runtime.Remoting;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Report;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Common;

namespace BenQGuru.eMES.Web.SelectQuery
{
    public partial class FStorageTypeSP : BaseSelectorPageNew
    {
        private BenQGuru.eMES.SelectQuery.SPFacade facade;// = new FacadeFactory(base.DataProvider).CreateSPFacade() ;


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
        protected override DataRow GetSelectedGridRow(object obj)
        {
            DataRow row = DtSourceSelected.NewRow();
            row["Selector_SelectedCode"] = ((BenQGuru.eMES.Domain.Warehouse.Storage)obj).StorageCode;
            row["Selector_SelectedDesc"] = ((BenQGuru.eMES.Domain.Warehouse.Storage)obj).StorageName;
            return row;
        }

        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((BenQGuru.eMES.Domain.Warehouse.Storage)obj).StorageCode;
            row["Selector_UnSelectedDesc"] = ((BenQGuru.eMES.Domain.Warehouse.Storage)obj).StorageName;
            return row;
        }

        protected override object[] LoadSelectedDataSource(int inclusive, int exclusive)
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QuerySelectedStorageType(this.GetSelectedCodes());

        }

        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {

            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUnSelectedStorageType(this.txtStorageTypeEdit.Text, this.txtStorageDescEdit.Text, this.GetSelectedCodes(), inclusive, exclusive);
        }


        protected override int GetUnSelectedRowCount()
        {


            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUnSelectedStorageTypeCount(this.txtStorageTypeEdit.Text, this.txtStorageDescEdit.Text, this.GetSelectedCodes());
        }

        #endregion
    }
}
