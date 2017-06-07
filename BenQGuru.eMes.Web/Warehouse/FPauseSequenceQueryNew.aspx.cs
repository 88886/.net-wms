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
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Domain.Warehouse;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FPauseSequenceQueryNew : BaseMPageNew
    {
        protected System.Web.UI.WebControls.Label lblOperationSelectTitle;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private PauseFacade facade = null;

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

        #region Stable
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this.txtStorageCodeQuery.Text = this.GetRequestParam("storageCode");
                this.txtStackCodeQuery.Text = this.GetRequestParam("stackCode");
                this.txtPalletCodeQuery.Text = this.GetRequestParam("palletCode");
                this.txtItemCodeQuery.Text = this.GetRequestParam("itemCode");
                this.txtPauseCodeQuery.Text = this.GetRequestParam("pauseCode");
                this.txtCancelPauseQuery.Text = this.GetRequestParam("cancelCode");

            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        #endregion

        #region NotStable
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("StorageCodeNew", "���", null);
            this.gridHelper.AddColumn("StackCode", "��λ", null);
            this.gridHelper.AddColumn("PalletCode", "ջ��", null);
            this.gridHelper.AddColumn("CartonCode", "���", null);
            this.gridHelper.AddColumn("RuninngCard", "���к�", null);
            this.gridHelper.AddColumn("MOCode", "������", null);
            this.gridHelper.AddColumn("BOMVersion", "BOM�汾", null);
            this.gridHelper.AddColumn("ItemCode", "��Ʒ�Ϻ�", null);
            this.gridHelper.AddColumn("ItemDescription", "��Ʒ����", null);
            this.gridHelper.AddColumn("CancelPauseUser", "�����Ա", null);
            this.gridHelper.AddColumn("CancelPauseDate", "�������", null);
            this.gridHelper.AddColumn("CancelPauseReason", "���ԭ��", null);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.AddDefaultColumn(false, false);
            base.InitWebGrid();

            this.gridHelper.RequestData();
        }

        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new PauseFacade(this.DataProvider);
            }
            return this.facade.QueryPauseSequenceCount(
                FormatHelper.CleanString(this.txtStorageCodeQuery.Text),
               FormatHelper.CleanString(this.txtStackCodeQuery.Text),
               FormatHelper.CleanString(this.txtPalletCodeQuery.Text),
               FormatHelper.CleanString(this.txtItemCodeQuery.Text),
               FormatHelper.CleanString(this.txtPauseCodeQuery.Text),
               FormatHelper.CleanString(this.txtCancelPauseQuery.Text), false);

        }

        protected override DataRow GetGridRow(object obj)
        {
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
            //    new object[]{   ((PauseQuery)obj).StorageCode.ToString(),
            //                    ((PauseQuery)obj).StackCode.ToString(),
            //                    ((PauseQuery)obj).PalletCode.ToString(),
            //                    ((PauseQuery)obj).CartonCode.ToString(),
            //                    ((PauseQuery)obj).SerialNo.ToString(),
            //                    ((PauseQuery)obj).MOCode.ToString(),
            //                    ((PauseQuery)obj).BOM,
            //                    ((PauseQuery)obj).MCode.ToString(),
            //                    ((PauseQuery)obj).MDesc.ToString(),
            //                    //((PauseQuery)obj).CancelUser.ToString(),
            //                   ((PauseQuery)obj).GetDisplayText("CancelUser"),
            //                    FormatHelper.ToDateString(((PauseQuery)obj).CancelDate),
            //                    ((PauseQuery)obj).CancelReason.ToString()
            //                });

            DataRow row = this.DtSource.NewRow();
            row["StorageCodeNew"] = ((PauseQuery)obj).StorageCode.ToString();
            row["StackCode"] = ((PauseQuery)obj).StackCode.ToString();
            row["PalletCode"] = ((PauseQuery)obj).PalletCode.ToString();
            row["CartonCode"] = ((PauseQuery)obj).CartonCode.ToString();
            row["RuninngCard"] = ((PauseQuery)obj).SerialNo.ToString();
            row["MOCode"] = ((PauseQuery)obj).MOCode.ToString();
            row["BOMVersion"] = ((PauseQuery)obj).BOM;
            row["ItemCode"] = ((PauseQuery)obj).MCode.ToString();
            row["ItemDescription"] = ((PauseQuery)obj).MDesc.ToString();
            row["CancelPauseUser"] = ((PauseQuery)obj).GetDisplayText("CancelUser");
            row["CancelPauseDate"] = FormatHelper.ToDateString(((PauseQuery)obj).CancelDate);
            row["CancelPauseReason"] = ((PauseQuery)obj).CancelReason.ToString();
            return row;


        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new PauseFacade(this.DataProvider);
            }

            return facade.QueryPauseSequence(
               FormatHelper.CleanString(this.txtStorageCodeQuery.Text),
               FormatHelper.CleanString(this.txtStackCodeQuery.Text),
               FormatHelper.CleanString(this.txtPalletCodeQuery.Text),
               FormatHelper.CleanString(this.txtItemCodeQuery.Text),
               FormatHelper.CleanString(this.txtPauseCodeQuery.Text),
               FormatHelper.CleanString(this.txtCancelPauseQuery.Text), false,
               inclusive, exclusive);

        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("./FPauseHistoryQuery.aspx"));
        }

        #endregion

        #region Export
        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((PauseQuery)obj).StorageCode.ToString(),
                                ((PauseQuery)obj).StackCode.ToString(),
                                ((PauseQuery)obj).PalletCode.ToString(),
                                ((PauseQuery)obj).CartonCode.ToString(),
                                ((PauseQuery)obj).SerialNo.ToString(),
                                ((PauseQuery)obj).MOCode.ToString(),
                                ((PauseQuery)obj).BOM,
                                ((PauseQuery)obj).MCode.ToString(),
                                ((PauseQuery)obj).MDesc.ToString(),
                                //((PauseQuery)obj).CancelUser.ToString(),
                                   ((PauseQuery)obj).GetDisplayText("CancelUser"),
                                FormatHelper.ToDateString(((PauseQuery)obj).CancelDate),
                                ((PauseQuery)obj).CancelReason.ToString()};

        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"StorageCodeNew",
									"StackCode",	
									"PalletCode",
                                    "CartonCode",
                                    "RuninngCard",
									"MOCode",	
									"BOMVersion",
                                    "ItemCode",
                                    "ItemDescription",
									"CancelPauseUser",	
									"CancelPauseDate",
                                    "CancelPauseReason"};
        }

        #endregion
    }
}
