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
    public partial class FErrorCauseMP : BaseMPageNew
    {
        
        private System.ComponentModel.IContainer components;
		
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		
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

        protected BenQGuru.eMES.TSModel.TSModelFacade _facade ;//= TSModelFacadeFactory.CreateTSModelFacade();
		
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
            this.gridHelper.AddColumn( "ErrorCauseCode", "����ԭ�����",	null);
            this.gridHelper.AddColumn( "ErrorCauseDescription", "����ԭ������",	null);
            this.gridHelper.AddColumn( "MaintainUser", "ά����Ա",	null);
            this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
            this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
            this.gridHelper.AddDefaultColumn( true, true );
            
            this.gridHelper.ApplyLanguage( this.languageComponent1 );
        }
		
        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["ErrorCauseCode"] = ((ErrorCause)obj).ErrorCauseCode.ToString();
            row["ErrorCauseDescription"] = ((ErrorCause)obj).ErrorCauseDescription.ToString();
            row["MaintainUser"] = ((ErrorCause)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString((obj as ErrorCause).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString((obj as ErrorCause).MaintainTime);
            return row;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            return this._facade.QueryErrorCause( 
                FormatHelper.CleanString(FormatHelper.PKCapitalFormat(this.txtErrorCauseCodeQuery.Text)),
                inclusive, exclusive );
        }


        protected override int GetRowCount()
        {
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            return this._facade.QueryErrorCauseCount( 
                FormatHelper.CleanString(FormatHelper.PKCapitalFormat(this.txtErrorCauseCodeQuery.Text)));
        }

        #endregion

		#region Button

		protected override void AddDomainObject(object domainObject)
		{		
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			this._facade.AddErrorCause( (ErrorCause)domainObject );
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			this._facade.DeleteErrorCause( (ErrorCause[])domainObjects.ToArray( typeof(ErrorCause) ) );
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			 this._facade.UpdateErrorCause( (ErrorCause)domainObject );
		}

        protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
        {
            if ( pageAction == PageActionType.Add )
            {
                this.txtErrorCauseCodeEdit.ReadOnly = false;
            }

            if ( pageAction == PageActionType.Update )
            {
                this.txtErrorCauseCodeEdit.ReadOnly = true;
            }
        }
		#endregion

        #region Object <--> Page

        protected override object GetEditObject()
        {
//            this.ValidateInput();
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            ErrorCause errorCause = this._facade.CreateNewErrorCause();

            errorCause.ErrorCauseCode = FormatHelper.CleanString(FormatHelper.PKCapitalFormat(this.txtErrorCauseCodeEdit.Text), 40);
            errorCause.ErrorCauseDescription = FormatHelper.CleanString(this.txtErrorCauseDescriptionEdit.Text, 100);
            errorCause.MaintainUser = this.GetUserCode();

            return errorCause;
        }


        protected override object GetEditObject(GridRecord row)
        {	
            if (_facade == null)
            {
                _facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();
            }
            string strCode = string.Empty;
            object objCode = row.Items.FindItemByKey("ErrorCauseCode").Value;
            if (objCode != null)
            {
                strCode = objCode.ToString();
            }
            object obj = _facade.GetErrorCause(strCode);
            if (obj != null)
            {
                return (ErrorCause)obj;
            }
            return null;

        }

        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.txtErrorCauseCodeEdit.Text	= "";
                this.txtErrorCauseDescriptionEdit.Text	= "";

                return;
            }

            this.txtErrorCauseCodeEdit.Text	= ((ErrorCause)obj).ErrorCauseCode.ToString();
            this.txtErrorCauseDescriptionEdit.Text	= ((ErrorCause)obj).ErrorCauseDescription.ToString();
        }

        protected override bool ValidateInput()
        {
			PageCheckManager manager = new PageCheckManager();

			manager.Add( new LengthCheck(this.lblErrorCauseCodeEdit, this.txtErrorCauseCodeEdit, 40, true) );			
			manager.Add( new LengthCheck(this.lblErrorCauseDescriptionEdit, this.txtErrorCauseDescriptionEdit, 100, false) );			

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
				return false;
			}

			return true;
		}

        #endregion
        
        #region Export 	

        protected override string[] FormatExportRecord( object obj )
        {
            return new string[]{
                                   ((ErrorCause)obj).ErrorCauseCode.ToString(),
                                   ((ErrorCause)obj).ErrorCauseDescription.ToString(),
                                   ((ErrorCause)obj).GetDisplayText("MaintainUser"),
                                   FormatHelper.ToDateString(((ErrorCause)obj).MaintainDate)
                               }
                ;
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"����ԭ�����",
                                    "����ԭ������",
                                    "ά���û�",
                                    "ά������" };
        }
        #endregion
    }


}