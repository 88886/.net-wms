using System;
using BenQGuru.eMES.Common.MutiLanguage;

namespace ControlLibrary.Web.Language
{
	/// <summary>
	/// ILanguageComponent ��ժҪ˵����
	/// </summary>
	public interface ILanguageComponent
	{
		string Language
		{
			get;
			set;
		}
		
		string LanguagePackageDir
		{
			get;
			set;
		}

		void  LoadLanguage();
		void  SaveLanguage();

		string GetString( string keyWord );
		LanguageWord GetLanguage( string keyWord );
	}
}
