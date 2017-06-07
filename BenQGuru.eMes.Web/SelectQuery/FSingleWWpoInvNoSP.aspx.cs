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
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Web.UserControl;

namespace BenQGuru.eMES.Web.SelectQuery
{
    /// <summary>
    /// FSingleItemSP ��ժҪ˵����
    /// </summary>
    public partial class FSingleWWpoInvNoSP : BaseSingleSelectorPageNew
    {

        private BenQGuru.eMES.SelectQuery.SPFacade facade;//= FacadeFactory.CreateSPFacade() ;
     

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
            if (Request.QueryString["invCode"] != null)
            {
                txtYLInvNoQuery.Text = Request.QueryString["invCode"];
            }
        }

        #endregion

        #region WebGrid
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedCode", "�������ϱ���", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedPOLine", "SAP�����к�", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedSubLine", "SubLine", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedMCode", "���ϴ���", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedDQMCode", "�������ϱ���", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnSelectedMdes", "��������", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedQty", "����", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedUnit", "��λ", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedHWMCode", "��Ϊ����", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedCPDQMCode", "��Ʒ���ϱ���", null);
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedCPMDesc", "��Ʒ��������", null);
            this.gridUnSelectedHelper.AddDefaultColumn(true, false);
            //this.gridUnSelectedHelper.ApplyLanguage(this.languageComponent1);
            this.gridUnSelected.Columns["Selector_UnselectedCode"].Hidden = true;
            this.gridUnSelected.Columns["Selector_UnSelectedDesc"].Hidden = true;
        }


        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((MesWWPOExc)obj).DQMCode + "," + ((MesWWPOExc)obj).POLine.ToString();
            row["Selector_UnselectedPOLine"] = ((MesWWPOExc)obj).POLine;
            row["Selector_UnselectedSubLine"] = ((MesWWPOExc)obj).SubLine;
            row["Selector_UnselectedMCode"] = ((MesWWPOExc)obj).MCode;
            row["Selector_UnselectedDQMCode"] = ((MesWWPOExc)obj).DQMCode;
            row["Selector_UnSelectedMdes"] = ((MesWWPOExc)obj).MChLongDesc;
            row["Selector_UnselectedQty"] = ((MesWWPOExc)obj).Qty;
            row["Selector_UnselectedUnit"] = ((MesWWPOExc)obj).Unit;
            row["Selector_UnselectedHWMCode"] = ((MesWWPOExc)obj).HWMCode;
            row["Selector_UnselectedCPDQMCode"] = ((MesWWPOExc)obj).CPDQMCode;
            row["Selector_UnselectedCPMDesc"] = ((MesWWPOExc)obj).CPMDesc;
            return row;
        }


        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUnwWpoInvNo(
            FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtYLInvNoQuery.Text)),
            FormatHelper.CleanString(this.txtDQMoCodeQuery.Text),
            FormatHelper.CleanString(this.txtMaterialDescQuery.Text),
           new string[0],
            inclusive,
            exclusive);
        }


        protected override int GetUnSelectedRowCount()
        {
            if (facade == null) { facade = new FacadeFactory(base.DataProvider).CreateSPFacade(); }
            return this.facade.QueryUNWWpoInvNoCount(
        FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtYLInvNoQuery.Text)),
       FormatHelper.CleanString(this.txtDQMoCodeQuery.Text),
       FormatHelper.CleanString(this.txtMaterialDescQuery.Text),
 
        new string[0]);

        }


        #endregion

    }
}
