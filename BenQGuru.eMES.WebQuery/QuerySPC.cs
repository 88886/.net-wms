using System;
using System.Data;
using System.Collections;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.WebQuery
{
    /// <summary>
    /// QuerySPC ��ժҪ˵����
    /// </summary>
    public class QuerySPC
    {
        const int FIXED_FIELD_COUNT = 5;
        #region Provider,PersistBroker
        private IDomainDataProvider _domainDataProvider = null;
        private IDomainDataProvider _spcDomainDataProvider = null;
        private BenQGuru.eMES.Common.PersistBroker.OLEDBPersistBroker _spcBroker;

        public QuerySPC(IDomainDataProvider domainDataProvider)
        {
            this._domainDataProvider = domainDataProvider;
        }

        public IDomainDataProvider DataProvider
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

        public IDomainDataProvider SPCDataProvider
        {
            get
            {
                if (_spcDomainDataProvider == null)
                {
                    _spcDomainDataProvider = DomainDataProviderManager.DomainDataProvider(SPCBroker, null);
                }

                return _spcDomainDataProvider;
            }
        }

        public BenQGuru.eMES.Common.PersistBroker.OLEDBPersistBroker SPCBroker
        {
            get
            {
                if (this._spcBroker == null)
                {
                    string connStr = BenQGuru.eMES.Common.Config.ConfigSection.Current.DomainSetting.GetSetting(DBName.SPC.ToString()).ConnectString;
                    if (connStr == null || connStr == String.Empty)
                        ExceptionManager.Raise(typeof(QuerySPC), "$Error_SPC_Connection_String", null, null);

                    string type = BenQGuru.eMES.Common.Config.ConfigSection.Current.DomainSetting.GetSetting(DBName.SPC.ToString()).PersistBrokerType;
                    this._spcBroker = (BenQGuru.eMES.Common.PersistBroker.OLEDBPersistBroker)BenQGuru.eMES.Common.PersistBroker.PersistBrokerManager.PersistBroker(connStr, null, type);
                }
                return this._spcBroker;
            }
        }
        #endregion

        private string GetFieldName(string tableName, int seq)
        {
            string sqlschema = string.Format("select * from {0} where 1=2", tableName);
            DataTable dt = SPCBroker.Query(sqlschema).Tables[0];
            if (FIXED_FIELD_COUNT + seq > dt.Columns.Count)
            {
                BenQGuru.eMES.Common.ExceptionManager.Raise(this.GetType(),
                    "$Error_SPC_Test_Seq_Great",//���������Ŵ���, ���������ݱ��п��õ��ֶ���
                    string.Empty
                    );
            }
            return dt.Columns[FIXED_FIELD_COUNT + seq - 1].ColumnName;
        }
        /// <summary>
        /// ȡ������SQL Server���еĲ�������
        /// </summary>
        public DataTable GetTestData(string tableName, string item, string date, string fromtime, bool ifResource, ArrayList list, int seq)
        {
            //�õ���������ֶ���
            string fieldname = GetFieldName(tableName, seq);

            //���ֻ��һ��Resource,��ѯ������������ϴ���
            string sql = "";
            if (list.Count > 1)
                sql = String.Format("select DataTime,{3},Resource from {0} where Item='{1}' and DataDate={2} and {3} is not null",
                                        tableName, item, FormatHelper.TODateInt(date), fieldname);
            else
                sql = String.Format("select DataTime,{3} from {0} where Item='{1}' and DataDate={2} and {3} is not null",
                                tableName, item, FormatHelper.TODateInt(date), fieldname);

            if (ifResource)//��ѯ�����а���Resouce
            {
                //�ö���,�����ŷָ���Resource�б�
                string[] arr = new string[list.Count];
                Array.Copy(list.ToArray(), arr, arr.Length);
                string res = BenQGuru.eMES.Web.Helper.FormatHelper.ProcessQueryValues(arr);
                sql = sql + string.Format(" and Resource in({0})", res);
            }

            if (fromtime != null && fromtime != string.Empty)
                sql = sql + string.Format(" and DataTime>{0}", int.Parse(fromtime));

            sql = sql + " order by DataTime";

            return this.SPCBroker.Query(sql).Tables[0];
        }

        /// <summary>
        /// �������ܣ�����DataRead
        /// </summary>
        //public IDataReader GetTestDataReader(string tableName,string item,string date,string fromtime,bool ifResource,ArrayList list,int seq)
        //{
        //    //�õ���������ֶ���
        //    string fieldname = GetFieldName(tableName,seq);

        //    //���ֻ��һ��Resource,��ѯ������������ϴ���
        //    string sql = "";
        //    if(list.Count > 1)
        //        sql = String.Format("select DataTime,{3},Resource from {0} where Item='{1}' and DataDate={2} and {3} is not null",
        //            tableName,item,FormatHelper.TODateInt(date),fieldname);
        //    else
        //        sql = String.Format("select DataTime,{3} from {0} where Item='{1}' and DataDate={2} and {3} is not null",
        //            tableName,item,FormatHelper.TODateInt(date),fieldname);

        //    if(ifResource)//��ѯ�����а���Resouce
        //    {
        //        //�ö���,�����ŷָ���Resource�б�
        //        string[] arr  = new string[list.Count];
        //        Array.Copy(list.ToArray(),arr,arr.Length);
        //        string res = BenQGuru.eMES.Web.Helper.FormatHelper.ProcessQueryValues(arr);
        //        sql = sql + string.Format(" and Resource in({0})",res); 
        //    }

        //    if(fromtime != null && fromtime != string.Empty)
        //        sql = sql + string.Format(" and DataTime>{0}",int.Parse(fromtime));

        //    sql = sql + " order by DataTime";

        //    return this.SPCBroker.QueryDataReader(sql);
        //}

        public string GetTestDataCount(string tableName, string item, string date, string fromtime, bool ifResource, ArrayList list, int seq)
        {
            //�õ���������ֶ���
            string fieldname = GetFieldName(tableName, seq);

            //���ֻ��һ��Resource,��ѯ������������ϴ���
            string sql = "";
            if (list.Count > 1)
                sql = String.Format("select count(*) c from {0} where Item='{1}' and DataDate={2} and {3} is not null",
                    tableName, item, FormatHelper.TODateInt(date), fieldname);
            else
                sql = String.Format("select count(*) c from {0} where Item='{1}' and DataDate={2} and {3} is not null",
                    tableName, item, FormatHelper.TODateInt(date), fieldname);

            if (ifResource)//��ѯ�����а���Resouce
            {
                //�ö���,�����ŷָ���Resource�б�
                string[] arr = new string[list.Count];
                Array.Copy(list.ToArray(), arr, arr.Length);
                string res = BenQGuru.eMES.Web.Helper.FormatHelper.ProcessQueryValues(arr);
                sql = sql + string.Format(" and Resource in({0})", res);
            }

            if (fromtime != null && fromtime != string.Empty)
                sql = sql + string.Format(" and DataTime>{0}", int.Parse(fromtime));

            return this.SPCBroker.Query(sql).Tables[0].Rows[0][0].ToString();
        }
        /// <summary>
        /// ȡ��Resource�б�
        /// </summary>
        public void GetResourceList(string tableName, string item, string date, string fromtime, ArrayList list, int seq)
        {
            //�õ���������ֶ���
            string fieldname = GetFieldName(tableName, seq);

            string sql = String.Format("select distinct Resource from {0} where Item='{1}' and DataDate={2} and {3} is not null",
                tableName, item, FormatHelper.TODateInt(date), fieldname);
            if (fromtime != null && fromtime != string.Empty)
                sql = sql + string.Format(" and DataTime>{0}", int.Parse(fromtime));

            DataTable dt = SPCBroker.Query(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr["Resource"].ToString());
            }
        }
    }
}
