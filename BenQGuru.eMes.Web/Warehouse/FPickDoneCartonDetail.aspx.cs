using System;
using System.Data;
using System.Configuration;
using System.Collections;
using BenQGuru.eMES.Common;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Warehouse;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FPickDoneCartonDetail : BaseMPageNew
    {

        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;
        private static string PickNo = string.Empty;
        private static string PickLine = string.Empty;

        private WarehouseFacade facade = null;
        private InventoryFacade _InventoryFacade = null;
        private WarehouseFacade _WarehouseFacade = null;
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
                PickNo = this.GetRequestParam("PickNo");
                PickLine = this.GetRequestParam("PickLine");
                this.txtPickNoQuery.Text = PickNo;
                this.txtPickNoQuery.Enabled = false;
                this.InitPageLanguage(this.languageComponent1, false);
            }
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
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
            this.gridHelper.AddColumn("DQMaterialNO", "�������ϱ���", null);
            this.gridHelper.AddColumn("CusMCode", "�ͻ����ϱ���", null);
            this.gridHelper.AddColumn("BoxNo", "���", null);
            this.gridHelper.AddColumn("Qty", "����", null);
            this.gridHelper.AddColumn("MUOM", "��λ", null);
            this.gridHelper.AddColumn("LocationNO", "��λ��", null);
            this.gridHelper.AddColumn("LotNo", "���κ�", null);
            this.gridHelper.AddColumn("CUser", "������", null);
            this.gridHelper.AddColumn("CDate", "��������", null);
            this.gridHelper.AddColumn("CTime", "����ʱ��", null);
            this.gridHelper.AddColumn("PickNo", "����������", null);
            this.gridHelper.AddColumn("PickLine", "��Ŀ��", null);
            //this.gridHelper.AddEditColumn("SNDetail", "SN����");

            this.gridWebGrid.Columns.FromKey("PickLine").Hidden = true;
            this.gridWebGrid.Columns.FromKey("PickNo").Hidden = true;

            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            this.gridHelper.RequestData();
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["DQMaterialNO"] = ((Pickdetailmaterial)obj).DqmCode;
            row["CusMCode"] = ((Pickdetailmaterial)obj).CustmCode;
            row["BoxNo"] = ((Pickdetailmaterial)obj).Cartonno;
            row["Qty"] = ((Pickdetailmaterial)obj).Qty.ToString();
            row["MUOM"] = ((Pickdetailmaterial)obj).Unit;
            row["LocationNO"] = ((Pickdetailmaterial)obj).LocationCode;
            row["LotNo"] = ((Pickdetailmaterial)obj).Lotno;
            row["CUser"] = ((Pickdetailmaterial)obj).MaintainUser;
            row["CDate"] = FormatHelper.ToDateString(((Pickdetailmaterial)obj).MaintainDate);
            row["CTime"] = FormatHelper.ToTimeString(((Pickdetailmaterial)obj).MaintainTime);
            row["PickNo"] = ((Pickdetailmaterial)obj).Pickno;
            row["PickLine"] = ((Pickdetailmaterial)obj).Pickline;

            return row;
        }

        #region
        protected override object GetEditObject(GridRecord row)
        {

            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(this.DataProvider);
            }
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            object obj = _WarehouseFacade.GetPickdetailmaterial(row.Items.FindItemByKey("PickNo").Text, row.Items.FindItemByKey("BoxNo").Text);
            if (obj != null)
            {
                return obj;
            }

            return null;
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(this.DataProvider);
            }
            Pickdetailmaterial[] pickdetailmaterialList = ((Pickdetailmaterial[])domainObjects.ToArray(typeof(Pickdetailmaterial)));
            try
            {
                this.DataProvider.BeginTransaction();

                Pick pick = _InventoryFacade.GetPick(txtPickNoQuery.Text) as Pick;
                if (pick == null)
                {
                    WebInfoPublish.Publish(this, pick.PickNo + "�����������ڣ�", this.languageComponent1);
                    return;
                }

                foreach (Pickdetailmaterial pickdetailm in pickdetailmaterialList)
                {
                    int packageCount = _WarehouseFacade.GetPackageMaterialCartonnos(pick.PickNo);
                    if (packageCount > 0)
                    {
                        WebInfoPublish.Publish(this, pick.PickNo + "��װ��Ϣ���ڣ� ����ɾ����װ��Ϣ��", this.languageComponent1);
                        return;
                    }
                }
               


                foreach (Pickdetailmaterial pickdetailm in pickdetailmaterialList)
                {
                    #region delete
                    #region delete ���
                    //1��ֻ������ϸ�Ǽ����кͼ������״̬���Ҽ��������ͷ״̬�Ǽ���״̬�������䵥״̬ʱ�ſ���ɾ���� 
                    PickDetail pickDetail = _InventoryFacade.GetPickDetail(pickdetailm.Pickno, pickdetailm.Pickline) as PickDetail;
                    Pick pickHead = _InventoryFacade.GetPick(pickdetailm.Pickno) as Pick;
                    if (pickDetail == null || pickHead == null)
                    {
                        this.DataProvider.RollbackTransaction();
                        return;
                    }
                    if (!(pickDetail.Status == PickDetail_STATUS.Status_Pick ||
                        pickDetail.Status == PickDetail_STATUS.Status_WaitPick ||
                        pickDetail.Status == PickDetail_STATUS.Status_ClosePick || pickDetail.Status == PickDetail_STATUS.Status_Cancel))
                    {
                        this.DataProvider.RollbackTransaction();
                        WebInfoPublish.Publish(this, "����ϸ�Ǽ����кͼ������,ȡ��״̬���ſ���ɾ��", this.languageComponent1);
                        return;
                    }
                    if (!(pickHead.Status == PickHeadStatus.PickHeadStatus_Pick
                        || pickHead.Status == PickHeadStatus.PickHeadStatus_MakePackingList
                        || pickHead.Status == PickHeadStatus.PickHeadStatus_WaitPick || pickHead.Status == PickHeadStatus.PickHeadStatus_Cancel))
                    {
                        this.DataProvider.RollbackTransaction();
                        WebInfoPublish.Publish(this, "���������ͷ״̬�Ǽ���״̬�������䵥��ȡ��״̬ʱ���ſ���ɾ��", this.languageComponent1);
                        return;
                    }



                    //  2��ɾ���Ѽ�����ϸ�󣬸���״̬���Ϊ�����ϡ�
                    _WarehouseFacade.DeletePickdetailmaterial(pickdetailm);
                    //delete pickdetailmsn
                    _WarehouseFacade.DeletePickDetailMaterialSNByCartonNo(pickdetailm.Pickno, pickdetailm.Cartonno);



                    //3�������Ѽ�����ϸɾ���󣬼��������״̬���Ϊ�����ϡ�
                    #endregion

                    StorageDetail storageDetail = _WarehouseFacade.GetStorageDetail(pickdetailm.Cartonno) as StorageDetail;
                    if (storageDetail != null)
                    {
                        storageDetail.FreezeQty -= Convert.ToInt32(pickdetailm.Qty);
                        storageDetail.AvailableQty += Convert.ToInt32(pickdetailm.Qty);
                        _WarehouseFacade.UpdateStorageDetail(storageDetail);
                        _WarehouseFacade.UpdateStorageDetailSnbyCartonNo(pickdetailm.Cartonno, "N");
                    }

                    if (pickDetail.Status != "Cancel")
                        pickDetail.Status = PickDetail_STATUS.Status_Pick;

                    pickDetail.SQTY -= pickdetailm.Qty;
                    int count = _WarehouseFacade.GetPickdetailmaterialCount(pickdetailm.Pickno);
                    if (count == 0)
                    {
                        //pickHead.Status = PickHeadStatus.PickHeadStatus_WaitPick;
                        _WarehouseFacade.UpdatePick(pickHead);
                        //pickDetail.Status = PickDetail_STATUS.Status_WaitPick;
                    }
                    _WarehouseFacade.UpdatePickdetail(pickDetail);

                    #endregion
                }
                this.DataProvider.CommitTransaction();
                WebInfoPublish.Publish(this, "ɾ���ɹ�", this.languageComponent1);
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.PublishInfo(this, ex.Message, this.languageComponent1);
            }



        }
        #endregion


        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }
            return this.facade.QueryPickDetailMaterial(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(PickNo)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(PickLine)),
                FormatHelper.CleanString(txtCartonNoQurey.Text),
                inclusive,
                exclusive

               );
        }
        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }
            return this.facade.QueryPickDetailMaterialCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(PickNo)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(PickLine)),
                  FormatHelper.CleanString(txtCartonNoQurey.Text)
            );
        }

        #endregion

        #region Button

        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            //if (facade == null)
            //{
            //    facade = new WarehouseFacade(base.DataProvider);
            //}
            //string PickNo = row.Items.FindItemByKey("PickNo").Text.Trim();
            //string PickLine = row.Items.FindItemByKey("PickLine").Text.Trim();

            //if (commandName == "SNDetail")
            //{
            //    Response.Redirect(this.MakeRedirectUrl("FPickDoneSNDetail.aspx",
            //                        new string[] { "PickNo", "PickLine" },
            //                        new string[] { PickNo, PickLine }));
            //}
        }
        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.MakeRedirectUrl("FPickDoneMP.aspx",
                                   new string[] { "PickNo" },
                                   new string[] { PickNo }));
        }
        #endregion

        #region Object <--> Page



        #endregion

        #region For Export To Excel

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{

                    ((Pickdetailmaterial)obj).DqmCode,
                    ((Pickdetailmaterial)obj).CustmCode,
                    ((Pickdetailmaterial)obj).Cartonno,
                    ((Pickdetailmaterial)obj).Qty.ToString(),
                    ((Pickdetailmaterial)obj).Unit,
                    ((Pickdetailmaterial)obj).LocationCode,
                    ((Pickdetailmaterial)obj).Lotno,
                    ((Pickdetailmaterial)obj).MaintainUser,
                    FormatHelper.ToDateString(((Pickdetailmaterial)obj).MaintainDate),
                    FormatHelper.ToTimeString(((Pickdetailmaterial)obj).MaintainTime)

                                    
            };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	
                    "DQMaterialNO",
                    "CusMCode", 
                    "BoxNo", 
                    "Qty", 
                    "MUOM",
                    "LocationNO",
                    "LotNo",
                    "CUser",
                    "CDate",
                    "CTime"
           
            };
        }

        #endregion


    }
}
