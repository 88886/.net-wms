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
	/// FRejectConfirmMP ��ժҪ˵����
	/// </summary>
	public partial class FRejectConfirmMP : BaseMPage
	{
    
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        public BenQGuru.eMES.Web.UserControl.eMESDate dateFrom ;
        public BenQGuru.eMES.Web.UserControl.eMESTime timeFrom ;

        public BenQGuru.eMES.Web.UserControl.eMESDate dateTo ;
        public BenQGuru.eMES.Web.UserControl.eMESTime timeTo ;

        private BenQGuru.eMES.Rework.ReworkFacade _facade ;//= ReworkFacadeFactory.Create() ;

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
            if(! this.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

                this.dateFrom.Text = FormatHelper.ToDateString( FormatHelper.TODateInt( DateTime.Now )) ;
                this.dateTo.Text = FormatHelper.ToDateString( FormatHelper.TODateInt( DateTime.Now )) ;
                this.timeFrom.Text = FormatHelper.ToTimeString( 0 ) ;
				this.timeTo.Text = FormatHelper.ToTimeString(FormatHelper.TOTimeInt ( DateTime.Now ) );
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
            // TODO: �����е�˳�򼰱���

            this.gridHelper.AddColumn( "RunningCard" , "���к�" , null ) ;
            this.gridHelper.AddColumn( "RunningCardSequence", "˳���",	null);
           // this.gridHelper.AddColumn( "BoxNo", "���",	null);
            this.gridHelper.AddColumn( "LOTNO", "����",	null);
            this.gridHelper.AddColumn( "MOCode1", "����",	null);
            this.gridHelper.AddColumn( "ItemCode1", "��Ʒ",	null);
            this.gridHelper.AddColumn( "ModelCode1", "��Ʒ��",	null);
            this.gridHelper.AddColumn( "RejectStatus1", "״̬",	null);
            this.gridHelper.AddColumn( "OPCode1", "����",	null);
            this.gridHelper.AddColumn( "RejectDate", "����",	null);
            this.gridHelper.AddColumn( "RejectTime", "ʱ��",	null);
            this.gridHelper.AddColumn( "RejectUser", "��Ա",	null);
            this.gridHelper.AddColumn( "ErrorCodeInfo", "������Ϣ",	null);

            this.gridHelper.Grid.Columns.FromKey("RunningCardSequence").Hidden = true ;


            this.gridHelper.AddDefaultColumn( true, false );
            this.gridHelper.ApplyLanguage( this.languageComponent1 );
        }
		
        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
            return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
                new object[]{"false",
                                ((RejectEx)obj).RunningCard.ToString() ,
                                ((RejectEx)obj).RunningCardSequence.ToString() ,
            //                    "",
                                ((RejectEx)obj).LOTNO ,
                                ((RejectEx)obj).MOCode ,
                                ((RejectEx)obj).ItemCode ,
                                ((RejectEx)obj).ModelCode ,
                                this.languageComponent1.GetString( ((RejectEx)obj).RejectStatus ),
                                ((RejectEx)obj).OPCode ,
                                FormatHelper.ToDateString(((RejectEx)obj).RejectDate) ,
                                FormatHelper.ToTimeString(((RejectEx)obj).RejectTime) ,
                                ((RejectEx)obj).RejectUser ,
                                ((RejectEx)obj).ErrorCode        
                            }
                );        
		}

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
			if(this.txtBoxNoQuery.Text.Trim().Length > 0)
			{
				return null;
			}
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            return this._facade.QueryRejectEx(
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.drpModelQuery.SelectedValue )) , 
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtMOCodeQuery.Text )) , 
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtItemCodeQuery.Text )) ,
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtLotNoQuery.Text )) ,
                this.drpOpCodeQuery.SelectedValue ,
                this.dateFrom.Text ,
                this.timeFrom.Text ,
                this.dateTo.Text , 
                this.timeTo.Text ,
                this.drpRejectStatusQuery.SelectedValue,
                inclusive,exclusive  );
        }


        protected override int GetRowCount()
        {
			if(this.txtBoxNoQuery.Text.Trim().Length > 0)
			{
				return 0;
			}
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            return this._facade.QueryRejectExCount(
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtMOCodeQuery.Text )) , 
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtItemCodeQuery.Text )) ,
                FormatHelper.PKCapitalFormat( FormatHelper.CleanString( this.txtLotNoQuery.Text )) ,
                this.drpOpCodeQuery.SelectedValue ,
                this.dateFrom.Text ,
                this.timeFrom.Text ,
                this.dateTo.Text , 
                this.timeTo.Text ,
                this.drpRejectStatusQuery.SelectedValue
                );
        }

        #endregion

        #region Button
        #endregion

        #region Object <--> Page


        protected override object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
        {	
            decimal seq ;
            try
            {
                seq = decimal.Parse( row.Cells[2].Text.ToString() ) ;
            }
            catch
            {
                seq = 0 ;
            }
			if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
            object obj = _facade.GetReject( row.Cells[1].Text.ToString() ,seq );
			
            if (obj != null)
            {
                return (Reject)obj;
            }

            return null;
        }

		
        protected override bool ValidateInput()
        {
            return true ;
        }

        #endregion

        #region ���ݳ�ʼ��
        #endregion

        #region Export
        protected override string[] FormatExportRecord( object obj )
        {
            return new string[]{
                                ((RejectEx)obj).RunningCard.ToString() ,
                                "",
                                ((RejectEx)obj).LOTNO ,
                                ((RejectEx)obj).MOCode ,
                                ((RejectEx)obj).ItemCode ,
                                ((RejectEx)obj).ModelCode ,
                                this.languageComponent1.GetString( ((RejectEx)obj).RejectStatus ) ,
                                ((RejectEx)obj).OPCode ,
								 FormatHelper.ToDateString(((RejectEx)obj).RejectDate) ,
								 FormatHelper.ToTimeString(((RejectEx)obj).RejectTime) ,                                
                                ((RejectEx)obj).RejectUser ,
                                ((RejectEx)obj).ErrorCode        
                            };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"���к�" ,
                                    "���",
                                    "����",
                                    "����",
                                    "��Ʒ",
                                    "��Ʒ��",
                                    "״̬",
                                    "����",
                                    "����",
                                    "ʱ��",
                                    "��Ա",
                                    "������Ϣ"
                                };
        }
        #endregion

        protected void drpModelQuery_Load(object sender, System.EventArgs e)
        {
            if(!this.IsPostBack)
            {
                this.drpModelQuery.Items.Clear() ;
                this.drpModelQuery.Items.Add( string.Empty ) ;

                object[] models = new ReworkFacadeFactory(base.DataProvider).CreateModelFacade().GetAllModels() ;
                if(models != null)
                {
                    foreach(BenQGuru.eMES.Domain.MOModel.Model model in models)
                    {
                        this.drpModelQuery.Items.Add( model.ModelCode ) ;
                    }
                }
            }
        }

        protected void drpRejectStatusQuery_Load(object sender, System.EventArgs e)
        {
            if(!this.IsPostBack)
            {
                this.drpRejectStatusQuery.Items.Clear() ;
                this.drpRejectStatusQuery.Items.Add( string.Empty ) ;

                ArrayList ary = InternalSystemVariable.Lookup("RejectStatus").Items ;
                if( ary != null )
                {
                    foreach(string item in ary)
					{
						if( string.Compare( item, RejectStatus.UnReject, true ) == 0 ) continue;
                        this.drpRejectStatusQuery.Items.Add( 
                            new ListItem(this.languageComponent1.GetString( item),item )) ;
                    }
                }

            }
        
        }

        protected void drpOpCodeQuery_Load(object sender, System.EventArgs e)
        {
            if( ! this.IsPostBack )
            {
                this.drpOpCodeQuery.Items.Clear() ;
                this.drpOpCodeQuery.Items.Add( string.Empty ) ;

                object[] objs = new ReworkFacadeFactory(base.DataProvider).CreateBaseModelFacade().GetAllOperation() ;
                if( objs != null )
                {
                    foreach(BenQGuru.eMES.Domain.BaseSetting.Operation obj in objs)
                    {
                        this.drpOpCodeQuery.Items.Add( obj.OPCode ) ;
                    }
                }
            }
        }

		//ȷ������
        protected void cmdComfirm_ServerClick(object sender, System.EventArgs e)
        {
            ArrayList array = this.gridHelper.GetCheckedRows();

            if ( array.Count > 0 )
            {
				if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
                ArrayList objList = new ArrayList( array.Count );
			
                Reject obj  ;
                foreach (UltraGridRow row in array)
                {
                    obj = (Reject)this.GetEditObject(row) ;
                    objList.Add( obj );
                }
				ArrayList dealLots = this.GetLots(objList);					//Ҫ����������
				ArrayList dealNoLotReject = this.GetNoLots(objList);		//Ҫ�����ĸ��弯��

                //this._facade.UpdateReject( (Reject[])objList.ToArray( typeof(Reject)) );
				this._facade.ConfirmLots(dealLots);
				this._facade.ConfirmNoLots(dealNoLotReject);

                this.gridHelper.RequestData();
                this.buttonHelper.PageActionStatusHandle( PageActionType.Delete );
            }

        }

		#region ��ȡҪ��������

		//��ȡҪ����������
		private ArrayList GetLots(ArrayList _rejects)
		{
			ArrayList lots = new ArrayList();
			foreach(Reject _reject in _rejects)
			{
				if( _reject.LOTNO.Trim()!=null && _reject.LOTNO.Trim()!=string.Empty )
				{
					if(!lots.Contains(_reject.LOTNO))
					{
						//����б��в������ظ���Lotno,��ӵ��б���
						lots.Add( _reject.LOTNO );
					}
				}
			}

			return lots;
		}
		//��ȡҪ�����ĸ��壨�������κ��������ε�������Ϣ��
		private ArrayList GetNoLots(ArrayList _rejects)
		{
			ArrayList lots = new ArrayList();
			foreach(Reject _reject in _rejects)
			{
				if( _reject.LOTNO.Trim() == null || _reject.LOTNO.Trim() == string.Empty )
				{
					lots.Add( _reject );
				}
			}
			return lots;
		}

		#endregion

		//ȡ��ȷ��
        protected void cmdCancelConfirm_ServerClick(object sender, System.EventArgs e)
        {
            ArrayList array = this.gridHelper.GetCheckedRows();

            if ( array.Count > 0 )
            {
				if(_facade==null){_facade = new ReworkFacadeFactory(base.DataProvider).Create();}
                ArrayList objList = new ArrayList( array.Count );
			
                Reject obj  ;
                foreach (UltraGridRow row in array)
                {
                    obj = (Reject)this.GetEditObject(row) ;
                    objList.Add( obj );
                }

				ArrayList dealLots = this.GetLots(objList);					//Ҫ����������
				ArrayList dealNoLotReject = this.GetNoLots(objList);		//Ҫ�����ĸ��弯��
				this._facade.UnConfirmLots(dealLots);
				this._facade.UnConfirmNoLots(dealNoLotReject);
                //this._facade.UpdateReject( (Reject[])objList.ToArray( typeof(Reject)) );

                this.gridHelper.RequestData();
                this.buttonHelper.PageActionStatusHandle( PageActionType.Delete );
            }
        }

		//ȡ������
        protected void cmdCancelReject_ServerClick(object sender, System.EventArgs e)
        {
			/*������: Joanne Zhao 
				����ʱ��: 2005��9��21�� 12:03
				�ռ���: Simone Xu; David Liu; Eric Zhao
				����: Helen Wang
				����: ��: ���˲ɼ�

				Dear All,
				
				���˲ɼ�Ŀǰ���²����õ������Խ���������
				����ֻ����������д�����Ի�򵥺ܶࡣ

				ҵ��������

				�������������գ�����Ϊ��λ����

				ȡ�����ˣ��ԡ�����Ϊ��λ����ȡ�����ˣ����˺������ߵ����кſ��Խ��м������

				 

				ϵͳʵ�֣�

				�������������գ�FQC�ɼ�Ŀǰ������Ϊ��λ������ȷ��

				ȡ�����ˣ��Ը����Ⱥ�壬ѡ��Ҫ��ȡ�����ˡ������кţ�ϵͳ��ʾ�������к������ġ�������������������С�ȡ�����ˡ���ҵ��

				 

				Ŀǰ����״̬��������ʼ��δ�졢�ڼ졢ͨ��������5��״̬

				ȡ�����˺�Ĳ�Ʒ״̬ϣ�����˵����ڼ�״̬����Ȼ��̳�֮ǰ�����ˡ��ɼ�ǰ�ļ�¼�������ɼ���

				 

				���ڳ������Ƿ����ʵ�ָù��ܣ�����Simone����ȷ�ϡ�

				���������ʱMSN��ϵ��Joannebenq@hotmail.com   

				������ٿ��ʼ� :-)

				Joanne
			 * */

			// modified by jessie lee,2005/9/28,ʵ��ȡ�������˵Ĺ���
			ArrayList arrayList = this.gridHelper.GetCheckedRows();

			if( arrayList.Count>0 )
			{
				if(_facade==null)
				{
					_facade = new ReworkFacadeFactory(base.DataProvider).Create();
				}

				//�����Щ�����Ƿ����ȡ������
				ArrayList nolotReject = new ArrayList();		//û������������Ϣ����
				ArrayList lotNOs = new ArrayList();
				for(int i=0; i<arrayList.Count; i++)
				{
					UltraGridRow row = (UltraGridRow)arrayList[i] ;
					Reject reject = (Reject)this.GetEditObject(row) ;
					if( reject.LOTNO.Trim()!=null && reject.LOTNO.Trim()!=string.Empty )
					{
						lotNOs.Add(reject.LOTNO);
					}
					else
					{
						//���û�����ţ��Ը�����в�����
						nolotReject.Add(reject);
					}
				}

				//��û������������Ϣ���ϵ�ȡ�����˲���
				if(nolotReject.Count >0 )
				{
					_facade.CancelReject(nolotReject);
				}

				ArrayList lotsList = new ArrayList();
				for(int i=0; i<lotNOs.Count; i++)
				{
					bool isRepeat = false;
					for(int j=0; j<lotsList.Count; j++)
					{
						if(string.Compare((string)lotNOs[i],lotsList[j].ToString(),true)==0)
						{
							isRepeat = true ;
							break;
						}	
					}
					if(!isRepeat && (string)lotNOs[i] != string.Empty)
					{
						lotsList.Add(lotNOs[i]);
					}
				}

				if(!_facade.CheckLotToCancelReject(lotsList))
				{
					ExceptionManager.Raise(this.GetType() , "$Error_LOTs_Cannot_CancelReject" ) ;
					return;
				}
				
				/*added by jessie lee, 2005/11/30,
				 * ����ʱ�����ʱ��ӽ�����*/
				this.Page.Response.Write("<div id='mydiv' >"); 
				this.Page.Response.Write("_"); 
				this.Page.Response.Write("</div>"); 
				this.Page.Response.Write("<script>mydiv.innerText = '';</script>"); 
				this.Page.Response.Write("<script language=javascript>;"); 
				this.Page.Response.Write("var dots = 0;var dotmax = 10;function ShowWait()"); 
				this.Page.Response.Write("{var output; output = '���ڴ���,���Ժ�';dots++;if(dots>=dotmax)dots=1;"); 
				this.Page.Response.Write("for(var x = 0;x < dots;x++){output += '��';}mydiv.innerText =  output;}"); 
				this.Page.Response.Write("function StartShowWait(){mydiv.style.visibility = 'visible'; "); 
				this.Page.Response.Write("window.setInterval('ShowWait()',1000);}"); 
				this.Page.Response.Write("function HideWait(){mydiv.style.display = 'none';"); 
				this.Page.Response.Write("window.clearInterval();}"); 
				this.Page.Response.Write("StartShowWait();</script>"); 
				this.Page.Response.Flush(); 

				if(lotsList != null && lotsList.Count > 0)
				{
					_facade.MakeLotsCancelReject(lotsList);
				}
					
				this.Page.Response.Write("<script language=javascript>HideWait();</script>"); 
				
				/* added by jessie lee, 2005/11/30,
				 * ȡ����������Ժ󣬰ѽ���ʱ���Ϊ��ǰ */
				this.dateTo.Text = FormatHelper.ToDateString( FormatHelper.TODateInt( DateTime.Now ));
				this.timeTo.Text = FormatHelper.ToTimeString( FormatHelper.TOTimeInt( DateTime.Now ));

				this.gridHelper.RequestData();
				this.buttonHelper.PageActionStatusHandle( PageActionType.Delete );

			}           
        }

    }
}
