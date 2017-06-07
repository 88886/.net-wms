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
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.TSModel;
using BenQGuru.eMES.Domain.TSModel;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.TSModel
{
    public partial class FErrorCauseGroupMP : BaseMPageNew
    {
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
		
        protected BenQGuru.eMES.TSModel.TSModelFacade _facade ;//= TSModelFacadeFactory.CreateTSModelFacade();
	
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
			if( !this.IsPostBack )
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
            this.gridHelper.AddColumn( "ErrorCauseGroup", "����ԭ����",	null);
            this.gridHelper.AddColumn( "ErrorCauseGroupDescription", "����ԭ��������",	null);            
            this.gridHelper.AddColumn( "MaintainUser", "ά����Ա",	null);
            this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
            this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);
            this.gridHelper.AddLinkColumn( "SelectErrorCause","����ԭ���б�",null);

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
           // this.gridWebGrid.Columns.FromKey("SelectErrorCause").Width = 100;
            this.gridHelper.AddDefaultColumn( true, true );
            
            this.gridHelper.ApplyLanguage( this.languageComponent1 );
        }
		
        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["ErrorCauseGroup"] = ((ErrorCauseGroup)obj).ErrorCauseGroupCode.ToString();
            row["ErrorCauseGroupDescription"] =((ErrorCauseGroup)obj).ErrorCauseGroupDescription.ToString();
            row["MaintainUser"] = ((ErrorCauseGroup)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString((obj as ErrorCauseGroup).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString((obj as ErrorCauseGroup).MaintainTime);
            return row;

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            return this._facade.QueryErrorCauseGroup( 
                FormatHelper.CleanString(FormatHelper.PKCapitalFormat(this.txtErrorCauseGroupQuery.Text)),
                inclusive, exclusive );
        }


        protected override int GetRowCount()
        {
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            return this._facade.QueryErrorCauseGroupCount( 
                FormatHelper.CleanString(this.txtErrorCauseGroupQuery.Text));
        }

        #endregion

		#region Button

		protected override void AddDomainObject(object domainObject)
		{		
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			this._facade.AddErrorCauseGroup( (ErrorCauseGroup)domainObject );
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			this._facade.DeleteErrorCauseGroup( (ErrorCauseGroup[])domainObjects.ToArray( typeof(ErrorCauseGroup) ) );
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			this._facade.UpdateErrorCauseGroup( (ErrorCauseGroup)domainObject );

		}

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
			if ( pageAction == PageActionType.Add )
			{
				this.txtErrorCauseGroupEdit.ReadOnly = false;
			}

			if ( pageAction == PageActionType.Update )
			{
				this.txtErrorCauseGroupEdit.ReadOnly = true;
			}
		}

		#endregion        

        #region Object <--> Page

        protected override object GetEditObject()
        {
//            this.ValidateInput();
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            ErrorCauseGroup ErrorCauseGroup = this._facade.CreateNewErrorCauseGroup();

            ErrorCauseGroup.ErrorCauseGroupCode = FormatHelper.CleanString(FormatHelper.PKCapitalFormat(this.txtErrorCauseGroupEdit.Text), 40);
            ErrorCauseGroup.ErrorCauseGroupDescription = FormatHelper.CleanString(this.txtErrorCauseGroupDescriptionEdit.Text, 100);
            ErrorCauseGroup.MaintainUser = this.GetUserCode();

            return ErrorCauseGroup;
        }


        protected override object GetEditObject(GridRecord row)
        {	
            if (_facade == null)
            {
                _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();
            }
            string strCode = string.Empty;
            object objCode = row.Items.FindItemByKey("ErrorCauseGroup").Value;
            if (objCode != null)
            {
                strCode = objCode.ToString();
            }
            object obj = _facade.GetErrorCauseGroup(strCode);
            if (obj != null)
            {
                return (ErrorCauseGroup)obj;
            }
            return null;

        }

        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.txtErrorCauseGroupEdit.Text	= "";
                this.txtErrorCauseGroupDescriptionEdit.Text	= "";

                return;
            }

            this.txtErrorCauseGroupEdit.Text	= ((ErrorCauseGroup)obj).ErrorCauseGroupCode.ToString();
            this.txtErrorCauseGroupDescriptionEdit.Text	= ((ErrorCauseGroup)obj).ErrorCauseGroupDescription.ToString();
        }

		
        protected override bool ValidateInput()
        {
			PageCheckManager manager = new PageCheckManager();

			manager.Add( new LengthCheck(this.lblErrorCauseGroupCodeEdit, this.txtErrorCauseGroupEdit, 40, true) );			
			manager.Add( new LengthCheck(this.lblErrorCauseGroupDescriptionEdit, this.txtErrorCauseGroupDescriptionEdit, 100, false) );			

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
				return false;
			}

			return true;
        }

        #endregion

        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            if (commandName == "SelectErrorCause")
            {
                this.Response.Redirect(this.MakeRedirectUrl("./FErrorCauseGroup2ErrorCauseSP.aspx", new string[] { "ErrorCauseGroup" }, new string[] { row.Items.FindItemByKey("ErrorCauseGroup").Value.ToString() }));
            }
            
        }


        #region Export 	
        protected override string[] FormatExportRecord( object obj )
        {
            return new string[]{
                                   ((ErrorCauseGroup)obj).ErrorCauseGroupCode.ToString(),
                                   ((ErrorCauseGroup)obj).ErrorCauseGroupDescription.ToString(),
                                   ((ErrorCauseGroup)obj).GetDisplayText("MaintainUser"),
                                   FormatHelper.ToDateString(((ErrorCauseGroup)obj).MaintainDate)
                                   
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"����ԭ����",
                                    "����ԭ��������",
                                    "ά���û�",
                                    "ά������"};
        }
        #endregion
    }


}