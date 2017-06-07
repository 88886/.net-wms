using System;
using System.Collections.Generic;
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
using BenQGuru.eMES.SAPRFCService;
using BenQGuru.eMES.SAPRFCService.Domain;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
using BenQGuru.eMES.Domain.IQC;
using BenQGuru.eMES.IQC;
using BenQGuru.eMES.MOModel;
using System.Text;


namespace BenQGuru.eMES.Web.IQC
{
    public partial class FSQEJudgeMP : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private IQCFacade _IQCFacade = null;
        private SystemSettingFacade _SystemSettingFacade = null;
        private ItemFacade _ItemFacade = null;
        private InventoryFacade _InventoryFacade = null;
        private WarehouseFacade facade = null;
        private InventoryFacade _Invenfacade = null;
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
                this.InitIQCStatusList();
                InitStorageInASNList();

            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        private void InitStorageInASNList()
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }

            object[] objASN = _IQCFacade.GetASNIQCByStatus(IQCStatus.IQCStatus_SQEJudge);
            drpStorageInASNQuery.Items.Clear();
            this.drpStorageInASNQuery.Items.Add(new ListItem("", ""));
            if (objASN != null)
            {
                foreach (AsnIQC asn in objASN)
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
            this.drpIQCStatusQuery.Items.Add(new ListItem("", ""));
            this.drpIQCStatusQuery.Items.Add(new ListItem("��ʼ��", "Release"));
            this.drpIQCStatusQuery.Items.Add(new ListItem("������", "WaitCheck"));
            this.drpIQCStatusQuery.Items.Add(new ListItem("SQE�ж�", "SQEJudge"));
            this.drpIQCStatusQuery.Items.Add(new ListItem("IQC�������", "IQCClose"));
            this.drpIQCStatusQuery.Items.Add(new ListItem("ȡ��", "Cancel"));
            this.drpIQCStatusQuery.SelectedIndex = 3;
        }


        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("IQCNo", "IQC���鵥��", null);
            this.gridHelper.AddColumn("StorageInASN", "���ָ���", null);
            this.gridHelper.AddColumn("StorageInType", "�������", null);
            this.gridHelper.AddDataColumn("StType", "������ʹ���", true);
            this.gridHelper.AddColumn("SAPInvNo", "SAP���ݺ�", null);

            this.gridHelper.AddColumn("STORAGECODE1234", "����λ", null);
            this.gridHelper.AddColumn("DQMCode", "���ű���", null);
            this.gridHelper.AddColumn("DQMCODEDESC", "������������", null);
            this.gridHelper.AddColumn("DQMCode", "���ű���", null);
            this.gridHelper.AddColumn("VMCode", "��Ӧ�����ϱ���", null);
            this.gridHelper.AddColumn("Status", "״̬", null);
            this.gridHelper.AddDataColumn("IQCStatus", "IQC����״̬", true);
            this.gridHelper.AddColumn("IQCType", "���鷽ʽ", null);
            this.gridHelper.AddColumn("AQLResult", "AQL���", null);
            this.gridHelper.AddColumn("AppQty", "�ͼ�����", null);
            this.gridHelper.AddColumn("NGQty", "ȱ��Ʒ����", null);
            this.gridHelper.AddColumn("VendorNo", "��Ӧ�̴���", null);
            this.gridHelper.AddColumn("AppDate", "�ͼ�����", null);
            this.gridHelper.AddEditColumn("btnInspect", "�ж�");

            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.RequestData();//ҳ���ʼ������grid����
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            _InventoryFacade = new InventoryFacade(base.DataProvider);
            row["IQCNo"] = ((AsnIQCExt)obj).IqcNo;

            row["StorageInASN"] = ((AsnIQCExt)obj).StNo;
            row["StorageInType"] = this.GetInvInName(((AsnIQCExt)obj).StType);//������ͣ��������ͣ�
            row["StType"] = ((AsnIQCExt)obj).StType;
            row["SAPInvNo"] = ((AsnIQCExt)obj).InvNo;

            ASN a = (ASN)_InventoryFacade.GetASN(((AsnIQCExt)obj).StNo);
            if (a != null)
                row["STORAGECODE1234"] = a.StorageCode;
            else
                row["STORAGECODE1234"] = string.Empty;

            row["DQMCode"] = ((AsnIQCExt)obj).DQMCode;


            Domain.MOModel.Material m = (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(((AsnIQCExt)obj).DQMCode);
            if (m != null)
                row["DQMCODEDESC"] = m.MchlongDesc;
            else
                row["DQMCODEDESC"] = string.Empty;

            row["VMCode"] = ((AsnIQCExt)obj).VendorMCode;
            row["Status"] = this.GetStatusName(((AsnIQCExt)obj).Status);
            row["IQCStatus"] = ((AsnIQCExt)obj).Status;
            row["IQCType"] = FormatHelper.GetChName(((AsnIQCExt)obj).IqcType);
            row["AQLResult"] = FormatHelper.GetChName(((AsnIQCExt)obj).QcStatus);
            row["AppQty"] = ((AsnIQCExt)obj).AppQty;
            row["NGQty"] = ((AsnIQCExt)obj).NgQty;
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
                FormatHelper.CleanString(this.drpStorageInASNQuery.SelectedValue), "",
                "",
                FormatHelper.CleanString(this.txtIQCNoQuery.Text),
                FormatHelper.CleanString(this.drpIQCStatusQuery.SelectedValue),
                FormatHelper.TODateInt(this.txtAppBDateQuery.Text),
                FormatHelper.TODateInt(this.txtAppEDateQuery.Text), string.Empty, string.Empty,
                "", "", string.Empty,
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
                FormatHelper.CleanString(this.drpStorageInASNQuery.SelectedValue), "",
                "",
                FormatHelper.CleanString(this.txtIQCNoQuery.Text),
                FormatHelper.CleanString(this.drpIQCStatusQuery.SelectedValue),
                FormatHelper.TODateInt(this.txtAppBDateQuery.Text),
                FormatHelper.TODateInt(this.txtAppEDateQuery.Text), string.Empty, string.Empty, "", "", string.Empty);
        }

        #endregion

        #region Button

        //���Grid�а�ť
        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            if (_ItemFacade == null)
            {
                _ItemFacade = new ItemFacade(base.DataProvider);
            }
            if (commandName == "btnInspect")
            {
                string mControlType = string.Empty;//�ܿ�����
                string dQMCode = row.Items.FindItemByKey("DQMCode").Text.Trim();
                object objMaterial = _ItemFacade.GetMaterialByDQMCode(dQMCode);
                if (objMaterial != null)
                {
                    mControlType = ((Domain.MOModel.Material)objMaterial).MCONTROLTYPE;
                }
                string iqcNo = row.Items.FindItemByKey("IQCNo").Text.Trim();//IQC���ݺ�
                string stType = row.Items.FindItemByKey("StType").Text.Trim();//�������
                Response.Redirect(this.MakeRedirectUrl("FSQEProcessMP.aspx",
                                                        new string[] { "IQCNo", "StType", "MControlType" },
                                                        new string[] { iqcNo, stType, mControlType }));

                #region ע�� ��������ť������״̬��
                //string iqcStatus = row.Items.FindItemByKey("IQCStatus").Text.Trim();
                //if (iqcStatus == IQCStatus.IQCStatus_SQEJudge)
                //{
                //    string mControlType = string.Empty;//�ܿ�����
                //    string dQMCode = row.Items.FindItemByKey("DQMCode").Text.Trim();
                //    object objMaterial = _ItemFacade.GetMaterialByDQMCode(dQMCode);
                //    if (objMaterial != null)
                //    {
                //        mControlType = ((Domain.MOModel.Material)objMaterial).MCONTROLTYPE;
                //    }
                //    string iqcNo = row.Items.FindItemByKey("IQCNo").Text.Trim();//IQC���ݺ�
                //    string stType = row.Items.FindItemByKey("StType").Text.Trim();//�������
                //    Response.Redirect(this.MakeRedirectUrl("FSQEProcessMP.aspx",
                //                                            new string[] { "IQCNo", "StType", "MControlType" },
                //                                            new string[] { iqcNo, stType, mControlType }));
                //}
                //else
                //{
                //    WebInfoPublish.Publish(this, "״̬����SQE�ж��������ж�", this.languageComponent1);
                //}
                #endregion
            }

        }

        //����
        protected void cmdRejectbt_ServerClick(object sender, EventArgs e)
        {
            ArrayList array = this.gridHelper.GetCheckedRows();
            bool isReceive = false;
            List<string> iqclist = new List<string>();
            if (array.Count > 0)
            {
                StringBuilder sbShowMsg = new StringBuilder();
                foreach (GridRecord row in array)
                {
                    string iqcNo = row.Items.FindItemByKey("IQCNo").Value.ToString();
                    string inInvType = row.Items.FindItemByKey("StType").Value.ToString();
                    string status = row.Items.FindItemByKey("Status").Value.ToString();
                    if (status != "SQE�ж�")
                    {
                        sbShowMsg.AppendFormat("IQC���鵥��: {0} ״̬����SQE�ж������ܾ��� ", iqcNo);
                        continue;
                    }
                    if (inInvType == InInvType.PGIR)
                    {
                        sbShowMsg.AppendFormat("IQC���鵥��: {0} �������ΪPGI���ϣ����ܾ��� ", iqcNo);
                        continue;
                    }
                    try
                    {


                        //����
                        bool result = ToRejectbt(iqcNo);
                        if (!result)
                        {
                            //this.DataProvider.RollbackTransaction();
                            sbShowMsg.AppendFormat("IQC���鵥��: {0} �ϴ�SAPʧ�� ", iqcNo);
                            continue;
                        }

                        if (!iqclist.Contains(iqcNo))
                            iqclist.Add(iqcNo);

                    }
                    catch (Exception ex)
                    {
                        sbShowMsg.AppendFormat("IQC���鵥��: {0} {1} ", iqcNo, ex.Message);

                    }

                }
                if (sbShowMsg.Length > 0)
                {
                    string showMsg = sbShowMsg.ToString();
                    WebInfoPublish.Publish(this, showMsg, this.languageComponent1);
                }
                else
                {
                    isReceive = true;
                    WebInfoPublish.Publish(this, "���ճɹ�", this.languageComponent1);
                }

                this.gridHelper.RequestData();//ˢ��ҳ��
            }

            //#region SAP�ش� add by sam 2016��3��21��

            //if (isReceive)
            //{
            //    PoToSap(iqclist.ToArray());
            //}
            //#endregion
        }

        #endregion


        #region SAP�ش� add by sam 2016��3��21��

        private void PoToSap(string[] iqcNoList)
        {
            try
            {
                foreach (string iqcNo in iqcNoList)
                {
                    #region PoToSap
                    if (facade == null)
                    {
                        facade = new WarehouseFacade(base.DataProvider);
                    }
                    if (_Invenfacade == null)
                    {
                        _Invenfacade = new InventoryFacade(base.DataProvider);
                    }
                    DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                    int count = facade.GetRecordCount(dbTime.DBDate, dbTime.DBTime);
                    AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
                    #region
                    if (asnIqc != null)
                    {
                        string stno = asnIqc.StNo;
                        ASN asn = (ASN)_InventoryFacade.GetASN(stno);

                        #region 104 ����
                        //cͨ��tblasn����invno SAP���ݺ�.ͨ��SAP���ݺŲ������е���Ŀ�У�select *from tblinvoicesdetail��
                        object[] invoicesDetaillist = _InventoryFacade.GetInvoicesDetailByInvNoAndStno(asn.InvNo, stno);// .GetInvoicesDetailByInvNo(asn.InvNo);
                        if (invoicesDetaillist == null)
                        {
                            WebInfoPublish.Publish(this, "SAP���ݺŲ������е���Ŀ�����ݲ�����", this.languageComponent1);
                            return;
                        }
                        List<int> serialList = new List<int>();
                        List<PO> list = new List<PO>();
                        #region tblpolog

                        foreach (InvoicesDetail detail in invoicesDetaillist)
                        {
                            //i.	����ÿ����Ŀ�еľ���������select sum�� QTY�� from tblasndetailitem where stno=��***��and invno=��***��and invline=��***���� 
                            int receiveQty = _InventoryFacade.GetReceiveQtyInAsn(stno, detail.InvNo, detail.InvLine);
                            int qcPassQty = _InventoryFacade.GetQcPassQtyInAsn(stno, detail.InvNo, detail.InvLine);
                            int ngqty = receiveQty - qcPassQty;
                            Domain.MOModel.Material material =
                                (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(detail.DQMCode);

                            #region �ش�
                            PoLog poLog = new PoLog();
                            int serial = _InventoryFacade.GetMaxSerialInPoLog() + 1;
                            poLog.Serial = serial;
                            poLog.PONO = detail.InvNo;
                            poLog.PoLine = detail.InvLine.ToString();
                            poLog.FacCode = asn.FacCode;
                            poLog.SerialNO = asn.StNo; // asndetail.s;
                            poLog.MCode = detail.MCode;//SAPMcode
                            poLog.Qty = ngqty; // 
                            if (material != null)
                            {
                                poLog.Unit = material.Muom; //asndetailObj.Unit;
                            }
                            poLog.Status = "104"; // 
                            PoLog oldPoLogs =
                                (PoLog)
                                _Invenfacade.GetPoLog(poLog.PONO, poLog.PoLine.ToString(), poLog.SerialNO, "103");
                            if (oldPoLogs != null)
                            {
                                poLog.SAPMaterialInvoice = oldPoLogs.SAPMaterialInvoice;
                            }
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
                            _InventoryFacade.AddPoLog(poLog);

                            PO po = new PO();
                            po.PONO = detail.InvNo;
                            po.POLine = detail.InvLine;
                            po.FacCode = asn.FacCode;
                            po.SerialNO = asn.StNo; // asndetail.s;
                            po.MCode = detail.MCode;//SAPMcode
                            po.Qty = ngqty; //���� ��������
                            if (material != null)
                            {
                                po.Unit = material.Muom; //asndetailObj.Unit;
                            }
                            po.Status = "104"; //����

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

                            //int serial = _InventoryFacade.GetMaxSerialInPoLog();
                            serialList.Add(serial);

                            #endregion
                        }

                        #endregion

                        #region POToSAP

                        BenQGuru.eMES.SAPRFCService.POToSAP poToSAP = new POToSAP(this.DataProvider);

                        foreach (InvoicesDetail detail in invoicesDetaillist)
                        {
                            //i.	����ÿ����Ŀ�еľ���������select sum��ReceiveQTY�� from tblasndetailitem where stno=��***��and invno=��***��and invline=��***���� 
                            int receiveQty = _InventoryFacade.GetReceiveQtyInAsn(stno, detail.InvNo, detail.InvLine);
                            int qcPassQty = _InventoryFacade.GetQcPassQtyInAsn(stno, detail.InvNo, detail.InvLine);
                            int ngqty = receiveQty - qcPassQty;
                            Domain.MOModel.Material material =
                                (Domain.MOModel.Material)_InventoryFacade.GetMaterialByDQMCode(detail.DQMCode);

                            #region �ش�



                            #endregion
                        }

                        #endregion

                        #region �ش�
                        SAPRfcReturn msg = poToSAP.POReceiveToSAP(list);
                        foreach (int serial in serialList)
                        {
                            PoLog poLog = (PoLog)_InventoryFacade.GetPoLog(serial);
                            if (count > 0) //P�ش�
                            {
                                poLog.SAPMaterialInvoice = ""; //����ʱ�ţ�P��Ϊ��
                                poLog.SapReturn = "";
                            }
                            else
                            {
                                poLog.SAPMaterialInvoice = Getstring(msg.MaterialDocument); //
                                poLog.SapReturn = Getstring(msg.Result);  // msg.Result;//(S��ʾ�ɹ���E��ʾʧ��)
                                poLog.Message = Getstring(msg.Message);
                            }
                            _InventoryFacade.UpdatePoLog(poLog);
                        }

                        #endregion

                        #endregion

                    }
                    #endregion
                    #endregion
                }
            }
            catch (Exception ex)
            {
                BenQGuru.eMES.Common.Log.Error(ex.Message);
                BenQGuru.eMES.Common.Log.Error(ex.StackTrace);
                throw ex;
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
        #endregion

        #region SAP�ش�2 add by sam 2016��3��21��
        private void PoToSap2(string iqcNo)
        {
            #region PoToSap
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }
            if (_Invenfacade == null)
            {
                _Invenfacade = new InventoryFacade(base.DataProvider);
            }
            POToSAP poToSAP = new POToSAP();
            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            int count = facade.GetRecordCount(dbTime.DBDate, dbTime.DBTime);
            string datetime = dbTime.DBDate.ToString() + dbTime.DBTime.ToString().PadLeft(6, '0');
            AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
            // string cartonCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdit.Text));
            //  int receiveQty = _Invenfacade.GetQcPassQtyByStatus(asnIqc.StNo, asnIqc.DQMCode, "Reject");
            string seqstatus = "'" + SQEStatus.SQEStatus_Return + "'" + "," + "'" + SQEStatus.SQEStatus_Reform + "'";
            if (asnIqc != null)
            {
                string stno = asnIqc.StNo;
                int ngqty = _IQCFacade.GetAsnIQCDetailEcQtyByStatus("", "", asnIqc.IqcNo, asnIqc.StNo, seqstatus);
                if (asnIqc.IqcType == IQCType.IQCType_SpotCheck) //���
                {
                    //�Ƿ�ά��������Ϣ
                    object[] iqcEcList = _IQCFacade.GetAsnIQCDetailEcByStatus("", "", asnIqc.IqcNo, asnIqc.StNo, seqstatus);
                    if (iqcEcList != null)
                    {
                        #region
                        //ASN asnObj = (ASN)facade.GetAsn(stno);
                        //Invoices invoices = (Invoices)_Invenfacade.GetInvoices(asnObj.InvNo);
                        //object[] invoicesDetaillist = _Invenfacade.GetInvoicesDetailByInvNo(invoices.InvNo, asnIqc.DQMCode);
                        //#region check
                        //object[] allObjList = _Invenfacade.QueryAsnDetailByStNo(stno, asnIqc.DQMCode);
                        //if (allObjList == null)
                        //{
                        //    return;
                        //}
                        //Asndetail asndetailObj = allObjList[0] as Asndetail;
                        //if (asndetailObj == null)
                        //{
                        //    return;
                        //}
                        #endregion

                        if (asnIqc.QcStatus == "Y")//
                        {
                            #region 2.���Ľ���Ǻϸ�SQE�ж�����գ�iqc���ž���,SAP
                            ASN asnObj = (ASN)facade.GetAsn(stno);
                            Invoices invoices = (Invoices)_Invenfacade.GetInvoices(asnObj.InvNo);
                            object[] invoicesDetaillist = _Invenfacade.GetInvoicesDetailByInvNo(invoices.InvNo, asnIqc.DQMCode);

                            #region check
                            object[] allObjList = _Invenfacade.QueryAsnDetailByStNo(stno, asnIqc.DQMCode);
                            if (allObjList == null)
                            {
                                return;
                            }
                            Asndetail asndetailObj = allObjList[0] as Asndetail;
                            if (asndetailObj == null)
                            {
                                return;
                            }
                            #endregion
                            Domain.MOModel.Material material = (Domain.MOModel.Material)_Invenfacade.GetMaterialByDQMCode(asndetailObj.DqmCode);
                            #region POList
                            List<PO> list = new List<PO>();
                            foreach (InvoicesDetail detail in invoicesDetaillist)
                            {
                                #region  PO�ش�
                                PO po = new PO();
                                po.PONO = detail.InvNo;
                                po.POLine = detail.InvLine;
                                po.FacCode = asnObj.FacCode;
                                po.SerialNO = asnObj.StNo;// asndetailObj.s;
                                po.MCode = asndetailObj.DqmCode; //txtMCode.Text;
                                po.Qty = ngqty;//detailSN. 
                                if (material != null)
                                {
                                    po.Unit = material.Muom;//asndetailObj.Unit;
                                }
                                po.Status = "104";//IQC
                                PoLog oldPoLog = (PoLog)_Invenfacade.GetPoLog(po.PONO, po.POLine.ToString(), po.SerialNO, "103");
                                if (oldPoLog != null)
                                {
                                    po.SAPMaterialInvoice = oldPoLog.SAPMaterialInvoice;
                                }
                                po.Operator = asndetailObj.CUser;// asndetailObj.;
                                po.VendorInvoice = asnObj.InvNo;
                                po.StorageCode = asnObj.StorageCode;
                                po.Remark = asndetailObj.Remark1;
                                po.InvoiceDate = asndetailObj.MaintainDate;
                                list.Add(po);
                                #endregion

                                #region poLog
                                PoLog poLog = new PoLog();
                                poLog.PONO = po.PONO;
                                poLog.PoLine = po.POLine.ToString();
                                poLog.FacCode = po.FacCode; // asndetail.;
                                poLog.SerialNO = po.SerialNO;// asndetail.s;
                                poLog.MCode = po.MCode; //txtMCode.Text;
                                poLog.Qty = po.Qty;//detailSN. //Convert.ToDecimal(txtQty.Text);
                                poLog.Unit = po.Unit;
                                poLog.Status = po.Status;// ����

                                poLog.Operator = po.Operator;// asndetail.;
                                poLog.VendorInvoice = po.VendorInvoice;
                                poLog.StorageCode = po.StorageCode;
                                poLog.Remark = po.Remark;
                                poLog.InvoiceDate = po.InvoiceDate;
                                if (count > 0)//P�ش�
                                {
                                    poLog.IsPBack = "";
                                    poLog.SapReturn = "";
                                    poLog.SapTimeStamp = int.Parse(datetime);
                                    _Invenfacade.AddPoLog(poLog);
                                }
                                else
                                {
                                    SAPRfcReturn msg = poToSAP.POReceiveToSAP(list);
                                    poLog.IsPBack = "Actual";
                                    poLog.SapReturn = msg.Result;//(S��ʾ�ɹ���E��ʾʧ��)
                                    poLog.SapTimeStamp = int.Parse(datetime);
                                    _Invenfacade.AddPoLog(poLog);
                                }
                                #endregion
                            }
                            #endregion
                            #endregion
                        }
                        else
                        {
                            #region 1.���Ľ���ǲ��ϸ�SQE�ж�����գ�asn��ȫ������,SAP��д    PO�ش�

                            #region check
                            ASN asnObj = (ASN)facade.GetAsn(stno);
                            Invoices invoices = (Invoices)_Invenfacade.GetInvoices(asnObj.InvNo);
                            object[] invoicesDetaillist = _Invenfacade.GetInvoicesDetailByInvNo(invoices.InvNo);
                            object[] allObjList = _Invenfacade.QueryASNDetailBySTNoCheckStatus(stno, "Reject");
                            if (allObjList == null)
                            {
                                return;
                            }
                            Asndetail asndetailObj = allObjList[0] as Asndetail;
                            if (asndetailObj == null)
                            {
                                return;
                            }
                            #endregion
                            Domain.MOModel.Material material = (Domain.MOModel.Material)_Invenfacade.GetMaterialByDQMCode(asndetailObj.DqmCode);
                            #region POList
                            List<PO> list = new List<PO>();
                            foreach (InvoicesDetail detail in invoicesDetaillist)
                            {
                                #region  PO�ش�
                                PO po = new PO();
                                po.PONO = detail.InvNo;
                                po.POLine = detail.InvLine;
                                po.FacCode = asnObj.FacCode;
                                po.SerialNO = asnObj.StNo;// asndetailObj.s;
                                po.MCode = asndetailObj.DqmCode; //txtMCode.Text;
                                po.Qty = ngqty;//detailSN. 
                                if (material != null)
                                {
                                    po.Unit = material.Muom;//asndetailObj.Unit;
                                }
                                po.Status = "104";//IQC
                                PoLog oldPoLog = (PoLog)_Invenfacade.GetPoLog(po.PONO, po.POLine.ToString(), po.SerialNO, "103");
                                if (oldPoLog != null)
                                {
                                    po.SAPMaterialInvoice = oldPoLog.SAPMaterialInvoice;
                                }
                                po.Operator = asndetailObj.CUser;// asndetailObj.;
                                po.VendorInvoice = asnObj.InvNo;
                                po.StorageCode = asnObj.StorageCode;
                                po.Remark = asndetailObj.Remark1;
                                po.InvoiceDate = asndetailObj.MaintainDate;
                                list.Add(po);
                                #endregion

                                #region poLog
                                PoLog poLog = new PoLog();
                                poLog.PONO = po.PONO;
                                poLog.PoLine = po.POLine.ToString();
                                poLog.FacCode = po.FacCode; // asndetail.;
                                poLog.SerialNO = po.SerialNO;// asndetail.s;
                                poLog.MCode = po.MCode; //txtMCode.Text;
                                poLog.Qty = po.Qty;//detailSN. //Convert.ToDecimal(txtQty.Text);
                                poLog.Unit = po.Unit;
                                poLog.Status = po.Status;// ����

                                poLog.Operator = po.Operator;// asndetail.;
                                poLog.VendorInvoice = po.VendorInvoice;
                                poLog.StorageCode = po.StorageCode;
                                poLog.Remark = po.Remark;
                                poLog.InvoiceDate = po.InvoiceDate;
                                if (count > 0)//P�ش�
                                {
                                    poLog.IsPBack = "";
                                    poLog.SapReturn = "";
                                    poLog.SapTimeStamp = int.Parse(datetime);
                                    _Invenfacade.AddPoLog(poLog);
                                }
                                else
                                {
                                    SAPRfcReturn msg = poToSAP.POReceiveToSAP(list);
                                    poLog.IsPBack = "Actual";
                                    poLog.SapReturn = msg.Result;//(S��ʾ�ɹ���E��ʾʧ��)
                                    poLog.SapTimeStamp = int.Parse(datetime);
                                    _Invenfacade.AddPoLog(poLog);
                                }
                                #endregion
                            }
                            #endregion

                            #endregion
                        }
                    }
                }
                else if (asnIqc.IqcType == IQCType.IQCType_FullCheck) //ȫ��
                {
                    #region  ȫ��
                    ASN asnObj = (ASN)facade.GetAsn(stno);
                    Invoices invoices = (Invoices)_Invenfacade.GetInvoices(asnObj.InvNo);
                    object[] invoicesDetaillist = _Invenfacade.GetInvoicesDetailByInvNo(invoices.InvNo, asnIqc.DQMCode);
                    #region check
                    object[] allObjList = _Invenfacade.QueryASNDetailBySTNoCheckStatus(stno, "Reject");
                    if (allObjList == null)
                    {
                        return;
                    }
                    Asndetail asndetailObj = allObjList[0] as Asndetail;
                    if (asndetailObj == null)
                    {
                        return;
                    }
                    #endregion
                    Domain.MOModel.Material material = (Domain.MOModel.Material)_Invenfacade.GetMaterialByDQMCode(asndetailObj.DqmCode);
                    #region POList
                    List<PO> list = new List<PO>();
                    foreach (InvoicesDetail detail in invoicesDetaillist)
                    {
                        #region  PO�ش�
                        PO po = new PO();
                        po.PONO = detail.InvNo;
                        po.POLine = detail.InvLine;
                        po.FacCode = asnObj.FacCode;
                        po.SerialNO = asnObj.StNo;// asndetailObj.s;
                        po.MCode = asndetailObj.DqmCode; //txtMCode.Text;
                        po.Qty = ngqty;//detailSN. 
                        if (material != null)
                        {
                            po.Unit = material.Muom;//asndetailObj.Unit;
                        }
                        po.Status = "104";//IQC
                        PoLog oldPoLog = (PoLog)_Invenfacade.GetPoLog(po.PONO, po.POLine.ToString(), po.SerialNO, "103");
                        if (oldPoLog != null)
                        {
                            po.SAPMaterialInvoice = oldPoLog.SAPMaterialInvoice;
                        }
                        po.Operator = asndetailObj.CUser;// asndetailObj.;
                        po.VendorInvoice = asnObj.InvNo;
                        po.StorageCode = asnObj.StorageCode;
                        po.Remark = asndetailObj.Remark1;
                        po.InvoiceDate = asndetailObj.MaintainDate;
                        list.Add(po);
                        #endregion

                        #region poLog
                        PoLog poLog = new PoLog();
                        poLog.PONO = po.PONO;
                        poLog.PoLine = po.POLine.ToString();
                        poLog.FacCode = po.FacCode; // asndetail.;
                        poLog.SerialNO = po.SerialNO;// asndetail.s;
                        poLog.MCode = po.MCode; //txtMCode.Text;
                        poLog.Qty = po.Qty;//detailSN. //Convert.ToDecimal(txtQty.Text);
                        poLog.Unit = po.Unit;
                        poLog.Status = po.Status;// ����

                        poLog.Operator = po.Operator;// asndetail.;
                        poLog.VendorInvoice = po.VendorInvoice;
                        poLog.StorageCode = po.StorageCode;
                        poLog.Remark = po.Remark;
                        poLog.InvoiceDate = po.InvoiceDate;
                        if (count > 0)//P�ش�
                        {
                            poLog.IsPBack = "";
                            poLog.SapReturn = "";
                            poLog.SapTimeStamp = int.Parse(datetime);
                            _Invenfacade.AddPoLog(poLog);
                        }
                        else
                        {
                            SAPRfcReturn msg = poToSAP.POReceiveToSAP(list);
                            poLog.IsPBack = "Actual";
                            poLog.SapReturn = msg.Result;//(S��ʾ�ɹ���E��ʾʧ��)
                            poLog.SapTimeStamp = int.Parse(datetime);
                            _Invenfacade.AddPoLog(poLog);
                        }
                        #endregion
                    }
                    #endregion
                    #endregion
                }
            }
            #endregion
        }
        #endregion


        #region �˻�
        //�˻ض��μ���
        protected void cmdReturned2Inspection_ServerClick(object sender, EventArgs e)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }

            ArrayList array = this.gridHelper.GetCheckedRows();
            if (array.Count > 0)
            {
                StringBuilder sbShowMsg = new StringBuilder();
                foreach (GridRecord row in array)
                {
                    string iqcNo = row.Items.FindItemByKey("IQCNo").Value.ToString();
                    string status = row.Items.FindItemByKey("Status").Value.ToString();
                    if (status != "SQE�ж�")
                    {
                        sbShowMsg.AppendFormat("IQC���鵥��: {0} ״̬����SQE�ж��������˻� ", iqcNo);
                        continue;
                    }
                    try
                    {
                        string iqcType = row.Items.FindItemByKey("IQCType").Value.ToString();
                        if (iqcType == "ȫ��")
                        {
                            sbShowMsg.AppendFormat(iqcNo + "���鷽ʽΪ[ȫ��]����������[�˻ض��μ���]");
                            //WebInfoPublish.Publish(this, iqcNo+"���鷽ʽΪ[ȫ��]����������[�˻ض��μ���]", this.languageComponent1);
                            continue;
                        }
                        this.DataProvider.BeginTransaction();

                        //�˻ض��μ���
                        ToReturned2Inspection(iqcNo);

                        this.DataProvider.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        sbShowMsg.AppendFormat("IQC���鵥��: {0} {1}���˻ض��μ���ʧ�� ", iqcNo, ex.Message);
                        this.DataProvider.RollbackTransaction();
                    }
                }
                if (sbShowMsg.Length > 0)
                {
                    string showMsg = sbShowMsg.ToString();
                    WebInfoPublish.Publish(this, showMsg, this.languageComponent1);
                }
                else
                {
                    WebInfoPublish.Publish(this, "�˻ض��μ���ɹ�", this.languageComponent1);
                }
                this.gridHelper.RequestData();//ˢ��ҳ��
            }
        }

        //�˻�IQC�ؼ�
        protected void cmdReturnedQC_ServerClick(object sender, EventArgs e)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }
            ArrayList array = this.gridHelper.GetCheckedRows();
            if (array.Count > 0)
            {
                StringBuilder sbShowMsg = new StringBuilder();
                foreach (GridRecord row in array)
                {

                    string iqcNo = row.Items.FindItemByKey("IQCNo").Value.ToString();
                    string status = row.Items.FindItemByKey("Status").Value.ToString();
                    if (status != "SQE�ж�")
                    {
                        sbShowMsg.AppendFormat("IQC���鵥��: {0} ״̬����SQE�ж��������˻� ", iqcNo);
                        continue;
                    }
                    try
                    {
                        string iqcType = row.Items.FindItemByKey("IQCType").Value.ToString();
                        if (iqcType == "ȫ��")
                        {
                            sbShowMsg.AppendFormat(iqcNo + "���鷽ʽΪ[ȫ��]����������[�˻�IQC�ؼ�]");
                            //WebInfoPublish.Publish(this, iqcNo+"���鷽ʽΪ[ȫ��]����������[�˻�IQC�ؼ�]", this.languageComponent1);
                            continue;
                        }
                        this.DataProvider.BeginTransaction();

                        AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
                        if (asnIqc != null)
                        {
                            asnIqc.Status = IQCStatus.IQCStatus_Release;
                            _IQCFacade.UpdateAsnIQC(asnIqc);
                        }

                        this.DataProvider.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        sbShowMsg.AppendFormat("IQC���鵥��: {0} {1}���˻�IQC�ؼ�ʧ�� ", iqcNo, ex.Message);
                        this.DataProvider.RollbackTransaction();
                    }
                }

                if (sbShowMsg.Length > 0)
                {
                    string showMsg = sbShowMsg.ToString();
                    WebInfoPublish.Publish(this, showMsg, this.languageComponent1);
                }
                else
                {
                    WebInfoPublish.Publish(this, "�˻�QC�ؼ�ɹ�", this.languageComponent1);
                }
                this.gridHelper.RequestData();//ˢ��ҳ��
            }
        }
        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((AsnIQCExt)obj).IqcNo,
                                ((AsnIQCExt)obj).StNo,
                                this.GetInvInName(((AsnIQCExt)obj).StType),
                                ((AsnIQCExt)obj).InvNo,
                                ((AsnIQCExt)obj).DQMCode,
                                ((AsnIQCExt)obj).VendorMCode,
                                //((AsnIQCExt)obj).VendorCode,
                                 this.GetStatusName(((AsnIQCExt)obj).Status),
                                FormatHelper.GetChName(((AsnIQCExt)obj).IqcType),
                                FormatHelper.GetChName(((AsnIQCExt)obj).QcStatus),
                                ((AsnIQCExt)obj).AppQty.ToString(),
                                ((AsnIQCExt)obj).NgQty.ToString(),
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
                                    "DQMCode",
                                    "VMCode",	
                                    "Status",
                                    "IQCType",	
                                    "AQLResult",
                                    "AppQty",
                                    "NGQty",
                                    "VendorNo",
                                    "AppDate"};
        }

        #endregion

        #region Method

        //����
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="iqcNo">�ͼ쵥��</param>
        private bool ToRejectbt(string iqcNo)
        {
            try
            {
                if (_IQCFacade == null)
                {
                    _IQCFacade = new IQCFacade(base.DataProvider);
                }
                if (facade == null)
                {
                    facade = new WarehouseFacade(base.DataProvider);
                }
                _InventoryFacade = new InventoryFacade(base.DataProvider);
                //1�������ͼ쵥 TBLASNIQC
                //��������TBLASNIQC��״̬��IQCRejection:IQC����
                AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);

                base.DataProvider.BeginTransaction();
                if (asnIqc != null)
                {
                    asnIqc.Status = IQCStatus.IQCStatus_IQCRejection;
                    _IQCFacade.UpdateAsnIQC(asnIqc);

                    #region ��invinouttrans��������һ������
                    //WarehouseFacade facade = new WarehouseFacade(this.DataProvider);

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
                    trans.MaintainUser = "PDA";
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
                //2������ASN��ϸ�� TBLASNDETAIL
                //��������TBLASNIQC��״̬��IQCRejection:IQC����
                ASN asn = null;
                List<ASNDetail> asnDetailList = new List<ASNDetail>();
                List<AsnIQCDetail> iqcDetails = new List<AsnIQCDetail>();
                object[] objAsnIqcDetail = _IQCFacade.GetAsnIQCDetailByIqcNo(iqcNo);


                Hashtable ht = new Hashtable();
                if (objAsnIqcDetail != null)
                {
                    asn = _InventoryFacade.GetASN((objAsnIqcDetail[0] as AsnIQCDetail).StNo) as ASN;
                    foreach (AsnIQCDetail asnIqcDetail in objAsnIqcDetail)
                    {

                        ASNDetail asnDetail = (ASNDetail)_InventoryFacade.GetASNDetail(Convert.ToInt32(asnIqcDetail.StLine), asnIqcDetail.StNo);
                        if (asnDetail != null)
                        {
                            asnDetailList.Add(asnDetail);
                            if (!ht.ContainsKey(asnIqcDetail.StNo))
                            {
                                ht.Add(asnIqcDetail.StNo, asnIqcDetail.StNo);
                            }
                            asnDetail.Status = ASNDetail_STATUS.ASNDetail_IQCRejection;
                            _InventoryFacade.UpdateASNDetail(asnDetail);

                            object[] objs_item = facade.GetASNDetailItembyStnoAndStline(asnDetail.StNo, asnDetail.StLine);
                            if (objs_item != null)
                            {
                                foreach (Asndetailitem item in objs_item)
                                {
                                    item.QcpassQty = 0;
                                    item.ActQty = 0;
                                    facade.UpdateAsndetailitem(item);
                                }
                            }
                        }
                        iqcDetails.Add(asnIqcDetail);
                    }
                }

                //���� asn״̬��������е�IQC״̬���Ǿ��գ����״̬Ϊ���գ� ΪIQCRejection:IQC����
                foreach (DictionaryEntry d in ht)
                {
                    bool FFlag = true;
                    object[] objs_asnds = _InventoryFacade.GetASNDetailByStNo(d.Key.ToString());
                    if (objs_asnds != null)
                    {
                        foreach (ASNDetail asnds in objs_asnds)
                        {
                            if (asnds.InitReceiveStatus != "Reject")
                            {
                                if (asnds.Status != ASNDetail_STATUS.ASNDetail_IQCRejection)
                                {
                                    FFlag = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (FFlag)
                    {
                        object obj_asn = _InventoryFacade.GetASN(d.Key.ToString());
                        if (obj_asn != null)
                        {
                            asn = obj_asn as ASN;
                            asn.Status = ASN_STATUS.ASN_IQCRejection;
                            _InventoryFacade.UpdateASN(asn);
                        }
                    }
                }
                //TODO: �߼�δ�ṩ
                //3��������ͣ�TBLASNIQC.STTYPEΪ��POR:PO���ʱSAP���ʴ���.....
                if (_IQCFacade.CanToOnlocationStaus(asnIqc.StNo))
                {
                    if (!_IQCFacade.BeIQCReject(asnIqc.StNo))
                    {
                        asn.Status = ASNHeadStatus.OnLocation;

                    }
                    else
                    {

                        asn.Status = ASNHeadStatus.IQCRejection;
                    }
                    _InventoryFacade.UpdateASN(asn);
                }




                if (asn.StType == SAP_ImportType.SAP_POR)
                {
                    bool result = PoToSap(asnDetailList.ToArray());//asnDetailList.Distinct<string>().ToArray());
                    if (!result)
                    {
                        this.DataProvider.RollbackTransaction();
                        return false;
                    }


                }

                WarehouseFacade ware = new WarehouseFacade(base.DataProvider);
                SendMail mail = ShareLib.ShareKit.IQCRejectThenGenerMail(asnIqc, asn, GetUserCode(), ware, _IQCFacade);
                if (mail != null)
                    ware.AddSendMail(mail);
                this.DataProvider.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                throw ex;
            }
        }
        /// <summary>
        /// �ϴ�SAP
        /// </summary>
        /// <param name="asnDetailList"></param>
        /// <returns></returns>
        private bool PoToSap(object[] asnDetailList)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            WarehouseFacade _WarehouseFacade = new WarehouseFacade(base.DataProvider);

            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

            #region add by sam
            bool is2Sap = _WarehouseFacade.GetRecordCount(dbTime.DBDate, dbTime.DBTime) > 0;
            #endregion

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
                int count = _WarehouseFacade.GetRecordCount(dbTime.DBDate, dbTime.DBTime);
                BenQGuru.eMES.SAPRFCService.POToSAP poToSAP = new POToSAP(this.DataProvider);
                List<PO> list = new List<PO>();
                List<PoLog> logList = new List<PoLog>();
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
                    //decimal actQTY = (obj_item as Asndetailitem).ActQty;
                    decimal receiveQty = (obj_item as Asndetailitem).ReceiveQty;
                    decimal qcPassQty = (obj_item as Asndetailitem).QcpassQty;
                    int ngqty = (int)receiveQty - (int)qcPassQty;
                    if (ngqty > 0)
                    {

                        PoLog poLog = new PoLog();
                        int serial = _InventoryFacade.GetMaxSerialInPoLog() + 1;
                        poLog.Serial = serial;
                        poLog.PONO = item.Invno;
                        poLog.PoLine = item.Invline.ToString();
                        poLog.FacCode = asn.FacCode;
                        poLog.SerialNO = asn.StNo; // asndetail.s;
                        poLog.MCode = item.MCode;//SAPMcode
                        poLog.Qty = ngqty; // 
                        poLog.Unit = invdetail.Unit; //asndetailObj.Unit;
                        poLog.Status = "104"; // 
                        poLog.Operator = asn.CUser; // asndetail.;
                        poLog.VendorInvoice = asn.InvNo;
                        poLog.StorageCode = asn.StorageCode;
                        poLog.Remark = asn.Remark1;
                        poLog.InvoiceDate = asn.MaintainDate;
                        poLog.SapDateStamp = dbTime.DBDate;
                        poLog.SapTimeStamp = dbTime.DBTime;
                        poLog.ZNUMBER = serial.ToString();
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


                        PO po = new PO();
                        po.PONO = item.Invno;
                        po.POLine = int.Parse(item.Invline);
                        po.FacCode = asn.FacCode;
                        po.SerialNO = asn.StNo; // asndetail.s;
                        po.MCode = invdetail.MCode;//SAPMcode
                        po.Qty = ngqty; //���� ��������
                        po.Unit = invdetail.Unit; //asndetailObj.Unit;
                        po.Status = "104"; //����


                        string invoice103 = string.Empty;
                        PoLog oldPoLogs =
                            (PoLog)
                            _InventoryFacade.GetPoLog(po.PONO, po.POLine.ToString(), po.SerialNO, "103");
                        if (oldPoLogs != null)
                            invoice103 = oldPoLogs.SAPMaterialInvoice;
                        else
                            invoice103 = _InventoryFacade.GetPo103Invoices(po.PONO, po.POLine.ToString(), po.SerialNO);

                        po.SAPMaterialInvoice = oldPoLogs.SAPMaterialInvoice;

                        po.Operator = asn.CUser;
                        po.VendorInvoice = asn.InvNo;
                        po.StorageCode = asn.StorageCode;
                        po.Remark = asn.Remark1;
                        po.InvoiceDate = asn.MaintainDate;
                        po.ZNUMBER = poLog.ZNUMBER;
                        list.Add(po);

                    }
                }
                if (list.Count > 0)
                {
                    if (is2Sap)
                    {
                        LogPO2Sap(list);
                    }
                    else
                    {
                        #region SAP��д

                        SAPRfcReturn msg = poToSAP.POReceiveToSAP(list);

                        #region ������˷���false

                        if (msg.Result.Trim().ToUpper() == "E")
                        {
                            isSuccess = false;
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "SAP��д����" + msg.Message, this.languageComponent1);
                        }

                        #endregion

                        #region дlog

                        foreach (PoLog poLog in logList)
                        {

                            if (count > 0) //P�ش�
                            {
                                poLog.SAPMaterialInvoice = ""; //����ʱ�ţ�P��Ϊ��
                                poLog.SapReturn = "";
                            }
                            else
                            {
                                poLog.SAPMaterialInvoice = Getstring(msg.MaterialDocument); //
                                poLog.SapReturn = Getstring(msg.Result); // msg.Result;//(S��ʾ�ɹ���E��ʾʧ��)
                                poLog.Message = Getstring(msg.Message);
                            }
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

                }
            }
            return true;

        }


        private void LogPO2Sap(List<BenQGuru.eMES.SAPRFCService.Domain.PO> dns)
        {
            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            _InventoryFacade = new InventoryFacade(this.DataProvider);
            foreach (BenQGuru.eMES.SAPRFCService.Domain.PO po in dns)
            {

                Po2Sap poLog = new Po2Sap();
                poLog.PONO = po.PONO;
                poLog.POLine = po.POLine;
                poLog.SerialNO = po.SerialNO;
                poLog.Qty = po.Qty; // 
                poLog.Unit = po.Unit;
                poLog.FacCode = po.FacCode;
                poLog.STNO = po.SerialNO;
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

        //�˻ض��μ���
        /// <summary>
        /// �˻ض��μ���
        /// </summary>
        /// <param name="iqcNo">�ͼ쵥��</param>
        private void ToReturned2Inspection(string iqcNo)
        {
            if (_IQCFacade == null)
            {
                _IQCFacade = new IQCFacade(base.DataProvider);
            }


            AsnIQC asnIqc = (AsnIQC)_IQCFacade.GetAsnIQC(iqcNo);
            if (asnIqc != null)
            {
                //1�������ͼ쵥�� TBLASNIQC
                asnIqc.Status = IQCStatus.IQCStatus_Cancel;  //ԭIQC��ɹص���״̬
                _IQCFacade.UpdateAsnIQC(asnIqc);

                //2������һ����IQC���鵥��,������Դ��ԭIQC���鵥��,�漰���ͼ쵥(TBLASNIQC)���ͼ쵥��ϸ(TBLASNIQCDETAIL)���ͼ쵥��ϸSN(TBLASNIQCDETAILSN)
                string newIqcNo = this.CreateNewIqcNo(iqcNo);

                #region 1)�ͼ쵥TBLASNIQC
                //1)�ͼ쵥TBLASNIQC
                AsnIQC newAsnIqc = _IQCFacade.CreateNewAsnIQC();
                newAsnIqc.IqcNo = newIqcNo;
                newAsnIqc.IqcType = IQCType.IQCType_FullCheck;
                newAsnIqc.StNo = asnIqc.StNo;
                newAsnIqc.InvNo = asnIqc.InvNo;
                newAsnIqc.StType = asnIqc.StType;
                newAsnIqc.Status = IQCStatus.IQCStatus_Release;
                newAsnIqc.AppDate = FormatHelper.GetNowDBDateTime(base.DataProvider).DBDate;
                newAsnIqc.AppTime = FormatHelper.GetNowDBDateTime(base.DataProvider).DBTime;
                newAsnIqc.InspDate = asnIqc.InspDate;
                newAsnIqc.InspTime = asnIqc.InspTime;
                newAsnIqc.CustmCode = asnIqc.CustmCode;
                newAsnIqc.MCode = asnIqc.MCode;
                newAsnIqc.DQMCode = asnIqc.DQMCode;
                newAsnIqc.MDesc = asnIqc.MDesc;
                newAsnIqc.Qty = asnIqc.Qty;
                newAsnIqc.QcStatus = "";
                newAsnIqc.VendorCode = asnIqc.VendorCode;
                newAsnIqc.VendorMCode = asnIqc.VendorMCode;
                newAsnIqc.Remark1 = "";
                newAsnIqc.CUser = asnIqc.CUser;
                newAsnIqc.CDate = asnIqc.CDate;
                newAsnIqc.CTime = asnIqc.CTime;
                newAsnIqc.MaintainUser = this.GetUserCode();
                _IQCFacade.AddAsnIQC(newAsnIqc);
                #endregion

                #region 2)�ͼ쵥��ϸTBLASNIQCDETAIL
                //2)�ͼ쵥��ϸTBLASNIQCDETAIL
                object[] objAsnIQCDetail = _IQCFacade.GetAsnIQCDetailByIqcNo(iqcNo);
                if (objAsnIQCDetail != null && objAsnIQCDetail.Length > 0)
                {
                    foreach (AsnIQCDetail oldAsnIQCDetail in objAsnIQCDetail)
                    {
                        AsnIQCDetail newAsnIqcDetail = _IQCFacade.CreateNewAsnIQCDetail();
                        newAsnIqcDetail.IqcNo = newIqcNo;
                        newAsnIqcDetail.StNo = oldAsnIQCDetail.StNo;
                        newAsnIqcDetail.StLine = oldAsnIQCDetail.StLine;
                        newAsnIqcDetail.CartonNo = oldAsnIQCDetail.CartonNo;
                        newAsnIqcDetail.Qty = oldAsnIQCDetail.Qty;
                        newAsnIqcDetail.QcPassQty = 0;
                        newAsnIqcDetail.Unit = oldAsnIQCDetail.Unit;
                        newAsnIqcDetail.NgQty = 0;
                        newAsnIqcDetail.ReturnQty = 0;
                        newAsnIqcDetail.ReformQty = 0;
                        newAsnIqcDetail.GiveQty = 0;
                        newAsnIqcDetail.AcceptQty = 0;
                        newAsnIqcDetail.QcStatus = "";
                        newAsnIqcDetail.Remark1 = "";
                        newAsnIqcDetail.CUser = oldAsnIQCDetail.CUser;
                        newAsnIqcDetail.CDate = oldAsnIQCDetail.CDate;
                        newAsnIqcDetail.CTime = oldAsnIQCDetail.CTime;
                        newAsnIqcDetail.MaintainUser = this.GetUserCode();
                        _IQCFacade.AddAsnIQCDetail(newAsnIqcDetail);
                    }
                }


                #endregion

                #region 3)�ͼ쵥��ϸSN TBLASNIQCDETAILSN
                object[] objAsnIqcDetailSN = _IQCFacade.GetAsnIqcDetailSNByIqcNo(iqcNo);
                if (objAsnIqcDetailSN != null && objAsnIqcDetailSN.Length > 0)
                {
                    foreach (AsnIqcDetailSN oldAsnIqcDetailSN in objAsnIqcDetailSN)
                    {
                        AsnIqcDetailSN newAsnIqcDetailSN = _IQCFacade.CreateNewAsnIqcDetailSN();
                        newAsnIqcDetailSN.IqcNo = newIqcNo;
                        newAsnIqcDetailSN.StNo = oldAsnIqcDetailSN.StNo;
                        newAsnIqcDetailSN.StLine = oldAsnIqcDetailSN.StLine;
                        newAsnIqcDetailSN.CartonNo = oldAsnIqcDetailSN.CartonNo;
                        newAsnIqcDetailSN.Sn = oldAsnIqcDetailSN.Sn;
                        newAsnIqcDetailSN.QcStatus = "";
                        newAsnIqcDetailSN.Remark1 = "";
                        newAsnIqcDetailSN.CUser = oldAsnIqcDetailSN.CUser;
                        newAsnIqcDetailSN.CDate = oldAsnIqcDetailSN.CDate;
                        newAsnIqcDetailSN.CTime = oldAsnIqcDetailSN.CTime;
                        newAsnIqcDetailSN.MaintainUser = this.GetUserCode();
                        _IQCFacade.AddAsnIqcDetailSN(newAsnIqcDetailSN);
                    }
                }

                #endregion
            }
        }

        //�����µ�IQC���鵥��
        /// <summary>
        /// �����µ�IQC���鵥��
        /// </summary>
        /// <param name="oldIqcNo">ԭIQC���鵥��</param>
        /// <returns></returns>
        private string CreateNewIqcNo(string oldIqcNo)
        {
            //����ԭIQC���鵥��+_+��λ��ˮ�ţ��磺IQCASN00000101_01
            WarehouseFacade warehouseFacade = new WarehouseFacade(this.DataProvider);
            string newIqcNo = string.Empty;
            string SNPrefix = oldIqcNo + "_";
            object objSerialBook = warehouseFacade.GetSerialBook(SNPrefix);
            if (objSerialBook == null)
            {
                DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                SERIALBOOK serialBook = new SERIALBOOK();
                serialBook.SNPrefix = SNPrefix;
                serialBook.MaxSerial = "1";
                serialBook.MUser = this.GetUserCode();
                serialBook.MDate = dbDateTime.DBDate;
                serialBook.MTime = dbDateTime.DBTime;
                warehouseFacade.AddSerialBook(serialBook);

                return SNPrefix + "01";
            }
            else
            {
                SERIALBOOK serialBook = (SERIALBOOK)objSerialBook;
                if (serialBook.MaxSerial == "99")
                {
                    return "";
                }
                serialBook.MaxSerial = (Convert.ToInt32(serialBook.MaxSerial) + 1).ToString();
                warehouseFacade.UpdateSerialBook(serialBook);

                return serialBook.SNPrefix + serialBook.MaxSerial.PadLeft(2, '0');

            }
        }
        #endregion

    }
}
