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
using BenQGuru.eMES.AlertModel;
#endregion

namespace BenQGuru.eMES.Web.Alert
{
	/// <summary>
	/// ItemMP ��ժҪ˵����
	/// </summary>
	public partial class FAlertBillEP : BasePage
	{
		#region ��������
		protected System.Web.UI.WebControls.Label lblItemTypeQuery;
		private System.ComponentModel.IContainer components;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected System.Web.UI.WebControls.Label lblItemNameQuery;
		protected System.Web.UI.WebControls.Label lblAlert;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox Textbox1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox Textbox2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList Dropdownlist1;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.TextBox Textbox3;
		protected System.Web.UI.WebControls.TextBox Textbox4;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox Textbox5;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.DropDownList Dropdownlist2;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox Textbox6;
		protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;
		protected System.Web.UI.HtmlControls.HtmlInputButton Submit2;
		protected System.Web.UI.WebControls.DropDownList DropDownList3;
		protected System.Web.UI.WebControls.TextBox TextBox7;
		protected System.Web.UI.WebControls.TextBox TextBox8;
		protected System.Web.UI.WebControls.Label Lab;
		protected BenQGuru.eMES.Web.UserControl.eMESDate dateValidDate;
		private BenQGuru.eMES.AlertModel.AlertBillFacade m_alertBillFacade;
		protected BenQGuru.eMES.BaseSetting.UserFacade m_userfacade;
		//string _itemcode;
		string _alerttype;
		/// <summary>
		/// Ԥ������Ƿ�����Դ������
		/// </summary>
		private bool isResourceNG = false;

		BenQGuru.eMES.Web.Alert.AlertConst _alertConst;

		private int _billId;
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
			this.cmdAddUser.Attributes["onclick"] = "document.getElementById('stbUser$ctl02').click();";
			this.cmdSave.Attributes["onclick"] = "if(document.getElementById('lstUser').innerText != '' && document.getElementById('chbMailNotify').checked == false)"
												+ " { return confirm('"
												+ this.languageComponent1.GetString("$If_AlertBill_Mail")//"ѡ�����ʼ������ˣ���û�й�ѡ�ʼ�֪ͨ���Ƿ����?"
												+ "');}";
			//_itemcode = Request.QueryString["itemcode"];
			_alerttype = Request.QueryString["alerttype"];
			//_alertitem = Request.QueryString["alertitem"];
			_billId = int.Parse( Request.QueryString["billid"].ToString());

            if (this._alerttype == AlertType_Old.ResourceNG) { isResourceNG = true; }
			//if(isResourceNG)
			//{
			//	_rescode = Request.QueryString["rescode"];
			//	_ecg2ec = Request.QueryString["ecg2ec"];
			//}

			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.lblAlertInfo.Text = this._alertConst.GetTemplateHelp(this.languageComponent1);
				LoadData();
			}
		}

		BenQGuru.eMES.AlertModel.AlertBillFacade _alertBillFacade
		{
			get{
				if(m_alertBillFacade == null)
					m_alertBillFacade = new BenQGuru.eMES.AlertModel.AlertBillFacade(DataProvider);

				return m_alertBillFacade;
			}
		}

		BenQGuru.eMES.BaseSetting.UserFacade _userfacade
		{
			get{
				if(m_userfacade == null)
					m_userfacade = new BenQGuru.eMES.BaseSetting.UserFacade(this.DataProvider);

				return m_userfacade;
			}
		}
		private void LoadData()
		{
            if (this._alerttype == AlertType_Old.First || this._alerttype == AlertType_Old.ResourceNG)
				this.txtStartNum.ReadOnly = true;

			
			if(isResourceNG)
			{
				
				this.LoadResData();
				return;
			}

			this.ExecuteClientFunction("setResUnDisplay","");
			BenQGuru.eMES.Domain.Alert.AlertBill alert = (BenQGuru.eMES.Domain.Alert.AlertBill)this._alertBillFacade.GetAlertBill(this._billId);
			if(alert != null)
			{
				this.ViewState["_itemcode"] = alert.ItemCode;
				_alerttype = alert.AlertType;
				this.ViewState["_alertitem"] = alert.AlertItem;

				this.txtItemCode.Text = alert.ItemCode;
				this.txtAlertItem.Text = AlertMsg.GetAlertName(alert.AlertItem,this.languageComponent1);
				this.txtAlertType.Text = AlertMsg.GetAlertName(alert.AlertType,this.languageComponent1);
				
				this.txtItemCodeQuery.Text = alert.ProductCode;

				//�׼�����ֻ�д��ڵ��ڲ���Ч
                if (this._alerttype == AlertType_Old.First)
				{
					this.txtStartNum.Text = string.Empty;
					this.drpOperator.Items.RemoveAt(0);
					this.drpOperator.Items.RemoveAt(0);
				}
				else
					this.txtStartNum.Text = NumberHelper.TrimZero(alert.StartNum);

				this.drpOperator.SelectedValue = alert.Operator;
                if (this.drpOperator.SelectedValue != Operator_Old.BW)
				{
					this.lblAnd.Visible = false;
					this.txtUp.Visible = false;
				}
				this.txtLow.Text = NumberHelper.TrimZero(alert.LowValue);
				this.txtUp.Text = NumberHelper.TrimZero(alert.UpValue);
				this.dateValidDate.Text = FormatHelper.ToDateString(alert.ValidDate);
				this.txtAlertMsg.Text = alert.AlertMsg;
				this.txtDesc.Text = alert.Description;
				this.chbMailNotify.Checked=(alert.MailNotify=="Y");
				//load �û�
				//if(this.chbMailNotify.Checked)
				//{
					object[] objs = _alertBillFacade.QueryAlertNotifier(_billId);
					if(objs != null)
					{
						foreach(object obj in objs)
						{
							BenQGuru.eMES.Domain.Alert.AlertNotifier notifier  = obj as BenQGuru.eMES.Domain.Alert.AlertNotifier;
							if(notifier == null )
								continue;

							BenQGuru.eMES.Domain.BaseSetting.User user = _userfacade.GetUser(notifier.UserCode) as BenQGuru.eMES.Domain.BaseSetting.User;
							if(user!= null && user.UserEmail != null)
							{
								this.lstUser.Items.Add(new ListItem(user.UserCode + "(" + user.UserEmail + ")",user.UserCode));
							}
							else //û�ҵ������ֶ�������û�
							{
								this.lstUser.Items.Add(new ListItem(notifier.UserCode + "(" +notifier.EMail + ")",notifier.UserCode));
							}
						}
					}
				//}
			}
			
		}

		private void LoadResData()
		{
			this.ExecuteClientFunction("setResDisplay","");
			BenQGuru.eMES.Domain.Alert.AlertResBill alert = (BenQGuru.eMES.Domain.Alert.AlertResBill)this._alertBillFacade.GetAlertResBill(this._billId);
			if(alert != null)
			{
				this.ViewState["_itemcode"] = alert.ItemCode;
				_alerttype = alert.AlertType;
				this.ViewState["_alertitem"] = alert.AlertItem;
				this.ViewState["_rescode"] = alert.ResourceCode;
				this.ViewState["._ecg2ec"] = alert.ErrorGroup2Code;

				this.txtResource.Text = this.ViewState["_rescode"].ToString();
				this.txtEcg2Ec.Text = this.ViewState["._ecg2ec"].ToString();
				this.txtItemCode.Text = alert.ItemCode;
				this.txtAlertItem.Text = AlertMsg.GetAlertName(alert.AlertItem,this.languageComponent1);
				this.txtAlertType.Text = AlertMsg.GetAlertName(alert.AlertType,this.languageComponent1);

				//������Դֻ�д��ڵ��ڲ���Ч
                if (this._alerttype == AlertType_Old.ResourceNG)
				{
					this.txtStartNum.Text = string.Empty;
					this.drpOperator.Items.RemoveAt(0);
					this.drpOperator.Items.RemoveAt(0);
				}
				else
					this.txtStartNum.Text = NumberHelper.TrimZero(alert.StartNum);

				this.drpOperator.SelectedValue = alert.Operator;
                if (this.drpOperator.SelectedValue != Operator_Old.BW)
				{
					this.lblAnd.Visible = false;
					this.txtUp.Visible = false;
				}
				this.txtLow.Text = NumberHelper.TrimZero(alert.LowValue);
				this.txtUp.Text = NumberHelper.TrimZero(alert.UpValue);
				this.dateValidDate.Text = FormatHelper.ToDateString(alert.ValidDate);
				this.txtAlertMsg.Text = alert.AlertMsg;
				this.txtDesc.Text = alert.Description;
				this.chbMailNotify.Checked=(alert.MailNotify=="Y");
				//load �û�
				//if(this.chbMailNotify.Checked)
				//{
				object[] objs = _alertBillFacade.QueryAlertNotifier(_billId);
				if(objs != null)
				{
					foreach(object obj in objs)
					{
						BenQGuru.eMES.Domain.Alert.AlertNotifier notifier  = obj as BenQGuru.eMES.Domain.Alert.AlertNotifier;
						if(notifier == null )
							continue;

						BenQGuru.eMES.Domain.BaseSetting.User user = _userfacade.GetUser(notifier.UserCode) as BenQGuru.eMES.Domain.BaseSetting.User;
						if(user!= null && user.UserEmail != null)
						{
							this.lstUser.Items.Add(new ListItem(user.UserCode + "(" + user.UserEmail + ")",user.UserCode));
						}
						else //û�ҵ������ֶ�������û�
						{
							this.lstUser.Items.Add(new ListItem(notifier.UserCode + "(" +notifier.EMail + ")",notifier.UserCode));
						}
					}
				}
				//}
			}
		}
		protected ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}

		protected void cmdSave_ServerClick(object sender, System.EventArgs e)
		{
			if(this.ValidateInput())
			{
				SaveData();
				cmdReturn_ServerClick(null,null);
			}
		}
		
		private void SaveData()
		{
			try
			{
				DataProvider.BeginTransaction();

				BenQGuru.eMES.Domain.Alert.AlertBill tempAlert =null;
				if(isResourceNG)
				{
					BenQGuru.eMES.Domain.Alert.AlertResBill alert = (BenQGuru.eMES.Domain.Alert.AlertResBill)this._alertBillFacade.GetAlertResBill(this._billId);

					//alert.BillId = this._billId;
					//alert.ItemCode = _itemcode;
					//alert.ResourceCode = _rescode;
					//alert.ErrorGroup2Code = _ecg2ec;
					//alert.AlertItem = this._alertitem;
					//alert.AlertType = this._alerttype;
					alert.AlertMsg  = this.txtAlertMsg.Text;
					alert.Description = this.txtDesc.Text;
				
					alert.Operator = this.drpOperator.SelectedValue;
					alert.StartNum = int.Parse(this.txtStartNum.Text==""?"0":this.txtStartNum.Text);
					alert.MailNotify = this.chbMailNotify.Checked?"Y":"N";
					alert.LowValue = decimal.Parse(this.txtLow.Text);			
					alert.UpValue = decimal.Parse(this.txtUp.Text==""?"0":this.txtUp.Text);
					alert.ValidDate = BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(this.dateValidDate.Text);
					alert.MaintainDate = BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(DateTime.Now.ToShortDateString());
					alert.MaintainUser = this.GetUserCode();
					alert.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
					tempAlert = (BenQGuru.eMES.Domain.Alert.AlertBill)alert;
					_alertBillFacade.UpdateAlertResBill(alert);
				}
				else
				{
					BenQGuru.eMES.Domain.Alert.AlertBill alert = (BenQGuru.eMES.Domain.Alert.AlertBill)this._alertBillFacade.GetAlertBill(this._billId);

					//alert.BillId = this._billId;
					//alert.ItemCode = _itemcode;
					//alert.AlertItem = this._alertitem;
					//alert.AlertType = this._alerttype;
					alert.AlertMsg  = this.txtAlertMsg.Text;
					alert.Description = this.txtDesc.Text;
				
					alert.Operator = this.drpOperator.SelectedValue;
					alert.StartNum = int.Parse(this.txtStartNum.Text==""?"0":this.txtStartNum.Text);
					alert.MailNotify = this.chbMailNotify.Checked?"Y":"N";
					alert.LowValue = decimal.Parse(this.txtLow.Text);			
					alert.UpValue = decimal.Parse(this.txtUp.Text==""?"0":this.txtUp.Text);
					alert.ValidDate = BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(this.dateValidDate.Text);
					alert.MaintainDate = BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(DateTime.Now.ToShortDateString());
					alert.MaintainUser = this.GetUserCode();
					alert.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
					tempAlert = alert;
					_alertBillFacade.UpdateAlertBill(alert);

				}
				//���ӽ�����
				if(tempAlert != null)
				{
					object[] objs = _alertBillFacade.QueryAlertNotifier(tempAlert.BillId);
					if(objs != null)
					{
						foreach(object obj in objs)
						{
							BenQGuru.eMES.Domain.Alert.AlertNotifier an = obj as BenQGuru.eMES.Domain.Alert.AlertNotifier;
							if(an != null)
								_alertBillFacade.DeleteAlertNotifier(an);
						}
					}
				}

				//if(this.chbMailNotify.Checked)
				//{
					for(int i=0;i<this.lstUser.Items.Count;i++)
					{
						if(this.lstUser.Items[i].Value != string.Empty)
						{
							BenQGuru.eMES.Domain.Alert.AlertNotifier user = new BenQGuru.eMES.Domain.Alert.AlertNotifier();
							user.AlertItem = tempAlert.AlertItem;
							user.AlertType = tempAlert.AlertType;
							user.ItemCode  = tempAlert.ItemCode;
							user.UserCode= this.lstUser.Items[i].Value;
							user.EMail = GetEMail(this.lstUser.Items[i].Text);
							user.BillId = tempAlert.BillId;
							if(_alertBillFacade.GetAlertNotifier(user.UserCode,user.BillId) == null)
							_alertBillFacade.AddAlertNotifier(user);
						}
					}
				//}

				DataProvider.CommitTransaction();
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				throw ex;
			}
		}
		
		//��adb(abc@asd.com)��ȡ��email
		private string GetEMail(string str)
		{
			int i = str.IndexOf("(");
			return str.Substring(i + 1,str.Length - (i + 1 + 1));
		}
		protected bool ValidateInput()
		{
			string ie = BenQGuru.eMES.Web.Helper.MessageCenter.ParserMessage("$Error_Input_Empty",this.languageComponent1);
			if(this.chbMailNotify.Checked && this.lstUser.Items.Count == 0)
			{
				WebInfoPublish.Publish(this,this.lblReceiver.Text + " " + ie,this.languageComponent1);
				return false;
			}
			PageCheckManager manager = new PageCheckManager();

            if (this._alerttype != BenQGuru.eMES.AlertModel.AlertType_Old.First && this._alerttype != BenQGuru.eMES.AlertModel.AlertType_Old.ResourceNG) //��������׼�����
				manager.Add(new NumberCheck(this.lblStartNum,this.txtStartNum,1,int.MaxValue,true));
			
			if(this.drpOperator.SelectedValue == "BW")
			{
				manager.Add(new DecimalRangeCheck(this.lblAlertCondition,this.txtLow.Text,this.lblAlertCondition,this.txtUp.Text,true));
			}
			else
			{
				manager.Add(new DecimalCheck(this.lblAlertCondition,this.txtLow,true));
			}
			
			manager.Add(new LengthCheck(this.lblAlertMsg,this.txtAlertMsg,1000,true));
			
			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
				return false;
			}

			return true;
		}

		protected void cmdAddUser_Click(object sender, System.EventArgs e)
		{
			BenQGuru.eMES.BaseSetting.UserFacade  userFacade =null;
			string[] users = this.stbUser.Text.Trim().Split(',');
			if(users.Length > 0)
				userFacade = new BenQGuru.eMES.BaseSetting.UserFacade(this.DataProvider);

			for(int i=0;i<users.Length;i++)
			{
				if(this.lstUser.Items.FindByValue(users[i]) == null)
				{
					BenQGuru.eMES.Domain.BaseSetting.User user = (BenQGuru.eMES.Domain.BaseSetting.User)userFacade.GetUser(users[i]);
					if(user != null)
						lstUser.Items.Add(new ListItem(user.UserCode + "(" + user.UserEmail + ")",user.UserCode));
				}
					
			}
		}

		protected void cmdDeleteUser_Click(object sender, System.EventArgs e)
		{
			this.lstUser.Items.Remove(this.lstUser.SelectedItem);
			this.stbUser.Text = string.Empty;
			string[] users = new string[this.lstUser.Items.Count];
			for(int i = 0;i<this.lstUser.Items.Count;i++)
			{
				users[i] = this.lstUser.Items[i].Value;
			}

			this.stbUser.Text = String.Join(",",users);
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("FAlertBillMP.aspx"));
		}

		protected void drpOperator_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(this.drpOperator.SelectedValue==BenQGuru.eMES.AlertModel.Operator_Old.BW)
			{
				this.lblAnd.Visible = true;
				this.txtUp.Visible = true;
			}
			else
			{
				this.lblAnd.Visible = false;
				this.txtUp.Visible = false;
			}

			SetAlertMsg();//����AlertMessage
		}

		private void SetAlertMsg()
		{
			string msg = string.Empty;
			if(this._alerttype == BenQGuru.eMES.AlertModel.AlertType_Old.CPK)
				msg = msg + _alertConst.CPKTESTITEM_TEMP + ",";

			 msg = msg +this.txtAlertType.Text + _alertConst.OVERFLOW_TEMP + ","
				+ _alertConst.STANDARD_TEMP + (this.drpOperator.SelectedValue==BenQGuru.eMES.AlertModel.Operator_Old.BW?_alertConst.CONDITIONVALUE2_TEMP:"")
				+ "," + _alertConst.DATAVALUE_TEMP;

			msg = _alertConst.GetTemplateName(this.ViewState["_alertitem"].ToString()) + "," + msg;
			
			this.txtAlertMsg.Text = msg;
		}

		//���ֶ�������û�����͵����ʼ�����
		protected void btnAdd_Click(object sender, System.EventArgs e)
		{
			if(this.txtUser.Text.Trim() == string.Empty)
			{
				WebInfoPublish.Publish(this, lblUserSN.Text + BenQGuru.eMES.Web.Helper.MessageCenter.ParserMessage("$Error_Input_Empty",this.languageComponent1),
					this.languageComponent1);
				return;	
			}

			if(this.txtEMail.Text.Trim() == string.Empty)
			{
				WebInfoPublish.Publish(this, lblEMail.Text + BenQGuru.eMES.Web.Helper.MessageCenter.ParserMessage("$Error_Input_Empty",this.languageComponent1),
					this.languageComponent1);
				return;	
			}

			if(this.txtEMail.Text.IndexOf("@") == -1)
			{	
				WebInfoPublish.Publish(this, "�����ʼ���ʽ�������",this.languageComponent1);
				return;
			}

			//����û������Ƿ��Ѿ�����
			if(this.lstUser.Items.FindByValue(this.txtUser.Text.Trim()) != null)
			{
				WebInfoPublish.Publish(this, "�û������ظ�",this.languageComponent1);
				return;
			}
			ListItem  li = new ListItem(this.txtUser.Text.Trim() + "(" + this.txtEMail.Text.Trim() + ")",this.txtUser.Text.Trim());
			this.lstUser.Items.Add(li);
		}

		
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
	}
}
