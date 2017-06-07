using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BenQGuru.eMES.BaseDataModel
{
    
    public class FieldDataFrom
    {
        public static string ParentItem = "ParentItem";     // ���Ը�Object��ĳ����λ
        public static string ParentNode = "ParentNode";     // ���Ը����ý����ĳNode
        public static string Query = "Query";               // ����SQL����ѯ
        public static string UserCode = "UserCode";         // ִ��ʱ��UserCode
        public static string Date = "Date";                 // ��ǰ����
        public static string Time = "Time";                 // ��ǰʱ��
        public static string GUID = "GUID";                 // GUID
    }
    public class FieldCheckType
    {
        public static string Exist = "Exist";               // ��������Ƿ����
        public static string DataType = "DataType";         // �����������
        public static string Length = "Length";             // ���ȣ��ڱ��ʽ��д��{0}>=2��ʾ����������2
        public static string DataRange = "DataRange";       // ���ݷ�Χ���ڱ��ʽ��д��{0}>=0&&{0}<=9����ʾ��0��9
    }

    public class ConfigObject
    {
        public string Type = "";    // Class Type��������
        public string Name = "";    // ��ʶ���ƣ�����NodeList��
        public string Text = "";    // ��ʾ�ı�

        public string TemplateFileName = "";    // ģ���ļ���

        public List<ConfigField> FieldList = null;      // �ֶ��б�
        public List<ConfigField> DefaultFieldList = null;   // Ĭ���ֶ��б�

        public static void LoadConfig(string xmlFile, out List<ConfigObject> outConfigObjList, out MatchType matchType)
        {
            outConfigObjList = null;
            matchType = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);
            XmlNode nodeItemRoot = doc.SelectSingleNode("//ItemList");
            if (nodeItemRoot == null)
                return;
            XmlNodeList nodeItemList = nodeItemRoot.SelectNodes("Item");
            if (nodeItemList == null)
                return;
            outConfigObjList = new List<ConfigObject>();
            for (int i = 0; i < nodeItemList.Count; i++)
            {
                XmlNode nodeItem = nodeItemList[i];
                ConfigObject cfgObj = GetConfigObjectFromXmlNode(nodeItem);
                if (cfgObj != null)
                    outConfigObjList.Add(cfgObj);
            }
            XmlNode nodeMatchRoot = doc.SelectSingleNode("//MatchList");
            if (nodeMatchRoot != null)
            {
                matchType = new MatchType();
                XmlNodeList nodeMatchList = nodeMatchRoot.SelectNodes("Match");
                for (int i = 0; nodeMatchList != null && i < nodeMatchList.Count; i++)
                {
                    string strMatchName = GetNodeAttribute(nodeMatchList[i], "Name");
                    XmlNodeList nodeMatchDtl = nodeMatchList[i].SelectNodes("List");
                    for (int n = 0; nodeMatchDtl != null && n < nodeMatchDtl.Count; n++)
                    {
                        matchType.Add(strMatchName, GetNodeAttribute(nodeMatchDtl[n], "Value"), GetNodeAttribute(nodeMatchDtl[n], "Text"));
                    }
                }
            }
            return;
        }
        private static ConfigObject GetConfigObjectFromXmlNode(XmlNode nodeItem)
        {
            ConfigObject cfgObj = new ConfigObject();
            cfgObj.Type = GetNodeAttribute(nodeItem, "Type");
            cfgObj.Name = GetNodeAttribute(nodeItem, "Name");
            cfgObj.Text = GetNodeAttribute(nodeItem, "Text");
            cfgObj.TemplateFileName = GetNodeAttribute(nodeItem, "TemplateFileName");
            XmlNodeList nodeFieldList = nodeItem.SelectNodes("FieldList/Field");
            if (nodeFieldList == null)
                return null;
            cfgObj.FieldList = new List<ConfigField>();
            for (int n = 0; n < nodeFieldList.Count; n++)
            {
                XmlNode nodeField = nodeFieldList[n];
                cfgObj.FieldList.Add(GetConfigFieldFromXmlNode(nodeField));
            }
            nodeFieldList = nodeItem.SelectNodes("DefaultFieldList/Field");
            if (nodeFieldList != null)
            {
                cfgObj.DefaultFieldList = new List<ConfigField>();
                for (int n = 0; n < nodeFieldList.Count; n++)
                {
                    XmlNode nodeField = nodeFieldList[n];
                    cfgObj.DefaultFieldList.Add(GetConfigFieldFromXmlNode(nodeField));
                }
            }
            return cfgObj;
        }
        private static ConfigField GetConfigFieldFromXmlNode(XmlNode nodeField)
        {
            ConfigField cfgFld = new ConfigField();
            cfgFld.Name = GetNodeAttribute(nodeField, "Name");
            cfgFld.Text = GetNodeAttribute(nodeField, "Text");
            cfgFld.AllowNull = ConvertStringToBoolean(GetNodeAttribute(nodeField, "AllowNull", "true"));
            cfgFld.UniqueGroup = GetNodeAttribute(nodeField, "UniqueGroup");
            cfgFld.DefaultValue = GetNodeAttribute(nodeField, "DefaultValue");
            cfgFld.MatchType = GetNodeAttribute(nodeField, "MatchType");
            cfgFld.DataType = GetNodeAttribute(nodeField, "DataType", "string");
            cfgFld.DataFrom = GetNodeAttribute(nodeField, "DataFrom");
            cfgFld.ParentNodeField = GetNodeAttribute(nodeField, "ParentNodeField");
            cfgFld.IgnoreForeignWhenNull = ConvertStringToBoolean(GetNodeAttribute(nodeField, "IgnoreForeignWhenNull", "true"));
            cfgFld.DataQuerySql = GetNodeAttribute(nodeField, "DataQuerySql");
            cfgFld.DataQueryParam = GetNodeAttribute(nodeField, "DataQueryParam");
            cfgFld.IncludeItem = ConvertStringToBoolean(GetNodeAttribute(nodeField, "IncludeItem", "true"));
            cfgFld.ForeignItem = ConvertStringToBoolean(GetNodeAttribute(nodeField, "ForeignItem", "false"));
            if (cfgFld.ForeignItem == true)
            {
                XmlNode nodeTmp = nodeField.SelectSingleNode("ForeignItem");
                if (nodeTmp != null)
                {
                    cfgFld.ForeignObject = GetConfigObjectFromXmlNode(nodeTmp);
                }
            }
            XmlNodeList nodeChkList = nodeField.SelectNodes("CheckList/Check");
            if (nodeChkList != null)
            {
                List<ConfigCheck> chkList = new List<ConfigCheck>();
                for (int i = 0; i < nodeChkList.Count; i++)
                {
                    chkList.Add(GetConfigCheckFromXmlNode(nodeChkList[i]));
                }
                cfgFld.CheckList = chkList;
            }
            return cfgFld;
        }
        private static ConfigCheck GetConfigCheckFromXmlNode(XmlNode nodeCheck)
        {
            ConfigCheck chk = new ConfigCheck();
            chk.Type = GetNodeAttribute(nodeCheck, "Type");
            chk.ParentObjectType = GetNodeAttribute(nodeCheck, "ParentObjectType");
            chk.ParentObjectField = GetNodeAttribute(nodeCheck, "ParentObjectField");
            chk.CheckDataType = GetNodeAttribute(nodeCheck, "CheckDataType");
            chk.LengthExpression = GetNodeAttribute(nodeCheck, "LengthExpression");
            chk.DataRangeExpression = GetNodeAttribute(nodeCheck, "DataRangeExpression");
            chk.ExistCheckSql = GetNodeAttribute(nodeCheck, "ExistCheckSql");
            return chk;
        }
        private static string GetNodeAttribute(XmlNode node, string attributeName)
        {
            return GetNodeAttribute(node, attributeName, "");
        }
        private static string GetNodeAttribute(XmlNode node, string attributeName, string defaultValue)
        {
            if (node.Attributes[attributeName] == null)
                return defaultValue;
            else
                return node.Attributes[attributeName].Value;
        }
        private static bool ConvertStringToBoolean(string value)
        {
            return (value.ToUpper() == "TRUE" || value == "1");
        }
    }

    public class ConfigField
    {
        public string Name = "";        // ��������(Class������)
        public string Text = "";        // ����
        public bool AllowNull = true;   // �Ƿ�Ϊ�գ�true/false
        public string UniqueGroup = ""; // Ψһ�Լ��Ⱥ�飬UniqueGroup��ͬ����λ��ʾ���ϵ�Ψһ�ֶ�
        public string DefaultValue = "";    // Ĭ��ֵ����ȡʱֱ�ӽ�Attributeֵ��ֵ����λ
        public string MatchType = "";       // ƥ������
        public string DataType = "";        // ��������
        public bool IncludeItem = true;     // �Ƿ������Class�У�true/false
        public bool ForeignItem = false;    // �Ƿ������ⲿClass��true/false
        public bool IgnoreForeignWhenNull = true;   // ������Ϊ��ʱ�����ٴ����ⲿ��¼
        public string DataFrom = "";        // ������Դ���ο�FieldDataFrom�б�
        public string ParentNodeField = ""; // ���DataFrom=ParentObject��ParentItem��������ñ�ʾ���Ը�����Ľ������������
        public string DataQuerySql = "";    // ���DataFrom=Query��������ñ�ʾ��ѯ��SQL��䣬ֻ����һ���ֶΣ��м������{0}��{1}����ʽ����
        public string DataQueryParam = "";  // ���DataFrom=Query��������ñ�ʾ����ĸ�ʽ������Class����������������ö��Ÿ���

        public ConfigObject ForeignObject = null;
        public List<ConfigCheck> CheckList = null;

        public object Value = null;
    }

    public class ConfigCheck
    {
        public string Type = "";                // ������ͣ���ο�FieldCheckType
        public string ParentObjectType = "";    // ���Type=Exist��������ñ�ʾ����ⲿ��Ķ�������
        public string ParentObjectField = "";   // ���Type=Exist��������ñ�ʾ����ⲿ��Ķ����������
        public string CheckDataType = "";       // ���Type=DataType��������ñ�ʾ�����������ͣ�integer(����)��numeric(����)
        public string LengthExpression = "";    // ���Type=Length��������ñ�ʾ���ȼ����ʽ�����磺{0}>=2��{0}>=2&&{0}<=5������{0}�Ĵ���ֵ������ʵ�ʳ���
        public string DataRangeExpression = ""; // ���Type=DataRange��������ñ�ʾ��Χ�����ʽ�����磺{0}>=2&&{0}<=5��{0}>={MinValue}������{0}�Ĵ���ֵ������ʵ��ֵ��{MinValue}��ʾȡClass��MinValue����ֵ
        public string ExistCheckSql = "";       // ���Type=Exist��������ü�����ݴ����Ե�SQL��䣬����count(*)��ʽ��SQL����п��԰���{0}��Ϊʵ�����ݵ�ռλ��ʽ
    }

    public class MatchType
    {
        private Dictionary<string, Dictionary<string, string>> list = new Dictionary<string, Dictionary<string, string>>();

        public void Add(string name, string value, string text)
        {
            name = name.ToUpper();
            text = text.ToUpper();
            Dictionary<string, string> dic = null;
            if (list.ContainsKey(name) == true)
                dic = list[name];
            else
            {
                dic = new Dictionary<string, string>();
                list.Add(name, dic);
            }
            if (dic.ContainsKey(text) == false)
                dic.Add(text,value);
            else
                dic[text] = value;
        }

        public string GetValue(string name, string text)
        {
            name = name.ToUpper();
            text = text.ToUpper();
            Dictionary<string, string> dic = list[name];
            if (dic == null)
                return "";

            string ret = null;
            try
            {
                ret = dic[text];
            }
            catch
            {
                ret = string.Empty;
            }
            if (ret == null)
                return string.Empty;

            return ret;
        }

    }
    
}
