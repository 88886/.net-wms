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
using BenQGuru.eMES.Common ;
using BenQGuru.eMES.Web.Helper ;
using BenQGuru.eMES.Web.UserControl ;
using BenQGuru.eMES.SelectQuery;
using BenQGuru.eMES.Material;

namespace BenQGuru.eMES.Web.SelectQuery
{
	/// <summary>
	/// Selector ��ժҪ˵����
	/// </summary>
	public partial class FMOSP : BaseSelectorPageNew
	{

        private BenQGuru.eMES.SelectQuery.SPFacade facade ;//= FacadeFactory.CreateSPFacade() ;


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

		}
		#endregion

        #region Init
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}
			factory_load();
		}

        #endregion

        #region WebGrid
        protected override DataRow GetSelectedGridRow(object obj)
        {
            DataRow row = DtSourceSelected.NewRow();
            row["Selector_SelectedCode"] = ((BenQGuru.eMES.SelectQuery.MO2Item)obj).MOCode;
            row["Selector_SelectedDesc"] = ((BenQGuru.eMES.SelectQuery.MO2Item)obj).ItemName;
            return row;
        }

        protected override DataRow GetUnSelectedGridRow(object obj)
        {
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((BenQGuru.eMES.SelectQuery.MO2Item)obj).MOCode;
            row["Selector_UnSelectedDesc"] = ((BenQGuru.eMES.SelectQuery.MO2Item)obj).ItemName;
            return row;
        }

        protected override object[] LoadSelectedDataSource(int inclusive, int exclusive)
        {
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}
            return this.facade.QuerySelectedMO( this.GetSelectedCodes() ) ;
        }

        protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
        {
//			if ( this.GetRequestParam("frompage") == "FOnWipQP" )
//			{
//				return this.facade.QueryUnSelectedMOByItemCode( 
//						FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text) ),
//						FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtMOCodeQuery.Text) ), 
//						MOManufactureStatus.MOSTATUS_OPEN + "," + MOManufactureStatus.MOSTATUS_PENDING,						
//						this.GetSelectedCodes(),
//						inclusive, exclusive ) ;
//			}
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}

            return this.facade.QueryUnSelectedMO( 
							FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtModelCodeQuery.Text) ),
							FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text) ),
							FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtMOCodeQuery.Text) ), 
							this.drpFactory.SelectedValue,
							this.GetSelectedCodes(),
							this.getMOStatus(),
							inclusive, exclusive ) ;
        }


        protected override int GetUnSelectedRowCount()
		{
//			if ( this.GetRequestParam("frompage") == "FOnWipQP" )
//			{
//				return this.facade.QueryUnSelectedMOByItemCodeCount( 
//							FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text) ),
//							FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtMOCodeQuery.Text) ), 
//							MOManufactureStatus.MOSTATUS_OPEN + "," + MOManufactureStatus.MOSTATUS_PENDING,						
//							this.GetSelectedCodes() ) ;
//					}
			if(facade==null){facade = new FacadeFactory(base.DataProvider).CreateSPFacade() ;}

            return this.facade.QueryUnSelectedMOCount( 
							FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtModelCodeQuery.Text) ),
							FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtItemCodeQuery.Text) ),
							FormatHelper.PKCapitalFormat( FormatHelper.CleanString(this.txtMOCodeQuery.Text) ), 
							this.drpFactory.SelectedValue,
							this.GetSelectedCodes() ,
							this.getMOStatus()) ;
        }
        

		private string getMOStatus()
		{
			//returnMOStatusParam Ϊfalse ��ʾ����״̬��������
			//returnMOStatusParam Ϊtrue ���߿�  ��ʾ����״̬������(ȥ����ʼ�������·��Ĺ���)
			string returnMOStatusParam = this.txtOthers.Value;
			
			return returnMOStatusParam;
		}
        #endregion

		private void factory_load()
		{
			if(!Page.IsPostBack)
			{
				WarehouseFacade whFacade = new WarehouseFacade(base.DataProvider);
				object[]  factorys  = whFacade.GetAllFactory();
				if( factorys != null )
				{
					foreach( BenQGuru.eMES.Domain.Warehouse.Factory _factory in factorys)
					{
						this.drpFactory.Items.Add( _factory.FactoryCode ) ;
					}
				}
				new DropDownListBuilder( this.drpFactory ).AddAllItem( this.languageComponent1 ) ;
			}
		}

	}
}
