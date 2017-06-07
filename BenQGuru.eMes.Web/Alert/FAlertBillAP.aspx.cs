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
	public partial class FAlertBillAP : BasePage
	{
		#region ��������
		protected System.Web.UI.WebControls.Label lblItemTypeQuery;
		private System.ComponentModel.IContainer components;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		protected System.Web.UI.WebControls.Label lblItemNameQuery;
		protected System.Web.UI.WebControls.Label lblAlert;
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
		protected System.Web.UI.WebControls.Button btnLeft;
		/// <summary>
		/// Ԥ������Ƿ�����Դ������
		/// </summary>
		private bool isResourceNG = false;
		BenQGuru.eMES.Web.Alert.AlertConst _alertConst;
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
			this.cmdAddProduct.Attributes["onclick"] = "document.getElementById('sbProduct$ctl02').click();";
			this.cmdAddItem.Attributes["onclick"] = "document.getElementById('stbItem$ctl02').click();";
			this.cmdAddUser.Attributes["onclick"] = "document.getElementById('stbUser$ctl02').click();";
			this.cmdAddResource.Attributes["onclick"] = "document.getElementById('stbResource$ctl02').click();";
			this.cmdAddErrorCode.Attributes["onclick"] = "document.getElementById('stbErrorCode$ctl02').click();";
			this.cmdSave.Attributes["onclick"] = "if(document.getElementById('lstUser').innerText != '' && document.getElementById('chbMailNotify').checked == false)"
													+ " { return confirm('"
													+ languageComponent1.GetString("$If_AlertBill_Mail")//"ѡ�����ʼ������ˣ���û�й�ѡ�ʼ�֪ͨ���Ƿ����?"
													+ "');}";

			if (!IsPostBack)
			{	
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.lblAlertInfo.Text = this._alertConst.GetTemplateHelp(this.languageComponent1);
				SetAlertMsg();
				this.dateValidDate.Text = DateTime.Now.ToShortDateString();

                OperatorBuilder.Build(this.drpOperator.Items, BenQGuru.eMES.AlertModel.AlertType_Old.NG, this._alertConst);

				this.rdbRes.Checked = true;
				this.rblAlertItem_SelectedIndexChanged(null,null);
			}
			if(this.rblAlertType.SelectedValue == "ResourceNG")
			{
				this.isResourceNG = true;
				this.ExecuteClientFunction("setResDisplay","");
			}
			else
			{
				this.ExecuteClientFunction("setResUnDisplay","");
			}

			//this.stbErrorCode.Tag = "���ǲ��Ե�tag";
		}

		BenQGuru.eMES.AlertModel.AlertBillFacade _alertBillFacade
		{
			get{
				if(m_alertBillFacade == null)
					m_alertBillFacade = new BenQGuru.eMES.AlertModel.AlertBillFacade(DataProvider);

				return m_alertBillFacade;
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
				if(isResourceNG)
				{
					#region ��Դ������Ԥ��
					
					this.AddAlertResBill();
					
					#endregion
				}
				else
				{
					for(int i = 0;i<this.lstItem.Items.Count;i++)
					{
						if(lstItem.Items[i].Value.Trim() != string.Empty)
							AddAlertBill(lstItem.Items[i].Value);
					}
				
				}
				//ClearInput();

				//WebInfoPublish.Publish(this,"$CS_Save_Success",this.languageComponent1);
			
				//this.cmdReturn_ServerClick(null,null);

				this.ExecuteClientFunction("SuccessAlert","");
			}
		}
		
		private void ClearInput()
		{
			this.txtStartNum.Text = string.Empty;
			this.txtLow.Text = string.Empty;
			this.txtUp.Text = string.Empty;
			this.lstItem.Items.Clear();
			this.lstUser.Items.Clear();
			this.txtDesc.Text = string.Empty;
			this.SetAlertMsg(); //��Alert��Ϣ���default
		}
		private string GetCheckItemCode()
		{
			if(this.rdbItemEdit.Checked)
			{
				return AlertItem_Old.Item;
			}
			else if(this.rdbModel.Checked)
			{
				return AlertItem_Old.Model;
			}
			else if(this.rdbSS.Checked)
			{
				return AlertItem_Old.SS;
			}
			else if(this.rdbSegment.Checked)
			{
				return AlertItem_Old.Segment;
			}
			else if(this.rdbRes.Checked)
			{
				return AlertItem_Old.Resource;
			}
			else
				return "";
		}

		private void AddAlertBillByProduct(string itemcode,string product)
		{
			BenQGuru.eMES.Domain.Alert.AlertBill alert = _alertBillFacade.CreateNewAlertBill();
			alert.AlertItem = GetCheckItemCode();
			alert.AlertType = this.rblAlertType.SelectedValue;
			alert.AlertMsg  = this.txtAlertMsg.Text;
			alert.Description = this.txtDesc.Text;
			alert.ItemCode = itemcode;
			alert.Operator = this.drpOperator.SelectedValue;
			alert.StartNum = int.Parse(this.txtStartNum.Text==""?"0":this.txtStartNum.Text);
			alert.MailNotify = this.chbMailNotify.Checked?"Y":"N";
			alert.LowValue = decimal.Parse(this.txtLow.Text);			
			alert.UpValue = decimal.Parse(this.txtUp.Text==""?"0":this.txtUp.Text);
			alert.ValidDate = BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(this.dateValidDate.Text);
			alert.MaintainDate = BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(DateTime.Now.ToShortDateString());
			alert.MaintainUser = this.GetUserCode();
			alert.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
			alert.ProductCode = product;
			_alertBillFacade.AddAlertBill(alert);

			//����email֪ͨ��
			//if(this.chbMailNotify.Checked)
			//{
			for(int i=0;i<this.lstUser.Items.Count;i++)
			{
				if(this.lstUser.Items[i].Value != string.Empty)
				{
					BenQGuru.eMES.Domain.Alert.AlertNotifier user = new BenQGuru.eMES.Domain.Alert.AlertNotifier();
					user.AlertItem = alert.AlertItem;
					user.AlertType = alert.AlertType;
					user.ItemCode  = alert.ItemCode;
					user.UserCode= this.lstUser.Items[i].Value;
					user.EMail = GetEMail(this.lstUser.Items[i].Text);
					user.BillId = alert.BillId;
					_alertBillFacade.AddAlertNotifier(user);
				}
			}
			//}
		}
		private void AddAlertBill(string itemcode)
		{
			try
			{
				DataProvider.BeginTransaction();

				if(rdbRes.Checked)
				{
					foreach(ListItem li in this.lstProduct.Items)
					{
						this.AddAlertBillByProduct(itemcode,li.Value);	
					}
				}
				else
					this.AddAlertBillByProduct(itemcode,string.Empty);

				DataProvider.CommitTransaction();
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				throw ex;
			}
		}

		private void AddAlertResBill(string itemcode,string resCode,string ErrorCode)
		{
			try
			{
				DataProvider.BeginTransaction();

				this.AddAlertResBill2(itemcode,resCode,ErrorCode);
				DataProvider.CommitTransaction();
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				throw ex;
			}
		}

		private void AddAlertResBill()
		{
			try
			{
				DataProvider.BeginTransaction();

				for(int i = 0;i<this.lstItem.Items.Count;i++)
				{
					if(lstItem.Items[i].Value.Trim() != string.Empty)
					{
						string itemcode = lstItem.Items[i].Value;
						for(int j = 0;j<this.lstResource.Items.Count;j++)
						{
							if(lstResource.Items[j].Value.Trim() != string.Empty)
							{
								string resourceCode = lstResource.Items[j].Value;
								for(int k = 0;k<this.lstErrorCode.Items.Count;k++)
								{
									if(lstErrorCode.Items[k].Value.Trim() != string.Empty)
									{
										string errorCode = lstErrorCode.Items[k].Value;
										try
										{
											this.AddAlertResBill2(itemcode,resourceCode,errorCode);
										}
										catch
										{
											WebInfoPublish.Publish(this,"$Error_Alert_Bill_Exist!",this.languageComponent1);
										}
									}
								}
							}
						}
					}
				}
				
				DataProvider.CommitTransaction();
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				throw ex;
			}
		}

		//������,���������
		private void AddAlertResBill2(string itemcode,string resCode,string ErrorCode)
		{
			try
			{

				BenQGuru.eMES.Domain.Alert.AlertResBill alert = new BenQGuru.eMES.Domain.Alert.AlertResBill();
				alert.AlertItem = GetCheckItemCode();
				alert.AlertType = this.rblAlertType.SelectedValue;
				alert.AlertMsg  = this.txtAlertMsg.Text;
				alert.Description = this.txtDesc.Text;
				alert.ItemCode = itemcode;							//��Ʒ����
				alert.ResourceCode = resCode;						//��Դ	
				alert.ErrorGroup2Code = ErrorCode;					//��������
				alert.Operator = this.drpOperator.SelectedValue;
				alert.StartNum = int.Parse(this.txtStartNum.Text==""?"0":this.txtStartNum.Text);
				alert.MailNotify = this.chbMailNotify.Checked?"Y":"N";
				alert.LowValue = decimal.Parse(this.txtLow.Text);			
				alert.UpValue = decimal.Parse(this.txtUp.Text==""?"0":this.txtUp.Text);
				alert.ValidDate = BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(this.dateValidDate.Text);
				alert.MaintainDate = BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(DateTime.Now.ToShortDateString());
				alert.MaintainUser = this.GetUserCode();
				alert.MaintainTime = FormatHelper.TOTimeInt(DateTime.Now);
				_alertBillFacade.AddAlertResBill(alert);

				//����email֪ͨ��
				//if(this.chbMailNotify.Checked)
				//{
				for(int i=0;i<this.lstUser.Items.Count;i++)
				{
					if(this.lstUser.Items[i].Value != string.Empty)
					{
						BenQGuru.eMES.Domain.Alert.AlertNotifier user = new BenQGuru.eMES.Domain.Alert.AlertNotifier();
						user.AlertItem = alert.AlertItem;
						user.AlertType = alert.AlertType;
						user.ItemCode  = alert.ItemCode;
						user.UserCode= this.lstUser.Items[i].Value;
						user.EMail = GetEMail(this.lstUser.Items[i].Text);
						user.BillId = alert.BillId;
						if(_alertBillFacade.GetAlertNotifier(user.UserCode,user.BillId) == null)
							_alertBillFacade.AddAlertNotifier(user);
					}
				}
				//}
			}
			catch(System.Exception ex)
			{
				throw ex;
			}
		}
		
		//��adb(abc@asd.com)��ȡ��email
		private string GetEMail(string str)
		{
			int i = str.IndexOf("(");
			return str.Substring(i + 1,str.Length - (i + 1 + 1));
		}
		
		protected void rblAlertItem_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.lstItem.Items.Clear();

			string item = "";
			if(this.rdbItemEdit.Checked)
			{
				item = rdbItemEdit.Text;
				this.stbItem.Type = "item";
			}
			else if(this.rdbModel.Checked)
			{
				item = rdbModel.Text;
				this.stbItem.Type = "model";
			}
			else if(this.rdbSS.Checked)
			{
				item = rdbSS.Text;
				this.stbItem.Type = "stepsequence";
			}
			else if(this.rdbSegment.Checked)
			{
				item = rdbSegment.Text;
				this.stbItem.Type = "segment";
			}
			else if(this.rdbRes.Checked)
			{
				item = rdbRes.Text;
				this.stbItem.Type = "resource";
			}                                                    

			ShowRes(this.rdbRes.Checked);

			this.stbItem.Target = this.stbItem.Type;
			this.cmdAddItem.Text = _alertConst.GetName("Add") + item;//"���"+ item; 
			this.cmdDeleteItem.Text = _alertConst.GetName("Delete") + item;//"ɾ��"+ item;
			this.lblAlertItem.Text = _alertConst.GetName("Alert") + item;//"Ԥ��" + item;

			SetAlertMsg();
		}

		private void ShowRes(bool visible)
		{
			this.lblAlertItem2.Visible = visible;
			this.lstProduct.Visible = visible;
			this.cmdAddProduct.Visible = visible;
			this.cmdDeleteProduct.Visible = visible;
			this.trProduct.Visible = visible;
			if(!visible)
			{
				this.lstProduct.Items.Clear();
				this.sbProduct.Text = string.Empty;
			}
		}
		#region 
		protected void cmdAddItem_Click(object sender, System.EventArgs e)
		{
			this.stbErrorCode.Tag = this.stbItem.Text;	//������Դɸѡ����
			string[] items = this.stbItem.Text.Trim().Split(',');
			for(int i=0;i<items.Length;i++)
			{
				if(lstItem.Items.FindByValue(items[i]) == null && items[i].Trim()!=string.Empty)
					lstItem.Items.Add(items[i].Trim());
			}
		}

		protected void cmdDeleteItem_Click(object sender, System.EventArgs e)
		{
			this.lstItem.Items.Remove(this.lstItem.SelectedItem);
			this.stbItem.Text = string.Empty;
			string[] items = new string[this.lstItem.Items.Count];
			for(int i = 0;i<this.lstItem.Items.Count;i++)
			{
				items[i] = this.lstItem.Items[i].Value;
			}

			this.stbItem.Text = String.Join(",",items);
		}

		protected void cmdAddProduct_Click(object sender, System.EventArgs e)
		{
			string[] items = this.sbProduct.Text.Trim().Split(',');
			for(int i=0;i<items.Length;i++)
			{
				if(this.lstProduct.Items.FindByValue(items[i]) == null && items[i].Trim()!=string.Empty)
					lstProduct.Items.Add(items[i].Trim());
			}
		}

		protected void cmdDeleteProduct_Click(object sender, System.EventArgs e)
		{
			this.lstProduct.Items.Remove(this.lstProduct.SelectedItem);
			this.sbProduct.Text = string.Empty;
			string[] items = new string[this.lstProduct.Items.Count];
			for(int i = 0;i<this.lstProduct.Items.Count;i++)
			{
				items[i] = this.lstProduct.Items[i].Value;
			}

			this.sbProduct.Text = String.Join(",",items);
		}

		#endregion

		#region 

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

		#endregion

		#region ���ɾ����Դ

		protected void cmdAddResource_Click(object sender, System.EventArgs e)
		{
			string[] items = this.stbResource.Text.Trim().Split(',');
			for(int i=0;i<items.Length;i++)
			{
				if(lstResource.Items.FindByValue(items[i]) == null && items[i].Trim()!=string.Empty)
					lstResource.Items.Add(items[i].Trim());
			}
		}

		protected void cmdDeleteResource_Click(object sender, System.EventArgs e)
		{
			this.lstResource.Items.Remove(this.lstResource.SelectedItem);
			this.stbResource.Text = string.Empty;
			string[] items = new string[this.lstResource.Items.Count];
			for(int i = 0;i<this.lstResource.Items.Count;i++)
			{
				items[i] = this.lstResource.Items[i].Value;
			}

			this.stbResource.Text = String.Join(",",items);
		}

		#endregion

		#region ���ɾ����������

		protected void cmdAddErrorCode_Click(object sender, System.EventArgs e)
		{
			string[] items = this.stbErrorCode.Text.Trim().Split(',');
			for(int i=0;i<items.Length;i++)
			{
				if(lstErrorCode.Items.FindByValue(items[i]) == null && items[i].Trim()!=string.Empty)
					lstErrorCode.Items.Add(items[i].Trim());
			}
		}

		protected void cmdDeleteErrorCode_Click(object sender, System.EventArgs e)
		{
			this.lstErrorCode.Items.Remove(this.lstErrorCode.SelectedItem);
			this.stbErrorCode.Text = string.Empty;
			string[] items = new string[this.lstErrorCode.Items.Count];
			for(int i = 0;i<this.lstErrorCode.Items.Count;i++)
			{
				items[i] = this.lstErrorCode.Items[i].Value;
			}

			this.stbErrorCode.Text = String.Join(",",items);
		}

		#endregion

		//Ԥ����η������ı��¼�
		protected void rblAlertType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//�����,Ĭ��Ϊ��������,ֻ���׼�����ʱ,��������
			this.txtStartNum.ReadOnly = false;

			//ֻ�в�����ʱ����Դ����Ч
			this.rdbRes.Enabled = false;
			if(this.rblAlertType.SelectedValue == "NG")
			{
				if(this.rdbSegment.Checked) 
				{
					this.rdbSegment.Checked = false;
					this.rdbItemEdit.Checked = true;
				}
				this.rdbSegment.Enabled = false;

				this.rdbItemEdit.Enabled = true;
				this.rdbModel.Enabled = true;
				this.rdbSS.Enabled = true;
				this.rdbRes.Enabled = true;
			}
			else if(this.rblAlertType.SelectedValue == "PPM" || this.rblAlertType.SelectedValue == "CPK" || this.rblAlertType.SelectedValue == "ResourceNG")
			{
				this.rdbSegment.Checked = false;
				this.rdbSegment.Enabled = false;
				this.rdbModel.Checked = false;
				this.rdbModel.Enabled = false;
				this.rdbSS.Checked = false;
				this.rdbSS.Enabled = false;
			
				this.rdbItemEdit.Enabled = true;
				this.rdbItemEdit.Checked = true;
				if(this.rblAlertType.SelectedValue == "ResourceNG")
				{
					this.txtStartNum.Text = "";
					this.txtStartNum.ReadOnly = true;
				}
			}
			else if(this.rblAlertType.SelectedValue == "DirectPass")
			{
				if(this.rdbModel.Checked)
				{
					this.rdbModel.Checked = false;
					this.rdbItemEdit.Checked = true;
				}
				this.rdbSegment.Enabled = true;
				this.rdbItemEdit.Enabled = true;
				this.rdbModel.Enabled = false;
				this.rdbSS.Enabled = true;
			}
			else
			{
				this.rdbSegment.Checked = false;
				this.rdbSegment.Enabled = false;
				this.rdbSS.Checked = false;
				this.rdbSS.Enabled = false;
				this.rdbModel.Checked = false;
				this.rdbModel.Enabled = false;
				
				this.rdbItemEdit.Enabled = true;
				this.rdbItemEdit.Checked = true;
				this.txtStartNum.Text = "";
				this.txtStartNum.ReadOnly = true;
			}

			rblAlertItem_SelectedIndexChanged(null,null);//����ѡ�����֮����¼�
			
			OperatorBuilder.Build(this.drpOperator.Items,this.rblAlertType.SelectedValue,this._alertConst);

			this.drpOperator_SelectedIndexChanged(null,null);
		}

		private void SetAlertMsg()
		{
            if (this.rblAlertType.SelectedValue == BenQGuru.eMES.AlertModel.AlertType_Old.ResourceNG)
			{
				this.txtAlertMsg.Text = this.GetResAlertMsg();return;
			}
			string msg = string.Empty;
            if (this.rblAlertType.SelectedValue == BenQGuru.eMES.AlertModel.AlertType_Old.CPK)
				msg = msg + _alertConst.CPKTESTITEM_TEMP + ",";

			msg = msg + this.rblAlertType.SelectedItem.Text + _alertConst.OVERFLOW_TEMP + ","
                + _alertConst.STANDARD_TEMP + (this.drpOperator.SelectedValue == BenQGuru.eMES.AlertModel.Operator_Old.BW ? _alertConst.CONDITIONVALUE2_TEMP : "")
				+ "," + _alertConst.DATAVALUE_TEMP;

			if(this.rdbItemEdit.Checked)
			{
                if (this.rblAlertType.SelectedValue != BenQGuru.eMES.AlertModel.AlertType_Old.First) //�׼������⴦��
					msg = _alertConst.PRODUCT_TEMP + ","
						+ msg;
					
			}
			else if(this.rdbModel.Checked)
			{
				msg = _alertConst.MODEL_TEMP + ","
					+ msg;
			}
			else if(this.rdbSS.Checked)
			{
				msg = _alertConst.LINE_TEMP + ","
					+ msg;
			}
			else if(this.rdbSegment.Checked)
			{
				msg = _alertConst.SEGMENT_TEMP + ","
					+ msg;
			}
			else if(this.rdbRes.Checked)
			{
				msg = _alertConst.RESOURCE_TEMP + ","
					+ msg;
			}
            if (this.rblAlertType.SelectedValue == BenQGuru.eMES.AlertModel.AlertType_Old.First)
				msg =  msg + "," + _alertConst.PRODUCT_TEMP + "," + _alertConst.LINE_TEMP;

			this.txtAlertMsg.Text = msg;
		}

		//��ȡ��Դ������Ԥ����Ϣ
		private string GetResAlertMsg()
		{
			//��Դ������Ԥ����Ϣ��ʽ
			//
			//��Ʒ<%PRODUCT%>,
			//ʱ���<%TIMEPERIOD%>
			//��Դ<%RESOURCE%>
			//����<%ERRORCODEGROUP:ERRORCODE%>
			//����,��׼Ϊ<%CONDITION%>
			//<%CONDITIONVALUE1%>��<%CONDITIONVALUE2%>,
			//����ֵΪ<%VALUE%>


			string msg = string.Empty;
			msg = _alertConst.PRODUCT_TEMP + ",";
			msg += _alertConst.TIMEPERIOD_TEMP + ",";
			msg += _alertConst.RESOURCE_TEMP + ",";
			msg += _alertConst.ECG2EC_TEMP + ",";
			msg += this.rblAlertType.SelectedItem.Text + _alertConst.OVERFLOW_TEMP + ",";
			msg += _alertConst.STANDARD_TEMP + ",";
			//msg += _alertConst.CONDITIONVALUE2_TEMP + ",";
			msg += _alertConst.DATAVALUE_TEMP;

			return msg;
		}

		protected void drpOperator_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SetAlertMsg();
            if (this.drpOperator.SelectedValue == BenQGuru.eMES.AlertModel.Operator_Old.BW)
			{
				this.lblAnd.Visible = true;
				this.txtUp.Visible = true;
			}
			else
			{
				this.lblAnd.Visible = false;
				this.txtUp.Visible = false;
			}
		}

		private bool ValidateInput()
		{
			if(this.isResourceNG){return this.ValidateResInput();}	//��Դ�������
			string ie = BenQGuru.eMES.Web.Helper.MessageCenter.ParserMessage("$Error_Input_Empty",this.languageComponent1);
			if(this.lstItem.Items.Count == 0)
			{
				WebInfoPublish.Publish(this,this.lblAlertItem.Text + " " + ie,this.languageComponent1);
				return false;
			}

			if(this.rdbRes.Checked)
			{
				if(this.lstProduct.Items.Count == 0)
				{
					WebInfoPublish.Publish(this,this.lblAlertItem2.Text + " " + ie,this.languageComponent1);
					return false;
				}
			}

			if(this.chbMailNotify.Checked && this.lstUser.Items.Count == 0)
			{
				WebInfoPublish.Publish(this,this.lblReceiver.Text + " " + ie,this.languageComponent1);
				return false;
			}
			PageCheckManager manager = new PageCheckManager();

            if (this.rblAlertType.SelectedValue != AlertType_Old.First && this.rblAlertType.SelectedValue != "ResourceNG") //��������׼����ߺ���Դ������
				manager.Add(new NumberCheck(this.lblStartNum,this.txtStartNum,1,int.MaxValue,true));

            if (this.drpOperator.SelectedValue == Operator_Old.BW)
			{
				manager.Add(new DecimalRangeCheck(this.lblAlertCondition,this.txtLow.Text,this.lblAlertCondition,this.txtUp.Text,true));
			}
			else
			{
				manager.Add(new DecimalCheck(this.lblAlertCondition,this.txtLow,true));
			}
			
			manager.Add(new LengthCheck(this.lblAlertMsg,this.txtAlertMsg,1000,true));
			
			foreach(ListItem li in this.lstItem.Items)
			{
				if(li.Value != string.Empty)
					manager.Add(new UniqueCheck(li.Value,this.rblAlertType.SelectedValue,this.GetCheckItemCode(),this._alertBillFacade));
			}
			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
				return false;
			}

			
			return true;
		}

		//��֤��Դ����������
		private bool ValidateResInput()
		{
			string ie = BenQGuru.eMES.Web.Helper.MessageCenter.ParserMessage("$Error_Input_Empty",this.languageComponent1);
			if(this.lstItem.Items.Count == 0)
			{
				WebInfoPublish.Publish(this,this.lblAlertItem.Text + " " + ie,this.languageComponent1);
				return false;
			}
			if(this.lstResource.Items.Count == 0)
			{
				WebInfoPublish.Publish(this,this.lblAlertResource.Text + " " + ie,this.languageComponent1);
				return false;
			}
			if(this.lstErrorCode.Items.Count == 0)
			{
				WebInfoPublish.Publish(this,this.lblNGCodeEdit.Text + " " + ie,this.languageComponent1);
				return false;
			}

			if(this.chbMailNotify.Checked && this.lstUser.Items.Count == 0)
			{
				WebInfoPublish.Publish(this,lblReceiver.Text + " " + ie,this.languageComponent1);
				return false;
			}
			PageCheckManager manager = new PageCheckManager();

			manager.Add(new DecimalCheck(this.lblAlertCondition,this.txtLow,true));
			manager.Add(new LengthCheck(this.lblAlertMsg,this.txtAlertMsg,1000,true));
			
			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
				return false;
			}

			return true;
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("FAlertBillMP.aspx"));
		}

		//���û��ֶ��ӵ�Mail�ӵ���ַ��
		protected void cmdAdd_Click(object sender, System.EventArgs e)
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
				WebInfoPublish.Publish(this, "$Error_Email_Format",this.languageComponent1);
				return;
			}

			//����û������Ƿ��Ѿ�����
			if(this.lstUser.Items.FindByValue(this.txtUser.Text.Trim()) != null)
			{
				WebInfoPublish.Publish(this, "$Error_User_Exist",this.languageComponent1);
				return;
			}
			ListItem  li = new ListItem(this.txtUser.Text.Trim() + "(" + this.txtEMail.Text.Trim() + ")",this.txtUser.Text.Trim());
			this.lstUser.Items.Add(li);
			this.txtUser.Text = string.Empty;
			this.txtEMail.Text = string.Empty;
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

		class UniqueCheck
			:IPageCheck
		{
			private string _itemCode;
			private string _alerttype;
			private BenQGuru.eMES.AlertModel.AlertBillFacade _facade;
			private string _alertitem;
			public UniqueCheck(string itemcode,string alerttype,string alertitem,BenQGuru.eMES.AlertModel.AlertBillFacade facade)
			{
				_itemCode = itemcode;
				_alerttype = alerttype;
				_facade = facade;
				_alertitem = alertitem;
			}
			#region IPageCheck ��Ա

			public string CheckMessage
			{
				get
				{
					return "$Error_AlertBill_Repeat";
				}
			}

			#endregion

			#region ICheck ��Ա

			public bool Check()
			{
				//object obj = _facade.GetAlertBill(this._itemCode,this._alerttype,this._alertitem);
				//BenQGuru.eMES.Domain.Alert.AlertBill alert = obj as BenQGuru.eMES.Domain.Alert.AlertBill;
				//if(alert != null && alert.ItemCode != null)
				//	return false;
			
				return true;
			}

			#endregion
		}

	}

	//����Ԥ�����������б�
	class OperatorBuilder
	{
		public static void Build(System.Web.UI.WebControls.ListItemCollection items,string  alerttype,AlertConst alertconst)
		{
			items.Clear();
			if(alerttype == BenQGuru.eMES.AlertModel.AlertType_Old.First || alerttype == BenQGuru.eMES.AlertModel.AlertType_Old.ResourceNG)
			{
				items.Add(new ListItem(alertconst.GetName(BenQGuru.eMES.AlertModel.Operator_Old.GE),BenQGuru.eMES.AlertModel.Operator_Old.GE));
			}
			else
			{
				items.Add(new ListItem("����",BenQGuru.eMES.AlertModel.Operator_Old.BW));
				items.Add(new ListItem(alertconst.GetName(BenQGuru.eMES.AlertModel.Operator_Old.LE),BenQGuru.eMES.AlertModel.Operator_Old.LE));
				items.Add(new ListItem(alertconst.GetName(BenQGuru.eMES.AlertModel.Operator_Old.GE),BenQGuru.eMES.AlertModel.Operator_Old.GE));
			}
		}
	}
}
