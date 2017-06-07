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

namespace BenQGuru.eMES.Web.RealTimeReport
{
	/// <summary>
	/// FRealReportYR ��ժҪ˵����
	/// </summary>
	public partial class FRealReportYR : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			PhysicalLayoutDataPreparationHelper pHelper = new PhysicalLayoutDataPreparationHelper(null, this.drpSegmentCodeQuery, this.drpStepSequenceCodeQuery, null, this.drpShiftCodeQuery);
			pHelper.Load();

			RouteDataPreparationHelper rHelper =new RouteDataPreparationHelper(null, this.drpModelQuery, this.drpItemCodeQuery, this.drpMOCodeQuery, null);
			rHelper.Load();

			dateStartDateQuery.Enable  = "false";
			dateStartDateQuery.Date_DateTime = System.DateTime.Now;  

			this.BuildHtmlContent(); 
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

		protected BenQGuru.eMES.Web.UserControl.eMESDate dateStartDateQuery;

		
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

		private void BuildHtmlContent()
		{
			try
			{
				System.Text.StringBuilder  sb = new System.Text.StringBuilder();

				BenQGuru.eMES.BaseSetting.BaseModelFacade baseModelFacade= new FacadeFactory(this.DataProvider).CreateBaseModelFacade();  
				object[] stepSequences = baseModelFacade.GetStepSequenceBySegmentCode(this.drpShiftCodeQuery.SelectedValue); 
			

					sb.Append("<tr>");
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����"));
					if (stepSequences != null)
					{
						for(int i=0; i<stepSequences.Length;i++)
						{
							sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", ((BenQGuru.eMES.Domain.BaseSetting.StepSequence)stepSequences[i]).StepSequenceCode));
						}
					}
					else
					{
						sb.Append(string.Format("<td class='gridWebGrid-hc'  width='100%'>{0}</td>\n", "&nbsp;"));
					}
					sb.Append("</tr>\n");

					sb.Append("<tr>");
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "ֱ̨ͨ��"));
					if (stepSequences != null)
					{
						for(int i=0; i<stepSequences.Length;i++)
						{
							sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 1));
						}
					}
					else
					{
							sb.Append(string.Format("<td class='gridWebGrid-ic' >{0}</td>\n", "&nbsp;"));
					}
					sb.Append("</tr>\n");

					sb.Append("<tr>");
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "��ͨ��̨��"));
					if (stepSequences != null)
					{
						for(int i=0; i<stepSequences.Length;i++)
						{
							sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 2));
						}
					}
					else
					{
						sb.Append(string.Format("<td class='gridWebGrid-ic' >{0}</td>\n", "&nbsp;"));
					}
					sb.Append("</tr>\n");

					sb.Append("<tr>");
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����ֱͨ��"));
					if (stepSequences != null)
					{
						for(int i=0; i<stepSequences.Length;i++)
						{
							sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 3));
						}
					}
					else
					{
						sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", "&nbsp;"));
					}
					sb.Append("</tr>\n");

					sb.Append("<tr>");
					sb.Append(string.Format("<td class='gridWebGrid-hc'>{0}</td>\n", "����ֱͨ��"));
					if (stepSequences != null)
					{
						for(int i=0; i<stepSequences.Length;i++)
						{
							sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", 4));
						}
					}
					else
					{
						sb.Append(string.Format("<td class='gridWebGrid-ic'>{0}</td>\n", "&nbsp;"));
					}
					sb.Append("</tr>\n");
				
				this.HTMLContent = sb.ToString(); 
			}
			catch(Exception e)
			{
				BenQGuru.eMES.Common.ExceptionManager.Raise(null, e.Message, e);      
			}
			
		}

		protected void Submit2_ServerClick(object sender, System.EventArgs e)
		{
			this.BuildHtmlContent(); 
		}
	}
}
