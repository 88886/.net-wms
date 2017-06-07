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
using BenQGuru.eMES.Common ;
using BenQGuru.eMES.Web.Helper ;
using BenQGuru.eMES.Web.UserControl ;

using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.SelectQuery;
using Infragistics.Web.UI.GridControls;

namespace BenQGuru.eMES.Web.SelectQuery
{
	/// <summary>
	/// FErrorCodeSP ��ժҪ˵����
	/// </summary>
	public partial class FErrorCodeSP : BaseSelectorPageNew
	{

		private BenQGuru.eMES.SelectQuery.SPFacade facade ;
		protected System.Web.UI.WebControls.DropDownList Dropdownlist1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList Dropdownlist2;
		private BenQGuru.eMES.MOModel.ModelFacade _modelFacade ;

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

		}
		#endregion

		#region Init
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}
			this.Setpostback();
		}

		#endregion

		#region WebGrid
		protected override void InitWebGrid()
		{
            base.InitWebGrid2();
            this.gridSelectedHelper.AddColumn("Selector_SelectedCode", "����������", null);
			this.gridSelectedHelper.AddColumn( "SelectedECGDesc", "��������������",	null);
			this.gridSelectedHelper.AddColumn( "SelectedECCode", "��������",	null);
			this.gridSelectedHelper.AddColumn( "SelectedECDesc", "������������",	null);
			this.gridSelectedHelper.AddDefaultColumn(true,false) ;
			this.gridSelectedHelper.ApplyLanguage( this.languageComponent1 );

            base.InitWebGrid();
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedCode", "����������", null);
			this.gridUnSelectedHelper.AddColumn( "UnSelectedECGDesc", "��������������",	null);
			this.gridUnSelectedHelper.AddColumn( "UnSelectedECCode", "��������",	null);
			this.gridUnSelectedHelper.AddColumn( "UnSelectedECDesc", "������������",	null);
			this.gridUnSelectedHelper.AddDefaultColumn(true,false) ;
			this.gridUnSelectedHelper.ApplyLanguage( this.languageComponent1 );
		}

		protected override DataRow GetSelectedGridRow(object obj)
		{
			ErrorGroup2CodeSelect item = (ErrorGroup2CodeSelect)obj;
            DataRow row = DtSourceSelected.NewRow();
            row["Selector_SelectedCode"] = item.ErrorCodeGroup;
            row["SelectedECGDesc"] = item.ErrorCodeGroupDescription;
             row["SelectedECCode"] =item.ErrorCode;
             row["SelectedECDesc"] = item.ErrorDescription;
            return row;
		}

		protected override DataRow GetUnSelectedGridRow(object obj)
		{
			ErrorGroup2CodeSelect item = (ErrorGroup2CodeSelect)obj;
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = item.ErrorCodeGroup;
            row["UnSelectedECGDesc"] = item.ErrorCodeGroupDescription;
            row["UnSelectedECCode"] =item.ErrorCode;
            row["UnSelectedECDesc"] = item.ErrorDescription;
            return row;
		}

		protected override object[] LoadSelectedDataSource(int inclusive, int exclusive)
		{
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}
			return this.facade.QuerySelectedErrorCode(this.GetSelectedCodes()) ;
		}

		protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
		{
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}
			return this.facade.QueryUnSelectedErrorCode(this.drpModelEdit.SelectedValue,this.drpErrorCodeGroupEdit.SelectedValue,string.Empty,this.GetSelectedCodes(),inclusive,exclusive) ;
		}


		protected override int GetUnSelectedRowCount()
		{
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}
			return this.facade.QueryUnSelectedErrorCodeCount(this.drpModelEdit.SelectedValue,this.drpErrorCodeGroupEdit.SelectedValue,string.Empty,this.GetSelectedCodes()) ;
		}

		protected override void AddNewRow(ArrayList rows)
		{
			foreach( GridRecord row in rows )
			{
                DataRow newrow = DtSourceSelected.NewRow();
                newrow["GUID"] = row.Items.FindItemByKey("GUID").Value;
                newrow["Selector_SelectedCode"] = row.Items.FindItemByKey("Selector_UnselectedCode").Value;
                newrow["SelectedECGDesc"] = row.Items.FindItemByKey("UnSelectedECGDesc").Value;
                newrow["SelectedECCode"] = row.Items.FindItemByKey("UnSelectedECCode").Value;
                newrow["SelectedECDesc"] = row.Items.FindItemByKey("UnSelectedECDesc").Value;
                this.DtSourceSelected.Rows.Add(newrow);
			}
            this.gridSelectedHelper.Grid.DataSource = this.DtSourceSelected;
            this.gridSelectedHelper.Grid.DataBind();
		}

		protected override void SetSelectedCodes()
		{
			string[] codes = new string[ this.gridSelectedHelper.Grid.Rows.Count ];
			for(int i=0 ; i<codes.Length ;i++)
			{
                codes[i] = this.gridSelectedHelper.Grid.Rows[i].Items.FindItemByKey("Selector_SelectedCode").Text + ":" + this.gridSelectedHelper.Grid.Rows[i].Items.FindItemByKey("SelectedECCode").Text;
			}

			Control control = this.FindControl("txtSelected") ;
			if(control == null)
			{
				return  ;
			}

			else
			{
				((System.Web.UI.HtmlControls.HtmlTextArea)control).Value = string.Join(DATA_SPLITER,codes) ;
			}
		}


        
		#endregion

		#region DropDownList Init

		protected void drpModelEdit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.drpErrorCodeGroup_Load();
		}

		#region ��Ʒ��
		//��Ʒ��
		protected void drpModelEdit_Load(object sender, System.EventArgs e)
		{
			if(this.pagePostBackCount.Value == "2")
			{
				this.drpModelEdit.Items.Clear();
				//ListItem emptyItem = new ListItem("","");
				//this.drpModelEdit.Items.Insert(0,emptyItem);
				DropDownListBuilder builder = new DropDownListBuilder(this.drpModelEdit);
				builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate(this.GetModels);
				builder.Build("ModelCode", "ModelCode");

				drpErrorCodeGroup_Load();
			}
		}
		//���ݲ�Ʒ��ȡModel(��Ʒ��)
		private object[] GetModels()
		{
			if(_modelFacade==null){_modelFacade = new BenQGuru.eMES.MOModel.ModelFacade(base.DataProvider);}
			if(this.txtOthers.Value != string.Empty)
			return this._modelFacade.QueryModelByItems(this.txtOthers.Value);

			return null;
		}

		#endregion

		#region ����������

		//����������
		private void drpErrorCodeGroup_Load()
		{
			this.drpErrorCodeGroupEdit.Items.Clear();
			DropDownListBuilder builder = new DropDownListBuilder(this.drpErrorCodeGroupEdit);
			builder.AddAllItem(languageComponent1);
			object[] ecgs = this.GetErrorGroupByModel();
			if(ecgs!=null)
			foreach(ErrorCodeGroupA ecga in ecgs)
			{
				this.drpErrorCodeGroupEdit.Items.Add(new ListItem(ecga.ErrorCodeGroup,ecga.ErrorCodeGroup));
			}
		}
		private object[] GetErrorGroupByModel()
		{
			string _modelCode = this.drpModelEdit.SelectedValue;
			BenQGuru.eMES.TSModel.TSModelFacade tsmFacade  = new BenQGuru.eMES.TSModel.TSModelFacade(base.DataProvider);
			return tsmFacade.GetErrorCodeGroupByModel(_modelCode);
		}

		private object[] GetErrorGroupByItem()
		{
			string _itemCode = string.Empty;
			BenQGuru.eMES.TSModel.TSModelFacade tsmFacade  = new BenQGuru.eMES.TSModel.TSModelFacade(base.DataProvider);
			return tsmFacade.GetErrorCodeGroupByItemCode(_itemCode);
		}
		#endregion

		#region ��������

		//��������
//		private void drpErrorCode_Load()
//		{
//			this.drpErrorCodeEdit.Items.Clear();
//			DropDownListBuilder builder = new DropDownListBuilder(this.drpErrorCodeEdit);
//			builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate(this.GetErrorCodeByGroup);
//			builder.Build("ErrorCode", "ErrorCode");
//		}
//		private object[] GetErrorCodeByGroup()
//		{
//			string _groupCode = string.Empty;
//			BenQGuru.eMES.TSModel.TSModelFacade tsmFacade  = new BenQGuru.eMES.TSModel.TSModelFacade(base.DataProvider);
//			return tsmFacade.GetErrorCodeByGroup(_groupCode);
//		}

		#endregion

		#endregion


		private void Setpostback()
		{
			if(this.pagePostBackCount.Value ==string.Empty)
			{
				this.pagePostBackCount.Value = "1";
			}
			else
			{
				int count = int.Parse(this.pagePostBackCount.Value) +1;
				this.pagePostBackCount.Value = count.ToString();
			}
		}
	}
}
