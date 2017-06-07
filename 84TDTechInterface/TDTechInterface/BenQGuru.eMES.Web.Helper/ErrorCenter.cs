using System;
using System.Collections;
using BenQGuru.eMES.Common.MutiLanguage;


namespace BenQGuru.eMES.Web.Helper
{
	public class ErrorCenter
	{
		
		private static Hashtable _hashtable = null;

		//this is  for module BOMFileParser
		#region this is for BOMFileParser
		//key
		public static string ERROR_UPLOAD_UPPERLIMIT = "error_upload_upperlimit";
		public static string SYSTEM_ERROR = "system_error";
		public static string FILE_COLUMN_ERROR = "error_file_column";
		public static string NOT_EMPTY_ERROR = "error_not_empty";
		public static string EFFECTDATE_ERROR = "error_effectdate_error";
		public static string INEFFECTDATE_ERROR = "error_ineffectdate_error";
		public static string ERROR_CREATENEWREWORKMOCODE="�����ع�������ʧ�ܣ�";

		//value
		private static string bomfileparser_error_upload_upperlimit = "�ļ����󳬹�1M!";
		private static string bomfileparser_system_error ="ϵͳ���󣬽����ļ�ʧ�ܣ�";
		private static string bomfileparser_error_file_column = "�����ļ�������ڵ���8�У�";
		private static string bomfileparser_error_not_empty ="�в���Ϊ�գ�";
		private static string bomfileparser_error_effectdate_error ="ʧЧ���ڱ��������Ч���ڣ�";
		private static string bomfileparser_error_ineffectdate_error ="ʧЧ���ڱ�����ڽ��죡";
	
		#endregion

		#region this is for model
		//key
		public static string ADDMODEL_ERROR = "";
		#endregion

		#region this is for ModelFacade
		/// <summary>
		/// crystal chu 20050411
		/// </summary>
		public static string ERROR_ASSIGNITEMSTOMODEL="assign items to Model {0} failure!";
		public static string ERROR_REMOVEITEMSFROMMODEL ="remove items from Model {0} failure!";
		public static string ERROR_ASSGINROUTEALTSTOMODEL ="assign routealts to model {0} failure!";
		public static string ERROR_REMOVEROUTEALTSTOMODEL = "remove routealts from Model {0} failure!";
		public static string ERROR_ADDMODELROUTE =" Add model2Route error, modelcode ='{0}' and routecode='{1}'";
		public static string ERROR_MODELROUTEUSED =" The Model2Route modelcode='{0}',routecode='{1}' has used! ";
		public static string ERROR_DELETEMODELROUTE=" Delete model2route failure,the model2route,modelcode='{0}',routecode='{1}'";
		public static string ERROR_DELETEMODELROUTES=" Delete model2routes failure!";
		public static string ERROR_UPDATEMODEL2OPERATION=" update model2operation failure!modelcode='{0}',opcode='{1}'";

		#endregion

		#region this is for MOFacade
		public static string ERROR_DOWNLOADMO =" dowload mos error!";
		public static string ERROR_MOSTATUS =" the MO status error!the status must be {0}";
		public static string ERROR_DELETEMO =" Delete MO error! mocode='{0}'";
		public static string ERROR_DELETEMOS =" Delete MOs error!";
		public static string ERROR_UPDATEMO =" Update MO error! mocode='{0}'";
		public static string ERROR_MOSTATUSCHANGED = "The MO mocode='{0}', can not turn status into '{1}'!";
		public static string ERROR_GETMONORMALROUTEBYMOCODE = " The MO mocode='{0}' has  no normal route!";
		#endregion

		#region this is for opBOMFacade
		public static string ERROR_OPBOMUSED =" The opBOM has been used! opBOMCode ='{0}'";
		public static string ERROR_BOMCOMPONENTLOADINGUSED =" The opBOM opBOMCode ='{0}' has maintianed the component loading information can not delete!";
		public static string ERROR_DELETEOPBOM ="Delete opBOM opBOMCode='{0}' failure!";
		public static string ERROR_ASSIGNBOMITEMTOOPERATION =" Assign bom item to operation opcode='{0}' failure!";
		public static string ERROR_OPBOMITEMCONTROL="opBOMitem itemcode='{0}' control information has existed!";
		public static string ERROR_DELETEOPBOMITEM =" Delete opBOMitem  itemcode='{0}' failure!";
		#endregion

		#region this is for opBOMItemControlFacade
		public static string ERROR_ADDOPITEMCONTROL="Add OPBOMItem itemcode='{0}' failure!";
		public static string ERROR_DELETEITEMCONTROL =" Delete item itemcode='{0}''s itemcontrol failure!";
		#endregion

		#region this is for rework
		public static string ERROR_REWORKSTATUS = "�ع����󵥵�״̬����Ϊ{0}";
		public static string ERROR_APPROVESTATUS = "ǩ��״̬����Ϊ{0}";
		public static string ERROR_APPROVER ="δ�����û�{0}ǩ��";
		public static string ERROR_PASSREWORKAPPROVE="ǩ��ʧ��";
		#endregion

		#region TS

		#endregion
		
		/// <summary>
		/// sammer kong 20050308 for xml config class
		/// </summary>
		public static string ERROR_ARGUMENT_NULL = "{0} is null!Please check!";
		public static string ERROR_FILE_LOST = "The file {0} is lost!";
		public static string ERROR_CONFIG = "Please check {0}! The configuration is error!";
		public static string ERROR_TYPE_CONVERTOR = "Please check type {0}!";
	
		public static string ERROR_USERGROUPNOTEXIST = "ERROR_USERGROUPNOTEXIST";
		/// <summary>
		/// Jane Shu 2005/03/09 for database error
		/// </summary>
		public static string ERROR_PKOVERLAP = "�����ظ�";
		public static string ERROR_NOTEXIST  = "��¼������";
		public static string ERROR_ADD		 = "������¼����";
		public static string ERROR_UPDATE	 = "���¼�¼����";	
		public static string ERROR_DELETE	 = "ɾ����¼����";
		public static string ERROR_ADDCHECK  = "������¼������";
		public static string ERROR_UPDATECHECK = "���¼�¼������";
		public static string ERROR_DELETECHECK = "ɾ����¼������";
		public static string ERROR_ASSOCIATEEXIST = "��{0}���ڹ�����¼";
		public static string ERROR_PASSWORMATCH = "���벻ƥ��";

		/// <summary>
		/// Jane Shu 2005/03/09 for user input check
		/// </summary>
		public static string ERROR_WITHOUTINPUT = "{0}ȱ������";
		public static string ERROR_FORMAT		= "{0}�����ʽ����";
		public static string ERROR_TOLONG		= "{0}�����ַ���������ӦС��{1}";
		public static string ERROR_NUMNERTOLONG	= "{0}������Чֵ��Χ��ӦС��{1}";
		public static string ERROR_PARENTISCHILDREN = "{0}��������������ӽڵ�";

		#region Login & Rights Check
		public static string ERROR_NOUSERCODE				= "ȱ���û�����";		
		public static string ERROR_NOPASSWORD				= "ȱ�����룡";
		public static string ERROR_USERNOTEXIST				= "�û��������ڣ�";
		public static string ERROR_PASSWORDNOTMARCH			= "�û��������";
		public static string ERROR_USERNOTBELONGTOANYGROUP  = "�û��������κ��û��飡";
		public static string ERROR_USERGROUPHASNORIGHTS		= "�û���û���κ�Ȩ�ޣ�";
		public static string ERROR_NOACCESSRIGHT			= "û�з���Ȩ�ޣ�";
		public static string ERROR_USERNOTINUSERGROUP		= "���û��������κ��û��飡";
		public static string ERROR_MODULENOTEXIST			= "ģ��{0}����ϵͳ�У���֪ͨ����Ա��";
		public static string ERROR_LOGINOVERTIME			= "δ��¼���¼��ʱ�������µ�¼��";
		public static string ERROR_NOMODULECODE				= "ҳ�治�����κ�ģ�飬��֪ͨ����Ա��";
		#endregion
	
		static ErrorCenter()
		{
			_hashtable  = new Hashtable();
			_hashtable.Add("bomfileparser_error_upload_upperlimit",bomfileparser_error_upload_upperlimit);
			_hashtable.Add("bomfileparser_system_error",bomfileparser_system_error);
			_hashtable.Add("bomfileparser_error_file_column",bomfileparser_error_file_column);
			_hashtable.Add("bomfileparser_error_not_empty",bomfileparser_error_not_empty);
			_hashtable.Add("bomfileparser_error_effectdate_error",bomfileparser_error_effectdate_error);
			_hashtable.Add("bomfileparser_error_ineffectdate_error",bomfileparser_error_ineffectdate_error);
			//_hashtable.Add("additemroute_error",additemroute_error);
		}

		public static string GetErrorServerDescription(Type type,string errorCode, LanguageType  languageType)
		{
			return errorCode;
		}

		public static string GetErrorServerDescription(Type type,string errorCode, System.Globalization.CultureInfo cultureInfo)
		{
			return errorCode;
		}

		public static string GetErrorServerDescription(Type type,string errorCode)
		{
			return errorCode;
		}

		public static string GetErrorUserDescription(Type type,string errorCode, LanguageType  languageType)
		{
			return errorCode;
		}

		public static string GetErrorUserDescription(Type type,string errorCode, System.Globalization.CultureInfo cultureInfo)
		{
			return errorCode;
		}

		public static string GetErrorUserDescription(Type type,string errorCode)
		{
			return errorCode;
		}
		//	
		//
		//		public static string GetErrorDescription(Type type,string errorCode)
		//		{
		//			return _hashtable[type.Name.ToLower()+"_"+errorCode.ToLower()].ToString();
		//		}
		//
		//		public static string GetParseErrorDescription(Type type,string errorCode,int errorLine)
		//		{
		//			return string.Empty;
		//		}
	}
}