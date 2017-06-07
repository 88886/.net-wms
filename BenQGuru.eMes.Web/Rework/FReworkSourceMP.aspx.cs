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
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Rework;
using BenQGuru.eMES.Domain.Rework;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.Rework
{
	/// <summary>
	/// FReworkSourceMP ��ժҪ˵����
	/// </summary>
    public partial class FReworkSourceMP : BaseMPageMinus
{

		protected BenQGuru.eMES.Web.Helper.ExcelExporter excelExporter;
        //private BenQGuru.eMES.Web.Helper.GridHelper gridHelper = null;
        private BenQGuru.eMES.Web.Helper.ButtonHelper buttonHelper = null;
        //protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        private BenQGuru.eMES.Rework.ReworkFacade _facade ;//= new ReworkFacadeFactory(base.DataProvider).Create();

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
            //this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
			this.excelExporter = new BenQGuru.eMES.Web.Helper.ExcelExporter(this.components);
			//this.gridWebGrid.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
			// 
			// languageComponent1
			// 
            //this.languageComponent1.Language = "CHS";
            //this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
            //this.languageComponent1.RuntimePage = null;
            //this.languageComponent1.RuntimeUserControl = null;
            //this.languageComponent1.UserControlName = "";
			// 
			// excelExporter
			// 
			this.excelExporter.FileExtension = "xls";
			this.excelExporter.LanguageComponent = null;
			this.excelExporter.Page = this;
			this.excelExporter.RowSplit = "\r\n";

		}
		#endregion


        #region Init
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.InitOnPostBack();

            if (!IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

                this.InitButton();
                this.InitWebGrid();
            }
        }

        private void InitOnPostBack()
        {		
            this.buttonHelper = new ButtonHelper(this);
            this.buttonHelper.SetEditObjectHandle = new SetEditObjectDelegate(this.SetEditObject);
            this.buttonHelper.AfterPageStatusChangeHandle = new PageStatusChangeDelegate(this.buttonHelper_AfterPageStatusChangeHandle);

            this.gridHelper = new GridHelperNew(this.gridWebGrid,this.DtSource);
            this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegateNew(this.LoadDataSource);
            this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegateNew(this.GetGridRow);

            this.pagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);
			this.excelExporter.LanguageComponent = this.languageComponent1;
			this.excelExporter.LoadExportDataHandle = new LoadExportDataDelegate(LoadDataSource);
			this.excelExporter.FormatExportRecordHandle = new FormatExportRecordDelegate(FormatExportRecord);
			this.excelExporter.GetColumnHeaderTextHandle = new GetColumnHeaderTextDelegate(GetColumnHeaderText);

        }

        private void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
        {
            this.gridHelper.GridBind( this.pagerToolBar.PageIndex, this.pagerToolBar.PageSize );
        }

        private void InitButton()
        {	
            this.buttonHelper.PageActionStatusHandle( PageActionType.Add );
            this.buttonHelper.AddDeleteConfirm();
        }
        #endregion

        #region WebGrid
        private void InitWebGrid()
        {
            // TODO: �����е�˳�򼰱���

            this.gridHelper.AddColumn( "ReworkSourceCode", "������Դ����",	null);
            this.gridHelper.AddColumn( "ReworkSourceDescription", "������Դ����",	null);
            this.gridHelper.AddColumn( "MaintainUser", "ά����Ա",	null);
            this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
            this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
            this.gridHelper.AddDefaultColumn( true, true );
            
            this.gridHelper.ApplyLanguage( this.languageComponent1 );
        }
		
        protected DataRow GetGridRow(object obj)
        {
            DataRow row = DtSource.NewRow();
            row["ReworkSourceCode"] = ((ReworkSource)obj).ReworkSourceCode.ToString();
            row["ReworkSourceDescription"] = ((ReworkSource)obj).Description.ToString();
            row["MaintainUser"] = ((ReworkSource)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((ReworkSource)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((ReworkSource)obj).MaintainTime);
            return row;

        }

        private object[] LoadDataSource(int inclusive, int exclusive)
        {
            // TODO: ʹ������֮����ֶβ�ѯ����ĺ�̨Facade��Query����
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            return this._facade.QueryReworkSource( 
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtReworkSourceCodeQuery.Text) ),
                inclusive, exclusive );
        }


        private int GetRowCount()
        {
            // TODO: ʹ������֮����ֶβ�ѯ����ĺ�̨Facade��Query����
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            return this._facade.QueryReworkSourceCount( 
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtReworkSourceCodeQuery.Text)));
        }

        #endregion

        #region Button

        protected void cmdAdd_ServerClick(object sender, System.EventArgs e)
        {		
            if( !this.ValidateInput())
            {
                return ;
            }
            
            if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            this._facade.AddReworkSource( (ReworkSource)this.GetEditObject() );

            this.RequestData();
            this.buttonHelper.PageActionStatusHandle( PageActionType.Add );
        }

        protected void cmdDelete_ServerClick(object sender, System.EventArgs e)
        {
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            ArrayList array = this.gridHelper.GetCheckedRows();
            ArrayList reworkSources = new ArrayList( array.Count );
			
            foreach (GridRecord row in array)
            {
                reworkSources.Add( (ReworkSource)this.GetEditObject(row) );
            }

            this._facade.DeleteReworkSource( (ReworkSource[])reworkSources.ToArray( typeof(ReworkSource) ) );

            this.RequestData();
            this.buttonHelper.PageActionStatusHandle( PageActionType.Delete );
        }

        protected void cmdSave_ServerClick(object sender, System.EventArgs e)
        {
            if( !this.ValidateInput())
            {
                return ;
            }
            
            if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            this._facade.UpdateReworkSource( (ReworkSource)this.GetEditObject() );

            this.RequestData();
            this.buttonHelper.PageActionStatusHandle( PageActionType.Save );
        }

        protected void cmdCancel_ServerClick(object sender, System.EventArgs e)
        {
            this.buttonHelper.PageActionStatusHandle( PageActionType.Cancel );		
        }

        protected void cmdQuery_ServerClick(object sender, System.EventArgs e)
        {
            this.RequestData();
            this.buttonHelper.PageActionStatusHandle( PageActionType.Query );		
        }

        private void RequestData()
        {
            this.pagerToolBar.PageSize = this.pagerSizeSelector.PageSize;
			this.gridHelper.GridBind(PageGridBunding.Page, this.pagerSizeSelector.PageSize);

            this.pagerToolBar.RowCount = GetRowCount();
            this.pagerToolBar.InitPager();
        }

        protected override void Grid_ClickCell(GridRecord row, string commandName)
        {
            if (commandName == "Edit")
            {
                object obj = this.GetEditObject(row);

                if (obj != null)
                {
                    this.SetEditObject(obj);
                    this.buttonHelper.PageActionStatusHandle(PageActionType.Update);
                }
            }
        }

        private void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
        {
            // TODO: ���ɸ��ĵ��ֶ���ֻ��

            if ( pageAction == PageActionType.Add )
            {
                this.txtReworkSourceCodeEdit.ReadOnly = false;
            }

            if ( pageAction == PageActionType.Update )
            {
                this.txtReworkSourceCodeEdit.ReadOnly = true;
            }
        }

        #endregion

        #region Object <--> Page

        private object GetEditObject()
        {
            // TODO: �����ʹ��TextBox�����޸�

			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            ReworkSource reworkSource = this._facade.CreateNewReworkSource();

            reworkSource.ReworkSourceCode = FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtReworkSourceCodeEdit.Text, 40) );
            reworkSource.Description = FormatHelper.CleanString(this.txtDescriptionEdit.Text, 100);
            reworkSource.MaintainUser = this.GetUserCode();

            return reworkSource;
        }


        private object GetEditObject(GridRecord row)
        {	
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            // TODO: �������е�Index���滻keyIndex
            object obj = _facade.GetReworkSource(row.Items.FindItemByKey("ReworkSourceCode").Value.ToString());
			
            if (obj != null)
            {
                return (ReworkSource)obj;
            }

            return null;
        }

        private void SetEditObject(object obj)
        {
            // TODO: �����ʹ��TextBox�����޸�

            if (obj == null)
            {
                this.txtReworkSourceCodeEdit.Text	= "";
                this.txtDescriptionEdit.Text	= "";

                return;
            }

            this.txtReworkSourceCodeEdit.Text	= ((ReworkSource)obj).ReworkSourceCode.ToString();
            this.txtDescriptionEdit.Text	= ((ReworkSource)obj).Description.ToString();
        }

		
        private bool ValidateInput()
        {

            PageCheckManager manager = new PageCheckManager();

            manager.Add( new LengthCheck(lblReworkSourceCodeEdit, txtReworkSourceCodeEdit, 40, true) );
            manager.Add( new LengthCheck(lblReworkDescriptionEdit,txtDescriptionEdit,100,false)) ;

            if ( !manager.Check() )
            {
                WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
                return false;
            }

            return true ;


        }

        #endregion

        #region Export 		
        // 2005-04-06
        protected void cmdGridExport_ServerClick(object sender, System.EventArgs e)
        {
            this.excelExporter.Export();
        }

        private string[] FormatExportRecord( object obj )
        {
            return new string[]{
                                ((ReworkSource)obj).ReworkSourceCode.ToString(),
                                ((ReworkSource)obj).Description.ToString(),
                                //((ReworkSource)obj).MaintainUser.ToString(),
                               ((ReworkSource)obj).GetDisplayText("MaintainUser"),
                                FormatHelper.ToDateString(((ReworkSource)obj).MaintainDate)
                               };

        }

        private string[] GetColumnHeaderText()
        {
            return new string[] {	"ReworkSourceCode",
                                    "ReworkSourceDescription",
                                    "MaintainUser",
                                    "MaintainDate"
                                };
        }

        private object[] LoadDataSource()
        {
            return this.LoadDataSource( 1, int.MaxValue );
        }
        #endregion
	}
}
