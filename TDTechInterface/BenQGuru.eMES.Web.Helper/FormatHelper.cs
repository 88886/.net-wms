using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.MutiLanguage;
using System.Collections.Specialized;

namespace BenQGuru.eMES.Web.Helper
{
    /// <summary>
    /// Class1 ��ժҪ˵����
    /// </summary>
    public class FormatHelper : MarshalByRefObject
    {
        public FormatHelper()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        //Laws Lu,max life time to unlimited
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public const char TRUE = '1';
        public const char FALSE = '0';
        public const string TRUE_STRING = "1";
        public const string FALSE_STRING = "0";
        //sammer kong 20050819
        public const int TIME_DEFAULT = -1;

        public static int TODateInt(System.DateTime dateTime)
        {
            if (dateTime <= DateTime.MinValue)
            {
                return 0;
            }

            return dateTime.Year * 10000 + dateTime.Month * 100 + dateTime.Day;

        }

        public static int TODateInt(string Date)
        {
            if (Date == null || Date.Trim() == string.Empty)
            {
                return 0;
            }
            char[] split = new char[2];
            split[0] = '/';
            split[1] = '-';
            string[] array = null;
            array = Date.Split(split);
            return System.Int32.Parse(array[0]) * 10000 + System.Int32.Parse(array[1]) * 100 + System.Int32.Parse(array[2]);
        }

        public static int TOTimeInt(System.DateTime dateTime)
        {
            if (dateTime <= DateTime.MinValue)
            {
                return 0;
            }

            return dateTime.Hour * 10000 + dateTime.Minute * 100 + dateTime.Second;
        }

        public static int TOTimeInt(string time)
        {
            if (time == null || time.Trim() == string.Empty)
            {
                return 0;
            }

            char[] split = new char[] { ':' };
            string[] array = time.Split(split);

            return System.Int32.Parse(array[0]) * 10000 + System.Int32.Parse(array[1]) * 100 + System.Int32.Parse(array[2]);
        }

        public static string TODateTimeString(int date, int time)
        {
            return TODateTimeString(date, time, "/", ":");
        }

        public static string ToDateString(int date)
        {
            return ToDateString(date, "-");
        }

        public static string ToDateString(decimal date)
        {
            return ToDateString(System.Int32.Parse(date.ToString()));
        }


        /// <summary>
        /// sammer kong 20050819
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToTimeString(int time)
        {
            if (time == FormatHelper.TIME_DEFAULT)
            {
                return string.Empty;
            }
            return ToTimeString(time, ":");
        }

        /// <summary>
        /// ֻ��ʾʱ�֣�����ʾ��
        /// added by jessie lee
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToShortTimeString(int time)
        {
            string timeString = time.ToString().PadLeft(6, '0');
            string timeSplitChar = ":";
            return string.Format("{0}{1}{2}"
                , timeString.Substring(0, 2)
                , timeSplitChar
                , timeString.Substring(2, 2));
        }

        public static DateTime ToDateTime(int date, int time)
        {
            if (date == 0 || time == 0)
            {
                return DateTime.Now;
            }
            string dateString = date.ToString().PadLeft(8, '0');
            string timeString = time.ToString().PadLeft(6, '0');
            return new DateTime(System.Int32.Parse(dateString.Substring(0, 4)),
                                System.Int32.Parse(dateString.Substring(4, 2)),
                                System.Int32.Parse(dateString.Substring(6, 2)),
                System.Int32.Parse(timeString.Substring(0, 2)),
                System.Int32.Parse(timeString.Substring(2, 2)),
                System.Int32.Parse(timeString.Substring(4, 2)));
        }

        public static DateTime ToDateTime(int date)
        {
            if (date == 0)
            {
                return DateTime.Now;
            }
            string dateString = date.ToString().PadLeft(8, '0');
            return new DateTime(System.Int32.Parse(dateString.Substring(0, 4)),
                                System.Int32.Parse(dateString.Substring(4, 2)),
                                System.Int32.Parse(dateString.Substring(6, 2)));
        }


        public static string TODateTimeString(int date, int time, string dateSplitChar, string timeSplitChar)
        {
            if (date <= 0 && time <= 0)
            {
                return string.Empty;
            }

            return string.Format("{0} {1}", ToDateString(date, dateSplitChar), ToTimeString(time, timeSplitChar));
        }

        public static string ToDateString(int date, string dateSplitChar)
        {
            if (date == 0)
            {
                return string.Empty;
            }

            string dateString = date.ToString().PadLeft(8, '0');

            return string.Format("{0}{1}{2}{3}{4}"
                                , dateString.Substring(0, 4)
                                , dateSplitChar
                                , dateString.Substring(4, 2)
                                , dateSplitChar
                                , dateString.Substring(6, 2));
        }

        public static string ToTimeString(int time, string timeSplitChar)
        {
            string timeString = time.ToString().PadLeft(6, '0');

            return string.Format("{0}{1}{2}{3}{4}"
                                , timeString.Substring(0, 2)
                                , timeSplitChar
                                , timeString.Substring(2, 2)
                                , timeSplitChar
                                , timeString.Substring(4, 2));
        }

        public static int TimeAddSeconds(int time, int addSeconds)
        {
            int dateRef = 0;
            while (time < 0)
            {
                time += 240000;
                dateRef--;
            }
            while (time >= 240000)
            {
                time -= 240000;
                dateRef++;
            }

            if (time == 0)
            {
                time = 1;
            }
            else
            {
                DateTime temp = ToDateTime(20000101, time);
                time = TOTimeInt(temp.AddSeconds(addSeconds));
                TimeSpan span = temp.AddSeconds(addSeconds).Date - temp.Date;
                dateRef += (int)Math.Floor(span.TotalDays);
            }

            time += 240000 * dateRef;
            return time;
        }

        public static string CleanString(string inputString)
        {
            StringBuilder retVal = new StringBuilder();

            // check incoming parameters for null or blank string
            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = inputString.Trim();

                //convert some harmful symbols incase the regular
                //expression validators are changed
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }

                // Replace single quotes with white space
                retVal.Replace("'", " ");
            }

            return retVal.ToString().Trim();
        }

        public static string CleanString(string inputString, int maxLength)
        {
            // check incoming parameters for null or blank string
            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = CleanString(inputString);

                //chop the string incase the client-side max length
                //fields are bypassed to prevent buffer over-runs
                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);
            }

            return inputString;
        }

        public static string DisplayBoolean(bool value, ControlLibrary.Web.Language.LanguageComponent languageControl)
        {
            LanguageWord trueText = languageControl.GetLanguage("trueText");
            LanguageWord falseText = languageControl.GetLanguage("falseText");

            if (trueText == null || falseText == null)
            {
                return value ? "Y" : "N";
            }

            return value ? trueText.ControlText : falseText.ControlText;
        }

        public static string DisplayBoolean(string value, ControlLibrary.Web.Language.LanguageComponent languageControl)
        {
            return DisplayBoolean(value == TRUE_STRING, languageControl);
        }

        public static bool StringToBoolean(string value)
        {
            return value == TRUE_STRING;
        }

        /// <summary>
        /// ������ָ���ַ���λ����Ϊ1���򷵻�true��0����false
        /// </summary>
        /// <param name="orginalString">ָ���ַ���</param>
        /// <param name="index">λ������</param>
        /// <returns></returns>
        public static bool StringToBoolean(string orginalString, int index)
        {
            if ((index > orginalString.Length) || (index < 0))
            {
                ExceptionManager.Raise(typeof(FormatHelper), "$Error_Index_Out_Of_Range");
            }
            return StringToBoolean(orginalString[index].ToString());
        }

        public static bool StringToBoolean(string value, string trueValue)
        {
            return value == trueValue;
        }

        public static string BooleanToString(bool value)
        {
            return value ? TRUE_STRING : FALSE_STRING;
        }


        public static string BooleanToGirdCheckBoxString(char value)
        {
            return value == TRUE ? "true" : "false";
        }

        public static string BooleanToString(string orginalString, int index,bool value)
        {
            if ((index > orginalString.Length) || (index < 0))
            {
                ExceptionManager.Raise(typeof(FormatHelper), "$Error_Index_Out_Of_Range");
            }
            char[] tmpChar = orginalString.ToCharArray();
            tmpChar[index] = value ? TRUE : FALSE;
            return new string(tmpChar);
        }

        public static string PKCapitalFormat(string text)
        {
            return text.ToUpper();
        }

        public static string ProcessQueryValues(string text)
        {
            string[] array = text.ToUpper().Split(new char[] { ',', ';' });

            return ProcessQueryValues(array);
        }

        public static string ProcessQueryValues(string text, bool toUpper)
        {
            if (toUpper)
            {
                string[] array = text.ToUpper().Split(new char[] { ',', ';' });

                return ProcessQueryValues(array);
            }
            else
            {

                string[] array = text.Split(new char[] { ',', ';' });

                return ProcessQueryValues(array);
            }
        }

        public static string ProcessQueryValues(string[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "";
            }
            else
            {
                string retText = "";
                foreach (string str in array)
                {
                    retText += "'" + str.Trim() + "',";
                }
                return retText.Substring(0, retText.Length - 1);
            }
        }
        public static string ProcessQueryValues(string[] array, bool toUpper)
        {
            if (toUpper)
            {
                if (array == null || array.Length == 0)
                {
                    return "";
                }
                else
                {
                    string retText = "";
                    foreach (string str in array)
                    {
                        retText += "'" + str.ToUpper() + "',";
                    }
                    return retText.Substring(0, retText.Length - 1);
                }
            }
            else
            {
                if (array == null || array.Length == 0)
                {
                    return "";
                }
                else
                {
                    string retText = "";
                    foreach (string str in array)
                    {
                        retText += "'" + str + "',";
                    }
                    return retText.Substring(0, retText.Length - 1);
                }
            }
        }
        public static DBDateTime GetNowDBDateTime(IDomainDataProvider domainDataProvider)
        {
            object[] objs = domainDataProvider.CustomQuery(typeof(DBDateTime),
                new SQLParamCondition("select to_char(sysdate,'yyyymmdd') as dbdate,to_char(sysdate,'hh24miss')  as dbtime from dual where $RCARD = '1'"
                , new SQLParameter[] { new SQLParameter("RCARD", typeof(string), "1") }));
            if (objs.Length == 0)
                ExceptionManager.Raise(typeof(FormatHelper), "$SystemError_GetDBTimeError");
            return (DBDateTime)objs[0];
        }

        public static int GetSpanSeconds(int beginDate, int beginTime, int endDate, int endTime)
        {
            DateTime begin = ToDateTime(beginDate, beginTime);
            DateTime end = ToDateTime(endDate, endTime);

            TimeSpan timeSpan = end - begin;
            return (int)Math.Ceiling(timeSpan.TotalSeconds) + 1;
        }

        public static Updater GetCsVersion(IDomainDataProvider domainDataProvider)
        {
            object[] objs = domainDataProvider.CustomQuery(typeof(Updater),
                new SQLCondition("select CSVERSION,LOCATION,LOGINUSER,LOGINPASSWORD,ISAVIABLE from TBLCSUPDATER where  ISAVIABLE = 1"));
            if (objs == null || objs.Length < 1)
                return null;
            return (Updater)objs[0];
        }

        public static int GetUpdateErrorCount(IDomainDataProvider domainDataProvider, string version)
        {
            return domainDataProvider.GetCount(
               new SQLCondition("select count(*) from TBLUPDATELOG where VERSION = '" + version + "' and result='FALSE'"));
        }

        public static string GetUniqueID(string moCode, string rCard,string cardSequnce)
        {
            string str = rCard + cardSequnce + System.Guid.NewGuid();
            if (str.Length > 40)
            {
                return str.Substring(0, 40);
            }
            else
            {
                return str;
            }
        }

        //��ȡʱ�䷶Χsql
        public static string GetDateRangeSql(string DateColumnName, string TimeColumnName, int beginDate, int beginTime, int endDate,int endTime)
        {

            #region �����ж�
            //�����λ����Ϊ��,���ؿ�
            if (DateColumnName == string.Empty || TimeColumnName == string.Empty) return string.Empty;
            //�����ʼʱ��ͽ���ʱ��Ƿ�(<0),���ؿ�
            if (beginDate <= 0 || endDate <= 0) return string.Empty;
            //�����ʼʱ��ͽ���ʱ����ͬ , ֻȡ��������
            if (beginDate == endDate)
            {
                return string.Format(" AND {0} = {1}  AND {2} > {3} AND {2} < {4} ", DateColumnName, beginDate, TimeColumnName, beginTime, endTime);
            }

            #endregion

            string returnSql = string.Empty;
            returnSql = string.Format(" AND ({0} * 1000000 + {1}) > {2} AND ({0} * 1000000 + {1}) < {3} ", DateColumnName, TimeColumnName, FormatHelper.ToDateTimeInt(beginDate, beginTime), FormatHelper.ToDateTimeInt(endDate, endTime));

            return returnSql;
        }

        private static decimal ToDateTimeInt(int date, int time)
        {
            decimal dd = Convert.ToDecimal(date * 1000000D + time);
            return dd;
        }

        //��ȡʱ�䷶Χsql
        //����sql��ʽΪ AND columnName > (beginDate-1) AND columnName < (endDate+1) 
        //�����滻 AND columnName >=beginDate AND columnName < =endDate
        //�����滻 AND columnName between beginDate AND endDate
        //��Ϊ>= �� between and �����õ�����,�����ѯЧ�ʵ���
        public static string GetDateRangeSql(string columnName, int beginDate,int endDate)
        {

            #region �����ж�
            //�����λ����Ϊ��,���ؿ�
            if (columnName == string.Empty) return string.Empty;
            //�����ʼʱ��ͽ���ʱ��Ƿ�(<0),���ؿ�
            if (beginDate <= 0 || endDate <= 0) return string.Empty;
            //�����ʼʱ��ͽ���ʱ����ͬ , ֻȡ��������
            if (beginDate == endDate)
            {
                return string.Format(" AND {0} = {1} ", columnName, beginDate);
            }

            #endregion

            int smallDate = beginDate;
            int bigDate = endDate;
            if (beginDate > endDate)
            {
                smallDate = endDate;
                bigDate = beginDate;
            }

            string returnSql = string.Empty;
            returnSql = string.Format(" AND {0} > {1} AND {0} < {2} ", columnName, smallDate - 1, bigDate + 1);
            return returnSql;
        }
        //��ȡʱ�䷶Χsql
        //����sql��ʽΪ AND columnName > (beginDate-1) AND columnName < (endDate+1) 
        //�����滻 AND columnName >=beginDate AND columnName < =endDate
        //�����滻 AND columnName between beginDate AND endDate
        //��Ϊ>= �� between and �����õ�����,�����ѯЧ�ʵ���
        public static string GetDateTimeRangeSql(string dateColumn, string timeColumn, decimal startDateTime,decimal endDateTime)
        {


            #region �����ж�
            //�����λ����Ϊ��,���ؿ�
            if (dateColumn == string.Empty) return string.Empty;
            //�����ʼʱ��ͽ���ʱ��Ƿ�(<0),���ؿ�
            if (startDateTime <= 0 || endDateTime <= 0) return string.Empty;
            //�����ʼʱ��ͽ���ʱ����ͬ , ֻȡ��������

            string columnName = string.Format(" {0}*1000000+{1} ", dateColumn, timeColumn);
            if (startDateTime == endDateTime)
            {
                return string.Format(" AND {0} = {1} ", columnName, startDateTime);
            }

            #endregion

            decimal smallDateTime = startDateTime;
            decimal bigDateTime = endDateTime;
            if (startDateTime > endDateTime)
            {
                smallDateTime = endDateTime;
                bigDateTime = startDateTime;
            }

            string returnSql = string.Empty;
            returnSql = string.Format(" AND {0} > {1} AND {0} < {2} ", columnName, FormatHelper.GetRangeStartInt(smallDateTime), FormatHelper.GetRangeEndInt(bigDateTime));
            return returnSql;
        }


        //��ȡCode��Χsql
        //����sql��ʽΪ AND columnName BETWEEN beginCode AND endCode
        public static string GetCodeRangeSql(string columnName, string beginCode,string endCode)
        {

            #region �����ж�

            //�����λ����Ϊ��,���ؿ�
            if (columnName == string.Empty) return string.Empty;
            //�����ʼ��źͽ������Ϊ��,���ؿ�
            if (beginCode == string.Empty || endCode == string.Empty) return string.Empty;
            //�����ʼ��źͽ��������ͬ , ֻȡ��������
            if (beginCode == endCode)
            {
                return string.Format(" AND {0} = '{1}' ", columnName, beginCode);
            }

            #endregion

            string smallCode = beginCode;
            string bigCode = endCode;
            //����˳��Ƚ�
            if (string.Compare(beginCode, endCode, StringComparison.Ordinal) > 0)
            {
                smallCode = endCode;
                bigCode = beginCode;
            }
            //if(beginCode.CompareTo(endCode) == 1)
            //{
            //    smallCode = endCode;
            //    bigCode = beginCode;
            //}

            string returnSql = string.Empty;
            returnSql = string.Format(" AND {0} BETWEEN '{1}' AND '{2}' ", columnName, smallCode, bigCode);
            return returnSql;
        }

        //��ȡRCard��Χsql
        //����sql��ʽΪ AND columnName > beginCode-1 AND columnName < endCode+1 
        //���RCard�����ַ��޷�+1����-1,����sql��ʽΪ AND columnName BETWEEN beginCode AND endCode
        //��Ϊ>= �� between and �����õ�����,�����ѯЧ�ʵ���
        public static string GetRCardRangeSql(string columnName, string beginCode,string endCode)
        {
            #region modified by jessie lee
            if (beginCode != endCode && (beginCode.Length == 0 || endCode.Length == 0))
            {
                beginCode = endCode = beginCode.Length == 0 ? endCode : beginCode;
            }
            #endregion

            #region �����ж�

            //�����λ����Ϊ��,���ؿ�
            if (columnName == string.Empty) return string.Empty;
            //�����ʼ��źͽ������Ϊ��,���ؿ�
            if (beginCode == string.Empty || endCode == string.Empty) return string.Empty;
            //�����ʼ��źͽ��������ͬ , ֻȡ��������
            if (beginCode == endCode)
            {
                return string.Format(" AND {0} = '{1}' ", columnName, beginCode);
            }

            //���RCard�����ַ��޷�+1����-1,����sql��ʽΪ AND columnName BETWEEN beginCode AND endCode
            int lastCharASCII = (int)(Char)beginCode[beginCode.Length - 1];
            if (lastCharASCII < 48 || lastCharASCII > 57)
            {
                return FormatHelper.GetCodeRangeSql(columnName, beginCode, endCode);
            }

            #endregion

            string smallCode = beginCode;
            string bigCode = endCode;
            //����˳��Ƚ�
            if (string.Compare(beginCode, endCode, StringComparison.Ordinal) > 0)
            {
                smallCode = endCode;
                bigCode = beginCode;
            }
            //if(beginCode.CompareTo(endCode) == 1)
            //{
            //    smallCode = endCode;
            //    bigCode = beginCode;
            //}

            string returnSql = string.Empty;
            returnSql = string.Format(" AND {0} > '{1}' AND {0} < '{2}' ", columnName, FormatHelper.GetRangeStartCard(smallCode), FormatHelper.GetRangeEndCard(bigCode));
            return returnSql;
        }

        #region	�ж��õ�ʱ�� +1,-1���� (���·���ֻ���Date*1000000+Time)

        //ʱ��-1
        //�������ΪDate + Time
        public static decimal GetRangeStartInt(decimal startInt)
        {
            return startInt - 1;
        }

        //ʱ��+1
        //�������ΪDate + Time
        public static decimal GetRangeEndInt(decimal startInt)
        {
            return startInt + 1;
        }

        #endregion

        #region �ж��õ��ַ���(�����ֽ�β��)��+1 ,-1����

        //�����ֽ�β���ַ�������,ĩβ��-1
        public static string GetRangeStartCard(string beginCode)
        {
            #region �����ж�
            if (beginCode.Trim() == string.Empty) return string.Empty;
            #endregion

            string dealString = beginCode;
            int brokenPoint = dealString.Length - 2;
            for (int i = dealString.Length - 1; i > 0; i--)
            {
                int ASCII = (int)(Char)dealString[i];
                if (ASCII < 48 || ASCII > 57)
                {
                    //�жϲ��������ַ���λ��(����λ��)
                    brokenPoint = i;
                    break;
                }
            }
            string prefixStr = dealString.Substring(0, brokenPoint + 1);									//��ĸǰ׺
            string postfixStr = dealString.Substring(brokenPoint + 1, dealString.Length - (brokenPoint + 1));	//���ֺ�׺

            if (postfixStr != string.Empty)
            {
                decimal numPost = decimal.Parse(postfixStr);//���ֺ�׺decimal

                if (numPost > 0)
                {
                    decimal num = numPost - 1;
                    postfixStr = num.ToString().PadLeft(postfixStr.Length, '0'); //�����ַ�����
                }
                else
                {
                    //�߽����� ���ĩβ����Ϊ0�����,���µĺ�׺0��������ԭ����һ�� ,�����ﵽ�ַ�����ԭ��С
                    postfixStr = decimal.Zero.ToString().PadRight(dealString.Length - (brokenPoint + 1) - 1, '0');
                }
            }

            return prefixStr + postfixStr;
        }

        //�����ֽ�β���ַ�������,ĩβ��+1
        public static string GetRangeEndCard(string beginCode)
        {
            #region �����ж�
            if (beginCode.Trim() == string.Empty) return string.Empty;
            #endregion

            string dealString = beginCode;
            int brokenPoint = dealString.Length - 2;
            for (int i = dealString.Length - 1; i > 0; i--)
            {
                int ASCII = (int)(Char)dealString[i];
                if (ASCII < 48 || ASCII > 57)
                {
                    //�жϲ��������ַ���λ��(����λ��)
                    brokenPoint = i;
                    break;
                }
            }
            string prefixStr = dealString.Substring(0, brokenPoint + 1);									//��ĸǰ׺
            string postfixStr = dealString.Substring(brokenPoint + 1, dealString.Length - (brokenPoint + 1));	//���ֺ�׺
            if (postfixStr != string.Empty)
            {
                decimal numPost = decimal.Parse(postfixStr);//���ֺ�׺decimal
                if (numPost.ToString() != string.Empty.PadLeft(dealString.Length - (brokenPoint + 1), '9'))
                {
                    decimal num = numPost + 1;
                    postfixStr = num.ToString().PadLeft(postfixStr.Length, '0'); //�����ַ�����
                }
                else
                {
                    //�߽����� ���ĩβ����Ϊ9�����,���µĺ�׺���һ��0 ,�����ﵽ�ַ�����ԭ����
                    postfixStr = numPost.ToString().PadRight(dealString.Length - (brokenPoint + 1) + 1, '0');
                }
            }
            return prefixStr + postfixStr;
        }

        #endregion

        public static void SetSNRangeValue(System.Web.UI.WebControls.TextBox txtStartSnQuery, System.Web.UI.WebControls.TextBox txtEndSnQuery)
        {
            //�����ʼ��źͽ�����Ŷ���Ϊ��,������,����
            if (txtStartSnQuery.Text.Trim() != string.Empty && txtEndSnQuery.Text.Trim() != string.Empty) return;
            //�����ʼ��źͽ�����Ŷ�Ϊ��,������,����
            if (txtStartSnQuery.Text.Trim() == string.Empty && txtEndSnQuery.Text.Trim() == string.Empty) return;
            //�������һ����Ϊ��,����һ��Ϊ��,��ʼ��źͽ�����Ÿ���ͬ��ֵ
            if (txtStartSnQuery.Text.Trim() != string.Empty || txtEndSnQuery.Text.Trim() != string.Empty)
            {
                if (txtStartSnQuery.Text.Trim() != string.Empty) { txtEndSnQuery.Text = txtStartSnQuery.Text; }
                if (txtEndSnQuery.Text.Trim() != string.Empty) { txtStartSnQuery.Text = txtEndSnQuery.Text; }
            }
        }


        public static string GetAllFieldWithDesc(Type domainObjectType, string tableAlias, string[] codeFieldList, string[] descFieldList)
        {

            if (codeFieldList.Length != codeFieldList.Length)
            {
                return tableAlias + ".*";
            }

            string returnValue = " " + DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(domainObjectType).Replace(DomainObjectUtility.GetTableMapAttribute(domainObjectType).TableName, tableAlias) + ",";
            for (int i = 0; i < codeFieldList.Length; i++)
            {
                returnValue = returnValue.Replace(" " + tableAlias + "." + codeFieldList[i] + ",", " " + tableAlias + "." + codeFieldList[i] + " || ' - ' || " + descFieldList[i] + " AS " + codeFieldList[i] + ",");
            }

            returnValue = returnValue.Substring(1, returnValue.Length - 2);

            return returnValue;
        }

        public static string GetLinkTableSQL(string coreTable, string coreTableFiled, string joinTable, string joinTableField)
        {
            return " LEFT OUTER JOIN " + joinTable + " ON " + coreTable + "." + coreTableFiled + " = " + joinTable + "." + joinTableField + " ";
        }

        public static string GetFieldWithDesc(string coreTable, string coreTableFiled, string joinTable, string joinTableField)
        {
            return " " + coreTable + "." + coreTableFiled + " || ' - ' || " + joinTable + "." + joinTableField + " AS " + coreTableFiled + " ";
        }

        public static int GetRecentWeekOfYear()
        {
            int weekOfYear = 0;

            int days = DateTime.Now.DayOfYear;

            int dayofweek = 0;
            //DateTime.Now.DayOfWeek = DayOfWeek.
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dayofweek = 1;
                    break;
                case DayOfWeek.Monday:
                    dayofweek = 2;
                    break;
                case DayOfWeek.Tuesday:
                    dayofweek = 3;
                    break;
                case DayOfWeek.Wednesday:
                    dayofweek = 4;
                    break;
                case DayOfWeek.Thursday:
                    dayofweek = 5;
                    break;
                case DayOfWeek.Friday:
                    dayofweek = 6;
                    break;
                case DayOfWeek.Saturday:
                    dayofweek = 7;
                    break;
                default:
                    dayofweek = 0;
                    break;
            }

            weekOfYear = (days - dayofweek) / 7 + 1;

            return weekOfYear;
        }

        public static int GetRecentWeekOfYear(DateTime dt)
        {
            int weekOfYear = 0;
            int days = dt.DayOfYear;

            int dayofweek = 0;
            //DateTime.Now.DayOfWeek = DayOfWeek.
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dayofweek = 1;
                    break;
                case DayOfWeek.Monday:
                    dayofweek = 2;
                    break;
                case DayOfWeek.Tuesday:
                    dayofweek = 3;
                    break;
                case DayOfWeek.Wednesday:
                    dayofweek = 4;
                    break;
                case DayOfWeek.Thursday:
                    dayofweek = 5;
                    break;
                case DayOfWeek.Friday:
                    dayofweek = 6;
                    break;
                case DayOfWeek.Saturday:
                    dayofweek = 7;
                    break;
                default:
                    dayofweek = 0;
                    break;
            }

            weekOfYear = (days - dayofweek) / 7 + 1;

            return weekOfYear;
        }

        /// <summary>
        /// ʮ����ת��Ϊ36����
        /// </summary>
        /// <param name="input">ʮ�����ַ���</param>
        /// <returns></returns>
        public static string DecTo36(string input)
        {
            long lTmp = long.Parse(input);
            long lTmp1 = lTmp;
            int iLen = 0;
            while (lTmp1 > 0)
            {
                iLen++;
                lTmp1 = lTmp1 / 36;
            }

            int[] iTo = new int[iLen];
            iLen = 0;
            while (lTmp > 0)
            {
                iTo[iLen] = Convert.ToInt32(lTmp % 36);
                lTmp = lTmp / 36;
                iLen++;
            }

            string strValue = string.Empty;
            for (int i = iLen - 1; i >= 0; i--)
            {
                if (iTo[i] <= 9)
                    strValue += iTo[i].ToString();
                else
                    strValue += Convert.ToChar(iTo[i] + 55).ToString();
            }
            return strValue;
        }

        /// <summary>
        /// 36����ת��Ϊʮ����
        /// </summary>
        /// <param name="input">36������</param>
        /// <returns></returns>
        public static long DecFrom36(string input)
        {
            input = input.ToUpper();
            long lTotal = 0;
            for (int i = input.Length; i > 0; i--)
            {
                char c = Convert.ToChar(input.Substring(input.Length - i, 1));
                if (c >= '0' && c <= '9')
                    lTotal += Convert.ToInt64((c - 48) * Math.Pow(36, i - 1));
                else
                    lTotal += Convert.ToInt64((c - 55) * Math.Pow(36, i - 1));
            }
            return lTotal;
        }

        private static System.Globalization.CultureInfo gbCL = null;
        public static string GB2Big5(string gb)
        {
            if (gbCL == null)
                gbCL = new System.Globalization.CultureInfo("zh-CN", false);

            string big5 = Microsoft.VisualBasic.Strings.StrConv(gb, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, gbCL.LCID);
            return big5;
        }

        private static System.Globalization.CultureInfo big5CL;
        public static string Big52GB(string big5)
        {
            if (big5CL == null)
                big5CL = new System.Globalization.CultureInfo("zh-TW", false);

            string gb = Microsoft.VisualBasic.Strings.StrConv(big5, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, big5CL.LCID);
            return gb;
        }

        public static string GetModuleTitle(ControlLibrary.Web.Language.LanguageComponent languageComponent, string moduleCode)
        {
            string returnValue = string.Empty;

            returnValue = languageComponent.GetString("module_" + moduleCode.ToUpper());
            if (returnValue.Trim().Length <= 0)
            {
                returnValue = moduleCode;
            }

            return returnValue;
        }

        public static Dictionary<string, Type> GetReportParameterDic()
        {
            Dictionary<string, Type> returnValue = new Dictionary<string, Type>();

            returnValue.Add("UCWhereConditions1.ddlGoodSemiGoodWhere", typeof(ItemType));
            returnValue.Add("UCWhereConditions1.txtItemCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtMaterialModelCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtMOCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.ddlMOTypeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtOrderNoWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.ddlMOBOMVersionWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtMaterialMachineTypeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtLotNoWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtBigSSCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtSegCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtSSCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtOPCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtResCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.ddlShiftCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.ddlCrewCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.datStartDateWhere", typeof(DateTime));
            returnValue.Add("UCWhereConditions1.datEndDateWhere", typeof(DateTime));
            returnValue.Add("UCWhereConditions1.ddlFirstClassWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.ddlSecondClassWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.ddlThirdClassWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.ddlInputOututWhere", typeof(InputOutputType));
            returnValue.Add("UCWhereConditions1.txtMOMemoWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.ddlNewMassWhere", typeof(MOProductType));
            returnValue.Add("UCWhereConditions1.ddlMaterialExportImportWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtProductionTypeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtOQCLotTypeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtErrorCauseGroupCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtDutyCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtExceptionCodeWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.chbExceptionFlagWhere", typeof(string));
            returnValue.Add("UCWhereConditions1.txtInspectorWhere", typeof(string));

            returnValue.Add("UCGroupConditions1.rblByTimeTypeGroup", typeof(NewReportByTimeType));
            returnValue.Add("UCGroupConditions1.chbCompareGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.rblCompareTypeGroup", typeof(NewReportCompareType));
            returnValue.Add("UCGroupConditions1.rblCompleteTypeGroup", typeof(NewReportCompleteType));
            returnValue.Add("UCGroupConditions1.chbBigSSCodeGroupRequired", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbSegCodeGroupRequired", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbSSCodeGroupRequired", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbOPCodeGroupRequired", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbResCodeGroupRequired", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbGoodSemiGoodGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbInspectorGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbItemCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbMaterialModelCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbMaterialMachineTypeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbMaterialExportImportGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbLotNoGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbProductionTypeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbOQCLotTypeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbMOCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbMOMemoGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbNewMassGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbCrewCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbFirstClassGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbSecondClassGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbThirdClassGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbFacCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbBigSSCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbSegCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbSSCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbOPCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbResCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbExceptionCodeGroup", typeof(bool));
            returnValue.Add("UCGroupConditions1.rblExceptionOrDuty", typeof(NewReportExceptionOrDuty));
            returnValue.Add("UCGroupConditions1.chbExcludeLostManHour", typeof(bool));
            returnValue.Add("UCGroupConditions1.chbIncludeIndirectManHour", typeof(bool));

            returnValue.Add("UCQueryDataType1.rblQueryDataType", typeof(NewReportQueryDataType));

            return returnValue;
        }

        #region ����ʽȡ���к�
        /// <summary>
        /// �����кŵ�ָ���ֶ� ����һ����43check sum ���������ɵı�־λ
        /// </summary>
        /// <param name="rcardsPrefix">���кŵĲ���λ��</param>
        /// <returns></returns>
        public static string GetFormatRcards(string rcardsPrefix)
        {
            string keys = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
            StringDictionary KeyValuePairs = new StringDictionary();
            int stringCount = 0;//���������ַ���Ӧ����ֵ��

            for (int i = 0; i < keys.Length; i++)
            {
                string value = keys.Substring(i, 1).ToString();
                KeyValuePairs.Add(value, i.ToString());
                if (KeyValuePairs.ContainsKey(i.ToString()) == false)
                {
                    KeyValuePairs.Add(i.ToString(), value);
                }
            }
            //KeyValuePairs["Sp"] = "38";  //�ٽ�Sp���
            //KeyValuePairs["38"] = "Sp";

            for (int j = 0; j < rcardsPrefix.Length; j++)
            {
                stringCount += Convert.ToInt32(KeyValuePairs[rcardsPrefix.Substring(j, 1)]);
            }

            stringCount = stringCount % 43; //ȡ����

            return KeyValuePairs[stringCount.ToString()].ToString();
        }
        #endregion 

    }
    #region DBDateTime
    /// <summary>
    /// DBDateTime ��ժҪ˵����
    /// �ļ���:		
    /// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
    /// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
    /// ��������:	20050819 14:21:26
    /// �޸���:Mark Lee
    /// �޸�����:20050819
    /// �� ��:	�������ݿ�ʱ���ʽ
    /// �� ��:	
    /// </summary>
    [Serializable, TableMap("dual", "systimestamp")]
    public class DBDateTime : DomainObject
    {
        public DBDateTime()
        {
        }

        public DBDateTime(DateTime dateTime)
        {
            this.DBDate = int.Parse(dateTime.ToString("yyyyMMdd"));
            this.DBTime = int.Parse(dateTime.ToString("HHmmss"));
        }

        [FieldMapAttribute("DBDate", typeof(int), 100, false)]
        public int DBDate;
        [FieldMapAttribute("DBTime", typeof(int), 100, false)]
        public int DBTime;

        public DateTime DateTime
        {
            get { return FormatHelper.ToDateTime(DBDate, DBTime); }
        }

    }
    #endregion

    #region Updater
    /// <summary>
    /// DBDateTime ��ժҪ˵����
    /// �ļ���:		
    /// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
    /// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
    /// ��������:	20050819 14:21:26
    /// �޸���:Laws Lu
    /// �޸�����:20050823
    /// �� ��:	�Զ�����
    /// �� ��:	
    /// </summary>
    [Serializable, TableMap("TBLCSUPDATER", "CSVERSION,ISAVIABLE",true)]
    public class Updater : DomainObject
    {
        public Updater()
        {
        }

        [FieldMapAttribute("CSVERSION", typeof(string), 30, false)]
        public string CSVersion;
        [FieldMapAttribute("LOCATION", typeof(string), 100, false)]
        public string Location;
        [FieldMapAttribute("LOGINUSER", typeof(string), 30, false)]
        public string LoginUser;
        [FieldMapAttribute("LOGINPASSWORD", typeof(string), 20, false)]
        public string LoginPassword;
        [FieldMapAttribute("ISAVIABLE", typeof(string), 1, false)]
        public int IsAviable;

    }
    #endregion
}
