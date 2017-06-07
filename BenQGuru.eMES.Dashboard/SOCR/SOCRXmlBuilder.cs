using System;

namespace BenQGuru.eMES.Dashboard
{
	public class SOCRXmlBuilder
	{
		public SOCRXmlBuilder()
		{
			XmlContent.Insert(0,"<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		}

		public System.Text.StringBuilder XmlContent  = new System.Text.StringBuilder();
		//���ݸ��ڵ�
		public void BeginBuildRoot()
		{
			XmlContent.Append("<data>");
		}
		//������
		public void BeginBuildItem(string itemType,string orderqty,string totalorderqty,string percent)
		{
			XmlContent.Append(
				"<item itemtype=\"" + itemType 
				+ "\" orderqty=\"" + orderqty 
				+ "\"  ordertotalqty=\"" + totalorderqty 
				+ "\" scale=\"" + percent + "\">");
		}
		//������
		public void BeginBuildDateShip(string date,string qty)
		{
			XmlContent.Append(
				"<dateship date=\"" + date 
				+ "\" qty=\"" + qty 
				+ "\">");
		}
		//��ϸ����
		public void BuildDateShipDetail(string itemcode,string ordernum,string seller,string planshipdate,string actshiptdate,string shipqty)
		{
			XmlContent.Append(
				"<shipdetail " +
				" itemcode=\"" + itemcode +  "\"" +
				" ordernum=\"" + ordernum +  "\"" +
				" seller=\"" + seller +  "\"" +
				" planshipdate=\"" + planshipdate +  "\"" +
				" actshiptdate=\"" + actshiptdate +  "\"" +
				" shipqty=\"" + shipqty +  "\"/>");
		}

		public void EndBuildDateShip()
		{
			XmlContent.Append("</dateship>");
		}

		public void EndBuildItem()
		{
			XmlContent.Append("</item>");
		}

		public void EndBuildRoot()
		{
			XmlContent.Append("</data>");
		}
	}
}
