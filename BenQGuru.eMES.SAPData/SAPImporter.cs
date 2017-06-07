#region System
using System;
using System.Text;
using System.Runtime.Remoting;  
using System.Collections;
#endregion

#region project
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain; 
using BenQGuru.eMES.Common.Helper;  
using BenQGuru.eMES.Common.DomainDataProvider;   
using BenQGuru.eMES.Domain.MOModel;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.Rework;
#endregion

namespace BenQGuru.eMES.SAPData
{
	/// <summary>
	/// SAPMaper �������ݵ�MES
	/// </summary>
	public class SAPImporter
	{
		private  IDomainDataProvider _domainDataProvider= null;
		private  FacadeHelper _helper					= null;
		
		
		public SAPImportLoger importLogger; 
		public int pageCountNum = 0;	    //�������ݵĵ�ǰҳ��
		public int SucceedImportNum = 0;	//�ɹ��������ݵ�����

		#region �������

		private Hashtable InitailMOCodeHT = null;					//���г�ʼ״̬�Ĺ�������

		private static Hashtable AllItemCodeHT;						//MES ϵͳ�����еĲ�Ʒ����

		private static Hashtable DeleteSbomItemCodeHT;				//��ɾ����׼bom�Ĳ�Ʒ����

		private static Hashtable ToDBMOCodesHT = new Hashtable();	//��ǰ�����Ѿ�����(���������޸ĵ�)��MOCode

		private static Hashtable AllSAPMOCodesHT = new Hashtable();	//��ǰ��ȡ������sap���µ�MOCode

		#endregion
		
		public SAPImporter(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
			this._helper = new FacadeHelper(DataProvider);
		}

		protected IDomainDataProvider DataProvider
		{
			get
			{
				if (_domainDataProvider == null)
				{
					//TODO �˴�Ӧ������MES�����ݿ�
					_domainDataProvider = DomainDataProviderManager.DomainDataProvider(DBName.MES);
				}
				return _domainDataProvider;
			}	
		}

		#region �����Ѿ�ӳ���MES����

		public bool Import(ArrayList importDatas)
		{
			#region �����ж�
			
			if(importDatas == null)return false;
			if(!(importDatas.Count>0))return false;
			//�����ж�
			
			#endregion

			string messagePage =  string.Format("��{0}ҳ ",pageCountNum.ToString(),DateTime.Now.ToString());
			int addCount = 0;
			int updateCount = 0;
			
			this.DataProvider.BeginTransaction();

			//�����׼bom֮ǰɾ����׼bom
			DeleteSBomByItemCode(importDatas);

			try
			{
				foreach(DomainObject obj in importDatas)
				{
					bool isMO = (obj.GetType() == typeof(MO));		 //�жϵ�ǰ�����Ƿ�MO
					
					
					//����Ʒ�Ƿ����,����������򲻵���
					if(!CheckIfExistItem(obj)) continue;

					if(isMO)
					{
						AllSAPMOCodesHT.Add(((MO)obj).MOCode,(MO)obj);
						DealMOObject(obj);	//�����������
					}

					//����Ƿ����
					if(isMO && this.CheckIfExist(obj))//����ǹ���,��ֻ���³�ʼ״̬�Ĺ���
					{
						//����ǹ���,��ֻ���³�ʼ״̬�Ĺ���
						if(obj.GetType() == typeof(MO))
						{
							if(!CheckIfInitailMO((MO)obj))
							{
								continue;
							}
						}
						
						//�����¼�Ѿ�����,�����
						this._helper.UpdateDomainObject(obj);
						updateCount++;
						if(isMO)
						{
							//��¼�·������ݿ�����Ĺ���
							ToDBMOCodesHT.Add(((MO)obj).MOCode,(MO)obj);
						}
					}
					else	//������MO������
					{
						System.Type entityType = obj.GetType() ;
						//��������
						if(entityType == typeof(MOBOM))
						{
							//����bom
							//if(ToDBMOCodesHT.Contains(((MOBOM)obj).MOCode))	//ֻ���·��������ݲ����Ĺ�����Ӧ�Ĺ���bom ,���߼���ע��.
							//ֻҪ��sap���µĹ���,��mobomһ��Ҫ����,AllSAPMOCodesHTΪ����sap���µĹ���
							if(AllSAPMOCodesHT.Contains(((MOBOM)obj).MOCode))
							{
								try
								{
									addCount ++;
									this._helper.AddDomainObject(obj,false);
								}
								catch
								{
									//����BOM�������ظ�����.�ۼӵ�������
									this.UpdateMOBom(((MOBOM)obj));
								}
								
							}
						}
						else if(entityType == typeof(SBOM))
						{
							try
							{
								this._helper.AddDomainObject(obj,false);
								addCount ++;
							}
							catch
							{
								//��׼BOM�������ظ�����.��log�м�¼�ظ�������.
								string msg = string.Format("��׼bom�����ظ� ��Ʒ����{0}  �ӽ��Ϻ�{1}  ��������{2}  ��Ʒ��Ч����{3}",((SBOM)obj).ItemCode,((SBOM)obj).SBOMItemCode,((SBOM)obj).SBOMItemQty,((SBOM)obj).SBOMItemEffectiveDate);
								if(importLogger!=null){importLogger.Write(msg);}
							}
						}
						else
						{
							try
							{
								this._helper.AddDomainObject(obj);
								addCount ++;
							}
							catch
							{
								//�����������ظ�����.��log�м�¼�ظ�������.
								string msg = string.Format("���������ظ� ����{0} ��Ʒ����{1} ",((MO)obj).MOCode,((MO)obj).ItemCode);
								if(importLogger!=null){importLogger.Write(msg);}
							}
							
							if(isMO)
							{
								//��¼�·������ݿ�����Ĺ���
								ToDBMOCodesHT.Add(((MO)obj).MOCode,(MO)obj);
							}
						}

						//if(importLogger !=null){importLogger.Write(this.GetLog(obj,JobActionResult.ADD));}	//��Ϊ�������ϴ�,������������ʱ��дlog
					}
				}
				//messagePage += "  " + DateTime.Now.ToString();
				this.DataProvider.CommitTransaction();
				
			}
			catch(Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				this.RobackDeleteItemHT(importDatas);
				if(importLogger !=null){importLogger.Write("��������쳣 " +ex.Message);}
				return false;
			}
			SucceedImportNum = addCount + updateCount;
			if(importLogger !=null){importLogger.Write(string.Format("{0}��������{1}�� , �޸�����{2}�� ����������{3}��",messagePage,addCount,updateCount,SucceedImportNum));}
			return true;
		}

		#region ������

		//����¼�Ƿ����
		private bool CheckIfExist(DomainObject obj)
		{
			bool ifExist = false;	//��¼�Ƿ����,Ĭ�ϲ�����
			ICheck[] checkList= new ICheck[]{new ExistenceCheck( obj, this.DataProvider )};
			foreach( ExistenceCheck check in checkList )
			{
				try 
				{
					ifExist = check.CheckWithNoException();
				}
				catch(Exception ex)
				{
					return false;
				}
			}

			return ifExist;
		}
		
		//����Ƿ���ڲ�Ʒ
		private bool CheckIfExistItem(DomainObject obj)
		{
			if(AllItemCodeHT == null){AllItemCodeHT = this.GetAllItemCode();} // ��ȡMESϵͳ�����еĲ�Ʒ

			bool ifExist = false;	//Ĭ�ϲ�����

			if(obj.GetType() == typeof(MO))
			{
				if(AllItemCodeHT.Contains(((MO)obj).ItemCode.Trim())){ ifExist = true; }
			}
			else if(obj.GetType() == typeof(MOBOM))
			{
				if(AllItemCodeHT.Contains(((MOBOM)obj).ItemCode.Trim())){ ifExist = true; } //MOBOM ��ʱû�в�Ʒ����
				ifExist = true;
			}
			else if(obj.GetType() == typeof(SBOM))
			{
				if(AllItemCodeHT.Contains(((SBOM)obj).ItemCode.Trim())){ ifExist = true; }
			}

			return ifExist;
		}

		//����Ƿ��ʼ״̬�Ĺ���
		private bool CheckIfInitailMO(MO _MOobj)
		{
			if(InitailMOCodeHT == null){InitailMOCodeHT = this.GetInitialMOCode();} // ��ȡMESϵͳ�г�ʼ״̬�Ĺ���

			bool ifInitail = false;	//Ĭ�ϲ��ǳ�ʼ
			if(InitailMOCodeHT.Contains(((MO)_MOobj).MOCode.Trim())){ ifInitail = true; }

			return ifInitail;
		}

		#endregion 

		#region �����Ʒbomǰ ��ɾ����Ʒbom

		public bool DeleteSBOM(ArrayList importDatas)
		{
			#region �����ж�
			
			if(importDatas == null)return false;
			if(!(importDatas.Count>0))return false;
			//�����ж�
			
			#endregion
			
			this.DataProvider.BeginTransaction();
			try
			{

				DeleteSBomByItemCode(importDatas);					//ɾ����Ʒ�ı�׼bom
				
				this.DataProvider.CommitTransaction();
				
			}
			catch(Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				return false;
			}
			return true;
		}

		#endregion

		#endregion

		#region ��ȡmesϵͳ�е����в�Ʒ����

		public Hashtable GetAllItemCode()
		{
            string sql = string.Format("select " + DomainObjectUtility.GetDomainObjectFieldsString(typeof(Item)) + " from TBLITEM where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " ");
			object[] allItems =  this.DataProvider.CustomQuery(typeof(Item), new SQLCondition(sql));

			Hashtable allItemsHT = new Hashtable();
			if(allItems != null && allItems.Length>0)
			{
				foreach(Item _item in allItems)
				{
					allItemsHT.Add(_item.ItemCode,_item.ItemCode);
				}
			}

			return allItemsHT;
		}

		#endregion

		#region ��ȡ���г�ʼ״̬�Ĺ���

		public Hashtable GetInitialMOCode()
		{
			string sql = string.Format("select "+DomainObjectUtility.GetDomainObjectFieldsString(typeof(MO))+" from TBLMO where mostatus = '{0}' " + GlobalVariables.CurrentOrganizations.GetSQLCondition(),MOManufactureStatus.MOSTATUS_INITIAL);
			object[] initialMOes =  this.DataProvider.CustomQuery(typeof(MO), new SQLCondition(sql));

			Hashtable initialMOHT = new Hashtable();
			if(initialMOes != null && initialMOes.Length>0)
			{
				foreach(MO _mo in initialMOes)
				{
					initialMOHT.Add(_mo.MOCode,_mo.MOCode);
				}
			}

			return initialMOHT;
		}

		#endregion

		#region	ɾ��������Ӧ��mobom

		//Ԥ�������
		private void DealMOObject(DomainObject obj)
		{
			if(obj.GetType() == typeof(MO))
			{
				//�Թ��� ��ɾ��������Ӧ��MOBOM
				this.DeleteMOBomByMoCode(((MO)obj).MOCode);
			}
		}

		//ɾ��������Ӧ��MOBOM
		private void DeleteMOBomByMoCode(string mocode)
		{
			string sql = string.Format(" delete from tblmobom where mocode =  '{0}' ",mocode);
			this.DataProvider.CustomExecute(new SQLCondition(sql));
		}


		#endregion

		#region ɾ����Ʒ��Ӧ�ı�׼bom

		//ɾ����Ʒ��Ӧ�ı�׼BOM
		private void DeleteSBomByItemCode(string itemCode)
		{
            string sql = string.Format(" delete from tblsbom where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and itemcode = '{0}' ", itemCode);
			this.DataProvider.CustomExecute(new SQLCondition(sql));
		}

		private void DeleteSBomByItemCode(ArrayList importDatas)
		{
			if(importDatas ==null || importDatas.Count==0)return;
			if(importDatas[0].GetType() != typeof(SBOM)) return;
			Hashtable itemCodeHT = new Hashtable();
			foreach(SBOM sbom in importDatas)
			{
				if(!itemCodeHT.Contains(sbom.ItemCode))
				{
					itemCodeHT.Add(sbom.ItemCode,sbom.ItemCode);
				}
			}
			if(DeleteSbomItemCodeHT == null)DeleteSbomItemCodeHT = new Hashtable();

			foreach(DictionaryEntry _entry in itemCodeHT)
			{
				string htKey = _entry.Key.ToString(); //���ϴ�����ΪHashtalbe����
				if(!DeleteSbomItemCodeHT.Contains(htKey))
				{
					this.DeleteSBomByItemCode(htKey);
					DeleteSbomItemCodeHT.Add(htKey,htKey);
				}
			}
			
		}

		//ɾ������,�ع�Hashtable����ɾ����¼
		private void RobackDeleteItemHT(ArrayList importDatas)
		{
			if(importDatas ==null || importDatas.Count==0)return;
			if(importDatas[0].GetType() != typeof(SBOM)) return;
			Hashtable itemCodeHT = new Hashtable();
			foreach(SBOM sbom in importDatas)
			{
				if(!itemCodeHT.Contains(sbom.ItemCode))
				{
					itemCodeHT.Add(sbom.ItemCode,sbom.ItemCode);
				}
			}
			if(DeleteSbomItemCodeHT == null)DeleteSbomItemCodeHT = new Hashtable();
			foreach(DictionaryEntry _entry in itemCodeHT)
			{
				string htKey = _entry.Key.ToString();
				if(DeleteSbomItemCodeHT.Contains(htKey))
				{
					DeleteSbomItemCodeHT.Remove(htKey);
				}
			}
		}

		#endregion

		#region ���±�׼BOM��ѡ��

		/// <summary>
		/// ���±�׼BOM��ѡ��
		/// </summary>
		/// <param name="updateDatas">sap������ѡ��BOM</param>
		/// <returns></returns>
		public bool UpdateSBom(object[] updateDatas)
		{
			#region �����ж�
			
			if(updateDatas == null)return false;
			if(!(updateDatas.Length>0))return false;
			//�����ж�
			
			#endregion

			string messagePage =  string.Format("��{0}ҳ",pageCountNum.ToString());
			
			this.DataProvider.BeginTransaction();
			try
			{
				foreach(DomainObject obj in updateDatas)
				{
					this.UpdateSBOMSourceItemCode(obj);
				}
				this.DataProvider.CommitTransaction();
				
			}
			catch(Exception ex)
			{
				this.DataProvider.RollbackTransaction();
				if(importLogger !=null){importLogger.Write("����ͬ����׼bom��ѡ�ϳ����쳣 " +ex.Message);}
				return false;
			}
			return true;
		}

		/// <summary>
		/// ���±�׼BOM��ѡ��
		/// </summary>
		/// <param name="obj">sap bom����</param>
		private void UpdateSBOMSourceItemCode(DomainObject obj )
		{
			SAPBOM _sapEntity = (obj as SAPBOM);
			//SAPBom���ȼ���λΪ"1" ��ʾ��ѡ��
			if(_sapEntity != null && _sapEntity.ZPRI.Trim() == "1" && _sapEntity.ALPGR != string.Empty)
			{
				this.UpdateSBOMSourceItemCode(_sapEntity.SBOMItemCode,_sapEntity.FItemCode,_sapEntity.SBOMItemCode,_sapEntity.ALPGR);
			}
		}
		

		/// <summary>
		/// ���±�׼BOM��ѡ��
		/// </summary>
		/// <param name="_sbsitemcode">��ѡ��</param>
		/// <param name="_itemcode">��Ʒ����</param>
		/// <param name="_sbitemcode">�ӽ��Ϻ�</param>
		/// <param name="_alpgr">�����Ŀ��</param>
		private void UpdateSBOMSourceItemCode(string _sbsitemcode,string _itemcode,string _sbitemcode ,string _alpgr)
		{
            string sql = string.Format(" update TBLSBOM set SBSITEMCODE = '{0}' where 1=1 " + GlobalVariables.CurrentOrganizations.GetSQLCondition() + " and ITEMCODE = '{1}'  and ALPGR = '{3}' "
										,_sbsitemcode,_itemcode,_sbitemcode,_alpgr);
			this.DataProvider.CustomExecute(new SQLCondition(sql));
		}

		#endregion

		#region �ۼ�mobom ����bom

		private void UpdateMOBom(MOBOM mobomObj)
		{
			try
			{
				string sql = string.Format("update tblmobom set MOBITEMQTY = MOBITEMQTY + {0} where 1=1  and MOCODE = '{1}' and ITEMCODE = '{2}' and MOBITEMCODE = '{3}'",mobomObj.MOBOMItemQty,mobomObj.MOCode,mobomObj.ItemCode,mobomObj.MOBOMItemCode);
				this.DataProvider.CustomExecute(new SQLCondition(sql));
			}
			catch
			{}
		}

		#endregion

		#region

		private string GetLog(DomainObject obj,JobActionResult importResult)
		{
			string dataType = string.Empty;		//��������
			string dataCode = string.Empty;		//����Code
			if(obj.GetType() == typeof(MO))
			{
				dataType = "��������";
				dataCode = ((MO)obj).MOCode;
			}
			else if(obj.GetType() == typeof(MOBOM))
			{

				dataType = "����BOM�ӽ��Ϻ�";
				dataCode = ((MOBOM)obj).MOBOMItemCode;
			}
			else if(obj.GetType() == typeof(SBOM))
			{
				dataType = "BOM�Ϻ��ӽ��Ϻ�";
				dataCode = ((SBOM)obj).SBOMItemCode;
			}

			string returnStr = string.Format("������: {2} {0} Ϊ {1} ������",dataType,dataCode,importResult.ToString());
			return returnStr;
		}
		#endregion

		#region ��ȡ�������Ͳ���ʵ��
		private object[] GetMOType()
		{
			string parameterGroup = "MOTYPE";	//�������Ͳ��� 
			string parameterCode = "";			//��������
			string condition = "";				//sqlCondition

			if ( parameterCode != null && parameterCode.Length != 0)
			{
				condition = string.Format("{0} and PARAMCODE like '{1}%'", condition, parameterCode);
			}

			if ( parameterGroup != null && parameterGroup.Length != 0)
			{
				condition = string.Format("{0} and PARAMGROUPCODE  = '{1}'", condition, parameterGroup);
			}

			return this.DataProvider.CustomQuery(typeof(Parameter), new SQLCondition(string.Format("select {0} from TBLSYSPARAM where 1=1 {1}", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Parameter)), condition)));
		}

		#endregion
	}
}
