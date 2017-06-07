using System;
using BenQGuru.eMES.Common.Domain;

namespace BenQGuru.eMES.DataImportConsole
{
	#region ImpItem
	/// <summary>
	/// �Ϻ�
	/// </summary>
	[Serializable, TableMap("siim", "iprod,idep")]
	public class ImpItem : DomainObject
	{
		public ImpItem()
		{
		}

		[FieldMapAttribute("iprod", typeof(string), 40, true)]
		public string  ItemCode;

		/// <summary>
		/// ��Ʒ����[ItemDesc]
		/// </summary>
		[FieldMapAttribute("idesc", typeof(string), 100, false)]
		public string  ItemDescription;

		/// <summary>
		/// ��Ʒ���[ItemType]
		/// </summary>
		[FieldMapAttribute("ityp", typeof(string), 40, true)]
		public string  ItemType;

		/// <summary>
		/// ��Ʒ����[ItemName]
		/// </summary>
		[FieldMapAttribute("iname", typeof(string), 100, false)]
		public string  ItemName;

		/// <summary>
		/// ������λ[ItemUOM]
		/// </summary>
		[FieldMapAttribute("iumn", typeof(string), 40, true)]
		public string  ItemUOM;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("idep", typeof(string), 40, true)]
		public string  Factory;

		
	}
	#endregion

	#region ImpModel2Item
	/// <summary>
	/// �Ϻ�
	/// </summary>
	[Serializable, TableMap("siim", "imodl,Iprod,idep")]
	public class ImpModel2Item : DomainObject
	{
		public ImpModel2Item()
		{
		}

		/// <summary>
		/// ���ִ���[Model]
		/// </summary>
		[FieldMapAttribute("imodl", typeof(string), 40, true)]
		public string  ModelCode;

		/// <summary>
		/// �Ϻ�[ItemCode]
		/// </summary>
		[FieldMapAttribute("Iprod", typeof(string), 40, true)]
		public string  ItemCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("idep", typeof(string), 40, true)]
		public string  Factory;

	}
	#endregion

	#region ImpSBOM
	/// <summary>
	/// [SBOM]
	/// </summary>
	[Serializable, TableMap("sbom", "bprod,bdep,bfac,bitem,bchld")]
	public class ImpSBOM : DomainObject
	{
		public ImpSBOM()
		{
		}
 
		/// <summary>
		/// ECN
		/// </summary>
		[FieldMapAttribute("becn", typeof(string), 40, false)]
		public string  SBOMItemECN;

		/// <summary>
		/// ���ϴ���
		/// </summary>
		[FieldMapAttribute("bchld", typeof(string), 40, true)]
		public string  SBOMItemCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("bdesc", typeof(string), 40, false)]
		public string  SBOMItemName;

		/// <summary>
		/// ��Ч����
		/// </summary>
		[FieldMapAttribute("bdeff", typeof(int), 8, true)]
		public int  SBOMItemEffectiveDate;

		/// <summary>
		/// ʧЧ����
		/// </summary>
		[FieldMapAttribute("bdiss", typeof(int), 8, true)]
		public int  SBOMItemInvalidDate;

		/// <summary>
		/// λ��
		/// </summary>
		[FieldMapAttribute("bpn", typeof(string), 40, false)]
		public string  SBOMItemLocation;

		/// <summary>
		/// ��Ʒ����
		/// </summary>
		[FieldMapAttribute("bprod", typeof(string), 40, true)]
		public string  ItemCode;


		/// <summary>
		/// ������λ
		/// </summary>
		[FieldMapAttribute("bumn", typeof(string), 40, true)]
		public string  SBOMItemUOM;

		/// <summary>
		/// ��λ����
		/// </summary>
		[FieldMapAttribute("bqreq", typeof(decimal), 15, true)]
		public decimal  SBOMItemQty;

		/// <summary>
		/// ��ѡ��
		/// </summary>
		[FieldMapAttribute("bitem", typeof(string), 40, true)]
		public string  SBOMSourceItemCode;

		/// <summary>
		/// ���
		/// </summary>
		[FieldMapAttribute("bfac", typeof(string), 40, true)]
		public string  SBOMWH;

		/// <summary>
		/// ���ϸ����Ϻ�
		/// </summary>
		[FieldMapAttribute("bpars", typeof(string), 40, true)]
		public string  SBOMParentItemCode;

		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("bdep", typeof(string), 40, true)]
		public string  Factory;

	}
	#endregion

	#region ImpMO
	/// <summary>
	/// ����
	/// </summary>
	[Serializable, TableMap("sfso", "fsord,fdep")]
	public class ImpMO : DomainObject
	{
		public ImpMO()
		{
		}
 
		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("fsord", typeof(string), 40, true)]
		public string  MOCode;
		/// <summary>
		/// ��������
		/// ����ϵͳ�����Ķ���:
		/// 1. ϵͳ��������: MOTYPE
		/// </summary>
		[FieldMapAttribute("fcom", typeof(string), 40, true)]
		public string  MOType;

		/// <summary>
		/// Ԥ�ƿ�������
		/// </summary>
		[FieldMapAttribute("frdte", typeof(int), 8, true)]
		public int  MOPlanStartDate;

		/// <summary>
		/// Ԥ���깤����
		/// </summary>
		[FieldMapAttribute("fddte", typeof(int), 8, true)]
		public int  MOPlanEndDate;


		/// <summary>
		/// Ԥ�Ʋ�����
		/// </summary>
		[FieldMapAttribute("fqreq", typeof(decimal), 10, true)]
		public decimal  MOPlanQty;

		/// <summary>
		/// �ͻ�����
		/// </summary>
		[FieldMapAttribute("fpo", typeof(string), 40, false)]
		public string  CustomerOrderNO;

		/// <summary>
		/// �ͻ�����
		/// </summary>
		[FieldMapAttribute("fcus", typeof(string), 40, false)]
		public string  CustomerCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("fmemo", typeof(string), 100, false)]
		public string  MOMemo;

		/// <summary>
		/// �Ϻ�[ItemCode]
		/// </summary>
		[FieldMapAttribute("fprod", typeof(string), 40, true)]
		public string  ItemCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("fdep", typeof(string), 40, true)]
		public string  Factory;

	}
	#endregion

	#region ImpMOBOM
	/// <summary>
	/// [MOBOM]
	/// </summary>
	[Serializable, TableMap("sfma", "fsord,fdep,fchld")]
	public class ImpMOBOM : DomainObject
	{
		public ImpMOBOM()
		{
		}

		/// <summary>
		/// [CItemCode]
		/// </summary>
		[FieldMapAttribute("fchld", typeof(string), 40, false)]
		public string  MOBOMItemCode;

		/// <summary>
		/// [CItemName]
		/// </summary>
		[FieldMapAttribute("fdesc", typeof(string), 40, false)]
		public string  MOBOMItemName;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("fqreq", typeof(decimal), 15, true)]
		public decimal  MOBOMItemQty;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("fitem", typeof(string), 40, false)]
		public string  MOBOMSourceItemCode;
        
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("fumn", typeof(string), 100, true)]
		public string  MOBOMItemUOM;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("fsord", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("fdep", typeof(string), 40, true)]
		public string  Factory;
	}

	#endregion

	#region ImpERPBOM
	/// <summary>
	/// [MOBOM]
	/// </summary>
	[Serializable, TableMap("lotformes", "serialno")]
	public class ImpERPBOM : DomainObject
	{
		public ImpERPBOM()
		{
		}

		/// <summary>
		/// [CItemCode]
		/// </summary>
		[FieldMapAttribute("itemno", typeof(string), 40, false)]
		public string  BITEMCODE;

		/// <summary>
		/// [QTY]
		/// </summary>
		[FieldMapAttribute("qty", typeof(decimal),15, false)]
		public decimal  BQTY;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("lot", typeof(string), 40, true)]
		public string  LOTNO;

		/// <summary>
		/// [MoCode]
		/// </summary>
		[FieldMapAttribute("so", typeof(string), 40, false)]
		public string  MOCODE;
        
		/// <summary>
		/// [Factory]
		/// </summary>
		[FieldMapAttribute("dep", typeof(string), 40, true)]
		public string  FACTORY;

		/// <summary>
		/// [Sequence]
		/// </summary>
		[FieldMapAttribute("serialno", typeof(decimal), 15, true)]
		public decimal  SEQUENCE;

		#endregion
	}
}
