using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace UserControl
{
	/// <summary>
	/// UCDatetTime ��ժҪ˵����
	/// </summary>
	public class UCDatetTime : System.Windows.Forms.UserControl,IUIAlign
	{
		private System.Windows.Forms.Label labCaption;
		private System.Windows.Forms.DateTimePicker datDate;
		private System.Windows.Forms.DateTimePicker datTime;
		/// <summary> 
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UCDatetTime()
		{
			// �õ����� Windows.Forms ���������������ġ�
			InitializeComponent();

			// TODO: �� InitializeComponent ���ú�����κγ�ʼ��

			labCaption.Top=5;
			datDate.Top=0;
			datTime.Top=0;
			this.Height = datDate.Height;
			
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
			this.datDate = new System.Windows.Forms.DateTimePicker();
			this.datTime = new System.Windows.Forms.DateTimePicker();
			this.labCaption = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// datDate
			// 
			this.datDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.datDate.Location = new System.Drawing.Point(55, 6);
			this.datDate.Name = "datDate";
			this.datDate.Size = new System.Drawing.Size(88, 21);
			this.datDate.TabIndex = 0;
			// 
			// datTime
			// 
			this.datTime.AllowDrop = true;
			this.datTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.datTime.Location = new System.Drawing.Point(143, 6);
			this.datTime.Name = "datTime";
			this.datTime.ShowUpDown = true;
			this.datTime.Size = new System.Drawing.Size(72, 21);
			this.datTime.TabIndex = 1;
			// 
			// labCaption
			// 
			this.labCaption.AutoSize = true;
			this.labCaption.Location = new System.Drawing.Point(7, 10);
			this.labCaption.Name = "labCaption";
			this.labCaption.Size = new System.Drawing.Size(42, 17);
			this.labCaption.TabIndex = 2;
			this.labCaption.Text = "label1";
			this.labCaption.SizeChanged += new System.EventHandler(this.labCaption_SizeChanged);
			// 
			// UCDatetTime
			// 
			this.Controls.Add(this.labCaption);
			this.Controls.Add(this.datTime);
			this.Controls.Add(this.datDate);
			this.Name = "UCDatetTime";
			this.Size = new System.Drawing.Size(224, 32);
			this.ResumeLayout(false);

		}
		#endregion

		[Bindable(true),
		Category("���")]
		public string Caption
		{
			get
			{
				return labCaption.Text;
			}
			set
			{
				labCaption.Text =value;
			}
		}
		private DateTimeTypes _showType=DateTimeTypes.DateTime;
		[Bindable(true),
		Category("���")]
		public DateTimeTypes ShowType
		{
			get
			{
				return _showType;
			}
			set
			{
				_showType =value;
				ChangeType();
				
			}
		}
		private void ChangeType()
		{
			switch (_showType)
			{
				case DateTimeTypes.Date :
					this.datDate.Visible = true;
					this.datTime.Visible = false;
					break;

				case DateTimeTypes.Time :
					this.datDate.Visible = false;
					this.datTime.Visible = true;
					//this.datTime.Left = this.datDate.Left;
					break;

				case DateTimeTypes.DateTime :
					this.datDate.Visible = true;
					this.datTime.Visible = true;
					//this.datTime.Left = this.datDate.Left+92;
					break;
			}
			AutoChange();
		}

		private void labCaption_SizeChanged(object sender, System.EventArgs e)
		{
			AutoChange();			
		}

		//protected override void OnPaint(PaintEventArgs e)
		//{
		//	labCaption_SizeChanged(null,null);
		//	base.OnPaint (e);
		//}


		private DateTime _datetime=DateTime.Now;
		[Bindable(true),
		Category("���")]
		public DateTime Value
		{
			get 
			{
				_datetime = new DateTime(datDate.Value.Year,datDate.Value.Month,datDate.Value.Day,datTime.Value.Hour,datTime.Value.Minute,datTime.Value.Second);
				return 	_datetime;		
			}
			set
			{   
				_datetime = value;
				datDate.Value = _datetime;
				datTime.Value = _datetime;
			}
		}

		private int _xAlign=-1;		
		[Bindable(true),
		Category("���")]
		public int XAlign
		{
			get 
			{  
				_xAlign = this.Left+datDate.Left;
				return 	_xAlign;		
			}
			set
			{   
				_xAlign = value;
				this.Left = value-datDate.Left;	
				
			}
		}
		public void AutoChange()
		{
			switch (_showType)
			{
				case DateTimeTypes.Date :
					this.Width =labCaption.Width +datDate.Width + UIStyleBuilder.SepOfLabAndEditBox;	
					datDate.Left=labCaption.Width + UIStyleBuilder.SepOfLabAndEditBox;
					break;

				case DateTimeTypes.Time :
					this.Width =labCaption.Width + datTime.Width + UIStyleBuilder.SepOfLabAndEditBox;	
					datTime.Left=labCaption.Width + UIStyleBuilder.SepOfLabAndEditBox;
					break;

				case DateTimeTypes.DateTime :
					this.Width =labCaption.Width +datDate.Width+datTime.Width+ UIStyleBuilder.SepOfLabAndEditBox;	
					datDate.Left=labCaption.Width + UIStyleBuilder.SepOfLabAndEditBox;
					datTime.Left=labCaption.Width + UIStyleBuilder.SepOfLabAndEditBox+datDate.Width;
					break;
			}			
			labCaption.Left =0;	
			if (_xAlign!=-1)
				this.Left = _xAlign-datDate.Left;	
		}
	}
}
