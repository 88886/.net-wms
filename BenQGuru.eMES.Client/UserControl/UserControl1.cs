using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UserControl
{
	public class UserControl1 : UserControl.UCButton
	{
		private System.ComponentModel.IContainer components = null;

		public UserControl1()
		{
			// �õ����� Windows ���������������ġ�
			InitializeComponent();
			//InitializeLayout();

			// TODO: �� InitializeComponent ���ú�����κγ�ʼ��
		}

		private void InitializeLayout()
		{

			if (this.DesignMode)
			{
				if(this.Container.Components.Count>0)
				{
					object obj = this.Container.Components[0];
					if (obj is System.Windows.Forms.Form)
					{
					   System.Windows.Forms.MessageBox.Show(obj.ToString());  
					}

					System.Windows.Forms.MessageBox.Show(obj.ToString()); 
				}
			}

		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region ��������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// UserControl1
			// 
			this.Name = "UserControl1";

		}
		#endregion
		protected override void OnPaint(PaintEventArgs e)
		{

			base.OnPaint (e);
		}
		public bool _autoPanle;
		[Bindable(true),
		Category("���")]
		public bool AutoPanle
		{
			get
			{
				return _autoPanle;
			}
			set
			{
				_autoPanle=value;
				this.Left =200-this.Width;

			}
		}

	}
}

