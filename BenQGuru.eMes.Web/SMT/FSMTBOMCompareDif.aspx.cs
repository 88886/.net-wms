#region system
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
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.SMT;
using BenQGuru.eMES.Domain.SMT;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
#endregion

namespace BenQGuru.eMES.Web.SMT
{
	/// <summary>
	/// FSMTBOMCompareDif ��ժҪ˵����
	/// </summary>
	public class FSMTBOMCompareDif  : BasePage
	{
		protected Infragistics.WebUI.UltraWebGrid.UltraWebGrid gridWebGrid;
		protected Infragistics.WebUI.UltraWebGrid.UltraWebGrid gridInMOBom;
		protected Infragistics.WebUI.UltraWebGrid.UltraWebGrid gridInStationBom;
		protected BenQGuru.eMES.Web.Helper.PagerSizeSelector pagerSizeSelector;
		//protected BenQGuru.eMES.Web.Helper.PagerToolBar pagerToolBar;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdReturn;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;

		private GridHelper gridHelper = null;
		private GridHelper gridInMOBomHelper = null;
		private GridHelper gridInStationBomHelper = null;
		private ExcelExporter excelExporter = null;
		private ButtonHelper buttonHelper = null;

		//private BenQGuru.eMES.SMT.SMTFacade _facade ;//= new SMTFacadeFactory().Create();
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdExport;
		private BenQGuru.eMES.Web.Helper.ExcelExporter excelExporter1;
		private BenQGuru.eMES.Web.Helper.ExcelExporter excelExporter2;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdMOOpen;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdMOClose;
		protected System.Web.UI.WebControls.Label lblTitles;
		//private BenQGuru.eMES.MOModel.MOFacade _modelFacade ;//= SMTFacadeFactory.CreateMOFacade();
		
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			InitHanders();
			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
				// ��ʼ������UI
				this.InitUI();
				//InitParameters();		//���ղ�������
				this.InitWebGrid();
				this.RequestData();		//��������
			}
			
		}


		private void InitParameters()
		{
			if(this.Request.Params["modelcode"] == null)
			{
				ExceptionManager.Raise(this.GetType().BaseType,"$Error_RequestUrlParameter_Lost");
			}
			else
			{
				this.ViewState["modelcode"] = this.Request.Params["modelcode"];
			}
		}


		private void InitHanders()
		{
			//gridInMOBom					gridInMOBomHelper
			//gridInStationBom				gridInStationBomHelper
			this.gridHelper = new GridHelper(this.gridWebGrid);
			this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadDataSource);
			this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridRow);

			this.gridInMOBomHelper = new GridHelper(this.gridInMOBom);
			this.gridInMOBomHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadMOBOMDataSource);
			this.gridInMOBomHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridMOBOMRow);

			this.gridInStationBomHelper = new GridHelper(this.gridInStationBom);
			this.gridInStationBomHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadStationBOMDataSource);
			this.gridInStationBomHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridStationBOMRow);

			this.buttonHelper = new ButtonHelper(this);
			
			//����ֻ������վ���е�����
			this.excelExporter.LoadExportDataNoPageHandle = new LoadExportDataDelegateNoPage(this.GetInSationBOMobj);
			this.excelExporter.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatExportRecord);
			this.excelExporter.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetColumnHeaderText);	

			//�����ɹ������� 
			this.excelExporter1.LoadExportDataNoPageHandle = new LoadExportDataDelegateNoPage(this.GetSucessObj);
			this.excelExporter1.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatSucessExportRecord);
			this.excelExporter1.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetSucessColumnHeaderText);	

			//����ֻ�����ڹ��������嵥������
			this.excelExporter2.LoadExportDataHandle = new LoadExportDataDelegate(this.LoadMOBOMDataSource);
			this.excelExporter2.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatMOBOMExportRecord);
			this.excelExporter2.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetMOBOMColumnHeaderText);	

		}

		#region GetGridRow
		//�ȶԳɹ�
		protected Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{
								((StationBOM)obj).MOCode.ToString(),
								((StationBOM)obj).ResourceCode.ToString(),
								((StationBOM)obj).StationCode.ToString(),
								((StationBOM)obj).FeederCode.ToString(),
								((StationBOM)obj).OBItemCode.ToString(),
								((StationBOM)obj).CompareResult.ToString()
							});
		}
	
		//ֻ������վ����
		protected Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridStationBOMRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{
								((StationBOM)obj).MOCode.ToString(),
								((StationBOM)obj).ResourceCode.ToString(),
								((StationBOM)obj).StationCode.ToString(),
								((StationBOM)obj).FeederCode.ToString(),
								((StationBOM)obj).OBItemCode.ToString(),
								((StationBOM)obj).CompareResult.ToString()
							});
		}

		//ֻ�����������嵥
		protected Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridMOBOMRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{
								((MOItemBOM)obj).MOCode.ToString(),
								((MOItemBOM)obj).ItemCode.ToString(),
								((MOItemBOM)obj).OBItemCode.ToString(),
								((MOItemBOM)obj).OBItemName.ToString(),
								((MOItemBOM)obj).OBItemQTY.ToString(),
								((MOItemBOM)obj).OBItemUnit.ToString(),
								((MOItemBOM)obj).CompareResult.ToString()
							});
		}

		#endregion

		protected  void InitWebGrid()
		{
			//�ȶԳɹ�
			this.gridHelper.AddColumn( "MOCODE", "����",	null);
			this.gridHelper.AddColumn( "RESCODE", "��̨����",	null);
			this.gridHelper.AddColumn( "StationCode", "վλ���",	null);
			this.gridHelper.AddColumn( "FEEDERCODE", "�ϼܹ�����",	null);
			this.gridHelper.AddColumn( "OBITEMCODE", "�Ϻ�",	null);
			this.gridHelper.AddColumn( "CompareResult", "�ȶԽ��",	null);
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

			//ֻ������վ����
			this.gridInStationBomHelper.AddColumn( "MOCODE", "����",	null);
			this.gridInStationBomHelper.AddColumn( "RESCODE", "��̨����",	null);
			this.gridInStationBomHelper.AddColumn( "StationCode", "վλ���",	null);
			this.gridInStationBomHelper.AddColumn( "FEEDERCODE", "�ϼܹ�����",	null);
			this.gridInStationBomHelper.AddColumn( "OBITEMCODE", "�Ϻ�",	null);
			this.gridInStationBomHelper.AddColumn( "CompareResult", "�ȶԽ��",	null);
			this.gridInStationBom.Columns.FromKey("MOCODE").Hidden = true ;	
			this.gridInStationBomHelper.ApplyLanguage( this.languageComponent1 );

			//ֻ�����������嵥
			this.gridInMOBomHelper.AddColumn( "MOCODE", "����",	null);
			this.gridInMOBomHelper.AddColumn( "ITEMCODE", "��Ʒ����",	null);
			this.gridInMOBomHelper.AddColumn( "OBITEMCODE", "�ӽ��Ϻ�",	null);
			this.gridInMOBomHelper.AddColumn( "OBITEMNAME", "�ӽ�������",	null);
			this.gridInMOBomHelper.AddColumn( "OBITEMQTY", "��������",	null);
			this.gridInMOBomHelper.AddColumn( "OBITEMUNIT", "������λ",	null);
			this.gridInMOBomHelper.AddColumn( "CompareResult", "�ȶԽ��",	null);
			this.gridInMOBomHelper.ApplyLanguage( this.languageComponent1 );

		}

		private object[] LoadDataSource(int inclusive, int exclusive)
		{
			return GetSucessObj();
		}

		private object[] LoadMOBOMDataSource(int inclusive, int exclusive)
		{
			return GetInMOBOMObj();
		}
		private object[] LoadStationBOMDataSource(int inclusive, int exclusive)
		{
			return GetInSationBOMobj();
		}

		private object[] LoadMOBOMDataSource()
		{
			return this. LoadMOBOMDataSource(1, 20);
		}

		#region LoadSourece
		//�ȶԳɹ�
		private object[] GetSucessObj()
		{
			Hashtable SessionCompareHT = (Hashtable)Session["SessionCompareHT"];
			if(SessionCompareHT!=null)
			{
				ArrayList returnObjList = (ArrayList)SessionCompareHT["SucessResult"];
				if(returnObjList.Count >0)
				return (StationBOM[])returnObjList.ToArray(typeof(StationBOM) ) ;
			}
			return new object[]{};
		}
	
		//ֻ������վ����
		private object[] GetInSationBOMobj()
		{
			Hashtable SessionCompareHT = (Hashtable)Session["SessionCompareHT"];
			if(SessionCompareHT!=null)
			{
				ArrayList returnObjList = (ArrayList)SessionCompareHT["InStationResult"];
				if(returnObjList.Count >0)
					return (StationBOM[])returnObjList.ToArray(typeof(StationBOM) ) ;
			}
			return new object[]{};

			#region ��������
			//			ArrayList returnObjList = new ArrayList();
			//			BenQGuru.eMES.SMT.StationBOM stBom = new StationBOM();
			//			stBom.MOCode = "����123";
			//			stBom.ResourceCode = "��̨456";
			//			stBom.FeederCode = "Feeder������";
			//			stBom.StationCode = "վλһһ��";
			//			stBom.OBItemCode = "���ddd";
			//			stBom.CompareResult = "�ȶԳɹ�";
			//
			//			returnObjList.Add(stBom);
			//			return (StationBOM[])returnObjList.ToArray(typeof(StationBOM) ) ;
			#endregion
		}
		//ֻ�����������嵥
		private object[] GetInMOBOMObj()
		{

			Hashtable SessionCompareHT = (Hashtable)Session["SessionCompareHT"];
			if(SessionCompareHT!=null)
			{
				ArrayList returnObjList = (ArrayList)SessionCompareHT["InMoBOMResult"];
				if(returnObjList.Count >0)
					return (MOItemBOM[])returnObjList.ToArray(typeof(MOItemBOM) ) ;
			}
			return new object[]{};


		}


		#endregion

	
		//��ȡ����
		private void RequestData()
		{
			this.gridHelper.GridBind(1, 20);
			this.gridInMOBomHelper.GridBind(1, 20);
			this.gridInStationBomHelper.GridBind(1, 20);
		}



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
			this.excelExporter = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
			this.excelExporter1 = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
			this.excelExporter2 = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
			this.cmdMOClose.ServerClick += new System.EventHandler(this.cmdSucessExport_ServerClick);
			this.cmdMOOpen.ServerClick += new System.EventHandler(this.cmdInMOBOMExport_ServerClick);
			this.cmdExport.ServerClick += new System.EventHandler(this.cmdGridExport_ServerClick);
			this.cmdReturn.ServerClick += new System.EventHandler(this.cmdReturn_ServerClick);
			// 
			// languageComponent1
			// 
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";
			// 
			// excelExporter
			// 
			this.excelExporter.CellSplit = ",";
			this.excelExporter.FileExtension = "csv";
			this.excelExporter.LanguageComponent = this.languageComponent1;
			this.excelExporter.Page = this;
			this.excelExporter.RowSplit = "";
			// 
			// excelExporter1
			// 
			this.excelExporter1.CellSplit = ",";
			this.excelExporter1.FileExtension = "csv";
			this.excelExporter1.LanguageComponent = this.languageComponent1;
			this.excelExporter1.Page = this;
			this.excelExporter1.RowSplit = "";
			// 
			// excelExporter2
			// 
			this.excelExporter2.CellSplit = ",";
			this.excelExporter2.FileExtension = "csv";
			this.excelExporter2.LanguageComponent = this.languageComponent1;
			this.excelExporter2.Page = this;
			this.excelExporter2.RowSplit = "";
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void cmdQuery_ServerClick(object sender, System.EventArgs e)
		{
			this.RequestData();
		}


		private object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
		{	
//			Model2Route model2Route = this._modelFacade.CreateNewModel2Route();
//			model2Route.ModelCode = ModelCode;
//			model2Route.RouteCode = row.Cells[1].Text;
//			model2Route.MaintainUser = this.GetUserCode();
//
//			return model2Route;
			return null;
		}

		private void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			Session.Remove("SessionCompareHT");
			//���� , Ӧ�õ��ÿͻ��˽ű�����
			this.ExecuteClientFunction("Close","");
		}

		
		#region Export

		private void cmdGridExport_ServerClick(object sender, System.EventArgs e)
		{
			this.excelExporter.RowSplit = "\r\n" ;
			this.excelExporter.Export();
		}

		
		private void cmdSucessExport_ServerClick(object sender, System.EventArgs e)
		{
			this.excelExporter1.RowSplit = "\r\n" ;
			this.excelExporter1.Export();
		}

		private void cmdInMOBOMExport_ServerClick(object sender, System.EventArgs e)
		{
			this.excelExporter2.RowSplit = "\r\n" ;
			this.excelExporter2.Export();
		}

		#region �����ȶԳɹ�������

		protected  string[] FormatSucessExportRecord( object obj )
		{
			//�������� ��Feeder��λ
			return new string[]{  
								   ((StationBOM)obj).MOCode.ToString(),
								   ((StationBOM)obj).ResourceCode.ToString(),
								   ((StationBOM)obj).StationCode.ToString(),
								   ((StationBOM)obj).FeederCode.ToString(),
								   ((StationBOM)obj).OBItemCode.ToString(),
			};
			
		
		}


		protected  string[] GetSucessColumnHeaderText()
		{
			//�������� ��Feeder��λ
			return new string[] {   
									"MOCode",
									"ResourceCode1",
									"StationCode",
									"FeederCode",
									"MaterialItemCode"
								};
		}

		#endregion

		#region ����վ�������
		protected  string[] FormatExportRecord( object obj )
		{
			//�������� ��Feeder��λ
			return new string[]{  
								   ((StationBOM)obj).ResourceCode.ToString(),
								   ((StationBOM)obj).StationCode.ToString(),
								   ((StationBOM)obj).FeederCode.ToString(),
								   ((StationBOM)obj).OBItemCode.ToString(),
								   ((StationBOM)obj).CompareResult.ToString()
							   };
			
		
		}


		protected  string[] GetColumnHeaderText()
		{
			//�������� ��Feeder��λ
			return new string[] {   
									"ResourceCode1",
									"StationCode",
									"FeederCode",
									"MaterialItemCode",
									"CompareResult"
								};
		}
		#endregion

		#region ���������嵥������
		protected  string[] FormatMOBOMExportRecord( object obj )
		{
			//�������� ��Feeder��λ
			return new string[]{  
								   ((MOItemBOM)obj).MOCode.ToString(),
								   ((MOItemBOM)obj).ItemCode.ToString(),
								   ((MOItemBOM)obj).OBItemCode.ToString(),
								   ((MOItemBOM)obj).OBItemName.ToString(),
								   ((MOItemBOM)obj).OBItemQTY.ToString(),
								   ((MOItemBOM)obj).OBItemUnit.ToString()
			};
			
		
		}


		protected  string[] GetMOBOMColumnHeaderText()
		{
			//�������� ��Feeder��λ
			return new string[] {   
									"MOCode",
									"ItemCode",
									"OBItemCode",
									"OBItemName",
									"OBItemQTY",
									"OBItemUnit"
								};
		}
		#endregion

		#endregion
		
		#region ˽�з���

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

		private void Alert(string msg)
		{
			msg = msg.Replace("'","");
			msg = msg.Replace("\r","");
			msg = msg.Replace("\n","");
			string _msg = string.Format("<script language='JavaScript'>  alert('{0}');</script>",msg);
			Page.RegisterStartupScript("",_msg);
		}

		#endregion



	}
}
