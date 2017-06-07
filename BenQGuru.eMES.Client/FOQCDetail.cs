using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BenQGuru.eMES.Client
{
	/// <summary>
	/// FOQCDetail ��ժҪ˵����
	/// </summary>
	public class FOQCDetail : System.Windows.Forms.Form
	{
		private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid1;
		private System.Windows.Forms.GroupBox groupBox4;
		private UserControl.UCButton ucButton4;
		private UserControl.UCButton ucButton3;
		private System.Data.DataSet dataSet1;
		private System.Data.DataTable dataTable1;
		private System.Data.DataColumn dataColumn1;
		private System.Data.DataColumn dataColumn2;
		private System.Data.DataColumn dataColumn3;
		private System.Data.DataColumn dataColumn4;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FOQCDetail()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
			dataTable1.Rows.Add(new object[]{"�ֻ���������״��","Grade C","GOOD",""
											}
				);
			dataTable1.Rows.Add(new object[]{"�ֻ�����ƽ����","Grade C","GOOD",""
											}
				);
			dataTable1.Rows.Add(new object[]{"�ֻ���ʾ��LCD���","Grade C","GOOD",""
											}
				);
			UserControl.UIStyleBuilder.GridUI(ultraGrid1);
			UserControl.UIStyleBuilder.FormUI(this);	
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

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Table1", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column1");
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column2");
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column3");
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column4");
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FOQCDetail));
			this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.dataTable1 = new System.Data.DataTable();
			this.dataColumn1 = new System.Data.DataColumn();
			this.dataColumn2 = new System.Data.DataColumn();
			this.dataColumn3 = new System.Data.DataColumn();
			this.dataColumn4 = new System.Data.DataColumn();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.ucButton4 = new UserControl.UCButton();
			this.ucButton3 = new UserControl.UCButton();
			this.dataSet1 = new System.Data.DataSet();
			((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
			this.SuspendLayout();
			// 
			// ultraGrid1
			// 
			this.ultraGrid1.Cursor = System.Windows.Forms.Cursors.Default;
			this.ultraGrid1.DataSource = this.dataTable1;
			appearance1.BackColor = System.Drawing.Color.White;
			this.ultraGrid1.DisplayLayout.Appearance = appearance1;
			ultraGridColumn1.Header.Caption = "�����Ŀ";
			ultraGridColumn2.Header.Caption = "�ȼ�";
			ultraGridColumn3.Header.Caption = "���";
			ultraGridColumn4.Header.Caption = "��ע";
			ultraGridBand1.Columns.Add(ultraGridColumn1);
			ultraGridBand1.Columns.Add(ultraGridColumn2);
			ultraGridBand1.Columns.Add(ultraGridColumn3);
			ultraGridBand1.Columns.Add(ultraGridColumn4);
			this.ultraGrid1.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.ultraGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ultraGrid1.Location = new System.Drawing.Point(0, 0);
			this.ultraGrid1.Name = "ultraGrid1";
			this.ultraGrid1.Size = new System.Drawing.Size(528, 273);
			this.ultraGrid1.TabIndex = 0;
			// 
			// dataTable1
			// 
			this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
																			  this.dataColumn1,
																			  this.dataColumn2,
																			  this.dataColumn3,
																			  this.dataColumn4});
			this.dataTable1.TableName = "Table1";
			// 
			// dataColumn1
			// 
			this.dataColumn1.ColumnName = "Column1";
			// 
			// dataColumn2
			// 
			this.dataColumn2.ColumnName = "Column2";
			// 
			// dataColumn3
			// 
			this.dataColumn3.ColumnName = "Column3";
			// 
			// dataColumn4
			// 
			this.dataColumn4.ColumnName = "Column4";
			// 
			// groupBox4
			// 
			this.groupBox4.BackColor = System.Drawing.Color.White;
			this.groupBox4.Controls.Add(this.ucButton4);
			this.groupBox4.Controls.Add(this.ucButton3);
			this.groupBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox4.Location = new System.Drawing.Point(0, 209);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(528, 64);
			this.groupBox4.TabIndex = 156;
			this.groupBox4.TabStop = false;
			// 
			// ucButton4
			// 
			this.ucButton4.BackColor = System.Drawing.SystemColors.Control;
			this.ucButton4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucButton4.BackgroundImage")));
			this.ucButton4.ButtonType = UserControl.ButtonTypes.Exit;
			this.ucButton4.Caption = "�˳�";
			this.ucButton4.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ucButton4.Location = new System.Drawing.Point(288, 24);
			this.ucButton4.Name = "ucButton4";
			this.ucButton4.Size = new System.Drawing.Size(88, 22);
			this.ucButton4.TabIndex = 9;
			// 
			// ucButton3
			// 
			this.ucButton3.BackColor = System.Drawing.SystemColors.Control;
			this.ucButton3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucButton3.BackgroundImage")));
			this.ucButton3.ButtonType = UserControl.ButtonTypes.Confirm;
			this.ucButton3.Caption = "ȷ��";
			this.ucButton3.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ucButton3.Location = new System.Drawing.Point(144, 24);
			this.ucButton3.Name = "ucButton3";
			this.ucButton3.Size = new System.Drawing.Size(88, 22);
			this.ucButton3.TabIndex = 8;
			// 
			// dataSet1
			// 
			this.dataSet1.DataSetName = "NewDataSet";
			this.dataSet1.Locale = new System.Globalization.CultureInfo("zh-CN");
			this.dataSet1.Tables.AddRange(new System.Data.DataTable[] {
																		  this.dataTable1});
			// 
			// FOQCDetail
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(528, 273);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.ultraGrid1);
			this.Name = "FOQCDetail";
			this.Text = "�����ϸ";
			((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
			this.groupBox4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
