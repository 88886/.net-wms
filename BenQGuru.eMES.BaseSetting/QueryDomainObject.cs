using System;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Domain.MOModel;

namespace BenQGuru.eMES.BaseSetting
{
	/// <summary>
	/// RouteOfRouteAlt ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		sammer kong
	/// ��������:	2005/04/05
	/// �޸���:
	/// �޸�����:
	/// �� ��:		
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class ModuleWithViewValue : Module
	{
		[FieldMapAttribute("VIEWVALUE", typeof(string), 40, true)]
		public string ViewValue;
		
	}

	/// <summary>
	/// RouteOfRouteAlt ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Jane Shu
	/// ��������:	2005/03/18
	/// �޸���:
	/// �޸�����:
	/// �� ��:		�̳���Route������Route��RouteAlt��RouteSequence��Ϣ
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class RouteOfRouteAlt : Route
	{
		/// <summary>
		/// ����;��Ⱥ������
		/// </summary>
		[FieldMapAttribute("ROUTEALTCODE", typeof(string), 40, true)]
		public string  RouteAltCode;	
	
		/// <summary>
		/// ����;��˳��
		/// </summary>
		[FieldMapAttribute("ROUTESEQ", typeof(decimal), 10, true)]
		public decimal  RouteSequence;
	}

	/// <summary>
	/// ResourceOfOperation ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Jane Shu
	/// ��������:	2005/03/18
	/// �޸���:
	/// �޸�����:
	/// �� ��:		�̳���Resource������Resource��OperationCode��ResourceSequence��Ϣ
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class ResourceOfOperation : Resource
	{		
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("RESSEQ", typeof(decimal), 10, true)]
		public decimal  ResourceSequence;

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("OPCODE", typeof(string), 40, true)]
		public string  OPCode;
	}

	/// <summary>
	/// OperationOfRoute ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Jane Shu
	/// ��������:	2005/03/18
	/// �޸���:
	/// �޸�����:
	/// �� ��:		�̳���Operation������Operation��RouteCode��OPSequence��Ϣ
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class OperationOfRoute : Operation
	{
		
		/// <summary>
		/// ����;�̴���
		/// </summary>
		[FieldMapAttribute("ROUTECODE", typeof(string), 40, true)]
		public string  RouteCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("OPSEQ", typeof(decimal), 10, true)]
		public decimal  OPSequence;
	}



	/// <summary>
	/// Right ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Jane Shu
	/// ��������:	2005/03/18
	/// �޸���:
	/// �޸�����:
	/// �� ��:		�̳���Operation������Operation��RouteCode��OPSequence��Ϣ
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class FormRight : DomainObject
	{		
		/// <summary>
		/// ����;�̴���
		/// </summary>
		[FieldMapAttribute("MDLCODE", typeof(string), 40, true)]
		public string  ModuleCode;

		/// <summary>
		///
		/// </summary>
		[FieldMapAttribute("VIEWVALUE", typeof(string), 40, false)]
		public string ViewValue;
	}

	/// <summary>
	/// Right ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Jane Shu
	/// ��������:	2005/03/18
	/// �޸���:
	/// �޸�����:
	/// �� ��:		�̳���Operation������Operation��RouteCode��OPSequence��Ϣ
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class MenuWithUrl : Menu
	{
		/// <summary>
		/// ҳ��Url
		/// </summary>
		[FieldMapAttribute("FORMURL", typeof(string), 100, false)]
		public string  FormUrl;
	}
}
