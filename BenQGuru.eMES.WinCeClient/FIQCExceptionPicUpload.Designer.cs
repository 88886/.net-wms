﻿namespace BenQGuru.eMES.WinCeClient
{
    partial class FIQCExceptionPicUpload
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.ExampleDataTable = new System.Windows.Forms.DataGridTableStyle();
            this.Check = new System.Windows.Forms.DataGridTextBoxColumn();
            this.SERIAL = new System.Windows.Forms.DataGridTextBoxColumn();
            this.DocName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnUpPic = new System.Windows.Forms.Button();
            this.lbluppic = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGrid1.Location = new System.Drawing.Point(5, 74);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(231, 122);
            this.dataGrid1.TabIndex = 65;
            this.dataGrid1.TableStyles.Add(this.ExampleDataTable);
            this.dataGrid1.CurrentCellChanged += new System.EventHandler(this.dataGrid1_CurrentCellChanged);
            // 
            // ExampleDataTable
            // 
            this.ExampleDataTable.GridColumnStyles.Add(this.Check);
            this.ExampleDataTable.GridColumnStyles.Add(this.SERIAL);
            this.ExampleDataTable.GridColumnStyles.Add(this.DocName);
            this.ExampleDataTable.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            // 
            // Check
            // 
            this.Check.Format = "";
            this.Check.FormatInfo = null;
            this.Check.HeaderText = "选择";
            this.Check.MappingName = "选择";
            // 
            // SERIAL
            // 
            this.SERIAL.Format = "";
            this.SERIAL.FormatInfo = null;
            this.SERIAL.HeaderText = "SERIAL";
            this.SERIAL.MappingName = "SERIAL";
            // 
            // DocName
            // 
            this.DocName.Format = "";
            this.DocName.FormatInfo = null;
            this.DocName.HeaderText = "文件名";
            this.DocName.MappingName = "文件名";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "文件大小";
            this.dataGridTextBoxColumn1.MappingName = "文件大小";
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(196, 300);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(40, 20);
            this.btnReturn.TabIndex = 64;
            this.btnReturn.Text = "返回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnUpPic
            // 
            this.btnUpPic.Location = new System.Drawing.Point(26, 221);
            this.btnUpPic.Name = "btnUpPic";
            this.btnUpPic.Size = new System.Drawing.Size(72, 20);
            this.btnUpPic.TabIndex = 63;
            this.btnUpPic.Text = "图片上传";
            this.btnUpPic.Click += new System.EventHandler(this.btnUpPic_Click);
            // 
            // lbluppic
            // 
            this.lbluppic.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lbluppic.Location = new System.Drawing.Point(5, 9);
            this.lbluppic.Name = "lbluppic";
            this.lbluppic.Size = new System.Drawing.Size(184, 20);
            this.lbluppic.Text = "IQC异常图片上传";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(142, 221);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 20);
            this.btnDelete.TabIndex = 71;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(5, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 20);
            this.label3.Text = "消息提示:";
            // 
            // lblMessage
            // 
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(80, 262);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(156, 35);
            this.lblMessage.Text = "X";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FIQCExceptionPicUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lbluppic);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnUpPic);
            this.Name = "FIQCExceptionPicUpload";
            this.Size = new System.Drawing.Size(240, 320);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnUpPic;
        private System.Windows.Forms.Label lbluppic;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DataGridTableStyle ExampleDataTable;
        private System.Windows.Forms.DataGridTextBoxColumn Check;
        private System.Windows.Forms.DataGridTextBoxColumn SERIAL;
        private System.Windows.Forms.DataGridTextBoxColumn DocName;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
