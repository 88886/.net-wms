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
    /// FUserGroup2FunctionGroupAP ��ժҪ˵����
	/// </summary>
	public partial class FUserGroup2FunctionGroupAP : BaseAPageNew
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.WebControls.Label lblUserSelectTitle;

		private SystemSettingFacade facade = null;//new SystemSettingFacadeFactory().CreateSystemSettingFacade();


		#region Stable
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
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

		#region Not Stable

		protected override void InitWebGrid()
		{
            base.InitWebGrid();
            this.gridHelper.AddColumn("FunctionGroupCode", "���������", null);
            this.gridHelper.AddColumn("FunctionGroupDescription", "����������", null);
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
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateSystemSettingFacade() ;
			}
            facade.AddUserGroup2FunctionGroup((UserGroup2FunctionGroup[])domainObject.ToArray(typeof(UserGroup2FunctionGroup)));
		}

		protected override object GetEditObject(GridRecord row)
		{
			if(facade==null)
			{
                facade = new SystemSettingFacadeFactory(base.DataProvider).CreateSystemSettingFacade();
			}
            UserGroup2FunctionGroup relation = facade.CreateNewUserGroup2FunctionGroup();

			relation.UserGroupCode = this.txtUserGroupCodeQuery.Text.Trim();
            relation.FunctionGroupCode = row.Items.FindItemByKey("FunctionGroupCode").Text;		
			relation.MaintainUser = this.GetUserCode();

			return relation;
		}

		protected override int GetRowCount()
		{
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateSystemSettingFacade() ;
			}
			return this.facade.GetUnselectedFunctionGroupByUserGroupCodeCount( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtFunctionGroupCodeQuery.Text)));
		}

		protected override DataRow GetGridRow(object obj)
		{
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow(
            //    new object[]{"false",
            //                    ((FunctionGroup)obj).FunctionGroupCode.ToString(),
            //                    ((FunctionGroup)obj).FunctionGroupDescription.ToString(),
            //                    ((FunctionGroup)obj).GetDisplayText("MaintainUser"),
            //                    FormatHelper.ToDateString(((FunctionGroup)obj).MaintainDate),
            //                    FormatHelper.ToTimeString(((FunctionGroup)obj).MaintainTime)
            //                    });
            DataRow row = this.DtSource.NewRow();
            row["FunctionGroupCode"] = ((FunctionGroup)obj).FunctionGroupCode.ToString();
            row["FunctionGroupDescription"] = ((FunctionGroup)obj).FunctionGroupDescription.ToString();
            row["MaintainUser"] = ((FunctionGroup)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((FunctionGroup)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((FunctionGroup)obj).MaintainTime);
            return row;

		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{	
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).CreateSystemSettingFacade() ;
			}
            return this.facade.GetUnselectedFunctionGroupByUserGroupCode( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtFunctionGroupCodeQuery.Text)),
				inclusive,exclusive);
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FUserGroup2FunctionGroupSP.aspx", new string[]{"usergroupcode"}, new string[]{this.txtUserGroupCodeQuery.Text}));
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
