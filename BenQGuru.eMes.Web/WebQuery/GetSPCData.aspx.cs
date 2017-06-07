#region System
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
#endregion

#region eMes
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Web.Helper;
#endregion

namespace BenQGuru.eMES.Web.WebQuery
{
    /// <summary>
    /// ��SPC�������ݴ��ظ�ActiveX�ؼ���
    /// </summary>
    public partial class GetSPCData : System.Web.UI.Page
    {
        private string _itemCode;
        private string _dateFrom;
        private string _dateTo;
        private ArrayList _resList;//�����ѯ�����а���Resource,����Դ�ڲ�ѯ����,������Դ�ڲ�ѯ���
        private ArrayList _dateList;
        private string _resourceCode;
        private string _testName;
        private string _groupSeq;
        private string _fromTime;
        private string _tableName;
        private int _seq;
        private Item2SPCTest _test;
        private bool _ifResource = false;//��ѯ�������Ƿ����Resource
        private string _testResult;

        private BenQGuru.eMES.WebQuery.QuerySPC _query = null; // new BenQGuru.eMES.WebQuery.QuerySPC();
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private BenQGuru.eMES.MOModel.ItemFacade _itemfacade = null; // new BenQGuru.eMES.MOModel.ItemFacade();
        private BenQGuru.eMES.Common.Domain.IDomainDataProvider _provider;
        private BenQGuru.eMES.MOModel.SPCFacade spcFacade = null;
        private BenQGuru.eMES.Domain.SPC.SPCItemSpec itemSpec = null;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                try
                {
                    _provider = BenQGuru.eMES.Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider();
                    _query = new BenQGuru.eMES.WebQuery.QuerySPC(this._provider);
                    _itemfacade = new ItemFacade(this._provider);

                    GetParam();

                    #region ���ػ������ϲ���
                    if (_fromTime == null || _fromTime == string.Empty)//��һ�β�ѯ,����ˢ��
                    {
                        /*��Ʒ��
                        Ʒ��
                        ����								(YYYY-MM-DD)
                        ��Դ�б� 							(�Զ��ŷָ��Դ������Ķ����ÿո����)
                        ������
                        USL,LSL,UCL,LCL,�Ƿ��Զ�����
                        */
                        ModelFacade modelfacade = new FacadeFactory(_provider).CreateModelFacade();
                        Model model = (Model)modelfacade.GetModelByItemCode(_itemCode);
                        this.Writeln(model != null ? model.ModelCode : "");

                        Item item = (Item)_itemfacade.GetItem(_itemCode, GlobalVariables.CurrentOrganizations.First().OrganizationID);
                        this.Writeln(item != null ? item.ItemName : "");

                        //this.Writeln(_dateFrom);
                        WriteDateList();

                        WriteResList();

                        this.Writeln(this._testName);

                        string strtest = string.Empty;

                        strtest = itemSpec.USL.ToString() + ","
                            + itemSpec.LSL.ToString() + ","
                            + itemSpec.UCL.ToString() + ","
                            + itemSpec.LCL.ToString() + ","
                            + itemSpec.AutoCL + ","
                            + itemSpec.LimitUpOnly + ","
                            + itemSpec.LimitLowOnly;

                        this.Writeln(strtest);
                    }
                    else //ˢ��
                    {
                        WriteDateList();
                        //��Դ�б�
                        WriteResList();
                    }
                    #endregion

                    #region �������ݲ���
                    //����								(���¼�¼�����ܺ�)
                    //HHMMSS��ֵ,����վ					(ʱ�����ֵ��û�пո�)

                    //��SQL Server�в�ѯ����
                    if (_query == null)
                    {
                        _query = new BenQGuru.eMES.WebQuery.QuerySPC(this._provider);
                    }



                    BenQGuru.eMES.SPCDataCenter.DataHandler dataHandler = new BenQGuru.eMES.SPCDataCenter.DataHandler(this._provider);
                    int iFromTime = 0;
                    if (this._fromTime != null && this._fromTime != string.Empty)
                        iFromTime = int.Parse(this._fromTime);
                    string[][] spcData = dataHandler.QuerySPCData(this._itemCode, this._testName, int.Parse(this._groupSeq), this._resourceCode, FormatHelper.TODateInt(this._dateFrom), FormatHelper.TODateInt(this._dateTo), _testResult, iFromTime);
                    if ((_fromTime == null || _fromTime == string.Empty) && spcData.Length < 10)
                    {
                        ExceptionManager.Raise(this.GetType(), "$SPC_SamplePoint_Too_Little");
                    }
                    // ��¼��
                    this.Writeln(spcData.Length);
                    for (int i = 0; i < spcData.Length; i++)
                    {
                        string strLine = string.Empty;
                        strLine = this._dateList.IndexOf(spcData[i][1]) + "," + FormatHelper.ToTimeString(int.Parse(spcData[i][2])).Replace(":", "") + spcData[i][3];
                        if (this._resList.Count > 1)
                            strLine += "," + this._resList.IndexOf(spcData[i][0]);
                        this.Writeln(strLine);
                    }

                    #endregion

                    Response.End();

                    if (!IsPostBack)
                    {
                        // ��ʼ��ҳ������
                        //this.InitPageLanguage(this.languageComponent1, false);
                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                }
                catch (System.Exception ex)
                {
                    Response.Clear();
                    string msg = MessageCenter.ParserMessage(ex.Message, this.languageComponent1);
                    if (ex.InnerException != null)
                        msg = msg + " " + ex.InnerException.Message;
                    msg = msg.Replace("\r", "");
                    msg = msg.Replace("\n", "");
                    Response.Write(msg);
                    try
                    {
                        Response.End();
                    }
                    catch (System.Threading.ThreadAbortException)
                    {
                    }
                }
            }
			finally //�ر����ݿ�����
            {
                if (_query != null && this._query.SPCBroker != null)
                    this._query.SPCBroker.CloseConnection();

                if (_provider != null)
                    ((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)_provider).PersistBroker.CloseConnection();
            }
        }

        private void Writeln(object obj)
        {
            Response.Write(obj == null ? "" : obj.ToString());
            Response.Write("\r\n");
        }

        private void WriteResList()
        {
            //����ѯ������û��Resourceʱ,Ҫ�Լ������
            if (_resList.Count == 0)
            {

                int iFromTime = 0;
                if (this._fromTime != null && this._fromTime != string.Empty)
                    iFromTime = int.Parse(this._fromTime);
                BenQGuru.eMES.SPCDataCenter.DataHandler dataHandler = new BenQGuru.eMES.SPCDataCenter.DataHandler(this._provider);
                string[] arrRes = dataHandler.QuerySPCDataResource(this._itemCode, this._testName, int.Parse(this._groupSeq), this._resourceCode, FormatHelper.TODateInt(this._dateFrom), FormatHelper.TODateInt(this._dateTo), _testResult, iFromTime);
                for (int i = 0; i < arrRes.Length; i++)
                    _resList.Add(arrRes[i]);
            }

            string[] arr = new string[_resList.Count];

            for (int i = 0; i < _resList.Count; i++)
                arr[i] = ((string)_resList[i]).Replace(",", " ");

            string res = string.Join(",", arr);
            this.Writeln(res);
        }

        private void WriteDateList()
        {
            this.Writeln(_dateFrom);
        }

        private string GetParamValueFromUrl(string paramName)
        {
            string url = Request.Url.ToString();
            int startindex = url.IndexOf(paramName) + paramName.Length + 1;
            int endindex = url.Length;
            for (int i = startindex; i < url.Length; i++)
            {
                if (url[i] == '&')
                {
                    endindex = i;
                    break;
                }
            }

            return url.Substring(startindex, endindex - startindex);

        }
        /// <summary>
        /// ��ͨ��URL���Ĳ����ֽ����
        /// </summary>
        private void GetParam()
        {
            _itemCode = Request.QueryString["itemcode"];

            //��������пո������´���һ��
            if (_itemCode.IndexOf(" ") > 0)
            {
                _itemCode = GetParamValueFromUrl("itemcode").Trim();
            }
            _itemCode = _itemCode.ToUpper();

            _dateFrom = Request.QueryString["datefrom"];
            _dateTo = Request.QueryString["dateto"];
            _dateList = new ArrayList();
            if (_dateTo == string.Empty || _dateFrom == _dateTo)
                _dateList.Add(FormatHelper.TODateInt(_dateFrom).ToString());
            else
            {
                DateTime dtFrom = DateTime.Parse(_dateFrom);
                DateTime dtTo = DateTime.Parse(_dateTo);
                DateTime dtTmp = dtFrom;
                string strDate = string.Empty;
                while (dtTmp <= dtTo)
                {
                    _dateList.Add(FormatHelper.TODateInt(dtTmp).ToString());
                    dtTmp = dtTmp.AddDays(1);
                }
            }
            string res = Request.QueryString["resource"];
            //��������пո������´���һ��
            if (res.IndexOf(" ") > 0)
            {
                res = this.GetParamValueFromUrl("resource").Trim();
            }
            _resourceCode = res;

            _resList = new ArrayList();
            if (res != null && res != String.Empty)
            {
                string[] res_arr = res.Split(',');

                for (int i = 0; i < res_arr.Length; i++)
                {
                    _resList.Add(res_arr[i]);
                }
            }
            if (_resList.Count > 0)
                _ifResource = true;

            _testName = Request.QueryString["testitem"];
            _groupSeq = Request.QueryString["condition"];
            if (_testName.IndexOf(" ") > 0)
            {
                _testName = this.GetParamValueFromUrl("testitem").Trim();
            }
            _fromTime = Request.QueryString["fromtime"];
            _testResult = Request.QueryString["testresult"];

            //�Ըùܿ���Ŀ�����Ƿ�ά���洢��Ϣ
            if (spcFacade == null)
                spcFacade = new BenQGuru.eMES.MOModel.SPCFacade(this._provider);

            object[] objSpcObjectStore = spcFacade.GetSPCObjectStore(this._testName, decimal.Parse(this._groupSeq));
            if (objSpcObjectStore == null)
            {
                ExceptionManager.Raise(this.GetType(), "$Error_SPC_No_ObjectStore", string.Empty);//�˲�Ʒ��SPC������û��ά��		
            }

            // ��ѯ���Թ��

            itemSpec = (BenQGuru.eMES.Domain.SPC.SPCItemSpec)spcFacade.GetSPCItemSpec(this._itemCode, decimal.Parse(this._groupSeq), this._testName);
            if (itemSpec == null)
                ExceptionManager.Raise(this.GetType(), "$Error_SPC_No_TestItem", string.Empty);//�˲�Ʒ��SPC������û��ά��		
            // Added end

        }

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
            // 
            // languageComponent1
            // 
            this.languageComponent1.Language = "CHS";
            this.languageComponent1.LanguagePackageDir = "\\\\..";
            this.languageComponent1.RuntimePage = null;
            this.languageComponent1.RuntimeUserControl = null;
            this.languageComponent1.UserControlName = "";

        }
        #endregion
    }
}
