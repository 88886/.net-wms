using System;
using System.Collections;
using UserControl;
using BenQGuru.eMES.Domain.LotDataCollect;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.MOModel;

namespace BenQGuru.eMES.LotDataCollect.Action
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
            LotSimulation simulation = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulation;
            LotSimulationReport simulationReport = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulationReport;
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
                if (mINNO.ResourceCode != simulation.ResCode)
                    throw new Exception("$CS_INNOnotForResource $CS_Param_ResourceCode=" + mINNO.ResourceCode);

                LotOnWipItem wipItem = new LotOnWipItem();
                wipItem.DateCode = mINNO.DateCode;
                wipItem.LOTNO = mINNO.LotNO;/*ActionOnLineHelper.StringNull;*/
                wipItem.MItemCode = mINNO.MItemCode;/*ActionOnLineHelper.StringNull;*/
                wipItem.VendorCode = mINNO.VendorCode;
                wipItem.VendorItemCode = mINNO.VendorItemCode;
                wipItem.Version = mINNO.Version;
                wipItem.Eattribute1 = simulation.EAttribute1;
                wipItem.ItemCode = simulation.ItemCode;
                wipItem.ResCode = simulation.ResCode;
                wipItem.RouteCode = simulation.RouteCode;
                wipItem.LotCode = simulation.LotCode;
                wipItem.LotSeq = simulation.LotSeq;
                wipItem.SegmentCode = simulationReport.SegmentCode;
                wipItem.BeginShiftCode = simulationReport.BeginShiftCode;
                wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                wipItem.BeginTimePeriodCode = simulationReport.BeginTimePeriodCode;
                wipItem.MOCode = simulation.MOCode;
                wipItem.ModelCode = simulation.ModelCode;
                wipItem.OPCode = simulation.OPCode;
                wipItem.CollectStatus = simulation.CollectStatus;
                wipItem.BeginDate= simulation.BeginDate;
                wipItem.BeginTime = simulation.BeginTime;
                wipItem.MaintainUser = simulation.MaintainUser;

                wipItem.Qty = (int)mINNO.Qty * simulation.LotQty;
                wipItem.MCardType = MCardType.MCardType_INNO;
                //Laws Lu,2005/12/20,����	�ɼ�����
                wipItem.ActionType = (int)MaterialType.CollectMaterial;
                wipItem.MOSeq = simulation.MOSeq;   // Added by Icyer 2007/07/02

                dataCollectFacade.AddLotOnWIPItem(wipItem);

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
            LotSimulation simulation = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulation;
            LotSimulationReport simulationReport = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulationReport;
            Messages returnValue = new Messages();
            string lotNoList = string.Empty;// add by Jarvis For onWipItem                       

            ProductInfo productionInfo = actionEventArgs.ProductInfo;
            LotSimulation sim = actionEventArgs.ProductInfo.NowSimulation;
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
                MItemName = material.MaterialName;
            }

            //��Ӳ�Ʒ�����Ͽ����ж�  tblonwip
            string lotNo = productionInfo.NowSimulation.LotCode;
            decimal seq = productionInfo.NowSimulation.LotSeq;
            //object[] objOnWip = dataCollectFacade.QueryLotOnWIP(lotNo, moCode, opCode, "CINNO");

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

            //�Թ�����׼BOMΪ��׼,�ۼ���ǰ�����ĵ�����أ�tblmo. EATTRIBUTE2�������Ӧ�Ŀ����Ϣ
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

                        iOPBOMItemQty = (decimal)((OPBOMDetail)opbomObjs[j]).OPBOMItemQty;
                            iOPBOMItemQty *= sim.LotQty;
                        
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
                        LotOnWipItem wipItem = new LotOnWipItem();
                        MINNO minnoTemp = null;
                        object[] minnoTemps = materialFacade.QueryMINNO(moCode, routeCode, opCode, resCode, moBomVer, lotInfo.Mcode, minno.MSourceItemCode, lotInfo.Lotno);
                        if (minnoTemps != null)
                        {
                            minnoTemp = (MINNO)minnoTemps[0];
                        }
                        wipItem.DateCode = minnoTemp.DateCode;
                        wipItem.LOTNO = minnoTemp.LotNO;
                        wipItem.MItemCode = minnoTemp.MItemCode;
                        wipItem.VendorCode = minnoTemp.VendorCode;
                        wipItem.VendorItemCode = minnoTemp.VendorItemCode;
                        wipItem.Version = minnoTemp.Version;
                        wipItem.MSeq = seqForDeductQty;
                        wipItem.MCardType = minno.EAttribute1;

                        wipItem.Eattribute1 = simulation.EAttribute1;
                        wipItem.ItemCode = simulation.ItemCode;
                        wipItem.ResCode = simulation.ResCode;
                        wipItem.RouteCode = simulation.RouteCode;
                        wipItem.LotCode = simulation.LotCode;
                        wipItem.LotSeq = simulation.LotSeq;
                        wipItem.SegmentCode = simulationReport.SegmentCode;
                        wipItem.BeginShiftCode = simulationReport.BeginShiftCode;
                        wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                        wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                        wipItem.BeginTimePeriodCode = simulationReport.BeginTimePeriodCode;
                        wipItem.MOCode = simulation.MOCode;
                        wipItem.ModelCode = simulation.ModelCode;
                        wipItem.OPCode = simulation.OPCode;
                        wipItem.CollectStatus = simulation.CollectStatus;
                        wipItem.BeginDate = simulation.BeginDate;
                        wipItem.BeginTime = simulation.BeginTime;
                        wipItem.MaintainUser = simulation.MaintainUser;
                        wipItem.TransStatus = TransactionStatus.TransactionStatus_YES;
                        wipItem.Qty = lotInfo.Lotqty;
                        
                        wipItem.ActionType = (int)MaterialType.CollectMaterial;
                        wipItem.MOSeq = simulation.MOSeq; 

                        dataCollectFacade.AddLotOnWIPItem(wipItem);

                        LotSimulationReport simulationRpt = dataCollectFacade.GetLastLotSimulationReport(wipItem.LotCode);
                        if (simulationRpt != null)
                        {
                            dataCollectFacade.UpdateLotSimulationReport(simulationRpt);
                                                        
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
                        LotOnWipItem wipItem = new LotOnWipItem();
                        MINNO minnoTemp = null;
                        object[] minnoTemps = materialFacade.QueryMINNO(moCode, routeCode, opCode, resCode, moBomVer, lotInfo.Mcode, minno.MSourceItemCode, lotInfo.Lotno);
                        if (minnoTemps != null)
                        {
                            minnoTemp = (MINNO)minnoTemps[0];
                        }
                        wipItem.DateCode = minnoTemp.DateCode;
                        wipItem.LOTNO = minnoTemp.LotNO;
                        wipItem.MItemCode = minnoTemp.MItemCode;
                        wipItem.VendorCode = minnoTemp.VendorCode;
                        wipItem.VendorItemCode = minnoTemp.VendorItemCode;
                        wipItem.Version = minnoTemp.Version;
                        wipItem.MSeq = seqForDeductQty;
                        wipItem.MCardType = minno.EAttribute1;

                        wipItem.Eattribute1 = simulation.EAttribute1;
                        wipItem.ItemCode = simulation.ItemCode;
                        wipItem.ResCode = simulation.ResCode;
                        wipItem.RouteCode = simulation.RouteCode;
                        wipItem.LotCode = simulation.LotCode;
                        wipItem.LotSeq = simulation.LotSeq;
                        wipItem.SegmentCode = simulationReport.SegmentCode;
                        wipItem.BeginShiftCode = simulationReport.BeginShiftCode;
                        wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                        wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                        wipItem.BeginTimePeriodCode = simulationReport.BeginTimePeriodCode;
                        wipItem.MOCode = simulation.MOCode;
                        wipItem.ModelCode = simulation.ModelCode;
                        wipItem.OPCode = simulation.OPCode;
                        wipItem.CollectStatus = simulation.CollectStatus;
                        wipItem.BeginDate = simulation.BeginDate;
                        wipItem.BeginTime = simulation.BeginTime;
                        wipItem.MaintainUser = simulation.MaintainUser;
                        wipItem.TransStatus = TransactionStatus.TransactionStatus_YES;
                        wipItem.Qty = iOPBOMItemQty;

                        wipItem.ActionType = (int)MaterialType.CollectMaterial;
                        wipItem.MOSeq = simulation.MOSeq;

                        dataCollectFacade.AddLotOnWIPItem(wipItem);

                        LotSimulationReport simulationRpt = dataCollectFacade.GetLastLotSimulationReport(wipItem.LotCode);
                        if (simulationRpt != null)
                        {
                            dataCollectFacade.UpdateLotSimulationReport(simulationRpt);
                                                        
                        }
                        seqForDeductQty++;
                        #endregion
                        iOPBOMItemQty = 0;
                        lotNoList += ("," + lotInfo.Lotno + ",");
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
            LotSimulation simulation = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulation;
            LotSimulationReport simulationReport = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulationReport;
            MaterialFacade material = new MaterialFacade(_domainDataProvider);
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
                if (!isDeductQty)//���������ֻ��¼ͬһ��ѡ�ϵ�һ�ʹ���
                {
                    //ԭ�й��˼�¼��Ų���˴�
                    LotOnWipItem wipItem = new LotOnWipItem();

                    wipItem.DateCode = mINNO.DateCode;
                    wipItem.LOTNO = mINNO.LotNO;
                    wipItem.MItemCode = mINNO.MItemCode;
                    wipItem.VendorCode = mINNO.VendorCode;
                    wipItem.VendorItemCode = mINNO.VendorItemCode;
                    wipItem.Version = mINNO.Version;
                    wipItem.MSeq = i;
                    wipItem.MCardType = mINNO.EAttribute1;

                    wipItem.Eattribute1 = simulation.EAttribute1;
                    wipItem.ItemCode = simulation.ItemCode;
                    wipItem.ResCode = simulation.ResCode;
                    wipItem.RouteCode = simulation.RouteCode;
                    wipItem.LotCode = simulation.LotCode;
                    wipItem.LotSeq = simulation.LotSeq;
                    wipItem.SegmentCode = simulationReport.SegmentCode;
                    wipItem.BeginShiftCode = simulationReport.BeginShiftCode;
                    wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                    wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                    wipItem.BeginTimePeriodCode = simulationReport.BeginTimePeriodCode;
                    wipItem.MOCode = simulation.MOCode;
                    wipItem.ModelCode = simulation.ModelCode;
                    wipItem.OPCode = simulation.OPCode;
                    wipItem.CollectStatus = simulation.CollectStatus;
                    wipItem.BeginDate = simulation.BeginDate;
                    wipItem.BeginTime = simulation.BeginTime;
                    wipItem.MaintainUser = simulation.MaintainUser;
                    wipItem.TransStatus = TransactionStatus.TransactionStatus_YES;

                    if (mINNO.Qty.ToString() != string.Empty && Convert.ToInt32(mINNO.Qty) != 0)
                    {
                        wipItem.Qty = mINNO.Qty * simulation.LotQty;
                    }
                    else
                    {
                        wipItem.Qty = simulation.LotQty;
                    }

                    //Laws Lu,2005/12/20,����	�ɼ�����
                    wipItem.ActionType = (int)MaterialType.CollectMaterial;
                    wipItem.MOSeq = simulation.MOSeq;   // Added by Icyer 2007/07/02

                    dataCollectFacade.AddLotOnWIPItem(wipItem);

                    LotSimulationReport simulationRpt = dataCollectFacade.GetLastLotSimulationReport(wipItem.LotCode);
                    if (simulationRpt != null)
                    {
                        dataCollectFacade.UpdateLotSimulationReport(simulationRpt);

                        // End Added
                    }
                    i++;
                }
                else
                {
                    //add by Jarvis 20120316 For ����
                    DeductQty(actionEventArgs, dataCollectFacade, mINNO);
                }
                
            }
            seqForDeductQty = 0;
            return returnValue;
        }


        //Laws Lu,2005/12/23,����INNO���ϼ�¼
        //Laws Lu,2006/01/06,�����¼Lot�ϵ���ϸ��¼
        public void InsertINNOOnWipItem(ActionEventArgs actionEventArgs, DataCollectFacade dataCollectFacade)
        {
            string iNNO = ((CINNOActionEventArgs)actionEventArgs).INNO;
            LotSimulation simulation = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulation;
            LotSimulationReport simulationReport = ((CINNOActionEventArgs)actionEventArgs).ProductInfo.NowSimulationReport;
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
                if (mINNO.ResourceCode != simulation.ResCode)
                    throw new Exception("$CS_INNOnotForResource $CS_Param_ResourceCode=" + mINNO.ResourceCode);

                LotOnWipItem wipItem = new LotOnWipItem();


                wipItem.DateCode = mINNO.DateCode;
                wipItem.LOTNO = mINNO.MItemPackedNo;//.LotNO;
                wipItem.MItemCode = mINNO.MItemCode;/*ActionOnLineHelper.StringNull;*/
                wipItem.MSeq = i;
                wipItem.VendorCode = mINNO.VendorCode;
                wipItem.VendorItemCode = mINNO.VendorItemCode;
                wipItem.Version = mINNO.Version;
                wipItem.Eattribute1 = simulation.EAttribute1;
                wipItem.ItemCode = simulation.ItemCode;
                wipItem.ResCode = simulation.ResCode;
                wipItem.RouteCode = simulation.RouteCode;
                wipItem.LotCode = simulation.LotCode;
                wipItem.LotSeq = simulation.LotSeq;
                wipItem.SegmentCode = simulationReport.SegmentCode;
                wipItem.BeginShiftCode = simulationReport.BeginShiftCode;
                wipItem.ShiftTypeCode = simulationReport.ShiftTypeCode;
                wipItem.StepSequenceCode = simulationReport.StepSequenceCode;
                wipItem.BeginTimePeriodCode = simulationReport.BeginTimePeriodCode;
                wipItem.MOCode = simulation.MOCode;
                wipItem.ModelCode = simulation.ModelCode;
                wipItem.OPCode = simulation.OPCode;
                wipItem.CollectStatus = simulation.CollectStatus;
                wipItem.BeginDate = simulation.BeginDate;
                wipItem.BeginTime = simulation.BeginTime;
                wipItem.MaintainUser = simulation.MaintainUser;

                wipItem.Qty = mINNO.Qty * simulation.LotQty;
                wipItem.MCardType = MCardType.MCardType_INNO;
                //Laws Lu,2005/12/20,����	�ɼ�����
                wipItem.ActionType = (int)MaterialType.CollectMaterial;
                wipItem.MOSeq = simulation.MOSeq;   // Added by Icyer 2007/07/02

                dataCollectFacade.AddLotOnWIPItem(wipItem);

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
                        #endregion

                        // Added by Icyer 2005/08/16
                        // ���Ͽۿ��
                        // ��ʱ���� FOR ����P3�汾 MARK LEE 2005/08/22
                        // ȡ������ Icyer 2005/08/23
                        //Laws Lu,2005/10/20,����	ʹ�������ļ�����������ģ���Ƿ�ʹ��
                        if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                        {
                            BenQGuru.eMES.Material.WarehouseFacade wfacade = new WarehouseFacade(this.DataProvider);
                            wfacade.CollectMaterialStock(actionEventArgs.ProductInfo.NowSimulation.LotCode, actionEventArgs.ProductInfo.NowSimulation.LotSeq.ToString(), actionEventArgs.ProductInfo.NowSimulation.MOCode);
                        }
                        // Added end

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
                        #region ��д������Ϣ��  ��INNO
                        if (actionEventArgs.ActionType == ActionType.DataCollectAction_CollectINNO)
                        {
                            if (System.Configuration.ConfigurationSettings.AppSettings["NeedMaterialModule"].Trim() == "1")
                            {
                                wfacade = ((CINNOActionEventArgs)actionEventArgs).Warehouse;
                            }

                            InsertINNOOnWipItem(actionEventArgs, dataCollectFacade);

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
                            wfacade.CollectMaterialStock(actionEventArgs.ProductInfo.NowSimulation.LotCode, actionEventArgs.ProductInfo.NowSimulation.LotSeq.ToString(), actionEventArgs.ProductInfo.NowSimulation.MOCode);
                    }
                    // Added end

                    //��Action�����б�
                    actionCheckStatus.ActionList.Add(actionEventArgs);
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
                            wfacade.CollectMaterialStock(actionEventArgs.ProductInfo.NowSimulation.LotCode, actionEventArgs.ProductInfo.NowSimulation.LotSeq.ToString(), actionEventArgs.ProductInfo.NowSimulation.MOCode);
                    }
                    // Added end


                    //��Action�����б�
                    actionCheckStatus.ActionList.Add(actionEventArgs);


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

        public const string MCardType_INNO = "1";

    }


}
