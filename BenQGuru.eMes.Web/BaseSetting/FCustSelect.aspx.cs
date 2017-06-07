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

using Infragistics.WebUI.UltraWebGrid;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Security;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;

namespace BenQGuru.eMES.Web.BaseSetting
{
	/// <summary>
	/// FUserGroupMP ��ժҪ˵����
	/// </summary>
	public partial class FCustSelect : BaseMPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		private BenQGuru.eMES.BaseSetting.UserFacade _facade = null ; //new SystemSettingFacadeFactory().CreateUserFacade();
	
		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
			this.gridWebGrid.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
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
				this.InitialData();
			}
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}

		private void InitialData()
		{
			
		}

		#endregion

		#region WebGrid
		protected override void InitWebGrid()
		{
			this.gridHelper.AddColumn( "CGroupCode", "�ͻ������",	null);
			this.gridHelper.AddColumn( "CGroupType", "�ͻ������",	null);
			this.gridHelper.AddColumn( "CGroupDescription", "�ͻ�������",null);
			this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this.gridHelper.AddColumn( "MaintainDate", "ά������",null);
			this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",null);
			this.gridHelper.AddLinkColumn( "EditUser","��Ʒ",null);
			
			this.gridHelper.AddDefaultColumn(true,false);
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}
        #endregion

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null)
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade() ;
			}
			return this._facade.CQueryUserGroup( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
				inclusive, exclusive );
		}
		protected override int GetRowCount()
		{
			if(_facade==null)
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade() ;
			}
			return this._facade.CQueryUserGroupCount( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)));
		}
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{"false",
								((UserGroup)obj).UserGroupCode.ToString(),
								((UserGroup)obj).UserGroupType.ToString(),
								((UserGroup)obj).UserGroupDescription.ToString(),
								((UserGroup)obj).MaintainUser.ToString(),
								FormatHelper.ToDateString(((UserGroup)obj).MaintainDate),
								FormatHelper.ToTimeString(((UserGroup)obj).MaintainTime),
								""});
		}

		


		#region Export
		// 2005-04-06

		protected override string[] FormatExportRecord( object obj )
		{
			return new string[]{ ((UserGroup)obj).UserGroupCode.ToString(),
								   ((UserGroup)obj).UserGroupType.ToString(),
								   ((UserGroup)obj).UserGroupDescription.ToString(),
								   ((UserGroup)obj).MaintainUser.ToString(),
								   FormatHelper.ToDateString(((UserGroup)obj).MaintainDate),
								   FormatHelper.ToTimeString(((UserGroup)obj).MaintainTime) };
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	"�ͻ������",
									"�ͻ������",
									"�ͻ�������",
									"ά���û�",	
									"ά������",	
									"ά��ʱ��" };
		}

		#endregion

		private void gridWebGrid_ClickCellButton(object sender, Infragistics.WebUI.UltraWebGrid.CellEventArgs e)
		{
		    string url="./FSetCustPro.aspx?CustCode="+e.Cell.Row.Cells.FromKey("CGroupCode").Text.ToString();
			Response.Redirect(url,true);
		}
	}
}