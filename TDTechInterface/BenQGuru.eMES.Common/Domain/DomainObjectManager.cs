using System;

namespace BenQGuru.eMES.Common.Domain
{
	/// <summary>
	/// DomainObjectManager ��ժҪ˵����
	/// </summary>
	public class DomainObjectManager
	{
		public DomainObjectManager()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		public static object CreateDomainObject(Type type)
		{
			return type.Assembly.CreateInstance(type.FullName); 
		}

		public static object CreateDomainObject ( Type type , System.Boolean ignoreCase)
		{
			return type.Assembly.CreateInstance(type.FullName, ignoreCase); 
		}

		public static object CreateDomainObject(Type type , System.Boolean ignoreCase , System.Reflection.BindingFlags bindingAttr , System.Reflection.Binder binder , object[] args , System.Globalization.CultureInfo culture , object[] activationAttributes)
		{
			return type.Assembly.CreateInstance( type.FullName,  ignoreCase,  bindingAttr, binder, args, culture, activationAttributes);
		}
	}
}
