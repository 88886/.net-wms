using System;
using System.Collections;
using UserControl;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Domain.Warehouse;

namespace BenQGuru.eMES.DataCollect.Action
{
    /// <summary>
    /// �ļ���:		ActionItem.cs
    /// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
    /// ������:		Mark Lee
    /// ��������:	2005-05-17 11:23:20
    /// �޸���:
    /// �޸�����:
    /// �� ��:	���ϲɼ�
    /// �� ��:	
    /// </summary>
    public class ActionItem : IActionWithStatus, IActionWithStatusNew
    {

        private IDomainDataProvider _domainDataProvider = null;
        private int seqForDeductQty = 0;//����ʱ��¼����˳��
        //		public ActionItem()
        //		{	
        //		}

        public ActionItem(IDomainDataProvider domainDataProvider)
        {
            this._domainDataProvider = domainDataProvider;
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
        //Laws Lu,2005/12/23,����INNO���ϼ�¼
        //Laws Lu,2006/01/06,�����¼Lot�ϵ���ϸ��¼ 
        public void InsertINNOOnWipItem(ActionEventArgs actionEventArgs, DataCollectFacade dataCollectFacade, object[] OPBOMDetail)
        {
            string iNNO = ((CINNOActionEventArgs)actionEventArgs).INNO;
            Simulation simulation = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulation;
            SimulationReport simulationReport = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulationReport;
            MaterialFacade material = new MaterialFacade(_domainDataProvider);
            //object[] mINNOs = material.GetLastMINNOs(iNNO);
            object[] mINNOs = OPBOMDetail;
            int i = 0;
            if (mINNOs == null)
            {
                throw new Exception("$CS_INNO_NOT_EXIST");
            }
            foreach (MINNO mINNO in mINNOs)
            {
                if (mINNO == null)
                    throw new Exception("$CS_INNOnotExist");
                if (mINNO.MOCode != simulation.MOCode)
                    throw new Exception("$CS_INNOnotForMO $CS_Param_MOCode=" + mINNO.MOCode);
                if (mINNO.RouteCode != simulation.RouteCode)
                    throw new Exception("$CS_INNOnotForRoute $CS_Param_RouteCode=" + mINNO.RouteCode);
                if (mINNO.OPCode != simulation.OPCode)
                    throw new Exception("$CS_INNOnotForOP $CS_Param_OPCode =" + mINNO.OPCode);
                if (mINNO.ResourceCode != simulation.ResourceCode)
                    throw new Exception("$CS_INNOnotForResource $CS_Param_ResourceCode=" + mINNO.ResourceCode);

                OnWIPItem wipItem = new OnWIPItem();
                wipItem.MCARD = mINNO.INNO;
                wipItem.BIOS = mINNO.BIOS;
                wipItem.DateCode = mINNO.DateCode;
                wipItem.LotNO = mINNO.LotNO;/*ActionOnLineHelper.StringNull;*/
                wipItem.MItemCode = mINNO.MItemCode;/*ActionOnLineHelper.StringNull;*/
                wipItem.PCBA = mINNO.PCBA;
                wipItem.VendorCode = mINNO.VendorCode;
                wipItem.VendorItemCode = mINNO.VendorItemCode;
                wipItem.Version = mINNO.Version;

                wipItem.EAttribute1 = simulation.EAttribute1;
                wipItem.ItemCode = simulation.ItemCode;
                wipItem.ResourceCode = simulation.ResourceCode;
                wipItem.RouteCode = simulation.RouteCode;
                wipItem.RunningCard = simulation.RunningCard;
                wipItem.RunningCardSequence = simulation.RunningCardSequence;
                wipItem.SegmentCode = simulationReport.SegmentCode;
                wipItem.ShiftCode = simulationReport.ShiftCode;
                wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                wipItem.TimePeriodCode = simulationReport.TimePeriodCode;
                wipItem.MOCode = simulation.MOCode;
                wipItem.ModelCode = simulation.ModelCode;
                wipItem.OPCode = simulation.OPCode;

                wipItem.MaintainDate = simulation.MaintainDate;
                wipItem.MaintainTime = simulation.MaintainTime;
                wipItem.MaintainUser = simulation.MaintainUser;

                wipItem.Qty = mINNO.Qty;
                wipItem.TransactionStatus = TransactionStatus.TransactionStatus_NO;
                wipItem.MCardType = MCardType.MCardType_INNO;
                wipItem.MSequence = i;
                //Laws Lu,2005/12/20,����	�ɼ�����
                wipItem.ActionType = (int)MaterialType.CollectMaterial;
                wipItem.MOSeq = simulation.MOSeq;   // Added by Icyer 2007/07/02

                dataCollectFacade.AddOnWIPItem(wipItem);

                //if (simulationRpt != null)
                //{
                //    simulationRpt.IsLoadedPart = FormatHelper.BooleanToString(true);
                //    simulationRpt.LoadedRCard = simulation.RunningCard;
                //    dataCollectFacade.UpdateSimulationReport(simulationRpt);
                //}
                i++;
            }
        }

        #region ����
        //add by Jarvis Chen 20120315

        public void UpdateStorageQty(StorageLotInfo lotInfo, InventoryFacade inventoryFacade, decimal qty)
        {
            object obj = inventoryFacade.GetStorageInfo(lotInfo.Storageid, lotInfo.Mcode, lotInfo.Stackcode);
            if (obj != null)
            {
                (obj as StorageInfo).Storageqty = (obj as StorageInfo).Storageqty - qty;
                //��ɿ��ϣ�ʣ������Ϊ0ɾ����¼��>0ʱ�������� Jarvis 20120316
                if ((obj as StorageInfo).Storageqty == 0)
                {
                    inventoryFacade.DeleteStorageInfo((obj as StorageInfo));
                }
                else
                {
                    inventoryFacade.UpdateStorageInfo(obj as StorageInfo);
                }
            }
        }

        public Messages DeductQty(ActionEventArgs actionEventArgs, DataCollectFacade dataCollectFacade, MINNO minno)
        {
            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            MaterialFacade materialFacade = new MaterialFacade(this.DataProvider);
            MOFacade moFacade = new MOFacade(this.DataProvider);
            OPBOMFacade opbomFacade = new OPBOMFacade(this.DataProvider);
            ItemFacade itemFacade = new ItemFacade(this.DataProvider);
            InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
            Simulation simulation = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulation;
            SimulationReport simulationReport = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulationReport;
            Messages returnValue = new Messages();
            string lotNoList = string.Empty;// add by Jarvis For onWipItem

            string type = "";
            if (minno.EAttribute1 == "1")
            {
                type = BOMItemControlType.ITEM_CONTROL_LOT;
            }
            if (minno.EAttribute1 == "0")
            {
                type = BOMItemControlType.ITEM_CONTROL_KEYPARTS;
            }

            Parameter objParameter = (Parameter)systemSettingFacade.GetParameter("DEDUCTQTY", "DEDUCTMATERIAL");

            if (objParameter == null || objParameter.ParameterAlias != "Y")
            {
                return returnValue;
            }

            ProductInfo productionInfo = actionEventArgs.ProductInfo;
            Simulation sim = actionEventArgs.ProductInfo.NowSimulation;            
            int orgid = actionEventArgs.ProductInfo.Resource.OrganizationID;
            MO mo = actionEventArgs.ProductInfo.CurrentMO;

            if (mo == null)
            {
                mo = moFacade.GetMO(sim.MOCode) as MO;
            }

            //��ȡ��ǰ������
            string moCode = productionInfo.NowSimulation.MOCode;
            //��ȡ��ǰ�����
            string opCode = productionInfo.NowSimulation.OPCode;
            //��ȡ��ǰ��Ʒ��
            string itemCode = productionInfo.NowSimulation.ItemCode;
            //��ȡ;�̴��� 
            string routeCode = productionInfo.NowSimulation.RouteCode;
            string resCode = productionInfo.Resource.ResourceCode;
            //��ȡORGID
            int orgID = productionInfo.Resource.OrganizationID;
            string moBomVer = string.Empty;

            object objMo = moFacade.GetMO(moCode);

            if (objMo != null)
            {
                moBomVer = (objMo as MO).BOMVersion;
            }

            //��ȡ��������
            string MItemName = string.Empty;
            Domain.MOModel.Material material = ((Domain.MOModel.Material)itemFacade.GetMaterial(minno.MItemCode, orgID));
            if (material != null)
            {
                //ע�� by sam 2016��2��25��13:14:36
                //MItemName = material.MaterialName;

            }

            //��Ӳ�Ʒ�����Ͽ����ж�  tblonwip
            string rCard = productionInfo.NowSimulation.RunningCard;
            decimal seq = productionInfo.NowSimulation.RunningCardSequence;
            //object[] objOnWip = dataCollectFacade.QueryOnWIP(rCard, moCode, opCode, "CINNO");

            //if (objOnWip != null && objOnWip.Length > 0)
            //{
            //    return returnValue;
            //}

            //remove by Jarvis ����鹤��BOM 20120321
            //object[] objMoBoms = moFacade.QueryMoBom(sim.ItemCode, minno.MItemCode, sim.MOCode);//��鹤��BOM�Ƿ��и���ѡ��, Jarvis 20120319
            //if (objMoBoms == null)
            //{
            //    throw new Exception("$CS_ItemCode[" + minno.MItemCode + "]" + "$Error_NotExistInMoBOM" + String.Format("[$MOCode='{0}']", sim.MOCode));
            //}

            object[] opbomObjs = opbomFacade.QueryOPBOMDetail(sim.ItemCode, minno.MItemCode, string.Empty, string.Empty, string.Empty, sim.RouteCode, opCode, (int)MaterialType.CollectMaterial, int.MinValue, int.MaxValue, orgid, true);
            if (opbomObjs == null)
            {
                throw new Exception("$CS_ItemCode[" + minno.MItemCode + "]" + "$Error_NotExistInOPBOM" + String.Format("[$ItemCode='{0}']", sim.ItemCode));
            }

            object[] moRouteObjs = moFacade.QueryMORoutes(sim.MOCode, sim.RouteCode);
            if (moRouteObjs == null)
            {
                throw new Exception("$Error_MORouteNOExist");
            }

            bool iflag = false;

            decimal iOPBOMItemQty = 0;
            //��Ҫ�ȶԵ��ӽ����Ϻ� �ȶԳɹ��� ֻ�ڹ�������BOM ��
            if (opbomObjs == null)//ȥ����鹤��BOM�Ƿ�Ϊ��
            {
                return returnValue;
            }

            //�Թ�����׼BOMΪ��׼,�ۼ���ǰ�����ĵ�����أ�tblmo. EATTRIBUTE2�������Ӧ�Ŀ����Ϣ��	

            //for (int n = 0; n < objMoBoms.Length; n++)//ȥ������BOM 20120321 Jarvis
            //{
                //���opbom�ж�Ӧ����Ʒ
                string TempMOBOMItemCode = minno.MItemCode;
                iflag = false;
                iOPBOMItemQty = 0;
                for (int j = 0; j < opbomObjs.Length; j++)
                {
                    if (TempMOBOMItemCode.ToUpper() == ((OPBOMDetail)opbomObjs[j]).OPBOMItemCode.ToUpper()
                        || TempMOBOMItemCode.ToUpper() == ((OPBOMDetail)opbomObjs[j]).OPBOMSourceItemCode.ToUpper())
                    {   //�ӽ��ϴ��ڻ�������Ͽ���
                        iflag = true;
                        //TempMOBOMItemCode = ((OPBOMDetail)opbomObjs[j]).OPBOMItemCode;//remove by Jarvis 20120316
                        //For ����ϣ���¼��ѡ�Ϻ�,Jarvis 20120316
                        //TempMOBOMItemCode = ((MOBOM)objMoBoms[n]).MOBOMItemCode;

                        if (((OPBOMDetail)opbomObjs[j]).OPBOMItemControlType == "item_control_lot")
                        {
                            iOPBOMItemQty = (decimal)((OPBOMDetail)opbomObjs[j]).OPBOMItemQty;

                            //��ȡ���������Jarvis 20120323���ȴ�TBLSMTRelationQty��ȡƴ�������������������TBLMO��ȡƴ�����
                            BenQGuru.eMES.SMT.SMTFacade smtFacade = new BenQGuru.eMES.SMT.SMTFacade(this._domainDataProvider);
                            object smtRelation = smtFacade.GetSMTRelationQty(simulation.RunningCard, moCode);
                            if (smtRelation != null)
                            {
                                iOPBOMItemQty *= ((BenQGuru.eMES.Domain.SMT.Smtrelationqty)smtRelation).Relationqtry;
                            }
                            else
                            {

                                iOPBOMItemQty *= (int)(((MO)objMo).IDMergeRule);
                            }
                        }
                        else if (((OPBOMDetail)opbomObjs[j]).OPBOMItemControlType == "item_control_keyparts")
                        {
                            iOPBOMItemQty = 1;
                        }
                        break;
                    }
                }

                //�ȶԳɹ����ӽ����Ϻ�һ��
                if (iflag)//�ӽ��ϲ����ڣ� ��ֻ�ڹ�����׼bom��
                {
                    //object[] objInfos = inventoryFacade.QueryStorageInfoByIDAndMCode(mo.EAttribute2, TempMOBOMItemCode.ToUpper());
                    object[] objInfos = inventoryFacade.QueryStorageInfoByIDAndMCode(TempMOBOMItemCode.ToUpper());
                    if (objInfos == null)
                    {
                        throw new Exception("$CS_ItemCode[" + minno.MItemCode + "]" + "$CS_StorageQty_ERROR");
                    }

                    //��ȡ���ϵ��ܿ������Jarvis 20120316
                    //decimal total = inventoryFacade.GetStorageQty(mo.EAttribute2, TempMOBOMItemCode.ToUpper());
                    decimal total = inventoryFacade.GetStorageQty(TempMOBOMItemCode.ToUpper());

                    if (total < iOPBOMItemQty)
                    {
                        throw new Exception("$CS_ItemCode[" + minno.MItemCode + "]" + "$CS_StorageQty_ERROR");
                    }

                    #region //���жϱ��������е��������������ۼ����˳�
                    decimal temlotQty = 0;
                    object[] objStorageLotInfo = null;
                    ArrayList StorageLotInfoList = new ArrayList();
                    
                    if (type == "item_control_keyparts")
                    {
                        //objStorageLotInfo = inventoryFacade.QueryStorageLot(minno.LotNO, mo.EAttribute2, TempMOBOMItemCode.ToUpper());
                        objStorageLotInfo = inventoryFacade.QueryStorageLot(minno.LotNO, TempMOBOMItemCode.ToUpper());

                        //��ȡ�ɿۼ�����������Jarvis 20120316
                        if (objStorageLotInfo != null)
                        {
                            foreach (StorageLotInfo lotInfo in objStorageLotInfo)
                            {
                                temlotQty += lotInfo.Lotqty;
                                if (lotInfo.Lotqty <= 0)//���������Ϊ0����¼����
                                {
                                    continue;
                                }
                                StorageLotInfoList.Add(lotInfo);
                            }
                        }
                    }
                    if (type == "item_control_lot")//��ȡ�����Ϣ��Jarvis 20120316
                    {
                        //��ȡ�ӽ��ϵı�����Ϣ��Jarvis 20120316����Seq����
                        object[] minnoss = materialFacade.QueryMINNO_New(moCode, routeCode, opCode, resCode, moBomVer, minno.MSourceItemCode);//��ȡͬһ��ѡ�ϵı�����Ϣ��Jarvis 20120321

                        //��ȡ������Ϣ�пɿۼ�����Jarvis 20120316
                        foreach (MINNO temp in minnoss)
                        {
                            //objStorageLotInfo = inventoryFacade.QueryStorageLot(temp.LotNO, mo.EAttribute2, temp.MItemCode);
                            objStorageLotInfo = inventoryFacade.QueryStorageLot(temp.LotNO, temp.MItemCode);
                            if (objStorageLotInfo != null)
                            {
                                foreach (StorageLotInfo lotInfo in objStorageLotInfo)
                                {
                                    temlotQty += lotInfo.Lotqty;
                                    if (lotInfo.Lotqty <= 0)//���������Ϊ0����¼����
                                    {
                                        continue;
                                    }
                                    StorageLotInfoList.Add(lotInfo);
                                }
                            }
                        }
                    }                    

                    if (temlotQty < iOPBOMItemQty)
                    {
                        throw new Exception("$CS_ItemCode[" + minno.MItemCode + "-" + MItemName + "]" + "$CS_DeductQty_ERROR");
                    }
                    #endregion

                    #region �������Կۼ�

                    foreach (StorageLotInfo lotInfo in StorageLotInfoList)
                    {                        

                        if (iOPBOMItemQty > lotInfo.Lotqty)
                        {
                            iOPBOMItemQty = iOPBOMItemQty - lotInfo.Lotqty;
                            inventoryFacade.DeleteStorageLotInfo(lotInfo);
                            this.UpdateStorageQty(lotInfo, inventoryFacade, lotInfo.Lotqty);
                            lotNoList += ("," + lotInfo.Lotno + ",");

                            #region ��¼������Ϣ
                            OnWIPItem wipItem = new OnWIPItem();
                            if (type == "item_control_lot")
                            {
                                MINNO minnoTemp = null;
                                object[] minnoTemps = materialFacade.QueryMINNO(moCode, routeCode, opCode, resCode, moBomVer, lotInfo.Mcode, minno.MSourceItemCode, lotInfo.Lotno);
                                if (minnoTemps != null)
                                {
                                    minnoTemp = (MINNO)minnoTemps[0];
                                }
                                wipItem.MCARD = minnoTemp.MItemPackedNo;
                                wipItem.BIOS = minnoTemp.BIOS;
                                wipItem.DateCode = minnoTemp.DateCode;
                                wipItem.LotNO = minnoTemp.LotNO;
                                wipItem.MItemCode = minnoTemp.MItemCode;
                                wipItem.PCBA = minnoTemp.PCBA;
                                wipItem.VendorCode = minnoTemp.VendorCode;
                                wipItem.VendorItemCode = minnoTemp.VendorItemCode;
                                wipItem.Version = minnoTemp.Version;
                                wipItem.MCardType = minno.EAttribute1;

                                wipItem.Qty = lotInfo.Lotqty;
                            }
                            else if (type == "item_control_keyparts")
                            {
                                wipItem.MCARD = minno.MItemPackedNo;
                                wipItem.BIOS = minno.BIOS;
                                wipItem.DateCode = minno.DateCode;
                                wipItem.LotNO = minno.LotNO;
                                wipItem.MItemCode = minno.MItemCode;
                                wipItem.PCBA = minno.PCBA;
                                wipItem.VendorCode = minno.VendorCode;
                                wipItem.VendorItemCode = minno.VendorItemCode;
                                wipItem.Version = minno.Version;
                                wipItem.MCardType = minno.EAttribute1;
                                wipItem.Qty = 1;
                            }                            

                            wipItem.EAttribute1 = simulation.EAttribute1;
                            wipItem.ItemCode = simulation.ItemCode;
                            wipItem.ResourceCode = simulation.ResourceCode;
                            wipItem.RouteCode = simulation.RouteCode;
                            wipItem.RunningCard = simulation.RunningCard;
                            wipItem.RunningCardSequence = simulation.RunningCardSequence;
                            wipItem.SegmentCode = simulationReport.SegmentCode;
                            wipItem.ShiftCode = simulationReport.ShiftCode;
                            wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                            wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                            wipItem.TimePeriodCode = simulationReport.TimePeriodCode;
                            wipItem.MOCode = simulation.MOCode;
                            wipItem.ModelCode = simulation.ModelCode;
                            wipItem.OPCode = simulation.OPCode;

                            wipItem.MaintainDate = simulation.MaintainDate;
                            wipItem.MaintainTime = simulation.MaintainTime;
                            wipItem.MaintainUser = simulation.MaintainUser;
                            
                            wipItem.TransactionStatus = TransactionStatus.TransactionStatus_NO;
                            wipItem.MSequence = seqForDeductQty;

                            wipItem.ActionType = (int)MaterialType.CollectMaterial;
                            wipItem.MOSeq = simulation.MOSeq;

                            dataCollectFacade.AddOnWIPItem(wipItem);

                            SimulationReport simulationRpt = dataCollectFacade.GetLastSimulationReport(wipItem.MCARD);
                            if (simulationRpt != null)
                            {
                                simulationRpt.IsLoadedPart = FormatHelper.BooleanToString(true);
                                simulationRpt.LoadedRCard = wipItem.RunningCard;
                                dataCollectFacade.UpdateSimulationReport(simulationRpt);

                                returnValue.AddMessages(dataCollectFacade.RemoveFromPallet(wipItem.MCARD, actionEventArgs.UserCode, true));                                

                                dataCollectFacade.RemoveFromCarton(wipItem.MCARD, actionEventArgs.UserCode);

                            }
                            seqForDeductQty++;
                            #endregion
                        }
                        else
                        {
                            lotInfo.Lotqty = lotInfo.Lotqty - iOPBOMItemQty;
                            inventoryFacade.UpdateStorageLotInfo(lotInfo);
                            this.UpdateStorageQty(lotInfo, inventoryFacade, iOPBOMItemQty);                            

                            #region ��¼������Ϣ
                            OnWIPItem wipItem = new OnWIPItem();
                            if (type == "item_control_lot")
                            {
                                MINNO minnoTemp = null;
                                object[] minnoTemps = materialFacade.QueryMINNO(moCode, routeCode, opCode, resCode, moBomVer, lotInfo.Mcode, minno.MSourceItemCode, lotInfo.Lotno);
                                if (minnoTemps != null)
                                {
                                    minnoTemp = (MINNO)minnoTemps[0];
                                }
                                wipItem.MCARD = minnoTemp.MItemPackedNo;
                                wipItem.BIOS = minnoTemp.BIOS;
                                wipItem.DateCode = minnoTemp.DateCode;
                                wipItem.LotNO = minnoTemp.LotNO;
                                wipItem.MItemCode = minnoTemp.MItemCode;
                                wipItem.PCBA = minnoTemp.PCBA;
                                wipItem.VendorCode = minnoTemp.VendorCode;
                                wipItem.VendorItemCode = minnoTemp.VendorItemCode;
                                wipItem.Version = minnoTemp.Version;
                                wipItem.MCardType = minno.EAttribute1;

                                wipItem.Qty = iOPBOMItemQty;
                            }
                            else if (type == "item_control_keyparts")
                            {
                                wipItem.MCARD = minno.MItemPackedNo;
                                wipItem.BIOS = minno.BIOS;
                                wipItem.DateCode = minno.DateCode;
                                wipItem.LotNO = minno.LotNO;
                                wipItem.MItemCode = minno.MItemCode;
                                wipItem.PCBA = minno.PCBA;
                                wipItem.VendorCode = minno.VendorCode;
                                wipItem.VendorItemCode = minno.VendorItemCode;
                                wipItem.Version = minno.Version;
                                wipItem.MCardType = minno.EAttribute1;
                                wipItem.Qty = 1;
                            }

                            wipItem.EAttribute1 = simulation.EAttribute1;
                            wipItem.ItemCode = simulation.ItemCode;
                            wipItem.ResourceCode = simulation.ResourceCode;
                            wipItem.RouteCode = simulation.RouteCode;
                            wipItem.RunningCard = simulation.RunningCard;
                            wipItem.RunningCardSequence = simulation.RunningCardSequence;
                            wipItem.SegmentCode = simulationReport.SegmentCode;
                            wipItem.ShiftCode = simulationReport.ShiftCode;
                            wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                            wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                            wipItem.TimePeriodCode = simulationReport.TimePeriodCode;
                            wipItem.MOCode = simulation.MOCode;
                            wipItem.ModelCode = simulation.ModelCode;
                            wipItem.OPCode = simulation.OPCode;

                            wipItem.MaintainDate = simulation.MaintainDate;
                            wipItem.MaintainTime = simulation.MaintainTime;
                            wipItem.MaintainUser = simulation.MaintainUser;

                            wipItem.TransactionStatus = TransactionStatus.TransactionStatus_NO;
                            wipItem.MSequence = seqForDeductQty;

                            wipItem.ActionType = (int)MaterialType.CollectMaterial;
                            wipItem.MOSeq = simulation.MOSeq;

                            dataCollectFacade.AddOnWIPItem(wipItem);

                            SimulationReport simulationRpt = dataCollectFacade.GetLastSimulationReport(wipItem.MCARD);
                            if (simulationRpt != null)
                            {
                                simulationRpt.IsLoadedPart = FormatHelper.BooleanToString(true);
                                simulationRpt.LoadedRCard = wipItem.RunningCard;
                                dataCollectFacade.UpdateSimulationReport(simulationRpt);

                                returnValue.AddMessages(dataCollectFacade.RemoveFromPallet(wipItem.MCARD, actionEventArgs.UserCode, true));                                

                                dataCollectFacade.RemoveFromCarton(wipItem.MCARD, actionEventArgs.UserCode);

                            }
                            seqForDeductQty++;
                            #endregion
                            iOPBOMItemQty = 0;
                            lotNoList += ("," + lotInfo.Lotno + ",");


                            #region ���ݲ����趨����ۼ��ǹܿ���

                            //���ϲ����� ��Ӧ��������ALERTDISER��ALERTDISFLAG��ALERTDISNORMAL��ALERTDISOP
                            string paramGroup = "AlertMaterialDisGroup";
                            string isDisMaterial = string.Empty;
                            string alertOPCode = string.Empty;
                            decimal normalAlert = 0;
                            decimal erAlert = 0;

                            object[] paramAlertList = systemSettingFacade.GetParametersByParameterGroup(paramGroup.ToUpper());
                            foreach (Parameter param in paramAlertList)
                            {
                                decimal tmpTime = 0;
                                if (param.ParameterCode == "ALERTDISFLAG")
                                {
                                    isDisMaterial = param.ParameterAlias.ToUpper();
                                }
                                else if (param.ParameterCode == "ALERTDISOP")
                                {
                                    alertOPCode = param.ParameterAlias.ToUpper();
                                }
                                else if (param.ParameterCode == "ALERTDISNORMAL")
                                {
                                    if (decimal.TryParse(param.ParameterAlias, out tmpTime))
                                        normalAlert = tmpTime;
                                }
                                else if (param.ParameterCode == "ALERTDISER")
                                {
                                    if (decimal.TryParse(param.ParameterAlias, out tmpTime))
                                        erAlert = tmpTime;
                                }
                            }

                            if (isDisMaterial == "Y")//��Ҫ����
                            {
                                //MOFacade moFacade = new MOFacade(this.DataProvider);

                                //��ȡ������ϸ��Ϣ
                                object[] disDetailObjs = dataCollectFacade.QueryDisToLineDetailForMType(
                                                                                 simulationReport.OPCode,
                                                                                 simulationReport.MOCode,
                                                                                 simulationReport.StepSequenceCode,
                                                                                 "'item_control_keyparts','item_control_lot'");

                                if (disDetailObjs != null)
                                {
                                    //����DisToLineDetail,�ҳ��͹���bom������Ϻţ������ݹ��������ۼ���
                                    foreach (DisToLineDetail disDetail in disDetailObjs)
                                    {
                                        if (disDetail.MCode == lotInfo.Mcode)
                                        {
                                            //�ۼ�����
                                            if (type == BOMItemControlType.ITEM_CONTROL_LOT)
                                            {
                                                disDetail.MssleftQty = disDetail.MssleftQty - iOPBOMItemQty;
                                                disDetail.MaintainDate = simulationReport.MaintainDate;
                                                disDetail.MaintainTime = simulationReport.MaintainTime;
                                                disDetail.MaintainUser = simulationReport.MaintainUser;
                                                dataCollectFacade.UpdateDistolinedetail(disDetail);
                                            }
                                            else if (type == BOMItemControlType.ITEM_CONTROL_KEYPARTS)
                                            {
                                                disDetail.MssleftQty = disDetail.MssleftQty - 1;
                                                disDetail.MaintainDate = simulationReport.MaintainDate;
                                                disDetail.MaintainTime = simulationReport.MaintainTime;
                                                disDetail.MaintainUser = simulationReport.MaintainUser;
                                                dataCollectFacade.UpdateDistolinedetail(disDetail);
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion
  

                            break;
                        }


                    }
                    
                    #endregion
                }
            //}                
                return returnValue;
        }

        #endregion

        public Messages InsertLotOnWipItem(ActionEventArgs actionEventArgs, DataCollectFacade dataCollectFacade, object[] OPBOMDetail)
        {
            Messages returnValue = new Messages();

            string iNNO = ((CINNOActionEventArgs)actionEventArgs).INNO;
            Simulation simulation = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulation;
            SimulationReport simulationReport = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulationReport;
            MaterialFacade material = new MaterialFacade(_domainDataProvider);
            string itemCodeList = string.Empty;//add by Jarvis for ��ջ���� 20120316
            string lotNoList = string.Empty;// add by Jarvis For onWipItem 20120319
            //object[] mINNOs = material.GetLastMINNOs(iNNO);
            object[] mINNOs = OPBOMDetail;
            int i = 0;
            if (mINNOs == null)
            {
                throw new Exception("$CS_INNO_NOT_EXIST");
            }

            //ȷ���Ƿ���Ҫ����
            SystemSettingFacade systemSettingFacade = new SystemSettingFacade(this.DataProvider);
            Parameter objParameter = (Parameter)systemSettingFacade.GetParameter("DEDUCTQTY", "DEDUCTMATERIAL");
            bool isDeductQty = true;
            if (objParameter == null || objParameter.ParameterAlias != "Y")
            {
                isDeductQty = false;
            }

            foreach (MINNO mINNO in mINNOs)
            {
                if (mINNO == null)
                    throw new Exception("$CS_INNOnotExist");
                //if (mINNO.MOCode != simulation.MOCode)
                //    throw new Exception("$CS_INNOnotForMO $CS_Param_MOCode=" + mINNO.MOCode);
                //if (mINNO.RouteCode != simulation.RouteCode)
                //    throw new Exception("$CS_INNOnotForRoute $CS_Param_RouteCode=" + mINNO.RouteCode);
                //if (mINNO.OPCode != simulation.OPCode)
                //    throw new Exception("$CS_INNOnotForOP $CS_Param_OPCode =" + mINNO.OPCode);
                //if (mINNO.ResourceCode != simulation.ResourceCode)
                //    throw new Exception("$CS_INNOnotForResource $CS_Param_ResourceCode=" + mINNO.ResourceCode);

                OPBOMFacade opbomFacade = new OPBOMFacade(this.DataProvider);
                Domain.MOModel.OPBOMDetail opbomdetail = opbomFacade.GetOPBOMDetail(mINNO.MOCode, (actionEventArgs.OnWIP[0] as OnWIP).RouteCode, (actionEventArgs.OnWIP[0] as OnWIP).OPCode, mINNO.MItemCode) as Domain.MOModel.OPBOMDetail;

                //����ܿ������ǵ����ܿز��ҽ�����ʽ��������Ϣ������Ҫ���п��϶�����added by Gawain.Gu@20130417
                if (opbomdetail != null && opbomdetail.OPBOMParseType == OPBOMDetailParseType.PARSE_PRODUCT && opbomdetail.OPBOMItemControlType == BOMItemControlType.ITEM_CONTROL_KEYPARTS)
                {
                    isDeductQty = false;
                }

                if (!isDeductQty)//���������ֻ��¼ͬһ��ѡ�ϵ�һ�ʹ���
                {
                    //ԭ�й��˼�¼��Ų���˴�
                    OnWIPItem wipItem = new OnWIPItem();
                    wipItem.MCARD = mINNO.MItemPackedNo;
                    wipItem.BIOS = mINNO.BIOS;
                    wipItem.DateCode = mINNO.DateCode;
                    wipItem.LotNO = mINNO.LotNO;
                    wipItem.MItemCode = mINNO.MItemCode;
                    wipItem.PCBA = mINNO.PCBA;
                    wipItem.VendorCode = mINNO.VendorCode;
                    wipItem.VendorItemCode = mINNO.VendorItemCode;
                    wipItem.Version = mINNO.Version;

                    wipItem.MCardType = mINNO.EAttribute1;

                    wipItem.EAttribute1 = simulation.EAttribute1;
                    wipItem.ItemCode = simulation.ItemCode;
                    wipItem.ResourceCode = simulation.ResourceCode;
                    wipItem.RouteCode = simulation.RouteCode;
                    wipItem.RunningCard = simulation.RunningCard;
                    wipItem.RunningCardSequence = simulation.RunningCardSequence;
                    wipItem.SegmentCode = simulationReport.SegmentCode;
                    wipItem.ShiftCode = simulationReport.ShiftCode;
                    wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                    wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                    wipItem.TimePeriodCode = simulationReport.TimePeriodCode;
                    wipItem.MOCode = simulation.MOCode;
                    wipItem.ModelCode = simulation.ModelCode;
                    wipItem.OPCode = simulation.OPCode;

                    wipItem.MaintainDate = simulation.MaintainDate;
                    wipItem.MaintainTime = simulation.MaintainTime;
                    wipItem.MaintainUser = simulation.MaintainUser;

                    if (mINNO.Qty.ToString() != string.Empty && Convert.ToInt32(mINNO.Qty) != 0)
                    {
                        wipItem.Qty = mINNO.Qty;
                    }
                    else
                    {
                        wipItem.Qty = 1;
                    }
                    wipItem.TransactionStatus = TransactionStatus.TransactionStatus_NO;
                    wipItem.MSequence = i;
                    //Laws Lu,2005/12/20,����	�ɼ�����
                    wipItem.ActionType = (int)MaterialType.CollectMaterial;
                    wipItem.MOSeq = simulation.MOSeq;   // Added by Icyer 2007/07/02

                    //��֤��tblonwipitem�У�ÿ��Key Partֻ��һ������
                    //if (wipItem.MCardType == MCardType.MCardType_Keyparts)
                    //{
                    //    object[] oldOnWIPItem = dataCollectFacade.QueryOnWIPItem(wipItem.MCARD, wipItem.MItemCode);
                    //    if (oldOnWIPItem != null)
                    //    {
                    //        foreach (OnWIPItem item in oldOnWIPItem)
                    //        {
                    //            dataCollectFacade.DeleteOnWIPItem(item);
                    //        }
                    //    }
                    //}

                    dataCollectFacade.AddOnWIPItem(wipItem);

                    SimulationReport simulationRpt = dataCollectFacade.GetLastSimulationReport(wipItem.MCARD);
                    if (simulationRpt != null)
                    {
                        simulationRpt.IsLoadedPart = FormatHelper.BooleanToString(true);
                        simulationRpt.LoadedRCard = wipItem.RunningCard;
                        dataCollectFacade.UpdateSimulationReport(simulationRpt);

                        // Added By Hi1/Venus.feng on 20081114 for Hisense Version : Add Remove Pallet and Carton
                        // ����Ͳ�Pallet                    
                        returnValue.AddMessages(dataCollectFacade.RemoveFromPallet(wipItem.MCARD, actionEventArgs.UserCode, true));
                        if (returnValue.IsSuccess())
                        {
                            returnValue.ClearMessages();
                        }
                        else
                        {
                            return returnValue;
                        }

                        dataCollectFacade.RemoveFromCarton(wipItem.MCARD, actionEventArgs.UserCode);
                        // End Added
                    }
                    i++;
                }
                else
                {
                    //add by Jarvis 20120316  �����Ͽ���ʱ����tblitemlotdetail.serialstatus =��UNSTORAGE��
                    MOFacade moFacade = new MOFacade(this.DataProvider);
                    ItemFacade itemFacade = new ItemFacade(this.DataProvider);
                    InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
                    MO _mo = (MO)moFacade.GetMO(mINNO.MOCode);
                    Domain.MOModel.Material materialNew = (Domain.MOModel.Material)itemFacade.GetMaterial(mINNO.MItemCode, _mo.OrganizationID);
                    if (materialNew != null)
                    {
                        //ע�� by sam 2016��2��25��13:14:36
                        //if (materialNew.MaterialControlType == "item_control_keyparts")
                        //{
                        //    ItemLotDetail itemLotDetail = (ItemLotDetail)inventoryFacade.GetItemLotDetail(mINNO.MItemPackedNo, mINNO.MItemCode);
                        //    if (itemLotDetail == null || itemLotDetail.Serialstatus != "STORAGE")
                        //    {
                        //        throw new Exception("$CS_Error_SerialNotInStorage:" + mINNO.MItemPackedNo);
                        //    }
                        //    else
                        //    {
                        //        itemLotDetail.Serialstatus = "UNSTORAGE";
                        //        inventoryFacade.UpdateItemLotDetail(itemLotDetail);
                        //    }
                        //}
                    }

                    //add by Jarvis 20120316 For ����
                    returnValue = DeductQty(actionEventArgs, dataCollectFacade, mINNO);
                    if (returnValue.IsSuccess())
                    {
                        returnValue.ClearMessages();
                    }
                    else
                    {
                        seqForDeductQty = 0;
                        return returnValue;
                    }                    
                }
            }
            seqForDeductQty = 0;
            return returnValue;
        }

        //Laws Lu,2005/12/23,����KeyParts���ϼ�¼
        public void InsertKeyPartOnWipItem(ActionEventArgs actionEventArgs, DataCollectFacade dataCollectFacade)
        {
            OPBomKeyparts keyParts = ((CKeypartsActionEventArgs)actionEventArgs).Keyparts;
            Simulation simulation = ((CKeypartsActionEventArgs)actionEventArgs).ProductInfo.NowSimulation;
            SimulationReport simulationReport = ((CKeypartsActionEventArgs)actionEventArgs).ProductInfo.NowSimulationReport;
            int n = keyParts.GetbomKeypartCount();
            for (int i = 0; i < n; i++)
            {
                MKeyPart keypart = keyParts.GetKeypart(i);
                OnWIPItem wipItem = new OnWIPItem();
                wipItem.BIOS = keypart.BIOS;
                wipItem.DateCode = keypart.DateCode;
                wipItem.LotNO = keypart.LotNO;
                wipItem.MCARD = keypart.Keypart;
                wipItem.MItemCode = keypart.MItemCode;
                wipItem.PCBA = keypart.PCBA;
                wipItem.VendorCode = keypart.VendorCode;
                wipItem.VendorItemCode = keypart.VendorItemCode;
                wipItem.Version = keypart.Version;
                wipItem.EAttribute1 = simulation.EAttribute1;
                wipItem.ItemCode = simulation.ItemCode;
                wipItem.ResourceCode = simulation.ResourceCode;
                wipItem.RouteCode = simulation.RouteCode;
                wipItem.RunningCard = simulation.RunningCard;
                wipItem.RunningCardSequence = simulation.RunningCardSequence;
                wipItem.SegmentCode = simulationReport.SegmentCode;
                wipItem.ShiftCode = simulationReport.ShiftCode;
                wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                wipItem.TimePeriodCode = simulationReport.TimePeriodCode;
                wipItem.MOCode = simulation.MOCode;
                wipItem.ModelCode = simulation.ModelCode;
                wipItem.OPCode = simulation.OPCode;

                wipItem.MaintainDate = simulation.MaintainDate;
                wipItem.MaintainTime = simulation.MaintainTime;
                wipItem.MaintainUser = simulation.MaintainUser;

                wipItem.Qty = 1;
                wipItem.TransactionStatus = TransactionStatus.TransactionStatus_NO;
                wipItem.MCardType = MCardType.MCardType_Keyparts;
                wipItem.MSequence = i;
                //Laws Lu,2005/12/20,����	�ɼ�����
                wipItem.ActionType = (int)MaterialType.CollectMaterial;
                wipItem.MOSeq = simulation.MOSeq;   // Added by Icyer 2007/07/02

                dataCollectFacade.AddOnWIPItem(wipItem);

                // Added by Icyer 2006/11/07
                // �޸�KeyPartʹ��
                if (FormatHelper.StringToBoolean(keyParts.GetOPBomDetail(wipItem.MItemCode).OpBomDetial.CheckStatus) == true)
                {
                    SimulationReport simulationRpt = dataCollectFacade.GetLastSimulationReport(keypart.Keypart);
                    if (simulationRpt != null)
                    {
                        simulationRpt.IsLoadedPart = FormatHelper.BooleanToString(true);
                        simulationRpt.LoadedRCard = simulation.RunningCard;
                        dataCollectFacade.UpdateSimulationReport(simulationRpt);
                    }
                }
                // Added end

                ////add by Jarvis 20120316  �����Ͽ���ʱ����tblitemlotdetail.serialstatus =��UNSTORAGE��
                //InventoryFacade inventoryFacade = new InventoryFacade(this.DataProvider);
                //ItemLotDetail itemLotDetail = (ItemLotDetail)inventoryFacade.GetItemLotDetail(keypart.Keypart, keypart.MItemCode);
                //if (itemLotDetail == null || itemLotDetail.Serialstatus != "STORAGE")
                //{
                //    throw new Exception("$CS_Error_SerialNotInStorage:" + keypart.Keypart);
                //}
                //else
                //{
                //    itemLotDetail.Serialstatus = "UNSTORAGE";
                //    inventoryFacade.UpdateItemLotDetail(itemLotDetail);
                //}
                ////end add

                ////add by Jarvis 20120316 For ����
                //MINNO minno = new MINNO();
                //minno.LotNO = wipItem.LotNO;
                //minno.MSourceItemCode = wipItem.MItemCode;
                //minno.MItemCode = wipItem.MItemCode;
                //minno.EAttribute1 = "0";
                //DeductQty(actionEventArgs, dataCollectFacade, minno);
            }
        }

        //Laws Lu,2005/12/23,����INNO���ϼ�¼
        //Laws Lu,2006/01/06,�����¼Lot�ϵ���ϸ��¼
        public void InsertINNOOnWipItem(ActionEventArgs actionEventArgs, DataCollectFacade dataCollectFacade)
        {
            string iNNO = ((CINNOActionEventArgs)actionEventArgs).INNO;
            Simulation simulation = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulation;
            SimulationReport simulationReport = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulationReport;
            MaterialFacade material = new MaterialFacade(_domainDataProvider);
            object[] mINNOs = material.GetLastMINNOs(iNNO);
            int i = 0;
            if (mINNOs == null)
            {
                throw new Exception("$CS_INNO_NOT_EXIST");
            }
            foreach (MINNO mINNO in mINNOs)
            {
                if (mINNO == null)
                    throw new Exception("$CS_INNOnotExist");
                if (mINNO.MOCode != simulation.MOCode)
                    throw new Exception("$CS_INNOnotForMO $CS_Param_MOCode=" + mINNO.MOCode);
                if (mINNO.RouteCode != simulation.RouteCode)
                    throw new Exception("$CS_INNOnotForRoute $CS_Param_RouteCode=" + mINNO.RouteCode);
                if (mINNO.OPCode != simulation.OPCode)
                    throw new Exception("$CS_INNOnotForOP $CS_Param_OPCode =" + mINNO.OPCode);
                if (mINNO.ResourceCode != simulation.ResourceCode)
                    throw new Exception("$CS_INNOnotForResource $CS_Param_ResourceCode=" + mINNO.ResourceCode);

                OnWIPItem wipItem = new OnWIPItem();
                wipItem.MCARD = iNNO;
                wipItem.BIOS = mINNO.BIOS;
                wipItem.DateCode = mINNO.DateCode;
                wipItem.LotNO = mINNO.MItemPackedNo;//.LotNO;
                wipItem.MItemCode = mINNO.MItemCode;/*ActionOnLineHelper.StringNull;*/
                wipItem.PCBA = mINNO.PCBA;
                wipItem.VendorCode = mINNO.VendorCode;
                wipItem.VendorItemCode = mINNO.VendorItemCode;
                wipItem.Version = mINNO.Version;

                //				wipItem.DateCode =ActionOnLineHelper.StringNull;
                //				wipItem.LotNO = mINNO.LotNO;/*ActionOnLineHelper.StringNull;*/
                //				wipItem.MItemCode = mINNO.MItemCode;/*ActionOnLineHelper.StringNull;*/					
                //				wipItem.PCBA =ActionOnLineHelper.StringNull;									
                //				wipItem.VendorCode =ActionOnLineHelper.StringNull;
                //				wipItem.VendorItemCode =ActionOnLineHelper.StringNull ;
                //				wipItem.Version =ActionOnLineHelper.StringNull ;

                wipItem.EAttribute1 = simulation.EAttribute1;
                wipItem.ItemCode = simulation.ItemCode;
                wipItem.ResourceCode = simulation.ResourceCode;
                wipItem.RouteCode = simulation.RouteCode;
                wipItem.RunningCard = simulation.RunningCard;
                wipItem.RunningCardSequence = simulation.RunningCardSequence;
                wipItem.SegmentCode = simulationReport.SegmentCode;
                wipItem.ShiftCode = simulationReport.ShiftCode;
                wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                wipItem.TimePeriodCode = simulationReport.TimePeriodCode;
                wipItem.MOCode = simulation.MOCode;
                wipItem.ModelCode = simulation.ModelCode;
                wipItem.OPCode = simulation.OPCode;

                wipItem.MaintainDate = simulation.MaintainDate;
                wipItem.MaintainTime = simulation.MaintainTime;
                wipItem.MaintainUser = simulation.MaintainUser;

                wipItem.Qty = mINNO.Qty;
                wipItem.TransactionStatus = TransactionStatus.TransactionStatus_NO;
                wipItem.MCardType = MCardType.MCardType_INNO;
                wipItem.MSequence = i;
                //Laws Lu,2005/12/20,����	�ɼ�����
                wipItem.ActionType = (int)MaterialType.CollectMaterial;
                wipItem.MOSeq = simulation.MOSeq;   // Added by Icyer 2007/07/02

                dataCollectFacade.AddOnWIPItem(wipItem);

                i++;
            }
        }


        /// <summary>
        /// ** ��������:	���ϲɼ�������INNO��KEYPARTS����
        ///							�������Ϻţ���������Ҷ�Ӧ��RESOURCE
        ///							KEYPARTS�ϵļ����ǰ̨�Ѿ�����
        ///							������������洦��
        /// ** �� ��:		Mark Lee
        /// ** �� ��:		2005-06-01
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="actionEventArgs"></param>
        /// <returns></returns>
        public Messages Execute(ActionEventArgs actionEventArgs)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            Messages msgAutoGoMO = new Messages();		// Added by Icyer 2007/03/09
            try
            {
                // Added by Icyer 2007/03/09
                // ����Զ���������
                ActionGoToMO actionGoToMO = new ActionGoToMO(this.DataProvider);
                msgAutoGoMO = actionGoToMO.AutoGoMO(actionEventArgs);
                // Added end

                ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
                //��дSIMULATION ��鹤����ID��;�̡�����
                messages.AddMessages(dataCollect.CheckID(actionEventArgs));
                if (messages.IsSuccess())
                {
                    //���ϼ��  ��INNO��KEYPARTS TODO 
                    //if (actionEventArgs.ActionType ==ActionType.DataCollectAction_CollectINNO)


                    messages.AddMessages(dataCollect.Execute(actionEventArgs));
                    if (messages.IsSuccess())
                    {
                        DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);

                        #region ��д������Ϣ��  ��INNO��KEYPARTS TODO
                        if (actionEventArgs.ActionType == ActionType.DataCollectAction_CollectINNO)
                        {

                            InsertINNOOnWipItem(actionEventArgs, dataCollectFacade);

                        }
                        else
                        {
                            InsertKeyPartOnWipItem(actionEventArgs, dataCollectFacade);
                        }
                        #endregion

                        // Added by Icyer 2005/08/16
                        // ���Ͽۿ��
                        // ��ʱ���� FOR ����P3�汾 MARK LEE 2005/08/22
                        // ȡ������ Icyer 2005/08/23
                        //Laws Lu,2005/10/20,����	ʹ�������ļ�����������ģ���Ƿ�ʹ��
                        if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                        {
                            BenQGuru.eMES.Material.WarehouseFacade wfacade = new WarehouseFacade(this.DataProvider);
                            wfacade.CollectMaterialStock(actionEventArgs.ProductInfo.NowSimulation.RunningCard, actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence.ToString(), actionEventArgs.ProductInfo.NowSimulation.MOCode);
                        }
                        // Added end


                        //AMOI  MARK  START  20050806 ���Ӱ���Դͳ�Ʋ���
                        #region ��д���Ա��� ����Դͳ��
                        //ReportHelper reportCollect = new ReportHelper(this.DataProvider);
                        //messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider
                        //    , actionEventArgs.ActionType, actionEventArgs.ProductInfo));
                        #endregion
                        //AMOI  MARK  END

                    }
                }
            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            //return messages;
            if (msgAutoGoMO.Count() < 1 || (msgAutoGoMO.IsSuccess() == true && messages.IsSuccess() == false))
                return messages;
            else
            {
                msgAutoGoMO.IgnoreError();
                msgAutoGoMO.AddMessages(messages);
                return msgAutoGoMO;
            }
        }

        // Added by Icyer 2005/10/28
        //��չһ����ActionCheckStatus�����ķ���
        public Messages Execute(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            Messages msgAutoGoMO = new Messages();		// Added by Icyer 2007/03/09
            try
            {
                // Added by Icyer 2007/03/09
                // ����Զ���������
                ActionGoToMO actionGoToMO = new ActionGoToMO(this.DataProvider);
                msgAutoGoMO = actionGoToMO.AutoGoMO(actionEventArgs, actionCheckStatus);
                // Added end

                ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
                //��дSIMULATION ��鹤����ID��;�̡�����
                messages.AddMessages(dataCollect.CheckID(actionEventArgs, actionCheckStatus));

                if (messages.IsSuccess())
                {
                    //���ϼ��  ��INNO��KEYPARTS TODO 
                    //if (actionEventArgs.ActionType ==ActionType.DataCollectAction_CollectINNO)


                    if (actionCheckStatus.NeedUpdateSimulation == true)
                    {
                        messages.AddMessages(dataCollect.Execute(actionEventArgs));
                    }
                    else
                    {
                        messages.AddMessages(dataCollect.Execute(actionEventArgs, actionCheckStatus));
                    }

                    BenQGuru.eMES.Material.WarehouseFacade wfacade = null;
                    if (messages.IsSuccess())
                    {
                        DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
                        #region ��д������Ϣ��  ��INNO��KEYPARTS
                        if (actionEventArgs.ActionType == ActionType.DataCollectAction_CollectINNO)
                        {
                            if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                            {
                                wfacade = ((CINNOActionEventArgs)actionEventArgs).Warehouse;
                            }

                            InsertINNOOnWipItem(actionEventArgs, dataCollectFacade);

                        }
                        else
                        {
                            InsertKeyPartOnWipItem(actionEventArgs, dataCollectFacade);

                            if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                            {
                                wfacade = ((CKeypartsActionEventArgs)actionEventArgs).Warehouse;
                            }

                        }
                        //						if (actionEventArgs.ActionType ==ActionType.DataCollectAction_CollectINNO)
                        //						{
                        //							#region Laws Lu��2005/12/26 ע��
                        //							string  iNNO=((CINNOActionEventArgs) actionEventArgs).INNO;
                        //							Simulation simulation=((CINNOActionEventArgs) actionEventArgs).ProductInfo.NowSimulation ;
                        //							SimulationReport simulationReport=((CINNOActionEventArgs) actionEventArgs).ProductInfo.NowSimulationReport ;
                        //							MaterialFacade material=new MaterialFacade(_domainDataProvider);
                        //							MINNO mINNO=(MINNO)material.GetMINNO(iNNO);
                        //							if (mINNO ==null)
                        //								throw new Exception("$CS_INNOnotExist");
                        //							if (mINNO.MOCode !=simulation.MOCode)
                        //								throw new Exception("$CS_INNOnotForMO $CS_Param_MOCode="+mINNO.MOCode);
                        //							if (mINNO.RouteCode !=simulation.RouteCode)
                        //								throw new Exception("$CS_INNOnotForRoute $CS_Param_RouteCode="+mINNO.RouteCode);
                        //							if (mINNO.OPCode !=simulation.OPCode)
                        //								throw new Exception("$CS_INNOnotForOP $CS_Param_OPCode ="+mINNO.OPCode);
                        //							if (mINNO.ResourceCode !=simulation.ResourceCode)
                        //								throw new Exception("$CS_INNOnotForResource $CS_Param_ResourceCode="+mINNO.ResourceCode);
                        //
                        //							OnWIPItem wipItem=new OnWIPItem();
                        //							wipItem.MCARD =iNNO;	
                        //							wipItem.BIOS =ActionOnLineHelper.StringNull;
                        //							wipItem.DateCode =ActionOnLineHelper.StringNull;
                        //							wipItem.LotNO =ActionOnLineHelper.StringNull;															
                        //							wipItem.MItemCode =ActionOnLineHelper.StringNull;									
                        //							wipItem.PCBA =ActionOnLineHelper.StringNull;									
                        //							wipItem.VendorCode =ActionOnLineHelper.StringNull;
                        //							wipItem.VendorItemCode =ActionOnLineHelper.StringNull ;
                        //							wipItem.Version =ActionOnLineHelper.StringNull ;
                        //
                        //							wipItem.EAttribute1 =simulation.EAttribute1 ;
                        //							wipItem.ItemCode =simulation.ItemCode;
                        //							wipItem.ResourceCode =simulation.ResourceCode;
                        //							wipItem.RouteCode =simulation.RouteCode;
                        //							wipItem.RunningCard =simulation.RunningCard ;
                        //							wipItem.RunningCardSequence =simulation.RunningCardSequence ;
                        //							wipItem.SegmentCode =simulationReport.SegmentCode;
                        //							wipItem.ShiftCode =simulationReport.ShiftCode;
                        //							wipItem.ShiftTypeCode =simulationReport.ShiftTypeCode;
                        //							wipItem.StepSequenceCode =simulationReport.StepSequenceCode;
                        //							wipItem.TimePeriodCode =simulationReport.TimePeriodCode;
                        //							wipItem.MOCode =simulation.MOCode;
                        //							wipItem.ModelCode =simulation.ModelCode;									
                        //							wipItem.OPCode =simulation.OPCode;
                        //
                        //							wipItem.MaintainDate =simulation.MaintainDate;
                        //							wipItem.MaintainTime =simulation.MaintainTime ;
                        //							wipItem.MaintainUser =simulation.MaintainUser ;
                        //
                        //							wipItem.Qty =1;									
                        //							wipItem.TransactionStatus =TransactionStatus.TransactionStatus_NO;
                        //							wipItem.MCardType =MCardType.MCardType_INNO;
                        //							wipItem.MSequence =ActionOnLineHelper.StartSeq;
                        //
                        ////							if (actionCheckStatus.NeedUpdateSimulation == true)
                        ////							{
                        //								dataCollectFacade.AddOnWIPItem(wipItem);
                        ////							}
                        ////							else
                        ////							{
                        ////								actionEventArgs.OnWIP.Add(wipItem);
                        ////							}
                        //
                        //							
                        //						}
                        //						else
                        //						{
                        //							OPBomKeyparts keyParts=((CKeypartsActionEventArgs) actionEventArgs).Keyparts;
                        //							Simulation simulation=((CKeypartsActionEventArgs) actionEventArgs).ProductInfo.NowSimulation ;
                        //							SimulationReport simulationReport=((CKeypartsActionEventArgs) actionEventArgs).ProductInfo.NowSimulationReport ;
                        //							int n=keyParts.GetbomKeypartCount();
                        //							for (int i=0;i<n;i++)
                        //							{
                        //								MKeyPart keypart= keyParts.GetKeypart(i);
                        //								OnWIPItem wipItem=new OnWIPItem();
                        //								wipItem.BIOS =keypart.BIOS;
                        //								wipItem.DateCode =keypart.DateCode;
                        //								wipItem.LotNO =keypart.LotNO;									
                        //								wipItem.MCARD =keypart.Keypart;									
                        //								wipItem.MItemCode =keypart.MItemCode;									
                        //								wipItem.PCBA =keypart.PCBA;									
                        //								wipItem.VendorCode =keypart.VendorCode;
                        //								wipItem.VendorItemCode =keypart.VendorItemCode ;
                        //								wipItem.Version =keypart.Version ;
                        //								wipItem.EAttribute1 =simulation.EAttribute1 ;
                        //								wipItem.ItemCode =simulation.ItemCode;
                        //								wipItem.ResourceCode =simulation.ResourceCode;
                        //								wipItem.RouteCode =simulation.RouteCode;
                        //								wipItem.RunningCard =simulation.RunningCard ;
                        //								wipItem.RunningCardSequence =simulation.RunningCardSequence ;
                        //								wipItem.SegmentCode =simulationReport.SegmentCode;
                        //								wipItem.ShiftCode =simulationReport.ShiftCode;
                        //								wipItem.ShiftTypeCode =simulationReport.ShiftTypeCode;
                        //								wipItem.StepSequenceCode =simulationReport.StepSequenceCode;
                        //								wipItem.TimePeriodCode =simulationReport.TimePeriodCode;
                        //								wipItem.MOCode =simulation.MOCode;
                        //								wipItem.ModelCode =simulation.ModelCode;									
                        //								wipItem.OPCode =simulation.OPCode;
                        //
                        //								wipItem.MaintainDate =simulation.MaintainDate;
                        //								wipItem.MaintainTime =simulation.MaintainTime ;
                        //								wipItem.MaintainUser =simulation.MaintainUser ;
                        //
                        //								wipItem.Qty =1;									
                        //								wipItem.TransactionStatus =TransactionStatus.TransactionStatus_NO;
                        //								wipItem.MCardType =MCardType.MCardType_Keyparts;
                        //								wipItem.MSequence =i;
                        //
                        ////								if (actionCheckStatus.NeedUpdateSimulation == true)
                        ////								{
                        //									dataCollectFacade.AddOnWIPItem(wipItem);
                        ////								}
                        ////								else
                        ////								{
                        ////									actionEventArgs.OnWIP.Add(wipItem);
                        ////								}
                        //							}
                        #endregion
                    }
                    // Added by Icyer 2005/08/16
                    // ���Ͽۿ��
                    // ��ʱ���� FOR ����P3�汾 MARK LEE 2005/08/22
                    // ȡ������ Icyer 2005/08/23
                    //Laws Lu,2005/10/20,����	ʹ�������ļ�����������ģ���Ƿ�ʹ��
                    if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                    {
                        //BenQGuru.eMES.Material.WarehouseFacade wfacade = new WarehouseFacade(this.DataProvider);
                        if (wfacade != null)
                            wfacade.CollectMaterialStock(actionEventArgs.ProductInfo.NowSimulation.RunningCard, actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence.ToString(), actionEventArgs.ProductInfo.NowSimulation.MOCode);
                    }
                    // Added end

                    //						#endregion						
                    //AMOI  MARK  START  20050806 ���Ӱ���Դͳ�Ʋ���
                    #region ��д���Ա��� ����Դͳ��
                    //�����Ҫ���±���
                    //if (actionCheckStatus.NeedFillReport)
                    //{
                    //    ReportHelper reportCollect = new ReportHelper(this.DataProvider);
                    //    messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider
                    //        , actionEventArgs.ActionType, actionEventArgs.ProductInfo, actionCheckStatus));
                    //}

                    //��Action�����б�
                    actionCheckStatus.ActionList.Add(actionEventArgs);
                    #endregion
                    //AMOI  MARK  END

                }
            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            //return messages;
            if (msgAutoGoMO.Count() < 1 || (msgAutoGoMO.IsSuccess() == true && messages.IsSuccess() == false))
                return messages;
            else
            {
                msgAutoGoMO.IgnoreError();
                msgAutoGoMO.AddMessages(messages);
                return msgAutoGoMO;
            }
        }
        // Added end


        //��չһ����ActionCheckStatus��OPBOMDetail�����ķ���
        public Messages Execute(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus, object[] OPBOMDetail)
        {
            Messages messages = new Messages();
            DataCollectDebug dataCollectDebug = new DataCollectDebug(this.GetType().ToString() + "Collect");
            dataCollectDebug.WhenFunctionIn(messages);
            Messages msgAutoGoMO = new Messages();		// Added by Icyer 2007/03/09
            try
            {
                // Added by Icyer 2007/03/09
                // ����Զ���������
                ActionGoToMO actionGoToMO = new ActionGoToMO(this.DataProvider);
                msgAutoGoMO = actionGoToMO.AutoGoMO(actionEventArgs, actionCheckStatus);
                // Added end

                ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
                //��дSIMULATION ��鹤����ID��;�̡�����
                messages.AddMessages(dataCollect.CheckID(actionEventArgs, actionCheckStatus));

                if (messages.IsSuccess())
                {
                    //���ϼ��  ��INNO��KEYPARTS TODO 
                    //if (actionEventArgs.ActionType ==ActionType.DataCollectAction_CollectINNO)


                    if (actionCheckStatus.NeedUpdateSimulation == true)
                    {
                        messages.AddMessages(dataCollect.Execute(actionEventArgs));
                    }
                    else
                    {
                        messages.AddMessages(dataCollect.Execute(actionEventArgs, actionCheckStatus));
                    }

                    BenQGuru.eMES.Material.WarehouseFacade wfacade = null;
                    if (messages.IsSuccess())
                    {
                        DataCollectFacade dataCollectFacade = new DataCollectFacade(this.DataProvider);
                        #region ��д������Ϣ��  ��INNO��KEYPARTS
                        if (actionEventArgs.ActionType == ActionType.DataCollectAction_CollectINNO)
                        {
                            if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                            {
                                wfacade = ((CINNOActionEventArgs)actionEventArgs).Warehouse;
                            }

                            messages.AddMessages(InsertLotOnWipItem(actionEventArgs, dataCollectFacade, OPBOMDetail));
                            if (!messages.IsSuccess())
                            {
                                return messages;
                            }
                        }
                        else
                        {
                            InsertKeyPartOnWipItem(actionEventArgs, dataCollectFacade);

                            if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                            {
                                wfacade = ((CKeypartsActionEventArgs)actionEventArgs).Warehouse;
                            }

                        }
                        #endregion
                    }
                    // Added by Icyer 2005/08/16
                    // ���Ͽۿ��
                    // ��ʱ���� FOR ����P3�汾 MARK LEE 2005/08/22
                    // ȡ������ Icyer 2005/08/23
                    //Laws Lu,2005/10/20,����	ʹ�������ļ�����������ģ���Ƿ�ʹ��
                    if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                    {
                        //BenQGuru.eMES.Material.WarehouseFacade wfacade = new WarehouseFacade(this.DataProvider);
                        if (wfacade != null)
                            wfacade.CollectMaterialStock(actionEventArgs.ProductInfo.NowSimulation.RunningCard, actionEventArgs.ProductInfo.NowSimulation.RunningCardSequence.ToString(), actionEventArgs.ProductInfo.NowSimulation.MOCode);
                    }
                    // Added end

                    //						#endregion						
                    //AMOI  MARK  START  20050806 ���Ӱ���Դͳ�Ʋ���
                    #region ��д���Ա��� ����Դͳ��
                    //�����Ҫ���±���
                    //if (actionCheckStatus.NeedFillReport)
                    //{
                    //    ReportHelper reportCollect = new ReportHelper(this.DataProvider);
                    //    messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider
                    //        , actionEventArgs.ActionType, actionEventArgs.ProductInfo, actionCheckStatus));
                    //}

                    //��Action�����б�
                    actionCheckStatus.ActionList.Add(actionEventArgs);
                    #endregion
                    //AMOI  MARK  END

                }
            }
            catch (Exception e)
            {
                messages.Add(new Message(e));
            }
            dataCollectDebug.WhenFunctionOut(messages);
            //return messages;
            if (msgAutoGoMO.Count() < 1 || (msgAutoGoMO.IsSuccess() == true && messages.IsSuccess() == false))
                return messages;
            else
            {
                msgAutoGoMO.IgnoreError();
                msgAutoGoMO.AddMessages(messages);
                return msgAutoGoMO;
            }
        }

        public const string MCardType_Keyparts = "0";
        public const string MCardType_INNO = "1";

    }
    /// <summary>
    /// �ļ���:		ActionItem.cs
    /// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
    /// ������:		Mark Lee
    /// ��������:	2005-05-17 11:23:20
    /// �޸���:
    /// �޸�����:
    /// �� ��:	����һ�� KEYPARTS��BOM������һ�����ϣ����KEYPARTS
    /// �� ��:	
    /// </summary>
    public class OPBomKeypart
    {
        public OPBOMDetail OpBomDetial;
        public int Count;
        public OPBomKeypart()
        {
            MKeyparts = new ArrayList();
        }
        public ArrayList MKeyparts;

    }
    /// <summary>
    /// �ļ���:		ActionItem.cs
    /// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
    /// ������:		Mark Lee
    /// ��������:	2005-05-17 11:23:20
    /// �޸���:
    /// �޸�����:
    /// �� ��:	����һ��BOM������������ϣ��������KEYPARTS���߼�
    /// �� ��:	
    /// </summary>
    public class OPBomKeyparts
    {
        private object[] _opBomDetials;
        private int _iDMergeRule;
        private IDomainDataProvider _domainDataProvider;
        /// <summary>
        /// ** ��������:	���ϲɼ�
        ///							��������ϣ�������������
        ///							����ݹ�������������ʾ���ϸ���
        /// ** �� ��:		Mark Lee
        /// ** �� ��:		2005-06-01
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="opBomDetials">��Ҫ�ϵ����е���</param>
        /// <param name="iDMergeRule">��������</param>
        /// <param name="domainDataProvider"></param>
        public OPBomKeyparts(object[] opBomDetials, int iDMergeRule, IDomainDataProvider domainDataProvider)
        {
            _opBomDetials = opBomDetials;
            _iDMergeRule = iDMergeRule;
            _domainDataProvider = domainDataProvider;
            _bomKeyparts = new ArrayList();
            //��¼���е���ѡ���Ϻ�
            ArrayList sourceItemCodes = new ArrayList();
            if (opBomDetials == null)
            {
                opBomDetials = new object[0];
                return;
            }
            for (int i = 0; i < opBomDetials.Length; i++)
            {
                OPBOMDetail opBomDetial = (OPBOMDetail)opBomDetials[i];
                //��ѡ����ѡ��
                if (opBomDetial.OPBOMItemControlType == BOMItemControlType.ITEM_CONTROL_KEYPARTS)
                {
                    if (opBomDetial.OPBOMItemCode == opBomDetial.OPBOMSourceItemCode)
                    {
                        OPBomKeypart BomKeypart = new OPBomKeypart();
                        BomKeypart.OpBomDetial = opBomDetial;
                        this._count = this._count + Convert.ToInt32(opBomDetial.OPBOMItemQty) * iDMergeRule;

                        _bomKeyparts.Add(BomKeypart);
                        sourceItemCodes.Add(opBomDetial.OPBOMSourceItemCode);
                    }
                }
            }
            for (int i = 0; i < opBomDetials.Length; i++)
            {
                OPBOMDetail opBomDetial = (OPBOMDetail)opBomDetials[i];
                //��ѡ����ѡ��  ������Щû��ά����ѡ�ϵ��ӽ���
                if (opBomDetial.OPBOMItemControlType == BOMItemControlType.ITEM_CONTROL_KEYPARTS)
                {
                    if (opBomDetial.OPBOMItemCode != opBomDetial.OPBOMSourceItemCode)
                    {
                        if (sourceItemCodes.IndexOf(opBomDetial.OPBOMSourceItemCode) < 0)
                        {
                            OPBomKeypart BomKeypart = new OPBomKeypart();
                            BomKeypart.OpBomDetial = opBomDetial;
                            this._count = this._count + Convert.ToInt32(opBomDetial.OPBOMItemQty) * iDMergeRule;
                            _bomKeyparts.Add(BomKeypart);
                            sourceItemCodes.Add(opBomDetial.OPBOMSourceItemCode);
                        }
                    }
                }
            }

        }
        private int _count = 0;
        public int Count
        {
            get
            {
                return _count;
            }
        }
        private ArrayList _bomKeyparts;
        /// <summary>
        /// ** ��������:	���ϲɼ� ����һ��KEYPARTS
        ///							��������ϣ�������������
        ///							����ݹ�������������ʾ���ϸ���
        /// ** �� ��:		Mark Lee
        /// ** �� ��:		2005-06-01
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="keyPart"></param>
        public Messages AddKeyparts(string keyPart, string moCode)
        {
            return AddKeyparts(keyPart, moCode, string.Empty);
        }
        /// <summary>
        /// ** ��������:	���ϲɼ� ����һ��KEYPARTS
        ///							��������ϣ�������������
        ///							����ݹ�������������ʾ���ϸ���
        /// ** �� ��:		Mark Lee
        /// ** �� ��:		2005-06-01
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="keyPart"></param>
        public Messages AddKeyparts(string keyPart, string moCode, string runningCard)
        {
            Messages messages = new Messages();
            MaterialFacade material = new MaterialFacade(_domainDataProvider);
            object[] MKeyparts = material.QueryMKeyPart(string.Empty, keyPart, 0, 100);
            if (MKeyparts == null)
                throw new Exception("$CS_NoKeyPartInfo $CS_Param_Keypart=" + keyPart);
            if (MKeyparts.Length == 0)
                throw new Exception("$CS_NoKeyPartInfo $CS_Param_Keypart=" + keyPart);
            //			if (MKeyparts.Length>1)
            //				throw new Exception("$CS_Error_GetTooMoreKeyPartInfo $CS_Param_Keypart="+keyPart);

            MKeyPart mKeyPart = null;//Laws Lu,2005/12/27���޸�=(MKeyPart)MKeyparts[0];

            ArrayList ar = new ArrayList();
            foreach (MKeyPart key in MKeyparts)
            {
                if (key.RunningCardStart.Length == keyPart.Length
                    && String.Compare(key.RunningCardStart, keyPart, false) <= 0
                    && String.Compare(key.RunningCardEnd, keyPart, false) >= 0)
                {
                    mKeyPart = key;
                    ar.Add(key);
                }
            }
            if (mKeyPart == null)
            {
                throw new Exception("$CS_NoKeyPartInfo $CS_Param_Keypart=" + keyPart);
            }
            int n = GetbomKeypart(mKeyPart.MItemCode);
            if (n < 0)
                throw new Exception("$CS_KeyPartNotForThisOP $CS_Param_Keypart=" + keyPart + " $CS_Param_KeypartItem=" + mKeyPart.MItemCode);
            else
            {
                OPBomKeypart BomKeypart = (OPBomKeypart)_bomKeyparts[n];
                //����Ƿ��ظ�
                for (int i = 0; i < BomKeypart.MKeyparts.Count; i++)
                {
                    if (((MKeyPart)BomKeypart.MKeyparts[i]).Keypart == keyPart)
                        throw new Exception("$CS_KeyPartRepeat");
                }
                if (BomKeypart.OpBomDetial.OPBOMItemQty * _iDMergeRule <= BomKeypart.MKeyparts.Count)
                {
                    throw new Exception("$CS_KeyPartOutofItemBomCount "/*$CS_Param_BOMKeypartCount="+BomKeypart.OpBomDetial.OPBOMItemQty*_iDMergeRule
						*/
                          + " $CS_Param_KeypartItem=" + mKeyPart.MItemCode);
                }

                MaterialFacade mf = new MaterialFacade(this._domainDataProvider);
                //Laws Lu,2005/08/31,����	RunningCard���ȼ��
                bool isExist = false;

                foreach (MKeyPart key in ar)
                {
                    if (key.MoCode != moCode && key.MoCode != String.Empty)
                    {
                        throw new Exception("$CS_KEYPART_NOT_BELONG_MO $Domain_MO=" + moCode);
                    }
                    if (mf.RunningCardRangeCheck(key.MItemCode.ToString().ToUpper().Trim(), keyPart, key.Sequence.ToString().Trim()))
                    {
                        isExist = true;
                        break;
                    }

                }

                if (!isExist)
                {
                    throw new Exception("$CS_NoKeyPartInfo $CS_Param_Keypart=" + keyPart);
                }

                // Added by Icyer 2006/11/06
                // ���KeyPart�깤״̬���Ƿ�������
                if (BomKeypart.OpBomDetial.ActionType == (int)MaterialType.CollectMaterial)
                {
                    if (FormatHelper.StringToBoolean(BomKeypart.OpBomDetial.CheckStatus) == true)
                    {
                        Messages msg = CheckKeyPartStatus(keyPart);
                        if (msg.IsSuccess() == false)
                        {
                            return msg;
                        }
                    }
                }
                else if (runningCard != string.Empty)	// ����
                {
                    //if (FormatHelper.StringToBoolean(BomKeypart.OpBomDetial.CheckStatus) == true)
                    //{
                    if (CheckKeyPartUnLoadStatus(keyPart, runningCard) == false)
                    {
                        messages.Add(new Message(MessageType.Error, "$DropMaterial_KeyPart_Not_On_RCard"));
                        return messages;
                    }
                    //}
                }
                // Added end

                mKeyPart.Keypart = keyPart;
                BomKeypart.MKeyparts.Add(mKeyPart);
            }
            return messages;
        }
        /// <summary>
        /// ** ��������:	����������Ż�ȡKEYPARTS
        /// ** �� ��:		Mark Lee
        /// ** �� ��:		2005-06-01
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MKeyPart GetKeypart(int index)
        {
            int n = index;
            for (int i = 0; i < _bomKeyparts.Count; i++)
            {
                OPBomKeypart BomKeypart = (OPBomKeypart)_bomKeyparts[i];
                if (n >= BomKeypart.MKeyparts.Count)
                    n = n - BomKeypart.MKeyparts.Count;
                else
                    return (MKeyPart)BomKeypart.MKeyparts[n];
            }
            throw new Exception("$CS_System_KeyPartsOutofCount");
        }
        /// <summary>
        /// ** ��������:	�����ϺŻ�ȡ��Ӧ��������Ϣ
        /// ** �� ��:		Mark Lee
        /// ** �� ��:		2005-06-01
        /// ** �� ��:
        /// ** �� ��: 
        /// </summary>
        /// <param name="bomItemCode"></param>
        /// <returns></returns>
        private int GetbomKeypart(string bomItemCode)
        {
            //���ҵ���Ӧ�����Ϻ�
            string itemCode = string.Empty;
            for (int i = 0; i < _opBomDetials.Length; i++)
            {
                OPBOMDetail opBomDetail = (OPBOMDetail)_opBomDetials[i];
                if (opBomDetail.OPBOMItemCode == bomItemCode || opBomDetail.OPBOMSourceItemCode == bomItemCode)
                {
                    //���ITEMCONTROL
                    if (opBomDetail.OPBOMItemControlType == BOMItemControlType.ITEM_CONTROL_KEYPARTS)
                        itemCode = opBomDetail.OPBOMSourceItemCode;
                    else
                        throw new Exception("$CS_ItemControl_Is_NotKeyparts $CS_Param_ItemControl=" + opBomDetail.OPBOMItemControlType);
                }
            }
            if (itemCode == string.Empty)
                return -1;
            //�����
            for (int i = 0; i < _bomKeyparts.Count; i++)
            {
                OPBomKeypart BomKeypart = (OPBomKeypart)_bomKeyparts[i];
                if (BomKeypart.OpBomDetial.OPBOMSourceItemCode == itemCode)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// ** ��������:	��ȡ����KEYPARTS����
        /// ** �� ��:		Mark Lee
        /// ** �� ��:		2005-06-01
        /// ** �� ��:
        /// ** �� ��: 
        /// </summary>
        /// <returns></returns>
        public int GetbomKeypartCount()
        {
            int n = 0;
            for (int i = 0; i < _bomKeyparts.Count; i++)
            {
                OPBomKeypart BomKeypart = (OPBomKeypart)_bomKeyparts[i];
                n = n + BomKeypart.MKeyparts.Count;
            }
            return n;
        }

        // Added by Icyer 2006/11/06
        /// <summary>
        /// ���KeyPart�Ƿ��깤
        /// ���KeyPart�Ƿ����
        /// </summary>
        public Messages CheckKeyPartStatus(string keyPartNo)
        {
            return CheckKeyPartStatus(keyPartNo, string.Empty);
        }
        /// <summary>
        /// ���KeyPart�Ƿ��깤
        /// ���KeyPart�Ƿ����
        /// </summary>
        public Messages CheckKeyPartStatus(string keyPartNo, string checkItemCode)
        {
            Messages msg = new Messages();
            DataCollectFacade dcFacade = new DataCollectFacade(this._domainDataProvider);
            SimulationReport simulationRpt = dcFacade.GetLastSimulationReport(keyPartNo);
            // ���û���ҵ�������Ϣ�������û������Ƿ񱣴�
            if (simulationRpt == null)
            {
                SystemSettingFacade sysFacade = new SystemSettingFacade(this._domainDataProvider);
                object obj = sysFacade.GetParameter("ITEM_CONFIRM", "DATACOLLECT_ITEM");
                bool bConfirm = false;
                if (obj != null && ((Parameter)obj).ParameterAlias == "1")
                {
                    bConfirm = true;
                }
                if (bConfirm == true)
                {
                    string strMsg = UserControl.MutiLanguages.ParserString("$LoadKeyPart_NoSimulation_Confirm");
                    if (System.Windows.Forms.MessageBox.Show(strMsg, string.Empty, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        msg.Add(new Message(MessageType.Error, " "));
                        return msg;
                    }
                }
                else
                {
                    msg.Add(new Message(MessageType.Error, "$LoadKeyPart_NotComplete"));
                    return msg;
                }
            }
            else
            {
                // ����Ƿ��깤
                if (FormatHelper.StringToBoolean(simulationRpt.IsComplete) == false)
                {
                    msg.Add(new Message(MessageType.Error, "$LoadKeyPart_NotComplete"));
                    return msg;
                }
                // ����Ƿ���Ʒ
                if (simulationRpt.Status != ProductStatus.GOOD)
                {
                    msg.Add(new Message(MessageType.Error, "$LoadKeyPart_Status_NG"));
                    return msg;
                }
                // ����Ƿ�������
                if (FormatHelper.StringToBoolean(simulationRpt.IsLoadedPart) == true)
                {
                    msg.Add(new Message(MessageType.Error, "$LoadKeyPart_KeyPart_Loaded_Already"));
                    return msg;
                }
                if (checkItemCode != null && checkItemCode != string.Empty && simulationRpt.ItemCode != checkItemCode)
                {
                    msg.Add(new Message(MessageType.Error, "$Error_Inv_Product_Error"));
                    return msg;
                }
            }
            return msg;
        }
        /// <summary>
        /// ���KeyPart�Ƿ��������к��ϵ����ϲ���
        /// </summary>
        public bool CheckKeyPartUnLoadStatus(string keyPartNo, string runningCard)
        {
            string strSql = "SELECT * FROM tblOnWIPItem WHERE MCard='" + keyPartNo + "' ORDER BY MDate DESC,MTime DESC ";
            object[] objs = this._domainDataProvider.CustomQuery(typeof(OnWIPItem), new SQLCondition(strSql));
            if (objs == null || objs.Length < 1)
                return false;
            OnWIPItem item = (OnWIPItem)objs[0];
            // ���KeyPart���һ�β���������
            if (item.ActionType == (int)MaterialType.CollectMaterial)
            {
                // ��Ӧ�����к��ǵ�ǰ���к�
                if (item.RunningCard == runningCard)
                {
                    return true;
                }
                else
                {
                    // ��ѯ���ϵ����к��ǲ��Ǿ������ת��
                    BenQGuru.eMES.Material.CastDownHelper castHelper = new BenQGuru.eMES.Material.CastDownHelper(this._domainDataProvider);
                    ArrayList arRcard = new ArrayList();
                    castHelper.GetAllRCard(ref arRcard, runningCard);
                    if (arRcard.Contains(item.RunningCard) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// ͨ��KeyPart�ϺŲ�ѯOPBOM Detail
        /// </summary>
        public OPBomKeypart GetOPBomDetail(string bomItemCode)
        {
            int i = this.GetbomKeypart(bomItemCode);
            if (i < 0)
                return null;
            return (OPBomKeypart)_bomKeyparts[i];
        }
        // Added end
    }

}
