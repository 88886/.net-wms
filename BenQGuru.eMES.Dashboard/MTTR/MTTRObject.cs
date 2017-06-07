using System;

using BenQGuru.eMES.Common.Domain; 

namespace BenQGuru.eMES.Dashboard
{
	
	public class MTTRQueryObject:DomainObject
	{
		[FieldMapAttribute("modelcode", typeof(string), 40,false)]
		public string  ModelCode;
		[FieldMapAttribute("itemcode", typeof(string), 40,false)]
		public string  ItemCode;
		[FieldMapAttribute("tsrescode", typeof(string), 40,false)]
		public string  ResourceCode;
		[FieldMapAttribute("rcard", typeof(string), 40,false)]
		public string  SN;
		[FieldMapAttribute("confirmdate", typeof(string), 40,false)]
		public string ConfrimDate;
		[FieldMapAttribute("confirmweek", typeof(string), 40,false)]
		public string ConfrimWeek;
		[FieldMapAttribute("confirmmonth", typeof(string), 40,false)]
		public string ConfrimMonth;
		[FieldMapAttribute("tsdate", typeof(string), 40,false)]
		public string  CompleteDate;
		[FieldMapAttribute("tsspantime", typeof(string), 40,false)]
		public string  TTR;
	}
	//��ӦXML��item����
	public struct MTTRObject
	{
		public string FieldCode;

		public double MTTR;

		public double TTR;

		public double TotalMTTR;

		public double TSQty;

		public System.Collections.SortedList DailyMTTRs;
	}
	//��ӦXML��DateShip����
	public struct SubMTTR
	{
		public string Date;

		public string FieldCode;

		public int TSQty;

		public double TTR;

		public double MTTR;

		public System.Collections.ArrayList TTRDetails;
	}
}
