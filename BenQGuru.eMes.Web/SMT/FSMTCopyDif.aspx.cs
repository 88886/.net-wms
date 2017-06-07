#region System
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
using System.Text;
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;
#endregion


namespace BenQGuru.eMES.Web.SMT
{
	/// <summary>
	/// FSMTCopyDif ��ժҪ˵����
	/// </summary>
	public class FSMTCopyDif : BasePage
	{
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdSave;
		protected Infragistics.WebUI.UltraWebGrid.UltraWebGrid gridWebGrid;

		private GridHelper gridHelper = null;
		private ButtonHelper buttonHelper = null;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected System.Web.UI.WebControls.CheckBox chbSelectAll;
		protected System.Web.UI.WebControls.Label lblCopyAlert;
		protected System.Web.UI.WebControls.CheckBoxList chklstOPControlEdit;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdCopy;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdReturn;
		private ArrayList arrayDeleteRoutes = new ArrayList();
		private BenQGuru.eMES.SMT.SMTFacade _facade;// = new SMTFacadeFactory(base.DataProvider).Create();
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			InitHanders();
			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
				// ��ʼ������UI
				this.InitUI();
				InitParameters();
				this.InitWebGrid();
				RequestData();
			}
		}

		private string EditGrid 
		{
			get 
			{
				return (string) this.ViewState["editgrid"];
			}
			set
			{
				this.ViewState["editgrid"] = value;
			}
		}

		private void InitParameters()
		{
			if(this.Request.Params["itemcode"] == null)
			{
				ExceptionManager.Raise(this.GetType().BaseType,"$Error_RequestUrlParameter_Lost");
			}
			else
			{
				this.ViewState["itemcode"] = this.Request.Params["itemcode"];
			}
		}

		public string ItemCode
		{
			get
			{
				return (string) this.ViewState["itemcode"];
			}
		}





		private void InitHanders()
		{
			this.gridHelper = new GridHelper(this.gridWebGrid);
			this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadDataSource);
			this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridRow);

			this.buttonHelper = new ButtonHelper(this);

		}

		protected Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return null;
		}


		private void InitWebGrid()
		{
			this.gridHelper.AddColumn( "RouteCode", "����;�̴���",	null);
			this.gridHelper.AddColumn( "RouteDescription", "����;������",	null);
			this.gridHelper.AddColumn( "EffectiveDate", "��Ч����",	null);
			this.gridHelper.AddColumn( "InvalidDate", "ʧЧ����",	null);
			this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this.gridHelper.AddColumn( "IsReference", "������",	null);
			this.gridHelper.AddColumn( "IsOPBOMUsed", "�Ƿ�OPBOMʹ��",	null);

			this.gridWebGrid.Columns.FromKey("IsReference").Hidden = true;
			this.gridWebGrid.Columns.FromKey("IsOPBOMUsed").Hidden = true;
			this.gridWebGrid.Columns.FromKey("EffectiveDate").Hidden = true;
			this.gridWebGrid.Columns.FromKey("InvalidDate").Hidden = true;

			this.gridHelper.AddDefaultColumn( true, false );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
			EditGrid = "gridWebGrid";
		}

		private object[] LoadDataSource(int inclusive, int exclusive)
		{
			return null;
		}

		private int GetRowCount()
		{
			return 0;
		}

		private void RequestData()
		{
		
		}

		private object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
		{	
			return null;
		}




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
			this.chklstOPControlEdit.Load += new System.EventHandler(this.chklstOPControlEdit_Load);
			this.cmdCopy.ServerClick += new System.EventHandler(this.cmdCopy_ServerClick);
			this.cmdReturn.ServerClick += new System.EventHandler(this.cmdReturn_ServerClick);

		}
		#endregion

		private void cmdSave_ServerClick(object sender, System.EventArgs e)
		{
			
			
		}

		private void RegistScript()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append ("<script language=\"javascript\">");
			stringBuilder.Append ("document.parentWindow.parent.location.reload(); ");
			stringBuilder.Append ("</script>");
			this.RegisterClientScriptBlock ("refresh", stringBuilder.ToString());
		}


		
		private void chklstOPControlEdit_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{LoadSameResCodes();}
		}

		#region ������Ӧ�¼�

		//����Ҫ���ǵĻ�̨
		private void LoadSameResCodes()
		{
			if(Session["HT"] != null)
			{
				Hashtable sessionHT = (Hashtable)Session["HT"];
				ArrayList sameResCodeList = (ArrayList)sessionHT["SameResourceCode"];
				foreach(string rescode in sameResCodeList)
				{
					this.chklstOPControlEdit.Items.Add(new ListItem(rescode,rescode));
				}
				foreach(ListItem item in this.chklstOPControlEdit.Items)
				{
					item.Selected = true;
				}
				this.lblCopyAlert.Text = "Ŀ�깤���»�̨���з�������,�Ƿ񸲸�?";
			}
		}

		private void cmdCopy_ServerClick(object sender, System.EventArgs e)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			Hashtable sessionHT = (Hashtable)Session["HT"];
			ArrayList diffResCodeList = (ArrayList)sessionHT["DiffResourceCode"];
			string toMOCode = (string)sessionHT["ToMOCode"];
			string FromMOCode = (string)sessionHT["FromMOCode"];

			foreach(ListItem item in this.chklstOPControlEdit.Items)
			{
				if(item.Selected){diffResCodeList.Add(item.Value); }
			}
			if(diffResCodeList.Count != 0)
			{
				_facade.CopySMTResourceBOM(FromMOCode,toMOCode,diffResCodeList);
			}
			Session.Remove("HT");
			this.ExecuteClientFunction("FClose","");
		}

		private void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			Session.Remove("HT");
			this.ExecuteClientFunction("Close","");
		}



		#region ˽�з���

		/// <summary>
		/// ִ�пͻ��˵ĺ���
		/// </summary>
		/// <param name="FunctionName">������</param>
		/// <param name="FunctionParam">����</param>
		/// <param name="Page">��ǰҳ�������</param>
		public  void ExecuteClientFunction(string FunctionName,string FunctionParam)
		{
			try
			{
				string _msg = string.Empty;
				if(FunctionParam != string.Empty)
					_msg = string.Format("<script language='JavaScript'>  {0}('{1}');</script>",FunctionName,FunctionParam);
				else
					_msg = string.Format("<script language='JavaScript'>  {0}();</script>",FunctionName);

				//��Keyֵ��Ϊ�����,��ֹ�ű��ظ�
				Page.RegisterStartupScript(Guid.NewGuid().ToString(),_msg);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private void Alert(string msg)
		{
			msg = msg.Replace("'","");
			msg = msg.Replace("\r","");
			msg = msg.Replace("\n","");
			string _msg = string.Format("<script language='JavaScript'>  alert('{0}');</script>",msg);
			Page.RegisterStartupScript("",_msg);
		}

		#endregion

		

		#endregion
	}
}
