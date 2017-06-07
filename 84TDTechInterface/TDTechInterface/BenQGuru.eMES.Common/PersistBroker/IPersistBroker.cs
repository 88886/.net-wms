using System;
using System.Data; 
using BenQGuru.eMES.Common.MutiLanguage;
using System.Collections;


namespace BenQGuru.eMES.Common.PersistBroker
{
	/// <summary>
	/// IPersistBroker ��ժҪ˵����
	/// </summary>
	public interface IPersistBroker:IPersistBrokerTransaction, ILanguage
	{
		/// <summary>
		/// Laws Lu,2006/12/20 �޸�,֧���ֶ��ر�����
		/// �Ƿ� �Զ��ر�����
		/// </summary>
		bool AutoCloseConnection
		{
		get;
		set;
		}

		/// <summary>
		/// Laws Lu,2007/04/03 �Ƿ�Log�û�����
		/// </summary>
		bool AllowSQLLog
		{
			get;
			set;
		}

		/// <summary>
		/// Laws Lu,2007/04/03 Log���ݿ�
		/// </summary>
		string SQLLogConnectString
		{
			get;
			set;
		}

		/// <summary>
		/// Laws Lu,2007/04/03 �޸�,�����¼��ǰ�û�
		/// ��ȡ��������ִ���û�
		/// </summary>
		string ExecuteUser
		{
			get;
			set;
		}

		/// <summary>
		/// ִ��SQL������Ӱ������
		/// </summary>
		/// <param name="commandText">SQL���</param>
		/// <param name="parameters">�����б�</param>
		/// <param name="parameterTypes">���������б�</param>
		/// <param name="parameterValues">����ֵ�б�</param>
		/// <returns>Ӱ������</returns>
		int ExecuteWithReturn(string commandText, string[] parameters, Type[] parameterTypes, object[] parameterValues);
		/// <summary>
		/// ִ��SQL������Ӱ������
		/// </summary>
		/// <param name="commandText">SQL���</param>
		/// <returns>Ӱ������</returns>
		int ExecuteWithReturn(string commandText);
		/// <summary>
		/// ִ��SQL���
		/// </summary>
		/// <param name="commandText">SQL���</param>
		void Execute(string commandText);
		/// <summary>
		/// ִ��SQL���
		/// </summary>
		/// <param name="commandText">SQL���</param>
		/// <param name="parameters">�����б�</param>
		/// <param name="parameterTypes">���������б�</param>
		/// <param name="parameterValues">����ֵ�б�</param>
		void Execute(string commandText, string[] parameters, Type[] parameterTypes, object[] parameterValues);
		/// <summary>
		/// ִ��SQL ����ѯ������������DataSet
		/// </summary>
		/// <param name="commandText">SQL���</param>
		/// <returns>��ѯ�����DataSet��</returns>
		DataSet Query(string commandText);
		/// <summary>
		/// ִ��SQL ����ѯ������������DataSet
		/// </summary>
		/// <param name="commandText">SQL���</param>
		/// /// <param name="parameters">�����б�</param>
		/// <param name="parameterTypes">���������б�</param>
		/// <param name="parameterValues">����ֵ�б�</param>
		/// <returns>��ѯ�����DataSet��</returns>
		DataSet Query(string commandText, string[] parameters, Type[] parameterTypes, object[] parameterValues);

        /// <summary>
        /// ִ��Procedure
        /// </summary>
        /// <param name="commandText">Procedure����</param>
        /// <param name="parameters">�����б�</param>
        void ExecuteProcedure(string commandText, ref ArrayList parameters);


		/// <summary>
		/// �����ݿ�����
		/// </summary>
		void OpenConnection();
		/// <summary>
		/// �ر����ݿ�����
		/// </summary>
		void CloseConnection();
	}

	public interface IPersistBrokerTransaction
	{
		void BeginTransaction();
		void RollbackTransaction();
		void CommitTransaction();
		bool IsInTransaction
		{get;}
	}
}
