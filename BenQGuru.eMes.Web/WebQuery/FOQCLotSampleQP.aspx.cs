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

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;
using BenQGuru.eMES.Domain.OQC;
using BenQGuru.eMES.OQC;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.DataCollect;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;


namespace BenQGuru.eMES.Web.WebQuery
{
    /// <summary>
    /// FOQCLotSampleQP ��ժҪ˵����
    /// </summary>
    public partial class FOQCLotSampleQP : BaseQPageNew
    {

        protected ExcelExporter excelExporter = null;
        protected WebQueryHelperNew _helper = null;
        //protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        //private System.ComponentModel.IContainer components;
        protected GridHelperNew _gridHelper = null;
        protected System.Web.UI.WebControls.Label lblSoftwareNameQuery;
        protected System.Web.UI.WebControls.TextBox txtSoftwareNameQuery;
        protected System.Web.UI.WebControls.Label lblSoftwareVersionQuery;
        protected System.Web.UI.WebControls.Label lblStepSequenceConditionQuery;
        protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtConditionStepSequence;
        protected System.Web.UI.WebControls.TextBox txtSoftwareVersionQuery;

        //protected BenQGuru.eMES.Web.UserControl.eMESTime txtOQCBeginTime;
        //protected BenQGuru.eMES.Web.UserControl.eMESTime txtOQCEndTime;

        private QueryOQCFunctionFacade _facade = null;

        protected GridHelper gridSNHelper;
        protected WebQueryHelper _gridSNHelper = null;

        protected GridHelper gridFTHelper;
        protected WebQueryHelper _gridFTHelper;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
            this._gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);

            this._helper = new WebQueryHelperNew(this.cmdQuery, this.cmdGridExport, this.gridWebGrid, this.pagerSizeSelector, this.pagerToolBar, this.languageComponent1, this.DtSource);
            this._helper.LoadGridDataSource += new EventHandler(_helper_LoadGridDataSource);
            this._helper.DomainObjectToGridRow += new EventHandler(_helper_DomainObjectToGridRow);
            this._helper.DomainObjectToExportRow += new EventHandler(_helper_DomainObjectToExportRow);
            this._helper.GetExportHeadText += new EventHandler(_helper_GetExportHeadText);
            //this._helper.GridCellClick += new EventHandler(_helper_GridCellClick);

            FormatHelper.SetSNRangeValue(txtStartSnQuery, txtEndSnQuery);

            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this._initialWebGrid();

                this.drpCrewCodeQuery_Load();

                bool loaddata = false;
                txtOQCBeginDate.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(DateTime.Today));
                txtOQCEndDate.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(DateTime.Today));

                //this.txtOQCBeginTime.Text = FormatHelper.ToTimeString(0);
                //this.txtOQCEndTime.Text = FormatHelper.ToTimeString(235959);

                if (Page.Request["reworkrcard"] != null && Page.Request["reworkrcard"] != string.Empty)
                {
                    this.txtStartSnQuery.Text = Page.Request["reworkrcard"];
                    this.txtEndSnQuery.Text = this.txtStartSnQuery.Text;

                    loaddata = true;
                }

                if (Page.Request["reworklotno"] != null && Page.Request["reworklotno"] != string.Empty)
                {
                    this.txtOQCLotQuery.Text = Page.Request["reworklotno"];

                    loaddata = true;
                }

                if (loaddata)
                {
                    DateTime begin = new DateTime(DateTime.Now.Year - 2, DateTime.Now.Month, DateTime.Now.Day);
                    this.txtOQCBeginDate.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(begin));
                    this._helper.Query(null);
                }
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

        #region FQCSample
        private void _initialWebGrid()
        {
            base.InitWebGrid();
            //��Ʒ,����,����,��Ʒ���к�,�������,�����,������Ŀ,��������
            this._gridHelper.AddColumn("RCARD", "��Ʒ���к�", null);
            this._gridHelper.AddColumn("LOTNO", "����", null);
            this._gridHelper.AddColumn("CHECKSEQ", "����˳���", null);
            this._gridHelper.AddColumn("OQCResult", "�����", null);
            this._gridHelper.AddColumn("ITEMCODE", "��Ʒ", null);
            this._gridHelper.AddColumn("MOCODE", "����", null);
            this._gridHelper.AddColumn("BigLine", "����", null);
            this._gridHelper.AddColumn("MaterialModelCode", "��������", null);
            this._gridHelper.AddColumn("ReworkCode", "�������󵥺�", null);
            this._gridHelper.AddColumn("OQCLotType", "����", null);
            this._gridHelper.AddColumn("ProductType", "��������", null);
            this._gridHelper.AddColumn("CrewCode", "����", null);
            this._gridHelper.AddColumn("OQCMUSER", "�����", null);
            this._gridHelper.AddColumn("MaintainDate1", "�������", null);
            this._gridHelper.AddColumn("MaintainTime1", "���ʱ��", null);
            this._gridHelper.AddColumn("LOTNOSEQ", "�ͼ���˳��", null);
            this._gridHelper.AddColumn("RCARDSEQ", "���˳��", null);
            this._gridHelper.AddLinkColumn("CHECKITEMLIST", "������Ŀ", null);
            this._gridHelper.AddLinkColumn("ERRORCODE", "��������", null);


            //this._gridHelper.AddLinkColumn("TESTDATA","��������",null);

            //������
            this._gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridWebGrid.Columns.FromKey("LOTNOSEQ").Hidden = true;
            this.gridWebGrid.Columns.FromKey("RCARDSEQ").Hidden = true;
            this.gridWebGrid.Columns.FromKey("CHECKSEQ").Hidden = true;

        }

        private void _helper_LoadGridDataSource(object sender, EventArgs e)
        {
            PageCheckManager manager = new PageCheckManager();

            manager.Add(new DateRangeCheck(this.lblGenerateLotdate, this.txtOQCBeginDate.Text, this.txtOQCEndDate.Text, false));


            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return;
            }

            if ((sender is System.Web.UI.HtmlControls.HtmlInputButton) && ((System.Web.UI.HtmlControls.HtmlInputControl)(((System.Web.UI.HtmlControls.HtmlInputButton)(sender)))).Name == "cmdGridExport")
            {
                //TODO ForSimone
                this.ExportQueryEvent(sender, e);
            }
            else
            {
                this.QueryEvent(sender, e);
            }
        }

        #region ��ѯ�¼�

        private void QueryEvent(object sender, EventArgs e)
        {
            //�����к�ת��ΪSourceCode
            DataCollectFacade dataCollectfacade = new DataCollectFacade(this.DataProvider);
            //�������кŵ��������Ҫ����һ�´���
            string startRCard = FormatHelper.CleanString(this.txtStartSnQuery.Text.Trim().ToUpper());
            string endRCard = FormatHelper.CleanString(this.txtEndSnQuery.Text.Trim().ToUpper());
            //ת����SourceCard
            string startSourceCard = dataCollectfacade.GetSourceCard(startRCard, string.Empty);
            string endSourceCard = dataCollectfacade.GetSourceCard(endRCard, string.Empty);
            //end

            int OQCBeginDate = FormatHelper.TODateInt(this.txtOQCBeginDate.Text);
            int OQCEndDate = FormatHelper.TODateInt(this.txtOQCEndDate.Text);

            //int OQCBeginTime = FormatHelper.TOTimeInt(this.txtOQCBeginTime.Text);
            //int OQCEndTime = FormatHelper.TOTimeInt(this.txtOQCEndTime.Text);

            BenQGuru.eMES.OQC.OQCFacade oqcfacade = new BenQGuru.eMES.OQC.OQCFacade(base.DataProvider);
            (e as WebQueryEventArgsNew).GridDataSource =
                oqcfacade.QueryOQCLot2CardCheck(
                FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
                FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
                FormatHelper.CleanString(this.txtOQCLotQuery.Text),
                FormatHelper.CleanString(startSourceCard),
                FormatHelper.CleanString(endSourceCard),
                FormatHelper.CleanString(this.txtSSCode.Text),

                FormatHelper.CleanString(this.txtBigSSCodeWhere.Text),
                FormatHelper.CleanString(this.txtMaterialModelCodeWhere.Text),
                FormatHelper.CleanString(this.txtOQCLotTypeQuery.Text),
                FormatHelper.CleanString(this.txtReWorkMOQuery.Text),
                FormatHelper.CleanString(this.txtProductionTypeQuery.Text),
                FormatHelper.CleanString(this.drpCrewCodeQuery.SelectedValue),
                FormatHelper.CleanString(this.txtOQCMUserRQuery.Text),

                OQCBeginDate, 0,
                OQCEndDate, 235959,
                (e as WebQueryEventArgsNew).StartRow,
                (e as WebQueryEventArgsNew).EndRow);

            (e as WebQueryEventArgsNew).RowCount =
                oqcfacade.QueryOQCLot2CardCheckCount(
                FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
                FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
                FormatHelper.CleanString(this.txtOQCLotQuery.Text),
                FormatHelper.CleanString(startSourceCard),
                FormatHelper.CleanString(endSourceCard),
                FormatHelper.CleanString(this.txtSSCode.Text),

                FormatHelper.CleanString(this.txtBigSSCodeWhere.Text),
                FormatHelper.CleanString(this.txtMaterialModelCodeWhere.Text),
                FormatHelper.CleanString(this.txtOQCLotTypeQuery.Text),
                FormatHelper.CleanString(this.txtReWorkMOQuery.Text),
                FormatHelper.CleanString(this.txtProductionTypeQuery.Text),
                FormatHelper.CleanString(this.drpCrewCodeQuery.SelectedValue),
                FormatHelper.CleanString(this.txtOQCMUserRQuery.Text),

                OQCBeginDate, 0,
                OQCEndDate, 235959);
        }

        //�����¼�
        private void ExportQueryEvent(object sender, EventArgs e)
        {
            this.QueryEvent(sender, e);
        }

        #endregion


        private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToGridRowEventArgsNew).DomainObject != null)
            {
                OQCLot2CardCheckQuery obj = (e as DomainObjectToGridRowEventArgsNew).DomainObject as OQCLot2CardCheckQuery;

                DataRow row = this.DtSource.NewRow();
                row["RCARD"] = obj.RunningCard;
                row["LOTNO"] = obj.LOTNO;
                row["CHECKSEQ"] = obj.CheckSequence;
                row["OQCResult"] = obj.Status;
                row["ITEMCODE"] = obj.ItemCode;
                row["MOCODE"] = obj.MOCode;
                row["BigLine"] = obj.BigStepSequenceCode;
                row["MaterialModelCode"] = obj.MmodelCode;
                row["ReworkCode"] = obj.ReworkCode;
                row["OQCLotType"] = this.languageComponent1.GetString(obj.OQCLotType.ToString());
                row["ProductType"] = this.languageComponent1.GetString(obj.ProductionType.ToString());
                row["CrewCode"] = obj.CrewCode;
                row["OQCMUSER"] = obj.OQCUser;
                row["MaintainDate1"] = FormatHelper.ToDateString(obj.MaintainDate);
                row["MaintainTime1"] = FormatHelper.ToTimeString(obj.MaintainTime);
                row["LOTNOSEQ"] = obj.LotSequence;
                row["RCARDSEQ"] = obj.RunningCardSequence;



                (e as DomainObjectToGridRowEventArgsNew).GridRow = row;
                    //new UltraGridRow(new object[]{
                    //                                  obj.RunningCard,
                    //                                  obj.LOTNO,
                    //                                  obj.CheckSequence,
                    //                                  obj.Status,
                    //                                  obj.ItemCode,
                    //                                  obj.MOCode,

                    //                                  obj.BigStepSequenceCode,
                    //                                 obj.MmodelCode,
                    //                                 obj.ReworkCode,
                    //                                 this.languageComponent1.GetString(obj.OQCLotType.ToString()),
                    //                                 this.languageComponent1.GetString(obj.ProductionType.ToString()),
                    //                                 obj.CrewCode,
                    //                                 obj.OQCUser,

                    //                                  FormatHelper.ToDateString(obj.MaintainDate),
                    //                                  FormatHelper.ToTimeString(obj.MaintainTime),
                    //                                  obj.LotSequence,
                    //                                  obj.RunningCardSequence,
                    //                                  "",
                    //                                  ""
                    //                              }
                    //);
            }
        }

        private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
        {

            if ((e as DomainObjectToExportRowEventArgsNew).DomainObject != null)
            {
                OQCLot2CardCheckQuery obj = (OQCLot2CardCheckQuery)((DomainObjectToExportRowEventArgsNew)e).DomainObject;
                (e as DomainObjectToExportRowEventArgsNew).ExportRow =
                    new string[]{
									obj.RunningCard,
									obj.LOTNO,
									obj.Status,
									obj.ItemCode,
									obj.MOCode,
                                    obj.BigStepSequenceCode,
                                     obj.MmodelCode,
                                     obj.ReworkCode,
                                     this.languageComponent1.GetString(obj.OQCLotType.ToString()),
                                     this.languageComponent1.GetString(obj.ProductionType.ToString()),
                                     obj.CrewCode,
                                     obj.OQCUser,
									FormatHelper.ToDateString(obj.MaintainDate),
									FormatHelper.ToTimeString(obj.MaintainTime)
				};
            }
        }

        private void _helper_GetExportHeadText(object sender, EventArgs e)
        {
            (e as ExportHeadEventArgsNew).Heads =
                new string[]{
								"RunningCard",
								"LOTNO",
								"OQCResult",
								"ItemCode",
								"MOCODE",
                                "BigLine",
                                "MaterialModelCode",
                                "ReworkCode",
                                "OQCLotType",
                                "ProductType",
                                "CrewCode",
                                "OQCUser",
                               
								"OQCDATE",
								"OQCTIME"
							};
        }

        protected override void Grid_ClickCell(GridRecord row, string command)
        {
            if (command.ToUpper() == "CHECKITEMLIST".ToUpper())
            {
                decimal rcardseq = Convert.ToDecimal(row.Items.FindItemByKey("RCARDSEQ").Text);
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FOQCCardCheckList.aspx",
                    new string[]{
									"LotNo",
									"LotSeq",
									"ItemCode",
									"MoCode",
									"RunningCard",									
                                    "CHECKSEQ",
                                    "RunningCardSeq",
									"BackUrl"
								},
                    new string[]{
									row.Items.FindItemByKey("LOTNO").Text,
									row.Items.FindItemByKey("LOTNOSEQ").Text,
									row.Items.FindItemByKey("ITEMCODE").Text,
									row.Items.FindItemByKey("MOCODE").Text,
									row.Items.FindItemByKey("RCARD").Text,
                                    row.Items.FindItemByKey("CHECKSEQ").Text,
									rcardseq.ToString(),
									"FOQCLotSampleQP.aspx"
								})
                    );

            }
            else if (command.ToUpper() == "ERRORCODE".ToUpper())
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FOQCSampleNGDetailQP.aspx",
                    new string[]{
									"LotNo",
									"LotSeq",
									"MoCode",
									"RunningCard",
									"RunningCardSeq",
									"BackUrl"
								},
                    new string[]{
									row.Items.FindItemByKey("LOTNO").Text,
									row.Items.FindItemByKey("LOTNOSEQ").Text,
									row.Items.FindItemByKey("MOCODE").Text,
									row.Items.FindItemByKey("RCARD").Text,
									row.Items.FindItemByKey("RCARDSEQ").Text,
									"FOQCLotSampleQP.aspx"
								})
                    );
            }
            //			else if( (e as GridCellClickEventArgs).Cell.Column.Key.ToUpper() == "TESTDATA" )
            //			{
            //				this.Response.Redirect(
            //					this.MakeRedirectUrl(
            //					"FOQCFuncValueQP.aspx",
            //					new string[]{
            //									"LotNo",
            //									"LotSeq",
            //									"RCard",
            //									"RCardSeq",
            //									"BackUrl"
            //								},
            //					new string[]{
            //									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("LOTNO").Text,
            //									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("LOTNOSEQ").Text,
            //									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARD").Text,
            //									(e as GridCellClickEventArgs).Cell.Row.Cells.FromKey("RCARDSEQ").Text,
            //									"FOQCLotSampleQP.aspx"
            //								})
            //					);
            //
            //			}
        }
        #endregion

        private void cmdQuery_ServerClick(object sender, System.EventArgs e)
        {
            //this.gridFTHelper.RequestData();
            //this.gridSNHelper.RequestData();
        }

        private void grdFT_ClickCellButton(object sender, Infragistics.WebUI.UltraWebGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Key.ToUpper() == "TESTDATA")
            {
                string url = this.MakeRedirectUrl(
                    "FOQCFuncValueDataQP.aspx",
                    new string[]{
									"LotNo",
									"LotSeq",
									"RCard",
									"RCardSeq",
									"BackUrl"
								},
                    new string[]{
									e.Cell.Row.Cells.FromKey("LotNo").Text,//this.txtLotNo.Value,
									e.Cell.Row.Cells.FromKey("LotSeq").Text,//this.txtLotSeq.Value,
									e.Cell.Row.Cells.FromKey("RCard").Text,
									e.Cell.Row.Cells.FromKey("RCardSeq").Text,
									"FOQCLotSampleQP.aspx"
								});

                this.Response.Redirect(url, true);

            }
        }

        private void gridSN_ClickCellButton(object sender, Infragistics.WebUI.UltraWebGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Key.ToUpper() == "TESTDATA")
            {
                string url = this.MakeRedirectUrl(
                    "FOQCFuncValueDataQP.aspx",
                    new string[]{
									"LotNo",
									"LotSeq",
									"RCard",
									"RCardSeq",
									"BackUrl"
								},
                    new string[]{
									e.Cell.Row.Cells.FromKey("LotNo").Text,//this.txtLotNo.Value,
									e.Cell.Row.Cells.FromKey("LotSeq").Text,//this.txtLotSeq.Value,
									e.Cell.Row.Cells.FromKey("RCard").Text,
									e.Cell.Row.Cells.FromKey("RCardSeq").Text,
									"FOQCLotSampleQP.aspx"
								});

                this.Response.Redirect(url, true);

            }
        }

        private void drpCrewCodeQuery_Load()
        {
            if (!IsPostBack)
            {
                ShiftModel shiftModel = new ShiftModel(this.DataProvider);

                DropDownListBuilder builder = new DropDownListBuilder(this.drpCrewCodeQuery);
                builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate(shiftModel.GetAllShiftCrew);
                builder.Build("CrewCode", "CrewCode");
                this.drpCrewCodeQuery.Items.Insert(0, new ListItem("", ""));
            }
        }


    }
}
