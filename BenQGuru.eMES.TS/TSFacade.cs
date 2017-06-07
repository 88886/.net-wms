using System;
using System.Text;
using System.Runtime.Remoting;
using System.Collections;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.TS;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.MOModel;


namespace BenQGuru.eMES.TS
{
    /// <summary>
    /// TSFacade ��ժҪ˵����
    /// �ļ���:		TSFacade.cs
    /// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
    /// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
    /// ��������:	2005-05-31 15:34:59
    /// �޸���:
    /// �޸�����:
    /// �� ��:	
    /// �� ��:	
    /// </summary>
    public class TSFacade : MarshalByRefObject
    {
        private IDomainDataProvider _domainDataProvider = null;
        private FacadeHelper _helper = null;

        public TSFacade(IDomainDataProvider domainDataProvider)
        {
            this._domainDataProvider = domainDataProvider;
            this._helper = new FacadeHelper(DataProvider);
        }

        public TSFacade()
        {
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

        //Laws Lu,max life time to unlimited
        public override object InitializeLifetimeService()
        {
            return null;
        }


        public const string TSSource_OnWIP = "tssource_onwip";	//����ά��	Note by Simone Xu
        public const string TSSource_TS = "tssource_ts";		//����ά��  Note by Simone Xu
        public const string TSSource_RMA = "tssource_rma";		    //RMAά��  Note by Simone Xu

        public const string TransactionStatus_None = "none";

        public void ParseErrorInfo(string userCode, string errorCode, out ArrayList errList, out ArrayList errLocList, string factory)
        {
            /*ErrorCode Format
             * ����������1����������1^KeyName1#KeyName2����������2^KeyName3#KeyName4*����������2����������3^KeyName5#KeyName6 
            */


            errList = new ArrayList();
            errLocList = new ArrayList();

            //Laws Lu,2006/06/26 Get error code from Power FT 

            string[] errorParmInfo = errorCode.Split('*');
            //ArrayList errList = new ArrayList();
            if (errorCode != string.Empty)
            {
                foreach (string epinfo in errorParmInfo)//top loop
                {
                    string[] errorDetail = epinfo.Split(':');
                    for (int i = 0; i < errorDetail.Length; i++)//detail loop
                    {
                        //							if( i == 0)
                        //							{
                        //								//set error group
                        //								errinfo.ErrorCodeGroup = errorDetail[0].ToUpper().Trim();
                        //							}
                        //							else
                        //							{
                        if (i > 0)
                        {
                            ErrorCodeGroup2ErrorCode errinfo = new ErrorCodeGroup2ErrorCode();
                            errinfo.ErrorCodeGroup = errorDetail[0].ToUpper().Trim();

                            string[] errorLoc = errorDetail[i].Split('^');
                            for (int j = 0; j < errorLoc.Length; j++)//error code 2 location loop
                            {
                                #region set error code 2 error location

                                //									if(j == 0)
                                //									{
                                //										//set error code
                                //										
                                //									}
                                if (j > 0)
                                {
                                    errinfo.ErrorCode = errorLoc[0].ToUpper().Trim();

                                    string[] errorLocDetails = errorLoc[j].Split('#');
                                    for (int k = 0; k < errorLocDetails.Length; k++)//error location loop
                                    {
                                        TSErrorCode2Location errLoc = new TSErrorCode2Location();
                                        //set error location
                                        errLoc.ErrorCodeGroup = errinfo.ErrorCodeGroup;
                                        errLoc.ErrorCode = errinfo.ErrorCode;
                                        errLoc.ErrorLocation = errorLocDetails[k].ToUpper().Trim();
                                        errLoc.AB = "A";

                                        errLocList.Add(errLoc);
                                    }

                                }

                                #endregion
                            }
                            errList.Add(errinfo);
                        }

                    }
                }
            }

        }


        #region TS
        /// <summary>
        /// 
        /// </summary>
        public BenQGuru.eMES.Domain.TS.TS CreateNewTS()
        {
            return new BenQGuru.eMES.Domain.TS.TS();
        }

        public void AddTS(BenQGuru.eMES.Domain.TS.TS tS)
        {
            this.DataProvider.Insert(tS);
        }

        public void AddTS_New(BenQGuru.eMES.Domain.TS.TS tS)
        {
            this._helper.AddDomainObject(tS);
        }

        public void UpdateTS(BenQGuru.eMES.Domain.TS.TS tS)
        {
            this.DataProvider.Update(tS);
        }

        public void UpdateTS_New(BenQGuru.eMES.Domain.TS.TS tS)
        {
            //this.DataProvider.Update(tS);
            this._helper.UpdateDomainObject(tS);
        }

        /// <summary>
        /// Laws Lu
        /// 2005/10/31
        /// �Ż�Update Confirm��������������
        /// </summary>
        /// <param name="tS">TSʵ��</param>
        public void UpdateTSConfirmStatus(BenQGuru.eMES.Domain.TS.TS tS)
        {
            //			string updateSql ="update TBLTS set TSSTATUS = '" + tS.TSStatus 
            //				+"',CONFIRMUSER='" + tS.ConfirmUser 
            //				+"',CONFIRMDATE=" + tS.ConfirmDate 
            //				+",CONFIRMTIME=" + tS.ConfirmTime 
            //				+",COPCODE='TS'" 
            //				+",CRESCODE='" + tS.ConfirmResourceCode
            //				+",muser"+tS.MaintainUser
            //				+""+tS.MaintainDate
            //				+""+tS.MaintainTime
            //				+ "' where TSID='" + tS.TSId + "'";

            ///modified by jessie lee, 2005/11/24
            string updateSql = string.Format(
                @"update TBLTS set 
				TSSTATUS ='{0}',
				CONFIRMUSER='{1}',
				CONFIRMDATE={2},
				CONFIRMTIME={3},
				COPCODE='TS',
				CRESCODE='{4}',
				MUSER='{5}',MDATE={6},MTIME={7} WHERE TSID='{8}'",
                tS.TSStatus,
                tS.ConfirmUser,
                tS.ConfirmDate,
                tS.ConfirmTime,
                tS.ConfirmResourceCode,
                tS.MaintainUser,
                tS.MaintainDate,
                tS.MaintainTime,
                tS.TSId);
            this.DataProvider.CustomExecute(new SQLCondition(updateSql));
        }

        /// <summary>
        /// Laws Lu
        /// 2005/10/31
        /// �Ż�Update Status��������������
        /// </summary>
        /// <param name="tS">TSʵ��</param>
        public void UpdateTSStatus(BenQGuru.eMES.Domain.TS.TS tS)
        {
            string updateSql = "update TBLTS set TSSTATUS = '" + tS.TSStatus
                + "' where TSID='" + tS.TSId + "'";
            this.DataProvider.CustomExecute(new SQLCondition(updateSql));
        }

        /// <summary>
        /// Laws Lu
        /// 2005/10/31
        /// �Ż�Update ������������������
        /// </summary>
        /// <param name="tS">TSʵ��</param>
        public void UpdateTSReflowStatus(BenQGuru.eMES.Domain.TS.TS tS)
        {
            //			string updateSql ="update TBLTS set TSSTATUS = '" + tS.TSStatus 
            //				+"',REFMOCODE='" + tS.ReflowMOCode 
            //				+"',REFROUTECODE='" + tS.ReflowRouteCode 
            //				+"',REFOPCODE='" + tS.ReflowOPCode 
            //				+ "' where TSID='" + tS.TSId + "'";

            ///modified by jessie lee, 2005/11/24
            string updateSql = "update TBLTS set TSSTATUS = '" + tS.TSStatus
                + "',REFMOCODE='" + tS.ReflowMOCode
                + "',REFROUTECODE='" + tS.ReflowRouteCode
                + "',REFOPCODE='" + tS.ReflowOPCode
                + "',muser='" + tS.MaintainUser
                + "',tsuser='" + tS.TSUser
                + "',mdate=" + tS.MaintainDate
                + ",mtime=" + tS.MaintainTime
                + ",TSRESCODE= '" + tS.TSResourceCode
                + "',tsdate=" + tS.TSDate
                + ",tstime=" + tS.TSTime
                + " where TSID='" + tS.TSId + "'";
            this.DataProvider.CustomExecute(new SQLCondition(updateSql));
        }

        public void DeleteTS(BenQGuru.eMES.Domain.TS.TS tS)
        {
            this.DataProvider.Delete(tS);
        }

        public object GetTS(string tSId)
        {
            return this.DataProvider.CustomSearch(typeof(BenQGuru.eMES.Domain.TS.TS), new object[] { tSId });
        }

        public Decimal GetMaxTSTimes(string runningCard)
        {
            string sql = "SELECT NVL(MAX(tstimes), 0) AS tstimes FROM tblts WHERE rcard = '" + runningCard + "' "; ;
            object[] list = this.DataProvider.CustomQuery(typeof(Domain.TS.TS), new SQLCondition(sql));
            if (list == null)
            {
                return 0;
            }
            else
            {
                return ((Domain.TS.TS)list[0]).TSTimes;
            }
        }

        /// <summary>
        /// ** ��������:	��ѯTS��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <returns> TS���ܼ�¼��</returns>
        public int QueryTSCount(string tSId)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLTS where 1=1 and TSID like '{0}%' ", tSId)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯTS
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> TS����</returns>
        public object[] QueryTS(string tSId, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS), new PagerCondition(string.Format("select {0} from TBLTS where 1=1 and TSID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS)), tSId), "TSID", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�TS
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>TS���ܼ�¼��</returns>
        public object[] GetAllTS()
        {
            return this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS), new SQLCondition(string.Format("select {0} from TBLTS order by TSID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS)))));
        }

        public int QueryTSCountByLine(string runningCard, string stepSequenceCode)
        {
            return this.DataProvider.GetCount(new SQLParamCondition(@"select count(*) from TBLTS where RCARD = $rcard and FRMSSCODE= $frmsscode ", new SQLParameter[] { new SQLParameter("rcard", typeof(string), runningCard), new SQLParameter("frmsscode", typeof(string), stepSequenceCode) }));
        }

        public object QueryLastTSByRunningCard(string runningCard)
        {
            string sql = "SELECT {0} FROM tblts WHERE rcard = '{1}' ORDER BY mdate DESC, mtime DESC ";
            sql = string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(Domain.TS.TS)), runningCard);
            object[] list = this.DataProvider.CustomQuery(typeof(Domain.TS.TS), new SQLCondition(sql));
            if (list == null)
            {
                return null;
            }
            else
            {
                return list[0];
            }
        }

        public int QueryTSCountByLine(string runningCard, string stepSequenceCode, int rcardSeq)
        {
            return this.DataProvider.GetCount(new SQLParamCondition(@"select count(0) from (select distinct rcard from TBLTS where RCARD = $rcard and FRMSSCODE= $frmsscode)", new SQLParameter[]{
																																														   new SQLParameter("rcard",typeof(string),runningCard),
																																														   new SQLParameter("frmsscode",typeof(string),stepSequenceCode),
																																														   }));
        }
        public int QueryTSCountByMo(string runningCard, string moCode)
        {
            return this.DataProvider.GetCount(new SQLParamCondition(@"select count(0) from (select distinct rcard from TBLTS where RCARD = $RCARD and MOCODE= $MOCODE)", new SQLParameter[]{
																																																	 new SQLParameter("rcard",typeof(string),runningCard),
																																																	 new SQLParameter("MOCODE",typeof(string),moCode),
			}));
        }

        public int QueryTSCountLineByLotNo(string oqcLotNo, string stepSequenceCode)
        {
            if ((oqcLotNo.Trim().Length == 0) || (stepSequenceCode.Trim().Length == 0))
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }
            return this.DataProvider.GetCount(new SQLCondition(String.Format("select count(*) from (select distinct (rcard) from tblts where rcard in (select rcard from TBLLOT2CARDCHECK where lotno='{0}') where  FRMSSCODE= '{1}')", oqcLotNo, stepSequenceCode)));
        }

        public int QueryNoTSStusTSCountByRunningCard(string runningCard, string tsStatus, string moCode)
        {
            if ((runningCard.Trim().Length == 0) || (tsStatus.Trim().Length == 0) || (moCode.Trim().Length == 0))
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }
            return this.DataProvider.GetCount(new SQLCondition(String.Format("select count(*) from  tblts where rcard='{0}' and mocode='{1}' and tsstatus='{2}'", new object[] { runningCard, moCode, tsStatus })));
        }

        public bool IsCardScrapeOrSplit(string runningCard, string moCode, string rcardSeq)
        {
            if ((runningCard.Trim().Length == 0) || (moCode.Trim().Length == 0))
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }
            if (this.DataProvider.GetCount(new SQLCondition(
                String.Format("select count(*) from  tblts where rcard='{0}' and mocode='{1}' and rcardseq={2}"
                + " and (tsstatus = '" + TSStatus.TSStatus_Scrap + "'"
                + " or tsstatus = '" + TSStatus.TSStatus_Split + "'"
                + " or tsstatus = '" + TSStatus.TSStatus_Reflow + "'" + ")"
                , new object[] { runningCard, moCode, rcardSeq }))) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object[] GetCardScrapeOrSplit(string runningCard, string moCode, string rcardSeq)
        {
            if ((runningCard.Trim().Length == 0) || (moCode.Trim().Length == 0))
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }
            return this.DataProvider.CustomQuery(typeof(Domain.TS.TS), new SQLCondition(
                String.Format("select {0} from  tblts where rcard='{1}' and mocode='{2}' and rcardseq={3}"
                + " and (tsstatus = '" + TSStatus.TSStatus_Scrap + "'"
                + " or tsstatus = '" + TSStatus.TSStatus_Split + "'"
                //+" or tsstatus = '" + TSStatus.TSStatus_Reflow + "'"
                + ")"
                , new object[] { DomainObjectUtility.GetDomainObjectFieldsString(typeof(Domain.TS.TS)), runningCard, moCode, rcardSeq })));

        }

        //Laws Lu,2005/08/25	ע��
        public bool HaveNewStatusCardInOQCLot(string oqcLot, string moCode)
        {
            //			if( (oqcLot.Trim().Length ==0)||(moCode.Trim().Length==0))
            //			{
            //				ExceptionManager.Raise(this.GetType().BaseType,"$Error_System_Error");
            //			}
            if ((oqcLot.Trim().Length == 0))
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            // Modified by Jane Shu		Date:2005/08/09
            //			if( this.DataProvider.GetCount(new SQLCondition(String.Format("select count(*) from  tblts where  mocode='{0}' and tsstatus <> '"
            //				+ TSStatus.TSStatus_New
            //				+ "' and tsstatus <> '" + TSStatus.TSStatus_Reflow + "'"
            //				+ " and tsstatus <> '" + TSStatus.TSStatus_Scrap + "'"
            //				+ " and tsstatus <> '" + TSStatus.TSStatus_Split + "'"
            //				+" and rcard in (select rcard from tbllot2card where lotno='{0}')" , new object[]{oqcLot,moCode}))) >0)
            //			{
            //				return true;
            //			}
            //			else
            //			{
            //				return false;
            //			}
            //Laws Lu,2005/11/05,�޸�	FQC��Ҫ֧�ֻ쵥
            if (this.DataProvider.GetCount(new SQLCondition(String.Format("select count(*) from  tblts where tsstatus <> '"
                + TSStatus.TSStatus_New
                + "' and tsstatus <> '" + TSStatus.TSStatus_Complete + "'"
                + " and tsstatus <> '" + TSStatus.TSStatus_Reflow + "'"
                + " and tsstatus <> '" + TSStatus.TSStatus_Scrap + "'"
                + " and tsstatus <> '" + TSStatus.TSStatus_Split + "'"
                + " and tsstatus <> '" + TSStatus.TSStatus_RepeatNG + "'"
                + " and rcard in (select rcard from tbllot2card where lotno='{0}')", new object[] { oqcLot }))) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Laws Lu

        public bool CheckCardInTS(string moCode, string runningCard, string cardSequnce)
        {
            bool bReturn;
            if (moCode.Trim().Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            // Modified by Jane Shu		Date:2005/08/09
            //Laws Lu,2005/08/25	�޸�	
            //������������ɸ��Ѿ�����������ȷ�ϡ�תΪ�����ޡ�״̬��������ά���У����˲����Լ���
            if (this.DataProvider.GetCount(new SQLCondition(String.Format("select count(*) from  tblts where  mocode='{0}' and tsstatus <> '"
                + TSStatus.TSStatus_New
                + "' and tsstatus <> '" + TSStatus.TSStatus_Reflow + "'"
                + " and tsstatus <> '" + TSStatus.TSStatus_Scrap + "'"
                + " and tsstatus <> '" + TSStatus.TSStatus_Split + "'"
                + " and RCARD ='{1}' and RCARDSEQ = '{2}'", new object[] { moCode, runningCard, cardSequnce }))) > 0)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }

            return bReturn;

        }

        public void DeleteCardInTS(string moCode, string runningCard, string cardSequnce)
        {
            if (moCode.Trim().Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            // Modified by Jane Shu		Date:2005/08/09
            //Laws Lu,2005/08/25	�޸�	
            //����ȫ�������������ǡ����ޡ�״̬��û�о�������ȷ�ϣ�����ʱ��������.ɾ��TS�е�������Ϣ

            if (this.DataProvider.GetCount(new SQLCondition(String.Format("select count(*) from  tblts where  mocode='{0}' and tsstatus = '"
                + TSStatus.TSStatus_New + "' and "
                + " RCARD ='{1}' and RCARDSEQ = '{2}'", new object[] { moCode, runningCard, cardSequnce }))) > 0)
            {
                this.DataProvider.CustomExecute(new SQLCondition(
                        String.Format("update TBLTS set tsstatus='" + TSStatus.TSStatus_Complete + "' where RCARD ='{0}' and RCARDSEQ = '{1}' and TSSTATUS ='{2}'"
                    , runningCard, cardSequnce, TSStatus.TSStatus_New)));

                //				this.DataProvider.CustomExecute(new SQLCondition(
                //					String.Format("delete from TBLTSERRORCODE where RCARD ='{0}' and RCARDSEQ = '{1}'"
                //					,runningCard,cardSequnce)));
            }

        }

        /// <summary>
        /// �жϲ�Ʒ���к���ά�޼�¼���Ƿ����
        /// </summary>
        /// <param name="mnid"></param>
        /// <returns></returns>
        public bool IsCardInTS(string rcard)
        {
            if (rcard.Trim().Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            // Modified by Jane Shu		Date:2005/08/09
            if (this.DataProvider.GetCount(new SQLParamCondition(@"select count(*) from tblts where rcard =$rcard ", new SQLParameter[] { new SQLParameter("rcard", typeof(string), rcard.Trim()) })) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RunningCardCanBeClollected(string runningCard, string cardType)
        {
            string sql = string.Empty;
            sql += "SELECT COUNT(*) ";
            sql += "FROM tblts ";
            sql += "WHERE 1 = 1 ";
            sql += "AND rcard = '" + runningCard.Trim().ToUpper() + "' ";
            sql += "AND cardtype = '" + cardType.Trim() + "'";
            sql += "AND frminputtype = '" + TSSource.TSSource_TS + "' ";
            sql += "AND tsstatus IN ('" + TSStatus.TSStatus_New + "', '" + TSStatus.TSStatus_Confirm + "', '" + TSStatus.TSStatus_TS + "', '" + TSStatus.TSStatus_Scrap + "') ";

            if (this.DataProvider.GetCount(new SQLCondition(sql)) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object GetCardLastTSRecordInNewStatus(string rcard)
        {
            if (rcard.Trim().Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            // Modified by Jane Shu		Date:2005/08/09
            //Laws Lu��2005/10/26���޸�	������������
            object[] objs = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS),
                new SQLParamCondition(String.Format(@"SELECT {0}  FROM tblts WHERE rcard=$RCARD1 and tsstatus=$tsstatus"
                , DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS)))
                , new SQLParameter[] { 
										new SQLParameter("RCARD",typeof(string),rcard.Trim())
										,new SQLParameter("tsstatus",typeof(string),TSStatus.TSStatus_New)}));
            if (objs != null)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Laws Lu,2005/10/11
        /// ��ȡǰһ��TS�ļ�¼
        /// </summary>
        /// <param name="rcard">Running Card</param>
        /// <param name="rcardseq">Running Card Sequence</param>
        /// <param name="mocode">������</param>
        /// <returns>TSʵ��</returns>
        public object GetCardPreviousTSRecord(string rcard, string rcardseq, string mocode)
        {
            if (rcard.Trim().Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            if (rcardseq.Trim().Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            if (mocode.Trim().Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            object[] objs = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS)
                , new SQLCondition(String.Format(@"select * from (select {0} from tblts where rcard='{1}' and rcardseq < {2} and mocode = '{3}'  order by rcardseq DESC) where  rownum = 1"
                , new object[]{DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS))
								 ,rcard
								 ,rcardseq
								 ,mocode
							 })));

            if (objs == null)
            {
                return null;
            }
            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }

            return null;
        }

        public object GetCardLastTSRecordInConfirmOrTSStatus(string rcard)
        {
            if (rcard.Trim().Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            // Modified by Jane Shu		Date:2005/08/09
            //Laws Lu��2005/10/26���޸�	������������
            object[] objs = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS),
                new SQLParamCondition(String.Format(
                @"SELECT {0}  FROM tblts WHERE tsid = (select tsid
									from (SELECT tsid
											FROM tblts
											WHERE rcard = $RCARD
											order by MDATE * 1000000 + MTIME DESC)
									where rownum = 1) and tsstatus in ('" + TSStatus.TSStatus_Confirm + "','" + TSStatus.TSStatus_TS + "') ",
                                                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS))),
                                        new SQLParameter[] { 
															   new SQLParameter("RCARD",typeof(string),rcard.Trim())}));
            if (objs != null)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }


        public object GetCardLastTSRecordInTSStatus(string rcard)
        {
            if (rcard.Trim().Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_System_Error");
            }

            // Modified by Jane Shu		Date:2005/08/09
            //Laws Lu��2005/10/26���޸�	������������
            object[] objs = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS),
                new SQLParamCondition(String.Format(
                @"SELECT {0}  FROM tblts WHERE tsid = (select tsid
									from (SELECT tsid
											FROM tblts
											WHERE rcard = $RCARD
											order by MDATE * 1000000 + MTIME DESC)
									where rownum = 1) and tsstatus=$TSSTATUS ",
                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS))),
                new SQLParameter[] { 
									   new SQLParameter("RCARD",typeof(string),rcard.Trim()),
									   new SQLParameter("TSSTATUS",typeof(string),TSStatus.TSStatus_TS)}));
            if (objs != null)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ** ��������:	�ɲ�Ʒ���кŻ�����µ�һ��ά�޼�¼
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-17
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="rcard"></param>
        /// <returns></returns>
        public object GetCardLastTSRecord(string runningCard)
        {
            return GetCardLastTSRecord(runningCard, string.Empty);
        }
        public object GetCardLastTSRecord(string runningCard, string moCode)
        {
            if (runningCard.Trim().Length <= 0)
            {
                return null;
            }

            string sql = string.Empty;
            sql += string.Format("SELECT {0} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS)));
            sql += "FROM tblts ";
            sql += "WHERE tsid = ";
            sql += "  ( ";
            sql += "  SELECT tsid ";
            sql += "  FROM ";
            sql += "    ( ";
            sql += "    SELECT tsid ";
            sql += "    FROM tblts ";
            sql += string.Format("    WHERE rcard = '{0}' ", runningCard.Trim());
            if (moCode.Trim().Length > 0)
            {
                sql += string.Format("    AND mocode = '{0}' ", moCode.Trim());
            }
            sql += "    ORDER BY mdate * 1000000 + mtime DESC";
            sql += "    )";
            sql += "  WHERE rownum = 1)";

            object[] list = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS), new SQLCondition(sql));

            if (list != null && list.Length > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��ѯά�޼�¼
        /// </summary>
        /// <param name="rcard">֧��ģ����ѯ����Ʒ���к�</param>
        /// <param name="itemCode">֧��ģ����ѯ����Ʒ����</param>
        /// <param name="moCode">֧��ģ����ѯ����������</param>
        /// <param name="segmentCode">֧��ģ����ѯ�����δ���</param>
        /// <param name="stepsequenceCode">֧��ģ����ѯ�����ߴ���</param>
        /// <param name="resourceCode">֧��ģ����ѯ����Դ����</param>
        /// <param name="startDate"></param>
        /// <param name="startTime"></param>
        /// <param name="endDate"></param>
        /// <param name="endTime"></param>
        /// <param name="IsNewStatus"></param>
        /// <returns></returns>
        public object[] IllegibilityQueryTS(string rcard, string itemCode, string moCode, string operationcode, string stepsequenceCode, string resourceCode, int startDate, int startTime, int endDate, int endTime, bool IsNewStatus)
        {
            // Modified by Jane Shu		Date:2005/08/09
            StringBuilder stringBuilder = new StringBuilder(string.Format(@"select {0} from tblts where rcard like '{1}%' ",
                new object[]{
								DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS)),
				                rcard.Trim()}));

            // Added By Jane Shu	Date:2005/08/15
            if (operationcode != null && operationcode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and frmopcode like '{0}%'", operationcode));
            }

            if (stepsequenceCode != null && stepsequenceCode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and frmsscode like '{0}%'", stepsequenceCode));
            }

            if (resourceCode != null && resourceCode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and frmrescode like '{0}%'", resourceCode));
            }
            //Laws Lu,2005/08/24,����
            if (itemCode != null && itemCode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and itemcode like '{0}%'", itemCode));
            }
            if (moCode != null && moCode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and mocode like '{0}%'", moCode));
            }

            if (startDate != endDate)
            {
                stringBuilder.Append(String.Format(@" and (frmdate between {0} and {1} or (frmdate= {2} and frmtime >={3}) or (frmdate ={4} and frmtime <={5}))", new object[] { startDate + 1, endDate - 1, startDate, startTime, endDate, endTime }));
            }
            else
            {
                stringBuilder.Append(string.Format(@" and (frmdate= {0} and frmtime between {1} and {2})", new object[] { startDate, startTime, endTime }));
            }

            if (IsNewStatus)
            {
                stringBuilder.Append(" and tsstatus = '" + TSStatus.TSStatus_New + "'");
            }
            else
            {
                //stringBuilder.Append( " and tsstatus in ('"+TSStatus.TSStatus_New+"','"+TSStatus.TSStatus_Confirm+"')");
                //Laws Lu,2005/08/24,�޸�
                //				stringBuilder.Append( " and tsstatus in in ('"+TSStatus.TSStatus_Confirm+"')");
                stringBuilder.Append(" and tsstatus <> '" + TSStatus.TSStatus_New + "'");
            }
            return this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS), new SQLCondition(stringBuilder.ToString()));
        }

        public object[] IllegibilityQueryTS(string rcard, string itemCode, string moCode, string operationcode, string stepsequenceCode, string resourceCode, int startDate, int startTime, int endDate, int endTime, string tsStatus)
        {
            //			StringBuilder stringBuilder = new StringBuilder(String.Format(@"select {0} from tblts wherercard like '{1}%' ",

            string selectColunms = DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(Domain.TS.TS)).Replace(DomainObjectUtility.GetTableMapAttribute(typeof(Domain.TS.TS)).TableName, "tblts");
            StringBuilder stringBuilder = new StringBuilder(String.Format(@"select {0},tblitem.itemname as itemname from tblts LEFT OUTER JOIN tblitem ON tblts.itemcode=tblitem.itemcode " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " where 1 = 1  ", selectColunms));

            if (rcard != null && rcard != string.Empty)
            {
                stringBuilder.Append(string.Format(" and tblts.rcard like '{0}%'", rcard));
            }
            // Added By Jane Shu	Date:2005/08/15
            if (operationcode != null && operationcode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and tblts.frmopcode like '{0}%'", operationcode));
            }

            if (stepsequenceCode != null && stepsequenceCode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and tblts.frmsscode like '{0}%'", stepsequenceCode));
            }

            if (resourceCode != null && resourceCode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and tblts.frmrescode like '{0}%'", resourceCode));
            }

            //Laws Lu,2005/08/24,����
            if (itemCode != null && itemCode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and tblts.itemcode like '{0}%'", itemCode));
            }
            if (moCode != null && moCode != string.Empty)
            {
                stringBuilder.Append(string.Format(" and tblts.mocode like '{0}%'", moCode));
            }

            if (tsStatus != null && tsStatus != string.Empty)
            {
                if (tsStatus == TSStatus.TSStatus_Complete)
                {
                    stringBuilder.Append(string.Format(" and  (tblts.tsstatus = '{0}' or tblts.tsstatus ='{1}')", TSStatus.TSStatus_Complete, TSStatus.TSStatus_Reflow));
                    //stringBuilder.Append(string.Format(" and tsstatus='{0}'", ));
                }
                else
                    stringBuilder.Append(string.Format(" and tsstatus='{0}'", tsStatus));
            }

            if (startDate != endDate)
            {
                stringBuilder.Append(String.Format(@" and (tblts.frmdate between {0} and {1} or (tblts.frmdate= {2} and tblts.frmtime >={3}) or (tblts.frmdate ={4} and tblts.frmtime <={5}))", new object[] { startDate + 1, endDate - 1, startDate, startTime, endDate, endTime }));
            }
            else
            {
                stringBuilder.Append(string.Format(@" and (tblts.frmdate= {0} and tblts.frmtime between {1} and {2})", new object[] { startDate, startTime, endTime }));
            }

            //stringBuilder.Append(" order by rcard,itemcode,mocode,frmopcode,frmsscode,frmrescode,tsstatus");

            return this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TSAndItemName), new SQLCondition(stringBuilder.ToString()));
        }

        /// <summary>
        /// ** ��������:	����ά�޼�¼״̬
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-17
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsid"></param>
        /// <param name="tsStatus"></param>
        /// <param name="userCode"></param>
        public void UpdateTSStatus(string tsid, string tsStatus, string userCode)
        {
            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            this.DataProvider.CustomExecute(new SQLParamCondition(
                "update tblts set tsstatus=$tsstatus1, muser=$muser, mdate=$mdate, mtime=$mtime where tsid=$tsid and tsstatus<>$tsstatus2",
                new SQLParameter[] { 
									   new SQLParameter( "tsstatus1", typeof(string), tsStatus ),
									   new SQLParameter( "muser",	typeof(string), userCode ) ,
									   new SQLParameter( "mdate",	typeof(int), dbDateTime.DBDate ) ,
									   new SQLParameter( "mtime",	typeof(int), dbDateTime.DBTime ) , 
									   new SQLParameter( "tsid",	typeof(string), tsid ),
									   new SQLParameter( "tsstatus2", typeof(string), tsStatus )
								   }
                ));
        }

        public object GetTSInfoOfSameMO(string rcard, BenQGuru.eMES.Domain.TS.TS otherTS, object[] infos)
        {
            if (rcard.Trim().Length == 0 || otherTS == null)
            {
                return null;
            }

            // Modified by Jane Shu		Date:2005/08/09
            //Laws Lu��2005/10/26���޸�	������������
            /*
            object[] objs =  this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS),  
                new SQLParamCondition( String.Format(@"SELECT {0}  FROM tblts WHERE rcardseq = (select RCARDSEQ from (select RCARDSEQ,MDATE,MTIME from (SELECT RCARDSEQ,MDATE,MTIME FROM tblts WHERE rcard = $RCARD) order by MDATE*1000000+MTIME DESC) where rownum =1) and rcard=$RCARD1",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS))) ,
                new SQLParameter[] { 
                                       new SQLParameter("RCARD",typeof(string),rcard.Trim()),
                                       new SQLParameter("RCARD1",typeof(string),rcard.Trim()) }));
            */

            /* modified by jessie lee, 2005/12/14,  */

            string ecgcodes = "";
            string ecodes = "";

            if (infos != null && infos.Length > 0)
            {
                ecgcodes = "and at.ECGCODE in (";
                ecodes = "and at.ecode in (";

                foreach (TSErrorInfo info in infos)
                {
                    //					if ( info.ErrorCauseList == null )
                    //					{
                    //						continue;
                    //					}
                    //
                    //					foreach ( TSErrorCause errCause in info.ErrorCauseList )
                    //					{
                    ecgcodes += "'" + info.ErrorCodeGroup + "',";
                    ecodes += "'" + info.ErrorCode + "',";
                    //					}

                }


                ecgcodes = ecgcodes.Substring(0, ecgcodes.Length - 1) + ") ";
                ecodes = ecodes.Substring(0, ecodes.Length - 1) + ") ";
            }

            string sql = String.Format(
                "SELECT {0} FROM tblts WHERE tsid = (select tsid from (SELECT distinct a.tsid,a.MDATE,a.MTIME FROM tblts a,tblts b,TBLTSERRORCODE at" +
                " WHERE a.MOCODE='{1}' and a.itemcode='{2}' and a.tsid=at.tsid  " +
                " {3} {4}  and a.tsstatus='{5}'  " +
                " order by a.MDATE * 1000000 + a.MTIME DESC) where rownum = 1)",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS)),
                otherTS.MOCode, otherTS.ItemCode, ecgcodes, ecodes, TSStatus.TSStatus_Reflow);

            /*string sql =  String.Format(
                "SELECT {0} FROM tblts WHERE tsid = (select tsid from (SELECT distinct a.tsid,a.MDATE,a.MTIME FROM tblts a,tblts b,TBLTSERRORCODE at,TBLTSERRORCODE bt "+
                " WHERE b.rcard = $RCARD and b.MOCODE=a.MOCODE and a.itemcode=b.itemcode and a.tsid=at.tsid and b.tsid=bt.tsid and "+
                " bt.ECGCODE= at.ecgcode and bt.ECODE=at.ecode  and a.tsstatus='{1}' "+
                " order by a.MDATE * 1000000 + a.MTIME DESC) where rownum = 1)",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TS)),
                TSStatus.TSStatus_Complete);*/

            object[] objs = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS),
                new SQLParamCondition(sql,
                new SQLParameter[] { 
									   new SQLParameter("RCARD",typeof(string),rcard.Trim()) }));
            if (objs != null)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region TSItem
        public object[] ExtraQueryTSItem(string tsID)
        {
            // Modified by Jane Shu		Date:2005/08/09
            return this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TSItem),
                new SQLParamCondition(string.Format("select {0} from TBLTSITEM where tsid=$tsid order by mitemcode",
                                                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TSItem))),
                                        new SQLParameter[]{ 
															  new SQLParameter("tsid", typeof(string), tsID) }));
        }

        public void AddTSItem(TSItem tsItem)
        {
            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            tsItem.ItemSequence = this.GetUniqueTSItemSequence(tsItem.TSId);
            tsItem.MaintainDate = dbDateTime.DBDate;
            tsItem.MaintainTime = dbDateTime.DBTime;

            this.DataProvider.Insert(tsItem);
        }

        public void UpdateTSItem(TSItem tsItem)
        {
            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            tsItem.MaintainDate = dbDateTime.DBDate;
            tsItem.MaintainTime = dbDateTime.DBTime;

            this.DataProvider.Update(tsItem);
        }

        public void DeleteTSItem(TSItem tsItem)
        {
            this.DataProvider.Delete(tsItem);
        }

        public void DeleteTSItem(string tsid, string[] itemseqs, string[] errTS)
        {
            if (itemseqs == null || itemseqs.Length == 0)
            {
                return;
            }

            this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete TBLTSITEM where tsid='{0}' and itemseq in ({1})", tsid, string.Join(",", itemseqs))));
            //Laws Lu,2005/12/22,����	ɾ��TS��ά�޵� ����������Ϣ
            foreach (string rcard in errTS)
            {
                object obj = this.GetCardLastTSRecord(rcard);

                if (obj != null)
                {
                    if ((obj as Domain.TS.TS).TSStatus == TSStatus.TSStatus_New)
                    {
                        this.DeleteTS(obj as Domain.TS.TS);
                        this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete TBLTSERRORCODE where RCARD in ('{0}') ", rcard)));
                    }
                    else
                    {
                        throw new Exception("$CSError_Card_TSStatus_Error");
                    }
                }
                //this.DataProvider.CustomExecute( new SQLCondition(string.Format("delete TBLTS where RCARD in ('{0}')", string.Join("','", errTS) )) );

            }


        }

        public TSItem CreateTSItem()
        {
            return new TSItem();
        }

        public decimal GetUniqueTSItemSequence(string tsid)
        {
            object[] objs = this.DataProvider.CustomQuery(typeof(TSItem),
                new PagerParamCondition("select ITEMSEQ from TBLTSITEM where TSID=$TSID",
                "ITEMSEQ desc", 1, 1, new SQLParameter[] { new SQLParameter("TSID", typeof(string), tsid) }));

            if (objs == null || objs.Length < 1)
            {
                return 0;
            }

            return ((TSItem)objs[0]).ItemSequence + 1;
        }
        #endregion

        #region TSSMTItem
        /// <summary>
        /// 
        /// </summary>
        public TSSMTItem CreateNewTSSMTItem()
        {
            return new TSSMTItem();
        }

        public void AddTSSMTItem(TSSMTItem tSSMTItem)
        {
            this.DataProvider.Insert(tSSMTItem);
        }

        public void UpdateTSSMTItem(TSSMTItem tSSMTItem)
        {
            this.DataProvider.Update(tSSMTItem);
        }

        public void DeleteTSSMTItem(TSSMTItem tSSMTItem)
        {
            this.DataProvider.Delete(tSSMTItem);
        }

        public void DeleteTSSMTItem(TSSMTItem[] tSSMTItem)
        {
            for (int i = 0; i < tSSMTItem.Length; i++)
            {
                this.DeleteTSSMTItem(tSSMTItem[i]);
            }
        }

        public void DeleteTSSMTItem(string tsId, string itemSequence)
        {
            string strSql = "DELETE FROM tblTSSMTItem WHERE TSID='" + tsId + "' AND ITEMSEQ IN (" + itemSequence + ") ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
        }

        public object GetTSSMTItem(decimal itemSequence, string tSId)
        {
            return this.DataProvider.CustomSearch(typeof(TSSMTItem), new object[] { itemSequence, tSId });
        }

        /// <summary>
        /// ** ��������:	��ѯTSSMTItem��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-29 17:22:10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="itemSequence">ItemSequence��ģ����ѯ</param>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <returns> TSSMTItem���ܼ�¼��</returns>
        public int QueryTSSMTItemCount(decimal itemSequence, string tSId)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLTSSMTITEM where 1=1 and ITEMSEQ like '{0}%'  and TSID like '{1}%' ", itemSequence, tSId)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯTSSMTItem
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-29 17:22:10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="itemSequence">ItemSequence��ģ����ѯ</param>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> TSSMTItem����</returns>
        public object[] QueryTSSMTItem(decimal itemSequence, string tSId, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(TSSMTItem), new PagerCondition(string.Format("select {0} from TBLTSSMTITEM where 1=1 and ITEMSEQ like '{1}%'  and TSID like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSSMTItem)), itemSequence, tSId), "ITEMSEQ,TSID", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�TSSMTItem
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-29 17:22:10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>TSSMTItem���ܼ�¼��</returns>
        public object[] GetAllTSSMTItem()
        {
            return this.DataProvider.CustomQuery(typeof(TSSMTItem), new SQLCondition(string.Format("select {0} from TBLTSSMTITEM order by ITEMSEQ,TSID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSSMTItem)))));
        }

        public object[] ExtraQueryTSSMTItem(string tsID)
        {
            return this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TSSMTItem),
                new SQLParamCondition(string.Format("select {0} from TBLTSSMTITEM where tsid=$tsid order by mitemcode",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.TS.TSSMTItem))),
                new SQLParameter[]{ 
									  new SQLParameter("tsid", typeof(string), tsID) }));
        }

        public int GetTSSMTItemMaxSeq(string tsId)
        {
            string strSql = "SELECT MAX(ItemSeq) EAttribute1 FROM tblTSSMTItem WHERE TSID='" + tsId + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(TSSMTItem), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                TSSMTItem item = (TSSMTItem)objs[0];
                if (item.EAttribute1 != string.Empty)
                    return Convert.ToInt32(item.EAttribute1);
            }
            return 0;
        }

        #endregion

        #region TSErrorCause
        /// <summary>
        /// 
        /// </summary>
        public TSErrorCause CreateNewTSErrorCause()
        {
            return new TSErrorCause();
        }

        public void AddTSErrorCause(TSErrorCause tSErrorCause)
        {
            this.UpdateTSStatus(tSErrorCause.TSId, TSStatus.TSStatus_TS, tSErrorCause.MaintainUser);
            this.DataProvider.Insert(tSErrorCause);
        }
        public void AddTSErrorCauseOnly(TSErrorCause tSErrorCause)
        {
            this.DataProvider.Insert(tSErrorCause);
        }

        public void AddTSErrorCause_New(TSErrorCause tSErrorCause)
        {
            this._helper.AddDomainObject(tSErrorCause);
        }

        public void AddTSErrorCause(string currentUser, TSErrorCause tSErrorCause)
        {
            this.UpdateTSStatus(tSErrorCause.TSId, TSStatus.TSStatus_TS, currentUser);
            this.DataProvider.Insert(tSErrorCause);
        }

        public void UpdateTSErrorCause(TSErrorCause tSErrorCause)
        {
            this.DataProvider.Update(tSErrorCause);
        }

        public object GetTSErrorCause(string errorCauseCode, string errorCode, string errorCodeGroup, string tSId)
        {
            return this.DataProvider.CustomSearch(typeof(TSErrorCause), new object[] { errorCauseCode, errorCode, errorCodeGroup, tSId });
        }

        /// <summary>
        /// ** ��������:	��ѯTSErrorCause��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCauseCode">ErrorCauseCode��ģ����ѯ</param>
        /// <param name="errorCode">ErrorCode��ģ����ѯ</param>
        /// <param name="errorCodeGroup">ErrorCodeGroup��ģ����ѯ</param>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <returns> TSErrorCause���ܼ�¼��</returns>
        public int QueryTSErrorCauseCount(string errorCauseCode, string errorCode, string errorCodeGroup, string tSId)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLTSERRORCAUSE where 1=1 and ECSCODE like '{0}%'  and TBLEC_ECODE like '{1}%'  and ECGCODE like '{2}%'  and TSID like '{3}%' ", errorCauseCode, errorCode, errorCodeGroup, tSId)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯTSErrorCause
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCauseCode">ErrorCauseCode��ģ����ѯ</param>
        /// <param name="errorCode">ErrorCode��ģ����ѯ</param>
        /// <param name="errorCodeGroup">ErrorCodeGroup��ģ����ѯ</param>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> TSErrorCause����</returns>
        public object[] QueryTSErrorCause(string errorCauseCode, string errorCode, string errorCodeGroup, string tSId, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause), new PagerCondition(string.Format("select {0} from TBLTSERRORCAUSE where 1=1 and ECSCODE like '{1}%'  and TBLEC_ECODE like '{2}%'  and ECGCODE like '{3}%'  and TSID like '{4}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause)), errorCauseCode, errorCode, errorCodeGroup, tSId), "ECSCODE,TBLEC_ECODE,ECGCODE,TSID", inclusive, exclusive));
        }

        public object[] QueryTSErrorCause(string tSId, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause), new PagerCondition(string.Format("select {0} from TBLTSERRORCAUSE where 1=1 and TSID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause)), tSId), "TSID", inclusive, exclusive));
        }

        public object[] ExtraQueryTSErrorCause(string tsID)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause), new SQLParamCondition(String.Format("select {0} from TBLTSERRORCAUSE where tsid = $tsid", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause))), new SQLParameter[] { new SQLParameter("tsid", typeof(string), tsID) }));
        }

        /// <summary>
        /// ** ��������:	������е�TSErrorCause
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>TSErrorCause���ܼ�¼��</returns>
        public object[] GetAllTSErrorCause()
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause), new SQLCondition(string.Format("select {0} from TBLTSERRORCAUSE order by ECSCODE,TBLEC_ECODE,ECGCODE,TSID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause)))));
        }
        #endregion

        #region TSErrorCode
        /// <summary>
        /// 
        /// </summary>
        public TSErrorCode CreateNewTSErrorCode()
        {
            return new TSErrorCode();
        }

        public void AddTSErrorCode(TSErrorCode tSErrorCode)
        {
            this.DataProvider.Insert(tSErrorCode);
        }

        public void AddTSErrorCode_New(TSErrorCode tSErrorCode)
        {
            this._helper.AddDomainObject(tSErrorCode);
        }

        public void UpdateTSErrorCode(TSErrorCode tSErrorCode)
        {
            this.DataProvider.Update(tSErrorCode);
        }

        public void DeleteTSErrorCode(TSErrorCode tSErrorCode)
        {
            this.DataProvider.Delete(tSErrorCode);
        }

        public void DeleteTSErrorCodeWithChildrenWith(IDomainDataProvider currentDataProvider, TSErrorCode tSErrorCode)
        {
            string deletSQLForTSERRORCAUSE2EPART = String.Format(@" delete from tbltserrorcause2epart where ECODE='{0}'  and ECGCODE='{1}' and tsid ='{2}' ", tSErrorCode.ErrorCode, tSErrorCode.ErrorCodeGroup, tSErrorCode.TSId);
            string deleteSQLForTSErrorCause2Location = String.Format(@" delete from tbltserrorcause2loc where ECODE='{0}'  and ECGCODE='{1}' and tsid ='{2}' ", tSErrorCode.ErrorCode, tSErrorCode.ErrorCodeGroup, tSErrorCode.TSId);
            string deleteSQLForTSErrorCause = String.Format(@" delete from tbltserrorcause where ECODE='{0}'  and ECGCODE='{1}' and tsid ='{2}' ", tSErrorCode.ErrorCode, tSErrorCode.ErrorCodeGroup, tSErrorCode.TSId);
            string deleteSQLForTSErrorCode2Location = String.Format(@" delete from tbltserrorcode2loc where ECODE='{0}'  and ECGCODE='{1}' and tsid ='{2}' ", tSErrorCode.ErrorCode, tSErrorCode.ErrorCodeGroup, tSErrorCode.TSId);
            try
            {
                currentDataProvider.CustomExecute(new SQLCondition(deletSQLForTSERRORCAUSE2EPART));
                currentDataProvider.CustomExecute(new SQLCondition(deleteSQLForTSErrorCause2Location));
                currentDataProvider.CustomExecute(new SQLCondition(deleteSQLForTSErrorCause));
                currentDataProvider.CustomExecute(new SQLCondition(deleteSQLForTSErrorCode2Location));
                currentDataProvider.Delete(tSErrorCode);
            }
            catch (Exception e)
            {
                ExceptionManager.Raise(this.GetType(), "$System_Error", e);
            }
        }

        //		public void DeleteTSErrorCode(TSErrorCode[] tSErrorCode)
        //		{
        //			this.DataProvider.Delete( tSErrorCode, 
        // 								new ICheck[]{ new DeleteAssociateCheck( tSErrorCode,
        //														this.DataProvider, 
        //														new Type[]{
        //																typeof(TSErrorCause),
        //																typeof(TSErrorCode2Location)	})} );
        //		}

        public object GetTSErrorCode(string errorCodeGroup, string errorCode, string tSId)
        {
            return this.DataProvider.CustomSearch(typeof(TSErrorCode), new object[] { errorCodeGroup, errorCode, tSId });
        }

        /// <summary>
        /// ** ��������:	��ѯTSErrorCode��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCodeGroup">ErrorCodeGroup��ģ����ѯ</param>
        /// <param name="errorCode">ErrorCode��ģ����ѯ</param>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <returns> TSErrorCode���ܼ�¼��</returns>
        public int QueryTSErrorCodeCount(string errorCodeGroup, string errorCode, string tSId)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLTSERRORCODE where 1=1 and ECGCODE like '{0}%'  and ECODE like '{1}%'  and TSID like '{2}%' ", errorCodeGroup, errorCode, tSId)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯTSErrorCode
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCodeGroup">ErrorCodeGroup��ģ����ѯ</param>
        /// <param name="errorCode">ErrorCode��ģ����ѯ</param>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> TSErrorCode����</returns>
        public object[] QueryTSErrorCode(string errorCodeGroup, string errorCode, string tSId, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCode), new PagerCondition(string.Format("select {0} from TBLTSERRORCODE where 1=1 and ECGCODE like '{1}%'  and ECODE like '{2}%'  and TSID like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCode)), errorCodeGroup, errorCode, tSId), "ECGCODE,ECODE,TSID", inclusive, exclusive));
        }

        public object[] ExtraQueryTSErrorCode(string tsID)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCode), new SQLParamCondition(String.Format("select {0} from TBLTSERRORCODE where tsid = $tsid", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCode))), new SQLParameter[] { new SQLParameter("tsid", typeof(string), tsID) }));
        }

        /// <summary>
        /// ** ��������:	������е�TSErrorCode
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>TSErrorCode���ܼ�¼��</returns>
        public object[] GetAllTSErrorCode()
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCode), new SQLCondition(string.Format("select {0} from TBLTSERRORCODE order by ECGCODE,ECODE,TSID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCode)))));
        }


        #endregion

        #region ErrorCauseGroup

        public ErrorCauseGroup CreateNewErrorCauseGroup()
        {
            return new ErrorCauseGroup();
        }

        public void AddErrorCauseGroup(ErrorCauseGroup errorCauseGroup)
        {
            this.DataProvider.Insert(errorCauseGroup);
        }

        public void UpdateErrorCauseGroup(ErrorCauseGroup errorCauseGroup)
        {
            this.DataProvider.Update(errorCauseGroup);
        }

        public void DeleteErrorCauseGroup(ErrorCauseGroup errorCauseGroup)
        {
            this.DataProvider.Delete(errorCauseGroup);
        }

        public object GetErrorCauseGroup(string ecsgCode)
        {
            return this.DataProvider.CustomSearch(typeof(ErrorCauseGroup), new object[] { ecsgCode });
        }

        #endregion

        #region TSErrorCode2Location
        /// <summary>
        /// 
        /// </summary>
        public TSErrorCode2Location CreateNewTSErrorCode2Location()
        {
            return new TSErrorCode2Location();
        }

        public void AddTSErrorCode2Location(TSErrorCode2Location tSErrorCode2Location)
        {
            this.DataProvider.Insert(tSErrorCode2Location);
        }

        public void AddTSErrorCode2Location_New(TSErrorCode2Location tSErrorCode2Location)
        {
            this._helper.AddDomainObject(tSErrorCode2Location);
        }

        public void UpdateTSErrorCode2Location(TSErrorCode2Location tSErrorCode2Location)
        {
            this.DataProvider.Update(tSErrorCode2Location);
        }

        public void DeleteTSErrorCode2Location(TSErrorCode2Location tSErrorCode2Location)
        {
            this.DataProvider.Delete(tSErrorCode2Location);
        }

        //		public void DeleteTSErrorCode2Location(TSErrorCode2Location[] tSErrorCode2Location)
        //		{
        //			this.DataProvider.Delete( tSErrorCode2Location, 
        // 								new ICheck[]{ new DeleteAssociateCheck( tSErrorCode2Location,
        //														this.DataProvider, 
        //														new Type[]{
        //																typeof(TSErrorCause2Location)	})} );
        //		}

        public object GetTSErrorCode2Location(string errorCode, string errorLocation, string errorCodeGroup, string aB, string tSId)
        {
            return this.DataProvider.CustomSearch(typeof(TSErrorCode2Location), new object[] { errorCode, errorLocation, errorCodeGroup, aB, tSId });
        }

        /// <summary>
        /// ** ��������:	��ѯTSErrorCode2Location��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCode">ErrorCode��ģ����ѯ</param>
        /// <param name="errorLocation">ErrorLocation��ģ����ѯ</param>
        /// <param name="errorCodeGroup">ErrorCodeGroup��ģ����ѯ</param>
        /// <param name="aB">AB��ģ����ѯ</param>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <returns> TSErrorCode2Location���ܼ�¼��</returns>
        public int QueryTSErrorCode2LocationCount(string errorCode, string errorLocation, string errorCodeGroup, string aB, string tSId)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLTSERRORCODE2LOC where 1=1 and ECODE like '{0}%'  and ELOC like '{1}%'  and ECGCODE like '{2}%'  and AB like '{3}%'  and TSID like '{4}%' ", errorCode, errorLocation, errorCodeGroup, aB, tSId)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯTSErrorCode2Location
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="errorCode">ErrorCode��ģ����ѯ</param>
        /// <param name="errorLocation">ErrorLocation��ģ����ѯ</param>
        /// <param name="errorCodeGroup">ErrorCodeGroup��ģ����ѯ</param>
        /// <param name="aB">AB��ģ����ѯ</param>
        /// <param name="tSId">TSId��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> TSErrorCode2Location����</returns>
        public object[] QueryTSErrorCode2Location(string errorCode, string errorLocation, string errorCodeGroup, string aB, string tSId, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCode2Location), new PagerCondition(string.Format("select {0} from TBLTSERRORCODE2LOC where 1=1 and ECODE like '{1}%'  and ELOC like '{2}%'  and ECGCODE like '{3}%'  and AB like '{4}%'  and TSID like '{5}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCode2Location)), errorCode, errorLocation, errorCodeGroup, aB, tSId), "ECODE,ELOC,ECGCODE,AB,TSID", inclusive, exclusive));
        }

        public object[] ExtraQueryTSErrorCode2Location(string tsID)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCode2Location), new SQLParamCondition(String.Format("select {0} from TBLTSERRORCODE2LOC where tsid = $tsid", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCode2Location))), new SQLParameter[] { new SQLParameter("tsid", typeof(string), tsID) }));
        }

        /// <summary>
        /// ** ��������:	������е�TSErrorCode2Location
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-05-31 15:34:59
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>TSErrorCode2Location���ܼ�¼��</returns>
        public object[] GetAllTSErrorCode2Location()
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCode2Location), new SQLCondition(string.Format("select {0} from TBLTSERRORCODE2LOC order by ECODE,ELOC,ECGCODE,AB,TSID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCode2Location)))));
        }


        #endregion

        #region TSErrorCause2Location
        public void AddTSErrorCause2Location(TSErrorCause2Location tSErrorCause2Location)
        {
            //this.DataProvider.Insert(tSErrorCause2Location);
            this._helper.AddDomainObject(tSErrorCause2Location);
        }

        public object[] ExtraQueryTSErrorCause2Location(string tsID)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause2Location), new SQLParamCondition(String.Format("select {0} from TBLTSERRORCAUSE2LOC where tsid = $tsid", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause2Location))), new SQLParameter[] { new SQLParameter("tsid", typeof(string), tsID) }));
        }
        #endregion

        #region TSErrorCause2ErrorPart
        public void AddTSErrorCause2ErrorPart(TSErrorCause2ErrorPart tSErrorCause2ErrorPart)
        {
            //this.DataProvider.Insert(tSErrorCause2Location);
            this._helper.AddDomainObject(tSErrorCause2ErrorPart);
        }

        public object[] ExtraQueryTSErrorCause2ErrorPart(string tsID)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause2ErrorPart), new SQLParamCondition(String.Format("select {0} from TBLTSERRORCAUSE2EPART where tsid = $tsid", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause2ErrorPart))), new SQLParameter[] { new SQLParameter("tsid", typeof(string), tsID) }));
        }
        #endregion

        #region getTSModelInformationFromTS
        public object[] GetExistedErrorCode2ErrorCodesFromTSID(string tsID)
        {
            return this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TSModel.ErrorCodeGroup2ErrorCode), new SQLParamCondition(@"select distinct TBLECG2EC.Ecgcode,TBLECG2EC.Ecode,TBLECG2EC.Muser,TBLECG2EC.Mdate,TBLECG2EC.Mtime from TBLECG2EC,tbltserrorcode where TBLECG2EC.ECGCODE = tbltserrorcode.ecgcode and TBLECG2EC.Ecode = tbltserrorcode.ecode "
              + " and   tsid = $tsid ", new SQLParameter[] { new SQLParameter("tsid", typeof(string), tsID) }));
        }
        #endregion

        #region For ά����Ϣ
        /// <summary>
        /// ** ��������:	��TS���ά����Ϣ
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="ts"></param>
        /// <returns>TSErrorInfo����:TSErrorCode�б�,ÿ��TSErrorCode��TSErrorCause����</returns>
        public object[] GetTSInfoByTS(BenQGuru.eMES.Domain.TS.TS ts)
        {
            if (ts != null)
            {
                object[] objs = this.DataProvider.CustomQuery(typeof(TSErrorInfo),
                    new SQLParamCondition(string.Format("select {0} from TBLTSERRORCODE where tsid=$tsid order by ECGCODE, ECODE",
                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorInfo))),
                    new SQLParameter[] { new SQLParameter("tsid", typeof(string), ts.TSId) }));


                if (objs != null)
                {
                    foreach (TSErrorInfo info in objs)
                    {
                        info.ErrorCauseList = this.DataProvider.CustomQuery(typeof(TSErrorCause),
                            new SQLParamCondition(string.Format("select {0} from TBLTSERRORCAUSE where tsid=$tsid and ECGCODE=$ECGCODE and ECODE=$ECODE",
                            DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause))),
                            new SQLParameter[]{ new SQLParameter("tsid",typeof(string), info.TSId ),
												  new SQLParameter("ECGCODE",typeof(string), info.ErrorCodeGroup ),
												  new SQLParameter("ECODE",typeof(string), info.ErrorCode )}));
                    }
                }

                return objs;
            }

            return null;
        }

        #region TSErrorCode
        /// <summary>
        /// ** ��������:	��TSID���TSErrorCode
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-11
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsid"></param>
        /// <returns></returns>
        public object[] GetTSErrorCode(string tsid)
        {
            return this.DataProvider.CustomQuery(
                typeof(TSErrorCode),
                new SQLParamCondition(string.Format("select {0} from tbltserrorcode where tsid = $tsid order by ECGCODE, ECODE",
                                                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCode))),
                                        new SQLParameter[] { new SQLParameter("tsid", typeof(string), tsid) }));

        }

        /// <summary>
        /// ** ��������:	��TS�����TSErrorCode
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-11
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="tsErrorCodes">��ErrorCodeGroup,ErrorCode��UserCode</param>
        public void AddTSErrorCode(BenQGuru.eMES.Domain.TS.TS ts, object[] tsErrorCodes)
        {
            if (tsErrorCodes == null)
            {
                return;
            }

            foreach (TSErrorCode tsErrorCode in tsErrorCodes)
            {
                DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
                tsErrorCode.TSId = ts.TSId;
                tsErrorCode.RunningCard = ts.RunningCard;
                tsErrorCode.RunningCardSequence = ts.RunningCardSequence;
                tsErrorCode.ModelCode = ts.ModelCode;
                tsErrorCode.ItemCode = ts.ItemCode;
                tsErrorCode.MOCode = ts.MOCode;
                tsErrorCode.MOSeq = ts.MOSeq;       // Added by Icyer 2007/07/03
                tsErrorCode.MaintainDate = dbDateTime.DBDate;
                tsErrorCode.MaintainTime = dbDateTime.DBTime;

                this.DataProvider.Insert(tsErrorCode);
            }
        }

        /// <summary>
        /// ** ��������:	ɾ��TS�µ�TSErrorCode,���������е�TSErrorCode2ErrorLocation, 
        ///						TSErrorCause,TSErrorCause2ErrorLocation,TSErrorCause2ErrorPart
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="tsErrorCodes">��ErrorCodeGroup,ErrorCode</param>
        public void DeleteTSErrorCode(BenQGuru.eMES.Domain.TS.TS ts, object[] tsErrorCodes)
        {
            if (tsErrorCodes == null)
            {
                return;
            }

            foreach (TSErrorCode tsErrorCode in tsErrorCodes)
            {
                this.DeleteTSErrorCode(ts.TSId, tsErrorCode.ErrorCodeGroup, tsErrorCode.ErrorCode);
            }
        }

        /// <summary>
        /// ** ��������:	��TSID,ErrorCodeGroup,ErrorCodeɾ��TSErrorCode,TSErrorCode2ErrorLocation, 
        ///						TSErrorCause,TSErrorCause2ErrorLocation,TSErrorCause2ErrorPart
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsid"></param>
        /// <param name="errorCodeGroup"></param>
        /// <param name="errorCode"></param>
        public void DeleteTSErrorCode(string tsid, string errorCodeGroup, string errorCode)
        {
            string condition;
            SQLParameter[] parameters;
            if (errorCodeGroup == string.Empty)
            {
                condition = "where TSID=$TSID and ECODE=$ECODE";
                parameters = new SQLParameter[]{
															new SQLParameter("TSID", typeof(string), tsid),
															new SQLParameter("ECODE", typeof(string), errorCode) };
            }
            else
            {
                condition = "where TSID=$TSID and ECGCODE=$ECGCODE and ECODE=$ECODE";
                parameters = new SQLParameter[]{
															new SQLParameter("TSID", typeof(string), tsid),
															new SQLParameter("ECGCODE", typeof(string), errorCodeGroup),
															new SQLParameter("ECODE", typeof(string), errorCode) };
            }
            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCODE {0}", condition), parameters));
            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCODE2LOC {0}", condition), parameters));
            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE {0}", condition), parameters));
            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE2LOC {0}", condition), parameters));
            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE2EPART {0}", condition), parameters));
            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE2COM {0}", condition), parameters));
        }

        /// <summary>
        /// ** ��������:	TSErrorCode�Ƿ���ά��ά����Ϣ
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsErrorCode"></param>
        /// <returns></returns>
        public bool HasInfoBelowTSErrorCode(TSErrorCode tsErrorCode)
        {
            string condition = "where TSID=$TSID and ECGCODE=$ECGCODE and ECODE=$ECODE";
            SQLParameter[] parameters = new SQLParameter[]{
															  new SQLParameter("TSID", typeof(string), tsErrorCode.TSId),
															  new SQLParameter("ECGCODE", typeof(string), tsErrorCode.ErrorCodeGroup),
															  new SQLParameter("ECODE", typeof(string), tsErrorCode.ErrorCode) };

            if (this.DataProvider.GetCount(new SQLParamCondition(string.Format("select count(*) from TBLTSERRORCODE2LOC {0}", condition), parameters)) > 0)
            {
                return true;
            }
            if (this.DataProvider.GetCount(new SQLParamCondition(string.Format("select count(*) from TBLTSERRORCAUSE {0}", condition), parameters)) > 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region TSErrorCause
        /// <summary>
        /// ** ��������:	ɾ��TSErrorCause������ά����TSErrorCause2ErrorLocation,TSErrorCause2ErrorPart
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tSErrorCause"></param>
        public void DeleteTSErrorCause(TSErrorCause tSErrorCause)
        {
            string condition = "where TSID=$TSID and ECGCODE=$ECGCODE and ECODE=$ECODE and ECSCODE=$ECSCODE";
            SQLParameter[] parameters = new SQLParameter[]{
															new SQLParameter("TSID", typeof(string),	tSErrorCause.TSId),
															new SQLParameter("ECGCODE", typeof(string), tSErrorCause.ErrorCodeGroup),
															new SQLParameter("ECODE", typeof(string),	tSErrorCause.ErrorCode),
															new SQLParameter("ECSCODE", typeof(string), tSErrorCause.ErrorCauseCode) };

            this.DataProvider.BeginTransaction();

            try
            {
                this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE {0}", condition), parameters));
                this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE2LOC {0}", condition), parameters));
                this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE2EPART {0}", condition), parameters));
                this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE2COM {0}", condition), parameters));

                this.DataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                throw e;
            }
        }

        public void DeleteTSErrorCauseWithNoTrans(TSErrorCause tSErrorCause)
        {
            string condition = "where TSID=$TSID and ECGCODE=$ECGCODE and ECODE=$ECODE and ECSCODE=$ECSCODE";
            SQLParameter[] parameters = new SQLParameter[]{
															new SQLParameter("TSID", typeof(string),	tSErrorCause.TSId),
															new SQLParameter("ECGCODE", typeof(string), tSErrorCause.ErrorCodeGroup),
															new SQLParameter("ECODE", typeof(string),	tSErrorCause.ErrorCode),
															new SQLParameter("ECSCODE", typeof(string), tSErrorCause.ErrorCauseCode) };

            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE {0}", condition), parameters));
            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE2LOC {0}", condition), parameters));
            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE2EPART {0}", condition), parameters));
            this.DataProvider.CustomExecute(new SQLParamCondition(string.Format("delete from TBLTSERRORCAUSE2COM {0}", condition), parameters));
        }
        #endregion

        #region TSErrorCode2Location
        /// <summary>
        /// ** ��������:	��ò�������Ĳ���λ��
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsid"></param>
        /// <param name="errorCodeGroup"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public object[] GetTSErrorCode2Location(string tsid, string errorCodeGroup, string errorCode)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCode2Location),
                new SQLParamCondition(string.Format("select {0} from TBLTSERRORCODE2LOC where TSID=$TSID and ECGCODE=$ECGCODE and ECODE=$ECODE order by ELOC",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCode2Location))),
                new SQLParameter[] {
									   new SQLParameter("TSID", typeof(string), tsid),
									   new SQLParameter("ECGCODE", typeof(string), errorCodeGroup),
									   new SQLParameter("ECODE", typeof(string), errorCode)}));
        }

        /// <summary>
        /// ** ��������:	��ò�������δѡ�Ĳ���λ��,����λ����Դ�ڱ�׼BOM�����õ�Item�Ĳ���λ��
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsid"></param>
        /// <param name="errorCodeGroup"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public object[] GetUnselectedTSErrorCode2Location(string tsid, string errorCodeGroup, string errorCode)
        {
            return this.DataProvider.CustomQuery(typeof(ErrorLocation),
                new SQLParamCondition(@"select distinct SBITEMLOCATION as LOCCODE from TBLSBOM where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + @" and ITEMCODE=(select itemcode from tblts where TSID=$TSID1) 
and SBITEMLOCATION not in(select ELOC from TBLTSERRORCODE2LOC where TSID=$TSID2 and ECGCODE=$ECGCODE and ECODE=$ECODE)",
                new SQLParameter[] {
									   new SQLParameter("TSID1", typeof(string), tsid),
									   new SQLParameter("TSID2", typeof(string), tsid),
									   new SQLParameter("ECGCODE", typeof(string), errorCodeGroup),
									   new SQLParameter("ECODE", typeof(string), errorCode)}));
        }

        /// <summary>
        /// ** ��������:	������������Ӳ���λ��
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsErrorCode"></param>
        /// <param name="locationCodes"></param>
        public void AddTSErrorCode2Location(TSErrorCode tsErrorCode, string[] locationCodes)
        {
            AddTSErrorCode2Location(tsErrorCode, locationCodes, false);
        }

        public void AddTSErrorCode2Location(TSErrorCode tsErrorCode, string[] locationCodes, bool parseErrorPart)
        {
            if (locationCodes == null || locationCodes.Length == 0)
            {
                return;
            }

            TSErrorCode2Location tsErrorCode2Location = this.CreateNewTSErrorCode2Location();

            tsErrorCode2Location.TSId = tsErrorCode.TSId;
            tsErrorCode2Location.RunningCard = tsErrorCode.RunningCard;
            tsErrorCode2Location.RunningCardSequence = tsErrorCode.RunningCardSequence;
            tsErrorCode2Location.ModelCode = tsErrorCode.ModelCode;
            tsErrorCode2Location.ItemCode = tsErrorCode.ItemCode;
            tsErrorCode2Location.MOCode = tsErrorCode.MOCode;
            tsErrorCode2Location.MOSeq = tsErrorCode.MOSeq;     // Added by Icyer 2007/07/03

            tsErrorCode2Location.ErrorCodeGroup = tsErrorCode.ErrorCodeGroup;
            tsErrorCode2Location.ErrorCode = tsErrorCode.ErrorCode;
            tsErrorCode2Location.AB = ItemLocationSide.ItemLocationSide_AB;

            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            tsErrorCode2Location.MaintainUser = tsErrorCode.MaintainUser;
            tsErrorCode2Location.MaintainDate = dbDateTime.DBDate;
            tsErrorCode2Location.MaintainTime = dbDateTime.DBTime;

            foreach (string locationCode in locationCodes)
            {
                string errorLocation = locationCode;
                int pos = locationCode.IndexOf(":");
                if (parseErrorPart && pos >= 0)
                {
                    errorLocation = locationCode.Substring(0, pos);
                }

                tsErrorCode2Location.ErrorLocation = errorLocation;
                tsErrorCode2Location.SubErrorLocation = errorLocation;

                this.DataProvider.Insert(tsErrorCode2Location);
            }
        }

        /// <summary>
        /// ** ��������:	ɾ����������Ĳ���λ��
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsErrorCode"></param>
        /// <param name="locationCodes"></param>
        public void DeleteTSErrorCode2Location(TSErrorCode tsErrorCode, string[] locationCodes)
        {
            if (locationCodes == null || locationCodes.Length == 0)
            {
                return;
            }

            this.DataProvider.CustomExecute(
                new SQLCondition(string.Format("delete from TBLTSERRORCODE2LOC where TSID='{0}' and ECGCODE='{1}' and ECODE='{2}' and ELOC in ({3}) ",
                tsErrorCode.TSId, tsErrorCode.ErrorCodeGroup, tsErrorCode.ErrorCode, FormatHelper.ProcessQueryValues(locationCodes))));
        }
        #endregion

        #region TSErrorCause2Location
        /// <summary>
        /// ** ��������:	��ò���ԭ��Ĳ���λ��
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsid"></param>
        /// <param name="errorCodeGroup"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorCause"></param>
        /// <returns></returns>
        public object[] GetTSErrorCause2Location(string tsid, string errorCodeGroup, string errorCode, string errorCause)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause2Location),
                new SQLParamCondition(string.Format("select {0} from TBLTSERRORCAUSE2LOC where TSID=$TSID and ECGCODE=$ECGCODE and ECODE=$ECODE and ECSCODE=$ECSCODE order by ELOC",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause2Location))),
                new SQLParameter[] {
									   new SQLParameter("TSID", typeof(string), tsid),
									   new SQLParameter("ECGCODE", typeof(string), errorCodeGroup),
									   new SQLParameter("ECODE", typeof(string), errorCode),
									   new SQLParameter("ECSCODE", typeof(string), errorCause)}));
        }

        /// <summary>
        /// ** ��������:	��ò���ԭ��δѡ�Ĳ���λ��,����λ����Դ�ڱ�׼BOM�����õ�Item�Ĳ���λ��
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsid"></param>
        /// <param name="errorCodeGroup"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorCause"></param>
        /// <returns></returns>
        public object[] GetUnselectedTSErrorCause2Location(string tsid, string errorCodeGroup, string errorCode, string errorCause)
        {
            return this.DataProvider.CustomQuery(typeof(ErrorLocation),
                new SQLParamCondition(@"select distinct SBITEMLOCATION as LOCCODE from TBLSBOM where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + @" and ITEMCODE=(select itemcode from tblts where TSID=$TSID1) 
and SBITEMLOCATION not in(select ELOC from TBLTSERRORCAUSE2LOC where TSID=$TSID2 and ECGCODE=$ECGCODE and ECODE=$ECODE and ECSCODE=$ECSCODE)",
                new SQLParameter[] {
									   new SQLParameter("TSID1", typeof(string), tsid),
									   new SQLParameter("TSID2", typeof(string), tsid),
									   new SQLParameter("ECGCODE", typeof(string), errorCodeGroup),
									   new SQLParameter("ECODE", typeof(string), errorCode),
									   new SQLParameter("ECSCODE", typeof(string), errorCause)}));
        }

        public object GetTSErrorCause2Location(string errorCauseCode, string errorCauseGroupCode,
            string errorCode, string errorCodeGroup, string errorLocation, string ab, string tsID)
        {
            return this.DataProvider.CustomSearch(typeof(TSErrorCause2Location),
                new object[] { errorCauseCode, errorCauseGroupCode, errorCode, errorCodeGroup, errorLocation, ab, tsID });
        }

        /// <summary>
        /// ** ��������:	����ԭ������Ӳ���λ��
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsErrorCause"></param>
        /// <param name="locationCodes"></param>
        public void AddTSErrorCause2Location(TSErrorCause tsErrorCause, string[] locationCodes)
        {
            AddTSErrorCause2Location(tsErrorCause, locationCodes, false);
        }

        public void AddTSErrorCause2Location(TSErrorCause tsErrorCause, string[] locationCodes, bool parseErrorPart)
        {
            if (locationCodes == null || locationCodes.Length == 0)
            {
                return;
            }

            TSErrorCause2Location tsErrorCause2Location = new TSErrorCause2Location();

            tsErrorCause2Location.TSId = tsErrorCause.TSId;
            tsErrorCause2Location.RunningCard = tsErrorCause.RunningCard;
            tsErrorCause2Location.RunningCardSequence = tsErrorCause.RunningCardSequence;
            tsErrorCause2Location.ModelCode = tsErrorCause.ModelCode;
            tsErrorCause2Location.ItemCode = tsErrorCause.ItemCode;
            tsErrorCause2Location.MOCode = tsErrorCause.MOCode;
            tsErrorCause2Location.MOSeq = tsErrorCause.MOSeq;   // Added by Icyer 2007/07/03

            tsErrorCause2Location.ErrorCodeGroup = tsErrorCause.ErrorCodeGroup;
            tsErrorCause2Location.ErrorCode = tsErrorCause.ErrorCode;
            tsErrorCause2Location.ErrorCauseGroupCode = tsErrorCause.ErrorCauseGroupCode;
            tsErrorCause2Location.ErrorCauseCode = tsErrorCause.ErrorCauseCode;

            tsErrorCause2Location.RepairResourceCode = tsErrorCause.RepairResourceCode;
            tsErrorCause2Location.RepairOPCode = tsErrorCause.RepairOPCode;
            tsErrorCause2Location.AB = ItemLocationSide.ItemLocationSide_AB;

            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            tsErrorCause2Location.MaintainUser = tsErrorCause.MaintainUser;

            tsErrorCause2Location.MaintainDate = dbDateTime.DBDate;
            tsErrorCause2Location.MaintainTime = dbDateTime.DBTime;

            foreach (string locationCode in locationCodes)
            {
                string errorPart = string.Empty;
                string errorLocation = locationCode;

                int pos = locationCode.IndexOf(":");
                if (parseErrorPart && pos >= 0)
                {
                    errorPart = locationCode.Substring(pos + 1);
                    errorLocation = locationCode.Substring(0, pos);
                }

                tsErrorCause2Location.ErrorLocation = errorLocation;
                tsErrorCause2Location.SubErrorLocation = errorLocation;
                tsErrorCause2Location.ErrorPart = errorPart;

                if (GetTSErrorCause2Location(tsErrorCause2Location.ErrorCauseCode, tsErrorCause2Location.ErrorCauseGroupCode,
                    tsErrorCause2Location.ErrorCode, tsErrorCause2Location.ErrorCodeGroup, tsErrorCause2Location.ErrorLocation,
                    tsErrorCause2Location.AB, tsErrorCause2Location.TSId) == null)
                {
                    this.DataProvider.Insert(tsErrorCause2Location);
                }
                else
                {
                    this.DataProvider.Update(tsErrorCause2Location);
                }
            }
        }

        /// <summary>
        /// ** ��������:	�Ӳ���ԭ��ɾ������λ��
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsErrorCause"></param>
        /// <param name="locationCodes"></param>
        public void DeleteTSErrorCause2Location(TSErrorCause tsErrorCause, string[] locationCodes)
        {
            if (locationCodes == null || locationCodes.Length == 0)
            {
                return;
            }

            this.DataProvider.CustomExecute(
                new SQLCondition(string.Format("delete from TBLTSERRORCAUSE2LOC where TSID='{0}' and ECGCODE='{1}' and ECODE='{2}' and ECSCODE='{3}' and ELOC in ({4}) and ecsgcode='{5}'",
                tsErrorCause.TSId, tsErrorCause.ErrorCodeGroup, tsErrorCause.ErrorCode, tsErrorCause.ErrorCauseCode, FormatHelper.ProcessQueryValues(locationCodes), tsErrorCause.ErrorCauseGroupCode)));
        }
        #endregion

        #region TSErrorCause2ErrorPart
        /// <summary>
        /// ** ��������:	��ò���ԭ��Ĳ������
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsid"></param>
        /// <param name="errorCodeGroup"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorCause"></param>
        /// <returns></returns>
        public object[] GetTSErrorCause2ErrorPart(string tsid, string errorCodeGroup, string errorCode, string errorCause)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause2ErrorPart),
                new SQLParamCondition(string.Format("select {0} from TBLTSERRORCAUSE2EPART where TSID=$TSID and ECGCODE=$ECGCODE and ECODE=$ECODE and ECSCODE=$ECSCODE order by EPART",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause2ErrorPart))),
                new SQLParameter[] {
									   new SQLParameter("TSID", typeof(string), tsid),
									   new SQLParameter("ECGCODE", typeof(string), errorCodeGroup),
									   new SQLParameter("ECODE", typeof(string), errorCode),
									   new SQLParameter("ECSCODE", typeof(string), errorCause)}));
        }

        /// <summary>
        /// ** ��������:	��ò���ԭ��δѡ�Ĳ������,���������Դ��OPBOM��Item�µ��ӽ���
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsid"></param>
        /// <param name="errCause"></param>
        /// <param name="partCode">ģ����ѯ</param>
        /// <returns></returns>
        public object[] GetUnselectedTSErrorCause2ErrorPart(string tsid, string sBOMVersion)
        {
            /*
			return this.DataProvider.CustomQuery( typeof(SBOM),
                new SQLParamCondition(@"select distinct * from TBLSBOM where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + @" and ITEMCODE=(select itemcode from tblts where TSID=$TSID) order by SBITEMCODE",				
				new SQLParameter[] {
									   new SQLParameter("TSID", typeof(string), tsid)
									   } ));

            */
            string sql = "SELECT " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(SBOM)) + " FROM tblsbom WHERE 1=1 and sbomver='" + sBOMVersion + "' " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + @" and ITEMCODE=(select itemcode from tblts where TSID='" + tsid + "') order by SBITEMCODE";
            return this.DataProvider.CustomQuery(typeof(SBOM), new PagerCondition(sql, "SBITEMCODE", 0, int.MaxValue));
        }

        /// <summary>
        /// ** ��������:	����ԭ������Ӳ������
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsErrorCause"></param>
        /// <param name="partCodes"></param>
        public void AddTSErrorCause2ErrorPart(TSErrorCause tsErrorCause, string[] partCodes)
        {
            if (partCodes == null || partCodes.Length == 0)
            {
                return;
            }

            TSErrorCause2ErrorPart tsErrorCause2ErrorPart = new TSErrorCause2ErrorPart();

            tsErrorCause2ErrorPart.TSId = tsErrorCause.TSId;
            tsErrorCause2ErrorPart.RunningCard = tsErrorCause.RunningCard;
            tsErrorCause2ErrorPart.RunningCardSequence = tsErrorCause.RunningCardSequence;
            tsErrorCause2ErrorPart.ModelCode = tsErrorCause.ModelCode;
            tsErrorCause2ErrorPart.ItemCode = tsErrorCause.ItemCode;
            tsErrorCause2ErrorPart.MOCode = tsErrorCause.MOCode;
            tsErrorCause2ErrorPart.MOSeq = tsErrorCause.MOSeq;  // Added by Icyer 2007/07/03

            tsErrorCause2ErrorPart.ErrorCodeGroup = tsErrorCause.ErrorCodeGroup;
            tsErrorCause2ErrorPart.ErrorCode = tsErrorCause.ErrorCode;
            tsErrorCause2ErrorPart.ErrorCauseCode = tsErrorCause.ErrorCauseCode;
            tsErrorCause2ErrorPart.ErrorCauseGroupCode = tsErrorCause.ErrorCauseGroupCode;

            tsErrorCause2ErrorPart.RepairResourceCode = tsErrorCause.RepairResourceCode;
            tsErrorCause2ErrorPart.RepairOPCode = tsErrorCause.RepairOPCode;

            tsErrorCause2ErrorPart.MaintainUser = tsErrorCause.MaintainUser;

            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            tsErrorCause2ErrorPart.MaintainDate = dbDateTime.DBDate;
            tsErrorCause2ErrorPart.MaintainTime = dbDateTime.DBTime;

            foreach (string partCode in partCodes)
            {
                tsErrorCause2ErrorPart.ErrorPart = partCode;

                this.DataProvider.Insert(tsErrorCause2ErrorPart);
            }
        }

        /// <summary>
        /// ** ��������:	�Ӳ���ԭ����ɾ���������
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-10
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="tsErrorCause"></param>
        /// <param name="partCodes"></param>
        public void DeleteTSErrorCause2ErrorPart(TSErrorCause tsErrorCause, string[] partCodes)
        {
            if (partCodes == null || partCodes.Length == 0)
            {
                return;
            }

            this.DataProvider.CustomExecute(
                new SQLCondition(string.Format("delete from TBLTSERRORCAUSE2EPART where TSID='{0}' and ECGCODE='{1}' and ECODE='{2}' and ECSCODE='{3}' and EPART in ({4}) and ecsgcode='{5}'",
                tsErrorCause.TSId, tsErrorCause.ErrorCodeGroup, tsErrorCause.ErrorCode, tsErrorCause.ErrorCauseCode, FormatHelper.ProcessQueryValues(partCodes), tsErrorCause.ErrorCauseGroupCode)));
        }
        #endregion

        #endregion

        #region For �깤

        #region ONWIP
        public OnWIP CreateNewOnWIP()
        {
            return new OnWIP();
        }

        public void AddOnWIP(OnWIP onWIP)
        {

            this.DataProvider.Insert(onWIP);
        }

        public void UpdateOnWIP(OnWIP onWIP)
        {
            this.DataProvider.Update(onWIP);
        }

        public void DeleteOnWIP(OnWIP onWIP)
        {
            this.DataProvider.Delete(onWIP);
        }
        #endregion

        #region SimulationReport
        public SimulationReport CreateNewSimulationReport()
        {
            return new SimulationReport();
        }

        public void AddSimulationReport(SimulationReport simulationReport)
        {
            this.DataProvider.Insert(simulationReport);
        }

        public void UpdateSimulationReport(SimulationReport simulationReport)
        {
            this.DataProvider.Update(simulationReport);
        }

        public void DeleteSimulationReport(Simulation simulation)
        {
            this.DataProvider.CustomExecute(new SQLParamCondition("delete from  TBLSIMULATIONREPORT where RCARD=$RCARD and MOCODE=$MOCODE",
                new SQLParameter[] {
									   new SQLParameter("RCARD", typeof(string), simulation.RunningCard),
									   new SQLParameter("MOCODE", typeof(string), simulation.MOCode)
								   }));
        }

        public void DeleteSimulationReport(SimulationReport simulationReport)
        {
            this.DataProvider.Delete(simulationReport);
        }

        public object[] QuerySimulationReport(string moCode, string itemCode, string startsn, string endsn, int startDate, int endDate, int inclusive, int exclusive)
        {
            string itemCodition = string.Empty;
            if (itemCode != null && itemCode != string.Empty)
            {
                itemCodition = string.Format(" AND ITEMCODE LIKE '{0}%' ", itemCode);
            }
            string moCodition = string.Empty;
            if (moCode != null && moCode != string.Empty)
            {
                moCodition = string.Format(" AND MOCODE LIKE '{0}%' ", moCode);
            }
            string rcardCodition = string.Empty;
            if (startsn != null && startsn != string.Empty)
            {
                rcardCodition = FormatHelper.GetCodeRangeSql("rcard", startsn, endsn);
            }
            string dateCondition = FormatHelper.GetDateRangeSql("shiftday", startDate, endDate);

            string sql = string.Format(" SELECT tblsimulationreport.* FROM tblsimulationreport WHERE 1=1 {0} and status='" + ProductStatus.OffMo + "' {1}{2}{3} order by  rcard ", dateCondition, itemCodition, moCodition, rcardCodition);
            return this.DataProvider.CustomQuery(typeof(SimulationReport), new PagerCondition(sql, inclusive, exclusive));
        }

        public int QuerySimulationReportCount(string moCode, string itemCode, string startsn, string endsn, int startDate, int endDate)
        {
            string itemCodition = string.Empty;
            if (itemCode != null && itemCode != string.Empty)
            {
                itemCodition = string.Format(" AND ITEMCODE LIKE '{0}%' ", itemCode);
            }
            string moCodition = string.Empty;
            if (moCode != null && moCode != string.Empty)
            {
                moCodition = string.Format(" AND MOCODE LIKE '{0}%' ", moCode);
            }
            string rcardCodition = string.Empty;
            if (startsn != null && startsn != string.Empty)
            {
                rcardCodition = FormatHelper.GetCodeRangeSql("rcard", startsn, endsn);
            }
            string dateCondition = FormatHelper.GetDateRangeSql("shiftday", startDate, endDate);

            string sql = string.Format(" SELECT count(rcard) FROM tblsimulationreport WHERE 1=1 {0} and status='" + ProductStatus.OffMo + "' {1}{2}{3}  order by  rcard ", itemCodition, moCodition, rcardCodition, dateCondition);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        #endregion

        #region Simulation
        public Simulation CreateNewSimulation()
        {
            return new Simulation();
        }

        public void AddSimulation(Simulation simulation)
        {
            this.DataProvider.Insert(simulation);
        }

        public void UpdateSimulation(Simulation simulation)
        {
            this.DataProvider.Update(simulation);
        }

        public void DeleteSimulation(Simulation simulation)
        {
            this.DataProvider.Delete(simulation);
        }

        public object GetSimulation(string moCode, string runningCard, string runningCardSeq)
        {
            //return this.DataProvider.CustomSearch(typeof(Simulation), new object[]{ runningCard });
            object[] simulations = this.DataProvider.CustomQuery(typeof(Simulation),
                new SQLParamCondition(string.Format("select {0} from TBLSIMULATION where RCARD = $RCARD and MOCODE = $MOCODE and RCARDSEQ = $RCARDSEQ order by MDATE desc,MTIME desc",
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(Simulation))),
                new SQLParameter[]{ new SQLParameter("RCARD", typeof(string), runningCard.ToUpper())
								  ,new SQLParameter("MOCODE", typeof(string), moCode.ToUpper())
								  ,new SQLParameter("RCARDSEQ", typeof(string), runningCardSeq.ToUpper())}));

            if (simulations == null)
                return null;
            if (simulations.Length > 0)
                return simulations[0];
            else
                return null;

        }

        #endregion

        //Laws Lu,2005/08/19,����	�깤�߼�
        //Laws Lu,2005/08/26,����	���¹�����������
        private void doAction(string moCode, string runningCard, string runningCardSeq)
        {
            #region ��дSimulation

            Simulation sim = (Simulation)this.GetSimulation(moCode, runningCard, runningCardSeq);

            if (sim != null)
            {
                //Laws Lu,2005/10/26,����	������������
                //sim.ISTS = 0;
                sim.IsComplete = "1";
                sim.EAttribute1 = "SCRAP";//TSStatus.TSStatus_Scrap ��Mail����Ϊ׼
            }

            this.UpdateSimulation(sim);

            #endregion

            #region ��дSimulationReport

            SimulationReport simulationReport = new SimulationReport();
            simulationReport.RouteCode = sim.RouteCode;
            simulationReport.OPCode = sim.OPCode;
            simulationReport.CartonCode = sim.CartonCode;
            simulationReport.EAttribute1 = sim.EAttribute1;
            simulationReport.EAttribute2 = sim.EAttribute2;
            simulationReport.IDMergeRule = sim.IDMergeRule;
            simulationReport.IsComplete = sim.IsComplete;
            simulationReport.ItemCode = sim.ItemCode;
            simulationReport.LastAction = sim.LastAction;
            simulationReport.LOTNO = sim.LOTNO;
            simulationReport.MaintainDate = sim.MaintainDate;
            simulationReport.MaintainTime = sim.MaintainTime;
            simulationReport.MaintainUser = sim.MaintainUser;
            simulationReport.MOCode = sim.MOCode;
            simulationReport.ModelCode = sim.ModelCode;
            simulationReport.NGTimes = sim.NGTimes;
            simulationReport.PalletCode = sim.PalletCode;
            simulationReport.ResourceCode = sim.ResourceCode;
            simulationReport.RunningCard = sim.RunningCard;
            simulationReport.RunningCardSequence = sim.RunningCardSequence;
            simulationReport.Status = sim.ProductStatus;
            simulationReport.TranslateCard = sim.TranslateCard;
            simulationReport.TranslateCardSequence = sim.TranslateCardSequence;
            simulationReport.SourceCard = sim.SourceCard;
            simulationReport.SourceCardSequence = sim.SourceCardSequence;
            //simulationReport.ISTS = sim.ISTS;

            BaseModelFacade dataModel = new BaseModelFacade(this.DataProvider);
            Resource resource = (Resource)dataModel.GetResource(sim.ResourceCode);
            simulationReport.SegmentCode = resource.SegmentCode;

            ShiftModelFacade shiftModel = new ShiftModelFacade(this.DataProvider);
            TimePeriod period = (TimePeriod)shiftModel.GetTimePeriod(resource.ShiftTypeCode, simulationReport.MaintainTime);
            if (period == null)
            {
                throw new Exception("$OutOfPerid");
            }
            //							
            //			// Modified by Jane Shu		Date:2005-07-26
            //			if ( period.IsOverDate == FormatHelper.TRUE_STRING )
            //			{
            //				if ( period.TimePeriodBeginTime < period.TimePeriodEndTime )
            //				{
            //					simulationReport.ShiftDay =	FormatHelper.TODateInt(DateTime.Now.AddDays(-1)) ;
            //				}
            //				else if ( sim.MaintainTime < period.TimePeriodBeginTime)
            //				{
            //					simulationReport.ShiftDay =	FormatHelper.TODateInt(DateTime.Now.AddDays(-1)) ;
            //				}
            //				else
            //				{
            //					simulationReport.ShiftDay = FormatHelper.TODateInt(DateTime.Now) ;
            //				}
            //			}
            //			else
            //			{
            //				simulationReport.ShiftDay = FormatHelper.TODateInt(DateTime.Now) ;
            //			}
            simulationReport.ShiftTypeCode = resource.ShiftTypeCode;
            simulationReport.ShiftCode = period.ShiftCode;
            simulationReport.TimePeriodCode = period.TimePeriodCode;
            simulationReport.StepSequenceCode = resource.StepSequenceCode;
            simulationReport.MOSeq = sim.MOSeq;     // Added by Icyer 2007/07/03

            this.UpdateSimulationReport(simulationReport);

            #endregion

            #region ��дOnWIP
            //			OnWIP onwip	=	new OnWIP();
            //			onwip.Action	=	sim.LastAction;
            //			onwip.ActionResult	=	sim.ProductStatus;
            //			onwip.ItemCode		=	sim.ItemCode ;
            //			onwip.MaintainDate	=	FormatHelper.TODateInt(DateTime.Now) ;
            //			onwip.MaintainTime	=	FormatHelper.TOTimeInt(DateTime.Now) ;
            //			onwip.MaintainUser	=	sim.MaintainUser ;
            //			onwip.MOCode			=	sim.MOCode;
            //			onwip.ModelCode		=  sim.ModelCode;
            //			onwip.NGTimes			=  sim.NGTimes;
            //			onwip.OPCode			=  sim.OPCode;
            //			onwip.ResourceCode =  sim.ResourceCode;
            //			onwip.RouteCode		=  sim.RouteCode;
            //			onwip.RunningCard	=  sim.RunningCard;
            //			onwip.RunningCardSequence	= sim.RunningCardSequence ;
            //						
            ////			BaseModelFacade dataModel1 = new BaseModelFacade(this.DataProvider);
            ////			Resource resource1				= (Resource)dataModel1.GetResource(sim.ResourceCode);
            ////			onwip.SegmentCode				= resource1.SegmentCode ;
            //						
            ////			ShiftModelFacade shiftModel1	= new ShiftModelFacade(this.DataProvider);
            ////			TimePeriod  period1				= (TimePeriod)shiftModel1.GetTimePeriod(resource1.ShiftTypeCode,onwip.MaintainTime);		
            ////			if (period==null)
            ////			{
            ////				throw new Exception("$OutOfPerid");
            ////			}
            //							
            ////			if ( period1.IsOverDate == FormatHelper.TRUE_STRING )
            ////			{
            ////				if ( period1.TimePeriodBeginTime < period1.TimePeriodEndTime )
            ////				{
            ////					onwip.ShiftDay =	FormatHelper.TODateInt(DateTime.Now.AddDays(-1)) ;
            ////				}
            ////				else if ( sim.MaintainTime < period1.TimePeriodBeginTime)
            ////				{
            ////					onwip.ShiftDay =	FormatHelper.TODateInt(DateTime.Now.AddDays(-1)) ;
            ////				}
            ////				else
            ////				{
            ////					onwip.ShiftDay = FormatHelper.TODateInt(DateTime.Now) ;
            ////				}
            ////			}
            ////			else
            ////			{
            ////				onwip.ShiftDay = FormatHelper.TODateInt(DateTime.Now) ;
            ////			}
            ////			onwip.ShiftTypeCode	= resource1.ShiftTypeCode;
            ////			onwip.ShiftCode			= period1.ShiftCode;
            ////			onwip.TimePeriodCode	= period1.TimePeriodCode;
            //			
            //			onwip.SourceCard				  = sim.SourceCard;
            //			onwip.SourceCardSequence = sim.SourceCardSequence;
            ////			onwip.StepSequenceCode	  = resource1.StepSequenceCode;
            //			
            //			onwip.TranslateCard			= sim.TranslateCard;
            //			onwip.TranslateCardSequence = sim.TranslateCardSequence;
            //				
            //			this.UpdateOnWIP(onwip);
            #endregion

            //Laws Lu,2005/08/26,����	���¹����б��ϵ�����
            MOFacade moFAC = new MOFacade(_domainDataProvider);
            object objMO = moFAC.GetMO(sim.MOCode);

            if (objMO != null)
            {
                MO mo = (MO)objMO;

                mo.MOScrapQty = 1 * sim.IDMergeRule;

                moFAC.UpdateMOScrapQty(mo);
            }
            //End Laws Lu
        }

        #endregion

        #region For ���
        /// <summary>
        /// ** ��������:	��ò�Ʒ���к��ϵ�������Ʒ
        ///						�ؼ���: MItemCodeΪ�㲿�����Ϻ�,MCardΪ�㲿����SN,����Ϊ1
        ///						���ܿ���: MItemCodeΪ�Ϻ�,MCardΪ����
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-17
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="rcard"></param>
        /// <param name="rcardseq"></param>
        /// <returns></returns>
        public object[] GetItemsOfRunningCard(string rcard, decimal rcardseq)
        {

            //Laws Lu,2005/10/11,�޸�	��Ҫ֧�����ת�����Running Card
            int iCount = this.DataProvider.GetCount(new SQLCondition("select count(*) from TBLONWIPCARDTRANS where  idmergetype='"
                        + IDMergeType.IDMERGETYPE_IDMERGE + "' and RCARD = '" + rcard + "' and RCARDSEQ <= " + rcardseq.ToString()));


            object[] objs = null;

            if (iCount < 1)
            {
                objs = this.DataProvider.CustomQuery(typeof(ItemOfRunningCard),
                    new SQLParamCondition(string.Format("select mitemcode, mcard, 1 as qty, mcardtype, PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE from tblonwipitem " +
                    "where mcardtype='{0}'and rcard=$rcard1 " +
                    "union select mitemcode, mcard, sum(qty) as qty, mcardtype, PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE " +
                    "from tblonwipitem where mcardtype='{1}'and rcard=$rcard2 " +
                    "group by mcard, mcardtype, mitemcode, mcard,PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE order by mitemcode",
                    MCardType.MCardType_Keyparts,
                    MCardType.MCardType_INNO),
                    new SQLParameter[] { 
										   new SQLParameter("rcard1",	typeof(string),  rcard),
										   new SQLParameter("rcard2",	typeof(string),  rcard)}));
            }
            else if (iCount == 1)
            {
                objs = this.DataProvider.CustomQuery(typeof(ItemOfRunningCard),
                    new SQLCondition(string.Format("select mitemcode, mcard, 1 as qty, mcardtype, PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE from tblonwipitem " +
                    "where mcardtype='{0}' and rcard=(select * from (select tcard from TBLONWIPCARDTRANS where RCARD = '"
                    + rcard + "' and RCARDSEQ < "
                    + rcardseq.ToString()
                    + " and  idmergetype='"
                    + IDMergeType.IDMERGETYPE_IDMERGE + "') where rownum = 1) "
                    + "union select mitemcode, mcard, sum(qty) as qty, mcardtype, PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE "
                    + "from tblonwipitem where mcardtype='{1}'and rcard=(select * from (select tcard from TBLONWIPCARDTRANS where RCARD = '"
                    + rcard + "' and RCARDSEQ < "
                    + rcardseq.ToString()
                    + " and  idmergetype='"
                    + IDMergeType.IDMERGETYPE_IDMERGE + "') where rownum = 1)"
                    + "group by mcard, mcardtype, mitemcode, mcard,PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE order by mitemcode"
                    , MCardType.MCardType_Keyparts
                    , MCardType.MCardType_INNO)));
            }

            return objs;
        }

        /// <summary>
        /// ** ��������:	��ò�Ʒ���к��ϵ�������Ʒ
        ///						�ؼ���: MItemCodeΪ�㲿�����Ϻ�,MCardΪ�㲿����SN,����Ϊ1
        ///						���ܿ���: MItemCodeΪ�Ϻ�,MCardΪ����
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-17
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="rcard"></param>
        /// <param name="rcardseq"></param>
        /// <returns></returns>
        public object[] GetItemsOfRunningCard(string mocode, string rcard, decimal rcardseq)
        {

            //Laws Lu,2005/10/11,�޸�	��Ҫ֧�����ת�����Running Card
            int iCount = this.DataProvider.GetCount(new SQLCondition("select count(*) from TBLONWIPCARDTRANS where  idmergetype='"
                + IDMergeType.IDMERGETYPE_IDMERGE + "' and RCARD = '" + rcard + "' and RCARDSEQ <= " + rcardseq.ToString()
                + " and mocode = '" + mocode + "' "));


            object[] objs = null;

            if (iCount < 1)
            {
                objs = this.DataProvider.CustomQuery(typeof(ItemOfRunningCard),
                    new SQLParamCondition(string.Format("select mitemcode, mcard, 1 as qty, mcardtype, PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE from tblonwipitem " +
                    "where mcardtype='{0}'and rcard=$rcard1 " +
                    "union select mitemcode, mcard, sum(qty) as qty, mcardtype, PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE " +
                    "from tblonwipitem where mcardtype='{1}'and rcard=$rcard2 " +
                    "group by mcard, mcardtype, mitemcode, PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE order by mitemcode",
                    MCardType.MCardType_Keyparts,
                    MCardType.MCardType_INNO),
                    new SQLParameter[] { 
										   new SQLParameter("rcard1",	typeof(string),  rcard),
										   new SQLParameter("rcard2",	typeof(string),  rcard)}));
            }
            else if (iCount >= 1)
            {
                /* ���������к�ת�� */
                objs = this.DataProvider.CustomQuery(typeof(ItemOfRunningCard),
                    new SQLCondition(string.Format("select mitemcode, mcard, 1 as qty, mcardtype, PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE from tblonwipitem " +
                    "where mcardtype='{0}'and rcard=(select * from (select tcard from TBLONWIPCARDTRANS where RCARD = '"
                    + rcard + "' and RCARDSEQ = ( select max(RCARDSEQ) from TBLONWIPCARDTRANS where RCARD = '" + rcard + "' and mocode = '" + mocode + "' ) "
                    //+  rcardseq.ToString() 
                    + " and  idmergetype='"
                    + IDMergeType.IDMERGETYPE_IDMERGE + "') where rownum = 1) "
                    + "union select mitemcode, mcard, sum(qty) as qty, mcardtype, PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE "
                    + "from tblonwipitem where mcardtype='{1}'and rcard=(select * from (select tcard from TBLONWIPCARDTRANS where RCARD = '"
                    + rcard + "' and RCARDSEQ = ( select max(RCARDSEQ) from TBLONWIPCARDTRANS where RCARD = '" + rcard + "' and mocode = '" + mocode + "' ) "
                    //+  rcardseq.ToString()	
                    + " and  idmergetype='"
                    + IDMergeType.IDMERGETYPE_IDMERGE + "') where rownum = 1)"
                    + "group by mcard, mcardtype, mitemcode, mcard,PCBA,BIOS,VERSION,VENDORITEMCODE,VENDORCODE,DATECODE order by mitemcode"
                    , MCardType.MCardType_Keyparts
                    , MCardType.MCardType_INNO)));
            }

            return objs;
        }

        //Laws Lu,2005/09/02,����	��ȡ����б�
        public object[] GetSpliteItems(string rcard, decimal rcardseq, string mocode)
        {
            return this.DataProvider.CustomQuery(typeof(TSSplitItem),
                new SQLParamCondition("select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSSplitItem)) + " from TBLTSSPLITITEM " +
                "where rcard=$rcard and RCARDSEQ=$rcardseq and mocode=$mocode",
                new SQLParameter[] { 
										new SQLParameter("rcard",	typeof(string),  rcard),
										new SQLParameter("rcardseq",	typeof(decimal),  rcardseq),
										new SQLParameter("mocode",	typeof(string),  mocode)}));
        }

        // Added by Icyer 2006/11/07
        /// <summary>
        /// ��黻�µ�KeyPart�Ƿ�ȷʵ�������к���
        /// </summary>
        /// <returns></returns>
        public OnWIPItem GetItemLastLoaded(string mcard)
        {
            string strSql = "SELECT * FROM tblOnWIPItem WHERE MCard='" + mcard + "' ORDER BY MDate DESC,MTime DESC ";
            object[] objs = this.DataProvider.CustomQuery(typeof(OnWIPItem), new SQLCondition(strSql));
            if (objs == null || objs.Length == 0)
                return null;
            return (OnWIPItem)objs[0];
        }
        // Added end

        /// <summary>
        /// ** ��������:	��ⲻ��Ʒ
        ///					����Ʒ���ڣ����ޣ�״̬�Ĳſ��Խ��в��
        ///					����Ʒ��״̬�ɣ����ޣ�תΪ����⣢��
        ///					�㲿����ȥ���ϵ������󣬼������TBLTSSPLITITEM����
        ///					���µ�Ҫ��ά�޵��㲿������ά�ޱ�TBLTS����״̬Ϊ�����ޣ���
        ///					
        ///					
        ///					2005-10-12,�����޵�����Ĵ������Ҳ����
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-08-17
        /// ** �� ��:		Karron Qiu
        /// ** �� ��:		2005-10-12
        /// 
        /// </summary>
        /// <param name="tsid"></param>
        /// <param name="onwipItems"></param>
        /// <param name="userCode"></param>
        /// <param name="resourceCode"></param>
        /// <param name="ErrorCodes"></param>
        public void SplitItem(string tsid, object[] onwipItems, string userCode, string currentUser, string resourceCode, System.Collections.Hashtable ErrorCodes)
        {

            if (onwipItems == null || onwipItems.Length == 0)
            {
                return;
            }

            BenQGuru.eMES.Domain.TS.TS ts = (BenQGuru.eMES.Domain.TS.TS)this.GetTS(tsid);

            if (ts == null)
            {
                throw new Exception(string.Format("$CSError_Card_Not_In_TS"));
            }

            if (ts.TSStatus != TSStatus.TSStatus_Confirm)
            {
                throw new Exception(string.Format("$CSError_TSStatus_Should_be: ${0}", TSStatus.TSStatus_Confirm));
            }

            //			ts.TranslateCard		 = ts.RunningCard;
            //			ts.TranslateCardSequence = ts.RunningCardSequence;
            //			ts.CardType				 = CardType.CardType_Part;
            //			ts.TSStatus = TSStatus.TSStatus_Confirm;
            //			ts.ConfirmResourceCode	 = resourceCode;
            //			ts.ConfirmOPCode		 = OPType.TS;
            //			ts.ConfirmUser			 = userCode;
            //			ts.ConfirmDate			 = FormatHelper.TODateInt( DateTime.Now );
            //			ts.ConfirmTime			 = FormatHelper.TOTimeInt( DateTime.Now );
            //			ts.MaintainUser			 = userCode;
            //			ts.MaintainDate			 = FormatHelper.TODateInt( DateTime.Now );
            //			ts.MaintainTime			 = FormatHelper.TOTimeInt( DateTime.Now );
            //
            //			ts.ReplacedRunningCard = "";
            //			ts.TSTime = 0;
            //			ts.TSMEMO = "";
            //			ts.TSUser = "";
            //			ts.TSDate = 0;
            //			ts.TSTime = 0;
            //			ts.ReflowMOCode = "";
            //			ts.ReflowRouteCode = "";
            //			ts.ReflowOPCode = "";
            //			ts.ReflowResourceCode = "";
            //			ts.TransactionStatus = TransactionStatus.TransactionStatus_NO;
            //			ts.TestFileFullPath = "";

            TSSplitItem splitItem = new TSSplitItem();

            splitItem.TSId = ts.TSId;
            splitItem.RunningCard = ts.RunningCard;
            splitItem.RunningCardSequence = ts.RunningCardSequence;
            splitItem.ItemCode = ts.ItemCode;
            splitItem.MOCode = ts.MOCode;
            splitItem.ModelCode = ts.ModelCode;
            splitItem.RepairResourceCode = resourceCode;
            splitItem.RepairOPCode = OPType.TS;
            splitItem.MEMO = "";
            splitItem.LocationPoint = 0;
            splitItem.MaintainUser = userCode;
            splitItem.MOSeq = ts.MOSeq;

            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            splitItem.MaintainDate = dbDateTime.DBDate;
            splitItem.MaintainTime = dbDateTime.DBTime;

            splitItem.ItemSequence = this.GetUniqueTSSplitItemSequence(ts.TSId);
            MOModel.ModelFacade mf = new MOModel.ModelFacade(this.DataProvider);

            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
            this.DataProvider.BeginTransaction();

            try
            {
                ts.TSResourceCode = resourceCode;
                ts.TSUser = userCode;

                ts.TSDate = dbDateTime.DBDate;
                ts.TSTime = dbDateTime.DBTime;

                ts.TSStatus = Web.Helper.TSStatus.TSStatus_Split;

                //added by jessie lee, 2005/11/24
                ts.MaintainUser = currentUser;
                ts.MaintainDate = ts.TSDate;
                ts.MaintainTime = ts.TSTime;

                //TODO��Laws Lu,2005/11/09����Ҫ�Ż�
                this.UpdateTS(ts);

                foreach (ItemOfRunningCard item in onwipItems)
                {
                    //Laws Lu,2005/08/31,��������������ʵ������
                    if (item.Qty - item.ScraptQty >= 0)
                    {
                        splitItem.MCardType = item.MCardType;
                        splitItem.MCard = item.MCARD;
                        splitItem.MItemCode = item.MItemCode;
                        splitItem.OpenQty = item.Qty;
                        splitItem.ScrapQty = item.ScraptQty;
                        splitItem.Qty = item.Qty - item.ScraptQty;

                        splitItem.VendorCode = item.VendorCode;
                        splitItem.VendorItemCode = item.VendorItemCode;
                        splitItem.Reversion = item.Version;
                        splitItem.BIOS = item.BIOS;
                        splitItem.PCBA = item.PCBA;
                        splitItem.DateCode = item.DateCode;

                        this.DataProvider.Insert(splitItem);

                        // Added by Icyer 2006/11/07, ����KeyPart����״̬
                        if (item.MCardType == MCardType.MCardType_Keyparts && item.CheckMaterialStatus == true)
                        {
                            string strUpdateSql = string.Empty;
                            if (item.NeedTS == false)
                            {
                                strUpdateSql = "UPDATE tblSimulationReport SET IsLoadedPart='0',LoadedRCard='' WHERE RCard='" + item.MCARD + "'";
                            }
                            else
                            {
                                strUpdateSql = "UPDATE tblSimulationReport SET IsLoadedPart='0',LoadedRCard='',Status='NG' WHERE RCard='" + item.MCARD + "'";
                            }
                            this.DataProvider.CustomExecute(new SQLCondition(strUpdateSql));
                        }
                        // Added end

                        if (item.NeedTS && item.MCardType == MCardType.MCardType_Keyparts)
                        {
                            BenQGuru.eMES.Domain.TS.TS itemTs = new BenQGuru.eMES.Domain.TS.TS();
                            //Laws Lu,2005/09/01,�޸�	��������
                            itemTs.MOCode = ts.MOCode;
                            itemTs.MOSeq = ts.MOSeq;    // Added by Icyer 2007/07/03
                            itemTs.RunningCard = item.MCARD;
                            itemTs.RunningCardSequence = this.GetUniqueTSRunningCardSequence(item.MCARD);
                            //itemTs.TSId					 = ts.RunningCard + "-" + ts.RunningCardSequence;
                            itemTs.TSId = FormatHelper.GetUniqueID(ts.MOCode, itemTs.RunningCard, itemTs.RunningCardSequence.ToString());
                            //itemTs.EAttribute1			 = item.MItemCode;
                            itemTs.TranslateCard = ts.RunningCard;
                            itemTs.TranslateCardSequence = ts.RunningCardSequence;
                            itemTs.CardType = CardType.CardType_Part;
                            //֮ǰ����Ĳ�����״̬Ϊ�����ޡ������ڸ�Ϊ����ġ���Ҫ��ά�ޡ��Ĳ���״̬Ϊ�����ޡ���
                            //��������ά������ȷ�Ϻ�״̬�ʹӡ����ޡ�תΪ�����ޡ�
                            itemTs.TSStatus = TSStatus.TSStatus_New;//TSStatus.TSStatus_Confirm;
                            //����Ҫ��ά�ޡ��Ĳ���״̬Ϊ�����ޡ�,��û��ȷ��վ
                            //itemTs.ConfirmResourceCode	 = resourceCode;
                            /*1. ���ڲ����Ĳ������ϣ����ʼ״̬ӦΪ�����ޡ����Ҵ�ʱӦֻ����Դ��Ϣ��û�н�����Ϣ��ά����Ϣ��
                            2. ���ڲ����Ĳ������ϣ�״̬Ϊ�����ޡ�����ʱӦֻ����Դ��Ϣ�ͽ�����Ϣ��û��ά����Ϣ��
                            ע���й�ά��״̬����Ӧ����Ϣ�����������*/
                            //itemTs.ConfirmOPCode		 = "TS";
                            //itemTs.ConfirmUser			 = userCode;
                            //itemTs.ConfirmDate			 = FormatHelper.TODateInt( DateTime.Now );
                            //itemTs.ConfirmTime			 = FormatHelper.TOTimeInt( DateTime.Now );
                            itemTs.MaintainUser = userCode;

                            itemTs.MaintainDate = dbDateTime.DBDate;//itemTs.ConfirmDate;//modified by jessie lee, 2005/11/24
                            itemTs.MaintainTime = dbDateTime.DBTime;//itemTs.ConfirmTime;//modified by jessie lee, 2005/11/24
                            //itemTs.TSUser = userCode;
                            //itemTs.TSDate = itemTs.ConfirmDate;
                            //itemTs.TSTime = itemTs.ConfirmTime;
                            itemTs.FromInputType = TSFacade.TSSource_TS;
                            itemTs.FromUser = userCode;
                            itemTs.FromDate = itemTs.MaintainDate;
                            itemTs.FormTime = itemTs.MaintainTime;
                            //modified by jessie lee, 2005/11/24
                            //û��ȷ����Ϣ
                            itemTs.FromOPCode = "TS";//itemTs.ConfirmOPCode;
                            itemTs.FromResourceCode = resourceCode;//itemTs.ConfirmResourceCode;


                            #region ʱ�����Դ�Ĵ���
                            //Laws Lu,2005/09/16,�޸�	������~�ŵĳ���
                            itemTs.FromRouteCode = ts.FromRouteCode;//"~";
                            itemTs.FromSegmentCode = ts.FromSegmentCode;//"~";
                            itemTs.FromShiftCode = ts.FromShiftCode;//"~";
                            itemTs.FromShiftDay = ts.FromShiftDay;
                            itemTs.FromShiftTypeCode = ts.FromShiftTypeCode;//"~";
                            itemTs.FromStepSequenceCode = ts.FromStepSequenceCode;//"~";
                            itemTs.FromTimePeriodCode = ts.FromTimePeriodCode;//"~";						
                            #endregion
                            //Laws Lu,2005/09/01,ע�� �����¼��ԴTS�Ĺ�����
                            //                            itemTs.MOCode ="";
                            //Laws LU,2005/09/01,����	������Դ����
                            itemTs.FromSegmentCode = ts.FromSegmentCode;
                            itemTs.ItemCode = item.MItemCode;
                            Model model = mf.GetModelByItemCode(item.MItemCode);
                            if (model == null)
                            {
                                throw new Exception("$CS_Model_Lost $CS_Param_Keyparts="
                                    + item.MCARD + " $CS_Param_ItemCode=" + item.MItemCode);
                            }
                            itemTs.ModelCode = model.ModelCode;

                            itemTs.TransactionStatus = TransactionStatus.TransactionStatus_NO;
                            itemTs.TSDate = 0;
                            itemTs.TSTime = 0;

                            this.DataProvider.Insert(itemTs);

                            #region ���Ӵ������ karron qiu 2005-10-12

                            if (ErrorCodes.ContainsKey(itemTs.RunningCard))
                            {
                                object[] _ErrorCodes = (object[])ErrorCodes[itemTs.RunningCard];
                                for (int i = 0; i < _ErrorCodes.Length; i++)
                                {
                                    TSErrorCode tsErrorCode = new TSErrorCode();
                                    tsErrorCode.TSId = itemTs.TSId;
                                    tsErrorCode.RunningCard = itemTs.RunningCard;
                                    tsErrorCode.RunningCardSequence = itemTs.RunningCardSequence;
                                    tsErrorCode.ModelCode = itemTs.ModelCode;
                                    tsErrorCode.ItemCode = itemTs.ItemCode;
                                    tsErrorCode.MOCode = itemTs.MOCode;
                                    tsErrorCode.MaintainDate = itemTs.MaintainDate;
                                    tsErrorCode.MaintainTime = itemTs.MaintainTime;
                                    tsErrorCode.MaintainUser = itemTs.MaintainUser;
                                    tsErrorCode.ErrorCode = ((ErrorCodeGroup2ErrorCode)_ErrorCodes[i]).ErrorCode;
                                    tsErrorCode.ErrorCodeGroup = ((ErrorCodeGroup2ErrorCode)_ErrorCodes[i]).ErrorCodeGroup;

                                    this.AddTSErrorCode(tsErrorCode);
                                }
                            }

                            #endregion
                        }

                        splitItem.ItemSequence++;
                    }
                }



                //Laws Lu,2005/08/23,����	ӦIcyer��Ҫ�󣬿ⷿ����
                //Laws Lu,2005/10/20,����	ʹ�������ļ�����������ģ���Ƿ�ʹ��
                if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                {
                    BenQGuru.eMES.Material.WarehouseFacade wf = new BenQGuru.eMES.Material.WarehouseFacade(DataProvider);

                    wf.TSSplitItemWarehouseStock(ts.TSId);
                }
                //End Laws Lu


                if (ts.FromInputType == TSFacade.TSSource_OnWIP)
                {
                    //Laws Lu,2005/08/15,����	�깤�߼�
                    //Laws Lu,2005/08/26,����	���¹����б��ϵ�����
                    //���������⹤��
                    doAction(ts.MOCode, ts.RunningCard, ts.RunningCardSequence.ToString());
                    //End Laws Lu
                }

                this.DataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                this.DataProvider.RollbackTransaction();
                throw e;
            }
            finally
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }

        }

        public decimal GetUniqueTSSplitItemSequence(string tsid)
        {
            object[] objs = this.DataProvider.CustomQuery(typeof(TSSplitItem),
                new PagerParamCondition("select ITEMSEQ from TBLTSSPLITITEM where TSID=$TSID",
                "ITEMSEQ desc", 1, 1, new SQLParameter[] { new SQLParameter("TSID", typeof(string), tsid) }));

            if (objs == null || objs.Length < 1)
            {
                return 0;
            }

            return ((TSSplitItem)objs[0]).ItemSequence + 1;
        }

        public decimal GetUniqueTSRunningCardSequence(string rcard)
        {
            object[] objs = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS),
                new PagerParamCondition("select RCARDSEQ from TBLTS where RCARD=$RCARD",
                "RCARDSEQ desc", 1, 1, new SQLParameter[] { new SQLParameter("RCARD", typeof(string), rcard) }));

            if (objs == null || objs.Length < 1)
            {
                return 101;
            }

            return ((BenQGuru.eMES.Domain.TS.TS)objs[0]).RunningCardSequence + 101;
        }
        #endregion

        #region GetRouteCode
        //Nanjing 2005/08/10  Jessie
        /// <summary>
        /// GetRouteCode
        /// </summary>
        /// <param name="runningCard">RunningCard</param>
        /// <param name="moCode">MoCode</param>
        /// <returns>BenQGuru.eMES.Domain.DataCollect.Simulation</returns>
        public BenQGuru.eMES.Domain.DataCollect.Simulation GetSimulation(string runningCard, string moCode)
        {
            object[] simulations = this.DataProvider.CustomQuery
                (
                    typeof(BenQGuru.eMES.Domain.DataCollect.Simulation),
                    new SQLParamCondition(string.Format("select {0} from TBLSIMULATION where RCARD = $RCARD AND MOCODE= $MOCODE",
                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.DataCollect.Simulation))),
                    new SQLParameter[]{ 
										new SQLParameter("RCARD", typeof(string), runningCard.ToUpper()),
										new SQLParameter("MOCODE", typeof(string), moCode.ToUpper())
									  })
                );

            if (simulations == null)
                return null;
            if (simulations.Length > 0)
                return (BenQGuru.eMES.Domain.DataCollect.Simulation)simulations[0];
            else
                return null;
        }
        #endregion

        #region CheckForTSComplete
        //Nanjing 2005/08/10  Jessie  
        //������Ϊ�Ӻ���,����ֱ��� karron qiu 2005-9-26
        /// <summary>
        /// ����Ƿ�����ά����ɵ�����,ά����ɺͻ������
        /// </summary>
        /// <param name="runningCard">RunningCard</param>
        /// <returns>True:���� ; False:������</returns>
        public bool CheckForTSComplete(string runningCard)
        {
            if (!CheckErrorCodeCountAndErrorSolutionForTSComplete(runningCard))
                return false;

            if (CheckErrorPartAndErrorLocationForTSComplete(runningCard))
                return true;

            return false;
        }

        public bool CheckErrorCodeCountAndErrorSolutionForTSComplete(string runningCard)
        {
            int errorCodeCount = this.DataProvider.GetCount(new SQLCondition(String.Format(
                @"select count(*) from TBLTSERRORCODE where tsid = (select tsid
									from (SELECT tsid
											FROM tblts
											WHERE rcard = '{0}'
											order by MDATE * 1000000 + MTIME DESC)
									where rownum = 1)",
                runningCard.ToUpper())));

            if (errorCodeCount == 0)
            {
                return false;
            }

            //modified by jessie lee,2005/9/19
            //modified by jessie lee, 2005/11/24
            //ԭ��RunningCrad�����¹�������������ʱ��Sequence�����¹�һ����������ȡsequence��ʱ��Ҫȡ�������һ��,
            //������޸������ͬ
            int errorSolution = this.DataProvider.GetCount(new SQLCondition(string.Format(
                @"select count(*) from TBLTSERRORCAUSE where tsid = (select tsid
									from (SELECT tsid
											FROM tblts
											WHERE rcard = '{0}'
											order by MDATE * 1000000 + MTIME DESC)
									where rownum = 1)",
                runningCard.ToUpper())));

            //if( errorSolution == 0 || errorCodeCount != errorSolution )
            if (errorSolution == 0 || errorCodeCount > errorSolution) //Karron Qiu ,2006-6-21,�޸��ж�����
            {
                return false;
            }

            return true;
        }

        public bool CheckErrorPartAndErrorLocationForTSComplete(string runningCard)
        {
            int errorPart = this.DataProvider.GetCount(new SQLCondition(string.Format(
                @"select count(*) from TBLTSERRORCAUSE2EPART where tsid = (select tsid
									from (SELECT tsid
											FROM tblts
											WHERE rcard = '{0}'
											order by MDATE * 1000000 + MTIME DESC)
									where rownum = 1)",
                runningCard.ToUpper())));
            if (errorPart > 0)
            {
                return true;
            }

            int errorLocation = this.DataProvider.GetCount(new SQLCondition(String.Format(
                @"select count(*) from TBLTSERRORCAUSE2LOC where tsid = (select tsid
									from (SELECT tsid
											FROM tblts
											WHERE rcard = '{0}'
											order by MDATE * 1000000 + MTIME DESC)
									where rownum = 1)",
                runningCard.ToUpper())));

            if (errorLocation > 0)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region ����ά��
        public bool IsEnabledSmartTS(string errorCode)
        {
            string strSql = "select count(*) cnt from tbltssmartcfg where enabled='1' ";
            if (string.IsNullOrEmpty(errorCode) == false)
            {
                strSql += " and ecode='" + errorCode + "' ";
            }
            int iCount = 0;
            try
            {
                iCount = this.DataProvider.GetCount(new SQLCondition(strSql));
            }
            catch { }
            return (iCount > 0);
        }
        /// <summary>
        /// ���ݲ��������ѯ��ǰά�޼�¼
        /// </summary>
        public TSSmartErrorCause[] QueryErrorCodeSmartTS(string errorCode)
        {
            // ��ѯ����ά���趨
            string strSql = "select * from tbltssmartcfg where ecode='" + errorCode + "' and enabled='1' ";
            object[] objsSmartCfg = this.DataProvider.CustomQuery(typeof(TSSmartConfig), new SQLCondition(strSql));
            if (objsSmartCfg == null || objsSmartCfg.Length == 0)
                return null;
            TSSmartConfig smartCfg = (TSSmartConfig)objsSmartCfg[0];
            if (smartCfg == null)
                return null;
            if (smartCfg.ShowItemCount <= 0)
                return null;

            // ������ʼʱ��
            DBDateTime dbTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            DateTime dtFrom = FormatHelper.ToDateTime(dbTime.DBDate, 0);
            if (smartCfg.DateRangeType == "day")
            {
                dtFrom = dtFrom.AddDays(-1 * Convert.ToInt32(smartCfg.DateRange));
            }
            else if (smartCfg.DateRangeType == "month")
            {
                dtFrom = dtFrom.AddMonths(-1 * Convert.ToInt32(smartCfg.DateRange));
            }
            else if (smartCfg.DateRangeType == "year")
            {
                dtFrom = dtFrom.AddYears(-1 * Convert.ToInt32(smartCfg.DateRange));
            }
            int iFromDate = FormatHelper.TODateInt(dtFrom);

            // ��ѯ�������󡢽������
            int iShowItemCount = Convert.ToInt32(smartCfg.ShowItemCount);
            TSSmartErrorCause[] smartErrorCause = null;
            if (smartCfg.SortBy == "last_time")
            {
                strSql = "select ecgcode,ecode,ecsgcode,ecscode,solcode,dutycode,solmemo,substr(maxdate,1,8) mdate,substr(maxdate,9,6) mtime,cnt from (select ecgcode,ecode,ecsgcode,ecscode,solcode,dutycode,solmemo,max(mdate*1000000+mtime) maxdate,count(*) cnt from tbltserrorcause where ecode='" + errorCode + "' and mdate>=" + iFromDate.ToString() + " group by ecgcode,ecode,ecsgcode,ecscode,solcode,dutycode,solmemo order by maxdate desc) ";
                object[] objsTmp = this.DataProvider.CustomQuery(typeof(TSSmartErrorCause), new PagerCondition(strSql, 1, iShowItemCount));
                if (objsTmp != null && objsTmp.Length > 0)
                {
                    smartErrorCause = new TSSmartErrorCause[objsTmp.Length];
                    objsTmp.CopyTo(smartErrorCause, 0);
                }
            }
            else
            {
                strSql = "select ecgcode,ecode,ecsgcode,ecscode,solcode,dutycode,solmemo,substr(maxdate,1,8) mdate,substr(maxdate,9,6) mtime,cnt from (select ecgcode,ecode,ecsgcode,ecscode,solcode,dutycode,solmemo,max(mdate*1000000+mtime) maxdate,count(*) cnt from tbltserrorcause where ecode='" + errorCode + "' and mdate>=" + iFromDate.ToString() + " group by ecgcode,ecode,ecsgcode,ecscode,solcode,dutycode,solmemo order by cnt desc) ";
                object[] objsTmp = this.DataProvider.CustomQuery(typeof(TSSmartErrorCause), new PagerCondition(strSql, 1, iShowItemCount));
                if (objsTmp != null && objsTmp.Length > 0)
                {
                    smartErrorCause = new TSSmartErrorCause[objsTmp.Length];
                    objsTmp.CopyTo(smartErrorCause, 0);
                }
            }
            if (smartErrorCause == null || smartErrorCause.Length == 0)
                return null;
            // ��ѯ����λ�á��������
            for (int i = 0; i < smartErrorCause.Length; i++)
            {
                TSSmartErrorCause ecause = smartErrorCause[i];
                strSql = string.Format("select * from tbltserrorcause where ecgcode='{0}' and ecode='{1}' and ecsgcode='{2}' and ecscode='{3}' and mdate={4} and mtime={5} ",
                    ecause.ErrorCodeGroup, ecause.ErrorCode, ecause.ErrorCauseGroupCode, ecause.ErrorCauseCode, ecause.MaintainDate, ecause.MaintainTime);
                object[] objsTmp = this.DataProvider.CustomQuery(typeof(TSErrorCause), new SQLCondition(strSql));
                if (objsTmp.Length > 0)
                {
                    TSErrorCause tsecause = (TSErrorCause)objsTmp[0];
                    strSql = string.Format("select * from tbltserrorcause2loc where tsid='{0}' and ecgcode='{1}' and ecode='{2}' and ecscode='{3}' ",
                        tsecause.TSId, tsecause.ErrorCodeGroup, tsecause.ErrorCode, tsecause.ErrorCauseCode);
                    objsTmp = this.DataProvider.CustomQuery(typeof(TSErrorCause2Location), new SQLCondition(strSql));
                    if (objsTmp != null && objsTmp.Length > 0)
                    {
                        ecause.Locations = new TSErrorCause2Location[objsTmp.Length];
                        objsTmp.CopyTo(ecause.Locations, 0);
                    }

                    strSql = string.Format("select * from tbltserrorcause2epart where tsid='{0}' and ecgcode='{1}' and ecode='{2}' and ecscode='{3}' ",
                        tsecause.TSId, tsecause.ErrorCodeGroup, tsecause.ErrorCode, tsecause.ErrorCauseCode);
                    objsTmp = this.DataProvider.CustomQuery(typeof(TSErrorCause2ErrorPart), new SQLCondition(strSql));
                    if (objsTmp != null && objsTmp.Length > 0)
                    {
                        ecause.ErrorParts = new TSErrorCause2ErrorPart[objsTmp.Length];
                        objsTmp.CopyTo(ecause.ErrorParts, 0);
                    }
                }
            }
            return smartErrorCause;
        }
        #endregion

        #region TSErrorCause2Com
        /// <summary>
        /// 
        /// </summary>
        public TSErrorCause2Com CreateNewTSErrorCause2Com()
        {
            return new TSErrorCause2Com();
        }

        public void AddTSErrorCause2Com(TSErrorCause2Com tsErrorCause2Com)
        {
            //this.DataProvider.Insert(tsErrorCause2Com);
            this._helper.AddDomainObject(tsErrorCause2Com);
        }

        public void UpdateTSErrorCause2Com(TSErrorCause2Com tsErrorCause2Com)
        {
            this._helper.UpdateDomainObject(tsErrorCause2Com);
        }

        public void DeleteTSErrorCause2Com(string tSId, string errorCauseCode, string errorCodeGroupCode, string errorCode, string errorCauseGroupCode)
        {
            string sql = "delete TBLTSERRORCAUSE2COM where 1=1 ";
            if (!string.IsNullOrEmpty(tSId))
            {
                sql += " and tsid='"+tSId+"'";
            }
            if (!string.IsNullOrEmpty(errorCauseCode))
            {
                sql += " and ecscode='" + errorCauseCode + "'";
            }
            if (!string.IsNullOrEmpty(errorCodeGroupCode))
            {
                sql += " and ecgcode='" + errorCodeGroupCode + "'";
            }
            if (!string.IsNullOrEmpty(errorCode))
            {
                sql += " and ecode='" + errorCode + "'";
            }
            if (!string.IsNullOrEmpty(errorCauseGroupCode))
            {
                sql += " and ecsgcode='" + errorCauseGroupCode + "'";
            }
            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }

        public object[] GetTSErrorCause2Com(string tSId, string errorCauseCode, string errorCodeGroupCode, string errorCode, string errorCauseGroupCode)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause2Com),
                new SQLCondition(string.Format("select {0} from TBLTSERRORCAUSE2COM WHERE tsid='{1}' AND ecscode='{2}' AND ecgcode='{3}' AND ecode='{4}' AND ecsgcode='{5}'",
                                                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause2Com)), tSId, errorCauseCode, errorCodeGroupCode, errorCode, errorCauseGroupCode)));
        }

        public object[] ExtraQueryTSErrorCause2Com(string tsID)
        {
            return this.DataProvider.CustomQuery(typeof(TSErrorCause2Com), new SQLParamCondition(String.Format("select {0} from TBLTSERRORCAUSE2COM where tsid = $tsid", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TSErrorCause2Com))), new SQLParameter[] { new SQLParameter("tsid", typeof(string), tsID) }));
        }
        #endregion
    }
}

