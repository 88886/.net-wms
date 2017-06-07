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
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.SMT;
using BenQGuru.eMES.Domain.SMT;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.SMT
{
    /// <summary>
    /// FReelMP ��ժҪ˵����
    /// </summary>
    public partial class FCheckSMTMaterialMP : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        protected System.Web.UI.WebControls.Label lblCheckResult;

        private BenQGuru.eMES.SMT.SMTFacade _facade;//= new SMTFacadeFactory(base.DataProvider).Create();
        protected GridHelperNew _gridHelper = null;
        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
            this.gridWebGrid.InitializeRow += new Infragistics.Web.UI.GridControls.InitializeRowEventHandler(gridWebGrid_InitializeRow);
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
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
            this.gridHelper2 = new GridHelperNew(this.gridWebGrid2, this.DtSource2);
            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
            }
            this.cmdConfirmDifference.Attributes.Add("onclick", "return confirm('" + this.languageComponent1.GetString("$SMTCheckMaterial_AcceptException_Confirm") + "');");
            //InitGridErrorRowBackColor();
        }

        private bool InitGridErrorRowBackColor()
        {
            int columnCount = gridWebGrid.Columns.Count;
            bool returnValue = true;
            foreach (GridRecord row in gridWebGrid.Rows)
            {
                if (!string.IsNullOrEmpty(row.Items.FindItemByKey("CheckResult").Text))
                {
                    for (int i = 0; i < columnCount; i++)
                    {
                        // row.CssClass = "ForeColorRed";
                        string strScript = string.Format(@"
                         $('#gridWebGrid').children('table').children('tbody').children('tr').children('td').children('table').children('tbody:eq(1)').children('tr').children('td').children('div').children('table').children('tbody').children('tr:eq({0})').children('td:eq({1})').css('color','red');
                               ",
                                       row.Index, i);

                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
                    }
                    returnValue = false;
                }
            }
            return returnValue;
        }

        void gridWebGrid_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
        {
            int columnCount = gridWebGrid.Columns.Count;
            //���ԭ���ĸ�ʽ�������Ϊ��ɫ
            for (int i = 0; i < columnCount; i++)
            {
                string strScript = string.Format(@"
                         $('#gridWebGrid').children('table').children('tbody').children('tr').children('td').children('table').children('tbody:eq(1)').children('tr').children('td').children('div').children('table').children('tbody').children('tr:eq({0})').children('td:eq({1})').css('color','black');
                               ",
                                                  e.Row.Index, i);

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);

            }
            //�����ʽ���д�����������Ϊ��ɫ
            if (!string.IsNullOrEmpty(e.Row.Items.FindItemByKey("CheckResult").Text))
            {
                for (int i = 0; i < columnCount; i++)
                {
                    string strScript = string.Format(@"
                         $('#gridWebGrid').children('table').children('tbody').children('tr').children('td').children('table').children('tbody:eq(1)').children('tr').children('td').children('div').children('table').children('tbody').children('tr:eq({0})').children('td:eq({1})').css('color','red');
                               ",
                                    e.Row.Index, i);

                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
                }
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
            this.gridHelper.AddColumn("ItemCode", "��Ʒ����", null);
            this.gridHelper.AddColumn("MachineCode", "��̨����", null);
            this.gridHelper.AddColumn("MachineStationCode", "վλ", null);
            this.gridHelper.AddColumn("FeederSpecCode", "Feeder���", null);
            this.gridHelper.AddColumn("SourceMaterialCode", "�����ϴ���", null);
            this.gridHelper.AddColumn("MaterialCode", "���ϴ���", null);
            this.gridHelper.AddColumn("ItemQty", "��׼����", HorizontalAlign.Right);
            this.gridHelper.AddColumn("Table", "Table", null);
            this.gridHelper.AddColumn("IDMerge", "�������", HorizontalAlign.Right);
            this.gridHelper.AddColumn("CheckResult", "�ȶԽ��", null);
            //this.gridWebGrid.Columns.FromKey("ItemQty").Format = "#,#";
            //this.gridWebGrid.Columns.FromKey("ItemQty").CellStyle.HorizontalAlign = HorizontalAlign.Right;
            //this.gridWebGrid.Columns.FromKey("IDMerge").CellStyle.HorizontalAlign = HorizontalAlign.Right;

            this.gridHelper.AddDefaultColumn(false, false);
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            //this.gridHelper.RequestData();
            //melo �޸���2006.12.5 ���ڶ�����
            //			this.gridExcept.Columns.Clear();
            //			this.gridExcept.Columns.Add("MOCode", "��������");
            //			this.gridExcept.Columns.Add("MaterialCode", "���ϴ���");
            //			this.gridExcept.Columns.Add("MaterialName", "��������");
            //			this.gridExcept.Columns.Add("ItemQty", "��׼����");
            //			this.gridExcept.Columns.Add("UOM", "������λ");


        }

        protected override void InitWebGrid2()
        {
            base.InitWebGrid2();
            this.gridHelper2.AddColumn("MOCode", "��������", null);
            this.gridHelper2.AddColumn("MaterialCode", "���ϴ���", null);
            this.gridHelper2.AddColumn("MaterialName", "��������", null);
            this.gridHelper2.AddColumn("ItemQty", "��׼����", null);
            this.gridHelper2.AddColumn("UOM", "������λ", null);
            this.gridHelper2.ApplyLanguage(this.languageComponent1);
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null) { _facade = new SMTFacadeFactory(base.DataProvider).Create(); }
            return _facade.CheckSMTMaterial(this.txtMOCodeQuery.Text.Trim().ToUpper(), this.txtSSCodeQuery.Text.Trim().ToUpper(), this.GetUserCode(), false);
        }

        protected void cmdCompare_ServerClick(object sender, EventArgs e)
        {

            this.DtSource.Rows.Clear();
            this.DtSource2.Rows.Clear();
            this.gridWebGrid.DataSource = DtSource;
            this.gridWebGrid.DataBind();
            this.gridWebGrid2.DataSource = DtSource2;
            this.gridWebGrid2.DataBind();
            this.txtCheckResult.Text = string.Empty;
            if (_facade == null) { _facade = new SMTFacadeFactory(base.DataProvider).Create(); }
            SMTCheckMaterialDetail[] details = _facade.CheckSMTMaterial(this.txtMOCodeQuery.Text.Trim().ToUpper(), this.txtSSCodeQuery.Text.Trim().ToUpper(), this.GetUserCode(), true);
            if (details == null || details.Length == 0)
                return;

            Hashtable htSMTQty = new Hashtable();
            foreach (SMTCheckMaterialDetail detail in details)
            {
                if (htSMTQty.ContainsKey(detail.MaterialCode))
                {
                    htSMTQty[detail.MaterialCode] = decimal.Parse(htSMTQty[detail.MaterialCode].ToString()) + detail.SMTQty;
                }
                else
                {
                    htSMTQty.Add(detail.MaterialCode, detail.SMTQty);
                }
            }

            bool bExistError = false;
            Hashtable htMsg = new Hashtable();
            int i;
            for (i = 0; i < details.Length; i++)
            {
                if (details[i].Type == SMTCheckMaterialDetailType.SMT)
                {
                    if (this.chbShowErrorOnly.Checked == false ||
                        this.chbShowErrorOnly.Checked == true && FormatHelper.StringToBoolean(details[i].CheckResult) == false)
                    {
                        if (htMsg.ContainsKey(details[i].CheckDescription) == false)
                            htMsg.Add(details[i].CheckDescription, this.languageComponent1.GetString(details[i].CheckDescription));
                        DataRow row = this.DtSource.NewRow();
                        row["GUID"] = Guid.NewGuid().ToString();
                        row["ItemCode"] = details[i].ProductCode;
                        row["MachineCode"] = details[i].MachineCode;
                        row["MachineStationCode"] = details[i].MachineStationCode;
                        row["FeederSpecCode"] = details[i].FeederSpecCode;
                        row["SourceMaterialCode"] = details[i].SourceMaterialCode;
                        row["MaterialCode"] = details[i].MaterialCode;
                        row["ItemQty"] = details[i].SMTQty.ToString("#,#");
                        row["Table"] = details[i].EAttribute1;
                        row["IDMerge"] = (details[i].BOMQty != 0 ? (decimal.Parse(htSMTQty[details[i].MaterialCode].ToString()) / details[i].BOMQty).ToString() : string.Empty);
                        row["CheckResult"] = htMsg[details[i].CheckDescription];
                        this.DtSource.Rows.Add(row);

                    }
                }
                else
                {
                    DataRow row = this.DtSource2.NewRow();
                    row["GUID"] = Guid.NewGuid().ToString();
                    row["MOCode"] = this.txtMOCodeQuery.Text.Trim().ToUpper();
                    row["MaterialCode"] = details[i].MaterialCode;
                    row["MaterialName"] = details[i].MaterialName;
                    row["ItemQty"] = details[i].BOMQty;
                    row["UOM"] = details[i].BOMUOM;
                    this.DtSource2.Rows.Add(row);
                    this.gridWebGrid2.DataSource = this.DtSource2;
                    this.gridWebGrid2.DataBind();
                    bExistError = true;
                }
            }
            this.gridWebGrid.DataSource = this.DtSource;
            this.gridWebGrid.DataBind();

            if (!InitGridErrorRowBackColor())
            {
                bExistError = true;
            }
            this.cmdConfirmDifference.Disabled = !bExistError;
            if (bExistError == true)
            {
                this.txtCheckResult.Text = "FAIL";
                this.txtCheckResult.ForeColor = Color.Red;
            }
            else
            {
                this.txtCheckResult.Text = "PASS";
                this.txtCheckResult.ForeColor = Color.Green;
            }
        }

        protected void cmdConfirm_ServerClick(object sender, EventArgs e)
        {
            if (_facade == null) { _facade = new SMTFacadeFactory(base.DataProvider).Create(); }
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.BeginTransaction();
            try
            {
                _facade.CheckSMTMaterialConfirm(this.txtMOCodeQuery.Text.Trim().ToUpper(), this.txtSSCodeQuery.Text.Trim().ToUpper(), this.GetUserCode());
                this.cmdConfirmDifference.Disabled = true;
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CommitTransaction();
            }
            catch (Exception ex)
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.RollbackTransaction();
                throw ex;
            }
            finally
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }
        }
        #endregion

        #region Export
        protected override void cmdExport_Click(object sender, System.EventArgs e)
        {
            this.excelExporter.Export();
            InitGridErrorRowBackColor();
        }
        Hashtable htExpMsg = null;
        protected override string[] FormatExportRecord(object obj)
        {
            SMTCheckMaterialDetail dtl = (SMTCheckMaterialDetail)obj;
            if (htExpMsg == null)
                htExpMsg = new Hashtable();
            if (dtl.CheckDescription != null && htExpMsg.ContainsKey(dtl.CheckDescription) == false)
                htExpMsg.Add(dtl.CheckDescription, this.languageComponent1.GetString(dtl.CheckDescription));

            if (dtl.Type == SMTCheckMaterialDetailType.SMT)
            {
                return new string[]{
								   dtl.ProductCode,
								   dtl.MachineCode,
								   dtl.MachineStationCode,
								   dtl.FeederSpecCode,
								   dtl.SourceMaterialCode,
								   dtl.MaterialCode,
								   dtl.SMTQty.ToString(),
                                   dtl.EAttribute1.ToString(),
								   (dtl.BOMQty != 0 ? (dtl.SMTQty / dtl.BOMQty).ToString() : string.Empty),
								   (dtl.CheckDescription != null ? htExpMsg[dtl.CheckDescription].ToString() : string.Empty)
							   };
            }
            else
            {
                return null;
            }
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	
									"ItemCode",
									"MachineCode",
									"MachineStationCode",
									"FeederSpecCode",
									"SourceMaterialCode",
									"MaterialCode",
									"ItemQty",
                                    "Table",
									"IDMerge",
									"CheckResult"};
        }
        #endregion

    }
}
