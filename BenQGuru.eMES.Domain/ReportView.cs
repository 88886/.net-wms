using System;
using BenQGuru.eMES.Common.Domain;

/// <summary>
/// ** ��������:	DomainObject for ReportView
/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
/// ** �� ��:		2007-7-17 9:17:20
/// ** �� ��:
/// ** �� ��:
/// </summary>
namespace BenQGuru.eMES.Domain.ReportView
{

	#region RptViewChartCategory
	/// <summary>
	/// ���ݷ�����λ
	/// </summary>
	[Serializable, TableMap("TBLRPTVCHARTCATE", "RPTID,CHARTSEQ,CATESEQ")]
	public class RptViewChartCategory : DomainObject
	{
		public RptViewChartCategory()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHARTSEQ", typeof(decimal), 10, true)]
		public decimal  ChartSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ������λ
		/// </summary>
		[FieldMapAttribute("COLUMNNAME", typeof(string), 40, false)]
		public string  ColumnName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CATESEQ", typeof(decimal), 10, true)]
		public decimal  CategorySequence;

	}
	#endregion

	#region RptViewChartData
	/// <summary>
	/// ����ͳ����λ
	/// </summary>
	[Serializable, TableMap("TBLRPTVCHARTDATA", "RPTID,DATASEQ,CHARTSEQ")]
	public class RptViewChartData : DomainObject
	{
		public RptViewChartData()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASEQ", typeof(decimal), 10, true)]
		public decimal  DataSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ������λ
		/// </summary>
		[FieldMapAttribute("COLUMNNAME", typeof(string), 40, false)]
		public string  ColumnName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// ͳ�Ʒ�ʽ��Sum/Avg/Count
		/// </summary>
		[FieldMapAttribute("TOTALTYPE", typeof(string), 40, false)]
		public string  TotalType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHARTSEQ", typeof(decimal), 10, true)]
		public decimal  ChartSequence;

	}
	#endregion

	#region RptViewChartMain
	/// <summary>
	/// ������ʾ��λ
	/// </summary>
	[Serializable, TableMap("TBLRPTVCHARTMAIN", "RPTID,CHARTSEQ")]
	public class RptViewChartMain : DomainObject
	{
		public RptViewChartMain()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHARTSEQ", typeof(decimal), 10, true)]
		public decimal  ChartSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// ͼ������
		/// </summary>
		[FieldMapAttribute("CHARTTYPE", typeof(string), 40, false)]
		public string  ChartType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// �Ƿ���ʾͼ��
		/// </summary>
		[FieldMapAttribute("SHOWLEGEND", typeof(string), 1, true)]
		public string  ShowLegend;

		/// <summary>
		/// �Ƿ���ʾ�ڵ�
		/// </summary>
		[FieldMapAttribute("SHOWMARKER", typeof(string), 1, true)]
		public string  ShowMarker;

		/// <summary>
		/// �ڵ�����
		/// </summary>
		[FieldMapAttribute("MarkerType", typeof(string), 40, false)]
		public string  MarkerType;

		/// <summary>
		/// �Ƿ���ʾ��ǩ
		/// </summary>
		[FieldMapAttribute("SHOWLABEL", typeof(string), 1, true)]
		public string  ShowLabel;

		/// <summary>
		/// ��ǩ��ʾ��ʽ
		/// </summary>
		[FieldMapAttribute("LABELFORMATID", typeof(string), 40, false)]
		public string  LabelFormatID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHARTSUBTYPE", typeof(string), 40, false)]
		public string  ChartSubType;

	}
	#endregion

	#region RptViewChartSeries
	/// <summary>
	/// ϵ�з�����λ
	/// </summary>
	[Serializable, TableMap("TBLRPTVCHARTSER", "RPTID,CHARTSEQ,SERSEQ")]
	public class RptViewChartSeries : DomainObject
	{
		public RptViewChartSeries()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("CHARTSEQ", typeof(decimal), 10, true)]
		public decimal  ChartSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ������λ
		/// </summary>
		[FieldMapAttribute("COLUMNNAME", typeof(string), 40, false)]
		public string  ColumnName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SERSEQ", typeof(decimal), 10, true)]
		public decimal  SeriesSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

	}
	#endregion

	#region RptViewDataConnect
	/// <summary>
	/// ���ݿ�����
	/// </summary>
	[Serializable, TableMap("TBLRPTVCONNECT", "DATACONNECTID")]
	public class RptViewDataConnect : DomainObject
	{
		public RptViewDataConnect()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATACONNECTID", typeof(decimal), 10, true)]
		public decimal  DataConnectID;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("CONNECTNAME", typeof(string), 100, false)]
		public string  ConnectName;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// ���ݿ����ͣ�Oracle/SQLServer
		/// </summary>
		[FieldMapAttribute("SERVERTYPE", typeof(string), 40, false)]
		public string  ServerType;

		/// <summary>
		/// ���ݿ������(Oracle)/��������(SQLServer)
		/// </summary>
		[FieldMapAttribute("SERVICENAME", typeof(string), 40, false)]
		public string  ServiceName;

		/// <summary>
		/// �����û���
		/// </summary>
		[FieldMapAttribute("USERNAME", typeof(string), 40, false)]
		public string  UserName;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("PASSWORD", typeof(string), 100, false)]
		public string  Password;

		/// <summary>
		/// Ĭ�����ݿ���(����SQL Server��Ч)
		/// </summary>
		[FieldMapAttribute("DEFAULTDATABASE", typeof(string), 40, false)]
		public string  DefaultDatabase;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

	}
	#endregion

	#region RptViewDataFormat
	/// <summary>
	/// �ı���ʾ��ʽ
	/// </summary>
	[Serializable, TableMap("TBLRPTVDATAFMT", "FORMATID")]
	public class RptViewDataFormat : DomainObject
	{
		public RptViewDataFormat()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FORMATID", typeof(string), 40, true)]
		public string  FormatID;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("FONTFAMILY", typeof(string), 40, false)]
		public string  FontFamily;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// �����С
		/// </summary>
		[FieldMapAttribute("FONTSIZE", typeof(decimal), 10, true)]
		public decimal  FontSize;

		/// <summary>
		/// �������:Bold/Normal
		/// </summary>
		[FieldMapAttribute("FONTWEIGHT", typeof(string), 40, false)]
		public string  FontWeight;

		/// <summary>
		/// ����б��:Underline/Normal
		/// </summary>
		[FieldMapAttribute("TEXTDECORATION", typeof(string), 40, false)]
		public string  TextDecoration;

		/// <summary>
		/// ǰ��ɫ
		/// </summary>
		[FieldMapAttribute("COLOR", typeof(string), 40, false)]
		public string  Color;

		/// <summary>
		/// ����ɫ
		/// </summary>
		[FieldMapAttribute("BCOLOR", typeof(string), 40, false)]
		public string  BackgroundColor;

		/// <summary>
		/// ˮƽ���뷽ʽ
		/// </summary>
		[FieldMapAttribute("TextAlign", typeof(string), 40, false)]
		public string  TextAlign;

		/// <summary>
		/// �ݶ���
		/// </summary>
		[FieldMapAttribute("VerticalAlign", typeof(string), 40, false)]
		public string  VerticalAlign;

		/// <summary>
		/// �ı���ʾ��ʽ
		/// </summary>
		[FieldMapAttribute("TEXTFORMAT", typeof(string), 40, false)]
		public string  TextFormat;

		/// <summary>
		/// б�壺Normal/Italic
		/// </summary>
		[FieldMapAttribute("FONTSTYLE", typeof(string), 40, false)]
		public string  FontStyle;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("COLUMNWIDTH", typeof(decimal), 15, true)]
		public decimal  ColumnWidth;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("BORDERSTYLE", typeof(string), 40, false)]
		public string  BorderStyle;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("TEXTEXPRESS", typeof(string), 100, false)]
		public string  TextExpress;

	}
	#endregion

	#region RptViewDataSource
	/// <summary>
	/// ���ݲ�ѯ
	/// </summary>
	[Serializable, TableMap("TBLRPTVDATASRC", "DATASOURCEID")]
	public class RptViewDataSource : DomainObject
	{
		public RptViewDataSource()
		{
		}
 
		/// <summary>
		/// ����ԴID
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("NAME", typeof(string), 40, false)]
		public string  Name;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// ����Դ����:SQL/DLL
		/// </summary>
		[FieldMapAttribute("SOURCETYPE", typeof(string), 40, false)]
		public string  SourceType;

		/// <summary>
		/// SQL���
		/// </summary>
		[FieldMapAttribute("SQL", typeof(string), 100, false)]
		public string  SQL;

		/// <summary>
		/// DLL�ļ���
		/// </summary>
		[FieldMapAttribute("DLLFILENAME", typeof(string), 100, false)]
		public string  DllFileName;

		/// <summary>
		/// ���ݿ�����
		/// </summary>
		[FieldMapAttribute("DATACONNECTID", typeof(decimal), 10, true)]
		public decimal  DataConnectID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

	}
	#endregion

	#region RptViewDataSourceColumn
	/// <summary>
	/// ���ݲ�ѯ��λ����
	/// </summary>
	[Serializable, TableMap("TBLRPTVDATASRCCOLUMN", "DATASOURCEID,COLUMNSEQ,COLUMNNAME")]
	public class RptViewDataSourceColumn : DomainObject
	{
		public RptViewDataSourceColumn()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// ��λ����
		/// </summary>
		[FieldMapAttribute("COLUMNSEQ", typeof(decimal), 10, true)]
		public decimal  ColumnSequence;

		/// <summary>
		/// ��λ����
		/// </summary>
		[FieldMapAttribute("COLUMNNAME", typeof(string), 40, true)]
		public string  ColumnName;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// ��������(System.String/System.DateTime/System.Boolean)
		/// </summary>
		[FieldMapAttribute("DATATYPE", typeof(string), 40, false)]
		public string  DataType;

		/// <summary>
		/// �Ƿ�ɼ�
		/// </summary>
		[FieldMapAttribute("VISIBLE", typeof(string), 1, true)]
		public string  Visible;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

	}
	#endregion

	#region RptViewDataSourceParam
	/// <summary>
	/// ���ݲ�ѯ����(����DLL��Ч)
	/// </summary>
	[Serializable, TableMap("TBLRPTVDATASRCPARAM", "DATASOURCEID,PARAMSEQ")]
	public class RptViewDataSourceParam : DomainObject
	{
		public RptViewDataSourceParam()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PARAMSEQ", typeof(decimal), 10, true)]
		public decimal  ParameterSequence;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("PARAMNAME", typeof(string), 40, false)]
		public string  ParameterName;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// ��������(System.String/System.DateTime/System.Boolean)
		/// </summary>
		[FieldMapAttribute("DATATYPE", typeof(string), 40, false)]
		public string  DataType;

		/// <summary>
		/// Ĭ��ֵ
		/// </summary>
		[FieldMapAttribute("DEFAULTVALUE", typeof(string), 40, false)]
		public string  DefaultValue;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

	}
	#endregion

	#region RptViewDesignMain
	/// <summary>
	/// �Զ��屨������
	/// </summary>
	[Serializable, TableMap("TBLRPTVDESIGNMAIN", "RPTID")]
	public class RptViewDesignMain : DomainObject
	{
		public RptViewDesignMain()
		{
		}
 
		/// <summary>
		/// GUID
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// ��ʾ����
		/// </summary>
		[FieldMapAttribute("RPTNAME", typeof(string), 40, false)]
		public string  ReportName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// ����Դ
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// ��ʾ����:grid/chart/mix (�����������Ʊ���)
		/// </summary>
		[FieldMapAttribute("DISPLAYTYPE", typeof(string), 40, false)]
		public string  DisplayType;

		/// <summary>
		/// ������Դ:online/offline
		/// </summary>
		[FieldMapAttribute("RPTBUILDER", typeof(string), 40, false)]
		public string  ReportBuilder;

		/// <summary>
		/// ���ɵ��������ļ�
		/// </summary>
		[FieldMapAttribute("RPTFILENAME", typeof(string), 100, false)]
		public string  ReportFileName;

		/// <summary>
		/// ��������Ŀ¼
		/// </summary>
		[FieldMapAttribute("PRPTFOLDER", typeof(string), 40, false)]
		public string  ParentReportFolder;

		/// <summary>
		/// ״̬:initial(��ʼ���)/publish(�ѷ���)/redesign(�ٴ����)
		/// </summary>
		[FieldMapAttribute("STATUS", typeof(string), 40, false)]
		public string  Status;

		/// <summary>
		/// �����Ա
		/// </summary>
		[FieldMapAttribute("DESIGNUSER", typeof(string), 40, false)]
		public string  DesignUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DESIGNDATE", typeof(int), 8, true)]
		public int  DesignDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DESIGNTIME", typeof(int), 6, true)]
		public int  DesignTime;

		/// <summary>
		/// ������Ա
		/// </summary>
		[FieldMapAttribute("PUBLISHUSER", typeof(string), 40, false)]
		public string  PublishUser;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("PUBLISHDATE", typeof(int), 8, true)]
		public int  PublishDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PUBLISHTIME", typeof(int), 6, true)]
		public int  PublishTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

	}
	#endregion

	#region RptViewEntry
	/// <summary>
	/// ����ṹά��
	/// </summary>
	[Serializable, TableMap("TBLRPTVENTRY", "ENTRYCODE")]
	public class RptViewEntry : DomainObject
	{
		public RptViewEntry()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ENTRYCODE", typeof(string), 40, true)]
		public string  EntryCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ENTRYNAME", typeof(string), 40, false)]
		public string  EntryName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("PENTRYCODE", typeof(string), 40, false)]
		public string  ParentEntryCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// �Ƿ�ɼ�
		/// </summary>
		[FieldMapAttribute("VISIBLE", typeof(string), 1, true)]
		public string  Visible;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
		public decimal  Sequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("ENTRYTYPE", typeof(string), 40, false)]
		public string  EntryType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, false)]
		public string  ReportID;

	}
	#endregion

	#region RptViewExtendText
	/// <summary>
	/// ������ʾ��λ
	/// </summary>
	[Serializable, TableMap("TBLRPTVEXTTEXT", "RPTID,SEQ")]
	public class RptViewExtendText : DomainObject
	{
		public RptViewExtendText()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// �ı���ʾ��ʽ
		/// </summary>
		[FieldMapAttribute("LOCATION", typeof(string), 40, false)]
		public string  Location;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
		public decimal  Sequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FORMATID", typeof(string), 40, false)]
		public string  FormatID;

	}
	#endregion

	#region RptViewFileParameter
	/// <summary>
	/// �����ļ������б�
	/// </summary>
	[Serializable, TableMap("TBLRPTVFILEPARAM", "RPTID,SEQ")]
	public class RptViewFileParameter : DomainObject
	{
		public RptViewFileParameter()
		{
		}
 
		/// <summary>
		/// GUID
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// ��ʾ����
		/// </summary>
		[FieldMapAttribute("FILEPARAMNAME", typeof(string), 40, false)]
		public string  FileParameterName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATATYPE", typeof(string), 40, false)]
		public string  DataType;

		/// <summary>
		/// �Ƿ�������û�����
		/// </summary>
		[FieldMapAttribute("VIEWERINPUT", typeof(string), 1, true)]
		public string  ViewerInput;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DEFAULTVALUE", typeof(string), 40, false)]
		public string  DefaultValue;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
		public decimal  Sequence;

	}
	#endregion

	#region RptViewFilterUI
	/// <summary>
	/// ������λ����
	/// </summary>
	[Serializable, TableMap("TBLRPTVFILTERUI", "RPTID,SEQ")]
	public class RptViewFilterUI : DomainObject
	{
		public RptViewFilterUI()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// ������������:filter/fileparameter
		/// </summary>
		[FieldMapAttribute("INPUTTYPE", typeof(string), 40, false)]
		public string  InputType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
		public decimal  Sequence;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("INPUTNAME", typeof(string), 40, false)]
		public string  InputName;

		/// <summary>
		/// ����ֵ
		/// </summary>
		[FieldMapAttribute("UITYPE", typeof(string), 40, false)]
		public string  UIType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SQLFLTSEQ", typeof(decimal), 10, true)]
		public decimal  SqlFilterSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SELQTYPE", typeof(string), 40, false)]
		public string  SelectQueryType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LISTSRCTYPE", typeof(string), 40, false)]
		public string  ListDataSourceType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LISTSVAL", typeof(string), 100, false)]
		public string  ListStaticValue;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LISTDSRC", typeof(decimal), 10, true)]
		public decimal  ListDynamicDataSource;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LISTDTEXTCOL", typeof(string), 40, false)]
		public string  ListDynamicTextColumn;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("LISTDVALUECOL", typeof(string), 40, false)]
		public string  ListDynamicValueColumn;

        /// <summary>
        /// ��¼��ѡ���Ƿ��Ǳ�ѡ
        /// </summary>
        [FieldMapAttribute("CHECKEXIST", typeof(string), 1, false)]
        public string CheckExist;

	}
	#endregion

	#region RptViewGridColumn
	/// <summary>
	/// ������ʾ��λ
	/// </summary>
	[Serializable, TableMap("TBLRPTVGRIDCOLUMN", "RPTID,DISPLAYSEQ")]
	public class RptViewGridColumn : DomainObject
	{
		public RptViewGridColumn()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DISPLAYSEQ", typeof(decimal), 10, true)]
		public decimal  DisplaySequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("COLUMNNAME", typeof(string), 40, false)]
		public string  ColumnName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

	}
	#endregion

	#region RptViewGridDataFormat
	/// <summary>
	/// ������ʾ��λ
	/// </summary>
	[Serializable, TableMap("TBLRPTVGRIDDATAFMT", "RPTID,COLUMNNAME,STYLETYPE,GRPSEQ")]
	public class RptViewGridDataFormat : DomainObject
	{
		public RptViewGridDataFormat()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("COLUMNNAME", typeof(string), 40, true)]
		public string  ColumnName;

		/// <summary>
		/// ��ʽ��������:header/data/total
		/// </summary>
		[FieldMapAttribute("STYLETYPE", typeof(string), 40, true)]
		public string  StyleType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// �ı���ʾ��ʽ
		/// </summary>
		[FieldMapAttribute("FORMATID", typeof(string), 40, false)]
		public string  FormatID;

		/// <summary>
		/// �������ֻ�е�StyleType=subtotalʱ����Ч��
		/// </summary>
		[FieldMapAttribute("GRPSEQ", typeof(decimal), 10, true)]
		public decimal  GroupSequence;

	}
	#endregion

	#region RptViewGridDataStyle
	/// <summary>
	/// ������ʾ��λ
	/// </summary>
	[Serializable, TableMap("TBLRPTVGRIDDATASTYLE", "RPTID")]
	public class RptViewGridDataStyle : DomainObject
	{
		public RptViewGridDataStyle()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// �ı���ʾ��ʽ
		/// </summary>
		[FieldMapAttribute("STYLEID", typeof(decimal), 10, true)]
		public decimal  StyleID;

	}
	#endregion

	#region RptViewGridFilter
	/// <summary>
	/// ��������趨
	/// </summary>
	[Serializable, TableMap("TBLRPTVGRIDFLT", "RPTID,FLTSEQ")]
	public class RptViewGridFilter : DomainObject
	{
		public RptViewGridFilter()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("FLTSEQ", typeof(decimal), 10, true)]
		public decimal  FilterSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// ������λ
		/// </summary>
		[FieldMapAttribute("COLUMNNAME", typeof(string), 40, false)]
		public string  ColumnName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// �ȽϷ�ʽ:equal/greater/lesser/...
		/// </summary>
		[FieldMapAttribute("FLTOPERAT", typeof(string), 40, false)]
		public string  FilterOperation;

		/// <summary>
		/// ����Դ��������
		/// </summary>
		[FieldMapAttribute("PARAMNAME", typeof(string), 40, false)]
		public string  ParameterName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DEFAULTVALUE", typeof(string), 100, false)]
		public string  DefaultValue;

	}
	#endregion

	#region RptViewGridGroup
	/// <summary>
	/// ��������趨
	/// </summary>
	[Serializable, TableMap("TBLRPTVGRIDGRP", "RPTID,GRPSEQ")]
	public class RptViewGridGroup : DomainObject
	{
		public RptViewGridGroup()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("GRPSEQ", typeof(decimal), 10, true)]
		public decimal  GroupSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("COLUMNNAME", typeof(string), 40, false)]
		public string  ColumnName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

	}
	#endregion

	#region RptViewGridGroupTotal
	/// <summary>
	/// ��������趨
	/// </summary>
	[Serializable, TableMap("TBLRPTVGRIDGRPTOTAL", "RPTID,GRPSEQ,COLUMNNAME")]
	public class RptViewGridGroupTotal : DomainObject
	{
		public RptViewGridGroupTotal()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("GRPSEQ", typeof(decimal), 10, true)]
		public decimal  GroupSequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DATASOURCEID", typeof(decimal), 10, true)]
		public decimal  DataSourceID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("COLUMNNAME", typeof(string), 40, true)]
		public string  ColumnName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// ���ݻ��ܷ�ʽ:empty/sum/avg/count
		/// </summary>
		[FieldMapAttribute("TOTALTYPE", typeof(string), 40, false)]
		public string  TotalType;

	}
	#endregion

	#region RptViewReportSecurity
	/// <summary>
	/// ����Ȩ��
	/// </summary>
	[Serializable, TableMap("TBLRPTVRPTSECURITY", "RPTID,SEQ")]
	public class RptViewReportSecurity : DomainObject
	{
		public RptViewReportSecurity()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
		public decimal  Sequence;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("USERGROUPCODE", typeof(string), 40, false)]
		public string  UserGroupCode;

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("FUNCTIONGROUPCODE", typeof(string), 40, false)]
        public string FunctionGroupCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RIGHTACCESS", typeof(string), 40, false)]
		public string  RightAccess;

	}
	#endregion

	#region RptViewReportStyle
	/// <summary>
	/// ������ʽ
///
	/// </summary>
	[Serializable, TableMap("TBLRPTVSTYLE", "STYLEID")]
	public class RptViewReportStyle : DomainObject
	{
		public RptViewReportStyle()
		{
		}
 
		/// <summary>
		/// ����ԴID
		/// </summary>
		[FieldMapAttribute("STYLEID", typeof(decimal), 10, true)]
		public decimal  StyleID;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("NAME", typeof(string), 40, false)]
		public string  Name;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("DESCRIPTION", typeof(string), 100, false)]
		public string  Description;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

	}
	#endregion

	#region RptViewReportStyleDetail
	/// <summary>
	/// ������ʽ
///
	/// </summary>
	[Serializable, TableMap("TBLRPTVSTYLEDTL", "STYLEID,STYLETYPE")]
	public class RptViewReportStyleDetail : DomainObject
	{
		public RptViewReportStyleDetail()
		{
		}
 
		/// <summary>
		/// ����ԴID
		/// </summary>
		[FieldMapAttribute("STYLEID", typeof(decimal), 10, true)]
		public decimal  StyleID;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("STYLETYPE", typeof(string), 40, true)]
		public string  StyleType;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("FORMATID", typeof(string), 40, false)]
		public string  FormatID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, false)]
		public decimal  Sequence;

	}
	#endregion

	#region RptViewUserDefault
	/// <summary>
	/// �û�����
	/// </summary>
	[Serializable, TableMap("TBLRPTVUSERDFT", "USERCODE")]
	public class RptViewUserDefault : DomainObject
	{
		public RptViewUserDefault()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("USERCODE", typeof(string), 40, true)]
		public string  UserCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("DEFAULTRPTID", typeof(string), 40, false)]
		public string  DefaultReportID;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

	}
	#endregion

	#region RptViewUserSubscription
	/// <summary>
	/// �û�����
	/// </summary>
	[Serializable, TableMap("TBLRPTVUSERSUBSCR", "USERCODE,RPTID,SEQ")]
	public class RptViewUserSubscription : DomainObject
	{
		public RptViewUserSubscription()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("USERCODE", typeof(string), 40, true)]
		public string  UserCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RPTID", typeof(string), 40, true)]
		public string  ReportID;

		/// <summary>
		/// ������������:filter/fileparameter
		/// </summary>
		[FieldMapAttribute("INPUTTYPE", typeof(string), 40, false)]
		public string  InputType;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SEQ", typeof(decimal), 10, true)]
		public decimal  Sequence;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("INPUTNAME", typeof(string), 40, false)]
		public string  InputName;

		/// <summary>
		/// ����ֵ
		/// </summary>
		[FieldMapAttribute("INPUTVALUE", typeof(string), 40, false)]
		public string  InputValue;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MUSER", typeof(string), 40, false)]
		public string  MaintainUser;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MDATE", typeof(int), 8, true)]
		public int  MaintainDate;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MTIME", typeof(int), 6, true)]
		public int  MaintainTime;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("EAttribute1", typeof(string), 100, false)]
		public string  EAttribute1;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("SQLFLTSEQ", typeof(decimal), 10, true)]
		public decimal  SqlFilterSequence;

	}
	#endregion

    #region SelectQueryComplex
    /// <summary>
    /// ������λ����
    /// </summary>
    [Serializable, TableMap("", "")]
    public class SelectQuery : DomainObject
    {
        public SelectQuery()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute("Code", typeof(string), 100, false)]
        public string Code;

        /// <summary>
        /// ������������:filter/fileparameter
        /// </summary>
        [FieldMapAttribute("CodeDesc", typeof(string), 100, false)]
        public string CodeDesc;

    }
    #endregion

}

