using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UserControl
{
	/// <summary>
	/// UCBanner ��ժҪ˵����
	/// </summary>
	public class UCBanner : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.PictureBox pbBanner;
		private System.Windows.Forms.Label lblBannerCaption;
		/// <summary> 
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UCBanner()
		{
			// �õ����� Windows.Forms ���������������ġ�
			InitializeComponent();

			// TODO: �� InitializeComponent ���ú�����κγ�ʼ��

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

		#region �����������ɵĴ���
		/// <summary> 
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
		/// �޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UCBanner));
			this.pbBanner = new System.Windows.Forms.PictureBox();
			this.lblBannerCaption = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pbBanner
			// 
			this.pbBanner.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.pbBanner.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.pbBanner.Image = ((System.Drawing.Image)(resources.GetObject("pbBanner.Image")));
			this.pbBanner.Location = new System.Drawing.Point(56, -4);
			this.pbBanner.Name = "pbBanner";
			this.pbBanner.Size = new System.Drawing.Size(584, 34);
			this.pbBanner.TabIndex = 13;
			this.pbBanner.TabStop = false;
			// 
			// lblBannerCaption
			// 
			this.lblBannerCaption.AutoSize = true;
			this.lblBannerCaption.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(44)), ((System.Byte)(110)), ((System.Byte)(202)));
			this.lblBannerCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblBannerCaption.ForeColor = System.Drawing.Color.White;
			this.lblBannerCaption.Location = new System.Drawing.Point(0, 0);
			this.lblBannerCaption.Name = "lblBannerCaption";
			this.lblBannerCaption.Size = new System.Drawing.Size(245, 27);
			this.lblBannerCaption.TabIndex = 14;
			this.lblBannerCaption.Text = "������¹����ִ��ϵͳ";
			// 
			// UCBanner
			// 
			this.BackColor = System.Drawing.SystemColors.Highlight;
			this.Controls.Add(this.lblBannerCaption);
			this.Controls.Add(this.pbBanner);
			this.Name = "UCBanner";
			this.Size = new System.Drawing.Size(640, 24);
			this.ResumeLayout(false);

		}
		#endregion

		public string BannerCaption
		{
			get
			{
				return this.lblBannerCaption.Text; 
			}
			set
			{
				this.lblBannerCaption.Text = value;
			}
		}
	}
}
