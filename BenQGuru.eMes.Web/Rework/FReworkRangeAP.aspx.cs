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
using BenQGuru.eMES.Domain.Rework;
using BenQGuru.eMES.Rework;
using BenQGuru.eMES.Common ;

namespace BenQGuru.eMES.Web.Rework
{
	/// <summary>
	/// FReworkRangeAP ��ժҪ˵����
	/// </summary>
	public partial class FReworkRangeAP : BaseAPage
	{
    
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private BenQGuru.eMES.Rework.ReworkFacade _facade;// = ReworkFacadeFactory.Create();

        public BenQGuru.eMES.Web.UserControl.eMESDate dateFrom ;
        public BenQGuru.eMES.Web.UserControl.eMESTime timeFrom ;

        public BenQGuru.eMES.Web.UserControl.eMESDate dateTo ;
		protected BenQGuru.eMES.Web.SelectQuery.SelectableTextBox txtLotNoQuery2;
        public BenQGuru.eMES.Web.UserControl.eMESTime timeTo ;



        #region Stable
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if( this.GetRequestParam("ReworkSheetCode") == string.Empty)
            {
                ExceptionManager.Raise( this.GetType() , "$Error_RequestUrlParameter_Lost"); 
            }

            if( ! this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

                this.txtReworkSheetCode.Text = this.GetRequestParam("ReworkSheetCode");
                //this.dateFrom.Text = FormatHelper.ToDateString( FormatHelper.TODateInt( DateTime.Now )) ;
                //this.dateTo.Text = FormatHelper.ToDateString(FormatHelper.TODateInt( DateTime.Now )) ;
                this.timeFrom.Text = FormatHelper.ToTimeString( 0 ) ;
                this.timeTo.Text = FormatHelper.ToTimeString( FormatHelper.TOTimeInt( DateTime.Now )) ;
            }
        }
		
        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }
        #endregion

        #region Not Stable
        protected override void InitWebGrid()
        {
            this.gridHelper.AddColumn( "RunningCard" , "���к�" , null ) ;
            this.gridHelper.AddColumn( "RunningCardSequence", "˳���",	null);
            this.gridHelper.AddColumn( "LOTNO", "����",	null);
            this.gridHelper.AddColumn( "MOCode", "����",	null);
            this.gridHelper.AddColumn( "ItemCode", "��Ʒ",	null);
            this.gridHelper.AddColumn( "ModelCode", "��Ʒ��",	null);
            this.gridHelper.AddColumn( "OPCode", "��λ",	null);
            this.gridHelper.AddColumn( "RejectDate", "����",	null);
            this.gridHelper.AddColumn( "RejectTime", "ʱ��",	null);
            this.gridHelper.AddColumn( "RejectUser", "��Ա",	null);
            this.gridHelper.AddColumn( "ErrorCode", "������Ϣ",	null);

            this.gridHelper.AddDefaultColumn( true, false );
            
            this.gridHelper.ApplyLanguage( this.languageComponent1 );

            this.gridHelper.Grid.Columns.FromKey("RunningCardSequence").Hidden = true ;
        }

        protected override void AddDomainObject(ArrayList domainObject)
        {
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            _facade.AddReworkRange( (ReworkRange[])domainObject.ToArray(typeof(ReworkRange)));
        }

        protected override int GetRowCount()
        {
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            return this._facade.GetUnSelectedRejectExByReworkSheetCount(
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtReworkSheetCode.Text )),
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtMOCodeQuery.Text )) , 
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtLotNoQuery.Text )) ,
                this.drpOPCodeQuery.SelectedValue ,
                this.dateFrom.Text ,
                this.timeFrom.Text ,
                this.dateTo.Text , 
                this.timeTo.Text );
        }

        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            Reject2ErrorCode reject2ErrorCode = (Reject2ErrorCode)this._facade.GetReject2ErrorCode(
                ((Reject)obj).RunningCard ,
                ((Reject)obj).RunningCardSequence
                ) ;

            string errorCode = string.Empty ;

            if(reject2ErrorCode != null)
            {
                errorCode = reject2ErrorCode.ErrorCode ;
            }


            return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
                new object[]{"false",
                                ((Reject)obj).RunningCard.ToString() ,
                                ((Reject)obj).RunningCardSequence.ToString() ,
                                ((Reject)obj).LOTNO ,
                                ((Reject)obj).MOCode ,
                                ((Reject)obj).ItemCode ,
                                ((Reject)obj).ModelCode ,
                                ((Reject)obj).OPCode ,
                                FormatHelper.ToDateString(((Reject)obj).RejectDate) ,
                                FormatHelper.ToTimeString(((Reject)obj).RejectTime) ,
                                ((Reject)obj).RejectUser ,
                                errorCode                      
                            }
                );
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {			
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            return this._facade.GetUnSelectedRejectExByReworkSheet(
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtReworkSheetCode.Text )),
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtMOCodeQuery.Text )) , 
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtLotNoQuery.Text )) ,
                this.drpOPCodeQuery.SelectedValue ,
                this.dateFrom.Text ,
                this.timeFrom.Text ,
                this.dateTo.Text , 
                this.timeTo.Text ,
                inclusive,exclusive  );
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
			this.Response.Redirect(this.MakeRedirectUrl("./FReworkRangeSP.aspx", new string[]{"ReworkSheetCode"}, new string[]{FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtReworkSheetCode.Text ))} ));
        }

        #endregion

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
			this.languageComponent1.Language = "CHT";
			this.languageComponent1.LanguagePackageDir = "\\\\grd2-build\\language pack\\";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
        #endregion

        protected void drpOpCodeQuery_Load(object sender , System.EventArgs e)
        {
            if(!this.IsPostBack)
            {
                object[] ops = new ReworkFacadeFactory(base.DataProvider).CreateBaseModelFacade().GetAllOperation() ;
                foreach(BenQGuru.eMES.Domain.BaseSetting.Operation op in ops)
                {
                    this.drpOPCodeQuery.Items.Add( op.OPCode ) ;
                }

                new DropDownListBuilder( this.drpOPCodeQuery ).AddAllItem( this.languageComponent1 ) ;
            }
        }

        protected override object GetEditObject(UltraGridRow row)
        {
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            ReworkRange reworkRange = this._facade.CreateNewReworkRange() ;
            reworkRange.ReworkCode = this.txtReworkSheetCode.Text ;
            reworkRange.RunningCard = row.Cells[1].Text ;
            reworkRange.MaintainUser = this.GetUserCode() ;
            try
            {
                reworkRange.RunningCardSequence = decimal.Parse( row.Cells[2].Text ) ;
            }
            catch
            {
                ExceptionManager.Raise( this.GetType() , "$Error_Format") ;
//                throw new Exception(ErrorCenter.GetErrorUserDescription( this.GetType().BaseType, string.Format(ErrorCenter.ERROR_FORMAT, "RunningCardSequence") ) ); 
            }
            return reworkRange;
        }






    }
}
