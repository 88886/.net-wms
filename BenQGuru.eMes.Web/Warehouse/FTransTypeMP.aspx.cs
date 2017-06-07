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
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Material;

namespace BenQGuru.eMES.Web.WarehouseWeb
{
	/// <summary>
	/// FShiftMP ��ժҪ˵����
	/// </summary>
	public partial class FTransTypeMP : BaseMPage
	{
		protected System.Web.UI.WebControls.Label lblShiftTitle;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.WebControls.Label lblItemCodeQuery;
		protected System.Web.UI.WebControls.TextBox txtItemCodeQuery;
		protected System.Web.UI.WebControls.Label lblItemNameQuery;
		protected System.Web.UI.WebControls.TextBox txtItemNameQuery;
		protected System.Web.UI.WebControls.Label lblItemCodeEdit;
		protected System.Web.UI.WebControls.TextBox txtItemCodeEdit;
		protected System.Web.UI.WebControls.Label lblItemNameEdit;
		protected System.Web.UI.WebControls.TextBox txtItemNameEdit;
		protected System.Web.UI.WebControls.Label lblItemUOMEdit;
		protected System.Web.UI.WebControls.TextBox txtItemUOMEdit;
		protected System.Web.UI.WebControls.Label lblItemControlTypeEdit;
		protected System.Web.UI.WebControls.DropDownList drpItemControlTypeEdit;
		
		private BenQGuru.eMES.Material.WarehouseFacade _facade ;//= new BenQGuru.eMES.Material.WarehouseFacade();

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
			if(!Page.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				LoadDefaultTransType();
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
			this.gridHelper.AddColumn( "TransactionTypeCode", "���ݴ���",	null);
			this.gridHelper.AddColumn( "TransactionTypeName", "��������",	null);
			this.gridHelper.AddColumn( "TransactionTypeDescription", "��������",	null);
			this.gridHelper.AddColumn( "IsByMOControl", "�����ܿ�",	null);
			
			this.gridHelper.AddDefaultColumn( true, true );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}
		
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			TransactionType tt = (TransactionType)obj;
			Infragistics.WebUI.UltraWebGrid.UltraGridRow row = new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{"false",
								tt.TransactionTypeCode,
								tt.TransactionTypeName,
								tt.TransactionTypeDescription,
								GetBooleanDisplay(tt.IsByMOControl),
								""});
			tt = null;
			return row;
		}
		private Hashtable htbool = new Hashtable();
		private string GetBooleanDisplay(string value)
		{
			if (htbool[value] == null)
			{
				string strDisplay = FormatHelper.DisplayBoolean(value, this.languageComponent1);
				htbool.Add(value, strDisplay);
			}
			return htbool[value].ToString();
		}


		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			return this._facade.QueryTransactionType( 
				FormatHelper.CleanString(this.txtTransTypeCodeQuery.Text),
				FormatHelper.CleanString(this.txtTransTypeNameQuery.Text),
				inclusive, exclusive );
		}

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			return this._facade.QueryTransactionTypeCount( 
				FormatHelper.CleanString(this.txtTransTypeCodeQuery.Text),
				FormatHelper.CleanString(this.txtTransTypeNameQuery.Text));
		}

		#endregion

		#region Button
		protected override void AddDomainObject(object domainObject)
		{		
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			this._facade.AddTransactionType( (TransactionType)domainObject );
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			this._facade.DeleteTransactionType( (TransactionType[])domainObjects.ToArray( typeof(TransactionType) ) );
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			this._facade.UpdateTransactionType( (TransactionType)domainObject );
		}

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
			if ( pageAction == PageActionType.Add )
			{
				this.txtTransTypeCodeEdit.ReadOnly = false;
			}
			
			if ( pageAction == PageActionType.Update )
			{
				this.txtTransTypeCodeEdit.ReadOnly = true;
			}
		}
		#endregion

		#region Object <--> Page
		protected override object GetEditObject()
		{
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			TransactionType tt = this._facade.CreateNewTransactionType();

			tt.TransactionTypeCode	= FormatHelper.CleanString(this.txtTransTypeCodeEdit.Text, 50);
			tt.TransactionTypeName	= FormatHelper.CleanString(this.txtTransTypeNameEdit.Text, 100);
			tt.TransactionTypeDescription	= FormatHelper.CleanString(this.txtTransTypeDescriptionEdit.Text, 100);
			//tt.IsByMOControl		= FormatHelper.BooleanToString(this.chbIsByMOControl.Checked);	
			tt.IsByMOControl		= FormatHelper.BooleanToString(this.JudgeByMO(tt.TransactionTypeCode,this.chbIsByMOControl.Checked));
			tt.TransactionPrefix	= FormatHelper.PKCapitalFormat(this.txtTransTypeCategoryEdit.Text.Trim());
			tt.MaintainUser		= this.GetUserCode();

			if (tt.TransactionPrefix == string.Empty)
			{
				tt.TransactionPrefix = FormatHelper.PKCapitalFormat(GetTransTypePrefix(tt.TransactionTypeCode));
			}
			return tt;
		}

		private bool JudgeByMO(string transtypecode,bool setBool)
		{
			//��ϵͳ�ڲ��趨���¼��ֽ��׵������ͣ��ܹ����ܿصĵ�������ҲҪ��ϵͳ���趨��ȥ��
			//�����ܹ����ܿص��а����� �����ϵ��������ϵ��������ϵ�������ⵥ��
			//���в��ܹ����ܿص��а��������ƿⵥ��

			bool returnBool = setBool;
			if(transtypecode == "541" || transtypecode == "551" || transtypecode == "262" || transtypecode == "801" )
			{
				returnBool = true;
			}
			else if(transtypecode == "311")
			{
				returnBool = false;
			}
			return returnBool;
		}

		protected override object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
		{	
			if(_facade==null){_facade = new WarehouseFacade(base.DataProvider);}
			object obj = _facade.GetTransactionType( row.Cells[1].Text.ToString() );
			
			if (obj != null)
			{
				return obj as TransactionType;
			}

			return null;
		}

		protected override void SetEditObject(object obj)
		{
			if (obj == null)
			{
				this.txtTransTypeCodeEdit.Text	= "";
				this.txtTransTypeNameEdit.Text	= "";
				this.txtTransTypeDescriptionEdit.Text	= "";
				this.chbIsByMOControl.Checked	= false;
				this.txtTransTypeCategoryEdit.Text = "";

				return;
			}

			TransactionType tt = (TransactionType)obj;
			this.txtTransTypeCodeEdit.Text	= tt.TransactionTypeCode;
			this.txtTransTypeNameEdit.Text	= tt.TransactionTypeName;
			this.txtTransTypeDescriptionEdit.Text 	= tt.TransactionTypeDescription;
			this.chbIsByMOControl.Checked = FormatHelper.StringToBoolean(tt.IsByMOControl);
			this.txtTransTypeCategoryEdit.Text = tt.TransactionPrefix;

			this.txtTransTypeNameEdit.ReadOnly = (tt.IsInit == "1");
		}
		
		protected override bool ValidateInput()
		{
			PageCheckManager manager = new PageCheckManager();

			manager.Add( new LengthCheck(lblTicketCodeEdit, txtTransTypeCodeEdit, 40, true) );
			manager.Add( new LengthCheck(lblTransNameEdit, txtTransTypeNameEdit, 40, true) );
			manager.Add( new LengthCheck(lblTransTypeDescriptionEdit, txtTransTypeDescriptionEdit, 100, false) );

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
				return false;
			}
			return true;
		}

		private string GetTransTypePrefix(string transCode)
		{
			string strRet = "";
			try
			{
				string strFile = this.MapPath("TransTypeMoStock.xml");
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
				doc.Load(strFile);
				System.Xml.XmlNode node = doc.SelectSingleNode(string.Format("//TransTypePrefix/TransType[@Code='{0}']", transCode));
				if (node != null)
				{
					strRet = node.FirstChild.Value;
				}
				node = null;
				doc = null;
			}
			catch
			{}
			return strRet;
		}
		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			TransactionType tt = (TransactionType)obj;
			string[] strValue = new string[]{  tt.TransactionTypeCode,
												tt.TransactionTypeName,
												tt.TransactionTypeDescription,
												GetBooleanDisplay(tt.IsByMOControl),
											};
			tt = null;
			return strValue;
		}

		protected override string[] GetColumnHeaderText()
		{		
			// TODO: �����ֶ�ֵ��˳��ʹ֮��Grid���ж�Ӧ
			return new string[] {	
									"TransactionTypeCode",
									"TransactionTypeName",
									"TransactionTypeDescription",
									"IsByMOControl"
									};
		}
		#endregion

		#region	����Ĭ�Ͻ��׵�������

		private void LoadDefaultTransType()
		{
			this.drpTransTypeEdit.Items.Clear();
			DropDownListBuilder _builder = new DropDownListBuilder(this.drpTransTypeEdit);
			Hashtable _ht = this.GetDefaultTransType();
			if(_ht != null && _ht.Count >0)
			{
				this.drpTransTypeEdit.Items.Add(new ListItem(string.Empty,string.Empty));
				foreach(DictionaryEntry _entry in _ht)
				{
					this.drpTransTypeEdit.Items.Add(new ListItem(_entry.Value.ToString(),_entry.Key.ToString()));
				}
			}

			if(this.drpTransTypeEdit.Items.Count >0 )
			{
				this.drpTransTypeEdit.SelectedIndex = 0;
				drpTransTypeEdit_SelectedIndexChanged(null,null);
			}
		}

		#endregion

		#region	Ĭ�Ͻ��׵�����

		//��ȡĬ�Ͻ��׵���
		private Hashtable GetDefaultTransType()
		{
			Hashtable _ht = new Hashtable();
			try
			{
				string strFile = this.MapPath("TransTypeMoStock.xml");
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
				doc.Load(strFile);
				System.Xml.XmlNode node = doc.SelectSingleNode(string.Format("//TransTypeName"));
				if (node != null)
				{
					foreach(System.Xml.XmlNode _childnode in node.SelectNodes("TransType"))
					{
						_ht.Add(_childnode.Attributes["Code"].Value,_childnode.Attributes["Name"].Value);
					}
				}
				node = null;
				doc = null;
			}
			catch
			{}
			return _ht;
		}

		#endregion

		protected void drpTransTypeEdit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.txtTransTypeCodeEdit.Text = this.drpTransTypeEdit.SelectedValue;
			string transtypecode = this.drpTransTypeEdit.SelectedValue;
			this.txtTransTypeCodeEdit.Enabled = false;

			this.chbIsByMOControl.Checked = this.JudgeByMO(this.drpTransTypeEdit.SelectedValue,false);
			if(transtypecode == "541" || transtypecode == "551" || transtypecode == "262" || transtypecode == "801" || transtypecode == "311")
			{
				this.chbIsByMOControl.Enabled = false;
			}
			else
			{
				this.chbIsByMOControl.Enabled = true;
			}
			
		}

	}
}
