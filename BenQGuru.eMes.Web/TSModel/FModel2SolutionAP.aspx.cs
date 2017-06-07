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
	/// FOperation2ResourceSP ��ժҪ˵����
	/// </summary>
	public partial class FModel2SolutionAP : BaseAPageNew
	{

		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.WebControls.Label lblErrorGroupCode;

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
			this.gridHelper.AddColumn( "UnAssSolutionCode", "δ���������������",	null);
			this.gridHelper.AddColumn( "SolutionDescription", "�����������",	null);
			this.gridHelper.AddColumn( "SolutionImprove", "��������Ľ�",	null);
			this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
			this.gridWebGrid.Columns.FromKey("SolutionImprove").Hidden = true;
			this.gridHelper.AddDefaultColumn( true, false );
			
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		protected override void AddDomainObject(ArrayList domainObject)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			_facade.AddModel2Solution( (Model2Solution[])domainObject.ToArray(typeof(Model2Solution)));
		}

		protected override int GetRowCount()
		{			
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			return this._facade.GetUnselectedSolutionByModelCodeCount( 
				FormatHelper.PKCapitalFormat(this.txtModelCodeQuery.Text.Trim()),
				FormatHelper.PKCapitalFormat(this.txtSolutionCodeQuery.Text.Trim()));
		}

		protected override DataRow GetGridRow(object obj)
		{
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
            //    new object[]{"false",
            //                    ((Solution)obj).SolutionCode.ToString(),
            //                    ((Solution)obj).SolutionDescription.ToString(),
            //                    ((Solution)obj).SolutionImprove.ToString(),
            //                    ((Solution)obj).MaintainUser.ToString(),
            //                    FormatHelper.ToDateString(((Solution)obj).MaintainDate),
            //                    FormatHelper.ToTimeString(((Solution)obj).MaintainTime)});
            DataRow row = this.DtSource.NewRow();
            row["UnAssSolutionCode"] = ((Solution)obj).SolutionCode.ToString();
            row["SolutionDescription"] = ((Solution)obj).SolutionDescription.ToString();
            row["SolutionImprove"] = ((Solution)obj).SolutionImprove.ToString();
            row["MaintainUser"] = ((Solution)obj).MaintainUser.ToString();
            row["MaintainDate"] = FormatHelper.ToDateString(((Solution)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((Solution)obj).MaintainTime);
            return row;
		}

		protected override object GetEditObject(GridRecord row)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			Model2Solution relation = _facade.CreateNewModel2Solution();
			relation.ModelCode = this.txtModelCodeQuery.Text.Trim();
            relation.SolutionCode = row.Items.FindItemByKey("UnAssSolutionCode").Text;		
			relation.MaintainUser = this.GetUserCode();

			return relation;
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{			
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			return this._facade.GetUnselectedSolutionByModelCode( 
				FormatHelper.PKCapitalFormat(this.txtModelCodeQuery.Text.Trim()),
				FormatHelper.PKCapitalFormat(this.txtSolutionCodeQuery.Text.Trim()),
				inclusive,exclusive);
		}

        //protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        //{
        //    this.Response.Redirect(this.MakeRedirectUrl("./FModel2SolutionSP.aspx", new string[]{"modelcode"}, new string[]{this.txtModelCodeQuery.Text.Trim()}));
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
