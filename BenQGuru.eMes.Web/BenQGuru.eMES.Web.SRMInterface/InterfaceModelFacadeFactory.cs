using System;
using BenQGuru.eMES.Common.Domain ;


namespace BenQGuru.eMES.Web.SRMInterface
{
	/// <summary>
	/// BaseModelFacadeFactory ��ժҪ˵����
	/// </summary>
	public class InterfaceModelFacadeFactory
	{
		private IDomainDataProvider _dataProvider = null ;

		public InterfaceModelFacadeFactory(IDomainDataProvider dataProvider)
		{
			
			_dataProvider = dataProvider ;
		
		}

		public BenQGuru.eMES.SRMInterface.InterfaceModelFacade Create()
		{
            return new BenQGuru.eMES.SRMInterface.InterfaceModelFacade(_dataProvider);
		}
	}
}
