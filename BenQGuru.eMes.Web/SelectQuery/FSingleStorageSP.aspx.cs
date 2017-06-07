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

namespace BenQGuru.eMES.Web.SelectQuery
{
    public partial class FSingleStorageSP : BaseSelectorPageNew
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
        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((Domain.Warehouse.Storage)obj).StorageCode;
            row["Selector_UnSelectedDesc"] = ((Domain.Warehouse.Storage)obj).StorageName;
            return row;
        }

        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {

            if (facade == null) 
            { 
                facade = new FacadeFactory(base.DataProvider).CreateSPFacade();
            }
            return this.facade.QueryUnSelectedStorageCode(this.txtStorageQuery.Text, this.GetSelectedCodes(), inclusive, exclusive);
        }


        protected override int GetUnSelectedRowCount()
        {

            if (facade == null)
            { 
                facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); 
            }
            return this.facade.QueryUnSelectedStorageCodeCount(this.txtStorageQuery.Text, this.GetSelectedCodes());


        #endregion
        }
    }
}
