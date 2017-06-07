using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using BenQGuru.eMES.OQC;
using BenQGuru.eMES.Domain.OQC;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Client.Service;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.DataCollect.Action;
using BenQGuru.eMES.DataCollect;
using UserControl;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.MOModel;

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FfunTestInfo ��ժҪ˵����
	/// </summary>
	public class FfunTestInfo : System.Windows.Forms.Form
	{
		private string _lotno;
		private DataTable dtIDList = new DataTable();

		private UserControl.UCLabelEdit txtFunTestQty;
		private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridMain;
		private UserControl.UCButton btnExit;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FfunTestInfo()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();
			UserControl.UIStyleBuilder.FormUI(this);	
			UserControl.UIStyleBuilder.GridUI(ultraGridMain);	

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		public FfunTestInfo(string lotno)
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();
			UserControl.UIStyleBuilder.FormUI(this);	
			UserControl.UIStyleBuilder.GridUI(ultraGridMain);	

			_lotno = lotno;
			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void InitializeGrid()
		{
			dtIDList.Columns.Clear();
			dtIDList.Columns.Add("runningcard",typeof(string)).ReadOnly = true;
			dtIDList.Columns.Add("result",typeof(string)).ReadOnly = true;

			this.ultraGridMain.DataSource = dtIDList;
		}

		private void ultraGridMain_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			UltraWinGridHelper ultraWinGridHelper = new UltraWinGridHelper(this.ultraGridMain);
			ultraWinGridHelper.AddCommonColumn("runningcard","��Ʒ���к�");
			ultraWinGridHelper.AddCommonColumn("result","���Խ��");

			//			foreach(UltraGridBand ub in ultraGridMain.co
		}

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FfunTestInfo));
			this.txtFunTestQty = new UserControl.UCLabelEdit();
			this.ultraGridMain = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.btnExit = new UserControl.UCButton();
			((System.ComponentModel.ISupportInitialize)(this.ultraGridMain)).BeginInit();
			this.SuspendLayout();
			// 
			// txtFunTestQty
			// 
			this.txtFunTestQty.AllowEditOnlyChecked = true;
			this.txtFunTestQty.Caption = "���ܲ�������";
			this.txtFunTestQty.Checked = false;
			this.txtFunTestQty.EditType = UserControl.EditTypes.Integer;
			this.txtFunTestQty.Location = new System.Drawing.Point(24, 16);
			this.txtFunTestQty.MaxLength = 40;
			this.txtFunTestQty.Multiline = false;
			this.txtFunTestQty.Name = "txtFunTestQty";
			this.txtFunTestQty.PasswordChar = '\0';
			this.txtFunTestQty.ReadOnly = true;
			this.txtFunTestQty.ShowCheckBox = false;
			this.txtFunTestQty.Size = new System.Drawing.Size(220, 24);
			this.txtFunTestQty.TabIndex = 1;
			this.txtFunTestQty.TabNext = true;
			this.txtFunTestQty.Value = "";
			this.txtFunTestQty.WidthType = UserControl.WidthTypes.Normal;
			this.txtFunTestQty.XAlign = 111;
			// 
			// ultraGridMain
			// 
			this.ultraGridMain.Cursor = System.Windows.Forms.Cursors.Default;
			this.ultraGridMain.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
			this.ultraGridMain.Location = new System.Drawing.Point(16, 48);
			this.ultraGridMain.Name = "ultraGridMain";
			this.ultraGridMain.Size = new System.Drawing.Size(464, 240);
			this.ultraGridMain.TabIndex = 8;
			this.ultraGridMain.Text = "���ܲ��Բ�Ʒ";
			this.ultraGridMain.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridMain_InitializeLayout);
			// 
			// btnExit
			// 
			this.btnExit.BackColor = System.Drawing.SystemColors.Control;
			this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
			this.btnExit.ButtonType = UserControl.ButtonTypes.Exit;
			this.btnExit.Caption = "�˳�";
			this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnExit.Location = new System.Drawing.Point(384, 16);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(88, 22);
			this.btnExit.TabIndex = 10;
			// 
			// FfunTestInfo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(496, 293);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.ultraGridMain);
			this.Controls.Add(this.txtFunTestQty);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FfunTestInfo";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "���ܲ��Բ�����Ϣ";
			this.Load += new System.EventHandler(this.FfunTestInfo_Load);
			((System.ComponentModel.ISupportInitialize)(this.ultraGridMain)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private IDomainDataProvider _domainDataProvider =ApplicationService.Current().DataProvider;
		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		private void FfunTestInfo_Load(object sender, System.EventArgs e)
		{
			InitializeGrid();
			LoadData();
		}

		private void LoadData()
		{
			OQCFacade oqcFacade = new OQCFacade(this.DataProvider);
			object[] objs = oqcFacade.GetOQCFuncTestValueByLotNo(this._lotno, OQCFacade.Lot_Sequence_Default);
			if (objs != null)
			{
				for (int i = 0; i < objs.Length; i++)
				{
					OQCFuncTestValue testValue = (OQCFuncTestValue)objs[i];
					DataRow row = dtIDList.NewRow();
					row["runningcard"] = testValue.RunningCard;
					row["result"] = testValue.ProductStatus;
					dtIDList.Rows.Add(row);
				}
				this.txtFunTestQty.Value = objs.Length.ToString();
			}
			else
			{
				this.txtFunTestQty.Value = "0";
			}
		}

	}
}
