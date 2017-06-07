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
	/// FSMTFeederLogQP ��ժҪ˵����
	/// </summary>
	public partial class FSMTFeederLogQP : BaseMPageNew
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		
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
			this.gridHelper.AddColumn( "FeederCode", "Feeder����",	null);
			this.gridHelper.AddColumn( "FeederType", "Feeder����",	null);
			this.gridHelper.AddColumn( "FeederSpecCode", "������",	null);
			this.gridHelper.AddColumn( "Status", "״̬",	null);
			this.gridHelper.AddColumn( "MOCode", "��������",	null);
			this.gridHelper.AddColumn( "sscode", "���ߴ���",	null);
			this.gridHelper.AddColumn( "OperationType", "��������",	null);
			this.gridHelper.AddColumn( "OperationUser", "������Ա",	null);
			this.gridHelper.AddColumn( "OperationDate", "��������",	null);
			this.gridHelper.AddColumn( "OperationTime", "����ʱ��",	null);

			this.gridHelper.AddDefaultColumn( false, false );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
            this.gridWebGrid.Columns.FromKey("FeederType").Hidden = true;

		}
		
		protected override DataRow GetGridRow(object obj)
		{
			FeederStatusLog item = (FeederStatusLog)obj;
            //Infragistics.WebUI.UltraWebGrid.UltraGridRow row = new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
            //    new object[]{
            //                    item.FeederCode,
            //                    item.FeederType,
            //                    item.FeederSpecCode,
            //                    this.languageComponent1.GetString("SMT_FeederStatus_" + item.Status),
            //                    item.MOCode,
            //                    item.StepSequenceCode,
            //                    this.languageComponent1.GetString("SMT_FeederOperationType_" + item.StatusChangedReason),
            //                    item.OperationUser,
            //                    FormatHelper.ToDateString(item.StatusChangedDate),
            //                    FormatHelper.ToTimeString(item.StatusChangedTime)
            //                });
            DataRow row = this.DtSource.NewRow();
            row["FeederCode"] = item.FeederCode;
            row["FeederType"] = item.FeederType;
            row["FeederSpecCode"] = item.FeederSpecCode;
            row["Status"] = this.languageComponent1.GetString("SMT_FeederStatus_" + item.Status);
            row["MOCode"] = item.MOCode;
            row["sscode"] = item.StepSequenceCode;
            row["OperationType"] = this.languageComponent1.GetString("SMT_FeederOperationType_" + item.StatusChangedReason);
            row["OperationUser"] = item.OperationUser;
            row["OperationDate"] = FormatHelper.ToDateString(item.StatusChangedDate);
            row["OperationTime"] = FormatHelper.ToTimeString(item.StatusChangedTime);

			return row;
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QueryFeederStatusLog(
				this.txtConditionItem.Text.Trim().ToUpper(),
				this.txtConditionMo.Text.Trim().ToUpper(),
				this.txtSSCode.Text.Trim().ToUpper(),
				this.txtFeederSpecCode.Text.Trim().ToUpper(),
				this.txtFeederCode.Text.Trim().ToUpper(),
				inclusive, exclusive );
		}

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QueryFeederStatusLogCount( 
				this.txtConditionItem.Text.Trim().ToUpper(),
				this.txtConditionMo.Text.Trim().ToUpper(),
				this.txtSSCode.Text.Trim().ToUpper(),
				this.txtFeederSpecCode.Text.Trim().ToUpper(),
				this.txtFeederCode.Text.Trim().ToUpper());
		}

		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			FeederStatusLog item = (FeederStatusLog)obj;
			return new string[]{
								item.FeederCode,
								//item.FeederType,
								item.FeederSpecCode,
                                this.languageComponent1.GetString("SMT_FeederStatus_" + item.Status),
							    item.MOCode,								
								item.StepSequenceCode,
								this.languageComponent1.GetString("SMT_FeederOperationType_" + item.StatusChangedReason),
								item.OperationUser,
								FormatHelper.ToDateString(item.StatusChangedDate),
								FormatHelper.ToTimeString(item.StatusChangedTime)
							};
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	
									"FeederCode",
									//"FeederType",
									"FeederSpecCode",
									"Status",
									"MOCode",
									"sscode",
									"OperationType",
									"OperationUser",
									"OperationDate",
									"OperationTime"};
		}
		#endregion


	}
}
