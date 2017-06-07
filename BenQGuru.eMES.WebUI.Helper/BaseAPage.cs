using System;
using System.Web.UI;
using System.Collections;

using Infragistics.WebUI.UltraWebGrid;

namespace BenQGuru.eMES.Web.Helper
{
	/// <summary>
	/// BaseAPage ��ժҪ˵����
	/// </summary>
	public class BaseAPage : BasePage
	{
		public BaseAPage() : base()
		{
		}
				
		protected GridHelper gridHelper = null;
		protected ButtonHelper buttonHelper = null;

		private Infragistics.WebUI.UltraWebGrid.UltraWebGrid gridWebGrid;
		private ControlLibrary.Web.Language.LanguageComponent languageComponent;

		#region Init
		protected override void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{	
			this.gridWebGrid = this.GetWebGrid();
			this.languageComponent = this.GetLanguageComponent();
			this.Load += new System.EventHandler(this.Page_Load);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.InitOnPostBack();

			if (!IsPostBack)
			{
				this.InitPageLanguage(this.languageComponent, false);
				this.InitWebGrid();
			}
		}

		private void InitOnPostBack()
		{			
			this.buttonHelper = new ButtonHelper(this);
			
			if ( this.buttonHelper.CmdAdd != null )
			{
				this.buttonHelper.CmdAdd.ServerClick += new EventHandler(cmdAdd_Click);
			}
			
			if ( this.buttonHelper.CmdSelect != null )
			{
				this.buttonHelper.CmdSelect.ServerClick += new EventHandler(cmdSelect_Click);
			}
			
			if ( this.buttonHelper.CmdQuery != null )
			{			
				this.buttonHelper.CmdQuery.ServerClick += new EventHandler(cmdQuery_Click);
			}

			this.gridHelper = new GridHelper(this.gridWebGrid);
			this.gridHelper.LoadDataSourceHandle = new LoadDataSourceDelegate(this.LoadDataSource);
			this.gridHelper.GetRowCountHandle = new GetRowCountDelegate(this.GetRowCount);
			this.gridHelper.BuildGridRowhandle = new BuildGridRowDelegate(this.GetGridRow);
		}
		#endregion

		#region Button
		/// <summary>
		/// ���������ťʱ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void cmdAdd_Click(object sender, System.EventArgs e)
		{		
			ArrayList array = this.gridHelper.GetCheckedRows();
			object obj = null;

			if ( array.Count > 0 )
			{
				ArrayList objList = new ArrayList( array.Count );
			
				foreach (UltraGridRow row in array)
				{
					obj = this.GetEditObject(row);

					if ( obj != null )
					{
						objList.Add( obj );
					}
				}

				this.AddDomainObject(objList);

				this.gridHelper.RequestData();
				this.buttonHelper.PageActionStatusHandle( PageActionType.Add );
			}
		}
		
		/// <summary>
		/// ���ѡ��ťʱ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void cmdSelect_Click(object sender, System.EventArgs e)
		{		
			ArrayList array = this.gridHelper.GetCheckedRows();
			object obj = null;

			if ( array.Count > 0 )
			{
				ArrayList objList = new ArrayList( array.Count );
			
				foreach (UltraGridRow row in array)
				{
					obj = this.GetEditObject(row);

					if ( obj != null )
					{
						objList.Add( obj );
					}
				}

				this.AddDomainObject(objList);

				this.gridHelper.RequestData();
				this.buttonHelper.PageActionStatusHandle( PageActionType.Select );
			}
		}

		/// <summary>
		/// �����ѯ��ťʱ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void cmdQuery_Click(object sender, System.EventArgs e)
		{
			this.gridHelper.RequestData();
			this.buttonHelper.PageActionStatusHandle( PageActionType.Query );
		}
		#endregion

		#region override

		#region Control
		/// <summary>
		/// ����UltraWebGrid�����Grid����ΪgridWebGrid����������
		/// </summary>
		/// <returns></returns>
		protected virtual Infragistics.WebUI.UltraWebGrid.UltraWebGrid GetWebGrid()
		{
			Control ctrl = this.FindControl("gridWebGrid");

			if ( ctrl != null )
			{
				return (UltraWebGrid)ctrl;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// ����LanguageComponent��������
		/// </summary>
		/// <returns></returns>
		protected virtual ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return null;
		}
		#endregion
		
		#region CRUD
		/// <summary>
		/// ��ò�ѯ���õķ�ҳ���ݣ�������
		/// </summary>
		/// <param name="inclusive"></param>
		/// <param name="exclusive"></param>
		/// <returns></returns>
		protected virtual object[] LoadDataSource(int inclusive, int exclusive)
		{
			return null;
		}	
		
		/// <summary>
		/// ��ò�ѯ���õ�������������������
		/// </summary>
		/// <returns></returns>		
		protected virtual int GetRowCount()
		{
			return 0;
		}

		/// <summary>
		/// �������DomainObject�����ݿ⣬������
		/// </summary>
		/// <param name="domainObject"></param>
		protected virtual void AddDomainObject(ArrayList domainObject)
		{
		}

		#endregion

		#region Format Data
		/// <summary>
		/// ��ʼ��WebGrid�������أ����ں���������base.InitWebGrid();
		/// </summary>
		protected virtual void InitWebGrid()
		{
			this.gridHelper.ApplyLanguage( this.languageComponent );
		}
		
		/// <summary>
		/// ��object���ֶ����UltraGridRow��������
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
		{
			return new Infragistics.WebUI.UltraWebGrid.UltraGridRow();
		}

		/// <summary>
		/// ��UltraGridRow�������ֵ�����DomainObject��������
		/// </summary>
		/// <returns></returns>
		protected virtual object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
		{	
			return null;
		}
		#endregion

		#endregion
	}
}
