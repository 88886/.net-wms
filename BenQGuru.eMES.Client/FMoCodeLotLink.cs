using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BenQGuru.eMES.CodeSoftPrint;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.IQC;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Client.Service;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using UserControl;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Data;
using System.Text.RegularExpressions;

namespace BenQGuru.eMES.Client
{
    public class FMoCodeLotLink : BaseForm 
    {
        private bool _IsBatchPrint = true;
        private string _DataDescFileName = "Label.dsc";
        private PrintTemplate[] _PrintTemplateList = null;
        private DataSet m_CheckList = null;
        private DataTable m_Mo2LotLink = null;
        private string currentItemCode = string.Empty;
        private decimal currentLotQty = 0;
        private string currentMo = string.Empty;
        private static decimal lotCount = 0;
        private string endLotNo = string.Empty;
        private int remnant = 0;

        Hashtable lotno = new Hashtable();

        #region �Զ�����

        private System.Windows.Forms.GroupBox grpQuery;
        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private UserControl.UCButton ucBtnDelete;
        private UserControl.UCButton ucBtnExit;
        private System.ComponentModel.IContainer components = null;
        private UCLabelEdit ucLabelEditLotPrefix;
        private UCLabelEdit ucLabelEditPrintCount;
        public UCLabelCombox ucLabelComboxPrintTemplete;
        public UCLabelCombox ucLabelComboxPrintList;
        public UCButton ucButtonPrint;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridMain;
        private UCLabelEdit ucLEStartLotNo;
        private CheckBox checkBoxNoPrint;
        private UCLabelEdit ucLabelLotQuery;
        private UCButton ucButtonQuery;
        private CheckBox checkBoxSNContentCheck;
        private UCLabelEdit ucLabelLotQty;
        private UCLabelEdit txtMoCodeEdit;
        private RadioButton radioButton4;
        private UCLabelEdit ucLabelLotLength;
        private UCButton ucButtonCalcLotNoEnd;
        private UCButton ucButtonExit;
        private UCLabelEdit ucLabelCreateQTY;
        public Infragistics.Win.UltraWinEditors.UltraOptionSet ultraOptionSetScale;
        private UCLabelEdit ucLabelMOPlanQty;
        private UCLabelEdit txtMoCodeQuery;

        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ��������ɵĴ���

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMoCodeLotLink));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.grpQuery = new System.Windows.Forms.GroupBox();
            this.txtMoCodeQuery = new UserControl.UCLabelEdit();
            this.ucLabelLotQuery = new UserControl.UCLabelEdit();
            this.ucButtonQuery = new UserControl.UCButton();
            this.panelButton = new System.Windows.Forms.Panel();
            this.ucBtnExit = new UserControl.UCButton();
            this.ucButtonPrint = new UserControl.UCButton();
            this.ucLabelComboxPrintTemplete = new UserControl.UCLabelCombox();
            this.ucLabelEditPrintCount = new UserControl.UCLabelEdit();
            this.ucLabelComboxPrintList = new UserControl.UCLabelCombox();
            this.checkBoxNoPrint = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ucLabelMOPlanQty = new UserControl.UCLabelEdit();
            this.ultraOptionSetScale = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ucButtonExit = new UserControl.UCButton();
            this.ucButtonCalcLotNoEnd = new UserControl.UCButton();
            this.txtMoCodeEdit = new UserControl.UCLabelEdit();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.ucLabelLotQty = new UserControl.UCLabelEdit();
            this.checkBoxSNContentCheck = new System.Windows.Forms.CheckBox();
            this.ucBtnDelete = new UserControl.UCButton();
            this.ucLabelLotLength = new UserControl.UCLabelEdit();
            this.ucLabelCreateQTY = new UserControl.UCLabelEdit();
            this.ucLabelEditLotPrefix = new UserControl.UCLabelEdit();
            this.ucLEStartLotNo = new UserControl.UCLabelEdit();
            this.ultraGridMain = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpQuery.SuspendLayout();
            this.panelButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOptionSetScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridMain)).BeginInit();
            this.SuspendLayout();
            // 
            // grpQuery
            // 
            this.grpQuery.Controls.Add(this.txtMoCodeQuery);
            this.grpQuery.Controls.Add(this.ucLabelLotQuery);
            this.grpQuery.Controls.Add(this.ucButtonQuery);
            this.grpQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpQuery.Location = new System.Drawing.Point(0, 0);
            this.grpQuery.Name = "grpQuery";
            this.grpQuery.Size = new System.Drawing.Size(831, 39);
            this.grpQuery.TabIndex = 0;
            this.grpQuery.TabStop = false;
            // 
            // txtMoCodeQuery
            // 
            this.txtMoCodeQuery.AllowEditOnlyChecked = true;
            this.txtMoCodeQuery.AutoUpper = true;
            this.txtMoCodeQuery.Caption = "��������";
            this.txtMoCodeQuery.Checked = false;
            this.txtMoCodeQuery.EditType = UserControl.EditTypes.String;
            this.txtMoCodeQuery.Location = new System.Drawing.Point(24, 13);
            this.txtMoCodeQuery.MaxLength = 40;
            this.txtMoCodeQuery.Multiline = false;
            this.txtMoCodeQuery.Name = "txtMoCodeQuery";
            this.txtMoCodeQuery.PasswordChar = '\0';
            this.txtMoCodeQuery.ReadOnly = false;
            this.txtMoCodeQuery.ShowCheckBox = false;
            this.txtMoCodeQuery.Size = new System.Drawing.Size(194, 24);
            this.txtMoCodeQuery.TabIndex = 59;
            this.txtMoCodeQuery.TabNext = false;
            this.txtMoCodeQuery.Value = "";
            this.txtMoCodeQuery.WidthType = UserControl.WidthTypes.Normal;
            this.txtMoCodeQuery.XAlign = 85;
            // 
            // ucLabelLotQuery
            // 
            this.ucLabelLotQuery.AllowEditOnlyChecked = true;
            this.ucLabelLotQuery.AutoUpper = true;
            this.ucLabelLotQuery.Caption = "��������";
            this.ucLabelLotQuery.Checked = false;
            this.ucLabelLotQuery.EditType = UserControl.EditTypes.String;
            this.ucLabelLotQuery.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ucLabelLotQuery.Location = new System.Drawing.Point(354, 13);
            this.ucLabelLotQuery.MaxLength = 40;
            this.ucLabelLotQuery.Multiline = false;
            this.ucLabelLotQuery.Name = "ucLabelLotQuery";
            this.ucLabelLotQuery.PasswordChar = '\0';
            this.ucLabelLotQuery.ReadOnly = false;
            this.ucLabelLotQuery.ShowCheckBox = false;
            this.ucLabelLotQuery.Size = new System.Drawing.Size(194, 24);
            this.ucLabelLotQuery.TabIndex = 58;
            this.ucLabelLotQuery.TabNext = true;
            this.ucLabelLotQuery.Value = "";
            this.ucLabelLotQuery.WidthType = UserControl.WidthTypes.Normal;
            this.ucLabelLotQuery.XAlign = 415;
            // 
            // ucButtonQuery
            // 
            this.ucButtonQuery.BackColor = System.Drawing.SystemColors.Control;
            this.ucButtonQuery.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucButtonQuery.BackgroundImage")));
            this.ucButtonQuery.ButtonType = UserControl.ButtonTypes.Query;
            this.ucButtonQuery.Caption = "��ѯ";
            this.ucButtonQuery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucButtonQuery.Location = new System.Drawing.Point(689, 13);
            this.ucButtonQuery.Name = "btnQuery";
            this.ucButtonQuery.Size = new System.Drawing.Size(88, 22);
            this.ucButtonQuery.TabIndex = 57;
            this.ucButtonQuery.Click += new System.EventHandler(this.ucButtonQuery_Click);
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.ucBtnExit);
            this.panelButton.Controls.Add(this.ucButtonPrint);
            this.panelButton.Controls.Add(this.ucLabelComboxPrintTemplete);
            this.panelButton.Controls.Add(this.ucLabelEditPrintCount);
            this.panelButton.Controls.Add(this.ucLabelComboxPrintList);
            this.panelButton.Controls.Add(this.checkBoxNoPrint);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 612);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(831, 80);
            this.panelButton.TabIndex = 4;
            // 
            // ucBtnExit
            // 
            this.ucBtnExit.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnExit.BackgroundImage")));
            this.ucBtnExit.ButtonType = UserControl.ButtonTypes.Exit;
            this.ucBtnExit.Caption = "�˳�";
            this.ucBtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExit.Location = new System.Drawing.Point(438, 53);
            this.ucBtnExit.Name = "btnExit";
            this.ucBtnExit.Size = new System.Drawing.Size(88, 22);
            this.ucBtnExit.TabIndex = 25;
            // 
            // ucButtonPrint
            // 
            this.ucButtonPrint.BackColor = System.Drawing.SystemColors.Control;
            this.ucButtonPrint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucButtonPrint.BackgroundImage")));
            this.ucButtonPrint.ButtonType = UserControl.ButtonTypes.None;
            this.ucButtonPrint.Caption = "��ӡ";
            this.ucButtonPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucButtonPrint.Location = new System.Drawing.Point(304, 53);
            this.ucButtonPrint.Name = "btnPrint";
            this.ucButtonPrint.Size = new System.Drawing.Size(88, 22);
            this.ucButtonPrint.TabIndex = 53;
            this.ucButtonPrint.Click += new System.EventHandler(this.ucButtonPrint_Click);
            // 
            // ucLabelComboxPrintTemplete
            // 
            this.ucLabelComboxPrintTemplete.AllowEditOnlyChecked = true;
            this.ucLabelComboxPrintTemplete.Caption = "��ӡģ��";
            this.ucLabelComboxPrintTemplete.Checked = false;
            this.ucLabelComboxPrintTemplete.Location = new System.Drawing.Point(256, 27);
            this.ucLabelComboxPrintTemplete.Name = "ucLabelComboxPrintTemplete";
            this.ucLabelComboxPrintTemplete.SelectedIndex = -1;
            this.ucLabelComboxPrintTemplete.ShowCheckBox = false;
            this.ucLabelComboxPrintTemplete.Size = new System.Drawing.Size(194, 20);
            this.ucLabelComboxPrintTemplete.TabIndex = 49;
            this.ucLabelComboxPrintTemplete.WidthType = UserControl.WidthTypes.Normal;
            this.ucLabelComboxPrintTemplete.XAlign = 317;
            // 
            // ucLabelEditPrintCount
            // 
            this.ucLabelEditPrintCount.AllowEditOnlyChecked = true;
            this.ucLabelEditPrintCount.AutoUpper = true;
            this.ucLabelEditPrintCount.Caption = "��ӡ����";
            this.ucLabelEditPrintCount.Checked = false;
            this.ucLabelEditPrintCount.EditType = UserControl.EditTypes.Integer;
            this.ucLabelEditPrintCount.Location = new System.Drawing.Point(491, 27);
            this.ucLabelEditPrintCount.MaxLength = 8;
            this.ucLabelEditPrintCount.Multiline = false;
            this.ucLabelEditPrintCount.Name = "ucLabelEditPrintCount";
            this.ucLabelEditPrintCount.PasswordChar = '\0';
            this.ucLabelEditPrintCount.ReadOnly = false;
            this.ucLabelEditPrintCount.ShowCheckBox = false;
            this.ucLabelEditPrintCount.Size = new System.Drawing.Size(194, 24);
            this.ucLabelEditPrintCount.TabIndex = 48;
            this.ucLabelEditPrintCount.TabNext = false;
            this.ucLabelEditPrintCount.Tag = "";
            this.ucLabelEditPrintCount.Value = "1";
            this.ucLabelEditPrintCount.WidthType = UserControl.WidthTypes.Normal;
            this.ucLabelEditPrintCount.XAlign = 552;
            // 
            // ucLabelComboxPrintList
            // 
            this.ucLabelComboxPrintList.AllowEditOnlyChecked = true;
            this.ucLabelComboxPrintList.Caption = "��ӡ���б�";
            this.ucLabelComboxPrintList.Checked = false;
            this.ucLabelComboxPrintList.Location = new System.Drawing.Point(33, 27);
            this.ucLabelComboxPrintList.Name = "ucLabelComboxPrintList";
            this.ucLabelComboxPrintList.SelectedIndex = -1;
            this.ucLabelComboxPrintList.ShowCheckBox = false;
            this.ucLabelComboxPrintList.Size = new System.Drawing.Size(206, 20);
            this.ucLabelComboxPrintList.TabIndex = 50;
            this.ucLabelComboxPrintList.WidthType = UserControl.WidthTypes.Normal;
            this.ucLabelComboxPrintList.XAlign = 106;
            // 
            // checkBoxNoPrint
            // 
            this.checkBoxNoPrint.AutoSize = true;
            this.checkBoxNoPrint.Location = new System.Drawing.Point(11, 6);
            this.checkBoxNoPrint.Name = "checkBoxNoPrint";
            this.checkBoxNoPrint.Size = new System.Drawing.Size(60, 16);
            this.checkBoxNoPrint.TabIndex = 57;
            this.checkBoxNoPrint.Text = "δ��ӡ";
            this.checkBoxNoPrint.UseVisualStyleBackColor = true;
            this.checkBoxNoPrint.CheckedChanged += new System.EventHandler(this.checkBoxNoPrint_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ucLabelMOPlanQty);
            this.groupBox1.Controls.Add(this.ultraOptionSetScale);
            this.groupBox1.Controls.Add(this.ucButtonExit);
            this.groupBox1.Controls.Add(this.ucButtonCalcLotNoEnd);
            this.groupBox1.Controls.Add(this.txtMoCodeEdit);
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.ucLabelLotQty);
            this.groupBox1.Controls.Add(this.checkBoxSNContentCheck);
            this.groupBox1.Controls.Add(this.ucBtnDelete);
            this.groupBox1.Controls.Add(this.ucLabelLotLength);
            this.groupBox1.Controls.Add(this.ucLabelCreateQTY);
            this.groupBox1.Controls.Add(this.ucLabelEditLotPrefix);
            this.groupBox1.Controls.Add(this.ucLEStartLotNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 473);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(831, 139);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // ucLabelMOPlanQty
            // 
            this.ucLabelMOPlanQty.AllowEditOnlyChecked = true;
            this.ucLabelMOPlanQty.AutoUpper = true;
            this.ucLabelMOPlanQty.Caption = "�����ƻ�����";
            this.ucLabelMOPlanQty.Checked = false;
            this.ucLabelMOPlanQty.EditType = UserControl.EditTypes.Integer;
            this.ucLabelMOPlanQty.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ucLabelMOPlanQty.Location = new System.Drawing.Point(272, 22);
            this.ucLabelMOPlanQty.MaxLength = 40;
            this.ucLabelMOPlanQty.Multiline = false;
            this.ucLabelMOPlanQty.Name = "ucLabelMOPlanQty";
            this.ucLabelMOPlanQty.PasswordChar = '\0';
            this.ucLabelMOPlanQty.ReadOnly = false;
            this.ucLabelMOPlanQty.ShowCheckBox = false;
            this.ucLabelMOPlanQty.Size = new System.Drawing.Size(218, 24);
            this.ucLabelMOPlanQty.TabIndex = 6;
            this.ucLabelMOPlanQty.TabNext = true;
            this.ucLabelMOPlanQty.Value = "";
            this.ucLabelMOPlanQty.WidthType = UserControl.WidthTypes.Normal;
            this.ucLabelMOPlanQty.XAlign = 357;
            // 
            // ultraOptionSetScale
            // 
            appearance1.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.FontData.BoldAsString = "False";
            this.ultraOptionSetScale.Appearance = appearance1;
            this.ultraOptionSetScale.BackColor = System.Drawing.Color.Transparent;
            this.ultraOptionSetScale.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraOptionSetScale.CausesValidation = false;
            this.ultraOptionSetScale.CheckedIndex = 2;
            appearance14.ForeColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(117)))), ((int)(((byte)(111)))));
            this.ultraOptionSetScale.ItemAppearance = appearance14;
            valueListItem1.DataValue = "0";
            valueListItem1.DisplayText = "10����";
            valueListItem2.DataValue = "1";
            valueListItem2.DisplayText = "16����";
            valueListItem3.DataValue = "2";
            valueListItem3.DisplayText = "34����";
            this.ultraOptionSetScale.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.ultraOptionSetScale.Location = new System.Drawing.Point(533, 82);
            this.ultraOptionSetScale.Name = "ultraOptionSetScale";
            this.ultraOptionSetScale.Size = new System.Drawing.Size(188, 20);
            this.ultraOptionSetScale.TabIndex = 67;
            this.ultraOptionSetScale.Text = "34����";
            this.ultraOptionSetScale.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ucButtonExit
            // 
            this.ucButtonExit.BackColor = System.Drawing.SystemColors.Control;
            this.ucButtonExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucButtonExit.BackgroundImage")));
            this.ucButtonExit.ButtonType = UserControl.ButtonTypes.Cancle;
            this.ucButtonExit.Caption = "ȡ��";
            this.ucButtonExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucButtonExit.Location = new System.Drawing.Point(438, 109);
            this.ucButtonExit.Name = "btnExit";
            this.ucButtonExit.Size = new System.Drawing.Size(88, 22);
            this.ucButtonExit.TabIndex = 66;
            this.ucButtonExit.Click += new System.EventHandler(this.ucButtonExit_Click);
            // 
            // ucButtonCalcLotNoEnd
            // 
            this.ucButtonCalcLotNoEnd.BackColor = System.Drawing.SystemColors.Control;
            this.ucButtonCalcLotNoEnd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucButtonCalcLotNoEnd.BackgroundImage")));
            this.ucButtonCalcLotNoEnd.ButtonType = UserControl.ButtonTypes.None;
            this.ucButtonCalcLotNoEnd.Caption = "����";
            this.ucButtonCalcLotNoEnd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucButtonCalcLotNoEnd.Location = new System.Drawing.Point(224, 109);
            this.ucButtonCalcLotNoEnd.Name = "btnCalcLotNoEnd";
            this.ucButtonCalcLotNoEnd.Size = new System.Drawing.Size(88, 22);
            this.ucButtonCalcLotNoEnd.TabIndex = 65;
            this.ucButtonCalcLotNoEnd.Click += new System.EventHandler(this.ucButtonCalcLotNoEnd_Click);
            // 
            // txtMoCodeEdit
            // 
            this.txtMoCodeEdit.AllowEditOnlyChecked = true;
            this.txtMoCodeEdit.AutoUpper = true;
            this.txtMoCodeEdit.Caption = "��������";
            this.txtMoCodeEdit.Checked = false;
            this.txtMoCodeEdit.EditType = UserControl.EditTypes.String;
            this.txtMoCodeEdit.Location = new System.Drawing.Point(39, 22);
            this.txtMoCodeEdit.MaxLength = 40;
            this.txtMoCodeEdit.Multiline = false;
            this.txtMoCodeEdit.Name = "txtMoCodeEdit";
            this.txtMoCodeEdit.PasswordChar = '\0';
            this.txtMoCodeEdit.ReadOnly = false;
            this.txtMoCodeEdit.ShowCheckBox = false;
            this.txtMoCodeEdit.Size = new System.Drawing.Size(194, 24);
            this.txtMoCodeEdit.TabIndex = 59;
            this.txtMoCodeEdit.TabNext = false;
            this.txtMoCodeEdit.Value = "";
            this.txtMoCodeEdit.WidthType = UserControl.WidthTypes.Normal;
            this.txtMoCodeEdit.XAlign = 100;
            this.txtMoCodeEdit.TxtboxKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMoCodeEdit_TxtboxKeyPress);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(376, 512);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(59, 16);
            this.radioButton4.TabIndex = 63;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "16����";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // ucLabelLotQty
            // 
            this.ucLabelLotQty.AllowEditOnlyChecked = true;
            this.ucLabelLotQty.AutoUpper = true;
            this.ucLabelLotQty.Caption = "��������";
            this.ucLabelLotQty.Checked = false;
            this.ucLabelLotQty.EditType = UserControl.EditTypes.Integer;
            this.ucLabelLotQty.Location = new System.Drawing.Point(39, 78);
            this.ucLabelLotQty.MaxLength = 8;
            this.ucLabelLotQty.Multiline = false;
            this.ucLabelLotQty.Name = "ucLabelLotQty";
            this.ucLabelLotQty.PasswordChar = '\0';
            this.ucLabelLotQty.ReadOnly = false;
            this.ucLabelLotQty.ShowCheckBox = false;
            this.ucLabelLotQty.Size = new System.Drawing.Size(194, 24);
            this.ucLabelLotQty.TabIndex = 8;
            this.ucLabelLotQty.TabNext = true;
            this.ucLabelLotQty.Value = "";
            this.ucLabelLotQty.WidthType = UserControl.WidthTypes.Normal;
            this.ucLabelLotQty.XAlign = 100;
            // 
            // checkBoxSNContentCheck
            // 
            this.checkBoxSNContentCheck.AutoSize = true;
            this.checkBoxSNContentCheck.Location = new System.Drawing.Point(533, 53);
            this.checkBoxSNContentCheck.Name = "checkBoxSNContentCheck";
            this.checkBoxSNContentCheck.Size = new System.Drawing.Size(210, 16);
            this.checkBoxSNContentCheck.TabIndex = 61;
            this.checkBoxSNContentCheck.Text = "�������к�����Ϊ�ַ�,���ֺͿո�";
            this.checkBoxSNContentCheck.UseVisualStyleBackColor = true;
            // 
            // ucBtnDelete
            // 
            this.ucBtnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.ucBtnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucBtnDelete.BackgroundImage")));
            this.ucBtnDelete.ButtonType = UserControl.ButtonTypes.Delete;
            this.ucBtnDelete.Caption = "ɾ��";
            this.ucBtnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnDelete.Location = new System.Drawing.Point(331, 109);
            this.ucBtnDelete.Name = "btnDelete";
            this.ucBtnDelete.Size = new System.Drawing.Size(88, 22);
            this.ucBtnDelete.TabIndex = 22;
            this.ucBtnDelete.Click += new System.EventHandler(this.ucBtnDelete_Click);
            // 
            // ucLabelLotLength
            // 
            this.ucLabelLotLength.AllowEditOnlyChecked = true;
            this.ucLabelLotLength.AutoUpper = true;
            this.ucLabelLotLength.Caption = "�������볤��";
            this.ucLabelLotLength.Checked = false;
            this.ucLabelLotLength.EditType = UserControl.EditTypes.Integer;
            this.ucLabelLotLength.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ucLabelLotLength.Location = new System.Drawing.Point(272, 50);
            this.ucLabelLotLength.MaxLength = 4;
            this.ucLabelLotLength.Multiline = false;
            this.ucLabelLotLength.Name = "ucLabelLotLength";
            this.ucLabelLotLength.PasswordChar = '\0';
            this.ucLabelLotLength.ReadOnly = false;
            this.ucLabelLotLength.ShowCheckBox = false;
            this.ucLabelLotLength.Size = new System.Drawing.Size(218, 24);
            this.ucLabelLotLength.TabIndex = 2;
            this.ucLabelLotLength.TabNext = true;
            this.ucLabelLotLength.Value = "";
            this.ucLabelLotLength.WidthType = UserControl.WidthTypes.Normal;
            this.ucLabelLotLength.XAlign = 357;
            // 
            // ucLabelCreateQTY
            // 
            this.ucLabelCreateQTY.AllowEditOnlyChecked = true;
            this.ucLabelCreateQTY.AutoUpper = true;
            this.ucLabelCreateQTY.Caption = "������������";
            this.ucLabelCreateQTY.Checked = false;
            this.ucLabelCreateQTY.EditType = UserControl.EditTypes.Integer;
            this.ucLabelCreateQTY.Location = new System.Drawing.Point(533, 22);
            this.ucLabelCreateQTY.MaxLength = 8;
            this.ucLabelCreateQTY.Multiline = false;
            this.ucLabelCreateQTY.Name = "ucLabelCreateQTY";
            this.ucLabelCreateQTY.PasswordChar = '\0';
            this.ucLabelCreateQTY.ReadOnly = false;
            this.ucLabelCreateQTY.ShowCheckBox = false;
            this.ucLabelCreateQTY.Size = new System.Drawing.Size(218, 24);
            this.ucLabelCreateQTY.TabIndex = 7;
            this.ucLabelCreateQTY.TabNext = true;
            this.ucLabelCreateQTY.Value = "";
            this.ucLabelCreateQTY.WidthType = UserControl.WidthTypes.Normal;
            this.ucLabelCreateQTY.XAlign = 618;
            // 
            // ucLabelEditLotPrefix
            // 
            this.ucLabelEditLotPrefix.AllowEditOnlyChecked = true;
            this.ucLabelEditLotPrefix.AutoUpper = true;
            this.ucLabelEditLotPrefix.Caption = "��������ǰ׺";
            this.ucLabelEditLotPrefix.Checked = false;
            this.ucLabelEditLotPrefix.EditType = UserControl.EditTypes.String;
            this.ucLabelEditLotPrefix.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ucLabelEditLotPrefix.Location = new System.Drawing.Point(15, 50);
            this.ucLabelEditLotPrefix.MaxLength = 40;
            this.ucLabelEditLotPrefix.Multiline = false;
            this.ucLabelEditLotPrefix.Name = "ucLabelEditLotPrefix";
            this.ucLabelEditLotPrefix.PasswordChar = '\0';
            this.ucLabelEditLotPrefix.ReadOnly = false;
            this.ucLabelEditLotPrefix.ShowCheckBox = false;
            this.ucLabelEditLotPrefix.Size = new System.Drawing.Size(218, 24);
            this.ucLabelEditLotPrefix.TabIndex = 2;
            this.ucLabelEditLotPrefix.TabNext = true;
            this.ucLabelEditLotPrefix.Value = "";
            this.ucLabelEditLotPrefix.WidthType = UserControl.WidthTypes.Normal;
            this.ucLabelEditLotPrefix.XAlign = 100;
            // 
            // ucLEStartLotNo
            // 
            this.ucLEStartLotNo.AllowEditOnlyChecked = true;
            this.ucLEStartLotNo.AutoUpper = true;
            this.ucLEStartLotNo.Caption = "��ʼ���";
            this.ucLEStartLotNo.Checked = false;
            this.ucLEStartLotNo.EditType = UserControl.EditTypes.String;
            this.ucLEStartLotNo.Location = new System.Drawing.Point(296, 78);
            this.ucLEStartLotNo.MaxLength = 8;
            this.ucLEStartLotNo.Multiline = false;
            this.ucLEStartLotNo.Name = "ucLEStartLotNo";
            this.ucLEStartLotNo.PasswordChar = '\0';
            this.ucLEStartLotNo.ReadOnly = false;
            this.ucLEStartLotNo.ShowCheckBox = false;
            this.ucLEStartLotNo.Size = new System.Drawing.Size(194, 24);
            this.ucLEStartLotNo.TabIndex = 9;
            this.ucLEStartLotNo.TabNext = true;
            this.ucLEStartLotNo.Value = "";
            this.ucLEStartLotNo.WidthType = UserControl.WidthTypes.Normal;
            this.ucLEStartLotNo.XAlign = 357;
            // 
            // ultraGridMain
            // 
            appearance4.BackColor = System.Drawing.SystemColors.Window;
            appearance4.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridMain.DisplayLayout.Appearance = appearance4;
            this.ultraGridMain.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridMain.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMain.DisplayLayout.GroupByBox.Appearance = appearance13;
            appearance2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMain.DisplayLayout.GroupByBox.BandLabelAppearance = appearance2;
            this.ultraGridMain.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMain.DisplayLayout.GroupByBox.PromptAppearance = appearance3;
            this.ultraGridMain.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridMain.DisplayLayout.MaxRowScrollRegions = 1;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridMain.DisplayLayout.Override.ActiveCellAppearance = appearance12;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridMain.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.ultraGridMain.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridMain.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridMain.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance5.BorderColor = System.Drawing.Color.Silver;
            appearance5.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridMain.DisplayLayout.Override.CellAppearance = appearance5;
            this.ultraGridMain.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraGridMain.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMain.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance11.TextHAlignAsString = "Left";
            this.ultraGridMain.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.ultraGridMain.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridMain.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridMain.DisplayLayout.Override.RowAppearance = appearance10;
            this.ultraGridMain.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance8.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridMain.DisplayLayout.Override.TemplateAddRowAppearance = appearance8;
            this.ultraGridMain.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridMain.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridMain.Location = new System.Drawing.Point(0, 39);
            this.ultraGridMain.Name = "ultraGridMain";
            this.ultraGridMain.Size = new System.Drawing.Size(831, 434);
            this.ultraGridMain.TabIndex = 5;
            this.ultraGridMain.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridMain_InitializeLayout);
            this.ultraGridMain.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGridMain_CellChange);
            // 
            // FMoCodeLotLink
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(831, 692);
            this.Controls.Add(this.ultraGridMain);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelButton);
            this.Controls.Add(this.grpQuery);
            this.Name = "FMoCodeLotLink";
            this.Text = "����������Ʒ��������";
            this.Load += new System.EventHandler(this.FMoCodeLotLink_Load);
            this.grpQuery.ResumeLayout(false);
            this.panelButton.ResumeLayout(false);
            this.panelButton.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOptionSetScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        #region Form Base
        private ItemLotFacade _ItemLotFacade = null;
        private ItemFacade _ItemFacade = null;
        private MOFacade _MOFacade = null;
        private MOLotFacade _MOLotFacade = null;
        private DataCollect.DataCollectFacade _DataCollectFacade = null;
        private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;
        private string LineNO = string.Empty;
        private string repNO = string.Empty;

        public IDomainDataProvider DataProvider
        {
            get
            {
                return _domainDataProvider;
            }
        }

        public FMoCodeLotLink()
        {
            // �õ����� Windows ���������������ġ�
            InitializeComponent();

            // TODO: �� InitializeComponent ���ú�����κγ�ʼ��

            //UserControl.UIStyleBuilder.GridUI(ultraGridMain);
            UserControl.UIStyleBuilder.FormUI(this);

            ultraGridMain.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Default;
            //UserControl.UIStyleBuilder.FormUI(this);
            this.ultraGridMain.UpdateMode = UpdateMode.OnCellChange;
            this.ultraGridMain.DisplayLayout.Appearance.BackColor = System.Drawing.Color.White; ;
            this.ultraGridMain.DisplayLayout.CaptionAppearance.BackColor = Color.FromName("WhiteSmoke");
            this.ultraGridMain.DisplayLayout.Appearance.BackColor = Color.FromArgb(255, 255, 255);
            this.ultraGridMain.DisplayLayout.Override.HeaderAppearance.BackColor = Color.FromName("WhiteSmoke");
            this.ultraGridMain.DisplayLayout.Override.RowAppearance.BackColor = Color.FromArgb(230, 234, 245);
            this.ultraGridMain.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.FromArgb(255, 255, 255);
            this.ultraGridMain.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridMain.DisplayLayout.Override.ActiveRowAppearance.BackColor = System.Drawing.Color.Gainsboro;
            this.ultraGridMain.DisplayLayout.Override.ActiveRowAppearance.ForeColor = System.Drawing.Color.Black;
            this.ultraGridMain.DisplayLayout.ScrollBarLook.Appearance.BackColor = Color.FromName("LightGray");

            this._ItemLotFacade = new ItemLotFacade(this.DataProvider);
            this._ItemFacade = new ItemFacade(this.DataProvider);
            this._MOFacade = new MOFacade(this.DataProvider);
            this._MOLotFacade = new MOLotFacade(this.DataProvider);
            this._DataCollectFacade = new DataCollect.DataCollectFacade(this.DataProvider);
        }


        protected void ShowMessage(string message)
        {
            ApplicationRun.GetInfoForm().Add(message);
        }

        protected void ShowMessage(Exception e)
        {
            ApplicationRun.GetInfoForm().Add(new UserControl.Message(e));
        }

        protected void ShowMessage(Messages messages)
        {
            ApplicationRun.GetInfoForm().Add(messages);
        }

        protected void ShowMessage(UserControl.Message message)
        {
            ApplicationRun.GetInfoForm().Add(message);
        }

        #endregion

        #region User Events
        private void FMoCodeLotLink_Load(object sender, System.EventArgs e)
        {
            this.ucLabelEditPrintCount.InnerTextBox.ForeColor = Color.Black;
            this.ucLabelEditPrintCount.InnerTextBox.TextAlign = HorizontalAlignment.Right;
            InitializeMainGrid();
            this.ultraGridMain.DisplayLayout.Bands[0].Columns["Checked"].Header.CheckBoxVisibility = Infragistics.Win.UltraWinGrid.HeaderCheckBoxVisibility.WhenUsingCheckEditor;
            LoadPrinter();
            LoadTemplateList();
            SetControlIsEnable(false);

            //this.InitPageLanguage();
            //this.InitGridLanguage(ultraGridMain);
        }
        #endregion

        #region Button Events

        //��ѯ��ť
        private void ucButtonQuery_Click(object sender, EventArgs e)
        {
            ////LotNo��Ҫʹ��GetSourceLot����ת���ɳ�ʼ��������
            //string lotNo = string.Empty;
            //if (this.ucLabelLotQuery.Value.Trim() != "")
            //{
            //    lotNo = _DataCollectFacade.GetSourceLot(this.ucLabelLotQuery.Value.Trim().ToUpper(), string.Empty);
            //}

            LoadCheckList(this.ucLabelLotQuery.Value.Trim(), this.txtMoCodeQuery.Value.Trim());
        }

        //ɾ����ť
        private void ucBtnDelete_Click(object sender, System.EventArgs e)
        {

            if (!CheckISSelectRow())
            {
                ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_NO_ROW_SELECTED"));
                return;
            }

            for (int i = 0; i < this.ultraGridMain.Rows.Count; i++)
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow row = ultraGridMain.Rows[i];
                
                if (bool.Parse(row.Cells["Checked"].Value.ToString()))
                {
                    //���ݹ�������+���ŵ���TBLLOTONWIP��ȡ����
                    //int count = _DataCollectFacade.GetRCardInfoCount(row.Cells["LotNo"].Value.ToString(), row.Cells["MoCode"].Value.ToString(), "", "");
                    //if (count > 0)
                    //{
                    //    ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LOTNO_EXIST_ONWIP$LotNo:" + row.Cells["LotNo"].Value.ToString()));
                    //    return;
                    //}
                    
                    //������ڲ������������������򲻿�ɾ��
                    object mo2LotLink = _MOLotFacade.GetMO2LotLink(row.Cells["LotNo"].Value.ToString().Trim(), row.Cells["MoCode"].Value.ToString().Trim());
                    if ((mo2LotLink != null) && (!(((MO2LotLink)mo2LotLink).LotStatus.Equals(LotStatusForMO2LotLink.LOTSTATUS_NEW))))
                    {
                        ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LOTNO_IS_USED $LotNo:" + row.Cells["LotNo"].Value.ToString()));
                        return;
                    }
                }
            }
            try
            {
                this.DataProvider.BeginTransaction();
                for (int i = 0; i < this.ultraGridMain.Rows.Count; i++)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = ultraGridMain.Rows[i];
                    
                    if (bool.Parse(row.Cells["Checked"].Value.ToString()))
                    {
                        object obj = _MOLotFacade.GetMO2LotLink(row.Cells["LotNo"].Value.ToString(), row.Cells["MoCode"].Value.ToString());
                        if (obj != null)
                        {
                            _MOLotFacade.DeleteMO2LotLink(obj as MO2LotLink);
                        }
                    }
                }
                this.DataProvider.CommitTransaction();

                ////LotNo��Ҫʹ��GetSourceLot����ת���ɳ�ʼ��������
                //string lotNo = string.Empty;
                //if (this.ucLabelLotQuery.Value.Trim() != "")
                //{
                //    lotNo = _DataCollectFacade.GetSourceLot(this.ucLabelLotQuery.Value.Trim().ToUpper(), string.Empty);
                //}

                LoadCheckList(this.ucLabelLotQuery.Value.Trim(), this.txtMoCodeQuery.Value.Trim());

            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
            }

        }

        //������������
        private void ucButtonCalcLotNoEnd_Click(object sender, EventArgs e)
        {

            //����Ƿ����빤���������Ƿ����TBLMO�С�
            if (this.txtMoCodeEdit.Value.Trim() == "")
            {
                ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_INPUT_MOCODE"));
                return;
            }
            else
            {
                object objMo = this._MOFacade.GetMO(currentMo);
                long createCount = 0;
                if (objMo != null)
                {
                    //����Ƿ����롱���ַ�����������ʼ�������롱����������������"������������"�����ұ��������������ɴ��ڼ���õ���������������
                    if (this.ucLabelEditLotPrefix.Value.Trim() == "")//��ʾ����ά���������������Ϣ
                    {
                        ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_ITEM2LOT_NOT_EXIST"));
                        this.ucLabelEditLotPrefix.TextFocus(false, true);
                        return;
                    }
                    if (this.ucLabelCreateQTY.Value.Trim() == "")//��ά��������������
                    {
                        ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LotCreateQty$Error_Input_Empty"));
                        this.ucLabelCreateQTY.TextFocus(false, true);
                        return;
                    }
                    else
                    {
                        if (decimal.Parse(this.ucLabelCreateQTY.Value) > currentLotQty)//���������������ɴ���currentLotQty
                        {
                            ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LotCreateQty_Over " + (int)currentLotQty));
                            this.ucLabelCreateQTY.TextFocus(false, true);
                            return;
                        }
                    }
                    if (this.ucLabelLotQty.Value.Trim() == "")//��ά����������
                    {
                        ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LotQty$Error_Input_Empty"));
                        this.ucLabelLotQty.TextFocus(false, true);
                        return;
                    }
                    if (this.ucLEStartLotNo.Value.Trim() == "")//��ά����ʼ���
                    {
                        ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LotStart$Error_Input_Empty"));
                        this.ucLEStartLotNo.TextFocus(false, true);
                        return;
                    }

                    //��ȡ������������ʱ��ȡ�Ľ���
                    NumberScale scale = NumberScale.Scale34;
                    string letters = string.Empty;
                    if (ultraOptionSetScale.CheckedIndex == 0)
                    {
                        letters = "0123456789";
                        scale = NumberScale.Scale10;
                    }
                    else if (ultraOptionSetScale.CheckedIndex == 1)
                    {
                        letters = "0123456789ABCDEF";
                        scale = NumberScale.Scale16; 
                    }
                    else if (ultraOptionSetScale.CheckedIndex == 2)
                    {
                        letters = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
                        scale = NumberScale.Scale34;
                    }

                    //����Ƿ����ѡ��Ľ���
                    for (int i = 0; i < this.ucLEStartLotNo.Value.Trim().Length; i++)
                    {
                        if (letters.IndexOf(this.ucLEStartLotNo.Value.Trim().Substring(i, 1)) < 0)
                        {
                            ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LotStart$Error_Scale"));
                            this.ucLEStartLotNo.TextFocus(false, true);
                            return;
                        }
                    }

                    //��ȡ�������볤��
                    int lotNoLength = int.Parse(this.ucLabelLotLength.Value.Trim());

                    long startSN = 0;//���ݽ������ɵ���ʼ����תΪʮ����
                    try
                    {
                        startSN = long.Parse(NumberScaleHelper.ChangeNumber(ucLEStartLotNo.Value.Trim(), scale, NumberScale.Scale10));
                    }
                    catch (Exception ex)
                    {
                        this.ShowMessage(ex);
                    }

                    int startLotNoLength = 0;
                    int endLotNoLenght = 0;                    

                    //��ʼ���ų���
                    startLotNoLength = this.ucLabelEditLotPrefix.Value.Trim().Length + this.ucLEStartLotNo.Value.Trim().Length;
                    //�����������ŵ�����
                    //int LotNoQty = GetQtyForLotNo((int)currentLotQty, int.Parse(this.ucLabelLotQty.Value.Trim()));
                    //ʵ��Ҫ���ɵ���������
                    createCount = GetQtyForLotNo(int.Parse(this.ucLabelCreateQTY.Value.Trim()), int.Parse(this.ucLabelLotQty.Value.Trim()));                    

                    //������ɵ����볤���Ƿ����Ҫ��
                    if (ultraOptionSetScale.CheckedIndex == 0)
                    {
                        //�������ų���                        
                        int length = (int.Parse(this.ucLEStartLotNo.Value.Trim()) + createCount - 1).ToString().Length;
                        endLotNoLenght = this.ucLabelEditLotPrefix.Value.Trim().Length + length;

                        if (lotNoLength < startLotNoLength || endLotNoLenght > lotNoLength)
                        {
                            ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LotNoLength_ERROR"));
                            this.ucLEStartLotNo.TextFocus(false, true);
                            return;
                        }

                    }
                    else if (ultraOptionSetScale.CheckedIndex == 1 || ultraOptionSetScale.CheckedIndex == 2)
                    {
                        //�������ų���
                        int length = NumberScaleHelper.ChangeNumber((startSN + createCount - 1).ToString(), NumberScale.Scale10, scale).Length;
                        endLotNoLenght = this.ucLabelEditLotPrefix.Value.Trim().Length + length;

                        if (lotNoLength < startLotNoLength || endLotNoLenght > lotNoLength)
                        {
                            ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LotNoLength_ERROR"));
                            this.ucLEStartLotNo.TextFocus(false, true);
                            return;
                        }

                    }

                    //����й���ѡ�������к�����Ϊ�ַ�,���ֺͿո񡱺͡����кų��ȡ�����Ҫ������������к��Ƿ���Ϲ���
                    if (this.checkBoxSNContentCheck.Checked)
                    {
                        string pattern = @"^([A-Za-z0-9]+[ ]*)*[A-Za-z0-9]+$";
                        string lotPrefix = this.ucLabelEditLotPrefix.Value.Trim();
                        Regex rex = new Regex(pattern, RegexOptions.IgnoreCase);
                        Match match = rex.Match(lotPrefix);

                        if (!match.Success)
                        {
                            ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_SNContent_CheckLotWrong $LotNo:" + this.ucLabelEditLotPrefix.Value.Trim().ToString()));
                            return;
                        }

                    }

                    //������ɵ��������ڵ���1000�������û����������ɵ����Ÿ�������1000�����п���Ӱ��ϵͳЧ�ʣ��Ƿ�������������û�ѡ���Ƿ����������
                    if (createCount > 1000)
                    {
                        DialogResult dr = MessageBox.Show(MutiLanguages.ParserMessage("$Generate_number_Is_Greate_One"), MutiLanguages.ParserMessage("$ShowMessage"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dr == DialogResult.No)
                        {
                            this.ucLabelCreateQTY.TextFocus(false, true);
                            return;
                        }
                    }

                    //10���ơ�16���ơ�34������������
                    List<string> LotNoList = CalcLotNoEnd(createCount);

                    //�������ɵ���ʼ���źͽ������ż���Ƿ��ڸù����ѹ��������������У������򱨴�
                    foreach (string lotNo in LotNoList)
                    {
                        object[] obj = _MOLotFacade.GetMO2LotLink(lotNo);
                        if (obj != null)
                        {
                            ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_LotNo_EXIST_MO2LOTLINK $LotNo:" + lotNo));
                            this.ucLEStartLotNo.TextFocus(false, true);
                            return;
                        }
                    }

                    this.DataProvider.BeginTransaction();
                    try
                    {
                        foreach (string lotNo in LotNoList)
                        {
                            MO2LotLink lotLink = new MO2LotLink();
                            lotLink.MOCode = currentMo;
                            lotLink.PrintTimes = 0;
                            lotLink.LastPrintDate = 0;
                            lotLink.LastPrintTime = 0;
                            lotLink.LastPrintUser = "";
                            lotLink.LotNo = lotNo;
                            if (lotNo.Equals(endLotNo) && remnant == 1)
                            {
                                lotLink.LotQty = int.Parse(this.ucLabelCreateQTY.Value.Trim()) % int.Parse(this.ucLabelLotQty.Value.Trim());
                            }
                            else
                            {
                                lotLink.LotQty = int.Parse(this.ucLabelLotQty.Value.Trim());
                            }                            
                            lotLink.LotStatus = LotStatusForMO2LotLink.LOTSTATUS_NEW;

                            lotLink.MUser = ApplicationService.Current().UserCode;
                            _MOLotFacade.AddMO2LotLink(lotLink);
                        }

                        this.DataProvider.CommitTransaction();
                        ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Success, "$CS_SUCCESS_CREATE_LOT"));

                        if (LotNoList.Count > 0)//add by Jarvis
                        {
                            LoadCheckListNewCreate(this.txtMoCodeEdit.Value.Trim(), LotNoList[0], LotNoList[LotNoList.Count - 1]);
                        }

                        return;

                    }
                    catch (Exception ex)
                    {
                        this.DataProvider.RollbackTransaction();
                        ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, ex.Message));
                        return;
                    }
                }
                else
                {
                    ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_MO_Not_Exist"));
                    return;
                }
            }
        }

        //��ӡ��ť
        private void ucButtonPrint_Click(object sender, EventArgs e)
        {
            //��ȡ��ӡģ���·��
            if (this.ucLabelComboxPrintTemplete.SelectedIndex < 0)
            {
                this.ShowMessage("$Error_NO_TempLeteSelect");
                return;
            }

            PrintLotNoList();

        }

        //ȡ����ť
        private void ucButtonExit_Click(object sender, EventArgs e)
        {
            this.txtMoCodeEdit.Value = "";
            //this.ucLabelComboxCheckType.Clear();
            this.checkBoxSNContentCheck.Enabled = true;
            this.checkBoxSNContentCheck.Checked = false;
            this.ucLabelEditLotPrefix.Value = "";
            this.ucLabelEditLotPrefix.Enabled = true;
            this.ucLEStartLotNo.Value = "";
            this.ucLabelLotQty.Value = "";
            this.ucLabelLotLength.Value = "";
            this.ucLabelLotLength.Checked = false;
            this.ucLabelLotLength.Enabled = true;
            this.ucLabelCreateQTY.Value = "";
            this.ultraOptionSetScale.Enabled = true;
            this.ultraOptionSetScale.CheckedIndex = 2;
            this.ucLabelMOPlanQty.Value = "";
            this.ucLabelMOPlanQty.Enabled = true;
                        
            ClearCheckList();//��հ󶨵�����
            this.ultraGridMain.UpdateData();
            SetControlIsEnable(false);//����ֻ��
            this.txtMoCodeEdit.TextFocus(false, true);
        }

        #endregion

        #region ������ż��ص�Grid

        #region Grid��ʼ���Լ���Ԫ�񵥻��¼�
        private void InitializeMainGrid()
        {
            this.m_CheckList = new DataSet();

            this.m_Mo2LotLink = new DataTable("MO2LotLink");
            this.m_Mo2LotLink.Columns.Add("Checked", typeof(string));
            this.m_Mo2LotLink.Columns.Add("MoCode", typeof(string));
            this.m_Mo2LotLink.Columns.Add("LotNo", typeof(string));
            this.m_Mo2LotLink.Columns.Add("LotQty", typeof(string));
            this.m_Mo2LotLink.Columns.Add("LotStatus", typeof(string));
            this.m_Mo2LotLink.Columns.Add("LotStatusHidden", typeof(string));
            this.m_Mo2LotLink.Columns.Add("PrintTimes", typeof(string));
            this.m_Mo2LotLink.Columns.Add("lastPrintUSER", typeof(string));
            this.m_Mo2LotLink.Columns.Add("lastPrintDate", typeof(string));
            this.m_Mo2LotLink.Columns.Add("lastPrintTime", typeof(string));
            this.m_Mo2LotLink.Columns.Add("MUser", typeof(string));
            this.m_Mo2LotLink.Columns.Add("MDate", typeof(string));
            this.m_Mo2LotLink.Columns.Add("MTime", typeof(string));
            this.m_CheckList.Tables.Add(this.m_Mo2LotLink);
            this.m_CheckList.AcceptChanges();
            this.ultraGridMain.DataSource = this.m_CheckList;
        }

        //��ʼ��Grid�Ļ���������Ϣ
        private void ultraGridMain_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // ����Ӧ�п�
            e.Layout.AutoFitColumns = false;
            e.Layout.Override.AllowColSizing = AllowColSizing.Free;
            e.Layout.ScrollBounds = ScrollBounds.ScrollToFill;

            //e.Layout.MaxBandDepth = 1;
            // �Զ��ж��Ƿ���ʾǰ���+��-��
            e.Layout.Override.ExpansionIndicator = ShowExpansionIndicator.CheckOnDisplay;

            // ����Grid��Split���ڸ�������������Ϊ1--������Split
            e.Layout.MaxColScrollRegions = 1;
            e.Layout.MaxRowScrollRegions = 1;

            // ������
            e.Layout.UseFixedHeaders = true;
            e.Layout.Override.FixedHeaderAppearance.BackColor = Color.LightYellow;
            e.Layout.Override.FixedHeaderAppearance.ForeColor = Color.Blue;
            e.Layout.Override.FixedCellAppearance.BackColor = Color.LightYellow;
            e.Layout.Override.FixedCellAppearance.ForeColor = Color.Blue;
            e.Layout.Override.FixedHeaderIndicator = FixedHeaderIndicator.None;

            // ����
            e.Layout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;

            // ������ɾ��
            e.Layout.Override.AllowDelete = DefaultableBoolean.False;

            // ������ʾ
            e.Layout.Bands[0].ScrollTipField = "MoCode";

            // �����п��������
            e.Layout.Bands[0].Columns["Checked"].Header.Caption = "";
            e.Layout.Bands[0].Columns["MoCode"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["LotNo"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["LotQty"].Header.Caption = "��������";
            e.Layout.Bands[0].Columns["LotStatus"].Header.Caption = "����״̬";
            e.Layout.Bands[0].Columns["LotStatusHidden"].Header.Caption = "����״̬";
            e.Layout.Bands[0].Columns["PrintTimes"].Header.Caption = "��ӡ����";
            e.Layout.Bands[0].Columns["lastPrintUSER"].Header.Caption = "����ӡ��";
            e.Layout.Bands[0].Columns["lastPrintDate"].Header.Caption = "�������";
            e.Layout.Bands[0].Columns["lastPrintTime"].Header.Caption = "���ʱ��";
            e.Layout.Bands[0].Columns["MUser"].Header.Caption = "ά����";
            e.Layout.Bands[0].Columns["MDate"].Header.Caption = "ά������";
            e.Layout.Bands[0].Columns["MTime"].Header.Caption = "ά��ʱ��";

            e.Layout.Bands[0].Columns["Checked"].Width = 40;
            e.Layout.Bands[0].Columns["MoCode"].Width = 100;
            e.Layout.Bands[0].Columns["LotNo"].Width = 100;
            e.Layout.Bands[0].Columns["LotQty"].Width = 60;
            e.Layout.Bands[0].Columns["LotStatus"].Width = 60;
            e.Layout.Bands[0].Columns["LotStatusHidden"].Width = 100;
            e.Layout.Bands[0].Columns["PrintTimes"].Width = 80;
            e.Layout.Bands[0].Columns["lastPrintUSER"].Width = 100;
            e.Layout.Bands[0].Columns["lastPrintDate"].Width = 100;
            e.Layout.Bands[0].Columns["lastPrintTime"].Width = 100;
            e.Layout.Bands[0].Columns["MUser"].Width = 100;
            e.Layout.Bands[0].Columns["MDate"].Width = 100;
            e.Layout.Bands[0].Columns["MTime"].Width = 100;


            // ������λ�Ƿ�����༭������λ����ʾ��ʽ
            e.Layout.Bands[0].Columns["Checked"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            e.Layout.Bands[0].Columns["MoCode"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly;
            e.Layout.Bands[0].Columns["LotNo"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["LotQty"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["LotStatus"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["LotStatusHidden"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["MUser"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["MDate"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["MTime"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["PrintTimes"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["lastPrintUSER"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["lastPrintDate"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["lastPrintTime"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            //���ÿɱ༭�е���ɫ
            //e.Layout.Bands[0].Columns["DATECODE"].CellAppearance.BackColor = Color.LawnGreen;
            //e.Layout.Bands[0].Columns["VenderLotNO"].CellAppearance.BackColor = Color.LawnGreen ;
            //e.Layout.Bands[0].Columns["VenderITEMCODE"].CellAppearance.BackColor = Color.LawnGreen;

            // ����ɸѡ
            e.Layout.Bands[0].Columns["MoCode"].AllowRowFiltering = DefaultableBoolean.True;
            e.Layout.Bands[0].Columns["MoCode"].SortIndicator = SortIndicator.Ascending;

            //������
            e.Layout.Bands[0].Columns["LotStatusHidden"].Hidden = true;

        }

        //Grid��Ԫ�񱻵����������¼�
        private void ultraGridMain_CellChange(object sender, CellEventArgs e)
        {
            this.ultraGridMain.UpdateData();
            if (e.Cell.Column.Key == "Checked")
            {
                if (e.Cell.Row.Band.Index == 0) //Parent
                {

                }

                if (e.Cell.Row.Band.Index == 1) // Child
                {
                    if (Convert.ToBoolean(e.Cell.Value) == true)
                    {
                        //e.Cell.Row.ParentRow.Cells["Checked"].Value = e.Cell.Value;
                    }
                }
            }
            this.ultraGridMain.UpdateData();
        }
        #endregion

        #region Grid���������Լ����
        //���ݹ�����������Ų�ѯ������������ص�Grid
        private void LoadCheckList(string lotNo, string mocode)
        {
            try
            {
                this.ClearCheckList();

                object[] mo2LotLinks = _MOLotFacade.QueryMO2LotLink(lotNo, mocode);
                DataRow rowMO2Lot;

                foreach (MO2LotLink item in mo2LotLinks)
                {
                    rowMO2Lot = this.m_CheckList.Tables["MO2LotLink"].NewRow();
                    rowMO2Lot["Checked"] = "true";
                    rowMO2Lot["MoCode"] = item.MOCode;
                    rowMO2Lot["LotNo"] = item.LotNo;
                    rowMO2Lot["LotQty"] = item.LotQty;
                    rowMO2Lot["LotStatus"] = MutiLanguages.ParserString(item.LotStatus);
                    rowMO2Lot["LotStatusHidden"] = item.LotStatus;
                    rowMO2Lot["PrintTimes"] = item.PrintTimes;
                    rowMO2Lot["lastPrintUSER"] = item.LastPrintUser;
                    rowMO2Lot["lastPrintDate"] = FormatHelper.ToDateString(item.LastPrintDate);
                    rowMO2Lot["lastPrintTime"] = FormatHelper.ToTimeString(item.LastPrintTime);
                    rowMO2Lot["MUser"] = item.MUser;
                    rowMO2Lot["MDate"] = FormatHelper.ToDateString(item.MaintainDate);
                    rowMO2Lot["MTime"] = FormatHelper.ToTimeString(item.MaintainTime);
                    this.m_CheckList.Tables["MO2LotLink"].Rows.Add(rowMO2Lot);
                }
                this.m_CheckList.Tables["MO2LotLink"].AcceptChanges();
                this.m_CheckList.AcceptChanges();
                this.ultraGridMain.DataSource = this.m_CheckList;
                this.ultraGridMain.UpdateData();
            }
            catch (Exception ex)
            {
            }
        }

        //���ݹ�������ʼ���źͽ������Ż�ȡ���Ž�������ص�Grid
        private void LoadCheckListNewCreate(string moCode, string beginLotNo, string endLotNo)
        {
            try
            {
                this.ClearCheckList();

                object[] mo2LotLinks = _MOLotFacade.GetMO2LotLinkByMoCode(moCode, beginLotNo, endLotNo, "");
                DataRow rowMO2Lot;

                foreach (MO2LotLink item in mo2LotLinks)
                {
                    rowMO2Lot = this.m_CheckList.Tables["MO2LotLink"].NewRow();
                    rowMO2Lot["Checked"] = "true";
                    rowMO2Lot["MoCode"] = item.MOCode;
                    rowMO2Lot["LotNo"] = item.LotNo;
                    rowMO2Lot["LotQty"] = item.LotQty;
                    rowMO2Lot["LotStatus"] = MutiLanguages.ParserString(item.LotStatus);
                    rowMO2Lot["LotStatusHidden"] = item.LotStatus;
                    rowMO2Lot["PrintTimes"] = item.PrintTimes;
                    rowMO2Lot["lastPrintUSER"] = item.LastPrintUser;
                    rowMO2Lot["lastPrintDate"] = FormatHelper.ToDateString(item.LastPrintDate);
                    rowMO2Lot["lastPrintTime"] = FormatHelper.ToTimeString(item.LastPrintTime);
                    rowMO2Lot["MUser"] = item.MUser;
                    rowMO2Lot["MDate"] = FormatHelper.ToDateString(item.MaintainDate);
                    rowMO2Lot["MTime"] = FormatHelper.ToTimeString(item.MaintainTime);
                    this.m_CheckList.Tables["MO2LotLink"].Rows.Add(rowMO2Lot);
                }
                this.m_CheckList.Tables["MO2LotLink"].AcceptChanges();
                this.m_CheckList.AcceptChanges();
                this.ultraGridMain.DataSource = this.m_CheckList;
                this.ultraGridMain.UpdateData();
            }
            catch (Exception ex)
            {
            }
        }

        //���Grid�󶨵�����Դ
        private void ClearCheckList()
        {
            if (this.m_CheckList == null)
            {
                return;
            }
            this.m_CheckList.Tables["MO2LotLink"].Rows.Clear();
            this.m_CheckList.Tables["MO2LotLink"].AcceptChanges();

            this.m_CheckList.AcceptChanges();
        }
        #endregion

        #endregion

        #region ��ӡ

        //���ش�ӡ���б� Jarvis 20120326
        private void LoadPrinter()
        {
            this.ucLabelComboxPrintList.Clear();

            if (System.Drawing.Printing.PrinterSettings.InstalledPrinters == null ||
                System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count == 0)
            {
                this.ShowMessage(new UserControl.Message(MessageType.Error, "$CS_PleaseInstallPrinter"));

                return;
            }

            int defaultprinter = 0;
            for (int i = 0; i < System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count; i++)
            {
                this.ucLabelComboxPrintList.AddItem(System.Drawing.Printing.PrinterSettings.InstalledPrinters[i], System.Drawing.Printing.PrinterSettings.InstalledPrinters[i]);
                System.Drawing.Printing.PrinterSettings pts = new System.Drawing.Printing.PrinterSettings();
                pts.PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters[i];
                if (pts.IsDefaultPrinter)//��¼Ĭ�ϴ�ӡ����������
                {
                    defaultprinter = i;
                }
            }
            this.ucLabelComboxPrintList.SelectedIndex = defaultprinter;//ѡ��Ĭ�ϵĴ�ӡ��
        }

        //��ӡģ��
        private void LoadTemplateList()
        {

            this.ucLabelComboxPrintTemplete.Clear();

            object[] objs = this.LoadTemplateListDataSource();
            if (objs == null)
            {
                this.ShowMessage("$CS_No_Data_To_Display");
                return;
            }

            _PrintTemplateList = new PrintTemplate[objs.Length];

            for (int i = 0; i < objs.Length; i++)
            {
                _PrintTemplateList[i] = (PrintTemplate)objs[i];

                ucLabelComboxPrintTemplete.AddItem(_PrintTemplateList[i].TemplateName, _PrintTemplateList[i]);

            }
        }

        //��ӡģ������Դ���������д�ӡģ�棬Jarvis 20120326
        private object[] LoadTemplateListDataSource()
        {
            try
            {
                PrintTemplateFacade printTemplateFacade = new PrintTemplateFacade(this.DataProvider);
                return printTemplateFacade.QueryPrintTemplate(string.Empty, string.Empty, int.MinValue, int.MaxValue);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }

            return null;
        }

        //��ӡǰ�������
        private bool ValidateInput(string printer, PrintTemplate printTemplate)
        {
            if (this.ucLabelEditPrintCount.Value.Trim() == "")
            {
                this.ShowMessage(new UserControl.Message(MessageType.Error, "$Error_Print_Count_Empty"));
                return false;
            }

            //ģ��
            if (printTemplate == null)
            {
                this.ShowMessage(new UserControl.Message(MessageType.Error, "$Error_PrintTemplate_Empty"));
                return false;
            }

            //��ӡ��
            if (printer == null || printer.Trim().Length <= 0)
            {
                this.ShowMessage(new UserControl.Message(MessageType.Error, "$Error_Printer_Empty"));
                return false;
            }

            return true;
        }

        //���������ӡ����
        private void PrintLotNoList()
        {
            try
            {
                if (this.ucLabelComboxPrintList.ComboBoxData.Items.Count == 0)
                {
                    this.ShowMessage(new UserControl.Message(MessageType.Error, "$CS_PleaseInstallPrinter"));
                    return;
                }                

                SetPrintButtonStatus(false);//��ʱ���ð�ť����ֹ��ε��

                PrintTemplateFacade printTemplateFacade = new PrintTemplateFacade(this.DataProvider);
                string printer = this.ucLabelComboxPrintList.SelectedItemText;

                PrintTemplate printTemplate = (PrintTemplate)this.ucLabelComboxPrintTemplete.SelectedItemValue;
                printTemplate = (PrintTemplate)printTemplateFacade.GetPrintTemplate(printTemplate.TemplateName);

                List<MO2LotLink> mo2LotLinkList = new List<MO2LotLink>();
                for (int i = 0; i < ultraGridMain.Rows.Count; i++)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = ultraGridMain.Rows[i];

                    if (bool.Parse(row.Cells["Checked"].Value.ToString()))
                    {
                        object obj = _MOLotFacade.GetMO2LotLink(row.Cells["LotNo"].Value.ToString(), row.Cells["MoCode"].Value.ToString());
                        mo2LotLinkList.Add(obj as MO2LotLink);
                    }

                }

                if (mo2LotLinkList.Count == 0)
                {
                    return;
                }

                if (!ValidateInput(printer, printTemplate))
                {
                    return;
                }

                for (int i = 0; i < int.Parse(this.ucLabelEditPrintCount.Value.Trim()); i++)
                {
                    Messages msg = this.Print(printer, printTemplate.TemplatePath, mo2LotLinkList);

                    if (msg.IsSuccess())
                    {
                        //��ӡ������ݴ���
                        try
                        {
                            string userCode = ApplicationService.Current().UserCode;
                            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

                            this.DataProvider.BeginTransaction();
                            foreach (MO2LotLink lotLink in mo2LotLinkList)
                            {
                                lotLink.PrintTimes++;
                                lotLink.LastPrintUser = userCode;
                                lotLink.LastPrintDate = dbDateTime.DBDate;
                                lotLink.LastPrintTime = dbDateTime.DBTime;

                                _MOLotFacade.UpdateMO2LotLink(lotLink);
                            }
                            this.DataProvider.CommitTransaction();
                        }
                        catch (System.Exception ex)
                        {
                            this.DataProvider.RollbackTransaction();

                            this.ShowMessage(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                            return;
                        }
                    }

                    this.ShowMessage(msg);
                }

                ////LotNo��Ҫʹ��GetSourceLot����ת���ɳ�ʼ��������
                //string lotNo = string.Empty;
                //if (this.ucLabelLotQuery.Value.Trim() != "")
                //{
                //    lotNo = _DataCollectFacade.GetSourceLot(this.ucLabelLotQuery.Value.Trim().ToUpper(), string.Empty);
                //}

                LoadCheckList(this.ucLabelLotQuery.Value.Trim(), this.txtMoCodeQuery.Value.Trim());

            }
            finally
            {
                SetPrintButtonStatus(true);
            }
        }

        //��ӡ
        public UserControl.Messages Print(string printer, string templatePath, List<MO2LotLink> mo2LotLink)
        {
            UserControl.Messages messages = new UserControl.Messages();
            CodeSoftFacade _CodeSoftFacade = new CodeSoftFacade();
            CodeSoftPrintFacade _CodeSoftPrintFacade = new CodeSoftPrintFacade(this.DataProvider);
            try
            {
                try
                {
                    _CodeSoftPrintFacade.PrePrint();
                    _CodeSoftFacade.OpenTemplate(printer, templatePath);
                }
                catch (System.Exception ex)
                {
                    messages.Add(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                    return messages;
                }

                //������ӡǰ�����ı��ļ�
                string strBatchDataFile = string.Empty;
                if (_IsBatchPrint)
                {
                    strBatchDataFile = _CodeSoftPrintFacade.CreateFile();
                }

                Hashtable ht = new Hashtable();

                for (int i = 0; i < mo2LotLink.Count; i++)
                {
                    MO2LotLink lotLink = (MO2LotLink)mo2LotLink[i];

                    if (!ht.ContainsKey(lotLink.MOCode))
                    {
                        MO mo = _MOFacade.GetMO(lotLink.MOCode) as MO;
                        Item item = _ItemFacade.GetItem(mo.ItemCode, mo.OrganizationID) as Item;
                        ht.Add(lotLink.MOCode, item);
                    }

                    LabelPrintVars labelPrintVars = new LabelPrintVars();

                    string[] vars = new string[0];

                    if (messages.IsSuccess())
                    {
                        try
                        {
                            //Ҫ����Codesoft�����飬�ֶ�˳�����޸�
                            vars = _CodeSoftPrintFacade.GetPrintVars(lotLink.LotNo, (ht[lotLink.MOCode] as Item).ItemCode, (ht[lotLink.MOCode] as Item).ItemName, lotLink.MOCode, "", "");

                            //������ӡǰ��д�ļ�
                            if (_IsBatchPrint)
                            {
                                string[] printVars = _CodeSoftPrintFacade.ProcessVars(vars, labelPrintVars);
                                _CodeSoftPrintFacade.WriteFile(strBatchDataFile, printVars);
                            }
                            //ֱ�Ӵ�ӡ
                            else
                            {
                                _CodeSoftFacade.LabelPrintVars = labelPrintVars;
                                _CodeSoftFacade.Print(vars);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            messages.Add(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                            return messages;
                        }
                    }
                }

                //������ӡ
                if (_IsBatchPrint)
                {
                    try
                    {
                        _CodeSoftFacade.Print(strBatchDataFile, _CodeSoftPrintFacade.GetDataDescPath(_DataDescFileName));
                    }
                    catch (System.Exception ex)
                    {
                        messages.Add(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                        return messages;
                    }
                }

                messages.Add(new UserControl.Message(UserControl.MessageType.Success, "$Success_Print_Label"));
            }
            finally
            {
                _CodeSoftFacade.ReleaseCom();
            }

            return messages;
        }

        //���ô�ӡ��ť״̬
        private void SetPrintButtonStatus(bool enabled)
        {
            this.ucButtonPrint.Enabled = enabled;

            if (enabled)
            {
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
            else
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            }
        }
        
        //δ��ӡѡ�ѡ�¼�
        private void checkBoxNoPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxNoPrint.Checked)
            {
                for (int i = 0; i < this.ultraGridMain.Rows.Count; i++)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = ultraGridMain.Rows[i];
                    if (row.Cells["PrintTimes"].Value.ToString() == "0")
                    {
                        row.Cells["Checked"].Value = true;
                    }
                    else
                    {
                        row.Cells["Checked"].Value = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.ultraGridMain.Rows.Count; i++)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = ultraGridMain.Rows[i];
                    row.Cells["Checked"].Value = false;
                }
            }

            this.ultraGridMain.UpdateData();
        }

        #endregion

        #region �����س�����ز���
        private void txtMoCodeEdit_TxtboxKeyPress(object sender, KeyPressEventArgs e)
        {
            //����������鹤����Ϣ���������ù��������Ĳ�Ʒ�ķ����趨
            if (e.KeyChar == '\r')
            {
                currentMo = this.txtMoCodeEdit.Value.Trim();
                //��鹤���Ƿ���ڣ���״̬��Ϊ�ر�
                object objMo = this._MOFacade.GetMO(currentMo);

                if (objMo != null)
                {
                    if ((objMo as MO).MOStatus == MOStatus.Close)
                    {
                        Clear();
                        ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_ERROR_MO_COLSE"));
                        this.txtMoCodeEdit.TextFocus(false, true);
                        return;
                    }
                    else
                    {
                        //������Ʒ�����趨����Ϣ
                        currentItemCode = (objMo as MO).ItemCode;
                        if (!Init_CheckType(currentItemCode))//���δά����Ʒ������Ϣ��ʾ������
                        {
                            Clear();
                            ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_ITEM2LOT_NOT_EXIST"));
                            this.txtMoCodeEdit.TextFocus(false, true);
                            return;
                        }

                        //�������������������Ϣ������¼
                        currentLotQty = CalcCreatedLotQty(currentMo, (objMo as MO).MOPlanQty);
                        this.ucLabelMOPlanQty.Value = (objMo as MO).MOPlanQty.ToString();
                        this.ucLabelCreateQTY.Value = ((int)currentLotQty).ToString();
                        this.ucLabelCreateQTY.TextFocus(false, true);
                    }
                }
                else
                {
                    Clear();                    
                    ApplicationRun.GetInfoForm().Add(new UserControl.Message(MessageType.Error, "$CS_MO_Not_Exist"));
                    this.txtMoCodeEdit.TextFocus(false, true);
                    return;
                }

            }
        }

        //��ʼ����Ʒ�������ŵ���Ϣ
        private bool Init_CheckType(string itemCode)
        {
            object obj = _ItemLotFacade.GetItem2LotCheck(currentItemCode);

            if (obj != null)
            {
                Item2LotCheck item2LotCheck = obj as Item2LotCheck;
                if (item2LotCheck.SNContentCheck == "Y")
                {
                    this.checkBoxSNContentCheck.Checked = true;
                }
                else
                {
                    this.checkBoxSNContentCheck.Checked = false;
                }

                if (item2LotCheck.CreateType.Equals(CreateType.CREATETYPE_DECIMAL))
                {
                    this.ultraOptionSetScale.CheckedIndex = 0;
                }
                else if (item2LotCheck.CreateType.Equals(CreateType.CREATETYPE_HEXADECIMAL))
                {
                    this.ultraOptionSetScale.CheckedIndex = 1;
                }
                else if (item2LotCheck.CreateType.Equals(CreateType.CREATETYPE_THIRTYFOUR))
                {
                    this.ultraOptionSetScale.CheckedIndex = 2;
                }

                this.ucLabelLotLength.Value = item2LotCheck.SNLength.ToString();
                this.ucLabelEditLotPrefix.Value = item2LotCheck.SNPrefix.ToString();
                SetControlIsEnable(false);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void Clear()
        {
            currentItemCode = string.Empty;
            currentLotQty = 0;
            //this.ucLabelComboxCheckType.Clear();

            this.checkBoxSNContentCheck.Enabled = true;
            this.checkBoxSNContentCheck.Checked = false;
            this.ucLabelEditLotPrefix.Value = "";
            this.ucLEStartLotNo.Value = "";
            this.ucLabelLotQty.Value = "";
            this.ucLabelLotLength.Value = "";
            this.ucLabelLotLength.Checked = false;
            this.ucLabelLotLength.Enabled = true;
            this.ucLabelCreateQTY.Value = "";
            this.ultraOptionSetScale.Enabled = true;
            this.ultraOptionSetScale.CheckedIndex = 2;
            this.ucLabelMOPlanQty.Value = "";
            SetControlIsEnable(false);//����ֻ��
        }

        //�趨�Ƿ�����޸���Ϣ
        private void SetControlIsEnable(bool isEnable)
        {
            this.ucLabelEditLotPrefix.Enabled = isEnable;
            //this.ucLabelLotLength.Checked = (!isEnable);
            this.ucLabelLotLength.Enabled = isEnable;

            this.checkBoxSNContentCheck.Enabled = isEnable;
            this.ultraOptionSetScale.Enabled = isEnable;
            this.ucLabelMOPlanQty.Enabled = isEnable;
        }

        private decimal CalcCreatedLotQty(string moCode, decimal moPlanQty)
        {
            decimal actLotQty = _MOLotFacade.SumLotQtyByMoCode(moCode);
            return moPlanQty - actLotQty > 0 ? moPlanQty - actLotQty : 0;
        }
        #endregion

        #region �ж��Ƿ�ѡ�˼�¼
        private bool CheckISSelectRow()
        {
            bool flag = false;

            //���ж�Grid�е������Ƿ���ѡ��
            for (int i = 0; i < this.ultraGridMain.Rows.Count; i++)
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow row = ultraGridMain.Rows[i];
                if (Convert.ToBoolean(row.Cells["Checked"].Value) == true)
                {
                    flag = true;
                }
            }

            return flag;
        }
        #endregion

        #region ����10��16��34��������
        private List<string> CalcLotNoEnd(long count)
        {
            List<string> lotNoList = new List<string>();

            NumberScale scale = NumberScale.Scale34;
            if (ultraOptionSetScale.CheckedIndex == 0)
                scale = NumberScale.Scale10;
            else if (ultraOptionSetScale.CheckedIndex == 1)
                scale = NumberScale.Scale16;
            else if (ultraOptionSetScale.CheckedIndex == 2)
                scale = NumberScale.Scale34;

            long startSN = 0;
            try
            {
                startSN = long.Parse(NumberScaleHelper.ChangeNumber(ucLEStartLotNo.Value.Trim(), scale, NumberScale.Scale10));
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }

            long endSN = startSN + count - 1;

            try
            {
                for (long i = startSN; i <= endSN; i++)
                {
                    string lotNoEnd = NumberScaleHelper.ChangeNumber(i.ToString(), NumberScale.Scale10, scale);
                    lotNoEnd = lotNoEnd.PadLeft((int.Parse(this.ucLabelLotLength.Value.Trim()) - this.ucLabelEditLotPrefix.Value.Trim().Length), '0');
                    endLotNo = this.ucLabelEditLotPrefix.Value.Trim() + lotNoEnd;
                    lotNoList.Add(endLotNo);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }

            return lotNoList;
        }
        #endregion

        #region ��ñ�����Ҫ���ɵ�����
        private int GetQtyForLotNo(int createQty, int lotQty)
        {
            remnant = (createQty % lotQty > 0 ? 1 : 0);
            return (createQty / lotQty) + remnant;
        }
        #endregion

    }

}

