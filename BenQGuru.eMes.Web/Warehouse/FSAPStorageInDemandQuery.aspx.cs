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
using System.Collections.Generic;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FSAPStorageInDemandQuery : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private InventoryFacade _InventoryFacade = null;
        private WarehouseFacade _warehouseFacade = null;
        SystemSettingFacade _SystemSettingFacade = null;
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
                this.InitCompleteList();
                this.InitStorageInTypeList();
                //add by sam 68
                if (isVendor)
                {
                    object[] objs_vendorCode = _UserFacade.GetVendorCode(this.GetUserCode());
                    foreach (UserGroup2Vendor ven in objs_vendorCode)
                    {
                        this.txtVendorCodeQuery.Text += ven.VendorCode + ",";
                    }
                    this.txtVendorCodeQuery.Text = this.txtVendorCodeQuery.Text.Trim().Substring(0, this.txtVendorCodeQuery.Text.Trim().Length - 1);
                    //this.txtVendorCodeQuery.Text = this.GetUserCode();
                    this.txtVendorCodeQuery.ReadOnly = true;
                }

                string invNo = Request.QueryString["InvNo"];
                txtInvNoQuery.Text = invNo;

                //if (!string.IsNullOrEmpty(this.GetRequestParam("StorageInType")))
                //{
                //    this.drpStorageInTypeQuery.SelectedIndex = this.drpStorageInTypeQuery.Items.IndexOf(this.drpStorageInTypeQuery.Items.FindByValue(this.GetRequestParam("StorageInType")));
                //}               
                //this.txtInvNoQuery.Text = this.GetRequestParam("InvNo");
                //this.drpCompleteQuery.SelectedIndex = this.drpStorageInTypeQuery.Items.IndexOf(this.drpStorageInTypeQuery.Items.FindByValue(this.GetRequestParam("Complete")));
                //if (!string.IsNullOrEmpty(this.GetRequestParam("VendorCode")))
                //{
                //    this.txtVendorCodeQuery.Text = this.GetRequestParam("VendorCode");
                //}
                //this.txtCreateUserQuery.Text = this.GetRequestParam("CreateUser");
                //string date = this.GetRequestParam("CBDate").ToString();
                //string time = this.GetRequestParam("EDate").ToString();
                //if (date.Length > 1)
                //{
                //    this.txtCBDateQuery.Text = FormatHelper.ToDateString(int.Parse(this.GetRequestParam("CBDate").ToString()));
                //}
                //if (time.Length > 1)
                //{
                //    this.txtCEDateQuery.Text = FormatHelper.ToTimeString(int.Parse(this.GetRequestParam("EDate").ToString()));
                //}
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        //��ʼ���������
        /// <summary>
        /// ��ʼ���������
        /// </summary>
        private void InitCompleteList()
        {
            this.drpCompleteQuery.Items.Add(new ListItem("", ""));
            this.drpCompleteQuery.Items.Add(new ListItem("N", "N"));
            this.drpCompleteQuery.Items.Add(new ListItem("Y", "Y"));
            this.drpCompleteQuery.SelectedIndex = 0;
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
            //base.InitWebGrid();
            //this.gridHelper.AddColumn("InvNo", "SAP���ݺ�", null);
            //this.gridHelper.AddColumn("StorageInType", "�������", null);
            //this.gridHelper.AddDataColumn("StorageInTypeCode", "������ʹ���", true);
            //this.gridHelper.AddColumn("InvStatus", "״̬", null);
            //this.gridHelper.AddColumn("FinishFlag", "���", null);
            //this.gridHelper.AddColumn("CUser", "������", null);
            //this.gridHelper.AddColumn("CDate", "��������", null);
            //this.gridHelper.AddColumn("VendorCode", "��Ӧ�̴���", null);
            //this.gridHelper.AddColumn("VendorName", "��Ӧ������", null);
            //this.gridHelper.AddColumn("OANo", "OA��ˮ��", null);
            //this.gridHelper.AddColumn("ASNAvailable", "�ɴ������ָ��", null);
            //this.gridHelper.AddEditColumn("CreateASN", "�������ָ��");
            //this.gridHelper.AddLinkColumn("LinkToDetail", "����");

            //this.gridHelper.AddDefaultColumn(false, false);

            ////������
            //this.gridHelper.ApplyLanguage(this.languageComponent1);
            base.InitWebGrid();
            for (int i = 0; i < this.SAPHeadViewFieldList.Length; i++)
            {
                this.gridHelper.AddColumn(this.SAPHeadViewFieldList[i].FieldName, /*this.languageComponent1.GetString(*/this.SAPHeadViewFieldList[i].Description/*)*/, null);
            }
            this.gridHelper.AddDefaultColumn(false, false);
            this.gridHelper.AddEditColumn("CreateASN", "�������ָ��");
            this.gridHelper.AddLinkColumn("LinkToDetail", "����");

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            if (!string.IsNullOrEmpty(this.txtInvNoQuery.Text))
            {
                this.gridHelper.RequestData();
            }
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();

            //row["InvNo"] = ((InvoicesExt)obj).InvNo;
            //row["StorageInType"] = this.GetInvInName(((InvoicesExt)obj).InvType);//������ͣ��������ͣ�
            //row["StorageInTypeCode"] = ((InvoicesExt)obj).InvType;
            //row["InvStatus"] = this.GetStatusName(((InvoicesExt)obj).InvStatus);
            //row["FinishFlag"] = ((InvoicesExt)obj).FinishFlag;
            ////row["CUser"] = ((InvoicesExt)obj).CUser;
            ////row["CDate"] = ((InvoicesExt)obj).CDate;
            //row["CUser"] = ((InvoicesExt)obj).CreateUser;
            //row["CDate"] = ((InvoicesExt)obj).PoCreateDate;
            //row["VendorCode"] = ((InvoicesExt)obj).VendorCode; // ((InvoicesExt)obj).VendorCode;
            //row["VendorName"] = ((InvoicesExt)obj).VendorName;
            //row["OANo"] = ((InvoicesExt)obj).OaMo;
            //row["ASNAvailable"] = ((InvoicesExt)obj).AsnAvailable;

            InvoicesExt inv = obj as InvoicesExt;
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
                if (field.FieldName == "InvType")
                {
                    strValue = this.GetInvInName(inv.InvType);

                }
                if (field.FieldName == "FinishFlag1")
                {
                    strValue = inv.FinishFlag;
                }
                else if (field.FieldName == "OrderStatus1")
                {
                    strValue = inv.OrderStatus;// this.GetStatusName(inv.OrderStatus);
                }
                else if (field.FieldName == "InvStatus")
                {
                    strValue = inv.InvStatus;// this.GetStatusName(inv.InvStatus);
                }
                else if (field.FieldName == "OrderStatus")
                {
                    strValue = this.GetStatusName(inv.OrderStatus);
                }
                else if (field.FieldName == "CreateUser")
                {

                    strValue = inv.CreateUser;
                    //if ((inv.InvType == "PRC") || (inv.InvType == "YFR") || (inv.InvType == "GZC"))
                    //{
                    //    strValue = inv.SAPCuser;
                    //}
                    //else
                    //{
                    //    strValue = inv.CreateUser;
                    //}
                }
                else if (field.FieldName == "PoCreateDate")
                {
                    if ((inv.InvType == "PRC") || (inv.InvType == "YFR") || (inv.InvType == "GZC"))
                    {
                        strValue = FormatHelper.ToDateString(inv.PoupDateDate);
                    }
                    else
                    {
                        strValue = FormatHelper.ToDateString(inv.PoCreateDate);
                    }
                }
                else if (field.FieldName == "PoupDateDate")
                {
                    strValue = FormatHelper.ToDateString(inv.PoupDateDate);
                }
                else if (field.FieldName == "PoupDateTime")
                {
                    strValue = FormatHelper.ToTimeString(inv.PoupDateTime);
                }
                else if (field.FieldName == "ApplyDate")
                {
                    strValue = FormatHelper.ToDateString(inv.ApplyDate);
                }
                else if (field.FieldName == "VoucherDate")
                {
                    strValue = FormatHelper.ToDateString(inv.VoucherDate);
                }

                else if (field.FieldName == "MESCDate")
                {
                    strValue = FormatHelper.ToDateString(inv.CDate);
                }
                else if (field.FieldName == "MESCTime")
                {
                    strValue = FormatHelper.ToTimeString(inv.CTime);
                }
                else if (field.FieldName == "MESCUser")
                {
                    strValue = inv.CUser;
                }
                else if (field.FieldName == "MESMaintainUser")
                {
                    strValue = inv.MaintainUser;
                }
                else if (field.FieldName == "MESMaintainDate")
                {
                    strValue = FormatHelper.ToDateString(inv.MaintainDate);
                }
                else if (field.FieldName == "MESMaintainTime")
                {
                    strValue = FormatHelper.ToTimeString(inv.MaintainTime);
                }

                else if (field.FieldName == "PlangiDate")
                {
                    strValue = FormatHelper.ToDateString(inv.PlangiDate);
                }
                else if (field.FieldName == "DnMDate")
                {
                    strValue = FormatHelper.ToDateString(inv.DnMDate);
                }
                else if (field.FieldName == "SAPRSCuser ")
                {
                    strValue = inv.SAPCuser;
                }
                else if (field.FieldName == "InvNo")
                {

                }
                else if (field.FieldName == "AsnAvailable")
                {
                    if (CanCreateASN(inv.InvNo))
                    {
                        strValue = "��";

                    }
                    else
                    {
                        strValue = "��";
                    }

                }
                else if (field.FieldName.ToUpper() == "NOTOUTCHECKFLAG")
                {
                    if (string.IsNullOrEmpty(inv.NotOutCheckFlag))
                    {
                        strValue = "��";
                    }
                    else if (inv.NotOutCheckFlag.ToUpper() == "X")
                    {
                        strValue = "��";
                    }
                    else
                    {
                        strValue = inv.NotOutCheckFlag;
                    }

                }
                //PlanSendDate
                row[i + 1] = strValue;

            }

            return row;

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }

            return this._InventoryFacade.QueryInvoices(
                FormatHelper.CleanString(this.drpStorageInTypeQuery.SelectedValue),
                FormatHelper.CleanString(this.txtInvNoQuery.Text),
                FormatHelper.CleanString(this.drpCompleteQuery.SelectedValue),
                FormatHelper.CleanString(this.txtVendorCodeQuery.Text),
                FormatHelper.CleanString(this.txtCreateUserQuery.Text),
                FormatHelper.TODateInt(this.txtCBDateQuery.Text),
                FormatHelper.TODateInt(this.txtCEDateQuery.Text),
                inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            return this._InventoryFacade.QueryInvoicesCount(
                FormatHelper.CleanString(this.drpStorageInTypeQuery.SelectedValue),
                FormatHelper.CleanString(this.txtInvNoQuery.Text),
                FormatHelper.CleanString(this.drpCompleteQuery.SelectedValue),
                FormatHelper.CleanString(this.txtVendorCodeQuery.Text),
                FormatHelper.CleanString(this.txtCreateUserQuery.Text),
                FormatHelper.TODateInt(this.txtCBDateQuery.Text),
                FormatHelper.TODateInt(this.txtCEDateQuery.Text));
        }

        #endregion

        #region Button
        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }

            if (_warehouseFacade == null)
            {
                _warehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            string invNo = row.Items.FindItemByKey("InvNo").Text.Trim();
            InvoicesExt ext = this._InventoryFacade.QueryInvoices(
               string.Empty,
               invNo,
               string.Empty,
               string.Empty,
               string.Empty,
               0,
               0,
               1, 1)[0] as InvoicesExt;

            string storageInType = ext.InvType;
            string asnAvailable = ext.AsnAvailable;

            if (commandName == "CreateASN")
            {
                if (asnAvailable == "N")
                {
                    WebInfoPublish.Publish(this, "�����Դ������ָ��", this.languageComponent1);
                    return;
                }




                if (!CanCreateASN(invNo))
                {
                    WebInfoPublish.Publish(this, "�ѳ���" + invNo + "������������������Ѿ���ȡ��", this.languageComponent1);
                    return;
                }



                if (storageInType == "UB")
                {
                    Pick pick = (Pick)_InventoryFacade.GetPickByInvNo(invNo);
                    if (pick == null)
                    {
                        WebInfoPublish.Publish(this, "��Ӧ�ļ����������ڣ�", this.languageComponent1);
                        return;
                    }
                    if (pick.Status != "Close")
                    {
                        WebInfoPublish.Publish(this, "������δ����", this.languageComponent1);
                        return;
                    }

                    bool isFirst = _InventoryFacade.IsFirstCreateAsn(invNo);
                    if (isFirst)
                    {
                        string stno = autoCreateASNFromCheckoutInfo(invNo, pick);
                        Response.Redirect(this.MakeRedirectUrl("FASNForBuyerAndLogisticMP.aspx",
                                         new string[] { "InvNo", "StorageInType", "StNo" },
                                        new string[] { invNo, storageInType, stno }));
                    }

                }


                if (isVendor)
                {
                    Response.Redirect(this.MakeRedirectUrl("FASNForVendorMP.aspx",
                                    new string[] { "InvNo", "StorageInType" },
                                    new string[] { invNo, storageInType }));
                }
                else
                {
                    Response.Redirect(this.MakeRedirectUrl("FASNForBuyerAndLogisticMP.aspx",
                                     new string[] { "InvNo", "StorageInType" },
                                     new string[] { invNo, storageInType }));
                }
            }
            else if (commandName == "LinkToDetail")
            {
                //67	7.����ֶ�ΪYʱ��������ת��fix
                if (asnAvailable == "Y")
                {
                    return;
                }

                Response.Redirect(this.MakeRedirectUrl("FSAPStorageInDemandDeatil.aspx",
                                    new string[] { "InvNo", "StorageInType", "StorageInType", "Complete", "VendorCode", "CreateUser" },
                                    new string[] { invNo, storageInType,
                                         FormatHelper.CleanString(this.drpStorageInTypeQuery.SelectedValue),
                FormatHelper.CleanString(this.drpCompleteQuery.SelectedValue),
                FormatHelper.CleanString(this.txtVendorCodeQuery.Text),
                FormatHelper.CleanString(this.txtCreateUserQuery.Text), }));
            }
        }

        private string autoCreateASNFromCheckoutInfo(string invNo, Pick pick)
        {
            try
            {
                InvoicesDetail invD = (InvoicesDetail)_InventoryFacade.GetInvoicesDetail(invNo);
                CARTONINVOICES[] cartos = _warehouseFacade.GetGrossAndWeight(pick.PickNo);
                string stno = CreateStNO();
                Asn asn = new Asn();
                asn.Stno = stno;
                asn.StType = "UB";
                asn.CDate = FormatHelper.TODateInt(DateTime.Now);
                asn.CTime = FormatHelper.TOTimeInt(DateTime.Now);
                asn.CUser = GetUserCode();
                asn.Status = "Release";
                asn.Invno = invNo;
                asn.FacCode = pick.FacCode;
                if (cartos.Length > 0)
                {
                    asn.Gross_weight = (decimal)cartos[0].GROSS_WEIGHT;
                    asn.Volume = cartos[0].VOLUME;
                }
                asn.StorageCode = invD.StorageCode ?? " ";
                asn.MaintainUser = GetUserCode();
                asn.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                asn.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
                CartonInvDetailMaterial[] cartonMs = _warehouseFacade.GetCartonInvDetailMaterial(pick.PickNo);
                MOModel.ItemFacade _itemfacade = new BenQGuru.eMES.MOModel.ItemFacade(this.DataProvider);

                int i = 1;

                this.DataProvider.BeginTransaction();
                foreach (CartonInvDetailMaterial m in cartonMs)
                {
                    object materobj = _itemfacade.GetMaterial(m.MCODE);
                    Domain.MOModel.Material mater = materobj as Domain.MOModel.Material;
                    if (materobj == null)
                    {
                        this.DataProvider.RollbackTransaction();
                        throw new Exception("���ϱ�û�����ϣ�" + m.MCODE);


                    }
                    string custMCode = _warehouseFacade.GetCustMCodeForUB(pick.PickNo, m.DQMCODE);
                    if (mater.MCONTROLTYPE == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS)
                    {
                        ASNDetail asnd = new ASNDetail();
                        asnd.ActQty = 0;
                        //asnd.CartonNo = " ";
                        asnd.DQMCode = m.DQMCODE;
                        asnd.Qty = (int)m.QTY;
                        asnd.StLine = i.ToString();
                        asnd.CustMCode = custMCode;
                        asnd.MCode = m.MCODE;
                        asnd.Status = "Release";
                        asnd.StNo = stno;
                        asnd.MDesc = mater.MchshortDesc;
                        asnd.Unit = m.UNIT;
                        asnd.CDate = FormatHelper.TODateInt(DateTime.Now);
                        asnd.CTime = FormatHelper.TOTimeInt(DateTime.Now);
                        asnd.CUser = GetUserCode();
                        asnd.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
                        asnd.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                        asnd.MaintainUser = GetUserCode();
                        asnd.LotNo = " ";
                        CARTONINVDETAILSN[] sns = _warehouseFacade.GetCartonInvDetailSn(m.CARTONNO, pick.PickNo);
                        List<string> snList = new List<string>();
                        if (sns.Length > 0)
                        {
                            foreach (CARTONINVDETAILSN sn in sns)
                            {
                                snList.Add(sn.SN);
                            }
                            Asndetail detail = _warehouseFacade.GetFirstCheckInAsnDetail(snList);
                            if (detail != null)
                            {
                                asnd.ProductionDate = detail.Production_Date;
                                asnd.SupplierLotNo = detail.Supplier_lotno;
                                asnd.LotNo = detail.Lotno;
                                asnd.StorageAgeDate = detail.StorageageDate;
                            }
                        }
                        DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                        Asndetailitem detailitem = _warehouseFacade.CreateNewAsndetailitem();
                        detailitem.CDate = dbTime.DBDate;
                        detailitem.CTime = dbTime.DBTime;
                        detailitem.CUser = this.GetUserCode();
                        detailitem.MaintainDate = dbTime.DBDate;
                        detailitem.MaintainTime = dbTime.DBTime;
                        detailitem.MaintainUser = this.GetUserCode();
                        detailitem.Stline = i.ToString();
                        detailitem.Stno = asn.Stno;
                        detailitem.MCode = asnd.MCode;
                        detailitem.DqmCode = asnd.DQMCode;

                        //���Ҷ�Ӧ��SAP��
                        object[] qtyobjs = _warehouseFacade.GetSAPNOandLinebyMCODE(asn.Invno, asnd.MCode, asnd.DQMCode);
                        if (qtyobjs == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            throw new Exception("�������" + asn.Invno + " û���������" + asnd.MCode + "," + asnd.DQMCode + "���ߴ����ѱ�ȡ����");


                        }
                        decimal sub = asnd.Qty;
                        for (int k = 0; k < qtyobjs.Length; k++)
                        {
                            InvoicesDetail invdetail = qtyobjs[k] as InvoicesDetail;
                            decimal subNeed = 0;
                            object findNeedQTY_old = _warehouseFacade.GetNeedImportQtyOLD(invdetail.InvNo, invdetail.InvLine, asnd.StNo);  //�����invoice���Ѿ������˶��٣������˶���
                            Asndetailitem subItemOld = findNeedQTY_old as Asndetailitem;
                            object findNeedQTY_now = _warehouseFacade.GetNeedImportQtyNow(invdetail.InvNo, invdetail.InvLine, asnd.StNo);  //�����invoice���Ѿ������˶��٣������˶���
                            Asndetailitem subItemNow = findNeedQTY_now as Asndetailitem;

                            subNeed = invdetail.PlanQty - subItemOld.Qty + (subItemOld.Qty - subItemOld.ReceiveQty) + (subItemOld.ReceiveQty - subItemOld.QcpassQty);
                            subNeed = subNeed - subItemNow.Qty;

                            if (subNeed == 0)
                                continue;

                            //�����������������������---���в��
                            if (sub > subNeed)
                            {
                                sub = sub - subNeed;  //  sub��ʣ���
                                detailitem.Qty = subNeed;
                                detailitem.Invline = invdetail.InvLine.ToString();
                                detailitem.Invno = invdetail.InvNo;
                                detailitem.ActQty = detailitem.Qty;
                                detailitem.QcpassQty = detailitem.Qty;
                                detailitem.ReceiveQty = detailitem.Qty;
                                _warehouseFacade.AddAsndetailitem(detailitem);


                            }

                            //����䵥����С�ڵ�������������--ֱ������
                            else
                            {

                                detailitem.Qty = sub;
                                detailitem.Invline = invdetail.InvLine.ToString();
                                detailitem.Invno = invdetail.InvNo;
                                detailitem.ActQty = detailitem.Qty;
                                detailitem.QcpassQty = detailitem.Qty;
                                detailitem.ReceiveQty = detailitem.Qty;
                                _warehouseFacade.AddAsndetailitem(detailitem);
                                sub = 0;

                            }
                            if (sub == 0)
                            {
                                break;
                            }
                        }
                        //���sub>0��˵�������������࣬����
                        if (sub > 0)
                        {
                            this.DataProvider.RollbackTransaction();

                            throw new Exception("�䵥����������������������!");

                        }

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
                            asnSN.Stno = stno;
                            _warehouseFacade.AddAsndetailsn(asnSN);
                        }
                        _InventoryFacade.AddASNDetail(asnd);
                    }
                    else if (mater.MCONTROLTYPE == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_LOT || mater.MCONTROLTYPE == SAP_CONTROLTYPE.SAP_ITEM_CONTROL_NOCONTROL)
                    {
                        ASNDetail asnd = new ASNDetail();
                        asnd.ActQty = 0;
                        asnd.CustMCode = custMCode;
                        asnd.DQMCode = m.DQMCODE;
                        asnd.Qty = (int)m.QTY;
                        asnd.StLine = i.ToString();
                        asnd.MCode = m.MCODE;
                        asnd.StNo = stno;
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
                        Pickdetailmaterial pickMaterial = (Pickdetailmaterial)_warehouseFacade.GetLotNOInformationFromDQMCODE(m.DQMCODE, pick.PickNo);

                        if (pickMaterial != null)
                        {
                            asnd.ProductionDate = pickMaterial.Production_Date;
                            asnd.SupplierLotNo = pickMaterial.Supplier_lotno;
                            asnd.LotNo = pickMaterial.Lotno;
                            asnd.StorageAgeDate = pickMaterial.StorageageDate;
                        }
                        else
                        {

                            throw new Exception(pick.PickNo + "û���ҵ�������Ϣ��");
                        }
                        _InventoryFacade.AddASNDetail(asnd);

                        DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                        Asndetailitem detailitem = _warehouseFacade.CreateNewAsndetailitem();
                        detailitem.CDate = dbTime.DBDate;
                        detailitem.CTime = dbTime.DBTime;
                        detailitem.CUser = this.GetUserCode();
                        detailitem.MaintainDate = dbTime.DBDate;
                        detailitem.MaintainTime = dbTime.DBTime;
                        detailitem.MaintainUser = this.GetUserCode();
                        detailitem.Stline = i.ToString();
                        detailitem.Stno = asn.Stno;
                        detailitem.MCode = asnd.MCode;
                        detailitem.DqmCode = asnd.DQMCode;

                        //���Ҷ�Ӧ��SAP��
                        object[] qtyobjs = _warehouseFacade.GetSAPNOandLinebyMCODE(asn.Invno, asnd.MCode, asnd.DQMCode);
                        if (qtyobjs == null)
                        {
                            this.DataProvider.RollbackTransaction();
                            throw new Exception("�䵥����������������������!");

                        }
                        decimal sub = asnd.Qty;
                        for (int k = 0; k < qtyobjs.Length; k++)
                        {
                            InvoicesDetail invdetail = qtyobjs[k] as InvoicesDetail;
                            decimal subNeed = 0;
                            object findNeedQTY_old = _warehouseFacade.GetNeedImportQtyOLD(invdetail.InvNo, invdetail.InvLine, asnd.StNo);  //�����invoice���Ѿ������˶��٣������˶���
                            Asndetailitem subItemOld = findNeedQTY_old as Asndetailitem;
                            object findNeedQTY_now = _warehouseFacade.GetNeedImportQtyNow(invdetail.InvNo, invdetail.InvLine, asnd.StNo);  //�����invoice���Ѿ������˶��٣������˶���
                            Asndetailitem subItemNow = findNeedQTY_now as Asndetailitem;

                            subNeed = invdetail.PlanQty - subItemOld.Qty + (subItemOld.Qty - subItemOld.ReceiveQty) + (subItemOld.ReceiveQty - subItemOld.QcpassQty);
                            subNeed = subNeed - subItemNow.Qty;

                            if (subNeed == 0)
                                continue;

                            //�����������������������---���в��
                            if (sub > subNeed)
                            {
                                sub = sub - subNeed;  //  sub��ʣ���
                                detailitem.Qty = subNeed;
                                detailitem.Invline = invdetail.InvLine.ToString();
                                detailitem.Invno = invdetail.InvNo;
                                detailitem.ActQty = detailitem.Qty;
                                detailitem.QcpassQty = detailitem.Qty;
                                detailitem.ReceiveQty = detailitem.Qty;
                                _warehouseFacade.AddAsndetailitem(detailitem);


                            }

                            //����䵥����С�ڵ�������������--ֱ������
                            else
                            {

                                detailitem.Qty = sub;
                                detailitem.Invline = invdetail.InvLine.ToString();
                                detailitem.Invno = invdetail.InvNo;
                                detailitem.ActQty = detailitem.Qty;
                                detailitem.QcpassQty = detailitem.Qty;
                                detailitem.ReceiveQty = detailitem.Qty;
                                _warehouseFacade.AddAsndetailitem(detailitem);
                                sub = 0;

                            }
                            if (sub == 0)
                            {
                                break;
                            }
                        }
                        //���sub>0��˵�������������࣬����
                        if (sub > 0)
                        {
                            this.DataProvider.RollbackTransaction();

                            throw new Exception("�䵥����������������������!");

                        }


                    }
                    i++;
                }
                _warehouseFacade.AddAsn(asn);
                WarehouseFacade facade = new WarehouseFacade(base.DataProvider);
                #region ��invinouttrans��������һ������
                //ASN asn = (ASN)domainObject;
                DBDateTime dbTime1 = FormatHelper.GetNowDBDateTime(this.DataProvider);
                InvInOutTrans trans = facade.CreateNewInvInOutTrans();
                trans.CartonNO = string.Empty;
                trans.DqMCode = string.Empty;
                trans.FacCode = asn.FacCode;
                trans.FromFacCode = asn.FromfacCode;
                trans.FromStorageCode = asn.FromstorageCode;
                trans.InvNO = asn.Invno;
                trans.InvType = asn.StType;
                trans.LotNo = string.Empty;
                trans.MaintainDate = dbTime1.DBDate;
                trans.MaintainTime = dbTime1.DBTime;
                trans.MaintainUser = this.GetUserCode();
                trans.MCode = string.Empty;
                trans.ProductionDate = 0;
                trans.Qty = 0;
                trans.Serial = 0;
                trans.StorageAgeDate = 0;
                trans.StorageCode = asn.StorageCode;
                trans.SupplierLotNo = string.Empty;
                trans.TransNO = asn.Stno;
                trans.TransType = "IN";
                trans.Unit = string.Empty;
                trans.ProcessType = "UBCreateASN";
                facade.AddInvInOutTrans(trans);
                #endregion
                this.DataProvider.CommitTransaction();
                return stno;

            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                throw ex;
            }
        }
        #endregion

        private string CreateStNO()
        {
            WarehouseFacade warehouseFacade = new WarehouseFacade(base.DataProvider);
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
            string preFix = "IN" + dbDateTime.DBDate.ToString().Substring(2);
            object objSerialBook = warehouseFacade.GetSerialBook(preFix);

            if (objSerialBook == null)
            {
                SERIALBOOK serialBook = new SERIALBOOK();
                serialBook.SNPrefix = preFix;
                serialBook.MaxSerial = "1";
                serialBook.MUser = this.GetUserCode();
                serialBook.MDate = dbDateTime.DBDate;
                serialBook.MTime = dbDateTime.DBTime;

                warehouseFacade.AddSerialBook(serialBook);
                return preFix + "0001";


            }
            else
            {
                SERIALBOOK serialBook = (SERIALBOOK)objSerialBook;
                if (serialBook.MaxSerial == "9999")
                {
                    throw new Exception("��������ָ������ã�");

                }
                serialBook.MaxSerial = (Convert.ToInt32(serialBook.MaxSerial) + 1).ToString();

                warehouseFacade.UpdateSerialBook(serialBook);
                return serialBook.SNPrefix + serialBook.MaxSerial.PadLeft(4, '0');


            }



        }
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
                    object[] objs = _InventoryFacade.QueryViewFieldByUserCode(this.GetUserCode(), "TBLINVOICESEX");
                    if (objs != null)
                    {
                        viewFieldList = new ViewField[objs.Length];
                        objs.CopyTo(viewFieldList, 0);
                    }
                    else
                    {
                        objs = _InventoryFacade.QueryViewFieldDefault("INVOICESEX_FIELD_LIST_SYSTEM_DEFAULT", "TBLINVOICESEX");
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
                            if (viewFieldList[i].FieldName == "InvNo")
                            {
                                bExistPickNo = true;
                                break;
                            }
                        }
                        if (bExistPickNo == false)
                        {
                            ViewField field = new ViewField();
                            field.FieldName = "InvNo";
                            field.Description = "�������";
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
        #region Others
        private bool CanCreateASN(string invNo)
        {
            if (_warehouseFacade == null)
                _warehouseFacade = new WarehouseFacade(base.DataProvider);


            if (!_warehouseFacade.IsInvoicesAvailable(invNo))
                return false;
            return true;
        }
        #endregion

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            //return new string[]{((InvoicesExt)obj).InvNo,
            //                    this.GetInvInName(((InvoicesExt)obj).InvType),
            //                    this.GetStatusName(((InvoicesExt)obj).InvStatus),
            //                    ((InvoicesExt)obj).FinishFlag,
            //                    ((InvoicesExt)obj).CUser,
            //                    FormatHelper.ToDateString(((InvoicesExt)obj).CDate),
            //                    "",//((InvoicesExt)obj).VendorCode,
            //                    ((InvoicesExt)obj).VendorName,
            //                    ((InvoicesExt)obj).OaMo,
            //                    ((InvoicesExt)obj).AsnAvailable
            //                   };

            string[] objs = new string[this.SAPHeadViewFieldList.Length];
            InvoicesExt inv = obj as InvoicesExt;
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
                if (field.FieldName == "InvType")
                {
                    strValue = this.GetInvInName(inv.InvType);

                }
                if (field.FieldName == "FinishFlag1")
                {
                    strValue = inv.FinishFlag;
                }
                else if (field.FieldName == "OrderStatus1")
                {
                    strValue = inv.OrderStatus;// this.GetStatusName(inv.OrderStatus);
                }
                else if (field.FieldName == "InvStatus")
                {
                    strValue = inv.InvStatus;// this.GetStatusName(inv.InvStatus);
                }
                else if (field.FieldName == "PoCreateDate")
                {
                    strValue = FormatHelper.ToDateString(inv.PoCreateDate);
                }
                else if (field.FieldName == "PoupDateDate")
                {
                    strValue = FormatHelper.ToDateString(inv.PoupDateDate);
                }
                else if (field.FieldName == "PoupDateTime")
                {
                    strValue = FormatHelper.ToTimeString(inv.PoupDateTime);
                }
                else if (field.FieldName == "ApplyDate")
                {
                    strValue = FormatHelper.ToDateString(inv.ApplyDate);
                }
                else if (field.FieldName == "VoucherDate")
                {
                    strValue = FormatHelper.ToDateString(inv.VoucherDate);
                }
                else if (field.FieldName == "CDate")
                {
                    strValue = FormatHelper.ToDateString(inv.CDate);
                }
                else if (field.FieldName == "CTime")
                {
                    strValue = FormatHelper.ToTimeString(inv.CTime);
                }
                else if (field.FieldName == "PlangiDate")
                {
                    strValue = FormatHelper.ToDateString(inv.PlangiDate);
                }
                else if (field.FieldName == "DnMDate")
                {
                    strValue = FormatHelper.ToDateString(inv.DnMDate);
                }
                else if (field.FieldName == "AsnAvailable")
                {
                    if (CanCreateASN(inv.InvNo))
                    {
                        strValue = "��";

                    }
                    else
                    {
                        strValue = "��";
                    }

                }
                objs[i] = strValue;
            }
            return objs;
        }

        protected override string[] GetColumnHeaderText()
        {
            //return new string[] {	"InvNo",
            //                        "StorageInType",
            //                        "InvStatus",
            //                        "FinishFlag",
            //                        "CUser",
            //                        "CDate",	
            //                        "VendorCode",
            //                        "VendorName",	
            //                        "OANo",
            //                        "ASNAvailable"};

            string[] strHeader = new string[this.SAPHeadViewFieldList.Length];

            for (int i = 0; i < strHeader.Length; i++)
            {
                strHeader[i] = this.SAPHeadViewFieldList[i].Description;
            }
            return strHeader;
        }

        #endregion

    }
}
