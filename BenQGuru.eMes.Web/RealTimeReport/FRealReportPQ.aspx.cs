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
using BenQGuru.eMES.RealTimeReport;


namespace BenQGuru.eMES.Web.RealTimeReport
{
	/// <summary>
	/// FRealReportPQ ��ժҪ˵����
	/// </summary>
	public partial class FRealReportPQ : BasePage
	{
		protected BenQGuru.eMES.Web.UserControl.eMESDate dateStartDateQuery;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			PhysicalLayoutDataPreparationHelper pHelper = new PhysicalLayoutDataPreparationHelper(null, this.drpSegmentCodeQuery, this.drpStepSequenceCodeQuery, null ,this.drpShiftCodeQuery);
			pHelper.Load();

			RouteDataPreparationHelper rHelper =new RouteDataPreparationHelper(null, this.drpModelQuery, this.drpItemCodeQuery, this.drpMOCodeQuery, null);
			rHelper.Load();

			dateStartDateQuery.Enable  = "false";
			dateStartDateQuery.Date_DateTime = System.DateTime.Now;   
     
			if(!this.IsPostBack)
			{
				this.BuildHtmlContent();
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

		private string _htmlContent = string.Empty;
 
		public string HTMLContent
		{
			get
			{
				return _htmlContent;
			}
			set
			{
				_htmlContent = value;
			}
		}

		protected void Submit1_ServerClick(object sender, System.EventArgs e)
		{
			this.BuildHtmlContent();
		}

		

		private void BuildHtmlContent()
		{
			try
			{
				System.Text.StringBuilder  sb = new System.Text.StringBuilder();

				ReportFacade reportFacade= new FacadeFactory(this.DataProvider).CreateReportFacade(); 
				BenQGuru.eMES.BaseSetting.ShiftModelFacade shiftModelFacade= new FacadeFactory(this.DataProvider).CreateShiftModelFacade();  
				object[] tps = shiftModelFacade.GetTimePeriodByShiftCode(this.drpShiftCodeQuery.SelectedValue); 
				object[] reports = reportFacade.QueryOPQty(this.drpSegmentCodeQuery.SelectedValue,
																			BenQGuru.eMES.Web.Helper.FormatHelper.TODateInt(this.dateStartDateQuery.Text), 
																			this.drpShiftCodeQuery.SelectedValue,
																			this.drpStepSequenceCodeQuery.SelectedValue,
																			this.drpItemCodeQuery.SelectedValue,
																			this.drpModelQuery.SelectedValue,
																			this.drpMOCodeQuery.SelectedValue); 
				if (tps == null)
				{
					sb.Append("<tr>");
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����"));
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����"));
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����"));
					sb.Append(string.Format("<td class='gridWebGrid-hc' colSpan='{0}' noWrap>{1}</td>\n", 1, "ʱ��"));
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����"));
					sb.Append("</tr>\n");
				}
				else
				{
					sb.Append("<tr>");
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����"));
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����"));
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����"));
					//sb.Append(string.Format("<td class='gridWebGrid-hc' colSpan='{0}' noWrap>{1}</td>\n", tps.Length, "ʱ��"));
					if (tps != null)
					{
						for(int i=0;i< tps.Length; i++)
						{
							sb.Append(string.Format("<td class='gridWebGrid-hc' noWrap>{0} ~ {1}</td>\n",
								BenQGuru.eMES.Web.Helper.FormatHelper.ToTimeString(((BenQGuru.eMES.Domain.BaseSetting.TimePeriod)tps[i]).TimePeriodBeginTime),
								BenQGuru.eMES.Web.Helper.FormatHelper.ToTimeString(((BenQGuru.eMES.Domain.BaseSetting.TimePeriod)tps[i]).TimePeriodEndTime)
								));
						}
					}

					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����"));
					sb.Append("</tr>\n");
				}

				sb.Append("<tr>");
				sb.Append(string.Format("<td class='gridWebGrid-hc' colSpan='{0}'>{1}</td>\n", 3, "����"));

				//����������� -B
			
				//���û������
				if (reports == null)
				{
					if (tps != null)
					{
						for(int i=0;i< tps.Length; i++)
						{
							sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 0));
						}
					}
					else
					{
						sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 0));
					}
					sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 0));
					sb.Append("</tr>\n");
				}
				//���������
				else
				{
					//1. �� Model ����
					//2. �� MO ����
					//1. �� Line ����
					if (tps != null)
					{
						for(int i=0;i< tps.Length; i++)
						{
							sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 0));
						}
					}
					else
					{
						sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 0));
					}
					sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 0));
					sb.Append("</tr>\n");
				}

				//����������� -E
				
				this.HTMLContent = sb.ToString(); 
			}
			catch(Exception e)
			{
				BenQGuru.eMES.Common.ExceptionManager.Raise(null, e.Message, e);      
			}
			
		}
	}
}
