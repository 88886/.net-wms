using System;
using System.Data;
using System.Xml;

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common;

namespace BenQGuru.eMES.DataImportConsole
{
	/// <summary>
	/// Class1 ��ժҪ˵����
	/// </summary>
	class DataImportEngine
	{
		/// <summary>
		/// Ӧ�ó��������ڵ㡣
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			new DataImportHelper().Import();						
		}
	}
}
