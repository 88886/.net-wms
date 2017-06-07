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
using BenQGuru.eMES.SMT;
using BenQGuru.eMES.Domain.SMT;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;



namespace BenQGuru.eMES.Web.SMT
{
	/// <summary>
	/// FFeederMP ��ժҪ˵����
	/// </summary>
	public partial class FSMTLoadMaterialLogQP : BaseMPageNew
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
        protected global::System.Web.UI.WebControls.TextBox txtImportDate;
		
		private BenQGuru.eMES.SMT.SMTFacade _facade ;//= new SMTFacadeFactory().Create();
	
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
			//this.gridWebGrid.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
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
			if (!this.IsPostBack)
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
			this.gridHelper.AddColumn( "LogNo", "", null);
			this.gridHelper.AddColumn( "ProductCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "sscode", "���ߴ���",	null);
			this.gridHelper.AddColumn( "ImportUser", "������Ա",	null);
			this.gridHelper.AddColumn( "ImportDate", "��������",	null);
			this.gridHelper.AddColumn( "ImportTime", "����ʱ��",	null);
			this.gridHelper.AddLinkColumn( "ViewDetail", "��ϸ�б�", null);

			this.gridWebGrid.Columns.FromKey("LogNo").Hidden = true;
			this.gridHelper.AddDefaultColumn( true, false );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}

        protected override DataRow GetGridRow(object obj)
		{
			SMTFeederMaterialImportLog item = (SMTFeederMaterialImportLog)obj;
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
            //    new object[]{
            //                    "",
            //                    item.LOGNO.ToString(),
            //                    item.ProductCode,
            //                    item.StepSequenceCode,
            //                    item.ImportUser,
            //                    FormatHelper.ToDateString(item.ImportDate),
            //                    FormatHelper.ToTimeString(item.ImportTime),
            //                    ""
            //                });
            DataRow row = this.DtSource.NewRow();
            row["LogNo"] = item.LOGNO.ToString();
            row["ProductCode"] = item.ProductCode;
            row["sscode"] = item.StepSequenceCode;
            row["ImportUser"] = item.ImportUser;
            row["ImportDate"] = FormatHelper.ToDateString(item.ImportDate);
            row["ImportTime"] = FormatHelper.ToTimeString(item.ImportTime);
            return row;
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QuerySMTFeederMaterialImportLog( 
				this.txtItemCodeQuery.Text.Trim().ToUpper(),
				this.txtSSCodeQuery.Text.Trim().ToUpper(),
				this.txtImportUserQuery.Text.Trim().ToUpper(),
				FormatHelper.TODateInt(this.txtImportDate.Text).ToString(),
				inclusive, exclusive );
		}

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QuerySMTFeederMaterialImportLogCount( 
				this.txtItemCodeQuery.Text.Trim().ToUpper(),
				this.txtSSCodeQuery.Text.Trim().ToUpper(),
				this.txtImportUserQuery.Text.Trim().ToUpper(),
				FormatHelper.TODateInt(this.txtImportDate.Text).ToString());
		}

        protected override void Grid_ClickCell(GridRecord row, string commandName)
		{
            if (commandName == "ViewDetail")
			{
				string strUrl = this.MakeRedirectUrl("FSMTLoadMaterialLogDetailQP.aspx", 
					new string[]{"itemcode", "sscode", "importuser", "importdate", "importtime", "logno"},
                    new string[]{row.Items.FindItemByKey("ProductCode").Text,
									row.Items.FindItemByKey("sscode").Text,
									row.Items.FindItemByKey("ImportUser").Text,
									FormatHelper.TODateInt(row.Items.FindItemByKey("ImportDate").Text).ToString(),
									FormatHelper.TOTimeInt(row.Items.FindItemByKey("ImportTime").Text).ToString(),
									row.Items.FindItemByKey("LogNo").Text }
					);
				this.Response.Redirect(strUrl);
			}
		}

		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			SMTFeederMaterialImportLog item = (SMTFeederMaterialImportLog)obj;
			return new string[]{
								   item.ProductCode,
								   item.StepSequenceCode,
								   item.ImportUser,
								   FormatHelper.ToDateString(item.ImportDate),
								   FormatHelper.ToTimeString(item.ImportTime)
							};
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	
									"ProductCode",
									"sscode",
									"ImportUser",
									"ImportDate",
									"ImportTime"};
		}
		#endregion
	}
}
