#region System
using System;
using System.Text;
using System.Runtime.Remoting;  
using System.Collections;
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Common.Helper;  
using BenQGuru.eMES.Common.DomainDataProvider;   
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Rework;
#endregion
 

namespace BenQGuru.eMES.SAPData
{
	/// <summary>
	/// SAPImportJob ִ�мƻ�
	/// </summary>
	public class SAPImportJob
	{
		private object[] sapItems;

		private int page_exclusive = 500;

		private SAPImportLoger importLogger; // д�ı���־,ÿ��ִ��Job���ᴴ��һ���ļ�

		public SAPImportJob()
		{
			importLogger = new SAPImportLoger();
		}

		public void ImportData()
		{
			this.Begin();		//��ʼ
			this.CheckData();	//���ݼ��
			this.Running();		//����
			this.Logging();		//д��־
			this.End();			//����
		}
		
		//��ʼ
		private void Begin()
		{
			System.Console.WriteLine("׼����SAPȡ����");
			importLogger.Write("׼����SAPȡ����");
			
		}

		//���ݼ��
		private bool CheckData()
		{
			System.Console.WriteLine("����ִ�����ݼ��");
			return false;
		}

		//����
		private void Running()
		{
			
			// �ȵ��빤��,�ٵ��빤��BOM,���BOM
			this.Import(DataName.SAPMO);		//���빤��							
			this.Import(DataName.SAPMOBom);		//���빤��bom
			this.Import(DataName.SAPBom);		//����BOM	

		}

		#region �������� ����ҳ����

		
		private void Import(DataName _dataName)
		{
			DateTime jobLogStartTime  = DateTime.Now;		//JobLog��־��ʼʱ��
			#region	��sap���ݿ��ȡ����
			//��sap���ݿ��ȡ����
			IDomainDataProvider _sapDataProvider = DomainDataProviderManager.DomainDataProvider(DBName.SAP);
			IDomainDataProvider _mesDataProvider = DomainDataProviderManager.DomainDataProvider(DBName.MES);

			if(_sapDataProvider!=null && ((SQLDomainDataProvider)_sapDataProvider).PersistBroker != null)
			{
				importLogger.Write(string.Format("�Ѿ����ӵ�SAP�����ݿ� {0}",_dataName));
				System.Console.WriteLine(string.Format("�Ѿ����ӵ�SAP�����ݿ� {0}",_dataName));
			}
			else
			{
				importLogger.Write(string.Format("���ӵ�SAP�����ݿ�ʧ�� {0}",_dataName));
				System.Console.WriteLine(string.Format("���ӵ�SAP�����ݿ�ʧ�� {0}",_dataName));
			}

			if(_mesDataProvider!=null && ((SQLDomainDataProvider)_mesDataProvider).PersistBroker != null)
			{
				importLogger.Write(string.Format("�Ѿ����ӵ�MES�����ݿ� {0}",_dataName));
				System.Console.WriteLine(string.Format("�Ѿ����ӵ�MES�����ݿ� {0}",_dataName));
			}
			else
			{
				importLogger.Write(string.Format("���ӵ�MES�����ݿ�ʧ�� {0}",_dataName));
				System.Console.WriteLine(string.Format("���ӵ�MES�����ݿ�ʧ�� {0}",_dataName));
			}

			importLogger.Write("");
			System.Console.WriteLine("");

			bool isImportSuccess = true; //�����Ƿ�ɹ�
			try
			{
				SAPDataGeter dateGetter = new SAPDataGeter(_sapDataProvider);
				int count = dateGetter.GetImportCount(_dataName);
				importLogger.Write(string.Format("��ȡ��{0}���� {1}��",_dataName,count.ToString()));
				
				if(count > 0)
				{
					int pageCount = (int)Math.Floor(Convert.ToDecimal(count/this.page_exclusive)) + 1; //��ȡ��ҳ��
					importLogger.Write(string.Format("��Ҫ�ֳ�{0}ҳ���е���,ÿҳ����{1}��",pageCount.ToString(),page_exclusive.ToString()));
					importLogger.Write(string.Format("���ڵ���{0}���� ",_dataName));
					System.Console.WriteLine(string.Format("���ڵ���{0}���� ",_dataName));
					int SucceedNum = 0;		//�ɹ����������
					for( int i = 1;i< pageCount+1;i++ )
					{
						if(i == 1)System.Console.WriteLine(string.Format("��Ҫ�ֳ�{0}ҳ���е���,ÿҳ����{1}��",pageCount.ToString(),page_exclusive.ToString()));
						SucceedNum += this.RunItemImportByPage(_dataName,_sapDataProvider,_mesDataProvider,i);
						System.Console.WriteLine(string.Format("���ڵ���{0}��{1}ҳ����",_dataName,i.ToString()));
					}
					importLogger.Write(string.Format("�ɹ�����{0}  {1}������ ",_dataName,SucceedNum));
					System.Console.WriteLine(string.Format("�ɹ�����{0}  {1}������ ",_dataName,SucceedNum));
					System.Console.WriteLine("");
				}

			}
			catch(Exception ex)
			{
				importLogger.Write(string.Format("����{0}���� ����,��ϸ��ϢΪ{1}",_dataName,ex.Message));
				isImportSuccess = false;
			}
			finally
			{
				if(_sapDataProvider!=null)((SQLDomainDataProvider)_sapDataProvider).PersistBroker.CloseConnection();
				if(_mesDataProvider!=null)((SQLDomainDataProvider)_mesDataProvider).PersistBroker.CloseConnection();
			}

			#endregion

			//#region дJobLog

			IDomainDataProvider _JobLogDataProvider = DomainDataProviderManager.DomainDataProvider(DBName.SAP);

			if(_JobLogDataProvider!=null && ((SQLDomainDataProvider)_JobLogDataProvider).PersistBroker != null)
			{
				importLogger.Write("�Ѿ����ӵ�JobLog�����ݿ�");
				System.Console.WriteLine("�Ѿ����ӵ�JobLog�����ݿ�");
			}
			else
			{
				importLogger.Write("���ӵ�JobLog�����ݿ�ʧ��");
				System.Console.WriteLine("���ӵ�JobLog�����ݿ�ʧ��");
			}

			try
			{
				DateTime jobLogEndTime  = DateTime.Now;			//JobLog��־����ʱ��
				importLogger.Write(string.Format("����д{0} JobLog",_dataName));
				SAPDataGeter JobLogWriter = new SAPDataGeter(_JobLogDataProvider);

				if(isImportSuccess)
				{
					JobLogWriter.AddSuccessJobLog(this.getJobName(_dataName),jobLogStartTime,jobLogEndTime);
				}
				else
				{
					JobLogWriter.AddFailedJobLog(this.getJobName(_dataName),jobLogStartTime,jobLogEndTime);
				}

				importLogger.Write(string.Format("д��{0} JobLog ���",_dataName));
				importLogger.Write("");
			}
			catch(Exception ex)
			{
				importLogger.Write(string.Format("д��{0} JobLog ʧ��,,��ϸ��ϢΪ{1}",_dataName,ex.Message));
			}
			finally
			{
				if(_JobLogDataProvider!=null)((SQLDomainDataProvider)_JobLogDataProvider).PersistBroker.CloseConnection();
			}

			//#endregion
		}

		private int RunItemImportByPage(DataName _dataName,IDomainDataProvider _sapDataProvider,IDomainDataProvider _mesDataProvider,int currentPage)
		{
			#region	��sap���ݿ��ȡ����
			//��sap���ݿ��ȡ����
			SAPMapper mapper = new SAPMapper();
			ArrayList mesItems  = new ArrayList();
			
			SAPDataGeter dateGetter = new SAPDataGeter(_sapDataProvider);
			int inclusive = this.page_exclusive * (currentPage-1) +1;
			int exclusive = this.page_exclusive * currentPage;
			sapItems = dateGetter.GetSAPData(_dataName,inclusive,exclusive);
			#endregion

			#region ӳ������
			//ӳ���Ʒ
			if(sapItems!=null && sapItems.Length>0)
			{
				mesItems = mapper.MapSAPData(this.sapItems);
			}
			#endregion

			#region ��������

			//���뵽MES���ݿ�
			SAPImporter sapImpoter = new SAPImporter(_mesDataProvider);
			sapImpoter.importLogger = this.importLogger;	//��־д����
			sapImpoter.pageCountNum = currentPage;			//����ĵ�ǰҳ
			sapImpoter.Import(mesItems);

			if(sapItems!=null && sapItems.Length>0 && sapItems[0].GetType() == typeof(SAPBOM))
			{
				//�����SAPBOM�����⴦����ѡ������
				sapImpoter.UpdateSBom(sapItems);
			}

			return sapImpoter.SucceedImportNum;
			#endregion
		}

		#endregion


		//д��־
		private void Logging()
		{
			System.Console.WriteLine("����д������־");
		}


		//����
		private void End()
		{
			System.Console.WriteLine("���ݴ�SAP�ɹ����뵽MES���ݿ�!");
			System.Console.WriteLine("�������");
			//System.Console.ReadLine();
		}

		public JobName getJobName(DataName _dataName)
		{
			if(_dataName == DataName.SAPMO)
			{
				return JobName.MO;
			}
			else if(_dataName == DataName.SAPMOBom)
			{
				return JobName.MOBom;
			}
			else if(_dataName == DataName.SAPBom)
			{
				return JobName.BOM;
			}

			return JobName.MO;
		}
	}

	public class LogMessage
	{
		public static string LogHead = string.Format("SAP���ݵ�����־ {0}",DateTime.Now.ToString());

		public static string BeginGetDataMsg = "׼����SAPȡ����";
		public static string SucessGetDataMsg = "�ɹ���ȡ������";
		public static string FailGetDataMsg = "��ȡ����ʧ��";

		public static string BeginImportDataMsg = "���ڵ�������";
		public static string SucessImportDataMsg = "�����������";
		public static string FailImportDataMsg= "��������ʧ��";

		public static string SucessMsg = "���ݴ�SAP�ɹ����뵽MES���ݿ�";
		public static string FailMsg = "���ݴ�SAP���뵽MES���ݿ�ʧ��";
		public static string EndMsg = "�������";
	}
}
