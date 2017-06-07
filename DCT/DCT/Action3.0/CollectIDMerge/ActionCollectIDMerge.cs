using System;
using System.Collections;
using UserControl;
using BenQGuru.eMES.Common.DCT.Core;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.DataCollect;
using BenQGuru.eMES.DataCollect.Action;
using BenQGuru.eMES.Material;

namespace BenQGuru.eMES.Common.DCT.Action
{
	/// <summary>
	/// ActionCollectIDMerge ��ժҪ˵����
	/// </summary>
	public class ActionCollectIDMerge:BaseDCTAction
	{
		public ActionCollectIDMerge()
		{
            this.InitMessage = (new ActionHelper()).GetActionDesc(this);
            this.OutMesssage = new Message(MessageType.Normal, "$CS_Please_Input_SN_For_Merge");
            this.LastPrompMesssage = new Message(MessageType.Normal, "$CS_Please_Input_SN_For_Merge");
		}

		private int IDMergeRule = 0;	// �ְ����
		private string mergeIdType = string.Empty;	// ���ת�����Ƿְ�
		private bool isSameMO = false;	// �Ƿ���ͬ����
		private decimal existIMEISeq = 0;
		private bool updateSimulation = false;
		private Hashtable mergeList = null;	// "ProdcutInfo"�д�Ű���ProductInfo��Messages; "MergeIDList"�д��ת�������кŵ�ArrayList
		
		private void ResetData()
		{
			IDMergeRule = 0;
			mergeIdType = string.Empty;
			isSameMO = false;
			existIMEISeq = 0;
			updateSimulation = true;
			mergeList = null;
		}
		
		public override Messages PreAction(object act)
		{
			// Added by Icyer 2006/12/14
			// �����Ʒ���к��Ǽ��
			DataCollect.Action.ActionEventArgs args = new BenQGuru.eMES.DataCollect.Action.ActionEventArgs();
			if (this.ObjectState != null)
			{
				args = (DataCollect.Action.ActionEventArgs)this.ObjectState;
			}
			else
			{
				args.RunningCard = act.ToString().ToUpper();
			}
			
			if (this.mergeList == null)
			{
				Messages msgCheck = CheckProduct(act, args.RunningCard);
				if (msgCheck.IsSuccess() == false)
				{
					ResetData();

                    ProcessBeforeReturn(this.Status, msgCheck);
					return msgCheck;
				}
			}
			
			this.ObjectState = args;
			// Added end
			
			base.PreAction (act);

			int iCurrent = 1;
			if (this.mergeList != null && this.mergeList["MergeIdList"] != null)
				iCurrent = ((ArrayList)this.mergeList["MergeIdList"]).Count + 1;
			this.OutMesssage = new UserControl.Message(MessageType.Normal,"$CS_Please_Input_Merge_ID " + iCurrent.ToString() + "/" + this.IDMergeRule.ToString());
			Messages msg = new Messages();
			msg.Add(this.OutMesssage);

            ProcessBeforeReturn(this.Status, msg);
			return  msg;
		}

		// Added by Icyer 2006/12/15
		// ����Ʒ
		private Messages CheckProduct(object act, string runningCard)
		{
			// ��ѯ��Ʒ��Ϣ
			BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider = null;
			if((act as IDCTClient).DBConnection != null)
			{
				domainProvider = (act as IDCTClient).DBConnection as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
			}
			else
			{
				domainProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider() 
					as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
				(act as IDCTClient).DBConnection = domainProvider;
			}
			
			ActionOnLineHelper _helper = new ActionOnLineHelper(domainProvider ); 
			UserControl.Messages msgProduct =  _helper.GetIDInfo( runningCard ) ;
			ProductInfo product= (ProductInfo)msgProduct.GetData().Values[0];
			if (product == null || product.LastSimulation == null)
			{
				msgProduct.ClearMessages();
				msgProduct.Add(new UserControl.Message(MessageType.Error, "$NoSimulation"));
				return msgProduct;
			}
			mergeList = new Hashtable();
			mergeList.Add("ProdcutInfo", msgProduct);

			// ת������
			//this.IDMergeRule = Convert.ToInt32(product.LastSimulation.IDMergeRule);	// ת�������ӹ����ж�
			
			IDCTClient client = act as IDCTClient;
			OPBOMFacade opBOMFacade=new OPBOMFacade( domainProvider);
			// ���;��
			Messages messages1= _helper.CheckID(
				new CKeypartsActionEventArgs(
				ActionType.DataCollectAction_Split,
				product.LastSimulation.RunningCard,
				client.LoginedUser,
				client.ResourceCode,
				product,
				null,
				null));
			if (messages1.IsSuccess() == true)
			{
				object op = new ItemFacade(domainProvider).GetItemRoute2Operation( 
					product.NowSimulation.ItemCode,
					product.NowSimulation.RouteCode,
					product.NowSimulation.OPCode );
				if (op == null)
				{
					messages1.Add(new UserControl.Message(MessageType.Error, "$Error_CS_Current_OP_Not_Exist"));
					return messages1;
				}
				if ( ((ItemRoute2OP)op).OPControl[(int) BenQGuru.eMES.BaseSetting.OperationList.IDTranslation] != '1' )
				{
					messages1.Add(new UserControl.Message(MessageType.Error,"$CS_OP_Not_SplitOP"));		//��ǰ���������ת������
					return messages1;
				}
				// ת������
				this.IDMergeRule = 1;
				if ( ((ItemRoute2OP)op).IDMergeType == IDMergeType.IDMERGETYPE_ROUTER )
				{
					this.IDMergeRule = (int)((ItemRoute2OP)op).IDMergeRule;
				}
				// ���ת������
				mergeIdType = ((ItemRoute2OP)op).IDMergeType;
				ArrayList listId = new ArrayList();
				mergeList.Add("MergeIdList", listId);
			}
			else
			{
				return messages1;
			}

			return msgProduct;
		}
		// Added end

		public override Messages Action(object act)
		{
			Messages msg = new Messages();

			BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider domainProvider = null;

			if(act == null)
			{
				return msg;
			}

			if (this.mergeList == null)
			{
				return msg;
			}
			
			DataCollect.Action.ActionEventArgs args;
			if(ObjectState == null)
			{
				args = new BenQGuru.eMES.DataCollect.Action.ActionEventArgs();
			}
			else
			{
				args = ObjectState as DataCollect.Action.ActionEventArgs;
			}

			string data = act.ToString().ToUpper().Trim();	//ת�������к�
			if((act as IDCTClient).DBConnection != null)
			{
				domainProvider = (act as IDCTClient).DBConnection as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
			}
			else
			{
				domainProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider() 
					as BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider;
				(act as IDCTClient).DBConnection = domainProvider;
			}

			if( mergeList == null )
			{
				msg = this.CheckProduct(act, args.RunningCard);
			}

			msg = CheckData(data, domainProvider);
			if (msg.IsSuccess() == false)
			{
                ProcessBeforeReturn(this.Status, msg);
				return msg;
			}

			if( msg.IsSuccess() )
			{
				msg = mergeList["ProdcutInfo"] as Messages;

				ProductInfo product = (ProductInfo)msg.GetData().Values[0];

				ArrayList listId = (ArrayList)mergeList["MergeIdList"];
				if( this.IDMergeRule > listId.Count )
				{
					listId.Add(data);

					mergeList["MergeIdList"] = listId;
				}

				if( this.IDMergeRule > listId.Count )
				{
					this.OutMesssage = new UserControl.Message(MessageType.Normal,"$CS_Please_Input_Merge_ID " +  (listId.Count + 1).ToString() + "/" + this.IDMergeRule.ToString());

                    msg.Add(this.OutMesssage);
                    ProcessBeforeReturn(this.Status, msg);
					return msg;
				}

				msg = this.DoDataCollectAction(domainProvider, (IDCTClient)act, this.isSameMO, Convert.ToInt32(this.existIMEISeq), this.updateSimulation);
			}

            if (msg.IsSuccess())
            {
                base.Action(act);
                this.ObjectState = null;
                ResetData();
            }

            ProcessBeforeReturn(this.Status, msg);
			return msg;
		}
		// 
		private Messages DoDataCollectAction(Common.DomainDataProvider.SQLDomainDataProvider domainProvider, IDCTClient client, bool IsSameMO, int ExistIMEISeq, bool UpdateSimulation)
		{
			Messages messages = new Messages();

			ProductInfo product = (ProductInfo)(((UserControl.Messages)mergeList["ProdcutInfo"]).GetData().Values[0]);
			ArrayList listId = (ArrayList)mergeList["MergeIdList"];
			SplitIDActionEventArgs args = new SplitIDActionEventArgs(
				ActionType.DataCollectAction_Split, 
				product.LastSimulation.RunningCard, 
				client.LoginedUser,
				client.ResourceCode,
				product, 
				(object[])listId.ToArray(),
				this.mergeIdType,
				IsSameMO,
				ExistIMEISeq,
				UpdateSimulation);

			IAction action = new BenQGuru.eMES.DataCollect.Action.ActionFactory(domainProvider).CreateAction(ActionType.DataCollectAction_Split);
			
			domainProvider.BeginTransaction();
			try
			{
				messages.AddMessages(action.Execute(args));	

				if ( messages.IsSuccess() )
				{
					domainProvider.CommitTransaction();
                    messages.Add(new UserControl.Message(MessageType.Success, "$CS_SplitID_CollectSuccess"));
				}
				else
				{
					domainProvider.RollbackTransaction();
				}

				return messages;
			}
			catch(Exception ex)
			{
				domainProvider.RollbackTransaction();
				messages.Add(new UserControl.Message(ex));
				return messages;
			}
			finally
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)domainProvider).PersistBroker.CloseConnection();
			}
		}

		public override Messages AftAction(object act)
		{
			base.AftAction (act);

			return null;
		}

		/// <returns></returns>
		public Messages CheckData(string data, Common.DomainDataProvider.SQLDomainDataProvider domainProvider)
		{
			Messages msg = new Messages();
			isSameMO = false;
			existIMEISeq = 0;
			updateSimulation = false;
			if (mergeList.ContainsKey("MergeIdList") == true)
			{
				ArrayList list = (ArrayList)mergeList["MergeIdList"];
				if (list.Contains(data) == true)
				{
					msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_Merge_ID_Exist"));
					return msg;
				}
			}
			ActionOnLineHelper _helper = new ActionOnLineHelper(domainProvider);
			msg = _helper.GetIDInfo(data.ToUpper());
			if ( ((ProductInfo)msg.GetData().Values[0]).LastSimulation != null )
			{
				if( string.Compare( this.mergeIdType, IDMergeType.IDMERGETYPE_IDMERGE, true) != 0 )
				{
					msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_ID_Already_Exist"));//���к��Ѵ���
					return msg;
				}

				ProductInfo newProduct = (ProductInfo)msg.GetData().Values[0];
				ProductInfo oriProduct = (ProductInfo)(((UserControl.Messages)this.mergeList["ProdcutInfo"]).GetData().Values[0]);
				/* �����к�ת������ 
					 * ת��ǰ��rcard �� ת�����rcard  ����ͬ
					 * ��ͬ�� check IMEI �ظ�ʹ��
					 * */
				if( string.Compare( oriProduct.LastSimulation.RunningCard, newProduct.LastSimulation.RunningCard, true) != 0 )  
				{
					string bMoCode = oriProduct.LastSimulation.MOCode;
					string aMoCode = newProduct.LastSimulation.MOCode;
					
					/* �ж����IMEI���Ƿ񱨷ϻ��߲�� */
					bool isSpliteOrScrape = CheckIMEISpliteOrScrape(
						domainProvider,
						newProduct.LastSimulation.RunningCard,
						newProduct.LastSimulation.RunningCardSequence,
						aMoCode) ;
					if( !isSpliteOrScrape )
					{
						/* rcard �깤������δ�� */
						if(newProduct.LastSimulation.IsComplete != "1"
							&& newProduct.LastSimulation.ProductStatus != ProductStatus.OffMo)
						{
							msg.Add(new UserControl.Message(MessageType.Error,"$Error_CS_ID_Already_Exist"));//���к��Ѵ���
							return msg;
						}
					}
					
					/* ����ͬһ�Ź��� */
					if( string.Compare( bMoCode,aMoCode,true )==0 )
					{
						isSameMO = true ;
					}
					else
					{
						/* ������ͬ���� */
						isSameMO = false ;
					}
					existIMEISeq = newProduct.LastSimulation.RunningCardSequence ;
					updateSimulation = true;
					
				}
				else /* rcard == tcard */
				{
					isSameMO = true ;
					existIMEISeq = newProduct.LastSimulation.RunningCardSequence ;
				}
			}
			// ����Ƿ��ڹ������кŷ�Χ��
			if (mergeList != null && mergeList.ContainsKey("ProdcutInfo") == true)
			{
				if (System.Configuration.ConfigurationSettings.AppSettings["CheckRCardRange"] == "1")
				{
					msg.AddMessages(CheckSNRange(data, domainProvider));
				}
			}
			return msg;
		}
		public Messages CheckSNRange(string data, Common.DomainDataProvider.SQLDomainDataProvider domainProvider)
		{
			Messages msg = new Messages();
			MORunningCardFacade rcardFacade = new MORunningCardFacade(domainProvider);
			Messages msgProduct = mergeList["ProdcutInfo"] as Messages;
			ProductInfo product = (ProductInfo)msgProduct.GetData().Values[0];
			if (rcardFacade.CheckRunningCardInRange(product.LastSimulation.MOCode, MORunningCardType.AfterConvert, data) == false)
			{
				msg.Add(new Message(MessageType.Error, "$DCT_GOMO_SN_Not_In_Range"));
			}
			return msg;
		}

		// ������к��Ƿ���򱨷�
		private bool CheckIMEISpliteOrScrape(Common.DomainDataProvider.SQLDomainDataProvider domainProvider, string rcard, decimal rcardseq, string mocode )
		{
			string sql = string.Format(" select count(*) from tblts where rcard='{0}' and rcardseq={1} and mocode='{2}' and tsstatus in ('{3}','{4}')",
				rcard,rcardseq,mocode,TSStatus.TSStatus_Scrap,TSStatus.TSStatus_Split);
			int count = domainProvider.GetCount( new Common.Domain.SQLCondition(sql) );
			if( count>0 )
			{
				return true;
			}
			return false;
		}
		
	}
}
