using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.Performance;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Performance;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.BaseSetting
{
    /// <summary>
    /// FShiftMP ��ժҪ˵����
    /// </summary>
    public partial class FExceptionMP : BaseMPageNew
    {
        public TextBox DateEdit;
        public TextBox DateQuery;
        public BenQGuru.eMES.Web.UserControl.eMESTime BeginTimeEdit;
        public BenQGuru.eMES.Web.UserControl.eMESTime EndTimeEdit;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        private PerformanceFacade _facade = null;
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
            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                DBDateTime dbDateTime=FormatHelper.GetNowDBDateTime(this.DataProvider);
                this.DateEdit.Text = dbDateTime.DateTime.ToString("yyyy-MM-dd");                
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        #endregion

        #region WebGrid
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("Serial", "", null);
            this.gridHelper.AddColumn("sscode", "���ߴ���", null);
            this.gridHelper.AddColumn("Date", "����", null);
            this.gridHelper.AddColumn("ShiftCode", "��δ���", null);
            this.gridHelper.AddColumn("ItemCode", "���ϴ���", null);
            this.gridHelper.AddColumn("StartDateTime", "��ʼʱ��", null);
            this.gridHelper.AddColumn("EndDateTime", "����ʱ��", null);
            this.gridHelper.AddColumn("ExceptionCode", "�쳣�¼�����", null);
            this.gridHelper.AddColumn("Memo", "��ע", null);
            this.gridHelper.AddColumn("ComfirmMEMO", "ȷ�ϱ�ע", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridHelper.AddDefaultColumn(true, true);
            this.gridWebGrid.Columns.FromKey("Serial").Hidden = true;


            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override DataRow GetGridRow(object obj)
        {
           
            DataRow row = this.DtSource.NewRow();
            row["Serial"] = ((ExceptionEvent)obj).Serial.ToString();
            row["sscode"] = ((ExceptionEvent)obj).SSCode.ToString();
            row["Date"] = FormatHelper.ToDateString(((ExceptionEvent)obj).ShiftDate);
            row["ShiftCode"] = ((ExceptionEvent)obj).ShiftCode.ToString();
            row["ItemCode"] = ((ExceptionEvent)obj).ItemCode.ToString();
            row["StartDateTime"] = FormatHelper.ToTimeString(((ExceptionEvent)obj).BeginTime);
            row["EndDateTime"] = FormatHelper.ToTimeString(((ExceptionEvent)obj).EndTime);
            row["ExceptionCode"] = ((ExceptionEvent)obj).ExceptionCode.ToString();
            row["Memo"] = ((ExceptionEvent)obj).Memo.ToString();
            row["ComfirmMEMO"] = ((ExceptionEvent)obj).ComfirmMemo.ToString();
            row["MaintainUser"] = ((ExceptionEvent)obj).MaintainUser.ToString();
            row["MaintainDate"] = FormatHelper.ToDateString(((ExceptionEvent)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((ExceptionEvent)obj).MaintainTime);
            return row;

        }


        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            return this._facade.QueryExceptionEvent(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSSQuery.Text)),
                FormatHelper.TODateInt(this.DateQuery.Text),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtShiftCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtExceptionCodeQuery.Text)),
                inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            return this._facade.GetExceptionEventCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSSQuery.Text)),
                FormatHelper.TODateInt(this.DateQuery.Text),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtShiftCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtExceptionCodeQuery.Text)));
        }

        #endregion

        #region Button
        protected override void AddDomainObject(object domainObject)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }

            this._facade.AddExceptionEvent((ExceptionEvent)domainObject);
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            this._facade.DeleteExceptionEvent((ExceptionEvent[])domainObjects.ToArray(typeof(ExceptionEvent)));
        }

        protected override void UpdateDomainObject(object domainObject)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            this._facade.UpdateExceptionEvent((ExceptionEvent)domainObject);
        }

        protected override void buttonHelper_AfterPageStatusChangeHandle(string pageAction)
        {

        }
        #endregion

        #region Object <--> Page
        protected override object GetEditObject()
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }

            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            ExceptionEvent exceptionEvent = this._facade.CreateNewExceptionEvent();

            exceptionEvent.Serial = this.txtSerial.Text == string.Empty ? 0 : int.Parse(this.txtSerial.Text);
            exceptionEvent.ShiftDate = FormatHelper.TODateInt(this.DateEdit.Text);
            exceptionEvent.SSCode = FormatHelper.CleanString(this.txtSSEdit.Text.ToUpper());
            exceptionEvent.ShiftCode = FormatHelper.CleanString(this.txtShiftCodeEdit.Text.ToUpper());
            exceptionEvent.ItemCode = FormatHelper.CleanString(this.txtItemCodeEdit.Text.ToUpper());
            exceptionEvent.BeginTime = FormatHelper.TOTimeInt(this.BeginTimeEdit.TimeString);
            exceptionEvent.EndTime = FormatHelper.TOTimeInt(this.EndTimeEdit.TimeString);
            exceptionEvent.ExceptionCode = FormatHelper.CleanString(this.txtExceptionCodeEdit.Text.ToUpper());
            exceptionEvent.Memo = FormatHelper.CleanString(this.txtMemoEdit.Text.ToUpper());
            exceptionEvent.ComfirmMemo = FormatHelper.CleanString(this.txtComfirmMEMOEdit.Text.ToUpper());
            exceptionEvent.MaintainUser = this.GetUserCode();
            exceptionEvent.MaintainDate = dbDateTime.DBDate;
            exceptionEvent.MaintainTime = dbDateTime.DBTime;

            return exceptionEvent;
        }


        protected override object GetEditObject(GridRecord row)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            object obj = _facade.GetExceptionEvent(int.Parse(row.Items.FindItemByKey("Serial").Text.ToString()));

            if (obj != null)
            {
                return obj as ExceptionEvent;
            }

            return null;
        }

        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.txtSerial.Text = string.Empty;
                this.txtSSEdit.Text = string.Empty;
                this.txtShiftCodeEdit.Text = string.Empty;
                this.txtItemCodeEdit.Text = string.Empty;
                this.txtExceptionCodeEdit.Text = string.Empty;
                this.txtMemoEdit.Text = string.Empty;
                this.txtComfirmMEMOEdit.Text = string.Empty;
                this.BeginTimeEdit.TimeString = string.Empty;
                this.EndTimeEdit.TimeString = string.Empty;

                return;
            }

            this.txtSerial.Text = ((ExceptionEvent)obj).Serial.ToString();
            this.DateEdit.Text = FormatHelper.ToDateString(((ExceptionEvent)obj).ShiftDate);
            this.txtShiftCodeEdit.Text = ((ExceptionEvent)obj).ShiftCode.ToString();
            this.txtSSEdit.Text = ((ExceptionEvent)obj).SSCode.ToString();
            this.txtItemCodeEdit.Text = ((ExceptionEvent)obj).ItemCode.ToString();
            this.txtExceptionCodeEdit.Text = ((ExceptionEvent)obj).ExceptionCode.ToString();
            this.txtMemoEdit.Text = ((ExceptionEvent)obj).Memo.ToString();
            this.txtComfirmMEMOEdit.Text = ((ExceptionEvent)obj).ComfirmMemo.ToString();
            this.BeginTimeEdit.TimeString = FormatHelper.ToTimeString(((ExceptionEvent)obj).BeginTime);
            this.EndTimeEdit.TimeString = FormatHelper.ToTimeString(((ExceptionEvent)obj).EndTime);
        }

        protected override bool ValidateInput()
        {
            PageCheckManager manager = new PageCheckManager();

            manager.Add(new LengthCheck(lblSSEdit, txtSSEdit, 40, true));
            manager.Add(new DateCheck(lblDateQuery, DateEdit.Text, true));
            manager.Add(new LengthCheck(lblShiftCodeEdit, txtShiftCodeEdit, 40, true));
            manager.Add(new LengthCheck(lblItemCodeEdit, txtItemCodeEdit, 40, true));
            manager.Add(new LengthCheck(lblExceptionCodeEdit, txtExceptionCodeEdit, 40, true));
            manager.Add(new LengthCheck(lblMemoEdit, txtMemoEdit, 500, false));
            manager.Add(new LengthCheck(lblComfirmMEMOEdit, txtComfirmMEMOEdit, 500, false));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }

            //if (Convert.ToDateTime(this.BeginTimeEdit.TimeString) > Convert.ToDateTime(this.EndTimeEdit.TimeString))
            //{
            //    WebInfoPublish.Publish(this, "$Error_BeginTime_Biggger_EndTime", this.languageComponent1);
            //    return false;
            //}

            ShiftModelFacade shiftModelFacade = new ShiftModelFacade(this.DataProvider);
            object shiftObject = shiftModelFacade.GetShift(FormatHelper.CleanString(this.txtShiftCodeEdit.Text.ToUpper()));
            if (shiftObject == null)
            {
                WebInfoPublish.Publish(this, "$Error_Shift_Not_Exist", this.languageComponent1);
                return false;
            }

            ItemFacade itemFacade = new ItemFacade(this.DataProvider);
            object itemObject = itemFacade.GetItem(FormatHelper.CleanString(this.txtItemCodeEdit.Text.ToUpper()), GlobalVariables.CurrentOrganizations.First().OrganizationID);
            if (itemObject == null)
            {
                WebInfoPublish.Publish(this, "$Error_CS_No_OPBOMDetail", this.languageComponent1);
                return false;
            }

            BaseModelFacade baseModelFacade = new BaseModelFacade(this.DataProvider);
            object stepSequenceObject = baseModelFacade.GetStepSequence(FormatHelper.CleanString(this.txtSSEdit.Text.ToUpper()));
            if (stepSequenceObject == null)
            {
                WebInfoPublish.Publish(this, "$CS_STEPSEQUENCE_NOT_EXIST", this.languageComponent1);
                return false;
            }

            if (((Shift)shiftObject).ShiftTypeCode!=((StepSequence)stepSequenceObject).ShiftTypeCode)
            {
                WebInfoPublish.Publish(this, "$CS_STEPSEQUENCE_NOT_Math_ShiftCode", this.languageComponent1);
                return false;
            }

            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }

            object exceptionCodeObject = _facade.GetExceptionCode(FormatHelper.CleanString(this.txtExceptionCodeEdit.Text.ToUpper()));
            if (exceptionCodeObject == null)
            {
                WebInfoPublish.Publish(this, "$ExceptionCode_NOT_EXIST", this.languageComponent1);
                return false;
            }

            return true;
        }

        #endregion


        #region Export
        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{((ExceptionEvent)obj).SSCode.ToString(),
                                FormatHelper.ToDateString(((ExceptionEvent)obj).ShiftDate),
								((ExceptionEvent)obj).ShiftCode.ToString(),
					            ((ExceptionEvent)obj).ItemCode.ToString(),			
                                FormatHelper.ToTimeString(((ExceptionEvent)obj).BeginTime),
	                            FormatHelper.ToTimeString(((ExceptionEvent)obj).EndTime),
                                ((ExceptionEvent)obj).ExceptionCode.ToString(),			
                                ((ExceptionEvent)obj).Memo.ToString(),
	                            ((ExceptionEvent)obj).ComfirmMemo.ToString(),
                                ((ExceptionEvent)obj).MaintainUser.ToString(),
								FormatHelper.ToDateString(((ExceptionEvent)obj).MaintainDate),
								FormatHelper.ToTimeString(((ExceptionEvent)obj).MaintainTime)
                                };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	
									"sscode",
									"Date",
                                    "ShiftCode",
                                    "ItemCode",
                                    "StartDateTime",
                                    "EndDateTime",
                                    "ExceptionCode",
                                    "Memo",
                                    "ComfirmMEMO",
									"MaintainUser",
									"MaintainDate",
									"MaintainTime"
                                };
        }

        #endregion
    }
}
