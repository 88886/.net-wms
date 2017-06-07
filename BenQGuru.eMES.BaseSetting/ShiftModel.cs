using System;
using System.Runtime.Remoting;  
using BenQGuru.eMES.Common.Domain;   
using BenQGuru.eMES.Common.DomainDataProvider;   
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Web.Helper;
   
namespace BenQGuru.eMES.BaseSetting
{
	/// <summary>
	/// ShiftModel ��ժҪ˵����
	/// �ļ���:		ShiftModel.cs
	/// Copyright (c) 1999 -2003 ������¹��BenQGuru�������˾�з���
	/// ������:		Jane Shu
	/// ��������:	2005/03/08
	/// �޸���:		Jane Shu
	/// �޸�����:	2005-04-05  
	///					������д��ȥ��upper
	/// �� ��:		���ģ��ά����̨
	/// �� ��:	
	/// </summary>
	public class ShiftModel:MarshalByRefObject
	{
		private IDomainDataProvider _domainDataProvider = null;		
		private FacadeHelper _helper					 = null;

		public ShiftModel()
		{		
			this._helper = new FacadeHelper( DataProvider );
		}

		//Laws Lu,max life time to unlimited
		public override object InitializeLifetimeService()
		{
			return null;
		}

		public ShiftModel(IDomainDataProvider domainDataProvider)
		{
			this._domainDataProvider = domainDataProvider;
			this._helper = new FacadeHelper( DataProvider );
		}

		protected IDomainDataProvider DataProvider
		{
			get
			{
				if (_domainDataProvider == null)
				{
					_domainDataProvider = DomainDataProviderManager.DomainDataProvider(); 
				}
				return _domainDataProvider;
			}	
		}

		#region Shift Type
		public ShiftType CreateNewShiftType()
		{
			return new ShiftType();
		}

		public void AddShiftType(ShiftType shiftType)
		{
			this._helper.AddDomainObject( shiftType );
		}

		public void UpdateShiftType(ShiftType shiftType)
		{
			this._helper.UpdateDomainObject( shiftType );
		}

		public void DeleteShiftType(ShiftType shiftType)
		{
			this._helper.DeleteDomainObject( shiftType, new ICheck[]{ new DeleteAssociateCheck( shiftType, 
																	this.DataProvider,
																	new Type[]{typeof(Shift), typeof(Segment)}) });
		}
		
		public void DeleteShiftType(ShiftType[] shiftTypes)
		{
			this._helper.DeleteDomainObject( shiftTypes, new ICheck[]{ new DeleteAssociateCheck( shiftTypes, 
																		this.DataProvider,
																		new Type[]{typeof(Shift), typeof(Segment)}) });
		}

		public object GetShiftType( string  shiftTypeCode )
		{
			return this.DataProvider.CustomSearch(typeof(ShiftType), new object[]{ shiftTypeCode }); 
		}

		/// <summary>
		/// ** ��������:	��ѯShiftType��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="shiftTypeCode">ShiftTypeCode��ģ����ѯ</param>
		/// <returns> ShiftType���ܼ�¼��</returns>
		public int QueryShiftTypeCount( string shiftTypeCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSHIFTTYPE where 1=1 and SHIFTTYPECODE like '{0}%'" , shiftTypeCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯShiftType
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="shiftTypeCode">ShiftTypeCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> ShiftType����</returns>
		public object[] QueryShiftType( string shiftTypeCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(ShiftType), new PagerCondition(string.Format("select {0} from TBLSHIFTTYPE where 1=1 and SHIFTTYPECODE like '{1}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ShiftType)) , shiftTypeCode),"SHIFTTYPECODE", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�ShiftType
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>ShiftType���ܼ�¼��</returns>
		public object[] GetAllShiftType()
		{
			return this.DataProvider.CustomQuery(typeof(ShiftType), new SQLCondition(string.Format("select {0} from TBLSHIFTTYPE order by SHIFTTYPECODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(ShiftType)))));
		}

		#endregion

		#region Shift
		public Shift CreateNewShift()
		{
			return new Shift();
		}

		public void AddShift(Shift shift)
		{
			this._helper.AddDomainObject( shift );
		}	
	
		public void UpdateShift(Shift shift)
		{
			this._helper.UpdateDomainObject( shift );
		}

		public void DeleteShift(Shift shift)
		{
			this._helper.DeleteDomainObject( shift, new ICheck[]{ new DeleteAssociateCheck( shift, 
																			  this.DataProvider,
																			  new Type[]{typeof(TimePeriod)}) });
		}

		public void DeleteShift(Shift[] shifts)
		{
			this._helper.DeleteDomainObject( shifts, new ICheck[]{ new DeleteAssociateCheck( shifts, 
																	this.DataProvider,
																	new Type[]{typeof(TimePeriod)}) });
		}

		public object GetShift( string  shiftCode )
		{
			return this.DataProvider.CustomSearch(typeof(Shift), new object[]{shiftCode}); 
		}

		/// <summary>
		/// ** ��������:	��ѯShift��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="shiftCode">ShiftCode��ģ����ѯ</param>
		/// <param name="shiftTypeCode">ShiftTypeCode��ģ����ѯ</param> 
		/// <returns> Shift���ܼ�¼��</returns>
		public int QueryShiftCount( string shiftCode, string shiftTypeCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLSHIFT where 1=1 and SHIFTCODE like '{0}%' and SHIFTTYPECODE like '{1}%' " , shiftCode, shiftTypeCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯShift
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="shiftCode">ShiftCode��ģ����ѯ</param>
		/// <param name="shiftTypeCode">ShiftTypeCode��ģ����ѯ</param> 
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> Shift����</returns>
		public object[] QueryShift( string shiftCode, string shiftTypeCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(Shift), new PagerCondition(string.Format("select {0} from TBLSHIFT where 1=1 and SHIFTCODE like '{1}%' and SHIFTTYPECODE like '{2}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shift)) , shiftCode, shiftTypeCode),"SHIFTTYPECODE, SHIFTSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�Shift
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>Shift���ܼ�¼��</returns>
		public object[] GetAllShift()
		{
			return this.DataProvider.CustomQuery(typeof(Shift), new SQLCondition(string.Format("select {0} from TBLSHIFT order by SHIFTCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shift)))));
		}

		/// <summary>
		/// ** ��������:	�������ShiftType��Shift
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="shiftTypeCode">ShiftTypeCode����ȷ��ѯ</param>
		/// <returns> Shift����</returns>
		public object[] GetShiftByShiftTypeCode(string shiftTypeCode)
		{
			return this.DataProvider.CustomQuery(typeof(Shift), 
				new SQLCondition(string.Format("select {0} from TBLSHIFT where SHIFTTYPECODE='{1}' order by SHIFTSEQ",DomainObjectUtility.GetDomainObjectFieldsString(typeof(Shift)), shiftTypeCode)));
		}

		public bool RuleCheckShift(Shift shift, object[] shifts)
		{
			//Check shift ʱ���Ƿ��ظ�, ����
			return true;
		}
		#endregion

		#region Time Period

		public TimePeriod CreateNewTimePeriod()
		{
			return new TimePeriod();
		}

		public void AddTimePeriod(TimePeriod timePeriod)
		{
			this._helper.AddDomainObject( timePeriod );
		}		
		public void UpdateTimePeriod(TimePeriod timePeriod)
		{
			this._helper.UpdateDomainObject( timePeriod );
		}

		public void DeleteTimePeriod(TimePeriod timePeriod)
		{
			this._helper.DeleteDomainObject( timePeriod );
		}

		public void DeleteTimePeriod(TimePeriod[] timePeriods)
		{
			this._helper.DeleteDomainObject( timePeriods );
		}

		/// <summary>
		/// ** ��������:	��ѯTimePeriod��������
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="timePeriodCode">TimePeriodCode��ģ����ѯ</param>
		/// <returns> TimePeriod���ܼ�¼��</returns>
		public int QueryTimePeriodCount( string timePeriodCode)
		{
			return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLTP where 1=1 and TPCODE like '{0}%' " , timePeriodCode)));
		}

		/// <summary>
		/// ** ��������:	��ҳ��ѯTimePeriod
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <param name="timePeriodCode">TimePeriodCode��ģ����ѯ</param>
		/// <param name="inclusive">��ʼ����</param>
		/// <param name="exclusive">��������</param>
		/// <returns> TimePeriod����</returns>
		public object[] QueryTimePeriod( string timePeriodCode, int inclusive, int exclusive )
		{
			return this.DataProvider.CustomQuery(typeof(TimePeriod), new PagerCondition(string.Format("select {0} from TBLTP where 1=1 and TPCODE like '{1}%'", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TimePeriod)) , timePeriodCode),"SHIFTCODE, TPSEQ", inclusive, exclusive));
		}

		/// <summary>
		/// ** ��������:	������е�TimePeriod
		/// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Jane Shu
		/// ** �� ��:		2005-03-22 10:42:12
		/// ** �� ��:
		/// ** �� ��:
		/// </summary>
		/// <returns>TimePeriod���ܼ�¼��</returns>
		public object[] GetAllTimePeriod()
		{
			return this.DataProvider.CustomQuery(typeof(TimePeriod), new SQLCondition(string.Format("select {0} from TBLTP order by TPCODE", DomainObjectUtility.GetDomainObjectFieldsString(typeof(TimePeriod)))));
		}

		public bool RuleCheckTimePeriod(TimePeriod timePeriod, object[] timePeriods)
		{
			//Check timePeriod ʱ���Ƿ��ظ�, ����
			return true;
		}

		/// <summary>
		/// ** ��������:	���Shift�����е�TimePeriod
		/// ** �� ��:		Jane Shu
		/// ** �� ��:		2005-03-08
		/// ** �� ��:
		/// ** �� ��: 
		/// </summary>
		/// <param name="shiftCode">ShiftCode</param>
		/// <returns>TimePeriod����</returns>
		public object[] GetTimePeriodByShiftCode(string shiftCode)
		{
			return this.DataProvider.CustomQuery(typeof(TimePeriod), 
				new SQLCondition(string.Format("select {0} from TBLTP where SHIFTCODE='{1}' order by TPSEQ",DomainObjectUtility.GetDomainObjectFieldsString(typeof(TimePeriod)), shiftCode)));
		}

		public object GetTimePeriod(string timePeriodCode)
		{			
			return this.DataProvider.CustomSearch(typeof(TimePeriod), new object[]{timePeriodCode});
		}

		#endregion

        #region ShiftCrew
        public ShiftCrew CreateNewShiftCrew()
        {
            return new ShiftCrew();
        }

        public void AddShiftCrew(ShiftCrew shiftCrew)
        {
            this._helper.AddDomainObject(shiftCrew);
        }

        public void UpdateShiftCrew(ShiftCrew shiftCrew)
        {
            this._helper.UpdateDomainObject(shiftCrew);
        }

        public void DeleteShiftCrew(ShiftCrew[] shiftCrews)
        {
            this.DataProvider.BeginTransaction();
            try
            {
                foreach (ShiftCrew shiftCrew in shiftCrews)
                {
                    this.DataProvider.Delete(shiftCrew);
                    DeleteShiftCrewOnResource(shiftCrew.CrewCode);
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                throw ex;
            }
        }

        public void DeleteShiftCrewOnResource(string crewCode)
        {
            this.DataProvider.CustomExecute(new SQLCondition(string.Format("update TBLRES set crewcode='' where crewcode='{0}'", crewCode)));
        }

        public object GetShiftCrew(string shiftCrewCode)
        {
            return this.DataProvider.CustomSearch(typeof(ShiftCrew), new object[] { shiftCrewCode });
        }

        /// <summary>
        /// ** ��������:	��ѯShiftCrew��������
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Roger xue
        /// ** �� ��:		2008-10-13
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="shiftCode">ShiftCrewCode��ģ����ѯ</param>
        /// <param name="shiftTypeCode">ShiftCrewDesc��ģ����ѯ</param> 
        /// <returns> ShiftCrew���ܼ�¼��</returns>
        public int QueryShiftCrewCount(string crewCode, string crewDesc)
        {
            string sql = "select count(*) from TBLCREW where 1=1";
            if (crewCode != string.Empty && crewCode != null)
            {
                sql = string.Format("{0} and CREWCODE LIKE '%{1}%'", sql, crewCode);
            }

            if (crewDesc != string.Empty && crewDesc != null)
            {
                sql = string.Format("{0} and CREWDESC LIKE '%{1}%'", sql, crewDesc);
            }
            
            return this.DataProvider.GetCount(new SQLCondition(string.Format(sql)));
        }

        /// <summary>
        /// ** ��������:	��ҳ��ѯShiftCrew
        /// ** �� ��:		ER/Studio Basic Macro Code Generation  Created by Roger xue
        /// ** �� ��:		2008-10-13
        /// ** �� ��:
        /// ** �� ��:
        /// </summary>
        /// <param name="shiftCode">ShiftCrewCode��ģ����ѯ</param>
        /// <param name="shiftTypeCode">ShiftCrewDesc��ģ����ѯ</param> 
        /// <param name="inclusive">��ʼ����</param>
        /// <param name="exclusive">��������</param>
        /// <returns> Shift����</returns>
        public object[] QueryShiftCrew(string crewCode, string crewDesc, int inclusive, int exclusive)
        {
            string sql = "select {0} from TBLCREW where 1=1";
            if (crewCode != string.Empty && crewCode != null)
            {
                sql = string.Format("{0} and CREWCODE LIKE '%{1}%'", sql, crewCode);
            }

            if (crewDesc != string.Empty && crewDesc != null)
            {
                sql = string.Format("{0} and CREWDESC LIKE '%{1}%'", sql, crewDesc);
            }
            return this.DataProvider.CustomQuery(typeof(ShiftCrew), new PagerCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(ShiftCrew))), "CREWCODE", inclusive, exclusive));
        }

        public object[] GetAllShiftCrew()
        {
            string sql = "select {0} from TBLCREW order by crewcode ";

            return this.DataProvider.CustomQuery(typeof(ShiftCrew), new SQLCondition(string.Format(sql, DomainObjectUtility.GetDomainObjectFieldsString(typeof(ShiftCrew)))));
        }

        #endregion

        #region ShiftCrewResource
        public Resource CreateNewResource()
        {
            return new Resource();
        }

        //������Դ�б�
        public void DeleteShiftCrewResource(Resource[] resources)
        {
            this.DataProvider.BeginTransaction();
            try
            {
                foreach (Resource resource in resources)
                {
                    this.DataProvider.CustomExecute(new SQLCondition(string.Format("update TBLRES set crewcode='' where rescode='{0}'", resource.ResourceCode)));
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                throw ex;
            }
        }

        public int QueryShiftCrewResourceCount(string crewCode)
        {
            return this.DataProvider.GetCount(new SQLCondition(string.Format("select count(*) from TBLRES where CREWCODE = '{0}'", crewCode)));
        }

        public object[] QueryShiftCrewResource(string crewCode, int inclusive, int exclusive)
        {
            return this.DataProvider.CustomQuery(typeof(Resource), new PagerCondition(string.Format("select {0} from TBLRES where CREWCODE = '{1}' ", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Resource)), crewCode), "RESCODE", inclusive, exclusive));
        }

        //����������Դ�б�
        public void AddShiftCrewResource(Resource[] resources, string crewCode)
        {
            this.DataProvider.BeginTransaction();
            try
            {
                foreach (Resource resource in resources)
                {
                    this.DataProvider.CustomExecute(new SQLCondition(string.Format("update TBLRES set crewcode='{1}' where rescode='{0}'", resource.ResourceCode, crewCode)));
                }
                this.DataProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.DataProvider.RollbackTransaction();
                throw ex;
            }
        }

        public int QueryShiftCrew2NewResourceCount(string resCode)
        {
            string sql = string.Format("select count(*) from TBLRES where RESCODE like '%{0}%' and (crewcode ='' or crewcode is null)", resCode);

            sql += GlobalVariables.CurrentOrganizations.GetSQLCondition();
            
            return this.DataProvider.GetCount(new SQLCondition(sql));
        }

        public object[] QueryShiftCrew2NewResource(string resCode, int inclusive, int exclusive)
        {
            string sql = string.Format("select {0} from TBLRES where RESCODE like '%{1}%' and (crewcode ='' or crewcode is null)", DomainObjectUtility.GetDomainObjectFieldsString(typeof(Resource)), resCode);

            sql += GlobalVariables.CurrentOrganizations.GetSQLCondition();
            
            return this.DataProvider.CustomQuery(typeof(Resource), new PagerCondition(sql, "RESCODE", inclusive, exclusive));
        }

        #endregion
	}
}
