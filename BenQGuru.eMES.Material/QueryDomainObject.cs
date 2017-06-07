using System;
using System.Collections;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.Material;

namespace BenQGuru.eMES.Material
{
	/// <summary>
	/// QueryDomainObject ��ժҪ˵����
	/// </summary>
	/// 
	[Serializable]
	public class MaterialStockOutContent
	{
		public MaterialStockOutContent()
		{
		}

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("TKTNO", typeof(string), 40, true)]
		public string  TicketNO;

		/// <summary>
		/// ����
		/// </summary>
		[FieldMapAttribute("MODELCODE", typeof(string), 40, true)]
		public string  ModelCode;

		/// <summary>
		/// ������
		/// </summary>
		[FieldMapAttribute("DEALER", typeof(string), 40, true)]
		public string  Dealer;

		/// <summary>
		/// ��������
		/// 
		/// </summary>
		[FieldMapAttribute("OUTDATE", typeof(int), 8, true)]
		public int  OutDate;

		/// <summary>
		/// ��ά����
		/// </summary>
		[FieldMapAttribute("PLANATECODE", typeof(string), 40, true)]
		public string PlanateCode;

		/// <summary>
		/// ����ͬ�������,����,������,��������ͳ�Ƶ�����
		/// </summary>
		[FieldMapAttribute("QTY", typeof(int), 10, true)]
		public int Qty;
	}
	
	[Serializable]
	public class StockOutContentComparer : IComparer
	{
		#region IComparer ��Ա

		public int Compare(object x, object y)
		{
			if ( x is MaterialStockOutContent && y is MaterialStockOutContent )
			{
				int result = string.Compare(((MaterialStockOutContent)x).TicketNO, ((MaterialStockOutContent)y).TicketNO, false);
				if (  result != 0  )
				{
					return result;
				}

				result = string.Compare(((MaterialStockOutContent)x).ModelCode, ((MaterialStockOutContent)y).ModelCode, false);
				if (  result != 0  )
				{
					return result;
				}

				result = string.Compare(((MaterialStockOutContent)x).Dealer, ((MaterialStockOutContent)y).Dealer, false);
				if (  result != 0  )
				{
					return result;
				}

				return ((MaterialStockOutContent)x).OutDate - ((MaterialStockOutContent)y).OutDate;
			}

			throw new ArgumentException("object is not a MaterialStockOutContent");
		}

		#endregion

	}


	//�������ϲ�ѯ
	[Serializable]
	public class OnWIPItem2 : DomainObject
	{
		public OnWIPItem2()
		{
		}
 
		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MOCODE", typeof(string), 40, true)]
		public string  MOCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("MITEMCODE", typeof(string), 40, false)]
		public string  MItemCode;

		/// <summary>
		/// 
		/// </summary>
		[FieldMapAttribute("Qty", typeof(decimal), 10, true)]
		public decimal  Qty;

	}
}
