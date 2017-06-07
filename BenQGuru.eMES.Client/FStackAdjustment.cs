using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Client.Service;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Domain.OQC;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Package;
using BenQGuru.eMES.OQC;
using UserControl;
using Infragistics.Win.UltraWinGrid;

namespace BenQGuru.eMES.Client
{
    public partial class FStackAdjustment : BaseForm
    {
        private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
        private DataTable m_dtInfo;

        private object[] m_CompanyList;

        private const string packPallet = "0";
        private const string packRcard = "1";
        private const string packStack = "2";

        private IDomainDataProvider DataProvider
        {
            get
            {
                return _domainDataProvider;
            }
        }

        private string m_SelectedStorage;
        /// <summary>
        /// ����û�ѡ���Ŀ�λ
        /// </summary>
        public string SelectedStorage
        {
            get { return m_SelectedStorage; }
            set { m_SelectedStorage = value; }
        }

        private string m_SelectedStack;
        /// <summary>
        /// ����û�ѡ���Ķ�λ
        /// </summary>
        public string SelectedStack
        {
            get { return m_SelectedStack; }
            set { m_SelectedStack = value; }
        }

        public FStackAdjustment()
        {
            InitializeComponent();
        }

        private void InitializeCompany()
        {
            SystemSettingFacade systemFacade = new SystemSettingFacade(this.DataProvider);
            object[] objList = systemFacade.GetParametersByParameterGroup("COMPANYLIST");
            if (objList != null)
            {
                //this.cboCompany.Clear();
                //foreach (Parameter para in objList)
                //{
                //    this.cboCompany.AddItem(para.ParameterDescription, para.ParameterAlias);
                //}

                m_CompanyList = objList;
            }
        }

        private void InitializeStorageCode()
        {
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
            object[] storageList = inventoryFacade.GetAllStorage();

            if (storageList != null)
            {
                this.ucLabelComboxINVType.Clear();
                foreach (Storage storage in storageList)
                {
                    this.ucLabelComboxINVType.AddItem(storage.StorageName, storage.StorageCode);
                }
            }
        }

        private void FStackAdjustment_Load(object sender, EventArgs e)
        {
            UserControl.UIStyleBuilder.FormUI(this);
            UserControl.UIStyleBuilder.GridUI(this.gridInfo);

            this.InitializeStorageCode();
            this.InitializeCompany();

            this.InitialDataTable();
            this.txtRecordNum.Value = "0";
            this.txtRecordNum.InnerTextBox.TextAlign = HorizontalAlignment.Right;

            this.packObject.Value = packPallet;
            //this.InitPageLanguage();
        }

        private void InitialDataTable()
        {
            this.m_dtInfo = new DataTable();
            this.m_dtInfo.Columns.Add("ostackcode");
            this.m_dtInfo.Columns.Add("opalletcode");
            this.m_dtInfo.Columns.Add("tstackcode");
            this.m_dtInfo.Columns.Add("tpalletcode");
            this.m_dtInfo.Columns.Add("rcard");            
            this.m_dtInfo.Columns.Add("company");
            this.m_dtInfo.Columns.Add("itemcode");
            this.m_dtInfo.Columns.Add("itemdesc");

            this.gridInfo.DataSource = m_dtInfo;
        }

        private void btnGetStack_Click(object sender, EventArgs e)
        {
            if (this.ucLabelComboxINVType.SelectedItemText.Trim().Length == 0)
            {
                ApplicationRun.GetInfoForm().Add(
                   new UserControl.Message(MessageType.Error, "$CS_STORAGE_NOT_INPUT"));
                return;
            }

            FStackInfo objForm = new FStackInfo();
            objForm.StackInfoEvent += new ParentChildRelateEventHandler<ParentChildRelateEventArgs<System.Collections.Hashtable>>(objForm_StackInfoEvent);
            objForm.StorageCode = this.ucLabelComboxINVType.SelectedItemValue.ToString();
            objForm.StackCode = this.ucLabelEditStock.Value.ToString();
            objForm.ShowDialog();


        }

        void objForm_StackInfoEvent(object sender, ParentChildRelateEventArgs<System.Collections.Hashtable> e)
        {
            this.SelectedStack = e.CustomObject["stackcode"].ToString();
            this.SelectedStorage = e.CustomObject["storagecode"].ToString();

            //Check�Ӷ�λʹ��״����ҳ��������Ķ�λ�Ϳ�λ
            if (this.CheckSelecetedStackAndStorage(this.SelectedStorage, this.SelectedStack, Convert.ToString(this.ucLabelComboxINVType.SelectedItemValue)))
            {
                this.ucLabelEditStock.Value = e.CustomObject["stackcode"].ToString();
            }

            //this.ucLabelEditStock.Value = e.CustomObject["stackcode"].ToString();
        }

        /// <summary>
        /// Check �Ӷ�λʹ��״����ҳ��������Ķ�λ�Ϳ�λ
        /// </summary>
        /// <returns>true/false</returns>
        private bool CheckSelecetedStackAndStorage(string selectedStorage, string selectedStack, string originalStorage)
        {
            if (originalStorage.Trim().Length == 0)
            {
                //�������λ
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_STORAGE_NOT_INPUT"));
                return false;
            }

            if (!selectedStorage.Equals(originalStorage))
            {
                InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
                int stack2RCardCount = inventoryFacade.GetStack2RCardCount(selectedStack, originalStorage);

                if (stack2RCardCount > 0)
                {
                    //��λ�Ϳ�𲻶�Ӧ
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_STACK_STORAGE_NOT_SAME"));
                    return false;
                }
                else
                {
                    //��λ�Ϳ�𲻶�Ӧ,ȷ��ʹ�øö�λ?

                    if (MessageBox.Show(UserControl.MutiLanguages.ParserMessage("$CS_STACK_STORAGE_NOT_SAME_CONFIRM"), MutiLanguages.ParserString("$ShowConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        private void gridInfo_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // ����Ӧ�п�
            e.Layout.AutoFitColumns = false;
            //e.Layout.Override.AllowColSizing = AllowColSizing.None;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;

            e.Layout.ScrollBounds = ScrollBounds.ScrollToFill;

            // ����Grid��Split���ڸ�������������Ϊ1--������Split
            e.Layout.MaxColScrollRegions = 1;
            e.Layout.MaxRowScrollRegions = 1;

            // ����
            e.Layout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;

            e.Layout.Bands[0].Columns["ostackcode"].Header.Caption = "ԭ��λ";
            e.Layout.Bands[0].Columns["ostackcode"].Width = 100;
            e.Layout.Bands[0].Columns["ostackcode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            e.Layout.Bands[0].Columns["opalletcode"].Header.Caption = "ԭջ��";
            e.Layout.Bands[0].Columns["opalletcode"].Width = 150;
            e.Layout.Bands[0].Columns["opalletcode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            e.Layout.Bands[0].Columns["tstackcode"].Header.Caption = "Ŀ���λ";
            e.Layout.Bands[0].Columns["tstackcode"].Width = 100;
            e.Layout.Bands[0].Columns["tstackcode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            e.Layout.Bands[0].Columns["tpalletcode"].Header.Caption = "Ŀ��ջ��";
            e.Layout.Bands[0].Columns["tpalletcode"].Width = 150;
            e.Layout.Bands[0].Columns["tpalletcode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            e.Layout.Bands[0].Columns["rcard"].Header.Caption = "���к�";
            e.Layout.Bands[0].Columns["rcard"].Width = 150;
            e.Layout.Bands[0].Columns["rcard"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            
            e.Layout.Bands[0].Columns["company"].Header.Caption = "��˾��";
            e.Layout.Bands[0].Columns["company"].Width = 100;
            e.Layout.Bands[0].Columns["company"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            e.Layout.Bands[0].Columns["itemcode"].Header.Caption = "��Ʒ����";
            e.Layout.Bands[0].Columns["itemcode"].Width = 100;
            e.Layout.Bands[0].Columns["itemcode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            e.Layout.Bands[0].Columns["itemdesc"].Header.Caption = "��Ʒ����";
            e.Layout.Bands[0].Columns["itemdesc"].Width = 150;
            e.Layout.Bands[0].Columns["itemdesc"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            //this.InitGridLanguage(gridInfo);
        }

        private void opsetPackObject_ValueChanged(object sender, EventArgs e)
        {
            if (packObject.Value == packPallet)
            {
                this.ControlEnabled(true);
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Normal, "$CS_Please_Input_PALLET"));
                this.txtInput.TextFocus(true, true);
            }

            if (packObject.Value == packRcard)
            {
                this.ControlEnabled(true);
                if (this.rdoUseOPallet.Checked)
                {
                    packObject.Value = packPallet;
                    return;
                }

                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Normal, "$CS_Please_Input_RunningCard"));
                this.txtInput.TextFocus(true, true);
            }

            if (packObject.Value == packStack)
            {
                this.ControlEnabled(false);

                ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Normal, "$CS_PLEASE_INPUT_STACK"));
                this.txtInput.TextFocus(true, true);
            }
        }

        private void rdoUseOPallet_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoUseOPallet.Checked)
            {
                this.txtUseNPallet.ReadOnly = true;
                this.txtUseNPallet.Value = "";
                this.txtUseTPallet.ReadOnly = true;
                this.txtUseTPallet.Value = "";

                this.packObject.Value = packPallet;
            }

        }

        private void rdoUseTPallet_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoUseTPallet.Checked)
            {
                this.txtUseNPallet.ReadOnly = true;
                this.txtUseNPallet.Value = "";
                this.txtUseTPallet.ReadOnly = false;
                this.txtUseTPallet.Value = "";
                this.txtUseTPallet.TextFocus(false, true);
            }
        }

        private void rdoUseNPallet_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoUseNPallet.Checked)
            {
                this.txtUseNPallet.ReadOnly = false;
                this.txtUseNPallet.Value = "";
                this.txtUseTPallet.ReadOnly = true;
                this.txtUseTPallet.Value = "";
                this.txtUseNPallet.TextFocus(false, true);
            }
        }

        private bool CheckUI()
        {
            if (this.ucLabelComboxINVType.SelectedItemText.Trim().Length == 0)
            {
                //�������λ
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_STORAGE_NOT_INPUT"));
                this.ucLabelComboxINVType.Focus();
                return false;
            }

            if (this.ucLabelEditStock.Value.Trim().Length == 0)
            {
                //�������λ
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_PLEASE_INPUT_STACK"));
                this.ucLabelEditStock.TextFocus(false, true);
                return false;
            }

            if (!this.CheckSelecetedStackAndStorage(this.SelectedStorage, this.SelectedStack, Convert.ToString(this.ucLabelComboxINVType.SelectedItemValue)))
            {
                this.ucLabelEditStock.TextFocus(false, true);
                return false;
            }

            if (this.rdoUseTPallet.Checked && this.packObject.Value != packStack)
            {
                if (this.txtUseTPallet.Value.Trim().Length == 0)
                {
                    //������Ŀ���λջ���
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PLEASE_INPUT_TAR_PALLET_CODE"));
                    this.txtUseTPallet.TextFocus(false, true);
                    return false;
                }
            }

            if (rdoUseNPallet.Checked && this.packObject.Value != packStack)
            {
                if (this.txtUseNPallet.Value.Trim().Length == 0)
                {
                    //��������ջ���
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PLEASE_INPUT_NEW_PALLET_CODE"));
                    this.txtUseNPallet.TextFocus(false, true);
                    return false;
                }
            }

            return true;
        }

        private void txtInput_TxtboxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (this.txtInput.Value.Trim().Length == 0)
                {
                    this.txtInput.TextFocus(true, true);
                    return;
                }

                ////Check UI
                //
                if (!this.CheckUI())
                {
                    return;
                }


                switch (this.packObject.CheckedItem.DataValue.ToString())
                {
                    case packPallet:
                        ////����ջ�������
                        //                      
                        if (!this.InputPallet(this.txtInput.Value.Trim()))
                        {
                            this.txtInput.TextFocus(true, true);
                            return;
                        }
                        break;
                    case packRcard:
                        ////�������к�����
                        //
                        if (!this.InputRcard(this.txtInput.Value.Trim()))
                        {
                            this.txtInput.TextFocus(true, true);
                            return;
                        }
                        break;
                    case packStack:
                        if (!this.InputStack(this.txtInput.Value.Trim()))
                        {
                            this.txtInput.TextFocus(true, true);
                            return;
                        }
                        break;
                    default:
                        break;
                }

                //��ӳɹ�
                ApplicationRun.GetInfoForm().Add(
                new UserControl.Message(MessageType.Success, "$CS_Add_Success"));

                if (packObject.Value == packPallet)
                {
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Normal, "$CS_Please_Input_PALLET"));
                }

                if (packObject.Value == packRcard)
                {
                    if (this.rdoUseOPallet.Checked)
                    {
                        packObject.Value = packPallet;
                        this.txtInput.TextFocus(true, true);
                        return;
                    }

                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Normal, "$CS_Please_Input_RunningCard"));
                }

                this.txtInput.TextFocus(true, true);
            }
        }

        private bool InputPallet(string palletCode)
        {
            ////Check ջ���Ƿ����
            //
            PackageFacade objFacade = new PackageFacade(this.DataProvider);
            object pallet = objFacade.GetPallet(palletCode);

            if (pallet == null)
            {
                //Message:��ջ�岻����
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_PALLETNO_IS_NOT_EXIT"));
                return false;
            }


            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);

            ////ʹ��ԭջ��
            //
            if (this.rdoUseOPallet.Checked)
            {

                ////Check ջ����Ŀ���λ���Ѿ�����
                //
                object[] rcardToStackPallet = inventoryFacade.GetRcardToStackPallet(this.ucLabelEditStock.Value, palletCode, "");

                if (rcardToStackPallet != null)
                {
                    //Message:ջ����Ŀ���λ���Ѿ�����
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLET_IS_EXIST_IN_TAR_STACK"));
                    return false;
                }

                ////��ȡԴջ��Ķ�λ����
                //
                object[] rcardToStackPalletList = inventoryFacade.GetRcardToStackPallet("", palletCode, "");

                if (rcardToStackPalletList == null)
                {
                    //Message:Դջ���Ӧ�Ķ�λ��Ϣ������
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLET_IS_NOT_EXIST_IN_ORI_STACK"));
                    return false;
                }

                if (inventoryFacade.CheckStackIsOnlyAllowOneItem(ucLabelEditStock.Value.ToString()) && 
                    CheckStackItemError(this.ucLabelEditStock.Value.Trim(), this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                    ((RcardToStackPallet)rcardToStackPalletList[0]).ItemCode))
                {
                    //Message:Ŀ���λ�����Ϻ͵�ǰ���ϲ�һ��
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_STACK_ITEM_DIFF"));
                    return false;
                }

                ////����Դջ���Rcard����StackToRcard
                //
                inventoryFacade.UpdatePalletStackByWholePallet(rcardToStackPalletList,
                                                                this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                                                this.ucLabelEditStock.Value.Trim(),
                                                                ApplicationService.Current().LoginInfo.UserCode);

                //Load Grid
                this.LoadGrid(rcardToStackPalletList, this.ucLabelEditStock.Value.Trim(), palletCode);


            }

            ////ʹ��Ŀ���λջ��
            //
            if (this.rdoUseTPallet.Checked)
            {
                if (palletCode.Equals(this.txtUseTPallet.Value.Trim()))
                {
                    //Message:Դջ���Ŀ��ջ����ͬ
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLETNO_SAME_AS_TARPALLET"));
                    return false;
                }

                ////checkĿ���λջ���,��Ʒ,��˾��,��Ʒ����,�Ƿ�����кŵ�ջ��һ��
                //
                //��ȡԴջ����Ϣ
                object[] objOriStackToRcardList = inventoryFacade.GetStackToRcardInfoByPallet(palletCode);
                object[] objTarStackToRcardList = inventoryFacade.GetStackToRcardInfoByPallet(this.txtUseTPallet.Value.Trim());

                if (objOriStackToRcardList == null)
                {
                    //Message:Դջ���Ӧ�Ķ�λ��Ϣ������
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLET_IS_NOT_EXIST_IN_ORI_STACK"));
                    return false;

                }

                if (inventoryFacade.CheckStackIsOnlyAllowOneItem(ucLabelEditStock.Value.ToString()) && 
                    CheckStackItemError(this.ucLabelEditStock.Value.Trim(), this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                    ((StackToRcard)objOriStackToRcardList[0]).ItemCode))
                {
                    //Message:Ŀ���λ�����Ϻ͵�ǰ���ϲ�һ��
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_STACK_ITEM_DIFF"));
                    return false;
                }

                string strErrorMessage = string.Empty;
                StackToRcard objOri = (StackToRcard)objOriStackToRcardList[0];
                StackToRcard objTar = (StackToRcard)objTarStackToRcardList[0];
                if (objOri.ItemCode != objTar.ItemCode)
                {
                    strErrorMessage = strErrorMessage + "itemcode";
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_ITEM_NOT_SAME_IN_PALLET_TWO $CS_TARGET=" + objTar.ItemCode + " $CS_ORIGINAL=" + objOri.ItemCode));
                }

                if (objOri.Company != objTar.Company)
                {
                    strErrorMessage = strErrorMessage + "companycode";
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_COMPANY_NOT_SAME_IN_PALLET_TWO $CS_TARGET=" + objTar.Company + " $CS_ORIGINAL=" + objOri.Company));
                }                

                if (strErrorMessage != string.Empty)
                {
                    return false;
                }

                ////��ȡԴջ��Ķ�λ����
                //
                object[] rcardToStackPallet = inventoryFacade.GetRcardToStackPallet("", palletCode, "");

                ////��ԭջ�����ΪĿ��ջ��
                //
                inventoryFacade.UpdateOriPalletStackToTargetPallet(rcardToStackPallet,
                                                                    this.txtUseTPallet.Value.Trim(),
                                                                    this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                                                    this.ucLabelEditStock.Value.Trim(),
                                                                    ApplicationService.Current().LoginInfo.UserCode);

                //Load Grid
                this.LoadGrid(rcardToStackPallet, this.ucLabelEditStock.Value.Trim(), this.txtUseTPallet.Value.Trim());
            }

            ////ʹ����ջ��
            //
            if (this.rdoUseNPallet.Checked)
            {
                if (palletCode.Equals(this.txtUseNPallet.Value.Trim()))
                {
                    //Message:Դջ���Ŀ��ջ����ͬ
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLETNO_SAME_AS_TARPALLET"));
                    return false;
                }


                ////��ȡԴջ��Ķ�λ����
                //
                object[] rcardToStackPallet = inventoryFacade.GetRcardToStackPallet("", palletCode, "");

                if (rcardToStackPallet == null)
                {
                    //Message:Դջ���Ӧ�Ķ�λ��Ϣ������
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLET_IS_NOT_EXIST_IN_ORI_STACK"));
                    return false;
                }

                if (inventoryFacade.CheckStackIsOnlyAllowOneItem(ucLabelEditStock.Value.ToString()) && CheckStackItemError(this.ucLabelEditStock.Value.Trim(), this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                    ((RcardToStackPallet)rcardToStackPallet[0]).ItemCode))
                {
                    //Message:Ŀ���λ�����Ϻ͵�ǰ���ϲ�һ��
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_STACK_ITEM_DIFF"));
                    return false;
                }

                //���ջ�������Ƿ���
                if (!inventoryFacade.CheckStackCapacity(this.ucLabelComboxINVType.SelectedItemValue.ToString().Trim().ToUpper(),
                                                        this.ucLabelEditStock.Value.Trim().ToUpper()))
                {
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_STACK_CAPACITY_FULL"));
                    return false;
                }


                string lotNo = " ";
                DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
                SimulationReport simulationReport =(SimulationReport)dataCollectFacade.GetRcardFromSimulationReport(((RcardToStackPallet)rcardToStackPallet[0]).SerialNo);
                if (simulationReport!=null)
                {
                    OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
                    OQCLot2Card oqcLot2Card = (OQCLot2Card)oqcFacade.GetLastOQCLot2CardByRCard(simulationReport.RunningCard);

                    if (oqcLot2Card != null)
                    {
                        lotNo = oqcLot2Card.LOTNO;
                    } 
                }

                //��ԭջ�����Ϊ��ջ��
                inventoryFacade.UpdateOriPalletStackToNewPallet(rcardToStackPallet,
                                                                this.txtUseNPallet.Value.Trim(),
                                                                this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                                                this.ucLabelEditStock.Value.Trim(),
                                                                ApplicationService.Current().LoginInfo.UserCode,
                                                                ApplicationService.Current().ResourceCode,
                                                                lotNo);

                //Load Grid
                this.LoadGrid(rcardToStackPallet, this.ucLabelEditStock.Value.Trim(), this.txtUseNPallet.Value.Trim());
            }

            return true;
        }

        private bool InputRcard(string rcard)
        {

            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
            DataTable dtInfo = inventoryFacade.GetSimulationReportInfo(rcard, packRcard);

            if (dtInfo == null)
            {
                //�����кŲ�����
                ApplicationRun.GetInfoForm().Add(
                new UserControl.Message(MessageType.Error, "$CS_RCARD_IS_NOT_EXIT"));
                return false;
            }

            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
            string sourceRcard = dataCollectFacade.GetSourceCard(rcard.Trim().ToUpper(), string.Empty);

            ////ʹ��Ŀ���λջ��
            //
            if (this.rdoUseTPallet.Checked)
            {
                ////checkĿ���λջ���,��Ʒ,��˾��,��Ʒ����,�Ƿ�����кŵ�ջ��һ��
                //
                //��ȡԴջ����Ϣ
                object[] objOriRcardTOStackPalletList = inventoryFacade.GetRcardToStackPallet("", "", sourceRcard);
                object[] objTarStackToRcardList = inventoryFacade.GetStackToRcardInfoByPallet(this.txtUseTPallet.Value.Trim());

                if (objOriRcardTOStackPalletList == null)
                {
                    //Message:��Ʒ���кŲ����ڶ�λ��
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_RCARD_IS_NOT_EXIST_IN_ORI_STACK"));
                    return false;

                }

                ////CHeck ���кŶ�Ӧ��ջ���Ŀ��ջ���Ƿ���ͬ
                //
                if (((RcardToStackPallet)objOriRcardTOStackPalletList[0]).PalletCode.Equals(this.txtUseTPallet.Value.Trim()))
                {
                    //Message:Դջ���Ŀ��ջ����ͬ
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLETNO_SAME_AS_TARPALLET"));
                    return false;
                }

                if (inventoryFacade.CheckStackIsOnlyAllowOneItem(ucLabelEditStock.Value.ToString()) && CheckStackItemError(this.ucLabelEditStock.Value.Trim(), this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                    ((RcardToStackPallet)objOriRcardTOStackPalletList[0]).ItemCode))
                {
                    //Message:Ŀ���λ�����Ϻ͵�ǰ���ϲ�һ��
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_STACK_ITEM_DIFF"));
                    return false;
                }

                string strErrorMessage = string.Empty;
                RcardToStackPallet objOri = (RcardToStackPallet)objOriRcardTOStackPalletList[0];
                StackToRcard objTar = (StackToRcard)objTarStackToRcardList[0];
                if (objOri.ItemCode != objTar.ItemCode)
                {
                    strErrorMessage = strErrorMessage + "itemcode";
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_ITEM_NOT_SAME_IN_PALLET_TWO $CS_TARGET=" + objTar.ItemCode + " $CS_ORIGINAL=" + objOri.ItemCode));
                }

                if (objOri.Company != objTar.Company)
                {
                    strErrorMessage = strErrorMessage + "companycode";
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_COMPANY_NOT_SAME_IN_PALLET_TWO $CS_TARGET=" + objTar.Company + " $CS_ORIGINAL=" + objOri.Company));
                }                

                if (strErrorMessage != string.Empty)
                {
                    return false;
                }

                ////���¸ò�Ʒ���кŵ�Դջ��ΪĿ��ջ��
                //
                inventoryFacade.UpdateOriPalletStackToTargetPalletByRcard(objOri,
                                                                            this.txtUseTPallet.Value.Trim(),
                                                                            this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                                                            this.ucLabelEditStock.Value.Trim(),
                                                                             ApplicationService.Current().LoginInfo.UserCode);
                //Load Grid
                this.LoadGrid(objOriRcardTOStackPalletList, this.ucLabelEditStock.Value.Trim(), this.txtUseTPallet.Value.Trim());
            }

            ////ʹ����ջ��
            //
            if (this.rdoUseNPallet.Checked)
            {
                ////��ȡԴ���кŶ�Ӧ�Ķ�λ����
                //
                object[] rcardToStackPallet = inventoryFacade.GetRcardToStackPallet("", "", sourceRcard);

                if (rcardToStackPallet == null)
                {
                    //Message:��Ʒ���кŲ����ڶ�λ��
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_RCARD_IS_NOT_EXIST_IN_ORI_STACK"));
                    return false;
                }

                ////CHeck ���кŶ�Ӧ��ջ���Ŀ��ջ���Ƿ���ͬ
                //
                if (((RcardToStackPallet)rcardToStackPallet[0]).PalletCode.Equals(this.txtUseNPallet.Value.Trim()))
                {
                    //Message:���кŶ�Ӧ��ջ���Ŀ��ջ���Ƿ���ͬ
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLETNO_IN_RCARD_SAME_AS_TARPALLET"));
                    return false;
                }

                //��ȡԴջ����Ϣ
                object[] objOriRcardTOStackPalletList = inventoryFacade.GetRcardToStackPallet("", "", sourceRcard);

                if (objOriRcardTOStackPalletList == null)
                {
                    //Message:��Ʒ���кŲ����ڶ�λ��
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_RCARD_IS_NOT_EXIST_IN_ORI_STACK"));
                    return false;

                }

                if (inventoryFacade.CheckStackIsOnlyAllowOneItem(ucLabelEditStock.Value.ToString()) && 
                    CheckStackItemError(this.ucLabelEditStock.Value.Trim(), this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                    ((RcardToStackPallet)objOriRcardTOStackPalletList[0]).ItemCode))
                {
                    //Message:Ŀ���λ�����Ϻ͵�ǰ���ϲ�һ��
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_STACK_ITEM_DIFF"));
                    return false;
                }

                //���ջ�������Ƿ���
                if (!inventoryFacade.CheckStackCapacity(this.ucLabelComboxINVType.SelectedItemValue.ToString().Trim().ToUpper(),
                                                        this.ucLabelEditStock.Value.Trim().ToUpper()))
                {
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_STACK_CAPACITY_FULL"));
                    return false;
                }

                
                string lotNo = " ";
                OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
                OQCLot2Card oqcLot2Card =(OQCLot2Card) oqcFacade.GetLastOQCLot2CardByRCard(sourceRcard);

                if (oqcLot2Card!=null)
                {
                    lotNo = oqcLot2Card.LOTNO;
                }

                ////���¸ò�Ʒ���кŵ�Դջ��Ϊ��ջ��
                //
                inventoryFacade.UpdateOriPalletStackToNewPalletByRcard(rcardToStackPallet[0],
                                                                        this.txtUseNPallet.Value.Trim(),
                                                                        this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                                                        this.ucLabelEditStock.Value.Trim(),
                                                                        ApplicationService.Current().LoginInfo.UserCode,
                                                                        ApplicationService.Current().ResourceCode,
                                                                        lotNo);

                //Load Grid
                this.LoadGrid(rcardToStackPallet, this.ucLabelEditStock.Value.Trim(), this.txtUseNPallet.Value.Trim());
            }

            return true;
        }


        private bool InputStack(string stackCode)
        {
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);

            SStack stackObject = (SStack)inventoryFacade.GetSStack(stackCode.Trim().ToUpper());

            if (stackObject == null)
            {
                ApplicationRun.GetInfoForm().Add(
                 new UserControl.Message(MessageType.Error, "$CS_Stack_Is_Not_Exist"));
                return false;
            }

            object[] rcardToStackPallet = inventoryFacade.GetRcardToStackPallet(stackCode.Trim().ToUpper(), string.Empty, string.Empty);

            //ת�ƶ�λ������Ϣ
            object[] rcardToStackObjects = inventoryFacade.GetStackToRcardByStack(stackCode.Trim().ToUpper(), string.Empty);

            if (rcardToStackObjects == null)
            {
                //Message:ת�ƶ�λû������
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_STACK_Not_Have_Item"));
                return false;
            }

            if (inventoryFacade.CheckStackIsOnlyAllowOneItem(ucLabelEditStock.Value.ToString()) && 
                CheckStackItemError(this.ucLabelEditStock.Value.Trim(), this.ucLabelComboxINVType.SelectedItemValue.ToString(),
                                   ((RcardToStackPallet)rcardToStackPallet[0]).ItemCode))
            {
                //Message:Ŀ���λ�����Ϻ͵�ǰ���ϲ�һ��
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_STACK_ITEM_DIFF"));
                return false;
            }

            //����λ�����Ƿ���
            if (!inventoryFacade.CheckStackCapacity(this.ucLabelComboxINVType.SelectedItemValue.ToString().Trim().ToUpper(),
                                                    this.ucLabelEditStock.Value.Trim().ToUpper(), stackCode.Trim().ToUpper()))
            {
                ApplicationRun.GetInfoForm().Add(
                new UserControl.Message(MessageType.Error, "$CS_STACK_CAPACITY_FULL"));
                return false;
            }

            //���¶�λ
            inventoryFacade.UpdateStackToRcard(this.ucLabelComboxINVType.SelectedItemValue.ToString().Trim().ToUpper(),
                                                    this.ucLabelEditStock.Value.Trim().ToUpper(),
                                                    ((StackToRcard)rcardToStackObjects[0]).StorageCode,
                                                    stackCode.Trim().ToUpper(), ApplicationService.Current().UserCode);


            //Load Grid
            this.LoadGrid(rcardToStackPallet, this.ucLabelEditStock.Value.Trim(), this.txtUseNPallet.Value.Trim());

            return true;
        }



        private bool CheckStackItemError(string stackCode, string storageCode,string itemCode)
        {
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
            object[] stackToRcard = inventoryFacade.GetStackToRcardByStack(stackCode, storageCode);

            if (stackToRcard != null)
            {
                if (!((StackToRcard)stackToRcard[0]).ItemCode.Equals(itemCode))
                {
                    return true;
                }
            }

            return false;

        }

        private void LoadGrid(object[] rcardToStackPalletList, string tStackCode,string tPalletCode)
        {
            foreach (RcardToStackPallet obj in rcardToStackPalletList)
            {               
                DataRow dr = this.m_dtInfo.NewRow();
                dr["ostackcode"] = obj.StackCode;
                dr["opalletcode"] = obj.PalletCode;
                dr["tstackcode"] = tStackCode;
                if (packObject.Value == packStack)
                {
                    dr["tpalletcode"] = obj.PalletCode;
                }
                else
                {
                    dr["tpalletcode"] = tPalletCode;
                }                
               
                dr["rcard"] = obj.SerialNo;                
                dr["company"] = this.GetCompanyDesc(obj.Company);
                dr["itemcode"] = obj.ItemCode;
                dr["itemdesc"] = obj.ItemDescription;

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

        private void txtUseTPallet_Leave(object sender, EventArgs e)
        {
            if (this.txtUseTPallet.Value.Trim().Length != 0)
            {
                PackageFacade objFacade = new PackageFacade(this.DataProvider);
                object pallet = objFacade.GetPallet(this.txtUseTPallet.Value);

                if (pallet == null)
                {
                    //��ջ�岻����
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLETNO_IS_NOT_EXIT"));
                    txtUseTPallet.TextFocus(true, true);
                }

                InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
                object[] stackToRcardS = inventoryFacade.GetRcardToStackPallet(this.ucLabelEditStock.Value.Trim(), this.txtUseTPallet.Value, "");

                if (stackToRcardS == null)
                {
                    //��ջ�岻��Ŀ���λ�� ջ���
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLETNO_NOT_IN_TRA_STACK=" + this.txtUseTPallet.Value));
                    txtUseTPallet.TextFocus(true, true);
                }

            }
        }

        private void txtUseNPallet_Leave(object sender, EventArgs e)
        {
            if (this.txtUseNPallet.Value.Trim().Length != 0)
            {
                PackageFacade objFacade = new PackageFacade(this.DataProvider);
                object pallet = objFacade.GetPallet(this.txtUseNPallet.Value);

                if (pallet != null)
                {
                    //��ջ���Ѵ���
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLETNO_IS_EXIT"));
                    txtUseNPallet.TextFocus(true, true);
                }

                InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
                object[] stackToRcardS = inventoryFacade.GetRcardToStackPallet(this.ucLabelEditStock.Value.Trim(), this.txtUseNPallet.Value, "");

                if (stackToRcardS != null)
                {
                    //��ջ���Ѿ���Ŀ���λ�� ��λ=
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLETNO_IN_TRA_STACK=" + ((RcardToStackPallet)stackToRcardS[0]).StackCode));
                    txtUseNPallet.TextFocus(true, true);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ucLabelComboxINVType.SelectedIndex = -1;
            this.ucLabelEditStock.Value = "";
            this.SelectedStack = "";
            this.SelectedStorage = "";
            this.txtUseNPallet.Value = "";
            this.txtUseTPallet.Value = "";
            this.txtRecordNum.Value = "0";
            this.m_dtInfo.Clear();
            this.txtInput.Value = "";
            this.rdoUseOPallet.Checked = true;
            this.ucLabelComboxINVType.Focus();
        }

        private void ControlEnabled(bool isEnabled)
        {
            this.rdoUseOPallet.Enabled = isEnabled;
            this.rdoUseTPallet.Enabled = isEnabled;
            this.rdoUseNPallet.Enabled = isEnabled;
            this.txtUseTPallet.Enabled = isEnabled;
            this.txtUseNPallet.Enabled = isEnabled;
        }
    }
}