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
    public partial class FGFPackagingOperationsSN : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private InventoryFacade _InventoryFacade = null;
        private WarehouseFacade _WarehouseFacade = null;
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

                SetQueryTextBox();
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }


        private void SetQueryTextBox()
        {
            this.txtCarInvNoQuery.Text = Request.QueryString["CarInvNo"];
            this.txtCartonNoQuery.Text = Request.QueryString["CartonNo"];
        }

        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("IT_CartonNo", "��װ���", null);
            this.gridHelper.AddColumn("DQMCode", "�������ϱ���", null);
            this.gridHelper.AddColumn("GFHWItemCode", "�����Ϊ����", null);
            this.gridHelper.AddColumn("GFPackingSEQ", "�����װ���", null);
            this.gridHelper.AddColumn("SN", "SN", null);
            this.gridHelper.AddColumn("CarInvNo", "�����䵥��", null);
            this.gridWebGrid.Columns["CarInvNo"].Hidden = true;

            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            this.gridHelper.RequestData();//ҳ�����ʱ��ʼ��grid
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["IT_CartonNo"] = ((PackagingOperations)obj).CARTONNO;
            row["DQMCode"] = ((PackagingOperations)obj).DQMCODE;
            row["GFHWItemCode"] = ((PackagingOperations)obj).GFHWITEMCODE;
            row["GFPackingSEQ"] = ((PackagingOperations)obj).GFPACKINGSEQ;
            row["SN"] = ((PackagingOperations)obj).SN;
            row["CarInvNo"] = ((PackagingOperations)obj).CARINVNO;

            return row;

        }
        #region ɾ��
        protected override object GetEditObject(GridRecord row)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            object obj = _WarehouseFacade.GetCartoninvdetailsn(row.Items.FindItemByKey("CarInvNo").Text,
               row.Items.FindItemByKey("SN").Text);
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
            CARTONINVDETAILSN[] cartoninvdetailsnList = ((CARTONINVDETAILSN[])domainObjects.ToArray(typeof(CARTONINVDETAILSN)));
            try
            {
                this.DataProvider.BeginTransaction();
                foreach (CARTONINVDETAILSN cartoninvdetailsn in cartoninvdetailsnList)
                {
                    //1��ɾ����SN��ͬʱɾ������Ӧ��TBLCARTONINVDETAILMATERIAL,TBLCARTONINVDETAIL��Ϣ��
                    //2�����������ͷ״̬Ϊpick���ҷ����䵥ͷ״̬ΪReleaseʱ�ſ���ɾ����
                    #region delete
                    _WarehouseFacade.DELETECARTONINVDETAILSN(cartoninvdetailsn);
                    Pick pickHead = _InventoryFacade.GetPick(cartoninvdetailsn.PICKNO) as Pick;
                    PickDetail pickDetail = _InventoryFacade.GetPickDetail(cartoninvdetailsn.PICKNO, cartoninvdetailsn.PICKLINE) as PickDetail;
                    //Pickdetailmaterial pickdetailmaterial = _WarehouseFacade.GetPickdetailmaterial(cartonInvDetailMaterial.PICKNO, cartonInvDetailMaterial.CARTONNO) as Pickdetailmaterial;
                    if (pickDetail == null || pickHead == null)
                    {
                        this.DataProvider.RollbackTransaction();
                        return;
                    }
                    object[] objPickdetailmaterials = this._WarehouseFacade.QueryPICKDetailMaterialBydqMCode(cartoninvdetailsn.PICKNO, pickDetail.DQMCode);
                    if (objPickdetailmaterials == null)
                    {
                        this.DataProvider.RollbackTransaction();
                        return;
                    }
                    CARTONINVOICES cartoninvoices = _WarehouseFacade.GetCartoninvoices(cartoninvdetailsn.CARINVNO)
                    as CARTONINVOICES;
                    if (cartoninvoices == null)
                    {
                        this.DataProvider.RollbackTransaction();
                        return;
                    }
                    if (pickHead.Status != PickHeadStatus.PickHeadStatus_WaitPick)
                    {
                        if (
                            !(pickHead.Status == PickHeadStatus.PickHeadStatus_Pick &&
                              cartoninvoices.STATUS == PickDetail_STATUS.Status_Release))
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "���������ͷ״̬Ϊpick���ҷ����䵥ͷ״̬ΪReleaseʱ�ſ���ɾ��", this.languageComponent1);
                            return;
                        }
                    }

                    object[] cartoninvdetailSnList = _WarehouseFacade.GetCartoninvdetailsnByCartonno(cartoninvdetailsn.CARINVNO,
                                                                     cartoninvdetailsn.CARTONNO);
                    if (cartoninvdetailSnList == null)
                    {
                        CartonInvDetailMaterial cartonInvDetailMaterial = _WarehouseFacade.QueryCartonInvDetailMaterial(cartoninvdetailsn.CARINVNO,
                                                                         cartoninvdetailsn.CARTONNO, pickDetail.DQMCode) as CartonInvDetailMaterial;
                        _WarehouseFacade.DeleteCartonInvDetailMaterial(cartonInvDetailMaterial);
                        _WarehouseFacade.DeleteCartoninvdetailByCartonNo(cartoninvdetailsn.CARINVNO, cartoninvdetailsn.CARTONNO);
                    }
                    else
                    {
                        CartonInvDetailMaterial cartonInvDetailMaterial = _WarehouseFacade.QueryCartonInvDetailMaterial(cartoninvdetailsn.CARINVNO,
                                                                               cartoninvdetailsn.CARTONNO, pickDetail.DQMCode) as CartonInvDetailMaterial;
                        if (cartonInvDetailMaterial != null)
                        {
                            cartonInvDetailMaterial.QTY -= 1;
                            _WarehouseFacade.UpdateCartonInvDetailMaterial(cartonInvDetailMaterial);
                        }
                    }

                    pickDetail.PQTY -= 1;
                    _InventoryFacade.UpdatePickDetail(pickDetail);

                    #region Pickdetailmaterial
                    foreach (Pickdetailmaterial _pickdetailmaterial in objPickdetailmaterials)
                    {
                        decimal num = _pickdetailmaterial.PQty;
                        if (num > 0)
                        {
                            _pickdetailmaterial.PQty -= 1;
                            this._WarehouseFacade.UpdatePickdetailmaterial(_pickdetailmaterial);
                            break;
                        }
                    }
                    #endregion
                    //pickdetailmaterial.PQty-= 1;
                    //_WarehouseFacade.UpdatePickdetailmaterial(pickdetailmaterial);

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
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }

            return this._WarehouseFacade.QueryPackagingOperationsSN(
                FormatHelper.CleanString(this.txtCartonNoQuery.Text),
                   FormatHelper.CleanString(txtSNQuery.Text),

                inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            return this._WarehouseFacade.QueryPackagingOperationsSNCount(
                FormatHelper.CleanString(this.txtCartonNoQuery.Text),
                    FormatHelper.CleanString(txtSNQuery.Text)
                );
        }

        #endregion

        #region Button

        protected void cmdReturn_ServerClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("FGFPackagingOperations.aspx"));
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
                ((PackagingOperations)obj).SN
            };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	
                "IT_CartonNo",
                "DQMCode",
                "GFHWItemCode",
                "GFPackingSEQ",
                "SN"
            };
        }

        #endregion

    }
}
