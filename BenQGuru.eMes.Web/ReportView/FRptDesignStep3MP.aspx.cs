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
using System.Xml;
using System.Collections.Generic;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.ReportView;
using BenQGuru.eMES.Domain.ReportView;
using Infragistics.Web.UI.GridControls;

namespace BenQGuru.eMES.Web.ReportView
{
    /// <summary>
    /// FRptDesignStep3MP ��ժҪ˵����
    /// </summary>
    public partial class FRptDesignStep3MP : ReportWizardBasePage
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private object ActiveRowIndex
        {
            get
            {
                if (this.gridWebGrid.Behaviors.CreateBehavior<Activation>().ActiveCell == null)
                    return null;
                ViewState["ActiveRowIndex"] = this.gridWebGrid.Behaviors.Activation.ActiveCell.Row.Index;
                return this.ViewState["ActiveRowIndex"];
            }

            set { this.ViewState["ActiveRowIndex"] = value; }
        }
        //private GridHelperNew gridHelper = null;
        //private DataTable dtSource = new DataTable();
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

            this.imgSelect.ServerClick += new ImageClickEventHandler(imgSelect_ServerClick);
            this.imgUnSelect.ServerClick += new ImageClickEventHandler(imgUnSelect_ServerClick);
            //this.gridWebGrid.RowSelectionChanged += new SelectedRowEventHandler(gridWebGrid_RowSelectionChanged);
            this.imgUp.ServerClick += new ImageClickEventHandler(SetColumnUp);
            this.imgDown.ServerClick += new ImageClickEventHandler(SetColumnDown);

        }
        #endregion

        private bool bIsSqlDataSource = false;

        #region Init
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, DtSource);
            if (this.IsPostBack == false)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this.txtReportName.Text = this.designView.DesignMain.ReportName;

                this.InitColumnList();
                InitGrid();
                this.lblDesignStep3Title.Text = this.languageComponent1.GetString("$PageControl_DesignStep3Title");
            }
            string strColumnList = "";
            for (int i = 0; i < this.designView.GridColumns.Length; i++)
            {
                strColumnList += this.designView.GridColumns[i].ColumnName + ",";
            }
            string scriptString = "var DISPLAY_COLUMN_LIST='" + strColumnList + "';var DATA_SOURCE_ID='" + this.designView.DesignMain.DataSourceID.ToString() + "';";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), Guid.NewGuid().ToString(), scriptString, true);

            this.gridWebGrid.Behaviors.CreateBehavior<Activation>().Enabled = true;

        }

        private void InitColumnList()
        {
            this.lstUnSelectColumn.Items.Clear();
            ReportViewFacade rptFacade = new ReportViewFacade(this.DataProvider);
            RptViewDataSource dataSource = (RptViewDataSource)rptFacade.GetRptViewDataSource(this.designView.DesignMain.DataSourceID);
            RptViewDataSourceColumn[] columns = rptFacade.GetRptViewDataSourceColumnByDataSourceId(Convert.ToInt32(this.designView.DesignMain.DataSourceID));
            Dictionary<string, string> columnMap = new Dictionary<string, string>();
            for (int i = 0; i < columns.Length; i++)
            {
                columnMap.Add(columns[i].ColumnName, columns[i].Description);
            }
            for (int i = 0; i < this.designView.GridColumns.Length; i++)
            {
                this.lstUnSelectColumn.Items.Add(new ListItem(columnMap[this.designView.GridColumns[i].ColumnName], this.designView.GridColumns[i].ColumnName));
            }
            if (dataSource.SourceType == DataSourceType.SQL)
            {
                bIsSqlDataSource = true;
            }
        }

        private void InitGrid()
        {
            this.gridWebGrid.Columns.Clear();
            this.gridHelper.AddColumn("Sequence", "����", null);
            this.gridHelper.AddColumn("ColumnName", "��λ����", null);
            this.gridHelper.AddColumn("ColumnDesc", "��λ����", null);
            this.gridHelper.AddColumn("GroupTotalType", "������λ���㷽ʽ", null);

            this.gridWebGrid.Columns.FromKey("ColumnName").Hidden = true;
            ((BoundDataField)this.gridWebGrid.Columns.FromKey("GroupTotalType")).HtmlEncode = false;
            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override void DisplayDesignData()
        {
            string strHidValue = "";
            if (this.designView.GridGroups != null)
            {
                for (int i = 0; i < this.designView.GridGroups.Length; i++)
                {
                    ListItem item = this.lstUnSelectColumn.Items.FindByValue(this.designView.GridGroups[i].ColumnName);
                    if (item == null)
                        continue;
                    AddRowToGrid(item.Value, item.Text);
                    this.lstUnSelectColumn.Items.Remove(item);

                    string strOneGroup = "";
                    for (int n = 0; this.designView.GridGroupTotals != null && n < this.designView.GridGroupTotals.Length; n++)
                    {
                        if (this.designView.GridGroupTotals[n].GroupSequence == this.designView.GridGroups[i].GroupSequence)
                        {
                            strOneGroup += this.designView.GridGroupTotals[n].ColumnName + "," + this.designView.GridGroupTotals[n].TotalType + ";";
                        }
                    }
                    strHidValue += this.designView.GridGroups[i].ColumnName + "@" + strOneGroup + "|";
                }
                this.hidGroupTotal.Value = strHidValue;
            }
        }

        protected override bool ValidateInput()
        {
            return true;
        }

        protected override void RedirectToBack()
        {
            this.Response.Redirect("FRptDesignStep2MP.aspx");
        }

        protected override void RedirectToNext()
        {
            this.Response.Redirect("FRptDesignStep4MP.aspx");
        }

        protected override void UpdateReportDesignView()
        {
            string strGroupTotalValue = this.hidGroupTotal.Value;
            string[] strGroupTotalList = strGroupTotalValue.Split('|');
            RptViewGridGroup[] groups = new RptViewGridGroup[this.gridWebGrid.Rows.Count];
            ArrayList listGroupTotal = new ArrayList();
            for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
            {
                RptViewGridGroup grp = new RptViewGridGroup();
                grp.GroupSequence = i;
                grp.ColumnName = this.gridWebGrid.Rows[i].Items.FindItemByKey("ColumnName").Value.ToString();
                for (int n = 0; n < strGroupTotalList.Length; n++)
                {
                    string[] strGroupOne = strGroupTotalList[n].Split('@');
                    if (strGroupOne[0] == grp.ColumnName)
                    {
                        string[] strGroupTotals = strGroupOne[1].Split(';');
                        for (int x = 0; x < strGroupTotals.Length; x++)
                        {
                            if (strGroupTotals[x] != "")
                            {
                                string[] strTmpList = strGroupTotals[x].Split(',');
                                RptViewGridGroupTotal grpTotal = new RptViewGridGroupTotal();
                                grpTotal.GroupSequence = i;
                                grpTotal.ColumnName = strTmpList[0];
                                grpTotal.TotalType = strTmpList[1];
                                listGroupTotal.Add(grpTotal);
                            }
                        }
                        break;
                    }
                }
                groups[i] = grp;
            }
            this.designView.GridGroups = groups;
            this.designView.GridGroupTotals = new RptViewGridGroupTotal[listGroupTotal.Count];
            listGroupTotal.CopyTo(this.designView.GridGroupTotals);
        }

        #endregion

        void imgSelect_ServerClick(object sender, ImageClickEventArgs e)
        {
            for (int i = this.lstUnSelectColumn.Items.Count - 1; i >= 0; i--)
            {
                if (this.lstUnSelectColumn.Items[i].Selected == true)
                {
                    this.AddRowToGrid(this.lstUnSelectColumn.Items[i].Value, this.lstUnSelectColumn.Items[i].Text);
                    this.lstUnSelectColumn.Items.RemoveAt(i);
                }
            }
        }

        private void AddRowToGrid(string columnName, string columnDesc)
        {
            DataRow row = DtSource.NewRow();
            row["GUID"] = Guid.NewGuid().ToString();
            row["Sequence"] = this.gridWebGrid.Rows.Count + 1;
            row["ColumnName"] = columnName;
            row["ColumnDesc"] = columnDesc;
            row["GroupTotalType"] = "<a href='' onclick=\"OpenTotalSetWindow('" + this.gridWebGrid.Rows.Count + "');return false;\">Click</a>";
            DtSource.Rows.Add(row);
            this.gridWebGrid.DataSource = DtSource;
            this.gridWebGrid.DataBind();
        }

        void imgUnSelect_ServerClick(object sender, ImageClickEventArgs e)
        {
            if (this.gridWebGrid.Behaviors.CreateBehavior<Activation>().ActiveCell == null)
                return;
            int activeRowIndex = this.gridWebGrid.Behaviors.Activation.ActiveCell.Row.Index;
            DataRow row = DtSource.Rows[activeRowIndex];
            string columnName = row["ColumnName"].ToString();
            string columnDesc = row["ColumnDesc"].ToString();
            this.lstUnSelectColumn.Items.Add(new ListItem(columnDesc, columnName));
            this.DtSource.Rows.Remove(row);
            for (int i = 0; i < this.DtSource.Rows.Count; i++)
            {
                this.DtSource.Rows[i]["Sequence"] = (i + 1).ToString();
            }
            this.gridWebGrid.DataSource = DtSource;
            this.gridWebGrid.DataBind();
        }

        protected void lstUnSelectColumn_Init(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "lstUnSelectColumn_Init();", true);
        }
       
        protected void SetColumnDown(object sender, ImageClickEventArgs e)
        {
            if (ActiveRowIndex == null)
                return;
            int activeRowIndex = (int)ActiveRowIndex;

            DataRow row1 = DtSource.Rows[activeRowIndex - 1];
            DataRow tempRow = DtSource.NewRow();
            tempRow["ColumnName"] = row1["ColumnName"];
            tempRow["ColumnDesc"] = row1["ColumnDesc"];
            tempRow["GroupTotalType"] = row1["GroupTotalType"];
            row1["ColumnName"] = DtSource.Rows[activeRowIndex]["ColumnName"];
            row1["ColumnDesc"] = DtSource.Rows[activeRowIndex]["ColumnDesc"];
            row1["GroupTotalType"] = DtSource.Rows[activeRowIndex]["GroupTotalType"];
            DataRow row2 = DtSource.Rows[activeRowIndex];
            row2["ColumnName"] = tempRow["ColumnName"];
            row2["ColumnDesc"] = tempRow["ColumnDesc"];
            row2["GroupTotalType"] = tempRow["GroupTotalType"];
            this.gridWebGrid.DataSource = DtSource;
            this.gridWebGrid.DataBind();
        }

        protected void SetColumnUp(object sender, ImageClickEventArgs e)
        {
            if (ActiveRowIndex == null)
                return;
            int activeRowIndex = (int)ActiveRowIndex;
           
            DataRow row1 = DtSource.Rows[activeRowIndex];
            DataRow tempRow = DtSource.NewRow();
            tempRow["ColumnName"] = row1["ColumnName"];
            tempRow["ColumnDesc"] = row1["ColumnDesc"];
            tempRow["GroupTotalType"] = row1["GroupTotalType"];
            row1["ColumnName"] = DtSource.Rows[activeRowIndex + 1]["ColumnName"];
            row1["ColumnDesc"] = DtSource.Rows[activeRowIndex + 1]["ColumnDesc"];
            row1["GroupTotalType"] = DtSource.Rows[activeRowIndex + 1]["GroupTotalType"];
            DataRow row2 = DtSource.Rows[activeRowIndex + 1];
            row2["ColumnName"] = tempRow["ColumnName"];
            row2["ColumnDesc"] = tempRow["ColumnDesc"];
            row2["GroupTotalType"] = tempRow["GroupTotalType"];
            this.gridWebGrid.DataSource = DtSource;
            this.gridWebGrid.DataBind();
        }
    }
}
