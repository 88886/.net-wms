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



namespace  BenQGuru.eMES.DataCollect.Action
{
	/// <summary>
	/// ���������ɼ�
	/// </summary>
	public class ActionOQC: IAction
	{
		private IDomainDataProvider _domainDataProvider = null;

		public ActionOQC()
		{	
		}

		public ActionOQC(IDomainDataProvider domainDataProvider)
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
		/// <param name="actionEventArgs"> </param> params (0,lotno)
		/// <returns></returns>
		public Messages Execute(ActionEventArgs actionEventArgs)
		{				
			Messages messages=new Messages();
			DataCollectDebug dataCollectDebug =new DataCollectDebug(this.GetType().ToString()+"Collect");
			dataCollectDebug.WhenFunctionIn(messages);
			try
			{
				ActionOnLineHelper dataCollect = new ActionOnLineHelper(this.DataProvider);
				ActionOQCHelper oqcHelper = new ActionOQCHelper(this.DataProvider);
				
				//��дSIMULATION ��鹤����ID��;�̡�����
				messages.AddMessages( dataCollect.CheckID(actionEventArgs));
				if (messages.IsSuccess())
				{				
					//
					if(actionEventArgs.ProductInfo.NowSimulation == null)
					{
						throw new Exception("$System_Error");
					}
					//check oqclotstatus
					if(oqcHelper.IsOQCLotComplete(actionEventArgs.Params[0].ToString()))
					{
						throw new Exception("$Error_OQCLotNOHasComplete");
					}
					//��������м��Ĳ�Ʒ�����һ����Ϣȫ��ΪGood

					messages.AddMessages( dataCollect.Execute(actionEventArgs));
					if (messages.IsSuccess())
					{
						//�޸���״̬
						//�����޸�ÿ�����ӵ����һ��״̬Ϊreject

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
