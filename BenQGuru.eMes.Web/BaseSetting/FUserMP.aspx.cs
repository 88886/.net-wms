using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Security;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

using System.Collections.Generic;

namespace BenQGuru.eMES.Web.BaseSetting
{
	/// <summary>
	/// FUserMP ��ժҪ˵����
	/// </summary>
	public partial class FUserMP : BaseMPageNew
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		private BenQGuru.eMES.BaseSetting.UserFacade _facade = null ; //new SystemSettingFacadeFactory().CreateUserFacade();
		//private BenQGuru.eMES.Security.SecurityFacade _securityFacade = null ;//new BenQGuru.eMES.Security.SecurityFacade();
		//private BenQGuru.eMES.BaseSetting.SystemSettingFacade _systemFacade = null ;//new BenQGuru.eMES.BaseSetting.SystemSettingFacade();
	
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
			if (!IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				InitialData();
			}
            this.SelectableTextboxOrg.Readonly = false;
            this.SelectableTextboxOrg.CanKeyIn = false;
		}

	  	private void InitialData()
		{
			/*
			SystemParameterListBuilder builder = new SystemParameterListBuilder("department","paramcode",base.DataProvider);
			builder.Build( this.drpDepartmentEdit );
			*/

			InitDepartmentList();
		}

		private void InitDepartmentList()
		{
			this.drpDepartmentEdit.Items.Clear();
			BenQGuru.eMES.BaseSetting.SystemSettingFacade sysFacade = new BenQGuru.eMES.BaseSetting.SystemSettingFacade(this.DataProvider);
			ITreeObjectNode node = sysFacade.BuildParameterTree("DEPARTMENT");
			TreeObjectNodeSet nodeSet = node.GetSubLevelChildrenNodes();
			for (int i = 0; i < nodeSet.Count; i++)
			{
				AppendParentParameter((ITreeObjectNode)nodeSet[i], "");
			}
		}

		private void AppendParentParameter(ITreeObjectNode node, string prefix)
		{
			drpDepartmentEdit.Items.Add(new ListItem(prefix + node.Text, node.Text));
			TreeObjectNodeSet nodeSet = node.GetSubLevelChildrenNodes();
			for (int i = 0; i < nodeSet.Count; i++)
			{
				char nbsp = (char) 0xA0;
				AppendParentParameter((ITreeObjectNode)nodeSet[i], prefix + (new string(nbsp, 4)));
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
			//UserGridColumnBuilder builder = new UserGridColumnBuilder(this.gridWebGrid);
			//builder.Build();
            base.InitWebGrid();
//			this.gridWebGrid.Columns.FromKey("UserPassword").Hidden = true;
            this.gridHelper.AddColumn("UserCode", "�û�����", null);
            this.gridHelper.AddColumn("UserName", "�û���", null);
            this.gridHelper.AddColumn("UserTelephone", "�绰����", null);
            this.gridHelper.AddColumn("UserEmail", "��������", null);
            this.gridHelper.AddColumn("UserDepartment", "����", null);
            this.gridHelper.AddColumn("DefaultOrgDesc", "Ĭ����֯", null);
            this.gridHelper.AddColumn("UserStatus", "�û�״̬", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);
            //melo zheng �����2006.12.26 ����ҳ����ת,��ѯ��ǰ�û������û���
            this.gridHelper.AddLinkColumn("UserGroup", "�û���", null);


			this.gridHelper.AddDefaultColumn( true, true );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}
		
		protected override DataRow GetGridRow(object obj)
		{
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
            //    new object[]{"false",((UserEx)obj).UserCode.ToString(),
            //                    ((UserEx)obj).UserName.ToString(),
            //                    ((UserEx)obj).UserTelephone.ToString(),
            //                    ((UserEx)obj).UserEmail.ToString(),
            //                    ((UserEx)obj).UserDepartment.ToString(),
            //                    ((UserEx)obj).DefaultOrgDesc,			
            //                    //((UserEx)obj).MaintainUser.ToString(),
            //                 this.languageComponent1.GetString("UserStatus_"+((UserEx)obj).UserStatus),
            //                 ((UserEx)obj).GetDisplayText("MaintainUser"),
            //                    FormatHelper.ToDateString(((UserEx)obj).MaintainDate),
            //                    FormatHelper.ToTimeString(((UserEx)obj).MaintainTime),
            //                    ""});
            DataRow row = this.DtSource.NewRow();
            row["UserCode"] = ((UserEx)obj).UserCode.ToString();
            row["UserName"] = ((UserEx)obj).UserName.ToString();
            row["UserTelephone"] = ((UserEx)obj).UserTelephone.ToString();
            row["UserEmail"] = ((UserEx)obj).UserEmail.ToString();
            row["UserDepartment"] = ((UserEx)obj).UserDepartment.ToString();
            row["DefaultOrgDesc"] = ((UserEx)obj).DefaultOrgDesc;
            row["UserStatus"] = this.languageComponent1.GetString("UserStatus_" + ((UserEx)obj).UserStatus);
            row["MaintainUser"] = ((UserEx)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((UserEx)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((UserEx)obj).MaintainTime);
            return row;

		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null)
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
			}
			return this._facade.QueryUserEx( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserCodeQuery.Text)),
				inclusive, exclusive );
		}

		protected override int GetRowCount()
		{
			if(_facade==null)
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
			}
            return this._facade.QueryUserExCount( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserCodeQuery.Text)));
		}
	
		//melo zheng �����2006.12.26 ����ҳ����ת,��ѯ��ǰ�û������û���
        protected override void Grid_ClickCell(GridRecord row, string commandName)
		{
            if (commandName=="UserGroup")
            {
                this.Response.Redirect(this.MakeRedirectUrl("./FUserGroup2UserSP2.aspx", new string[] { "usercode" }, new string[] { row.Items.FindItemByKey("UserCode").Text.Trim() }));
            }
		}

		#endregion

		#region Button

		/// <summary>
		/// �����ˣ�
		/// �������ڣ�
		/// ��;��
		/// �޸��ˣ�   Angel Zhu
		/// �޸����ڣ� 2004/04/28
		/// �޸�ԭ�� ���û����������MD5���ܺ󣬴������ݿ�.
		/// </summary>
		/// <param name="domainObject"></param>
		protected override void AddDomainObject(object domainObject)
		{
            ((UserEx)domainObject).UserPassword = EncryptionHelper.MD5Encryption(((UserEx)domainObject).UserPassword.Trim().ToUpper());
			if(_facade==null)
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
			}
            this._facade.AddUserEx((UserEx)domainObject);
			//ά�������û�ʱ���û�״̬��ΪN����һ�ε�½ʱ��ʾ��������  joe
			((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
			DataProvider.BeginTransaction();
			try
			{
				string newuser = "update tbluser set userstat ='N' where usercode ='"+txtUserCodeEdit.Text.Trim().ToUpper()+"'";
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.Execute(newuser);
				DataProvider.CommitTransaction();				
			}
			catch
			{
				DataProvider.RollbackTransaction();
			}
			finally
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
			}
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null)
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
			}
            this._facade.DeleteUserEx((UserEx[])domainObjects.ToArray(typeof(UserEx)));
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			if(_facade==null)
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
			}
            this._facade.UpdateUserEx((UserEx)domainObject);
		}

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
            if (pageAction == PageActionType.Add)
            {
                this.txtUserCodeEdit.ReadOnly = false;
            }

            if (pageAction == PageActionType.Update)
            {
                this.txtUserCodeEdit.ReadOnly = true;
            }
            
            if(pageAction == PageActionType.Save)
			{
				this.txtUserCodeEdit.ReadOnly = false;
			}

            if (pageAction == PageActionType.Cancel)
            {
                this.txtUserCodeEdit.ReadOnly = false;
            }

		}

		#endregion

		#region Object <--> Page

		protected override object GetEditObject()
		{
			//this.ValidateInput();
			if(_facade==null)
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
			}
			UserEx userEx = this._facade.CreateNewUserEx();

            userEx.UserName = FormatHelper.CleanString(this.txtUserNameEdit.Text, 40);
            userEx.UserTelephone = FormatHelper.CleanString(this.txtUserTelephoneEdit.Text, 40);
            userEx.UserEmail = FormatHelper.CleanString(this.txtUserEmailEdit.Text, 100);
            userEx.UserDepartment = FormatHelper.CleanString(this.drpDepartmentEdit.SelectedValue, 40);
            userEx.UserCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserCodeEdit.Text, 40));
            userEx.MaintainUser = this.GetUserCode();

			if( this.cmdSave.Disabled == true )	//add
			{
                userEx.UserPassword = FormatHelper.CleanString(this.txtUserPasswordEdit.Text, 60);
			}
			else	//update
			{
                userEx.UserPassword = FormatHelper.CleanString(this.txtPasswordCache.Text);
			}

            
            userEx.user2OrgList = GetUser2OrgList(this.SelectableTextboxOrg.Tag, userEx.UserCode);

            return userEx;
		}


		protected override object GetEditObject(GridRecord row)
		{
			if(_facade==null)
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
			}
            object obj = _facade.GetUserEx(row.Items.FindItemByKey("UserCode").Text.ToString());
			
			if (obj != null)
			{
                return (UserEx)obj;
			}

			return null;
		}

		protected override void SetEditObject(object obj)
		{
			if (obj == null)
			{
				this.txtUserNameEdit.Text	= "";
				this.txtUserPasswordEdit.Text	= "";
				this.txtUserPasswordMatchEdit.Text	= "";
				this.txtUserTelephoneEdit.Text	= "";
				this.txtUserEmailEdit.Text	= "";
				this.drpDepartmentEdit.SelectedIndex = 0 ;
				this.txtUserCodeEdit.Text	= "";
				this.txtPasswordCache.Text = "";
				this.txtUserPasswordEdit.Enabled = true;
				this.txtUserPasswordMatchEdit.Enabled = true;
                this.SelectableTextboxOrg.Text = string.Empty;
                this.SelectableTextboxOrg.Tag = string.Empty;

                return;
			}

			this.txtUserNameEdit.Text	= ((UserEx)obj).UserName.ToString();
            this.txtUserPasswordEdit.Text = ((UserEx)obj).UserPassword.ToString();
			this.txtUserPasswordMatchEdit.Text	= this.txtUserPasswordEdit.Text;
            this.txtUserTelephoneEdit.Text = ((UserEx)obj).UserTelephone.ToString();
            this.txtUserEmailEdit.Text = ((UserEx)obj).UserEmail.ToString();
			try
			{
                this.drpDepartmentEdit.SelectedValue = ((UserEx)obj).UserDepartment.ToString();
			}
			catch
			{
				this.drpDepartmentEdit.SelectedIndex = 0;
			}
            this.txtUserCodeEdit.Text = ((UserEx)obj).UserCode.ToString();

            this.txtPasswordCache.Text = ((UserEx)obj).UserPassword.ToString();
			this.txtUserPasswordEdit.Enabled = false;
			this.txtUserPasswordMatchEdit.Enabled = false;

            this.SelectableTextboxOrg.Text = GetOrgDescListString(((UserEx)obj).UserCode);
            this.SelectableTextboxOrg.Tag = GetOrgIDListString(((UserEx)obj).user2OrgList);            
		}

		
		protected override bool ValidateInput()
		{
			PageCheckManager manager = new PageCheckManager();

			manager.Add( new LengthCheck(this.lblUserNameEdit, this.txtUserNameEdit, 40, false) );							
			manager.Add( new LengthCheck(this.lblUserTelephoneEdit, this.txtUserTelephoneEdit, 40, false) );
			manager.Add( new LengthCheck(this.lblUserEmailEdit, this.txtUserEmailEdit, 100, false) );
			manager.Add( new LengthCheck(this.lblUserDepartmentEdit, this.drpDepartmentEdit, 40, true) );
			manager.Add( new LengthCheck(this.lblUserSNEdit, this.txtUserCodeEdit, 40, true) );
            manager.Add(new LengthCheck(this.lblOrgEdit, this.SelectableTextboxOrg, 1000, true));

			if ( this.cmdSave.Disabled == true )
			{
				manager.Add( new LengthCheck(this.lblUserPasswordEdit, this.txtUserPasswordEdit, 40, true) );		
			}
			if ( this.cmdSave.Disabled == false )
			{
				//manager.Add( new LengthCheck(this.lblUserPasswordEdit, this.txtPasswordCache, 40, true) );		
			}

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
				return false;
			}
			else
			{
				if( this.txtUserPasswordMatchEdit.Text != this.txtUserPasswordEdit.Text )
				{
					WebInfoPublish.Publish(this, "$Error_Password_Not_Match", this.languageComponent1);
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Export

		protected override string[] FormatExportRecord( object obj )
		{
            return new string[]{ ((UserEx)obj).UserCode.ToString(),
								   ((UserEx)obj).UserName.ToString(),
								   ((UserEx)obj).UserTelephone.ToString(),
								   ((UserEx)obj).UserEmail.ToString(),
								   ((UserEx)obj).UserDepartment.ToString(),	
				                   ((UserEx)obj).DefaultOrgDesc.ToString(),				
                                   //((UserEx)obj).MaintainUser.ToString(),
                                 ((UserEx)obj).GetDisplayText("MaintainUser"),
								   FormatHelper.ToDateString(((UserEx)obj).MaintainDate),
								   FormatHelper.ToTimeString(((UserEx)obj).MaintainTime) };
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	"�û�����",
									"�û���",	
									"�绰����",	
									"��������",
									"����",	
                                    "Ĭ����֯",
									"ά���û�",	
									"ά������",	
									"ά��ʱ��" };
		}
		#endregion

		#region Confined,UnLock
		//joe Confined,UnLock user
		protected void cmdUnLock_ServerClick(object sender, System.EventArgs e)
		{
			((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
			DataProvider.BeginTransaction();
			try
			{
				string UnLock = "update tbluser set userstat ='O' where usercode ='"+txtUserCodeEdit.Text.Trim().ToUpper()+"'";
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.Execute(UnLock);
				DataProvider.CommitTransaction();
                this.gridHelper.RequestData();
                this.buttonHelper.PageActionStatusHandle(PageActionType.Query);
			}
			catch
			{
				DataProvider.RollbackTransaction();
			}
            finally
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }
		}

		protected void cmdConfined_ServerClick(object sender, System.EventArgs e)
		{
			((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
			DataProvider.BeginTransaction();
			try
			{
				string Confined = "update tbluser set userstat ='C' where usercode ='"+txtUserCodeEdit.Text.Trim().ToUpper()+"'";
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.Execute(Confined);
				DataProvider.CommitTransaction();
                this.gridHelper.RequestData();
                this.buttonHelper.PageActionStatusHandle(PageActionType.Query);
			}
			catch
			{
				DataProvider.RollbackTransaction();
			}
            finally
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }
		}
		#endregion

        private string GetOrgDescListString(string userCode)
        {
            string returnValue = string.Empty;

            BaseModelFacade baseModelFacade = new BaseModelFacadeFactory(base.DataProvider).Create();
            foreach (Organization org in baseModelFacade.GetAllOrgByUserCode(userCode))
            {
                returnValue += org.OrganizationDescription + ",";
            }
            if (returnValue.Length > 0) returnValue = returnValue.Substring(0, returnValue.Length - 1);

            return returnValue;
        }

        private string GetOrgIDListString(User2Org[] user2OrgList)
        {
            string returnValue = string.Empty;

            foreach (User2Org user2Org in user2OrgList)
            {
                returnValue += user2Org.OrganizationID.ToString() + ",";

                if (user2Org.IsDefaultOrg == 1) returnValue = "(" + user2Org.OrganizationID.ToString() + ")" + returnValue;
            }
            if (returnValue.Length > 0) returnValue = returnValue.Substring(0, returnValue.Length - 1);

            return returnValue;
        }

        private User2Org[] GetUser2OrgList(string orgIDList, string userCode)
        {
            orgIDList = orgIDList.Trim();

            int defaultOrgID = -1;
            if (orgIDList.Length > 0)
            {
                defaultOrgID = int.Parse(orgIDList.Substring(1, orgIDList.IndexOf(")") - 1).Trim());
                orgIDList = orgIDList.Substring(orgIDList.IndexOf(")") + 1).Trim();
            }

            string[] orgIDs = orgIDList.Split(new char[]{','});

            User2Org[] user2OrgList = null;
            if (orgIDs.Length > 0)
            {
                user2OrgList = new User2Org[orgIDs.Length];

                if (_facade == null)
                {
                    _facade = new SystemSettingFacadeFactory(base.DataProvider).CreateUserFacade();
                }

                for (int i = 0; i < orgIDs.Length; i++)
                {
                    User2Org user2Org = _facade.CreateNewUser2Org();
                    user2Org.OrganizationID = int.Parse(orgIDs[i].Trim());
                    user2Org.UserCode = userCode;
                    user2Org.IsDefaultOrg = 0;
                    if (defaultOrgID == user2Org.OrganizationID) user2Org.IsDefaultOrg = 1;
                    user2Org.MaintainUser = this.GetUserCode();

                    user2OrgList[i] = user2Org;
                }
            }

            return user2OrgList;
        }
       
	}
}
