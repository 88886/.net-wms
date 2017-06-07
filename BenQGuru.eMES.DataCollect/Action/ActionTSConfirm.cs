#region system;
using System;
using UserControl;
#endregion

#region project
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.TS;
#endregion



namespace  BenQGuru.eMES.DataCollect.Action
{
	/// <summary>
	/// ���������ɼ�
	/// </summary>
	public class ActionTSConfirm: IAction
	{
		private IDomainDataProvider _domainDataProvider = null;

		//		public ActionTSConfirm()
		//		{	
		//		}

		public ActionTSConfirm(IDomainDataProvider domainDataProvider)
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

		/// <summary>
		/// ** ��������:	ȷ�����̿����кŵ�ά��
		/// ** �� ��:		crystal chu
		/// ** �� ��:		2005/07/26/
		/// ** �� ��:
		/// ** �� ��:
		/// ** nunit
		/// </summary>
		/// <param name="actionEventArgs"> </param> 
		/// <returns></returns>
		public Messages Execute(ActionEventArgs actionEventArgs)
		{				
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
				//���res�ڲ���OPTS��
				messages.AddMessages(dataCollect.CheckResourceInOperationTS(actionEventArgs));

				//Laws Lu,2006/11/13 uniform system collect date
				DBDateTime dbDateTime;
				//Laws Lu,2006/11/13 uniform system collect date
				if(actionEventArgs.ProductInfo != null && actionEventArgs.ProductInfo.WorkDateTime != null)
				{
					dbDateTime = actionEventArgs.ProductInfo.WorkDateTime;
				}
				else
				{
					dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
					if(actionEventArgs.ProductInfo != null)
					{
						actionEventArgs.ProductInfo.WorkDateTime = dbDateTime;
					}
							
				}

				DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);
				if (messages.IsSuccess())
				{	
					//Laws Lu,2005/09/16,�޸�	�����߼�
					TSFacade tsFacade = new TSFacade(this.DataProvider);
					//					if( !tsFacade.IsCardInTS(actionEventArgs.RunningCard))
					//					{
					//						messages.Add(new Message(MessageType.Error,"$CSError_Card_Not_In_TS"));
					//					}
					//Laws Lu��2005/10/16���ڶ���1�е��Ѿ����Ĳ�Ʒ���кţ�
					//����ϵͳ�п���ʵ�ֽ��ò�Ʒ���к����¹���������һ������2��
					//���Ǹò�Ʒ���к��ڹ���2�ɼ��겻����
					//��������ҵʱ��ϵͳ���������Ϣ��
					//�ò�Ʒ״̬Ϊ����⡱״̬�����¸ò�Ʒ���к��޷�����ά�ޡ�
					object obj= tsFacade.GetCardLastTSRecordInNewStatus(actionEventArgs.RunningCard);

					if(obj == null)
					{
						//Laws Lu,2006,07/05 Support RMA Repair
						//						if(actionEventArgs.IsRMA)
						//						{
						MOFacade moFAC = new MOFacade(DataProvider);
						object objRMA = moFAC.GetRepairRMARCARDByRcard(actionEventArgs.RunningCard);
						if(objRMA != null)
						{
							#region ����ά����Ϣ

							RMARCARD rma = objRMA as RMARCARD ;
							Domain.TS.TS tS = tsFacade.CreateNewTS();
							tS.TSId = FormatHelper.GetUniqueID((rma.REWORKMOCODE == String.Empty?rma.RMABILLNO:rma.REWORKMOCODE)
								,actionEventArgs.RunningCard,ActionOnLineHelper.StartSeq.ToString());

							tS.CardType = CardType.CardType_Product;;
							tS.FormTime = rma.MTIME;
							tS.FromDate = rma.MDATE;
							tS.FromInputType = TSFacade.TSSource_RMA;
							tS.FromUser = rma.MUSER;
							tS.FromMemo = rma.EATTRIBUTE1;
							//Laws Lu,2006/07/05 add support RMA
							tS.RMABillCode = rma.RMABILLNO;
							//����ʱ��͹�ҵ������Ϣ

							//Laws Lu,2005/11/09,����	��¼ShiftDay
							BaseSetting.BaseModelFacade dataModel = new BaseSetting.BaseModelFacade(this.DataProvider);
							Domain.BaseSetting.Resource res = (Domain.BaseSetting.Resource)dataModel.GetResource(actionEventArgs.ResourceCode);
							//onwip.SegmentCode				= productInfo.NowSimulationReport.SegmentCode;
							//2006/11/17,Laws Lu add get DateTime from db Server
						
							BaseSetting.ShiftModelFacade shiftModel	= new BaseSetting.ShiftModelFacade(this.DataProvider);
							Domain.BaseSetting.TimePeriod  period	= (Domain.BaseSetting.TimePeriod)shiftModel.GetTimePeriod(res.ShiftTypeCode,Web.Helper.FormatHelper.TOTimeInt(dtNow));		
							if (period==null)
							{
								throw new Exception("$OutOfPerid");
							}
				
							if ( period.IsOverDate == Web.Helper.FormatHelper.TRUE_STRING )
							{
								if ( period.TimePeriodBeginTime < period.TimePeriodEndTime )
								{
									tS.FromShiftDay =	FormatHelper.TODateInt(dtNow.AddDays(-1)) ;
								}
								else if ( Web.Helper.FormatHelper.TOTimeInt(dtNow) < period.TimePeriodBeginTime)
								{
									tS.FromShiftDay =	FormatHelper.TODateInt(dtNow.AddDays(-1)) ;
								}
								else
								{
									tS.FromShiftDay = FormatHelper.TODateInt(dtNow) ;
								}
							}
							else
							{
								tS.FromShiftDay = FormatHelper.TODateInt(dtNow) ;
							}

							tS.FromOPCode = "TS";
							tS.FromResourceCode = actionEventArgs.ResourceCode;
							tS.FromRouteCode = String.Empty;
							tS.FromSegmentCode = res.SegmentCode;
							tS.FromShiftCode = period.ShiftCode;/*Laws Lu,2006/03/11 ���������д����*/
							tS.FromShiftTypeCode = period.ShiftTypeCode;
							tS.FromStepSequenceCode = res.StepSequenceCode;
							tS.FromTimePeriodCode = period.TimePeriodCode;

							tS.ItemCode = rma.ITEMCODE;
							tS.MaintainDate = FormatHelper.TODateInt(dtNow) ;
							tS.MaintainTime = FormatHelper.TOTimeInt(dtNow) ;
							tS.MaintainUser = actionEventArgs.MaintainUser;

							tS.MOCode = (rma.REWORKMOCODE == String.Empty?rma.RMABILLNO:rma.REWORKMOCODE);
							tS.ModelCode = rma.MODELCODE;
							tS.RunningCard = actionEventArgs.RunningCard;
							tS.RunningCardSequence = ActionOnLineHelper.StartSeq;
							tS.SourceCard = actionEventArgs.RunningCard;
							tS.SourceCardSequence = ActionOnLineHelper.StartSeq;
							tS.TransactionStatus = TSFacade.TransactionStatus_None;

							tS.TranslateCard = actionEventArgs.RunningCard;
							tS.TranslateCardSequence = ActionOnLineHelper.StartSeq;
							//tS.TSStatus = TSStatus.TSStatus_New;
								
							tS.Week =(new ReportHelper(DataProvider)).WeekOfYear(tS.FromShiftDay.ToString());
							tS.Month = int.Parse(tS.FromShiftDay.ToString().Substring(4,2));
							tS.TSTimes = ActionOnLineHelper.StartNGTimes;

							tS.ConfirmUser = actionEventArgs.UserCode;
							tS.ConfirmDate = FormatHelper.TODateInt(dtNow);
							tS.ConfirmTime = FormatHelper.TOTimeInt(dtNow);
							tS.ConfirmOPCode = "TS";
							tS.ConfirmResourceCode  = actionEventArgs.ResourceCode;
							tS.TSStatus = TSStatus.TSStatus_Confirm;
                            // Added by Icyer 2007/07/03
                            MO mo = (MO)moFAC.GetMO(tS.MOCode);
                            if (mo != null)
                            {
                                tS.MOSeq = mo.MOSeq;
                            }
                            // Added end
								
							tsFacade.AddTS(tS);
							//Laws Lu,2006/07/07 add ,Success release than delete rma
							moFAC.DeleteRMARCARD(rma);
							#endregion
						}
						else
						{
							messages.Add(new Message(MessageType.Error,"$CSError_Card_Not_In_TS_OR_Status_Error"));
						}
						//						}
						//						else
						//						{
						//							messages.Add(new Message(MessageType.Error,"$CSError_Card_Not_In_TS_OR_Status_Error"));
						//						}
					}
					else
					{
						BenQGuru.eMES.Domain.TS.TS ts = (BenQGuru.eMES.Domain.TS.TS)obj;

						if(ts.TSStatus == TSStatus.TSStatus_New)
						{
							ts.ConfirmUser = actionEventArgs.UserCode;
							ts.ConfirmDate = FormatHelper.TODateInt(dtNow);
							ts.ConfirmTime = FormatHelper.TOTimeInt(dtNow);
							ts.ConfirmOPCode = "TS";
							ts.ConfirmResourceCode  = actionEventArgs.ResourceCode;
							ts.TSStatus = TSStatus.TSStatus_Confirm;
							
							//added by jessie lee, 2005/11/24,
							ts.MaintainUser = actionEventArgs.MaintainUser;
							ts.MaintainDate = FormatHelper.TODateInt(dtNow) ;
							ts.MaintainTime = FormatHelper.TOTimeInt(dtNow) ;

							tsFacade.UpdateTSConfirmStatus(ts);
							
						}
						else
						{
							messages.Add(new Message(MessageType.Error,"$CSError_Card_TSStatus_Error $Current_Status $"+ts.TSStatus));
						}
					}
				}
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}
	
	}
}
