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
using BenQGuru.eMES.Common ;
using BenQGuru.eMES.Web.Helper ;
using BenQGuru.eMES.Web.UserControl ;

namespace BenQGuru.eMES.Web.SelectQuery
{
	/// <summary>
	/// FUserSP ��ժҪ˵����
	/// </summary>
	public partial class FUserMailSP : BaseSelectorPageNew
	{
	
		private BenQGuru.eMES.SelectQuery.SPFacade _facade = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}
		}

		private BenQGuru.eMES.SelectQuery.SPFacade facade
		{
			get
			{
				if(_facade == null)
					_facade = new FacadeFactory(base.DataProvider).CreateSPFacade();

				return _facade;
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

		#region WebGrid
		protected override DataRow GetSelectedGridRow(object obj)
		{
            DataRow row = DtSourceSelected.NewRow();
            row["Selector_SelectedCode"] = ((BenQGuru.eMES.Domain.BaseSetting.User)obj).UserCode;
            row["UserName"] = ((BenQGuru.eMES.Domain.BaseSetting.User)obj).UserName;
            row["EMail"] = ((BenQGuru.eMES.Domain.BaseSetting.User)obj).UserEmail;
            return row;
		}

		protected override DataRow GetUnSelectedGridRow(object obj)
		{
            DataRow row = DtSourceUnSelected.NewRow();
            row["Selector_UnselectedCode"] = ((BenQGuru.eMES.Domain.BaseSetting.User)obj).UserCode;
            row["UserName"] = ((BenQGuru.eMES.Domain.BaseSetting.User)obj).UserName;
            row["EMail"] = ((BenQGuru.eMES.Domain.BaseSetting.User)obj).UserEmail;
            return row;
		}

		protected override object[] LoadSelectedDataSource(int inclusive, int exclusive)
		{
			return this.facade.QuerySelectedUserMail( this.GetSelectedCodes() ) ;
		}

		protected override object[] LoadUnSelectedDataSource(int inclusive, int exclusive)
		{
            return this.facade.QueryUnSelectedUserMail(this.txtDepartmentQuery.Text, this.txtUserCodeQuery.Text, this.txtUserNameQuery.Text, this.GetSelectedCodes(), inclusive, exclusive);
		}


		protected override int GetUnSelectedRowCount()
		{
            return this.facade.QueryUnSelectedUserMailCount(this.txtDepartmentQuery.Text, this.txtUserCodeQuery.Text, this.txtUserNameQuery.Text, this.GetSelectedCodes());
		}

        protected override void InitWebGrid()
        {
            base.InitWebGrid2();
            this.gridSelectedHelper.AddColumn("Selector_SelectedCode", "��ѡ�����Ŀ", null);
            this.gridSelectedHelper.AddColumn("UserName", "�û�����", null);
            this.gridSelectedHelper.AddColumn("EMail", "�����ʼ�", null);            
            this.gridSelectedHelper.AddDefaultColumn(true, false);
            this.gridSelectedHelper.ApplyLanguage(this.languageComponent1);
            base.InitWebGrid();
            this.gridUnSelectedHelper.AddColumn("Selector_UnselectedCode", "δѡ�����Ŀ", null);
            this.gridUnSelectedHelper.AddColumn("UserName", "�û�����", null);
            this.gridUnSelectedHelper.AddColumn("EMail", "�����ʼ�", null);            
            this.gridUnSelectedHelper.AddDefaultColumn(true, false);
            this.gridUnSelectedHelper.ApplyLanguage(this.languageComponent1);

           // this.gridSelectedHelper.Grid.DisplayLayout.ClientSideEvents.CellClickHandler = "RowSelect";
           // this.gridUnSelectedHelper.Grid.DisplayLayout.ClientSideEvents.CellClickHandler = "RowSelect";
        }

		#endregion

	}
}
