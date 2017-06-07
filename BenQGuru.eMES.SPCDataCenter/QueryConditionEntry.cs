using System;

namespace BenQGuru.eMES.SPCDataCenter
{
	/// <summary>
	/// QueryConditionEntry ��ժҪ˵����
	/// </summary>
	public class QueryConditionEntry
	{
		public QueryConditionEntry()
		{
		}
		public QueryConditionEntry(int objectCount)
		{
			ObjectCode = new string[objectCount];
			TestCount = new int[objectCount];
			ObjectGroupCount = new int[objectCount];
			ColumnIndex = new int[objectCount][];
		}

		/// <summary>
		/// ����
		/// </summary>
		public string TableName;

		/// <summary>
		/// ���ƶ�������б�
		/// </summary>
		public string[] ObjectCode;

		/// <summary>
		/// ���ƶ�����Դ���
		/// </summary>
		public int[] TestCount;

		/// <summary>
		/// ÿ�����ƶ����������
		/// </summary>
		public int[] ObjectGroupCount;

		/// <summary>
		/// ÿ�����ƶ���ÿ�����ʼ��λ����һ�������ǿ��ƶ�����򣬵ڶ��������ǰ���ε�ÿ����ʼ��λ
		/// </summary>
		public int[][] ColumnIndex;
	}
}
