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
    /// FUserGroup2UserAP ��ժҪ˵����
	/// </summary>
	public partial class FUserGroup2UserAP2 : BaseAPageNew
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.WebControls.Label lblUserSelectTitle;

		private UserFacade facade = null;//new SystemSettingFacadeFactory().CreateUserFacade();


		#region Stable
		protected void Page_Load(object sender, System.EventArgs e)
		{
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
			
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

			this.gridHelper.AddDefaultColumn(true,false);
		}

		protected override void AddDomainObject(ArrayList domainObject)
		{
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade() ;
			}
			facade.AddUserGroup2User( (UserGroup2User[])domainObject.ToArray(typeof(UserGroup2User)));
		}

		protected override object GetEditObject(GridRecord row)
		{
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade() ;
			}
			UserGroup2User relation  = facade.CreateUserGroup2User();

			relation.UserCode = this.txtUserCodeQuery.Text.Trim();
            relation.UserGroupCode = row.Items.FindItemByKey("UserGroupCode").Text;		
			relation.MaintainUser = this.GetUserCode();

			return relation;
		}

		protected override int GetRowCount()
		{
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade() ;
			}
            return this.facade.GetUnselectedUserGroupByUserCodeCount( 
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
            //                    FormatHelper.ToTimeString(((UserGroup)obj).MaintainTime)});
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
            return this.facade.GetUnselectedUserGroupByUserCode( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserCodeQuery.Text)),
				inclusive,exclusive);
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FUserGroup2UserSP2.aspx", new string[]{"usercode"}, new string[]{this.txtUserCodeQuery.Text}));
		}
		#endregion

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
