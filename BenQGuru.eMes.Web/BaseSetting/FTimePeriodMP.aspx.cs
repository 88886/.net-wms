using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.Common;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.BaseSetting
{
	/// <summary>
	/// FTimePeriodMP ��ժҪ˵����
	/// </summary>
	public partial class FTimePeriodMP : BaseMPageNew
	{

		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;	
		protected System.Web.UI.WebControls.Label lblTimePeriodTitle;
		protected BenQGuru.eMES.Web.UserControl.eMESTime timeTimePeriodBeginTimeEdit;
		protected BenQGuru.eMES.Web.UserControl.eMESTime timeTimePeriodEndTimeEdit;

		private BenQGuru.eMES.BaseSetting.ShiftModelFacade _facade = null ; //new ShiftModelFacadeFactory().Create();

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
			this.languageComponent1 = new ControlLibrary.Web.Language.LanguageComponent(this.components);
			// 
			// languageComponent1
			// 
			this.languageComponent1.Language = "CHS";
			this.languageComponent1.LanguagePackageDir = "D:\\SQC2.0\\eMES\\Source\\bin";
			this.languageComponent1.RuntimePage = null;
			this.languageComponent1.RuntimeUserControl = null;
			this.languageComponent1.UserControlName = "";

		}
		#endregion

		#region Init
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);
			}
		}

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}
		#endregion

		#region WebGrid
		protected override void InitWebGrid()
		{
            base.InitWebGrid();
			this.gridHelper.AddColumn( "TimePeriodSequence", "ʱ������",	null);
			this.gridHelper.AddColumn( "TimePeriodCode", "ʱ�δ���",	null);
			this.gridHelper.AddColumn( "TimePeriodDescription", "ʱ������",	null);
			this.gridHelper.AddColumn( "TimePeriodType", "ʱ������",	null);
			this.gridHelper.AddColumn( "TimePeriodShiftCode", "�������",	null);
			this.gridHelper.AddColumn( "ShiftTypeCode", "���ƴ���",	null);
			this.gridHelper.AddColumn( "TimePeriodBeginTime", "��ʼʱ��",	null);
			this.gridHelper.AddColumn( "TimePeriodEndTime", "����ʱ��",	null);
			this.gridHelper.AddColumn( "IsOverDate", "�Ƿ������",	null);			
			this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);

			this.gridWebGrid.Columns.FromKey("ShiftTypeCode").Hidden = true;
			//this.gridWebGrid.Columns.FromKey("TimePeriodSequence").Hidden = true;
			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;

			this.gridHelper.AddDefaultColumn( true, true );

			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
		}
		
		protected override DataRow GetGridRow(object obj)
		{
            DataRow row = this.DtSource.NewRow();
            row["TimePeriodSequence"] = ((TimePeriod)obj).TimePeriodSequence.ToString();
            row["TimePeriodCode"] = ((TimePeriod)obj).TimePeriodCode.ToString();
            row["TimePeriodDescription"] = ((TimePeriod)obj).TimePeriodDescription.ToString();
            row["TimePeriodType"] = ((TimePeriod)obj).TimePeriodType.ToString();
            row["TimePeriodShiftCode"] = ((TimePeriod)obj).GetDisplayText("ShiftCode");
            row["ShiftTypeCode"] = ((TimePeriod)obj).ShiftTypeCode.ToString();
            row["TimePeriodBeginTime"] = FormatHelper.ToTimeString(((TimePeriod)obj).TimePeriodBeginTime);
            row["TimePeriodEndTime"] = FormatHelper.ToTimeString(((TimePeriod)obj).TimePeriodEndTime);
            row["IsOverDate"] = FormatHelper.DisplayBoolean(((TimePeriod)obj).IsOverDate, this.languageComponent1);
            row["MaintainUser"] = ((TimePeriod)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString(((TimePeriod)obj).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString(((TimePeriod)obj).MaintainTime);
            return row;

		}

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if(_facade==null)
			{
				_facade = new ShiftModelFacadeFactory(base.DataProvider).Create() ;
			}
			return this._facade.QueryTimePeriod( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtTimePeriodCodeQuery.Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtShiftCodeQuery.Text)),
				inclusive, exclusive );
		}


		protected override int GetRowCount()
		{
			if(_facade==null)
			{
				_facade = new ShiftModelFacadeFactory(base.DataProvider).Create() ;
			}
			return this._facade.QueryTimePeriodCount( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtTimePeriodCodeQuery.Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtShiftCodeQuery.Text)));
		}

		#endregion

		#region Button
		protected override void AddDomainObject(object domainObject)
		{
			if(_facade==null)
			{
				_facade = new ShiftModelFacadeFactory(base.DataProvider).Create() ;
			}
			this._facade.AddTimePeriod( (TimePeriod)domainObject );
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if(_facade==null)
			{
				_facade = new ShiftModelFacadeFactory(base.DataProvider).Create() ;
			}
			this._facade.DeleteTimePeriod( (TimePeriod[])domainObjects.ToArray( typeof(TimePeriod) ) );
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			if(_facade==null)
			{
				_facade = new ShiftModelFacadeFactory(base.DataProvider).Create() ;
			}
			this._facade.UpdateTimePeriod( (TimePeriod)domainObject );
		}

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
			if ( pageAction == PageActionType.Add )
			{
				this.txtTimePeriodCodeEdit.ReadOnly = false;
			}

			if ( pageAction == PageActionType.Update )
			{
				this.txtTimePeriodCodeEdit.ReadOnly = true;
			}
		}

		#endregion

		#region Object <--> Page
		protected override object GetEditObject()
		{
			if(_facade==null)
			{
				_facade = new ShiftModelFacadeFactory(base.DataProvider).Create() ;
			}
			TimePeriod timePeriod = this._facade.CreateNewTimePeriod();

			timePeriod.TimePeriodBeginTime		= FormatHelper.TOTimeInt(this.timeTimePeriodBeginTimeEdit.Text);
			timePeriod.TimePeriodEndTime		= FormatHelper.TOTimeInt(this.timeTimePeriodEndTimeEdit.Text);
			timePeriod.IsOverDate				= FormatHelper.BooleanToString(this.chbIsOverDateEdit.Checked);
			timePeriod.TimePeriodCode			= FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtTimePeriodCodeEdit.Text, 40));
			timePeriod.TimePeriodDescription	= FormatHelper.CleanString(this.txtTimePeriodDescriptionEdit.Text, 100);
			timePeriod.TimePeriodType			= this.drpTimePeriodTypeEdit.SelectedValue;
			timePeriod.ShiftCode				= this.drpShiftCodeEdit.SelectedValue;
			timePeriod.ShiftTypeCode			= FormatHelper.CleanString(this.txtShiftTypeCodeEdit.Text, 40);
			timePeriod.TimePeriodSequence		= System.Int32.Parse( this.txtTimePeriodSequenceEdit.Text );
			timePeriod.MaintainUser				= this.GetUserCode();

			return timePeriod;
		}


		protected override object GetEditObject(GridRecord row)
		{
            if (_facade == null)
            {
                _facade = new ShiftModelFacadeFactory(base.DataProvider).Create();
            }
            string strCode = string.Empty;
            object objCode = row.Items.FindItemByKey("TimePeriodCode").Value;
            if (objCode != null)
            {
                strCode = objCode.ToString();
            }
            object obj = _facade.GetTimePeriod(strCode);
            if (obj != null)
            {
                return (TimePeriod)obj;
            }
            return null;
		}

		protected override void SetEditObject(object obj)
		{
			if (obj == null)
			{
				this.timeTimePeriodBeginTimeEdit.Text	= "";
				this.timeTimePeriodEndTimeEdit.Text	= "";
				this.chbIsOverDateEdit.Checked = false;
				this.txtTimePeriodCodeEdit.Text	= "";
				this.txtTimePeriodDescriptionEdit.Text	= "";
				this.drpTimePeriodTypeEdit.SelectedIndex = 0;
				this.drpShiftCodeEdit.SelectedIndex	= 0;
				this.txtShiftTypeCodeEdit.Text	= "";
				this.txtTimePeriodSequenceEdit.Text = "0";
				this.txtShiftBeginTime.Text = "";
				this.txtShiftEndTime.Text = "";

				return;
			}

			this.timeTimePeriodBeginTimeEdit.Text	= FormatHelper.ToTimeString(((TimePeriod)obj).TimePeriodBeginTime);
			this.timeTimePeriodEndTimeEdit.Text		= FormatHelper.ToTimeString(((TimePeriod)obj).TimePeriodEndTime);
			this.chbIsOverDateEdit.Checked			= FormatHelper.StringToBoolean(((TimePeriod)obj).IsOverDate);
			this.txtTimePeriodCodeEdit.Text			= ((TimePeriod)obj).TimePeriodCode.ToString();
			this.txtTimePeriodDescriptionEdit.Text	= ((TimePeriod)obj).TimePeriodDescription.ToString();
			this.txtTimePeriodSequenceEdit.Text = ((TimePeriod)obj).TimePeriodSequence.ToString();
			
			try
			{
				this.drpTimePeriodTypeEdit.SelectedValue	= ((TimePeriod)obj).TimePeriodType.ToString();
			}
			catch
			{	
				this.drpTimePeriodTypeEdit.SelectedIndex = 0;
			}

			try
			{
				this.drpShiftCodeEdit.SelectedValue	= ((TimePeriod)obj).ShiftCode.ToString();
			}
			catch
			{
				this.drpShiftCodeEdit.SelectedIndex	= 0;
			}

			drpShiftCodeEdit_SelectedIndexChanged(this, null);
		}

		
		protected override bool ValidateInput()
		{			
			PageCheckManager manager = new PageCheckManager();

            //manager.Add(new NumberCheck(this.lblTimePeriodSequenceEdit, this.txtTimePeriodSequenceEdit, 0, 100, true));
			manager.Add( new LengthCheck(lblTimePeriodCodeEdit, txtTimePeriodCodeEdit, 40, true) );				
			manager.Add( new LengthCheck(lblTimePeriodTypeEdit, drpTimePeriodTypeEdit, 40, true) );
			manager.Add( new LengthCheck(lblShiftCEdit, drpShiftCodeEdit, 40, true) );
//			manager.Add( new LengthCheck(lblShiftTypeCodeEdit, txtShiftTypeCodeEdit, 40, true) );
			manager.Add( new LengthCheck(lblTimePeriodDescriptionEdit, txtTimePeriodDescriptionEdit, 100, false) );
            
//			if ( !this.chbIsOverDateEdit.Checked )
//			{
//				manager.Add( new TimeRangeCheck(this.lblTimePeriodBeginTimeEdit, this.timeTimePeriodBeginTimeEdit.Text, this.lblTimePeriodEndTimeEdit, this.timeTimePeriodEndTimeEdit.Text, true) );
//			}
//			else
//			{
//				manager.Add( new TimeRangeCheck(this.lblTimePeriodEndTimeEdit, this.timeTimePeriodEndTimeEdit.Text, this.lblTimePeriodBeginTimeEdit, this.timeTimePeriodBeginTimeEdit.Text, true) );
//			}

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
				return false;
			}

            

			return true;
		}

		#endregion

		#region ���ݳ�ʼ��
		protected void drpShiftCodeEdit_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				DropDownListBuilder builder = new DropDownListBuilder(this.drpShiftCodeEdit);
				if(_facade==null)
				{
					_facade = new ShiftModelFacadeFactory(base.DataProvider).Create() ;
				}
				builder.HandleGetObjectList = new BenQGuru.eMES.Web.Helper.GetObjectListDelegate(this._facade.GetAllShift);

                builder.Build("ShiftDescription", "ShiftCode");
			}	

			this.drpShiftCodeEdit_SelectedIndexChanged(sender,e);
		}

		protected void drpShiftCodeEdit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(_facade==null)
			{
				_facade = new ShiftModelFacadeFactory(base.DataProvider).Create() ;
			}
			object obj = this._facade.GetShift(this.drpShiftCodeEdit.SelectedValue);
			
			if ( obj == null )
			{
				this.txtShiftTypeCodeEdit.Text = "";
				this.txtShiftBeginTime.Text = "";
				this.txtShiftEndTime.Text = "";

				return;
			}

			this.txtShiftTypeCodeEdit.Text = ((Shift)obj).ShiftTypeCode;
//			this.txtShiftBeginTime.Text = FormatHelper.ToTimeString( ((Shift)obj).ShiftBeginTime );
//			this.txtShiftEndTime.Text = FormatHelper.ToTimeString( ((Shift)obj).ShiftEndTime );
		}

		protected void drpTimePeriodTypeEdit_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack)
			{
				new SystemParameterListBuilder("TimePeriodType",base.DataProvider).Build(this.drpTimePeriodTypeEdit);
				
			}
			//����Ĭ��ѡ��
			//SetTimePeriodTypeEditDefault();	
		}
		#endregion

		#region Export
		protected override string[] FormatExportRecord( object obj )
		{
			return new string[]{ 
                                   ((TimePeriod)obj).TimePeriodSequence.ToString(),
								   ((TimePeriod)obj).TimePeriodCode.ToString(),
								   ((TimePeriod)obj).TimePeriodDescription.ToString(),
								   ((TimePeriod)obj).TimePeriodType.ToString(),
								   ((TimePeriod)obj).GetDisplayText("ShiftCode"),
								   ((TimePeriod)obj).ShiftTypeCode.ToString(),
								   FormatHelper.ToTimeString(((TimePeriod)obj).TimePeriodBeginTime),
								   FormatHelper.ToTimeString(((TimePeriod)obj).TimePeriodEndTime),
								   FormatHelper.DisplayBoolean(((TimePeriod)obj).IsOverDate, this.languageComponent1),
								   
								   ((TimePeriod)obj).GetDisplayText("MaintainUser"),
								   FormatHelper.ToDateString(((TimePeriod)obj).MaintainDate) };
		}

		protected override string[] GetColumnHeaderText()
		{
			return new string[] {	
                                    "TimePeriodSequence",
									"TimePeriodCode",
									"TimePeriodDescription",
									"TimePeriodType",
									"TimePeriodShiftCode",
									"ShiftTypeCode",
									"TimePeriodBeginTime",
									"TimePeriodEndTime",
									"IsOverDate",
									
									"MaintainUser",
									"MaintainDate"};
		}
		#endregion
	}
}
