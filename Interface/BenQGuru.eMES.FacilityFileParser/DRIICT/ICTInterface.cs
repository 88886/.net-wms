using System;
using System.Collections;

using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.FacilityFileParser
{
	/// <summary>
	/// AOI DATA ��ժҪ˵����
	/// </summary>
	public class ICTData
	{
		public ICTData()
		{
		}

		/// <summary>
		/// ���Խ��
		/// </summary>
		[FieldMapAttribute("RESULT", typeof(string), 40, true)]
		public string  RESULT;

		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, true)]
		public string  RCARD;

		/// <summary>
		/// ��Դ���߲��ߴ���
		/// </summary>
		[FieldMapAttribute("RESOURCE", typeof(string), 40, true)]
		public string  RESOURCE;

		/// <summary>
		/// �û�
		/// </summary>
		[FieldMapAttribute("USER", typeof(string), 40, true)]
		public string USER;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public string MDATE;

		/// <summary>
		/// ʱ��
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 8, true)]
		public string MTIME;
//
//		/// <summary>
//		/// ��������ͳ��
//		/// </summary>
//		[FieldMapAttribute("ERRORCOUNT", typeof(int), 8, true)]
//		public string ERRORCOUNT;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("ERRORCODES", typeof(int), 1000, true)]
		public string ERRORCODES;
	}
}
