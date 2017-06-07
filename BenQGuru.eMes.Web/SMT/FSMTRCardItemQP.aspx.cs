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
	/// FSMTRCardItemQP ��ժҪ˵����
	/// </summary>
	public partial class FSMTRCardItemQP : BaseMPageNew
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
        protected global::System.Web.UI.WebControls.TextBox txtLoadBeginDate;
        protected global::System.Web.UI.WebControls.TextBox txtLoadEndDate;
		
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

				this.txtLoadBeginDate.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(DateTime.Today));
				this.txtLoadEndDate.Text = this.txtLoadBeginDate.Text;
			}
			if (this.txtStartSN.Text == string.Empty && this.txtEndSN.Text != string.Empty)
				this.txtStartSN.Text = this.txtEndSN.Text;
			else if (this.txtEndSN.Text == string.Empty && this.txtStartSN.Text != string.Empty)
				this.txtEndSN.Text = this.txtStartSN.Text;
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
			this.gridHelper.AddColumn( "ProductCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "MOCode", "��������",	null);
			this.gridHelper.AddColumn( "RCard", "��Ʒ���к�",	null);
			this.gridHelper.AddColumn( "LoadResource", "������Դ",	null);
			this.gridHelper.AddColumn( "LoadStepSequenceCode", "���ϲ���",	null);
			this.gridHelper.AddColumn( "LoadUser", "������Ա",	null);
			this.gridHelper.AddColumn( "LoadDate", "��������",	null);
			this.gridHelper.AddColumn( "LoadTime", "����ʱ��",	null);
			this.gridHelper.AddLinkColumn( "MaterialDetail", "������ϸ", null);

			this.gridHelper.AddDefaultColumn( false, false );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

		}
		
		protected override DataRow GetGridRow(object obj)
		{
			SMTRCardMaterial item = (SMTRCardMaterial)obj;
          
            DataRow row = this.DtSource.NewRow();
            row["ProductCode"] = item.ItemCode;
            row["MOCode"] = item.MOCode;
            row["RCard"] = item.RunningCard;
            row["LoadResource"] = item.ResourceCode;
            row["LoadStepSequenceCode"] = item.StepSequenceCode;
            row["LoadUser"] = item.MaintainUser;
            row["LoadDate"] = FormatHelper.ToDateString(item.MaintainDate);
            row["LoadTime"] = FormatHelper.ToTimeString(item.MaintainTime);

			return row;
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QuerySMTRCardMaterial(
				this.txtConditionItem.Text.Trim().ToUpper(),
				this.txtConditionMo.Text.Trim().ToUpper(),
				FormatHelper.TODateInt(this.txtLoadBeginDate.Text),
				FormatHelper.TODateInt(this.txtLoadEndDate.Text),
				this.txtStartSN.Text.Trim().ToUpper(),
				this.txtEndSN.Text.Trim().ToUpper(),
				this.txtResourceCode.Text.Trim().ToUpper(),
				this.txtSSCode.Text.Trim().ToUpper(),
				this.txtMaterialCode.Text.Trim().ToUpper(),
				this.txtReelNoQuery.Text.Trim().ToUpper(),
				this.txtMaterialLotNoQuery.Text.Trim().ToUpper(),
				this.txtDateCodeQuery.Text.Trim().ToUpper(),
				inclusive, exclusive );
		}

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QuerySMTRCardMaterialCount( 
				this.txtConditionItem.Text.Trim().ToUpper(),
				this.txtConditionMo.Text.Trim().ToUpper(),
				FormatHelper.TODateInt(this.txtLoadBeginDate.Text),
				FormatHelper.TODateInt(this.txtLoadEndDate.Text),
				this.txtStartSN.Text.Trim().ToUpper(),
				this.txtEndSN.Text.Trim().ToUpper(),
				this.txtResourceCode.Text.Trim().ToUpper(),
				this.txtSSCode.Text.Trim().ToUpper(),
				this.txtMaterialCode.Text.Trim().ToUpper(),
				this.txtReelNoQuery.Text.Trim().ToUpper(),
				this.txtMaterialLotNoQuery.Text.Trim().ToUpper(),
				this.txtDateCodeQuery.Text.Trim().ToUpper());
		}

        protected override void Grid_ClickCell(GridRecord row, string commandName)
		{
            if (commandName == "MaterialDetail")
			{
				string strUrl = this.MakeRedirectUrl("FSMTRCardItemDetailQP.aspx", 
					new string[]{"mocode", "rcard"},
					new string[]{row.Items.FindItemByKey("MOCode").Text.ToString(), row.Items.FindItemByKey("RCard").Text.ToString()}
					);
				Response.Redirect(strUrl);
			}
		}

		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			SMTRCardMaterial item = (SMTRCardMaterial)obj;
			return new string[]{
								   item.ItemCode,
								   item.MOCode,
								   item.RunningCard,
								   item.ResourceCode,
								   item.StepSequenceCode,
								   item.MaintainUser,
								   FormatHelper.ToDateString(item.MaintainDate),
								   FormatHelper.ToTimeString(item.MaintainTime) };
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	
									"ItemCode",
									"MOCode",
									"RCard",
									"ResourceCode",
									"sscode",
									"MaintainUser",
									"MaintainDate",
									"MaintainTime"};
		}
		#endregion
	}
}
