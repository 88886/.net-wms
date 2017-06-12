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
using BenQGuru.eMES.Domain.IQC;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.IQC;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FExecuteASNMP : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private WarehouseFacade _WarehouseFacade = null;
        private WarehouseFacade _TransferFacade;
        SystemSettingFacade _SystemSettingFacade = null;
        private IQCFacade _IQCFacade = null;
        private InventoryFacade _InventoryFacade = null;
        private InventoryFacade facade = null;
        private UserFacade _UserFacade = null;
        bool isVendor = false;//�жϵ�ǰ�û��Ƿ�Ϊ��Ӧ��

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
            if (_UserFacade == null)
            {
                _UserFacade = new UserFacade(this.DataProvider);
            }
            isVendor = _UserFacade.IsVendor(this.GetUserCode());
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                this.InitStorageInTypeList();
                InitStatusList();
                InitStorageList();

                string stno = Request.QueryString["StNo"];

                this.txtStorageInASNQuery.Text = stno;
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }


        //��ʼ��λ������
        /// <summary>
        /// ��ʼ����λ
        /// </summary>
        private void InitStorageList()
        {
            #region ע��
            //if (facade == null)
            //{
            //    facade = new InventoryFacade(base.DataProvider);
            //}
            //this.drpStackListQuery.Items.Add(new ListItem("", ""));
            //object[] objStorage = facade.GetAllStorage();
            //if (objStorage != null && objStorage.Length > 0)
            //{
            //    foreach (Storage storage in objStorage)
            //    {

            //        this.drpStackListQuery.Items.Add(new ListItem(
            //            //string.Format("{0}��{1}", storage.StorageCode, storage.StorageName),
            //                storage.StorageCode + "-" + storage.StorageName, storage.StorageCode)
            //            );
            //    }
            //}
            //this.drpStackListQuery.SelectedIndex = 0; 
            #endregion

            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            UserFacade userFacade = new UserFacade(this.DataProvider);
            string[] usergroupList = userFacade.GetUserGroupCodeofUser(GetUserCode());//+TOSTORAGE
            this.drpStackListQuery.Items.Add(new ListItem("", ""));
            object[] parameters = systemSettingFacade.GetDistinctParaInParameterGroup(usergroupList);
            if (parameters != null)
            {
                foreach (Domain.BaseSetting.Parameter parameter in parameters)
                {
                    drpStackListQuery.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterCode));
                }
            }
            this.drpStackListQuery.SelectedIndex = 0;
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
            drpStatusQuery.Items.Remove(new ListItem("��ʼ��", ASNHeadStatus.Release));
            this.drpStatusQuery.SelectedIndex = 0;
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
            if (isVendor)
            {
                this.drpStorageInTypeQuery.Items.Add(new ListItem("PO���", "POR"));
                this.drpStorageInTypeQuery.SelectedIndex = 0;
            }
            else
            {
                object[] parameters = _SystemSettingFacade.GetParametersByParameterGroup("INVINTYPE");

                this.drpStorageInTypeQuery.Items.Add(new ListItem("", ""));
                foreach (Domain.BaseSetting.Parameter parameter in parameters)
                {
                    if (parameter.ParameterAlias != "PGIR" && parameter.ParameterAlias != "SCTR")
                        this.drpStorageInTypeQuery.Items.Add(new ListItem(parameter.ParameterDescription, parameter.ParameterAlias));
                }
                this.drpStorageInTypeQuery.SelectedIndex = 0;
            }
        }

        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("ASN", "���ָ���", null);
            this.gridHelper.AddColumn("CDate", "��������", null);
            this.gridHelper.AddColumn("CUser", "������", null);
            this.gridHelper.AddColumn("StorageInType", "�������", null);
            this.gridHelper.AddColumn("SAPInvNo", "SAP���ݺ�", null);
            this.gridHelper.AddColumn("Status", "״̬", null);
            //this.gridHelper.AddLinkColumn("LinkToCartonImport", "����/�鿴�䵥", null);
            this.gridHelper.AddColumn("StorageInCode", "����λ", null);
            this.gridHelper.AddColumn("PredictDate", "Ԥ�Ƶ�������", null);
            this.gridHelper.AddColumn("DirectFlag", "��Ӧ��ֱ����ʶ", null);
            this.gridHelper.AddColumn("PickNo", "����������", null);
            this.gridHelper.AddColumn("VendorCode", "��Ӧ�̴���", null);
            this.gridHelper.AddColumn("ExigencyFlag", "������ʶ", null);
            this.gridHelper.AddColumn("RejectsFlag", "���������벻��Ʒ���ʶ", null);
            this.gridHelper.AddColumn("OANo", "OA��ˮ��", null);
            this.gridHelper.AddColumn("PackingListNo", "��Ӧ��װ���", null);
            this.gridHelper.AddColumn("ProvideDate", "�䵥��������", null);
            this.gridHelper.AddColumn("GrossWeight", "ë��", null);
            this.gridHelper.AddColumn("Volume", "���", null);
            this.gridHelper.AddColumn("FromStorageCode", "�����λ", null);


            this.gridHelper.AddColumn("Remark1", "��ע", null);
            this.gridHelper.AddLinkColumn("Detail", "����", null);
            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            if (!string.IsNullOrEmpty(this.txtStorageInASNQuery.Text))
            {
                this.gridHelper.RequestData();
            }

        }

        protected override DataRow GetGridRow(object obj)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            DataRow row = this.DtSource.NewRow();

            row["ASN"] = ((ASN)obj).StNo;
            row["CDate"] = FormatHelper.ToDateString(((ASN)obj).CDate);
            row["CUser"] = ((ASN)obj).CUser;
            row["StorageInType"] = this.GetInvInName(((ASN)obj).StType);
            row["SAPInvNo"] = ((ASN)obj).InvNo;
            row["Status"] = languageComponent1.GetString(((ASN)obj).Status);

            row["StorageInCode"] = ((ASN)obj).StorageCode;
            row["PredictDate"] = FormatHelper.ToDateString(((ASN)obj).PreictDate);
            row["DirectFlag"] = ((ASN)obj).DirectFlag;
            row["PickNo"] = ((ASN)obj).PickNo;
            row["VendorCode"] = ((ASN)obj).VendorCode;
            row["ExigencyFlag"] = ((ASN)obj).ExigencyFlag;
            row["RejectsFlag"] = ((ASN)obj).RejectsFlag;
            row["OANo"] = ((ASN)obj).OANo;
            row["PackingListNo"] = ((ASN)obj).PackingListNo;
            row["ProvideDate"] = ((ASN)obj).ProvideDate;// FormatHelper.ToDateString(((ASN)obj).ProvideDate);
            row["GrossWeight"] = ((ASN)obj).GrossWeight;
            row["Volume"] = ((ASN)obj).Volume;


            row["FromStorageCode"] = ((ASN)obj).FromStorageCode;
            row["Remark1"] = ((ASN)obj).Remark1;

            return row;

        }



        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }

            return this._WarehouseFacade.QueryExecuteASN(
           FormatHelper.CleanString(this.txtStorageInASNQuery.Text),
           this.drpStorageInTypeQuery.SelectedValue,
           FormatHelper.CleanString(this.txtInvNoQuery.Text),
           FormatHelper.CleanString(this.drpStackListQuery.SelectedValue),
           FormatHelper.CleanString(this.drpStatusQuery.SelectedValue),
           FormatHelper.TODateInt(this.txtCBDateQuery.Text),
           FormatHelper.TODateInt(this.txtCEDateQuery.Text),
           GetUserCode(),
           inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            return this._WarehouseFacade.QueryExecuteASNCount(
                  FormatHelper.CleanString(this.txtStorageInASNQuery.Text),
                  this.drpStorageInTypeQuery.SelectedValue,
                  FormatHelper.CleanString(this.txtInvNoQuery.Text),
                  FormatHelper.CleanString(this.drpStackListQuery.SelectedValue),
                  FormatHelper.CleanString(this.drpStatusQuery.SelectedValue),
                  FormatHelper.TODateInt(this.txtCBDateQuery.Text),
                  FormatHelper.TODateInt(this.txtCEDateQuery.Text),
                  GetUserCode()
                  );
        }

        #endregion



        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                                ((ASN)obj).StNo,
                                FormatHelper.ToDateString(((ASN)obj).CDate),
                                ((ASN)obj).CUser,
                                this.GetInvInName(((ASN)obj).StType),
                                ((ASN)obj).InvNo,
                                this.GetStatusName(((ASN)obj).Status),
                                ((ASN)obj).StorageCode,
                                FormatHelper.ToDateString(((ASN)obj).PreictDate),
                                ((ASN)obj).DirectFlag,
                                ((ASN)obj).PickNo,
                                ((ASN)obj).VendorCode,
                                ((ASN)obj).ExigencyFlag,
                                ((ASN)obj).RejectsFlag,
                                ((ASN)obj).OANo,
                                ((ASN)obj).PackingListNo,
                          ((ASN)obj).ProvideDate,//      FormatHelper.ToDateString(((ASN)obj).ProvideDate),
                                ((ASN)obj).GrossWeight.ToString(),
                                ((ASN)obj).Volume,
                                ((ASN)obj).FromStorageCode,
                                ((ASN)obj).Remark1
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[]
                {
                                    "ASN",
                                    "CDate",
                                    "CUser",
                                    "StorageInType",
                                    "SAPInvNo",
                                    "Status",	
                                    "StorageInCode",
                                    "PredictDate",	
                                    "DirectFlag",
                                    "PickNo",	
                                    "VendorCode",
                                    "ExigencyFlag",	
                                    "RejectsFlag",
                                    "OANo",
                                    "PackingListNo",	
                                    "ProvideDate",
                                    "GrossWeight",	
                                    "Volume",
                                    "FromStorageCode",	
                                    "Remark1"
                };
        }

        #endregion



        protected override void gridWebGrid_ItemCommand(GridRecord row, string commandName)
        {
            if (commandName == "Detail")
            {
                string asnNo = row.Items.FindItemByKey("ASN").Text.Trim();
                Response.Redirect(this.MakeRedirectUrl("FExecuteASNDetailMP.aspx", new string[] { "ACT", "ASN", "Page" }, new string[] { "EDIT", asnNo, "FExecuteASNMP.aspx" }));
            }
        }

        #region Button
        //ȡ���·�
        protected void cmdInitial_ServerClick(object sender, EventArgs e)
        {
            GetServerClick("initial");
        }
        //����
        protected void cmdInitialCheck_ServerClick(object sender, EventArgs e)
        {
            GetServerClick("InitialCheck");
        }
        //����IQC
        protected void cmdApplyIQC_ServerClick(object sender, EventArgs e)
        {
            GetServerClick("ApplyIQC");
        }

        protected override object GetEditObject(GridRecord row)
        {
            if (facade == null)
            {
                facade = new InventoryFacade(base.DataProvider);
            }
            object obj = facade.GetASN(row.Items.FindItemByKey("ASN").Text);

            if (obj != null)
            {
                return obj;
            }

            return null;
        }



        #region GetServerClick
        private void GetServerClick(string clickName)
        {
            ArrayList array = this.gridHelper.GetCheckedRows();
            object obj = null;

            if (array.Count > 1)
            {
                WebInfoPublish.Publish(this, "ֻ��ѡȡһ������", this.languageComponent1);
                return;
            }
            if (array.Count > 0)
            {
                ArrayList objList = new ArrayList(array.Count);

                foreach (GridRecord row in array)
                {
                    obj = this.GetEditObject(row);

                    if (obj != null)
                    {
                        objList.Add(obj);
                    }
                }
                object[] asnList = ((ASN[])objList.ToArray(typeof(ASN)));
                ASN asn = (ASN)asnList[0];

                if (clickName == "initial")
                {
                    this.InitialObjects(asnList);
                }
                else if (clickName == "InitialCheck")
                {
                    if (asn.DirectFlag.ToUpper() == "Y")
                    {
                        WebInfoPublish.Publish(this, "�����ָ���ǹ�Ӧ��ֱ�������������²���[����][����IQC]", this.languageComponent1);
                        return;
                    }
                    InitialCheckObjects(asnList);
                }
                else if (clickName == "ApplyIQC")
                {
                    if (asn.DirectFlag.ToUpper() == "Y")
                    {
                        WebInfoPublish.Publish(this, "�����ָ���ǹ�Ӧ��ֱ�������������²���[����][����IQC]", this.languageComponent1);
                        return;
                    }
                    ApplyIqcObjects(asnList);
                }
                this.gridHelper.RequestData();
                this.buttonHelper.PageActionStatusHandle(PageActionType.Add);
            }
        }
        #endregion

        #region ȡ���·�
        protected void InitialObjects(object[] asnList)
        {
            //1>	���ָ���״̬Ϊ��WaitReceive:���ջ������޸����ָ���(TBLASN��TBLASNDETAIL)״̬Ϊ��Release:��ʼ��
            //2>	���ָ���״̬��Ϊ��WaitReceive:���ջ����򱨴���ʾ��ǰ���ָ��Ų���ȡ���·�

            try
            {

                InventoryFacade facade = new InventoryFacade(base.DataProvider);

                WarehouseFacade _wa = new WarehouseFacade(DataProvider);
                //foreach (ASN asn in asnList)
                // {
                ASN asn = (ASN)asnList[0];

                if (asn.Status != "Release" && asn.Status != "Receive" && asn.Status != "WaitReceive")
                {
                    WebInfoPublish.Publish(this, asn.StNo + "���ָ��Ų���ȡ���·�,״̬�����ǵ��������в���ȡ���·�", this.languageComponent1);
                    return;
                }

                this.DataProvider.BeginTransaction();

                _wa.UpdateASNForCancelDown(new string[] { asn.StNo }, "Release");

                facade.UpdateASNDetail(asn.StNo, ASNHeadStatus.Release);
                facade.UpdateASNDetailSN(asn.StNo);
                //facade.UpdateASNDetailItem(asn.StNo);


                DBDateTime dbTime1 = FormatHelper.GetNowDBDateTime(this.DataProvider);

                InvInOutTrans trans = _wa.CreateNewInvInOutTrans();
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
                trans.ProcessType = "CANCELISSUE";
                _wa.AddInvInOutTrans(trans);


                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {

                this.DataProvider.RollbackTransaction();
                throw ex;
            }

        }
        # endregion

        #region ����
        protected void InitialCheckObjects(object[] asnList)
        {
            //1>	������ָ���״̬�Ƿ�Ϊ��WaitReceive:���ջ��������򱨴���ʾ��ǰ���ָ���״̬���ǣ�
            //WaitReceive:���ջ�������ڶ���
            //2>	��ת��ͼ6.8.1������ǩ�ս��棬�����޸����ָ���(TBLASN��TBLASNDETAIL)״̬Ϊ��Receive:����
            if (facade == null)
            {
                facade = new InventoryFacade(base.DataProvider);
            }

            ASN asn = (ASN)asnList[0];

            if (asn.Status == ASNHeadStatus.WaitReceive)
            {
                try
                {
                    this.DataProvider.BeginTransaction();

                    string stNo = string.Format("'{0}'", asn.StNo);

                    facade.UpdateASNStatusByStNo(ASNHeadStatus.Receive, stNo);
                    facade.UpdateASNDetailStatusByStNo(ASNHeadStatus.Receive, stNo);

                    DBDateTime dbTime1 = FormatHelper.GetNowDBDateTime(this.DataProvider);
                    WarehouseFacade _wa = new WarehouseFacade(DataProvider);
                    InvInOutTrans trans = _wa.CreateNewInvInOutTrans();
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
                    trans.ProcessType = "RECEIVEBEGIN";
                    _wa.AddInvInOutTrans(trans);


                    this.DataProvider.CommitTransaction();
                    WebInfoPublish.Publish(this, "����ɹ�", this.languageComponent1);
                    Response.Redirect(this.MakeRedirectUrl("FASNReceiveMP.aspx", new string[] { "StNo" },
                    new string[] { asn.StNo }));
                }
                catch (Exception ex)
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.PublishInfo(this, ex.Message, this.languageComponent1);
                }
            }
            else
            {
                //WebInfoPublish.Publish(this, "���ܽ��г���", this.languageComponent1);
                //return;
                if (asn.Status != ASNHeadStatus.Release)
                {
                    Response.Redirect(this.MakeRedirectUrl("FASNReceiveMP.aspx", new string[] { "StNo" },
                    new string[] { asn.StNo }));
                }
                else
                {
                    WebInfoPublish.Publish(this, "���ܽ��г���", this.languageComponent1);
                    return;
                }
            }

        }
        # endregion

        #region ����IQC
        protected void ApplyIqcObjects(object[] asnList)
        {
            if (facade == null)
            {
                facade = new InventoryFacade(base.DataProvider);
            }
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            #region 6>	���¼���������ɵ������IQC��
            //1�� �������Ϊ��PD:�̵�
            //2�� �������Ϊ��POR: PO��� ���ҹ�Ӧ��ֱ����ʶΪ��Y
            //3�� �������Ϊ��SCTR:�������� �������������벻��Ʒ���ʶΪ��Y
            ASN asn = (ASN)asnList[0];
            if (asn.StType == InInvType.PD)
            {
                WebInfoPublish.Publish(this, "�������Ϊ�̵㣬��������IQC", this.languageComponent1);
                return;
            }
            else if (asn.StType == InInvType.POR && asn.DirectFlag == "Y")
            {
                WebInfoPublish.Publish(this, "�������ΪPO��Ⲣ�ҹ�Ӧ��ֱ����ʶΪY����������IQC", this.languageComponent1);
                return;
            }
            else if (asn.StType == InInvType.SCTR && asn.RejectsFlag == "Y")
            {
                WebInfoPublish.Publish(this, "�������Ϊ�������ϲ������������벻��Ʒ���ʶΪY����������IQC", this.languageComponent1);
                return;
            }
            //1>	����������ָ��Ŷ�Ӧ��ASN��ϸ����(TBLASNDETAIL)��ASN������Ŀ״̬����Ϊ��ReceiveClose:������ɣ�
            //  ���ҳ������״̬(TBLASNDETAIL.InitReceiveStatus)ȫ����Ϊ������(Reject:����)���������IQC�����򲻿�����IQC
            //bool hasdetail = facade.CheckASNHasDetail(asn.StNo, ASNLineStatus.ReceiveClose);//edit by sam
            //if (hasdetail)
            //{
            //    WebInfoPublish.Publish(this, "ASN������Ŀ״̬����Ϊ�������", this.languageComponent1);
            //    return;
            //}
            bool hasDetail = facade.CheckASNHasDetail(asn.StNo, ASNLineStatus.ReceiveClose);
            if (!hasDetail)
            {
                bool hasReject = facade.CheckASNReceiveStatusHasDetail(asn.StNo, "Reject");
                if (hasReject)
                {
                    //��ͷ���ݸ�Ϊ����״̬ IQCRejection:IQC���գ�
                    ASN oldAsn = (ASN)facade.GetASN(asn.StNo);
                    oldAsn.Status = ASNHeadStatus.IQCRejection;
                    facade.UpdateASN(oldAsn);
                    WebInfoPublish.Publish(this, "�������״̬��ȫ��Ϊ����״̬", this.languageComponent1);
                    return;
                }
            }
            else
            {
                WebInfoPublish.Publish(this, "ASN������Ŀ״̬����Ϊ�������", this.languageComponent1);
                return;
            }
            #endregion

            IQCFacade iqcFacade = new IQCFacade(base.DataProvider);
            object[] disdqMcodeList = facade.QueryAsnDetailForDqMcode(asn.StNo);
            if (disdqMcodeList == null)
            {
                WebInfoPublish.Publish(this, "���ָ��Ŷ�Ӧ��ASN��ϸ���в����ڣ���������IQC", this.languageComponent1);
                return;
            }
            //ͬһ���ָ���£�ͬһ�������ϱ��룬����һ��IQC���鵥�š�
            object[] dqMcodeList = facade.QueryAsnDetailForCreateIqc(asn.StNo);
            if (dqMcodeList == null)
            {
                WebInfoPublish.Publish(this, "IQC���鵥���Ѵ��ڣ�", this.languageComponent1);
                return;
            }
            try
            {
                this.DataProvider.BeginTransaction();
                string iqcNo = string.Empty;
                foreach (ASNDetail dqMcode in dqMcodeList)
                {
                    //edit by sam 2016��3��21�� �޳�����״̬
                    object[] detailList = facade.QueryAsnDetailByStNo(asn.StNo, dqMcode.DQMCode, "Reject");
                    if (detailList != null)
                    {
                        #region AsnIQC
                        ASNDetail asnDetailobj = detailList[0] as ASNDetail;
                        string newIqcNo = this.CreateNewIqcNo(asnDetailobj.StNo);
                        AsnIQC asnIqc = new AsnIQC();
                        asnIqc.IqcNo = newIqcNo;
                        iqcNo = newIqcNo;
                        asnIqc.IqcType = "";
                        asnIqc.StNo = asn.StNo;
                        asnIqc.InvNo = !string.IsNullOrEmpty(asn.InvNo) ? asn.InvNo : asn.StNo;


                        asnIqc.StType = asn.StType;	//	 STTYPE
                        asnIqc.Status = IQCStatus.IQCStatus_Release;	//	STATUS
                        asnIqc.AppDate = dbDateTime.DBDate;	//	MDATE
                        asnIqc.AppTime = dbDateTime.DBTime;	//	MTIME
                        asnIqc.InspDate = 0;	//	INSPDATE
                        asnIqc.InspTime = 0;	//	INSPTIME
                        asnIqc.CustmCode = asnDetailobj.CustMCode;	//	CUSTMCODE ��Ϊ���Ϻ�
                        asnIqc.MCode = asnDetailobj.MCode;	//	MCODE
                        asnIqc.DQMCode = asnDetailobj.DQMCode;	//	DQMCODE
                        asnIqc.MDesc = asnDetailobj.MDesc;	//	MDESC
                        // asnIqc.Qty = asnDetailobj.ReceiveQty;	//	QTY
                        asnIqc.QcStatus = "";	//	QCSTATUS IQC״̬(Y:�ϸ�N:���ϸ�)
                        asnIqc.VendorCode = asn.VendorCode;	//	VENDORCODE
                        asnIqc.VendorMCode = asnDetailobj.VendorMCodeDesc;	//	VENDORMCODE
                        asnIqc.Remark1 = asn.Remark1;	//	REMARK1
                        asnIqc.CUser = this.GetUserCode();	//	CUSER
                        asnIqc.CDate = dbDateTime.DBDate;	//	CDATE
                        asnIqc.CTime = dbDateTime.DBTime;//	CTIME
                        asnIqc.MaintainDate = dbDateTime.DBDate;	//	MDATE
                        asnIqc.MaintainTime = dbDateTime.DBTime;	//	MTIME
                        asnIqc.MaintainUser = this.GetUserCode();		//	MUSER
                        foreach (ASNDetail asnDetail in detailList)
                        {
                            asnIqc.Qty += asnDetail.ReceiveQty;
                        }
                        iqcFacade.AddAsnIQC(asnIqc);
                        #endregion
                        foreach (ASNDetail asnDetail in detailList)
                        {
                            #region
                            AsnIQCDetail iqcDetail = new AsnIQCDetail();

                            #region  iqcDetail
                            iqcDetail.IqcNo = newIqcNo;	//	IQCNO	�ͼ쵥��
                            iqcDetail.StNo = asnDetail.StNo;	//	STNO	ASN����
                            iqcDetail.StLine = asnDetail.StLine;	//	STLINE	ASN������Ŀ
                            iqcDetail.CartonNo = asnDetail.CartonNo; 	//	CARTONNO	�������
                            iqcDetail.Qty = asnDetail.ReceiveQty; 	//	QTY	�ͼ�����
                            iqcDetail.QcPassQty = 0; 	//	QCPASSQTY	����ͨ������
                            iqcDetail.Unit = asnDetail.Unit; 	//	UNIT	��λ
                            iqcDetail.NgQty = 0; 	//	NGQTY	ȱ��Ʒ��
                            iqcDetail.ReturnQty = 0; 	//	ReturnQTY	�˻�������
                            iqcDetail.ReformQty = 0; 	//	ReformQTY	�ֳ���������
                            iqcDetail.GiveQty = 0; 	//	GiveQTY	�ò���������
                            iqcDetail.AcceptQty = 0; 	//	AcceptQTY	�ز�����
                            iqcDetail.QcStatus = ""; 	//	QCSTATUS	 IQC״̬(Y:�ϸ�N:���ϸ�)
                            iqcDetail.Remark1 = asnDetail.Remark1; 	//	REMARK1	��ע
                            iqcDetail.CUser = this.GetUserCode();	//	CUSER
                            iqcDetail.CDate = dbDateTime.DBDate;	//	CDATE
                            iqcDetail.CTime = dbDateTime.DBTime;//	CTIME
                            iqcDetail.MaintainDate = dbDateTime.DBDate;	//	MDATE
                            iqcDetail.MaintainTime = dbDateTime.DBTime;	//	MTIME
                            iqcDetail.MaintainUser = this.GetUserCode();		//	MUSER
                            iqcFacade.AddAsnIQCDetail(iqcDetail);
                            #endregion

                            #region  AsnIqcDetailSN

                            object[] iqcDetailsnList = facade.GetSNbySTNo(asnDetail.StNo, asnDetail.StLine);
                            if (iqcDetailsnList != null)
                            {
                                foreach (Asndetailsn detailsn in iqcDetailsnList)
                                {
                                    AsnIqcDetailSN iqcDetailsn = new AsnIqcDetailSN();
                                    iqcDetailsn.IqcNo = newIqcNo; //	IQCNO	�ͼ쵥��
                                    iqcDetailsn.StNo = asnDetail.StNo; //	STNO	ASN����
                                    iqcDetailsn.StLine = asnDetail.StLine; //	STLINE	ASN������Ŀ
                                    iqcDetailsn.Sn = detailsn.Sn;
                                    iqcDetailsn.CartonNo = asnDetail.CartonNo; //	CARTONNO	�������
                                    iqcDetailsn.StNo = asnDetail.StNo; //	SN	SN����
                                    iqcDetailsn.QcStatus = ""; //	QCSTATUS	SN IQC״̬(Y:�ϸ�N:���ϸ�)
                                    iqcDetailsn.Remark1 = asnDetail.Remark1; //	REMARK1	��ע
                                    iqcDetailsn.CUser = this.GetUserCode(); //	CUSER
                                    iqcDetailsn.CDate = dbDateTime.DBDate; //	CDATE
                                    iqcDetailsn.CTime = dbDateTime.DBTime; //	CTIME
                                    iqcDetailsn.MaintainDate = dbDateTime.DBDate; //	MDATE
                                    iqcDetailsn.MaintainTime = dbDateTime.DBTime; //	MTIME
                                    iqcDetailsn.MaintainUser = this.GetUserCode(); //	MUSER
                                    iqcFacade.AddAsnIqcDetailSN(iqcDetailsn);
                                }
                            }
                            #endregion
                            #endregion
                        }
                    }
                    //�ж��Ƿ����������
                    if (_WarehouseFacade == null)
                        _WarehouseFacade = new WarehouseFacade(base.DataProvider);
                    BenQGuru.eMES.Domain.MOModel.Material mar = _WarehouseFacade.GetMaterialFromDQMCode(dqMcode.DQMCode);
                    int count = _WarehouseFacade.GetStockRecordCount(dbDateTime.DBDate, dbDateTime.DBTime, mar.MCode);
                    if (count > 0)
                    {
                        //���������
                        try
                        {
                            ToSTS(iqcNo);
                        }
                        catch (Exception ex)
                        {

                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, ex.Message, this.languageComponent1);
                            return;
                        }
                    }
                }
                // 3>	IQC���鵥������Դ��ASN����(TBLASN)��ASN��ϸ��(TBLASNDETAIL)��ASN��ϸSN��(TBLASNDETAILSN)��
                //�������ݱ��У��ͼ쵥(TBLASNIQC)���ͼ쵥��ϸ(TBLASNIQCDETAIL)���ͼ쵥��ϸSN��(TBLASNIQCDETAILSN)��
                //ע���ͼ�����(TBLASNIQCDETAIL.QTY)ΪASN��ϸ���еĽ�������(TBLASNDETAIL.ReceiveQTY)
                //4>	IQC�ͼ쵥�Ź���IQC+���ָ���+��λ��ˮ�ţ��磺IQCASN00000101

                //5>	����ASN����(TBLASN)״̬Ϊ��IQC:IQC
                var oldasn = (ASN)facade.GetASN(asn.StNo);
                if (oldasn != null)
                {
                    if (!(oldasn.Status == ASN_STATUS.ASN_Close || oldasn.Status == ASN_STATUS.ASN_Cancel || oldasn.Status == ASN_STATUS.ASN_OnLocation || oldasn.Status == ASN_STATUS.ASN_IQC))
                    {
                        oldasn.Status = "IQC";
                        facade.UpdateASN(oldasn);
                    }
                }

                this.DataProvider.CommitTransaction();
                WebInfoPublish.Publish(this, "����IQC�ɹ�", this.languageComponent1);


            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.PublishInfo(this, ex.Message, this.languageComponent1);
            }
        }

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
                asnIqc.IqcType = "ExemptCheck";
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
                trans.TransNO = asnIqc.StNo;
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
                    if (asnDetail != null)
                    {
                        asnDetail.QcPassQty = asnDetail.ReceiveQty;
                        asnDetail.Status = IQCStatus.IQCStatus_IQCClose;
                        _InventoryFacade.UpdateASNDetail(asnDetail);
                    }

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

            if (_IQCFacade.CanToOnlocationStaus(asnIqc.StNo))
            {

                ASN asn = (ASN)_InventoryFacade.GetASN(asnIqc.StNo);
                asn.Status = ASNHeadStatus.OnLocation;
                _InventoryFacade.UpdateASN(asn);
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
        # endregion


        private string CreateNewIqcNo(string stno)
        {
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            if (_TransferFacade == null)
            {
                _TransferFacade = new WarehouseFacade(this.DataProvider);
            }
            string maxserial = _TransferFacade.GetMaxSerial("IQC" + stno);

            //����������ֵ�ͷ���Ϊ��
            if (maxserial == "99")
            {
                return "";
            }

            SERIALBOOK serialbook = new SERIALBOOK();

            if (maxserial == "")
            {
                serialbook.SNPrefix = "IQC" + stno;
                serialbook.MaxSerial = "1";
                serialbook.MDate = dbDateTime.DBDate;
                serialbook.MTime = dbDateTime.DBTime;
                serialbook.MUser = this.GetUserCode();
                _TransferFacade.AddSerialBook(serialbook);
                return "IQC" + stno + string.Format("{0:00}", int.Parse(serialbook.MaxSerial));
            }
            else
            {
                serialbook.SNPrefix = "IQC" + stno;
                serialbook.MaxSerial = (int.Parse(maxserial) + 1).ToString();
                serialbook.MDate = dbDateTime.DBDate;
                serialbook.MTime = dbDateTime.DBTime;
                serialbook.MUser = this.GetUserCode();
                _TransferFacade.UpdateSerialBook(serialbook);
                return "IQC" + stno + string.Format("{0:00}", int.Parse(serialbook.MaxSerial));
            }
        }

        #endregion

    }
}
