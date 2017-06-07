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
	public partial class FSMTLoadFeederQueryQP : BaseMPageNew
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
			this.gridHelper.AddColumn( "MachineCode", "��̨����",	null);
			this.gridHelper.AddColumn( "MachineStationCode", "��̨����",	null);
			this.gridHelper.AddColumn( "FeederSpecCode", "Feeder���",	null);
			this.gridHelper.AddColumn( "FeederCode", "Feeder����",	null);
			this.gridHelper.AddColumn( "ReelNo", "�Ͼ���",	null);
			this.gridHelper.AddColumn( "MaterialCode", "���ϴ���",	null);
			this.gridHelper.AddColumn( "LotNo", "����",	null);
			this.gridHelper.AddColumn( "ResourceCode", "��Դ",	null);
			this.gridHelper.AddColumn( "StepSequenceCode", "����",	null);
			this.gridHelper.AddColumn( "CheckResult", "�ȶԽ��",	null);
			this.gridHelper.AddColumn( "LoadType", "��������",	null);
			this.gridHelper.AddColumn( "LoadUser", "������Ա",	null);
			this.gridHelper.AddColumn( "LoadDate", "��������",	null);
			this.gridHelper.AddColumn( "LoadTime", "����ʱ��",	null);
			this.gridHelper.AddColumn( "UnLoadType", "��������",	null);
			this.gridHelper.AddColumn( "UnLoadUser", "������Ա",	null);
			this.gridHelper.AddColumn( "UnLoadDate", "��������",	null);
			this.gridHelper.AddColumn( "UnLoadTime", "����ʱ��",	null);
            this.gridHelper.AddColumn("ReelUsedQty", "�Ͼ�����", HorizontalAlign.Right);
			this.gridHelper.AddColumn( "ExchangeFeederCode", "����Feeder",	null);
			this.gridHelper.AddColumn( "ExchageReelNo", "�����Ͼ�",	null);
			this.gridHelper.AddColumn( "FailReason", "ʧ��ԭ��",	null);
            this.gridHelper.AddColumn("RowCheckResult", "hhhhh", true);

            this.gridWebGrid.Columns.FromKey("RowCheckResult").Hidden = true;
            //this.gridWebGrid.Columns.FromKey("ReelUsedQty").Format = "#,#";
            //this.gridWebGrid.Columns.FromKey("ReelUsedQty").CellStyle.HorizontalAlign = HorizontalAlign.Right;

			this.gridHelper.AddDefaultColumn( true, false );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

			for (int i = 1; i < this.gridWebGrid.Columns.Count; i++)
			{
				this.gridWebGrid.Columns[i].Width = Unit.Pixel(90);
			}
		}

        protected override void cmdQuery_Click(object sender, EventArgs e)
        {
            base.cmdQuery_Click(sender, e);
            foreach (GridRecord row  in this.gridWebGrid.Rows)
            {
                if (FormatHelper.StringToBoolean(row.Items.FindItemByKey("RowCheckResult").Value.ToString().Trim()) == false)
                {
                    row.CssClass = "ForeColorRed";
                }
            }
        }
		protected override DataRow GetGridRow(object obj)
		{
			MachineFeederLog item = (MachineFeederLog)obj;
                //new object[]{
                //                "",
                //                item.ProductCode,
                //                item.MOCode,
                //                item.MachineCode,
                //                item.MachineStationCode,
                //                item.FeederSpecCode,
                //                item.FeederCode,
                //                item.ReelNo,
                //                item.MaterialCode,
                //                item.LotNo,
                //                item.OpeResourceCode,
                //                item.OpeStepSequenceCode,
                //                FormatHelper.StringToBoolean(item.CheckResult).ToString(),
                //                item.OperationType,
                //                item.LoadUser,
                //                FormatHelper.ToDateString(item.LoadDate),
                //                FormatHelper.ToTimeString(item.LoadTime),
                //                item.UnLoadType,
                //                item.UnLoadUser,
                //                FormatHelper.ToDateString(item.UnLoadDate),
                //                FormatHelper.ToTimeString(item.UnLoadTime),
                //                item.ReelUsedQty,
                //                item.ExchangeFeederCode,
                //                item.ExchageReelNo,
                //                ParseFailReason(item.FailReason)
                //                });

            DataRow row = this.DtSource.NewRow();
            row["ProductCode"] = item.ProductCode;
            row["MOCode"] = item.MOCode;
            row["MachineCode"] = item.MachineCode;
            row["MachineStationCode"] = item.MachineStationCode;
            row["FeederSpecCode"] = item.FeederSpecCode;
            row["FeederCode"] = item.FeederCode;
            row["ReelNo"] = item.ReelNo;
            row["MaterialCode"] = item.MaterialCode;
            row["LotNo"] = item.LotNo;
            row["ResourceCode"] = item.OpeResourceCode;
            row["StepSequenceCode"] = item.OpeStepSequenceCode;
            row["CheckResult"] = FormatHelper.StringToBoolean(item.CheckResult).ToString();
            row["LoadType"] = item.OperationType;
            row["LoadUser"] = item.LoadUser;
            row["LoadDate"] = FormatHelper.ToDateString(item.LoadDate);
            row["LoadTime"] = FormatHelper.ToTimeString(item.LoadTime);
            row["UnLoadType"] = item.UnLoadType;
            row["UnLoadUser"] = item.UnLoadUser;
            row["UnLoadDate"] = FormatHelper.ToDateString(item.UnLoadDate);
            row["UnLoadTime"] = FormatHelper.ToTimeString(item.UnLoadTime);
            row["ReelUsedQty"] = String.Format("{0:#,#}",item.ReelUsedQty);
            row["ExchangeFeederCode"] = item.ExchangeFeederCode;
            row["ExchageReelNo"] = item.ExchageReelNo;
            row["FailReason"] = ParseFailReason(item.FailReason);
            row["RowCheckResult"] = item.CheckResult;
            //if (FormatHelper.StringToBoolean(item.CheckResult) == false)
            //{
            //    row.Style.ForeColor = Color.Red;
            //}
			return row;
		}
		private string ParseFailReason(string reason)
		{
			if (reason == string.Empty)
				return string.Empty;
			if (reason.IndexOf(" ") < 0)
				return this.languageComponent1.GetString(reason);
			else
				return this.languageComponent1.GetString(reason.Substring(0, reason.IndexOf(" "))) + reason.Substring(reason.IndexOf(" "));
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QueryMachineFeederLog(
				this.txtConditionItem.Text.Trim().ToUpper(),
				this.txtConditionMo.Text.Trim().ToUpper(),
				this.txtMaterialCodeQuery.Text.Trim().ToUpper(),
				this.txtLineCodeQuery.Text.Trim().ToUpper(),
				this.txtMachineCodeQuery.Text.Trim().ToUpper(),
				this.txtMachineStationCodeQuery.Text.Trim().ToUpper(),
				this.txtUserCodeQuery.Text.Trim().ToUpper(),
				this.txtMaterialLotNoQuery.Text.Trim(),
				this.txtReelNoQuery.Text.Trim().ToUpper(),
				this.ddlResultQuery.SelectedValue,
				FormatHelper.TODateInt(this.txtLoadBeginDate.Text).ToString(),
				FormatHelper.TODateInt(this.txtLoadEndDate.Text).ToString(),
				inclusive, exclusive );
		}

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QueryMachineFeederLogCount( 
				this.txtConditionItem.Text.Trim().ToUpper(),
				this.txtConditionMo.Text.Trim().ToUpper(),
				this.txtMaterialCodeQuery.Text.Trim().ToUpper(),
				this.txtLineCodeQuery.Text.Trim().ToUpper(),
				this.txtMachineCodeQuery.Text.Trim().ToUpper(),
				this.txtMachineStationCodeQuery.Text.Trim().ToUpper(),
				this.txtUserCodeQuery.Text.Trim().ToUpper(),
				this.txtMaterialLotNoQuery.Text.Trim().ToUpper(),
				this.txtReelNoQuery.Text.Trim().ToUpper(),
				this.ddlResultQuery.SelectedValue,
				FormatHelper.TODateInt(this.txtLoadBeginDate.Text).ToString(),
				FormatHelper.TODateInt(this.txtLoadEndDate.Text).ToString());
		}

		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			MachineFeederLog item = (MachineFeederLog)obj;
			return new string[]{
								   item.ProductCode,
								   item.MOCode,
								   item.MachineCode,
								   item.MachineStationCode,
								   item.FeederSpecCode,
								   item.FeederCode,
								   item.ReelNo,
								   item.MaterialCode,
								   item.LotNo,
								   item.OpeResourceCode,
								   item.OpeStepSequenceCode,
								   FormatHelper.StringToBoolean(item.CheckResult).ToString(),
								   item.OperationType,
								   item.LoadUser,
								   FormatHelper.ToDateString(item.LoadDate),
								   FormatHelper.ToTimeString(item.LoadTime),
								   item.UnLoadType,
								   item.UnLoadUser,
								   FormatHelper.ToDateString(item.UnLoadDate),
								   FormatHelper.ToTimeString(item.UnLoadTime),
								   item.ReelUsedQty.ToString(),
								   item.ExchangeFeederCode,
								   item.ExchageReelNo,
								   ParseFailReason(item.FailReason) };
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	
									"ItemCode",
									"MOCode",
									"MachineCode",
									"MachineStationCode",
									"FeederSpecCode",
									"FeederCode",
									"ReelNo",
									"MaterialCode",
									"LotNo",
									"ResourceCode",
									"StepSequenceCode",
									"CheckResult",
									"LoadType",
									"LoadUser",
									"LoadDate",
									"LoadTime",
									"UnLoadType",
									"UnLoadUser",
									"UnLoadDate",
									"UnLoadTime",
									"ReelUsedQty",
									"ExchangeFeederCode",
									"ExchageReelNo",
									"FailReason"};
		}
		#endregion


	}
}
