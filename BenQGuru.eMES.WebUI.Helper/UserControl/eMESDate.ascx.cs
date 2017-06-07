namespace BenQGuru.eMES.Web.UserControl
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using BenQGuru.eMES.Web.Helper;

	/// <summary>
	///		ucDate ��ժҪ˵����
	/// </summary>
	public class eMESDate : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox GuruDate;
		protected System.Web.UI.WebControls.Table Table1;
		
		public string Enable
		{
			set
			{
				this.Attributes["isEnable"] = value;
			}
			get
			{
				if ( this.Attributes["isEnable"] == null )
				{
					return "true";
				}
				return this.Attributes["isEnable"];
			}
		}


		private void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// ��ʾһ����ʱ�䣬ֻ��
		/// </summary>
		public DateTime DateTime_Null
		{
			get
			{
				return DateTime.MinValue;
			}
		}
		/// <summary>
		/// �ؼ��Ŀͻ���ID
		/// </summary>
		public string DateClientID
		{
			get
			{
				return GuruDate.ClientID;
			}
		}

		/// <summary>
		/// ��ǰʱ��
		/// </summary>
		public string Date_String
		{
			get
			{
				return Text;
			}
			set
			{
				Text=value;
			}
		}

		/// <summary>
		/// ʹ���ַ�����ʽ����������������ؼ�������ֵ
		/// </summary>
		/// <remarks>add by tilanc yao 20041105 �����������Ϊ�����滻ԭ����text�ؼ�ʱ����Ҫ�޸����ô���</remarks>
		public string Text
		{
			get
			{
				return GuruDate.Text;
			}
			set
			{
				string _dateStr = "";
				if (value == null || value.Length == 0)
				{
				}
				else
				{
					 _dateStr = DateTime.Parse(value).ToString("yyyy-MM-dd");
				}
				GuruDate.Text=_dateStr;
				GuruDate.Attributes["tab"] = _dateStr;
			}
		}

		/// <summary>
		/// ʹ��������ʽ������ʱ��
		/// </summary>
		public DateTime Date_DateTime
		{
			get
			{
				if( GuruDate.Text.Trim().Length == 0)
				{
					return DateTime_Null;
				}
				return DateTime.Parse(GuruDate.Text);
			}
			set
			{
				if( value == DateTime_Null)
				{
					GuruDate.Text="";
				}
				else
				{
					GuruDate.Text=value.ToString("yyyy-MM-dd");
				}
			}
		}

		/// <summary>
		/// ���
		/// </summary>
		public void Clear()
		{
			GuruDate.Text="";
		}

		/// <summary>
		/// �����������
		/// </summary>
		public int Width
		{
			set
			{
				GuruDate.Width=value;
			}
		}

		/// <summary>
		/// ����������Ƿ��������
		/// </summary>
		/// <remarks>add by tilancs yao 20041104</remarks>
		public bool DateTextIsReadOnly
		{
			set
			{
				this.GuruDate.ReadOnly = value;
			}
			get
			{
				return this.GuruDate.ReadOnly;
			}
		}

		/// <summary>
		/// ���Ӧ�ó���ĸ�·��
		/// </summary>
		/// <returns></returns>
		/// <remarks>add by tilancs yao 20041104</remarks>
		/// <remarks>add by tilancs yao 20050207 �ع����ȡ��ǰ׺�ķ���</remarks>
		public string GetBaseUrl()
		{
			return this.VirtualHostRoot ;
		}

		/// <summary>
		/// ������ڼ��ű��ļ�·��
		/// </summary>
		/// <returns></returns>
		/// <remarks>add by tilancs yao 20041104</remarks>
		public string GetDateCheckJsFileUrl()
		{
			return GetBaseUrl()+"UserControl/DateTime/Script/DateCheck.js";
		}

		/// <summary>
		/// �������ѡ��ؼ��ű��ļ�·��
		/// </summary>
		/// <returns></returns>
		/// <remarks>add by tilancs yao 20041104</remarks>
		public string GetCalendarJsFileUrl()
		{
			return GetBaseUrl()+"UserControl/DateTime/Script/calendarSrc.js";
		}

		public string VirtualHostRoot
		{
			get
			{
				return string.Format("{0}{1}"
					, this.Request.Url.Segments[0]
					, this.Request.Url.Segments[1]);
			}
		}
	
		public string CssClass
		{
			get
			{
				return this.GuruDate.CssClass;
			}
			set
			{
				this.GuruDate.CssClass = value;
			}
		}
	}
}
