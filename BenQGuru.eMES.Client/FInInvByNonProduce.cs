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
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Domain.BaseSetting;
using UserControl;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Win.UltraWinGrid;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Package;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.Package;

namespace BenQGuru.eMES.Client
{
    public partial class FInInvByNonProduce : BaseForm
    {
        private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
        private DataTable m_dtPalletToRcard;
        private const string packAdd = "0";
        private const string packDelete = "2";


        private IDomainDataProvider DataProvider
        {
            get
            {
                return _domainDataProvider;
            }
        }


        public FInInvByNonProduce()
        {
            InitializeComponent();
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




        private void FInInvByNonProduce_Load(object sender, EventArgs e)
        {
            UserControl.UIStyleBuilder.FormUI(this);
            UserControl.UIStyleBuilder.GridUI(this.gridRcard);

            this.ultraOptionSetAddDelete.Value = packAdd;
            this.ucLabelEditQty.Value = "0";
            this.ucLabelEditQty.InnerTextBox.TextAlign = HorizontalAlignment.Right;
            this.txtPlanQty.InnerTextBox.TextAlign = HorizontalAlignment.Right;

            this.InitializeComboBox();
            this.InitialDataTable();           
            this.chbRcardLength.Checked = true;
            this.txtLength.Value = Convert.ToString(23);

            //this.InitPageLanguage();
        }

        private void InitialDataTable()
        {
            this.m_dtPalletToRcard = new DataTable();
            this.m_dtPalletToRcard.Columns.Add("palletcode");
            this.m_dtPalletToRcard.Columns.Add("rcard");

            this.gridRcard.DataSource = m_dtPalletToRcard;
        }

        /// <summary>
        /// ���UI�ؼ��Ƿ�����
        /// </summary>
        /// <returns>true/false</returns>
        private bool CheckUIInfo()
        {
            if (this.txtTransCode.Value.Trim().Length == 0)
            {
                //Message:�����뵥�ݺ���
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_TRANS_CODE_INPUT"));
                this.txtTransCode.TextFocus(false, true);
                return false;
            }

            if (this.cboBusinssCode.SelectedItemText.ToString().Trim().Length == 0)
            {
                //Message:��ѡ��ҵ������
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_BUSINESS_CODE_INPUT"));
                this.cboBusinssCode.Select();
                return false;
            }

            if (this.txtItemCode.Value.Trim().Length == 0)
            {
                //Message:�������Ʒ����
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_ITEM_CODE_INPUT"));
                this.txtItemCode.TextFocus(false, true);
                return false;
            }


            if (this.ucLabelComboxINVType.SelectedItemText.Trim().Length == 0 ||
                this.ucLabelComboxCompany.SelectedItemText.Trim().Length == 0 ||
                this.ucLabelEditStock.Value.Trim().Length == 0
                )
            {
                //Message:���ѶϢ�趨��ȫ
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_IN_STORAGE_INFO_ERROR"));
                return false;
            }

            if (this.txtPalletCode.Checked)
            {
                if (this.txtPalletCode.Value.Trim().Length == 0)
                {
                    //Message:���������ջ��
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PALLET_CODE_INPUT"));
                    this.txtPalletCode.TextFocus(false, true);
                    return false;
                }

                if (!CheckRcardIsInTheSameStack(this.txtPalletCode.Value, this.ucLabelEditStock.Value))
                {
                    return false;
                }

                //else
                //{
                //    PackageFacade objPFFacade = new PackageFacade(this.DataProvider);
                //    //�ж�ջ����Ƿ����
                //    object objPallet = objPFFacade.GetPallet(this.txtPalletCode.Value.Trim());
                //    if (objPallet == null)
                //    {
                //        //��ջ��Ų�����
                //        ApplicationRun.GetInfoForm().Add(
                //        new UserControl.Message(MessageType.Error, "$CS_PALLETNO_IS_NOT_EXIT"));
                //        return false;

                //    }
                //}
            }

            return true;
        }



        //����Rcard����ջ����Rcard��ͬһ��λ��
        private bool CheckRcardIsInTheSameStack(string pallet, string stackCode)
        {
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
            PackageFacade packageFacade = new PackageFacade(this.DataProvider);
            DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);

            object[] pallet2RCardList = packageFacade.GetPallet2RCardListByPallet(pallet);
            if (pallet2RCardList == null)
            {
                return true;
            }

            for (int i = 0; i < pallet2RCardList.Length; i++)
            {
                string cartonCode = string.Empty;
                SimulationReport simulationReport = (SimulationReport)dataCollectFacade.GetLastSimulationReport(((Pallet2RCard)pallet2RCardList[i]).RCard);
                if (simulationReport != null)
                {
                    cartonCode = simulationReport.CartonCode;
                }

                object[] stack2RcardList = inventoryFacade.QueryStacktoRcardByRcardAndCarton(((Pallet2RCard)pallet2RCardList[i]).RCard, cartonCode);
                if (stack2RcardList != null && !((StackToRcard)stack2RcardList[0]).StackCode.Equals(stackCode))
                {
                    ApplicationRun.GetInfoForm().Add(
                new UserControl.Message(MessageType.Error, "$CS_RCARD_IS_NOT_INSAME_STACK:" + ((StackToRcard)stack2RcardList[0]).StackCode));
                    return false;
                }
            }

            return true;
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

                    if (MessageBox.Show(UserControl.MutiLanguages.ParserMessage("$CS_STACK_STORAGE_NOT_SAME_CONFIRM"), UserControl.MutiLanguages.ParserMessage("$CS_CONFIRM"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        private void InitializeComboBox()
        {
            this.InitializeBusinessType();
            this.InitializeStorageCode();
            this.InitializeCompany();
        }

        private void InitializeBusinessType()
        {
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
            object[] objInvBusinessList = inventoryFacade.GetInvBusiness(BussinessReason.type_noneproduce, BussinessType.type_in);

            if (objInvBusinessList != null)
            {
                this.cboBusinssCode.Clear();

                foreach (InvBusiness invBusiness in objInvBusinessList)
                {
                    this.cboBusinssCode.AddItem(invBusiness.BusinessDescription, invBusiness.BusinessCode);
                }
            }

        }

        private void InitializeCompany()
        {
            SystemSettingFacade systemFacade = new SystemSettingFacade(this.DataProvider);
            object[] objList = systemFacade.GetParametersByParameterGroup("COMPANYLIST");
            if (objList != null)
            {
                this.ucLabelComboxCompany.Clear();
                foreach (Parameter para in objList)
                {
                    this.ucLabelComboxCompany.AddItem(para.ParameterDescription, para.ParameterAlias);
                }
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

            InventoryFacade inventoryFacade = new InventoryFacade(DataProvider);
            //���¶�λ״̬
            if (this.ucLabelEditStock.Value.Trim().Length > 0)
            {
                object objectStackMessage = inventoryFacade.GetStackMessage(this.ucLabelEditStock.Value.Trim().ToUpper());
                if (objectStackMessage != null)
                {
                    this.ucLabelEditstackMessage.Value = ((StackMessage)objectStackMessage).StackQtyMessage;
                }
                else
                {
                    SStack objectStack = (SStack)inventoryFacade.GetSStack(this.ucLabelEditStock.Value.Trim().ToUpper());
                    if (objectStack != null)
                    {
                        this.ucLabelEditstackMessage.Value = "0" + "/" + objectStack.Capacity;
                    }
                    else
                    {
                        this.ucLabelEditstackMessage.Value = string.Empty;
                    }
                }
            }

            //this.ucLabelEditStock.Value = e.CustomObject["stackcode"].ToString();
        }

        private void cmdItemSelect_Click(object sender, EventArgs e)
        {
            FProductSelect objForm = new FProductSelect();
            objForm.ProductInfoEvent += new ParentChildRelateEventHandler<ParentChildRelateEventArgs<System.Collections.Hashtable>>(objForm_ProductInfoEvent);
            objForm.ShowDialog();

        }

        void objForm_ProductInfoEvent(object sender, ParentChildRelateEventArgs<System.Collections.Hashtable> e)
        {
            this.txtItemCode.Value = e.CustomObject["itemcode"].ToString();
            this.txtItemDescV.Value = e.CustomObject["itemdesc"].ToString();
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            this.m_dtPalletToRcard.Clear();
            this.ucLabelEditQty.Value = "0";
            this.txtCartonOrRCard.TextFocus(true, true);
        }

        private void txtRCard_TxtboxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (this.txtCartonOrRCard.Value.Trim().Length == 0)
                {
                    this.txtCartonOrRCard.TextFocus(false, true);
                    return;
                }

                if (this.checkItemCode.Checked && this.txtCartonOrRCard.Value.Trim().ToUpper().IndexOf(this.txtItemCode.Value.Trim().ToUpper()) < 0)
                {
                    ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_RCARD_IS_NOT_Match"));
                    this.txtCartonOrRCard.TextFocus(false, true);
                    return;
                }

                if (this.chbRcardLength.Checked && int.Parse(this.txtLength.Value) != this.txtCartonOrRCard.Value.Trim().Length)
                {
                    ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$Error_SNLength_Wrong"));
                    this.txtCartonOrRCard.TextFocus(false, true);
                    return;
                }

                if (!CheckUIInfo())
                {
                    return;
                }

                //////��������Ʒ���к��Ƿ����
                ////
                //DataCollectFacade dataCollectFacade = new DataCollectFacade(DataProvider);
                //SimulationReport objSimulationReport = dataCollectFacade.GetLastSimulationReport(this.txtRCard.Value.Trim());

                //if (objSimulationReport == null)
                //{
                //    ApplicationRun.GetInfoForm().Add(
                //        new UserControl.Message(MessageType.Error, "$CS_RCARD_IS_NOT_EXIT"));
                //    this.txtRCard.TextFocus(false, true);
                //    return;
                //}

                if (ultraOptionSetAddDelete.CheckedItem.DataValue.ToString().Equals(packDelete))
                {
                    //For Delete
                    if (RemoveGridRecord(this.txtCartonOrRCard.Value.Trim()) == 0)
                    {
                        ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_RCARD_IS_NOT_EXIT"));
                        this.txtCartonOrRCard.TextFocus(true, true);
                        return;
                    }
                    else
                    {
                        ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Success, "$CS_Delete_Success"));
                        this.txtCartonOrRCard.TextFocus(true, true);
                        return;
                    }

                }
                else if (ultraOptionSetAddDelete.CheckedItem.DataValue.ToString().Equals(packAdd))
                {
                    ////For Add
                    //
                    ////���Grid�Ƿ��Ѿ��ӹ�RCard
                    //
                    if (CheckDuplicateInputRcard(this.txtCartonOrRCard.Value.Trim()))
                    {
                        ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_IDRepeatCollect"));
                        this.txtCartonOrRCard.TextFocus(true, true);
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.txtPlanQty.Value) && int.Parse(this.txtPlanQty.Value.Trim()) <= int.Parse(this.ucLabelEditQty.Value.Trim()))
                    {
                        ApplicationRun.GetInfoForm().Add(
                            new UserControl.Message(MessageType.Error, "$ERROR_Number_Cannot_Bigger_PlanNumber"));
                        return;
                    }

                    if (!this.CheckInInv(this.txtCartonOrRCard.Value.Trim(), this.ucLabelEditStock.Value.Trim(), this.txtItemCode.Value.Trim(), this.txtPalletCode.Value.Trim(), this.ucLabelComboxCompany.SelectedItemValue.ToString()))
                    {
                        this.txtCartonOrRCard.TextFocus(true, true);
                        return;
                    }

                    DataRow dr = this.m_dtPalletToRcard.NewRow();
                    dr["palletcode"] = this.txtPalletCode.Value.Trim();
                    dr["rcard"] = this.txtCartonOrRCard.Value.Trim();

                    this.m_dtPalletToRcard.Rows.Add(dr);
                    this.ucLabelEditQty.Value = Convert.ToString(Convert.ToInt32(this.ucLabelEditQty.Value) + 1);

                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Success, "$CS_Add_Success"));
                    this.txtCartonOrRCard.TextFocus(true, true);
                }

                InventoryFacade inventoryFacade = new InventoryFacade(DataProvider);
                //���¶�λ״̬
                if (this.ucLabelEditStock.Value.Trim().Length > 0)
                {
                    object objectStackMessage = inventoryFacade.GetStackMessage(this.ucLabelEditStock.Value.Trim().ToUpper());
                    if (objectStackMessage != null)
                    {
                        this.ucLabelEditstackMessage.Value = ((StackMessage)objectStackMessage).StackQtyMessage;
                    }
                }
            }
        }

        /// <summary>
        /// Remove record in grid when input barcode
        /// </summary>
        private int RemoveGridRecord(string rcard)
        {
            int intNum = 0;
            for (int i = this.gridRcard.Rows.Count - 1; i >= 0; i--)
            {

                if (this.gridRcard.Rows[i].Cells["rcard"].Value.ToString().Equals(rcard))
                {
                    this.gridRcard.Rows[i].Delete(false);
                    this.ucLabelEditQty.Value = Convert.ToString(Convert.ToInt32(this.ucLabelEditQty.Value) - 1);
                    intNum = intNum + 1;
                }
            }

            return intNum;
        }

        private bool CheckDuplicateInputRcard(string rcard)
        {
            for (int i = 0; i < this.gridRcard.Rows.Count; i++)
            {
                if (rcard.Equals(this.gridRcard.Rows[i].Cells["rcard"].Text.Trim()))
                {
                    return true;
                }
            }
            return false;
        }

        private void cmdInINV_Click(object sender, EventArgs e)
        {
            if (!CheckUIInfo())
            {
                return;
            }

            if (!string.IsNullOrEmpty(this.txtPlanQty.Value) && int.Parse(this.txtPlanQty.Value.Trim())<int.Parse(this.ucLabelEditQty.Value.Trim()))
            {
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$ERROR_Number_Cannot_Bigger_PlanNumber"));
                return;
            }

            if (this.gridRcard.Rows.Count == 0)
            {
                //�����û������
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_GRID_NO_RECORD"));
                return;
            }



            for (int i = 0; i < this.gridRcard.Rows.Count; i++)
            {
                if (!this.CheckInInv(this.gridRcard.Rows[i].Cells["rcard"].Value.ToString(), this.ucLabelEditStock.Value.Trim(), this.txtItemCode.Value.Trim(), this.txtPalletCode.Value.Trim(), this.ucLabelComboxCompany.SelectedItemValue.ToString()))
                {
                    return;
                }
            }

            InventoryFacade invFacade = new InventoryFacade(this.DataProvider);
            invFacade.SaveInInventoryByNonProduce(this.ucLabelComboxINVType.SelectedItemValue.ToString().Trim(),
                                                  this.ucLabelEditStock.Value.Trim(),
                                                  this.ucLabelComboxCompany.SelectedItemValue.ToString().Trim(),
                                                  this.txtDeliverUser.Value.Trim(),
                                                  ApplicationService.Current().LoginInfo.UserCode,
                                                  this.txtTransCode.Value.Trim(),
                                                  this.txtItemCode.Value.Trim(),
                                                  this.cboBusinssCode.SelectedItemValue.ToString().Trim(),
                                                  FormatHelper.CleanString(this.txtMemo.Value.Trim()),
                                                  this.txtRelationDoc.Value.Trim(),
                                                  ApplicationService.Current().ResourceCode,
                                                  this.m_dtPalletToRcard);

            m_dtPalletToRcard.Clear();
            this.ucLabelEditQty.Value = "0";

            //���¶�λ״̬
            if (this.ucLabelEditStock.Value.Trim().Length > 0)
            {
                object objectStackMessage = invFacade.GetStackMessage(this.ucLabelEditStock.Value.Trim().ToUpper());
                if (objectStackMessage != null)
                {
                    this.ucLabelEditstackMessage.Value = ((StackMessage)objectStackMessage).StackQtyMessage;
                }
            }

            ApplicationRun.GetInfoForm().Add(
                new UserControl.Message(MessageType.Success, "$CS_IN_INV_SUCCESS"));

        }

        private bool CheckInInv(string rcard, string stackCode, string itemCode, string palletCode, string companyCode)
        {
            if (!this.CheckSelecetedStackAndStorage(this.SelectedStorage, this.SelectedStack, Convert.ToString(this.ucLabelComboxINVType.SelectedItemValue)))
            {
                return false;
            }

            if (!CheckStackAndRcardInfo(rcard, stackCode, itemCode, palletCode, companyCode, Convert.ToString(this.ucLabelComboxINVType.SelectedItemValue)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ��Ʒ�����
        /// </summary>
        /// <returns>true/false</returns>
        private bool CheckStackAndRcardInfo(string rcard, string stackCode, string itemCode, string palletCode, string companyCode, string storageCode)
        {
            DataCollectFacade dcFacade = new DataCollectFacade(this.DataProvider);
            SimulationReport report = dcFacade.GetLastSimulationReport(rcard);

            if (report != null)
            {
                if (!report.ItemCode.Equals(itemCode))
                {
                    //Message:��Ʒ���к�����Ӧ���������������ϲ���Ӧ
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_PRODUCT_ITEM_NOT_SAME $ITEM_CODE=" + report.ItemCode));
                    return false;
                }
            }


            ////Check�Ӷ�λʹ��״����ҳ��������Ķ�λ�Ϳ�λ
            //if (!this.CheckSelecetedStackAndStorage(this.SelectedStorage, this.SelectedStack, Convert.ToString(this.ucLabelComboxINVType.SelectedItemValue)))
            //{
            //    return false;
            //}
            //else
            //{
            //Check���кŶ�Ӧ���Ϻ��Ƿ�Ͷ�λ�Ĳ�һ��
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
            object[] objStackToRcards = inventoryFacade.GetAnyStack2RCardByStack(stackCode);

            if (objStackToRcards != null)
            {
                if (inventoryFacade.CheckStackIsOnlyAllowOneItem(stackCode) && !((StackToRcard)objStackToRcards[0]).ItemCode.Equals(itemCode))
                {
                    //��λ���Ϻź͵�ǰ��Ʒ���ϺŲ�һ��
                    //ucMessage1.AddEx(this._FunctionName, this.opsetPackObject.CheckedItem.DisplayText + ": " + this.txtRCard.Value + ";�Ϻ�:" + this.txtItemCode.Value, new UserControl.Message(MessageType.Error, "$CS_STACK_AND_PRODUCT_ITEM_NOT_SAME"), true);
                    ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_STACK_AND_PRODUCT_ITEM_NOT_SAME $ITEM_CODE=" + itemCode));
                    return false;
                }
            }

            //Check ���к��Ƿ��Ѿ������
            object objStarckToRcard = inventoryFacade.GetStackToRcard(rcard);

            if (objStarckToRcard != null)
            {
                //���к��ظ����
                //ucMessage1.AddEx(this._FunctionName, "���к�" + ": " + this.txtRCard.Value, new UserControl.Message(MessageType.Error, "$CS_SERIAL_EXIST"), true);
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_SERIAL_EXIST $SERIAL_NO=" + rcard));
                return false;
            }
            //}

            //Check ���к��Ƿ��Ѿ�������ջ����
            PackageFacade objPFFacade = new PackageFacade(this.DataProvider);
            object objPallet2RCard = objPFFacade.GetPallet2RCardByRCard(rcard);
            if (objPallet2RCard != null)
            {
                ApplicationRun.GetInfoForm().Add(
                    new UserControl.Message(MessageType.Error, "$CS_RCARD_EXIST_IN_PALLET " + ((Pallet2RCard)objPallet2RCard).PalletCode));
                return false;
            }

            //Checkջ��(�����ջ��,����ջ����ϵͳ���Ѿ�����)�Ĺ�˾��,��Ʒ��Ͳ�Ʒ�ȼ��Ƿ�͵�ǰ��һ��
            if (palletCode.Length > 0)
            {
                object[] objStackToRcardList = inventoryFacade.GetStackToRcardInfoByPallet(palletCode);
                if (objStackToRcardList != null)
                {
                    string strErrorMessage = string.Empty;
                    StackToRcard obj = (StackToRcard)objStackToRcardList[0];
                    if (obj.ItemCode != itemCode)
                    {
                        strErrorMessage = strErrorMessage + "itemcode";
                        ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_ITEM_NOT_SAME_IN_PALLET=" + obj.ItemCode));
                    }

                    if (obj.Company != companyCode)
                    {
                        strErrorMessage = strErrorMessage + "companycode";
                        ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_COMPANY_NOT_SAME_IN_PALLET=" + obj.Company));
                    }

                    //if (obj.ItemGrade != itemGrade)
                    //{
                    //    //ApplicationRun.GetInfoForm().Add(
                    //    //new UserControl.Message(MessageType.Error, "$CS_RCARD_EXIST_IN_PALLET" + ((Pallet2RCard)objPallet2RCard).PalletCode));
                    //    //return false;
                    //    strErrorMessage = strErrorMessage + "itemgrade";
                    //    ApplicationRun.GetInfoForm().Add(
                    //    new UserControl.Message(MessageType.Error, "$CS_ITEMGRADE_NOT_SAME_IN_PALLET=" + obj.ItemGrade));
                    //}

                    if (strErrorMessage != string.Empty)
                    {
                        return false;
                    }


                }
                else
                {
                    if (!inventoryFacade.CheckStackCapacity(storageCode, stackCode))
                    {
                        ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_STACK_CAPACITY_FULL"));
                        return false;
                    }
                }
            }

            //Check ����Ĳ�Ʒ�͵�ǰѡ��Ĳ�Ʒ��һ��
            object[] outTransList = inventoryFacade.GetInvOutTransaction(rcard, "");

            if (outTransList != null)
            {
                if (!((InvOutTransaction)outTransList[0]).ItemCode.Equals(itemCode))
                {
                    //Message:����Ĳ�Ʒ:XXXXXXXXXXX�͵�ǰѡ��Ĳ�Ʒ��һ��,�Ƿ����ʹ�õ�ǰѡ��Ĳ�Ʒ
                    if (MessageBox.Show(UserControl.MutiLanguages.ParserMessage("$CS_OUT_INV_ITEM_NOT_SAME") + "=" + ((InvOutTransaction)outTransList[0]).ItemCode + "," + UserControl.MutiLanguages.ParserMessage("$CS_OUT_INV_ITEM_NOT_SAME_ASK") + "?", UserControl.MutiLanguages.ParserMessage("$CS_CONFIRM"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return false;
                    }
                }
            }

            if (!this.CheckRule(rcard))
            {
                return false;
            }


            return true;
        }

        private bool CheckRule(string inputNo)
        {
            if (this.cboBusinssCode.SelectedItemText.Trim().Length == 0)
            {
                return true;
            }

            InventoryFacade invFacade = new InventoryFacade(this.DataProvider);

            object[] ruleList = invFacade.GetInvBusiness2Formula(this.cboBusinssCode.SelectedItemValue.ToString(), GlobalVariables.CurrentOrganizations.First().OrganizationID);

            if (ruleList == null)
            {
                ////Message:ҵ����������Ӧ�Ĺ���û�趨
                //ApplicationRun.GetInfoForm().Add(
                //    new UserControl.Message(MessageType.Error, "$CS_RULE_IS_NOT_SET"));
                return true;
            }

            foreach (InvBusiness2Formula rule in ruleList)
            {
                ////����Ʒ�Ƿ��Ѿ�������
                //
                if (rule.FormulaCode.Equals(OutInvRuleCheck.RelatedInvOutDoc))
                {
                    if (this.txtRelationDoc.Value.Trim().Length == 0)
                    {
                        //Message:�������������
                        ApplicationRun.GetInfoForm().Add(
                            new UserControl.Message(MessageType.Error, "$CS_INPUT_TRANS_CODE"));
                        return false;
                    }

                    if (!invFacade.CheckRelatedInvOutDoc(inputNo, this.txtRelationDoc.Value.Trim()))
                    {
                        //Message:���к��ڹ���������û�г���ѶϢ
                        ApplicationRun.GetInfoForm().Add(
                            new UserControl.Message(MessageType.Error, "$CS_RCARD_IS_NOT_RelatedInvOutDoc $CS_Param_RunSeq=" + inputNo));
                        return false;
                    }
                }
            }

            return true;
        }


        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.txtTransCode.Value = "";
            this.cboBusinssCode.SelectedIndex = -1;
            this.txtDeliverUser.Value = "";
            this.txtItemCode.Value = "";
            this.txtItemDescV.Value = "";
            this.txtPlanQty.Value = "";
            this.txtMemo.Value = "";
            this.ucLabelComboxCompany.SelectedIndex = -1;
            this.ucLabelComboxINVType.SelectedIndex = -1;
            this.ucLabelEditQty.Value = "0";
            this.ucLabelEditStock.Value = "";
            this.txtPalletCode.Checked = false;
            this.txtCartonOrRCard.Value = "";
            this.txtRelationDoc.Value = "";
            this.m_dtPalletToRcard.Clear();
            this.txtTransCode.TextFocus(true, true);


        }

        private void gridRcard_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // ����Ӧ�п�
            e.Layout.AutoFitColumns = false;
            //e.Layout.Override.AllowColSizing = AllowColSizing.None;

            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;

            // ����
            e.Layout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;

            e.Layout.Bands[0].Columns["palletcode"].Width = 300;
            e.Layout.Bands[0].Columns["rcard"].Width = 300;

            // ��λ����
            e.Layout.Bands[0].Columns["palletcode"].Header.Caption = "ջ���";
            e.Layout.Bands[0].Columns["rcard"].Header.Caption = "���к�";

            // ������λ�Ƿ�����༭������λ����ʾ��ʽ
            e.Layout.Bands[0].Columns["palletcode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["rcard"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            //this.InitGridLanguage(gridRcard);
        }

        private void ultraOptionSetAddDelete_ValueChanged(object sender, EventArgs e)
        {
            this.txtCartonOrRCard.TextFocus(false, true);
        }

        private void txtItemCode_TxtboxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                string itemCode = this.txtItemCode.Value.Trim().ToUpper();
                if (itemCode == string.Empty)
                {
                    return;
                }

                if (itemCode.Length > 12)
                {
                    itemCode = itemCode.Substring(0, 12);
                    this.txtItemCode.Value = itemCode;
                }

                ItemFacade objFacade = new ItemFacade(this.DataProvider);
                object objItem = objFacade.GetItem(FormatHelper.CleanString(itemCode), GlobalVariables.CurrentOrganizations.First().OrganizationID);

                if (objItem != null)
                {
                    this.txtItemDescV.Value = ((Item)objItem).ItemDescription;
                }
                else
                {
                    this.txtItemDescV.Value = "";
                    ApplicationRun.GetInfoForm().Add(
                        new UserControl.Message(MessageType.Error, "$CS_PRODUCT_CODE_NOT_EXIST"));

                }
            }
        }


    }
}