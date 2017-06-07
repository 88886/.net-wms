using System;

namespace BenQGuru.eMES.Client.Service
{
	/// <summary>
	/// Laws Lu,2005/12/16,����Rcard���뷴��ƥ�乤��
	/// </summary>
	public abstract class clsRCard2Mo
	{
		private clsRCard2Mo()
		{
			
		}
		//Laws Lu��2005/12/16������	ƥ�乤��
		/// <summary>
		/// ��Ʒ���кŵڶ�λӦ�ú͹�����һλ��ͬ
		/// ��Ʒ���кŵ���~��λӦ�ú͹��������λ��ͬ
		/// </summary>
		/// <param name="rcardCode">��Ʒ���к�</param>
		/// <param name="moCode">����</param>
		/// <returns>True��ƥ��/False��ƥ��</returns>

		public static bool MatchMo(string rcardCode,string moCode) 
		{
			bool iReturn = false;
			if(rcardCode.Length >= 7 && moCode != String.Empty && moCode.Length >= 6)
			{
				string RcardSecondString = rcardCode.Substring(1,1);
				string RcardThird2SevenString = rcardCode.Substring(2,5);
				string MoFirstString = moCode.Substring(0,1);
				string MoLastFiveString = moCode = moCode.Substring(moCode.Length - 5,5);
				
				if(RcardSecondString == MoFirstString
					&& MoLastFiveString == RcardThird2SevenString)
				{
					iReturn = true;
				}
			}

			return iReturn;
		}
		
		/// <summary>
		/// ��������Ĳ�Ʒ���кź͹������ϣ�ѡ��ƥ��Ĺ���
		/// </summary>
		/// <param name="rcard">��Ʒ���к�</param>
		/// <param name="moCodes">����</param>
		/// <returns>����</returns>
		public static string GetMatchMo(string rcard,string[] moCodes)
		{
			string iReturnMoCode = String.Empty;
			foreach(string moCode in moCodes)
			{
				if(MatchMo(rcard.Trim().ToUpper(),moCode.Trim().ToUpper()))
				{
					iReturnMoCode = moCode;
					break;
				}
			}

			return iReturnMoCode;
		}
	}
}
