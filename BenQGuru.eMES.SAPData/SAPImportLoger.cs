using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace BenQGuru.eMES.SAPData
{
	/// <summary>
	/// SAPImportLoger ������־
	/// </summary>
	public class SAPImportLoger
	{
		private string fileName=string.Empty;	//�ļ���
		public SAPImportLoger()
		{
			this.CreateDefaultLogFile();
			this.Write(@"SAP ���ݵ��뵽 MES Log �ļ�");
		}

		//������־�ļ�(ÿʵ����һ�ζ��ᴴ��һ���µ�log�ļ�)
		//�ļ��������� : DateTime.Now.ToString();
		private void CreateDefaultLogFile()
		{
			fileName = DateTime.Now.ToString("yyMMdd-HHmmss");
			//File.Create(fileName);
		}

		//д��Ϣ���ļ���
		public void Write(string message)
		{
			if(fileName == string.Empty)return;

			string DirectoryName = @"SAPImportLog\";																		//log�����ļ���Name
			string domainPath =Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),DirectoryName);	//��ǰӦ�ó���·��
			domainPath = domainPath.Replace(@"bin\",string.Empty);						//����дlog·������binĿ¼�£�����BS�Զ�������
			Directory.CreateDirectory(domainPath);										//���·��������,�򴴽�
			string filePath = string.Format("{0}{1}-log.txt",domainPath,fileName);		//log�ļ�·��

			StreamWriter objWrite=File.AppendText(filePath);
			objWrite.WriteLine(message);
			objWrite.Close();
		
		}

	}
}
