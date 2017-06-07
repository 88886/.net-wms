using System;
using System.Text.RegularExpressions;

namespace BenQGuru.eMES.Common.Domain
{
	/// <summary>
	/// IDomainObjectQuery�ӿ�
	/// </summary>
	public interface IQueryCondition
	{
		Condition Insert(Condition condition, Type type);	
		Condition Remove(Condition condition, Type type);	
		Condition[] Conditions
		{
			get;
		}
	}
	/// <summary>
	/// SQL ����
	/// </summary>
	public enum SQLType
	{
		Text,
        Command,
        StoredProcedure
	}
    /// <summary>
    /// Direction ����
    /// </summary>
    public enum DirectionType
    {
        Input,
        InputOutput,
        Output,
        ReturnValue
    }
	/// <summary>
	/// ���ݿ�����
	/// </summary>
	public enum DBName
	{
		//DBName ��ʾ���ӵ���ͬ�����ݿ�
		//DBName.MES ��ʾ����MES�����ݿ�����		(Oracle)
		//DBName.SAP ��ʾ����SAP�����ݿ�����		(Oracle)
		//DBName.SPC ��ʾ����SPC�����ݿ�����		(SqlServer)
		//DBName.ERP ��ʾ����ERP�����ݿ�����		(Informix)
		MES,
		SAP,
		SPC,
		ERP,
		HIS
	}
	/// <summary>
	/// SQL ��ѯ����
	/// </summary>
	[Serializable]
	public class Condition
	{
		private SQLType _sqlType = SQLType.Text;
		private string _sql = string.Empty;

		public Condition()
		{
			_sqlType = SQLType.Text;
		}

		
		public Condition(SQLType sqlType)
		{
			_sqlType = sqlType;
		}

		/// <summary>
		/// SQL���
		/// </summary>
		public virtual string SQLText
		{
			get
			{
				return _sql;
			}
			set
			{
				_sql = value;
			}
		}

		/// <summary>
		/// SQL�������
		/// </summary>
		public SQLType SQLType
		{
			get
			{
				return _sqlType;
			}
		}
	}
	/// <summary>
	/// ����SQL����
	/// </summary>
	/// <example>
	/// int iCount = 
	///		this.DataProvider.GetCount(new SQLCondition("select count(*) from tblmo"));
	/// </example>
	[Serializable]
	public class SQLCondition : Condition
	{
		/// <summary>
		/// ����SQL����
		/// </summary>
		/// <param name="sql">SQL���</param>
		public SQLCondition(string sql) : base(SQLType.Text)
		{
			this.SQLText  = sql;
		}
	}
	/// <summary>
	/// SQL ��ѯ����
	/// </summary>
	/// <example>
	/// MO[] moList = 
	///		this.DataProvider.CustomQuery(
	///			typeof(MO), 
	///			new SQLParamCondition(
	///				"select * from tblmo where itemcode=$itemcode",
	///				new SQLParameter[]{
	///					new SQLParameter("itemcode", typeof(string), "ITEM1")
	///				}
	///			)
	///		);
	/// </example>
	[Serializable]
	public class SQLParameter
	{
		private string _name;
		private Type _type;
		private object _value;

		/// <summary>
		/// SQL��ѯ����
		/// </summary>
		/// <param name="name">��������</param>
		/// <param name="type">����</param>
		/// <param name="value">ֵ</param>
		public SQLParameter(string name, Type type, object value)
		{
			this._name = name;
			this._type = type;
			this._value = value;
		}

        /// <summary>
        /// ��������
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public Type Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        /// <summary>
        /// ����ֵ
        /// </summary>
        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
	}
	/// <summary>
	/// SQL ��ѯ�������������
	/// </summary>
	/// <example>
	/// MO[] moList = 
	///		this.DataProvider.CustomQuery(
	///			typeof(MO), 
	///			new SQLParamCondition(
	///				"select * from tblmo where itemcode=$itemcode",
	///				new SQLParameter[]{
	///					new SQLParameter("itemcode", typeof(string), "ITEM1")
	///				}
	///			)
	///		);
	/// </example>
	[Serializable]
	public class SQLParamCondition : Condition
	{	
		private SQLParameter[] _parameters;

		/// <summary>
		/// SQL ��ѯ�������������
		/// </summary>
		/// <param name="sql">��������SQL���</param>
		/// <param name="parameters">�����б�</param>
		public SQLParamCondition(string sql, SQLParameter[] parameters) : base(SQLType.Command)
		{
			this.SQLText = sql;
			this._parameters = parameters;
		}

		/// <summary>
		/// �����б�
		/// </summary>
		public SQLParameter[] Parameters
		{
			get
			{
				return this._parameters;
			}
		}
	}
	/// <summary>
	/// SQL ��ҳ�Ͳ������
	/// </summary>
	/// <example>
	/// MO[] moList = 
	///		this.DataProvider.CustomQuery(
	///			typeof(MO), 
	///			new PagerCondition("select * from tblmo", "mocode", 1, 50)
	///		);
	/// </example>
	[Serializable]
	public class PagerCondition : Condition
	{				
		/// <summary>
		/// SQL ��ҳ�Ͳ������
		/// </summary>
		/// <param name="sql">��ѯ���ݵ�SQL���</param>
		/// <param name="inclusive">��ʼ��¼��</param>
		/// <param name="exclusive">��ȡ����</param>
		/// <param name="group">������λ</param>
		public PagerCondition(string sql, int inclusive, int exclusive, bool group) : base(SQLType.Text)
		{
			this.BuildPagerSql( sql,  inclusive, exclusive, group );
		}	

		/// <summary>
		/// SQL ��ҳ�Ͳ������
		/// </summary>
		/// <param name="sql">��ѯ���ݵ�SQL���</param>
		/// <param name="orderbyFields">��Group By�Ӿ�ʱ��order byҪ��ȡ���Ľ�������ֶ���</param>
		/// <param name="inclusive">��ʼ��¼��</param>
		/// <param name="exclusive">��ȡ����</param>
		/// <param name="group">�Ƿ���û��Group By�Ӿ�</param>
		public PagerCondition(string sql, string orderbyFields, int inclusive, int exclusive, bool group) : base(SQLType.Text)
		{
			this.BuildPagerSql( sql, orderbyFields, inclusive, exclusive, group );
		}

		/// <summary>
		/// SQL ��ҳ�Ͳ������
		/// </summary>
		/// <param name="sql">��ѯ���ݵ�SQL���</param>
		/// <param name="inclusive">��ʼ��¼��</param>
		/// <param name="exclusive">��ȡ����</param>
		public PagerCondition(string sql, int inclusive, int exclusive) : this(sql, inclusive, exclusive, false)
		{
		}

		/// <summary>
		/// SQL ��ҳ�Ͳ������
		/// </summary>
		/// <param name="sql">��ѯ���ݵ�SQL���</param>
		/// <param name="orderbyFields">��Group By�Ӿ�ʱ��order byҪ��ȡ���Ľ�������ֶ���</param>
		/// <param name="inclusive">��ʼ��¼��</param>
		/// <param name="exclusive">��ȡ����</param>
		public PagerCondition(string sql, string orderbyFields, int inclusive, int exclusive) : this(sql, orderbyFields, inclusive, exclusive, false)
		{
		}

		private void BuildPagerSql( string sql, int inclusive, int exclusive, bool group )
		{
			if ( !group )
			{
				this.SQLText = sql.Trim().Remove(0, 6);

				if ( this.SQLText.Trim().StartsWith("*") )
				{
					//throw new Exception("*��ֹʹ�ã�");
					ExceptionManager.Raise(this.GetType(),"$Error_Forbidden_Asterisk");
				}

				this.SQLText = string.Format("select * from (select rownum as rid,{0}) where rid between {1} and {2}", 
					this.SQLText, 
					inclusive.ToString(), 
					exclusive.ToString());
			}
			else
			{
				this.SQLText = string.Format("select * from (select rownum as rid, result.* from ({0}) result) where rid between {1} and {2}", 
					sql, 
					inclusive.ToString(), 
					exclusive.ToString());
			}
		}
		/// <summary>
		/// �Զ�Build��ҳSQL���
		/// </summary>
		/// <param name="sql">����������SQL</param>
		/// <param name="orderbyFields">�����ֶ�</param>
		/// <param name="inclusive">��ʼ��¼��</param>
		/// <param name="exclusive">������¼��</param>
		/// <param name="group">����</param>
		private void BuildPagerSql( string sql, string orderbyFields, int inclusive, int exclusive, bool group )
		{
			string LeadingTable = GetLeadingSql(sql);//��ȡLeading �ı�����
			if ( !group )
			{
				this.SQLText = sql.Trim().Remove(0, 6);

				if (this.SQLText.Trim().StartsWith("*"))
				{
					//throw new Exception("*��ֹʹ�ã�");
					ExceptionManager.Raise(this.GetType(),"$Error_Forbidden_Asterisk");
				}

				this.SQLText = string.Format(@"select * from (select /*+ leading({4}) */  ROW_NUMBER() OVER (ORDER BY {0}) as rid,{1}) where rid between {2} and {3}", 
											orderbyFields,
											this.SQLText, 
											inclusive.ToString(), 
											exclusive.ToString(),
											LeadingTable);
			}
			else
			{
				this.SQLText = string.Format(@"select * from (select /*+ leading({4}) */  ROW_NUMBER() OVER (ORDER BY {0}) as rid, result.* from ({1}) result) where rid between {2} and {3}", 
											orderbyFields,
											sql, 
											inclusive.ToString(), 
											exclusive.ToString(),
											LeadingTable);
			}	
		}
		/// <summary>
		///��ȡLeading �ı�����
		///��Ӵ˷�����ԭ��	:	���ݱ���������Ӳ�ѯ���ִ���,����ָ�������ױ�  /*+ leading(tblitem) */
		///�������			:	ָ�����ӱ���
		///��ȡ����			:	��ȡ��һ��from����ı�����Ϊ�����ӱ�
		///ע��				:	�˷���ֻ�������������,��õ������������ӱ�����Ϊ���������
		///�޸�				:	�ж��Ƿ��ܹ�ȡ�ÿո��index
		///Add By Simone		2005/09/13
		/// </summary>
		/// <param name="sql">SQL</param>
		/// <returns>ת�����SQL</returns>
		private string GetLeadingSql(string sql)
		{
			string[] splitSTR = Regex.Split(sql,"from",RegexOptions.IgnoreCase);
			string afterFromStr = splitSTR[1].Trim();
			int index = afterFromStr.IndexOf(' ',0,afterFromStr.Length); 

			string LeadingtableName = afterFromStr;
			if(index >= 0)
			{
				LeadingtableName = afterFromStr.Substring(0,index);
			}

			return LeadingtableName;
		}
	}
	/// <summary>
	/// SQL ��������ҳ���
	/// </summary>
	public class PagerParamCondition : SQLParamCondition
	{	
		/// <summary>
		/// SQL ��������ҳ���
		/// </summary>
		/// <param name="sql">��ѯ���ݵ�SQL���</param>
		/// <param name="inclusive">��ʼ��¼��</param>
		/// <param name="exclusive">��ȡ����</param>
		/// <param name="parameters">�����б�</param>
		public PagerParamCondition(string sql, int inclusive, int exclusive, SQLParameter[] parameters) : base(sql, parameters)
		{
			this.SQLText = new PagerCondition( sql, inclusive, exclusive).SQLText;
		}

		/// <summary>
		/// SQL ��������ҳ���
		/// </summary>
		/// <param name="sql">��ѯ���ݵ�SQL���</param>
		/// <param name="orderbyFields">��Group By�Ӿ�ʱ��order byҪ��ȡ���Ľ�������ֶ���</param>
		/// <param name="inclusive">��ʼ��¼��</param>
		/// <param name="exclusive">��ȡ����</param>
		/// <param name="parameters">�����б�</param>
		public PagerParamCondition(string sql, string orderbyFields, int inclusive, int exclusive, SQLParameter[] parameters) : base(sql, parameters)
		{
			this.SQLText = new PagerCondition( sql, orderbyFields, inclusive, exclusive).SQLText;
		}

		/// <summary>
		/// SQL ��������ҳ���
		/// </summary>
		/// <param name="sql">��ѯ���ݵ�SQL���</param>
		/// <param name="inclusive">��ʼ��¼��</param>
		/// <param name="exclusive">��ȡ����</param>
		/// <param name="group">�Ƿ��з���</param>
		public PagerParamCondition(string sql, int inclusive, int exclusive, SQLParameter[] parameters, bool group) : base(sql, parameters)
		{
			this.SQLText = new PagerCondition( sql, inclusive, exclusive, group ).SQLText;
		}

		/// <summary>
		/// SQL ��������ҳ���
		/// </summary>
		/// <param name="sql">��ѯ���ݵ�SQL���</param>
		/// <param name="orderbyFields">��Group By�Ӿ�ʱ��order byҪ��ȡ���Ľ�������ֶ���</param>
		/// <param name="inclusive">��ʼ��¼��</param>
		/// <param name="exclusive">��ȡ����</param>
		/// <param name="parameters">�����б�</param>
		/// <param name="group">�Ƿ��з���</param>
		public PagerParamCondition(string sql, string orderbyFields, int inclusive, int exclusive, SQLParameter[] parameters, bool group) : base(sql, parameters)
		{
			this.SQLText = new PagerCondition( sql, orderbyFields, inclusive, exclusive, group ).SQLText;
		}
	}
	/// <summary>
	/// SQL ���ڷ�Χ������
	/// </summary>
	public class DateTimeRangeCondition : Condition
	{
		string _sql = string.Empty;

		/// <summary>
		/// SQL ���ڷ�Χ������
		/// </summary>
		/// <param name="dateField">�����ֶ�</param>
		/// <param name="timeField">ʱ���ֶ�</param>
		/// <param name="beginDate">��ʼ����</param>
		/// <param name="beginTime">��ʼʱ��</param>
		/// <param name="endDate">��������</param>
		/// <param name="endTime">����ʱ��</param>
		public DateTimeRangeCondition( string dateField, string timeField, int beginDate, int beginTime, int endDate, int endTime )
		{
			if ( beginDate != endDate )
			{
				_sql = string.Format( " ({0} between {1}+1 and {2}-1 or ( {0} = {1} and {3} >= {4} ) or ( {0} = {2} and {3} <= {5} ))",
					dateField, beginDate, endDate, timeField, beginTime, endTime  );			
			}
			else
			{
				_sql = string.Format(" ({0} = {1} AND {3} between {4} and {5} )", 
					dateField, beginDate, endDate, timeField, beginTime, endTime);
			}
		}

		/// <summary>
		/// ȡʱ�䷶ΧSQL
		/// select * from table where DateTimeRangeCondition().SQLText
		/// select * from table where ... and DateTimeRangeCondition().SQLText
		/// </summary>
		public override string SQLText
		{
			get
			{
				return this._sql;
			}
		}

	}

    /// <summary>
    /// Procedure ��ѯ����
    /// </summary>
    /// <example>
    /// ProcedureParameter[] paras=new ProcedureParameter[2];
    /// paras[0]=new ProcedureParameter("str",typeof(System.String),100,DirectionType.Input,"John");
    /// paras[1]=new ProcedureParameter("strout",typeof(System.String),100,DirectionType.Output,string.Empty);
    /// ProcedureCondition condition = new ProcedureCondition(proc_name, paras);
    /// IDomainDataProvider provider = DomainDataProviderManager.DomainDataProvider();
    /// provider.CustomProcedure(ref condition);
    /// </example>
    [Serializable]
    public class ProcedureParameter : SQLParameter
    {
        /// <summary>
        ///Procedure��ѯ����
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="type">����</param>
        /// <param name="value">ֵ</param>
        public ProcedureParameter(string name, Type type, int length, DirectionType direction, object value)
            : base(name, type, value)
        {
            this._length = length;
            this._direction = direction;
        }

        private DirectionType _direction;
        /// <summary>
        /// ��������
        /// </summary>
        public DirectionType Direction
        {
            get
            {
                return this._direction;
            }
            set
            {
                this._direction = value;
            }
        }

        private int _length;
        /// <summary>
        /// ����ֵ
        /// </summary>
        public int Length
        {
            get
            {
                return this._length;
            }
            set
            {
                this._length = value;
            }
        }
    }

    /// <summary>
    /// Procedure ��ѯ�������������
    /// </summary>
    /// <example>
    /// ProcedureParameter[] paras=new ProcedureParameter[2];
    /// paras[0]=new ProcedureParameter("str",typeof(System.String),100,DirectionType.Input,"John");
    /// paras[1]=new ProcedureParameter("strout",typeof(System.String),100,DirectionType.Output,string.Empty);
    /// ProcedureCondition condition = new ProcedureCondition(proc_name, paras);
    /// IDomainDataProvider provider = DomainDataProviderManager.DomainDataProvider();
    /// provider.CustomProcedure(ref condition);
    /// </example>
    [Serializable]
    public class ProcedureCondition : Condition
    {
        private ProcedureParameter[] _parameters;

        /// <summary>
        /// Procedure ��ѯ�������������
        /// </summary>
        /// <param name="sql">��������SQL���</param>
        /// <param name="parameters">�����б�</param>
        public ProcedureCondition(string Procedure, ProcedureParameter[] parameters)
            : base(SQLType.StoredProcedure)
        {
            this.SQLText = Procedure;
            this._parameters = parameters;
        }

        /// <summary>
        /// �����б�
        /// </summary>
        public ProcedureParameter[] Parameters
        {
            get
            {
                return this._parameters;
            }
            set
            {
                this._parameters = value;
            }
        }
    }
}
