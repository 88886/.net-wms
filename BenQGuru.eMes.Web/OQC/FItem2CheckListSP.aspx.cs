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
    public partial class FItem2CheckListSP : BaseMPage
    {
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        protected System.Web.UI.WebControls.Label lblErrorGroupCode;

        private OQCFacade facade ;//= new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;

        #region Stable
        protected void Page_Load(object sender, System.EventArgs e)
        {
			//this.pagerSizeSelector.Readonly = true;

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				if( this.GetRequestParam("ItemCode") == string.Empty)
				{
					ExceptionManager.Raise( this.GetType() , "$Error_RequestUrlParameter_Lost"); 
				}

				this.txtItemCodeQuery.Text = this.GetRequestParam("ItemCode");
				
			}
        }

        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        #endregion

        #region Not Stable
        protected override void InitWebGrid()
        {
            this.gridHelper.AddColumn( "Sequence1", "���",	null);
            this.gridHelper.AddColumn( "CheckListCode", "������Ŀ",	null);
			this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

            this.gridHelper.AddDefaultColumn( true, true );
            base.InitWebGrid();

			this.gridHelper.RequestData();
        }

        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
            facade.DeleteItem2OQCCheckList( (Item2OQCCheckList[])domainObjects.ToArray(typeof(Item2OQCCheckList)));
        }

        protected override object GetEditObject(UltraGridRow row)
        {
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
            Item2OQCCheckList relation = (Item2OQCCheckList)facade.GetItem2OQCCheckList(this.txtItemCodeQuery.Text , row.Cells[2].Text, GlobalVariables.CurrentOrganizations.First().OrganizationID);
            return relation;
        }

        protected override int GetRowCount()
        {			
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
            return this.facade.QueryItem2OQCCheckListCount( this.txtItemCodeQuery.Text.Trim(),this.txtCheckListCodeQuery.Text.Trim());
        }

		#region Export
		// 2005-04-06
		protected override string[] FormatExportRecord( object obj )
		{
			return new string[]{
				((Item2OQCCheckList)obj).Sequence.ToString(),
                ((Item2OQCCheckList)obj).CheckItemCode.ToString(),
				((Item2OQCCheckList)obj).MaintainUser,
				FormatHelper.ToDateString(((Item2OQCCheckList)obj).MaintainDate)};
		}


		protected override string[] GetColumnHeaderText()
		{
			return new string[]
				{
					"Sequence",
					"CheckListCode",
					"MaintainUser",
					"MaintainDate"};
		}

		#endregion

        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
            return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
                new object[]{"false",
                                ((Item2OQCCheckList)obj).Sequence.ToString(),
                                ((Item2OQCCheckList)obj).CheckItemCode.ToString(),
								((Item2OQCCheckList)obj).MaintainUser,
								FormatHelper.ToDateString( ((Item2OQCCheckList)obj).MaintainDate ) 
                            });
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {			
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
            object[] returnObjs =  this.facade.QueryItem2OQCCheckList(
                FormatHelper.PKCapitalFormat(this.txtItemCodeQuery.Text.Trim()) ,
                FormatHelper.PKCapitalFormat(this.txtCheckListCodeQuery.Text.Trim()),
                inclusive,Int32.MaxValue);

			this.lblCount.Text = "�� 0 ��";
			if(returnObjs != null)
			this.lblCount.Text = string.Format("�� {0} ��",returnObjs.Length.ToString());

			return returnObjs;
        }

		protected void cmdSelect_ServerClick(object sender, System.EventArgs e)
        {
            //this.Response.Redirect("./FItem2CheckListAP.aspx?ItemCode=" + FormatHelper.CleanString( this.txtItemCodeQuery.Text ));
            this.Response.Redirect(this.MakeRedirectUrl("./FItem2CheckListAP.aspx", new string[]{"ItemCode"}, new string[]{FormatHelper.CleanString(this.txtItemCodeQuery.Text)}));
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect("../MOModel/FItemMP.aspx");
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

        protected override object GetEditObject()
        {
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
            Item2OQCCheckList obj =  (Item2OQCCheckList)this.facade.GetItem2OQCCheckList( this.txtItemCodeQuery.Text , this.txtCheckListCodeEdit.Text, GlobalVariables.CurrentOrganizations.First().OrganizationID) ;
            obj.Sequence = int.Parse(this.txtSequenceEdit.Text);
			obj.MaintainUser = FormatHelper.CleanString(this.GetUserCode());
            return obj ;
        }

		protected override void UpdateDomainObject(object domainObject)
		{
			if(facade==null){facade = new OQCFacadeFactory(base.DataProvider).CreateOQCFacade() ;}
			this.facade.UpdateItem2OQCCheckList( (Item2OQCCheckList)this.GetEditObject() );
		}

        protected override bool ValidateInput()
        {

            PageCheckManager manager = new PageCheckManager();

            manager.Add( new NumberCheck(lblSequenceEdit, txtSequenceEdit,true) );

            if ( !manager.Check() )
            {
                WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
                return false;
            }

            return true ;

        }

        protected override void SetEditObject(object obj)
        {
			if (obj == null)
			{
				this.txtSequenceEdit.Text = "";
				this.txtCheckListCodeEdit.Text = "" ;       
        
				return;
			}

			this.txtSequenceEdit.Text = ((Item2OQCCheckList)obj).Sequence.ToString() ;
			this.txtCheckListCodeEdit.Text = ((Item2OQCCheckList)obj).CheckItemCode.ToString() ;       
        }
		
    }
}
