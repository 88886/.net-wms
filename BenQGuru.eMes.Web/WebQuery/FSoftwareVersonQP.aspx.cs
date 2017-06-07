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
using BenQGuru.eMES.WebQuery;
using BenQGuru.eMES.Domain.DataCollect;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FSoftwareVersonQP ��ժҪ˵����
	/// </summary>
	public partial class FSoftwareVersonQP : BaseQPage
	{

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected GridHelper _gridHelper = null;
		private string SoftCompareResult = string.Empty;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{

			if(rdbUpToSnuff.Checked)
			{
				SoftCompareResult = rdbUpToSnuff.ID;
			}
			else
			{
				SoftCompareResult = rdbAbnormity.ID;
			}
			this._gridHelper = new GridHelper(this.gridWebGrid);

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);	
			
			FormatHelper.SetSNRangeValue(txtStartSNQuery,txtEndSNQuery);

			if(!Page.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();
				this.txtBeginDate.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.txtEndDate.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.rdbUpToSnuff.Attributes.Add("onclick","OnChecked(this.id)");
				this.rdbAbnormity.Attributes.Add("onclick","OnChecked(this.id)");

				rdbUpToSnuff.Checked = true;
			}
			
			//this.ExecuteClientFunction("OnChecked",this.SoftCompareResult);
		}

		private void _initialWebGrid()
		{
			this.gridWebGrid.Columns.Clear();
			if(this.SoftCompareResult== SoftCompareStatus.Success)
			{
				this._gridHelper.AddColumn("RunningCard",			"��Ʒ���к�",null);
				this._gridHelper.AddColumn("SoftwareName",			"�������",null);
				this._gridHelper.AddColumn("SoftwareVersion",		"����汾",null);	
				this._gridHelper.AddColumn("MOCode",				"����",null);
				this._gridHelper.AddColumn("ItemCode",			"��Ʒ����",null);
				this._gridHelper.AddColumn("CollectionOperationCode",			"�ɼ���λ",null);
				this._gridHelper.AddColumn("CollectionStepSequenceCode",			"�ɼ��߱�",null);
				this._gridHelper.AddColumn("CollectionResourceCode",				"�ɼ���Դ",null);
				this._gridHelper.AddColumn( "EmployeeNo",			"Ա������",	null);
				this._gridHelper.AddColumn( "CollectionDate",		"�ɼ�����",	null);
				this._gridHelper.AddColumn( "CollectionTime",		"�ɼ�ʱ��",	null);	
			}
			else
			{
				this._gridHelper.AddColumn("RunningCard",			"��Ʒ���к�",null);
				this._gridHelper.AddColumn("SoftwareName",			"�������",null);
				this._gridHelper.AddColumn("SoftwareVersion",		"����汾",null);	
				this._gridHelper.AddColumn("SoftErrorVersion",		"����汾",null);	
				this._gridHelper.AddColumn("MOCode",				"����",null);
				this._gridHelper.AddColumn("ItemCode",			"��Ʒ����",null);
				this._gridHelper.AddColumn("CollectionOperationCode",			"�ɼ���λ",null);
				this._gridHelper.AddColumn("CollectionStepSequenceCode",			"�ɼ��߱�",null);
				this._gridHelper.AddColumn("CollectionResourceCode",				"�ɼ���Դ",null);
				this._gridHelper.AddColumn( "EmployeeNo",			"Ա������",	null);
				this._gridHelper.AddColumn( "CollectionDate",		"�ɼ�����",	null);
				this._gridHelper.AddColumn( "CollectionTime",		"�ɼ�ʱ��",	null);		
			}

			//������
			this._gridHelper.ApplyLanguage( this.languageComponent1 );
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

		private bool _checkRequireFields()
		{			

			PageCheckManager manager = new PageCheckManager();
			manager.Add( new LengthCheck(this.lblItemCodeQuery,this.txtConditionItem.TextBox,System.Int32.MaxValue,true) );
			manager.Add(new DateRangeCheck(this.lblBegindate,this.txtBeginDate.Text,this.lblEnddate,this.txtEndDate.Text,true));

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return false;
			}	
			return true;
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			int BeginDate = FormatHelper.TODateInt(this.txtBeginDate.Text);
			int EndDate = FormatHelper.TODateInt(this.txtEndDate.Text);
			string softCompareResult = this.SoftCompareResult;

			FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
			( e as WebQueryEventArgs ).GridDataSource = 
				facadeFactory.CreateQuerySoftwareVersionFacade().QuerySoftwareVersion(
				FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
				FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
				FormatHelper.CleanString(this.txtStartSNQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtEndSNQuery.Text).ToUpper(),
				BeginDate,EndDate,softCompareResult,
				FormatHelper.CleanString(this.txtSoftwareNameQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtSoftwareVersionQuery.Text).ToUpper(),
				( e as WebQueryEventArgs ).StartRow,
				( e as WebQueryEventArgs ).EndRow);

			( e as WebQueryEventArgs ).RowCount = 
				facadeFactory.CreateQuerySoftwareVersionFacade().QuerySoftwareVersionCount(
				FormatHelper.CleanString(this.txtConditionItem.Text).ToUpper(),
				FormatHelper.CleanString(this.txtConditionMo.Text).ToUpper(),
				FormatHelper.CleanString(this.txtStartSNQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtEndSNQuery.Text).ToUpper(),
				BeginDate,EndDate,softCompareResult,
				FormatHelper.CleanString(this.txtSoftwareNameQuery.Text).ToUpper(),
				FormatHelper.CleanString(this.txtSoftwareVersionQuery.Text).ToUpper());
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				if(this.SoftCompareResult== SoftCompareStatus.Success)
				{
					OnWIPSoftVersion obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as OnWIPSoftVersion;
					( e as DomainObjectToGridRowEventArgs ).GridRow = 
						new UltraGridRow( new object[]{
														  obj.RunningCard,
														  obj.SoftwareName,
														  obj.SoftwareVersion,
														  obj.MOCode,
														  obj.ItemCode,
														  obj.OPCode,													  
														  obj.StepSequenceCode,
														  obj.ResourceCode,
														  obj.MaintainUser,
														  FormatHelper.ToDateString(obj.MaintainDate),
														  FormatHelper.ToTimeString(obj.MaintainTime)
													  }
						);
				}
				else
				{
					OnWIPSoftVersionError obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as OnWIPSoftVersionError;
					( e as DomainObjectToGridRowEventArgs ).GridRow = 
						new UltraGridRow( new object[]{
														  obj.Rcard,
														  obj.SoftwareName,
														  obj.MoVersionInfo,
														  obj.VersionInfo,
														  obj.Mocode,
														  obj.ItemCode,
														  obj.OPCode,													  
														  obj.StepSequenceCode,
														  obj.ResourceCode,
														  obj.MUser,
														  FormatHelper.ToDateString(obj.MDate),
														  FormatHelper.ToTimeString(obj.MTime)
													  }
						);
				}
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if(this.SoftCompareResult == SoftCompareStatus.Success)
			{
				if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
				{
					OnWIPSoftVersion obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as OnWIPSoftVersion;
					( e as DomainObjectToExportRowEventArgs ).ExportRow = 
						new string[]{
										obj.RunningCard,
										obj.SoftwareName,
										obj.SoftwareVersion,
										obj.MOCode,
										obj.ItemCode,
										obj.OPCode,													  
										obj.StepSequenceCode,
										obj.ResourceCode,
										obj.MaintainUser,
										FormatHelper.ToDateString(obj.MaintainDate),
										FormatHelper.ToTimeString(obj.MaintainTime)
									};
				}
			}
			else
			{
				if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
				{
					OnWIPSoftVersionError obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as OnWIPSoftVersionError;
					( e as DomainObjectToExportRowEventArgs ).ExportRow = 
						new string[]{
										obj.Rcard,
										obj.SoftwareName,
										obj.MoVersionInfo,
										obj.VersionInfo,
										obj.Mocode,
										obj.ItemCode,
										obj.OPCode,													  
										obj.StepSequenceCode,
										obj.ResourceCode,
										obj.MUser,
										FormatHelper.ToDateString(obj.MDate),
										FormatHelper.ToTimeString(obj.MTime)
									};
				}
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			if(this.SoftCompareResult == SoftCompareStatus.Success)
			{
				( e as ExportHeadEventArgs ).Heads = 
					new string[]{
									"RunningCard",
									"SoftwareName",
									"SoftwareVersion",
									"MOCode",
									"ItemCode",
									"CollectionOperationCode",
									"CollectionStepSequenceCode",
									"CollectionResourceCode",
									"EmployeeNo",
									"CollectionDate",
									"CollectionTime"
								};
			}
			else
			{
				( e as ExportHeadEventArgs ).Heads = 
					new string[]{
									"RunningCard",
									"SoftwareName",
									"SoftwareVersion",
									"SoftErrorVersion",
									"MOCode",
									"ItemCode",
									"CollectionOperationCode",
									"CollectionStepSequenceCode",
									"CollectionResourceCode",
									"EmployeeNo",
									"CollectionDate",
									"CollectionTime"
								};
			}
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

	}
}

