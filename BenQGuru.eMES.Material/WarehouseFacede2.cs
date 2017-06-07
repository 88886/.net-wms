using System;
using System.Collections;
using System.Text;
using System.Runtime.Remoting;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.Warehouse;
using BenQGuru.eMES.Domain.DataCollect;
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Domain.TS;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.Material
{
	/// <summary>
	/// WarehouseFacede2 ��ժҪ˵����
	/// </summary>
	public class WarehouseFacede2:WarehouseFacade
	{
		public WarehouseFacede2(IDomainDataProvider domainDataProvider):base(domainDataProvider)
		{	
		}

		public WarehouseFacede2()
		{	
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}


		#region �̵�����Ʒ

		/// <summary>
		/// �̵�����Ʒ (���ϼ�¼)
		/// </summary>
		/// <param name="warehouseCode"></param>
		private Hashtable CycleSimulation2(string warehouseCode)
		{
			//��ȡҪ�̵�Ŀⷿ��Ӧ�Ĳ�����Դ������Ʒ������Ϣ
			//tblonwipitem ���� ActionType��ʾ���ϣ�0���������ϣ�1��
			//TRANSSTATUS��ʾ�Ƿ���пⷿ���� NO��ʾû�пⷿ������YES��ʽ�����˿ⷿ���� 

			#region oldsql
//			string sql = string.Format(@" SELECT {0}
//										FROM tblonwipitem
//										WHERE 1 = 1
//										AND ACTIONTYPE=0
//										AND TRANSSTATUS = 'YES'
//										AND mitemcode > '0'
//										AND EXISTS (
//												SELECT mocode
//													FROM tblmo
//												WHERE tblmo.mocode = tblonwipitem.mocode
//													AND mostatus IN ('mostatus_pending', 'mostatus_open'))
//										AND EXISTS (
//												SELECT rescode
//													FROM tblres
//												WHERE tblres.rescode = tblonwipitem.rescode
//													AND EXISTS (
//															SELECT sscode
//																FROM tblwh2sscode
//																WHERE tblwh2sscode.sscode = tblres.sscode
//																	AND whcode = '{1}')) ",DomainObjectUtility.GetDomainObjectFieldsString(typeof(BenQGuru.eMES.Domain.DataCollect.OnWIPItem))
//				,warehouseCode);

			#endregion

			string sql = string.Format(
				"select mitemcode, sum(qty) as qty" + 
				"	from tblonwipitem " + 
				"	where   1 = 1 " +
				"		AND ACTIONTYPE = 0 " +
				"		AND TRANSSTATUS = 'YES' " +
				"		AND mitemcode > '0' " +
				"		and mocode in " +
				"		(select mocode " +
				"			from tblsimulation " +
				"			where EXISTS " + 
				"			(SELECT mocode " +
				"					FROM tblmo " +
				"					WHERE tblmo.mocode = tblsimulation.mocode " +
                GlobalVariables.CurrentOrganizations.GetSQLCondition() + 
				"					AND mostatus IN ('mostatus_pending', 'mostatus_open')) " +
				"			AND EXISTS " +
				"			(SELECT rescode " +
				"					FROM tblres " +
				"					WHERE tblres.rescode = tblsimulation.rescode " +
                GlobalVariables.CurrentOrganizations.GetSQLCondition() + 
				"					AND EXISTS " +
				"					(SELECT sscode " +
				"							FROM tblwh2sscode " +
				"							WHERE tblwh2sscode.sscode = tblres.sscode " +
				"							AND whcode = '{0}'))) " +
				"	group by mitemcode",warehouseCode);

			object[] objs = this.DataProvider.CustomQuery(typeof(BenQGuru.eMES.Domain.DataCollect.OnWIPItem), new SQLCondition(sql));

			//��������Ϣ��ͳ�ơ�
			Hashtable _ht = new Hashtable();	//�����Ϻ�����ֵ������������Value
			if(objs !=null && objs.Length > 0)
				foreach(BenQGuru.eMES.Domain.DataCollect.OnWIPItem _onwipitem in objs)
				{
					string htkey = _onwipitem.MItemCode;
					if(_ht.Contains(htkey))
					{
						_ht[htkey] = Convert.ToDecimal(_ht[htkey]) + _onwipitem.Qty;
					}
					else
					{
						_ht.Add(_onwipitem.MItemCode,_onwipitem.Qty);
					}
				}

			//����ͳ�ƽ��
			return _ht;
		}

		#endregion

		/// <summary>
		/// �̵�ʱ��ѯ������� , ���̵�����Ʒ
		/// </summary>
		/// <returns></returns>
		public object[] QueryWarehouseStockInCheck3( string itemCode, string warehouseCode,/* string segmentCode,*/ string factoryCode, int inclusive, int exclusive )
		{
			object[] objs = this.QueryWarehouseStockInCheck( itemCode, warehouseCode,/* segmentCode,*/ factoryCode, inclusive, exclusive );

			//ƥ������Ʒ�̵��������ϼ�¼��
			Hashtable _ht =this.CycleSimulation2(warehouseCode);

			foreach(WarehouseCycleCountDetail dtl in objs)
			{
				if(_ht.Contains(dtl.ItemCode))
				{
					//dtl.Qty ;														//��ɢ����
					dtl.LineQty = Convert.ToDecimal(_ht[dtl.ItemCode]);				//����Ʒ�������
					dtl.Warehouse2LineQty = dtl.Qty + dtl.LineQty;					//������
				}
			}

			return objs;
		}

		
	}
}
