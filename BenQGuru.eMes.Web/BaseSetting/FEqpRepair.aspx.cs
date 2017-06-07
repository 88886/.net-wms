using System;
using System.Text;
using System.Security.Cryptography;
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
using BenQGuru.eMES.Domain.Equipment;
using BenQGuru.eMES.Common;

namespace BenQGuru.eMES.Web
{
    /// <summary>
    /// FUserPassWordModifyMP ��ժҪ˵����
    /// </summary>
    public partial class FEqpRepair : BasePage
    {
        //private BenQGuru.eMES.BaseSetting.UserFacade _userFacade = new BenQGuru.eMES.BaseSetting.UserFacade();
        private BenQGuru.eMES.Material.EquipmentFacade _equipmentFacade = null;
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);
            }

            // �ڴ˴������û������Գ�ʼ��ҳ��
            this.txtEquipment.Text = this.EqpId;
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

        private string EqpId
        {
            get
            {
                return Server.UrlDecode(Request.QueryString["Id"].ToString());
            }
        }

        protected bool ValidateInput()
        {
            PageCheckManager manager = new PageCheckManager();
            manager.Add(new LengthCheck(this.lblTsinfo, this.txtTsinfo, 400, true));
            //manager.Add(new NumberCheck(this.lblEqpTsDurationEdit, this.txtEqpTsDurationEdit, 0, 99999999, true));

            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }

            return true;
        }


        protected void cmdSave_ServerClick(object sender, System.EventArgs e)
        {
            if (_equipmentFacade == null)
            {
                _equipmentFacade = new BenQGuru.eMES.Material.EquipmentFacade(this.DataProvider);
            }
            if (ValidateInput())
            {
                if (_equipmentFacade.CheckEQPTSLogExists(this.EqpId, EquipmentTSLogStatus.EquipmentTSLogStatus_New) > 0)
                {
                    WebInfoPublish.Publish(this, "$EQPTSLog_Exists", this.languageComponent1);
                }
                else
                {
                    DBDateTime dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
                    EQPTSLog eqptsLog = new EQPTSLog();
                    eqptsLog.Eqpid = this.EqpId;
                    eqptsLog.FindUser = this.GetUserCode();
                    eqptsLog.TsInfo = FormatHelper.CleanString(this.txtTsinfo.Text, 400);
                    eqptsLog.FindMdate = dbDateTime.DBDate;
                    eqptsLog.FindMtime = dbDateTime.DBTime;
                    eqptsLog.Status = EquipmentTSLogStatus.EquipmentTSLogStatus_New;
                    eqptsLog.Duration = 0;
                    eqptsLog.MaintainUser = this.GetUserCode();
                    eqptsLog.Mdate = dbDateTime.DBDate;
                    eqptsLog.Mtime = dbDateTime.DBTime;
                    _equipmentFacade.AddEQPTSLog(eqptsLog);
                    WebInfoPublish.Publish(this, "$AddEQPTSLog_Success", this.languageComponent1);
                }
                this.ClientScript.RegisterStartupScript(base.GetType(), "CloseThis", "window.close();",true);        
            }
        }
    }
}
