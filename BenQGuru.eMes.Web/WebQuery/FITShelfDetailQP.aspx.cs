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

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Web.Helper ;
using BenQGuru.eMES.Web.UserControl ;
using BenQGuru.eMES.WebQuery ;
using BenQGuru.eMES.MOModel ;
using BenQGuru.eMES.Domain.MOModel ;
using BenQGuru.eMES.Domain.DataCollect;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FItemTracingQP ��ժҪ˵����
	/// </summary>
	public partial class FITShelfDetailQP : BaseMPage
	{
    

        private System.ComponentModel.IContainer components;
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;

        private ShelfFacade _facade = null ;
 //FacadeFactory.CreateQueryFacade2() ;

		private string shelfpk = String.Empty;

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
			this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
		#endregion


        #region Init
        protected void Page_Load(object sender, System.EventArgs e)
        {
			shelfpk = this.GetRequestParam("ShelfPK") ;

			if(!Page.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				if( _facade == null )
				{
					_facade = new ShelfFacade(base.DataProvider) ;
				}
				object obj = this._facade.GetShelfActionList( shelfpk ) ;
				if(obj == null)
				{
					ExceptionManager.Raise(this.GetType().BaseType,"$Error_RequestUrlParameter_Lost");
				}

				this.txtShelf.Value = ((ShelfActionList)obj).ShelfNO.ToString() ;
				this.txtBurnInDT.Value = FormatHelper.TODateTimeString( ((ShelfActionList)obj).BurnInDate, ((ShelfActionList)obj).BurnInTime ) ; 
				this.txtVolumn.Value = ((ShelfActionList)obj).eAttribute1.ToString() ; 
				this.txtBurnOutDT.Value = FormatHelper.TODateTimeString( ((ShelfActionList)obj).BurnOutDate, ((ShelfActionList)obj).BurnOutTIme ) ;
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
			/* ��Ʒ���кš��������롢��Ʒ���� */
            this.gridHelper.AddColumn("SN","��Ʒ���к�",null) ;
            this.gridHelper.AddColumn( "MOCode", "��������",	null);
            this.gridHelper.AddColumn( "ItemCode", "��Ʒ����",	null);
			this.gridHelper.AddColumn( "ItemStatus", "��Ʒ״̬",	null);

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
 
            base.InitWebGrid();

            this.gridHelper.RequestData();
            this.cmdQuery.Visible = false ;

        }
		
        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{
								((ItemTracingForShelf)obj).RCard,
								((ItemTracingForShelf)obj).MOCode,
								((ItemTracingForShelf)obj).ItemCode,
								this.languageComponent1.GetString(((ItemTracingForShelf)obj).ItemStatus),
							});         
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
			if( _facade == null )
			{
				_facade = new ShelfFacade(base.DataProvider) ;
			}
			object[] objs = this._facade.QueryShelf2RCard(
				shelfpk, inclusive, exclusive );

            return objs;
        }


        protected override int GetRowCount()
        {
			if( _facade == null )
			{
				_facade = new ShelfFacade(base.DataProvider) ;
			}
            return this._facade.QueryShelf2RCardCount( shelfpk );
        }

        #endregion
        
        #region Export 	
        protected override string[] FormatExportRecord( object obj )
        {
            return  new string[]{
									((ItemTracingForShelf)obj).RCard,
									((ItemTracingForShelf)obj).MOCode,
									((ItemTracingForShelf)obj).ItemCode,
									this.languageComponent1.GetString(((ItemTracingForShelf)obj).ItemStatus),
                                }
                ;
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {
                                    "SN", 
                                    "MOCode", 
                                    "ItemCode", 
									"Status",
                                } ;
        }
        #endregion

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			string referedURL = this.GetRequestParam("REFEREDURL") ;
			if( referedURL == string.Empty)
			{
				referedURL = "FItemTracingQP.aspx" ;
			}
			else
			{
				referedURL = System.Web.HttpUtility.UrlDecode(referedURL) ;
			}
			Response.Redirect( referedURL ) ;
		}

        private void SetParam(string key,string pValue)
        {
            ViewState.Add(key,pValue) ;
        }

        private string GetParam(string key)
        {
            if(ViewState[key] == null)
            {
                return string.Empty ;
            }
            else
            {
                return ViewState[key].ToString() ;
            }
        }


	}
}
