using System;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Domain.MOModel;

namespace  BenQGuru.eMES.MOModel
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
	public class ItemOfModel : Item
	{
		/// <summary>
		/// ��������[MODELCODE]
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string  ModelCode;
	}

	/// <summary>
	/// ��Ʒ;�̹���Ĺ������
	/// </summary>
	[Serializable]
	public class OPSeqsence : DomainObject
	{
		/// <summary>
		/// MaxSeq	������
		/// </summary>
		[FieldMapAttribute("MAXSEQ", typeof(decimal), 10, true)]
		public decimal  MaxSeq;

		/// <summary>
		/// MinSeq	��С���
		/// </summary>
		[FieldMapAttribute("MINSEQ", typeof(decimal), 10, true)]
		public decimal  MinSeq;
	}
	
}
