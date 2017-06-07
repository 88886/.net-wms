using System;
using System.Text;
using System.Collections;
using System.Runtime.Remoting;  
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.Domain;   
using BenQGuru.eMES.Common.DomainDataProvider;   
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.BaseSetting;

namespace BenQGuru.eMES.Security
{
	/// <summary>
	/// SecurityFacade ��ժҪ˵����
	/// �ļ���:		
	///			SecurityFacade.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		
	///			Jane Shu
	/// ��������:	
	///			2005/03/08
	/// �޸���:
	/// 
	/// �޸�����:
	///			
	/// �� ��:		
	///			Ȩ�޲���
	/// �� ��:	
	/// </summary>
	public class SecurityFacade:MarshalByRefObject
	{
		private IDomainDataProvider _domainDataProvider= null;
		private UserFacade _userFacade = null;
		private SystemSettingFacade _systemSettingFacade = null;


		private const char HasRightChar = '1';
		private const char NoRightChar  = '0';

		public SecurityFacade()
		{
			this._userFacade = new UserFacade( this.DataProvider );
			this._systemSettingFacade = new SystemSettingFacade( this.DataProvider );
		}

		//Laws Lu,max life time to unlimited
		public override object InitializeLifetimeService()
		{
			return null;
		}

		public SecurityFacade(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
			this._userFacade = new UserFacade( this._domainDataProvider );
			this._systemSettingFacade = new SystemSettingFacade( this._domainDataProvider );
		}

		public IDomainDataProvider DataProvider
		{
			get
			{
				if (_domainDataProvider == null)
				{
					this._domainDataProvider = DomainDataProviderManager.DomainDataProvider(); 
					this._userFacade = new UserFacade( this.DataProvider );
					this._systemSettingFacade = new SystemSettingFacade( this.DataProvider );
				}
				return _domainDataProvider;
			}	
		}

		public UserSecurityEntity LoadUserSecurityEntity(string userCode)
		{
			return new UserSecurityEntity( userCode,this.DataProvider);
		}

		#region ��¼���
		/// <summary>
		/// ** ��������:	 ��½��飬����Ϊ�����
		///						1���û�����
		///						2������ƥ��
		///						3���û��Ƿ��Ѿ����䵽һ���û���
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005/03/10
		/// ** �� ��:		sammer kong
		/// ** �� ��:		2005/03/14 change user to usercode
		/// </summary>
		/// <param name="userCode">�û���</param>
		/// <param name="password">����</param>
		/// <returns>ͨ��Check����true
		///	 û���û�����û�����룬�û������ڣ����벻ƥ�䣬�û��������κ��û��� ���׳��쳣
		/// </returns>
		public User LoginCheck( string userCode, string password ,out object[] groups)
		{
			User user = null;
			user = this.PasswordCheck(userCode,password);

			groups = this._userFacade.GetUserGroupofUser( userCode );
			if( groups == null || groups.Length == 0 )
			{
				ExceptionManager.Raise(this.GetType(), "$Error_User_Not_Belong_To_Any_User_Group");	
			}

			return user;
		}

		/// <summary>
		/// ** ��������:	 ��½��飬����Ϊ�����
		///						1���û�����
		///						2������ƥ��
		///						3���û��Ƿ��Ѿ����䵽һ���û���
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005/03/10
		/// ** �� ��:		sammer kong
		/// ** �� ��:		2005/03/14 change user to usercode
		/// </summary>
		/// <param name="userCode">�û���</param>
		/// <param name="password">����</param>
		/// <returns>ͨ��Check����true
		///	 û���û�����û�����룬�û������ڣ����벻ƥ�䣬�û��������κ��û��� ���׳��쳣
		/// </returns>
		public User LoginCheck( string userCode, string password)
		{
			User user = null;
			user = this.PasswordCheck(userCode,password);

			object[] groups = this._userFacade.GetUserGroupofUser( userCode );
			if( groups == null || groups.Length == 0 )
			{
				ExceptionManager.Raise(this.GetType(), "$Error_User_Not_Belong_To_Any_User_Group");	
			}

			return user;
		}

		public User PasswordCheck(string userCode,string password)
		{
			// û���û���
			if ( userCode == null || userCode == string.Empty )
			{
				ExceptionManager.Raise(this.GetType(), "$Error_User_Code_Empty");
	
			}

			// û������
			if ( password == null || password == string.Empty )
			{
				ExceptionManager.Raise(this.GetType(), "$Error_Password_Empty");	

			}
            
			// user existence	
			object obj = this._userFacade.GetUser( userCode.ToUpper() );
			if( obj == null || !( obj is User ) )
			{
				ExceptionManager.Raise(this.GetType(), "$Error_User_Not_Exist");
	
			}

			User user = obj as User;

			//password match            
			if ( user.UserPassword != EncryptionHelper.MD5Encryption(password.ToUpper()) )
			{
				ExceptionManager.Raise(this.GetType(), "$Error_Password_Not_Match");	

				
			}

			return user;
		}
		#endregion

		#region ���벿��
		public void ModifyPassword(string userCode,string oldPassword,string newPassword)
		{
			PasswordCheck(userCode,oldPassword);

			object obj = this._userFacade.GetUser( userCode.ToUpper() ) ;
			if( obj != null )
			{
				User user = obj as User;
				user.UserPassword = EncryptionHelper.MD5Encryption( newPassword.ToUpper() );

				this._userFacade.UpdateUser( user );
			}
		}

		public void AdminModifyPassword(string adminCode,string adminPassword,string userCode,string newPassword)
		{
			//admin password check
			PasswordCheck(adminCode,adminPassword);

			//user existence
			object obj = this._userFacade.GetUser( userCode.ToUpper() );
			if( obj == null )
			{
				ExceptionManager.Raise(this.GetType(), "$Error_User_Not_Exist");

				return ;
			}
			else
			{
				User user = obj as User;

				//admin-->admin group
				object[] userGroups = this._userFacade.GetUserGroupofUser( adminCode );
				if( userGroups == null || userGroups.Length == 0 )
				{
					ExceptionManager.Raise(this.GetType(), "$Error_Admin_Group_Granted");
	
					return;
				}
				else
				{
					bool isIn = false;
					foreach(UserGroup group in userGroups)
					{
						object objPara = this._systemSettingFacade.GetParameter(group.UserGroupType.ToUpper(),UserGroupType.UserGroupTypeName.ToUpper()) ;
						if( objPara != null )
						{
							Parameter para = objPara as Parameter;
							if( UserGroupType.Administrator.ToUpper() == para.ParameterValue.ToUpper() )
							{
								isIn = true;
								user.UserPassword = EncryptionHelper.MD5Encryption( newPassword.ToUpper() );

								this._userFacade.UpdateUser( user );
							}
							break;
						}
						else
						{
							ExceptionManager.Raise(this.GetType(),"$Error_UserGroupType_Lost");

							return;
						}
					}
					if( ! isIn )
					{
						ExceptionManager.Raise(this.GetType(),"$Error_UserGroup_NotAdmin");
					}
				}
			}
		}
		#endregion

		#region Ȩ�޵Ĵ洢/��ȡ
		/// <summary>
		/// ** ��������:	ƴ�ϲ���Ȩ�ޣ��ð�λ��ó�
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-03-10
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="viewValue1"></param>
		/// <param name="viewValue2"></param>
		/// <returns>ƴ�ϵĲ���Ȩ��</returns>
		public string MergeViewValue( string viewValue1, string viewValue2 )
		{			
			try
			{
				return (System.Int32.Parse(viewValue1) | System.Int32.Parse(viewValue2)).ToString();
			}
			catch
			{
				ExceptionManager.Raise(this.GetType(), "$Error_ViewValue_Format");

				return null;
			}
		}

		/// <summary>
		/// ** ��������:	��Ȩ��ƴ��ViewValue
		///						ViewValue�Ĵ洢��ʽ������������תΪʮ�������洢
		///							����λ��PrintȨ��
		///							����λ��ReadȨ��
		///							�ڶ�λ��WriteȨ��
		///							��һλ��DeleteȨ��	
		///							������5��Ϊ������101����ʾ��Read��DeleteȨ�ޣ�û��WriteȨ��
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-03-10
		/// ** �� ��:
		/// </summary>
		/// <param name="rights">Read/Write/Delete/Print</param>
		/// <returns>ViewValue</returns>
		public string SpellViewValueFromRights( bool[] rights )
		{
			int viewValue = 0;

			// Read * 2^2 + Write * 2^1 + Delete * 2^0
			for ( int i = 0; i < rights.Length; i++ )
			{
				if ( rights[ rights.Length - i - 1] )
				{
					viewValue += (int)System.Math.Pow(2, i);
				}
			}

			return viewValue.ToString();
		}

		public string GetViewValueOfUserByUrl( string userCode, string formUrl )
		{
			object[] objs = this._systemSettingFacade.GetFormRightsByUserAndUrl( userCode, formUrl );

			if ( objs == null || objs.Length == 0)
			{
				ExceptionManager.Raise(this.GetType(), "$Error_No_Access_Right", new Exception(formUrl));

				return "";
			}
		
			string viewValue = ((FormRight)objs[0]).ViewValue;

			for( int i=1; i<objs.Length; i++ )
			{
				viewValue = this.MergeViewValue( viewValue, ((FormRight)objs[i]).ViewValue );
			}

			return viewValue;
		}
		#endregion

		#region Ȩ���ж�
		/// <summary>
		/// ** ��������:	��ģ����������Ȩ��
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-03-10
		/// ** �� ��:
		/// </summary>
		/// <param name="ht">����Ȩ�޵�Hashtable</param>
		/// <param name="moduleCode">ģ�����</param>
		/// <returns>�з���Ȩ�޷���true��û��Ȩ�޷���false</returns>
		public bool CheckAccessRight( string userCode, string formUrl )
		{
			if ( HasRight( GetViewValueOfUserByUrl(userCode, formUrl), RightType.Access ) )
			{
				return true;
			}

			return false;
		}
		
		/// <summary>
		/// ** ��������:	��ViewValue�ж��Ƿ���Ȩ��
		///						ViewValue�Ĵ洢��ʽ������������תΪʮ�������洢
		///							����λ��ExportȨ��
		///							����λ��ReadȨ��
		///							�ڶ�λ��WriteȨ��
		///							��һλ��DeleteȨ��	
		///							������5��Ϊ������101����ʾ��Read��DeleteȨ�ޣ�û��WriteȨ��
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-03-10
		/// ** �� ��:		Jane Shu  2005-04-27
		/// </summary>
		/// <param name="viewValue">ViewValue</param>
		/// <param name="rightType">Ȩ�����ͣ�Read/Write/Delete/Export</param>
		/// <returns>��Ȩ�޷���true��û�з���false</returns>
		public bool HasRight( string viewValue, RightType rightType )
		{
			return HasRight( viewValue, rightType, true );
		}

		/// <summary>
		/// ** ��������:	��ViewValue�ж��Ƿ���Ȩ��
		///						ViewValue�Ĵ洢��ʽ������������תΪʮ�������洢
		///							����λ��ExportȨ��
		///							����λ��ReadȨ��
		///							�ڶ�λ��WriteȨ��
		///							��һλ��DeleteȨ��	
		///							������5��Ϊ������101����ʾ��Read��DeleteȨ�ޣ�û��WriteȨ��
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-03-10
		/// ** �� ��:		Jane Shu  2005-04-27
		/// </summary>
		/// <param name="viewValue">ViewValue</param>
		/// <param name="rightType">Ȩ�����ͣ�Read/Write/Delete/Export</param>
		/// <param name="merge">�Ƿ���Ȩ�޵�ƴ��</param>
		/// <returns>��Ȩ�޷���true��û�з���false</returns>
		public bool HasRight( string viewValue, RightType rightType, bool merge )
		{	
			if ( viewValue == null )
			{
				return false;
			}

			if ( rightType == RightType.Access )
			{
				return true;
			}

			int value = 0;
				
			try
			{
				value = System.Int32.Parse(viewValue);
			}
			catch
			{
				ExceptionManager.Raise(this.GetType(), "$Error_ViewValue_Format");

				return false;
			}

			if ( rightType == RightType.Export )
			{
				return (value & 8) == 8;
			}

			if ( rightType == RightType.Read )
			{
				if ( merge )
				{
					return (value & 4) == 4 
						|| (value & 2) == 2 // ��дȨ�ޱ�ʾ�ж�Ȩ��
						|| (value & 1) == 1;// ��ɾ��Ȩ�ޱ�ʾ�ж�Ȩ��
				}
				else
				{
					return (value & 4) == 4;
				}
			}

			if ( rightType == RightType.Write )
			{
				return (value & 2) == 2;
			}

			if ( rightType == RightType.Delete )
			{
				return (value & 1) == 1;
			}

			return true;
		}

		public bool IsBelongToAdminGroup( string userCode )
		{
			bool isBelongToAdminGroup = false ;
			object[] obj = this._userFacade.GetUserGroupofUser( userCode );
			if(obj!=null)
			{
				for(int i=0; i<obj.Length; i++)
				{
					if( (obj[i] as UserGroup).UserGroupType == "ADMIN" )
					{
						isBelongToAdminGroup = true;
						break;
					}
				}
				
			}

			return isBelongToAdminGroup ;
		}

		/// <summary>
		/// ** ��������:	�û����Ƿ���Resource��Ȩ��
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-05-30
		/// ** �� ��:		
		/// </summary>
		/// <param name="userGroupCode">ViewValue</param>
		/// <param name="resourceCode">Ȩ�����ͣ�Read/Write/Delete</param>
		public bool CheckResourceRight( string userCode, string resourceCode )
		{
			if(IsBelongToAdminGroup(userCode))
			{
				return true ;
			}

			if ( this._userFacade.GetUser2ResourceCount( userCode, resourceCode ) > 0 )
			{
				return true;
			}

			return false;
		}
		#endregion

		public string GetRelativeUrl(Uri uri)
		{
			string[] segments = uri.AbsolutePath.Split('/');

			return string.Join("/", segments, 2, segments.Length-2);
		}

	}	
	
	public enum RightType
	{
		Access, 
		Read, 
		Write, 
		Delete,
		Export
	}	
}
