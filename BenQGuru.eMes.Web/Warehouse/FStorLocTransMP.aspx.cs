using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using System.Collections.Generic;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FStorLocTransMP : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private WarehouseFacade _WarehouseFacade = null;
        SystemSettingFacade _SystemSettingFacade = null;
        private InventoryFacade facade = null;
        private UserFacade _UserFacade = null;

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
                InitStorageList();

                string transNo = Request.QueryString["TRANSNO"];
                if (!string.IsNullOrEmpty(transNo)) txtTransNoQuery.Text = transNo;
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
            if (facade == null)
            {
                facade = new InventoryFacade(base.DataProvider);
            }
            this.drpFStorageQuery.Items.Add(new ListItem("", ""));
            this.drpTStorageQuery.Items.Add(new ListItem("", ""));
            object[] objStorage = facade.GetAllStorage();
            if (objStorage != null && objStorage.Length > 0)
            {
                foreach (Storage storage in objStorage)
                {

                    this.drpFStorageQuery.Items.Add(new ListItem(
                         storage.StorageName, storage.StorageCode)
                        );
                    this.drpTStorageQuery.Items.Add(new ListItem(
                         storage.StorageName, storage.StorageCode)
                        );
                }
            }
            this.drpFStorageQuery.SelectedIndex = 0;
            this.drpTStorageQuery.SelectedIndex = 0;
        }

        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("TransNo", "ת������", null);
            this.gridHelper.AddColumn("InvNo", "SAP���ݺ�", null);
            this.gridHelper.AddColumn("Status", "״̬", null);
            this.gridHelper.AddColumn("FromStorage", "Դ��λ", null);
            this.gridHelper.AddColumn("ToStorage", "Ŀ���λ", null);
            this.gridHelper.AddColumn("CDate", "��������", null);
            this.gridHelper.AddColumn("CTime", "����ʱ��", null);
            this.gridHelper.AddColumn("CUser", "������", null);
            this.gridHelper.AddLinkColumn("Detail", "��ϸ��Ϣ", null);

            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);


            if (!string.IsNullOrEmpty(txtTransNoQuery.Text))
            {
                this.gridHelper.RequestData();
            }
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();

            row["TransNo"] = ((Storloctrans)obj).Transno;
            row["InvNo"] = ((Storloctrans)obj).Invno;
            row["Status"] = languageComponent1.GetString(((Storloctrans)obj).Status);
            row["FromStorage"] = ((Storloctrans)obj).FromstorageCode;
            row["ToStorage"] = ((Storloctrans)obj).StorageCode;
            row["CDate"] = FormatHelper.ToDateString(((Storloctrans)obj).CDate);
            row["CTime"] = FormatHelper.ToTimeString(((Storloctrans)obj).CTime);
            row["CUser"] = ((Storloctrans)obj).CUser;

            return row;

        }



        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }

            return this._WarehouseFacade.QueryStorloctrans(
           FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtTransNoQuery.Text)),
           this.drpFStorageQuery.SelectedValue,
           this.drpTStorageQuery.SelectedValue,
           FormatHelper.TODateInt(this.txtCDateFromQuery.Text),
           FormatHelper.TODateInt(this.txtCDateToQuery.Text),
           FormatHelper.CleanString(this.txtInvNoQuery.Text), TransType.TransType_Transfer,
           inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            return this._WarehouseFacade.QueryStorloctransCount(
            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtTransNoQuery.Text)),
           this.drpFStorageQuery.SelectedValue,
           this.drpTStorageQuery.SelectedValue,
           FormatHelper.TODateInt(this.txtCDateFromQuery.Text),
           FormatHelper.TODateInt(this.txtCDateToQuery.Text),
            FormatHelper.CleanString(this.txtInvNoQuery.Text), TransType.TransType_Transfer
                  );
        }

        #endregion



        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                                ((Storloctrans)obj).Transno,
                                     languageComponent1.GetString(((Storloctrans)obj).Status),
                                ((Storloctrans)obj).StorageCode,
                                ((Storloctrans)obj).FromstorageCode,
                                FormatHelper.ToDateString(((Storloctrans)obj).CDate),
                                FormatHelper.ToTimeString(((Storloctrans)obj).CTime),
                                ((Storloctrans)obj).CUser
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[]
                {
                                    "TransNo",
                                    "Status", 
                                    "FromStorage",
                                    "ToStorage",
                                    "CDate",
                                    "CTime",
                                    "CUser"                                    
                                   
                };
        }

        #endregion



        protected override void gridWebGrid_ItemCommand(GridRecord row, string commandName)
        {
            if (commandName == "Detail")
            {
                string transNo = row.Items.FindItemByKey("TransNo").Text.Trim();
                Response.Redirect(this.MakeRedirectUrl("FStorLocTransDetailMP.aspx", new string[] { "TRANSNO" }, new string[] { transNo }));
            }
        }

        protected override void Grid_ClickCell(GridRecord row, string command)
        {
            base.Grid_ClickCell(row, command);
            if (command == "Detail")
            {
                string transNo = row.Items.FindItemByKey("TransNo").Text.Trim();
                Response.Redirect(this.MakeRedirectUrl("FStorLocTransDetailMP.aspx", new string[] { "TRANSNO" }, new string[] { transNo }));
            }
        }
        #region Button

        //�·�
        protected void cmdRelease_ServerClick(object sender, EventArgs e)
        {
            GetServerClick("Release");
        }

        //ȡ���·�
        protected void cmdInitial_ServerClick(object sender, EventArgs e)
        {
            GetServerClick("ReleaseCancel");
        }

        protected override object GetEditObject(GridRecord row)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            object obj = _WarehouseFacade.GetStorloctrans(row.Items.FindItemByKey("TransNo").Text);

            if (obj != null)
            {
                return obj;
            }

            return null;
        }

        #region GetServerClick

        private void GetServerClick(string clickName)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            ArrayList array = this.gridHelper.GetCheckedRows();
            object obj = null;
            if (array.Count > 0)
            {
                //�ж��Ƿ��������
                foreach (GridRecord row in array)
                {
                    obj = this.GetEditObject(row);

                    if (obj != null)
                    {
                        Storloctrans storloctrans = obj as Storloctrans;
                        if (clickName == "Release")
                        {
                            if (storloctrans.Status != StorageTrans_STATUS.Trans_Release)
                            {
                                WebInfoPublish.Publish(this, storloctrans.Transno + "�Ǵ�ת��״̬�������·�", this.languageComponent1);
                                return;
                            }
                            if (string.IsNullOrEmpty(storloctrans.StorageCode))
                            {
                                WebInfoPublish.Publish(this, storloctrans.Transno + "ת������Ŀ���λ����Ϊ��", this.languageComponent1);
                                return;
                            }
                            if (string.IsNullOrEmpty(storloctrans.FromstorageCode))
                            {
                                WebInfoPublish.Publish(this, storloctrans.Transno + "ת������ԭ��λ����Ϊ��", this.languageComponent1);
                                return;
                            }

                            if (storloctrans.Status=="Cancel")
                            {
                                WebInfoPublish.Publish(this, storloctrans.Transno + "��ȡ�������·���", this.languageComponent1);
                                return;
                            }
                        }
                        else if (clickName == "ReleaseCancel")
                        {
                            if (storloctrans.Status != StorageTrans_STATUS.Trans_Pick)
                            {
                                WebInfoPublish.Publish(this, storloctrans.Transno + "�Ǽ���״̬������ȡ���·�", this.languageComponent1);
                                return;
                            }
                            object[] objDetail = _WarehouseFacade.QueryDetailBYNo(storloctrans.Transno);
                            bool isAllRelease = true;
                            if (objDetail != null)
                            {

                                foreach (StorloctransDetail sdetail in objDetail)
                                {
                                    if (sdetail.Status != StorageTrans_STATUS.Trans_Release)//�ж���ϸ�Ƿ�ȫ�ǳ�ʼ״̬
                                    {
                                        isAllRelease = false;
                                    }
                                }
                            }
                            if (!isAllRelease)
                            {
                                WebInfoPublish.Publish(this, storloctrans.Transno + " ��ϸ��ȫ�Ǵ�ת��״̬������ȡ���·�", this.languageComponent1);
                                return;
                            }
                        }
                    }
                }

                List<Storloctrans> objList = new List<Storloctrans>();
                foreach (GridRecord row in array)
                {
                    obj = this.GetEditObject(row);

                    if (obj != null)
                    {
                        objList.Add((Storloctrans)obj);
                    }
                }

                this.UpdateTrans(objList, clickName);

                this.gridHelper.RequestData();
                this.buttonHelper.PageActionStatusHandle(PageActionType.Add);
            }
        }
        #endregion

        #region ����״̬

        protected void UpdateTrans(List<Storloctrans> objList, string type)
        {
            //�·���ť�������TBLStorLocTrans. Status��Release:��ʼ��״̬��ת��������ΪPick:����״̬
            //ȡ���·���ť�������TBLStorLocTrans. Status��Pick:����״̬��ת��������ΪRelease:��ʼ��״̬��
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            DBDateTime dbDateTime = new DBDateTime();
            try
            {
                this.DataProvider.BeginTransaction();
                foreach (Storloctrans storloctrans in objList)
                {
                    if (type == "Release")
                    {
                        storloctrans.Status = StorageTrans_STATUS.Trans_Pick;
                        storloctrans.MaintainDate = dbDateTime.DBDate;
                        storloctrans.MaintainTime = dbDateTime.DBTime;
                        storloctrans.MaintainUser = this.GetUserCode();
                    }
                    else if (type == "ReleaseCancel")
                    {
                        storloctrans.Status = StorageTrans_STATUS.Trans_Release;
                        storloctrans.MaintainDate = dbDateTime.DBDate;
                        storloctrans.MaintainTime = dbDateTime.DBTime;
                        storloctrans.MaintainUser = this.GetUserCode();
                    }
                    _WarehouseFacade.UpdateStorloctrans(storloctrans);
                }
                this.DataProvider.CommitTransaction();
                if (type == "Release")
                {
                    WebInfoPublish.Publish(this, "�·����", this.languageComponent1);
                }
                else if (type == "ReleaseCancel")
                {
                    WebInfoPublish.Publish(this, "ȡ���·��ɹ�", this.languageComponent1);
                }
            }
            catch (Exception ex)
            {
                WebInfoPublish.Publish(this, ex.Message, this.languageComponent1);
                this.DataProvider.RollbackTransaction();
            }


        }

        # endregion

        #endregion

    }
}
