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
using BenQGuru.eMES.MOModel;



namespace BenQGuru.eMES.Web.Warehouse
{
    public partial class FGFPackagingOperations : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

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
                this.InitPageLanguage(this.languageComponent1, false);

                string pickNo = Request.QueryString["PickNo"];
                if (!string.IsNullOrEmpty(pickNo))
                    txtPickNoQuery.Text = pickNo;
                string carinvno = Request.QueryString["CARINVNO"];
                if (!string.IsNullOrEmpty(carinvno))
                    txtCarInvNoQuery.Text = carinvno;

                if (Request.QueryString.AllKeys.Length > 0)
                    cmdReturn.Visible = true;


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
            this.gridHelper.AddColumn("CarInvNo", "�����䵥��", null);
            this.gridHelper.AddColumn("IT_CartonNo", "��װ���", null);
            this.gridHelper.AddColumn("DQMCode", "�������ϱ���", null);
            this.gridHelper.AddColumn("DQSMCode", "����S����", null);
            this.gridHelper.AddColumn("GFHWItemCode", "�����Ϊ����", null);
            this.gridHelper.AddColumn("GFPackingSEQ", "�����װ���", null);
            this.gridHelper.AddColumn("PickedQTY", "�Ѽ�����", null);
            this.gridHelper.AddColumn("PackingQTY", "��װ����", null);

            this.gridHelper.AddDefaultColumn(true, false);
            this.gridHelper.AddLinkColumn("LinkToSN", "SN��Ϣ");

            this.gridWebGrid.Columns["CarInvNo"].Hidden = true;

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            if (!string.IsNullOrEmpty(txtPickNoQuery.Text))
            {

                this.gridHelper.RequestData();
            }

        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["CarInvNo"] = ((PackagingOperations)obj).CARINVNO;
            row["IT_CartonNo"] = ((PackagingOperations)obj).CARTONNO;
            row["DQMCode"] = ((PackagingOperations)obj).DQMCODE;
            row["GFHWItemCode"] = ((PackagingOperations)obj).GFHWITEMCODE;
            row["GFPackingSEQ"] = ((PackagingOperations)obj).GFPACKINGSEQ;
            row["PickedQTY"] = ((PackagingOperations)obj).SQTY;
            row["PackingQTY"] = ((PackagingOperations)obj).QTY;
            row["DQSMCode"] = ((PackagingOperations)obj).DQSMCODE;
            return row;

        }

        #region ɾ��
        protected override object GetEditObject(GridRecord row)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            object obj = _WarehouseFacade.QueryCartonInvDetailMaterial(row.Items.FindItemByKey("CarInvNo").Text,
                row.Items.FindItemByKey("IT_CartonNo").Text, row.Items.FindItemByKey("DQMCode").Text);
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
            CartonInvDetailMaterial[] cartonInvDetailMaterialList = ((CartonInvDetailMaterial[])domainObjects.ToArray(typeof(CartonInvDetailMaterial)));
            try
            {

                if (cartonInvDetailMaterialList == null || cartonInvDetailMaterialList.Length == 0)
                {
                    WebInfoPublish.Publish(this, "������Ҫɾ�������ݣ�", this.languageComponent1);
                    return;

                }
                string carinvno = cartonInvDetailMaterialList[0].CARINVNO;
                CARTONINVOICES cartoninvoices = _WarehouseFacade.GetCartoninvoices(carinvno)
                 as CARTONINVOICES;
                if (cartoninvoices == null)
                {

                    WebInfoPublish.Publish(this, carinvno + "��װ��Ų����ڣ�", this.languageComponent1);
                    return;
                }

                if (cartoninvoices.STATUS == "ClosePack" ||
                  cartoninvoices.STATUS == "Close" ||
                  cartoninvoices.STATUS == "ClosePackingList")
                {
                    WebInfoPublish.Publish(this, "�����䵥״̬����Ϊ��װ��ɡ��䵥��ɡ�����⣡", this.languageComponent1);
                    return;

                }
                //TBLCartonInvDetailMaterial
                //  1���������������ѯ����������ɾ����

                this.DataProvider.BeginTransaction();
                foreach (CartonInvDetailMaterial cartonInvDetailMaterial in cartonInvDetailMaterialList)
                {
                    #region delete
                    Pick pickHead = _InventoryFacade.GetPick(cartonInvDetailMaterial.PICKNO) as Pick;
                    PickDetail pickDetail = _InventoryFacade.GetPickDetail(cartonInvDetailMaterial.PICKNO, cartonInvDetailMaterial.PICKLINE) as PickDetail;
                    object[] objPickdetailmaterials = this._WarehouseFacade.QueryPICKDetailMaterialBydqMCode(cartonInvDetailMaterial.PICKNO, cartonInvDetailMaterial.DQMCODE);
                    //Pickdetailmaterial pickdetailmaterial = _WarehouseFacade.GetPickdetailmaterial(cartonInvDetailMaterial.PICKNO, cartonInvDetailMaterial.CARTONNO) as Pickdetailmaterial;
                    if (pickDetail == null || pickHead == null || objPickdetailmaterials == null)
                    {
                        this.DataProvider.RollbackTransaction();
                        return;
                    }


                    //4��������ϸɾ����Ϻ�ͬʱɾ����Ӧ��tblcartoninvdetail��
                    _WarehouseFacade.DeleteCartonInvDetailMaterial(cartonInvDetailMaterial);
                    //2��ɾ����ź�ͬʱɾ�������е�����SN��Ϣ��
                    _WarehouseFacade.DeleteCartoninvdetailsnByCartonNo(cartonInvDetailMaterial.CARTONNO);

                    object[] list = _WarehouseFacade.QueryCartonInvDetailMaterial(cartonInvDetailMaterial.CARINVNO);
                    if (list == null)
                    {
                        CartonInvDetail cartonInvDetail =
                     _WarehouseFacade.GetCartonInvDetail(cartonInvDetailMaterial.CARINVNO,
                                                         cartonInvDetailMaterial.CARTONNO) as CartonInvDetail;
                        if (cartonInvDetail != null)
                        {
                            _WarehouseFacade.DeleteCartonInvDetail(cartonInvDetail);
                        }


                        pickHead.Status = PickHeadStatus.PickHeadStatus_Pick;
                        _WarehouseFacade.UpdatePick(pickHead);
                    }

                    //5�����������ͷ״̬Ϊpick��pack���ҷ����䵥ͷ״̬ΪRelease��packʱ�ſ���ɾ����
                    //    (��Ӧ����ϣ�a��tblpick.status=pick&&tblcartoninvoices.status=Release
                    //  b��tblpick.status=pack&&tblcartoninvoices.status=pack)
                    //if ((pickHead.Status == PickHeadStatus.PickHeadStatus_Pick && cartoninvoices.STATUS == PickDetail_STATUS.Status_Release)
                    //    || (pickHead.Status == PickHeadStatus.PickHeadStatus_Pack && cartoninvoices.STATUS == PickDetail_STATUS.Status_Pack)
                    //    || pickHead.Status == PickHeadStatus.PickHeadStatus_WaitPick)
                    //{
                    //    //3����ϸɾ����Ϻ󽫼��������ͷ����ΪMakePackingList��
                    //    object[] list = _WarehouseFacade.QueryCartonInvDetailMaterial(cartonInvDetailMaterial.CARINVNO);
                    //    if (list == null)
                    //    {
                    //        CartonInvDetail cartonInvDetail =
                    //   _WarehouseFacade.GetCartonInvDetail(cartonInvDetailMaterial.CARINVNO,
                    //                                       cartonInvDetailMaterial.CARTONNO) as CartonInvDetail;
                    //        if (cartonInvDetail != null)
                    //        {
                    //            _WarehouseFacade.DeleteCartonInvDetail(cartonInvDetail);
                    //        }
                    //        pickHead.Status = PickHeadStatus.PickHeadStatus_MakePackingList;
                    //        _WarehouseFacade.UpdatePick(pickHead);
                    //    }
                    //}
                    //else
                    //{
                    //    this.DataProvider.RollbackTransaction();
                    //    WebInfoPublish.Publish(this, "���������ͷ״̬Ϊpick�����䵥ͷ״̬ΪRelease���߼��������ͷ״̬Ϊpack�����䵥ͷ״̬Ϊpackʱ�ſ���ɾ��", this.languageComponent1);
                    //    return;
                    //}

                    pickDetail.PQTY -= cartonInvDetailMaterial.QTY;
                    _InventoryFacade.UpdatePickDetail(pickDetail);

                    #region Pickdetailmaterial
                    decimal qTY = cartonInvDetailMaterial.QTY;
                    foreach (Pickdetailmaterial _pickdetailmaterial in objPickdetailmaterials)
                    {
                        decimal num = _pickdetailmaterial.PQty;
                        if (num > 0)
                        {
                            if (qTY > num)
                            {
                                _pickdetailmaterial.PQty -= _pickdetailmaterial.PQty;
                                this._WarehouseFacade.UpdatePickdetailmaterial(_pickdetailmaterial);
                                qTY -= num;
                            }
                            else
                            {
                                _pickdetailmaterial.PQty -= qTY;
                                this._WarehouseFacade.UpdatePickdetailmaterial(_pickdetailmaterial);
                                qTY = 0;
                                break;
                            }
                        }
                    }

                    #endregion
                    //pickdetailmaterial.PQty -= cartonInvDetailMaterial.QTY;
                    //_WarehouseFacade.UpdatePickdetailmaterial(pickdetailmaterial);
                    cartonInvDetailMaterial.QTY -= cartonInvDetailMaterial.QTY;
                    _WarehouseFacade.UpdateCartonInvDetailMaterial(cartonInvDetailMaterial);
                    #endregion
                }
                CartonInvDetailMaterial[] cas = _WarehouseFacade.GetCartonInvDetailMaterialByCa(txtCarInvNoQuery.Text);
                if (cas.Length == 0)
                {
                    CARTONINVOICES cartoninvoices1 = (CARTONINVOICES)_WarehouseFacade.GetCartoninvoices(txtCarInvNoQuery.Text);
                    cartoninvoices1.STATUS = CartonInvoices_STATUS.Status_Release;
                    _WarehouseFacade.UpdateCartoninvoices(cartoninvoices1);
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
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }

            string pickNo = FormatHelper.CleanString(this.txtPickNoQuery.Text.Trim().ToUpper());
            object obj = _WarehouseFacade.GetCartonInvoices(pickNo);
            if (obj == null)
            {
                this.txtCarInvNoQuery.Text = string.Empty;
            }
            else
            {
                this.txtCarInvNoQuery.Text = (obj as CARTONINVOICES).CARINVNO;
            }

            BindGFHWItemCode();
            BindDQMaterialNO();
            this.txtSNEdit.Text = string.Empty;

            return this._WarehouseFacade.QueryPackagingOperations(pickNo, FormatHelper.CleanString(this.txtCARTONNOQuery.Text), inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            return this._WarehouseFacade.QueryPackagingOperationsCount(
                FormatHelper.CleanString(this.txtPickNoQuery.Text.Trim().ToUpper()),
                FormatHelper.CleanString(this.txtCARTONNOQuery.Text)
                );
        }

        #endregion

        #region Button

        #region  add by sam ��ѯǰ���
        protected override void cmdQuery_Click(object sender, System.EventArgs e)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            string pickNo = this.txtPickNoQuery.Text.Trim().ToUpper();
            Pick pick = (Pick)_WarehouseFacade.GetPick(pickNo);
            if (pick != null)
            {
                if (pick.GFFlag != "X")
                {
                    WebInfoPublish.Publish(this, "������װҳ�����", this.languageComponent1);
                    return;
                }
            }
            this.gridHelper.RequestData();
            if (this.gridHelper2 != null)
                this.gridHelper2.RequestData();
            this.buttonHelper.PageActionStatusHandle(PageActionType.Query);
        }
        #endregion

        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            if (commandName == "LinkToSN")
            {
                string carInvNo = row.Items.FindItemByKey("CarInvNo").Text.Trim();
                string cartonNo = row.Items.FindItemByKey("IT_CartonNo").Text.Trim();

                Response.Redirect(
                                    this.MakeRedirectUrl("FGFPackagingOperationsSN.aspx",
                                    new string[] { "CarInvNo", "CartonNo" },
                                    new string[] { carInvNo, cartonNo })
                                   );
            }
        }

        protected void cmdSaveIt_ServerClick(object sender, System.EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }
            if (string.IsNullOrEmpty(this.txtCartonNo.Text))
            {
                WebInfoPublish.Publish(this, "�����������", this.languageComponent1);
                return;
            }
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            string pickNo = this.txtPickNoQuery.Text.Trim().ToUpper();
            string carInvNo = string.Empty;
            object obj = _WarehouseFacade.GetCartonInvoices(pickNo);
            if (obj != null)
            {
                carInvNo = (obj as CARTONINVOICES).CARINVNO;
            }
            else
            {
                WebInfoPublish.Publish(this, "�ü���������δ���������䵥��", this.languageComponent1);//add by sam 2016��5��25��
                return;
            }
            if (string.IsNullOrEmpty(ddlGFHWItemCode.SelectedValue))
            {
                WebInfoPublish.Publish(this, "������������Ϊ����", this.languageComponent1);
                return;
            }
            if (string.IsNullOrEmpty(ddlGFPackingSEQ.SelectedValue))
            {
                WebInfoPublish.Publish(this, "������������װ���", this.languageComponent1);
                return;
            }
            string gfHWItemCode = this.ddlGFHWItemCode.SelectedValue;
            string gfPackingSEQ = this.ddlGFPackingSEQ.SelectedValue;
            string suiteQTY = this.txtSuiteQTY.Text.Trim();
            string dqMaterialNO = this.ddlDQMaterialNO.SelectedValue;
            string dqsMaterialNO = this.drpDQSMCodeEdit.SelectedValue;
            string cartonNo = this.txtCartonNo.Text.Trim().ToUpper();
            string qty = this.txtQTY.Text.Trim();
            string sn = this.txtSNEdit.Text.Trim().ToUpper();

            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
            string mUser = this.GetUserCode();
            int mDate = dbDateTime.DBDate;
            int mTime = dbDateTime.DBTime;
            CartonInvDetailMaterial cartonInvDetailMaterialObj =
            _WarehouseFacade.QueryCartonInvDetailMaterial(carInvNo, cartonNo, dqMaterialNO) as CartonInvDetailMaterial;
            if (cartonInvDetailMaterialObj != null)
            {
                if (cartonInvDetailMaterialObj.GFPACKINGSEQ != gfPackingSEQ)
                {
                    WebInfoPublish.Publish(this, "�����Ѱ�װ����������!", this.languageComponent1);
                }
            }

            try
            {
                this.DataProvider.BeginTransaction();
                //����״̬
                object objPick = this._InventoryFacade.GetPick(pickNo);
                if (objPick != null)
                {
                    Pick pick = objPick as Pick;
                    pick.Status = "Pack";
                    pick.MaintainUser = mUser;
                    pick.MaintainDate = mDate;
                    pick.MaintainTime = mTime;
                    this._InventoryFacade.UpdatePick(pick);
                }

                object objPICKDetailMaterial = this._WarehouseFacade.QueryPICKDetailMaterial(pickNo, cartonNo);
                //���ϱ����Ƿ������ţ�
                if (objPICKDetailMaterial == null)
                {
                    #region ���������
                    //�����ţ��������Ϻţ��ǹ�����
                    if (string.IsNullOrEmpty(this.ddlDQMaterialNO.SelectedValue))
                    {
                        this.DataProvider.RollbackTransaction();
                        WebInfoPublish.Publish(this, "����ѡ�������Ϻ�", this.languageComponent1);
                        return;
                    }

                    //�����ڣ�Ҫͨ���������Ϻ��ж���ȡSN��������
                    //1�����ݶ������Ϻţ����ж��ǵ����ܿػ��Ƿǵ����ܿ�
                    object[] mar_objs = _WarehouseFacade.GetMaterialInfoByQDMCode(dqMaterialNO);
                    if (mar_objs == null)
                    {
                        this.DataProvider.RollbackTransaction();
                        WebInfoPublish.Publish(this, "��Ч�Ķ������Ϻ�", this.languageComponent1);
                        return;
                    }
                    Domain.MOModel.Material mar = mar_objs[0] as Domain.MOModel.Material;
                    if (mar.MCONTROLTYPE == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)  //�����ܿ�
                    {
                        #region �����ܿ�
                        if (string.IsNullOrEmpty(this.txtSNEdit.Text))
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "��������SN����", this.languageComponent1);
                            return;
                        }
                        //  ȡSN
                        //���SN��tblpickdetailmaterialsn������������û�б����жԱ�tblcartoninvoicesmaterial��tblcartoninvoicesmaterialsn����
                        object objPickdetailmaterialsn = this._WarehouseFacade.GetPickdetailmaterialsn(pickNo, sn);
                        if (objPickdetailmaterialsn == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "ˢ��SN���벻����", this.languageComponent1);
                            return;
                        }
                        Pickdetailmaterialsn pikdetailsn = objPickdetailmaterialsn as Pickdetailmaterialsn;
                        object objPickdetailmaterial = _WarehouseFacade.GetPickdetailmaterial(pikdetailsn.Pickno, pikdetailsn.Cartonno);
                        if (objPickdetailmaterial == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�����SN�Ҳ��������Ϣ", this.languageComponent1);
                            return;
                        }
                        Pickdetailmaterial Pickdetailmaterial = objPickdetailmaterial as Pickdetailmaterial;
                        //2>	���SN�����Ƿ���ڵ�ǰ���������Ŷ�Ӧ�����䵥�ķ����䵥��ϸSN��Ϣ��(TBLCartonInvDetailSN)�У������򱨴���ʾˢ��SN�����Ѱ�װ��
                        object _obj = this._WarehouseFacade.GetCartoninvdetailsn(carInvNo, sn);
                        if (_obj != null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "ˢ��SN�����Ѱ�װ��", this.languageComponent1);
                            return;
                        }
                        //�����SN�Ķ������Ϻ��Ƿ���ѡ�е���ͬ

                        if (Pickdetailmaterial.DqmCode != dqMaterialNO)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "SN�ĺ�����ѡ��Ķ������ϺŲ�һ��", this.languageComponent1);
                            return;
                        }

                        //3.	���ͨ��SN�������Ѽ�������ϸ��SN��Ϣ��(TBLPICKDetailMaterialSN)�ж�Ӧ�����(TBLPICKDetailMaterialSN.CARTONNO)���Ѽ�������ϸ��(TBLPICKDetailMaterial)��Ӧ�����������ϸ��(TBLPICKDETAIL)�еĹ����Ϊ����(TBLPICKDETAIL.GFHWITEMCODE)�������װ���(TBLPICKDETAIL.GFPACKINGSEQ)��༭������ѡ��Ĺ����Ϊ���롢�����װ����Ƿ�ͬ�ȣ��粻����򱨴���ʾ����Ŷ�Ӧѡ��Ĺ����Ϊ���롢�����װ��Ų����
                        if (string.IsNullOrEmpty(gfHWItemCode))
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "��ѡ������Ϊ����", this.languageComponent1);
                            return;
                        }
                        if (string.IsNullOrEmpty(gfPackingSEQ))
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "��ѡ������װ���", this.languageComponent1);
                            return;
                        }
                        if (string.IsNullOrEmpty(suiteQTY))
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�׼�������Ϊ��", this.languageComponent1);
                            return;
                        }
                        try
                        {
                            decimal _suiteQTY = decimal.Parse(suiteQTY);
                            if (_suiteQTY <= 0)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "�׼�������Ϊ�����������", this.languageComponent1);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�׼�������Ϊ�����������", this.languageComponent1);
                            return;
                        }
                        //object objectPickDetail = this._WarehouseFacade.GetPickdetail(pickNo, Pickdetailmaterial.Pickline);
                        //PickDetail pickDetail = objectPickDetail as PickDetail;
                        //if (pickDetail.GFHWItemCode.Equals(gfHWItemCode) && pickDetail.GFPackingSeq.Equals(gfPackingSEQ))
                        //{

                        //}
                        //else
                        //{
                        //    this.DataProvider.RollbackTransaction();
                        //    WebInfoPublish.Publish(this, "����Ŷ�Ӧѡ��Ĺ����Ϊ���롢�����װ��Ų����", this.languageComponent1);
                        //    return;
                        //}

                        //4.	���ѡ����������š������Ϊ���롢�����װ����ڷ����䵥��ϸ������Ϣ��(TBLCartonInvDetailMaterial)�ж�Ӧ�����������(TBLCartonInvDetail.CARTONNO)�������Ƿ�С�ڱ༭����������׼�������С���򱨴���ʾѡ��ļ��������š������Ϊ���롢�����װ����Ѱ�װ���׼����Ѿ����ڵ��������׼���
                        //object _objCartonInvDetailMaterial = this._WarehouseFacade.QueryCartonInvDetailMaterial(carInvNo, cartonNo, dqMaterialNO);
                        //if (_objCartonInvDetailMaterial == null)
                        //{
                        //    CartonInvDetailMaterial cartonInvDetailMaterial = new CartonInvDetailMaterial();
                        //    cartonInvDetailMaterial.CARINVNO = carInvNo;
                        //    cartonInvDetailMaterial.PICKNO = pikdetailsn.Pickno;
                        //    cartonInvDetailMaterial.PICKLINE = pikdetailsn.Pickline;
                        //    cartonInvDetailMaterial.CARTONNO = cartonNo;
                        //    cartonInvDetailMaterial.MCODE = mar.MCode;
                        //    cartonInvDetailMaterial.DQMCODE = dqMaterialNO;
                        //    cartonInvDetailMaterial.QTY = 0;
                        //    cartonInvDetailMaterial.UNIT = Pickdetailmaterial.Unit;
                        //    cartonInvDetailMaterial.GFHWITEMCODE = this.ddlGFHWItemCode.SelectedValue;
                        //    cartonInvDetailMaterial.GFPACKINGSEQ = this.ddlGFPackingSEQ.SelectedValue;
                        //    cartonInvDetailMaterial.CUSER = mUser;
                        //    cartonInvDetailMaterial.CDATE = mDate;
                        //    cartonInvDetailMaterial.CTIME = mTime;
                        //    cartonInvDetailMaterial.MDATE = mDate;
                        //    cartonInvDetailMaterial.MTIME = mTime;
                        //    cartonInvDetailMaterial.MUSER = mUser;

                        //    _WarehouseFacade.AddCartonInvDetailMaterial(cartonInvDetailMaterial);
                        //}
                        //if (this._WarehouseFacade.GetCartonNoCount(pickNo, gfHWItemCode, gfPackingSEQ) < decimal.Parse(suiteQTY))
                        //{
                        //    this.DataProvider.RollbackTransaction();
                        //    WebInfoPublish.Publish(this, "ѡ��ļ��������š������Ϊ���롢�����װ����Ѱ�װ���׼����Ѿ����ڵ��������׼���", this.languageComponent1);
                        //    return;
                        //}

                        //5>	���������䵥��ϸSN��Ϣ��(TBLCartonInvDetailSN)����
                        Pickdetailmaterialsn pickdetailmaterialsn = objPickdetailmaterialsn as Pickdetailmaterialsn;
                        CARTONINVDETAILSN _CARTONINVDETAILSN = new CARTONINVDETAILSN();
                        _CARTONINVDETAILSN.CARINVNO = carInvNo;
                        _CARTONINVDETAILSN.PICKNO = pickNo;
                        _CARTONINVDETAILSN.PICKLINE = pickdetailmaterialsn.Pickline;
                        _CARTONINVDETAILSN.CARTONNO = cartonNo;
                        _CARTONINVDETAILSN.SN = pickdetailmaterialsn.Sn;
                        _CARTONINVDETAILSN.MUSER = mUser;
                        _CARTONINVDETAILSN.MDATE = mDate;
                        _CARTONINVDETAILSN.MTIME = mTime;
                        this._WarehouseFacade.AddCARTONINVDETAILSN(_CARTONINVDETAILSN);

                        object _pickdetail_obj = _WarehouseFacade.GetPickdetail(pickNo, pikdetailsn.Pickline);
                        PickDetail pickDetail = _pickdetail_obj as PickDetail;

                        pickDetail.GFHWItemCode = gfHWItemCode;
                        pickDetail.GFPackingSeq = gfPackingSEQ;
                        //3>	����װ����Ƿ���ڵ�ǰ���������Ŷ�Ӧ�����䵥�ķ����䵥��ϸ��Ϣ��(TBLCartonInvDetail)�У������������������䵥��ϸ��Ϣ��(TBLCartonInvDetail)����
                        object objCartonInvDetail = this._WarehouseFacade.GetCartonInvDetail(carInvNo, cartonNo);
                        if (objCartonInvDetail == null)
                        {
                            //���Ƿ����--������
                            CartonInvDetail cartonInvDetail = new CartonInvDetail();
                            cartonInvDetail.CARINVNO = carInvNo;
                            cartonInvDetail.PICKNO = pickNo;
                            cartonInvDetail.STATUS = "Pack";
                            cartonInvDetail.CARTONNO = cartonNo;
                            cartonInvDetail.PACKMCODE = "";
                            cartonInvDetail.PACKQTY = 1;
                            cartonInvDetail.CUSER = mUser;
                            cartonInvDetail.CDATE = mDate;
                            cartonInvDetail.CTIME = mTime;
                            cartonInvDetail.MDATE = mDate;
                            cartonInvDetail.MTIME = mTime;
                            cartonInvDetail.MUSER = mUser;
                            this._WarehouseFacade.AddCartonInvDetail(cartonInvDetail);

                            //this.DataProvider.CommitTransaction();

                            //  ���� cartonInvDetailMaterial
                            CartonInvDetailMaterial cartonInvDetailMaterial = new CartonInvDetailMaterial();
                            cartonInvDetailMaterial.CARINVNO = carInvNo;
                            cartonInvDetailMaterial.CARTONNO = cartonNo;
                            cartonInvDetailMaterial.CDATE = mDate;
                            cartonInvDetailMaterial.CTIME = mTime;
                            cartonInvDetailMaterial.CUSER = mUser;
                            cartonInvDetailMaterial.DQMCODE = dqMaterialNO;
                            cartonInvDetailMaterial.DQSMCODE = dqsMaterialNO;//add by sam
                            cartonInvDetailMaterial.MCODE = mar.MCode;
                            cartonInvDetailMaterial.MDATE = mDate;
                            cartonInvDetailMaterial.MTIME = mTime;
                            cartonInvDetailMaterial.MUSER = mUser;
                            cartonInvDetailMaterial.PICKLINE = pikdetailsn.Pickline;
                            cartonInvDetailMaterial.PICKNO = pikdetailsn.Pickno;
                            cartonInvDetailMaterial.QTY = 1;
                            cartonInvDetailMaterial.UNIT = Pickdetailmaterial.Unit;
                            cartonInvDetailMaterial.GFHWITEMCODE = pickDetail.GFHWItemCode;
                            cartonInvDetailMaterial.GFPACKINGSEQ = pickDetail.GFPackingSeq;

                            _WarehouseFacade.AddCartonInvDetailMaterial(cartonInvDetailMaterial);
                        }
                        else
                        {


                            object objCartonInvDetailMaterial = this._WarehouseFacade.QueryCartonInvDetailMaterial(carInvNo, cartonNo, dqMaterialNO);
                            if (objCartonInvDetailMaterial == null)
                            {
                                CartonInvDetailMaterial cartonInvDetailMaterial = new CartonInvDetailMaterial();
                                cartonInvDetailMaterial.CARINVNO = carInvNo;
                                cartonInvDetailMaterial.CARTONNO = cartonNo;
                                cartonInvDetailMaterial.CDATE = mDate;
                                cartonInvDetailMaterial.CTIME = mTime;
                                cartonInvDetailMaterial.CUSER = mUser;
                                cartonInvDetailMaterial.DQMCODE = dqMaterialNO;
                                cartonInvDetailMaterial.MCODE = mar.MCode;
                                cartonInvDetailMaterial.MDATE = mDate;
                                cartonInvDetailMaterial.MTIME = mTime;
                                cartonInvDetailMaterial.MUSER = mUser;
                                cartonInvDetailMaterial.PICKLINE = pikdetailsn.Pickline;
                                cartonInvDetailMaterial.PICKNO = pikdetailsn.Pickno;
                                cartonInvDetailMaterial.QTY = 1;
                                cartonInvDetailMaterial.UNIT = Pickdetailmaterial.Unit;
                                cartonInvDetailMaterial.GFHWITEMCODE = pickDetail.GFHWItemCode;
                                cartonInvDetailMaterial.GFPACKINGSEQ = pickDetail.GFPackingSeq;

                                _WarehouseFacade.AddCartonInvDetailMaterial(cartonInvDetailMaterial);
                            }
                            else
                            {
                                CartonInvDetailMaterial cartonInvDetailMaterial = objCartonInvDetailMaterial as CartonInvDetailMaterial;
                                cartonInvDetailMaterial.QTY += 1;
                                cartonInvDetailMaterial.MUSER = mUser;
                                cartonInvDetailMaterial.MDATE = mDate;
                                cartonInvDetailMaterial.MTIME = mTime;
                                this._WarehouseFacade.UpdateCartonInvDetailMaterial(cartonInvDetailMaterial);
                            }
                            // ���±�CartonInvDetail���԰�����
                            CartonInvDetail cartonInvDetail = objCartonInvDetail as CartonInvDetail;
                            object[] objs_cartondetailmaterial = _WarehouseFacade.GetCartonInvDetailMaterial(carInvNo, cartonNo);
                            if (objs_cartondetailmaterial == null)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "���·��������", this.languageComponent1);
                                return;
                            }
                            double ss = 0;
                            for (int i = 0; i < objs_cartondetailmaterial.Length; i++)
                            {
                                CartonInvDetailMaterial cartondetailmar = objs_cartondetailmaterial[i] as CartonInvDetailMaterial;
                                ss += (double)cartondetailmar.QTY;
                            }
                            cartonInvDetail.PACKQTY = ss;
                            cartonInvDetail.MUSER = mUser;
                            cartonInvDetail.MDATE = mDate;
                            cartonInvDetail.MTIME = mTime;
                            _WarehouseFacade.UpdateCartonInvDetail(cartonInvDetail);



                        }
                        #endregion
                        #region //���� pickdetailmaterial
                        Pickdetailmaterial.PQty += 1;
                        Pickdetailmaterial.MaintainDate = mDate;
                        Pickdetailmaterial.MaintainTime = mTime;
                        Pickdetailmaterial.MaintainUser = mUser;
                        _WarehouseFacade.UpdatePickdetailmaterial(Pickdetailmaterial);

                        #endregion
                        #region //���� pickdetail
                        object[] pickdetailmaterial_obj = _WarehouseFacade.GetAllPickDetailMaterialByPickNoAndLine(pickNo, pikdetailsn.Pickline);
                        if (pickdetailmaterial_obj == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�����װ��������", this.languageComponent1);
                            return;
                        }
                        decimal sum = 0;
                        for (int i = 0; i < pickdetailmaterial_obj.Length; i++)
                        {
                            Pickdetailmaterial pickdetailmater = pickdetailmaterial_obj[i] as Pickdetailmaterial;
                            sum += pickdetailmater.PQty;
                        }
                        object pickdetail_obj = _WarehouseFacade.GetPickdetail(pickNo, pikdetailsn.Pickline);
                        if (pickdetail_obj == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "���¼����ϸ�����", this.languageComponent1);
                            return;
                        }
                        PickDetail pickdetail = pickdetail_obj as PickDetail;
                        pickdetail.PQTY = sum;
                        if (pickdetail.PQTY == pickdetail.SQTY)
                        {
                            pickdetail.Status = PickDetail_STATUS.Status_ClosePack;
                        }
                        else
                        {
                            pickdetail.Status = PickDetail_STATUS.Status_Pack;
                        }
                        pickdetail.MaintainDate = mDate;
                        pickdetail.MaintainTime = mTime;
                        pickdetail.MaintainUser = mUser;

                        _WarehouseFacade.UpdatePickdetail(pickdetail);
                        #endregion

                        #region  ���װ��ɣ��ı�״̬
                        object objCartonInvDetail1 = this._WarehouseFacade.GetCartonInvDetail(carInvNo, cartonNo);
                        CartonInvDetail car = objCartonInvDetail1 as CartonInvDetail;
                        if (pickdetail.SQTY == (decimal)car.PACKQTY)
                        {
                            car.STATUS = CartonInvoices_STATUS.Status_ClosePack;
                            car.MUSER = mUser;
                            car.MDATE = mDate;
                            car.MTIME = mTime;
                            _WarehouseFacade.UpdateCartonInvDetail(car);
                        }
                        #endregion

                        InvInOutTrans trans = _WarehouseFacade.CreateNewInvInOutTrans();
                        trans.CartonNO = cartonNo;
                        trans.DqMCode = mar.DqmCode; ;
                        trans.FacCode = string.Empty;
                        trans.FromFacCode = string.Empty;
                        trans.FromStorageCode = string.Empty;
                        trans.InvNO = (objPick as Pick).InvNo;//.InvNo;
                        trans.InvType = (objPick as Pick).PickType;
                        trans.LotNo = string.Empty;
                        trans.MaintainDate = mDate;
                        trans.MaintainTime = mTime;
                        trans.MaintainUser = mUser;
                        trans.MCode = mar.MCode;


                        trans.Qty = 1;
                        // stor.AvailableQty =stor.StorageQty-stor.FreezeQty;


                        trans.ProductionDate = 0;

                        trans.Serial = 0;
                        trans.StorageAgeDate = 0;
                        trans.StorageCode = (objPick as Pick).StorageCode;
                        trans.SupplierLotNo = string.Empty;
                        trans.TransNO = (objPick as Pick).PickNo;
                        trans.TransType = "OUT";
                        trans.Unit = string.Empty;
                        trans.ProcessType = "PACK";
                        _WarehouseFacade.AddInvInOutTrans(trans);


                    }
                    else // �ǵ����ܿ�
                    {
                        // ȡҳ���ϵ�����
                        //���һ�£��������Ϻ��������
                        // �жԱ�tblcartoninvoicesmaterial tblpickdetailmaterial �����ݡ� 
                        #region �ǵ����ܿ�
                        if (string.IsNullOrEmpty(this.txtQTY.Text))
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "������������", this.languageComponent1);
                            return;
                        }
                        //�ж������Ƿ������ָ�ʽ
                        try
                        {
                            decimal _qty = decimal.Parse(qty);
                            if (_qty <= 0)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "��������Ϊ�����������", this.languageComponent1);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "��������Ϊ�����������", this.languageComponent1);
                            return;
                        }

                        object[] objPickdetail = _WarehouseFacade.GetAllPickDetailByPickNoAndDQMCode(pickNo, dqMaterialNO);
                        if (objPickdetail == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "���������ϸ��û�иö������Ϻ�", this.languageComponent1);
                            return;
                        }
                        PickDetail Pickdetail = objPickdetail[0] as PickDetail;
                        Pickdetail.GFHWItemCode = gfHWItemCode;
                        Pickdetail.GFPackingSeq = gfPackingSEQ;
                        object objCartonInvDetail = this._WarehouseFacade.GetCartonInvDetail(carInvNo, cartonNo);
                        if (objCartonInvDetail == null)
                        {
                            CartonInvDetail cartonInvDetail = new CartonInvDetail();
                            cartonInvDetail.CARINVNO = carInvNo;
                            cartonInvDetail.PICKNO = pickNo;
                            cartonInvDetail.STATUS = "Pack";
                            cartonInvDetail.CARTONNO = cartonNo;
                            cartonInvDetail.PACKMCODE = "";
                            cartonInvDetail.PACKQTY = double.Parse(qty);
                            cartonInvDetail.CUSER = mUser;
                            cartonInvDetail.CDATE = mDate;
                            cartonInvDetail.CTIME = mTime;
                            cartonInvDetail.MDATE = mDate;
                            cartonInvDetail.MTIME = mTime;
                            cartonInvDetail.MUSER = mUser;
                            this._WarehouseFacade.AddCartonInvDetail(cartonInvDetail);
                            // 
                            CartonInvDetailMaterial cartonInvDetailMaterial = new CartonInvDetailMaterial();
                            cartonInvDetailMaterial.CARINVNO = carInvNo;
                            cartonInvDetailMaterial.CARTONNO = cartonNo;
                            cartonInvDetailMaterial.CDATE = mDate;
                            cartonInvDetailMaterial.CTIME = mTime;
                            cartonInvDetailMaterial.CUSER = mUser;
                            cartonInvDetailMaterial.DQMCODE = dqMaterialNO;
                            cartonInvDetailMaterial.MCODE = mar.MCode;
                            cartonInvDetailMaterial.MDATE = mDate;
                            cartonInvDetailMaterial.MTIME = mTime;
                            cartonInvDetailMaterial.MUSER = mUser;
                            cartonInvDetailMaterial.PICKLINE = Pickdetail.PickLine;
                            cartonInvDetailMaterial.PICKNO = Pickdetail.PickNo;
                            cartonInvDetailMaterial.QTY = decimal.Parse(qty);
                            cartonInvDetailMaterial.UNIT = Pickdetail.Unit;
                            cartonInvDetailMaterial.GFHWITEMCODE = Pickdetail.GFHWItemCode;
                            cartonInvDetailMaterial.GFPACKINGSEQ = Pickdetail.GFPackingSeq;

                            _WarehouseFacade.AddCartonInvDetailMaterial(cartonInvDetailMaterial);

                        }
                        else
                        {

                            CartonInvDetail CartonInvDetail = objCartonInvDetail as CartonInvDetail;
                            object objCartonInvDetailMaterial = this._WarehouseFacade.QueryCartonInvDetailMaterial(carInvNo, cartonNo, dqMaterialNO);
                            if (objCartonInvDetailMaterial == null)
                            {
                                CartonInvDetailMaterial cartonInvDetailMaterial = new CartonInvDetailMaterial();
                                cartonInvDetailMaterial.CARINVNO = carInvNo;
                                cartonInvDetailMaterial.CARTONNO = cartonNo;
                                cartonInvDetailMaterial.CDATE = mDate;
                                cartonInvDetailMaterial.CTIME = mTime;
                                cartonInvDetailMaterial.CUSER = mUser;
                                cartonInvDetailMaterial.DQMCODE = dqMaterialNO;
                                cartonInvDetailMaterial.MCODE = mar.MCode;
                                cartonInvDetailMaterial.MDATE = mDate;
                                cartonInvDetailMaterial.MTIME = mTime;
                                cartonInvDetailMaterial.MUSER = mUser;
                                cartonInvDetailMaterial.PICKLINE = Pickdetail.PickLine;
                                cartonInvDetailMaterial.PICKNO = Pickdetail.PickNo;
                                cartonInvDetailMaterial.QTY = decimal.Parse(qty);
                                cartonInvDetailMaterial.UNIT = Pickdetail.Unit;
                                cartonInvDetailMaterial.GFHWITEMCODE = Pickdetail.GFHWItemCode;
                                cartonInvDetailMaterial.GFPACKINGSEQ = Pickdetail.GFPackingSeq;

                                _WarehouseFacade.AddCartonInvDetailMaterial(cartonInvDetailMaterial);

                            }
                            else
                            {
                                CartonInvDetailMaterial cartonInvDetailMaterial = objCartonInvDetailMaterial as CartonInvDetailMaterial;
                                cartonInvDetailMaterial.QTY += decimal.Parse(qty);
                                cartonInvDetailMaterial.MUSER = mUser;
                                cartonInvDetailMaterial.MDATE = mDate;
                                cartonInvDetailMaterial.MTIME = mTime;
                                this._WarehouseFacade.UpdateCartonInvDetailMaterial(cartonInvDetailMaterial);
                            }

                            // ���±�CartonInvDetail���԰�����
                            CartonInvDetail cartonInvDetail = objCartonInvDetail as CartonInvDetail;
                            object[] objs_cartondetailmaterial = _WarehouseFacade.GetCartonInvDetailMaterial(carInvNo, cartonNo);
                            if (objs_cartondetailmaterial == null)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "���·��������", this.languageComponent1);
                                return;
                            }
                            double ss = 0;
                            for (int i = 0; i < objs_cartondetailmaterial.Length; i++)
                            {
                                CartonInvDetailMaterial cartondetailmar = objs_cartondetailmaterial[i] as CartonInvDetailMaterial;
                                ss += (double)cartondetailmar.QTY;
                            }
                            cartonInvDetail.PACKQTY = ss;
                            cartonInvDetail.MUSER = mUser;
                            cartonInvDetail.MDATE = mDate;
                            cartonInvDetail.MTIME = mTime;
                            _WarehouseFacade.UpdateCartonInvDetail(cartonInvDetail);

                        }
                        #endregion
                        #region //���� pickdetailmaterial
                        //5>	�����Ѽ�������ϸ��(TBLPICKDetailMaterial)����
                        decimal qTY = decimal.Parse(qty);
                        object[] objPickdetailmaterials = this._WarehouseFacade.QueryPICKDetailMaterials(pickNo, dqMaterialNO);
                        foreach (Pickdetailmaterial _pickdetailmaterial in objPickdetailmaterials)
                        {
                            decimal num = _pickdetailmaterial.Qty - _pickdetailmaterial.PQty;
                            if (num > 0)
                            {
                                if (qTY > num)
                                {
                                    _pickdetailmaterial.PQty = _pickdetailmaterial.Qty;
                                    _pickdetailmaterial.MaintainUser = mUser;
                                    _pickdetailmaterial.MaintainDate = mDate;
                                    _pickdetailmaterial.MaintainTime = mTime;
                                    this._WarehouseFacade.UpdatePickdetailmaterial(_pickdetailmaterial);

                                    qTY -= num;
                                }
                                else
                                {
                                    _pickdetailmaterial.PQty += qTY;
                                    _pickdetailmaterial.MaintainUser = mUser;
                                    _pickdetailmaterial.MaintainDate = mDate;
                                    _pickdetailmaterial.MaintainTime = mTime;
                                    this._WarehouseFacade.UpdatePickdetailmaterial(_pickdetailmaterial);

                                    qTY = 0;

                                    break;
                                }
                            }
                        }
                        if (qTY > 0)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "����İ�װ��������", this.languageComponent1);
                            return;
                        }

                        #endregion
                        #region //���� pickdetail
                        object[] pickdetailmaterial_obj = _WarehouseFacade.GetAllPickDetailMaterialByPickNoAndDQMCode(pickNo, dqMaterialNO);
                        if (pickdetailmaterial_obj == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�����װ��������", this.languageComponent1);
                            return;
                        }
                        decimal sum = 0;
                        for (int i = 0; i < pickdetailmaterial_obj.Length; i++)
                        {
                            Pickdetailmaterial pickdetailmater = pickdetailmaterial_obj[i] as Pickdetailmaterial;
                            sum += pickdetailmater.PQty;
                        }
                        object[] pickdetail_obj = _WarehouseFacade.GetAllPickDetailByPickNoAndDQMCode(pickNo, dqMaterialNO);
                        if (pickdetail_obj == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "���¼����ϸ�����", this.languageComponent1);
                            return;
                        }
                        PickDetail pickdetail = pickdetail_obj[0] as PickDetail;
                        pickdetail.PQTY = sum;
                        if (pickdetail.PQTY == pickdetail.SQTY)
                        {
                            pickdetail.Status = PickDetail_STATUS.Status_ClosePack;
                        }
                        else
                        {
                            pickdetail.Status = PickDetail_STATUS.Status_Pack;
                        }
                        pickdetail.MaintainDate = mDate;
                        pickdetail.MaintainTime = mTime;
                        pickdetail.MaintainUser = mUser;

                        _WarehouseFacade.UpdatePickdetail(pickdetail);
                        #endregion

                        #region  ���װ��ɣ��ı�״̬
                        object objCartonInvDetail1 = this._WarehouseFacade.GetCartonInvDetail(carInvNo, cartonNo);
                        CartonInvDetail car = objCartonInvDetail1 as CartonInvDetail;
                        if (pickdetail.SQTY == (decimal)car.PACKQTY)
                        {
                            car.STATUS = CartonInvoices_STATUS.Status_ClosePack;
                            car.MUSER = mUser;
                            car.MDATE = mDate;
                            car.MTIME = mTime;
                            _WarehouseFacade.UpdateCartonInvDetail(car);
                        }
                        #endregion

                        InvInOutTrans trans = _WarehouseFacade.CreateNewInvInOutTrans();
                        trans.CartonNO = cartonNo;
                        trans.DqMCode = mar.DqmCode; ;
                        trans.FacCode = string.Empty;
                        trans.FromFacCode = string.Empty;
                        trans.FromStorageCode = string.Empty;
                        trans.InvNO = (objPick as Pick).InvNo;//.InvNo;
                        trans.InvType = (objPick as Pick).PickType;
                        trans.LotNo = string.Empty;
                        trans.MaintainDate = mDate;
                        trans.MaintainTime = mTime;
                        trans.MaintainUser = mUser;
                        trans.MCode = mar.MCode;


                        trans.Qty = decimal.Parse(qty); ;
                        // stor.AvailableQty =stor.StorageQty-stor.FreezeQty;


                        trans.ProductionDate = 0;

                        trans.Serial = 0;
                        trans.StorageAgeDate = 0;
                        trans.StorageCode = (objPick as Pick).StorageCode;
                        trans.SupplierLotNo = string.Empty;
                        trans.TransNO = (objPick as Pick).PickNo;
                        trans.TransType = "OUT";
                        trans.Unit = string.Empty;
                        trans.ProcessType = "PACK";
                        _WarehouseFacade.AddInvInOutTrans(trans);

                    }
                    #endregion
                }
                else
                {
                    #region �������
                    ////����������
                    ////1, cartonNo ��tblpickdetailmaterial�� ��picknoΪ��������
                    ////����У��ͽ�tblpickdetailmaterial where carton=��cartonNo�� and pick=�� �ᵽtblcartoninvoicesmaterial��
                    ////��������tblpickdetailmaterialsn�������ݣ�����Щ���ݰᵽtblcartoninvoicesmaterialsn��

                    ////1)	����װ������Ѽ�������ϸ��(TBLPICKDetailMaterial)��Ӧ�����������ϸ��(TBLPICKDETAIL)�еĹ����Ϊ����(TBLPICKDETAIL.GFHWITEMCODE)�������װ���(TBLPICKDETAIL.GFPACKINGSEQ)��༭������ѡ��Ĺ����Ϊ���롢�����װ����Ƿ�ͬ�ȣ��粻����򱨴���ʾ����Ŷ�Ӧѡ��Ĺ����Ϊ���롢�����װ��Ų����
                    //Pickdetailmaterial pickdetailmaterial = objPICKDetailMaterial as Pickdetailmaterial;

                    //object obj_Pickdetail = this._WarehouseFacade.GetPickdetail(pickNo, pickdetailmaterial.Pickline);
                    //PickDetail pickDetail = obj_Pickdetail as PickDetail;
                    //if (pickDetail.GFHWItemCode.Equals(gfHWItemCode) && pickDetail.GFPackingSeq.Equals(gfPackingSEQ))
                    //{

                    //}
                    //else
                    //{
                    //    this.DataProvider.RollbackTransaction();
                    //    WebInfoPublish.Publish(this, "����Ŷ�Ӧѡ��Ĺ����Ϊ���롢�����װ��Ų����", this.languageComponent1);
                    //    return;
                    //}

                    ////2)	���ѡ����������š������Ϊ���롢�����װ����ڷ����䵥��ϸ������Ϣ��(TBLCartonInvDetailMaterial)�ж�Ӧ�����������(TBLCartonInvDetail.CARTONNO)�������Ƿ�С�ڱ༭����������׼�������С���򱨴���ʾѡ��ļ��������š������Ϊ���롢�����װ����Ѱ�װ���׼����Ѿ����ڵ��������׼���
                    //if (this._WarehouseFacade.GetCartonNoCount(pickNo, gfHWItemCode, gfPackingSEQ) < decimal.Parse(suiteQTY))
                    //{
                    //    this.DataProvider.RollbackTransaction();
                    //    WebInfoPublish.Publish(this, "ѡ��ļ��������š������Ϊ���롢�����װ����Ѱ�װ���׼����Ѿ����ڵ��������׼���", this.languageComponent1);
                    //    return;
                    //}

                    //if (pickdetailmaterial.PQty != 0)
                    //{
                    //    this.DataProvider.RollbackTransaction();
                    //    WebInfoPublish.Publish(this, "������Ѳ��ְ�װ,������ʹ��ԭ�����", this.languageComponent1);
                    //    return;
                    //}
                    //else
                    //{
                    //    //1>	���������䵥��ϸ��Ϣ��(TBLCartonInvDetail)����
                    //    CartonInvDetail cartonInvDetail = new CartonInvDetail();
                    //    cartonInvDetail.CARINVNO = carInvNo;
                    //    cartonInvDetail.PICKNO = pickNo;
                    //    cartonInvDetail.STATUS = "ClosePack";
                    //    cartonInvDetail.CARTONNO = cartonNo;
                    //    //cartonInvDetail.PACKMCODE = "";
                    //    //cartonInvDetail.PACKQTY = 0;
                    //    cartonInvDetail.CUSER = mUser;
                    //    cartonInvDetail.CDATE = mDate;
                    //    cartonInvDetail.CTIME = mTime;
                    //    cartonInvDetail.MDATE = mDate;
                    //    cartonInvDetail.MTIME = mTime;
                    //    cartonInvDetail.MUSER = mUser;
                    //    this._WarehouseFacade.AddCartonInvDetail(cartonInvDetail);

                    //    //  this.DataProvider.CommitTransaction();

                    //    //2>	���������䵥��ϸ������Ϣ��(TBLCartonInvDetailMaterial)����
                    //    //object _obj = this._WarehouseFacade.GetPickdetailmaterial(pickNo,cartonNo);
                    //    //if (_obj == null)
                    //    //{
                    //    //    this.DataProvider.RollbackTransaction();
                    //    //    WebInfoPublish.Publish(this, "�����Ѿ�������ϸ����", this.languageComponent1);
                    //    //    return;
                    //    //}
                    //    //Pickdetailmaterial pickDetailmar = _obj as Pickdetailmaterial;
                    //    CartonInvDetailMaterial cartonInvDetailMaterial = new CartonInvDetailMaterial();
                    //    cartonInvDetailMaterial.CARINVNO = carInvNo;
                    //    cartonInvDetailMaterial.PICKNO = pickNo;
                    //    cartonInvDetailMaterial.PICKLINE = pickdetailmaterial.Pickline;
                    //    cartonInvDetailMaterial.CARTONNO = cartonNo;
                    //    cartonInvDetailMaterial.MCODE = pickdetailmaterial.MCode;
                    //    cartonInvDetailMaterial.DQMCODE = pickdetailmaterial.DqmCode;
                    //    cartonInvDetailMaterial.QTY = pickdetailmaterial.Qty;
                    //    cartonInvDetailMaterial.UNIT = pickdetailmaterial.Unit;
                    //    cartonInvDetailMaterial.GFHWITEMCODE = string.Empty;
                    //    cartonInvDetailMaterial.GFPACKINGSEQ = string.Empty;
                    //    cartonInvDetailMaterial.CUSER = mUser;
                    //    cartonInvDetailMaterial.CDATE = mDate;
                    //    cartonInvDetailMaterial.CTIME = mTime;
                    //    cartonInvDetailMaterial.MDATE = mDate;
                    //    cartonInvDetailMaterial.MTIME = mTime;
                    //    cartonInvDetailMaterial.MUSER = mUser;
                    //    this._WarehouseFacade.AddCartonInvDetailMaterial(cartonInvDetailMaterial);

                    //    //3>	���������䵥��ϸSN��Ϣ��(TBLCartonInvDetailSN)����
                    //    object[] objs = this._WarehouseFacade.GetPickDetailMaterialSN(pickNo, cartonNo);
                    //    if (objs == null)
                    //    {
                    //        //this.DataProvider.RollbackTransaction();
                    //        //WebInfoPublish.Publish(this, "�����Ѽ�����SN����", this.languageComponent1);
                    //        //return;
                    //    }
                    //    else
                    //    {
                    //        CARTONINVDETAILSN cartonINVDETAILSN = new CARTONINVDETAILSN();

                    //        foreach (Pickdetailmaterialsn pickdetailmaterialsn in objs)
                    //        {
                    //            cartonINVDETAILSN.CARINVNO = carInvNo;
                    //            cartonINVDETAILSN.PICKNO = pickNo;
                    //            cartonINVDETAILSN.PICKLINE = pickdetailmaterialsn.Pickline;
                    //            cartonINVDETAILSN.CARTONNO = cartonNo;
                    //            cartonINVDETAILSN.SN = pickdetailmaterialsn.Sn;
                    //            cartonINVDETAILSN.MUSER = mUser;
                    //            cartonINVDETAILSN.MDATE = mDate;
                    //            cartonINVDETAILSN.MTIME = mTime;
                    //            this._WarehouseFacade.AddCARTONINVDETAILSN(cartonINVDETAILSN);
                    //        }
                    //    }

                    //    ////6>	�����Ѽ�������ϸ��(TBLPICKDetailMaterial)����
                    //    //pickdetailmaterial.PQty = pickdetailmaterial.Qty;
                    //    //pickdetailmaterial.MaintainUser = mUser;
                    //    //pickdetailmaterial.MaintainDate = mDate;
                    //    //pickdetailmaterial.MaintainTime = mTime;
                    //    //this._WarehouseFacade.UpdatePickdetailmaterial(pickdetailmaterial);

                    //    ////4>	���¼���������ͷ��(TBLPICK)����
                    //    //object objPick = this._InventoryFacade.GetPick(pickNo);
                    //    //if (objPick != null)
                    //    //{
                    //    //    Pick pick = objPick as Pick;
                    //    //    pick.Status = "Pack";
                    //    //    pick.MaintainUser = mUser;
                    //    //    pick.MaintainDate = mDate;
                    //    //    pick.MaintainTime = mTime;
                    //    //    this._InventoryFacade.UpdatePick(pick);
                    //    //}

                    //    #region //���� pickdetailmaterial
                    //    pickdetailmaterial.PQty = pickdetailmaterial.Qty;
                    //    pickdetailmaterial.MaintainDate = mDate;
                    //    pickdetailmaterial.MaintainTime = mTime;
                    //    pickdetailmaterial.MaintainUser = mUser;
                    //    _WarehouseFacade.UpdatePickdetailmaterial(pickdetailmaterial);
                    //    #endregion

                    //    #region //���� pickdetail
                    //    object[] pickdetailmaterial_obj = _WarehouseFacade.GetAllPickDetailMaterialByPickNoAndLine(pickNo, pickdetailmaterial.Pickline);
                    //    if (pickdetailmaterial_obj == null)
                    //    {
                    //        this.DataProvider.RollbackTransaction();
                    //        WebInfoPublish.Publish(this, "�����װ��������", this.languageComponent1);
                    //        return;
                    //    }
                    //    decimal sum = 0;
                    //    for (int i = 0; i < pickdetailmaterial_obj.Length; i++)
                    //    {
                    //        Pickdetailmaterial pickdetailmater = pickdetailmaterial_obj[i] as Pickdetailmaterial;
                    //        sum += pickdetailmater.PQty;
                    //    }
                    //    object pickdetail_obj = _WarehouseFacade.GetPickdetail(pickNo, pickdetailmaterial.Pickline);
                    //    if (pickdetail_obj == null)
                    //    {
                    //        this.DataProvider.RollbackTransaction();
                    //        WebInfoPublish.Publish(this, "���¼����ϸ�����", this.languageComponent1);
                    //        return;
                    //    }
                    //    PickDetail pickdetail = pickdetail_obj as PickDetail;
                    //    pickdetail.PQTY = sum;
                    //    if (pickdetail.PQTY == pickdetail.SQTY)
                    //    {
                    //        pickdetail.Status = PickDetail_STATUS.Status_ClosePack;
                    //    }
                    //    else
                    //    {
                    //        pickdetail.Status = PickDetail_STATUS.Status_Pack;
                    //    }
                    //    pickdetail.MaintainDate = mDate;
                    //    pickdetail.MaintainTime = mTime;
                    //    pickdetail.MaintainUser = mUser;

                    //    _WarehouseFacade.UpdatePickdetail(pickdetail);
                    //    #endregion
                    //}
                    #endregion

                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.Publish(this, "��ʹ������", this.languageComponent1);
                    return;
                }

                object objCARTONINVOICES = this._WarehouseFacade.GetTBLCartonInvoices(carInvNo);
                if (objCARTONINVOICES == null)
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.Publish(this, "�Ҳ������������Ŷ�Ӧ�ķ����䵥ͷ��Ϣ", this.languageComponent1);
                    return;
                }
                CARTONINVOICES _CARTONINVOICES = objCARTONINVOICES as CARTONINVOICES;
                if (_CARTONINVOICES.STATUS != CartonInvoices_STATUS.Status_Pack)
                {
                    _CARTONINVOICES.STATUS = CartonInvoices_STATUS.Status_Pack;
                    _CARTONINVOICES.MUSER = mUser;
                    _CARTONINVOICES.MDATE = mDate;
                    _CARTONINVOICES.MTIME = mTime;
                    this._WarehouseFacade.UpdateTBLCartonInvoices(_CARTONINVOICES);
                }

                //8>	��鵱ǰ�����������ڼ����������ϸ��(TBLPICKDETAIL)�����м�¼SQTY=PQTYʱ�����·����䵥��״̬(TBLCartonInvoices .STATUS)Ϊ��ClosePack:��װ���
                object[] _objs = this._WarehouseFacade.QueryPickDetail(pickNo);
                if (_objs != null)
                {
                    bool isTrue = true;
                    foreach (PickDetail _pickDetail in _objs)
                    {
                        if (_pickDetail.SQTY != _pickDetail.PQTY)
                        {
                            isTrue = false;
                            break;
                        }
                    }
                    if (isTrue)
                    {
                        object _objCARTONINVOICES = this._WarehouseFacade.GetTBLCartonInvoices(carInvNo);
                        if (_objCARTONINVOICES != null)
                        {
                            CARTONINVOICES cARTONINVOICES = objCARTONINVOICES as CARTONINVOICES;
                            cARTONINVOICES.STATUS = "ClosePack";
                            cARTONINVOICES.FDATE = mDate;
                            cARTONINVOICES.FTIME = mTime;
                            cARTONINVOICES.MUSER = mUser;
                            cARTONINVOICES.MDATE = mDate;
                            cARTONINVOICES.MTIME = mTime;
                            this._WarehouseFacade.UpdateTBLCartonInvoices(cARTONINVOICES);



                            InvInOutTrans trans = _WarehouseFacade.CreateNewInvInOutTrans();
                            trans.CartonNO = string.Empty;
                            trans.DqMCode = string.Empty;
                            trans.FacCode = string.Empty;
                            trans.FromFacCode = string.Empty;
                            trans.FromStorageCode = string.Empty;
                            trans.InvNO = (objPick as Pick).InvNo;//.InvNo;
                            trans.InvType = (objPick as Pick).PickType;
                            trans.LotNo = string.Empty;
                            trans.MaintainDate = mDate;
                            trans.MaintainTime = mTime;
                            trans.MaintainUser = this.GetUserCode();
                            trans.MCode = string.Empty;
                            trans.ProductionDate = 0;
                            trans.Qty = 0;
                            trans.Serial = 0;
                            trans.StorageAgeDate = 0;
                            trans.StorageCode = (objPick as Pick).StorageCode;
                            trans.SupplierLotNo = string.Empty;
                            trans.TransNO = (objPick as Pick).PickNo; // asnIqc.IqcNo;
                            trans.TransType = "OUT";
                            trans.Unit = string.Empty;
                            trans.ProcessType = "ClosePack";
                            _WarehouseFacade.AddInvInOutTrans(trans);
                        }
                    }
                }

                this.DataProvider.CommitTransaction();
                WebInfoPublish.Publish(this, "����ɹ�", this.languageComponent1);
                this.gridHelper.RefreshData();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.Publish(this, "����ʧ�ܣ�" + ex.Message, this.languageComponent1);
            }
        }

        protected void cmdPackingFinished_ServerClick(object sender, System.EventArgs e)
        {
            //if (!ValidateInput())
            //{
            //    return;
            //}
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
            string mUser = this.GetUserCode();
            int mDate = dbDateTime.DBDate;
            int mTime = dbDateTime.DBTime;

            string pickNo = this.txtPickNoQuery.Text.Trim().ToUpper();
            object obj = this._WarehouseFacade.GetCartonInvoices(pickNo);





            if (obj == null)
            {
                WebInfoPublish.Publish(this, "��ǰ����������û�ж�Ӧ�ķ����䵥��Ϣ", this.languageComponent1);
                return;
            }

            CARTONINVOICES carvoice = obj as CARTONINVOICES;
            if (carvoice.STATUS != "Release" && carvoice.STATUS != "Pack")
            {
                WebInfoPublish.Publish(this, "�䵥״̬�����ǳ�ʼ�����߰�װ�в��ܲ�����", this.languageComponent1);
                return;
            }

            Pick pick = (Pick)_WarehouseFacade.GetPick(pickNo);
            //������ɼ���
            if (pick != null)
            {
                if (!(pick.Status == PickHeadStatus.PickHeadStatus_Pack || pick.Status == PickHeadStatus.PickHeadStatus_MakePackingList))
                {
                    WebInfoPublish.Publish(this, "������ɼ���", this.languageComponent1);
                    return;
                }
            }
            else
            {
                WebInfoPublish.Publish(this, "�������������", this.languageComponent1);
                return;
            }
            try
            {
                this.DataProvider.BeginTransaction();

                //1. ��ǰ��װ����ڷ����䵥��ϸ��Ϣ��(TBLCartonInvDetail)�е�״̬����Ϊ��ClosePack:��װ���
                _WarehouseFacade.UpdateCartoninvdetailByCARINVNO((obj as CARTONINVOICES).CARINVNO, CartonInvoices_STATUS.Status_ClosePack);
                //object _obj = this._WarehouseFacade.GetCartonInvDetail((obj as CARTONINVOICES).CARINVNO, this.txtCartonNo.Text.Trim().ToUpper());
                //if (_obj == null)
                //{
                //    WebInfoPublish.Publish(this, "��ǰ��װ���û�ж�Ӧ�ķ����䵥��ϸ��Ϣ", this.languageComponent1);
                //    return;
                //}

                //CartonInvDetail cartonInvDetail = _obj as CartonInvDetail;
                //cartonInvDetail.STATUS = CartonInvoices_STATUS.Status_ClosePack;
                //cartonInvDetail.MUSER = mUser;
                //cartonInvDetail.MDATE = mDate;
                //cartonInvDetail.MTIME = mTime;
                //this._WarehouseFacade.UpdateCartonInvDetail(cartonInvDetail);

                //2. ��鵱ǰ�����������ڼ����������ϸ��(TBLPICKDETAIL)�����м�¼SQTY=PQTYʱ�����·����䵥��״̬(TBLCartonInvoices .STATUS)Ϊ��ClosePack:��װ���
                object[] objs = this._WarehouseFacade.GetPickLineByPickNoNotCancel(pickNo);
                if (objs == null)
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.Publish(this, "��ǰ����������û�ж�Ӧ�ļ����������ϸ��Ϣ", this.languageComponent1);
                    return;
                }

                bool isTrue = true;
                foreach (PickDetail pickDetail in objs)
                {
                    if (pickDetail.SQTY != pickDetail.PQTY)
                    {
                        isTrue = false;
                        break;
                    }
                }
                if (isTrue)
                {
                    CARTONINVOICES _CARTONINVOICES = obj as CARTONINVOICES;
                    _CARTONINVOICES.STATUS = CartonInvoices_STATUS.Status_ClosePack;
                    _CARTONINVOICES.FDATE = mDate;
                    _CARTONINVOICES.FTIME = mTime;
                    _CARTONINVOICES.MUSER = mUser;
                    _CARTONINVOICES.MDATE = mDate;
                    _CARTONINVOICES.MTIME = mTime;
                    this._WarehouseFacade.UpdateTBLCartonInvoices(_CARTONINVOICES);
                }
                else
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.Publish(this, "��ǰ�����䵥δ��װ���", this.languageComponent1);
                    return;
                }

                foreach (PickDetail pickDetail in objs)
                {
                    pickDetail.Status = PickDetail_STATUS.Status_ClosePack;
                    pickDetail.MaintainUser = mUser;
                    pickDetail.MaintainDate = mDate;
                    pickDetail.MaintainTime = mTime;
                    _WarehouseFacade.UpdatePickdetail(pickDetail);
                }


                #region ��invinouttrans��������һ������
                WarehouseFacade facade = new WarehouseFacade(base.DataProvider);


                DBDateTime dbTime1 = FormatHelper.GetNowDBDateTime(this.DataProvider);
                foreach (PickDetail pickDetail in objs)
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
                    trans.TransNO = pickDetail.PickNo; // asnIqc.IqcNo;
                    trans.TransType = "OUT";
                    trans.Unit = string.Empty;
                    trans.ProcessType = "ClosePack";
                    facade.AddInvInOutTrans(trans);
                }



                #endregion


                this.DataProvider.CommitTransaction();
                WebInfoPublish.Publish(this, "װ��ɹ�", this.languageComponent1);
                this.gridHelper.RefreshData();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.Publish(this, "װ��ʧ�ܣ�" + ex.Message, this.languageComponent1);
            }
        }

        #endregion

        #region Object <--> Page

        //�󶨹����Ϊ����
        protected void BindGFHWItemCode()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            string pickNo = this.txtPickNoQuery.Text.Trim().ToUpper();
            Pick pick = _WarehouseFacade.GetPick(pickNo) as Pick;
            string[] str = null;
            if (pick != null)
                str = _WarehouseFacade.GetGFHWMcodeByPickNo(pickNo);

            //string[] str = _WarehouseFacade.QueryGFHWItemCode(pickNo);
            this.ddlGFHWItemCode.Items.Clear();
            if (str != null)
            {
                this.ddlGFHWItemCode.Items.Add(string.Empty);
                foreach (string s in str)
                {
                    this.ddlGFHWItemCode.Items.Add(s);
                }
            }
            else
            {
                this.ddlGFHWItemCode.Items.Add(string.Empty);
            }
            this.ddlGFHWItemCode.SelectedIndex = 0;
        }

        //�󶨶������ϱ���
        protected void BindDQMaterialNO()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            string pickNo = this.txtPickNoQuery.Text.Trim().ToUpper();

            string[] str = _WarehouseFacade.QueryDQMaterialNO(pickNo);
            this.drpDQSMCodeEdit.Items.Clear();
            this.ddlDQMaterialNO.Items.Clear();
            if (str != null)
            {
                this.ddlDQMaterialNO.Items.Add(string.Empty);
                foreach (string s in str)
                {
                    this.ddlDQMaterialNO.Items.Add(s);
                }
            }
            else
            {
                this.ddlDQMaterialNO.Items.Add(string.Empty);
            }
            this.ddlDQMaterialNO.SelectedIndex = 0;
        }

        //�����Ϊ���� �ı���������ֵ
        protected void ddlGFHWItemCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            string pickNo = this.txtPickNoQuery.Text.Trim().ToUpper();
            string gfHWItemCode = this.ddlGFHWItemCode.SelectedValue;
            //�󶨹����װ���
            //string[] str = this._WarehouseFacade.QueryGFPackingSEQ(pickNo, gfHWItemCode);//QueryGFSEQ

            string[] str = null;
            Pick pick = _WarehouseFacade.GetPick(pickNo) as Pick;
            if (pick != null)
            {
                string invno = pick.InvNo;
                str = this._WarehouseFacade.QueryGFSEQ(invno, gfHWItemCode);//
            }
            this.ddlGFPackingSEQ.Items.Clear();
            if (str != null)
            {
                this.ddlGFPackingSEQ.Items.Add(string.Empty);
                foreach (string s in str)
                {
                    this.ddlGFPackingSEQ.Items.Add(s);
                }
            }
            else
            {
                this.ddlGFPackingSEQ.Items.Add(string.Empty);
            }
            this.ddlGFPackingSEQ.SelectedIndex = 0;
        }

        protected void ddlDQMaterialNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            string dqMCode = this.ddlDQMaterialNO.SelectedValue;
            object[] mar_objs = _WarehouseFacade.GetMaterialInfoByQDMCode(dqMCode);
            if (mar_objs == null)
            {
                WebInfoPublish.Publish(this, "��Ч�Ķ������Ϻ�", this.languageComponent1);
                return;
            }
            Domain.MOModel.Material mar = mar_objs[0] as Domain.MOModel.Material;
            if (mar.MCONTROLTYPE == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)  //�����ܿ�
            {
                this.txtQTY.Enabled = false;
                this.txtQTY.Text = string.Empty;
                this.txtSNEdit.Enabled = true;
            }
            else
            {
                this.txtQTY.Enabled = true;
                this.txtSNEdit.Enabled = false;
                this.txtSNEdit.Text = string.Empty;
            }

            #region DQSMcode
            this.drpDQSMCodeEdit.Items.Clear();
            string[] str = null;
            string pickNo = this.txtPickNoQuery.Text.Trim().ToUpper();
            Pick pick = _WarehouseFacade.GetPick(pickNo) as Pick;
            if (pick != null)
            {
                string invno = pick.InvNo;
                str = this._WarehouseFacade.QueryGFDqsMcode(invno, dqMCode);//
            }
            if (str != null)
            {
                this.drpDQSMCodeEdit.Items.Add(string.Empty);
                foreach (string s in str)
                {
                    this.drpDQSMCodeEdit.Items.Add(s);
                }
            }
            else
            {
                this.drpDQSMCodeEdit.Items.Add(string.Empty);
            }
            this.drpDQSMCodeEdit.SelectedIndex = 0;
            #endregion

        }

        //�����װ��� �ı���������ֵ
        //protected void ddlGFPackingSEQ_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (_WarehouseFacade == null)
        //    {
        //        _WarehouseFacade = new WarehouseFacade(base.DataProvider);
        //    }
        //    string pickNo = this.txtPickNoQuery.Text.Trim().ToUpper();
        //    string gfHWItemCode = this.ddlGFHWItemCode.SelectedValue;
        //    string gfPackingSEQ = this.ddlGFPackingSEQ.SelectedValue;

        //    string[] str = _WarehouseFacade.QueryDQMaterialNO(pickNo, gfHWItemCode, gfPackingSEQ);
        //    this.ddlDQMaterialNO.Items.Clear();
        //    if (str != null)
        //    {
        //        this.ddlDQMaterialNO.Items.Add(string.Empty);
        //        foreach (string s in str)
        //        {
        //            this.ddlDQMaterialNO.Items.Add(s);
        //        }
        //    }
        //    else
        //    {
        //        this.ddlDQMaterialNO.Items.Add(string.Empty);
        //    }
        //    this.ddlDQMaterialNO.SelectedIndex = 0;
        //}

        protected override bool ValidateInput()
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            PageCheckManager manager = new PageCheckManager();

            manager.Add(new LengthCheck(this.lblPickNoQuery, this.txtPickNoQuery, 40, true));
            manager.Add(new LengthCheck(this.lblGFHWItemCode, this.ddlGFHWItemCode, 40, false));
            manager.Add(new LengthCheck(this.lblGFPackingSEQ, this.ddlGFPackingSEQ, 40, false));
            manager.Add(new LengthCheck(this.lblDQMaterialNO, this.ddlDQMaterialNO, 40, false));
            manager.Add(new LengthCheck(this.lblPackingCartonNo, this.txtCartonNo, 40, true));
            manager.Add(new LengthCheck(this.lblSNEdit, this.txtSNEdit, 40, false));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            return true;
        }

        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                ((PackagingOperations)obj).CARTONNO,
                ((PackagingOperations)obj).DQMCODE,
                ((PackagingOperations)obj).GFHWITEMCODE,
                ((PackagingOperations)obj).GFPACKINGSEQ,
                ((PackagingOperations)obj).SQTY.ToString(),
                ((PackagingOperations)obj).QTY.ToString()
            };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	
                "IT_CartonNo",
                "DQMCode",
                "GFHWItemCode",
                "GFPackingSEQ",
                "PickedQTY",
                "PackingQTY"
            };
        }

        #endregion


        protected void cmdReturn_ServerClick(object sender, EventArgs e)
        {




            string page = Request.QueryString["Page"];
            Response.Redirect(this.MakeRedirectUrl(page,
                                 new string[] { "PickNo", "CARINVNO" },
                                 new string[] { txtPickNoQuery.Text.Trim().ToUpper(),
                                         txtCarInvNoQuery.Text.Trim().ToUpper()
                                        
                                    }));

        }
    }
}
