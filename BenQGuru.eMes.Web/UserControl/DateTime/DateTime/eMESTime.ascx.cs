namespace BenQGuru.eMES.Web.UserControl
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Globalization;


	/// <summary>
	///		ucTime ��ժҪ˵����
	/// </summary>
	public partial class eMESTime : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
		}

		public string Text
		{
			get
			{
				if ( this.txtTimeEditor.Text.Trim() == string.Empty )
				{
					return string.Empty;
				}

				return TimeString;
			}
			set
			{
				if ( value == string.Empty )
				{
					this.txtTimeEditor.Text = "";
				}
				else
				{
					TimeString = value;
				}
			}
		}

		/// <summary>
		/// ��ȡ������ʱ���ַ���
		/// </summary>
		public string TimeString
		{
			get
			{
				return this.txtTimeEditor.Date.ToString("HH:mm:ss");
			}
			set
			{
				try
				{
					this.txtTimeEditor.Date=DateTime.Parse(value,(new CultureInfo("fr-FR",false)).DateTimeFormat);
				}
				catch//(Exception e)
				{
					this.txtTimeEditor.Text = "";
				}
			}
		}

		/// <summary>
		/// ��ȡ�����ÿ��
		/// </summary>
		public int Width
		{
			get
			{
				return (int)this.txtTimeEditor.Width.Value;
			}
			set
			{
				this.txtTimeEditor.Width=new Unit(value);
			}
		}

		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddAttribute(string key,string value)
		{
			this.txtTimeEditor.Attributes.Add(key,value);
		}

		/// <summary>
		/// ɾ������
		/// </summary>
		/// <param name="key"></param>
		public void RemoveAttribute(string key)
		{
			this.txtTimeEditor.Attributes.Remove(key);
		}

		public string CssClass
		{
			get
			{
				return this.txtTimeEditor.CssClass;
			}
			set
			{
				this.txtTimeEditor.CssClass = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return this.txtTimeEditor.Enabled;
			}
			set
			{
				this.txtTimeEditor.Enabled=value;
			}
		}
		public new string ClientID
		{
			get
			{
				return this.txtTimeEditor.ClientID;
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
