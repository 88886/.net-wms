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
using BenQGuru.eMES.IQC;
using BenQGuru.eMES.Domain.IQC;
using System.Text;


namespace BenQGuru.eMES.Web.IQC
{
    public partial class FIQCCheckListQuery : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private InventoryFacade _InventoryFacade = null;
        private SystemSettingFacade _SystemSettingFacade = null;
        private IQCFacade _IQCFacade = null;

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
            InitHander();
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                InitStorageInASNList();
                this.InitIQCStatusList();
                this.InitStorageInTypeList();
                InitStorageList();
                InitWebGrid();
                this.cmdQuery_Click(null, null);
                this.RequestData();
            }
        }

        #region Ĭ�ϲ�ѯ
        private void RequestData()
        {

            this.pagerToolBar.PageSize = this.pagerSizeSelector.PageSize;
            this.gridHelper.GridBind(PageGridBunding.Page, this.pagerSizeSelector.PageSize);

            this.pagerToolBar.RowCount = GetRowCount();
            this.pagerToolBar.InitPager();
        }
        private void InitHander()
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
            this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadDataSource);
            this.gridHelper.GetRowCountHandle = new GetRowCountDelegateNew(this.GetRowCount);
            this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridRow);

            this.buttonHelper = new ButtonHelper(this);

            this.pagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);


            #region Exporter
            this.excelExporter = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
            this.excelExporter.Page = this;
            this.excelExporter.LanguageComponent = this.languageComponent1;
            this.excelExporter.LoadExportDataHandle = new LoadExportDataDelegate(LoadDataSource);
            this.excelExporter.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatExportRecord);
            this.excelExporter.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetColumnHeaderText);
            #endregion

        }
        private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
        {
            this.gridHelper.GridBind(this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize);
        }

        #endregion
        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        //��ʼ���ָ���������
        /// <summary>
        /// ��ʼ���ָ���������
        /// </summary>
        private void InitStorageInASNList()
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            object[] objASN = _InventoryFacade.GetASNByStatus(ASNHeadStatus.IQC);
            this.drpStorageInASNQuery.Items.Add(new ListItem("", ""));
            if (objASN != null)
            {
                foreach (ASN asn in objASN)
                {
                    this.drpStorageInASNQuery.Items.Add(new ListItem(asn.StNo, asn.StNo));
                }
            }
            this.drpStorageInASNQuery.SelectedIndex = 0;
        }

        //��ʼIQC���鵥״̬������
        /// <summary>
        /// ��ʼIQC���鵥״̬������
        /// </summary>
        private void InitIQCStatusList()
        {
            if (_SystemSettingFacade == null)
            {
                _SystemSettingFacade = new SystemSettingFacade(base.DataProvider);
            }
            object[] parameters = _SystemSettingFacade.GetParametersByParameterGroup("IQCSTATUS");
            this.drpIQCStatusQuery.Items.Add(new ListItem("", ""));
            foreach (Domain.BaseSetting.Parameter parameter in parameters)
            {
                this.drpIQCStatusQuery.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));
            }
            this.drpIQCStatusQuery.SelectedIndex = 0;
        }
        //��ʼ�������������
        /// <summary>
        /// ��ʼ���������������
        /// </summary>
        private void InitStorageInTypeList()
        {
            if (_SystemSettingFacade == null)
            {
                _SystemSettingFacade = new SystemSettingFacade(base.DataProvider);
            }
            object[] parameters = _SystemSettingFacade.GetParametersByParameterGroup("ININVTYPE");
            this.drpStorageInTypeQuery.Items.Add(new ListItem("", GetDrpStorageInTypeEmptyValue()));
            foreach (Domain.BaseSetting.Parameter parameter in parameters)
            {
                if (parameter.ParameterAlias == InInvType.PD)
                {
                    continue;
                }
                this.drpStorageInTypeQuery.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));
            }
            this.drpStorageInTypeQuery.SelectedIndex = 0;
        }
        //��ȡ������Ϳ�ֵʱValue
        /// <summary>
        /// ��ȡ������Ϳ�ֵʱValue
        /// </summary>
        /// <returns></returns>
        private string GetDrpStorageInTypeEmptyValue()
        {
            if (_SystemSettingFacade == null)
            {
                _SystemSettingFacade = new SystemSettingFacade(base.DataProvider);
            }
            string result = string.Empty;
            ArrayList valueList = new ArrayList();
            object[] parameters = _SystemSettingFacade.GetParametersByParameterGroup("ININVTYPE");
            if (parameters != null && parameters.Length > 0)
            {
                foreach (Domain.BaseSetting.Parameter parameter in parameters)
                {
                    if (parameter.ParameterAlias == InInvType.PD)
                    {
                        continue;
                    }
                    valueList.Add(string.Format("'{0}'", parameter.ParameterAlias));
                }
            }
            if (valueList.Count > 0)
            {
                result = string.Join(",", valueList.ToArray(typeof(string)) as string[]);
            }
            return result;
        }

        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("IQCNo", "IQC���鵥��", null);
            this.gridHelper.AddColumn("StorageInASN", "���ָ���", null);
            this.gridHelper.AddColumn("StorageInType", "�������", null);
            this.gridHelper.AddColumn("SAPInvNo", "SAP���ݺ�", null);
            this.gridHelper.AddColumn("STORAGECODE1234", "����λ", null);
            this.gridHelper.AddDataColumn("DQMCode", "�������ϱ���", 120, false, true, HorizontalAlign.Left);
            this.gridHelper.AddColumn("DQMCODEDESC", "������������", null);
            this.gridHelper.AddColumn("VMCode", "��Ӧ�����ϱ���", null);
            this.gridHelper.AddColumn("Status", "״̬", null);
            this.gridHelper.AddDataColumn("IQCStatus", "״̬", true);//������
            this.gridHelper.AddColumn("IQCType", "���鷽ʽ", null);
            this.gridHelper.AddColumn("AQLResult", "AQL���", null);
            this.gridHelper.AddColumn("AppQty", "�ͼ�����", null);
            this.gridHelper.AddColumn("NGQty", "ȱ��Ʒ��", null);



            this.gridHelper.AddColumn("ReturnQty", "�˻�������", null);
            this.gridHelper.AddColumn("ReformQty", "�ֳ���������", null);
            this.gridHelper.AddColumn("GiveQty", "�ò���������", null);
            this.gridHelper.AddColumn("AcceptQty", "�ز�����", null);



            this.gridHelper.AddColumn("VendorNo", "��Ӧ�̴���", null);
            this.gridHelper.AddColumn("AppDate", "�ͼ�����", null);
            this.gridHelper.AddEditColumn("btnInspect", "����");

            this.gridHelper.AddDefaultColumn(true, false);

            //������
            //this.gridHelper.ApplyLanguage(this.languageComponent1);

        }

        protected override DataRow GetGridRow(object obj)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }

            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            DataRow row = this.DtSource.NewRow();

            row["IQCNo"] = ((AsnIQCExt)obj).IqcNo;
            row["StorageInASN"] = ((AsnIQCExt)obj).StNo;
            row["StorageInType"] = this.GetInvInName(((AsnIQCExt)obj).StType);//������ͣ��������ͣ�
            row["SAPInvNo"] = ((AsnIQCExt)obj).InvNo;

            row["STORAGECODE1234"] = ((AsnIQCExt)obj).STORAGECODE;


            row["DQMCode"] = ((AsnIQCExt)obj).DQMCode;

            Domain.MOModel.Material m = (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(((AsnIQCExt)obj).DQMCode);
            if (m != null)
                row["DQMCODEDESC"] = m.MchlongDesc;
            else
                row["DQMCODEDESC"] = string.Empty;


            row["VMCode"] = ((AsnIQCExt)obj).StType == "UB" ? ((AsnIQCExt)obj).CustmCode : ((AsnIQCExt)obj).VendorMCode;
            row["Status"] = this.GetStatusName(((AsnIQCExt)obj).Status);
            row["IQCStatus"] = ((AsnIQCExt)obj).Status;
            row["IQCType"] = FormatHelper.GetChName(((AsnIQCExt)obj).IqcType);

            if (((AsnIQCExt)obj).IqcType == "SpotCheck")
            {
                row["AQLResult"] = FormatHelper.GetChName(((AsnIQCExt)obj).QcStatus);

            }


            row["AppQty"] = ((AsnIQCExt)obj).AppQty;
            row["NGQty"] = ((AsnIQCExt)obj).NgQty;



            string IQCNO = ((AsnIQCExt)obj).IqcNo;
            row["ReturnQty"] = _IQCFacade.ReturnQtyTotalWithIQCNO(IQCNO);
            row["ReformQty"] = _IQCFacade.ReformQtyTotalWithIQCNO(IQCNO);
            row["GiveQty"] = _IQCFacade.GiveQtyTotalWithIQCNO(IQCNO);
            row["AcceptQty"] = _IQCFacade.AcceptQtyTotalWithIQCNO(IQCNO);


            row["VendorNo"] = ((AsnIQCExt)obj).VendorCode;
            row["AppDate"] = FormatHelper.ToDateString(((AsnIQCExt)obj).AppDate, "/");

            return row;

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            UserFacade userFacade = new UserFacade(this.DataProvider);
            string[] usergroupList = userFacade.GetUserGroupCodeofUser1(GetUserCode());
            return this._IQCFacade.QueryAsnIQC1(usergroupList,
                FormatHelper.CleanString(this.drpStorageInASNQuery.SelectedValue),
                      FormatHelper.CleanString(this.txtInvNoQuery.Text),
                this.drpStorageInTypeQuery.SelectedValue,
                FormatHelper.CleanString(this.txtIQCNoQuery.Text),
                FormatHelper.CleanString(this.drpIQCStatusQuery.SelectedValue),
                FormatHelper.TODateInt(this.txtAppCDateQuery.Text),
                FormatHelper.TODateInt(this.txtAppEDateQuery.Text), txtCartonCode.Text.ToUpper(), txtSNQuery.Text.ToUpper(),
                 FormatHelper.CleanString(this.txtDQMCodeQuery.Text), FormatHelper.CleanString(this.txtCusMCodeQuery.Text), drpStorageQuery.SelectedValue,
                inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            UserFacade userFacade = new UserFacade(this.DataProvider);
            string[] usergroupList = userFacade.GetUserGroupCodeofUser1(GetUserCode());
            return this._IQCFacade.QueryAsnIQC1Count(usergroupList,
                FormatHelper.CleanString(this.drpStorageInASNQuery.SelectedValue),
                   FormatHelper.CleanString(this.txtInvNoQuery.Text),
                this.drpStorageInTypeQuery.SelectedValue,
                FormatHelper.CleanString(this.txtIQCNoQuery.Text),
                FormatHelper.CleanString(this.drpIQCStatusQuery.SelectedValue),
                FormatHelper.TODateInt(this.txtAppCDateQuery.Text),
                FormatHelper.TODateInt(this.txtAppEDateQuery.Text), txtCartonCode.Text.ToUpper(), txtSNQuery.Text.ToUpper(),
                    FormatHelper.CleanString(this.txtDQMCodeQuery.Text), FormatHelper.CleanString(this.txtCusMCodeQuery.Text), drpStorageQuery.SelectedValue);
        }

        #endregion

        #region Button

        //���
        protected void cmdStatusSTS_ServerClick(object sender, EventArgs e)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            ArrayList array = this.gridHelper.GetCheckedRows();
            _InventoryFacade = new InventoryFacade(base.DataProvider);
            if (array.Count > 0)
            {
                StringBuilder sbShowMsg = new StringBuilder();
                foreach (GridRecord row in array)
                {
                    string iqcNo = row.Items.FindItemByKey("IQCNo").Value.ToString();
                    AsnIQC iqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);


                    if (iqc.Status != IQCStatus.IQCStatus_Release)
                    {
                        //IQC���鵥��: {0} ״̬���ǳ�ʼ��
                        sbShowMsg.AppendFormat("IQC���鵥��: {0} ״̬���ǳ�ʼ����������� ", iqcNo);
                        continue;
                    }

                    ASN asn1 = (ASN)_InventoryFacade.GetASN(iqc.StNo);
                    if (asn1.Status != ASN_STATUS.ASN_IQC)
                    {
                        WebInfoPublish.Publish(this, asn1.StNo + "���ָ���״̬������IQC���飡", this.languageComponent1);
                        return;
                    }
                    //���
                    try
                    {
                        this.DataProvider.BeginTransaction();
                        ToSTS(iqcNo);
                        this.DataProvider.CommitTransaction();
                    }
                    catch (Exception ex)
                    {

                        sbShowMsg.AppendFormat("IQC���鵥��: {0} {1}", iqcNo, ex.Message);
                        this.DataProvider.RollbackTransaction();
                        continue;
                    }

                }
                if (sbShowMsg.Length > 0)
                {
                    string showMsg = sbShowMsg.ToString();
                    WebInfoPublish.Publish(this, showMsg, this.languageComponent1);

                }
                else
                {
                    WebInfoPublish.Publish(this, "���ɹ�", this.languageComponent1);
                }
                this.gridHelper.RequestData();//ˢ��ҳ��
            }

        }

        //Grid�е����ť
        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            if (commandName == "btnInspect")
            {

                string iqcNo = row.Items.FindItemByKey("IQCNo").Text.Trim();
                AsnIQC iqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
                if (iqc.Status == IQCStatus.IQCStatus_Cancel)
                {
                    WebInfoPublish.Publish(this, "IQC���鵥��ȡ����", this.languageComponent1);
                    return;
                }
                else if (iqc.Status == IQCStatus.IQCStatus_Release)
                {
                    //���¼��鵥״̬ΪWaitCheck
                    AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
                    asnIqc.Status = IQCStatus.IQCStatus_WaitCheck;
                    _IQCFacade.UpdateAsnIQC(asnIqc);
                }
                Response.Redirect(this.MakeRedirectUrl("FIQCCheckResultMP.aspx", new string[] { "IQCNo" }, new string[] { iqcNo }));
            }
        }
        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {


            _IQCFacade = new IQCFacade(base.DataProvider);



            string IQCNO = ((AsnIQCExt)obj).IqcNo;



            _InventoryFacade = new InventoryFacade(this.DataProvider);

            string desc = string.Empty;
            Domain.MOModel.Material m = (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(((AsnIQCExt)obj).DQMCode);
            if (m != null)
                desc = m.MchlongDesc;



            return new string[]{((AsnIQCExt)obj).IqcNo,
                                ((AsnIQCExt)obj).StNo,
                                this.GetInvInName(((AsnIQCExt)obj).StType),
                                ((AsnIQCExt)obj).InvNo,
                                ((AsnIQCExt)obj).STORAGECODE,
                                ((AsnIQCExt)obj).DQMCode,
                                desc,
                                 ((AsnIQCExt)obj).StType == "UB" ? ((AsnIQCExt)obj).CustmCode : ((AsnIQCExt)obj).VendorMCode,
                                this.GetStatusName(((AsnIQCExt)obj).Status),
                                 FormatHelper.GetChName(((AsnIQCExt)obj).IqcType),
                                ((AsnIQCExt)obj).QcStatus == "Y" ? "�ϸ�" : (((AsnIQCExt)obj).QcStatus == "N" ? "���ϸ�" : ""),
                                ((AsnIQCExt)obj).AppQty.ToString(),
                                ((AsnIQCExt)obj).NgQty.ToString(),
                                _IQCFacade.ReturnQtyTotalWithIQCNO(IQCNO).ToString(),
                                _IQCFacade.ReformQtyTotalWithIQCNO(IQCNO).ToString(),
                                _IQCFacade.GiveQtyTotalWithIQCNO(IQCNO).ToString(),
                                 _IQCFacade.AcceptQtyTotalWithIQCNO(IQCNO).ToString(),
                                ((AsnIQCExt)obj).VendorCode,
                                FormatHelper.ToDateString(((AsnIQCExt)obj).AppDate,"/")
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"IQCNo",
                                    "StorageInASN",
                                    "StorageInType",
                                    "SAPInvNo",
                                    "STORAGECODE1234",
                                    "DQMCode",
                                    "DQMCODEDESC",
                                    "VMCode",	
                                    "Status",
                                    "IQCType",	
                                    "AQLResult",
                                    "AppQty",
                                    "NGQty",
                                    "ReturnQty",
                                    "ReformQty",
                                    "GiveQty",
                                    "AcceptQty",

                                    "VendorNo",
                                    "AppDate"};
        }

        #endregion

        #region Method

        //���
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="iqcNo">IQC���鵥��</param>
        private void ToSTS(string iqcNo)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            _InventoryFacade = new InventoryFacade(base.DataProvider);

            //1�������ͼ쵥TBLASNIQC
            AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
            if (asnIqc != null)
            {
                asnIqc.IqcType = IQCType.IQCType_ExemptCheck;
                asnIqc.Status = IQCStatus.IQCStatus_IQCClose;
                asnIqc.QcStatus = "Y";
                _IQCFacade.UpdateAsnIQC(asnIqc);

                #region ��invinouttrans��������һ������
                WarehouseFacade facade = new WarehouseFacade(base.DataProvider);

                //ASN asn = (ASN)domainObject;
                DBDateTime dbTime1 = FormatHelper.GetNowDBDateTime(this.DataProvider);
                InvInOutTrans trans = facade.CreateNewInvInOutTrans();
                trans.CartonNO = string.Empty;
                trans.DqMCode = asnIqc.DQMCode;
                trans.FacCode = string.Empty;
                trans.FromFacCode = string.Empty;
                trans.FromStorageCode = string.Empty;
                trans.InvNO = asnIqc.InvNo;
                trans.InvType = asnIqc.IqcType;
                trans.LotNo = string.Empty;
                trans.MaintainDate = dbTime1.DBDate;
                trans.MaintainTime = dbTime1.DBTime;
                trans.MaintainUser = this.GetUserCode();
                trans.MCode = asnIqc.MCode;
                trans.ProductionDate = 0;
                trans.Qty = asnIqc.Qty;
                trans.Serial = 0;
                trans.StorageAgeDate = 0;
                trans.StorageCode = string.Empty;
                trans.SupplierLotNo = string.Empty;
                trans.TransNO = asnIqc.IqcNo;
                trans.TransType = "IN";
                trans.Unit = string.Empty;
                trans.ProcessType = "IQC";
                facade.AddInvInOutTrans(trans);
                #endregion
            }

            object[] objAsnIqcDetail = _IQCFacade.GetAsnIQCDetailByIqcNo(iqcNo);
            if (objAsnIqcDetail != null)
            {
                foreach (AsnIQCDetail asnIqcDetail in objAsnIqcDetail)
                {
                    //2�������ͼ쵥��ϸTBLASNIQCDETAIL
                    asnIqcDetail.QcPassQty = asnIqcDetail.Qty;
                    asnIqcDetail.QcStatus = "Y";
                    _IQCFacade.UpdateAsnIQCDetail(asnIqcDetail);


                    //4������ASN��ϸTBLASNDETAIL
                    ASNDetail asnDetail = (ASNDetail)_InventoryFacade.GetASNDetail(Convert.ToInt32(asnIqcDetail.StLine), asnIqcDetail.StNo);
                    asnDetail.QcPassQty = asnDetail.ReceiveQty;
                    asnDetail.Status = IQCStatus.IQCStatus_IQCClose;
                    _InventoryFacade.UpdateASNDetail(asnDetail);


                    //5������ASN��ϸ��Ӧ��������ϸTBLASNDETAILITEM
                    object[] objAsnDetaileItem = _InventoryFacade.GetAsnDetailItem(asnIqcDetail.StNo, Convert.ToInt32(asnIqcDetail.StLine));
                    if (objAsnDetaileItem != null)
                    {
                        foreach (Asndetailitem asnDetaileItem in objAsnDetaileItem)
                        {
                            asnDetaileItem.QcpassQty = asnDetaileItem.ReceiveQty;
                            _InventoryFacade.UpdateAsndetailitem(asnDetaileItem);
                        }
                    }

                }
            }

            object[] objAsnIqcDetailSN = _IQCFacade.GetAsnIqcDetailSNByIqcNo(iqcNo);
            if (objAsnIqcDetailSN != null)
            {
                foreach (AsnIqcDetailSN asnIqcDetailSN in objAsnIqcDetailSN)
                {
                    //3�������ͼ쵥��ϸSNTBLASNIQCDETAILSN
                    asnIqcDetailSN.QcStatus = "Y";
                    _IQCFacade.UpdateAsnIqcDetailSN(asnIqcDetailSN);

                    //6������ASN��ϸSN TBLASNDETAILSN
                    Asndetailsn asnDetailSn = (Asndetailsn)_InventoryFacade.GetAsndetailsn(asnIqcDetailSN.Sn, asnIqcDetailSN.StNo, Convert.ToInt32(asnIqcDetailSN.StLine));
                    if (asnDetailSn != null)
                    {
                        asnDetailSn.QcStatus = "Y";
                        _InventoryFacade.UpdateAsndetailsn(asnDetailSn);
                    }
                }
            }

            //7�����ϱ����ݸ�����ɺ���ASN��ϸ��(TBLASNDETAIL)�����м�¼״̬Ϊ��IQCClose:IQC��� or OnLocation:�ϼ� or Close:��� or Cancel:ȡ��ʱ��
            //   ����ASN����(TBLASN)״̬(TBLASN.STATUS)Ϊ��OnLocation:�ϼ�
            bool isAllIQCClose = CheckAllASNDetailIsIQCClose(iqcNo);
            bool isAllOnLocation = CheckAllASNDetailIsOnLocation(iqcNo);
            bool isAllClose = CheckAllASNDetailIsClose(iqcNo);
            bool isAllCancel = CheckAllASNDetailIsCancel(iqcNo);

            if (isAllIQCClose || isAllOnLocation ||
                isAllClose || isAllCancel
                )
            {
                ASN asn = (ASN)_InventoryFacade.GetASN(asnIqc.StNo);
                if (asn != null)
                {
                    asn.Status = ASNHeadStatus.OnLocation;
                    _InventoryFacade.UpdateASN(asn);
                }
            }
        }

        //���ASN��ϸ������״̬ΪIQCClose
        /// <summary>
        /// ���ASN��ϸ������״̬ΪIQCClose
        /// </summary>
        /// <param name="iqcNo">IQC���鵥��</param>
        /// <returns>ȫ����IQCClose��true;����false</returns>
        private bool CheckAllASNDetailIsIQCClose(string iqcNo)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            _InventoryFacade = new InventoryFacade(base.DataProvider);

            AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
            if (asnIqc != null)
            {
                object[] objAsnDetail = _InventoryFacade.GetASNDetailByStNo(asnIqc.StNo);
                if (objAsnDetail != null)
                {
                    foreach (ASNDetail asnDetail in objAsnDetail)
                    {
                        if (asnDetail.Status != ASNLineStatus.IQCClose)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //���ASN��ϸ������״̬ΪOnLocation
        /// <summary>
        /// ���ASN��ϸ������״̬ΪOnLocation
        /// </summary>
        /// <param name="iqcNo">IQC���鵥��</param>
        /// <returns>ȫ����OnLocation��true;����false</returns>
        private bool CheckAllASNDetailIsOnLocation(string iqcNo)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            _InventoryFacade = new InventoryFacade(base.DataProvider);

            AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
            if (asnIqc != null)
            {
                object[] objAsnDetail = _InventoryFacade.GetASNDetailByStNo(asnIqc.StNo);
                if (objAsnDetail != null)
                {
                    foreach (ASNDetail asnDetail in objAsnDetail)
                    {
                        if (asnDetail.Status != ASNLineStatus.OnLocation)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //���ASN��ϸ������״̬ΪClose
        /// <summary>
        /// ���ASN��ϸ������״̬ΪClose
        /// </summary>
        /// <param name="iqcNo">IQC���鵥��</param>
        /// <returns>ȫ����Close��true;����false</returns>
        private bool CheckAllASNDetailIsClose(string iqcNo)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            _InventoryFacade = new InventoryFacade(base.DataProvider);

            AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
            if (asnIqc != null)
            {
                object[] objAsnDetail = _InventoryFacade.GetASNDetailByStNo(asnIqc.StNo);
                if (objAsnDetail != null)
                {
                    foreach (ASNDetail asnDetail in objAsnDetail)
                    {
                        if (asnDetail.Status != ASNLineStatus.Close)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //���ASN��ϸ������״̬ΪCancel
        /// <summary>
        /// ���ASN��ϸ������״̬ΪCancel
        /// </summary>
        /// <param name="iqcNo">IQC���鵥��</param>
        /// <returns>ȫ����Cancel��true;����false</returns>
        private bool CheckAllASNDetailIsCancel(string iqcNo)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            _InventoryFacade = new InventoryFacade(base.DataProvider);

            AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
            if (asnIqc != null)
            {
                object[] objAsnDetail = _InventoryFacade.GetASNDetailByStNo(asnIqc.StNo);
                if (objAsnDetail != null)
                {
                    foreach (ASNDetail asnDetail in objAsnDetail)
                    {
                        if (asnDetail.Status != ASNLineStatus.Cancel)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #endregion


        private void InitStorageList()
        {
            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            UserFacade userFacade = new UserFacade(this.DataProvider);
            string[] usergroupList = userFacade.GetUserGroupCodeofUser(GetUserCode());//+TOSTORAGE
            this.drpStorageQuery.Items.Add(new ListItem("", ""));

            object[] parameters = systemSettingFacade.GetDistinctParaInParameterGroup(usergroupList);
            if (parameters != null)
            {
                foreach (Domain.BaseSetting.Parameter parameter in parameters)
                {
                    drpStorageQuery.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterCode));

                }
            }
            this.drpStorageQuery.SelectedIndex = 0;

        }
    }
}
