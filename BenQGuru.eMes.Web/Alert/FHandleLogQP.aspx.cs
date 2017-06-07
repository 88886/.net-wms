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
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.AlertModel;
using BenQGuru.eMES.Domain.Alert;

namespace BenQGuru.eMES.Web.Alert
{
    public partial class FHandleLogQP : BaseMPage
    {
        private System.ComponentModel.IContainer components;
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected BenQGuru.eMES.AlertModel.AlertFacade _facade;
		protected BenQGuru.eMES.BaseSetting.UserFacade _userfacade;
		private AlertConst _alertConst;
		private decimal _alertID;
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
			this.languageComponent1.LanguagePackageDir = "\\\\..";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
        #endregion

		#region Init
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}

			 _facade = new BenQGuru.eMES.AlertModel.AlertFacade(DataProvider);
			_alertConst = new AlertConst(this.languageComponent1);
			try
			{
				_alertID = int.Parse(Request.QueryString["alertid"]);
			}
			catch
			{
				_alertID = 0;
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
		    this.gridHelper.AddColumn( "HandleMsg", "�����¼",	null);
            this.gridHelper.AddColumn( "HandleUser", "������",	null);
		    this.gridHelper.AddColumn( "UserEmail", "��������",	null);
			this.gridHelper.AddColumn( "AlertLevel", "Ԥ������",	null);
			this.gridHelper.AddColumn( "AlertStatus", "״̬",	null);
			this.gridHelper.AddColumn( "HandleDate", "��������",	null);
			this.gridHelper.AddColumn( "HandleTime", "����ʱ��",	null);

            this.gridHelper.ApplyLanguage( this.languageComponent1 );
			
			this.gridWebGrid.Bands[0].Columns.FromKey("HandleMsg").Width = Unit.Parse("300px");
			this.gridHelper.RequestData();
        }
		
        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
			BenQGuru.eMES.Domain.Alert.AlertHandleLog log = obj as BenQGuru.eMES.Domain.Alert.AlertHandleLog;
			if(log == null )
				return null;

			Infragistics.WebUI.UltraWebGrid.UltraGridRow ur = new UltraGridRow(
																				new object[]
																							{
																							log.HandleMsg,
																							log.HandleUser,
																							log.UserEmail,
																							_alertConst.GetName(log.AlertLevel),
																							_alertConst.GetName(log.AlertStatus),
																							FormatHelper.ToDateString(log.HandleDate),
																							FormatHelper.ToTimeString(log.HandleTime)
																							}
																				);
				
			return ur;

        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
			return  _facade.QueryAlertHandleLog(this._alertID);
        }
        #endregion    

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			Response.Redirect("FAlertMP.aspx");
		}

      }

}