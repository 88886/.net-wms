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
using BenQGuru.eMES.Common;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Domain.BaseSetting;
using BenQGuru.eMES.BaseSetting;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.LayoutControls;

namespace BenQGuru.eMES.Web.BaseSetting
{
	/// <summary>
	/// FOperation2ResourceSP ��ժҪ˵����
	/// </summary>
	public partial class FRoute2OperationSP : BaseMPageNew
	{
		protected System.Web.UI.WebControls.Label lblOperationSelectTitle;
		protected ControlLibrary.Web.Language.LanguageComponent languageComponent1;
		private System.ComponentModel.IContainer components;
		private BaseModelFacade facade = null ;//new BaseModelFacadeFactory().Create();	

		#region Stable

		protected override ControlLibrary.Web.Language.LanguageComponent GetLanguageComponent()
		{
			return this.languageComponent1;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//this.pagerSizeSelector.Readonly = true;
			if(!IsPostBack)
			{
				// ��ʼ��ҳ������
				this.InitPageLanguage(this.languageComponent1, false);

				this.txtRouteCodeQuery.Text = this.GetRequestParam("routecode");
            
			}
		}

		#endregion

		#region Not Stable
		protected override void InitWebGrid()
		{
            base.InitWebGrid();
			this.gridHelper.AddColumn( "OPSequence", "��������",	null);
			this.gridHelper.AddColumn( "OPCode", "�������",	null);
			this.gridHelper.AddColumn( "OPDescription", "��������",	null);

			new OperationListFactory().CreateOperationListColumns( this.gridHelper );

			this.gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this.gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this.gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);

			this.gridWebGrid.Columns.FromKey("MaintainTime").Hidden = true;

			this.gridHelper.AddDefaultColumn( true, true );
      
			//������
			this.gridHelper.ApplyLanguage( this.languageComponent1 );
			
			//this.gridWebGrid.Columns.FromKey("OPSequence").CellStyle.BackColor = Color.FromArgb(255, 252, 240);
            this.gridWebGrid.Behaviors.CreateBehavior<EditingCore>().Behaviors.CreateBehavior<CellEditing>().ColumnSettings["OPSequence"].ReadOnly = false;

            if (!IsPostBack)
            {
                this.gridHelper.RequestData();
            }
		}

		protected override void DeleteDomainObjects(ArrayList domainObjects)
		{
			if( facade==null )
			{
				facade = new BaseModelFacadeFactory(base.DataProvider).Create();
			}
			facade.DeleteRoute2Operation( (Route2Operation[])domainObjects.ToArray(typeof(Route2Operation)));
		}	

		protected override int GetRowCount()
		{
			if( facade==null )
			{
				facade = new BaseModelFacadeFactory(base.DataProvider).Create();
			}
			return this.facade.GetSelectedOperationByRouteCodeCount( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtRouteCodeQuery.Text)), 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtOperationCodeQuery.Text)));
		}


        //protected override Infragistics.WebUI.UltraWebGrid.UltraGridRow GetGridRow(object obj)
        //{
        //    bool[] opList = new OperationListFactory().CreateOperationListBooleanArray( (obj as OperationOfRoute).OPControl );
        //    object[] values = new object[ 8 + opList.Length ];
        //    values[0] = "false";
        //    values[1] = ((OperationOfRoute)obj).OPSequence;
        //    values[2] = ((OperationOfRoute)obj).OPCode.ToString();
        //    values[3] = ((OperationOfRoute)obj).OPDescription.ToString();

        //    for(int i=0;i<opList.Length;i++)
        //    {
        //        values[ 4 + i ] = opList[i];
        //    }

        //    values[4 + opList.Length] = ((Operation)obj).GetDisplayText("MaintainUser");
        //    values[ 4 + opList.Length + 1 ] = FormatHelper.ToDateString(((Operation)obj).MaintainDate);
        //    values[ 4 + opList.Length + 2 ] = FormatHelper.ToTimeString(((Operation)obj).MaintainTime);
        //    values[ 4 + opList.Length + 3 ] = "";

        //    return new Infragistics.WebUI.UltraWebGrid.UltraGridRow( values );
        //}

        protected override DataRow GetGridRow(object obj)
        {

            DataRow row = this.DtSource.NewRow();
            row["OPSequence"] = ((OperationOfRoute)obj).OPSequence;
            row["OPCode"] = ((OperationOfRoute)obj).OPCode.ToString();
            row["OPDescription"] = ((OperationOfRoute)obj).OPDescription.ToString();

            bool[] opList = new OperationListFactory().CreateOperationListBooleanArray((obj as Operation).OPControl);
            string[] strings = new OperationListFactory().CreateOperationListColumnsHead();
            for (int i = 0; i < opList.Length; i++)
            {
                row[strings[i]] = opList[i];
            }

            row["MaintainUser"] = ((Operation)obj).GetDisplayText("MaintainUser");
            row["MaintainDate"] = FormatHelper.ToDateString((obj as Operation).MaintainDate);
            row["MaintainTime"] = FormatHelper.ToTimeString((obj as Operation).MaintainTime);
            return row;

        }

		protected override void buttonHelper_AfterPageStatusChangeHandle( string pageAction )
		{
			if ( pageAction == PageActionType.Update )
			{
				if( facade==null )
				{
					facade = new BaseModelFacadeFactory(base.DataProvider).Create();
				}
				if( this.facade.IsRouteOperationInUsage( 
					FormatHelper.PKCapitalFormat(FormatHelper.CleanString( this.txtRouteCodeQuery.Text )),
					FormatHelper.PKCapitalFormat(FormatHelper.CleanString( this.txtOperationCodeEdit.Text))) )
				{
					this.txtOperationSequenceEdit.ReadOnly = true;
				}
				else
				{
					this.txtOperationSequenceEdit.ReadOnly = false;
				}
			}
		}

		#region Export
		// 2005-04-06
		protected override string[] FormatExportRecord( object obj )
		{
			bool[] opList = new OperationListFactory().CreateOperationListBooleanArray( (obj as OperationOfRoute).OPControl );
            ArrayList arrayList = new ArrayList(opList.Length);
            for (int i = 0; i < opList.Length; i++)
            {
                arrayList.Add(opList[i]);
            }
            //������ʱ����Ҫ���У���RemoveAtȥ����Ӧ�ģ�����Ķ������ݲο�OperationListFactory��
            //7��ӦSMT����
            arrayList.RemoveAt(7);
            string[] values = new string[5 + arrayList.Count];
			values[0] = ((OperationOfRoute)obj).OPSequence.ToString();
			values[1] = ((OperationOfRoute)obj).OPCode.ToString();
			values[2] = ((OperationOfRoute)obj).OPDescription.ToString();

            for (int i = 0; i < arrayList.Count; i++)
			{
                values[3 + i] = FormatHelper.DisplayBoolean(Convert.ToBoolean(arrayList[i].ToString()), this.languageComponent1);
			}

            values[3 + arrayList.Count] = ((OperationOfRoute)obj).GetDisplayText("MaintainUser");
            values[3 + arrayList.Count + 1] = FormatHelper.ToDateString(((OperationOfRoute)obj).MaintainDate);

			return values;
		}


		protected override string[] GetColumnHeaderText()
		{
			string[] opHeads = (new OperationListFactory()).CreateOperationListColumnsHead();

			string[] heads = new string[5 + opHeads.Length];

			heads[0] = "OPSequence";
			heads[1] = "OPCode";
			heads[2] = "OPDescription";

			for(int i=0;i<opHeads.Length;i++)
			{
				heads[ 3 + i ] = opHeads[i];
			}

			heads[ 3 + opHeads.Length ] = "MaintainUser";
			heads[ 3 + opHeads.Length + 1 ] = "MaintainDate";

			return heads;
		}

		#endregion

		protected override object[] LoadDataSource(int inclusive, int exclusive)
		{
			if( facade==null )
			{
				facade = new BaseModelFacadeFactory(base.DataProvider).Create();
			}
			return facade.GetSelectedOperationOfRouteByRouteCode( 
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtRouteCodeQuery.Text)),
				FormatHelper.PKCapitalFormat(FormatHelper.CleanString(this.txtOperationCodeQuery.Text)),inclusive,exclusive);
		}

		protected void cmdSelect_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FRoute2OperationAP.aspx",
				new string[]{"routecode"},
				new string[]{this.txtRouteCodeQuery.Text.Trim()}));
		}

		protected void cmdReturn_ServerClick(object sender, System.EventArgs e)
		{
			this.Response.Redirect(this.MakeRedirectUrl("./FRouteMP.aspx"));
		}

		protected override void SetEditObject(object obj)
		{
			if (obj == null)
			{
				this.txtOperationCodeEdit.Text	= "";
				this.txtOperationSequenceEdit.Text = "";

				if( this.chklstOPControlEdit.Items.Count == 0 )
				{
                    (new OperationListFactory()).CreateNewOperationListCheckBoxList(this.chklstOPControlEdit, this.chklstOPAttributeEdit, this.languageComponent1);
				}
                (new OperationListFactory()).CreateOperationListCheckBoxList(this.chklstOPControlEdit, this.chklstOPAttributeEdit, "");


				return;
			}

			this.txtOperationCodeEdit.Text	= ((Route2Operation)obj).OPCode;
			this.txtOperationSequenceEdit.Text = ((Route2Operation)obj).OPSequence.ToString();

            (new OperationListFactory()).CreateOperationListCheckBoxList(this.chklstOPControlEdit, this.chklstOPAttributeEdit, ((Route2Operation)obj).OPControl);
		}

        //protected override object GetEditObject(Infragistics.WebUI.UltraWebGrid.UltraGridRow row)
        //{
        //    if( facade==null )
        //    {
        //        facade = new BaseModelFacadeFactory(base.DataProvider).Create();
        //    }
        //    object obj = this.facade.GetRoute2Operation( this.txtRouteCodeQuery.Text.Trim(),row.Cells[2].Text.Trim());
			
        //    if (obj != null)
        //    {
        //        return (Route2Operation)obj;
        //    }

        //    return null;
        //}

        protected override object GetEditObject(GridRecord row)
        {
            if (facade == null)
            {
                facade = new BaseModelFacadeFactory(base.DataProvider).Create();
            }
            string strCode = string.Empty;
            object objCode = row.Items.FindItemByKey("OPCode").Value;
            if (objCode != null)
            {
                strCode = objCode.ToString();
            }
            object obj = facade.GetRoute2Operation(this.txtRouteCodeQuery.Text.Trim(),strCode);
            if (obj != null)
            {
                return (Route2Operation)obj;
            }
            return null;

        }

		protected override object GetEditObject()
		{
			if( facade==null )
			{
				facade = new BaseModelFacadeFactory(base.DataProvider).Create();
			}
			Route2Operation relation = this.facade.CreateNewRoute2Operation();
			relation.RouteCode = FormatHelper.CleanString(this.txtRouteCodeQuery.Text.Trim(),40);
			relation.OPCode = FormatHelper.CleanString(this.txtOperationCodeEdit.Text,40);
			relation.OPSequence = System.Int32.Parse( this.txtOperationSequenceEdit.Text );
            relation.OPControl = (new OperationListFactory()).CreateOperationList(this.chklstOPControlEdit, this.chklstOPAttributeEdit);
			relation.MaintainUser = this.GetUserCode();

			return relation;
		}

		protected override bool ValidateInput()
		{
			PageCheckManager manager = new PageCheckManager();

			manager.Add( new NumberCheck(this.lblOperationSequenceEdit, this.txtOperationSequenceEdit,true) );			

			if ( !manager.Check() )
			{
				WebInfoPublish.Publish(this, manager.CheckMessage, this.languageComponent1);
				return false;
			}

			return true;
		}

		protected override void UpdateDomainObject(object domainObject)
		{
			if( facade==null )
			{
				facade = new BaseModelFacadeFactory(base.DataProvider).Create();
			}
			this.facade.UpdateRoute2Operation((Route2Operation)domainObject);
		}
		#endregion

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

		protected void chklstOPControlEdit_Load(object sender, System.EventArgs e)
		{
			if( !this.IsPostBack )
			{
				if( this.chklstOPControlEdit.Items.Count == 0 )
				{
                    new OperationListFactory().CreateNewOperationListCheckBoxList(this.chklstOPControlEdit, this.chklstOPAttributeEdit, this.languageComponent1);
				}
			}
		}

		protected void cmdSaveTotal_ServerClick(object sender, EventArgs e)
        {
            #region Check seq before batch save
            string seqList = ",";
            string seq = string.Empty;
            for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
            {
                seq = int.Parse(this.gridWebGrid.Rows[i].Items.FindItemByKey("OPSequence").Value.ToString()).ToString();

                if (seqList.IndexOf("," + seq + ",") >= 0)
                {
                    ExceptionManager.Raise(this.GetType(), "$Error_Route2Operation_Sequence_Cannot_Repeat");
                    break;
                }
                else
                {
                    seqList += seq + ",";
                }
            } 
            #endregion

            if ( facade==null )
			{
				facade = new BaseModelFacadeFactory(base.DataProvider).Create();
			}
			for (int i = 0; i < this.gridWebGrid.Rows.Count; i++)
			{
                Route2Operation obj = (Route2Operation)this.facade.GetRoute2Operation(this.txtRouteCodeQuery.Text.Trim(), gridWebGrid.Rows[i].Items.FindItemByKey("OPCode").Value.ToString());
				if (obj != null)
				{
                    obj.OPSequence = int.Parse(this.gridWebGrid.Rows[i].Items.FindItemByKey("OPSequence").Value.ToString());
					facade.UpdateRoute2Operation(obj, false);
				}
			}
			this.gridHelper.RequestData();
		}
	}
}
