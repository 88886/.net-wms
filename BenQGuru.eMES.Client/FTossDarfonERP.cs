using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;	

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Common.DomainDataProvider;
using Infragistics.Win.UltraWinGrid;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Client.Service;
using UserControl;
using BenQGuru.eMES.OQC;

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FTossDarfonERP ��ժҪ˵����
	/// </summary>
	public class FTossDarfonERP : System.Windows.Forms.Form
	{
		protected DataTable dtToss = new DataTable();
		private System.Windows.Forms.Panel pnlMain;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugrdREC;
		private System.Windows.Forms.Panel pnlControl;
		private UserControl.UCButton btnToss;
		private UserControl.UCButton btnCancle;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FTossDarfonERP()
		{
			//
			// Windows ���������֧���������
			//
			
			InitializeComponent();
			UserControl.UIStyleBuilder.FormUI(this);
			UserControl.UIStyleBuilder.GridUI(ugrdREC);
			ugrdREC.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Default;
			//ugrdREC.DisplayLayout.Override.al

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}
//
//		public FTossDarfonERP(string recno,string mocode)
//		{
//			//
//			// Windows ���������֧���������
//			//
//			InitializeComponent();
//			UserControl.UIStyleBuilder.FormUI(this);
//			UserControl.UIStyleBuilder.GridUI(ugrdREC);
//
//			//
//			// TODO: �� InitializeComponent ���ú�����κι��캯������
//			//
//		}
		public IDomainDataProvider DataProvider
		{
			get
			{
				return _domainDataProvider;
			}
		}

		private IDomainDataProvider _domainDataProvider = ApplicationService.Current().DataProvider;

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

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FTossDarfonERP));
			this.pnlMain = new System.Windows.Forms.Panel();
			this.ugrdREC = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.pnlControl = new System.Windows.Forms.Panel();
			this.btnToss = new UserControl.UCButton();
			this.btnCancle = new UserControl.UCButton();
			this.pnlMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ugrdREC)).BeginInit();
			this.pnlControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlMain
			// 
			this.pnlMain.Controls.Add(this.ugrdREC);
			this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlMain.Location = new System.Drawing.Point(0, 0);
			this.pnlMain.Name = "pnlMain";
			this.pnlMain.Size = new System.Drawing.Size(560, 416);
			this.pnlMain.TabIndex = 0;
			// 
			// ugrdREC
			// 
			this.ugrdREC.Cursor = System.Windows.Forms.Cursors.Default;
			this.ugrdREC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ugrdREC.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
			this.ugrdREC.Location = new System.Drawing.Point(0, 0);
			this.ugrdREC.Name = "ugrdREC";
			this.ugrdREC.Size = new System.Drawing.Size(560, 416);
			this.ugrdREC.TabIndex = 20;
			this.ugrdREC.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugrdREC_InitializeLayout);
			// 
			// pnlControl
			// 
			this.pnlControl.Controls.Add(this.btnCancle);
			this.pnlControl.Controls.Add(this.btnToss);
			this.pnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlControl.Location = new System.Drawing.Point(0, 416);
			this.pnlControl.Name = "pnlControl";
			this.pnlControl.Size = new System.Drawing.Size(560, 37);
			this.pnlControl.TabIndex = 1;
			// 
			// btnToss
			// 
			this.btnToss.BackColor = System.Drawing.SystemColors.Control;
			this.btnToss.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnToss.BackgroundImage")));
			this.btnToss.ButtonType = UserControl.ButtonTypes.Confirm;
			this.btnToss.Caption = "��ת";
			this.btnToss.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnToss.Location = new System.Drawing.Point(160, 8);
			this.btnToss.Name = "btnToss";
			this.btnToss.Size = new System.Drawing.Size(88, 22);
			this.btnToss.TabIndex = 18;
			this.btnToss.Click += new System.EventHandler(this.btnToss_Click);
			// 
			// btnCancle
			// 
			this.btnCancle.BackColor = System.Drawing.SystemColors.Control;
			this.btnCancle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancle.BackgroundImage")));
			this.btnCancle.ButtonType = UserControl.ButtonTypes.Exit;
			this.btnCancle.Caption = "�˳�";
			this.btnCancle.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnCancle.Location = new System.Drawing.Point(280, 7);
			this.btnCancle.Name = "btnCancle";
			this.btnCancle.Size = new System.Drawing.Size(88, 22);
			this.btnCancle.TabIndex = 19;
			// 
			// FTossDarfonERP
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(560, 453);
			this.Controls.Add(this.pnlControl);
			this.Controls.Add(this.pnlMain);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FTossDarfonERP";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "���������ת";
			this.Load += new System.EventHandler(this.FTossDarfonERP_Load);
			this.pnlMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ugrdREC)).EndInit();
			this.pnlControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void InitialForm()
		{
			FillDataGrid(LoadDataSource());
		}

		//Initialize Grid and build columns
		private void InitializeGrid()
		{
			dtToss.Columns.Clear();
			//dtIDList.Columns.Add("checkbox",typeof(System.Boolean));
			dtToss.Columns.Add("��ⵥ��",typeof(string)).ReadOnly = true;
			dtToss.Columns.Add("������",typeof(string)).ReadOnly = true;
			dtToss.Columns.Add("��Ʒ����",typeof(string)).ReadOnly = true;
			//dtIDList.Columns.Add("stepsequence",typeof(string)).ReadOnly = true;
			dtToss.Columns.Add("����",typeof(int)).ReadOnly = true;
			//dtIDList.Columns.Add("cartonno",typeof(string)).ReadOnly = true;
			//dtIDList.Columns.Add("collecttype",typeof(string)).ReadOnly = true;

			this.ugrdREC.DataSource = dtToss;
		}

		private object[] LoadDataSource()
		{
			return (new InventoryFacade(DataProvider)).GetAllERPINVInterface();

		}

		private void FillDataGrid(object[] objs)
		{
			dtToss.Clear();
			if(objs != null)
			{
				foreach(ERPINVInterface erp in objs)
				{
					if(erp.STATUS != Web.Helper.INVERPType.INVERPTYPE_PROCESSED)
					{
						dtToss.Rows.Add(new object[]{
														erp.RECNO
														,erp.MOCODE	
														,erp.ITEMCODE
														,erp.QTY
													});
					}
				}
				dtToss.AcceptChanges();
			}
		}

		private void btnToss_Click(object sender, System.EventArgs e)
		{
			Common.Domain.IDomainDataProvider erpDataProvider = Common.DomainDataProvider.DomainDataProviderManager.DomainDataProvider(DBName.ERP);
			//ugrdREC.Selected
			
			
			ERPSRMX srmx = new ERPSRMX();

			string factory = "PO";
			if (System.Configuration.ConfigurationSettings.AppSettings["InvFactory"] != null)
			{
				factory = System.Configuration.ConfigurationSettings.AppSettings["InvFactory"].Trim();
			}

						
			InventoryFacade mesFac = new InventoryFacade(DataProvider);
			//Laws Lu,2006/11/13 uniform system collect date
			DBDateTime dbDateTime;
			
				dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
				
			DateTime workDateTime = FormatHelper.ToDateTime(dbDateTime.DBDate,dbDateTime.DBTime);


			srmx.SRMXNO = mesFac.GetMaxSRNO(int.Parse(workDateTime.Year.ToString().Substring(2,2)),factory);

			ArrayList arSRMX = new ArrayList();

//			foreach(Infragistics.Win.UltraWinGrid.UltraGridRow row in ugrdREC.Rows)
			for ( int iGridRowLoopIndex = 0; iGridRowLoopIndex < ugrdREC.Rows.Count; iGridRowLoopIndex++)
			{
				Infragistics.Win.UltraWinGrid.UltraGridRow row = ugrdREC.Rows[iGridRowLoopIndex];
				if(row.Selected == true)
				{
					DataProvider.BeginTransaction();
					erpDataProvider.BeginTransaction();

					try
					{

						srmx.ENDATE = FormatHelper.TODateInt(workDateTime);
						string mocode = row.Cells["������"].Text.Trim().Substring(5, row.Cells["������"].Text.Trim().Length - 5);
						srmx.SONO =  Convert.ToInt32(mocode);
						srmx.FINQTY  = Convert.ToDecimal(row.Cells["����"].Text.Trim());
						srmx.SRPROD =  row.Cells["��Ʒ����"].Text.Trim();
						srmx.SRDESC =  row.Cells["��Ʒ����"].Text.Trim();
						srmx.UID = ApplicationService.Current().UserCode;
						srmx.STA = "F";
						

						if(!arSRMX.Contains(srmx.SRMXNO + srmx.SONO))
						{
							arSRMX.Add(srmx.SRMXNO + srmx.SONO.ToString());
							//��ERP�в�������
							erpDataProvider.Insert(srmx);
						}
						else
						{
							erpDataProvider.CustomExecute(new SQLCondition("UPDATE SRMX SET \"uid\" = '" 
								+ ApplicationService.Current().UserCode 
								+ "',FINQTY = FINQTY + " 
								+  Convert.ToDecimal(row.Cells["����"].Text.Trim())
								+ ",ENDATE = " +  FormatHelper.TODateInt(workDateTime)
								+ " WHERE SRMXNO = '" + srmx.SRMXNO + "' AND SONO = " + srmx.SONO));
						}

						object objErp = mesFac.GetERPINVInterface(
							row.Cells["��ⵥ��"].Text.Trim(),
							row.Cells["������"].Text.Trim(),
							Web.Helper.INVERPType.INVERPTYPE_NEW
							,InventoryFacade.NewSRNO);

						if(objErp != null)
						{
							ERPINVInterface erp = objErp as ERPINVInterface;
							//����ϵͳ����
							erp.SRNO = srmx.SRMXNO;
							erp.UPLOADUSER  = ApplicationService.Current().UserCode;
							erp.UPLOADDATE  = FormatHelper.TODateInt(workDateTime);
							erp.UPLOADTIME  = FormatHelper.TOTimeInt(workDateTime);
							erp.STATUS = Web.Helper.INVERPType.INVERPTYPE_PROCESSED;
							mesFac.UpdateERPINVInterfaceStatus(erp);
						}

						DataProvider.CommitTransaction();
						erpDataProvider.CommitTransaction();

					}
					catch(Exception ex)
					{
						Log.Error(ex.Message);
						ApplicationRun.GetInfoForm().Add(new UserControl.Message(ex));
						
						erpDataProvider.RollbackTransaction();
						DataProvider.RollbackTransaction();
					}
					finally
					{
						((SQLDomainDataProvider)DataProvider).PersistBroker.CloseConnection();
						((SQLDomainDataProvider)erpDataProvider).PersistBroker.CloseConnection();
					}
				}
			}
			ugrdREC_InitializeLayout(null,null);
			InitializeGrid();
			InitialForm();
		}

		private void ugrdREC_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			UltraWinGridHelper ultraWinGridHelper = new UltraWinGridHelper(this.ugrdREC);
			//ultraWinGridHelper.AddCheckColumn("checkbox","*");
			ultraWinGridHelper.AddCommonColumn("��ⵥ��","��ⵥ��");
			ultraWinGridHelper.AddCommonColumn("������","������");
			ultraWinGridHelper.AddCommonColumn("��Ʒ����","��Ʒ����");
			//ultraWinGridHelper.AddCommonColumn("stepsequence","������");
			ultraWinGridHelper.AddCommonColumn("����","����");
		}

		private void FTossDarfonERP_Load(object sender, System.EventArgs e)
		{
			InitializeGrid();
			InitialForm();
		}
	}
}
