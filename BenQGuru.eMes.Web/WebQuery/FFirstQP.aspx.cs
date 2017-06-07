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
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.WebQuery;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;  
using BenQGuru.eMES.Domain;
using BenQGuru.eMES.Domain.Alert;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;

namespace BenQGuru.eMES.Web.WebQuery
{
	/// <summary>
	/// FFirstQP ��ժҪ˵����
	/// </summary>
	public partial class FFirstQP : BaseQPage
	{
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		protected System.Web.UI.WebControls.TextBox txtInTicketToQuery;
		protected System.Web.UI.WebControls.Label lblReceivNoQuery;
		protected System.Web.UI.WebControls.Label lblStatusQuery;
		protected System.Web.UI.WebControls.DropDownList drpStatus;


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
			this.gridWebGrid.ClickCellButton += new Infragistics.WebUI.UltraWebGrid.ClickCellButtonEventHandler(this.gridWebGrid_ClickCellButton);
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

		protected GridHelper _gridHelper = null;
		protected WebQueryHelper _helper = null;

		#region ҳ��loadʱ�Ķ���
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._gridHelper = new GridHelper(this.gridWebGrid);

			this._helper = new WebQueryHelper( this.cmdQuery,this.cmdGridExport,this.gridWebGrid,this.pagerSizeSelector,this.pagerToolBar,this.languageComponent1 );
			this._helper.LoadGridDataSource +=new EventHandler(_helper_LoadGridDataSource);
			this._helper.DomainObjectToGridRow +=new EventHandler(_helper_DomainObjectToGridRow);
			this._helper.DomainObjectToExportRow +=new EventHandler(_helper_DomainObjectToExportRow);
			this._helper.GetExportHeadText +=new EventHandler(_helper_GetExportHeadText);
			//this._helper.MergeColumnIndexList = new object[]{ new int[]{0,1}, new int[]{2,3} };

			if(!Page.IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this._initialWebGrid();

				this.txtDate.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );
				this.txtDateTo.Text = FormatHelper.ToDateString(  FormatHelper.TODateInt(DateTime.Today) );
				BindSegment();
				BindShift();
			}
		}

		private void BindSegment()
		{
			BenQGuru.eMES.Common.Domain.IDomainDataProvider provider = null;
			try
			{
				this.drpSegment.Items.Clear();
				provider = base.DataProvider;

				BenQGuru.eMES.BaseSetting.BaseModelFacade facade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(provider);
				object[] objs = facade.GetAllSegment();
				if(objs != null && objs.Length > 0)
				{
					foreach(object obj in objs)
					{
						BenQGuru.eMES.Domain.BaseSetting.Segment seg = obj as BenQGuru.eMES.Domain.BaseSetting.Segment;
						if(seg != null)
						{
							//this.drpSegment.Items.Add(new ListItem(seg.SegmentCode,seg.ShiftTypeCode));
							this.drpSegment.Items.Add(seg.SegmentCode);
						}
					}
				}

			}
			finally
			{
				if(provider != null)
					((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)provider).PersistBroker.CloseConnection();
			}
		}

		private void BindShift()
		{
			BenQGuru.eMES.Common.Domain.IDomainDataProvider provider = null;
			try
			{
				provider = base.DataProvider;
				this.drpShift.Items.Clear();
				if(this.drpSegment.SelectedItem != null)
				{
					BenQGuru.eMES.BaseSetting.BaseModelFacade facade = new BenQGuru.eMES.BaseSetting.BaseModelFacade(provider);
					BenQGuru.eMES.Domain.BaseSetting.Segment seg = facade.GetSegment(this.drpSegment.SelectedItem.ToString()) as BenQGuru.eMES.Domain.BaseSetting.Segment;
					if(seg != null)
					{
						//string shifttype = seg.ShiftTypeCode;
						//object[] objs = provider.CustomQuery(typeof(Shift), new SQLCondition(string.Format("select {0} from TBLSHIFT where shifttypecode='{1}'order by SHIFTBTIME", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shift)),shifttype)));
                        object[] objs = (new ShiftModelFacade(base.DataProvider)).QueryShiftBySegment("", seg.SegmentCode, 0, int.MaxValue);
                        if(objs != null && objs.Length > 0)
						{
							foreach(object obj in objs)
							{
								Shift s = obj as BenQGuru.eMES.Domain.BaseSetting.Shift;
								if( s!= null)
								{
									this.drpShift.Items.Add(s.ShiftCode);
								}
							}
						}
					}

				}
				this.drpShift.Items.Insert(0,new ListItem("����",string.Empty));
			}
			finally
			{
				if(provider != null)
					((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)provider).PersistBroker.CloseConnection();
			}
		}

		protected void drpSegment_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindShift();
		}

		private void _initialWebGrid()
		{
			this._gridHelper.AddColumn("Date","����",null);
			this._gridHelper.AddColumn("Shift","���",null);
			this._gridHelper.AddColumn("SS","����",null);
			this._gridHelper.AddColumn("ItemCode","��Ʒ����",null);
			this._gridHelper.AddColumn("ShiftTime","�ϰ�ʱ��",null);
			this._gridHelper.AddColumn("OnlineTime","��һ̨Ͷ��ʱ��",null);
			this._gridHelper.AddColumn("OfflineTime","��һ̨����ʱ��",null);
			this._gridHelper.AddColumn("Time1","��̨���ߺ�ʱ",null);
			this._gridHelper.AddColumn("Time2","����׼����ʱ",null);
			this._gridHelper.AddColumn("EndTime","�°�ʱ��",null);
			this._gridHelper.AddColumn("LastOnTime","į̃Ͷ��ʱ��",null);
			this._gridHelper.AddColumn("LastOffTime","į̃����ʱ��",null);
			this._gridHelper.AddColumn("Time3","į̃���ߺ�ʱ",null);
			this._gridHelper.AddColumn("Time4","��Ч����ʱ��",null);

			//������
			this._gridHelper.ApplyLanguage( this.languageComponent1 );
		}

		private void _helper_GetExportHeadText(object sender, EventArgs e)
		{
				( e as ExportHeadEventArgs ).Heads = 
					new string[]{
									"����",
									"���",
									"����",
									"��Ʒ����",
									"�ϰ�ʱ��",
									"��һ̨Ͷ��ʱ��",
									"��һ̨����ʱ��",
									"��̨���ߺ�ʱ",
									"����׼����ʱ",
									"�°�ʱ��",
									"į̃Ͷ��ʱ��",
									"į̃����ʱ��",
									"į̃���ߺ�ʱ",
									"��Ч����ʱ��"
								};
		}

		#endregion

		private void _helper_LoadGridDataSource(object sender, EventArgs e)
		{
				
			PageCheckManager manager = new PageCheckManager();

			if( !manager.Check() )
			{
				WebInfoPublish.Publish(this,manager.CheckMessage,this.languageComponent1);
				return;
			}

			BenQGuru.eMES.Common.Domain.IDomainDataProvider provider = null;
			try
			{
				provider = base.DataProvider;

				WebQueryEventArgs we =  e as WebQueryEventArgs;

				string shift;
				if(drpShift.SelectedValue == null)
					shift =string.Empty;
				else
					shift = drpShift.SelectedValue.ToString();

				if(shift == string.Empty)
				{
					string[] shiftArray = new string[drpShift.Items.Count];
					for(int i=0;i<shiftArray.Length;i++)
					{
						shiftArray[i] = drpShift.Items[i].Value;
					}
					shift = FormatHelper.ProcessQueryValues(shiftArray);
				}
				else
				{
					shift = FormatHelper.ProcessQueryValues(drpShift.SelectedValue);
				}
				object[] dataSource = QueryFirstOnlineWeb(provider,
														this.txtDate.Text,
														this.txtDateTo.Text,
														shift,
														this.txtSS.Text.Trim(),
														this.txtItemCode.Text.Trim(),
														txtModel.Text.Trim(),
														this.drpSegment.SelectedValue,
														we.StartRow,
														we.EndRow);

				we.GridDataSource = dataSource;

				we.RowCount = QueryFirstOnlineWebCount(provider,
														this.txtDate.Text,
														this.txtDateTo.Text,
														shift,
														this.txtSS.Text.Trim(),
														this.txtItemCode.Text.Trim(),
														txtModel.Text.Trim(),
														this.drpSegment.SelectedValue);

			}
			finally
			{
				if(provider != null)
					((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)provider).PersistBroker.CloseConnection();
			}
	
		}

		private void _helper_DomainObjectToGridRow(object sender, EventArgs e)
		{	
			DomainObjectToGridRowEventArgs de = ( e as DomainObjectToGridRowEventArgs );
			if(de != null)
			{
				de.GridRow = new UltraGridRow(GetArrayFrom(de.DomainObject));
			}
		}

		private string[] GetArrayFrom(DomainObject domain)
		{
			BenQGuru.eMES.Domain.Alert.FirstOnline  obj = domain as BenQGuru.eMES.Domain.Alert.FirstOnline;
			if(obj != null)
			{
				string offtime = obj.OffLineTime == 0?string.Empty:FormatHelper.ToTimeString(obj.OffLineTime);
				
				//���
				BenQGuru.eMES.BaseSetting.ShiftModelFacade shiftFacade = new BenQGuru.eMES.BaseSetting.ShiftModelFacade(this.DataProvider);
				BenQGuru.eMES.Domain.BaseSetting.Shift shift = shiftFacade.GetShift(obj.ShiftCode) as BenQGuru.eMES.Domain.BaseSetting.Shift;
				if(shift == null) 
					return null;

				#region ��̨���ߺ�ʱ
				string tmOff = string.Empty;
				if(obj.ActionType == "OFF")
				{
					//������죬�������ڵڶ���
					if(obj.OffLineTime < obj.ShiftTime && obj.IsOverDay==FormatHelper.TRUE_STRING)
					{
						TimeSpan it = DateTime.Parse(FormatHelper.ToTimeString(obj.OffLineTime)).AddDays(1) - DateTime.Parse(FormatHelper.ToTimeString(obj.OnLineTime));		
						int tm = it.Hours * 60 + it.Minutes;
						if(tm >= 24 * 60)
							tm = 24 * 60 - tm;

						tmOff = tm.ToString();
					}
					else
					{
						TimeSpan it = DateTime.Parse(FormatHelper.ToTimeString(obj.OffLineTime)) - DateTime.Parse(FormatHelper.ToTimeString(obj.OnLineTime));
						
						int tm = it.Hours * 60 + it.Minutes;
						if(tm >= 24 * 60)
							tm = 24 * 60 - tm;

						tmOff = tm.ToString();
					}
					
				}
				#endregion

				#region ����׼����ʱ
				//������죬�������ڵڶ���
				string tmOn = string.Empty;
				if(	obj.OnLineTime < shift.ShiftEndTime //����ʱ���ڵڶ���
					&&
					obj.OnLineTime < obj.ShiftTime��///�ϰ�ʱ���ڵ�һ��
					&&
					obj.ShiftTime > shift.ShiftEndTime��
					&&
					obj.IsOverDay==FormatHelper.TRUE_STRING)
				{
					TimeSpan it2 = DateTime.Parse(FormatHelper.ToTimeString(obj.OnLineTime)).AddDays(1) - DateTime.Parse(FormatHelper.ToTimeString(obj.ShiftTime));		
					int tm = it2.Hours * 60 + it2.Minutes;
					if(tm >= 24 * 60)
						tm = 24 * 60 - tm;
					tmOn = tm.ToString();
				}
				else
				{
					TimeSpan it2 = DateTime.Parse(FormatHelper.ToTimeString(obj.OnLineTime)) - DateTime.Parse(FormatHelper.ToTimeString(obj.ShiftTime));
					int tm = it2.Hours * 60 + it2.Minutes;
					if(tm >= 24 * 60)
						tm = 24 * 60 - tm;
					tmOn = tm.ToString();
				}
				#endregion

				#region į̃���ߺ�ʱ
				string tmLastOff = string.Empty;
				if(obj.LastType  == "OFF")
				{
					//������죬���ߺ����߲���ͬһ��
					if(obj.LastOffTime < obj.LastOnTime && obj.IsOverDay==FormatHelper.TRUE_STRING)
					{
						TimeSpan it = DateTime.Parse(FormatHelper.ToTimeString(obj.LastOffTime)).AddDays(1) - DateTime.Parse(FormatHelper.ToTimeString(obj.LastOnTime));		
						int tm = it.Hours * 60 + it.Minutes;
						if(tm >= 24 * 60)
							tm = 24 * 60 - tm;

						tmLastOff = tm.ToString();
					}
					else
					{
						TimeSpan it = DateTime.Parse(FormatHelper.ToTimeString(obj.LastOffTime)) - DateTime.Parse(FormatHelper.ToTimeString(obj.LastOnTime));
						int tm = it.Hours * 60 + it.Minutes;
						if(tm >= 24 * 60)
							tm = 24 * 60 - tm;

						tmLastOff = tm.ToString();
					}
					
				}
				#endregion

				#region ��Ч����ʱ��
				//������죬���°�ʱ���ĩ�����߲ɼ�ʱ�䲻��ͬһ��
				string tmLastOn = string.Empty;
				if(obj.LastType  == "OFF")
				{
					//�°�ʱ����ϰ�ʱ��С��ĩ�����߱��ϰ�ʱ��󣬲��ұ��°�ʱ���������λ��ǰһ�죬�°�λ�ں�һ��
					if(
						obj.ShiftTime > shift.ShiftEndTime //�ϰ�ʱ���ڵ�һ��
						&&
						obj.EndTime < obj.ShiftTime��///�°�ʱ���ڵڶ���
						&& 
						obj.LastOffTime > obj.ShiftTime ///�ɼ�ʱ���ڵ�1�� 
						&& 
						obj.IsOverDay==FormatHelper.TRUE_STRING)
					{
						TimeSpan it2 = DateTime.Parse(FormatHelper.ToTimeString(obj.EndTime)).AddDays(1) - DateTime.Parse(FormatHelper.ToTimeString(obj.LastOffTime));		
						int tm = it2.Hours * 60 + it2.Minutes;
						if(tm >= 24 * 60)
							tm = 24 * 60 - tm;

						tmLastOn = tm.ToString();
					}
					else
					{
						TimeSpan it2 = DateTime.Parse(FormatHelper.ToTimeString(obj.EndTime)) - DateTime.Parse(FormatHelper.ToTimeString(obj.LastOffTime));
						int tm = it2.Hours * 60 + it2.Minutes;
						if(tm >= 24 * 60)
							tm = 24 * 60 - tm;

						tmLastOn = tm.ToString();
					}
				}
				#endregion

				return new string[]{
									FormatHelper.ToDateString(obj.MaintainDate),
									obj.ShiftCode,
									obj.SSCode,
									obj.ItemCode,
									FormatHelper.ToTimeString(obj.ShiftTime),
									FormatHelper.ToTimeString(obj.OnLineTime),
									offtime,
									tmOff,
									tmOn,
									FmtTime(obj.EndTime),
									FmtTime(obj.LastOnTime),
									FmtTime(obj.LastOffTime),
									tmLastOff,
									tmLastOn
									};
			}

			return null;
		}

		private string FmtTime(int time)
		{
			return time==0?string.Empty:FormatHelper.ToTimeString(time);
		}
		private void _helper_DomainObjectToExportRow(object sender, EventArgs e)
		{
			
			DomainObjectToExportRowEventArgs de = e as DomainObjectToExportRowEventArgs;
			if(de != null)
			{
				de.ExportRow = this.GetArrayFrom(de.DomainObject);
			}
		}

		private void gridWebGrid_ClickCellButton(object sender, Infragistics.WebUI.UltraWebGrid.CellEventArgs e)
		{
			

		}

		#region Web��ѯSQL
		private string GetFirstOnlineWhere(string date, string dateto,string shift, string ss,string itemcode,string model,string seg)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(" from TBLFIRSTONLINE where 1=1 ");
			if(date.Trim() != string.Empty)
				sb.Append(" and MDate>=").Append(FormatHelper.TODateInt(date));

			if(dateto.Trim() != string.Empty)
				sb.Append(" and MDate<=").Append(FormatHelper.TODateInt(dateto));

			if(ss != string.Empty)
				sb.Append(" and sscode like '").Append(ss.ToUpper()).Append("%'");

			if(shift != string.Empty)
				sb.Append(" and shiftcode in(").Append(shift).Append(")");

			if(itemcode != string.Empty)
				sb.Append(" and itemcode like '").Append(itemcode.ToUpper()).Append("%'");

			if(model != string.Empty)
				sb.Append(" and modelcode like '").Append(model.ToUpper().Trim()).Append("%'");

			if(seg != null && seg != string.Empty)
                sb.Append(" and sscode in(select sscode from tblss where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and segcode='").Append(seg.ToUpper().Trim()).Append("')");
			return sb.ToString();

		}
		public object[] QueryFirstOnlineWeb(BenQGuru.eMES.Common.Domain.IDomainDataProvider provider,string date, string dateto,string shift, string ss,string itemcode,string model,string seg,int inclusive, int exclusive )
		{
			string str = this.GetFirstOnlineWhere(date,dateto,shift,ss,itemcode,model,seg);
			return provider.CustomQuery(typeof(FirstOnline), new PagerCondition(string.Format("select {0} {1}", DomainObjectUtility.GetDomainObjectFieldsString(typeof(FirstOnline)) ,str), "MDATE,SSCODE", inclusive, exclusive));
		}

		public int QueryFirstOnlineWebCount(BenQGuru.eMES.Common.Domain.IDomainDataProvider provider,string date, string dateto,string shift, string ss,string itemcode,string model,string seg)
		{
			string str = this.GetFirstOnlineWhere(date,dateto,shift,ss,itemcode,model,seg);
			return provider.GetCount(new SQLCondition(string.Format("select count(*) c {0}", str)));
		}

		#endregion
	}
}
