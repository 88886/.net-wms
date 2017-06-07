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
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;
using Infragistics.Web.UI.GridControls;

namespace BenQGuru.eMES.Web.WebQuery
{
    /// <summary>
    /// FTSRecordQP ��ժҪ˵����
    /// </summary>
    public partial class FRMATSRecordQP : BaseQPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        protected ExcelExporter excelExporter = null;
        protected WebQueryHelperNew _helper = null;

        protected BenQGuru.eMES.Web.UserControl.eMESTime txtReceiveBeginTime;
        protected BenQGuru.eMES.Web.UserControl.eMESTime txtReceiveEndTime;

        //protected GridHelper gridHelper = null;	

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.gridHelper = new GridHelperNew(this.gridWebGrid, DtSource);

            this._helper = new WebQueryHelperNew(this.cmdQuery, this.cmdGridExport, this.gridWebGrid, this.pagerSizeSelector, this.pagerToolBar, this.languageComponent1, DtSource);
            this._helper.LoadGridDataSource += new EventHandler(_helper_LoadGridDataSource);
            this._helper.DomainObjectToGridRow += new EventHandler(_helper_DomainObjectToGridRow);
            this._helper.DomainObjectToExportRow += new EventHandler(_helper_DomainObjectToExportRow);
            this._helper.GetExportHeadText += new EventHandler(_helper_GetExportHeadText);
            //this._helper.GridCellClick += new EventHandler(_helper_GridCellClick);

            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                this._initialWebGrid();
                TSStatus tsstatus = new TSStatus();
                tsstatus.Items.Remove(TSStatus.TSStatus_Reflow);		//����������
                tsstatus.Items.Remove(TSStatus.TSStatus_RepeatNG);
                new CheckBoxListBuilder(tsstatus, this.chkTSStateList, this.languageComponent1).Build();

                this.txtReceiveBeginDate.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
                this.txtReceiveEndDate.Text = this.txtReceiveBeginDate.Text;

                this.txtReceiveBeginTime.Text = FormatHelper.ToTimeString(0);
                this.txtReceiveEndTime.Text = FormatHelper.ToTimeString(235959);

            }
            CheckBoxListBuilder.FormatListControlStyle(this.chkTSStateList, 80);

        }

        private void _initialWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("Ts_SN", "���к�", null);
            this.gridHelper.AddColumn("Ts_SNSeq", "���к�˳��", null);
            this.gridHelper.AddColumn("Ts_TsState", "ά��״̬", null);
            this.gridHelper.AddLinkColumn("Ts_Details", "ά����Ϣ", null);
            this.gridHelper.AddLinkColumn("Ts_ChangedItemDetails", "������Ϣ", null);
            this.gridHelper.AddColumn("Ts_ModelCode", "��Ʒ��", null);
            this.gridHelper.AddColumn("Ts_ItemCode", "��Ʒ", null);
            this.gridHelper.AddColumn("Ts_RMABillCode", "RMA����", null);
            this.gridHelper.AddColumn("Ts_RMAReMoCode", "RMA��������", null);
            this.gridHelper.AddColumn("Ts_ConfirmDate", "��������", null);
            this.gridHelper.AddColumn("Ts_ConfirmTime", "����ʱ��", null);
            this.gridHelper.AddColumn("Ts_ConfirmResource", "����վ", null);
            this.gridHelper.AddColumn("Ts_ConfirmUser", "������", null);
            this.gridHelper.AddColumn("Ts_SouceResourceDate", "ά������", null);
            this.gridHelper.AddColumn("Ts_SouceResourceTime", "ά��ʱ��", null);
            this.gridHelper.AddColumn("Ts_RepaireResource", "ά��վ", null);
            this.gridHelper.AddColumn("Ts_TSUser", "ά�޹�", null);
            this.gridHelper.AddColumn("Ts_DestOpCode", "ȥ����", null);

            this.gridHelper.AddColumn("Ts_NGDate", "��������", null);
            this.gridHelper.AddColumn("Ts_NGTime", "����ʱ��", null);

            this.gridWebGrid.Columns.FromKey("Ts_SNSeq").Hidden = true;
            this.gridWebGrid.Columns.FromKey("Ts_NGDate").Hidden = true;
            this.gridWebGrid.Columns.FromKey("Ts_NGTime").Hidden = true;

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

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            return true;
        }

        private void _helper_LoadGridDataSource(object sender, EventArgs e)
        {

            if ((sender is System.Web.UI.HtmlControls.HtmlInputButton) && ((System.Web.UI.HtmlControls.HtmlInputControl)(((System.Web.UI.HtmlControls.HtmlInputButton)(sender)))).Name == "cmdQuery")
            {
                //ά�޲�ѯ
                this.QueryEvent(sender, e);
            }
            else
            {
                //������ѯ
                this.ExprotEvent(sender, e);
            }

        }

        #region ��ѯ���ݿ���¼�

        //��ѯ��ť�¼�
        private void QueryEvent(object sender, EventArgs e)
        {
            if (this._checkRequireFields())
            {
                int receiveBeginDate = FormatHelper.TODateInt(this.txtReceiveBeginDate.Text);
                int receiveEndDate = FormatHelper.TODateInt(this.txtReceiveEndDate.Text);

                int receiveBeginTime = FormatHelper.TOTimeInt(this.txtReceiveBeginTime.Text);
                int receiveEndTime = FormatHelper.TOTimeInt(this.txtReceiveEndTime.Text);

                FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
                (e as WebQueryEventArgsNew).GridDataSource =
                    facadeFactory.CreateQueryRMATSFacade().QueryRMATSRecord(
                    FormatHelper.CleanString(this.txtConditionModel.Text).ToUpper(),
                    FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
                    FormatHelper.CleanString(this.txtReworkMo.Text).ToUpper(),
                    FormatHelper.CleanString(this.txtRMABillCode.Text).ToUpper(),
                    receiveBeginDate, receiveBeginTime,
                    receiveEndDate, receiveEndTime,
                    CheckBoxListBuilder.GetCheckedList(this.chkTSStateList),
                    (e as WebQueryEventArgsNew).StartRow,
                    (e as WebQueryEventArgsNew).EndRow);

                (e as WebQueryEventArgsNew).RowCount =
                    facadeFactory.CreateQueryRMATSFacade().QueryRMATSRecordCount(
                    FormatHelper.CleanString(this.txtConditionModel.Text).ToUpper(),
                    FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
                    FormatHelper.CleanString(this.txtReworkMo.Text).ToUpper(),
                    FormatHelper.CleanString(this.txtRMABillCode.Text).ToUpper(),
                    receiveBeginDate, receiveBeginTime,
                    receiveEndDate, receiveEndTime,
                    CheckBoxListBuilder.GetCheckedList(this.chkTSStateList));
            }
        }

        //������ť�¼�
        private void ExprotEvent(object sender, EventArgs e)
        {
            if (this._checkRequireFields())
            {
                this.QueryEvent(sender, e);
            }
        }

        #endregion

        private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToGridRowEventArgsNew).DomainObject != null)
            {
                QDORMATSRecord obj = (e as DomainObjectToGridRowEventArgsNew).DomainObject as QDORMATSRecord;
                DataRow row = DtSource.NewRow();
                row["Ts_SN"] = obj.SN;
                row["Ts_SNSeq"] = obj.RunningCardSequence;
                row["Ts_TsState"] = this.languageComponent1.GetString(obj.TsState);
                row["Ts_Details"] = "";
                row["Ts_ChangedItemDetails"] = "";
                row["Ts_ModelCode"] = obj.ModelCode;
                row["Ts_ItemCode"] = obj.ItemCode;
                row["Ts_RMABillCode"] = obj.RMABillCode;
                row["Ts_RMAReMoCode"] = obj.MoCode;
                row["Ts_ConfirmDate"] = FormatHelper.ToDateString(obj.ConfirmDate);
                row["Ts_ConfirmTime"] = FormatHelper.ToTimeString(obj.ConfiemTime);
                row["Ts_ConfirmResource"] = obj.ConfirmResource;
                row["Ts_ConfirmUser"] = obj.ConfirmUser;
                row["Ts_SouceResourceDate"] = FormatHelper.ToDateString(obj.RepaireDate);
                row["Ts_SouceResourceTime"] = FormatHelper.ToTimeString(obj.RepaireTime);
                row["Ts_RepaireResource"] = obj.RepaireResource;
                row["Ts_TSUser"] = obj.TSUser;
                row["Ts_DestOpCode"] = obj.DestOpCode;
                row["Ts_NGDate"] = FormatHelper.ToDateString(obj.SourceResourceDate);
                row["Ts_NGTime"] = FormatHelper.ToTimeString(obj.SourceResourceTime);
                (e as DomainObjectToGridRowEventArgsNew).GridRow = row;

            }
        }

        private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
        {
            if ((e as DomainObjectToExportRowEventArgsNew).DomainObject != null)
            {

                QDORMATSRecord obj = (e as DomainObjectToExportRowEventArgsNew).DomainObject as QDORMATSRecord;
                (e as DomainObjectToExportRowEventArgsNew).ExportRow =
                    new string[]{
									obj.SN,
									this.languageComponent1.GetString(obj.TsState),
									obj.ModelCode,
									obj.ItemCode,
									obj.MoCode,
									obj.RMABillCode,

									FormatHelper.ToDateString(obj.ConfirmDate),
									FormatHelper.ToTimeString(obj.ConfiemTime),
									obj.ConfirmResource,
									obj.ConfirmUser,

									FormatHelper.ToDateString(obj.RepaireDate),
									FormatHelper.ToTimeString(obj.RepaireTime),
									obj.RepaireResource,
									obj.TSUser,
									obj.DestOpCode
								};

            }
        }

        private void _helper_GetExportHeadText(object sender, EventArgs e)
        {
            (e as ExportHeadEventArgsNew).Heads =
                new string[]{
								"Ts_SN",
								"Ts_TsState",
								"Ts_ModelCode",
								"Ts_ItemCode",
								"Ts_MoCode",
								"Ts_RMABillCode",

								"Ts_ConfirmDate",
								"Ts_ConfirmTime",
								"Ts_ConfirmResource",
								"Ts_ConfirmUser",

								"Ts_SouceResourceDate",
								"Ts_SouceResourceTime",
								"Ts_RepaireResource",
								"Ts_TSUser",
								"Ts_DestOpCode"};

        }

        protected override void Grid_ClickCell(GridRecord row, string command)
        {
            if (command == "Ts_Details")
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FRMATSRecordDetailsQP.aspx",
                    new string[]{
									"ModelCode",
									"ItemCode",
									"MoCode",
									"RunningCard",
									"RunningCardSeq",
									"TSState",
									"TSResourceCode",
									"TSDate",
									"TSTime",
									"BackUrl"
								},
                    new string[]{
									row.Items.FindItemByKey("Ts_ModelCode").Text,
									row.Items.FindItemByKey("Ts_ItemCode").Text,
									row.Items.FindItemByKey("Ts_RMAReMoCode").Text,
									row.Items.FindItemByKey("Ts_SN").Text,
									row.Items.FindItemByKey("Ts_SNSeq").Text,
									row.Items.FindItemByKey("Ts_TsState").Text,
									row.Items.FindItemByKey("Ts_RepaireResource").Text,
									row.Items.FindItemByKey("Ts_NGDate").Text,
									row.Items.FindItemByKey("Ts_NGTime").Text,
									"FRMATSRecordQP.aspx"
								})
                    );
            }
            else if (command == "Ts_ChangedItemDetails")
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FRMATSChangedItemQP.aspx",
                    new string[]{
									"ModelCode",
									"ItemCode",
									"MoCode",
									"RunningCard",
									"TSState",
									"TSResourceCode",
									"TSDate",
									"TSTime",
									"BackUrl"
								},
                    new string[]{
									row.Items.FindItemByKey("Ts_ModelCode").Text,
									row.Items.FindItemByKey("Ts_ItemCode").Text,
									row.Items.FindItemByKey("Ts_RMAReMoCode").Text,
									row.Items.FindItemByKey("Ts_SN").Text,
									row.Items.FindItemByKey("Ts_TsState").Text,
									row.Items.FindItemByKey("Ts_RepaireResource").Text,
									row.Items.FindItemByKey("Ts_NGDate").Text,
									row.Items.FindItemByKey("Ts_NGTime").Text,
									"FRMATSRecordQP.aspx"
								})
                    );
            }
        }
    }
}
