using System;

namespace BenQGuru.eMES.Common.DCT.Core
{
	/// <summary>
	/// ICommandAction ��ժҪ˵����
	/// </summary>
	public interface IFactoryAction
	{
		BaseDCTAction CachedAction
		{
			get;
			set;
		}
	}
}
