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
using BenQGuru.eMES.Domain.DataLink;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FFTDetailQuery ��ժҪ˵����
	/// </summary>
	public partial class FFTDetailQuery : BaseQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected GridHelper _gridHelper = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// ��ʼ��ҳ������
			this.InitPageLanguage(this.languageComponent1, false);

			this.txtSNQuery.Text = this.GetRequestParam("Rcard");
			this.txtTestSeq.Text = this.GetRequestParam("TestSeq");

			this._gridHelper = new GridHelper(this.gridWebGrid);

			this._helper = new WebQueryHelper( null,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );

			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);

			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);				

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				this._helper.Query(sender);
			}
		}

		private void _initialWebGrid()
		{
			// ��Ʒ���к�        ������       ��ѹ ����1~20
			this._gridHelper.AddColumn("SN",			"��Ʒ���к�",null);
			this._gridHelper.AddColumn("TestGroup",		"������",null);

			this._gridHelper.AddColumn("FreqLowSpec",			"Ƶ��(����)",null);
			this._gridHelper.AddColumn("FreqUpSpec",			"Ƶ��(����)",null);
			this._gridHelper.AddColumn("Freq",			"Ƶ��",null);
	
			this._gridHelper.AddColumn("DutyLowSpec",			"Duty_RATO(����)",null);
			this._gridHelper.AddColumn("DutyUpSpec",			"Duty_RATO(����)",null);
			this._gridHelper.AddColumn("Duty_RT",			"Duty_RT",null);

			this._gridHelper.AddColumn("BurstLowSpec",			"Burst_MD(����)",null);
			this._gridHelper.AddColumn("BurstUpSpec",			"Burst_MD(����)",null);
			this._gridHelper.AddColumn("Burst_MD",			"Burst_MD",null);

			this._gridHelper.AddColumn("ACLowSpec",			"����(����)",null);
			this._gridHelper.AddColumn("ACUpSpec",			"����(����)",null);
			

			this._gridHelper.AddColumn("AC1",			"���Ե���1",null);
			this._gridHelper.AddColumn("AC2",			"���Ե���2",null);
			this._gridHelper.AddColumn("AC3",			"���Ե���3",null);
			this._gridHelper.AddColumn("AC4",			"���Ե���4",null);
			this._gridHelper.AddColumn("AC5",			"���Ե���5",null);

			this._gridHelper.AddColumn("AC6",			"���Ե���6",null);
			this._gridHelper.AddColumn("AC7",			"���Ե���7",null);
			this._gridHelper.AddColumn("AC8",			"���Ե���8",null);
			this._gridHelper.AddColumn("AC9",			"���Ե���9",null);
			this._gridHelper.AddColumn("AC10",			"���Ե���10",null);

			this._gridHelper.AddColumn("AC11",			"���Ե���11",null);
			this._gridHelper.AddColumn("AC12",			"���Ե���12",null);
			this._gridHelper.AddColumn("AC13",			"���Ե���13",null);
			this._gridHelper.AddColumn("AC14",			"���Ե���14",null);
			this._gridHelper.AddColumn("AC15",			"���Ե���15",null);

			this._gridHelper.AddColumn("AC16",			"���Ե���16",null);
			this._gridHelper.AddColumn("AC17",			"���Ե���17",null);
			this._gridHelper.AddColumn("AC18",			"���Ե���18",null);
			this._gridHelper.AddColumn("AC19",			"���Ե���19",null);
			this._gridHelper.AddColumn("AC20",			"���Ե���20",null);

			this._gridHelper.AddColumn("AC21",			"���Ե���21",null);
			this._gridHelper.AddColumn("AC22",			"���Ե���22",null);
			this._gridHelper.AddColumn("AC23",			"���Ե���23",null);
			this._gridHelper.AddColumn("AC24",			"���Ե���24",null);
			this._gridHelper.AddColumn("AC25",			"���Ե���25",null);

			this._gridHelper.AddColumn("AC26",			"���Ե���26",null);
			this._gridHelper.AddColumn("AC27",			"���Ե���27",null);
			this._gridHelper.AddColumn("AC28",			"���Ե���28",null);
			this._gridHelper.AddColumn("AC29",			"���Ե���29",null);
			this._gridHelper.AddColumn("AC30",			"���Ե���30",null);

			this._gridHelper.AddColumn("AC31",			"���Ե���31",null);

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
			manager.Add( new LengthCheck(this.lblSN,this.txtSNQuery,System.Int32.MaxValue,true) );

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return false;
			}	
			return true;
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			if( this._checkRequireFields() )
			{
				FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);

				( e as WebQueryEventArgs ).GridDataSource = 
					facadeFactory.CreateDataLinkFacade().QueryFTDetail(					
					this.GetRequestParam("ItemCode"),
					FormatHelper.CleanString(this.txtSNQuery.Text).ToUpper(),
					FormatHelper.CleanString(this.txtTestSeq.Text).ToUpper()
					);

				( e as WebQueryEventArgs ).RowCount =
					facadeFactory.CreateDataLinkFacade().QueryFTDetailCount(
					this.GetRequestParam("ItemCode"),
					FormatHelper.CleanString(this.txtSNQuery.Text).ToUpper(),
					FormatHelper.CleanString(this.txtTestSeq.Text).ToUpper());
					
			}
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				FTDetail obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as FTDetail;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  obj.RCard,
													  obj.TGroup,

													  obj.FreqLowSpec.ToString(),
													  obj.FreqUpSpec.ToString(),
													  obj.Freq.ToString(),

													  obj.DutyLowSpec.ToString(),
													  obj.DutyUpSpec.ToString(),
													  obj.Duty_Rt.ToString(),

													  obj.BurstLowSpec.ToString(),
													  obj.BurstUpSpec.ToString(),
													  obj.Burst_Md.ToString(),

													  obj.ACLowSpec.ToString(),
													  obj.ACUpSpec.ToString(),
													  obj.AC1.ToString(),
													  obj.AC2.ToString(),
													  obj.AC3.ToString(),
													  obj.AC4.ToString(),
													  obj.AC5.ToString(),

													  obj.AC6.ToString(),
													  obj.AC7.ToString(),
													  obj.AC8.ToString(),
													  obj.AC9.ToString(),
													  obj.AC10.ToString(),

													  obj.AC11.ToString(),
													  obj.AC12.ToString(),
													  obj.AC13.ToString(),
													  obj.AC14.ToString(),
													  obj.AC15.ToString(),

													  obj.AC16.ToString(),
													  obj.AC17.ToString(),
													  obj.AC18.ToString(),
													  obj.AC19.ToString(),
													  obj.AC20.ToString(),

													  obj.AC21.ToString(),
													  obj.AC22.ToString(),
													  obj.AC23.ToString(),
													  obj.AC24.ToString(),
													  obj.AC25.ToString(),

													  obj.AC26.ToString(),
													  obj.AC27.ToString(),
												      obj.AC28.ToString(),
													  obj.AC29.ToString(),
													  obj.AC30.ToString(),
													  obj.AC31.ToString(),
												  }
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				FTDetail obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as FTDetail;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									obj.RCard,
									obj.TGroup.ToString(),

									obj.FreqLowSpec.ToString(),
									obj.FreqUpSpec.ToString(),
									obj.Freq.ToString(),

									obj.DutyLowSpec.ToString(),
									obj.DutyUpSpec.ToString(),
									obj.Duty_Rt.ToString(),

									obj.BurstLowSpec.ToString(),
									obj.BurstUpSpec.ToString(),
									obj.Burst_Md.ToString(),

									obj.ACLowSpec.ToString(),
									obj.ACUpSpec.ToString(),

									obj.AC1.ToString(),
									obj.AC2.ToString(),
									obj.AC3.ToString(),
									obj.AC4.ToString(),
									obj.AC5.ToString(),

									obj.AC6.ToString(),
									obj.AC7.ToString(),
									obj.AC8.ToString(),
									obj.AC9.ToString(),
									obj.AC10.ToString(),

									obj.AC11.ToString(),
									obj.AC12.ToString(),
									obj.AC13.ToString(),
									obj.AC14.ToString(),
									obj.AC15.ToString(),

									obj.AC16.ToString(),
									obj.AC17.ToString(),
									obj.AC18.ToString(),
									obj.AC19.ToString(),
									obj.AC20.ToString(),

									obj.AC21.ToString(),
									obj.AC22.ToString(),
									obj.AC23.ToString(),
									obj.AC24.ToString(),
									obj.AC25.ToString(),

									obj.AC26.ToString(),
									obj.AC27.ToString(),
									obj.AC28.ToString(),
									obj.AC29.ToString(),
									obj.AC30.ToString(),
									obj.AC31.ToString(),
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{

			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"��Ʒ���к�",
								"������",
								"Ƶ��(����)",
								"Ƶ��(����)",
								"Freq",
								"DUTY_RT(����)",
								"DUTY_RT(����)",
								"Duty_Rt",

								"BURST_MD(����)",
								"BURST_MD(����)",
								"Burst_Md",

								"����(����)",
								"����(����)",
								
								"AC1",
								"AC2",
								"AC3",
								"AC4",
								"AC5",

								"AC6",
								"AC7",
								"AC8",
								"AC9",
								"AC10",

								"AC11",
								"AC12",
								"AC13",
								"AC14",
								"AC15",

								"AC16",
								"AC17",
								"AC18",
								"AC19",
								"AC20",

								"AC21",
								"AC22",
								"AC23",
								"AC24",
								"AC25",

								"AC26",
								"AC27",
								"AC28",
								"AC29",
								"AC30",
								"AC31",
							};
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect("FFTQuery.aspx");
		}		
	}
}
