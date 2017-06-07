using System;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.Rework;

namespace BenQGuru.eMES.SelectQuery
{
	/// <summary>
	/// ItemOfModel ��ժҪ˵����
	/// �ļ���:		QueryDomainObject.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Simone Xu
	/// ��������:	2005/06/20
	/// �޸���:
	/// �޸�����:
	/// �� ��:		�̳���Item������Model��ModelCode��Ϣ
	/// �� ��:	
	/// </summary>
	[Serializable]
	public class MO2Item : MO
	{
		/// <summary>
		/// ��Ʒ����[ItemName]
		/// </summary>
		[FieldMapAttribute("ITEMNAME", typeof(string), 100, false)]
		public string  ItemName;
	}

	//������ѡ�����
	[Serializable]
	public class SelectReworkSheet : ReworkSheet
	{
		/// <summary>
		/// ��Ʒ����[ItemName]
		/// </summary>
		[FieldMapAttribute("ITEMNAME", typeof(string), 100, false)]
		public string  ItemName;
	}

	[Serializable]
	public class ErrorGroup2CodeSelect : DomainObject
	{
		/// <summary>
		/// �������������
		/// </summary>
		[FieldMapAttribute("ECGCODE", typeof(string), 40, true)]
		public string  ErrorCodeGroup;

		/// <summary>
		/// ��������������
		/// </summary>
		[FieldMapAttribute("ECGDESC", typeof(string), 100, false)]
		public string  ErrorCodeGroupDescription;

		/// <summary>
		/// �����������
		/// </summary>
		[FieldMapAttribute("ECODE", typeof(string), 40, true)]
		public string  ErrorCode;

		/// <summary>
		/// ������������
		/// </summary>
		[FieldMapAttribute("ECDESC", typeof(string), 100, false)]
		public string  ErrorDescription;
	}
}
