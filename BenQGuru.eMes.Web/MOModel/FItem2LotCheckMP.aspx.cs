using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Infragistics.WebUI.UltraWebGrid;

using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Domain.MOModel;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.MOModel
{
    public partial class FItem2LotCheckMP : BaseMPageNew
    {
        #region ����
        //�����Ա������½�ҳ��ʱֱ�ӱ����Ϳ��ԣ����账��
        protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
        private System.ComponentModel.IContainer components;

        //��Ա������ѭ��̹淶
        private ItemLotFacade m_ItemLotFacade = null;
        private ItemFacade m_ItemFacade = null;
        #endregion

        #region Web ������������ɵĴ��� �˲��ַ���һ�㲻��Ҫ�䶯
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

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

        #region Init
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // ��ʼ��ҳ������
                this.InitPageLanguage(this.languageComponent1, false);

                //����ҳ���״μ���ʱ��Ҫͬʱ���õķ��������������б���ǰ���ڵ�
                this.LoadItemType();
                this.LoadExportImport();
                this.LoadCreateType();

                //JS�Ķ�������ʾ�ɽ�����ص�ҳ���������Ȼ��ҳ���ȡ
                this.hiddenInfo.Value = this.languageComponent1.GetString("$Error_LotNo_Length_IsNumber");
            }
        }

        //��Ϊ�����Ե��ã�����������
        protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
        {
            return this.languageComponent1;
        }

        #endregion

        #region Init DropDownLists RadioButtonList

        /// <summary>
        /// ��ʼ�������б�����ѡ������б�(��Ʒ���)
        /// </summary>
        private void LoadItemType()
        {
            this.drpItemTypeQuery.Items.Clear();

            this.drpItemTypeQuery.Items.Add(new ListItem("", ""));//���""����ѡ���Ϊȫѡ
            ItemType itemType = new ItemType();
            foreach (string itemTypeUse in itemType.Items)//�����Ҫ�����������ö�ٵ�����ֵ
            {
                this.drpItemTypeQuery.Items.Add(new ListItem(this.languageComponent1.GetString(itemTypeUse.Trim()), itemTypeUse));//ֵΪö��ֵ����ʾ�ı��Ӷ������ļ���ȡ
            }
            this.drpItemTypeQuery.Items.Remove(new ListItem(this.languageComponent1.GetString(ItemType.ITEMTYPE_RAWMATERIAL), ItemType.ITEMTYPE_RAWMATERIAL));//�Ƴ�����Ҫ��������
        }

        /// <summary>
        /// ��ʼ�������б�����ѡ������б�(������)
        /// </summary>
        private void LoadExportImport()
        {
            this.drpExportImportQuery.Items.Clear();
            #region �����������Ƽ�����д��
            this.drpExportImportQuery.Items.Add(new ListItem("", ""));
            this.drpExportImportQuery.Items.Add(new ListItem(this.languageComponent1.GetString("materialexportimport_import"), "IMPORT"));
            this.drpExportImportQuery.Items.Add(new ListItem(this.languageComponent1.GetString("materialexportimport_export"), "EXPORT"));
            #endregion
        }

        /// <summary>
        /// ��ʼ����ѡ�飬����ѡ��ѡ���뵥ѡ��(����)
        /// </summary>
        private void LoadCreateType()
        {
            this.RadioButtonListCreateTypeEdit.Items.Clear();
            CreateType createTypes = new CreateType();
            foreach (string createType in createTypes.Items)//����ö����ά����ֵ�ĵ�ѡ
            {
                this.RadioButtonListCreateTypeEdit.Items.Add(new ListItem(this.languageComponent1.GetString(createType.Trim()), createType));//ֵΪö��ֵ����ʾ�ı��Ӷ������ļ���ȡ
            }
            this.RadioButtonListCreateTypeEdit.SelectedIndex = 0;//�趨Ĭ��ѡ��
        }

        #endregion

        #region WebGrid
        /// <summary>
        /// �趨WebGrid�����Լ���Ҫ���ص��У�Ӧ��WebGrid������
        /// </summary>
        protected override void InitWebGrid()
        {
            base.InitWebGrid();

            //�����ͨ�У���Ҫ�ڶ������ļ���ά��������
            this.gridHelper.AddDataColumn("ItemCode", "��Ʒ����");
            this.gridHelper.AddDataColumn("ItemDescription", "��Ʒ����");
            this.gridHelper.AddDataColumn("LotPrefix", "��������ǰ׺");
            this.gridHelper.AddDataColumn("LotLength", "�������볤��");
            this.gridHelper.AddDataColumn("CreateType", "���ɽ���");
            this.gridHelper.AddDataColumn("MUser", "ά����Ա");
            this.gridHelper.AddDataColumn("MDate", "ά������");
            this.gridHelper.AddDataColumn("MTime", "ά��ʱ��");
            //this.gridHelper.AddColumn("Type", "");

            //�趨��Ҫ���ص���
            //this.gridWebGrid.Columns.FromKey("Type").Hidden = true;

            //��ӹ�ѡ�кͱ༭��
            this.gridHelper.AddDefaultColumn(true, true);

            //������
            this.gridHelper.ApplyLanguage(this.languageComponent1);
        }

        /// <summary>
        /// �ò�ѯ�õ���ʵ�����Grid���У�����ѡ������ǰ���༭���������ͨ�����м䣬��˳��������伴��
        /// </summary>
        /// <param name="obj">Item2LotCheckMP</param>
        /// <returns>UltraGridRow</returns>
        protected override DataRow GetGridRow(object obj)
        {
            DataRow row = this.DtSource.NewRow();
            row["ItemCode"] = (obj as Item2LotCheckMP).ItemCode.ToString();
            row["ItemDescription"] = (obj as Item2LotCheckMP).ItemDesc.ToString();
            row["LotPrefix"] = (obj as Item2LotCheckMP).SNPrefix.ToString();
            row["LotLength"] = (obj as Item2LotCheckMP).SNLength.ToString();
            row["CreateType"] = this.languageComponent1.GetString(((Item2LotCheckMP)obj).CreateType.ToString());
            row["MUser"] = (obj as Item2LotCheckMP).MUser.ToString();
            row["MDate"] = FormatHelper.ToDateString(((Item2LotCheckMP)obj).MaintainDate);
            row["MTime"] = FormatHelper.ToTimeString(((Item2LotCheckMP)obj).MaintainTime);

            return row;
        }

        /// <summary>
        /// ��ҳ��ò�ѯ������
        /// </summary>
        /// <param name="inclusive">��С</param>
        /// <param name="exclusive">���</param>
        /// <returns>object[]</returns>
        protected override object[] LoadDataSource(int inclusive, int exclusive)
        {
            #region ��ѯ��λ���
            if (this.txtSNLengthQuery.Text.Trim().Length > 0)
            {
                PageCheckManager manager = new PageCheckManager();

                manager.Add(new NumberCheck(lblLotLengthQuery, txtSNLengthQuery, 0, 40, false));

                if (!manager.Check())
                {
                    WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                    return null;
                }
            }
            #endregion

            if (m_ItemLotFacade == null)
            {
                m_ItemLotFacade = new ItemLotFacade(this.DataProvider);
            }
            return this.m_ItemLotFacade.QueryItem2LotCheck(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)),
                FormatHelper.CleanString(this.txtItemDescQuery.Text),
                FormatHelper.CleanString(this.drpItemTypeQuery.SelectedValue.ToString()),
                FormatHelper.CleanString(this.drpExportImportQuery.SelectedValue.ToString()),
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNPrefixQuery.Text)),
                FormatHelper.CleanString(this.txtSNLengthQuery.Text),
                inclusive, exclusive);

        }

        /// <summary>
        /// ��ò�ѯ������ܼ�¼��
        /// </summary>
        /// <returns>int</returns>
        protected override int GetRowCount()
        {
            if (m_ItemLotFacade == null)
            {
                m_ItemLotFacade = new ItemLotFacade(this.DataProvider);
            }
            return this.m_ItemLotFacade.QueryItem2LotCheckCount(
                FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCodeQuery.Text)), FormatHelper.CleanString(this.txtItemDescQuery.Text), FormatHelper.CleanString(this.drpItemTypeQuery.SelectedValue.ToString()),
                FormatHelper.CleanString(this.drpExportImportQuery.SelectedValue.ToString()), FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNPrefixQuery.Text)), FormatHelper.CleanString(this.txtSNLengthQuery.Text));
        }

        #endregion

        #region Button
        /// <summary>
        /// ���������ťʱ���ã���ʵ����Ϣ����һ�����ݵ����ݿ���
        /// </summary>
        /// <param name="domainObject">��Ҫ������¼��ʵ��</param>
        protected override void AddDomainObject(object domainObject)
        {
            if (m_ItemLotFacade == null)
            {
                m_ItemLotFacade = new ItemLotFacade(this.DataProvider);
            }
            if (m_ItemFacade == null)
            {
                m_ItemFacade = new ItemFacade(this.DataProvider);
            }

            #region ����ʱ��ValidateInput����Ҫ���Ķ�����
            object objMaterial = m_ItemFacade.GetMaterial(FormatHelper.PKCapitalFormat(this.txtItemCode.Text.Trim()), GlobalVariables.CurrentOrganizations.First().OrganizationID);
            if (objMaterial == null)
            {
                WebInfoPublish.Publish(this, "$Error_ItemCode_NotExist", this.languageComponent1);
                return;
            }
            if (string.Compare(((Domain.MOModel.Material)objMaterial).MaterialType, ItemType.ITEMTYPE_RAWMATERIAL, true) == 0)
            {
                WebInfoPublish.Publish(this, "$Error_RowMaterialNeedNotSNCheck", this.languageComponent1);
                return;
            }
            #endregion

            this.m_ItemLotFacade.AddItem2LotCheck((Item2LotCheck)domainObject);
        }

        /// <summary>
        /// ���ɾ��ʱ���ã�����ѡ�ļ�¼�����ݿ���ɾ��
        /// </summary>
        /// <param name="domainObjects">��Ҫɾ���Ķ����б�</param>
        protected override void DeleteDomainObjects(ArrayList domainObjects)
        {
            if (m_ItemLotFacade == null)
            {
                m_ItemLotFacade = new ItemLotFacade(this.DataProvider);
            }

            this.m_ItemLotFacade.DeleteItem2LotCheck((Item2LotCheck[])domainObjects.ToArray(typeof(Item2LotCheck)));
        }

        /// <summary>
        /// �������ʱ���ã����޸ĺ����Ϣ���µ����ݿ���
        /// </summary>
        /// <param name="domainObject">��Ҫ���µ�ʵ��</param>
        protected override void UpdateDomainObject(object domainObject)
        {
            if (m_ItemLotFacade == null)
            {
                m_ItemLotFacade = new ItemLotFacade(this.DataProvider);
            }

            this.m_ItemLotFacade.UpdateItem2LotCheck((Item2LotCheck)domainObject);
        }

        /// <summary>
        /// ���ݵ�ǰ���������趨ҳ��ؼ�״̬
        /// </summary>
        /// <param name="pageAction">��ǰ��������</param>
        protected override void buttonHelper_AfterPageStatusChangeHandle(string pageAction)
        {
            if (pageAction == PageActionType.Add)
            {
                this.txtItemCode.ReadOnly = false;
            }

            if (pageAction == PageActionType.Update)
            {
                this.txtItemCode.ReadOnly = true;
            }
        }

        #endregion

        #region Object <--> Page
        /// <summary>
        /// ��ȡ��ǰ��Ҫ�����ʵ�壬�����ͱ���ʱ�������
        /// </summary>
        /// <returns>��Ҫ�����object</returns>
        protected override object GetEditObject()
        {
            if (m_ItemLotFacade == null)
            {
                m_ItemLotFacade = new ItemLotFacade(this.DataProvider);
            }

            Item2LotCheck item2LotCheck = this.m_ItemLotFacade.CreateNewItem2LotCheck();
            item2LotCheck.ItemCode = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtItemCode.Text, 40));
            item2LotCheck.SNPrefix = FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtSNPrefix.Text, 40));
            if (this.txtSNLength.Text.Trim().Length > 0)
            {
                item2LotCheck.SNLength = int.Parse(this.txtSNLength.Text);
            }
            item2LotCheck.CreateType = this.RadioButtonListCreateTypeEdit.SelectedValue;
            item2LotCheck.SNContentCheck = this.chkSNContentCheck.Checked ? SNContentCheckStatus.SNContentCheckStatus_Need : SNContentCheckStatus.SNContentCheckStatus_NONeed;
            item2LotCheck.MUser = this.GetUserCode();

            return item2LotCheck;
        }

        /// <summary>
        /// ����༭��ť���ȡ�뵱ǰ�еļ�¼��Ӧ��ʵ�壬ɾ��ʱҲ�����
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override object GetEditObject(GridRecord row)
        {
            if (m_ItemLotFacade == null)
            {
                m_ItemLotFacade = new ItemLotFacade(this.DataProvider);
            }
            string strCode = string.Empty;
            object objCode = row.Items.FindItemByKey("ItemCode").Value;
            if (objCode != null)
            {
                strCode = objCode.ToString();
            }
            //����ʹ��row.Cells[1]��Ӧʹ��row.Cells.FromKey("ItemCode")
            object obj = m_ItemLotFacade.GetItem2LotCheck(strCode);

            if (obj != null)
            {
                return (Item2LotCheck)obj;
            }

            return null;
        }

        /// <summary>
        /// ����Ҫ�����ʵ����ʾ���༭�����Ӧ��λ��
        /// </summary>
        /// <param name="obj">�������ʵ��</param>
        protected override void SetEditObject(object obj)
        {
            if (obj == null)
            {
                this.txtItemCode.Text = "";
                this.txtSNPrefix.Text = "";
                this.txtSNLength.Text = "";
                this.chkSNContentCheck.Checked = true;
                this.RadioButtonListCreateTypeEdit.SelectedIndex = 0;
                return;
            }

            this.txtItemCode.Text = ((Item2LotCheck)obj).ItemCode.ToString();
            this.txtSNPrefix.Text = ((Item2LotCheck)obj).SNPrefix.ToString();
            if (!string.IsNullOrEmpty(((Item2LotCheck)obj).SNLength.ToString()))
            {
                this.txtSNLength.Text = ((Item2LotCheck)obj).SNLength.ToString();
            }

            if (((Item2LotCheck)obj).SNContentCheck.ToString() == SNContentCheckStatus.SNContentCheckStatus_Need)
            {
                this.chkSNContentCheck.Checked = true;
            }
            else
            {
                this.chkSNContentCheck.Checked = false;
            }

            this.RadioButtonListCreateTypeEdit.SelectedValue = ((Item2LotCheck)obj).CreateType.ToString();
        }

        /// <summary>
        /// �����ͱ���ȹ��õı༭�����������ݵļ��
        /// </summary>
        /// <returns>�Ƿ���ͨ��</returns>
        protected override bool ValidateInput()
        {
            #region ʹ�ù������ṩ�ļ��
            PageCheckManager manager = new PageCheckManager();
            manager.Add(new LengthCheck(lblItemCode, txtItemCode, 40, true));
            manager.Add(new LengthCheck(lblLotPrefix, txtSNPrefix, 40, true));
            manager.Add(new LengthCheck(lblLotLength, txtSNLength, 6, true));
            if (!manager.Check())
            {
                WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
                return false;
            }
            #endregion

            #region �Զ���ļ��
            if ((this.txtSNLength.Text.Trim().Length == 0 || Int32.Parse(this.txtSNLength.Text.Trim()) == 0)
                && this.txtSNPrefix.Text.Trim().Length == 0)
            {
                WebInfoPublish.Publish(this, "$LotLengthAndLotPrefix_Must_Input_One", this.languageComponent1);
                return false;
            }

            if (this.txtSNLength.Text.Trim().Length > 0
                && Int32.Parse(this.txtSNLength.Text.Trim()) > 0
                && this.txtSNPrefix.Text.Trim().Length > 0
                && this.txtSNPrefix.Text.Trim().Length >= int.Parse(this.txtSNLength.Text.Trim()))
            {
                WebInfoPublish.Publish(this, "$Error_LotLengthTooShort", this.languageComponent1);
                return false;
            }
            #endregion

            return true;
        }

        #endregion

        #region Export
        /// <summary>
        /// ��ʵ��תΪ�ַ����飬�Ӷ���������Excel
        /// </summary>
        /// <param name="obj">��Ҫת����ʵ��</param>
        /// <returns>�ַ�����</returns>
        protected override string[] FormatExportRecord(object obj)
        {
            return new string[]{     ((Item2LotCheckMP)obj).ItemCode.ToString(),
                                            ((Item2LotCheckMP)obj).ItemDesc.ToString(),
                                            ((Item2LotCheckMP)obj).SNPrefix.ToString(),
                                            ((Item2LotCheckMP)obj).SNLength.ToString(),
                                            this.languageComponent1.GetString(((Item2LotCheckMP)obj).CreateType.ToString()),
                                            ((Item2LotCheckMP)obj).MUser.ToString(),
                                            FormatHelper.ToDateString(((Item2LotCheckMP)obj).MaintainDate),
                                            FormatHelper.ToTimeString(((Item2LotCheckMP)obj).MaintainTime) }
                ;
        }

        /// <summary>
        /// ��ʾ�������������������ֵӦ�ö�����
        /// </summary>
        /// <returns>�ַ�����</returns>
        protected override string[] GetColumnHeaderText()
        {
            return new string[] {	"ItemCode",
                                            "ItemDesc",   
                                            "LotPrefix",
                                            "LotLength",
                                            "CreateType",
                                            "MUser",
                                            "MDate",
                                            "MTime"};
        }

        #endregion

    }
}
