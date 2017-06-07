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
using BenQGuru.eMES.Common;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;



namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FPauseDetailsQuery : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private PauseFacade facade = null;

        //protected GridHelper _gridHelper;

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
           // this._gridHelper = new GridHelper(this.gridWebGrid);
            
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
            this.gridHelper.AddColumn("StorageCodeNew", "���", true);
            this.gridHelper.AddColumn("StorageDisplay", "���", null);
            this.gridHelper.AddColumn("StackCode", "��λ", null);
            this.gridHelper.AddColumn("PalletCode", "ջ��", null);
            this.gridHelper.AddColumn("RcardCount", "��Ʒ����", HorizontalAlign.Right);
            this.gridHelper.AddColumn("PauseCount", "ͣ������", HorizontalAlign.Right);
            this.gridHelper.AddColumn("CancelPauseCount", "���ͣ������", HorizontalAlign.Right);
            this.gridHelper.AddColumn("ItemCode", "��Ʒ����", null);
            this.gridHelper.AddColumn("ItemDescription", "��Ʒ����", null);

            this.gridHelper.AddLinkColumn("DetailsQuery", "��ϸ", null);

            //this.gridHelper.Grid.Bands[0].Columns.FromKey("RcardCount").CellStyle.HorizontalAlign = HorizontalAlign.Right;
            //this.gridHelper.Grid.Bands[0].Columns.FromKey("PauseCount").CellStyle.HorizontalAlign = HorizontalAlign.Right;
            //this.gridHelper.Grid.Bands[0].Columns.FromKey("CancelPauseCount").CellStyle.HorizontalAlign = HorizontalAlign.Right;

            //this.gridHelper.Grid.Bands[0].Columns.FromKey("StorageCodeNew").Hidden = true;

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override DataRow GetGridRow(object obj)
        {
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
            //    new object[]{   ((PauseQuery)obj).StorageCode,
            //                    ((PauseQuery)obj).GetDisplayText("StorageCode"),
            ////                    ((PauseQuery)obj).StackCode.ToString(),
            //                    ((PauseQuery)obj).PalletCode.ToString(),
            //                    ((PauseQuery)obj).RcardCount.ToString(),
            //                    ((PauseQuery)obj).PauseQty.ToString(),
            //                    ((PauseQuery)obj).CancelQty.ToString(),
            //                    ((PauseQuery)obj).MCode.ToString(),            
            //                    ((PauseQuery)obj).MDesc.ToString()});
            DataRow row = this.DtSource.NewRow();
            row["StorageCodeNew"] = ((PauseQuery)obj).StorageCode;
            row["StorageDisplay"] = ((PauseQuery)obj).GetDisplayText("StorageCode");
            row["StackCode"] = ((PauseQuery)obj).StackCode.ToString();
            row["PalletCode"] = ((PauseQuery)obj).PalletCode.ToString();
            row["RcardCount"] = ((PauseQuery)obj).RcardCount.ToString();
            row["PauseCount"] = ((PauseQuery)obj).PauseQty.ToString();
            row["CancelPauseCount"] = ((PauseQuery)obj).CancelQty.ToString();
            row["ItemCode"] = ((PauseQuery)obj).MCode.ToString();
            row["ItemDescription"] = ((PauseQuery)obj).MDesc.ToString();
            return row;

        }


        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new PauseFacade(base.DataProvider);
            }

            string pauseCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPauseCodeQuery.Text));

            if (pauseCode != null && pauseCode.Length > 0)
            {
                object[] obj = facade.QueryLast(pauseCode);

                if (obj != null)
                {
                    Pause pauseQuery = obj[0] as Pause;

                    this.txtPauseReasonEdit.Text = pauseQuery.PauseReason;
                    this.txtPauseUserEdit.Text = pauseQuery.PUser;
                    this.txtPauseDateEdit.Text = pauseQuery.PDate.ToString();
                }
            }
            else
            {
                WebInfoPublish.Publish(this, "$PAUSECODE_CANT_BE_BULL", this.languageComponent1);
                return null;
            }
           

            return this.facade.QueryPauseDetails(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPauseCodeQuery.Text)),
                FormatHelper.CleanString(this.txtCancleSequenceCodeQuery.Text),
                inclusive, exclusive);

        }


        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new PauseFacade(base.DataProvider);
            }
            return this.facade.QueryPauseDetailsCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPauseCodeQuery.Text)),
                FormatHelper.CleanString(this.txtCancleSequenceCodeQuery.Text)
            );

        }

        #endregion

        #region Button

        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            //base.Grid_ClickCell(cell);

            string pauseCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPauseCodeQuery.Text));
            string cancelSeq = FormatHelper.CleanString(this.txtCancleSequenceCodeQuery.Text);
            
           //if (this._gridHelper.IsClickColumn("DetailsQuery", cell))
            if (commandName == "DetailsQuery")
            {
                this.Response.Redirect(this.MakeRedirectUrl("./FPauseSequenceQuery.aspx",
                    new string[] { "storageCode", "stackCode", "palletCode", "itemCode", "pauseCode", "cancelCode" },
                    new string[] { row.Items.FindItemByKey("StorageCodeNew").Text.Trim(),row.Items.FindItemByKey("StackCode").Text.Trim(), row.Items.FindItemByKey("PalletCode").Text.Trim(),
                       row.Items.FindItemByKey("ItemCode").Text.Trim(), pauseCode, cancelSeq }));
            }
        }

        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((PauseQuery)obj).GetDisplayText("StorageCode"),
                                ((PauseQuery)obj).StackCode.ToString(),
                                ((PauseQuery)obj).PalletCode.ToString(),
                                ((PauseQuery)obj).RcardCount.ToString(),
                                ((PauseQuery)obj).PauseQty.ToString(),
                                ((PauseQuery)obj).CancelQty.ToString(),
                                ((PauseQuery)obj).MCode.ToString(),            
                                ((PauseQuery)obj).MDesc.ToString()};
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"StorageCodeNew",
                                    "StackCode",
                                    "PalletCode",	
                                    "RcardCount",
                                    "PauseCount",
                                    "CancelPauseCount",
                                    "ItemCode",	
                                    "ItemDescription"};
        }

        #endregion

    }
}
