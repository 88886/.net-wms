using System;

using BenQGuru.eMES.Common.Domain; 

namespace BenQGuru.eMES.Dashboard
{
	
	public class ModelQueryObject:DomainObject
	{
		[FieldMapAttribute("modelcode", typeof(string), 40,false)]
		public string  ModelCode;
		[FieldMapAttribute("itemcode", typeof(string), 40,false)]
		public string  ItemCode;
	}
	//��ӦXML��item����
	public struct ModelObject
	{
		public string ModelCode;

		public string ItemCode;

		public System.Collections.ArrayList Items;
	}
}
