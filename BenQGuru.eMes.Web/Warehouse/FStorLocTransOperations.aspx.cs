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
//using System.Windows.Forms;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Web.WarehouseWeb;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
using BenQGuru.eMES.MOModel;
using Infragistics.WebUI.Shared;
using ShareLib;


namespace BenQGuru.eMES.Web.Warehouse
{
    public partial class FStorLocTransOperations : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        private InventoryFacade _InventoryFacade = null;
        private WarehouseFacade _WarehouseFacade = null;
        private InventoryFacade inventoryFacade = null;
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
                InitTransNoList();
                this.InitPageLanguage(this.languageComponent1, false);
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        #endregion


        private void InitTransNoList()
        {
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }

            object[] objPick = _InventoryFacade.GetTransNoByPStatus();
            this.drpTransNoQuery.Items.Add(new ListItem("", ""));
            if (objPick != null)
            {
                foreach (Storloctrans trans in objPick)
                {
                    this.drpTransNoQuery.Items.Add(new ListItem(trans.Transno, trans.Transno));
                }
            }
            this.drpTransNoQuery.SelectedIndex = 0;
        }

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("DQMCode", "�������ϱ���", null);
            this.gridHelper.AddColumn("InvNo", "SAP���ݺ�", null);
            this.gridHelper.AddColumn("RequireQty", "��������", null);
            //this.gridHelper.AddColumn("HasReMoveQty", "���ƶ�����", null);
            this.gridHelper.AddColumn("HasRePickQty", "�Ѽ������", null);
            this.gridHelper.AddColumn("MUOM", "��λ", null);
            this.gridHelper.AddColumn("FromStorage", "Դ��λ", null);
            this.gridHelper.AddColumn("ToStorage", "Ŀ���λ", null);


            this.gridHelper.AddColumn("FromLocation", "Դ��λ", null);
            this.gridHelper.AddColumn("ToLocation", "Ŀ���λ", null);
            this.gridHelper.AddColumn("FromCartonno", "Դ���", null);
            this.gridHelper.AddColumn("ToCartonno", "Ŀ�����", null);



            this.gridHelper.AddColumn("TransUser", "������", null);
            this.gridHelper.AddColumn("TransTime", "����ʱ��", null);

            this.gridHelper.AddDefaultColumn(true, false);
            this.gridHelper.AddLinkColumn("LinkToCarton", "�����Ϣ");
            this.gridHelper.AddLinkColumn("LinkToSN", "SN��Ϣ");
            this.gridWebGrid.Columns.FromKey("LinkToSN").Hidden = true;

            this.gridHelper.AddColumn("TransNo", "��λ�ƶ�����", null);
            this.gridHelper.AddColumn("MCode", "���ϴ���", null);
            this.gridWebGrid.Columns.FromKey("TransNo").Hidden = true;
            this.gridWebGrid.Columns.FromKey("MCode").Hidden = true;


            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["TransNo"] = ((StorLocTransOperations)obj).Transno;
            row["MCode"] = ((StorLocTransOperations)obj).MCode;

            row["DQMCode"] = ((StorLocTransOperations)obj).DqmCode;
            row["InvNo"] = ((StorLocTransOperations)obj).InvNo;
            row["RequireQty"] = ((StorLocTransOperations)obj).Qty;
            row["HasRePickQty"] = ((StorLocTransOperations)obj).HasReMoveQty;
            row["MUOM"] = ((StorLocTransOperations)obj).Unit;
            row["FromStorage"] = ((StorLocTransOperations)obj).FromstorageCode;
            row["ToStorage"] = ((StorLocTransOperations)obj).StorageCode;


            row["FromLocation"] = ((StorLocTransOperations)obj).FromlocationCode;

            row["ToLocation"] = ((StorLocTransOperations)obj).LocationCode;
            row["FromCartonno"] = ((StorLocTransOperations)obj).Fromcartonno;
            row["ToCartonno"] = ((StorLocTransOperations)obj).Cartonno;



            row["TransUser"] = ((StorLocTransOperations)obj).CUser;
            row["TransTime"] = ((StorLocTransOperations)obj).CTime;

            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }

            return this._WarehouseFacade.QueryStorLocTransOperations(
                FormatHelper.CleanString(this.drpTransNoQuery.SelectedValue.Trim().ToUpper()),
                inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            return this._WarehouseFacade.QueryStorLocTransOperationsCount(
                FormatHelper.CleanString(this.drpTransNoQuery.SelectedValue.Trim().ToUpper())
                );
        }




        #endregion

        #region Button
        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            string transNo = this.drpTransNoQuery.SelectedValue.Trim();

            if (commandName == "LinkToCarton")
            {
                string dqMcode = row.Items.FindItemByKey("DQMCode").Text.Trim();
                Response.Redirect(
                                    this.MakeRedirectUrl("FStorLocTransDetailCarton.aspx",
                                    new string[] { "TRANSNO", "Page", "DQMCODE" },
                                    new string[] { transNo, "FStorLocTransOperations.aspx", dqMcode })
                                   );
            }

            if (commandName == "LinkToSN")
            {
                string dqMcode = row.Items.FindItemByKey("DQMCode").Text.Trim();
                Response.Redirect(
                                    this.MakeRedirectUrl("FStorLocTransDetailSN.aspx",
                                    new string[] { "TRANSNO", "Page", "DQMCODE" },
                                    new string[] { transNo, "FStorLocTransOperations.aspx", dqMcode })
                                   );
            }
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {


        }




        //�ύ 
        protected void cmdSubmit_ServerClick(object sender, System.EventArgs e)
        {
            if (!ValidateCommit())
            {
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
            string transNo = this.drpTransNoQuery.SelectedValue.Trim().ToUpper();
            string fromCartonNo = this.txtFromCartonNo.Text.Trim().ToUpper();
            string qty = this.txtQTY.Text.Trim();
            string sn = this.txtSNEdit.Text.Trim().ToUpper();

            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
            string mUser = this.GetUserCode();
            int mDate = dbDateTime.DBDate;
            int mTime = dbDateTime.DBTime;

            string tLocationCartonNo = "";
            string locationCode = "";
            bool isSN = false;

            ShareTrans shareTrans = new ShareTrans();
            string message = shareTrans.Submit(transNo, fromCartonNo, qty, sn, " ", rdoSelectType.SelectedValue, mUser, DataProvider);
            WebInfoPublish.Publish(this, message, this.languageComponent1);

        }

        //�ϼ�
        protected void cmdShelves_ServerClick(object sender, System.EventArgs e)
        {
            if (!ValidateShelves())
            {
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
            string tCartonNo = this.txtTLocationCartonEdit.Text.Trim().ToUpper();
            string locationCode = this.txtLocationCode.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(tCartonNo))
            {
                WebInfoPublish.Publish(this, "Ŀ����ű��", this.languageComponent1);
                return;
            }
            if (string.IsNullOrEmpty(locationCode))
            {
                WebInfoPublish.Publish(this, "Ŀ���λ�ű��", this.languageComponent1);
                return;
            }
            #region ��ѡһ��
            if (gridWebGrid.Rows.Count <= 0)
            {
                WebInfoPublish.Publish(this, "Gird���ݲ���Ϊ��", this.languageComponent1);
                return;
            }
            ArrayList array = this.gridHelper.GetCheckedRows();

            if (array.Count != 1)
            {
                WebInfoPublish.Publish(this, "ֻ��ѡ��һ������", this.languageComponent1);
                return;
            }

            string transNo = "";
            string inputqty = "";
            string mcode = "";
            string fromCartonno = "";
            string mUser = this.GetUserCode();
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
            int mDate = dbDateTime.DBDate;
            int mTime = dbDateTime.DBTime;
            foreach (GridRecord row in array)
            {
                transNo = row.Items.FindItemByKey("TransNo").Text;
                inputqty = row.Items.FindItemByKey("HasRePickQty").Text;//�Ѽ�����
                mcode = row.Items.FindItemByKey("MCode").Text;
                fromCartonno = row.Items.FindItemByKey("FromCartonno").Text;
            }
            StorageDetail storageDetail = (StorageDetail)_InventoryFacade.GetStorageDetail(fromCartonno);
            if (storageDetail == null)
            {
                DataProvider.RollbackTransaction();
                WebInfoPublish.Publish(this, "�����ϸ��Ϣ����û�ж�Ӧ��ŵ�����", this.languageComponent1);
                return;
            }



            int qty = Convert.ToInt32(Convert.ToDecimal(inputqty));

            #endregion
            try
            {
                this.DataProvider.BeginTransaction();

                //����ж�Ŀ���λ�Ƿ�����ת������Ӧ��Ŀ���λ
                Location Location = this._InventoryFacade.GetLocation(locationCode, GlobalVariables.CurrentOrganizations.First().OrganizationID) as Location;
                if (Location == null)
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.Publish(this, "Ŀ���λ������", this.languageComponent1);
                    return;
                }
                Storloctrans _Storloctrans = (Storloctrans)this._WarehouseFacade.GetStorloctrans(transNo);
                if (_Storloctrans == null)
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.Publish(this, "�޴�ת������Ϣ", this.languageComponent1);
                    return;
                }
                if (!Location.StorageCode.Equals(_Storloctrans.StorageCode))
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.Publish(this, "Ŀ���λ������ת������Ӧ��Ŀ���λ", this.languageComponent1);
                    return;
                }

                decimal totalsum = 0;
                StorloctransDetailCarton[] locatransCartons = _WarehouseFacade.GetStorloctransdetailcarton1s(transNo, fromCartonno);
                foreach (StorloctransDetailCarton loccartonno in locatransCartons)
                {
                    totalsum += loccartonno.Qty;
                }

                if (storageDetail.StorageQty > totalsum && tCartonNo == fromCartonno)
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.Publish(this, "����Ŀ����Ų�����ԭ���һ����", this.languageComponent1);
                    return;
                }

                _WarehouseFacade.UpdateStorloctransdetailcarton1(transNo, tCartonNo, locationCode, fromCartonno);
                _WarehouseFacade.UpdateStorloctransDetailSN1(transNo, tCartonNo, fromCartonno);

                StorloctransDetail storloctransDetail = _WarehouseFacade.GetStorloctransdetail(transNo, storageDetail.MCode)
                 as StorloctransDetail;
                if (storloctransDetail == null)
                {
                    WebInfoPublish.Publish(this, "ת������û�ж�Ӧ�����Ϻ�", this.languageComponent1);
                    return;

                }
                //�� �ж�����TransNo,MCODE ��sum��TBLStorLocTransDetailCarton.QTY���Ƿ������������TBLStorLocTransDetail.QTY��
                ///������������װ��TBLStorLocTransDetail.Status=Close:���״̬��
                decimal sum = 0;
                object[] _objs = _WarehouseFacade.GetStorloctransdetailcarton(transNo, storageDetail.MCode);
                foreach (StorloctransDetailCarton storloctransDetailCarton in _objs)
                {
                    sum += storloctransDetailCarton.Qty;
                }
                if (sum == storloctransDetail.Qty)
                {
                    storloctransDetail.Status = "Close";
                    storloctransDetail.MaintainUser = mUser;
                    storloctransDetail.MaintainDate = mDate;
                    storloctransDetail.MaintainTime = mTime;
                    _WarehouseFacade.UpdateStorloctransdetail(storloctransDetail);
                }
                else if (sum > storloctransDetail.Qty)
                {
                    DataProvider.RollbackTransaction();

                    WebInfoPublish.Publish(this, "��������������������", this.languageComponent1);
                    return;
                }


                //StorageDetail storageDetail = _WarehouseFacade.GetStorageDetail(tCartonNo) as StorageDetail;
                //if (storageDetail != null)
                //{
                //    storageDetail.AvailableQty += qty;
                //    storageDetail.StorageQty += qty;
                //    _WarehouseFacade.UpdateStorageDetail(storageDetail);
                //}
                //else
                //{
                //    object[] _storloctransDetailList = _WarehouseFacade.GetStorloctransdetailcarton(transNo, mcode);
                //    StorloctransDetailCarton storloctransDetailCarton = _storloctransDetailList[0] as StorloctransDetailCarton;
                //    StorloctransDetail _storloctransDetail = (StorloctransDetail)_WarehouseFacade.GetStorloctransdetail(transNo, storloctransDetailCarton.MCode);
                //    StorageDetail stordetail = this._InventoryFacade.CreateNewStorageDetail();
                //    stordetail.AvailableQty = qty;
                //    stordetail.CartonNo = tCartonNo;
                //    stordetail.CDate = mDate;
                //    stordetail.CTime = mTime;
                //    stordetail.CUser = mUser;
                //    stordetail.DQMCode = _storloctransDetail.DqmCode;
                //    stordetail.FacCode = storloctransDetailCarton.FacCode;
                //    stordetail.FreezeQty = 0;
                //    stordetail.LastStorageAgeDate = mDate;
                //    stordetail.LocationCode = locationCode;
                //    stordetail.Lotno = storloctransDetailCarton.Lotno;
                //    stordetail.MaintainDate = mDate;
                //    stordetail.MaintainTime = mTime;
                //    stordetail.MaintainUser = mUser;
                //    stordetail.MCode = storloctransDetailCarton.MCode;
                //    stordetail.MDesc = _storloctransDetail.MDesc;
                //    stordetail.ProductionDate = storloctransDetailCarton.Production_Date;
                //    stordetail.ReworkApplyUser = string.Empty;
                //    stordetail.StorageAgeDate = storloctransDetailCarton.StorageageDate;
                //    stordetail.StorageCode = _Storloctrans.StorageCode;
                //    stordetail.StorageQty = 0;
                //    stordetail.SupplierLotNo = storloctransDetailCarton.Supplier_lotno;
                //    stordetail.Unit = _storloctransDetail.Unit;
                //    stordetail.ValidStartDate = storloctransDetailCarton.ValidStartDate;
                //    this._InventoryFacade.AddStorageDetail(stordetail);
                //}

                this.DataProvider.CommitTransaction();
                WebInfoPublish.Publish(this, "�ϼܳɹ�", this.languageComponent1);
                this.gridHelper.RefreshData();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.Publish(this, "�ϼ�ʧ�ܣ�" + ex.Message, this.languageComponent1);
            }
        }


        #region Object <--> Page

        protected bool ValidateCommit()
        {
            PageCheckManager manager = new PageCheckManager();

            manager.Add(new LengthCheck(this.lblTransNoQuery, this.drpTransNoQuery, 40, true));
            manager.Add(new LengthCheck(this.lblOriginalCartonEdit, this.txtFromCartonNo, 40, true));
            manager.Add(new LengthCheck(this.lblSNEdit, this.txtSNEdit, 40, false));
            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            return true;
        }
        protected bool ValidateShelves()
        {
            PageCheckManager manager = new PageCheckManager();
            manager.Add(new LengthCheck(this.lblTransNoQuery, this.drpTransNoQuery, 40, true));
            manager.Add(new LengthCheck(this.lblLocationCode, this.txtLocationCode, 40, true));
            manager.Add(new LengthCheck(this.lblTLocationCartonEdit, this.txtTLocationCartonEdit, 40, true));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            return true;
        }

        protected override bool ValidateInput()
        {
            PageCheckManager manager = new PageCheckManager();

            manager.Add(new LengthCheck(this.lblTransNoQuery, this.drpTransNoQuery, 40, true));
            manager.Add(new LengthCheck(this.lblLocationCode, this.txtLocationCode, 40, true));
            manager.Add(new LengthCheck(this.lblOriginalCartonEdit, this.txtFromCartonNo, 40, true));
            manager.Add(new LengthCheck(this.lblSNEdit, this.txtSNEdit, 40, false));
            manager.Add(new LengthCheck(this.lblTLocationCartonEdit, this.txtTLocationCartonEdit, 40, true));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            return true;
        }

        #endregion

        protected void cmdSAPBack_ServerClick(object sender, System.EventArgs e)
        {

            #region add by sam ��Ϣ����
            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(this.DataProvider);
            }
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(this.DataProvider);
            }

            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
            string mUser = this.GetUserCode();
            int mDate = dbDateTime.DBDate;
            int mTime = dbDateTime.DBTime;
            string transNo = this.drpTransNoQuery.SelectedValue.Trim().ToUpper();

            #endregion

            #region add by sam
            Storloctrans _storloctrans = (Storloctrans)_WarehouseFacade.GetStorloctrans(transNo);
            if (_storloctrans == null)
            {
                WebInfoPublish.Publish(this, "�޴�ת������Ϣ", this.languageComponent1);
                return;
            }
            if (_storloctrans.Status == "Close")
            {
                WebInfoPublish.Publish(this, "ת�����Ѿ����", this.languageComponent1);
                return;
            }

            StorloctransDetailCarton[] cartons = _WarehouseFacade.GetStorloctransdetailcartons(transNo);

            if (cartons.Length == 0)
            {
                WebInfoPublish.Publish(this, transNo + "��ת����Ϣ��", this.languageComponent1);
                return;
            }
            foreach (StorloctransDetailCarton cartonno in cartons)
            {
                if (string.IsNullOrEmpty(cartonno.LocationCode.Trim()))
                {
                    WebInfoPublish.Publish(this, transNo + "��Ŀ���λ���붼��ֵ��", this.languageComponent1);
                    return;
                }
                if (string.IsNullOrEmpty(cartonno.Cartonno.Trim()))
                {
                    WebInfoPublish.Publish(this, transNo + "��Ŀ����ű��붼��ֵ��", this.languageComponent1);
                    return;
                }
                if (string.IsNullOrEmpty(cartonno.Fromcartonno.Trim()))
                {
                    WebInfoPublish.Publish(this, transNo + "��Դ��λ���붼��ֵ��", this.languageComponent1);
                    return;
                }
                if (string.IsNullOrEmpty(cartonno.Fromcartonno.Trim()))
                {
                    WebInfoPublish.Publish(this, transNo + "��Դ��ű��붼��ֵ��", this.languageComponent1);
                    return;
                }
            }
            foreach (string MCODE in _WarehouseFacade.GetStorloctransdetailcartonMCODE(transNo))
            {
                decimal sum = 0;
                object[] _objs = _WarehouseFacade.GetStorloctransdetailcarton(transNo, MCODE);
                foreach (StorloctransDetailCarton storloctransDetailCarton in _objs)
                {
                    sum += storloctransDetailCarton.Qty;
                }

                StorloctransDetail storloctransDetail = _WarehouseFacade.GetStorloctransdetail(transNo, MCODE) as StorloctransDetail;
                if (storloctransDetail == null)
                {
                    DataProvider.RollbackTransaction();

                    WebInfoPublish.Publish(this, "ת������ϸ��Ϣ�����ڣ�", this.languageComponent1);
                    return;
                }
                if (sum == storloctransDetail.Qty)
                {
                    storloctransDetail.Status = "Close";
                    storloctransDetail.MaintainUser = mUser;
                    storloctransDetail.MaintainDate = mDate;
                    storloctransDetail.MaintainTime = mTime;
                    _WarehouseFacade.UpdateStorloctransdetail(storloctransDetail);
                }
                else if (sum > storloctransDetail.Qty)
                {
                    DataProvider.RollbackTransaction();

                    WebInfoPublish.Publish(this, "��������������������", this.languageComponent1);
                    return;
                }
            }




            bool isTrue = true;
            object[] objs_StorLocTransDetail = this._WarehouseFacade.GetStorloctransdetailExCludeCancel(transNo);
            if (objs_StorLocTransDetail != null)
            {
                foreach (StorloctransDetail _storloctransDetail in objs_StorLocTransDetail)
                {

                    if (!_storloctransDetail.Status.Equals("Close"))
                    {
                        isTrue = false;
                        break;
                    }
                }
            }
            #endregion


            //4��	����SAP PGI�ӿڣ�������档
            //5��	����SAP GR�ӿڣ�������档�ӿ����ƣ�ZCHN_MM_STO_INBOUND_OUTBOUND  
            #region add by sam  �ӿ�


            //if (_storloctrans.Status == "Close")
            if (isTrue)
            {
                BenQGuru.eMES.Material.StorLocTransDN[] ds = _WarehouseFacade.GeStorLocTransForDN(transNo);
                List<string> batchCodes = new List<string>();
                foreach (BenQGuru.eMES.Material.StorLocTransDN i in ds)
                {
                    if (!batchCodes.Contains(i.BatchCode))
                    {
                        batchCodes.Add(i.BatchCode);
                    }
                }
                foreach (string batchCode in batchCodes)
                {
                    #region

                    BenQGuru.eMES.Material.InvoicesDetailEx[] ins =
                        _WarehouseFacade.GetInVoicesDetailsForNotDN(batchCode);

                    if (ins == null || ins.Length <= 0)
                        throw new SAPException(batchCode + "�������Ŷ�Ӧ��SAP���ݺ�Ϊ�գ�");
                    Dictionary<string, decimal> dicIn = new Dictionary<string, decimal>();
                    Dictionary<string, decimal> dicOut = new Dictionary<string, decimal>();
                    Dictionary<string, decimal> ddd = new Dictionary<string, decimal>();
                    foreach (BenQGuru.eMES.Material.StorLocTransDN i in ds)
                    {
                        if (ddd.ContainsKey(i.DQMCODE))
                        {
                            ddd[i.DQMCODE] += i.QTY;
                        }
                        else
                        {
                            ddd.Add(i.DQMCODE, i.QTY);
                        }
                    }


                    foreach (string key in ddd.Keys)
                    {
                        dicIn.Add(key, ddd[key]);
                        dicOut.Add(key, ddd[key]);
                    }
                    UBToSAPForOut(dicOut, batchCode, ins);
                    UBToSAPForIn(dicIn, batchCode, ins);

                    #endregion
                }
            }
            else
            {
                WebInfoPublish.Publish(this, "ת����δ���", this.languageComponent1);
                return;
            }

            #endregion

            #region




            try
            {
                this.DataProvider.BeginTransaction();

                object objStorloctrans = this._WarehouseFacade.GetStorloctrans(transNo);
                Storloctrans storloctrans = objStorloctrans as Storloctrans;
                _storloctrans.Status = "Close";
                _storloctrans.MaintainUser = mUser;
                _storloctrans.MaintainDate = mDate;
                _storloctrans.MaintainTime = mTime;
                this._WarehouseFacade.UpdateStorloctrans(_storloctrans);

                #region ����Sn

                //1��	����ת������TransNo����TBLStorLocTransDetailSN����SN��Ϣ��Ȼ��ɾ����TBLStorageDetailSN����TBLStorLocTransDetailSN���TBLStorageDetailSNд��һ�����ݣ�
                object[] objs_StorLocTransDetailSN = _WarehouseFacade.GetStorloctransdetailsn(transNo);
                if (objs_StorLocTransDetailSN != null)
                {
                    #region

                    foreach (StorloctransDetailSN storloctransDetailSN in objs_StorLocTransDetailSN)
                    {
                        StorageDetailSN objStorageDetailSN =
                            (StorageDetailSN)_InventoryFacade.GetStorageDetailSN(storloctransDetailSN.Sn);
                        if (objStorageDetailSN != null)
                        {
                            this._InventoryFacade.DeleteStorageDetailSN(objStorageDetailSN);
                        }

                        StorageDetailSN _storageDetailSN = new StorageDetailSN();
                        _storageDetailSN.CartonNo = storloctransDetailSN.Cartonno;
                        _storageDetailSN.SN = storloctransDetailSN.Sn;
                        _storageDetailSN.PickBlock = "N";
                        _storageDetailSN.CUser = mUser;
                        _storageDetailSN.CDate = mDate;
                        _storageDetailSN.CTime = mTime;
                        _storageDetailSN.MaintainDate = mDate;
                        _storageDetailSN.MaintainTime = mTime;
                        _storageDetailSN.MaintainUser = mUser;
                        this._InventoryFacade.AddStorageDetailSN(_storageDetailSN);
                    }

                    #endregion
                }


                #endregion

                #region add by sam �����
                //2��	����ת������TransNo����TBLStorLocTransDetailCarton����ԭ���FromCARTONNO��Ϣ������ԭ��Ų��ұ�TBLStorageDetail��Ϣ�������²�����
                DataTable dt = this._WarehouseFacade.GetFromCartonNoAndSQTY(transNo);

                #region Storagedetaillog

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StorageDetail stod = (StorageDetail)_InventoryFacade.GetStorageDetail(dt.Rows[i][0].ToString());

                    if (stod != null)
                    {
                        //������ǣ����±�TBLStorageDetail��TBLStorageDetail. FreezeQTY= TBLStorageDetail. FreezeQTY-sum��TBLStorLocTransDetailCarton.QTY��;TBLStorageDetail. Storageqty= TBLStorageDetail. Storageqty- sum��TBLStorLocTransDetailCarton.QTY��;
                        stod.FreezeQty -= int.Parse(dt.Rows[i][1].ToString());
                        stod.StorageQty -= int.Parse(dt.Rows[i][1].ToString());

                        stod.MaintainUser = mUser;
                        stod.MaintainDate = mDate;
                        stod.MaintainTime = mTime;
                        this._InventoryFacade.UpdateStorageDetail(stod);

                        if (stod.StorageQty == 0)
                            this._InventoryFacade.DeleteStorageDetail(stod);

                        Storagedetaillog log = this._WarehouseFacade.CreateNewStoragedetaillog();
                        log.AvailableQty = stod.AvailableQty;
                        log.Cartonno = stod.CartonNo;
                        log.DqmCode = stod.DQMCode;
                        log.FacCode = stod.FacCode;
                        log.FreezeQty = stod.FreezeQty;
                        log.LaststorageageDate = stod.LastStorageAgeDate;
                        log.LocationCode = stod.LocationCode;
                        log.Lotno = stod.Lotno;
                        log.MaintainDate = mDate;
                        log.MaintainTime = mTime;
                        log.MaintainUser = mUser;
                        log.MCode = stod.MCode;
                        log.MDesc = stod.MDesc;
                        log.Production_Date = stod.ProductionDate;
                        log.Qty = decimal.Parse(dt.Rows[i][1].ToString());
                        log.ReworkapplyUser = stod.ReworkApplyUser;
                        log.Serial = 0;
                        log.Serialno = transNo;
                        log.StorageageDate = stod.StorageAgeDate;
                        log.StorageCode = stod.StorageCode;
                        log.StorageQty = stod.StorageQty;
                        log.Supplier_lotno = stod.SupplierLotNo;
                        log.Type = "TRA";
                        log.Unit = stod.Unit;
                        log.ValidStartDate = stod.ValidStartDate;
                        this._WarehouseFacade.AddStoragedetaillog(log);
                    }




                }

                #endregion


                #endregion

                #region �ӿ��
                dt = _WarehouseFacade.GetToCartonNoAndSQTY(transNo);



                #region �ӿ��
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StorageDetail stod = (StorageDetail)_InventoryFacade.GetStorageDetail(dt.Rows[i][0].ToString());
                    if (stod != null)
                    {

                        stod.StorageQty += int.Parse(dt.Rows[i][1].ToString());
                        stod.AvailableQty += int.Parse(dt.Rows[i][1].ToString());
                        stod.MaintainUser = mUser;
                        stod.MaintainDate = mDate;
                        stod.MaintainTime = mTime;
                        this._InventoryFacade.UpdateStorageDetail(stod);
                    }
                    else
                    {
                        object[] objs_StorLocTransDetailCarton = _WarehouseFacade.GetStorloctransdetailcartons(transNo, dt.Rows[i][0].ToString());
                        StorageDetail stordetail = this._InventoryFacade.CreateNewStorageDetail();
                        foreach (StorloctransDetailCarton storloctransDetailCarton in objs_StorLocTransDetailCarton)
                        {

                            if (!string.IsNullOrEmpty((stordetail.DQMCode)) && storloctransDetailCarton.DqmCode != stordetail.DQMCode)
                            {
                                this.DataProvider.RollbackTransaction();
                                WebInfoPublish.Publish(this, "Ŀ�����" + dt.Rows[i][0].ToString() + "��������ͬ������", this.languageComponent1);
                                return;
                            }
                            stordetail.AvailableQty = stordetail.AvailableQty + int.Parse(storloctransDetailCarton.Qty.ToString("G0"));
                            stordetail.CartonNo = storloctransDetailCarton.Cartonno;
                            if (string.IsNullOrEmpty(stordetail.DQMCode))
                                stordetail.DQMCode = storloctransDetailCarton.DqmCode;
                            if (string.IsNullOrEmpty(stordetail.FacCode))
                                stordetail.FacCode = storloctransDetailCarton.FacCode;

                            stordetail.FreezeQty = 0;
                            if (string.IsNullOrEmpty(stordetail.LocationCode))
                                stordetail.LocationCode = storloctransDetailCarton.LocationCode;
                            if (string.IsNullOrEmpty(stordetail.Lotno))
                                stordetail.Lotno = storloctransDetailCarton.Lotno;
                            if (string.IsNullOrEmpty(stordetail.MCode))
                                stordetail.MCode = storloctransDetailCarton.MCode;
                            if (string.IsNullOrEmpty(stordetail.MDesc))
                                stordetail.MDesc = storloctransDetailCarton.MDesc;
                            if (stordetail.ProductionDate == 0 || stordetail.ProductionDate > storloctransDetailCarton.Production_Date)
                                stordetail.ProductionDate = storloctransDetailCarton.Production_Date;

                            if (stordetail.StorageAgeDate == 0 || stordetail.StorageAgeDate > storloctransDetailCarton.StorageageDate)
                                stordetail.StorageAgeDate = storloctransDetailCarton.StorageageDate;

                            stordetail.StorageQty = stordetail.StorageQty + (int)storloctransDetailCarton.Qty;
                            if (string.IsNullOrEmpty(stordetail.SupplierLotNo))
                                stordetail.SupplierLotNo = storloctransDetailCarton.Supplier_lotno;
                            if (string.IsNullOrEmpty(stordetail.Unit))
                                stordetail.Unit = storloctransDetailCarton.Unit;

                            stordetail.ValidStartDate = storloctransDetailCarton.ValidStartDate;

                        }
                        if (string.IsNullOrEmpty(stordetail.StorageCode))
                            stordetail.StorageCode = _storloctrans.StorageCode;
                        stordetail.ReworkApplyUser = string.Empty;
                        stordetail.CDate = mDate;
                        stordetail.CTime = mTime;
                        stordetail.CUser = mUser;
                        stordetail.LastStorageAgeDate = mDate;

                        stordetail.MaintainUser = mUser;
                        stordetail.MaintainDate = mDate;
                        stordetail.MaintainTime = mTime;

                        this._InventoryFacade.AddStorageDetail(stordetail);
                    }

                }



                #endregion

                #endregion

                #region
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StorageDetail storageDetail = (StorageDetail)_InventoryFacade.GetStorageDetail(dt.Rows[i][0].ToString());//From���

                    //3��	����TBLStorageInfo������TBLStorLocTrans���ԭ��λ��Ŀ���λ+mocode����ѯ��TBLStorageDetail�������ͣ����±�TBLStorageInfo�еĿ��������
                    int _sum = _WarehouseFacade.GetStorageDetailsForQty(storageDetail.MCode,
                                                                                             _storloctrans
                                                                                                 .FromstorageCode);

                    StorageInfo storageInfo = (StorageInfo)_WarehouseFacade.GetStorageinfo(storageDetail.MCode,
                                                                                 _storloctrans.FromstorageCode);
                    if (storageInfo != null)
                    {

                        storageInfo.Storageqty = _sum;
                        storageInfo.Muser = mUser;
                        storageInfo.Mdate = mDate;
                        storageInfo.Mtime = mTime;
                        this._WarehouseFacade.UpdateStorageinfo(storageInfo);
                    }
                    else
                    {
                        StorageInfo newstorageInfo = new StorageInfo();
                        newstorageInfo.StorageCode = _storloctrans.FromstorageCode;
                        newstorageInfo.Mcode = storageDetail.MCode;
                        newstorageInfo.Storageqty = _sum;
                        newstorageInfo.Muser = mUser;
                        newstorageInfo.Mdate = mDate;
                        newstorageInfo.Mtime = mTime;
                        this._WarehouseFacade.AddStorageinfo(newstorageInfo);
                    }

                    #region from
                    _sum = _WarehouseFacade.GetStorageDetailsForQty(storageDetail.MCode,
                                                                             _storloctrans.StorageCode);


                    StorageInfo storageInfoFrom = (StorageInfo)_WarehouseFacade.GetStorageinfo(storageDetail.MCode,
                                                                          _storloctrans.StorageCode);
                    if (storageInfoFrom != null)
                    {
                        storageInfoFrom.Storageqty = _sum;
                        storageInfoFrom.Muser = mUser;
                        storageInfoFrom.Mdate = mDate;
                        storageInfoFrom.Mtime = mTime;
                        this._WarehouseFacade.UpdateStorageinfo(storageInfoFrom);
                    }
                    else
                    {
                        StorageInfo newstorageInfo = new StorageInfo();
                        newstorageInfo.StorageCode = _storloctrans.StorageCode;
                        newstorageInfo.Mcode = storageDetail.MCode;
                        newstorageInfo.Storageqty = _sum;
                        newstorageInfo.Muser = mUser;
                        newstorageInfo.Mdate = mDate;
                        newstorageInfo.Mtime = mTime;
                        this._WarehouseFacade.AddStorageinfo(newstorageInfo);
                    }
                    #endregion
                }
                #endregion


                this.DataProvider.CommitTransaction();
                WebInfoPublish.Publish(this, "�ύ�ɹ�", this.languageComponent1);
                this.gridHelper.RefreshData();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.Publish(this, "�ύʧ�ܣ�" + ex.Message, this.languageComponent1);
            }

            #endregion


        }



        #region UB out in add  by sam 2016��4��19��
        private void UBToSAPForOut(Dictionary<string, decimal> ddd, string batchCode, BenQGuru.eMES.Material.InvoicesDetailEx[] ins)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new BenQGuru.eMES.Material.WarehouseFacade(base.DataProvider);
            }
            if (inventoryFacade == null)
            {
                inventoryFacade = new BenQGuru.eMES.Material.InventoryFacade(base.DataProvider);
            }
            Dictionary<string, List<BenQGuru.eMES.SAPRFCService.Domain.UB>> UBGroupByInvNo = new Dictionary<string, List<BenQGuru.eMES.SAPRFCService.Domain.UB>>();

            foreach (BenQGuru.eMES.Material.InvoicesDetailEx inv in ins)
            {
                BenQGuru.eMES.SAPRFCService.Domain.UB ub = new BenQGuru.eMES.SAPRFCService.Domain.UB();
                if (ddd.ContainsKey(inv.DQMCode))
                {
                    ddd[inv.DQMCode] -= inv.PlanQty;

                    if (ddd[inv.DQMCode] >= 0)
                        ub.Qty = inv.PlanQty;
                    else
                        ub.Qty = ddd[inv.DQMCode];

                    ub.UBNO = inv.InvNo;

                    ub.Unit = inv.Unit;
                    ub.UBLine = inv.InvLine;

                    ub.InOutFlag = "351";
                    ub.MCode = inv.MCode;
                    ub.ContactUser = " ";
                    ub.DocumentDate = FormatHelper.TODateInt(DateTime.Now);
                    ub.FacCode = inv.FacCode;
                    ub.MesTransNO = batchCode;
                    ub.StorageCode = inv.FromStorageCode;
                }
                else
                {
                    throw new SAPException(batchCode + "����������������" + inv.InvNo + "�����Ϻ�" + inv.DQMCode);
                }

                if (UBGroupByInvNo.ContainsKey(inv.InvNo))
                {
                    UBGroupByInvNo[inv.InvNo].Add(ub);
                }
                else
                {
                    UBGroupByInvNo[inv.InvNo] = new List<BenQGuru.eMES.SAPRFCService.Domain.UB>();
                    UBGroupByInvNo[inv.InvNo].Add(ub);
                }
            }
            #region add by sam

            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            bool is2Sap = _WarehouseFacade.GetRecordCount(dbTime.DBDate, dbTime.DBTime) > 0;

            #endregion
            foreach (string invo in UBGroupByInvNo.Keys)
            {
                if (UBGroupByInvNo[invo].Count > 0)
                {


                    if (is2Sap)
                    {
                        LogUB2Sap(UBGroupByInvNo[invo]);
                    }
                    else
                    {
                        #region SAP
                        BenQGuru.eMES.SAPRFCService.Domain.SAPRfcReturn r = SendUBToSAP(UBGroupByInvNo[invo]);
                        List<BenQGuru.eMES.SAPRFCService.Domain.DN> dns =
                            new List<BenQGuru.eMES.SAPRFCService.Domain.DN>();

                        foreach (BenQGuru.eMES.SAPRFCService.Domain.UB ub in UBGroupByInvNo[invo])
                        {
                            BenQGuru.eMES.SAPRFCService.Domain.DN dn = new BenQGuru.eMES.SAPRFCService.Domain.DN();
                            dn.BatchNO = ub.MesTransNO;
                            dn.DNLine = ub.UBLine;
                            dn.DNNO = ub.UBNO;
                            dn.Qty = ub.Qty;
                            dn.Unit = ub.Unit;
                            dns.Add(dn);
                        }
                        LogDN(dns, r, "UB");
                        if (r == null)
                        {
                            throw new SAPException(invo + "SAP��д���ؿ�:");
                        }
                        if (string.IsNullOrEmpty(r.Result))
                        {
                            throw new SAPException(invo + "SAP��дʧ�� ֵ:" + r.Result + ":" + r.Message);
                        }
                        if (r.Result.ToUpper().Trim() != "S")
                        {
                            throw new SAPException(invo + "SAP��дʧ�� ֵ:" + r.Result + r.Message);
                        }
                    }
                        #endregion
                }
                else
                {
                    throw new SAPException(invo + "���Ű����Ļ�д����Ϊ�գ�");
                }

            }

        }


        private void UBToSAPForIn(Dictionary<string, decimal> ddd, string batchCode, BenQGuru.eMES.Material.InvoicesDetailEx[] ins)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new BenQGuru.eMES.Material.WarehouseFacade(base.DataProvider);
            }
            if (inventoryFacade == null)
            {
                inventoryFacade = new BenQGuru.eMES.Material.InventoryFacade(base.DataProvider);
            }
            #region add by sam

            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            bool is2Sap = _WarehouseFacade.GetRecordCount(dbTime.DBDate, dbTime.DBTime) > 0;

            #endregion

            Dictionary<string, List<BenQGuru.eMES.SAPRFCService.Domain.UB>> UBGroupByInvNo = new Dictionary<string, List<BenQGuru.eMES.SAPRFCService.Domain.UB>>();

            foreach (BenQGuru.eMES.Material.InvoicesDetailEx inv in ins)
            {
                BenQGuru.eMES.SAPRFCService.Domain.UB ub = new BenQGuru.eMES.SAPRFCService.Domain.UB();
                if (ddd.ContainsKey(inv.DQMCode))
                {
                    ddd[inv.DQMCode] -= inv.PlanQty;

                    if (ddd[inv.DQMCode] >= 0)
                        ub.Qty = inv.PlanQty;
                    else
                        ub.Qty = ddd[inv.DQMCode];

                    ub.UBNO = inv.InvNo;

                    ub.Unit = inv.Unit;
                    ub.UBLine = inv.InvLine;

                    ub.InOutFlag = "101";
                    ub.MCode = inv.MCode;
                    ub.ContactUser = " ";
                    ub.DocumentDate = FormatHelper.TODateInt(DateTime.Now);
                    ub.FacCode = inv.FacCode;
                    ub.MesTransNO = batchCode;
                    ub.StorageCode = inv.StorageCode;
                }
                else
                {
                    throw new SAPException(batchCode + "����������������" + inv.InvNo + "�����Ϻ�" + inv.DQMCode);
                }

                if (UBGroupByInvNo.ContainsKey(inv.InvNo))
                {
                    UBGroupByInvNo[inv.InvNo].Add(ub);
                }
                else
                {
                    UBGroupByInvNo[inv.InvNo] = new List<BenQGuru.eMES.SAPRFCService.Domain.UB>();
                    UBGroupByInvNo[inv.InvNo].Add(ub);
                }
            }

            foreach (string invo in UBGroupByInvNo.Keys)
            {
                if (UBGroupByInvNo[invo].Count > 0)
                {
                    if (is2Sap)
                    {
                        LogUB2Sap(UBGroupByInvNo[invo]);
                    }
                    else
                    {
                        #region SAP
                        BenQGuru.eMES.SAPRFCService.Domain.SAPRfcReturn r = SendUBToSAP(UBGroupByInvNo[invo]);
                        List<BenQGuru.eMES.SAPRFCService.Domain.DN> dns = new List<BenQGuru.eMES.SAPRFCService.Domain.DN>();

                        foreach (BenQGuru.eMES.SAPRFCService.Domain.UB ub in UBGroupByInvNo[invo])
                        {
                            BenQGuru.eMES.SAPRFCService.Domain.DN dn = new BenQGuru.eMES.SAPRFCService.Domain.DN();
                            dn.BatchNO = ub.MesTransNO;
                            dn.DNLine = ub.UBLine;
                            dn.DNNO = ub.UBNO;
                            dn.Qty = ub.Qty;
                            dn.Unit = ub.Unit;
                            dns.Add(dn);
                        }
                        LogDN(dns, r, "UB");
                        if (r == null)
                        {
                            throw new SAPException(invo + "SAP��д���ؿ�:");
                        }
                        if (string.IsNullOrEmpty(r.Result))
                        {
                            throw new SAPException(invo + "SAP��дʧ�� ֵ:" + r.Result + ":" + r.Message);
                        }
                        if (r.Result.ToUpper().Trim() != "S")
                        {
                            throw new SAPException(invo + "SAP��дʧ�� ֵ:" + r.Result + r.Message);
                        }

                        #endregion
                    }
                }
                else
                {
                    throw new SAPException(invo + "���Ű����Ļ�д����Ϊ�գ�");
                }
            }

        }

        #endregion


        #region add by sam SAP
        private BenQGuru.eMES.SAPRFCService.Domain.SAPRfcReturn SendUBToSAP(List<BenQGuru.eMES.SAPRFCService.Domain.UB> ubs)
        {
            BenQGuru.eMES.SAPRFCService.Domain.SAPRfcReturn r = null;
            BenQGuru.eMES.SAPRFCService.UBToSAP uToS = new BenQGuru.eMES.SAPRFCService.UBToSAP(this.DataProvider);

            try
            {

                r = uToS.PostUBToSAP(ubs);
            }
            catch (Exception ex)
            {
                throw new SAPException(ex.Message);
            }

            return r;

        }

        private void LogDN(List<BenQGuru.eMES.SAPRFCService.Domain.DN> dns, BenQGuru.eMES.SAPRFCService.Domain.SAPRfcReturn ret, string isAll)
        {
            foreach (BenQGuru.eMES.SAPRFCService.Domain.DN dn in dns)
            {

                DNLOG log = new DNLOG();
                log.DNLINE = dn.DNLine;
                log.DNNO = dn.DNNO;
                log.ISALL = isAll;
                if (ret == null)
                    log.RESULT = "empty";
                else
                    log.RESULT = ret.Result;


                log.MDATE = FormatHelper.TODateInt(DateTime.Now);
                log.MTIME = FormatHelper.TOTimeInt(DateTime.Now);
                log.MUSER = GetUserCode();
                log.Qty = dn.Qty;
                log.Unit = dn.Unit;
                log.MESSAGE = ret != null ? ret.Message : "null";
                if (ret != null && string.IsNullOrEmpty(ret.Message))
                {
                    log.MESSAGE = "empty";
                }
                log.DNBATCHNO = dn.BatchNO;
                _WarehouseFacade.InsertDNLOG(log);
            }
        }

        #endregion



        private void LogUB2Sap(List<BenQGuru.eMES.SAPRFCService.Domain.UB> dns)
        {
            foreach (BenQGuru.eMES.SAPRFCService.Domain.UB ub in dns)
            {

                Ub2Sap ubLog = new Ub2Sap();
                ubLog.Qty = ub.Qty;
                ubLog.UBNO = ub.UBNO;
                ubLog.Unit = ub.Unit;
                ubLog.UBLine = ub.UBLine;
                ubLog.InOutFlag = ub.InOutFlag;
                ubLog.MCode = ub.MCode;
                ubLog.ContactUser = ub.ContactUser;
                ubLog.DocumentDate = ub.DocumentDate;
                ubLog.FacCode = ub.FacCode;
                ubLog.MesTransNO = ub.MesTransNO;
                ubLog.StorageCode = ub.StorageCode;
                ubLog.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
                ubLog.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                ubLog.MaintainUser = GetUserCode();
                inventoryFacade.AddUb2Sap(ubLog);
            }
        }


        protected void rdoSelectType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.rdoSelectType.SelectedValue == "����")
            {
                this.txtQTY.Enabled = false;
                this.txtSNEdit.Enabled = false;
                this.txtQTY.Text = string.Empty;
                this.txtSNEdit.Text = string.Empty;
            }
            else
            {
                this.txtQTY.Enabled = true;
                this.txtSNEdit.Enabled = true;
            }
        }
        #endregion



        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                ((StorLocTransOperations)obj).DqmCode,
                ((StorLocTransOperations)obj).Qty.ToString(),
                ((StorLocTransOperations)obj).HasReMoveQty.ToString(),
                ((StorLocTransOperations)obj).Unit,
                ((StorLocTransOperations)obj).FromstorageCode,
                ((StorLocTransOperations)obj).StorageCode,
                ((StorLocTransOperations)obj).CUser,
                ((StorLocTransOperations)obj).CTime.ToString()
            };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	
                "DQMCode",
                "RequireQty",
                "HasRePickQty",
                "MUOM",
                "FromStorage",
                "ToStorage",
                "TransUser",
                "TransTime"
            };
        }

        #endregion

        #region add by sam
        protected override void cmdQuery_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(FormatHelper.CleanString(drpTransNoQuery.SelectedValue)))
            {
                WebInfoPublish.Publish(this, "ת�����Ų���Ϊ��", this.languageComponent1);
                return;
            }
            this.gridHelper.RequestData();
            if (this.gridHelper2 != null)
                this.gridHelper2.RequestData();
            this.buttonHelper.PageActionStatusHandle(PageActionType.Query);
        }
        #endregion
    }
}
