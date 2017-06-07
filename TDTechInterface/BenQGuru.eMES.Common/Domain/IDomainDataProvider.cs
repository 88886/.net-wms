using System;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.MutiLanguage;
using System.Data;

namespace BenQGuru.eMES.Common.Domain
{
	public interface IDomainTransaction
	{
		/// <summary>
		/// ��ʼ����
		/// </summary>
		void BeginTransaction();
		/// <summary>
		/// �ع�����
		/// </summary>
		void RollbackTransaction();
		/// <summary>
		/// �ύ����
		/// </summary>
		void CommitTransaction();

		/// <summary>
		/// ����ӳ��ʵ��
		/// </summary>
		/// <param name="domainObject">ʵ��</param>
		void Insert(object domainObject);
		/// <summary>
		/// �Զ������ʵ��
		/// </summary>
		/// <param name="domainObject">ʵ��</param>
		/// <param name="attributes">ֵ</param>
		void CustomInsert(object domainObject, string[] attributes);
		/// <summary>
		/// �Զ������ʵ��
		/// </summary>
		/// <param name="type">ʵ��</param>
		/// <param name="attributes">�����б�</param>
		/// <param name="attributeValus">ֵ�б�</param>
		void CustomInsert(Type type, string[] attributes, object[] attributeValus);

		//
		/// <summary>
		/// ɾ��ʵ��
		/// </summary>
		/// <param name="domainObject">ʵ��</param>
		void Delete(object domainObject);
		void CustomDelete(object domainObject, Condition[] conditions);
		void CustomDelete(object domainObject, string[] attributes);

		void CustomDelete(Type type, object[] keyAttributeValus);
		void CustomDelete(Type type, string[] attributes, object[] attributeValus);

		void Update(object domainObject);
		void CustomUpdate(object domainObject, string[] attributes);
		void CustomUpdate(object domainObject, string[] attributes, object[] attributeValus);
	}

	public interface IDomainSearch
	{
		/// <summary>
		/// ��ѯ����
		/// </summary>
		/// <param name="type">����ʵ������</param>
		/// <param name="keyAttributeValus">�����ֶ�ֵ�б�</param>
		/// <returns>ʵ��ʵ��</returns>
		object CustomSearch(Type type, object[] keyAttributeValus);
		/// <summary>
		/// �Զ����ѯ
		/// </summary>
		/// <param name="type">����ʵ������</param>
		/// <param name="condition">��ѯ����</param>
		/// <returns>����ʵ���б�</returns>
		object[]  CustomSearch(Type type, Condition condition);
		/// <summary>
		/// �Զ����ѯ
		/// </summary>
		/// <param name="type">����ʵ������</param>
		/// <param name="attributes">���������б�</param>
		/// <param name="attributeValus">����ֵ�б�</param>
		/// <returns>����ʵ���б�</returns>
		object[]  CustomSearch(Type type,  string[] attributes, object[] attributeValus);

		/// <summary>
		/// ��ѯ��¼����
		/// </summary>
		/// <param name="type">ʵ������</param>
		/// <param name="condition">��ѯ����</param>
		/// <returns>��¼����</returns>
		int GetDomainObjectCount(Type type, Condition condition);
		/// <summary>
		/// ��ѯ��¼����
		/// </summary>
		/// <param name="type">ʵ������</param>
		/// <param name="attributes">���������б�</param>
		/// <param name="attributeValus">����ֵ�б�</param>
		/// <returns>��¼����</returns>
		int GetDomainObjectCount(Type type, string[] attributes, object[] attributeValus);
	}

	public interface IDomainQuery
	{
		/// <summary>
		/// ��ѯ����
		/// </summary>
		/// <param name="type">�������ݵ�ʵ������</param>
		/// <param name="condition">��ѯ����</param>
		/// <returns>���ض����б�</returns>
		/// <example>
		/// BenQGuru.eMES.Domain.MOModel.MO[] moList = 
		///		this.DataProvider.CustomQuery(
		///			typeof(BenQGuru.eMES.Domain.MOModel.MO),
		///			new SQLCondition("select * from tblmo")
		///		);
		///</example>
		object[]  CustomQuery(Type type, Condition condition);
		/// <summary>
		/// ��ѯ��¼����
		/// </summary>
		/// <param name="conditions">��ѯ������������Ҫ����count(*)�ĸ�ʽ���ؼ�¼��</param>
		/// <returns>��¼��</returns>
		/// <example>
		/// int iCount = 
		///		this.DataProvider.GetCount(new SQLCondition("select count(*) from tblmo"));
		/// </example>
		int GetCount(Condition conditions);
		/// <summary>
		/// ִ��SQL���
		/// </summary>
		/// <param name="conditions">SQL����</param>
		/// <example>
		/// this.DataProvider.CustomExecute(new SQLCondition("update tblmo set itemcode='ITEM1' where mocode='MO_TEST_1'"));
		/// </example>
		void CustomExecute(Condition conditions);
        /// <summary>
        /// ִ��Procedure
        /// </summary>
        /// <param name="conditions">Procedure����</param>
        /// <example>
        /// 
        /// </example>
        void CustomProcedure(ref ProcedureCondition condition);

        /// <summary>
        /// ��ȡ���в�ѯ�Ľ��,���еĲ�ѯSQL����Ҫ���������ʵ����
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string[] GetStringResult(Condition condition);
        DataSet QueryData(Condition condition);
	}


	/// <summary>
	/// IDomainObjectDataProvider ��ժҪ˵����
	/// </summary>
	public interface IDomainDataProvider:IDomainTransaction, IDomainSearch, IDomainQuery, ILanguage
	{	
	}
}
