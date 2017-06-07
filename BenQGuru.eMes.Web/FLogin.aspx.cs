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
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Security;

namespace BenQGuru.eMES.Web
{
	/// <summary>
	/// FMain ��ժҪ˵����
	/// </summary>
	public partial class FLogin : BenQGuru.eMES.Web.Helper.BasePage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.WebControls.Image imgDotNetLogo;

		private SecurityFacade _facade ;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!Page.IsPostBack)
			{
				this.InitPageLanguage(languageComponent1, false);
			}
//			this.InitUI();
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

		protected void cmdOK_ServerClick(object sender, System.EventArgs e)
		{
			SessionHelper sessionHelper = SessionHelper.Current(this.Session); 
			_facade = new SecurityFacade(base.DataProvider);
			try
			{
				// δ�����û���
				if ( this.txtUserCode.Text.Trim() == string.Empty )
				{
					ExceptionManager.Raise(this.GetType(), "$Error_User_Code_Empty");	
				}
				//�û������������5��
				if(this.loguser.Value != this.txtUserCode.Text.Trim() && this.loguser.Value != string.Empty )
				{
				this.logintimes.Value = "0";//��½�û�����һ�ε��û���ͬ�Ҳ��ǵ�һ�ε�½,������������������
				}
				if(this.loguser.Value == this.txtUserCode.Text.Trim() || this.loguser.Value == string.Empty )//��½�û���һ�λ��ߺ��ϴε�½����ͬ
				{
					this.loguser.Value = this.txtUserCode.Text.Trim();
					int logtimes = Convert.ToInt32(this.logintimes.Value);
					logtimes = logtimes + 1;
					this.logintimes.Value = logtimes.ToString();
					((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
					DataProvider.BeginTransaction();
					try
					{
						string login = "update tbluser set userstat ='L' where usercode ='"+txtUserCode.Text.Trim().ToUpper()+"'";
						if(logtimes > 5)
						{
							((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.Execute(login);//�������5�ν�userstat��ΪL,�����˻�
							DataProvider.CommitTransaction();
							this.logintimes.Value = "0";
							return;
						}
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
				// δ��������
				if ( this.txtPassword.Text.Trim() == string.Empty )
				{
					ExceptionManager.Raise(this.GetType(), "$Error_Password_Empty");	
				}
				
					BenQGuru.eMES.Domain.BaseSetting.User user = this._facade.LoginCheck(FormatHelper.CleanString(this.txtUserCode.Text.ToUpper()),FormatHelper.CleanString(this.txtPassword.Text.ToUpper()));

				// �û���������
				if ( user == null )
				{
					ExceptionManager.Raise(this.GetType(), "$Error_User_Not_Exist");	
				}
				//���û������������,�û�����,�����˻�	
				string userstat="select userstat from tbluser where usercode ='"+txtUserCode.Text.Trim().ToUpper()+"'";
				DataSet ds=((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.Query(userstat);
				if ( ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					if(ds.Tables[0].Rows[0][0].ToString() == "C")//�û�����
					{
						ExceptionManager.Raise(this.GetType(), "$Error_User_Confined");
						return;
					}
					else if (ds.Tables[0].Rows[0][0].ToString() == "L")//�����˻�
					{
						ExceptionManager.Raise(this.GetType(), "$Error_User_Locked");
						return;
					}
					else if (ds.Tables[0].Rows[0][0].ToString() == "N")//���û������������
					{
						ExceptionManager.Raise(this.GetType(), "$Error_User_New");
						return;
					}
				}
				
				sessionHelper.IsBelongToAdminGroup = this._facade.IsBelongToAdminGroup( this.txtUserCode.Text.ToUpper() );
				sessionHelper.UserName = user.UserName;
				sessionHelper.UserCode = user.UserCode;
				sessionHelper.UserMail = user.UserEmail;
				sessionHelper.Language = this.drpLanguage.Value;

//				//sammer kong 20050408 statisical for account of loggin user
//				if( sessionHelper.UserCode != null )
//				{
//					WebStatisical.Instance()["user"].Add( (sessionHelper.UserCode ) );
//				}
					
				this.Response.Redirect(this.MakeRedirectUrl("./FStartPage.aspx"),false);
				
			}
			catch(Exception ex)
			{
				this.lblMessage.Text = MessageCenter.ParserMessage( ex.Message, this.languageComponent1 );
			}
		}

	}
}
