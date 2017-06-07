using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Config;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;
using BenQGuru.Web.ReportCenter.UserControls;

using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.Web.ReportCenter
{
    public partial class FNewReportQuantityQP : BaseQPageNew
    {
        #region ҳ���ʼ��

        private System.ComponentModel.IContainer components;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;

        private const string preferredTable = "tblmesentitylist,tblmaterial,tblmo,tblres,tblline2crew,tblitemclass,tbltimedimension";

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
            this.languageComponent1.Language = "CHS";
            this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
            this.languageComponent1.RuntimePage = null;
            this.languageComponent1.RuntimeUserControl = null;
            this.languageComponent1.UserControlName = "";
        }

        private void InitWhereControls()
        {
            this.UCWhereConditions1.SetControlPosition(0, 0, UCWhereConditions1.PanelGoodSemiGoodWhere.ID);
            this.UCWhereConditions1.SetControlPosition(0, 1, UCWhereConditions1.PanelItemCodeWhere.ID);
            //this.UCWhereConditions1.SetControlPosition(0, 2, UCWhereConditions1.PanelMaterialModelCodeWhere.ID);
            this.UCWhereConditions1.SetControlPosition(0, 2, UCWhereConditions1.PanelMOTypeWhere.ID);


            this.UCWhereConditions1.SetControlPosition(1, 0, UCWhereConditions1.PanelMOCodeWhere.ID);
            this.UCWhereConditions1.SetControlPosition(1, 1, UCWhereConditions1.PanelBigSSCodeWhere.ID);
            this.UCWhereConditions1.SetControlPosition(1, 2, UCWhereConditions1.PanelSegCodeWhere.ID);
           
            //this.UCWhereConditions1.SetControlPosition(1, 2, UCWhereConditions1.PanelOrderNoWhere.ID);


            this.UCWhereConditions1.SetControlPosition(2, 0, UCWhereConditions1.PanelSSCodeWhere.ID);

            this.UCWhereConditions1.SetControlPosition(2, 1, UCWhereConditions1.PanelOPCodeWhere.ID);
            this.UCWhereConditions1.SetControlPosition(2, 2, UCWhereConditions1.PanelShiftCodeWhere.ID);

            this.UCWhereConditions1.SetControlPosition(3, 0, UCWhereConditions1.PanelCrewCodeWhere.ID);

            this.UCWhereConditions1.SetControlPosition(4, 0, UCWhereConditions1.PanelStartDateWhere.ID);
            this.UCWhereConditions1.SetControlPosition(4, 1, UCWhereConditions1.PanelEndDateWhere.ID);

            //this.UCWhereConditions1.SetControlPosition(5, 0, UCWhereConditions1.PanelFirstClassWhere.ID);
            //this.UCWhereConditions1.SetControlPosition(5, 1, UCWhereConditions1.PanelSecondClassWhere.ID);
            //this.UCWhereConditions1.SetControlPosition(5, 2, UCWhereConditions1.PanelThirdClassWhere.ID);

            this.UCWhereConditions1.SetControlPosition(6, 0, UCWhereConditions1.PanelInputOututWhere.ID);
            this.UCWhereConditions1.SetControlPosition(6, 1, UCWhereConditions1.PanelMOBOMVersionWhere.ID);

            //this.UCWhereConditions1.SetControlPosition(7, 0, UCWhereConditions1.PanelMOMemoWhere.ID);
            this.UCWhereConditions1.SetControlPosition(6, 2, UCWhereConditions1.PanelNewMassWhere.ID);
        }

        private void InitGroupControls()
        {
            this.UCGroupConditions1.ShowPeriod = true;
            this.UCGroupConditions1.ShowShift = true;
            this.UCGroupConditions1.ShowShiftDay = true;
            this.UCGroupConditions1.ShowWeek = true;
            this.UCGroupConditions1.ShowMonth = true;
            this.UCGroupConditions1.ShowYear = true;

            this.UCGroupConditions1.ShowByTimePanel = true;
            this.UCGroupConditions1.ShowCompareTypePanel = true;
            //this.UCGroupConditions1.ShowCompleteTypePanel = true;

            this.UCGroupConditions1.ShowBigSSCode = true;
            this.UCGroupConditions1.ShowSegCode = true;
            this.UCGroupConditions1.ShowSSCode = true;
            this.UCGroupConditions1.ShowOPCode = true;
            this.UCGroupConditions1.ShowResCode = true;

            this.UCGroupConditions1.ShowGoodSemiGood = true;
            this.UCGroupConditions1.ShowItemCode = true;
            //this.UCGroupConditions1.ShowMaterialModelCode = true;
            this.UCGroupConditions1.ShowMOCode = true;
            //this.UCGroupConditions1.ShowMOMemo = true;
            this.UCGroupConditions1.ShowNewMass = true;
            this.UCGroupConditions1.ShowCrewCode = true;

            //this.UCGroupConditions1.ShowFirstClass = true;
            //this.UCGroupConditions1.ShowSecondClass = true;
            //this.UCGroupConditions1.ShowThirdClass = true;

            this.UCGroupConditions1.ShowSp1 = true;
            this.UCGroupConditions1.ShowSp2 = true;
            this.UCGroupConditions1.ShowSp3 = true;
        }

        private void InitResultControls()
        {
            //this.gridWebGrid.Visible = false;
            this.cmdGridExport.Visible = false;
        }

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);

            //��ʼ���ؼ���λ�úͿɼ���
            InitWhereControls();
            InitGroupControls();
            InitResultControls();
        }

        #endregion

        #region �������Ժ��¼�����

        private void LoadDisplayControls()
        {
            if (!this.IsPostBack)
            {
                List<ListItem> displayList = new List<ListItem>();
                displayList.Add(new ListItem(this.languageComponent1.GetString(NewReportDisplayType.Grid), NewReportDisplayType.Grid));
                displayList.Add(new ListItem(this.languageComponent1.GetString(NewReportDisplayType.LineChart), NewReportDisplayType.LineChart));
                displayList.Add(new ListItem(this.languageComponent1.GetString(NewReportDisplayType.HistogramChart), NewReportDisplayType.HistogramChart));
                this.UCDisplayConditions1.DisplayList = displayList;

                if (this.Request.Params["Width"] != null)
                {
                    ViewState["Width"] = this.Request.Params["Width"];
                }

                if (this.Request.Params["Height"] != null)
                {
                    ViewState["Height"] = this.Request.Params["Height"];
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gridHelper = new GridHelperNew(this.gridWebGrid, DtSource);
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this.UCWhereConditions1.InitUserControl(this.languageComponent1, this.DataProvider);
                this.UCGroupConditions1.InitUserControl(this.languageComponent1, this.DataProvider);
                this.UCDisplayConditions1.InitUserControl(this.languageComponent1, this.DataProvider);

                this.RefreshController1.Interval = ConfigSection.Current.DomainSetting.Interval;

                this.columnChart.Visible = false;
                this.lineChart.Visible = false;
            }

            //���ؿؼ���ֵ
            LoadDisplayControls();
            this.gridWebGrid.Behaviors.RowSelectors.RowNumbering = false;
            this.gridWebGrid.Behaviors.Sorting.Enabled = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            ReportPageHelper.SetControlValue(this, this.Request.Params);
            ReportPageHelper.DoQueryForBSHome(this, this.Request.Params, this.DoQuery);

            if (this.AutoRefresh)
            {
                this.DoQuery();
            }

            base.OnPreRender(e);
        }

        public bool AutoRefresh
        {
            get
            {
                if (this.ViewState["AutoRefresh"] != null)
                {
                    try
                    {
                        return bool.Parse(this.ViewState["AutoRefresh"].ToString());
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            set
            {
                this.ViewState["AutoRefresh"] = value.ToString();

                if (value)
                {
                    this.RefreshController1.Start();
                }
                else
                {
                    this.RefreshController1.Stop();
                }
            }
        }

        protected void cmdGridExport_ServerClick(object sender, System.EventArgs e)
        {
            this.DoQuery();
            this.GridExport(this.gridWebGrid);
        }

        protected void cmdQuery_ServerClick(object sender, System.EventArgs e)
        {
            this.DoQuery();
        }

        protected void chkRefreshAuto_CheckedChanged(object sender, System.EventArgs e)
        {
            this.DoQuery();
        }

        #endregion

        #region ʹ��ReportSQLEngine��صĺ���

        private object[] LoadDataSource(bool isForCompare, bool roundDate)
        {
            string inputOutput = UCWhereConditions1.UserSelectInputOutput;
            string byTimeType = UCGroupConditions1.UserSelectByTimeType;
            string compareType = UCGroupConditions1.UserSelectCompareType;
            string completeType = UCGroupConditions1.UserSelectCompleteType;

            if (!isForCompare)
            {
                compareType = string.Empty;
            }

            bool bigSSChecked = UCGroupConditions1.BigSSChecked;
            bool opResChecked = UCGroupConditions1.OPChecked || UCGroupConditions1.ResChecked;
            bool segSSChecked = UCGroupConditions1.SegChecked || UCGroupConditions1.SSChecked;

            string groupFieldsX = this.UCGroupConditions1.GetGroupFieldList(preferredTable, "X");
            string groupFieldsY = this.UCGroupConditions1.GetGroupFieldList(preferredTable, "Y");

            //���ڻ���ͬ�ڱ�ʱ���޸�ʱ���������
            int dateAdjust = 0;
            if (string.Compare(compareType, NewReportCompareType.LastYear, true) == 0)
            {
                dateAdjust = -12;
            }
            else if (string.Compare(compareType, NewReportCompareType.Previous, true) == 0)
            {
                dateAdjust = -1;
            }

            ReportSQLEngine engine = new ReportSQLEngine(this.DataProvider, this.languageComponent1);
            engine.DetailedCoreTable = GetDetailedCoreTable();
            engine.Formular = GetFormular(inputOutput, compareType, completeType, bigSSChecked, opResChecked, segSSChecked);
            engine.WhereCondition = this.UCWhereConditions1.GetWhereSQLStatement(preferredTable, byTimeType, roundDate, dateAdjust);
            engine.HavingCondition = GetHavingCondition(inputOutput, compareType, completeType, bigSSChecked, opResChecked, segSSChecked);
            engine.GroupFieldsX = groupFieldsX;
            engine.GroupFieldsY = groupFieldsY;

            return engine.GetReportDataSource(byTimeType, dateAdjust); ;
        }

        private string GetDetailedCoreTable()
        {
            string returnValue = string.Empty;

            if (UCGroupConditions1.OPChecked || UCGroupConditions1.ResChecked)
            {
                returnValue = ReportSQLEngine.GetDetailedCoreForRptOpQty();
            }
            else
            {
                returnValue = ReportSQLEngine.GetDetailedCoreForRptSoQty();
            }

            return returnValue;
        }

        private string GetFormular(string inputOutput, string compareType, string completeType, bool bigSSChecked, bool opResChecked, bool segSSChecked)
        {
            string returnValue = string.Empty;
            string inputForm = string.Empty;
            string outputForm = string.Empty;
            string CompeleteOutPut = string.Empty;

            if (opResChecked)
            {
                inputForm = "SUM(**.inputtimes) AS input";
                outputForm = "SUM(**.outputtimes) AS output";
                CompeleteOutPut = "SUM(**.outputtimes) AS CompeletePut";
            }
            else if (segSSChecked)
            {
                inputForm = "SUM(**.lineinputcount) AS input";
                outputForm = "SUM(**.lineoutputcount) AS output";
                CompeleteOutPut = "SUM(**.lineoutputcount) AS CompeletePut";
            }
            else
            {
                inputForm = "SUM(**.moinputcount) AS input";
                outputForm = "SUM(**.molineoutputcount) AS output";
                CompeleteOutPut = "SUM(**.mooutputcount) AS CompeletePut";

            }

            inputOutput = inputOutput.Trim().ToLower();
            if (inputOutput == "input")
            {
                returnValue = inputForm;
            }
            else if (inputOutput == "output")
            {
                returnValue = outputForm;
            }
            else if (inputOutput == "compeleteput")
            {
                returnValue = CompeleteOutPut;
            }
            else
            {
                returnValue = inputForm + "," + outputForm + "," + CompeleteOutPut;
            }

            //EATTRIBUTE1���ڱ��Ͷ��/����+�Ƚ�����
            if (compareType.Trim().Length > 0)
            {
                if (returnValue.Trim().Length > 0)
                {
                    returnValue += ",";
                }
                returnValue += "'" + compareType.Trim() + "'" + " AS EATTRIBUTE1 ";
            }

            return returnValue;
        }

        private string GetHavingCondition(string inputOutput, string compareType, string completeType, bool bigSSChecked, bool opResChecked, bool segSSChecked)
        {
            string returnValue = string.Empty;
            string inputForm = string.Empty;
            string outputForm = string.Empty;
            string CompeleteOutPut = string.Empty;

            if (opResChecked)
            {
                inputForm = "SUM(**.inputtimes) > 0";
                outputForm = "SUM(**.outputtimes) > 0";
                CompeleteOutPut = "SUM(**.outputtimes) > 0";
            }
            else if (segSSChecked)
            {
                inputForm = "SUM(**.lineinputcount) > 0";
                outputForm = "SUM(**.lineoutputcount) > 0";
                CompeleteOutPut = "SUM(**.lineoutputcount) > 0";
            }
            else
            {
                inputForm = "SUM(**.moinputcount) > 0";
                outputForm = "SUM(**.molineoutputcount) > 0";
                CompeleteOutPut = "SUM(**.mooutputcount) > 0";

            }

            inputOutput = inputOutput.Trim().ToLower();
            if (inputOutput == "input")
            {
                returnValue = inputForm;
            }
            else if (inputOutput == "output")
            {
                returnValue = outputForm;
            }
            else if (inputOutput == "compeleteput")
            {
                returnValue = CompeleteOutPut;
            }
            else
            {
                returnValue = inputForm + " OR " + outputForm + " OR " + CompeleteOutPut;
            }

            return returnValue;
        }

        #endregion

        #region ��صĺ���

        private string[] GetOWCSchema()
        {
            string[] rows = GetRows().ToArray();
            string[] columns = GetColumns().ToArray();

            ArrayList schemaList = new ArrayList();
            foreach (string row in rows)
            {
                schemaList.Add(row);
            }
            foreach (string column in columns)
            {
                schemaList.Add(column);
            }

            schemaList.Add("Input");
            schemaList.Add("Output");
            schemaList.Add("CompeletePut");
            schemaList.Add("EAttribute1");


            return (string[])schemaList.ToArray(typeof(string));
        }

        private List<string> GetColumns()
        {
            List<string> returnValue = new List<string>();

            string rowString = UCGroupConditions1.GetGroupFieldAliasList(preferredTable, "X");

            if (rowString.Trim().Length > 0)
            {

                returnValue.AddRange(rowString.Split(','));

                for (int i = 0; i < returnValue.Count; i++)
                {
                    returnValue[i] = DomainObjectUtility.GetPropertyNameByFieldName(typeof(NewReportDomainObject), returnValue[i]);
                }
            }

            return returnValue;
        }

        private List<string> GetRows()
        {
            List<string> returnValue = new List<string>();

            string columnString = UCGroupConditions1.GetGroupFieldAliasList(preferredTable, "Y");

            if (columnString.Trim().Length > 0)
            {

                returnValue.AddRange(columnString.Split(','));

                for (int i = 0; i < returnValue.Count; i++)
                {
                    returnValue[i] = DomainObjectUtility.GetPropertyNameByFieldName(typeof(NewReportDomainObject), returnValue[i]);
                }
            }

            return returnValue;
        }



        #endregion

        private bool CheckBeforeQuery()
        {
            //ͼ����ʾʱ��Ͷ����ߵ�����ѡ��һ
            if (UCWhereConditions1.UserSelectInputOutput.Trim().Length <= 0
                && UCDisplayConditions1.GetDisplayType().Trim().ToLower() != NewReportDisplayType.Grid.ToLower())
            {
                WebInfoPublish.Publish(this, "$Report_InputOrOutputNeedWhenChart", this.languageComponent1);
                return false;
            }

            //����������ѡ��OPʱ�����������б���ѡ��Res����OP
            if (UCWhereConditions1.UserSelectOP.Trim().Length > 0
                && !UCGroupConditions1.OPChecked
                && !UCGroupConditions1.ResChecked)
            {
                WebInfoPublish.Publish(this, "$Report_GroupByOPOrResMustBeChecked", this.languageComponent1);
                return false;
            }

            //ͬ�ڱ�ֻ��������
            if (UCGroupConditions1.UserSelectCompareType == NewReportCompareType.LastYear
                && UCGroupConditions1.UserSelectByTimeType != NewReportByTimeType.Month)
            {
                WebInfoPublish.Publish(this, "$Report_LastYearOnlyForMonth", this.languageComponent1);
                return false;
            }

            //����ֻ�������ܡ��¡���
            if (UCGroupConditions1.UserSelectCompareType == NewReportCompareType.Previous
                && UCGroupConditions1.UserSelectByTimeType != NewReportByTimeType.Week
                && UCGroupConditions1.UserSelectByTimeType != NewReportByTimeType.Month
                && UCGroupConditions1.UserSelectByTimeType != NewReportByTimeType.Year)
            {
                WebInfoPublish.Publish(this, "$Report_PreviousOnlyForWeekMonthYear", this.languageComponent1);
                return false;
            }

            return true;
        }

        protected override void DoQuery()
        {
            base.DoQuery();
            if (this.CheckBeforeQuery())
            {
                this.AutoRefresh = this.chbRefreshAuto.Checked;

                string compareType = this.UCGroupConditions1.UserSelectCompareType.Trim().ToLower();
                string byTimeType = this.UCGroupConditions1.UserSelectByTimeType.Trim().ToLower();
                string inputOutput = this.UCWhereConditions1.UserSelectInputOutput.Trim().ToLower();
                object[] dateSource = null;
                object[] dateSourceCompare = null;

                //һ������
                dateSource = this.LoadDataSource(false, compareType.Trim().Length > 0);
                if (dateSource == null || dateSource.Length <= 0)
                {
                    this.gridWebGrid.Visible = false;
                    this.cmdGridExport.Visible = false;
                    this.lineChart.Visible = false;
                    this.columnChart.Visible = false;


                    ReportPageHelper.SetPageScrollToBottom(this);
                    return;
                }

                //����/ͬ�ڱ�����
                if (compareType.Trim().Length > 0)
                {
                    dateSourceCompare = this.LoadDataSource(true, true);
                }
                if (dateSourceCompare == null)
                {
                    dateSourceCompare = new NewReportDomainObject[0] { };
                }

                //���ݼ��ص�Grid
                List<string> fixedColumnList = GetRows();
                List<ReportGridDim3Property> dim3PropertyList = new List<ReportGridDim3Property>();

                if (inputOutput.Trim().Length == 0)
                {
                    dim3PropertyList.Add(new ReportGridDim3Property("Input", "0", "SUM", "SUM", false));
                    dim3PropertyList.Add(new ReportGridDim3Property("Output", "0", "SUM", "SUM", false));
                    dim3PropertyList.Add(new ReportGridDim3Property("CompeletePut", "0", "SUM", "SUM", false));
                }
                else if (string.Compare(inputOutput.Trim(), "Input", true) == 0)
                {
                    dim3PropertyList.Add(new ReportGridDim3Property("Input", "0", "SUM", "SUM", false));
                }
                else if (string.Compare(inputOutput.Trim(), "Output", true) == 0)
                {
                    dim3PropertyList.Add(new ReportGridDim3Property("Output", "0", "SUM", "SUM", false));
                }
                else if (string.Compare(inputOutput.Trim(), "CompeletePut", true) == 0)
                {
                    dim3PropertyList.Add(new ReportGridDim3Property("CompeletePut", "0", "SUM", "SUM", false));
                }

                ReportGridHelperNew reportGridHelper = new ReportGridHelperNew(this.DataProvider, this.languageComponent1, this.gridWebGrid,DtSource);

                reportGridHelper.DataSource = dateSource;
                reportGridHelper.DataSourceForCompare = dateSourceCompare;
                reportGridHelper.Dim1PropertyList = fixedColumnList;
                reportGridHelper.Dim3PropertyList = dim3PropertyList;
                reportGridHelper.HasDim3PropertyNameRowColumn = true;
                reportGridHelper.CompareType = compareType;
                reportGridHelper.ByTimeType = byTimeType;
                reportGridHelper.ShowGrid();
                base.InitWebGrid();
                this.gridWebGrid.Behaviors.RowSelectors.RowNumbering = false;
                this.gridWebGrid.Behaviors.Sorting.Enabled = false;

                //��ȡ����ͼʾ
                if (UCDisplayConditions1.GetDisplayType().Trim().ToLower() == NewReportDisplayType.Grid)
                {
                    this.gridWebGrid.Visible = true;
                    this.cmdGridExport.Visible = true;
                    this.lineChart.Visible = false;
                    this.columnChart.Visible = false;
                }
                else if (UCDisplayConditions1.GetDisplayType().Trim().ToLower() == NewReportDisplayType.LineChart
                    || UCDisplayConditions1.GetDisplayType().Trim().ToLower() == NewReportDisplayType.PieChart
                    || UCDisplayConditions1.GetDisplayType().Trim().ToLower() == NewReportDisplayType.HistogramChart)
                {

                    NewReportDomainObject[] dateSourceForOWC = new NewReportDomainObject[dateSource.Length + dateSourceCompare.Length];
                    dateSource.CopyTo(dateSourceForOWC, 0);
                    for (int i = 0; i < dateSourceCompare.Length; i++)
                    {
                        dateSourceForOWC[dateSource.Length + i] = (NewReportDomainObject)dateSourceCompare[i];
                    }
                    string propertyName = this.languageComponent1.GetString(dim3PropertyList[0].Name);
                    foreach (NewReportDomainObject domainObject in dateSourceForOWC)
                    {
                        domainObject.EAttribute1 = propertyName + domainObject.EAttribute1;
                    }

                    List<string> rowPropertyList = GetColumns();
                    List<string> columnPropertyList = GetRows();
                    columnPropertyList.Add("EAttribute1");
                    List<string> valuePropertyList = new List<string>();
                    foreach (ReportGridDim3Property property in dim3PropertyList)
                    {
                        if (!property.Hidden)
                        {
                            valuePropertyList.Add(property.Name);
                        }
                    }
                    List<string> dataPropertyList = valuePropertyList;

                    foreach (NewReportDomainObject obj in dateSourceForOWC)
                    {
                        if (UCWhereConditions1.UserSelectInputOutput == "input")
                        {
                            obj.TempValue = obj.Input.ToString();
                        }
                        if (UCWhereConditions1.UserSelectInputOutput == "output")
                        {
                            obj.TempValue = obj.Output.ToString();
                        }
                        if (UCWhereConditions1.UserSelectInputOutput == "CompeletePut")
                        {
                            obj.TempValue = obj.CompeletePut.ToString();
                        }
                        //added by Alex hu
                        //ʱ�Ρ���Ρ��졢�ܡ��¡���
                        if (UCGroupConditions1.UserSelectByTimeType == NewReportByTimeType.Period)
                        {
                            obj.PeriodCode = obj.PeriodCode.ToString();
                        }

                        if (UCGroupConditions1.UserSelectByTimeType == NewReportByTimeType.Shift)
                        {
                            obj.PeriodCode = obj.ShiftCode.ToString();
                        }

                        if (UCGroupConditions1.UserSelectByTimeType == NewReportByTimeType.ShiftDay)
                        {
                            obj.PeriodCode = obj.ShiftDay.ToString();
                        }

                        if (UCGroupConditions1.UserSelectByTimeType == NewReportByTimeType.Week)
                        {
                            obj.PeriodCode = obj.Week.ToString();
                        }

                        if (UCGroupConditions1.UserSelectByTimeType == NewReportByTimeType.Month)
                        {
                            obj.PeriodCode = obj.Month.ToString();
                        }

                        if (UCGroupConditions1.UserSelectByTimeType == NewReportByTimeType.Year)
                        {
                            obj.PeriodCode = obj.Year.ToString();
                        }
                        //end
                    }

                    if (UCDisplayConditions1.GetDisplayType().Trim().ToLower() == NewReportDisplayType.LineChart)
                    {
                        this.columnChart.Visible = false;
                        this.lineChart.Visible = true;

                        lineChart.ChartGroupByString = UCGroupConditions1.GetCheckedColumnsString();

                        //������ҳ����Ĵ�С
                        if (ViewState["Width"] != null)
                        {
                            lineChart.Width = int.Parse(ViewState["Width"].ToString());
                        }

                        if (ViewState["Height"] != null)
                        {
                            lineChart.Height = int.Parse(ViewState["Height"].ToString());
                        }
                        //end

                        this.lineChart.ChartTextFormatString = "<DATA_VALUE:0.##>";
                        this.lineChart.YLabelFormatString = "<DATA_VALUE:0.##>";
                        this.lineChart.DataType = true;
                        this.lineChart.DataSource = dateSourceForOWC;
                        this.lineChart.DataBind();
                    }
                    else if (UCDisplayConditions1.GetDisplayType().Trim().ToLower() == NewReportDisplayType.HistogramChart)
                    {
                        this.columnChart.Visible = true;
                        this.lineChart.Visible = false;

                        columnChart.ChartGroupByString = UCGroupConditions1.GetCheckedColumnsString();
                        //������ҳ����Ĵ�С
                        if (ViewState["Width"] != null)
                        {
                            columnChart.Width = int.Parse(ViewState["Width"].ToString());
                        }

                        if (ViewState["Height"] != null)
                        {
                            columnChart.Height = int.Parse(ViewState["Height"].ToString());
                        }
                        //end
                        this.columnChart.ChartTextFormatString = "<DATA_VALUE:0.##>";
                        this.columnChart.YLabelFormatString = "<DATA_VALUE:0.##>";
                        this.columnChart.DataType = true;
                        this.columnChart.DataSource = dateSourceForOWC;
                        this.columnChart.DataBind();
                    }
                    else
                    {
                        this.columnChart.Visible = false;
                        this.lineChart.Visible = false;
                    }

                    this.gridWebGrid.Visible = false;

                    this.cmdGridExport.Visible = false;
                }

                ReportPageHelper.SetPageScrollToBottom(this);
            }
            else
            {
                this.chbRefreshAuto.Checked = false;
                this.AutoRefresh = false;
            }

        }
    }
}
