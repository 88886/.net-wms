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
using Infragistics.WebUI.UltraWebGrid;
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
#endregion

namespace BenQGuru.eMES.Web.MOModel
{
    /// <summary>
    /// FMOBOMCompare ��ժҪ˵����
    /// </summary>
    public partial class FMOBOMCompare : BaseMPageMinus
    {
        protected BenQGuru.eMES.Web.Helper.PagerSizeSelector pagerSizeSelector;
        private System.ComponentModel.IContainer components;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        //private GridHelper gridHelper = null;
        //private GridHelper gridInMORouteBomHelper = null;
        //private GridHelper gridInMOStandardBomHelper = null;

        private ButtonHelper buttonHelper = null;
        private ExcelExporter excelExporter = null;
        private BenQGuru.eMES.Web.Helper.ExcelExporter excelExporter1;
        private BenQGuru.eMES.Web.Helper.ExcelExporter excelExporter2;
        private BenQGuru.eMES.MOModel.MOFacade facade;

        private Hashtable CompareHT;	//��ȡ�ȶԽ��

        protected void Page_Load(object sender, System.EventArgs e)
        {
            InitHanders();
            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                // ��ʼ������UI
                this.InitUI();
                this.InitParameters();		//���ղ�������
                this.InitWebGrid();
                this.GetCompareDate();		//��ȡ�ȶԽ��
                this.RequestData();			//��������
            }
        }

        //��ȡ�ȶԽ��
        private void GetCompareDate()
        {
            Hashtable _compareHT = new Hashtable();
            //TODO �˴����ò�ѯ����
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateMOFacade(); }

            //��ȡ������עBOM
            object[] MOStandardBoms = facade.GetMOBOM(this.MOCode);

            #region ���� �ȶ�


            //
            //			MOBOM StandardmoBom0 = new MOBOM();
            //			StandardmoBom0.MOCode = "FFF-050718-05";
            //			StandardmoBom0.ItemCode = "GSM7503RD18";
            //			StandardmoBom0.MOBOMItemCode = "FKV8.074.967WE01";
            //			StandardmoBom0.MOBOMItemName = "�ӽ���0";
            //			StandardmoBom0.MOBOMItemQty = 1;
            //			StandardmoBom0.MOBOMItemUOM = "EA";
            //
            //			MOBOM StandardmoBom1 = new MOBOM();
            //			StandardmoBom1.MOCode = "FFF-050718-05";
            //			StandardmoBom1.ItemCode = "GSM7503RD18";
            //			StandardmoBom1.MOBOMItemCode = "FKV8.074.967RD01";
            //			StandardmoBom1.MOBOMItemName = "�ӽ���1";
            //			StandardmoBom1.MOBOMItemQty = 1;
            //			StandardmoBom1.MOBOMItemUOM = "EA";
            //
            //			MOBOM StandardmoBom2 = new MOBOM();
            //			StandardmoBom2.MOCode = "FFF-050718-05";
            //			StandardmoBom2.ItemCode = "GSM7503RD18";;
            //			StandardmoBom2.MOBOMItemCode = "FKV8.074.968WE01";
            //			StandardmoBom2.MOBOMItemName = "�ӽ���2";
            //			StandardmoBom2.MOBOMItemQty = 1;
            //			StandardmoBom2.MOBOMItemUOM = "EA";
            //
            //			//��������
            //			MOBOM StandardmoBom3 = new MOBOM();
            //			StandardmoBom3.MOCode = "FFF-050718-05";
            //			StandardmoBom3.ItemCode = "GSM7503RD18";;
            //			StandardmoBom3.MOBOMItemCode = "FKV8.074.966RD01";
            //			StandardmoBom3.MOBOMItemName = "�ӽ���3";
            //			StandardmoBom3.MOBOMItemQty = 5;
            //			StandardmoBom3.MOBOMItemUOM = "EA";
            //
            //			//ֻ������MOBOM
            //			MOBOM StandardmoBom4 = new MOBOM();
            //			StandardmoBom4.MOCode = "FFF-050718-05";
            //			StandardmoBom4.ItemCode = "GSM7503RD18";;
            //			StandardmoBom4.MOBOMItemCode = "FKV8.074.968WW01";
            //			StandardmoBom4.MOBOMItemName = "�ӽ���w";
            //			StandardmoBom4.MOBOMItemQty = 1;
            //			StandardmoBom4.MOBOMItemUOM = "EA";
            //
            //			//��ȡ������עBOM
            //			object[] MOStandardBoms = new object[]{StandardmoBom0,StandardmoBom1,StandardmoBom2,StandardmoBom3,StandardmoBom4};

            #endregion

            //��ȡ�ȶԽ��
            _compareHT = facade.CompareMOBOM(MOStandardBoms, this.ItemCode, this.MOCode, this.RouteCode);

            //Test : ��������	��ʾ	
            #region  ��������	��ʾ

            //			ArrayList SucessResult = new ArrayList();
            //			MOBOM moBom = new MOBOM();
            //			moBom.MOCode = "����123";
            //			moBom.ItemCode = "�ֻ�456";
            //			moBom.MOBOMItemCode = "YJ001";
            //			moBom.MOBOMItemName = "Һ�����001";
            //			moBom.MOBOMItemQty = 24;
            //			moBom.MOBOMItemUOM = "��";
            //			moBom.MOBOMException = "�ȶԳɹ�";
            //			SucessResult.Add(moBom);
            //
            //			ArrayList InMOStandardBOMResult = new ArrayList();
            //			MOBOM StandardmoBom = new MOBOM();
            //			StandardmoBom.MOCode = "����123";
            //			StandardmoBom.ItemCode = "�ֻ�456";
            //			StandardmoBom.MOBOMItemCode = "YJ0099";
            //			StandardmoBom.MOBOMItemName = "Һ�����99";
            //			StandardmoBom.MOBOMItemQty = 29;
            //			StandardmoBom.MOBOMItemUOM = "��";
            //			StandardmoBom.MOBOMException = "�ȶ�ʧ�� ֻ�����ڱ�׼BOM";
            //			InMOStandardBOMResult.Add(StandardmoBom);
            //
            //			ArrayList InMORouteResult = new ArrayList();
            //			OPBOMDetail opBOM = new OPBOMDetail();
            //			opBOM.ItemCode = "�ֻ�456";
            //			opBOM.OPBOMItemCode = "YJ100";
            //			opBOM.OPBOMItemName = "Һ�����100";
            //			opBOM.OPBOMItemQty = 65;
            //			opBOM.OPBOMItemUOM = "��";
            //			opBOM.OPBOMSourceItemCode = "YJ1001";
            //			InMORouteResult.Add(opBOM);
            //
            //
            //			_compareHT["SucessResult"] = SucessResult;
            //			_compareHT["InMOStandardBOMResult"] = InMOStandardBOMResult;
            //			_compareHT["InMORouteResult"] = InMORouteResult;

            #endregion

            this.CompareHT = _compareHT;
            Session["CompareHT"] = this.CompareHT;
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
            if (Request.Params["mocode"] == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_RequestUrlParameter_Lost");
            }
            else
            {
                this.ViewState["mocode"] = Request.Params["mocode"].ToString();
            }
            if (Request.Params["routecode"] == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_RequestUrlParameter_Lost");
            }
            else
            {
                this.ViewState["routecode"] = Request.Params["routecode"].ToString();
            }
        }

        public string MOCode
        {
            get
            {
                return (string)this.ViewState["mocode"];
            }
        }

        public string ItemCode
        {
            get
            {
                return (string)this.ViewState["itemcode"];
            }
        }

        public string RouteCode
        {
            get
            {
                return (string)this.ViewState["routecode"];
            }
        }


        private void InitHanders()
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, DtSource);
            this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadDataSource);
            this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridRow);

            this.gridHelper2 = new GridHelperNew(this.gridWebGrid2, DtSource2);
            this.gridHelper2.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadMORouteBOMDataSource);
            this.gridHelper2.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridMORouteBOMRow);

            this.gridHelper3 = new GridHelperNew(this.gridWebGrid3, DtSource3);
            this.gridHelper3.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadMOStandardBOMDataSource);
            this.gridHelper3.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridStandardBOMRow);

            this.buttonHelper = new ButtonHelper(this);


            //����ֻ�����ڹ���BOM MOBOM
            this.excelExporter.LoadExportDataNoPageHandle = new LoadExportDataDelegateNoPage(this.GetInStandardBOMobj);
            this.excelExporter.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatMOBOMExportRecord);
            this.excelExporter.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetMOBOMColumnHeaderText);

            //�����ȶԳɹ�
            this.excelExporter1.LoadExportDataNoPageHandle = new LoadExportDataDelegateNoPage(this.GetSucessObj);
            this.excelExporter1.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatExportRecord);
            this.excelExporter1.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetColumnHeaderText);

            //����ֻ�����ڲ�Ʒ����BOM OPBOM
            this.excelExporter2.LoadExportDataNoPageHandle = new LoadExportDataDelegateNoPage(this.LoadMORouteBOMDataSource);
            this.excelExporter2.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatOPBOMExportRecord);
            this.excelExporter2.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetOPBOMColumnHeaderText);

        }

        #region GetGridRow
        //�ȶԳɹ�
        protected DataRow GetGridRow(object obj)
        {
            DataRow row = DtSource.NewRow();
            row["MOCode"] = ((MOBOM)obj).MOCode.ToString();
            row["ItemCode"] = ((MOBOM)obj).ItemCode.ToString();
            row["MOBOMItemCode"] = ((MOBOM)obj).MOBOMItemCode.ToString();
            row["MOBOMItemName"] = ((MOBOM)obj).MOBOMItemName.ToString();
            row["MOBOMItemQty1"] = ((MOBOM)obj).MOBOMItemQty.ToString();
            row["OPBOMItemQty1"] = ((MOBOM)obj).OPBOMItemQty.ToString();
            row["MOBOMItemUOM"] = ((MOBOM)obj).MOBOMItemUOM.ToString();
            row["MOBOMException"] = ((MOBOM)obj).MOBOMException.ToString();
            return row;
        }


        //ֻ�����ڲ�Ʒ����BOM
        protected DataRow GetGridMORouteBOMRow(object obj)
        {
            DataRow row = DtSource2.NewRow();
            row["MOCode"] = this.MOCode;
            row["ItemCode"] = ((OPBOMDetail)obj).ItemCode.ToString();
            row["MOBOMItemCode"] = ((OPBOMDetail)obj).OPBOMItemCode.ToString();
            row["MOBOMItemName"] = ((OPBOMDetail)obj).OPBOMItemName.ToString();
            row["MOBOMItemQty"] = ((OPBOMDetail)obj).OPBOMItemQty.ToString();
            row["MOBOMItemUOM"] = ((OPBOMDetail)obj).OPBOMItemUOM.ToString();
            row["MOBOMException"] = "ֻ�����ڲ�Ʒ����BOM";
            return row;
        }

        //ֻ�����ڹ�����׼BOM
        protected DataRow GetGridStandardBOMRow(object obj)
        {
            DataRow row = DtSource3.NewRow();
            row["MOCode"] = ((MOBOM)obj).MOCode.ToString();
            row["ItemCode"] = ((MOBOM)obj).ItemCode.ToString();
            row["OPBOMItemCode"] = ((MOBOM)obj).MOBOMItemCode.ToString();
            row["OPBOMItemName"] = ((MOBOM)obj).MOBOMItemName.ToString();
            row["OPBOMItemQty"] = ((MOBOM)obj).MOBOMItemQty.ToString();
            row["OPBOMItemUOM"] = ((MOBOM)obj).MOBOMItemUOM.ToString();
            row["MOBOMException"] = ((MOBOM)obj).MOBOMException.ToString();
            return row;
        }

        #endregion

        protected void InitWebGrid()
        {
            base.InitWebGrid();
            base.InitWebGrid2();
            base.InitWebGrid3();
            //MOBOM ��ʾ���� ����Excel 
            //����������,��Ʒ,�ӽ��Ϻ�,�ӽ�������,��������,������λ


            //�ȶԳɹ�
            this.gridHelper.AddColumn("MOCode", "����", null);
            this.gridHelper.AddColumn("ItemCode", "��Ʒ", null);
            this.gridHelper.AddColumn("MOBOMItemCode", "�ӽ����Ϻ�", null);
            this.gridHelper.AddColumn("MOBOMItemName", "�ӽ�������", null);
            this.gridHelper.AddColumn("MOBOMItemQty1", "�������� (���������嵥)", null);
            this.gridHelper.AddColumn("OPBOMItemQty1", "�������� (��Ʒ����bom)", null);
            this.gridHelper.AddColumn("MOBOMItemUOM", "������λ", null);
            this.gridHelper.AddColumn("MOBOMException", "�쳣����", null);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            //ֻ�����ڹ�����׼BOM
            this.gridHelper2.AddColumn("MOCode", "����", null);
            this.gridHelper2.AddColumn("ItemCode", "��Ʒ", null);
            this.gridHelper2.AddColumn("MOBOMItemCode", "�ӽ����Ϻ�", null);
            this.gridHelper2.AddColumn("MOBOMItemName", "�ӽ�������", null);
            this.gridHelper2.AddColumn("MOBOMItemQty", "��������", null);
            this.gridHelper2.AddColumn("MOBOMItemUOM", "������λ", null);
            this.gridHelper2.AddColumn("MOBOMException", "�쳣����", null);

            //������
            this.gridHelper2.ApplyLanguage(this.languageComponent1);


            //ֻ�����ڲ�Ʒ����BOM (OPBOMDetail)
            this.gridHelper3.AddColumn("MOCode", "����", null);
            this.gridHelper3.AddColumn("ItemCode", "��Ʒ", null);
            this.gridHelper3.AddColumn("OPBOMItemCode", "�ӽ����Ϻ�", null);
            this.gridHelper3.AddColumn("OPBOMItemName", "�ӽ�������", null);
            this.gridHelper3.AddColumn("OPBOMItemQty", "��������", null);
            this.gridHelper3.AddColumn("OPBOMItemUOM", "������λ", null);
            this.gridHelper3.AddColumn("MOBOMException", "�쳣����", null);
            //this.gridInMORouteBomHelper.AddColumn( "OPBOMSourceItemCode", "�����",	null);

            //������
            this.gridHelper3.ApplyLanguage(this.languageComponent1);

        }

        private object[] LoadDataSource(int inclusive, int exclusive)
        {
            //�ȶԳɹ�
            return GetSucessObj();
        }

        private object[] LoadMORouteBOMDataSource(int inclusive, int exclusive)
        {
            //ֻ�����ڲ�Ʒ����BOM
            return GetInMORouteBOMObj();
        }
        private object[] LoadMOStandardBOMDataSource(int inclusive, int exclusive)
        {
            //ֻ�����ڹ�����׼BOM
            return GetInStandardBOMobj();
        }

        private object[] LoadMORouteBOMDataSource()
        {
            return this.LoadMORouteBOMDataSource(1, 20);
        }

        #region LoadSourece
        //�ȶԳɹ�
        private object[] GetSucessObj()
        {
            if (CompareHT == null)
            {
                if (Session["CompareHT"] != null)
                {
                    CompareHT = (Hashtable)Session["CompareHT"];
                }
            }
            if (CompareHT != null)
            {
                ArrayList returnObjList = (ArrayList)CompareHT["SucessResult"];
                if (returnObjList != null && returnObjList.Count > 0)
                {
                    this.lblgridWebGrid.Text = string.Format("�� {0} ��", returnObjList.Count.ToString());
                    return (MOBOM[])returnObjList.ToArray(typeof(MOBOM));
                }
            }
            return new object[] { };
        }

        //ֻ�����ڹ�����׼BOM
        private object[] GetInStandardBOMobj()
        {
            if (CompareHT == null)
            {
                if (Session["CompareHT"] != null)
                {
                    CompareHT = (Hashtable)Session["CompareHT"];
                }
            }
            if (CompareHT != null)
            {
                ArrayList returnObjList = (ArrayList)CompareHT["InMOStandardBOMResult"];
                if (returnObjList != null && returnObjList.Count > 0)
                {
                    this.lblgridInMOStandardBom.Text = string.Format("�� {0} ��", returnObjList.Count.ToString());
                    return (MOBOM[])returnObjList.ToArray(typeof(MOBOM));
                }
            }
            return new object[] { };

        }

        //ֻ�����ڲ�Ʒ����BOM
        private object[] GetInMORouteBOMObj()
        {
            if (CompareHT == null)
            {
                if (Session["CompareHT"] != null)
                {
                    CompareHT = (Hashtable)Session["CompareHT"];
                }
            }
            //TODO: ע��: �˴����صĶ���Ӧ����OPBOMDetail 
            if (CompareHT != null)
            {
                ArrayList returnObjList = (ArrayList)CompareHT["InMORouteResult"];
                if (returnObjList != null && returnObjList.Count > 0)
                {
                    this.lblgridInMORouteBom.Text = string.Format("�� {0} ��", returnObjList.Count.ToString());
                    return (OPBOMDetail[])returnObjList.ToArray(typeof(OPBOMDetail));
                }
            }
            return new object[] { };
        }

        #endregion

        #region GridDefaultCount

        private void SetDefaultCount()
        {
            this.lblgridWebGrid.Text = string.Format("�� {0} ��", "0");
            this.lblgridInMORouteBom.Text = string.Format("�� {0} ��", "0");
            this.lblgridInMOStandardBom.Text = string.Format("�� {0} ��", "0");
        }

        #endregion


        //��ȡ����
        private void RequestData()
        {
            this.SetDefaultCount();								//����gridĬ������
            this.gridHelper.GridBind(1, 20);					//�ȶԳɹ�
            this.gridHelper2.GridBind(1, 20);		//ֻ�����ڲ�Ʒ����BOM
            this.gridHelper3.GridBind(1, 20);		//ֻ�����ڹ�����׼BOM
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
            this.components = new System.ComponentModel.Container();
            this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
            this.excelExporter = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
            this.excelExporter1 = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
            this.excelExporter2 = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
            // 
            // languageComponent1
            // 
            this.languageComponent1.Language = "CHS";
            this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
            this.languageComponent1.RuntimePage = null;
            this.languageComponent1.RuntimeUserControl = null;
            this.languageComponent1.UserControlName = "";
            // 
            // excelExporter
            // 
            this.excelExporter.CellSplit = ",";
            this.excelExporter.FileExtension = "csv";
            this.excelExporter.LanguageComponent = this.languageComponent1;
            this.excelExporter.Page = this;
            this.excelExporter.RowSplit = "\r\n";
            // 
            // excelExporter1
            // 
            this.excelExporter1.CellSplit = ",";
            this.excelExporter1.FileExtension = "csv";
            this.excelExporter1.LanguageComponent = this.languageComponent1;
            this.excelExporter1.Page = this;
            this.excelExporter1.RowSplit = "\r\n";
            // 
            // excelExporter2
            // 
            this.excelExporter2.CellSplit = ",";
            this.excelExporter2.FileExtension = "csv";
            this.excelExporter2.LanguageComponent = this.languageComponent1;
            this.excelExporter2.Page = this;
            this.excelExporter2.RowSplit = "\r\n";

        }
        #endregion

        private void cmdQuery_ServerClick(object sender, System.EventArgs e)
        {
            this.RequestData();
        }


        private object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
        {
            return null;
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            Session.Remove("CompareHT");
            //���� , Ӧ�õ��ÿͻ��˽ű�����
            Response.Redirect(this.MakeRedirectUrl("FMOEP.aspx", new string[] { "ACT", "MOCode" }, new string[] { "EDIT", MOCode }));
        }


        #region Export

        protected void cmdGridExport_ServerClick(object sender, System.EventArgs e)
        {
            this.excelExporter.RowSplit = "\r\n";
            this.excelExporter.Export();
        }


        protected void cmdSucessExport_ServerClick(object sender, System.EventArgs e)
        {
            this.excelExporter1.RowSplit = "\r\n";
            this.excelExporter1.Export();
        }

        protected void cmdInMOBOMExport_ServerClick(object sender, System.EventArgs e)
        {
            this.excelExporter2.RowSplit = "\r\n";
            this.excelExporter2.Export();
        }

        #region �����ȶԳɹ�����

        protected string[] FormatExportRecord(object obj)
        {
            return new string[]{  
								    ((MOBOM)obj).MOCode.ToString(),
								    ((MOBOM)obj).ItemCode.ToString(),
								    ((MOBOM)obj).MOBOMItemCode.ToString(),
									((MOBOM)obj).MOBOMItemName.ToString(),
									((MOBOM)obj).MOBOMItemQty.ToString(),
								    ((MOBOM)obj).OPBOMItemQty.ToString(),
									((MOBOM)obj).MOBOMItemUOM.ToString(),
									((MOBOM)obj).MOBOMException.ToString()
			};

        }


        protected string[] GetColumnHeaderText()
        {
            return new string[] {   
									"MOCode",
									"ItemCode",
									"MOBOMItemCode",
									"MOBOMItemName",
									"MOBOMItemQty",
									"OPBOMItemQty",
									"MOBOMItemUOM",
									"MOBOMException"
								};
        }
        #endregion

        #region ����MOBOM����

        protected string[] FormatMOBOMExportRecord(object obj)
        {
            return new string[]{  
								   ((MOBOM)obj).MOCode.ToString(),
								   ((MOBOM)obj).ItemCode.ToString(),
								   ((MOBOM)obj).MOBOMItemCode.ToString(),
								   ((MOBOM)obj).MOBOMItemName.ToString(),
								   ((MOBOM)obj).MOBOMItemQty.ToString(),
								   ((MOBOM)obj).MOBOMItemUOM.ToString(),
								   ((MOBOM)obj).MOBOMException.ToString()
							   };

        }


        protected string[] GetMOBOMColumnHeaderText()
        {
            return new string[] {   
									"MOCode",
									"ItemCode",
									"MOBOMItemCode",
									"MOBOMItemName",
									"MOBOMItemQty",
									"MOBOMItemUOM",
									"MOBOMException"
								};
        }
        #endregion

        #region ����OPBOMDetail������
        protected string[] FormatOPBOMExportRecord(object obj)
        {
            return new string[]{  
								   this.MOCode,
								   ((OPBOMDetail)obj).ItemCode.ToString(),
								   ((OPBOMDetail)obj).OPBOMItemCode.ToString(),
								   ((OPBOMDetail)obj).OPBOMItemName.ToString(),
								   ((OPBOMDetail)obj).OPBOMItemQty.ToString(),
								   ((OPBOMDetail)obj).OPBOMItemUOM.ToString(),
									"ֻ�����ڲ�Ʒ����BOM"
								   //((OPBOMDetail)obj).OPBOMSourceItemCode.ToString()
			};


        }


        protected string[] GetOPBOMColumnHeaderText()
        {
            return new string[] {   
									"MOCode",
									"ItemCode",
									"OPBOMItemCode",
									"OPBOMItemName",
									"OPBOMItemQty",
									"OPBOMItemUOM",
									"MOBOMException"
									//"OPBOMSourceItemCode"
								};
        }
        #endregion

        #endregion

        #region ˽�з���

        /// <summary>
        /// ִ�пͻ��˵ĺ���
        /// </summary>
        /// <param name="FunctionName">������</param>
        /// <param name="FunctionParam">����</param>
        /// <param name="Page">��ǰҳ�������</param>
        public void ExecuteClientFunction(string FunctionName, string FunctionParam)
        {
            try
            {
                string _msg = string.Empty;
                if (FunctionParam != string.Empty)
                    _msg = string.Format("<script language='JavaScript'>  {0}('{1}');</script>", FunctionName, FunctionParam);
                else
                    _msg = string.Format("<script language='JavaScript'>  {0}();</script>", FunctionName);

                //��Keyֵ��Ϊ�����,��ֹ�ű��ظ�
                Page.RegisterStartupScript(Guid.NewGuid().ToString(), _msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Alert(string msg)
        {
            msg = msg.Replace("'", "");
            msg = msg.Replace("\r", "");
            msg = msg.Replace("\n", "");
            string _msg = string.Format("<script language='JavaScript'>  alert('{0}');</script>", msg);
            Page.RegisterStartupScript("", _msg);
        }

        #endregion

    }
}
