using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Domain.IQC;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.IQC;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
//using BenQGuru.eMES.CodeSoftPrint;
using UserControl;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FTDCartonPrint : BaseMPageNew
    {
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;


        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private WarehouseFacade _WarehouseFacade = null;
        private WarehouseFacade _TransferFacade;
        SystemSettingFacade _SystemSettingFacade = null;
        private InventoryFacade facade = null;
        private UserFacade _UserFacade = null;
        bool isVendor = false;//�жϵ�ǰ�û��Ƿ�Ϊ��Ӧ��
       //     CodeSoftPrintFacade _codeSoftPrintFacade = null;
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
            this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
            this.languageComponent1.RuntimePage = null;
            this.languageComponent1.RuntimeUserControl = null;
            this.languageComponent1.UserControlName = "";
            UnLoadPageLoad();
        }
        #endregion

        #region Init

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (_UserFacade == null)
            {
                _UserFacade = new UserFacade(this.DataProvider);
            }
            isVendor = _UserFacade.IsVendor(this.GetUserCode());
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
            }
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }


      
        #endregion

        #region WebGrid

        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("TDCartonNo", "�������", null);
            this.gridHelper.AddDefaultColumn(true, false);
            
            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            //row["Check"] = "Checked";
            row["TDCartonNo"] = ((BarCode)obj).BarCodeNo;
            return row;
        }

        private string MoldidToSQL(string moldid)
        {
            if (moldid.Equals(string.Empty))
            {
                return "";
            }
            string sql = "";
            string[] str = moldid.Split(',');
            foreach (string s in str)
            {
                if (!s.Equals(string.Empty))
                {
                    sql += "'" + s + "',";
                }
            }

            return sql.Remove(sql.LastIndexOf(','));
        }
        protected virtual void cmdQuery_Click(object sender, System.EventArgs e)
        {
            this.gridHelper.RequestNewData();
            //if (this.gridHelper2 != null)
            //    this.gridHelper2.RequestNewData();
            this.buttonHelper.PageActionStatusHandle(PageActionType.Query);
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }

            return this._WarehouseFacade.QueryBarCode(
           FormatHelper.CleanString(this.txtCartonNoQurey.Text),
          this.txtBarCodeListQuery.Text,
           inclusive, exclusive);
        }

        protected override int GetRowCount()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            return this._WarehouseFacade.QueryBarCodeCount(
                     FormatHelper.CleanString(this.txtCartonNoQurey.Text),
                    this.txtBarCodeListQuery.Text
                  );
        }

        #endregion

     

        #region Export

        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{
                              ((BarCode)obj).BarCodeNo
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[]
                {
                                    "TDCartonNo"
                };
        }

        #endregion


        #region Button
        //����������밴ť
        protected void cmdCreateBarCode_ServerClick(object sender, System.EventArgs e)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            //����������밴ť�������������������������������������ʾ��Grid�в�������TBLBARCODE����
            //4.	������ű������CT+������+��λ��ˮ�룺�磺CT20160131000000001����ˮ�벻����(��ˮ�봴����Ӧ��Sequences�ۼ�)
            if (string.IsNullOrEmpty(txtQtyEdit.Text.Trim()))
            {
                WebInfoPublish.Publish(this, "��������Ϊ��", this.languageComponent1);
                return;
            }
            int qty= int.Parse(txtQtyEdit.Text.Trim());
            if (qty == 0)
            {
                WebInfoPublish.Publish(this, "����ֻ���������0������", this.languageComponent1);
                return;
            }
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            int date=dbDateTime.DBDate;
            string barCodeList = "";
            try
            {
                this.DataProvider.BeginTransaction();
                for (int i = 0; i <qty; i++)
                {
                    BarCode bar=new BarCode();
                    string serialNo = CreateSerialNo(date);
                    bar.BarCodeNo = "CT" + date + serialNo;
                    bar.Type = "CARTONNO";
                    bar.MCode = "";
                    bar.EnCode = "";
                    bar.SpanYear =date.ToString().Substring(0,4);
                    bar.SpanDate = date;
                    if (!string.IsNullOrEmpty(serialNo))
                    {
                        bar.SerialNo = int.Parse(serialNo);
                    }
                    bar.PrintTimes = 0;
                    bar.CUser = this.GetUserCode();	//	CUSER
                    bar.CDate = dbDateTime.DBDate;	//	CDATE
                    bar.CTime = dbDateTime.DBTime;//	CTIME
                    bar.MaintainDate = dbDateTime.DBDate;	//	MDATE
                    bar.MaintainTime = dbDateTime.DBTime;	//	MTIME
                    bar.MaintainUser = this.GetUserCode();		//	MUSER
                    _WarehouseFacade.AddBarCode(bar);
                    barCodeList += "'" + bar.BarCodeNo + "',";
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.PublishInfo(this, ex.Message, this.languageComponent1);
            }
            if (!string.IsNullOrEmpty(barCodeList))
            {
                if (string.IsNullOrEmpty(txtBarCodeListQuery.Text))
                {
                    txtBarCodeListQuery.Text = barCodeList.Substring(0, barCodeList.Length - 1);
                }
                else
                {
                    txtBarCodeListQuery.Text += ",";
                    txtBarCodeListQuery.Text += barCodeList.Substring(0, barCodeList.Length - 1);
                }
            }
            cmdQuery_Click(null,null);
        }

        //��ӡ��ť
        protected void cmdPrint_ServerClick(object sender, System.EventArgs e)
        {
            //GetServerClick("Print");
            Print();
            txtBarCodeListQuery.Text = "";
        }

        #endregion

        private void Print()
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            if (this.gridWebGrid.Rows.Count <= 0)
            {
                return;
            }
            try
            {
                this.DataProvider.BeginTransaction();

                for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
                {
                    string barno = this.gridWebGrid.Rows[i].Items.FindItemByKey("TDCartonNo").Value.ToString();
                    BarCode bar = (BarCode)_WarehouseFacade.GetBarCode(barno);
                    bar.PrintTimes = bar.PrintTimes + 1;
                    _WarehouseFacade.UpdateBarCode(bar);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.PublishInfo(this, ex.Message, this.languageComponent1);
            }
        }



        #region GetServerClick
        private void GetServerClick(string clickName)
        {
            ArrayList array = this.gridHelper.GetCheckedRows();
            object obj = null;
         
            if (array.Count > 0)
            {
                ArrayList objList = new ArrayList(array.Count);

                foreach (GridRecord row in array)
                {
                    obj = this.GetEditObject(row);

                    if (obj != null)
                    {
                        objList.Add(obj);
                    }
                }
                object[] asnList = ((BarCode[])objList.ToArray(typeof(BarCode)));
                if (clickName == "Print")
                {
                    this.InitialObjects(asnList);
                }
                this.gridHelper.RequestData();
                this.buttonHelper.PageActionStatusHandle(PageActionType.Add);
            }
        }

      


        #endregion

        protected void InitialObjects(object[] barList)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }

            try
            {
                this.DataProvider.BeginTransaction();
                foreach (BarCode bar in barList)
                {
                    bar.PrintTimes = bar.PrintTimes + 1;
                    _WarehouseFacade.UpdateBarCode(bar);
                }
               // Page.RegisterClientScriptBlock("Print", "<script>Print()</script>");
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                WebInfoPublish.PublishInfo(this, ex.Message, this.languageComponent1);
            }
        }



        protected override object GetEditObject(GridRecord row)
        {
            if (_WarehouseFacade == null)
            {
                _WarehouseFacade = new WarehouseFacade(base.DataProvider);
            }
            object obj = _WarehouseFacade.GetBarCode(row.Items.FindItemByKey("TDCartonNo").Text);

            if (obj != null)
            {
                return obj;
            }

            return null;
        }

        #region CreateSerialNo
        private string CreateSerialNo(int stno)
        {
            DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(this.DataProvider);
            if (_TransferFacade == null)
            {
                _TransferFacade = new WarehouseFacade(this.DataProvider);
            }
            string maxserial = _TransferFacade.GetMaxSerial("CT"+ stno);

            //����������ֵ�ͷ���Ϊ��
            if (maxserial == "999999999")
            {
                return "";
            }

            SERIALBOOK serialbook = new SERIALBOOK();
            if (maxserial == "")
            {
                serialbook.SNPrefix = "CT" + stno;
                serialbook.MaxSerial = "1";
                serialbook.MDate = dbDateTime.DBDate;
                serialbook.MTime = dbDateTime.DBTime;
                serialbook.MUser = this.GetUserCode();
                _TransferFacade.AddSerialBook(serialbook);
                return string.Format("{0:000000000}", int.Parse(serialbook.MaxSerial));
            }
            else
            {
                serialbook.SNPrefix = "CT" + stno;
                serialbook.MaxSerial = (int.Parse(maxserial) + 1).ToString();
                serialbook.MDate = dbDateTime.DBDate;
                serialbook.MTime = dbDateTime.DBTime;
                serialbook.MUser = this.GetUserCode();
                _TransferFacade.UpdateSerialBook(serialbook);
                return string.Format("{0:000000000}", int.Parse(serialbook.MaxSerial));
            }
        }

        #endregion

        #region ��ӡ
        //private void PrintRcardList()
        //{
        //    #region ������1����ӡ��
        //    //������1����ӡ��
        //    string printer = ucLabelComPrint.SelectedItemValue.ToString();
        //    #endregion

        //    #region ������2����ӡģ��·��
        //    //ģ������
        //    string templateName = ucLabelComPrintTemplate.SelectedItemText;

        //    //��ӡģ�����
        //    PrintTemplate template = _printTemplateFacade.GetPrintTemplate(templateName) as PrintTemplate;

        //    //������2����ӡģ��·��
        //    string printTemplatePath = string.Empty;
        //    if (template != null)
        //    {
        //        printTemplatePath = template.TemplatePath;
        //    }
        //    #endregion

        //    #region ������3��Ҫ��ӡ��ֵ
        //    //������3��Ҫ��ӡ��ֵ
        //    List<StringDictionary> printValues = GetStringDictionaryList();
        //    #endregion

        //    if (_codeSoftPrintFacade == null)
        //        _codeSoftPrintFacade = new CodeSoftPrintFacade(DataProvider);

        //    //��ȡ��ӡִ�к�Ľ��
        //    Messages msg = _codeSoftPrintFacade.Print(printer, printTemplatePath, printValues);

        //    _codeSoftPrintFacade = null;

        //    /**************************************************************
        //     * TODO:��ӡ���
        //     * ����TBLCARTONINFO��Ĵ�ӡ��������ӡ�˺�����ʱ��
        //     **************************************************************/
        //    string cartonNo = ucLabelPalletNo.Value.Trim();//ջ���
        //    string userCode = ApplicationService.Current().UserCode;
        //    DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);

        //    if (msg.IsSuccess())
        //    {
        //        try
        //        {
        //            #region ����CARTONINFO��
        //            //CARTONINFO cartonInfo = _packageFacade.GetCARTONINFO(cartonNo) as CARTONINFO;
        //            //if (cartonInfo != null)
        //            //{
        //            //    //��ӡ������1
        //            //    cartonInfo.PrintCount = cartonInfo.PrintCount + 1;
        //            //    cartonInfo.MUSER = userCode;
        //            //    cartonInfo.MDATE = dbDateTime.DBDate;
        //            //    cartonInfo.MTIME = dbDateTime.DBTime;
        //            //}

        //            //����
        //           // _packageFacade.UpdateCARTONINFO(cartonInfo);
        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //          //  ShowMessage(new UserControl.Message(MessageType.Error, ex.Message));
        //            return;
        //        }
        //    }

        // //   ApplicationRun.GetInfoForm().Add(msg);
        //}

        #endregion


     
    }
}
