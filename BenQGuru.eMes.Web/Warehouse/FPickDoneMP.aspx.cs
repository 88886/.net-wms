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
    public partial class FPickDoneMP : BaseMPageNew
    {

        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;
        private static string poStno = string.Empty;
        private static string poStline = string.Empty;
        private static string PickNo = string.Empty;
        //private static decimal editQTY = 0;  //��¼�༭ǰ���༭�е�ԭ������
        private WarehouseFacade facade = null;
        private InventoryFacade _Invenfacade = null;
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
                if (!string.IsNullOrEmpty(PickNo))
                    this.txtPickNoQuery.Text = PickNo;
                this.InitPageLanguage(this.languageComponent1, false);
                LoadDatabaseList();
            }
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }

        }

        private void LoadDatabaseList()
        {
            this.rdoSelectType.Items.Clear();
            this.rdoSelectType.Items.Add(new ListItem("����", "AllCarton"));
            this.rdoSelectType.Items.Add(new ListItem("����", "SplitCarton"));
            rdoSelectType.SelectedIndex = 0;
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

            this.gridHelper.AddColumn("MCONTROLTYPE", "�ܿ�����", null);
            this.gridHelper.AddColumn("Status", "״̬", null);
            this.gridHelper.AddColumn("InvNo", "SAP���ݺ�", null);
            this.gridHelper.AddColumn("CusMCode", "�ͻ����ϱ���", null);
            this.gridHelper.AddColumn("RequireQty", "��������", null);
            this.gridHelper.AddColumn("PickedQTY", "�Ѽ�����", null);
            this.gridHelper.AddColumn("MUOM", "��λ", null);
            this.gridHelper.AddColumn("sumBoxNo", "����", null);

            this.gridHelper.AddColumn("OweQTY", "Ƿ������", null);
            //this.gridHelper.AddColumn("BoxNo", "���", null);
            //this.gridHelper.AddColumn("LotNo", "���κ�", null);
            //this.gridHelper.AddColumn("LocationNO", "��λ��", null);
            //this.gridHelper.AddColumn("SN", "SN", null);
            this.gridHelper.AddColumn("MUser", "ά����", null);
            this.gridHelper.AddColumn("MDate", "ά������", null);
            this.gridHelper.AddColumn("MTime", "ά��ʱ��", null);
            this.gridHelper.AddColumn("PickNo", "����������", null);
            this.gridHelper.AddColumn("PickLine", "��Ŀ��", null);
            this.gridHelper.AddEditColumn("CartonNoDetail", "�������");
            this.gridHelper.AddEditColumn("SNDetail", "SN����");

            this.gridWebGrid.Columns.FromKey("PickLine").Hidden = true;
            this.gridWebGrid.Columns.FromKey("PickNo").Hidden = true;

            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            if (!string.IsNullOrEmpty(PickNo))
            {
                this.gridHelper.RequestData();
            }
        }

        protected override DataRow GetGridRow(object obj)
        {

            DataRow row = this.DtSource.NewRow();
            string dqmCode = ((PickDetailEx)obj).DQMCode;
            row["DQMaterialNO"] = dqmCode;

            facade = new WarehouseFacade(base.DataProvider);

            BenQGuru.eMES.Domain.MOModel.Material m = facade.GetMaterialFromDQMCode(dqmCode);

            PickDetailEx PikDetail = (PickDetailEx)obj;
            Pick pick = (Pick)facade.GetPick(PikDetail.PickNo);

            row["MCONTROLTYPE"] = languageComponent1.GetString(m.MCONTROLTYPE);
            row["Status"] = languageComponent1.GetString(((PickDetailEx)obj).Status);
            row["InvNo"] = ((PickDetailEx)obj).InvNo;
            row["CusMCode"] = pick.PickType == PickType.PickType_UB ? PikDetail.CustMCode : PikDetail.VEnderItemCode;
            row["RequireQty"] = ((PickDetailEx)obj).QTY.ToString();
            row["PickedQTY"] = ((PickDetailEx)obj).SQTY.ToString();
            row["MUOM"] = ((PickDetailEx)obj).Unit;
            row["sumBoxNo"] = ((PickDetailEx)obj).sumBox;
            row["OweQTY"] = ((PickDetailEx)obj).OweQTY;
            row["MUser"] = ((PickDetailEx)obj).MaintainUser;
            row["MDate"] = FormatHelper.ToDateString(((PickDetailEx)obj).MaintainDate);
            row["MTime"] = FormatHelper.ToTimeString(((PickDetailEx)obj).MaintainTime);
            row["PickNo"] = ((PickDetailEx)obj).PickNo;
            row["PickLine"] = ((PickDetailEx)obj).PickLine;

            return row;
        }


        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {

            if (string.IsNullOrEmpty(this.txtPickNoQuery.Text))
            {
                WebInfoPublish.Publish(this, "�������������ļ���������", this.languageComponent1);
                return null;
            }

            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }
            //��ѯδ�·��ļ��������ţ�Ӧ��ʾ�ü��������δ�·���
            string pickno = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text));
            Pick pick = (Pick)facade.GetPick(pickno);
            if (pick != null)
            {
                if (pick.Status == Pick_STATUS.Status_Release)
                {
                    WebInfoPublish.Publish(this, "�ü��������δ�·�", this.languageComponent1);
                    return null;
                }
            }
            return this.facade.QueryPickDetail(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text)),
                FormatHelper.CleanString(this.txtInvNoQuery.Text),
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
            return this.facade.QueryPickDetailCount(
               FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text)),
                FormatHelper.CleanString(this.txtInvNoQuery.Text)
            );
        }

        #endregion

        #region Button

        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }
            string PickNo = row.Items.FindItemByKey("PickNo").Text.Trim();
            string PickLine = row.Items.FindItemByKey("PickLine").Text.Trim();

            if (commandName == "CartonNoDetail")
            {
                Response.Redirect(this.MakeRedirectUrl("FPickDoneCartonDetail.aspx",
                                    new string[] { "PickNo", "PickLine" },
                                    new string[] { PickNo, PickLine }));
            }
            else if (commandName == "SNDetail")
            {
                Response.Redirect(this.MakeRedirectUrl("FPickDoneSNDetail.aspx",
                                    new string[] { "PickNo", "PickLine" },
                                    new string[] { PickNo, PickLine }));
            }
        }
        //ɾ��
        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {

        }

        protected override void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //�ж�������Ƿ�������
                if (!string.IsNullOrEmpty(this.txtNumberEdite.Text))
                {


                    PageCheckManager manager = new PageCheckManager();
                    manager.Add(new NumberCheck(this.lblNumberEdite, this.txtNumberEdite, false));
                    if (!manager.Check())
                    {
                        WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                        return;
                    }
                }
                DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                if (this.rdoSelectType.SelectedValue == "AllCarton")   //  ѡ������
                {
                    if (string.IsNullOrEmpty(this.txtCartonNoEdite.Text))
                    {
                        WebInfoPublish.Publish(this, "���������Ų���Ϊ��", this.languageComponent1);
                        return;
                    }

                    //����
                    if (!string.IsNullOrEmpty(txtNumberEdite.Text))
                    {
                        WebInfoPublish.Publish(this, "����ʱ����Ҫ����������SN", this.languageComponent1);
                        return;
                    }
                    if (!string.IsNullOrEmpty(txtSNEdite.Text))
                    {
                        WebInfoPublish.Publish(this, "����ʱ����Ҫ����������SN", this.languageComponent1);
                        return;
                    }

                    if (!CheckStorageCode(string.Empty, true))
                    {
                        return;
                    }
                    if (facade == null)
                    {
                        facade = new WarehouseFacade(base.DataProvider);
                    }
                    if (_Invenfacade == null)
                    {
                        _Invenfacade = new InventoryFacade(base.DataProvider);
                    }




                    this.DataProvider.BeginTransaction();
                    try
                    {

                        object obj = _Invenfacade.GetStorageDetail(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdite.Text)));
                        if (obj == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�����û�и����", this.languageComponent1);
                            return;
                        }
                        StorageDetail sto = obj as StorageDetail;
                        object[] pickline_obj = facade.GetPickLine(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text)), sto.DQMCode);
                        if (pickline_obj == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�����������û�ж������Ϻţ�" + sto.DQMCode, this.languageComponent1);
                            return;
                        }
                        #region  ���±� pickdetail
                        PickDetail pikd = pickline_obj[0] as PickDetail;
                        pikd.SQTY += sto.AvailableQty;
                        pikd.MaintainDate = dbTime.DBDate;
                        pikd.MaintainTime = dbTime.DBTime;
                        pikd.MaintainUser = this.GetUserCode();
                        object obj_pik = _Invenfacade.GetPick(pikd.PickNo);
                        if (pikd.SQTY == pikd.QTY)
                        {
                            pikd.Status = PickDetail_STATUS.Status_ClosePick;
                         
                            InvInOutTrans trans = facade.CreateNewInvInOutTrans();
                            trans.CartonNO = string.Empty;
                            trans.DqMCode = pikd.DQMCode;
                            trans.FacCode = string.Empty;
                            trans.FromFacCode = string.Empty;
                            trans.FromStorageCode = string.Empty;
                            trans.InvNO = (obj_pik as Pick).InvNo;//.InvNo;
                            trans.InvType = (obj_pik as Pick).PickType;
                            trans.LotNo = string.Empty;
                            trans.MaintainDate = dbTime.DBDate;
                            trans.MaintainTime = dbTime.DBTime;
                            trans.MaintainUser = GetUserCode();
                            trans.MCode = string.Empty;
                            trans.ProductionDate = 0;
                            trans.Qty = 0;
                            trans.Serial = 0;
                            trans.StorageAgeDate = 0;
                            trans.StorageCode = (obj_pik as Pick).StorageCode;
                            trans.SupplierLotNo = string.Empty;
                            trans.TransNO = (obj_pik as Pick).PickNo;
                            trans.TransType = "OUT";
                            trans.Unit = string.Empty;
                            trans.ProcessType = "ClosePick";
                            facade.AddInvInOutTrans(trans);
                        }
                        else
                        {
                            pikd.Status = PickDetail_STATUS.Status_Pick;
                        }
                        facade.UpdatePickdetail(pikd);
                        #endregion
                        #region
                     
                        if (obj_pik == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "���¼��������ͷ����", this.languageComponent1);
                            return;
                        }
                        else
                        {
                            Pick pi = obj_pik as Pick;
                            if (pi.Status == Pick_STATUS.Status_WaitPick)
                            {
                                pi.Status = Pick_STATUS.Status_Pick;
                            }

                            if (pi.Status == Pick_STATUS.Status_WaitPick)
                            {
                                pi.Status = Pick_STATUS.Status_Pick;

                                InvInOutTrans trans = facade.CreateNewInvInOutTrans();
                                trans.CartonNO = string.Empty;
                                trans.DqMCode = sto.DQMCode;
                                trans.FacCode = string.Empty;
                                trans.FromFacCode = string.Empty;
                                trans.FromStorageCode = string.Empty;
                                trans.InvNO = (obj_pik as Pick).InvNo;//.InvNo;
                                trans.InvType = (obj_pik as Pick).PickType;
                                trans.LotNo = string.Empty;
                                trans.MaintainDate = dbTime.DBDate;
                                trans.MaintainTime = dbTime.DBTime;
                                trans.MaintainUser = GetUserCode();
                                trans.MCode = sto.MCode;
                                trans.ProductionDate = 0;
                                trans.Qty = sto.AvailableQty;
                                trans.Serial = 0;
                                trans.StorageAgeDate = 0;
                                trans.StorageCode = (obj_pik as Pick).StorageCode;
                                trans.SupplierLotNo = string.Empty;
                                trans.TransNO = (obj_pik as Pick).PickNo;
                                trans.TransType = "OUT";
                                trans.Unit = string.Empty;
                                trans.ProcessType = "PICK";
                                facade.AddInvInOutTrans(trans);


                                pi.MaintainDate = dbTime.DBDate;
                                pi.MaintainTime = dbTime.DBTime;
                                pi.MaintainUser = GetUserCode();
                                _Invenfacade.UpdatePick(pi);
                            }

                            pi.MaintainDate = dbTime.DBDate;
                            pi.MaintainTime = dbTime.DBTime;
                            pi.MaintainUser = this.GetUserCode();

                            _Invenfacade.UpdatePick(pi);
                        }
                        #endregion
                        #region  ����һ�����ݵ�tblPickDetailMaterial
                        Pickdetailmaterial pikm = facade.CreateNewPickdetailmaterial();
                        pikm.Cartonno = sto.CartonNo;
                        pikm.CDate = dbTime.DBDate;
                        pikm.CTime = dbTime.DBTime;
                        pikm.CUser = this.GetUserCode();
                        pikm.CustmCode = pikd.CustMCode;
                        pikm.DqmCode = pikd.DQMCode;
                        pikm.LocationCode = sto.LocationCode;
                        pikm.Lotno = sto.Lotno;
                        pikm.MaintainDate = dbTime.DBDate;
                        pikm.MaintainTime = dbTime.DBTime;
                        pikm.MaintainUser = this.GetUserCode();
                        pikm.MCode = pikd.MCode;
                        pikm.Pickline = pikd.PickLine;
                        pikm.Pickno = pikd.PickNo;
                        pikm.Production_Date = sto.ProductionDate;
                        //pikm.QcStatus = string.Empty;
                        pikm.Qty = sto.AvailableQty;
                        //pikm.Status = string.Empty;   ////xu yao xiu gai 
                        pikm.StorageageDate = sto.LastStorageAgeDate;
                        pikm.Supplier_lotno = sto.SupplierLotNo;
                        pikm.Unit = sto.Unit;
                        pikm.PQty = 0;

                        facade.AddPickdetailmaterial(pikm);
                        #endregion

                        #region  ����ǵ����ܿأ�����һ�����ݵ�tblPickDetailMaterialSN
                        MOModel.ItemFacade itemFacade = new MOModel.ItemFacade(base.DataProvider);
                        object material_obj = itemFacade.GetMaterial(sto.MCode);
                        if ((material_obj as Domain.MOModel.Material).MCONTROLTYPE == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                        {
                            object[] stosn = facade.GetStorageDetailSnbyCartonNo(sto.CartonNo);
                            if (stosn.Length != sto.AvailableQty)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "��ţ�" + sto.CartonNo + "��SN���������ڿ���������", this.languageComponent1);
                                return;
                            }

                            for (int i = 0; i < stosn.Length; i++)
                            {
                                Pickdetailmaterialsn piksn = facade.CreateNewPickdetailmaterialsn();
                                StorageDetailSN stsn = stosn[i] as StorageDetailSN;
                                piksn.Cartonno = stsn.CartonNo;
                                piksn.MaintainDate = dbTime.DBDate;
                                piksn.MaintainTime = dbTime.DBTime;
                                piksn.MaintainUser = this.GetUserCode();
                                piksn.Pickline = pikd.PickLine;
                                piksn.Pickno = pikd.PickNo;
                                piksn.QcStatus = string.Empty;
                                piksn.Sn = stsn.SN;
                                facade.AddPickdetailmaterialsn(piksn);
                                #region   ����״̬
                                stsn.PickBlock = "Y";
                                stsn.MaintainDate = dbTime.DBDate;
                                stsn.MaintainTime = dbTime.DBTime;
                                stsn.MaintainUser = this.GetUserCode();
                                _Invenfacade.UpdateStorageDetailSN(stsn);
                                #endregion
                            }
                        }
                        #endregion
                        #region  ���¿���������
                        sto.FreezeQty += sto.AvailableQty;
                        sto.AvailableQty = 0;
                        sto.MaintainDate = dbTime.DBDate;
                        sto.MaintainTime = dbTime.DBTime;
                        sto.MaintainUser = this.GetUserCode();
                        _Invenfacade.UpdateStorageDetail(sto);
                        #endregion
                        
                    }
                    catch (Exception ex)
                    {
                        this.DataProvider.RollbackTransaction();
                        WebInfoPublish.Publish(this, "���ϳ���" + ex, this.languageComponent1);
                        return;
                    }
                }
                else //ѡ�����
                {
                    if (facade == null)
                    {
                        facade = new WarehouseFacade(base.DataProvider);
                    }
                    if (_Invenfacade == null)
                    {
                        _Invenfacade = new InventoryFacade(base.DataProvider);
                    }
                    #region ����߼�
                    if (string.IsNullOrEmpty(this.txtSNEdite.Text) && string.IsNullOrEmpty(this.txtCartonNoEdite.Text))
                    {
                        WebInfoPublish.Publish(this, "��Ҫ����Ż�SN��Ϣ", this.languageComponent1);
                        return;
                    }
                    #region �ж��ǵ����ܿػ������ܿ�
                    string ControlType = string.Empty;
                    string CartonCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdite.Text));
                    if (!string.IsNullOrEmpty(this.txtCartonNoEdite.Text))  //�����
                    {

                        object obj = _Invenfacade.GetStorageDetail(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdite.Text)));
                        if (obj == null)
                        {
                            WebInfoPublish.Publish(this, "�����û�и������Ϣ", this.languageComponent1);
                            return;
                        }
                        else
                        {
                            StorageDetail sto = obj as StorageDetail;
                            MOModel.ItemFacade itemFacade = new MOModel.ItemFacade(base.DataProvider);
                            //object material_obj
                            Domain.MOModel.Material matr = itemFacade.GetMaterial(sto.MCode) as Domain.MOModel.Material;
                            if (matr == null)
                            {
                                WebInfoPublish.Publish(this, "����Ŷ�Ӧ��������Ϣ�����ϱ��в�����", this.languageComponent1);
                                return;
                            }
                            else
                            {
                                //Domain.MOModel.Material matr = material_obj as Domain.MOModel.Material;
                                ControlType = matr.MCONTROLTYPE;
                                if (matr.MCONTROLTYPE == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                                {
                                    #region �����ܿ�
                                    if (!string.IsNullOrEmpty(this.txtNumberEdite.Text))
                                    {
                                        WebInfoPublish.Publish(this, "�����ܿ��ϲ���Ҫ�����������м���", this.languageComponent1);
                                        return;
                                    }

                                    if (string.IsNullOrEmpty(this.txtSNEdite.Text))
                                    {
                                        WebInfoPublish.Publish(this, "�����ܿ��ϱ�������SN���м���", this.languageComponent1);
                                        return;
                                    }
                                    else
                                    {
                                        object obj1 = _Invenfacade.GetStorageDetailSN(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNEdite.Text)));
                                        if (obj1 == null)
                                        {
                                            WebInfoPublish.Publish(this, "�����û��SN����", this.languageComponent1);
                                            return;
                                        }
                                        else
                                        {
                                            object obj2 = _Invenfacade.GetStorageDetailSN(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNEdite.Text)));
                                            if (obj2 == null)
                                            {
                                                WebInfoPublish.Publish(this, "��Ż�SN����û��SN��Ϣ", this.languageComponent1);
                                                return;
                                            }
                                            else
                                            {
                                                string car = (_Invenfacade.GetStorageDetailSN(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNEdite.Text))) as StorageDetailSN).CartonNo;
                                                if (CartonCode != car)
                                                {
                                                    WebInfoPublish.Publish(this, "����������SN�Ų�ƥ�䣬�����ܿ��Ͽ���ֻ����SN", this.languageComponent1);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }

                    }
                    else  // û���������
                    {
                        if (!string.IsNullOrEmpty(this.txtSNEdite.Text))
                        {
                            #region û���������
                            if (!string.IsNullOrEmpty(this.txtNumberEdite.Text))
                            {
                                WebInfoPublish.Publish(this, "�����ܿ��ϲ���Ҫ�����������м���", this.languageComponent1);
                                return;
                            }
                            object obj = _Invenfacade.GetStorageDetailSN(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNEdite.Text)));
                            if (obj == null)
                            {
                                WebInfoPublish.Publish(this, "�����û��SN����", this.languageComponent1);
                                return;
                            }
                            else
                            {
                                object obj2 = _Invenfacade.GetStorageDetailSN(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNEdite.Text)));
                                if (obj2 == null)
                                {
                                    WebInfoPublish.Publish(this, "��SN��Ϣ", this.languageComponent1);
                                    return;
                                }
                                else
                                {
                                    CartonCode = (obj2 as StorageDetailSN).CartonNo;

                                    object obj3 = _Invenfacade.GetStorageDetail(CartonCode);
                                    if (obj3 == null)
                                    {
                                        WebInfoPublish.Publish(this, "�����û�и������Ϣ", this.languageComponent1);
                                        return;
                                    }
                                    else
                                    {
                                        StorageDetail sto = obj3 as StorageDetail;
                                        MOModel.ItemFacade itemFacade = new MOModel.ItemFacade(base.DataProvider);
                                        object material_obj = itemFacade.GetMaterial(sto.MCode);
                                        if (material_obj == null)
                                        {
                                            WebInfoPublish.Publish(this, "����Ŷ�Ӧ��������Ϣ�����ϱ��в�����", this.languageComponent1);
                                            return;
                                        }
                                        else
                                        {
                                            Domain.MOModel.Material matr = material_obj as Domain.MOModel.Material;
                                            ControlType = matr.MCONTROLTYPE;
                                            if (matr.MCONTROLTYPE != SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                                            {
                                                WebInfoPublish.Publish(this, "SN�ϺŲ��ǵ����ܿ��ϣ�����������", this.languageComponent1);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    if (!CheckStorageCode(CartonCode, false))
                    {
                        return;
                    }
                    #endregion
                    try
                    {
                        this.DataProvider.BeginTransaction();
                        //DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                        object sto_obj = null;
                        object[] pickline_obj = null;
                        StorageDetail stor = null;
                        if (string.IsNullOrEmpty(CartonCode))
                        {
                            CartonCode = (_Invenfacade.GetStorageDetailSN(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNEdite.Text))) as StorageDetailSN).CartonNo;
                        }
                        if (ControlType == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                        {
                            //�����ܿ�
                            if (string.IsNullOrEmpty(txtSNEdite.Text))
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "������Ϊ�������ϣ�������SN", this.languageComponent1);
                                return;
                            }
                            if (!string.IsNullOrEmpty(this.txtNumberEdite.Text))
                            {
                                WebInfoPublish.Publish(this, "�����ܿ��ϲ���Ҫ�����������м���", this.languageComponent1);
                                return;
                            }
                            object sto_sn = _Invenfacade.GetStorageDetailSN(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNEdite.Text)));
                            if (sto_sn == null)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "�����û�и����к�", this.languageComponent1);
                                return;
                            }
                            CartonCode = (sto_sn as StorageDetailSN).CartonNo;
                        }
                        else
                        {
                            //�ǵ����ܿ�
                            if (string.IsNullOrEmpty(this.txtNumberEdite.Text))
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "������Ϊ�ǵ������ϣ�����������", this.languageComponent1);
                                return;
                            }
                            if (!string.IsNullOrEmpty(this.txtSNEdite.Text))
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "������Ϊ�ǵ������ϣ�����ҪSN", this.languageComponent1);
                                return;
                            }
                        }

                        sto_obj = _Invenfacade.GetStorageDetail(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(CartonCode)));
                        if (sto_obj == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�����û�и����", this.languageComponent1);
                            return;
                        }
                        stor = sto_obj as StorageDetail;
                        pickline_obj = facade.GetPickLine(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text)), stor.DQMCode);
                        if (pickline_obj == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�����������û�ж������Ϻţ�" + stor.DQMCode, this.languageComponent1);
                            return;
                        }


                        #region  ���±� pickdetail
                        PickDetail pikd = pickline_obj[0] as PickDetail;
                        if (ControlType == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                        {
                            pikd.SQTY += 1;
                        }
                        else
                        {
                            pikd.SQTY += int.Parse(this.txtNumberEdite.Text.Trim());
                        }
                        pikd.MaintainDate = dbTime.DBDate;
                        pikd.MaintainTime = dbTime.DBTime;
                        pikd.MaintainUser = this.GetUserCode();
                        object obj_pik = _Invenfacade.GetPick(pikd.PickNo);
                        if (pikd.SQTY == pikd.QTY)
                        {
                            pikd.Status = PickDetail_STATUS.Status_ClosePick;

                            InvInOutTrans trans = facade.CreateNewInvInOutTrans();
                            trans.CartonNO = string.Empty;
                            trans.DqMCode = string.Empty;
                            trans.FacCode = string.Empty;
                            trans.FromFacCode = string.Empty;
                            trans.FromStorageCode = string.Empty;
                            trans.InvNO = (obj_pik as Pick).InvNo;//.InvNo;
                            trans.InvType = (obj_pik as Pick).PickType;
                            trans.LotNo = string.Empty;
                            trans.MaintainDate = dbTime.DBDate;
                            trans.MaintainTime = dbTime.DBTime;
                            trans.MaintainUser = GetUserCode();
                            trans.MCode = string.Empty;
                            trans.ProductionDate = 0;
                            trans.Qty = 0;
                            trans.Serial = 0;
                            trans.StorageAgeDate = 0;
                            trans.StorageCode = (obj_pik as Pick).StorageCode;
                            trans.SupplierLotNo = string.Empty;
                            trans.TransNO = (obj_pik as Pick).PickNo;
                            trans.TransType = "OUT";
                            trans.Unit = string.Empty;
                            trans.ProcessType = "ClosePick";
                            facade.AddInvInOutTrans(trans);

                        }
                        else
                        {
                            pikd.Status = PickDetail_STATUS.Status_Pick;
                        }
                        facade.UpdatePickdetail(pikd);
                        #endregion
                        #region
                        
                        if (obj_pik == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "���¼��������ͷ����", this.languageComponent1);
                            return;
                        }
                        else
                        {
                            Pick pi = obj_pik as Pick;
                            if (pi.Status == Pick_STATUS.Status_WaitPick)
                            {
                                pi.Status = Pick_STATUS.Status_Pick;
                            }
                            pi.MaintainDate = dbTime.DBDate;
                            pi.MaintainTime = dbTime.DBTime;
                            pi.MaintainUser = this.GetUserCode();

                            _Invenfacade.UpdatePick(pi);
                        }
                        #endregion
                        #region  ����һ�����ݵ�tblPickDetailMaterial

                        object find_obj = facade.GetPickdetailmaterial(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text)), CartonCode);
                        if (find_obj == null)
                        {

                            Pickdetailmaterial pikm = facade.CreateNewPickdetailmaterial();
                            pikm.Cartonno = stor.CartonNo;
                            pikm.CDate = dbTime.DBDate;
                            pikm.CTime = dbTime.DBTime;
                            pikm.CUser = this.GetUserCode();
                            pikm.CustmCode = pikd.CustMCode;
                            pikm.DqmCode = pikd.DQMCode;
                            pikm.LocationCode = stor.LocationCode;
                            pikm.Lotno = stor.Lotno;
                            pikm.MaintainDate = dbTime.DBDate;
                            pikm.MaintainTime = dbTime.DBTime;
                            pikm.MaintainUser = this.GetUserCode();
                            pikm.MCode = pikd.MCode;
                            pikm.Pickline = pikd.PickLine;
                            pikm.Pickno = pikd.PickNo;
                            pikm.Production_Date = stor.ProductionDate;
                            //pikm.QcStatus = string.Empty;
                            if (ControlType == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                            {
                                pikm.Qty = 1;
                            }
                            else
                            {
                                pikm.Qty = Int32.Parse(this.txtNumberEdite.Text);
                            }
                            //pikm.Status = string.Empty;   ////xu yao xiu gai 
                            pikm.StorageageDate = stor.LastStorageAgeDate;
                            pikm.Supplier_lotno = stor.SupplierLotNo;
                            pikm.Unit = stor.Unit;
                            pikm.PQty = 0;
                            facade.AddPickdetailmaterial(pikm);
                        }
                        else
                        {
                            Pickdetailmaterial pikm = find_obj as Pickdetailmaterial;
                            if (ControlType == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                            {
                                pikm.Qty += 1;
                            }
                            else
                            {
                                pikm.Qty += Int32.Parse(this.txtNumberEdite.Text);
                            }
                            pikm.MaintainDate = dbTime.DBDate;
                            pikm.MaintainTime = dbTime.DBTime;
                            pikm.MaintainUser = this.GetUserCode();
                            facade.UpdatePickdetailmaterial(pikm);
                        }


                        #endregion

                        #region  ����ǵ����ܿأ�����һ�����ݵ�tblPickDetailMaterialSN
                        if (ControlType == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                        {

                            object stosn = _Invenfacade.GetStorageDetailSN(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNEdite.Text)));
                            if (stosn == null)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "SN��" + this.txtSNEdite.Text + "������", this.languageComponent1);
                                return;
                            }
                            if ((stosn as StorageDetailSN).PickBlock == "Y")
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "SN��" + this.txtSNEdite.Text + "�Ѿ�������", this.languageComponent1);
                                return;
                            }

                            Pickdetailmaterialsn piksn = facade.CreateNewPickdetailmaterialsn();
                            StorageDetailSN stsn = stosn as StorageDetailSN;
                            piksn.Cartonno = stsn.CartonNo;
                            piksn.MaintainDate = dbTime.DBDate;
                            piksn.MaintainTime = dbTime.DBTime;
                            piksn.MaintainUser = this.GetUserCode();
                            piksn.Pickline = pikd.PickLine;
                            piksn.Pickno = pikd.PickNo;
                            piksn.QcStatus = string.Empty;
                            piksn.Sn = stsn.SN;
                            facade.AddPickdetailmaterialsn(piksn);

                            #region   ����״̬
                            stsn.PickBlock = "Y";
                            stsn.MaintainDate = dbTime.DBDate;
                            stsn.MaintainTime = dbTime.DBTime;
                            stsn.MaintainUser = this.GetUserCode();
                            _Invenfacade.UpdateStorageDetailSN(stsn);
                            #endregion

                        }
                        #endregion
                        #region  ���¿���������
                        if (ControlType == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                        {
                            stor.FreezeQty += 1;
                            // stor.AvailableQty =stor.StorageQty-stor.FreezeQty;
                        }
                        else
                        {
                            stor.FreezeQty += int.Parse(this.txtNumberEdite.Text.Trim());
                            // stor.AvailableQty = stor.StorageQty-stor.FreezeQty;
                        }
                        stor.AvailableQty = stor.StorageQty - stor.FreezeQty;

                        stor.MaintainDate = dbTime.DBDate;
                        stor.MaintainTime = dbTime.DBTime;
                        stor.MaintainUser = this.GetUserCode();
                        _Invenfacade.UpdateStorageDetail(stor);
                        if (stor.AvailableQty < 0)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�����Ŀ�����������Ϊ����", this.languageComponent1);
                        }
                        #endregion


                    }
                    catch (Exception ex)
                    {
                        this.DataProvider.RollbackTransaction();
                        WebInfoPublish.Publish(this, "���ϳ���" + ex, this.languageComponent1);
                        return;
                    }
                }
                #region ���������䵥  --���¼��������ͷ
                CARTONINVOICES CartonH = facade.CreateNewCartoninvoices();
                string pickno = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text));
                object[] pickObj = facade.GetCartoninvoicesByPickNo(pickno);
                if (pickObj == null)
                {
                    #region ���������䵥
                    //edit by sam
                    object[] pikdetail_obj = facade.GetAllLineByPickNoNotCancel(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text)));
                    if (pikdetail_obj == null)
                    {
                        //
                        object obj_pik = _Invenfacade.GetPick(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text)));
                        if (obj_pik == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "������������", this.languageComponent1);
                            return;
                        }
                        else
                        {
                            Pick pi = obj_pik as Pick;
                            pi.Status = Pick_STATUS.Status_MakePackingList;
                            pi.FinishDate = dbTime.DBDate;
                            pi.FinishTime = dbTime.DBTime;
                            pi.MaintainDate = dbTime.DBDate;
                            pi.MaintainTime = dbTime.DBTime;
                            pi.MaintainUser = this.GetUserCode();

                            _Invenfacade.UpdatePick(pi);
                        }
                        //
                        object objLot = null;
                        objLot = facade.GetNewLotNO("K", dbTime.DBDate.ToString().Substring(2, 6).ToString());
                        Serialbook serbook = facade.CreateNewSerialbook();
                        if (objLot == null)
                        {
                            CartonH.CARINVNO = "K" + dbTime.DBDate.ToString().Substring(2, 6).ToString() + "001";
                            CartonH.PICKNO = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text));
                            CartonH.STATUS = CartonInvoices_STATUS.Status_Release;
                            CartonH.CDATE = dbTime.DBDate;
                            CartonH.CTIME = dbTime.DBTime;
                            CartonH.CUSER = this.GetUserCode();
                            CartonH.MDATE = dbTime.DBDate;
                            CartonH.MTIME = dbTime.DBTime;
                            CartonH.MUSER = this.GetUserCode();
                            facade.AddCartoninvoices(CartonH);

                            serbook.SNprefix = "K" + dbTime.DBDate.ToString().Substring(2, 6).ToString();
                            serbook.MAXSerial = "2";
                            serbook.MUser = this.GetUserCode();
                            serbook.MDate = dbTime.DBDate;
                            serbook.MTime = dbTime.DBTime;

                            facade.AddSerialbook(serbook);


                        }
                        else
                        {
                            string MAXNO = (objLot as Serialbook).MAXSerial;
                            string SNNO = (objLot as Serialbook).SNprefix;
                            CartonH.CARINVNO = SNNO + Convert.ToString(MAXNO).PadLeft(3, '0');
                            CartonH.PICKNO = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text));
                            CartonH.STATUS = CartonInvoices_STATUS.Status_Release;
                            CartonH.CDATE = dbTime.DBDate;
                            CartonH.CTIME = dbTime.DBTime;
                            CartonH.CUSER = this.GetUserCode();
                            CartonH.MDATE = dbTime.DBDate;
                            CartonH.MTIME = dbTime.DBTime;
                            CartonH.MUSER = this.GetUserCode();
                            facade.AddCartoninvoices(CartonH);

                            //����tblserialbook
                            serbook.SNprefix = SNNO;
                            serbook.MAXSerial = Convert.ToString((int.Parse(MAXNO) + 1));
                            serbook.MUser = this.GetUserCode();
                            serbook.MDate = dbTime.DBDate;
                            serbook.MTime = dbTime.DBTime;
                            facade.UpdateSerialbook(serbook);
                        }
                    }
                    #endregion
                }


                #endregion
                // _Invenfacade.GetStorageDetail
                this.DataProvider.CommitTransaction();
                WebInfoPublish.Publish(this, "����ɹ���", this.languageComponent1);
                this.gridHelper.RequestData();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.Publish(this, "���ϳ���", this.languageComponent1);
                return;
            }
            return;
        }

        protected void cmdClosePick_Click(object sender, EventArgs e)
        {
            //1���ж��Ѽ�������=��������   
            //2��������  ��������������������
            //3������  update tblpickdetail ClosePick  tblpick ClosePick
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }
            string pickno = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text));

            Pick pick = facade.GetPick(pickno) as Pick;
            if (pick == null)
            {
                WebInfoPublish.Publish(this, pickno + "���������Ų����ڣ�", this.languageComponent1);
                return;

            }
            if (pick.Status != "WaitPick" && pick.Status != "Pick")
            {

                WebInfoPublish.Publish(this, pickno + "�����Ǵ����Ϻͼ����в��ܲ�����", this.languageComponent1);
                return;
            }
            object[] pikdetailObj = facade.GetPickLineByPickNoNotCancel(pickno);
            if (pikdetailObj == null)
            {
                WebInfoPublish.Publish(this, pickno + "��������Ϣȱʧ��", this.languageComponent1);
                return;

            }
            try
            {
                this.DataProvider.BeginTransaction();




                foreach (PickDetail pickDetail in pikdetailObj)
                {
                    if (pickDetail.QTY == pickDetail.SQTY + pickDetail.OweQTY ||
                        ((pickDetail.OweQTY == pickDetail.SQTY) && pickDetail.OweQTY != 0))
                    {
                        pickDetail.Status = PickDetail_STATUS.Status_ClosePick;
                        facade.UpdatePickdetail(pickDetail);
                    }
                    else
                    {
                        int total = facade.QueryPickMaterialTotal(pickDetail.PickNo, pickDetail.PickLine, pickDetail.DQMCode);

                        if (total + pickDetail.OweQTY == pickDetail.QTY)
                        {
                            pickDetail.Status = PickDetail_STATUS.Status_ClosePick;
                            pickDetail.SQTY = pickDetail.QTY;
                            facade.UpdatePickdetail(pickDetail);

                        }
                        else
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "������������", this.languageComponent1);
                            return;
                        }
                    }
                }

                foreach (PickDetail pickDetail in pikdetailObj)
                {

                }

                DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                object[] pickObj = facade.GetCartoninvoicesByPickNo(pickno);
                if (pickObj == null)
                {
                    #region ���������䵥
                    //edit by sam
                    CARTONINVOICES CartonH = facade.CreateNewCartoninvoices();

                    object[] pikdetail_obj = facade.GetAllLineByPickNoNotCancel(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text)));
                    if (pikdetail_obj == null)
                    {

                        object objLot = null;
                        objLot = facade.GetNewLotNO("K", dbTime.DBDate.ToString().Substring(2, 6).ToString());
                        Serialbook serbook = facade.CreateNewSerialbook();
                        if (objLot == null)
                        {
                            CartonH.CARINVNO = "K" + dbTime.DBDate.ToString().Substring(2, 6).ToString() + "001";
                            CartonH.PICKNO = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text));
                            CartonH.STATUS = CartonInvoices_STATUS.Status_Release;
                            CartonH.CDATE = dbTime.DBDate;
                            CartonH.CTIME = dbTime.DBTime;
                            CartonH.CUSER = this.GetUserCode();
                            CartonH.MDATE = dbTime.DBDate;
                            CartonH.MTIME = dbTime.DBTime;
                            CartonH.MUSER = this.GetUserCode();
                            facade.AddCartoninvoices(CartonH);

                            serbook.SNprefix = "K" + dbTime.DBDate.ToString().Substring(2, 6).ToString();
                            serbook.MAXSerial = "2";
                            serbook.MUser = this.GetUserCode();
                            serbook.MDate = dbTime.DBDate;
                            serbook.MTime = dbTime.DBTime;

                            facade.AddSerialbook(serbook);


                        }
                        else
                        {
                            string MAXNO = (objLot as Serialbook).MAXSerial;
                            string SNNO = (objLot as Serialbook).SNprefix;
                            CartonH.CARINVNO = SNNO + Convert.ToString(MAXNO).PadLeft(3, '0');
                            CartonH.PICKNO = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text));
                            CartonH.STATUS = CartonInvoices_STATUS.Status_Release;
                            CartonH.CDATE = dbTime.DBDate;
                            CartonH.CTIME = dbTime.DBTime;
                            CartonH.CUSER = this.GetUserCode();
                            CartonH.MDATE = dbTime.DBDate;
                            CartonH.MTIME = dbTime.DBTime;
                            CartonH.MUSER = this.GetUserCode();
                            facade.AddCartoninvoices(CartonH);

                            //����tblserialbook
                            serbook.SNprefix = SNNO;
                            serbook.MAXSerial = Convert.ToString((int.Parse(MAXNO) + 1));
                            serbook.MUser = this.GetUserCode();
                            serbook.MDate = dbTime.DBDate;
                            serbook.MTime = dbTime.DBTime;
                            facade.UpdateSerialbook(serbook);
                        }
                    }
                    #endregion
                }



                if (pick != null)
                {
                    pick.Status = Pick_STATUS.Status_MakePackingList;

                    pick.FinishDate = dbTime.DBDate;
                    pick.FinishTime = dbTime.DBTime;
                    pick.MaintainDate = dbTime.DBDate;
                    pick.MaintainTime = dbTime.DBTime;
                    pick.MaintainUser = this.GetUserCode();
                    facade.UpdatePick(pick);
                }



                #region ��invinouttrans��������һ������ �䵥�������
                DBDateTime dbTime1 = FormatHelper.GetNowDBDateTime(this.DataProvider);
                foreach (PickDetail pickDetail in pikdetailObj)
                {
                    InvInOutTrans trans = facade.CreateNewInvInOutTrans();
                    trans.CartonNO = string.Empty;
                    trans.DqMCode = pickDetail.DQMCode;
                    trans.FacCode = string.Empty;
                    trans.FromFacCode = string.Empty;
                    trans.FromStorageCode = string.Empty;
                    trans.InvNO = pick.InvNo;//.InvNo;
                    trans.InvType = pick.PickType;
                    trans.LotNo = string.Empty;
                    trans.MaintainDate = dbTime1.DBDate;
                    trans.MaintainTime = dbTime1.DBTime;
                    trans.MaintainUser = this.GetUserCode();
                    trans.MCode = pickDetail.MCode;
                    trans.ProductionDate = 0;
                    trans.Qty = pickDetail.QTY;
                    trans.Serial = 0;
                    trans.StorageAgeDate = 0;
                    trans.StorageCode = string.Empty;
                    trans.SupplierLotNo = string.Empty;
                    trans.TransNO = pickDetail.PickNo;
                    trans.TransType = "OUT";
                    trans.Unit = string.Empty;
                    trans.ProcessType = "ClosePick";
                    facade.AddInvInOutTrans(trans);
                }

                #endregion

                this.DataProvider.CommitTransaction();
                WebInfoPublish.Publish(this, "�������", this.languageComponent1);

                if (string.IsNullOrEmpty(pick.GFFlag))
                {

                    Response.Redirect(this.MakeRedirectUrl("FPackagingOperations.aspx",
                           new string[] { "Page", "PickNo" },
                           new string[] {"FPickDoneMP.aspx", txtPickNoQuery.Text.Trim().ToUpper()
                                        
                                    }));

                }
                else if (pick.GFFlag.ToUpper() == "X")
                {


                    Response.Redirect(this.MakeRedirectUrl("FGFPackagingOperations.aspx",
                         new string[] { "Page", "PickNo" },
                         new string[] { "FPickDoneMP.aspx",txtPickNoQuery.Text.Trim().ToUpper()
                                        
                                    }));

                }
                else
                { throw new Exception("�����ǲ���ȷ��-" + pick.GFFlag); }

            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.PublishInfo(this, ex.Message, this.languageComponent1);
            }


        }


        protected override void cmdSave_Click(object sender, EventArgs e)
        {
            if (facade == null)
                facade = new WarehouseFacade(base.DataProvider);
            if (!CheckGrid(false))
            {
                return;
            }
            //�ж������û�и�������ⵥ���й���
            //
            string pickno = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text));
            Pick pik = facade.GetPick(pickno) as Pick;
            if (pik == null)
            {
                WebInfoPublish.Publish(this, "���������ͷ������", this.languageComponent1);
                return;
            }
            else
            {
                if (pik.GFFlag == "X")
                {
                    WebInfoPublish.Publish(this, "������Ͳ�����Ƿ�Ϸ���", this.languageComponent1);
                    return;
                }
                if (pik.PickType == PickType.PickType_PRC)
                {
                    WebInfoPublish.Publish(this, "Ԥ����������Ƿ�Ϸ���", this.languageComponent1);
                    return;
                }
            }


            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            ArrayList array = this.gridHelper.GetCheckedRows();
            if (array.Count > 0)
            {
                foreach (GridRecord row in array)
                {
                    PickDetail PickDetail = (PickDetail)GetEditObject(row);
                    if (PickDetail.QTY == PickDetail.SQTY)
                    {
                        WebInfoPublish.PublishInfo(this, "�Ѽ���������������������������Ƿ��", this.languageComponent1);
                        return;
                    }
                    else
                    {
                        PickDetail.Status = PickDetail_STATUS.Status_Owe;
                        PickDetail.MaintainDate = dbTime.DBDate;
                        PickDetail.MaintainTime = dbTime.DBTime;
                        PickDetail.MaintainUser = this.GetUserCode();

                        facade.UpdatePickdetail(PickDetail);

                        WebInfoPublish.PublishInfo(this, "����Ƿ�ϳɹ���", this.languageComponent1);
                    }
                }
            }
            this.gridHelper.RequestData();
        }
        protected void cmdInOut_Click(object sender, EventArgs e)
        {
            if (facade == null)
                facade = new WarehouseFacade(base.DataProvider);
            if (_Invenfacade == null)
                _Invenfacade = new InventoryFacade(base.DataProvider);
            if (!CheckGrid(true))
            {
                return;
            }
            //�ж������û�и�������ⵥ���й���
            //
            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            ArrayList array = this.gridHelper.GetCheckedRows();
            if (array.Count > 0)
            {
                foreach (GridRecord row in array)
                {
                    PickDetail PickDetail = (PickDetail)GetEditObject(row);
                    object pik_obj = _Invenfacade.GetPick(PickDetail.PickNo);
                    string StorageCode = (pik_obj as Pick).StorageCode;
                    string DQMCode = PickDetail.DQMCode;
                    string PickNo = PickDetail.PickNo;
                    Response.Redirect(this.MakeRedirectUrl("FPickInOutDetail.aspx",
                                   new string[] { "StorageCode", "DQMCode", "PickNo" },
                                   new string[] { StorageCode, DQMCode, PickNo }));
                }
            }
        }
        #endregion
        #region    Others
        protected bool CheckStorageCode(string CartonNo, bool IsAll)
        {
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }
            if (string.IsNullOrEmpty(CartonNo))
            {
                CartonNo = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdite.Text));
            }
            int result = facade.CheckStorageCode(CartonNo, FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtPickNoQuery.Text)), this.chkINOUTCHECK.Checked, IsAll, FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtNumberEdite.Text)), FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNEdite.Text)));
            switch (result)
            {
                case 1:
                    WebInfoPublish.Publish(this, "����в����ڸ����", this.languageComponent1);
                    return false;

                case 2:
                    WebInfoPublish.Publish(this, "���������ţ�" + this.txtPickNoQuery.Text + "�޼������", this.languageComponent1);
                    return false;

                case 3:
                    WebInfoPublish.Publish(this, "��ţ�" + CartonNo + "�ڿ���еĿ�λ�����������еĿ�λ��ͬ", this.languageComponent1);
                    return false;
                case 4:
                    WebInfoPublish.Publish(this, "��ţ�" + CartonNo + "������������Χ", this.languageComponent1);// �������ϱ��볬�����ڼ����������
                    return false;
                case 5:
                    WebInfoPublish.Publish(this, "��ţ�" + CartonNo + "�������������������", this.languageComponent1);
                    return false;
                case 6:

                    WebInfoPublish.Publish(this, "��ţ�" + CartonNo + "Υ���Ƚ��ȳ�����", this.languageComponent1);
                    return false;
                case 7:
                    WebInfoPublish.Publish(this, "��ţ�" + CartonNo + "�Ѿ������", this.languageComponent1);
                    return false;
                case 8:
                    WebInfoPublish.Publish(this, "���ݲɼ���ȫ", this.languageComponent1);
                    return false;
                case 9:
                    WebInfoPublish.Publish(this, "�����û�п��õ�SN", this.languageComponent1);
                    return false;
                case 10:
                    WebInfoPublish.Publish(this, "���ܿر�����������", this.languageComponent1);
                    return false;
                case 11:
                    WebInfoPublish.Publish(this, "���ϱ�û��ά���ܿ�����", this.languageComponent1);
                    return false;
                case 12:
                    WebInfoPublish.Publish(this, "û�п��õ����������䲻�����", this.languageComponent1);
                    return false;
                case 13:
                    WebInfoPublish.Publish(this, "�������ܳ������п�������", this.languageComponent1);
                    return false;
                case 14:
                    WebInfoPublish.Publish(this, "����������Ƿ��,���ܼ�������.", this.languageComponent1);
                    return false;
                default:
                    return true;

            }

        }
        protected bool CheckGrid(bool IsCheck)
        {
            if (this.gridWebGrid.Rows.Count == 0)
            {
                //��ʾ����
                WebInfoPublish.PublishInfo(this, "Gridû������", this.languageComponent1);
                return false;
            }
            int count = 0;
            for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
            {
                if (this.gridWebGrid.Rows[i].Items.FindItemByKey("Check").Text.ToUpper() == "TRUE")
                {
                    count++;
                }
            }
            if (count == 0)
            {
                WebInfoPublish.PublishInfo(this, "�빴ѡ��ֻ��ѡһ������", this.languageComponent1);
                return false;
            }
            else if (count > 1 && IsCheck)
            {
                WebInfoPublish.PublishInfo(this, "ֻ�ܹ�ѡһ������", this.languageComponent1);
                return false;
            }
            return true;
        }
        #endregion
        #region Object <--> Page

        protected override object GetEditObject(GridRecord row)
        {
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }
            object obj = facade.GetPickdetail(row.Items.FindItemByKey("PickNo").Value.ToString(), row.Items.FindItemByKey("PickLine").Value.ToString());

            if (obj != null)
            {
                return (PickDetail)obj;
            }
            return null;
        }

        #endregion

        #region For Export To Excel

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{

                    ((PickDetailEx)obj).DQMCode,
                    ((PickDetailEx)obj).CustMCode,
                    ((PickDetailEx)obj).QTY.ToString(),
                    ((PickDetailEx)obj).SQTY.ToString(),
                    ((PickDetailEx)obj).Unit,
                    ((PickDetailEx)obj).sumBox,
                    ((PickDetailEx)obj).MaintainUser,
                    FormatHelper.ToDateString(((PickDetailEx)obj).MaintainDate),
                    FormatHelper.ToTimeString(((PickDetailEx)obj).MaintainTime),
                 
            };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	

                    "DQMaterialNO",
                    "CusMCode", 
                    "RequireQty", 
                    "PickedQTY", 
                    "MUOM",
                    "sumBoxNo",
                    "MUser",
                    "MDate",
                    "MTime"
           
            };
        }

        #endregion



        protected void cmdReturn_ServerClick(object sender, EventArgs e)
        {

            Response.Redirect(this.MakeRedirectUrl("FQueryStoragePickMP.aspx",
                                 new string[] { "PickNo" },
                                 new string[] { txtPickNoQuery.Text.Trim().ToUpper()
                                        
                                    }));

        }
    }
}
