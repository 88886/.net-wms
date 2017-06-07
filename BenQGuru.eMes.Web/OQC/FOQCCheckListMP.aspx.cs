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
using BenQGuru.eMES.OQC;
using BenQGuru.eMES.Domain.OQC;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.OQC
{
    /// <summary>
    /// FOQCCheckListMP ��ժҪ˵����
    /// </summary>
    public partial class FOQCCheckListMP : BaseMPageNew
    {

        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        private BenQGuru.eMES.OQC.OQCFacade _facade;// =new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;

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
            if (!this.IsPostBack)
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
            this.gridHelper.AddColumn("CheckItemCode", "������Ŀ", null);
            this.gridHelper.AddColumn("CheckItemDesc", "����", null);
            this.gridHelper.AddColumn("CheckValueMin", "��С�趨ֵ", null);
            this.gridHelper.AddColumn("CheckValueMax", "����趨ֵ", null);
            this.gridHelper.AddColumn("Unit", "��λ", null);
            this.gridHelper.AddColumn("MaintainUser", "ά����Ա", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridHelper.AddDefaultColumn(true, true);
            //this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
            //this.gridWebGrid.Columns.FromKey("Description").Hidden = true;

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["CheckItemCode"] = ((OQCCheckList)obj).CheckItemCode.ToString();
            row["CheckItemDesc"] = ((OQCCheckList)obj).Description.ToString();
            row["CheckValueMin"] = ((OQCCheckList)obj).CheckValueMin.ToString();
            row["CheckValueMax"] = ((OQCCheckList)obj).CheckValueMax.ToString();
            row["Unit"] = ((OQCCheckList)obj).CheckIemUnit.ToString();
            row["MaintainUser"] = ((OQCCheckList)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString((obj as OQCCheckList).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString((obj as OQCCheckList).MaintainTime);
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null) { _facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade(); }
            return this._facade.QueryOQCCheckList(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCheckListCodeQuery.Text)),
                inclusive, exclusive);
        }


        protected override int GetRowCount()
        {
            if (_facade == null) { _facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade(); }
            return this._facade.QueryOQCCheckListCount(
                FormatHelper.CleanString(this.txtCheckListCodeQuery.Text));
        }

        #endregion

        #region Button

        protected override void AddDomainObject(object domainObject)
        {
            if (_facade == null) { _facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade(); }
            this._facade.AddOQCCheckList((OQCCheckList)this.GetEditObject());
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (_facade == null) { _facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade(); }
            this._facade.DeleteOQCCheckList((OQCCheckList[])domainObjects.ToArray(typeof(OQCCheckList)));
        }

        protected override void UpdateDomainObject(object domainObject)
        {
            if (_facade == null) { _facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade(); }
            this._facade.UpdateOQCCheckList((OQCCheckList)this.GetEditObject());

        }

        protected override void buttonHelper_AfterPageStatusChangeHandle(string pageAction)
        {
            if (pageAction == PageActionType.Add)
            {
                this.txtCheckListCodeEdit.ReadOnly = false;
            }

            if (pageAction == PageActionType.Update)
            {
                this.txtCheckListCodeEdit.ReadOnly = true;
            }
        }

        #endregion

        #region Object <--> Page

        protected override object GetEditObject()
        {
            if (_facade == null) { _facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade(); }
            OQCCheckList checklist = this._facade.CreateNewOQCCheckList();

            checklist.CheckItemCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCheckListCodeEdit.Text, 40));
            checklist.Description = FormatHelper.CleanString(this.txtCheckItemDesc.Text);
            checklist.CheckIemUnit = FormatHelper.CleanString(this.txtUnit.Text);
            checklist.CheckValueMax = FormatHelper.CleanString(this.txtCheckValueMax.Text);
            checklist.CheckValueMin = FormatHelper.CleanString(this.txtCheckValueMin.Text);
            checklist.MaintainUser = this.GetUserCode();

            return checklist;
        }


        protected override object GetEditObject(GridRecord row)
        {
            if (_facade == null)
            {
                _facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade();
            }
            string strCode = string.Empty;
            object objCode = row.Items.FindItemByKey("CheckItemCode").Value;
            if (objCode != null)
            {
                strCode = objCode.ToString();
            }
            object obj = _facade.GetOQCCheckList(strCode);
            if (obj != null)
            {
                return (OQCCheckList)obj;
            }
            return null;
        }

        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.txtCheckListCodeEdit.Text = "";
                this.txtCheckItemDesc.Text = "";
                this.txtCheckValueMax.Text = "";
                this.txtCheckValueMin.Text = "";
                this.txtUnit.Text = "";

                return;
            }

            this.txtCheckListCodeEdit.Text = ((OQCCheckList)obj).CheckItemCode.ToString();
            this.txtCheckItemDesc.Text = ((OQCCheckList)obj).Description;
            this.txtCheckValueMax.Text = ((OQCCheckList)obj).CheckValueMax;
            this.txtCheckValueMin.Text = ((OQCCheckList)obj).CheckValueMin;
            this.txtUnit.Text = ((OQCCheckList)obj).CheckIemUnit;

        }


        protected override bool ValidateInput()
        {
            PageCheckManager manager = new PageCheckManager();

            manager.Add(new LengthCheck(lblCheckListCodeEdit, txtCheckListCodeEdit, 40, true));
            manager.Add(new LengthCheck(lblCheckItemDesc, txtCheckItemDesc, 100, false));
            manager.Add(new LengthCheck(lblCheckValueMin, txtCheckValueMin, 40, false));
            if (txtCheckValueMin.Text != string.Empty)
            {
                manager.Add(new DecimalCheck(lblCheckValueMin, txtCheckValueMin, Convert.ToDecimal("-9999999999.999999"), Convert.ToDecimal("9999999999.999999"), false));
            }
            manager.Add(new LengthCheck(lblCheckValueMax, txtCheckValueMax, 40, false));
            if (txtCheckValueMax.Text != string.Empty)
            {
                manager.Add(new DecimalCheck(lblCheckValueMax, txtCheckValueMax, Convert.ToDecimal("-9999999999.999999"), Convert.ToDecimal("9999999999.999999"), false));
            }
            manager.Add(new LengthCheck(lblUnit, txtUnit, 40, false));
            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }

            return true;
        }

        #endregion


        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{   ((OQCCheckList)obj).CheckItemCode.ToString(),
                                ((OQCCheckList)obj).Description .ToString(),
                                               ((OQCCheckList)obj).CheckValueMax .ToString(),
                ((OQCCheckList)obj).CheckValueMin .ToString(),
 
                ((OQCCheckList)obj).CheckIemUnit .ToString(),
                                    ((OQCCheckList)obj).GetDisplayText("MaintainUser"),
                                    FormatHelper.ToDateString(((OQCCheckList)obj).MaintainDate)}
                ;
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"CheckItemCode",
                                    "CheckItemDesc",
                                    "CheckValueMax",
                                    "CheckValueMin",
                                    "Unit",
                                    "MaintainUser",
                                    "MaintainDate"};
        }
        #endregion
    }
}
