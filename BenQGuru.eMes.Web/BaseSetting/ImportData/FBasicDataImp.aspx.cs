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

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common;

using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.BaseSetting
{
    /// <summary>
    /// FBasicDataImp ��ժҪ˵����
    /// </summary>
    public partial class FBasicDataImp : BaseMPageMinus
    {
        protected System.Web.UI.WebControls.Label lblTitle;
        protected System.Web.UI.WebControls.Label lblBOMDownloadTitle;
        protected System.Web.UI.HtmlControls.HtmlInputButton cmdReturn;
        //private System.ComponentModel.IContainer components;

        //protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;

        //private GridHelper gridHelper = null;
        private ButtonHelper buttonHelper = null;
        protected System.Web.UI.WebControls.TextBox ErrorLog;
        private string ImportXMLPath = string.Empty;
        protected BenQGuru.eMES.Web.Helper.ExcelExporter excelExporter;
        protected UpdatePanel UpdatePanel1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //PostBackTrigger triggerCmdView = new PostBackTrigger();
            //triggerCmdView.ControlID = this.cmdView.ID;
            //(this.impForm.FindControl("up1") as UpdatePanel).Triggers.Add(triggerCmdView);
            //PostBackTrigger triggerInputTypeDDL = new PostBackTrigger();
            //triggerInputTypeDDL.ControlID = this.InputTypeDDL.ID;
            //(this.impForm.FindControl("up1") as UpdatePanel).Triggers.Add(triggerInputTypeDDL);
            //PostBackTrigger triggerCmdEnter = new PostBackTrigger();
            this.InitUI();

            //this.cmdView.Attributes.Add("onclick", "return Check();");
            if (!IsPostBack)
            {
                this.ImportXMLPath = this.Request.MapPath("") + @"\ImportBasicData.xml";
                this.XmlHelper = new BenQGuru.eMES.Web.BaseSetting.ImportData.ImportXMLHelper(ImportXMLPath);
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                // ��ʼ��radiobuttonlist
                this.ImportList.Items.Add(new ListItem(this.languageComponent1.GetString("$PageControl_Jump"), "Skip"));
                this.ImportList.Items.Add(new ListItem(this.languageComponent1.GetString("$PageControl_RollBack"), "RoolBack"));
                this.ImportList.SelectedValue = "Skip";

                // ��ʼ������UI
                this.BuildDataTypeDDL();
                
                InitHander();
                //this.InitWebGrid();
            }
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
        }
        //protected override void AddParsedSubObject(object obj)
        //{
        //    this.needUpdatePanel = false;
        //    base.AddParsedSubObject(obj);
        //}
        public ImportData.ImportXMLHelper XmlHelper
        {
            get
            {
                if (this.ViewState["XmlHelper"] != null)
                {
                    return this.ViewState["XmlHelper"] as ImportData.ImportXMLHelper;
                }
                else
                {
                    return new BenQGuru.eMES.Web.BaseSetting.ImportData.ImportXMLHelper(ImportXMLPath);
                }
            }
            set
            {
                this.ViewState["XmlHelper"] = value;
            }
        }

        public string InputType
        {
            get
            {
                if (this.ViewState["InputType"] != null)
                {
                    return this.ViewState["InputType"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["InputType"] = value;
            }
        }

        public string UploadedFileName
        {
            get
            {
                if (this.ViewState["UploadedFileName"] != null)
                {
                    return this.ViewState["UploadedFileName"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["UploadedFileName"] = value;
            }
        }

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
            //this.components = new System.ComponentModel.Container();
            //this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
            // 
            // languageComponent1
            // 
            //this.languageComponent1.Language = "CHS";
            //this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
            //this.languageComponent1.RuntimePage = null;
            //this.languageComponent1.RuntimeUserControl = null;
            //this.languageComponent1.UserControlName = "";

        }
        #endregion

        private void InitHander()
        {
            this.buttonHelper = new ButtonHelper(this);
            //this.gridHelper.Grid.DisplayLayout.AllowUpdateDefault = AllowUpdate.No;
        }

        /// <summary>
        /// ��ʼ��Grid����λ
        /// </summary>
        protected override void InitWebGrid()
        {
            DtSource = new DataTable();
            this.gridWebGrid.Columns.Clear();

            //			ArrayList htable = this.XmlHelper.GridBuilder;
            ArrayList htable = this.XmlHelper.GetGridBuilder(this.languageComponent1);
            if (htable.Count > 0)
            {
                this.gridHelper.Grid.Visible = true;
                
                this.gridHelper.AddDefaultColumn(true, false);
                foreach (DictionaryEntry de in htable)
                {
                    this.gridHelper.AddColumn(de.Key.ToString(), de.Value.ToString(), null);
                }
                this.gridHelper.AddColumn("ImportResult", "������", null);
                base.SetGridHeightAndWidth(true);
                //this.gridWebGrid.DataSource = this.DtSource;
                //this.gridWebGrid.DataBind();
            }
            base.InitWebGrid();

 
        }

        /// <summary>
        /// ����xml����dropDownlist
        /// </summary>
        private void BuildDataTypeDDL()
        {
            this.InputTypeDDL.Items.Clear();
            this.InputTypeDDL.Items.Add(new ListItem("", ""));

            //			ArrayList htable = this.XmlHelper.ImportType;
            ArrayList htable = this.XmlHelper.GetImportType(this.languageComponent1);
            if (htable.Count > 0)
            {
                foreach (DictionaryEntry de in htable)
                {
                    this.InputTypeDDL.Items.Add(
                        new ListItem(de.Value.ToString(), de.Key.ToString()));
                }
            }
        }



        protected void InputTypeDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            InputType = InputTypeDDL.SelectedValue;
            this.XmlHelper.SelectedImportType = InputType;
            this.lblCount.Text = "0";
            if (InputType.Length == 0)
            {
                //chbSelectAll.Visible = false;
                this.gridHelper.Grid.Visible = false;
                this.cmdEnter.Disabled = true;
                this.cmdView.Disabled = true;
                this.DownLoadPathBom.Disabled = true;
                aFileDownLoad.HRef = string.Empty;
                //this.chbSelectAll.Enabled = false ;
                this.cmdGridExport.Disabled = false;
                return;
            }

            //chbSelectAll.Visible = true;
            this.cmdEnter.Disabled = true;
            this.cmdView.Disabled = false;
            this.DownLoadPathBom.Disabled = false;
            //melo �޸���2006.12.8 ���ڶ�����
            if (this.languageComponent1.Language.ToString() == "CHT")
            {
                InputType = InputType + "CHT";
            }
            else if (this.languageComponent1.Language.ToString() == "ENU")
            {
                InputType = InputType + "ENU";
            }
            aFileDownLoad.HRef = string.Format(@"{0}download\{1}.xls", this.VirtualHostRoot, InputType);
            //this.chbSelectAll.Enabled = true ;
            this.cmdGridExport.Disabled = false;
            //this.gridHelper.Grid.Columns.Clear();
            //this.gridHelper.Grid.Rows.Clear();
            this.InitWebGrid();
            this.gridWebGrid.ClearDataSource();
            this.gridWebGrid.DataBind();

            this.gridWebGrid.DataSource = DtSource;
            this.gridWebGrid.DataBind();
        }

        public ArrayList GetAllRows()
        {
            ArrayList array = new ArrayList();
            foreach (GridRecord row in gridWebGrid.Rows)
            {
                array.Add(row);
            }

            if (array.Count == 0)
            {
                //û�����ݣ�Ҫ�ı�����Ϣ
                gridHelper.ShowMessageDiv(this.Page, GridHelper._warningNoRow);
            }

            return array;
        }

        protected void cmdQuery_ServerClick(object sender, System.EventArgs e)
        {
            string fileName = FileLoadProcess.UploadFile2ServerUploadFolder(this.Page, this.DownLoadPathBom, null);
            if (fileName == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_UploadFileIsEmpty");
            }
            string fileName2 = fileName.ToUpper();
            if (fileName2.LastIndexOf(".XLS") == -1)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_UploadFileTypeError_XLS");
            }

            this.UploadedFileName = fileName;

            this.RequestData();

            ArrayList importArray = this.GetAllRows();

            if (importArray.Count == 0)
            {
                return;
            }
            DataTable dt = this.GetImportDT(importArray);

            ImportData.ImportDateEngine importEngine = new BenQGuru.eMES.Web.BaseSetting.ImportData.ImportDateEngine(
                base.DataProvider, this.InputType, dt, this.GetUserCode(), importArray, this.gridHelper);

            importEngine.CheckDataValid();
            foreach (GridRecord row in gridWebGrid.Rows)
            {
                if (!string.IsNullOrEmpty(row.Items.FindItemByKey("ImportResult").Text))
                {
                    this.cmdEnter.Disabled = true;
                    this.cmdGridExport.Disabled = false;
                    return;
                }
            }
            this.cmdEnter.Disabled = false;
            this.cmdGridExport.Disabled = true;
        }

        private void RequestData()
        {
            this.InitWebGrid();
            ImportExcel imXls = new ImportExcel(this.UploadedFileName, this.InputType, this.XmlHelper.GridBuilder, this.XmlHelper.NotAllowNullField);
            DtSource= imXls.XlaDataTable;
            DtSource.Columns.Add(this.gridHelper.CheckColumnKey,typeof(bool));
            DtSource.Columns.Add("GUID");
            DtSource.Columns.Add("ImportResult");
            DtSource.AcceptChanges();
            foreach (DataRow row in DtSource.Rows)
            {
                row[this.gridHelper.CheckColumnKey] = false;
                row["GUID"] = Guid.NewGuid().ToString();
            }
            this.gridWebGrid.ClearDataSource();
            this.gridWebGrid.DataBind();
            gridWebGrid.DataSource = DtSource;
            gridWebGrid.DataBind();
            this.lblCount.Text = DtSource.Rows.Count.ToString();
        }

        protected void cmdAdd_ServerClick(object sender, EventArgs e)
        {
            //RequestData();
            //ArrayList importArray = this.gridHelper.GetCheckedRows();
            ArrayList importArray = this.gridHelper.GetCheckedRows();//����һ�� �߼��������������
            if (importArray.Count == 0)
            {
                return;
            }

            DataTable dt = this.GetImportDT(importArray);

            ImportData.ImportDateEngine importEngine = new BenQGuru.eMES.Web.BaseSetting.ImportData.ImportDateEngine(
                base.DataProvider, this.InputType, dt, this.GetUserCode(), importArray, this.gridHelper);

            //this.gridHelper.AddColumn("ImportResult","������",150);

            #region ��֤������Ч��
            //try
            //{
            //    /*added by jessie lee, 2005/11/30,
            //     * ����ʱ�����ʱ��ӽ�����*/

            //    this.Page.Response.Write("<div id='mydiv' >"); 
            //    this.Page.Response.Write("_"); 
            //    this.Page.Response.Write("</div>"); 
            //    this.Page.Response.Write("<script>mydiv.innerText = '';</script>"); 
            //    this.Page.Response.Write("<script language=javascript>;"); 
            //    this.Page.Response.Write("var dots = 0;var dotmax = 10;function ShowWait()"); 
            //    this.Page.Response.Write("{var output; output = '������֤�������ݵ���Ч��,���Ժ�';dots++;if(dots>=dotmax)dots=1;"); 
            //    this.Page.Response.Write("for(var x = 0;x < dots;x++){output += '��';}mydiv.innerText =  output;}"); 
            //    this.Page.Response.Write("function StartShowWait(){mydiv.style.visibility = 'visible'; "); 
            //    this.Page.Response.Write("window.setInterval('ShowWait()',1000);}"); 
            //    this.Page.Response.Write("function HideWait(){mydiv.style.display = 'none';"); 
            //    this.Page.Response.Write("window.clearInterval();}"); 
            //    this.Page.Response.Write("StartShowWait();</script>"); 
            //    this.Page.Response.Flush();

            //    //importEngine.CheckDataValid();	

            //    this.Page.Response.Write("<script language=javascript>HideWait();</script>"); 

            //    /*
            //    string script1 = 
            //        "<div id='mydiv' >_</div>"+
            //        "<script>mydiv.innerText = '';</script>"+
            //        "<script language=javascript>;"+
            //        "var dots = 0;var dotmax = 10;function ShowWait()"+
            //        "{var output; output = '���ڵ�������,���Ժ�';dots++;if(dots>=dotmax)dots=1;"+
            //        "for(var x = 0;x < dots;x++){output += '��';}mydiv.innerText =  output;}"
            //        +"function StartShowWait(){mydiv.style.visibility = 'visible'; "+
            //        "window.setInterval('ShowWait()',1000);}"+
            //        "function HideWait(){mydiv.style.display = 'none';"+
            //        "window.clearInterval();}"+
            //        "StartShowWait();</script>";
            //    this.Page.RegisterStartupScript(Guid.NewGuid().ToString(),script1);
            //    importEngine.CheckDataValid();	
            //    this.Page.RegisterStartupScript(Guid.NewGuid().ToString(),"<script language=javascript>HideWait();</script>");
            //    */

            //}
            //catch(Exception ex)
            //{

            //    throw ex ;
            //}
            //finally
            //{
            //    this.Page.Response.Write("<script language=javascript>HideWait();</script>"); 
            //}
            #endregion

            #region ��������
            try
            {
                /*added by jessie lee, 2005/11/30,
                 * ����ʱ�����ʱ��ӽ�����*/

                //this.Page.Response.Write("<div id='mydiv2' >"); 
                //this.Page.Response.Write("_"); 
                //this.Page.Response.Write("</div>"); 
                //this.Page.Response.Write("<script>mydiv2.innerText = '';</script>"); 
                //this.Page.Response.Write("<script language=javascript>;"); 
                //this.Page.Response.Write("var dots = 0;var dotmax = 10;function ShowWait()"); 
                //this.Page.Response.Write("{var output; output = '���ڵ�������,���Ժ�';dots++;if(dots>=dotmax)dots=1;"); 
                //this.Page.Response.Write("for(var x = 0;x < dots;x++){output += '��';}mydiv2.innerText =  output;}"); 
                //this.Page.Response.Write("function StartShowWait(){mydiv2.style.visibility = 'visible'; "); 
                //this.Page.Response.Write("window.setInterval('ShowWait()',1000);}"); 
                //this.Page.Response.Write("function HideWait(){mydiv2.style.display = 'none';"); 
                //this.Page.Response.Write("window.clearInterval();}"); 
                //this.Page.Response.Write("StartShowWait();</script>"); 
                //this.Page.Response.Flush();

                bool isRollBack = this.ImportList.SelectedValue == "RoolBack" ? true : false;
                importEngine.Import(isRollBack);

                //this.Page.Response.Write("<script language=javascript>HideWait();</script>"); 

                /*
                string script2 = 
                    "<div id='mydiv2' >_</div>"+
                    "<script>mydiv2.innerText = '';</script>"+
                    "<script language=javascript>;"+
                    "var dots = 0;var dotmax = 10;function ShowWait()"+
                    "{var output; output = '���ڵ�������,���Ժ�';dots++;if(dots>=dotmax)dots=1;"+
                    "for(var x = 0;x < dots;x++){output += '��';}mydiv2.innerText =  output;}"
                    +"function StartShowWait(){mydiv2.style.visibility = 'visible'; "+
                    "window.setInterval('ShowWait()',1000);}"+
                    "function HideWait(){mydiv2.style.display = 'none';"+
                    "window.clearInterval();}"+
                    "StartShowWait();</script>";
                this.Page.RegisterStartupScript(Guid.NewGuid().ToString(),script2);
                importEngine.Import();	
                this.Page.RegisterStartupScript(Guid.NewGuid().ToString(),"<script language=javascript>HideWait();</script>");
                */

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //this.Page.Response.Write("<script language=javascript>HideWait();</script>"); 
            }
            #endregion

            if (importEngine.ErrorArray != null)
            {
                importEngine.ErrorArray = null;
            }

            this.cmdGridExport.Disabled = false;
            this.cmdEnter.Disabled = true;
        }

        private DataTable GetImportDT(ArrayList array)
        {
            /* ���ɱ�ṹ */
            DataTable dt = new DataTable();
            dt.Rows.Clear();
            dt.Columns.Clear();
            dt.Columns.Add(this.gridHelper.CheckColumnKey,typeof(bool));
            dt.Columns.Add("GUID");
            dt.Columns.Add("ImportResult");
            dt.AcceptChanges();
            ArrayList htable = this.XmlHelper.GridBuilder;
            string orgid = string.Empty;
            if (htable.Count > 0)
            {
                foreach (DictionaryEntry de in htable)
                {
                    string key = de.Key.ToString();
                    dt.Columns.Add(key);
                }

                //�����֯ID Add By Jarvis 20121213
                switch (this.InputType.ToUpper())
                {
                    case "ITEM":
                    case "MODEL":
                    case "ITEMLOCATION":
                    case "OPBOM":
                        htable.Insert(0, new DictionaryEntry("ORGID", "ORGID"));
                        dt.Columns.Add("ORGID");
                        Domain.BaseSetting.Organization org = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider).
                            GetUserDefaultOrgByUser(this.GetUserCode()) as Domain.BaseSetting.Organization;
                        if (org != null)
                        {
                            orgid = org.OrganizationID.ToString().ToUpper();
                        }
                        break;
                    default:
                        break;
                }
            }

            /* ������ */
            for (int i = 0; i < array.Count; i++)
            {
                GridRecord gridRow = array[i] as GridRecord;
                DataRow row = dt.NewRow();
                row["GUID"] = Guid.NewGuid().ToString();
                row["ImportResult"] = "";
                row[gridHelper.CheckColumnKey] = false;
                if (htable.Count > 0)
                {
                    foreach (DictionaryEntry de in htable)
                    {
                        string key = de.Key.ToString();

                        //�����֯ID Add By Jarvis 20121213
                        if (key.ToUpper() == "ORGID")
                        {
                            row[key] = orgid;
                            continue;
                        }

                        if (XmlHelper.NeedMatchCtoE(key))
                        {
                            string value1 = GetECode(key, gridRow.Items.FindItemByKey(key).Value.ToString());
                            if (gridRow.Items.FindItemByKey(key).Value.ToString().Trim().Length != 0
                                && value1.Length == 0)
                            {
                                ExceptionManager.Raise(this.GetType().BaseType, "$Error_UploadFile_ContentError");
                            }
                            else
                            {
                                row[key] = value1;
                            }
                        }
                        else
                        {
                            row[key] = gridRow.Items.FindItemByKey(key).Value.ToString();
                        }
                    }
                }
                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueWord"></param>
        /// <returns></returns>
        private string GetECode(string key, string valueWord)
        {
            return XmlHelper.GetMatchKeyWord(valueWord, XmlHelper.MatchType(key));
        }

        protected void cmdGridExport_ServerClick(object sender, System.EventArgs e)
        {
            WebDataGrid Grid2 = null;
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
            Grid2 = this.gridHelper.Grid;
            for (int i = Grid2.Rows.Count - 1; i >= 0; i--)
            {
                GridRecord row = Grid2.Rows[i];
                if (bool.Parse(row.Items.FindItemByKey(this.gridHelper.CheckColumnKey).Value.ToString())
                    && row.Items.FindItemByKey("ImportResult").Value.ToString() == "����ɹ�")
                {
                    Grid2.Rows.Remove(Grid2.Rows[i]);
                }
            }
            //Grid2.Columns.Remove(Grid2.Columns[0]);
            //this.WebGridExcelExporter1.Export(Grid2);

            //this.excelExporter.ExportNew(Grid2);
            this.GridExportExcColFir(Grid2);
        }

        private string[] FormatExportRecord(object obj)
        {
            return new string[]{
                
            };
        }

        private string[] GetColumnHeaderText()
        {
            return new string[] {	
              
            };

        }
    }
}
