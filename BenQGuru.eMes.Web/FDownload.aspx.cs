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

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Security;
using System.IO;

namespace BenQGuru.eMES.Web
{
    /// <summary>
    /// FDownload ��ժҪ˵����
    /// </summary>
    public partial class FDownload : BasePage
    {

        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;
        private ArrayList _extForbidden;

        private ArrayList extForbidden
        {
            get
            {
                if (_extForbidden == null)
                {
                    string[] exts = new string[] { ".EXE", ".DLL", ".ASPX", ".CS", ".RESX", ".INI", ".CONFIG", ".MDB" };
                    _extForbidden = new ArrayList(exts);
                }
                return _extForbidden;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��


            this.InitPageLanguage(languageComponent1, false);

            string fileName = this.GetRequestParam("fileName");



            if (fileName == string.Empty)
            {
                BenQGuru.eMES.Common.ExceptionManager.Raise(this.GetType(), "$Error_File_Name_Invalid");
                return;
            }

            string filePath = this.MapPath(fileName);

            if (extForbidden.Contains(System.IO.Path.GetExtension(filePath).ToUpper()))
            {
                BenQGuru.eMES.Common.ExceptionManager.Raise(this.GetType(), "$Error_File_Type_Forbidden");
                return;
            }

            if (!File.Exists(filePath))
            {
                BenQGuru.eMES.Common.ExceptionManager.Raise(this.GetType(), "$File_Not_Exist");
                return;
            }
            try
            {

                #region MyRegion
                //Response.Clear();
                //Response.AddHeader("Content-Type", "x-file/unknow");
                //Response.ContentEncoding = System.Text.Encoding.UTF8;
                //  Response.AddHeader("Content-Disposition", " attachment; filename=" + getFileName(filePath));
                //Response.AddHeader("Content-Description", "FileDownload");

                //Response.WriteFile(filePath); 
                #endregion

                #region �޸�
                System.IO.FileInfo file = new System.IO.FileInfo(filePath);
                Response.Clear();
                Response.Charset = "utf-8";//��������ı���
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name));
                Response.AddHeader("Content-Length", file.Length.ToString());
                //Response.ContentType = "application/msword";
                Response.WriteFile(file.FullName);
                Response.End(); 
                #endregion
            }
            catch (Exception ex)
            {
                BenQGuru.eMES.Common.ExceptionManager.Raise(this.GetType(), "$Error_File_Access_Fail", ex);
            }
            Response.End();
        }

        private string getFileName(string filePath)
        {
            return System.IO.Path.GetFileName(filePath);
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
    }
}
