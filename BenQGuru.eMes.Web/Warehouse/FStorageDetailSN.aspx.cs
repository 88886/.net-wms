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

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FStorageDetailSN : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private InventoryFacade _InventoryFacade = null;
        private SystemSettingFacade _SystemSettingFacade = null;


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
            _SystemSettingFacade = new SystemSettingFacade(this.DataProvider);

            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("MaterialNo", "���ϱ���", null);
            this.gridHelper.AddColumn("DQMCode", "�������ϱ���", null);
            this.gridHelper.AddColumn("MDesc", "��������", null);
            this.gridHelper.AddColumn("StorageCode", "��λ����", null);
            this.gridHelper.AddColumn("StorageName", "��λ����", null);
            this.gridHelper.AddColumn("LocationCode", "��λ����", null);
            this.gridHelper.AddColumn("LocationName", "��λ����", null);
            this.gridHelper.AddColumn("CartonNo", "���", null);
            this.gridHelper.AddColumn("Unit", "��λ", null);
            this.gridHelper.AddColumn("SN", "SN", null);
            this.gridHelper.AddColumn("StorageAgeDate", "��Ч������ʱ��", null);
            this.gridHelper.AddColumn("MaintainUser", "ά����Ա", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddDefaultColumn(false, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            this.gridHelper.RequestData();//ҳ�����ʱ��ʼ��grid
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();

            row["MaterialNo"] = ((StorageDetailExt)obj).MCode;
            row["DQMCode"] = ((StorageDetailExt)obj).DQMCode;
            row["MDesc"] = ((StorageDetailExt)obj).MDesc;
            row["StorageCode"] = ((StorageDetailExt)obj).StorageCode;
            row["StorageName"] = ((StorageDetailExt)obj).StorageName;
            row["LocationCode"] = ((StorageDetailExt)obj).LocationCode;
            row["LocationName"] = ((StorageDetailExt)obj).LocationName;
            row["CartonNo"] = ((StorageDetailExt)obj).CartonNo;
            row["Unit"] = ((StorageDetailExt)obj).Unit;
            row["SN"] = ((StorageDetailExt)obj).SN;
            row["StorageAgeDate"] = ((StorageDetailExt)obj).StorageAgeDate;
            row["MaintainUser"] = ((StorageDetailExt)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((StorageDetailExt)obj).MaintainDate);
            return row;

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }

            return this._InventoryFacade.QueryStorageDetailSN(
                FormatHelper.CleanString(Request.QueryString["MCode"]),
                FormatHelper.CleanString(Request.QueryString["DQMCode"]),
                FormatHelper.CleanString(Request.QueryString["StorageCode"]),
                FormatHelper.CleanString(Request.QueryString["LocationCode"]),
                FormatHelper.PKCapitalFormat(Request.QueryString["CartonNo"]),
                FormatHelper.CleanString(Request.QueryString["SN"]),
                inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            return this._InventoryFacade.QueryStorageDetailSNCount(
                FormatHelper.CleanString(Request.QueryString["MCode"]),
                FormatHelper.CleanString(Request.QueryString["DQMCode"]),
                FormatHelper.CleanString(Request.QueryString["StorageCode"]),
                FormatHelper.CleanString(Request.QueryString["LocationCode"]),
                FormatHelper.PKCapitalFormat(Request.QueryString["CartonNo"]),
                FormatHelper.CleanString(Request.QueryString["SN"]));
        }

        #endregion

        #region Button

        protected void cmdReturn_ServerClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("FStorageDetailMP.aspx"));
        }

        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((StorageDetailExt)obj).MCode,
                                ((StorageDetailExt)obj).DQMCode,
                                ((StorageDetailExt)obj).MDesc,
                                ((StorageDetailExt)obj).StorageCode,
                                ((StorageDetailExt)obj).StorageName,
                                ((StorageDetailExt)obj).LocationCode,
                                ((StorageDetailExt)obj).LocationName,
                                ((StorageDetailExt)obj).CartonNo,
                                ((StorageDetailExt)obj).Unit,
                                ((StorageDetailExt)obj).SN,
                                ((StorageDetailExt)obj).StorageAgeDate.ToString(),
                                ((StorageDetailExt)obj).GetDisplayText("MaintainUser"),
                                FormatHelper.ToDateString(((StorageDetailExt)obj).MaintainDate)};
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"MaterialNo",
                                    "DQMCode",
                                    "MDesc",
                                    "StorageCode",
                                    "StorageName",
                                    "LocationCode",	
                                    "LocationName",
                                    "CartonNo",	
                                    "Unit",
                                    "SN",	
                                    "StorageAgeDate",	
                                    "MaintainUser",
                                    "MaintainDate"};
        }

        #endregion

    }
}
