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

using BenQGuru.eMES.Web.UserControl ;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;


namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FOQCFirstHandingYieldDetailQP ��ժҪ˵����
	/// </summary>
	public partial class FOQCFirstHandingYieldDetailQP2  : BaseRMPage
	{

		private System.ComponentModel.IContainer components;
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected System.Web.UI.WebControls.Label lblMO;
		protected System.Web.UI.HtmlControls.HtmlInputText txtMO;

		private QueryFacade1 _facade = null ;
 // FacadeFactory.CreateQueryFacade1();
		protected GridHelperForRPT  _gridHelper = null;


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
			if(!this.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.txtModelCode.Value = this.GetRequestParam("MODELCODE") ;
				this.txtItemCode.Value = this.GetRequestParam("ITEMCODE") ;
				this.txtStartDate.Value = this.GetRequestParam("STARTDATE");
				this.txtEndDate.Value = this.GetRequestParam("ENDDATE");
				
			}

			_gridHelper = new GridHelperForRPT(gridWebGrid);

			this._gridHelper.GridHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadDataSource);
			this._gridHelper.GridHelper.GetRowCountHandle = new GetRowCountDelegate(this.GetRowCount);
			this._gridHelper.GridHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridRow);	
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}
		#endregion

		#region WebGrid
		protected override void InitWebGrid()
		{
			this._gridHelper.GridHelper.AddColumn( "LOTNO1", "�������",	null);
			this._gridHelper.GridHelper.AddColumn( "LOTSIZE", "����",	null);
			this._gridHelper.GridHelper.AddColumn( "PlanSIZE", "�ƻ����������",	null);
			this._gridHelper.GridHelper.AddColumn( "ActCheckSIZE", "ʵ�ʳ��������",	null);
			this._gridHelper.GridHelper.AddColumn( "AGRADETIMES", "A�ȼ�",	null);
			this._gridHelper.GridHelper.AddColumn( "BGGRADETIMES", "B�ȼ�",	null);
			this._gridHelper.GridHelper.AddColumn( "CGRADETIMES", "C�ȼ�",	null);
            this._gridHelper.GridHelper.AddColumn( "ZGRADETIMES", "Z�ȼ�", null);
			this._gridHelper.GridHelper.AddColumn( "LOTSTATUS1", "�ж����",	null);
			this._gridHelper.GridHelper.AddColumn( "MDATE1", "�ж�����",	null);
			this._gridHelper.GridHelper.AddColumn( "MTIME1", "�ж�ʱ��",	null);
			this._gridHelper.GridHelper.AddColumn( "MUSER1", "������",	null);

			//������
			this._gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );

			base.InitWebGrid();
			this._gridHelper.GridHelper.RequestData();

		}
		
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{
								((OQCFirstHandingYieldDetail)obj).LotNo.ToString(),
								((OQCFirstHandingYieldDetail)obj).LotSize.ToString(),
								((OQCFirstHandingYieldDetail)obj).SSize.ToString(),
								((OQCFirstHandingYieldDetail)obj).ActCheckSize.ToString(),
								((OQCFirstHandingYieldDetail)obj).Agradetimes.ToString(),
								((OQCFirstHandingYieldDetail)obj).Bggradetimes.ToString(),
								((OQCFirstHandingYieldDetail)obj).Cgradetimes.ToString(),
                                ((OQCFirstHandingYieldDetail)obj).ZGrageTimes.ToString(),
								this.languageComponent1.GetString(((OQCFirstHandingYieldDetail)obj).LotStatus.ToString()),
								FormatHelper.ToDateString(((OQCFirstHandingYieldDetail)obj).MaintainDate),
								FormatHelper.ToTimeString(((OQCFirstHandingYieldDetail)obj).MaintainTime),
								((OQCFirstHandingYieldDetail)obj).MaintainUser.ToString()
							}
                
                
				);
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null)
			{
				_facade = new FacadeFactory(base.DataProvider).CreateQueryFacade1();
			}
			return this._facade.QueryFirstHandingYieldDetail(
				"",
				this.txtItemCode.Value,
				FormatHelper.TODateInt(this.txtStartDate.Value),
				FormatHelper.TODateInt(this.txtEndDate.Value),
				inclusive,
				exclusive);
		}


		protected override int GetRowCount()
		{
			if(_facade==null)
			{
				_facade = new FacadeFactory(base.DataProvider).CreateQueryFacade1();
			}
			return this._facade.QueryFirstHandingYieldDetailCount(
				"",
				this.txtItemCode.Value,
				FormatHelper.TODateInt(this.txtStartDate.Value),
				FormatHelper.TODateInt(this.txtEndDate.Value)
				);
		}

		#endregion
        
		#region Export 	
		protected override string[] FormatExportRecord( object obj )
		{
			return  new string[]{
									((OQCFirstHandingYieldDetail)obj).LotNo.ToString(),
									((OQCFirstHandingYieldDetail)obj).LotSize.ToString(),
									((OQCFirstHandingYieldDetail)obj).SSize.ToString(),
									((OQCFirstHandingYieldDetail)obj).Agradetimes.ToString(),
									((OQCFirstHandingYieldDetail)obj).Bggradetimes.ToString(),
									((OQCFirstHandingYieldDetail)obj).Cgradetimes.ToString(),
                                    ((OQCFirstHandingYieldDetail)obj).ZGrageTimes.ToString(),
									this.languageComponent1.GetString(((OQCFirstHandingYieldDetail)obj).LotStatus.ToString()),
									FormatHelper.ToDateString(((OQCFirstHandingYieldDetail)obj).MaintainDate),
									FormatHelper.ToTimeString(((OQCFirstHandingYieldDetail)obj).MaintainTime),
									((OQCFirstHandingYieldDetail)obj).MaintainUser.ToString()
			}
			;
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {
									"LOTNO1",
									"LOTSIZE1",
									"SSIZE",
									"AGRADETIMES", 
									"BGGRADETIMES", 
									"CGRADETIMES", 
                                    "ZGRADETIMES",
									"LOTSTATUS1", 
									"MDATE1", 
									"MTIME1", 
									"MUSER1" 
								};
			
		}
		#endregion

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			string referedURL = this.GetRequestParam("REFEREDURL") ;
			if( referedURL == string.Empty)
			{
				referedURL = "FOQCFirstHandingYieldQP.aspx" ;
			}
			else
			{
				referedURL = System.Web.HttpUtility.UrlDecode(referedURL) ;
			}
			Response.Redirect( referedURL ) ;

		}
	}
}
