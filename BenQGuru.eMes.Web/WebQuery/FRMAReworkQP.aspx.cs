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

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Web.UserControl;
using BenQGuru.eMES.WebQuery;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.DataCollect;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.WebQuery
{
    /// <summary>
    /// FItemTracingQP ��ժҪ˵����
    /// </summary>
    public partial class FRMAReworkQP : BaseMPageNew
    {


        private System.ComponentModel.IContainer components;
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;

        private QueryRMATSFacade _facade = null; //FacadeFactory.CreateQueryFacade2() ;



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
            //this.gridWebGrid.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
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
            FormatHelper.SetSNRangeValue(txtStartSnQuery, txtEndSnQuery);
            if (!Page.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
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
            this.gridHelper.AddLinkColumn("IT_ProductionProcess", "��������", null);
            this.gridHelper.AddLinkColumn("INNODetail", "����������Ϣ", null);
            this.gridHelper.AddLinkColumn("KeypartsDetail", "��������Ϣ", null);
            this.gridHelper.AddColumn("MaterialModelCode", "������о", null);
            this.gridHelper.AddColumn("ItemCode", "��Ʒ����", null);
            this.gridHelper.AddColumn("IT_SN", "���к�", null);
            this.gridHelper.AddColumn("IT_TCard", "ת��ǰ���к�", null);
            this.gridHelper.AddColumn("BigLine", "����", null);
            this.gridHelper.AddColumn("IT_MOCode", "����", null);
            this.gridHelper.AddLinkColumn("IT_MOLink", "������Ϣ", null);
            this.gridHelper.AddColumn("IT_ItemStatus", "��Ʒ״̬", null);
            this.gridHelper.AddColumn("IT_LOCAOP", "���ڹ���", null);
            this.gridHelper.AddColumn("IT_OPType", "������", null);
            this.gridHelper.AddColumn("IT_OPResult", "������", null);
            this.gridHelper.AddColumn("IT_Route", "����;��", null);
            this.gridHelper.AddColumn("IT_Segment", "����", null);
            this.gridHelper.AddColumn("IT_Line", "������", null);
            this.gridHelper.AddColumn("IT_Resource", "��Դ", null);
            this.gridHelper.AddColumn("IT_MaintainDate", "����", null);
            this.gridHelper.AddColumn("IT_MaintainTime", "ʱ��", null);
            this.gridHelper.AddColumn("IT_MaintainUser", "������", null);

            this.gridHelper.AddColumn("IT_OPType_ORI", "������", null);

            this.gridHelper.Grid.Columns.FromKey("IT_OPType_ORI").Hidden = true;
            this.gridHelper.Grid.Columns.FromKey("IT_OPType").Hidden = true;
            this.gridHelper.Grid.Columns.FromKey("IT_TCard").Hidden = true;

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);

            string rcardFrom = this.GetRequestParam("RCARDFROM");
            string rcardTo = this.GetRequestParam("RCARDTO");

            if (rcardFrom != string.Empty || rcardTo != string.Empty)
            {

                this.txtStartSnQuery.Text = rcardFrom;
                this.txtEndSnQuery.Text = rcardTo;
                this.gridHelper.RequestData();
            }

        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = DtSource.NewRow();
            row["IT_ProductionProcess"] = "";
            row["INNODetail"] = "";
            row["KeypartsDetail"] = "";
            row["MaterialModelCode"] = ((ItemTracingQuery)obj).MaterialModelCode;
            row["ItemCode"] = ((ItemTracingQuery)obj).ItemCode;
            row["IT_SN"] = ((ItemTracingQuery)obj).RCard;
            row["IT_TCard"] = ((ItemTracingQuery)obj).TCard;
            row["BigLine"] = ((ItemTracingQuery)obj).BigStepSequenceCode;
            row["IT_MOCode"] = ((ItemTracingQuery)obj).MOCode;
            row["IT_MOLink"] = "";
            row["IT_ItemStatus"] = this.languageComponent1.GetString(((ItemTracingQuery)obj).ItemStatus);
            row["IT_LOCAOP"] = ((ItemTracing)obj).OPCode;
            row["IT_OPType"] = this.languageComponent1.GetString(((ItemTracingQuery)obj).OPType);
            row["IT_OPResult"] = GetOPTypeString((ItemTracingQuery)obj);
            row["IT_Route"] = ((ItemTracingQuery)obj).RouteCode;
            row["IT_Segment"] = ((ItemTracingQuery)obj).SegmentCode;
            row["IT_Line"] = ((ItemTracingQuery)obj).LineCode;
            row["IT_Resource"] = ((ItemTracingQuery)obj).ResCode;
            row["IT_MaintainDate"] = FormatHelper.ToDateString(((ItemTracingQuery)obj).MaintainDate);
            row["IT_MaintainTime"] = FormatHelper.ToTimeString(((ItemTracingQuery)obj).MaintainTime);
            row["IT_MaintainUser"] = ((ItemTracingQuery)obj).MaintainUser;
            row["IT_OPType_ORI"] = ((ItemTracingQuery)obj).OPType;
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
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

            if (_facade == null)
            {
                _facade = new QueryRMATSFacade(this.DataProvider);
            }


            return this._facade.QueryRMARework(
                FormatHelper.PKCapitalFormat(startSourceCard),
                FormatHelper.PKCapitalFormat(endSourceCard),
                FormatHelper.CleanString(this.txtReMOQuery.Text.Trim()),
                inclusive, exclusive);


        }


        protected override int GetRowCount()
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

            if (_facade == null)
            {
                _facade = new QueryRMATSFacade(this.DataProvider);
            }


            return this._facade.QueryRMAReworkCount(
                    FormatHelper.PKCapitalFormat(startSourceCard),
                    FormatHelper.PKCapitalFormat(endSourceCard),
                    FormatHelper.CleanString(this.txtReMOQuery.Text.Trim())
                   );

        }

        #endregion

        protected override void cmdQuery_Click(object sender, EventArgs e)
        {
            DoQueryCheck();
            base.cmdQuery_Click(sender, e);
        }

        private void DoQueryCheck()
        {
            // �������������Ϊ��
            //if (FormatHelper.CleanString(this.txtStartSnQuery.Text) == string.Empty && FormatHelper.CleanString(this.txtEndSnQuery.Text) == string.Empty)
            //{
            //    ExceptionManager.Raise(this.GetType(), "$ItemTracing_At_Least_One_SN_Not_NULL");
            //}

        }


        #region Export
        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                                    ((ItemTracingQuery)obj).RCard.ToString(),
                                    ((ItemTracingQuery)obj).MOCode.ToString(),
                                    ((ItemTracingQuery)obj).MaterialModelCode.ToString(),
                                    ((ItemTracingQuery)obj).ItemCode.ToString(),
                                    ((ItemTracingQuery)obj).BigStepSequenceCode.ToString(),
                                    this.languageComponent1.GetString (((ItemTracingQuery)obj).ItemStatus.ToString()),
                                    ((ItemTracingQuery)obj).OPCode.ToString(),
                                    WebQueryHelper.GetOPResultLinkText(this.languageComponent1, ((ItemTracingQuery)obj).OPType ),
                                    ((ItemTracingQuery)obj).RouteCode.ToString(),
                                    ((ItemTracingQuery)obj).SegmentCode.ToString(),
                                    ((ItemTracingQuery)obj).LineCode.ToString(),
                                    ((ItemTracingQuery)obj).ResCode.ToString(),
                                    FormatHelper.ToDateString(((ItemTracingQuery)obj).MaintainDate),
                                    FormatHelper.ToTimeString(((ItemTracingQuery)obj).MaintainTime),
                                    ((ItemTracingQuery)obj).MaintainUser.ToString()
                                }
                ;
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {   "IT_SN", 
                                    "IT_MOCode", 
                                    "MaterialModelCode",
                                    "ItemCode",
                                    "BigLine",
                                    "IT_ItemStatus", 
                                    "IT_LOCAOP", 
                                    "IT_OPResult", 
                                    "IT_Route", 
                                    "IT_Segment", 
                                    "IT_Line", 
                                    "IT_Resource", 
                                    "IT_MaintainDate", 
                                    "IT_MaintainTime", 
                                    "IT_MaintainUser"
                                };
        }
        #endregion

        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            string moCode = row.Items.FindItemByKey("IT_MOCode").Text;
            string rcard = row.Items.FindItemByKey("IT_SN").Text;
            string tcard = row.Items.FindItemByKey("IT_TCard").Text;
            string opType = row.Items.FindItemByKey("IT_OPType").Text;
            string opcode = row.Items.FindItemByKey("IT_LOCAOP").Text;

            if (commandName=="IT_ProductionProcess" )
            {
                Response.Redirect(
                    //"FITProductionProcessQP.aspx?MOCODE=" + moCode + "&RCARD=" + rcard + "&TCARD=" + tcard );
                    this.MakeRedirectUrl("FITProductionProcessQP.aspx",
                    new string[] { "MOCODE", "RCARD", "TCARD" },
                    new string[] { moCode, rcard, tcard }));
            }
            else if (commandName=="IT_MOLink" )
            {
                Response.Redirect("FITMOInfoQP.aspx?RCARD=" + rcard);
            }
            else if (commandName=="INNODetail" )
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FINNOInfoQP.aspx",
                    new string[]{
									"INNO",
									"OPCode"
								},
                    new string[]{
									rcard,
									opcode
								})
                    );
            }
            else if (commandName=="KeypartsDetail" )
            {
                this.Response.Redirect(
                    this.MakeRedirectUrl(
                    "FKeypartsInfoQP.aspx",
                    new string[]{
									"Keyparts",
									"OPCode"
								},
                    new string[]{
									rcard,
									opcode
								})
                    );
            }
        }


        private string GetOPTypeString(ItemTracing obj)
        {
            return WebQueryHelper.GetOPResultLinkHtml2(
                this.languageComponent1,
                obj.LastAction,
                obj.RCard,
                obj.RCardSeq,
                obj.MOCode,
                "FItemTracingQP.aspx"
                );
            //            return WebQueryHelper.GetOPResultLinkHtml(
            //                this.languageComponent1,
            //                obj.OPType,
            //                obj.RCard,
            //                obj.RCardSeq,
            //                obj.MOCode,
            //                "FItemTracingQP.aspx"
            //                //"FItemTracingQP.aspx?RCARDFROM=" + this.txtStartSnQuery.Value + "&RCARDTO=" + this.txtEndSnQuery.Value
            //                ) ;
        }
    }
}
