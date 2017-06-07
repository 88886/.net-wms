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

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.TSModel;

namespace BenQGuru.eMES.Web.TSModel
{
	/// <summary>
	/// FModel2ErrorCauseAP ��ժҪ˵����
	/// </summary>
	public partial class FItemOpCauseGroupAP : BaseMPage
	{
    
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
	
		protected System.Web.UI.WebControls.Label lblErrorGroupCode;
		protected System.Web.UI.WebControls.Label lblErrorCodeCodeQuery;
		private string _opid;
		TSModelFacade _facade;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{		
			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}

			_opid = this.Request.Params["opid"];
			_facade = new TSModelFacade(base.DataProvider);
			if(!Page.IsPostBack)
			{
				BuildErrorCauseGroup();
			}
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}
	
		//ȡ�������Ʒ���µ����в���ԭ����
		private void BuildErrorCauseGroup()
		{
			ItemFacade itemFacade = new ItemFacade(base.DataProvider);
			ItemRoute2OP ip = (ItemRoute2OP)itemFacade.GetItemRoute2Op(_opid, GlobalVariables.CurrentOrganizations.First().OrganizationID);
			Model model = new ModelFacade(base.DataProvider).GetModelByItemCode(ip.ItemCode);
			object[] ecgList = new TSModelFacade(base.DataProvider).QueryModel2ErrorCauseGroup(model.ModelCode,string.Empty,1,int.MaxValue);

			BenQGuru.eMES.TSModel.TSModelFacade fa = new TSModelFacade(base.DataProvider);
			this.drpErrorCauseGroupEdit.Items.Clear();
			if(ecgList != null)
			{
				foreach(BenQGuru.eMES.Domain.TSModel.Model2ErrorCauseGroup ecg in ecgList)
				{
					BenQGuru.eMES.Domain.TSModel.ErrorCauseGroup obj = fa.GetErrorCauseGroup(ecg.ErrorCauseGroupCode) as BenQGuru.eMES.Domain.TSModel.ErrorCauseGroup;
					
					ListItem item = new ListItem(obj.ErrorCauseGroupDescription,ecg.ErrorCauseGroupCode);
					if (!this.drpErrorCauseGroupEdit.Items.Contains(item))
						this.drpErrorCauseGroupEdit.Items.Add(item);
				}
			}
		}

		#region Not Stable
		protected override void InitWebGrid()
		{
			this.gridHelper.AddColumn( "ErrorCauseGroupCode", "����ԭ�������",	null);
			this.gridHelper.AddColumn( "ErrorCauseGroupCodeDesc", "����ԭ��������",	null);
			this.gridHelper.AddColumn( "ItemCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "RouteCode", ";�̴���",	null);
			this.gridHelper.AddColumn( "OpCode", "�������",	null);
			this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);	

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
			this.gridHelper.AddDefaultColumn( true, false );

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
            
			base.InitWebGrid();

			this.gridHelper.RequestData();
		}

		protected override void AddDomainObject(object domainObject)
		{
			_facade.AddItemRouteOp2ErrorCauseGroup((ItemRouteOp2ErrorCauseGroup)domainObject);
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			_facade.DeleteItemRouteOp2ErrorCauseGroup((ItemRouteOp2ErrorCauseGroup[])domainObjects.ToArray(typeof(ItemRouteOp2ErrorCauseGroup)));
		}

		protected override int GetRowCount()
		{			
			return _facade.QueryItemRouteOp2ErrorCauseGroupCount(this._opid,string.Empty);
		}

		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			BenQGuru.eMES.TSModel.TSModelFacade fa = new TSModelFacade(base.DataProvider);
			BenQGuru.eMES.Domain.TSModel.ItemRouteOp2ErrorCauseGroup icg = obj as ItemRouteOp2ErrorCauseGroup;
			BenQGuru.eMES.Domain.TSModel.ErrorCauseGroup obj2 = fa.GetErrorCauseGroup(icg.ErrorCauseGroupCode) as BenQGuru.eMES.Domain.TSModel.ErrorCauseGroup;
			if(obj != null)
			{
				return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
					new object[]{"false",
									icg.ErrorCauseGroupCode,
									obj2.ErrorCauseGroupDescription,
									icg.ItemCode,
									icg.RouteCode,
									icg.OpCode,
									icg.MaintainUser.ToString(),
									FormatHelper.ToDateString(icg.MaintainDate),
									FormatHelper.ToTimeString(icg.MaintainTime)
								});
			}
			else
				return null;
		}

		protected override object GetEditObject(UltraGridRow row)
		{
			if(row != null)
			{
				ItemFacade itemFacade = new ItemFacade(base.DataProvider);
				ItemRoute2OP ip = (ItemRoute2OP)itemFacade.GetItemRoute2Op(_opid, GlobalVariables.CurrentOrganizations.First().OrganizationID);
				
				ItemRouteOp2ErrorCauseGroup relation = _facade.CreateNewItemRouteOp2ErrorCauseGroup();
				relation.OpID = _opid;
				relation.ErrorCauseGroupCode = row.Cells.FromKey("ErrorCauseGroupCode").Value.ToString();
				relation.ItemCode = ip.ItemCode;
				relation.RouteCode = ip.RouteCode;
				relation.OpCode = ip.OPCode;
				relation.MaintainUser = this.GetUserCode();

				return relation;
			}

			return null;

		}
		protected override object GetEditObject()
		{
			if(this.ValidateInput())
			{
				ItemFacade itemFacade = new ItemFacade(base.DataProvider);
				ItemRoute2OP ip = (ItemRoute2OP)itemFacade.GetItemRoute2Op(_opid, GlobalVariables.CurrentOrganizations.First().OrganizationID);
				
				ItemRouteOp2ErrorCauseGroup relation = _facade.CreateNewItemRouteOp2ErrorCauseGroup();
				relation.OpID = _opid;
				relation.ErrorCauseGroupCode = this.drpErrorCauseGroupEdit.SelectedValue;
				relation.ItemCode = ip.ItemCode;
				relation.RouteCode = ip.RouteCode;
				relation.OpCode = ip.OPCode;
				relation.MaintainUser = this.GetUserCode();

				return relation;
			}
			else
				return null;
		}

		protected override bool ValidateInput()
		{
			PageCheckManager manager = new PageCheckManager();
			manager.Add( new LengthCheck(this.lblErrorCauseGroupCodeEdit,this.drpErrorCauseGroupEdit, 40, true) );
			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage,languageComponent1);
				return false;
			}
			return true;
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{		
			
			return this._facade.QueryItemRouteOp2ErrorCauseGroup(this._opid,string.Empty,1,int.MaxValue);
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FItemRouteOperationEP.aspx", new string[]{"opid"}, new string[]{this.Request.Params["opid"]}));
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
