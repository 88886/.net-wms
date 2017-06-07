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
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Common;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using BenQGuru.eMES.Web.MOModel.ImportData;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.MOModel
{
    /// <summary>
    /// FMODownload ��ժҪ˵����
    /// </summary>
    public partial class FMODownload : BaseMPageMinus
    {
        //protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        //private System.ComponentModel.IContainer components = null;

        private object[] mos = null;
        //private BenQGuru.eMES.Web.Helper.GridHelper gridHelper = null;
        private BenQGuru.eMES.MOModel.MOFacade facade;
        // = new FacadeFactory(base.DataProvider).CreateMOFacade();
        private BenQGuru.eMES.BaseSetting.SystemSettingFacade sysSettingFacade;// = new ItemFacadeFactory().CreateSystemSettingFacade()  ;
         protected UpdatePanel UpdatePanel1;
        //protected override void AddParsedSubObject(object obj)
        //{
        //    this.needUpdatePanel = false;
        //    if (obj is HtmlForm)
        //    {
        //        HtmlForm form1 = obj as HtmlForm;

        //        UpdatePanel up1 = new UpdatePanel();

        //        PostBackTrigger trigger = new PostBackTrigger();
        //        trigger.ControlID = this.cmdViewMO.ID;
        //        up1.Triggers.Add(trigger);

        //        up1.ID = "up1";
        //        foreach (Control formChildren in form1.Controls)
        //        {
        //            if (formChildren is HtmlTable)
        //            {
        //                up1.ContentTemplateContainer.Controls.Add(formChildren);
        //            }
        //        }
        //        form1.Controls.Add(up1);

        //    }
        //    base.AddParsedSubObject(obj);
        //}

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.InitOnPostBack();

            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                this.InitUI();
                string fileType = "ImportMO";
                if (this.languageComponent1.Language.ToString() == "CHT")
                {
                    fileType = fileType + "_CHT";
                }
                else if (this.languageComponent1.Language.ToString() == "ENU")
                {
                    fileType = fileType + "_ENU";
                }
                aFileDownLoad.HRef = string.Format(@"{0}download\{1}.xls", this.VirtualHostRoot, fileType);
                this.cmdEnter.Disabled = true;

                this.InitWebGrid();
            }

            
        }

        private void InitOnPostBack()
        {

            this.gridHelper = new GridHelperNew(this.gridWebGrid,this.DtSource);
            this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadDataSource);
            this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridRow);

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
            //// 
            //// languageComponent1
            //// 
            //this.languageComponent1.Language = "CHS";
            //this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
            //this.languageComponent1.RuntimePage = null;
            //this.languageComponent1.RuntimeUserControl = null;
            //this.languageComponent1.UserControlName = "";

        }
        #endregion

        protected void cmdCancel_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect(this.MakeRedirectUrl("FMOMP.aspx"));
        }

        protected void cmdView_ServerClick(object sender, System.EventArgs e)
        {
            string fileName = FileLoadProcess.UploadFile2ServerUploadFolder(this.Page, this.DownLoadPathMO, null);
            if (fileName == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_UploadFileIsEmpty");
            }
            //add by crystal chu 2005/07/15
            if (!(fileName.ToLower().EndsWith(".xls") || fileName.ToLower().EndsWith(".xlsx")))
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_UploadFileTypeError_XLS");
            }

            this.ViewState.Add("UploadedFileName", fileName);
            this.RequestData();
            //this.gridHelper.CheckAllRows(CheckStatus.Checked); // modify by Simone

            //���û���쳣��Ϣ�������밴ť��Ϊ����
            for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(this.gridWebGrid.Rows[i].Items.FindItemByKey("importException").Text))
                {
                    this.cmdEnter.Disabled = false;
                }
            }
            if (this.gridWebGrid.Rows.Count == 0)
            {
                this.cmdEnter.Disabled = true;
            }
        }

        private void InitWebGrid()
        {
            this.gridHelper.AddColumn("Factory", "����", null);
            this.gridHelper.AddColumn("MOCode", "����", null);
            this.gridHelper.AddColumn("ItemCode", "��Ʒ", null);
            this.gridHelper.AddColumn("MOType", "��������", null);
            this.gridHelper.AddColumn("MOPlanQTY", "�ƻ�����", null);
            this.gridHelper.AddColumn("MOPlanStartDate", "Ԥ�ƿ�������", null);
            this.gridHelper.AddColumn("MOPlanEndDate", "Ԥ���������", null);
            this.gridHelper.AddColumn("CustomerCode", "�ͻ�", null);
            this.gridHelper.AddColumn("CustomerOrderNO", "�ͻ�����", null);
            this.gridHelper.AddColumn("MOMemo", "��ע", null);
            this.gridHelper.AddColumn("BOMVersion", "����BOM", null);
            this.gridHelper.AddColumn("IsExist", "�Ƿ��Ѵ���", null);
            this.gridHelper.AddColumn("importException", "�쳣��Ϣ", null);
            //add by crystal chu 2005/07/15
            this.gridHelper.Grid.Columns.FromKey("IsExist").Hidden = true;

            this.gridHelper.AddDefaultColumn(true, false);
            //this.gridWebGrid.Columns.FromKey("importException").Width = 200;
            //this.gridWebGrid.Columns.FromKey("importException").CellStyle.ForeColor = System.Drawing.Color.Red;

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        private object[] LoadDataSource(int inclusive, int exclusive)
        {
            // TODO:����Ҫ��
            ArrayList newMOs = new ArrayList();
            if (mos == null)
            {
                this.GetAllMO();
            }
            for (int i = 1; i <= mos.Length; i++)
            {
                if (i >= inclusive && i <= exclusive)
                {
                    newMOs.Add(mos[i - 1]);
                }
            }
            return newMOs.ToArray();
        }


        protected DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["Factory"] = ((MO)obj).Factory;
            row["MOCode"] = ((MO)obj).MOCode;
            row["ItemCode"] = ((MO)obj).ItemCode;
            row["MOType"] = ((MO)obj).MOType;
            row["MOPlanQTY"] = ((MO)obj).MOPlanQty.ToString();
            row["MOPlanStartDate"] = FormatHelper.ToDateString(((MO)obj).MOPlanStartDate);
            row["MOPlanEndDate"] = FormatHelper.ToDateString(((MO)obj).MOPlanEndDate);
            row["CustomerCode"] = ((MO)obj).CustomerCode.ToString();
            row["CustomerOrderNO"] = ((MO)obj).CustomerOrderNO.ToString();
            row["MOMemo"] = ((MO)obj).MOMemo.ToString();
            row["BOMVersion"] = ((MO)obj).BOMVersion.ToString();
            row["IsExist"] = IsMOExist(((MO)obj).MOCode);
            row["importException"] = FormartErrorMessage(((MO)obj).EAttribute6);
            return row;
        }

        private string FormartErrorMessage(string errorMesssage)
        {
            if (!string.IsNullOrEmpty(errorMesssage))
            {
                errorMesssage = errorMesssage.TrimEnd(';').TrimStart(';');
            }
            return errorMesssage;
        }


        private string IsMOExist(string moCode)
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateMOFacade(); }
            MO mo = (MO)this.facade.GetMO(moCode);
            if (mo != null)
            {
                return FormatHelper.DisplayBoolean(true, this.languageComponent1);
            }
            else
            {
                return FormatHelper.DisplayBoolean(false, this.languageComponent1);
            }
        }

        private object[] GetAllMO()
        {
            string fileName = string.Empty;

            fileName = this.ViewState["UploadedFileName"].ToString();

            string configFile = this.getParseConfigFileName();

            DataFileParser parser = new DataFileParser();
            parser.FormatName = "MO";
            parser.ConfigFile = configFile;
            parser.CheckValidHandle = new CheckValid(this.MODownloadCheck);
            mos = parser.Parse(fileName);



            foreach (MO mo in mos)
            {
                mo.MOCode = FormatHelper.PKCapitalFormat(mo.MOCode);
                mo.ItemCode = FormatHelper.PKCapitalFormat(mo.ItemCode);
                mo.MODownloadDate = FormatHelper.TODateInt(DateTime.Today);
                mo.MOUser = this.GetUserCode();
                mo.MORemark = " ";
                mo.MaintainUser = this.GetUserCode();
                mo.MaintainDate = FormatHelper.TODateInt(DateTime.Today);
                mo.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);

            }

            return mos;

        }

        private object[] moObjects;
        private object[] GetAllMOByExcle()
        {
            try
            {
                string fileName = string.Empty;
                ArrayList columnList = new ArrayList();

                //�������������ȡ������
                string factory = string.Empty;// ����
                string mocode = string.Empty;// ����
                string itemcode = string.Empty;// ��Ʒ����
                string moType = string.Empty;// ��������
                string moPlanQty = string.Empty;// �����ƻ�����
                string planSDate = string.Empty;// �ƻ���ʼ����
                string planEDate = string.Empty;// �ƻ��������
                string customerCode = string.Empty;// �ͻ�����
                string customerOrder = string.Empty;// �ͻ�����
                string moMemo = string.Empty;
                string moBOM = string.Empty;

                string xmlPath = this.Request.MapPath("") + @"\ImportMOData.xml";
                string dataType = "ImportMO";
            
                ArrayList lineValues = new ArrayList();
                lineValues = GetXMLHeader(xmlPath, dataType);

                for (int i = 0; i < lineValues.Count; i++)
                {
                    DictionaryEntry dictonary = new DictionaryEntry();
                    dictonary = (DictionaryEntry)lineValues[i];
                    if (dictonary.Key.ToString().Equals(MoDictionary.Factory))
                    {
                        factory = dictonary.Value.ToString().ToUpper();
                        columnList.Add(factory);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.Mocode))
                    {
                        mocode = dictonary.Value.ToString().ToUpper();
                        columnList.Add(mocode);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.Itemcode))
                    {
                        itemcode = dictonary.Value.ToString().ToUpper();
                        columnList.Add(itemcode);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.MoType))
                    {
                        moType = dictonary.Value.ToString().ToUpper();
                        columnList.Add(moType);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.MoPlanQty))
                    {
                        moPlanQty = dictonary.Value.ToString().ToUpper();
                        columnList.Add(moPlanQty);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.PlanSDate))
                    {
                        planSDate = dictonary.Value.ToString().ToUpper();
                        columnList.Add(planSDate);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.PlanEDate))
                    {
                        planEDate = dictonary.Value.ToString().ToUpper();
                        columnList.Add(planEDate);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.CustomerCode))
                    {
                        customerCode = dictonary.Value.ToString().ToUpper();
                        columnList.Add(customerCode);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.CustomerOrder))
                    {
                        customerOrder = dictonary.Value.ToString().ToUpper();
                        columnList.Add(customerOrder);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.MoMemo))
                    {
                        moMemo = dictonary.Value.ToString().ToUpper();
                        columnList.Add(moMemo);
                    }
                    if (dictonary.Key.ToString().Equals(MoDictionary.MoBOM))
                    {
                        moBOM = dictonary.Value.ToString().ToUpper();
                        columnList.Add(moBOM);
                    }
                }

                if (this.ViewState["UploadedFileName"] == null)
                {
                    BenQGuru.eMES.Common.ExceptionManager.Raise(this.GetType().BaseType, "$Error_UploadFileIsEmpty");
                }
                fileName = this.ViewState["UploadedFileName"].ToString();

                //��ȡEXCEL��ʽ�ļ�
                System.Data.DataTable dt = new DataTable();
                try
                {
                    ImportXMLHelper xmlHelper=new ImportXMLHelper(xmlPath,dataType);
                    ArrayList gridBuilder = new ArrayList();
                    ArrayList notAllowNullField = new ArrayList();

                    gridBuilder = xmlHelper.GetGridBuilder(this.languageComponent1, dataType);
                    notAllowNullField = xmlHelper.GetNotAllowNullField(this.languageComponent1);

                    ImportExcel imXls = new ImportExcel(fileName, dataType, gridBuilder, notAllowNullField);
                     dt = imXls.XlaDataTable;
                  //  dt = GetExcelData(fileName);
                }
                catch (Exception)
                {
                    ExceptionManager.Raise(this.GetType().BaseType, "$GetExcelDataFiledCheckTemplate");
                }

                //��dt�������ݼ�飬ȥ������
                List<DataRow> removelist = new List<DataRow>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool rowdataisnull = true;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j].ToString().Trim() != "")
                        {
                            rowdataisnull = false;
                        }
                    }
                    if (rowdataisnull)
                    {
                        removelist.Add(dt.Rows[i]);
                    }
                }
                for (int i = 0; i < removelist.Count; i++)
                {
                    dt.Rows.Remove(removelist[i]);
                }

                CheckTemplateRight(dt, columnList);

                DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                MOFacade mofacade = new MOFacade(this.DataProvider);
                ArrayList objList = new ArrayList();
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int h = 0; h < dt.Rows.Count; h++)
                    {
                        MO mo = new MO();
                        try
                        {
                            //���DOMAIN,���ﲻ��Check
                            mo.MOCode = FormatHelper.CleanString(dt.Rows[h][mocode].ToString().ToUpper());
                            mo.Factory = FormatHelper.CleanString(dt.Rows[h][factory].ToString().ToUpper());
                            mo.ItemCode = FormatHelper.CleanString(dt.Rows[h][itemcode].ToString().ToUpper());
                            mo.MOType = FormatHelper.CleanString(dt.Rows[h][moType].ToString().ToUpper());
                            try
                            {
                                mo.MOPlanQty = Convert.ToDecimal(FormatHelper.CleanString(dt.Rows[h][moPlanQty].ToString().ToUpper()));
                            }
                            catch (Exception)
                            {
                                ExceptionManager.Raise(this.GetType().BaseType, "$MOPlanQty_Must_Be_Decimal");
                                return null;
                            }

                            mo.MOPlanStartTime = 0;
                            mo.MOPlanEndTime = 0;
                            try
                            {
                                mo.MOPlanStartDate = FormatHelper.TODateInt(FormatHelper.CleanString(dt.Rows[h][planSDate].ToString().ToUpper()));
                                mo.MOPlanEndDate = FormatHelper.TODateInt(FormatHelper.CleanString(dt.Rows[h][planEDate].ToString().ToUpper()));
                            }
                            catch (Exception)
                            {
                                ExceptionManager.Raise(this.GetType().BaseType,"$Data_Formart_Must_Be_YYYY-MM-DD");
                                return null;
                            }

                            mo.CustomerCode = FormatHelper.CleanString(dt.Rows[h][customerCode].ToString().ToUpper());
                            mo.CustomerOrderNO = FormatHelper.CleanString(dt.Rows[h][customerOrder].ToString().ToUpper());
                            mo.MOMemo = FormatHelper.CleanString(dt.Rows[h][moMemo].ToString().ToUpper());
                            mo.BOMVersion = FormatHelper.CleanString(dt.Rows[h][moBOM].ToString().ToUpper());

                            mo.MaintainUser = this.GetUserCode();
                            mo.MaintainDate = dbDateTime.DBDate;
                            mo.MaintainTime = dbDateTime.DBTime;

                            mo.MOInputQty = 0;
                            mo.MORemark = " ";
                            mo.MOScrapQty = 0;
                            mo.MOActualQty = 0;
                            mo.MOActualStartDate = 0;
                            mo.MOActualEndDate = 0;
                            mo.MOStatus = MOManufactureStatus.MOSTATUS_INITIAL;
                            mo.IDMergeRule = 1;
                            mo.OrganizationID = GlobalVariables.CurrentOrganizations.First().OrganizationID;
                            mo.MOPCBAVersion = "1";
                            mo.MOVersion = "1.0";
                            mo.MODownloadDate = dbDateTime.DBDate;   //��ǰʱ��
                            mo.IsControlInput = "0"; ;
                            mo.IsBOMPass = "9";
                            mo.MOUser = this.GetUserCode();

                            objList.Add(mo);
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.Raise(this.GetType().BaseType, ex.Message);
                        }
                    }
                }
                moObjects = (MO[])objList.ToArray(typeof(MO));

                //��鵱ǰ�����Ƿ���ϵ���Ҫ��(���ڷ���һ���Ƿ�ͨ�����ı�־)
                if (moObjects != null)
                {
                    foreach (MO mo in moObjects)
                    {
                        MODownloadCheck(mo);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionManager.Raise(this.GetType().BaseType, e.Message);
            }

            return moObjects;
        }

        private void RequestData()
        {
            if (mos == null)
            {
                // mos = GetAllMO() ;
                mos = GetAllMOByExcle();
            }

            //this.gridHelper.Grid.DisplayLayout.Pager.AllowPaging = true;
            //this.gridHelper.Grid.DisplayLayout.Pager.PageSize = int.MaxValue;
            this.gridHelper.GridBind(1, int.MaxValue);

            
        }        

        protected void cmdDownload_ServerClick(object sender, System.EventArgs e)
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateMOFacade(); }
            if (this.ViewState["UploadedFileName"] == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_NOExamineFile");
            }

            ArrayList array = this.gridHelper.GetCheckedRows();
            if (array.Count > 0)
            {
                ArrayList moCodes = new ArrayList(array.Count);

                foreach (GridRecord row in array)
                {
                    moCodes.Add(row.Items.FindItemByKey("MOCode").Value.ToString());
                }
                this.RequestData();

                this.DataProvider.BeginTransaction();
                try
                {
                    foreach (MO mo in this.mos)
                    {
                        foreach (string code in moCodes)
                        {
                            if (mo.MOCode == code)
                            {
                                /*CS187 �������ӡ��������ڡ���λ*/
                                //DateTime newDT = DateTime.Now;
                                //mo.MOImportDate = FormatHelper.TODateInt(newDT);
                                //mo.MOImportTime = FormatHelper.TOTimeInt(newDT);
                                DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                                mo.MOImportDate = dbTime.DBDate;
                                mo.MOImportTime = dbTime.DBTime;
                                mo.IDMergeRule = 1;
                                //mo.MORemark = string.Empty;
                                mo.OrganizationID = GlobalVariables.CurrentOrganizations.First().OrganizationID;
                                //�����ʾ��Ϣ
                                mo.EAttribute6 = string.Empty;
                                try
                                {
                                    facade.AddMO(mo);
                                    continue;
                                }
                                catch (Exception)
                                {
                                    this.DataProvider.RollbackTransaction();
                                    WebInfoPublish.Publish(this, "$MoCode_Import_Filed", this.languageComponent1);
                                    this.cmdEnter.Disabled = true;
                                }
                        
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.DataProvider.RollbackTransaction();
                    ExceptionManager.Raise(this.GetType().BaseType,ex.Message);
                }

                this.DataProvider.CommitTransaction();
                this.gridWebGrid.Rows.Clear();
                WebInfoPublish.Publish(this, "$MoCode_Import_Success", this.languageComponent1);
                this.cmdEnter.Disabled = true;
            }
        }

        private string getParseConfigFileName()
        {
            string configFile = this.Server.MapPath(this.TemplateSourceDirectory);
            if (configFile[configFile.Length - 1] != '\\')
            {
                configFile += "\\";
            }
            configFile += "DataFileParser.xml";
            return configFile;
        }

        /// <summary>
        /// �жϵ����MO�Ƿ���Ч
        /// </summary>
        /// <param name="obj">�����MO</param>
        /// <returns>�����Ч,����true,����,����false;����falseʱ,���MO�����ᱻ����</returns>
        private bool MODownloadCheck(object obj)
        {
            MO mo = (MO)obj;
            //�ǿռ��
            CheckMODomain(mo);

            if (sysSettingFacade == null) { sysSettingFacade = new BenQGuru.eMES.BaseSetting.SystemSettingFacade(base.DataProvider); }
            object[] parameters = sysSettingFacade.GetParametersByParameterGroup(MOType.GroupType);

            //add by crystal chu 2005/05/08
            if (parameters == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            if (mo.MOPlanStartDate > mo.MOPlanEndDate)
            {
               // ExceptionManager.Raise(this.GetType().BaseType, "$Error_MOPlanEndDateBigThanMOPlanStartDate");
                mo.EAttribute6 += ";"+this.languageComponent1.GetString("$Error_MOPlanEndDateBigThanMOPlanStartDate");
            }

            bool isParamExist = false;
            foreach (BenQGuru.eMES.Domain.BaseSetting.Parameter parameter in parameters)
            {
                if (mo.MOType == parameter.ParameterCode)
                {
                    isParamExist = true;
                    break;
                }
            }

            if (!isParamExist)
            {
                //ExceptionManager.Raise(this.GetType().BaseType, "$Error_MOType_NotExisted");
                //modified by kathy @20130812 ������ʾ��Ϣ���������Ͳ�����
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_MOType_NotExisted");
            }

            // ��� ItemCode
            BenQGuru.eMES.MOModel.ItemFacade itemFacade = new ItemFacade(base.DataProvider);
            object item = itemFacade.GetItem(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(mo.ItemCode)), GlobalVariables.CurrentOrganizations.First().OrganizationID);
            if (item == null)
            {
                //ExceptionManager.Raise(this.GetType().BaseType, "$Error_ItemCode_NotExist");
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_ItemCode_NotExist");
            }

            MOFacade moFacade = new FacadeFactory(base.DataProvider).CreateMOFacade();
            object isExistMO = moFacade.GetMO(FormatHelper.PKCapitalFormat(mo.MOCode));
            if (isExistMO != null)
            {
              //  ExceptionManager.Raise(this.GetType().BaseType, "$Error_MO_HasExisted", String.Format("[$MOCode='{0}']", mo.MOCode));
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_MO_HasExisted");
            }

            return true;
        }


        #region ��Excle�ж�ȡ����
        public DataTable GetExcelData(string astrFileName)
        {
            string strSheetName = GetExcelWorkSheets(astrFileName)[0].ToString();
            return GetExcelData(astrFileName, strSheetName);
        }

        /// <summary> 
        /// ����ָ���ļ��������Ĺ������б�;�����WorkSheet���ͷ����Թ���������������ArrayList�����򷵻ؿ� 
        /// </summary> 
        /// <param name="strFilePath">Ҫ��ȡ��Excel</param> 
        /// <returns>�����WorkSheet���ͷ����Թ���������������ArrayList�����򷵻ؿ�</returns> 
        public ArrayList GetExcelWorkSheets(string strFilePath)
        {
            ArrayList alTables = new ArrayList();
            OleDbConnection odn = new OleDbConnection(GetExcelConnection(strFilePath));
            odn.Open();
            DataTable dt = new DataTable();
            dt = odn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                throw new Exception("�޷���ȡָ��Excel�ļܹ���");
            }
            foreach (DataRow dr in dt.Rows)
            {
                string tempName = dr["Table_Name"].ToString();
                int iDolarIndex = tempName.IndexOf('$');
                if (iDolarIndex > 0)
                {
                    tempName = tempName.Substring(0, iDolarIndex);
                }
                //������Excel2003��ĳЩ����������Ϊ���ֵı��޷���ȷʶ���BUG�� 
                if (tempName[0] == '\'')
                {
                    if (tempName[tempName.Length - 1] == '\'')
                    {
                        tempName = tempName.Substring(1, tempName.Length - 2);
                    }
                    else
                    {
                        tempName = tempName.Substring(1, tempName.Length - 1);
                    }
                }
                if (!alTables.Contains(tempName))
                {
                    alTables.Add(tempName);
                }
            }
            odn.Close();
            if (alTables.Count == 0)
            {
                return null;
            }
            return alTables;
        }

        /// <summary> 
        /// ��ȡָ��·����ָ�����������Ƶ�Excel���� 
        /// </summary> 
        /// <param name="FilePath">�ļ��洢·��</param> 
        /// <param name="WorkSheetName">����������</param> 
        /// <returns>�����ȡ�ҵ������ݻ᷵��һ��������Table�����򷵻��쳣</returns> 
        public DataTable GetExcelData(string FilePath, string WorkSheetName)
        {
            DataTable dtExcel = new DataTable();
            OleDbConnection con = new OleDbConnection(GetExcelConnection(FilePath));
            OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from [" + WorkSheetName + "$]", con);
            //��ȡ 
            con.Open();
            adapter.FillSchema(dtExcel, SchemaType.Mapped);
            adapter.Fill(dtExcel);
            con.Close();
            dtExcel.TableName = WorkSheetName;
            //���� 
            return dtExcel;
        }

        /// <summary> 
        /// ��ȡ�����ַ��� 
        /// </summary> 
        /// <param name="strFilePath"></param> 
        /// <returns></returns> 
        public string GetExcelConnection(string strFilePath)
        {
            if (!File.Exists(strFilePath))
            {
                throw new Exception("ָ����Excel�ļ������ڣ�");
            }
            //return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended properties=\"Excel 8.0;Imex=1;HDR=Yes;\"";
            return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath + ";Extended properties=\"Excel 12.0;Imex=1;HDR=Yes;\"";
            //@"Provider=Microsoft.Jet.OLEDB.4.0;" + 
            //@"Data Source=" + strFilePath + ";" + 
            //@"Extended Properties=" + Convert.ToChar(34).ToString() + 
            //@"Excel 8.0;" + "Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString(); 
        }
        #endregion

        #region ��ȡExcle�ļ���������
        //��ȡxml�ļ���ȡ����
        private ArrayList GetXMLHeader(string xmlPath, string dateType)
        {
            ImportXMLHelper importHelper = new ImportXMLHelper(xmlPath, dateType);
            ArrayList lineValues = new ArrayList();
            lineValues = importHelper.GetColumnNameByLanguage(this.languageComponent1);
            if (lineValues != null)
            {
                return lineValues;
            }
            else
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$ImportMOData.xml_NOT_Exists");
                return null;
            }
        }

        //�жϵ�ǰ�����ļ������͵�ǰ��������ƥ��
        protected void CheckTemplateRight(DataTable dt, ArrayList columnList)
        {
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                try
                {
                    for (int i = 0; i < columnList.Count; i++)
                    {
                        if (!dt.Columns.Contains(columnList[i].ToString()))
                        {
                            ExceptionManager.Raise(this.GetType().BaseType, "$Error_Template_Is_Not_Right");
                        }
                    }
                }
                catch (Exception e)
                {
                    ExceptionManager.Raise(this.GetType().BaseType, e.Message.ToString());
                }
            }
        }
        #endregion

        //��鹤��Domain�ķǿ��ֶ���Ϣ
        private void CheckMODomain(MO mo)
        {
            if (string.IsNullOrEmpty(mo.MOCode))
            {
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_MOCODE_Is_Null");
            }
            if (string.IsNullOrEmpty(mo.ItemCode))
            {
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_ItemCode_NotExist");
            }
            if (string.IsNullOrEmpty(mo.MOType))
            {
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_MoType_NotExist");
            }
            if (string.IsNullOrEmpty(mo.MOPlanQty.ToString()))
            {
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_MoPlanQty_NotExist");
            }
            if (string.IsNullOrEmpty(mo.MOPlanEndDate.ToString()))
            {
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_MoPlanEndDate_NotExist");
            }
            if (string.IsNullOrEmpty(mo.MOPlanStartDate.ToString()))
            {
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_MoPlanStartDate_NotExist");
            }
            if (string.IsNullOrEmpty(mo.BOMVersion.ToString()))
            {
                mo.EAttribute6 += ";" + this.languageComponent1.GetString("$Error_BOM_NotExist");
            }
        }

    }

    public class MoDictionary
    {
        public static string Factory
        {
            get { return "FACTORY"; }
        }

        public static string Mocode
        {
            get { return "MOCODE"; }
        }

        public static string Itemcode
        {
            get { return "ITEMCODE"; }
        }

        public static string MoType
        {
            get { return "MOTYPE"; }
        }

        public static string MoPlanQty
        {
            get { return "MOPLANQTY"; }
        }

        public static string PlanSDate
        {
            get { return "STARTDATE"; }
        }

        public static string PlanEDate
        {
            get { return "ENDDATE"; }
        }

        public static string CustomerCode
        {
            get { return "CUSTOMERCODE"; }
        }

        public static string CustomerOrder
        {
            get { return "CUSTOMERORDER"; }
        }

        public static string MoMemo
        {
            get { return "MOMEMO"; }
        }

        public static string MoBOM
        {
            get { return "MOBOM"; }
        }
    }
}
