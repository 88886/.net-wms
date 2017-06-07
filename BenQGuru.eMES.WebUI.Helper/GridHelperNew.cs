using System;
using System.Collections;

using Infragistics.Web.UI.GridControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Infragistics.Web.UI;
using System.IO;
using System.Diagnostics;

namespace BenQGuru.eMES.Web.Helper
{
    public delegate object[] LoadDataSourceDelegateNew(int inclusive, int exclusive);
    public delegate int GetRowCountDelegateNew();
    public delegate DataRow BuildGridRowDelegateNew(object obj);

    public enum NewColumnStyle
    {
        Text, Link, Edit
    }

    /// <summary>
    /// GridHelper ��ժҪ˵����
    /// </summary>	
    public class GridHelperNew
    {
        public LoadDataSourceDelegateNew LoadDataSourceHandle = null;
        public BuildGridRowDelegateNew BuildGridRowhandle = null;
        public GetRowCountDelegateNew GetRowCountHandle = null;


        public string CheckColumnKey = "Check";
        public string CheckColumnText = "*";
        public string EditColumnKey = "Edit";
        public string EditColumnText = "�༭";

        public WebDataGrid _grid = null;
        //public System.Web.UI.WebControls.CheckBox _chbSelectAll = null;
        public PagerToolBar _pagerToolBar = null;
        public PagerSizeSelector _pagerSizeSelector = null;
        public DataTable dtSource = null;

        public static string _warningNoRow = "��ѡ������һ����¼��";

        public WebDataGrid Grid
        {
            get
            {
                return this._grid;
            }
            set
            {
                this._grid = value;
            }
        }

        public PagerToolBar PagerToolBar
        {
            get
            {
                return this._pagerToolBar;
            }
            set
            {
                this._pagerToolBar = value;
            }
        }

        public PagerSizeSelector PagerSizeSelector
        {
            get
            {
                return this._pagerSizeSelector;
            }
            set
            {
                this._pagerSizeSelector = value;
            }
        }

        public GridHelperNew(WebDataGrid grid, DataTable dt)
        {
            this._grid = grid;
            this.dtSource = dt;
            this.InitGridHelper();
        }

        //GridHelperNew���캯�� add by jinger 20160224
        /// <summary>
        /// ** ���ƣ�GridHelperNew���캯��
        /// ** ���ã����һ��ҳ����ֶ��grid�ͷ�ҳ״̬����Ĺ�������
        /// </summary>
        /// <param name="grid">ҳ��Grid</param>
        /// <param name="dt">table</param>
        /// <param name="pagerToolBarID">��ҳ�ؼ�ID</param>
        /// <param name="pagerSizeSelectorID">ҳ���ؼ�ID</param>
        public GridHelperNew(WebDataGrid grid, DataTable dt, string pagerToolBarID, string pagerSizeSelectorID)
        {
            this._grid = grid;
            this.dtSource = dt;
            this.InitPager(pagerToolBarID, pagerSizeSelectorID);
        }

        public void InitGridHelper()
        {
            this.FindControls();

            if (this.PagerToolBar != null)
            {
                this.PagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);
                this.Grid.Behaviors.CreateBehavior<Paging>().PageSize = this.PagerToolBar.PageSize;
            }


            if (this._pagerSizeSelector != null)
            {
                this._pagerSizeSelector.OnPagerSizeChanged += new PagerSizeSelector.PagerSizeChangedHandle(this.pagerSizeSelector_OnPageSizeChanged);
            }


            this.ApplyDefaultStyle();

        }

        //��ʼ����ҳ�ؼ�
        /// <summary>
        /// ��ʼ����ҳ�ؼ�
        /// </summary>
        /// <param name="pagerToolBarID">��ҳ�ؼ�ID</param>
        /// <param name="pagerSizeSelectorID">ҳ���ؼ�ID</param>
        public void InitPager(string pagerToolBarID, string pagerSizeSelectorID)
        {
            this.FindPagerControls(pagerToolBarID, pagerSizeSelectorID);

            if (this.PagerToolBar != null)
            {
                this.PagerToolBar.OnPagerToolBarClick += new EventHandler(this.PagerToolBar_OnPagerToolBarClick);
                this.Grid.Behaviors.CreateBehavior<Paging>().PageSize = this.PagerToolBar.PageSize;
            }


            if (this._pagerSizeSelector != null)
            {
                this._pagerSizeSelector.OnPagerSizeChanged += new PagerSizeSelector.PagerSizeChangedHandle(this.pagerSizeSelector_OnPageSizeChanged);
            }


            this.ApplyDefaultStyle();

        }

        public void pagerSizeSelector_OnPageSizeChanged(object sender, int pageSize)
        {
            this.Grid.Behaviors.Paging.PageSize = pageSize;
            this.PagerToolBar.PageSize = pageSize;
            this.PagerToolBar.RowCount = this.PagerToolBar.RowCount;
            int iPageCount = 0;
            if (this.PagerToolBar.RowCount % pageSize == 0)
                iPageCount = this.PagerToolBar.RowCount / pageSize;
            else
                iPageCount = this.PagerToolBar.RowCount / pageSize + 1;
            if (this.PagerToolBar.PageIndex >= iPageCount)
            {
                this.PagerToolBar.PageIndex = 0;
            }
            if (this.PagerToolBar.RowCount > 0)
                RequestData();
        }

        public void PagerToolBar_OnPagerToolBarClick(object sender, System.EventArgs e)
        {
            this.RefreshData();
        }

        public void FindControls()
        {
            System.Web.UI.Control ctrl;
            //System.Web.UI.Control ctrl = this.Grid.Page.FindControl("chbSelectAll");

            //if (ctrl is System.Web.UI.WebControls.CheckBox)
            //{
            //    this._chbSelectAll = (System.Web.UI.WebControls.CheckBox)ctrl;
            //}

            ctrl = this.Grid.Page.FindControl("pagerToolBar");

            if (ctrl is PagerToolBar)
            {
                this._pagerToolBar = (PagerToolBar)ctrl;
            }

            ctrl = this.Grid.Page.FindControl("pagerSizeSelector");

            if (ctrl is PagerSizeSelector)
            {
                this._pagerSizeSelector = (PagerSizeSelector)ctrl;
            }
        }

        //���ݷ�ҳ�ؼ�ID�ҵ���ҳ�ؼ�
        /// <summary>
        ///���ݷ�ҳ�ؼ�ID�ҵ���ҳ�ؼ�
        /// </summary>
        /// <param name="pagerToolBarID">��ҳ�ؼ�ID</param>
        /// <param name="pagerSizeSelectorID">ҳ���ؼ�ID</param>
        public void FindPagerControls(string pagerToolBarID, string pagerSizeSelectorID)
        {
            System.Web.UI.Control ctrl;
            ctrl = this.Grid.Page.FindControl(pagerToolBarID);

            if (ctrl is PagerToolBar)
            {
                this._pagerToolBar = (PagerToolBar)ctrl;
            }

            ctrl = this.Grid.Page.FindControl(pagerSizeSelectorID);

            if (ctrl is PagerSizeSelector)
            {
                this._pagerSizeSelector = (PagerSizeSelector)ctrl;
            }
        }

        public void ApplyLanguage(ControlLibrary.Web.Language.LanguageComponent languageComponent)
        {
            if (languageComponent != null)
            {
                BenQGuru.eMES.Common.MutiLanguage.LanguageWord word = null;

                foreach (GridField column in this.Grid.Columns)
                {
                    word = languageComponent.GetLanguage(column.Key);

                    if (column.Key.ToString() == "Edit")
                    {
                        if (languageComponent.Language.ToString() == "CHS")
                        {
                            column.Header.Text = "�༭";
                        }
                        else if (languageComponent.Language.ToString() == "CHT")
                        {
                            column.Header.Text = "��݋";
                        }
                        else
                        {
                            column.Header.Text = "Edit";
                        }
                    }

                    if (word == null)
                    {
                        continue;
                    }

                    if (word.ControlText.Trim() == string.Empty)
                    {
                        //						continue;
                        column.Header.Text = column.Key.ToString();
                    }
                    else
                    {
                        column.Header.Text = word.ControlText;
                    }
                }

                word = languageComponent.GetLanguage("$warningNoRow");

                if (word != null)
                {
                    GridHelperNew._warningNoRow = word.ControlText; ;
                }
            }
        }


        public void AddDefaultColumn(bool hasCheckColumn, bool hasEditColumn)
        {
            if (hasCheckColumn)
            {
                this.AddGreatCheckBoxColumn(CheckColumnKey);

                this.Grid.Columns.FromKey(CheckColumnKey).VisibleIndex = 0;

            }

            if (hasEditColumn)
            {
                this.AddEditColumn(EditColumnKey, EditColumnText);
                //�����е���ʾλ�ã������ݰ�֮����ܸı�
                //(this.Grid.Columns.FromKey(EditColumnKey) as GridField).VisibleIndex= 0;

            }
        }

        /// <summary>
        /// ��������Ĺ�ѡ���У�headΪ��ѡ��
        /// </summary>
        /// <param name="key"></param>
        public void AddGreatCheckBoxColumn(string key)
        {
            if (!this.dtSource.Columns.Contains(key))
            {
                this.dtSource.Columns.Add(key, typeof(bool));
                this.dtSource.AcceptChanges();
            }

            if (this.Grid.Columns.FromKey(key) != null)
                return;

            UnboundCheckBoxField col = new UnboundCheckBoxField();
            col.Key = key;
            col.HeaderChecked = true;
            col.Width = new System.Web.UI.WebControls.Unit(30);

            //����ˮƽλ��
            col.CssClass = HorizontalAlign.Center.ToString();
            col.Header.CssClass = HorizontalAlign.Center.ToString();

            //col.HeaderCheckBoxMode = CheckBoxMode.TriState;

            this.Grid.Columns.Add(col);

            //checkbox�в���Ҫ����
            SortingColumnSetting sortColSet = new SortingColumnSetting(this.Grid);
            sortColSet.ColumnKey = key;
            sortColSet.Sortable = false;
            this.Grid.Behaviors.CreateBehavior<Sorting>().ColumnSettings.Add(sortColSet);
        }

        /// <summary>
        /// ������ͨ�Ĺ�ѡ���У�headΪ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        public void AddCheckBoxColumn(string key, string text)
        {
            AddCheckBoxColumn(key, text, true, 30);
        }

        //��Ӳ����������Ƿ����Ƿ�ɱ༭
        public void AddCheckBoxColumn(string key, string text, bool isreadonly, int width)
        {

            if (!this.dtSource.Columns.Contains(key))
            {
                this.dtSource.Columns.Add(key, typeof(bool));
                this.dtSource.AcceptChanges();
            }

            if (this.Grid.Columns.FromKey(key) != null)
                return;

            //UnboundCheckBoxField col = new UnboundCheckBoxField();
            BoundCheckBoxField col = new BoundCheckBoxField();
            col.Key = key;
            if (width > 0)
            {
                col.Width = new System.Web.UI.WebControls.Unit(width);
            }
            else
            {
                col.Width = new System.Web.UI.WebControls.Unit(30);
            }
            col.Header.Text = text;
            //����ˮƽλ��
            col.CssClass = HorizontalAlign.Center.ToString();
            col.Header.CssClass = HorizontalAlign.Center.ToString();
            this.Grid.Columns.Add(col);

            //checkbox�в���Ҫ����
            SortingColumnSetting sortColSet = new SortingColumnSetting(this.Grid);
            sortColSet.ColumnKey = key;
            sortColSet.Sortable = false;
            this.Grid.Behaviors.CreateBehavior<Sorting>().ColumnSettings.Add(sortColSet);

            EditingColumnSetting checkCol = new EditingColumnSetting(this.Grid);
            checkCol.ColumnKey = key;
            checkCol.ReadOnly = isreadonly;
            this.Grid.Behaviors.CreateBehavior<EditingCore>().Behaviors.CreateBehavior<CellEditing>().ColumnSettings.Add(checkCol);
        
        }
        // header ��checkbox
        public void AddCheckBoxColumn1(string key, string text, bool isreadonly, int width)
        {

            if (!this.dtSource.Columns.Contains(key))
            {
                this.dtSource.Columns.Add(key, typeof(bool));
                this.dtSource.AcceptChanges();
            }

            if (this.Grid.Columns.FromKey(key) != null)
                return;

            UnboundCheckBoxField col = new UnboundCheckBoxField();
            //BoundCheckBoxField col = new BoundCheckBoxField();
            col.Key = key;
            if (width > 0)
            {
                col.Width = new System.Web.UI.WebControls.Unit(width);
            }
            else
            {
                col.Width = new System.Web.UI.WebControls.Unit(30);
            }
            col.Header.Text = text;
            col.HeaderChecked = false;
            //����ˮƽλ��
            col.CssClass = HorizontalAlign.Center.ToString();
            col.Header.CssClass = HorizontalAlign.Center.ToString();
            this.Grid.Columns.Add(col);

            //checkbox�в���Ҫ����
            SortingColumnSetting sortColSet = new SortingColumnSetting(this.Grid);
            sortColSet.ColumnKey = key;
            sortColSet.Sortable = false;
            this.Grid.Behaviors.CreateBehavior<Sorting>().ColumnSettings.Add(sortColSet);

            EditingColumnSetting checkCol = new EditingColumnSetting(this.Grid);
            checkCol.ColumnKey = key;
            checkCol.ReadOnly = isreadonly;
            this.Grid.Behaviors.CreateBehavior<EditingCore>().Behaviors.CreateBehavior<CellEditing>().ColumnSettings.Add(checkCol);

        }

        public void AddCheckBoxColumn(string key, string text, bool isreadonly)
        {
            AddCheckBoxColumn(key, text, isreadonly, 30);
        }
        public void AddCheckBoxColumn1(string key, string text, bool isreadonly)
        {
            AddCheckBoxColumn1(key, text, isreadonly, 30);
        }
        public void AddCheckBoxColumn(string key, string text, bool att1, object obj)
        {
            AddCheckBoxColumn(key, text);
        }


        public void AddEditColumn(string key, string text)
        {
            if (!this.dtSource.Columns.Contains(key))
            {
                this.dtSource.Columns.Add(key);
                this.dtSource.AcceptChanges();
            }

            if (this.Grid.Columns.FromKey(key) != null)
                return;

            BoundDataField col = new BoundDataField();
            col.Key = key;
            col.Header.Text = text;

            col.CssClass = "tdEdit";
            col.Header.CssClass = HorizontalAlign.Center.ToString();

            col.Width = new System.Web.UI.WebControls.Unit(70);

            //�����Ըı��п�
            ColumnResizeSetting crs = new ColumnResizeSetting(this.Grid);
            crs.EnableResize = false;
            crs.ColumnKey = key;
            this.Grid.Behaviors.CreateBehavior<ColumnResizing>().ColumnSettings.Add(crs);

            //����������
            SortingColumnSetting sortColSet = new SortingColumnSetting(this.Grid);
            sortColSet.ColumnKey = key;
            sortColSet.Sortable = false;
            this.Grid.Behaviors.CreateBehavior<Sorting>().ColumnSettings.Add(sortColSet);

            EditingColumnSetting checkCol = new EditingColumnSetting(this.Grid);
            checkCol.ColumnKey = key;
            checkCol.ReadOnly = true;
            this.Grid.Behaviors.CreateBehavior<EditingCore>().Behaviors.CreateBehavior<CellEditing>().ColumnSettings.Add(checkCol);

            this.Grid.Columns.Add(col);

            //ԭ���ķ�ʽ-ʹ��ģ����
            //ģ���еķ�ʽ�ή���ٶ�,���Բ�����
            //AddTemplateColumn(key, text, NewColumnStyle.Edit, HorizontalAlign.Center);
        }


        /// <summary>
        /// ��link��ť���У�Ĭ�Ͼ���,�п�100
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        public void AddLinkColumn(string key, string text)
        {
            AddLinkColumn(key, text, 100, string.Empty);
            //AddTemplateColumn(key, text, NewColumnStyle.Link, HorizontalAlign.Center);
        }

        public void AddLinkColumn(string key, string text, int width)
        {
            AddLinkColumn(key, text, width, string.Empty);
            //AddTemplateColumn(key, text, NewColumnStyle.Link, HorizontalAlign.Center);
        }

        public void AddLinkColumn(string key, string text, string cssClass)
        {
            AddLinkColumn(key, text, 100, cssClass);
            //AddTemplateColumn(key, text, NewColumnStyle.Link, HorizontalAlign.Center);
        }
        /// <summary>
        /// ��link��ť���У�Ĭ�Ͼ���,�п�100
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        /// <param name="obj"></param>
        public void AddLinkColumn(string key, string text, object obj)
        {
            AddLinkColumn(key, text, 100, string.Empty);
        }

        public void AddLinkColumn(string key, string text, int width, string cssClass)
        {
            if (!this.dtSource.Columns.Contains(key))
            {
                this.dtSource.Columns.Add(key);
                this.dtSource.AcceptChanges();
            }

            if (this.Grid.Columns.FromKey(key) != null)
                return;

            BoundDataField col = new BoundDataField();
            col.Key = key;
            col.Header.Text = text;

            if (string.IsNullOrEmpty(cssClass))
                col.CssClass = "tdLink";
            else
                col.CssClass = cssClass;

            col.Header.CssClass = HorizontalAlign.Center.ToString();

            if (width > 0)
                col.Width = new System.Web.UI.WebControls.Unit(width);
            else
                col.Width = new System.Web.UI.WebControls.Unit(100);

            //�����Ըı��п�
            ColumnResizeSetting crs = new ColumnResizeSetting(this.Grid);
            crs.EnableResize = false;
            crs.ColumnKey = key;
            this.Grid.Behaviors.CreateBehavior<ColumnResizing>().ColumnSettings.Add(crs);

            //����������
            SortingColumnSetting sortColSet = new SortingColumnSetting(this.Grid);
            sortColSet.ColumnKey = key;
            sortColSet.Sortable = false;
            this.Grid.Behaviors.CreateBehavior<Sorting>().ColumnSettings.Add(sortColSet);

            EditingColumnSetting checkCol = new EditingColumnSetting(this.Grid);
            checkCol.ColumnKey = key;
            checkCol.ReadOnly = true;
            this.Grid.Behaviors.CreateBehavior<EditingCore>().Behaviors.CreateBehavior<CellEditing>().ColumnSettings.Add(checkCol);

            this.Grid.Columns.Add(col);

            //ԭ���ķ�ʽ-ʹ��ģ����
            //ģ���еķ�ʽ�ή���ٶ�,���Բ�����
            //AddTemplateColumn(key, text, NewColumnStyle.Link, HorizontalAlign.Center);
        }

        /// <summary>
        /// ���ģ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="align"></param>
        /// <param name="hidden"></param>
        private void AddTemplateColumn(string key, string text, NewColumnStyle style, int width, HorizontalAlign align, bool hidden)
        {
            if (!this.dtSource.Columns.Contains(key))
            {
                this.dtSource.Columns.Add(key);
                this.dtSource.AcceptChanges();
            }

            TemplateDataField tempCol = this.Grid.Columns.FromKey(key) as TemplateDataField;
            if (tempCol == null)
            {
                tempCol = new TemplateDataField();
                tempCol.Key = key;
                this.Grid.Columns.Add(tempCol);
            }

            tempCol.ItemTemplate = new CustomColumnTemplate(key, text, style, DataControlRowType.DataRow, align);
            tempCol.HeaderTemplate = new CustomColumnTemplate(key, text, style, DataControlRowType.Header, align);

            if (width != 0)
                tempCol.Width = new System.Web.UI.WebControls.Unit(width);

            tempCol.Hidden = hidden;

            //ģ���в���Ҫ����
            SortingColumnSetting sortColSet = new SortingColumnSetting(this.Grid);
            sortColSet.ColumnKey = key;
            sortColSet.Sortable = false;
            this.Grid.Behaviors.CreateBehavior<Sorting>().ColumnSettings.Add(sortColSet);
        }

        private void AddTemplateColumn(string key, string text, NewColumnStyle style, HorizontalAlign align, bool hidden)
        {
            AddTemplateColumn(key, text, style, 0, align, hidden);
        }

        private void AddTemplateColumn(string key, string text, NewColumnStyle style, HorizontalAlign align)
        {
            AddTemplateColumn(key, text, style, 0, align, false);
        }

        private void AddTemplateColumn(string key, string text, NewColumnStyle style)
        {
            AddTemplateColumn(key, text, style, 0, HorizontalAlign.Left, false);
        }

        private void AddTemplateColumn(string key, string text)
        {
            AddTemplateColumn(key, text, NewColumnStyle.Text, 0, HorizontalAlign.Center, false);
        }

        /// <summary>
        /// ������ͨ�����ݰ��У�Ĭ��ˮƽ���������趨ˮƽλ�ã���ʹ��ģ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="headerText"></param>
        /// <param name="width"></param>
        /// <param name="hidden"></param>
        /// <param name="readOnly">�Ƿ�ֻ����������Ƿ���Ա༭�� add by jinger 20160308</param>
        /// <param name="align"></param>
        public void AddDataColumn(string key, string headerText, int width, bool hidden,bool readOnly, HorizontalAlign align)
        {
            if (!this.dtSource.Columns.Contains(key))
            {
                this.dtSource.Columns.Add(key);
                this.dtSource.AcceptChanges();
            }

            if (this.Grid.Columns.FromKey(key) != null)
                return;

            BoundDataField col = new BoundDataField();
            col.Key = key;
            col.Header.Text = headerText;

            col.CssClass = align.ToString();
            col.Header.CssClass = align.ToString();

            if (width != 0)
                col.Width = new System.Web.UI.WebControls.Unit(width);

            this.Grid.Columns.Add(col);
            col.Hidden = hidden;

            EditingColumnSetting checkCol = new EditingColumnSetting(this.Grid);
            checkCol.ColumnKey = key;
            checkCol.ReadOnly = readOnly;
            this.Grid.Behaviors.CreateBehavior<EditingCore>().Behaviors.CreateBehavior<CellEditing>().ColumnSettings.Add(checkCol);
        }

        public void AddDataColumn(string key, string headerText)
        {
            AddDataColumn(key, headerText, 0, false, true, HorizontalAlign.Left);
        }

        public void AddDataColumn(string key, string headerText, int width)
        {
            AddDataColumn(key, headerText, width, false, true, HorizontalAlign.Left);
        }

        public void AddDataColumn(string key, string headerText, bool hidden)
        {
            AddDataColumn(key, headerText, 0, hidden, true, HorizontalAlign.Left);
        }

        public void AddDataColumn(string key, string headerText, HorizontalAlign align)
        {
            AddDataColumn(key, headerText, 0, false, true, align);
        }

        public void AddDataColumn(string key, string headerText, int width, HorizontalAlign align)
        {
            AddDataColumn(key, headerText, width, false, true, align);
        }

        public void AddDataColumn(string key, string headerText, bool hidden, HorizontalAlign align)
        {
            AddDataColumn(key, headerText, 0, hidden, true, align);
        }

        /// <summary>
        /// �����ͨ��������,�������Ӧ��Ĭ����ʾ��,ˮƽ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="headerText"></param>
        /// <param name="obj"></param>
        public void AddColumn(string key, string headerText, object obj)
        {
            AddDataColumn(key, headerText, 0, false, true, HorizontalAlign.Left);
        }

        //�����ͨ��������,�������Ӧ��Ĭ����ʾ��,ˮƽ�����Ƿ����ñ༭ add jinger 20150308
        /// <summary>
        /// �����ͨ��������,�������Ӧ��Ĭ����ʾ��,ˮƽ����
        /// </summary>
        /// <param name="key">��ͷ</param>
        /// <param name="headerText">��ͷ����</param>
        /// <param name="readOnly">���Ƿ�ֻ��</param>
        public void AddColumn(string key, string headerText, bool readOnly)
        {
            AddDataColumn(key, headerText, 0, false, readOnly, HorizontalAlign.Left);
        }

        /// <summary>
        /// ����Grid��Ĭ����ʽ
        /// </summary>
        public virtual void ApplyDefaultStyle()
        {
            //ѡ��ģʽ
            //this.Grid.Behaviors.Selection.CellClickAction = CellClickAction.Row;
            this.Grid.Behaviors.CreateBehavior<Selection>().Enabled = false;
            this.Grid.Behaviors.CreateBehavior<Activation>().Enabled = true;

            this.Grid.Behaviors.CreateBehavior<RowSelectors>().Enabled = true;
            this.Grid.Behaviors.CreateBehavior<RowSelectors>().RowNumbering = true;

            //����
            this.Grid.Behaviors.CreateBehavior<Sorting>().Enabled = true;
            this.Grid.Behaviors.CreateBehavior<Sorting>().SortingMode = SortingMode.Multi;


            //�����ҳ
            this.Grid.Behaviors.CreateBehavior<Paging>().Enabled = false;

            //Ĭ�Ϲ�ѡ�����           
            //EditingColumnSetting checkCol = new EditingColumnSetting(this.Grid);
            //checkCol.ColumnKey = this.CheckColumnKey;
            //checkCol.ReadOnly = true;
            this.Grid.Behaviors.CreateBehavior<EditingCore>().Behaviors.CreateBehavior<CellEditing>().Enabled = true;
            //this.Grid.Behaviors.CreateBehavior<EditingCore>().Behaviors.CreateBehavior<CellEditing>().ColumnSettings.Add(checkCol);

            //�п�ı�
            this.Grid.Behaviors.CreateBehavior<ColumnResizing>().Enabled = true;

            //�ȴ�ͼ��
            this.Grid.AjaxIndicator.Enabled = DefaultableBoolean.True;
            this.Grid.AjaxIndicator.BlockArea = AjaxIndicatorBlockArea.Page;
            //this.Grid.AjaxIndicator.BlockCssClass = "ig_AjaxIndicatorBlock";
        }

        //void Grid_HeaderCheckBoxClicked(object sender, HeaderCheckBoxEventArgs e)
        //{
        //    if (e.PreviousState == CheckBoxState.Checked)
        //    {
        //        foreach (ControlDataRecord row in this.Grid.Rows)
        //        {
        //            row.Items[this.Grid.Columns.FromKey(CheckColumnKey).Index].Value = false;
        //        }

        //    }
        //    else if (e.PreviousState == CheckBoxState.Unchecked)
        //    {
        //        foreach (ControlDataRecord row in this.Grid.Rows)
        //        {
        //            row.Items[this.Grid.Columns.FromKey(CheckColumnKey).Index].Value = true;
        //        }
        //    }

        //}

        //public void CheckAllRows(CheckStatus status)
        //{
        //    if (this.Grid.Columns.FromKey(CheckColumnKey).AllowUpdate == AllowUpdate.Yes)
        //    {
        //        if (status == CheckStatus.Checked)
        //        {
        //            foreach (UltraGridRow row in this.Grid.Rows)
        //            {
        //                if (row.Cells.FromKey(CheckColumnKey).AllowEditing != AllowEditing.No)
        //                {
        //                    row.Cells.FromKey(CheckColumnKey).Text = "true";
        //                }
        //            }
        //        }

        //        if (status == CheckStatus.Unchecked)
        //        {
        //            foreach (UltraGridRow row in this.Grid.Rows)
        //            {
        //                if (row.Cells.FromKey(CheckColumnKey).AllowEditing != AllowEditing.No)
        //                {
        //                    row.Cells.FromKey(CheckColumnKey).Text = "false";
        //                }
        //            }
        //        }
        //    }
        //}

        protected void RunScript(string strScript)
        {
            if (ScriptManager.GetCurrent(this.Grid.Page) != null)
            {
                ScriptManager.RegisterStartupScript(this.Grid.Page, this.Grid.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                this.Grid.Page.ClientScript.RegisterStartupScript(this.Grid.Page.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        public ArrayList GetCheckedRows()
        {
           
            //strSelectRowGUIDS�м�¼��ͨ��JSд�����ѡ����е�GUID���ϣ��磺(GUID1)(GUID2)
            string strSelectRowGUIDS=(this._grid.Page.FindControl("hdnSelectRowGUIDS_"+this._grid.ID) as HiddenField).Value;

            ArrayList array = new ArrayList();
            foreach (GridRecord row in this.Grid.Rows)
            {
                //if (row.Items[this.Grid.Columns.FromKey(CheckColumnKey).Index].Value.ToString() == "True")
                //{
                //    //array.Add((row.DataItem as DataRowView).Row);
                //    array.Add(row);
                //}
                if (strSelectRowGUIDS.Contains("(" + row.Items.FindItemByKey("GUID").Value.ToString() + ")"))
                {
                    array.Add(row);
                }
            }

            if (array.Count == 0)
            {
                ShowMessageDiv(this.Grid.Page, GridHelper._warningNoRow);
            }

            return array;
        }

        /// <summary>
        /// ��ʾ��ͨ����Ϣ��ʾ��
        /// </summary>
        /// <param name="page"></param>
        /// <param name="msg"></param>
        public void ShowMessageDiv(Page page, string msg)
        {
            string stcrSript = string.Empty;
            if (WebInfoPublish.isUseDiv)
            {
                stcrSript = string.Format(@"
                if(window.top.location.pathname.indexOf('FStartPage.aspx')<0)
                {{
                    alert('{0}');          
                }}
                else
                {{
                    window.top.showMessageDialog('{0}');
                }}", msg);
            }
            else
            {
                stcrSript = string.Format("alert('{0}');", msg);
            }
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), Guid.NewGuid().ToString(), stcrSript, true);
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), stcrSript, true);

        }

        /// <summary>
        /// ��ʾ������Ϣ��ʾ��
        /// </summary>
        /// <param name="page"></param>
        /// <param name="msg"></param>
        public void ShowErrorDiv(Page page, string errorMsg, string errorDetail)
        {
            string stcrSript = string.Format(@"
            
             if(window.top.location.pathname.indexOf('FStartPage.aspx')<0)
                {{
                    alert('{0},{1}');          
                }}
                else
                {{
                    window.top.showErrorDialog('{0}','{1}');
                }}
", errorMsg, errorDetail);
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), Guid.NewGuid().ToString(), "<script language=javascript>" + stcrSript + "</script>");
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), stcrSript, true);
        }

        public void GridBind(int pageIndex, int pageSize)
        {
            this.dtSource.Rows.Clear();

            this.dtSource.AcceptChanges();

            this.Grid.Rows.Clear();


            if (LoadDataSourceHandle == null || BuildGridRowhandle == null)
            {
                return;
            }

            if (pageIndex == PageGridBunding.None)
            {
                return;
            }

            int inclusive = (pageIndex - 1) * pageSize + 1;
            int exclusive = inclusive + pageSize - 1;

            object[] objs = this.LoadDataSourceHandle(inclusive, exclusive);

            if (objs == null)
            {
                //				this.Grid.Page.Response.Write( @"<script language=javascript>alert('û�в�ѯ�����������ļ�¼��')</script>");
                //todo,Grid��ʾ��������ʾ
            }
            else
            {
                foreach (object obj in objs)
                {
                    DataRow row = this.BuildGridRowhandle(obj);

                    //�����ص�ϵͳ������ֵ
                    if (row.Table.Columns.Contains("GUID") && (row["GUID"] == null || row["GUID"].ToString() == string.Empty))
                        row["GUID"] = Guid.NewGuid().ToString();

                    //����ѡ����и�ֵ
                    if (row.Table.Columns.Contains(this.CheckColumnKey) && (row[this.CheckColumnKey] == null || row[this.CheckColumnKey].ToString() == string.Empty))
                        row[this.CheckColumnKey] = false;

                    //�����ص�ϵͳ������ֵ
                    if (row.Table.Columns.Contains(this.EditColumnKey) && (row[this.EditColumnKey] == null || row[this.EditColumnKey].ToString() == string.Empty))
                        row[this.EditColumnKey] = string.Empty;

                    dtSource.Rows.Add(row);
                }
            }
            this.Grid.ClearDataSource();
            this.Grid.DataBind();

            this.Grid.DataSource = dtSource;
            this.Grid.DataBind();
            if (this.Grid.Columns.FromKey(CheckColumnKey) != null)
            {
                if (this.Grid.Columns.FromKey(CheckColumnKey) is UnboundCheckBoxField)
                {
                    (this.Grid.Columns.FromKey(CheckColumnKey) as UnboundCheckBoxField).HeaderChecked = false;
                }
            }

        }

        #region add by sam

        public void GridNewBind(int pageIndex, int pageSize)
        {
            this.dtSource.Rows.Clear();

            this.dtSource.AcceptChanges();

            this.Grid.Rows.Clear();


            if (LoadDataSourceHandle == null || BuildGridRowhandle == null)
            {
                return;
            }

            if (pageIndex == PageGridBunding.None)
            {
                return;
            }

            int inclusive = (pageIndex - 1) * pageSize + 1;
            int exclusive = inclusive + pageSize - 1;

            object[] objs = this.LoadDataSourceHandle(inclusive, exclusive);

            if (objs == null)
            {
                //				this.Grid.Page.Response.Write( @"<script language=javascript>alert('û�в�ѯ�����������ļ�¼��')</script>");
                //todo,Grid��ʾ��������ʾ
            }
            else
            {
                foreach (object obj in objs)
                {
                    DataRow row = this.BuildGridRowhandle(obj);

                    //�����ص�ϵͳ������ֵ
                    if (row.Table.Columns.Contains("GUID") && (row["GUID"] == null || row["GUID"].ToString() == string.Empty))
                        row["GUID"] = Guid.NewGuid().ToString();

                    //����ѡ����и�ֵ
                    if (row.Table.Columns.Contains(this.CheckColumnKey) && (row[this.CheckColumnKey] == null || row[this.CheckColumnKey].ToString() == string.Empty))
                        row[this.CheckColumnKey] = true;

                    //�����ص�ϵͳ������ֵ
                    if (row.Table.Columns.Contains(this.EditColumnKey) && (row[this.EditColumnKey] == null || row[this.EditColumnKey].ToString() == string.Empty))
                        row[this.EditColumnKey] = string.Empty;

                    dtSource.Rows.Add(row);
                }
            }
            this.Grid.ClearDataSource();
            this.Grid.DataBind();

            this.Grid.DataSource = dtSource;
            this.Grid.DataBind();
            if (this.Grid.Columns.FromKey(CheckColumnKey) != null)
            {
                if (this.Grid.Columns.FromKey(CheckColumnKey) is UnboundCheckBoxField)
                {
                    (this.Grid.Columns.FromKey(CheckColumnKey) as UnboundCheckBoxField).HeaderChecked = true;
                }
            }

        }
        #endregion
        //public bool IsClickEditColumn(Infragistics.WebUI.UltraWebGrid.CellEventArgs e)
        //{
        //    return IsClickEditColumn(e.Cell);
        //}

        //public bool IsClickEditColumn(Infragistics.WebUI.UltraWebGrid.UltraGridCell cell)
        //{
        //    if (cell.Column.Key == EditColumnKey)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public bool IsClickColumn(string columnKey, Infragistics.WebUI.UltraWebGrid.CellEventArgs e)
        //{
        //    return IsClickColumn(columnKey, e.Cell);
        //}

        //public bool IsClickColumn(string columnKey, Infragistics.WebUI.UltraWebGrid.UltraGridCell cell)
        //{
        //    if (cell.Column.Key == columnKey)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        public void RequestData()
        {
            if (this.PagerToolBar != null)
            {
                if (this.PagerSizeSelector != null)
                {
                    this.PagerToolBar.PageSize = this.PagerSizeSelector.PageSize;
                }

                this.PagerToolBar.InitPager();

            }

            this.RefreshData();
        }

        public void RefreshData()
        {
            if (this.PagerToolBar != null)
            {
                if (this.GetRowCountHandle != null)
                {
                    this.PagerToolBar.RowCount = this.GetRowCountHandle();
                }
                this.Grid.Behaviors.Paging.PageIndex = this.PagerToolBar.PageIndex - 1;
                this.GridBind(this.PagerToolBar.PageIndex, this.PagerToolBar.PageSize);
            }
            else
            {
                this.GridBind(1, int.MaxValue);
            }
        }


        #region add  by sam 
        public void RequestNewData()
        {
            if (this.PagerToolBar != null)
            {
                if (this.PagerSizeSelector != null)
                {
                    this.PagerToolBar.PageSize = this.PagerSizeSelector.PageSize;
                }

                this.PagerToolBar.InitPager();

            }

            this.RefreshNewData();
        }

        public void RefreshNewData()
        {
            if (this.PagerToolBar != null)
            {
                if (this.GetRowCountHandle != null)
                {
                    this.PagerToolBar.RowCount = this.GetRowCountHandle();
                }
                this.Grid.Behaviors.Paging.PageIndex = this.PagerToolBar.PageIndex - 1;
                this.GridNewBind(this.PagerToolBar.PageIndex, this.PagerToolBar.PageSize);
            }
            else
            {
                this.GridNewBind(1, int.MaxValue);
            }
        } 
        #endregion

    }




    public class CustomColumnTemplate : System.Web.UI.ITemplate
    {
        private HorizontalAlign align = HorizontalAlign.Left;

        private DataControlRowType templateType = DataControlRowType.DataRow;

        private NewColumnStyle colStyle = NewColumnStyle.Text;

        private string columnName;

        private string cId;

        //public Control ChildControl;


        public CustomColumnTemplate(string controlId, string colname, NewColumnStyle style, DataControlRowType type, HorizontalAlign align)
        {

            this.columnName = colname;

            this.cId = controlId;


            this.templateType = type;


            this.colStyle = style;


            this.align = align;
        }



        public void InstantiateIn(System.Web.UI.Control container)
        {
            container.Controls.Clear();
            switch (templateType)
            {

                case DataControlRowType.Header:

                    HtmlGenericControl divControlHear = new HtmlGenericControl("div");
                    divControlHear.Style.Add("text-align", align.ToString());
                    divControlHear.ID = "divControlHear";
                    divControlHear.InnerText = columnName;
                    container.Controls.Add(divControlHear);
                    break;

                case DataControlRowType.DataRow:
                    switch (colStyle)
                    {
                        case NewColumnStyle.Text:

                            HtmlGenericControl divControlText = new HtmlGenericControl("div");
                            divControlText.Style.Add("text-align", align.ToString());
                            divControlText.ID = this.cId;
                            //divControlText.EnableViewState = true;
                            divControlText.DataBinding += new EventHandler(divControlText_DataBinding);
                            container.Controls.Add(divControlText);
                            break;

                        case NewColumnStyle.Link:

                            ImageButton btnLink = new ImageButton();
                            //ChildControl = btn;
                            btnLink.ID = "btnLink";
                            btnLink.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                            btnLink.CommandName = cId;
                            //btnLink.EnableViewState = true;
                            btnLink.Style.Add("tempIamgeBtnStyle", "cursor:pointer;;BACKGROUND-POSITION: center center;Background-repeat:no-repeat");

                            btnLink.ImageUrl = "/" + btnLink.TemplateSourceDirectory.Split('/')[1] + "/skin/image/detail16.gif";
                            //��ҳ��ش�����Ϊ���ݣ�����ˢ������                        
                            btnLink.DataBinding += new EventHandler(btnLink_DataBinding);

                            HtmlGenericControl divControlLink = new HtmlGenericControl("div");
                            divControlLink.Style.Add("text-align", align.ToString());
                            divControlLink.Controls.Add(btnLink);
                            divControlLink.ID = "divControlLink";
                            //divControlLink.EnableViewState = true;
                            container.Controls.Add(divControlLink);

                            break;

                        case NewColumnStyle.Edit:

                            ImageButton btnEdit = new ImageButton();
                            //ChildControl = btn;
                            btnEdit.ID = "btnEdit";
                            btnEdit.CommandName = cId;
                            btnEdit.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                            //btnEdit.EnableViewState = true;
                            btnEdit.Style.Add("tempIamgeBtnStyle", "cursor:pointer;;BACKGROUND-POSITION: center center;Background-repeat:no-repeat");

                            btnEdit.ImageUrl = "/" + btnEdit.TemplateSourceDirectory.Split('/')[1] + "/skin/image/edit16.gif";
                            btnEdit.DataBinding += new EventHandler(btnLink_DataBinding);

                            HtmlGenericControl divControlEdit = new HtmlGenericControl("div");
                            divControlEdit.Style.Add("text-align", align.ToString());
                            divControlEdit.Controls.Add(btnEdit);
                            divControlEdit.ID = "divControlEdit";
                            //divControlEdit.EnableViewState = true;
                            container.Controls.Add(divControlEdit);
                            break;

                        default:
                            break;
                    }
                    break;
                default:

                    // Insert code to handle unexpected values.  

                    break;
            }


        }

        void btnLink_DataBinding(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            btn.Attributes.Add("onclick", "document.forms[0].__EVENTTARGET.value='" + NewColumnStyle.Link.ToString() + "';document.forms[0].__EVENTARGUMENT.value='" + btn.CommandName + "," + ((btn.NamingContainer as TemplateContainer).Item as GridRecordItem).Record.Index + "'");
        }



        void divControlText_DataBinding(object sender, EventArgs e)
        {
            HtmlGenericControl control = sender as HtmlGenericControl;

            try
            {
                control.InnerText = ((control.NamingContainer as TemplateContainer).DataItem as DataRowView).Row[cId].ToString();
            }
            catch (NullReferenceException ex)
            {
            }
            //else if ((control.NamingContainer as TemplateContainer).Item != null)
            //    control.InnerText = ((control.NamingContainer as TemplateContainer).Item as GridRecordItem).Row.Items.FindItemByKey(cId).Value.ToString();
        }

    }


}
