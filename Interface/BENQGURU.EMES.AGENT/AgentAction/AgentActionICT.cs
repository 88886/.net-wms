using System;
using System.Collections;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.DataCollect ;
using BenQGuru.eMES.DataCollect.Action ;
using BenQGuru.eMES.Domain.TS;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.FacilityFileParser;
using UserControl;

namespace BenQGuru.eMES.Agent
{
	/// <summary>
	/// AgentActionAOI ��ժҪ˵����
	/// </summary>
	public class AgentActionICT : IAgentAction	
	{
		private IDomainDataProvider _domainDataProvider = null;
		public AgentActionICT(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
		}

		private object[] parserObjs = null;

		#region IAgentAction ��Ա

		public UserControl.Messages CollectExcute(string filePath, string encoding)
		{
			object[] objs = null;
			ICTFileParser parser = new ICTFileParser();
			try
			{
				objs = parser.Parse(filePath);
			}
			catch{}

			#region ForTest
			//			if(objs != null && objs.Length > 0)
			//			{
			//				foreach(object obj in objs)
			//				{
			//					ICTData ictdata = obj as ICTData;
			//					string error = ictdata.ERRORCODES;
			//
			//					object[] tst =  GetErrorInfor( ictdata);
			//					string ss ="";
			//				}
			//			}
			#endregion

			parserObjs = objs;

			UserControl.Messages returnMsg = new Messages();

			((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.AutoCloseConnection = false;
			((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.OpenConnection();

			UserControl.Messages checkMsg= this.CheckData(parserObjs);
			UserControl.Messages goodMsg = this.GoodCollect(parserObjs);
			UserControl.Messages ngMsg	 = this.NGCollect(parserObjs);

			((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.CloseConnection();
			((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.AutoCloseConnection = true;
			if(checkMsg != null)
			{
				returnMsg.AddMessages(checkMsg);
			}
			if(goodMsg != null)
			{
				returnMsg.AddMessages(goodMsg);
			}
			if(ngMsg != null)
			{
				returnMsg.AddMessages(ngMsg);
			}
			return returnMsg;
		}

		public UserControl.Messages GoodCollect(object[] parserObjs)
		{
			UserControl.Messages returnMsg = new UserControl.Messages();
			try
			{
				foreach(object obj in parserObjs)
				{
					ICTData ictData = obj as ICTData;
					
					if(ictData.RESULT.Trim().ToUpper() == "PASS")
					{
						ActionOnLineHelper onLine=new ActionOnLineHelper(this._domainDataProvider);

						((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.AutoCloseConnection = false;
						((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.OpenConnection();
						Messages messages=  onLine.GetIDInfo(ictData.RCARD.Trim().ToUpper());
						try
						{
							ProductInfo product=(ProductInfo)messages.GetData().Values[0];
							this._domainDataProvider.BeginTransaction();

							if(AgentInfo.AllowGoToMO == true)
							{
								string moCode = this.getMOCode(ictData.RCARD.Trim().ToUpper());

								if(System.Configuration.ConfigurationSettings.AppSettings["MOPREFIXSTRING"] != null)
								{
									string moPrefix = System.Configuration.ConfigurationSettings.AppSettings["MOPREFIXSTRING"].Trim();
									//���ַ������
									if(moCode.Length < moPrefix.Length || moCode.Substring(0,moPrefix.Length) != moPrefix)
									{
										returnMsg.Add(new UserControl.Message(MessageType.Error
											,"$CS_Before_Card_FLetter_NotCompare $CS_Param_ID: " + ictData.RCARD.Trim().ToUpper()));
									}

									
								}
								if(System.Configuration.ConfigurationSettings.AppSettings["SNLENGTH"] != null)
								{
									try
									{
										int snLength = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["SNLENGTH"].Trim());
										//���ȼ��
										if(ictData.RCARD.Trim().ToUpper().Length != snLength)
										{
											returnMsg.Add(new UserControl.Message(MessageType.Error,
												"$CS_Before_Card_Length_FLetter_NotCompare $CS_Param_ID: " + ictData.RCARD.Trim().ToUpper()));
										}
									}
									catch(Exception ex)
									{
										returnMsg.Add(new UserControl.Message(ex));
									}
								}

								if(returnMsg.IsSuccess())
								{
									IAction dataCollectMO = new ActionFactory(this._domainDataProvider).CreateAction(ActionType.DataCollectAction_GoMO);
									messages.AddMessages(((IActionWithStatus)dataCollectMO).Execute(
										new GoToMOActionEventArgs(ActionType.DataCollectAction_GoMO, 
										ictData.RCARD.Trim().ToUpper(),
										ictData.USER.Trim().ToUpper(),
										AgentHelp.getResCode(ictData.RESOURCE.Trim().ToUpper()),
										product,moCode)));
							
									
									if(messages.IsSuccess())
									{
										returnMsg.Add( new UserControl.Message(UserControl.MessageType.Success,string.Format("{0} $CS_GOMO_CollectSuccess",ictData.RCARD.ToUpper())) );
									}
									else
									{
										returnMsg.Add( new UserControl.Message(UserControl.MessageType.Error,AgentHelp.GetErrorMessage(messages)));
									}
								}
							
								messages.ClearMessages();
							}
							
							string goodResult = string.Empty;
							//GOOD�ɼ�
							messages=  onLine.GetIDInfo(ictData.RCARD.Trim().ToUpper());
							product=(ProductInfo)messages.GetData().Values[0];
							BenQGuru.eMES.DataCollect.DataCollectFacade dcFacade = new BenQGuru.eMES.DataCollect.DataCollectFacade(this._domainDataProvider);
							goodResult = dcFacade.ActionCollectGood( ictData.RCARD.ToUpper(),ictData.USER.ToUpper() ,AgentHelp.getResCode(ictData.RESOURCE.ToUpper()) ); 
							if (goodResult == "OK")
							{
								this._domainDataProvider.CommitTransaction();

								returnMsg.Add( new UserControl.Message(UserControl.MessageType.Success,string.Format("{0} $CS_GOOD_CollectSuccess",ictData.RCARD.ToUpper())) );
								returnMsg.Add( new UserControl.Message(" "));
								BenQGuru.eMES.Common.Log.Info(string.Format("{0} $CS_GOOD_CollectSuccess�� {1}",ictData.RCARD.Trim().ToUpper(),"OK"));
								messages.Add(new UserControl.Message(MessageType.Success ,string.Format("$CS_GOODSUCCESS,$CS_Param_ID:{0}",ictData.RCARD.ToUpper())));
							}
							else
							{
								this._domainDataProvider.RollbackTransaction();

								string errorMsg = string.Format("{0} $CS_GOOD_CollectFail �� {1}",ictData.RCARD.Trim().ToUpper(),goodResult);
								returnMsg.Add( new UserControl.Message(UserControl.MessageType.Error,errorMsg) );
								returnMsg.Add( new UserControl.Message(" "));
								BenQGuru.eMES.Common.Log.Info(errorMsg);
							}
							
						}
						catch( Exception ex )
						{
							this._domainDataProvider.RollbackTransaction();
							BenQGuru.eMES.Common.Log.Info(AgentHelp.GetErrorMessage(messages),ex);
						}
						finally
						{
//							((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.CloseConnection();
//							((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.AutoCloseConnection = true;
						}
					}
				}
			}
			catch
			{}

			return returnMsg;
		}

		public UserControl.Messages NGCollect(object[] parserObjs)
		{
			UserControl.Messages returnMsg = new UserControl.Messages();
			try
			{
				foreach(object obj in parserObjs)
				{
					ICTData ictData = obj as ICTData;
					if(ictData.RESULT.Trim().ToUpper() == "FAIL")
					{
						ActionOnLineHelper onLine=new ActionOnLineHelper(this._domainDataProvider);

						((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.AutoCloseConnection = false;
						((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.OpenConnection();
						Messages messages=  onLine.GetIDInfo(ictData.RCARD.Trim().ToUpper());
						try
						{
							ProductInfo product=(ProductInfo)messages.GetData().Values[0];

							this._domainDataProvider.BeginTransaction();

							if(AgentInfo.AllowGoToMO == true)
							{
								string moCode = this.getMOCode(ictData.RCARD.Trim().ToUpper());

								if(System.Configuration.ConfigurationSettings.AppSettings["MOPREFIXSTRING"] != null)
								{
									string moPrefix = System.Configuration.ConfigurationSettings.AppSettings["MOPREFIXSTRING"].Trim();
									//���ַ������
									if(moCode.Length < moPrefix.Length || moCode.Substring(0,moPrefix.Length) != moPrefix)
									{
										returnMsg.Add(new UserControl.Message(MessageType.Error
											,"$CS_Before_Card_FLetter_NotCompare $CS_Param_ID: " + ictData.RCARD.Trim().ToUpper()));
									}

									
								}
								if(System.Configuration.ConfigurationSettings.AppSettings["SNLENGTH"] != null)
								{
									try
									{
										int snLength = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["SNLENGTH"].Trim());
										//���ȼ��
										if(ictData.RCARD.Trim().ToUpper().Length != snLength)
										{
											returnMsg.Add(new UserControl.Message(MessageType.Error,
												"$CS_Before_Card_Length_FLetter_NotCompare $CS_Param_ID: " + ictData.RCARD.Trim().ToUpper()));
										}
									}
									catch(Exception ex)
									{
										returnMsg.Add(new UserControl.Message(ex));
									}
								}

								if(returnMsg.IsSuccess())
								{
									IAction dataCollectMO = new ActionFactory(this._domainDataProvider).CreateAction(ActionType.DataCollectAction_GoMO);
									messages.AddMessages(((IActionWithStatus)dataCollectMO).Execute(
										new GoToMOActionEventArgs(ActionType.DataCollectAction_GoMO, 
										ictData.RCARD.Trim().ToUpper(),
										ictData.USER.Trim().ToUpper(),
										AgentHelp.getResCode(ictData.RESOURCE.Trim().ToUpper()),
										product,moCode)));
							
									
									if(messages.IsSuccess())
									{
										returnMsg.Add( new UserControl.Message(UserControl.MessageType.Success,string.Format("{0} $CS_GOMO_CollectSuccess",ictData.RCARD.ToUpper())) );
									}
									else
									{
										returnMsg.Add( new UserControl.Message(UserControl.MessageType.Error,AgentHelp.GetErrorMessage(messages)));
									}
								}
							
								messages.ClearMessages();
							}
							
							//ȡ������Ϣ
							object[] errorinfor =  GetErrorInfor(ictData);
							messages=  onLine.GetIDInfo(ictData.RCARD.Trim().ToUpper());
							product=(ProductInfo)messages.GetData().Values[0];
							//NG�ɼ�
							IAction dataCollectNG = new ActionFactory(this._domainDataProvider).CreateAction(ActionType.DataCollectAction_SMTNG);
							messages.AddMessages(((IActionWithStatus)dataCollectNG).Execute(
								new TSActionEventArgs(ActionType.DataCollectAction_SMTNG,
								ictData.RCARD.Trim().ToUpper(),
								ictData.USER.Trim().ToUpper(),
								AgentHelp.getResCode(ictData.RESOURCE.Trim().ToUpper()),
								product,
								errorinfor, 
								"")));
				
							if (messages.IsSuccess())
							{
								this._domainDataProvider.CommitTransaction();

								returnMsg.Add( new UserControl.Message(UserControl.MessageType.Success,string.Format("{0} $CS_NGSUCCESS",ictData.RCARD.ToUpper())) );
								returnMsg.Add( new UserControl.Message(" "));
								BenQGuru.eMES.Common.Log.Info(string.Format("{0} $CS_NGSUCCESS�� {1}",ictData.RCARD.Trim().ToUpper(),"OK"));
								messages.Add(new UserControl.Message(MessageType.Success ,string.Format("$CS_NGSUCCESS,$CS_Param_ID:{0}",ictData.RCARD.ToUpper())));
							}
							else
							{
								this._domainDataProvider.RollbackTransaction();

								string errorMsg = string.Format("{0} $CS_NGFail �� {1}",ictData.RCARD.Trim().ToUpper(),AgentHelp.GetErrorMessage(messages));
								returnMsg.Add( new UserControl.Message(UserControl.MessageType.Error,errorMsg) );
								returnMsg.Add( new UserControl.Message(" "));
								BenQGuru.eMES.Common.Log.Info(errorMsg);
							}
						}
						catch ( Exception ex )
						{
							this._domainDataProvider.RollbackTransaction();
							BenQGuru.eMES.Common.Log.Info(AgentHelp.GetErrorMessage(messages),ex);
						}
						finally
						{
//							((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.CloseConnection();
//							((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.AutoCloseConnection = true;
						}
					}
				}
			}
			catch
			{}

			return returnMsg;
		}

		private object[] GetErrorInfor(ICTData ictData)
		{
			string[] errorList = ictData.ERRORCODES.Split('|');
			ArrayList ecg2ecList = new ArrayList();
			for (int i=0 ; i<errorList.Length; i++)
			{
				if(errorList[i].Trim() == string.Empty)continue;

				bool hasShort = false;	//�Ƿ��ж�·����
				bool hasOpen = false;   //�Ƿ��п�·����
				string[] deError = errorList[i].Split(';');
				if(deError != null && deError.Length>0)
				{
					for(int j=0;j<deError.Length;j++)
					{
						if(deError[j].Trim()==string.Empty)continue;

						string[] ldeErrorInfo=deError[j].Split(',');
						TSErrorCode2Location tsinfo = new TSErrorCode2Location();
						object[] ecg2ec = this.QueryECG2ECByECode(ldeErrorInfo[0]);
						tsinfo.ErrorCode = ldeErrorInfo[0];

						if(tsinfo.ErrorCode.IndexOf("PVIB") > -1)
						{
							//OPEN Error
							if(hasShort)continue;
						}
						else if(tsinfo.ErrorCode.IndexOf("PVIA") > -1)
						{
							//Short Error
							if(hasOpen)continue;
						}


						tsinfo.ErrorLocation = getErrorLocation(ldeErrorInfo[1]);
						tsinfo.AB = "A";
						if(ecg2ec!=null && ecg2ec.Length>0)
						{
							tsinfo.ErrorCodeGroup = ((ErrorCodeGroup2ErrorCode)ecg2ec[0]).ErrorCodeGroup;
						}
						else
						{
							tsinfo.ErrorCodeGroup = ldeErrorInfo[0] + "GROUP";	//���û�в��������飬����Ĭ��ֵ
						}
						
						ecg2ecList.Add(tsinfo);
						if(tsinfo.ErrorCode.IndexOf("PVIB") > -1)
						{
							//OPEN Error
							hasShort = true;
						}
						else if(tsinfo.ErrorCode.IndexOf("PVIA") > -1)
						{
							//Short Error
							hasOpen = true;
						}
					}
				}
			}

			return (TSErrorCode2Location[])ecg2ecList.ToArray(typeof(TSErrorCode2Location));
		}

		private string getErrorLocation(string eclocation)
		{
			if(eclocation.Length > 40)
			{
				return eclocation.Substring(0,40).Trim();
			}
			return eclocation;
		}

		

		//���ݲ�������������ѯ����������Ͳ�������
		private object[] QueryECG2EC(string ecdesc)
		{
			try
			{
				string sql = string.Format("select tblecg2ec.* from tblecg2ec where ecode in (select ecode from tblec where ecdesc='{0}')",ecdesc);
				object[] ecg2ec = this._domainDataProvider.CustomQuery(typeof(ErrorCodeGroup2ErrorCode),new SQLCondition(sql));
				return ecg2ec;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		//���ݲ��������ѯ����������Ͳ�������
		private object[] QueryECG2ECByECode(string ecode)
		{
			try
			{
				string sql = string.Format("select tblecg2ec.* from tblecg2ec where ecode = '{0}'",ecode);
				object[] ecg2ec = this._domainDataProvider.CustomQuery(typeof(ErrorCodeGroup2ErrorCode),new SQLCondition(sql));
				return ecg2ec;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		//��ȡ��������
		private string getMOCode(string sn)
		{
			//��Ʒ���кŵĵڶ�λ������λ�ǹ������롱��Ҳ����˵���Ĳ�Ʒ���к��а�����5λ��������
			//MESϵͳ�еĹ��������ǡ��꣨4λ�����ָ�����������ERP�еĹ������루5λ��������ڽ�����Ʒ���кŵõ�5λ�����������ǰ�����ӵ�ǰ���ڶ�Ӧ���꣨4λ�����ټ��Ϸָ��������������µĹ������루����MESϵͳ�еĹ������룩
//			string mocode = DateTime.Now.Year.ToString() + "-" + sn.Substring(1,5);
//			return mocode;

			//Laws Lu,2006/12/26 �Զ��������������� 
			string index = System.Configuration.ConfigurationSettings.AppSettings["AutoGoMOIndex"];
			string len = System.Configuration.ConfigurationSettings.AppSettings["AutoGoMOLen"];

			// Added by Icyer 2006/07/06
			if (sn.Length < Convert.ToInt32(index) + Convert.ToInt32(len) - 1)
			{
				throw new  Exception("$Format_Error");
			}
			// Added end

			string mocode = DateTime.Now.Year.ToString()+"-"+ sn.Substring( Convert.ToInt32(index)-1, Convert.ToInt32(len) );
			//��Ʒ���кŵĵڶ�λ������λ�ǹ������롱��Ҳ����˵���Ĳ�Ʒ���к��а�����5λ��������
			//MESϵͳ�еĹ��������ǡ��꣨4λ�����ָ�����������ERP�еĹ������루5λ��������ڽ�����Ʒ���кŵõ�5λ�����������ǰ�����ӵ�ǰ���ڶ�Ӧ���꣨4λ�����ټ��Ϸָ��������������µĹ������루����MESϵͳ�еĹ������룩
			//string mocode = DateTime.Now.Year.ToString() + "-" + sn.Substring(1,5);
			return mocode;
		}

		#endregion

		#region ���ݼ��

		private UserControl.Messages CheckData(object[] parserObjs)
		{
			UserControl.Messages returnMsg = new Messages();
			
			if(parserObjs!=null && parserObjs.Length>0)
			{
				foreach(object obj in parserObjs)
				{
					ICTData ictData = obj as ICTData;

					//�����
					if(ictData.RESULT == string.Empty)
					{
						returnMsg.Add(new UserControl.Message(UserControl.MessageType.Error,"ICT HAS NO RESULT!"));
					}

					//�û����
					if(ictData.USER == string.Empty)
					{
						returnMsg.Add(new UserControl.Message(UserControl.MessageType.Error,"USERNAME IS EMPTY!"));
					}

					//��Դ���
					if(ictData.RESOURCE == string.Empty)
					{
						returnMsg.Add(new UserControl.Message(UserControl.MessageType.Error,"RESOURECODE IS EMPTY!"));
					}
				
				}
			}

			return returnMsg;
		}

		#endregion

		#region Ȩ�޺�;�̼��

		
		/// <summary>
		/// ���Ȩ��
		/// </summary>
		/// <param name="resrouce"></param>
		/// <param name="userCode"></param>
		/// <returns></returns>
		private string CheckRight( string resrouce,string userCode )
		{
			try
			{
				BenQGuru.eMES.Security.SecurityFacade sFacade = new BenQGuru.eMES.Security.SecurityFacade(this._domainDataProvider);
				return sFacade.CheckResourceRight(userCode, resrouce)==true? "OK":"û��Ȩ��";
				
			}
			catch(Exception ex)
			{
				return ex.Message;
			}
			finally
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.CloseConnection();
			}
		}

		/// <summary>
		/// ;�̼��
		/// </summary>
		/// <param name="id">��Ʒ���к�</param>
		/// <param name="resrouce">��Դ����</param>
		/// <param name="userCode">�û�����</param>
		/// <returns>��OK�� ��ʾ���ͨ��������Ϊ������Ϣ</returns>
		private string CheckRoute(string id,string resrouce,string userCode)
		{ 
			try
			{
				BenQGuru.eMES.DataCollect.DataCollectFacade dcFacade = new BenQGuru.eMES.DataCollect.DataCollectFacade(this._domainDataProvider);
				return dcFacade.CheckRoute(id, resrouce, userCode, 0); 
			}
			catch(Exception ex)
			{
				return ex.Message;
			}
			finally
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.CloseConnection();
			}
		}

		#endregion
	}
}
