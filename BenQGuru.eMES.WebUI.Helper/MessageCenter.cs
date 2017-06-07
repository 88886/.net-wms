using System;
using System.Text.RegularExpressions;

using ControlLibrary.Web.Language;

namespace BenQGuru.eMES.Web.Helper
{
	/// <summary>
	/// MessageCenter ��ժҪ˵����
	/// </summary>
	public class MessageCenter
	{
		public MessageCenter()
		{
		}

		private static ILanguageComponent _languageComponent = null;
		/// <summary>
		/// ** ��������:	ʵ����Ϣ�Ķ�����,����'$'��ͷ���� ��ĸ�����ּ�'_' ��ɵ��ַ����滻��languageComponent���ҵ����ַ���
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2004/05/16
		/// ** �� ��:
		/// ** �� ��:
		/// ** �汾:
		/// </summary>
		/// <param name="originMsg">ԭʼ��Ϣ</param>
		/// <param name="language"></param>
		/// <returns></returns>
		public static string ParserMessage(string originMsg, ILanguageComponent languageComponent )
		{
			if ( originMsg == string.Empty )
			{
				return string.Empty;
			}

			_languageComponent = languageComponent;

			return new Regex(@"\$([A-Za-z0-9_]+)").Replace( originMsg, new MatchEvaluator( replaceErrorCode ) );
		}

		private static string replaceErrorCode( Match match )
		{
			return _languageComponent.GetString( match.Value );
		}
	}
}
