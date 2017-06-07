using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    public partial class FNewReportOQCQualityLevelTrend : BaseQPageNew
    {
        #region ҳ���ʼ��

        private System.ComponentModel.IContainer components;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;

        private const string preferredTable = "tblmaterial,tblitemclass,tbltimedimension,**,tblmesentitylist,tblmo,tblres,tblline2crew,";

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);

            //��ʼ���ؼ���λ�úͿɼ���
            InitWhereControls();
            InitGroupControls();
            InitResultControls();
        }

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
            this.UCWhereConditions1.SetControlPosition(0, 2, UCWhereConditions1.PanelMaterialModelCodeWhere.ID);

            this.UCWhereConditions1.SetControlPosition(1, 0, UCWhereConditions1.PanelMaterialMachineTypeWhere.ID);
            this.UCWhereConditions1.SetControlPosition(1, 1, UCWhereConditions1.PanelLotNoWhere.ID);
            this.UCWhereConditions1.SetControlPosition(1, 2, UCWhereConditions1.PanelBigSSCodeWhere.ID);

            this.UCWhereConditions1.SetControlPosition(2, 0, UCWhereConditions1.PanelSSCodeWhere.ID);
            this.UCWhereConditions1.SetControlPosition(2, 1, UCWhereConditions1.PanelMaterialExportImportWhere.ID);
            this.UCWhereConditions1.SetControlPosition(2, 2, UCWhereConditions1.PanelCrewCodeWhere.ID);

            this.UCWhereConditions1.SetControlPosition(3, 0, UCWhereConditions1.PanelStartDateWhere.ID);
            this.UCWhereConditions1.SetControlPosition(3, 1, UCWhereConditions1.PanelEndDateWhere.ID);

            this.UCWhereConditions1.SetControlPosition(4, 0, UCWhereConditions1.PanelFirstClassWhere.ID);
            this.UCWhereConditions1.SetControlPosition(4, 1, UCWhereConditions1.PanelSecondClassWhere.ID);
            this.UCWhereConditions1.SetControlPosition(4, 2, UCWhereConditions1.PanelThirdClassWhere.ID);

            this.UCWhereConditions1.SetControlPosition(5, 0, UCWhereConditions1.PanelProductionTypeWhere.ID);
            this.UCWhereConditions1.SetControlPosition(5, 1, UCWhereConditions1.PanelOQCLotTypeWhere.ID);
        }

        private void InitGroupControls()
        {
            this.UCGroupConditions1.ShowShiftDay = true;
            this.UCGroupConditions1.ShowWeek = true;
            this.UCGroupConditions1.ShowMonth = true;
            this.UCGroupConditions1.ShowYear = true;

            this.UCGroupConditions1.ShowByTimePanel = true;
            this.UCGroupConditions1.ShowCompareTypePanel = true;

            this.UCGroupConditions1.ShowBigSSCode = true;
            this.UCGroupConditions1.ShowSSCode = true;

            this.UCGroupConditions1.ShowGoodSemiGood = true;
            this.UCGroupConditions1.ShowItemCode = true;
            this.UCGroupConditions1.ShowMaterialModelCode = true;
            this.UCGroupConditions1.ShowMaterialMachineType = true;
            this.UCGroupConditions1.ShowMaterialExportImport = true;
            this.UCGroupConditions1.ShowLotNo = true;
            this.UCGroupConditions1.ShowProductionType = true;
            this.UCGroupConditions1.ShowOQCLotType = true;
            this.UCGroupConditions1.ShowCrewCode = true;

            this.UCGroupConditions1.ShowFirstClass = true;
            this.UCGroupConditions1.ShowSecondClass = true;
            this.UCGroupConditions1.ShowThirdClass = true;

            this.UCGroupConditions1.ShowSp2 = true;
        }

        private void InitResultControls()
        {
            //this.gridWebGrid.Visible = false;
            this.cmdGridExport.Visible = false;
            this.columnChart.Visible = false;
            this.lineChart.Visible = false;
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
            this.gridHelper = new GridHelperNew(this.gridWebGrid,this.DtSource);
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this.UCWhereConditions1.InitUserControl(this.languageComponent1, this.DataProvider);
                this.UCGroupConditions1.InitUserControl(this.languageComponent1, this.DataProvider);
                this.UCDisplayConditions1.InitUserControl(this.languageComponent1, this.DataProvider);

                this.RefreshController1.Interval = ConfigSection.Current.DomainSetting.Interval;
            }

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
            string byTimeType = UCGroupConditions1.UserSelectByTimeType;
            string compareType = UCGroupConditions1.UserSelectCompareType;

            if (!isForCompare)
            {
                compareType = string.Empty;
            }

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
            engine.DetailedCoreTable = GetCoreTableDetail();
            engine.Formular = GetFormular(compareType);
            engine.WhereCondition = this.UCWhereConditions1.GetWhereSQLStatement(preferredTable, byTimeType, roundDate, dateAdjust);
            engine.GroupFieldsX = groupFieldsX;
            engine.GroupFieldsY = groupFieldsY;

            return engine.GetReportDataSource(byTimeType, dateAdjust); ;
        }

        private string GetCoreTableDetail()
        {
            string returnValue = string.Empty;

            returnValue += "SELECT tbllot.lotno, tbllot.itemcode, tbllot.shiftday, " + "\r\n";
            returnValue += "tbllot.sscode, tbllot.rescode, tblss.bigsscode, tblline2crew.crewcode, " + "\r\n";
            returnValue += "zgradetimes, agradetimes, bggradetimes, cgradetimes " + "\r\n";
            returnValue += "FROM tbloqclotcklist " + "\r\n";
            returnValue += "INNER JOIN tbllot " + "\r\n";
            returnValue += "ON tbloqclotcklist.lotno = tbllot.lotno " + "\r\n";
            returnValue += "AND tbloqclotcklist.lotseq = tbllot.lotseq " + "\r\n";
            returnValue += "LEFT OUTER JOIN tblss " + "\r\n";
            returnValue += "ON tbllot.sscode = tblss.sscode " + "\r\n";
            returnValue += "LEFT OUTER JOIN tblline2crew " + "\r\n";
            returnValue += "ON tbllot.shiftday = tblline2crew.shiftdate " + "\r\n";
            returnValue += "AND tbllot.shiftcode = tblline2crew.shiftcode " + "\r\n";
            returnValue += "AND tbllot.sscode = tblline2crew.sscode " + "\r\n";           

            return returnValue;
        }

        private string GetFormular(string compareType)
        {
            string returnValue = string.Empty;

            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);

            BenQGuru.eMES.Domain.BaseSetting.Parameter aCoefficient = (BenQGuru.eMES.Domain.BaseSetting.Parameter)systemSettingFacade.GetParameter("ACOEFFICIENT", "OQCQUALITYLEVELSETTING");
            BenQGuru.eMES.Domain.BaseSetting.Parameter bCoefficient = (BenQGuru.eMES.Domain.BaseSetting.Parameter)systemSettingFacade.GetParameter("BCOEFFICIENT", "OQCQUALITYLEVELSETTING");
            BenQGuru.eMES.Domain.BaseSetting.Parameter cCoefficient = (BenQGuru.eMES.Domain.BaseSetting.Parameter)systemSettingFacade.GetParameter("CCOEFFICIENT", "OQCQUALITYLEVELSETTING");
            BenQGuru.eMES.Domain.BaseSetting.Parameter zCoefficient = (BenQGuru.eMES.Domain.BaseSetting.Parameter)systemSettingFacade.GetParameter("ZCOEFFICIENT", "OQCQUALITYLEVELSETTING");

            double a = 0;
            double b = 0;
            double c = 0;
            double z = 0;

            if (aCoefficient != null)
            {
                double.TryParse(aCoefficient.ParameterAlias, out a);
            }
            if (bCoefficient != null)
            {
                double.TryParse(bCoefficient.ParameterAlias, out b);
            }
            if (cCoefficient != null)
            {
                double.TryParse(cCoefficient.ParameterAlias, out c);
            }
            if (zCoefficient != null)
            {
                double.TryParse(zCoefficient.ParameterAlias, out z);
            }

            returnValue = "SUM(**.zgradetimes*" + z.ToString("0.00") + "+agradetimes*" + a.ToString("0.00") +
                "+bggradetimes*" + b.ToString("0.00") + "+cgradetimes*" + c.ToString("0.00") + ") AS oqcqualitylevelvalue";

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

            schemaList.Add("OQCSampleNGRate");
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
                object[] dateSource = null;
                object[] dateSourceCompare = null;

                //һ������
                dateSource = this.LoadDataSource(false, compareType.Trim().Length > 0);

                if (dateSource == null || dateSource.Length <= 0)
                {
                    this.gridWebGrid.Visible = false;
                    this.cmdGridExport.Visible = false;
                    lineChart.Visible = false;
                    columnChart.Visible = false;

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

                dim3PropertyList.Add(new ReportGridDim3Property("OQCQualityLevelValue", "0.00", "", "", false));
                
                ReportGridHelperNew reportGridHelper = new ReportGridHelperNew(this.DataProvider, this.languageComponent1, this.gridWebGrid,this.DtSource);
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
                    lineChart.Visible = false;
                    columnChart.Visible = false;
                }
                else if (UCDisplayConditions1.GetDisplayType().Trim().ToLower() == NewReportDisplayType.LineChart
                    || UCDisplayConditions1.GetDisplayType().Trim().ToLower() == NewReportDisplayType.PieChart
                    || UCDisplayConditions1.GetDisplayType().Trim().ToLower() == NewReportDisplayType.HistogramChart)
                {
                    List<string> rowPropertyList = GetColumns();
                    List<string> columnPropertyList = GetRows();
                    columnPropertyList.Add("EAttribute1");
                    List<string> dataPropertyList = new List<string>();
                    dataPropertyList.Add("OQCQualityLevelValue");

                    NewReportDomainObject[] dateSourceForOWC = new NewReportDomainObject[dateSource.Length + dateSourceCompare.Length];
                    dateSource.CopyTo(dateSourceForOWC, 0);
                    for (int i = 0; i < dateSourceCompare.Length; i++)
                    {
                        dateSourceForOWC[dateSource.Length + i] = (NewReportDomainObject)dateSourceCompare[i];
                    }
                    string propertyName = this.languageComponent1.GetString(dataPropertyList[0]);
                    foreach (NewReportDomainObject domainObject in dateSourceForOWC)
                    {
                        domainObject.EAttribute1 = propertyName + domainObject.EAttribute1;
                    }


                    //add by seven 20110111
                    foreach (NewReportDomainObject obj in dateSourceForOWC)
                    {
                        obj.TempValue = obj.OQCQualityLevelValue.ToString();
                        //�졢�ܡ��¡���
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

                        this.lineChart.ChartTextFormatString = "<DATA_VALUE:#0.##>";
                        this.lineChart.YLabelFormatString = "<DATA_VALUE:#0.##>";
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

                        this.columnChart.ChartTextFormatString = "<DATA_VALUE:#0.##>";
                        this.columnChart.YLabelFormatString = "<DATA_VALUE:#0.##>";
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
