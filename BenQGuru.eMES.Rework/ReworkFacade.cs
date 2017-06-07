using System;
using System.Runtime.Remoting;
using System.Collections;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.Rework;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.OQC;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Domain.TS;
using BenQGuru.eMES.TS;

namespace BenQGuru.eMES.Rework
{
    #region RejectEx - Reject with ErrorCode info

    public class RejectError : DomainObject
    {
        [FieldMapAttribute("ECODE", typeof(string), 40, true)]
        public string ErrorCode;

        [FieldMapAttribute("RCARD", typeof(string), 40, true)]
        public string RunningCard;

        [FieldMapAttribute("RCARDSEQ", typeof(string), 40, true)]
        public int RunningCardSeq;

    }

    public class ReworkRangeQuery : DomainObject
    {
        [FieldMapAttribute("RunningCard", typeof(string), 40, true)]
        public string RunningCard;

        [FieldMapAttribute("LotList", typeof(string), 1000, false)]
        public string LotList;

        [FieldMapAttribute("MoCode", typeof(string), 40, true)]
        public string MoCode;

        [FieldMapAttribute("ItemCode", typeof(string), 40, true)]
        public string ItemCode;

        [FieldMapAttribute("MModelCode", typeof(string), 40, true)]
        public string MModelCode;

        [FieldMapAttribute("MUSER", typeof(string), 40, true)]
        [FieldDisplay(FieldDisplayModifyType.Append, "TBLUSER", "USERCODE", "USERNAME")]

        public string MaintainUser;

        [FieldMapAttribute("MDATE", typeof(int), 8, true)]
        public int MaintainDate;

        [FieldMapAttribute("MTIME", typeof(int), 6, true)]
        public int MaintainTime;

    }


    [Serializable, TableMap("TBLREJECT", "RCARD,RCARDSEQ")]
    public class RejectEx : Reject
    {
        /// <summary>
        /// ErrorCode
        /// </summary>
        [FieldMapAttribute("ECODE", typeof(string), 40, true)]
        public string ErrorCode;

    }

    #endregion

    #region ReworkPassEx - ReworkPass with Department and DisplayStatus
    [Serializable, TableMap("TBLREWORKPASS", "SEQ,ReworkCode")]
    public class ReworkPassEx : ReworkPass
    {
        [FieldMapAttribute("USERDEPART", typeof(string), 40, true)]
        public string UserDepartment;

        [FieldMapAttribute("CSEQ", typeof(decimal), 10, false)]
        public decimal CurrentPassSeq;

        [FieldMapAttribute("REWORKSTATUS", typeof(string), 40, false)]
        public string ReworkStatus;

    }
    #endregion


    /// <summary>
    /// ReworkFacade ��ժҪ˵����
    /// �ļ���:		ReworkFacade.cs
    /// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
    /// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
    /// ��������:	2005-04-20 11:11:55
    /// �޸���:
    /// �޸�����:
    /// �� ��:	
    /// �� ��:	
    /// </summary>
    public class ReworkFacade
    {
        private IDomainDataProvider _domainDataProvider = null;
        private FacadeHelper _helper = null;

        public const string ReworkLot_Status_NEW = "NEW";
        public const string ReworkLot_Status_DEAL = "DEAL";
        public ReworkFacade(IDomainDataProvider domainDataProvider)
        {
            this._domainDataProvider = domainDataProvider;
            this._helper = new FacadeHelper(DataProvider);
        }

        public IDomainDataProvider DataProvider
        {
            get
            {
                if (_domainDataProvider == null)
                {
                    _domainDataProvider = DomainDataProviderManager.DomainDataProvider();
                }

                return _domainDataProvider;
            }
        }



        #region Reject

        /// <summary>
        /// 
        /// </summary>
        public Reject CreateNewReject()
        {
            return new Reject();
        }

        public void AddReject(Reject reject)
        {
            this._helper.AddDomainObject(reject);
        }

        public void UpdateReject(Reject reject)
        {
            this._helper.UpdateDomainObject(reject);
        }

        public void UpdateReject(object[] rejects)
        {
            if (rejects != null)
            {
                try
                {
                    this._domainDataProvider.BeginTransaction();
                    foreach (Reject obj in rejects)
                    {
                        this.UpdateReject(obj);
                    }
                    this._domainDataProvider.CommitTransaction();
                }
                catch (Exception e)
                {
                    this._domainDataProvider.RollbackTransaction();
                    ExceptionManager.Raise(this.GetType(), "$Error_update_fail", e);
                }

            }
        }

        public void DeleteReject(Reject reject)
        {
            this._helper.DeleteDomainObject(reject,
                new ICheck[]{ new DeleteAssociateCheck( reject,
                                this.DataProvider, 
                                new Type[]{
                                              typeof(Reject2ErrorCode)	})});
        }

        public void DeleteReject(Reject[] reject)
        {
            this._helper.DeleteDomainObject(reject,
                new ICheck[]{ new DeleteAssociateCheck( reject,
                                this.DataProvider, 
								new Type[]{
                                              typeof(Reject2ErrorCode)	})});
        }

        public object GetReject(string runningCard, decimal runningCardSequence)
        {
            return this.DataProvider.CustomQuery(typeof(Reject), new SQLCondition(string.Format("select {0} from TBLREJECT where 1=1 and RCARD like '{1}%'  and RCARDSEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject)), runningCard, runningCardSequence)));
        }
        public object GetReject(string runningCard, decimal runningCardSequence,string mocode)
        {
            return this.DataProvider.CustomSearch(typeof(Reject), new object[] { runningCard, runningCardSequence,mocode });
        }

        /// <summary>
        /// ** ��������:	��ѯReject��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <returns> Reject���ܼ�¼��</returns>
        public int QueryRejectCount(string runningCard, decimal runningCardSequence)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREJECT where 1=1 and RCARD like '{0}%'  and RCARDSEQ like '{1}%' ", runningCard, runningCardSequence)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Reject����</returns>
        public object[] QueryReject(string runningCard, decimal runningCardSequence, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(Reject), new PagerCondition(string.Format("select {0} from TBLREJECT where 1=1 and RCARD like '{1}%'  and RCARDSEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject)), runningCard, runningCardSequence), "RCARD,RCARDSEQ", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�Reject
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>Reject���ܼ�¼��</returns>
        public object[] GetAllReject()
        {
            return this.DataProvider.CustomQuery(typeof(Reject), new SQLCondition(string.Format("select {0} from TBLREJECT order by RCARD,RCARDSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject)))));
        }



        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Reject����</returns>
        public object[] QueryReject(string moCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo, int inclusive, int exclusive)
        {
            string sql = string.Format(
                "select {0} from TBLREJECT where 1=1 and MOCODE like '{1}%'  and  OPCODE='{3}' and (REJECTDATE * 1000000 + REJECTTIME) >= {4} and  (REJECTDATE * 1000000 + REJECTTIME) <= {5} ",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject)),
                moCode, lotNo, opCode,
                FormatHelper.TODateInt(dateFrom) * 1000000 + FormatHelper.TOTimeInt(timeFrom),
                FormatHelper.TODateInt(dateTo) * 1000000 + FormatHelper.TOTimeInt(timeTo)
                );

            if (lotNo != string.Empty)
            {
                sql += " and lotno like '" + lotNo + "%' ";
            }
            return this.DataProvider.CustomQuery(
                typeof(Reject),
                new PagerCondition(
                sql,
                "RCARD,RCARDSEQ",
                inclusive, exclusive
                )
                );
        }


        //add by roger.xue on 2008/10/31 for hisense: Get ReworkRangeQuery
        public int GetReworkRangeQueryCount(string reworkSheetCode)
        {
            string sql = string.Format("SELECT COUNT(*) FROM ("
                + "SELECT DISTINCT t1.rcard AS RunningCard, t2.lotlist AS LotList, t2.mocode AS MoCode, t2.itemcode AS ItemCode, "
                + "t3.mmodelcode AS MModelCode, t1.mdate AS MDate, t1.mtime AS MTime, t1.muser AS MUser "
                + "FROM tblreworkrange t1, tblreworksheet t2, tblmaterial t3 "
                + "WHERE t1.reworkcode = t2.reworkcode AND t2.itemcode = t3.mcode "
                + "AND t2.reworkcode = '{0}') ", reworkSheetCode);

            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        public object[] GetReworkRangeQuery(string reworkSheetCode, int inclusive, int exclusive)
        {
            string sql = string.Format("SELECT RunningCard, LotList, MoCode, ItemCode, MModelCode, MDate, MTime, MUser FROM ("
                + "SELECT DISTINCT t1.rcard AS RunningCard, t2.lotlist AS LotList, t2.mocode AS MoCode, t2.itemcode AS ItemCode, "
                + "t3.mmodelcode AS MModelCode, t1.mdate AS MDate, t1.mtime AS MTime, t1.muser AS MUser "
                + "FROM tblreworkrange t1, tblreworksheet t2, tblmaterial t3 "
                + "WHERE t1.reworkcode = t2.reworkcode AND t2.itemcode = t3.mcode "
                + "AND t2.reworkcode = '{0}') ", reworkSheetCode);

            return this.DataProvider.CustomQuery(typeof(ReworkRangeQuery), new PagerCondition(sql, "RunningCard", inclusive, exclusive));

        }

        //end add


        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">�ع����󵥴���,��ȷ��ѯ</param>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������,Ϊ��ʱ����Ϊ����</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Reject����</returns>
        public object[] GetSelectedRejectByReworkSheet(string reworkCode, string moCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo, int inclusive, int exclusive)
        {
            string sql = string.Format(
                "select {0} from TBLREJECT where 1=1 and MOCODE like '{1}%'  and  (RDATE * 1000000 + RTIME) >= {3} and  (RDATE * 1000000 + RTIME) <= {4} and RCARD in ( select RCARD from TBLREWORKRANGE where ReworkCode = '{5}' )",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject)),
                moCode, lotNo,
                this.ToDateTimeDecimal(dateFrom, timeFrom),
                this.ToDateTimeDecimal(dateTo, timeTo),
                reworkCode
                );

            if (lotNo != string.Empty)
            {
                sql += "and tblreject.lotno like '" + lotNo + "%' ";
            }

            if (opCode != string.Empty)
            {
                sql = string.Format("{0} and OPCODE='{1}' ", sql, opCode);
            }

            return this.DataProvider.CustomQuery(
                typeof(Reject),
                new PagerCondition(
                sql,
                "RCARD,RCARDSEQ",
                inclusive, exclusive
                )
                );
        }


        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">�ع����󵥴���,��ȷ��ѯ</param>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������,Ϊ��ʱ����Ϊ����</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Reject����</returns>
        public int GetSelectedRejectByReworkSheetCount(string reworkCode, string moCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo)
        {
            string sql = string.Format(
                "select count(RCARD) from TBLREJECT where 1=1 and MOCODE like '{0}%'   and RCARD in ( select RCARD from TBLREWORKRANGE where ReworkCode = '{2}' )",
                moCode, lotNo,
                reworkCode
                );

            if (dateFrom != null && dateFrom != string.Empty)
            {
                sql += " and (RDATE * 1000000 + RTIME) >=" + this.ToDateTimeDecimal(dateFrom, timeFrom);
            }

            if (dateTo != null && dateTo != string.Empty)
            {
                sql += "and  (RDATE * 1000000 + RTIME) <= " + this.ToDateTimeDecimal(dateTo, timeTo);
            }

            if (lotNo != string.Empty)
            {
                sql += " and TBLREJECT.LOTNO in (" + BenQGuru.eMES.Web.Helper.FormatHelper.ProcessQueryValues(lotNo, true) + ") ";
            }

            if (opCode != string.Empty)
            {
                sql = string.Format("{0} and OPCODE='{1}' ", sql, opCode);
            }

            return this.DataProvider.GetCount(
                new SQLCondition(sql)
                );

        }



        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">�ع����󵥴���,��ȷ��ѯ</param>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������,Ϊ��ʱ����Ϊ����</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Reject����</returns>
        public object[] GetUnSelectedRejectByReworkSheet(string reworkCode, string moCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo, int inclusive, int exclusive)
        {
            string sql = string.Format(
                "select {0} from TBLREJECT where 1=1 and MOCODE like '{1}%'  and (RDATE * 1000000 + RTIME) >= {3} and  (RDATE * 1000000 + RTIME) <= {4} and RCARD not in ( select RCARD from TBLREWORKRANGE where ReworkCode = '{5}' )",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject)),
                moCode, lotNo,
                this.ToDateTimeDecimal(dateFrom, timeFrom),
                this.ToDateTimeDecimal(dateTo, timeTo),
                reworkCode
                );

            if (lotNo != string.Empty)
            {
                sql += " and tblreject.lotno like '" + lotNo + "%' ";
            }
            if (opCode != string.Empty)
            {
                sql = string.Format("{0} and OPCODE='{1}' ", sql, opCode);
            }

            return this.DataProvider.CustomQuery(
                typeof(Reject),
                new PagerCondition(
                sql,
                "RCARD,RCARDSEQ",
                inclusive, exclusive
                )
                );
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">�ع����󵥴���,��ȷ��ѯ</param>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Reject����</returns>
        public int GetUnSelectedRejectByReworkSheetCount(string reworkCode, string moCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo)
        {
            string sql = string.Format(
                "select count(RCARD) from TBLREJECT where 1=1 and MOCODE like '{0}%'  and (RDATE * 1000000 + RTIME) >= {2} and  (RDATE * 1000000 + RTIME) <= {3} and RCARD NOT in ( select RCARD from TBLREWORKRANGE where ReworkCode = '{4}' )",
                moCode, lotNo,
                this.ToDateTimeDecimal(dateFrom, timeFrom),
                this.ToDateTimeDecimal(dateTo, timeTo),
                reworkCode
                );

            if (lotNo != string.Empty)
            {
                sql += "  and TBLREJECT.LOTNO like '" + lotNo + "%' ";
            }

            if (opCode != string.Empty)
            {
                sql = string.Format("{0} and OPCODE='{1}' ", sql, opCode);
            }

            return this.DataProvider.GetCount(
                new SQLCondition(sql)
                );
        }


        /// <summary>
        /// ͨ��RunningCard��StepSequenceCode�ж�Reject��¼�Ƿ����
        /// </summary>
        /// <param name="runningcard">RunningCard</param>
        /// <param name="StepSequenceCode">StepSequenceCode</param>
        /// <returns></returns>
        public bool IsInReject(string runningcard, string StepSequenceCode)
        {
            return (this.GetRejectCount(runningcard, StepSequenceCode) > 0);
        }

        /// <summary>
        /// ͨ��RunningCard��StepSequenceCode�ж�Reject��¼������
        /// </summary>
        /// <param name="runningcard"></param>
        /// <param name="StepSequenceCode"></param>
        /// <returns></returns>
        public int GetRejectCount(string runningcard, string StepSequenceCode)
        {
            return this.DataProvider.GetCount(new SQLParamCondition(
                @"select count(*) 
				 from TBLREJECT where RCARD=$rcard AND SSCODE=$sscode ", new SQLParameter[] { new SQLParameter("rcard", typeof(string), runningcard), new SQLParameter("sscode", typeof(string), StepSequenceCode) }));
        }

        /// <summary>
        /// ͨ��RunningCard��MoCode�ж�Reject��¼������
        /// </summary>
        /// <param name="runningcard"></param>
        /// <param name="StepSequenceCode"></param>
        /// <returns></returns>
        public int GetRejectCountByMO(string runningcard, string moCode)
        {
            return this.DataProvider.GetCount(new SQLParamCondition(
                @"select count(*) 
				 from TBLREJECT where RCARD=$rcard AND MoCode=$moCode ", new SQLParameter[] { new SQLParameter("rcard", typeof(string), runningcard), new SQLParameter("moCode", typeof(string), moCode) }));
        }

        #endregion

        #region RejectEx

        protected string GetErrorCode(string rcard, int rcardSeq)
        {
            string sql = string.Format("select ecode,rcard,rcardseq from tblreject2errorcode where rcard='{0}' and rcardseq={1} ", rcard, rcardSeq);

            object[] objs = this.DataProvider.CustomQuery(typeof(RejectError), new SQLCondition(sql));

            if (objs == null)
            {
                return string.Empty;
            }

            ArrayList ecodes = new ArrayList();
            foreach (RejectError re in objs)
            {
                ecodes.Add(re.ErrorCode);
            }

            string[] codes = (string[])ecodes.ToArray(typeof(string));
            return string.Join(",", codes);


        }
        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="modelCode">��ȷ��ѯ</param>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> RejectEx����</returns>
        public object[] QueryRejectEx(string modelCode, string moCode, string itemCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo, string rejectStatus, int inclusive, int exclusive)
        {
            string sql =
                string.Format(
                "select {0} from TBLREJECT where 1=1 and TBLREJECT.MOCODE like '{1}%' and (TBLREJECT.RDATE * 1000000 + TBLREJECT.RTIME) >= {3} and  (TBLREJECT.RDATE * 1000000 + TBLREJECT.RTIME) <= {4} and TBLREJECT.ITEMCODE like '{5}%' ",
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(Reject)),
                moCode, lotNo,
                this.ToDateTimeDecimal(dateFrom, timeFrom),
                this.ToDateTimeDecimal(dateTo, timeTo),
                itemCode

                );

            if (lotNo != string.Empty)
            {
                sql += "  and TBLREJECT.LOTNO like '" + lotNo + "%' ";
            }


            if (opCode != string.Empty)
            {
                sql += " and TBLREJECT.OPCODE='" + opCode + "'";
            }

            if (rejectStatus != string.Empty)
            {
                sql += " and TBLREJECT.REJSTATUS='" + rejectStatus + "' ";
            }

            if (modelCode != string.Empty)
            {
                sql += " and TBLREJECT.MODELCODE='" + modelCode + "' ";
            }


            object[] rejects = this.DataProvider.CustomQuery(
                typeof(RejectEx),
                new PagerCondition(sql,
                "TBLREJECT.RCARD,TBLREJECT.RCARDSEQ",
                inclusive, exclusive
                )
                );

            if (rejects == null)
            {
                return null;
            }

            for (int i = 0; i < rejects.Length; i++)
            {

                ((RejectEx)rejects[i]).ErrorCode = this.GetErrorCode(((RejectEx)rejects[i]).RunningCard, (int)((RejectEx)rejects[i]).RunningCardSequence);
            }

            return rejects;
        }

        /// <summary>
        /// ** ��������:	��ѯReject��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <returns> Reject���ܼ�¼��</returns>
        public int QueryRejectExCount(string moCode, string itemCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo, string rejectStatus)
        {
            string sql =
                string.Format(
                "select count(RCARD) from TBLREJECT where 1=1 and TBLREJECT.MOCODE like '{0}%'  and (TBLREJECT.RDATE * 1000000 + TBLREJECT.RTIME) >= {2} and  (TBLREJECT.RDATE * 1000000 + TBLREJECT.RTIME) <= {3}  and TBLREJECT.ITEMCODE like '{4}%'  ",
                moCode, lotNo,
                this.ToDateTimeDecimal(dateFrom, timeFrom),
                this.ToDateTimeDecimal(dateTo, timeTo),
                itemCode
                );

            if (lotNo != string.Empty)
            {
                sql += "  and TBLREJECT.LOTNO like '" + lotNo + "%' ";
            }

            if (opCode != string.Empty)
            {
                sql += " and TBLREJECT.OPCODE='" + opCode + "'";
            }

            if (rejectStatus != string.Empty)
            {
                sql += " and TBLREJECT.REJSTATUS='" + rejectStatus + "' ";
            }

            return this.DataProvider.GetCount(
                new SQLCondition(sql)
                );
        }


        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">�ع����󵥴���,��ȷ��ѯ</param>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������,Ϊ��ʱ����Ϊ����</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> RejectEx����</returns>
        public object[] GetSelectedRejectExByReworkSheet(string reworkCode, string moCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo, int inclusive, int exclusive)
        {
            string sql = string.Format(
                "select {0},'' as ecode from TBLREJECT,TBLREWORKSHEET where 1=1 and TBLREJECT.MOCODE like '{1}%' and TBLREJECT.RCARD in ( select RCARD from TBLREWORKRANGE where ReworkCode = '{3}' )  and TBLREJECT.REJSTATUS = '{4}' and TBLREJECT.ITEMCODE = TBLREWORKSHEET.ITEMCODE and TBLREWORKSHEET.REWORKCODE = '{3}' ",
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(Reject)),
                moCode, lotNo,
                reworkCode,
                BenQGuru.eMES.Web.Helper.RejectStatus.Handle
                );


            if (dateFrom != null && dateFrom != string.Empty)
            {
                sql += " and (RDATE * 1000000 + RTIME) >=" + this.ToDateTimeDecimal(dateFrom, timeFrom);
            }

            if (dateTo != null && dateTo != string.Empty)
            {
                sql += "and  (RDATE * 1000000 + RTIME) <= " + this.ToDateTimeDecimal(dateTo, timeTo);
            }

            if (lotNo != string.Empty)
            {
                sql += " and TBLREJECT.LOTNO in (" + BenQGuru.eMES.Web.Helper.FormatHelper.ProcessQueryValues(lotNo, true) + ") ";
            }

            if (opCode != string.Empty)
            {
                sql = string.Format("{0} and TBLREJECT.OPCODE='{1}' ", sql, opCode);
            }

            object[] rejects = this.DataProvider.CustomQuery(
                typeof(RejectEx),
                new PagerCondition(
                sql,
                "TBLREJECT.RCARD,TBLREJECT.RCARDSEQ",
                inclusive, exclusive
                )
                );

            if (rejects == null)
            {
                return null;
            }

            for (int i = 0; i < rejects.Length; i++)
            {

                ((RejectEx)rejects[i]).ErrorCode = this.GetErrorCode(((RejectEx)rejects[i]).RunningCard, (int)((RejectEx)rejects[i]).RunningCardSequence);
            }

            return rejects;

        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">�ع����󵥴���,��ȷ��ѯ</param>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������,Ϊ��ʱ����Ϊ����</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> RejectEx����</returns>
        public int GetSelectedRejectExByReworkSheetCount(string reworkCode, string moCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo)
        {
            return GetSelectedRejectByReworkSheetCount(reworkCode, moCode, lotNo, opCode, dateFrom, timeFrom, dateTo, timeTo);

        }



        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">�ع����󵥴���,��ȷ��ѯ</param>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������,Ϊ��ʱ����Ϊ����</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> RejectEx����</returns>
        public object[] GetUnSelectedRejectExByReworkSheet(string reworkCode, string moCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo, int inclusive, int exclusive)
        {
            string sql = string.Format(
                "select {0},'' as ecode from TBLREJECT,TBLREWORKSHEET where 1=1 and TBLREJECT.MOCODE like '{1}%'   and TBLREJECT.RCARD not in ( select RCARD from TBLREWORKRANGE where ReworkCode = '{3}' )  and TBLREJECT.REJSTATUS = '{4}' and TBLREJECT.ITEMCODE = TBLREWORKSHEET.ITEMCODE and TBLREWORKSHEET.REWORKCODE = '{3}' ",
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(Reject)),
                moCode, lotNo,
                reworkCode,
                BenQGuru.eMES.Web.Helper.RejectStatus.Confirm
                );

            if (dateFrom != null && dateFrom != string.Empty)
            {
                sql += " and (TBLREJECT.RDATE * 1000000 + TBLREJECT.RTIME) >= " + this.ToDateTimeDecimal(dateFrom, timeFrom);
            }
            if (dateTo != null && dateTo != string.Empty)
            {
                sql += " and  (TBLREJECT.RDATE * 1000000 + TBLREJECT.RTIME) <= " + this.ToDateTimeDecimal(dateTo, timeTo);
            }
            if (lotNo != string.Empty)
            {
                sql += " and TBLREJECT.LOTNO in(" + BenQGuru.eMES.Web.Helper.FormatHelper.ProcessQueryValues(lotNo, true) + ") ";
            }

            if (opCode != string.Empty)
            {
                sql = string.Format("{0} and TBLREJECT.OPCODE='{1}' ", sql, opCode);
            }

            object[] rejects = this.DataProvider.CustomQuery(
                typeof(RejectEx),
                new PagerCondition(
                sql,
                "TBLREJECT.RCARD,TBLREJECT.RCARDSEQ",
                inclusive, exclusive
                )
                );

            if (rejects == null)
            {
                return null;
            }

            for (int i = 0; i < rejects.Length; i++)
            {

                ((RejectEx)rejects[i]).ErrorCode = this.GetErrorCode(((RejectEx)rejects[i]).RunningCard, (int)((RejectEx)rejects[i]).RunningCardSequence);
            }

            return rejects;
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReject
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">�ع����󵥴���,��ȷ��ѯ</param>
        /// <param name="moCode">ģ����ѯ,��������</param>
        /// <param name="lotNo">ģ����ѯ,����</param>
        /// <param name="opCode">��ȷ��ѯ,�������</param>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="timeFrom">��ʼʱ��</param>
        /// <param name="dateTo">��������</param>
        /// <param name="timeTo">����ʱ��</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> RejectEx����</returns>
        public int GetUnSelectedRejectExByReworkSheetCount(string reworkCode, string moCode, string lotNo, string opCode, string dateFrom, string timeFrom, string dateTo, string timeTo)
        {
            string sql = string.Format(
                "select count(tblreject.rcard) from TBLREJECT,TBLREWORKSHEET where 1=1 and TBLREJECT.MOCODE like '{1}%' and TBLREJECT.RCARD not in ( select RCARD from TBLREWORKRANGE where ReworkCode = '{3}' )  and TBLREJECT.REJSTATUS = '{4}' and TBLREJECT.ITEMCODE = TBLREWORKSHEET.ITEMCODE and TBLREWORKSHEET.REWORKCODE = '{3}' ",
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(Reject)),
                moCode, lotNo,
                reworkCode,
                BenQGuru.eMES.Web.Helper.RejectStatus.Confirm
                );


            if (dateFrom != null && dateFrom != string.Empty)
            {
                sql += " and (TBLREJECT.RDATE * 1000000 + TBLREJECT.RTIME) >= " + this.ToDateTimeDecimal(dateFrom, timeFrom);
            }
            if (dateTo != null && dateTo != string.Empty)
            {
                sql += " and  (TBLREJECT.RDATE * 1000000 + TBLREJECT.RTIME) <= " + this.ToDateTimeDecimal(dateTo, timeTo);
            }
            if (lotNo != string.Empty)
            {
                sql += " and TBLREJECT.LOTNO in(" + BenQGuru.eMES.Web.Helper.FormatHelper.ProcessQueryValues(lotNo, true) + ") ";
            }

            if (opCode != string.Empty)
            {
                sql = string.Format("{0} and TBLREJECT.OPCODE='{1}' ", sql, opCode);
            }

            return this.DataProvider.GetCount(
                new SQLCondition(
                sql
                )
                );

        }



        #endregion

        #region Reject2ErrorCode
        /// <summary>
        /// 
        /// </summary>
        public Reject2ErrorCode CreateNewReject2ErrorCode()
        {
            return new Reject2ErrorCode();
        }

        public void AddReject2ErrorCode(Reject2ErrorCode reject2ErrorCode)
        {
            this._helper.AddDomainObject(reject2ErrorCode);
        }

        public void UpdateReject2ErrorCode(Reject2ErrorCode reject2ErrorCode)
        {
            this._helper.UpdateDomainObject(reject2ErrorCode);
        }

        public void DeleteReject2ErrorCode(Reject2ErrorCode reject2ErrorCode)
        {
            this._helper.DeleteDomainObject(reject2ErrorCode);
        }

        public void DeleteReject2ErrorCode(Reject2ErrorCode[] reject2ErrorCode)
        {
            this._helper.DeleteDomainObject(reject2ErrorCode);
        }

        public object GetReject2ErrorCode(string errorCode, string runningCard, decimal runningCardSequence, string errorCodeGroup,string mocode)
        {
            return this.DataProvider.CustomSearch(typeof(Reject2ErrorCode), new object[] { errorCode, runningCard, runningCardSequence, errorCodeGroup,mocode });
        }

        public object GetReject2ErrorCode(string errorCode, string runningCard, decimal runningCardSequence, string errorCodeGroup)
        {
            object[] objs = this.QueryReject2ErrorCode(string.Empty, runningCard, runningCardSequence, errorCodeGroup, 1, int.MaxValue);
            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }

            return null;
        }

        public object GetReject2ErrorCode(string runningCard, decimal runningCardSequence)
        {
            object[] objs = this.QueryReject2ErrorCode(string.Empty, runningCard, runningCardSequence, string.Empty, 1, int.MaxValue);
            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }

            return null;
        }

        /// <summary>
        /// ** ��������:	��ѯReject2ErrorCode��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCode">ErrorCode��ģ����ѯ</param>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <param name="errorCodeGroup">ErrorCodeGroup��ģ����ѯ</param>
        /// <returns> Reject2ErrorCode���ܼ�¼��</returns>
        public int QueryReject2ErrorCodeCount(string errorCode, string runningCard, decimal runningCardSequence, string errorCodeGroup)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREJECT2ERRORCODE where 1=1 and ECODE like '{0}%'  and RCARD like '{1}%'  and RCARDSEQ like '{2}%'  and ErrorCodeGroup like '{3}%' ", errorCode, runningCard, runningCardSequence, errorCodeGroup)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReject2ErrorCode
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCode">ErrorCode��ģ����ѯ</param>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <param name="errorCodeGroup">ErrorCodeGroup��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Reject2ErrorCode����</returns>
        public object[] QueryReject2ErrorCode(string errorCode, string runningCard, decimal runningCardSequence, string errorCodeGroup, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(Reject2ErrorCode), new PagerCondition(string.Format("select {0} from TBLREJECT2ERRORCODE where 1=1 and ECODE like '{1}%'  and RCARD like '{2}%'  and RCARDSEQ like '{3}%'  and ErrorCodeGroup like '{4}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject2ErrorCode)), errorCode, runningCard, runningCardSequence, errorCodeGroup), "ECODE,RCARD,RCARDSEQ,ErrorCodeGroup", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�Reject2ErrorCode
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>Reject2ErrorCode���ܼ�¼��</returns>
        public object[] GetAllReject2ErrorCode()
        {
            return this.DataProvider.CustomQuery(typeof(Reject2ErrorCode), new SQLCondition(string.Format("select {0} from TBLREJECT2ERRORCODE order by ECODE,RCARD,RCARDSEQ,ErrorCodeGroup", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject2ErrorCode)))));
        }


        #endregion

        #region ReworkRange
        /// <summary>
        /// 
        /// </summary>
        public ReworkRange CreateNewReworkRange()
        {
            return new ReworkRange();
        }

        public void AddReworkRange(ReworkRange reworkRange)
        {
            this._helper.AddDomainObject(reworkRange);

            // �����˵�״̬��Ϊ Handle 
            Reject reject = (Reject)this.GetReject(reworkRange.RunningCard, reworkRange.RunningCardSequence);
            if (reject != null)
            {
                reject.RejectStatus = RejectStatus.Handle;
                reject.ReworkCode = reworkRange.ReworkCode;
                this.UpdateReject(reject);
            }
        }

        //vizo:�������ظ������ݼ���,��Ҫ��
        public void AddReworkRange(ReworkRange[] reworkRanges)
        {
            try
            {
                this._domainDataProvider.BeginTransaction();

                foreach (ReworkRange range in reworkRanges)
                {
                    try
                    {
                        AddReworkRange(range);
                    }
                    catch
                    {
                    }
                }

                this._domainDataProvider.CommitTransaction();
            }
            catch
            {
                this._domainDataProvider.RollbackTransaction();
            }

        }


        public void UpdateReworkRange(ReworkRange reworkRange)
        {
            this._helper.UpdateDomainObject(reworkRange);
        }

        public void DeleteReworkRange(ReworkRange reworkRange)
        {
            this._helper.DeleteDomainObject(reworkRange);

            // �����˵�״̬��Ϊ Confirm 
            Reject reject = (Reject)this.GetReject(reworkRange.RunningCard, reworkRange.RunningCardSequence);
            reject.RejectStatus = RejectStatus.Confirm;
            reject.ReworkCode = string.Empty;
            this.UpdateReject(reject);

        }

        public void DeleteReworkRange(ReworkRange[] reworkRange)
        {
            try
            {
                this._domainDataProvider.BeginTransaction();
                foreach (ReworkRange range in reworkRange)
                {
                    //try
                    //{
                    DeleteReworkRange(range);
                    //}
                    //catch
                    //{
                    //}
                }
                this._domainDataProvider.CommitTransaction();
            }
            catch (System.Exception ex)
            {
                this._domainDataProvider.RollbackTransaction();
                throw ex;
            }
        }

        public object GetReworkRange(string reworkCode, string runningCard, decimal runningCardSequence)
        {
            return this.DataProvider.CustomSearch(typeof(ReworkRange), new object[] { reworkCode, runningCard, runningCardSequence });
        }

        /// <summary>
        /// ** ��������:	��ѯReworkRange��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode��ģ����ѯ</param>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <returns> ReworkRange���ܼ�¼��</returns>
        public int QueryReworkRangeCount(string reworkCode, string runningCard, decimal runningCardSequence)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREWORKRANGE where 1=1 and ReworkCode like '{0}%'  and RCARD like '{1}%'  and RCARDSEQ like '{2}%' ", reworkCode, runningCard, runningCardSequence)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReworkRange
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode��ģ����ѯ</param>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ReworkRange����</returns>
        public object[] QueryReworkRange(string reworkCode, string runningCard, decimal runningCardSequence, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkRange), new PagerCondition(string.Format("select {0} from TBLREWORKRANGE where 1=1 and ReworkCode like '{1}%'  and RCARD like '{2}%'  and RCARDSEQ like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkRange)), reworkCode, runningCard, runningCardSequence), "ReworkCode,RCARD,RCARDSEQ", inclusive, exclusive));
        }

        public object[] QueryReworkRange(string reworkCode, string runningCard)
        {
            string sql = "SELECT {0} FROM tblreworkrange WHERE reworkcode = '{1}' AND rcard = '{2}' ";
            sql= string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkRange)), reworkCode, runningCard);
            return this.DataProvider.CustomQuery(typeof(ReworkRange), new SQLCondition(sql));
        }

        /// <summary>
        /// ** ��������:	������е�ReworkRange
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-26 17:56:00
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ReworkRange���ܼ�¼��</returns>
        public object[] GetAllReworkRange()
        {
            return this.DataProvider.CustomQuery(typeof(ReworkRange), new SQLCondition(string.Format("select {0} from TBLREWORKRANGE order by ReworkCode,RCARD,RCARDSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkRange)))));
        }

        public object GetLatestReworkRange(string runningCard)
        {
            string sql = "SELECT {0} FROM tblreworkrange WHERE rcard = '{1}' ORDER BY mdate DESC, mtime DESC ";
            sql = string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkRange)), runningCard);

            object[] list = this.DataProvider.CustomQuery(typeof(ReworkRange), new SQLCondition(sql));
            if (list == null)
            {
                return null;
            }
            else
            {
                return list[0];
            }
        }

        #endregion

        #region ReworkCause
        /// <summary>
        /// 
        /// </summary>
        public ReworkCause CreateNewReworkCause()
        {
            return new ReworkCause();
        }

        public void AddReworkCause(ReworkCause reworkCause)
        {
            this._helper.AddDomainObject(reworkCause);
        }

        public void UpdateReworkCause(ReworkCause reworkCause)
        {
            this._helper.UpdateDomainObject(reworkCause);
        }

        public void DeleteReworkCause(ReworkCause reworkCause)
        {
            this._helper.DeleteDomainObject(reworkCause,
                new ICheck[]{ new DeleteAssociateCheck( reworkCause,
                                this.DataProvider, 
                                new Type[]{
                                              typeof(ReworkSheet2Cause)	})});
        }

        public void DeleteReworkCause(ReworkCause[] reworkCause)
        {
            this._helper.DeleteDomainObject(reworkCause,
                new ICheck[]{ new DeleteAssociateCheck( reworkCause,
                                this.DataProvider, 
                                new Type[]{
                                              typeof(ReworkSheet2Cause)	})});
        }

        public object GetReworkCause(string reworkCauseCode)
        {
            return this.DataProvider.CustomSearch(typeof(ReworkCause), new object[] { reworkCauseCode });
        }

        /// <summary>
        /// ** ��������:	��ѯReworkCause��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCauseCode">ReworkCauseCode��ģ����ѯ</param>
        /// <returns> ReworkCause���ܼ�¼��</returns>
        public int QueryReworkCauseCount(string reworkCauseCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREWORKCAUSE where 1=1 and REWORKCCODE like '{0}%' ", reworkCauseCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReworkCause
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCauseCode">ReworkCauseCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ReworkCause����</returns>
        public object[] QueryReworkCause(string reworkCauseCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkCause), new PagerCondition(string.Format("select {0} from TBLREWORKCAUSE where 1=1 and REWORKCCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkCause)), reworkCauseCode), "REWORKCCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�ReworkCause
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ReworkCause���ܼ�¼��</returns>
        public object[] GetAllReworkCause()
        {
            return this.DataProvider.CustomQuery(typeof(ReworkCause), new SQLCondition(string.Format("select {0} from TBLREWORKCAUSE order by REWORKCCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkCause)))));
        }


        #endregion

        #region ReworkPass
        /// <summary>
        /// 
        /// </summary>
        public ReworkPass CreateNewReworkPass()
        {
            return new ReworkPass();
        }

        /// <summary>
        /// ** nunit
        /// </summary>
        /// <param name="reworkPass"></param>
        public void AddReworkPass(ReworkPass reworkPass)
        {
            ReworkSheet reworkSheet = (ReworkSheet)GetReworkSheet(reworkPass.ReworkCode);
            if (reworkSheet.Status != BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_NEW)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_ReworkStatus", string.Format("[$Status={0}]", reworkSheet.Status), null);
            }
            decimal sequence = 0;
            object[] objs = QueryReworkPass(reworkPass.ReworkCode, string.Empty, string.Empty, string.Empty, int.MinValue, int.MaxValue);
            if (objs != null)
            {
                foreach (ReworkPass obj in objs)
                {
                    if (obj.PassSequence == reworkPass.PassSequence && obj.UserCode == reworkPass.UserCode)
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_ReworkPass_Exist");
                    }
                    sequence = System.Math.Max(sequence, obj.Sequence);
                }

                sequence += 1;

            }
            reworkPass.Sequence = sequence;
            this._helper.AddDomainObject(reworkPass);
        }




        public void UpdateReworkPass(ReworkPass reworkPass)
        {
            UpdateReworkPass(reworkPass, true);
        }

        /// <summary>
        /// ** nunit
        /// make if else structure block
        /// </summary>
        /// <param name="reworkPass"></param>
        /// <param name="needCheckStatus"></param>
        public void UpdateReworkPass(ReworkPass reworkPass, bool needCheckStatus)
        {
            ReworkSheet reworkSheet = (ReworkSheet)GetReworkSheet(reworkPass.ReworkCode);
            if (needCheckStatus)
            {
                object oriPass = this.GetReworkPass(reworkPass.ReworkCode, reworkPass.Sequence.ToString());

                if (oriPass == null)
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_ReworkStatus", string.Format("[$Status={0}]", reworkSheet.Status), null);
                }

                if (reworkSheet.Status != BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_NEW)
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_ReworkStatus", string.Format("[$Status={0}]", reworkSheet.Status), null);
                }

                if ((reworkPass.Status != BenQGuru.eMES.Web.Helper.ApproveStatus.APPROVESTATUS_WAITING))
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_ApproveStatus", string.Format("[$Status={0}]", reworkPass.Status), null);
                }

                // ������ͬ������,ͬ����ǩ�˲㼶�Ƿ����
                // �������,�������ǩ�����̲������
                // ����,��������Ǹ���ǩ�����,
                // ����Ҫ��������,��Ϊǩ�˵��˺Ͳ㼶���ǲ����
                object[] objs = QueryReworkPass(reworkPass.ReworkCode, reworkPass.UserCode, reworkPass.PassSequence.ToString(), string.Empty, int.MinValue, int.MaxValue);
                if (objs != null)
                {
                    // ˵����һ����ͬ��ǩ���˺Ͳ㼶,
                    // ����Ҫ�ж�,���������
                    // 1.û�����޸�,ֱ�ӵ���SAVE��ť
                    // 2.�����޸�,�����µĲ㼶�Ѿ������ǩ������
                    // ֻҪ���sequence,������sequence��ͬ��ʱ��
                    // ˵�������2
                    // ��ʱ��Ҫ����
                    // ��ʵ��������,objs������Ӧ��ֻ����һ��Ԫ��
                    ReworkPass rp = (ReworkPass)objs[0];
                    if (rp.Sequence != ((ReworkPass)oriPass).Sequence)
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_ReworkPass_Exist");
                    }
                }
            }


            this._helper.UpdateDomainObject(reworkPass);
        }


        /// <summary>
        /// ** nunit
        /// </summary>
        /// <param name="reworkPass"></param>
        public void DeleteReworkPass(ReworkPass reworkPass)
        {
            ReworkSheet reworkSheet = (ReworkSheet)GetReworkSheet(reworkPass.ReworkCode);
            if (reworkSheet.Status != BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_NEW)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_DeleteCheck", string.Format("[$ReworkSheetStatus={0}]", reworkSheet.Status), null);
            }
            this._helper.DeleteDomainObject(reworkPass);
        }


        /// <summary>
        /// �жϸ��ع����������󵥺�Ϊnew��ʱ��ſ��Ա�ɾ��
        /// </summary>
        /// <param name="reworkPass"></param>
        public void DeleteReworkPass(ReworkPass[] reworkPass)
        {
            try
            {
                this.DataProvider.BeginTransaction();
                for (int i = 0; i < reworkPass.Length; i++)
                {
                    DeleteReworkPass(reworkPass[i]);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_Delete", null, ex);
            }
        }

        public object GetReworkPass(string reworkCode, string reworkSequence)
        {
            return this.DataProvider.CustomSearch(typeof(ReworkPass), new object[] { reworkSequence, reworkCode });
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReworkPass
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        ///                 update by crystal chu
        /// ** �� ��:		2005-03-31 18:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">reworkCode����Ϊ��</param>
        /// <param name="passSequence">passSequence����Ϊ��</param>
        /// <param name="status">status����Ϊ��</param>
        /// <param name="inclusive"></param>
        /// <param name="exclusive"></param>
        /// <returns></returns>
        public int QueryReworkPassCount(string reworkCode, string userCode, string passSequence, string status)
        {
            string selectSql = " select count(*)  from TBLREWORKPASS where 1=1  {0} ";
            string tmpString = string.Empty;
            if ((reworkCode != string.Empty) && (reworkCode.Trim() != string.Empty))
            {
                tmpString += " and reworkcode='" + reworkCode.Trim() + "'";
            }
            if ((userCode != string.Empty) && (userCode.Trim() != string.Empty))
            {
                tmpString += " and usercode='" + userCode.Trim() + "'";
            }
            if ((passSequence != string.Empty) && (passSequence.Trim() != string.Empty))
            {
                tmpString += " and pseq=" + passSequence.Trim();
            }
            if ((status != string.Empty) && (status.Trim() != string.Empty))
            {
                tmpString += " and status='" + status.Trim() + "'";
            }
            return this.DataProvider.GetCount(new SQLCondition(String.Format(selectSql, tmpString)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReworkPass
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        ///                 update by crystal chu
        /// ** �� ��:		2005-03-31 18:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">reworkCode����Ϊ��</param>
        /// <param name="passSequence">passSequence����Ϊ��</param>
        /// <param name="status">status����Ϊ��</param>
        /// <param name="inclusive"></param>
        /// <param name="exclusive"></param>
        /// <returns></returns>
        public object[] QueryReworkPass(string reworkCode, string userCode, string passSequence, string status, int inclusive, int exclusive)
        {
            string selectSql = " select {0}  from TBLREWORKPASS where 1=1  {1} ";
            string tmpString = string.Empty;
            if ((reworkCode != string.Empty) && (reworkCode.Trim() != string.Empty))
            {
                tmpString += " and reworkcode='" + reworkCode.Trim() + "'";
            }
            if ((userCode != string.Empty) && (userCode.Trim() != string.Empty))
            {
                tmpString += " and usercode='" + userCode.Trim() + "'";
            }
            if ((passSequence != string.Empty) && (passSequence.Trim() != string.Empty))
            {
                tmpString += " and pseq=" + passSequence.Trim();
            }
            if ((status != string.Empty) && (status.Trim() != string.Empty))
            {
                tmpString += " and status='" + status.Trim() + "'";
            }
            return this.DataProvider.CustomQuery(typeof(ReworkPass), new PagerCondition(String.Format(selectSql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkPass)), tmpString), "pseq", inclusive, exclusive));
        }



        /// <summary>
        /// ** ��������:	������е�ReworkPass
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ReworkPass���ܼ�¼��</returns>
        public object[] GetAllReworkPass()
        {
            return this.DataProvider.CustomQuery(typeof(ReworkPass), new SQLCondition(string.Format("select {0} from TBLREWORKPASS order by REWORKSEQ,REWORKCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkPass)))));
        }

        public void AddReworkPass(ReworkPass[] reworkPasss)
        {
            this._helper.AddDomainObject(reworkPasss);
        }

        #region deleted code
        /*
                /// <summary>
                /// ���ǩ�˵���Ϣ����������������
                /// 1.�ع����󵥵�״̬����Ϊnew
                /// 2.ǩ��״̬ΪWaiting
                /// 
                /// ��ѯ������"�ȴ�������"ʱ,���������
                /// 1.��ûǩ��,Ҳûǩ��������㼶
                /// 2.��ǩ�˹���,ͨ����,�����Һ��滹����Ҫǩ��
                /// 
                /// ����statusΪ Passed �� Waiting �������ǵȴ�������
                /// </summary>
                /// <param name="userCode"></param>
                /// <param name="approveStatus"></param>
                /// <param name="inclusive"></param>
                /// <param name="exclusive"></param>
                /// <returns></returns>
                public object[] QueryReworkApproveByUser(string userCode,string approveStatus,int inclusive, int exclusive)
                {
                    string selectSql = "select '' as USERDEPART,0 as CSEQ,"
                        + DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(ReworkPass))
                        + " from tblreworkpass where tblreworkpass.reworkcode in ("
                        + " select tblreworksheet.reworkcode from tblreworksheet,tblreworkpass where (tblreworksheet.status = '"
                        + ReworkStatus.REWORKSTATUS_NEW
                        + "' and  tblreworkpass.status = 2 and tblreworkpass.reworkcode = tblreworksheet.reworkcode ) or tblreworksheet.status<>'"
                        + ReworkStatus.REWORKSTATUS_NEW
                        + "' ) {0}";
                    string tmpString = string.Empty;
                    if((userCode != string.Empty)&&(userCode.Trim() != string.Empty))
                    {
                        tmpString += " and tblreworkpass.usercode='"+userCode.Trim()+"'";
                    }

                    if((approveStatus != string.Empty)&&(approveStatus.Trim() !=string.Empty))
                    {
                        if( approveStatus != ApproveStatus.APPROVESTATUS_WAITING_OTHERS.ToString())
                        {
                            tmpString += " and tblreworkpass.status='"+approveStatus.Trim()+"'";
                        }
                    }

                    // ����objs�����п��ܷ��������� ReworkPass,��Ҫ��������"�ȴ�������"״̬
                    object[] objs = this.DataProvider.CustomQuery(typeof(ReworkPassEx),new PagerCondition(String.Format(selectSql,tmpString), "pseq", inclusive, exclusive));

                    if(objs == null)
                    {
                        return null ;
                    }

                    Hashtable ht = new Hashtable() ;
                    ArrayList al = new ArrayList() ;

                    // �� ht ���� reworkSheet��ǰ��ǩ�˲㼶
                    // �����ǩ������,����û�п�ʼǩ��,��ǰǩ�˲㼶Ϊ decimal.maxValue
                    // Ӧ�ò���ײ����
                    foreach(ReworkPassEx obj in objs)
                    {
                        if(!ht.ContainsKey( obj.ReworkCode ))
                        {
                            ht.Add(obj.ReworkCode , 0) ;

                            try
                            {
                                ht[obj.ReworkCode] = (decimal)this.DataProvider.GetCount(new SQLCondition( "select max(seq) from tblreworkpass where status=0 and reworkcode='" + obj.ReworkCode + "' group by reworkcode,status" )) ;
                            }
                            catch
                            {
                                ht[obj.ReworkCode] = decimal.MaxValue ;
                            }

                        }
                        // ��ǰǩ�˵��ĸ��㼶��??
                        obj.CurrentPassSeq = (decimal) ht[ obj.ReworkCode ] ;

                        // �����ѯ�Ĳ���"�ȴ�������"״̬,��ôֻҪ��"ͨ��"״̬��,��"�ȴ�������"״̬��ReworkPassȥ��
                        if( approveStatus != ApproveStatus.APPROVESTATUS_WAITING_OTHERS.ToString())
                        {
                            if(approveStatus != ApproveStatus.APPROVESTATUS_PASSED.ToString())
                            {
                                al.Add( obj) ;
                            }
                            else if(obj.IsPass == IsPass.ISPASS_PASS)
                            {
                                // ���� "ͨ��" ��״̬,������ISPASSֵ��PASSʱ,��ʾ�Ѿ�ȫ��ǩ����,˵�����ǵȴ�������
                                al.Add(obj) ;
                            }
                        }
                        else if(obj.CurrentPassSeq != obj.Sequence && obj.IsPass==IsPass.ISPASS_NOPASS)
                        {
                            // ��ѯ���ǵȴ�������ʱ,ֻҪû��ǩ����(ISPASSֵΪNOTPASS),���Ҳ��ǵ�ǰ�㼶�ͷ�������
                            al.Add(obj) ;
                        }

                    }

                    return (ReworkPassEx[])al.ToArray(typeof(ReworkPassEx)) ;


                }


                public int QueryReworkApproveCountsByUser(string userCode,string approveStatus)
                {
                    object[] objs = QueryReworkApproveByUser(userCode,approveStatus,1,int.MaxValue) ;
                    if(objs == null)
                    {
                        return 0 ;
                    }
                    else
                    {
                        return objs.Length ;
                    }
                }

        */
        #endregion

        /// <summary>
        /// ǩ��ͨ����ɣ�
        /// 1.��ͬ���ǩ��״̬STATUS��Ϊpassed
        /// 2.��������һ�㣬�����ع�����״̬ΪOpen,ISPASS��Ϊ�Ѿ�ǩ��
        /// ** nunit
        /// </summary>
        /// <param name="reworkPass"></param>
        private void PassReworkApprove(ReworkPass reworkPass)
        {
            //�쿴ǩ�ϼ�¼�����Ƿ��Ѿ�����ǩ�϶����ˣ�������򱧴�

            //�쿴ǩ�϶����ع����󵥣���״̬�Ƿ��ǡ��ȴ�������������򱧴�

            //�쿴������Ƿ���Ҫǩ��

            #region Backup
            //����Ƿ����ǩ��
            ApproveCheck(reworkPass);

            object[] objs = null;
            //�õ���Ӧ���ع�����
            ReworkSheet currentReworkSheet = (ReworkSheet)GetReworkSheet(reworkPass.ReworkCode);
            //��ѯ����ع������µ���һ�㼫���е��ع�ǩ�ϼ�¼
            objs = QueryReworkPass(reworkPass.ReworkCode, string.Empty, reworkPass.PassSequence.ToString(), string.Empty, int.MinValue, int.MaxValue);
            //��ѯ����ع�����������ǩ�ϲ㼫
            int maxpseq = this.DataProvider.GetCount(new SQLCondition(String.Format(
                "select max(pseq) from TBLREWORKPASS where reworkcode='{0}'", reworkPass.ReworkCode)));

            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    ((ReworkPass)objs[i]).Status = BenQGuru.eMES.Web.Helper.ApproveStatus.APPROVESTATUS_PASSED;
                    ((ReworkPass)objs[i]).PassUser = reworkPass.PassUser;
                    if (((ReworkPass)objs[i]).Sequence == reworkPass.Sequence)
                    {
                        ((ReworkPass)objs[i]).PassContent = reworkPass.PassContent;
                        ((ReworkPass)objs[i]).IsPass = BenQGuru.eMES.Web.Helper.IsPass.ISPASS_PASS;
                    }
                    this._helper.UpdateDomainObject((DomainObject)objs[i]);
                }
            }

            if (maxpseq == reworkPass.PassSequence)
            {
                if (currentReworkSheet.ReworkType == BenQGuru.eMES.Web.Helper.ReworkType.REWORKTYPE_REMO)
                {
                    // �жϹ����Ƿ���� 
                    if (this.GetMO(currentReworkSheet.MOCode) == null)
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_Rework_MO_Not_Exist");
                    }

                    currentReworkSheet.Status = BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_OPEN;
                }

                if (currentReworkSheet.ReworkType == BenQGuru.eMES.Web.Helper.ReworkType.REWORKTYPE_ONLINE)
                {
                    currentReworkSheet.Status = BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_RELEASE;
                }

                this._helper.UpdateDomainObject(currentReworkSheet);
                objs = QueryReworkPass(reworkPass.ReworkCode, string.Empty, string.Empty, string.Empty, int.MinValue, int.MaxValue);
                for (int i = 0; i < objs.Length; i++)
                {
                    //                    ((ReworkPass)objs[i]).IsPass = BenQGuru.eMES.Web.Helper.IsPass.ISPASS_PASS;
                    this._helper.UpdateDomainObject((DomainObject)objs[i]);
                }
            }
            #endregion
        }


        public void PassReworkApprove(ReworkPass[] reworkPass)
        {
            try
            {
                this.DataProvider.BeginTransaction();
                for (int i = 0; i < reworkPass.Length; i++)
                {
                    PassReworkApprove(reworkPass[i]);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_PassReworkApprove", null, ex);
            }
        }

        /// <summary>
        /// ǩ�˲�ͨ��
        /// 1.�����ع�����״̬ΪNew
        /// 2. ���������ع����󵥵�ǩ��״̬(IsApprove)ΪNoPassed
        /// </summary>
        /// <param name="reworkPass"></param>
        private void NOPassReworkApprove(ReworkPass reworkPass)
        {
            ApproveCheck(reworkPass);
            ReworkSheet currentReworkSheet = (ReworkSheet)GetReworkSheet(reworkPass.ReworkCode);
            currentReworkSheet.Status = BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_NEW;
            this._helper.UpdateDomainObject(currentReworkSheet);

            object[] objs = QueryReworkPass(reworkPass.ReworkCode, string.Empty, reworkPass.PassSequence.ToString(), string.Empty, int.MinValue, int.MaxValue);
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    // vizo: ����Ҫ�ָ���û��ʼǩ��ʱ��״̬

                    ((ReworkPass)objs[i]).PassUser = reworkPass.PassUser;
                    ((ReworkPass)objs[i]).Status = ApproveStatus.APPROVESTATUS_NOPASSED;


                    if (((ReworkPass)objs[i]).Sequence == reworkPass.Sequence)
                    {
                        ((ReworkPass)objs[i]).PassContent = reworkPass.PassContent;
                        ((ReworkPass)objs[i]).IsPass = BenQGuru.eMES.Web.Helper.IsPass.ISPASS_NOPASS;
                    }
                    this._helper.UpdateDomainObject((DomainObject)objs[i]);
                }
            }
        }

        public void NOPassReworkApprove(ReworkPass[] reworkPass)
        {
            try
            {
                this.DataProvider.BeginTransaction();
                for (int i = 0; i < reworkPass.Length; i++)
                {
                    NOPassReworkApprove(reworkPass[i]);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_PassReworkApprove", null, ex);
            }
        }

        /// <summary>
        /// ǩ�˼�����������������
        /// 1.�ع����󵥵�״̬����Ϊwaiting
        /// 2.ǩ��״̬������waiting
        /// 3.��ǰǩ����ǰ���ǩ���˱����Ѿ����ǩ��
        /// </summary>
        /// <param name="reworkPass"></param>
        private void ApproveCheck(ReworkPass reworkPass)
        {
            //ȡ���ع�����
            ReworkSheet reworkSheet = (ReworkSheet)GetReworkSheet(reworkPass.ReworkCode);
            //����ع�����״̬��Ϊ�ȴ������׳��쳣������ǩ���Ѿ�����
            if (reworkSheet.Status != BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_WAITING)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_ReworkStatus", string.Format("[$Status={0}]", reworkSheet.Status), null);
            }
            //����ع�ǩ�ϵ�״̬��Ϊ�ȴ������׳��쳣������ǩ���Ѿ�������
            if (reworkPass.Status != BenQGuru.eMES.Web.Helper.ApproveStatus.APPROVESTATUS_WAITING)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_ApproveStatus", string.Format("[$Status={0}]", reworkPass.Status), null);
            }
            //��preqС�ڵ�ǰ�Ĳ���е������ˣ�û�л�û��ǩ�ϵļ�¼��
            string selectSql =
                " select " +
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkPass)) +
                " from " +
                " tblreworkpass " +
                " where " +
                " reworkcode not in ( " +
                " select " +
                " reworkcode " +
                " from " +
                " tblreworksheet " +
                " where " +
                " status = '" +
                BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_NEW +
                "' ) and  " +
                " reworkcode = '" + reworkPass.ReworkCode + "'  and  " +
                " pseq < " + reworkPass.PassSequence +
                " and status <> " + BenQGuru.eMES.Web.Helper.ApproveStatus.APPROVESTATUS_PASSED;

            object[] objs = this.DataProvider.CustomQuery(typeof(ReworkPass), new PagerCondition(selectSql, "pseq", int.MinValue, int.MaxValue));
            if (objs != null)	//�鲻����˵����ǩ�ϵĶ�ǩ�Ϲ���
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Approve", string.Format("[$UserCode={0}]", reworkPass.PassUser), null);
            }
        }


        #region User --> ReworkSheet
        /// <summary>
        /// ** ��������:	��UserCode���ReworkSheet
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userCode">UserCode,��ȷ��ѯ</param>
        /// <returns>ReworkSheet����</returns>
        public object[] GetReworkSheetByUserCode(string userCode)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet), new SQLCondition(string.Format("select {0} from TBLREWORKSHEET where REWORKCODE in ( select REWORKCODE from TBLREWORKPASS where USERCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)), userCode)));
        }

        /// <summary>
        /// ** ��������:	��UserCode�������User��ReworkSheet������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userCode">UserCode,��ȷ��ѯ</param>
        /// <param name="reworkCode">ReworkCode,ģ����ѯ</param>
        /// <returns>ReworkSheet������</returns>
        public int GetSelectedReworkSheetByUserCodeCount(string userCode, string reworkCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREWORKPASS where USERCODE ='{0}' and REWORKCODE like '{1}%'", userCode, FormatHelper.PKCapitalFormat(reworkCode))));
        }

        /// <summary>
        /// ** ��������:	��UserCode�������User��ReworkSheet����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userCode">UserCode,��ȷ��ѯ</param>
        /// <param name="reworkCode">ReworkCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>ReworkSheet����</returns>
        public object[] GetSelectedReworkSheetByUserCode(string userCode, string reworkCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet),
                new PagerCondition(string.Format("select {0} from TBLREWORKSHEET where REWORKCODE in ( select REWORKCODE from TBLREWORKPASS where USERCODE ='{1}') and REWORKCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)), userCode, FormatHelper.PKCapitalFormat(reworkCode)), "REWORKCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	��UserCode��ò�����User��ReworkSheet������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userCode">UserCode,��ȷ��ѯ</param>
        /// <param name="reworkCode">ReworkCode,ģ����ѯ</param>
        /// <returns>ReworkSheet������</returns>
        public int GetUnselectedReworkSheetByUserCodeCount(string userCode, string reworkCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREWORKSHEET where REWORKCODE not in ( select REWORKCODE from TBLREWORKPASS where USERCODE ='{0}') and REWORKCODE like '{1}%'", userCode, FormatHelper.PKCapitalFormat(reworkCode))));
        }

        /// <summary>
        /// ** ��������:	��UserCode��ò�����User��ReworkSheet����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userCode">UserCode,��ȷ��ѯ</param>
        /// <param name="reworkCode">ReworkCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>ReworkSheet����</returns>
        public object[] GetUnselectedReworkSheetByUserCode(string userCode, string reworkCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet),
                new PagerCondition(string.Format("select {0} from TBLREWORKSHEET where REWORKCODE not in ( select REWORKCODE from TBLREWORKPASS where USERCODE ='{1}') and REWORKCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)), userCode, FormatHelper.PKCapitalFormat(reworkCode)), "REWORKCODE", inclusive, exclusive));
        }
        #endregion

        #region ReworkSheet --> User
        /// <summary>
        /// ** ��������:	��ReworkCode���User
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <returns>User����</returns>
        public object[] GetUserByReworkCode(string reworkCode)
        {
            return this.DataProvider.CustomQuery(typeof(User), new SQLCondition(string.Format("select {0} from TBLUSER where USERCODE in ( select USERCODE from TBLREWORKPASS where REWORKCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(User)), reworkCode)));
        }

        /// <summary>
        /// ** ��������:	��ReworkCode�������ReworkSheet��User������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <param name="userCode">UserCode,ģ����ѯ</param>
        /// <returns>User������</returns>
        public int GetSelectedUserByReworkCodeCount(string reworkCode, string userCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREWORKPASS where REWORKCODE ='{0}' and USERCODE like '{1}%'", reworkCode, FormatHelper.PKCapitalFormat(userCode))));
        }

        /// <summary>
        /// ** ��������:	��ReworkCode�������ReworkSheet��User����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <param name="userCode">UserCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>User����</returns>
        public object[] GetSelectedUserByReworkCode(string reworkCode, string userCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(User),
                new PagerCondition(string.Format("select {0} from TBLUSER where USERCODE in ( select USERCODE from TBLREWORKPASS where REWORKCODE ='{1}') and USERCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(User)), reworkCode, FormatHelper.PKCapitalFormat(userCode)), "USERCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	��ReworkCode��ò�����ReworkSheet��User������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <param name="userCode">UserCode,ģ����ѯ</param>
        /// <returns>User������</returns>
        public int GetUnselectedUserByReworkCodeCount(string reworkCode, string userCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLUSER where USERCODE not in ( select USERCODE from TBLREWORKPASS where REWORKCODE ='{0}') and USERCODE like '{1}%'", reworkCode, FormatHelper.PKCapitalFormat(userCode))));
        }

        /// <summary>
        /// ** ��������:	��ReworkCode��ò�����ReworkSheet��User����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-03-31 13:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <param name="userCode">UserCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>User����</returns>
        public object[] GetUnselectedUserByReworkCode(string reworkCode, string userCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(User),
                new PagerCondition(string.Format("select {0} from TBLUSER where USERCODE not in ( select USERCODE from TBLREWORKPASS where REWORKCODE ='{1}') and USERCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(User)), reworkCode, FormatHelper.PKCapitalFormat(userCode)), "USERCODE", inclusive, exclusive));
        }
        #endregion


        #endregion

        #region ReworkPassEx
        /// <summary>
        /// ** ��������:	��ҳ��ѯReworkPassEx
        /// ** �� ��:		vizo fan
        /// ** �� ��:		2005-05-13 18:22:48
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">reworkCode����Ϊ��</param>
        /// <param name="userCode"></param>
        /// <param name="passSequence">passSequence����Ϊ��</param>
        /// <param name="status">status����Ϊ��</param>
        /// <param name="inclusive"></param>
        /// <param name="exclusive"></param>
        /// <returns></returns>
        public object[] QueryReworkPassEx(string reworkCode, string userCode, string passSequence, string status, int inclusive, int exclusive)
        {
            string sql = " select {0},TBLUSER.USERDEPART,0 as CSEQ,TBLREWORKSHEET.STATUS as REWORKSTATUS from TBLREWORKPASS,TBLUSER,TBLREWORKSHEET where 1=1  {1} ";
            string tmpString = string.Empty;
            if ((reworkCode != string.Empty) && (reworkCode.Trim() != string.Empty))
            {
                tmpString += " and TBLREWORKPASS.reworkcode='" + reworkCode.Trim() + "'";
            }
            if ((userCode != string.Empty) && (userCode.Trim() != string.Empty))
            {
                tmpString += " and TBLREWORKPASS.usercode='" + userCode.Trim() + "'";
            }
            if ((passSequence != string.Empty) && (passSequence.Trim() != string.Empty))
            {
                tmpString += " and TBLREWORKPASS.pseq=" + passSequence.Trim();
            }
            if ((status != string.Empty) && (status.Trim() != string.Empty))
            {
                tmpString += " and TBLREWORKPASS.status='" + status.Trim() + "'";
            }

            tmpString += " and TBLREWORKPASS.usercode=TBLUSER.USERCODE";

            sql = String.Format(sql, DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(ReworkPass)), tmpString);
            object[] objs = this.DataProvider.CustomQuery(typeof(ReworkPassEx), new PagerCondition(sql, "pseq", inclusive, exclusive));
            if (objs == null)
            {
                return null;
            }

            Hashtable ht = new Hashtable();
            ArrayList al = new ArrayList();

            // �� ht ���� reworkSheet��ǰ��ǩ�˲㼶
            // �����ǩ������,����û�п�ʼǩ��,��ǰǩ�˲㼶Ϊ decimal.maxValue
            // Ӧ�ò���ײ����
            foreach (ReworkPassEx obj in objs)
            {
                if (!ht.ContainsKey(obj.ReworkCode))
                {
                    ht.Add(obj.ReworkCode, 0);

                    try
                    {
                        ht[obj.ReworkCode] = (decimal)this.DataProvider.GetCount(new SQLCondition("select max(seq) from tblreworkpass where status=0 and reworkcode='" + obj.ReworkCode + "' group by reworkcode,status"));
                    }
                    catch
                    {
                        ht[obj.ReworkCode] = decimal.MaxValue;
                    }

                }
                // ��ǰǩ�˵��ĸ��㼶��??
                obj.CurrentPassSeq = (decimal)ht[obj.ReworkCode];

                // �����ѯ�Ĳ���"�ȴ�������"״̬,��ôֻҪ��"ͨ��"״̬��,��"�ȴ�������"״̬��ReworkPassȥ��
                if (status != ApproveStatus.APPROVESTATUS_WAITING_OTHERS.ToString())
                {
                    if (status != ApproveStatus.APPROVESTATUS_PASSED.ToString())
                    {
                        al.Add(obj);
                    }
                    else if (obj.IsPass == IsPass.ISPASS_PASS)
                    {
                        // ���� "ͨ��" ��״̬,������ISPASSֵ��PASSʱ,��ʾ�Ѿ�ȫ��ǩ����,˵�����ǵȴ�������
                        al.Add(obj);
                    }
                }
                else if (obj.CurrentPassSeq != obj.PassSequence && obj.IsPass == IsPass.ISPASS_NOPASS)
                {
                    // ��ѯ���ǵȴ�������ʱ,ֻҪû��ǩ����(ISPASSֵΪNOTPASS),���Ҳ��ǵ�ǰ�㼶�ͷ�������
                    al.Add(obj);
                }

            }

            return (ReworkPassEx[])al.ToArray(typeof(ReworkPassEx));
        }


        /// <summary>
        /// ��ǩ����Ϣ���г����Ĺ���,�õ���������Ҫ�ٴ�������������һ��
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <param name="userCode"></param>
        /// <param name="passSequence"></param>
        /// <param name="status"></param>
        /// <param name="reworkStatus"></param>
        /// <param name="showNewReworkSheet">�Ƿ���ʾ�µ�,δǩ�˹���������Ϣ</param>
        /// <param name="showNotApproved"></param>
        /// <param name="inclusive"></param>
        /// <param name="exclusive"></param>
        /// <returns></returns>
        private object[] GetReworkApprove(string reworkCode, string userCode, string passSequence, string status, string reworkStatus, bool showNewReworkSheet, bool showNotApproved, int inclusive, int exclusive)
        {
            string sql = " select {0},TBLUSER.USERDEPART,TBLREWORKSHEET.STATUS as REWORKSTATUS,0 as CSEQ from TBLREWORKPASS,TBLUSER,TBLREWORKSHEET where 1=1  {1} ";
            string tmpString = string.Empty;

            if ((reworkCode != string.Empty) && (reworkCode.Trim() != string.Empty))
            {
                tmpString += " and TBLREWORKPASS.reworkcode='" + reworkCode.Trim() + "'";
            }

            if ((userCode != string.Empty) && (userCode.Trim() != string.Empty))
            {
                tmpString += " and TBLREWORKPASS.usercode='" + userCode.Trim() + "'";
            }

            if ((passSequence != string.Empty) && (passSequence.Trim() != string.Empty))
            {
                tmpString += " and TBLREWORKPASS.pseq=" + passSequence.Trim();
            }

            if ((status != string.Empty) && (status.Trim() != string.Empty))
            {
                if (status != ApproveStatus.APPROVESTATUS_WAITING_OTHERS.ToString())
                {
                    tmpString += " and TBLREWORKPASS.status='" + status.Trim() + "'";
                }
            }

            if (!showNewReworkSheet)
            {
                tmpString += " and TBLREWORKPASS.REWORKCODE in (select tblreworksheet.reworkcode from tblreworksheet,tblreworkpass where tblreworkpass.reworkcode = tblreworksheet.reworkcode and ( (tblreworksheet.status = '" + ReworkStatus.REWORKSTATUS_NEW + "' and tblreworkpass.status <> '0') or tblreworksheet.status <>  '" + ReworkStatus.REWORKSTATUS_NEW + "'))";
            }


            tmpString += " and TBLREWORKPASS.usercode=TBLUSER.USERCODE";
            tmpString += " and TBLREWORKPASS.REWORKCODE=TBLREWORKSHEET.REWORKCODE";

            sql = String.Format(sql, DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(ReworkPass)), tmpString);

            object[] objs = this.DataProvider.CustomQuery(typeof(ReworkPassEx), new PagerCondition(sql, "TBLREWORKPASS.ReworkCode,TBLREWORKPASS.pseq", inclusive, exclusive));

            return objs;
        }


        public object[] QueryReworkApprove(string reworkCode, string userCode, string passSequence, string status, string reworkStatus, bool showNewReworkSheet, bool showNotApproved, int inclusive, int exclusive)
        {
            object[] objs = GetReworkApprove(reworkCode, userCode, passSequence, status, reworkStatus, showNewReworkSheet, showNotApproved, inclusive, exclusive);

            if (objs == null)
            {
                return null;
            }

            Hashtable ht = new Hashtable();
            ArrayList al = new ArrayList();


            // �� ht ���� reworkSheet��ǰ��ǩ�˲㼶
            // �����ǩ������,����û�п�ʼǩ��,��ǰǩ�˲㼶Ϊ decimal.maxValue
            // Ӧ�ò���ײ����
            foreach (ReworkPassEx obj in objs)
            {
                if (!ht.ContainsKey(obj.ReworkCode))
                {
                    ht.Add(obj.ReworkCode, 0);

                    try
                    {
                        ht[obj.ReworkCode] = (decimal)this.DataProvider.GetCount(new SQLCondition("select min(pseq) from tblreworkpass where status=0 and reworkcode='" + obj.ReworkCode + "' group by reworkcode,status"));
                    }
                    catch
                    {
                        ht[obj.ReworkCode] = decimal.MinValue;
                    }

                }
                // ��ǰǩ�˵��ĸ��㼶��??
                obj.CurrentPassSeq = (decimal)(ht[obj.ReworkCode]);


                // ���showNotApprovedΪtrue,��ʾ��ʾ�㼶��ǩ���ò㼶δǩ�˵���
                if (!showNotApproved
                    && obj.IsPass == IsPass.ISPASS_NOTACTION
                    && obj.Status != ApproveStatus.APPROVESTATUS_WAITING
                    )
                {
                    continue;
                }

                // �����ѯ�Ĳ���"�ȴ�������"״̬,��ôֻҪ��"ͨ��"״̬��,��"�ȴ�������"״̬��ReworkPassȥ��

                if (status == ApproveStatus.APPROVESTATUS_WAITING_OTHERS.ToString())
                {
                    if (!(obj.CurrentPassSeq != obj.PassSequence
                        && obj.ReworkStatus == ReworkStatus.REWORKSTATUS_WAITING
                        ))
                    {
                        continue;
                    }
                }

                if (status == ApproveStatus.APPROVESTATUS_WAITING.ToString())
                {
                    if (!(obj.CurrentPassSeq == obj.PassSequence))
                    {
                        continue;
                    }
                }

                if (status == ApproveStatus.APPROVESTATUS_NOPASSED.ToString())
                {
                    if (!(obj.ReworkStatus == ReworkStatus.REWORKSTATUS_NEW
                        && obj.IsPass == IsPass.ISPASS_NOPASS)
                        )
                    {
                        continue;
                    }
                }

                if (status == ApproveStatus.APPROVESTATUS_PASSED.ToString())
                {
                    if (!(obj.IsPass == IsPass.ISPASS_PASS
                        && obj.ReworkStatus != ReworkStatus.REWORKSTATUS_WAITING))
                    {
                        continue;
                    }
                }

                al.Add(obj);


            }

            return (ReworkPassEx[])al.ToArray(typeof(ReworkPassEx));
        }

        public int QueryReworkApproveCount(string reworkCode, string userCode, string passSequence, string status, string reworkStatus, bool showNewReworkSheet, bool showNotApproved)
        {
            object[] objs = QueryReworkApprove(reworkCode, userCode, passSequence, status, reworkStatus, showNewReworkSheet, showNotApproved, 1, int.MaxValue);
            if (objs == null)
            {
                return 0;
            }

            return objs.Length;
        }


        public object[] QueryReworkApprover(string reworkCode, string userCode, string passSequence, string status, string reworkStatus, bool showNewReworkSheet, bool showNotApproved, int inclusive, int exclusive)
        {
            object[] objs = GetReworkApprove(reworkCode, userCode, passSequence, status, reworkStatus, showNewReworkSheet, showNotApproved, inclusive, exclusive);

            if (objs == null)
            {
                return null;
            }

            Hashtable ht = new Hashtable();
            ArrayList al = new ArrayList();


            // �� ht ���� reworkSheet��ǰ��ǩ�˲㼶
            // �����ǩ������,����û�п�ʼǩ��,��ǰǩ�˲㼶Ϊ decimal.maxValue
            // Ӧ�ò���ײ����
            foreach (ReworkPassEx obj in objs)
            {
                if (!ht.ContainsKey(obj.ReworkCode))
                {
                    ht.Add(obj.ReworkCode, 0);

                    try
                    {
                        ht[obj.ReworkCode] = (decimal)this.DataProvider.GetCount(new SQLCondition("select min(pseq) from tblreworkpass where status=0 and reworkcode='" + obj.ReworkCode + "' group by reworkcode,status"));
                    }
                    catch
                    {
                        ht[obj.ReworkCode] = decimal.MinValue;
                    }

                }
                // ��ǰǩ�˵��ĸ��㼶��??
                obj.CurrentPassSeq = (decimal)(ht[obj.ReworkCode]);


                // ���showNotApprovedΪtrue,��ʾ��ʾ�㼶��ǩ���ò㼶δǩ�˵���
                if (!showNotApproved
                    && obj.IsPass == IsPass.ISPASS_NOTACTION
                    && obj.Status != ApproveStatus.APPROVESTATUS_WAITING
                    )
                {
                    continue;
                }

                // �����ѯ�Ĳ���"�ȴ�������"״̬,��ôֻҪ��"ͨ��"״̬��,��"�ȴ�������"״̬��ReworkPassȥ��

                if (status == ApproveStatus.APPROVESTATUS_WAITING_OTHERS.ToString())
                {
                    if (!(obj.CurrentPassSeq != obj.PassSequence
                        && obj.ReworkStatus == ReworkStatus.REWORKSTATUS_WAITING
                        ))
                    {
                        continue;
                    }
                }

                if (status == ApproveStatus.APPROVESTATUS_WAITING.ToString())
                {
                    if (obj.Status != ApproveStatus.APPROVESTATUS_WAITING)
                    {
                        continue;
                    }
                }

                if (status == ApproveStatus.APPROVESTATUS_NOPASSED.ToString())
                {
                    if (obj.Status != ApproveStatus.APPROVESTATUS_NOPASSED)
                    {
                        continue;
                    }
                }

                if (status == ApproveStatus.APPROVESTATUS_PASSED.ToString())
                {
                    if (obj.Status != ApproveStatus.APPROVESTATUS_PASSED)
                    {
                        continue;
                    }
                }

                al.Add(obj);


            }

            return (ReworkPassEx[])al.ToArray(typeof(ReworkPassEx));
        }

        public int QueryReworkApproverCount(string reworkCode, string userCode, string passSequence, string status, string reworkStatus, bool showNewReworkSheet, bool showNotApproved)
        {
            object[] objs = QueryReworkApprover(reworkCode, userCode, passSequence, status, reworkStatus, showNewReworkSheet, showNotApproved, 1, int.MaxValue);
            if (objs == null)
            {
                return 0;
            }

            return objs.Length;
        }

        #endregion

        #region ReworkSheet
        /// <summary>
        /// 
        /// </summary>
        public ReworkSheet CreateNewReworkSheet()
        {
            return new ReworkSheet();
        }

        public ReworkSheet GetMaxReworkSheet(int currentDate)
        {
            string sql = "SELECT NVL(MAX(reworkcode),'R" + currentDate.ToString() + "000') as reworkcode FROM tblreworksheet";
            sql += " WHERE reworkcode BETWEEN 'R" + currentDate.ToString() + "001' AND 'R" + currentDate.ToString() + "999'";

            return this.DataProvider.CustomQuery(typeof(ReworkSheet), new SQLCondition(sql))[0] as ReworkSheet;
        }

        protected string GetNormalRouteByMOCode(string moCode)
        {
            string sql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO2Route)) + " from tblmo2route where ismroute ='1'  ";

            sql += " and mocode = '" + moCode.Trim() + "'";

            object[] objs = this.DataProvider.CustomQuery(typeof(MO2Route), new SQLCondition(sql));

            if (objs == null)
            {
                ExceptionManager.Raise(this.GetType(), "$ERROR_MO_ROUTE_IS_EMPTY");
            }
            if (objs[0] == null)
            {
                ExceptionManager.Raise(this.GetType(), "$ERROR_MO_ROUTE_IS_EMPTY");
            }
            return ((MO2Route)objs[0]).RouteCode;
        }

        public void AddReworkSheetWithOutTrans(ReworkSheet reworkSheet)
        {
            if (reworkSheet.ReworkType == ReworkType.REWORKTYPE_REMO)
            {
                string sql = string.Format(
                    "update tblreject set rejstatus='{0}',reworkcode='{2}' where mocode = '{1}' ",
                    RejectStatus.Handle,
                    reworkSheet.MOCode,
                    reworkSheet.ReworkCode
                    );
                this.DataProvider.CustomExecute(new SQLCondition(sql));
                reworkSheet.NewRouteCode = GetNormalRouteByMOCode(reworkSheet.MOCode);
                reworkSheet.NewRouteCode = "";
            }

            this._helper.AddDomainObject(reworkSheet);
        }

        public void AddReworkSheet(ReworkSheet reworkSheet)
        {

            this.DataProvider.BeginTransaction();


            try
            {
                if (reworkSheet.ReworkType == ReworkType.REWORKTYPE_REMO)
                {
                    string sql = string.Format(
                        "update tblreject set rejstatus='{0}',reworkcode='{2}' where mocode = '{1}' ",
                        RejectStatus.Handle,
                        reworkSheet.MOCode,
                        reworkSheet.ReworkCode
                        );
                    this.DataProvider.CustomExecute(new SQLCondition(sql));
                    reworkSheet.NewRouteCode = GetNormalRouteByMOCode(reworkSheet.MOCode);
                    reworkSheet.NewRouteCode = "";
                }

                this._helper.AddDomainObject(reworkSheet);

                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_Add_Domain_Object", ex);
            }
        }

        public void UpdateReworkSheetWithoutTransaction(ReworkSheet reworkSheet)
        {
            try
            {
                object oriRS = this.GetReworkSheet(reworkSheet.ReworkCode);
                if (oriRS != null)
                {
                    if (reworkSheet.ReworkType != ((ReworkSheet)oriRS).ReworkType)
                    {
                        string sql = string.Empty;
                        if (reworkSheet.ReworkType == ReworkType.REWORKTYPE_ONLINE)
                        {
                            sql = string.Format(
                                "update tblreject set rejstatus='{0}',reworkcode='' where mocode = '{1}' and rejstatus = '{2}' ",
                                RejectStatus.Confirm,
                                ((ReworkSheet)oriRS).MOCode,
                                RejectStatus.Handle

                                );
                        }
                        else if (reworkSheet.ReworkType == ReworkType.REWORKTYPE_REMO)
                        {
                            sql = string.Format(
                                "update tblreject set rejstatus='{0}',reworkcode='{2}' where mocode = '{1}' ",
                                RejectStatus.Handle,
                                reworkSheet.MOCode,
                                reworkSheet.ReworkCode
                                );
                            reworkSheet.NewRouteCode = GetNormalRouteByMOCode(reworkSheet.MOCode);
                            reworkSheet.NewRouteCode = "";
                        }
                        this.DataProvider.CustomExecute(new SQLCondition(sql));
                    }
                }
                this._helper.UpdateDomainObject(reworkSheet);

            }
            catch (Exception e)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Update_Domain_Object", e);
            }

        }


        public void UpdateReworkSheet(ReworkSheet reworkSheet)
        {
            this.DataProvider.BeginTransaction();
            try
            {
                object oriRS = this.GetReworkSheet(reworkSheet.ReworkCode);
                if (oriRS != null)
                {
                    if (reworkSheet.ReworkType != ((ReworkSheet)oriRS).ReworkType)
                    {
                        string sql = string.Empty;
                        if (reworkSheet.ReworkType == ReworkType.REWORKTYPE_ONLINE)
                        {
                            sql = string.Format(
                                "update tblreject set rejstatus='{0}',reworkcode='' where mocode = '{1}' and rejstatus = '{2}' ",
                                RejectStatus.Confirm,
                                ((ReworkSheet)oriRS).MOCode,
                                RejectStatus.Handle

                                );
                        }
                        else if (reworkSheet.ReworkType == ReworkType.REWORKTYPE_REMO)
                        {
                            sql = string.Format(
                                "update tblreject set rejstatus='{0}',reworkcode='{2}' where mocode = '{1}' ",
                                RejectStatus.Handle,
                                reworkSheet.MOCode,
                                reworkSheet.ReworkCode
                                );
                            reworkSheet.NewRouteCode = GetNormalRouteByMOCode(reworkSheet.MOCode);
                            reworkSheet.NewRouteCode = "";
                        }
                        this.DataProvider.CustomExecute(new SQLCondition(sql));
                    }
                }
                this._helper.UpdateDomainObject(reworkSheet);

                this.DataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_Update_Domain_Object", e);
            }

        }

        public void UpdateReworkSheetRealQty(string reworkCode)
        {
            string strsql;
            strsql = "";
            strsql += "UPDATE tblreworksheet ";
            strsql += "   SET reworkrealqty=(";
            strsql += "    SELECT COUNT (*)";
            strsql += "        FROM (SELECT DISTINCT rcard";
            strsql += "                FROM tblreworkrange";
            strsql += "               WHERE reworkcode = '" + reworkCode + "'))";
            strsql += " WHERE reworkcode='" + reworkCode + "'";

            this.DataProvider.CustomExecute(new SQLCondition(strsql));
        }

        public void DeleteReworkSheetWithTrans(ReworkSheet reworkSheet)
        {
            this.DataProvider.BeginTransaction();
            try
            {
                this.DeleteReworkSheet(reworkSheet);
                this.DataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_Delete_Domain_Object", e);
            }
        }


        public void DeleteReworkSheet(ReworkSheet reworkSheet)
        {
            if (reworkSheet.ReworkType == ReworkType.REWORKTYPE_REMO)
            {
                string sql = string.Format(
                    "update tblreject set rejstatus='{0}',reworkcode='' where mocode = '{1}' and rejstatus = '{2}' ",
                    RejectStatus.Confirm,
                    reworkSheet.MOCode,
                    RejectStatus.Handle

                    );

                this.DataProvider.CustomExecute(new SQLCondition(sql));

            }

            this._helper.DeleteDomainObject(reworkSheet,
                new ICheck[]{ new DeleteAssociateCheck( reworkSheet,
								this.DataProvider, 
								new Type[]{
											  typeof(ReworkSheet2Cause),
											  typeof(ReworkRange),
											  typeof(ReworkPass)	})});



        }

        public void DeleteReworkSheet(ReworkSheet[] reworkSheets)
        {
            if (reworkSheets != null)
            {
                ArrayList newReworkSheets = new ArrayList();
                ArrayList unNewReworkSheets = new ArrayList();
                foreach (ReworkSheet reworkSheet in reworkSheets)
                {
                    if (reworkSheet.Status == ReworkStatus.REWORKSTATUS_NEW)
                    {
                        newReworkSheets.Add(reworkSheet);
                    }
                    else
                    {
                        unNewReworkSheets.Add(reworkSheet);
                    }
                }
                this.DataProvider.BeginTransaction();
                try
                {
                    //batch delete rework sheet with not new status
                    foreach (ReworkSheet sheet in unNewReworkSheets)
                    {
                        this.DeleteReworkSheet(sheet);
                    }

                    //batch delete rework sheet with status new
                    foreach (ReworkSheet sheet in newReworkSheets)
                    {
                        this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete from TBLREWORKSHEET2CAUSE where REWORKCODE='{0}'", sheet.ReworkCode.ToUpper())));
                        this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete from TBLREWORKPASS where REWORKCODE='{0}'", sheet.ReworkCode.ToUpper())));

                        //Added by Jessie Lee for P4.4 AM0183,2005/8/31
                        object[] objs = this.DataProvider.CustomQuery(typeof(Reject), new SQLParamCondition(
                            String.Format("select {0} from TBLREJECT where rcard in ( select rcard from tblreworkrange where reworkcode = $REWORKCODE ) ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject))),
                            new SQLParameter[]
							{
								new SQLParameter("REWORKCODE",typeof(string),sheet.ReworkCode)	 
							}));
                        if (objs != null)
                        {
                            for (int i = 0; i < objs.Length; i++)
                            {
                                ((Reject)objs[i]).RejectStatus = RejectStatus.Confirm;
                                this.UpdateReject((Reject)objs[i]);
                            }
                        }

                        this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete from tblreworkrange where REWORKCODE='{0}'", sheet.ReworkCode.ToUpper())));
                        //end

                        this.DataProvider.CustomDelete(typeof(ReworkSheet), new object[] { sheet.ReworkCode.ToUpper() });
                    }
                    this.DataProvider.CommitTransaction();
                }
                catch (Exception ex)
                {
                    this.DataProvider.RollbackTransaction();
                    ExceptionManager.Raise(this.GetType(), "$Error_Delete_Domain_Object", ex);
                }
            }
        }

        /// <summary>
        /// added by jessie lee for CS0044,2005/10/12,P4.10
        /// </summary>
        /// <param name="reworkSheets"></param>
        public void CancelOpenReworkSheet(ReworkSheet[] reworkSheets)
        {
            if (reworkSheets != null)
            {
                if (!CheckReworkSheetForCancelOpen(reworkSheets))
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_ReworkSheet_CancelOpen_Fail");
                }
                for (int i = 0; i < reworkSheets.Length; i++)
                {
                    ReworkSheet reworkSheet = reworkSheets[i];
                    reworkSheet.Status = ReworkStatus.REWORKSTATUS_RELEASE;

                    this.UpdateReworkSheet(reworkSheet);
                }
            }
        }

        public object GetReworkSheet(string reworkCode)
        {
            return this.DataProvider.CustomSearch(typeof(ReworkSheet), new object[] { reworkCode });
        }

        /// <summary>
        /// ** ��������:	��ѯReworkSheet��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode��ģ����ѯ</param>
        /// <returns> ReworkSheet���ܼ�¼��</returns>
        public int QueryReworkSheetCount(string reworkCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREWORKSHEET where 1=1 and ReworkCode like '{0}%' ", reworkCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReworkSheet
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ReworkSheet����</returns>
        public object[] QueryReworkSheet(string reworkCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet), new PagerCondition(string.Format("select {0} from TBLREWORKSHEET where 1=1 and ReworkCode like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)), reworkCode), "ReworkCode", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	��ѯReworkSheet
        /// ** �� ��:		Laws Lu
        /// ** �� ��:		2005-09-10 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ReworkSheet����</returns>
        public object[] QueryReworkSheet(string reworkCode)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet), new SQLCondition(string.Format("select {0} from TBLREWORKSHEET where 1=1 and ReworkCode like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)), reworkCode)));
        }

        /// <summary>
        /// ** ��������:	������е�ReworkSheet
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ReworkSheet���ܼ�¼��</returns>
        public object[] GetAllReworkSheet()
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet), new SQLCondition(string.Format("select {0} from TBLREWORKSHEET order by ReworkCode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)))));
        }







        /// <summary>
        /// 
        /// </summary>
        /// <param name="reworkSheetCode">���󵥱��</param></param>
        /// <param name="modelCode">����,��ȷ��ѯ</param>
        /// <param name="itemCode">��Ʒ,ģ����ѯ</param>
        /// <param name="moCode">�������,ģ����ѯ</param>
        /// <param name="reworkStatus">����״̬,��ȷ��ѯ</param>
        /// <param name="lotno">����,ģ����ѯ</param>
        /// <param name="runningCard">���к�,ģ����ѯ</param>
        /// <param name="inclusive"></param>
        /// <param name="exclusive"></param>
        /// <returns></returns>
        public object[] QueryReworkSheet(string reworkSheetCode, string modelCode, string itemCode,
            string moCode, string reworkStatus, int beginDate, int endDate, int reworkDate, string reworkType,
            string sscode, string modelcode, string dutycode, string lotNo, int inclusive, int exclusive)
        {
            string moCondition;
            if (moCode == string.Empty)
            {
                moCondition = " 1=1 ";
            }
            else
            {
                moCondition = " mocode like '" + moCode + "%' ";
            }

            string itemCondition;
            if (itemCode == string.Empty)
            {
                itemCondition = " 1=1 ";
            }
            else
            {
                itemCondition = " itemcode like '" + itemCode + "%' ";
            }

            string modelCondition;
            if (modelCode == string.Empty)
            {
                modelCondition = "1=1";
            }
            else
            {
                modelCondition = " itemcode in ( select itemcode from TBLMODEL2ITEM where modelcode ='" + modelCode + "' " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ") ";
            }

            string statusCondition;
            if (reworkStatus == string.Empty)
            {
                statusCondition = "1=1";
            }
            else
            {
                statusCondition = "status = '" + reworkStatus + "' ";
            }

            //add by roger.xue on 2008/10/31: use date to query
            string beginDateCondition;
            if (beginDate == 0)
            {
                beginDateCondition = "1=1";
            }
            else
            {
                beginDateCondition = "mdate >= " + beginDate;
            }

            string endDateCondition;
            if (endDate == 0)
            {
                endDateCondition = "1=1";
            }
            else
            {
                endDateCondition = "mdate <= " + endDate;
            }
            //end add

            string rejectCondition;
            if (lotNo == string.Empty)
            {
                rejectCondition = " 1=1 ";
            }
            else
            {
                if (lotNo.Trim().IndexOf(",") >= 0)
                {
                    lotNo = lotNo.Trim().Replace(",", "','");
                    rejectCondition = " lotlist in ('" + lotNo.Trim() + "')";
                }
                else
                {
                    rejectCondition = " lotlist like '" + lotNo.Trim().ToUpper() + "%'";
                }
            }

            string reWorkTypeCondition = string.Empty;
            if (string.IsNullOrEmpty(reworkType))
            {
                reWorkTypeCondition = " reworktype in ('" + ReworkType.REWORKTYPE_ONLINE + "','" + ReworkType.REWORKTYPE_REMO + "')";
            }
            else
            {
                reWorkTypeCondition = " reworktype ='" + reworkType.Trim() + "' ";
            }

            string dutyCondition = string.Empty;
            if (dutycode.Trim().Length == 0)
            {
                dutyCondition = " 1=1 ";
            }
            else
            {
                if (dutycode.Trim().IndexOf(",") >= 0)
                {
                    dutycode = dutycode.Replace(",", "','");
                    dutyCondition = " dutycode in ('" + dutycode + "') ";
                }
                else
                {
                    dutyCondition = " dutycode like '" + dutycode.Trim().ToUpper() + "%' ";
                }
            }

            string modelcodeCondition = string.Empty;
            if (modelcode.Trim().Length == 0)
            {
                modelcodeCondition = " 1=1 ";
            }
            else
            {
                if (modelcode.Trim().IndexOf(",") >= 0)
                {
                    modelcode = modelcode.Trim().Replace(",", "','");
                    modelcodeCondition = " itemcode in (SELECT mcode FROM tblmaterial  WHERE mmodelcode IN ('" + modelcode.Trim() + "') ) ";
                }
                else
                {
                    modelcodeCondition = " itemcode in (SELECT mcode FROM tblmaterial  WHERE mmodelcode like  '" + modelcode.Trim() + "%') ";//�����λ��Сд�޷�ȷ������ȥ��
                }
            }

            string sscodeCondition = string.Empty;
            if (sscode.Trim().Length == 0)
            {
                sscodeCondition = " 1=1 ";
            }
            else
            {
                if (sscode.Trim().IndexOf(",") >= 0)
                {
                    sscode = sscode.Trim().Replace(",", "','");
                    sscodeCondition = " LOTLIST IN (SELECT LOTNO FROM TBLLOT WHERE SSCODE IN (SELECT SSCODE FROM TBLSS WHERE BIGSSCODE IN ('" + sscode.Trim() + "'))) ";
                }
                else
                {
                    sscodeCondition = " LOTLIST IN (SELECT LOTNO FROM TBLLOT WHERE SSCODE IN (SELECT SSCODE FROM TBLSS WHERE BIGSSCODE like  '" + sscode.Trim().ToUpper() + "%')) ";
                }
            }

            string reworkDateCondition = string.Empty;
            if (reworkDate == 0)
            {
                reworkDateCondition = " 1=1 ";
            }
            else
            {
                reworkDateCondition = "  LOTLIST in (SELECT lotno FROM  tbllot WHERE Ddate=" + reworkDate + ") ";
            }

            string sql1 = "select {0} from tblreworksheet where reworkcode like '{1}%' and {2} and {3} and {4} and {5} and {6} and {7} and {8} and {9} and {10} and {11} and {12} and {13} ";

            sql1 = string.Format(
                sql1,
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)),
                reworkSheetCode,
                itemCondition,
                moCondition,
                statusCondition,
                modelCondition,
                beginDateCondition,
                endDateCondition,
                rejectCondition,
                reWorkTypeCondition,
                dutyCondition,
                modelcodeCondition,
                sscodeCondition,
                reworkDateCondition
                );


            string selectColumns = "a.REWORKTIME,a.REWORKQTY,a.REWORKMAXQTY,a.REWORKREALQTY,a.MUSER,a.MDATE,a.MTIME,a.EATTRIBUTE1,a.ITEMCODE,a.REWORKREASON,a.REASONANALYSE,";
            selectColumns += "a.SOLUTION,a.NEEDCHECK,a.LOTLIST,a.AUTOLOTNO,a.DUTYCODE || ' - ' || TBLDUTY.DUTYDESC AS DUTYCODE ,a.REWORKCODE,a.MOCODE,a.CUSER,a.CDATE,a.CTIME,a.STATUS,a.REWORKSCODE,";
            selectColumns += "a.REWORKTYPE,a.NEWMOCODE,a.NEWROUTECODE,a.NEWOPCODE,a.NEWOPBOMCODE,a.NEWOPBOMVER,a.REWORKHC,a.DEPARTMENT,a.CONTENT,a.REWORKDATE";
            string sql = string.Format("select {0},tblmaterial.mmodelcode,tblss.bigsscode, tbllot.ddate from ({1}) a ",selectColumns, sql1);

            sql += " LEFT  JOIN TBLMATERIAL ON TBLMATERIAL.Mcode=a .itemcode ";
            sql += " LEFT  JOIN  tbllot  ON a.lotlist =tbllot.lotno ";
            sql += " LEFT  JOIN  tblss  ON tbllot.sscode=tblss.sscode ";
            sql += " LEFT  JOIN  tblduty  ON a.dutycode=tblduty.dutycode ";

            return this.DataProvider.CustomQuery(typeof(ReworkSheetQuery), new PagerCondition(sql, "REWORKCODE", inclusive, exclusive));

        }



        public int QueryReworkSheetCount(string reworkSheetCode, string modelCode, string itemCode,
            string moCode, string reworkStatus, int beginDate, int endDate, int reworkDate, string reworkType,
            string sscode, string modelcode, string dutycode, string lotNo)
        {
            string moCondition;
            if (moCode == string.Empty)
            {
                moCondition = " 1=1 ";
            }
            else
            {
                moCondition = " mocode like '" + moCode + "%' ";
            }

            string itemCondition;
            if (itemCode == string.Empty)
            {
                itemCondition = " 1=1 ";
            }
            else
            {
                itemCondition = " itemcode like '" + itemCode + "%' ";
            }

            string modelCondition;
            if (modelCode == string.Empty)
            {
                modelCondition = "1=1";
            }
            else
            {
                modelCondition = " itemcode in ( select itemcode from TBLMODEL2ITEM where modelcode ='" + modelCode + "' " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ") ";
            }

            string statusCondition;
            if (reworkStatus == string.Empty)
            {
                statusCondition = "1=1";
            }
            else
            {
                statusCondition = "status = '" + reworkStatus + "' ";
            }


            //add by roger.xue on 2008/10/31: use date to query
            string beginDateCondition;
            if (beginDate == 0)
            {
                beginDateCondition = "1=1";
            }
            else
            {
                beginDateCondition = "mdate >= " + beginDate;
            }

            string endDateCondition;
            if (endDate == 0)
            {
                endDateCondition = "1=1";
            }
            else
            {
                endDateCondition = "mdate <= " + endDate;
            }
            //end add

            string rejectCondition;
            if (lotNo == string.Empty)
            {
                rejectCondition = " 1=1 ";
            }
            else
            {
                rejectCondition = string.Format(" lotlist='{0}'", lotNo);
            }

            string reWorkTypeCondition = string.Empty;
            if (string.IsNullOrEmpty(reworkType))
            {
                reWorkTypeCondition = " reworktype in ('" + ReworkType.REWORKTYPE_ONLINE + "','" + ReworkType.REWORKTYPE_REMO + "')";
            }
            else
            {
                reWorkTypeCondition = " reworktype ='" + reworkType.Trim() + "' ";
            }

            string dutyCondition = string.Empty;
            if (dutycode.Trim().Length == 0)
            {
                dutyCondition = " 1=1 ";
            }
            else
            {
                if (dutycode.Trim().IndexOf(",") >= 0)
                {
                    dutycode = dutycode.Replace(",", "','");
                    dutyCondition = " dutycode in ('" + dutycode + "') ";
                }
                else
                {
                    dutyCondition = " dutycode like '" + dutycode.Trim().ToUpper() + "%' ";
                }
            }

            string modelcodeCondition = string.Empty;
            if (modelcode.Trim().Length == 0)
            {
                modelcodeCondition = " 1=1 ";
            }
            else
            {
                if (modelcode.Trim().IndexOf(",") >= 0)
                {
                    modelcode = modelcode.Trim().Replace(",", "','");
                    modelcodeCondition = " itemcode in (SELECT mcode FROM tblmaterial  WHERE mmodelcode IN ('" + modelcode.Trim() + "') ) ";
                }
                else
                {
                    modelcodeCondition = " itemcode in (SELECT mcode FROM tblmaterial  WHERE mmodelcode like  '" + modelcode.Trim() + "%') ";
                }
            }

            string sscodeCondition = string.Empty;
            if (sscode.Trim().Length == 0)
            {
                sscodeCondition = " 1=1 ";
            }
            else
            {
                if (sscode.Trim().IndexOf(",") >= 0)
                {
                    sscode = sscode.Trim().Replace(",", "','");
                    sscodeCondition = " LOTLIST IN (SELECT LOTNO FROM TBLLOT WHERE SSCODE IN (SELECT SSCODE FROM TBLSS WHERE BIGSSCODE IN ('" + sscode.Trim() + "'))) ";
                }
                else
                {
                    sscodeCondition = " LOTLIST IN (SELECT LOTNO FROM TBLLOT WHERE SSCODE IN (SELECT SSCODE FROM TBLSS WHERE BIGSSCODE like  '" + sscode.Trim().ToUpper() + "%')) ";
                }
            }

            string reworkDateCondition = string.Empty;
            if (reworkDate == 0)
            {
                reworkDateCondition = " 1=1 ";
            }
            else
            {
                reworkDateCondition = "  LOTLIST in (SELECT lotno FROM  tbllot WHERE Ddate=" + reworkDate + ") ";
            }


            string sql1 = "select {0} from tblreworksheet where reworkcode like '{1}%'  and {2} and {3} and {4} and {5} and {6} and {7} and {8} and {9} and {10} and {11} and {12} and {13} ";


            sql1 = string.Format(
                sql1,
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)),
                 reworkSheetCode,
                itemCondition,
                moCondition,
                statusCondition,
                modelCondition,
                beginDateCondition,
                endDateCondition,
                rejectCondition,
                reWorkTypeCondition,
                dutyCondition,
                modelcodeCondition,
                sscodeCondition,
                reworkDateCondition
                );

            string sql = string.Format("select a.*,tblmaterial.mmodelcode,tblss.bigsscode, tbllot.ddate from ({0}) a  ", sql1);

            sql += " LEFT  JOIN TBLMATERIAL ON TBLMATERIAL.Mcode=a .itemcode ";
            sql += " LEFT  JOIN  tbllot  ON a.lotlist =tbllot.lotno ";
            sql += " LEFT  JOIN  tblss  ON tbllot.sscode=tblss.sscode ";

            sql = string.Format("select count(*) from ( {0} ) ", sql);
            return this.DataProvider.GetCount(new SQLCondition(sql));

        }




        private bool IsNewMoCodeExist(string newMoCode, string reworkCode)
        {

            string sql = "";
            if (reworkCode.Trim() == "")
                sql = "select count(reworkcode) from TBLREWORKSHEET where NEWMOCODE='" + FormatHelper.PKCapitalFormat(newMoCode) + "'  ";
            else
                sql = "select count(reworkcode) from tblreworksheet where reworkcode<>'"
                    + FormatHelper.PKCapitalFormat(reworkCode) + "' and NEWMOCODE='" + FormatHelper.PKCapitalFormat(newMoCode) + "'  ";
            int cnt = this._domainDataProvider.GetCount(new SQLCondition(sql));
            if (cnt > 0)
                return true;
            else
                return false;
        }

        public object[] GetRejectMOs()
        {
            string sql = string.Format(
                "select {0} from tblmo where mocode in (select mocode from tblreject where rejstatus='{1}' ) " + GlobalVariables.CurrentOrganizations.GetSQLCondition(),
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)),
                RejectStatus.Confirm
            );

            return this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(sql));
        }

        public object[] GetAllReworkMOs(string reworkCode)
        {
            object[] objs = this.GetAllReworkMOs();
            ArrayList al = new ArrayList();
            if (objs == null)
            {
                return null;
            }

            foreach (BenQGuru.eMES.Domain.MOModel.MO mo in objs)
            {
                string sql = "select count(mocode) from tblreworksheet where mocode = '" + mo.MOCode + "'";
                if (reworkCode != string.Empty)
                {
                    sql += " and reworkcode <> '" + reworkCode + "' ";
                }

                if (this._domainDataProvider.GetCount(new SQLCondition(sql)) == 0)
                {
                    al.Add(mo);
                }
            }

            if (al.Count == 0)
            {
                return null;
            }

            return (object[])al.ToArray(typeof(BenQGuru.eMES.Domain.MOModel.MO));
        }

        #region ״̬���� sammer kong �ɷ�ͳһ�����������һ���ӿ�

        /// <summary>
        /// ** ��������:	�����ع����󵥵�״̬Ϊ OPEN,���ʧ��,�ع���������
        /// ** �� ��:		vizo
        /// ** �� ��:		2005-04-01 14:41:48
        /// ** �� ��:
        /// ** �� ��:
        /// ** nunit
        /// </summary>
        /// <param name="reworkSheets">ReworkSheet[] Ҫ���������</param>
        public void OpenReworkSheets(ReworkSheet[] reworkSheets)
        {
            this._domainDataProvider.BeginTransaction();

            try
            {
                foreach (ReworkSheet reworkSheet in reworkSheets)
                {
                    CheckReworkSheetOpen(reworkSheet);
                    StatusManager manager = new StatusManager(this, reworkSheet);
                    manager.Open();
                }
                this._domainDataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this._domainDataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_Rework_Open_Fail", null, ex);
            }



        }


        /// <summary>
        /// ����Ƿ��������ΪWatting״̬
        /// </summary>
        /// <param name="reworkSheet"></param>
        private void CheckReworkSheetWait(ReworkSheet reworkSheet)
        {
            /*
            // �ع�ԭ��
            if( this.GetSelectedReworkCauseByReworkCodeCount( reworkSheet.ReworkCode , string.Empty ) == 0 )
            {
                ExceptionManager.Raise(this.GetType() , "$Error_Rework_Cause_Not_Set") ;
            }
            */

            // �ع�ǩ������
            if (this.QueryReworkPassCount(reworkSheet.ReworkCode, string.Empty, string.Empty, string.Empty) == 0)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Rework_Pass_Not_Set $Domain_ReworkSheet=" + reworkSheet.ReworkCode);
            }

            // Marked by Hi1/Venus.feng on 20070729 for Hisense version
            /*
            // �ع���Χ
            if( reworkSheet.ReworkType == BenQGuru.eMES.Web.Helper.ReworkType.REWORKTYPE_ONLINE
                && 0 == this.GetSelectedRejectByReworkSheetCount(
                reworkSheet.ReworkCode ,
                string.Empty ,
                string.Empty ,
                string.Empty ,
                FormatHelper.ToDateString( FormatHelper.TODateInt(DateTime.MinValue)) ,
                FormatHelper.ToTimeString( FormatHelper.TOTimeInt(DateTime.MinValue)) ,
                FormatHelper.ToDateString( FormatHelper.TODateInt(DateTime.MaxValue)) ,
                FormatHelper.ToTimeString( FormatHelper.TOTimeInt(DateTime.MaxValue))
                )
                )
            {
                ExceptionManager.Raise(this.GetType() , "$Error_Rework_Range_Not_Set") ;
            }
            */

            // �����Ƿ����
            if (reworkSheet.ReworkType == BenQGuru.eMES.Web.Helper.ReworkType.REWORKTYPE_REMO
                &&
                this.GetMO(reworkSheet.MOCode) == null
                )
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Rework_MO_Not_Exist $Domain_ReworkSheet=" + reworkSheet.ReworkCode);
            }

            // Added by Icyer 2006/07/12
            if (reworkSheet.Status == BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_CLOSE)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_ReworkSheet_Status_Error $Domain_ReworkSheet=" + reworkSheet.ReworkCode);
            }
            // Added end


        }


        /// <summary>
        /// ����Ƿ��������ΪOpen״̬
        /// </summary>
        /// <param name="reworkSheet"></param>
        private void CheckReworkSheetOpen(ReworkSheet reworkSheet)
        {
            // Marked By HI1/Venus.Feng no 20080727 for Hisense Version
            // Unmarked By HI1/Venus.Feng on 20081127 for Hisense Version : Need Check ReworkRoute for online Sheet
            if (reworkSheet.NewRouteCode.ToString() == string.Empty)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Rework_Route_Not_Set $Domain_ReworkSheet=" + reworkSheet.ReworkCode);
            }
            // End Marked

            // �����Ƿ����
            if (reworkSheet.ReworkType == BenQGuru.eMES.Web.Helper.ReworkType.REWORKTYPE_REMO
                &&
                this.GetMO(reworkSheet.MOCode) == null
                )
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Rework_MO_Not_Exist $Domain_ReworkSheet=" + reworkSheet.ReworkCode);
            }

            // Added by Icyer 2006/07/12
            if (reworkSheet.Status == BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_CLOSE)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_ReworkSheet_Status_Error $Domain_ReworkSheet=" + reworkSheet.ReworkCode);
            }
            // Added end


        }

        public void WaitingReworkSheets(ReworkSheet[] reworkSheets)
        {
            this.DataProvider.BeginTransaction();
            object[] objs = null;
            try
            {
                foreach (ReworkSheet reworkSheet in reworkSheets)
                {
                    // ���ReworkSheet�Ƿ����Waiting

                    CheckReworkSheetWait(reworkSheet);

                    StatusManager manager = new StatusManager(this, reworkSheet);
                    manager.Waiting();
                    //update the reworkpass approve status = waiting 
                    objs = QueryReworkPass(reworkSheet.ReworkCode, string.Empty, string.Empty, string.Empty, int.MinValue, int.MaxValue);
                    if (objs != null)
                    {
                        for (int i = 0; i < objs.Length; i++)
                        {
                            ((ReworkPass)objs[i]).Status = BenQGuru.eMES.Web.Helper.ApproveStatus.APPROVESTATUS_WAITING;
                            ((ReworkPass)objs[i]).IsPass = BenQGuru.eMES.Web.Helper.IsPass.ISPASS_NOTACTION;
                            ((ReworkPass)objs[i]).PassUser = string.Empty;
                            ((ReworkPass)objs[i]).PassContent = string.Empty;

                            this._helper.UpdateDomainObject((DomainObject)objs[i]);
                        }
                    }
                    else
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_Rework_Pass_Not_Set  $Domain_ReworkSheet=" + reworkSheet.ReworkCode);
                    }
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_Rework_Wait_Fail", null, ex);
            }

        }

        // Added by Icyer 2006/07/12
        /// <summary>
        /// �ص�
        /// </summary>
        /// <param name="reworkSheets"></param>
        public void CloseReworkSheets(ReworkSheet[] reworkSheets, string userCode)
        {
            this.DataProvider.BeginTransaction();
            try
            {
                foreach (ReworkSheet reworkSheet in reworkSheets)
                {
                    // ���ReworkSheet�Ƿ����Close
                    CheckReworkSheetClose(reworkSheet);

                    reworkSheet.Status = BenQGuru.eMES.Web.Helper.ReworkStatus.REWORKSTATUS_CLOSE;
                    reworkSheet.MaintainUser = userCode;
                    reworkSheet.MaintainDate = FormatHelper.TODateInt(DateTime.Today);
                    reworkSheet.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);

                    this._helper.UpdateDomainObject(reworkSheet);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_Rework_Close_Fail", null, ex);
            }
        }

        /// <summary>
        /// ����Ƿ��������ΪClose״̬
        /// </summary>
        /// <param name="reworkSheet"></param>
        private void CheckReworkSheetClose(ReworkSheet reworkSheet)
        {
            // �ع���Χ
            if (reworkSheet.Status != ReworkStatus.REWORKSTATUS_OPEN)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_ReworkSheet_Status_Not_Open $Domain_ReworkSheet=" + reworkSheet.ReworkCode);
            }
        }
        // Added end


        #endregion

        #endregion

        #region ReworkSheet2Cause
        /// <summary>
        /// 
        /// </summary>
        public ReworkSheet2Cause CreateNewReworkSheet2Cause()
        {
            return new ReworkSheet2Cause();
        }

        public void AddReworkSheet2Cause(ReworkSheet2Cause reworkSheet2Cause)
        {
            this._helper.AddDomainObject(reworkSheet2Cause);
        }

        public void UpdateReworkSheet2Cause(ReworkSheet2Cause reworkSheet2Cause)
        {
            this._helper.UpdateDomainObject(reworkSheet2Cause);
        }

        public void DeleteReworkSheet2Cause(ReworkSheet2Cause reworkSheet2Cause)
        {
            this._helper.DeleteDomainObject(reworkSheet2Cause);
        }

        public void DeleteReworkSheet2Cause(ReworkSheet2Cause[] reworkSheet2Cause)
        {
            this._helper.DeleteDomainObject(reworkSheet2Cause);
        }

        public object GetReworkSheet2Cause(string reworkCode, string reworkCauseCode)
        {
            return this.DataProvider.CustomSearch(typeof(ReworkSheet2Cause), new object[] { reworkCode, reworkCauseCode });
        }

        /// <summary>
        /// ** ��������:	��ѯReworkSheet2Cause��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode��ģ����ѯ</param>
        /// <param name="reworkCauseCode">ReworkCauseCode��ģ����ѯ</param>
        /// <returns> ReworkSheet2Cause���ܼ�¼��</returns>
        public int QueryReworkSheet2CauseCount(string reworkCode, string reworkCauseCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLReworkSheet2Cause where 1=1 and ReworkCode like '{0}%'  and REWORKCCODE like '{1}%' ", reworkCode, reworkCauseCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReworkSheet2Cause
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode��ģ����ѯ</param>
        /// <param name="reworkCauseCode">ReworkCauseCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ReworkSheet2Cause����</returns>
        public object[] QueryReworkSheet2Cause(string reworkCode, string reworkCauseCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet2Cause), new PagerCondition(string.Format("select {0} from TBLReworkSheet2Cause where 1=1 and ReworkCode like '{1}%'  and REWORKCCODE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet2Cause)), reworkCode, reworkCauseCode), "ReworkCode,REWORKCCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�ReworkSheet2Cause
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ReworkSheet2Cause���ܼ�¼��</returns>
        public object[] GetAllReworkSheet2Cause()
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet2Cause), new SQLCondition(string.Format("select {0} from TBLReworkSheet2Cause order by ReworkCode,REWORKCCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet2Cause)))));
        }

        public void AddReworkSheet2Cause(ReworkSheet2Cause[] reworkSheet2Causes)
        {
            this._helper.AddDomainObject(reworkSheet2Causes);
        }

        #region ReworkCause --> ReworkSheet
        /// <summary>
        /// ** ��������:	��ReworkCauseCode���ReworkSheet
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCauseCode">ReworkCauseCode,��ȷ��ѯ</param>
        /// <returns>ReworkSheet����</returns>
        public object[] GetReworkSheetByReworkCauseCode(string reworkCauseCode)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet), new SQLCondition(string.Format("select {0} from TBLREWORKSHEET where ReworkCode in ( select ReworkCode from TBLReworkSheet2Cause where REWORKCCODE='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)), reworkCauseCode)));
        }

        /// <summary>
        /// ** ��������:	��ReworkCauseCode�������ReworkCause��ReworkSheet������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCauseCode">ReworkCauseCode,��ȷ��ѯ</param>
        /// <param name="reworkCode">ReworkCode,ģ����ѯ</param>
        /// <returns>ReworkSheet������</returns>
        public int GetSelectedReworkSheetByReworkCauseCodeCount(string reworkCauseCode, string reworkCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLReworkSheet2Cause where REWORKCCODE ='{0}' and ReworkCode like '{1}%'", reworkCauseCode, reworkCode)));
        }

        /// <summary>
        /// ** ��������:	��ReworkCauseCode�������ReworkCause��ReworkSheet����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCauseCode">ReworkCauseCode,��ȷ��ѯ</param>
        /// <param name="reworkCode">ReworkCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>ReworkSheet����</returns>
        public object[] GetSelectedReworkSheetByReworkCauseCode(string reworkCauseCode, string reworkCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet),
                new PagerCondition(string.Format("select {0} from TBLREWORKSHEET where ReworkCode in ( select ReworkCode from TBLReworkSheet2Cause where REWORKCCODE ='{1}') and ReworkCode like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)), reworkCauseCode, reworkCode), "ReworkCode", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	��ReworkCauseCode��ò�����ReworkCause��ReworkSheet������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCauseCode">ReworkCauseCode,��ȷ��ѯ</param>
        /// <param name="reworkCode">ReworkCode,ģ����ѯ</param>
        /// <returns>ReworkSheet������</returns>
        public int GetUnselectedReworkSheetByReworkCauseCodeCount(string reworkCauseCode, string reworkCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREWORKSHEET where ReworkCode not in ( select ReworkCode from TBLReworkSheet2Cause where REWORKCCODE ='{0}') and ReworkCode like '{1}%'", reworkCauseCode, reworkCode)));
        }

        /// <summary>
        /// ** ��������:	��ReworkCauseCode��ò�����ReworkCause��ReworkSheet����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCauseCode">ReworkCauseCode,��ȷ��ѯ</param>
        /// <param name="reworkCode">ReworkCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>ReworkSheet����</returns>
        public object[] GetUnselectedReworkSheetByReworkCauseCode(string reworkCauseCode, string reworkCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSheet),
                new PagerCondition(string.Format("select {0} from TBLREWORKSHEET where ReworkCode not in ( select ReworkCode from TBLReworkSheet2Cause where REWORKCCODE ='{1}') and ReworkCode like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)), reworkCauseCode, reworkCode), "ReworkCode", inclusive, exclusive));
        }
        #endregion

        #region ReworkSheet --> ReworkCause
        /// <summary>
        /// ** ��������:	��ReworkCode���ReworkCause
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <returns>ReworkCause����</returns>
        public object[] GetReworkCauseByReworkCode(string reworkCode)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkCause), new SQLCondition(string.Format("select {0} from TBLREWORKCAUSE where REWORKCCODE in ( select REWORKCCODE from TBLReworkSheet2Cause where ReworkCode='{1}')", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkCause)), reworkCode)));
        }

        /// <summary>
        /// ** ��������:	��ReworkCode�������ReworkSheet��ReworkCause������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <param name="reworkCauseCode">ReworkCauseCode,ģ����ѯ</param>
        /// <returns>ReworkCause������</returns>
        public int GetSelectedReworkCauseByReworkCodeCount(string reworkCode, string reworkCauseCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLReworkSheet2Cause where ReworkCode ='{0}' and REWORKCCODE like '{1}%'", reworkCode, reworkCauseCode)));
        }

        /// <summary>
        /// ** ��������:	��ReworkCode�������ReworkSheet��ReworkCause����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <param name="reworkCauseCode">ReworkCauseCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>ReworkCause����</returns>
        public object[] GetSelectedReworkCauseByReworkCode(string reworkCode, string reworkCauseCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkCause),
                new PagerCondition(string.Format("select {0} from TBLREWORKCAUSE where REWORKCCODE in ( select REWORKCCODE from TBLReworkSheet2Cause where ReworkCode ='{1}') and REWORKCCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkCause)), reworkCode, reworkCauseCode), "REWORKCCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	��ReworkCode��ò�����ReworkSheet��ReworkCause������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <param name="reworkCauseCode">ReworkCauseCode,ģ����ѯ</param>
        /// <returns>ReworkCause������</returns>
        public int GetUnselectedReworkCauseByReworkCodeCount(string reworkCode, string reworkCauseCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREWORKCAUSE where REWORKCCODE not in ( select REWORKCCODE from TBLReworkSheet2Cause where ReworkCode ='{0}') and REWORKCCODE like '{1}%'", reworkCode, reworkCauseCode)));
        }

        /// <summary>
        /// ** ��������:	��ReworkCode��ò�����ReworkSheet��ReworkCause����ҳ
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkCode">ReworkCode,��ȷ��ѯ</param>
        /// <param name="reworkCauseCode">ReworkCauseCode,ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns>ReworkCause����</returns>
        public object[] GetUnselectedReworkCauseByReworkCode(string reworkCode, string reworkCauseCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkCause),
                new PagerCondition(string.Format("select {0} from TBLREWORKCAUSE where REWORKCCODE not in ( select REWORKCCODE from TBLReworkSheet2Cause where ReworkCode ='{1}') and REWORKCCODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkCause)), reworkCode, reworkCauseCode), "REWORKCCODE", inclusive, exclusive));
        }
        #endregion


        #endregion

        #region ReworkSource
        /// <summary>
        /// 
        /// </summary>
        public ReworkSource CreateNewReworkSource()
        {
            return new ReworkSource();
        }

        public void AddReworkSource(ReworkSource reworkSource)
        {
            this._helper.AddDomainObject(reworkSource);
        }

        public void UpdateReworkSource(ReworkSource reworkSource)
        {
            this._helper.UpdateDomainObject(reworkSource);
        }

        public void DeleteReworkSource(ReworkSource reworkSource)
        {
            this._helper.DeleteDomainObject(reworkSource,
                new ICheck[]{ new DeleteAssociateCheck( reworkSource,
                                this.DataProvider, 
                                new Type[]{
                                              typeof(ReworkSheet)	})});
        }

        public void DeleteReworkSource(ReworkSource[] reworkSource)
        {
            this._helper.DeleteDomainObject(reworkSource,
                new ICheck[]{ new DeleteAssociateCheck( reworkSource,
                                this.DataProvider, 
                                new Type[]{
                                              typeof(ReworkSheet)	})});
        }

        public object GetReworkSource(string reworkSourceCode)
        {
            return this.DataProvider.CustomSearch(typeof(ReworkSource), new object[] { reworkSourceCode });
        }

        /// <summary>
        /// ** ��������:	��ѯReworkSource��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkSourceCode">ReworkSourceCode��ģ����ѯ</param>
        /// <returns> ReworkSource���ܼ�¼��</returns>
        public int QueryReworkSourceCount(string reworkSourceCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREWORKSOURCE where 1=1 and REWORKSCODE like '{0}%' ", reworkSourceCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReworkSource
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reworkSourceCode">ReworkSourceCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ReworkSource����</returns>
        public object[] QueryReworkSource(string reworkSourceCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(
                typeof(ReworkSource),
                new PagerCondition(
                    string.Format("select {0} from TBLREWORKSOURCE where 1=1 and REWORKSCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSource)), reworkSourceCode), "REWORKSCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�ReworkSource
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-04-20 11:11:55
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ReworkSource���ܼ�¼��</returns>
        public object[] GetAllReworkSource()
        {
            return this.DataProvider.CustomQuery(typeof(ReworkSource), new SQLCondition(string.Format("select {0} from TBLREWORKSOURCE order by REWORKSCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSource)))));
        }


        #endregion

        #region ReworkLotNo
        /// <summary>
        /// 
        /// </summary>
        public ReworkLotNo CreateNewReworkLotNo()
        {
            return new ReworkLotNo();
        }

        public void AddReworkLotNo(ReworkLotNo reworkLotNo)
        {
            this.DataProvider.Insert(reworkLotNo);
        }

        public void UpdateReworkLotNo(ReworkLotNo reworkLotNo)
        {
            this.DataProvider.Update(reworkLotNo);
        }

        public void DeleteReworkLotNo(ReworkLotNo reworkLotNo)
        {
            this.DataProvider.Delete(reworkLotNo);
        }

        public object GetReworkLotNo(string lotno)
        {
            return this.DataProvider.CustomSearch(typeof(ReworkLotNo), new object[] { lotno });
        }

        public void DeleteLotAndAllRCard(string lotNo)
        {
            string sql = "DELETE FROM tbltempreworklotno WHERE lotno='" + lotNo + "'";
            this.DataProvider.CustomExecute(new SQLCondition(sql));

            sql = "DELETE FROM tbltempreworkrcard WHERE lotno='" + lotNo + "'";
            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }

        #endregion

        #region ReworkRcard
        /// <summary>
        /// 
        /// </summary>
        public ReworkRcard CreateNewReworkRcard()
        {
            return new ReworkRcard();
        }

        public void AddReworkRcard(ReworkRcard reworkRcard)
        {
            this.DataProvider.Insert(reworkRcard);
        }

        public void UpdateReworkRcard(ReworkRcard reworkRcard)
        {
            this.DataProvider.Update(reworkRcard);
        }

        public void DeleteReworkRcard(ReworkRcard reworkRcard)
        {
            this.DataProvider.Delete(reworkRcard);
        }

        public void DeleteReworkRcard(ReworkRcard[] reworkRcard)
        {
            foreach (ReworkRcard rr in reworkRcard)
            {
                this.DeleteReworkRcard(rr);
            }
        }

        public void DeleteReworkRcard(string rcards, string lotno)
        {
            string sql = string.Format("delete from tBLTempREWORKRCARD where lotno='{0}'", lotno);
            if (rcards.Trim() != string.Empty)
            {
                sql += " and RCARD not in ('" + rcards + "')";
            }

            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }

        public object GetReworkRcard(string lotno, string rcard)
        {
            return this.DataProvider.CustomSearch(typeof(ReworkRcard), new object[] { lotno, rcard });
        }

        public object[] QueryReworkRcard(string lotno)
        {
            return this.DataProvider.CustomQuery(typeof(ReworkRcard), new SQLCondition(string.Format("select {0} from tbltempreworkrcard where lotno ='{1}'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkRcard)), lotno)));
        }


        #endregion

        #region ReworkSheetByReject
        private decimal ToDateTimeDecimal(string date, string time)
        {
            return
                System.Convert.ToDecimal(FormatHelper.TODateInt(date)) * 1000000m
                + System.Convert.ToDecimal(FormatHelper.TOTimeInt(time));
        }


        public object GetReworkSheetByReject(string rcard, decimal rcardseq)
        {
            object obj = this.GetReject(rcard, rcardseq);
            if (obj != null)
            {
                if (((Reject)obj).RejectStatus == RejectStatus.Handle)
                {
                    object reworkSheet = this.GetReworkSheet(((Reject)obj).ReworkCode);
                    if (reworkSheet != null)
                    {
                        if (((ReworkSheet)reworkSheet).Status == ReworkStatus.REWORKSTATUS_OPEN)
                        {
                            if (
                                ((ReworkSheet)reworkSheet).ReworkType == ReworkType.REWORKTYPE_REMO
                                &&
                                ((Reject)obj).MOCode == ((ReworkSheet)reworkSheet).MOCode)
                            {
                                return reworkSheet;
                            }

                            if (
                                ((ReworkSheet)reworkSheet).ReworkType == ReworkType.REWORKTYPE_ONLINE
                              )
                            {
                                return reworkSheet;
                            }

                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// added by jessie lee, 2005/12/9
        /// </summary>
        /// <param name="rcard"></param>
        /// <param name="rcardseq"></param>
        /// <param name="mocode"></param>
        /// <returns>ReworkSheet</returns>
        public object GetReworkSheetByReject(string rcard, decimal rcardseq, string mocode)
        {
            /* table tblreject 's PK is rcard,rcardseq and mocode */
            object[] obj = this.DataProvider.CustomQuery(
                typeof(Reject),
                new SQLCondition(
                    string.Format("select {0} from tblreject where rcard='{1}' and rcardseq ={2} and mocode='{3}' ",
                        DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject)), rcard, rcardseq, mocode)));

            if (obj[0] != null)
            {
                Reject objReject = obj[0] as Reject;
                if (string.Compare(objReject.RejectStatus, RejectStatus.Handle, true) == 0)
                {
                    ReworkSheet reworkSheet = this.GetReworkSheet(objReject.ReworkCode) as ReworkSheet;
                    if (reworkSheet != null)
                    {
                        if (string.Compare(reworkSheet.Status, ReworkStatus.REWORKSTATUS_OPEN, true) == 0)
                        {
                            if (string.Compare(reworkSheet.ReworkType, ReworkType.REWORKTYPE_REMO, true) == 0
                                && string.Compare(objReject.MOCode, reworkSheet.MOCode, true) == 0)
                            {
                                return reworkSheet;
                            }

                            if (string.Compare(reworkSheet.ReworkType, ReworkType.REWORKTYPE_ONLINE, true) == 0)
                            {
                                return reworkSheet;
                            }

                        }
                    }
                }
            }

            return null;
        }

        public object GetMO(string moCode)
        {
            return this.DataProvider.CustomSearch(typeof(BenQGuru.eMES.Domain.MOModel.MO), new object[] { moCode });
        }

        public object[] GetAllReworkMOs()
        {
            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            object[] parameters = systemSettingFacade.GetParametersByParameterValue(BenQGuru.eMES.Web.Helper.MOType.GroupType, BenQGuru.eMES.Web.Helper.MOType.MOTYPE_REWORKMOTYPE);
            if (parameters != null)
            {
                string tmpReworkType = string.Empty;
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i == 0)
                    {
                        tmpReworkType = "'" + ((Parameter)parameters[i]).ParameterCode + "'";

                    }
                    else
                    {
                        tmpReworkType += ",'" + ((Parameter)parameters[i]).ParameterCode + "'";
                    }
                }
                return this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(string.Format("select {0} from tblmo where motype in(" + tmpReworkType + ") " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " order by mocode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)))));
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region	Confirm Reject	ȷ������

        public void ConfirmLots(ArrayList lots)
        {
            if (!(lots != null && lots.Count > 0)) { return; }
            object[] _rejects = this.GetRejectsByLotNo(lots);
            try
            {
                foreach (Reject _reject in _rejects)
                {
                    if (_reject.RejectStatus == BenQGuru.eMES.Web.Helper.RejectStatus.Reject)
                    {
                        _reject.RejectStatus = BenQGuru.eMES.Web.Helper.RejectStatus.Confirm;
                    }
                    else
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_Reject_Status_Error");
                    }
                }
                this.UpdateReject(_rejects);
            }
            catch (Exception ex)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Confirm_Reject_Fail", ex);
            }
        }

        public void ConfirmNoLots(ArrayList nolotRejects)
        {
            try
            {
                foreach (Reject _reject in nolotRejects)
                {
                    if (_reject.RejectStatus == BenQGuru.eMES.Web.Helper.RejectStatus.Reject)
                    {
                        _reject.RejectStatus = BenQGuru.eMES.Web.Helper.RejectStatus.Confirm;
                    }
                    else
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_Reject_Status_Error");
                    }
                }
                this.UpdateReject((Reject[])nolotRejects.ToArray(typeof(Reject)));
            }
            catch (Exception ex)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Confirm_Reject_Fail", ex);
            }
        }

        //ͨ�����λ�ȡ��������������Ϣ
        private object[] GetRejectsByLotNo(ArrayList LotNOs)
        {
            string[] lotNOs = (string[])LotNOs.ToArray(typeof(string));
            object[] rejects = this.DataProvider.CustomQuery(
                typeof(Reject),
                new SQLCondition(
                string.Format("select {0} from TBLREJECT where lotno in ( {1} ) ",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reject)), FormatHelper.ProcessQueryValues(lotNOs))));
            return rejects;
        }

        #endregion

        #region UnConfirm Reject ȡ��ȷ��

        public void UnConfirmLots(ArrayList lots)
        {
            if (!(lots != null && lots.Count > 0)) { return; }
            object[] _rejects = this.GetRejectsByLotNo(lots);
            try
            {
                foreach (Reject _reject in _rejects)
                {
                    if (_reject.RejectStatus == BenQGuru.eMES.Web.Helper.RejectStatus.Confirm)
                    {
                        _reject.RejectStatus = BenQGuru.eMES.Web.Helper.RejectStatus.Reject;
                    }
                    else
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_Reject_Status_Error");
                    }
                }
                this.UpdateReject(_rejects);
            }
            catch (Exception ex)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_UnConfirm_Reject_Fail", ex);
            }
        }

        public void UnConfirmNoLots(ArrayList nolotRejects)
        {
            try
            {
                foreach (Reject _reject in nolotRejects)
                {
                    if (_reject.RejectStatus == BenQGuru.eMES.Web.Helper.RejectStatus.Confirm)
                    {
                        _reject.RejectStatus = BenQGuru.eMES.Web.Helper.RejectStatus.Reject;
                    }
                    else
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_Reject_Status_Error");
                    }
                }
                this.UpdateReject((Reject[])nolotRejects.ToArray(typeof(Reject)));
            }
            catch (Exception ex)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_UnConfirm_Reject_Fail", ex);
            }
        }


        #endregion

        #region Cancle Reject ȡ�����ˣ����û�����ε�������Ϣ��

        public void CancelReject(ArrayList nolotRejects)
        {
            this.DataProvider.BeginTransaction();
            try
            {
                foreach (Reject _reject in nolotRejects)
                {
                    this.CancleReject(_reject);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_Cancel_Reject_Fail", ex);
            }
        }

        public void CancleReject(Reject _reject)
        {
            string _rcard = _reject.RunningCard;
            string _rcardseq = _reject.RunningCardSequence.ToString();
            string _mocode = _reject.MOCode;

            /* step1:get all the second latest onwip which belong to the lot's rcards */
            object[] onWipSecond = this.DataProvider.CustomQuery(typeof(OnWIP),
                new SQLCondition(
                string.Format(@" select {2}
								from tblonwip a
								where a.rcard ='{0}'  and MOCODE='{1}' " +
                //added by jessie lee, 2005/11/30
                                    @" and rcardseq =
										(select max(rcardseq)
										from tblonwip b
										where a.rcard = b.rcard
											and rcardseq < 
													(select rcardseq
													from (select rcardseq,rcard
															from tblonwip 
															order by mdate * 1000000 + mtime desc) c
													where rownum = 1 and c.rcard = a.rcard ))",
                            _rcard, _mocode, DomainObjectUtility.GetDomainObjectFieldsString(typeof(OnWIP)))));

            /* step2: update simulation ,simulationreport,delete the latest onwip */
            for (int j = 0; j < onWipSecond.Length; j++)
            {
                OnWIP onWipSec = onWipSecond[j] as OnWIP;

                //update simulation
                Simulation sim = GetSimulation(onWipSec.RunningCard) as Simulation;

                sim.LastAction = onWipSec.Action;
                sim.ProductStatus = onWipSec.ActionResult;
                sim.ItemCode = onWipSec.ItemCode;
                sim.MaintainDate = onWipSec.MaintainDate;
                sim.MaintainTime = onWipSec.MaintainTime;
                sim.MaintainUser = onWipSec.MaintainUser;
                sim.MOCode = onWipSec.MOCode;
                sim.ModelCode = onWipSec.ModelCode;
                sim.NGTimes = onWipSec.NGTimes;
                sim.OPCode = onWipSec.OPCode;
                sim.ResourceCode = onWipSec.ResourceCode;
                sim.RouteCode = onWipSec.RouteCode;
                sim.RunningCard = onWipSec.RunningCard;
                sim.RunningCardSequence = onWipSec.RunningCardSequence;
                sim.SourceCard = onWipSec.SourceCard;
                sim.SourceCardSequence = onWipSec.SourceCardSequence;
                sim.TranslateCard = onWipSec.TranslateCard;
                sim.TranslateCardSequence = onWipSec.TranslateCardSequence;
                sim.ActionList = GetSecondLastActionList(sim.ActionList.Trim());

                //step3:update simulationReport
                SimulationReport simulationReport = GetSimulationReport(onWipSec.RunningCard) as SimulationReport;

                simulationReport.RouteCode = sim.RouteCode;
                simulationReport.OPCode = sim.OPCode;
                simulationReport.ItemCode = sim.ItemCode;
                simulationReport.LastAction = sim.LastAction;
                simulationReport.MaintainDate = sim.MaintainDate;
                simulationReport.MaintainTime = sim.MaintainTime;
                simulationReport.MaintainUser = sim.MaintainUser;
                simulationReport.MOCode = sim.MOCode;
                simulationReport.ModelCode = sim.ModelCode;
                simulationReport.NGTimes = sim.NGTimes;
                simulationReport.ResourceCode = sim.ResourceCode;
                simulationReport.RunningCard = sim.RunningCard;
                simulationReport.RunningCardSequence = sim.RunningCardSequence;
                simulationReport.Status = sim.ProductStatus;
                simulationReport.TranslateCard = sim.TranslateCard;
                simulationReport.TranslateCardSequence = sim.TranslateCardSequence;
                simulationReport.SourceCard = sim.SourceCard;
                simulationReport.SourceCardSequence = sim.SourceCardSequence;

                this.DataProvider.Update(sim);
                this.DataProvider.Update(simulationReport);
                /* step4:delete the latest onwip  */
                this.DataProvider.CustomExecute(
                    new SQLCondition(
                    //modified by jessie lee, 2005/11/29
                    string.Format(@"delete from TBLONWIP where rcard = '{0}' and mocode='{1}' and rcardseq = ( select rcardseq
															from (select rcardseq
																from tblonwip
																where rcard = '{0}'
																order by mdate * 1000000 + mtime desc,rcardseq desc)
															where rownum = 1
														)",
                    onWipSec.RunningCard, _reject.MOCode)));

                //Laws Lu,2006/08/10 write off
                /* step5:rewrite TS if the status is OQCNG or NG */
                if (String.Compare(onWipSec.Action, ActionType.DataCollectAction_OQCNG, true) == 0 ||
                    String.Compare(onWipSec.Action, ActionType.DataCollectAction_NG, true) == 0 ||
                    String.Compare(onWipSec.Action, ActionType.DataCollectAction_SMTNG, true) == 0)
                {
                    if (CheckForTSReWrite(onWipSec.RunningCard, onWipSec.RunningCardSequence.ToString()))
                    {
                        this.CollectErrorInfor(sim, simulationReport, _reject);
                    }
                }
            }
            /* step6:delete all rcard of the lot in the table TBLREJECT */
            this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete from tblreject2errorcode where RCARD='{0}' and RCARDSEQ='{1}' and MOCODE='{2}' ", _rcard, _rcardseq, _mocode)));
            this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete from TBLReject where RCARD='{0}' and RCARDSEQ='{1}' and MOCODE='{2}' ", _rcard, _rcardseq, _mocode)));
        }
        #endregion

        #region Cancel Lot Reject
        /// <summary>
        /// Added by jessie lee,2005/9/27
        /// </summary>
        /// <param name="LotNO">����</param>
        /// <returns>true:����ȡ�����ˣ�false:������ȡ������</returns>
        public bool CheckLotToCancelReject(string LotNO)
        {
            int count = this.DataProvider.GetCount(
                new SQLParamCondition(
                    string.Format("select count(rcard) from tblreject where lotno = $LotNO and rejstatus <> '{0}' ", RejectStatus.Reject),
                    new SQLParameter[] { new SQLParameter("LotNO", typeof(string), LotNO) }));
            if (count > 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Added by jessie lee,2005/9/27
        /// </summary>
        /// <param name="LotNO">����</param>
        /// <returns>true:����ȡ�����ˣ�false:������ȡ������</returns>
        public bool CheckLotToCancelReject(ArrayList LotNOs)
        {
            for (int i = 0; i < LotNOs.Count; i++)
            {
                int count = this.DataProvider.GetCount(
                    new SQLParamCondition(
                    string.Format("select count(rcard) from tblreject where lotno = $LotNO and rejstatus <> '{0}' ", RejectStatus.Reject),
                    new SQLParameter[] { new SQLParameter("LotNO", typeof(string), LotNOs[i].ToString()) }));
                if (count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Added by jessie lee,2005/9/27
        /// *****ȡ������*****
        /// �޸��߼���
        /// 1.�޸�����״̬���Լ���Check�е�����
        /// 2.ɾ�������������е���Ӧ����
        /// 3.��onwip�е����ڶ������ݻ�д��simulation��simulationreport
        /// 4.��дTS
        /// </summary>
        /// <param name="LotNOs"></param>
        public void MakeLotsCancelReject(ArrayList LotNOs)
        {
            string[] lotNOs = (string[])LotNOs.ToArray(typeof(string));
            this.DataProvider.BeginTransaction();
            try
            {

                object[] lots = this.DataProvider.CustomQuery(
                    typeof(OQCLot),
                    new SQLCondition(
                    string.Format("select {0} from tblLot where lotno in ( {1} ) ",
                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(OQCLot)), FormatHelper.ProcessQueryValues(lotNOs))));

                for (int i = 0; i < lots.Length; i++)
                {
                    /* Step1:update Lot Status */
                    (lots[i] as OQCLot).LOTStatus = OQCLotStatus.OQCLotStatus_Examing;
                    //oqcFacade.UpdateOQCLot(lots[i] as OQCLot);
                    this._helper.UpdateDomainObject(lots[i] as OQCLot);

                    string lotno = (lots[i] as OQCLot).LOTNO;

                    /* added by jessie lee, 2005/11/30, add step
                     * step2:update tbllot2card status from 'REJECT' to 'GOOD'*/
                    this.DataProvider.CustomExecute(new SQLCondition(string.Format(
                        "update tbllot2card set status = '{0}' where lotno = '{1}' ",
                        ActionType.DataCollectAction_GOOD,
                        lotno)));

                    /* step3:delete all rcard of the lot in the table TBLREJECT */
                    this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete from tblreject2errorcode where (rcard,rcardseq) in (select rcard,rcardseq from TBLReject where lotno = '{0}' )", lotno)));
                    this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete from TBLReject where lotno = '{0}'", lotno)));

                    /* step4:get all the second latest onwip which belong to the lot's rcards */
                    object[] onWipSecond = this.DataProvider.CustomQuery(typeof(OnWIP),
                        new SQLParamCondition(
                        string.Format(@" select {0}
										from tblonwip a
										where a.rcard in
												(select rcard from tbllot2card where lotno = $LotNO )" +
                        //added by jessie lee, 2005/11/30
                        //��ӹ����ж�����
                                            @" and mocode in ( select mocode from tbllot2card where lotno = $LotNO1 )  	
											and rcardseq =
												(select max(rcardseq)
												from tblonwip b
												where a.rcard = b.rcard
													and rcardseq < 
														 (select rcardseq
															from (select rcardseq,rcard
																	from tblonwip 
																	order by mdate * 1000000 + mtime desc) c
															where rownum = 1 and c.rcard = a.rcard ))",
                        DomainObjectUtility.GetDomainObjectFieldsString(typeof(OnWIP))),
                        new SQLParameter[]
						{
							new SQLParameter("LotNO",typeof(string), lotno),
							new SQLParameter("LotNO1",typeof(string), lotno)
						}));

                    /* step5: update simulation ,simulationreport,delete the latest onwip */
                    for (int j = 0; j < onWipSecond.Length; j++)
                    {
                        OnWIP onWipSec = onWipSecond[j] as OnWIP;

                        //update simulation
                        Simulation sim = GetSimulation(onWipSec.RunningCard) as Simulation;

                        sim.LastAction = onWipSec.Action;
                        sim.ProductStatus = onWipSec.ActionResult;
                        sim.ItemCode = onWipSec.ItemCode;
                        sim.MaintainDate = onWipSec.MaintainDate;
                        sim.MaintainTime = onWipSec.MaintainTime;
                        sim.MaintainUser = onWipSec.MaintainUser;
                        sim.MOCode = onWipSec.MOCode;
                        sim.ModelCode = onWipSec.ModelCode;
                        sim.NGTimes = onWipSec.NGTimes;
                        sim.OPCode = onWipSec.OPCode;
                        sim.ResourceCode = onWipSec.ResourceCode;
                        sim.RouteCode = onWipSec.RouteCode;
                        sim.RunningCard = onWipSec.RunningCard;
                        sim.RunningCardSequence = onWipSec.RunningCardSequence;
                        sim.SourceCard = onWipSec.SourceCard;
                        sim.SourceCardSequence = onWipSec.SourceCardSequence;
                        sim.TranslateCard = onWipSec.TranslateCard;
                        sim.TranslateCardSequence = onWipSec.TranslateCardSequence;
                        sim.ActionList = GetSecondLastActionList(sim.ActionList.Trim());

                        //update simulationReport
                        SimulationReport simulationReport = GetSimulationReport(onWipSec.RunningCard) as SimulationReport;

                        simulationReport.RouteCode = sim.RouteCode;
                        simulationReport.OPCode = sim.OPCode;
                        simulationReport.ItemCode = sim.ItemCode;
                        simulationReport.LastAction = sim.LastAction;
                        simulationReport.MaintainDate = sim.MaintainDate;
                        simulationReport.MaintainTime = sim.MaintainTime;
                        simulationReport.MaintainUser = sim.MaintainUser;
                        simulationReport.MOCode = sim.MOCode;
                        simulationReport.ModelCode = sim.ModelCode;
                        simulationReport.NGTimes = sim.NGTimes;
                        simulationReport.ResourceCode = sim.ResourceCode;
                        simulationReport.RunningCard = sim.RunningCard;
                        simulationReport.RunningCardSequence = sim.RunningCardSequence;
                        simulationReport.Status = sim.ProductStatus;
                        simulationReport.TranslateCard = sim.TranslateCard;
                        simulationReport.TranslateCardSequence = sim.TranslateCardSequence;
                        simulationReport.SourceCard = sim.SourceCard;
                        simulationReport.SourceCardSequence = sim.SourceCardSequence;

                        this.DataProvider.Update(sim);
                        this.DataProvider.Update(simulationReport);
                        /* delete the latest onwip  */
                        this.DataProvider.CustomExecute(
                            new SQLCondition(
                            //string.Format("delete from TBLONWIP where rcard = '{0}' and rcardseq = ( select max(rcardseq) from tblonwip where rcard = '{0}' )",
                            //modified by jessie lee, 2005/11/29
                            string.Format(@"delete from TBLONWIP where rcard = '{0}' and rcardseq = ( select rcardseq
												from (select rcardseq
													from tblonwip
													where rcard = '{0}'
													order by mdate * 1000000 + mtime desc,rcardseq desc)
												where rownum = 1
											)",
                            onWipSec.RunningCard)));

                        //Laws Lu,2006/08/10 write off
                        /* step6:rewrite TS if the status is OQCNG or NG */
                        if (String.Compare(onWipSec.Action, ActionType.DataCollectAction_OQCNG, true) == 0 ||
                            String.Compare(onWipSec.Action, ActionType.DataCollectAction_NG, true) == 0 ||
                            String.Compare(onWipSec.Action, ActionType.DataCollectAction_SMTNG, true) == 0)
                        {
                            if (CheckForTSReWrite(onWipSec.RunningCard, onWipSec.RunningCardSequence.ToString()))
                            {
                                this.CollectErrorInfor(sim, simulationReport, lotno);
                            }
                        }
                    }
                }

                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_Cancel_Reject_Fail", ex);
            }
        }

        #region CollectErrorInfor ȡ�����˻�дTS��Ϣ��TSErrorCode
        //��Ե���û������Ϣ��������Ϣ
        private void CollectErrorInfor(Simulation sim, SimulationReport simReport, Reject _reject)
        {
            TSFacade tsFacade = new TSFacade(_domainDataProvider);
            //			BenQGuru.eMES.Domain.TS.TS tS = new  BenQGuru.eMES.Domain.TS.TS();
            //			TSErrorCode tsErrorCode = new TSErrorCode();
            //			//TSErrorCode2Location tsErrorCode2Location = new TSErrorCode2Location();
            //			
            //			tS.CardType = CardType.CardType_Product;;
            //			tS.FormTime = sim.MaintainTime;
            //			tS.FromDate = sim.MaintainDate;
            //			tS.FromInputType = TSFacade.TSSource_OnWIP;
            //			tS.FromUser = sim.MaintainUser;
            //			//tS.FromMemo = memo;
            //			tS.FromOPCode = sim.OPCode;
            //			tS.FromResourceCode = sim.ResourceCode;
            //			tS.FromRouteCode = sim.RouteCode;
            //			tS.FromSegmentCode = simReport.SegmentCode;
            //			tS.FromShiftCode = simReport.ShiftTypeCode;
            //			tS.FromShiftDay = simReport.ShiftDay;
            //			tS.FromShiftTypeCode = simReport.ShiftTypeCode;
            //			tS.FromStepSequenceCode = simReport.StepSequenceCode;
            //			tS.FromTimePeriodCode = simReport.TimePeriodCode;
            //			tS.FromUser = sim.MaintainUser;
            //			tS.ItemCode = sim.ItemCode;
            //			tS.MaintainDate = sim.MaintainDate ;
            //			tS.MaintainTime = sim.MaintainTime ;
            //			tS.MaintainUser = sim.MaintainUser;
            //			tS.MOCode = sim.MOCode;
            //			tS.ModelCode = sim.ModelCode;
            //			tS.RunningCard = sim.RunningCard;
            //			tS.RunningCardSequence = sim.RunningCardSequence;
            //			tS.SourceCard =sim.SourceCard;
            //			tS.SourceCardSequence =sim.SourceCardSequence;
            //			tS.TransactionStatus = TSFacade.TransactionStatus_None;
            //			tS.TranslateCard = sim.TranslateCard;
            //			tS.TranslateCardSequence = sim.TranslateCardSequence;
            //			tS.TSId = FormatHelper.GetUniqueID(sim.MOCode
            //				,tS.RunningCard,tS.RunningCardSequence.ToString());
            //			tS.TSStatus = TSStatus.TSStatus_New;
            //			tS.TSTimes = sim.NGTimes;
            //			tS.TSUser = sim.MaintainUser;

            object objExistTS = tsFacade.GetCardLastTSRecord(sim.RunningCard);

            if (objExistTS != null)
            {
                Domain.TS.TS existTS = objExistTS as Domain.TS.TS;

                if (existTS.TSStatus == Web.Helper.TSStatus.TSStatus_Complete)
                {
                    tsFacade.UpdateTSStatus(existTS.TSId, Web.Helper.TSStatus.TSStatus_New, existTS.MaintainUser);
                }
            }

            //			this.DataProvider.Insert( tS );

            //			string _rcard = _reject.RunningCard;
            //			string _rcardseq = _reject.RunningCardSequence.ToString();
            //			string _mocode	 = _reject.MOCode;
            //
            //			//ȡerrorcode
            //			object[] errorinfo= this.DataProvider.CustomQuery(typeof(Reject2ErrorCode),new SQLCondition(
            //				string.Format("select * from tblreject2errorcode  where RCARD='{0}' and RCARDSEQ='{1}' and MOCODE='{2}' ", _rcard,_rcardseq,_mocode )));
            //			if (errorinfo != null)
            //			{
            //				for (int i=0; i<errorinfo.Length; i++)
            //				{
            //					int j = tsFacade.QueryTSErrorCodeCount(((Reject2ErrorCode)errorinfo[i]).ErrorCodeGroup,
            //						((Reject2ErrorCode)errorinfo[i]).ErrorCode, tS.TSId);
            //					if (j==0)
            //					{				
            //						tsErrorCode.ErrorCode = ((Reject2ErrorCode)errorinfo[i]).ErrorCode;
            //						tsErrorCode.ErrorCodeGroup = ((Reject2ErrorCode)errorinfo[i]).ErrorCodeGroup;
            //						tsErrorCode.ItemCode = tS.ItemCode;
            //						tsErrorCode.MaintainDate = tS.MaintainDate;
            //						tsErrorCode.MaintainTime = tS.MaintainTime;
            //						tsErrorCode.MaintainUser = tS.MaintainUser;
            //						tsErrorCode.MOCode = tS.MOCode;
            //						tsErrorCode.ModelCode = tS.ModelCode;
            //						tsErrorCode.RunningCard = tS.RunningCard;
            //						tsErrorCode.RunningCardSequence = tS.RunningCardSequence;
            //						tsErrorCode.TSId = tS.TSId;
            //
            //						this.DataProvider.Insert ( tsErrorCode );
            //					}
            //					
            //				}
            //			}
        }

        //�����
        private void CollectErrorInfor(Simulation sim, SimulationReport simReport, string LOTNO)
        {
            TSFacade tsFacade = new TSFacade(_domainDataProvider);
            //			BenQGuru.eMES.Domain.TS.TS tS = new  BenQGuru.eMES.Domain.TS.TS();
            //			TSErrorCode tsErrorCode = new TSErrorCode();
            //			//TSErrorCode2Location tsErrorCode2Location = new TSErrorCode2Location();
            //			
            //			tS.CardType = CardType.CardType_Product;;
            //			tS.FormTime = sim.MaintainTime;
            //			tS.FromDate = sim.MaintainDate;
            //			tS.FromInputType = TSFacade.TSSource_OnWIP;
            //			tS.FromUser = sim.MaintainUser;
            //			//tS.FromMemo = memo;
            //			tS.FromOPCode = sim.OPCode;
            //			tS.FromResourceCode = sim.ResourceCode;
            //			tS.FromRouteCode = sim.RouteCode;
            //			tS.FromSegmentCode = simReport.SegmentCode;
            //			tS.FromShiftCode = simReport.ShiftTypeCode;
            //			tS.FromShiftDay = simReport.ShiftDay;
            //			tS.FromShiftTypeCode = simReport.ShiftTypeCode;
            //			tS.FromStepSequenceCode = simReport.StepSequenceCode;
            //			tS.FromTimePeriodCode = simReport.TimePeriodCode;
            //			tS.FromUser = sim.MaintainUser;
            //			tS.ItemCode = sim.ItemCode;
            //			tS.MaintainDate = sim.MaintainDate ;
            //			tS.MaintainTime = sim.MaintainTime ;
            //			tS.MaintainUser = sim.MaintainUser;
            //			tS.MOCode = sim.MOCode;
            //			tS.ModelCode = sim.ModelCode;
            //			tS.RunningCard = sim.RunningCard;
            //			tS.RunningCardSequence = sim.RunningCardSequence;
            //			tS.SourceCard =sim.SourceCard;
            //			tS.SourceCardSequence =sim.SourceCardSequence;
            //			tS.TransactionStatus = TSFacade.TransactionStatus_None;
            //			tS.TranslateCard = sim.TranslateCard;
            //			tS.TranslateCardSequence = sim.TranslateCardSequence;
            //			tS.TSId = FormatHelper.GetUniqueID(sim.MOCode
            //				,tS.RunningCard,tS.RunningCardSequence.ToString());
            //			tS.TSStatus = TSStatus.TSStatus_New;
            //			tS.TSTimes = sim.NGTimes;
            //			tS.TSUser = sim.MaintainUser;
            //			//tsFacade.AddTS(tS);
            //			this.DataProvider.Insert( tS );
            //
            object objExistTS = tsFacade.GetCardLastTSRecord(sim.RunningCard);

            if (objExistTS != null)
            {
                Domain.TS.TS existTS = objExistTS as Domain.TS.TS;

                if (existTS.TSStatus == Web.Helper.TSStatus.TSStatus_Complete)
                {
                    tsFacade.UpdateTSStatus(existTS.TSId, Web.Helper.TSStatus.TSStatus_New, existTS.MaintainUser);
                }
            }


            //			object[] errorinfo= this.DataProvider.CustomQuery(typeof(Domain.OQC.OQCLotCard2ErrorCode),new SQLParamCondition(
            //				string.Format("select * from tbloqclotcard2errorcode where rcard=$RCARD and rcardseq=$RCARDSEQ and lotno=$LOTNO"),
            //				new SQLParameter[]
            //				{
            //					new SQLParameter("RCARD",typeof(string),sim.RunningCard),
            //					new SQLParameter("RCARDSEQ",typeof(string),sim.RunningCardSequence),
            //					new SQLParameter("LOTNO",typeof(string),LOTNO)
            //				}));
            //			if (errorinfo != null)
            //			{
            //				for (int i=0; i<errorinfo.Length; i++)
            //				{
            //					int j = tsFacade.QueryTSErrorCodeCount(((Domain.OQC.OQCLotCard2ErrorCode)errorinfo[i]).ErrorCodeGroup,
            //						((Domain.OQC.OQCLotCard2ErrorCode)errorinfo[i]).ErrorCode, tS.TSId);
            //					if (j==0)
            //					{				
            //						tsErrorCode.ErrorCode = ((Domain.OQC.OQCLotCard2ErrorCode)errorinfo[i]).ErrorCode;
            //						tsErrorCode.ErrorCodeGroup = ((Domain.OQC.OQCLotCard2ErrorCode)errorinfo[i]).ErrorCodeGroup;
            //						tsErrorCode.ItemCode = tS.ItemCode;
            //						tsErrorCode.MaintainDate = tS.MaintainDate;
            //						tsErrorCode.MaintainTime = tS.MaintainTime;
            //						tsErrorCode.MaintainUser = tS.MaintainUser;
            //						tsErrorCode.MOCode = tS.MOCode;
            //						tsErrorCode.ModelCode = tS.ModelCode;
            //						tsErrorCode.RunningCard = tS.RunningCard;
            //						tsErrorCode.RunningCardSequence = tS.RunningCardSequence;
            //						tsErrorCode.TSId = tS.TSId;
            //						//tsFacade.AddTSErrorCode(tsErrorCode);
            //						this.DataProvider.Insert ( tsErrorCode );
            //					}
            //					
            //				}
            //			}
        }

        #endregion

        private object GetSimulation(string runningCard)
        {
            object[] simulations = this.DataProvider.CustomQuery(typeof(Simulation),
                new SQLParamCondition(string.Format("select {0} from TBLSIMULATION where RCARD = $RCARD order by MDATE*1000000+MTIME desc",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(Simulation))),
                new SQLParameter[] { new SQLParameter("RCARD", typeof(string), runningCard.ToUpper()) }));

            if (simulations == null)
                return null;
            if (simulations.Length > 0)
                return simulations[0];
            else
                return null;
        }

        private object GetSimulationReport(string runningCard)
        {
            object[] simReports = this.DataProvider.CustomQuery(typeof(SimulationReport),
                new SQLParamCondition(string.Format("select {0} from tblsimulationreport where RCARD = $RCARD order by MDATE*1000000+MTIME desc",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(SimulationReport))),
                new SQLParameter[] { new SQLParameter("RCARD", typeof(string), runningCard.ToUpper()) }));

            if (simReports == null)
                return null;
            if (simReports.Length > 0)
                return simReports[0];
            else
                return null;
        }

        /// <summary>
        /// ActionList ���˵������ڶ���
        /// </summary>
        /// <param name="actionList"></param>
        /// <returns></returns>
        private string GetSecondLastActionList(string actionList)
        {
            string[] aList = actionList.Split(';');

            if (aList.Length < 1)
            {
                return actionList;
            }

            int count = 0;
            for (int i = aList.Length - 1; i >= 0; i--)
            {
                if (aList[i].Trim() != string.Empty)
                {
                    count = i;
                    break;
                }
            }

            if (String.Compare(ActionType.DataCollectAction_OQCReject, aList[count], true) != 0)
            {
                return actionList;
            }

            if (count == 0 && String.Compare(ActionType.DataCollectAction_OQCReject, aList[count], true) == 0)
            {
                return String.Empty;
            }

            //the last action is OQCReject
            int index = 0;
            for (int i = count - 1; i >= 0; i--)
            {
                if (aList[i].Trim() != string.Empty)
                {
                    index = i;
                    break;
                }
            }

            string newActionList = ";";
            for (int i = 0; i <= index; i++)
            {
                newActionList += aList[i] + ";";
            }
            return newActionList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rcard"></param>
        /// <param name="rcardseq"></param>
        /// <returns>true:need to rewrite;false:not need to rewrite</returns>
        private bool CheckForTSReWrite(string rcard, string rcardseq)
        {
            int count = this.DataProvider.GetCount(
                new SQLParamCondition(
                string.Format("select count(rcard) from tblts where rcard = $RCARD and rcardseq =$RCARDSEQ and tsstatus = '{0}' ", TSStatus.TSStatus_New),
                new SQLParameter[]
				{ 
					new SQLParameter( "RCARD",typeof(string),rcard ) ,
					new SQLParameter( "RCARDSEQ",typeof(string),rcardseq ) 
				}));
            if (count > 1)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region private method for Cancel Release
        /// <summary>
        /// added by jessie lee for CS0044, 2005/10/12, P4.10
        /// ���ReworkSheet�Ƿ����ȡ���·������� 
        /// </summary>
        /// <param name="reworkSheet"></param>
        /// <returns>true:OK;false:not OK</returns>
        private bool CheckReworkSheetForCancelOpen(ReworkSheet reworkSheet)
        {
            //step1:check RunningCard Sequence to be the same
            string simSQL = string.Format(@"select {0}
					   from tblsimulation a
					  where exists (select rcard
								      from tblreject b
								     where reworkcode = '{1}'
								       and a.rcard = b.rcard
								       and a.rcardseq > b.rcardseq)",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(Simulation)), reworkSheet.ReworkCode);
            object[] simulations = this.DataProvider.CustomQuery(typeof(Simulation), new SQLCondition(simSQL));
            if (simulations != null)
            {
                return false;
            }

            // Marked by Hi1/Venus.feng on 20080729 for Hisense Version
            /*
			//step2:check routecode
			simSQL = string.Format(@"select {0}
									   from tblsimulation a
                                      where rcard in (select rcard
														from tblreject b
													   where reworkcode = '{1}'
														 and a.rcard = b.rcard )
									    and a.routecode = (select newroutecode 
															 from tblreworksheet 
                                                            where reworkcode = '{1}')  ",
				DomainObjectUtility.GetDomainObjectFieldsString( typeof(Simulation)),reworkSheet.ReworkCode );
			object[] simulations2 = this.DataProvider.CustomQuery( typeof(Simulation),new SQLCondition( simSQL )); 
			if( simulations != null )
			{
				return false;
			}
            */
            // End Marked

            //step3:check fromroute neither is empty
            simSQL = string.Format(@" select {0}
										from tblsimulation a
									   where exists (select rcard
												       from tblreject b
												      where reworkcode = '{1}'
												        and a.rcard = b.rcard
												        and a.rcardseq = b.rcardseq)
										 and fromroute is not null ",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(Simulation)), reworkSheet.ReworkCode);
            object[] simulations3 = this.DataProvider.CustomQuery(typeof(Simulation), new SQLCondition(simSQL));
            if (simulations != null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// added by jessie lee for CS0044, 2005/10/12, P4.10
        /// ���ReworkSheet�Ƿ����ȡ���·������� 
        /// </summary>
        /// <param name="reworkSheet"></param>
        /// <returns>true:OK;false:not OK</returns>
        private bool CheckReworkSheetForCancelOpen(ReworkSheet[] reworkSheets)
        {
            for (int i = 0; i < reworkSheets.Length; i++)
            {
                ReworkSheet reworkSheet = reworkSheets[i];
                if (!CheckReworkSheetForCancelOpen(reworkSheet))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region GetApprover
        /// <summary>
        /// ��ȡ��һλǩ����(�����ж�λ)
        /// </summary>
        /// <param name="reworkPass"></param>
        /// <returns>User</returns>
        public object[] GetNextApprover(ReworkPass reworkPass)
        {
            string sql = string.Format(@"select {0}
							from Tbluser
							where usercode in (select usercode
														from TBLREWORKPASS
														where reworkcode = $REWORKCODE
														and pseq =( select min(pseq) from TBLREWORKPASS where reworkcode = $REWORKCODE1 and pseq>$PSEQ ))",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(User)));
            Object[] objs = this.DataProvider.CustomQuery(typeof(User),
                new SQLParamCondition(sql,
                new SQLParameter[]{
									  new SQLParameter("$REWORKCODE",typeof(string),reworkPass.ReworkCode),
									  new SQLParameter("$REWORKCODE1",typeof(string),reworkPass.ReworkCode),
									  new SQLParameter("$PSEQ",typeof(decimal),reworkPass.PassSequence)	
								  }));
            if (objs == null || objs.Length == 0)
            {
                return null;
            }

            return objs;
        }

        /// <summary>
        /// ��ȡ��ǰǩ����
        /// </summary>
        /// <param name="reworkPass"></param>
        /// <returns>User</returns>
        public object[] GetFirstApprover(ReworkSheet reworkSheet)
        {
            string sql = string.Format(@"select {0}
							from Tbluser
							where usercode in (select usercode
														from TBLREWORKPASS
														where reworkcode = $REWORKCODE
														and pseq = ( select min(pseq) from TBLREWORKPASS where reworkcode = $REWORKCODE1 ))", DomainObjectUtility.GetDomainObjectFieldsString(typeof(User)));
            Object[] objs = this.DataProvider.CustomQuery(typeof(User),
                new SQLParamCondition(sql,
                new SQLParameter[]{
									  new SQLParameter("$REWORKCODE",typeof(string),reworkSheet.ReworkCode),
									  new SQLParameter("$REWORKCODE1",typeof(string),reworkSheet.ReworkCode)	
								  }));
            if (objs == null || objs.Length == 0)
            {
                return null;
            }

            return objs;
        }
        #endregion

        #region TempReworkLotNo
        public object[] GetTempReworkLotNo(string status)
        {
            string strsql = "";

            if (status == ReworkLot_Status_NEW)
            {
                strsql += "SELECT t1.lotno as LotNO, t1.itemcode as ItemCode, t1.muser as MUser, t1.mdate as MDate, t1.mtime as MTime, t2.mdesc as MDesc,";
                strsql += "       (SELECT COUNT (*)";
                strsql += "          FROM tbltempreworkrcard t3";
                strsql += "         WHERE t3.lotno = t1.lotno) AS TotalCount,";
                strsql += "       (SELECT COUNT (*)";
                strsql += "          FROM tbltempreworkrcard t3";
                strsql += "         WHERE t3.lotno = t1.lotno AND t3.status = '" + ReworkLot_Status_NEW + "') AS UNCONFIRMEDCOUNT";
                strsql += "  FROM tbltempreworklotno t1, tblmaterial t2";
                strsql += " WHERE t1.itemcode = t2.mcode AND t1.status = '" + ReworkLot_Status_NEW + "'";
            }
            else
            {
                strsql += "SELECT t1.lotno as LotNO, t1.itemcode as ItemCode, t1.muser as MUser, t1.mdate as MDate, t1.mtime as MTime, t2.mdesc as MDesc,";
                strsql += "       (SELECT COUNT (*)";
                strsql += "          FROM tbltempreworkrcard t3";
                strsql += "         WHERE t3.lotno = t1.lotno) AS TotalCount";
                strsql += "  FROM tbltempreworklotno t1, tblmaterial t2";
                strsql += " WHERE t1.itemcode = t2.mcode AND t1.status = '" + ReworkLot_Status_DEAL + "'";
            }

            return this.DataProvider.CustomQuery(typeof(TempRework), new SQLCondition(strsql));
        }

        public int QueryReworkLotNoNotConfirmCount(string lotNo)
        {
            string sql = string.Format("select count(*) from tbltempreworklotno where status='" + ReworkLot_Status_NEW + "' and lotno='{0}'", lotNo);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        public int QueryReworkLotNoConfirmCount(string lotNo)
        {
            string sql = string.Format("select count(*) from tbltempreworklotno where status='" + ReworkLot_Status_DEAL + "' and lotno='{0}'", lotNo);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        public int QueryReworkLotNoCount(string lotNo)
        {
            string sql = string.Format("select count(*) from tbltempreworklotno where lotno='{0}'", lotNo);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        public int QueryReworkRcardNotConfirmCount(string lotNo)
        {
            string sql = string.Format("select count(*) from tbltempreworkrcard where status='" + ReworkLot_Status_NEW + "' and lotno='{0}'", lotNo);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        public int QueryReworkRcardCount(string lotNo)
        {
            string sql = string.Format("select count(*) from tbltempreworkrcard where lotno='{0}'", lotNo);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        #endregion
    }
}

