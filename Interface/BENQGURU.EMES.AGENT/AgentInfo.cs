using System;

namespace BenQGuru.eMES.Agent
{
	/// <summary>
	/// AgentInfo ��ժҪ˵����
	/// </summary>
	public class AgentInfo
	{
		public AgentInfo()
		{
		}

		/// <summary>
		/// �Ƿ�����
		/// </summary>
		public static string RunStatus;

		/// <summary>
		/// Agent ģ��
		/// </summary>
		public static string Module;

		/// <summary>
		/// ���Ŀ¼
		/// </summary>
		public static string DirectoryPath;

		/// <summary>
		/// �ļ�����Ŀ¼
		/// </summary>
		public static string BackDirectoryPath
		{
			get
			{
				if(DirectoryPath != null && DirectoryPath != string.Empty)
				{
					return DirectoryPath + "_bak";
				}
				return string.Empty;
			}
		}

		/// <summary>
		/// Agent ѭ��ִ�еļ��
		/// </summary>
		public static int Interval = 1000;

		/// <summary>
		/// �ļ�����ĸ�ʽ
		/// </summary>
		public static string FileEncoding;

		// Added by Icyer 2006/08/03
		/// <summary>
		/// �Ƿ���ҪSMT����
		/// </summary>
		public static bool SMTLoadItem;

		// Added by Laws 2007/01/09
		/// <summary>
		/// �Ƿ������������
		/// </summary>
		public static bool AllowGoToMO;
		// Added end
	}
}
