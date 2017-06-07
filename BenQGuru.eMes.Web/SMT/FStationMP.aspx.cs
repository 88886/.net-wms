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
using BenQGuru.eMES.SMT;
using BenQGuru.eMES.Domain.SMT;

namespace BenQGuru.eMES.Web.SMT
{
	/// <summary>
	/// FStationMP ��ժҪ˵����
	/// </summary>
	public class FStationMP : BaseMPage
	{
		protected System.Web.UI.WebControls.Label lblStationCodeQuery;
		protected System.Web.UI.WebControls.TextBox txtStationCodeQuery;
		protected System.Web.UI.WebControls.CheckBox chbSelectAll;
		protected System.Web.UI.WebControls.Label lblStationCodeEdit;
		protected System.Web.UI.WebControls.TextBox txtStationCodeEdit;
		protected System.Web.UI.WebControls.Label lblDescriptionEdit;
		protected System.Web.UI.WebControls.TextBox txtDescriptionEdit;
		protected System.Web.UI.WebControls.Label lblResourceCodeEdit;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdQuery;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdAdd;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdDelete;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdSave;
		protected Infragistics.WebUI.UltraWebGrid.UltraWebGrid gridWebGrid;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdCancel;
		protected System.Web.UI.WebControls.DropDownList drpResourceCodeEdit;
		protected BenQGuru.eMES.Web.Helper.PagerToolBar pagerToolBar;
		protected BenQGuru.eMES.Web.Helper.PagerSizeSelector pagerSizeSelector;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdGridExport;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList drpResourceCodeQuery;
		protected System.Web.UI.WebControls.Label lblTitle;
		
		private BenQGuru.eMES.SMT.SMTFacade _facade ;//= new SMTFacadeFactory(base.DataProvider).Create();
	
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
			this.drpResourceCodeQuery.Load += new System.EventHandler(this.drpResourceCodeQuery_Load);
			this.drpResourceCodeEdit.Load += new System.EventHandler(this.drpResourceCodeEdit_Load);
			// 
			// languageComponent1
			// 
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region Init
		private void Page_Load(object sender, System.EventArgs e)
		{
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}
		#endregion

		#region WebGrid
		protected override void InitWebGrid()
		{
			// TODO: �����е�˳�򼰱���

			this.gridHelper.AddColumn( "ResourceCode", "��Դ����",	null);
			this.gridHelper.AddColumn( "StationCode", "վλ���",	null);
			this.gridHelper.AddColumn( "Description", "վ������",	null);			
			this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);			
			this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;
			this.gridHelper.AddDefaultColumn( true, true );
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}
		
		protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( 
				new object[]{"false",
								((Station)obj).ResourceCode.ToString(),
								((Station)obj).StationCode.ToString(),
								((Station)obj).Description.ToString(),								
								((Station)obj).MaintainUser.ToString(),								
								FormatHelper.ToDateString(((Station)obj).MaintainDate),
								FormatHelper.ToTimeString(((Station)obj).MaintainTime),
								""});
		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QueryStation( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtStationCodeQuery.Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpResourceCodeQuery.SelectedValue)),
				inclusive, exclusive );
		}


		protected override int GetRowCount()
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			return this._facade.QueryStationCount( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtStationCodeQuery.Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.drpResourceCodeQuery.SelectedValue)));
		}

		#endregion

		#region Button
		protected override void AddDomainObject(object domainObject)
		{		
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			this._facade.AddStation( (Station)domainObject );
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			this._facade.DeleteStation( (Station[])domainObjects.ToArray( typeof(Station) ) );
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			this._facade.UpdateStation( (Station)domainObject );
		}

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
			if ( pageAction == PageActionType.Add )
			{
				this.txtStationCodeEdit.ReadOnly = false;
                this.drpResourceCodeEdit.Enabled = true ;
			}

			if ( pageAction == PageActionType.Update )
			{
				this.txtStationCodeEdit.ReadOnly = true;
                this.drpResourceCodeEdit.Enabled = false ;

			}
		}
		#endregion

		#region Object <--> Page

		protected override object GetEditObject()
		{
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			Station station = this._facade.CreateNewStation();

			station.StationCode	 = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtStationCodeEdit.Text, 40));
			station.Description	 = FormatHelper.CleanString(this.txtDescriptionEdit.Text, 100);
			station.ResourceCode = this.drpResourceCodeEdit.SelectedValue;
			station.MaintainUser = this.GetUserCode();

			return station;
		}


		protected override object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
		{	
			if(_facade==null){_facade = new SMTFacadeFactory(base.DataProvider).Create();}
			// TODO: �������е�Index���滻keyIndex
			object obj = _facade.GetStation(  row.Cells[1].Text.ToString(), row.Cells[2].Text.ToString() );
			
			if (obj != null)
			{
				return (Station)obj;
			}

			return null;
		}

		protected override void SetEditObject(object obj)
		{
			// TODO: �����ʹ��TextBox�����޸�

			if (obj == null)
			{
				this.txtStationCodeEdit.Text	= "";
				this.txtDescriptionEdit.Text	= "";
				this.drpResourceCodeEdit.SelectedIndex	= 0;

				return;
			}

			this.txtStationCodeEdit.Text	= ((Station)obj).StationCode.ToString();
			this.txtDescriptionEdit.Text	= ((Station)obj).Description.ToString();

			try
			{
				this.drpResourceCodeEdit.SelectedValue	= ((Station)obj).ResourceCode.ToString();
			}
			catch
			{
				this.drpResourceCodeEdit.SelectedIndex	= 0;
			}
		}

		
		protected override bool ValidateInput()
		{

            PageCheckManager manager = new PageCheckManager();

            manager.Add( new LengthCheck(lblStationCodeEdit, txtStationCodeEdit, 40, true) );
            manager.Add( new LengthCheck(lblDescriptionEdit,txtDescriptionEdit,100,false)) ;
            manager.Add( new LengthCheck(lblResourceCodeEdit, drpResourceCodeEdit, 40, true) );

            if ( !manager.Check() )
            {
                WebInfoPublish.Publish(this, manager.CheckMessage,this.languageComponent1);
                return false;
            }

            return true ;



		}

		#endregion

		#region ���ݳ�ʼ��
		private void drpResourceCodeEdit_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				DropDownListBuilder builder = new DropDownListBuilder(this.drpResourceCodeEdit);
				builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate(new SMTFacadeFactory(base.DataProvider).CreateBaseModelFacadeFacade().GetAllResource);

				builder.Build("ResourceCode", "ResourceCode");
			}
		}
		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			return new string[]{ ((Station)obj).ResourceCode.ToString(),
								   ((Station)obj).StationCode.ToString(),
								   ((Station)obj).Description.ToString(),								   
								   ((Station)obj).MaintainUser.ToString(),
								   FormatHelper.ToDateString(((Station)obj).MaintainDate)};
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	"ResourceCode",
									"StationCode",
									"Description",
									"MaintainUser",	
									"MaintainDate" };
		}
		#endregion

		private void drpResourceCodeQuery_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				DropDownListBuilder builder = new DropDownListBuilder(this.drpResourceCodeQuery);
				builder.HandleGetObjectList += new BenQGuru.eMES.Web.Helper.GetObjectListDelegate(new SMTFacadeFactory(base.DataProvider).CreateBaseModelFacadeFacade().GetAllResource);

				builder.Build("ResourceCode", "ResourceCode");
				builder.AddAllItem(this.languageComponent1);
			}
		}
	}
}
