using System;
using NUnit.Framework;
using UserControl;

namespace nunit
{
	/// <summary>
	/// NUnit ��ժҪ˵����
	/// </summary>
	[TestFixture]
	public class NUnit_UCLabelCombox
	{
		public NUnit_UCLabelCombox()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		private UCLabelCombox uCLabelCombox;
		[SetUp]
		public void SetUp()
		{
			uCLabelCombox= new UCLabelCombox();
		}

		[Test]
		public void UCLabelCombox_Test()
		{
			uCLabelCombox.Caption = "����";
			Assert.AreEqual(uCLabelCombox.Caption,"����");

			uCLabelCombox.AddItem("����һ","����һ");
			uCLabelCombox.SelectedIndex=0;

			Assert.AreEqual(uCLabelCombox.SelectedItemText,"����һ");
			Assert.AreEqual(uCLabelCombox.SelectedItemValue.ToString(),"����һ");
			
			uCLabelCombox.SelectedIndex=-1;
			uCLabelCombox.SetSelectItem("����һ");
			Assert.AreEqual(uCLabelCombox.SelectedIndex,0);
			Assert.AreEqual(uCLabelCombox.SelectedItemText,"����һ");
			Assert.AreEqual(uCLabelCombox.SelectedItemValue.ToString(),"����һ");
		}
	}
}
