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
	public partial class FErrorCauseGroup2ErrorCauseAP : BaseAPageNew
	{
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		
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
			this.txtErrorCauseGroupCodeQuery.Text = this.GetRequestParam("ErrorCauseGroup");
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
			this.gridHelper.AddColumn( "UnAssErrorCause", "δ��������ԭ��",	null);
			this.gridHelper.AddColumn( "ErrorCauseDescription", "����ԭ������",	null);
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
			_facade.AddErrorCauseGroup2ErrorCause((ErrorCauseGroup2ErrorCause[])domainObject.ToArray(typeof(ErrorCauseGroup2ErrorCause)));
		}

		protected override int GetRowCount()
		{			
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			return this._facade.GetUnselectedErrorCauseByErrorCauseGroupCodeCount(FormatHelper.PKCapitalFormat( this.txtErrorCauseGroupCodeQuery.Text.Trim()),
                FormatHelper.PKCapitalFormat( this.txtErrorCauseCodeQuery.Text.Trim()));
		}

		protected override DataRow GetGridRow(object obj)
		{
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
            //    new object[]{"false",
            //                    FormatHelper.PKCapitalFormat( ((ErrorCause)obj).ErrorCauseCode.ToString() ),
            //                    ((ErrorCause)obj).ErrorCauseDescription.ToString() ,
            //                    ((ErrorCause)obj).GetDisplayText("MaintainUser"),
            //                    FormatHelper.ToDateString(((ErrorCause)obj).MaintainDate),
            //                    FormatHelper.ToTimeString(((ErrorCause)obj).MaintainTime)});
            DataRow row = this.DtSource.NewRow();
            row["UnAssErrorCause"] = FormatHelper.PKCapitalFormat(((ErrorCause)obj).ErrorCauseCode.ToString());
            row["ErrorCauseDescription"] = ((ErrorCause)obj).ErrorCauseDescription.ToString();
            row["MaintainUser"] = ((ErrorCause)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((ErrorCause)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((ErrorCause)obj).MaintainTime);
            return row;
		}

		protected override object GetEditObject(GridRecord row)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			ErrorCauseGroup2ErrorCause relation = _facade.CreateNewErrorCauseGroup2ErrorCause();
			relation.ErrorCauseGroupCode = this.txtErrorCauseGroupCodeQuery.Text.Trim();
            relation.ErrorCause = row.Items.FindItemByKey("UnAssErrorCause").Text;		
			relation.MaintainUser = this.GetUserCode();

			return relation;
		}


		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{			
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			return _facade.GetUnselectedErrorCauseByErrorCauseGroupCode(
                FormatHelper.PKCapitalFormat( this.txtErrorCauseGroupCodeQuery.Text.Trim()),
                FormatHelper.PKCapitalFormat( this.txtErrorCauseCodeQuery.Text.Trim()),
                inclusive,exclusive);
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FErrorCauseGroup2ErrorCauseSP.aspx", new string[]{"ErrorCauseGroup"}, new string[]{this.GetRequestParam("ErrorCauseGroup")}));
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
