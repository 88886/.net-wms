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
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Security;

namespace BenQGuru.eMES.Web.BaseSetting
{
	/// <summary>
	/// FUserGroup2ModuleNewSP ��ժҪ˵����
	/// </summary>
	public partial class FUserGroup2ModuleNewSP : BaseMPage
	{
		protected System.Web.UI.WebControls.Label lblModuleSelectTitle;
		protected System.Web.UI.WebControls.Label lblModuleCodeQuery;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.WebControls.TextBox txtOperationCodeQuery;
		protected System.Web.UI.WebControls.Label lblRouteCodeEdit;
		protected System.Web.UI.WebControls.TextBox txtRouteCodeEdit;
		protected System.Web.UI.WebControls.Label lblRouteSequenceEdit;
		protected System.Web.UI.WebControls.TextBox txtRouteSequenceEdit;
		private SystemSettingFacade facade = null;
		private SecurityFacade securityFacade = null ;// new SystemSettingFacadeFactory().CreateSecurityFacade();

		#region Stable
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.pagerSizeSelector.Readonly = true;

			if(!IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.txtUserGroupCodeQuery.Text = this.GetRequestParam("usergroupcode");				
			}
			this.cmdSave.Disabled = false;
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}

		#endregion


		#region Not Stable

		protected override void InitWebGrid()
		{
			this.AddExpandColumn("expand", "");
			this.gridHelper.AddColumn("ExpandCollact",	"ExpandCollact",		null);
			this.gridHelper.AddColumn("MDLDescription1",	"ģ������",		null);
			this.gridHelper.AddColumn("ModuleCode",		"ģ�����",		null);
			this.gridHelper.AddColumn("ParentModuleCode",		"��ģ�����",		null);
			this.gridHelper.AddColumn("FormUrl",		"ҳ��URL",		null);
			
			this.gridHelper.AddCheckBoxColumn("Export",		"����",	false,	null);
			this.gridHelper.AddCheckBoxColumn("Read",		"��",	false,	null);
			this.gridHelper.AddCheckBoxColumn("Write",		"д",	false,	null);
			this.gridHelper.AddCheckBoxColumn("Delete",		"ɾ",	false,	null);
			for (int i = this.gridWebGrid.Columns.Count - 1; i >= this.gridWebGrid.Columns.Count - 4; i--)
			{
				this.gridWebGrid.Columns[i].Width = Unit.Pixel(30);
			}

			this.gridHelper.AddDefaultColumn( false, false );

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
			
			this.gridWebGrid.Columns.FromKey("expand").Hidden = true;
			this.gridWebGrid.Columns.FromKey("ExpandCollact").Hidden = true;
			this.gridWebGrid.Columns.FromKey("ParentModuleCode").Hidden = true;
			this.gridWebGrid.Columns.FromKey("Export").AllowUpdate = AllowUpdate.Yes;
			this.gridWebGrid.Columns.FromKey("Read").AllowUpdate = AllowUpdate.Yes;
			this.gridWebGrid.Columns.FromKey("Write").AllowUpdate = AllowUpdate.Yes;
			this.gridWebGrid.Columns.FromKey("Delete").AllowUpdate = AllowUpdate.Yes;

			//this.gridWebGrid.DisplayLayout.LoadOnDemand = LoadOnDemand.Manual;
			gridWebGrid.Bands.Add(new UltraGridBand());
			
			this.chbSelectAll.Visible = false;

			this.cmdQuery_Click(null, null);
			this.cmdSave.Disabled = false;
		}
		private void AddExpandColumn(string key, string text)
		{
			this.gridWebGrid.Bands[0].Columns.Add(key);
			this.gridWebGrid.Bands[0].Columns.FromKey(key).HeaderText						= text;
			this.gridWebGrid.Bands[0].Columns.FromKey(key).Width							= new System.Web.UI.WebControls.Unit(50);
			this.gridWebGrid.Bands[0].Columns.FromKey(key).Type							= Infragistics.WebUI.UltraWebGrid.ColumnType.Button;
			this.gridWebGrid.Bands[0].Columns.FromKey(key).CellButtonStyle.BackColor		= System.Drawing.Color.Transparent;
			this.gridWebGrid.Bands[0].Columns.FromKey(key).CellButtonStyle.BorderStyle		= System.Web.UI.WebControls.BorderStyle.None;
			this.gridWebGrid.Bands[0].Columns.FromKey(key).CellButtonStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;				
			this.gridWebGrid.Bands[0].Columns.FromKey(key).CellButtonStyle.Cursor			= Infragistics.WebUI.Shared.Cursors.Hand;
			this.gridWebGrid.Bands[0].Columns.FromKey(key).CellStyle.CustomRules		="BACKGROUND-POSITION: center center;Background-repeat:no-repeat";
			this.gridWebGrid.Bands[0].Columns.FromKey(key).CellStyle.BackgroundImage	= this.VirtualHostRoot + "skin/image/Expand.gif";
			
			this.gridWebGrid.Bands[0].Columns.FromKey(key).CellButtonStyle.CustomRules		="BACKGROUND-POSITION: center center;Background-repeat:no-repeat";
			this.gridWebGrid.Bands[0].Columns.FromKey(key).CellButtonStyle.BackgroundImage	= this.VirtualHostRoot + "skin/image/Expand.gif";
			
			this.gridWebGrid.Bands[0].Columns.FromKey(key).Width = Unit.Pixel(15);
		}

		private int moduleMaxLevel = -1;
		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{	
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			// ��ѯ����ģ��
			object[] objs = facade.GetModuleWithViewValueVisibility(this.GetQueryParameterModuleParent());
			
			ArrayList listMdl = new ArrayList();
			if (objs != null)
			{
				// ��ѯ�û�����е�Ȩ��
				object[] objsSelected = facade.GetSelectedModuleWithViewValueByUserGroupCode( 				
					FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
					string.Empty,
					string.Empty,
					string.Empty,
					this.GetQueryParameterModuleParent());		//this.GetRequestParam("expand")
				Hashtable htSelected = new Hashtable();
				if (objsSelected != null)
				{
					for (int i = 0; i < objsSelected.Length; i++)
					{
						ModuleWithViewValue module = (ModuleWithViewValue)objsSelected[i];
						htSelected.Add(module.ModuleCode, module);
					}
				}
				
				// ����ÿ��ģ�����ģ��
				Hashtable htMdlChild = new Hashtable();
				for (int i = 0; i < objs.Length; i++)
				{
					Module module = (Module)objs[i];
					ArrayList listChild = new ArrayList();
					if (htMdlChild.ContainsKey(module.ParentModuleCode) == true)
						listChild = (ArrayList)htMdlChild[module.ParentModuleCode];
					else
						htMdlChild.Add(module.ParentModuleCode, listChild);
					listChild.Add(module);
				}
				// ��ѯ������ģ���ģ�����
				object[] objsParent = facade.GetModuleWithChild();
				Hashtable htParent = new Hashtable();
				for (int i = 0; i < objsParent.Length; i++)
				{
					htParent.Add(((Module)objsParent[i]).ModuleCode, ((Module)objsParent[i]).ModuleCode);
				}
				// ������ͽṹ
				listMdl.AddRange(GetChildrenModule(objs, string.Empty, 1, htMdlChild));
				// ����ģ��㼶����Grid��
				AdjustGridColumnByLevel(moduleMaxLevel);
				if(securityFacade==null)
				{
					securityFacade = new SystemSettingFacadeFactory(base.DataProvider).CreateSecurityFacade();
				}
				// ������
				for (int i = 0; i < listMdl.Count; i++)
				{
					object[] objMdl = (object[])listMdl[i];
					this.AppendRow((Module)objMdl[1], Convert.ToInt32(objMdl[0]), htParent, htSelected);
				}
				// ����ѡ����
				SaveModuleRightWhenPost();
				// ����չ��/����ͼ��
				AdjustExpandCollact(listMdl, htParent);
				
			}
			return null;
		}
		private ArrayList GetChildrenModule(object[] objs, string parentCode, int level, Hashtable htMdlChild)
		{
			ArrayList list = new ArrayList();
			if (htMdlChild.ContainsKey(parentCode) == true)
			{
				ArrayList listChild = (ArrayList)htMdlChild[parentCode];
				for (int i = 0; i < listChild.Count; i++)
				{
					Module module = (Module)listChild[i];
					object[] objLevel = new object[]{level, module};
					list.Add(objLevel);
					if (level > moduleMaxLevel)
						moduleMaxLevel = level;
					list.AddRange(GetChildrenModule(objs, module.ModuleCode, level + 1, htMdlChild));
				}
			}
			return list;
		}

		private void AdjustGridColumnByLevel(int maxLevel)
		{
			for (int i = this.gridWebGrid.Columns.Count - 9; i < maxLevel; i++)
			{
				string key = "MDLDescription" + (i + 2).ToString();
				this.gridWebGrid.Bands[0].Columns.Add(key);
				this.gridWebGrid.Bands[0].Columns.FromKey(key).HeaderText = "ģ������";	
				this.gridWebGrid.Bands[0].Columns.FromKey(key).Move(this.gridWebGrid.Columns.Count - 7 - i);
			}
		}
		private void AppendRow(Module module, int level, Hashtable htParent, Hashtable htSelected)
		{
			object[] objRow = new object[this.gridWebGrid.Columns.Count];
			objRow[0] = string.Empty;
			objRow[1] = "1";
			objRow[1 + level] = module.ModuleDescription;
			objRow[this.gridWebGrid.Columns.Count - 7] = module.ModuleCode;
			objRow[this.gridWebGrid.Columns.Count - 6] = module.ParentModuleCode;
			objRow[this.gridWebGrid.Columns.Count - 5] = module.FormUrl;
			
			string strRight = "";
			if (htSelected.ContainsKey(module.ModuleCode) == true)
			{
				ModuleWithViewValue moduleSelected = (ModuleWithViewValue)htSelected[module.ModuleCode];
				strRight = moduleSelected.ViewValue;
			}
			if (this.GetModuleRight(module.ModuleCode) != null)
			{
				strRight = this.GetModuleRight(module.ModuleCode);
			}
			if (strRight != "")
			{
				objRow[this.gridWebGrid.Columns.Count - 4] = securityFacade.HasRight( strRight, RightType.Export, false ).ToString();
				objRow[this.gridWebGrid.Columns.Count - 3] = securityFacade.HasRight( strRight, RightType.Read, false ).ToString();
				objRow[this.gridWebGrid.Columns.Count - 2] = securityFacade.HasRight( strRight, RightType.Write, false ).ToString();
				objRow[this.gridWebGrid.Columns.Count - 1] = securityFacade.HasRight( strRight, RightType.Delete, false ).ToString();
			}
			
			UltraGridRow gridRow = new UltraGridRow(objRow);
			// �����Ƿ�ͻ����ʾ
			if ((this.drpModuleTypeEdit.SelectedValue != "" && module.ModuleType == this.drpModuleTypeEdit.SelectedValue) ||
				(this.txtModuleCodeQuery.Text.Trim() != "" && module.ModuleCode.StartsWith(this.txtModuleCodeQuery.Text.Trim().ToUpper()) == true) ||
				(this.txtModuleDescEdit.Text.Trim() != "" && module.ModuleDescription.StartsWith(this.txtModuleDescEdit.Text.Trim().ToUpper()) == true) ||
				(this.txtModuleFormURLQuery.Text.Trim() != "" && module.FormUrl.ToUpper().IndexOf(this.txtModuleFormURLQuery.Text.Trim().ToUpper()) >= 0))
			{
				gridRow.Style.BackColor = Color.PaleGoldenrod;
			}
			this.gridWebGrid.Rows.Add(gridRow);
		}
		private void AdjustExpandCollact(ArrayList listMdl, Hashtable htParent)
		{
			for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
			{
				this.gridWebGrid.Rows[i].ShowExpand = true;
				object[] objMdl = (object[])listMdl[i];
				Module module = (Module)objMdl[1];
				if (i < this.gridWebGrid.Rows.Count - 1)
				{
					objMdl = (object[])listMdl[i + 1];
					Module moduleNext = (Module)objMdl[1];
					if (moduleNext.ParentModuleCode == module.ModuleCode)
					{
						this.gridWebGrid.Rows[i].Cells[0].Style.BackgroundImage = this.VirtualHostRoot + "skin/image/LiftUp.gif";
						this.gridWebGrid.Rows[i].Cells[1].Text = "0";
						this.gridWebGrid.Rows[i].Expand(false);
					}
				}
				if (htParent.ContainsKey(module.ModuleCode) == false)
				{
					this.gridWebGrid.Rows[i].Cells[0].Style.BackgroundImage = this.VirtualHostRoot + "skin/image/LiftUp.gif";
					this.gridWebGrid.Rows[i].Cells[1].Text = "-1";
					this.gridWebGrid.Rows[i].ShowExpand = false;
				}
			}
		}

		protected override int GetRowCount()
		{
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			return facade.GetModuleWithViewValueVisibilityCount(this.GetQueryParameterModuleParent());
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FSecurityMP.aspx"));
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
			this.gridWebGrid.CollapseRow += new Infragistics.WebUI.UltraWebGrid.CollapseRowEventHandler(this.gridWebGrid_CollapseRow);
			this.gridWebGrid.ExpandRow += new Infragistics.WebUI.UltraWebGrid.ExpandRowEventHandler(this.gridWebGrid_ExpandRow);
			this.PreRender += new System.EventHandler(this.FUserGroup2ModuleNewSP_PreRender);

		}
		#endregion
		

		protected void drpModuleTypeEdit_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				this.drpModuleTypeEdit.Items.Clear();
				this.drpModuleTypeEdit.Items.Add("");

				if( InternalSystemVariable.Lookup("ModuleType") == null )
				{
					return;
				}
				
				foreach (string _Items in (InternalSystemVariable.Lookup("ModuleType").Items))
				{
					drpModuleTypeEdit.Items.Add(_Items);
				}
																							
			}
		}

		private string GetQueryParameterModuleParent()
		{
			bool bFlag = false;
			if (this.ViewState["QueryModuleType"] == null || this.ViewState["QueryModuleType"].ToString() != this.drpModuleTypeEdit.SelectedValue)
			{
				bFlag = true;
				 this.ViewState["QueryModuleType"] = this.drpModuleTypeEdit.SelectedValue;
			}
			if (this.ViewState["QueryModuleCode"] == null || this.ViewState["QueryModuleCode"].ToString() != this.txtModuleCodeQuery.Text.Trim().ToUpper())
			{
				bFlag = true;
				this.ViewState["QueryModuleCode"] = this.txtModuleCodeQuery.Text.Trim().ToUpper();
			}
			if (this.ViewState["QueryModuleDesc"] == null || this.ViewState["QueryModuleDesc"].ToString() != this.txtModuleDescEdit.Text.Trim().ToUpper())
			{
				bFlag = true;
				this.ViewState["QueryModuleDesc"] = this.txtModuleDescEdit.Text.Trim().ToUpper();
			}
			if (this.ViewState["QueryModuleFormURL"] == null || this.ViewState["QueryModuleFormURL"].ToString() != this.txtModuleFormURLQuery.Text.Trim().ToUpper())
			{
				bFlag = true;
				this.ViewState["QueryModuleFormURL"] = this.txtModuleFormURLQuery.Text.Trim().ToUpper();
			}
			if (this.ViewState["QueryShowExistModule"] == null || this.ViewState["QueryShowExistModule"].ToString() != this.chkShowExistModule.Checked.ToString())
			{
				bFlag = true;
				this.ViewState["QueryShowExistModule"] = this.chkShowExistModule.Checked;
			}
			if (bFlag == false)
				return this.ExpandModuleCode;
			else
			{
				if(facade==null)
				{
					facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
				}
				string strRet = facade.GetQueryParentList(this.drpModuleTypeEdit.SelectedValue, this.txtModuleCodeQuery.Text.Trim().ToUpper(), this.txtModuleDescEdit.Text.Trim().ToUpper(), this.txtModuleFormURLQuery.Text.Trim().ToUpper(), this.chkShowExistModule.Checked.ToString(), this.txtUserGroupCodeQuery.Text);
				this.ExpandModuleCode = strRet;
				return strRet;
			}
		}
		private string ExpandModuleCode
		{
			get
			{
				if (this.ViewState["ExpandModuleCode"] == null)
					return ";";
				else
					return this.ViewState["ExpandModuleCode"].ToString();
			}
			set
			{
				this.ViewState["ExpandModuleCode"] = value;
			}
		}
		private void SetModuleRight(string moduleCode, string parentModuleCode, string rightString)
		{
			Hashtable htRight = new Hashtable();
			if (this.ViewState["ModuleRightTemp"] == null)
				this.ViewState["ModuleRightTemp"] = htRight;
			else
				htRight = (Hashtable)this.ViewState["ModuleRightTemp"];
			string[] savedObj = new string[]{parentModuleCode, rightString};
			if (htRight.ContainsKey(moduleCode) == true)
				htRight[moduleCode] = savedObj;
			else if (rightString != string.Empty && rightString != "0")
				htRight.Add(moduleCode, savedObj);
		}
		private string GetModuleRight(string moduleCode)
		{
			Hashtable htRight = (Hashtable)this.ViewState["ModuleRightTemp"];
			if (htRight == null || htRight.ContainsKey(moduleCode) == false)
				return null;
			else
			{
				string[] savedObj = (string[])htRight[moduleCode];
				return savedObj[1];
			}
		}

		private void SaveModuleRightWhenPost()
		{
			// ����ģ�����޸ĵ����ݱ��浽ViewState
			if( securityFacade==null )
			{
				securityFacade = new SystemSettingFacadeFactory(base.DataProvider).CreateSecurityFacade();
			}
			for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
			{
				string strSubModule = this.gridWebGrid.Rows[i].Cells.FromKey("ModuleCode").Text;
				string strSubParentModule = this.gridWebGrid.Rows[i].Cells.FromKey("ParentModuleCode").Text;
				string viewValue = securityFacade.SpellViewValueFromRights( new bool[]{
																						  Convert.ToBoolean(this.gridWebGrid.Rows[i].Cells.FromKey("Export").Text),
																						  Convert.ToBoolean(this.gridWebGrid.Rows[i].Cells.FromKey("Read").Text),
																						  Convert.ToBoolean(this.gridWebGrid.Rows[i].Cells.FromKey("Write").Text),
																						  Convert.ToBoolean(this.gridWebGrid.Rows[i].Cells.FromKey("Delete").Text)});
				this.SetModuleRight(strSubModule, strSubParentModule, viewValue);
			}
		}
		private void gridWebGrid_ExpandRow(object sender, RowEventArgs e)
		{
			string strModule = e.Row.Cells.FromKey("ModuleCode").Text;
			SaveModuleRightWhenPost();
			ExpandModuleCode = ExpandModuleCode + strModule + ";";
			this.cmdQuery_Click(null, null);
		}

		private void gridWebGrid_CollapseRow(object sender, RowEventArgs e)
		{
			string strModule = e.Row.Cells.FromKey("ModuleCode").Text;
			SaveModuleRightWhenPost();
			ExpandModuleCode = (";" + ExpandModuleCode + ";").Replace(";" + strModule + ";", ";");
			this.cmdQuery_Click(null, null);
		}

		protected void FUserGroup2ModuleNewSP_PreRender(object sender, EventArgs e)
		{
			this.cmdSave.Disabled = false;
		}
		protected override void cmdSave_Click(object sender, EventArgs e)
		{
			SaveModuleRightWhenPost();
			
			if(facade==null)
			{
				facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			facade.UpdateSelectModuleInUserGroup(
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtUserGroupCodeQuery.Text)),
				(Hashtable)this.ViewState["ModuleRightTemp"], 
				this.GetUserCode());
		}

		
	}
}
