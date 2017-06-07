namespace BenQGuru.eMES.Web.UserControl
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
    using System.Web.UI;

	/// <summary>
	///		UCNumericUpDown ��ժҪ˵����
	/// </summary>
	public partial class UCNumericUpDown : System.Web.UI.UserControl
	{
        private int _maxValue = int.MaxValue ;
        private int _minValue =0 ;
        private int _value = 0;
        private int _increment =1 ;

        public int MaxValue
        {
            get
            {
                return _maxValue ;
            }
            set
            {
                if(value < _minValue)
                {
                    throw new Exception("MaxValue should larger than MinValue");
                }
                if(value < _value)
                {
                    _value = value ;
                }

                _maxValue = value ;
            }
        }

        public int MinValue
        {
            get
            {
                return _minValue ;
            }
            set
            {
                if(value > _maxValue)
                {
                    throw new Exception("MinValue should less than MaxValue");
                }
                if(value > _value)
                {
                    _value = value ;
                }

                _minValue = value ;
            }
        }

        public int Value
        {
            get
            {
                return _value ;
            }
            set
            {
                if(value > _maxValue)
                {
                    throw new Exception("Value should less than MaxValue");
                }

                if(value < _minValue)
                {
                    throw new Exception("Value should larger than MinValue");
                }

                _value = value ;

            }
        }

        public int Increment 
        {
            get
            {
                return _increment ;
            }
            set
            {
                if(value <=0)
                {
                    throw new Exception("Increment should larger than 0") ;
                }
                _increment  = value ;
            }
        }

        public TextBox Control
        {
            get
            {
                return this.num ;
            }

        }


        protected void Page_Load(object sender, System.EventArgs e)
        {
            if(  this.IsPostBack )
            {
                this._value = int.Parse( this.num.Text ) ;
            }
        }


        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            string strScript = "var UCNumericUpDownID = '" + this.ID + "';";
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
            //Response.Write(@"<script>var UCNumericUpDownID = '" + this.ID + "';</script>");
            this.num.Attributes.Add("MaxValue", _maxValue.ToString() ) ;
            this.num.Attributes.Add("MinValue", _minValue.ToString() ) ;
            this.num.Attributes.Add("Number" , _value.ToString() ) ;
            this.num.Attributes.Add("Increment" , _increment.ToString() ) ;

//            Response.Write(this.num.ID);
            this.num.Text = _value.ToString() ;
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
            this.PreRender += new System.EventHandler(this.Page_PreRender);

        }
		#endregion
	}
}
