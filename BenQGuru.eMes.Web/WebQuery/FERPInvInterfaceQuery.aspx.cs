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
using BenQGuru.eMES.Domain.DeviceInterface;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Common;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FPTQuery ��ժҪ˵����
	/// </summary>
	public partial class FERPInvInterfaceQuery  : BaseQPage
	{

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelper _helper = null;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected GridHelper _gridHelper = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelper(this.gridWebGrid);

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				txtRecieveBeginDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );
				txtRecieveEndDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );

				txtTossBeginDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );
				txtTossEndDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );

				InitDropDownList();

				gridWebGrid.DisplayLayout.SelectTypeRowDefault = Infragistics.WebUI.UltraWebGrid.SelectType.Single;

				if(!this.Page.IsStartupScriptRegistered("SelectableTextBox_Startup_js"))
				{
					string scriptString = string.Format("<script>var STB_Virtual_Path = \"{0}\";</script><script src='{0}SelectQuery/selectableTextBox.js'></script>",this.VirtualHostRoot ) ;
                
					this.Page.RegisterStartupScript("SelectableTextBox_Startup_js", scriptString);
				}
				
			}

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);				

			//FormatHelper.SetSNRangeValue(txtStartSnQuery,txtEndSnQuery);
		}

		private void InitDropDownList()
		{
			DropDownListBuilder builder = new DropDownListBuilder(this.drpStatusEdit);
			//			if(BaseMode_Facade==null)
			//			{
			//				BaseMode_Facade = new FacadeFactory(base.DataProvider).CreateBaseModelFacade() ;
			//			}
			//			builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate(BaseMode_Facade.GetAllResource);
			//Web.Helper.INVERPType.INVERPTYPE_NEW 
			//builder.Build("ResourceCode", "ResourceCode");
			this.drpStatusEdit.Items.Add(new ListItem(languageComponent1.GetString(Web.Helper.INVERPType.INVERPTYPE_NEW)
				,Web.Helper.INVERPType.INVERPTYPE_NEW));
			this.drpStatusEdit.Items.Add(new ListItem(languageComponent1.GetString(Web.Helper.INVERPType.INVERPTYPE_PROCESSED)
				,Web.Helper.INVERPType.INVERPTYPE_PROCESSED));
			this.drpStatusEdit.Items.Insert(0, "");
		}

		private void getAllStatus()
		{
			//return Web.Helper.e
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

		private void _initialWebGrid()
		{
			//��Ʒ���кš���Ʒ���롢�������롢���ߡ���Դ�����Ա�׼ֵ,�������ֵ,������Сֵ�����Խ����������Ա,�������ڣ�����ʱ��
		
			this._gridHelper.AddColumn( "RECNO",			"��ⵥ��",null);
			this._gridHelper.AddColumn( "ITEMCODE",		"��Ʒ����",null);
			this._gridHelper.AddColumn( "MOCODE",		"��������",null);
			this._gridHelper.AddColumn( "RECIEVEDATE",		"�������",null);
			this._gridHelper.AddColumn( "RECIEVEUSER",		"�����Ա",null);
			
			this._gridHelper.AddColumn( "QTY",	"����",null);
			this._gridHelper.AddColumn( "STATUS",	"��ת��ʶ",null);
			this._gridHelper.AddColumn( "UPLOADUSER",	"��ת��Ա",null);
			this._gridHelper.AddColumn( "UPLOADDATE",	"��ת����",null);

			this._gridHelper.AddColumn( "SRNO",		"SR����",null);

			this._gridHelper.AddDefaultColumn(true,false);

			//������
			this._gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
			PageCheckManager manager = new PageCheckManager();
			//			// Added by Icyer 2006/08/16
			//			if (this.txtItemQuery.Text.Trim() == string.Empty)
			//			{
			//				throw new Exception("$Error_ItemCode_NotCompare");
			//			}
			//			// Added end
		
			this.QueryEvent(sender,e);
		}

		#region ��ѯ�¼�

		private void QueryEvent(object sender, EventArgs e)
		{
			//			int BeginDate = FormatHelper.TODateInt(this.txtBeginDate.Text);
			//			int EndDate = FormatHelper.TODateInt(this.txtEndDate.Text);

			string recBegDate = String.Empty;
			string recEndDate =  String.Empty;
			string tossBegDate =  String.Empty;
			string tossEndDate =  String.Empty;

			if(chbRecieve.Checked == true)
			{
				recBegDate = FormatHelper.TODateInt(DateTime.Parse(this.txtRecieveBeginDate.Text)).ToString();
				recEndDate = FormatHelper.TODateInt(DateTime.Parse(this.txtRecieveEndDate.Text)).ToString();
			}
			if(chbToss.Checked == true)
			{
				tossBegDate = FormatHelper.TODateInt(DateTime.Parse(this.txtTossBeginDate.Text)).ToString();
				tossEndDate = FormatHelper.TODateInt(DateTime.Parse(this.txtTossEndDate.Text)).ToString();
			}

			string moCode = String.Empty;
			string itemCode = String.Empty;
			if(FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper() != String.Empty)
			{
				moCode = String.Join("','",FormatHelper.CleanString(this.txtMoQuery.Text).ToUpper().Split(','));
			}
			if(FormatHelper.CleanString(this.txtItemQuery.Text).ToUpper() != String.Empty)
			{
				itemCode = String.Join("','",FormatHelper.CleanString(this.txtItemQuery.Text).ToUpper().Split(','));
			}

			FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
			( e as WebQueryEventArgs ).GridDataSource = 
				facadeFactory.CreateInventoryFacade().QueryERPINVInterface(
				FormatHelper.CleanString(this.txtRecNO.Text).ToUpper(),
				moCode,
				itemCode,
				recBegDate,
				recEndDate,
				tossBegDate,
				tossEndDate,
				FormatHelper.CleanString(this.drpStatusEdit.SelectedValue).ToUpper(),
				( e as WebQueryEventArgs ).StartRow,
				( e as WebQueryEventArgs ).EndRow);

			( e as WebQueryEventArgs ).RowCount =
				facadeFactory.CreateInventoryFacade().QueryERPINVInterfaceCount(
				FormatHelper.CleanString(this.txtRecNO.Text).ToUpper(),
				moCode,
				itemCode,
				recBegDate,
				recEndDate,
				tossBegDate,
				tossEndDate,
				FormatHelper.CleanString(this.drpStatusEdit.SelectedValue).ToUpper()
				);
		}

		#endregion

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgs ).DomainObject != null )
			{
				//��ⵥ�š���Ʒ���롢�������롢������ڡ������Ա����������ת��ʶ����ת��Ա����ת���ڡ�SR����
				ERPINVInterface obj = ( e as DomainObjectToGridRowEventArgs ).DomainObject as ERPINVInterface;
				( e as DomainObjectToGridRowEventArgs ).GridRow = 
					new UltraGridRow( new object[]{
													  "false",
													  obj.RECNO,
													  obj.ITEMCODE,
													  obj.MOCODE,
													  obj.MDATE,
													  obj.MUSER,
													  obj.QTY,
													  languageComponent1.GetString(obj.STATUS),
													  obj.UPLOADUSER,
													  obj.UPLOADDATE,
													  obj.SRNO,
				}
					);
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{

			if( ( e as DomainObjectToExportRowEventArgs ).DomainObject != null )
			{
				//��ⵥ�š���Ʒ���롢�������롢������ڡ������Ա����������ת��ʶ����ת��Ա����ת���ڡ�SR����
				ERPINVInterface obj = ( e as DomainObjectToExportRowEventArgs ).DomainObject as ERPINVInterface;
				( e as DomainObjectToExportRowEventArgs ).ExportRow = 
					new string[]{
									"false",
									obj.RECNO,
									obj.ITEMCODE,
									obj.MOCODE,
									obj.MDATE.ToString(),
									obj.MUSER.ToString(),
									obj.QTY.ToString(),
									languageComponent1.GetString(obj.STATUS),
									obj.UPLOADUSER,
									obj.UPLOADDATE.ToString(),
									obj.SRNO
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			//��ⵥ�š���Ʒ���롢�������롢������ڡ������Ա����������ת��ʶ����ת��Ա����ת���ڡ�SR����
			( e as ExportHeadEventArgs ).Heads = 
				new string[]{
								"ѡ��",
								"��ⵥ��",
								"��Ʒ����",
								"��������",
								"�������",
								"�����Ա",
								"����",
								"��ת��ʶ",
								"��ת��Ա",
								"��ת����",
								"SR����"
							};
			
		}

		protected void cmdAdd_ServerClick(object sender, System.EventArgs e)
		{
			Common.Domain.IDomainDataProvider erpDataProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider(DBName.ERP);

			FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
			InventoryFacade mesFac = facadeFactory.CreateInventoryFacade();
			((SQLDomainDataProvider)erpDataProvider).PersistBroker.OpenConnection();
			erpDataProvider.BeginTransaction();
			((SQLDomainDataProvider)base.DataProvider).PersistBroker.OpenConnection();
			DataProvider.BeginTransaction();
			int iSelectedRowCount = 0;
			try
			{
				
				foreach(Infragistics.WebUI.UltraWebGrid.UltraGridRow row in this.gridWebGrid.Rows)
				{
					if(row.Selected == true)
					{
						iSelectedRowCount ++;
						string factory = "PO";
						if (System.Configuration.ConfigurationSettings.AppSettings["InvFactory"] != null)
						{
							factory = System.Configuration.ConfigurationSettings.AppSettings["InvFactory"].Trim();
						}

						if(row.Cells[7].Text.Trim() != languageComponent1.GetString(Web.Helper.INVERPType.INVERPTYPE_NEW))
						{
							ERPSRMX srmx = new ERPSRMX();
							srmx.SRMXNO = mesFac.GetMaxSRNO(int.Parse(DateTime.Now.Year.ToString().Substring(2,2)),factory);
	
							srmx.ENDATE = FormatHelper.TODateInt(DateTime.Now);
							string mocode = row.Cells[3].Text.Trim().Substring(5, row.Cells[3].Text.Trim().Length - 5);
							srmx.SONO =  Convert.ToInt32(mocode);
							srmx.FINQTY  = Convert.ToDecimal(row.Cells[6].Text.Trim());
							srmx.SRPROD =  row.Cells[2].Text.Trim();
							srmx.SRDESC =  row.Cells[2].Text.Trim();
							//srmx.
							srmx.UID = GetUserCode();
							srmx.STA = "F";
					
							//��ERP�в�������
							erpDataProvider.Insert(srmx);

							ERPINVInterface erp = new  ERPINVInterface();
							object objERP = mesFac.GetERPINVInterface(
								row.Cells[1].Text.Trim()
								,row.Cells[3].Text.Trim()
								,Web.Helper.INVERPType.INVERPTYPE_PROCESSED
								,row.Cells[10].Text.Trim());

							if(objERP != null)
							{
								erp = objERP as ERPINVInterface;
							}
							else
							{
								erp.RECNO =  row.Cells[1].Text.Trim();
								erp.SRNO =  srmx.SRMXNO;
								erp.ITEMCODE  =  row.Cells[2].Text.Trim();
								
								erp.MDATE =  FormatHelper.TODateInt(row.Cells[4].Text.Trim());
								erp.MOCODE = row.Cells[3].Text.Trim();
								erp.MTIME =  FormatHelper.TOTimeInt(DateTime.Now);
								erp.MUSER =  GetUserCode();
								erp.QTY = Decimal.Parse( row.Cells[6].Text.Trim() );
								erp.STATUS =  Web.Helper.INVERPType.INVERPTYPE_PROCESSED;
//								erp.UPLOADDATE =  erp.MDATE;
//								erp.UPLOADTIME= erp.MTIME;
//								erp.UPLOADUSER =  erp.MUSER;

							}

							erp.UPLOADDATE =  FormatHelper.TODateInt(DateTime.Now);
							erp.UPLOADTIME=  FormatHelper.TOTimeInt(DateTime.Now);
							erp.UPLOADUSER =  GetUserCode();;
							erp.LINKSRNO =  row.Cells[10].Text.Trim();
							erp.SRNO = srmx.SRMXNO;

							mesFac.AddERPINVInterface(erp);
						}
						else
						{
							ExceptionManager.Raise(typeof(ERPINVInterface),"״̬����Ϊ" + languageComponent1.GetString(Web.Helper.INVERPType.INVERPTYPE_PROCESSED));
						}
						erpDataProvider.CommitTransaction();
						DataProvider.CommitTransaction();
						
					}
					
				}
			}
			catch(Exception ex)
			{
				ExceptionManager.Raise( this.GetType(),"",ex);
				erpDataProvider.RollbackTransaction();
			}
			finally
			{
				((SQLDomainDataProvider)erpDataProvider).PersistBroker.CloseConnection();
				((SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
			}

			if(iSelectedRowCount == 0)
			{
				ExceptionManager.Raise(typeof(ERPINVInterface),"û��ѡ���κμ�¼");
			}
			else
			{
				this.Response.Redirect("FERPInvInterfaceQuery.aspx");
			}
		}
	}
}

