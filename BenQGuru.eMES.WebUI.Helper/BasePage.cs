using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.MutiLanguage;
using BenQGuru.eMES.Security;
using BenQGuru.eMES.Common.DomainDataProvider;
using Infragistics.WebUI.UltraWebGrid;
using BenQGuru.eMES.Domain.MOModel;

using System.Xml;
using BenQGuru.Public.LicenseController;
using BenQGuru.eMES.BaseSetting;
using Infragistics.Web.UI.LayoutControls;
using System.Web.UI.WebControls;
using ControlLibrary.Web.Language;
using System.ComponentModel;
using Infragistics.Web.UI.GridControls;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Domain.Warehouse;

namespace BenQGuru.eMES.Web.Helper
{

    #region Cached Item
    //	/// <summary>
    //	/// Laws Lu,2007/03/21 ��Ʒ��Cache
    //	/// </summary>
    //	/// 
    //	[Serializable]
    //	public class CachedItem
    //	{
    //		public CachedItem()
    //		{
    //		
    //		}
    //
    //		public CachedItem(BenQGuru.eMES.Common.Domain.IDomainDataProvider dataProvider)
    //		{
    //			UpdateItem(dataProvider);
    //		}
    //		/// <summary>
    //		/// ����������
    //		/// </summary>
    //		public int LastUpdateDate;
    //		
    //		/// <summary>
    //		/// ������ʱ��
    //		/// </summary>
    //		public int LastUpdateTime;
    //
    //		/// <summary>
    //		/// ��Ʒ�б�
    //		/// </summary>
    //		public Hashtable ItemList;
    //		/// <summary>
    //		/// ��ȡ��Ʒ����
    //		/// </summary>
    //		/// <param name="itemCode">��Ʒ����</param>
    //		/// <returns>��Ʒ����</returns>
    //		public string getItemName(BenQGuru.eMES.Common.Domain.IDomainDataProvider dataProvider,string itemCode)
    //		{
    //			UpdateItem(dataProvider,itemCode);
    //
    //			if(ItemList.ContainsKey(itemCode))
    //			{
    //				return (ItemList[itemCode] as Item).ItemName;
    //			}
    //			else
    //			{
    //				return String.Empty;
    //			}
    //		}
    //		/// <summary>
    //		/// ���²�Ʒ�б�
    //		/// </summary>
    //		/// <param name="itemCode">��Ʒ����</param>
    //		/// <returns>��Ʒ����</returns>
    //		public void UpdateItem(BenQGuru.eMES.Common.Domain.IDomainDataProvider dataProvider,string itemCode)
    //		{
    //			string lastUpdate = FormatHelper.TODateTimeString(LastUpdateDate,LastUpdateTime);
    //			TimeSpan ts = DateTime.Parse(lastUpdate) - DateTime.Now;
    //			if(ts.TotalMinutes > 30)
    //			{
    //				object[] items = dataProvider.CustomQuery(typeof(Item),
    //					new Common.Domain.SQLCondition(
    //					String.Format("SELECT {0} FROM TBLITEM WHERE ITEMCODE = '{1}'"
    //					,Common.Domain.DomainObjectUtility.GetDomainObjectFieldsString(typeof(Item))
    //					,itemCode
    //					)));
    //
    //				if(items != null && items.Length > 0)
    //				{
    //					Item newItem = (items[0] as Item);
    //					if(ItemList.ContainsKey(newItem.ItemCode))
    //					{
    //						Item oldItem = (ItemList[newItem.ItemCode] as Item);
    //						//����ʱ�䲻һ��,�����ItemList
    //						if(newItem.MaintainDate != oldItem.MaintainDate
    //							|| newItem.MaintainTime != oldItem.MaintainTime)
    //						{
    //							object thisObj = new object();
    //				
    //							lock(thisObj)
    //							{
    //								ItemList[newItem.ItemCode] = newItem;
    //							}
    //
    //						}
    //					}
    //					else
    //					{
    //						object thisObj = new object();
    //				
    //						lock(thisObj)
    //						{
    //							ItemList.Add(newItem.ItemCode,newItem);
    //						}
    //					}
    //				}
    //			}
    //		}
    //
    //		/// <summary>
    //		/// ���²�Ʒ�б�
    //		/// </summary>
    //		/// <param name="itemCode">��Ʒ����</param>
    //		/// <returns>��Ʒ����</returns>
    //		public void UpdateItem(BenQGuru.eMES.Common.Domain.IDomainDataProvider dataProvider)
    //		{
    //			string lastUpdate = FormatHelper.TODateTimeString(LastUpdateDate,LastUpdateTime);
    //			TimeSpan ts = DateTime.Parse(lastUpdate) - DateTime.Now;
    //			if(ts.TotalMinutes > 30)
    //			{
    //				object[] items = dataProvider.CustomQuery(typeof(Item),
    //					new Common.Domain.SQLCondition(
    //					String.Format("SELECT {0} FROM TBLITEM",Common.Domain.DomainObjectUtility.GetDomainObjectFieldsString(typeof(Item))
    //					)));
    //
    //				if(items != null && items.Length > 0)
    //				{
    //					foreach(Item newItem in items)
    //					{
    //						if(ItemList.ContainsKey(newItem.ItemCode))
    //						{
    //							Item oldItem = (ItemList[newItem.ItemCode] as Item);
    //							//����ʱ�䲻һ��,�����ItemList
    //							if(newItem.MaintainDate != oldItem.MaintainDate
    //								|| newItem.MaintainTime != oldItem.MaintainTime)
    //							{
    //								object thisObj = new object();
    //								lock(thisObj)
    //								{
    //									ItemList[newItem.ItemCode] = newItem;
    //									break;
    //								}
    //
    //							}
    //						}
    //						else
    //						{
    //							object thisObj = new object();
    //				
    //							lock(thisObj)
    //							{
    //								ItemList.Add(newItem.ItemCode,newItem);
    //							}
    //						}
    //					}
    //				}
    //				else
    //				{
    //					lock(items)
    //					{
    //						ItemList.Clear();
    //					}
    //				}
    //			}
    //		}
    //	}

    #endregion

    /// <summary>
    /// BasePage ��ժҪ˵����
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        private string imBoss = String.Empty;
        private string _title = string.Empty;
        public const string FRAME_NAVIGATION = "frmNav";
        private static string Module_Prefix = "module_";
        ArrayList arrayListLabel = new ArrayList();
        ArrayList arrayListButton = new ArrayList();
        ArrayList arrayListCheckBox = new ArrayList();
        ArrayList arrayListPagerSizeSelector = new ArrayList();
        ArrayList arrayListHyperLink = new ArrayList();
        ArrayList arrayListRadioButtonList = new ArrayList();
        //provider 
        //���ݿ����,����ҳ��ʹ��(ʹ��ǰ��Ҫʵ����) Simone Xu
        //BasePage�Ḻ��ر����provider
        protected BenQGuru.eMES.Common.Domain.IDomainDataProvider DataProvider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = BenQGuru.eMES.Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider();
                }

                if (_provider is Common.DomainDataProvider.SQLDomainDataProvider)
                {
                    //Laws Lu,2007/04/03	�����¼�����û�
                    if ((_provider as Common.DomainDataProvider.SQLDomainDataProvider).PersistBroker.ExecuteUser == "MESDefaultUser")
                    {
                        (_provider as Common.DomainDataProvider.SQLDomainDataProvider).PersistBroker.ExecuteUser = GetUserCode();
                    }
                }
                return _provider;
            }

        }

        private BenQGuru.eMES.Common.Domain.IDomainDataProvider _provider;

        public BasePage()
            : base()
        {

        }

        //		public CachedItem Items
        //		{
        //			get
        //			{
        //				if(Application["ItemList"] != null)
        //				{
        //					return (Application["ItemList"] as CachedItem);
        //				}
        //				else
        //				{
        //					return null;
        //				}
        //			}
        //		}

        #region ����
        public virtual string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        /// <summary>
        ///  CSS��ʽ�ļ���ַ
        /// </summary>
        public virtual string StyleSheet
        {
            get
            {
                return string.Format("{0}{1}"
                    , this.VirtualHostRoot
                    , @"Skin/StyleSheet.css");
            }
        }

        public string VirtualHostRoot
        {
            get
            {
                //				return string.Format("http://{0}{1}{2}"
                //					, this.Request.Url.Host 
                //					, this.Request.Url.Segments[0]
                //					, this.Request.Url.Segments[1]);

                return string.Format("{0}{1}"
                    , this.Request.Url.Segments[0]
                    , this.Request.Url.Segments[1]);
            }
        }

        public string PageAction
        {
            get
            {
                if (this.ViewState["$State"] == null)
                {
                    return PageActionType.Add;
                }
                return (string)this.ViewState["$State"];
            }
            set
            {
                this.ViewState["$State"] = value;
            }
        }

        public string DepartmentName
        {
            get
            {
                string departmentName = "DepartmentName";
                if (System.Configuration.ConfigurationSettings.AppSettings["DepartmentName"] != null
                    && System.Configuration.ConfigurationSettings.AppSettings["DepartmentName"].Trim() != String.Empty)
                {
                    departmentName = System.Configuration.ConfigurationSettings.AppSettings["DepartmentName"].Trim();
                }
                return departmentName;
            }
        }
        #endregion

        #region ����

        //��ȡ����������� add by jinger 2016-01-27
        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        /// <param name="code">������ʹ���</param>
        /// <returns></returns>
        public string GetInvInName(string parameterCode)
        {
            SystemSettingFacade _SystemSettingFacade = new SystemSettingFacade(this.DataProvider);
            Domain.BaseSetting.Parameter parameter = (Domain.BaseSetting.Parameter)_SystemSettingFacade.GetParameter(parameterCode, "ININVTYPE");
            if (parameter != null)
            {
                return parameter.ParameterDescription;
            }
            return string.Empty;
        }


        //add by sam
        public string GetStNo(string stno)
        {
            if (stno.Trim() != string.Empty)
            {
                if (stno.IndexOf(",") >= 0)
                {
                    string[] lists = stno.Trim().Split(',');

                    for (int i = 0; i < lists.Length; i++)
                    {
                        lists[i] = "'" + lists[i] + "'";
                    }
                    stno = string.Join(",", lists);
                }
                else
                {
                    stno = "'" + stno + "'";
                }
            }
            return stno;
        }

        //��ȡ������������ add by jinger 2016-03-09
        /// <summary>
        /// ��ȡ������������
        /// </summary>
        /// <param name="code">�������ʹ���</param>
        /// <returns></returns>
        public string GetPickTypeName(string parameterCode)
        {
            SystemSettingFacade _SystemSettingFacade = new SystemSettingFacade(this.DataProvider);
            Domain.BaseSetting.Parameter parameter = (Domain.BaseSetting.Parameter)_SystemSettingFacade.GetParameter(parameterCode, "PICKTYPE");
            if (parameter != null)
            {
                return parameter.ParameterDescription;
            }
            return parameterCode;
        }

        //��ȡ����״̬���� add by jinger 2016-02-01
        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <param name="code">����״̬����</param>
        /// <returns></returns>
        public string GetStatusName(string parameterCode)
        {
            SystemSettingFacade _SystemSettingFacade = new SystemSettingFacade(this.DataProvider);
            Domain.BaseSetting.Parameter parameter = (Domain.BaseSetting.Parameter)_SystemSettingFacade.GetParameter(parameterCode.ToUpper(), "STATUS");
            if (parameter != null)
            {
                return parameter.ParameterDescription;
            }
            return parameterCode;
        }

        //��ȡ����״̬���� add by bela 2016-02-23
        /// <summary>
        /// ��ȡ������״̬����
        /// </summary>
        /// <param name="code">����״̬����</param>
        /// <returns></returns>
        public string GetLineStatusName(string parameterCode)
        {
            SystemSettingFacade _SystemSettingFacade = new SystemSettingFacade(this.DataProvider);
            Domain.BaseSetting.Parameter parameter = (Domain.BaseSetting.Parameter)_SystemSettingFacade.GetParameter(parameterCode.ToUpper(), "ASNLINESTATUS");
            if (parameter != null)
            {
                return parameter.ParameterDescription;
            }
            return parameterCode;
        }

        //��ȡSQE�ж�״̬���� add by bela 2016-02-27
        /// <summary>
        /// ��ȡSQE�ж�״̬����
        /// </summary>
        /// <param name="code">����״̬����</param>
        /// <returns></returns>
        public string GetSQEStatusName(string parameterCode)
        {
            SystemSettingFacade _SystemSettingFacade = new SystemSettingFacade(this.DataProvider);
            Domain.BaseSetting.Parameter parameter = (Domain.BaseSetting.Parameter)_SystemSettingFacade.GetParameter(parameterCode.ToUpper(), "SQESTATUS");
            if (parameter != null)
            {
                return parameter.ParameterDescription;
            }
            return parameterCode;
        }

        //��ȡ�������� add by jinger 2016-02-23
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        public string GetFacCode()
        {
            InventoryFacade _InventoryFacade = new InventoryFacade(this.DataProvider);
            object[] objInvoicesDetail = _InventoryFacade.GetAllInvoicesDetail();
            string facCode = "1";
            if (objInvoicesDetail != null)
            {
                foreach (InvoicesDetail invoicesDetail in objInvoicesDetail)
                {
                    if (!string.IsNullOrEmpty(invoicesDetail.FacCode))
                    {
                        facCode = invoicesDetail.FacCode;
                        break;
                    }
                }
            }
            return facCode;
        }


        /// <summary>
        /// ** ��������:	��õ�¼���û���
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-03-15
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        public string GetUserCode()
        {
            SessionHelper sessionHelper = SessionHelper.Current(this.Session);

            if (sessionHelper != null && sessionHelper.UserCode != null)
            {
                return sessionHelper.UserCode;
            }

            return "";
        }
        /// <summary>
        /// added by jessie lee, 2005/12/12
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            SessionHelper sessionHelper = SessionHelper.Current(this.Session);

            if (sessionHelper != null && sessionHelper.UserName != null)
            {
                return sessionHelper.UserName;
            }

            return "";
        }

        public bool IsBelongToAdminGroup()
        {
            SessionHelper sessionHelper = SessionHelper.Current(this.Session);

            if (sessionHelper != null && sessionHelper.UserName != null)
            {
                return sessionHelper.IsBelongToAdminGroup;
            }

            return false;
        }

        public string GetUserMail()
        {
            SessionHelper sessionHelper = SessionHelper.Current(this.Session);

            if (sessionHelper != null && sessionHelper.UserMail != null)
            {
                return sessionHelper.UserMail;
            }

            return "";
        }
        /// <summary>
        /// ** ��������:	���ҳ�����
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-03-15
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        public string GetRequestParam(string name)
        {
            string param = Server.UrlDecode(this.Request.Params[name]);
            if (param != null)
            {
                if (param.IndexOf(",") >= 0)
                {
                    param = param.Split(',')[1];
                }
                return param;
            }

            return "";
        }

        public string GetRequestPara1m(string name)
        {
            string param = Server.UrlDecode(this.Request.Params[name]);
            if (param != null)
            {
                if (param.IndexOf(",") >= 0)
                {
                    param = param.Split(',')[0];
                }
                return param;
            }

            return "";
        }

        public string GetRequestParamNotChange(string name)
        {
            string param = this.Request.Params[name];
            if (param != null)
            {
                return param;
            }

            return string.Empty;
        }

        public string MakeRedirectUrl(string url)
        {
            return Server.UrlPathEncode(url);
        }

        /// <summary>
        /// ** ��������:	��Url��Paramsƴ���ض���Url�����жԲ����͵�ַ���б���
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-05-21
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        public string MakeRedirectUrl(string url, string[] names, string[] values)
        {
            string[] paramList = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == null || names[i] == "")
                {
                    continue;
                }

                if (i > values.Length - 1)
                {
                    paramList[i] = string.Format("{0}={1}", Server.UrlEncode(names[i]), string.Empty);
                }
                else
                {
                    if (values[i] == null)
                    {
                        paramList[i] = string.Format("{0}={1}", Server.UrlEncode(names[i]), string.Empty);

                    }
                    else
                    {
                        paramList[i] = string.Format("{0}={1}", Server.UrlEncode(names[i]), Server.UrlEncode(values[i]));
                    }
                }
            }

            //return Server.UrlPathEncode( string.Format("{0}?{1}", url, string.Join("&", paramList)) );
            if (paramList.Length > 0)
            {
                return Server.UrlPathEncode(string.Format("{0}?{1}", url, string.Join("&", paramList)));
            }
            else
            {
                return Server.UrlPathEncode(url);
            }
        }

        /// <summary>
        /// �����Ե�Url����ȥhttp://emes/
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string GetRelativeUrl(Uri uri)
        {
            string[] segments = uri.AbsolutePath.Split('/');

            return string.Join("/", segments, 2, segments.Length - 2);
        }
        #endregion

        #region ����Դ
        /// <summary>
        ///  ���г�����ϵͳ�еİ�ť��ID�б��Ա����ʽ
        /// </summary>
        /// <returns></returns>
        protected string[] getButtons()
        {
            return new string[]{
                "cmdResolve","cmdOK", "cmdAdd", "cmdSave","cmdLeave","cmdNew","cmdCreat","cmdSendDown","cmdInStorage",
                "cmdStatusSTS","cmdCommit","cmdReturned2Inspection","cmdReturnedQC","cmdReturnedOQC",
                "cmdQuery", "cmdCancel","cmdReturn", 
                "cmdSelect","cmdInitial","cmdRelease",
                "cmdMOClose","cmdMOOpen","cmdDownload",
                "cmdPending","cmdPass","cmdNoPass",
                "cmdClose","cmdWait","cmdOpen","cmdConfirm",
                "cmdExit","cmdCopy","cmdCancelConfirm",
                "cmdCancelReject","cmdComfirm","cmdView", 
                "cmdCompare","cmdImport",/*"cmdExport"*/"cmdRefresh",
                "cmdPrint","cmdTrans","cmdAddLot",	//Added by Icyer
                "cmdConfined","cmdUnLock",
                "btnLockEdit","btnJSNEdit","btnTestQty","btnTestEndSN",
                //added by melo 2006.11.13
                "cmdEnter","cmdEnterBOM","cmdNGReasonSelect",/*"cmdDelete",*/"cmdViewBOM","cmdContrast","cmdExport",
                "cmdViewMO","cmdAnnulPending","cmdCloseMO","cmdValidate","cmdInitialize","cmdGoTrans","cmdAdjust",
                "cmdCloseAccount","cmdExportBook","cmdImportBook","cmdAppend","cmdAddItem","cmdAddProduct",
                "cmdDeleteItem","cmdDeleteProduct","cmdAddResource","cmdDeleteResource","cmdAddErrorCode",
                "cmdDeleteErrorCode","cmdAddUser","cmdDeleteUser","cmdExampleSelect","cmdComfirmReject","cmdBillClose",
                "cmdChangeMO","cmdStartEnd","cmdConfirmDifference","cmdDetail","cmdUploadError","cmdChangePassword",
                "cmdReToss","cmdBOM", "cmdSaveTotal", "cmdPublish", "cmdBack", "cmdNext","cmdPreview","cmdFinish",
                "cmdTest","cmdSure","cmdMOConfirm","cmdUpload","cmdCancelPause","cmdColseChecked","cmdSplit","cmdScrap","cmdOffMO",
                "cmdCreateIQCFromASN","cmdCreateIQCFromPO","cmdSendCheck","cmdIQCPrint","cmdIQCCancel","cmdIQCReceive","cmdIQCReceiveBatch",
                "cmdRegressToAcceptButton","cmdGoodChecked","cmdOKSuit","cmdCancelSuit","cmdSaveMailSubject","cmdQuickDeal","cmdSaveReturn","cmdDistribution",
                "cmdMailSetup","cmdCheck","cmdMerge","cmdProduct",//add by benny
                "cmdAddImport","cmdQueryASN","cmdCheckSN","cmdSubmitCarton","cmdClear","cmdRejectbt","cmdReceivebt","cmdGiveinbt","cmdPicUpLoad","cmdPicDelete"
                ,"cmdInitialCheck","cmdApplyIQC","cmdCreateBarCode","cmdPrintCode","cmdNoPackage","cmdAchieve","cmdExportSendGoodReceipt","cmdExportLightSendGoodReceipt","cmdExportLoadGoodReceipt","cmdSystemOutStorage","cmdApplyOQC",//add by sam
                "cmdLotSave","cmdConfirmOweQty", "cmdCreateNo",
                "cmdSaveIt","cmdPackingFinished"//add by Chris.H.Wang @20160305
                ,"cmdApply","cmdInOut","cmdShelves" //Add by Amy
                ,"cmdCreate","cmdLoadCheckResult","cmdSAPDNBack","cmdClosePick","cmdSoftwareOut","cmdStockCheckOp","cmdPackageOutLine","cmdStorageCheckClose",                                        
							   };
        }

        private string[] getLongButtons()
        {
            return new string[] { "cmdSAPStorageSync" };
        }

        /// <summary>
        /// ��ѯ���ܰ�ťID�б�
        /// </summary>
        /// <returns></returns>
        private string[] getReadControls()
        {
            return new string[]{"cmdQuery", "cmdView", "cmdRefresh",
								//added by melo 2006.11.13
								"cmdViewBOM","cmdViewMO","cmdPrint",};
        }

        /// <summary>
        /// д���ܰ�ťID�б�
        /// </summary>
        /// <returns></returns>
        private string[] getWriteControls()
        {
            return new string[]{
								   /*
				"cmdAdd", "cmdSave","cmdSelect",
				"cmdDownload","cmdRelease","cmdInitial","cmdPending","cmdMOOpen","cmdMOClose", //��������
				"cmdComfirm","cmdCancelConfirm","cmdCancelReject",//����
				"cmdPass","cmdNoPass",//����ǩ��
				"cmdchangeMO",
				"cmdOpen","cmdInitial","cmdWait"//��������
				*/

								   "cmdResolve","cmdOK", "cmdAdd", "cmdSave","cmdLeave","cmdNew",
								   /*"cmdQuery",*/ "cmdCancel",/*"cmdReturn", */
								   "cmdSelect","cmdInitial","cmdRelease",
								   /*"cmdMOClose","cmdMOOpen",*/"cmdDownload",
								   "cmdPending","cmdPass","cmdNoPass",
								   "cmdClose","cmdWait","cmdOpen","cmdConfirm",
								   /*"cmdExit",*/"cmdCopy","cmdCancelConfirm",
								   "cmdCancelReject","cmdComfirm",/*"cmdView",*/ 
								   "cmdCompare","cmdImport",/*"cmdExport","cmdRefresh",*/
								   /*"cmdPrint",*/"cmdTrans","cmdAddLot",
								   //added by melo 2006.11.13
								   "cmdEnter","cmdEnterBOM",/*"cmdNGReasonSelect","cmdDelete","cmdViewBOM",*/"cmdContrast","cmdExport",
								   /*"cmdViewMO",*/"cmdAnnulPending","cmdCloseMO","cmdValidate","cmdInitialize",/*"cmdGoTrans",*/"cmdAdjust",
								   "cmdCloseAccount",/*"cmdExportBook",*/"cmdImportBook","cmdAppend","cmdAddItem","cmdAddProduct",
								   /*"cmdDeleteItem","cmdDeleteProduct",*/"cmdAddResource",/*"cmdDeleteResource",*/"cmdAddErrorCode",
								   /*"cmdDeleteErrorCode",*/"cmdAddUser",/*"cmdDeleteUser","cmdExampleSelect",*/"cmdComfirmReject","cmdBillClose",
								   /*"cmdChangeMO",*/"cmdStartEnd","cmdConfirmDifference",/*"cmdDetail","cmdUploadError",*/"cmdChangePassword",
								   "cmdReToss","cmdMOConfirm","cmdUpload","cmdMerge","cmdProduct"
                                    ,"cmdInitialCheck","cmdApplyIQC","cmdLotSave","cmdConfirmOweQty","cmdCreateNo",
                                    "cmdCreate","cmdSAPDNBack","cmdClosePick"//add by sam
                                    ,"cmdSoftwareOut"
							   };
        }

        /// <summary>
        /// ɾ�����ܰ�ťID�б�
        /// </summary>
        /// <returns></returns>
        private string[] getDeleteControls()
        {
            return new string[]{"cmdDelete",
							//added by melo 2006.11.13
							"cmdDeleteItem","cmdDeleteProduct","cmdDeleteResource","cmdDeleteErrorCode","cmdDeleteUser"};
        }

        /// <summary>
        /// �������ܰ�ťID�б�    //Add by Anco 
        /// </summary>
        /// <returns></returns>
        private string[] getExportControls()
        {
            return new string[]{"cmdGridExport",
							//added by melo 2006.11.13
							"cmdMOClose","cmdMOOpen","cmdExport","cmdExportBook"};
        }

        /// <summary>
        /// ������Ȩ�޵�ҳ������
        /// </summary>
        /// <returns></returns>
        private string[] getNeedlessToCheckPage()
        {
            return new string[]{
								    "_StandardMaintainPage","_StandardMaintainPageDetail","FLogin",/*"FStartPage",*/ "FErrorPage", "FPageNavigator",
                                    "FUserPassWordModifyMP","FDownload",
								    "FItemSP","FModelSP","FMOSP","FOPSP","FErrorCodeSP",
								    "FItemSP2","FSingleItemSP","FSingleMOSP","FReworkMOSP",
                                    "FResourceSP","FSegmentSP","FMOSP2","FUserSP",
                                    "FStepSequenceSP","FStepSequenceSP2",
                                    "FRealTimeQuantityDetails","FSMTCopyDif",
                                    "FSMTBOMCompareDif","FSMTImportError",
                                    "FSingleReworkMOSP","FSingleRMAReworkMOSP","FSMTSelectMO","FMOBOMCompare",
									"FSingleSymptomSP","FSingleBUSP","FDepartmentSP",
									"FRMABillSP","FReMOSP","FSymptomSP","FShelfSP",
									"FFactorySP","FSingleFactorySP","FSingleCustomer","FCustomerSP",
									"FLoginNew","FLoginNewForSRM","cmdBOM","FOrgChangePage","FDatabaseChangePage","FSingleRouteSP","FSingleOPSP",
                                    "FSingleReworkCodeSP","FMModelCodeSP","FBigLineSP","FMmachinetypeSP","FSingleVendorSP",
                                    "FMaterialSP","FSingleMaterialSP","FsoftwareVersionImport","FPOMaterialSP2","FVendorSP",
                                    "FDutyCodeSP","FSingleDutyCodeSP","FProductionTypeSP","FOQCLotTypeSP","FMOMemoSP","FErrorCauseGroupSP","FReworkCodeSP","FStorageSP",
                                    "FCommonSingleSP","FCommonMultiSP","FExcelDataImp","FStackSP","FShiftCodeSP","FCrewCodeSP","FsingleExceptionCodeSP","FsingleShiftcodeBySSCodeSP",
                                    "FCreateUserSP","FInspectorSP","FIframePage","FException","FAlertNotice","FErrorCodeWithGroupSP","FErrorCauseWithGroupSP","FCKItemCodeSP","FEqpRepair",
                                    "FSingleStorageSP","FTransferSP","FSingleRes","FSingleItemSP"
                                       ,"FSingleInvNoSP","FSingleASNSP","FSingleInvNoMaterialSP","FSingleScrapMaterialSP","FASNSP","FSingleWWpoInvNoSP","FViewFieldEP.aspx",
                                       "FWWpoInvNoSP","FWWpoSP","FStorageCodes"
							   };
        }

        private string getTitleLabel()
        {
            return "lblTitle";
        }
        #endregion

        #region ��ʼ��
        /// <summary>
        /// ** ��������:	����UI����
        ///						�����еİ�ť����ͼƬ��ʽ
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-03-15
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        protected virtual void InitUI()
        {
            foreach (string button in this.getButtons())
            {
                System.Web.UI.Control control = this.FindControl(button);

                if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                {
                    ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["style"] = string.Format("BACKGROUND-IMAGE: url({0}Skin/Image/buttonblue-2013.gif);background-size:cover;", this.VirtualHostRoot);
                    ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["onmouseover"] = string.Format("document.getElementById('{0}').style.backgroundImage='url({1}Skin/Image/ButtonGray-2013.gif)';", button, this.VirtualHostRoot);
                    ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["onmouseout"] = string.Format("document.getElementById('{0}').style.backgroundImage='url({1}Skin/Image/buttonblue-2013.gif)';", button, this.VirtualHostRoot);
                }
            }

            foreach (string button in this.getLongButtons())
            {
                System.Web.UI.Control control = this.FindControl(button);

                if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                {
                    ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["style"] = string.Format("BACKGROUND-IMAGE: url({0}Skin/Image/ButtonBlueLong.gif)", this.VirtualHostRoot);
                    ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["onmouseover"] = string.Format("document.getElementById('{0}').style.backgroundImage='url({1}Skin/Image/ButtonDarkBlueLong.gif)';", button, this.VirtualHostRoot);
                    ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["onmouseout"] = string.Format("document.getElementById('{0}').style.backgroundImage='url({1}Skin/Image/ButtonBlueLong.gif)';", button, this.VirtualHostRoot);
                }
            }

            foreach (string button in this.getReadControls())
            {
                System.Web.UI.Control control = this.FindControl(button);

                if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                {
                    ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["style"] = string.Format("BACKGROUND-IMAGE: url({0}Skin/Image/query.gif);BACKGROUND-POSITION-Y: center;BACKGROUND-REPEAT: no-repeat", this.VirtualHostRoot);
                    ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["onmouseover"] = string.Empty;
                    ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["onmouseout"] = string.Empty;
                }
            }

            //Add by Jarvis 20121212 License
#if !DEBUG
            if (LicenseContinue.LicenseGo == string.Empty)
            {
                if (this.GetType().BaseType.Name != "FErrorPage")
                {
                    DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
                    DateTime dtNow = FormatHelper.ToDateTime(dbDateTime.DBDate, dbDateTime.DBTime);
                    ContextHelper.CurrentServerDate = dtNow;
                    LicenseInfo license = ContextHelper.License;
                    if (license.Status != LicenseStatus.Valid)
                    {
                        Exception ex = new Exception("$License_Expired , $Contact_SysAdmin ��");
                        ExceptionManager.Raise(this.GetType().BaseType, "$License_Expired", "", ex);
                        return;
                    }
                    //else
                    //{
                    //    DateTime exprieDate = license.ExpireDate;
                    //    TimeSpan ts = exprieDate - dtNow;
                    //    if (ts.Days <= LicenseContinue.LicenseAdvanceDays)
                    //    {
                    //        Exception ex = new Exception("$License_Will_Expire_After " + ts.Days + " $License_Will_Expire_Days , $Contact_SysAdmin ��");
                    //        ExceptionManager.Raise(this.GetType().BaseType, "$License_Will_Expire", "", ex);
                    //    }

                    //    //�������ڵ�¼ʱ/����ҳ����License
                    //    LicenseContinue.LicenseGo = "Continue";
                    //}

                }
            }
#endif
        }

        /// <summary>
        /// ��ʼ��ҳ�������
        /// </summary>
        /// <param name="languageControl"></param>
        /// <param name="forceLoad"></param>
        public void InitPageLanguage(ControlLibrary.Web.Language.LanguageComponent languageControl, bool forceLoad)
        {
            SessionHelper sessionHelper = SessionHelper.Current(this.Session);
            sessionHelper.InitMutiLanguage(languageControl, this, forceLoad);

            //melo 2006.11.1
            foreach (Control control in Page.Controls)
            {
                if (control != null)
                {
                    this.FindAllControls(control);
                    foreach (Control c in this.arrayListLabel)
                    {
                        if (c.ID != "lblPrimaryColor" && c.ID != "lblImportantColor" && c.ID != "lblSeverityColor")
                        {
                            string lblName = languageControl.GetString("$PageControl_" + c.ID.Substring(3));
                            System.Web.UI.Control lbl = this.FindControl(c.ID);

                            if (lbl == null)
                            {
                                lbl = c;
                            }

                            if (lblName != "" && lblName != null && ((System.Web.UI.WebControls.Label)lbl).Text != "" && ((System.Web.UI.WebControls.Label)lbl).Text != null)
                            {
                                ((System.Web.UI.WebControls.Label)lbl).Text = lblName;
                            }
                            else if (((System.Web.UI.WebControls.Label)lbl).Text != "" && ((System.Web.UI.WebControls.Label)lbl).Text != null)
                            {
                                ((System.Web.UI.WebControls.Label)lbl).Text = c.ID.Substring(3);
                            }
                        }
                    }
                    foreach (Control c in this.arrayListButton)
                    {
                        if (c.ID != null && c.ID.Trim() != "cmdHidden")
                        {
                            string buttonName = languageControl.GetString("$PageControl_" + c.ID.Substring(3));
                            System.Web.UI.Control button = this.FindControl(c.ID);

                            if (button == null)
                                continue;

                            if (buttonName != "" && ((System.Web.UI.HtmlControls.HtmlInputButton)button).Value.ToString().Trim() != "")
                            {
                                ((System.Web.UI.HtmlControls.HtmlInputButton)button).Value = buttonName;
                            }
                            else if (((System.Web.UI.HtmlControls.HtmlInputButton)button).Value.ToString().Trim() != "")
                            {
                                ((System.Web.UI.HtmlControls.HtmlInputButton)button).Value = c.ID.Substring(3);
                            }
                        }
                    }
                    foreach (Control c in this.arrayListCheckBox)
                    {
                        if (c.ID.Length > 3)
                        {
                            string checkBoxName = languageControl.GetString("$PageControl_" + c.ID.Substring(3));
                            System.Web.UI.Control checkBox = this.FindControl(c.ID);

                            if (checkBox == null)
                            {
                                checkBox = c;
                            }

                            if (checkBoxName != "" && checkBoxName != null && ((System.Web.UI.WebControls.CheckBox)checkBox).Text != "" && ((System.Web.UI.WebControls.CheckBox)checkBox).Text != null)
                            {
                                ((System.Web.UI.WebControls.CheckBox)checkBox).Text = checkBoxName;
                            }
                            else if (((System.Web.UI.WebControls.CheckBox)checkBox).Text != "" && ((System.Web.UI.WebControls.CheckBox)checkBox).Text != null)
                            {
                                ((System.Web.UI.WebControls.CheckBox)checkBox).Text = c.ID.Substring(3);
                            }
                        }
                    }
                    foreach (Control c in this.arrayListPagerSizeSelector)
                    {
                        string pagerSizeSelectorName = languageControl.GetString("$PageControl_" + c.ID);
                        System.Web.UI.Control pagerSizeSelector = this.FindControl(c.ID);
                        if (pagerSizeSelectorName != "" && pagerSizeSelectorName != null && ((BenQGuru.eMES.Web.Helper.PagerSizeSelector)pagerSizeSelector).Text != "" && ((BenQGuru.eMES.Web.Helper.PagerSizeSelector)pagerSizeSelector).Text != null)
                        {
                            ((BenQGuru.eMES.Web.Helper.PagerSizeSelector)pagerSizeSelector).Text = pagerSizeSelectorName;
                        }
                        else
                        {
                            ((BenQGuru.eMES.Web.Helper.PagerSizeSelector)pagerSizeSelector).Text = c.ID;
                        }
                    }
                    foreach (Control c in this.arrayListHyperLink)
                    {
                        if (c.ID == null)
                            continue;
                        string hyperLinkName = languageControl.GetString("$PageControl_" + c.ID.Substring(3));
                        System.Web.UI.Control hyperLink = this.FindControl(c.ID);
                        if (hyperLink == null)
                            continue;
                        if (hyperLinkName != "" && hyperLinkName != null && ((System.Web.UI.WebControls.HyperLink)hyperLink).Text != "" && ((System.Web.UI.WebControls.HyperLink)hyperLink).Text != null)
                        {
                            ((System.Web.UI.WebControls.HyperLink)hyperLink).Text = hyperLinkName;
                        }
                        else
                        {
                            ((System.Web.UI.WebControls.HyperLink)hyperLink).Text = c.ID.Substring(3);
                        }

                    }
                    foreach (Control c in this.arrayListRadioButtonList)
                    {
                        foreach (System.Web.UI.WebControls.ListItem item in ((System.Web.UI.WebControls.RadioButtonList)c).Items)
                        {
                            string listitemName = languageControl.GetString("$PageControl_" + c.ID.Substring(7) + "_" + item.Value);

                            if (listitemName != "" && listitemName != null)
                            {
                                item.Text = listitemName;
                            }
                            else
                            {
                                item.Text = item.Value;
                            }
                        }
                    }
                }
            }

            System.Web.UI.Control lblTitle = this.FindControl(this.getTitleLabel());

            if (lblTitle != null && lblTitle is System.Web.UI.WebControls.Label)
            {
                string moduleName = languageControl.GetString(Module_Prefix + sessionHelper.ModuleCode);
                if (moduleName != string.Empty)
                {
                    ((System.Web.UI.WebControls.Label)lblTitle).Text = moduleName;
                }
            }

            string word = languageControl.GetString("$" + this.GetType().BaseType.Name);
            if (word != string.Empty)
            {
                this.Title = word;
            }

            word = languageControl.GetString("deleteConfirm");

            if (word != string.Empty)
            {
                foreach (string id in this.getDeleteControls())
                {
                    System.Web.UI.Control control = this.FindControl(id);

                    if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                    {
                        ((System.Web.UI.HtmlControls.HtmlInputButton)control).Attributes["onclick"] = "{ return confirm('" + word + "'); }";
                    }
                }
            }
        }

        //����ҳ�����пؼ�
        private void FindAllControls(Control control)
        {
            if (control is System.Web.UI.WebControls.Label)
            {
                this.arrayListLabel.Add(control);
            }
            else if (control is System.Web.UI.HtmlControls.HtmlInputButton)
            {
                this.arrayListButton.Add(control);
            }
            else if (control is System.Web.UI.WebControls.CheckBox)
            {
                this.arrayListCheckBox.Add(control);
            }
            else if (control is BenQGuru.eMES.Web.Helper.PagerSizeSelector)
            {
                this.arrayListPagerSizeSelector.Add(control);
            }
            else if (control is System.Web.UI.WebControls.HyperLink)
            {
                this.arrayListHyperLink.Add(control);
            }
            else if (control is System.Web.UI.WebControls.RadioButtonList)
            {
                this.arrayListRadioButtonList.Add(control);
            }
            else
            {
                if (control != null)
                {
                    foreach (Control sonControls in control.Controls)
                    {
                        this.FindAllControls(sonControls);
                    }
                }
            }
        }

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
            //Laws Lu,2007/04/12 �����boss�򲻼��Session
            if (Page.Request.QueryString["Boss"] != null)
            {
                Session["Boss"] = Server.UrlDecode(Page.Request.QueryString["Boss"]);
            }
            if (Session["Boss"] != null)
            {
                bool hasReadRight = true;

                foreach (string id in this.getReadControls())
                {
                    System.Web.UI.Control control = this.FindControl(id);

                    if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                    {
                        ((HtmlInputButton)control).Disabled = !hasReadRight || ((HtmlInputButton)control).Disabled;
                    }
                }

                bool hasExportRight = true;

                foreach (string id in this.getExportControls())
                {
                    System.Web.UI.Control control = this.FindControl(id);

                    if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                    {
                        ((HtmlInputButton)control).Disabled = !hasExportRight || ((HtmlInputButton)control).Disabled;
                    }
                }

            }
            else
            {
                SessionHelper sessionHelper = SessionHelper.Current(this.Session);

                if (NeedCheck)
                {
                    // ���Session
                    this.CheckSession(sessionHelper);

                    string moduleCode = new FacadeFactory(this.DataProvider).CreateSystemSettingFacade().GetModuleCodeByUri(this.GetRelativeUrl(this.Request.Url));

                    if (moduleCode == null)
                    {
                        ExceptionManager.Raise(this.GetType().BaseType, "$Error_Module_Not_Exist", string.Format("[$ModuleCode={0}]", moduleCode));
                        return;
                    }

                    sessionHelper.ModuleCode = moduleCode;

                    int length = this.Request.QueryString.AllKeys.Length;
                    string[] keys = new string[length];
                    string[] values = new string[length];


                    for (int i = 0; i < length; i++)
                    {
                        keys[i] = this.Request.QueryString.AllKeys.GetValue(i).ToString();
                        values[i] = this.Request.QueryString[i].ToString();
                    }

                    sessionHelper.Url = this.MakeRedirectUrl(this.Request.Path, keys, values);

                    Hashtable urlHT = sessionHelper.Urls;

                    if (urlHT.Contains(this.Request.Path.ToUpper()))
                    {
                        urlHT.Remove(this.Request.Path.ToUpper());
                    }
                    urlHT.Add(this.Request.Path.ToUpper(), sessionHelper.Url);

                    sessionHelper.Urls = urlHT;

                    if (NeedCheckRights)
                    {
                        // ������Ȩ��
                        this.CheckAccessRights(sessionHelper);
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
            this.PreRender += new EventHandler(BasePage_PreRender);

            //#if !DEBUG
            if (this.GetType().BaseType.Name != "FErrorPage")
            {
                this.Error += new EventHandler(BasePage_Error);
            }
            //#endif

        }
        #endregion

        #region �¼�

        private void Page_Load(object sender, System.EventArgs e)
        {

            //���빫��js
            Page.ClientScript.RegisterClientScriptInclude("selectall", this.VirtualHostRoot + "Scripts/CommonScript.js");

            // changed by icyer 2007/05/08
            // ���ű��Ƴ�IsPostBack��������ǰģ��������ű�
            // ���ܣ���IE��"����"ʱ���ܸ��µ�����
            if (NeedCheck && this.GetType().BaseType.Name != "FStartPage" && this.GetType().BaseType.Name != "FPageEntry")
            {
                SessionHelper sessionHelper = SessionHelper.Current(this.Session);
                // vizo:20060617
                // ���˸�try-catch
                // ��ֹ�ű�����
                //Response.Write("<script>");
                //Response.Write("try{");
                //Response.Write(string.Format("if (window.parent.frames('{0}').CheckCurrentModuleCode('{1}')==false) ", FRAME_NAVIGATION, sessionHelper.ModuleCode));
                //Response.Write(string.Format("window.parent.frames('{0}').location.replace('{1}FPageNavigator.aspx?modulecode={2}&currentmoduleurl={3}');", FRAME_NAVIGATION, this.VirtualHostRoot, sessionHelper.ModuleCode, Server.UrlEncode(sessionHelper.Url)));
                //Response.Write("}catch(e){}");
                //Response.Write("</script>");

                string strJs = @"<script>    
                try{
                    if (window.parent.frames('" + FRAME_NAVIGATION + @"').CheckCurrentModuleCode('{" + sessionHelper.ModuleCode + @"}')==false)
                    {
                         window.parent.frames('" + FRAME_NAVIGATION + @"').location.replace('" + this.VirtualHostRoot + @"FPageNavigator.aspx?modulecode=" + sessionHelper.ModuleCode + "&currentmoduleurl=" + Server.UrlEncode(sessionHelper.Url) + @"');
                    }
                }catch(e){}
                </script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "refresh", strJs);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), strJs, false);
            }
            if (!IsPostBack)
            {

                //				this.Items.UpdateItem(this.DataProvider);
                // ��ҳ��д�ű���ˢ�µ�����

                this.InitUI();
            }
        }

        /// <summary>
        /// ���쳣��Ϣ��������ҳ��
        /// </summary>
        private void BasePage_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            string errMsg = string.Join("@", ex.Message.Split(Environment.NewLine.ToCharArray()));
            string innerErrMsg = "";
            ArrayList errList = new ArrayList();

            Exception innerEx = ex.InnerException;

            while (innerEx != null)
            {
                if (innerEx.Message != null)
                {
                    errList.Add(string.Join("@", innerEx.Message.Split(Environment.NewLine.ToCharArray())));
                }

                innerEx = innerEx.InnerException;
            }

            innerErrMsg = string.Join("@", (string[])errList.ToArray(typeof(string)));

            Server.ClearError();

            //����DIV��ķ�ʽ����,added by Gawain@20130902
            LanguageComponent lang = new LanguageComponent(new Container());
            errMsg = MessageCenter.ParserMessage(errMsg, lang).Replace("@", Environment.NewLine);
            innerErrMsg = FormatHelper.CleanString(MessageCenter.ParserMessage(innerErrMsg, lang)).Replace("@", Environment.NewLine);
            try
            {
                //Server.Transfer(this.MakeRedirectUrl(string.Format("{0}FExceptionNew.htm", this.VirtualHostRoot),
                //    new string[] { "msg", "innermsg" },
                //    new string[] { errMsg, innerErrMsg }), false);
                //    string strScript = "parent.window.frames['frmWorkSpace'].src='"+ this.MakeRedirectUrl(string.Format("{0}FExceptionNew.htm", this.VirtualHostRoot),
                //        new string[] { "msg", "innermsg" },
                //        new string[] { errMsg, innerErrMsg })+"'";
                //    ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), strScript, true);
                //
                Response.Write(string.Format(@"<script>
     
                try
                {{                   
                    window.top.showErrorDialog('{0}','{1}');
                    if(window.top.frmWorkSpace.iframe1==null)
                    {{
                        window.top.history.back(-1);
                    }}
                }}
                catch(e)
                {{
                    alert('{0},{1}');
                }}   
            
                </script>", jsEncoder(errMsg), jsEncoder(innerErrMsg)));

            }
            catch
            {
            }
            finally
            {
                //Server.Transfer(this.MakeRedirectUrl(string.Format("{0}FExceptionNew.htm", this.VirtualHostRoot),
                //    new string[] { "msg", "innermsg" },
                //    new string[] { errMsg, innerErrMsg }));
            }


            /* modified by jessie lee, 2005/12/27
             * ����ض���ʱ��������� */
            //try
            //{
            //    this.Response.Redirect(this.MakeRedirectUrl(string.Format("{0}FErrorPage.aspx", this.VirtualHostRoot),
            //        new string[] { "msg", "innermsg" },
            //        new string[] { errMsg, innerErrMsg }), false);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    Server.Transfer(this.MakeRedirectUrl(string.Format("{0}FErrorPage.aspx", this.VirtualHostRoot),
            //        new string[] { "msg", "innermsg" },
            //        new string[] { errMsg, innerErrMsg }));
            //}
        }

        /**Encode for Javascript. */
        public static String jsEncoder(String str)
        {
            if (str == null || str.Equals(""))
                return "";
            String res_str;
            res_str = str.Replace("\\", "\\\\");    //��\�滻��\\ 
            res_str = res_str.Replace("'", "\\'");    //��'�滻��\' 
            res_str = res_str.Replace("\"", "\\\"");//��"�滻��\"  
            res_str = res_str.Replace("\r\n", "\\\n");//��\r\n�滻��\\n     
            res_str = res_str.Replace("\n", "\\\n");//��\n�滻��\\n     
            res_str = res_str.Replace("\r", "\\\n");//��\r�滻��\\n     
            return res_str;
        }

        private void BasePage_PreRender(object sender, EventArgs e)
        {
            SessionHelper sessionHelper = SessionHelper.Current(this.Session);
            //Laws Lu,2007/04/12 �����boss�򲻼��Session
            if (Session["Boss"] != null)
            {
                bool hasReadRight = true;

                foreach (string id in this.getReadControls())
                {
                    System.Web.UI.Control control = this.FindControl(id);

                    if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                    {
                        ((HtmlInputButton)control).Disabled = !hasReadRight || ((HtmlInputButton)control).Disabled;
                    }
                }

                bool hasExportRight = true;

                foreach (string id in this.getExportControls())
                {
                    System.Web.UI.Control control = this.FindControl(id);

                    if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                    {
                        ((HtmlInputButton)control).Disabled = !hasExportRight || ((HtmlInputButton)control).Disabled;
                    }
                }

            }
            else if (NeedCheck)
            {
                // ���Session

                this.CheckSession(sessionHelper);

                // ����ҳ��Ȩ��
                if (NeedCheckRights)
                {
                    this.SetViewState(sessionHelper);
                }
            }
        }

        /* Gawain ���� ����� �����ԣ���ʱע��
        protected override void Render(HtmlTextWriter writer)
        {
            //Removed by Icyer 2007/02/27		�������޸�
            //base.Render (writer);

            //// ��ȫѡ��ť����Grid�ĵ�һ�е�һ��
            //writer.Write("<script language=javascript>ResetSelectAllPosition('chbSelectAll','gridWebGrid')</script>");
           
            // Added by Icyer 2007/02/27
            // ����Grid�Ŀ�ȣ�ʹ֮ÿһ�ж��������ɵ������
            // ����Ч����Ҫ�ﵽ����Ҫ��Grid�����еĿ����Ҫ��������Grid��ÿһ�ж�����ֻ�е������
            // ԭ��
            //		ԭ���Ĵ����У�����ColWidthDefaultΪ�գ�Grid���Զ�ƽ�������п�����IE�е����п��ʱ��ֻ�ܵ���һ��
            //		�����Է��֣�ֻҪÿ�ж������ò�Ϊ�յ��п��������ɵ���
            //		���һ�еĿ������Ϊ�ٷֱȣ�IE������п�ʱ����Ӱ�����ڵ��п�
            // �޸ģ�
            //		��BasePage��Render�У���������Grid���Ƚ�������(û�����ù̶��п����)���п�����Ϊ�ٷֱ� (100 / ������)
            //		����ű���䣬��ÿ�еĿ���ɰٷֱȵ���Ϊ����
            // ��ԭ�еĲ��죺
            //		��ҳ��ı��Сʱ��ԭ����Grid���ݲ��ֻ��Զ�������С��Header�������
            //			�������ݲ��ֺ�Header�������Զ�����

           // string strScript = SetWebGridColumnWidth();		// ��ȡ����Ľű���䣬��ΪҪ����Grid�п�����Ҫ��base.Render֮ǰ

            base.Render(writer);

            //writer.WriteLine(strScript);	// ����ű�

            // ��ȫѡ��ť����Grid�ĵ�һ�е�һ��
            writer.Write("<script language=javascript>ResetSelectAllPosition('chbSelectAll','gridWebGrid')</script>");
            writer.Write("<script language=javascript>ResetSelectAllPosition('chbSelectAll1','gridWebGrid1')</script>");
            writer.Write("<script language=javascript>ResetSelectAllPosition('chbSelectAll2','gridWebGrid2')</script>");

            //���javascript: resetSelectedCheckBox
            string resetSelectedCheckBox = @"
<script language='javascript'>
    function resetSelectedCheckBox()
    {

        var webGrids = document.all.tags('TABLE');
        for (var i=0; i < webGrids.length; i++)  
        {
            if (webGrids[i].id.indexOf('G_') == 0)
            {
                var gridName = webGrids[i].id.substring(2, webGrids[i].id.length);
                var grid = igtbl_getGridById(gridName);
                if (!grid) return;
                //����Ϊ��ȡWebGrid�ؼ�

                var checkBoxs = document.all.tags('INPUT');
                for (var j=0; j < checkBoxs.length; j++)  
                {
                    if (checkBoxs[j].type == 'checkbox' && checkBoxs[j].onpropertychange != null && checkBoxs[j].parentElement.parentElement.id.indexOf(gridName + 'rc') == 0)
                    {
                    //����Ϊ��ȡChekcBox

                        if (checkBoxs[j].checked)  
                        {
                            //ʹ��igtbl_saveChangedCell����ChekcBox
                            var cell = checkBoxs[j].parentNode;
                            while(cell && !(cell.tagName == 'TD' && cell.id != '')) cell = cell.parentNode;
                            if(cell && grid)
                            {
                                var rowId = igtbl_getCellById(cell.id).Row.Element.id;
                                igtbl_saveChangedCell(grid,rowId,cell.id,checkBoxs[j].checked.toString());
                            }
                        }
                    }     
                } 
            }   
        }
    } 

    function getFuncContent(funcBody)
    {
        var pos = funcBody.indexOf(')');
        if (pos >= 10) return funcBody.substring(pos+1, funcBody.length);
        else return funcBody;
    }

    //����document.body.onloadʹ��resetSelectedCheckBox����������
    if(document.body.onload.toString()) 
    { 
        document.body.onload=new Function(getFuncContent(document.body.onload.toString()) + 'resetSelectedCheckBox();'); 
    } 
    else 
    {
        document.body.onload=new Function('resetSelectedCheckBox();'); 
    }

</script> ";
            writer.Write(resetSelectedCheckBox);

        }

        private string _setWebGridColumnWidthScriptFormat = @"
	<script langauge=""javascript"">
		function SetWebGridColumnWidth(gridId)
		{
			var grid = igtbl_getGridById(gridId);
			var oBands = grid.Bands;
			var oBand = oBands[0];
			var oColumns = oBand.Columns;
			var count = oColumns.length;
			var obj;

			var i;
			var columnId;
			for (i = 0; i < count; i++)
			{
				columnId = gridId + 'c_0_' + i;
				obj = document.getElementById(columnId);
				var iWidth2;
				iWidth2 = obj.clientWidth;
				igtbl_resizeColumn(gridId,columnId,iWidth2);
			}
		}

{0}

	</script>
";
        // ���������п�Ľű����
        private string SetWebGridColumnWidth()
        {
            ArrayList listGrid = new ArrayList();
            // �������е�Grid
            SetWebGridColumnWidth(this, listGrid);
            if (listGrid.Count > 0)
            {
                string strScript = "";
                // ÿ��Grid������SetWebGridColumnWidth�ű�
                for (int i = 0; i < listGrid.Count; i++)
                {
                    strScript += "SetWebGridColumnWidth('" + ((Control)listGrid[i]).ID + "');\r\n";
                }
                string strOutput = _setWebGridColumnWidthScriptFormat.Replace("{0}", strScript);
                return strOutput;
            }
            else
                return "";
        }
        // �������е�Grid
        private void SetWebGridColumnWidth(Control ctl, ArrayList listGrid)
        {
            for (int i = 0; i < ctl.Controls.Count; i++)
            {
                if (ctl.Controls[i] is Infragistics.WebUI.UltraWebGrid.UltraWebGrid)
                {
                    if (AdjustWebGridColumnWidth(ctl.Controls[i]) == true)		// �Ƿ���Ҫ����Grid�п�
                    {
                        listGrid.Add(ctl.Controls[i]);
                    }
                }
                else
                {
                    SetWebGridColumnWidth(ctl.Controls[i], listGrid);
                }
            }
        }
        // ����Grid�п�
        private bool AdjustWebGridColumnWidth(Control ctl)
        {
            Infragistics.WebUI.UltraWebGrid.UltraWebGrid grid = (Infragistics.WebUI.UltraWebGrid.UltraWebGrid)ctl;

            ArrayList listAdjustIdx = new ArrayList();
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                // ԭʼ�п��ǿյ��У�������Ҫ��������
                if (grid.Columns[i].Hidden == false && grid.Columns[i].Width == System.Web.UI.WebControls.Unit.Empty)
                {
                    listAdjustIdx.Add(i);
                }
            }
            if (listAdjustIdx.Count == 0)
                return false;
            else
            {
                // ����ÿһ�е��п�Ϊ�ٷֱ�
                int iWidthPer = Convert.ToInt32(100 / listAdjustIdx.Count);
                for (int i = 0; i < listAdjustIdx.Count; i++)
                {
                    grid.Columns[Convert.ToInt32(listAdjustIdx[i])].Width = System.Web.UI.WebControls.Unit.Percentage(Convert.ToDouble(iWidthPer));
                }
                return true;
            }
        }
        */
        #endregion

        #region Ȩ�޼��
        /// <summary>
        /// ** ��������:	������Ȩ��
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-03-15
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        private void CheckAccessRights(SessionHelper sessionHelper)
        {
            SecurityFacade facade = new FacadeFactory(this.DataProvider).CreateSecurityFacade();

            if (!facade.CheckAccessRight(sessionHelper.UserCode, this.GetRelativeUrl(this.Request.Url)))
            {
                ExceptionManager.Raise(this.GetType().BaseType, "$Error_No_Access_Right", new Exception(this.GetRelativeUrl(this.Request.Url)));
            }
            //joe ��ͨ��ȫ Pagelog
            else
            {
                string Page = this.GetRelativeUrl(this.Request.Url).ToString();
                ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.OpenConnection();

                try
                {

                    XmlDocument xmldoc = new XmlDocument();
                    string constr = "";
                    string ProgramBool = "";
                    xmldoc.Load(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\dblog.xml");

                    XmlNodeReader xr = new XmlNodeReader(xmldoc);

                    while (xr.Read())
                    {
                        if (xr.GetAttribute("name") == "BS")
                        {
                            ProgramBool = xr.ReadString();
                        }
                        if (xr.GetAttribute("name") == "Constr")
                        {
                            constr = xr.ReadString();
                        }
                    }
                    if (ProgramBool == "true")
                    {
                        DataProvider.BeginTransaction();

                        string PageLog = "insert into tblpagelog (muser,mtime,page) values ('" + sessionHelper.UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + Page + "')";

                        ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.Execute(PageLog);
                        DataProvider.CommitTransaction();
                    }
                }
                catch
                {
                    DataProvider.RollbackTransaction();
                }
                finally
                {
                    ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
                }

            }

        }
        /// <summary>
        /// ** ��������:	����ViewValue����ҳ��״̬
        /// ** �� ��:		Jane Shu
        /// ** �� ��:		2005-03-15
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        private void SetViewState(SessionHelper sessionHelper)
        {
            SecurityFacade facade = new FacadeFactory(this.DataProvider).CreateSecurityFacade();


            string viewState = facade.GetViewValueOfUserByUrl(sessionHelper.UserCode, this.GetRelativeUrl(this.Request.Url));

            #region ReadRight

            bool hasReadRight = facade.HasRight(viewState, RightType.Read);

            foreach (string id in this.getReadControls())
            {
                System.Web.UI.Control control = this.FindControl(id);

                if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                {
                    ((HtmlInputButton)control).Disabled = !hasReadRight || ((HtmlInputButton)control).Disabled;
                }
            }

            #endregion

            #region WriteRight

            bool hasWriteRight = facade.HasRight(viewState, RightType.Write);

            foreach (string id in this.getWriteControls())
            {
                System.Web.UI.Control control = this.FindControl(id);

                if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                {
                    ((HtmlInputButton)control).Disabled = !hasWriteRight || ((HtmlInputButton)control).Disabled;
                }
            }

            #endregion

            #region DeleteRight

            bool hasDeleteRight = facade.HasRight(viewState, RightType.Delete);

            foreach (string id in this.getDeleteControls())
            {
                System.Web.UI.Control control = this.FindControl(id);

                if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                {
                    ((HtmlInputButton)control).Disabled = !hasDeleteRight || ((HtmlInputButton)control).Disabled;
                }
            }

            #endregion

            #region ExportRight

            bool hasExportRight = facade.HasRight(viewState, RightType.Export);

            foreach (string id in this.getExportControls())
            {
                System.Web.UI.Control control = this.FindControl(id);

                if (control != null && control is System.Web.UI.HtmlControls.HtmlInputButton)
                {
                    ((HtmlInputButton)control).Disabled = !hasExportRight || ((HtmlInputButton)control).Disabled;
                }
            }

            #endregion

        }    //Add by Anco

        /// <summary>
        /// ҳ���Ƿ���Ҫ���Session/Ȩ��
        /// </summary>
        private bool NeedCheck
        {
            get
            {
                bool isNeedlessToCheck = true;
                foreach (string page in this.getNeedlessToCheckPage())
                {
                    if (page == this.GetType().BaseType.Name)
                    {
                        isNeedlessToCheck = false;
                        break;
                    }
                }

                return isNeedlessToCheck;
            }
        }

        /// <summary>
        /// �û��Ƿ���Ҫ���Ȩ��
        /// </summary>
        private bool NeedCheckRights
        {
            get
            {
                if (this.GetUserCode().ToUpper() == "ADMIN" || this.IsBelongToAdminGroup())
                {
                    return false;
                }

                return true;
            }
        }

        public virtual void CheckSession(SessionHelper sessionHelper)
        {
            // Session����
            if (!sessionHelper.IsLogin)
            {
                sessionHelper.RemoveAll();
                this.Response.Write(string.Format("<script language=javascript>window.top.location.href='{0}FLoginNew.aspx'</script>", this.VirtualHostRoot));
                this.Response.End();
            }
        }

        #endregion

        #region ClosePersistBroker

        protected override void OnUnload(EventArgs e)
        {
            if (this._provider != null)
            {
                ((SQLDomainDataProvider)_provider).PersistBroker.CloseConnection();
            }
            base.OnUnload(e);
        }


        #endregion

        #region Export UltraWebGrid
        /// <summary>
        /// Export UltraWebGrid
        /// not to export hidden column
        /// </summary>
        /// <param name="grid">Infragistics.WebUI.UltraWebGrid.UltraWebGrid</param>
        public void GridExport(Infragistics.WebUI.UltraWebGrid.UltraWebGrid grid)
        {
            string downloadPhysicalPath = this.Request.PhysicalApplicationPath + @"upload\";
            if (!Directory.Exists(downloadPhysicalPath))
            {
                Directory.CreateDirectory(downloadPhysicalPath);
            }

            string filename = string.Format("Export_{0}_{1}", FormatHelper.TODateInt(System.DateTime.Now).ToString(), FormatHelper.TOTimeInt(System.DateTime.Now).ToString());
            string filepath = string.Format(@"{0}{1}{2}", downloadPhysicalPath, filename, ".xls");

            while (File.Exists(filepath))
            {
                filename = string.Format("{0}_{1}", filename, "0");
                filepath = string.Format(@"{0}{1}{2}", downloadPhysicalPath, filename, ".xls");
            }

            StreamWriter writer = new StreamWriter(filepath, false, System.Text.Encoding.Unicode);
            writer.Write(this.GetXlsString(grid));
            writer.Flush();
            writer.Close();

            // ����������ļ��������ṩ�˵���
            string pageVirtualHostRoot = string.Format("{0}{1}"
                , this.Page.Request.Url.Segments[0]
                , this.Page.Request.Url.Segments[1]);

            /*
            string script = string.Format( @"<script language=javascript> 
                                        pop=window.open('" + string.Format( @"{0}FDownload.aspx", pageVirtualHostRoot )
                + "?fileName="
                + string.Format( @"{0}{1}", pageVirtualHostRoot+"upload/", filename+".xls" ) 
                +@"', 'ExportWindow', 'top=60000,left=60000,height=1,width=1,status=no,toolbar=no,menubar=no,location=no'); 
                                        if(window.location.pathname.split('/')[3]!='FSMTImport.aspx')
                                        history.back()
                                </script>");

            this.Response.Write( script );
            */
            Response.Write(@"<iframe width='0' height='0' src="
                + string.Format(@"{0}FDownload.aspx", pageVirtualHostRoot)
                + "?fileName=" + string.Format(@"{0}{1}", pageVirtualHostRoot + "upload/", filename + ".xls")
                + "></iframe><script language=javascript>window.setTimeout('history.back()',2000);</script>"
                );
        }

        private string GetXlsString(Infragistics.WebUI.UltraWebGrid.UltraWebGrid grid)
        {
            StringBuilder strBuilder = new StringBuilder();
            int columnCount = grid.Columns.Count;
            for (int i = 0; i < columnCount; i++)
            {
                if (!grid.Columns[i].Hidden)
                {
                    if (i < columnCount - 1)
                    {
                        strBuilder.Append(grid.Columns[i].HeaderText.Replace("<br>", "~") + "\t");
                    }
                    else
                    {
                        strBuilder.Append(grid.Columns[i].HeaderText.Replace("<br>", "~") + "\r\n");
                    }
                }
            }

            int rowCount = grid.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                UltraGridRow row = grid.Rows[i];
                if (row != null && !row.Hidden)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (!row.Cells[j].Column.Hidden)
                        {
                            if (j < columnCount - 1)
                            {
                                strBuilder.Append(row.Cells[j].Text + "\t");
                            }
                            else
                            {
                                strBuilder.Append(row.Cells[j].Text + "\r\n");
                            }
                        }
                    }
                }
            }

            return strBuilder.ToString();

        }
        #endregion

        #region Export WebDataGrid
        public void GridExport(WebDataGrid grid)
        {
            string downloadPhysicalPath = this.Request.PhysicalApplicationPath + @"upload\";
            if (!Directory.Exists(downloadPhysicalPath))
            {
                Directory.CreateDirectory(downloadPhysicalPath);
            }

            string filename = string.Format("Export_{0}_{1}", FormatHelper.TODateInt(System.DateTime.Now).ToString(), FormatHelper.TOTimeInt(System.DateTime.Now).ToString());
            string filepath = string.Format(@"{0}{1}{2}", downloadPhysicalPath, filename, ".xls");

            while (File.Exists(filepath))
            {
                filename = string.Format("{0}_{1}", filename, "0");
                filepath = string.Format(@"{0}{1}{2}", downloadPhysicalPath, filename, ".xls");
            }



            StreamWriter writer = new StreamWriter(filepath, false, System.Text.Encoding.Unicode);
            writer.Write(this.GetXlsString(grid));
            writer.Flush();
            writer.Close();

            // ����������ļ��������ṩ�˵���
            string pageVirtualHostRoot = string.Format("{0}{1}"
                , this.Page.Request.Url.Segments[0]
                , this.Page.Request.Url.Segments[1]);

            /*
            string script = string.Format( @"<script language=javascript> 
                                        pop=window.open('" + string.Format( @"{0}FDownload.aspx", pageVirtualHostRoot )
                + "?fileName="
                + string.Format( @"{0}{1}", pageVirtualHostRoot+"upload/", filename+".xls" ) 
                +@"', 'ExportWindow', 'top=60000,left=60000,height=1,width=1,status=no,toolbar=no,menubar=no,location=no'); 
                                        if(window.location.pathname.split('/')[3]!='FSMTImport.aspx')
                                        history.back()
                                </script>");

            this.Response.Write( script );
            */
            //Response.Write(@"<iframe src="
            //    + string.Format(@"'{0}FDownload.aspx", pageVirtualHostRoot)
            //    + "?fileName=" + string.Format(@"{0}{1}", pageVirtualHostRoot + "upload/", filename + ".xls'")
            //    + "></iframe><script language='javascript'>window.setTimeout('history.back()',2000);</script>"
            //    );

            this.DownloadFile(filename);
        }

        //��һ�в�����
        public void GridExportExcColFir(WebDataGrid grid)
        {
            string downloadPhysicalPath = this.Request.PhysicalApplicationPath + @"upload\";
            if (!Directory.Exists(downloadPhysicalPath))
            {
                Directory.CreateDirectory(downloadPhysicalPath);
            }

            string filename = string.Format("Export_{0}_{1}", FormatHelper.TODateInt(System.DateTime.Now).ToString(), FormatHelper.TOTimeInt(System.DateTime.Now).ToString());
            string filepath = string.Format(@"{0}{1}{2}", downloadPhysicalPath, filename, ".xls");

            while (File.Exists(filepath))
            {
                filename = string.Format("{0}_{1}", filename, "0");
                filepath = string.Format(@"{0}{1}{2}", downloadPhysicalPath, filename, ".xls");
            }



            StreamWriter writer = new StreamWriter(filepath, false, System.Text.Encoding.Unicode);
            writer.Write(this.GetXlsExcColFir(grid));
            writer.Flush();
            writer.Close();

            this.DownloadFile(filename);
        }

        /// <summary>
        /// ����excel
        /// </summary>
        /// <param name="filename"></param>
        public void DownloadFile(string filename)
        {
            string pageVirtualHostRoot = string.Format("{0}{1}"
                , this.Page.Request.Url.Segments[0]
                , this.Page.Request.Url.Segments[1]);


            DownloadFileFull(pageVirtualHostRoot + "upload/" + filename + ".xls");

        }

        public void DownloadFileFull(string fullFilename)
        {
            // ����������ļ��������ṩ�˵���
            string pageVirtualHostRoot = string.Format("{0}{1}"
                , this.Page.Request.Url.Segments[0]
                , this.Page.Request.Url.Segments[1]);


            string strScript = @"var frameDown =$('<a></a>');
            frameDown.appendTo($('form'));
            frameDown.html('<span></span>');
            //frameDown.attr('target','frmWorkSpace');
            frameDown.attr('href', '"
      + string.Format(@"{0}FDownload.aspx", pageVirtualHostRoot)
      + "?fileName=" + string.Format(@"{0}", Server.UrlEncode(fullFilename))
      + @"');
            frameDown.children().click();
            frameDown.remove();";
            if (ScriptManager.GetCurrent(this.Page) != null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
        }

        protected void RunScript(string strScript)
        {
            if (ScriptManager.GetCurrent(this.Page) != null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        private string GetXlsString(WebDataGrid grid)
        {
            StringBuilder strBuilder = new StringBuilder();
            int columnCount = grid.Columns.Count;

            for (int i = 0; i < columnCount; i++)
            {
                if (!grid.Columns[i].Hidden)
                {
                    if (i < columnCount - 1)
                    {
                        strBuilder.Append(grid.Columns[i].Header.Text.Replace("<br>", "~") + "\t");
                    }
                    else
                    {
                        strBuilder.Append(grid.Columns[i].Header.Text.Replace("<br>", "~") + "\r\n");
                    }
                }

            }
            if (columnCount >= 1 && grid.Columns[columnCount - 1].Hidden)
            {
                strBuilder.Append("\r\n");
            }
            int rowCount = grid.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                GridRecord row = grid.Rows[i];
                if (row != null)//&& !row.Hidden)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (!row.Items[j].Column.Hidden)
                        {
                            if (j < columnCount - 1)
                            {
                                strBuilder.Append(row.Items[j].Text + "\t");
                            }
                            else
                            {
                                strBuilder.Append(row.Items[j].Text + "\r\n");
                            }
                        }
                    }
                }
                if (grid.Columns[columnCount - 1].Hidden)
                {
                    strBuilder.Append("\r\n");
                }
            }

            return strBuilder.ToString();

        }


        //����ȡ��һ������
        private string GetXlsExcColFir(WebDataGrid grid)
        {
            StringBuilder strBuilder = new StringBuilder();
            int columnCount = grid.Columns.Count;

            for (int i = 2; i < columnCount; i++)
            {
                if (!grid.Columns[i].Hidden)
                {
                    if (i < columnCount - 1)
                    {
                        strBuilder.Append(grid.Columns[i].Header.Text.Replace("<br>", "~") + "\t");
                    }
                    else
                    {
                        strBuilder.Append(grid.Columns[i].Header.Text.Replace("<br>", "~") + "\r\n");
                    }
                }

            }
            if (columnCount >= 1 && grid.Columns[columnCount - 1].Hidden)
            {
                strBuilder.Append("\r\n");
            }
            int rowCount = grid.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                GridRecord row = grid.Rows[i];
                if (row != null)//&& !row.Hidden)
                {
                    for (int j = 2; j < columnCount; j++)
                    {
                        if (!row.Items[j].Column.Hidden)
                        {
                            if (j < columnCount - 1)
                            {
                                strBuilder.Append(row.Items[j].Text + "\t");
                            }
                            else
                            {
                                strBuilder.Append(row.Items[j].Text + "\r\n");
                            }
                        }
                    }
                }
                if (grid.Columns[columnCount - 1].Hidden)
                {
                    strBuilder.Append("\r\n");
                }
            }

            return strBuilder.ToString();

        }

        #endregion
        #region {Enter} -> {Tab}
        /// <summary>
        /// �ڿؼ�������Enter��ʱ�������Զ�ת����һ�ؼ�
        /// </summary>
        /// <param name="webControls">�ؼ��б�</param>
        /// <param name="loop">�Ƿ�ѭ��</param>
        public void Enter2Tab(System.Web.UI.WebControls.WebControl[] webControls, bool loop)
        {
            if (webControls == null || webControls.Length <= 1)
                return;
            for (int i = 0; i < webControls.Length; i++)
            {
                if (i == webControls.Length - 1 && loop == false)
                    break;
                string strToId = string.Empty;
                if (i < webControls.Length - 1)
                {
                    strToId = webControls[i + 1].ID;
                }
                else
                {
                    strToId = webControls[0].ID;
                }
                string strScript = "if (event.keyCode==13){ document.getElementById('" + strToId + "').focus(); return false;} ";
                webControls[i].Attributes.Add("onkeydown", strScript);
            }
        }
        #endregion

    }
}
