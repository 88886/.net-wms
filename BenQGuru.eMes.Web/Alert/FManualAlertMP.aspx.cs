#region system
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
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Alert;
using BenQGuru.eMES.AlertModel;
#endregion

namespace BenQGuru.eMES.Web.Alert
{
	/// <summary>
	/// ItemMP ��ժҪ˵����
	/// </summary>
	public partial class FManualAlertMP : BaseMPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdSave;
		private System.ComponentModel.IContainer components;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdCancel;

		private BenQGuru.eMES.AlertModel.AlertFacade m_alertFacade;
		private AlertConst _alertConst;

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

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_alertConst = new AlertConst(this.languageComponent1);
			
			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				AlertLevelBuilder.Build(this.drpAlertLevel.Items,this._alertConst);
				this.drpAlertLevel.Items.Insert(0,(new ListItem(_alertConst.GetName("*"),"*")));
			}
		}

		BenQGuru.eMES.AlertModel.AlertFacade _alertFacade
		{
			get
			{
				if(m_alertFacade == null)
					m_alertFacade = new BenQGuru.eMES.AlertModel.AlertFacade(DataProvider);

				return m_alertFacade;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(Session["ss_action"] != null
				&&
				Session["ss_action"].ToString() == PageActionType.Add)
			{
				this.gridHelper.RequestData();
			}
			else if(Session["ss_action"] != null
				&&
				Session["ss_action"].ToString() == PageActionType.Update)
			{
				if(Session["ss_alertlevel"] != null)
				{
					this.drpAlertLevel.SelectedValue = Session["ss_alertlevel"].ToString();	
				}
				this.gridHelper.RequestData();
			}

			Session["ss_action"] = string.Empty;
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}
		

		#region WebGrid
		protected override void Grid_ClickCell(UltraGridCell cell)
		{

			base.Grid_ClickCell (cell);
			if(this.gridHelper.IsClickEditColumn(cell))
			{
				Session["ss_action"] = PageActionType.Update;
				Session["ss_alertlevel"] = this.drpAlertLevel.SelectedValue;

				Response.Redirect(this.MakeRedirectUrl ("FManualAlertEP.aspx",new string[] {"action","alertid"},new string[] {"edit",cell.Row.Cells[1].Value.ToString()}));
			}
		}

		protected override void Grid_DblClick(object sender, ClickEventArgs e)
		{
			Infragistics.WebUI.UltraWebGrid.UltraGridRow Row = e.Row;
			if(Row != null)
			{
				Session["ss_action"] = PageActionType.Update;
				Session["ss_alertlevel"] = this.drpAlertLevel.SelectedValue;

				Response.Redirect(this.MakeRedirectUrl ("FManualAlertEP.aspx",new string[] {"action","alertid"},new string[] {"edit",Row.Cells[1].Value.ToString()}));
			}
		}

		protected override void InitWebGrid()
		{
			this.gridHelper.AddColumn("AlertID",       "AlertID",   null);
			this.gridHelper.AddColumn( "AlertLevel",   "Ԥ������",  null);
			this.gridHelper.AddColumn( "AlertMsg",     "�¼�����",	null);
			this.gridHelper.AddColumn( "AlertStatus",  "Ԥ��״̬",	null);
			this.gridHelper.AddColumn( "MainNotified", "�Ƿ��ʼ�֪ͨ",	null);
			this.gridHelper.AddColumn( "SendUser",     "������",	null);
			this.gridHelper.AddColumn( "AlertDate",    "Ԥ������",	null);
			this.gridHelper.AddColumn( "AlertTime",    "Ԥ��ʱ��",	null);
			this.gridHelper.AddColumn( "MaintainUser", "ά����Ա",	null);
			this.gridHelper.AddDefaultColumn( true, true );
            
			this.gridHelper.ApplyLanguage( this.languageComponent1 );

			this.GetWebGrid().Columns.FromKey("AlertID").Hidden = true;

			//this.gridHelper.RequestData();
		}
		
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			BenQGuru.eMES.Domain.Alert.Alert alert = obj as BenQGuru.eMES.Domain.Alert.Alert;
			if(alert != null)
			{
				Infragistics.WebUI.UltraWebGrid.UltraGridRow ur =  new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
																										new object[]{"false",
																										alert.AlertID.ToString(),
																										_alertConst.GetName(alert.AlertLevel),
																										alert.AlertMsg,
																										_alertConst.GetName(alert.AlertStatus),
																										alert.MailNotify=="Y"?"��":"��",
																										alert.SendUser,
																										FormatHelper.ToDateString(alert.AlertDate),
																										FormatHelper.ToTimeString(alert.AlertTime),
																										alert.MaintainUser
																										});
				ur.Cells[2].Style.ForeColor = ColorHelper.GetColor(alert.AlertLevel);
				ur.Cells[4].Style.ForeColor = ColorHelper.GetColor(alert.AlertStatus);

				return ur;
			}
				
			else
				return null;
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			this._alertFacade.DeleteAlertAndRelation(domainObjects);
			try
			{
				this.DataProvider.BeginTransaction();

				for(int i=0;i<domainObjects.Count;i++)
				{
					BenQGuru.eMES.Domain.Alert.Alert alert = domainObjects[i] as BenQGuru.eMES.Domain.Alert.Alert;
					if(alert != null)
					{
						
					}
	
					_alertFacade.DeleteAlert(alert);
				}
				
				DataProvider.CommitTransaction();
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				throw ex;
			}
		}

		protected override object GetEditObject(UltraGridRow row)
		{
			BenQGuru.eMES.Domain.Alert.Alert alert = new BenQGuru.eMES.Domain.Alert.Alert();
			alert.AlertID = int.Parse(row.Cells[1].Value.ToString());
			return alert;
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			return _alertFacade.QueryManualAlert(this.drpAlertLevel.SelectedValue,inclusive,exclusive);
		}


		protected override int GetRowCount()
		{
			return _alertFacade.QueryManualAlertCount(this.drpAlertLevel.SelectedValue);
		}

		#endregion

        
		#region Export 

		protected override string[] FormatExportRecord( object obj )
		{
			BenQGuru.eMES.Domain.Alert.Alert alert = obj as BenQGuru.eMES.Domain.Alert.Alert;
			if(alert != null)
			{
				
				return new string[]{
									   _alertConst.GetName(alert.AlertLevel),
									   alert.AlertMsg,
										_alertConst.GetName(alert.AlertStatus),
									   alert.MailNotify=="Y"?"��":"��",
										alert.SendUser,
										FormatHelper.ToDateString(alert.AlertDate),
										FormatHelper.ToTimeString(alert.AlertTime),
										alert.MaintainUser
								   };
			}
			else
				return null;
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] { "Ԥ������","�¼�����", "Ԥ��״̬", "�Ƿ��ʼ�֪ͨ","������","Ԥ������","Ԥ��ʱ��","ά����Ա"};
		}						   
		#endregion				 	
			
		protected void cmdAdd_ServerClick(object sender, System.EventArgs e)
		{
			
			Session["ss_action"] = PageActionType.Add;
			this.Response.Redirect("FManualAlertEP.aspx?action=add");
		}						 	
	}
}
