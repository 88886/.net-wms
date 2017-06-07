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
	/// FSMTRCardItemDetailQP ��ժҪ˵����
	/// </summary>
	public partial class FSMTRCardItemDetailQP : BaseMPageNew
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

				this.txtMOCodeQuery.Text = this.GetRequestParam("mocode");
				this.txtRCardQuery.Text = this.GetRequestParam("rcard");
				if (this.txtMOCodeQuery.Text == string.Empty || this.txtRCardQuery.Text == string.Empty)
				{
					BenQGuru.eMES.Common.ExceptionManager.Raise( this.GetType() , "$Error_RequestUrlParameter_Lost"); 
				}
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
			this.gridHelper.AddColumn( "MaterialCode", "���ϴ���",	null);
			this.gridHelper.AddColumn( "ReelNo", "�Ͼ���",	null);
			this.gridHelper.AddColumn( "LotNo", "��������",	null);
			this.gridHelper.AddColumn( "DateCode", "��������",	null);
            this.gridHelper.AddColumn("UnitQty", "��λ����", HorizontalAlign.Right);
			

			this.gridHelper.AddDefaultColumn( false, false );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

			this.cmdQuery_Click(null, null);
		}
		
		protected override DataRow GetGridRow(object obj)
		{
			SMTMachineInno item = (SMTMachineInno)obj;
          
            DataRow row = this.DtSource.NewRow();
            row["MaterialCode"] = item.MaterialCode;
            row["ReelNo"] = item.ReelNo;
            row["LotNo"] = item.LotNo;
            row["DateCode"] = item.DateCode;
            row["UnitQty"] = String.Format("{0:#,#}",item.UnitQty);

			return row;
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QuerySMTRCardMaterialDetail(
				this.txtMOCodeQuery.Text.Trim().ToUpper(),
				this.txtRCardQuery.Text.Trim().ToUpper(),
				inclusive, exclusive );
		}

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QuerySMTRCardMaterialDetailCount( 
				this.txtMOCodeQuery.Text.Trim().ToUpper(),
				this.txtRCardQuery.Text.Trim().ToUpper());
		}

		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			//Laws Lu,2006/12/12 type convert error
			SMTMachineInno item = (SMTMachineInno)obj;
			return new string[]{
								   item.MaterialCode,
								   item.ReelNo,
								   item.LotNo,
								   item.DateCode,
								   item.UnitQty.ToString()
							   };
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	
									"MaterialCode",
									"ReelNo",
									"LotNo",
									"DateCode",
									"UnitQty" };
		}
		#endregion

		protected void cmdCancel_ServerClick(object sender, EventArgs e)
		{
			Response.Redirect("FSMTRCardItemQP.aspx");
		}
	}
}
