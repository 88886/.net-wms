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
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.MOModel;

namespace BenQGuru.eMES.Web.MOModel
{
    public partial class FItem2SPCTblMP : BaseMPage
    {		
        private System.ComponentModel.IContainer components;
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected BenQGuru.eMES.Web.UserControl.eMESDate dateStartDateEdit;
		protected BenQGuru.eMES.Web.UserControl.eMESDate dateEndDateEdit;
        protected BenQGuru.eMES.MOModel.ItemFacade _facade ;//= new ItemFacade();
		private string _itemCode = "";
		private string _itemName = "";
	
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
			this.languageComponent1.LanguagePackageDir = "\\\\..";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
        #endregion

		#region Init
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}

			SessionHelper ssh = SessionHelper.Current(this.Session);
			_itemCode = (string)ssh.LoadStoredObject("spc2itemcode");
			_itemName = (string)ssh.LoadStoredObject("spc2itemname");
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}
		#endregion

        #region WebGrid
        protected override void InitWebGrid()
        {
			this.gridHelper.AddColumn("OID","OID",null);
            this.gridHelper.AddColumn( "ItemName", "��Ʒ����",	null);
            this.gridHelper.AddColumn( "TableName", "����",	null);
			this.gridHelper.AddColumn( "StartDate", "��ʼ����",	null);
			this.gridHelper.AddColumn( "EndDate", "��������",	null);
			this.gridHelper.AddColumn( "Desc", "��ע",	null);
            this.gridHelper.AddColumn( "MaintainUser", "ά����Ա",	null);
            this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
		    this.gridHelper.AddDefaultColumn( true, true );

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

			this.gridWebGrid.Columns.FromKey("OID").Hidden = true;

			this.gridHelper.RequestData();
        }
		
        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
			Item2SPCTable spc = obj as Item2SPCTable;
			if(spc != null)
				return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
																		new object[]{"false",
																					spc.OID,
																					_itemName,
																					spc.SPCTableName,
																					FormatHelper.ToDateString(spc.StartDate),
																					FormatHelper.ToDateString(spc.EndDate),
																					spc.Description,
																					spc.MaintainUser.ToString(),
																					FormatHelper.ToDateString(spc.MaintainDate)
																					});
			else
				return null;
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
			if(_facade==null){_facade = new ItemFacade(base.DataProvider);}
            return this._facade.QueryItem2SPCTable( 
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(_itemCode)),
                inclusive, exclusive );
        }


        protected override int GetRowCount()
        {
			if(_facade==null){_facade = new ItemFacade(base.DataProvider);}
            return this._facade.QueryItem2SPCTableCount( 
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(_itemCode))
                );
        }

        #endregion

		#region Button

		protected override void AddDomainObject(object domainObject)
		{		
			if(_facade==null){_facade = new ItemFacade(base.DataProvider);}
			this._facade.AddItem2SPCTable( (Item2SPCTable)domainObject );
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null){_facade = new ItemFacade(base.DataProvider);}
			this._facade.DeleteItem2SPCTable( (Item2SPCTable[])domainObjects.ToArray( typeof(Item2SPCTable) ) );
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			if(_facade==null){_facade = new ItemFacade(base.DataProvider);}
			this._facade.UpdateItem2SPCTable( (Item2SPCTable)domainObject);
		}

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
			if ( pageAction == PageActionType.Add )
			{
				//this.txtErrorCodeEdit.ReadOnly = false;
			}

			if ( pageAction == PageActionType.Update )
			{
				//this.txtErrorCodeEdit.ReadOnly = true;
			}
		}
		#endregion

        #region Object <--> Page

        protected override object GetEditObject()
        {
			if(_facade==null){_facade = new ItemFacade(base.DataProvider);}
			if(	this.ValidateInput())
			{

				Item2SPCTable spc = this._facade.CreateNewItem2SPCTable();
			
				spc.OID = this.txtOID.Text.Trim()!=String.Empty?this.txtOID.Text:Guid.NewGuid().ToString();
				spc.ItemCode = this._itemCode;
				spc.SPCTableName = this.txtTableNameEdit.Text;
				spc.StartDate = FormatHelper.TODateInt(this.dateStartDateEdit.Text);
				spc.EndDate = FormatHelper.TODateInt(this.dateEndDateEdit.Text);
				spc.Description = this.txtDescription.Text;
				spc.MaintainDate = FormatHelper.TODateInt(DateTime.Now);
				spc.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
				spc.MaintainUser = this.GetUserCode();

				return spc;
			}
			else
				return null;
        }


        protected override object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
        {	
			if(_facade==null){_facade = new ItemFacade(base.DataProvider);}
            object obj = _facade.GetItem2SPCTable(row.Cells[1].Text.ToString());
            if (obj != null)
            {
                return (Item2SPCTable)obj;
            }

            return null;
        }

        protected override void SetEditObject(object obj)
        {
			if (obj == null)
            {
				this.txtOID.Text = String.Empty;
                this.txtTableNameEdit.Text	= String.Empty;;
				this.txtItemCodeEdit.Text = this._itemName;
				this.dateStartDateEdit.Text = DateTime.Now.ToShortDateString();
				this.dateEndDateEdit.Text = "2999-12-31";
				this.txtDescription.Text = String.Empty;
                return;
            }

			Item2SPCTable spc = obj as Item2SPCTable;
			if(spc == null) return;

			this.txtOID.Text = spc.OID;
			this.txtTableNameEdit.Text	= spc.SPCTableName;
			this.txtItemCodeEdit.Text = this._itemName;
			this.dateStartDateEdit.Text = FormatHelper.ToDateString(spc.StartDate);
			this.dateEndDateEdit.Text = FormatHelper.ToDateString(spc.EndDate);
			this.txtDescription.Text = spc.Description;

        }

		
        protected override bool ValidateInput()
        {
			if(_facade==null){_facade = new ItemFacade(base.DataProvider);}
			PageCheckManager manager = new PageCheckManager();

			manager.Add( new LengthCheck(this.lblTableNameEdit,this.txtTableNameEdit,40,true));			
			manager.Add( new RangeCheck(this.lblStartDateEdit,this.dateStartDateEdit.Text,this.lblEndDateEdit,this.dateEndDateEdit.Text,true));
			//manager.Add(new TableNameUniqueCheck(this._facade,this._itemCode,this.txtTableNameEdit.Text,this.txtOID.Text));
			manager.Add(new DateUniqueCheck(this._facade,
											_itemCode,
											FormatHelper.TODateInt(this.dateStartDateEdit.Text),
											FormatHelper.TODateInt(this.dateEndDateEdit.Text),
											this.txtOID.Text));
			manager.Add(new TableNameCheck(this.txtTableNameEdit.Text));

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
				return false;
			}

			return true;
        }

        #endregion
        
        #region Export 

        protected override string[] FormatExportRecord( object obj )
        {
            return new string[]{
                                   _itemName,
                                   ((Item2SPCTable)obj).SPCTableName,
								   ((Item2SPCTable)obj).StartDate.ToString(),
								   ((Item2SPCTable)obj).EndDate.ToString(),
								   ((Item2SPCTable)obj).Description,
                                   ((Item2SPCTable)obj).MaintainUser.ToString(),
                                   FormatHelper.ToDateString(((Item2SPCTable)obj).MaintainDate)
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
           return new string[] {	"��Ʒ����",
                                    "����",
                                    "��ʼ����",
                                    "��������",
									"��ע",
									"ά����Ա",
									"ά������"};
        }
        #endregion
    }

	/// <summary>
	/// ���ͬһ����Ʒ�Ƿ����ظ��ı��� 
	/// </summary>
	class TableNameUniqueCheck
		:IPageCheck
	{
		private string _itemCode;
		private string _tblName;
		private ItemFacade _facade;
		private string _oldOID;
		public TableNameUniqueCheck(ItemFacade facade,string itemCode,string tblName,string oldOID)
		{
			_itemCode = itemCode;
			_tblName = tblName;
			_facade = facade;
			_oldOID = oldOID;
		}
		#region IPageCheck ��Ա

		public string CheckMessage
		{
			get
			{
				return "$Error_SPC_Table_Repeat";
			}
		}

		#endregion

		#region ICheck ��Ա

		public bool Check()
		{
			object[] objs = _facade.GetItem2SPCTable(_itemCode,_tblName);
			if(objs != null && objs.Length > 0 && ((Item2SPCTable)objs[0]).OID != _oldOID)
				return false;
			
			return true;
		}

		#endregion
	}

	/// <summary>
	/// ͬһ��Ʒ���������ظ�������
	/// </summary>
	class DateUniqueCheck
		:IPageCheck
	{
		private int _startDate;
		private int _endDate;
		private ItemFacade _facade;
		private string _oldOID;
		private string _itemCode;

		public DateUniqueCheck(ItemFacade facade,string itemCode,int startDate,int endDate,string oldOID)
		{
			_itemCode = itemCode;
			_startDate = startDate;
			_endDate = endDate;
			_facade = facade;
			_oldOID = oldOID;
		}
		#region IPageCheck ��Ա

		public string CheckMessage
		{
			get
			{
				return "$Error_SPC_Date_Repeat";
			}
		}

		#endregion

		#region ICheck ��Ա

		public bool Check()
		{
			object[] objs = _facade.GetItem2SPCTable(_itemCode,_startDate,_endDate);
			if(objs != null && objs.Length > 0 && ((Item2SPCTable)objs[0]).OID != _oldOID)
				return false;
			
			return true;
		}

		#endregion
	}

	/// <summary>
	/// ͬһ��Ʒ���������ظ�������
	/// </summary>
	class TableNameCheck
		:IPageCheck
	{
		private string _tableName;

		public TableNameCheck(string tableName)
		{
			_tableName = tableName;
		}
		#region IPageCheck ��Ա

		public string CheckMessage
		{
			get
			{
				return "$Error_SPC_TableName";
			}
		}

		#endregion

		#region ICheck ��Ա

		public bool Check()
		{
			System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9!@#\$%\^&\*\(\)_]+$",
																								System.Text.RegularExpressions.RegexOptions.ECMAScript);
			if(reg.IsMatch(_tableName))
				return true;
			else
				return false;
		}

		#endregion
	}
}