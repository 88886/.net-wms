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
    public partial class FAlertSampleMP : BaseMPage
    {		
        private System.ComponentModel.IContainer components;
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected System.Web.UI.WebControls.Label lblItemNameEdit;
		
		private AlertFacade m_facade;

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
		}

		AlertFacade _facade
		{
			get{
				if(m_facade == null)
					m_facade = new AlertFacade(DataProvider);

				return m_facade;
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
			this.gridHelper.AddColumn("OID","OID",null);
            this.gridHelper.AddColumn( "SampleDesc", "��������",	null);
			this.gridHelper.AddColumn( "MaintainUser", "ά����Ա",	null);
			this.gridHelper.AddColumn( "MaintianDate", "ά������",	null);
		    this.gridHelper.AddDefaultColumn( true, true );
            
            this.gridHelper.ApplyLanguage( this.languageComponent1 );

			this.gridWebGrid.Columns.FromKey("OID").Hidden = true;

			this.gridHelper.RequestData();
        }
		
        protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        {
			AlertSample sample = obj as AlertSample;
			if(obj != null)
			{
				return new UltraGridRow(new object[]{
														"false",
														sample.ID,
														sample.SampleDesc,
														sample.MaintainUser,
														FormatHelper.ToDateString(sample.MaintainDate)
													}
										);
			}
			else
			{
				return null;
			}
        }

        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
          return _facade.QueryAlertSample(inclusive,exclusive);
        }


        protected override int GetRowCount()
        {
           return _facade.QueryAlertSampleCount();
        }

        #endregion

		#region Button

		protected override void AddDomainObject(object domainObject)
		{		
			AlertSample sample = domainObject as AlertSample;
			if(sample != null)
				_facade.AddAlertSample(sample);
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			_facade.DeleteAlertSample((AlertSample[])domainObjects.ToArray(typeof(AlertSample)));	
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			AlertSample sample = domainObject as AlertSample;
			if(sample != null)
				_facade.UpdateAlertSample(sample);
		}

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
			if ( pageAction == PageActionType.Add )
			{
				//this.txtErrorCodeEdit.ReadOnly = false;
			}

			if ( pageAction == PageActionType.Update )
			{
				//this.txtErrorCodeEdit.ReadOnly = true;
			}
		}
		#endregion

        #region Object <--> Page

        protected override object GetEditObject()
        {
			if(	this.ValidateInput())
			{
				AlertSample sample = _facade.CreateNewAlertSample();
				sample.ID = this.txtOID.Text.Trim()==string.Empty?Guid.NewGuid().ToString():this.txtOID.Text.Trim();
				sample.SampleDesc = this.txtSampleDescEdit.Text;
				sample.MaintainDate = FormatHelper.TODateInt(DateTime.Now.ToShortDateString());
				sample.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
				sample.MaintainUser = this.GetUserCode();

				return sample;
			}
			else
				return null;
        }


        protected override object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
        {	
			AlertSample sample = _facade.CreateNewAlertSample();
			sample.ID = row.Cells[1].Value.ToString();
			sample.SampleDesc = row.Cells[2].Value.ToString();
			sample.MaintainUser = row.Cells[3].Value.ToString();
			sample.MaintainDate = FormatHelper.TODateInt(row.Cells[4].Value.ToString());

			return sample;
        }

        protected override void SetEditObject(object obj)
        {
			if (obj == null)
			{
				this.txtOID.Text = String.Empty;
				this.txtSampleDescEdit.Text= string.Empty;
				return;
			}
			else
			{
				AlertSample sample = obj as AlertSample;
				if(sample != null)
				{
					this.txtOID.Text = sample.ID;
					this.txtSampleDescEdit.Text = sample.SampleDesc;
				}
			}
        }

		
        protected override bool ValidateInput()
        {
			PageCheckManager manager = new PageCheckManager();
			manager.Add(new LengthCheck(this.lblSampleDescEdit,this.txtSampleDescEdit,1000,true));
			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
				return false;
			}

			return true;
        }

        #endregion
        
        #region Export 

        protected override string[] FormatExportRecord( object obj )
        {
			AlertSample sample = obj as AlertSample;
			if(sample != null)
				return new string[]{
										sample.SampleDesc,
										sample.MaintainUser,
										FormatHelper.ToDateString(sample.MaintainDate)
					               };
			else
				return null;
        }

        protected override string[] GetColumnHeaderText()
        {
			return new string[] {"��������", "ά����Ա", "ά������"};
		}
		
		#endregion
    }
}