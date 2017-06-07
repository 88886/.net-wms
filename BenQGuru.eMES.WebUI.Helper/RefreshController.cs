using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace BenQGuru.eMES.Web.Helper
{

    [DefaultProperty("Interval"), 
    ToolboxData("<{0}:RefreshController runat=server></{0}:RefreshController>")]
    public class RefreshController : System.Web.UI.WebControls.WebControl, INamingContainer 
    {

        public RefreshController(System.ComponentModel.IContainer container)
        {
            ///
            /// Windows.Forms ��׫д�����֧���������
            ///
            container.Add(this);
            InitializeComponent();

            //
            // TODO: �� InitializeComponent ���ú�����κι��캯������
            //
        }

        public RefreshController()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
           

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.keepRefresh)
            {
                string script = "setTimeout(\"document.forms[0].submit()\","
                    + this.Interval.ToString()
                    + ");";
                //string script = "alert('keepRefresh')";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RefreshController_js", script, true);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "RefreshController_js", script, true);
                
            }
            base.Render(writer);
        }


        #region �����������ɵĴ���
        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion


        [Bindable(true), Category("Action"), DefaultValue(10), Description("ˢ�¼��")] 
        public int Interval
        {
            get
            {
                try
                {
                    return int.Parse(this.ViewState["refreshCtrl_Interval"].ToString());
                }
                catch
                {
                    this.ViewState["refreshCtrl_Interval"] = 10000;
                    return 10000 ;
                }
            }
            set
            {
                this.ViewState["refreshCtrl_Interval"] = value;
            }
        }

        private bool keepRefresh
        {
            get
            {
                try
                {
                    return bool.Parse(this.ViewState["refreshCtrl_keepRefresh"].ToString());
                }
                catch
                {
                    return false;
                }
            }

            set
            {
                this.ViewState["refreshCtrl_keepRefresh"] = value;
            }
        }

		
        public void Start()
        {
            this.keepRefresh = true ;
            
        }

        public void Stop()
        {
            this.keepRefresh = false ;
        }

    }
}
