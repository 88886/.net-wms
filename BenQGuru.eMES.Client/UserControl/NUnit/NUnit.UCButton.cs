using System;
using NUnit.Framework;
using UserControl;

namespace nunit
{
	/// <summary>
	/// NUnit ��ժҪ˵����
	/// </summary>
    [TestFixture] 
	public class NUnit_UCButton
	{
		public NUnit_UCButton()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		private UCButton uCButton;
		[SetUp]
		public void SetUp()
		{
			 uCButton = new UCButton();
		}

		[Test]
		public void UCButton_Test()
		{
			uCButton.ButtonType = ButtonTypes.None;
			uCButton.Caption = "����";
			Assert.AreEqual(uCButton.Caption, "����");
			uCButton.ButtonType = ButtonTypes.Add;
			Assert.AreEqual(uCButton.Caption, "���");
			uCButton.ButtonType = ButtonTypes.Cancle;
			Assert.AreEqual(uCButton.Caption, "ȡ��");
			uCButton.ButtonType = ButtonTypes.Confirm;
			Assert.AreEqual(uCButton.Caption, "ȷ��");
			uCButton.ButtonType = ButtonTypes.Delete;
			Assert.AreEqual(uCButton.Caption, "ɾ��");
			uCButton.ButtonType = ButtonTypes.Edit;
			Assert.AreEqual(uCButton.Caption, "�༭");
			uCButton.ButtonType = ButtonTypes.Exit;
			Assert.AreEqual(uCButton.Caption, "�˳�");
			uCButton.ButtonType = ButtonTypes.Query;
			Assert.AreEqual(uCButton.Caption, "��ѯ");
			uCButton.ButtonType = ButtonTypes.Refresh;
			Assert.AreEqual(uCButton.Caption, "ˢ��");
			uCButton.ButtonType = ButtonTypes.Save;
			Assert.AreEqual(uCButton.Caption, "����");
			uCButton.ButtonType = ButtonTypes.Copy;
			Assert.AreEqual(uCButton.Caption, "����");
			uCButton.ButtonType = ButtonTypes.AllLeft;
			Assert.AreEqual(uCButton.Caption, "<<");
			uCButton.ButtonType = ButtonTypes.AllRight;
			Assert.AreEqual(uCButton.Caption, ">>");
			uCButton.ButtonType = ButtonTypes.Change;
			Assert.AreEqual(uCButton.Caption, "����");
			uCButton.ButtonType = ButtonTypes.Move;
			Assert.AreEqual(uCButton.Caption, "�Ƴ�");
		}
	}
}
