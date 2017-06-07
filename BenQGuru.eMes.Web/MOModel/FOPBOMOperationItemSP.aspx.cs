#region system
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
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
#endregion

namespace BenQGuru.eMES.Web.MOModel
{
    /// <summary>
    /// FOPBOMOperationItemSP ��ժҪ˵����
    /// </summary>
    public partial class FOPBOMOperationItemSP : BaseMPageMinus
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        //private GridHelper gridHelper = null;
        private ButtonHelper buttonHelper = null;
        private OPBOMFacade _opBOMFacade;// = new FacadeFactory(base.DataProvider).CreateOPBOMFacade();
        private SBOMFacade _sbomFacade;// = new FacadeFactory(base.DataProvider).CreateSBOMFacade();
        //private ModelFacade _modelFacade = FacadeFactory.CreateModelFacade();
        private ItemFacade _itemFacade;//= new FacadeFactory(base.DataProvider).CreateItemFacade();



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

        #region page events
        protected void Page_Load(object sender, System.EventArgs e)
        {
            InitHander();
            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                // ��ʼ������UI
                this.InitUI();
                InitParameters();
                InitWebGrid();

                this.TextboxSBOMVersionQuery.ReadOnly = true;

                this.RequestData();

                //this.pagerSizeSelector.Readonly = true;
            }
        }

        private void RequestData()
        {
            // 2005-04-06
            this.pagerToolBar.PageSize = this.pagerSizeSelector.PageSize;
            this.gridHelper.GridBind(PageGridBunding.Page, this.pagerSizeSelector.PageSize);

            this.pagerToolBar.RowCount = GetRowCount();
            this.pagerToolBar.InitPager();
        }


        protected void cmdAdd_ServerClick(object sender, System.EventArgs e)
        {
            if (_opBOMFacade == null) { _opBOMFacade = new FacadeFactory(base.DataProvider).CreateOPBOMFacade(); }
            if (_itemFacade == null) { _itemFacade = new FacadeFactory(base.DataProvider).CreateItemFacade(); }

            ArrayList array = this.GetNoRepeatSelectObjs(this.gridHelper.GetCheckedRows());

            if (array.Count > 0)
            {
                ArrayList sboms = new ArrayList();
                Hashtable notInOPBOMHT = this.GetNotInOPBOMHashtalbe();
                string returnMsg = string.Empty;

                foreach (GridRecord row in array)
                {
                    object obj = this.GetEditObject(row);
                    if (obj != null)
                    {
                        if (notInOPBOMHT.Contains(((SBOM)obj).SBOMItemCode))//ֻ�в���opbom�е��ӽ��ϲſ�����ӵ��б���
                            sboms.Add((SBOM)obj);
                    }
                }
                if (sboms.Count > 0)
                {
                    ItemRoute2OP itemRoute2Operation = (ItemRoute2OP)_itemFacade.GetItemRoute2Op(OPID, GlobalVariables.CurrentOrganizations.First().OrganizationID);
                    _opBOMFacade.AssignBOMItemToOperation(OPBOMCode, OPBOMVersion, itemRoute2Operation, (SBOM[])sboms.ToArray(typeof(SBOM)), this.Actiontype);

                    //Laws Lu,2006/09/01
                    /*1,Ŀǰ����BOM�����߼����䣬������Ч��鹦�ܺ�ʧЧ���ܣ�
                         * ��ʼ�����Ĺ���BOM���ϴ���ʧЧ״̬,ͨ����Ч��������Ч״̬��
                         * ��ʱ�������޸ģ�ֻ��ʧЧ״̬�Ĺ���BOM�ſ����޸ġ�
                         * ��Ч����߼������������Ĺ���BOM�������ӽ����ϣ�����ϣ��������ĳ�������е��ѷ������ϴ��룬
                         * ���磬�������������а���5�����ϣ�����BOM�е��ӽ����ϱ���Ҳ�����������ϣ�
                         * ����ѡ�ϲ���������������֮����������ϡ�����Ĺ������û��ڽ���ָ����
                         * �������£�����������������A,B,C,D��������*/
                    DataProvider.BeginTransaction();
                    try
                    {
                        MOFacade moFac = (new FacadeFactory(DataProvider)).CreateMOFacade();

                        object objOPBOM = moFac.GetOPBOM(itemRoute2Operation.ItemCode, OPBOMCode, OPBOMVersion, GlobalVariables.CurrentOrganizations.First().OrganizationID);

                        if (objOPBOM != null)
                        {
                            OPBOM opBOM = objOPBOM as OPBOM;

                            opBOM.Avialable = 0;

                            moFac.UpdateOPBOM(opBOM);
                        }
                        // Added by Icyer 2005/08/16
                        // ͬʱ�����ϼ��뵽����������
                        BenQGuru.eMES.Material.WarehouseFacade wf = new BenQGuru.eMES.Material.WarehouseFacade(base.DataProvider);
                        wf.AddWarehouseItem((SBOM[])sboms.ToArray(typeof(SBOM)));

                        DataProvider.CommitTransaction();
                        //this.cmdReturn_ServerClick(sender,e);
                        // Added end
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);

                        DataProvider.RollbackTransaction();
                        throw ex;

                    }
                    finally
                    {
                        ((SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
                    }

                    foreach (SBOM somobject in (SBOM[])sboms.ToArray(typeof(SBOM)))
                    {
                        ItemFacade itemFacade = new ItemFacade(this.DataProvider);
                        Domain.MOModel.Material MaterialObject = (Domain.MOModel.Material)itemFacade.GetMaterial(somobject.SBOMItemCode.Trim().ToUpper(), GlobalVariables.CurrentOrganizations.First().OrganizationID);
                        if (MaterialObject != null)
                        {
                            if (MaterialObject.MaterialCheckType.IndexOf(OPBOMDetailCheckType.CHECK_LINKBARCODE) < 0)
                            {
                                returnMsg += "$ITEM_NOT_LINKBARCODE:" + somobject.SBOMItemCode.Trim().ToUpper() + "\n ";
                            }
                        }
                    }
                }

                //if (addItemMessage.Trim()!=string.Empty && addItemMessage.Trim().Length>0)
                //{
                //    WebInfoPublish.Publish(this, addItemMessage, this.languageComponent1);
                //}

                this.RequestData();
                this.Return(sender, e, returnMsg);
            }

        }

        //ɸѡGrid�в��ظ����ӽ���
        private ArrayList GetNoRepeatSelectObjs(ArrayList array)
        {
            Hashtable existHT = new Hashtable();
            ArrayList returnList = new ArrayList();
            if (array != null)
            {
                foreach (GridRecord row in array)
                {
                    string obitemcode = row.Items.FindItemByKey("SBOMItemCode").Value.ToString();
                    if (!existHT.Contains(obitemcode))
                    {
                        existHT.Add(obitemcode, obitemcode);
                        returnList.Add(row);
                    }
                }
            }
            return returnList;
        }

        //��ȡ���ڹ���bom�е��ӽ��Ϻ�
        private Hashtable GetNotInOPBOMHashtalbe()
        {
            Hashtable returnHT = new Hashtable();
            object[] notInOpbomitems = this.GetNotInOPBOMItems();
            if (notInOpbomitems != null && notInOpbomitems.Length > 0)
            {
                foreach (object sbomitem in notInOpbomitems)
                {
                    SBOM tempSbom = (sbomitem as SBOM);
                    if (tempSbom != null && !returnHT.Contains(tempSbom.SBOMItemCode))
                    {
                        returnHT.Add(tempSbom.SBOMItemCode, tempSbom.SBOMItemCode);
                    }
                }
            }
            return returnHT;
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Return(sender, e, string.Empty);
        }

        protected void Return(object sender, System.EventArgs e, string returnMessage)
        {
            if (_itemFacade == null) { _itemFacade = new FacadeFactory(base.DataProvider).CreateItemFacade(); }
            ItemRoute2OP itemRoute2OP = (ItemRoute2OP)_itemFacade.GetItemRoute2Op(OPID, GlobalVariables.CurrentOrganizations.First().OrganizationID);

            string target = this.MakeRedirectUrl("FOPBOMOperationComponetLoadingMP.aspx",
                    new string[] { "opid", "opcode", "itemcode", "opbomcode", "opbomversion", "routecode", "actiontype", "OrgID" },
                    new string[] { OPID, itemRoute2OP.OPCode, Server.UrlEncode(ItemCode), OPBOMCode, OPBOMVersion, Server.UrlEncode(RouteCode), this.Actiontype.ToString(), OrgID.ToString() });


            if (returnMessage.Trim().Length > 0)
            {
                Session["ReturnMessage"] = returnMessage;
            }

            this.Response.Redirect(target);

        }

        #endregion

        #region private method
        private void InitHander()
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
            this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadDataSource);
            this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridRow);

            this.buttonHelper = new ButtonHelper(this);

            this.pagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);
        }

        private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
        {
            this.gridHelper.GridBind(this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize);
        }

        private void InitParameters()
        {
            if (Request.Params["itemcode"] == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_RequestUrlParameter_Lost");
            }
            else
            {
                this.ViewState["itemcode"] = Request.Params["itemcode"].ToString();
            }
            if (Request.Params["opbomcode"] == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_RequestUrlParameter_Lost");
            }
            else
            {
                this.ViewState["opbomcode"] = Request.Params["opbomcode"].ToString();
            }
            if (Request.Params["opbomversion"] == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_RequestUrlParameter_Lost");
            }
            else
            {
                //this.ViewState["opbomversion"] = Request.Params["opbomversion"].ToString();
                this.TextboxSBOMVersionQuery.Text = Request.Params["opbomversion"].ToString();
            }
            if (Request.Params["routecode"] == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_RequestUrlParameter_Lost");
            }
            else
            {
                this.ViewState["routecode"] = Request.Params["routecode"].ToString();
            }
            if (Request.Params["opid"] == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_RequestUrlParameter_Lost");
            }
            else
            {
                this.ViewState["opid"] = Request.Params["opid"].ToString();
            }
            if (Request.Params["actiontype"] == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_RequestUrlParameter_Lost");
            }
            else
            {
                this.ViewState["actiontype"] = Request.Params["actiontype"].ToString();
            }

        }

        /// <summary>
        /// ����0 ����1
        /// </summary>
        public int Actiontype
        {
            get
            {
                try
                {
                    //Ĭ��������
                    if (ViewState["actiontype"] != null)
                    { return Convert.ToInt32((string)ViewState["actiontype"]); }
                    else return 0;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string ItemCode
        {
            get
            {
                return (string)ViewState["itemcode"];
            }
        }

        public string OPBOMCode
        {
            get
            {
                return (string)ViewState["opbomcode"];
            }
        }

        public string OPBOMVersion
        {
            get
            {
                //return (string)ViewState["opbomversion"];
                return TextboxSBOMVersionQuery.Text;
            }
        }
        public string RouteCode
        {
            get
            {
                return (string)ViewState["routecode"];
            }
        }
        public string OPID
        {
            get
            {
                return (string)ViewState["opid"];
            }
        }

        public int OrgID
        {
            get
            {
                if (this.ViewState["OrgID"] == null)
                    return GlobalVariables.CurrentOrganizations.First().OrganizationID;
                else return int.Parse(this.ViewState["OrgID"].ToString());
            }
        }

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("SBOMItemCode", "�ӽ����Ϻ�", null);
            this.gridHelper.AddColumn("SBOMItemName", "�ӽ�������", null);
            this.gridHelper.AddColumn("Sequence", "���", null);
            this.gridHelper.AddColumn("SBOMSourceItemCode", "��ѡ��", null);
            this.gridHelper.AddColumn("SBOMItemQty", "��������", null);
            this.gridHelper.AddColumn("SBOMItemUOM", "������λ", null);
            this.gridHelper.AddColumn("EffectiveDate", "��Ч����", null);
            this.gridHelper.AddColumn("IneffectiveDate", "ʧЧ����", null);
            //this.gridHelper.AddColumn( "ECNNO",		 "ECN����",		null);
            this.gridHelper.AddColumn("ItemControlType", "�ܿ�����", null);
            this.gridHelper.AddColumn("MaterialDesc", "��������", null);

            this.gridWebGrid.Columns.FromKey("Sequence").Hidden = true;

            this.gridHelper.AddDefaultColumn(true, false);
            //this.gridHelper.ApplyDefaultStyle();

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["SBOMItemCode"] = ((SBOM)obj).SBOMItemCode.ToString();
            row["SBOMItemName"] = ((SBOM)obj).SBOMItemName.ToString();
            row["Sequence"] = ((SBOM)obj).Sequence.ToString();
            row["SBOMSourceItemCode"] = ((SBOM)obj).SBOMSourceItemCode.ToString();
            row["SBOMItemQty"] = ((SBOM)obj).SBOMItemQty.ToString();
            row["SBOMItemUOM"] = ((SBOM)obj).SBOMItemUOM.ToString();
            row["EffectiveDate"] = FormatHelper.ToDateString(((SBOM)obj).SBOMItemEffectiveDate);
            row["IneffectiveDate"] = FormatHelper.ToDateString(((SBOM)obj).SBOMItemInvalidDate);
            row["ItemControlType"] = this.languageComponent1.GetString(((SBOM)obj).SBOMItemControlType);
            row["MaterialDesc"] = ((SBOM)obj).EAttribute1;
            return row;

        }


        private object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_sbomFacade == null) { _sbomFacade = new FacadeFactory(base.DataProvider).CreateSBOMFacade(); }
            if (this.cbOPBOMEdit.Checked)
            {
                return this._sbomFacade.GetUnSelectSBOMItems(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(ItemCode)), OPBOMCode, OPBOMVersion, RouteCode, OPID, FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSBOMItemCodeQuery.Text)), this.txtSBOMItemNameQuery.Text, this.txtSBOMSourceItemCodeQuery.Text.ToUpper(),
                    inclusive, exclusive);
            }
            else
            {
                return this._sbomFacade.GetAllSBOMItemsByItem(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(ItemCode)), OPBOMCode, OPBOMVersion, RouteCode, OPID, FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSBOMItemCodeQuery.Text)), this.txtSBOMItemNameQuery.Text, this.txtSBOMSourceItemCodeQuery.Text.ToUpper(),
                    inclusive, exclusive);
            }
        }

        private int GetRowCount()
        {
            if (_sbomFacade == null) { _sbomFacade = new FacadeFactory(base.DataProvider).CreateSBOMFacade(); }
            if (this.cbOPBOMEdit.Checked)
            {
                return this._sbomFacade.GetUnSelectSBOMItemsCounts(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(ItemCode)), OPBOMCode, OPBOMVersion, RouteCode, OPID, FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSBOMItemCodeQuery.Text)), this.txtSBOMItemNameQuery.Text, this.txtSBOMSourceItemCodeQuery.Text.ToUpper());
            }
            else
            {
                return this._sbomFacade.GetAllSBOMItemsByItemCounts(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(ItemCode)), OPBOMCode, OPBOMVersion, RouteCode, OPID, FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSBOMItemCodeQuery.Text)), this.txtSBOMItemNameQuery.Text, this.txtSBOMSourceItemCodeQuery.Text.ToUpper());
            }
        }


        private object[] GetNotInOPBOMItems()
        {
            if (_sbomFacade == null) { _sbomFacade = new FacadeFactory(base.DataProvider).CreateSBOMFacade(); }
            return this._sbomFacade.GetUnSelectSBOMItems(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(ItemCode)),
                OPBOMCode,
                OPBOMVersion,
                RouteCode, OPID,
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSBOMItemCodeQuery.Text)),
                this.txtSBOMItemNameQuery.Text,
                this.txtSBOMSourceItemCodeQuery.Text,
                this.Actiontype,
                0, int.MaxValue);
        }

        private object GetEditObject(GridRecord row)
        {
            if (_sbomFacade == null) { _sbomFacade = new FacadeFactory(base.DataProvider).CreateSBOMFacade(); }

            string itemcode = this.ItemCode;
            string sbitemcode = row.Items.FindItemByKey("SBOMItemCode").Value.ToString();
            string sbsitemcode = row.Items.FindItemByKey("SBOMSourceItemCode").Value.ToString();
            string sbitemqty = row.Items.FindItemByKey("SBOMItemQty").Value.ToString();
            string effdate = FormatHelper.TODateInt(DateTime.Parse(row.Items.FindItemByKey("EffectiveDate").Text).Date).ToString();


            object obj = this._sbomFacade.GetSBOM(itemcode, sbitemcode, sbsitemcode, sbitemqty, effdate, GlobalVariables.CurrentOrganizations.First().OrganizationID, string.Empty);

            if (obj != null)
            {
                return obj;
            }

            return null;
        }





        #endregion

        protected void cmdQuery_ServerClick(object sender, System.EventArgs e)
        {
            RequestData();
            this.buttonHelper.PageActionStatusHandle(PageActionType.Query);
        }
    }
}
