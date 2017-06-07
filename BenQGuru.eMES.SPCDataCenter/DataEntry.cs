using System;
using System.Collections;

namespace BenQGuru.eMES.SPCDataCenter
{
	/// <summary>
	/// DataEntry ��ժҪ˵����
	/// </summary>
	public class DataEntry
	{
		public DataEntry()
		{
		}

		private string _modelCode = string.Empty;
		/// <summary>
		/// ���ִ���
		/// </summary>
		public string ModelCode
		{
			get { return _modelCode; }
			set { _modelCode = value; }
		}

		private string _itemCode = string.Empty;
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		public string ItemCode
		{
			get { return _itemCode; }
			set { _itemCode = value; }
		}

		private string _moCode = string.Empty;
		/// <summary>
		/// ��������
		/// </summary>
		public string MOCode
		{
			get { return _moCode; }
			set { _moCode = value; }
		}

		private string _runningCard = string.Empty;
		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		public string RunningCard
		{
			get { return _runningCard; }
			set { _runningCard = value; }
		}

		private decimal _runningCardSequence = 0;
		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		public decimal RunningCardSequence
		{
			get { return _runningCardSequence; }
			set { _runningCardSequence = value; }
		}

		private string _segmentCode = string.Empty;
		/// <summary>
		/// ����
		/// </summary>
		public string SegmentCode
		{
			get { return _segmentCode; }
			set { _segmentCode = value; }
		}

		private string _lineCode = string.Empty;
		/// <summary>
		/// ����
		/// </summary>
		public string LineCode
		{
			get { return _lineCode; }
			set { _lineCode = value; }
		}

		private string _resourceCode = string.Empty;
		/// <summary>
		/// ��Դ����
		/// </summary>
		public string ResourceCode
		{
			get { return _resourceCode; }
			set { _resourceCode = value; }
		}

		private string _opCode = string.Empty;
		/// <summary>
		/// �������
		/// </summary>
		public string OPCode
		{
			get { return _opCode; }
			set { _opCode = value; }
		}

		private string _lotNo = string.Empty;
		/// <summary>
		/// �ͼ�����
		/// </summary>
		public string LotNo
		{
			get { return _lotNo; }
			set { _lotNo = value; }
		}

		private string _testResult = string.Empty;
		/// <summary>
		/// ���Խ��
		/// </summary>
		public string TestResult
		{
			get { return _testResult; }
			set { _testResult = value; }
		}

		private string _machineTool = string.Empty;
		/// <summary>
		/// ���Թ���
		/// </summary>
		public string MachineTool
		{
			get { return _machineTool; }
			set { _machineTool = value; }
		}

		private string _testUser = string.Empty;
		/// <summary>
		/// ������Ա
		/// </summary>
		public string TestUser
		{
			get { return _testUser; }
			set { _testUser = value; }
		}

		private int _testDate = 0;
		/// <summary>
		/// ��������
		/// </summary>
		public int TestDate
		{
			get { return _testDate; }
			set { _testDate = value; }
		}

		private int _testTime = 0;
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public int TestTime
		{
			get { return _testTime; }
			set { _testTime = value; }
		}


		private ArrayList listTestData = new ArrayList();
		/// <summary>
		/// ���������б�
		/// </summary>
		public ArrayList ListTestData
		{
			get
			{
				return listTestData;
			}
		}

		/// <summary>
		/// ��Ӳ�������
		/// </summary>
		/// <param name="testData"></param>
		public void AddTestData(DataEntryTestData testData)
		{
			listTestData.Add(testData);
		}

		/// <summary>
		/// ��Ӳ�������
		/// </summary>
		public void AddTestData(string objectCode, object data)
		{
			DataEntryTestData testData = new DataEntryTestData();
			testData.ObjectCode = objectCode;
			testData.GroupSequence = 1;
			testData.Data = data;
			listTestData.Add(testData);
		}

		/// <summary>
		/// ��Ӳ�������
		/// </summary>
		public void AddTestData(string objectCode, decimal groupSequence, object data)
		{
			DataEntryTestData testData = new DataEntryTestData();
			testData.ObjectCode = objectCode;
			testData.GroupSequence = groupSequence;
			testData.Data = data;
			listTestData.Add(testData);
		}


	}


}
