using System;

namespace BenQGuru.eMES.Dashboard
{
	public class ModelXmlBuilder
	{
		public ModelXmlBuilder()
		{
			XmlContent.Insert(0,"<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		}

		public System.Text.StringBuilder XmlContent  = new System.Text.StringBuilder();
		//���ݸ��ڵ�
		public void BeginBuildRoot()
		{
			XmlContent.Append("<data>");
		}
		//����������
		public void BeginBuildModel(string label,string data)
		{
			XmlContent.Append(
				"<model label=\"" + label 
				+ "\" data=\"" + data +	"\">");
		}
		//��Ʒ����
		public void BeginBuildItem(string label,string data)
		{
			XmlContent.Append(
				"<item label=\"" + label 
				+ "\" data=\"" + data 
				+ "\"/>");
		}

		public void EndBuildModel()
		{
			XmlContent.Append("</model>");
		}

		public void EndBuildRoot()
		{
			XmlContent.Append("</data>");
		}
	}
}
