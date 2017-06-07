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

using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.TSModel;

namespace BenQGuru.eMES.Web.TSModel
{
	/// <summary>
	/// FModel2ErrorCauseAP ��ժҪ˵����
	/// </summary>
	public partial class FModel2ErrorCauseGroupAP : BaseAPageNew
	{
    
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
	
        protected System.Web.UI.WebControls.Label lblErrorGroupCode;
        protected System.Web.UI.WebControls.Label lblErrorCodeCodeQuery;

        private TSModelFacade _facade ;//= TSModelFacadeFactory.CreateTSModelFacade();

        #region Stable
        protected void Page_Load(object sender, System.EventArgs e)
		{
            this.gridHelper = new GridHelperNew(this.gridWebGrid,this.DtSource);
			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}
            this.txtModelCodeQuery.Text = this.GetRequestParam("ModelCode");
        }

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}
        #endregion

        #region Not Stable
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn( "UnAsssErrorCauseGroup", "δ��������ԭ�������",	null);
            this.gridHelper.AddColumn( "ErrorCauseGroupDescription", "����ԭ��������",	null);
            this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
            this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
            this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);	

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
			this.gridHelper.AddDefaultColumn( true, false );

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
            
            base.InitWebGrid();
        }

		protected override void AddDomainObject(ArrayList domainObject)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			_facade.AddModel2ErrorCauseGroup( (Model2ErrorCauseGroup[])domainObject.ToArray(typeof(Model2ErrorCauseGroup)));
		}

        protected override int GetRowCount()
        {			
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            return this._facade.GetUnselectedErrorCauseGroupByModelCodeCount( 
				FormatHelper.PKCapitalFormat(this.txtModelCodeQuery.Text.Trim()),
				FormatHelper.PKCapitalFormat(this.txtErrorCauseGroupCodeQuery.Text.Trim()));
        }

        protected override DataRow GetGridRow(object obj)
        {
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
            //    new object[]{"false",
            //                    ((ErrorCauseGroup)obj).ErrorCauseGroupCode.ToString(),
            //                    ((ErrorCauseGroup)obj).ErrorCauseGroupDescription.ToString(),
            //                    ((ErrorCauseGroup)obj).MaintainUser.ToString(),
            //                    FormatHelper.ToDateString(((ErrorCauseGroup)obj).MaintainDate),
            //                    FormatHelper.ToTimeString(((ErrorCauseGroup)obj).MaintainTime)
            //                });
            DataRow row = this.DtSource.NewRow();
            row["UnAsssErrorCauseGroup"] = ((ErrorCauseGroup)obj).ErrorCauseGroupCode.ToString();
            row["ErrorCauseGroupDescription"] = ((ErrorCauseGroup)obj).ErrorCauseGroupDescription.ToString();
            row["MaintainUser"] = ((ErrorCauseGroup)obj).MaintainUser.ToString();
            row["MaintainDate"] = FormatHelper.ToDateString(((ErrorCauseGroup)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((ErrorCauseGroup)obj).MaintainTime);
            return row;
        }

		protected override object GetEditObject(GridRecord row)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			Model2ErrorCauseGroup relation = _facade.CreateNewModel2ErrorCauseGroup();
			relation.ModelCode = this.txtModelCodeQuery.Text.Trim();
            relation.ErrorCauseGroupCode = row.Items.FindItemByKey("UnAsssErrorCauseGroup").Text;	
			relation.MaintainUser = this.GetUserCode();

			return relation;
		}

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {		
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
            return this._facade.GetUnselectedErrorCauseGroupByModelCode( 
				FormatHelper.PKCapitalFormat(this.txtModelCodeQuery.Text.Trim()),
				FormatHelper.PKCapitalFormat(this.txtErrorCauseGroupCodeQuery.Text.Trim()),
				inclusive,exclusive);
        }

        //protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        //{
        //    this.Response.Redirect(this.MakeRedirectUrl("./FModel2ErrorCauseGroupSP.aspx", new string[]{"modelcode"}, new string[]{this.txtModelCodeQuery.Text.Trim()}));
        //}

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
