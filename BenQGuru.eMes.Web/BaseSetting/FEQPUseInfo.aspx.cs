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
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.BaseSetting
{
    /// <summary>
    /// FRouteMP ��ժҪ˵����
    /// </summary>
    public partial class FEQPUseInfo : BaseMPageNew
    {
        protected System.Web.UI.WebControls.Label lblRouteTitle;


        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        BenQGuru.eMES.BaseSetting.SystemSettingFacade sysFacade = null;
        private BenQGuru.eMES.Material.EquipmentFacade _facade = null;//	new BaseModelFacadeFactory().Create();
        private BenQGuru.eMES.BaseSetting.BaseModelFacade _BaseModelFacadeFactory = null;

        public TextBox txtDateUseBeginQuery;
        public TextBox txtDateUseEndQuery;
        public TextBox txtDateUseEdit;

        public BenQGuru.eMES.Web.UserControl.eMESTime txtOnTimeEdit;
        public BenQGuru.eMES.Web.UserControl.eMESTime txtOffTimeEdit;

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
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
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
            this.gridHelper.AddColumn("EQPID", "�豸����", null);
            this.gridHelper.AddColumn("EQPDESC", "�豸����", null);
            this.gridHelper.AddColumn("UseDateUnVisible", "ʹ������", null);
            this.gridHelper.AddColumn("UseDate", "ʹ������", null);
            this.gridHelper.AddColumn("EQPWorkingTime", "�ƻ�����ʱ��", null);
            this.gridHelper.AddColumn("OnTime", "����ʱ��", null);
            this.gridHelper.AddColumn("OffTime", "�ػ�ʱ��", null);
            this.gridHelper.AddColumn("RUNuration", "ʵ�ʹ���ʱ��", null);
            this.gridHelper.AddColumn("STOPDuration", "ͣ��ʱ��(��)", null);            
            this.gridHelper.AddColumn("MaintainUser", "ά������", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);
 
            this.gridHelper.AddDefaultColumn(true, true);
            this.gridWebGrid.Columns.FromKey("UseDateUnVisible").Hidden = true;
            //this.gridWebGrid.Columns.FromKey("OffTime").Hidden = true;
            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override DataRow GetGridRow(object obj)
        {
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
            //    new object[]{"false",
            //                    ((Domain.Equipment.EQPUseInfoForQuery)obj).Eqpid,
            //                    ((Domain.Equipment.EQPUseInfoForQuery)obj).EQPDESC,
            //                    ((Domain.Equipment.EQPUseInfoForQuery)obj).Usedate,
            //                    FormatHelper.ToDateString(((Domain.Equipment.EQPUseInfoForQuery)obj).Usedate),
            //                    ((Domain.Equipment.EQPUseInfoForQuery)obj).Worktime,
            //                    FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfoForQuery)obj).Ontime),
            //                    FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfoForQuery)obj).Offtime),
            //                    ((Domain.Equipment.EQPUseInfoForQuery)obj).Runuration,
            //                    ((Domain.Equipment.EQPUseInfoForQuery)obj).Stopduration,
            //                    ((Domain.Equipment.EQPUseInfoForQuery)obj).GetDisplayText("MaintainUser"),
            //                    FormatHelper.ToDateString(((Domain.Equipment.EQPUseInfoForQuery)obj).Mdate),
            //                    FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfoForQuery)obj).Mtime),""
            //    });
            DataRow row = this.DtSource.NewRow();
            row["EQPID"] = ((Domain.Equipment.EQPUseInfoForQuery)obj).Eqpid;
            row["EQPDESC"] = ((Domain.Equipment.EQPUseInfoForQuery)obj).EQPDESC;
            row["UseDateUnVisible"] = ((Domain.Equipment.EQPUseInfoForQuery)obj).Usedate;
            row["UseDate"] = FormatHelper.ToDateString(((Domain.Equipment.EQPUseInfoForQuery)obj).Usedate);
            row["EQPWorkingTime"] = ((Domain.Equipment.EQPUseInfoForQuery)obj).Worktime;
            row["OnTime"] = FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfoForQuery)obj).Ontime);
            row["OffTime"] = FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfoForQuery)obj).Offtime);
            row["RUNuration"] = ((Domain.Equipment.EQPUseInfoForQuery)obj).Runuration;
            row["STOPDuration"] = ((Domain.Equipment.EQPUseInfoForQuery)obj).Stopduration;
            row["MaintainUser"] = ((Domain.Equipment.EQPUseInfoForQuery)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((Domain.Equipment.EQPUseInfoForQuery)obj).Mdate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfoForQuery)obj).Mtime);
            return row;

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null)
            {
                _facade = new BenQGuru.eMES.Material.EquipmentFacade(base.DataProvider);
            }
            int beginDate = FormatHelper.TODateInt(this.txtDateUseBeginQuery.Text.Trim());
            int endDate = FormatHelper.TODateInt(this.txtDateUseEndQuery.Text.Trim());

            return this._facade.QueryEQPUseInfo(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtEQPIDQuery.Text)),beginDate,endDate,inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_facade == null)
            {
                _facade = new BenQGuru.eMES.Material.EquipmentFacade(base.DataProvider);
            }

            int beginDate = FormatHelper.TODateInt(this.txtDateUseBeginQuery.Text.Trim());
            int endDate = FormatHelper.TODateInt(this.txtDateUseEndQuery.Text.Trim());

            return this._facade.QueryEQPUseInfoCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtEQPIDQuery.Text)), beginDate,endDate);

        }

        #endregion

        #region Button

        protected override void AddDomainObject(object domainObject)
        {
            if (_facade == null)
            {
                _facade = new BenQGuru.eMES.Material.EquipmentFacade(base.DataProvider);
            }

            object objEqp = this._facade.GetEquipment(((Domain.Equipment.EQPUseInfo)domainObject).Eqpid);
            if (objEqp == null)
            {
                WebInfoPublish.Publish(this, "$Error_EQPID_IS_NOT_EXIST", languageComponent1);
                return;

            }
            object obj = this._facade.GetEQPUseInfo(((Domain.Equipment.EQPUseInfo)domainObject).Eqpid, ((Domain.Equipment.EQPUseInfo)domainObject).Usedate);
            if (obj == null)
            {
                this._facade.AddEQPUseInfo((Domain.Equipment.EQPUseInfo)domainObject);
            }
            else
            {
                WebInfoPublish.Publish(this, "$Error_PK_is_Repeat", languageComponent1);
            }
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (_facade == null)
            {
                _facade = new BenQGuru.eMES.Material.EquipmentFacade(base.DataProvider);
            }

            this._facade.DeleteEQPUseInfo((Domain.Equipment.EQPUseInfo[])domainObjects.ToArray(typeof(Domain.Equipment.EQPUseInfo)));

        }

        protected override void UpdateDomainObject(object domainObject)
        {
            if (_facade == null)
            {
                _facade = new BenQGuru.eMES.Material.EquipmentFacade(base.DataProvider);
            }
            this._facade.UpdateEQPUseInfo((Domain.Equipment.EQPUseInfo)domainObject);
        }

        //protected override void Grid_ClickCell(UltraGridCell cell)
        //{
        //    base.Grid_ClickCell(cell);
        //}

        protected override void buttonHelper_AfterPageStatusChangeHandle(string pageAction)
        {
            if (pageAction == PageActionType.Add)
            {
                this.txtEQPIDEdit.Readonly = false;
                this.txtDateUseEdit.Enabled = true;
                this.txtDateUseEdit.ReadOnly = false;
            }

            if (pageAction == PageActionType.Update)
            {
                this.txtEQPIDEdit.Readonly = true;
                this.txtDateUseEdit.Enabled = false;
                this.txtDateUseEdit.ReadOnly = true;
            }
            if (pageAction == PageActionType.Delete)
            {
                this.gridHelper.RequestData();
            }
        }
        #endregion

        #region Object <--> Page

        protected override object GetEditObject()
        {
            if (_facade == null)
            {
                _facade = new Material.EquipmentFacade(base.DataProvider);
            }

            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            Domain.Equipment.EQPUseInfo route = this._facade.CreateNewEQPUseInfo();

            route.Eqpid = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtEQPIDEdit.Text, 40));
            route.Usedate = FormatHelper.TODateInt(this.txtDateUseEdit.Text.Trim());
            route.Offtime = FormatHelper.TOTimeInt(this.txtOffTimeEdit.Text.Trim());
            route.Ontime = FormatHelper.TOTimeInt(this.txtOnTimeEdit.Text.Trim());
            route.Runuration = GetRunTime(route.Usedate, route.Offtime, route.Ontime);
            route.Stopduration = int.Parse(this.txtSTOPDurationEdit.Text.Trim());
            route.MaintainUser = this.GetUserCode();
            route.Mdate = dbDateTime.DBDate;
            route.Mtime = dbDateTime.DBTime;

            return route;
        }


        protected override object GetEditObject(GridRecord row)
        {
            if (_facade == null)
            {
                _facade = new BenQGuru.eMES.Material.EquipmentFacade(base.DataProvider);
            }
            object obj = _facade.GetEQPUseInfo(row.Items.FindItemByKey("EQPID").Text.ToString(), int.Parse(row.Items.FindItemByKey("UseDateUnVisible").Text.Trim()));

            if (obj != null)
            {
                return (Domain.Equipment.EQPUseInfo)obj;
            }

            return null;
        }

        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.txtEQPIDEdit.Text = "";
                this.txtDateUseEdit.Text = "";
                this.txtOffTimeEdit.Text = "";
                this.txtOnTimeEdit.Text = "";
                this.txtSTOPDurationEdit.Text = "";
                return;
            }

             this.txtEQPIDEdit.Text = ((Domain.Equipment.EQPUseInfo)obj).Eqpid.ToString();
             this.txtDateUseEdit.Text = FormatHelper.ToDateString(((Domain.Equipment.EQPUseInfo)obj).Usedate);
             this.txtOffTimeEdit.Text = FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfo)obj).Offtime);
             this.txtOnTimeEdit.Text = FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfo)obj).Ontime);
             this.txtSTOPDurationEdit.Text = ((Domain.Equipment.EQPUseInfo)obj).Stopduration.ToString();

        }


        protected override bool ValidateInput()
        {
            PageCheckManager manager = new PageCheckManager();

            manager.Add(new LengthCheck(this.lblPEQPIDEdit, this.txtEQPIDEdit, 40, true));
            manager.Add(new NumberCheck(this.lblSTOPDurationEdit, this.txtSTOPDurationEdit,0, 999999, true));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }

            if (this.txtDateUseEdit.Text.Trim().Length <= 0)
            {
                WebInfoPublish.Publish(this, "$Error_UserDate_Null", this.languageComponent1);
                return false;
            }
            if (this.txtOnTimeEdit.Text.Trim().Length <= 0)
            {
                WebInfoPublish.Publish(this, "$Error_OnTime_Null", this.languageComponent1);
                return false;
            }
            if (this.txtOffTimeEdit.Text.Trim().Length <= 0)
            {
                WebInfoPublish.Publish(this, "$Error_OffTime_Null", this.languageComponent1);
                return false;
            }
            if (FormatHelper.TOTimeInt(this.txtOffTimeEdit.Text.Trim()) <= FormatHelper.TOTimeInt(this.txtOnTimeEdit.Text.Trim()))
            {                
                WebInfoPublish.Publish(this, "$Message_OffTime_Must_Bigger_Than_OnTime", this.languageComponent1);
                return false;
            }
            
            return true;
        }

        //ʵ������ʱ��=�ػ�ʱ��-����ʱ��
        public int GetRunTime(int today,int offtime,int ontime)
        {
            DateTime dtOfftime =  FormatHelper.ToDateTime(today,offtime);
            DateTime dtOntime = FormatHelper.ToDateTime(today, ontime);
            int runtime = (int)((dtOfftime.Ticks - dtOntime.Ticks) / (10000000 * 60));                                
            return runtime;
        }

        #endregion

        #region Export
        // 2005-04-06
        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                                ((Domain.Equipment.EQPUseInfoForQuery)obj).Eqpid,
                                ((Domain.Equipment.EQPUseInfoForQuery)obj).EQPDESC,
								((Domain.Equipment.EQPUseInfoForQuery)obj).Usedate.ToString(),
                                ((Domain.Equipment.EQPUseInfoForQuery)obj).Worktime.ToString(),
                                FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfoForQuery)obj).Ontime),
                                FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfoForQuery)obj).Offtime),
                                ((Domain.Equipment.EQPUseInfoForQuery)obj).Runuration.ToString(),
                                ((Domain.Equipment.EQPUseInfoForQuery)obj).Stopduration.ToString(),
 								((Domain.Equipment.EQPUseInfoForQuery)obj).GetDisplayText("MaintainUser"),
								FormatHelper.ToDateString(((Domain.Equipment.EQPUseInfoForQuery)obj).Mdate),
								FormatHelper.ToTimeString(((Domain.Equipment.EQPUseInfoForQuery)obj).Mtime)
                                };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"EQPID",
									"EQPDESC",
                                    "UseDate",
                                    "EQPWorkingTime",
                                    "OnTime",
                                    "OffTime",
                                    "RUNuration",
                                    "STOPDuration",
									"MaintainUser","MaintainDate","MaintainTime"};
        }

        #endregion
    }
}

