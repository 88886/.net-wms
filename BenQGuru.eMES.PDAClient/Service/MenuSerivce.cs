using System;
using System.Collections;

using BenQGuru.eMES.PDAClient.Command;
using BenQGuru.eMES.PDAClient.Menu;
using BenQGuru.eMES.PDAClient;
using BenQGuru.eMES.Security;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;

namespace BenQGuru.eMES.PDAClient.Service
{
	/// <summary>
	/// MenuSerivce ��ժҪ˵����
	/// </summary>
	public class MenuService
	{
		private IMenu[] _loadMenus = null;
		private IDomainDataProvider _domainDataProvider;
		private SecurityFacade _facade;
		private string parse(string orginStr)
		{
			return UserControl.MutiLanguages.ParserString(orginStr);
		}

		public MenuService(IDomainDataProvider provider)
		{
			if(provider != null)
			{
				_domainDataProvider = provider;
			}

			try
			{
				_facade = (SecurityFacade)Activator.CreateInstance(typeof(SecurityFacade),new object[]{provider});
			}
			catch(Exception ex)
			{
				Common.Log.Error(ex.Message);
                //ApplicationRun.GetInfoForm().Add(new UserControl.Message(ex));
                _facade = new SecurityFacade(provider);
			}
//			_facade = new SecurityFacade(_domainDataProvider);
		}

		public IMenu[] LoadMenus(bool isForce)
		{
			return LoadMenus(isForce, false);
		}
		public IMenu[] LoadMenus(bool isForce, bool withPermission)
		{
			if (isForce || (_loadMenus == null))
			{
				if (withPermission == false)
					_loadMenus = BuildMenus();
				else
				{
					IMenu[] perMenus = (new MenuBuilder()).Build();
					ArrayList listMenu = new ArrayList();
                    //marked by carey.cheng on 2010-05-18 for new login mode
					//listMenu.Add(this.DefaultFileMenu());
                    //end marked by carey.cheng on 2010-05-18 for new login mode
					if (perMenus != null)
					{
						listMenu.AddRange(perMenus);
					}
					_loadMenus = new IMenu[listMenu.Count];
					listMenu.CopyTo(_loadMenus);
				}
			}
			
			return _loadMenus;
		}

		public void ClearMenu()
		{
			_loadMenus = null;
		}

		public IMenu[] BuildMenus()
		{
			#region ���õĴ���
			/*
			MenuCommand mdFile =new MenuCommand(parse("$menu_document"), 0, null);	//�ļ�
			mdFile.SubMenus = new IMenu[] { new MenuCommand("BenQGuru.eMES.PDAClient.FLogin", parse("$menu_Login"), 0, new CommandLogin("BenQGuru.eMES.PDAClient.FLogin"))	//��¼
											  ,new MenuCommand("BenQGuru.eMES.PDAClient.Close", parse("$menu_logout"), 1, new CommandFileClose())							//�˳�
//											  ,new MenuCommand("���", 2, new CommandWindowsCascade())
//											  ,new MenuCommand("ˮƽƽ��", 3, new CommandWindowsHorizontal())
//											  ,new MenuCommand("��ֱƽ��", 4, new CommandWindowsVertical())
										  };
			
			//Amoi,Laws Lu,2005/08/04,�޸�good/ng�ɼ���smt good/ng�ɼ��Ĳ˵�
			//Laws Lu,2005/10/27,�޸�	����SMT���ֻ�
			MenuCommand mdCollect =new MenuCommand(parse("$menu_DataCollect"), 0, null);		//���ݲɼ���ҵ
			if(System.Configuration.ConfigurationSettings.AppSettings["NeedSMT"] == "1")
			{
				mdCollect.SubMenus = new IMenu[] {	  new MenuCommand("BenQGuru.eMES.PDAClient.FMultiGoMO", parse("$menu_gomo"),0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FMultiGoMO"))								//���������ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FOffMo", parse("$menu_offmo"),1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FOffMo"))									//���빤���ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectionMetrial", parse("$menu_collectMaterial"),3, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectionMetrial"))	//���ϲɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FDropMaterial", parse("$menu_dropMaterial"),5, new CommandOpenForm("BenQGuru.eMES.PDAClient.FDropMaterial"))				//���ϲɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FBurnIn", parse("$menu_burnin"),7, new CommandOpenForm("BenQGuru.eMES.PDAClient.FBurnIn"))									//Burn In
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FBurnOut", parse("$menu_burnout"),9, new CommandOpenForm("BenQGuru.eMES.PDAClient.FBurnOut"))								//Burn Out
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectionSMTGDNG", parse("$menu_SMTGoodNGCollect"), 11, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectionSMTGDNG"))//SMT��Ʒ/����Ʒ�ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectionIDMerge", parse("$menu_IDMerge"), 13, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectionIDMerge"))		//���ת���ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectionSoftware", parse("$menu_softCollect"), 15, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectionSoftware"))	//�����Ϣ�ɼ�										  
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectReject", parse("$menu_rejectCollect"), 17, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectReject"))			//���˲ɼ�									 
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FManualAlert", parse("$menu_manualAlert"),19, new CommandOpenForm("BenQGuru.eMES.PDAClient.FManualAlert"))					//�ֶ�Ԥ��

													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FPTCollectionGDNG", parse("$menu_PTGoodNGCollect"), 21, new CommandOpenForm("BenQGuru.eMES.PDAClient.FPTCollectionGDNG"))	    ////Karron Qiu,2006-5-30 ,PT��Ʒ/����Ʒ�ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FSNCheck", parse("$menu_SN_IDMerge_Check"), 23, new CommandOpenForm("BenQGuru.eMES.PDAClient.FSNCheck"))	    ////Laws Lu,2006-6-12 ,���ת������
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FRMACollectionGDNG", parse("$menu_FRMACollectionGDNG"), 23, new CommandOpenForm("BenQGuru.eMES.PDAClient.FRMACollectionGDNG"))	    //
												 };
			}
			else
			{
				mdCollect.SubMenus = new IMenu[] {	  new MenuCommand("BenQGuru.eMES.PDAClient.FMultiGoMO", parse("$menu_gomo"),0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FMultiGoMO"))								//���������ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FOffMo", parse("$menu_offmo"),1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FOffMo"))									//���빤���ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectionMetrial", parse("$menu_collectMaterial"),3, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectionMetrial"))	//���ϲɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FDropMaterial", parse("$menu_dropMaterial"),5, new CommandOpenForm("BenQGuru.eMES.PDAClient.FDropMaterial"))				//���ϲɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FBurnIn", parse("$menu_burnin"),7, new CommandOpenForm("BenQGuru.eMES.PDAClient.FBurnIn"))									//Burn In
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FBurnOut", parse("$menu_burnout"),9, new CommandOpenForm("BenQGuru.eMES.PDAClient.FBurnOut"))								//Burn Out
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectionGDNG", parse("$menu_GoodNGCollect"), 11, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectionGDNG"))	    //��Ʒ/����Ʒ�ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectionIDMerge", parse("$menu_IDMerge"), 13, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectionIDMerge"))		//���ת���ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectionSoftware", parse("$menu_softCollect"), 15, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectionSoftware"))	//�����Ϣ�ɼ�									  
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectReject", parse("$menu_rejectCollect"), 17, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectReject"))			//���˲ɼ�									 
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FManualAlert", parse("$menu_manualAlert"),19, new CommandOpenForm("BenQGuru.eMES.PDAClient.FManualAlert"))					//�ֶ�Ԥ��
													 
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FPTCollectionGDNG", parse("$menu_PTGoodNGCollect"), 21, new CommandOpenForm("BenQGuru.eMES.PDAClient.FPTCollectionGDNG"))	    ////Karron Qiu,2006-5-30 ,PT��Ʒ/����Ʒ�ɼ�
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FSNCheck", parse("$menu_SN_IDMerge_Check"), 23, new CommandOpenForm("BenQGuru.eMES.PDAClient.FSNCheck"))	    ////Laws Lu,2006-6-12 ,���ת������
													 ,new MenuCommand("BenQGuru.eMES.PDAClient.FRMACollectionGDNG", parse("$menu_FRMACollectionGDNG"), 23, new CommandOpenForm("BenQGuru.eMES.PDAClient.FRMACollectionGDNG"))	    //
												 };
			}

			//EndAmoi


			MenuCommand mdMateriel  = new MenuCommand(parse("$menu_stockManage"), 1, null);	//������ҵ
			mdMateriel.SubMenus = new IMenu[] { 
												  new MenuCommand("BenQGuru.eMES.PDAClient.FINNO", parse("$menu_innoCollect"), 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FINNO"))									//������������ά��
												  ,new MenuCommand("BenQGuru.eMES.PDAClient.FKeyParts", parse("$menu_keyPartsCollect"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FKeyParts"))						//KeyPart����ά��
												  // ,new MenuCommand("Stationά��", 3, new CommandFMetrialStation(""))	
												  // ,new MenuCommand("Feederά��", 4, new CommandFMetrialFeeder(""))	
												  // ,new MenuCommand("SMT��������ά��", 4, new CommandFSMTFeederAndStation(""))
											  };

			MenuCommand mdTS  = new MenuCommand(parse("$menu_TSManage"), 2, null);	//ά����ҵ
			mdTS.SubMenus = new IMenu[] { new MenuCommand("BenQGuru.eMES.PDAClient.FTSInput", parse("$menu_NGSendTS"), 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FTSInput"))										//����Ʒ����
											,new MenuCommand("BenQGuru.eMES.PDAClient.FTSInputEdit", parse("$menu_TSDeal"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FTSInputEdit"))								//ά�޴���				
											,new MenuCommand("BenQGuru.eMES.PDAClient.FRMATSInputEdit", parse("$menu_FRMATSInputEdit"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FRMATSInputEdit"))								//ά�޴���				
											//,new MenuCommand("BenQGuru.eMES.PDAClient.FTSInputComplete", parse("$menu_TSComplete"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FTSInputComplete"))					//ά�����
											,new MenuCommand("BenQGuru.eMES.PDAClient.FTSScrap", parse("$menu_TSScrap"),3, new CommandOpenForm("BenQGuru.eMES.PDAClient.FTSScrap"))										//ά�ޱ���
										};

			MenuCommand mdCarton  = new MenuCommand(parse("$menu_packgeManage"), 3, null);	//��װ��ҵ
			mdCarton.SubMenus =new IMenu[] { new MenuCommand("BenQGuru.eMES.PDAClient.FPackCarton", parse("$menu_CartonPack"), 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FPackCarton"))
											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FTransferCarton", parse("$menu_CartonTransfer"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FTransferCarton"))
											   , new MenuCommand("BenQGuru.eMES.PDAClient.FGenLotIDMerge", parse("$menu_GenLot"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FGenLotIDMerge"))
											  
											   // ,new MenuCommand("��װ��", 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCartonSplit"))
											   // ,new MenuCommand("�ϲ���װ", 2, new CommandCartonMerge(""))		
										   };

			MenuCommand mdOQC = new MenuCommand(parse("$menu_FQCManage"), 4, null);	//FQC��ҵ
			mdOQC.SubMenus = new IMenu[] { 
											 //	new MenuCommand("ȫ�����ݲɼ�", 0, new CommandTestCollect("")),
											 new MenuCommand("BenQGuru.eMES.PDAClient.FOQCSamplePlan", parse("$menu_FQCSamplePlan"), 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FOQCSamplePlan"))					//FQC �����ƻ�
											 ,new MenuCommand("BenQGuru.eMES.PDAClient.FCollectionOQC", parse("$menu_FQCDataCollect"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FCollectionOQC"))					//FQC ���ݲɼ�						 
											 ,new MenuCommand("BenQGuru.eMES.PDAClient.FOQCFuncTest", parse("$menu_FQCFuncTest"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FOQCFuncTest"))					//FQC ���ܲ���
											 ,new MenuCommand("BenQGuru.eMES.PDAClient.FOQCDimention", parse("$menu_FQC_Dimention"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FOQCDimention"))     //�ߴ����ݲɼ�
										 };

			MenuCommand mdStock = new MenuCommand(parse("$menu_ProductIOManage"),5, null);	//��Ʒ�������ҵ

			mdStock.SubMenus = new IMenu[] { 

											   new MenuCommand("BenQGuru.eMES.PDAClient.FInvReceive", parse("$menu_importBaseData"), 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FInvReceive"))						//��ⵥ����ά��                                                                                             

											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FNormalReceive", parse("$menu_importDataCollect"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FNormalReceive"))				//�������ɼ�

											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FUnNormalReceive", parse("$menu_nonProduceImportCollect"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FUnNormalReceive"))	//���������ɼ�

											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FNormalShip", parse("$menu_sellExportCollect"), 3, new CommandOpenForm("BenQGuru.eMES.PDAClient.FNormalShip"))					//���۳����ɼ�

											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FUnNormalShip", parse("$menu_nonSellExportCollect"), 4, new CommandOpenForm("BenQGuru.eMES.PDAClient.FUnNormalShip"))			//�����۳����ɼ�
											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FInvShipImp", parse("$menu_exportSheetInsert"), 5, new CommandOpenForm("BenQGuru.eMES.PDAClient.FInvShipImp"))					//����������
											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FInvRecCheck", parse("$menu_inv_rec_check"), 6, new CommandOpenForm("BenQGuru.eMES.PDAClient.FInvRecCheck"))     //���������֤
											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FInvShipCheck", parse("$menu_inv_ship_check"), 7, new CommandOpenForm("BenQGuru.eMES.PDAClient.FInvShipCheck"))     //����������֤
											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FInvRCardSwitchType", parse("$menu_FInvRCardSwitchType"), 7, new CommandOpenForm("BenQGuru.eMES.PDAClient.FInvRCardSwitchType"))     //����������֤

										   };



			MenuCommand mdFirstLine = new MenuCommand(parse("$menu_FistProductManage"),6, null);	//�׼�����
			mdFirstLine.SubMenus = new IMenu[] { 
											   // new MenuCommand("ȫ�����ݲɼ�", 0, new CommandTestCollect("BenQGuru.eMES.PDAClient.FTestCollect")),
											   new MenuCommand("BenQGuru.eMES.PDAClient.FFirstOnline", parse("$menu_FirstOnline"), 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FFirstOnline"))						//�׼�����
											   ,new MenuCommand("BenQGuru.eMES.PDAClient.FFirstOffline", parse("$menu_FirstOffline"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FFirstOffline"))					//�׼�����
											   , new MenuCommand("BenQGuru.eMES.PDAClient.FLastOnline", parse("$menu_LastOnline"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FLastOnline"))							//ĩ������
                                               ,new MenuCommand("BenQGuru.eMES.PDAClient.FLastOffline", parse("$menu_LastOffline"), 3, new CommandOpenForm("BenQGuru.eMES.PDAClient.FLastOffline"))						//ĩ������


										   };

//			MenuCommand mdReportPQ = new MenuCommand("ʵʱ����", 6, null);
//			mdReportPQ.SubMenus = new IMenu[] { 
//												  new MenuCommand("ʵʱ������ѯ", 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FRealReportPQ"))	
//												  ,new MenuCommand("ʵʱ���ʲ�ѯ", 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FRealReportYR"))	
//												  ,new MenuCommand("ʵʱȱ�ݷ���", 5, new CommandOpenForm("BenQGuru.eMES.PDAClient.FRealReportReject"))
//											  };

			MenuCommand mdSMT = new MenuCommand("SMT����",6, null);	//�׼�����
			mdSMT.SubMenus = new IMenu[] { 
												   new MenuCommand("BenQGuru.eMES.PDAClient.FFeederGetOut", parse("$menu_FeederUse"), 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FFeederGetOut"))
												   ,new MenuCommand("BenQGuru.eMES.PDAClient.FSMTReelPrepare", parse("$menu_FSMTReelPrepare"), 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FSMTReelPrepare"))
												   ,new MenuCommand("BenQGuru.eMES.PDAClient.FFeederMaintain", parse("$menu_FeederMainten"), 1, new CommandOpenForm("BenQGuru.eMES.PDAClient.FFeederMaintain"))
												   , new MenuCommand("BenQGuru.eMES.PDAClient.FSMTLoadFeeder", parse("$menu_SMT_Collect"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FSMTLoadFeeder"))
											 , new MenuCommand("BenQGuru.eMES.PDAClient.FSMTFeederReelWatch", parse("$menu_SMT_FeederReelWatch"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FSMTFeederReelWatch"))
											 , new MenuCommand("BenQGuru.eMES.PDAClient.FSMTTransferMO", parse("$menu_SMT_TransferMO"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FSMTTransferMO"))
											 , new MenuCommand("BenQGuru.eMES.PDAClient.FSMTCheckReelQty", parse("$menu_SMT_FSMTCheckReelQty"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FSMTCheckReelQty"))
											 , new MenuCommand("BenQGuru.eMES.PDAClient.FSPControl", parse("$menu_SMT_SPManagement"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FSPControl"))
											 , new MenuCommand("BenQGuru.eMES.PDAClient.FArmorPlate", parse("$menu_SMT_ArmorPlate"), 2, new CommandOpenForm("BenQGuru.eMES.PDAClient.FArmorPlate"))
										 };

			MenuCommand mdWatch = new MenuCommand("��ʱ����",6, null);	//��ʱ����
			mdWatch.SubMenus = new IMenu[]{
											  new MenuCommand("BenQGuru.eMES.PDAClient.FWatchPanel", parse("$menu_FWatchPanel"), 0, new CommandOpenForm("BenQGuru.eMES.PDAClient.FWatchPanel"))
										  };

			//return (new IMenu[] {mdFile, mdCollect, mdMateriel, mdTS,mdCarton, mdOQC});
			return (new IMenu[] {mdFile, mdCollect, mdMateriel,mdTS, mdCarton, mdOQC,mdStock,mdSMT,mdWatch});
			*/
			#endregion

			//return new IMenu[]{DefaultFileMenu()};
            //modified by carey.cheng on 2010-05-18 for new login mode
            return new IMenu[] { };
            //End modified by carey.cheng on 2010-05-18 for new login mode
		}
		private IMenu DefaultFileMenu()
		{
			MenuCommand mdFile =new MenuCommand(parse("menu_document"), 0, null);	//�ļ�
			mdFile.SubMenus = new IMenu[] { new MenuCommand("BenQGuru.eMES.PDAClient.FLogin", parse("menu_Login"), 0, new CommandLogin("BenQGuru.eMES.PDAClient.FLogin"))	//��¼
											  ,new MenuCommand("BenQGuru.eMES.PDAClient.Close", parse("menu_logout"), 1, new CommandFileClose())							//�˳�
										  };
			return mdFile;
		}


		private bool IsLoad = false;
		public  void  MergeUltraWinMainMenu(Infragistics.Win.UltraWinToolbars.UltraToolbar mainMenuBar)
		{
			MergeUltraWinMainMenu(mainMenuBar, false);
		}
		public  void  MergeUltraWinMainMenu(Infragistics.Win.UltraWinToolbars.UltraToolbar mainMenuBar, bool withPermission)
		{
			UserControl.MutiLanguages.Language = ApplicationService.Current().Language;
			mainMenuBar.IsMainMenuBar = true;
			mainMenuBar.Settings.ToolDisplayStyle	= Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			mainMenuBar.Settings.CaptionPlacement	= Infragistics.Win.TextPlacement.BelowImage;
			mainMenuBar.DockedPosition	= Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			bool isForce = false;
			if (withPermission == true)
				isForce = true;
			IMenu[] menus = DrawMenus2UltraWinMenuItem(this.LoadMenus(isForce, withPermission), mainMenuBar);
			if (menus== null)
			{
				return;
			}
			//if(!IsLoad)
			//{

			mainMenuBar.ToolbarsManager.ToolClick -=new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(ToolbarsManager_ToolClick);
			mainMenuBar.ToolbarsManager.ToolClick +=new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(ToolbarsManager_ToolClick);
			ApplicationService.Current().MainWindows.ultraExplorerBar.ItemClick -= new Infragistics.Win.UltraWinExplorerBar.ItemClickEventHandler(ultraExplorerBar_ItemClick);
			ApplicationService.Current().MainWindows.ultraExplorerBar.ItemClick += new Infragistics.Win.UltraWinExplorerBar.ItemClickEventHandler(ultraExplorerBar_ItemClick);
			//}
			IsLoad = true;
		}
		public IMenu[] DrawMenus2UltraWinMenuItem(IMenu[] menus, Infragistics.Win.UltraWinToolbars.UltraToolbar mainMenuBar)
		{
			if (menus== null)
			{
				return null;
			}
			
			for (int i=0;i<menus.Length; i++)
			{
				DrawUltraWinMenuItem(menus[i], mainMenuBar);	
			}
			if(menus.Length>0)
			{
				UpdateView(menus[0]);
			}
			return menus;
		}
		private IMenu DrawUltraWinMenuItem(IMenu menu, Infragistics.Win.UltraWinToolbars.UltraToolbar mainMenuBar)
		{
			Infragistics.Win.UltraWinToolbars.ToolBase subMenuItem;
			//if (menu.SubMenus != null)
			//{	
			//	subMenuItem = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(menu.Caption);	
			//}
			//else
			//{
				subMenuItem = new Infragistics.Win.UltraWinToolbars.ButtonTool(menu.Caption);
			//}

			subMenuItem.SharedProps.Caption		=  menu.Caption; 
			subMenuItem.SharedProps.CustomizerCaption  =  menu.Caption; 
			subMenuItem.SharedProps.Shortcut = menu.Shortcut;
			subMenuItem.Key =  (string)menu.Key;
			//subMenuItem.Index   = menu.Index; 
			menu.MenuObject = subMenuItem;

			mainMenuBar.ToolbarsManager.Tools.Add((Infragistics.Win.UltraWinToolbars.ToolBase)menu.MenuObject);
			mainMenuBar.Tools.Add((Infragistics.Win.UltraWinToolbars.ToolBase)menu.MenuObject) ;

//			if (menu.SubMenus != null)
//			{
//				DrawSubUltraWinMenuItem(menu, mainMenuBar);	
//			}

			return menu;
		}
		private void DrawSubUltraWinMenuItem(IMenu menu, Infragistics.Win.UltraWinToolbars.UltraToolbar mainMenuBar)
		{	
			for (int i=0;i<menu.SubMenus.Length; i++)
			{
				Infragistics.Win.UltraWinToolbars.ToolBase subMenuItem;
				if (menu.SubMenus[i].SubMenus != null)
				{
					subMenuItem = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(menu.SubMenus[i].Caption);
				}
				else
				{
					subMenuItem = new Infragistics.Win.UltraWinToolbars.ButtonTool(menu.SubMenus[i].Caption);
				}

				subMenuItem.SharedProps.Caption = menu.SubMenus[i].Caption;
				subMenuItem.SharedProps.CustomizerCaption = menu.SubMenus[i].Caption;
				subMenuItem.SharedProps.Shortcut = menu.SubMenus[i].Shortcut;
				//subMenuItem.Index   = menu.Index; 
				subMenuItem.Key =  (string)menu.SubMenus[i].Key;
				menu.SubMenus[i].MenuObject = subMenuItem;
				
				mainMenuBar.ToolbarsManager.Tools.Add(subMenuItem);
				((Infragistics.Win.UltraWinToolbars.PopupMenuTool)menu.MenuObject).Tools.Add(subMenuItem); 

				if (menu.SubMenus[i].SubMenus != null)
				{
					DrawSubUltraWinMenuItem(menu.SubMenus[i], mainMenuBar);	
				}
			}
		}
		private void ToolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			DispathToolbarOnClick(e.Tool.Key);
		}


		public  void  MergeMainMenu(System.Windows.Forms.MainMenu mainMenu)
		{
			IMenu[] menus = DrawMenus2WinMenuItem(this.LoadMenus(false));
			if (menus== null)
			{
				return;
			}
			for (int i=0;i<menus.Length; i++)
			{
				mainMenu.MenuItems.Add((System.Windows.Forms.MenuItem)menus[i].MenuObject) ;
			}
		}
		public IMenu[] DrawMenus2WinMenuItem(IMenu[] menus)
		{
			if (menus== null)
			{
				return null;
			}
			
			for (int i=0;i<menus.Length; i++)
			{
				DrawMenuItem(menus[i]);	
			}

			return menus;
		}
		private IMenu DrawMenuItem(IMenu menu)
		{
			System.Windows.Forms.MenuItem subMenuItem = new System.Windows.Forms.MenuItem();
			subMenuItem.Text = menu.Caption;
			subMenuItem.Shortcut = menu.Shortcut;
			subMenuItem.Index  = menu.Index; 
			subMenuItem.Click += new EventHandler(menuItem_Click);
			menu.Key = subMenuItem;
			menu.MenuObject = subMenuItem;
			if (menu.SubMenus != null)
			{
				DrawSubMenuItem(menu);	
			}
			 return menu;
		}
		private void DrawSubMenuItem(IMenu menu)
		{	
			for (int i=0;i<menu.SubMenus.Length; i++)
			{
				System.Windows.Forms.MenuItem subMenuItem = new System.Windows.Forms.MenuItem();
				subMenuItem.Text = menu.SubMenus[i].Caption;
				subMenuItem.Shortcut = menu.SubMenus[i].Shortcut;
				subMenuItem.Index  = menu.SubMenus[i].Index; 
				subMenuItem.Click += new EventHandler(menuItem_Click);
				menu.SubMenus[i].Key = subMenuItem;
				menu.SubMenus[i].MenuObject = subMenuItem;
				((System.Windows.Forms.MenuItem)menu.MenuObject).MenuItems.Add(subMenuItem); 
				
				if (menu.SubMenus[i].SubMenus != null)
				{
					DrawSubMenuItem(menu.SubMenus[i]);	
				}
			}
		}

		private void menuItem_Click(object sender, EventArgs e)
		{
			//System.Windows.Forms.MessageBox.Show(sender.ToString());
			DispathToolbarOnClick(sender);
		}


		public void DispathToolbarOnClick(object key)
		{			
			if (_loadMenus != null)
			{
				for(int i=0; i<_loadMenus.Length ;i++ )
				{
					if (_loadMenus[i].Key!=null)
					{
						if (_loadMenus[i].Key.Equals(key))
						{

							if (_loadMenus[i].Command!=null)
							{
								_loadMenus[i].Command.Execute(); 								
							}

							UpdateView(_loadMenus[i]);													
							break;
						}

						if (_loadMenus[i].SubMenus != null)
						{
							DispathSubToolbarOnClick(key, _loadMenus[i]);
						}
					}
				}
			}
		}

		private void DispathSubToolbarOnClick(object key, IMenu menu)
		{
			try
			{
				for (int i=0;i<menu.SubMenus.Length; i++)
				{	
					if (menu.SubMenus[i].Key!=null)
					{
						if (menu.SubMenus[i].Key.Equals(key))
						{
							// ���Ȩ��							
							bool needCheck = true;

							foreach ( string needlessCheckKey in this.getNeedlessCheckRightKey() )
							{
								if ( key.ToString() == needlessCheckKey )
								{
									needCheck = false;
									break;
								}
							}

							if ( needCheck )
							{
								if ( ApplicationService.Current().UserCode == null )
								{
									//ApplicationRun.GetInfoForm().Add("���ȵ�¼ϵͳ��");
									return;
								}

								// ���ResourceȨ��
								//Laws Lu,2005/08/31,�޸�	Admin��Ȩ�ޣ�����Pass
//								((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.OpenConnection();
								//2005/10/26,�޸�	��������
								object[] objUserGroup = ApplicationService.Current().LoginInfo.UserGroups;//(new BaseSetting.UserFacade(_domainDataProvider)).GetUserGroupofUser(ApplicationService.Current().UserCode);

								bool bIsAdmin = false;
								if(objUserGroup != null)
								{
									foreach(object o in objUserGroup)
									{
										if(((BenQGuru.eMES.Domain.BaseSetting.UserGroup)o).UserGroupType == "ADMIN")
										{
											bIsAdmin = true;
											break;
										}
									}
								}

								if (!bIsAdmin)
								{
//									if ( !this._facade.CheckResourceRight(ApplicationService.Current().UserCode, ApplicationService.Current().ResourceCode) )
//									{
//										throw new Exception("$Error_No_Resource_Right");	
//										return;
//									}
									if ( !this._facade.CheckAccessRight(ApplicationService.Current().UserCode, key.ToString().ToUpper()) )
									{
										//throw new Exception("û�з���Ȩ�ޣ�");	
										//ApplicationRun.GetInfoForm().Add(new UserControl.Message(UserControl.MessageType.Error, "$Error_No_Access_Right"));
										return;
									}
								}
								//							if ( ApplicationService.Current().UserCode != "ADMIN" && !this._facade.CheckAccessRight(ApplicationService.Current().UserCode, key.ToString()) )
								//							{
								//								ApplicationRun.GetInfoForm().Add("û�з���Ȩ�ޣ�");
								//								return;
								//							}	
							}

							if (menu.SubMenus[i].Command!=null)
							{							
								menu.SubMenus[i].Command.Execute();  
							}

							break;
						}

						if (menu.SubMenus[i].SubMenus != null)
						{
							DispathSubToolbarOnClick(key, menu.SubMenus[i]);
						}
					}
				}
			}
			catch
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.CloseConnection();
			}
			finally
			{
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_domainDataProvider).PersistBroker.CloseConnection();
			}
		}

		private void UpdateView(IMenu menu)
		{
			if (menu.Key==null)
			{
				return;
			}

			ApplicationService.Current().MainWindows.ultraExplorerBar.Groups.Clear(); 
			Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup barGroup = 
				ApplicationService.Current().MainWindows.ultraExplorerBar.Groups.Add(
				menu.Key.ToString().Trim(),  menu.Caption.ToString().Trim()); 
			//barGroup.ItemSettings.  Jarvis 20120504
            barGroup.Settings.ItemAreaInnerMargins.Right = 0;
            barGroup.Settings.ItemAreaOuterMargins.Right = 76;
			for (int i=0;i<menu.SubMenus.Length; i++)
			{	
				if (menu.SubMenus[i].Key!=null)
				{
					barGroup.Items.Add(menu.SubMenus[i].Key.ToString().Trim(), 
						menu.SubMenus[i].Caption.ToString().Trim());
				}
			}
		}

		private void ultraExplorerBar_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
		{
			DispathToolbarOnClick(e.Item.Key);
		}

		private string[] getNeedlessCheckRightKey()
		{
			return new string[]{ "BenQGuru.eMES.PDAClient.FLogin", "BenQGuru.eMES.PDAClient.Close" };
		}
	}
}
