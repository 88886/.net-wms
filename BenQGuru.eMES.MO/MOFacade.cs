#region System
using System;
using System.Text;
using System.Runtime.Remoting;
using System.Collections;
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Rework;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Domain.SMT;
#endregion


/// MOFacade ��ժҪ˵����
/// �ļ���:
/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
/// ������:Crystal Chu
/// ��������:2005/03/22
/// �޸���: Crystal chu
/// �޸�����: 2005/4/20
/// �� ��: �Թ����Ĳ�������
///        Crystal chu 2005/4/20 ����release,pending->open��ʱ����й����汾������
///        Crystal chu 2005/4/20  IsOPBOMUsed ���opBOM�Ƿ��б�ʹ�õ�ʱ�򹤵���״̬������open    
///        Crystal chu 2005/04/29 ���ӹ���BOM��OPBOM�ıȶ�                 
///          
/// /// �� ��:	
/// </summary>  
namespace BenQGuru.eMES.MOModel
{
    public class MOFacade : MarshalByRefObject
    {
        //private static readonly log4net.ILog _log = BenQGuru.eMES.Common.Log.GetLogger(typeof(MOFacade));
        private IDomainDataProvider _domainDataProvider = null;
        private FacadeHelper _helper = null;

        public const string ISMAINROUTE_TRUE = "1";

        public MOFacade(IDomainDataProvider domainDataProvider)
        {
            this._domainDataProvider = domainDataProvider;
            this._helper = new FacadeHelper(DataProvider);
        }

        //Laws Lu,max life time to unlimited
        public override object InitializeLifetimeService()
        {
            return null;
        }


        public MO CreateNewMO()
        {
            MO mo = new MO();
            mo.MOStatus = MOManufactureStatus.MOSTATUS_INITIAL;
            return mo;
        }
        //Laws Lu,2006/07/19 recover the default construction function
        public MOFacade()
        {
        }

        protected IDomainDataProvider DataProvider
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


        private string[] canModifyStatus = new string[] { MOManufactureStatus.MOSTATUS_INITIAL, MOManufactureStatus.MOSTATUS_PENDING };

        #region Begin for MO2SAP
        public void AddMO2SAP(MO2SAP mo2sap)
        {
            this.DataProvider.Insert(mo2sap);
        }

        public void UpdateMO2SAP(MO2SAP mo2sap)
        {
            this.DataProvider.Update(mo2sap);
        }

        public void DeleteMO2SAP(MO2SAP mo2sap)
        {
            this.DataProvider.Delete(mo2sap);
        }

        public object GetMO2SAP(string moCode, decimal postSeq)
        {
            return this._domainDataProvider.CustomSearch(typeof(MO2SAP), new object[] { moCode, postSeq });
        }

        public object[] GetMO2SAPList(string moCode, int inclusive, int exclusive)
        {
            string sql = "SELECT {0} FROM tblMO2SAP WHERE mocode='" + moCode.ToUpper() + "'";
            sql = string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO2SAP)));

            return this.DataProvider.CustomQuery(typeof(MO2SAP), new PagerCondition(sql, "postseq", inclusive, exclusive));
        }

        public object GetMO2SAPSumQty(string moCode)
        {
            string sql;
            sql = "";
            sql += "SELECT NVL (SUM (moproduced), 0) AS moproduced,";
            sql += "       NVL (SUM (moscrap), 0) AS moscrap";
            sql += "  FROM tblmo2sap";
            sql += " WHERE mocode = '" + moCode.ToUpper() + "'";

            object[] mo2sap = this.DataProvider.CustomQuery(typeof(MO2SAP), new SQLCondition(sql));
            return mo2sap[0];
        }

        public object GetMO2SAPMaxPostSeq(string moCode)
        {
            string sql = "SELECT NVL(MAX(postseq), 0) AS POSTSEQ FROM tblmo2sap WHERE mocode='" + moCode.ToUpper() + "'";
            object[] mo2sap = this.DataProvider.CustomQuery(typeof(MO2SAP), new SQLCondition(sql));
            return mo2sap[0];
        }

        public void UpdateMO2SAPFlag(string moCode, decimal postSeq)
        {
            string sql = "UPDATE tblmo2sap SET flag='SAP' WHERE mocode='" + moCode + "' AND postseq=" + postSeq;
            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }

        #endregion

        #region MO2SAPDetail
        public void AddMO2SAPDetail(MO2SAPDetail mo2sapdetail)
        {
            this.DataProvider.Insert(mo2sapdetail);
        }

        public void UpdateMO2SAPDetail(MO2SAPDetail mo2sapdetail)
        {
            this.DataProvider.Update(mo2sapdetail);
        }

        public void DeleteMO2SAPDetail(MO2SAPDetail mo2sapdetail)
        {
            this.DataProvider.Delete(mo2sapdetail);
        }

        public object GetMO2SAPDetail(string moCode, string rcard)
        {
            return this._domainDataProvider.CustomSearch(typeof(MO2SAPDetail), new object[] { moCode, rcard });
        }

        public object[] QueryMO2SAPDetailList(string moCode)
        {
            //string sql = "SELECT A.*  FROM (SELECT M.MOCODE AS MOCODE, 0 AS POSTSEQ,COUNT(S.SERIALNO) AS MOPRODUCED,";
            //sql += " 0 AS MOSCRAP,'' AS MOCONFIRM,0 AS MOMANHOUR,0 AS MOMACHINEHOUR,0 AS MOCLOSEDATE,G.SAPSTORAGE AS MOLOCATION,";
            //sql += " S.ITEMGRADE AS MOGRADE,M.MOOP AS MOOP, '' AS FLAG, '' AS ERRORMESSAGE, '' AS MUSER,0 AS MDATE,";
            //sql += " 0 AS MTIME, '' AS EATTRIBUTE1,0 AS ORGID";
            //sql += " FROM TBLSTACK2RCARD   S,  TBLSTORAGE   G, TBLSIMULATIONREPORT T,TBLMO  M, tblinvintransaction i ";
            //sql += " WHERE S.STORAGECODE = G.STORAGECODE AND s.serialno = t.rcard AND i.serial = s.transinserial AND i.mocode = t.mocode ";
            //sql += " AND T.ISCOM = 1 AND M.MOCODE = T.MOCODE AND M.itemcode=s.itemcode AND T.MOCODE = '" + moCode.Trim().ToUpper() + "'";
            //sql += " AND NOT EXISTS (SELECT *  FROM TBLMO2SAPDETAIL D  WHERE D.MOCODE = T.MOCODE   AND D.RCARD = T.RCARD)";
            //sql += " GROUP BY S.ITEMGRADE, G.SAPSTORAGE, M.MOOP, M.MOCODE";
            //sql += " UNION ALL  SELECT * FROM TBLMO2SAP where TBLMO2SAP.mocode='" + moCode.Trim().ToUpper() + "') A ORDER BY A.POSTSEQ";

            //�����е���6.24������һ������TBLInvInTransaction������,��Ϊ�˷�ֹһ����Ʒ������ͬ�����󶼿��Ա������������������ʵ��ʹ�ã�����������������SAP�������Ź���ʱ����Ҫ�����ģ���Ϊ��Ͷ���ˣ��������޸�ȥ��������

            string sql = "SELECT A.*  FROM (SELECT M.MOCODE AS MOCODE, 0 AS POSTSEQ,COUNT(S.SERIALNO) AS MOPRODUCED,";
            sql += " 0 AS MOSCRAP,'' AS MOCONFIRM,0 AS MOMANHOUR,0 AS MOMACHINEHOUR,0 AS MOCLOSEDATE,G.SAPSTORAGE AS MOLOCATION,";
            sql += " S.ITEMGRADE AS MOGRADE,M.MOOP AS MOOP, '' AS FLAG, '' AS ERRORMESSAGE, '' AS MUSER,0 AS MDATE,";
            sql += " 0 AS MTIME, '' AS EATTRIBUTE1,0 AS ORGID";
            sql += " FROM TBLSTACK2RCARD   S,  TBLSTORAGE   G, TBLSIMULATIONREPORT T,TBLMO  M ";
            sql += " WHERE S.STORAGECODE = G.STORAGECODE AND s.serialno = t.rcard ";
            sql += " AND T.ISCOM = 1  AND M.MOCODE = T.MOCODE AND M.itemcode=s.itemcode AND T.MOCODE = '" + moCode.Trim().ToUpper() + "'";
            //Added By Nettie Chen ON 2009/10/28
            sql += " AND t.eattribute1 ='GOOD' ";
            //End Added
            sql += " AND NOT EXISTS (SELECT *  FROM TBLMO2SAPDETAIL D  WHERE D.MOCODE = T.MOCODE   AND D.RCARD = T.RCARD)";
            sql += " GROUP BY S.ITEMGRADE, G.SAPSTORAGE, M.MOOP, M.MOCODE";
            sql += " UNION ALL  SELECT * FROM TBLMO2SAP where TBLMO2SAP.mocode='" + moCode.Trim().ToUpper() + "') A ORDER BY A.POSTSEQ";


            return this.DataProvider.CustomQuery(typeof(MO2SAP), new SQLCondition(sql));
        }

        public object[] QueryRcardFromStack2Rcard(string itemGrade, string sapStorage, string moCode)
        {
            //string sql = "SELECT C.RCARD AS SERIALNO  FROM TBLSTACK2RCARD A, TBLSTORAGE B, TBLSIMULATIONREPORT C, tblinvintransaction i ";
            //sql += " WHERE A.STORAGECODE = B.STORAGECODE AND a.serialno = c.rcard AND i.serial = a.transinserial AND i.mocode = c.mocode AND A.ITEMCODE=C.ITEMCODE";
            //sql += " AND C.ISCOM = 1  AND C.mocode='" + moCode.Trim() + "'  AND A.ITEMGRADE = '" + itemGrade.Trim() + "'    AND B.SAPSTORAGE = '" + sapStorage.Trim() + "' ";
            //sql += " AND NOT EXISTS  (SELECT * FROM TBLMO2SAPDETAIL D WHERE D.RCARD =C.RCARD";
            //sql += " AND D.MOCODE=C.MOCODE)";

            //changed by hiro 20091030 
            string sql = "SELECT C.RCARD AS SERIALNO  FROM TBLSTACK2RCARD A, TBLSTORAGE B, TBLSIMULATIONREPORT C ";
            sql += " WHERE A.STORAGECODE = B.STORAGECODE AND a.serialno = c.rcard   AND A.ITEMCODE=C.ITEMCODE AND C.EATTRIBUTE1 ='GOOD'";
            sql += " AND C.ISCOM = 1  AND C.mocode='" + moCode.Trim() + "'  AND A.ITEMGRADE = '" + itemGrade.Trim() + "'    AND B.SAPSTORAGE = '" + sapStorage.Trim() + "' ";
            sql += " AND NOT EXISTS  (SELECT * FROM TBLMO2SAPDETAIL D WHERE D.RCARD =C.RCARD";
            sql += " AND D.MOCODE=C.MOCODE)";
            //end

            return this.DataProvider.CustomQuery(typeof(StackToRcard), new SQLCondition(sql));
        }

        public object GetMaxPostSEQWithMOcode(string moCode)
        {
            string sql = "SELECT NVL(MAX(postseq), 0) AS POSTSEQ FROM tblmo2sapdetail WHERE mocode='" + moCode.ToUpper() + "'";
            object[] mo2sapdetail = this.DataProvider.CustomQuery(typeof(MO2SAPDetail), new SQLCondition(sql));
            return mo2sapdetail[0];
        }

        #endregion
        #region MO2SAPLog
        public void AddMO2SAPLog(MO2SAPLog log)
        {
            this.DataProvider.Insert(log);
        }

        public void UpdateMO2SAPLog(MO2SAPLog log)
        {
            this.DataProvider.Update(log);
        }

        public void DeleteMO2SAPLog(MO2SAPLog log)
        {
            this.DataProvider.Delete(log);
        }

        public object GetMO2SAPLog(string moCode, decimal postseq, int seq)
        {
            return this.DataProvider.CustomSearch(typeof(MO2SAPLog), new object[] { moCode, postseq, seq });
        }

        public object[] GetMO2SAPLogList(string moCode, decimal postSeq, int inclusive, int exclusive)
        {
            string sql = "SELECT {0} FROM tblmo2saplog WHERE active='Y' AND mocode='" + moCode + "' and postseq=" + postSeq;
            sql = string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO2SAPLog)));

            return this.DataProvider.CustomQuery(typeof(MO2SAPLog), new PagerCondition(sql, "seq", inclusive, exclusive));
        }

        public int GetMO2SAPLogListCount(string moCode, decimal postSeq)
        {
            string sql = "SELECT COUNT(*) FROM tblmo2saplog WHERE active='Y' AND mocode='" + moCode + "' and postseq=" + postSeq;
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        public void UpdateMO2SAPLogStatus(string moCode, decimal postSeq)
        {
            string sql = "UPDATE tblmo2saplog SET active='N' WHERE mocode='" + moCode + "' AND postseq=" + postSeq;

            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }

        public int GetMaxMO2SAPSequence(string moCode, decimal postSeq)
        {
            string sql = "SELECT NVL(MAX(seq),0) AS seq FROM tblmo2saplog WHERE mocode='" + moCode + "' AND postseq=" + postSeq;
            object mo2sapLog = this.DataProvider.CustomQuery(typeof(MO2SAPLog), new SQLCondition(sql))[0];
            return (mo2sapLog as MO2SAPLog).Sequence + 1;
        }
        #endregion

        #region MO,MOBaseData
        /// <summary>
        /// ** ������:
        /// ** ��������:
        ///     ���ڹ������е���ʱ�Թ�����Ⱥ��Ĳ���,
        ///     ����������Ѿ����ڵĹ��������ҹ�����״̬Ϊinitial��ʱ����й�����Ϣ�ĸ���
        ///     ����ط��Ƿ���Ҫ�汾�ĸ��£������������鲻��Ҫ),���״̬��Ϊinitial��������Ĵ˹���
        ///     ���������������Ѷ�Ӧ�Ĺ�����ӵ����ݿ�
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��: Crystal Chu
        /// ** �� ��: 2005-03-23
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// ** nunit
        /// </summary>
        /// <param name="mos"></param>
        public void DownLoadMOs(MO[] mos)
        {
            ItemFacade _itemFacade = new ItemFacade(this.DataProvider);

            try
            {
                this.DataProvider.BeginTransaction();
                for (int i = 0; i < mos.Length; i++)
                {
                    //����Ƿ����
                    object mo = GetMO(mos[i].MOCode);

                    if (mo != null)
                    {
                        //���ڼ�鹤����Ϊinitial

                        MO currentMO = (MO)mo;
                        object item = _itemFacade.GetItem(currentMO.ItemCode, GlobalVariables.CurrentOrganizations.First().OrganizationID);
                        if (currentMO.MOStatus == MOManufactureStatus.MOSTATUS_INITIAL)
                        {
                            //����Ӧ��item�Ƿ����

                            if (item != null)
                            {
                                //sammer kong 20050411
                                UpdateMO(mos[i], false);
                                //								this.DataProvider.Update(mos[i]);
                            }
                            else
                            {
                                _itemFacade.AddItem(_itemFacade.CreateDefaultItem(currentMO));
                                UpdateMO(mos[i], true);
                            }
                        }
                        //���ڼ�鹤����Ϊinitial�����������
                    }
                    else
                    {
                        object item = _itemFacade.GetItem(((MO)mo).ItemCode, GlobalVariables.CurrentOrganizations.First().OrganizationID);
                        //sammer kong 20050411
                        if (item != null)
                        {
                            _itemFacade.AddItem(_itemFacade.CreateDefaultItem((MO)mo));
                        }
                        AddMO(mos[i]);

                        //����Ӧ��item�Ƿ����
                        //						if(item != null)
                        //						{
                        //							this.DataProvider.Insert(mos[i]);
                        //						}
                        //						else
                        //						{
                        //							_itemFacade.AddItem(_itemFacade.CreateDefaultItem((MO)mo));
                        //							AddMO(mos[i]);
                        //						}
                    }
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                //_log.Error(ex.Message);
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_DownLoadMOs_Failure", ex);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade),ErrorCenter.ERROR_DOWNLOADMO),ex);
            }
        }


        public void AddMO(MO mo)
        {
            this._helper.AddDomainObject(mo);
        }


        public object GetMO(string moCode)
        {
            return this.DataProvider.CustomSearch(typeof(MO), new object[] { moCode });
        }


        /// <summary>
        /// ** ������:
        /// ** ��������:
        ///     ����ɾ���������жϹ�����״̬
        ///     ��������ѡ���;�̺ͽ��������̿��ķ�Χһ��ɾ��
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��:
        /// ** �� ��:
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// ** nunit
        /// </summary>
        /// <param name="mo"></param>
        private void DeleteMO(MO mo)
        {
            if (mo == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Null_Paramter");
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade),String.Format(ErrorCenter.ERROR_ARGUMENT_NULL,"mo")));
            }
            if ((mo.MOStatus != MOManufactureStatus.MOSTATUS_INITIAL))
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Delete_MOStatus", String.Format("[$MOStatus='{0}']", MOManufactureStatus.MOSTATUS_INITIAL), null);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade),String.Format(ErrorCenter.ERROR_MOSTATUS,MOSTATUS_INITIAL+" or "+MOSTATUS_PENDING)));
            }

            try
            {
                //this.DataProvider.BeginTransaction();
                //ɾ��;��ά����Ϣ
                object[] objs = QueryMORoutes(mo.MOCode, string.Empty);
                if (objs != null)
                {
                    for (int i = 0; i < objs.Length; i++)
                    {
                        this.DataProvider.Delete(objs[i]);
                    }
                }
                this.DataProvider.Delete(mo);
                //this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                //_log.Error(ex.Message,ex);
                //this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_DeleteMO", String.Format("[$MOCode='{0}']", mo.MOCode), ex);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade),String.Format(ErrorCenter.ERROR_DELETEMO,mo.MOCode)),ex);
            }

            //���̿��ķ�Χһ��ɾ��
        }


        public void DeleteMO(MO[] mos)
        {
            this.DataProvider.BeginTransaction();
            try
            {
                foreach (MO mo in mos)
                {
                    DeleteMO(mo);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_DeleteMO", ex);
                //                throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade),ErrorCenter.ERROR_DELETEMOS),ex);
            }


        }


        //        public void UpdateMO(MO mo)
        //        {
        //            UpdateMO(mo,true);
        //        }


        /// <summary>
        /// ** ������:
        /// ** ��������:
        ///     �����жϹ�����״̬���˴��������޸Ĳ��������Թ���״̬���޸ģ�
        ///     ����״̬���޸���MOStatusChanged
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��:   Crystal
        /// ** �� ��:   2005-03-23
        /// ** �� ��:  
        /// ** �� ��:
        /// ** �汾
        /// ** nunit
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="checkStatus">�Ƿ���Ҫ���״̬?Ϊtrue��ֻ�����޸�initial��pending״̬��MO,Ϊfalseʱ����������״̬��MO</param>
        public void UpdateMO(MO mo, bool checkStatus)
        {
            if (checkStatus)
            {
                MO oriMO = (MO)(this.GetMO(mo.MOCode));
                if ((oriMO.MOStatus != MOManufactureStatus.MOSTATUS_INITIAL) && (oriMO.MOStatus != MOManufactureStatus.MOSTATUS_PENDING))
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_MOStatus", String.Format("[$MOStatus='{0}','{1}']", MOManufactureStatus.MOSTATUS_INITIAL, MOManufactureStatus.MOSTATUS_PENDING), null);
                    //                    throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade),String.Format(ErrorCenter.ERROR_MOSTATUS,MOSTATUS_INITIAL+" or "+MOSTATUS_PENDING)));
                }
            }
            try
            {
                //Laws Lu,2006/11/13 uniform system collect date
                DBDateTime dbDateTime;

                dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

                mo.MaintainDate = dbDateTime.DBDate;
                mo.MaintainTime = dbDateTime.DBTime;
                this.DataProvider.Update(mo);
            }
            catch (Exception ex)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_UpdateMO", String.Format("[$MOCode='{0}']", mo.MOCode), ex);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade), String.Format(ErrorCenter.ERROR_UPDATEMO,mo.MOCode)), ex);
            }
        }
        //Laws Lu,2006/02/28,����	�����ı����������²�׼ȷ
        public void UpdateMOScrapQty(MO mo)
        {
            try
            {
                int iReturn = -1;
                iReturn = (this.DataProvider as SQLDomainDataProvider).CustomExecuteWithReturn(
                    new SQLCondition("update tblmo set MOSTATUS = '" + mo.MOStatus
                    + "',MOSCRAPQTY =  MOSCRAPQTY +" + mo.MOScrapQty + " where mocode='" + mo.MOCode + "'"));

                if (iReturn <= 0)
                {
                    new Exception("$Error_UpdateMO " + String.Format("[$MOCode='{0}']", mo.MOCode));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_UpdateMO", String.Format("[$MOCode='{0}']", mo.MOCode), ex);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade), String.Format(ErrorCenter.ERROR_UPDATEMO,mo.MOCode)), ex);
            }
        }

        public void UpdateMOInformation(MO mo, string routeCode)
        {
            OPBOMFacade opBOMFacade = new OPBOMFacade(this.DataProvider);
            BaseModelFacade baseModelFacade = new BaseModelFacade(this.DataProvider);
            this.DataProvider.BeginTransaction();
            try
            {
                this.UpdateMO(mo, true);
                MO2Route mo2Route = (MO2Route)GetMONormalRouteByMOCode(mo.MOCode);
                if (mo2Route != null)
                {
                    this.DeleteMORoute(mo2Route);
                }
                if (routeCode != string.Empty)
                {
                    Route route = (Route)baseModelFacade.GetRoute(routeCode);
                    OPBOM opBOM = opBOMFacade.GetOPBOMByRouteCode(mo.ItemCode, route.RouteCode, mo.OrganizationID, mo.BOMVersion);
                    MO2Route newMo2Route = new MO2Route();
                    newMo2Route.MOCode = mo.MOCode;
                    newMo2Route.IsMainRoute = ISMAINROUTE_TRUE;
                    newMo2Route.MaintainUser = mo.MaintainUser;
                    newMo2Route.OPBOMCode = opBOM.OPBOMCode;
                    newMo2Route.OPBOMVersion = opBOM.OPBOMVersion;
                    newMo2Route.RouteCode = route.RouteCode;
                    newMo2Route.RouteType = route.RouteType;
                    this.AddMORoute(newMo2Route);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_UpdateMOInformation", ex);
            }
        }





        /// <summary>
        /// ** ������:  QueryMO
        /// ** ��������:
        ///     ����Code,Item,MOType,MOStatus��Route��ѯ����
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��: viz0
        /// ** �� ��:   2005-03-23
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// </summary>
        /// <param name="moCode">ģ����ѯ</param>
        /// <param name="itemCode">ģ����ѯ</param>
        /// <param name="moType">��ȷ��ѯ</param>
        /// <param name="moStatus">��ȷ��ѯ</param>
        /// <param name="routeCode">��ȷ��ѯ</param>
        /// <param name="inclusive"></param>
        /// <param name="exclusive"></param>
        /// <returns>����MO���͵�����</returns>
        public object[] QueryMO(
            string moCode,
            string itemCode,
            string moType,
            string moStatus,
            string routeCode,
            string factory,
            int inclusive,
            int exclusive
            )
        {
            string condition = "";

            if (moCode != null && moCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE like '{0}%' ", moCode);
            }

            if (itemCode != null && itemCode.Trim().Length > 0)
            {
                condition += string.Format("and ITEMCODE like '{0}%' ", itemCode);
            }

            if (moType != null && moType.Trim().Length > 0)
            {
                condition += string.Format("and MOTYPE = '{0}' ", moType);
            }

            if (moStatus != null && moStatus.Trim().Length > 0)
            {
                condition += string.Format("and MOSTATUS = '{0}' ", moStatus);
            }

            if ((factory != null) && (factory.Trim() != string.Empty))
            {
                condition += string.Format(" and factory='{0}'", factory);
            }

            if (routeCode != null && routeCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE in ( select MOCODE from TBLMO2ROUTE where ROUTECODE = '{0}' ) ", routeCode);
            }



            string sql = string.Format("select {0} from TBLMO where 1=1 {1} " + GlobalVariables.CurrentOrganizations.GetSQLCondition(), DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)), condition);
            return this.DataProvider.CustomQuery(
                typeof(MO),
                new PagerCondition(sql, inclusive, exclusive));
        }

        public object[] QueryMOIllegibility(
            string moCode,
            string itemCode,
            string moType,
            string moStatus,
            string routeCode,
            string factory,
            int inclusive,
            int exclusive
            )
        {
            string condition = "";

            if (moCode != null && moCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE like '{0}%' ", moCode);
            }

            if (itemCode != null && itemCode.Trim().Length > 0)
            {
                condition += string.Format("and ITEMCODE like '{0}%' ", itemCode);
            }

            if (moType != null && moType.Trim().Length > 0)
            {
                condition += string.Format("and MOTYPE = '{0}' ", moType);
            }

            if (moStatus != null && moStatus.Trim().Length > 0)
            {
                condition += string.Format("and MOSTATUS = '{0}' ", moStatus);
            }

            if ((factory != null) && (factory.Trim() != string.Empty))
            {
                condition += string.Format(" and upper(factory) like'{0}%'", factory.ToUpper());
            }

            if (routeCode != null && routeCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE in ( select MOCODE from TBLMO2ROUTE where ROUTECODE = '{0}' ) ", routeCode);
            }



            string sql = string.Format("select {0} from TBLMO where 1=1 {1} " + GlobalVariables.CurrentOrganizations.GetSQLCondition(), DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)), condition);
            return this.DataProvider.CustomQuery(
                typeof(MO),
                new PagerCondition(sql, inclusive, exclusive));
        }

        /// <summary>
        /// added by jessie lee for AM0219, 2005/10/13, P4.10
        /// �����·�ʱ������Ϊɸѡ����
        /// modified by jessie lee, 2005/12/8
        /// </summary>
        public object[] QueryMOIllegibility(
            string moCode,
            string itemCode,
            string itemDesc,
            string moType,
            string moStatus,
            string routeCode,
            string factory,
            int ReleaseStartDate,
            int ReleaseEndDate,
            int ImportStartDate,
            int ImportEndDate,
            int inclusive,
            int exclusive
            )
        {
            return QueryMOIllegibility(moCode, itemCode, itemDesc, moType, moStatus, routeCode, factory,
                ReleaseStartDate, ReleaseEndDate, ImportStartDate, ImportEndDate, 0, 0, inclusive, exclusive);
        }

        public object[] QueryMOIllegibility(
           string moCode,
           string itemCode,
           string itemDesc,
           string moType,
           string moStatus,
           string routeCode,
           string factory,
           int ReleaseStartDate,
           int ReleaseEndDate,
           int ImportStartDate,
           int ImportEndDate,
           int actStarDate,
           int actEndDate,
           int inclusive,
           int exclusive
           )
        {
            string condition = "";

            if (moCode != null && moCode.Trim().Length > 0)
            {
                condition += string.Format("and a.MOCODE like '{0}%' ", moCode);
            }

            if (itemCode != null && itemCode.Trim().Length > 0)
            {
                condition += string.Format("and a.ITEMCODE like '{0}%' ", itemCode);
            }

            if (itemDesc != null && itemDesc.Trim().Length > 0)
            {
                condition += string.Format("and upper(a.ITEMDESC) like '%{0}%' ", itemDesc.ToUpper());
            }

            if (moType != null && moType.Trim().Length > 0)
            {
                condition += string.Format("and a.MOTYPE = '{0}' ", moType);
            }

            if (moStatus != null && moStatus.Trim().Length > 0)
            {
                condition += string.Format("and a.MOSTATUS = '{0}' ", moStatus);
            }

            if ((factory != null) && (factory.Trim() != string.Empty))
            {
                condition += string.Format(" and upper(a.factory) like'{0}%'", factory.ToUpper());
            }

            if (routeCode != null && routeCode.Trim().Length > 0)
            {
                condition += string.Format("and a.MOCODE in ( select MOCODE from TBLMO2ROUTE where ROUTECODE = '{0}' ) ", routeCode);
            }

            if (actStarDate > 0)
            {
                if (actEndDate == 0)
                {
                    condition += " and ( a.MOACTSTARTDATE >=" + actStarDate;
                }
                else
                {
                    condition += " and a.MOACTSTARTDATE >=" + actStarDate;
                }
            }

            if (actEndDate > 0)
            {
                if (actStarDate == 0)
                {
                    condition += " and a.MOACTSTARTDATE <=" + actEndDate + " and a.MOACTSTARTDATE>0";
                }
                else
                {
                    condition += " and a.MOACTSTARTDATE <=" + actEndDate;
                }

            }

            if (actStarDate > 0 && actEndDate == 0)
            {
                condition += " OR  a.MOACTSTARTDATE =" + 0 + ")";
            }

            string ReleaseDateCondition = string.Empty;
            string ImportDateCondition = string.Empty;
            if (ReleaseStartDate != 0)
            {
                ReleaseDateCondition = FormatHelper.GetDateRangeSql("a.MORELEASEDATE", ReleaseStartDate, ReleaseEndDate);
            }
            /* added by jessie lee, 2005/12/8 */
            if (ImportStartDate != 0)
            {
                ImportDateCondition = FormatHelper.GetDateRangeSql("a.MOIMPORTDATE", ImportStartDate, ImportEndDate);
            }

            string orgidCon = string.Empty;
            foreach (Organization org in GlobalVariables.CurrentOrganizations.GetOrganizationList())
            {
                orgidCon += org.OrganizationID + ",";
            }
            if (orgidCon.Length > 0)
            {
                orgidCon = orgidCon.Substring(0, orgidCon.Length - 1);

                orgidCon = " AND a.orgid IN (" + orgidCon + ") ";
            }

            string sql = string.Format(@"select a.*,b.itemname,b.itemdesc as ItemDescription from TBLMO a 
                LEFT JOIN TBLITEM b ON a.itemcode=b.itemcode and a.orgid = b.orgid where 1=1 {0} {1} {2} "
                + orgidCon, ReleaseDateCondition, ImportDateCondition, condition);
            return this.DataProvider.CustomQuery(
                typeof(MOWithItem),
                new PagerCondition(sql, inclusive, exclusive));
        }

        public object[] QueryMOIllegibility(
            string moCode,
            string itemCode,
            string moType,
            string[] moStatus,
            string routeCode,
            string factory,
            int inclusive,
            int exclusive
            )
        {
            string condition = "";

            if (moCode != null && moCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE like '{0}%' ", moCode);
            }

            if (itemCode != null && itemCode.Trim().Length > 0)
            {
                condition += string.Format("and ITEMCODE like '{0}%' ", itemCode);
            }

            if (moType != null && moType.Trim().Length > 0)
            {
                condition += string.Format("and MOTYPE = '{0}' ", moType);
            }

            if (moStatus != null && moStatus.Length > 0)
            {
                string in_status = "(''";
                foreach (string mo_status in moStatus)
                {
                    in_status += string.Format(",'{0}'", mo_status);
                }
                in_status += ")";
                condition += string.Format("and MOSTATUS IN {0} ", in_status);
            }

            if ((factory != null) && (factory.Trim() != string.Empty))
            {
                condition += string.Format(" and upper(factory) like'{0}%'", factory.ToUpper());
            }

            if (routeCode != null && routeCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE in ( select MOCODE from TBLMO2ROUTE where ROUTECODE = '{0}' ) ", routeCode);
            }

            string sql = string.Format("select {0} from TBLMO where 1=1 {1} " + GlobalVariables.CurrentOrganizations.GetSQLCondition(), DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)), condition);
            return this.DataProvider.CustomQuery(
                typeof(MO),
                new PagerCondition(sql, "MOSTATUS,MOCODE", inclusive, exclusive));
        }





        /// <summary>
        /// ** ������:  QueryMO
        /// ** ��������:
        ///     ����Code,Item,MOType,MOStatus��Route��ѯ����������
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��: viz0
        /// ** �� ��:   2005-03-23
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// </summary>
        /// <param name="moCode">ģ����ѯ</param>
        /// <param name="itemCode">ģ����ѯ</param>
        /// <param name="moType">��ȷ��ѯ</param>
        /// <param name="moStatus">��ȷ��ѯ</param>
        /// <param name="routeCode">��ȷ��ѯ</param>
        /// <param name="inclusive"></param>
        /// <param name="exclusive"></param>
        /// <returns>����MO������</returns>
        public int QueryMOCount(
            string moCode,
            string itemCode,
            string moType,
            string moStatus,
            string routeCode,
            string factory
            )
        {
            string condition = "";

            if (moCode != null && moCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE like '{0}%' ", moCode);
            }

            if (itemCode != null && itemCode.Trim().Length > 0)
            {
                condition += string.Format("and ITEMCODE like '{0}%' ", itemCode);
            }

            if (moType != null && moType.Trim().Length > 0)
            {
                condition += string.Format("and MOTYPE = '{0}' ", moType);
            }

            if (moStatus != null && moStatus.Trim().Length > 0)
            {
                condition += string.Format("and MOSTATUS = '{0}' ", moStatus);
            }

            if ((factory != null) && (factory.Trim() != string.Empty))
            {
                condition += string.Format(" and factory='{0}'", factory);
            }

            if (routeCode != null && routeCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE in ( select MOCODE from TBLMO2ROUTE where ROUTECODE = '{0}' ) ", routeCode);
            }


            string sql = string.Format("select count(mocode) from TBLMO where 1=1 {1} " + GlobalVariables.CurrentOrganizations.GetSQLCondition(), DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)), condition);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }


        public int QueryMOIllegibilityCount(
            string moCode,
            string itemCode,
            string moType,
            string moStatus,
            string routeCode,
            string factory
            )
        {
            string condition = "";

            if (moCode != null && moCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE like '{0}%' ", moCode);
            }

            if (itemCode != null && itemCode.Trim().Length > 0)
            {
                condition += string.Format("and ITEMCODE like '{0}%' ", itemCode);
            }

            if (moType != null && moType.Trim().Length > 0)
            {
                condition += string.Format("and MOTYPE = '{0}' ", moType);
            }

            if (moStatus != null && moStatus.Trim().Length > 0)
            {
                condition += string.Format("and MOSTATUS = '{0}' ", moStatus);
            }

            if ((factory != null) && (factory.Trim() != string.Empty))
            {
                condition += string.Format(" and upper(factory) like '{0}%'", factory.ToUpper());
            }

            if (routeCode != null && routeCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE in ( select MOCODE from TBLMO2ROUTE where ROUTECODE = '{0}' ) ", routeCode);
            }


            string sql = string.Format("select count(mocode) from TBLMO where 1=1 {1} " + GlobalVariables.CurrentOrganizations.GetSQLCondition(), DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)), condition);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moCode"></param>
        /// <param name="itemCode"></param>
        /// <param name="moType"></param>
        /// <param name="moStatus"></param>
        /// <param name="routeCode"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public int QueryMOIllegibilityCount(
            string moCode,
            string itemCode,
            string itemDesc,
            string moType,
            string moStatus,
            string routeCode,
            string factory,
            int ReleaseStartDate,
            int ReleaseEndDate,
            int ImportStartDate,
            int ImportEndDate
            )
        {
            return QueryMOIllegibilityCount(moCode, itemCode, itemDesc, moType, moStatus, routeCode, factory,
                                            ReleaseStartDate, ReleaseEndDate, ImportStartDate, ImportEndDate, 0, 0);
        }


        public int QueryMOIllegibilityCount(
        string moCode,
        string itemCode,
        string itemDesc,
        string moType,
        string moStatus,
        string routeCode,
        string factory,
        int ReleaseStartDate,
        int ReleaseEndDate,
        int ImportStartDate,
        int ImportEndDate,
        int actStarDate,
        int actEndDate
        )
        {
            string condition = "";

            if (moCode != null && moCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE like '{0}%' ", moCode);
            }

            if (itemCode != null && itemCode.Trim().Length > 0)
            {
                condition += string.Format("and ITEMCODE like '{0}%' ", itemCode);
            }

            if (itemDesc != null && itemDesc.Trim().Length > 0)
            {
                condition += string.Format("and upper(ITEMDESC) like '%{0}%' ", itemDesc.ToUpper());
            }

            if (moType != null && moType.Trim().Length > 0)
            {
                condition += string.Format("and MOTYPE = '{0}' ", moType);
            }

            if (moStatus != null && moStatus.Trim().Length > 0)
            {
                condition += string.Format("and MOSTATUS = '{0}' ", moStatus);
            }

            if ((factory != null) && (factory.Trim() != string.Empty))
            {
                condition += string.Format(" and upper(factory) like '{0}%'", factory.ToUpper());
            }

            if (routeCode != null && routeCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE in ( select MOCODE from TBLMO2ROUTE where ROUTECODE = '{0}' ) ", routeCode);
            }

            if (actStarDate > 0)
            {
                if (actEndDate == 0)
                {
                    condition += " and ( MOACTSTARTDATE >=" + actStarDate;
                }
                else
                {
                    condition += " and MOACTSTARTDATE >=" + actStarDate;
                }
            }

            if (actEndDate > 0)
            {
                if (actStarDate == 0)
                {
                    condition += " and MOACTSTARTDATE <=" + actEndDate + " and MOACTSTARTDATE>0";
                }
                else
                {
                    condition += " and MOACTSTARTDATE <=" + actEndDate;
                }

            }

            if (actStarDate > 0 && actEndDate == 0)
            {
                condition += " or  MOACTSTARTDATE =" + 0 + ")";
            }

            string ReleaseDateCondition = string.Empty;
            string ImportDateCondition = string.Empty;
            if (ReleaseStartDate != 0)
            {
                ReleaseDateCondition = FormatHelper.GetDateRangeSql("MORELEASEDATE", ReleaseStartDate, ReleaseEndDate);
            }
            /* added by jessie lee, 2005/12/8 */
            if (ImportStartDate != 0)
            {
                ImportDateCondition = FormatHelper.GetDateRangeSql("MOIMPORTDATE", ImportStartDate, ImportEndDate);
            }

            string sql = string.Format("select count(mocode) from TBLMO where 1=1 {0} {1} {2} " + GlobalVariables.CurrentOrganizations.GetSQLCondition(), ReleaseDateCondition, ImportDateCondition, condition);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        public int QueryMOIllegibilityCount(
            string moCode,
            string itemCode,
            string moType,
            string[] moStatus,
            string routeCode,
            string factory
            )
        {
            string condition = "";

            if (moCode != null && moCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE like '{0}%' ", moCode);
            }

            if (itemCode != null && itemCode.Trim().Length > 0)
            {
                condition += string.Format("and ITEMCODE like '{0}%' ", itemCode);
            }

            if (moType != null && moType.Trim().Length > 0)
            {
                condition += string.Format("and MOTYPE = '{0}' ", moType);
            }

            if (moStatus != null && moStatus.Length > 0)
            {
                string in_status = "(''";
                foreach (string mo_status in moStatus)
                {
                    in_status += string.Format(",'{0}'", mo_status);
                }
                in_status += ")";
                condition += string.Format("and MOSTATUS IN {0} ", in_status);
            }

            if ((factory != null) && (factory.Trim() != string.Empty))
            {
                condition += string.Format(" and upper(factory) like '{0}%'", factory.ToUpper());
            }

            if (routeCode != null && routeCode.Trim().Length > 0)
            {
                condition += string.Format("and MOCODE in ( select MOCODE from TBLMO2ROUTE where ROUTECODE = '{0}' ) ", routeCode);
            }


            string sql = string.Format("select count(mocode) from TBLMO where 1=1 {1} " + GlobalVariables.CurrentOrganizations.GetSQLCondition(), DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)), condition);
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        /// <summary>
        /// ** ������: IsMOStatusChanged
        /// ** ��������: �жϹ���״̬�Ƿ�ı��
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��: vizo
        /// ** �� ��: 2005-03-23
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// </summary>
        /// <param name="mo"></param>
        /// <returns>true �Ѹı� , false δ�ı�</returns>
        public bool IsMOStatusChanged(MO mo)
        {
            MO oriMO = (MO)GetMO(mo.MOCode);
            return oriMO.MOStatus != mo.MOStatus;
        }


        /// <summary>
        /// ** ������: MOCheck
        /// ** ��������: �ͼ�������, ����check
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��: roger.xue
        /// ** �� ��: 2008-08-21
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// </summary>
        /// <param name="mo"></param>
        /// <returns>true �Ѹı� , false δ�ı�</returns>
        public void MOCheck(MO mo)
        {
            ItemFacade itemFacade = new ItemFacade(this.DataProvider);
            BaseModelFacade bmf = new BaseModelFacade(this.DataProvider);
            object objItem = itemFacade.GetItem(mo.ItemCode, mo.OrganizationID);

            Item item = objItem as Item;

            if (item.CheckItemOP == null || item.CheckItemOP.Trim() == String.Empty)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_NoItemGenerateLotOPCode");
            }
            else
            {
                object[] moroutes = this.GetAllMORoutes(mo.MOCode);
                Route route = moroutes[0] as Route;

                if (bmf.GetRoute2Operation(route.RouteCode, item.CheckItemOP) == null)
                {
                    ExceptionManager.Raise(this.GetType(), "$CS_CheckOPNotInMOOPList $CS_CheckOPCode=" + item.CheckItemOP + " $Domain_MO2Route=" + route.RouteCode);
                }
            }

            if (item.LotSize == null || (int)item.LotSize <= 0)
            {
                ExceptionManager.Raise(this.GetType(), "$CS_PleaseMaintainLotSize");
            }
        }


        /// <summary>
        /// ** ������: MOStatusChanged
        /// ** ��������:
        ///     ֻ�Թ�����״̬���޸�
        ///     initial->release 
        ///     release->initial
        ///     open->pending ����һ�����ߵĶ���
        ///     pending->open�����ٵ����ߵĶ��������а��������汾������,
        ///     ͬʱ�����鹤��;�̣�����;�̣���BOM�İ汾�Ƿ��б仯��������
        ///     ����б����滻������µİ汾
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��: crystal
        /// ** �� ��:
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// ** nunit
        /// </summary>
        /// <param name="mo"></param>
        public void MOStatusChanged(BenQGuru.eMES.Domain.MOModel.MO mo)
        {
            if (mo == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Null_Paramter");
            }
            MO origianlMO = (MO)GetMO(mo.MOCode);
            if (origianlMO.MOStatus == mo.MOStatus)
            {
                return;
            }

            #region Marked Code ,Backup
            if ((origianlMO.MOStatus == MOManufactureStatus.MOSTATUS_INITIAL) && (mo.MOStatus == MOManufactureStatus.MOSTATUS_RELEASE))
            {
                //add by crystal  2005/05/09 �����������ع�������release
                SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
                Parameter paramter = (Parameter)systemSettingFacade.GetParameter(origianlMO.MOType, BenQGuru.eMES.Web.Helper.MOType.GroupType);
                object[] objs = null;
                object objRMA = null;

                if (mo.MOType == paramter.ParameterCode && mo.MOType == "RMA")
                {
                    if (mo.RMABillCode.Trim() == String.Empty)
                    {
                        ExceptionManager.Raise(this.GetType(), "$MO_RMACODE_MUST");
                    }

                    objRMA = this.GetRMARCARDByMoCode(mo.MOCode);

                    if (objRMA == null)
                    {
                        ExceptionManager.Raise(this.GetType(), "$MO_RMACODE_MUST");
                    }
                }

                if (paramter.ParameterValue == BenQGuru.eMES.Web.Helper.MOType.MOTYPE_REWORKMOTYPE && mo.MOType != paramter.ParameterCode)
                {
                    //					ReworkFacade reworkFacade = new ReworkFacade(this.DataProvider);
                    objs = this.QueryReworkSheet(string.Empty, string.Empty, mo.ItemCode, mo.MOCode, ReworkStatus.REWORKSTATUS_OPEN, "", "", int.MinValue, int.MaxValue);
                    if (objs == null)
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_ReworkSheet_NotRelease", String.Format("[$MOCode='{0}']", mo.MOCode));
                    }
                }
                else
                {

                }
                //���;�̵�ά��
                objs = QueryMORoutes(mo.MOCode, string.Empty, ISMAINROUTE_TRUE);
                if (objs == null)
                {
                    //add by roger.xue 2008/08/19 for hisense
                    /*-------�����·���ʱ��,���������;��û��ά��,��Ѳ�Ʒ��Ĭ��;��update�������� ------*/
                    object objDefaultRoute = this.GetDefaultItem2Route(mo.ItemCode);

                    if (objDefaultRoute != null)
                    {
                        DefaultItem2Route defaultItem2Route = objDefaultRoute as DefaultItem2Route;
                        string routecode = defaultItem2Route.RouteCode;

                        MO2Route mo2Route = new MO2Route();

                        mo2Route.MOCode = mo.MOCode;
                        mo2Route.RouteCode = routecode;
                        mo2Route.RouteType = RouteType.Normal;
                        mo2Route.OPBOMCode = routecode; //
                        mo2Route.OPBOMVersion = mo.BOMVersion;
                        mo2Route.IsMainRoute = "1";
                        mo2Route.EAttribute1 = "";
                        mo2Route.MaintainUser = mo.MaintainUser;

                        this.AddMORoute(mo2Route);
                    }
                    else
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_MORoute_NotExist", String.Format("[$MOCode='{0}']", mo.MOCode));
                    }
                }
                //������Ϲ����Ƿ���ά��BOM
                if (!this.CheckMORouteItemLoadingOPBOM(mo))
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_OPBOMNotExist1", String.Format("[$MOCode='{0}']", mo.MOCode));
                }

                //�����·�ʱ��,���ò�Ʒ��Ӧ�ġ������ͼ������� �͡��������Ƿ���ά��
                //MOCheck(mo);

                //���±ȶ�ȡ�� 6��15�� Custom Feedback��CS0018Ҫ��  modify by Simone
                //���bom�ȶ�
                //				if(origianlMO.IsBOMPass ==IsPass.ISPASS_NOPASS.ToString())
                //				{
                //					ExceptionManager.Raise(this.GetType(),"$Error_MOBOM_FailureCompare",String.Format("[$MOCode='{0}']",mo.MOCode));
                //				}

                //add by crystal  2005/04 /20 chu �汾������
                Decimal moVersion = 0;
                try
                {
                    moVersion = System.Decimal.Parse(origianlMO.MOVersion) + 1;
                }
                catch
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_ConvertMOVersion_Failure");
                }
                mo.MOVersion = moVersion.ToString();
                mo.MOSeq = GetNextMOSeq();
                UpdateMO(mo, false);
                //Laws Lu,2006/07/07 add ,Success release than delete rma
                if (objRMA != null)
                {
                    this.DeleteRMARCARD(objRMA as RMARCARD);
                }

                this.SyncOPBomDetailToWHItem(mo.MOCode);//�����·�����ȡ����ͣ��ʱ��,ͬ���ⷿ����������.
                return;
            }
            else if ((origianlMO.MOStatus == MOManufactureStatus.MOSTATUS_RELEASE) && (mo.MOStatus == MOManufactureStatus.MOSTATUS_INITIAL))
            {
                UpdateMO(mo, false);

                //add by roger.xue 2008/08/19 for hisense
                /*------- ��ȡ���·���ʱ��ѹ�����;��ɾ�� ------*/
                object[] mo2Routes = QueryMORoutes(mo.MOCode, string.Empty, ISMAINROUTE_TRUE);
                if (mo2Routes != null && mo2Routes.Length > 0)
                {
                    MO2Route mo2Route = (MO2Route)mo2Routes[0];

                    this.DeleteMORoute(mo2Route);
                }
                //end
                return;
            }
            else if ((origianlMO.MOStatus == MOManufactureStatus.MOSTATUS_PENDING) && (mo.MOStatus == MOManufactureStatus.MOSTATUS_OPEN))
            {
                //add by crystal  2005/04 /20 chu �汾������
                Decimal moVersion = 0;
                try
                {
                    moVersion = System.Decimal.Parse(origianlMO.MOVersion) + 1;
                }
                catch
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_ConvertMOVersion_Failure");
                }
                //������Ϲ����Ƿ���ά��BOM
                if (!this.CheckMORouteItemLoadingOPBOM(mo))
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_OPBOMNotExist1", String.Format("[$MOCode='{0}']", mo.MOCode));
                }

                //ȡ����ͣʱ��,���ò�Ʒ��Ӧ�ġ������ͼ������� �͡��������Ƿ���ά��
                //MOCheck(mo);

                mo.MOVersion = moVersion.ToString();
                UpdateMO(mo, false);
                this.SyncOPBomDetailToWHItem(mo.MOCode);//�����·�����ȡ����ͣ��ʱ��,ͬ���ⷿ����������.
                return;
            }
            else if ((origianlMO.MOStatus == MOManufactureStatus.MOSTATUS_OPEN) && (mo.MOStatus == MOManufactureStatus.MOSTATUS_PENDING))
            {
                UpdateMO(mo, false);
                return;
            }
            else if (origianlMO.MOStatus == MOManufactureStatus.MOSTATUS_OPEN && mo.MOStatus == MOManufactureStatus.MOSTATUS_CLOSE)
            {
                //�ص����߼�: �ڹ���û�С�����Ʒ���͡�����Ʒ��ʱ��������й����ص���(���߼�����ʹ��)
                //modified by jessie lee for AM0245, 2005/10/19, P4.12
                //����ȷ��������Ʒ�����Ͷ������ʵ���깤�����������0����ʱ�ǿ��Թص��ġ����Ͷ������ʵ���깤���������>0,��ʾ������������Ʒ
                if (MOCloseCheck(mo))
                {
                    UpdateMO(mo, false);  // �ı乤��״̬Ϊ�ص�
                    ClearSimulation(mo); // ���Simulation��ر�
                    Deletetblpackingchk(); // ���tblpackingchk
                }
                return;
            }
            else if (origianlMO.MOStatus == MOManufactureStatus.MOSTATUS_INITIAL && mo.MOStatus == MOManufactureStatus.MOSTATUS_CLOSE)
            {
                /* added by jessie lee, 2005/12/8
                 * CS187 : �Գ�ʼ���Ĺ������йص�����ʼ���Ĺ���ʹ�á��ص�����ť���йص����ص����״̬ʹ���������ص���ͬ������״̬�����ѹص��� */
                UpdateMO(mo, false);  // �ı乤��״̬Ϊ�ص�
                return;
            }
            #endregion

            //sammer kong 20050411 exception message too simple
            ExceptionManager.Raise(this.GetType(), "$Error_MOStatus_Changed", String.Format("[ $MOCode='{0}', $MOStatus ${1}->${2} ]", mo.MOCode, origianlMO.MOStatus, mo.MOStatus), null);
        }
        private decimal GetNextMOSeq()
        {
            string strSql = "SELECT MAX(MOSEQ) MOSEQ FROM TBLMO";
            object[] objs = this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(strSql));
            decimal dSeq = 1;
            if (objs != null && objs.Length > 0)
                dSeq = ((MO)objs[0]).MOSeq + 1;
            return dSeq;
        }

        #region �رչ������ ˽�з���

        /// <summary>
        /// �ص�check
        /// �ص����߼�: �ڹ���û�С�����Ʒ���͡�����Ʒ��ʱ��������й����ص���(���߼�����ʹ��)
        /// modified by jessie lee for AM0245, 2005/10/19, P4.12
        /// ����ȷ��������Ʒ�����Ͷ������ʵ���깤�����������0����ʱ�ǿ��Թص��ġ����Ͷ������ʵ���깤���������>0,��ʾ������������Ʒ
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        private bool MOCloseCheck(MO mo)
        {
            bool returnBool = false;

            #region ����Ƿ�������Ʒ
            if (CheckMOSimulation(mo))
            {
                returnBool = true;
            }
            else
            {
                returnBool = false;
                ExceptionManager.Raise(this.GetType(), "$Error_MOCloseFailure_Simulation");
            }
            #endregion

            #region ����Ƿ�������Ʒ
            if (CheckMOTS(mo))
            {
                returnBool = true;
            }
            else
            {
                returnBool = false;
                ExceptionManager.Raise(this.GetType(), "$Error_MOCloseFailure_TS");
            }
            #endregion

            return returnBool;
        }
        //����Ƿ�������Ʒ
        private bool CheckMOSimulation(MO mo)
        {
            bool returnBool = false;
            string sql = string.Format("select COUNT(RCARD) from TBLSimulation where mocode = '{0}' AND ISCOM ='0'", mo.MOCode);

            if (this.DataProvider.GetCount(new SQLCondition(sql)) > 0)
            {
                returnBool = false;
            }
            else
            {
                returnBool = true;
            }
            return returnBool;
        }

        /// <summary>
        /// ����Ƿ�������Ʒ
        /// ���Ͷ������ʵ���깤���������-���빤��������0����ʱ�ǿ��Թص��ġ����Ͷ������ʵ���깤���������>0,��ʾ������������Ʒ
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        private bool CheckMOTS(MO mo)
        {
            //�ص�����Ʒ�ļ�鲻��������ά�޵Ĳ�Ʒ,ֻ�������ά��
            //TSSource.TSSource_OnWIP //����ά��	
            //TSSource.TSSource_TS	  //����ά�� 
            /*
            bool returnBool = false;
			
            string sql = string.Format(@"select COUNT(RCARD) from TBLTS where mocode = '{0}' AND FRMINPUTTYPE = '{1}' 
                            AND  TSSTATUS  in ('"+TSStatus.TSStatus_New+"','"+TSStatus.TSStatus_Confirm+"','"+TSStatus.TSStatus_TS+"')",mo.MOCode,TSSource.TSSource_OnWIP);
			
            if(this.DataProvider.GetCount( new SQLCondition(sql) ) > 0)
            {
                returnBool = false;
            }
            else
            {
                returnBool = true;
            }
            return returnBool;
            */

            decimal tsCount = mo.MOInputQty - mo.MOActualQty - mo.MOScrapQty - mo.MOOffQty;
            if (tsCount == 0)
            {
                //if (mo.MOInputQty >= mo.MOPlanQty)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return true;
            }
            else return false;
        }

        #endregion

        #region ��鹤��;���Ƿ������Ϲ���,�Լ��Ƿ�ά�����Ϲ���BOM

        private bool CheckMORouteItemLoadingOPBOM(BenQGuru.eMES.Domain.MOModel.MO mo)
        {
            MO2Route mo2Route = (MO2Route)GetMONormalRouteByMOCode(mo.MOCode);
            //Route route = (Route) baseModelFacade.GetRoute(routeCode);

            string sql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOM)) + " from tblopbom where 1=1 and opbomver='" + mo.BOMVersion + "' AND itemcode ='" + mo.ItemCode.Trim() + "' and obroute ='" + mo2Route.RouteCode.Trim() + "' and orgid=" + mo.OrganizationID;

            OPBOMFacade opBOMFacade = new OPBOMFacade(this.DataProvider);
            //����;���Ƿ������Ϲ���
            bool IsComponetLoading = opBOMFacade.CheckItemRouteIsContainComponetLoading(mo.ItemCode, mo2Route.RouteCode, mo.OrganizationID);
            object[] objsOp = opBOMFacade.GetItemRouteIsContainComponetLoading(mo.ItemCode, mo2Route.RouteCode, mo.OrganizationID);
            object[] objs = this.DataProvider.CustomQuery(typeof(OPBOM), new SQLCondition(String.Format(sql)));


            //��鹤��;���Ƿ������Ϲ���,�������OPBOM������Ϊ�ղ��׳��쳣,����������һ��OPBOM�������ݿ�,���������OPBOM,�����׳��쳣
            //	Modify By Simone Xu 2005/08/15
            if (IsComponetLoading)
            {
                if (objs == null)
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_OPBOMNotExist", string.Format("[$ItemCode='{0}',$RouteCode='{1}',$BOMVersion='{2}']", mo.ItemCode, mo2Route.RouteCode, mo.BOMVersion), null);
                    return false;
                }
                else
                {
                    foreach (object opboms in objsOp)
                    {
                        string selectSql = "select count(*) from tblopbomdetail where 1= 1 {0} and opbomver='" + mo.BOMVersion + "' and orgid=" + mo.OrganizationID;
                        string tmpString2 = " and itemcode ='" + mo.ItemCode.Trim() + "'";
                        tmpString2 += " and opcode='" + ((ItemRoute2OP)opboms).OPCode.Trim() + "' ";
                        string opcodesSelect = string.Format(@"AND obcode in (''");
                        foreach (object opbom in objs)
                        {
                            opcodesSelect += string.Format(",'{0}'", ((OPBOM)opbom).OPBOMCode);
                        }
                        opcodesSelect += ")";
                        tmpString2 += opcodesSelect;
                        if (!(this.DataProvider.GetCount(new SQLCondition(String.Format(selectSql, tmpString2))) > 0))
                        {
                            ExceptionManager.Raise(this.GetType(), "$Error_OPBOMNotExist1", string.Format("[$ItemCode='{0}',$RouteCode='{1}',$OpCode='{2}',$BOMVersion='{3}']", mo.ItemCode, mo2Route.RouteCode, ((ItemRoute2OP)opboms).OPCode, mo.BOMVersion), null);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        #endregion

        #region �����·�����ȡ����ͣ��ʱ��,ͬ���ⷿ����������.

        private void SyncOPBomDetailToWHItem(string mocode)
        {
            //��ȡ������Ӧ�Ĺ���bom�����嵥
            string opbomdetailSql = string.Format(@"SELECT tblopbomdetail.*
															FROM tblopbomdetail
															WHERE (obcode, opcode) IN (SELECT routecode, opcode
																						FROM tblroute2op
																						WHERE routecode IN (SELECT routecode
																											FROM tblmo2route
																											WHERE mocode = '{0}'))" + GlobalVariables.CurrentOrganizations.GetSQLCondition(), mocode);
            object[] opbomDetailObjs = this.DataProvider.CustomQuery(typeof(OPBOMDetail), new SQLCondition(opbomdetailSql));

            //�жϿⷿ�����������Ƿ��д���Ʒ
            //ͬ������bom�Ϳⷿ���������������嵥.
            WarehouseFacade wareFacade = new WarehouseFacade(this.DataProvider);
            wareFacade.AddWarehouseItem(opbomDetailObjs);
        }

        #endregion
        
				/// <summary>
        /// ���ݹ����źͲ�Ʒ���룬��ѯ���깤�Ĳ�Ʒ���к�
        /// </summary>
        /// <param name="moCode"></param>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string[] GetRcardsByMoAndItem(string moCode,string itemCode)
        {
            string sql = string.Format("SELECT rcard from tblsimulation WHERE  mocode = '{0}' AND itemcode='{1}' AND iscom=1", moCode, itemCode);
            return this.DataProvider.GetStringResult(new SQLCondition(sql));
        }

        /// <summary>
        /// ����OQC���źͲ�Ʒ���룬��ѯ���깤�Ĳ�Ʒ���к�
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string[] GetRcardsByOqcAndItem(string lotNo, string itemCode)
        {
            string sql = string.Format("SELECT rcard from tblsimulation WHERE  lotno = '{0}' AND itemcode='{1}' AND iscom=1", lotNo, itemCode);
            return this.DataProvider.GetStringResult(new SQLCondition(sql));
        }


        public object[] QueryReworkSheet(string reworkSheetCode, string modelCode, string itemCode, string moCode, string reworkStatus, string lotno, string runningCard, int inclusive, int exclusive)
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

            string rejectCondition;
            if (lotno == string.Empty && runningCard == string.Empty)
            {
                rejectCondition = " 1=1 ";
            }
            else
            {
                rejectCondition = string.Format("reworkcode in(select tblreworkrange.reworkcode from tblreworkrange,tblreject where tblreject.rcard = tblreworkrange.rcard and tblreject.rcardseq =tblreworkrange.rcardseq and tblreject.lotno like '{0}%' and tblreject.rcard like '{1}%' )", lotno, runningCard);
            }


            string sql1 = "select {0} from tblreworksheet where reworkcode like '{1}%' and reworktype = '{2}' and {3} and {4} and {5} and {6} and {7}";

            sql1 = string.Format(
                sql1,
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)),
                reworkSheetCode,
                BenQGuru.eMES.Web.Helper.ReworkType.REWORKTYPE_REMO,
                itemCondition,
                moCondition,
                statusCondition,
                rejectCondition,
                modelCondition
                );






            string sql2 = "select {0} from tblreworksheet where reworkcode like '{1}%' and reworktype = '{2}' and {3} and {4} and {5} and {6} and {7}";

            sql2 = string.Format(
                sql2,
                DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)),
                reworkSheetCode,
                BenQGuru.eMES.Web.Helper.ReworkType.REWORKTYPE_ONLINE,
                itemCondition,
                moCondition,
                statusCondition,
                rejectCondition,
                modelCondition
                );




            string sql = string.Format("select {0} from ({1} union all {2}) ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReworkSheet)), sql1, sql2);
            return this.DataProvider.CustomQuery(typeof(ReworkSheet), new PagerCondition(sql, "REWORKCODE", inclusive, exclusive));

        }

        public void MOStatusChanged(BenQGuru.eMES.Domain.MOModel.MO[] mos)
        {
            this._domainDataProvider.BeginTransaction();
            try
            {
                foreach (MO mo in mos)
                {
                    MOStatusChanged(mo);
                }
                this._domainDataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this._domainDataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_MOStatussChanged", ex);
            }
        }


        /// <summary>
        /// ���������İ汾
        /// </summary>
        /// <param name="mo"></param>
        private void MOUpgradeVersion(MO mo)
        {
        }


        /// <summary>
        /// �жϹ�����ѡ��BOM�汾�Ƿ�Ϊ����
        /// ���ݹ�����Item+Route�ҵ�OPBOM���°汾OPBOMDescription
        /// Ȼ��ͨ���ȽϹ���OPBOMVersion �����һ����Ϊ����true
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        private bool IsBOMVersionChange(MO mo)
        {
            return true;
        }




        /// <summary>
        /// �жϹ����Ƿ�Ϊ���ϣ������Ѿ�����
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        public bool IsOnline(BenQGuru.eMES.Domain.MOModel.MO mo)
        {
            return true;
        }



        #region MORoute
        public MO2Route CreateNewMO2Route()
        {
            return new MO2Route();
        }
        public void AddMORoute(MO2Route mo2Route)
        {
            this._helper.AddDomainObject(mo2Route);
        }

        /// <summary>
        /// ֻ���޸Ķ�Ӧ��BOM
        /// </summary>
        /// <param name="mo2Route"></param>
        public void UpdateMORoute(MO2Route mo2Route)
        {
            MO oriMO = (MO)(this.GetMO(mo2Route.MOCode));
            if ((oriMO.MOStatus != MOManufactureStatus.MOSTATUS_INITIAL) && (oriMO.MOStatus != MOManufactureStatus.MOSTATUS_PENDING))
            {
                ExceptionManager.Raise(this.GetType(), "$Error_MOStatus", String.Format("[$MOStatus='{0}','{1}']", MOManufactureStatus.MOSTATUS_INITIAL, MOManufactureStatus.MOSTATUS_PENDING), null);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade),String.Format(ErrorCenter.ERROR_MOSTATUS,MOSTATUS_INITIAL+" or "+MOSTATUS_PENDING)));
            }
            this._helper.UpdateDomainObject(mo2Route);
        }

        /// <summary>
        /// ���빤��״̬��initial�������Ϲ���;�̺Ͷ�Ӧ�������嵥����Ϣ
        /// </summary>
        /// <param name="mo2Route"></param>
        public void DeleteMORoute(MO2Route mo2Route)
        {
            MO oriMO = (MO)(this.GetMO(mo2Route.MOCode));
            if ((oriMO.MOStatus != MOManufactureStatus.MOSTATUS_INITIAL) && (oriMO.MOStatus != MOManufactureStatus.MOSTATUS_PENDING))
            {
                ExceptionManager.Raise(this.GetType(), "$Error_MOStatus", String.Format("[$MOStatus='{0}','{1}']", MOManufactureStatus.MOSTATUS_INITIAL, MOManufactureStatus.MOSTATUS_PENDING), null);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade),String.Format(ErrorCenter.ERROR_MOSTATUS,MOSTATUS_INITIAL+" or "+MOSTATUS_PENDING)));
            }
            this._helper.DeleteDomainObject(mo2Route);
        }



        public object GetMONormalRouteByMOCode(string moCode)
        {
            object[] objs = QueryMORoutes(moCode, string.Empty, ISMAINROUTE_TRUE);
            if (objs != null)
            {
                return objs[0];
            }
            return null;
        }

        public object[] QueryMORoutes(string moCode, string routeCode)
        {
            return QueryMORoutes(moCode, routeCode, string.Empty, string.Empty);
        }

        public object[] QueryMORoutes(string moCode, string routeCode, string isMainRoute)
        {
            return QueryMORoutes(moCode, routeCode, isMainRoute, string.Empty);
        }

        public object[] QueryMORoutes(string moCode, string routeCode, string isMainRoute, string opBOMVersion)
        {
            string selectSql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO2Route)) + " from tblmo2route where {0}";

            string tmpString = "1=1";
            if ((moCode != string.Empty) && (moCode.Trim() != string.Empty))
            {
                tmpString += " and mocode = '" + moCode.Trim() + "'";
            }
            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and routecode ='" + routeCode.Trim() + "'";
            }
            if ((isMainRoute != string.Empty) && (isMainRoute.Trim() != string.Empty))
            {
                tmpString += " and ismroute ='" + isMainRoute.Trim() + "'";
            }
            if ((opBOMVersion != string.Empty) && (opBOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + opBOMVersion.Trim() + "'";
            }
            return this.DataProvider.CustomQuery(typeof(MO2Route), new SQLCondition(String.Format(selectSql, tmpString)));
        }


        /// <summary>
        /// �жϸ�;����û���ڹ�����ʹ�ã�����б�ʹ���򷵻�true
        /// </summary>
        /// <param name="routeCode">����Ϊ��</param>
        /// <returns>���ز�����</returns>
        public bool IsModelRouteUsed(string routeCode)
        {
            if ((routeCode == string.Empty) || (routeCode.Trim() == string.Empty))
            {
                //sammer kong
                ExceptionManager.Raise(this.GetType(), "$Error_Argument_Null", "routeCode", null);
                //				throw new ArgumentException(ErrorCenter.GetErrorServerDescription(typeof(MOFacade),String.Format(ErrorCenter.ERROR_ARGUMENT_NULL,routeCode)));
            }
            if (this.DataProvider.GetCount(new SQLCondition(String.Format("select count(*) from tblmo2route where routecode ='{0}'", "routeCode"))) > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// �жϸ�opbom�Ͷ�Ӧ�İ汾��û�б�����ʹ�ã�����б�ʹ���򷵻�true
        /// </summary>
        /// <param name="opBOMCode"></param>
        /// <param name="opBOMVersion"></param>
        /// <returns>���ز�����</returns>
        //		public bool IsOPBOMUsed(string opBOMCode,string opBOMVersion)
        //		{
        //			//update by crystal chu 2005/04/20 ����״̬������open
        //			string selectSql = "select "+DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO2Route))+" from tblmo2route where 1=1 and mocode in (select mo from tblmo where mostatus ='"+MOStatus.MOSTATUS_OPEN+"' {0}";
        //			string tmpString = string.Empty;
        //			if((opBOMCode != string.Empty)&&(opBOMCode.Trim() != string.Empty))
        //			{
        //				tmpString += " and opbomcode ='"+opBOMCode.Trim()+"'";
        //			}
        //			if((opBOMVersion != string.Empty)&&(opBOMVersion.Trim() != string.Empty))
        //			{
        //				tmpString += " and opbomver ='"+opBOMVersion.Trim()+"'";
        //			}
        //			if(this.DataProvider.CustomQuery(typeof(MOFacade),new SQLCondition(String.Format(selectSql,tmpString)))!=null)
        //			{
        //				return true;
        //			}
        //			return false;
        //		}
        #endregion

        public object[] GetOPBOMUsedMOs(string itemCode, string opBOMCode, string opBOMVersion, int orgID)
        {
            //string selectSql = "select "+DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO))+" from tblmo where  mostatus ='"+MOManufactureStatus.MOSTATUS_OPEN+"' and mocode in (select mocode from tblmo2route where 1=1 {0} ) and itemcode ='{1}'";
            //ֻ��mostatus_initial��mostatus_pending �Ĺ�����Ӧ��OPBOM�ſ����޸� InternalFeedBack 6��15 INT0018   modify by Simone
            string selectSql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO))
                + " from tblmo where  mostatus !='" + MOManufactureStatus.MOSTATUS_INITIAL
                + "' and mostatus !='" + MOManufactureStatus.MOSTATUS_PENDING
                + "' and mostatus !='" + MOManufactureStatus.MOSTATUS_CLOSE
                + "' and mocode in (select mocode from tblmo2route where 1=1 {0} ) and itemcode ='{1}'"
                + "  and orgid=" + orgID;
            string tmpString = string.Empty;
            if ((opBOMCode != string.Empty) && (opBOMCode.Trim() != string.Empty))
            {
                tmpString += " and opbomcode ='" + opBOMCode.Trim() + "'";
            }
            if ((opBOMVersion != string.Empty) && (opBOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + opBOMVersion.Trim() + "'";
            }
            return this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(String.Format(selectSql, tmpString, itemCode)));
        }



        /// <summary>
        /// ����һ��������Ӧ��TS ErrorCode;��
        /// </summary>
        /// <param name="moCode"></param>
        /// <param name="moCode"></param>
        /// <returns>Route[]</returns>
        public object[] QueryTSErrorCodeRouteByMO(string moCode, string routeCode)
        {
            string sql = "SELECT {0} FROM tblroute where routecode in (select routecode from TBLMODEL2ECG WHERE modelcode IN (SELECT modelcode FROM tblitem WHERE 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and itemcode IN (SELECT itemcode FROM tblmo WHERE mocode = '{1}'))) and routecode like '{2}%'";
            sql = string.Format(sql,
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(BenQGuru.eMES.Domain.BaseSetting.Route)),
                moCode,
                routeCode

                );
            return this.DataProvider.CustomQuery(
                typeof(BenQGuru.eMES.Domain.BaseSetting.Route),
                new SQLCondition(sql)
                );
        }

        public int QueryTSErrorCodeRouteCountByMO(string moCode, string routeCode)
        {
            string sql = "SELECT count(routecode) FROM tblroute where routecode in (select routecode from TBLMODEL2ECG WHERE modelcode IN (SELECT modelcode FROM tblitem WHERE 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and itemcode IN (SELECT itemcode FROM tblmo WHERE mocode = '{1}'))) and routecode like '{2}%'";
            sql = string.Format(sql,
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(BenQGuru.eMES.Domain.BaseSetting.Route)),
                moCode,
                routeCode
                );
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }





        /// <summary>
        /// ����һ��������Ӧ��TS ErrorCause;��
        /// </summary>
        /// <param name="moCode"></param>
        /// <returns>���� Route[]����</returns>
        public object[] QueryTSErrorCauseRouteByMO(string moCode, string routeCode)
        {
            string sql = "SELECT {0} FROM tblroute where routecode in (select routecode from TBLMODEL2ECSG WHERE modelcode IN (SELECT modelcode FROM tblitem WHERE 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and itemcode IN (SELECT itemcode FROM tblmo WHERE mocode = '{1}'))) and routecode like '{2}%'";
            sql = string.Format(sql,
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(BenQGuru.eMES.Domain.BaseSetting.Route)),
                moCode,
                routeCode
                );
            return this.DataProvider.CustomQuery(
                typeof(BenQGuru.eMES.Domain.BaseSetting.Route),
                new SQLCondition(sql)
                );
        }

        public int QueryTSErrorCauseRouteCountByMO(string moCode, string routeCode)
        {
            string sql = "SELECT count(routecode) FROM tblroute where routecode in (select routecode from TBLMODEL2ECSG WHERE modelcode IN (SELECT modelcode FROM tblitem WHERE 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and itemcode IN (SELECT itemcode FROM tblmo WHERE mocode = '{1}'))) and routecode like '{2}%'";
            sql = string.Format(sql,
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(BenQGuru.eMES.Domain.BaseSetting.Route)),
                moCode,
                routeCode
                );
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }


        /// <summary>
        /// ** ������:QueryNormalRouteByMO
        /// ** ��������:���ع�����Ӧ��normal;��
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��:   vizo
        /// ** �� ��:   2005-03-23
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾

        /// 
        /// </summary>
        /// <param name="moCode">����Code</param>
        /// <param name="routeCode">;��Code</param>
        /// <returns>����Route[]����</returns>
        public object[] QueryNormalRouteByMO(string moCode, string routeCode)
        {
            string sql = "SELECT {0} FROM tblroute where routecode in (SELECT routecode FROM tblitem2route WHERE itemcode IN (SELECT itemcode FROM tblmo WHERE mocode = '{1}') " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ") and routecode like '{2}%'";
            sql = string.Format(sql,
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(BenQGuru.eMES.Domain.BaseSetting.Route)),
                moCode,
                routeCode
                );
            return this.DataProvider.CustomQuery(
                typeof(BenQGuru.eMES.Domain.BaseSetting.Route),
                new SQLCondition(sql)
                );
        }

        /// <summary>
        /// added by jessie lee, 2006-3-22, ��ȥ״̬Ϊ����ʹ�á���;��
        /// </summary>
        /// <param name="moCode"></param>
        /// <returns></returns>
        public object[] QueryNormalRouteByMOEnabled(string moCode, string routeCode)
        {
            string sql = "SELECT {0} FROM tblroute where routecode in (SELECT routecode FROM tblitem2route WHERE itemcode IN (SELECT itemcode FROM tblmo WHERE mocode = '{1}') " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ") and routecode like '{2}%' and enabled = '{3}'";
            sql = string.Format(sql,
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(BenQGuru.eMES.Domain.BaseSetting.Route)),
                moCode,
                routeCode,
                FormatHelper.TRUE_STRING
                );
            return this.DataProvider.CustomQuery(
                typeof(BenQGuru.eMES.Domain.BaseSetting.Route),
                new SQLCondition(sql)
                );
        }

        public object[] GetAllMORoutes(string moCode)
        {
            return this.DataProvider.CustomQuery(typeof(Route), new SQLCondition(string.Format("select {0} from  tblroute where routecode in (select routecode from tblmo2route  where mocode = '{1}') order by routecode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Route)), moCode)));
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
                return this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(string.Format("select {0} from tblmo where motype in(" + tmpReworkType + ")  order by mocode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)))));
            }
            else
            {
                return null;
            }

        }

        public int QueryNormalRouteCountByMO(string moCode, string routeCode)
        {
            string sql = "SELECT count(*) FROM tblroute where routecode in (SELECT routecode FROM tblitem2route WHERE itemcode IN (SELECT itemcode FROM tblmo WHERE mocode = '{1}') " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ") and routecode like '{2}%'";
            sql = string.Format(sql,
                DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(BenQGuru.eMES.Domain.BaseSetting.Route)),
                moCode,
                routeCode
                );
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        /// <summary>
        /// sammer kong 2005/06/02
        /// </summary>
        /// <param name="moCode"></param>
        /// <returns></returns>
        public object[] GetAllOperationsByMoCode(string moCode)
        {
            if (moCode == "" || moCode == null)
            {
                return null;
            }
            else
            {
                return this.DataProvider.CustomQuery(
                    typeof(Route2Operation),
                    new SQLCondition(
                    string.Format("select {0} from TBLROUTE2OP where " +
                    " routecode in ( select routecode from TBLMO2ROUTE where mocode = '{1}')",
                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(Route2Operation)),
                    moCode.ToUpper())));
            }
        }


        /// <summary>
        /// ** ������: GetAllMO
        /// ** ��������:�������й���
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��:   vizo
        /// ** �� ��:   2005-03-31
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// </summary>
        /// <returns>MO[]����</returns>
        public object[] GetAllMO()
        {
            return this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(string.Format("select {0} from TBLMO where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " order by MOCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)))));
        }

        /// <summary>
        /// ** ������: GetMOByStatus
        /// ** ��������:����״̬���ع���
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��:   Simone Xu
        /// ** �� ��:   2005-06-24
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// </summary>
        /// <returns>MO[]����</returns>
        public object[] GetMOByStatus(string[] StatusList)
        {
            StringBuilder sqlbuilder = new StringBuilder();
            sqlbuilder.Append(string.Format("select {0} from TBLMO where 1=1 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO))));
            sqlbuilder.Append(" AND MOSTATUS IN ('' ");
            foreach (string status in StatusList)
            {
                sqlbuilder.Append(string.Format(",'{0}'", status));
            }
            sqlbuilder.Append(")");
            sqlbuilder.Append(GlobalVariables.CurrentOrganizations.GetSQLCondition());
            sqlbuilder.Append(" order by MOCODE ");
            return this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(sqlbuilder.ToString()));
        }

        /// <summary>
        /// ** ������: GetMoByItemCode
        /// ** ��������:���ݲ�Ʒ��״̬״̬���ع���
        /// ** ȫ�ֱ���:
        /// ** ����ģ��:
        /// ** �� ��:   Simone Xu
        /// ** �� ��:   2005-06-24
        /// ** �� ��:
        /// ** �� ��:
        /// ** �汾
        /// </summary>
        /// <returns>MO[]����</returns>
        public object[] GetMoByItemCode(string itemCoce, string[] StatusList)
        {

            StringBuilder sqlbuilder = new StringBuilder();
            sqlbuilder.Append(string.Format("select {0} from TBLMO where ITEMCODE='{1}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO)), itemCoce));
            sqlbuilder.Append(" AND MOSTATUS IN ('' ");
            foreach (string status in StatusList)
            {
                sqlbuilder.Append(string.Format(",'{0}'", status));
            }
            sqlbuilder.Append(")");
            sqlbuilder.Append(GlobalVariables.CurrentOrganizations.GetSQLCondition());
            sqlbuilder.Append(" order by MOCODE ");
            return this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(sqlbuilder.ToString()));
        }


        /// <summary>
        /// no need
        /// </summary>
        /// <returns></returns>
        public string[] GetMOStatuses()
        {
            return new string[]{
								   MOManufactureStatus.MOSTATUS_INITIAL,
								   MOManufactureStatus.MOSTATUS_CLOSE,
								   MOManufactureStatus.MOSTATUS_OPEN,
								   MOManufactureStatus.MOSTATUS_PENDING,
								   MOManufactureStatus.MOSTATUS_RELEASE
							   };
        }

        //ͨ������Code��ȡ��Ӧ��MOBOM(������׼BOM)
        public object[] GetMOBOM(string moCode)
        {
            if (moCode == "" || moCode == null)
            {
                return null;
            }
            else
            {
                //				string sql = string.Format(@"SELECT tblmobom.mocode, tblmobom.itemcode, tblmobom.seq, tblmobom.mobomitemuom, tblmobom.mobitemcontype,
                //						    tblmobom.mobitemqty, tblmobom.mobitemefftime, tblmobom.eattribute1, tblmobom.mobiteminvtime, tblmobom.mobitemver,
                //						    tblmobom.muser, tblmobom.mobitemdesc, tblmobom.mtime, tblmobom.mobsitemcode, tblmobom.mdate, tblmobom.mobitemlocation,
                //							tblmobom.mobitemstatus, tblmobom.mobitemname, tblmobom.mobiteminvdate, tblmobom.mobitemcode,
                //							tblmobom.mobitemeffdate, tblmobom.mobitemecn from TBLMOBOM where 1=1 AND mocode = '{0}' ",moCode.ToUpper());
                //opcode select
                string sql = string.Format(@"SELECT mocode, itemcode, seq,mobomitemuom, mobitemcontype, mobitemqty,mobitemefftime, eattribute1, mobiteminvtime,mobitemver, muser, mobitemdesc,mtime, mobsitemcode, mdate,mobitemlocation, mobitemstatus, mobitemname,mobiteminvdate, mobitemcode, mobitemeffdate,mobitemecn, opcode from TBLMOBOM where 1=1 AND mocode = '{0}' ", moCode.ToUpper());
                return this.DataProvider.CustomQuery(
                    typeof(MOBOM),
                    new SQLCondition(sql));
            }

        }

        //ͨ������Code(һ������)��OPCode��ȡ��Ӧ��MOBOM(������׼BOM) ����Ϊ0(��������Ϊ0)�Ĳ���ȡ
        public object[] GetMOBOM(string moCode, string opCode)
        {
            if (moCode == "" || moCode == null)
            {
                return null;
            }
            else
            {
                string opCodeCondition = string.Empty;	//OPCode��ѯ����,֧�ֶ��opcode��ѯ,�Զ��Ÿ���,����ǰ��Ҫ���ո�
                if (opCode != null && opCode.Trim() != string.Empty)
                {
                    opCodeCondition = string.Format(" AND OPCODE in ({0})  ", FormatHelper.ProcessQueryValues(opCode.Trim())); ;
                }
                string sql = string.Format(@"SELECT mocode, itemcode, seq,
													mobomitemuom, mobitemcontype, mobitemqty,
													mobitemefftime, eattribute1, mobiteminvtime,
													mobitemver, muser, mobitemdesc,
													mtime, mobsitemcode, mdate,
													mobitemlocation, mobitemstatus, mobitemname,
													mobiteminvdate, mobitemcode, mobitemeffdate,
													mobitemecn, opcode from TBLMOBOM where MOBITEMQTY >0 and mocode in ( {0} ) {1} ", FormatHelper.ProcessQueryValues(moCode.ToUpper()), opCodeCondition);
                return this.DataProvider.CustomQuery(
                typeof(MOBOM),
                new SQLCondition(sql));
            }

        }

        #endregion


        #region RCardLink
        /// <summary>
        /// 
        /// </summary>
        public MO2RCARDLINK CreateNewMO2RCardLink()
        {
            return new MO2RCARDLINK();
        }

        public void AddMO2RCardLink(MO2RCARDLINK mO2RCardLink)
        {
            this._helper.AddDomainObject(mO2RCardLink);
        }

        public void UpdateMO2RCardLink(MO2RCARDLINK mO2RCardLink)
        {
            this._helper.UpdateDomainObject(mO2RCardLink);
        }

        public void DeleteMO2RCardLink(MO2RCARDLINK mO2RCardLink)
        {
            this._helper.DeleteDomainObject(mO2RCardLink);
        }

        public void DeleteMO2RCardLink(MO2RCARDLINK[] mO2RCardLink)
        {
            this._helper.DeleteDomainObject(mO2RCardLink);
        }

        public object GetMO2RCardLink(string pK)
        {
            return this.DataProvider.CustomSearch(typeof(MO2RCARDLINK), new object[] { pK });
        }

        public object[] GetMO2RCardLinkByMoCode(string moCode)
        {
            object[] obj = null;
            string sqlQueryString = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO2RCARDLINK)) + " from TBLMO2RCARDLINK where  moCode='" + moCode + "' ";

            sqlQueryString += " order by RCard";
            obj = this.DataProvider.CustomQuery(typeof(MO2RCARDLINK), new SQLCondition(sqlQueryString));

            return obj;
        }

        public object GetMO2RCardLinkByRcard(string rcard, string moCode)
        {
            object[] obj = this.DataProvider.CustomQuery(typeof(MO2RCARDLINK), new SQLCondition("select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO2RCARDLINK))
                + " from TBLMO2RCARDLINK where rcard='" + rcard + "' and mocode='" + moCode + "'"));

            object objReturn = null;

            if (obj != null && obj.Length > 0)
            {
                objReturn = obj[0];
            }

            return objReturn;
        }

        public object[] QueryMO2RCardLink(string rcard, string moCode)
        {

            string sql = "SELECT * FROM TBLMO2RCARDLINK WHERE 1=1 ";
            if (rcard != "")
            {
                sql += " AND RCARD like '%" + FormatHelper.CleanString(rcard) + "%'";
            }
            if (moCode != "")
            {
                sql += " AND mocode='" + FormatHelper.CleanString(moCode).ToUpper() + "'";
            }

            sql += " ORDER BY RCARD ";

            object[] obj = this.DataProvider.CustomQuery(typeof(MO2RCARDLINK), new SQLCondition(sql));

            return obj;

        }

        public object[] GetMO2RCardLinkByMoCode(string moCode, string beginRcard, string endRcard, string printTimes)
        {
            object[] obj = null;
            string sqlQueryString = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO2RCARDLINK)) + " from TBLMO2RCARDLINK where  moCode='" + moCode + "' ";

            if (beginRcard != "")
            {
                sqlQueryString += " and RCard >= '" + beginRcard + "'";
            }
            if (endRcard != "")
            {
                sqlQueryString += " and RCard <= '" + endRcard + "'";
            }
            if (printTimes != "")
            {
                sqlQueryString += " and PrintTimes = '" + printTimes + "' ";
            }

            sqlQueryString += " order by RCard";
            obj = this.DataProvider.CustomQuery(typeof(MO2RCARDLINK), new SQLCondition(sqlQueryString));

            return obj;
        }

        public string GetMmachineType(string moCode)
        {

            //string sql = "SELECT mr.MMACHINETYPE  FROM TBLMO MO ,  TBLMATERIAL mr WHERE MO.ITEMCODE = mr.MCODE AND MO.MOCODE = '" + moCode + "'";
            //sql += " and mo.orgid = mr.orgid ";
            //object[] obj = this.DataProvider.CustomQuery(typeof(Domain.MOModel.Material), new SQLCondition(sql));
            //if (obj != null && obj.Length > 0)
            //{
            //    Domain.MOModel.Material material = (Domain.MOModel.Material)obj[0];
            //    return material.MaterialMachineType;

            //}

            return "";


        }

        public object GetMaterial(string moCode, decimal orgId)
        {
            return this._domainDataProvider.CustomSearch(typeof(MO2SAP), new object[] { moCode, orgId });
        }

        public object GetMaterial1(string moCode, decimal orgId)
        {
            return this._domainDataProvider.CustomSearch(typeof(BenQGuru.eMES.Domain.MOModel.Material), new object[] { moCode, orgId });
        }
        /// <summary>
        /// ** ��������:	������е�OffMoCard
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-12-15 18:22:52
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>OffMoCard���ܼ�¼��</returns>
        public object[] GetAllMO2RCardLink()
        {
            return this.DataProvider.CustomQuery(typeof(MO2RCARDLINK), new SQLCondition(string.Format("select {0} from TBLMO2RCARDLINK order by PK", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO2RCARDLINK)))));
        }
        #endregion

        #region MOViewField
        /// <summary>
        /// ����
        /// </summary>
        public MOViewField CreateNewMOViewField()
        {
            return new MOViewField();
        }

        public void AddMOViewField(MOViewField mOViewField)
        {
            this._helper.AddDomainObject(mOViewField);
        }

        public void UpdateMOViewField(MOViewField mOViewField)
        {
            this._helper.UpdateDomainObject(mOViewField);
        }

        public void DeleteMOViewField(MOViewField mOViewField)
        {
            this._helper.DeleteDomainObject(mOViewField);
        }

        public void DeleteMOViewField(MOViewField[] mOViewField)
        {
            this._helper.DeleteDomainObject(mOViewField);
        }

        public object GetMOViewField(string userCode, decimal sequence)
        {
            return this.DataProvider.CustomSearch(typeof(MOViewField), new object[] { userCode, sequence });
        }

        /// <summary>
        /// ** ��������:	��ѯMOViewField��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-12-9 9:19:26
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userCode">UserCode��ģ����ѯ</param>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <returns> MOViewField���ܼ�¼��</returns>
        public int QueryMOViewFieldCount(string userCode, decimal sequence)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMOVIEWFIELD where 1=1 and USERCODE like '{0}%'  and SEQ like '{1}%' ", userCode, sequence)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯMOViewField
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-12-9 9:19:26
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="userCode">UserCode��ģ����ѯ</param>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> MOViewField����</returns>
        public object[] QueryMOViewField(string userCode, decimal sequence, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(MOViewField), new PagerCondition(string.Format("select {0} from TBLMOVIEWFIELD where 1=1 and USERCODE like '{1}%'  and SEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MOViewField)), userCode, sequence), "USERCODE,SEQ", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�MOViewField
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-12-9 9:19:26
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>MOViewField���ܼ�¼��</returns>
        public object[] GetAllMOViewField()
        {
            return this.DataProvider.CustomQuery(typeof(MOViewField), new SQLCondition(string.Format("select {0} from TBLMOVIEWFIELD order by USERCODE,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MOViewField)))));
        }

        public object[] QueryMOViewFieldByUserCode(string userCode)
        {
            string strSql = "SELECT * FROM tblMOViewField WHERE UserCode='" + userCode + "' ORDER BY SEQ ";
            return this.DataProvider.CustomQuery(typeof(MOViewField), new SQLCondition(strSql));
        }
        public object[] QueryMOViewFieldDefault()
        {
            string strSql = "SELECT * FROM tblMOViewField WHERE UserCode='MO_FIELD_LIST_SYSTEM_DEFAULT' ORDER BY SEQ ";
            return this.DataProvider.CustomQuery(typeof(MOViewField), new SQLCondition(strSql));
        }

        public void SaveMOViewField(string userCode, string moFieldList)
        {
            this.DataProvider.BeginTransaction();
            try
            {
                string strSql = "DELETE FROM tblMOViewField WHERE UserCode='" + userCode + "' ";
                this.DataProvider.CustomExecute(new SQLCondition(strSql));
                object[] objs = this.QueryMOViewFieldDefault();
                Hashtable htDesc = new Hashtable();
                if (objs != null)
                {
                    for (int i = 0; i < objs.Length; i++)
                    {
                        MOViewField field = (MOViewField)objs[i];
                        htDesc.Add(field.FieldName, field.Description);
                    }
                }
                string[] moField = moFieldList.Split(';');
                for (int i = 0; i < moField.Length; i++)
                {
                    if (moField[i].Trim() != string.Empty)
                    {
                        MOViewField field = new MOViewField();
                        field.UserCode = userCode;
                        field.Sequence = i;
                        field.FieldName = moField[i];
                        field.Description = htDesc[field.FieldName].ToString();
                        this.AddMOViewField(field);
                    }
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                throw ex;
            }
        }

        #endregion

        #region DefaultItem2Route
        /// <summary>
        /// 
        /// </summary>
        public DefaultItem2Route CreateNewDefaultItem2Route()
        {
            return new DefaultItem2Route();
        }

        public void AddDefaultItem2Route(DefaultItem2Route defaultItem2Route)
        {
            this._helper.AddDomainObject(defaultItem2Route);
        }

        public void UpdateDefaultItem2Route(DefaultItem2Route defaultItem2Route)
        {
            this._helper.UpdateDomainObject(defaultItem2Route);
        }

        public void DeleteDefaultItem2Route(DefaultItem2Route defaultItem2Route)
        {
            this._helper.DeleteDomainObject(defaultItem2Route);
        }

        public object GetDefaultItem2Route(string itemCode)
        {
            return this.DataProvider.CustomSearch(typeof(DefaultItem2Route), new object[] { itemCode });
        }

        /// <summary>
        /// ** ��������:	��ѯDefaultItem2Route��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-9-25 10:18:25
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="itemCode">ItemCode��ģ����ѯ</param>
        /// <param name="routeCode">RouteCode��ģ����ѯ</param>
        /// <returns> DefaultItem2Route���ܼ�¼��</returns>
        public int QueryDefaultItem2RouteCount(string itemCode, string routeCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TblDefaultItem2Route where 1=1 and ItemCode like '{0}%'  and RouteCode like '{1}%' ", itemCode, routeCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯDefaultItem2Route
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-9-25 10:18:25
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="itemCode">ItemCode��ģ����ѯ</param>
        /// <param name="routeCode">RouteCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> DefaultItem2Route����</returns>
        public object[] QueryDefaultItem2Route(string itemCode, string routeCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(DefaultItem2Route), new PagerCondition(string.Format("select {0} from TblDefaultItem2Route where 1=1 and ItemCode like '{1}%'  and RouteCode like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(DefaultItem2Route)), itemCode, routeCode), "ItemCode,RouteCode", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯDefaultItem2Route
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-9-25 10:18:25
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="itemCode">ItemCode��ģ����ѯ</param>
        /// <param name="routeCode">RouteCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> DefaultItem2Route����</returns>
        public object[] QueryUnSelectDefaultItem2Route(string itemCode, string routeCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(DefaultItem2Route), new PagerCondition(string.Format("select {0} from TblDefaultItem2Route where 1=1 and ItemCode like '{1}%'  and RouteCode like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(DefaultItem2Route)), itemCode, routeCode), "ItemCode,RouteCode", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�DefaultItem2Route
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-9-25 10:18:25
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>DefaultItem2Route���ܼ�¼��</returns>
        public object[] GetAllDefaultItem2Route()
        {
            return this.DataProvider.CustomQuery(typeof(DefaultItem2Route), new SQLCondition(string.Format("select {0} from TblDefaultItem2Route order by ItemCode,RouteCode", DomainObjectUtility.GetDomainObjectFieldsString(typeof(DefaultItem2Route)))));
        }

        public object[] GetAllMOBOMVersion()
        {
            string sql = string.Format("select distinct mobom from tblmo where mobom is not null order by mobom");

            return this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(sql));
        }

        #endregion

        #region FirstCheckByMO
        /// <summary>
        /// 
        /// </summary>
        public FirstCheckByMO CreateNewFirstCheckByMO()
        {
            return new FirstCheckByMO();
        }

        public void AddFirstCheckByMO(FirstCheckByMO firstCheckByMO)
        {
            this._helper.AddDomainObject(firstCheckByMO);
        }

        public void UpdateFirstCheckByMO(FirstCheckByMO firstCheckByMO)
        {
            this._helper.UpdateDomainObject(firstCheckByMO);
        }

        public void DeleteFirstCheckByMO(FirstCheckByMO firstCheckByMO)
        {
            this._helper.DeleteDomainObject(firstCheckByMO);
        }

        public void DeleteFirstCheckByMO(FirstCheckByMO[] firstCheckByMO)
        {
            this._helper.DeleteDomainObject(firstCheckByMO);
        }

        public object GetFirstCheckByMO(string moCode)
        {
            return this.DataProvider.CustomSearch(typeof(FirstCheckByMO), new object[] { moCode });
        }

        public object[] QueryFirstCheckByMO(string moCode, int checkDate, int inclusive, int exclusive)
        {
            string sql = "select A.MOCODE,B.ITEMCODE,C.ITEMNAME,A.CHECKDATE,A.CHECKRESULT,A.MEMO,A.MUSER,A.MDATE,A.MTIME";
            sql += " from TBLFIRSTCHECKBYMO A left join TBLMO B on A.MOCODE = B.MOCODE";
            sql += " left join TBLITEM C on B.ITEMCODE = C.ITEMCODE";
            sql += " AND b.orgid=c.orgid";
            sql += " where 1=1";
            if (!String.IsNullOrEmpty(moCode))
            {
                sql += string.Format(" and A.MOCODE in ({0})", FormatHelper.ProcessQueryValues(moCode));
            }
            if (checkDate > 0)
            {
                sql += " and A.CHECKDATE = '" + checkDate + "'";
            }

            return this.DataProvider.CustomQuery(typeof(FirstCheckByMOForQuery),
                new PagerCondition(sql, "A.CHECKDATE,A.MOCODE", inclusive, exclusive));
        }

        public int QueryFirstCheckByMOCount(string moCode, int checkDate)
        {
            string sql = "select count(*)";
            sql += " from TBLFIRSTCHECKBYMO A left join TBLMO B on A.MOCODE = B.MOCODE";
            sql += " left join TBLITEM C on B.ITEMCODE = C.ITEMCODE";
            sql += " AND b.orgid=c.orgid";
            sql += " where 1=1";
            if (!String.IsNullOrEmpty(moCode))
            {
                sql += string.Format(" and A.MOCODE in ({0})", FormatHelper.ProcessQueryValues(moCode));
            }
            if (checkDate > 0)
            {
                sql += " and A.CHECKDATE = '" + checkDate + "'";
            }
            return this.DataProvider.GetCount(
                new SQLCondition(sql));
        }

        public object GetFirstCheckByMO(string moCode, int checkDate)
        {
            object[] obj = this.DataProvider.CustomQuery(typeof(FirstCheckByMO), new SQLCondition("select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(FirstCheckByMO))
                + " from TBLFIRSTCHECKBYMO where moCode='" + moCode + "' and checkdate=" + checkDate + ""));

            object objReturn = null;

            if (obj != null && obj.Length > 0)
            {
                objReturn = obj[0];
            }

            return objReturn;
        }
        #endregion

        #region ����bom�ȶ� (MOBOM���е�������OPBOM�ȶ�) ������������� : �ȶԳɹ� , ֻ�ڹ�������BOM ��(��OPBOM) ,ֻ�ڹ�����׼bom��(��MOBOM�е�����)

        public Hashtable CompareMOBOM(object[] moBOMs, string itemCode, string mocode, string routeCode)
        {
            //getOPBOMCode by routeCode
            string errorMSG = string.Empty;
            //			if(moBOMs == null)
            //			{
            //				ExceptionManager.Raise(this.GetType().BaseType,"$Error_NullMOBOMS");
            //			}
            object[] moRouteObjs = QueryMORoutes(mocode, routeCode);
            if (moRouteObjs == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MORouteNOExist");
            }

            object mo = GetMO(mocode);

            OPBOMFacade opBOMFacade = new OPBOMFacade(this.DataProvider);
            //OPBOM ����Ҫ�ȶԵ����� ()
            object[] opbomObjs = opBOMFacade.QueryOPBOMDetail(itemCode, string.Empty, ((MO2Route)moRouteObjs[0]).OPBOMCode, ((MO2Route)moRouteObjs[0]).OPBOMVersion, routeCode, string.Empty, int.MinValue, int.MaxValue, ((MO)mo).OrganizationID);
            if (opbomObjs == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_OPBOMNOExist", String.Format("[$ItemCode='{0}']", itemCode));
            }

            bool iflag = false;
            bool iCheckPass = true;
            Decimal iOPBOMItemQty = 0;

            //��Ҫ�ȶԵ�
            //�ӽ����Ϻ�
            //�ӽ�������

            Hashtable returnHT = new Hashtable();
            ArrayList SucessResult = new ArrayList();				//�ȶԳɹ�
            ArrayList InMORouteResult = new ArrayList();			//ֻ�ڹ�������BOM ��
            ArrayList InMOStandardBOMResult = new ArrayList();		//ֻ�ڹ�����׼bom��

            // �ȶԳɹ���
            // ֻ�ڹ�������BOM ��
            // ֻ�ڹ�����׼bom��

            //�Թ�����׼BOMΪ��׼�� �ȶԽ��Ϊ�ȶԳɹ�������ֻ�ڱ�׼bom�У���������֣��ӽ��ϲ����� �� �ӽ��ϴ��� �������������ԣ�
            if (moBOMs != null && opbomObjs != null)
                for (int i = 0; i < moBOMs.Length; i++)
                {
                    //���opbom�ж�Ӧ����Ʒ
                    iflag = false;
                    iOPBOMItemQty = 0;
                    for (int j = 0; j < opbomObjs.Length; j++)
                    {
                        if ((((MOBOM)moBOMs[i]).MOBOMItemCode.ToUpper() == ((OPBOMDetail)opbomObjs[j]).OPBOMItemCode.ToUpper()))
                        {
                            //�ӽ��ϴ���
                            iflag = true;
                            iOPBOMItemQty += ((OPBOMDetail)opbomObjs[j]).OPBOMItemQty;
                        }
                    }
                    //modified by jessie lee,2005/11/22
                    //�ȶԳɹ����ӽ����Ϻź���Ӧ�ĵ���������һ��
                    //����������һ�£��ӽ����Ϻ�һ�£���Ӧ�ĵ���������һ��
                    if (iflag)//�ӽ��ϴ���
                    {
                        ((MOBOM)moBOMs[i]).OPBOMItemQty = iOPBOMItemQty;
                        if (((MOBOM)moBOMs[i]).MOBOMItemQty != iOPBOMItemQty)
                        {
                            iCheckPass = false;
                            errorMSG = "$Error_MOBOMItemQty_NotEqualOPBOMQty";
                            errorMSG = "����������һ��";
                            ((MOBOM)moBOMs[i]).MOBOMException = errorMSG;
                            //InMOStandardBOMResult.Add(moBOMs[i]);
                            //�ӽ��ϴ��� ��������������, ��ֻ�ڹ�����׼bom��
                        }
                        else
                        {
                            errorMSG = "$MSG_MOBOMItemQty_EqualOPBOMQty";
                            errorMSG = "�ȶԳɹ�";
                            ((MOBOM)moBOMs[i]).MOBOMException = errorMSG;
                            //SucessResult.Add(moBOMs[i]);
                            //�ӽ��ϴ��ڣ�������ȷ���ȶԳɹ�
                        }
                        SucessResult.Add(moBOMs[i]);
                    }
                    else
                    {
                        //�ӽ��ϲ����ڣ� ��ֻ�ڹ�����׼bom��
                        iCheckPass = false;
                        errorMSG = "$Error_MOBOMItem_NotExistOPBOM";
                        errorMSG = "ֻ�����ڹ��������嵥��";
                        ((MOBOM)moBOMs[i]).MOBOMException = errorMSG;
                        InMOStandardBOMResult.Add(moBOMs[i]);
                    }
                    ((MOBOM)moBOMs[i]).MOBOMException = errorMSG;
                }

            //��opbomΪ��׼�� �ȶԽ��Ϊֻ�ڹ�������BOM ��
            if (opbomObjs != null)
                for (int i = 0; i < opbomObjs.Length; i++)
                {
                    //���opbom�ж�Ӧ����Ʒ
                    iflag = false;
                    if (moBOMs != null)
                    {
                        for (int j = 0; j < moBOMs.Length; j++)
                        {
                            if ((((MOBOM)moBOMs[j]).MOBOMItemCode.ToUpper() == ((OPBOMDetail)opbomObjs[i]).OPBOMItemCode.ToUpper()))
                            {
                                iflag = true;
                                break;
                            }
                        }

                        if (!iflag)
                        {
                            //�ӽ���ֻ�ڹ�������BOM �� OPBOM��
                            errorMSG = "$Error_MOBOMItem_NotExistMOBOM";
                            errorMSG = "ֻ�����ڹ�������BOM��";
                            InMORouteResult.Add((OPBOMDetail)opbomObjs[i]);

                        }

                    }
                    else
                    {
                        errorMSG = "ֻ�����ڹ�������BOM(OPBOM) ��";
                        //���������׼bom������,�ӽ���ֻ�ڹ�������BOM ��
                        InMORouteResult.Add((OPBOMDetail)opbomObjs[i]);
                    }
                }

            returnHT["SucessResult"] = SucessResult;
            returnHT["InMORouteResult"] = InMORouteResult;
            returnHT["InMOStandardBOMResult"] = InMOStandardBOMResult;

            return returnHT;
        }

        #endregion

        #region ����BOM�ȶ�OPBOM
        public MOBOM[] CompareMOBOMOPBOM(MOBOM[] moBOMs, string itemCode, string mocode, string routeCode)
        {
            //getOPBOMCode by routeCode
            string errorMSG = string.Empty;
            if (moBOMs == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_NullMOBOMS");
            }
            object[] moRouteObjs = QueryMORoutes(mocode, routeCode);
            if (moRouteObjs == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MORouteNOExist");
            }

            object mo = GetMO(mocode);

            OPBOMFacade opBOMFacade = new OPBOMFacade(this.DataProvider);
            object[] opbomObjs = opBOMFacade.QueryOPBOMDetail(itemCode, string.Empty, ((MO2Route)moRouteObjs[0]).OPBOMCode, ((MO2Route)moRouteObjs[0]).OPBOMVersion, routeCode, string.Empty, int.MinValue, int.MaxValue, ((MO)mo).OrganizationID);
            if (opbomObjs == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_OPBOMNOExist", String.Format("[$ItemCode='{0}']", itemCode));
            }

            bool iflag = false;
            bool iCheckPass = true;
            Decimal iOPBOMItemQty = 0;

            for (int i = 0; i < moBOMs.Length; i++)
            {
                //���opbom�ж�Ӧ����Ʒ
                iflag = false;
                iOPBOMItemQty = 0;
                for (int j = 0; j < opbomObjs.Length; j++)
                {
                    if ((moBOMs[i].MOBOMItemCode.ToUpper() == ((OPBOMDetail)opbomObjs[j]).OPBOMItemCode.ToUpper()))
                    {
                        iflag = true;
                        iOPBOMItemQty += ((OPBOMDetail)opbomObjs[j]).OPBOMItemQty;
                    }
                }
                if (iflag)
                {
                    moBOMs[i].OPBOMItemQty = iOPBOMItemQty;
                    if (moBOMs[i].MOBOMItemQty != iOPBOMItemQty)
                    {
                        iCheckPass = false;
                        errorMSG = "$Error_MOBOMItemQty_NotEqualOPBOMQty";
                    }
                    else
                    {
                        errorMSG = "$MSG_MOBOMItemQty_EqualOPBOMQty";
                    }
                }
                else
                {
                    iCheckPass = false;
                    errorMSG = "$Error_MOBOMItem_NotExistOPBOM";
                }
                moBOMs[i].MOBOMException = errorMSG;
            }

            if (!iCheckPass)
            {
                MO currentMO = (MO)GetMO(mocode);
                currentMO.IsBOMPass = BenQGuru.eMES.Web.Helper.IsPass.ISPASS_NOPASS.ToString();
                UpdateMO(currentMO, false);
            }
            else
            {
                MO currentMO = (MO)GetMO(mocode);
                currentMO.IsBOMPass = BenQGuru.eMES.Web.Helper.IsPass.ISPASS_PASS.ToString();
                UpdateMO(currentMO, false);
            }
            return moBOMs;
        }

        public MOBOM CreateNewMOBOM()
        {
            return new MOBOM();
        }

        public void AddMOBOM(MOBOM moBOM)
        {
            this._helper.AddDomainObject(moBOM);
        }

        public void DeleteMOBOMByMOCode(string moCode)
        {
            string sql = "DELETE FROM tblMOBOM WHERE mocode='" + moCode.ToUpper() + "'";
            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }

        public decimal GetMOBOMMaxSequence(string moCode)
        {
            string sql = "SELECT NVL(MAX(seq), 0) AS seq FROM tblmobom WHERE mocode='" + moCode.ToUpper() + "'";
            object[] moboms = this.DataProvider.CustomQuery(typeof(MOBOM), new SQLCondition(sql));
            MOBOM moBOM = moboms[0] as MOBOM;
            if (moBOM == null || moBOM.Sequence == 0)
            {
                return 1;
            }
            else
            {
                return moBOM.Sequence + 1;
            }
        }

        //add By Jarvis For DeductQty 20120315
        public object[] QueryMoBom(string itemCode, string mcode, string moCode)
        {
            string sql = string.Format(@"SELECT * from TBLMOBOM where MOBITEMQTY >0");

            if (itemCode.Trim() != "")
            {
                sql += " And itemcode='" + itemCode + "'";
            }
            if (mcode.Trim() != "")
            {
                sql += " And MOBITEMCODE='" + mcode + "'";
            }
            if (moCode.Trim() != "")
            {
                sql += " And mocode='" + moCode + "'";
            }
            return this.DataProvider.CustomQuery(typeof(MOBOM), new SQLCondition(sql));

        }

        #endregion

        #region �ص�ʱ���Simulation (tblsimulation ,tblsimulationreport) , �ص��ɹ���ִ�д˷���

        private void ClearSimulation(MO mo)
        {
            this.Deletetblsimulation(mo.MOCode);			//���tblsimulation
            //this.Deletetblsimulationreport(mo.MOCode);	//���tblsimulationreport	,��ʱ����,���ڱ����ѯ
            this.Deletetblmobom(mo.MOCode);					//���tblmobom
        }

        #region ɾ����ر�

        //���tblsimulation
        private void Deletetblsimulation(string moCode)
        {
            string sql = string.Format(" delete from tblsimulation where mocode ='{0}' ", moCode);
            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }

        //���tblsimulationreport
        private void Deletetblsimulationreport(string moCode)
        {

            string sql = string.Format(" delete from tblsimulationreport where mocode ='{0}' ", moCode);
            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }

        //���tblmobom
        private void Deletetblmobom(string moCode)
        {

            string sql = string.Format(" delete from tblmobom where mocode ='{0}' ", moCode);
            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }

        //���tblpackingchk
        private void Deletetblpackingchk()
        {
            string sql = "DELETE FROM tblpackingchk where not exists(select 1 from tblsimulation where tblpackingchk.rcard = tblsimulation.rcard ) ";
            this.DataProvider.CustomExecute(new SQLCondition(sql));
        }


        #endregion

        #endregion

        #region for cs Datacollect
        /// <summary>
        /// ������Ӧ�ĵ�ǰ;�̺͸ù�����Ӧ���ع�������ά�������е�;��
        /// </summary>
        /// <param name="moCode"></param>
        /// <returns></returns>
        public object[] GetNormalAndReworkRouteByMOCode(string moCode)
        {
            string selectSql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(Route)) + " from tblroute where routecode in("
                // ����;�̺��ؿ������ع��Ĺ�����Ӧ��;��
                             + " select routecode from tblmo2route where mocode =$moCode1"

                        // ������Ӧ���ع�����:�����ع��Ĺ�����Ӧ��RunningCard�����ع����󵥵ķ�Χ
                             + " union all select NEWROUTECODE from tblreworksheet where reworkcode in "
                             + "(select distinct reworkcode from tblreworkrange where rcard in"
                             + "(select MORCARDSTART from tblmorcard where mocode=$moCode2 )))";
            return this.DataProvider.CustomQuery(typeof(Route), new SQLParamCondition(selectSql, new SQLParameter[] { new SQLParameter("moCode1", typeof(string), moCode.ToUpper()), new SQLParameter("moCode2", typeof(string), moCode.ToUpper()) }));
        }

        //Laws Lu,2005/10/31���޸�	ͳһ������������ı�׼
        //Laws Lu��2005/11/18���޸�	��������Ϊ0���׳��쳣
        public void UpdateMOOutPutQty(string moCode/*,int outPutQty*/, int qty)
        {

            string updateSql = "update TBLMO "
                    + " set MOACTQTY=MOACTQTY+" + qty.ToString()
                    + " where MOCODE='" + moCode.Trim() + "'";

            int iReturn = 0;
            iReturn = ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).CustomExecuteWithReturn(new SQLCondition(updateSql));

            if (iReturn <= 0)
            {
                throw new Exception("$Error_UpdateMO $CS_PLEASE_RETRY");
            }
        }

        //Laws Lu,2005/10/31���޸�	ͳһ������������ı�׼
        public void UpdateMOInPutQty(string moCode/*,int outPutQty*/, MO mo, int qty)
        {
            string updateSql = String.Empty;
            if (mo != null && mo.MOStatus == Web.Helper.MOManufactureStatus.MOSTATUS_OPEN)
            {
                updateSql = "update TBLMO "
                    + " set MOINPUTQTY=MOINPUTQTY+" + qty.ToString()
                    + " where MOCODE='" + moCode.Trim() + "'";
            }
            else
            {
                updateSql = "update TBLMO "
                    + " set MOINPUTQTY=MOINPUTQTY+" + qty.ToString()
                    + ",MOSTATUS=(case  mostatus  when '" + Web.Helper.MOManufactureStatus.MOSTATUS_OPEN + "' then mostatus else '" + Web.Helper.MOManufactureStatus.MOSTATUS_OPEN + "' end)"
                    + ",MOACTSTARTDATE=(case  mostatus  when '" + Web.Helper.MOManufactureStatus.MOSTATUS_OPEN + "' then MOACTSTARTDATE else " + FormatHelper.TODateInt(DateTime.Now) + " end)"
                    //+",MOACTSTARTDATE=" + FormatHelper.TODateInt(DateTime.Now)
                    + " where MOCODE='" + moCode.Trim() + "'";
            }
            int iReturn = 0;
            iReturn = ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).CustomExecuteWithReturn(new SQLCondition(updateSql));

            if (iReturn <= 0)
            {
                throw new Exception("$Error_UpdateMO $CS_PLEASE_RETRY");
            }

        }

        //Laws Lu,2005/10/31���޸�	ͳһ������������ı�׼
        public void UpdateMOQty(string moCode, string actType, int offMoQty)
        {
            string updateSql = String.Empty;

            #region

            int morule = 0;
            string sisql = "select decode(sum(s.idmergerule),'',0,sum(s.idmergerule)) as idmergerule from tblsimulation s where s.mocode = '" + moCode + "' and s.iscom = 1 AND s.productstatus = 'GOOD'";
            object[] simobj = this.DataProvider.CustomQuery(typeof(Simulation), new SQLCondition(sisql));
            if (simobj != null)
            {
                morule = Convert.ToInt32(((Simulation)simobj[0]).IDMergeRule);
            }

            #endregion

            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            if (actType == ActionType.DataCollectAction_OffMo)
            {
                updateSql = "update TBLMO "
                    + " set MOACTQTY='" + morule + "'"
                    //+ " set MOACTQTY=(SELECT COUNT(*) FROM tblsimulation WHERE mocode = '" + moCode + "' AND iscom = 1 AND productstatus='GOOD')"
                    + ",MOINPUTQTY=(SELECT decode(SUM (EAttribute1),null,0,SUM (EAttribute1)) FROM tblrptreallineqty WHERE mocode = '" + moCode + "')"
                    + ",MOSTATUS=(case  mostatus  when '" + Web.Helper.MOManufactureStatus.MOSTATUS_OPEN + "' then mostatus else '" + Web.Helper.MOManufactureStatus.MOSTATUS_OPEN + "' end)"
                    + ",MOACTSTARTDATE=(case  mostatus  when '" + Web.Helper.MOManufactureStatus.MOSTATUS_OPEN + "' then MOACTSTARTDATE else " + dbDateTime.DBDate + " end)"
                    + ",OFFMOQTY = OFFMOQTY + " + offMoQty.ToString()
                    + " where MOCODE='" + moCode + "'";
            }
            else
            {
                updateSql = "update TBLMO "
                    + " set MOACTQTY='" + morule + "'"
                    //+ " set MOACTQTY=(SELECT COUNT(*) FROM tblsimulation WHERE mocode = '" + moCode + "' AND iscom = 1 AND productstatus='GOOD')"
                    + ",MOINPUTQTY=(SELECT decode(SUM (EAttribute1),null,0,SUM (EAttribute1)) FROM tblrptreallineqty WHERE mocode = '" + moCode + "')"
                    + ",MOSTATUS=(case  mostatus  when '" + Web.Helper.MOManufactureStatus.MOSTATUS_OPEN + "' then mostatus else '" + Web.Helper.MOManufactureStatus.MOSTATUS_OPEN + "' end)"
                    + ",MOACTSTARTDATE=(case  mostatus  when '" + Web.Helper.MOManufactureStatus.MOSTATUS_OPEN + "' then MOACTSTARTDATE else " + dbDateTime.DBDate + " end)"
                    + " where MOCODE='" + moCode + "'";
            }

            int iReturn = 0;
            iReturn = ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).CustomExecuteWithReturn(new SQLCondition(updateSql));

            if (iReturn <= 0)
            {
                throw new Exception("$Error_UpdateMO $CS_PLEASE_RETRY");
            }
        }

        //Andy xin���޸�	ͳһ�����깤��������ı�׼
        public void UpdateMOACTQTY(string moCode)
        {
            string updateSql = String.Empty;

            //Laws Lu,2006/11/13 uniform system collect date
            DBDateTime dbDateTime;

            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            updateSql = "update TBLMO  set MOACTQTY=(SELECT DECODE( sum(GOODQTY),'',0,SUM(GOODQTY) ) AS MOACTQTY FROM TBLLOTSIMULATION  WHERE mocode = '" + moCode + "' AND iscom = 1 AND productstatus='GOOD') WHERE mocode = '" + moCode + "'";              

            int iReturn = 0;
            iReturn = ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).CustomExecuteWithReturn(new SQLCondition(updateSql));

            if (iReturn <= 0)
            {
                throw new Exception("$Error_UpdateMO $CS_PLEASE_RETRY");
            }
        }
        #endregion

        #region OffMoCard
        /// <summary>
        /// 
        /// </summary>
        public OffMoCard CreateNewOffMoCard()
        {
            return new OffMoCard();
        }

        public void AddOffMoCard(OffMoCard offMoCard)
        {
            this._helper.AddDomainObject(offMoCard);
        }

        public void UpdateOffMoCard(OffMoCard offMoCard)
        {
            this._helper.UpdateDomainObject(offMoCard);
        }

        public void DeleteOffMoCard(OffMoCard offMoCard)
        {
            this._helper.DeleteDomainObject(offMoCard);
        }

        public void DeleteOffMoCard(OffMoCard[] offMoCard)
        {
            this._helper.DeleteDomainObject(offMoCard);
        }

        public object GetOffMoCard(string pK)
        {
            return this.DataProvider.CustomSearch(typeof(OffMoCard), new object[] { pK });
        }

        public object GetOffMoCardByRcard(string rcard)
        {
            object[] obj = this.DataProvider.CustomQuery(typeof(OffMoCard), new SQLCondition("select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OffMoCard))
                + " from TBLOffMoCard where rcard='" + rcard + "' order by mdate desc,mtime desc"));

            object objReturn = null;

            if (obj != null && obj.Length > 0)
            {
                objReturn = obj[0];
            }

            return objReturn;
        }

        /// <summary>
        /// ** ��������:	��ѯOffMoCard��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-12-15 18:22:52
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="pK">PK��ģ����ѯ</param>
        /// <returns> OffMoCard���ܼ�¼��</returns>
        public int QueryOffMoCardCount(string pK)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLOffMoCard where 1=1 and PK like '{0}%' ", pK)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯOffMoCard
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-12-15 18:22:52
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="pK">PK��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> OffMoCard����</returns>
        public object[] QueryOffMoCard(string pK, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(OffMoCard), new PagerCondition(string.Format("select {0} from TBLOffMoCard where 1=1 and PK like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(OffMoCard)), pK), "PK", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�OffMoCard
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2005-12-15 18:22:52
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>OffMoCard���ܼ�¼��</returns>
        public object[] GetAllOffMoCard()
        {
            return this.DataProvider.CustomQuery(typeof(OffMoCard), new SQLCondition(string.Format("select {0} from TBLOffMoCard order by PK", DomainObjectUtility.GetDomainObjectFieldsString(typeof(OffMoCard)))));
        }


        #endregion

        #region Resource2MO
        /// <summary>
        /// 
        /// </summary>
        public Resource2MO CreateNewResource2MO()
        {
            return new Resource2MO();
        }

        public void AddResource2MO(Resource2MO resource2MO)
        {
            string strSql = "select max(seq) seq from tblres2mo";
            int iSeq = 0;
            object[] objs = this.DataProvider.CustomQuery(typeof(Resource2MO), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                Resource2MO res2mo = (Resource2MO)objs[0];
                iSeq = Convert.ToInt32(res2mo.Sequence) + 1;
            }
            resource2MO.Sequence = iSeq;
            this._helper.AddDomainObject(resource2MO);
        }

        public void UpdateResource2MO(Resource2MO resource2MO)
        {
            this._helper.UpdateDomainObject(resource2MO);
        }

        public void DeleteResource2MO(Resource2MO resource2MO)
        {
            this._helper.DeleteDomainObject(resource2MO);
        }

        public void DeleteResource2MO(Resource2MO[] resource2MO)
        {
            this._helper.DeleteDomainObject(resource2MO);
        }

        public object GetResource2MO(decimal sequence)
        {
            return this.DataProvider.CustomSearch(typeof(Resource2MO), new object[] { sequence });
        }

        /// <summary>
        /// ** ��������:	��ѯResource2MO��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2007-3-8 9:58:08
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <returns> Resource2MO���ܼ�¼��</returns>
        public int QueryResource2MOCount(string resourceCode, int dateFrom, int dateTo)
        {
            string strSql = string.Format("select count(*) from TBLRES2MO where 1=1 and RESCODE like '{0}%' ", resourceCode);
            if (dateFrom == 0)
                dateFrom = 0;
            if (dateTo == 0)
                dateTo = int.MaxValue;
            strSql += " and (";
            strSql += " not ( STARTDATE>" + dateTo.ToString() + " or ENDDATE<" + dateFrom.ToString() + ") ";
            strSql += " ) ";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯResource2MO
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2007-3-8 9:58:08
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Resource2MO����</returns>
        public object[] QueryResource2MO(string resourceCode, int dateFrom, int dateTo, int inclusive, int exclusive)
        {
            string strSql = string.Format("select {0} from TBLRES2MO where 1=1 and RESCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Resource2MO)), resourceCode);
            if (dateFrom == 0)
                dateFrom = 0;
            if (dateTo == 0)
                dateTo = int.MaxValue;
            strSql += " and (";
            strSql += " not ( STARTDATE>" + dateTo.ToString() + " or ENDDATE<" + dateFrom.ToString() + ") ";
            strSql += " ) ";
            return this.DataProvider.CustomQuery(typeof(Resource2MO), new PagerCondition(strSql, "SEQ DESC", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�Resource2MO
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2007-3-8 9:58:08
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>Resource2MO���ܼ�¼��</returns>
        public object[] GetAllResource2MO()
        {
            return this.DataProvider.CustomQuery(typeof(Resource2MO), new SQLCondition(string.Format("select {0} from TBLRES2MO order by SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Resource2MO)))));
        }

        public object[] QueryResource2MOByResourceDate(string resourceCode, int dateFrom, int timeFrom, int dateTo, int timeTo)
        {
            string strSql = string.Format("select {0} from TBLRES2MO where 1=1 and RESCODE = '{1}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Resource2MO)), resourceCode);
            if (dateFrom == 0)
                dateFrom = 0;
            if (dateFrom == 0)
                timeFrom = 0;
            if (timeTo == 0)
                timeTo = 235959;
            if (dateTo == 0)
            {
                dateTo = FormatHelper.TODateInt(DateTime.MaxValue);
                timeTo = 0;
            }
            string strDateFrom = dateFrom.ToString() + timeFrom.ToString().PadLeft(6, '0');
            string strDateTo = dateTo.ToString() + timeTo.ToString().PadLeft(6, '0');
            strSql += " and (";
            strSql += " not ( STARTDATE * 1000000 + STARTTIME>" + strDateTo.ToString() + " or ENDDATE * 1000000 + ENDTIME<" + strDateFrom.ToString() + ") ";
            strSql += " ) ";
            return this.DataProvider.CustomQuery(typeof(Resource2MO), new SQLCondition(strSql));
        }


        #endregion

        #region RMARCARD
        /// <summary>
        /// 
        /// </summary>
        public RMARCARD CreateNewRMARCARD()
        {
            return new RMARCARD();
        }

        public void AddRMARCARD(RMARCARD rMARCARD)
        {
            this._helper.AddDomainObject(rMARCARD);
        }

        public void UpdateRMARCARD(RMARCARD rMARCARD)
        {
            this._helper.UpdateDomainObject(rMARCARD);
        }

        public void DeleteRMARCARD(RMARCARD rMARCARD)
        {
            this._helper.DeleteDomainObject(rMARCARD);
        }

        public void DeleteRMARCARD(RMARCARD[] rMARCARD)
        {
            this._helper.DeleteDomainObject(rMARCARD);
        }

        //		public object GetRMARCARD( string rMABILLNO )
        //		{
        //			return this.DataProvider.CustomSearch(typeof(RMARCARD), new object[]{ rMABILLNO });
        //		}

        public void DeleteRMARCARDByRcard(string rcard)
        {
            DataProvider.CustomExecute(new SQLCondition(String.Format("delete from tblrmarcard where rcard = '{1}'", rcard)));

        }

        public object GetRMARCARDByRcard(string rcard)
        {
            object[] objs = DataProvider.CustomQuery(typeof(RMARCARD)
                , new SQLCondition(String.Format("select {0} from tblrmarcard where rcard = '{1}'"
                , DomainObjectUtility.GetDomainObjectFieldsString(typeof(RMARCARD)), rcard)));

            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }

        public object GetRMARCARDByMoCode(string mocode)
        {
            object[] objs = DataProvider.CustomQuery(typeof(RMARCARD)
                , new SQLCondition(String.Format("select {0} from tblrmarcard where reworkmocode = '{1}'"
                , DomainObjectUtility.GetDomainObjectFieldsString(typeof(RMARCARD)), mocode)));

            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }

        public object GetRepairRMARCARDByRcard(string rcard)
        {
            object[] objs = DataProvider.CustomQuery(typeof(RMARCARD)
                , new SQLCondition(String.Format("select {0} from tblrmarcard where rcard = '{1}' and RMATYPE='" + RMAHandleWay.TSCenter + "'"
                , DomainObjectUtility.GetDomainObjectFieldsString(typeof(RMARCARD)), rcard)));

            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ** ��������:	��ѯRMARCARD��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-6 10:27:03
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="rMABILLNO">RMABILLNO��ģ����ѯ</param>
        /// <returns> RMARCARD���ܼ�¼��</returns>
        public int QueryRMARCARDCount(string rMABILLNO)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRMARCARD where 1=1 and RMABILLNO like '{0}%' ", rMABILLNO)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯRMARCARD
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-6 10:27:03
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="rMABILLNO">RMABILLNO��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> RMARCARD����</returns>
        public object[] QueryRMARCARD(string rMABILLNO, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(RMARCARD), new PagerCondition(string.Format("select {0} from TBLRMARCARD where 1=1 and RMABILLNO like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RMARCARD)), rMABILLNO), "RMABILLNO", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�RMARCARD
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-6 10:27:03
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>RMARCARD���ܼ�¼��</returns>
        public object[] GetAllRMARCARD()
        {
            return this.DataProvider.CustomQuery(typeof(RMARCARD), new SQLCondition(string.Format("select {0} from TBLRMARCARD order by RMABILLNO", DomainObjectUtility.GetDomainObjectFieldsString(typeof(RMARCARD)))));
        }


        #endregion

        public object GetOPBOM(string itemCode, string obCode, string opBomVer, int orgID)
        {
            return this.DataProvider.CustomSearch(typeof(OPBOM), new object[] { obCode, itemCode, opBomVer, orgID });
        }

        public void UpdateOPBOM(OPBOM opBOM)
        {
            this.DataProvider.Update(opBOM);
        }
        /// <summary>
        /// Added By Jessie Lee for P4.4,2005/8/30
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public bool CheckItemCodeUsed(string itemCode)
        {
            string sql = " select count(mocode) from tblmo where itemcode = $ItemCode and mostatus in ('" + MOManufactureStatus.MOSTATUS_OPEN + "','" + MOManufactureStatus.MOSTATUS_RELEASE + "') ";
            int useMOCount = this.DataProvider.GetCount(
                new SQLParamCondition(sql, new SQLParameter[] { new SQLParameter("ItemCode", typeof(string), itemCode) }));
            if (useMOCount > 0) return true;
            else return false;
        }

        #region ERPBOM
        /// <summary>
        /// 
        /// </summary>
        public ERPBOM CreateNewERPBOM()
        {
            return new ERPBOM();
        }

        public void AddERPBOM(ERPBOM eRPBOM)
        {
            this._helper.AddDomainObject(eRPBOM);
        }

        public void UpdateERPBOM(ERPBOM eRPBOM)
        {
            this._helper.UpdateDomainObject(eRPBOM);
        }

        public void DeleteERPBOM(ERPBOM eRPBOM)
        {
            this._helper.DeleteDomainObject(eRPBOM);
        }

        public void DeleteERPBOM(ERPBOM[] eRPBOM)
        {
            this._helper.DeleteDomainObject(eRPBOM);
        }

        public object GetERPBOM(decimal sEQUENCE)
        {
            return this.DataProvider.CustomSearch(typeof(ERPBOM), new object[] { sEQUENCE });
        }

        /// <summary>
        /// ** ��������:	��ѯERPBOM��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-31 14:44:35
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="sEQUENCE">SEQUENCE��ģ����ѯ</param>
        /// <returns> ERPBOM���ܼ�¼��</returns>
        public int QueryERPBOMCount(decimal sEQUENCE)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLERPBOM where 1=1 and SEQUENCE like '{0}%' ", sEQUENCE)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯERPBOM
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-31 14:44:35
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="sEQUENCE">SEQUENCE��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ERPBOM����</returns>
        public object[] QueryERPBOM(decimal sEQUENCE, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ERPBOM), new PagerCondition(string.Format("select {0} from TBLERPBOM where 1=1 and SEQUENCE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ERPBOM)), sEQUENCE), "SEQUENCE", inclusive, exclusive));
        }
        /// <summary>
        /// Get ERPBom by MoCode
        /// </summary>
        /// <param name="mocode">mo code</param>
        /// <returns></returns>
        public object[] QueryERPBOM(string mocode)
        {
            string sql = "select {0} from tblerpbom where mocode='{1}'";

            return this.DataProvider.CustomQuery(typeof(ERPBOM)
                , new SQLCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(ERPBOM)), mocode)));
        }

        /// <summary>
        /// Get ERPBom by MoCode
        /// </summary>
        /// <param name="mocode">mo code</param>
        /// <returns></returns>
        public object[] QueryERPBOM(string mocode, string lotno)
        {
            string sql = "select {0} from tblerpbom where mocode='{1}' and lotno='{2}'";

            return this.DataProvider.CustomQuery(typeof(ERPBOM)
                , new SQLCondition(
                string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(ERPBOM))
                , mocode
                , lotno)));
        }

        /// <summary>
        /// Get ERPBom by MoCode
        /// </summary>
        /// <param name="mocode">mo code</param>
        /// <returns></returns>
        public bool CheckERPBOM(string opID, string mocode)
        {
            bool isExist = true;
            string sqlERPBOM = "select {0} from tblerpbom where mocode='{1}'";
            //string sqlERPBOMQty = "select sum(BQTY) as qty from tblerpbom where mocode='{0}' group by BITEMCODE";
            object mo = this.GetMO(mocode);

            string sqlOPBOM = "select {0} from tblopbomdetail where opid='{1}' and orgid=" + ((MO)mo).OrganizationID;

            //int iERPCount = 0,iOPBOMCount = 0;
            object[] objERPBOMs = this.DataProvider.CustomQuery(typeof(ERPBOM),
                new SQLCondition(string.Format(sqlERPBOM
                , DomainObjectUtility.GetDomainObjectFieldsString(typeof(ERPBOM))
                , mocode)));

            object[] objOPBOMs = this.DataProvider.CustomQuery(typeof(OPBOMDetail),
                new SQLCondition(string.Format(sqlOPBOM
                , DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOMDetail))
                , opID)));


            if (objERPBOMs != null && objERPBOMs.Length > 0)
            {
                //				iERPCount = objERPBOMs.Length ;

                //bool isInclude = true;

                Hashtable htOPBOMCount = new Hashtable();//��ѡ��
                Hashtable htOPBOMCountA = new Hashtable();//�ӽ��Ϻ�
                Hashtable htERPBOMCount = new Hashtable();
                if (objOPBOMs != null && objOPBOMs.Length > 0)
                {

                    foreach (OPBOMDetail opDetail in objOPBOMs)
                    {
                        if (htOPBOMCountA.ContainsKey(opDetail.OPBOMItemCode))
                        {
                            htOPBOMCountA[opDetail.OPBOMItemCode] = Convert.ToInt32(htOPBOMCountA[opDetail.OPBOMItemCode]) + 1;
                        }
                        else
                        {
                            htOPBOMCountA.Add(opDetail.OPBOMItemCode, 1);
                        }

                        if (htOPBOMCount.ContainsKey(opDetail.OPBOMSourceItemCode))
                        {
                            htOPBOMCount[opDetail.OPBOMSourceItemCode] = Convert.ToInt32(htOPBOMCount[opDetail.OPBOMSourceItemCode]) + 1;
                        }
                        else
                        {
                            htOPBOMCount.Add(opDetail.OPBOMSourceItemCode, 1);
                        }
                    }

                    foreach (ERPBOM erp in objERPBOMs)
                    {
                        if (htERPBOMCount.ContainsKey(erp.BITEMCODE))
                        {
                            htERPBOMCount[erp.BITEMCODE] = Convert.ToInt32(htERPBOMCount[erp.BITEMCODE]) + 1;
                        }
                        else
                        {
                            htERPBOMCount.Add(erp.BITEMCODE, 1);
                        }
                    }
                }
                //�������������а���5�����ϣ�����BOM�е��ӽ����ϱ���Ҳ�����������ϣ�
                if (htERPBOMCount.Keys.Count <= htOPBOMCountA.Keys.Count && htERPBOMCount.Keys.Count != 0)
                {
                    //����ѡ�ϲ���������������֮�����������
                    foreach (string key in htOPBOMCount.Keys)
                    {
                        if (!htERPBOMCount.ContainsKey(key))
                        {
                            isExist = false;
                            break;
                        }
                    }
                    foreach (string key in htERPBOMCount.Keys)
                    {
                        if (!htOPBOMCountA.ContainsKey(key))
                        {
                            isExist = false;
                            break;
                        }
                    }
                }
                else
                {
                    isExist = false;
                }

                int status = 0;
                if (isExist)
                {
                    status = 1;
                }

                if (objOPBOMs != null && objOPBOMs.Length > 0)
                {
                    OPBOMDetail opBOMDetail = objOPBOMs[0] as OPBOMDetail;

                    object objOPBOM = this.GetOPBOM(opBOMDetail.ItemCode, opBOMDetail.OPBOMCode, opBOMDetail.OPBOMVersion, opBOMDetail.OrganizationID);
                    if (objOPBOM != null)
                    {
                        OPBOM opBOM = objOPBOM as OPBOM;

                        opBOM.Avialable = status;

                        DataProvider.BeginTransaction();
                        try
                        {
                            UpdateOPBOM(opBOM);

                            DataProvider.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                            DataProvider.RollbackTransaction();
                        }
                        finally
                        {
                            ((SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
                        }
                    }

                }
            }

            return isExist;
        }

        /// <summary>
        /// ** ��������:	������е�ERPBOM
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-31 14:44:35
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ERPBOM���ܼ�¼��</returns>
        public object[] GetAllERPBOM()
        {
            return this.DataProvider.CustomQuery(typeof(ERPBOM), new SQLCondition(string.Format("select {0} from TBLERPBOM order by SEQUENCE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ERPBOM)))));
        }


        #endregion

        //Remark by HI1/Venus.Feng on 20080625 for Hisense Version : Never to use this code
        //melo zheng,2007.1.8,��ȡItemRoute2Operation���ܼ�¼��
        public int GetItemRoute2OperationCount(string itemCode, string routeCode, string opCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from tblitemroute2op where itemcode='" + itemCode + "' and routecode='" + routeCode + "' and opcode ='" + opCode + "'")));

        }
        // End 

        // Added By Hi1/Venus.Feng on 20081120 for Hisense Version : For MO Tail
        public object[] GetMOTailList(string moCode, int inclusive, int exclusive)
        {
            string strSql = "";
            strSql += "SELECT   {0}";
            strSql += "    FROM tblsimulationreport";
            strSql += "   WHERE mocode = '" + moCode + "' AND iscom = '0' ";
            strSql += "ORDER BY rcard";

            return this.DataProvider.CustomQuery(typeof(SimulationReport),
                new PagerCondition(string.Format(strSql,
                                    DomainObjectUtility.GetDomainObjectFieldsString(typeof(SimulationReport))), "rcard", inclusive, exclusive));
        }

        public int GetMOTailListCount(string moCode)
        {
            string strSql = "";
            strSql += "SELECT   COUNT(*)";
            strSql += "    FROM tblsimulationreport";
            strSql += "   WHERE mocode = '" + moCode + "' AND iscom = '0' ";
            strSql += "ORDER BY rcard";

            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        public void DoSplit(SimulationReport simulationReport, DBDateTime currentDateTime, string userCode)
        {
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

            // Step1 : Update Simulation
            Simulation sim = (Simulation)this.GetSimulation(simulationReport.MOCode, simulationReport.RunningCard, simulationReport.RunningCardSequence.ToString());
            if (sim != null)
            {
                sim.IsComplete = "1";
                sim.EAttribute1 = ProductStatus.OffMo;
                sim.ProductStatus = ProductStatus.OffMo;
                sim.MaintainUser = userCode;
                sim.MaintainDate = dbDateTime.DBDate;
                sim.MaintainTime = dbDateTime.DBTime;

                this.UpdateSimulation(sim);
            }

            // Step2 : Update SimulationReport
            simulationReport.EAttribute1 = ProductStatus.OffMo;
            simulationReport.Status = ProductStatus.OffMo;
            simulationReport.IsComplete = "1";
            simulationReport.MaintainUser = userCode;
            simulationReport.MaintainDate = dbDateTime.DBDate;
            simulationReport.MaintainTime = dbDateTime.DBTime;

            this.UpdateSimulationReport(simulationReport);

            // Step3 : Update TS
            object objTS = this.GetCardLastTSRecordInTS(simulationReport.RunningCard);
            if (objTS != null)
            {
                BenQGuru.eMES.Domain.TS.TS ts = objTS as BenQGuru.eMES.Domain.TS.TS;
                ts.TSStatus = TSStatus.TSStatus_OffMo;
                ts.MaintainDate = currentDateTime.DBDate;
                ts.MaintainTime = currentDateTime.DBTime;
                ts.MaintainUser = userCode;

                this.UpdateTS(ts);
            }

            // Step4 : Drop Material
            OnWIPItem[] onwipitems = (new MaterialFacade(this.DataProvider)).QueryLoadedPartByRCard(simulationReport.RunningCard, simulationReport.MOCode);
            if (onwipitems != null)
            {
                CastDownHelper castDownHelper = new CastDownHelper(DataProvider);
                string sql;
                foreach (OnWIPItem wipItem in onwipitems)
                {
                    //��ȡarRcard
                    ArrayList arRcard = new ArrayList();
                    castDownHelper.GetAllRCard(ref arRcard, simulationReport.RunningCard);
                    if (arRcard.Count == 0)
                    {
                        arRcard.Add(simulationReport.RunningCard);
                    }

                    string runningCards = "('" + String.Join("','", (string[])arRcard.ToArray(typeof(string))) + "')";
                    sql = string.Format("update TBLONWIPITEM set TRANSSTATUS='{0}',ActionType=" + (int)MaterialType.DropMaterial +
                        ",DropOP = ''" +
                        ",DropUser='" + userCode + "'" +
                        ",DropDate=" + currentDateTime.DBDate +
                        ",DropTime=" + currentDateTime.DBTime +
                        " where RCARD in {1} and ActionType='{2}'" +
                        " and MCARD in ('" + wipItem.MCARD.Trim().ToUpper() + "')"
                        , TransactionStatus.TransactionStatus_YES
                        , runningCards
                        , (int)Web.Helper.MaterialType.CollectMaterial);
                    this.DataProvider.CustomExecute(new SQLCondition(sql));

                    if (wipItem.MCardType == MCardType.MCardType_Keyparts)
                    {
                        sql = "update TBLSIMULATIONREPORT set IsLoadedPart='0',LoadedRCard='' " +
                            " where RCARD in ('" + wipItem.MCARD.Trim().ToUpper() + "')";
                        this.DataProvider.CustomExecute(new SQLCondition(sql));
                    }
                }
            }

            MO mo = this.GetMO(simulationReport.MOCode) as MO;
            OffMoCard offCard = new OffMoCard();
            offCard.PK = System.Guid.NewGuid().ToString();
            offCard.MoCode = simulationReport.MOCode;
            offCard.RCARD = simulationReport.RunningCard;
            offCard.MoType = mo.MOType;
            offCard.MUSER = userCode;
            offCard.MDATE = currentDateTime.DBDate;
            offCard.MTIME = currentDateTime.DBTime;
            this.AddOffMoCard(offCard);

            // Step5 : Update tblmo.mooffqty
            this.UpdateMOQty(simulationReport.MOCode, "OFFMO", Convert.ToInt32(1 * simulationReport.IDMergeRule));
        }

        public void DoScrap(SimulationReport simulationReport, DBDateTime currentDateTime, string userCode)
        {
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);

            // Step1 : Update Simulation
            Simulation sim = (Simulation)this.GetSimulation(simulationReport.MOCode, simulationReport.RunningCard, simulationReport.RunningCardSequence.ToString());
            if (sim != null)
            {
                sim.IsComplete = "1";
                sim.EAttribute1 = "SCRAP";
                sim.ProductStatus = "SCRAP";
                sim.MaintainUser = userCode;
                sim.MaintainDate = dbDateTime.DBDate;
                sim.MaintainTime = dbDateTime.DBTime;

                this.UpdateSimulation(sim);
            }

            // Step2 : Update SimulationReport
            simulationReport.EAttribute1 = "SCRAP";
            simulationReport.IsComplete = "1";
            simulationReport.Status = "SCRAP";
            simulationReport.MaintainUser = userCode;
            simulationReport.MaintainDate = dbDateTime.DBDate;
            simulationReport.MaintainTime = dbDateTime.DBTime;

            this.UpdateSimulationReport(simulationReport);

            // Step3 : Update TS
            object objTS = this.GetCardLastTSRecordInTS(simulationReport.RunningCard);
            if (objTS != null)
            {
                BenQGuru.eMES.Domain.TS.TS ts = objTS as BenQGuru.eMES.Domain.TS.TS;
                ts.TSStatus = TSStatus.TSStatus_Scrap;
                ts.MaintainDate = currentDateTime.DBDate;
                ts.MaintainTime = currentDateTime.DBTime;
                ts.MaintainUser = userCode;

                this.UpdateTS(ts);
            }

            // Step4 : Update tblmo.moscrapqty+1
            MO mo = this.GetMO(simulationReport.MOCode) as MO;
            if (mo != null)
            {
                mo.MOScrapQty = 1 * simulationReport.IDMergeRule;

                this.UpdateMOScrapQty(mo);
            }
        }
        public void UpdateTS(BenQGuru.eMES.Domain.TS.TS tS)
        {
            this.DataProvider.Update(tS);
        }
        public object GetCardLastTSRecordInTS(string rcard)
        {
            object[] objs = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.TS.TS),
                new SQLParamCondition(String.Format(
                @"SELECT {0}  FROM tblts WHERE tsid = (select tsid
									from (SELECT tsid
											FROM tblts
											WHERE rcard = $RCARD
											order by MDATE * 1000000 + MTIME DESC)
									where rownum = 1) and tsstatus in ('" + TSStatus.TSStatus_New + "','" + TSStatus.TSStatus_Confirm + "','" + TSStatus.TSStatus_TS + "','" + TSStatus.TSStatus_Reflow + "') ",//��ǰֻ�и���ά�����ݣ�������ӻ�����
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
        // End Added

          //add by leo 20120920
        public object GetMaxRcardSameLotNo(string rcardPrefix)
        {
            string sql = string.Format(@"SELECT MAX(rcard) AS rcard FROM TBLMO2RCARDLINK WHERE rcard LIKE '{0}%'", FormatHelper.CleanString(rcardPrefix));
            object[] objs = this.DataProvider.CustomQuery(typeof(MO2RCARDLINK), new SQLCondition(sql));
            if (objs != null)
            {
                return objs[0];
            }
            return null;
        }
      
        //add  by Leo @2011-11-22 for Query  M02RcardLink
        public object[] GetMO2RCardLinkMOItemForQuery(string rcard, string moCode)
        {
            string sql = string.Format(@"  SELECT   TBLMO2RCARDLINK.mocode,TBLMO2RCARDLINK.Rcard, TBLMO2RCARDLINK.Printtimes,TBLMO2RCARDLINK.Lastprintuser,
                                           TBLMO2RCARDLINK.Lastprintdate,TBLMO2RCARDLINK.Lastprinttime,TBLMO2RCARDLINK.Muser,TBLMO2RCARDLINK.Mdate,TBLMO2RCARDLINK.Mtime,
                                           tblmo.Lotno,Tblmo.Moplanqty,Tblitem.Itemcode,Tblitem.Itemdesc
                                           FROM  TBLMO2RCARDLINK,Tblmo,Tblitem  WHERE 1=1  
                                           AND  Tblitem.Itemcode=Tblmo.Itemcode
                                           AND  tblMO2RCARDLINK.Mocode=Tblmo.Mocode");
            if (moCode.Length != 0 && moCode != string.Empty)
            {
                sql += string.Format(@" AND  TBLMO2RCARDLINK.Mocode='{0}'", moCode);
            }
            if (rcard.Length != 0 && rcard != string.Empty)
            {
                sql += string.Format(@"AND  TBLMO2RCARDLINK.Rcard='{0}'", rcard);
            }

            sql += string.Format("ORDER BY  RCARD");

            return this.DataProvider.CustomQuery(typeof(MO2RCARDLINKForQuery), new SQLCondition(sql));

        }

    }

    [Serializable]
    public class BItemQty : DomainObject
    {
        [FieldMapAttribute("QTY", typeof(int), true)]
        public int QTY;
    }

}