using System;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.DataCollect;

namespace BenQGuru.eMES.WebQuery
{
	/// <summary>
	/// QDOTryItem ��ժҪ˵����
	/// </summary>
	public class QDOTryItem : OnWIPItem
	{
		[FieldMapAttribute("TryItemCode", typeof(string), 40, true)]
		public string TryItemCode;

		[FieldMapAttribute("snstate", typeof(string), 40, true)]
		public string SNState;
	}
}
