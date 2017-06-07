using System;

namespace BenQGuru.eMES.SPCDataCenter
{
	/// <summary>
	/// DataEntryTestData ��ժҪ˵����
	/// </summary>
	public class DataEntryTestData
	{
		public DataEntryTestData()
		{
		}

		private string _objectCode = string.Empty;
		/// <summary>
		/// ���Զ���
		/// </summary>
		public string ObjectCode
		{
			get { return _objectCode; }
			set { _objectCode = value; }
		}

		private decimal _groupSequence = 1;
		/// <summary>
		/// �������
		/// </summary>
		public decimal GroupSequence
		{
			get { return _groupSequence; }
			set { _groupSequence = value; }
		}

		private object _data = null;
		/// <summary>
		/// ��������(����������ݣ�����decimal�����������ݣ�����decimal[])
		/// </summary>
		public object Data
		{
			get { return _data; }
			set { _data = value; }
		}

		private int _columnIndex = -1;
		/// <summary>
		/// �������ݴ�ŵ���λ����
		/// </summary>
		internal int ColumnIndex
		{
			get { return _columnIndex; }
			set { _columnIndex = value; }
		}

	}
}
