using System;
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

namespace BenQGuru.eMES.DataCollect.Action
{
	/// <summary>
	/// ���������ɼ�
	/// </summary>
	public class ActionPack:IAction
	{
		
		private IDomainDataProvider _domainDataProvider = null;

//		public ActionPack()
//		{	
//		}

		public ActionPack(IDomainDataProvider domainDataProvider)
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
/// 
/// </summary>
/// <param name="iD"></param>
/// <param name="actionType"></param>
/// <param name="resourceCode"></param>
/// <param name="userCode"></param>
/// <param name="product"></param>
/// <returns></returns>
		public Messages CheckID(string iD,string actionType,string resourceCode,string userCode, ProductInfo product)
		{
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"CheckID");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				//Laws Lu,2005/09/06,����
				DataCollectFacade dataCollect = new DataCollectFacade(this._domainDataProvider);
				messages.AddMessages( dataCollect.CheckID(iD,actionType,resourceCode,userCode, product));
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}

		public Messages CheckIDIn(ActionEventArgs actionEventArgs)
		{
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"GetIDInfo");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				DataCollectFacade dataCollect=new DataCollectFacade(this.DataProvider);
				if (((PackActionEventArgs)actionEventArgs).IDDatas ==null)
				{
					throw new Exception("$CS_Sys_Pack_LostOPParam");
				}

				string LotCode= ((PackActionEventArgs)actionEventArgs).IDDatas[0].ToString();	
			
				messages.AddMessages( dataCollect.CheckID(actionEventArgs.RunningCard,actionEventArgs.RunningCard,actionEventArgs.ResourceCode,actionEventArgs.UserCode, actionEventArgs.ProductInfo));
				//�ܷ��װ���������  TODO
				
				//
				actionEventArgs.ProductInfo.NowSimulation.LOTNO =LotCode;
				actionEventArgs.ProductInfo.NowSimulationReport.LOTNO =LotCode;
				
			}
			catch (Exception e)
			{
				messages.Add(new Message(e));
			}
			dataCollectDebug.WhenFunctionOut(messages);
			return messages;
		}
		/// <summary>
		/// ��Ʒ�ɼ�
		/// </summary>
		/// <param name="domainDataProvider"></param>
		/// <param name="iD"></param>
		/// <param name="actionType"></param>
		/// <param name="resourceCode"></param>
		/// <param name="userCode"></param>
		/// <param name="product"></param>
		/// <param name="datas1">����</param>
		/// <param name="datas2">NULL</param>
		/// <returns></returns>
		public Messages Execute(ActionEventArgs actionEventArgs)
		{				
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				ActionOnLineHelper dataCollect=new ActionOnLineHelper(this.DataProvider);
				//��дSIMULATION ��鹤����ID��;�̡�����
				messages.AddMessages( this.CheckIDIn(actionEventArgs));
				if (messages.IsSuccess())
				{				
					//
					messages.AddMessages( dataCollect.Execute(actionEventArgs));
					if (messages.IsSuccess())
					{
						//��д��װ���� TODO

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
