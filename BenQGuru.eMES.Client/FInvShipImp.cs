#region using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Material;
using BenQGuru.eMES.MOModel;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Client.Service;
using UserControl;
using BenQGuru.eMES.OQC;
#endregion

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FInvShipImp ��ժҪ˵����
	/// </summary>
	public class FInvShipImp : System.Windows.Forms.Form
	{
		#region ������������
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.Panel panelMiddle;
		private UserControl.UCLabelEdit txtFileName;
		private UserControl.UCButton btnRead;
		private UserControl.UCButton btnBrowse;
		private UserControl.UCButton btnExit;
		private UserControl.UCButton btnSave;
		private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridContent;
		private System.Data.DataTable _tmpTable = new System.Data.DataTable();
		private IDomainDataProvider _domainDataProvider;
		private FInfoForm _infoForm;
		private System.Windows.Forms.OpenFileDialog fileDialog;

		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region ϵͳ���ܺͷ���
		public FInvShipImp()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

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

		public IDomainDataProvider DataProvider
		{			 
			get
			{
				if(_domainDataProvider == null)
					_domainDataProvider = ApplicationService.Current().DataProvider;
				return _domainDataProvider;
			}
		}

		private void FInvShipImp_Closed(object sender, System.EventArgs e)
		{
			if (this.DataProvider!=null)
				((BenQGuru.eMES.Common.DomainDataProvider.SQLDomainDataProvider)this.DataProvider).PersistBroker.CloseConnection();  
		}

		private void ErrorMsg(string msg)
		{
			try
			{
				_infoForm.Add(new UserControl.Message(UserControl.MessageType.Error,msg));
			}
			catch
			{}
		}

		private void SucessMsg(string msg)
		{
			try
			{
				_infoForm.Add(new UserControl.Message(UserControl.MessageType.Success,msg));
			}
			catch
			{}
		}

		#endregion

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FInvShipImp));
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnRead = new UserControl.UCButton();
            this.btnBrowse = new UserControl.UCButton();
            this.txtFileName = new UserControl.UCLabelEdit();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnExit = new UserControl.UCButton();
            this.btnSave = new UserControl.UCButton();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.ultraGridContent = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridContent)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnRead);
            this.panelTop.Controls.Add(this.btnBrowse);
            this.panelTop.Controls.Add(this.txtFileName);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(734, 37);
            this.panelTop.TabIndex = 0;
            // 
            // btnRead
            // 
            this.btnRead.BackColor = System.Drawing.SystemColors.Control;
            this.btnRead.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRead.BackgroundImage")));
            this.btnRead.ButtonType = UserControl.ButtonTypes.None;
            this.btnRead.Caption = "��ȡ�ļ�����";
            this.btnRead.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRead.Location = new System.Drawing.Point(578, 7);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(88, 22);
            this.btnRead.TabIndex = 2;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColor = System.Drawing.SystemColors.Control;
            this.btnBrowse.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBrowse.BackgroundImage")));
            this.btnBrowse.ButtonType = UserControl.ButtonTypes.None;
            this.btnBrowse.Caption = "���";
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.Location = new System.Drawing.Point(484, 7);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(88, 22);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.AllowEditOnlyChecked = true;
            this.txtFileName.Caption = "�����ļ���";
            this.txtFileName.Checked = false;
            this.txtFileName.EditType = UserControl.EditTypes.String;
            this.txtFileName.Location = new System.Drawing.Point(5, 7);
            this.txtFileName.MaxLength = 40;
            this.txtFileName.Multiline = false;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.PasswordChar = '\0';
            this.txtFileName.ReadOnly = false;
            this.txtFileName.ShowCheckBox = false;
            this.txtFileName.Size = new System.Drawing.Size(473, 23);
            this.txtFileName.TabIndex = 0;
            this.txtFileName.TabNext = true;
            this.txtFileName.Value = "";
            this.txtFileName.WidthType = UserControl.WidthTypes.TooLong;
            this.txtFileName.XAlign = 78;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.btnExit);
            this.panelBottom.Controls.Add(this.btnSave);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 240);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(734, 33);
            this.panelBottom.TabIndex = 1;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.ButtonType = UserControl.ButtonTypes.Exit;
            this.btnExit.Caption = "�˳�";
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Location = new System.Drawing.Point(631, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(88, 22);
            this.btnExit.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.ButtonType = UserControl.ButtonTypes.None;
            this.btnSave.Caption = "����";
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Location = new System.Drawing.Point(537, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 22);
            this.btnSave.TabIndex = 0;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.ultraGridContent);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 37);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(734, 203);
            this.panelMiddle.TabIndex = 2;
            // 
            // ultraGridContent
            // 
            this.ultraGridContent.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraGridContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridContent.Location = new System.Drawing.Point(0, 0);
            this.ultraGridContent.Name = "ultraGridContent";
            this.ultraGridContent.Size = new System.Drawing.Size(734, 203);
            this.ultraGridContent.TabIndex = 9;
            this.ultraGridContent.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridContent_InitializeLayout);
            // 
            // fileDialog
            // 
            this.fileDialog.Filter = "CSV�ļ�|*.csv";
            // 
            // FInvShipImp
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(734, 273);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "FInvShipImp";
            this.Text = "����������";
            this.Load += new System.EventHandler(this.FInvShipImp_Load);
            this.Closed += new System.EventHandler(this.FInvShipImp_Closed);
            this.panelTop.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelMiddle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridContent)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region ������ʾ����
		private void FInvShipImp_Load(object sender, System.EventArgs e)
		{
			_infoForm = ApplicationRun.GetInfoForm();

			UserControl.UIStyleBuilder.FormUI(this);

			#region ��ʼ��Grid
			UserControl.UIStyleBuilder.GridUI(this.ultraGridContent);
			_tmpTable = new DataTable();
			_tmpTable.Columns.Clear();
	
			_tmpTable.Columns.Add("ShipNo",typeof(string));
			_tmpTable.Columns.Add("Seq",typeof(string));
			_tmpTable.Columns.Add( "ShipType", typeof( string ));
			_tmpTable.Columns.Add( "PartnerCode", typeof( string ));
			_tmpTable.Columns.Add( "PartnerDesc", typeof( string ));
			_tmpTable.Columns.Add( "CustomerOrderNo", typeof( string ));
			_tmpTable.Columns.Add( "ShipMethod", typeof( string ));
			_tmpTable.Columns.Add( "ShipDate", typeof( string ) );
			//_tmpTable.Columns.Add( "��Ʒ��", typeof( string ) );
			_tmpTable.Columns.Add( "ItemCode", typeof( string ) );
			_tmpTable.Columns.Add( "ShipQty", typeof( string ) );
			_tmpTable.Columns.Add( "PrintDate", typeof( string ) );

			ultraGridContent.DataSource = _tmpTable;

			//this.ultraGridContent.DisplayLayout.Bands[0].Columns["������"].Hidden = true;
			this.txtFileName.InnerTextBox.Multiline = false;
			#endregion
		}

		private void ultraGridContent_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			UltraWinGridHelper gridHelper = new UltraWinGridHelper(this.ultraGridContent);
			gridHelper.AddCommonColumn("ShipNo","��������");
			gridHelper.AddCommonColumn("Seq","���");
			gridHelper.AddCommonColumn("ShipType","����������");
			gridHelper.AddCommonColumn("PartnerCode","�ͻ�����/Location");
			gridHelper.AddCommonColumn("PartnerDesc","�ͻ�����");
			gridHelper.AddCommonColumn("CustomerOrderNo","�ͻ�������");
			gridHelper.AddCommonColumn("ShipMethod","������ʽ");
			gridHelper.AddCommonColumn("ShipDate","��������");
			//_tmpTable.Columns.Add( "��Ʒ��", typeof( string ) );
			gridHelper.AddCommonColumn("ItemCode","��Ʒ����" );
			gridHelper.AddCommonColumn("ShipQty","����������" );
			gridHelper.AddCommonColumn("PrintDate","������" );
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			if( fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.txtFileName.InnerTextBox.Text = fileDialog.FileName;
			}
		}

		private void ClearDataRow()
		{
			this._tmpTable.Rows.Clear();
			this.ultraGridContent.DataSource = null;
			this.ultraGridContent.DataSource = this._tmpTable;
		}
		#endregion

		
		#region ���ļ�����������
		
		private void btnRead_Click(object sender, System.EventArgs e)
		{
			if(this.txtFileName.InnerTextBox.Text.Trim() == string.Empty)
			{
				this.ErrorMsg("$Error_Input_FileName");//�������ļ���
				this.txtFileName.TextFocus(false, true);
				return;
			}
			
			this._tmpTable.Rows.Clear();
			this.ultraGridContent.DataSource = null;
			this.ultraGridContent.DataSource = this._tmpTable;

			ReadFileData2();
		}
		
		private void ReadFileData2()
		{
			try
			{
				string fileName = txtFileName.InnerTextBox.Text.Trim();
            
				string configFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
					"DataFileParser.xml");

				BenQGuru.eMES.Material.DataFileParser parser = new BenQGuru.eMES.Material.DataFileParser();
				parser.FormatName = "INVSHIP" ;
				parser.ConfigFile = configFile ;
				parser.CheckValidHandle = null;
				try
				{
					object[] objs = parser.Parse(fileName) ;
					if(objs != null && objs.Length > 0)
					{
						foreach(object obj in objs)
						{
							InvShip ship = obj as InvShip;
							if(ship != null)
							{
								this._tmpTable.Rows.Add(new string[]
												{
													ship.ShipNo.ToUpper(),
													ship.ShipSeq.ToString(),
													ship.ShipType,
													ship.PartnerCode.ToUpper(),
													ship.PartnerDesc,
													ship.CustomerOrderNo,
													ship.ShipMethod,
													FormatHelper.ToDateString(ship.ShipDate),
													ship.ItemCode.ToUpper(),
													ship.PlanQty.ToString(),
													FormatHelper.ToDateString(ship.PrintDate)
												});
							}
						}
					}
				}
				finally
				{
					parser.CloseFile();
				}
			}
			catch(System.IO.IOException ex)
			{
				this.ErrorMsg("$Error_FileReadError "+ex.Message);
				return;
			}
		}

		#region OLD
//		private void ReadFileData()
//		{
//			System.IO.FileStream fs = null;
//			System.IO.StreamReader sr = null;
//			try
//			{
//				this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
//
//				ClearDataRow();
//
//				fs = new System.IO.FileStream(txtFileName.InnerTextBox.Text.Trim(),
//					System.IO.FileMode.Open,System.IO.FileAccess.Read);
//
//				sr = new System.IO.StreamReader(fs,System.Text.Encoding.Default);
//				
//				string line = sr.ReadLine();
//				while(line != null && line != string.Empty)
//				{
//					string[] arr = line.Split(',');
//					if(arr.Length != 9)
//					{
//						this.ErrorMsg("�ļ������ݸ�ʽ����");
//						return;
//					}
//	
//					arr[4] = DateStringConv(arr[4]);
//					arr[8] = DateStringConv(arr[8]);
//
//					this._tmpTable.Rows.Add(arr);
//					line = sr.ReadLine();
//				}
//				this.SucessMsg("�ļ���ȡ����");
//			}
//			catch(System.IO.IOException ex)
//			{
//				this.ErrorMsg("��ȡ�ļ����� "+ex.Message);
//				return;
//			}
//			finally
//			{
//				if(sr != null)
//					sr.Close();
//				if(fs != null)
//					fs.Close();
//
//				this.Cursor = System.Windows.Forms.Cursors.Default;
//			}
//		}
//		private string DateStringConv(string datefrom)
//		{
//			string[] strList = datefrom.Split(new char[]{'.','-','/'});
//			if(strList.Length != 3)
//				throw new Exception("���ڸ�ʽ����");
//
//			return strList[0] + "-" + strList[1] + "-" + strList[2];
//		}
		#endregion

		#endregion

		#region	��������д�����ݿ���	
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

				this.DataProvider.BeginTransaction();
				BenQGuru.eMES.Material.InventoryFacade facade = new InventoryFacade(this.DataProvider);
				BenQGuru.eMES.MOModel.ItemFacade itemFacade = new ItemFacade(this.DataProvider);
				BenQGuru.eMES.MOModel.ModelFacade modelFacade = new ModelFacade(this.DataProvider);
				//һ���е�д��
				foreach(DataRow dr in this._tmpTable.Rows)
				{
					//�ж��Ƿ��Ѿ������ɼ�
					string shipNo = dr[0].ToString().ToUpper();
					int shipSeq = int.Parse(dr[1].ToString());
					string itemcode = dr["ItemCode"].ToString().ToUpper();
					string partnercode = dr["PartnerCode"].ToString().ToUpper();

					//�жϴ˳������Ƿ����(���ݵ��ţ���Ʒ���룬�����̴��룩
					object[] objs  = facade.QueryInvShip(shipNo,itemcode,partnercode);
					if(objs != null && objs.Length > 0)
					{
						InvShip shipt = objs[0] as InvShip;
						if(shipt != null)
						{
							if(shipt.ShipStatus == ShipStatus.Shipped)
							{
								throw new Exception(shipNo + " $CS_Inv_Ship_has_Shiped"); //�Ѿ���ɳ����ˣ��������µ���
							}
							if(shipt.ActQty > 0)
							{
								throw new Exception(shipNo + " $CS_Inv_Ship_has_Collected");//�Ѿ����������ɼ������Ƚ��ɼ������к�ɾ��
							}
							facade.DeleteInvShip(shipNo,itemcode,partnercode);
						}
					}

					//int count = this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLINVRCARD where 1=1 and shipNo ='{0}' and shipSeq={1}" , shipNo,shipSeq)));
					//if(count > 0)
					//	throw new Exception(shipNo + "����������Ѿ����������ɼ����������µ���");

					//�жϴ˳������Ƿ����
					//count = this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLINVSHIP where 1=1 and shipNo = '{0}' " , shipNo)));
					//if(count > 0)
					//{
					//	this.DataProvider.CustomExecute(new SQLCondition(string.Format("delete from tblinvship where shipNo='{0}'",shipNo)));
					//}
					//����������д�뵽���ݿ�
					

					InvShip ship = facade.CreateNewInvShip();
					ship.ActQty = 0;
					ship.ItemCode = dr[8].ToString();

                    BenQGuru.eMES.Domain.MOModel.Item item = itemFacade.GetItem(ship.ItemCode, GlobalVariables.CurrentOrganizations.First().OrganizationID) as BenQGuru.eMES.Domain.MOModel.Item;
					if(item != null)
					{
						ship.ItemDesc = item.ItemDescription;
						BenQGuru.eMES.Domain.MOModel.Model model = modelFacade.GetModelByItemCode(item.ItemCode) as BenQGuru.eMES.Domain.MOModel.Model;
						if(model != null)
							ship.ModelCode = model.ModelCode;
						else
							ship.ModelCode = " ";
					}
					else
					{
						ship.ItemDesc = string.Empty;
						
					}

					ship.MainitainUser = ApplicationService.Current().UserCode;
					//Laws Lu,2006/11/13 uniform system collect date
					DBDateTime dbDateTime;
					
					dbDateTime = FormatHelper.GetNowDBDateTime(DataProvider);
					

					ship.MaintainDate = dbDateTime.DBDate;
					ship.MaintainTime = dbDateTime.DBTime;

					ship.PartnerCode = dr[3].ToString().ToUpper();
					ship.PartnerDesc = dr[4].ToString();
					ship.PlanQty = int.Parse(dr[9].ToString());
					ship.ShipDate = FormatHelper.TODateInt(dr[7].ToString());
					ship.ShipNo = dr[0].ToString().ToUpper();
					ship.ShipStatus = ShipStatus.Shipping;
					ship.ShipDesc = string.Empty;
					ship.ShipInnerType = BenQGuru.eMES.Material.ReceiveInnerType.Normal;
					ship.ShipType = dr[2].ToString();
					ship.ShipUser = string.Empty;
					ship.PrintDate = FormatHelper.TODateInt(dr[10].ToString());
					ship.ShipSeq = shipSeq;
					ship.CustomerOrderNo = dr[5].ToString().ToUpper();
					ship.ShipMethod=dr[6].ToString().ToUpper();

					facade.AddInvShip2(ship);
				}

				this.DataProvider.CommitTransaction();
				this.SucessMsg("$CS_Save_Success");
			}
			catch(System.Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				this.ErrorMsg(ex.Message);
			}
			finally
			{
				this.Cursor = System.Windows.Forms.Cursors.Default;
			}
		}

		#endregion

	}
}
