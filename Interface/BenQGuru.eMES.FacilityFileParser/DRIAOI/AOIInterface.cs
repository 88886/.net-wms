using System;
using System.Collections;

using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.FacilityFileParser
{
	/// <summary>
	/// AOI DATA ��ժҪ˵����
	/// </summary>
	public class AOIData
	{
		public AOIData()
		{
		}

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("ITEMCODE", typeof(string), 40, true)]
		public string  ITEMCODE;

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
		/// ������
		/// </summary>
		[FieldMapAttribute("SHIP", typeof(string), 40, true)]
		public string  SHIP;

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

		/// <summary>
		/// ��������ͳ��
		/// </summary>
		[FieldMapAttribute("ERRORCOUNT", typeof(int), 8, true)]
		public string ERRORCOUNT;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("ERRORCODES", typeof(int), 1000, true)]
		public string ERRORCODES;
	}
}
