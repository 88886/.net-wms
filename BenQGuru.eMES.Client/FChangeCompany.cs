using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Client.Service;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using UserControl;
using Infragistics.Win.UltraWinGrid;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Package;

namespace BenQGuru.eMES.Client
{
    public partial class FChangeCompany : Form
    {
        private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
        private DataTable m_dtInfo;

        private object[] m_CompanyList;

        private const string packStack = "0";
        private const string packPallet = "1";
        private const string packRcard = "2";

        private IDomainDataProvider DataProvider
        {
            get
            {
                return _domainDataProvider;
            }
        }
        
        public FChangeCompany()
        {
            InitializeComponent();
        }

        private void FChangeCompany_Load(object sender, EventArgs e)
        {
            UserControl.UIStyleBuilder.FormUI(this);
            UserControl.UIStyleBuilder.GridUI(this.gridInfo);
            
            this.InitializeCompany();
            this.InitialDataTable();
            this.txtRecordNum.Value = "0";
            this.txtRecordNum.InnerTextBox.TextAlign = HorizontalAlignment.Right;
            this.opsetPackObject.Value = packStack;

        }

        private void InitialDataTable()
        {
            this.m_dtInfo = new DataTable();
            this.m_dtInfo.Columns.Add("originalcompany");
            this.m_dtInfo.Columns.Add("stackcode");
            this.m_dtInfo.Columns.Add("palletcode");
            this.m_dtInfo.Columns.Add("rcard");
            this.m_dtInfo.Columns.Add("itemcode");
            this.m_dtInfo.Columns.Add("itemdesc");

            this.gridInfo.DataSource = m_dtInfo;
        }


        private void InitializeCompany()
        {
            SystemSettingFacade systemFacade = new SystemSettingFacade(this.DataProvider);
            object[] objList = systemFacade.GetParametersByParameterGroup("COMPANYLIST");
            if (objList != null)
            {
                this.cboCompany.Clear();
                foreach (Parameter para in objList)
                {
                    this.cboCompany.AddItem(para.ParameterDescription, para.ParameterAlias);
                }

                m_CompanyList = objList;
            }
        }

        private void opsetPackObject_ValueChanged(object sender, EventArgs e)
        {
            if (opsetPackObject.Value == packStack)
            {
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Normal, "$CS_PLEASE_INPUT_STACK"));
                this.txtInput.TextFocus(true, true);
            }

            if (opsetPackObject.Value == packPallet)
            {
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Normal, "$CS_Please_Input_PALLET"));
                this.txtInput.TextFocus(true, true);
            }

            if (opsetPackObject.Value == packRcard)
            {
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Normal, "$CS_Please_Input_RunningCard"));
                this.txtInput.TextFocus(true, true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.m_dtInfo.Clear();
            this.txtRecordNum.Value = "0";
            this.txtInput.TextFocus(true, true);
        }

        private void gridInfo_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // ����Ӧ�п�
            e.Layout.AutoFitColumns = false;
            //e.Layout.Override.AllowColSizing = AllowColSizing.None;

            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;

            // ����
            e.Layout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;

            e.Layout.Bands[0].Columns["originalcompany"].Width = 100;
            e.Layout.Bands[0].Columns["stackcode"].Width = 150;
            e.Layout.Bands[0].Columns["palletcode"].Width = 150;
            e.Layout.Bands[0].Columns["rcard"].Width = 150;
            e.Layout.Bands[0].Columns["itemcode"].Width = 150;
            e.Layout.Bands[0].Columns["itemdesc"].Width = 150;

            // ��λ����
            e.Layout.Bands[0].Columns["originalcompany"].Header.Caption = "ԭ��˾";
            e.Layout.Bands[0].Columns["stackcode"].Header.Caption = "��λ";
            e.Layout.Bands[0].Columns["palletcode"].Header.Caption = "ջ��";
            e.Layout.Bands[0].Columns["rcard"].Header.Caption = "���к�";
            e.Layout.Bands[0].Columns["itemcode"].Header.Caption = "��Ʒ";
            e.Layout.Bands[0].Columns["itemdesc"].Header.Caption = "��Ʒ����";

            // ������λ�Ƿ�����༭������λ����ʾ��ʽ
            e.Layout.Bands[0].Columns["originalcompany"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["stackcode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["palletcode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["rcard"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["itemcode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["itemdesc"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
        }

        private void txtInput_TxtboxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (this.txtInput.Value.Trim().Length ==0)
                {
                    this.txtInput.TextFocus(true, true);
                    return;
                }

                if (this.cboCompany.SelectedItemText.Trim().Length == 0)
                {
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_PLEASE_INPUT_TAR_COMPANY"));
                    this.cboCompany.Focus();
                    return;
                }

                switch (this.opsetPackObject.CheckedItem.DataValue.ToString())
                {
                    case packStack:
                        ////�����λ
                        //
                        if (!this.InputStack(this.txtInput.Value.Trim(), this.cboCompany.SelectedItemValue.ToString()))
                        {
                            this.txtInput.TextFocus(false, true);
                            return;
                        }
                        break;
                    case packPallet:
                        ////����ջ��
                        //
                        if (!this.InputPallet(this.txtInput.Value.Trim(), this.cboCompany.SelectedItemValue.ToString()))
                        {
                            this.txtInput.TextFocus(false, true);
                            return;
                        }
                        break;
                    case packRcard:
                        ////�������к�
                        //
                        if (!this.InputRcard(this.txtInput.Value.Trim(), this.cboCompany.SelectedItemValue.ToString()))
                        {
                            this.txtInput.TextFocus(false, true);
                            return;
                        }
                        break;
                    default:
                        break;
                }

                //Message:���������˾�ɹ�
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Success, "$CS_CHANGE_COMPANY_SUCCESS"));

                if (opsetPackObject.Value == packStack)
                {
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Normal, "$CS_PLEASE_INPUT_STACK"));
                    
                }

                if (opsetPackObject.Value == packPallet)
                {
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Normal, "$CS_Please_Input_PALLET"));
                }

                if (opsetPackObject.Value == packRcard)
                {
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Normal, "$CS_Please_Input_RunningCard"));
                }
                this.txtInput.TextFocus(true, true);
            }
        }

        private bool InputStack(string stackCode,string companyCode)
        {
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
            
            ////���˶�λ�Ƿ������ز�Ʒ
            //
            object[] objRcardToStackPalletList = inventoryFacade.GetRcardToStackPallet(stackCode, "", "");

            if (objRcardToStackPalletList == null)
            {
                //Message:��λ��������ز�Ʒ
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_RCARD_NOT_EXIST_IN_STACK"));
                return false;
            }

            ////���Company
            //
            inventoryFacade.UpdateCompany(stackCode, "", "", companyCode, ApplicationService.Current().LoginInfo.UserCode);

            ////Add To Grid
            //
            this.LoadDataSource(objRcardToStackPalletList);

            return true;
        }

        private bool InputPallet(string palletCode, string companyCode)
        {
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);

            ////���˶�λ�Ƿ������ز�Ʒ
            //
            object[] objRcardToStackPalletList = inventoryFacade.GetRcardToStackPallet("",palletCode,"");

            if (objRcardToStackPalletList == null)
            {
                //Message:ջ�岻������ز�Ʒ
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_RCARD_NOT_EXIST_IN_PALLET"));
                return false;
            }

            ////���Company
            //
            inventoryFacade.UpdateCompany("", palletCode, "", companyCode, ApplicationService.Current().LoginInfo.UserCode);

            ////Add To Grid
            //
            this.LoadDataSource(objRcardToStackPalletList);

            return true;
        }

        private bool InputRcard(string rcard, string companyCode)
        {
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);

            //ת����Ʒ���к�Ϊԭʼ���к�
            DataCollect.DataCollectFacade dcf = new BenQGuru.eMES.DataCollect.DataCollectFacade(DataProvider);
            string sourceRCard = dcf.GetSourceCard(rcard.Trim().ToUpper(), string.Empty);

            ////���˶�λ�Ƿ������ز�Ʒ
            //
            object[] objRcardToStackPalletList = inventoryFacade.GetRcardToStackPallet("", "", sourceRCard);

            if (objRcardToStackPalletList == null)
            {
                //Message:��λ��������ز�Ʒ
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_RCARD_NOT_EXIST_IN_STACK"));
                return false;
            }

            PackageFacade pf = new PackageFacade(this.DataProvider);
            object objPalletRcrad = pf.GetPallet2RCardByRCard(sourceRCard);
            if (objPalletRcrad != null)
            {
                //���кŴ���ջ�壬����ջ��������й�˾���
                ApplicationRun.GetInfoForm().Add(
                new UserControl.Message(MessageType.Error, "$CS_RCARD_EXIST_PALLET_NOT_CHANGE_COMPANY"));
                return false;
            }

            ////���Company
            //
            inventoryFacade.UpdateCompany("", "", sourceRCard, companyCode, ApplicationService.Current().LoginInfo.UserCode);

            ////Add To Grid
            //
            this.LoadDataSource(objRcardToStackPalletList);

            return true;
        }

        private void LoadDataSource(object[] rcardToStackPalletList)
        {
            foreach (RcardToStackPallet rcardToStackPallet in rcardToStackPalletList)
            {
                DataRow dr = this.m_dtInfo.NewRow();
                dr["originalcompany"] = GetCompanyDesc(rcardToStackPallet.Company);
                dr["stackcode"] = rcardToStackPallet.StackCode;
                dr["palletcode"] = rcardToStackPallet.PalletCode;
                dr["rcard"] = rcardToStackPallet.SerialNo;
                dr["itemcode"] = rcardToStackPallet.ItemCode;
                dr["itemdesc"] = rcardToStackPallet.ItemDescription;

                this.m_dtInfo.Rows.Add(dr);
                this.txtRecordNum.Value = Convert.ToString(Convert.ToInt32(this.txtRecordNum.Value) + 1);
            }

        }

        private string GetCompanyDesc(string companyCode)
        {
            foreach (Parameter objPara in this.m_CompanyList)
            {
                if (objPara.ParameterAlias.Equals(companyCode))
                {
                    return objPara.ParameterDescription;
                }
            }

            return "";
        }
    }
}