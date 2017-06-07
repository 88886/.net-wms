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
using BenQGuru.eMES.Web.Helper;
using Infragistics.WebUI.UltraWebGrid;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FTSRecordDetailSP ��ժҪ˵����
	/// </summary>
	public partial class FTSRecordDetailSP2 : BasePage
	{
		protected Infragistics.WebUI.UltraWebGrid.UltraWebGrid UltraWebGrid1;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected BenQGuru.eMES.Web.Helper.ExcelExporter excelExporter;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.WebControls.CheckBox Checkbox1;
		protected BenQGuru.eMES.Web.Helper.PagerToolBar Pagertoolbar1;
		private BenQGuru.eMES.Web.Helper.GridHelperForRPT gridHelper = null;
		private BenQGuru.eMES.Web.Helper.ButtonHelper buttonHelper = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.InitOnPostBack();

			if(!IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.InitWebGrid();
				object[] objs = new object[]{"false","ErrGroup001","Err001","Model001","OP1","MO-002","TS002","Res_AI","Route110","TSStation1","TSWorker","2004/05/09"};
				UltraGridRow Row = new UltraGridRow(objs);
				this.gridWebGrid.Rows.Add(Row);
				
			}
			// �ڴ˴������û������Գ�ʼ��ҳ��
		}

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
			this.excelExporter = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
			// 
			// languageComponent1
			// 
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";
			// 
			// excelExporter
			// 
			this.excelExporter.FileExtension = "xls";
			this.excelExporter.LanguageComponent = null;
			this.excelExporter.Page = null;
			this.excelExporter.RowSplit = "\r\n";

		}

		#region(Init)
		
		#endregion
		private void InitOnPostBack()
		{		
			this.buttonHelper = new ButtonHelper(this);
//			this.buttonHelper.SetEditObjectHandle = new SetEditObjectDelegate(this.SetEditObject);

			this.gridHelper = new GridHelperForRPT(this.gridWebGrid);
//			this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadDataSource);
//			this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridRow);

//			this.pagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);
			//			this.excelExporter.LoadExportDataHandle = new LoadExportDataDelegate(LoadDataSource);
			//			this.excelExporter.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatExportRecord);
			//			this.excelExporter.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetColumnHeaderText);
		}

		private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
		{
//			this.gridHelper.GridBind( this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize );
		}

		private void InitButton()
		{	
			this.buttonHelper.PageActionStatusHandle( PageActionType.Add );
			this.buttonHelper.AddDeleteConfirm();
		}

		protected Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			// TODO: �����ֶ�ֵ��˳��ʹ֮��Grid���ж�Ӧ

			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{"false","TS","","Model","Item111","MO00111","2005/04/02","Res_AI","2005/04/06","2005/04/08","Station11",""
							});
		}


		/// <summary>
		/// angel zhu add 
		/// Column�ڵ�Key�ֲ���ȷ�����Ժ���Ҫ��������
		/// </summary>
		private void InitWebGrid()
		{   
			this.gridHelper.GridHelper.AddColumn("a", "����������",	null);
			this.gridHelper.GridHelper.AddColumn( "b", "��������",	null);
			this.gridHelper.GridHelper.AddColumn("c","����ԭ��",null);
			this.gridHelper.GridHelper.AddColumn( "d", "���α�",	null);
			this.gridHelper.GridHelper.AddColumn( "e", "�������",	null);
			this.gridHelper.GridHelper.AddColumn( "f", "����λ��",	null);
			this.gridHelper.GridHelper.AddColumn( "g", "����Ԫ��",	null);
			this.gridHelper.GridHelper.AddColumn( "h", "����Ԫ��",	null);
			this.gridHelper.GridHelper.AddColumn( "i", "ά��վ",	null);
			this.gridHelper.GridHelper.AddColumn( "j", "ά�޹�",	null);
			this.gridHelper.GridHelper.AddColumn( "k", "ά������",	null);
			this.gridHelper.GridHelper.AddDefaultColumn(true, false);

			//������
			this.gridHelper.GridHelper.ApplyLanguage( this.languageComponent1 );
		}

		#endregion
	}
}
