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

using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.MOModel;

namespace BenQGuru.eMES.Web.BaseSetting
{
    /// <summary>
    /// FUserGroup2VendorAP ��ժҪ˵����
    /// </summary>
    public partial class FUserGroup2VendorAP : BaseAPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        protected System.Web.UI.WebControls.Label lblUserSelectTitle;

        private UserFacade facade = null;

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

                this.txtUserGroupCodeQuery.Text = this.GetRequestParam("usergroupcode");
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

            this.gridHelper.AddColumn("VendorCodeN", "��Ӧ�̴���", null);
            this.gridHelper.AddColumn("VendorNameN", "��Ӧ������", null);
            this.gridHelper.AddColumn("VendorALIAS", "��Ӧ�̱���", null);
            this.gridHelper.AddColumn("VendorUser", "��ϵ��", null);
            this.gridHelper.AddColumn("VendorAddres", "��ַ", null);
            this.gridHelper.AddColumn("VendorFax", "����", null);
            this.gridHelper.AddColumn("VendorTelephone", "�ƶ��绰", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridHelper.ApplyLanguage(this.languageComponent1);

            this.gridHelper.AddDefaultColumn(true, false);
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
            }
            return this.facade.GetUnSelectedVendorByUserGroupCode(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtVendorCodeQuery.Text)),
                inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
            }
            return this.facade.GetUnSelectedVendorByUserGroupCodeCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtVendorCodeQuery.Text)));
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["VendorCodeN"] = ((Vendor)obj).VendorCode;
            row["VendorNameN"] = ((Vendor)obj).VendorName;
            row["VendorALIAS"] = ((Vendor)obj).ALIAS;
            row["VendorUser"] = ((Vendor)obj).VENDORUSER;
            row["VendorAddres"] = ((Vendor)obj).VENDORADDR;
            row["VendorFax"] = ((Vendor)obj).FAXNO;
            row["VendorTelephone"] = ((Vendor)obj).MOBILENO;
            row["MaintainUser"] = ((Vendor)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((Vendor)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((Vendor)obj).MaintainTime);
            return row;
        }

        #endregion

        #region Button
        protected override void AddDomainObject(ArrayList domainObject)
        {
            if (facade == null)
            {
                facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
            }
            facade.AddUserGroup2Vendor((UserGroup2Vendor[])domainObject.ToArray(typeof(UserGroup2Vendor)));
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect(this.MakeRedirectUrl("./FUserGroup2VendorSP.aspx", new string[] { "usergroupcode" }, new string[] { this.txtUserGroupCodeQuery.Text }));
        } 
        #endregion

        #region Object <--> Page
        protected override object GetEditObject(GridRecord row)
        {
            if (facade == null)
            {
                facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
            }
            UserGroup2Vendor relation = facade.CreateNewUserGroup2Vendor();

            relation.UserGroupCode = this.txtUserGroupCodeQuery.Text.Trim();
            relation.VendorCode = row.Items.FindItemByKey("VendorCodeN").Text;
            relation.MaintainUser = this.GetUserCode();

            return relation;
        }
        #endregion
    }
}
