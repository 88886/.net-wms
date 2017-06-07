using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ControlLibrary.Web.Language
{
	/// <summary>
	/// LanguageComponentDesigner ��ժҪ˵����
	/// </summary>
	public class LanguageComponentDesigner : System.ComponentModel.Design.ComponentDesigner
	{
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;
		private LanguageComponent processor;

		public LanguageComponentDesigner()
		{
			this.processor = null;
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region �����������ɵĴ���

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.processor = (LanguageComponent)component;
		}

		#endregion

		public void OnEditLanguage(object sender, EventArgs e)
		{
			LanguageEditor editor1 = new LanguageEditor();
			editor1.ShowDialog();
		}

		public void OnLoadLanguage(object sender, EventArgs e)
		{
			try
			{
				this.processor.LoadLanguage();
				MessageBox.Show(string.Format("Language profile [{0}] load successfully.", this.processor.LanguageProfileName));
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		public void OnSaveLanguage(object sender, EventArgs e)
		{
			try
			{
				this.processor.SaveLanguage();
				MessageBox.Show(string.Format("Language profile [{0}] saved successfully.", this.processor.LanguageProfileName));
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				DesignerVerbCollection collection1 = new DesignerVerbCollection();
				collection1.Add(new DesignerVerb("&Load Language", new EventHandler(this.OnLoadLanguage)));
				collection1.Add(new DesignerVerb("&Save Language", new EventHandler(this.OnSaveLanguage)));
				collection1.Add(new DesignerVerb("&Edit Language", new EventHandler(this.OnEditLanguage)));
				return collection1;
			}
		}
	}
}
