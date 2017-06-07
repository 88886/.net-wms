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
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.TSModel;

namespace BenQGuru.eMES.Web.TSModel
{
	/// <summary>
	/// FModel2ErrorCauseSP ��ժҪ˵����
	/// </summary>
	public partial class FModel2ErrorCauseSP : BaseSPage
	{
        protected System.Web.UI.WebControls.Label lblErrorErrorCauseCodeQuery;
    
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

        private TSModelFacade _facade ;//= TSModelFacadeFactory.CreateTSModelFacade();

        #region Stable
        protected void Page_Load(object sender, System.EventArgs e)
        {		
			//this.pagerSizeSelector.Readonly = true;

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.txtModelCodeQuery.Text = this.GetRequestParam("Modelcode");            				
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
            this.gridHelper.AddColumn( "ErrorCause", "����ԭ�����",	null);
            this.gridHelper.AddColumn( "ErrorCauseDescription", "����ԭ������",	null);
            this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
            this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
            this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);		

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
			this.gridHelper.AddDefaultColumn( true, false );

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

			base.InitWebGrid();

			this.gridHelper.RequestData();
        }

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			_facade.DeleteModel2ErrorCause( (Model2ErrorCause[])domainObjects.ToArray(typeof(Model2ErrorCause)));
		}

        protected override int GetRowCount()
        {			
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            return this._facade.GetSelectedErrorCauseByModelCodeCount( 
				FormatHelper.PKCapitalFormat( this.txtModelCodeQuery.Text.Trim() ) , 
				FormatHelper.PKCapitalFormat( this.txtErrorCauseCodeQuery.Text.Trim() ));
        }

        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
            return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
                new object[]{"false",
                                ((ErrorCause)obj).ErrorCauseCode.ToString(),
                                ((ErrorCause)obj).ErrorCauseDescription.ToString(),
                                ((ErrorCause)obj).MaintainUser.ToString(),
                                FormatHelper.ToDateString(((ErrorCause)obj).MaintainDate),
                                FormatHelper.ToTimeString(((ErrorCause)obj).MaintainTime)
                            });
        }

		protected override string[] FormatExportRecord( object obj )
		{
			return new string[]{((ErrorCause)obj).ErrorCauseCode.ToString(),
								   ((ErrorCause)obj).ErrorCauseDescription.ToString(),
								   ((ErrorCause)obj).MaintainUser.ToString(),
								   FormatHelper.ToDateString(((ErrorCause)obj).MaintainDate)};
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	"ErrorCause",
									"ErrorCauseDescription",
									"MaintainUser",
									"MaintainDate"};
		}

		protected override object GetEditObject(UltraGridRow row)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			Model2ErrorCause relation = _facade.CreateNewModel2ErrorCause();
			relation.ModelCode = this.txtModelCodeQuery.Text.Trim();
			relation.ErrorCauseCode = row.Cells[1].Text;	
			relation.MaintainUser = this.GetUserCode();

			return relation;
		}

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {			
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            return this._facade.GetSelectedErrorCauseByModelCode( 
				FormatHelper.PKCapitalFormat( this.txtModelCodeQuery.Text.Trim() ) , 
				FormatHelper.PKCapitalFormat( this.txtErrorCauseCodeQuery.Text.Trim() ),
				inclusive,exclusive);
        }

        protected void cmdSelect_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect("./FModel2ErrorCauseAP.aspx?Modelcode=" + this.txtModelCodeQuery.Text.Trim());
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            this.Response.Redirect("../MOMODEL/FModelMP.aspx");
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
