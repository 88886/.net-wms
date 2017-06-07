using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls; 
using System.Web.UI.HtmlControls;

using Infragistics.WebUI.UltraWebGrid ;
using Infragistics.WebUI.UltraWebNavigator ;
using Infragistics.WebUI.Shared ;

using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.SMT;
using BenQGuru.eMES.Domain.SMT;
using BenQGuru.eMES.Domain.BaseSetting ;
using BenQGuru.eMES.Domain.MOModel ;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Web.WebQuery;

namespace BenQGuru.eMES.Web.SMT
{
	/// <summary>
	/// FSMTLoadingMPSecond ��ժҪ˵����
	/// </summary>
	public class FSMTImport2  : BaseMPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdCopy;
		protected Infragistics.WebUI.UltraWebGrid.UltraWebGrid gridWebGrid;
		protected System.Web.UI.WebControls.Label lblItemCodeQuery;
		protected System.Web.UI.WebControls.Label lblMOCopySourceQuery;
		protected System.Web.UI.WebControls.Label lblStationEdit;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdGridExport;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdDelete;
		protected BenQGuru.eMES.Web.Helper.PagerSizeSelector pagerSizeSelector;
		protected BenQGuru.eMES.Web.Helper.PagerToolBar pagerToolBar;
		protected System.Web.UI.WebControls.Label lblSupplierItemEdit;
		protected System.Web.UI.WebControls.Label lblLotNOEdit;
		protected System.Web.UI.WebControls.Label lblSupplyCodeEdit;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdSave;
    
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.WebControls.DropDownList drpMOCopySourceQuery;
		protected System.Web.UI.WebControls.Label lblItemCodeEdit;
		protected System.Web.UI.WebControls.TextBox txtSupplierItemEdit;
		protected System.Web.UI.WebControls.TextBox txtLotNOEdit;
		protected System.Web.UI.WebControls.TextBox txtSupplyCodeEdit;
		protected System.Web.UI.WebControls.Label lblDateCodeEdit;
		protected System.Web.UI.WebControls.TextBox txtDateCodeEdit;
		protected System.Web.UI.WebControls.Label lblPCBAEdit;
		protected System.Web.UI.WebControls.TextBox txtPCBAEdit;
		protected System.Web.UI.WebControls.Label lblBIOSEdit;
		protected System.Web.UI.WebControls.TextBox txtBIOSEdit;
		protected Infragistics.WebUI.UltraWebNavigator.UltraWebTree treeWebTree;
		protected System.Web.UI.WebControls.CheckBox chbSelectAll;
		protected System.Web.UI.WebControls.CheckBox chbifImportCheck;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbSupplierItemEdit;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbLotNOEdit;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbSupplyCodeEdit;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbDateCodeEdit;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbPCBAEdit;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbBIOSEdit;
		protected System.Web.UI.WebControls.Label lblVersionEdit;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbVersionEdit;
		protected System.Web.UI.WebControls.TextBox txtVersionEdit;
		protected System.Web.UI.WebControls.DropDownList drpMOCode;
		protected System.Web.UI.WebControls.Label lblFeederEdit;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbFeederEdit;
		protected System.Web.UI.WebControls.Label lblSapOpcode;
		protected System.Web.UI.WebControls.TextBox txtSapOPCode;

		private BenQGuru.eMES.SMT.SMTFacade _facade ;//= new SMTFacadeFactory().Create();
		protected System.Web.UI.WebControls.TextBox txtOperationCode;
		protected System.Web.UI.WebControls.TextBox txtResourceCode;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdCancel;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbExportByRes;
		protected System.Web.UI.WebControls.TextBox txtItemCodeQuery;
		protected System.Web.UI.WebControls.Label lblTitle;
		protected System.Web.UI.WebControls.TextBox Textbox3;
		protected System.Web.UI.WebControls.TextBox Textbox5;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.HtmlControls.HtmlInputFile fileExcel;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox txtItemCode;
		protected System.Web.UI.HtmlControls.HtmlInputFile FileMOItem;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox CheckboxBOM;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnImport;
		protected System.Web.UI.WebControls.TextBox txtItemCodeEdit;
		protected System.Web.UI.WebControls.TextBox txtStationEdit;
		protected System.Web.UI.WebControls.TextBox txtRouteCode;
		protected System.Web.UI.WebControls.TextBox txtFeederEdit;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList drpResource;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdMOClose;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdImport;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidifImportUse;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chbIfContainFeeder;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdReFlesh;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdRelease;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidtxtMOCode;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdchangeMO;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdFreshTree;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdCompare;
		protected System.Web.UI.WebControls.Label lblVisibleStyle;
		protected System.Web.UI.WebControls.RadioButtonList rblMOBOMSourceSelect;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidComparesource;
		protected System.Web.UI.WebControls.Label lblMoQuery;
		protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtMoQuery;

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
			this.drpMOCopySourceQuery.SelectedIndexChanged += new System.EventHandler(this.drpMOCopySourceQuery_SelectedIndexChanged);
			this.rblMOBOMSourceSelect.SelectedIndexChanged += new System.EventHandler(this.rblMOBOMSourceSelect_SelectedIndexChanged);
			this.treeWebTree.Load += new System.EventHandler(this.treeWebTree_Load);
			this.treeWebTree.NodeClicked += new Infragistics.WebUI.UltraWebNavigator.NodeClickedEventHandler(this.treeWebTree_NodeClicked);
			this.cmdCopy.ServerClick += new System.EventHandler(this.cmdCopy_ServerClick);
			this.cmdImport.ServerClick += new System.EventHandler(this.cmdImport_ServerClick);
			this.cmdCompare.ServerClick += new System.EventHandler(this.cmdCompare_ServerClick);
			this.cmdMOClose.ServerClick += new System.EventHandler(this.cmdMOClose_ServerClick);
			this.cmdRelease.ServerClick += new System.EventHandler(this.cmdRelease_ServerClick);
			this.cmdchangeMO.ServerClick += new System.EventHandler(this.cmdchangeMO_ServerClick);
			this.cmdFreshTree.ServerClick += new System.EventHandler(this.cmdFreshTree_ServerClick);
			this.Load += new System.EventHandler(this.Page_Load);

		}


		#region Init
		private void Page_Load(object sender, System.EventArgs e)
		{
			string strMessage = this.languageComponent1.GetString("$Message_SMTLoading_DataOverwritten");

			this.cmdCopy.Attributes.Add("onclick","return CheckIfCopy();" ) ;
			this.cmdImport.Attributes.Add("onclick","return CheckImport();" ) ;
			this.cmdCompare.Attributes.Add("onclick","return CheckCompare();" ) ;
			string deleteWord = languageComponent1.GetString("deleteConfirm");
			//this.cmdPending.Attributes["onclick"] = "return popSelectrPage();";
			this.cmdRelease.Attributes["onclick"] = "{ return confirm('" + deleteWord + "'); }";
			this.txtMoQuery.Tag = "false";  //����ѡ��״̬��������
			InitHanders();

			RadioButtonListBuilder builder1 = new RadioButtonListBuilder(new CompareSourceType(),this.rblMOBOMSourceSelect,this.languageComponent1);
			if( !this.IsPostBack )
			{
				builder1.Build();
			}

			RadioButtonListBuilder.FormatListControlStyle( this.rblMOBOMSourceSelect,60 );
		}

		private void InitHanders()
		{
			this.excelExporter.LoadExportDataNoPageHandle = new LoadExportDataDelegateNoPage( LoadExportData ) ;
			this.excelExporter.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatExportRecord);
			this.excelExporter.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetColumnHeaderText);
			this.excelExporter.CellSplit = ",";
			this.excelExporter.FileExtension = "csv";
			this.excelExporter.RowSplit = "\r\n" ;
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}

		//��ȡѡ��ĵ�һ�Ź���
		private string GetFirstMOCode()
		{
			string returnMOCode = string.Empty;
			if(this.txtMoQuery.Text != string.Empty)
			{
				returnMOCode = this.txtMoQuery.Text.Split(',')[0].ToString();
			}
			return returnMOCode;
		}


		#endregion

		#region WebGrid
		protected override void InitWebGrid()
		{
			this.gridHelper.AddColumn( "ResourceCode1", "��̨",	null);
			this.gridHelper.AddColumn( "StationCode", "վλ",	null);
			this.gridHelper.AddColumn( "FeederCode", "�ϼܹ�����",	null);
			this.gridHelper.AddColumn( "MaterialItemCode", "�Ϻ�",	null);
			this.gridHelper.AddColumn( "Version", "���ϰ汾",	null);
			this.gridHelper.AddColumn( "LotNO", "��������",	null);
			this.gridHelper.AddColumn( "DateCode", "��������",	null);
			this.gridHelper.AddColumn( "BIOSVersion", "BIOS�汾",	null);
			this.gridHelper.AddColumn( "PCBAVersion", "PCBA�汾",	null);
			this.gridHelper.AddColumn( "VendorCode1", "����",	null);
			this.gridHelper.AddColumn( "VenderItemCode", "�����Ϻ�",	null);
			
			this.gridHelper.AddColumn( "ItemCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "MOCode", "��������",	null);
			this.gridHelper.AddColumn( "RouteCode", ";�̴���",	null);
			this.gridHelper.AddColumn( "OPCode", "�������",	null);

			this.gridHelper.AddDefaultColumn( true, true );

			//this.gridWebGrid.Columns.FromKey("ResourceCode1").Hidden = true ;	
			this.gridWebGrid.Columns.FromKey("ItemCode").Hidden = true ;	
			this.gridWebGrid.Columns.FromKey("MOCode").Hidden = true ;	
			this.gridWebGrid.Columns.FromKey("RouteCode").Hidden = true ;	
			this.gridWebGrid.Columns.FromKey("OPCode").Hidden = true ;	
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}
		
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{   "false",
								((SMTResourceBOM)obj).ResourceCode.ToString(),
								((SMTResourceBOM)obj).StationCode.ToString(),
								((SMTResourceBOM)obj).FeederCode.ToString(),
								((SMTResourceBOM)obj).OPBOMItemCode.ToString(),
								((SMTResourceBOM)obj).Version.ToString(),
								((SMTResourceBOM)obj).LotNO.ToString(),
								((SMTResourceBOM)obj).DateCode.ToString(),
								((SMTResourceBOM)obj).BIOS.ToString(),
								((SMTResourceBOM)obj).PCBA.ToString(),
								((SMTResourceBOM)obj).VendorCode.ToString(),
								((SMTResourceBOM)obj).VenderItemCode.ToString(),
								((SMTResourceBOM)obj).ItemCode.ToString(),
								((SMTResourceBOM)obj).MOCode.ToString(),
								((SMTResourceBOM)obj).RouteCode.ToString(),
								((SMTResourceBOM)obj).OPCode.ToString(),
								""});
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			if(this.ViewState["Export"]!=null && this.ViewState["Export"].ToString()=="True")
			{
				this.ViewState["Export"] = "False";
				return this.LoadExportData();
			}
			if(this.treeWebTree.SelectedNode!=null && this.treeWebTree.SelectedNode.Text != string.Empty )
			{

				//ͨ����ǰά�������ͻ�̨��ѯSMTResourceBOM (��ǰά���Ĺ���Ϊѡ��ĵ�һ�Ź���)
				return this._facade.QuerySMTResourceBOM(string.Empty , this.GetFirstMOCode() , string.Empty, string.Empty, this.treeWebTree.SelectedNode.Text , string.Empty,inclusive,exclusive) ;
			}
			else
			{
				return null ;
			}
		}

		private void cmdRelease_ServerClick(object sender, System.EventArgs e)
		{
			ArrayList array = this.gridHelper.GetCheckedRows();
			object obj = null;

			if ( array.Count > 0 )
			{
				ArrayList objList = new ArrayList( array.Count );
			
				foreach (UltraGridRow row in array)
				{
					obj = this.GetEditObject(row);

					if ( obj != null )
					{
						objList.Add( obj );
					}
				}

				this.DeleteDomainObjects( objList );

				this.gridHelper.RequestData();
				this.buttonHelper.PageActionStatusHandle( PageActionType.Delete );
				this.cmdFreshTree_ServerClick(null,null);
			}
		}

		#region ѡ���̨����

		//�������
		private void ExportCheck()
		{
			//ָ����̨�������ж�ѡ���TreeNode
			if(this.treeWebTree.CheckedNodes.Count == 0)
			{
				ExceptionManager.Raise( this.GetType() , "$Error_Resource_SelectedNULL") ;// "û��ѡ���̨����ѡ���̨"
			}
			#region ע����ʵ��
//			if(this.chbExportByRes.Checked)
//			{
//				//ָ����̨�������ж�ѡ���TreeNode
//				if(this.treeWebTree.CheckedNodes.Count == 0)
//				{
//					ExceptionManager.Raise( this.GetType() , "$Error_Resource_SelectedNULL") ;// "û��ѡ���̨����ѡ���̨"
//				}
//
//			}
//			else
//			{
//				//�������л�̨��ResourceBOM���ж��Ƿ��л�̨
//				if(!(this.treeWebTree.Nodes.Count>0))
//				{
//					ExceptionManager.Raise( this.GetType() , "$Error_Resource_NULL") ;	// "û�п��Ե����Ļ�̨"
//				}
//			}
			#endregion
		}

		//��ȡ�����Ļ�̨����
		private ArrayList GetExportResource()
		{
			ArrayList ResourceCode = new ArrayList();
			ArrayList selectNodes = new ArrayList(this.treeWebTree.Nodes.Count);
			//ָ����̨�������ж�ѡ���TreeNode
			selectNodes = this.treeWebTree.CheckedNodes;
			#region ע�����߼� 
//			if(this.chbExportByRes.Checked)
//			{
//				//ָ����̨�������ж�ѡ���TreeNode
//				selectNodes = this.treeWebTree.CheckedNodes;
//			}
//			else
//			{
//				
//				selectNodes.AddRange(this.treeWebTree.Nodes);
//			}
			#endregion
			foreach(Node resourceNode in selectNodes)
			{
				ResourceCode.Add(resourceNode.Text);
			}
			return ResourceCode;
		}

		#endregion

		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			if( this.GetFirstMOCode() != string.Empty )
			{
				return this._facade.QuerySMTResourceBOMCount(string.Empty , this.GetFirstMOCode() , string.Empty, string.Empty, this.treeWebTree.SelectedNode.Text , string.Empty) ;
			}
			else
			{
				return 0 ;
			}
		}

		#endregion

		#region Button
		protected override void AddDomainObject(object domainObject)
		{		
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			this._facade.AddSMTResourceBOM( (SMTResourceBOM)domainObject );
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			this._facade.DeleteSMTResourceBOM( (SMTResourceBOM[])domainObjects.ToArray( typeof(SMTResourceBOM) ) );

			this.gridHelper.RequestData();
			this.buttonHelper.PageActionStatusHandle( PageActionType.Delete );
			this.cmdFreshTree_ServerClick(null,null);
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			this._facade.UpdateSMTResourceBOM( (SMTResourceBOM)domainObject );
		}

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
			if ( pageAction == PageActionType.Add )
			{
				//this.drpStationEdit.Enabled = true ;
				this.clearEditArea() ;

			}

			if ( pageAction == PageActionType.Update )
			{
				//this.drpStationEdit.Enabled = false ;
			}

			if( pageAction == PageActionType.Save )
			{
				this.clearEditArea() ;
			}
		}
		#endregion

		#region Object <--> Page

		protected override object GetEditObject()
		{
			if( !this.ValidateInput() )
			{
				return null ;
			}

			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			SMTResourceBOM bom = this._facade.CreateNewSMTResourceBOM() ;
			if(this.chbBIOSEdit.Checked)
			{
				bom.BIOS = FormatHelper.CleanString(this.txtBIOSEdit.Text) ;
			}
			else
			{
				bom.BIOS = string.Empty ;
			}

			if(this.chbDateCodeEdit.Checked)
			{
				bom.DateCode = FormatHelper.CleanString(this.txtDateCodeEdit.Text) ;
			}

			bom.FeederCode = FormatHelper.CleanString(this.txtFeederEdit.Text) ;

			bom.ItemCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)) ;
            
			bom.OPBOMItemCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeEdit.Text)) ;

			if(this.chbLotNOEdit.Checked)
			{
				bom.LotNO = FormatHelper.CleanString(this.txtLotNOEdit.Text) ;
			}

			bom.MOCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.GetFirstMOCode())) ;
			//bom.MOCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpMOCode.SelectedValue)) ;

			bom.OPCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtOperationCode.Text)) ;

			if(this.chbPCBAEdit.Checked)
			{
				bom.PCBA = FormatHelper.CleanString(this.txtPCBAEdit.Text) ;
			}

            
			bom.ResourceCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtResourceCode.Text)) ;
			bom.RouteCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtRouteCode.Text)) ;
			bom.StationCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtStationEdit.Text)) ;
            
			if(this.chbSupplierItemEdit.Checked)
			{
				bom.VenderItemCode  = FormatHelper.CleanString(this.txtSupplierItemEdit.Text) ; 
			}

			if(this.chbSupplyCodeEdit.Checked)
			{
				bom.VendorCode = FormatHelper.CleanString(this.txtSupplyCodeEdit.Text) ;
			}

			if(this.chbVersionEdit.Checked)
			{
				bom.Version = FormatHelper.CleanString(this.txtVersionEdit.Text) ;
			}

			BenQGuru.eMES.MOModel.MOFacade moFacade =  new SMTFacadeFactory(base.DataProvider).CreateMOFacade() ;
			MO2Route mo2route = (MO2Route) moFacade.GetMONormalRouteByMOCode(this.drpMOCopySourceQuery.SelectedValue) ;
			if(mo2route != null)
			{
				bom.OPBOMCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(mo2route.OPBOMCode)) ;
				bom.OPBOMVersion = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(mo2route.OPBOMVersion)) ;
			}
            
			bom.MaintainUser = this.GetUserCode() ;

			return bom;
		}


		protected override object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
		{	
			#region  ��ǰ�ķ���
//			object obj = _facade.GetSMTResourceBOM(
//				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("ItemCode").Text)),
//				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("MOCode").Text)) ,
//				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("RouteCode").Text)),
//				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("OPCode").Text)),
//				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("ResourceCode1").Text)) ,
//				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("StationCode").Text)),
//				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("MaterialItemCode").Text)));
			#endregion
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			object obj = _facade.GetSMTResourceBOM(
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("ItemCode").Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("MOCode").Text)) ,
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("ResourceCode1").Text)) ,
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("StationCode").Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(row.Cells.FromKey("MaterialItemCode").Text)));

			if (obj != null)
			{
				return (SMTResourceBOM)obj;
			}

			return null;
		}

		protected override void SetEditObject(object obj)
		{
			if(obj != null)
			{

				this.txtBIOSEdit.Text = ((SMTResourceBOM)obj).BIOS ;
				this.txtDateCodeEdit.Text = ((SMTResourceBOM)obj).DateCode ;
				this.txtLotNOEdit.Text = ((SMTResourceBOM)obj).LotNO ;
				this.txtPCBAEdit.Text = ((SMTResourceBOM)obj).PCBA ;
				this.txtSupplierItemEdit.Text = ((SMTResourceBOM)obj).VenderItemCode ;
				this.txtSupplyCodeEdit.Text = ((SMTResourceBOM)obj).VendorCode ;
				this.txtVersionEdit.Text = ((SMTResourceBOM)obj).Version ;

				this.txtStationEdit.Text = ((SMTResourceBOM)obj).StationCode ;
				this.txtItemCodeEdit.Text = ((SMTResourceBOM)obj).OPBOMItemCode ;
				this.txtFeederEdit.Text = ((SMTResourceBOM)obj).FeederCode ;
				
				this.chbBIOSEdit.Checked = !(this.txtBIOSEdit.Text == "" ) ;                
				this.chbDateCodeEdit.Checked = !(this.txtDateCodeEdit.Text == "" ) ;
				this.chbLotNOEdit.Checked = !(this.txtLotNOEdit.Text == "" ) ;
				this.chbPCBAEdit.Checked = !(this.txtPCBAEdit.Text == "") ;
				this.chbSupplierItemEdit.Checked = !(this.txtSupplierItemEdit.Text == "") ;
				this.chbSupplyCodeEdit.Checked = !(this.txtSupplyCodeEdit.Text == "") ;
				this.chbVersionEdit.Checked = !(this.txtVersionEdit.Text == "") ;
				this.chbFeederEdit.Checked = !(this.txtFeederEdit.Text == "") ;

				this.txtOperationCode.Text = ((SMTResourceBOM)obj).OPCode ;
				this.txtResourceCode.Text = ((SMTResourceBOM)obj).ResourceCode ;
				this.txtItemCodeQuery.Text = ((SMTResourceBOM)obj).ItemCode ;
				this.txtRouteCode.Text = ((SMTResourceBOM)obj).RouteCode ;

			}
		}

		
		protected override bool ValidateInput()
		{

			PageCheckManager manager = new PageCheckManager();


			if(this.chbBIOSEdit.Checked)
			{
				manager.Add( new LengthCheck(lblBIOSEdit, txtBIOSEdit, 40, true) );
			}

			if(this.chbDateCodeEdit.Checked)
			{
				manager.Add( new LengthCheck(lblDateCodeEdit, txtDateCodeEdit, 40, true) );
			}

			if(this.chbFeederEdit.Checked)
			{

			}
            
			if(this.chbLotNOEdit.Checked)
			{
				manager.Add( new LengthCheck(lblLotNOEdit, txtLotNOEdit, 40, true) );
			}

			if(this.chbPCBAEdit.Checked)
			{
				manager.Add( new LengthCheck(lblPCBAEdit, txtPCBAEdit, 40, true) );
			}

			if(this.chbSupplierItemEdit.Checked)
			{
				manager.Add( new LengthCheck(lblSupplierItemEdit, txtSupplierItemEdit, 40, true) );
			}

			if(this.chbSupplyCodeEdit.Checked)
			{
				manager.Add( new LengthCheck(lblSupplyCodeEdit, txtSupplyCodeEdit, 40, true) );
			}

			if(this.chbVersionEdit.Checked)
			{
				manager.Add( new LengthCheck(lblVersionEdit, txtVersionEdit, 40, true) );
			}

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
				return false;
			}

			return true ;


		}

		#endregion

		#region Export

		protected override string[] FormatExportRecord( object obj )
		{
			if(this.chbIfContainFeeder.Checked)
			{
				//this.drpMOCode.SelectedValue,
				//�������� ��Feeder��λ
				return new string[]{  
									   this.GetFirstMOCode(),
									   ((SMTResourceBOM)obj).ResourceCode.ToString(),
									   ((SMTResourceBOM)obj).StationCode.ToString(),
									   ((SMTResourceBOM)obj).FeederCode.ToString(),
									   ((SMTResourceBOM)obj).OPBOMItemCode.ToString()
								   };
			}
			else
			{
				//this.drpMOCode.SelectedValue,
				//�������� û��Feeder��λ
				return new string[]{   this.GetFirstMOCode(),
									   ((SMTResourceBOM)obj).ResourceCode.ToString(),
									   ((SMTResourceBOM)obj).StationCode.ToString(),
									   ((SMTResourceBOM)obj).OPBOMItemCode.ToString()
								   };
			}
		}


		protected override string[] GetColumnHeaderText()
		{
			if(this.chbIfContainFeeder.Checked)
			{
				//�������� ��Feeder��λ
				return new string[] {   "MOCode",
										"ResourceCode1",
										"StationCode",
										"FeederCode",
										"MaterialItemCode"
									};
			}
			else
			{
				//�������� û��Feeder��λ
				return new string[] {   "MOCode",
										"ResourceCode1",
										"StationCode",
										"MaterialItemCode"
									};
			}
		}


		#endregion

		#region �ؼ���ʼ���¼�

		private void drpMOCode_Load(object sender, System.EventArgs e)
		{
			if(! this.IsPostBack)
			{
				DropDownListBuilder builder = new DropDownListBuilder(this.drpMOCode);
				builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate( this.GetMOByStatus );

				builder.Build("MOCode", "MOCode");

				this.drpMOCode.Items.Insert(0, "" );
			}
		}


		//��Դ�������أ�ע�⣺ ��Ӧ�ð�����ǰ�Ĺ�����
		private void drpMOCopySourceQuery_Load(object sender, System.EventArgs e)
		{
			if(! this.IsPostBack) //�˴��ڵ�ǰ�����ı��ʱ�����¼��أ����򲻼��أ��˼��ط���Ӧ�ö�������������
			{
				this.P_drpMOCopySourceQuery_Load();
			}
		}

		//������Դ��������������ǰ��ά������,����Դ������Ӧ�Ĳ�Ʒ�Ϻ���ͬ��
		private void P_drpMOCopySourceQuery_Load()
		{
			this.drpMOCopySourceQuery.Items.Clear();		//����ǰ���
			DropDownListBuilder builder = new DropDownListBuilder(this.drpMOCopySourceQuery);
			builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate( this.GetSourceItemCode);

			builder.Build("MOCode", "MOCode");

			this.drpMOCopySourceQuery.Items.Insert(0, "" );
//			if(this.drpMOCode.SelectedValue != "")
//			{
//				this.drpMOCopySourceQuery.Items.Remove(this.drpMOCode.SelectedValue);	//�Ƴ���ǰ�Ĺ���(����Ϊ��ǰ�Ĺ���Code)
//			}
			if(this.GetFirstMOCode() != "")
			{
				this.drpMOCopySourceQuery.Items.Remove(this.GetFirstMOCode());	//�Ƴ���ǰ�Ĺ���(����Ϊ��ǰ�Ĺ���Code)
			}
			this.F_drpResourceQuery_Load();
		}


		//��̨���νṹ����
		public void treeWebTree_Load(object sender, System.EventArgs e)
		{
			//���ݴ�ά���������ػ�̨����Դ��
			//if(this.drpMOCode.SelectedValue != string.Empty)
			if(this.GetFirstMOCode() != string.Empty)
			{
				this.treeWebTree.CheckBoxes = true;
			}
		}

			
		private void drpResourceQuery_Load(object sender, System.EventArgs e)
		{
			if( ! this.IsPostBack )		
			{
				this.F_drpResourceQuery_Load();
			}
		}


		private void F_drpResourceQuery_Load()
		{
			//��̨Ӧ�ø�����Դ�������أ���������л�̨��ѡ��
			this.drpResource.Items.Clear() ;
			if(this.drpMOCopySourceQuery.SelectedValue == string.Empty)return;
			object[]  MOResources  = new SMTFacadeFactory(base.DataProvider).CreateBaseModelFacadeFacade().GetSMTResourceByMoCode(this.drpMOCopySourceQuery.SelectedValue);
			if( MOResources != null )
			{
				foreach( Resource jitai in MOResources)
				{
					this.drpResource.Items.Add( jitai.ResourceCode ) ;
				}

				new DropDownListBuilder( this.drpResource ).AddAllItem( this.languageComponent1 ) ;
			}
		}


		#region	˽�з���
		private object[] GetSourceItemCode()
		{
			return new SMTFacadeFactory(base.DataProvider).CreateMOFacade().GetMoByItemCode(this.txtItemCode.Text,new string[]{ MOManufactureStatus.MOSTATUS_RELEASE, MOManufactureStatus.MOSTATUS_OPEN ,MOManufactureStatus.MOSTATUS_CLOSE});
		}
		
		private object[] GetMOByStatus()
		{
			return new SMTFacadeFactory(base.DataProvider).CreateMOFacade().GetMOByStatus(this.GetMOStatusList());
		}

		//��ȡ������ɸѡ������release��open
		private string[] GetMOStatusList()
		{
			return new string[]{ MOManufactureStatus.MOSTATUS_RELEASE, MOManufactureStatus.MOSTATUS_OPEN };
			
		}

		#endregion

		#endregion

		#region �ؼ��ı��¼�,�ؼ�����¼�

		private void cmdReFlesh_ServerClick(object sender, System.EventArgs e)
		{
			//ˢ��ҳ��
			Session.Remove("HT");
			//���¼��ػ�̨���οؼ�
			this.BuildRouteTree() ;
			this.Alert("�������");
		}

		private void treeWebTree_NodeClicked(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
		{
			//���ݻ�̨����Grid��ϸ
			DoTreeNodeClick(true) ;
		}


		private void DoTreeNodeClick(bool isRealClick)
		{
			Node node = this.treeWebTree.SelectedNode ;
			if( node != null )
			{
				// �г���ǰmo��BOM��Ϣ
				this.gridHelper.RequestData();
				this.buttonHelper.PageActionStatusHandle( PageActionType.Query );

				this.clearEditArea() ;
			}
		}


		//��ά������ѡ��ı�
		private void drpMOCode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			//���ò�Ʒ��ʾ
			MO currentMO = (new SMTFacadeFactory(base.DataProvider).CreateMOFacade().GetMO(this.GetFirstMOCode()) as MO);
			if(currentMO!=null)
			{
				this.hidtxtMOCode.Value = currentMO.MOCode; //��ͬ��
				this.txtItemCode.Text = currentMO.ItemCode;
				
				//���¼�����Դ����
				P_drpMOCopySourceQuery_Load();

				//���¼��ػ�̨���οؼ�
				this.BuildRouteTree() ;	
			}
			else
			{
				this.ExecuteClientFunction("popSelectrPage","");
			}
		}


		//��Դ���ݸı��¼�
		private void drpMOCopySourceQuery_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//������Դ�������¼��ض�Ӧ��̨
			this.F_drpResourceQuery_Load();
		}


		private void BuildRouteTree()
		{
			this.gridHelper.Grid.Rows.Clear() ;
			string moCode = this.GetFirstMOCode() ;

			if(moCode != string.Empty)
			{
				object[]  MOResources  = new SMTFacadeFactory(base.DataProvider).CreateBaseModelFacadeFacade().GetSMTResourceByMoCode(moCode);
				this.treeWebTree.Nodes.Clear() ;
				if(MOResources!=null)
				{
					Node node=null;
					foreach(object jitai in MOResources)
					{
						node = new Node();
						node.Text = (jitai as Resource).ResourceCode;
						node.Tag = (jitai as Resource).ResourceCode;
						this.treeWebTree.Nodes.Add( node );
					}
				}

				this.treeWebTree.ExpandAll() ;
				foreach(Node node in treeWebTree.Nodes)
				{
					node.Checked = true;
				}
			}
			else
			{
				this.treeWebTree.Nodes.Clear() ;
				this.txtItemCode.Text = string.Empty ;
				this.txtResourceCode.Text = string.Empty ;
				this.txtItemCodeQuery.Text = string.Empty ;
			}

			this.clearEditArea() ;
			this.ClickDefaultNode();
		}


		private void ClickDefaultNode()
		{
			//��ȡ��̨����һ����㣬ִ��nodeClick�¼�
			if(this.treeWebTree.Nodes.Count >0 )
			{
				this.treeWebTree.SelectedNode  = this.treeWebTree.Nodes[0];
				this.treeWebTree_NodeClicked(null,null);
			}
		}


		private void clearEditArea()
		{
			this.txtBIOSEdit.Text = "" ;
			this.txtDateCodeEdit.Text = "" ;
			this.txtLotNOEdit.Text = "" ;
			this.txtPCBAEdit.Text = "" ;
			this.txtSupplierItemEdit.Text = "" ;
			this.txtSupplyCodeEdit.Text = "" ;
			this.txtVersionEdit.Text = "" ;

			this.txtStationEdit.Text = "" ;
			this.txtItemCodeEdit.Text = "" ;
			this.txtFeederEdit.Text = "";

			this.chbBIOSEdit.Checked = false ;
			this.chbDateCodeEdit.Checked = false ;
			this.chbLotNOEdit.Checked = false ;
			this.chbPCBAEdit.Checked = false ;
			this.chbSupplierItemEdit.Checked = false ;
			this.chbSupplyCodeEdit.Checked = false ;
			this.chbVersionEdit.Checked = false ;
			this.chbFeederEdit.Checked = false ;
		}


		//���Ʋ���
		//��Ҫ����ѡ��Ļ�̨���и���,ע�⣺��̨������ȫ��������Դ���������л�̨�ķ������ϣ�
		private void cmdCopy_ServerClick(object sender, System.EventArgs e)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			if(this.GetFirstMOCode()== string.Empty)
			{
				this.Alert(this.languageComponent1.GetString("$Error_TARGET_MOCODE_EMPTY"));
				return;
			}

			if(this.drpMOCopySourceQuery.SelectedValue == string.Empty)
			{
				//ExceptionManager.Raise(this.GetType(), "$Error_SOURCE_MOCODE_EMPTY");
				this.Alert(this.languageComponent1.GetString("$Error_SOURCE_MOCODE_EMPTY"));
				return;
			}
			//����Դ��̨�ļ��
			if(this.drpResource.Items.Count == 0 || (this.drpResource.Items.Count== 1 && this.drpResource.Items[0].Value =="ȫ��"))
			{
				//ExceptionManager.Raise(this.GetType(), "$Error_SOURCE_RESOURCE_EMPTY");
				this.Alert(this.languageComponent1.GetString("$Error_SOURCE_RESOURCE_EMPTY"));
				return;
			}

			//��ȡ�û�ѡ��Ļ�̨
			ArrayList selectResCodeList = this.GetFromResource();

			//��ȡ��ͬ�Ļ�̨
			//Hashtable returnResCodeHT = _facade.CheckSameResource(this.drpMOCode.SelectedValue,selectResCodeList);
			Hashtable returnResCodeHT = _facade.CheckSameResource(this.GetFirstMOCode(),selectResCodeList);

			ArrayList sameResCodeList = (ArrayList)returnResCodeHT["SameResCodes"] ;	//��ͬ�Ļ�̨,��Ҫ�û�ѡ���Ƿ񸲸ǵĻ�̨
			ArrayList diffResCodeList = (ArrayList)returnResCodeHT["DifferentResCodes"] ;	//ֻ����Դ�������еĻ�̨
			if(sameResCodeList.Count >0 )
			{
				Hashtable sessionHT = new Hashtable();
				sessionHT["SameResourceCode"] =  sameResCodeList;
				sessionHT["DiffResourceCode"] =  diffResCodeList;
				//sessionHT["ToMOCode"] = this.drpMOCode.SelectedValue;
				sessionHT["ToMOCode"] = this.GetFirstMOCode();
				sessionHT["FromMOCode"] = this.drpMOCopySourceQuery.SelectedValue;
				Session["HT"] = sessionHT;
				//�������ͬ�Ļ�̨,������ͬ��̨��ѡ���,ѡ����Ը��ǵĻ�̨��,ִ�и��Ƴ���,��ʾ���Ƴɹ���رյ�������
				this.ExecuteClientFunction("popCopyPage","");
			}
			else
			{
				//���û����ͬ�Ļ�̨,ֱ��ִ�и��Ƴ���,�����ʾ���Ƴɹ�
				if(diffResCodeList.Count!=0)
				{
					//_facade.CopySMTResourceBOM(this.drpMOCopySourceQuery.SelectedValue,this.drpMOCode.SelectedValue,diffResCodeList);
					_facade.CopySMTResourceBOM(this.drpMOCopySourceQuery.SelectedValue,this.GetFirstMOCode(),diffResCodeList);
					BuildRouteTree();
					this.Alert("�������");
				}
			}

		}

		#region

		//��ȡ��ǰѡ��Ļ�̨
		private ArrayList GetFromResource()
		{
			ArrayList returnList = new ArrayList();
			if(this.drpResource.SelectedValue == string.Empty)
			{
				foreach(ListItem itemRecCode in this.drpResource.Items)
				{
					if(itemRecCode.Value !=string.Empty)
						returnList.Add(itemRecCode.Text);
				}
			}
			else
			{
				returnList.Add(this.drpResource.SelectedValue);
			}
			return returnList;
		}


		/// <summary>
		/// ִ�пͻ��˵ĺ���
		/// </summary>
		/// <param name="FunctionName">������</param>
		/// <param name="FunctionParam">����</param>
		/// <param name="Page">��ǰҳ�������</param>
		public  void ExecuteClientFunction(string FunctionName,string FunctionParam)
		{
			try
			{
				string _msg = string.Empty;
				if(FunctionParam != string.Empty)
					_msg = string.Format("<script language='JavaScript'>  {0}('{1}');</script>",FunctionName,FunctionParam);
				else
					_msg = string.Format("<script language='JavaScript'>  {0}();</script>",FunctionName);

				//��Keyֵ��Ϊ�����,��ֹ�ű��ظ�
				Page.RegisterStartupScript(Guid.NewGuid().ToString(),_msg);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		private void cmdImport_ServerClick(object sender, System.EventArgs e)
		{
			#region
			//վ����
			//����ȡ���ļ���ÿһ�����ϣ��ֱ��жϸ������ϵĻ�̨��վλ��Feeder�����Ƿ���������������
			//ȡ����������Ͻ��е����ж�

			//�����ϵ�������ʾ���ͻ�

			#endregion

			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			if(this.GetFirstMOCode() == string.Empty)
			{
				ExceptionManager.Raise(this.GetType(), "$Error_TARGET_MOCODE_EMPTY");
			}

			//�ϴ��ļ���������
			string fileName = FileLoadProcess.UplodFile2ServerUploadFolder(this.Page,this.fileExcel,null);
			if(fileName == null)
			{
				ExceptionManager.Raise(this.GetType().BaseType,"$Error_UploadFileIsEmpty");
			}
			this.ViewState.Add("UploadedFileName",fileName);
			//ȡ�õ�������
			object[] smtboms = this.GetImportStationItemBOM();

			//�������,��ʾ���д�����Ϣ
			//�κδ��󶼲���ִ�е���
			object[] errorMessages = _facade.ValudateImportSMTBOM( smtboms);
			if(errorMessages!=null && errorMessages.Length>0)
			{
				Session["ErrorSMTBOM"] = errorMessages;
				this.ExecuteClientFunction("popErrorPage","");
			}
			else
			{
				//��������
				foreach(SMTResourceBOM smtbom in smtboms)
				{
					_facade.DeleteSMTResourceBOM(smtbom);
					_facade.AddSMTResourceBOM(smtbom);
					continue ;
				}

				//���¼��ػ�̨���οؼ�
				this.BuildRouteTree() ;
			}
		}


		private void cmdCompare_ServerClick(object sender, System.EventArgs e)
		{
			#region
			//�ȶ�BOM�߼�
			//�ȶ�,��ʾ�쳣

			Session.Remove("HT");
			Session.Remove("SessionCompareHT");
			#endregion

			object[] moboms = new object[]{};
			if(this.rblMOBOMSourceSelect.SelectedValue == CompareSourceType.DB)
			{
				#region	��ȡ����MOBOM, ���Ը���OPCode��ѯ
				MOFacade mofacade = new MOFacade(base.DataProvider);

				//TODO ForSimone ������Ҫ��Ϊ��ѡ��ȥ��ѡ��Ĺ�������
				object[] dbmoboms = mofacade.GetMOBOM(this.txtMoQuery.Text,this.txtSapOPCode.Text); //���ݹ�����OPCode��ȡMOBOM

				//�ȶ�MOBOMITEM
				moboms = this.MapperMOBOMITEM( dbmoboms );

				#endregion
			}
			else if(this.rblMOBOMSourceSelect.SelectedValue == CompareSourceType.Excel)
			{
				#region ��ȡMOBom����ʵ�� ��Excel
				//			//�ϴ��ļ���������
				string fileName2 = FileLoadProcess.UplodFile2ServerUploadFolder(this.Page,this.FileMOItem,null);
				if(fileName2 == null)
				{
					ExceptionManager.Raise(this.GetType().BaseType,"$Error_UploadFileIsEmpty");
				}
				this.ViewState.Add("UploadedFileName2",fileName2);
				//ȡ������(���������嵥)
				moboms = this.GetUploadMOBOM();

				if(!this.CheckMO(moboms))
				{
					this.Alert("�����嵥�������ڵ�ǰ����,������ѡ�񹤵������嵥�ļ�!");return;
				}
				#endregion
			}

			#region ��ȡվ����ʵ��
			string fileName = FileLoadProcess.UplodFile2ServerUploadFolder(this.Page,this.fileExcel,null);
			if(fileName == null)
			{
				ExceptionManager.Raise(this.GetType().BaseType,"$Error_UploadFileIsEmpty");
			}
			this.ViewState.Add("UploadedFileName",fileName);
			//ȡ�õ�������
			object[] stboms = this.GetUploadStationItemBOM();

			#endregion

			
			//�ȶԲ���,��Session���ݱȶԽ��
			Hashtable SessionCompareHT = this.GetCompareResult(ClearBlankStationRow(stboms),ClearBlankMOItemBOMRow(moboms));
			Session["SessionCompareHT"] = SessionCompareHT;
			this.ExecuteClientFunction("popDifferentBomPage","");
		}

		private object[] MapperMOBOMITEM(object[] dbmoboms )
		{
			ArrayList returnMOBOMITEMList = new ArrayList();
			if(dbmoboms!=null)
			{
				foreach(MOBOM _mobom in dbmoboms)
				{
					MOItemBOM _moItemBom = new MOItemBOM();
					_moItemBom.MOCode = _mobom.MOCode;
					_moItemBom.ItemCode = _mobom.ItemCode;
					_moItemBom.OBItemCode = _mobom.MOBOMItemCode;
					_moItemBom.OBItemName = _mobom.MOBOMItemName;
					_moItemBom.OBItemQTY = _mobom.MOBOMItemQty.ToString();
					_moItemBom.OBItemUnit = _mobom.MOBOMItemUOM;

					returnMOBOMITEMList.Add(_moItemBom);
				}
			}
			return (object[])returnMOBOMITEMList.ToArray(typeof(MOItemBOM));;
		}

		private bool CheckMO(object[] moboms)
		{
			MOItemBOM moitem = (MOItemBOM)moboms[0];
			if(moitem.MOCode == this.hidtxtMOCode.Value)
			{
				return true;
			}
			return false;
		}

		
		//ȥ������
		private object[] ClearBlankStationRow(object[] stationboms)
		{
			if(stationboms ==null || stationboms.Length==0)return stationboms;
			ArrayList returnList = new ArrayList();
			foreach(StationBOM stbom in stationboms)
			{
				if(stbom.OBItemCode!=string.Empty)
				{
					returnList.Add(stbom);
				}
			}
			return returnList.ToArray();
		}

		//ȥ������
		private object[] ClearBlankMOItemBOMRow(object[] moboms)
		{
			if(moboms ==null || moboms.Length==0)return moboms;
			ArrayList returnList = new ArrayList();
			foreach(MOItemBOM item in moboms)
			{
				if(item.OBItemCode!=string.Empty)
				{
					returnList.Add(item);
				}
			}
			return returnList.ToArray();
		}

		private void cmdchangeMO_ServerClick(object sender, System.EventArgs e)
		{
			//��ǰ��ά�������ı�
			this.drpMOCode_SelectedIndexChanged(null,null);
		}

		private void cmdFreshTree_ServerClick(object sender, System.EventArgs e)
		{
			this.BuildRouteTree(); //ˢ�����ṹ
		}

		private void cmdMOClose_ServerClick(object sender, System.EventArgs e)
		{
			this.ExportCheck();
			this.ViewState["Export"] = "True";
			this.excelExporter.RowSplit = "\r\n" ;
			this.excelExporter.Export();
		}

		#region ����˽�з���

		private object[] GetImportStationItemBOM()
		{
			string fileName = string.Empty ;

			fileName = this.ViewState["UploadedFileName"].ToString() ;
            
			string configFile = this.getParseConfigFileName() ;

			DataFileParser parser = new DataFileParser();
			parser.FormatName = "SMTResourceBOM" ;
			parser.ConfigFile = configFile ;
			//parser.CheckValidHandle = new CheckValid( this.SMTResourceBOMDownloadCheck );			//����߼�
			object[] smtboms = parser.Parse(fileName) ;

			object[] filterboms = _facade.FilterImportData(this.GetFirstMOCode(),smtboms);	//�ȶԼ��,���ؿ��Ե��������

			foreach(SMTResourceBOM SMTrBOM in filterboms)
			{
				//ӳ�������Լ����ݸ�ʽ
				//SMTrBOM.MOCode = FormatHelper.PKCapitalFormat(this.drpMOCode.SelectedValue);
				SMTrBOM.MOCode = FormatHelper.PKCapitalFormat(this.GetFirstMOCode());
				SMTrBOM.ItemCode = FormatHelper.PKCapitalFormat(this.txtItemCode.Text);
				SMTrBOM.ResourceCode = FormatHelper.PKCapitalFormat(SMTrBOM.ResourceCode);
				SMTrBOM.StationCode = FormatHelper.PKCapitalFormat(SMTrBOM.StationCode);
				SMTrBOM.FeederCode = FormatHelper.PKCapitalFormat(SMTrBOM.FeederCode);
				SMTrBOM.OPBOMItemCode = FormatHelper.PKCapitalFormat(SMTrBOM.OPBOMItemCode);

				SMTrBOM.MaintainUser        = this.GetUserCode();
				SMTrBOM.MaintainDate        = FormatHelper.TODateInt(DateTime.Today);
				SMTrBOM.MaintainTime        = FormatHelper.TOTimeInt(DateTime.Now);
			}

			return filterboms ;

		}
		//ȥ������
		private object[] ClearBlankSMTResourceBOMRow(object[] smtboms)
		{
			if(smtboms ==null || smtboms.Length==0)return smtboms;
			ArrayList returnList = new ArrayList();
			foreach(SMTResourceBOM SMTrBOM in smtboms)
			{
				if(SMTrBOM.OPBOMItemCode!=string.Empty)
				{
					returnList.Add(SMTrBOM);
				}
			}
			return returnList.ToArray();
		}


		private string getParseConfigFileName()
		{
			string configFile = this.Server.MapPath(this.TemplateSourceDirectory )  ;
			if(configFile[ configFile.Length - 1 ] != '\\')
			{
				configFile += "\\" ;
			}
			configFile += "DataFileParser.xml" ;
			return configFile ;
		}

		//�������ݼ��
		private bool SMTResourceBOMDownloadCheck(object obj)
		{
			#region �����߼�����߼�

			//����ȡ���ļ���ÿһ�����ϣ��ֱ��жϸ������ϵĻ�̨��վλ��Feeder�����Ƿ���������������
			//Ӧ�ü������SMTResourceBOM������

			#endregion

			if(!this.chbifImportCheck.Checked){return true;}		//�ÿؼ���ʶ�Ƿ�ִ�м���߼�,Ĭ���Ǽ��
			SMTResourceBOM sbom = obj as SMTResourceBOM;

			// ����̨ ResourceCode
			BenQGuru.eMES.BaseSetting.BaseModelFacade baseModelFacade =new SMTFacadeFactory(base.DataProvider).CreateBaseModelFacadeFacade() ;
			object resource = baseModelFacade.GetResource(FormatHelper.PKCapitalFormat(FormatHelper.CleanString( sbom.ResourceCode ))) ;
			if( resource == null )
			{
				ExceptionManager.Raise(this.GetType().BaseType,"$Error_ResourceCode_NotExist");
			}

			// ���վλ StationCode
			object station = _facade.GetStation(FormatHelper.PKCapitalFormat(FormatHelper.CleanString( sbom.ResourceCode )),FormatHelper.PKCapitalFormat(FormatHelper.CleanString( sbom.StationCode ))) ;
			if( station == null )
			{
				ExceptionManager.Raise(this.GetType().BaseType,"$Error_StationCode_NotExist");
			}

			// ��� FeederCode
			if(sbom.FeederCode != string.Empty)
			{
				object Feeder = _facade.GetFeeder(FormatHelper.PKCapitalFormat(FormatHelper.CleanString( sbom.FeederCode ))) ;
				if( Feeder == null )
				{
					ExceptionManager.Raise(this.GetType().BaseType,"$Error_FeederCode_NotExist");
				}
			}

			return true;
		}


		private void Alert(string msg)
		{
			msg = msg.Replace("'","");
			msg = msg.Replace("\r","");
			msg = msg.Replace("\n","");
			string _msg = string.Format("<script language='JavaScript'>  alert('{0}');</script>",msg);
			Page.RegisterStartupScript("",_msg);
		}

		#endregion
        
		protected object[] LoadExportData()
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			ArrayList ResourceCodeList = this.GetExportResource();
			//��ѯ��������������̨�¶�Ӧ������SMTResourceBOM
			object[] boms = this._facade.QuerySMTResourceBOMExport(this.GetFirstMOCode(), ResourceCodeList) ;
			//object[] boms = this._facade.QuerySMTResourceBOMExport(this.drpMOCode.SelectedValue, ResourceCodeList) ;
			return boms ;
		}


		#endregion

		#region վ��ȶ�BOM

		#region �ȶ�BOM˽�з���

		//��ȡ�ȶԽ��
		private Hashtable GetCompareResult(object[] StationObjs,object[] MoBomObjs)
		{
			Hashtable returnHT = new Hashtable();
			ArrayList SucessResult = new ArrayList();			//�ȶԳɹ�
			ArrayList InStationResult = new ArrayList();		//ֻ��վ����
			ArrayList InMoBOMResult = new ArrayList();			//ֻ�������嵥

			//ֻ�ȶ��Ϻ��Ƿ���ͬ
			Hashtable stationHT = new Hashtable();		//station�Ϻż���
			Hashtable moBomHT = new Hashtable();		//�����Ϻż���

			string strMOCode = this.hidtxtMOCode.Value;//((MOItemBOM)MoBomObjs[0]).MOCode;	//��������

			#region ��ʼ���ȶ�Hashtable
			foreach(object stObj in StationObjs)
			{
				StationBOM stbom = (stObj as StationBOM);
				if(stbom!=null)
				{
					if(!stationHT.Contains(stbom.OBItemCode)){stationHT.Add(stbom.OBItemCode,stbom.OBItemCode);}
				}
			}

			foreach(object moitemObj in MoBomObjs)
			{
				MOItemBOM moitem = (moitemObj as MOItemBOM);
				if(moitem!=null)
				{
					if(!moBomHT.Contains(moitem.OBItemCode)){moBomHT.Add(moitem.OBItemCode,moitem.OBItemCode);}
				}
			}

			#endregion
			//��վ��Ϊ�����ȶ�
			foreach(object stObj in StationObjs)
			{
				StationBOM stbom = (stObj as StationBOM);
				if(moBomHT.Contains(stbom.OBItemCode))
				{
					stbom.MOCode = strMOCode;
					stbom.CompareResult = "һ��";
					SucessResult.Add(stbom);		//�����moitembom��,�ȶԳɹ�
				}
				else
				{
					stbom.MOCode = strMOCode;
					stbom.CompareResult = "ֻ������վ����";
					InStationResult.Add(stbom);		//�����ʾֻ��վ������
				}
			}

			foreach(object moitemObj in MoBomObjs)
			{
				MOItemBOM moitem = (moitemObj as MOItemBOM);
				if(!stationHT.Contains(moitem.OBItemCode))
				{
					moitem.CompareResult = "ֻ�����������嵥";
					InMoBOMResult.Add(moitem);		//���վ����û��,��ʾֻ��moitembom����
				}
			}

			//�ж������
			#region �ж������

			Hashtable sucessResStation = new Hashtable();
			foreach(StationBOM sbom in SucessResult)
			{
				string strResStation = sbom.ResourceCode.Trim() + sbom.StationCode.Trim();
				if(!sucessResStation.Contains(strResStation))
					sucessResStation.Add(strResStation,strResStation);
			}
			foreach(StationBOM sbom in InStationResult)
			{
				string strResStation = sbom.ResourceCode.Trim() + sbom.StationCode.Trim();
				if(sucessResStation.Contains(strResStation))
				{
					sbom.CompareResult = "�����";
				}
			}

			#endregion

			returnHT["SucessResult"] = SucessResult;
			returnHT["InStationResult"] = InStationResult;
			returnHT["InMoBOMResult"] = InMoBOMResult;
		
			return returnHT;
		}


		//��ȡվ����ʵ��
		private object[] GetUploadStationItemBOM()
		{
			string fileName = string.Empty ;

			fileName = this.ViewState["UploadedFileName"].ToString() ;
            
			string configFile = this.getParseConfigFileName() ;

			DataFileParser parser = new DataFileParser();
			parser.FormatName = "StationBOM" ;
			parser.ConfigFile = configFile ;
			object[] stboms = parser.Parse(fileName) ;

			return stboms ;

		}
		//ȡ��(���������嵥)����ʵ��
		private object[] GetUploadMOBOM()
		{
			string fileName = string.Empty ;
			fileName = this.ViewState["UploadedFileName2"].ToString() ;
            
			string configFile = this.getParseConfigFileName() ;

			DataFileParser parser = new DataFileParser();
			parser.FormatName = "MOItemBOM" ;
			parser.ConfigFile = configFile ;
			object[] moboms = parser.Parse(fileName) ;

			return moboms ;
		}

		#endregion

		private void Submit1_ServerClick(object sender, System.EventArgs e)
		{
			this.ExecuteClientFunction("popErrorPage","");
		}

		#endregion

		private void rblMOBOMSourceSelect_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.hidComparesource.Value = rblMOBOMSourceSelect.SelectedValue;
			this.ExecuteClientFunction("OnMOBOMSourceChange","");
			this.ExecuteClientFunction("OnMOBOMSourceChange","");
		}

		

	}
	
}
