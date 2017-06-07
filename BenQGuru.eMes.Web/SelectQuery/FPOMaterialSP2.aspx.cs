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
using BenQGuru.eMES.Common ;
using BenQGuru.eMES.Web.Helper ;
using BenQGuru.eMES.Web.UserControl ;

namespace BenQGuru.eMES.Web.SelectQuery
{
	/// <summary>
	/// Selector ��ժҪ˵����
	/// </summary>
	public partial class FPOMaterialSP2 : BaseSelectorPageNew
	{

        private BenQGuru.eMES.SelectQuery.SPFacade facade ;//= FacadeFactory.CreateSPFacade() ;


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
			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}
        }

        #endregion

        #region WebGrid
        protected override DataRow GetSelectedGridRow(object obj)
        {
            DataRow row = DtSourceSelected.NewRow();
            row["Selector_SelectedCode"] = ((BenQGuru.eMES.Domain.MOModel.Material)obj).MCode;
            row["Selector_SelectedDesc"] = ((BenQGuru.eMES.Domain.MOModel.Material)obj).MchshortDesc;
            return row;
        }

        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((BenQGuru.eMES.Domain.MOModel.Material)obj).MCode;
            row["Selector_UnSelectedDesc"] = ((BenQGuru.eMES.Domain.MOModel.Material)obj).MchshortDesc;
            return row;
        }

        protected override object[] LoadSelectedDataSource(int inclusive, int exclusive)
        {
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}
            return this.facade.QuerySelectedPOMaterial(this.txtPOQuery.Text, this.GetSelectedCodes());
        }

        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}
            return this.facade.QueryUnSelectedPOMaterialByPO(this.txtPOQuery.Text, this.txtMaterialCodeQuery.Text, this.txtMaterialDescQuery.Text, this.GetSelectedCodes(), inclusive, exclusive);
        }


        protected override int GetUnSelectedRowCount()
        {
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}
            return this.facade.QueryUnSelectedPOMaterialByPOCount(this.txtPOQuery.Text, this.txtMaterialCodeQuery.Text, this.txtMaterialDescQuery.Text, this.GetSelectedCodes());
        }

        
        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            this.writerOutted = true ;
            base.Render (writer);
            //writer.Write("<script language=javascript>try{ResetSelectAllPosition('chbUnSelected','gridUnSelected');ResetSelectAllPosition('chbSelected','gridSelected');}catch(e){};</script>");
            //writer.Write("<script language=javascript>try{if(window.top.valueLoaded != true){document.getElementById('txtSelected').innerText = window.top.dialogArguments.Codes;if(window.top.dialogArguments.DataObject.DocumentSegment){document.getElementById('txtPOQuery').value=window.top.dialogArguments.DataObject.DocumentSegment;document.getElementById('txtPOQuery').readOnly=true;}document.getElementById('cmdInit').click();window.top.valueLoaded = true ;}}catch(e){};</script>");
        }

        protected override void cmdInit_ServerClick(object sender, System.EventArgs e)
        {
            this.gridSelectedHelper.RequestData();
            this.gridUnSelectedHelper.RequestData();
            SetSelectedCodes();
            txtPOQuery.ReadOnly = true;
        }
	}
}
