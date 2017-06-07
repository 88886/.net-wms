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

using BenQGuru.eMES.Web.Helper ;
using BenQGuru.eMES.Web.UserControl ;
using BenQGuru.eMES.WebQuery ;
using BenQGuru.eMES.Common ;
using BenQGuru.eMES.Domain.MOModel ;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FItemTracingQP ��ժҪ˵����
	/// </summary>
	public partial class FITMOInfoQP : BaseMPageNew
	{


        private System.ComponentModel.IContainer components;
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;

        private BenQGuru.eMES.MOModel.MOFacade _facade = null ; //new FacadeFactory().CreateMOFacade() ;


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
			//this.gridWebGrid.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
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
            if(!this.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
                
                QueryFacade2 _qFacade = new FacadeFactory(base.DataProvider).CreateQueryFacade2() ;

                string rcard = this.GetRequestParam("RCARD") ;
                int rcardseq ;
                try
                {
                    rcardseq = int.Parse( this.GetRequestParam("RCARDSEQ") );
                }
                catch
                {
                    rcardseq = -1 ;
                }
                string moCode = this.GetRequestParam("MOCODE") ;

                object obj = _qFacade.GetProductionProcess( moCode,rcard,rcardseq ) ;
                if(obj == null)
                {
                    ExceptionManager.Raise(this.GetType() , "$Error_ItemTracing_not_exist") ;
                }

                this.txtItem.Value = ((ProductionProcess)obj).ItemCode ;
                this.txtMO.Value = ((ProductionProcess)obj).MOCode ; 
                this.txtModel.Value = ((ProductionProcess)obj).ModelCode ; 
                this.txtSN.Value = ((ProductionProcess)obj).RCard ; 
                this.txtSeq.Value = ((ProductionProcess)obj).RCardSequence.ToString() ;

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
            this.gridHelper.AddColumn("IT_Factory","����",null) ;
			this.gridHelper.AddColumn( "IT_MOCode", "����",	null);
            this.gridHelper.AddLinkColumn( "IT_MODetail", "������ϸ",	null);
            this.gridHelper.AddColumn( "IT_ProductType", "��������",	null);
            this.gridHelper.AddColumn( "IT_MOStatus", "����״̬",	null);
            this.gridHelper.AddColumn( "IT_PlanStartDate", "�ƻ�������",	null);
            this.gridHelper.AddColumn( "IT_PlanEndDate", "�ƻ��깤��",	null);
            this.gridHelper.AddColumn( "IT_ActualStartDate", "ʵ�ʿ�����",	null);
            this.gridHelper.AddColumn( "IT_ActualEndDate", "ʵ���깤��",	null);
            this.gridHelper.AddColumn( "IT_PlanQty", "�ƻ�����",	null);
			this.gridHelper.AddColumn( "IT_InputQty", "��Ͷ������",	null);
            this.gridHelper.AddColumn( "IT_ActualQty", "���������",	null);
			this.gridHelper.AddColumn( "IT_ScrapQty", "�������",	null);
			this.gridHelper.AddColumn( "IT_NotFinishQty", "δ�깤����",	null);

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

            this.gridHelper.RequestData() ;
            this.cmdQuery.Visible = false ;
 

        }
		
        protected override DataRow GetGridRow(object obj)
        {
            string actualStartDate = string.Empty ;
            string actualEndDate = string.Empty ;

            int minDate = FormatHelper.TODateInt( DateTime.MinValue ) ;
            int maxDate = FormatHelper.TODateInt( DateTime.MaxValue ) ;

            if( ((MO)obj).MOActualStartDate != minDate && ((MO)obj).MOActualStartDate != maxDate )
            {
                actualStartDate = FormatHelper.ToDateString( ((MO)obj).MOActualStartDate ) ;
            }

            if( ((MO)obj).MOActualEndDate != minDate && ((MO)obj).MOActualEndDate != maxDate )
            {
                actualEndDate = FormatHelper.ToDateString( ((MO)obj).MOActualEndDate ) ;
            }

            DataRow row = this.DtSource.NewRow();
            row["IT_Factory"] = ((MO)obj).Factory.ToString();
            row["IT_MOCode"] = ((MO)obj).MOCode;
            row["IT_ProductType"] = this.languageComponent1.GetString(((MO)obj).MOType.ToString());
            row["IT_MOStatus"] = this.languageComponent1.GetString(((MO)obj).MOStatus);
            row["IT_PlanStartDate"] = FormatHelper.ToDateString(((MO)obj).MOPlanStartDate);
            row["IT_PlanEndDate"] = FormatHelper.ToDateString(((MO)obj).MOPlanEndDate);
            row["IT_ActualStartDate"] = actualStartDate;
            row["IT_ActualEndDate"] = actualEndDate;
            row["IT_PlanQty"] = ((MO)obj).MOPlanQty.ToString();
            row["IT_InputQty"] = ((MO)obj).MOInputQty.ToString();
            row["IT_ActualQty"] = ((MO)obj).MOActualQty.ToString();
            row["IT_ScrapQty"] = ((MO)obj).MOScrapQty.ToString();
            row["IT_NotFinishQty"] = string.Format("{0}", ((MO)obj).MOInputQty - ((MO)obj).MOActualQty - ((MO)obj).MOScrapQty);
            return row;


            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
            //    new object[]{
            //                    ((MO)obj).Factory.ToString(),
            //                    ((MO)obj).MOCode,
            //                    "",
            //                    this.languageComponent1.GetString(((MO)obj).MOType.ToString()),
            //                    this.languageComponent1.GetString( ((MO)obj).MOStatus ),
            //                    FormatHelper.ToDateString( ((MO)obj).MOPlanStartDate ),
            //                    FormatHelper.ToDateString( ((MO)obj).MOPlanEndDate ),
            //                    actualStartDate,
            //                    actualEndDate,
            //                    ((MO)obj).MOPlanQty.ToString(),
            //                    ((MO)obj).MOInputQty.ToString(),
            //                    ((MO)obj).MOActualQty.ToString(),
            //                    ((MO)obj).MOScrapQty.ToString(),
            //                    string.Format("{0}", ((MO)obj).MOInputQty - ((MO)obj).MOActualQty-((MO)obj).MOScrapQty )
            //                    }
                
                
            //);
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
			//��ȡrcard��Ӧ�Ĺ���,��������ǰ����
			QueryFacade2 queryFacade = new QueryFacade2(base.DataProvider);
			object[] rcard2MOs = queryFacade.GetMOByRcard(this.txtSN.Value);
			return rcard2MOs;

			#region 

//            object[] objs = new object[1] ;
//			if(_facade ==null)
//			{
//				_facade = new FacadeFactory(base.DataProvider).CreateMOFacade();
//			}
//            object mo = this._facade.GetMO( this.txtMO.Value );
//            if(mo==null)
//            {
//                return null ;
//            }
//            else
//            {
//                objs[0] = mo ;
//                return objs ;
//            }

			#endregion

        }


        protected override int GetRowCount()
        {
            return 1;
        }

        #endregion
        
        #region Export 	
        protected override string[] FormatExportRecord( object obj )
        {
			string actualStartDate = string.Empty ;
			string actualEndDate = string.Empty ;

			int minDate = FormatHelper.TODateInt( DateTime.MinValue ) ;
			int maxDate = FormatHelper.TODateInt( DateTime.MaxValue ) ;

			if( ((MO)obj).MOActualStartDate != minDate && ((MO)obj).MOActualStartDate != maxDate )
			{
				actualStartDate = FormatHelper.ToDateString( ((MO)obj).MOActualStartDate ) ;
			}

			if( ((MO)obj).MOActualEndDate != minDate && ((MO)obj).MOActualEndDate != maxDate )
			{
				actualEndDate = FormatHelper.ToDateString( ((MO)obj).MOActualEndDate ) ;
			}

            return  new string[]{
                                    ((MO)obj).Factory.ToString(),
									((MO)obj).MOCode.ToString(),
                                    this.languageComponent1.GetString(((MO)obj).MOType.ToString()),
                                    this.languageComponent1.GetString( ((MO)obj).MOStatus ),
                                    FormatHelper.ToDateString( ((MO)obj).MOPlanStartDate ),
                                    FormatHelper.ToDateString( ((MO)obj).MOPlanEndDate ),
									actualStartDate,
									actualEndDate,
									((MO)obj).MOPlanQty.ToString(),
									((MO)obj).MOInputQty.ToString(),
                                    ((MO)obj).MOActualQty.ToString(),
									((MO)obj).MOScrapQty.ToString(),
                                    string.Format("{0}", ((MO)obj).MOInputQty - ((MO)obj).MOActualQty-((MO)obj).MOScrapQty )
                                }
                ;
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {
                                    "IT_Factory", 
									"IT_MOCode", 
                                    "IT_ProductType", 
                                    "IT_MOStatus", 
                                    "IT_PlanStartDate", 
                                    "IT_PlanEndDate", 
                                    "IT_ActualStartDate", 
                                    "IT_ActualEndDate", 
                                    "IT_PlanQty", 
									"IT_InputQty",
                                    "IT_ActualQty", 
									"IT_ScrapQty",
                                    "IT_NotFinishQty"
                                } ;
        }
        #endregion

        protected override void Grid_ClickCell(GridRecord row, string command)
        {
			string moCode = row.Items.FindItemByKey("IT_MOCode").Text ;
            if(command=="IT_MODetail")
            {
                Response.Redirect( string.Format("{0}?MOCODE={1}&RCARD={2}" , "FITMODetailQP.aspx" , moCode , this.txtSN.Value ) );
            }

        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("FItemTracingQP.aspx") ;
        }


	}
}
