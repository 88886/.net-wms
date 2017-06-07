#region system;
using System;
using UserControl;
#endregion

#region project
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.OQC;
using BenQGuru.eMES.DataCollect.Action;
using BenQGuru.eMES.OQC;
using BenQGuru.eMES.Domain.MOModel;

#endregion


namespace BenQGuru.eMES.DataCollect
{
	public class ActionOQCHelper
	{
		private IDomainDataProvider _domainDataProvider = null;

//		public ActionOQCHelper()
//		{	
//		}

		public ActionOQCHelper(IDomainDataProvider domainDataProvider)
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

		protected Messages messages = new Messages();



		public object[] GetOQCLot2Cards(string oqcLot)
		{
			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
			return oqcFacade.ExactQueryOQCLot2Card(oqcLot.ToUpper(),OQCFacade.Lot_Sequence_Default);
		}

		//�ж������Ƿ��ظ�
		public bool IsOQCLotNOExisted(string OQCLot)
		{
			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
			object  obj = oqcFacade.GetOQCLot(OQCLot,OQCFacade.Lot_Sequence_Default);
			if(obj == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		//�Ƿ���ڻ쵥
		public bool IsRemixMOCode(string oqcLotNO,string moCode)
		{
			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
			if( oqcFacade.GetUnSelectOQCLot2Card(oqcLotNO,moCode,string.Empty)>0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		//�Ƿ���ڶ��Item
		public bool IsRemixItemCode(string itemCode,string oqcLotNO)
		{
			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
			if( oqcFacade.GetUnSelectOQCLot2Card(oqcLotNO,string.Empty,itemCode)>0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}



		//��������Ƿ�Ϸ���������ڵ�״̬Ϊ(pass,reject)�ͳ���
		public bool IsOQCLotComplete(string OQCLot)
		{
			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
			object  obj = oqcFacade.GetOQCLot(OQCLot,OQCFacade.Lot_Sequence_Default);
			if(obj == null)
			{
				throw new Exception("Error_OQCLot_NotExisted,OQCLot='"+OQCLot+"'");
			}
			OQCLot oqcLot = obj as OQCLot;
			if( (oqcLot.LOTStatus == OQCLotStatus.OQCLotStatus_Pass)||(oqcLot.LOTStatus == OQCLotStatus.OQCLotStatus_Reject))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//��������Ƿ�Ϸ���������ڵ�״̬Ϊ(pass,reject)�ͳ���
		public bool IsOQCLotCompleteByLot(Domain.OQC.OQCLot obj)
		{
//			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
//			object  obj = oqcFacade.GetOQCLot(OQCLot,OQCFacade.Lot_Sequence_Default);
			if(obj == null)
			{
				throw new Exception("Error_OQCLot_NotExisted,OQCLot='"+obj.LOTNO+"'");
			}
			OQCLot oqcLot = obj as OQCLot;
			if( (oqcLot.LOTStatus == OQCLotStatus.OQCLotStatus_Pass)||(oqcLot.LOTStatus == OQCLotStatus.OQCLotStatus_Reject))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		
		//��������Ƿ�Ϸ���������ڵ�״̬Ϊ(pass,reject)�ͳ���
		public bool IsOQCLotCanExam(string OQCLot)
		{
			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
			object  obj = oqcFacade.GetOQCLot(OQCLot,OQCFacade.Lot_Sequence_Default);
			if(obj == null)
			{
				throw new Exception("Error_OQCLot_NotExisted,OQCLot='"+OQCLot+"'");
			}
			OQCLot oqcLot = obj as OQCLot;
			if( (oqcLot.LOTStatus == OQCLotStatus.OQCLotStatus_Pass)||(oqcLot.LOTStatus == OQCLotStatus.OQCLotStatus_Reject)||(oqcLot.LOTStatus == OQCLotStatus.OQCLotStatus_Initial) )
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public bool IsOQCLotInitial(string OQCLot)
		{
			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
			object  obj = oqcFacade.GetOQCLot(OQCLot,OQCFacade.Lot_Sequence_Default);
			if(obj == null)
			{
				throw new Exception("Error_OQCLot_NotExisted,OQCLot='"+OQCLot+"'");
			}
			OQCLot oqcLot = obj as OQCLot;
			if( oqcLot.LOTStatus == OQCLotStatus.OQCLotStatus_Initial)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsPackingOperation(string itemCode,string routeCode,string opCode)
		{
			ItemFacade itemFacade = new ItemFacade(this.DataProvider);
			object obj = itemFacade.GetItemRoute2Operation(itemCode,routeCode,opCode);
			if(obj == null)
			{
				throw new Exception("Error_ItemRoute2OP_NotExisted");
			}
			ItemRoute2OP itemRoute2Op = obj as ItemRoute2OP;

			return FormatHelper.StringToBoolean(itemRoute2Op.OPControl,(int)OperationList.Packing);
		}


		//�ж�һ��IDֻ������һ����
		public bool IsIDHasOnlyOQCLotNo(string runningID,string moCode,string oqcLotNO)
		{
			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
			return !oqcFacade.IsCardUsedByAnotherLot(runningID,moCode,oqcLotNO,OQCFacade.Lot_Sequence_Default);
		}

		//��ü�������������
		public int GetIDCountInOQCLotNo(string oqcLotNO)
		{
			OQCFacade oqcFacde = new OQCFacade(this.DataProvider);
			return oqcFacde.ExactQueryCountOQCLot2Card(oqcLotNO,OQCFacade.Lot_Sequence_Default);
		}

		/// <summary>
		/// ��ѯLOT������CARD��SIMULATION��¼
		/// 
		/// </summary>
		/// <param name="lOTNO"></param>
		/// <param name="lotSequence"></param>
		/// <returns></returns>
		public object[] QueryCardOfLot(string lOTNO, decimal lotSequence)
		{
			return this.DataProvider.CustomQuery(typeof(Simulation),
				new SQLParamCondition(string.Format("SELECT {0} FROM TBLSIMULATION s,TBLLOT2CARD l WHERE s.RCARD=l.RCARD and s.MOCODE=l.MOCODE and "
				+" l.LOTNO = $LOTNO AND l.LOTSEQ=$LOTSEQ "
				+" ORDER BY s.MOCODE,s.ROUTECODE,s.OPCODE,s.RESCODE,s.PRODUCTSTATUS,s.LACTION,s.ACTIONLIST,s.ISCOM for update nowait",
				GetDomainObjectFieldsString(typeof(Simulation),"s")) ,
				new SQLParameter[]{ new SQLParameter("LOTNO", typeof(string), lOTNO.ToUpper()) 
									  ,new SQLParameter("LOTSEQ", typeof(string), lotSequence)} ));
			//return simulations;
			//return this.DataProvider.CustomQuery(typeof(OQCLot2Card), new PagerCondition(
			//	string.Format("SELECT {0} FROM TBLLOT2CARD WHERE 1=1 AND LOTNO = '{1}'  AND LOTSEQ = '{2}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(OQCLot2Card)) , lOTNO,lotSequence), "RCARD,MOCODE,LOTNO,LOTSEQ",int.MinValue, int.MaxValue)); 
		}

		/// <summary>
		/// ���DomainObject��Ӧ���������ݿ��ֶ�������','ƴ�ɵ��ַ���
		/// </summary>
		/// <param name="type">DomainObject������</param>
		/// <returns></returns>
		public string GetDomainObjectFieldsString(Type type,string tblName)
		{
			string s=tblName+"."+ String.Join( ", "+tblName+".", (string[])DomainObjectUtility.GetDomainObjectFieldsName(type).ToArray(typeof(string)) );
			return s;
		}
		

	}
}