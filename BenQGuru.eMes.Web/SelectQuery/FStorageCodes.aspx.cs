﻿using System;
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
    /// FSingleItemSP 的摘要说明。
    /// </summary>
    public partial class FStorageCodes : BaseSingleSelectorPageNew2
    {

        private BenQGuru.eMES.SelectQuery.SPFacade facade;//= FacadeFactory.CreateSPFacade() ;
     

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
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
                // 初始化页面语言
                this.InitPageLanguage(this.languageComponent1, false);
            }
          
        }

        #endregion

        #region WebGrid
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedCode", "库位号", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedStorageName", "库位名称", null);
            
            this.gridUnSelectedHelper.AddDefaultColumn(true, false);
            //this.gridUnSelectedHelper.ApplyLanguage(this.languageComponent1);
            //this.gridUnSelected.Columns["Selector_UnselectedCode"].Hidden = true;
            //this.gridUnSelected.Columns["Selector_UnSelectedDesc"].Hidden = true;
        }

              
        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((Storage)obj).StorageCode;
            row["Selector_UnselectedStorageName"] = ((Storage)obj).StorageName;
          
            return row;
        }


        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUnSelectedStorageCode(
            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtStorageCodeQuery.Text)),
            inclusive,
            exclusive);
        }


        protected override int GetUnSelectedRowCount()
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUnSelectedStorageCodeCount(
           FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtStorageCodeQuery.Text)));

        }


        #endregion

    }
}
