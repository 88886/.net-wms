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
using BenQGuru.eMES.TSModel;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.TS;

namespace BenQGuru.eMES.Web.TSModel
{
    public partial class FTSSmartQueryQP : BaseMPage
    {		
        private System.ComponentModel.IContainer components;
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		
        protected BenQGuru.eMES.TS.TSFacade _facade;
	
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
            this.cmdReturn.Attributes["onclick"] = "javascript:history.go(-1);return false;";
			if( !this.IsPostBack )
			{
                if (this.GetRequestParam("sequence") == "")
                    throw new Exception("$Error_RequestUrlParameter_Lost");
                BenQGuru.eMES.TSModel.TSModelFacade tsModelFacade = new TSModelFacade(this.DataProvider);
                TSSmartConfig smartCfg = (TSSmartConfig)tsModelFacade.GetTSSmartConfig(decimal.Parse(this.GetRequestParam("sequence")));
                if (smartCfg == null)
                    throw new Exception("$Error_RequestUrlParameter_Lost");
                this.txtErrorCodeQuery.Text = smartCfg.ErrorCode;

				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
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
            this.gridHelper.AddColumn("ErrorCauseGroup", "����ԭ����", null);
            this.gridHelper.AddColumn("ErrorCauseCode", "����ԭ�����", null);
            this.gridHelper.AddColumn("ErrorLocation", "����λ��", null);
            this.gridHelper.AddColumn("ErrorPart", "����Ԫ��", null);
            this.gridHelper.AddColumn("LastCollectDate", "���ɼ�����", null);
            this.gridHelper.AddColumn("CollectCount", "�ɼ�����", null);

            this.gridHelper.AddDefaultColumn( false, false );
            
            this.gridHelper.ApplyLanguage( this.languageComponent1 );

            this.gridHelper.RefreshData();
        }
		
        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
            TSSmartErrorCause smartErrorCause = (TSSmartErrorCause)obj;
            string strLocations = "";
            if (smartErrorCause.Locations != null)
            {
                for (int i = 0; i < smartErrorCause.Locations.Length; i++)
                {
                    strLocations += smartErrorCause.Locations[i].ErrorLocation + "; ";
                }
                if (strLocations.Length > 0)
                    strLocations = strLocations.Substring(0, strLocations.Length - 2);
            }
            string strErrorParts = "";
            if (smartErrorCause.ErrorParts != null)
            {
                for (int i = 0; i < smartErrorCause.ErrorParts.Length; i++)
                {
                    strErrorParts += smartErrorCause.ErrorParts[i].ErrorPart + "; ";
                }
                if (strErrorParts.Length > 0)
                    strErrorParts = strErrorParts.Substring(0, strErrorParts.Length - 2);
            }
            return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
                new object[]{
                                smartErrorCause.ErrorCauseGroupCode,
                                smartErrorCause.ErrorCauseCode,
                                strLocations,
                                strErrorParts,
                                FormatHelper.ToDateString(smartErrorCause.MaintainDate),
                                smartErrorCause.Count
                });
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            if (_facade == null) { _facade = new TSFacade(this.DataProvider); }
            object[] objs = this._facade.QueryErrorCodeSmartTS(this.txtErrorCodeQuery.Text);
            if (objs != null)
                this.gridHelper.PagerToolBar.RowCount = objs.Length;
            return objs;
        }


        protected override int GetRowCount()
        {
            return 0;
        }

        #endregion
        
        #region Export 

        protected override string[] FormatExportRecord( object obj )
        {
            TSSmartErrorCause smartErrorCause = (TSSmartErrorCause)obj;
            string strLocations = "";
            if (smartErrorCause.Locations != null)
            {
                for (int i = 0; i < smartErrorCause.Locations.Length; i++)
                {
                    strLocations += smartErrorCause.Locations[i].ErrorLocation + "; ";
                }
                if (strLocations.Length > 0)
                    strLocations = strLocations.Substring(0, strLocations.Length - 2);
            }
            string strErrorParts = "";
            if (smartErrorCause.ErrorParts != null)
            {
                for (int i = 0; i < smartErrorCause.ErrorParts.Length; i++)
                {
                    strErrorParts += smartErrorCause.ErrorParts[i].ErrorPart + "; ";
                }
                if (strErrorParts.Length > 0)
                    strErrorParts = strErrorParts.Substring(0, strErrorParts.Length - 2);
            }
            return new string[]{
                                smartErrorCause.ErrorCauseGroupCode,
                                smartErrorCause.ErrorCauseCode,
                                strLocations,
                                strErrorParts,
                                FormatHelper.ToDateString(smartErrorCause.MaintainDate),
                                smartErrorCause.Count.ToString()
                               };
        }

        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"ErrorCauseGroup",
                                    "ErrorCauseCode",
                                    "ErrorLocation",
                                    "ErrorPart",
                                    "LastCollectDate",
                                    "CollectCount" };
        }
        #endregion

    }


}