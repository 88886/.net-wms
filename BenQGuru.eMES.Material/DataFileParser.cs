using System;
using System.IO;
using System.Collections;
using System.Runtime.Remoting;
using System.Xml ;
using System.Text.RegularExpressions ;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.Helper;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.Material
{    

	/// <summary>
	/// ������ⲿ�ӿ��ļ�����
	/// Laws Lu
	/// 2005/09/14
	/// �̳���DataFileParser
	/// </summary>
	/// 
	[Serializable]
	public class StockFileParser:DataFileParser
	{
		protected int pCodePosition = 4;

		/// <summary>
		/// ����������Ʒ�ļ�
		/// </summary>
		/// <param name="testFileName">�ļ�����</param>
		/// <returns></returns>
		public override object[] Parse(string testFileName)
		{
			this._ary = new ArrayList() ;

			LoadFormat( this._formatName );
			LoadFileName(testFileName);
			LoadHeader(testFileName);

			if(_format.FormatType == FileFormatType.Header)
			{
				// ����ļ���������Data��Ϣ,������������ֵ��Mapping
				AssignData();
			}

			while(LoadRecord(testFileName))
			{
				AssignData();
			}

			return (object[])( this._ary.ToArray( typeof(object) ));
		}
		/// <summary>
		/// ���ö�ά������ֵ�λ��
		/// </summary>
		public int PlanateCodePostion
		{
			set
			{
				pCodePosition = value;
			}
		}
		
		/// <summary>
		/// Laws Lu
		/// 2005/09/14
		/// ��������ֻ��Ͷ�ά����
		/// </summary>
		new protected void AssignData()
		{
			if(!(((Array)_data.Content[0]).Length>1)) return; //����ǿ���,������]

			int len = _format.ObjectMap.ColumnFormats.Length ;

			Type type= Type.GetType(_format.ObjectMap.ObjectTypeName) ;

			object obj = type.Assembly.CreateInstance(_format.ObjectMap.ObjectTypeName.Split(new char[]{','})[0].ToString());

			string objValue = string.Empty;

			for(int i = 0;i< len;i++)
			{
				ColumnFormat col = _format.ObjectMap.ColumnFormats[i] ;
				string attributeName = col.AttributeName ;
				string columnName = col.ColumnName ;

				switch(col.DataSource)
				{
					case DataSource.FileName:
						objValue = GetSubString(_data.GetFileName(col.DataLine,col.DataColumn) , col.DataStringFrom , col.DataStringTo);
						break;

					case DataSource.Header:
						objValue = GetSubString(_data.GetHeader(col.DataLine,col.DataColumn) , col.DataStringFrom , col.DataStringTo);;
						break;
                    
					case DataSource.Content:
						if(i >= pCodePosition && i <= (pCodePosition + 3))
						{
							objValue = objValue + GetSubString(_data.GetContent(col.DataLine,col.DataColumn), col.DataStringFrom , col.DataStringTo) + ",";
						}
						else
						{
							objValue = GetSubString(_data.GetContent(col.DataLine,col.DataColumn), col.DataStringFrom , col.DataStringTo);
						}
						break;

					case DataSource.DefaultValue:
						objValue = col.Default ;
						break;

					default:
						objValue = col.Default ;
						break;

				}

				if(objValue == string.Empty)
				{
					objValue = col.Default ;
				}

				// ����ǿ��ת��

				if( col.DataType == DataType.Int )
				{
					objValue = this.ConvertToInt( objValue ).ToString() ;
				}

				if( col.DataType == DataType.Float )
				{
					objValue = this.ConvertToFloat( objValue ).ToString() ;
				}

				if( col.DataType == DataType.DateTime )
				{
					objValue = this.ConvertToDateTimeFloat( objValue ).ToString("#") ;
				}

				if( col.DataType == DataType.Date )
				{
					objValue = this.ConvertToDateFloat( objValue ).ToString("#") ;
				}

				if( col.DataType == DataType.Time )
				{
					objValue = this.ConvertToTimeFloat( objValue ).ToString("#") ;
				}

				
				if(i >= pCodePosition && i < (pCodePosition + 3))
				{
				}
				else
				{
					if(objValue != String.Empty && objValue.Substring(objValue.Length - 1,1) == ",")
					{
						objValue = objValue.Substring(0,objValue.Length - 1);
						if(objValue != String.Empty && objValue.Substring(objValue.Length - 1,1) == ",")
						{
							objValue = objValue.Substring(0,objValue.Length - 2);
						}
					}

					BenQGuru.eMES.Common.Domain.DomainObjectUtility.SetValue(obj,attributeName,objValue);

					objValue = String.Empty;
				}

			}


			// vizo 2005-04-14
			// �����һ��ί�е��ж�

			bool objectValid = true ;

			if( this.CheckValidHandle != null)
			{
				objectValid = this.CheckValidHandle( obj ) ;
			}

			// ��� object ����Ч��,������
			if( objectValid )
			{
				_ary.Add( obj ) ;

				if(_format.SaveDataDirectly)
				{
					_dataProvider.Insert(obj);
				}
			}
		}

	}
}
