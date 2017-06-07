using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Rework;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.Rework;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
using BenQGuru.eMES.Material;

namespace BenQGuru.eMES.Web.BaseSetting
{
    /// <summary>
    /// FDCTCommandMP ��ժҪ˵����
    /// </summary>
    public partial class FMaterialMP : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        protected System.Web.UI.WebControls.Label lblResourceTitle;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private BenQGuru.eMES.MOModel.ItemFacade _facade = null; //new BaseModelFacadeFactory().Create();
        private WarehouseFacade _WarehouseFacade = null;
        private BenQGuru.eMES.Material.InventoryFacade inventoryFacade = null;

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
        protected void Page_Init(object sender, System.EventArgs e)
        {
            PostBackTrigger tri = new PostBackTrigger();
            tri.ControlID = this.cmdEnter.ID;
            (this.FindControl("up1") as UpdatePanel).Triggers.Add(tri);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.cmdEnter.Attributes.Add("onclick", "return Check();");
            if (!this.IsPostBack)
            {
                #region ����

                string fileType = "MaterialImport";// 
                //if (this.languageComponent1.Language.ToString() == "CHT")
                //{
                //    fileType = fileType + "_CHT";
                //}
                //else if (this.languageComponent1.Language.ToString() == "ENU")
                //{
                //    fileType = fileType + "_ENU";
                //}
                aFileDownLoad.HRef = string.Format(@"{0}download\{1}.xls", this.VirtualHostRoot, fileType);
                
                #endregion
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
                InitDropList();
                //this.gridWebGrid.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(gridWebGrid_InitializeRow);
            }
        }
       
        protected void InitDropList()
        {
            this.drpMaterialSourceQuery.Items.Add(new ListItem( "",""));
            this.drpMaterialSourceQuery.Items.Add(new ListItem("SAP","SAP"));
            this.drpMaterialSourceQuery.Items.Add(new ListItem("MES", "MES"));
            this.drpMaterialSourceQuery.SelectedIndex = 1;

            this.drpMaterialTypeQuery.Items.Add(new ListItem("", ""));
            this.drpMaterialTypeQuery.Items.Add(new ListItem("�����ܿ�", SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS));
            this.drpMaterialTypeQuery.Items.Add(new ListItem("���ιܿ�", SAP_CONTROLTYPE.SAP_ITEM_CONTROL_LOT));
            this.drpMaterialTypeQuery.Items.Add(new ListItem("���ܿ�", SAP_CONTROLTYPE.SAP_ITEM_CONTROL_NOCONTROL));
            this.drpMaterialType.SelectedIndex = 1;

            this.drpMaterialType.Items.Add(new ListItem("", ""));
            this.drpMaterialType.Items.Add(new ListItem("�����ܿ�", SAP_CONTROLTYPE.SAP_ITEM_CONTROL_KEYPARTS));
            this.drpMaterialType.Items.Add(new ListItem("���ιܿ�", SAP_CONTROLTYPE.SAP_ITEM_CONTROL_LOT));
            this.drpMaterialType.Items.Add(new ListItem("���ܿ�", SAP_CONTROLTYPE.SAP_ITEM_CONTROL_NOCONTROL));
            this.drpMaterialType.SelectedIndex = 1;

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
            this.gridHelper.AddColumn("MaterialNo", "���ϱ���", null);
            this.gridHelper.AddColumn("DQMaterialNo", "�������ϱ���", null);

            this.gridHelper.AddColumn("ENSDesc", "Ӣ�Ķ�����", null);
            this.gridHelper.AddColumn("ENLDesc", "Ӣ�ĳ�����", null);
            this.gridHelper.AddColumn("CHSDesc", "���Ķ�����", null);
            this.gridHelper.AddColumn("CHLDesc", "���ĳ�����", null);
            this.gridHelper.AddColumn("SpecialDesc", "������������", null);
            this.gridHelper.AddColumn("MaterialItem", "Ʒ��", null);
            this.gridHelper.AddColumn("MUOM", "��λ", null);
            this.gridHelper.AddColumn("MaterialType", "��������", null);
            this.gridHelper.AddColumn("ROHS1", "������־", null);
            this.gridHelper.AddColumn("MStatus", "����״̬", null);
            this.gridHelper.AddColumn("MControlType", "�ع�����", null);
            this.gridHelper.AddColumn("MValidity", "��Ч��", null);
            this.gridHelper.AddColumn("Msource", "������Դ", null);

            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
            //this.gridWebGrid.Columns.FromKey("SelectResource").Width = 100;

            this.gridHelper.AddDefaultColumn(true, true);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override DataRow GetGridRow(object obj)
        {

            DataRow row = this.DtSource.NewRow();
            BenQGuru.eMES.Domain.MOModel.Material merterail = obj as BenQGuru.eMES.Domain.MOModel.Material;
            row["MaterialNo"] = merterail.MCode;
            row["DQMaterialNo"] = merterail.DqmCode;
            row["ENSDesc"] = merterail.MenshortDesc;
            row["ENLDesc"] =merterail.MenlongDesc;
            row["CHSDesc"] =merterail.MchshortDesc;
            row["CHLDesc"] =merterail.MchlongDesc;

            row["SpecialDesc"] = merterail.MspecialDesc;
            row["MaterialItem"] = merterail.ModelCode;
            row["MUOM"] =merterail.Muom;
            row["MaterialType"] =merterail.MaterialType;

            row["ROHS1"] =merterail.Rohs;
            row["MStatus"] =merterail.Mstate;
            row["MControlType"] = merterail.MCONTROLTYPE;
            row["MValidity"] =merterail.Validity;
            row["Msource"] =merterail.Sourceflag;

            row["MaintainUser"] =merterail.MaintainUser;
            row["MaintainDate"] =FormatHelper.ToDateString(merterail.MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(merterail.MaintainTime);

            return row;
        }

        protected void gridWebGrid_InitializeRow(object sender, RowEventArgs e)
        {
           
            if (e.Row.Items[14].Text.ToString() == "SAP")
            {

            }
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null)
            {
                _facade = new MOModel.ItemFacade(base.DataProvider); 
            }
            return this._facade.GetMaterial(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMaterialNOQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtDQMaterialNOQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMaterialDescQuery.Text)),
                 FormatHelper.CleanString(this.drpMaterialTypeQuery.SelectedValue),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpMaterialSourceQuery.SelectedValue)),
                inclusive, exclusive);
        }


        protected override int GetRowCount()
        {
            if (_facade == null)
            {
                _facade = new MOModel.ItemFacade(base.DataProvider); 
            }
            return this._facade.GetMaterialCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMaterialNOQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtDQMaterialNOQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMaterialDescQuery.Text)),
               FormatHelper.CleanString(this.drpMaterialTypeQuery.SelectedValue),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpMaterialSourceQuery.SelectedValue))
            );
        }

        #endregion

        #region Button

        //protected override void AddDomainObject(object domainObject)
        //{
        //    if (_facade == null)
        //    {
        //        _facade = new MOModel.ItemFacade(base.DataProvider); 
        //    }
        //    this._facade.AddDct((Dct)domainObject);
        //}

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (_facade == null)
            {
                _facade = new MOModel.ItemFacade(base.DataProvider); 
            }
            this._facade.DeleteMaterial((BenQGuru.eMES.Domain.MOModel.Material[])domainObjects.ToArray(typeof(BenQGuru.eMES.Domain.MOModel.Material)));
        }

        protected override void UpdateDomainObject(object domainObject)
        {
            if (_facade == null)
            {
                _facade = new MOModel.ItemFacade(base.DataProvider); 
            }
            this._facade.UpdateMaterial((BenQGuru.eMES.Domain.MOModel.Material)domainObject);
        }


        protected override void buttonHelper_AfterPageStatusChangeHandle(string pageAction)
        {
            if (pageAction == PageActionType.Add)
            {
                this.txtMaterialNO.ReadOnly = false;
            }

            if (pageAction == PageActionType.Update)
            {
                this.txtMaterialNO.ReadOnly = true;
                this.txtMaterialENDesc.ReadOnly = true;
                this.txtMaterialCHDesc.ReadOnly = true;
                this.txtDQMaterialNO.ReadOnly = true;
               
            }
        }
       
        #endregion

        #region Object <--> Page

        protected override object GetEditObject()
        {
            if (_facade == null)
            {
                _facade = new MOModel.ItemFacade(base.DataProvider); 
            }
            BenQGuru.eMES.Domain.MOModel.Material merterial = this._facade.CreateNewMaterial();
            merterial = _facade.GetMaterial(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMaterialNO.Text, 40))) as BenQGuru.eMES.Domain.MOModel.Material;

            merterial.MCONTROLTYPE = FormatHelper.CleanString(this.drpMaterialType.SelectedValue, 40);
            merterial.Validity =Int32.Parse( FormatHelper.CleanString(this.txtMaterialValidity.Text, 100));
            merterial.MaintainUser= this.GetUserCode();

            return merterial;
        }


        protected override object GetEditObject(GridRecord row)
        {
            object objSource = row.Items.FindItemByKey("Msource").Value;
            string strSource = string.Empty;
            if (objSource != null)
            {
                strSource = objSource.ToString();
            }
            if (strSource.ToUpper().Trim() == "SAP")
            {
                //WebInfoPublish.Publish(this, "$Error_SAP_NO_EDITE", this.languageComponent1);
                //return null;

                this.txtMaterialNO.Enabled = false;
                this.txtMaterialENDesc.Enabled = false;
                this.txtMaterialCHDesc.Enabled = false;
                this.txtDQMaterialNO.Enabled = false;
            }
            //if (_WarehouseFacade == null)
            //{
            //    _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            //}
            //if (row.Items.FindItemByKey("MControlType").Value.ToString() == "�����ܿ�")
            //{
            //    object[] objs = this._WarehouseFacade.GetStorageDetailsFromDQMCODE(row.Items.FindItemByKey("DQMaterialNo").Value.ToString());
            //    if (objs != null)
            //    {
            //        this.drpMaterialType.Enabled = false;
            //    }
            //}

            if (_facade == null)
            {
                _facade = new MOModel.ItemFacade(base.DataProvider); 
            }
            string strCode = string.Empty;
            object objCode = row.Items.FindItemByKey("MaterialNo").Value;
            if (objCode != null)
            {
                strCode = objCode.ToString();
            }
            object obj = _facade.GetMaterial(strCode);
            if (obj != null)
            {
                return (BenQGuru.eMES.Domain.MOModel.Material)obj;
            }
            return null;
        }

        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.txtMaterialNO.Text = "";
                this.txtMaterialENDesc.Text = "";
                this.txtMaterialCHDesc.Text = "";
                this.txtDQMaterialNO.Text = "";
                this.drpMaterialType.SelectedIndex = 0;
                this.txtMaterialValidity.Text = "";
                return;
            }
            BenQGuru.eMES.Domain.MOModel.Material merterial=obj as BenQGuru.eMES.Domain.MOModel.Material;
            this.txtMaterialNO.Text = merterial.MCode;
            this.txtMaterialENDesc.Text = merterial.MenshortDesc;
            this.txtMaterialCHDesc.Text = merterial.MchshortDesc;
            this.txtDQMaterialNO.Text = merterial.DqmCode;
            this.drpMaterialType.SelectedIndex = this.drpMaterialType.Items.IndexOf(this.drpMaterialType.Items.FindByValue(merterial.MCONTROLTYPE)); ;
            this.txtMaterialValidity.Text = merterial.Validity.ToString();
           
        }

        protected override bool ValidateInput()
        {

            if (txtMaterialValidity.Text.Trim().Length>10)
            {
                WebInfoPublish.Publish(this, "$Error_VALID_DAY_NOT_MORE_THREE", this.languageComponent1);
                return false;
            }
            PageCheckManager manager = new PageCheckManager();
            //manager.Add(new LengthCheck(this.lblMaterialValidity, this.txtMaterialValidity, 3, true));
            manager.Add(new LengthCheck(this.lblMaterialType, this.drpMaterialType, 100, false));
            manager.Add(new NumberCheck(this.lblMaterialValidity,this.txtMaterialValidity,true));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }


            return true;
        }

        #endregion

        #region Export
        // 2005-04-06

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{  ((BenQGuru.eMES.Domain.MOModel.Material)obj).MCode,
								   ((BenQGuru.eMES.Domain.MOModel.Material)obj).DqmCode,
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MenshortDesc,
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MenlongDesc,
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MchshortDesc,
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MchlongDesc,

                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MspecialDesc,
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).ModelCode,
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).Muom,
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MaterialType,

                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).Rohs,
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).Mstate.ToString(),
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MCONTROLTYPE,
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).Validity.ToString(),
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).Sourceflag,

                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MaintainUser,
								   //((BenQGuru.eMES.Domain.MOModel.Material)obj).GetDisplayText("MaintainUser"),
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MaintainDate.ToString(),
                                   ((BenQGuru.eMES.Domain.MOModel.Material)obj).MaintainTime.ToString()
            };
            

        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"MaterialNo",
									"DQMaterialNo",									
									"ENSDesc",
									"ENLDesc",
                                    "CHSDesc",
                                    "CHLDesc",

                                    "SpecialDesc",
									"MaterialItem",
                                    "MUOM",
                                    "MaterialType",

                                    "ROHS1",
									"MStatus",
                                    "MControlType",
                                    "MValidity",
                                    "Msource",

                                    "MaintainUser",
                                    "MaintainDate",
                                    "MaintainTime"
            };
        }

        #endregion


        #region ���� add by sam 2016��7��13��
        protected void cmdEnter_ServerClick(object sender, System.EventArgs e)
        {
            this.GetAllItem();
            this.gridHelper.RequestData();
        }
        
        #endregion


        private void GetAllItem()
        {

            try
            {

                string fileName = string.Empty;
                 
                #region  new

                #region File
                if (_facade == null)
                {
                    _facade = new MOModel.ItemFacade(base.DataProvider);
                }
                if (inventoryFacade == null)
                {
                    inventoryFacade = new BenQGuru.eMES.Material.InventoryFacade(base.DataProvider);
                }
                string upfileName = "";
                BenQGuru.eMES.Domain.Warehouse.InvDoc doc = new BenQGuru.eMES.Domain.Warehouse.InvDoc();
                if (this.DownLoadPathBom.PostedFile != null)
                {
                    string stno = "MaterialImport";// FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtASNSTNOQuery.Text));
                    HttpPostedFile postedFile = this.DownLoadPathBom.PostedFile;
                    string path = Server.MapPath(this.VirtualHostRoot + "InvDoc/" + "����ά��/");
                    DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(base.DataProvider);
                    //string CARINVNO = row.Items.FindItemByKey("CARINVNO").Text;
                    doc.InvDocNo = stno;
                    doc.InvDocType = "MaterialImport";//FMaterialMP
                    string[] part = postedFile.FileName.Split(new char[] { '/', '\\' });

                    doc.DocType = Path.GetExtension(postedFile.FileName);
                    doc.DocName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                    doc.DocSize = postedFile.ContentLength / 1024;
                    doc.UpUser = GetUserCode();
                    doc.UpfileDate = FormatHelper.TODateInt(DateTime.Now);
                    doc.Dirname = "����ά��";
                    doc.MaintainUser = this.GetUserCode();
                    doc.MaintainDate = dbDateTime.DBDate;
                    doc.MaintainTime = dbDateTime.DBTime;
                    doc.ServerFileName = doc.DocName + "_" + stno + DateTime.Now.ToString("yyyyMMddhhmmss") + doc.DocType;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    fileName = doc.ServerFileName;

                    upfileName = path + fileName;
                    this.DownLoadPathBom.PostedFile.SaveAs(upfileName);
                }
                else
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.PublishInfo(this, "�����ļ�����Ϊ��", this.languageComponent1);
                    return;
                }

                #endregion

                this.UploadedFileName = upfileName;
                #endregion

                DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
                if (this.ViewState["UploadedFileName"] == null)
                {
                    WebInfoPublish.Publish(this, "$Error_UploadFileIsEmpty", this.languageComponent1);
                    //BenQGuru.eMES.Common.ExceptionManager.Raise(this.GetType().BaseType, "$Error_UploadFileIsEmpty");
                }
                fileName = this.ViewState["UploadedFileName"].ToString();
                //��ȡEXCEL��ʽ�ļ�
                System.Data.DataTable dt = GetExcelData(fileName);

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int t = 0; t < dt.Rows.Count; t++)
                    {
                        string dqmcode = dt.Rows[t][0].ToString().Trim(); //���Ϻ�
                        string validity = dt.Rows[t][1].ToString().Trim(); //��Ч��
                        if (validity.Length > 10)
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "$Error_VALID_DAY_NOT_MORE_THREE", this.languageComponent1);
                            return;
                        }
                        if (!IsInt(validity))
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, "�������Ϻ�" + dqmcode + "��Ч�ڱ�������ֵ��", this.languageComponent1);
                            return;
                        }
                        BenQGuru.eMES.Domain.MOModel.Material material =
                            _facade.GetMaterialByDQMCode(dqmcode) as BenQGuru.eMES.Domain.MOModel.Material;
                        if (material != null)
                        {
                            material.Validity = Convert.ToInt32(FormatHelper.CleanString(validity, 100));
                            material.MaintainDate = dbTime.DBDate;
                            material.MaintainTime = dbTime.DBTime;
                            material.MaintainUser = this.GetUserCode();
                            this._facade.UpdateMaterial(material);
                        }
                        else
                        {
                            this.DataProvider.RollbackTransaction();
                            WebInfoPublish.Publish(this, dqmcode + "EXCEL���ж������ϺŲ�����", this.languageComponent1);
                            return;
                        }
                    }

                    this.DataProvider.CommitTransaction();
                    WebInfoPublish.Publish(this, "����ɹ���", this.languageComponent1);
                }
                else
                {
                    this.DataProvider.RollbackTransaction();
                    WebInfoPublish.Publish(this, "�������ݲ���Ϊ��", this.languageComponent1);//add by sam
                }
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.PublishInfo(this, ex.Message, this.languageComponent1);
            }
        }


        private bool IsInt(object obj)
        {
            try
            {
                Int32.Parse(obj.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region ��ȡ�ļ�
        /// <summary> 
        /// ��ȡָ��·����ָ�����������Ƶ�Excel����:ȡ��һ��sheet������ 
        /// </summary> 
        /// <param name="FilePath">�ļ��洢·��</param> 
        /// <param name="WorkSheetName">����������</param> 
        /// <returns>�����ȡ�ҵ������ݻ᷵��һ��������Table�����򷵻��쳣</returns> 
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
            // return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended properties=\"Excel 8.0;Imex=1;HDR=Yes;\"";
            return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"";

            //@"Provider=Microsoft.Jet.OLEDB.4.0;" + 
            //@"Data Source=" + strFilePath + ";" + 
            //@"Extended Properties=" + Convert.ToChar(34).ToString() + 
            //@"Excel 8.0;" + "Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString(); 
        }

        #endregion

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
        

        #region ע��
        //protected override void Grid_ClickCell(GridRecord row, string commandName)
        //{
        //    if (commandName == "SelectResource")
        //    {
        //        this.Response.Redirect(this.MakeRedirectUrl("./FDCTResourceSP.aspx", new string[] { "dctCode" }, new string[] { row.Items.FindItemByKey("DctCode").Value.ToString() }));
        //    }
        //}

        //private object[] GetAllOrg()
        //{
        //    BaseModelFacade facadeBaseModel = new BaseModelFacade(base.DataProvider);
        //    return facadeBaseModel.GetCurrentOrgList();
        //}
    	#endregion
    }
}
