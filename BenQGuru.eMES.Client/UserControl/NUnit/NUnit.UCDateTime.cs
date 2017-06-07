using System;
using NUnit.Framework;
using UserControl;

namespace nunit
{
	/// <summary>
	/// NUnit ��ժҪ˵����
	/// </summary>
	[TestFixture] 
	public class NUnit_UCDateTime
	{
		public NUnit_UCDateTime()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		private UCDatetTime uCDatetTime;
		[SetUp]
		public void SetUp()
		{
			 uCDatetTime = new UCDatetTime();
			 uCDatetTime.Caption = "����ʱ��";
		}
		[Test]
		public void UCDatetTime_Test()
		{
			uCDatetTime.ShowType = DateTimeTypes.DateTime;
            int _xAlign = uCDatetTime.XAlign;
			uCDatetTime.ShowType = DateTimeTypes.Date;
			Assert.AreEqual(uCDatetTime.XAlign, _xAlign);
			uCDatetTime.ShowType = DateTimeTypes.Time;
			Assert.AreEqual(uCDatetTime.XAlign, _xAlign);

			Assert.AreEqual(uCDatetTime.Caption, "����ʱ��");
			
			DateTime _datetime=DateTime.Now;
			uCDatetTime.Value= _datetime;
			Assert.AreEqual(uCDatetTime.Value, _datetime);
		}
	}
}
