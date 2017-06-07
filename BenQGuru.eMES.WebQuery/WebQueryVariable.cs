using System;
using System.Collections;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.WebQuery
{
    public class TimingType : IInternalSystemVariable
    {
        private ArrayList _list = new ArrayList();

        public TimingType()
        {
            this._list.Add(TimingType.TimePeriod);
            this._list.Add(TimingType.Shift);
            this._list.Add(TimingType.Day);
            this._list.Add(TimingType.Week);
            this._list.Add(TimingType.Month);
        }

        public const string TimePeriod = "timingtype_timeperiod";
        public const string Shift = "timingtype_shift";
        public const string Day = "timingtype_day";
        public const string Week = "timingtype_week";
        public const string Month = "timingtype_month";

        public static string[] ParserAttributeTimingType(string timingType)
        {
            if (timingType.ToUpper() == TimingType.TimePeriod.ToUpper())
            {
                return new string[] { "TimePeriodCode", "ShiftDay" };
            }
            else if (timingType.ToUpper() == TimingType.Shift.ToUpper())
            {
                return new string[] { "ShiftCode", "ShiftDay" };
            }
            else if (timingType.ToUpper() == TimingType.Day.ToUpper())
            {
                return new string[] { "ShiftDay", "ShiftCode" };
            }
            else if (timingType.ToUpper() == TimingType.Week.ToUpper())
            {
                return new string[] { "Week", "ShiftDay" };
            }
            else if (timingType.ToUpper() == TimingType.Month.ToUpper())
            {
                return new string[] { "Month", "ShiftDay" };
            }
            return null;
        }

        public static string[] ParserAttributeTimingType2(string timingType)
        {
            if (timingType.ToUpper() == TimingType.TimePeriod.ToUpper())
            {
                return new string[] { "TimePeriodCode" };
            }
            else if (timingType.ToUpper() == TimingType.Shift.ToUpper())
            {
                return new string[] { "ShiftCode" };
            }
            else if (timingType.ToUpper() == TimingType.Day.ToUpper())
            {
                return new string[] { "ShiftDay" };
            }
            else if (timingType.ToUpper() == TimingType.Week.ToUpper())
            {
                return new string[] { "Week" };
            }
            else if (timingType.ToUpper() == TimingType.Month.ToUpper())
            {
                return new string[] { "Month" };
            }
            return null;
        }

        public static string ParserAttributeTimingType3(string timingType)
        {
            if (timingType.ToUpper() == TimingType.TimePeriod.ToUpper())
            {
                return "newreportbytimetype_period";
            }
            else if (timingType.ToUpper() == TimingType.Shift.ToUpper())
            {
                return "newreportbytimetype_shift";
            }
            else if (timingType.ToUpper() == TimingType.Day.ToUpper())
            {
                return "newreportbytimetype_shiftday";
            }
            else if (timingType.ToUpper() == TimingType.Week.ToUpper())
            {
                return "newreportbytimetype_week";
            }
            else if (timingType.ToUpper() == TimingType.Month.ToUpper())
            {
                return "newreportbytimetype_month";
            }
            return null;
        }

        public static string[] ParserAttributeTimingType4(string timingType)
        {
            if (timingType.ToUpper() == TimingType.TimePeriod.ToUpper())
            {
                return new string[] { "PeriodCode" };
            }
            else if (timingType.ToUpper() == TimingType.Shift.ToUpper())
            {
                return new string[] { "ShiftCode" };
            }
            else if (timingType.ToUpper() == TimingType.Day.ToUpper())
            {
                return new string[] { "ShiftDay" };
            }
            else if (timingType.ToUpper() == TimingType.Week.ToUpper())
            {
                return new string[] { "Week" };
            }
            else if (timingType.ToUpper() == TimingType.Month.ToUpper())
            {
                return new string[] { "Month" };
            }
            return null;
        }

        #region IInternalSystemVariable ��Ա

        public string Group
        {
            get
            {
                return "TimingType";
            }
        }

        public ArrayList Items
        {
            get
            {
                return this._list;
            }
        }

        #endregion
    }

    public class SummaryTarget : IInternalSystemVariable
    {
        private ArrayList _list = new ArrayList();

        public SummaryTarget()
        {
            this._list.Add(SummaryTarget.Model);
            this._list.Add(SummaryTarget.Item);
            this._list.Add(SummaryTarget.Mo);
            this._list.Add(SummaryTarget.Operation);
            this._list.Add(SummaryTarget.Segment);
            this._list.Add(SummaryTarget.StepSequence);
            this._list.Add(SummaryTarget.Resource);
        }

        public const string Model = "summarytarget_model";
        public const string Item = "summarytarget_item";
        public const string Mo = "summarytarget_mo";
        public const string Operation = "summarytarget_operation";
        public const string Segment = "summarytarget_segment";
        public const string StepSequence = "summarytarget_stepsequence";
        public const string Resource = "summarytarget_resource";

        public static string ParserAttributeSummaryTarget(string summaryTarget)
        {
            if (summaryTarget.ToUpper() == SummaryTarget.Model.ToUpper())
            {
                return "ModelCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Item.ToUpper())
            {
                return "ItemCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Mo.ToUpper())
            {
                return "MOCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Operation.ToUpper())
            {
                return "OperationCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Segment.ToUpper())
            {
                return "SegmentCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.StepSequence.ToUpper())
            {
                return "StepSequenceCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Resource.ToUpper())
            {
                return "ResourceCode";
            }
            return "";
        }

        public static string[] ParserAttributeSummaryTarget2(string summaryTarget)
        {

            //ѡ����֣������֡���Ʒ��ΪOWC�����ֶΣ���ƷĬ�����أ�
            //
            //ѡ���Ʒ������Ʒ����������������ΪOWC�����ֶΣ�������������Ĭ�����أ�
            //
            //ѡ�񹤵�������������������ΪOWC�����ֶΣ�������Ĭ�����أ�
            //
            //ѡ���򣬽�����,���֡���Ʒ��������ΪOWC�����ֶ�
            //
            //ѡ�񹤶Σ�������,���֡���Ʒ��������ΪOWC�����ֶ�
            //
            //ѡ�������ߣ���������,���֡���Ʒ��������ΪOWC�����ֶ�
            //
            //ѡ����Դ������Դ,���֡���Ʒ��������ΪOWC�����ֶ�

            //			cs085
            //			1. ������������ͳ�ƶ���Ϊ���򡢹��Ρ������ߡ���Դʱ����ͳ�ƶ�������֧���¹һ��֡���Ʒ�������Ķ����������
            //			2. ������������ͳ�ƶ���Ϊ����ʱ���¹ҵ���Դ�������������Դ��ȥ����ԭ�򣺿�ͨ��ѡ����Դͳ�ƶ���Ȼ������Դ��ѯ������ɸѡ��Ӧ�������Դ������ʵ�ֶ�һ������鿴������Ӧ����Դ����Ӧ���������
            //			3. ������������ͳ�ƶ���Ϊ����ʱ���¹ҵ���Դ�������������������ȥ����ԭ�򣺿�ͨ��ѡ��������ͳ�ƶ���Ȼ���������߲�ѯ������ɸѡ��Ӧ���ε������ߣ�����ʵ�ֶ�һ�����β鿴������Ӧ�������ߵ���Ӧ���������

            if (summaryTarget.ToUpper() == SummaryTarget.Model.ToUpper())
            {
                return new string[] { "ModelCode", "ItemCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Item.ToUpper())
            {
                return new string[] { "ItemCode", "MoCode", "StepSequenceCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Mo.ToUpper())
            {
                return new string[] { "MoCode", "StepSequenceCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Operation.ToUpper())
            {
                return new string[] { "OperationCode", "ModelCode", "ItemCode", "MoCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Segment.ToUpper())
            {
                return new string[] { "SegmentCode", "ModelCode", "ItemCode", "MoCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.StepSequence.ToUpper())
            {
                return new string[] { "StepSequenceCode", "ModelCode", "ItemCode", "MoCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Resource.ToUpper())
            {
                return new string[] { "ResourceCode", "ModelCode", "ItemCode", "MoCode" };
            }

            return null;
        }

        public static string ParserAttributeSummaryTarget3(string summaryTarget)
        {
            if (summaryTarget.ToUpper() == SummaryTarget.Model.ToUpper())
            {
                return "ModelCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Item.ToUpper())
            {
                return "ItemCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Mo.ToUpper())
            {
                return "MOCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Operation.ToUpper())
            {
                return "OPCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Segment.ToUpper())
            {
                return "SegCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.StepSequence.ToUpper())
            {
                return "SSCode";
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Resource.ToUpper())
            {
                return "ResCode";
            }
            return "";
        }

        public static string[] ParserAttributeSummaryTarget2(string summaryTarget, string yieldCatalog)
        {

            //ѡ����֣������֡���Ʒ��ΪOWC�����ֶΣ���ƷĬ�����أ�
            //
            //ѡ���Ʒ������Ʒ����������������ΪOWC�����ֶΣ�������������Ĭ�����أ�
            //
            //ѡ�񹤵�������������������ΪOWC�����ֶΣ�������Ĭ�����أ�
            //
            //ѡ���򣬽�����,���֡���Ʒ��������ΪOWC�����ֶ�
            //
            //ѡ�񹤶Σ�������,���֡���Ʒ��������ΪOWC�����ֶ�
            //
            //ѡ�������ߣ���������,���֡���Ʒ��������ΪOWC�����ֶ�
            //
            //ѡ����Դ������Դ,���֡���Ʒ��������ΪOWC�����ֶ�

            //			cs085
            //			1. ������������ͳ�ƶ���Ϊ���򡢹��Ρ������ߡ���Դʱ����ͳ�ƶ�������֧���¹һ��֡���Ʒ�������Ķ����������
            //			2. ������������ͳ�ƶ���Ϊ����ʱ���¹ҵ���Դ�������������Դ��ȥ����ԭ�򣺿�ͨ��ѡ����Դͳ�ƶ���Ȼ������Դ��ѯ������ɸѡ��Ӧ�������Դ������ʵ�ֶ�һ������鿴������Ӧ����Դ����Ӧ���������
            //			3. ������������ͳ�ƶ���Ϊ����ʱ���¹ҵ���Դ�������������������ȥ����ԭ�򣺿�ͨ��ѡ��������ͳ�ƶ���Ȼ���������߲�ѯ������ɸѡ��Ӧ���ε������ߣ�����ʵ�ֶ�һ�����β鿴������Ӧ�������ߵ���Ӧ���������

            if (summaryTarget.ToUpper() == SummaryTarget.Model.ToUpper())
            {
                return new string[] { "ModelCode", "ItemCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Item.ToUpper())
            {
                if (yieldCatalog.ToUpper() == YieldCatalog.PPM.ToUpper())
                {
                    return new string[] { "ItemCode" };
                }
                return new string[] { "ItemCode", "MoCode", "StepSequenceCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Mo.ToUpper())
            {
                return new string[] { "MoCode", "StepSequenceCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Operation.ToUpper())
            {
                return new string[] { "OperationCode", "ModelCode", "ItemCode", "MoCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Segment.ToUpper())
            {
                return new string[] { "SegmentCode", "ModelCode", "ItemCode", "MoCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.StepSequence.ToUpper())
            {
                return new string[] { "StepSequenceCode", "ModelCode", "ItemCode", "MoCode" };
            }
            else if (summaryTarget.ToUpper() == SummaryTarget.Resource.ToUpper())
            {
                return new string[] { "ResourceCode", "ModelCode", "ItemCode", "MoCode" };
            }

            return null;
        }

        #region IInternalSystemVariable ��Ա

        public string Group
        {
            get
            {
                return "SummaryTarget";
            }
        }

        public ArrayList Items
        {
            get
            {
                return this._list;
            }
        }

        #endregion
    }

    public class VisibleStyle : IInternalSystemVariable
    {
        private ArrayList _list = new ArrayList();

        public VisibleStyle()
        {
            this._list.Add(VisibleStyle.Pivot);
            this._list.Add(VisibleStyle.Chart);
        }

        public const string Pivot = "visiblestyle_pivot";
        public const string Chart = "visiblestyle_chart";

        #region IInternalSystemVariable ��Ա

        public string Group
        {
            get
            {
                return "VisibleStyle";
            }
        }

        public ArrayList Items
        {
            get
            {
                return this._list;
            }
        }

        #endregion
    }

    public class ChartStyle : IInternalSystemVariable
    {
        private ArrayList _list = new ArrayList();

        public ChartStyle()
        {
            this._list.Add(ChartStyle.Histogram);
            this._list.Add(ChartStyle.Line);
        }

        public const string Histogram = "chartstyle_histogram";
        public const string Line = "chartstyle_line";

        #region IInternalSystemVariable ��Ա

        public string Group
        {
            get
            {
                return "ChartStyle";
            }
        }

        public ArrayList Items
        {
            get
            {
                return this._list;
            }
        }

        #endregion
    }

    public class RMAVisibleStyle : IInternalSystemVariable
    {
        private ArrayList _list = new ArrayList();

        public RMAVisibleStyle()
        {
            this._list.Add(RMAVisibleStyle.Pivot);
            this._list.Add(RMAVisibleStyle.Pie3D);
        }

        public const string Pivot = "RMAVisibleStyle_Pivot";
        public const string Pie3D = "RMAVisibleStyle_Pie3D";

        #region IInternalSystemVariable ��Ա

        public string Group
        {
            get
            {
                return "RMAVisibleStyle";
            }
        }

        public ArrayList Items
        {
            get
            {
                return this._list;
            }
        }

        #endregion
    }


    public class RMAVisibleStyle2 : IInternalSystemVariable
    {
        private ArrayList _list = new ArrayList();

        public RMAVisibleStyle2()
        {
            this._list.Add(RMAVisibleStyle2.Pie);
            this._list.Add(RMAVisibleStyle2.Clusterd);
        }

        public const string Clusterd = "RMAVisibleStyle2_Clusterd";
        public const string Pie = "RMAVisibleStyle2_Pie";

        #region IInternalSystemVariable ��Ա

        public string Group
        {
            get
            {
                return "RMAVisibleStyle2";
            }
        }

        public ArrayList Items
        {
            get
            {
                return this._list;
            }
        }

        #endregion
    }

    public class YieldCatalog : IInternalSystemVariable
    {
        private ArrayList _list = new ArrayList();

        public YieldCatalog()
        {
            this._list.Add(YieldCatalog.NotYield);
            this._list.Add(YieldCatalog.AllGood);
            this._list.Add(YieldCatalog.PPM);
        }

        public const string NotYield = "yieldcatalog_notyield";
        public const string AllGood = "yieldcatalog_allgood";
        public const string PPM = "yieldcatalog_ppm";

        #region IInternalSystemVariable ��Ա

        public string Group
        {
            get
            {
                return "YieldCatalog";
            }
        }

        public ArrayList Items
        {
            get
            {
                return this._list;
            }
        }

        #endregion



        public static string[] ParserTotalFields(string summaryTarget, string yieldCatalog)
        {
            ArrayList list = new ArrayList();
            if (yieldCatalog.ToUpper() == YieldCatalog.NotYield.ToUpper())
            {
                if (summaryTarget.ToUpper() == SummaryTarget.Operation.ToUpper() ||
                    summaryTarget.ToUpper() == SummaryTarget.Resource.ToUpper())
                {
                    list.Add("NGTimes");
                    list.Add("AllTimes");
                    list.Add("NotYieldPercent");
                }
                else
                {
                    list.Add("NGTimes");
                    //list.Add("Quantity");
                    list.Add("InputQuantity");	//��������Ϊ������ʱ��ͳ�ƶ�����֡���Ʒ�����������Ρ������߹�5�ֵļ��㹫ʽ�ķ�ĸ������������ΪͶ����
                    list.Add("NotYieldPercent");
                }
            }
            else if (yieldCatalog.ToUpper() == YieldCatalog.AllGood.ToUpper())
            {
                if (summaryTarget.ToUpper() != SummaryTarget.Operation.ToUpper() &&
                    summaryTarget.ToUpper() != SummaryTarget.Resource.ToUpper())
                {
                    list.Add("AllGoodQuantity");
                    list.Add("Quantity");
                    list.Add("AllGoodYieldPercent");
                }
            }
            else if (yieldCatalog.ToUpper() == YieldCatalog.PPM.ToUpper())
            {
                if (summaryTarget.ToUpper() == SummaryTarget.Item.ToUpper())
                {
                    list.Add("NotYieldLocation");
                    list.Add("TotalLocation");
                    list.Add("PPM");
                }
                else if (summaryTarget.ToUpper() == SummaryTarget.StepSequence.ToUpper())
                {
                    list.Add("NotYieldLocation");
                    list.Add("TotalLocation");
                    list.Add("PPM");
                }
            }
            return (string[])list.ToArray(typeof(string));
        }

        public static string ParserTotalField(string summaryTarget, string yieldCatalog)
        {
            string totalField = "";
            if (yieldCatalog.ToUpper() == YieldCatalog.NotYield.ToUpper())
            {
                totalField = "NotYieldPercent";
            }
            else if (yieldCatalog.ToUpper() == YieldCatalog.AllGood.ToUpper())
            {
                totalField = "AllGoodYieldPercent";
            }
            else if (yieldCatalog.ToUpper() == YieldCatalog.PPM.ToUpper())
            {
                totalField = "PPM";
            }
            return totalField;
        }
    }

    public class TSPerformanceSummaryTarget : IInternalSystemVariable
    {
        private ArrayList _list = new ArrayList();

        public TSPerformanceSummaryTarget()
        {
            this._list.Add(TSPerformanceSummaryTarget.Quantity);
        }

        public const string Quantity = "tssummarytarget_quantity";

        #region IInternalSystemVariable ��Ա

        public string Group
        {
            get
            {
                return "TSSummaryTarget";
            }
        }

        public ArrayList Items
        {
            get
            {
                return this._list;
            }
        }

        #endregion
    }

    public class TSInfoSummaryTarget : IInternalSystemVariable
    {
        private ArrayList _list = new ArrayList();

        public TSInfoSummaryTarget()
        {
            this._list.Add(TSInfoSummaryTarget.ErrorCodeGroup);
            this._list.Add(TSInfoSummaryTarget.ErrorCode);
            this._list.Add(TSInfoSummaryTarget.ErrorCauseGroup);
            this._list.Add(TSInfoSummaryTarget.ErrorCause);
            this._list.Add(TSInfoSummaryTarget.ErrorLocation);
            this._list.Add(TSInfoSummaryTarget.Duty);
            this._list.Add(TSInfoSummaryTarget.Errorcomponent);
        }

        public const string ErrorCodeGroup = "tsinfo_errorcodegroup";
        public const string ErrorCode = "tsinfo_errorcode";
        public const string ErrorCauseGroup = "tsinfo_errorcausegroup";
        public const string ErrorCause = "tsinfo_errorcause";
        public const string ErrorLocation = "tsinfo_errorlocation";
        public const string Duty = "tsinfo_duty";
        public const string Errorcomponent = "tsinfo_errorcomponent";

        #region IInternalSystemVariable ��Ա

        public string Group
        {
            get
            {
                return "TSInfoSummaryTarget";
            }
        }

        public ArrayList Items
        {
            get
            {
                return this._list;
            }
        }

        #endregion
    }

}
