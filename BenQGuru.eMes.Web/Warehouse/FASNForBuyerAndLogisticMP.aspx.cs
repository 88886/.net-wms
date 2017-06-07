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
//using System.Linq;

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
using BenQGuru.eMES.MOModel;
using System.Text;
using System.IO;
using System.Collections.Generic;
using BenQGuru.eMES.SAPRFCService;
using BenQGuru.eMES.SAPRFCService.Domain;
using BenQGuru.eMES.Common;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FASNForBuyerAndLogisticMP : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private InventoryFacade _InventoryFacade = null;
        private SystemSettingFacade _SystemSettingFacade = null;
        private WarehouseFacade facade = null;
        private InventoryFacade _Invenfacade = null;
        private WarehouseFacade _WarehouseFacade = null;
        private bool _RedirectFlag = false;//ҳ����ת��ʶ
        private bool _IsAdd = false;//������ʶ
        private static string storageType = string.Empty;


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
        protected void Page_Init(object sender, System.EventArgs e)
        {
            PostBackTrigger tri = new PostBackTrigger();
            tri.ControlID = this.cmdEnter.ID;
            (this.FindControl("up1") as UpdatePanel).Triggers.Add(tri);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {




            InitHander();


            #region ע��
            //if (_InventoryFacade == null)
            //{
            //    _InventoryFacade = new InventoryFacade(base.DataProvider);
            //}
            //string asnNo = FormatHelper.CleanString(this.txtStorageInASNEdit.Text);
            //if (string.IsNullOrEmpty(asnNo))
            //{
            //    WebInfoPublish.PublishInfo(this, "���ָ���Ϊ��", this.languageComponent1);
            //    return;
            //}
            //object[] objs = _InventoryFacade.GetFileUpLoad(asnNo, "DirectSign");
            //if (objs != null)
            //{
            //    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            //    string path = Server.MapPath(this.VirtualHostRoot + "InvDoc/");
            //    foreach (InvDoc doc in objs)
            //    {
            //        aFileDownLoad.HRef = string.Format(@"{0}InvDoc\{1}", this.VirtualHostRoot, doc.ServerFileName);
            //        ////info.WorkingDirectory = path;
            //        //info.FileName = path.Replace('/', '\\') + doc.ServerFileName;
            //        //info.Arguments = "";
            //        //info.UseShellExecute = false;
            //        //try
            //        //{
            //        //    System.Diagnostics.Process.Start(info);
            //        //}
            //        //catch (System.ComponentModel.Win32Exception we)
            //        //{
            //        //    WebInfoPublish.PublishInfo(this, we.Message, this.languageComponent1);
            //        //    return;
            //        //}
            //    }
            //}
            //else
            //{
            //    WebInfoPublish.PublishInfo(this, "û�и���", this.languageComponent1);
            //    return;
            //} 
            #endregion
            string invNo = string.Empty;
            string stNo = string.Empty;
            storageType = string.Empty;


            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                this.InitStorageInTypeList();
                this.InitStatusList();
                this.InitVendorCodeList();

                invNo = Request.QueryString["InvNo"];
                storageType = Request.QueryString["StorageInType"];
                //drpVendorCodeEdit.Enabled = false;//add by sam
                if (!string.IsNullOrEmpty(invNo) && !string.IsNullOrEmpty(storageType))
                {
                    _RedirectFlag = true;
                    this.SetInvNoAndStorageTypeByRequestParam(invNo, storageType);
                    cmdCreat_ServerClick(null, null);//ҳ����ת�Զ����������ָ��š�

                }


                if (!string.IsNullOrEmpty(storageType))
                {
                    drpStorageInTypeEdit.SelectedValue = storageType;
                }

                stNo = Request.QueryString["StNo"];

                InitWebGrid();





                this.InitStorageInList();
                this.InitControlsStatus();



                ChangeCblItmeEnabled();


                if (!string.IsNullOrEmpty(stNo))
                {

                    this.txtStorageInASNQuery.Text = stNo;

                    txtStorageInASNEdit.Text = stNo;

                    _InventoryFacade = new InventoryFacade(this.DataProvider);
                    ASN asn = (ASN)_InventoryFacade.GetASN(stNo);
                    if (asn == null)
                        throw new Exception(stNo + "���ָ��Ų����ڣ�");
                    drpStorageInTypeEdit.Text = asn.StType;
                    this.txtStorageInASNEdit.Text = asn.StNo;
                    drpStorageInEdit.Text = asn.StorageCode;
                    return;
                }
            }


            if (drpStorageInTypeEdit.SelectedValue != "SCTR")
            {
                drpVendorCodeEdit.Enabled = false;
                drpVendorCodeEdit.SelectedIndex = 0;
            }
            else
                drpVendorCodeEdit.Enabled = true;
            this.ViewState["stNo"] = stNo;
            this.ViewState["InvNo"] = invNo;
            this.ViewState["StorageInType"] = storageType;
        }

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

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        //��ʼ���������������
        /// <summary>
        /// ��ʼ���������������
        /// </summary>
        private void InitStorageInTypeList()
        {
            if (_SystemSettingFacade == null)
            {
                _SystemSettingFacade = new SystemSettingFacade(base.DataProvider);
            }
            //if (_RedirectFlag)
            //{
            //this.drpStorageInTypeQuery.Items.Add(new ListItem("", GetDrpStorageInTypeEmptyValue(_RedirectFlag)));
            this.drpStorageInTypeQuery.Items.Clear();
            this.drpStorageInTypeEdit.Items.Clear();
            this.drpStorageInTypeQuery.Items.Add(new ListItem("", ""));//edit by sam 
            this.drpStorageInTypeEdit.Items.Add(new ListItem("", ""));
            object[] parameters1 = _SystemSettingFacade.GetParametersByParameterGroup("INVINTYPE");
            object[] parameters2 = _SystemSettingFacade.GetParametersByParameterGroup("INVINTYPE1");
            if (string.IsNullOrEmpty(this.GetRequestParam("InvNo")))
            {
                if (parameters1 != null && parameters1.Length > 0)
                {
                    foreach (Domain.BaseSetting.Parameter parameter in parameters1)
                    {
                        this.drpStorageInTypeQuery.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));
                    }



                }

                if (parameters2 != null && parameters2.Length > 0)
                {
                    foreach (Domain.BaseSetting.Parameter parameter in parameters2)
                    {
                        this.drpStorageInTypeEdit.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));
                    }
                }
            }
            else
            {
                if (parameters1 != null && parameters1.Length > 0)
                {
                    foreach (Domain.BaseSetting.Parameter parameter in parameters1)
                    {
                        this.drpStorageInTypeQuery.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));
                        this.drpStorageInTypeEdit.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));

                    }
                }

            }
            //}
            //else
            //{
            //    //��ѯ����
            //    this.drpStorageInTypeQuery.Items.Add(new ListItem("", GetDrpStorageInTypeEmptyValue(_RedirectFlag)));
            //    this.drpStorageInTypeQuery.Items.Add(new ListItem("PGI����", InInvType.PGIR));
            //    this.drpStorageInTypeQuery.Items.Add(new ListItem("��������", InInvType.SCTR));

            //    //�༭����
            //    this.drpStorageInTypeEdit.Items.Add(new ListItem("", ""));
            //    this.drpStorageInTypeEdit.Items.Add(new ListItem("PGI����", InInvType.PGIR));
            //    this.drpStorageInTypeEdit.Items.Add(new ListItem("��������",InInvType.SCTR));
            //}

            this.drpStorageInTypeQuery.SelectedIndex = 0;
            if (string.IsNullOrEmpty(storageType))
            {
                this.drpStorageInTypeEdit.SelectedIndex = 0;
            }
            else
            {
                this.drpStorageInTypeEdit.SelectedIndex = this.drpStorageInTypeEdit.Items.IndexOf(this.drpStorageInTypeEdit.Items.FindByValue(storageType));
            }
            //drpStorageInTypeEdit.Enabled = true;
        }

        //��ȡ������Ϳ�ֵʱValue
        /// <summary>
        /// ��ȡ������Ϳ�ֵʱValue
        /// </summary>
        /// <param name="isRedirect">�Ƿ�ҳ����ת</param>
        /// <returns></returns>
        private string GetDrpStorageInTypeEmptyValue(bool isRedirect)
        {
            if (isRedirect)
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
                        valueList.Add(string.Format("'{0}'", parameter.ParameterAlias));
                    }
                }
                if (valueList.Count > 0)
                {
                    result = string.Join(",", valueList.ToArray(typeof(string)) as string[]);
                }
                return result;
            }
            return string.Format("'{0}','{1}'", InInvType.PGIR, InInvType.SCTR);
        }

        //��ʼ��״̬������
        /// <summary>
        /// ��ʼ��״̬������
        /// </summary>
        private void InitStatusList()
        {
            if (_SystemSettingFacade == null)
            {
                _SystemSettingFacade = new SystemSettingFacade(base.DataProvider);
            }
            this.drpStatusQuery.Items.Add(new ListItem("", ""));

            object[] parameters = _SystemSettingFacade.GetParametersByParameterGroup("ASNHEADSTATUS");
            if (parameters != null && parameters.Length > 0)
            {
                foreach (Domain.BaseSetting.Parameter parameter in parameters)
                {
                    this.drpStatusQuery.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));

                }
            }
            this.drpStatusQuery.SelectedIndex = 0;
        }

        //��Ӧ�̴���������
        /// <summary>
        /// ��Ӧ�̴���������
        /// </summary>
        private void InitVendorCodeList()
        {
            if (_SystemSettingFacade == null)
            {
                _SystemSettingFacade = new SystemSettingFacade(base.DataProvider);
            }
            object[] parameters = _SystemSettingFacade.GetParametersByParameterGroup("VENDORCODE");
            if (parameters == null)
            {
                WebInfoPublish.PublishInfo(this, "�������ù�Ӧ�̴���", this.languageComponent1);
                // WebInfoPublish.Publish(this, "$Error_ASN_NO_DATA", this.languageComponent1);
                return;
            }
            this.drpVendorCodeEdit.Items.Add(new ListItem("", ""));
            foreach (Domain.BaseSetting.Parameter parameter in parameters)
            {
                this.drpVendorCodeEdit.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));
            }
            this.drpVendorCodeEdit.SelectedIndex = 0;
        }

        //��ʼ������λ������
        /// <summary>
        /// ��ʼ������λ
        /// </summary>
        private void InitStorageInList()
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            this.drpStorageInEdit.Items.Clear();
            this.drpStorageInEdit.Items.Add(new ListItem("", ""));
            object[] objStorage = _InventoryFacade.GetStorage("SAP");
            if (objStorage != null && objStorage.Length > 0)
            {
                foreach (Storage storage in objStorage)
                {
                    this.drpStorageInEdit.Items.Add(new ListItem(storage.StorageCode + "-" + storage.StorageName, storage.StorageCode));
                }
            }
            string invNo = FormatHelper.CleanString(this.txtSAPInvNoEdit.Text);
            if (!string.IsNullOrEmpty(invNo))
            {
                InvoicesDetail invoicesDetail = (InvoicesDetail)_InventoryFacade.GetInvoicesDetail(invNo);
                if (invoicesDetail != null)
                {
                    this.drpStorageInEdit.SelectedValue = invoicesDetail.StorageCode;
                }
            }
            else
            {
                this.drpStorageInEdit.SelectedIndex = 0;
            }
            //drpStorageInTypeEdit.Enabled = true;
        }

        //����QueryString��ʼ��SAP���ݺź��������
        /// <summary>
        /// ����QueryString��ʼ��SAP���ݺź��������
        /// </summary>
        /// <param name="invNo"></param>
        /// <param name="storageType"></param>
        private void SetInvNoAndStorageTypeByRequestParam(string invNo, string storageType)
        {
            if (_RedirectFlag)
            {
                //��ѯ����
                this.txtInvNoQuery.Text = invNo;
                //this.drpStorageInTypeQuery.SelectedValue = storageType;

                //�༭����
                this.txtSAPInvNoEdit.Text = invNo;
                //this.drpStorageInTypeEdit.SelectedValue = storageType;

                //this.txtSAPInvNoEdit.Text = invNo;
                //this.drpStorageInTypeEdit.SelectedValue = storageType;
                this.drpStorageInTypeEdit.Enabled = false;

                //�ı临ѡ������״̬
                ChangeCblItmeEnabled();
            }
        }

        //��ʼ��ҳ��ؼ�״̬
        /// <summary>
        /// ��ʼ��ҳ��ؼ�״̬
        /// </summary>
        private void InitControlsStatus()
        {
            this.txtStorageInASNEdit.Enabled = false;//���ָ���
            this.txtSAPInvNoEdit.Enabled = false;//SAP���ݺ�
            this.cmdReturn.Visible = false;//���ذ�ť
            if (_RedirectFlag)
            {
                this.cmdReturn.Visible = true;
            }

            //����ؼ��Ƿ�����
            if (this.drpStorageInTypeEdit.SelectedValue == InInvType.POR)
            {
                this.FileImport.Disabled = false;
                this.cmdEnter.Disabled = false;
            }
            //if (this.cblFlag.Items[0].Selected)
            //{
            //    this.FileImport.Disabled = false;
            //    this.cmdEnter.Disabled = false;
            //}
            //else
            //{
            //    this.FileImport.Disabled = true;
            //    this.cmdEnter.Disabled = true;
            //}
        }

        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            #region ע��
            //this.gridHelper.AddColumn("ASN", "���ָ���", null);
            //this.gridHelper.AddColumn("CDate", "��������", null);
            //this.gridHelper.AddColumn("CUser", "������", null);
            //this.gridHelper.AddColumn("StorageInType", "�������", null);
            //this.gridHelper.AddColumn("SAPInvNo", "SAP���ݺ�", null);
            //this.gridHelper.AddColumn("Status", "״̬", null);
            //this.gridHelper.AddLinkColumn("LinkToCartonImport", "����/�鿴�䵥", null);
            //this.gridHelper.AddDefaultColumn(true, true);
            //this.gridHelper.AddColumn("StorageInCode", "����λ", null);
            //this.gridHelper.AddColumn("PredictDate", "Ԥ�Ƶ�������", null);
            //this.gridHelper.AddColumn("DirectFlag", "��Ӧ��ֱ����ʶ", null);
            //this.gridHelper.AddColumn("PickNo", "����������", null);
            //this.gridHelper.AddColumn("VendorCode", "��Ӧ�̴���", null);
            //this.gridHelper.AddColumn("ExigencyFlag", "������ʶ", null);
            //this.gridHelper.AddColumn("RejectsFlag", "���������벻��Ʒ���ʶ", null);
            //this.gridHelper.AddColumn("OANo", "OA��ˮ��", null);
            //this.gridHelper.AddColumn("PackingListNo", "��Ӧ��װ���", null);
            //this.gridHelper.AddColumn("ProvideDate", "�䵥��������", null);
            //this.gridHelper.AddColumn("GrossWeight", "ë��", null);
            //this.gridHelper.AddColumn("Volume", "���", null);
            //this.gridHelper.AddColumn("FromStorageCode", "�����λ", null);
            //this.gridHelper.AddColumn("Remark1", "��ע", null); 
            #endregion
            if (this.SAPHeadViewFieldList.Length > 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    this.gridHelper.AddColumn(this.SAPHeadViewFieldList[i].FieldName, /*this.languageComponent1.GetString(*/this.SAPHeadViewFieldList[i].Description/*)*/, null);
                }
                this.gridHelper.AddLinkColumn("LinkToCartonImport", "����/�鿴�䵥", null);
                this.gridHelper.AddLinkColumn("ASNDetail", "ASN����", null);

                this.gridHelper.AddDefaultColumn(true, true);
                for (int i = 6; i < this.SAPHeadViewFieldList.Length; i++)
                {
                    this.gridHelper.AddColumn(this.SAPHeadViewFieldList[i].FieldName, /*this.languageComponent1.GetString(*/this.SAPHeadViewFieldList[i].Description/*)*/, null);
                }
            }
            else
            {
                for (int i = 0; i < this.SAPHeadViewFieldList.Length; i++)
                {
                    this.gridHelper.AddColumn(this.SAPHeadViewFieldList[i].FieldName, /*this.languageComponent1.GetString(*/this.SAPHeadViewFieldList[i].Description/*)*/, null);
                }
                this.gridHelper.AddLinkColumn("LinkToCartonImport", "����/�鿴�䵥", null);
                this.gridHelper.AddLinkColumn("ASNDetail", "ASN����", null);

                this.gridHelper.AddDefaultColumn(true, true);
            }

            if (SAPHeadViewFieldList.Length > 0)
            {
                this.gridHelper.AddDataColumn("ASNCreateTime", "���ָ���ʱ��", 20);
                this.gridHelper.AddDataColumn("ReformCount", "�ֳ���������", 20);
                this.gridHelper.AddDataColumn("ReturnCount", "�˻�������", 20);
                this.gridHelper.AddDataColumn("RejectCount", "�����������", 20);

                this.gridHelper.AddDataColumn("SendDownTime", "�·�ʱ��", 20);
                this.gridHelper.AddColumn("ReceiveOkTime", "�������ʱ��", 20);
                this.gridHelper.AddColumn("IQCOKTIME", "IQC���ʱ��", 20);
                this.gridHelper.AddColumn("INSTORAGETIME", "������ʱ��", 20);

            }
            //������
            //this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.RequestData();
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            #region ע��
            //row["ASN"] = ((ASN)obj).StNo;
            //row["CDate"] = FormatHelper.ToDateString(((ASN)obj).CDate);
            //row["CUser"] = ((ASN)obj).CUser;
            //row["StorageInType"] =this.GetInvInName(((ASN)obj).StType);
            //row["SAPInvNo"] = ((ASN)obj).InvNo;
            //row["Status"] = this.GetStatusName(((ASN)obj).Status);
            //row["StorageInCode"] = ((ASN)obj).StorageCode;
            //row["PredictDate"] = FormatHelper.ToDateString(((ASN)obj).PreictDate);
            //row["DirectFlag"] = ((ASN)obj).DirectFlag;
            //row["PickNo"] = ((ASN)obj).PickNo;
            //row["VendorCode"] = ((ASN)obj).VendorCode;
            //row["ExigencyFlag"] = ((ASN)obj).ExigencyFlag;
            //row["RejectsFlag"] = ((ASN)obj).RejectsFlag;
            //row["OANo"] = ((ASN)obj).OANo;
            //row["PackingListNo"] = ((ASN)obj).PackingListNo;
            //row["ProvideDate"] =  FormatHelper.ToDateString(((ASN)obj).ProvideDate);
            //row["GrossWeight"] = ((ASN)obj).GrossWeight;
            //row["Volume"] = ((ASN)obj).Volume;
            //row["FromStorageCode"] = ((ASN)obj).FromStorageCode;
            //row["Remark1"] = ((ASN)obj).Remark1;

            #endregion
            ASN inv = obj as ASN;
            Type type = inv.GetType();
            _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            _Invenfacade = new InventoryFacade(base.DataProvider);
            if (this.SAPHeadViewFieldList.Length > 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    ViewField field = this.SAPHeadViewFieldList[i];
                    string strValue = string.Empty;
                    System.Reflection.FieldInfo fieldInfo = type.GetField(field.FieldName);
                    if (fieldInfo != null)
                    {
                        strValue = fieldInfo.GetValue(inv).ToString();
                    }
                    if (field.FieldName == "CDate")
                    {
                        strValue = FormatHelper.ToDateString(inv.CDate);

                    }
                    else if (field.FieldName == "StType")
                    {
                        strValue = this.GetInvInName(inv.StType);
                    }
                    else if (field.FieldName == "Status")
                    {
                        strValue = languageComponent1.GetString(inv.Status);
                    }
                    else if (field.FieldName == "PredictDate")
                    {
                        strValue = FormatHelper.ToDateString(inv.PreictDate);
                    }
                    else if (field.FieldName == "ProvideDate")
                    {
                        strValue = inv.ProvideDate;// FormatHelper.ToDateString(inv.ProvideDate);
                    }
                    else if (field.FieldName == "InitReceiveDesc")
                    {
                        if (!string.IsNullOrEmpty(strValue))
                            strValue = _Invenfacade.GetRejectDESC(strValue);// FormatHelper.ToDateString(inv.ProvideDate);
                    }
                    //PlanSendDate
                    row[i + 1] = strValue;
                }
                for (int i = 6; i < this.SAPHeadViewFieldList.Length; i++)
                {
                    ViewField field = this.SAPHeadViewFieldList[i];
                    string strValue = string.Empty;
                    System.Reflection.FieldInfo fieldInfo = type.GetField(field.FieldName);
                    if (fieldInfo != null)
                    {
                        strValue = fieldInfo.GetValue(inv).ToString();
                    }
                    else if (field.FieldName == "StType")
                    {
                        strValue = this.GetInvInName(inv.StType);
                    }
                    if (field.FieldName == "CDate")
                    {
                        strValue = FormatHelper.ToDateString(inv.CDate);
                    }
                    else if (field.FieldName == "StorageInType")
                    {
                        strValue = this.GetInvInName(inv.StType);
                    }
                    else if (field.FieldName == "Status")
                    {
                        strValue = this.GetStatusName(inv.Status);
                    }
                    else if (field.FieldName == "PredictDate")
                    {
                        strValue = FormatHelper.ToDateString(inv.PreictDate);
                    }
                    else if (field.FieldName == "ProvideDate")
                    {
                        strValue = inv.ProvideDate;//  FormatHelper.ToDateString(inv.ProvideDate);
                    }
                    else if (field.FieldName == "InitReceiveDesc")
                    {
                        if (!string.IsNullOrEmpty(strValue))
                            strValue = _Invenfacade.GetRejectDESC(strValue);// FormatHelper.ToDateString(inv.ProvideDate);
                    }
                    //PlanSendDate
                    row[i + 5] = strValue;
                }
            }
            else
            {
                for (int i = 0; i < this.SAPHeadViewFieldList.Length; i++)
                {
                    ViewField field = this.SAPHeadViewFieldList[i];
                    string strValue = string.Empty;
                    System.Reflection.FieldInfo fieldInfo = type.GetField(field.FieldName);
                    if (fieldInfo != null)
                    {
                        strValue = fieldInfo.GetValue(inv).ToString();
                    }
                    if (field.FieldName == "CDate")
                    {
                        strValue = FormatHelper.ToDateString(inv.CDate);

                    }
                    else if (field.FieldName == "StorageInType")
                    {
                        strValue = this.GetInvInName(inv.StType);
                    }
                    else if (field.FieldName == "Status")
                    {
                        strValue = this.GetStatusName(inv.Status);
                    }
                    else if (field.FieldName == "PredictDate")
                    {
                        strValue = FormatHelper.ToDateString(inv.PreictDate);
                    }
                    else if (field.FieldName == "ProvideDate")
                    {
                        strValue = inv.ProvideDate;//  FormatHelper.ToDateString(inv.ProvideDate);
                    }
                    else if (field.FieldName == "InitReceiveDesc")
                    {
                        if (!string.IsNullOrEmpty(strValue))
                            strValue = _Invenfacade.GetRejectDESC(strValue);// FormatHelper.ToDateString(inv.ProvideDate);
                    }
                    //PlanSendDate
                    row[i + 1] = strValue;
                }
            }
            if (this.SAPHeadViewFieldList.Length > 0)
            {


                facade = new WarehouseFacade(base.DataProvider);

                Asn asn = (Asn)facade.GetAsn(txtStorageInASNQuery.Text.Trim().ToUpper());
                if (asn != null)
                {
                    string createTime = asn.CDate.ToString() + " " + FormatHelper.ToTimeString(asn.CTime);
                    row["ASNCreateTime"] = createTime;

                }

                BenQGuru.eMES.IQC.IQCFacade iqcFacade = new BenQGuru.eMES.IQC.IQCFacade(base.DataProvider);
                row["ReformCount"] = iqcFacade.ReformQtyTotalWithStNo(inv.StNo);
                row["ReturnCount"] = iqcFacade.ReturnQtyTotalWithStNo(inv.StNo);
                string status = inv.Status;

                int rejectCount = 0;
                Asndetail[] asndetails = facade.GetAsnDetails(inv.StNo);

                if (status != ASNHeadStatus.Release && status != ASNHeadStatus.WaitReceive && status != ASNHeadStatus.Receive)
                {
                    foreach (Asndetail d in asndetails)
                    {
                        rejectCount += (d.Qty - d.ReceiveQty);
                    }
                }

                row["RejectCount"] = rejectCount;
                row["SendDownTime"] = _WarehouseFacade.GetProcessPhaseTime1(((ASN)obj).StNo, "ISSUE");
                row["ReceiveOkTime"] = _WarehouseFacade.GetProcessPhaseTime1(((ASN)obj).StNo, "Receive");
                row["IQCOKTIME"] = _WarehouseFacade.GetProcessPhaseTime1(((ASN)obj).StNo, "IQC");
                row["INSTORAGETIME"] = _WarehouseFacade.GetProcessPhaseTime1(((ASN)obj).StNo, "INSTORAEE");
            }
            return row;

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }

            #region
            string chk = "";
            if (chbInINVQuery.Checked)
            { chk += "'" + ASNHeadStatus.Close + "',"; }
            if (chbIQCQuery.Checked)
            { chk += "'" + ASNHeadStatus.IQC + "',"; }
            if (chbIQCRejectionQuery.Checked)
            { chk += "'" + ASNHeadStatus.IQCRejection + "',"; }
            if (chbOnLocationQuery.Checked)
            { chk += "'" + ASNHeadStatus.OnLocation + "',"; }

            if (chbInReceiveQuery.Checked)
            { chk += "'" + ASNHeadStatus.Receive + "',"; }
            if (chbReceiveRejectionQuery.Checked)
            { chk += "'" + ASNHeadStatus.ReceiveRejection + "',"; }
            if (chbinReleaseQuery.Checked)
            { chk += "'" + ASNHeadStatus.Release + "',"; }
            if (chbWaitReceiveQuery.Checked)
            { chk += "'" + ASNHeadStatus.WaitReceive + "',"; }
            #endregion
            return this._InventoryFacade.QueryASN(
                FormatHelper.CleanString(this.txtStorageInASNQuery.Text),
                this.drpStorageInTypeQuery.SelectedValue,
                FormatHelper.CleanString(this.txtInvNoQuery.Text),
                FormatHelper.CleanString(this.txtCreateUserQuery.Text),
                chk,
                //FormatHelper.CleanString(this.drpStatusQuery.SelectedValue),
                FormatHelper.TODateInt(this.txtCBDateQuery.Text),
                FormatHelper.TODateInt(this.txtCEDateQuery.Text),
                chbIsHasRejectionQuery.Checked,
                inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            #region
            string chk = "";
            if (chbInINVQuery.Checked)
            { chk += "'" + ASNHeadStatus.Close + "',"; }
            if (chbIQCQuery.Checked)
            { chk += "'" + ASNHeadStatus.IQC + "',"; }
            if (chbIQCRejectionQuery.Checked)
            { chk += "'" + ASNHeadStatus.IQCRejection + "',"; }
            if (chbOnLocationQuery.Checked)
            { chk += "'" + ASNHeadStatus.OnLocation + "',"; }

            if (chbInReceiveQuery.Checked)
            { chk += "'" + ASNHeadStatus.Receive + "',"; }
            if (chbReceiveRejectionQuery.Checked)
            { chk += "'" + ASNHeadStatus.ReceiveRejection + "',"; }
            if (chbinReleaseQuery.Checked)
            { chk += "'" + ASNHeadStatus.Release + "',"; }
            if (chbWaitReceiveQuery.Checked)
            { chk += "'" + ASNHeadStatus.WaitReceive + "',"; }
            #endregion
            return this._InventoryFacade.QueryASNCount(
                FormatHelper.CleanString(this.txtStorageInASNQuery.Text),
                this.drpStorageInTypeQuery.SelectedValue,
                FormatHelper.CleanString(this.txtInvNoQuery.Text),
                FormatHelper.CleanString(this.txtCreateUserQuery.Text),
                chk,

                //FormatHelper.CleanString(this.drpStatusQuery.SelectedValue),
                FormatHelper.TODateInt(this.txtCBDateQuery.Text),
                FormatHelper.TODateInt(this.txtCEDateQuery.Text),
                   chbIsHasRejectionQuery.Checked
                );
        }

        #endregion

        #region Button

        //������� �ı���������ֵ
        protected void drpStorageInTypeEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeCblItmeEnabled();
        }
        /// <summary>
        ///�ı临ѡ������״̬
        /// </summary>
        private void ChangeCblItmeEnabled()
        {
            ////this.cblFlag.Items[0].Enabled = true;
            ////this.cblFlag.Items[1].Enabled = true;
            ////this.cblFlag.Items[2].Enabled = true;

            //#region add by sam 
            ////��Ӧ��ֱ�����ָ�ѡ��ֻ�е����������PO���ʱ���Ϊ����״̬�������������Ϊ������״̬��
            ////��������Ʒ��⸴ѡ��ֻ���������Ϊ�����������ʱΪ����״̬�������������Ϊ������״̬��
            ////�������Ϊ�����������ʱ����Ӧ�̴���������Ϊ��ѡ����Ҫ��ɫ��ʾ�������������Ϊ������״̬
            //this.cblFlag.Items[0].Enabled = false;
            //this.cblFlag.Items[1].Enabled = false;
            //this.cblFlag.Items[2].Enabled = false;
            string storageInType = this.drpStorageInTypeEdit.SelectedValue;
            //drpVendorCodeEdit.Enabled = false;
            //lblVendorCodeEdit.ForeColor = System.Drawing.Color.Black;
            //if (storageInType == InInvType.POR)
            //{
            //    this.cblFlag.Items[0].Enabled = true;
            //}
            //else if (storageInType == InInvType.SCTR)//��������
            //{
            //    this.cblFlag.Items[2].Enabled = true;
            //    drpVendorCodeEdit.Enabled = true;
            //    lblVendorCodeEdit.ForeColor = System.Drawing.Color.Red;
            //}
            //#endregion

            #region
            this.cblFlag.Items[0].Selected = false;
            this.cblFlag.Items[1].Selected = false;
            this.cblFlag.Items[2].Selected = false;

            this.cblFlag.Items[0].Enabled = false;
            this.cblFlag.Items[1].Enabled = true;
            this.cblFlag.Items[2].Enabled = false;
            if (storageInType == InInvType.POR)
            {
                this.cblFlag.Items[0].Enabled = true;
            }
            else if (storageInType == InInvType.SCTR)
            {
                this.cblFlag.Items[2].Enabled = true;
            }
            ////���ѡ���з����ı�ؼ�����״̬
            //else if (storageType == InInvType.YFR)
            //{
            //    this.drpVendorCodeEdit.Enabled = false;
            //    this.txtPickNoEdit.Enabled = false;
            //    this.cmdInStorage.Disabled = true;

            //    this.cblFlag.Items[0].Enabled = false;
            //    this.cblFlag.Items[2].Enabled = false;
            //}
            //else
            //{
            //    this.drpVendorCodeEdit.Enabled = true;
            //    this.txtPickNoEdit.Enabled = true;
            //    this.cmdInStorage.Disabled = false;
            //}
            #endregion
        }

        //��ѡ�� ѡ��ı�
        protected void cblFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cblFlag.Items[0].Selected)
            {
                this.FileImport.Disabled = false;
                this.cmdEnter.Disabled = false;
            }
            else
            {
                this.FileImport.Disabled = true;
                this.cmdEnter.Disabled = true;
            }
        }

        //���Grid�а�ť
        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            string asn = row.Items.FindItemByKey("StNo").Text.Trim();

            if (commandName == "LinkToCartonImport")
            {
                Response.Redirect(this.MakeRedirectUrl("FCartonDataImpMP.aspx",
                                    new string[] { "ASN", "Page" },
                                    new string[] { asn, "FASNForBuyerAndLogisticMP.aspx" }));
            }
            else if (commandName == "ASNDetail")
            {
                Response.Redirect(this.MakeRedirectUrl("FExecuteASNDetailMP.aspx",
                              new string[] { "ASN", "Page" },
                              new string[] { asn, "FASNForBuyerAndLogisticMP.aspx" }));
            }

        }

        //����
        protected void cmdCreat_ServerClick(object sender, System.EventArgs e)
        {
            WarehouseFacade warehouseFacade = new WarehouseFacade(base.DataProvider);
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
            string preFix = "IN" + dbDateTime.DBDate.ToString().Substring(2);
            object objSerialBook = warehouseFacade.GetSerialBook(preFix);
            bool iss = false;
            if (objSerialBook == null)
            {
                SERIALBOOK serialBook = new SERIALBOOK();
                serialBook.SNPrefix = preFix;
                serialBook.MaxSerial = "1";
                serialBook.MUser = this.GetUserCode();
                serialBook.MDate = dbDateTime.DBDate;
                serialBook.MTime = dbDateTime.DBTime;
                try
                {
                    warehouseFacade.AddSerialBook(serialBook);
                    this.txtStorageInASNEdit.Text = preFix + "0001";
                    iss = true;
                }
                catch (Exception ex)
                {

                    WebInfoPublish.Publish(this, "����ʧ�ܣ�" + ex.Message, this.languageComponent1);
                }
            }
            else
            {
                SERIALBOOK serialBook = (SERIALBOOK)objSerialBook;
                if (serialBook.MaxSerial == "9999")
                {
                    WebInfoPublish.Publish(this, "��������ָ������ã�", this.languageComponent1);
                    return;
                }
                serialBook.MaxSerial = (Convert.ToInt32(serialBook.MaxSerial) + 1).ToString();
                try
                {
                    warehouseFacade.UpdateSerialBook(serialBook);
                    this.txtStorageInASNEdit.Text = serialBook.SNPrefix + serialBook.MaxSerial.PadLeft(4, '0');
                    iss = true;
                }
                catch (Exception ex)
                {

                    WebInfoPublish.Publish(this, "����ʧ�ܣ�" + ex.Message, this.languageComponent1);
                }
            }
            if (iss)
            {
                cmdCreat.Disabled = true;
            }
        }

        //����
        protected override void UpdateDomainObject(object domainObject)
        {
            //if (this.drpStorageInTypeEdit.SelectedValue == InInvType.POR || this.drpStorageInTypeEdit.SelectedValue == InInvType.CAR || this.drpStorageInTypeEdit.SelectedValue == InInvType.BLR || this.drpStorageInTypeEdit.SelectedValue == InInvType.JCR || this.drpStorageInTypeEdit.SelectedValue == InInvType.PGIR || this.drpStorageInTypeEdit.SelectedValue == InInvType.SCTR)
            //{
            //    if (string.IsNullOrEmpty(this.txtExpectedArrivalDateEdit.Text))
            //    {
            //        WebInfoPublish.Publish(this, "�������Ϊ��PO��⡢CLAIM��⡢����Ʒ��⡢��ⷵ����⡢ PGI���ϡ���������ʱԤ�Ƶ�������Ϊ����ѡ��", this.languageComponent1);
            //        return;
            //    }
            //}
            //if (string.IsNullOrEmpty(this.drpStorageInEdit.SelectedValue))
            //{
            //    WebInfoPublish.Publish(this, "����λ����ѡ��", this.languageComponent1);
            //    return;
            //}

            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            WarehouseFacade facade = new WarehouseFacade(base.DataProvider);
            if (_IsAdd)
            {
                try
                {
                    this.DataProvider.BeginTransaction();

                    if (((ASN)domainObject).StType == SAP_ImportType.SAP_PGIR)
                        autoCreateASNFromCheckoutInfo((ASN)domainObject, ((ASN)domainObject).PickNo);
                    #region ��invinouttrans��������һ������
                    ASN asn = (ASN)domainObject;
                    DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                    InvInOutTrans trans = facade.CreateNewInvInOutTrans();
                    trans.CartonNO = string.Empty;
                    trans.DqMCode = string.Empty;
                    trans.FacCode = asn.FacCode;
                    trans.FromFacCode = asn.FromFacCode;
                    trans.FromStorageCode = asn.FromStorageCode;
                    trans.InvNO = asn.InvNo;
                    trans.InvType = asn.StType;
                    trans.LotNo = string.Empty;
                    trans.MaintainDate = dbTime.DBDate;
                    trans.MaintainTime = dbTime.DBTime;
                    trans.MaintainUser = this.GetUserCode();
                    trans.MCode = string.Empty;
                    trans.ProductionDate = 0;
                    trans.Qty = 0;
                    trans.Serial = 0;
                    trans.StorageAgeDate = 0;
                    trans.StorageCode = asn.StorageCode;
                    trans.SupplierLotNo = string.Empty;
                    trans.TransNO = asn.StNo;
                    trans.TransType = "IN";
                    trans.Unit = string.Empty;
                    trans.ProcessType = "ASN";
                    facade.AddInvInOutTrans(trans);
                    
                    Invoices inv = (Invoices)_InventoryFacade.GetInvoices(asn.InvNo);
                    if (inv != null)
                        asn.ReWorkApplyUser = inv.ReworkApplyUser;
                    if (inv != null && string.IsNullOrEmpty(asn.VendorCode))
                        asn.VendorCode = inv.VendorCode;


                    #endregion
                    this._InventoryFacade.AddASN((ASN)domainObject);
                    this.DataProvider.CommitTransaction();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    Log.Error(ex.StackTrace);
                    this.DataProvider.RollbackTransaction();
                    throw ex;
                }
            }
            else
            {
                this._InventoryFacade.UpdateASN((ASN)domainObject);
            }

            this.ViewState["InvNo"] = string.Empty;
            this.ViewState["StorageInType"] = string.Empty;

            this.ViewState["stNo"] = string.Empty;
        }

        private void autoCreateASNFromCheckoutInfo(ASN asn, string pickNo)
        {
            try
            {
                if (facade == null)
                {
                    facade = new WarehouseFacade(base.DataProvider);
                }
                CARTONINVOICES[] cartos = facade.GetGrossAndWeight(pickNo);


                if (cartos.Length > 0)
                {
                    asn.GrossWeight = (decimal)cartos[0].GROSS_WEIGHT;
                    asn.Volume = cartos[0].VOLUME;
                }



                CartonInvDetailMaterial[] cartonMs = facade.GetCartonInvDetailMaterial(pickNo);
                MOModel.ItemFacade _itemfacade = new BenQGuru.eMES.MOModel.ItemFacade(this.DataProvider);
                string message;
                if (IsCartonnoHaveTwoMCodes(cartonMs, out message)) { WebInfoPublish.Publish(this, message, this.languageComponent1); return; }



                int i = 1;

                foreach (CartonInvDetailMaterial m in cartonMs)
                {
                    object materobj = _itemfacade.GetMaterial(m.MCODE);
                    Domain.MOModel.Material mater = materobj as Domain.MOModel.Material;
                    if (materobj == null)
                    {
                        throw new Exception(m.MCODE + "���ϲ�����");

                    }

                    if (mater.MCONTROLTYPE == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                    {
                        ASNDetail asnd = new ASNDetail();
                        asnd.ActQty = 0;

                        asnd.DQMCode = m.DQMCODE;
                        asnd.Qty = (int)m.QTY;
                        asnd.StLine = i.ToString();

                        asnd.MCode = m.MCODE;
                        asnd.Status = "Release";
                        asnd.StNo = txtStorageInASNEdit.Text;
                        asnd.MDesc = mater.MchshortDesc;
                        asnd.Unit = m.UNIT;
                        asnd.CDate = FormatHelper.TODateInt(DateTime.Now);
                        asnd.CTime = FormatHelper.TOTimeInt(DateTime.Now);
                        asnd.CUser = GetUserCode();
                        asnd.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
                        asnd.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                        asnd.MaintainUser = GetUserCode();
                        asnd.LotNo = " ";
                        CARTONINVDETAILSN[] sns = facade.GetCartonInvDetailSn(m.CARTONNO, pickNo);


                        List<string> snList = new List<string>();

                        foreach (CARTONINVDETAILSN sn in sns)
                        {
                            snList.Add(sn.SN);
                        }
                        if (sns.Length > 0)
                        {

                            Pickdetailmaterial detail = facade.GetFirstCheckInFromPickMaterial(pickNo, snList);
                            if (detail != null)
                            {
                                asnd.ProductionDate = detail.Production_Date;
                                asnd.SupplierLotNo = detail.Supplier_lotno;
                                asnd.LotNo = detail.Lotno;
                                asnd.StorageAgeDate = detail.StorageageDate;
                            }
                        }

                        DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                        Asndetailitem detailitem = facade.CreateNewAsndetailitem();
                        detailitem.CDate = dbTime.DBDate;
                        detailitem.CTime = dbTime.DBTime;
                        detailitem.CUser = this.GetUserCode();
                        detailitem.MaintainDate = dbTime.DBDate;
                        detailitem.MaintainTime = dbTime.DBTime;
                        detailitem.MaintainUser = this.GetUserCode();
                        detailitem.Stline = i.ToString();
                        detailitem.Stno = asn.StNo;
                        detailitem.MCode = asnd.MCode;
                        detailitem.DqmCode = asnd.DQMCode;
                        detailitem.Qty = asnd.Qty;
                        detailitem.Invline = asnd.StLine;
                        detailitem.Invno = asnd.StNo;
                        detailitem.ActQty = detailitem.Qty;
                        detailitem.QcpassQty = detailitem.Qty;
                        detailitem.ReceiveQty = detailitem.Qty;
                        facade.AddAsndetailitem(detailitem);


                        foreach (CARTONINVDETAILSN sn in sns)
                        {

                            Asndetailsn asnSN = new Asndetailsn();

                            asnSN.CDate = FormatHelper.TODateInt(DateTime.Now);
                            asnSN.CTime = FormatHelper.TOTimeInt(DateTime.Now);
                            asnSN.CUser = GetUserCode();
                            asnSN.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
                            asnSN.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                            asnSN.MaintainUser = GetUserCode();
                            asnSN.Sn = sn.SN;
                            asnSN.Stline = i.ToString();
                            asnSN.Stno = asn.StNo;
                            facade.AddAsndetailsn(asnSN);
                        }
                        _InventoryFacade.AddASNDetail(asnd);
                    }
                    else if (mater.MCONTROLTYPE == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_LOT)
                    {
                        ASNDetail asnd = new ASNDetail();
                        asnd.ActQty = 0;

                        asnd.DQMCode = m.DQMCODE;
                        asnd.Qty = (int)m.QTY;
                        asnd.StLine = i.ToString();
                        asnd.ActQty = 0;
                        asnd.MCode = m.MCODE;
                        asnd.StNo = asn.StNo;
                        asnd.Status = "Release";
                        asnd.MDesc = mater.MchshortDesc;
                        asnd.Unit = m.UNIT;
                        asnd.CDate = FormatHelper.TODateInt(DateTime.Now);
                        asnd.CTime = FormatHelper.TOTimeInt(DateTime.Now);
                        asnd.CUser = GetUserCode();
                        asnd.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
                        asnd.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                        asnd.MaintainUser = GetUserCode();
                        asnd.LotNo = " ";


                        Pickdetailmaterial pickMaterial = facade.GetFirstCheckInPickMaterialFromDQMCode(m.DQMCODE);
                        if (pickMaterial != null)
                        {
                            asnd.ProductionDate = pickMaterial.Production_Date;
                            asnd.SupplierLotNo = pickMaterial.Supplier_lotno;
                            asnd.LotNo = pickMaterial.Lotno;
                            asnd.StorageAgeDate = pickMaterial.StorageageDate;
                        }
                        _InventoryFacade.AddASNDetail(asnd);

                        DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                        Asndetailitem detailitem = facade.CreateNewAsndetailitem();
                        detailitem.CDate = dbTime.DBDate;
                        detailitem.CTime = dbTime.DBTime;
                        detailitem.CUser = this.GetUserCode();
                        detailitem.MaintainDate = dbTime.DBDate;
                        detailitem.MaintainTime = dbTime.DBTime;
                        detailitem.MaintainUser = this.GetUserCode();
                        detailitem.Stline = i.ToString();
                        detailitem.Stno = asn.StNo;
                        detailitem.MCode = asnd.MCode;
                        detailitem.DqmCode = asnd.DQMCode;
                        detailitem.Qty = asnd.Qty;
                        detailitem.Invline = asnd.StLine;
                        detailitem.Invno = asnd.StNo;
                        detailitem.ActQty = detailitem.Qty;
                        detailitem.QcpassQty = detailitem.Qty;
                        detailitem.ReceiveQty = detailitem.Qty;
                        facade.AddAsndetailitem(detailitem);


                        CARTONINVDETAILSN[] sns = facade.GetCartonInvDetailSn(m.CARTONNO, pickNo);
                        foreach (CARTONINVDETAILSN sn in sns)
                        {

                            Asndetailsn asnSN = new Asndetailsn();

                            asnSN.CDate = FormatHelper.TODateInt(DateTime.Now);
                            asnSN.CTime = FormatHelper.TOTimeInt(DateTime.Now);
                            asnSN.CUser = GetUserCode();
                            asnSN.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
                            asnSN.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                            asnSN.MaintainUser = GetUserCode();
                            asnSN.Sn = sn.SN;
                            asnSN.Stline = i.ToString();
                            asnSN.Stno = asn.StNo;
                            facade.AddAsndetailsn(asnSN);
                        }
                    }
                    i++;
                }
                asn.InvNo = txtStorageInASNEdit.Text;


                this.DataProvider.CommitTransaction();


            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                throw ex;
            }
        }

        private bool IsCartonnoHaveTwoMCodes(CartonInvDetailMaterial[] cartonMs, out string message)
        {
            List<string> cartonno = new List<string>();
            message = string.Empty;
            foreach (CartonInvDetailMaterial mm in cartonMs)
            {
                if (!cartonno.Contains(mm.CARTONNO))
                    cartonno.Add(mm.CARTONNO);
                else
                {
                    message = mm.CARTONNO + "����������ϣ������µ��䵥";
                    return true;
                }
            }
            return false;
        }

        //ɾ��
        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            WarehouseFacade _warehouseFacade = new WarehouseFacade(base.DataProvider);
            object[] list = domainObjects.ToArray();
            bool isRelease = false;
            foreach (ASN asn in list)
            {
                if (asn.Status != Status.Release)
                {
                    isRelease = true;
                }
            }
            if (isRelease)
            {
                WebInfoPublish.Publish(this, "�·��󲻿���ɾ��", this.languageComponent1);
                return;
            }
            this.DataProvider.BeginTransaction();
            try
            {
                this._InventoryFacade.DeleteASN1(domainObjects.ToArray(typeof(ASN)) as ASN[]);
                foreach (ASN asn in domainObjects.ToArray(typeof(ASN)) as ASN[])
                {
                    object[] objs_asnd = _InventoryFacade.GetASNDetailByStNo(asn.StNo);
                    if (objs_asnd != null)
                    {
                        foreach (ASNDetail asnd in objs_asnd)
                        {
                            _InventoryFacade.DeleteASNDetail(asnd);
                            object[] objs_item = _warehouseFacade.GetASNDetailItembyStnoAndStline(asnd.StNo, asnd.StLine);
                            if (objs_item != null)
                            {
                                foreach (Asndetailitem item in objs_item)
                                {
                                    _warehouseFacade.DeleteAsndetailitem(item);
                                }
                            }
                            object[] objs_sn = _warehouseFacade.GetASNDetailSNbyStnoandStline(asnd.StNo, asnd.StLine);
                            if (objs_sn != null)
                            {
                                foreach (Asndetailsn sn in objs_sn)
                                {
                                    _warehouseFacade.DeleteAsndetailsn(sn);
                                }
                            }
                        }
                    }


                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                throw ex;
            }
        }

        //�·�
        protected void cmdSendDown_ServerClick(object sender, EventArgs e)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            ArrayList array = this.gridHelper.GetCheckedRows();
            string StNo = string.Empty;

            if (array.Count > 0)
            {
                ArrayList stNoList = new ArrayList(array.Count);
                StringBuilder sbShowMsg = new StringBuilder();
                foreach (GridRecord row in array)
                {
                    StNo = row.Items.FindItemByKey("StNo").Value.ToString();
                    if (!string.IsNullOrEmpty(StNo))
                    {
                        ASN asn = (ASN)_InventoryFacade.GetASN(StNo);
                        if (asn == null)
                        {
                            //���ָ��ţ� {0} ������
                            sbShowMsg.AppendFormat("$NO_ASN {0} ������ ", StNo);
                            continue;
                        }

                        if (asn.StType == "SCTR")
                        {
                            if (string.IsNullOrEmpty(asn.StorageCode))
                            {
                                sbShowMsg.Append("���������·�������д����λ�� ");
                                continue;
                            }
                        }


                        if (asn.Status != ASNHeadStatus.Release)
                        {
                            //���ָ��ţ� {0} ״̬���ǳ�ʼ���������·�
                            sbShowMsg.AppendFormat("$NO_ASN {0} $Error_Status_Is_NOT_RELEASE ", StNo);
                            continue;
                        }
                        if (asn.DirectFlag == "Y")
                        {
                            //�����е���ǩ�յ�
                            bool hasInvDoc = _InventoryFacade.CheckHasInvDoc(StNo, InvDocType.DirectSign);
                            if (!hasInvDoc)
                            {
                                //���ָ��ţ� {0} û�е���ǩ�յ��������·�
                                sbShowMsg.AppendFormat(" $NO_ASN {0}  $Error_Has_Not_InvDoc ", StNo);
                                continue;
                            }
                        }
                        bool hasDetail = _InventoryFacade.CheckASNHasDetail(StNo, ASNLineStatus.Cancel);

                        if (!hasDetail)
                        {
                            //���ָ��ţ� {0} û�е����䵥�������·�
                            sbShowMsg.AppendFormat(" $NO_ASN {0} $Error_Has_Not_Improt_CartonNo ", StNo);
                            continue;
                        }
                        stNoList.Add(string.Format("'{0}'", StNo));
                    }

                }

                string stNo = string.Join(",", stNoList.ToArray(typeof(string)) as string[]);
                if (!string.IsNullOrEmpty(stNo))
                {
                    try
                    {
                        this.DataProvider.BeginTransaction();
                        this._InventoryFacade.UpdateASNStatusByStNo(ASNHeadStatus.WaitReceive, stNo);
                        this._InventoryFacade.UpdateASNDetailStatusByStNo(ASNLineStatus.WaitReceive, stNo, ASNLineStatus.Release);


                        facade = new WarehouseFacade(base.DataProvider);
                        ASN asn = (ASN)_InventoryFacade.GetASN(StNo);
                        DBDateTime dbTime1 = FormatHelper.GetNowDBDateTime(this.DataProvider);
                        InvInOutTrans trans = facade.CreateNewInvInOutTrans();
                        trans.CartonNO = string.Empty;
                        trans.DqMCode = " ";
                        trans.FacCode = asn.FacCode;
                        trans.FromFacCode = string.Empty;
                        trans.FromStorageCode = string.Empty;
                        trans.InvNO = asn.InvNo;
                        trans.InvType = asn.StType;
                        trans.LotNo = string.Empty;
                        trans.MaintainDate = dbTime1.DBDate;
                        trans.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                        trans.MaintainUser = this.GetUserCode();
                        trans.MCode = " ";
                        trans.ProductionDate = 0;
                        trans.Qty = 0;
                        trans.Serial = 0;
                        trans.StorageAgeDate = 0;
                        trans.StorageCode = asn.StorageCode;
                        trans.SupplierLotNo = string.Empty;
                        trans.TransNO = asn.StNo;
                        trans.TransType = "IN";
                        trans.Unit = string.Empty;
                        trans.ProcessType = "ISSUE";
                        facade.AddInvInOutTrans(trans);

                        this.DataProvider.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        WebInfoPublish.Publish(this, "�·�ʧ�ܣ�" + ex.Message, this.languageComponent1);
                        this.DataProvider.RollbackTransaction();
                        return;
                    }

                }

                if (sbShowMsg.Length > 0)
                {
                    string showMsg = sbShowMsg.ToString();
                    WebInfoPublish.Publish(this, showMsg, this.languageComponent1);
                }
                else
                {
                    WebInfoPublish.Publish(this, "�·��ɹ�", this.languageComponent1);
                }
                this.gridHelper.RequestData();//ˢ��ҳ��
            }
        }





        //���
        protected void cmdInStorage_ServerClick(object sender, EventArgs e)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            ArrayList array = this.gridHelper.GetCheckedRows();
            string StNo = string.Empty;

            if (array.Count > 0)
            {
                StringBuilder sbShowMsg = new StringBuilder();
                foreach (GridRecord row in array)
                {
                    StNo = row.Items.FindItemByKey("StNo").Value.ToString();
                    if (!string.IsNullOrEmpty(StNo))
                    {
                        ASN asn = (ASN)_InventoryFacade.GetASN(StNo);
                        if (asn == null)
                        {
                            //���ָ��ţ� {0} ������
                            sbShowMsg.AppendFormat("$NO_ASN {0} ������ ", StNo);
                            continue;
                        }

                        if (asn.DirectFlag != "Y")
                        {
                            //���ָ��ţ� {0} ���ǹ�Ӧ��ֱ�����������
                            sbShowMsg.AppendFormat(" $NO_ASN {0}  ���ǹ�Ӧ��ֱ����������� ", StNo);
                            continue;
                        }
                        else
                        {
                            object[] objInvDoc = this._InventoryFacade.GetInvDocByInvDocNo(StNo);
                            if (objInvDoc == null)
                            {
                                //���ָ��ţ� {0} û���ϴ�ֱ�����ͻ�����ƾ֤���������
                                sbShowMsg.AppendFormat(" $NO_ASN {0}  û���ϴ�ֱ�����ͻ�����ƾ֤��������� ", StNo);
                                continue;
                            }
                        }
                        if (asn.Status != ASNHeadStatus.WaitReceive)
                        {
                            //���ָ��ţ� {0} δ�·����������
                            sbShowMsg.AppendFormat("$NO_ASN {0} δ�·���������� ", StNo);
                            continue;
                        }
                        if (CheckIsAllCancel(StNo))
                        {
                            sbShowMsg.AppendFormat("$NO_ASN {0} �Ѿ�ȡ����������� ", StNo);
                            continue;
                        }
                        if (CheckIsAllOnLocation(StNo))
                        {
                            //���ָ��ţ� {0} δȫ���ϼܣ��������
                            sbShowMsg.AppendFormat("$NO_ASN {0} û��ȫ���ϼܣ�������� ", StNo);
                            continue;
                        }
                        //���

                        DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                        object[] objs_asndd = _InventoryFacade.GetASNDetailByStNo(StNo);

                        if (objs_asndd == null)
                        {
                            WebInfoPublish.Publish(this, "���������û�ж�Ӧ����Ŀ��", this.languageComponent1);
                            return;

                        }
                        foreach (ASNDetail asndetail in objs_asndd)
                        {

                            if (asndetail.Status == ASNDetail_STATUS.ASNDetail_Close)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "�����ظ��ϼ�", this.languageComponent1);
                                return;
                            }
                        }

                        string message = string.Empty;
                        string sapInvoice = string.Empty;
                        string sapInvoice2 = string.Empty;
                        bool result = PoToSap(objs_asndd, "103", out message, string.Empty, out sapInvoice);

                        if (!result)
                            throw new Exception("SAP-103��д���� " + message);

                        result = PoToSap(objs_asndd, "105", out message, sapInvoice, out sapInvoice2);

                        if (!result)
                            throw new Exception("SAP-105��д���� " + message);


                        this.DataProvider.BeginTransaction();
                        try
                        {
                            if (facade == null)
                            {
                                facade = new WarehouseFacade(base.DataProvider);
                            }
                            if (_Invenfacade == null)
                            {
                                _Invenfacade = new InventoryFacade(base.DataProvider);
                            }
                            string Stno = string.Empty;


                            foreach (ASNDetail asndetail in objs_asndd)
                            {
                                Stno = asndetail.StNo;
                                #region ����asndetail�� ����actqty��״̬
                                //update actqty and status
                                if (asn.StType == SAP_ImportType.SAP_PD || asn.RejectsFlag.ToUpper() == "Y")
                                {
                                    asndetail.ActQty = asndetail.ReceiveQty;
                                }
                                else
                                {
                                    asndetail.ActQty = asndetail.Qty;
                                    asndetail.ReceiveQty = asndetail.Qty;
                                }
                                asndetail.Status = ASNDetail_STATUS.ASNDetail_Close;

                                asndetail.MaintainDate = dbTime.DBDate;
                                asndetail.MaintainTime = dbTime.DBTime;
                                asndetail.MaintainUser = this.GetUserCode();
                                if (asn.StType == SAP_ImportType.SAP_POR || asn.StType == SAP_ImportType.SAP_YFR || asn.StType == SAP_ImportType.SAP_PD)
                                {
                                    asndetail.StorageAgeDate = dbTime.DBDate;
                                }
                                //��Ŵ���

                                object objLot = null;
                                objLot = facade.GetNewLotNO("CT", dbTime.DBDate.ToString());
                                Serialbook serbook = facade.CreateNewSerialbook();
                                if (objLot == null)
                                {
                                    //�����ݵ�tblserialbook
                                    serbook.SNprefix = "CT" + dbTime.DBDate.ToString();
                                    serbook.MAXSerial = "2";
                                    serbook.MUser = this.GetUserCode();
                                    serbook.MDate = dbTime.DBDate;
                                    serbook.MTime = dbTime.DBTime;
                                    facade.AddSerialbook(serbook);
                                    asndetail.CartonNo = "CT" + dbTime.DBDate.ToString() + "000000001ZC";

                                }
                                else
                                {
                                    string MAXNO = (objLot as Serialbook).MAXSerial;
                                    string SNNO = (objLot as Serialbook).SNprefix;
                                    asndetail.CartonNo = SNNO + Convert.ToString(MAXNO).PadLeft(9, '0') + "ZC";

                                    //����tblserialbook
                                    serbook.SNprefix = SNNO;
                                    serbook.MAXSerial = Convert.ToString((int.Parse(MAXNO) + 1));
                                    serbook.MUser = this.GetUserCode();
                                    serbook.MDate = dbTime.DBDate;
                                    serbook.MTime = dbTime.DBTime;
                                    facade.UpdateSerialbook(serbook);

                                }
                                _Invenfacade.UpdateASNDetail(asndetail);
                                #endregion
                                #region ����sn���е����


                                object[] objs_asn_sn = facade.GetASNDetailSNbyStnoandStline(asndetail.StNo, asndetail.StLine);
                                if (objs_asn_sn != null)
                                {
                                    foreach (Asndetailsn sn in objs_asn_sn)
                                    {
                                        sn.Cartonno = asndetail.CartonNo;
                                        facade.UpdateAsndetailsn(sn);
                                    }
                                }
                                #endregion
                                #region ����asndetailitem�� ���� actqty �͸���invoicedetail������actqty
                                object[] itemobjs = facade.GetASNDetailItembyStnoAndStline(asndetail.StNo, asndetail.StLine.ToString());
                                if (itemobjs != null)
                                {
                                    for (int i = 0; i < itemobjs.Length; i++)
                                    {
                                        Asndetailitem asnitem = itemobjs[i] as Asndetailitem;
                                        if (asn.StType == SAP_ImportType.SAP_PD || asn.RejectsFlag.ToUpper() == "Y")
                                        {
                                            asnitem.ActQty = (int)asnitem.ReceiveQty;
                                        }
                                        else
                                        {
                                            asnitem.ActQty = (int)asnitem.QcpassQty;
                                        }
                                        //asnitem.ActQty = asnitem.QcpassQty;
                                        asnitem.MaintainDate = dbTime.DBDate;
                                        asnitem.MaintainTime = dbTime.DBTime;
                                        asnitem.MaintainUser = this.GetUserCode();
                                        facade.UpdateAsndetailitem(asnitem);

                                        object invoiobj = _Invenfacade.GetInvoicesDetail(asnitem.Invno, int.Parse(asnitem.Invline));
                                        InvoicesDetail inv = invoiobj as InvoicesDetail;
                                        if (asn.StType == SAP_ImportType.SAP_PD || asn.RejectsFlag.ToUpper() == "Y")
                                        {
                                            inv.ActQty = (int)asnitem.ReceiveQty;
                                        }
                                        else
                                        {
                                            inv.ActQty = (int)asnitem.QcpassQty;
                                        }
                                        //inv.ActQty +=Convert.ToInt32(asnitem.QcpassQty);
                                        inv.MaintainDate = dbTime.DBDate;
                                        inv.MaintainTime = dbTime.DBTime;
                                        inv.MaintainUser = this.GetUserCode();

                                        _Invenfacade.UpdateInvoicesDetail(inv);
                                    }
                                }
                                #endregion
                                #region ��storagedetail��������һ������
                                StorageDetail stordetail = _Invenfacade.CreateNewStorageDetail();
                                stordetail.AvailableQty = asndetail.ActQty;
                                stordetail.CartonNo = asndetail.CartonNo;
                                stordetail.CDate = dbTime.DBDate;
                                stordetail.CTime = dbTime.DBTime;
                                stordetail.CUser = this.GetUserCode();
                                stordetail.DQMCode = asndetail.DQMCode;
                                stordetail.FacCode = asn.FacCode;
                                stordetail.FreezeQty = 0;
                                stordetail.LastStorageAgeDate = dbTime.DBDate;
                                stordetail.LocationCode = facade.GetLocationCode(asn.StorageCode);
                                stordetail.Lotno = asndetail.LotNo;
                                stordetail.MaintainDate = dbTime.DBDate;
                                stordetail.MaintainTime = dbTime.DBTime;
                                stordetail.MaintainUser = this.GetUserCode();
                                stordetail.MCode = asndetail.MCode;
                                stordetail.MDesc = asndetail.MDesc;
                                stordetail.ProductionDate = asndetail.ProductionDate;

                                stordetail.ReworkApplyUser = asn.ReWorkApplyUser;
                                stordetail.StorageAgeDate = string.IsNullOrEmpty(asndetail.StorageAgeDate.ToString()) ? dbTime.DBDate : asndetail.StorageAgeDate;
                                stordetail.StorageCode = asn.StorageCode;
                                stordetail.StorageQty = asndetail.ActQty;
                                stordetail.SupplierLotNo = asndetail.SupplierLotNo;
                                stordetail.Unit = asndetail.Unit;

                                _Invenfacade.AddStorageDetail(stordetail);
                                #endregion
                                #region ��StorageDetailSN������������
                                //��������tblStorageDetailSN

                                object[] snobj = facade.GetASNDetailSNbyStnoandStline(asndetail.StNo, asndetail.StLine.ToString());
                                if (snobj != null)
                                {
                                    //this.DataProvider.RollbackTransaction();
                                    //WebInfoPublish.Publish(this, "$Error_ASNDetail_NO_DATA", this.languageComponent1);
                                    //return;                        
                                    for (int i = 0; i < snobj.Length; i++)
                                    {
                                        StorageDetailSN storDetailSN = _Invenfacade.CreateNewStorageDetailSN();
                                        Asndetailsn detailSN = snobj[i] as Asndetailsn;
                                        storDetailSN.CartonNo = detailSN.Cartonno;
                                        storDetailSN.CDate = dbTime.DBDate;
                                        storDetailSN.CTime = dbTime.DBTime;
                                        storDetailSN.CUser = this.GetUserCode();
                                        storDetailSN.MaintainDate = dbTime.DBDate;
                                        storDetailSN.MaintainTime = dbTime.DBTime;
                                        storDetailSN.MaintainUser = this.GetUserCode();
                                        storDetailSN.PickBlock = "N";
                                        storDetailSN.SN = detailSN.Sn;

                                        _Invenfacade.AddStorageDetailSN(storDetailSN);

                                    }
                                }
                                #endregion
                                #region ��invinouttrans��������һ������
                                InvInOutTrans trans = facade.CreateNewInvInOutTrans();
                                trans.CartonNO = asndetail.CartonNo;
                                trans.DqMCode = asndetail.DQMCode;
                                trans.FacCode = asn.FacCode;
                                trans.FromFacCode = asn.FromFacCode;
                                trans.FromStorageCode = asn.FromStorageCode;
                                trans.InvNO = asn.InvNo;
                                trans.InvType = asn.StType;
                                trans.LotNo = asndetail.LotNo;
                                trans.MaintainDate = dbTime.DBDate;
                                trans.MaintainTime = dbTime.DBTime;
                                trans.MaintainUser = this.GetUserCode();
                                trans.MCode = asndetail.MCode;
                                trans.ProductionDate = asndetail.ProductionDate;
                                trans.Qty = asndetail.ActQty;
                                trans.Serial = 0;
                                trans.StorageAgeDate = asndetail.StorageAgeDate;
                                trans.StorageCode = asn.StorageCode;
                                trans.SupplierLotNo = asndetail.SupplierLotNo;
                                trans.TransNO = asn.StNo;
                                trans.TransType = "IN";
                                trans.ProcessType = "INSTORAEE";
                                trans.Unit = asndetail.Unit;
                                facade.AddInvInOutTrans(trans);
                                #endregion

                                #region  ����tblstorageinfo
                                string sumQty = facade.GetStorageQtyByMcodeAndStorageCode(stordetail.MCode, stordetail.StorageCode);
                                object stoinfo_obj = facade.GetStorageinfo(stordetail.MCode, stordetail.StorageCode);
                                if (stoinfo_obj == null)
                                {
                                    StorageInfo sto_info = facade.CreateNewStorageinfo();
                                    sto_info.Mcode = stordetail.MCode;
                                    sto_info.Mdate = dbTime.DBDate;
                                    sto_info.Mtime = dbTime.DBTime;
                                    sto_info.Muser = this.GetUserCode();
                                    sto_info.StorageCode = stordetail.StorageCode;
                                    sto_info.Storageqty = Int32.Parse(sumQty);
                                    facade.AddStorageinfo(sto_info);
                                }
                                else
                                {
                                    StorageInfo sto_info = stoinfo_obj as StorageInfo;
                                    sto_info.Storageqty = Int32.Parse(sumQty);
                                    sto_info.Mdate = dbTime.DBDate;
                                    sto_info.Mtime = dbTime.DBTime;
                                    sto_info.Muser = this.GetUserCode();
                                    facade.UpdateStorageinfo(sto_info);
                                }
                                #endregion


                            }
                            #region  �ϼ���ɺ󣬼�����stno��asndetail�е�״̬�Ƿ���close��cancel������ǽ�asn���״̬����Ϊclose��cancel
                            if (facade.JudgeASNDetailStatus(Stno, ASNDetail_STATUS.ASNDetail_Close))
                            {
                                object asnobj = facade.GetAsn(Stno);
                                if (asnobj == null)
                                {
                                    this.DataProvider.RollbackTransaction();
                                    WebInfoPublish.Publish(this, "$Error_ASN_NO_DATA", this.languageComponent1);

                                    return;
                                }
                                asn.Status = ASN_STATUS.ASN_Close;
                                asn.MaintainDate = dbTime.DBDate;
                                asn.MaintainTime = dbTime.DBTime;
                                asn.MaintainUser = this.GetUserCode();

                                _Invenfacade.UpdateASN(asn);
                            }
                            if (facade.JudgeASNDetailStatus(Stno, ASNDetail_STATUS.ASNDetail_Cancel))
                            {
                                object asnobj = facade.GetAsn(Stno);
                                if (asnobj == null)
                                {
                                    this.DataProvider.RollbackTransaction();
                                    WebInfoPublish.Publish(this, "$Error_ASN_NO_DATA", this.languageComponent1);

                                    return;
                                }
                                asn.Status = ASN_STATUS.ASN_Cancel;
                                asn.MaintainDate = dbTime.DBDate;
                                asn.MaintainTime = dbTime.DBTime;
                                asn.MaintainUser = this.GetUserCode();

                                _Invenfacade.UpdateASN(asn);
                            }
                            #endregion
                            #region  ͨ�����ָ��ţ�����invno�����actqty�Ƿ����planqty��������ڽ�finishflag=Y
                            object asnobj2 = facade.GetAsn(Stno);
                            if (asnobj2 == null)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "$Error_ASN_NO_DATA", this.languageComponent1);

                                return;
                            }
                            Asn asn1 = asnobj2 as Asn;
                            if (facade.JudgeInvoiceDetailStatus(asn1.Invno))
                            {
                                object invobj = _Invenfacade.GetInvoices(asn1.Invno);
                                if (invobj == null)
                                {
                                    this.DataProvider.RollbackTransaction();
                                    WebInfoPublish.Publish(this, "$Error_INV_NO_DATA", this.languageComponent1);

                                    return;
                                }
                                Invoices inv = invobj as Invoices;
                                inv.FinishFlag = "Y";
                                inv.MaintainDate = dbTime.DBDate;
                                inv.MaintainTime = dbTime.DBTime;
                                inv.MaintainUser = this.GetUserCode();
                                _Invenfacade.UpdateInvoices(inv);
                            }
                            #endregion

                            //ToInStorage(asn);



                            this.DataProvider.CommitTransaction();
                            WebInfoPublish.Publish(this, "$Message_ONLocation_Success", this.languageComponent1);

                        }
                        catch (Exception ex)
                        {
                            this.DataProvider.RollbackTransaction();
                            Log.Error(ex.StackTrace);
                            throw ex;
                        }


                    }

                }
                if (sbShowMsg.Length > 0)
                {
                    string showMsg = sbShowMsg.ToString();
                    WebInfoPublish.Publish(this, showMsg, this.languageComponent1);
                }
                //else
                //{
                //    WebInfoPublish.Publish(this, "���ɹ�", this.languageComponent1);
                //}
                this.gridHelper.RequestData();//ˢ��ҳ��
            }
        }

        //����
        protected void cmdReturn_ServerClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("FSAPStorageInDemandQuery.aspx"));
        }
        //�鿴����
        protected void cmdView_ServerClick(object sender, EventArgs e)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            string asnNo = FormatHelper.CleanString(this.txtStorageInASNEdit.Text);
            if (string.IsNullOrEmpty(asnNo))
            {
                WebInfoPublish.PublishInfo(this, "���ָ���Ϊ��", this.languageComponent1);
                return;
            }
            object[] objs = _InventoryFacade.GetFileUpLoad(asnNo, "DirectSign");
            if (objs != null)
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                string path = Server.MapPath(this.VirtualHostRoot + "InvDoc/");
                foreach (InvDoc doc in objs)
                {
                    //info.WorkingDirectory = path;
                    info.FileName = path.Replace('/', '\\') + doc.ServerFileName;
                    info.Arguments = "";
                    info.UseShellExecute = false;
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch (System.ComponentModel.Win32Exception we)
                    {
                        WebInfoPublish.PublishInfo(this, we.Message, this.languageComponent1);
                        return;
                    }
                }
            }
            else
            {
                WebInfoPublish.PublishInfo(this, "û�и���", this.languageComponent1);
                return;
            }
        }
        //����
        protected void cmdEnter_ServerClick(object sender, EventArgs e)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            string asnNo = FormatHelper.CleanString(this.txtStorageInASNEdit.Text);
            if (string.IsNullOrEmpty(asnNo))
            {
                WebInfoPublish.PublishInfo(this, "���ָ���Ϊ��", this.languageComponent1);
                return;
            }
            if (this.FileImport.PostedFile != null)
            {
                try
                {
                    HttpPostedFile postedFile = this.FileImport.PostedFile;

                    DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
                    InvDoc invDoc = _InventoryFacade.CreateNewInvDoc();

                    invDoc.InvDocNo = asnNo;
                    invDoc.InvDocType = "DirectSign";
                    invDoc.DocType = Path.GetExtension(postedFile.FileName);
                    invDoc.DocName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                    invDoc.DocSize = postedFile.ContentLength / 1024;
                    invDoc.UpUser = this.GetUserCode();
                    invDoc.UpfileDate = dbDateTime.DBDate;
                    invDoc.MaintainUser = this.GetUserCode();
                    invDoc.MaintainDate = dbDateTime.DBDate;
                    invDoc.MaintainTime = dbDateTime.DBTime;

                    string path = Server.MapPath(this.VirtualHostRoot + "InvDoc/" + "ֱ�����ͻ�����ƾ֤/");
                    string fileName = string.Format("{0}_DirectSign_{1}{2}{3}",
                        this.txtStorageInASNEdit.Text, dbDateTime.DBDate, dbDateTime.DBTime, invDoc.DocType);

                    invDoc.ServerFileName = fileName;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    invDoc.Dirname = "ֱ�����ͻ�����ƾ֤";
                    this.FileImport.PostedFile.SaveAs(path + fileName);
                    _InventoryFacade.AddInvDoc(invDoc);
                    WebInfoPublish.PublishInfo(this, "$Success_UpLoadFile", this.languageComponent1);
                }
                catch (Exception ex)
                {

                    WebInfoPublish.PublishInfo(this, ex.Message, this.languageComponent1);
                }

            }
            else
            {
                WebInfoPublish.PublishInfo(this, "�����ļ�����Ϊ��", this.languageComponent1);
            }

        }

        protected override void buttonHelper_AfterPageStatusChangeHandle(string pageAction)
        {
            if (pageAction == PageActionType.Query)
            {
                this.cmdSave.Disabled = false;

                this.FileImport.Disabled = true;
                this.cmdEnter.Disabled = true;
                //if (this.cblFlag.Items[0].Selected)
                //{
                //    this.FileImport.Disabled = false;
                //    this.cmdEnter.Disabled = false;
                //}
                //else
                //{
                //    this.FileImport.Disabled = true;
                //    this.cmdEnter.Disabled = true;
                //}
                //�ı临ѡ������״̬
                ChangeCblItmeEnabled();

                string invNo = this.ViewState["InvNo"] == null ? string.Empty : this.ViewState["InvNo"].ToString();
                string storageType = this.ViewState["StorageInType"] == null ? string.Empty : this.ViewState["StorageInType"].ToString();
                string stNo = this.ViewState["stNo"] == null ? string.Empty : this.ViewState["stNo"].ToString();
                this.SetInvNoAndStorageTypeByRequestParam(invNo, storageType);

                this.txtStorageInASNEdit.Text = stNo;

            }
            if (pageAction == PageActionType.Add)
            {
                this.cmdSave.Disabled = false;
                this.cmdCreat.Disabled = false;

                this.drpStorageInTypeEdit.Enabled = true;
                this.cblFlag.Items[0].Enabled = true;
                this.cblFlag.Items[1].Enabled = true;
                this.cblFlag.Items[2].Enabled = true;

                this.FileImport.Disabled = true;
                this.cmdEnter.Disabled = true;
            }
            if (pageAction == PageActionType.Update)
            {
                this.cmdCreat.Disabled = true;
                this.drpStorageInTypeEdit.Enabled = false;
                if (this.drpStorageInTypeEdit.SelectedValue == InInvType.POR)
                {
                    this.FileImport.Disabled = false;
                    this.cmdEnter.Disabled = false;
                }
                //if (this.cblFlag.Items[0].Selected)
                //{
                //    this.FileImport.Disabled = false;
                //    this.cmdEnter.Disabled = false;
                //}
                //else
                //{
                //    this.FileImport.Disabled = true;
                //    this.cmdEnter.Disabled = true;
                //}
            }
        }

        #endregion

        #region Object <--> Page

        protected override object GetEditObject()
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            string stNo = FormatHelper.CleanString(this.txtStorageInASNEdit.Text, 40);
            ASN asn = (ASN)_InventoryFacade.GetASN(stNo);
            if (asn == null)
            {
                _IsAdd = true;
                asn = this._InventoryFacade.CreateNewASN();
            }
            asn.StNo = stNo;
            asn.StType = FormatHelper.CleanString(this.drpStorageInTypeEdit.SelectedValue, 40);
            asn.InvNo = FormatHelper.CleanString(this.txtSAPInvNoEdit.Text, 40);
            if (drpStorageInTypeEdit.SelectedValue == "SCTR")
                asn.VendorCode = FormatHelper.CleanString(this.drpVendorCodeEdit.SelectedValue, 40);
            asn.StorageCode = FormatHelper.CleanString(this.drpStorageInEdit.Text, 40);
            asn.PreictDate = FormatHelper.TODateInt(this.txtExpectedArrivalDateEdit.Text);
            asn.DirectFlag = this.cblFlag.Items[0].Selected ? "Y" : "";
            asn.ExigencyFlag = this.cblFlag.Items[1].Selected ? "Y" : "";
            asn.RejectsFlag = this.cblFlag.Items[2].Selected ? "Y" : "";
            asn.PickNo = FormatHelper.CleanString(this.txtPickNoEdit.Text, 40);
            asn.Remark1 = FormatHelper.CleanString(this.txtMemoEdit.Text, 300);

            if (_IsAdd)
            {
                asn.Status = ASNLineStatus.Release;
                asn.FacCode = this.GetFacCode();
                DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
                asn.CDate = dbDateTime.DBDate;
                asn.CTime = dbDateTime.DBTime;
                asn.CUser = this.GetUserCode();
            }
            asn.MaintainUser = this.GetUserCode();

            return asn;
        }

        protected override object GetEditObject(GridRecord row)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }

            object obj = _InventoryFacade.GetASN(row.Items.FindItemByKey("StNo").Value.ToString());

            if (obj != null)
            {
                return (ASN)obj;
            }
            return null;
        }

        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.txtStorageInASNEdit.Text = "";
                this.drpStorageInTypeEdit.SelectedIndex = 0;
                this.txtSAPInvNoEdit.Text = "";
                this.txtExpectedArrivalDateEdit.Text = "";
                this.drpStorageInEdit.SelectedIndex = 0;
                this.drpVendorCodeEdit.SelectedIndex = 0;
                this.txtPickNoEdit.Text = "";
                this.txtMemoEdit.Text = "";
                this.cblFlag.Items[0].Selected = false;
                this.cblFlag.Items[1].Selected = false;
                this.cblFlag.Items[2].Selected = false;
                InitStorageInTypeList();
                return;
            }

            this.txtStorageInASNEdit.Text = ((ASN)obj).StNo;

            //this.drpStorageInTypeEdit.SelectedValue = ((ASN)obj).StType;


            try
            {
                this.drpStorageInTypeEdit.SelectedValue = ((ASN)obj).StType;

            }
            catch (Exception)
            {
                this.drpStorageInTypeEdit.Items.Clear();
                this.drpStorageInTypeEdit.Items.Add(new ListItem(this.GetInvInName(((ASN)obj).StType), ((ASN)obj).StType));
                this.drpStorageInTypeEdit.SelectedValue = ((ASN)obj).StType;
            }
            drpStorageInTypeEdit.Enabled = false;
            //�ı临ѡ������״̬
            ChangeCblItmeEnabled();

            this.txtSAPInvNoEdit.Text = ((ASN)obj).InvNo;
            this.txtExpectedArrivalDateEdit.Text = FormatHelper.ToDateString(((ASN)obj).PreictDate, "-");


            this.drpStorageInEdit.SelectedValue = ((ASN)obj).StorageCode;



            foreach (ListItem item in this.drpVendorCodeEdit.Items)
            {
                if (item.Value == ((ASN)obj).VendorCode)
                    this.drpVendorCodeEdit.SelectedValue = ((ASN)obj).VendorCode;

            }


            this.txtPickNoEdit.Text = ((ASN)obj).PickNo;
            this.txtMemoEdit.Text = ((ASN)obj).Remark1;


            this.cblFlag.Items[0].Selected = ((ASN)obj).DirectFlag == "Y";
            this.cblFlag.Items[1].Selected = ((ASN)obj).ExigencyFlag == "Y";
            this.cblFlag.Items[2].Selected = ((ASN)obj).RejectsFlag == "Y";
        }

        protected override bool ValidateInput()
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }

            PageCheckManager manager = new PageCheckManager();
            manager.Add(new LengthCheck(this.lblStorageInASNEdit, this.txtStorageInASNEdit, 40, true));
            manager.Add(new LengthCheck(this.lblStorageInTypeEdit, this.drpStorageInTypeEdit, 40, true));
            manager.Add(new LengthCheck(this.lblStorageInEdit, this.drpStorageInEdit, 40, true));
            manager.Add(new LengthCheck(this.lblInvNoEdit, this.txtSAPInvNoEdit, 40, false));

            string storageInType = this.drpStorageInTypeEdit.SelectedValue;
            if (storageInType == InInvType.POR || storageInType == InInvType.CAR ||
                storageInType == InInvType.BLR || storageInType == InInvType.JCR ||
                storageInType == InInvType.PGIR || storageInType == InInvType.SCTR || storageInType == InInvType.DNR)
            {
                manager.Add(new LengthCheck(this.lblExpectedArrivalDateEdit, this.txtExpectedArrivalDateEdit, 22, true));
            }
            else
            {
                manager.Add(new LengthCheck(this.lblExpectedArrivalDateEdit, this.txtExpectedArrivalDateEdit, 22, false));
            }



            if (storageInType == InInvType.SCTR)//��������
            {
                manager.Add(new LengthCheck(this.lblMesFacNameEdit, this.drpVendorCodeEdit, 40, true));
            }
            else
            {
                manager.Add(new LengthCheck(this.lblMesFacNameEdit, this.drpVendorCodeEdit, 40, false));
            }


            if (storageInType == InInvType.PGIR)
            {
                manager.Add(new LengthCheck(this.lblPickNoEdit, this.txtPickNoEdit, 40, true));
            }
            else
            {
                manager.Add(new LengthCheck(this.lblPickNoEdit, this.txtPickNoEdit, 40, false));
            }

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            if (storageInType == InInvType.PGIR)
            {

                Pick p = (Pick)_InventoryFacade.GetPick(txtPickNoEdit.Text);
                if (p == null)
                {
                    WebInfoPublish.Publish(this, "���ָ��Ų����ڣ�", this.languageComponent1);
                    return false;
                }
                if (p.StorageCode != drpStorageInEdit.SelectedValue)
                {
                    WebInfoPublish.Publish(this, "����ָ��Ŀ�λ�����ѡ�������λһ�£�", this.languageComponent1);
                    return false;
                }
            }
            return true;
        }



        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            string[] strHeaderList2 = {"ASNCreateTime", "ReformCount","ReturnCount","RejectCount", 
                                    "SendDownTime", "ReceiveOkTime", "IQCOKTIME","INSTORAGETIME" };
            string[] objs = new string[this.SAPHeadViewFieldList.Length + strHeaderList2.Length];
            ASN inv = obj as ASN;
            Type type = inv.GetType();
            for (int i = 0; i < this.SAPHeadViewFieldList.Length; i++)
            {
                ViewField field = this.SAPHeadViewFieldList[i];
                string strValue = string.Empty;
                System.Reflection.FieldInfo fieldInfo = type.GetField(field.FieldName);
                if (fieldInfo != null)
                {
                    strValue = fieldInfo.GetValue(inv).ToString();
                }
                if (field.FieldName == "CDate")
                {
                    strValue = FormatHelper.ToDateString(inv.CDate);

                }
                else if (field.FieldName == "StorageInType")
                {
                    strValue = this.GetInvInName(inv.StType);
                }
                else if (field.FieldName == "Status")
                {
                    strValue = this.GetStatusName(inv.Status);
                }
                else if (field.FieldName == "PredictDate")
                {
                    strValue = FormatHelper.ToDateString(inv.PreictDate);
                }
                else if (field.FieldName == "ProvideDate")
                {
                    strValue = inv.ProvideDate;
                }
                objs[i] = strValue;
            }

            #region add by sam 2016��7��29��

            int j = this.SAPHeadViewFieldList.Length;
            facade = new WarehouseFacade(base.DataProvider);
            Asn asn = (Asn)facade.GetAsn(txtStorageInASNQuery.Text.Trim().ToUpper());
            if (asn != null)
            {
                string createTime = asn.CDate.ToString() + " " + FormatHelper.ToTimeString(asn.CTime);
                objs[j] = createTime;

            }
            j++;
            BenQGuru.eMES.IQC.IQCFacade iqcFacade = new BenQGuru.eMES.IQC.IQCFacade(base.DataProvider);
            objs[j++] = iqcFacade.ReformQtyTotalWithStNo(inv.StNo).ToString();
            objs[j++] = iqcFacade.ReturnQtyTotalWithStNo(inv.StNo).ToString();
            string status = inv.Status;

            int rejectCount = 0;
            Asndetail[] asndetails = facade.GetAsnDetails(inv.StNo);

            if (status != ASNHeadStatus.Release && status != ASNHeadStatus.WaitReceive && status != ASNHeadStatus.Receive)
            {
                foreach (Asndetail d in asndetails)
                {
                    rejectCount += (d.Qty - d.ReceiveQty);
                }
            }

            objs[j++] = rejectCount.ToString();
            objs[j++] = facade.GetProcessPhaseTime1(((ASN)obj).StNo, "ISSUE");
            objs[j++] = facade.GetProcessPhaseTime1(((ASN)obj).StNo, "Receive");
            objs[j++] = facade.GetProcessPhaseTime1(((ASN)obj).StNo, "IQC");
            objs[j] = facade.GetProcessPhaseTime1(((ASN)obj).StNo, "INSTORAEE");
            #endregion
            return objs;
        }

        protected override string[] GetColumnHeaderText()
        {
            #region ע��
            //return new string[] {	"ASN",
            //                        "CDate",
            //                        "CUser",
            //                        "StorageInType",
            //                        "SAPInvNo",
            //                        "Status",	
            //                        "StorageInCode",
            //                        "PredictDate",	
            //                        "DirectFlag",
            //                        "PickNo",	
            //                        "VendorCode",
            //                        "ExigencyFlag",	
            //                        "RejectsFlag",
            //                        "OANo",
            //                        "PackingListNo",	
            //                        "ProvideDate",
            //                        "GrossWeight",	
            //                        "Volume",
            //                        "FromStorageCode",	
            //                        "Remark1"}; 
            #endregion

            string[] strHeaderList = { "���ָ���ʱ��", "�ֳ���������", "�˻�������", "�����������", 
                                    "�·�ʱ��", "�������ʱ��", "IQC���ʱ��",   "������ʱ��" };
            string[] strHeader = new string[this.SAPHeadViewFieldList.Length + strHeaderList.Length];

            for (int i = 0; i < SAPHeadViewFieldList.Length; i++)
            {
                #region ע��
                //string strText = languageComponent1.GetString(this.SAPHeadViewFieldList[i].FieldName);
                //if (strText == string.Empty)
                //    strText = this.SAPHeadViewFieldList[i].Description; 
                #endregion
                strHeader[i] = this.SAPHeadViewFieldList[i].Description;
            }
            int j = this.SAPHeadViewFieldList.Length;
            for (int i = 0; i < strHeaderList.Length; i++)
            {
                strHeader[i + j] = strHeaderList[i];
            }
            return strHeader;
        }

        #endregion

        #region Method

        private bool PoToSap105(object[] asnDetailList)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            _WarehouseFacade = new WarehouseFacade(base.DataProvider);

            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            List<PoLog> logList = new List<PoLog>();
            int count = _WarehouseFacade.GetRecordCount(dbTime.DBDate, dbTime.DBTime);
            try
            {
                string stno = "";
                string dqmcode = "";
                string stline = "";
                bool isSuccess = true;
                foreach (ASNDetail asndetail in asnDetailList)
                {
                    stno = asndetail.StNo;
                    dqmcode = asndetail.DQMCode;
                    stline = asndetail.StLine;
                    //ȡ��detailitem�У�������䵽��Щpo��
                    object[] objs_item = _InventoryFacade.GetInvoiceLineFromASNDetailItem(stno, stline);
                    if (objs_item == null)
                    {
                        this.DataProvider.RollbackTransaction();
                        WebInfoPublish.Publish(this, "asndetailitem������", this.languageComponent1);
                        return false;
                    }
                    ASN asn = (ASN)_InventoryFacade.GetASN(stno);

                    BenQGuru.eMES.SAPRFCService.POToSAP poToSAP = new POToSAP(this.DataProvider);
                    List<PO> list = new List<PO>();

                    // item��ÿ��invline���в���
                    foreach (Asndetailitem item in objs_item)
                    {
                        //ȡPO�е���Ϣ
                        object invoicesDetaillist = _InventoryFacade.GetInvoicesDetail(item.Invno, int.Parse(item.Invline));
                        if (invoicesDetaillist == null)
                        {
                            WebInfoPublish.Publish(this, "SAP���ݺŲ������е���Ŀ�����ݲ�����", this.languageComponent1);
                            this.DataProvider.CommitTransaction();
                            return false;
                        }
                        InvoicesDetail invdetail = invoicesDetaillist as InvoicesDetail;
                        object obj_item = _InventoryFacade.GetQcRejectQtyFromASNDetailItem(item.Stno, item.Invno, item.Invline, item.Stline);
                        if (obj_item == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "SAP���ݺŲ������е���Ŀ������item�в�����", this.languageComponent1);
                            return false;
                        }
                        decimal actQTY = (obj_item as Asndetailitem).ActQty;
                        //Domain.MOModel.Material material =
                        //    (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(detail.DQMCode);
                        #region ��¼log
                        PoLog poLog = new PoLog();
                        int serial = _InventoryFacade.GetMaxSerialInPoLog() + 1;
                        poLog.Serial = serial;
                        poLog.PONO = item.Invno;
                        poLog.PoLine = item.Invline.ToString();
                        poLog.FacCode = asn.FacCode;
                        poLog.SerialNO = asn.StNo; // asndetail.s;
                        poLog.MCode = item.MCode;//SAPMcode
                        poLog.Qty = actQTY; // 
                        poLog.Unit = invdetail.Unit; //asndetailObj.Unit;
                        poLog.Status = "105"; // 
                        poLog.Operator = asn.CUser; // asndetail.;
                        poLog.VendorInvoice = asn.InvNo;
                        poLog.StorageCode = asn.StorageCode;
                        poLog.Remark = asn.Remark1;
                        poLog.InvoiceDate = asn.MaintainDate;
                        poLog.SapDateStamp = dbTime.DBDate;
                        poLog.SapTimeStamp = dbTime.DBTime;
                        if (count > 0) //P�ش�
                        {
                            poLog.SAPMaterialInvoice = "";
                            poLog.IsPBack = "";
                            poLog.SapReturn = "";
                        }
                        else
                        {
                            poLog.IsPBack = "Actual";
                        }
                        logList.Add(poLog);
                        #endregion

                        #region �ش��ӿ�

                        PO po = new PO();
                        po.PONO = item.Invno;
                        po.POLine = int.Parse(item.Invline);
                        po.FacCode = asn.FacCode;
                        po.ZNUMBER = serial.ToString();
                        po.SerialNO = asn.StNo; // asndetail.s;
                        po.MCode = invdetail.MCode;//SAPMcode
                        po.Qty = actQTY; //���� ��������
                        po.Unit = invdetail.Unit; //asndetailObj.Unit;
                        po.Status = "105"; //����
                        PoLog oldPoLogs =
                            (PoLog)
                            _Invenfacade.GetPoLog(po.PONO, po.POLine.ToString(), po.SerialNO, "103");
                        if (oldPoLogs != null)
                        {
                            po.SAPMaterialInvoice = oldPoLogs.SAPMaterialInvoice;
                        }
                        po.Operator = asn.CUser;
                        po.VendorInvoice = asn.InvNo;
                        po.StorageCode = asn.StorageCode;
                        po.Remark = asn.Remark1;
                        po.InvoiceDate = asn.MaintainDate;
                        list.Add(po);
                        #endregion

                    }
                    SAPRfcReturn msg = poToSAP.POReceiveToSAP(list);
                    #region ������˷���false
                    if (msg.Result.Trim().ToUpper() == "E")
                    {
                        isSuccess = false;
                        this.DataProvider.RollbackTransaction();
                        WebInfoPublish.Publish(this, "SAP��д����", this.languageComponent1);
                    }
                    #endregion
                    #region дlog
                    foreach (PoLog poLog in logList)
                    {

                        if (count > 0)//P�ش�
                        {
                            poLog.SAPMaterialInvoice = "";//����ʱ�ţ�P��Ϊ��
                            poLog.SapReturn = "";
                        }
                        else
                        {
                            poLog.SAPMaterialInvoice = Getstring(msg.MaterialDocument); //
                            poLog.SapReturn = Getstring(msg.Result);  // msg.Result;//(S��ʾ�ɹ���E��ʾʧ��)
                            poLog.Message = Getstring(msg.Message);
                        }
                        _InventoryFacade.AddPoLog(poLog);

                    }
                    #endregion
                    if (!isSuccess)
                    {
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                #region дlog
                foreach (PoLog poLog in logList)
                {

                    if (count > 0)//P�ش�
                    {
                        poLog.SAPMaterialInvoice = "";//����ʱ�ţ�P��Ϊ��
                        poLog.SapReturn = "";
                    }
                    else
                    {
                        poLog.SAPMaterialInvoice = Getstring("ERROR"); //
                        poLog.SapReturn = Getstring("ERROR");  // msg.Result;//(S��ʾ�ɹ���E��ʾʧ��)
                        poLog.Message = Getstring(ex.Message);
                    }
                    _InventoryFacade.AddPoLog(poLog);

                }
                #endregion

                return false;
            }

        }
        private string Getstring(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
            return "";
        }



        //private bool PoToSap103(object[] asnDetailList, string sapCode, out string message, out string sapInvoice)
        //{
        //    return PoToSap(asnDetailList, sapCode, out  message, string.Empty, out sapInvoice);

        //}

        //private bool PoToSap105(object[] asnDetailList, string sapInvoice, out string message)
        //{
        //    string sapInvoice1 = string.Empty;
        //    return PoToSap(asnDetailList, "105", out message, sapInvoice, out sapInvoice1);
        //}


        private bool PoToSap(object[] asnDetailList, string sapCode, out string message, string sapInvoice, out string sapInvoice1)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            sapInvoice1 = string.Empty;
            message = string.Empty;
            _WarehouseFacade = new WarehouseFacade(base.DataProvider);

            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(base.DataProvider);

            bool isSuccess = true;
            List<string> stlines = new List<string>();
            foreach (ASNDetail asndetail in asnDetailList)
            {
                stlines.Add(asndetail.StLine);
            }
            List<PO> list = new List<PO>();
            List<PoLog> logList = new List<PoLog>();

            string stno = ((ASNDetail)asnDetailList[0]).StNo;
            ASN asn = (ASN)_InventoryFacade.GetASN(stno);
            object[] obj_item = _InventoryFacade.GetQcRejectQtyFromASNDetailItem1(stno, stlines);

            foreach (Asndetailitem itemNew in obj_item)
            {

                object invoicesDetaillist = _InventoryFacade.GetInvoicesDetail(asn.InvNo, int.Parse(itemNew.Invline));
                InvoicesDetail invdetail = invoicesDetaillist as InvoicesDetail;
                decimal actQTY = itemNew.ActQty;
                if (actQTY <= 0)
                    continue;



                PoLog poLog = new PoLog();
                int serial = _InventoryFacade.GetMaxSerialInPoLog() + 1;
                poLog.Serial = serial;
                poLog.ZNUMBER = serial.ToString();
                poLog.PONO = asn.InvNo;
                poLog.PoLine = itemNew.Invline.ToString();
                poLog.FacCode = asn.FacCode;
                poLog.SerialNO = asn.StNo; // asndetail.s;
                poLog.MCode = invdetail.MCode;
                poLog.Qty = actQTY; // 
                poLog.Unit = invdetail.Unit; //asndetailObj.Unit;
                poLog.Status = sapCode; // 
                poLog.Operator = GetUserCode(); // asndetail.;
                poLog.VendorInvoice = asn.InvNo;
                poLog.StorageCode = asn.StorageCode;
                poLog.Remark = asn.Remark1;
                poLog.InvoiceDate = asn.MaintainDate;
                poLog.SapDateStamp = dbTime.DBDate;
                poLog.SapTimeStamp = dbTime.DBTime;

                logList.Add(poLog);




                PO po = new PO();
                po.PONO = asn.InvNo;
                po.POLine = int.Parse(itemNew.Invline);
                po.FacCode = asn.FacCode;
                po.ZNUMBER = serial.ToString();
                po.SerialNO = asn.StNo; // asndetail.s;
                po.MCode = invdetail.MCode;//SAPMcode
                po.Qty = actQTY; //���� ��������
                po.Unit = invdetail.Unit; //asndetailObj.Unit;
                po.Status = sapCode; //����
                po.Operator = GetUserCode();
                po.VendorInvoice = asn.InvNo;
                po.StorageCode = asn.StorageCode;
                po.Remark = asn.Remark1;
                po.InvoiceDate = asn.MaintainDate;
                po.SAPMaterialInvoice = sapInvoice;
                list.Add(po);

            }

            if (list.Count <= 0)
            {
                message = "��д������Ϊ�գ�";
                return false;
            }

            bool is2Sap = _WarehouseFacade.GetRecordCount(dbTime.DBDate, dbTime.DBTime) > 0;

            if (is2Sap)
            {
                LogPO2Sap(list);
                message = "�ѹ������ڣ���дʧ�ܣ�";
                return true;
            }
            else
            {
                #region SAP
                BenQGuru.eMES.SAPRFCService.POToSAP poToSAP = new POToSAP(this.DataProvider);
                SAPRfcReturn msg = poToSAP.POReceiveToSAP(list);


                if (sapCode == "103")
                    sapInvoice1 = msg.MaterialDocument;
                #region ������˷���false

                if (msg.Result.Trim().ToUpper() == "E")
                {
                    isSuccess = false;

                    message = msg.Message;
                }

                #endregion

                #region дlog

                foreach (PoLog poLog in logList)
                {


                    poLog.SAPMaterialInvoice = Getstring(msg.MaterialDocument); //
                    poLog.SapReturn = Getstring(msg.Result); // msg.Result;//(S��ʾ�ɹ���E��ʾʧ��)
                    poLog.Message = Getstring(msg.Message);

                    _InventoryFacade.AddPoLog(poLog);

                }

                #endregion

                if (!isSuccess)
                {
                    return false;
                }
                #endregion
            }

            ShareLib.ShareKit.PoToSupport(list, false);
            return true;

        }


        private void LogPO2Sap(List<BenQGuru.eMES.SAPRFCService.Domain.PO> dns)
        {
            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            foreach (BenQGuru.eMES.SAPRFCService.Domain.PO po in dns)
            {

                Po2Sap poLog = new Po2Sap();
                poLog.PONO = po.PONO;
                poLog.POLine = po.POLine;
                poLog.SerialNO = po.SerialNO;
                poLog.Qty = po.Qty; // 
                poLog.STNO = po.SerialNO;
                poLog.Unit = po.Unit;
                poLog.FacCode = po.FacCode;
                poLog.InvoiceDate = po.InvoiceDate; //  
                poLog.MCode = po.MCode;//SAPMcode
                poLog.SAPMaterialInvoice = po.SAPMaterialInvoice;
                poLog.Operator = po.Operator;
                poLog.Status = po.Status;
                poLog.VendorInvoice = po.VendorInvoice;
                poLog.StorageCode = po.StorageCode;
                poLog.Remark = po.Remark;
                poLog.SapDateStamp = dbTime.DBDate;
                poLog.SapTimeStamp = dbTime.DBTime;
                poLog.ZNUMBER = po.ZNUMBER;

                //poLog.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
                //poLog.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                //poLog.MaintainUser = GetUserCode();
                //poLog.r = "empty";
                //poLog.Message = "empty";
                _InventoryFacade.AddPo2Sap(poLog);
            }
        }

        //private bool PoToSap103(string[] asnDetailList)
        //{
        //    if (_InventoryFacade == null)
        //    {
        //        _InventoryFacade = new InventoryFacade(base.DataProvider);
        //    }
        //    _WarehouseFacade = new WarehouseFacade(base.DataProvider);
        //    string stno = this.txtStorageInASNQuery.Text.Trim().ToUpper();
        //    if (asnDetailList != null)
        //    {
        //        stno = asnDetailList[0];
        //    }
        //    DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
        //    int count = _WarehouseFacade.GetRecordCount(dbTime.DBDate, dbTime.DBTime);
        //    ASN asn = (ASN)_InventoryFacade.GetASN(stno);
        //    #region check
        //    //if (asn.StType != SAP_ImportType.SAP_POR)
        //    //{
        //    //    WebInfoPublish.Publish(this, "ASN�������ͱ���ΪPO���", this.languageComponent1);
        //    //    return;
        //    //}
        //    //b)	���STNO�����е��е�״̬�Ƿ�ȫ����(ReceiveClose:�������),�������,���˳�
        //    bool hasDetail = _InventoryFacade.CheckASNHasDetail(asn.StNo, ASNLineStatus.Close);
        //    if (hasDetail)
        //    {
        //        WebInfoPublish.Publish(this, "���STNO�����е��е�״̬������ȫ���ǳ������", this.languageComponent1);
        //        //this.DataProvider.CommitTransaction();
        //        this.DataProvider.RollbackTransaction();
        //        return false;
        //    }
        //    #endregion
        //    #region ����103

        //    //cͨ��tblasn����invno SAP���ݺ�.ͨ��SAP���ݺŲ������е���Ŀ�У�select *from tblinvoicesdetail��
        //    object[] invoicesDetaillist = _InventoryFacade.GetInvoicesDetailByInvNoAndStno(asn.InvNo, stno);//GetInvoicesDetailByInvNo
        //    if (invoicesDetaillist == null)
        //    {
        //        WebInfoPublish.Publish(this, "SAP���ݺŲ������е���Ŀ�����ݲ�����", this.languageComponent1);
        //        //this.DataProvider.CommitTransaction();
        //        this.DataProvider.RollbackTransaction();
        //        return false;
        //    }
        //    List<int> serialList = new List<int>();

        //    #region tblpolog
        //    foreach (InvoicesDetail detail in invoicesDetaillist)
        //    {
        //        //i.	����ÿ����Ŀ�еľ���������select sum��ReceiveQTY�� from tblasndetailitem where stno=��***��and invno=��***��and invline=��***���� 
        //        int receiveQty = _InventoryFacade.GetReceiveQtyInAsn(stno, detail.InvNo, detail.InvLine);
        //        Domain.MOModel.Material material = (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(detail.DQMCode);
        //        //_WarehouseFacade.GetSTLineInPOIntridution(detail.InvNo, detail.InvLine.ToString(), stno);
        //        #region �ش�
        //        PoLog poLog = new PoLog();
        //        int serial = _InventoryFacade.GetMaxSerialInPoLog() + 1;
        //        poLog.Serial = serial;
        //        poLog.PONO = detail.InvNo;
        //        poLog.PoLine = detail.InvLine.ToString();
        //        poLog.FacCode = asn.FacCode;
        //        poLog.SerialNO = asn.StNo;// asndetail.s;
        //        poLog.MCode = detail.MCode;//SAPMcode
        //        poLog.Qty = receiveQty;//���� ��������
        //        if (material != null)
        //        {
        //            poLog.Unit = material.Muom;//asndetailObj.Unit;
        //        }
        //        poLog.Status = "103";//����
        //        poLog.SAPMaterialInvoice = "";
        //        poLog.Operator = asn.CUser;// asndetail.;
        //        poLog.VendorInvoice = asn.InvNo;
        //        poLog.StorageCode = asn.StorageCode;
        //        poLog.Remark = asn.Remark1;
        //        poLog.InvoiceDate = asn.MaintainDate;
        //        poLog.SapDateStamp = dbTime.DBDate;
        //        poLog.SapTimeStamp = dbTime.DBTime;
        //        if (count > 0)//P�ش�
        //        {
        //            poLog.IsPBack = "";
        //        }
        //        else
        //        {
        //            poLog.IsPBack = "Actual";
        //        }
        //        _InventoryFacade.AddPoLog(poLog);
        //        //int serial = _InventoryFacade.GetMaxSerialInPoLog();
        //        serialList.Add(serial);
        //        #endregion
        //    }
        //    #endregion

        //    #region POToSAP
        //    BenQGuru.eMES.SAPRFCService.POToSAP poToSAP = new POToSAP(this.DataProvider);
        //    List<PO> list = new List<PO>();
        //    foreach (InvoicesDetail detail in invoicesDetaillist)
        //    {
        //        //i.	����ÿ����Ŀ�еľ���������select sum��ReceiveQTY�� from tblasndetailitem where stno=��***��and invno=��***��and invline=��***���� 
        //        int receiveQty = _InventoryFacade.GetReceiveQtyInAsn(stno, detail.InvNo, detail.InvLine);
        //        Domain.MOModel.Material material = (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(detail.DQMCode);
        //        #region �ش�
        //        PO po = new PO();
        //        po.PONO = detail.InvNo;
        //        po.POLine = detail.InvLine;
        //        po.FacCode = asn.FacCode;
        //        po.SerialNO = asn.StNo;// asndetail.s;
        //        po.MCode = detail.MCode;//SAPMcode
        //        po.Qty = receiveQty;//���� ��������

        //        if (material != null)
        //        {
        //            po.Unit = material.Muom;//asndetailObj.Unit;
        //        }
        //        po.Status = "103";//����
        //        po.SAPMaterialInvoice = "";
        //        po.Operator = asn.CUser;// asndetail.;
        //        po.VendorInvoice = asn.InvNo;
        //        po.StorageCode = asn.StorageCode;
        //        po.Remark = asn.Remark1;
        //        po.InvoiceDate = asn.MaintainDate;
        //        list.Add(po);
        //        #endregion
        //    }
        //    #endregion

        //    #region �ش�
        //    try
        //    {
        //        SAPRfcReturn msg = poToSAP.POReceiveToSAP(list);
        //        //Log.Error(msg.Result);
        //        foreach (int serial in serialList)
        //        {
        //            PoLog poLog = (PoLog)_InventoryFacade.GetPoLog(serial);
        //            if (count > 0)//P�ش�
        //            {
        //                poLog.SAPMaterialInvoice = "";//����ʱ�ţ�P��Ϊ��
        //                poLog.SapReturn = "";
        //            }
        //            else
        //            {
        //                poLog.SAPMaterialInvoice = Getstring(msg.MaterialDocument); //
        //                poLog.SapReturn = Getstring(msg.Result);  // msg.Result;//(S��ʾ�ɹ���E��ʾʧ��)
        //                poLog.Message = Getstring(msg.Message);
        //            }
        //            _InventoryFacade.UpdatePoLog(poLog);
        //            #region ����ع�
        //            if (msg.Result.ToUpper() == "E")
        //            {
        //                string Mess = msg.Message;
        //                WebInfoPublish.PublishInfo(this, "�ش�SAP����", this.languageComponent1);

        //                #region tblpolog

        //                foreach (InvoicesDetail detail in invoicesDetaillist)
        //                {
        //                    //i.	����ÿ����Ŀ�еľ���������select sum��ReceiveQTY�� from tblasndetailitem where stno=��***��and invno=��***��and invline=��***���� 
        //                    int receiveQty = _InventoryFacade.GetReceiveQtyInAsn(stno, detail.InvNo, detail.InvLine);
        //                    Domain.MOModel.Material material = (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(detail.DQMCode);
        //                    //_WarehouseFacade.GetSTLineInPOIntridution(detail.InvNo, detail.InvLine.ToString(), stno);
        //                    #region �ش�
        //                    PoLog poLog1 = new PoLog();
        //                    int serial1 = _InventoryFacade.GetMaxSerialInPoLog() + 1;
        //                    poLog1.Serial = serial1;
        //                    poLog1.PONO = detail.InvNo;
        //                    poLog1.PoLine = detail.InvLine.ToString();
        //                    poLog1.FacCode = asn.FacCode;
        //                    poLog1.SerialNO = asn.StNo;// asndetail.s;
        //                    poLog1.MCode = detail.MCode;//SAPMcode
        //                    poLog1.Qty = receiveQty;//���� ��������
        //                    if (material != null)
        //                    {
        //                        poLog1.Unit = material.Muom;//asndetailObj.Unit;
        //                    }
        //                    poLog1.Status = "103";//����
        //                    poLog1.SAPMaterialInvoice = "";
        //                    poLog1.Operator = asn.CUser;// asndetail.;
        //                    poLog1.VendorInvoice = asn.InvNo;
        //                    poLog1.StorageCode = asn.StorageCode;
        //                    poLog1.Remark = asn.Remark1;
        //                    poLog1.InvoiceDate = asn.MaintainDate;
        //                    poLog1.SapDateStamp = dbTime.DBDate;
        //                    poLog1.SapTimeStamp = dbTime.DBTime;
        //                    poLog1.SapReturn = "E";
        //                    poLog1.Message = Mess;
        //                    if (count > 0)//P�ش�
        //                    {
        //                        poLog1.IsPBack = "";
        //                    }
        //                    else
        //                    {
        //                        poLog1.IsPBack = "Actual";
        //                    }
        //                    _InventoryFacade.AddPoLog(poLog1);

        //                    #endregion
        //                }
        //                #endregion

        //                return false;
        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //        //this.DataProvider.CommitTransaction();
        //        this.DataProvider.RollbackTransaction();
        //        #region tblpolog

        //        foreach (InvoicesDetail detail in invoicesDetaillist)
        //        {
        //            //i.	����ÿ����Ŀ�еľ���������select sum��ReceiveQTY�� from tblasndetailitem where stno=��***��and invno=��***��and invline=��***���� 
        //            int receiveQty = _InventoryFacade.GetReceiveQtyInAsn(stno, detail.InvNo, detail.InvLine);
        //            Domain.MOModel.Material material = (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(detail.DQMCode);
        //            //_WarehouseFacade.GetSTLineInPOIntridution(detail.InvNo, detail.InvLine.ToString(), stno);
        //            #region �ش�
        //            PoLog poLog1 = new PoLog();
        //            int serial1 = _InventoryFacade.GetMaxSerialInPoLog() + 1;
        //            poLog1.Serial = serial1;
        //            poLog1.PONO = detail.InvNo;
        //            poLog1.PoLine = detail.InvLine.ToString();
        //            poLog1.FacCode = asn.FacCode;
        //            poLog1.SerialNO = asn.StNo;// asndetail.s;
        //            poLog1.MCode = detail.MCode;//SAPMcode
        //            poLog1.Qty = receiveQty;//���� ��������
        //            if (material != null)
        //            {
        //                poLog1.Unit = material.Muom;//asndetailObj.Unit;
        //            }
        //            poLog1.Status = "103";//����
        //            poLog1.SAPMaterialInvoice = "";
        //            poLog1.Operator = asn.CUser;// asndetail.;
        //            poLog1.VendorInvoice = asn.InvNo;
        //            poLog1.StorageCode = asn.StorageCode;
        //            poLog1.Remark = asn.Remark1;
        //            poLog1.InvoiceDate = asn.MaintainDate;
        //            poLog1.SapDateStamp = dbTime.DBDate;
        //            poLog1.SapTimeStamp = dbTime.DBTime;
        //            poLog1.SapReturn = "E";
        //            poLog1.Message = ex.Message;
        //            if (count > 0)//P�ش�
        //            {
        //                poLog1.IsPBack = "";
        //            }
        //            else
        //            {
        //                poLog1.IsPBack = "Actual";
        //            }
        //            _InventoryFacade.AddPoLog(poLog1);

        //            #endregion
        //        }
        //        #endregion

        //        return false;
        //    }

        //    #endregion

        //    #endregion
        //    return true;
        //}
        //���
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="asn">ASNʵ��</param>
        private void ToInStorage(ASN asn)
        {
            //a)	������¼�������ϸ��Ϣ��(TBLStorageDetail)
            //b)	������¼�������ϸSN��Ϣ��(TBLStorageDetailSN)
            //c)	������¼������⽻����ϸ(TBLInvInOutTrans)
            //d)	ͨ����ǰ���ָ��ż��ASN��ϸ��(TBLASNDETAIL)���м�¼��״̬Ϊ��Close:��� or Cancel:ȡ��ʱ��ASN����(TBLASN)״̬(TBLASN .STATUS)����Ϊ��Close:����Cancelȡ��
            //e)	ͨ����ǰ���ָ����ҵ�ASN����(TBLASN)��SAP���ݺ�(INVNO)����ͨ��SAP���ݺ�(INVNO)���SAP������ϸ��(TBLInvoicesDetail)���м�¼��PLANQTY= ACTQTYʱ��SAP���ݱ�(TBLInvoices) SAP�������״̬(FINISHFLAG)����Ϊ��Y
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            object[] objAsnDetails = _InventoryFacade.GetASNDetailByStNo(asn.StNo);
            if (objAsnDetails != null)
            {
                foreach (ASNDetail asnDetail in objAsnDetails)
                {
                    //�����ϸ
                    StorageDetail storageDetail = _InventoryFacade.CreateNewStorageDetail();
                    storageDetail.MCode = asnDetail.MCode;
                    storageDetail.StorageCode = "VirtualStorage";
                    storageDetail.LocationCode = "VirtualLocation";
                    storageDetail.CartonNo = asnDetail.CartonNo;
                    storageDetail.DQMCode = asnDetail.DQMCode;
                    storageDetail.MDesc = asnDetail.MDesc;
                    storageDetail.Unit = asnDetail.Unit;
                    storageDetail.StorageQty = asnDetail.ActQty;
                    storageDetail.AvailableQty = asnDetail.ActQty;
                    storageDetail.FreezeQty = 0;
                    storageDetail.SupplierLotNo = asnDetail.SupplierLotNo;
                    storageDetail.Lotno = asnDetail.LotNo;
                    storageDetail.ProductionDate = string.IsNullOrEmpty(asnDetail.ProductionDate.ToString()) ?
                                                        FormatHelper.GetNowDBDateTime(base.DataProvider).DBDate : asnDetail.ProductionDate;

                    storageDetail.StorageAgeDate = string.IsNullOrEmpty(asnDetail.StorageAgeDate.ToString()) ?
                                                        FormatHelper.GetNowDBDateTime(base.DataProvider).DBDate : asnDetail.StorageAgeDate;

                    storageDetail.LastStorageAgeDate = FormatHelper.GetNowDBDateTime(base.DataProvider).DBDate;
                    storageDetail.AvailableQty = asnDetail.ActQty;
                    storageDetail.FacCode = asn.FacCode;
                    storageDetail.ReworkApplyUser = asn.ReWorkApplyUser;
                    storageDetail.CUser = this.GetUserCode();
                    storageDetail.CDate = FormatHelper.GetNowDBDateTime(base.DataProvider).DBDate;
                    storageDetail.CTime = FormatHelper.GetNowDBDateTime(base.DataProvider).DBTime;
                    storageDetail.MaintainUser = this.GetUserCode();

                    //����⽻����ϸ
                    WarehouseFacade _WarehouseFacade = new WarehouseFacade(base.DataProvider);
                    InvInOutTrans invInOutTrans = _WarehouseFacade.CreateNewInvInOutTrans();
                    invInOutTrans.TransNO = asn.StNo;
                    invInOutTrans.TransType = "IN";
                    invInOutTrans.InvType = asn.StType;
                    invInOutTrans.InvNO = asn.InvNo;
                    invInOutTrans.CartonNO = asnDetail.CartonNo;
                    invInOutTrans.MCode = asnDetail.MCode;
                    invInOutTrans.DqMCode = asnDetail.DQMCode;
                    invInOutTrans.Qty = asnDetail.Qty;
                    invInOutTrans.Unit = asnDetail.Unit;
                    invInOutTrans.FacCode = asn.FacCode;
                    invInOutTrans.StorageCode = asn.StorageCode;
                    invInOutTrans.FromFacCode = asn.FromFacCode;
                    invInOutTrans.FromStorageCode = asn.FromStorageCode;
                    invInOutTrans.ProductionDate = asnDetail.ProductionDate;
                    invInOutTrans.SupplierLotNo = asnDetail.SupplierLotNo;
                    invInOutTrans.LotNo = asnDetail.LotNo;
                    invInOutTrans.StorageAgeDate = asnDetail.StorageAgeDate;
                    invInOutTrans.MaintainUser = this.GetUserCode();
                    invInOutTrans.MaintainDate = FormatHelper.GetNowDBDateTime(base.DataProvider).DBDate;
                    invInOutTrans.MaintainTime = FormatHelper.GetNowDBDateTime(base.DataProvider).DBTime;


                    _InventoryFacade.AddStorageDetail(storageDetail);
                    _WarehouseFacade.AddInvInOutTrans(invInOutTrans);

                }
            }
            object[] objAsnDetailSn = _InventoryFacade.GetASNDetaileSNByStNo(asn.StNo);
            if (objAsnDetailSn != null)
            {
                foreach (Asndetailsn asnDetailSn in objAsnDetailSn)
                {
                    StorageDetailSN storageDetailSn = _InventoryFacade.CreateNewStorageDetailSN();
                    storageDetailSn.CartonNo = asnDetailSn.Cartonno;
                    storageDetailSn.SN = asnDetailSn.Sn;
                    storageDetailSn.PickBlock = "N";
                    storageDetailSn.CUser = this.GetUserCode();
                    storageDetailSn.CDate = FormatHelper.GetNowDBDateTime(base.DataProvider).DBDate;
                    storageDetailSn.CTime = FormatHelper.GetNowDBDateTime(base.DataProvider).DBTime;
                    storageDetailSn.MaintainUser = this.GetUserCode();

                    _InventoryFacade.AddStorageDetailSN(storageDetailSn);
                }
            }

            //�������ָ����״̬
            _InventoryFacade.UpdateASNDetailStatusByStNo(ASNLineStatus.Close, asn.StNo, ASNLineStatus.OnLocation);

            //�������ָ��ͷ״̬
            _InventoryFacade.UpdateASNStatusByStNo(ASNLineStatus.Close, asn.StNo);

            //����SAP�������״̬
            if (CheckIsAllPlanQtyEqualActQty(asn.InvNo))
            {
                Invoices invoices = (Invoices)_InventoryFacade.GetInvoices(asn.InvNo);
                invoices.FinishFlag = "Y";
                invoices.AsnAvailable = "N";
                _InventoryFacade.UpdateInvoices(invoices);
            }


        }

        //���SAP������ϸ����������PlanQty��ActQty�Ƿ����
        /// <summary>
        /// ���SAP������ϸ����������PlanQty��ActQty�Ƿ����
        /// </summary>
        /// <param name="invNo">SAP���ݺ�</param>
        /// <returns>��ȣ�true;����ȣ�false</returns>
        private bool CheckIsAllPlanQtyEqualActQty(string invNo)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            object[] objInvoicesDetail = _InventoryFacade.GetInvoicesDetailByInvNo(invNo);
            if (objInvoicesDetail != null)
            {
                foreach (InvoicesDetail invoicesDetail in objInvoicesDetail)
                {
                    if (invoicesDetail.PlanQty != invoicesDetail.ActQty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //���ASN��ϸ���������Ƿ�ȡ��
        /// <summary>
        /// ���ASN��ϸ���������Ƿ�ȡ��
        /// </summary>
        /// <param name="invNo">SAP���ݺ�</param>
        /// <returns>ȡ����true;δȡ����false</returns>
        private bool CheckIsAllCancel(string stNo)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            object[] objASNDetail = _InventoryFacade.GetASNDetailByStNo(stNo);
            if (objASNDetail != null)
            {
                foreach (ASNDetail asnDetail in objASNDetail)
                {
                    if (asnDetail.Status != ASNLineStatus.Cancel)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //���ASN��ϸ���������Ƿ��ϼ�
        /// <summary>
        /// ���ASN��ϸ���������Ƿ��ϼ�
        /// </summary>
        /// <param name="invNo">SAP���ݺ�</param>
        /// <returns>�ϼܣ�true;δ�ϼܣ�false</returns>
        private bool CheckIsAllOnLocation(string stNo)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            object[] objASNDetail = _InventoryFacade.GetASNDetailByStNo(stNo);
            if (objASNDetail != null)
            {
                foreach (ASNDetail asnDetail in objASNDetail)
                {
                    if (asnDetail.Status == ASNLineStatus.Cancel)
                    {
                        continue;//�ų�ȡ��״̬��
                    }
                    if (asnDetail.Status != ASNLineStatus.OnLocation)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
        private ViewField[] viewFieldList = null;
        private ViewField[] SAPHeadViewFieldList
        {
            get
            {
                if (viewFieldList == null)
                {
                    if (_InventoryFacade == null)
                    {
                        _InventoryFacade = new InventoryFacade(base.DataProvider);
                    }
                    object[] objs = _InventoryFacade.QueryViewFieldByUserCode(this.GetUserCode(), "TBLASN");
                    if (objs != null)
                    {
                        viewFieldList = new ViewField[objs.Length];
                        objs.CopyTo(viewFieldList, 0);
                    }
                    else
                    {
                        objs = _InventoryFacade.QueryViewFieldDefault("ASN_FIELD_LIST_SYSTEM_DEFAULT", "TBLASN");
                        if (objs != null)
                        {
                            ArrayList list = new ArrayList();
                            for (int i = 0; i < objs.Length; i++)
                            {
                                ViewField field = (ViewField)objs[i];
                                if (FormatHelper.StringToBoolean(field.IsDefault) == true)
                                {
                                    list.Add(field);
                                }
                            }
                            viewFieldList = new ViewField[list.Count];
                            list.CopyTo(viewFieldList, 0);
                        }
                    }
                    if (viewFieldList != null)
                    {
                        bool bExistPickNo = false;
                        for (int i = 0; i < viewFieldList.Length; i++)
                        {
                            if (viewFieldList[i].FieldName == "StNo")
                            {
                                bExistPickNo = true;
                                break;
                            }
                        }
                        if (bExistPickNo == false)
                        {
                            ViewField field = new ViewField();
                            field.FieldName = "StNo";
                            field.Description = "���ָ���";
                            ArrayList list = new ArrayList();
                            list.Add(field);
                            list.AddRange(viewFieldList);
                            viewFieldList = new ViewField[list.Count];
                            list.CopyTo(viewFieldList, 0);
                        }
                    }
                }
                return viewFieldList;
            }
        }
        //add by sam
        protected override void gridWebGrid_ItemCommand(GridRecord row, string commandName)
        {
            if (commandName == "Edit")
            {
                object obj = this.GetEditObject(row);

                if (obj != null)
                {
                    ASN asn = obj as ASN;
                    if (asn != null && asn.Status != Status.Release)
                    {
                        WebInfoPublish.Publish(this, "�·��󲻿��Ա༭", this.languageComponent1);
                        return;
                    }
                    if (drpStorageInTypeEdit.SelectedValue != "SCTR")
                    {
                        drpVendorCodeEdit.Enabled = false;

                    }
                    else
                        drpVendorCodeEdit.Enabled = true;
                    this.SetEditObject(obj);

                    this.buttonHelper.PageActionStatusHandle(PageActionType.Update);


                }
            }
            else
            {
                Grid_ClickCellButton(row, commandName);
                Grid_ClickCell(row, commandName);
            }
        }

    }
}


