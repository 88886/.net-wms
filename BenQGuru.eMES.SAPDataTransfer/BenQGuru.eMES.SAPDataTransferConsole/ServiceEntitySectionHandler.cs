using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;

namespace BenQGuru.eMES.SAPDataTransferConsole
{
    public class ServiceEntitySectionHandler : IConfigurationSectionHandler
    {
        public ServiceEntitySectionHandler()
        {

        }

        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            List<ServiceEntity> serviceEntityes = new List<ServiceEntity>();
            try
            {
                XmlElement element = null;
                element = (XmlElement)section;
                serviceEntityes = ServiceEntity.ServiceEntities(element);
            }
            catch
            {
                throw new System.Configuration.ConfigurationErrorsException("Invalid service entity configuration");
            }
            return serviceEntityes;
        }

        #endregion
    }
}