using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.BaseSetting;

namespace BenQGuru.eMES.SAPDataTransfer
{
    public class DNTransferArgument
    {
        public DNTransferArgument(IDomainDataProvider dataProvider)
        {
            DBDateTime currentDateTime = FormatHelper.GetNowDBDateTime(dataProvider);
            DateTime wantedDateTime = FormatHelper.ToDateTime(currentDateTime.DBDate, currentDateTime.DBTime).AddDays(InternalVariables.MS_DateOffSet);

            this.m_MaintainDate_B = wantedDateTime.Date;
            this.m_MaintainDate_E = wantedDateTime.Date;

            this.m_TransactionCode = TransferFacade.DNTransferJobID + "_"
                + currentDateTime.DBDate.ToString() + "_" + currentDateTime.DBTime.ToString() + DateTime.Now.Millisecond.ToString();
        }

        public void GenerateNewTransactionCode(IDomainDataProvider dataProvider)
        {
            DBDateTime currentDateTime = FormatHelper.GetNowDBDateTime(dataProvider);
            this.m_TransactionCode = TransferFacade.DNTransferJobID + "_"
                + currentDateTime.DBDate.ToString() + "_" + currentDateTime.DBTime.ToString() + DateTime.Now.Millisecond.ToString();
        }

        private DateTime m_MaintainDate_B;
        [Description("��ʼ����")]
        [DisplayName("ά������--��ʼ����")]
        [Category("���в���--��ʽ1")]
        public DateTime MaintainDate_B
        {
            get { return m_MaintainDate_B; }
            set { m_MaintainDate_B = value; }
        }

        private DateTime m_MaintainDate_E;
        [Description("��������")]
        [DisplayName("ά������--��������")]
        [Category("���в���--��ʽ1")]
        public DateTime MaintainDate_E
        {
            get { return m_MaintainDate_E; }
            set { m_MaintainDate_E = value; }
        }

        private string m_TransactionCode = "";
        [Description("ÿ�η�������̶����͸����ϣ������޸�")]
        [Category("�̶����ݲ���")]
        [ReadOnly(true)]
        public string TransactionCode
        {
            get
            {
                return this.m_TransactionCode;
            }
        }

        private string[] m_OrgList;
        [Description("�����б�")]
        [DisplayName("�����б�")]
        [Category("���в���--��ʽ1")]
        public string[] OrgList
        {
            get { return m_OrgList; }
            set { m_OrgList = value; }
        }

        private string m_DNCode = string.Empty;
        [Description("��������")]
        [DisplayName("��������")]
        [Category("���в���--��ʽ2")]
        public string DNCode
        {
            get { return m_DNCode; }
            set { m_DNCode = value; }
        }
    }
}
