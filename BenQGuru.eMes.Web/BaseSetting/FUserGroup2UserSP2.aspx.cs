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

namespace BenQGuru.eMES.Web.BaseSetting
{
	/// <summary>
    /// FUserGroup2UserSP ��ժҪ˵����
	/// </summary>
    public partial class FUserGroup2UserSP2 : BaseMPageNew
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		private UserFacade facade = null;//new SystemSettingFacadeFactory().CreateUserFacade();	

		#region Stable
		protected void Page_Load(object sender, System.EventArgs e)
		{		
			//this.pagerSizeSelector.Readonly = true;

			if(!IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.txtUserCodeQuery.Text = this.GetRequestParam("usercode");				
			}
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}

		#endregion

		#region Not Stable

		protected override void InitWebGrid()
		{
            base.InitWebGrid();
            this.gridHelper.AddColumn("UserGroupCode", "�û������", null);
            this.gridHelper.AddColumn("UserGroupType", "�û������", null);
            this.gridHelper.AddColumn("UserGroupDescription", "�û�������", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);
			this.gridHelper.AddDefaultColumn( true, false );
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            this.gridHelper.RequestData();
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade() ;
			}
			facade.DeleteUserGroup2User((UserGroup2User[])domainObjects.ToArray(typeof(UserGroup2User)));
		}

		protected override int GetRowCount()
		{
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade() ;
			}
            return this.facade.GetSelectedUserGroupByUserCodeCount( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserCodeQuery.Text)));
		}

		protected override DataRow GetGridRow(object obj)
		{
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
            //    new object[]{"false",
            //                    ((UserGroup)obj).UserGroupCode.ToString(),
            //                    ((UserGroup)obj).UserGroupType.ToString(),
            //                    ((UserGroup)obj).UserGroupDescription.ToString(),
            //                    ((UserGroup)obj).GetDisplayText("MaintainUser"),
            //                    FormatHelper.ToDateString(((UserGroup)obj).MaintainDate),
            //                    FormatHelper.ToTimeString(((UserGroup)obj).MaintainTime),
            //                    ""});
            DataRow row = this.DtSource.NewRow();
            row["UserGroupCode"] = ((UserGroup)obj).UserGroupCode.ToString();
            row["UserGroupType"] = ((UserGroup)obj).UserGroupType.ToString();
            row["UserGroupDescription"] = ((UserGroup)obj).UserGroupDescription.ToString();
            row["MaintainUser"] = ((UserGroup)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((UserGroup)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((UserGroup)obj).MaintainTime);
            return row;
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{	
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade() ;
			}
            return facade.GetSelectedUserGroupByUserCode( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserCodeQuery.Text)),
				inclusive,exclusive);
		}

		protected override object GetEditObject(GridRecord row)
		{
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade() ;
			}
			UserGroup2User relation = facade.CreateUserGroup2User();

			relation.UserCode = this.txtUserCodeQuery.Text.Trim();
            relation.UserGroupCode = row.Items.FindItemByKey("UserGroupCode").Text;		
			relation.MaintainUser = this.GetUserCode();

			return relation;
		}

		protected void cmdSelect_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FUserGroup2UserAP2.aspx", new string[]{"usercode"}, new string[]{this.txtUserCodeQuery.Text.Trim()}));
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
            this.Response.Redirect(this.MakeRedirectUrl("./FUserMP.aspx"));
		}

		#endregion

        //ˢ��ҳ��ʹ��
        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            this.gridHelper.RequestData();
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
			this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
		#endregion
		
	}
}
