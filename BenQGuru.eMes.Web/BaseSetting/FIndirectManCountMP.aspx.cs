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
    public partial class FIndirectManCountMP : BaseMPageNew
    {
        public TextBox DateEdit;
        public TextBox DateQuery;
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
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }


        protected void drpFirstClass_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ItemFacade itemFacade = new ItemFacade(this.DataProvider);

                DropDownListBuilder builder = new DropDownListBuilder(this.drpFirstClassQuery);

                builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate(itemFacade.GetItemFirstClass);

                builder.Build("FirstClass", "FirstClass");

                this.drpFirstClassQuery.Items.Insert(0, new ListItem("", ""));
            }
        }

        protected void drpFirstClassGroup_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ItemFacade itemFacade = new ItemFacade(this.DataProvider);

                DropDownListBuilder builder = new DropDownListBuilder(this.drpFirstClassEdit);

                builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate(itemFacade.GetItemFirstClass);

                builder.Build("FirstClass", "FirstClass");

                this.drpFirstClassEdit.Items.Insert(0, new ListItem("", ""));
            }
        }
        #endregion

        #region WebGrid
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("Date", "����", null);
            this.gridHelper.AddColumn("ShiftCode", "��δ���", null);
            this.gridHelper.AddColumn("ShiftDescription", "�������", 90);
            this.gridHelper.AddColumn("CrewCode", "�������", null);
            this.gridHelper.AddColumn("CrewDesc", "��������", null);
            this.gridHelper.AddColumn("FacCode", "�������", null);
            this.gridHelper.AddColumn("FacDesc", "��������", 90);
            this.gridHelper.AddColumn("FirstClass", "һ������", null);
            this.gridHelper.AddColumn("ManCount", "����", null);
            this.gridHelper.AddColumn("Duration", "ʱ��(Сʱ)", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", 100);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);
            
            this.gridHelper.AddDefaultColumn(true, true);
       

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override DataRow GetGridRow(object obj)
        {
        
            DataRow row = this.DtSource.NewRow();
            row["Date"] = FormatHelper.ToDateString(((IndirectManCountWithMessage)obj).ShiftDate);
            row["ShiftCode"] = ((IndirectManCountWithMessage)obj).ShiftCode.ToString();
            row["ShiftDescription"] = ((IndirectManCountWithMessage)obj).ShiftDescription.ToString();
            row["CrewCode"] = ((IndirectManCountWithMessage)obj).CrewCode.ToString();
            row["CrewDesc"] = ((IndirectManCountWithMessage)obj).CrewDesc.ToString();
            row["FacCode"] = ((IndirectManCountWithMessage)obj).FactoryCode.ToString();
            row["FacDesc"] =((IndirectManCountWithMessage)obj).FacDesc.ToString() ;
            row["FirstClass"] = ((IndirectManCountWithMessage)obj).FirstClass.ToString();
            row["ManCount"] = ((IndirectManCountWithMessage)obj).ManCount.ToString();
            row["Duration"] = Convert.ToString(Math.Round((Convert.ToDecimal(((IndirectManCountWithMessage)obj).Duration) / 3600), 2)) == "0.00" ? "0" : Convert.ToString(Math.Round((Convert.ToDecimal(((IndirectManCountWithMessage)obj).Duration) / 3600), 2));
            row["MaintainUser"] = ((IndirectManCountWithMessage)obj).MaintainUser.ToString();
            row["MaintainDate"] = FormatHelper.ToDateString(((IndirectManCountWithMessage)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((IndirectManCountWithMessage)obj).MaintainTime);
            return row;

        }


        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            return this._facade.QueryIndirectManCount(
                FormatHelper.TODateInt(this.DateQuery.Text),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtShiftCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCrewCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtFACCodeQuery.Text)),
                this.drpFirstClassQuery.SelectedValue,
                inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            return this._facade.GetIndirectManCountCount(
                FormatHelper.TODateInt(this.DateQuery.Text),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtShiftCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCrewCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtFACCodeQuery.Text)),
                this.drpFirstClassQuery.SelectedValue);
        }

        #endregion

        #region Button
        protected override void AddDomainObject(object domainObject)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }

            IndirectManCount indirectManCount = (IndirectManCount)_facade.GetIndirectManCount(FormatHelper.TODateInt(this.DateEdit.Text),
                                                                                            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtShiftCodeEdit.Text)),
                                                                                            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCrewCodeEdit.Text)),
                                                                                            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtFACCodeEdit.Text)),
                                                                                            this.drpFirstClassEdit.SelectedValue);
            if (indirectManCount != null)
            {
                WebInfoPublish.Publish(this, "$The_Same_Date_Is_Exist", this.languageComponent1);
                return;
            }
            this._facade.AddIndirectManCount((IndirectManCount)domainObject);
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            this._facade.DeleteIndirectManCount((IndirectManCount[])domainObjects.ToArray(typeof(IndirectManCount)));
        }

        protected override void UpdateDomainObject(object domainObject)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            this._facade.UpdateIndirectManCount((IndirectManCount)domainObject);
        }

        protected override void buttonHelper_AfterPageStatusChangeHandle(string pageAction)
        {
            if (pageAction == PageActionType.Add)
            {
                this.DateEdit.ReadOnly = false;
                this.DateEdit.Enabled = true;
                this.txtShiftCodeEdit.Readonly = false;
                this.txtCrewCodeEdit.Readonly = false;
                this.txtFACCodeEdit.Readonly = false;
                this.drpFirstClassEdit.Enabled = true;
            }

            if (pageAction == PageActionType.Update)
            {
                this.DateEdit.ReadOnly = true;
                this.DateEdit.Enabled = false;
                this.txtShiftCodeEdit.Readonly = true;
                this.txtCrewCodeEdit.Readonly = true;
                this.txtFACCodeEdit.Readonly = true;
                this.drpFirstClassEdit.Enabled = false;
            }
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
            IndirectManCount indirectManCount = this._facade.CreateNewIndirectManCount();

            indirectManCount.ShiftDate = FormatHelper.TODateInt(this.DateEdit.Text);
            indirectManCount.ShiftCode = FormatHelper.CleanString(this.txtShiftCodeEdit.Text.ToUpper());
            indirectManCount.CrewCode = FormatHelper.CleanString(this.txtCrewCodeEdit.Text.ToUpper());
            indirectManCount.FactoryCode = FormatHelper.CleanString(this.txtFACCodeEdit.Text.ToUpper(), 40);
            indirectManCount.FirstClass = this.drpFirstClassEdit.SelectedValue;
            indirectManCount.ManCount = int.Parse(this.txtManCountEdit.Text.Trim());
            indirectManCount.Duration = Convert.ToInt32(Convert.ToDecimal(this.txtDurationEdit.Text.Trim())*3600);
            indirectManCount.MaintainUser = this.GetUserCode();
            indirectManCount.MaintainDate = dbDateTime.DBDate;
            indirectManCount.MaintainTime = dbDateTime.DBTime;
            return indirectManCount;
        }


        protected override object GetEditObject(GridRecord row)
        {
            if (_facade == null)
            {
                _facade = new PerformanceFacade(base.DataProvider);
            }
            object obj = _facade.GetIndirectManCount(FormatHelper.TODateInt(row.Items.FindItemByKey("Date").Text.ToString()), row.Items.FindItemByKey("ShiftCode").Text.ToString(), row.Items.FindItemByKey("CrewCode").Text.ToString(), row.Items.FindItemByKey("FacCode").Text.ToString(), row.Items.FindItemByKey("FirstClass").Text.ToString());

            if (obj != null)
            {
                return obj as IndirectManCount;
            }

            return null;
        }

        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.DateEdit.Text = string.Empty;                
                this.txtShiftCodeEdit.Text = string.Empty;
                this.txtCrewCodeEdit.Text = string.Empty;
                this.txtFACCodeEdit.Text = string.Empty;
                this.drpFirstClassEdit.SelectedIndex = 0;
                this.txtDurationEdit.Text = string.Empty;
                this.txtManCountEdit.Text = string.Empty;

                return;
            }

            this.DateEdit.Text = FormatHelper.ToDateString(((IndirectManCount)obj).ShiftDate);
            this.txtShiftCodeEdit.Text = ((IndirectManCount)obj).ShiftCode.ToString();
            this.txtCrewCodeEdit.Text = ((IndirectManCount)obj).CrewCode.ToString();
            this.txtFACCodeEdit.Text = ((IndirectManCount)obj).FactoryCode.ToString();
            this.drpFirstClassEdit.SelectedValue = ((IndirectManCount)obj).FirstClass.ToString();
            this.txtDurationEdit.Text = Convert.ToString(Math.Round((Convert.ToDecimal(((IndirectManCount)obj).Duration) / 3600), 2)) == "0.00" ? "0" : Convert.ToString(Math.Round((Convert.ToDecimal(((IndirectManCount)obj).Duration) / 3600), 2));
            this.txtManCountEdit.Text = ((IndirectManCount)obj).ManCount.ToString();
        }

        protected override bool ValidateInput()
        {
            PageCheckManager manager = new PageCheckManager();

            manager.Add(new DateCheck(lblDateQuery, DateEdit.Text, true));
            manager.Add(new LengthCheck(lblShiftCodeEdit, txtShiftCodeEdit, 40, true));
            manager.Add(new LengthCheck(lblCrewCodeEdit, txtCrewCodeEdit, 40, true));
            manager.Add(new LengthCheck(lblFACCodeEdit, txtFACCodeEdit, 40, true));
            manager.Add(new LengthCheck(lblFirstClassGroup, drpFirstClassEdit, 40, true));
            manager.Add(new DecimalCheck(lblDurationEdit, txtDurationEdit, 0, 9999999999, true));
            manager.Add(new NumberCheck(lblManCountEdit, txtManCountEdit, 0,9999999999, true));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }

            if (Convert.ToDecimal(this.txtDurationEdit.Text.Trim())<=0)
            {
                WebInfoPublish.Publish(this, "$Duration_Must_Over_Zero", this.languageComponent1);
                return false;
            }

            if (Convert.ToDecimal(this.txtManCountEdit.Text.Trim()) <= 0)
            {
                WebInfoPublish.Publish(this, "$ManCount_Must_Over_Zero", this.languageComponent1);
                return false;
            }

            ShiftModelFacade shiftModelFacade = new ShiftModelFacade(this.DataProvider);
            object shiftObject = shiftModelFacade.GetShift(FormatHelper.CleanString(this.txtShiftCodeEdit.Text.ToUpper()));
            if (shiftObject == null)
            {
                WebInfoPublish.Publish(this, "$Error_Shift_Not_Exist", this.languageComponent1);
                return false;
            }

            ShiftModel shiftModel = new ShiftModel(this.DataProvider);
            object crewObject = shiftModel.GetShiftCrew(FormatHelper.CleanString(this.txtCrewCodeEdit.Text.ToUpper()));
            if (crewObject == null)
            {
                WebInfoPublish.Publish(this, "$Error_ShiftCrew_Not_Exist", this.languageComponent1);
                return false;
            }

            WarehouseFacade  warehouseFacade =new WarehouseFacade(this.DataProvider);
            object facObject = warehouseFacade.GetFactory(FormatHelper.CleanString(this.txtFACCodeEdit.Text.ToUpper()));
            if (facObject == null)
            {
                WebInfoPublish.Publish(this, "$Error_FACCODE_Not_Exist", this.languageComponent1);
                return false;
            }
           
            return true;
        }

        #endregion


        #region Export
        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{ ((IndirectManCountWithMessage)obj).ShiftDate.ToString(),
								((IndirectManCountWithMessage)obj).ShiftCode.ToString(),
                                ((IndirectManCountWithMessage)obj).ShiftDescription.ToString(),
								((IndirectManCountWithMessage)obj).CrewCode.ToString(),
					            ((IndirectManCountWithMessage)obj).CrewDesc.ToString(),			
                                ((IndirectManCountWithMessage)obj).FactoryCode.ToString(),
	                            ((IndirectManCountWithMessage)obj).FacDesc.ToString(),
                                ((IndirectManCountWithMessage)obj).FirstClass.ToString(),			
                                ((IndirectManCountWithMessage)obj).ManCount.ToString(),
	                            Convert.ToString(Math.Round((Convert.ToDecimal(((IndirectManCountWithMessage)obj).Duration) / 3600),2))=="0.00"?"0": Convert.ToString(Math.Round((Convert.ToDecimal(((IndirectManCountWithMessage)obj).Duration) / 3600),2)),
                                ((IndirectManCountWithMessage)obj).MaintainUser.ToString(),	
								FormatHelper.ToDateString(((IndirectManCountWithMessage)obj).MaintainDate),
								FormatHelper.ToTimeString(((IndirectManCountWithMessage)obj).MaintainTime)
                                };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	
									"Date",
									"ShiftCode",
                                    "ShiftDescription",
                                    "CrewCode",
                                    "CrewDesc",
                                    "FacCode",
                                    "FacDesc",
                                    "FirstClass",
                                    "ManCount",
                                    "Duration",
									"MaintainUser",
									"MaintainDate",
									"MaintainTime"};
        }

        #endregion

        protected void cmdImport_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(this.MakeRedirectUrl("./ImportIndirectManCountDate/FExcelDataImp.aspx?itype=IndirectManCount"));
        }



    }
}
