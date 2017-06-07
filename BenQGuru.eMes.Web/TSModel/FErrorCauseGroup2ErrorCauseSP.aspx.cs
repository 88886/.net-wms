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
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.TSModel;
using BenQGuru.eMES.TSModel;

namespace BenQGuru.eMES.Web.TSModel
{
	/// <summary>
	/// FOperation2ResourceSP ��ժҪ˵����
	/// </summary>
	public partial class FErrorCauseGroup2ErrorCauseSP : BaseMPageNew
	{
		private ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;

		private TSModelFacade _facade ;//= TSModelFacadeFactory.CreateTSModelFacade();

		#region Stable
		protected void Page_Load(object sender, System.EventArgs e)
		{			
			//this.pagerSizeSelector.Readonly = true;

			if( !this.IsPostBack )
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.txtErrorCauseGroupCodeQuery.Text = this.GetRequestParam("ErrorCauseGroup");
       
			}
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}
		#endregion

		#region Not Stable
        protected override void InitWebGrid()
        {
            base.InitWebGrid();
            this.gridHelper.AddColumn("AssErrorCause", "�ѹ�������ԭ��", null);
            this.gridHelper.AddColumn("ErroCauserDescription", "����ԭ������", null);
            this.gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this.gridHelper.AddColumn("MaintainDate", "ά������", null);
            this.gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);

            this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
            this.gridHelper.AddDefaultColumn(true, false);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
            this.gridHelper.RequestData();

        }

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			ErrorCauseGroup2ErrorCause[] ecg2ec = (ErrorCauseGroup2ErrorCause[])domainObjects.ToArray(typeof(ErrorCauseGroup2ErrorCause));

			//���errorcause�Ƿ���ʹ��
			//if(_facade.CheckErrorCodeGroup2ErrorCodeIsUsed(ecg2ec))
			//{
			//	BenQGuru.eMES.Common.ExceptionManager.Raise(this.GetType() , "$Error_ErrorCause2Group_Has_Used" ) ;
			//}
			_facade.DeleteErrorCauseGroup2ErrorCause( ecg2ec );
		}

		protected override object GetEditObject(GridRecord row)
		{
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			ErrorCauseGroup2ErrorCause relation = _facade.CreateNewErrorCauseGroup2ErrorCause();
			relation.ErrorCauseGroupCode = this.txtErrorCauseGroupCodeQuery.Text.Trim();
            relation.ErrorCause = row.Items.FindItemByKey("AssErrorCause").Text;		
			relation.MaintainUser = this.GetUserCode();

			return relation;
		}

		protected override int GetRowCount()
		{			
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			return this._facade.GetSelectedErrorCauseByErrorCauseGroupCodeCount( 
				FormatHelper.PKCapitalFormat(this.txtErrorCauseGroupCodeQuery.Text.Trim()) ,
                FormatHelper.PKCapitalFormat(this.txtErrorCauseCodeQuery.Text.Trim()));
		}

		protected override DataRow GetGridRow(object obj)
		{
            //return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
            //    new object[]{"false",
            //                    ((ErrorCause)obj).ErrorCauseCode.ToString(),
            //                    ((ErrorCause)obj).ErrorCauseDescription.ToString(),
            //                    ((ErrorCause)obj).GetDisplayText("MaintainUser"),
            //                    FormatHelper.ToDateString(((ErrorCause)obj).MaintainDate),
            //                    FormatHelper.ToTimeString(((ErrorCause)obj).MaintainTime)});
            DataRow row = this.DtSource.NewRow();
            row["AssErrorCause"] = ((ErrorCause)obj).ErrorCauseCode.ToString();
            row["ErroCauserDescription"] = ((ErrorCause)obj).ErrorCauseDescription.ToString();
            row["MaintainUser"] = ((ErrorCause)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((ErrorCause)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((ErrorCause)obj).MaintainTime);
            return row;
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{			
			if(_facade==null){_facade = new TSModelFacadeFactory(base.DataProvider).CreateTSModelFacade();}
			return this._facade.GetSelectedErrorCauseByErrorCauseGroupCode(
                FormatHelper.PKCapitalFormat(this.txtErrorCauseGroupCodeQuery.Text.Trim()) ,
                FormatHelper.PKCapitalFormat(this.txtErrorCauseCodeQuery.Text.Trim()),
                inclusive,exclusive);
		}

		protected override string[] FormatExportRecord( object obj )
		{
			return new string[]{((ErrorCause)obj).ErrorCauseCode.ToString(),
								   ((ErrorCause)obj).ErrorCauseDescription.ToString(),
								   ((ErrorCause)obj).GetDisplayText("MaintainUser"),
								   FormatHelper.ToDateString(((ErrorCause)obj).MaintainDate)};
		}

		protected override string[] GetColumnHeaderText()
		{
            return new string[] {	"AssErrorCause",
									"ErrorCauseDescription",
									"MaintainUser",
									"MaintainDate"};
		}

		protected void cmdSelect_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FErrorCauseGroup2ErrorCauseAP.aspx", new string[]{"ErrorCauseGroup"}, new string[]{FormatHelper.CleanString( this.txtErrorCauseGroupCodeQuery.Text )}));
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FErrorCauseGroupMP.aspx"));
		}

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
			this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
		#endregion

        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            this.gridHelper.RequestData();
        }
		
	}
}
