using System;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Domain.Material;


namespace BenQGuru.eMES.WebQuery
{
	/// <summary>
	/// QStockDetail ��ժҪ˵����
	/// </summary>

	public class QStockInDetail : MaterialStockIn
	{
		public QStockInDetail() : base()
		{
		}

		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("QTY", typeof(int), 10, true)]
		public int Quantity;
	}

	public class QStockOutDetail : MaterialStockOut
	{
		public QStockOutDetail() : base()
		{
		}

		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, true)]
		public string RunningCard;
	}

	public class QStockContrast : DomainObject
	{
		public QStockContrast() : base()
		{
		}

		/// <summary>
		/// ��Ʒ���к�
		/// </summary>
		[FieldMapAttribute("RCARD", typeof(string), 40, true)]
		public string RunningCard;
		
		/// <summary>
		/// ��ⵥ��
		/// </summary>
		[FieldMapAttribute("INTKTNO", typeof(string), 40, true)]
		public string StockInTicketNo;
		
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OUTTKTNO", typeof(string), 40, true)]
		public string StockOutTicketNo;
		
		/// <summary>
		/// �������
		/// </summary>
		[FieldMapAttribute("INDATE", typeof(int), 8, true)]
		public int StockInDate;
		
		/// <summary>
		/// ��������
		/// </summary>
		[FieldMapAttribute("OUTDATE", typeof(int), 8, true)]
		public int StockOutDate;
		
		/// <summary>
		/// ����û�
		/// </summary>
		[FieldMapAttribute("INUSER", typeof(string), 40, true)]
		public string StockInUser;
		
		/// <summary>
		/// �����û�
		/// </summary>
		[FieldMapAttribute("OUTUSER", typeof(string), 40, true)]
		public string StockOutUser;

	}

	

}
