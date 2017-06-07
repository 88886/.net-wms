#region System
using System;
using System.Runtime.Remoting;
using System.Collections;
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.MOModel;
#endregion

/// OPBOMFacade ��ժҪ˵����
/// �ļ���:
/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
/// ������:Crystal Chu
/// ��������:2005/03/22
/// �޸���:
/// �޸�����:
/// �� ��: ��OPBOM�Ĳ�������
/// �� ��:	
/// </summary>     
namespace BenQGuru.eMES.MOModel
{

    public class OPBOMFacade : MarshalByRefObject
    {
        //private static readonly log4net.ILog _log = BenQGuru.eMES.Common.Log.GetLogger(typeof(MOFacade));
        private IDomainDataProvider _domainDataProvider = null;
        private FacadeHelper _helper = null;

        public static readonly string OPBOMITEMTYPE_DEFAULT = "lot";
        public static readonly string OPBOMVERSION_DEFAULT = "1";
        public static readonly string OPBOMISItemCheckValue_DEFAULT = "0";
        public static readonly string OPBOMItemCheckValue_DEFAULT = "0000000";
        public static readonly string OPBOMItemUOM_DEFAULT = "OPBOMItemUOM";

        public OPBOMFacade(IDomainDataProvider domainDataProvider)
        {
            this._domainDataProvider = domainDataProvider;
            this._helper = new FacadeHelper(DataProvider);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public OPBOMFacade()
        {
            this._helper = new FacadeHelper(DataProvider);
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


        #region opBOM
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OPBOM CreateNewOPBOM()
        {
            return new OPBOM();
        }


        public object GetOPBOM(string itemCode, string opBOMCode, string opBOMVersion, int orgID)
        {
            return this.DataProvider.CustomSearch(typeof(OPBOM), new object[] { itemCode, opBOMCode, opBOMVersion, orgID });
        }
        /// <summary>
        /// item��BOMCode,��ʱ�����ǰ汾�汾Ϊ1
        /// �����ǰ汾������
        /// </summary>
        /// <param name="opBOM"></param>
        public void AddOPBOM(OPBOM opBOM)
        {
            if (opBOM.OPBOMVersion.Trim().Length == 0)
                opBOM.OPBOMVersion = OPBOMVERSION_DEFAULT;
            this._helper.AddDomainObject(opBOM);
        }

        public void BuildOPBOM(string itemCode)
        {
            ItemFacade itemFacade = new ItemFacade(this.DataProvider);
            object[] objs = itemFacade.QueryItem2Route(itemCode, string.Empty, GlobalVariables.CurrentOrganizations.First().OrganizationID.ToString());
            if (objs == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_ItemRouteNotExist", String.Format("[$ItemCode='{0}']", itemCode), null);
            }

            SBOMFacade sbomFacade = new SBOMFacade(this.DataProvider);
            object[] sboms = sbomFacade.GetAllSBOMVersion(itemCode, GlobalVariables.CurrentOrganizations.First().OrganizationID);

            if (sboms == null || sboms.Length == 0)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_SBOMNotExist", "", null);
            }

            OPBOM opBOM = null;
            object[] objsOPBOM = null;

            for (int i = 0; i < objs.Length; i++)
            {
                foreach (SBOM sbom in sboms)
                {
                    //�жϸ�Org+Item+Route+Version�Ƿ��Ѿ����ڣ���������������,���ڲ����κζ���
                    objsOPBOM = QueryOPBOM(itemCode, ((Item2Route)objs[i]).RouteCode, sbom.SBOMVersion, int.MinValue, int.MaxValue, ((Item2Route)objs[i]).OrganizationID);
                    if (objsOPBOM == null)
                    {
                        opBOM = CreateNewOPBOM();
                        opBOM.ItemCode = itemCode;
                        opBOM.OPBOMVersion = sbom.SBOMVersion;
                        opBOM.OPBOMRoute = ((Item2Route)objs[i]).RouteCode;
                        opBOM.MaintainUser = ((Item2Route)objs[i]).MaintainUser;
                        opBOM.OPBOMCode = ((Item2Route)objs[i]).RouteCode;
                        opBOM.OrganizationID = ((Item2Route)objs[i]).OrganizationID;
                        this.AddOPBOM(opBOM);
                    }
                }
            }
        }

        public void UpdateOPBOM(OPBOM opBOM)
        {
            this._helper.UpdateDomainObject(opBOM);
        }

        /// <summary>
        /// ɾ�����߼���û�б��ù��Ϳ��Ա�ɾ����ɾ����ͬʱ
        /// ɾ��ά���Ķ�Ӧ��OPBOM��ϸ����Ϣ�����Ͽ��Ƶ���Ϣ
        /// �ù��ļ���߼��������ڹ����м��bom�Ͷ�Ӧ�İ汾�Ƿ���� 
        /// </summary>
        /// <param name="opBOM"></param>
        public void DeleteOPBOM(OPBOM opBOM)
        {
            if (opBOM == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Null_Paramter");
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),String.Format(ErrorCenter.ERROR_ARGUMENT_NULL,"opBOM")));
            }
            OPBOMChangedCheck(opBOM.ItemCode, opBOM.OPBOMCode, opBOM.OPBOMVersion, opBOM.OrganizationID);

            //�Ƿ��Ѿ�ά�����ϵ���Ϣ��������򲻿��Ա�ɾ��
            if (IsBOMComponentLoading(opBOM))
            {
                ExceptionManager.Raise(this.GetType(), "$Error_CannotDelete_OPBOMItemControlExist", String.Format("[$OPBOMCode='{0}']", opBOM.OPBOMCode), null);
                //					throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),String.Format(ErrorCenter.ERROR_BOMCOMPONENTLOADINGUSED,opBOM.OPBOMCode)));
            }
            try
            {
                this.DataProvider.Delete(opBOM);
            }
            catch (Exception ex)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_DeleteOPBOM", ex);
                //					throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(ItemFacade),String.Format(ErrorCenter.ERROR_DELETEOPBOM,opBOM.OPBOMCode)), ex);
            }
        }

        /// <summary>
        /// ɾ��opbom��Ⱥ��Ĳ�����������Ĵ���
        /// </summary>
        /// <param name="opBOMs"></param>
        public void DeleteOPBOM(OPBOM[] opBOMs)
        {
            try
            {
                this.DataProvider.BeginTransaction();
                for (int i = 0; i < opBOMs.Length; i++)
                {
                    DeleteOPBOM(opBOMs[i]);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();

                ExceptionManager.Raise(this.GetType(), "$Error_DeleteOPBOM", ex);
                //				throw ex;
            }
        }


        /// <summary>
        /// ֧��ģ����ѯ
        /// </summary>
        /// <param name="itemCode">ģ���ֶ�</param>
        /// <param name="opBOMCode">ģ���ֶ�</param>
        /// <param name="opBOMVersion"></param>
        /// <param name="inclusive"></param>
        /// <param name="exclusive"></param>
        /// <returns>����object������</returns>
        public object[] QueryOPBOM(string itemCode, string opBOMCode, string opBOMVersion, int inclusive, int exclusive, int orgID)
        {
            string sql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOM)) + " from tblopbom where 1=1 {0}";
            object[] objs = new object[1];
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode='" + itemCode.Trim() + "'";
            }
            if ((opBOMCode != string.Empty) && (opBOMCode.Trim() != string.Empty))
            {
                tmpString += " and obcode='" + opBOMCode.Trim() + "'";
            }
            if ((opBOMVersion != string.Empty) && (opBOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver='" + opBOMVersion.Trim() + "'";
            }
            tmpString += " and orgid=" + orgID;
            objs[0] = tmpString;
            return this.DataProvider.CustomQuery(typeof(OPBOM), new PagerCondition(String.Format(sql, objs), inclusive, exclusive));
        }
        public int QueryOPBOMCount(string itemCode, string opBOMCode, string opBOMVersion, int orgID)
        {
            string sql = "select count(*) from tblopbom where 1=1 {0}";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode='" + itemCode.Trim() + "'";
            }
            if ((opBOMCode != string.Empty) && (opBOMCode.Trim() != string.Empty))
            {
                tmpString += " and obcode='" + opBOMCode.Trim() + "'";
            }
            if ((opBOMVersion != string.Empty) && (opBOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver='" + opBOMVersion.Trim() + "'";
            }
            tmpString += " and orgid=" + orgID;
            return this.DataProvider.GetCount(new SQLCondition(String.Format(sql, tmpString)));
        }

        public object[] QueryDistinctOPBOMRoute(string itemCode, string opBOMVersion, int orgID)
        {
            string sql = "select distinct obroute from tblopbom where 1=1 {0} order by obroute ";
            object[] objs = new object[1];
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode='" + itemCode.Trim() + "'";
            }
            if ((opBOMVersion != string.Empty) && (opBOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver='" + opBOMVersion.Trim() + "'";
            }
            tmpString += " and orgid=" + orgID;
            objs[0] = tmpString;
            return this.DataProvider.CustomQuery(typeof(OPBOM), new SQLCondition(String.Format(sql, objs)));
        }

        public string GetOPBOMCodeByRouteCode(string itemCode, string routeCode)
        {
            string sql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOM)) + " from tblopbom where 1=1 {0}";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode ='" + itemCode.Trim() + "'";
            }
            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and obroute ='" + routeCode.Trim() + "'";
            }

            tmpString += " and orgid=" + GlobalVariables.CurrentOrganizations.First().OrganizationID;

            object[] objs = this.DataProvider.CustomQuery(typeof(OPBOM), new SQLCondition(String.Format(sql, tmpString)));
            if (objs == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_OPBOMNotExist", string.Format("[$ItemCode='{0}',$RouteCode='{1}']", itemCode, routeCode), null);
            }

            return ((OPBOM)objs[0]).OPBOMCode.ToString();
        }
        public OPBOM GetOPBOMByRouteCode(string itemCode, string routeCode, int orgID, string bomVersion)
        {
            string sql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOM)) + " from tblopbom where 1=1 {0}";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode ='" + itemCode.Trim() + "'";
            }
            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and obroute ='" + routeCode.Trim() + "'";
            }
            if ((bomVersion != string.Empty) && (bomVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + bomVersion.Trim() + "'";
            }
            tmpString += " and orgid=" + orgID;
            //����;���Ƿ������Ϲ���
            bool IsComponetLoading = this.CheckItemRouteIsContainComponetLoading(itemCode, routeCode, orgID);

            object[] objsOp = this.GetItemRouteIsContainComponetLoading(itemCode, routeCode, orgID);
            object[] objs = this.DataProvider.CustomQuery(typeof(OPBOM), new SQLCondition(String.Format(sql, tmpString)));


            //��鹤��;���Ƿ������Ϲ���,�������OPBOM������Ϊ�ղ��׳��쳣,����������һ��OPBOM�������ݿ�,���������OPBOM,�����׳��쳣
            //	Modify By Simone Xu 2005/08/15
            if (objs == null)
            {
                if (IsComponetLoading)
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_OPBOMNotExist1", string.Format("[$ItemCode='{0}',$RouteCode='{1}']", itemCode, routeCode), null);
                }
                else
                {
                    OPBOM opBOM = CreateNewOPBOM();
                    opBOM.ItemCode = itemCode;
                    opBOM.OPBOMRoute = routeCode;
                    opBOM.MaintainUser = "ADMIN";
                    opBOM.OPBOMCode = routeCode;
                    opBOM.OrganizationID = orgID;
                    opBOM.OPBOMVersion = bomVersion;
                    this.AddOPBOM(opBOM);
                    return opBOM;
                }
            }
            else
            {
                if (IsComponetLoading)
                {
                    foreach (object opbom in objsOp)
                    {
                        string selectSql = "select count(*) from tblopbomdetail where 1= 1 {0} and orgid=" + orgID;
                        string tmpString2 = " and itemcode ='" + itemCode.Trim() + "' ";
                        tmpString2 += " and opcode='" + ((ItemRoute2OP)opbom).OPCode.Trim() + "'";

                        string opcodesSelect = string.Format(" AND obcode = '" + ((OPBOM)objs[0]).OPBOMCode.Trim() + "' ");
                        tmpString2 += opcodesSelect;
                        if (!(this.DataProvider.GetCount(new SQLCondition(String.Format(selectSql, tmpString2))) > 0))
                        {
                            ExceptionManager.Raise(this.GetType(), "$Error_OPBOMNotExist1", string.Format("[$ItemCode='{0}',$RouteCode='{1}',$OpCode='{2}']", itemCode, routeCode, ((ItemRoute2OP)opbom).OPCode), null);
                            break;
                        }
                    }
                }
            }

            return (OPBOM)objs[0];
        }


        #region ����Ʒ;���Ƿ�������Ϲ���

        public bool CheckItemRouteIsContainComponetLoading(string itemcode, string routecode, int orgID)
        {
            bool returnbool = false;
            string sql = string.Format("select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(ItemRoute2OP)) + " from TBLITEMRoute2OP WHERE itemcode = '{0}' AND routecode = '{1}' AND orgid={2}", itemcode, routecode, orgID);
            object[] itemroute2ops = this.DataProvider.CustomQuery(typeof(ItemRoute2OP), new SQLCondition(sql));
            if (itemroute2ops != null)
            {
                foreach (object _item2op in itemroute2ops)
                {
                    if (IsComponentLoadingOperation(((ItemRoute2OP)_item2op).OPControl))
                    {
                        returnbool = true;
                        break;
                    }
                }
            }

            return returnbool;
        }

        public object[] GetItemRouteIsContainComponetLoading(string itemcode, string routecode, int orgID)
        {
            ArrayList aList = new ArrayList();
            string sql = string.Format("select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(ItemRoute2OP)) + " from TBLITEMRoute2OP WHERE itemcode = '{0}' AND routecode = '{1}' AND orgid={2}", itemcode, routecode, orgID);
            object[] itemroute2ops = this.DataProvider.CustomQuery(typeof(ItemRoute2OP), new SQLCondition(sql));
            if (itemroute2ops != null)
            {
                foreach (object _item2op in itemroute2ops)
                {
                    if (IsComponentLoadingOperation(((ItemRoute2OP)_item2op).OPControl))
                    {
                        aList.Add(_item2op);
                    }
                }
            }
            if (aList.Count > 0)
            {
                object[] objList = new object[aList.Count];
                aList.CopyTo(objList);
                return objList;
            }
            return null;
        }

        private bool IsComponentLoadingOperation(string opControl)
        {
            return FormatHelper.StringToBoolean(opControl, (int)OperationList.ComponentLoading);
        }

        #endregion

        #endregion

        #region opbom detail�Ĵ���
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OPBOMDetail CreateNewOPBOMDetail()
        {
            return new OPBOMDetail();
        }


        /// <summary>
        /// �����������ϵ��б�������
        /// </summary>
        /// <param name="OPBOMCode"></param>
        /// <param name="OPBOMVersion"></param>
        /// <param name="modelOPeration"></param>
        /// <param name="sboms"></param>
        public void AssignBOMItemToOperation(string OPBOMCode, string OPBOMVersion, Model2OP modelOPeration, SBOM[] sboms)
        {
            if ((OPBOMCode == string.Empty) || (OPBOMCode.Trim() == string.Empty) || (sboms == null))
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Null_Paramter");
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),String.Format(ErrorCenter.ERROR_ARGUMENT_NULL,"OPBOMCode")));
            }
            OPBOMChangedCheck(sboms[0].ItemCode, OPBOMCode, OPBOMVersion, GlobalVariables.CurrentOrganizations.First().OrganizationID, false);
            try
            {
                this.DataProvider.BeginTransaction();
                for (int i = 0; i < sboms.Length; i++)
                {
                    OPBOMDetail opBOMDetail = new OPBOMDetail();
                    opBOMDetail.ItemCode = sboms[i].ItemCode;
                    opBOMDetail.IsItemCheck = OPBOMCheckOption.ISITEMCHECK_DEFAULT;
                    opBOMDetail.ItemCheckValue = OPBOMCheckOption.ITEMCHECKVALUE_DEFAULT;

                    //Laws Lu,2006/11/13 uniform system collect date
                    DBDateTime dbDateTime;

                    dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

                    opBOMDetail.MaintainDate = dbDateTime.DBDate;
                    opBOMDetail.MaintainTime = dbDateTime.DBTime;

                    opBOMDetail.MaintainUser = sboms[i].MaintainUser;
                    opBOMDetail.OPBOMCode = OPBOMCode;
                    opBOMDetail.OPBOMItemCode = sboms[i].SBOMItemCode;
                    //opBOMDetail.OPBOMItemControlType = sboms[i].SBOMItemControlType;

                    if (string.IsNullOrEmpty(sboms[i].SBOMItemControlType))
                        sboms[i].SBOMItemControlType = " ";

                    if (sboms[i].SBOMItemControlType.ToLower() == "itemcontroltype_lot")
                        opBOMDetail.OPBOMItemControlType = BOMItemControlType.ITEM_CONTROL_LOT;
                    else
                        opBOMDetail.OPBOMItemControlType = sboms[i].SBOMItemControlType;
                    opBOMDetail.OPBOMItemECN = sboms[i].SBOMItemECN;
                    opBOMDetail.OPBOMItemName = sboms[i].SBOMItemName;
                    opBOMDetail.OPBOMItemQty = sboms[i].SBOMItemQty;
                    opBOMDetail.OPBOMItemType = BOMItemControlType.ITEM_CONTROL_NOCONTROL;//OPBOMITEMTYPE_DEFAULT;
                    opBOMDetail.OPBOMItemVersion = sboms[i].SBOMItemVersion;
                    opBOMDetail.OPBOMSourceItemCode = sboms[i].SBOMSourceItemCode;
                    opBOMDetail.OPBOMVersion = OPBOMVersion;
                    opBOMDetail.OPBOMItemUOM = string.IsNullOrEmpty(sboms[i].SBOMItemUOM) ? " " : sboms[i].SBOMItemUOM;
                    opBOMDetail.OPCode = modelOPeration.OPCode;
                    opBOMDetail.OPID = modelOPeration.OPID;
                    opBOMDetail.OPBOMItemEffectiveDate = sboms[i].SBOMItemEffectiveDate;
                    opBOMDetail.OPBOMItemEffectiveTime = sboms[i].SBOMItemEffectiveTime;
                    opBOMDetail.OPBOMItemInvalidDate = sboms[i].SBOMItemInvalidDate;
                    opBOMDetail.OPBOMItemInvalidTime = sboms[i].SBOMItemInvalidTime;
                    opBOMDetail.OrganizationID = GlobalVariables.CurrentOrganizations.First().OrganizationID;
                    //					opBOMDetail.RouteCode = modelOPeration.RouteCode;
                    if (this.DataProvider.CustomSearch(typeof(OPBOMDetail), new object[] { opBOMDetail.ItemCode, opBOMDetail.OPBOMCode, opBOMDetail.OPBOMVersion, opBOMDetail.OPID, opBOMDetail.OPBOMItemCode, opBOMDetail.OrganizationID }) != null)
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_OPBOMDetail_Exist", String.Format("[$OPCode='{0}']", opBOMDetail.OPCode), null);
                        //						throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),string.Format(ErrorCenter.ERROR_ASSIGNBOMITEMTOOPERATION,modelOPeration.OPCode)));
                    }
                    this.DataProvider.Insert(opBOMDetail);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                //_log.Error(ex.Message,ex);
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_AssignBOMItemToOperation_Failure", String.Format("[$OPCode='{0}']", modelOPeration.OPCode), ex);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),string.Format(ErrorCenter.ERROR_ASSIGNBOMITEMTOOPERATION,modelOPeration.OPCode)),ex);
            }
        }

        public void AssignBOMItemToOperation(string OPBOMCode, string OPBOMVersion, ItemRoute2OP itemroute2OP, SBOM[] sboms)
        {
            if ((OPBOMCode == string.Empty) || (OPBOMVersion.Trim() == string.Empty) || (sboms == null))
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Null_Paramter");
            }
            OPBOMChangedCheck(sboms[0].ItemCode, OPBOMCode, OPBOMVersion, itemroute2OP.OrganizationID, false);
            try
            {
                this.DataProvider.BeginTransaction();
                for (int i = 0; i < sboms.Length; i++)
                {
                    //Laws Lu,2006/11/13 uniform system collect date
                    DBDateTime dbDateTime;

                    dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

                    OPBOMDetail opBOMDetail = new OPBOMDetail();
                    opBOMDetail.ItemCode = sboms[i].ItemCode;
                    opBOMDetail.IsItemCheck = OPBOMCheckOption.ISITEMCHECK_DEFAULT;
                    opBOMDetail.ItemCheckValue = OPBOMCheckOption.ITEMCHECKVALUE_DEFAULT;

                    opBOMDetail.MaintainDate = dbDateTime.DBDate;
                    opBOMDetail.MaintainTime = dbDateTime.DBTime;

                    opBOMDetail.MaintainUser = sboms[i].MaintainUser;
                    opBOMDetail.OPBOMCode = OPBOMCode;
                    opBOMDetail.OPBOMItemCode = sboms[i].SBOMItemCode;
                    opBOMDetail.OPBOMItemControlType = string.IsNullOrEmpty(sboms[i].SBOMItemControlType) ? " " : sboms[i].SBOMItemControlType;
                    opBOMDetail.OPBOMItemECN = sboms[i].SBOMItemECN;
                    opBOMDetail.OPBOMItemName = sboms[i].SBOMItemName;
                    opBOMDetail.OPBOMItemQty = sboms[i].SBOMItemQty;
                    opBOMDetail.OPBOMItemType = BOMItemControlType.ITEM_CONTROL_NOCONTROL;///OPBOMITEMTYPE_DEFAULT;
                    opBOMDetail.OPBOMItemVersion = sboms[i].SBOMItemVersion;
                    opBOMDetail.OPBOMSourceItemCode = sboms[i].SBOMSourceItemCode;
                    opBOMDetail.OPBOMVersion = OPBOMVersion;
                    opBOMDetail.OPCode = itemroute2OP.OPCode;
                    opBOMDetail.OPBOMItemUOM = string.IsNullOrEmpty(sboms[i].SBOMItemUOM) ? " " : sboms[i].SBOMItemUOM;
                    opBOMDetail.OPID = itemroute2OP.OPID;
                    opBOMDetail.OPBOMItemEffectiveDate = sboms[i].SBOMItemEffectiveDate;
                    opBOMDetail.OPBOMItemEffectiveTime = sboms[i].SBOMItemEffectiveTime;
                    opBOMDetail.OPBOMItemInvalidDate = sboms[i].SBOMItemInvalidDate;
                    opBOMDetail.OPBOMItemInvalidTime = sboms[i].SBOMItemInvalidTime;

                    opBOMDetail.OrganizationID = itemroute2OP.OrganizationID;

                    if (this.DataProvider.CustomSearch(typeof(OPBOMDetail), new object[] { opBOMDetail.ItemCode, opBOMDetail.OPBOMCode, opBOMDetail.OPBOMVersion, opBOMDetail.OPID, opBOMDetail.OPBOMItemCode, opBOMDetail.OrganizationID }) != null)
                    {
                        ExceptionManager.Raise(this.GetType(), "$Error_OPBOMDetail_Exist", String.Format("[$OPCode='{0}']", opBOMDetail.OPCode), null);
                    }
                    this.DataProvider.Insert(opBOMDetail);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_AssignBOMItemToOperation_Failure", String.Format("[$OPCode='{0}']", itemroute2OP.OPCode), ex);
            }
        }

        //ActionType ����0 ����1
        //public void AssignBOMItemToOperation(string OPBOMCode, string OPBOMVersion, ItemRoute2OP itemroute2OP, SBOM[] sboms, int ActionType)
        //{
        //    if ((OPBOMCode == string.Empty) || (OPBOMVersion.Trim() == string.Empty) || (sboms == null))
        //    {
        //        ExceptionManager.Raise(this.GetType(), "$Error_Null_Paramter");
        //    }
        //    OPBOMChangedCheck(sboms[0].ItemCode, OPBOMCode, OPBOMVersion, itemroute2OP.OrganizationID, false);
        //    try
        //    {
        //        this.DataProvider.BeginTransaction();
        //        for (int i = 0; i < sboms.Length; i++)
        //        {
        //            //Laws Lu,2006/11/13 uniform system collect date
        //            DBDateTime dbDateTime;
        //            dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

        //            OPBOMDetail opBOMDetail = new OPBOMDetail();

        //            opBOMDetail.ItemCode = sboms[i].ItemCode;
        //            opBOMDetail.IsItemCheck = OPBOMCheckOption.ISITEMCHECK_DEFAULT;
        //            opBOMDetail.ItemCheckValue = OPBOMCheckOption.ITEMCHECKVALUE_DEFAULT;

        //            opBOMDetail.MaintainDate = dbDateTime.DBDate;
        //            opBOMDetail.MaintainTime = dbDateTime.DBTime;
        //            opBOMDetail.MaintainUser = sboms[i].MaintainUser;

        //            opBOMDetail.OPBOMCode = OPBOMCode;
        //            opBOMDetail.OPBOMItemCode = sboms[i].SBOMItemCode;
        //            opBOMDetail.OPBOMItemECN = sboms[i].SBOMItemECN;
        //            opBOMDetail.OPBOMItemName = sboms[i].SBOMItemName;
        //            opBOMDetail.OPBOMItemQty = sboms[i].SBOMItemQty;
        //            opBOMDetail.OPBOMItemType = OPBOMITEMTYPE_DEFAULT;
        //            opBOMDetail.OPBOMItemVersion = sboms[i].SBOMItemVersion;
        //            opBOMDetail.OPBOMSourceItemCode = sboms[i].SBOMSourceItemCode;
        //            opBOMDetail.OPBOMVersion = OPBOMVersion;
        //            opBOMDetail.OPCode = itemroute2OP.OPCode;
        //            opBOMDetail.OPBOMItemUOM = string.IsNullOrEmpty(sboms[i].SBOMItemUOM) ? " " : sboms[i].SBOMItemUOM;
        //            opBOMDetail.OPID = itemroute2OP.OPID;
        //            opBOMDetail.OPBOMItemEffectiveDate = sboms[i].SBOMItemEffectiveDate;
        //            opBOMDetail.OPBOMItemEffectiveTime = sboms[i].SBOMItemEffectiveTime;
        //            opBOMDetail.OPBOMItemInvalidDate = sboms[i].SBOMItemInvalidDate;
        //            opBOMDetail.OPBOMItemInvalidTime = sboms[i].SBOMItemInvalidTime;
        //            opBOMDetail.ActionType = ActionType;

        //            opBOMDetail.OrganizationID = itemroute2OP.OrganizationID;

        //            opBOMDetail.OPBOMValid = int.Parse(BenQGuru.eMES.Web.Helper.FormatHelper.TRUE_STRING);
        //            opBOMDetail.OPBOMItemSeq = GetMaxOPBOMItemSeq(opBOMDetail.OPID, opBOMDetail.ItemCode, opBOMDetail.OPBOMCode, opBOMDetail.OPBOMVersion, opBOMDetail.OrganizationID) + 1;

        //            ItemFacade itemFacade = new ItemFacade(this.DataProvider);
        //            Domain.MOModel.Material material = (Domain.MOModel.Material)itemFacade.GetMaterial(opBOMDetail.OPBOMItemCode, opBOMDetail.OrganizationID);
        //            if (material != null)
        //            {
        //                opBOMDetail.OPBOMItemControlType = material.MaterialControlType;

        //                opBOMDetail.OPBOMParseType = string.IsNullOrEmpty(material.MaterialParseType) ? " " : material.MaterialParseType;
        //                opBOMDetail.OPBOMCheckType = string.IsNullOrEmpty(material.MaterialCheckType) ? " " : material.MaterialCheckType;
        //                opBOMDetail.CheckStatus = material.CheckStatus;
        //                opBOMDetail.SerialNoLength = material.SerialNoLength;
        //                opBOMDetail.NeedVendor = material.NeedVendor;
        //            }
        //            else
        //            {
        //                opBOMDetail.OPBOMItemControlType = " ";

        //                opBOMDetail.OPBOMParseType = " ";
        //                opBOMDetail.OPBOMCheckType = " ";
        //                opBOMDetail.CheckStatus = BenQGuru.eMES.Web.Helper.FormatHelper.FALSE_STRING;
        //                opBOMDetail.SerialNoLength = 0;
        //                opBOMDetail.NeedVendor = NeedVendor.NeedVendor_N;
        //            }

        //            if (this.DataProvider.CustomSearch(typeof(OPBOMDetail), new object[] { opBOMDetail.ItemCode, opBOMDetail.OPBOMCode, opBOMDetail.OPBOMVersion, opBOMDetail.OPID, opBOMDetail.OPBOMItemCode, opBOMDetail.ActionType, opBOMDetail.OrganizationID }) != null)
        //            {
        //                ExceptionManager.Raise(this.GetType(), "$Error_OPBOMDetail_Exist", String.Format("[$OPCode='{0}']", opBOMDetail.OPCode), null);
        //            }

        //            this.DataProvider.Insert(opBOMDetail);
        //        }
        //        this.DataProvider.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.DataProvider.RollbackTransaction();
        //        ExceptionManager.Raise(this.GetType(), "$Error_AssignBOMItemToOperation_Failure", String.Format("[$OPCode='{0}']", itemroute2OP.OPCode), ex);
        //    }
        //}
        public void AddOPBOMItem(OPBOMDetail opBOMDetail)
        {
            OPBOMChangedCheck(opBOMDetail.OPID, opBOMDetail.ItemCode, opBOMDetail.OPBOMCode, opBOMDetail.OPBOMVersion, opBOMDetail.OrganizationID, opBOMDetail.OPBOMItemSeq, false);
            this._helper.AddDomainObject(opBOMDetail);
        }

        /// <summary>
        /// ��opbom�ڹ����г��֣���״̬Ϊ����״̬������޸�
        /// �����д�opbomû�г�����Ҳ�����޸�
        /// ������Ͽ�����Ϣ���ڣ��������޸ģ�����һͬ�޸ģ�������ط���Ҫ�ʹ���
        /// ** nunit
        /// </summary>
        /// <param name="opBOMDetail"></param>
        public void UpdateOPBOMItem(OPBOMDetail opBOMDetail)
        {
            OPBOMChangedCheck(opBOMDetail.ItemCode, opBOMDetail.OPBOMCode, opBOMDetail.OPBOMVersion, opBOMDetail.OrganizationID, false);
            this._helper.UpdateDomainObject(opBOMDetail);
        }

        /// <summary>
        /// �����bom�ڹ�����û�г�����OPBOMDetail���Ա�ɾ����
        /// �����ͬʱ�������ϵĿ��ƣ���һ��ɾ��
        /// ** nunit
        /// </summary>
        /// <param name="opBOMDetail"></param>
        public void DeleteOPBOMItem(OPBOMDetail opBOMDetail)
        {
            if (opBOMDetail == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Null_Paramter");
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),string.Format(ErrorCenter.ERROR_ARGUMENT_NULL,"opBOMDetail")));
            }
            OPBOMChangedCheck(opBOMDetail.ItemCode, opBOMDetail.OPBOMCode, opBOMDetail.OPBOMVersion, opBOMDetail.OrganizationID, false);
            //opitemcontrol used?
            OPItemControlFacade _opitemControlFacade = new OPItemControlFacade(this.DataProvider);
            if (_opitemControlFacade.IsOPBOMItemControlExist(opBOMDetail.ItemCode, opBOMDetail.OPBOMItemCode, opBOMDetail.OPBOMCode, opBOMDetail.OPBOMVersion))
            {
                ExceptionManager.Raise(this.GetType(), "Error_OPBOMItemControl_Exist", String.Format("$OPBOMItemcode='{0}'", opBOMDetail.OPBOMItemCode), null);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),String.Format(ErrorCenter.ERROR_OPBOMITEMCONTROL,opBOMDetail.OPBOMItemCode)));
            }
            try
            {
                this.DataProvider.Delete(opBOMDetail);
            }
            catch (Exception ex)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_DeleteOPBOMItem", ex);
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade), String.Format(ErrorCenter.ERROR_DELETEOPBOMITEM,opBOMDetail.OPBOMItemCode)), ex);
            }
        }


        /// <summary>
        /// why public
        /// </summary>
        /// <param name="opBOMCode"></param>
        /// <param name="opBOMVersion"></param>
        public void OPBOMChangedCheck(string opID, string itemCode, string opBOMCode, string opBOMVersion, int orgID, int opBOMItemSeq, bool checkMO)
        {
            //����ϵͳ�����Ͷ���ĳ������ж��Ƿ���Ҫ���
            if (!FormatHelper.StringToBoolean(OPBOMCheckOption.ISOPBOMCHANGED_CHECK))
            {
                return;
            }

            //����Ƿ�����Ѿ�ʹ�ù���BOM��MO���Ѿ�����Ҫ��
            if (checkMO)
            {
                MOFacade _moFacade = new MOFacade(this._domainDataProvider);
                object[] objs = _moFacade.GetOPBOMUsedMOs(itemCode, opBOMCode, opBOMVersion, orgID);
                if (objs != null)
                {
                    string strMOS = ((MO)objs[0]).MOCode;

                    for (int i = 1; i < objs.Length; i++)
                    {
                        strMOS += "," + ((MO)objs[i]).MOCode;
                    }
                    ExceptionManager.Raise(this.GetType(), "$Error_OPBOMUsed", String.Format("[$MOCodes={0}]", strMOS), null);
                }
            }

            if (opID != null && opID != string.Empty && opBOMItemSeq >= 0)
            {
                object[] opBOMDetails = QueryOPBOMDetail(opID, itemCode, opBOMCode, opBOMVersion, orgID, opBOMItemSeq);
                if (opBOMDetails != null && opBOMDetails.Length > 0)
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_OPBOMItemSeqUsed", "", null);
                }
            }

        }

        public void OPBOMChangedCheck(string itemCode, string opBOMCode, string opBOMVersion, int orgID)
        {
            OPBOMChangedCheck(string.Empty, itemCode, opBOMCode, opBOMVersion, orgID, -1, true);
        }

        public void OPBOMChangedCheck(string itemCode, string opBOMCode, string opBOMVersion, int orgID, bool checkMO)
        {
            OPBOMChangedCheck(string.Empty, itemCode, opBOMCode, opBOMVersion, orgID, -1, checkMO);
        }


        /// <summary>
        /// ɾ��OPBOM��ϸ�����ϵ���Ʒ��Ϣ
        /// </summary>
        /// 		/// <param name="opBOMDetails"></param>
        public void DeleteOPBOMItem(OPBOMDetail[] opBOMDetails)
        {
            try
            {
                this.DataProvider.BeginTransaction();

                for (int i = 0; i < opBOMDetails.Length; i++)
                {
                    DeleteOPBOMItem(opBOMDetails[i]);
                }

                //Laws Lu,2006/09/01
                /*1,Ŀǰ����BOM�����߼����䣬������Ч��鹦�ܺ�ʧЧ���ܣ�
                         * ��ʼ�����Ĺ���BOM���ϴ���ʧЧ״̬,ͨ����Ч��������Ч״̬��
                         * ��ʱ�������޸ģ�ֻ��ʧЧ״̬�Ĺ���BOM�ſ����޸ġ�
                         * ��Ч����߼������������Ĺ���BOM�������ӽ����ϣ�����ϣ��������ĳ�������е��ѷ������ϴ��룬
                         * ���磬�������������а���5�����ϣ�����BOM�е��ӽ����ϱ���Ҳ�����������ϣ�
                         * ����ѡ�ϲ���������������֮����������ϡ�����Ĺ������û��ڽ���ָ����
                         * �������£�����������������A,B,C,D��������*/
                MOFacade moFac = (new MOFacade(DataProvider));

                OPBOMDetail opBOPDTL = opBOMDetails[0] as OPBOMDetail;
                object objOPBOM = moFac.GetOPBOM(opBOPDTL.ItemCode, opBOPDTL.OPBOMCode, opBOPDTL.OPBOMVersion, opBOPDTL.OrganizationID);

                if (objOPBOM != null)
                {
                    OPBOM opBOM = objOPBOM as OPBOM;

                    opBOM.Avialable = 0;

                    moFac.UpdateOPBOM(opBOM);
                }

                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                //_log.Error(ex.Message,ex);
                this.DataProvider.RollbackTransaction();
                ExceptionManager.Raise(this.GetType(), "$Error_DeleteOPBOMItem", ex);
            }
        }


        /// <summary>
        /// ����itemControlTypes��Ŀǰ����Ϊ��������������Ϊϵͳ�Ĳ���
        /// no need
        /// </summary>
        /// <returns>string������</returns>
        public string[] GetItemControlTypes()
        {
            string[] itemControls = new string[3];
            itemControls[0] = BOMItemControlType.ITEM_CONTROL_NOCONTROL;
            itemControls[1] = BOMItemControlType.ITEM_CONTROL_KEYPARTS;
            itemControls[2] = BOMItemControlType.ITEM_CONTROL_LOT;
            return itemControls;
        }

        public string[] GetOPBOMDetailTypes()
        {
            string[] itemControls = new string[3];
            itemControls[0] = OPBOMDetailParseType.PARSE_BARCODE;
            itemControls[1] = OPBOMDetailParseType.PARSE_PREPARE;
            itemControls[2] = OPBOMDetailParseType.PARSE_PRODUCT;
            return itemControls;
        }

        public string[] GetOPBOMDetailCheckTypes()
        {
            string[] itemControls = new string[2];
            itemControls[0] = OPBOMDetailCheckType.CHECK_LINKBARCODE;
            itemControls[1] = OPBOMDetailCheckType.CHECK_COMPAREITEM;
            return itemControls;
        }

        public object[] GetOPBOMDetails(string moCode, string routeCode, string opCode)
        {
            MOFacade moFacade = new MOFacade(this.DataProvider);
            object currentMO = moFacade.GetMO(moCode);
            if (currentMO == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MONotExisted");
            }
            OPBOM currentOPBOM = GetOPBOMByRouteCode(((MO)currentMO).ItemCode, routeCode, ((MO)currentMO).OrganizationID, ((MO)currentMO).BOMVersion);
            return this.QueryOPBOMDetail(currentOPBOM.ItemCode, string.Empty, currentOPBOM.OPBOMCode, currentOPBOM.OPBOMVersion, currentOPBOM.OPBOMRoute, opCode, int.MinValue, int.MaxValue, ((MO)currentMO).OrganizationID);
        }

        public object[] GetOPBOMDetails(string moCode, string routeCode, string opCode, bool check)
        {
            return GetOPBOMDetails(moCode, routeCode, opCode, check,false);
        }

        public object[] GetOPBOMDetails(string moCode, string routeCode, string opCode, bool check, bool onlyValid)
        {
            MOFacade moFacade = new MOFacade(this.DataProvider);
            object currentMO = moFacade.GetMO(moCode);
            if (currentMO == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MONotExisted");
            }
            OPBOM currentOPBOM = GetOPBOMByRouteCode(((MO)currentMO).ItemCode, routeCode, ((MO)currentMO).OrganizationID, ((MO)currentMO).BOMVersion);
            return this.QueryOPBOMDetail(currentOPBOM.ItemCode, string.Empty, currentOPBOM.OPBOMCode, currentOPBOM.OPBOMVersion, currentOPBOM.OPBOMRoute, opCode, int.MinValue, int.MaxValue, ((MO)currentMO).OrganizationID, check, onlyValid);
        }

        public object[] GetOPDropBOMDetails(string moCode, string routeCode, string opCode)
        {
            MOFacade moFacade = new MOFacade(this.DataProvider);
            object currentMO = moFacade.GetMO(moCode);
            if (currentMO == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MONotExisted");
            }
            OPBOM currentOPBOM = GetOPBOMByRouteCode(((MO)currentMO).ItemCode, routeCode, ((MO)currentMO).OrganizationID, ((MO)currentMO).BOMVersion);
            return this.QueryOPDropBOMDetail(currentOPBOM.ItemCode, string.Empty, currentOPBOM.OPBOMCode, currentOPBOM.OPBOMVersion, currentOPBOM.OPBOMRoute, opCode, ((MO)currentMO).OrganizationID);
        }

        /// <summary>
        /// ��ȡOPBOM���������ͺ����͸���
        /// 
        /// </summary>
        /// <param name="moCode">����</param>
        /// <param name="routeCode">;��</param>
        /// <param name="opCode">����</param>
        /// <param name="keyparts">KeyParts����</param>
        /// <param name="innos">�������ϸ���</param>
        /// <returns></returns>
        public string GetOPBOMDetailType(string moCode, string routeCode, string opCode, out int keyparts, out int innos)
        {
            keyparts = 0;

            innos = 0;

            MOFacade moFacade = new MOFacade(this.DataProvider);
            object currentMO = moFacade.GetMO(moCode);
            if (currentMO == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MONotExisted");
            }
            OPBOM currentOPBOM = GetOPBOMByRouteCode(((MO)currentMO).ItemCode, routeCode, ((MO)currentMO).OrganizationID, ((MO)currentMO).BOMVersion);

            object[] objs = null;
            objs = this.QueryOPBOMDetailType(currentOPBOM.ItemCode, currentOPBOM.OPBOMCode, currentOPBOM.OPBOMVersion, opCode, currentOPBOM.OPBOMRoute, ((MO)currentMO).OrganizationID);

            string strBOMType = String.Empty;

            string key = BOMItemControlType.ITEM_CONTROL_KEYPARTS;
            string inno = BOMItemControlType.ITEM_CONTROL_LOT;

            if (objs != null)
            {
                foreach (object obj in objs)
                {
                    OPBOMDetail bomDetail = (OPBOMDetail)obj;

                    bool linlBarcode = false;
                    if (bomDetail.OPBOMCheckType != null)
                    {
                        string opBOMCheckType = "," + bomDetail.OPBOMCheckType.ToLower() + ",";
                        linlBarcode = opBOMCheckType.IndexOf("," + OPBOMDetailCheckType.CHECK_LINKBARCODE.ToLower() + ",") >= 0;
                    }

                    if (bomDetail.OPBOMItemControlType == key)
                    {
                        if (linlBarcode)
                        {
                            keyparts = keyparts + Convert.ToInt32(1 * bomDetail.OPBOMItemQty);
                            strBOMType = strBOMType + bomDetail.OPBOMItemControlType + ";";
                        }
                    }
                    //Laws Lu,2005/11/05,�޸�	ֻ��ҪCheckһ��
                    if (bomDetail.OPBOMItemControlType == inno)
                    {
                        if (linlBarcode)
                        {
                            if (innos == 0)
                            {
                                strBOMType = strBOMType + inno + ";";
                            }
                            innos = 1;
                        }
                    }
                }
            }


            return strBOMType;
        }

        /// <summary>
        /// ��ȡOPBOM���������ͺ����͸���
        /// 
        /// </summary>
        /// <param name="moCode">����</param>
        /// <param name="routeCode">;��</param>
        /// <param name="opCode">����</param>
        /// <param name="keyparts">KeyParts����</param>
        /// <param name="innos">�������ϸ���</param>
        /// <returns></returns>
        public string GetOPDropBOMDetailType(string moCode, string routeCode, string opCode, out int keyparts, out int innos)
        {
            keyparts = 0;

            innos = 0;

            MOFacade moFacade = new MOFacade(this.DataProvider);
            object currentMO = moFacade.GetMO(moCode);
            if (currentMO == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MONotExisted");
            }
            OPBOM currentOPBOM = GetOPBOMByRouteCode(((MO)currentMO).ItemCode, routeCode, ((MO)currentMO).OrganizationID, ((MO)currentMO).BOMVersion);

            object[] objs = null;
            objs = this.QueryOPDropBOMDetailType(currentOPBOM.ItemCode, currentOPBOM.OPBOMCode, currentOPBOM.OPBOMVersion, opCode, currentOPBOM.OPBOMRoute, ((MO)currentMO).OrganizationID);

            string strBOMType = String.Empty;

            string key = BOMItemControlType.ITEM_CONTROL_KEYPARTS;
            string inno = BOMItemControlType.ITEM_CONTROL_LOT;

            if (objs != null)
            {
                foreach (object obj in objs)
                {
                    OPBOMDetail bomDetail = (OPBOMDetail)obj;

                    bool linlBarcode = false;
                    if (bomDetail.OPBOMCheckType != null)
                    {
                        string opBOMCheckType = "," + bomDetail.OPBOMCheckType.ToLower() + ",";
                        linlBarcode = opBOMCheckType.IndexOf("," + OPBOMDetailCheckType.CHECK_LINKBARCODE.ToLower() + ",") >= 0;
                    }

                    if (bomDetail.OPBOMItemControlType == key)
                    {
                        if (linlBarcode)
                        {
                            keyparts = keyparts + Convert.ToInt32(1 * bomDetail.OPBOMItemQty);
                            strBOMType = strBOMType + bomDetail.OPBOMItemControlType + ";";
                        }
                    }
                    //Laws Lu,2005/11/05,�޸�	ֻ��ҪCheckһ��
                    if (bomDetail.OPBOMItemControlType == inno)
                    {
                        if (linlBarcode)
                        {
                            if (innos == 0)
                            {
                                strBOMType = strBOMType + inno + ";";
                            }
                            innos = 1;
                        }
                    }
                }
            }


            return strBOMType;
        }


        /// <summary>
        /// ��ȡOPBOM���������ͺ����͸���
        /// 
        /// </summary>
        /// <param name="moCode">����</param>
        /// <param name="routeCode">;��</param>
        /// <param name="opCode">����</param>
        /// <param name="keyparts">KeyParts����</param>
        /// <param name="innos">�������ϸ���</param>
        /// <returns></returns>
        public string GetOPBOMDetailType(string moCode, string routeCode, string opCode, out int innos)
        {

            innos = 0;

            MOFacade moFacade = new MOFacade(this.DataProvider);
            object currentMO = moFacade.GetMO(moCode);
            if (currentMO == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MONotExisted");
            }
            OPBOM currentOPBOM = GetOPBOMByRouteCode(((MO)currentMO).ItemCode, routeCode, ((MO)currentMO).OrganizationID, ((MO)currentMO).BOMVersion);

            object[] objs = null;
            objs = this.QueryOPBOMDetailType(currentOPBOM.ItemCode, currentOPBOM.OPBOMCode, currentOPBOM.OPBOMVersion, opCode, currentOPBOM.OPBOMRoute, ((MO)currentMO).OrganizationID);

            string strBOMType = String.Empty;

            string inno = BOMItemControlType.ITEM_CONTROL_LOT;

            if (objs != null)
            {
                foreach (object obj in objs)
                {
                    OPBOMDetail bomDetail = (OPBOMDetail)obj;

                    bool linlBarcode = false;
                    if (bomDetail.OPBOMCheckType != null)
                    {
                        string opBOMCheckType = "," + bomDetail.OPBOMCheckType.ToLower() + ",";
                        linlBarcode = opBOMCheckType.IndexOf("," + OPBOMDetailCheckType.CHECK_LINKBARCODE.ToLower() + ",") >= 0;
                    }

                    //Laws Lu,2005/11/05,�޸�	ֻ��ҪCheckһ��
                    if (bomDetail.OPBOMItemControlType == inno)
                    {
                        if (linlBarcode)
                        {
                            if (innos == 0)
                            {
                                strBOMType = strBOMType + inno + ";";
                            }
                            innos = 1;
                        }
                    }
                }
            }


            return strBOMType;
        }

        public string GetOPDropBOMDetailType(string moCode, string routeCode, string opCode, out int innos)
        {


            innos = 0;

            MOFacade moFacade = new MOFacade(this.DataProvider);
            object currentMO = moFacade.GetMO(moCode);
            if (currentMO == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MONotExisted");
            }
            OPBOM currentOPBOM = GetOPBOMByRouteCode(((MO)currentMO).ItemCode, routeCode, ((MO)currentMO).OrganizationID, ((MO)currentMO).BOMVersion);

            object[] objs = null;
            objs = this.QueryOPDropBOMDetailType(currentOPBOM.ItemCode, currentOPBOM.OPBOMCode, currentOPBOM.OPBOMVersion, opCode, currentOPBOM.OPBOMRoute, ((MO)currentMO).OrganizationID);

            string strBOMType = String.Empty;

            string inno = BOMItemControlType.ITEM_CONTROL_LOT;

            if (objs != null)
            {
                foreach (object obj in objs)
                {
                    OPBOMDetail bomDetail = (OPBOMDetail)obj;

                    bool linlBarcode = false;
                    if (bomDetail.OPBOMCheckType != null)
                    {
                        string opBOMCheckType = "," + bomDetail.OPBOMCheckType.ToLower() + ",";
                        linlBarcode = opBOMCheckType.IndexOf("," + OPBOMDetailCheckType.CHECK_LINKBARCODE.ToLower() + ",") >= 0;
                    }

                    //Laws Lu,2005/11/05,�޸�	ֻ��ҪCheckһ��
                    if (bomDetail.OPBOMItemControlType == inno)
                    {
                        if (linlBarcode)
                        {
                            if (innos == 0)
                            {
                                strBOMType = strBOMType + inno + ";";
                            }
                            innos = 1;
                        }
                    }
                }
            }


            return strBOMType;
        }


        private object[] QueryOPBOMDetailType(string itemCode, string BOMCode, string BOMVersion, string OPCode, string routeCode, int orgID)
        {
            string selectSql = "select OBITEMCONTYPE,OBITEMQTY from tblopbomdetail where 1=1 {0} ";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode ='" + itemCode.Trim() + "'";
            }
            //			if((OPID != string.Empty)&&(OPID.Trim() != string.Empty))
            //			{
            //				tmpString += " and opid ='"+OPID.Trim()+"'";
            //			}
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + BOMVersion.Trim() + "'";
            }

            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and opcode ='" + OPCode.Trim() + "'";
            }

            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and opcode in (select opcode from tblitemroute2op where routecode ='" + routeCode.Trim() + "' and orgid=" + orgID + ")";
            }

            tmpString += " and actiontype = " + ((int)MaterialType.CollectMaterial).ToString();
            tmpString += " and orgid = " + orgID;
            tmpString += " group by OBSITEMCODE,obitemcontype,obitemqty";

            return this.DataProvider.CustomQuery(typeof(OPBOMDetail), new SQLCondition(String.Format(selectSql, tmpString)));

        }

        private object[] QueryOPDropBOMDetailType(string itemCode, string BOMCode, string BOMVersion, string OPCode, string routeCode, int orgID)
        {
            string selectSql = "select OBITEMCONTYPE,OBITEMQTY from tblopbomdetail where actiontype = " + (int)MaterialType.DropMaterial + " {0}";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode ='" + itemCode.Trim() + "'";
            }
            //			if((OPID != string.Empty)&&(OPID.Trim() != string.Empty))
            //			{
            //				tmpString += " and opid ='"+OPID.Trim()+"'";
            //			}
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + BOMVersion.Trim() + "'";
            }

            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and opcode ='" + OPCode.Trim() + "'";
            }

            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and opcode in (select opcode from tblitemroute2op where routecode ='" + routeCode.Trim() + "' and orgid=" + orgID + ")";
            }
            tmpString += " and orgid=" + orgID;

            return this.DataProvider.CustomQuery(typeof(OPBOMDetail), new SQLCondition(String.Format(selectSql, tmpString)));
        }

        public object[] GetLotControlOPBOMDetails(string moCode, string routeCode, string opCode, int orgID)
        {
            MOFacade moFacade = new MOFacade(this.DataProvider);
            object currentMO = moFacade.GetMO(moCode);
            if (currentMO == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MONotExisted");
            }
            OPBOM currentOPBOM = GetOPBOMByRouteCode(((MO)currentMO).ItemCode, routeCode, orgID, ((MO)currentMO).BOMVersion);
            return this.QueryLotControlOPBOMDetail(currentOPBOM.ItemCode, string.Empty, currentOPBOM.OPBOMCode, currentOPBOM.OPBOMVersion, currentOPBOM.OPBOMRoute, opCode, int.MinValue, int.MaxValue, orgID);
        }

        public object GetOPBOMDetail(string moCode, string routeCode, string opCode, string opBOMItemCode)
        {
            MOFacade moFacade = new MOFacade(this.DataProvider);
            object currentMO = moFacade.GetMO(moCode);
            if (currentMO == null)
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_MONotExisted");
            }
            OPBOM currentOPBOM = GetOPBOMByRouteCode(((MO)currentMO).ItemCode, routeCode, ((MO)currentMO).OrganizationID, ((MO)currentMO).BOMVersion);
            string selectSql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOMDetail)) + " from tblopbomdetail where itemcode=$itemcode and orgid=" + ((MO)currentMO).OrganizationID + " and obcode=$obcode and opbomver=$opbomver and obitemcode=$obitemcode and opcode=$opcode  and opcode in (select opcode from tblitemroute2op where routecode =$routecode and orgid=" + ((MO)currentMO).OrganizationID + ") order by obitemcode";
            object[] objs = this.DataProvider.CustomQuery(typeof(OPBOMDetail), new SQLParamCondition(selectSql, new SQLParameter[] {new SQLParameter("itemcode",typeof(string),currentOPBOM.ItemCode),new SQLParameter("obcode",typeof(string),currentOPBOM.OPBOMCode),
			                                                                                      new SQLParameter("opbomver",typeof(string),currentOPBOM.OPBOMVersion),new SQLParameter("obitemcode",typeof(string),opBOMItemCode),new SQLParameter("opcode",typeof(string),opCode),
			                                                                                      new SQLParameter("routecode",typeof(string),routeCode)}));
            if (objs != null)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }

        public object GetOPBOMDetailByBItemCode(string moCode, string opBOMItemCode)
        {
            string selectSql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOMDetail))
                + " from tblopbomdetail where obitemcode='" + opBOMItemCode + "'";

            if (moCode != String.Empty)
            {
                selectSql = selectSql + " and opcode in (select opcode from tblitemroute2op where routecode "
                    + "in ( select routecode  from tblmo2route where mocode='" + moCode + "') " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ") ";
            }
            selectSql += GlobalVariables.CurrentOrganizations.GetSQLCondition();
            selectSql += " order by obitemcode";
            object[] objs = this.DataProvider.CustomQuery(typeof(OPBOMDetail), new SQLCondition(selectSql));
            if (objs != null)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }

        public object[] GetOPBOMDetailByMo(MO mo, string mItemCode)
        {
            object objRoute = (new MOFacade(DataProvider)).GetMONormalRouteByMOCode(mo.MOCode);
            if (objRoute == null)
            {
                throw new Exception("$Error_MORoute_NotExist");
            }
            string selectSql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOMDetail)) + " from tblopbomdetail where itemcode=$itemcode and orgid=" + mo.OrganizationID + " and obitemcode=$obitemcode  and opcode in (select opcode from tblitemroute2op where routecode = $routecode and itemcode='"
                + mo.ItemCode + "' and orgid=" + mo.OrganizationID + ") order by obitemcode";
            object[] objs = this.DataProvider.CustomQuery(typeof(OPBOMDetail), new SQLParamCondition(selectSql, new SQLParameter[] {new SQLParameter("itemcode",typeof(string),mo.ItemCode),
																																	   new SQLParameter("obitemcode",typeof(string),mItemCode),
																																	   new SQLParameter("routecode",typeof(string),(objRoute as MO2Route).RouteCode)}));
            return objs;
            //			if(objs != null)
            //			{
            //				return objs[0];
            //			}
            //			else
            //			{
            //				return null;
            //			}
        }


        public object GetOPBOMDetail(string itemCode, string opBOMCode, string opBOMVersion, string opBOMItemCode, string routeCode, string opCode, int orgID)
        {
            object[] objs = this.QueryOPBOMDetail(itemCode, string.Empty, opBOMCode, opBOMVersion, routeCode, opCode, int.MinValue, int.MaxValue, orgID);
            if (objs != null)
            {
                return objs[0];
            }
            else
            {
                return null;
            }
        }


        public object[] QueryLotControlOPBOMDetail(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int inclusive, int exclusive, int orgID)
        {
            string selectSql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOMDetail)) + " from tblopbomdetail where  obitemcontype='"
                + BOMItemControlType.ITEM_CONTROL_LOT + "'  and actiontype = " + (int)MaterialType.CollectMaterial + " {0}";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode ='" + itemCode.Trim() + "'";
            }
            if ((OPID != string.Empty) && (OPID.Trim() != string.Empty))
            {
                tmpString += " and opid ='" + OPID.Trim() + "'";
            }
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + BOMVersion.Trim() + "'";
            }

            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and opcode ='" + OPCode.Trim() + "'";
            }

            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and opcode in (select opcode from tblitemroute2op where routecode ='" + routeCode.Trim() + "' and orgID=" + orgID + ")";
            }
            tmpString += " and orgid=" + orgID;
            return this.DataProvider.CustomQuery(typeof(OPBOMDetail), new PagerCondition(String.Format(selectSql, tmpString), inclusive, exclusive));
        }


        public object[] QueryOPBOMDetail(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int inclusive, int exclusive, int orgID)
        {
            return QueryOPBOMDetail(itemCode, OPID, BOMCode, BOMVersion, routeCode, OPCode, inclusive, exclusive, orgID,false);
        }

        public object[] QueryOPBOMDetail(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int inclusive, int exclusive, int orgID, bool check)
        {
            return QueryOPBOMDetail(itemCode, OPID, BOMCode, BOMVersion, routeCode, OPCode, inclusive, exclusive, orgID, check,false);
        }

        public object[] QueryOPBOMDetail(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int inclusive, int exclusive, int orgID, bool check, bool onlyValid)
        {
            string selectSql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOMDetail)) + " from tblopbomdetail where actiontype = " + (int)MaterialType.CollectMaterial + " {0}";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode ='" + itemCode.Trim() + "'";
            }
            if ((OPID != string.Empty) && (OPID.Trim() != string.Empty))
            {
                tmpString += " and opid ='" + OPID.Trim() + "'";
            }
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + BOMVersion.Trim() + "'";
            }

            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and opcode ='" + OPCode.Trim() + "'";
            }

            if (check == true)
            {
                tmpString += " and OBITEMCONTYPE='item_control_keyparts'";
            }

            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and opcode in (select opcode from tblitemroute2op where routecode ='" + routeCode.Trim() + "' and orgid=" + orgID + ")";
                tmpString += " and obcode in (select obcode from tblopbom where obroute ='" + routeCode.Trim() + "' and orgid=" + orgID + ") ";
            }

            if (onlyValid)
            {
                tmpString += " and tblopbomdetail.obvalid = 1 ";
            }
            tmpString += " and orgid=" + orgID;
            tmpString += "order by obitemseq";

            return this.DataProvider.CustomQuery(typeof(OPBOMDetail), new PagerCondition(String.Format(selectSql, tmpString), "OBITEMCODE", inclusive, exclusive));
        }

        public object[] QueryOPDropBOMDetail(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int orgID)
        {
            string selectSql = "select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(OPBOMDetail)) + " from tblopbomdetail where actiontype="
                + (int)MaterialType.DropMaterial + " {0}";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode ='" + itemCode.Trim() + "'";
            }
            if ((OPID != string.Empty) && (OPID.Trim() != string.Empty))
            {
                tmpString += " and opid ='" + OPID.Trim() + "'";
            }
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + BOMVersion.Trim() + "'";
            }

            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and opcode ='" + OPCode.Trim() + "'";
            }

            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and opcode in (select opcode from tblitemroute2op where routecode ='" + routeCode.Trim() + "' and orgid=" + orgID + ")";
            }
            tmpString += " and orgid=" + orgID;
            return this.DataProvider.CustomQuery(typeof(OPBOMDetail), new SQLCondition(String.Format(selectSql, tmpString)));
        }



        public int QueryOPBOMDetailCounts(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int orgID)
        {
            string selectSql = "select count(*) from tblopbomdetail where actiontype = " + (int)MaterialType.CollectMaterial + " {0}";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode ='" + itemCode.Trim() + "'";
            }
            if ((OPID != string.Empty) && (OPID.Trim() != string.Empty))
            {
                tmpString += " and opid ='" + OPID.Trim() + "'";
            }
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + BOMVersion.Trim() + "'";
            }
            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and opcode ='" + OPCode.Trim() + "'";
            }
            tmpString += " and orgid=" + orgID;
            return this.DataProvider.GetCount(new SQLCondition(String.Format(selectSql, tmpString)));
        }

        public object[] QueryOPBOMDetail(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int actiontype, int inclusive, int exclusive, int orgID)
        {
            return QueryOPBOMDetail(itemCode, OPID, BOMCode, BOMVersion, routeCode, OPCode, actiontype, inclusive, exclusive, orgID, false);
        }

        public object[] QueryOPBOMDetail(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int actiontype, int inclusive, int exclusive, int orgID, bool onlyValid)
        {
            string field = DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(OPBOMDetail));
            field = field.ToLower().Replace("tblopbomdetail.eattribute1", "tblmaterial.mdesc as eattribute1");

            string selectSql = "select " + field + " from tblopbomdetail left outer join tblmaterial ";
            selectSql += "on tblopbomdetail.orgid = tblmaterial.orgid ";
            selectSql += "and tblopbomdetail.obitemcode = tblmaterial.mcode ";
            selectSql += "where 1=1 ";
            if (actiontype >= 0)
            {
                selectSql += "and ActionType = " + actiontype.ToString() + " ";
            }
            selectSql += " {0}";

            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.itemcode ='" + itemCode.Trim() + "'";
            }
            if ((OPID != string.Empty) && (OPID.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opid ='" + OPID.Trim() + "'";
            }
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and tblopbomdetail.obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opbomver ='" + BOMVersion.Trim() + "'";
            }

            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opcode ='" + OPCode.Trim() + "'";
            }

            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opcode in (select opcode from tblitemroute2op where routecode ='" + routeCode.Trim() + "' and orgid=" + orgID + ") ";
                tmpString += " and tblopbomdetail.obcode in (select obcode from tblopbom where obroute ='" + routeCode.Trim() + "' and orgid=" + orgID + ") ";
            }
            tmpString += " and tblopbomdetail.orgid=" + orgID;
            if (onlyValid)
            {
                tmpString += " and tblopbomdetail.obvalid = 1 ";
            }

            return this.DataProvider.CustomQuery(typeof(OPBOMDetail), new PagerCondition(String.Format(selectSql, tmpString), "OBITEMSEQ,OBITEMCODE", inclusive, exclusive));
        }

        public object[] QueryOPBOMDetail_New(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int actiontype, int inclusive, int exclusive, int orgID, bool onlyValid, string moCode, string resourceCode, bool showZero)
        {
            string field = DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(OPBOMDetail));
            field = field.ToLower().Replace("tblopbomdetail.eattribute1", "tblmaterial.mdesc as eattribute1");

            string selectSql = "SELECT a.*,b.LOTNO, b.MITEMPACKEDNO, b.seq, b.lotqty, b.lotactqty FROM (";
            selectSql += "select " + field + " from tblopbomdetail left outer join tblmaterial ";
            selectSql += "on tblopbomdetail.orgid = tblmaterial.orgid ";
            selectSql += "and tblopbomdetail.obitemcode = tblmaterial.mcode ";
            selectSql += "where 1=1 ";
            if (actiontype >= 0)
            {
                selectSql += "and ActionType = " + actiontype.ToString() + " ";
            }
            selectSql += " {0}";

            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.itemcode ='" + itemCode.Trim() + "'";
            }
            if ((OPID != string.Empty) && (OPID.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opid ='" + OPID.Trim() + "'";
            }
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and tblopbomdetail.obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opbomver ='" + BOMVersion.Trim() + "'";
            }

            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opcode ='" + OPCode.Trim() + "'";
            }

            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opcode in (select opcode from tblitemroute2op where routecode ='" + routeCode.Trim() + "' and orgid=" + orgID + ") ";
                tmpString += " and tblopbomdetail.obcode in (select obcode from tblopbom where obroute ='" + routeCode.Trim() + "' and orgid=" + orgID + ") ";
            }
            tmpString += " and tblopbomdetail.orgid=" + orgID;
            if (onlyValid)
            {
                tmpString += " and tblopbomdetail.obvalid = 1 ";
            }
            selectSql += " ) a ";
            selectSql += @" LEFT JOIN (
                                    SELECT a.LOTNO, a.MITEMPACKEDNO, a.opbomver, a.mitemcode, a.mobsitemcode, a.opcode, a.seq, NVL (b.lotqty, 0) as lotqty, NVL (c.lotqty, 0) as lotactqty
                                            FROM tblminno a
                                     LEFT JOIN tblitemlot b
                                                ON a.mitempackedno = b.lotno
                                     LEFT JOIN tblstoragelotinfo c
                                                ON a.mitempackedno = c.lotno AND b.mcode = c.mcode ";
            selectSql += " WHERE 1=1 ";
            selectSql += " {1} ";

            string tmpStringMINNO = string.Empty;
            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpStringMINNO += " and a.ROUTECODE = '" + routeCode.Trim() + "' ";                
            }
            if ((moCode != string.Empty) && (moCode.Trim() != string.Empty))
            {
                tmpStringMINNO += " and a.MOCODE = '" + moCode.Trim() + "' ";
            }
            if ((resourceCode != string.Empty) && (resourceCode.Trim() != string.Empty))
            {
                tmpStringMINNO += " and a.RESCODE = '" + resourceCode.Trim() + "' ";
            }

            selectSql += " ) b ";
            selectSql += " ON a.OPCODE = b.OPCODE AND a.OBITEMCODE = b.MITEMCODE AND a.OBSITEMCODE = b.MOBSITEMCODE AND a.opbomver = b.opbomver ";

            if (!showZero)
            {
                selectSql += " AND (b.lotactqty is null or b.lotactqty <> 0) ";
            }

            return this.DataProvider.CustomQuery(typeof(OPBOMDetailAndMINNO), new PagerCondition(String.Format(selectSql, tmpString, tmpStringMINNO), "a.obsitemcode,a.obitemcode,b.seq", inclusive, exclusive));
        }

        //add By Jarvis For DeductQty 20120315
        public object[] QueryOPBOMDetail(string itemCode, string mCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int actiontype, int inclusive, int exclusive, int orgID, bool onlyValid)
        {
            string field = DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(OPBOMDetail));
            field = field.ToLower().Replace("tblopbomdetail.eattribute1", "tblmaterial.mdesc as eattribute1");

            string selectSql = "select " + field + " from tblopbomdetail left outer join tblmaterial ";
            selectSql += "on tblopbomdetail.orgid = tblmaterial.orgid ";
            selectSql += "and tblopbomdetail.obitemcode = tblmaterial.mcode ";
            selectSql += "where 1=1 ";
            if (actiontype >= 0)
            {
                selectSql += "and ActionType = " + actiontype.ToString() + " ";
            }
            selectSql += " {0}";

            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.itemcode ='" + itemCode.Trim() + "'";
            }
            if ((mCode != string.Empty) && (mCode.Trim() != string.Empty))
            {
                //modified by Gawain.Gu,2011-10-18,for��λ����֧�������
                tmpString += " and (tblopbomdetail.OBITEMCODE ='" + mCode.Trim()
                           + "' or tblopbomdetail.obsitemcode ='" + mCode.Trim() + "')";
            }
            if ((OPID != string.Empty) && (OPID.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opid ='" + OPID.Trim() + "'";
            }
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and tblopbomdetail.obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opbomver ='" + BOMVersion.Trim() + "'";
            }

            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opcode ='" + OPCode.Trim() + "'";
            }

            if ((routeCode != string.Empty) && (routeCode.Trim() != string.Empty))
            {
                tmpString += " and tblopbomdetail.opcode in (select opcode from tblitemroute2op where routecode ='" + routeCode.Trim() + "' and orgid=" + orgID + ") ";
                tmpString += " and tblopbomdetail.obcode in (select obcode from tblopbom where obroute ='" + routeCode.Trim() + "' and orgid=" + orgID + ") ";
            }
            tmpString += " and tblopbomdetail.orgid=" + orgID;
            if (onlyValid)
            {
                tmpString += " and tblopbomdetail.obvalid = 1 ";
            }

            return this.DataProvider.CustomQuery(typeof(OPBOMDetail), new PagerCondition(String.Format(selectSql, tmpString), "OBITEMSEQ,OBITEMCODE", inclusive, exclusive));
        }

        public int QueryOPBOMDetailCounts(string itemCode, string OPID, string BOMCode, string BOMVersion, string routeCode, string OPCode, int actiontype, int orgID)
        {
            string selectSql = "select count(*) from tblopbomdetail where 1= 1  and ActionType=" + actiontype + " {0}";
            string tmpString = string.Empty;
            if ((itemCode != string.Empty) && (itemCode.Trim() != string.Empty))
            {
                tmpString += " and itemcode ='" + itemCode.Trim() + "'";
            }
            if ((OPID != string.Empty) && (OPID.Trim() != string.Empty))
            {
                tmpString += " and opid ='" + OPID.Trim() + "'";
            }
            if ((BOMCode != string.Empty) && (BOMCode.Trim() != string.Empty))
            {
                tmpString += "  and obcode ='" + BOMCode.Trim() + "'";
            }
            if ((BOMVersion != string.Empty) && (BOMVersion.Trim() != string.Empty))
            {
                tmpString += " and opbomver ='" + BOMVersion.Trim() + "'";
            }
            if ((OPCode != string.Empty) && (OPCode.Trim() != string.Empty))
            {
                tmpString += " and opcode ='" + OPCode.Trim() + "'";
            }
            tmpString += " and orgid=" + orgID;
            return this.DataProvider.GetCount(new SQLCondition(String.Format(selectSql, tmpString)));
        }

        //public object GetOPBOMDetail(string itemCode, string OPID, string opBOMCode, string opBOMVersion, string opBOMItemCode, int actiontype, int orgID)
        //{
        //    object returnValue = this.DataProvider.CustomSearch(typeof(OPBOMDetail), new object[] { opBOMItemCode, itemCode, opBOMCode, opBOMVersion, OPID, actiontype, orgID });

        //    if (returnValue != null)
        //    {
        //        BenQGuru.eMES.Domain.MOModel.Material material = (BenQGuru.eMES.Domain.MOModel.Material)(new ItemFacade(this.DataProvider)).GetMaterial(((OPBOMDetail)returnValue).OPBOMSourceItemCode, orgID);
        //        if (material != null)
        //        {
        //            ((OPBOMDetail)returnValue).EAttribute1 = material.MaterialDescription == null ? "" : material.MaterialDescription;
        //        }
        //    }

        //    return returnValue;
        //}

        /// <summary>
        /// ** ��������:	�ж�OPBOMItemCode�Ƿ����
        ///					
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-06-20
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="opBOMItemCode">OPBOMItemCode</param>
        /// <returns>���ڷ���true�������ڷ���false</returns>
        public bool IsOPBOMItemExist(string opBOMItemCode)
        {
            if (this.DataProvider.GetCount(new SQLParamCondition("select count(*) from TBLOPBOMDETAIL where OBITEMCODE = $OBITEMCODE" + GlobalVariables.CurrentOrganizations.GetSQLCondition(), new SQLParameter[] { new SQLParameter("OBITEMCODE", typeof(string), opBOMItemCode) }))
                    > 0)
            {
                return true;
            }

            return false;
        }

        public bool OPBOMItemIsKeyPartsControl(string opBOMItemCode)
        {
            if (this.DataProvider.GetCount(new SQLParamCondition("select count(*) from TBLOPBOMDETAIL where OBITEMCODE = $OBITEMCODE and OBITEMCONTYPE='" + BOMItemControlType.ITEM_CONTROL_KEYPARTS + "'" + GlobalVariables.CurrentOrganizations.GetSQLCondition(), new SQLParameter[] { new SQLParameter("OBITEMCODE", typeof(string), opBOMItemCode) }))
                > 0)
            {
                return true;
            }

            return false;
        }

        private int GetMaxOPBOMItemSeq(string opID, string itemCode, string opBOMCode, string opBOMVersion, int orgID)
        {
            string selectSql = "SELECT NVL(MAX(MAX(obitemseq)),0) FROM tblopbomdetail WHERE 1= 1 ";
            selectSql += "AND opid = '" + opID.Trim() + "' ";
            selectSql += "AND itemcode = '" + itemCode.Trim() + "' ";
            selectSql += "AND obcode = '" + opBOMCode.Trim() + "' ";
            selectSql += "AND opbomver = '" + opBOMVersion.Trim() + "' ";
            selectSql += "AND orgid = " + orgID.ToString() + " ";
            selectSql += "GROUP BY opid, itemcode, obcode, opbomver, orgid ";

            return this.DataProvider.GetCount(new SQLCondition(selectSql));
        }

        public object[] QueryOPBOMDetail(string opID, string itemCode, string opBOMCode, string opBOMVersion, int orgID, int opBOMItemSeq)
        {
            string selectSql = "SELECT {0} FROM tblopbomdetail WHERE 1= 1 ";
            selectSql += "AND opid = '" + opID.Trim() + "' ";
            selectSql += "AND itemcode = '" + itemCode.Trim() + "' ";
            selectSql += "AND obcode = '" + opBOMCode.Trim() + "' ";
            selectSql += "AND opbomver = '" + opBOMVersion.Trim() + "' ";
            selectSql += "AND orgid = " + orgID.ToString() + " ";
            selectSql += "AND obitemseq = " + opBOMItemSeq.ToString() + " ";

            return this.DataProvider.CustomQuery(typeof(OPBOMDetail), new SQLCondition(String.Format(selectSql, DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(OPBOMDetail)))));
        }

        public object[] QueryOPBOMDetail(string opID, string itemCode, string opBOMCode, string opBOMVersion, int orgID, string mItemcode)
        {
            string selectSql = "SELECT {0} FROM tblopbomdetail WHERE 1= 1 ";
            selectSql += "AND opid = '" + opID.Trim() + "' ";
            selectSql += "AND itemcode = '" + itemCode.Trim() + "' ";
            selectSql += "AND obcode = '" + opBOMCode.Trim() + "' ";
            selectSql += "AND opbomver = '" + opBOMVersion.Trim() + "' ";
            selectSql += "AND orgid = " + orgID.ToString() + " ";
            selectSql += "AND obitemcode = '" + mItemcode.ToString() + "' ";
            //selectSql += "AND obitemcontype = 'item_control_lot' ";  
            return this.DataProvider.CustomQuery(typeof(OPBOMDetail), new SQLCondition(String.Format(selectSql, DomainObjectUtility.GetDomainObjectFieldsStringWithTableName(typeof(OPBOMDetail)))));
        }
        #endregion

        #region private method
        /// <summary>
        /// �жϸ�BOM�Ƿ��������Ʒ��ά��
        /// </summary>
        /// <param name="opBOM"></param>
        /// <returns></returns>
        private bool IsBOMComponentLoading(OPBOM opBOM)
        {
            if (opBOM == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_Null_Paramter");
                //				throw new RemotingException(ErrorCenter.GetErrorServerDescription(typeof(OPBOMFacade),String.Format(ErrorCenter.ERROR_ARGUMENT_NULL,"opBOM")));
            }
            object[] objs = QueryOPBOMDetail(string.Empty, string.Empty, opBOM.OPBOMCode, opBOM.OPBOMVersion, string.Empty, string.Empty, int.MinValue, int.MaxValue, opBOM.OrganizationID);
            if (objs == null)
            {
                return false;
            }
            return true;
        }
        #endregion

    }

}