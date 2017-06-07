using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using BenQGuru.eMES.Domain.IQC;
using BenQGuru.eMES.IQC;
using BenQGuru.eMES.MOModel;
using System.Text;
using BenQGuru.eMES.Domain.OQC;
using BenQGuru.eMES.OQC;


namespace BenQGuru.eMES.Web.OQC
{
    public partial class FSQEJudgeMP : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private IQCFacade _IQCFacade = null;
        private OQCFacade _OQCFacade = null;
        private SystemSettingFacade _SystemSettingFacade = null;
        private ItemFacade _ItemFacade = null;
        private InventoryFacade _InventoryFacade = null;

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
                this.InitOQCStatusList();

            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        //��ʼOQC���鵥״̬������
        /// <summary>
        /// ��ʼOQC���鵥״̬������
        /// </summary>
        private void InitOQCStatusList()
        {
            if (_SystemSettingFacade == null)
            {
                _SystemSettingFacade = new SystemSettingFacade(base.DataProvider);
            }
            object[] parameters = _SystemSettingFacade.GetParametersByParameterGroup("OQCSTATUS");
            this.drpOQCStatusQuery.Items.Add(new ListItem("", ""));
            if (parameters != null)
            {
                foreach (Domain.BaseSetting.Parameter parameter in parameters)
                {
                    this.drpOQCStatusQuery.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));
                }
            }

            this.drpOQCStatusQuery.SelectedIndex = 3;
        }



        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("OQCNo", "OQC���鵥��", null);
            this.gridHelper.AddColumn("PickNo", "����������", null);
            this.gridHelper.AddColumn("StorageOutType", "��������", null);
            this.gridHelper.AddDataColumn("PickType", "�������ʹ���", true);
            this.gridHelper.AddColumn("SAPInvNo", "SAP���ݺ�", null);
            this.gridHelper.AddColumn("STORAGECODE123", "�����λ", null);
            //this.gridHelper.AddColumn("DQMCode", "�������ϴ���", null);
            //this.gridHelper.AddColumn("HWMCode", "��Ϊ���ϱ���", null);
            this.gridHelper.AddColumn("Status", "״̬", null);
            this.gridHelper.AddDataColumn("OQCStatus", "OQC����״̬", true);
            this.gridHelper.AddColumn("OQCType", "���鷽ʽ", null);
            this.gridHelper.AddColumn("AQLResult", "������", null);
            this.gridHelper.AddColumn("AppQty", "�ͼ�����", null);
            this.gridHelper.AddColumn("NGQty", "ȱ��Ʒ����", null);
            this.gridHelper.AddColumn("ReturnQty", "�˻�������", null);
            this.gridHelper.AddColumn("AppDate", "�ͼ�����", null);
            this.gridHelper.AddColumn("AppTime", "�ͼ�ʱ��", null);
            this.gridHelper.AddColumn("AppUser", "�ͼ���", null);
            this.gridHelper.AddEditColumn("btnInspect", "����");

            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.RequestData();//ҳ���ʼ������grid����
        }

        protected override DataRow GetGridRow(object obj)
        {

            DataRow row = this.DtSource.NewRow();
            _InventoryFacade = new InventoryFacade(base.DataProvider);
            row["OQCNo"] = ((OQCExt1)obj).OqcNo;
            row["PickNo"] = ((OQCExt1)obj).PickNo;
            row["StorageOutType"] = this.GetPickTypeName(((OQCExt1)obj).PickType);//�������ͣ��������ͣ�
            row["SAPInvNo"] = ((OQCExt1)obj).InvNo;

            Pick p = (Pick)_InventoryFacade.GetPick(((OQCExt1)obj).PickNo);
            if (p != null)
                row["STORAGECode123"] = p.StorageCode;
            else
                row["STORAGECode123"] = string.Empty;

            //row["DQMCode"] = ((OQCExt1)obj).DQMCode;
            //row["HWMCode"] = ((OQCExt1)obj).HWMCode;
            row["Status"] = this.GetStatusName(((OQCExt1)obj).Status);
            row["OQCStatus"] = ((OQCExt1)obj).Status;
            row["OQCType"] = FormatHelper.GetChName(((OQCExt1)obj).OqcType);
            row["AQLResult"] = FormatHelper.GetChName(((OQCExt1)obj).QcStatus);
            row["AppQty"] = ((OQCExt1)obj).AppQty;
            row["NGQty"] = ((OQCExt1)obj).NgQty;
            row["ReturnQty"] = ((OQCExt1)obj).ReturnQty;
            row["AppDate"] = FormatHelper.ToDateString(((OQCExt1)obj).AppDate, "/");
            row["AppTime"] = FormatHelper.ToTimeString(((OQCExt1)obj).AppTime, ":");
            row["AppUser"] = ((OQCExt1)obj).CUser;

            return row;

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_OQCFacade == null)
            {
                _OQCFacade = new OQCFacade(base.DataProvider);
            }
            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            UserFacade userFacade = new UserFacade(this.DataProvider);
            string[] usergroupList = userFacade.GetUserGroupCodeofUser1(GetUserCode());
            return this._OQCFacade.QueryOQC2(usergroupList,
                                            FormatHelper.CleanString(this.txtCarInvNoQuery.Text),
                                            FormatHelper.CleanString(this.txtPickNo.Text),
                                            FormatHelper.CleanString(this.txtOQCNoQuery.Text),
                                            FormatHelper.CleanString(this.drpOQCStatusQuery.SelectedValue),
                                              FormatHelper.TODateInt(this.txtAppBDateQuery.Text),
                                            FormatHelper.TODateInt(this.txtAppEDateQuery.Text),

                                               FormatHelper.CleanString(this.txtCartonNoQurey.Text),
                                                  FormatHelper.CleanString(this.txtSNQuery.Text),
                                                  string.Empty,
                                                  string.Empty,
                                                  string.Empty,
                                                  string.Empty, string.Empty,

                                            inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_OQCFacade == null)
            {
                _OQCFacade = new OQCFacade(base.DataProvider);
            }
            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            UserFacade userFacade = new UserFacade(this.DataProvider);
            string[] usergroupList = userFacade.GetUserGroupCodeofUser1(GetUserCode());
            return this._OQCFacade.QueryOQC2Count(
                                               usergroupList,
                                            FormatHelper.CleanString(this.txtCarInvNoQuery.Text),
                                            FormatHelper.CleanString(this.txtPickNo.Text),
                                            FormatHelper.CleanString(this.txtOQCNoQuery.Text),
                                            FormatHelper.CleanString(this.drpOQCStatusQuery.SelectedValue),
                                              FormatHelper.TODateInt(this.txtAppBDateQuery.Text),
                                            FormatHelper.TODateInt(this.txtAppEDateQuery.Text),

                                               FormatHelper.CleanString(this.txtCartonNoQurey.Text),
                                                  FormatHelper.CleanString(this.txtSNQuery.Text),
                                                  string.Empty,
                                                  string.Empty,
                                                  string.Empty,
                                                  string.Empty, string.Empty
                                                );
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
                //edit by sam ����״̬ 2016��4��15��
                //string oqcStatus = row.Items.FindItemByKey("OQCStatus").Text.Trim();
                //if (oqcStatus == OQCStatus.OQCStatus_SQEJudge)
                //{
                string mControlType = string.Empty;//�ܿ�����
                //string dQMCode = row.Items.FindItemByKey("DQMCode").Text.Trim();
                //object objMaterial =_ItemFacade.GetMaterialByDQMCode(dQMCode);
                //if (objMaterial != null)
                //{
                //    mControlType = ((Domain.MOModel.Material)objMaterial).MCONTROLTYPE ;
                //}
                string oqcNo = row.Items.FindItemByKey("OQCNo").Text.Trim();//OQC���ݺ�
                string pickType = row.Items.FindItemByKey("PickType").Text.Trim();//�������
                string aqlResult = row.Items.FindItemByKey("AQLResult").Text.Trim();//���
                Response.Redirect(this.MakeRedirectUrl("FSQEProcessMP.aspx",
                                                        new string[] { "OQCNo", "PickType", "MControlType", "AQLResult" },
                                                        new string[] { oqcNo, pickType, mControlType, aqlResult }));
                //}
                //else
                //{
                //    WebInfoPublish.Publish(this, "״̬����SQE�ж������ܴ���", this.languageComponent1);
                //}
            }
        }

        //�˻ض��μ���
        protected void cmdReturned2Inspection_ServerClick(object sender, EventArgs e)
        {
            ArrayList array = this.gridHelper.GetCheckedRows();
            if (array.Count > 0)
            {
                StringBuilder sbShowMsg = new StringBuilder();
                foreach (GridRecord row in array)
                {
                    string oqcNo = row.Items.FindItemByKey("OQCNo").Value.ToString();
                    string status = row.Items.FindItemByKey("Status").Value.ToString();
                    if (status != "SQE�ж�")
                    {
                        sbShowMsg.AppendFormat("OQC���鵥��: {0} ״̬����SQE�ж��������˻� ", oqcNo);
                        continue;
                    }
                    try
                    {
                        this.DataProvider.BeginTransaction();

                        //�˻ض��μ���
                        ToReturned2Inspection(oqcNo);

                        this.DataProvider.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        sbShowMsg.AppendFormat("OQC���鵥��: {0} {1}���˻ض��μ���ʧ�� ", oqcNo, ex.Message);
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

        //�˻�OQC�ؼ�
        protected void cmdReturnedOQC_ServerClick(object sender, EventArgs e)
        {
            if (_OQCFacade == null)
            {
                _OQCFacade = new OQCFacade(base.DataProvider);
            }
            ArrayList array = this.gridHelper.GetCheckedRows();
            if (array.Count > 0)
            {
                StringBuilder sbShowMsg = new StringBuilder();
                foreach (GridRecord row in array)
                {
                    string oqcNo = row.Items.FindItemByKey("OQCNo").Value.ToString();
                    string status = row.Items.FindItemByKey("Status").Value.ToString();
                    if (status != "SQE�ж�")
                    {
                        sbShowMsg.AppendFormat("OQC���鵥��: {0} ״̬����SQE�ж��������˻� ", oqcNo);
                        continue;
                    }
                    try
                    {
                        this.DataProvider.BeginTransaction();

                        Domain.OQC.OQC oqc = (Domain.OQC.OQC)_OQCFacade.GetOQC(oqcNo);
                        if (oqc != null)
                        {
                            oqc.Status = OQCStatus.OQCStatus_WaitCheck;
                            _OQCFacade.UpdateOQC(oqc);
                        }

                        this.DataProvider.CommitTransaction();
                    }
                    catch (Exception ex)
                    {

                        sbShowMsg.AppendFormat("OQC���鵥��: {0} {1}���˻�OQC�ؼ�ʧ�� ", oqcNo, ex.Message);
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
                    WebInfoPublish.Publish(this, "�˻�OQC�ؼ�ɹ�", this.languageComponent1);
                }
                this.gridHelper.RequestData();//ˢ��ҳ��
            }
        }
        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((OQCExt1)obj).OqcNo,
                                ((OQCExt1)obj).PickNo,
                                this.GetPickTypeName(((OQCExt1)obj).PickType),
                                ((OQCExt1)obj).InvNo,
                                //((OQCExt1)obj).DQMCode,
                                //((OQCExt1)obj).HWMCode,
                                this.GetStatusName(((OQCExt1)obj).Status),
                                FormatHelper.GetChName(((OQCExt1)obj).OqcType),
                                 FormatHelper.GetChName(((OQCExt1)obj).QcStatus),
                                ((OQCExt1)obj).AppQty.ToString(),
                                ((OQCExt1)obj).NgQty.ToString(),
                                FormatHelper.ToDateString(((OQCExt1)obj).AppDate, "/"),
                                FormatHelper.ToTimeString(((OQCExt1)obj).AppTime, ":"),
                                ((OQCExt1)obj).CUser
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"OQCNo",
                                    "PickNo",
                                    "StorageOutType",
                                    "SAPInvNo",
                                    //"DQMCode",
                                    //"HWMCode",	
                                    "Status",
                                    "OQCType",	
                                    "AQLResult",
                                    "AppQty",
                                    "NGQty",
                                    "AppDate",
                                    "AppTime",
                                    "AppUser"};
        }

        #endregion

        #region Method

        //�˻ض��μ���
        /// <summary>
        /// �˻ض��μ���
        /// </summary>
        /// <param name="iqcNo">�ͼ쵥��</param>
        private void ToReturned2Inspection(string oqcNo)
        {
            if (_OQCFacade == null)
            {
                _OQCFacade = new OQCFacade(base.DataProvider);
            }


            Domain.OQC.OQC oqc = (Domain.OQC.OQC)_OQCFacade.GetOQC(oqcNo);
            if (oqc != null)
            {
                //1�������ͼ쵥�� TBLOQC
                oqc.Status = OQCStatus.OQCStatus_Cancel;
                _OQCFacade.UpdateOQC(oqc);

                //2������һ����OQC���鵥��,������Դ��ԭ OQC���鵥��,�漰��OQC�ͼ쵥(TBLOQC)��OQC�ͼ쵥��ϸ(TBLOQCDETAIL)��OQC�ͼ쵥��ϸSN(TBLOQCDETAILSN)
                string newOqcNo = this.CreateNewIqcNo(oqcNo);

                #region 1)�ͼ쵥TBLOQC
                //1)�ͼ쵥TBLOQC
                Domain.OQC.OQC newOqc = _OQCFacade.CreateNewOqc();
                newOqc.OqcNo = newOqcNo;
                newOqc.OqcType = OQCType.OQCType_FullCheck;
                newOqc.PickNo = oqc.PickNo;
                newOqc.CarInvNo = oqc.CarInvNo;
                newOqc.Status = OQCStatus.OQCStatus_Release;
                newOqc.AppDate = FormatHelper.GetNowDBDateTime(base.DataProvider).DBDate;
                newOqc.AppTime = FormatHelper.GetNowDBDateTime(base.DataProvider).DBTime;
                newOqc.InspDate = oqc.InspDate;
                newOqc.InspTime = oqc.InspTime;
                newOqc.QcStatus = "";
                newOqc.CUser = oqc.CUser;
                newOqc.CDate = oqc.CDate;
                newOqc.CTime = oqc.CTime;
                newOqc.MaintainUser = this.GetUserCode();
                _OQCFacade.AddOQC(newOqc);
                #endregion

                #region 2)�ͼ쵥��ϸTBLOQCDETAIL
                //2)�ͼ쵥��ϸTBLOQCDETAIL
                object[] objOqcDetail = _OQCFacade.GetOQCDetailByOqcNo(oqcNo);
                if (objOqcDetail != null && objOqcDetail.Length > 0)
                {
                    foreach (OQCDetail oldOqcDetail in objOqcDetail)
                    {
                        OQCDetail newOqcDetail = _OQCFacade.CreateNewOQCDetail();
                        newOqcDetail.OqcNo = newOqcNo;
                        newOqcDetail.CarInvNo = oldOqcDetail.CarInvNo;
                        newOqcDetail.CartonNo = oldOqcDetail.CartonNo;
                        newOqcDetail.MCode = oldOqcDetail.MCode;
                        newOqcDetail.DQMCode = oldOqcDetail.DQMCode;
                        newOqcDetail.Qty = oldOqcDetail.Qty;
                        newOqcDetail.QcPassQty = 0;
                        newOqcDetail.Unit = oldOqcDetail.Unit;
                        newOqcDetail.NgQty = 0;
                        newOqcDetail.ReturnQty = 0;
                        newOqcDetail.GiveQty = 0;
                        newOqcDetail.QcStatus = "";
                        newOqcDetail.GfHwItemCode = oldOqcDetail.GfHwItemCode;
                        newOqcDetail.GfPackingSeq = oldOqcDetail.GfPackingSeq;
                        newOqcDetail.Remark1 = "";
                        newOqcDetail.CUser = oldOqcDetail.CUser;
                        newOqcDetail.CDate = oldOqcDetail.CDate;
                        newOqcDetail.CTime = oldOqcDetail.CTime;
                        newOqcDetail.MaintainUser = this.GetUserCode();
                        _OQCFacade.AddOQCDetail(newOqcDetail);
                    }
                }


                #endregion

                #region 3)�ͼ쵥��ϸSN TBLOQCDETAILSN
                object[] objOqcDetailSN = _OQCFacade.GetOQCDetailSNByOqcNo(oqcNo);
                if (objOqcDetailSN != null && objOqcDetailSN.Length > 0)
                {
                    foreach (OQCDetailSN oldOqcDetailSN in objOqcDetailSN)
                    {
                        OQCDetailSN newOqcDetailSN = _OQCFacade.CreateNewOQCDetailSN();
                        newOqcDetailSN.OqcNo = newOqcNo;
                        newOqcDetailSN.CarInvNo = oldOqcDetailSN.CarInvNo;
                        newOqcDetailSN.CartonNo = oldOqcDetailSN.CartonNo;
                        newOqcDetailSN.SN = oldOqcDetailSN.SN;
                        newOqcDetailSN.MCode = oldOqcDetailSN.MCode;
                        newOqcDetailSN.DQMCode = oldOqcDetailSN.DQMCode;
                        newOqcDetailSN.QcStatus = "";
                        newOqcDetailSN.MaintainUser = this.GetUserCode();
                        _OQCFacade.AddOQCDetailSN(newOqcDetailSN);
                    }
                }

                #endregion
            }
        }

        //�����µ�OQC���鵥��
        /// <summary>
        /// �����µ�IQC���鵥��
        /// </summary>
        /// <param name="oldIqcNo">ԭIQC���鵥��</param>
        /// <returns></returns>
        private string CreateNewIqcNo(string oldOqcNo)
        {
            //����ԭOQC���鵥�� +_+ ��λ��ˮ�ţ��磺OQCASN00000101_01
            WarehouseFacade warehouseFacade = new WarehouseFacade(this.DataProvider);
            string SNPrefix = oldOqcNo + "_";
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
