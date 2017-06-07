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

using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.Web.WarehouseWeb
{
	/// <summary>
	/// FQueryItemSelTransTypeSP ��ժҪ˵����
	/// </summary>
	public partial class FQueryItemSelTransTypeSP : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.InitPage();
				this.SelectInitValue();
			}
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

		}
		#endregion

		private void InitPage()
		{
			BenQGuru.eMES.Material.WarehouseFacade facade = new BenQGuru.eMES.Material.WarehouseFacade(base.DataProvider);
			object[] objs = facade.GetAllTransactionType();
			if (objs == null)
				return;
			listTransType.RepeatColumns = (objs.Length / 5) + 1;
			for (int i = 0; i < objs.Length; i++)
			{
				BenQGuru.eMES.Domain.Warehouse.TransactionType type = (BenQGuru.eMES.Domain.Warehouse.TransactionType)objs[i];
				listTransType.Items.Add(new ListItem(type.TransactionTypeName, type.TransactionTypeCode));
				type = null;
			}
			objs = null;
		}
		private void SelectInitValue()
		{
			string strSelected = this.Request.QueryString["selecteditem"];
			strSelected = "," + strSelected + ",";
			for (int i = 0; i < listTransType.Items.Count; i++)
			{
				if (strSelected.IndexOf("," + listTransType.Items[i].Value + ",") >= 0)
				{
					listTransType.Items[i].Selected = true;
				}
			}
		}

		protected void cmdSave_ServerClick(object sender, EventArgs e)
		{
			string strcode = "", strname = "";
			for (int i = 0; i < listTransType.Items.Count; i++)
			{
				if (listTransType.Items[i].Selected == true)
				{
					strcode += "," + listTransType.Items[i].Value;
					strname += "," + listTransType.Items[i].Text;
				}
			}
			if (strcode.Length > 0)
			{
				strcode = strcode.Substring(1);
				strname = strname.Substring(1);
			}
			this.txtSelectedItemCode.Value = strcode;
			this.txtSelectedItemName.Value = strname;
			this.Page.RegisterStartupScript("return to parent", "<script language='javascript'>ReturnValue()</script>");
		}
	}
}
