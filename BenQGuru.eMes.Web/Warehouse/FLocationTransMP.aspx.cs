using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using System.Collections.Generic;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FLocationTransMP : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private WarehouseFacade _WarehouseFacade = null;
        private InventoryFacade facade = null;

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
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                InitStorageList();


                string locationCode = Request.QueryString["LOCATIONNO"];
                if (!string.IsNullOrEmpty(locationCode)) txtLocationNoQuery.Text = locationCode;
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }


        //��ʼ��λ������
        /// <summary>
        /// ��ʼ����λ
        /// </summary>
        private void InitStorageList()
        {
            if (facade == null)
            {
                facade = new InventoryFacade(base.DataProvider);
            }
            this.drpStorageQuery.Items.Add(new ListItem("", ""));
            object[] objStorage = facade.GetAllStorage();
            if (objStorage != null && objStorage.Length > 0)
            {
                foreach (Storage storage in objStorage)
                {

                    this.drpStorageQuery.Items.Add(new ListItem(
                         storage.StorageName,storage.StorageCode)
                        );
                }
            }
            this.drpStorageQuery.SelectedIndex = 0;
        }

        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("LocStorTransNo", "��λ�ƶ�����", null);
            this.gridHelper.AddColumn("InvNo", "SAP���ݺ�", null);
            this.gridHelper.AddColumn("StorageLocation", "��λ", null);
            this.gridHelper.AddColumn("CDate", "��������", null);
            this.gridHelper.AddColumn("CTime", "����ʱ��", null);
            this.gridHelper.AddColumn("CUser", "������", null);
            this.gridHelper.AddLinkColumn("Detail", "��ϸ��Ϣ", null);

            this.gridHelper.AddDefaultColumn(false, false);
            
            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);


            if (!string.IsNullOrEmpty(txtLocationNoQuery.Text))
            {
                this.gridHelper.RequestData();
            }
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();

            row["LocStorTransNo"] = ((Storloctrans)obj).Transno;
            row["InvNo"] = ((Storloctrans)obj).Invno;
            row["StorageLocation"] = ((Storloctrans)obj).StorageCode;
            row["CDate"] = FormatHelper.ToDateString(((Storloctrans)obj).CDate);
            row["CTime"] = FormatHelper.ToTimeString(((Storloctrans)obj).CTime);
            row["CUser"] = ((Storloctrans)obj).CUser;

            return row;

        }

   

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }

            return this._WarehouseFacade.QueryStorloctrans(
           FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtLocationNoQuery.Text)),
           "",
           this.drpStorageQuery.SelectedValue,
           FormatHelper.TODateInt(this.txtCDateFromQuery.Text),
           FormatHelper.TODateInt(this.txtCDateToQuery.Text),"",TransType.TransType_Move,
           inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            return this._WarehouseFacade.QueryStorloctransCount(
                   FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtLocationNoQuery.Text)),
           "",
           this.drpStorageQuery.SelectedValue,
           FormatHelper.TODateInt(this.txtCDateFromQuery.Text),
           FormatHelper.TODateInt(this.txtCDateToQuery.Text),"",TransType.TransType_Move
                  );
        }

        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                                ((Storloctrans)obj).Transno,
                                ((Storloctrans)obj).StorageCode,
                                FormatHelper.ToDateString(((Storloctrans)obj).CDate),
                                FormatHelper.ToTimeString(((Storloctrans)obj).CTime),
                                ((Storloctrans)obj).CUser
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[]
                {
                                    "LocationNo",
                                    "StorageLocation",
                                    "CDate",
                                    "CTime",
                                    "CUser"                                    
                                   
                };
        }

        #endregion

        protected override void gridWebGrid_ItemCommand(GridRecord row, string commandName)
        {
            if (commandName == "Detail")
            {
                string transNo = row.Items.FindItemByKey("LocStorTransNo").Text.Trim();
                Response.Redirect(this.MakeRedirectUrl("FLocationTransDetailMP.aspx", new string[] { "LOCATIONNO" }, new string[] { transNo }));
            }
        }

        protected override void Grid_ClickCell(GridRecord row, string command)
        {
            base.Grid_ClickCell(row, command);
            if (command == "Detail")
            {
                string transNo = row.Items.FindItemByKey("LocStorTransNo").Text.Trim();
                Response.Redirect(this.MakeRedirectUrl("FStorLocTransDetailMP.aspx", new string[] { "LOCATIONNO" }, new string[] { transNo }));
            }
        }
    }
}
