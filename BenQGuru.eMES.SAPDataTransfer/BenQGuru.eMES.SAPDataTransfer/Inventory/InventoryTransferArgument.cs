using System;
using System.Collections.Generic;
using System.Text;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.BaseSetting;
using System.ComponentModel;

namespace BenQGuru.eMES.SAPDataTransfer
{
    public class InventoryTransferArgument
    {
        public InventoryTransferArgument(IDomainDataProvider domainDataProvider)
        {
            DBDateTime currentDateTime = FormatHelper.GetNowDBDateTime(domainDataProvider);

            this.m_TransactionCode = TransferFacade.InvertoryJobID + "_"
                + currentDateTime.DBDate.ToString() 
                + "_" + currentDateTime.DBTime.ToString() 
                + DateTime.Now.Millisecond.ToString(); 
        }

        public void GenerateNewTransactionCode(IDomainDataProvider domainDataProvider)
        {
            DBDateTime currentDateTime = FormatHelper.GetNowDBDateTime(domainDataProvider);

            this.m_TransactionCode = TransferFacade.InvertoryJobID + "_"
                + currentDateTime.DBDate.ToString() 
                + "_" + currentDateTime.DBTime.ToString() 
                + DateTime.Now.Millisecond.ToString(); 
        }

        private string[] m_OrgList;
        [Description("�����б�")]
        [DisplayName("�����б�")]
        [Category("���в���")]
        public string[] OrgList
        {
            get { return m_OrgList; }
            set { m_OrgList = value; }
        }

        private string m_MaterialNumber = string.Empty;
        [Description("���Ϻ�")]
        [DisplayName("���Ϻ�")]
        [Category("���в���")]
        public string MaterialNumber
        {
            get { return m_MaterialNumber; }
            set { m_MaterialNumber = value; }
        }

        private string[] m_Location;
        [Description("���ص�")]
        [DisplayName("���ص�")]
        [Category("���в���")]
        public string[] Location
        {
            get { return m_Location; }
            set { m_Location = value; }
        }


        private string m_TransactionCode = "";
        [Description("ÿ�η�������̶����͸����ϣ������޸�")]
        [Category("�̶����ݲ���")]
        [ReadOnly(true)]
        public string TransactionCode
        {
            get { return m_TransactionCode; }
        }	
        
    }
}
