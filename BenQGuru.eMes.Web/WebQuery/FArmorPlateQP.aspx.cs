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
using BenQGuru.eMES.Domain.ArmorPlate;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;
using BenQGuru.eMES.Web.UserControl;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.WebQuery
{
    /// <summary>
    /// FTSInfoQP ��ժҪ˵����
    /// </summary>
    public partial class FArmorPlateQP : BaseQPageNew
    {
        protected BenQGuru.eMES.Web.Helper.OWCChartSpace OWCChartSpace1;
        protected System.Web.UI.HtmlControls.HtmlInputButton Submit2;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        protected ExcelExporter excelExporter = null;
        protected WebQueryHelperNew _helper = null;
        //protected GridHelper gridHelper = null;	

        protected global::System.Web.UI.WebControls.TextBox txtDateFrom;
        protected global::System.Web.UI.WebControls.TextBox txtDateTo;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, DtSource);

            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                txtDateFrom.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(DateTime.Today));
                txtDateTo.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(DateTime.Today));

                this._initialWebGrid();
            }

            this._helper = new WebQueryHelperNew(this.cmdQuery, this.cmdGridExport, this.gridWebGrid, this.pagerSizeSelector, this.pagerToolBar, this.languageComponent1, DtSource);
            this._helper.LoadGridDataSource += new EventHandler(_helper_LoadGridDataSource);
            this._helper.DomainObjectToGridRow += new EventHandler(_helper_DomainObjectToGridRow);
            this._helper.DomainObjectToExportRow += new EventHandler(_helper_DomainObjectToExportRow);
            this._helper.GetExportHeadText += new EventHandler(_helper_GetExportHeadText);
        }

        private void _initialWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("ArmorPlateID", "���ڱ��", 120);
            this.gridHelper.AddColumn("BasePlateCode", "�����Ϻ�", 120);
            this.gridHelper.AddColumn("CurrentVersion", "�汾", 120);
            this.gridHelper.AddColumn("TensionA", "����A", 120);
            this.gridHelper.AddColumn("TensionB", "����B", 120);
            this.gridHelper.AddColumn("TensionC", "����C", 120);
            this.gridHelper.AddColumn("TensionD", "����D", 120);
            this.gridHelper.AddColumn("TensionE", "����E", 120);
            this.gridHelper.AddColumn("UMOCode", "���ù���", 120);
            this.gridHelper.AddColumn("USSCode", "���ò���", 120);
            this.gridHelper.AddColumn("UUser", "������Ա", 120);
            this.gridHelper.AddColumn("UDate", "��������", 120);
            this.gridHelper.AddColumn("UTime", "����ʱ��", 120);
            this.gridHelper.AddColumn("UsedTimes", "ʹ�ô���", 120);
            this.gridHelper.AddColumn("ReUser", "�˻���Ա", 120);
            this.gridHelper.AddColumn("ReDate", "�˻�����", 120);
            this.gridHelper.AddColumn("ReTime", "�˻�ʱ��", 120);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
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
            // 
            // languageComponent1
            // 
            this.languageComponent1.Language = "CHS";
            this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
            this.languageComponent1.RuntimePage = null;
            this.languageComponent1.RuntimeUserControl = null;
            this.languageComponent1.UserControlName = "";

        }
        #endregion

        private bool _checkRequireFields()
        {
            PageCheckManager manager = new PageCheckManager();

            //			manager.Add( new DateRangeCheck(this.chbFrmDate,this.txtDateFrom.Text,lblFrmDateT,txtDateTo.Text,0,7,true));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            return true;
        }


        private void _helper_LoadGridDataSource(object sender, EventArgs e)
        {
            //			this._initialWebGrid();

            string beginDate = String.Empty;
            string endDate = String.Empty;
            if (chbFrmDate.Checked == true)
            {
                beginDate = FormatHelper.CleanString(FormatHelper.TODateInt(this.txtDateFrom.Text).ToString()).ToUpper();
                endDate = FormatHelper.CleanString(FormatHelper.TODateInt(this.txtDateTo.Text).ToString()).ToUpper();

                //if( this._checkRequireFields() )
                //{
                //    return;
                //}
            }

            FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
            object[] dataSource = facadeFactory.CreateArmorPlateFacade().QueryArmorPlateContol(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtAPIDQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtBasePlateQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMOQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtLineQuery.Text)),
                beginDate,
                endDate,
                (e as WebQueryEventArgsNew).StartRow,
                (e as WebQueryEventArgsNew).EndRow);

            (e as WebQueryEventArgsNew).GridDataSource = dataSource;

            (e as WebQueryEventArgsNew).RowCount = facadeFactory.CreateArmorPlateFacade().QueryArmorPlateContolCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtAPIDQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtBasePlateQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMOQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtLineQuery.Text)),
                beginDate,
                endDate);
        }

        private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToGridRowEventArgsNew).DomainObject != null)
            {
                ArmorPlateContol obj = (e as DomainObjectToGridRowEventArgsNew).DomainObject as ArmorPlateContol;
                DataRow row = DtSource.NewRow();
                row["ArmorPlateID"] = obj.ArmorPlateID.ToString();
                row["BasePlateCode"] = obj.BasePlateCode.ToString();
                row["CurrentVersion"] = obj.Version.ToString();
                row["TensionA"] = obj.TensionA.ToString();
                row["TensionB"] = obj.TensionB.ToString();
                row["TensionC"] = obj.TensionC.ToString();
                row["TensionD"] = obj.TensionD.ToString();
                row["TensionE"] = obj.TensionE.ToString();
                row["UMOCode"] = obj.UsedMOCode.ToString();
                row["USSCode"] = obj.UsedSSCode.ToString();
                row["UUser"] = obj.UsedUser.ToString();
                row["UDate"] = FormatHelper.ToDateString(obj.UsedDate);
                row["UTime"] = FormatHelper.ToTimeString(obj.UsedTime);
                row["UsedTimes"] = obj.UsedTimesInMO.ToString("##.##");
                row["ReUser"] = obj.ReturnUser.ToString();
                row["ReDate"] = FormatHelper.ToDateString(obj.ReturnDate);
                row["ReTime"] = FormatHelper.ToTimeString(obj.ReturnTime);
                (e as DomainObjectToGridRowEventArgsNew).GridRow = row;

            }
        }

        private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToExportRowEventArgsNew).DomainObject != null)
            {
                ArmorPlateContol obj = (e as DomainObjectToExportRowEventArgsNew).DomainObject as ArmorPlateContol;

                (e as DomainObjectToExportRowEventArgsNew).ExportRow =
                    new string[]{
									obj.ArmorPlateID.ToString(),
									obj.BasePlateCode.ToString(),
									obj.Version.ToString(),

									obj.TensionA.ToString(),
									obj.TensionB.ToString(),
									obj.TensionC.ToString(),
									obj.TensionD.ToString(),
									obj.TensionE.ToString(),

									obj.UsedMOCode.ToString(),
									obj.UsedSSCode.ToString(),
									obj.UsedUser.ToString(),													   
									FormatHelper.ToDateString( obj.UsedDate ),
									FormatHelper.ToTimeString( obj.UsedTime ),
									obj.UsedTimesInMO.ToString("##.##"),

									obj.ReturnUser.ToString(),
									FormatHelper.ToDateString( obj.ReturnDate ),
									FormatHelper.ToTimeString( obj.ReturnTime )
								};
            }
        }

        private void _helper_GetExportHeadText(object sender, EventArgs e)
        {
            (e as ExportHeadEventArgsNew).Heads =
                new string[]{
								"ArmorPlateID",
								"BasePlateCode",
								"CurrentVersion",

								"TensionA",
								"TensionB",	
								"TensionC",	
								"TensionD",	
								"TensionE",	
		
								"UMOCode",						
								"USSCode",	
								"UUser",	
								"UDate",					
								"UTime",						
								"UsedTimes",	
					
								"ReUser",
								"ReDate",					
								"ReTime"
							};
        }

    }
}
