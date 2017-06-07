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
using Infragistics.WebUI.UltraWebNavigator;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.MutiLanguage;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;


namespace BenQGuru.eMES.Web.BaseSetting
{
	/// <summary>
	/// FMenuMP ��ժҪ˵����
	/// </summary>
	public partial class FMenuMP : BaseMPageNew
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		
		private BenQGuru.eMES.BaseSetting.SystemSettingFacade _facade = null ;// new SystemSettingFacadeFactory().Create();
	
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
			//this.treeMenu.NodeClicked += new Infragistics.WebUI.UltraWebNavigator.NodeClickedEventHandler(this.treeMenu_NodeClicked);

		}
		#endregion

		#region Init
		protected void Page_Load(object sender, System.EventArgs e)
		{

			if (!IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
                drpIsRestrain_Load();
				this.BuildMenuTree();
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
			this.gridHelper.AddColumn( "MenuSequence", "�˵�˳��",	null);
			this.gridHelper.AddColumn( "MenuCode", "�˵�����",	null);
			this.gridHelper.AddColumn( "ParentMenuCode", "���˵�����",	null);
			this.gridHelper.AddColumn( "ModuleCode", "ģ�����",	null);
			this.gridHelper.AddColumn( "MenuType", "�˵�����",	null);
			this.gridHelper.AddColumn( "MenuDescription", "�˵�����",	null);
			this.gridHelper.AddColumn( "Restrain", "������ʾ",	null);
			this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);

			this.gridHelper.AddDefaultColumn( true, true );

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}
		
		protected override DataRow GetGridRow(object obj)
		{
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
            //    new object[]{"false",
            //                    ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuSequence.ToString(),
            //                    ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuCode.ToString(),
            //                    ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).ParentMenuCode.ToString(),
            //                    ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).ModuleCode.ToString(),
            //                    ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuType.ToString(),
            //                    ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuDescription.ToString(),
            //                    languageComponent1.GetString(((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).Visibility),
            //                    //((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MaintainUser.ToString(),

            //          ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).GetDisplayText("MaintainUser"),
            //                    FormatHelper.ToDateString(((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MaintainDate),
            //                    FormatHelper.ToTimeString(((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MaintainTime),
            //                    ""});

            DataRow row = this.DtSource.NewRow();
            row["MenuSequence"] = ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuSequence.ToString();
            row["MenuCode"] = ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuCode.ToString();
            row["ParentMenuCode"] = ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).ParentMenuCode.ToString();
            row["ModuleCode"] = ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).ModuleCode.ToString();
            row["MenuType"] = ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuType.ToString();
            row["MenuDescription"] = ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuDescription.ToString();
            row["Restrain"] = languageComponent1.GetString(((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).Visibility);
            row["MaintainUser"] = ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MaintainTime);
            return row;

		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if( _facade == null )
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			return this._facade.GetSubMenusByMenuCode( FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMenuCodeQuery.Text)), 
											inclusive, exclusive );
		}


		protected override int GetRowCount()
		{
			if( _facade == null )
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			return this._facade.GetSubMenusByMenuCodeCount(FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMenuCodeQuery.Text)));
		}

		#endregion

		#region Button

		protected override void AddDomainObject(object domainObject)
		{
			if( _facade == null )
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			this._facade.AddMenu( (BenQGuru.eMES.Domain.BaseSetting.Menu)domainObject );
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if( _facade == null )
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			this._facade.DeleteMenu( (BenQGuru.eMES.Domain.BaseSetting.Menu[])domainObjects.ToArray( typeof(BenQGuru.eMES.Domain.BaseSetting.Menu) ) );
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			this.CheckParentMenu( this.drpParentMenuCodeEdit.SelectedValue );

			if( _facade == null )
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}	
			this._facade.UpdateMenu( (BenQGuru.eMES.Domain.BaseSetting.Menu)domainObject );
		}

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
			if ( pageAction == PageActionType.Add )
			{
				this.txtMenuCodeEdit.ReadOnly = false;
				this.BuildMenuTree();
			}

			if ( pageAction == PageActionType.Update )
			{
				this.txtMenuCodeEdit.ReadOnly = true;
			}
		}

		#endregion

		#region Object <--> Page

		protected override object GetEditObject()
		{
			if( _facade == null )
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			BenQGuru.eMES.Domain.BaseSetting.Menu menu = this._facade.CreateNewMenu();

			menu.MenuCode		= FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtMenuCodeEdit.Text, 40));
			menu.ModuleCode		= this.drpModuleCodeEdit.SelectedValue;
			menu.MenuSequence	= System.Decimal.Parse(this.txtMenuSequenceEdit.Text);
			menu.MenuType		= this.drpMenuTypeEdit.SelectedValue;
			menu.ParentMenuCode = this.drpParentMenuCodeEdit.SelectedValue;
			menu.MenuDescription = FormatHelper.CleanString(this.txtMenuDescriptionEdit.Text, 100);
			menu.Visibility = this.drpIsRestrain.SelectedValue;
			menu.MaintainUser	= this.GetUserCode();

			return menu;
		}

		protected override object GetEditObject(GridRecord row)
		{
			if( _facade == null )
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
                
			}
            object obj = _facade.GetMenu(row.Items.FindItemByKey("MenuCode").Text.ToString());
			
			if (obj != null)
			{
				return (BenQGuru.eMES.Domain.BaseSetting.Menu)obj;
			}

			return null;
		}

		protected override void SetEditObject(object obj)
		{
			if (obj == null)
			{
				this.txtMenuCodeEdit.Text	= "";
				this.txtMenuDescriptionEdit.Text	= "";
				this.drpModuleCodeEdit.SelectedIndex = 0;
				this.txtMenuSequenceEdit.Text	= "";
				this.drpIsRestrain.SelectedIndex = 0;

				try
				{
					this.drpParentMenuCodeEdit.SelectedValue = this.txtMenuCodeQuery.Text;
				}
				catch
				{
					this.drpParentMenuCodeEdit.SelectedIndex = 0;
				}

				return;
			}

			this.txtMenuCodeEdit.Text	= ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuCode.ToString();
			this.txtMenuDescriptionEdit.Text	= ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuDescription.ToString();
			this.txtMenuSequenceEdit.Text	= ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuSequence.ToString();
			
			try
			{
				this.drpMenuTypeEdit.SelectedValue = ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuType.ToString(); 
			}
			catch
			{
				this.drpMenuTypeEdit.SelectedIndex = 0;
			}

			try
			{
				this.drpModuleCodeEdit.SelectedValue	= ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).ModuleCode.ToString();
			}
			catch
			{
				this.drpModuleCodeEdit.SelectedIndex = 0;
			}
			try
			{
				this.drpParentMenuCodeEdit.SelectedValue = ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).ParentMenuCode.ToString();
			}
			catch
			{
				this.drpParentMenuCodeEdit.SelectedIndex = 0;
			}

			if (FormatHelper.StringToBoolean(((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).Visibility) == true)
				this.drpIsRestrain.SelectedIndex = 1;
			else
				this.drpIsRestrain.SelectedIndex = 0;
		}

		
		protected override bool ValidateInput()
		{
			PageCheckManager manager = new PageCheckManager();

			manager.Add( new LengthCheck(lblMenuCodeEdit, txtMenuCodeEdit, 40, true) );
			manager.Add( new NumberCheck(lblMenuSequenceEdit, txtMenuSequenceEdit, true) );
			manager.Add( new LengthCheck(lblMenuDescriptionEdit, txtMenuDescriptionEdit, 100, false) );

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
				return false;
			}
			return true;
		}

		#endregion

		#region Tree
				
		private ITreeObjectNode LoadMenuTree()
		{
			if( _facade == null )
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			return this._facade.BuildMenuTree();
		}

		/// <summary>
		/// ����Menu��
		/// </summary>
		private void BuildMenuTree()
		{	
			this.treeMenu.Nodes.Clear();
						
			ITreeObjectNode node = this.LoadMenuTree();

			this.treeMenu.Nodes.Add( BuildTreeNode(node) );

			LanguageWord lword  = this.languageComponent1.GetLanguage("menuRoot");

			if ( lword != null )
			{
				this.treeMenu.Nodes[0].Text = lword.ControlText;
			}

			//this.treeMenu.ExpandAll();
			this.treeMenu.CollapseAll();
			if (this.treeMenu.SelectedNode != null)
			{
				Infragistics.WebUI.UltraWebNavigator.Node nodeParent = this.treeMenu.SelectedNode.Parent;
				while (nodeParent != null)
				{
					nodeParent.Expand(false);
					nodeParent = nodeParent.Parent;
				}
			}
			this.BuildParenMenuCodeList();
		}

		private Infragistics.WebUI.UltraWebNavigator.Node BuildTreeNode(ITreeObjectNode treeNode)
		{
			Infragistics.WebUI.UltraWebNavigator.Node node = new Node();

			node.Text = treeNode.Text;
			node.Tag = treeNode;

			if ( treeNode.Text == this.txtMenuCodeQuery.Text )
			{
				this.treeMenu.SelectedNode = node;
			}

			foreach( ITreeObjectNode subNode in treeNode.GetSubLevelChildrenNodes() )
			{
				node.Nodes.Add( BuildTreeNode(subNode) );
			}

			return node;
		}

        protected void treeMenu_NodeSelectionChanged(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                this.txtMenuCodeQuery.Text = ((MenuTreeNode)e.Node.Tag).MenuWithUrl.MenuCode;
            }
            else
            {
                this.txtMenuCodeQuery.Text = "";
            }

            this.gridHelper.RequestData();
            this.buttonHelper.PageActionStatusHandle(PageActionType.Query);

            try
            {
                this.drpParentMenuCodeEdit.SelectedValue = this.txtMenuCodeQuery.Text;
            }
            catch
            {
                this.drpParentMenuCodeEdit.SelectedIndex = 0;
            }
        }
        /*
		private void treeMenu_NodeClicked(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
		{
			if ( e.Node.Tag != null )
			{
				this.txtMenuCodeQuery.Text =  ((MenuTreeNode)e.Node.Tag).MenuWithUrl.MenuCode;
			}
			else
			{
				this.txtMenuCodeQuery.Text = "";
			}
			
			this.gridHelper.RequestData();
			this.buttonHelper.PageActionStatusHandle( PageActionType.Query );			
			
			try
			{
				this.drpParentMenuCodeEdit.SelectedValue = this.txtMenuCodeQuery.Text;
			}
			catch
			{
				this.drpParentMenuCodeEdit.SelectedIndex = 0;
			}
		}
        */
		private bool CheckParentMenu(string parentMenuCode)
		{
			if ( parentMenuCode == "" )
			{
				return true;
			}

			ITreeObjectNode node = this.LoadMenuTree().GetTreeObjectNodeByID(this.txtMenuCodeEdit.Text);
		
			if ( node == null )
			{
				ExceptionManager.Raise(this.GetType(),"$Error_Node_Lost");
			}

			TreeObjectNodeSet set = node.GetAllNodes();

			foreach (ITreeObjectNode childNode in set)
			{
				if (childNode.ID.ToUpper() == parentMenuCode.ToUpper())
				{
					ExceptionManager.Raise(this.GetType(),"$Error_Parent_To_Children");
				}
			}	

			return true;
		}

		#endregion

		#region ���ݳ�ʼ��
		private void BuildParenMenuCodeList()
		{	
			DropDownListBuilder builder = new DropDownListBuilder( this.drpParentMenuCodeEdit );
			if( _facade == null )
			{
				_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
			}
			builder.HandleGetObjectList = new GetObjectListDelegate( this._facade.GetAllMenus );

			builder.Build("MenuCode","MenuCode");
			this.drpParentMenuCodeEdit.Items.Insert(0, "");
	
			try
			{
				this.drpParentMenuCodeEdit.SelectedValue = this.txtMenuCodeQuery.Text;
			}
			catch
			{
				this.drpParentMenuCodeEdit.SelectedIndex = 0;
			}
		}

		protected void drpModuleCodeEdit_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				DropDownListBuilder builder = new DropDownListBuilder( this.drpModuleCodeEdit );
				if( _facade == null )
				{
					_facade = new SystemSettingFacadeFactory(base.DataProvider).Create();
				}
				builder.HandleGetObjectList = new GetObjectListDelegate( this._facade.GetAllModules );

				builder.Build("ModuleCode","ModuleCode");
				this.drpModuleCodeEdit.Items.Insert(0, "");	
			}
		}

		protected void drpMenuType_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				this.drpMenuTypeEdit.Items.Clear();

				if( InternalSystemVariable.Lookup("MenuType") == null )
				{
					return;
				}
				
				foreach (string _Items in (InternalSystemVariable.Lookup("MenuType").Items))
				{
					this.drpMenuTypeEdit.Items.Add(_Items);
				}
																							
			}
		}

        public void drpIsRestrain_Load()
        {
            this.drpIsRestrain.Items.Clear();
            this.drpIsRestrain.Items.Add(new ListItem(this.languageComponent1.GetString("0"), "0"));
            this.drpIsRestrain.Items.Add(new ListItem(this.languageComponent1.GetString("1"), "1"));
            this.drpIsRestrain.SelectedIndex = 0;
        }
		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			return new string[]{   ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuSequence.ToString(),
								   ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuCode.ToString(),
								   ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).ParentMenuCode.ToString(),
								   ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).ModuleCode.ToString(),
								   ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MenuDescription.ToString(),
								   languageComponent1.GetString(((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).Visibility),
                                   //((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MaintainUser.ToString(),
                  ((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).GetDisplayText("MaintainUser"),
								   FormatHelper.ToDateString(((BenQGuru.eMES.Domain.BaseSetting.Menu)obj).MaintainDate) };
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	
									"MenuSequence",
									"MenuCode",
									//"MenuName",
									"ParentMenuCode",
									"ModuleCode",
									"MenuDescription",
									"Restrain",
									"MaintainUser",
									"MaintainDate"};
		}
		#endregion
	
	}
}
