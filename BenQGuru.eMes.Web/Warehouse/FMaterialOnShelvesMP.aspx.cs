using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;

using BenQGuru.eMES.Common;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BenQGuru.eMES.Material;

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Warehouse;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
using BenQGuru.eMES.SAPRFCService;
using BenQGuru.eMES.SAPRFCService.Domain;


namespace BenQGuru.eMES.Web.WarehouseWeb
{
    public partial class FMaterialOnShelvesMP : BaseMPageNew
    {

        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;
        private static string poStno = string.Empty;
        private static string poStline = string.Empty;

        private static decimal editQTY = 0;  //��¼�༭ǰ���༭�е�ԭ������
        private WarehouseFacade facade = null;
        private InventoryFacade _Invenfacade = null;
        private BenQGuru.eMES.Material.InventoryFacade inventoryFacade = null;
        private InventoryFacade _InventoryFacade = null;
        private WarehouseFacade _WarehouseFacade = null;
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


        }
        #endregion


        #region Init
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������

                this.InitPageLanguage(this.languageComponent1, false);
            }
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }
            this.txtPlanOnshelves.Text = facade.QueryPlanOnShelvesQTY(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdit.Text))).ToString();
            this.txtActOnshelves.Text = facade.QueryActOnShelvesQTY(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdit.Text))).ToString();
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
            this.gridHelper.AddColumn("ASN", "���ָ���", null);
            this.gridHelper.AddColumn("BigCartonNO", "����", null);
            this.gridHelper.AddColumn("BoxNo", "���", null);
            this.gridHelper.AddColumn("RecomLocation", "�Ƽ���λ", null);
            this.gridHelper.AddColumn("LocationNO", "��λ��", null);
            this.gridHelper.AddColumn("DQLotNO", "�������κ�", null);
            this.gridHelper.AddColumn("State", "״̬", null);
            this.gridHelper.AddColumn("InitialCheck", "������", null);
            //this.gridHelper.AddColumn("IQCCheckMode", "IQC���鷽ʽ", null);
            //this.gridHelper.AddColumn("IQCCheckResult", "IQC������", null);
            this.gridHelper.AddColumn("DQMaterialNO", "�������ϱ���", null);
            this.gridHelper.AddColumn("DQMaterialDesc", "������������", null);
            this.gridHelper.AddColumn("VendorMCODE", "��Ӧ�����ϱ���", null);
            this.gridHelper.AddColumn("VendorMCODEDesc", "��Ӧ����������", null);
            //this.gridHelper.AddColumn("MaterialDes", "��������", null);
            this.gridHelper.AddColumn("ASNQTY", "��������", null);
            this.gridHelper.AddColumn("ReceivedQTY", "�ѽ�������", null);
            this.gridHelper.AddColumn("IQCOKQTY", "IQC�ϸ�����", null);
            this.gridHelper.AddColumn("InstorageQTY", "���������", null);
            this.gridHelper.AddColumn("MUOM", "��λ", null);
            this.gridHelper.AddColumn("DateCode", "��������", null);
            this.gridHelper.AddColumn("VendorLotNo", "��Ӧ������", null);
            this.gridHelper.AddColumn("McontrolType", "���Ϲܿ�����", null);
            this.gridHelper.AddColumn("Memo", "��ע", null);
            this.gridHelper.AddColumn("stline", "line", null);

            this.gridWebGrid.Columns.FromKey("stline").Hidden = true;

            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            //this.gridHelper.RequestData();
        }

        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["ASN"] = ((Asndetailexp)obj).Stno;
            row["BigCartonNO"] = ((Asndetailexp)obj).Cartonbigseq;
            row["BoxNo"] = ((Asndetailexp)obj).Cartonno;
            row["RecomLocation"] = ((Asndetailexp)obj).ReLocationCode;
            row["LocationNO"] = ((Asndetailexp)obj).LocationCode;
            row["DQLotNO"] = ((Asndetailexp)obj).Lotno;
            row["State"] = ((Asndetailexp)obj).Status;          //״̬��Ҫ��һ��
            row["InitialCheck"] = ((Asndetailexp)obj).InitreceiveStatus;
            row["DQMaterialNO"] = ((Asndetailexp)obj).DqmCode;
            row["DQMaterialDesc"] = ((Asndetailexp)obj).MDesc;
            row["VendorMCODE"] = ((Asndetailexp)obj).VEndormCode;
            row["VendorMCODEDesc"] = ((Asndetailexp)obj).VEndormCodeDesc;
            //row["MaterialDes"] = ((Asndetailexp)obj).stno.ToString();
            row["ASNQTY"] = ((Asndetailexp)obj).Qty.ToString();
            row["ReceivedQTY"] = ((Asndetailexp)obj).ReceiveQty.ToString();
            row["IQCOKQTY"] = ((Asndetailexp)obj).QcpassQty.ToString();
            row["InstorageQTY"] = ((Asndetailexp)obj).ActQty.ToString();
            row["MUOM"] = ((Asndetailexp)obj).Unit;
            row["DateCode"] = FormatHelper.ToDateString(((Asndetailexp)obj).Production_Date);
            row["VendorLotNo"] = ((Asndetailexp)obj).Supplier_lotno;
            row["McontrolType"] = ((Asndetailexp)obj).MControlType;
            row["Memo"] = ((Asndetailexp)obj).Remark1;
            row["stline"] = ((Asndetailexp)obj).Stline.ToString();
            return row;
        }


        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }



            object[] objs = this.facade.QueryOnshelvesDetail(
               FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdit.Text)),
               FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtLocationNO.Text))
              );

            //List<string> stnos = new List<string>();
            //if (objs != null && objs.Length > 0)
            //{
            //    foreach (Asndetailexp o in objs)
            //    {
            //        if (!stnos.Contains(o.Stno))
            //            stnos.Add(o.Stno);
            //    }
            //}

            //BenQGuru.eMES.Domain.IQC.AsnIQC[] iqcs = facade.GetASNIQCFromASN(stnos);
            //List<string> IQCNos = new List<string>();
            //foreach (BenQGuru.eMES.Domain.IQC.AsnIQC iqc in iqcs)
            //{
            //    if (iqc.IqcType == "SpotCheck")
            //    {
            //        IQCNos.Add(iqc.IqcNo);
            //    }
            //}
            //BenQGuru.eMES.Domain.IQC.AsnIQCDetailEc[] ECs = facade.GetIQCECFromIQCNo(IQCNos);
            //List<object> passObjs = new List<object>();
            //if (objs != null && objs.Length > 0)
            //{
            //    foreach (Asndetailexp o in objs)
            //    {
            //        bool isOk = true;
            //        foreach (BenQGuru.eMES.Domain.IQC.AsnIQCDetailEc ec in ECs)
            //        {
            //            if (ec.StNo == o.Stno)
            //                isOk = false;
            //        }
            //        if (isOk)
            //            passObjs.Add(o);
            //    }
            //}
            //return passObjs.ToArray();
            return objs;

        }

        protected override int GetRowCount()
        {
            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }



            object[] objs = this.facade.QueryOnshelvesDetail(
               FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdit.Text)),
               FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtLocationNO.Text))
              );

            List<string> stnos = new List<string>();
            if (objs != null && objs.Length > 0)
            {
                foreach (Asndetailexp o in objs)
                {
                    if (!stnos.Contains(o.Stno))
                        stnos.Add(o.Stno);
                }
            }

            BenQGuru.eMES.Domain.IQC.AsnIQC[] iqcs = facade.GetASNIQCFromASN(stnos);
            List<string> IQCNos = new List<string>();
            foreach (BenQGuru.eMES.Domain.IQC.AsnIQC iqc in iqcs)
            {
                if (iqc.IqcType == "SpotCheck")
                {
                    IQCNos.Add(iqc.IqcNo);
                }
            }
            BenQGuru.eMES.Domain.IQC.AsnIQCDetailEc[] ECs = facade.GetIQCECFromIQCNo(IQCNos);
            List<object> passObjs = new List<object>();
            if (objs != null && objs.Length > 0)
            {
                foreach (Asndetailexp o in objs)
                {
                    bool isOk = true;
                    foreach (BenQGuru.eMES.Domain.IQC.AsnIQCDetailEc ec in ECs)
                    {
                        if (ec.StNo == o.Stno)
                            isOk = false;
                    }
                    if (isOk)
                        passObjs.Add(o);
                }
            }
            return passObjs.Count;

        }

        #endregion

   

        protected override void cmdSave_Click(object sender, EventArgs e)
        {
            ArrayList array = this.gridHelper.GetCheckedRows();

  
            List<Asndetail> asnDetailList = new List<Asndetail>();
            if (array.Count == 0)
            {
                WebInfoPublish.Publish(this, "������ѡ��һ������", this.languageComponent1);
                return;
            }
            foreach (GridRecord row in array)
            {
                object obj = null;
                obj = this.GetEditObject(row);
                if (obj == null)
                    throw new Exception("���л�ȡ��ASN��ϸΪ�գ�");
                Asndetail asndetail = obj as Asndetail;

                asnDetailList.Add(asndetail);
            }
            string message = string.Empty;
            ShareLib.ShareKit kit = new ShareLib.ShareKit();


            if (kit.OnShelf(txtCartonNoEdit.Text, txtLocationNO.Text, asnDetailList, base.DataProvider, out message, GetUserCode()))
            {
                this.txtPlanOnshelves.Text = facade.QueryPlanOnShelvesQTY(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdit.Text))).ToString();
                this.txtActOnshelves.Text = facade.QueryActOnShelvesQTY(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtCartonNoEdit.Text))).ToString();
                WebInfoPublish.Publish(this, "�ϼܳɹ���", this.languageComponent1);

            }
            else
            {
                WebInfoPublish.Publish(this, "�ϼ�ʧ�ܣ�" + message, this.languageComponent1);
            }
            
        }

       

      
        

        protected void cmdCheck_ServerClick(object sender, EventArgs e)
        {


            if (_InventoryFacade == null)
            {
                _InventoryFacade = new InventoryFacade(base.DataProvider);
            }
            if (this.gridWebGrid.Rows.Count == 0)
            {
                //��ʾ����
                WebInfoPublish.PublishInfo(this, "Grid��û������", this.languageComponent1);
                return;
            }
            List<AsnHead> obj = _InventoryFacade.QueryASNDetailSNCatron(this.txtCartonnoSN.Text.Trim());


            string stNO = "";
            string stLine = "";

            if (obj != null)
            {
                for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
                {
                    this.gridWebGrid.Rows[i].Items.FindItemByKey("Check").Value = false;
                }
                foreach (AsnHead o in obj)
                {

                    for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
                    {

                        stNO = this.gridWebGrid.Rows[i].Items.FindItemByKey("ASN").Text;
                        stLine = this.gridWebGrid.Rows[i].Items.FindItemByKey("stline").Text;

                        if ((o.STNO == stNO && (o.STlINE == stLine)))
                        {
                            this.gridWebGrid.Rows[i].Items.FindItemByKey("Check").Value = true;

                        }
                    }
                }


            }
            else
            {
                //��ʾû�ҵ������Ϣ
                WebInfoPublish.PublishInfo(this, "û��ƥ�����ָ�����Ϣ", this.languageComponent1);
                return;
            }

        }


    
    

       




        private void ShowMessage(string msg)
        {
            WebInfoPublish.Publish(this, msg, this.languageComponent1);
        }
        #region Object <--> Page

        protected override object GetEditObject(GridRecord row)
        {

            if (facade == null)
            {
                facade = new WarehouseFacade(base.DataProvider);
            }

            object obj = facade.GetAsndetail(int.Parse(row.Items.FindItemByKey("stline").Value.ToString()), row.Items.FindItemByKey("ASN").Value.ToString());

            if (obj != null)
            {
                return (Asndetail)obj;
            }

            return null;
        }



        #endregion


    }

   
}
