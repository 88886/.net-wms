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

using Infragistics.WebUI.UltraWebGrid;

using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.SMT;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FFeederMP ��ժҪ˵����
	/// </summary>
	public partial class FOutMainQP : BaseMPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdAdd;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdDelete;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdSave;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdCancel;
	
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
			this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
		#endregion
		
		private BenQGuru.eMES.WebQuery.OutSourcingQueryFacade _facade = null;
		#region Init
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if( !this.IsPostBack )
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
			this.gridHelper.Grid.Columns.Clear();
			this.gridHelper.Grid.Rows.Clear();
			
			if (this.rdbType.SelectedValue == "PCS")
			{
				this.gridHelper.AddColumn("MOCode", "��������");
				this.gridHelper.AddColumn("ItemCode", "�Ϻ�");
				this.gridHelper.AddColumn("PlanQty", "�ƻ�����");
				this.gridHelper.AddColumn("Qty", "����");
				this.gridHelper.AddColumn("OutFactory", "�����");
				this.gridHelper.AddColumn("CompaleteDate", "�깤����");
				this.gridHelper.AddColumn("StartSN", "��ʼ���к�");
				this.gridHelper.AddColumn("EndSN", "�������к�");
				this.gridHelper.AddColumn("Memo", "��ע");
				
				this.lblMOIDQuery.Visible = true;
				this.lblLotNoQuery.Visible = false;
			}
			else
			{
				this.gridHelper.AddColumn("LotNo", "Lot No.");
				this.gridHelper.AddColumn("ItemCode", "�Ϻ�");
				this.gridHelper.AddColumn("PlanQty", "�ƻ�����");
				this.gridHelper.AddColumn("Qty", "����");
				this.gridHelper.AddColumn("OutFactory", "�����");
				this.gridHelper.AddColumn("CompaleteDate", "�깤����");
				this.gridHelper.AddColumn("Memo", "��ע");
				
				this.lblMOIDQuery.Visible = false;
				this.lblLotNoQuery.Visible = true;
			}
			this.gridHelper.AddDefaultColumn( false, false );

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}
		
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			BenQGuru.eMES.Domain.OutSourcing.OutMO mo = (BenQGuru.eMES.Domain.OutSourcing.OutMO)obj;
			object[] objs = null;
			if (this.rdbType.SelectedValue == "LOT")
			{
				objs = new object[]{
									   mo.StartSN,
									   mo.ItemCode,
									   mo.PlanQty,
									   mo.Qty,
									   mo.OutFactory,
									   FormatHelper.ToDateString(mo.CompleteDate),
									   mo.Memo };
			}
			else
			{
				objs = new object[]{
									   mo.MOCode,
									   mo.ItemCode,
									   mo.PlanQty,
									   mo.Qty,
									   mo.OutFactory,
									   FormatHelper.ToDateString(mo.CompleteDate),
									   mo.StartSN,
									   mo.EndSN,
									   mo.Memo };
			}
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow(objs);
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new BenQGuru.eMES.WebQuery.OutSourcingQueryFacade(base.DataProvider);}
			return this._facade.QueryOutMO(
				this.txtConditionMo.Text,this.rdbType.SelectedValue, 
				inclusive, exclusive );
		}

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new BenQGuru.eMES.WebQuery.OutSourcingQueryFacade(base.DataProvider);}
			return this._facade.QueryOutMOCount(
				this.txtConditionMo.Text, this.rdbType.SelectedValue);
		}

		#endregion


		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			BenQGuru.eMES.Domain.OutSourcing.OutMO mo = (BenQGuru.eMES.Domain.OutSourcing.OutMO)obj;
			string[] str = null;
			if (this.rdbType.SelectedValue == "LOT")
			{
				str = new string[]{
									  mo.StartSN,
									  mo.ItemCode,
									  mo.PlanQty.ToString(),
									  mo.Qty.ToString(),
									  mo.OutFactory,
									  FormatHelper.ToDateString(mo.CompleteDate),
									  mo.Memo };
			}
			else
			{
				str = new string[]{
									  mo.MOCode,
									  mo.ItemCode,
									  mo.PlanQty.ToString(),
									  mo.Qty.ToString(),
									  mo.OutFactory,
									  FormatHelper.ToDateString(mo.CompleteDate),
									  mo.StartSN,
									  mo.EndSN,
									  mo.Memo };
			}
			return str;
		}

		protected override string[] GetColumnHeaderText()
		{
			string[] str = null;
			if (this.rdbType.SelectedValue == "PCS")
			{
				str = new string[]{
									  "MOCode",
									  "ItemCode",
									  "PlanQty",
									  "Qty",
									  "OutFactory",
									  "CompleteDate",
									  "StartSN",
									  "EndSN",
									  "Memo"};
			}
			else
			{
				str = new string[]{
									  "Lot No.",
									  "ItemCode",
									  "PlanQty",
									  "Qty",
									  "OutFactory",
									  "CompleteDate",
									  "Memo"};
			}
			return str;
		}
		#endregion

		protected void rdoType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.InitWebGrid();
		}
	}
}
