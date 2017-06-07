using System;
using BenQGuru.eMES.AlertModel;
using System.Web.UI.WebControls;

namespace BenQGuru.eMES.Web.Alert
{
	/// <summary>
	/// �ϰ汾,�µ���AlertConst
	/// </summary>
	public class AlertMsg
	{
		public AlertMsg()
		{
		}

		public static string GetAlertName(string code,ControlLibrary.Web.Language.ILanguageComponent lang)
		{
			AlertConst ac = new AlertConst(lang);
			return ac.GetName(code);
		}

		public static string GetAlertCode(string name,ControlLibrary.Web.Language.ILanguageComponent lang)
		{
			AlertConst ac = new AlertConst(lang);
			return ac.GetCode(name);
		}
	}

	
	//Alert���õ��Ĵ���ͺ������ֵ�ת��
	public class AlertConst
	{
		private ControlLibrary.Web.Language.ILanguageComponent _lang;
		private string _ngName;
		private string _directPassName;
		private string _firstName;
		private string _itemName;
		private string _ssName;
		private string _modelName;
		private string _segmentName;
		private string _timePeriod;
		private string _resource;
		private string _ecg2ec;

		public string MODEL_TEMP = "����<%MODEL%>";
		public string LINE_TEMP = "����<%LINE%>";
		public string PRODUCT_TEMP= "��Ʒ<%PRODUCT%>";
		public string SEGMENT_TEMP= "����<%SEGMENT%>";
		public string TIMEPERIOD_TEMP= "ʱ���<%TIMEPERIOD%>";
		public string RESOURCE_TEMP= "��Դ<%RESOURCE%>";
		public string ECG2EC_TEMP= "����<%ERRORCODEGROUP:ERRORCODE%>";
		public string CPKTESTITEM_TEMP= "CPK������<%CPKTESTITEM%>";
		public string STANDARD_TEMP = "��׼Ϊ<%CONDITION%><%CONDITIONVALUE1%>";
		public string CONDITIONVALUE2_TEMP = "��<%CONDITIONVALUE2%>";
		public string DATAVALUE_TEMP = "����ֵΪ<%VALUE%>";
		public string OVERFLOW_TEMP = "����";

		public AlertConst(ControlLibrary.Web.Language.ILanguageComponent lang)
		{
			_lang = lang;
			this._ngName = _lang.GetString("$ALERT_" + AlertType_Old.NG);
			this._directPassName = this._lang.GetString("$ALERT_" + AlertType_Old.DirectPass);
			this._firstName = _lang.GetString("$ALERT_" + AlertType_Old.First);
			this._itemName = _lang.GetString("$ALERT_" + AlertItem_Old.Item);
			this._ssName = _lang.GetString("$ALERT_" + AlertItem_Old.SS);
			this._modelName = _lang.GetString("$ALERT_" + AlertItem_Old.Model);
			this._segmentName = _lang.GetString("$ALERT_" + AlertItem_Old.Segment);
			this._timePeriod = _lang.GetString("$ALERT_" + AlertType_Old.ResourceNG + "_TIMEPERIOD");
			this._resource = _lang.GetString("$ALERT_" + AlertType_Old.ResourceNG + "_RESOURCE");
			this._ecg2ec = _lang.GetString("$ALERT_" + AlertType_Old.ResourceNG + "_ECG2EC");

			MODEL_TEMP = _lang.GetString("$MODEL_TEMP");
			LINE_TEMP = _lang.GetString("$LINE_TEMP");
			PRODUCT_TEMP= _lang.GetString("$PRODUCT_TEMP");
			SEGMENT_TEMP= _lang.GetString("$SEGMENT_TEMP");
			CPKTESTITEM_TEMP= _lang.GetString("$CPKTESTITEM_TEMP");
			STANDARD_TEMP = _lang.GetString("$STANDARD_TEMP");
			CONDITIONVALUE2_TEMP = _lang.GetString("$CONDITIONVALUE2_TEMP");
			DATAVALUE_TEMP = _lang.GetString("$DATAVALUE_TEMP");
			OVERFLOW_TEMP = _lang.GetString("$OVERFLOW_TEMP");

			TIMEPERIOD_TEMP = _lang.GetString("$TIMEPERIOD_TEMP");
			RESOURCE_TEMP = _lang.GetString("$RESOURCE_TEMP");
			ECG2EC_TEMP = _lang.GetString("$ECG2EC_TEMP");
		}

		public string GetTemplateName(string code)
		{
			if(code == AlertItem_Old.Item)
			{
				return PRODUCT_TEMP;
					
			}
			else if(code == AlertItem_Old.Model)
			{
				return MODEL_TEMP;
			}
			else if(code == AlertItem_Old.SS)
			{
				return LINE_TEMP;
			}
			else if(code == AlertItem_Old.Segment)
			{
				return SEGMENT_TEMP;
			}
			else if(code == AlertItem_Old.Resource)
			{
				return RESOURCE_TEMP;
			}
			else 
				return string.Empty;
		}

		public string GetTemplateHelp()
		{
			//return @"����˵��:<br>����:MODEL&nbsp;&nbsp;&nbsp;����:LINE<br>��Ʒ:PRODUCT&nbsp;&nbsp;&nbsp;����:SEGMENT<br>CPK������:CPKTESTITEM<br>����:CONDITION&nbsp;&nbsp;&nbsp;����ֵ:VALUE<br>����ֵ1:CONDITIONVALUE1&nbsp;����ֵ2:CONDITIONVALUE2";
			return @"����˵��:<br>��Ʒ:PRODUCT&nbsp;&nbsp;&nbsp;����:MODEL&nbsp;&nbsp;&nbsp;����:LINE&nbsp;&nbsp;&nbsp;����:SEGMENT<br>��Դ:RESOURCE&nbsp;&nbsp;&nbsp;����:ERRORCODEGROUP:ERRORCODE<br>CPK������:CPKTESTITEM&nbsp;&nbsp;&nbsp;ʱ���:TIMEPERIOD<br>����:CONDITION&nbsp;&nbsp;&nbsp;����ֵ:VALUE<br>����ֵ1:CONDITIONVALUE1&nbsp;����ֵ2:CONDITIONVALUE2";
			//return _lang.GetString("$ALERT_MSG_TEMP");
		}

		//melo �����2006.12.5 ���ڶ�����
		public string GetTemplateHelp(ControlLibrary.Web.Language.LanguageComponent languageComponent)
		{
			return @""+languageComponent.GetString("CodeDescription")+":<br>"+languageComponent.GetString("Item")+":PRODUCT&nbsp;&nbsp;&nbsp;"+languageComponent.GetString("ModelType")+":MODEL&nbsp;&nbsp;&nbsp;"+languageComponent.GetString("Line")+":LINE&nbsp;&nbsp;&nbsp;"+languageComponent.GetString("Segment")+":SEGMENT<br>"+languageComponent.GetString("Resource1")+":RESOURCE&nbsp;&nbsp;&nbsp;"+languageComponent.GetString("ErrorCodeGroup1")+":ERRORCODEGROUP:ERRORCODE<br>"+languageComponent.GetString("CPK")+":CPKTESTITEM&nbsp;&nbsp;&nbsp;"+languageComponent.GetString("TimePeriod")+":TIMEPERIOD<br>"+languageComponent.GetString("Condition")+":CONDITION&nbsp;&nbsp;&nbsp;"+languageComponent.GetString("Value")+":VALUE<br>"+languageComponent.GetString("Condition1")+":CONDITIONVALUE1&nbsp;"+languageComponent.GetString("Condition2")+":CONDITIONVALUE2";
		}

		public string GetName(string code)
		{
			string name = _lang.GetString("$ALERT_" + code);
			if(name == string.Empty)
				return code;
			else
				return name;

			#region OLD useless
			//			switch(code)
			//			{
			//				case AlertType_Old.Manual:
			//					return "�ֶ�Ԥ��";
			//				case AlertType_Old.NG:
			//					return "������";
			//				case "DirectPass":
			//					return "ֱͨ��";
			//				case "First":
			//					return "�׼�����";
			//				case "Item":
			//					return "��Ʒ";
			//				case "Model":
			//					return "����";
			//				case "SS":
			//					return "����";
			//				case "Segment":
			//					return "����";
			//				case "BW":
			//					return "����{0}��{1}֮��";
			//				case "LE":
			//					return "С�ڵ���";
			//				case "GE":
			//					return "���ڵ���";
			//				
			//				default:
			//					return code;
			//
			//			}
			#endregion

		}

		public string GetCode(string name)
		{
			#region OLD useless
//			switch(name)
//			{
//				case "������":
//					return "NG";
//				case "ֱͨ��":
//					return "DirectPass";
//				case "�׼�����":
//					return "First";
//				case "��Ʒ":
//					return "Item";
//				case "����":
//					return "Model";
//				case "����":
//					return "SS";
//				case "����":
//					return "Segment";
//				default:
//					return name;
//
//			}
			#endregion

			if(name ==this._ngName)
				return "NG";
			if(name == this._directPassName)
				return "DirectPass";
			if(name == this._firstName)
				return "First";
			if(name == this._itemName)
				return "Item";
			if(name == this._modelName)
				return "Model";
			if(name ==this._ssName)
				return "SS";
			if(name == this._segmentName)
				return "Segment";
			
			return name;
		}

		

	}

	public class AlertLevelBuilder
	{
		public static void Build(System.Web.UI.WebControls.ListItemCollection items,AlertConst alertConst)
		{
			items.Clear();
			items.Add(new ListItem(alertConst.GetName(AlertLevel_Old.Severity),AlertLevel_Old.Severity));
			items.Add(new ListItem(alertConst.GetName(AlertLevel_Old.Important),AlertLevel_Old.Important));
			items.Add(new ListItem(alertConst.GetName(AlertLevel_Old.Primary),AlertLevel_Old.Primary));
		}
	}

	public class AlertStatusBuilder
	{
		public static void Build(System.Web.UI.WebControls.ListItemCollection items,AlertConst alertConst)
		{
			items.Clear();
			items.Add(new ListItem(alertConst.GetName(AlertStatus_Old.Unhandled),AlertStatus_Old.Unhandled));
			items.Add(new ListItem(alertConst.GetName(AlertStatus_Old.Observing),AlertStatus_Old.Observing));
			items.Add(new ListItem(alertConst.GetName(AlertStatus_Old.Handling),AlertStatus_Old.Handling));
			items.Add(new ListItem(alertConst.GetName(AlertStatus_Old.Closed),AlertStatus_Old.Closed));
		}
	}

	public class AlertItemBuilder
	{
		public static void Build(string alerttype,System.Web.UI.WebControls.ListItemCollection items,AlertConst alertConst)
		{
			items.Clear();

			if(alerttype == AlertType_Old.NG)
			{
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Item),AlertItem_Old.Item));
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.SS),AlertItem_Old.SS));
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Model),AlertItem_Old.Model));
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Resource),AlertItem_Old.Resource));
			}
			else if(alerttype == AlertType_Old.PPM || alerttype == AlertType_Old.CPK)
			{
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Item),AlertItem_Old.Item));
			}
			else if(alerttype == AlertType_Old.DirectPass)
			{
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Item),AlertItem_Old.Item));
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.SS),AlertItem_Old.SS));
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Segment),AlertItem_Old.Segment));
			}
			else if(alerttype == AlertType_Old.First)
			{
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Item),AlertItem_Old.Item));
			}
			else if(alerttype == AlertType_Old.ResourceNG)
			{
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Item),AlertItem_Old.Item));
			}
			else if(alerttype == AlertType_Old.Manual)
			{

			}
			else
			{
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Item),AlertItem_Old.Item));
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.SS),AlertItem_Old.SS));
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Model),AlertItem_Old.Model));	
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Segment),AlertItem_Old.Segment));
				items.Add(new ListItem(alertConst.GetName(AlertItem_Old.Resource),AlertItem_Old.Resource));
			}
		}
	}

	public class AlertTypeBuilder
	{
		public static void Buid(ListItemCollection items,AlertConst alertConst)
		{
			items.Clear();
			items.Add(new ListItem(alertConst.GetName(AlertType_Old.NG),AlertType_Old.NG));
			items.Add(new ListItem(alertConst.GetName(AlertType_Old.PPM),AlertType_Old.PPM));
			items.Add(new ListItem(alertConst.GetName(AlertType_Old.DirectPass),AlertType_Old.DirectPass));
			items.Add(new ListItem(alertConst.GetName(AlertType_Old.CPK),AlertType_Old.CPK));
			items.Add(new ListItem(alertConst.GetName(AlertType_Old.First),AlertType_Old.First));
			items.Add(new ListItem(alertConst.GetName(AlertType_Old.ResourceNG),AlertType_Old.ResourceNG));
		}
	}

	//����AlertItem�Ĳ�ͬ,�趨SelectTextBox��ѯ��Ŀ���
	public class AlertItemValueHelper
	{
		public static void SetItemValue(BenQGuru.eMES.Web.SelectQuery.SelectableTextBox stbitem,string alerttype,string alertitem)
		{
			if(alertitem == "*")
			{
				if(alerttype == AlertType_Old.First)
					stbitem.Target = "stepsequence";
				else
					stbitem.Target = AlertItem_Old.Item.ToLower();
			}
			else if(alertitem == AlertItem_Old.SS)
				stbitem.Target = "stepsequence";
			else
				stbitem.Target = alertitem.ToLower();

			stbitem.Type = stbitem.Target;

			stbitem.Text = string.Empty;
		}
	}

	public class AutoRefreshConst
	{
		public const int MM_Unit = 1000 * 60;
		public const int defaultMI = 5;
		public static int GetMMInterval(int mi)
		{
			return mi * MM_Unit;
		}
	}

	public class ColorHelper
	{
		public static System.Drawing.Color GetColor(string code)
		{
			if(code == AlertLevel_Old.Primary)
				return  System.Drawing.Color.FromArgb(int.Parse("9933CC",System.Globalization.NumberStyles.HexNumber));

			else if(code == AlertLevel_Old.Important)
				return  System.Drawing.Color.FromArgb(int.Parse("FF9900",System.Globalization.NumberStyles.HexNumber));

			else if(code == AlertLevel_Old.Severity)
				return System.Drawing.Color.FromArgb(int.Parse("FF0000",System.Globalization.NumberStyles.HexNumber));

			else if(code == AlertStatus_Old.Unhandled)
				return System.Drawing.Color.FromArgb(int.Parse("FF9933",System.Globalization.NumberStyles.HexNumber));

			else if(code == AlertStatus_Old.Observing)
				return System.Drawing.Color.FromArgb(int.Parse("0033CC",System.Globalization.NumberStyles.HexNumber));

			else if(code == AlertStatus_Old.Handling)
				return System.Drawing.Color.FromArgb(int.Parse("009900",System.Globalization.NumberStyles.HexNumber));

			else if(code == AlertStatus_Old.Closed)
				return System.Drawing.Color.Black;
			else 
				return System.Drawing.Color.Black;
		}
	}

	public class AlertPageAction
	{
		public const string Add = "add";
		public const string Edit = "edit";
	}

	public class NumberHelper
	{
		public static string TrimZero(decimal d)
		{
			string s = d.ToString();
			if(s.IndexOf(".") >= 0 )
				return s.TrimEnd('0').TrimEnd('.');
			else
				return s;
		}
	}
}
