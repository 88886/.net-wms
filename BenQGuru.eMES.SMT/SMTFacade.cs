using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Data;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.SMT;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.DataCollect;
using UserControl;
using BenQGuru.eMES.Domain.OutSourcing;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;

namespace BenQGuru.eMES.SMT
{
    /// <summary>
    /// SMTFacade ��ժҪ˵����
    /// �ļ���:		SMTFacade.cs
    /// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
    /// ������:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
    /// ��������:	2006-5-25 10:45:16
    /// �޸���:
    /// �޸�����:
    /// �� ��:	
    /// �� ��:	
    /// </summary>
    public class SMTFacade
    {
        private IDomainDataProvider _domainDataProvider = null;
        private FacadeHelper _helper = null;

        public SMTFacade()
        {
            this._helper = new FacadeHelper(DataProvider);
        }

        public SMTFacade(IDomainDataProvider domainDataProvider)
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

        #region Line Control
        /// <summary>
        /// �������� (����)
        /// </summary>
        public Messages AddStartLineLog(MachineFeeder machineFeeder, string operationType, string userCode)
        {
            Messages msg = new Messages();
            string lineCode = machineFeeder.StepSequenceCode;
            decimal dLastID = 1;
            string strSql = "SELECT * FROM tblSMTLineCtlLog WHERE LogID=(SELECT MAX(LogID) FROM tblSMTLineCtlLog) ";
            object[] objsTmp = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            if (objsTmp != null && objsTmp.Length > 0)
                dLastID = ((SMTLineControlLog)objsTmp[0]).LogID + 1;
            // ��ѯ�����Ƿ���Կ���
            bool bCanStart = CheckLine(lineCode);
            strSql = "SELECT * FROM tblSMTLineCtlLog WHERE LogID=(SELECT MAX(LogID) FROM tblSMTLineCtlLog WHERE SSCode='" + lineCode + "') ";
            object[] objLast = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            SMTLineControlLog lastLog = null;
            string strLastStatus = string.Empty;
            if (objLast != null && objLast.Length > 0)
            {
                lastLog = (SMTLineControlLog)objLast[0];
                strLastStatus = lastLog.LineStatus;
            }
            // дLog
            SMTLineControlLog log = new SMTLineControlLog();
            log.LogID = dLastID;
            log.ProductCode = machineFeeder.ProductCode;
            log.MOCode = machineFeeder.MOCode;
            log.MachineCode = machineFeeder.MachineCode;
            log.MachineStationCode = machineFeeder.MachineStationCode;
            log.StepSequenceCode = machineFeeder.StepSequenceCode;
            log.FeederSpecCode = machineFeeder.FeederSpecCode;
            log.FeederCode = machineFeeder.FeederCode;
            log.ReelNo = machineFeeder.ReelNo;
            log.MaterialCode = machineFeeder.MaterialCode;
            log.UnitQty = machineFeeder.UnitQty;
            Reel reel = (Reel)this.GetReel(machineFeeder.ReelNo);
            log.ReelQty = reel.Qty;
            log.ReelUsedQty = reel.UsedQty;
            log.NextReelNo = machineFeeder.NextReelNo;
            log.OperationType = operationType;
            log.Enabled = machineFeeder.Enabled;
            log.ReelCeaseFlag = machineFeeder.ReelCeaseFlag;
            log.LineStatusOld = strLastStatus;
            log.LineStatus = FormatHelper.BooleanToString(bCanStart);
            if (log.LineStatus != log.LineStatusOld)
                log.ChangeLineStatus = FormatHelper.BooleanToString(true);
            else
                log.ChangeLineStatus = FormatHelper.BooleanToString(false);
            log.LineStatusOld = FormatHelper.BooleanToString(FormatHelper.StringToBoolean(log.LineStatusOld));
            log.MaintainUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            log.MaintainDate = FormatHelper.TODateInt(dtNow);
            log.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.AddSMTLineControlLog(log);
            msg.Add(new Message(MessageType.Data, string.Empty, new object[] { log }));
            return msg;
        }
        /// <summary>
        /// �������� (������Ч)
        /// </summary>
        public Messages AddStartLineLog(string moCode, string stepSequenceCode, string operationType, string userCode)
        {
            Messages msg = new Messages();
            // ��ѯ���ϼ�¼
            MachineFeeder machineFeeder = null;
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition("SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' AND Enabled='1' AND StationEnabled='1' "));
            if (objs == null || objs.Length == 0)
            {
                return msg;
            }
            machineFeeder = (MachineFeeder)objs[0];
            string lineCode = machineFeeder.StepSequenceCode;
            decimal dLastID = 1;
            string strSql = "SELECT * FROM tblSMTLineCtlLog WHERE LogID=(SELECT MAX(LogID) FROM tblSMTLineCtlLog) ";
            object[] objsTmp = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            if (objsTmp != null && objsTmp.Length > 0)
                dLastID = ((SMTLineControlLog)objsTmp[0]).LogID + 1;
            // ��ѯ�����Ƿ���Կ���
            bool bCanStart = CheckLine(lineCode);
            strSql = "SELECT * FROM tblSMTLineCtlLog WHERE LogID=(SELECT MAX(LogID) FROM tblSMTLineCtlLog WHERE SSCode='" + lineCode + "') ";
            object[] objLast = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            SMTLineControlLog lastLog = null;
            string strLastStatus = string.Empty;
            if (objLast != null && objLast.Length > 0)
            {
                lastLog = (SMTLineControlLog)objLast[0];
                strLastStatus = lastLog.LineStatus;
            }
            // дLog
            SMTLineControlLog log = new SMTLineControlLog();
            log.LogID = dLastID;
            log.ProductCode = machineFeeder.ProductCode;
            log.MOCode = machineFeeder.MOCode;
            log.MachineCode = string.Empty;
            log.MachineStationCode = string.Empty;
            log.StepSequenceCode = machineFeeder.StepSequenceCode;
            log.FeederSpecCode = string.Empty;
            log.FeederCode = string.Empty;
            log.ReelNo = string.Empty;
            log.MaterialCode = string.Empty;
            log.UnitQty = 0;
            log.ReelQty = 0;
            log.ReelUsedQty = 0;
            log.NextReelNo = string.Empty;
            log.OperationType = operationType;
            log.Enabled = machineFeeder.Enabled;
            log.ReelCeaseFlag = FormatHelper.BooleanToString(false);
            log.LineStatusOld = strLastStatus;
            log.LineStatus = FormatHelper.BooleanToString(bCanStart);
            if (log.LineStatus != log.LineStatusOld)
                log.ChangeLineStatus = FormatHelper.BooleanToString(true);
            else
                log.ChangeLineStatus = FormatHelper.BooleanToString(false);
            log.LineStatusOld = FormatHelper.BooleanToString(FormatHelper.StringToBoolean(log.LineStatusOld));
            log.MaintainUser = userCode;

            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            log.MaintainDate = FormatHelper.TODateInt(dtNow);
            log.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.AddSMTLineControlLog(log);
            msg.Add(new Message(MessageType.Data, string.Empty, new object[] { log }));
            return msg;
        }
        /// <summary>
        /// ͣ�� (�Ͼ�����)
        /// </summary>
        public Messages AddStopLineLog(ReelQty reelQty, string operationType, string userCode)
        {
            string strSql = "SELECT * FROM tblMachineFeeder WHERE ReelNo='" + reelQty.ReelNo + "'";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                MachineFeeder mf = (MachineFeeder)objs[0];
                mf.ReelCeaseFlag = FormatHelper.BooleanToString(true);
                this.UpdateMachineFeeder(mf);
                return AddStopLineLog(mf, reelQty, operationType, userCode);
            }
            return new Messages();
        }
        /// <summary>
        /// ͣ�� (�Ͼ�����)
        /// </summary>
        private Messages AddStopLineLog(MachineFeeder machineFeeder, ReelQty reelQty, string operationType, string userCode)
        {
            Messages msg = new Messages();
            string lineCode = machineFeeder.StepSequenceCode;
            decimal dLastID = 1;
            /*
            string strSql = "SELECT * FROM tblSMTLineCtlLog WHERE LogID=(SELECT MAX(LogID) FROM tblSMTLineCtlLog) ";
            object[] objsTmp = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            if (objsTmp != null && objsTmp.Length > 0)
                dLastID = ((SMTLineControlLog)objsTmp[0]).LogID + 1;
            */
            string strSql = "SELECT SEQ_SMTLOGSEQUENCE.NextVal LogID FROM dual";
            object[] objsTmp = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            if (objsTmp != null && objsTmp.Length > 0)
            {
                dLastID = ((SMTLineControlLog)objsTmp[0]).LogID;
            }

            strSql = "SELECT * FROM tblSMTLineCtlLog WHERE LogID=(SELECT MAX(LogID) FROM tblSMTLineCtlLog WHERE SSCode='" + lineCode + "') ";
            object[] objLast = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            SMTLineControlLog lastLog = null;
            string strLastStatus = string.Empty;
            if (objLast != null && objLast.Length > 0)
            {
                lastLog = (SMTLineControlLog)objLast[0];
                strLastStatus = lastLog.LineStatus;
            }
            // дLog
            SMTLineControlLog log = new SMTLineControlLog();
            log.LogID = dLastID;
            log.ProductCode = machineFeeder.ProductCode;
            log.MOCode = machineFeeder.MOCode;
            log.MachineCode = machineFeeder.MachineCode;
            log.MachineStationCode = machineFeeder.MachineStationCode;
            log.StepSequenceCode = machineFeeder.StepSequenceCode;
            log.FeederSpecCode = machineFeeder.FeederSpecCode;
            log.FeederCode = machineFeeder.FeederCode;
            log.ReelNo = machineFeeder.ReelNo;
            log.MaterialCode = machineFeeder.MaterialCode;
            log.UnitQty = machineFeeder.UnitQty;
            log.ReelQty = reelQty.Qty;
            log.ReelUsedQty = reelQty.UsedQty + reelQty.UpdatedQty;
            log.NextReelNo = machineFeeder.NextReelNo;
            log.OperationType = operationType;
            log.Enabled = machineFeeder.Enabled;
            log.ReelCeaseFlag = machineFeeder.ReelCeaseFlag;
            log.LineStatusOld = strLastStatus;
            log.LineStatus = FormatHelper.BooleanToString(false);
            if (log.LineStatus != log.LineStatusOld)
                log.ChangeLineStatus = FormatHelper.BooleanToString(true);
            else
                log.ChangeLineStatus = FormatHelper.BooleanToString(false);
            log.LineStatusOld = FormatHelper.BooleanToString(FormatHelper.StringToBoolean(log.LineStatusOld));
            log.MaintainUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            log.MaintainDate = FormatHelper.TODateInt(dtNow);
            log.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.AddSMTLineControlLog(log);
            msg.Add(new Message(MessageType.Data, string.Empty, new object[] { log }));
            return msg;
        }
        /// <summary>
        /// ͣ�� (����ʧЧ)
        /// </summary>
        public Messages AddStopLineLog(string moCode, string stepSequenceCode, string operationType, string userCode)
        {
            Messages msg = new Messages();
            // ��ѯ���ϼ�¼
            MachineFeeder machineFeeder = null;
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition("SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "'"));
            if (objs == null || objs.Length == 0)
            {
                return msg;
            }
            machineFeeder = (MachineFeeder)objs[0];
            string lineCode = machineFeeder.StepSequenceCode;
            decimal dLastID = 1;
            string strSql = "SELECT * FROM tblSMTLineCtlLog WHERE LogID=(SELECT MAX(LogID) FROM tblSMTLineCtlLog) ";
            object[] objsTmp = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            if (objsTmp != null && objsTmp.Length > 0)
                dLastID = ((SMTLineControlLog)objsTmp[0]).LogID + 1;
            strSql = "SELECT * FROM tblSMTLineCtlLog WHERE LogID=(SELECT MAX(LogID) FROM tblSMTLineCtlLog WHERE SSCode='" + lineCode + "') ";
            object[] objLast = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            SMTLineControlLog lastLog = null;
            string strLastStatus = string.Empty;
            if (objLast != null && objLast.Length > 0)
            {
                lastLog = (SMTLineControlLog)objLast[0];
                strLastStatus = lastLog.LineStatus;
            }
            // дLog
            SMTLineControlLog log = new SMTLineControlLog();
            log.LogID = dLastID;
            log.ProductCode = machineFeeder.ProductCode;
            log.MOCode = machineFeeder.MOCode;
            log.MachineCode = string.Empty;
            log.MachineStationCode = string.Empty;
            log.StepSequenceCode = machineFeeder.StepSequenceCode;
            log.FeederSpecCode = string.Empty;
            log.FeederCode = string.Empty;
            log.ReelNo = string.Empty;
            log.MaterialCode = string.Empty;
            log.UnitQty = 0;
            log.ReelQty = 0;
            log.ReelUsedQty = 0;
            log.NextReelNo = string.Empty;
            log.OperationType = operationType;
            log.Enabled = machineFeeder.Enabled;
            log.ReelCeaseFlag = FormatHelper.BooleanToString(false);
            log.LineStatusOld = strLastStatus;
            log.LineStatus = FormatHelper.BooleanToString(false);
            if (log.LineStatus != log.LineStatusOld)
                log.ChangeLineStatus = FormatHelper.BooleanToString(true);
            else
                log.ChangeLineStatus = FormatHelper.BooleanToString(false);
            log.LineStatusOld = FormatHelper.BooleanToString(FormatHelper.StringToBoolean(log.LineStatusOld));
            log.MaintainUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            log.MaintainDate = FormatHelper.TODateInt(dtNow);
            log.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.AddSMTLineControlLog(log);
            msg.Add(new Message(MessageType.Data, string.Empty, new object[] { log }));
            return msg;
        }
        /// <summary>
        /// �������Ƿ������������
        /// </summary>
        public bool CheckLine(string stepSequenceCode)
        {
            // 1. ����Ƿ��п��õĹ���
            // 2. ����Ƿ��е���ͣ�ߵ��Ͼ����

            // 1. ����Ƿ��п��õĹ���
            string strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE Enabled='1' AND StationEnabled='1' AND SSCode='" + stepSequenceCode + "' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) == 0)
                return false;
            // 2. ����Ƿ��е���ͣ�ߵ��Ͼ����
            strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE Enabled='1' AND StationEnabled='1' AND SSCode='" + stepSequenceCode + "' AND ReelCeaseFlag='1' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
                return false;
            return true;
        }
        #endregion

        #region SMT Feeder/Reel Watch
        /// <summary>
        /// ��ѯ���ߡ���̨�����ϼ�¼
        /// </summary>
        public object[] LoadMachineFeederByLineMachine(string lineCode, string machineCode)
        {
            string strSql = string.Empty;
            // ��ѯ���ϼ�¼
            strSql = "SELECT * FROM tblMachineFeeder WHERE Enabled='1' AND StationEnabled='1' ";
            if (lineCode != string.Empty)
            {
                strSql += " AND SSCode='" + lineCode + "' ";
            }
            if (machineCode != string.Empty)
            {
                strSql += " AND MachineCode='" + machineCode + "'";
            }
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            return objs;
        }

        /// <summary>
        /// ���ݲ��ߡ���̨��ѯ�Ͼ�����
        /// </summary>
        /// <returns></returns>
        public object[] LoadReelQtyByLineMachine(string lineCode, string machineCode)
        {
            string strSql = string.Empty;
            // ��ѯ���ϼ�¼
            strSql = "SELECT * FROM tblReelQty WHERE ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE Enabled='1' AND StationEnabled='1' ";
            if (lineCode != string.Empty)
            {
                strSql += " AND SSCode='" + lineCode + "' ";
            }
            if (machineCode != string.Empty)
            {
                strSql += " AND MachineCode='" + machineCode + "'";
            }
            strSql += ") ";
            object[] objs = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
            return objs;
        }

        /// <summary>
        /// ���ݲ��ߡ���̨��ѯFeeder����
        /// </summary>
        public object[] LoadFeederByLineMachine(string lineCode, string machineCode)
        {
            string strSql = string.Empty;
            // ��ѯ���ϼ�¼
            strSql = "SELECT * FROM tblFeeder WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE Enabled='1' AND StationEnabled='1' ";
            if (lineCode != string.Empty)
            {
                strSql += " AND SSCode='" + lineCode + "' ";
            }
            if (machineCode != string.Empty)
            {
                strSql += " AND MachineCode='" + machineCode + "'";
            }
            strSql += ")";
            object[] objs = this.DataProvider.CustomQuery(typeof(Feeder), new SQLCondition(strSql));
            return objs;
        }

        /// <summary>
        /// ���SMTԤ��
        /// </summary>
        public void AddSMTAlertInWatch(MachineFeeder machineFeeder, object alertObj)
        {
            BenQGuru.eMES.BaseSetting.ShiftModelFacade shiftModel = new BenQGuru.eMES.BaseSetting.ShiftModelFacade(this.DataProvider);
            string strLogSmtAlert = "0";
            BenQGuru.eMES.BaseSetting.SystemSettingFacade sysFacade = new BenQGuru.eMES.BaseSetting.SystemSettingFacade(this.DataProvider);
            object objTmp = sysFacade.GetParameter("SMT_ALERT_LOG", "SMT_LOG");
            if (objTmp != null)
                strLogSmtAlert = ((BenQGuru.eMES.Domain.BaseSetting.Parameter)objTmp).ParameterAlias;
            if (strLogSmtAlert != "1")
                return;
            string strSql = "SELECT COUNT(ALERTSEQ) FROM tblSMTAlert WHERE MachineCode='" + machineFeeder.MachineCode + "' AND MachineStationCode='" + machineFeeder.MachineStationCode + "' ";
            if (alertObj is ReelQty)
            {
                strSql += " AND ReelNo='" + ((ReelQty)alertObj).ReelNo + "' AND AlertType='" + SMTAlertType.Reel + "' ";
            }
            else
            {
                strSql += " AND FeederCode='" + ((Feeder)alertObj).FeederCode + "' AND AlertType='" + SMTAlertType.Feeder + "' ";
            }
            strSql += " AND AlertStatus='" + SMTAlertStatus.Initial + "'";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
                return;
            /*
            strSql = "SELECT * FROM tblSMTAlert WHERE AlertSeq=(SELECT MAX(AlertSeq) FROM tblSMTAlert)";
            object[] obj = this.DataProvider.CustomQuery(typeof(SMTAlert), new SQLCondition(strSql));
            decimal iSeq = 1;
            if (obj != null && obj.Length > 0)
                iSeq = ((SMTAlert)obj[0]).AlertSeq + 1;
            */
            strSql = "SELECT SEQ_SMTLOGSEQUENCE.NextVal AlertSeq FROM dual";
            object[] obj = this.DataProvider.CustomQuery(typeof(SMTAlert), new SQLCondition(strSql));
            decimal iSeq = 1;
            if (obj != null && obj.Length > 0)
            {
                iSeq = ((SMTAlert)obj[0]).AlertSeq;
            }

            SMTAlert alert = new SMTAlert();
            alert.AlertSeq = iSeq;
            if (alertObj is ReelQty)
                alert.AlertType = SMTAlertType.Reel;
            else
                alert.AlertType = SMTAlertType.Feeder;
            alert.ProductCode = machineFeeder.ProductCode;
            alert.StepSequenceCode = machineFeeder.StepSequenceCode;
            alert.MOCode = machineFeeder.MOCode;
            alert.MachineCode = machineFeeder.MachineCode;
            alert.MachineStationCode = machineFeeder.MachineStationCode;
            alert.FeederCode = machineFeeder.FeederCode;
            alert.ReelNo = machineFeeder.ReelNo;
            if (alertObj is Feeder)
            {
                Feeder feeder = (Feeder)alertObj;
                alert.FeederMaxCount = feeder.MaxCount;
                alert.FeederAlertCount = feeder.AlertCount;
                alert.FeederUsedCount = feeder.UsedCount;
            }
            else
            {
                ReelQty reel = (ReelQty)alertObj;
                alert.ReelQty = reel.Qty;
                alert.ReelUsedQty = reel.UsedQty + reel.UpdatedQty;
            }
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            alert.AlertDate = FormatHelper.TODateInt(dtNow);
            alert.AlertTime = FormatHelper.TOTimeInt(dtNow);
            alert.AlertStatus = SMTAlertStatus.Initial;
            alert.AlertLevel = string.Empty;
            alert.MaintainUser = string.Empty;
            alert.MaintainDate = alert.AlertDate;
            alert.MaintainTime = alert.AlertTime;
            this.AddSMTAlert(alert);
        }

        /// <summary>
        /// ����Feeder״̬
        /// </summary>
        public void UpdateFeederStatusInWatch(Feeder feeder)
        {
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
            string strNewStatus = string.Empty;
            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);
            DateTime mDate = FormatHelper.ToDateTime(Convert.ToInt32(feeder.TheMaintainDate), 000001);
            int alterDate = dtNow.Subtract(mDate).Days; //�㷨��now��ȥ��������

            if ((feeder.UsedCount >= feeder.AlertCount && feeder.UsedCount < feeder.MaxCount) ||
                (alterDate >= feeder.AlterMDay && alterDate < feeder.MaxMDay))
                strNewStatus = FeederStatus.WaitCheck;

            if (feeder.UsedCount >= feeder.MaxCount || alterDate >= feeder.MaxMDay)
                strNewStatus = FeederStatus.Disabled;

            if (strNewStatus == feeder.Status)
                return;

            string strSql = "UPDATE tblFeeder SET Status='" + strNewStatus + "' ,MDate=" + FormatHelper.TODateInt(dtNow).ToString() + " ,MTime=" + FormatHelper.TOTimeInt(dtNow) + " WHERE FeederCode='" + feeder.FeederCode + "'";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            string strOldStatus = feeder.Status;
            feeder.Status = strNewStatus;
            this.WriteFeederLog(feeder, strOldStatus, "Alert", "SYSTEM");
        }
        #endregion

        #region SMT Sensor Collect

        /// <summary>
        /// SMT���ϣ����빤��������added by Gawain@20130704
        /// </summary>
        /// <param name="lineCode"></param>
        /// <param name="qty"></param>
        /// <param name="unionNum"></param>
        /// <returns></returns>
        public string SensorCollectWithMo(string lineCode, string qty, string unionNum,string mocode)
        {
            lineCode = lineCode.Trim().ToUpper();
            // ת������
            int iQty = 0;
            try
            {
                iQty = int.Parse(qty);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            if (iQty == 0)
                return string.Empty;
            // ��ѯ���߶�Ӧ�Ļ�̨
            string strSql = "SELECT COUNT(FeederCode) FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND mocode='"+mocode +"' and Enabled='1' AND StationEnabled='1' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) == 0)
                return string.Empty;
            //��ϵͳ������ȡ��������
            SystemSettingFacade _SystemSettingFacade = new SystemSettingFacade();
            object parameter = _SystemSettingFacade.GetParameter("SMT_AUTO_MATERIAL", "SMT_MATERIAL");
            decimal rate = 0;
            if (parameter != null)
            {
                rate = decimal.Parse(((Parameter)parameter).ParameterAlias);
            }
            // �����Ͼ�����
            if (iQty > 0)
                strSql = "UPDATE tblFeeder SET UsedCount=UsedCount+" + (1 + rate).ToString() + "*" + iQty.ToString() + "*CurrUnitQty WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            else
                strSql = "UPDATE tblFeeder SET UsedCount=UsedCount" + (1 + rate).ToString() + "*" + iQty.ToString() + "*CurrUnitQty WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));

            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            // ����Feeder״̬
            strSql = "UPDATE tblFeeder SET Status='" + FeederStatus.WaitCheck + "',MDate=" + FormatHelper.TODateInt(dtNow).ToString() + ",MTime=" + FormatHelper.TOTimeInt(dtNow).ToString();
            strSql += " WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1') AND UsedCount>=AlertCount AND UsedCount<MaxCount ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            strSql = "UPDATE tblFeeder SET Status='" + FeederStatus.Disabled + "',MDate=" + FormatHelper.TODateInt(dtNow).ToString() + ",MTime=" + FormatHelper.TOTimeInt(dtNow).ToString();
            strSql += " WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1') AND UsedCount>=MaxCount ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));

            // �����Ͼ�����
            if (iQty > 0)
                strSql = "UPDATE tblReelQty SET UsedQty=UsedQty+" + (1 + rate).ToString() + "*" + iQty.ToString() + "*UnitQty WHERE ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            else
                strSql = "UPDATE tblReelQty SET UsedQty=UsedQty" + (1 + rate).ToString() + "*" + iQty.ToString() + "*UnitQty WHERE ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));

            // ����
            strSql = "SELECT * FROM tblReelQty WHERE UsedQty+UpdatedQty>=Qty AND ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            object[] objReels = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
            if (objReels != null && objReels.Length > 0)
            {
                // ����INNO @20060823
                ArrayList listMachine = new ArrayList();
                for (int i = 0; i < objReels.Length; i++)
                {
                    ReelQty reelQty = (ReelQty)objReels[i];
                    strSql = "SELECT * FROM tblMachineFeeder WHERE ReelNo='" + reelQty.ReelNo + "'";
                    object[] objsTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
                    if (objsTmp != null && objsTmp.Length > 0)
                    {
                        MachineFeeder mf = (MachineFeeder)objsTmp[0];
                        if (mf.NextReelNo != string.Empty)
                        {
                            decimal dOver = reelQty.UsedQty + reelQty.UpdatedQty - reelQty.Qty;
                            // ��ȥ����������ӵ���һ�Ͼ�
                            if (dOver > 0)
                            {
                                strSql = "UPDATE tblReelQty SET UsedQty=UsedQty-" + dOver.ToString() + " WHERE ReelNo='" + reelQty.ReelNo + "'";
                                this.DataProvider.CustomExecute(new SQLCondition(strSql));
                                strSql = "UPDATE tblReelQty SET UsedQty=UsedQty+" + dOver.ToString() + " WHERE ReelNo='" + mf.NextReelNo + "'";
                                this.DataProvider.CustomExecute(new SQLCondition(strSql));
                            }
                            // ���¾��Ͼ�����
                            this.UpdateMachineFeederReelQty(mf.MOCode, mf.MachineCode, mf.MachineStationCode, reelQty.ReelNo, SMTLoadFeederOperationType.Continue, "SYSTEM");
                            // �������ϼ�¼����
                            mf.ReelNo = mf.NextReelNo;
                            Reel reel = (Reel)this.GetReel(mf.NextReelNo);
                            mf.MaterialCode = reel.PartNo;
                            mf.NextReelNo = string.Empty;

                            mf.MaintainDate = FormatHelper.TODateInt(dtNow);
                            mf.MaintainTime = FormatHelper.TOTimeInt(dtNow);
                            this.UpdateMachineFeeder(mf);
                            // ����INNO @20060823
                            if (listMachine.Contains(mf.MOCode + ":" + mf.StepSequenceCode + ":" + mf.MachineCode) == false)
                            {
                                listMachine.Add(mf.MOCode + ":" + mf.StepSequenceCode + ":" + mf.MachineCode);
                            }
                        }
                    }
                }
                // ����INNO @20060823
                for (int i = 0; i < listMachine.Count; i++)
                {
                    string strKey = listMachine[i].ToString();
                    string[] updatedMachine = strKey.Split(':');
                    this.GenerateMachineInno(updatedMachine[0], updatedMachine[1], updatedMachine[2], "SYSTEM");
                }
            }
            // ��ѯ��Ρ�ʱ��
            BenQGuru.eMES.BaseSetting.ShiftModelFacade shiftModel = new BenQGuru.eMES.BaseSetting.ShiftModelFacade(this.DataProvider);
            string strSmtSegment = "SMT";
            BenQGuru.eMES.BaseSetting.SystemSettingFacade sysFacade = new BenQGuru.eMES.BaseSetting.SystemSettingFacade(this.DataProvider);
            object objTmp = sysFacade.GetParameter("SMT_SEGMENT", "SMT_LINE_MACHINE");
            if (objTmp != null)
                strSmtSegment = ((BenQGuru.eMES.Domain.BaseSetting.Parameter)objTmp).ParameterAlias;
            BenQGuru.eMES.BaseSetting.BaseModelFacade modelFacade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider);
            objTmp = modelFacade.GetSegment(strSmtSegment);

            string strShiftTypeCode = string.Empty;
            StepSequence ss = (StepSequence)modelFacade.GetStepSequence(lineCode);
            if (ss != null)
                strShiftTypeCode = ss.ShiftTypeCode;
            else
            {
                object[] objTmps = shiftModel.GetAllShiftType();
                if (objTmps == null || objTmps.Length == 0)
                    return string.Empty;
                strShiftTypeCode = ((BenQGuru.eMES.Domain.BaseSetting.ShiftType)objTmps[0]).ShiftTypeCode;
            }

            BenQGuru.eMES.Domain.BaseSetting.TimePeriod period = (BenQGuru.eMES.Domain.BaseSetting.TimePeriod)shiftModel.GetTimePeriod(strShiftTypeCode, FormatHelper.TOTimeInt(dtNow));
            // ���²�������
            int iUnionNum = 1;
            if (unionNum != string.Empty && unionNum != "0")
                iUnionNum = int.Parse(unionNum);
            int iQtyUpdate = Convert.ToInt32(qty) * iUnionNum;
            int iShiftDay = FormatHelper.TODateInt(dtNow);
            if (FormatHelper.StringToBoolean(period.IsOverDate) == true)
            {
                if (period.TimePeriodBeginTime < period.TimePeriodEndTime)
                {
                    iShiftDay = FormatHelper.TODateInt(dtNow.AddDays(-1));
                }
                else if (FormatHelper.TOTimeInt(dtNow) < period.TimePeriodBeginTime)
                {
                    iShiftDay = FormatHelper.TODateInt(dtNow.AddDays(-1));
                }
            }
            this.UpdateSensorQty(lineCode, iQtyUpdate, iShiftDay, period);

            return string.Empty;
        }


        public string SensorCollect(string lineCode, string qty, string unionNum)
        {
            lineCode = lineCode.Trim().ToUpper();
            // ת������
            int iQty = 0;
            try
            {
                iQty = int.Parse(qty);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            if (iQty == 0)
                return string.Empty;
            // ��ѯ���߶�Ӧ�Ļ�̨
            string strSql = "SELECT COUNT(FeederCode) FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) == 0)
                return string.Empty;
            //��ϵͳ������ȡ��������
            SystemSettingFacade _SystemSettingFacade = new SystemSettingFacade();
            object parameter = _SystemSettingFacade.GetParameter("SMT_AUTO_MATERIAL", "SMT_MATERIAL");
            decimal rate = 0;
            if (parameter != null)
            {
                rate = decimal.Parse(((Parameter)parameter).ParameterAlias);
            }
            // �����Ͼ�����
            if (iQty > 0)
                strSql = "UPDATE tblFeeder SET UsedCount=UsedCount+" + (1 + rate).ToString() + "*" + iQty.ToString() + "*CurrUnitQty WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            else
                strSql = "UPDATE tblFeeder SET UsedCount=UsedCount" + (1 + rate).ToString() + "*" + iQty.ToString() + "*CurrUnitQty WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));

            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            // ����Feeder״̬
            strSql = "UPDATE tblFeeder SET Status='" + FeederStatus.WaitCheck + "',MDate=" + FormatHelper.TODateInt(dtNow).ToString() + ",MTime=" + FormatHelper.TOTimeInt(dtNow).ToString();
            strSql += " WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1') AND UsedCount>=AlertCount AND UsedCount<MaxCount ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            strSql = "UPDATE tblFeeder SET Status='" + FeederStatus.Disabled + "',MDate=" + FormatHelper.TODateInt(dtNow).ToString() + ",MTime=" + FormatHelper.TOTimeInt(dtNow).ToString();
            strSql += " WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1') AND UsedCount>=MaxCount ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));

            // �����Ͼ�����
            if (iQty > 0)
                strSql = "UPDATE tblReelQty SET UsedQty=UsedQty+" + (1 + rate).ToString() + "*" + iQty.ToString() + "*UnitQty WHERE ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            else
                strSql = "UPDATE tblReelQty SET UsedQty=UsedQty" + (1 + rate).ToString() + "*" + iQty.ToString() + "*UnitQty WHERE ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));

            // ����
            strSql = "SELECT * FROM tblReelQty WHERE UsedQty+UpdatedQty>=Qty AND ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1')";
            object[] objReels = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
            if (objReels != null && objReels.Length > 0)
            {
                // ����INNO @20060823
                ArrayList listMachine = new ArrayList();
                for (int i = 0; i < objReels.Length; i++)
                {
                    ReelQty reelQty = (ReelQty)objReels[i];
                    strSql = "SELECT * FROM tblMachineFeeder WHERE ReelNo='" + reelQty.ReelNo + "'";
                    object[] objsTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
                    if (objsTmp != null && objsTmp.Length > 0)
                    {
                        MachineFeeder mf = (MachineFeeder)objsTmp[0];
                        if (mf.NextReelNo != string.Empty)
                        {
                            decimal dOver = reelQty.UsedQty + reelQty.UpdatedQty - reelQty.Qty;
                            // ��ȥ����������ӵ���һ�Ͼ�
                            if (dOver > 0)
                            {
                                strSql = "UPDATE tblReelQty SET UsedQty=UsedQty-" + dOver.ToString() + " WHERE ReelNo='" + reelQty.ReelNo + "'";
                                this.DataProvider.CustomExecute(new SQLCondition(strSql));
                                strSql = "UPDATE tblReelQty SET UsedQty=UsedQty+" + dOver.ToString() + " WHERE ReelNo='" + mf.NextReelNo + "'";
                                this.DataProvider.CustomExecute(new SQLCondition(strSql));
                            }
                            // ���¾��Ͼ�����
                            this.UpdateMachineFeederReelQty(mf.MOCode, mf.MachineCode, mf.MachineStationCode, reelQty.ReelNo, SMTLoadFeederOperationType.Continue, "SYSTEM");
                            // �������ϼ�¼����
                            mf.ReelNo = mf.NextReelNo;
                            Reel reel = (Reel)this.GetReel(mf.NextReelNo);
                            mf.MaterialCode = reel.PartNo;
                            mf.NextReelNo = string.Empty;

                            mf.MaintainDate = FormatHelper.TODateInt(dtNow);
                            mf.MaintainTime = FormatHelper.TOTimeInt(dtNow);
                            this.UpdateMachineFeeder(mf);
                            // ����INNO @20060823
                            if (listMachine.Contains(mf.MOCode + ":" + mf.StepSequenceCode + ":" + mf.MachineCode) == false)
                            {
                                listMachine.Add(mf.MOCode + ":" + mf.StepSequenceCode + ":" + mf.MachineCode);
                            }
                        }
                    }
                }
                // ����INNO @20060823
                for (int i = 0; i < listMachine.Count; i++)
                {
                    string strKey = listMachine[i].ToString();
                    string[] updatedMachine = strKey.Split(':');
                    this.GenerateMachineInno(updatedMachine[0], updatedMachine[1], updatedMachine[2], "SYSTEM");
                }
            }
            // ��ѯ��Ρ�ʱ��
            BenQGuru.eMES.BaseSetting.ShiftModelFacade shiftModel = new BenQGuru.eMES.BaseSetting.ShiftModelFacade(this.DataProvider);
            string strSmtSegment = "SMT";
            BenQGuru.eMES.BaseSetting.SystemSettingFacade sysFacade = new BenQGuru.eMES.BaseSetting.SystemSettingFacade(this.DataProvider);
            object objTmp = sysFacade.GetParameter("SMT_SEGMENT", "SMT_LINE_MACHINE");
            if (objTmp != null)
                strSmtSegment = ((BenQGuru.eMES.Domain.BaseSetting.Parameter)objTmp).ParameterAlias;
            BenQGuru.eMES.BaseSetting.BaseModelFacade modelFacade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider);
            objTmp = modelFacade.GetSegment(strSmtSegment);

            string strShiftTypeCode = string.Empty;
            StepSequence ss = (StepSequence)modelFacade.GetStepSequence(lineCode);
            if (ss != null)
                strShiftTypeCode = ss.ShiftTypeCode;
            else
            {
                object[] objTmps = shiftModel.GetAllShiftType();
                if (objTmps == null || objTmps.Length == 0)
                    return string.Empty;
                strShiftTypeCode = ((BenQGuru.eMES.Domain.BaseSetting.ShiftType)objTmps[0]).ShiftTypeCode;
            }

            BenQGuru.eMES.Domain.BaseSetting.TimePeriod period = (BenQGuru.eMES.Domain.BaseSetting.TimePeriod)shiftModel.GetTimePeriod(strShiftTypeCode, FormatHelper.TOTimeInt(dtNow));
            // ���²�������
            int iUnionNum = 1;
            if (unionNum != string.Empty && unionNum != "0")
                iUnionNum = int.Parse(unionNum);
            int iQtyUpdate = Convert.ToInt32(qty) * iUnionNum;
            int iShiftDay = FormatHelper.TODateInt(dtNow);
            if (FormatHelper.StringToBoolean(period.IsOverDate) == true)
            {
                if (period.TimePeriodBeginTime < period.TimePeriodEndTime)
                {
                    iShiftDay = FormatHelper.TODateInt(dtNow.AddDays(-1));
                }
                else if (FormatHelper.TOTimeInt(dtNow) < period.TimePeriodBeginTime)
                {
                    iShiftDay = FormatHelper.TODateInt(dtNow.AddDays(-1));
                }
            }
            this.UpdateSensorQty(lineCode, iQtyUpdate, iShiftDay, period);

            return string.Empty;
        }


        /// <summary>
        /// ��ѯ�����µ����л�̨�����б�
        /// </summary>
        /// <param name="lineCode"></param>
        /// <returns></returns>
        public string GetMachineListByLineCode(string lineCode)
        {
            BenQGuru.eMES.BaseSetting.SystemSettingFacade sysFacade = new BenQGuru.eMES.BaseSetting.SystemSettingFacade(this.DataProvider);
            object objLine = sysFacade.GetParameter("SMT_LINE_" + lineCode, "SMT_LINE_MACHINE");
            if (objLine == null)
                return string.Empty;
            string strMachineString = ((BenQGuru.eMES.Domain.BaseSetting.Parameter)objLine).ParameterDescription;
            if (strMachineString == string.Empty)
                return string.Empty;
            strMachineString = strMachineString.Replace(" ", "");
            return strMachineString;
        }

        /// <summary>
        /// Sensor�ӿ��и�������
        /// </summary>
        private void UpdateSensorQty(string lineCode, int qty, int shiftDay, BenQGuru.eMES.Domain.BaseSetting.TimePeriod tp)
        {
            string strSql = "SELECT DISTINCT MOCode,ProductCode,SSCode FROM tblMachineFeeder WHERE SSCode='" + lineCode + "' AND Enabled='1' AND StationEnabled='1'";
            object[] objsmf = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objsmf == null || objsmf.Length == 0)
                return;
            for (int i = 0; i < objsmf.Length; i++)
            {
                MachineFeeder mf = (MachineFeeder)objsmf[i];
                // ��ѯ�����ƻ��������Ƿ�ﵽ����Ŀ��
                BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
                MO mo = (MO)moFacade.GetMO(mf.MOCode);
                int iSumQty = 0;
                try
                {
                    strSql = "SELECT SUM(Qty) FROM tblSMTSensorQty WHERE MOCode='" + mf.MOCode + "'";
                    iSumQty = this.DataProvider.GetCount(new SQLCondition(strSql));
                }
                catch
                { }
                bool bMOEnd = false;
                if (iSumQty < mo.MOPlanQty && iSumQty + qty >= mo.MOPlanQty)
                    bMOEnd = true;
                SMTSensorQty sensorQty = (SMTSensorQty)this.GetSMTSensorQty(mf.ProductCode, mf.MOCode, mf.StepSequenceCode, shiftDay, tp.ShiftTypeCode, tp.ShiftCode, tp.TimePeriodCode);

                //2006/11/17,Laws Lu add get DateTime from db Server
                DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

                DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

                if (sensorQty == null)
                {
                    sensorQty = new SMTSensorQty();
                    sensorQty.ProductCode = mf.ProductCode;
                    sensorQty.StepSequenceCode = mf.StepSequenceCode;
                    sensorQty.MOCode = mf.MOCode;
                    sensorQty.ShiftDay = shiftDay;
                    sensorQty.ShiftTypeCode = tp.ShiftTypeCode;
                    sensorQty.ShiftCode = tp.ShiftCode;
                    sensorQty.TPCode = tp.TimePeriodCode;
                    sensorQty.TPBeginTime = tp.TimePeriodBeginTime;
                    sensorQty.TPEndTime = tp.TimePeriodEndTime;
                    sensorQty.TPSequence = tp.TimePeriodSequence;
                    // ��ѯ�����Ƿ��һ��Ͷ������
                    strSql = "SELECT COUNT(*) FROM tblSMTSensorQty WHERE MOCode='" + sensorQty.MOCode + "'";

                    if (this.DataProvider.GetCount(new SQLCondition(strSql)) == 0)
                    {
                        sensorQty.MOBeginDate = FormatHelper.TODateInt(dtNow);
                        sensorQty.MOBeginTime = FormatHelper.TOTimeInt(dtNow);
                    }
                    if (bMOEnd == true)
                    {
                        sensorQty.MOEndDate = FormatHelper.TODateInt(dtNow);
                        sensorQty.MOEndTime = FormatHelper.TOTimeInt(dtNow);
                    }
                    sensorQty.Qty = qty;
                    sensorQty.MaintainUser = "SYSTEM";
                    sensorQty.MaintainDate = FormatHelper.TODateInt(dtNow);
                    sensorQty.MaintainTime = FormatHelper.TOTimeInt(dtNow);
                    this.AddSMTSensorQty(sensorQty);
                }
                else
                {
                    if (bMOEnd == true)
                    {
                        sensorQty.MOEndDate = FormatHelper.TODateInt(dtNow);
                        sensorQty.MOEndTime = FormatHelper.TOTimeInt(dtNow);
                    }
                    sensorQty.Qty += qty;
                    sensorQty.MaintainDate = FormatHelper.TODateInt(dtNow);
                    sensorQty.MaintainTime = FormatHelper.TOTimeInt(dtNow);
                    this.UpdateSMTSensorQty(sensorQty);
                }
            }
        }
        #endregion

        #region SMT Load Feeder
        /// <summary>
        /// added by Gawain.Gu for ��Ʒ���Ͼ����@20130413
        /// </summary>
        /// <param name="moCode"></param>
        /// <param name="sscode"></param>
        /// <returns></returns>
        public object[] GetMachineLoadedFeeder(string moCode, string sscode)
        {
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + sscode + "' and  Enabled='1' and StationEnabled='1' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            return objs;
        }

        public object[] GetMachineLoadedFeeder(string moCode, string sscode, string machineCode)
        {
            return GetMachineLoadedFeeder(moCode, sscode, machineCode, string.Empty);
        }
        public object[] GetMachineLoadedFeeder(string moCode, string sscode, string machineCode, string tableGroup)
        {
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND MachineCode='" + machineCode + "' AND SSCode='" + sscode + "' ";
            if (tableGroup != string.Empty)
                strSql += " AND TblGrp='" + tableGroup + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            return objs;
        }
        public object[] GetMachineLoadedFeeder(string moCode)
        {
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCode + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            return objs;
        }
        public string GetActiveStationTable(string moCode, string sscode, string machineCode)
        {
            string strSql = "SELECT DISTINCT TBLGRP FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND MachineCode='" + machineCode + "' AND SSCode='" + sscode + "' AND StationEnabled='1' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                return ((MachineFeeder)objs[0]).TableGroup;
            }
            return string.Empty;
        }

        /// <summary>
        /// SMT���ϣ��¹��������е�Feeder���Ͼ�
        /// </summary>
        public Messages SMTUnLoadByMOCode(string moCode, string userCode, string resourceCode, string stepSequenceCode)
        {
            Messages msg = new Messages();
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs == null || objs.Length == 0)
                return msg;
            //��鹤���Ƿ�ΪʧЧ״̬,Modify by Sandy on 20130605
            strSql = "Select count(*) from tblmachinefeeder where MOCode='" + moCode + "' AND sscode='" + stepSequenceCode + "' and ENABLED='1' and STATIONENABLED='1'";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
            {
                msg.Add(new Message(MessageType.Error, "$Error_CS_MO_Enable_Not_Unload"));
                return msg;
            }
            // ͣ��
            msg.AddMessages(this.AddStopLineLog(moCode, stepSequenceCode, SMTLoadFeederOperationType.UnLoad, userCode));
            // Added by Icyer 2007/01/16	�����������µ��Ͼ��¼����ֹ����
            strSql = "UPDATE tblReelQty SET MTime=MTime+1 WHERE MOCode='" + moCode + "' AND sscode='" + stepSequenceCode + "'";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            // Added end
            for (int i = 0; i < objs.Length; i++)
            {
                MachineFeeder mf = (MachineFeeder)objs[i];
                UpdateMachineFeederReelQty(moCode, mf.MachineCode, mf.MachineStationCode, mf.ReelNo, SMTLoadFeederOperationType.UnLoad, userCode);
                this.DeleteMachineFeeder(mf);
            }
            strSql = "DELETE FROM tblReelQty WHERE MOCode='" + moCode + "' AND sscode='" + stepSequenceCode + "'";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            return msg;
        }

        /// <summary>
        /// SMT����
        /// </summary>
        public Messages SMTLoadFeeder(MO mo, string machineCode, string stationCode, string reelNo, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode)
        {
            return SMTLoadFeeder(mo, machineCode, stationCode, reelNo, feederCode, userCode, tblLoaded, resourceCode, stepSequenceCode, true, string.Empty);
        }
        public Messages SMTLoadFeeder(MO mo, string machineCode, string stationCode, string reelNo, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, bool stationEnabled, string tableGroup)
        {
            Messages msg = SMTLoadFeederCore(mo, machineCode, stationCode, reelNo, feederCode, userCode, tblLoaded, resourceCode, stepSequenceCode, stationEnabled, tableGroup);
            if (msg.IsSuccess() == false)
            {
                this.AddMachineFeederFailure(mo, machineCode, stationCode, reelNo, feederCode, msg.OutPut(), userCode, tblLoaded, SMTLoadFeederOperationType.Load, resourceCode, stepSequenceCode);
            }
            return msg;
        }
        private Messages SMTLoadFeederCore(MO mo, string machineCode, string stationCode, string reelNo, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, bool stationEnabled, string tableGroup)
        {
            Messages msg = new Messages();
            // ����Ͼ�
            Reel reel = (Reel)this.GetReel(reelNo);
            if (reel == null)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Not_Exist"));
                return msg;
            }
            if (FormatHelper.StringToBoolean(reel.UsedFlag) == false || reel.MOCode != mo.MOCode)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_UnGetOut_Or_MOCode_Error"));
                return msg;
            }
            // ����Ͼ��Ƿ�������̨ʹ��
            string strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND (ReelNo='" + reel.ReelNo + "' OR NextReelNo='" + reel.ReelNo + "') ";
            object[] objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objTmp != null && objTmp.Length > 0)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Used_Already"));
                return msg;
            }
            // ���Feeder
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            if (feeder == null)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Not_Exist"));
                return msg;
            }
            // Feeder�Ƿ�����
            if (FormatHelper.StringToBoolean(feeder.UseFlag) == false)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Not_GetOut"));
                return msg;
            }
            // Feeder���õĹ���
            if (feeder.MOCode != mo.MOCode)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Not_In_This_MOCode"));
                return msg;
            }
            // Feeder״̬
            if (feeder.Status != FeederStatus.Normal)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Status_Error"));
                return msg;
            }
            // ���Feeder�Ƿ�������̨ʹ��
            strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND FeederCode='" + feeder.FeederCode + "'";
            objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objTmp != null && objTmp.Length > 0)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Used_Already"));
                return msg;
            }
            // ��ѯ��վ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + stationCode + "' AND FeederSpecCode='" + feeder.FeederSpecCode + "' AND MaterialCode='" + reel.PartNo + "' AND SSCode='" + stepSequenceCode + "'";
            if (tableGroup != null && tableGroup != string.Empty)
            {
                strSql += " AND TblGrp='" + tableGroup + "' ";
            }
            object[] smtFeeders = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (smtFeeders == null)
            {
                msg.Add(new Message(MessageType.Error, "$SMTFeederMaterial_Not_Exist"));
                return msg;
            }
            SMTFeederMaterial smtFeeder = (SMTFeederMaterial)smtFeeders[0];

            #region Removed
            /*
			// ��ѯ�Ͼ�ǰʹ������
			strSql = "SELECT * FROM tblReelQty WHERE ReelNo='" + reelNo + "' AND MOCode='" + mo.MOCode + "'";
			object[] reelQtys = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
			if (reelQtys != null)
			{
				decimal d = 0;
				for (int i = 0; i < reelQtys.Length; i++)
				{
					ReelQty reelQty = (ReelQty)reelQtys[i];
					d += reelQty.UsedQty;
				}
				if (d > 0)
				{
					strSql = "UPDATE tblReel SET UsedQty=UsedQty+" + d.ToString() + " WHERE ReelNo='" + reelNo + "'";
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					strSql = "DELETE FROM tblReelQty WHERE ReelNo='" + reelNo + "' AND MOCode='" + mo.MOCode + "'";
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					reel.UsedQty += d;
				}
			}
			*/
            #endregion

            // ����Ͼ�ʣ����������վһ��ʹ������
            decimal dReelAlertQty = this.GetReelAlertQty(reel);
            if (reel.UsedQty + smtFeeder.Qty > dReelAlertQty)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_UsedQty_Alert"));
                return msg;
            }
            // ���վλ�Ƿ����Ϲ���
            for (int i = 0; i < tblLoaded.Rows.Count; i++)
            {
                if (tblLoaded.Rows[i]["MachineCode"].ToString() == machineCode &&
                    tblLoaded.Rows[i]["MachineStationCode"].ToString() == stationCode &&
                    FormatHelper.StringToBoolean(tblLoaded.Rows[i]["CheckResult"].ToString()) == true)
                {
                    msg.Add(new Message(MessageType.Error, "$MachineStation_Loaded_Already"));
                    return msg;
                }
            }
            /*
            // ����Ƿ������������ϱȶ�
            strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE CheckResult='1' AND MOCode='" + mo.MOCode + "'";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) == 0)
            {
                strSql = "SELECT * FROM tblSMTCheckMaterial WHERE CheckID=(SELECT MAX(CheckID) FROM tblSMTCheckMaterial WHERE MOCode='" + mo.MOCode + "')";
                object[] objschk = this.DataProvider.CustomQuery(typeof(SMTCheckMaterial), new SQLCondition(strSql));
                bool bPass = false;
                if (objschk != null && objschk.Length > 0)
                {
                    SMTCheckMaterial chk = (SMTCheckMaterial)objschk[0];
                    if (chk.CheckResult == SMTCheckMaterialResult.Accept_Exception ||
                        chk.CheckResult == SMTCheckMaterialResult.Matchable)
                    {
                        bPass = true;
                    }
                }
                if (bPass == false)
                {
                    msg.Add(new Message(MessageType.Error, "$MOCode_Not_Check_SMTMaterial_MOBOM"));
                    return msg;
                }
            }
            */
            // ���ӻ�̨Feeder
            AddMachineFeederPass(mo, feeder, reel, smtFeeder, userCode, tblLoaded, SMTLoadFeederOperationType.Load, resourceCode, stepSequenceCode, string.Empty, string.Empty, stationEnabled);
            return msg;
        }

        /// <summary>
        /// SMT����
        /// </summary>
        public Messages SMTExchangeFeeder(string operationType, MO mo, string machineCode, string stationCode, string reelNoOld, string reelNo, string feederCodeOld, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, string tableGroup)
        {
            Messages msg = SMTExchangeFeederCore(operationType, mo, machineCode, stationCode, reelNoOld, reelNo, feederCodeOld, feederCode, userCode, tblLoaded, resourceCode, stepSequenceCode, tableGroup);
            if (msg.IsSuccess() == false)
            {
                this.AddMachineFeederFailure(mo, machineCode, stationCode, reelNo, feederCode, msg.OutPut(), userCode, tblLoaded, operationType, resourceCode, stepSequenceCode);
            }
            return msg;
        }
        private Messages SMTExchangeFeederCore(string operationType, MO mo, string machineCode, string stationCode, string reelNoOld, string reelNo, string feederCodeOld, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, string tableGroup)
        {
            Messages msg = new Messages();
            // ���ԭ�����Ƿ���ȷ
            int iOldIdx = -1;
            if (tblLoaded != null)
            {
                for (int i = 0; i < tblLoaded.Rows.Count; i++)
                {
                    if (tblLoaded.Rows[i]["MachineCode"].ToString() == machineCode &&
                        tblLoaded.Rows[i]["MachineStationCode"].ToString() == stationCode &&
                        tblLoaded.Rows[i]["ReelNo"].ToString() != string.Empty &&
                        FormatHelper.StringToBoolean(tblLoaded.Rows[i]["CheckResult"].ToString()) == true)
                    {
                        if (tblLoaded.Rows[i]["FeederCode"].ToString() != feederCodeOld)
                        {
                            msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_FeederCode_Not_Exist [" + feederCodeOld + "]"));
                            return msg;
                        }
                        if (tblLoaded.Rows[i]["ReelNo"].ToString() != reelNoOld)
                        {
                            msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_ReelNo_Not_Exist [" + reelNoOld + "]"));
                            return msg;
                        }
                        iOldIdx = i;
                        break;
                    }
                }
            }
            if (iOldIdx == -1)
            {
                msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Not_Exist"));
                return msg;
            }
            // ����Ͼ�
            Reel reel = (Reel)this.GetReel(reelNo);
            if (reel == null)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Not_Exist"));
                return msg;
            }
            if (reelNo != reelNoOld)
            {
                if (FormatHelper.StringToBoolean(reel.UsedFlag) == false || reel.MOCode != mo.MOCode)
                {
                    msg.Add(new Message(MessageType.Error, "$Reel_UnGetOut_Or_MOCode_Error"));
                    return msg;
                }
            }
            string strSql = string.Empty;
            object[] objTmp = null;
            // ����Ͼ��Ƿ�������̨ʹ��
            if (reel.ReelNo != reelNoOld)
            {
                strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND (ReelNo='" + reel.ReelNo + "' OR NextReelNo='" + reel.ReelNo + "') ";
                objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
                if (objTmp != null && objTmp.Length > 0)
                {
                    msg.Add(new Message(MessageType.Error, "$Reel_Used_Already"));
                    return msg;
                }
            }
            // ���Feeder
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            if (feeder == null)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Not_Exist"));
                return msg;
            }
            // ���Feeder����Ƿ�һ��
            bool bFeederSpecSame = true;
            if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == true)
            {
                if (feeder.FeederSpecCode != tblLoaded.Rows[iOldIdx]["FeederSpecCode"].ToString())
                {
                    bFeederSpecSame = false;
                    /*
                    msg.Add(new Message(MessageType.Error, "$New_Feeder_SpecCode_Error"));
                    return msg;
                    */
                }
            }
            // Feeder�Ƿ�����
            if (feederCode != feederCodeOld)
            {
                if (FormatHelper.StringToBoolean(feeder.UseFlag) == false)
                {
                    msg.Add(new Message(MessageType.Error, "$Feeder_Not_GetOut"));
                    return msg;
                }
            }
            // Feeder���õĹ���
            if (feeder.MOCode != mo.MOCode)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Not_In_This_MOCode"));
                return msg;
            }
            // Feeder״̬
            if (feeder.Status != FeederStatus.Normal)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Status_Error"));
                return msg;
            }
            // ���Feeder�Ƿ�������̨ʹ��
            if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == false ||
                feeder.FeederCode != feederCodeOld)
            {
                strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND FeederCode='" + feeder.FeederCode + "'";
                objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
                if (objTmp != null && objTmp.Length > 0)
                {
                    msg.Add(new Message(MessageType.Error, "$Feeder_Used_Already"));
                    return msg;
                }
            }
            // ������Ͼ���ԭ�Ͼ��Ϻ��Ƿ�һ��
            bool bMaterialSame = true;
            if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == true)
            {
                if (reel.PartNo != tblLoaded.Rows[iOldIdx]["MaterialCode"].ToString())
                {
                    bMaterialSame = false;
                }
            }
            // ��ѯ��վ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + stationCode + "' AND FeederSpecCode='" + feeder.FeederSpecCode + "' AND MaterialCode='" + reel.PartNo + "' AND SSCode='" + stepSequenceCode + "'";
            if (tableGroup != null && tableGroup != string.Empty)
            {
                strSql += " AND TblGrp='" + tableGroup + "' ";
            }
            object[] smtFeeders = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (smtFeeders == null)
            {
                msg.Add(new Message(MessageType.Error, "$SMTFeederMaterial_Not_Exist"));
                return msg;
            }
            SMTFeederMaterial smtFeeder = (SMTFeederMaterial)smtFeeders[0];

            if (reelNo == reelNoOld)
            {
                // ��ѯ�Ͼ�ǰʹ������
                strSql = "SELECT * FROM tblReelQty WHERE ReelNo='" + reel.ReelNo + "' AND MOCode='" + mo.MOCode + "'";
                object[] reelQtys = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
                if (reelQtys != null)
                {
                    decimal d = 0;
                    for (int i = 0; i < reelQtys.Length; i++)
                    {
                        ReelQty reelQty = (ReelQty)reelQtys[i];
                        d += reelQty.UsedQty;
                    }
                    if (d > 0)
                    {
                        reel.UsedQty += d;
                    }
                }
            }

            if (reelNo != reelNoOld)
            {
                // ����Ͼ�ʣ����������վһ��ʹ������
                decimal dReelAlertQty = this.GetReelAlertQty(reel);
                if (reel.UsedQty + smtFeeder.Qty > dReelAlertQty)
                {
                    msg.Add(new Message(MessageType.Error, "$Reel_UsedQty_Alert"));
                    return msg;
                }
            }
            // ���»����Ͼ��ʹ������
            if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == true)
            {
                UpdateMachineFeederReelQty(mo.MOCode, machineCode, stationCode, reelNoOld, operationType, userCode);
            }
            // ���ӻ�̨Feeder
            msg.AddMessages(AddMachineFeederPass(mo, feeder, reel, smtFeeder, userCode, tblLoaded, operationType, resourceCode, stepSequenceCode, feederCodeOld, reelNoOld));
            // ����INNO @20060823
            if (reel.ReelNo != reelNoOld)
            {
                int iEnabled = this.DataProvider.GetCount(new SQLCondition("SELECT COUNT(*) FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND SSCode='" + stepSequenceCode + "' AND Enabled='1'"));
                if (iEnabled > 0)
                {
                    this.GenerateMachineInno(mo.MOCode, stepSequenceCode, machineCode, userCode);
                }
            }
            //
            if (bFeederSpecSame == false)
            {
                msg.Add(new Message("$SMTLoad_NewReel_OldFeederSpec_Diff"));
            }
            if (bMaterialSame == false)
            {
                msg.Add(new Message("$SMTLoad_NewReel_OldReel_Diff"));
            }
            return msg;
        }

        /// <summary>
        /// ����
        /// </summary>
        public Messages SMTContinueFeeder(string operationType, MO mo, string machineCode, string stationCode, string reelNoOld, string reelNo, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, string tableGroup)
        {
            Messages msg = new Messages();
            // ���ԭ�����Ƿ���ȷ
            int iOldIdx = -1;
            if (tblLoaded != null)
            {
                for (int i = 0; i < tblLoaded.Rows.Count; i++)
                {
                    if (tblLoaded.Rows[i]["MachineCode"].ToString() == machineCode &&
                        tblLoaded.Rows[i]["MachineStationCode"].ToString() == stationCode &&
                        tblLoaded.Rows[i]["ReelNo"].ToString() != string.Empty &&
                        FormatHelper.StringToBoolean(tblLoaded.Rows[i]["CheckResult"].ToString()) == true)
                    {
                        if (tblLoaded.Rows[i]["ReelNo"].ToString() != reelNoOld)
                        {
                            msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_ReelNo_Not_Exist [" + reelNoOld + "]"));
                            return msg;
                        }
                        iOldIdx = i;
                        break;
                    }
                }
            }
            if (iOldIdx == -1)
            {
                msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Not_Exist"));
                return msg;
            }
            // ����Ͼ�
            Reel reel = (Reel)this.GetReel(reelNo);
            if (reel == null)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Not_Exist"));
                return msg;
            }
            if (FormatHelper.StringToBoolean(reel.UsedFlag) == false || reel.MOCode != mo.MOCode)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_UnGetOut_Or_MOCode_Error"));
                return msg;
            }
            // ����Ͼ��Ƿ�������̨ʹ��
            string strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND (ReelNo='" + reel.ReelNo + "' OR NextReelNo='" + reel.ReelNo + "') ";
            object[] objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objTmp != null && objTmp.Length > 0)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Used_Already"));
                return msg;
            }
            // ������Ͼ���ԭ�Ͼ��Ϻ��Ƿ�һ��
            bool bMaterialSame = true;
            if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == true)
            {
                if (reel.PartNo != tblLoaded.Rows[iOldIdx]["MaterialCode"].ToString())
                {
                    bMaterialSame = false;
                    /*
                    msg.Add(new Message(MessageType.Error, "$New_Reel_Material_Error"));
                    return msg;
                    */
                }
            }
            // ���Feeder
            Feeder feeder = (Feeder)this.GetFeeder(tblLoaded.Rows[iOldIdx]["FeederCode"].ToString());
            if (feeder == null)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Not_Exist"));
                return msg;
            }
            // ��ѯ��վ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + stationCode + "' AND FeederSpecCode='" + feeder.FeederSpecCode + "' AND MaterialCode='" + reel.PartNo + "' AND SSCode='" + stepSequenceCode + "'";
            if (tableGroup != null && tableGroup != string.Empty)
            {
                strSql += " AND TblGrp='" + tableGroup + "' ";
            }
            object[] smtFeeders = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (smtFeeders == null)
            {
                msg.Add(new Message(MessageType.Error, "$SMTFeederMaterial_Not_Exist"));
                return msg;
            }
            SMTFeederMaterial smtFeeder = (SMTFeederMaterial)smtFeeders[0];

            #region Removed
            /*
			// ��ѯ�Ͼ�ǰʹ������
			strSql = "SELECT * FROM tblReelQty WHERE ReelNo='" + reel.ReelNo + "' AND MOCode='" + mo.MOCode + "'";
			object[] reelQtys = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
			if (reelQtys != null)
			{
				decimal d = 0;
				for (int i = 0; i < reelQtys.Length; i++)
				{
					ReelQty reelQty = (ReelQty)reelQtys[i];
					d += reelQty.UsedQty;
				}
				if (d > 0)
				{
					strSql = "UPDATE tblReel SET UsedQty=UsedQty+" + d.ToString() + " WHERE ReelNo='" + reel.ReelNo + "'";
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					strSql = "DELETE FROM tblReelQty WHERE ReelNo='" + reel.ReelNo + "' AND MOCode='" + mo.MOCode + "'";
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					reel.UsedQty += d;
				}
			}
			*/
            #endregion

            // ����Ͼ�ʣ����������վһ��ʹ������
            decimal dReelAlertQty = this.GetReelAlertQty(reel);
            if (reel.UsedQty + smtFeeder.Qty > dReelAlertQty)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_UsedQty_Alert"));
                return msg;
            }
            strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND MachineCode='" + smtFeeder.MachineCode + "' AND MachineStationCode='" + smtFeeder.MachineStationCode + "'";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                MachineFeeder machineFeeder = (MachineFeeder)objs[0];
                if (FormatHelper.StringToBoolean(machineFeeder.Enabled) == false)
                {
                    msg.Add(new Message(MessageType.Error, "$SMT_Continue_MO_Disabled"));
                    return msg;
                }
            }
            // ���ӻ�̨Feeder
            msg.AddMessages(AddMachineFeederPass(mo, feeder, reel, smtFeeder, userCode, tblLoaded, operationType, resourceCode, stepSequenceCode, feeder.FeederCode, reelNoOld));
            if (bMaterialSame == false && msg.IsSuccess() == true)
            {
                msg.Add(new Message("$SMTLoad_NewReel_OldReel_Diff"));
            }
            return msg;
        }

        /// <summary>
        /// ����
        /// </summary>
        public Messages SMTUnLoadFeederSingle(MO mo, string machineCode, string machineStationCode, string feederCode, string reelNo, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, string tableGroup)
        {
            Messages msg = new Messages();
            // ��ѯ���ϼ�¼���Ƿ����
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + machineStationCode + "' AND ReelNo='" + reelNo + "' AND FeederCode='" + feederCode + "' AND CheckResult='1' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs == null || objs.Length == 0)
            {
                msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Not_Exist"));
                return msg;
            }
            MachineFeeder machineFeeder = (MachineFeeder)objs[0];

            Reel reel = (Reel)this.GetReel(reelNo);
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);

            // ��ѯ��վ�����Ƿ����
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + machineStationCode + "' AND FeederSpecCode='" + feeder.FeederSpecCode + "' AND MaterialCode='" + reel.PartNo + "' AND SSCode='" + stepSequenceCode + "'";
            if (tableGroup != null && tableGroup != string.Empty)
            {
                strSql += " AND TblGrp='" + tableGroup + "' ";
            }
            object[] smtFeeders = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (smtFeeders != null && smtFeeders.Length > 0)
            {
                msg.Add(new Message(MessageType.Error, "$SMTUnLoadFeeder_In_FeederMaterial"));
                return msg;
            }

            UpdateMachineFeederReelQty(mo.MOCode, machineCode, machineStationCode, reelNo, SMTLoadFeederOperationType.UnLoadSingle, userCode);
            this.DeleteMachineFeeder(machineFeeder);

            // ����DataTable
            if (tblLoaded != null)
            {
                for (int i = 0; i < tblLoaded.Rows.Count; i++)
                {
                    if (tblLoaded.Rows[i]["MachineCode"].ToString() == machineCode &&
                        tblLoaded.Rows[i]["MachineStationCode"].ToString() == machineStationCode &&
                        tblLoaded.Rows[i]["MaterialCode"].ToString() == machineFeeder.MaterialCode &&
                        FormatHelper.StringToBoolean(tblLoaded.Rows[i]["CheckResult"].ToString()) == true)
                    {
                        if (tblLoaded.Rows[i]["FeederCode"].ToString() != feederCode)
                        {
                            msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_FeederCode_Not_Exist [" + feederCode + "]"));
                            return msg;
                        }
                        if (tblLoaded.Rows[i]["ReelNo"].ToString() != reelNo)
                        {
                            msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_ReelNo_Not_Exist [" + reelNo + "]"));
                            return msg;
                        }
                        tblLoaded.Rows[i]["FeederCode"] = "";
                        tblLoaded.Rows[i]["FeederLeftCount"] = "";
                        tblLoaded.Rows[i]["ReelNo"] = "";
                        tblLoaded.Rows[i]["ReelLeftQty"] = "";
                        tblLoaded.Rows[i]["LoadUser"] = "";
                        tblLoaded.Rows[i]["LoadDate"] = "";
                        tblLoaded.Rows[i]["LoadTime"] = "";
                        break;
                    }
                }
            }

            return msg;
        }

        /// <summary>
        /// ���ϼ��
        /// </summary>
        public Messages SMTLoadCheck(string moCode, string stepSequenceCode, string machineCode, string stationCode, string reelNo, string feederCode)
        {
            Messages msg = new Messages();
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + stationCode + "' AND CheckResult='1' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs == null || objs.Length == 0)	// û���ҵ����ϼ�¼
            {
                msg.Add(new Message(MessageType.Error, "$SMT_LoadCheck_Error_Input"));
                return msg;
            }
            MachineFeeder mf = (MachineFeeder)objs[0];
            if (mf.FeederCode != feederCode)	// Feeder����
            {
                msg.Add(new Message(MessageType.Error, "$SMT_LoadCheck_Feeder_Error"));
            }
            else
            {
                if (mf.ReelNo != reelNo && mf.NextReelNo != reelNo)		//�Ͼ����
                {
                    msg.Add(new Message(MessageType.Error, "$SMT_LoadCheck_ReelNo_Error"));
                }
                else if (mf.NextReelNo == reelNo)	// Ϊ�����Ͼ�
                {
                    msg.Add(new Message(MessageType.Success, "$SMT_LoadCheck_Is_NextReelNo"));
                }
                else
                {
                    msg.Add(new Message(MessageType.Success, "$SMT_LoadCheck_Pass"));
                }
            }
            return msg;
        }

        /// <summary>
        /// �趨����������Ϣ�Ƿ����
        /// </summary>
        /// <param name="moCode"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Messages SMTEnabledMachineFeeder(string moCode, string stepSequenceCode, bool enabled, string userCode)
        {
            Messages msg = new Messages();
            string strSql = string.Empty;
            // ����Ƿ��������
            strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE CheckResult='1' AND MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) == 0)
            {
                msg.Add(new Message(MessageType.Error, "$SMTEnabledMO_Not_MachineFeeder"));
                return msg;
            }
            if (enabled == true)
            {
                // ���������Ƿ�����������������
                strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE CheckResult='1' AND Enabled='1' AND MOCode<>'" + moCode + "' AND SSCode='" + stepSequenceCode + "' ";
                int iCount = this.DataProvider.GetCount(new SQLCondition(strSql));
                if (iCount > 0)
                {
                    msg.Add(new Message(MessageType.Error, "$SMTEnabledMO_Repeat_MO_In_SameLine"));
                    return msg;
                }
                strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' AND RowNum=1 ";
                object[] objsmf = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
                MachineFeeder mf = (MachineFeeder)objsmf[0];
                // ����Ƿ�ȫ������
                /*
                strSql = "SELECT COUNT(*) FROM tblSMTFeederMaterial smt WHERE NOT EXISTS ";
                strSql += "(SELECT * FROM tblMachineFeeder mf WHERE smt.ProductCode=mf.ProductCode AND smt.MachineCode=mf.MachineCode ";
                strSql += "AND smt.MachineStationCode=mf.MachineStationCode AND smt.SSCode=mf.SSCode AND mf.MOCode='" + moCode + "') ";
                strSql += "AND ProductCode='" + mf.ProductCode + "' AND SSCode='" + mf.StepSequenceCode + "'";
                */
                strSql = "SELECT * FROM tblSMTFeederMaterial smt WHERE NOT EXISTS ";
                strSql += "(SELECT * FROM tblMachineFeeder mf WHERE smt.ProductCode=mf.ProductCode AND smt.MachineCode=mf.MachineCode ";
                strSql += "AND smt.MachineStationCode=mf.MachineStationCode AND smt.SSCode=mf.SSCode AND smt.MaterialCode=mf.MaterialCode ";
                strSql += "AND mf.MOCode='" + moCode + "' AND mf.SSCode='" + mf.StepSequenceCode + "' AND stationenabled = '1') ";
                strSql += "AND (MachineCode,MachineStationCode,TblGrp) NOT IN ";
                strSql += "(SELECT MachineCode,MachineStationCode,TblGrp FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + mf.StepSequenceCode + "' AND stationenabled = '1') ";
                strSql += "AND ProductCode='" + mf.ProductCode + "' AND SSCode='" + mf.StepSequenceCode + "' ";
                string strTmp = "SELECT distinct smt.MachineCode,smt.TblGrp FROM tblSMTFeederMaterial smt ";
                strTmp += "WHERE smt.ProductCode='" + mf.ProductCode + "' AND SSCode='" + mf.StepSequenceCode + "' ";
                strTmp += "AND (MachineCode,TblGrp) NOT IN (SELECT MachineCode,TblGrp FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + mf.StepSequenceCode + "' AND StationEnabled='1') ";
                strTmp += "AND MachineCode IN (SELECT MachineCode FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + mf.StepSequenceCode + "' AND StationEnabled='1') ";
                object[] objsTmp = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strTmp));
                string strWhereExt = string.Empty;
                if (objsTmp != null && objsTmp.Length > 0)
                {
                    for (int i = 0; i < objsTmp.Length; i++)
                    {
                        strWhereExt += "('" + ((SMTFeederMaterial)objsTmp[i]).MachineCode + "','" + ((SMTFeederMaterial)objsTmp[i]).TableGroup + "'),";
                    }
                    strWhereExt = strWhereExt.Substring(0, strWhereExt.Length - 1);
                    strSql += " AND (MachineCode,TblGrp) NOT IN (" + strWhereExt + ") ";
                }
                /*
                if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
                {
                    msg.Add(new Message(MessageType.Error, "$SMTEnabledMO_Absent_MachineFeeder"));
                    return msg;
                }
                */
                object[] objsErr = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
                if (objsErr != null && objsErr.Length > 0)
                {
                    SMTFeederMaterial errItm = (SMTFeederMaterial)objsErr[0];
                    msg.Add(new Message(MessageType.Error, "$SMTEnabledMO_Absent_MachineFeeder [$MachineCode=" + errItm.MachineCode + ",$MachineStationCode=" + errItm.MachineStationCode + "]"));
                    return msg;
                }
            }
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            strSql = "UPDATE tblMachineFeeder SET Enabled='" + FormatHelper.BooleanToString(enabled) + "',MUSER='" + userCode + "',MDATE=" + FormatHelper.TODateInt(dtNow).ToString() + ",MTIME=" + FormatHelper.TOTimeInt(dtNow).ToString() + " WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' AND CheckResult='1' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            // ����INNO @20060822
            if (enabled == true)
            {
                strSql = "DELETE FROM TBLSMTMACHINEACTIVEINNO WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' ";
                this.DataProvider.CustomExecute(new SQLCondition(strSql));

                strSql = "SELECT DISTINCT MOCode,SSCode,MachineCode FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' AND Enabled='1' AND StationEnabled='1' ";
                object[] objsma = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
                if (objsma != null)
                {
                    for (int i = 0; i < objsma.Length; i++)
                    {
                        MachineFeeder mf = (MachineFeeder)objsma[i];
                        this.GenerateMachineInno(mf.MOCode, mf.StepSequenceCode, mf.MachineCode, userCode);
                    }
                }
            }
            else
            {
                /*
                strSql = "DELETE FROM TBLSMTMACHINEACTIVEINNO WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' ";
                this.DataProvider.CustomExecute(new SQLCondition(strSql));
                */
            }
            //
            // ���߿��Ƽ��
            if (enabled == true)
            {
                msg.AddMessages(this.AddStartLineLog(moCode, stepSequenceCode, SMTLoadFeederOperationType.MOEnabled, userCode));
            }
            else
            {
                msg.AddMessages(this.AddStopLineLog(moCode, stepSequenceCode, SMTLoadFeederOperationType.MODisabled, userCode));
            }
            return msg;
        }

        public decimal GetReelAlertQty(Reel reel)
        {
            //return reel.Qty * (decimal)0.9;
            return reel.Qty;
        }
        public decimal GetReelAlertQty(ReelQty reel)
        {
            //return reel.Qty * (decimal)0.9;
            return reel.Qty;
        }

        private void UpdateMachineFeederReelQty(string moCode, string machineCode, string machineStationCode, string reelNo, string operationType, string userCode)
        {
            // ��ѯLog�ж�Ӧ�ļ�¼
            string strSql = "SELECT * FROM tblMachineFeederLog WHERE LogNo IN (SELECT MAX(LogNo) FROM tblMachineFeederLog WHERE MOCode='" + moCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + machineStationCode + "' AND ReelNo='" + reelNo + "' AND CheckResult='1')";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeederLog), new SQLCondition(strSql));
            if (objs == null)
                return;
            MachineFeederLog log = (MachineFeederLog)objs[0];
            strSql = "SELECT * FROM tblReelQty WHERE ReelNo='" + reelNo + "' AND MOCode='" + moCode + "'";
            object[] reelQtys = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
            // �����Ѻ�������
            decimal d = 0;
            if (reelQtys != null)
            {
                for (int i = 0; i < reelQtys.Length; i++)
                {
                    ReelQty reelQty = (ReelQty)reelQtys[i];
                    d += reelQty.UsedQty;
                }
                if (d > 0)
                {
                    // �������������µ��Ͼ�����
                    strSql = "UPDATE tblReel SET UsedQty=UsedQty+" + d.ToString() + " WHERE ReelNo='" + reelNo + "'";
                    this.DataProvider.CustomExecute(new SQLCondition(strSql));
                }
            }
            // ɾ���Ͼ������м��
            strSql = "DELETE FROM tblReelQty WHERE ReelNo='" + reelNo + "' AND MOCode='" + moCode + "'";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            // ����Log
            log.UnLoadUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            log.UnLoadDate = FormatHelper.TODateInt(dtNow);
            log.UnLoadTime = FormatHelper.TOTimeInt(dtNow);
            log.UnLoadType = operationType;
            log.ReelUsedQty += d;
            log.MaintainUser = userCode;
            log.MaintainDate = FormatHelper.TODateInt(dtNow);
            log.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.UpdateMachineFeederLog(log);
        }

        public Messages AddMachineFeederPass(MO mo, Feeder feeder, Reel reel, SMTFeederMaterial smtFeeder, string userCode, DataTable tblLoaded, string operationType, string resourceCode, string stepSequenceCode, string oldFeederCode, string oldReelNo)
        {
            return AddMachineFeederPass(mo, feeder, reel, smtFeeder, userCode, tblLoaded, operationType, resourceCode, stepSequenceCode, oldFeederCode, oldReelNo, null);
        }
        public Messages AddMachineFeederPass(MO mo, Feeder feeder, Reel reel, SMTFeederMaterial smtFeeder, string userCode, DataTable tblLoaded, string operationType, string resourceCode, string stepSequenceCode, string oldFeederCode, string oldReelNo, object stationEnabled)
        {
            Messages msg = new Messages();

            if (feeder.FeederCode != oldFeederCode)
            {
                feeder.CurrentUnitQty = smtFeeder.Qty;
                this.UpdateFeeder(feeder);
            }

            bool bIsNew = true;
            MachineFeeder machineFeeder = new MachineFeeder();
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND MachineCode='" + smtFeeder.MachineCode + "' AND MachineStationCode='" + smtFeeder.MachineStationCode + "' AND TblGrp='" + smtFeeder.TableGroup + "' AND SSCode='" + stepSequenceCode + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                machineFeeder = (MachineFeeder)objs[0];
                bIsNew = false;
            }
            if (machineFeeder.ProductCode != string.Empty && operationType == SMTLoadFeederOperationType.Continue)
            {
                if (machineFeeder.NextReelNo != string.Empty)
                {
                    msg.Add(new Message(MessageType.Error, "$SMT_LoadFeeder_Continue_Exist_NextReel"));
                    return msg;
                }
                machineFeeder.NextReelNo = reel.ReelNo;
            }
            else
            {
                // ֻ��Feeder�����
                if (operationType == SMTLoadFeederOperationType.Exchange &&
                    (machineFeeder.ReelNo == reel.ReelNo || machineFeeder.NextReelNo == reel.ReelNo) &&
                    machineFeeder.FeederCode != feeder.FeederCode)
                { }
                else
                {
                    // �������Ͼ�����
                    if (oldReelNo != string.Empty && machineFeeder.NextReelNo == oldReelNo)
                    {
                        machineFeeder.NextReelNo = reel.ReelNo;
                    }
                    else
                    {
                        machineFeeder.ReelNo = reel.ReelNo;
                        machineFeeder.MaterialCode = reel.PartNo;
                        machineFeeder.UnitQty = smtFeeder.Qty;
                        machineFeeder.LotNo = reel.LotNo;
                        machineFeeder.DateCode = reel.DateCode;
                    }
                }
            }
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            machineFeeder.ProductCode = mo.ItemCode;
            machineFeeder.StepSequenceCode = stepSequenceCode;
            machineFeeder.MOCode = mo.MOCode;
            machineFeeder.MachineCode = smtFeeder.MachineCode;
            machineFeeder.MachineStationCode = smtFeeder.MachineStationCode;
            machineFeeder.FeederSpecCode = feeder.FeederSpecCode;
            machineFeeder.FeederCode = feeder.FeederCode;
            machineFeeder.LoadUser = userCode;
            machineFeeder.LoadDate = FormatHelper.TODateInt(dtNow);
            machineFeeder.LoadTime = FormatHelper.TOTimeInt(dtNow);
            machineFeeder.CheckResult = FormatHelper.BooleanToString(true);
            machineFeeder.OpeResourceCode = resourceCode;
            machineFeeder.OpeStepSequenceCode = stepSequenceCode;
            machineFeeder.TableGroup = smtFeeder.TableGroup;
            machineFeeder.MaintainUser = userCode;

            machineFeeder.MaintainDate = FormatHelper.TODateInt(dtNow);
            machineFeeder.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            // ���������������������Enabled״̬������ӵ�����Ҳ��Enabled
            int iEnabled = this.DataProvider.GetCount(new SQLCondition("SELECT COUNT(*) FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND SSCode='" + stepSequenceCode + "' AND CheckResult='1' AND Enabled='1'"));
            if (iEnabled > 0)
                machineFeeder.Enabled = FormatHelper.BooleanToString(true);
            else
                machineFeeder.Enabled = FormatHelper.BooleanToString(false);
            if (stationEnabled != null)
            {
                machineFeeder.StationEnabled = FormatHelper.BooleanToString(Convert.ToBoolean(stationEnabled));
            }
            if (bIsNew == false)
            {
                // ���߿��Ƽ��
                if (operationType == SMTLoadFeederOperationType.Continue ||
                    operationType == SMTLoadFeederOperationType.Exchange)
                {
                    if (FormatHelper.StringToBoolean(machineFeeder.ReelCeaseFlag) == true)
                    {
                        msg.AddMessages(this.AddStartLineLog(machineFeeder, operationType, userCode));
                        if (msg.GetData() != null && msg.GetData().Values != null && msg.GetData().Values.Length > 0)
                        {
                            SMTLineControlLog lineLog = (SMTLineControlLog)msg.GetData().Values[0];
                            if (FormatHelper.StringToBoolean(lineLog.LineStatus) == true)
                            {
                                machineFeeder.ReelCeaseFlag = FormatHelper.BooleanToString(false);
                            }
                        }
                    }
                }
                this.UpdateMachineFeeder(machineFeeder);
            }
            else
            {
                machineFeeder.ReelCeaseFlag = FormatHelper.BooleanToString(false);
                this.AddMachineFeeder(machineFeeder);
            }
            this.AddMachineFeederLog(machineFeeder, operationType, resourceCode, stepSequenceCode, feeder.FeederCode, reel.ReelNo, oldFeederCode, oldReelNo, reel);

            ReelQty reelQty = new ReelQty();
            reelQty.MOCode = mo.MOCode;
            reelQty.MachineCode = machineFeeder.MachineCode;
            reelQty.MachineStationCode = machineFeeder.MachineStationCode;
            reelQty.FeederCode = feeder.FeederCode;
            reelQty.FeederSpecCode = feeder.FeederSpecCode;
            reelQty.ReelNo = reel.ReelNo;
            reelQty.Qty = reel.Qty;
            reelQty.UsedQty = 0;
            reelQty.UpdatedQty = reel.UsedQty;
            reelQty.UnitQty = machineFeeder.UnitQty;
            reelQty.AlertQty = reelQty.Qty;
            reelQty.MaterialCode = reel.PartNo;
            reelQty.StepSequenceCode = stepSequenceCode;
            reelQty.MaintainUser = userCode;
            reelQty.MaintainDate = FormatHelper.TODateInt(dtNow);
            reelQty.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.AddReelQty(reelQty);

            if (tblLoaded != null)
            {
                int tableRowIdx = -1;
                if (tblLoaded != null)
                {
                    for (int i = 0; i < tblLoaded.Rows.Count; i++)
                    {
                        if (tblLoaded.Rows[i]["MachineCode"].ToString() == smtFeeder.MachineCode &&
                            tblLoaded.Rows[i]["MachineStationCode"].ToString() == smtFeeder.MachineStationCode &&
                            tblLoaded.Rows[i]["MaterialCode"].ToString() == smtFeeder.MaterialCode)
                        {
                            tableRowIdx = i;
                            break;
                        }
                    }
                }
                DataRow row = tblLoaded.NewRow();
                if (tableRowIdx >= 0)
                {
                    row = tblLoaded.Rows[tableRowIdx];
                }
                row["MachineCode"] = machineFeeder.MachineCode;
                row["MachineStationCode"] = machineFeeder.MachineStationCode;
                row["FeederCode"] = machineFeeder.FeederCode;
                row["FeederSpecCode"] = machineFeeder.FeederSpecCode;
                row["FeederLeftCount"] = feeder.MaxCount - feeder.UsedCount;
                if (machineFeeder.NextReelNo != null && machineFeeder.NextReelNo != string.Empty)
                    row["ReelNo"] = machineFeeder.NextReelNo;
                else
                    row["ReelNo"] = machineFeeder.ReelNo;
                row["ReelLeftQty"] = reel.Qty - reel.UsedQty;
                row["MaterialCode"] = machineFeeder.MaterialCode;
                row["LoadUser"] = machineFeeder.LoadUser;
                row["LoadDate"] = FormatHelper.ToDateString(machineFeeder.LoadDate);
                row["LoadTime"] = FormatHelper.ToTimeString(machineFeeder.LoadTime);
                row["CheckResult"] = FormatHelper.BooleanToString(true);
                row["FailReason"] = "";
                if (tableRowIdx < 0)
                {
                    tblLoaded.Rows.Add(row);
                }
            }
            return msg;
        }

        private void AddMachineFeederFailure(MO mo, string machineCode, string stationCode, string reelNo, string feederCode, string errorMessage, string userCode, DataTable tblLoaded, string operationType, string resourceCode, string stepSequenceCode)
        {
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            MachineFeeder machineFeeder = new MachineFeeder();
            machineFeeder.ProductCode = mo.ItemCode;
            machineFeeder.StepSequenceCode = stepSequenceCode;
            machineFeeder.MOCode = mo.MOCode;
            machineFeeder.MachineCode = machineCode;
            machineFeeder.MachineStationCode = stationCode;
            machineFeeder.FeederSpecCode = string.Empty;
            machineFeeder.FeederCode = feederCode;
            machineFeeder.ReelNo = reelNo;
            machineFeeder.LoadUser = userCode;

            machineFeeder.LoadDate = FormatHelper.TODateInt(dtNow);
            machineFeeder.LoadTime = FormatHelper.TOTimeInt(dtNow);
            machineFeeder.MaterialCode = string.Empty;
            machineFeeder.UnitQty = 0;
            machineFeeder.LotNo = string.Empty;
            machineFeeder.DateCode = string.Empty;
            machineFeeder.CheckResult = FormatHelper.BooleanToString(false);
            machineFeeder.FailReason = errorMessage;
            machineFeeder.OpeResourceCode = resourceCode;
            machineFeeder.OpeStepSequenceCode = stepSequenceCode;
            machineFeeder.MaintainUser = userCode;
            machineFeeder.MaintainDate = FormatHelper.TODateInt(dtNow);
            machineFeeder.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.AddMachineFeederLog(machineFeeder, operationType, resourceCode, stepSequenceCode, feederCode, reelNo, string.Empty, string.Empty, null);

            #region Removed
            /*
			bool bIsNew = true;
			int tableRowIdx = -1;
			if (tblLoaded != null)
			{
				for (int i = 0; i < tblLoaded.Rows.Count; i++)
				{
					if (tblLoaded.Rows[i]["MachineCode"].ToString() == machineCode && 
						tblLoaded.Rows[i]["MachineStationCode"].ToString() == stationCode)
					{
						if (FormatHelper.StringToBoolean(tblLoaded.Rows[i]["CheckResult"].ToString()) == true)
						{
							throw new Exception("$MachineStation_Duplation");
						}
						bIsNew = false;
						tableRowIdx = i;
						break;
					}
				}
			}
			MachineFeeder machineFeeder = new MachineFeeder();
			if (bIsNew == false)
			{
				string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + stationCode + "'";
				object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
				if (objs != null && objs.Length > 0)
					machineFeeder = (MachineFeeder)objs[0];
			}
			machineFeeder.ProductCode = mo.ItemCode;
			machineFeeder.MOCode = mo.MOCode;
			machineFeeder.MachineCode = machineCode;
			machineFeeder.MachineStationCode = stationCode;
			machineFeeder.FeederSpecCode = string.Empty;
			machineFeeder.FeederCode = feederCode;
			machineFeeder.ReelNo = reelNo;
			machineFeeder.LoadUser = userCode;
			machineFeeder.LoadDate = FormatHelper.TODateInt(DateTime.Today);
			machineFeeder.LoadTime = FormatHelper.TOTimeInt(DateTime.Now);
			machineFeeder.MaterialCode = string.Empty;
			machineFeeder.UnitQty = 0;
			machineFeeder.LotNo = string.Empty;
			machineFeeder.DateCode = string.Empty;
			machineFeeder.CheckResult = FormatHelper.BooleanToString(false);
			machineFeeder.FailReason = errorMessage;
			machineFeeder.ResourceCode = resourceCode;
			machineFeeder.StepSequenceCode = stepSequenceCode;
			machineFeeder.MaintainUser = userCode;
			machineFeeder.MaintainDate = FormatHelper.TODateInt(DateTime.Today);
			machineFeeder.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
			if (bIsNew == false)
				this.UpdateMachineFeeder(machineFeeder);
			else
				this.AddMachineFeeder(machineFeeder);
			this.AddMachineFeederLog(machineFeeder, operationType, resourceCode, stepSequenceCode);

			if (tblLoaded != null)
			{
				DataRow row = tblLoaded.NewRow();
				if (bIsNew == false)
				{
					row = tblLoaded.Rows[tableRowIdx];
				}
				row["MachineCode"] = machineFeeder.MachineCode;
				row["MachineStationCode"] = machineFeeder.MachineStationCode;
				row["FeederCode"] = machineFeeder.FeederCode;
				row["FeederSpecCode"] = machineFeeder.FeederSpecCode;
				row["FeederLeftCount"] = string.Empty;
				row["ReelNo"] = machineFeeder.ReelNo;
				row["ReelLeftQty"] = string.Empty;
				row["MaterialCode"] = machineFeeder.MaterialCode;
				row["LoadUser"] = machineFeeder.LoadUser;
				row["LoadDate"] = FormatHelper.ToDateString(machineFeeder.LoadDate);
				row["LoadTime"] = FormatHelper.ToTimeString(machineFeeder.LoadTime);
				row["CheckResult"] = FormatHelper.BooleanToString(false);
				row["FailReason"] = errorMessage;
				if (bIsNew == true)
				{
					tblLoaded.Rows.Add(row);
				}
			}
			*/
            #endregion
        }

        private void AddMachineFeederLog(MachineFeeder machineFeeder, string operationType, string resourceCode, string stepSequenceCode, string newFeederCode, string newReelNo, string oldFeederCode, string oldReelNo, Reel currentReel)
        {
            /*	Removed by Icyer 2007/01/15		�ĳɴ�Sequence��ȡ��ˮ��
            string strSql = "SELECT MAX(LogNo) LogNo FROM tblMachineFeederLog";
            object[] objSeq = this.DataProvider.CustomQuery(typeof(MachineFeederLog), new SQLCondition(strSql));
            decimal iSeq = 1;
            if (objSeq != null && objSeq.Length > 0)
            {
                iSeq = ((MachineFeederLog)objSeq[0]).LogNo + 1;
            }
            */
            string strSql = "SELECT SEQ_SMTLOGSEQUENCE.NextVal LogNo FROM dual";
            object[] objSeq = this.DataProvider.CustomQuery(typeof(MachineFeederLog), new SQLCondition(strSql));
            decimal iSeq = 1;
            if (objSeq != null && objSeq.Length > 0)
            {
                iSeq = ((MachineFeederLog)objSeq[0]).LogNo;
            }

            MachineFeederLog log = new MachineFeederLog();
            log.LogNo = iSeq;
            log.ProductCode = machineFeeder.ProductCode;
            log.StepSequenceCode = machineFeeder.StepSequenceCode;
            log.MOCode = machineFeeder.MOCode;
            log.MachineCode = machineFeeder.MachineCode;
            log.MachineStationCode = machineFeeder.MachineStationCode;
            log.FeederCode = newFeederCode;
            log.FeederSpecCode = machineFeeder.FeederSpecCode;
            log.ReelNo = newReelNo;
            log.LoadUser = machineFeeder.LoadUser;
            log.LoadDate = machineFeeder.LoadDate;
            log.LoadTime = machineFeeder.LoadTime;
            log.MaterialCode = machineFeeder.MaterialCode;
            log.UnitQty = machineFeeder.UnitQty;
            log.LotNo = machineFeeder.LotNo;
            log.DateCode = machineFeeder.DateCode;
            if (currentReel != null)
            {
                log.MaterialCode = currentReel.PartNo;
                log.LotNo = currentReel.LotNo;
                log.DateCode = currentReel.DateCode;
            }
            log.CheckResult = machineFeeder.CheckResult;
            log.FailReason = machineFeeder.FailReason;
            log.OperationType = operationType;
            log.ReelUsedQty = 0;
            log.OpeResourceCode = resourceCode;
            log.OpeStepSequenceCode = stepSequenceCode;
            log.ExchangeFeederCode = oldFeederCode;
            log.ExchageReelNo = oldReelNo;
            log.StationEnabled = machineFeeder.StationEnabled;
            log.TableGroup = machineFeeder.TableGroup;
            log.MaintainUser = machineFeeder.MaintainUser;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            log.MaintainDate = FormatHelper.TODateInt(dtNow);
            log.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            // Added by Icyer 2007/07/02
            BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(log.MOCode);
            log.MOSeq = mo.MOSeq;
            // Added end
            this.AddMachineFeederLog(log);
        }

        /// <summary>
        /// ���ϱȶ�
        /// </summary>
        /// <param name="moCode"></param>
        /// <returns></returns>
        public SMTCheckMaterialDetail[] CheckSMTMaterial(string moCode, string sscode, string userCode, bool autoUpdate)
        {
            // ��ѯ����
            BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(moCode);
            if (mo == null)
                throw new Exception("$CS_MO_Not_Exist");
            // ��ѯ����BOM
            object[] objsbom = moFacade.GetMOBOM(moCode);
            if (objsbom == null)
                objsbom = new MOBOM[0];
            // ��ѯ��վ��
            object[] objssmt = this.GetSMTFeederMaterial(mo.ItemCode, sscode);
            if (objssmt == null || objssmt.Length == 0)
                return null;
            MOBOM[] moboms = new MOBOM[objsbom.Length];
            objsbom.CopyTo(moboms, 0);
            Hashtable htBom = new Hashtable();
            for (int i = 0; i < moboms.Length; i++)
            {
                if (htBom.ContainsKey(moboms[i].MOBOMItemCode) == false)
                    htBom.Add(moboms[i].MOBOMItemCode, moboms[i]);
                else
                {
                    MOBOM mobom = (MOBOM)htBom[moboms[i].MOBOMItemCode];
                    mobom.MOBOMItemQty += moboms[i].MOBOMItemQty;
                }
            }
            // �ȶ���վ���������ڹ���BOM���Ƿ����
            ArrayList listResult = new ArrayList();
            ArrayList listSmtMaterial = new ArrayList();
            Hashtable htMaterialQty = new Hashtable();
            bool bExistError = false;
            for (int i = 0; i < objssmt.Length; i++)
            {
                SMTFeederMaterial smtMaterial = (SMTFeederMaterial)objssmt[i];
                if (htMaterialQty.ContainsKey(smtMaterial.MaterialCode) == false)
                {
                    htMaterialQty.Add(smtMaterial.MaterialCode, smtMaterial.Qty.ToString());
                }
                else
                {
                    string strValue = htMaterialQty[smtMaterial.MaterialCode].ToString();
                    strValue = (decimal.Parse(strValue.ToString()) + smtMaterial.Qty).ToString();
                    htMaterialQty[smtMaterial.MaterialCode] = strValue;
                }
            }
            for (int i = 0; i < objssmt.Length; i++)
            {
                SMTFeederMaterial smtMaterial = (SMTFeederMaterial)objssmt[i];
                SMTCheckMaterialDetail dtl = new SMTCheckMaterialDetail();
                dtl.Type = SMTCheckMaterialDetailType.SMT;
                dtl.MachineCode = smtMaterial.MachineCode;
                dtl.MachineStationCode = smtMaterial.MachineStationCode;
                dtl.ProductCode = smtMaterial.ProductCode;
                dtl.StepSequenceCode = smtMaterial.StepSequenceCode;
                dtl.MaterialCode = smtMaterial.MaterialCode;
                dtl.SourceMaterialCode = smtMaterial.SourceMaterialCode;
                dtl.FeederSpecCode = smtMaterial.FeederSpecCode;
                dtl.SMTQty = smtMaterial.Qty;
                dtl.EAttribute1 = smtMaterial.TableGroup;

                if (htBom.ContainsKey(dtl.MaterialCode) == false)	// ��վ�����ϲ��ڹ���BOM��
                {
                    dtl.CheckResult = FormatHelper.BooleanToString(false);
                    dtl.CheckDescription = SMTCheckMaterialResultDescription.MOBOM_Exception;
                    bExistError = true;
                }
                else
                {
                    MOBOM mobom = (MOBOM)htBom[dtl.MaterialCode];
                    dtl.MaterialName = mobom.MOBOMItemDescription;
                    dtl.BOMQty = mobom.MOBOMItemQty;
                    dtl.BOMUOM = mobom.MOBOMItemUOM;
                    string strValue = htMaterialQty[dtl.MaterialCode].ToString();
                    decimal dSumQty = decimal.Parse(strValue);
                    if (dtl.BOMQty > 0 && dSumQty == dtl.BOMQty)
                    {
                        dtl.CheckResult = FormatHelper.BooleanToString(true);
                        dtl.CheckDescription = SMTCheckMaterialResultDescription.Matchable;
                        if (strValue.IndexOf(",") >= 0)
                        {
                            string[] strIdxList = strValue.Split(':')[1].Split(',');
                            foreach (string strIdx in strIdxList)
                            {
                                if (strIdx != i.ToString())
                                {
                                    SMTCheckMaterialDetail dtlOld = (SMTCheckMaterialDetail)listResult[int.Parse(strIdx)];
                                    dtlOld.CheckResult = FormatHelper.BooleanToString(true);
                                    dtlOld.CheckDescription = SMTCheckMaterialResultDescription.Matchable;
                                }
                            }
                        }
                    }
                    else		// ��λ��������ͬ
                    {
                        dtl.CheckResult = FormatHelper.BooleanToString(false);
                        dtl.CheckDescription = SMTCheckMaterialResultDescription.Qty_Exception;
                        bExistError = true;
                        if (strValue.IndexOf(",") >= 0)
                        {
                            string[] strIdxList = strValue.Split(':')[1].Split(',');
                            foreach (string strIdx in strIdxList)
                            {
                                if (strIdx != i.ToString())
                                {
                                    SMTCheckMaterialDetail dtlOld = (SMTCheckMaterialDetail)listResult[int.Parse(strIdx)];
                                    dtlOld.CheckResult = FormatHelper.BooleanToString(false);
                                    dtlOld.CheckDescription = SMTCheckMaterialResultDescription.Qty_Exception;
                                }
                            }
                        }
                    }
                }
                listResult.Add(dtl);
                if (listSmtMaterial.Contains(dtl.MaterialCode) == false)
                    listSmtMaterial.Add(dtl.MaterialCode);
            }
            // ��ѯֻ�ڹ���BOM�д��ڵ�����
            for (int i = 0; i < moboms.Length; i++)
            {
                if (listSmtMaterial.Contains(moboms[i].MOBOMItemCode) == false)
                {
                    SMTCheckMaterialDetail dtl = new SMTCheckMaterialDetail();
                    dtl.Type = SMTCheckMaterialDetailType.MOBOM;
                    dtl.ProductCode = moboms[i].ItemCode;
                    dtl.MaterialCode = moboms[i].MOBOMItemCode;
                    dtl.MaterialName = moboms[i].MOBOMItemDescription;
                    dtl.BOMQty = moboms[i].MOBOMItemQty;
                    dtl.BOMUOM = moboms[i].MOBOMItemUOM;
                    dtl.CheckResult = FormatHelper.BooleanToString(false);
                    listResult.Add(dtl);
                    bExistError = true;
                }
            }
            SMTCheckMaterialDetail[] dtlList = new SMTCheckMaterialDetail[listResult.Count];
            listResult.CopyTo(dtlList);
            // ��������ڲ��죬���Զ�����
            if (bExistError == false && autoUpdate == true)
            {
                CheckSMTMaterialUpdate(moCode, dtlList, SMTCheckMaterialResult.Matchable, userCode);
            }
            return dtlList;
        }
        public void CheckSMTMaterialConfirm(string moCode, string sscode, string userCode)
        {
            SMTCheckMaterialDetail[] details = this.CheckSMTMaterial(moCode, sscode, userCode, false);
            if (details == null)
                return;
            CheckSMTMaterialUpdate(moCode, details, SMTCheckMaterialResult.Accept_Exception, userCode);
        }
        private void CheckSMTMaterialUpdate(string moCode, SMTCheckMaterialDetail[] details, string checkResult, string userCode)
        {
            string strSql = "SELECT * FROM tblSMTCheckMaterial WHERE CheckID=(SELECT MAX(CheckID) FROM tblSMTCheckMaterial)";
            decimal iCheckID = 1;
            object[] objs = this.DataProvider.CustomQuery(typeof(SMTCheckMaterial), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                iCheckID = ((SMTCheckMaterial)objs[0]).CheckID + 1;
            }
            SMTCheckMaterial chk = new SMTCheckMaterial();
            chk.CheckID = iCheckID;
            chk.MOCode = moCode;
            chk.ItemCode = details[0].ProductCode;
            chk.StepSequenceCode = details[0].StepSequenceCode;
            chk.CheckResult = checkResult;
            chk.CheckUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            chk.CheckDate = FormatHelper.TODateInt(dtNow);
            chk.CheckTime = FormatHelper.TOTimeInt(dtNow);
            chk.MaintainUser = userCode;
            chk.MaintainDate = chk.CheckDate;
            chk.MaintainTime = chk.CheckTime;
            this.AddSMTCheckMaterial(chk);
            for (int i = 0; i < details.Length; i++)
            {
                details[i].CheckID = iCheckID;
                details[i].Sequence = i + 1;
                details[i].MaintainUser = userCode;
                details[i].MaintainDate = FormatHelper.TODateInt(dtNow);
                details[i].MaintainTime = FormatHelper.TOTimeInt(dtNow);
                this.AddSMTCheckMaterialDetail(details[i]);
            }
        }

        /// <summary>
        /// ת�������в�ѯ�������ϵ�����
        /// </summary>
        /// <param name="moCode"></param>
        /// <returns></returns>
        public MachineFeeder[] GetLoadedMaterialByMO(string moCode, string ssCode)
        {
            // ��ѯ������������Ϣ
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + ssCode + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs == null || objs.Length == 0)
                return null;
            MachineFeeder[] machineFeeder = new MachineFeeder[objs.Length];
            objs.CopyTo(machineFeeder, 0);
            // ��ѯ������Feeder����
            strSql = "SELECT * FROM tblFeeder WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + ssCode + "' )";
            objs = this.DataProvider.CustomQuery(typeof(Feeder), new SQLCondition(strSql));
            Hashtable htFeeder = new Hashtable();
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    htFeeder.Add(((Feeder)objs[i]).FeederCode, objs[i]);
                }
            }
            // ��ѯ�������Ͼ�
            strSql = "SELECT * FROM tblReelQty WHERE MOCode='" + moCode + "' AND SSCode='" + ssCode + "' ";
            objs = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
            Hashtable htReel = new Hashtable();
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    htReel.Add(((ReelQty)objs[i]).ReelNo, objs[i]);
                }
            }
            // �������ϼ�¼��Feederʣ��������Ͼ�ʣ������
            for (int i = 0; i < machineFeeder.Length; i++)
            {
                Feeder feeder = (Feeder)htFeeder[machineFeeder[i].FeederCode];
                string feederLeftCount = string.Empty;
                if (feeder != null)
                    feederLeftCount = (feeder.MaxCount - feeder.UsedCount).ToString();
                string reelLeftQty = string.Empty;
                ReelQty reelQty = (ReelQty)htReel[machineFeeder[i].ReelNo];
                if (reelQty != null)
                    reelLeftQty = (reelQty.Qty - reelQty.UpdatedQty - reelQty.UsedQty).ToString();
                machineFeeder[i].EAttribute1 = feederLeftCount + ":" + reelLeftQty;
            }
            return machineFeeder;
        }

        public Messages TransferMaterialByMO(string moCodeFrom, string ssCodeFrom, string moCodeTo, string sscode)
        {
            Messages msg = new Messages();
            // ����Ƿ������������ϱȶ�
            string strSql = "SELECT * FROM tblSMTCheckMaterial WHERE CheckID=(SELECT MAX(CheckID) FROM tblSMTCheckMaterial WHERE MOCode='" + moCodeTo + "' AND SSCode='" + sscode + "')";
            object[] objschk = this.DataProvider.CustomQuery(typeof(SMTCheckMaterial), new SQLCondition(strSql));
            bool bPass = false;
            if (objschk != null && objschk.Length > 0)
            {
                SMTCheckMaterial chk = (SMTCheckMaterial)objschk[0];
                if (chk.CheckResult == SMTCheckMaterialResult.Accept_Exception ||
                    chk.CheckResult == SMTCheckMaterialResult.Matchable)
                {
                    bPass = true;
                }
            }
            if (bPass == false)
            {
                msg.Add(new Message(MessageType.Error, "$MOCode_Not_Check_SMTMaterial_MOBOM"));
                return msg;
            }
            // ��ѯ������������Ϣ
            strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCodeFrom + "' AND SSCode='" + ssCodeFrom + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs == null || objs.Length == 0)
            {
                msg.Add(new Message(MessageType.Error, "$MachineFeeder_Not_Exist"));
                return msg;
            }
            MachineFeeder[] machineFeeder = new MachineFeeder[objs.Length];
            objs.CopyTo(machineFeeder, 0);
            /*
            if (machineFeeder[0].StepSequenceCode != sscode)
            {
                msg.Add(new Message(MessageType.Error, "$SourceMO_StepSequenceCode_Is_Not_Current_LoginInfo"));
                return msg;
            }
            */
            // ��ѯ������Feeder����
            strSql = "SELECT * FROM tblFeeder WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE MOCode='" + moCodeFrom + "' AND SSCode='" + ssCodeFrom + "' )";
            objs = this.DataProvider.CustomQuery(typeof(Feeder), new SQLCondition(strSql));
            Hashtable htFeeder = new Hashtable();
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    htFeeder.Add(((Feeder)objs[i]).FeederCode, objs[i]);
                }
            }
            // ��ѯ�������Ͼ�
            strSql = "SELECT tblReelQty.ReelNo,tblReelQty.MOCode,tblReelQty.MachineCode,tblReelQty.MachineStationCode,tblReelQty.FeederSpecCode,tblReelQty.FeederCode,tblReelQty.Qty,tblReelQty.UsedQty,tblReelQty.UpdatedQty,tblReelQty.AlertQty,tblReelQty.UnitQty,";
            strSql += "tblMachineFeeder.MaterialCode EAttribute1 ";
            strSql += "FROM tblReelQty,tblMachineFeeder WHERE tblReelQty.ReelNo=tblMachineFeeder.ReelNo ";
            strSql += "AND tblMachineFeeder.MOCode='" + moCodeFrom + "' AND tblMachineFeeder.SSCode='" + ssCodeFrom + "' AND tblReelQty.MOCode='" + moCodeFrom + "' AND tblReelQty.SSCode='" + ssCodeFrom + "' ";
            objs = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
            Hashtable htReel = new Hashtable();
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    htReel.Add(((ReelQty)objs[i]).ReelNo, objs[i]);
                }
            }
            // ��ѯ�¹���
            BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(moCodeTo);
            if (mo == null)
            {
                msg.Add(new Message(MessageType.Error, "$CS_MO_Not_Exist"));
                return msg;
            }
            // �Ƚϲ�Ʒ�Ƿ�һ��
            if (mo.ItemCode != machineFeeder[0].ProductCode)
            {
                msg.Add(new Message(MessageType.Error, "$CS_ItemCode_Not_Compare"));
                return msg;
            }
            // ��ѯ�¹�����վ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND SSCode='" + sscode + "' ";
            objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            Hashtable htSmtFeeder = new Hashtable();
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    SMTFeederMaterial feederMaterial = (SMTFeederMaterial)objs[i];
                    string strKey = feederMaterial.MachineCode + ":" + feederMaterial.MachineStationCode;
                    if (htSmtFeeder.ContainsKey(strKey) == false)
                        htSmtFeeder.Add(strKey, feederMaterial);
                }
            }
            // �������ϼ�¼��Feederʣ��������Ͼ�ʣ������
            for (int i = 0; i < machineFeeder.Length; i++)
            {
                machineFeeder[i].FailReason = string.Empty;
                // ��ѯ��վ���Ƿ����
                SMTFeederMaterial feederMaterial = (SMTFeederMaterial)htSmtFeeder[machineFeeder[i].MachineCode + ":" + machineFeeder[i].MachineStationCode];
                if (feederMaterial == null)
                {
                    machineFeeder[i].FailReason = "$SMTFeederMaterial_Not_Exist";
                    continue;
                }
                Feeder feeder = (Feeder)htFeeder[machineFeeder[i].FeederCode];
                if (feeder == null)
                {
                    machineFeeder[i].FailReason = "$Feeder_Not_Exist";
                    continue;
                }
                ReelQty reelQty = (ReelQty)htReel[machineFeeder[i].ReelNo];
                if (reelQty == null)
                {
                    machineFeeder[i].FailReason = "$Reel_Not_Exist";
                    continue;
                }
                machineFeeder[i].EAttribute1 = (feeder.MaxCount - feeder.UsedCount).ToString() + ":" + (reelQty.Qty - reelQty.UpdatedQty - reelQty.UsedQty).ToString();
                // ��վ���Ӧ��Feeder��������Ƿ�һ��
                if (feederMaterial.FeederSpecCode != feeder.FeederSpecCode ||
                    feederMaterial.MaterialCode != reelQty.EAttribute1)
                {
                    machineFeeder[i].FailReason = "$SMTFeederMaterial_Not_Match";
                    continue;
                }
                // ���Feeder
                if (feeder.Status != FeederStatus.Normal)
                {
                    machineFeeder[i].FailReason = "$Feeder_Status_Error";
                    continue;
                }
                if (feeder.MaxCount - feeder.UsedCount < feederMaterial.Qty)
                {
                    machineFeeder[i].FailReason = "$FeedreLeftQty_Not_Enough";
                    continue;
                }
                // ����Ͼ�
                if (reelQty.Qty - reelQty.UpdatedQty - reelQty.UsedQty < feederMaterial.Qty)
                {
                    machineFeeder[i].FailReason = "$ReelLeftQty_Not_Enough";
                    continue;
                }
            }
            msg.Add(new Message(MessageType.Data, string.Empty, machineFeeder));
            return msg;
        }

        public Messages TransferMaterialByMOUpdate(MachineFeeder[] machineFeeder, string moCodeFrom, string ssCodeFrom, string moCodeTo, string userCode, string resourceCode, string stepSequenceCode)
        {
            Messages msg = new Messages();
            // ��ѯ����
            BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(moCodeTo);
            if (mo == null)
            {
                msg.Add(new Message(MessageType.Error, "$CS_MO_Not_Exist"));
                return msg;
            }
            string strSql = string.Empty;
            Hashtable htFeeder = new Hashtable();
            // ��ѯ������Feeder����
            strSql = "SELECT * FROM tblFeeder WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE MOCode='" + moCodeFrom + "' AND SSCode='" + ssCodeFrom + "' )";
            object[] objs = this.DataProvider.CustomQuery(typeof(Feeder), new SQLCondition(strSql));
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    htFeeder.Add(((Feeder)objs[i]).FeederCode, objs[i]);
                }
            }
            /*
            Hashtable htReel = new Hashtable();
            // ��ѯ�������Ͼ�����
            strSql = "SELECT * FROM tblReel WHERE ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE MOCode='" + moCodeFrom + "')";
            objs = this.DataProvider.CustomQuery(typeof(Reel), new SQLCondition(strSql));
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    htReel.Add(((Reel)objs[i]).ReelNo, objs[i]);
                }
            }
            */
            Hashtable htSmtFeeder = new Hashtable();
            // ��ѯ��վ��ֻ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' and SSCode='" + stepSequenceCode + "' ";
            objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    SMTFeederMaterial feederMaterial = (SMTFeederMaterial)objs[i];
                    string strKey = feederMaterial.MachineCode + ":" + feederMaterial.MachineStationCode;
                    if (htSmtFeeder.ContainsKey(strKey) == false)
                    {
                        htSmtFeeder.Add(strKey, objs[i]);
                    }
                }
            }
            // ͣ��
            msg.AddMessages(this.AddStopLineLog(moCodeFrom, ssCodeFrom, SMTLoadFeederOperationType.TransferMO, userCode));
            // ִ�й���ת��
            for (int i = 0; i < machineFeeder.Length; i++)
            {
                // �쳣���ݲ�����
                if (machineFeeder[i].FailReason != string.Empty)
                {
                    continue;
                }

                Feeder feeder = (Feeder)htFeeder[machineFeeder[i].FeederCode];
                SMTFeederMaterial feederMaterial = (SMTFeederMaterial)htSmtFeeder[machineFeeder[i].MachineCode + ":" + machineFeeder[i].MachineStationCode];
                if (feeder == null || feederMaterial == null)
                {
                    machineFeeder[i].FailReason = "$Data_Wrong";
                    continue;
                }

                // ����
                this.UpdateMachineFeederReelQty(moCodeFrom, machineFeeder[i].MachineCode, machineFeeder[i].MachineStationCode, machineFeeder[i].ReelNo, SMTLoadFeederOperationType.TransferMO, userCode);

                Reel reel = (Reel)this.GetReel(machineFeeder[i].ReelNo);
                reel.UsedFlag = FormatHelper.BooleanToString(true);
                reel.MOCode = mo.MOCode;
                reel.StepSequenceCode = stepSequenceCode;
                this.UpdateReel(reel);
                // Feeder����
                feeder.MOCode = mo.MOCode;
                // ����
                this.AddMachineFeederPass(mo, feeder, reel, feederMaterial, userCode, null, SMTLoadFeederOperationType.TransferMO, resourceCode, stepSequenceCode, string.Empty, string.Empty, FormatHelper.StringToBoolean(machineFeeder[i].StationEnabled));
                // ����������
                if (machineFeeder[i].NextReelNo != null && machineFeeder[i].NextReelNo != string.Empty)
                {
                    ReelQty reelQty = (ReelQty)this.GetReelQty(machineFeeder[i].NextReelNo, machineFeeder[i].MOCode);
                    if (reelQty != null)
                        this.DeleteReelQty(reelQty);
                    reel = (Reel)this.GetReel(machineFeeder[i].NextReelNo);
                    reel.UsedFlag = FormatHelper.BooleanToString(true);
                    reel.MOCode = mo.MOCode;
                    reel.StepSequenceCode = stepSequenceCode;
                    this.UpdateReel(reel);
                    AddMachineFeederPass(mo, feeder, reel, feederMaterial, userCode, null, SMTLoadFeederOperationType.Continue, resourceCode, stepSequenceCode, string.Empty, string.Empty, FormatHelper.StringToBoolean(machineFeeder[i].StationEnabled));
                }
                // ɾ��ԭ�����ϼ�¼
                this.DeleteMachineFeeder(machineFeeder[i]);
            }
            // ��Դ��������ΪʧЧ
            strSql = "UPDATE tblMachineFeeder SET Enabled='0' WHERE MOCode='" + moCodeFrom + "' AND SSCode='" + ssCodeFrom + "' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            msg.Add(new Message(MessageType.Success, "$TransferMaterialByMO_Success"));
            return msg;
        }

        // ����Tableʹ��
        public Messages SetTableGroupEnabled(MO mo, string sscode, string machineCode, string tableGroup, string resCode, string userCode)
        {
            Messages msg = new Messages();
            // ����Ƿ�ȫ������
            string strSql = "SELECT COUNT(*) FROM tblSMTFeederMaterial smt WHERE NOT EXISTS ";
            strSql += "(SELECT * FROM tblMachineFeeder mf WHERE smt.ProductCode=mf.ProductCode AND smt.MachineCode=mf.MachineCode ";
            strSql += "AND smt.MachineStationCode=mf.MachineStationCode AND smt.SSCode=mf.SSCode AND smt.MaterialCode=mf.MaterialCode ";
            strSql += "AND mf.MOCode='" + mo.MOCode + "' AND mf.SSCode='" + sscode + "' AND mf.TblGrp='" + tableGroup + "' AND smt.TblGrp='" + tableGroup + "' AND mf.MachineCode='" + machineCode + "' AND smt.MachineCode='" + machineCode + "' ) ";
            strSql += "AND (MachineCode,MachineStationCode) NOT IN ";
            strSql += "(SELECT MachineCode,MachineStationCode FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND SSCode='" + sscode + "' AND TblGrp='" + tableGroup + "' AND MachineCode='" + machineCode + "') ";
            strSql += "AND ProductCode='" + mo.ItemCode + "' AND SSCode='" + sscode + "' AND TblGrp='" + tableGroup + "' AND MachineCode='" + machineCode + "' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
            {
                msg.Add(new Message(MessageType.Error, "$SMTEnableTableGroup_Absent_MachineFeeder"));
                return msg;
            }
            // ��������������Ϊͣ��
            strSql = "UPDATE tblMachineFeeder SET StationEnabled='0' WHERE MOCode='" + mo.MOCode + "' AND SSCode='" + sscode + "' AND MachineCode='" + machineCode + "' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            // ����ָ������
            strSql = "UPDATE tblMachineFeeder SET StationEnabled='1' WHERE MOCode='" + mo.MOCode + "' AND SSCode='" + sscode + "' AND MachineCode='" + machineCode + "' AND TblGrp='" + tableGroup + "' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            // ����INNO
            this.GenerateMachineInno(mo.MOCode, sscode, machineCode, userCode);
            msg.Add(new Message(MessageType.Success, "$SMTEnabledTableGroup_Success"));
            return msg;
        }
        public Messages SetTableGroupDisabled(MO mo, string sscode, string machineCode, string tableGroup, string resCode, string userCode)
        {
            Messages msg = new Messages();
            // ͣ�ù���
            string strSql = "UPDATE tblMachineFeeder SET Enabled='0' WHERE MOCode='" + mo.MOCode + "' AND SSCode='" + sscode + "' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            // ͣ������
            strSql = "UPDATE tblMachineFeeder SET StationEnabled='0' WHERE MOCode='" + mo.MOCode + "' AND SSCode='" + sscode + "' AND MachineCode='" + machineCode + "' AND TblGrp='" + tableGroup + "' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            // ͣ����
            msg.AddMessages(this.AddStopLineLog(mo.MOCode, sscode, SMTLoadFeederOperationType.MODisabled, userCode));
            if (msg.IsSuccess() == true)
            {
                msg.Add(new Message(MessageType.Success, "$SMTDisabledTableGroup_Success"));
                msg.Add(new Message(MessageType.Success, "$SMT_Disabled_Success"));
            }
            return msg;
        }
        #endregion

        #region Feeder
        /// <summary>
        /// ϵͳ�û�
        /// </summary>
        public Feeder CreateNewFeeder()
        {
            return new Feeder();
        }

        public void AddFeeder(Feeder feeder)
        {
            feeder.Status = BenQGuru.eMES.Web.Helper.FeederStatus.Normal;
            feeder.UseFlag = FormatHelper.BooleanToString(false);
            this._helper.AddDomainObject(feeder);
        }

        public void UpdateFeeder(Feeder feeder)
        {
            this._helper.UpdateDomainObject(feeder);
        }

        public void DeleteFeeder(Feeder feeder)
        {
            if (FormatHelper.StringToBoolean(feeder.UseFlag) == true || feeder.Status != FeederStatus.Normal)
                throw new Exception("$Feeder_In_Used");
            this._helper.DeleteDomainObject(feeder);
        }

        public void DeleteFeeder(Feeder[] feeder)
        {
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.BeginTransaction();
            try
            {
                for (int i = 0; i < feeder.Length; i++)
                {
                    DeleteFeeder(feeder[i]);
                }
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CommitTransaction();
            }
            catch (Exception e)
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.RollbackTransaction();
                throw new Exception(e.Message);
            }
            finally
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }
        }

        public object GetFeeder(string feederCode)
        {
            return this.DataProvider.CustomSearch(typeof(Feeder), new object[] { feederCode });
        }

        /// <summary>
        /// ** ��������:	��ѯFeeder��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="feederCode">FeederCode��ģ����ѯ</param>
        /// <returns> Feeder���ܼ�¼��</returns>
        public int QueryFeederCount(string feederCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from tblFEEDER where 1=1 and FEEDERCODE like '{0}%' ", feederCode)));
        }
        public int QueryFeederCount(string feederCode, string feederSpecCode, string feederStatus)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from tblFEEDER where 1=1 and FEEDERCODE like '{0}%' and FEEDERSPECCODE like '{1}%' and STATUS like '{2}%' ", feederCode, feederSpecCode, feederStatus)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯFeeder
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="feederCode">FeederCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Feeder����</returns>
        public object[] QueryFeeder(string feederCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(Feeder), new PagerCondition(string.Format("select {0} from tblFEEDER where 1=1 and FEEDERCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Feeder)), feederCode), "FEEDERCODE", inclusive, exclusive));
        }
        public object[] QueryFeeder(string feederCode, string feederSpecCode, string feederStatus, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(Feeder), new PagerCondition(string.Format("select {0} from tblFEEDER where 1=1 and FEEDERCODE like '{1}%' and FEEDERSPECCODE like '{2}%' and STATUS like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Feeder)), feederCode, feederSpecCode, feederStatus), "FEEDERCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�Feeder
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>Feeder���ܼ�¼��</returns>
        public object[] GetAllFeeder()
        {
            return this.DataProvider.CustomQuery(typeof(Feeder), new SQLCondition(string.Format("select {0} from tblFEEDER order by FEEDERCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Feeder)))));
        }

        /// <summary>
        /// ��ѯ��������Ҫ���õ�Feeder����Լ������õ�Feeder
        /// </summary>
        /// <param name="moCode"></param>
        /// <returns></returns>
        public Messages GetFeederByMOCode(string moCode, string sscode, ref Feeder[] retFeeders)
        {
            retFeeders = null;
            // ��ѯ��Ʒ����
            BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(moCode);
            if (mo == null)
            {
                Messages msg = new Messages();
                msg.Add(new Message(MessageType.Error, "$CS_MO_Not_Exist"));
                return msg;
            }
            string productCode = mo.ItemCode;
            // ��ѯ��Ʒ��վ��Ϣ
            string strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + productCode + "' AND SSCode='" + sscode + "'";
            object[] objSmt = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (objSmt == null || objSmt.Length == 0)
                return null;
            // ����ÿ��Feeder������������
            Hashtable htSmt = new Hashtable();
            Hashtable htSmtQty = new Hashtable();
            for (int i = 0; i < objSmt.Length; i++)
            {
                SMTFeederMaterial feederMaterial = (SMTFeederMaterial)objSmt[i];
                if (htSmt.ContainsKey(feederMaterial.FeederSpecCode) == false)
                {
                    htSmt.Add(feederMaterial.FeederSpecCode, "0");
                }
                if (htSmtQty.ContainsKey(feederMaterial.FeederSpecCode) == false)
                {
                    htSmtQty.Add(feederMaterial.FeederSpecCode, "1");
                }
                else
                {
                    htSmtQty[feederMaterial.FeederSpecCode] = Convert.ToInt32(htSmtQty[feederMaterial.FeederSpecCode]) + 1;
                }
            }
            // ��ѯ���������õ�Feeder
            ArrayList list = new ArrayList();
            strSql = "SELECT * FROM tblFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + sscode + "' AND UseFlag='1'";
            object[] objFeeders = this.DataProvider.CustomQuery(typeof(Feeder), new SQLCondition(strSql));
            if (objFeeders != null)
            {
                for (int i = 0; i < objFeeders.Length; i++)
                {
                    Feeder feeder = (Feeder)objFeeders[i];
                    if (htSmt.ContainsKey(feeder.FeederSpecCode) == true)
                    {
                        htSmt[feeder.FeederSpecCode] = Convert.ToInt32(htSmt[feeder.FeederSpecCode]) + 1;
                    }
                }
                // ������������
                for (int i = 0; i < objFeeders.Length; i++)
                {
                    Feeder feeder = (Feeder)objFeeders[i];
                    if (htSmt.ContainsKey(feeder.FeederSpecCode) == true)
                    {
                        feeder.EAttribute1 = htSmtQty[feeder.FeederSpecCode].ToString() + "," + htSmt[feeder.FeederSpecCode].ToString();
                        list.Add(feeder);
                    }
                }
            }
            // ���ػ�û��Feeder������
            foreach (object feederSpecCode in htSmt.Keys)
            {
                if (Convert.ToInt32(htSmt[feederSpecCode]) == 0)
                {
                    Feeder feeder = new Feeder();
                    feeder.FeederSpecCode = feederSpecCode.ToString();
                    feeder.EAttribute1 = htSmtQty[feederSpecCode].ToString() + "," + htSmt[feederSpecCode].ToString();
                    list.Add(feeder);
                }
            }
            Feeder[] feeders = new Feeder[list.Count];
            list.CopyTo(feeders);
            retFeeders = feeders;
            return null;
        }

        /// <summary>
        /// Feeder״̬����Log
        /// </summary>
        private void WriteFeederLog(Feeder feeder, string oldStatus, string changedReason, string userCode)
        {
            FeederStatusLog log = new FeederStatusLog();
            string strSql = "SELECT * FROM tblFeederStatusLog WHERE FeederCode='" + feeder.FeederCode + "' AND Seq=(SELECT MAX(Seq) FROM tblFeederStatusLog WHERE FeederCode='" + feeder.FeederCode + "')";
            object[] feederSeq = this.DataProvider.CustomQuery(typeof(FeederStatusLog), new SQLCondition(strSql));
            decimal iSeq = 1;
            if (feederSeq != null && feederSeq.Length > 0)
                iSeq = Convert.ToDecimal(((FeederStatusLog)feederSeq[0]).Seq) + 1;
            log.FeederCode = feeder.FeederCode;
            log.Seq = iSeq;
            log.FeederType = feeder.FeederType;
            log.FeederSpecCode = feeder.FeederSpecCode;
            log.Status = feeder.Status;
            log.OldStatus = oldStatus;
            log.StatusChangedReason = changedReason;

            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            log.StatusChangedDate = FormatHelper.TODateInt(dtNow);
            log.StatusChangedTime = FormatHelper.TOTimeInt(dtNow);
            log.MOCode = feeder.MOCode;
            log.StepSequenceCode = feeder.StepSequenceCode;
            log.OperationUser = feeder.OperationUser;
            log.MaintainUser = userCode;
            log.MaintainDate = FormatHelper.TODateInt(dtNow);
            log.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.AddFeederStatusLog(log);
        }

        /// <summary>
        /// Feederά��Log
        /// </summary>
        private void WriteFeederMaintainLog(Feeder feeder, string maintainType, string oldStatus, string analyseReason, string operationMessage, bool scrapFlag, string userCode, DataTable maintainList)
        {
            FeederMaintain log = new FeederMaintain();
            string strSql = "SELECT * FROM tblFeederMaintain WHERE FeederCode='" + feeder.FeederCode + "' AND Seq=(SELECT MAX(Seq) FROM tblFeederMaintain WHERE FeederCode='" + feeder.FeederCode + "')";
            object[] feederSeq = this.DataProvider.CustomQuery(typeof(FeederMaintain), new SQLCondition(strSql));
            int iSeq = 1;
            if (feederSeq != null && feederSeq.Length > 0)
                iSeq = Convert.ToInt32(((FeederMaintain)feederSeq[0]).Seq) + 1;
            log.FeederCode = feeder.FeederCode;
            log.Seq = iSeq.ToString();
            log.FeederType = feeder.FeederType;
            log.FeederSpecCode = feeder.FeederSpecCode;
            log.Status = feeder.Status;
            log.MaxCount = feeder.MaxCount;
            log.UsedCount = feeder.UsedCount;
            log.TotalCount = feeder.TotalCount;
            log.OldStatus = oldStatus;
            log.MaintainType = maintainType;
            log.ReturnReason = feeder.ReturnReason;
            log.AnalyseReason = analyseReason;
            log.OperationMessage = operationMessage;
            log.ScrapFlag = FormatHelper.BooleanToString(scrapFlag);
            log.MaintainUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            log.MaintainDate = FormatHelper.TODateInt(dtNow);
            log.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.AddFeederMaintain(log);

            if (maintainList != null)
            {
                DataRow row = maintainList.NewRow();
                row["FeederCode"] = log.FeederCode;
                row["MaxCount"] = log.MaxCount;
                row["OldStatus"] = log.OldStatus;
                row["MaintainType"] = log.MaintainType;
                row["ReturnReason"] = log.ReturnReason;
                row["AnalyseReason"] = log.AnalyseReason;
                row["OperationMessage"] = log.OperationMessage;
                row["Status"] = log.Status;
                row["MaintainUser"] = log.MaintainUser;
                row["MaintainDate"] = FormatHelper.ToDateString(log.MaintainDate);
                row["MaintainTime"] = FormatHelper.ToTimeString(log.MaintainTime);
                maintainList.Rows.Add(row);
            }
        }

        /// <summary>
        /// Feeder����
        /// </summary>
        /// <param name="moCode">��������</param>
        /// <param name="feederCode">Feeder���</param>
        /// <param name="feederSpecList">Feeder����б�</param>
        /// <returns></returns>
        public Messages FeederGetOut(string moCode, string sscode, string feederCode, string operationUser, DataTable feederSpecList, string userCode)
        {
            Messages messages = new Messages();
            // ����Ƿ������������ϱȶ�
            string strSql = "SELECT * FROM tblSMTCheckMaterial WHERE CheckID=(SELECT MAX(CheckID) FROM tblSMTCheckMaterial WHERE MOCode='" + moCode + "' AND SSCode='" + sscode + "')";
            object[] objschk = this.DataProvider.CustomQuery(typeof(SMTCheckMaterial), new SQLCondition(strSql));
            bool bPass = false;
            if (objschk != null && objschk.Length > 0)
            {
                SMTCheckMaterial chk = (SMTCheckMaterial)objschk[0];
                if (chk.CheckResult == SMTCheckMaterialResult.Accept_Exception ||
                    chk.CheckResult == SMTCheckMaterialResult.Matchable)
                {
                    bPass = true;
                }
            }
            if (bPass == false)
            {
                messages.Add(new Message(MessageType.Error, "$MOCode_Not_Check_SMTMaterial_MOBOM"));
                return messages;
            }

            // ��ѯFeeder
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            if (feeder == null)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Not_Exist [" + feederCode + "]"));
                return messages;
            }
            // ���״̬
            if (feeder.Status != BenQGuru.eMES.Web.Helper.FeederStatus.Normal)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Status_Error [" + feeder.Status + "]"));
                return messages;
            }
            // ����Ƿ���ʹ��
            if (FormatHelper.StringToBoolean(feeder.UseFlag) == true)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_In_Used"));
                return messages;
            }
            // ���Feeder����Ƿ�����վ����
            int iSpecIdx = -1;
            string strStatusText = string.Empty;
            for (int i = 0; i < feederSpecList.Rows.Count; i++)
            {
                if (strStatusText == string.Empty && feederSpecList.Rows[i]["StatusValue"].ToString() == FeederStatus.Normal)
                    strStatusText = feederSpecList.Rows[i]["Status"].ToString();
                if (feederSpecList.Rows[i]["FeederSpecCode"].ToString() == feeder.FeederSpecCode)
                {
                    iSpecIdx = i;
                    /*	Feeder������
                    if (Convert.ToInt32(feederSpecList.Rows[iSpecIdx]["GetOutQty"]) >= Convert.ToInt32(feederSpecList.Rows[iSpecIdx]["Qty"]))
                    {
                        messages.Add(new Message(MessageType.Error, "$FeederSpec_Full"));
                        return messages;
                    }
                    */
                    break;
                }
            }
            if (iSpecIdx == -1)
            {
                messages.Add(new Message(MessageType.Error, "$FeederSpec_Not_Allow"));
                return messages;
            }

            //����Feeder
            feeder.UseFlag = FormatHelper.BooleanToString(true);
            feeder.MOCode = moCode;
            feeder.StepSequenceCode = sscode;
            feeder.OperationUser = operationUser;
            feeder.CurrentUnitQty = 0;
            feeder.MaintainUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            feeder.MaintainDate = FormatHelper.TODateInt(dtNow);
            feeder.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.UpdateFeeder(feeder);

            // ���·���Table
            DataRow row = feederSpecList.Rows[iSpecIdx];
            if (Convert.ToInt32(row["GetOutQty"]) == 0)
            {
                row["GetOutQty"] = 1;
            }
            else
            {
                row = feederSpecList.NewRow();
                row["FeederSpecCode"] = feederSpecList.Rows[iSpecIdx]["FeederSpecCode"];
                row["Qty"] = feederSpecList.Rows[iSpecIdx]["Qty"];
                row["GetOutQty"] = Convert.ToInt32(feederSpecList.Rows[iSpecIdx]["GetOutQty"]) + 1;

                for (int i = 0; i < feederSpecList.Rows.Count; i++)
                {
                    if (feederSpecList.Rows[i]["FeederSpecCode"].ToString() == row["FeederSpecCode"].ToString())
                    {
                        feederSpecList.Rows[i]["GetOutQty"] = Convert.ToInt32(feederSpecList.Rows[i]["GetOutQty"]) + 1;
                    }
                }
                feederSpecList.Rows.InsertAt(row, iSpecIdx);
            }
            row["FeederCode"] = feeder.FeederCode;
            row["MaxCount"] = feeder.MaxCount;
            row["UsedCount"] = feeder.UsedCount;
            if (strStatusText == string.Empty)
            {
                BenQGuru.eMES.BaseSetting.SystemSettingFacade sysSetting = new BenQGuru.eMES.BaseSetting.SystemSettingFacade(this.DataProvider);
                BenQGuru.eMES.Domain.BaseSetting.Parameter param = (BenQGuru.eMES.Domain.BaseSetting.Parameter)sysSetting.GetParameter(feeder.Status.ToUpper(), "FEEDERSTATUS");
                strStatusText = param.ParameterAlias;
            }
            row["Status"] = strStatusText;
            row["StatusValue"] = feeder.Status;

            // дLog
            WriteFeederLog(feeder, feeder.Status, FeederOperationType.GetOut, userCode);
            return messages;
        }

        /// <summary>
        /// Feeder����
        /// </summary>
        public Messages FeederExchange(string moCode, string feederCode, string oldFeederCode, string returnReason, bool disabledFlag, bool autoExchangeFeeder, DataTable feederSpecList, string userCode)
        {
            Messages messages = new Messages();
            // ԭFeeder�Ƿ�������
            string strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE FeederCode='" + oldFeederCode + "'";
            if (oldFeederCode != string.Empty)
            {
                if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
                {
                    messages.Add(new Message(MessageType.Error, "$Feeder_Loaded_In_Machine"));
                    return messages;
                }
            }

            // ��ѯFeeder
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            if (feeder == null)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Not_Exist [" + feederCode + "]"));
                return messages;
            }
            // ���״̬
            if (feeder.Status != BenQGuru.eMES.Web.Helper.FeederStatus.Normal)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Status_Error [" + feeder.Status + "]"));
                return messages;
            }
            // ����Ƿ���ʹ��
            if (FormatHelper.StringToBoolean(feeder.UseFlag) == true)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_In_Used"));
                return messages;
            }

            // �Զ�������ͬ����Feeder
            if (oldFeederCode == string.Empty && autoExchangeFeeder == true)
            {
                strSql = "SELECT FeederCode FROM tblFeeder WHERE UseFlag='1' AND FeederSpecCode='" + feeder.FeederSpecCode + "' AND MOCode='" + moCode + "' ";
                strSql += " AND FeederCode NOT IN (SELECT FeederCode FROM tblMachineFeeder WHERE FeederSpecCode='" + feeder.FeederSpecCode + "' AND CHECKRESULT='1' )";
                object[] objsTmp = this.DataProvider.CustomQuery(typeof(Feeder), new SQLCondition(strSql));
                if (objsTmp == null || objsTmp.Length == 0)
                {
                    messages.Add(new Message(MessageType.Error, "$Feeder_Loaded_In_Machine"));
                    return messages;
                }
                oldFeederCode = ((Feeder)objsTmp[0]).FeederCode;
            }
            // ���Feeder����Ƿ�����վ����
            int iSpecIdx = -1;
            string strStatusText = string.Empty;
            for (int i = 0; i < feederSpecList.Rows.Count; i++)
            {
                if (strStatusText == string.Empty && feederSpecList.Rows[i]["StatusValue"].ToString() == FeederStatus.Normal)
                    strStatusText = feederSpecList.Rows[i]["Status"].ToString();
                if (feederSpecList.Rows[i]["FeederCode"].ToString() == oldFeederCode)
                {
                    iSpecIdx = i;
                    break;
                }
            }
            if (iSpecIdx == -1)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Not_Exist [" + oldFeederCode + "]"));
                return messages;
            }

            //����Feeder
            feeder.UseFlag = FormatHelper.BooleanToString(true);
            feeder.MOCode = moCode;
            feeder.MaintainUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            feeder.MaintainDate = FormatHelper.TODateInt(dtNow);
            feeder.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.UpdateFeeder(feeder);
            //дLog
            WriteFeederLog(feeder, feeder.Status, FeederOperationType.Exchange, userCode);
            // ���·���Table
            DataRow row = feederSpecList.Rows[iSpecIdx];
            row["FeederCode"] = feeder.FeederCode;
            row["MaxCount"] = feeder.MaxCount;
            row["UsedCount"] = feeder.UsedCount;
            if (strStatusText == string.Empty)
            {
                BenQGuru.eMES.BaseSetting.SystemSettingFacade sysSetting = new BenQGuru.eMES.BaseSetting.SystemSettingFacade(this.DataProvider);
                BenQGuru.eMES.Domain.BaseSetting.Parameter param = (BenQGuru.eMES.Domain.BaseSetting.Parameter)sysSetting.GetParameter(feeder.Status, BenQGuru.eMES.Web.Helper.FeederStatus.GroupType);
                strStatusText = param.ParameterAlias;
            }
            row["Status"] = strStatusText;
            row["StatusValue"] = feeder.Status;

            feeder = (Feeder)this.GetFeeder(oldFeederCode);
            // ����ԭFeeder
            if (feeder == null)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Not_Exist [" + oldFeederCode + "]"));
                return messages;
            }
            string strOldStatus = feeder.Status;
            feeder.UseFlag = FormatHelper.BooleanToString(false);
            feeder.MOCode = string.Empty;
            feeder.MaintainUser = userCode;

            feeder.MaintainDate = FormatHelper.TODateInt(dtNow);
            feeder.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            if (disabledFlag == true)
            {
                feeder.Status = FeederStatus.Disabled;
                feeder.ReturnReason = returnReason;
            }
            this.UpdateFeeder(feeder);
            // дLog
            WriteFeederLog(feeder, strOldStatus, FeederOperationType.Exchange, userCode);
            return messages;
        }

        /// <summary>
        /// Feeder�˻�
        /// </summary>
        public Messages FeederReturn(string moCode, string sscode, string feederCode, string returnReason, bool disabledFlag, string operationUser, DataTable feederSpecList, string userCode)
        {
            Messages messages = new Messages();
            // Feeder�Ƿ�������
            string strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE FeederCode='" + feederCode + "'";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Loaded_In_Machine"));
                return messages;
            }
            // ��ѯFeeder
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            if (feeder == null)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Not_Exist [" + feederCode + "]"));
                return messages;
            }
            // ���Feeder����Ƿ�����վ����
            int iSpecIdx = -1;
            for (int i = 0; i < feederSpecList.Rows.Count; i++)
            {
                if (feederSpecList.Rows[i]["FeederCode"].ToString() == feeder.FeederCode)
                {
                    iSpecIdx = i;
                    break;
                }
            }
            if (iSpecIdx == -1)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Not_Exist"));
                return messages;
            }

            //����Feeder
            string strOldStatus = feeder.Status;
            feeder.UseFlag = FormatHelper.BooleanToString(false);
            feeder.ReturnReason = returnReason;
            feeder.OperationUser = operationUser;
            feeder.MaintainUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            feeder.MaintainDate = FormatHelper.TODateInt(dtNow);
            feeder.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            if (disabledFlag == true)
            {
                feeder.Status = FeederStatus.Disabled;
            }
            // дLog
            WriteFeederLog(feeder, strOldStatus, FeederOperationType.Return, userCode);
            // ����Feeder����
            feeder.MOCode = string.Empty;
            feeder.StepSequenceCode = string.Empty;
            this.UpdateFeeder(feeder);

            // ���·���Table
            DataRow row = feederSpecList.Rows[iSpecIdx];
            if (Convert.ToInt32(row["GetOutQty"]) == 1)
            {
                row["GetOutQty"] = 0;
                row["FeederCode"] = string.Empty;
                row["MaxCount"] = 0;
                row["UsedCount"] = 0;
                row["Status"] = string.Empty;
                row["StatusValue"] = string.Empty;
            }
            else
            {
                for (int i = 0; i < feederSpecList.Rows.Count; i++)
                {
                    if (feederSpecList.Rows[i]["FeederSpecCode"].ToString() == row["FeederSpecCode"].ToString())
                    {
                        feederSpecList.Rows[i]["GetOutQty"] = Convert.ToInt32(feederSpecList.Rows[i]["GetOutQty"]) - 1;
                    }
                }
                feederSpecList.Rows.RemoveAt(iSpecIdx);
            }
            return messages;
        }

        /// <summary>
        /// Feeder�˻�
        /// </summary>
        public Messages FeederReturnAll(string moCode, string sscode, string operationUser, DataTable feederSpecList, string userCode)
        {
            Messages messages = new Messages();

            //string strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND CheckResult='1'";
            //if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
            //{
            //    messages.Add(new Message(MessageType.Error, "$Feeder_Loaded_In_Machine"));
            //    return messages;
            //}

            //������ ״̬��Feeder�����˻ض���
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + sscode + "'";
            object[] machineFeeder = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            string feederStr = "";
            if (machineFeeder != null)
            {
                if (machineFeeder.Length == feederSpecList.Rows.Count)
                {
                    messages.Add(new Message(MessageType.Error, "$Feeder_Loaded_In_Machine"));
                    return messages;
                }
                System.Text.StringBuilder feederSB = new System.Text.StringBuilder();
                foreach (MachineFeeder item in machineFeeder)
                {
                    if (String.IsNullOrEmpty(feederSB.ToString()))
                    {
                        feederSB.Append("('");
                        feederSB.Append(item.FeederCode);
                        feederSB.Append("'");
                    }
                    else
                    {
                        feederSB.Append(",'");
                        feederSB.Append(item.FeederCode);
                        feederSB.Append("'");
                    }
                }
                feederSB.Append(")");
                feederStr = feederSB.ToString();
            }

            FeederStatusLog log = new FeederStatusLog();
            strSql = "SELECT * FROM tblFeederStatusLog WHERE Seq=(SELECT MAX(Seq) FROM tblFeederStatusLog)";
            object[] feederSeq = this.DataProvider.CustomQuery(typeof(FeederStatusLog), new SQLCondition(strSql));
            int iSeq = 1;
            if (feederSeq != null && feederSeq.Length > 0)
                iSeq = Convert.ToInt32(((FeederStatusLog)feederSeq[0]).Seq) + 1;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            int iDate = FormatHelper.TODateInt(dtNow);
            int iTime = FormatHelper.TOTimeInt(dtNow);
            sb.Append("INSERT INTO tblFeederStatusLog (FeederCode,Seq,FeederType,FeederSpecCode,Status,OldStatus,StatusChgReason,StatusChgDate,StatusChgTime,MOCode,SSCode,OPEUSER,OtherMessage,MUser,MDate,MTime) ");
            sb.Append("SELECT FeederCode,ROWNUM+" + iSeq.ToString() + ",FeederType,FeederSpecCode,Status,Status,'Return'," + iDate.ToString() + "," + iTime + ",MOCode,SSCode,'" + operationUser + "','','" + userCode + "'," + iDate.ToString() + "," + iTime.ToString() + " FROM tblFeeder ");
            sb.Append(" WHERE UseFlag='1' AND MOCode='" + moCode + "' AND SSCode='" + sscode + "'");
            if (!string.IsNullOrEmpty(feederStr))
            {
                sb.Append(" AND FeederCode NOT IN " + feederStr);
            }
            this.DataProvider.CustomExecute(new SQLCondition(sb.ToString()));

            sb = new System.Text.StringBuilder();
            sb.Append("UPDATE tblFeeder SET UseFlag='0',MOCode='',SSCode='',MUser='" + userCode + "',OPEUSER='" + operationUser + "'");
            sb.Append(",MDate=" + FormatHelper.TODateInt(dtNow).ToString());
            sb.Append(",MTime=" + FormatHelper.TOTimeInt(dtNow));
            sb.Append(" WHERE UseFlag='1' AND MOCode='" + moCode + "' AND SSCode='" + sscode + "'");
            if (!string.IsNullOrEmpty(feederStr))
            {
                sb.Append(" AND FeederCode NOT IN " + feederStr);
            }
            this.DataProvider.CustomExecute(new SQLCondition(sb.ToString()));

            // ���·���Table
            ArrayList listTmp = new ArrayList();
            if (machineFeeder != null)
            {
                for (int i = feederSpecList.Rows.Count - 1; i >= 0; i--)
                {
                    bool isReturn = true;
                    foreach (MachineFeeder item in machineFeeder)
                    {
                        if (item.FeederCode == feederSpecList.Rows[i]["FeederCode"].ToString())
                        {
                            isReturn = false;
                        }
                    }
                    if (isReturn)
                    {
                        if (listTmp.Contains(feederSpecList.Rows[i]["FeederSpecCode"].ToString()))
                        {
                            feederSpecList.Rows.RemoveAt(i);
                        }
                        else
                        {
                            feederSpecList.Rows[i]["GetOutQty"] = 0;
                            feederSpecList.Rows[i]["FeederCode"] = string.Empty;
                            feederSpecList.Rows[i]["MaxCount"] = 0;
                            feederSpecList.Rows[i]["UsedCount"] = 0;
                            feederSpecList.Rows[i]["Status"] = string.Empty;
                            feederSpecList.Rows[i]["StatusValue"] = string.Empty;
                            listTmp.Add(feederSpecList.Rows[i]["FeederSpecCode"].ToString());
                        }
                    }
                }
            }
            else
            {
                for (int i = feederSpecList.Rows.Count - 1; i >= 0; i--)
                {
                    if (listTmp.Contains(feederSpecList.Rows[i]["FeederSpecCode"].ToString()))
                    {
                        feederSpecList.Rows.RemoveAt(i);
                    }
                    else
                    {
                        feederSpecList.Rows[i]["GetOutQty"] = 0;
                        feederSpecList.Rows[i]["FeederCode"] = string.Empty;
                        feederSpecList.Rows[i]["MaxCount"] = 0;
                        feederSpecList.Rows[i]["UsedCount"] = 0;
                        feederSpecList.Rows[i]["Status"] = string.Empty;
                        feederSpecList.Rows[i]["StatusValue"] = string.Empty;
                        listTmp.Add(feederSpecList.Rows[i]["FeederSpecCode"].ToString());
                    }
                }
            }
            return messages;
        }

        /// <summary>
        /// Feeder����
        /// </summary>
        public Messages FeederMaintain(string feederCode, string userCode, DataTable maintainList)
        {
            Messages messages = new Messages();
            // ��ѯFeeder
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            if (feeder == null)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Not_Exist [" + feederCode + "]"));
                return messages;
            }
            // ����Ƿ���ʹ��
            if (FormatHelper.StringToBoolean(feeder.UseFlag) == true)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_In_Used"));
                return messages;
            }
            // ���״̬
            if (feeder.Status == FeederStatus.Scrap)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Status_Error"));
                return messages;
            }
            string strOldStatus = feeder.Status;
            // ����Feeder
            feeder.OperationUser = userCode;
            feeder.Status = FeederStatus.Normal;
            feeder.TotalCount = feeder.TotalCount + feeder.UsedCount;
            feeder.UsedCount = 0;

            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            feeder.TheMaintainDate = FormatHelper.TODateInt(dtNow);
            feeder.TheMaintainTime = FormatHelper.TOTimeInt(dtNow);
            feeder.TheMaintainUser = userCode;
            this.UpdateFeeder(feeder);
            // дLog
            WriteFeederLog(feeder, strOldStatus, FeederOperationType.Maintain, userCode);
            WriteFeederMaintainLog(feeder, FeederOperationType.Maintain, strOldStatus, string.Empty, string.Empty, false, userCode, maintainList);

            return messages;
        }

        /// <summary>
        /// FeederУ��
        /// </summary>
        public Messages FeederAdjust(string feederCode, string analyseReason, string operationMessage, bool scrapFlag, string userCode, DataTable maintainList)
        {
            Messages messages = new Messages();
            // ��ѯFeeder
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            if (feeder == null)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Not_Exist [" + feederCode + "]"));
                return messages;
            }
            // ����Ƿ���ʹ��
            if (FormatHelper.StringToBoolean(feeder.UseFlag) == true)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_In_Used"));
                return messages;
            }
            // ���״̬
            if (feeder.Status == FeederStatus.Scrap)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Status_Error"));
                return messages;
            }
            string strOldStatus = feeder.Status;
            // ����Feeder
            feeder.OperationUser = userCode;
            if (scrapFlag == true)
                feeder.Status = FeederStatus.Scrap;
            else
            {
                feeder.Status = FeederStatus.Normal;
                feeder.TotalCount = feeder.TotalCount + feeder.UsedCount;
                feeder.UsedCount = 0;
            }
            this.UpdateFeeder(feeder);
            // дLog
            WriteFeederLog(feeder, strOldStatus, FeederOperationType.Adjust, userCode);
            WriteFeederMaintainLog(feeder, FeederOperationType.Adjust, strOldStatus, analyseReason, operationMessage, scrapFlag, userCode, maintainList);
            return messages;
        }

        /// <summary>
        /// Feeder����
        /// </summary>
        public Messages FeederAnalyse(string feederCode, string analyseReason, string operationMessage, bool scrapFlag, string userCode, DataTable maintainList)
        {
            Messages messages = new Messages();
            // ��ѯFeeder
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            if (feeder == null)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Not_Exist [" + feederCode + "]"));
                return messages;
            }
            // ����Ƿ���ʹ��
            if (FormatHelper.StringToBoolean(feeder.UseFlag) == true)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_In_Used"));
                return messages;
            }
            // ���״̬
            if (feeder.Status == FeederStatus.Scrap)
            {
                messages.Add(new Message(MessageType.Error, "$Feeder_Status_Error"));
                return messages;
            }
            string strOldStatus = feeder.Status;
            // ����Feeder
            feeder.OperationUser = userCode;
            if (scrapFlag == true)
                feeder.Status = FeederStatus.Scrap;
            else
            {
                feeder.Status = FeederStatus.Normal;
                feeder.TotalCount = feeder.TotalCount + feeder.UsedCount;
                feeder.UsedCount = 0;
            }
            this.UpdateFeeder(feeder);
            // дLog
            WriteFeederLog(feeder, strOldStatus, FeederOperationType.Analyse, userCode);
            WriteFeederMaintainLog(feeder, FeederOperationType.Analyse, strOldStatus, analyseReason, operationMessage, scrapFlag, userCode, maintainList);
            return messages;
        }

        //
        #endregion

        #region FeederMaintain
        /// <summary>
        /// Feederά����Ϣ
        /// </summary>
        public FeederMaintain CreateNewFeederMaintain()
        {
            return new FeederMaintain();
        }

        public void AddFeederMaintain(FeederMaintain feederMaintain)
        {
            this._helper.AddDomainObject(feederMaintain);
        }

        public void UpdateFeederMaintain(FeederMaintain feederMaintain)
        {
            this._helper.UpdateDomainObject(feederMaintain);
        }

        public void DeleteFeederMaintain(FeederMaintain feederMaintain)
        {
            this._helper.DeleteDomainObject(feederMaintain);
        }

        public void DeleteFeederMaintain(FeederMaintain[] feederMaintain)
        {
            this._helper.DeleteDomainObject(feederMaintain);
        }

        public object GetFeederMaintain(string feederCode, string seq)
        {
            return this.DataProvider.CustomSearch(typeof(FeederMaintain), new object[] { feederCode, seq });
        }

        /// <summary>
        /// ** ��������:	��ѯFeederMaintain��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-26 11:03:18
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="feederCode">FeederCode��ģ����ѯ</param>
        /// <param name="seq">Seq��ģ����ѯ</param>
        /// <returns> FeederMaintain���ܼ�¼��</returns>
        public int QueryFeederMaintainCount(string feederCode, string seq)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLFEEDERMAINTAIN where 1=1 and FEEDERCODE like '{0}%'  and SEQ like '{1}%' ", feederCode, seq)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯFeederMaintain
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-26 11:03:18
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="feederCode">FeederCode��ģ����ѯ</param>
        /// <param name="seq">Seq��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> FeederMaintain����</returns>
        public object[] QueryFeederMaintain(string feederCode, string seq, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(FeederMaintain), new PagerCondition(string.Format("select {0} from TBLFEEDERMAINTAIN where 1=1 and FEEDERCODE like '{1}%'  and SEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(FeederMaintain)), feederCode, seq), "FEEDERCODE,SEQ", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�FeederMaintain
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-26 11:03:18
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>FeederMaintain���ܼ�¼��</returns>
        public object[] GetAllFeederMaintain()
        {
            return this.DataProvider.CustomQuery(typeof(FeederMaintain), new SQLCondition(string.Format("select {0} from TBLFEEDERMAINTAIN order by FEEDERCODE,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(FeederMaintain)))));
        }


        #endregion

        #region FeederSpec
        /// <summary>
        /// ϵͳ�û�
        /// </summary>
        public FeederSpec CreateNewFeederSpec()
        {
            return new FeederSpec();
        }

        public void AddFeederSpec(FeederSpec feederSpec)
        {
            this._helper.AddDomainObject(feederSpec);
        }

        public void UpdateFeederSpec(FeederSpec feederSpec)
        {
            this._helper.UpdateDomainObject(feederSpec);
        }

        public void DeleteFeederSpec(FeederSpec feederSpec)
        {
            string strSql = "SELECT COUNT(FeederCode) FROM tblFeeder WHERE FeederSpecCode='" + feederSpec.FeederSpecCode + "'";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
            {
                throw new Exception("$FeederSpecCode_In_Used");
            }
            this._helper.DeleteDomainObject(feederSpec);
        }

        public void DeleteFeederSpec(FeederSpec[] feederSpec)
        {
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.BeginTransaction();
            try
            {
                for (int i = 0; i < feederSpec.Length; i++)
                {
                    DeleteFeederSpec(feederSpec[i]);
                }
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CommitTransaction();
            }
            catch (Exception e)
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.RollbackTransaction();
                throw new Exception(e.Message);
            }
            finally
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }
        }

        public object GetFeederSpec(string feederSpecCode)
        {
            return this.DataProvider.CustomSearch(typeof(FeederSpec), new object[] { feederSpecCode });
        }

        /// <summary>
        /// ** ��������:	��ѯFeederSpec��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="feederSpecCode">FeederSpecCode��ģ����ѯ</param>
        /// <returns> FeederSpec���ܼ�¼��</returns>
        public int QueryFeederSpecCount(string feederSpecCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from tblFEEDERSPEC where 1=1 and FEEDERSPECCODE like '{0}%' ", feederSpecCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯFeederSpec
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="feederSpecCode">FeederSpecCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> FeederSpec����</returns>
        public object[] QueryFeederSpec(string feederSpecCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(FeederSpec), new PagerCondition(string.Format("select {0} from tblFEEDERSPEC where 1=1 and FEEDERSPECCODE like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(FeederSpec)), feederSpecCode), "FEEDERSPECCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�FeederSpec
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>FeederSpec���ܼ�¼��</returns>
        public object[] GetAllFeederSpec()
        {
            return this.DataProvider.CustomQuery(typeof(FeederSpec), new SQLCondition(string.Format("select {0} from tblFEEDERSPEC order by FEEDERSPECCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(FeederSpec)))));
        }


        #endregion

        #region FeederStatusLog
        /// <summary>
        /// ϵͳ�û�
        /// </summary>
        public FeederStatusLog CreateNewFeederStatusLog()
        {
            return new FeederStatusLog();
        }

        public void AddFeederStatusLog(FeederStatusLog feederStatusLog)
        {
            this._helper.AddDomainObject(feederStatusLog);
        }

        public void UpdateFeederStatusLog(FeederStatusLog feederStatusLog)
        {
            this._helper.UpdateDomainObject(feederStatusLog);
        }

        public void DeleteFeederStatusLog(FeederStatusLog feederStatusLog)
        {
            this._helper.DeleteDomainObject(feederStatusLog);
        }

        public void DeleteFeederStatusLog(FeederStatusLog[] feederStatusLog)
        {
            this._helper.DeleteDomainObject(feederStatusLog);
        }

        public object GetFeederStatusLog(string feederCode, string seq)
        {
            return this.DataProvider.CustomSearch(typeof(FeederStatusLog), new object[] { feederCode, seq });
        }

        /// <summary>
        /// ** ��������:	��ѯFeederStatusLog��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="feederCode">FeederCode��ģ����ѯ</param>
        /// <param name="seq">Seq��ģ����ѯ</param>
        /// <returns> FeederStatusLog���ܼ�¼��</returns>
        public int QueryFeederStatusLogCount(string feederCode, string seq)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLFEEDERSTATUSLOG where 1=1 and FEEDERCODE like '{0}%'  and SEQ like '{1}%' ", feederCode, seq)));
        }
        public int QueryFeederStatusLogCount(string productCode, string moCode, string ssCode, string feederSpecCode, string feederCode)
        {
            string strSql = "SELECT COUNT(*) FROM tblFeederStatusLog WHERE 1=1 ";
            if (moCode != string.Empty)
                strSql += " AND MOCode LIKE '" + moCode + "%' ";
            if (ssCode != string.Empty)
                strSql += " AND SSCode LIKE '" + ssCode + "%' ";
            if (feederSpecCode != string.Empty)
                strSql += " AND FeederSpecCode LIKE '" + feederSpecCode + "%' ";
            if (feederCode != string.Empty)
                strSql += " AND FeederCode LIKE '" + feederCode + "%' ";
            if (productCode != string.Empty && moCode == string.Empty)
                strSql += " AND MOCode IN (SELECT MOCode FROM tblMO WHERE ItemCode='" + productCode + "' " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ") ";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯFeederStatusLog
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="feederCode">FeederCode��ģ����ѯ</param>
        /// <param name="seq">Seq��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> FeederStatusLog����</returns>
        public object[] QueryFeederStatusLog(string feederCode, string seq, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(FeederStatusLog), new PagerCondition(string.Format("select {0} from TBLFEEDERSTATUSLOG where 1=1 and FEEDERCODE like '{1}%'  and SEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(FeederStatusLog)), feederCode, seq), "FEEDERCODE,SEQ", inclusive, exclusive));
        }
        public object[] QueryFeederStatusLog(string productCode, string moCode, string ssCode, string feederSpecCode, string feederCode, int inclusive, int exclusive)
        {
            string strSql = string.Format("SELECT {0} FROM tblFeederStatusLog WHERE 1=1 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(FeederStatusLog)));
            if (moCode != string.Empty)
                strSql += " AND MOCode LIKE '" + moCode + "%' ";
            if (ssCode != string.Empty)
                strSql += " AND SSCode LIKE '" + ssCode + "%' ";
            if (feederSpecCode != string.Empty)
                strSql += " AND FeederSpecCode LIKE '" + feederSpecCode + "%' ";
            if (feederCode != string.Empty)
                strSql += " AND FeederCode LIKE '" + feederCode + "%' ";
            if (productCode != string.Empty && moCode == string.Empty)
                strSql += " AND MOCode IN (SELECT MOCode FROM tblMO WHERE ItemCode='" + productCode + "' " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + ") ";
            return this.DataProvider.CustomQuery(typeof(FeederStatusLog), new PagerCondition(strSql, "FeederCode,Seq", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�FeederStatusLog
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>FeederStatusLog���ܼ�¼��</returns>
        public object[] GetAllFeederStatusLog()
        {
            return this.DataProvider.CustomQuery(typeof(FeederStatusLog), new SQLCondition(string.Format("select {0} from TBLFEEDERSTATUSLOG order by FEEDERCODE,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(FeederStatusLog)))));
        }


        #endregion

        #region MachineFeeder
        /// <summary>
        /// ��̨���ϼ�¼
        /// </summary>
        public MachineFeeder CreateNewMachineFeeder()
        {
            return new MachineFeeder();
        }

        public void AddMachineFeeder(MachineFeeder machineFeeder)
        {
            this._helper.AddDomainObject(machineFeeder);
        }

        public void UpdateMachineFeeder(MachineFeeder machineFeeder)
        {
            this._helper.UpdateDomainObject(machineFeeder);
        }

        public void DeleteMachineFeeder(MachineFeeder machineFeeder)
        {
            this._helper.DeleteDomainObject(machineFeeder);
        }

        public void DeleteMachineFeeder(MachineFeeder[] machineFeeder)
        {
            this._helper.DeleteDomainObject(machineFeeder);
        }

        public object GetMachineFeeder(string mOCode, string machineCode, string machineStationCode)
        {
            return this.DataProvider.CustomSearch(typeof(MachineFeeder), new object[] { mOCode, machineCode, machineStationCode });
        }

        /// <summary>
        /// ** ��������:	��ѯMachineFeeder��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-27 9:16:41
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="machineCode">MachineCode��ģ����ѯ</param>
        /// <param name="machineStationCode">MachineStationCode��ģ����ѯ</param>
        /// <returns> MachineFeeder���ܼ�¼��</returns>
        public int QueryMachineFeederCount(string mOCode, string machineCode, string machineStationCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMACHINEFEEDER where 1=1 and MOCODE like '{0}%'  and MACHINECODE like '{1}%'  and MACHINESTATIONCODE like '{2}%' ", mOCode, machineCode, machineStationCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯMachineFeeder
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-27 9:16:41
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="machineCode">MachineCode��ģ����ѯ</param>
        /// <param name="machineStationCode">MachineStationCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> MachineFeeder����</returns>
        public object[] QueryMachineFeeder(string mOCode, string machineCode, string machineStationCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(MachineFeeder), new PagerCondition(string.Format("select {0} from TBLMACHINEFEEDER where 1=1 and MOCODE like '{1}%'  and MACHINECODE like '{2}%'  and MACHINESTATIONCODE like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MachineFeeder)), mOCode, machineCode, machineStationCode), "MOCODE,MACHINECODE,MACHINESTATIONCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�MachineFeeder
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-27 9:16:41
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>MachineFeeder���ܼ�¼��</returns>
        public object[] GetAllMachineFeeder()
        {
            return this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(string.Format("select {0} from TBLMACHINEFEEDER order by MOCODE,MACHINECODE,MACHINESTATIONCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MachineFeeder)))));
        }

        /// <summary>
        /// ��ѯ��������Ч״̬�Ĺ���
        /// </summary>
        /// <param name="stepSequenceCode"></param>
        /// <returns></returns>
        public object[] QueryEnabledMOInLine(string stepSequenceCode)
        {
            string strSql = "SELECT DISTINCT SSCode,MOCode FROM tblMachineFeeder WHERE CheckResult='1' AND Enabled='1' AND SSCode='" + stepSequenceCode + "'";
            return this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
        }


        public int QueryMOMaterialRptCount(string moCode, string stepSequenceCode, string materialCode, string machineStationiCode)
        {
            string strSql = "SELECT ProductCode,MOCode,SSCode,MachineCode,MachineStationCode,MaterialCode FROM tblMachineFeederLog WHERE CheckResult='1' ";
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (stepSequenceCode != string.Empty)
                strSql += " AND SSCode LIKE '" + stepSequenceCode + "%' ";
            if (materialCode != string.Empty)
                strSql += " AND MaterialCode LIKE '" + materialCode + "%' ";
            if (machineStationiCode != string.Empty)
                strSql += " AND MachineStationCode LIKE '" + machineStationiCode + "%' ";
            strSql += " GROUP BY ProductCode,MOCode,SSCode,MachineCode,MachineStationCode,MaterialCode ";
            strSql = "SELECT COUNT(*) FROM (" + strSql + ")";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }
        public object[] QueryMOMaterialRpt(string moCode, string stepSequenceCode, string materialCode, string machineStationiCode, int inclusive, int exclusive)
        {
            string strSql = "SELECT ProductCode,MOCode,SSCode,MachineCode,MachineStationCode,MaterialCode,SUM(ReelUsedQty) ReelUsedQty,SUM(ReelChkDiffQty) ReelChkDiffQty FROM tblMachineFeederLog WHERE CheckResult='1' ";
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (stepSequenceCode != string.Empty)
                strSql += " AND SSCode LIKE '" + stepSequenceCode + "%' ";
            if (materialCode != string.Empty)
                strSql += " AND MaterialCode LIKE '" + materialCode + "%' ";
            if (machineStationiCode != string.Empty)
                strSql += " AND MachineStationCode LIKE '" + machineStationiCode + "%' ";
            strSql += " GROUP BY ProductCode,MOCode,SSCode,MachineCode,MachineStationCode,MaterialCode ";
            strSql = "SELECT ProductCode,MOCode,SSCode,MachineCode,MachineStationCode,MaterialCode,ReelUsedQty,ReelChkDiffQty FROM (" + strSql + ")";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeederLog), new PagerCondition(strSql, "MOCode,MaterialCode,MachineStationCode", inclusive, exclusive));
            if (objs == null || objs.Length == 0)
                return null;
            SMTRptMOMaterial[] rpts = new SMTRptMOMaterial[objs.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                MachineFeederLog mf = (MachineFeederLog)objs[i];
                SMTRptMOMaterial rpt = new SMTRptMOMaterial();
                rpt.ProductCode = mf.ProductCode;
                rpt.MOCode = mf.MOCode;
                rpt.StepSequenceCode = mf.StepSequenceCode;
                rpt.MachineCode = mf.MachineCode;
                rpt.MachineStationCode = mf.MachineStationCode;
                rpt.MaterialCode = mf.MaterialCode;
                // ��������
                rpt.LogicalUsedQty = mf.ReelUsedQty;
                // ʵ������
                rpt.ActualUsedQty = mf.ReelUsedQty + mf.ReelCheckDiffQty;
                // ��ѯ�豸��������
                strSql = "SELECT * FROM tblSMTMachineDiscard WHERE MOCode='" + mf.MOCode + "' AND SSCode='" + mf.StepSequenceCode + "' AND MaterialCode='" + mf.MaterialCode + "' AND MachineStationCode='" + mf.MachineStationCode + "'";
                object[] objsTmp = this.DataProvider.CustomQuery(typeof(SMTMachineDiscard), new SQLCondition(strSql));
                if (objsTmp != null && objsTmp.Length > 0)
                {
                    // �豸��������
                    SMTMachineDiscard discard = (SMTMachineDiscard)objsTmp[0];
                    rpt.MachineDiscardQty = discard.RejectParts + discard.NoPickup + discard.ErrorParts + discard.DislodgedParts;
                    // �豸������
                    if (discard.PickupCount != 0)
                        rpt.MachineDiscardRate = rpt.MachineDiscardQty / discard.PickupCount;
                }
                // ��Ϊ��������
                rpt.ManualDiscardQty = rpt.ActualUsedQty - rpt.LogicalUsedQty - rpt.MachineDiscardQty;
                // ��Ϊ������
                if (rpt.ActualUsedQty != 0)
                    rpt.ManualDiscardRate = rpt.ManualDiscardQty / rpt.ActualUsedQty;
                rpts[i] = rpt;
            }
            return rpts;
        }

        public void UpdateMachineFeeder(string itemcode, string sscode)
        {
            string strSqlMO = "UPDATE tblMachineFeeder SET Enabled='0' WHERE ProductCode='" + itemcode + "' AND SSCode='" + sscode + "' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSqlMO));
        }
        #endregion

        #region MachineFeederLog
        /// <summary>
        /// ��̨���ϼ�¼
        /// </summary>
        public MachineFeederLog CreateNewMachineFeederLog()
        {
            return new MachineFeederLog();
        }

        public void AddMachineFeederLog(MachineFeederLog machineFeederLog)
        {
            this._helper.AddDomainObject(machineFeederLog);
        }

        public void UpdateMachineFeederLog(MachineFeederLog machineFeederLog)
        {
            this._helper.UpdateDomainObject(machineFeederLog);
        }

        public void DeleteMachineFeederLog(MachineFeederLog machineFeederLog)
        {
            this._helper.DeleteDomainObject(machineFeederLog);
        }

        public void DeleteMachineFeederLog(MachineFeederLog[] machineFeederLog)
        {
            this._helper.DeleteDomainObject(machineFeederLog);
        }

        public object GetMachineFeederLog(decimal logNo)
        {
            return this.DataProvider.CustomSearch(typeof(MachineFeederLog), new object[] { logNo });
        }

        public int QueryMachineFeederLogCount(string productCode, string moCode, string materialCode, string lineCode, string machineCode, string machineStationCode, string userCode, string lotNo, string reelNo, string result, string beginDate, string endDate)
        {
            string strSql = "SELECT COUNT(*) FROM TBLMACHINEFEEDERLOG WHERE 1=1 ";
            if (productCode != string.Empty)
                strSql += " AND ProductCode IN ('" + productCode.Replace(",", "','") + "') ";
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (materialCode != string.Empty)
                strSql += " AND MaterialCode LIKE '" + materialCode + "%' ";
            if (lineCode != string.Empty)
                strSql += " AND SSCODE LIKE '" + lineCode + "%' ";
            if (machineCode != string.Empty)
                strSql += " AND MachineCode LIKE '" + machineCode + "%' ";
            if (userCode != string.Empty)
                strSql += " AND LoadUser LIKE '" + userCode + "%' ";
            if (lotNo != string.Empty)
                strSql += " AND LotNo LIKE '" + lotNo + "%' ";
            strSql += " AND CheckResult LIKE '" + result + "%' ";
            if (beginDate != string.Empty && beginDate != "0")
                strSql += " AND LoadDate>=" + beginDate + " ";
            if (endDate != string.Empty && endDate != "0")
                strSql += " AND LoadDate<=" + endDate + " ";
            if (machineStationCode != string.Empty)
                strSql += " AND MachineStationCode LIKE '" + machineStationCode + "%' ";
            if (reelNo != string.Empty)
                strSql += " AND ReelNo LIKE '" + reelNo + "%' ";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
            //return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLMACHINEFEEDERLOG where 1=1 and MOCODE like '{0}%' AND MACHINECODE LIKE '{1}%' AND MACHINESTATIONCODE LIKE '{2}%' AND CHECKRESULT LIKE '{3}%' " , moCode, machineCode, machineStationCode, result)));
        }

        public object[] QueryMachineFeederLog(string productCode, string moCode, string materialCode, string lineCode, string machineCode, string machineStationCode, string userCode, string lotNo, string reelNo, string result, string beginDate, string endDate, int inclusive, int exclusive)
        {
            //return this.DataProvider.CustomQuery(typeof(MachineFeederLog), new PagerCondition(string.Format("select {0} from TBLMACHINEFEEDERLOG where 1=1 and MOCODE like '{1}%' AND MACHINECODE LIKE '{2}%' AND MACHINESTATIONCODE LIKE '{3}%' AND CHECKRESULT LIKE '{4}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MachineFeederLog)) , moCode, machineCode, machineStationCode, result), "MOCODE,MACHINECODE,MACHINESTATIONCODE,LOGNO DESC", inclusive, exclusive));
            string strSql = string.Format("SELECT {0} FROM TBLMACHINEFEEDERLOG WHERE 1=1 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MachineFeederLog)));
            if (productCode != string.Empty)
                strSql += " AND ProductCode IN ('" + productCode.Replace(",", "','") + "') ";
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (materialCode != string.Empty)
                strSql += " AND MaterialCode LIKE '" + materialCode + "%' ";
            if (lineCode != string.Empty)
                strSql += " AND SSCODE LIKE '" + lineCode + "%' ";
            if (machineCode != string.Empty)
                strSql += " AND MachineCode LIKE '" + machineCode + "%' ";
            if (userCode != string.Empty)
                strSql += " AND LoadUser LIKE '" + userCode + "%' ";
            if (lotNo != string.Empty)
                strSql += " AND LotNo LIKE '" + lotNo + "%' ";
            strSql += " AND CheckResult LIKE '" + result + "%' ";
            if (beginDate != string.Empty && beginDate != "0")
                strSql += " AND LoadDate>=" + beginDate + " ";
            if (endDate != string.Empty && endDate != "0")
                strSql += " AND LoadDate<=" + endDate + " ";
            if (machineStationCode != string.Empty)
                strSql += " AND MachineStationCode LIKE '" + machineStationCode + "%' ";
            if (reelNo != string.Empty)
                strSql += " AND ReelNo LIKE '" + reelNo + "%' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeederLog), new PagerCondition(strSql, "LOGNO DESC", inclusive, exclusive));
            return objs;
        }

        public object[] GetAllMachineFeederLog()
        {
            return this.DataProvider.CustomQuery(typeof(MachineFeederLog), new SQLCondition(string.Format("select {0} from TBLMACHINEFEEDERLOG order by LOGNO", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MachineFeederLog)))));
        }


        #endregion

        #region Reel
        /// <summary>
        /// ϵͳ�û�
        /// </summary>
        public Reel CreateNewReel()
        {
            return new Reel();
        }

        public void AddReel(Reel reel)
        {
            if (reel.IsSpecial == string.Empty)
                reel.IsSpecial = FormatHelper.BooleanToString(false);
            if (reel.UsedFlag == string.Empty)
                reel.UsedFlag = FormatHelper.BooleanToString(false);
            this._helper.AddDomainObject(reel);
        }

        public void UpdateReel(Reel reel)
        {
            if (reel.IsSpecial == string.Empty)
                reel.IsSpecial = FormatHelper.BooleanToString(false);
            if (reel.UsedFlag == string.Empty)
                reel.UsedFlag = FormatHelper.BooleanToString(false);
            this._helper.UpdateDomainObject(reel);
        }

        public void DeleteReel(Reel reel)
        {
            if (reel.UsedQty > 0)
                throw new Exception("$Reel_Used_Already");
            if (FormatHelper.StringToBoolean(reel.UsedFlag) == true)
            {
                throw new Exception("$Reel_Used_Already");
            }
            /*
            string strSql = "SELECT COUNT(ReelNo) FROM tblMachineFeeder WHERE ReelNo='" + reel.ReelNo + "'";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
                throw new Exception("$Reel_Used_Already");
            */
            this._helper.DeleteDomainObject(reel);
        }

        public void DeleteReel(Reel[] reel)
        {
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();
            ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.BeginTransaction();
            try
            {
                for (int i = 0; i < reel.Length; i++)
                {
                    DeleteReel(reel[i]);
                }
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CommitTransaction();
            }
            catch (Exception e)
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.RollbackTransaction();
                throw new Exception(e.Message);
            }
            finally
            {
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
            }
        }

        public object GetReel(string reelNo)
        {
            return this.DataProvider.CustomSearch(typeof(Reel), new object[] { reelNo });
        }

        /// <summary>
        /// ** ��������:	��ѯReel��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reelNo">ReelNo��ģ����ѯ</param>
        /// <returns> Reel���ܼ�¼��</returns>
        public int QueryReelCount(string reelNo)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREEL where 1=1 and REELNO like '{0}%' ", reelNo)));
        }
        public int QueryReelCount(string moCode, string stepSequenceCode, string reelNo, string partNo)
        {
            string strSql = "select COUNT(*) from TBLREEL where 1=1 ";
            if (reelNo != string.Empty)
                strSql += " AND ReelNo LIKE '" + reelNo + "%' ";
            if (partNo != string.Empty)
                strSql += " AND PartNo LIKE '" + partNo + "%' ";
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (stepSequenceCode != string.Empty)
                strSql += " AND SSCode LIKE '" + stepSequenceCode + "%' ";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }
        public int QueryReelByMOCount(string moCode, string stepSequenceCode, string reelNo, string partNo)
        {
            string strSql = "SELECT COUNT(*) FROM tblReel WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' ";
            if (reelNo != string.Empty)
                strSql += " AND ReelNo LIKE '" + reelNo + "%' ";
            if (partNo != string.Empty)
                strSql += " AND PartNo LIKE '" + partNo + "%' ";
            strSql += " ORDER BY ReelNo";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReel
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reelNo">ReelNo��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Reel����</returns>
        public object[] QueryReel(string reelNo, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(Reel), new PagerCondition(string.Format("select {0} from TBLREEL where 1=1 and REELNO like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reel)), reelNo), "REELNO", inclusive, exclusive));
        }
        public object[] QueryReel(string moCode, string stepSequenceCode, string reelNo, string partNo, int inclusive, int exclusive)
        {
            string strSql = string.Format("select {0} from TBLREEL where 1=1 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reel)));
            if (reelNo != string.Empty)
                strSql += " AND ReelNo LIKE '" + reelNo + "%' ";
            if (partNo != string.Empty)
                strSql += " AND PartNo LIKE '" + partNo + "%' ";
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (stepSequenceCode != string.Empty)
                strSql += " AND SSCode LIKE '" + stepSequenceCode + "%' ";
            return this.DataProvider.CustomQuery(typeof(Reel), new PagerCondition(strSql, "REELNO", inclusive, exclusive));
        }
        public object[] QueryReelByMO(string moCode, string stepSequenceCode, string reelNo, string partNo)
        {
            string strSql = "SELECT * FROM tblReel WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' ";
            if (reelNo != string.Empty)
                strSql += " AND ReelNo LIKE '" + reelNo + "%' ";
            if (partNo != string.Empty)
                strSql += " AND PartNo LIKE '" + partNo + "%' ";
            strSql += " ORDER BY ReelNo";
            return this.DataProvider.CustomQuery(typeof(Reel), new SQLCondition(strSql));
        }
        public object[] QueryUsedReelByMO(string moCode, string stepSequenceCode)
        {
            string strSql = "SELECT * UsedQty FROM tblReel WHERE MOCode='" + moCode + "' AND SSCode='" + stepSequenceCode + "' ";
            return this.DataProvider.CustomQuery(typeof(Reel), new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	������е�Reel
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 10:45:16
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>Reel���ܼ�¼��</returns>
        public object[] GetAllReel()
        {
            return this.DataProvider.CustomQuery(typeof(Reel), new SQLCondition(string.Format("select {0} from TBLREEL order by REELNO", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Reel)))));
        }

        /// <summary>
        /// �Ͼ����õ�����
        /// </summary>
        public Messages UpdateReelToMO(Reel reel, bool isnew)
        {
            Messages msg = new Messages();
            if (reel.Qty - reel.UsedQty <= 0)
            {
                msg.Add(new Message(MessageType.Error, "$SMT_Prepare_Reel_LeftQty_Too_Low"));
                return msg;
            }
            string strSql = "SELECT COUNT(*) FROM tblReel WHERE ReelNo='" + reel.ReelNo + "' AND UsedFlag='1' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
            {
                msg.Add(new Message(MessageType.Error, "$SMT_Prepare_Reel_Used_Already"));
                return msg;
            }
            if (isnew == true)
                this.AddReel(reel);
            else
                this.UpdateReel(reel);
            // ��ӵ�Log
            if (FormatHelper.StringToBoolean(reel.UsedFlag) == true)
            {
                decimal dId = 1;
                /*
                strSql = "SELECT MAX(CheckID) CheckID FROM tblReelChkLog";
                object[] objsTmp = this.DataProvider.CustomQuery(typeof(ReelCheckedLog), new SQLCondition(strSql));
                if (objsTmp != null && objsTmp.Length > 0)
                {
                    dId = ((ReelCheckedLog)objsTmp[0]).CheckID + 1;
                }
                */
                strSql = "SELECT SEQ_SMTLOGSEQUENCE.NextVal CheckID FROM dual";
                object[] objsTmp = this.DataProvider.CustomQuery(typeof(ReelCheckedLog), new SQLCondition(strSql));
                if (objsTmp != null && objsTmp.Length > 0)
                {
                    dId = ((ReelCheckedLog)objsTmp[0]).CheckID;
                }

                ReelCheckedLog log = new ReelCheckedLog();
                log.CheckID = dId;
                log.MOCode = reel.MOCode;
                log.StepSequenceCode = reel.StepSequenceCode;
                log.ReelNo = reel.ReelNo;
                log.MaterialCode = reel.PartNo;
                log.ReelQty = reel.Qty;
                log.ReelCurrentQty = reel.Qty - reel.UsedQty;
                log.IsSpecial = reel.IsSpecial;
                log.IsChecked = FormatHelper.BooleanToString(false);
                log.Memo = reel.Memo;
                log.GetOutUser = reel.MaintainUser;
                log.GetOutDate = reel.MaintainDate;
                log.GetOutTime = reel.MaintainTime;
                log.IsChecked = FormatHelper.BooleanToString(false);
                this.AddReelCheckedLog(log);
            }
            return msg;
        }

        /// <summary>
        /// �Ͼ����õ�����
        /// </summary>
        public Messages DeleteReelFromMO(string reelNo, string userCode)
        {
            Messages msg = new Messages();
            Reel reel = (Reel)this.GetReel(reelNo);
            if (reel == null)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Not_Exist"));
                return msg;
            }
            // ��ѯ�Ͼ��Ƿ�����
            string strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE ReelNo='" + reelNo + "' AND CheckResult='1' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) > 0)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Loaded_In_Machine"));
                return msg;
            }
            reel.UsedFlag = FormatHelper.BooleanToString(false);
            reel.MOCode = string.Empty;
            reel.StepSequenceCode = string.Empty;
            reel.IsSpecial = FormatHelper.BooleanToString(false);
            reel.Memo = string.Empty;
            reel.MaintainUser = userCode;
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            reel.MaintainDate = FormatHelper.TODateInt(dtNow);
            reel.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.UpdateReel(reel);
            // ����Log
            strSql = "SELECT * FROM tblReelChkLog WHERE CheckID=(SELECT MAX(CheckID) FROM tblReelChkLog WHERE ReelNo='" + reelNo + "')";
            object[] objs = this.DataProvider.CustomQuery(typeof(ReelCheckedLog), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                ReelCheckedLog log = (ReelCheckedLog)objs[0];
                log.IsChecked = FormatHelper.BooleanToString(false);
                log.ReelLeftQty = reel.Qty - reel.UsedQty;
                log.ReelActualQty = log.ReelLeftQty;
                log.CheckUser = userCode;
                log.CheckDate = FormatHelper.TODateInt(dtNow);
                log.CheckTime = FormatHelper.TOTimeInt(dtNow);
                log.MaintainUser = userCode;
                log.MaintainDate = log.CheckDate;
                log.MaintainTime = log.CheckTime;
                this.UpdateReelCheckedLog(log);
            }
            return msg;
        }

        #endregion

        #region ReelCheckedLog
        /// <summary>
        /// 
        /// </summary>
        public ReelCheckedLog CreateNewReelCheckedLog()
        {
            return new ReelCheckedLog();
        }

        public void AddReelCheckedLog(ReelCheckedLog reelCheckedLog)
        {
            this._helper.AddDomainObject(reelCheckedLog);
        }

        public void UpdateReelCheckedLog(ReelCheckedLog reelCheckedLog)
        {
            this._helper.UpdateDomainObject(reelCheckedLog);
        }

        public void DeleteReelCheckedLog(ReelCheckedLog reelCheckedLog)
        {
            this._helper.DeleteDomainObject(reelCheckedLog);
        }

        public void DeleteReelCheckedLog(ReelCheckedLog[] reelCheckedLog)
        {
            this._helper.DeleteDomainObject(reelCheckedLog);
        }

        public object GetReelCheckedLog(decimal checkID)
        {
            return this.DataProvider.CustomSearch(typeof(ReelCheckedLog), new object[] { checkID });
        }

        /// <summary>
        /// ** ��������:	��ѯReelCheckedLog��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-2 16:17:11
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="checkID">CheckID��ģ����ѯ</param>
        /// <returns> ReelCheckedLog���ܼ�¼��</returns>
        public int QueryReelCheckedLogCount(decimal checkID)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREELCHKLOG where 1=1 and CheckID like '{0}%' ", checkID)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReelCheckedLog
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-2 16:17:11
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="checkID">CheckID��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ReelCheckedLog����</returns>
        public object[] QueryReelCheckedLog(decimal checkID, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReelCheckedLog), new PagerCondition(string.Format("select {0} from TBLREELCHKLOG where 1=1 and CheckID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReelCheckedLog)), checkID), "CheckID", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�ReelCheckedLog
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-2 16:17:11
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ReelCheckedLog���ܼ�¼��</returns>
        public object[] GetAllReelCheckedLog()
        {
            return this.DataProvider.CustomQuery(typeof(ReelCheckedLog), new SQLCondition(string.Format("select {0} from TBLREELCHKLOG order by CheckID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReelCheckedLog)))));
        }

        /// <summary>
        /// ����/����
        /// </summary>
        public Messages CheckReel(string reelNo, int actualQty, string userCode)
        {
            Messages msg = new Messages();
            Reel reel = (Reel)this.GetReel(reelNo);
            if (reel == null)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Not_Exist"));
                return msg;
            }
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            // ����Log
            string strSql = "SELECT * FROM tblReelChkLog WHERE CheckID=(SELECT MAX(CheckID) FROM tblReelChkLog WHERE ReelNo='" + reelNo + "')";
            object[] objsLog = this.DataProvider.CustomQuery(typeof(ReelCheckedLog), new SQLCondition(strSql));
            if (objsLog != null && objsLog.Length > 0)
            {
                ReelCheckedLog log = (ReelCheckedLog)objsLog[0];
                log.ReelLeftQty = reel.Qty - reel.UsedQty;
                if (actualQty != int.MinValue)
                {
                    log.ReelActualQty = actualQty;
                    log.IsChecked = FormatHelper.BooleanToString(true);
                }
                else
                {
                    log.ReelActualQty = log.ReelLeftQty;
                    log.IsChecked = FormatHelper.BooleanToString(false);
                }
                log.CheckUser = userCode;
                log.CheckDate = FormatHelper.TODateInt(dtNow);
                log.CheckTime = FormatHelper.TOTimeInt(dtNow);
                log.MaintainUser = userCode;
                log.MaintainDate = log.CheckDate;
                log.MaintainTime = log.CheckTime;
                this.UpdateReelCheckedLog(log);
            }
            // ���浽MachineFeederLog
            if (actualQty != int.MinValue)
            {
                strSql = "UPDATE tblMachineFeederLog SET ReelChkDiffQty=" + ((reel.Qty - reel.UsedQty) - actualQty).ToString() + " WHERE LogNo=(SELECT MAX(LogNo) FROM tblMachineFeederLog WHERE CheckResult='1' AND ReelNo='" + reel.ReelNo + "' AND MOCode='" + reel.MOCode + "' )";
                this.DataProvider.CustomExecute(new SQLCondition(strSql));
            }
            // ����Reel
            if (actualQty != int.MinValue)
            {
                reel.CheckDiffQty += (reel.Qty - reel.UsedQty) - actualQty;
                reel.UsedQty = reel.Qty - actualQty;
            }
            reel.UsedFlag = FormatHelper.BooleanToString(false);
            reel.MOCode = string.Empty;
            reel.StepSequenceCode = string.Empty;
            reel.IsSpecial = FormatHelper.BooleanToString(false);
            reel.Memo = string.Empty;
            reel.MaintainUser = userCode;
            reel.MaintainDate = FormatHelper.TODateInt(dtNow);
            reel.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.UpdateReel(reel);
            return msg;
        }

        #endregion

        #region ReelQty
        /// <summary>
        /// Feeder����
        /// </summary>
        public ReelQty CreateNewReelQty()
        {
            return new ReelQty();
        }

        public void AddReelQty(ReelQty reelQty)
        {
            this._helper.AddDomainObject(reelQty);
        }

        public void UpdateReelQty(ReelQty reelQty)
        {
            this._helper.UpdateDomainObject(reelQty);
        }

        public void DeleteReelQty(ReelQty reelQty)
        {
            this._helper.DeleteDomainObject(reelQty);
        }

        public void DeleteReelQty(ReelQty[] reelQty)
        {
            this._helper.DeleteDomainObject(reelQty);
        }

        public object GetReelQty(string reelNo, string mOCode)
        {
            return this.DataProvider.CustomSearch(typeof(ReelQty), new object[] { reelNo, mOCode });
        }

        /// <summary>
        /// ** ��������:	��ѯReelQty��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-27 9:16:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reelNo">ReelNo��ģ����ѯ</param>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <returns> ReelQty���ܼ�¼��</returns>
        public int QueryReelQtyCount(string reelNo, string mOCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREELQTY where 1=1 and REELNO like '{0}%'  and MOCODE like '{1}%' ", reelNo, mOCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReelQty
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-27 9:16:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="reelNo">ReelNo��ģ����ѯ</param>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ReelQty����</returns>
        public object[] QueryReelQty(string reelNo, string mOCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReelQty), new PagerCondition(string.Format("select {0} from TBLREELQTY where 1=1 and REELNO like '{1}%'  and MOCODE like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReelQty)), reelNo, mOCode), "REELNO,MOCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�ReelQty
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-27 9:16:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ReelQty���ܼ�¼��</returns>
        public object[] GetAllReelQty()
        {
            return this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(string.Format("select {0} from TBLREELQTY order by REELNO,MOCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReelQty)))));
        }


        #endregion

        #region ReelValidity
        /// <summary>
        /// 
        /// </summary>
        public ReelValidity CreateNewReelValidity()
        {
            return new ReelValidity();
        }

        public void AddReelValidity(ReelValidity reelValidity)
        {
            this._helper.AddDomainObject(reelValidity);
        }

        public void UpdateReelValidity(ReelValidity reelValidity)
        {
            this._helper.UpdateDomainObject(reelValidity);
        }

        public void DeleteReelValidity(ReelValidity reelValidity)
        {
            this._helper.DeleteDomainObject(reelValidity);
        }

        public void DeleteReelValidity(ReelValidity[] reelValidity)
        {
            this._helper.DeleteDomainObject(reelValidity);
        }

        public object GetReelValidity(string materialPrefix)
        {
            return this.DataProvider.CustomSearch(typeof(ReelValidity), new object[] { materialPrefix });
        }

        /// <summary>
        /// ** ��������:	��ѯReelValidity��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-1 10:28:32
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="materialPrefix">MaterialPrefix��ģ����ѯ</param>
        /// <returns> ReelValidity���ܼ�¼��</returns>
        public int QueryReelValidityCount(string materialPrefix)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLREELVALIDITY where 1=1 and MATERIALPREFIX like '{0}%' ", materialPrefix)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯReelValidity
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-1 10:28:32
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="materialPrefix">MaterialPrefix��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> ReelValidity����</returns>
        public object[] QueryReelValidity(string materialPrefix, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(ReelValidity), new PagerCondition(string.Format("select {0} from TBLREELVALIDITY where 1=1 and MATERIALPREFIX like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReelValidity)), materialPrefix), "MATERIALPREFIX", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�ReelValidity
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-1 10:28:32
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>ReelValidity���ܼ�¼��</returns>
        public object[] GetAllReelValidity()
        {
            return this.DataProvider.CustomQuery(typeof(ReelValidity), new SQLCondition(string.Format("select {0} from TBLREELVALIDITY order by MATERIALPREFIX", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ReelValidity)))));
        }

        /// <summary>
        /// ����Ͼ���Ч��
        /// </summary>
        /// <param name="materialCode"></param>
        /// <param name="dateCode"></param>
        /// <returns></returns>
        public Messages CheckReelValidity(string materialCode, string dateCode)
        {
            Messages msg = new Messages();
            if (dateCode == string.Empty)
            {
                msg.Add(new Message(MessageType.Error, "$SMT_Prepare_Reel_Please_Input_Date"));
                return msg;
            }
            DateTime dtDate = DateTime.MaxValue;
            // TODO ���ڸ�ʽ���ܻ�ı�
            if (dateCode.Length == 8)
            {
                dtDate = Convert.ToDateTime(FormatHelper.ToDateString(int.Parse(dateCode)));
            }
            // �����Ч��
            if (dtDate != DateTime.MaxValue)
            {
                ReelValidity validity = GetReelValidityByMaterial(materialCode);
                if (validity == null)
                {
                    msg.Add(new Message(MessageType.Error, "$SMT_Prepare_Reel_No_ReelValidity"));
                    return msg;
                }
                string[] strValid = validity.ValidityMonth.ToString().Split('.');
                int iMonth = int.Parse(strValid[0]);
                int iDay = 0;
                if (strValid.Length > 1)
                    iDay = Convert.ToInt32(decimal.Parse("0." + strValid[1]) * 30);
                DateTime dtValid = dtDate.AddMonths(iMonth);
                dtValid = dtValid.AddDays(iDay);
                // ����
                if (dtValid < DateTime.Today)
                {
                    msg.Add(new Message(MessageType.Error, "$SMT_Prepare_Reel_Extend_Validity"));
                    return msg;
                }
            }
            return msg;
        }
        private ReelValidity GetReelValidityByMaterial(string materialCode)
        {
            string strSql = "SELECT * FROM ( ";
            strSql += "SELECT * FROM tblReelValidity WHERE SUBSTR('" + materialCode + "', 1, LENGTH(MaterialPrefix))=MaterialPrefix ";
            strSql += "ORDER BY LENGTH(MaterialPrefix) DESC ";
            strSql += ") WHERE RowNum=1";
            object[] objs = this.DataProvider.CustomQuery(typeof(ReelValidity), new SQLCondition(strSql));
            if (objs == null || objs.Length == 0)
                return null;
            else
                return (ReelValidity)objs[0];
        }

        #endregion

        #region SMTAlert
        /// <summary>
        /// SMTԤ��
        /// </summary>
        public SMTAlert CreateNewSMTAlert()
        {
            return new SMTAlert();
        }

        public void AddSMTAlert(SMTAlert sMTAlert)
        {
            this._helper.AddDomainObject(sMTAlert);
        }

        public void UpdateSMTAlert(SMTAlert sMTAlert)
        {
            this._helper.UpdateDomainObject(sMTAlert);
        }

        public void DeleteSMTAlert(SMTAlert sMTAlert)
        {
            this._helper.DeleteDomainObject(sMTAlert);
        }

        public void DeleteSMTAlert(SMTAlert[] sMTAlert)
        {
            this._helper.DeleteDomainObject(sMTAlert);
        }

        public object GetSMTAlert(decimal alertSeq)
        {
            return this.DataProvider.CustomSearch(typeof(SMTAlert), new object[] { alertSeq });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTAlert��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-27 9:16:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="alertSeq">AlertSeq��ģ����ѯ</param>
        /// <returns> SMTAlert���ܼ�¼��</returns>
        public int QuerySMTAlertCount(decimal alertSeq)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTALERT where 1=1 and ALERTSEQ like '{0}%' ", alertSeq)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTAlert
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-27 9:16:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="alertSeq">AlertSeq��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTAlert����</returns>
        public object[] QuerySMTAlert(decimal alertSeq, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTAlert), new PagerCondition(string.Format("select {0} from TBLSMTALERT where 1=1 and ALERTSEQ like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTAlert)), alertSeq), "ALERTSEQ", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTAlert
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-27 9:16:42
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTAlert���ܼ�¼��</returns>
        public object[] GetAllSMTAlert()
        {
            return this.DataProvider.CustomQuery(typeof(SMTAlert), new SQLCondition(string.Format("select {0} from TBLSMTALERT order by ALERTSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTAlert)))));
        }


        #endregion

        #region SMTCheckMaterial
        /// <summary>
        /// 
        /// </summary>
        public SMTCheckMaterial CreateNewSMTCheckMaterial()
        {
            return new SMTCheckMaterial();
        }

        public void AddSMTCheckMaterial(SMTCheckMaterial sMTCheckMaterial)
        {
            this._helper.AddDomainObject(sMTCheckMaterial);
        }

        public void UpdateSMTCheckMaterial(SMTCheckMaterial sMTCheckMaterial)
        {
            this._helper.UpdateDomainObject(sMTCheckMaterial);
        }

        public void DeleteSMTCheckMaterial(SMTCheckMaterial sMTCheckMaterial)
        {
            this._helper.DeleteDomainObject(sMTCheckMaterial);
        }

        public void DeleteSMTCheckMaterial(SMTCheckMaterial[] sMTCheckMaterial)
        {
            this._helper.DeleteDomainObject(sMTCheckMaterial);
        }

        public object GetSMTCheckMaterial(decimal checkID)
        {
            return this.DataProvider.CustomSearch(typeof(SMTCheckMaterial), new object[] { checkID });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTCheckMaterial��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-6-27 13:42:09
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="checkID">CheckID��ģ����ѯ</param>
        /// <returns> SMTCheckMaterial���ܼ�¼��</returns>
        public int QuerySMTCheckMaterialCount(decimal checkID)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from tblSMTCheckMaterial where 1=1 and CheckID like '{0}%' ", checkID)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTCheckMaterial
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-6-27 13:42:09
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="checkID">CheckID��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTCheckMaterial����</returns>
        public object[] QuerySMTCheckMaterial(decimal checkID, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTCheckMaterial), new PagerCondition(string.Format("select {0} from tblSMTCheckMaterial where 1=1 and CheckID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTCheckMaterial)), checkID), "CheckID", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTCheckMaterial
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-6-27 13:42:09
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTCheckMaterial���ܼ�¼��</returns>
        public object[] GetAllSMTCheckMaterial()
        {
            return this.DataProvider.CustomQuery(typeof(SMTCheckMaterial), new SQLCondition(string.Format("select {0} from tblSMTCheckMaterial order by CheckID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTCheckMaterial)))));
        }


        #endregion

        #region SMTCheckMaterialDetail
        /// <summary>
        /// SMT Feeder����
        /// </summary>
        public SMTCheckMaterialDetail CreateNewSMTCheckMaterialDetail()
        {
            return new SMTCheckMaterialDetail();
        }

        public void AddSMTCheckMaterialDetail(SMTCheckMaterialDetail sMTCheckMaterialDetail)
        {
            this._helper.AddDomainObject(sMTCheckMaterialDetail);
        }

        public void UpdateSMTCheckMaterialDetail(SMTCheckMaterialDetail sMTCheckMaterialDetail)
        {
            this._helper.UpdateDomainObject(sMTCheckMaterialDetail);
        }

        public void DeleteSMTCheckMaterialDetail(SMTCheckMaterialDetail sMTCheckMaterialDetail)
        {
            this._helper.DeleteDomainObject(sMTCheckMaterialDetail);
        }

        public void DeleteSMTCheckMaterialDetail(SMTCheckMaterialDetail[] sMTCheckMaterialDetail)
        {
            this._helper.DeleteDomainObject(sMTCheckMaterialDetail);
        }

        public object GetSMTCheckMaterialDetail(decimal checkID, decimal sequence)
        {
            return this.DataProvider.CustomSearch(typeof(SMTCheckMaterialDetail), new object[] { checkID, sequence });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTCheckMaterialDetail��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-6-27 13:42:09
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="checkID">CheckID��ģ����ѯ</param>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <returns> SMTCheckMaterialDetail���ܼ�¼��</returns>
        public int QuerySMTCheckMaterialDetailCount(decimal checkID, decimal sequence)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from tblSMTCheckMaterialDtl where 1=1 and CheckID like '{0}%'  and SEQ like '{1}%' ", checkID, sequence)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTCheckMaterialDetail
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-6-27 13:42:09
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="checkID">CheckID��ģ����ѯ</param>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTCheckMaterialDetail����</returns>
        public object[] QuerySMTCheckMaterialDetail(decimal checkID, decimal sequence, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTCheckMaterialDetail), new PagerCondition(string.Format("select {0} from tblSMTCheckMaterialDtl where 1=1 and CheckID like '{1}%'  and SEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTCheckMaterialDetail)), checkID, sequence), "CheckID,SEQ", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTCheckMaterialDetail
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-6-27 13:42:09
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTCheckMaterialDetail���ܼ�¼��</returns>
        public object[] GetAllSMTCheckMaterialDetail()
        {
            return this.DataProvider.CustomQuery(typeof(SMTCheckMaterialDetail), new SQLCondition(string.Format("select {0} from tblSMTCheckMaterialDtl order by CheckID,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTCheckMaterialDetail)))));
        }


        #endregion

        #region SMTFeederMaterial
        /// <summary>
        /// ϵͳ�û�
        /// </summary>
        public SMTFeederMaterial CreateNewSMTFeederMaterial()
        {
            return new SMTFeederMaterial();
        }

        public void AddSMTFeederMaterial(SMTFeederMaterial sMTFeederMaterial)
        {
            this._helper.AddDomainObject(sMTFeederMaterial);
        }

        public void UpdateSMTFeederMaterial(SMTFeederMaterial sMTFeederMaterial)
        {
            this._helper.UpdateDomainObject(sMTFeederMaterial);
        }

        public void DeleteSMTFeederMaterial(SMTFeederMaterial sMTFeederMaterial)
        {
            this._helper.DeleteDomainObject(sMTFeederMaterial);
        }

        public void DeleteSMTFeederMaterial(SMTFeederMaterial[] sMTFeederMaterial)
        {
            this._helper.DeleteDomainObject(sMTFeederMaterial);
        }

        public object GetSMTFeederMaterial(string productCode, string machineCode, string materialCode, string machineStationCode)
        {
            return this.DataProvider.CustomSearch(typeof(SMTFeederMaterial), new object[] { productCode, machineCode, materialCode, machineStationCode });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTFeederMaterial��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 17:41:13
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="productCode">ProductCode��ģ����ѯ</param>
        /// <param name="machineCode">MachineCode��ģ����ѯ</param>
        /// <param name="materialCode">MaterialCode��ģ����ѯ</param>
        /// <param name="machineStationCode">MachineStationCode��ģ����ѯ</param>
        /// <returns> SMTFeederMaterial���ܼ�¼��</returns>
        public int QuerySMTFeederMaterialCount(string productCode, string sscode, string machineCode, string materialCode, string machineStationCode)
        {
            string strSql = "select count(*) from TBLSMTFEEDERMATERIAL where 1=1 ";
            if (productCode != "")
                strSql += " and PRODUCTCODE='" + productCode + "' ";
            if (sscode != "")
                strSql += " and SSCODE='" + sscode + "' ";
            if (machineCode != "")
                strSql += " and MACHINECODE='" + machineCode + "' ";
            if (machineStationCode != "")
                strSql += " and MACHINESTATIONCODE='" + machineStationCode + "' ";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTFeederMaterial
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 17:41:14
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="productCode">ProductCode��ģ����ѯ</param>
        /// <param name="machineCode">MachineCode��ģ����ѯ</param>
        /// <param name="materialCode">MaterialCode��ģ����ѯ</param>
        /// <param name="machineStationCode">MachineStationCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTFeederMaterial����</returns>
        public object[] QuerySMTFeederMaterial(string productCode, string sscode, string machineCode, string materialCode, string machineStationCode, int inclusive, int exclusive)
        {
            string strSql = string.Format("select {0} from TBLSMTFEEDERMATERIAL where 1=1 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTFeederMaterial)));
            if (productCode != "")
                strSql += " and PRODUCTCODE='" + productCode + "' ";
            if (sscode != "")
                strSql += " and SSCODE='" + sscode + "' ";
            if (machineCode != "")
                strSql += " and MACHINECODE='" + machineCode + "' ";
            if (machineStationCode != "")
                strSql += " and MACHINESTATIONCODE='" + machineStationCode + "' ";
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new PagerCondition(strSql, "PRODUCTCODE,MACHINECODE,MATERIALCODE,MACHINESTATIONCODE", inclusive, exclusive));
        }

        public object[] QuerySMTFeederMaterialByProductCode(string productCode, string sscode, string machineCode)
        {
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(string.Format("select {0} from TBLSMTFEEDERMATERIAL where 1=1 and PRODUCTCODE='{1}' AND MachineCode='{2}' AND SSCode='{3}' ORDER BY MachineStationCode ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTFeederMaterial)), productCode, machineCode, sscode)));
        }

        public object[] QuerySMTFeederMaterialByProductCode(string productCode, string sscode, string machineCode, string tableGroup)
        {
            string strSql = string.Format("select {0} from TBLSMTFEEDERMATERIAL where 1=1 and PRODUCTCODE='{1}' AND MachineCode='{2}' AND SSCode='{3}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTFeederMaterial)), productCode, machineCode, sscode);
            if (tableGroup != string.Empty)
            {
                strSql += " AND TBLGRP='" + tableGroup + "' ";
            }
            strSql += " ORDER BY MachineStationCode ";
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
        }

        public object[] QuerySMTFeederMaterialByProductCode(string productCode, string sscode)
        {
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(string.Format("select {0} from TBLSMTFEEDERMATERIAL where 1=1 and PRODUCTCODE='{1}' and SSCODE='{2}' ORDER BY MachineStationCode ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTFeederMaterial)), productCode, sscode)));
        }
        public object[] QuerySMTFeederMaterialGroupByMaterial(string productCode, string sscode)
        {
            string strSql = "SELECT ProductCode,MaterialCode,SUM(Qty) Qty FROM tblSMTFeederMaterial WHERE ProductCode='" + productCode + "' AND SSCode='" + sscode + "' ";
            strSql += "GROUP BY ProductCode,MaterialCode ORDER BY MaterialCode";
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
        }

        public string[] GetSMTFeederMatrialTableGroup(string productCode, string sscode, string machineCode)
        {
            object[] objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(string.Format("select distinct TBLGRP from TBLSMTFEEDERMATERIAL where 1=1 and PRODUCTCODE='{0}' AND MachineCode='{1}' AND SSCode='{2}' ORDER BY TBLGRP ", productCode, machineCode, sscode)));
            if (objs != null)
            {
                string[] strRet = new string[objs.Length];
                for (int i = 0; i < objs.Length; i++)
                {
                    SMTFeederMaterial obj = (SMTFeederMaterial)objs[i];
                    strRet[i] = obj.TableGroup;
                }
                return strRet;
            }
            return null;
        }

        /// <summary>
        /// ** ��������:	������е�SMTFeederMaterial
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-5-25 17:41:14
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTFeederMaterial���ܼ�¼��</returns>
        public object[] GetAllSMTFeederMaterial()
        {
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(string.Format("select {0} from TBLSMTFEEDERMATERIAL order by PRODUCTCODE,MACHINECODE,MATERIALCODE,MACHINESTATIONCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTFeederMaterial)))));
        }

        public int ImportSMTFeederMaterial(object[] items, string userCode)
        {
            return ImportSMTFeederMaterial(items, userCode, false);
        }
        public int ImportSMTFeederMaterial(object[] items, string userCode, bool replaceAll)
        {
            string strSql = "SELECT * FROM tblSMTFeederMaterialImpLog WHERE LogNo=(SELECT MAX(LogNo) FROM tblSMTFeederMaterialImpLog)";
            object[] objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterialImportLog), new SQLCondition(strSql));
            decimal iLogNo = 1;
            if (objs != null && objs.Length > 0)
            {
                iLogNo = ((SMTFeederMaterialImportLog)objs[0]).LOGNO + 1;
            }
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            int iImpDate = FormatHelper.TODateInt(dtNow);
            int iImpTime = FormatHelper.TOTimeInt(dtNow);
            strSql = "DELETE FROM tblSMTFeederMaterial WHERE ProductCode='{0}' AND SSCode='{1}' AND MachineCode='{2}'";
            // ȫ���滻
            if (replaceAll == true)
            {
                strSql = "DELETE FROM tblSMTFeederMaterial WHERE ProductCode='{0}' AND SSCode='{1}' ";
            }
            string strPrevProductCode = string.Empty;
            string strPrevMachineCode = string.Empty;
            string strPrevSSCode = string.Empty;
            string strPrevTableGroup = string.Empty;
            int iCount = 0;
            Hashtable listMachineFeeder = new Hashtable();
            for (int i = 0; i < items.Length; i++)
            {
                SMTFeederMaterial item = (SMTFeederMaterial)items[i];
                if (item.EAttribute2=="true")
                {
                    if (item.ProductCode != strPrevProductCode || item.MachineCode != strPrevMachineCode || item.StepSequenceCode != strPrevSSCode || item.TableGroup != strPrevTableGroup)
                    {
                        // ������+��Ʒ�Ĺ���ʧЧ
                        if (item.ProductCode != strPrevProductCode || item.StepSequenceCode != strPrevSSCode)
                        {
                            string strSqlMO = "UPDATE tblMachineFeeder SET Enabled='0' WHERE ProductCode='" + item.ProductCode + "' AND SSCode='" + item.StepSequenceCode + "' ";
                            this.DataProvider.CustomExecute(new SQLCondition(strSqlMO));
                        }
                        if (replaceAll == true)
                        {
                            if (item.ProductCode != strPrevProductCode || item.StepSequenceCode != strPrevSSCode)
                                this.DataProvider.CustomExecute(new SQLCondition(string.Format(strSql, item.ProductCode, item.StepSequenceCode)));
                        }
                        else
                        {
                            string strTmp = string.Format(strSql, item.ProductCode, item.StepSequenceCode, item.MachineCode);
                            if (item.TableGroup != string.Empty)
                                strTmp += " AND TblGrp='" + item.TableGroup + "'";
                            this.DataProvider.CustomExecute(new SQLCondition(strTmp));
                        }
                        strPrevProductCode = item.ProductCode;
                        strPrevMachineCode = item.MachineCode;
                        strPrevSSCode = item.StepSequenceCode;
                        strPrevTableGroup = item.TableGroup;
                        listMachineFeeder = new Hashtable();
                        object[] objmf = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition("SELECT * FROM tblMachineFeeder WHERE ProductCode='" + item.ProductCode + "' AND SSCode='" + item.StepSequenceCode + "' AND MachineCode='" + item.MachineCode + "'"));
                        if (objmf != null)
                        {
                            for (int n = 0; n < objmf.Length; n++)
                            {
                                MachineFeeder mf = (MachineFeeder)objmf[n];
                                string strKey = mf.ProductCode + ":" + mf.StepSequenceCode + ":" + mf.MachineCode + ":" + mf.MachineStationCode;
                                if (listMachineFeeder.ContainsKey(strKey) == false)
                                {
                                    listMachineFeeder.Add(strKey, mf);
                                }
                            }
                        }
                    }
                    item.MaintainUser = userCode;
                    item.MaintainDate = FormatHelper.TODateInt(dtNow);
                    item.MaintainTime = FormatHelper.TOTimeInt(dtNow);
                    this._helper.AddDomainObject(item);
                    string strKey1 = item.ProductCode + ":" + item.StepSequenceCode + ":" + item.MachineCode + ":" + item.MachineStationCode;
                    MachineFeeder mfeeder = (MachineFeeder)listMachineFeeder[strKey1];
                    if (mfeeder != null && mfeeder.UnitQty != item.Qty)
                    {
                        // �������ϼ�¼�еĵ�λ����
                        string strSql_update = "UPDATE tblMachineFeeder SET UnitQty=" + item.Qty.ToString() + " WHERE MachineCode='" + item.MachineCode + "' AND MachineStationCode='" + item.MachineStationCode + "' AND ProductCode='" + item.ProductCode + "' AND SSCode='" + item.StepSequenceCode + "' ";
                        this.DataProvider.CustomExecute(new SQLCondition(strSql_update));
                        // ����ReelQty�еĵ�λ����
                        strSql_update = "UPDATE tblReelQty SET UnitQty=" + item.Qty.ToString() + " WHERE ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE MachineCode='" + item.MachineCode + "' AND MachineStationCode='" + item.MachineStationCode + "' AND ProductCode='" + item.ProductCode + "' AND SSCode='" + item.StepSequenceCode + "') ";
                        this.DataProvider.CustomExecute(new SQLCondition(strSql_update));
                        // ����Feeder�еĵ�λ����
                        strSql_update = "UPDATE tblFeeder SET CurrUnitQty=" + item.Qty.ToString() + " WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE MachineCode='" + item.MachineCode + "' AND MachineStationCode='" + item.MachineStationCode + "' AND ProductCode='" + item.ProductCode + "' AND SSCode='" + item.StepSequenceCode + "') ";
                        this.DataProvider.CustomExecute(new SQLCondition(strSql_update));
                    }
                    iCount++;
                }
                // ����Log
                SMTFeederMaterialImportLog log = new SMTFeederMaterialImportLog();
                log.LOGNO = iLogNo;
                log.Sequence = Convert.ToDecimal(i + 1);
                log.ImportUser = userCode;
                log.ImportDate = iImpDate;
                log.ImportTime = iImpTime;
                log.CheckResult = FormatHelper.BooleanToString(true);
                if (item.EAttribute2 =="false")
                {
                    log.CheckResult = FormatHelper.BooleanToString(false);
                    log.CheckDescription = item.EAttribute1.Split(':')[1];
                }
                log.MachineCode = item.MachineCode;
                log.MachineStationCode = item.MachineStationCode;
                log.ProductCode = item.ProductCode;
                log.StepSequenceCode = item.StepSequenceCode;
                log.MaterialCode = item.MaterialCode;
                log.SourceMaterialCode = item.SourceMaterialCode;
                log.FeederSpecCode = item.FeederSpecCode;
                log.Qty = item.Qty;
                log.TableGroup = item.TableGroup;
                log.MaintainUser = item.MaintainUser;
                log.MaintainDate = item.MaintainDate;
                log.MaintainTime = item.MaintainTime;
                this.AddSMTFeederMaterialImportLog(log);
            }
            return iCount;
        }

        public object[] GetSMTFeederMaterial(string productCode, string sscode)
        {
            string strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + productCode + "' AND SSCode='" + sscode + "'";
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
        }
        #endregion

        #region SMTFeederMaterialImportLog
        /// <summary>
        /// SMT Feeder����
        /// </summary>
        public SMTFeederMaterialImportLog CreateNewSMTFeederMaterialImportLog()
        {
            return new SMTFeederMaterialImportLog();
        }

        public void AddSMTFeederMaterialImportLog(SMTFeederMaterialImportLog sMTFeederMaterialImportLog)
        {
            this._helper.AddDomainObject(sMTFeederMaterialImportLog);
        }

        public void UpdateSMTFeederMaterialImportLog(SMTFeederMaterialImportLog sMTFeederMaterialImportLog)
        {
            this._helper.UpdateDomainObject(sMTFeederMaterialImportLog);
        }

        public void DeleteSMTFeederMaterialImportLog(SMTFeederMaterialImportLog sMTFeederMaterialImportLog)
        {
            this._helper.DeleteDomainObject(sMTFeederMaterialImportLog);
        }

        public void DeleteSMTFeederMaterialImportLog(SMTFeederMaterialImportLog[] sMTFeederMaterialImportLog)
        {
            this._helper.DeleteDomainObject(sMTFeederMaterialImportLog);
        }

        public object GetSMTFeederMaterialImportLog(decimal lOGNO, decimal sequence)
        {
            return this.DataProvider.CustomSearch(typeof(SMTFeederMaterialImportLog), new object[] { lOGNO, sequence });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTFeederMaterialImportLog��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-5 13:18:31
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="lOGNO">LOGNO��ģ����ѯ</param>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <returns> SMTFeederMaterialImportLog���ܼ�¼��</returns>
        public int QuerySMTFeederMaterialImportLogCount(string itemCode, string sscode, string importUser, string importDate)
        {
            string strSql = "select count(*) from (select DISTINCT LogNo,ImpUser,ImpDate,ImpTime,ProductCode,SSCode from TBLSMTFEEDERMATERIALIMPLOG where 1=1 ";
            if (itemCode != string.Empty)
                strSql += " AND ProductCode = '" + itemCode + "' ";
            if (sscode != string.Empty)
                strSql += " AND SSCode = '" + sscode + "' ";
            if (importUser != string.Empty)
                strSql += " AND ImpUser = '" + importUser + "' ";
            if (importDate != string.Empty && importDate != "0")
                strSql += " AND ImpDate=" + importDate + " ";
            strSql += ") ";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTFeederMaterialImportLog
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-5 13:18:31
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="lOGNO">LOGNO��ģ����ѯ</param>
        /// <param name="sequence">Sequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTFeederMaterialImportLog����</returns>
        public object[] QuerySMTFeederMaterialImportLog(string itemCode, string sscode, string importUser, string importDate, int inclusive, int exclusive)
        {
            string strSql = "select LogNo,ImpUser,ImpDate,ImpTime,ProductCode,SSCode from (select DISTINCT LogNo,ImpUser,ImpDate,ImpTime,ProductCode,SSCode from TBLSMTFEEDERMATERIALIMPLOG where 1=1 ";
            if (itemCode != string.Empty)
                strSql += " AND ProductCode = '" + itemCode + "' ";
            if (sscode != string.Empty)
                strSql += " AND SSCode = '" + sscode + "' ";
            if (importUser != string.Empty)
                strSql += " AND ImpUser = '" + importUser + "' ";
            if (importDate != string.Empty && importDate != "0")
                strSql += " AND ImpDate=" + importDate + " ";
            strSql += ")";
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterialImportLog), new PagerCondition(strSql, "LOGNO DESC", inclusive, exclusive));
        }

        public int QuerySMTFeederMaterialImportLogDetailCount(string logNo)
        {
            string strSql = string.Format("select count(*) from TBLSMTFEEDERMATERIALIMPLOG where LOGNO={0} ", logNo);
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        public object[] QuerySMTFeederMaterialImportLogDetail(string logNo, int inclusive, int exclusive)
        {
            string strSql = string.Format("select {0} from TBLSMTFEEDERMATERIALIMPLOG where LOGNO={1} ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTFeederMaterialImportLog)), logNo);
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterialImportLog), new PagerCondition(strSql, "SEQ", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTFeederMaterialImportLog
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-5 13:18:31
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTFeederMaterialImportLog���ܼ�¼��</returns>
        public object[] GetAllSMTFeederMaterialImportLog()
        {
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterialImportLog), new SQLCondition(string.Format("select {0} from TBLSMTFEEDERMATERIALIMPLOG order by LOGNO,SEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTFeederMaterialImportLog)))));
        }


        public object GetSMTFeederMaterialImportLog(string productCode, string sscode)
        {
            string strSql = "select * from TBLSMTFEEDERMATERIALIMPLOG log where log.sscode = '" + sscode + "' and log.productcode='" + productCode + "' order by log.logno desc";

            object[] objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterialImportLog), new SQLCondition(strSql));

            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            return null;
        }

        public object[] QuerySMTFeederMaterialImportLog(decimal logNO, string itemCode, string sscode, string machineCode, string machineStationCode)
        {
            string strsql = " select * ";
            strsql += " from Tblsmtfeedermaterialimplog log";
            strsql += " where log.logno = " + logNO + "";
            strsql += " and log.productcode = '" + itemCode + "'";
            strsql += " and log.sscode = '" + sscode + "'";
            strsql += " and log.machinecode = '" + machineCode + "'";
            strsql += " and log.machinestationcode = '" + machineStationCode + "' order by log.seq desc";
            return this.DataProvider.CustomQuery(typeof(SMTFeederMaterialImportLog), new SQLCondition(strsql));
        }


        public object GetSMTFeederMaterialOfMaxLogNo(string productCode, string sscode)
        {
            string strSql = "select * from TBLSMTFEEDERMATERIALIMPLOG log where log.sscode = '" + sscode + "' and log.productcode='" + productCode + "' order by log.logno desc , log.seq desc";

            object[] objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterialImportLog), new SQLCondition(strSql));

            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            return null;
        }

        public object GetMaxSeqSmtFeederMaterialLog(decimal logNo)
        {
            string strsql = " select * ";
            strsql += " from Tblsmtfeedermaterialimplog log";
            strsql += " where log.logno = " + logNo + " order by log.seq desc";

            object[] objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterialImportLog), new SQLCondition(strsql));

            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            return null;
        }

        public object GetMaxLotNoSmtFeedermateriaLog()
        {
            string strsql = " select * ";
            strsql += " from Tblsmtfeedermaterialimplog log";
            strsql += " order by log.logno desc";

            object[] objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterialImportLog), new SQLCondition(strsql));

            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            return null;
        }

        #endregion

        #region SMTLineControlLog
        /// <summary>
        /// ��̨���ϼ�¼
        /// </summary>
        public SMTLineControlLog CreateNewSMTLineControlLog()
        {
            return new SMTLineControlLog();
        }

        public void AddSMTLineControlLog(SMTLineControlLog sMTLineControlLog)
        {
            string strSql = "SELECT MAX(LogID) FROM tblSMTLineCtlLog WHERE ";
            if (sMTLineControlLog.MachineCode != string.Empty)
            {
                strSql += " SSCode='" + sMTLineControlLog.StepSequenceCode + "' AND MachineCode='" + sMTLineControlLog.MachineCode + "' AND MachineStationCode='" + sMTLineControlLog.MachineStationCode + "' ";
            }
            else
            {
                strSql += " MOCode='" + sMTLineControlLog.MOCode + "' AND (MachineCode='' OR MachineCode IS NULL) ";
            }
            strSql = "SELECT * FROM tblSMTLineCtlLog WHERE LogID=(" + strSql + ")";
            object[] objs = this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                SMTLineControlLog log = (SMTLineControlLog)objs[0];
                if (log.LineStatus == sMTLineControlLog.LineStatus)
                    return;
            }
            this._helper.AddDomainObject(sMTLineControlLog);
        }

        public void UpdateSMTLineControlLog(SMTLineControlLog sMTLineControlLog)
        {
            this._helper.UpdateDomainObject(sMTLineControlLog);
        }

        public void DeleteSMTLineControlLog(SMTLineControlLog sMTLineControlLog)
        {
            this._helper.DeleteDomainObject(sMTLineControlLog);
        }

        public void DeleteSMTLineControlLog(SMTLineControlLog[] sMTLineControlLog)
        {
            this._helper.DeleteDomainObject(sMTLineControlLog);
        }

        public object GetSMTLineControlLog(decimal logID)
        {
            return this.DataProvider.CustomSearch(typeof(SMTLineControlLog), new object[] { logID });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTLineControlLog��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-28 8:44:07
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="logID">LogID��ģ����ѯ</param>
        /// <returns> SMTLineControlLog���ܼ�¼��</returns>
        public int QuerySMTLineControlLogCount(decimal logID)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTLINECTLLOG where 1=1 and LOGID like '{0}%' ", logID)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTLineControlLog
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-28 8:44:07
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="logID">LogID��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTLineControlLog����</returns>
        public object[] QuerySMTLineControlLog(decimal logID, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new PagerCondition(string.Format("select {0} from TBLSMTLINECTLLOG where 1=1 and LOGID like '{1}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTLineControlLog)), logID), "LOGID", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTLineControlLog
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-28 8:44:07
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTLineControlLog���ܼ�¼��</returns>
        public object[] GetAllSMTLineControlLog()
        {
            return this.DataProvider.CustomQuery(typeof(SMTLineControlLog), new SQLCondition(string.Format("select {0} from TBLSMTLINECTLLOG order by LOGID", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTLineControlLog)))));
        }

        #endregion

        #region SMTMachineActiveInno
        /// <summary>
        /// 
        /// </summary>
        public SMTMachineActiveInno CreateNewSMTMachineActiveInno()
        {
            return new SMTMachineActiveInno();
        }

        public void AddSMTMachineActiveInno(SMTMachineActiveInno sMTMachineActiveInno)
        {
            this._helper.AddDomainObject(sMTMachineActiveInno);
        }

        public void UpdateSMTMachineActiveInno(SMTMachineActiveInno sMTMachineActiveInno)
        {
            this._helper.UpdateDomainObject(sMTMachineActiveInno);
        }

        public void DeleteSMTMachineActiveInno(SMTMachineActiveInno sMTMachineActiveInno)
        {
            this._helper.DeleteDomainObject(sMTMachineActiveInno);
        }

        public void DeleteSMTMachineActiveInno(SMTMachineActiveInno[] sMTMachineActiveInno)
        {
            this._helper.DeleteDomainObject(sMTMachineActiveInno);
        }

        public object GetSMTMachineActiveInno(string mOCode, string stepSequenceCode, string machineCode)
        {
            return this.DataProvider.CustomSearch(typeof(SMTMachineActiveInno), new object[] { mOCode, stepSequenceCode, machineCode });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTMachineActiveInno��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-22 10:37:03
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="stepSequenceCode">StepSequenceCode��ģ����ѯ</param>
        /// <param name="machineCode">MachineCode��ģ����ѯ</param>
        /// <returns> SMTMachineActiveInno���ܼ�¼��</returns>
        public int QuerySMTMachineActiveInnoCount(string mOCode, string stepSequenceCode, string machineCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTMACHINEACTIVEINNO where 1=1 and MOCODE like '{0}%'  and SSCODE like '{1}%'  and MACHINECODE like '{2}%' ", mOCode, stepSequenceCode, machineCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTMachineActiveInno
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-22 10:37:03
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="stepSequenceCode">StepSequenceCode��ģ����ѯ</param>
        /// <param name="machineCode">MachineCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTMachineActiveInno����</returns>
        public object[] QuerySMTMachineActiveInno(string mOCode, string stepSequenceCode, string machineCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTMachineActiveInno), new PagerCondition(string.Format("select {0} from TBLSMTMACHINEACTIVEINNO where 1=1 and MOCODE like '{1}%'  and SSCODE like '{2}%'  and MACHINECODE like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTMachineActiveInno)), mOCode, stepSequenceCode, machineCode), "MOCODE,SSCODE,MACHINECODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTMachineActiveInno
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-22 10:37:03
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTMachineActiveInno���ܼ�¼��</returns>
        public object[] GetAllSMTMachineActiveInno()
        {
            return this.DataProvider.CustomQuery(typeof(SMTMachineActiveInno), new SQLCondition(string.Format("select {0} from TBLSMTMACHINEACTIVEINNO order by MOCODE,SSCODE,MACHINECODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTMachineActiveInno)))));
        }


        #endregion

        #region SMTMachineDiscard
        /// <summary>
        /// �豸���ϵ���
        /// </summary>
        public SMTMachineDiscard CreateNewSMTMachineDiscard()
        {
            return new SMTMachineDiscard();
        }

        public void AddSMTMachineDiscard(SMTMachineDiscard sMTMachineDiscard)
        {
            this._helper.AddDomainObject(sMTMachineDiscard);
        }

        public void UpdateSMTMachineDiscard(SMTMachineDiscard sMTMachineDiscard)
        {
            this._helper.UpdateDomainObject(sMTMachineDiscard);
        }

        public void DeleteSMTMachineDiscard(SMTMachineDiscard sMTMachineDiscard)
        {
            this._helper.DeleteDomainObject(sMTMachineDiscard);
        }

        public void DeleteSMTMachineDiscard(SMTMachineDiscard[] sMTMachineDiscard)
        {
            this._helper.DeleteDomainObject(sMTMachineDiscard);
        }

        public object GetSMTMachineDiscard(string mOCode, string stepSequenceCode, string materialCode, string machineStationCdoe)
        {
            return this.DataProvider.CustomSearch(typeof(SMTMachineDiscard), new object[] { mOCode, stepSequenceCode, materialCode, machineStationCdoe });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTMachineDiscard��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-3 9:55:57
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="stepSequenceCode">StepSequenceCode��ģ����ѯ</param>
        /// <param name="materialCode">MaterialCode��ģ����ѯ</param>
        /// <param name="machineStationCdoe">MachineStationCdoe��ģ����ѯ</param>
        /// <returns> SMTMachineDiscard���ܼ�¼��</returns>
        public int QuerySMTMachineDiscardCount(string mOCode, string stepSequenceCode, string materialCode, string machineStationCdoe)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTMACHINEDISCARD where 1=1 and MOCODE like '{0}%'  and SSCODE like '{1}%'  and MATERIALCODE like '{2}%'  and MACHINESTATIONCODE like '{3}%' ", mOCode, stepSequenceCode, materialCode, machineStationCdoe)));
        }
        public int QuerySMTMachineDiscardCount(string moCode, string stepSequenceCode)
        {
            string strSql = "select COUNT(*) from TBLSMTMACHINEDISCARD where 1=1 ";
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (stepSequenceCode != string.Empty)
                strSql += " AND SSCode LIKE '" + stepSequenceCode + "%' ";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTMachineDiscard
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-3 9:55:57
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="stepSequenceCode">StepSequenceCode��ģ����ѯ</param>
        /// <param name="materialCode">MaterialCode��ģ����ѯ</param>
        /// <param name="machineStationCdoe">MachineStationCdoe��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTMachineDiscard����</returns>
        public object[] QuerySMTMachineDiscard(string mOCode, string stepSequenceCode, string materialCode, string machineStationCdoe, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTMachineDiscard), new PagerCondition(string.Format("select {0} from TBLSMTMACHINEDISCARD where 1=1 and MOCODE like '{1}%'  and SSCODE like '{2}%'  and MATERIALCODE like '{3}%'  and MACHINESTATIONCODE like '{4}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTMachineDiscard)), mOCode, stepSequenceCode, materialCode, machineStationCdoe), "MOCODE,SSCODE,MATERIALCODE,MACHINESTATIONCODE", inclusive, exclusive));
        }
        public object[] QuerySMTMachineDiscard(string moCode, string stepSequenceCode, int inclusive, int exclusive)
        {
            string strSql = string.Format("select {0} from TBLSMTMACHINEDISCARD where 1=1 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTMachineDiscard)));
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (stepSequenceCode != string.Empty)
                strSql += " AND SSCode LIKE '" + stepSequenceCode + "%' ";
            return this.DataProvider.CustomQuery(typeof(SMTMachineDiscard), new PagerCondition(strSql, "MOCode,MaterialCode,MachineStationCode", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTMachineDiscard
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-3 9:55:57
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTMachineDiscard���ܼ�¼��</returns>
        public object[] GetAllSMTMachineDiscard()
        {
            return this.DataProvider.CustomQuery(typeof(SMTMachineDiscard), new SQLCondition(string.Format("select {0} from TBLSMTMACHINEDISCARD order by MOCODE,SSCODE,MATERIALCODE,MACHINESTATIONCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTMachineDiscard)))));
        }

        public int ImportSMTMachineDiscard(object[] items, string userCode)
        {
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            int iImpDate = FormatHelper.TODateInt(dtNow);
            int iImpTime = FormatHelper.TOTimeInt(dtNow);
            string strSql = "DELETE FROM TBLSMTMACHINEDISCARD WHERE MOCode='{0}' AND SSCode='{1}' AND MaterialCode='{2}' AND MachineStationCode='{3}' ";
            string strPrevMOCode = string.Empty;
            string strPrevMaterialCode = string.Empty;
            string strPrevSSCode = string.Empty;
            string strPrevMachineStationCode = string.Empty;
            int iCount = 0;
            Hashtable listMachineFeeder = new Hashtable();
            for (int i = 0; i < items.Length; i++)
            {
                SMTMachineDiscard item = (SMTMachineDiscard)items[i];
                if (item.MOCode != string.Empty && item.StepSequenceCode != string.Empty && item.MaterialCode != string.Empty && item.MachineStationCode != string.Empty)
                {
                    if (item.MOCode != strPrevMOCode || item.MaterialCode != strPrevMaterialCode || item.StepSequenceCode != strPrevSSCode || item.MachineStationCode != strPrevMachineStationCode)
                    {
                        this.DataProvider.CustomExecute(new SQLCondition(string.Format(strSql, item.MOCode, item.StepSequenceCode, item.MaterialCode, item.MachineStationCode)));
                        strPrevMOCode = item.MOCode;
                        strPrevMaterialCode = item.MaterialCode;
                        strPrevSSCode = item.StepSequenceCode;
                        strPrevMachineStationCode = item.MachineStationCode;
                    }
                    item.MaintainUser = userCode;
                    item.MaintainDate = FormatHelper.TODateInt(dtNow);
                    item.MaintainTime = FormatHelper.TOTimeInt(dtNow);
                    this._helper.AddDomainObject(item);
                    iCount++;
                }
            }
            return iCount;
        }

        #endregion

        #region SMTMachineInno
        /// <summary>
        /// 
        /// </summary>
        public SMTMachineInno CreateNewSMTMachineInno()
        {
            return new SMTMachineInno();
        }

        public void AddSMTMachineInno(SMTMachineInno sMTMachineInno)
        {
            this._helper.AddDomainObject(sMTMachineInno);
        }

        public void UpdateSMTMachineInno(SMTMachineInno sMTMachineInno)
        {
            this._helper.UpdateDomainObject(sMTMachineInno);
        }

        public void DeleteSMTMachineInno(SMTMachineInno sMTMachineInno)
        {
            this._helper.DeleteDomainObject(sMTMachineInno);
        }

        public void DeleteSMTMachineInno(SMTMachineInno[] sMTMachineInno)
        {
            this._helper.DeleteDomainObject(sMTMachineInno);
        }

        public object GetSMTMachineInno(decimal iNNO, decimal iNNOSequence)
        {
            return this.DataProvider.CustomSearch(typeof(SMTMachineInno), new object[] { iNNO, iNNOSequence });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTMachineInno��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-22 10:25:31
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="iNNO">INNO��ģ����ѯ</param>
        /// <param name="iNNOSequence">INNOSequence��ģ����ѯ</param>
        /// <returns> SMTMachineInno���ܼ�¼��</returns>
        public int QuerySMTMachineInnoCount(decimal iNNO, decimal iNNOSequence)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTMACHINEINNO where 1=1 and INNO like '{0}%'  and INNOSEQ like '{1}%' ", iNNO, iNNOSequence)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTMachineInno
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-22 10:25:31
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="iNNO">INNO��ģ����ѯ</param>
        /// <param name="iNNOSequence">INNOSequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTMachineInno����</returns>
        public object[] QuerySMTMachineInno(decimal iNNO, decimal iNNOSequence, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTMachineInno), new PagerCondition(string.Format("select {0} from TBLSMTMACHINEINNO where 1=1 and INNO like '{1}%'  and INNOSEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTMachineInno)), iNNO, iNNOSequence), "INNO,INNOSEQ", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTMachineInno
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-22 10:25:31
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTMachineInno���ܼ�¼��</returns>
        public object[] GetAllSMTMachineInno()
        {
            return this.DataProvider.CustomQuery(typeof(SMTMachineInno), new SQLCondition(string.Format("select {0} from TBLSMTMACHINEINNO order by INNO,INNOSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTMachineInno)))));
        }

        private void GenerateMachineInno(string moCode, string ssCode, string machineCode, string userCode)
        {
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            int iSeq = this.DataProvider.GetCount(new SQLCondition("SELECT SEQ_TBLSMTMACHINEINNO.NextVal Seq FROM DUAL"));
            string strSql = "INSERT INTO TBLSMTMACHINEINNO (INNO,INNOSEQ,MACHINECODE,MACHINESTATIONCODE,PRODUCTCODE,MOCODE,SSCODE,FEEDERSPECCODE,FEEDERCODE,REELNO,MATERIALCODE,UNITQTY,LOTNO,DATECODE,MUSER,MDATE,MTIME) ";
            strSql += " SELECT " + iSeq.ToString() + ",RowNum,MACHINECODE,MACHINESTATIONCODE,PRODUCTCODE,MOCODE,SSCODE,FEEDERSPECCODE,FEEDERCODE,REELNO,MATERIALCODE,UNITQTY,LOTNO,DATECODE,'" + userCode + "'," + FormatHelper.TODateInt(dtNow).ToString() + "," + FormatHelper.TOTimeInt(dtNow).ToString();
            strSql += " FROM tblMachineFeeder WHERE MOCode='" + moCode + "' AND SSCode='" + ssCode + "' AND MachineCode='" + machineCode + "' AND Enabled='1' AND StationEnabled='1' ";
            int iRet = ((SQLDomainDataProvider)DataProvider).PersistBroker.ExecuteWithReturn(strSql);
            if (iRet > 0)
            {
                strSql = "UPDATE TBLSMTMACHINEACTIVEINNO SET INNO=" + iSeq.ToString() + " WHERE MOCode='" + moCode + "' AND SSCode='" + ssCode + "' AND MachineCode='" + machineCode + "' ";
                iRet = ((SQLDomainDataProvider)DataProvider).PersistBroker.ExecuteWithReturn(strSql);
                if (iRet == 0)
                {
                    SMTMachineActiveInno inno = new SMTMachineActiveInno();
                    inno.MOCode = moCode;
                    inno.StepSequenceCode = ssCode;
                    inno.MachineCode = machineCode;
                    inno.INNO = iSeq;
                    this.AddSMTMachineActiveInno(inno);
                }
            }
        }

        #endregion

        #region SMTRCardInno
        /// <summary>
        /// 
        /// </summary>
        public SMTRCardInno CreateNewSMTRCardInno()
        {
            return new SMTRCardInno();
        }

        public void AddSMTRCardInno(SMTRCardInno sMTRCardInno)
        {
            this._helper.AddDomainObject(sMTRCardInno);
        }

        public void UpdateSMTRCardInno(SMTRCardInno sMTRCardInno)
        {
            this._helper.UpdateDomainObject(sMTRCardInno);
        }

        public void DeleteSMTRCardInno(SMTRCardInno sMTRCardInno)
        {
            this._helper.DeleteDomainObject(sMTRCardInno);
        }

        public void DeleteSMTRCardInno(SMTRCardInno[] sMTRCardInno)
        {
            this._helper.DeleteDomainObject(sMTRCardInno);
        }

        public object GetSMTRCardInno(string runningCard, decimal runningCardSequence, decimal iNNO)
        {
            return this.DataProvider.CustomSearch(typeof(SMTRCardInno), new object[] { runningCard, runningCardSequence, iNNO });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTRCardInno��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-22 10:25:31
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <param name="iNNO">INNO��ģ����ѯ</param>
        /// <returns> SMTRCardInno���ܼ�¼��</returns>
        public int QuerySMTRCardInnoCount(string runningCard, decimal runningCardSequence, decimal iNNO)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTRCARDINNO where 1=1 and RCARD like '{0}%'  and RCARDSEQ like '{1}%'  and INNO like '{2}%' ", runningCard, runningCardSequence, iNNO)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTRCardInno
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-22 10:25:31
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <param name="iNNO">INNO��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTRCardInno����</returns>
        public object[] QuerySMTRCardInno(string runningCard, decimal runningCardSequence, decimal iNNO, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTRCardInno), new PagerCondition(string.Format("select {0} from TBLSMTRCARDINNO where 1=1 and RCARD like '{1}%'  and RCARDSEQ like '{2}%'  and INNO like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTRCardInno)), runningCard, runningCardSequence, iNNO), "RCARD,RCARDSEQ,INNO", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTRCardInno
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-22 10:25:31
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTRCardInno���ܼ�¼��</returns>
        public object[] GetAllSMTRCardInno()
        {
            return this.DataProvider.CustomQuery(typeof(SMTRCardInno), new SQLCondition(string.Format("select {0} from TBLSMTRCARDINNO order by RCARD,RCARDSEQ,INNO", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTRCardInno)))));
        }


        #endregion

        #region SMTRCardMaterial
        /// <summary>
        /// SMT��RCard����
        /// </summary>
        public SMTRCardMaterial CreateNewSMTRCardMaterial()
        {
            return new SMTRCardMaterial();
        }

        public void AddSMTRCardMaterial(SMTRCardMaterial sMTRCardMaterial)
        {
            this._helper.AddDomainObject(sMTRCardMaterial);
        }

        public void UpdateSMTRCardMaterial(SMTRCardMaterial sMTRCardMaterial)
        {
            this._helper.UpdateDomainObject(sMTRCardMaterial);
        }

        public void DeleteSMTRCardMaterial(SMTRCardMaterial sMTRCardMaterial)
        {
            this._helper.DeleteDomainObject(sMTRCardMaterial);
        }

        public void DeleteSMTRCardMaterial(SMTRCardMaterial[] sMTRCardMaterial)
        {
            this._helper.DeleteDomainObject(sMTRCardMaterial);
        }

        public object GetSMTRCardMaterial(string runningCard, decimal runningCardSequence)
        {
            return this.DataProvider.CustomSearch(typeof(SMTRCardMaterial), new object[] { runningCard, runningCardSequence });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTRCardMaterial��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-3 8:40:05
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <returns> SMTRCardMaterial���ܼ�¼��</returns>
        public int QuerySMTRCardMaterialCount(string runningCard, decimal runningCardSequence)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTRCARDMATERIAL where 1=1 and RCARD like '{0}%'  and RCARDSEQ like '{1}%' ", runningCard, runningCardSequence)));
        }
        public int QuerySMTRCardMaterialCount(string itemCode, string moCode, int loadDateStart, int loadDateEnd, string rcardStart, string rcardEnd, string resourceCode, string stepSequenceCode, string materialCode, string reelNo, string lotNo, string dateCode)
        {
            string strSql = "";
            if (materialCode != string.Empty || reelNo != string.Empty || lotNo != string.Empty || dateCode != string.Empty)
            {
                strSql = "SELECT COUNT(*) FROM TBLSMTRCARDMATERIAL rcardm,(SELECT rcard FROM tblSMTRCardInno WHERE inno IN (SELECT  inno FROM tblSMTMachineInno WHERE 1=1 ";
                if (materialCode != string.Empty)
                    strSql += " AND MaterialCode LIKE '" + materialCode + "%' ";
                if (reelNo != string.Empty)
                    strSql += " AND ReelNo LIKE '" + reelNo + "%' ";
                if (lotNo != string.Empty)
                    strSql += " AND LotNo LIKE '" + lotNo + "%' ";
                if (dateCode != string.Empty)
                    strSql += " AND DateCode LIKE '" + dateCode + "%' ";
                strSql += " )) rcard ";
                strSql += " WHERE rcardm.RCard=rcard.RCard ";
                if (itemCode != string.Empty)
                    strSql += " AND ItemCode IN ('" + itemCode.Replace(",", "','") + "') ";
                if (moCode != string.Empty)
                    strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
                if (loadDateStart > 0)
                    strSql += " AND MDate>=" + loadDateStart.ToString();
                if (loadDateEnd > 0)
                    strSql += " AND MDate<=" + loadDateEnd.ToString();
                if (rcardStart != string.Empty)
                    strSql += " AND rcardm.RCard>='" + rcardStart + "' ";
                if (rcardEnd != string.Empty)
                    strSql += " AND rcardm.RCard<='" + rcardEnd + "' ";
                if (resourceCode != string.Empty)
                    strSql += " AND ResCode IN ('" + resourceCode.Replace(",", "','") + "') ";
                if (stepSequenceCode != string.Empty)
                    strSql += " AND SSCode IN ('" + stepSequenceCode.Replace(",", "','") + "') ";
            }
            else
            {
                strSql = "SELECT COUNT(*) FROM TBLSMTRCARDMATERIAL WHERE 1=1 ";
                if (itemCode != string.Empty)
                    strSql += " AND ItemCode IN ('" + itemCode.Replace(",", "','") + "') ";
                if (moCode != string.Empty)
                    strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
                if (loadDateStart > 0)
                    strSql += " AND MDate>=" + loadDateStart.ToString();
                if (loadDateEnd > 0)
                    strSql += " AND MDate<=" + loadDateEnd.ToString();
                if (rcardStart != string.Empty)
                    strSql += " AND RCard>='" + rcardStart + "' ";
                if (rcardEnd != string.Empty)
                    strSql += " AND RCard<='" + rcardEnd + "' ";
                if (resourceCode != string.Empty)
                    strSql += " AND ResCode IN ('" + resourceCode.Replace(",", "','") + "') ";
                if (stepSequenceCode != string.Empty)
                    strSql += " AND SSCode IN ('" + stepSequenceCode.Replace(",", "','") + "') ";
            }
            return this.DataProvider.GetCount(new SQLCondition(strSql));
            /*
            string strSql = "SELECT COUNT(*) FROM TBLSMTRCARDMATERIAL WHERE 1=1 ";
            if (itemCode != string.Empty)
                strSql += " AND ItemCode IN ('" + itemCode.Replace(",", "','") + "') ";
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (loadDateStart > 0)
                strSql += " AND MDate>=" + loadDateStart.ToString();
            if (loadDateEnd > 0)
                strSql += " AND MDate<=" + loadDateEnd.ToString();
            if (rcardStart != string.Empty)
                strSql += " AND RCard>='" + rcardStart + "' ";
            if (rcardEnd != string.Empty)
                strSql += " AND RCard<='" + rcardEnd + "' ";
            if (resourceCode != string.Empty)
                strSql += " AND ResCode IN ('" + resourceCode.Replace(",", "','") + "') ";
            if (stepSequenceCode != string.Empty)
                strSql += " AND SSCode IN ('" + stepSequenceCode.Replace(",", "','") + "') ";
            if (materialCode != string.Empty || reelNo != string.Empty || lotNo != string.Empty || dateCode != string.Empty)
            {
                string strReel = "SELECT RCard FROM TBLSMTRCARDMATERIALDTL WHERE 1=1 ";
                if (materialCode != string.Empty)
                    strReel += " AND MaterialCode LIKE '" + materialCode + "%' ";
                if (reelNo != string.Empty)
                    strReel += " AND ReelNo LIKE '" + reelNo + "%' ";
                if (lotNo != string.Empty)
                    strReel += " AND LotNo LIKE '" + lotNo + "%' ";
                if (dateCode != string.Empty)
                    strReel += " AND DateCode LIKE '" + dateCode + "%' ";
                strSql += " AND RCard IN (" + strReel + ") ";
            }
            return this.DataProvider.GetCount(new SQLCondition(strSql));
            */
        }
        public int QuerySMTRCardMaterialDetailCount(string moCode, string rcard)
        {
            //Laws Lu,2006/12/11 ǿ��Oracleʹ��tblSMTRCardInno�������
            string strSql = "SELECT COUNT(*) FROM tblSMTRCardInno a,tblSMTMachineInno b WHERE a.inno = b.inno and a.RCard='" + rcard + "' ";
            /*
            string strSql = "SELECT COUNT(*) FROM TBLSMTRCARDMATERIALDTL WHERE 1=1 ";
            if (moCode != string.Empty)
                strSql += " AND MOCode='" + moCode + "' ";
            if (rcard != string.Empty)
                strSql += " AND RCard='" + rcard + "' ";
            */
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTRCardMaterial
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-3 8:40:05
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTRCardMaterial����</returns>
        public object[] QuerySMTRCardMaterial(string runningCard, decimal runningCardSequence, int inclusive, int exclusive)
        {

            return this.DataProvider.CustomQuery(typeof(SMTRCardMaterial), new PagerCondition(string.Format("select {0} from TBLSMTRCARDMATERIAL where 1=1 and RCARD like '{1}%'  and RCARDSEQ like '{2}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTRCardMaterial)), runningCard, runningCardSequence), "RCARD,RCARDSEQ", inclusive, exclusive));
        }
        public object[] QuerySMTRCardMaterial(string itemCode, string moCode, int loadDateStart, int loadDateEnd, string rcardStart, string rcardEnd, string resourceCode, string stepSequenceCode, string materialCode, string reelNo, string lotNo, string dateCode, int inclusive, int exclusive)
        {
            string strSql = "";
            if (materialCode != string.Empty || reelNo != string.Empty || lotNo != string.Empty || dateCode != string.Empty)
            {
                strSql = "SELECT rcardm.ItemCode,rcardm.MOCode,rcardm.RCard,rcardm.SSCode,rcardm.ResCode,rcardm.MUser,rcardm.MDate,rcardm.MTime FROM TBLSMTRCARDMATERIAL rcardm,(SELECT rcard FROM tblSMTRCardInno WHERE inno IN (SELECT  inno FROM tblSMTMachineInno WHERE 1=1 ";
                if (materialCode != string.Empty)
                    strSql += " AND MaterialCode LIKE '" + materialCode + "%' ";
                if (reelNo != string.Empty)
                    strSql += " AND ReelNo LIKE '" + reelNo + "%' ";
                if (lotNo != string.Empty)
                    strSql += " AND LotNo LIKE '" + lotNo + "%' ";
                if (dateCode != string.Empty)
                    strSql += " AND DateCode LIKE '" + dateCode + "%' ";
                strSql += " )) rcard ";
                strSql += " WHERE rcardm.RCard=rcard.RCard ";
                if (itemCode != string.Empty)
                    strSql += " AND ItemCode IN ('" + itemCode.Replace(",", "','") + "') ";
                if (moCode != string.Empty)
                    strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
                if (loadDateStart > 0)
                    strSql += " AND MDate>=" + loadDateStart.ToString();
                if (loadDateEnd > 0)
                    strSql += " AND MDate<=" + loadDateEnd.ToString();
                if (rcardStart != string.Empty)
                    strSql += " AND rcardm.RCard>='" + rcardStart + "' ";
                if (rcardEnd != string.Empty)
                    strSql += " AND rcardm.RCard<='" + rcardEnd + "' ";
                if (resourceCode != string.Empty)
                    strSql += " AND ResCode IN ('" + resourceCode.Replace(",", "','") + "') ";
                if (stepSequenceCode != string.Empty)
                    strSql += " AND SSCode IN ('" + stepSequenceCode.Replace(",", "','") + "') ";
                return this.DataProvider.CustomQuery(typeof(SMTRCardMaterial), new PagerCondition(strSql, "rcardm.MOCode,rcardm.RCard", inclusive, exclusive));
            }
            else
            {
                strSql = string.Format("SELECT {0} FROM TBLSMTRCARDMATERIAL WHERE 1=1 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTRCardMaterial)));
                if (itemCode != string.Empty)
                    strSql += " AND ItemCode IN ('" + itemCode.Replace(",", "','") + "') ";
                if (moCode != string.Empty)
                    strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
                if (loadDateStart > 0)
                    strSql += " AND MDate>=" + loadDateStart.ToString();
                if (loadDateEnd > 0)
                    strSql += " AND MDate<=" + loadDateEnd.ToString();
                if (rcardStart != string.Empty)
                    strSql += " AND RCard>='" + rcardStart + "' ";
                if (rcardEnd != string.Empty)
                    strSql += " AND RCard<='" + rcardEnd + "' ";
                if (resourceCode != string.Empty)
                    strSql += " AND ResCode IN ('" + resourceCode.Replace(",", "','") + "') ";
                if (stepSequenceCode != string.Empty)
                    strSql += " AND SSCode IN ('" + stepSequenceCode.Replace(",", "','") + "') ";
                return this.DataProvider.CustomQuery(typeof(SMTRCardMaterial), new PagerCondition(strSql, "MOCode,RCard", inclusive, exclusive));
            }
            /*
            string strSql = string.Format("SELECT {0} FROM TBLSMTRCARDMATERIAL WHERE 1=1 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTRCardMaterial)) );
            if (itemCode != string.Empty)
                strSql += " AND ItemCode IN ('" + itemCode.Replace(",", "','") + "') ";
            if (moCode != string.Empty)
                strSql += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            if (loadDateStart > 0)
                strSql += " AND MDate>=" + loadDateStart.ToString();
            if (loadDateEnd > 0)
                strSql += " AND MDate<=" + loadDateEnd.ToString();
            if (rcardStart != string.Empty)
                strSql += " AND RCard>='" + rcardStart + "' ";
            if (rcardEnd != string.Empty)
                strSql += " AND RCard<='" + rcardEnd + "' ";
            if (resourceCode != string.Empty)
                strSql += " AND ResCode IN ('" + resourceCode.Replace(",", "','") + "') ";
            if (stepSequenceCode != string.Empty)
                strSql += " AND SSCode IN ('" + stepSequenceCode.Replace(",", "','") + "') ";
            if (materialCode != string.Empty || reelNo != string.Empty || lotNo != string.Empty || dateCode != string.Empty)
            {
                string strReel = "SELECT RCard FROM TBLSMTRCARDMATERIALDTL WHERE 1=1 ";
                if (materialCode != string.Empty)
                    strReel += " AND MaterialCode LIKE '" + materialCode + "%' ";
                if (reelNo != string.Empty)
                    strReel += " AND ReelNo LIKE '" + reelNo + "%' ";
                if (lotNo != string.Empty)
                    strReel += " AND LotNo LIKE '" + lotNo + "%' ";
                if (dateCode != string.Empty)
                    strReel += " AND DateCode LIKE '" + dateCode + "%' ";
                strSql += " AND RCard IN (" + strReel + ") ";
            }
            return this.DataProvider.CustomQuery(typeof(SMTRCardMaterial), new PagerCondition(strSql, "MOCode,RCard", inclusive, exclusive));
            */
        }
        public object[] QuerySMTRCardMaterialDetail(string moCode, string rcard, int inclusive, int exclusive)
        {
            //Laws Lu,2006/12/11 ǿ��Oracleʹ��tblSMTRCardInno�������
            string strSql = string.Format("SELECT a.* FROM tblSMTRCardInno b,tblSMTMachineInno a WHERE a.INNO = b.INNO and b.RCARD='" + rcard + "' ");
            /*
            if (moCode != string.Empty)
                strSql += " AND MOCode='" + moCode + "' ";
            if (rcard != string.Empty)
                strSql += " AND RCard='" + rcard + "' ";
            */
            return this.DataProvider.CustomQuery(typeof(SMTMachineInno), new PagerCondition(strSql, "MaterialCode, ReelNo", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTRCardMaterial
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-3 8:40:05
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTRCardMaterial���ܼ�¼��</returns>
        public object[] GetAllSMTRCardMaterial()
        {
            return this.DataProvider.CustomQuery(typeof(SMTRCardMaterial), new SQLCondition(string.Format("select {0} from TBLSMTRCARDMATERIAL order by RCARD,RCARDSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTRCardMaterial)))));
        }

        /// <summary>
        /// AOI����
        /// </summary>
        public Messages LoadMaterialForRCard(string rcard, string resourceCode, string userCode)
        {
            Messages msg = new Messages();
            // ��ѯSimulation
            string strSql = "SELECT * FROM tblSimulation WHERE RCard='" + rcard + "'";
            object[] objsTmp = this.DataProvider.CustomQuery(typeof(Simulation), new SQLCondition(strSql));
            if (objsTmp == null || objsTmp.Length == 0)
            {
                msg.Add(new Message(MessageType.Error, "$Error_CS_ID_Not_Exist"));
                return msg;
            }
            Simulation sim = (Simulation)objsTmp[0];
            // ��ѯ������Ϣ
            BenQGuru.eMES.BaseSetting.BaseModelFacade modelFacade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider);
            BenQGuru.eMES.Domain.BaseSetting.Resource resource = (BenQGuru.eMES.Domain.BaseSetting.Resource)modelFacade.GetResource(resourceCode);
            /*
            // ���û����Ч���������˳�
            strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE MOCode='" + sim.MOCode + "' AND SSCode='" + resource.StepSequenceCode + "' AND Enabled='1' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) <= 0)
                return msg;
            */
            // ���û����Ч���������˳�
            strSql = "SELECT COUNT(*) FROM TBLSMTMACHINEACTIVEINNO WHERE MOCode='" + sim.MOCode + "' AND SSCode='" + resource.StepSequenceCode + "' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) <= 0)
                return msg;
            // ����������������
            SMTRCardMaterial rcardM = new SMTRCardMaterial();
            rcardM.RunningCard = sim.RunningCard;
            rcardM.RunningCardSequence = sim.RunningCardSequence;
            rcardM.MOCode = sim.MOCode;
            rcardM.ItemCode = sim.ItemCode;
            rcardM.ModelCode = sim.ModelCode;
            rcardM.SegmentCode = resource.SegmentCode;
            rcardM.StepSequenceCode = resource.StepSequenceCode;
            rcardM.ResourceCode = resource.ResourceCode;
            rcardM.RouteCode = sim.RouteCode;
            rcardM.OPCode = sim.OPCode;
            BenQGuru.eMES.BaseSetting.ShiftModelFacade shiftModel = new BenQGuru.eMES.BaseSetting.ShiftModelFacade(this.DataProvider);

            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            BenQGuru.eMES.Domain.BaseSetting.TimePeriod period = (BenQGuru.eMES.Domain.BaseSetting.TimePeriod)shiftModel.GetTimePeriod(resource.ShiftTypeCode, FormatHelper.TOTimeInt(dtNow));
            rcardM.ShiftTypeCode = period.ShiftTypeCode;
            rcardM.ShiftCode = period.ShiftCode;
            rcardM.TimePeriodCode = period.TimePeriodCode;
            rcardM.ProductStatus = sim.ProductStatus;
            rcardM.MaintainUser = userCode;
            rcardM.MaintainDate = FormatHelper.TODateInt(dtNow);
            rcardM.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            rcardM.MOSeq = sim.MOSeq;   // Added by Icyer 2007/07/03
            // ��������ϣ����˳�
            strSql = "SELECT COUNT(*) FROM tblSMTRCardMaterial WHERE RCard='" + sim.RunningCard + "' ";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) == 0)
            {
                this.AddSMTRCardMaterial(rcardM);
            }

            // ����INNO	@20060823
            strSql = "INSERT INTO TBLSMTRCARDINNO (RCard,RCardSeq,INNO,MDate,MOSeq) ";
            strSql += " SELECT '" + sim.RunningCard + "'," + sim.RunningCardSequence.ToString() + ",INNO," + FormatHelper.TODateInt(DateTime.Today).ToString() + "," + sim.MOSeq.ToString() + " FROM TBLSMTMACHINEACTIVEINNO WHERE MOCode='" + rcardM.MOCode + "' AND SSCode='" + rcardM.StepSequenceCode + "' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            /*
            // ������ϸ
            strSql = "INSERT INTO tblSMTRCardMaterialDtl (RCard,RCardSeq,ReelSeq,MOCode,SSCode,MachineCode,MachineStationCode,FeederSpecCode,FeederCode,ReelNo,MaterialCode,UnitQty,LotNo,DateCode,MUser,MDate,MTime) ";
            strSql += " SELECT '" + rcardM.RunningCard + "'," + rcardM.RunningCardSequence.ToString() + ",RowNum,'" + rcardM.MOCode + "','" + rcardM.StepSequenceCode + "',MachineCode,MachineStationCode,FeederSpecCode,FeederCode,ReelNo,MaterialCode,UnitQty,LotNo,DateCode,";
            strSql += "'" + userCode + "'," + rcardM.MaintainDate.ToString() + "," + rcardM.MaintainTime.ToString() + " FROM tblMachineFeeder WHERE MOCode='" + rcardM.MOCode + "' AND SSCode='" + resource.StepSequenceCode + "' AND Enabled='1' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            */
            /*
            // ��ѯĿǰ��̨������Ϣ
            strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + sim.MOCode + "' AND SSCode='" + resource.StepSequenceCode + "'";
            objsTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            // ���������ϸ
            if (objsTmp != null)
            {
                for (int i = 0; i < objsTmp.Length; i++)
                {
                    MachineFeeder mf = (MachineFeeder)objsTmp[i];
                    SMTRCardMaterialDetail dtl = new SMTRCardMaterialDetail();
                    dtl.RunningCard = rcardM.RunningCard;
                    dtl.RunningCardSequence = rcardM.RunningCardSequence;
                    dtl.ReelSequence = i + 1;
                    dtl.MOCode = rcardM.MOCode;
                    dtl.StepSequenceCode = resource.StepSequenceCode;
                    dtl.MachineCode = mf.MachineCode;
                    dtl.MachineStationCode = mf.MachineStationCode;
                    dtl.FeederSpecCode = mf.FeederSpecCode;
                    dtl.FeederCode = mf.FeederCode;
                    dtl.ReelNo = mf.ReelNo;
                    dtl.MaterialCode = mf.MaterialCode;
                    dtl.UnitQty = mf.UnitQty;
                    dtl.LotNo = mf.LotNo;
                    dtl.DateCode = mf.DateCode;
                    dtl.MaintainUser = userCode;
                    dtl.MaintainDate = FormatHelper.TODateInt(DateTime.Today);
                    dtl.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
                    this.AddSMTRCardMaterialDetail(dtl);
                }
            }
            */
            return msg;
        }

        /// <summary>
        /// ��ѯ���к������ϵ����ϴ���
        /// </summary>
        public object[] QueryLoadedItemCodeByRCard(string rcard, string rcardSeq)
        {
            string strSql = "SELECT DISTINCT MaterialCode FROM tblSMTMachineInno WHERE Inno IN (";
            strSql += "SELECT Inno FROM tblSMTRCardInno WHERE RCard='" + rcard + "' ";
            if (rcardSeq != string.Empty)
                strSql += " AND RCardSeq=" + rcardSeq;
            strSql += " )";
            return this.DataProvider.CustomQuery(typeof(SMTMachineInno), new SQLCondition(strSql));
        }

        /// <summary>
        /// ��ѯ����BOM
        /// </summary>
        /// <param name="rcard"></param>
        /// <returns></returns>
        public object[] QueryMOBOM(string rcard)
        {
            string strSql = "SELECT * FROM tblMOBOM WHERE MOCode IN (SELECT MOCode FROM tblSimulationReport WHERE RCard='" + rcard + "') ";
            return this.DataProvider.CustomQuery(typeof(MOBOM), new SQLCondition(strSql));
        }
        #endregion

        #region SMTRCardMaterialDetail
        /// <summary>
        /// SMT��RCard����
        /// </summary>
        public SMTRCardMaterialDetail CreateNewSMTRCardMaterialDetail()
        {
            return new SMTRCardMaterialDetail();
        }

        public void AddSMTRCardMaterialDetail(SMTRCardMaterialDetail sMTRCardMaterialDetail)
        {
            this._helper.AddDomainObject(sMTRCardMaterialDetail);
        }

        public void UpdateSMTRCardMaterialDetail(SMTRCardMaterialDetail sMTRCardMaterialDetail)
        {
            this._helper.UpdateDomainObject(sMTRCardMaterialDetail);
        }

        public void DeleteSMTRCardMaterialDetail(SMTRCardMaterialDetail sMTRCardMaterialDetail)
        {
            this._helper.DeleteDomainObject(sMTRCardMaterialDetail);
        }

        public void DeleteSMTRCardMaterialDetail(SMTRCardMaterialDetail[] sMTRCardMaterialDetail)
        {
            this._helper.DeleteDomainObject(sMTRCardMaterialDetail);
        }

        public object GetSMTRCardMaterialDetail(string runningCard, decimal runningCardSequence, decimal reelSequence)
        {
            return this.DataProvider.CustomSearch(typeof(SMTRCardMaterialDetail), new object[] { runningCard, runningCardSequence, reelSequence });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTRCardMaterialDetail��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-3 8:40:05
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <param name="reelSequence">ReelSequence��ģ����ѯ</param>
        /// <returns> SMTRCardMaterialDetail���ܼ�¼��</returns>
        public int QuerySMTRCardMaterialDetailCount(string runningCard, decimal runningCardSequence, decimal reelSequence)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTRCARDMATERIALDTL where 1=1 and RCARD like '{0}%'  and RCARDSEQ like '{1}%'  and REELSEQ like '{2}%' ", runningCard, runningCardSequence, reelSequence)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTRCardMaterialDetail
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-3 8:40:05
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="runningCard">RunningCard��ģ����ѯ</param>
        /// <param name="runningCardSequence">RunningCardSequence��ģ����ѯ</param>
        /// <param name="reelSequence">ReelSequence��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTRCardMaterialDetail����</returns>
        public object[] QuerySMTRCardMaterialDetail(string runningCard, decimal runningCardSequence, decimal reelSequence, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTRCardMaterialDetail), new PagerCondition(string.Format("select {0} from TBLSMTRCARDMATERIALDTL where 1=1 and RCARD like '{1}%'  and RCARDSEQ like '{2}%'  and REELSEQ like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTRCardMaterialDetail)), runningCard, runningCardSequence, reelSequence), "RCARD,RCARDSEQ,REELSEQ", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTRCardMaterialDetail
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-8-3 8:40:05
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTRCardMaterialDetail���ܼ�¼��</returns>
        public object[] GetAllSMTRCardMaterialDetail()
        {
            return this.DataProvider.CustomQuery(typeof(SMTRCardMaterialDetail), new SQLCondition(string.Format("select {0} from TBLSMTRCARDMATERIALDTL order by RCARD,RCARDSEQ,REELSEQ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTRCardMaterialDetail)))));
        }


        #endregion

        #region SMTSensorQty
        /// <summary>
        /// SMTԤ��
        /// </summary>
        public SMTSensorQty CreateNewSMTSensorQty()
        {
            return new SMTSensorQty();
        }

        public void AddSMTSensorQty(SMTSensorQty sMTSensorQty)
        {
            this._helper.AddDomainObject(sMTSensorQty);
        }

        public void UpdateSMTSensorQty(SMTSensorQty sMTSensorQty)
        {
            this._helper.UpdateDomainObject(sMTSensorQty);
        }

        public void DeleteSMTSensorQty(SMTSensorQty sMTSensorQty)
        {
            this._helper.DeleteDomainObject(sMTSensorQty);
        }

        public void DeleteSMTSensorQty(SMTSensorQty[] sMTSensorQty)
        {
            this._helper.DeleteDomainObject(sMTSensorQty);
        }

        public object GetSMTSensorQty(string productCode, string mOCode, string stepSequenceCode, int shiftDay, string shiftTypeCode, string shiftCode, string tPCode)
        {
            return this.DataProvider.CustomSearch(typeof(SMTSensorQty), new object[] { productCode, mOCode, stepSequenceCode, shiftDay, shiftTypeCode, shiftCode, tPCode });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTSensorQty��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-24 13:53:25
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="productCode">ProductCode��ģ����ѯ</param>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="stepSequenceCode">StepSequenceCode��ģ����ѯ</param>
        /// <param name="shiftDay">ShiftDay��ģ����ѯ</param>
        /// <param name="shiftTypeCode">ShiftTypeCode��ģ����ѯ</param>
        /// <param name="shiftCode">ShiftCode��ģ����ѯ</param>
        /// <param name="tPCode">TPCode��ģ����ѯ</param>
        /// <returns> SMTSensorQty���ܼ�¼��</returns>
        public int QuerySMTSensorQtyCount(string productCode, string mOCode, string stepSequenceCode, int shiftDay, string shiftTypeCode, string shiftCode, string tPCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTSENSORQTY where 1=1 and PRODUCTCODE like '{0}%'  and MOCODE like '{1}%'  and SSCODE like '{2}%'  and SHIFTDAY like '{3}%'  and SHIFTTYPECODE like '{4}%'  and SHIFTCODE like '{5}%'  and TPCODE like '{6}%' ", productCode, mOCode, stepSequenceCode, shiftDay, shiftTypeCode, shiftCode, tPCode)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTSensorQty
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-24 13:53:25
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="productCode">ProductCode��ģ����ѯ</param>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="stepSequenceCode">StepSequenceCode��ģ����ѯ</param>
        /// <param name="shiftDay">ShiftDay��ģ����ѯ</param>
        /// <param name="shiftTypeCode">ShiftTypeCode��ģ����ѯ</param>
        /// <param name="shiftCode">ShiftCode��ģ����ѯ</param>
        /// <param name="tPCode">TPCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTSensorQty����</returns>
        public object[] QuerySMTSensorQty(string productCode, string mOCode, string stepSequenceCode, int shiftDay, string shiftTypeCode, string shiftCode, string tPCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTSensorQty), new PagerCondition(string.Format("select {0} from TBLSMTSENSORQTY where 1=1 and PRODUCTCODE like '{1}%'  and MOCODE like '{2}%'  and SSCODE like '{3}%'  and SHIFTDAY like '{4}%'  and SHIFTTYPECODE like '{5}%'  and SHIFTCODE like '{6}%'  and TPCODE like '{7}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTSensorQty)), productCode, mOCode, stepSequenceCode, shiftDay, shiftTypeCode, shiftCode, tPCode), "PRODUCTCODE,MOCODE,SSCODE,SHIFTDAY,SHIFTTYPECODE,SHIFTCODE,TPCODE", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTSensorQty
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-24 13:53:25
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTSensorQty���ܼ�¼��</returns>
        public object[] GetAllSMTSensorQty()
        {
            return this.DataProvider.CustomQuery(typeof(SMTSensorQty), new SQLCondition(string.Format("select {0} from TBLSMTSENSORQTY order by PRODUCTCODE,MOCODE,SSCODE,SHIFTDAY,SHIFTTYPECODE,SHIFTCODE,TPCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTSensorQty)))));
        }

        public object[] GetSMTSensorQtyByTPCode(string moCode, string tpCode, string shiftDay)
        {
            string strSql = "SELECT * FROM tblSMTSensorQty WHERE MOCode='" + moCode + "' AND TPCode='" + tpCode + "' AND ShiftDay=" + shiftDay;
            return this.DataProvider.CustomQuery(typeof(SMTSensorQty), new SQLCondition(strSql));
        }

        #endregion

        #region SMTTargetQty
        /// <summary>
        /// 
        /// </summary>
        public SMTTargetQty CreateNewSMTTargetQty()
        {
            return new SMTTargetQty();
        }

        public void AddSMTTargetQty(SMTTargetQty sMTTargetQty)
        {
            this._helper.AddDomainObject(sMTTargetQty);
        }

        public void UpdateSMTTargetQty(SMTTargetQty sMTTargetQty)
        {
            this._helper.UpdateDomainObject(sMTTargetQty);
        }

        public void DeleteSMTTargetQty(SMTTargetQty sMTTargetQty)
        {
            this._helper.DeleteDomainObject(sMTTargetQty);
        }

        public void DeleteSMTTargetQty(SMTTargetQty[] sMTTargetQty)
        {
            this._helper.DeleteDomainObject(sMTTargetQty);
        }

        public object GetSMTTargetQty(string mOCode, string sSCode, string tPCode)
        {
            return this.DataProvider.CustomSearch(typeof(SMTTargetQty), new object[] { mOCode, sSCode, tPCode });
        }

        /// <summary>
        /// ** ��������:	��ѯSMTTargetQty��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-27 8:55:12
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="sSCode">SSCode��ģ����ѯ</param>
        /// <param name="tPCode">TPCode��ģ����ѯ</param>
        /// <returns> SMTTargetQty���ܼ�¼��</returns>
        public int QuerySMTTargetQtyCount(string mOCode, string sSCode, string tPCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSMTTARGETQTY where 1=1 and MOCODE like '{0}%'  and SSCODE like '{1}%'  and TPCODE like '{2}%' ", mOCode, sSCode, tPCode)));
        }
        public int QuerySMTTargetQtyCount(string mOCode, string sSCode)
        {
            string strSql = "select count(*) from TBLSMTTARGETQTY where 1=1 ";
            if (mOCode != string.Empty)
                strSql += " AND MOCode IN ('" + mOCode.Replace(",", "','") + "') ";
            if (sSCode != string.Empty)
                strSql += " AND SSCode IN ('" + sSCode.Replace(",", "','") + "') ";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯSMTTargetQty
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-27 8:55:12
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="mOCode">MOCode��ģ����ѯ</param>
        /// <param name="sSCode">SSCode��ģ����ѯ</param>
        /// <param name="tPCode">TPCode��ģ����ѯ</param>
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> SMTTargetQty����</returns>
        public object[] QuerySMTTargetQty(string mOCode, string sSCode, string tPCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(SMTTargetQty), new PagerCondition(string.Format("select {0} from TBLSMTTARGETQTY where 1=1 and MOCODE like '{1}%'  and SSCODE like '{2}%'  and TPCODE like '{3}%' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTTargetQty)), mOCode, sSCode, tPCode), "MOCODE,SSCODE,TPCODE", inclusive, exclusive));
        }
        public object[] QuerySMTTargetQty(string mOCode, string sSCode, int inclusive, int exclusive)
        {
            string strSql = string.Format("select {0} from TBLSMTTARGETQTY where 1=1 ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTTargetQty)));
            if (mOCode != string.Empty)
                strSql += " AND MOCode IN ('" + mOCode.Replace(",", "','") + "') ";
            if (sSCode != string.Empty)
                strSql += " AND SSCode IN ('" + sSCode.Replace(",", "','") + "') ";
            return this.DataProvider.CustomQuery(typeof(SMTTargetQty), new PagerCondition(strSql, "MOCode,ShiftCode,TPSeq", inclusive, exclusive));
        }

        /// <summary>
        /// ** ��������:	������е�SMTTargetQty
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
        /// ** �� ��:		2006-7-27 8:55:12
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <returns>SMTTargetQty���ܼ�¼��</returns>
        public object[] GetAllSMTTargetQty()
        {
            return this.DataProvider.CustomQuery(typeof(SMTTargetQty), new SQLCondition(string.Format("select {0} from TBLSMTTARGETQTY order by MOCODE,SSCODE,TPCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTTargetQty)))));
        }

        public void UpdateSMTTargetQty(string segmentCode, string ssCode, string moCode, decimal qtyPerHour, string userCode)
        {
            if (segmentCode == string.Empty || ssCode == string.Empty || moCode == string.Empty)
                return;
            BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(moCode);
            if (mo == null)
                throw new Exception("$CS_MO_Not_Exist");
            BenQGuru.eMES.BaseSetting.BaseModelFacade modelFacade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(this.DataProvider);
            object objTmp = modelFacade.GetSegment(segmentCode);
            if (objTmp == null)
                return;

            StepSequence ss = (StepSequence)modelFacade.GetStepSequence(ssCode);
            if (ss == null)
                return;
            string strShiftTypeCode = ss.ShiftTypeCode;

            BenQGuru.eMES.BaseSetting.ShiftModel shiftModel = new BenQGuru.eMES.BaseSetting.ShiftModel(this.DataProvider);
            string strSql = "SELECT * FROM TBLTP WHERE ShiftTypeCode='" + strShiftTypeCode + "' ";
            object[] objtp = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.BaseSetting.TimePeriod), new SQLCondition(strSql));
            if (objtp == null || objtp.Length == 0)
                return;
            strSql = "DELETE FROM tblSMTTargetQty WHERE MOCode='" + moCode + "' AND SSCode='" + ssCode + "' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            for (int i = 0; i < objtp.Length; i++)
            {
                BenQGuru.eMES.Domain.BaseSetting.TimePeriod tp = (BenQGuru.eMES.Domain.BaseSetting.TimePeriod)objtp[i];
                DateTime dtBegin = DateTime.Parse("2006-01-01 " + FormatHelper.ToTimeString(tp.TimePeriodBeginTime));
                DateTime dtEnd = DateTime.Parse("2006-01-01 " + FormatHelper.ToTimeString(tp.TimePeriodEndTime));
                if (FormatHelper.StringToBoolean(tp.IsOverDate) == true)
                {
                    dtEnd = DateTime.Parse("2006-01-02 " + FormatHelper.ToTimeString(tp.TimePeriodEndTime));
                }
                TimeSpan ts = dtEnd - dtBegin;
                int iHour = ts.Hours;
                if (ts.Minutes >= 30)
                    iHour++;
                SMTTargetQty targetQty = new SMTTargetQty();
                targetQty.MOCode = moCode;
                targetQty.SSCode = ssCode;
                targetQty.TPCode = tp.TimePeriodCode;
                targetQty.ProductCode = mo.ItemCode;
                targetQty.SegmentCode = segmentCode;
                targetQty.ShiftTypeCode = tp.ShiftTypeCode;
                targetQty.ShfitCode = tp.ShiftCode;
                targetQty.TPBeginTime = tp.TimePeriodBeginTime;
                targetQty.TPEndTime = tp.TimePeriodEndTime;
                targetQty.TPSequence = tp.TimePeriodSequence;
                targetQty.TPDescription = tp.TimePeriodDescription;
                targetQty.TPQty = iHour * qtyPerHour;
                targetQty.QtyPerHour = qtyPerHour;
                targetQty.MaintainUser = userCode;
                //2006/11/17,Laws Lu add get DateTime from db Server
                DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

                DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

                targetQty.MaintainDate = FormatHelper.TODateInt(dtNow);
                targetQty.MaintainTime = FormatHelper.TOTimeInt(dtNow);
                this.AddSMTTargetQty(targetQty);
            }
        }

        /// <summary>
        /// ��ѯSMT���������¼��
        /// </summary>
        public int QueryRptMOActualQtyCount(string lineCode, string moCode)
        {
            string strWhere = string.Empty;
            if (lineCode != string.Empty)
                strWhere += " AND SSCode IN ('" + lineCode.Replace(",", "','") + "') ";
            if (moCode != string.Empty)
                strWhere += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            string strSql = "SELECT COUNT(*) FROM tblMO WHERE MOCode IN (SELECT MOCode FROM tblSMTSensorQty WHERE 1=1 " + strWhere + " ) " + GlobalVariables.CurrentOrganizations.GetSQLCondition();
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }
        /// <summary>
        /// ��ѯSMT��������
        /// </summary>
        public object[] QueryRptMOActualQty(string lineCode, string moCode)
        {
            // ��ѯ���߲���
            string strSql = "SELECT * FROM tblSMTSensorQty WHERE 1=1 ";
            string strWhere = string.Empty;
            if (lineCode != string.Empty)
                strWhere += " AND SSCode IN ('" + lineCode.Replace(",", "','") + "') ";
            if (moCode != string.Empty)
                strWhere += " AND MOCode IN ('" + moCode.Replace(",", "','") + "') ";
            strSql += strWhere;
            object[] objsQty = this.DataProvider.CustomQuery(typeof(SMTSensorQty), new SQLCondition(strSql));
            if (objsQty == null || objsQty.Length == 0)
                return null;
            // ��ѯ��������
            strSql = "SELECT * FROM tblMO WHERE MOCode IN (SELECT MOCode FROM tblSMTSensorQty WHERE 1=1 " + strWhere + " ) " + GlobalVariables.CurrentOrganizations.GetSQLCondition();
            object[] objsMO = this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(strSql));
            // ��ѯĿ�����
            strSql = "SELECT * FROM tblSMTTargetQty WHERE MOCode IN (SELECT MOCode FROM tblSMTSensorQty WHERE 1=1 " + strWhere + " ) ";
            strSql += " ORDER BY MOCode,TPBTIME ";
            object[] objTarget = this.DataProvider.CustomQuery(typeof(SMTTargetQty), new SQLCondition(strSql));
            // ����ÿ����������
            SMTRptLineQtyMO[] rpts = new SMTRptLineQtyMO[objsMO.Length];
            for (int i = 0; i < objsMO.Length; i++)
            {
                MO mo = (MO)objsMO[i];
                SMTRptLineQtyMO rpt = new SMTRptLineQtyMO();
                rpt.ProductCode = mo.ItemCode;
                rpt.MOCode = mo.MOCode;
                rpt.PlanQty = Convert.ToDecimal(mo.MOPlanQty);
                // ���㿪ʼ��������������
                int[] iTime = GetMOStartEndTime(mo.MOCode, objsQty);
                rpt.MOBeginDate = iTime[0];
                rpt.MOBeginTime = iTime[1];
                rpt.MOEndDate = iTime[2];
                rpt.MOEndTime = iTime[3];
                // ����ƻ���ʱ
                rpt.PlanManHour = GetMOPlanManHour(mo.MOCode, Convert.ToDecimal(rpt.PlanQty), objTarget);
                // ���㵱ǰĿ�����
                rpt.CurrentQty = GetMOCurrentQty(mo, rpt.MOBeginDate, rpt.MOBeginTime, objTarget);
                // �������ù�ʱ
                rpt.ActualManHour = GetMOActualManHour(rpt);
                // ���㹤��ʵ�ʲ���
                rpt.ActualQty = GetMOActualQty(mo.MOCode, objsQty);
                rpt.DifferenceQty = rpt.CurrentQty - rpt.ActualQty;
                // ���������
                if (rpt.ActualManHour != 0)
                    rpt.MOComPassRate = rpt.PlanManHour / rpt.ActualManHour;
                else
                    rpt.MOComPassRate = 0;
                rpts[i] = rpt;
            }
            return rpts;
        }

        /// <summary>
        /// ��ѯSMT��������ʱ����ϸ��¼��
        /// </summary>
        public int QueryRptMOTimePeriodActualQtyCount(string moCode, string tpCode)
        {
            string strSql = "SELECT COUNT(*) FROM tblSMTSensorQty WHERE MOCode='" + moCode + "' ";
            if (tpCode != string.Empty)
                strSql += " AND TPCode IN ('" + tpCode.Replace(",", "','") + "') ";
            return this.DataProvider.GetCount(new SQLCondition(strSql));
        }
        /// <summary>
        /// ��ѯSMT��������ʱ����ϸ
        /// </summary>
        public object[] QueryRptMOTimePeriodActualQty(string moCode, string tpCode, int inclusive, int exclusive)
        {
            // ��ѯSensor����
            string strSql = string.Format("SELECT {0} FROM tblSMTSensorQty WHERE MOCode='" + moCode + "' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(SMTSensorQty)));
            if (tpCode != string.Empty)
                strSql += " AND TPCode IN ('" + tpCode.Replace(",", "','") + "') ";
            object[] objsQty = this.DataProvider.CustomQuery(typeof(SMTSensorQty), new PagerCondition(strSql, "ShiftDay,TPBTIME", inclusive, exclusive));
            if (objsQty == null || objsQty.Length == 0)
                return null;
            // ��ѯĿ�����
            strSql = "SELECT * FROM tblSMTTargetQty WHERE MOCode='" + moCode + "' ";
            if (tpCode != string.Empty)
                strSql += " AND TPCode IN ('" + tpCode.Replace(",", "','") + "') ";
            object[] objsTarget = this.DataProvider.CustomQuery(typeof(SMTTargetQty), new SQLCondition(strSql));
            Hashtable ht = new Hashtable();
            if (objsTarget != null)
            {
                for (int i = 0; i < objsTarget.Length; i++)
                {
                    SMTTargetQty targetQty = (SMTTargetQty)objsTarget[i];
                    if (ht.ContainsKey(targetQty.TPCode) == false)
                        ht.Add(targetQty.TPCode, targetQty);
                }
            }
            // ��ѯ����
            BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(moCode);
            SMTRptLineQtyTimePeriod[] rpts = new SMTRptLineQtyTimePeriod[objsQty.Length];
            for (int i = 0; i < objsQty.Length; i++)
            {
                SMTSensorQty qty = (SMTSensorQty)objsQty[i];
                SMTRptLineQtyTimePeriod rpt = new SMTRptLineQtyTimePeriod();
                rpt.ProductCode = mo.ItemCode;
                rpt.MOCode = mo.MOCode;
                rpt.ShiftDay = qty.ShiftDay;
                rpt.TPCode = qty.TPCode;
                // ���Ҽƻ�����
                SMTTargetQty targetQty = (SMTTargetQty)ht[rpt.TPCode];
                if (targetQty != null)
                {
                    rpt.TPDescription = targetQty.TPDescription;
                    rpt.CurrentQty = targetQty.TPQty;
                }
                rpt.ActualQty = qty.Qty;
                rpt.DifferenceQty = rpt.CurrentQty - rpt.ActualQty;
                if (rpt.CurrentQty != 0)
                    rpt.TPComPassRate = rpt.ActualQty / rpt.CurrentQty;
                else
                    rpt.TPComPassRate = 0;
                rpt.MaintainReason = qty.DifferenceReason;
                rpt.MaintainUser = qty.DifferenceMaintainUser;
                rpt.MaintainDate = qty.DifferenceMaintainDate;
                rpt.MaintainTime = qty.DifferenceMaintainTime;
                rpt.Day = qty.MaintainDate;
                rpt.TPBeginTime = qty.TPBeginTime;
                rpt.TPEndTime = qty.TPEndTime;
                rpts[i] = rpt;
            }
            return rpts;
        }

        public void UpdateSMTSensorQtyDifferenceReason(string moCode, string shiftDay, string tpCode, string reason, string userCode)
        {
            string strSql = "SELECT * FROM tblSMTSensorQty WHERE MOCode='" + moCode + "' AND ShiftDay=" + shiftDay + " AND TPCode='" + tpCode + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(SMTSensorQty), new SQLCondition(strSql));
            SMTSensorQty qty = (SMTSensorQty)objs[0];
            DateTime dtEnd = Convert.ToDateTime(FormatHelper.ToDateString(qty.MaintainDate) + " " + FormatHelper.ToTimeString(qty.TPEndTime));

            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            // ʱ�ν���������Сʱ������ά��ԭ��
            DateTime dtEnd1 = dtEnd.AddHours(2);
            if (dtEnd > dtNow || dtEnd1 < dtNow)
            {
                throw new Exception("$SMT_SensorQty_Update_Reason_Overstep_Time");
            }
            strSql = "UPDATE tblSMTSensorQty SET DIFFREASON=$DIFFREASON,DIFFMUSER='" + userCode + "',DIFFMDATE=" + FormatHelper.TODateInt(dtNow).ToString() + ",DIFFMTIME=" + FormatHelper.TOTimeInt(dtNow).ToString();
            strSql += " WHERE MOCode='" + moCode + "' AND ShiftDay=" + shiftDay + " AND TPCode='" + tpCode + "' ";
            this.DataProvider.CustomExecute(new SQLParamCondition(strSql, new SQLParameter[] { new SQLParameter("$DIFFREASON", typeof(string), reason) }));
        }

        /// <summary>
        /// ���ҹ�����ʼ��������������
        /// </summary>
        /// <returns></returns>
        private int[] GetMOStartEndTime(string moCode, object[] smtSensorQty)
        {
            int iBDate = 0, iBTime = 0, iEDate = 0, iETime = 0;
            for (int i = 0; i < smtSensorQty.Length; i++)
            {
                SMTSensorQty qty = (SMTSensorQty)smtSensorQty[i];
                if (qty.MOCode == moCode)
                {
                    if (qty.MOBeginDate > 0)
                    {
                        iBDate = qty.MOBeginDate;
                        iBTime = qty.MOBeginTime;
                    }
                    if (qty.MOEndDate > 0)
                    {
                        iEDate = qty.MOEndDate;
                        iETime = qty.MOEndTime;
                    }
                    if (iBDate > 0 && iEDate > 0)
                        break;
                }
            }
            if (iEDate == 0)
            {
                //2006/11/17,Laws Lu add get DateTime from db Server
                DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

                DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

                iEDate = FormatHelper.TODateInt(dtNow);
                iETime = FormatHelper.TOTimeInt(dtNow);
            }
            return new int[] { iBDate, iBTime, iEDate, iETime };
        }
        /// <summary>
        /// ���ܹ���ʵ�ʲ���
        /// </summary>
        private int GetMOActualQty(string moCode, object[] smtSensorQty)
        {
            decimal dQty = 0;
            for (int i = 0; i < smtSensorQty.Length; i++)
            {
                SMTSensorQty qty = (SMTSensorQty)smtSensorQty[i];
                if (qty.MOCode == moCode)
                {
                    dQty += qty.Qty;
                }
            }
            return Convert.ToInt32(dQty);
        }
        /// <summary>
        /// ���㹤���ƻ���ʱ
        /// </summary>
        /// <returns></returns>
        private int GetMOPlanManHour(string moCode, decimal planQty, object[] smtTargetQty)
        {
            if (smtTargetQty == null)
                return 0;
            for (int i = 0; i < smtTargetQty.Length; i++)
            {
                SMTTargetQty qty = (SMTTargetQty)smtTargetQty[i];
                if (qty.MOCode == moCode)
                {
                    int iHour = 0;
                    if (qty.QtyPerHour != 0)
                        iHour = Convert.ToInt32(planQty / qty.QtyPerHour);
                    if (planQty % qty.QtyPerHour != 0)
                        iHour++;
                    return iHour;
                }
            }
            return 0;
        }
        /// <summary>
        /// ���㹤����ǰ����
        /// </summary>
        private int GetMOCurrentQty(MO mo, int beginDate, int beginTime, object[] smtTargetQty)
        {
            decimal dQtyPerHour = 0;
            if (smtTargetQty != null)
            {
                for (int i = 0; i < smtTargetQty.Length; i++)
                {
                    SMTTargetQty qty = (SMTTargetQty)smtTargetQty[i];
                    if (qty.MOCode == mo.MOCode)
                    {
                        dQtyPerHour = qty.QtyPerHour;
                        break;
                    }
                }
            }
            DateTime dtBegin = Convert.ToDateTime(FormatHelper.ToDateString(beginDate) + " " + FormatHelper.ToTimeString(beginTime));
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            DateTime dtEnd = dtNow;
            TimeSpan ts = dtEnd - dtBegin;
            int iHour = Convert.ToInt32(ts.TotalHours);
            if (ts.Minutes > 0)
                iHour++;
            decimal iQty = iHour * dQtyPerHour;
            if (iQty > mo.MOPlanQty)
                iQty = mo.MOPlanQty;
            return Convert.ToInt32(iQty);
        }
        /// <summary>
        /// ���㹤�����ù�ʱ
        /// </summary>
        private int GetMOActualManHour(SMTRptLineQtyMO rpt)
        {
            DateTime dtBegin = Convert.ToDateTime(FormatHelper.ToDateString(rpt.MOBeginDate) + " " + FormatHelper.ToTimeString(rpt.MOBeginTime));
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            DateTime dtEnd = dtNow;
            if (rpt.MOEndDate > 0)
                dtEnd = Convert.ToDateTime(FormatHelper.ToDateString(rpt.MOEndDate) + " " + FormatHelper.ToTimeString(rpt.MOEndTime));
            TimeSpan ts = dtEnd - dtBegin;
            int iHour = Convert.ToInt32(ts.TotalHours);
            if (ts.Minutes > 0)
                iHour++;
            return iHour;
        }
        #endregion

        #region SMTOutMaterial

        public object[] QueryMaterialByMO(string moCode, string materialCode, string reelNo)
        {
            string strSql = "SELECT MOCODE,MATERIALCODE,REELNO,QTY,ISSPECIAL,ISOUTMATERIAL,MUSER,MDATE"
                + " FROM TBLOUTMATERIAL"
                + " WHERE MOCode='" + moCode + "'";
            if (materialCode != string.Empty)
                strSql += " AND MATERIALCODE LIKE '" + materialCode + "%' ";
            if (reelNo != string.Empty)
                strSql += " AND REELNO LIKE '" + reelNo + "%' ";
            strSql += " ORDER BY materialCode";
            return this.DataProvider.CustomQuery(typeof(OutMaterial), new SQLCondition(strSql));

        }

        #endregion

        #region Smtrelationqty
        /// <summary>
        /// Smtrelationqty
        /// </summary>
        public Smtrelationqty CreateNewSmtrelationqty()
        {
            return new Smtrelationqty();
        }

        public void AddSmtrelationqty(Smtrelationqty smtrelationqty)
        {
            this.DataProvider.Insert(smtrelationqty);
        }

        public void DeleteSmtrelationqty(Smtrelationqty smtrelationqty)
        {
            this.DataProvider.Delete(smtrelationqty);
        }

        public void UpdateSmtrelationqty(Smtrelationqty smtrelationqty)
        {
            this.DataProvider.Update(smtrelationqty);
        }

        public object GetSmtrelationqty(string Rcard)
        {
            // return this.DataProvider.CustomSearch(typeof(Smtrelationqty), new object[] { Rcard });

            string sql = string.Format(
               @"select * from tblsmtrelationqty  where
					(rcard,mocode) in (
					select rcard, mocode
					from (select rcard, mocode
							from tblsmtrelationqty 
							where RCARD = '{0}'
							order by MDATE desc, MTIME desc)
							where rownum = 1)",              
                Rcard.ToUpper());

            object[] smtrelationqty = this.DataProvider.CustomQuery(typeof(Smtrelationqty),new SQLCondition(sql));

            if (smtrelationqty == null)
                return null;
            if (smtrelationqty.Length > 0)
                return smtrelationqty[0];
            else
                return null;
        }

        public object GetSMTRelationQty(string rCard, string moCode)
        {
            return this.DataProvider.CustomSearch(typeof(Smtrelationqty), new object[] { rCard, moCode });
        }

        public object[] Querysmtrelation(string mocode, string itemcode, string rcard, int begindate, int enddate)
        {
            string sql = string.Format(" select {0} from tblsmtrelationqty where 1=1", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Smtrelationqty)));
            if (!string.IsNullOrEmpty(mocode))
            {
                sql += " and mocode = '" + FormatHelper.CleanString(mocode).ToUpper() + "'";
            }
            if (!string.IsNullOrEmpty(itemcode))
            {
                sql += " and itemcode = '" + FormatHelper.CleanString(itemcode).ToUpper() + "'";
            }
            if (!string.IsNullOrEmpty(rcard))
            {
                sql += " and rcard = '" + FormatHelper.CleanString(rcard).ToUpper() + "'";
            }
            sql += " and mdate between " + begindate + " and " + enddate + "";
            sql += "   order by tblsmtrelationqty.itemcode asc,    tblsmtrelationqty.mocode   asc,  tblsmtrelationqty.rcard    asc    ";
            return this.DataProvider.CustomQuery(typeof(Smtrelationqty),   new SQLCondition(sql));
        }

        #endregion

        #region Splitboard
        /// <summary>
        /// Splitboard
        /// </summary>
        public Splitboard CreateNewSplitboard()
        {
            return new Splitboard();
        }

        public void AddSplitboard(Splitboard splitboard)
        {
            this.DataProvider.Insert(splitboard);
        }

        public void DeleteSplitboard(Splitboard splitboard)
        {
            this.DataProvider.Delete(splitboard);
        }

        public void UpdateSplitboard(Splitboard splitboard)
        {
            this.DataProvider.Update(splitboard);
        }

        public object GetSplitboard(int Seq, string Mocode, string Rcard)
        {
            return this.DataProvider.CustomSearch(typeof(Splitboard), new object[] { Seq, Mocode, Rcard });
        }

        #endregion

        public object[] QueryMachineFeeder(string moCode, string ssCode)
        {
            string strSql = string.Format("SELECT {0} FROM tblMachineFeeder WHERE MOCODE like '{1}%' AND SSCode='{2}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(MachineFeeder)), moCode, ssCode);
            return this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            //return null;
        }
        #region SMT No Feeder


        /// <summary>
        /// ��������ת�ƣ����ܿ�feeder
        /// </summary>
        /// <param name="moCodeFrom"></param>
        /// <param name="ssCodeFrom"></param>
        /// <param name="moCodeTo"></param>
        /// <param name="sscode"></param>
        /// <returns></returns>
        public Messages TransferMaterialByMONoFeeder(string moCodeFrom, string ssCodeFrom, string moCodeTo, string sscode)
        {
            Messages msg = new Messages();
            // ����Ƿ������������ϱȶ�
            string strSql = "SELECT * FROM tblSMTCheckMaterial WHERE CheckID=(SELECT MAX(CheckID) FROM tblSMTCheckMaterial WHERE MOCode='" + moCodeTo + "' AND SSCode='" + sscode + "')";
            object[] objschk = this.DataProvider.CustomQuery(typeof(SMTCheckMaterial), new SQLCondition(strSql));
            bool bPass = false;
            if (objschk != null && objschk.Length > 0)
            {
                SMTCheckMaterial chk = (SMTCheckMaterial)objschk[0];
                if (chk.CheckResult == SMTCheckMaterialResult.Accept_Exception ||
                    chk.CheckResult == SMTCheckMaterialResult.Matchable)
                {
                    bPass = true;
                }
            }
            if (bPass == false)
            {
                msg.Add(new Message(MessageType.Error, "$MOCode_Not_Check_SMTMaterial_MOBOM"));
                return msg;
            }
            // ��ѯ������������Ϣ
            strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + moCodeFrom + "' AND SSCode='" + ssCodeFrom + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs == null || objs.Length == 0)
            {
                msg.Add(new Message(MessageType.Error, "$MachineFeeder_Not_Exist"));
                return msg;
            }
            MachineFeeder[] machineFeeder = new MachineFeeder[objs.Length];
            objs.CopyTo(machineFeeder, 0);
            /*
            if (machineFeeder[0].StepSequenceCode != sscode)
            {
                msg.Add(new Message(MessageType.Error, "$SourceMO_StepSequenceCode_Is_Not_Current_LoginInfo"));
                return msg;
            }
            */


            //// ��ѯ������Feeder����
            //strSql = "SELECT * FROM tblFeeder WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE MOCode='" + moCodeFrom + "' AND SSCode='" + ssCodeFrom + "' )";
            //objs = this.DataProvider.CustomQuery(typeof(Feeder), new SQLCondition(strSql));
            //Hashtable htFeeder = new Hashtable();
            //if (objs != null)
            //{
            //    for (int i = 0; i < objs.Length; i++)
            //    {
            //        htFeeder.Add(((Feeder)objs[i]).FeederCode, objs[i]);
            //    }
            //}

            // ��ѯ�������Ͼ�
            strSql = "SELECT tblReelQty.ReelNo,tblReelQty.MOCode,tblReelQty.MachineCode,tblReelQty.MachineStationCode,tblReelQty.FeederSpecCode,tblReelQty.FeederCode,tblReelQty.Qty,tblReelQty.UsedQty,tblReelQty.UpdatedQty,tblReelQty.AlertQty,tblReelQty.UnitQty,";
            strSql += "tblMachineFeeder.MaterialCode EAttribute1 ";
            strSql += "FROM tblReelQty,tblMachineFeeder WHERE tblReelQty.ReelNo=tblMachineFeeder.ReelNo ";
            strSql += "AND tblMachineFeeder.MOCode='" + moCodeFrom + "' AND tblMachineFeeder.SSCode='" + ssCodeFrom + "' AND tblReelQty.MOCode='" + moCodeFrom + "' AND tblReelQty.SSCode='" + ssCodeFrom + "' ";
            objs = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
            Hashtable htReel = new Hashtable();
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    htReel.Add(((ReelQty)objs[i]).ReelNo, objs[i]);
                }
            }
            // ��ѯ�¹���
            BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(moCodeTo);
            if (mo == null)
            {
                msg.Add(new Message(MessageType.Error, "$CS_MO_Not_Exist"));
                return msg;
            }
            // �Ƚϲ�Ʒ�Ƿ�һ��
            if (mo.ItemCode != machineFeeder[0].ProductCode)
            {
                msg.Add(new Message(MessageType.Error, "$CS_ItemCode_Not_Compare"));
                return msg;
            }
            // ��ѯ�¹�����վ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND SSCode='" + sscode + "' ";
            objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            Hashtable htSmtFeeder = new Hashtable();
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    SMTFeederMaterial feederMaterial = (SMTFeederMaterial)objs[i];
                    string strKey = feederMaterial.MachineCode + ":" + feederMaterial.MachineStationCode;
                    if (htSmtFeeder.ContainsKey(strKey) == false)
                        htSmtFeeder.Add(strKey, feederMaterial);
                }
            }
            // �������ϼ�¼��Feederʣ��������Ͼ�ʣ������
            for (int i = 0; i < machineFeeder.Length; i++)
            {
                machineFeeder[i].FailReason = string.Empty;
                // ��ѯ��վ���Ƿ����
                SMTFeederMaterial feederMaterial = (SMTFeederMaterial)htSmtFeeder[machineFeeder[i].MachineCode + ":" + machineFeeder[i].MachineStationCode];
                if (feederMaterial == null)
                {
                    machineFeeder[i].FailReason = "$SMTFeederMaterial_Not_Exist";
                    continue;
                }

                //Feeder feeder = (Feeder)htFeeder[machineFeeder[i].FeederCode];
                //if (feeder == null)
                //{
                //    machineFeeder[i].FailReason = "$Feeder_Not_Exist";
                //    continue;
                //}

                ReelQty reelQty = (ReelQty)htReel[machineFeeder[i].ReelNo];
                if (reelQty == null)
                {
                    machineFeeder[i].FailReason = "$Reel_Not_Exist";
                    continue;
                }
                machineFeeder[i].EAttribute1 = (reelQty.Qty - reelQty.UpdatedQty - reelQty.UsedQty).ToString();

                // ��վ���Ӧ��Feeder��������Ƿ�һ��
                if (
                    //feederMaterial.FeederSpecCode != feeder.FeederSpecCode ||
                    feederMaterial.MaterialCode != reelQty.EAttribute1)
                {
                    machineFeeder[i].FailReason = "$SMTFeederMaterial_Not_Match";
                    continue;
                }

                //// ���Feeder
                //if (feeder.Status != FeederStatus.Normal)
                //{
                //    machineFeeder[i].FailReason = "$Feeder_Status_Error";
                //    continue;
                //}
                //if (feeder.MaxCount - feeder.UsedCount < feederMaterial.Qty)
                //{
                //    machineFeeder[i].FailReason = "$FeedreLeftQty_Not_Enough";
                //    continue;
                //}

                // ����Ͼ�
                if (reelQty.Qty - reelQty.UpdatedQty - reelQty.UsedQty < feederMaterial.Qty)
                {
                    machineFeeder[i].FailReason = "$ReelLeftQty_Not_Enough";
                    continue;
                }
            }
            msg.Add(new Message(MessageType.Data, string.Empty, machineFeeder));
            return msg;
        }

        /// <summary>
        /// ��������ת�Ƹ��£����ܿ�feeder
        /// </summary>
        /// <param name="machineFeeder"></param>
        /// <param name="moCodeFrom"></param>
        /// <param name="ssCodeFrom"></param>
        /// <param name="moCodeTo"></param>
        /// <param name="userCode"></param>
        /// <param name="resourceCode"></param>
        /// <param name="stepSequenceCode"></param>
        /// <returns></returns>
        public Messages TransferMaterialByMOUpdateNoFeeder(MachineFeeder[] machineFeeder, string moCodeFrom, string ssCodeFrom, string moCodeTo, string userCode, string resourceCode, string stepSequenceCode)
        {
            Messages msg = new Messages();
            // ��ѯ����
            BenQGuru.eMES.MOModel.MOFacade moFacade = new BenQGuru.eMES.MOModel.MOFacade(this.DataProvider);
            MO mo = (MO)moFacade.GetMO(moCodeTo);
            if (mo == null)
            {
                msg.Add(new Message(MessageType.Error, "$CS_MO_Not_Exist"));
                return msg;
            }
            string strSql = string.Empty;


            //Hashtable htFeeder = new Hashtable();
            //// ��ѯ������Feeder����
            //strSql = "SELECT * FROM tblFeeder WHERE FeederCode IN (SELECT FeederCode FROM tblMachineFeeder WHERE MOCode='" + moCodeFrom + "' AND SSCode='" + ssCodeFrom + "' )";
            //object[] objs = this.DataProvider.CustomQuery(typeof(Feeder), new SQLCondition(strSql));
            //if (objs != null)
            //{
            //    for (int i = 0; i < objs.Length; i++)
            //    {
            //        htFeeder.Add(((Feeder)objs[i]).FeederCode, objs[i]);
            //    }
            //}


            /*
            Hashtable htReel = new Hashtable();
            // ��ѯ�������Ͼ�����
            strSql = "SELECT * FROM tblReel WHERE ReelNo IN (SELECT ReelNo FROM tblMachineFeeder WHERE MOCode='" + moCodeFrom + "')";
            objs = this.DataProvider.CustomQuery(typeof(Reel), new SQLCondition(strSql));
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    htReel.Add(((Reel)objs[i]).ReelNo, objs[i]);
                }
            }
            */
            Hashtable htSmtFeeder = new Hashtable();
            // ��ѯ��վ��ֻ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' and SSCode='" + stepSequenceCode + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    SMTFeederMaterial feederMaterial = (SMTFeederMaterial)objs[i];
                    string strKey = feederMaterial.MachineCode + ":" + feederMaterial.MachineStationCode;
                    if (htSmtFeeder.ContainsKey(strKey) == false)
                    {
                        htSmtFeeder.Add(strKey, objs[i]);
                    }
                }
            }
            // ͣ��
            msg.AddMessages(this.AddStopLineLog(moCodeFrom, ssCodeFrom, SMTLoadFeederOperationType.TransferMO, userCode));
            // ִ�й���ת��
            for (int i = 0; i < machineFeeder.Length; i++)
            {
                // �쳣���ݲ�����
                if (machineFeeder[i].FailReason != string.Empty)
                {
                    continue;
                }

                // Feeder feeder = (Feeder)htFeeder[machineFeeder[i].FeederCode];
                SMTFeederMaterial feederMaterial = (SMTFeederMaterial)htSmtFeeder[machineFeeder[i].MachineCode + ":" + machineFeeder[i].MachineStationCode];
                if (feederMaterial == null)
                {
                    machineFeeder[i].FailReason = "$Data_Wrong";
                    continue;
                }

                // ����
                this.UpdateMachineFeederReelQty(moCodeFrom, machineFeeder[i].MachineCode, machineFeeder[i].MachineStationCode, machineFeeder[i].ReelNo, SMTLoadFeederOperationType.TransferMO, userCode);

                Reel reel = (Reel)this.GetReel(machineFeeder[i].ReelNo);
                reel.UsedFlag = FormatHelper.BooleanToString(true);
                reel.MOCode = mo.MOCode;
                reel.StepSequenceCode = stepSequenceCode;
                this.UpdateReel(reel);

                //// Feeder����
                //feeder.MOCode = mo.MOCode;

                // ����
                this.AddMachineFeederPassNoFeeder(mo, null, reel, feederMaterial, userCode, null, SMTLoadFeederOperationType.TransferMO, resourceCode, stepSequenceCode, string.Empty, string.Empty, FormatHelper.StringToBoolean(machineFeeder[i].StationEnabled));
                // ����������
                if (machineFeeder[i].NextReelNo != null && machineFeeder[i].NextReelNo != string.Empty)
                {
                    ReelQty reelQty = (ReelQty)this.GetReelQty(machineFeeder[i].NextReelNo, machineFeeder[i].MOCode);
                    if (reelQty != null)
                        this.DeleteReelQty(reelQty);
                    reel = (Reel)this.GetReel(machineFeeder[i].NextReelNo);
                    reel.UsedFlag = FormatHelper.BooleanToString(true);
                    reel.MOCode = mo.MOCode;
                    reel.StepSequenceCode = stepSequenceCode;
                    this.UpdateReel(reel);
                    AddMachineFeederPassNoFeeder(mo, null, reel, feederMaterial, userCode, null, SMTLoadFeederOperationType.Continue, resourceCode, stepSequenceCode, string.Empty, string.Empty, FormatHelper.StringToBoolean(machineFeeder[i].StationEnabled));
                }
                // ɾ��ԭ�����ϼ�¼
                this.DeleteMachineFeeder(machineFeeder[i]);
            }
            // ��Դ��������ΪʧЧ
            strSql = "UPDATE tblMachineFeeder SET Enabled='0' WHERE MOCode='" + moCodeFrom + "' AND SSCode='" + ssCodeFrom + "' ";
            this.DataProvider.CustomExecute(new SQLCondition(strSql));
            msg.Add(new Message(MessageType.Success, "$TransferMaterialByMO_Success"));
            return msg;
        }

        /// <summary>
        /// ���ϣ����ܿ�feeder
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="machineCode"></param>
        /// <param name="stationCode"></param>
        /// <param name="reelNo"></param>
        /// <param name="feederCode"></param>
        /// <param name="userCode"></param>
        /// <param name="tblLoaded"></param>
        /// <param name="resourceCode"></param>
        /// <param name="stepSequenceCode"></param>
        /// <returns></returns>
        public Messages SMTLoadFeederNoFeeder(MO mo, string machineCode, string stationCode, string reelNo, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode)
        {
            return SMTLoadFeederNoFeeder(mo, machineCode, stationCode, reelNo, feederCode, userCode, tblLoaded, resourceCode, stepSequenceCode, true, string.Empty);
        }
        public Messages SMTLoadFeederNoFeeder(MO mo, string machineCode, string stationCode, string reelNo, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, bool stationEnabled, string tableGroup)
        {
            Messages msg = SMTLoadFeederCoreNoFeeder(mo, machineCode, stationCode, reelNo, feederCode, userCode, tblLoaded, resourceCode, stepSequenceCode, stationEnabled, tableGroup);
            if (msg.IsSuccess() == false)
            {
                this.AddMachineFeederFailure(mo, machineCode, stationCode, reelNo, feederCode, msg.OutPut(), userCode, tblLoaded, SMTLoadFeederOperationType.Load, resourceCode, stepSequenceCode);
            }
            return msg;
        }


        /// <summary>
        /// ���ܿ�feeder��added by Gawain@20130423
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="machineCode"></param>
        /// <param name="stationCode"></param>
        /// <param name="reelNo"></param>
        /// <param name="feederCode"></param>
        /// <param name="userCode"></param>
        /// <param name="tblLoaded"></param>
        /// <param name="resourceCode"></param>
        /// <param name="stepSequenceCode"></param>
        /// <param name="stationEnabled"></param>
        /// <param name="tableGroup"></param>
        /// <returns></returns>
        private Messages SMTLoadFeederCoreNoFeeder(MO mo, string machineCode, string stationCode, string reelNo, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, bool stationEnabled, string tableGroup)
        {
            Messages msg = new Messages();
            // ����Ͼ�
            Reel reel = (Reel)this.GetReel(reelNo);
            if (reel == null)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Not_Exist"));
                return msg;
            }
            if (FormatHelper.StringToBoolean(reel.UsedFlag) == false || reel.MOCode != mo.MOCode)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_UnGetOut_Or_MOCode_Error"));
                return msg;
            }
            // ����Ͼ��Ƿ�������̨ʹ��
            string strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND (ReelNo='" + reel.ReelNo + "' OR NextReelNo='" + reel.ReelNo + "') ";
            object[] objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objTmp != null && objTmp.Length > 0)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Used_Already"));
                return msg;
            }

            /*
            // ���Feeder
            Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            if (feeder == null)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Not_Exist"));
                return msg;
            }
            // Feeder�Ƿ�����
            if (FormatHelper.StringToBoolean(feeder.UseFlag) == false)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Not_GetOut"));
                return msg;
            }
            // Feeder���õĹ���
            if (feeder.MOCode != mo.MOCode)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Not_In_This_MOCode"));
                return msg;
            }
            // Feeder״̬
            if (feeder.Status != FeederStatus.Normal)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Status_Error"));
                return msg;
            }
            // ���Feeder�Ƿ�������̨ʹ��
            strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND FeederCode='" + feeder.FeederCode + "'";
            objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objTmp != null && objTmp.Length > 0)
            {
                msg.Add(new Message(MessageType.Error, "$Feeder_Used_Already"));
                return msg;
            }

            */


            // ��ѯ��վ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + stationCode + "' AND MaterialCode='" + reel.PartNo + "' AND SSCode='" + stepSequenceCode + "'";
            if (tableGroup != null && tableGroup != string.Empty)
            {
                strSql += " AND TblGrp='" + tableGroup + "' ";
            }
            object[] smtFeeders = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (smtFeeders == null)
            {
                msg.Add(new Message(MessageType.Error, "$SMTFeederMaterial_Not_Exist"));
                return msg;
            }
            SMTFeederMaterial smtFeeder = (SMTFeederMaterial)smtFeeders[0];

            #region Removed
            /*
			// ��ѯ�Ͼ�ǰʹ������
			strSql = "SELECT * FROM tblReelQty WHERE ReelNo='" + reelNo + "' AND MOCode='" + mo.MOCode + "'";
			object[] reelQtys = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
			if (reelQtys != null)
			{
				decimal d = 0;
				for (int i = 0; i < reelQtys.Length; i++)
				{
					ReelQty reelQty = (ReelQty)reelQtys[i];
					d += reelQty.UsedQty;
				}
				if (d > 0)
				{
					strSql = "UPDATE tblReel SET UsedQty=UsedQty+" + d.ToString() + " WHERE ReelNo='" + reelNo + "'";
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					strSql = "DELETE FROM tblReelQty WHERE ReelNo='" + reelNo + "' AND MOCode='" + mo.MOCode + "'";
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					reel.UsedQty += d;
				}
			}
			*/
            #endregion

            // ����Ͼ�ʣ����������վһ��ʹ������
            decimal dReelAlertQty = this.GetReelAlertQty(reel);
            if (reel.UsedQty + smtFeeder.Qty > dReelAlertQty)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_UsedQty_Alert"));
                return msg;
            }
            // ���վλ�Ƿ����Ϲ���
            for (int i = 0; i < tblLoaded.Rows.Count; i++)
            {
                if (tblLoaded.Rows[i]["MachineCode"].ToString() == machineCode &&
                    tblLoaded.Rows[i]["MachineStationCode"].ToString() == stationCode &&
                    FormatHelper.StringToBoolean(tblLoaded.Rows[i]["CheckResult"].ToString()) == true)
                {
                    msg.Add(new Message(MessageType.Error, "$MachineStation_Loaded_Already"));
                    return msg;
                }
            }
            /*
            // ����Ƿ������������ϱȶ�
            strSql = "SELECT COUNT(*) FROM tblMachineFeeder WHERE CheckResult='1' AND MOCode='" + mo.MOCode + "'";
            if (this.DataProvider.GetCount(new SQLCondition(strSql)) == 0)
            {
                strSql = "SELECT * FROM tblSMTCheckMaterial WHERE CheckID=(SELECT MAX(CheckID) FROM tblSMTCheckMaterial WHERE MOCode='" + mo.MOCode + "')";
                object[] objschk = this.DataProvider.CustomQuery(typeof(SMTCheckMaterial), new SQLCondition(strSql));
                bool bPass = false;
                if (objschk != null && objschk.Length > 0)
                {
                    SMTCheckMaterial chk = (SMTCheckMaterial)objschk[0];
                    if (chk.CheckResult == SMTCheckMaterialResult.Accept_Exception ||
                        chk.CheckResult == SMTCheckMaterialResult.Matchable)
                    {
                        bPass = true;
                    }
                }
                if (bPass == false)
                {
                    msg.Add(new Message(MessageType.Error, "$MOCode_Not_Check_SMTMaterial_MOBOM"));
                    return msg;
                }
            }
            */
            // ���ӻ�̨Feeder
            AddMachineFeederPassNoFeeder(mo, null, reel, smtFeeder, userCode, tblLoaded, SMTLoadFeederOperationType.Load, resourceCode, stepSequenceCode, string.Empty, string.Empty, stationEnabled);
            return msg;
        }


        /// <summary>
        /// SMT����,���ܿ�feeder
        /// </summary>
        public Messages SMTExchangeFeederNoFeeder(string operationType, MO mo, string machineCode, string stationCode, string reelNoOld, string reelNo, string feederCodeOld, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, string tableGroup)
        {
            Messages msg = SMTExchangeFeederCoreNoFeeder(operationType, mo, machineCode, stationCode, reelNoOld, reelNo, feederCodeOld, feederCode, userCode, tblLoaded, resourceCode, stepSequenceCode, tableGroup);
            if (msg.IsSuccess() == false)
            {
                this.AddMachineFeederFailure(mo, machineCode, stationCode, reelNo, feederCode, msg.OutPut(), userCode, tblLoaded, operationType, resourceCode, stepSequenceCode);
            }
            return msg;
        }
        private Messages SMTExchangeFeederCoreNoFeeder(string operationType, MO mo, string machineCode, string stationCode, string reelNoOld, string reelNo, string feederCodeOld, string feederCode, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, string tableGroup)
        {
            Messages msg = new Messages();
            // ���ԭ�����Ƿ���ȷ
            int iOldIdx = -1;
            if (tblLoaded != null)
            {
                for (int i = 0; i < tblLoaded.Rows.Count; i++)
                {
                    if (tblLoaded.Rows[i]["MachineCode"].ToString() == machineCode &&
                        tblLoaded.Rows[i]["MachineStationCode"].ToString() == stationCode &&
                        tblLoaded.Rows[i]["ReelNo"].ToString() != string.Empty &&
                        FormatHelper.StringToBoolean(tblLoaded.Rows[i]["CheckResult"].ToString()) == true)
                    {
                        //if (tblLoaded.Rows[i]["FeederCode"].ToString() != feederCodeOld)
                        //{
                        //    msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_FeederCode_Not_Exist [" + feederCodeOld + "]"));
                        //    return msg;
                        //}
                        if (tblLoaded.Rows[i]["ReelNo"].ToString() != reelNoOld)
                        {
                            msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_ReelNo_Not_Exist [" + reelNoOld + "]"));
                            return msg;
                        }
                        iOldIdx = i;
                        break;
                    }
                }
            }
            if (iOldIdx == -1)
            {
                msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Not_Exist"));
                return msg;
            }
            // ����Ͼ�
            Reel reel = (Reel)this.GetReel(reelNo);
            if (reel == null)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Not_Exist"));
                return msg;
            }
            if (reelNo != reelNoOld)
            {
                if (FormatHelper.StringToBoolean(reel.UsedFlag) == false || reel.MOCode != mo.MOCode)
                {
                    msg.Add(new Message(MessageType.Error, "$Reel_UnGetOut_Or_MOCode_Error"));
                    return msg;
                }
            }
            string strSql = string.Empty;
            object[] objTmp = null;
            // ����Ͼ��Ƿ�������̨ʹ��
            if (reel.ReelNo != reelNoOld)
            {
                strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND (ReelNo='" + reel.ReelNo + "' OR NextReelNo='" + reel.ReelNo + "') ";
                objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
                if (objTmp != null && objTmp.Length > 0)
                {
                    msg.Add(new Message(MessageType.Error, "$Reel_Used_Already"));
                    return msg;
                }
            }

            //// ���Feeder
            //Feeder feeder = (Feeder)this.GetFeeder(feederCode);
            //if (feeder == null)
            //{
            //    msg.Add(new Message(MessageType.Error, "$Feeder_Not_Exist"));
            //    return msg;
            //}
            //// ���Feeder����Ƿ�һ��
            //bool bFeederSpecSame = true;
            //if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == true)
            //{
            //    if (feeder.FeederSpecCode != tblLoaded.Rows[iOldIdx]["FeederSpecCode"].ToString())
            //    {
            //        bFeederSpecSame = false;
            //        /*
            //        msg.Add(new Message(MessageType.Error, "$New_Feeder_SpecCode_Error"));
            //        return msg;
            //        */
            //    }
            //}

            //// Feeder�Ƿ�����
            //if (feederCode != feederCodeOld)
            //{
            //    if (FormatHelper.StringToBoolean(feeder.UseFlag) == false)
            //    {
            //        msg.Add(new Message(MessageType.Error, "$Feeder_Not_GetOut"));
            //        return msg;
            //    }
            //}
            //// Feeder���õĹ���
            //if (feeder.MOCode != mo.MOCode)
            //{
            //    msg.Add(new Message(MessageType.Error, "$Feeder_Not_In_This_MOCode"));
            //    return msg;
            //}
            //// Feeder״̬
            //if (feeder.Status != FeederStatus.Normal)
            //{
            //    msg.Add(new Message(MessageType.Error, "$Feeder_Status_Error"));
            //    return msg;
            //}
            //// ���Feeder�Ƿ�������̨ʹ��
            //if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == false ||
            //    feeder.FeederCode != feederCodeOld)
            //{
            //    strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND FeederCode='" + feeder.FeederCode + "'";
            //    objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            //    if (objTmp != null && objTmp.Length > 0)
            //    {
            //        msg.Add(new Message(MessageType.Error, "$Feeder_Used_Already"));
            //        return msg;
            //    }
            //}


            // ������Ͼ���ԭ�Ͼ��Ϻ��Ƿ�һ��
            bool bMaterialSame = true;
            if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == true)
            {
                if (reel.PartNo != tblLoaded.Rows[iOldIdx]["MaterialCode"].ToString())
                {
                    bMaterialSame = false;
                }
            }
            // ��ѯ��վ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + stationCode + "' AND MaterialCode='" + reel.PartNo + "' AND SSCode='" + stepSequenceCode + "'";
            if (tableGroup != null && tableGroup != string.Empty)
            {
                strSql += " AND TblGrp='" + tableGroup + "' ";
            }
            object[] smtFeeders = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (smtFeeders == null)
            {
                msg.Add(new Message(MessageType.Error, "$SMTFeederMaterial_Not_Exist"));
                return msg;
            }
            SMTFeederMaterial smtFeeder = (SMTFeederMaterial)smtFeeders[0];

            if (reelNo == reelNoOld)
            {
                // ��ѯ�Ͼ�ǰʹ������
                strSql = "SELECT * FROM tblReelQty WHERE ReelNo='" + reel.ReelNo + "' AND MOCode='" + mo.MOCode + "'";
                object[] reelQtys = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
                if (reelQtys != null)
                {
                    decimal d = 0;
                    for (int i = 0; i < reelQtys.Length; i++)
                    {
                        ReelQty reelQty = (ReelQty)reelQtys[i];
                        d += reelQty.UsedQty;
                    }
                    if (d > 0)
                    {
                        reel.UsedQty += d;
                    }
                }
            }

            if (reelNo != reelNoOld)
            {
                // ����Ͼ�ʣ����������վһ��ʹ������
                decimal dReelAlertQty = this.GetReelAlertQty(reel);
                if (reel.UsedQty + smtFeeder.Qty > dReelAlertQty)
                {
                    msg.Add(new Message(MessageType.Error, "$Reel_UsedQty_Alert"));
                    return msg;
                }
            }
            // ���»����Ͼ��ʹ������
            if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == true)
            {
                UpdateMachineFeederReelQty(mo.MOCode, machineCode, stationCode, reelNoOld, operationType, userCode);
            }
            // ���ӻ�̨Feeder
            msg.AddMessages(AddMachineFeederPassNoFeeder(mo, null, reel, smtFeeder, userCode, tblLoaded, operationType, resourceCode, stepSequenceCode, feederCodeOld, reelNoOld));
            // ����INNO @20060823
            if (reel.ReelNo != reelNoOld)
            {
                int iEnabled = this.DataProvider.GetCount(new SQLCondition("SELECT COUNT(*) FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND SSCode='" + stepSequenceCode + "' AND Enabled='1'"));
                if (iEnabled > 0)
                {
                    this.GenerateMachineInno(mo.MOCode, stepSequenceCode, machineCode, userCode);
                }
            }
            //
            //if (bFeederSpecSame == false)
            //{
            //    msg.Add(new Message("$SMTLoad_NewReel_OldFeederSpec_Diff"));
            //}
            if (bMaterialSame == false)
            {
                msg.Add(new Message("$SMTLoad_NewReel_OldReel_Diff"));
            }
            return msg;
        }


        /// <summary>
        /// ���ܿ�feeder��added by Gawain.Gu@20130423
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="feeder"></param>
        /// <param name="reel"></param>
        /// <param name="smtFeeder"></param>
        /// <param name="userCode"></param>
        /// <param name="tblLoaded"></param>
        /// <param name="operationType"></param>
        /// <param name="resourceCode"></param>
        /// <param name="stepSequenceCode"></param>
        /// <param name="oldFeederCode"></param>
        /// <param name="oldReelNo"></param>
        /// <returns></returns>
        public Messages AddMachineFeederPassNoFeeder(MO mo, Feeder feeder, Reel reel, SMTFeederMaterial smtFeeder, string userCode, DataTable tblLoaded, string operationType, string resourceCode, string stepSequenceCode, string oldFeederCode, string oldReelNo)
        {
            return AddMachineFeederPassNoFeeder(mo, feeder, reel, smtFeeder, userCode, tblLoaded, operationType, resourceCode, stepSequenceCode, oldFeederCode, oldReelNo, null);
        }

        /// <summary>
        /// ���ܿ�feeder��added by Gawain.Gu@20130423
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="feeder"></param>
        /// <param name="reel"></param>
        /// <param name="smtFeeder"></param>
        /// <param name="userCode"></param>
        /// <param name="tblLoaded"></param>
        /// <param name="operationType"></param>
        /// <param name="resourceCode"></param>
        /// <param name="stepSequenceCode"></param>
        /// <param name="oldFeederCode"></param>
        /// <param name="oldReelNo"></param>
        /// <param name="stationEnabled"></param>
        /// <returns></returns>
        public Messages AddMachineFeederPassNoFeeder(MO mo, Feeder feeder, Reel reel, SMTFeederMaterial smtFeeder, string userCode, DataTable tblLoaded, string operationType, string resourceCode, string stepSequenceCode, string oldFeederCode, string oldReelNo, object stationEnabled)
        {
            Messages msg = new Messages();

            //if (feeder.FeederCode != oldFeederCode)
            //{
            //    feeder.CurrentUnitQty = smtFeeder.Qty;
            //    this.UpdateFeeder(feeder);
            //}

            bool bIsNew = true;
            MachineFeeder machineFeeder = new MachineFeeder();
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND MachineCode='" + smtFeeder.MachineCode + "' AND MachineStationCode='" + smtFeeder.MachineStationCode + "' AND TblGrp='" + smtFeeder.TableGroup + "' AND SSCode='" + stepSequenceCode + "' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                machineFeeder = (MachineFeeder)objs[0];
                bIsNew = false;
            }
            if (machineFeeder.ProductCode != string.Empty && operationType == SMTLoadFeederOperationType.Continue)
            {
                if (machineFeeder.NextReelNo != string.Empty)
                {
                    msg.Add(new Message(MessageType.Error, "$SMT_LoadFeeder_Continue_Exist_NextReel"));
                    return msg;
                }
                machineFeeder.NextReelNo = reel.ReelNo;
            }
            else
            {
                // ֻ��Feeder�����
                if (operationType == SMTLoadFeederOperationType.Exchange &&
                    (machineFeeder.ReelNo == reel.ReelNo || machineFeeder.NextReelNo == reel.ReelNo) &&
                    machineFeeder.FeederCode != "DEFAULTFEEDER")//feeder.FeederCode)
                { }
                else
                {
                    // �������Ͼ�����
                    if (oldReelNo != string.Empty && machineFeeder.NextReelNo == oldReelNo)
                    {
                        machineFeeder.NextReelNo = reel.ReelNo;
                    }
                    else
                    {
                        machineFeeder.ReelNo = reel.ReelNo;
                        machineFeeder.MaterialCode = reel.PartNo;
                        machineFeeder.UnitQty = smtFeeder.Qty;
                        machineFeeder.LotNo = reel.LotNo;
                        machineFeeder.DateCode = reel.DateCode;
                    }
                }
            }
            //2006/11/17,Laws Lu add get DateTime from db Server
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

            DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);

            machineFeeder.ProductCode = mo.ItemCode;
            machineFeeder.StepSequenceCode = stepSequenceCode;
            machineFeeder.MOCode = mo.MOCode;
            machineFeeder.MachineCode = smtFeeder.MachineCode;
            machineFeeder.MachineStationCode = smtFeeder.MachineStationCode;
            machineFeeder.FeederSpecCode = "DEFAULTFEEDERSPEC";// feeder.FeederSpecCode;
            machineFeeder.FeederCode = "DEFAULTFEEDER";// feeder.FeederCode;
            machineFeeder.LoadUser = userCode;
            machineFeeder.LoadDate = FormatHelper.TODateInt(dtNow);
            machineFeeder.LoadTime = FormatHelper.TOTimeInt(dtNow);
            machineFeeder.CheckResult = FormatHelper.BooleanToString(true);
            machineFeeder.OpeResourceCode = resourceCode;
            machineFeeder.OpeStepSequenceCode = stepSequenceCode;
            machineFeeder.TableGroup = smtFeeder.TableGroup;
            machineFeeder.MaintainUser = userCode;

            machineFeeder.MaintainDate = FormatHelper.TODateInt(dtNow);
            machineFeeder.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            // ���������������������Enabled״̬������ӵ�����Ҳ��Enabled
            int iEnabled = this.DataProvider.GetCount(new SQLCondition("SELECT COUNT(*) FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND SSCode='" + stepSequenceCode + "' AND CheckResult='1' AND Enabled='1'"));
            if (iEnabled > 0)
                machineFeeder.Enabled = FormatHelper.BooleanToString(true);
            else
                machineFeeder.Enabled = FormatHelper.BooleanToString(false);
            if (stationEnabled != null)
            {
                machineFeeder.StationEnabled = FormatHelper.BooleanToString(Convert.ToBoolean(stationEnabled));
            }
            if (bIsNew == false)
            {
                // ���߿��Ƽ��
                if (operationType == SMTLoadFeederOperationType.Continue ||
                    operationType == SMTLoadFeederOperationType.Exchange)
                {
                    if (FormatHelper.StringToBoolean(machineFeeder.ReelCeaseFlag) == true)
                    {
                        msg.AddMessages(this.AddStartLineLog(machineFeeder, operationType, userCode));
                        if (msg.GetData() != null && msg.GetData().Values != null && msg.GetData().Values.Length > 0)
                        {
                            SMTLineControlLog lineLog = (SMTLineControlLog)msg.GetData().Values[0];
                            if (FormatHelper.StringToBoolean(lineLog.LineStatus) == true)
                            {
                                machineFeeder.ReelCeaseFlag = FormatHelper.BooleanToString(false);
                            }
                        }
                    }
                }
                this.UpdateMachineFeeder(machineFeeder);
            }
            else
            {
                machineFeeder.ReelCeaseFlag = FormatHelper.BooleanToString(false);
                this.AddMachineFeeder(machineFeeder);
            }
            this.AddMachineFeederLog(machineFeeder, operationType, resourceCode, stepSequenceCode, "DEFAULTFEEDER", reel.ReelNo, "DEFAULTFEEDER", oldReelNo, reel);

            ReelQty reelQty = new ReelQty();
            reelQty.MOCode = mo.MOCode;
            reelQty.MachineCode = machineFeeder.MachineCode;
            reelQty.MachineStationCode = machineFeeder.MachineStationCode;
            reelQty.FeederCode = "DEFAULTFEEDER";// feeder.FeederCode;
            reelQty.FeederSpecCode = "DEFAULTFEEDERSPEC"; //feeder.FeederSpecCode;
            reelQty.ReelNo = reel.ReelNo;
            reelQty.Qty = reel.Qty;
            reelQty.UsedQty = 0;
            reelQty.UpdatedQty = reel.UsedQty;
            reelQty.UnitQty = machineFeeder.UnitQty;
            reelQty.AlertQty = reelQty.Qty;
            reelQty.MaterialCode = reel.PartNo;
            reelQty.StepSequenceCode = stepSequenceCode;
            reelQty.MaintainUser = userCode;
            reelQty.MaintainDate = FormatHelper.TODateInt(dtNow);
            reelQty.MaintainTime = FormatHelper.TOTimeInt(dtNow);
            this.AddReelQty(reelQty);

            if (tblLoaded != null)
            {
                int tableRowIdx = -1;
                if (tblLoaded != null)
                {
                    for (int i = 0; i < tblLoaded.Rows.Count; i++)
                    {
                        if (tblLoaded.Rows[i]["MachineCode"].ToString() == smtFeeder.MachineCode &&
                            tblLoaded.Rows[i]["MachineStationCode"].ToString() == smtFeeder.MachineStationCode &&
                            tblLoaded.Rows[i]["MaterialCode"].ToString() == smtFeeder.MaterialCode)
                        {
                            tableRowIdx = i;
                            break;
                        }
                    }
                }
                DataRow row = tblLoaded.NewRow();
                if (tableRowIdx >= 0)
                {
                    row = tblLoaded.Rows[tableRowIdx];
                }
                row["MachineCode"] = machineFeeder.MachineCode;
                row["MachineStationCode"] = machineFeeder.MachineStationCode;
                row["FeederCode"] = machineFeeder.FeederCode;
                row["FeederSpecCode"] = machineFeeder.FeederSpecCode;
                row["FeederLeftCount"] = 0;// feeder.MaxCount - feeder.UsedCount;
                if (machineFeeder.NextReelNo != null && machineFeeder.NextReelNo != string.Empty)
                    row["ReelNo"] = machineFeeder.NextReelNo;
                else
                    row["ReelNo"] = machineFeeder.ReelNo;
                row["ReelLeftQty"] = reel.Qty - reel.UsedQty;
                row["MaterialCode"] = machineFeeder.MaterialCode;
                row["LoadUser"] = machineFeeder.LoadUser;
                row["LoadDate"] = FormatHelper.ToDateString(machineFeeder.LoadDate);
                row["LoadTime"] = FormatHelper.ToTimeString(machineFeeder.LoadTime);
                row["CheckResult"] = FormatHelper.BooleanToString(true);
                row["FailReason"] = "";
                if (tableRowIdx < 0)
                {
                    tblLoaded.Rows.Add(row);
                }
            }
            return msg;
        }

        /// <summary>
        /// ����,���ܿ�feeder
        /// </summary>
        public Messages SMTContinueNoFeeder(string operationType, MO mo, string machineCode, string stationCode, string reelNoOld, string reelNo, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, string tableGroup)
        {
            Messages msg = new Messages();
            // ���ԭ�����Ƿ���ȷ
            int iOldIdx = -1;
            if (tblLoaded != null)
            {
                for (int i = 0; i < tblLoaded.Rows.Count; i++)
                {
                    if (tblLoaded.Rows[i]["MachineCode"].ToString() == machineCode &&
                        tblLoaded.Rows[i]["MachineStationCode"].ToString() == stationCode &&
                        tblLoaded.Rows[i]["ReelNo"].ToString() != string.Empty &&
                        FormatHelper.StringToBoolean(tblLoaded.Rows[i]["CheckResult"].ToString()) == true)
                    {
                        if (tblLoaded.Rows[i]["ReelNo"].ToString() != reelNoOld)
                        {
                            msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_ReelNo_Not_Exist [" + reelNoOld + "]"));
                            return msg;
                        }
                        iOldIdx = i;
                        break;
                    }
                }
            }
            if (iOldIdx == -1)
            {
                msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Not_Exist"));
                return msg;
            }
            // ����Ͼ�
            Reel reel = (Reel)this.GetReel(reelNo);
            if (reel == null)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Not_Exist"));
                return msg;
            }
            if (FormatHelper.StringToBoolean(reel.UsedFlag) == false || reel.MOCode != mo.MOCode)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_UnGetOut_Or_MOCode_Error"));
                return msg;
            }
            // ����Ͼ��Ƿ�������̨ʹ��
            string strSql = "SELECT * FROM tblMachineFeeder WHERE CheckResult='1' AND (ReelNo='" + reel.ReelNo + "' OR NextReelNo='" + reel.ReelNo + "') ";
            object[] objTmp = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objTmp != null && objTmp.Length > 0)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_Used_Already"));
                return msg;
            }
            // ������Ͼ���ԭ�Ͼ��Ϻ��Ƿ�һ��
            bool bMaterialSame = true;
            if (FormatHelper.StringToBoolean(tblLoaded.Rows[iOldIdx]["CheckResult"].ToString()) == true)
            {
                if (reel.PartNo != tblLoaded.Rows[iOldIdx]["MaterialCode"].ToString())
                {
                    bMaterialSame = false;
                    /*
                    msg.Add(new Message(MessageType.Error, "$New_Reel_Material_Error"));
                    return msg;
                    */
                }
            }

            //// ���Feeder
            //Feeder feeder = (Feeder)this.GetFeeder(tblLoaded.Rows[iOldIdx]["FeederCode"].ToString());
            //if (feeder == null)
            //{
            //    msg.Add(new Message(MessageType.Error, "$Feeder_Not_Exist"));
            //    return msg;
            //}

            // ��ѯ��վ��
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + stationCode + "'  AND MaterialCode='" + reel.PartNo + "' AND SSCode='" + stepSequenceCode + "'";
            if (tableGroup != null && tableGroup != string.Empty)
            {
                strSql += " AND TblGrp='" + tableGroup + "' ";
            }
            object[] smtFeeders = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (smtFeeders == null)
            {
                msg.Add(new Message(MessageType.Error, "$SMTFeederMaterial_Not_Exist"));
                return msg;
            }
            SMTFeederMaterial smtFeeder = (SMTFeederMaterial)smtFeeders[0];

            #region Removed
            /*
			// ��ѯ�Ͼ�ǰʹ������
			strSql = "SELECT * FROM tblReelQty WHERE ReelNo='" + reel.ReelNo + "' AND MOCode='" + mo.MOCode + "'";
			object[] reelQtys = this.DataProvider.CustomQuery(typeof(ReelQty), new SQLCondition(strSql));
			if (reelQtys != null)
			{
				decimal d = 0;
				for (int i = 0; i < reelQtys.Length; i++)
				{
					ReelQty reelQty = (ReelQty)reelQtys[i];
					d += reelQty.UsedQty;
				}
				if (d > 0)
				{
					strSql = "UPDATE tblReel SET UsedQty=UsedQty+" + d.ToString() + " WHERE ReelNo='" + reel.ReelNo + "'";
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					strSql = "DELETE FROM tblReelQty WHERE ReelNo='" + reel.ReelNo + "' AND MOCode='" + mo.MOCode + "'";
					this.DataProvider.CustomExecute(new SQLCondition(strSql));
					reel.UsedQty += d;
				}
			}
			*/
            #endregion

            // ����Ͼ�ʣ����������վһ��ʹ������
            decimal dReelAlertQty = this.GetReelAlertQty(reel);
            if (reel.UsedQty + smtFeeder.Qty > dReelAlertQty)
            {
                msg.Add(new Message(MessageType.Error, "$Reel_UsedQty_Alert"));
                return msg;
            }
            strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND MachineCode='" + smtFeeder.MachineCode + "' AND MachineStationCode='" + smtFeeder.MachineStationCode + "'";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs != null && objs.Length > 0)
            {
                MachineFeeder machineFeeder = (MachineFeeder)objs[0];
                if (FormatHelper.StringToBoolean(machineFeeder.Enabled) == false)
                {
                    msg.Add(new Message(MessageType.Error, "$SMT_Continue_MO_Disabled"));
                    return msg;
                }
            }
            // ���ӻ�̨Feeder
            msg.AddMessages(AddMachineFeederPassNoFeeder(mo, null, reel, smtFeeder, userCode, tblLoaded, operationType, resourceCode, stepSequenceCode, string.Empty, reelNoOld));
            if (bMaterialSame == false && msg.IsSuccess() == true)
            {
                msg.Add(new Message("$SMTLoad_NewReel_OldReel_Diff"));
            }
            return msg;
        }


        /// <summary>
        /// ���ϣ����ܿ�feeder
        /// </summary>
        public Messages SMTUnLoadFeederSingleNoFeeder(MO mo, string machineCode, string machineStationCode, string feederCode, string reelNo, string userCode, DataTable tblLoaded, string resourceCode, string stepSequenceCode, string tableGroup)
        {
            Messages msg = new Messages();
            // ��ѯ���ϼ�¼���Ƿ����
            string strSql = "SELECT * FROM tblMachineFeeder WHERE MOCode='" + mo.MOCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + machineStationCode + "' AND ReelNo='" + reelNo + "' AND FeederCode='" + feederCode + "' AND CheckResult='1' ";
            object[] objs = this.DataProvider.CustomQuery(typeof(MachineFeeder), new SQLCondition(strSql));
            if (objs == null || objs.Length == 0)
            {
                msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Not_Exist"));
                return msg;
            }
            MachineFeeder machineFeeder = (MachineFeeder)objs[0];

            Reel reel = (Reel)this.GetReel(reelNo);
            //Feeder feeder = (Feeder)this.GetFeeder(feederCode);

            // ��ѯ��վ�����Ƿ����
            strSql = "SELECT * FROM tblSMTFeederMaterial WHERE ProductCode='" + mo.ItemCode + "' AND MachineCode='" + machineCode + "' AND MachineStationCode='" + machineStationCode + "' AND MaterialCode='" + reel.PartNo + "' AND SSCode='" + stepSequenceCode + "'";
            if (tableGroup != null && tableGroup != string.Empty)
            {
                strSql += " AND TblGrp='" + tableGroup + "' ";
            }
            object[] smtFeeders = this.DataProvider.CustomQuery(typeof(SMTFeederMaterial), new SQLCondition(strSql));
            if (smtFeeders != null && smtFeeders.Length > 0)
            {
                msg.Add(new Message(MessageType.Error, "$SMTUnLoadFeeder_In_FeederMaterial"));
                return msg;
            }

            UpdateMachineFeederReelQty(mo.MOCode, machineCode, machineStationCode, reelNo, SMTLoadFeederOperationType.UnLoadSingle, userCode);
            this.DeleteMachineFeeder(machineFeeder);

            // ����DataTable
            if (tblLoaded != null)
            {
                for (int i = 0; i < tblLoaded.Rows.Count; i++)
                {
                    if (tblLoaded.Rows[i]["MachineCode"].ToString() == machineCode &&
                        tblLoaded.Rows[i]["MachineStationCode"].ToString() == machineStationCode &&
                        tblLoaded.Rows[i]["MaterialCode"].ToString() == machineFeeder.MaterialCode &&
                        FormatHelper.StringToBoolean(tblLoaded.Rows[i]["CheckResult"].ToString()) == true)
                    {
                        if (tblLoaded.Rows[i]["FeederCode"].ToString() != feederCode)
                        {
                            msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_FeederCode_Not_Exist [" + feederCode + "]"));
                            return msg;
                        }
                        if (tblLoaded.Rows[i]["ReelNo"].ToString() != reelNo)
                        {
                            msg.Add(new Message(MessageType.Error, "$SMTExchangeFeeder_Old_ReelNo_Not_Exist [" + reelNo + "]"));
                            return msg;
                        }
                        tblLoaded.Rows[i]["FeederCode"] = "";
                        tblLoaded.Rows[i]["FeederLeftCount"] = "";
                        tblLoaded.Rows[i]["ReelNo"] = "";
                        tblLoaded.Rows[i]["ReelLeftQty"] = "";
                        tblLoaded.Rows[i]["LoadUser"] = "";
                        tblLoaded.Rows[i]["LoadDate"] = "";
                        tblLoaded.Rows[i]["LoadTime"] = "";
                        break;
                    }
                }
            }

            return msg;
        }


        #endregion
    }
}

