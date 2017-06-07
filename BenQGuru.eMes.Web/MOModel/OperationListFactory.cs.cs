using System;
using System.Collections;
using System.Web.UI.WebControls;
using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.Web.MOModel
{
	/// <summary>
	/// OperationListFactory ��ժҪ˵����
	/// </summary>
	public class OperationListFactory
	{
        private const int OPControl_Length = 16;
        private const string Default_OpControl = "0000000000000000";

        public OperationListFactory()
        {
        }

        //�÷�������Grid��ʾ���У��������Ҫ��ʾ���У���������������,������SMT��������Ϊ���ӣ����嵼����ҳ��ο�FOperationMP
        public virtual void CreateOperationListColumns(GridHelper gridHelper)
        {
            gridHelper.AddCheckBoxColumn("OP_componentloading", "���Ϲ���", false, null);
            gridHelper.AddCheckBoxColumn("OP_testing", "���Թ���", false, null);
            gridHelper.AddCheckBoxColumn("OP_idtranslate", "���ת������", false, null);
            gridHelper.AddCheckBoxColumn("OP_packing", "��װ����", false, null);
            gridHelper.AddCheckBoxColumn("OP_oqc", "OQC����", false, null);
            gridHelper.AddCheckBoxColumn("OP_ts", "ά�޹���", false, null);
            gridHelper.AddCheckBoxColumn("OP_outside_route", "���⹤��", false, null);
            gridHelper.AddCheckBoxColumn("OP_smt", "SMT��������", false, null);
            gridHelper.AddCheckBoxColumn("OP_spc", "SPC����", false, null);
            gridHelper.AddCheckBoxColumn("OP_deduct", "���Ϲ���", false, null);
            gridHelper.AddCheckBoxColumn("OP_midistoutput", "�м��������", false, null);
            gridHelper.AddCheckBoxColumn("OP_midistinput", "�м�Ͷ�빤��", false, null);
            gridHelper.AddCheckBoxColumn("OP_ComponentDown", "���Ϲ���", false, null);
            gridHelper.AddCheckBoxColumn("OP_BurnIn", "Burn In����", false, null);
            gridHelper.AddCheckBoxColumn("OP_BurnOut", "Burn Out����", false, null);
            gridHelper.AddCheckBoxColumn("OP_Offline", "���߹���", false, null);
            gridHelper.Grid.Columns.FromKey("OP_smt").Hidden = true;
        }
        //���ڵ������б���,�������Ҫ��ʾ���У���ע�͵� modeified by klaus 20130409
        public virtual string[] CreateOperationListColumnsHead()
        {
            ArrayList heads = new ArrayList();

            heads.Add("OP_componentloading");
            heads.Add("OP_testing");
            heads.Add("OP_idtranslate");
            heads.Add("OP_packing");
            heads.Add("OP_oqc");
            heads.Add("OP_ts");
            heads.Add("OP_outside_route");
            //heads.Add("OP_smt");
            heads.Add("OP_spc");
            heads.Add("OP_deduct");
            heads.Add("OP_midistoutput");
            heads.Add("OP_midistinput");
            heads.Add("OP_ComponentDown");
            heads.Add("OP_BurnIn");
            heads.Add("OP_BurnOut");
            heads.Add("OP_Offline");

            return (string[])heads.ToArray(typeof(string));
        }

        //����ΪOPControl��λ�ö�Ӧ�ĺ��壬������ʾ��ע�͵�
        public virtual void CreateNewOperationListCheckBoxList(CheckBoxList chkList, CheckBoxList chklistAttribute, ControlLibrary.Web.Language.LanguageComponent language)
        {
            //һ����ʾ������
            chkList.RepeatColumns = 5;

            chkList.Items.Add(new ListItem(language.GetString("OP_componentloading"), "0"));
            chkList.Items.Add(new ListItem(language.GetString("OP_testing"), "1"));
            chkList.Items.Add(new ListItem(language.GetString("OP_idtranslate"), "2"));
            chkList.Items.Add(new ListItem(language.GetString("OP_packing"), "3"));
            chkList.Items.Add(new ListItem(language.GetString("OP_oqc"), "4"));
            chkList.Items.Add(new ListItem(language.GetString("OP_ts"), "5"));

            chkList.Items.Add(new ListItem(language.GetString("OP_BurnIn"), "13"));
            chkList.Items.Add(new ListItem(language.GetString("OP_BurnOut"), "14"));
            //chkList.Items.Add(language.GetString("OP_Offline"));
            //chkList.Items.Add(new ListItem(language.GetString("OP_Offline"), "15"));

            chklistAttribute.RepeatColumns = 5;

            chklistAttribute.Items.Add(new ListItem(language.GetString("OP_outside_route"), "6"));
            //chklistAttribute.Items.Add(new ListItem(language.GetString("OP_smt"), "7"));
            chklistAttribute.Items.Add(new ListItem(language.GetString("OP_spc"), "8"));
            chklistAttribute.Items.Add(new ListItem(language.GetString("OP_deduct"), "9"));
            chklistAttribute.Items.Add(new ListItem(language.GetString("OP_midistoutput"), "10"));
            chklistAttribute.Items.Add(new ListItem(language.GetString("OP_midistinput"), "11"));
            chklistAttribute.Items.Add(new ListItem(language.GetString("OP_ComponentDown"), "12"));

            chklistAttribute.Items.Add(new ListItem(language.GetString("OP_Offline"), "15"));

        }

        //OPControl ת����CheckBox����
        public virtual void CreateOperationListCheckBoxList(CheckBoxList chkList, CheckBoxList chklistAttribute, string opList)
        {
            string op = this._padding(Default_OpControl);

            if (opList != "" && opList != null)
            {
                op = opList;
            }
            bool[] values = this.CreateOperationListBooleanArray(op);
            for (int i = 0; i < values.Length; i++)
            {
                //�ҵ���Ӧ��ѡ�����value��ֵ���й�ѡ
                if (chkList.Items.Contains(chkList.Items.FindByValue(i.ToString())))
                {
                    chkList.Items.FindByValue(i.ToString()).Selected = values[i];
                }
                if (chklistAttribute.Items.Contains(chklistAttribute.Items.FindByValue(i.ToString())))
                {
                    chklistAttribute.Items.FindByValue(i.ToString()).Selected = values[i];
                }
            }


        }
        //ƴ��OPControl
        public virtual bool[] CreateOperationListBooleanArray(string opList)
        {
            bool[] values = new bool[opList.Length];

            for (int i = 0; i < opList.Length; i++)
            {
                values[i] = FormatHelper.StringToBoolean(opList.Substring(i, 1));
            }

            return values;
        }

        public virtual string CreateOperationList(CheckBoxList chkList, CheckBoxList chklistAttribute)
        {
            string opcontrol = "";

            //foreach (System.Web.UI.WebControls.ListItem item in chkList.Items)
            //{
            //    opcontrol = string.Format("{0}{1}", opcontrol, FormatHelper.BooleanToString(item.Selected));
            //}
            //OPControl����Ϊ16λ
            for (int i = 0; i < 16; i++)
            {
                //2��ѡ���б�û����Ĭ�Ϸ�0
                if ((!chkList.Items.Contains(chkList.Items.FindByValue(i.ToString()))) &&
                      (!chklistAttribute.Items.Contains(chklistAttribute.Items.FindByValue(i.ToString()))))
                {
                    opcontrol = string.Format("{0}{1}", opcontrol, "0");
                }
                else
                {
                    //���chkListû�и�ѡ���chklistAttribute�϶��У������е�����ֵ
                    if (chkList.Items.Contains(chkList.Items.FindByValue(i.ToString())))
                    {
                        opcontrol = string.Format("{0}{1}", opcontrol, FormatHelper.BooleanToString
                        (chkList.Items.FindByValue(i.ToString()).Selected));
                    }
                    else
                    {
                        opcontrol = string.Format("{0}{1}", opcontrol, FormatHelper.BooleanToString
                          (chklistAttribute.Items.FindByValue(i.ToString()).Selected));
                    }

                }
            }
            return opcontrol;
        }

        private string _padding(string opControl)
        {
            if (opControl.Length < OperationListFactory.OPControl_Length)
            {
                opControl = opControl.PadRight(OperationListFactory.OPControl_Length - opControl.Length, '0');
            }
            return opControl;
        }
	}
}

