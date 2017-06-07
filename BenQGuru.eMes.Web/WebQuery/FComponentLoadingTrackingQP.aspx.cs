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
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FComponentLoadingTrackingQP ��ժҪ˵����
	/// </summary>
	public partial class FComponentLoadingTrackingQP : BaseQPageNew
	{
        //protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        //private System.ComponentModel.IContainer components;

		protected ExcelExporter excelExporter = null;
		protected WebQueryHelperNew _helper = null;
		protected GridHelperNew _gridHelper = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.gridHelper = new GridHelperNew(this.gridWebGrid, this.DtSource);
			this._gridHelper = new GridHelperNew(this.gridWebGrid,this.DtSource);

			this._helper = new WebQueryHelperNew( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1,this.DtSource );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);		
			//this._helper.GridCellClick +=new EventHandler(_helper_GridCellClick);

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				this.dateStartDateQuery.Text = FormatHelper.ToDateString(FormatHelper.TODateInt(System.DateTime.Now));
				this.dateEndDateQuery.Text = this.dateStartDateQuery.Text;
			}
		}

		private void _initialWebGrid()
		{
            base.InitWebGrid();
			this._gridHelper.AddLinkColumn("WipHistroy",	"��������",null);
			this._gridHelper.AddColumn("SN",				"���к�",null);
			this._gridHelper.AddLinkColumn("INNODetail",	"����������Ϣ",null);
			this._gridHelper.AddLinkColumn("KeypartsDetail","��������Ϣ",null);
			this._gridHelper.AddColumn("ModelCode",			"��Ʒ��",null);
			this._gridHelper.AddColumn("ItemCode",			"��Ʒ",null);
			this._gridHelper.AddColumn("MoCode",			"����",null);
			this._gridHelper.AddColumn("SNStatus",			"��Ʒ״̬",null);
			this._gridHelper.AddColumn("OperationCode",		"���ڹ���",null);
			this._gridHelper.AddColumn("RouteCode",			"����;��",null);
			this._gridHelper.AddColumn("SegmentCode",		"����",null);
			this._gridHelper.AddColumn("StepSequenceCode",	"������",null);
			this._gridHelper.AddColumn("ResourceCode",		"��Դ",null);			
			this._gridHelper.AddColumn( "MaintainDate",		"ά������",	null);
			this._gridHelper.AddColumn( "MaintainTime",		"ά��ʱ��",	null);
			this._gridHelper.AddColumn( "Operator",			"������",	null);
			this._gridHelper.AddColumn( "INNO",			"",	null);
			this._gridHelper.AddColumn( "Keyparts",			"",	null);
			this.gridWebGrid.Columns.FromKey("INNO").Hidden = true;
			this.gridWebGrid.Columns.FromKey("Keyparts").Hidden = true;

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
			manager.Add( new DateRangeCheck(this.lblStartDateQuery,this.dateStartDateQuery.Text,this.lblEndDateQuery,this.dateEndDateQuery.Text,true));

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return true;
			}	

			if(this.txtKeypartsStart.Text == string.Empty)
			{
				this.txtKeypartsStart.Text = this.txtKeypartsEnd.Text;
			}
			if(this.txtKeypartsEnd.Text == string.Empty)
			{
				this.txtKeypartsEnd.Text = this.txtKeypartsStart.Text;
			}
			return true;
		}

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{	
			DoQueryCheck();
			if( this._checkRequireFields() )
			{
				if(chbSplitTransition.Checked)
				{
					//���������к�,ֻ��ʾС��ļ�¼,����С�����к�,����ʾ�κμ�¼
					FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
					( e as WebQueryEventArgsNew ).GridDataSource = 
						facadeFactory.CreateQueryFacade1().QueryComponentLoadingSplitTracking(
						FormatHelper.CleanString(this.txtInsideItemCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtVendorCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtVendorItemCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtLotNo.Text).ToUpper(),
						FormatHelper.CleanString(this.txtDateCode.Text).ToUpper(),
						FormatHelper.CleanString(this.txtINNO.Text).ToUpper(),
						FormatHelper.CleanString(this.txtKeypartsStart.Text).ToUpper(),
						FormatHelper.CleanString(this.txtKeypartsEnd.Text).ToUpper(),
						FormatHelper.TODateInt(this.dateStartDateQuery.Text),
						FormatHelper.TODateInt(this.dateEndDateQuery.Text),
						( e as WebQueryEventArgsNew ).StartRow,
						( e as WebQueryEventArgsNew ).EndRow);

					( e as WebQueryEventArgsNew ).RowCount = 
						facadeFactory.CreateQueryFacade1().QueryComponentLoadingSplitTrackingCount(
						FormatHelper.CleanString(this.txtInsideItemCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtVendorCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtVendorItemCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtLotNo.Text).ToUpper(),
						FormatHelper.CleanString(this.txtDateCode.Text).ToUpper(),
						FormatHelper.CleanString(this.txtINNO.Text).ToUpper(),
						FormatHelper.CleanString(this.txtKeypartsStart.Text).ToUpper(),
						FormatHelper.CleanString(this.txtKeypartsEnd.Text).ToUpper(),
						FormatHelper.TODateInt(this.dateStartDateQuery.Text),
						FormatHelper.TODateInt(this.dateEndDateQuery.Text));
				}
				else
				{
					FacadeFactory facadeFactory = new FacadeFactory(base.DataProvider);
					( e as WebQueryEventArgsNew ).GridDataSource = 
						facadeFactory.CreateQueryFacade1().QueryComponentLoadingTracking2(
						FormatHelper.CleanString(this.txtInsideItemCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtVendorCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtVendorItemCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtLotNo.Text).ToUpper(),
						FormatHelper.CleanString(this.txtDateCode.Text).ToUpper(),
						FormatHelper.CleanString(this.txtINNO.Text).ToUpper(),
						FormatHelper.CleanString(this.txtKeypartsStart.Text).ToUpper(),
						FormatHelper.CleanString(this.txtKeypartsEnd.Text).ToUpper(),
						FormatHelper.TODateInt(this.dateStartDateQuery.Text),
						FormatHelper.TODateInt(this.dateEndDateQuery.Text),
						( e as WebQueryEventArgsNew ).StartRow,
						( e as WebQueryEventArgsNew ).EndRow);

					( e as WebQueryEventArgsNew ).RowCount = 
						facadeFactory.CreateQueryFacade1().QueryComponentLoadingTrackingCount2(
						FormatHelper.CleanString(this.txtInsideItemCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtVendorCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtVendorItemCodeQuery.Text).ToUpper(),
						FormatHelper.CleanString(this.txtLotNo.Text).ToUpper(),
						FormatHelper.CleanString(this.txtDateCode.Text).ToUpper(),
						FormatHelper.CleanString(this.txtINNO.Text).ToUpper(),
						FormatHelper.CleanString(this.txtKeypartsStart.Text).ToUpper(),
						FormatHelper.CleanString(this.txtKeypartsEnd.Text).ToUpper(),
						FormatHelper.TODateInt(this.dateStartDateQuery.Text),
						FormatHelper.TODateInt(this.dateEndDateQuery.Text));
				}
			}
		}

		private void DoQueryCheck()
		{
			// �������������Ĳ�ѯ������Ϊ��.���� "����������ѯ��������Ϊ��"
			if( FormatHelper.CleanString(this.txtInsideItemCodeQuery.Text) == string.Empty 
				&& FormatHelper.CleanString(this.txtVendorCodeQuery.Text) == string.Empty
				&& FormatHelper.CleanString(this.txtVendorItemCodeQuery.Text) == string.Empty
				&& FormatHelper.CleanString(this.txtLotNo.Text) == string.Empty
				&& FormatHelper.CleanString(this.txtDateCode.Text) == string.Empty
				&& FormatHelper.CleanString(this.txtINNO.Text) == string.Empty
				&& FormatHelper.CleanString(this.txtKeypartsStart.Text) == string.Empty
				&& FormatHelper.CleanString(this.txtKeypartsEnd.Text) == string.Empty
				)
			{
				ExceptionManager.Raise(this.GetType() , "$FComponentLoadingTrackingQP_At_Least_One_Query_Not_NULL" ) ;
			}

		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToGridRowEventArgsNew ).DomainObject != null )
			{
				if(chbSplitTransition.Checked)
				{
					//���ת��
					ItemTracing obj = ( e as DomainObjectToGridRowEventArgsNew ).DomainObject as ItemTracing;
                    if (obj != null)
                    {
                        DataRow row = this.DtSource.NewRow();

                        row["SN"] = obj.RCard;
                        row["ModelCode"] = obj.ModelCode;
                        row["ItemCode"] = obj.ItemCode;
                        row["MoCode"] = obj.MOCode;
                        row["SNStatus"] = this.languageComponent1.GetString(obj.ItemStatus);
                        row["OperationCode"] = obj.OPCode;
                        row["RouteCode"] = obj.RouteCode;
                        row["SegmentCode"] = obj.SegmentCode;
                        row["StepSequenceCode"] = obj.LineCode;
                        row["ResourceCode"] = obj.ResCode;
                        row["MaintainDate"] = FormatHelper.ToDateString(obj.MaintainDate);
                        row["MaintainTime"] = FormatHelper.ToTimeString(obj.MaintainTime);
                        row["Operator"] = obj.MaintainUser;
                        row["INNO"] = obj.TCard;
                        row["Keyparts"] = obj.TCard;
                        (e as DomainObjectToGridRowEventArgsNew).GridRow = row;
                    }
				}
				else
				{
					#region

					ComponentLoadingTracking obj = ( e as DomainObjectToGridRowEventArgsNew ).DomainObject as ComponentLoadingTracking;
                    DataRow row = this.DtSource.NewRow();

                    row["SN"] = obj.SN;
                    row["ModelCode"] = obj.ModelCode;
                    row["ItemCode"] = obj.ItemCode;
                    row["MoCode"] = obj.MoCode;
                    row["SNStatus"] = this.languageComponent1.GetString(obj.SNState);
                    row["OperationCode"] = obj.OperationCode;
                    row["RouteCode"] = obj.RouteCode;
                    row["SegmentCode"] = obj.SegmentCode;
                    row["StepSequenceCode"] = obj.StepSequenceCode;
                    row["ResourceCode"] = obj.ResourceCode;
                    row["MaintainDate"] = FormatHelper.ToDateString(obj.MaintainDate);
                    row["MaintainTime"] = FormatHelper.ToTimeString(obj.MaintainTime);
                    row["Operator"] = obj.MaintainUser;
                    row["INNO"] = obj.MCard;
                    row["Keyparts"] = obj.MCard;
                    (e as DomainObjectToGridRowEventArgsNew).GridRow = row;
                    //( e as DomainObjectToGridRowEventArgsNew ).GridRow = 
                    //    new UltraGridRow( new object[]{
                    //                                      "",
                    //                                      obj.SN,
                    //                                      "",
                    //                                      "",	
                    //                                      obj.ModelCode,
                    //                                      obj.ItemCode,
                    //                                      obj.MoCode,
                    //                                      this.languageComponent1.GetString(obj.SNState),
                    //                                      obj.OperationCode,
                    //                                      obj.RouteCode,
                    //                                      obj.SegmentCode,
                    //                                      obj.StepSequenceCode,
                    //                                      obj.ResourceCode,
                    //                                      FormatHelper.ToDateString(obj.MaintainDate),
                    //                                      FormatHelper.ToTimeString(obj.MaintainTime),
                    //                                      obj.MaintainUser,
                    //                                      obj.MCard,
                    //                                      obj.MCard
                    //                                  }
                    //    );

					#endregion
				}
			}
		}

		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			if( ( e as DomainObjectToExportRowEventArgsNew ).DomainObject != null )
			{
				ComponentLoadingTracking obj = ( e as DomainObjectToExportRowEventArgsNew ).DomainObject as ComponentLoadingTracking;
				( e as DomainObjectToExportRowEventArgsNew ).ExportRow = 
					new string[]{
									obj.SN,
									obj.ModelCode,
									obj.ItemCode,
									obj.MoCode,
									this.languageComponent1.GetString(obj.SNState),
									obj.OperationCode,
									obj.RouteCode,
									obj.SegmentCode,
									obj.StepSequenceCode,
									obj.ResourceCode,
									FormatHelper.ToDateString(obj.MaintainDate),
									FormatHelper.ToTimeString(obj.MaintainTime),
									obj.MaintainUser
								};
			}
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
			( e as ExportHeadEventArgsNew ).Heads = 
				new string[]{
								"SN",
								"ModelCode",
								"ItemCode",
								"MOCode",
								"SNState",
								"OperationCode",
								"RouteCode",
								"SegmentCode",
								"StepSequenceCode",
								"ResourceCode",
								"MaintainDate",
								"MaintainTime",
								"MaintainUser"
							};
		}

        protected override void Grid_ClickCell(GridRecord row, string command)
		{
            if (command.ToUpper() == "INNODetail".ToUpper())
			{
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FINNOInfoQP.aspx",
					new string[]{
									"INNO",
									"MCARD",
									"InsideItemCode",
									"VendorCode",
									"VendorItemCode",
									"LotNo",
									"DateCode",
									"OPCode"
								},
					new string[]{
									row.Items.FindItemByKey("SN").Text,
									row.Items.FindItemByKey("INNO").Text,
									FormatHelper.CleanString(this.txtInsideItemCodeQuery.Text),
									FormatHelper.CleanString(this.txtVendorCodeQuery.Text),
									FormatHelper.CleanString(this.txtVendorItemCodeQuery.Text),
									FormatHelper.CleanString(this.txtLotNo.Text),
									FormatHelper.CleanString(this.txtDateCode.Text),
									row.Items.FindItemByKey("OperationCode").Text
								})
					 );
			}
            if (command.ToUpper() == "KeypartsDetail".ToUpper())
			{
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FKeypartsInfoQP.aspx",
					new string[]{
									"Keyparts",
									"MCARD",
									"InsideItemCode",
									"VendorCode",
									"VendorItemCode",
									"LotNo",
									"DateCode",
									"OPCode"
								},
					new string[]{
									row.Items.FindItemByKey("SN").Text,
									row.Items.FindItemByKey("Keyparts").Text,
									FormatHelper.CleanString(this.txtInsideItemCodeQuery.Text),
									FormatHelper.CleanString(this.txtVendorCodeQuery.Text),
									FormatHelper.CleanString(this.txtVendorItemCodeQuery.Text),
									FormatHelper.CleanString(this.txtLotNo.Text),
									FormatHelper.CleanString(this.txtDateCode.Text),
									row.Items.FindItemByKey("OperationCode").Text
								})
					);
			}
			if( command.ToUpper() == "WipHistroy".ToUpper() )
			{			
				this.Response.Redirect( 
					this.MakeRedirectUrl(
					"FITProductionProcessQP.aspx",
					new string[]{
									"RCARD",
									"MOCODE",
									"REFEREDURL",
									"RCARDSEQ"
				},
					new string[]{
									row.Items.FindItemByKey("SN").Text,
									row.Items.FindItemByKey("MoCode").Text,
									"FComponentLoadingTrackingQP.aspx",
									"-1"
								})
					);
			}
		}
	}
}
