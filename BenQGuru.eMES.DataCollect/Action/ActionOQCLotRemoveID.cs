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
using BenQGuru.eMES.OQC;
using BenQGuru.eMES.Domain.OQC;
#endregion


namespace BenQGuru.eMES.DataCollect.Action
{
	/// <summary>
	/// ���������ɼ�
	/// </summary>
	public class ActionOQCLotRemoveID:IAction
	{
		
		private IDomainDataProvider _domainDataProvider = null;

//		public ActionOQCLotRemoveID()
//		{	
//		}

		public ActionOQCLotRemoveID(IDomainDataProvider domainDataProvider)
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

		public Messages Execute(ActionEventArgs oqcLotRemoveIDEventArgs)
		{				
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			
			try
			{
				ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
				ActionOQCHelper actionOQCHelper = new ActionOQCHelper(this.DataProvider);
				OQCFacade oqcFacade = new OQCFacade(this.DataProvider);

				//�������״̬
				if( !actionOQCHelper.IsOQCLotInitial( ((OQCLotRemoveIDEventArgs)oqcLotRemoveIDEventArgs).OQCLotNO))
				{
					throw new Exception("$Error_OQCLotNOIsNotInitial");
				}
				//��дSIMULATION ��鹤����ID��;�̡�����
				messages.AddMessages( dataCollect.CheckID(oqcLotRemoveIDEventArgs));
				if (messages.IsSuccess())
				{				
					
					#region update FQCLotSize,Laws Lu,2005/10/24���޸�	
					object obj = oqcFacade.GetOQCLot(((OQCLotRemoveIDEventArgs)oqcLotRemoveIDEventArgs).OQCLotNO,OQCFacade.Lot_Sequence_Default);
					if(obj == null)
					{
						throw new Exception("$Error_OQCLotNotExisted");
					}
					OQCLot oqcLot = obj as OQCLot;
					oqcLot.LotSize = -1;//oqcFacade.GetOQCLotSizeFromOQCLot2Card( ((OQCLotRemoveIDEventArgs)oqcLotRemoveIDEventArgs).OQCLotNO);
					oqcFacade.UpdateOQCLotSize(oqcLot);
					#endregion

					#region OQCADDID ����ļ��

					//�������״̬
					if( actionOQCHelper.IsOQCLotComplete( ((OQCLotRemoveIDEventArgs)oqcLotRemoveIDEventArgs).OQCLotNO))
					{
						throw new Exception("$Error_OQCLotNOHasComplete");
					}
					#endregion

					//��nowsimulation ��actionlist��OqcAdd,oQCDelete,replaceΪ��
					oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.ActionList = oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.ActionList.Replace(ActionType.DataCollectAction_OQCLotAddID,""); 
					oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.ActionList = oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.ActionList.Replace(ActionType.DataCollectAction_OQCLotRemoveID,""); 


					oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.LOTNO = String.Empty;
					/* added by jessie lee, 2006/6/20
					 * Power0063:��װʱ������ӳ������ɾ��һ����Ʒ��Ӧ�ôӰ�װ����ɾ���˲�Ʒ�� */
					string cartoncode = string.Empty;
					if( oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.CartonCode !=null 
						&& oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.CartonCode.Length>0)
					{
						cartoncode = oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.CartonCode;
						oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.CartonCode = string.Empty;
					}

					messages.AddMessages( dataCollect.Execute(oqcLotRemoveIDEventArgs));
					
					if (messages.IsSuccess())
					{
						//ɾ����ӦOQCLot2Card,��������һ����ɾ����ӦOQCLot
						#region OQC
						object objLot2Card = oqcFacade.GetOQCLot2Card(oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.RunningCard, oqcLotRemoveIDEventArgs.ProductInfo.NowSimulation.MOCode,((OQCLotRemoveIDEventArgs)oqcLotRemoveIDEventArgs).OQCLotNO,OQCFacade.Lot_Sequence_Default);
						if(objLot2Card == null)
						{
							throw new Exception("$Error_OQClot2CardNotExisted");
						}
						oqcFacade.DeleteOQCLot2Card( (OQCLot2Card)objLot2Card);
						object[] objs = oqcFacade.ExactQueryOQCLot2Card(((OQCLotRemoveIDEventArgs)oqcLotRemoveIDEventArgs).OQCLotNO,OQCFacade.Lot_Sequence_Default);
						if(objs == null)
						{
							//ɾ��OQCLot2ErrorCode
							oqcFacade.DeleteOQCLot2ErrorCode( ((OQCLotRemoveIDEventArgs)oqcLotRemoveIDEventArgs).OQCLotNO,OQCFacade.Lot_Sequence_Default.ToString());
							//ɾ��OQCLOTCheckList
							obj = oqcFacade.GetOQCLOTCheckList(((OQCLotRemoveIDEventArgs)oqcLotRemoveIDEventArgs).OQCLotNO,OQCFacade.Lot_Sequence_Default);
							if(obj != null)
							{
								oqcFacade.DeleteOQCLOTCheckList((OQCLOTCheckList)obj);
							}
							
							//ɾ��OQCLot
							obj = oqcFacade.GetOQCLot(((OQCLotRemoveIDEventArgs)oqcLotRemoveIDEventArgs).OQCLotNO,OQCFacade.Lot_Sequence_Default);
							if(obj == null)
							{
								throw new Exception("$Error_OQClotNotExisted");
							}
							oqcFacade.DeleteOQCLot((OQCLot)obj);
						}
						
						#endregion
						/* added by jessie lee, 2006/6/20
						 * Power0063:��װʱ������ӳ������ɾ��һ����Ʒ��Ӧ�ôӰ�װ����ɾ���˲�Ʒ�� */
						#region Carton
						if(cartoncode.Length>0)
						{
							Package.PackageFacade pf = new BenQGuru.eMES.Package.PackageFacade(DataProvider);
							object objCarton = pf.GetCARTONINFO(cartoncode);
 
							if(objCarton != null)
							{
								BenQGuru.eMES.Domain.Package.CARTONINFO carton = objCarton as BenQGuru.eMES.Domain.Package.CARTONINFO;
								pf.SubtractCollected((carton as BenQGuru.eMES.Domain.Package.CARTONINFO).CARTONNO);
							}
						}
						#endregion
						//AMOI  MARK  START  20050806 ���Ӱ���Դͳ�Ʋ���
						#region ��дͳ�Ʊ��� ����Դͳ��
                        //ReportHelper reportCollect= new ReportHelper(this.DataProvider);
                        //messages.AddMessages(reportCollect.ReportResQuanMaster(this.DataProvider
                        //    ,oqcLotRemoveIDEventArgs.ActionType,oqcLotRemoveIDEventArgs.ProductInfo));
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
			return messages;
		}
	
	}
}
