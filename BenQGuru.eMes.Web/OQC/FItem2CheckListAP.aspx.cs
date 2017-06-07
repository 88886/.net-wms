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

using Infragistics.WebUI.UltraWebGrid;

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.OQC;
using BenQGuru.eMES.OQC;
using BenQGuru.eMES.Common ;

namespace BenQGuru.eMES.Web.OQC
{
    /// <summary>
    /// FOperation2ResourceSP ��ժҪ˵����
    /// </summary>
    public partial class FItem2OQCCheckListAP : BaseAPage
    {
        protected System.Web.UI.WebControls.Label lblOperationSelectTitle;
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        private OQCFacade facade ;

        #region Stable
        protected void Page_Load(object sender, System.EventArgs e)
		{	
			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}

            if( this.GetRequestParam("ItemCode") == string.Empty)
            {
                ExceptionManager.Raise( this.GetType() , "$Error_RequestUrlParameter_Lost"); 
            }

            this.txtItemCodeQuery.Text = this.GetRequestParam("ItemCode");

        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        #endregion

        #region Not Stable
        protected override void InitWebGrid()
        {
            this.gridHelper.AddColumn( "CheckItemCode", "������Ŀ",	null);
            this.gridHelper.AddColumn( "Description", "����",	null);
            this.gridHelper.AddColumn( "MaintainUser", "ά����Ա",	null);
            this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
			this.gridWebGrid.Columns.FromKey("Description").Hidden = true;
            this.gridHelper.AddDefaultColumn( true, false );

            base.InitWebGrid();
        }

        protected override void AddDomainObject(ArrayList domainObject)
        {
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
            facade.AddItem2OQCCheckList( (Item2OQCCheckList[])domainObject.ToArray(typeof(Item2OQCCheckList)));
        }

        protected override int GetRowCount()
        {			
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
            return this.facade.GetUnselectedOQCCheckListByItemCodeCount( this.txtItemCodeQuery.Text.Trim(),this.txtCheckListCodeQuery.Text.Trim());
        }

        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
            return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
                new object[]{"false",
                                FormatHelper.PKCapitalFormat( ((OQCCheckList)obj).CheckItemCode.ToString() ),
                                ((OQCCheckList)obj).Description.ToString() ,
                                ((OQCCheckList)obj).MaintainUser.ToString(),
                                FormatHelper.ToDateString(((OQCCheckList)obj).MaintainDate),
                                FormatHelper.ToTimeString(((OQCCheckList)obj).MaintainTime)});
        }

        protected override object GetEditObject(UltraGridRow row)
        {
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
            Item2OQCCheckList relation = facade.CreateNewItem2OQCCheckList();
            relation.ItemCode = this.txtItemCodeQuery.Text.Trim();
            relation.CheckItemCode = row.Cells[1].Text;		
            relation.MaintainUser = this.GetUserCode();
            relation.OrganizationID = GlobalVariables.CurrentOrganizations.First().OrganizationID;

            return relation;
        }


        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {			
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
            return facade.GetUnselectedOQCCheckListByItemCode( 
                FormatHelper.PKCapitalFormat( this.txtItemCodeQuery.Text.Trim()),
                FormatHelper.PKCapitalFormat( this.txtCheckListCodeQuery.Text.Trim()),
                inclusive,exclusive);
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            //this.Response.Redirect("./FItem2CheckListSP.aspx?ItemCode=" + this.GetRequestParam("ItemCode"));
            this.Response.Redirect(this.MakeRedirectUrl("./FItem2CheckListSP.aspx", new string[]{"ItemCode"}, new string[]{this.GetRequestParam("ItemCode")}));
        }

        #endregion

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
		
    }
}
