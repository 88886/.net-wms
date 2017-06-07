using System;

namespace BenQGuru.eMES.Dashboard
{
	public class MPOCRXmlBuilder
	{
		public MPOCRXmlBuilder()
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
		public void BeginBuildItem(string itemType,string moqty,string totalmoqty,string percent)
		{
			XmlContent.Append(
				"<item itemtype=\"" + itemType 
				+ "\" moqty=\"" + moqty 
				+ "\"  totalmoqty=\"" + totalmoqty 
				+ "\" scale=\"" + percent + "\">");
		}
		//������
		public void BeginBuildDateMo(string date,string qty)
		{
			XmlContent.Append(
				"<datemo date=\"" + date 
				+ "\" moqty=\"" + qty 
				+ "\">");
		}
		//��ϸ����
		public void BuildDateMoDetail(string itemcode,string mocode,string plancompletedate,string actcompletedate,string outputqty)
		{
			XmlContent.Append(
				"<shipdetail " +
				" itemcode=\"" + itemcode +  "\"" +
				" mocode=\"" + mocode +  "\"" +
				" plancompletedate=\"" + plancompletedate +  "\"" +
				" actcompletedate=\"" + actcompletedate +  "\"" +
				" outputqty=\"" + outputqty +  "\"/>");
		}

		public void EndBuildDateMo()
		{
			XmlContent.Append("</datemo>");
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
