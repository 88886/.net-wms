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
using BenQGuru.eMES.Common ;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Domain.BaseSetting;

namespace BenQGuru.eMES.Web.MOModel
{
    /// <summary>
    /// FOperation2ResourceSP ��ժҪ˵����
    /// </summary>
    public partial class FItem2DimentionMP : BasePage
    {
        private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        protected System.Web.UI.WebControls.Label lblErrorGroupCode;

		BenQGuru.eMES.MOModel.ItemFacade _facade = null;


        protected void Page_Load(object sender, System.EventArgs e)
        {
			_facade = new BenQGuru.eMES.MOModel.ItemFacade(this.DataProvider);

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				if( this.GetRequestParam("ItemCode") == string.Empty)
				{
					ExceptionManager.Raise( this.GetType() , "$Error_RequestUrlParameter_Lost"); 
				}

				this.txtItemCodeQuery.Text = this.GetRequestParam("ItemCode");
                this.TextboxOrgID.Text = this.GetRequestParam("OrgID");

                Organization org = (Organization)(new BenQGuru.eMES.BaseSetting.BaseModelFacade()).GetOrg(int.Parse(this.GetRequestParam("OrgID").ToString()));
                this.TextboxOrg.Text = org.OrganizationDescription;
	
				this.LoadData();
			}
        }

        protected ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
        {
			this.Response.Redirect("FItemMP.aspx");
        }
		protected void cmdSave_ServerClick(object sender, System.EventArgs e)
		{
			if(this.ValidateInput())
			{
				this.DataProvider.BeginTransaction();
				try
				{
					EnumControl(this.Page);

					this.LoadData();

					this.DataProvider.CommitTransaction();

					WebInfoPublish.Publish(this,"$CS_Save_Success",this.languageComponent1);
				}
				catch(System.Exception ex)
				{
					this.DataProvider.RollbackTransaction();
					throw ex;
				}
			}
		}

		/// <summary>
		/// ö�ٽ����ϵ�TextBox,ÿ��TEXTBOX����һ������
		/// Add�����ݿ���,�� Update���ݿ� �� Delete
		/// </summary>
		/// <param name="control"></param>
		private void EnumControl(Control control)
		{
			foreach(Control con in control.Controls)
			{
				if(con.Controls.Count > 0)
					this.EnumControl(con);

				TextBox tb = con as TextBox;

				if(tb != null)
				{
					if(tb.ID != txtItemCodeQuery.ID && tb.ID != this.txtUnit.ID)
					{
						string paramname = tb.ID.Substring(3,tb.ID.Length - 3);
						BenQGuru.eMES.Domain.MOModel.Item2Dimention dim = _facade.GetItem2Dimention(this.txtItemCodeQuery.Text,
							paramname, int.Parse(this.TextboxOrgID.Text)) as BenQGuru.eMES.Domain.MOModel.Item2Dimention;

						if(tb.Text.Trim() != string.Empty) //�������,�� Add or Update
						{	
							if(dim == null)
							{
								dim = new BenQGuru.eMES.Domain.MOModel.Item2Dimention();
								dim.ItemCode = this.txtItemCodeQuery.Text;
								dim.MaintainUser = this.GetUserCode();
								dim.ParamName = paramname;
								dim.ParamValue = decimal.Parse(tb.Text);

								_facade.AddItem2Dimention(dim);
							}
							else
							{
								dim.ParamValue = decimal.Parse(tb.Text);
								_facade.UpdateItem2Dimention(dim);
							}
						}
						else //���û����,�����û��ǲ��Ѿ�����,���������ɾ��
						{	
							if(dim != null)
							{
								_facade.DeleteItem2Dimention(dim);
							}
						}
					}
				}
			}
		}

		private void LoadData()
		{
			BenQGuru.eMES.MOModel.ItemFacade facade = new BenQGuru.eMES.MOModel.ItemFacade(this.DataProvider);
			object[] dimList = facade.QueryItem2Dimention(this.txtItemCodeQuery.Text);
			if(dimList != null)
			{
				foreach(BenQGuru.eMES.Domain.MOModel.Item2Dimention dim in dimList)
				{
					TextBox tb = this.Page.FindControl("txt"+dim.ParamName) as TextBox;
					if(tb != null)
						tb.Text = dim.ParamValue.ToString();
				}
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


		/// <summary>
		/// ���ֵ����СֵҪô������,
		/// Ҫô��������
		/// </summary>
		private void EnumDecimalCheck(Control control)
		{
			foreach(Control con in control.Controls)
			{
				if(con.Controls.Count > 0)
					this.EnumDecimalCheck(con);

				TextBox tbMin = con as TextBox;

				if(tbMin != null)
				{
					if(tbMin.ID != txtItemCodeQuery.ID && tbMin.ID != this.txtUnit.ID)
					{
						string paramname = tbMin.ID.Substring(3,tbMin.ID.Length - 6);
						int index = tbMin.ID.IndexOf("Min");
						if( index >= 0)
						{
							string max = "txt" + paramname + "Max";
							TextBox tbMax = this.FindControl(max) as TextBox;

							Label lbl = this.FindControl("lbl" + paramname) as Label;
							if (tbMin.Text.Trim() != string.Empty || tbMax.Text.Trim() != string.Empty)
							{
								manager.Add( new BenQGuru.eMES.Web.Helper.DecimalRangeCheck(lbl,tbMin.Text.Trim(),lbl,tbMax.Text.Trim(),false));
							}
						}
					}
				}
			}
		}

		PageCheckManager manager = new PageCheckManager();
		protected bool ValidateInput()
		{
			this.EnumDecimalCheck(this.Page);

			if ( !manager.Check() )
			{
			    WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
			    return false;
			}

			return true ;

		}	
    }
}
