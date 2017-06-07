using System;
using Infragistics.WebUI.UltraWebGrid;

using BenQGuru.eMES.Web.Helper;

namespace BenQGuru.eMES.Web.BaseSetting
{
	/// <summary>
	/// GridColumnBuilder ��ժҪ˵����
	/// </summary>
	public abstract class GridColumnBuilder
	{
		protected UltraWebGrid _gridWebGrid = null;
		protected GridHelper _gridHelper = null;

		public GridColumnBuilder(UltraWebGrid grid)
		{
			this._gridWebGrid = grid;

			this._gridHelper = new GridHelper( this._gridWebGrid );
		}

		public abstract void Build();
	}

	public class OperationGridColumnBuilder : GridColumnBuilder
	{
		public OperationGridColumnBuilder(UltraWebGrid grid) : base(grid)
		{
		}

		public override void Build()
		{
			this._gridHelper = new GridHelper(this._gridWebGrid);

			this._gridHelper.AddColumn( "OPCode", "�������",	null);
			this._gridHelper.AddColumn( "OPDescription", "��������",	null);
			this._gridHelper.AddColumn( "OPCollectionType", "�����ռ���ʽ",	null);
			this._gridHelper.AddCheckBoxColumn( "OPControl1", "�Ƿ�;�̼��",	false,	null);
			this._gridHelper.AddCheckBoxColumn( "OPControl2", "�Ƿ����ϼ��",	false,	null);
			this._gridHelper.AddCheckBoxColumn( "OPControl3", "�Ƿ���ҪMNID���",	false,	null);
			this._gridHelper.AddCheckBoxColumn( "OPControl4", "�Ƿ񹤵�����",	false,	null);
			this._gridHelper.AddCheckBoxColumn( "OPControl5", "�Ƿ񹤵�����",	false,	null);
			this._gridHelper.AddCheckBoxColumn( "OPControl6", "�Ƿ��������ϼ�",	false,	null);
			this._gridHelper.AddCheckBoxColumn( "OPControl7", "�Ƿ�SPCͳ��",	false,	null);
			this._gridHelper.AddCheckBoxColumn( "OPControl8", "�Ƿ�����������",	false,	null);
			this._gridHelper.AddCheckBoxColumn( "OPControl9", "�Ƿ����ж�",	false,	null);
			
			this._gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this._gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this._gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);
		}

	}

	public class ResourceGridColumnBuilder : GridColumnBuilder
	{
		public ResourceGridColumnBuilder(UltraWebGrid grid) : base(grid)
		{
		}

		public override void Build()
		{
			this._gridHelper = new GridHelper(this._gridWebGrid);

			this._gridHelper.AddColumn( "ResourceCode", "��Դ����",	null);
			this._gridHelper.AddColumn( "ResourceType", "��Դ���",	null);
			this._gridHelper.AddColumn( "ResourceGroup", "��Դ����",	null);
			this._gridHelper.AddColumn( "ShiftTypeCode", "���ƴ���",	null);
			this._gridHelper.AddColumn( "StepSequenceCode", "�����ߴ���",	null);
			this._gridHelper.AddColumn( "SegmentCode", "���δ���",	null);
			this._gridHelper.AddColumn( "ResourceDescription", "��Դ����",	null);
			this._gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this._gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this._gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);		
		}

	}

	public class UserGridColumnBuilder : GridColumnBuilder
	{
		public UserGridColumnBuilder(UltraWebGrid grid) : base(grid)
		{
		}

		public override void Build()
		{
			this._gridHelper.AddColumn( "UserCode", "�û�����",	null);
			this._gridHelper.AddColumn( "UserName", "�û���",	null);
			this._gridHelper.AddColumn( "UserTelephone", "�绰����",	null);
			this._gridHelper.AddColumn( "UserEmail", "��������",	null);
			this._gridHelper.AddColumn( "UserDepartment", "����",null);
            this._gridHelper.AddColumn("DefaultOrgDesc", "Ĭ����֯", null);
            this._gridHelper.AddColumn("UserStatus","�û�״̬",null);
			this._gridHelper.AddColumn( "MaintainUser", "ά���û�",	null);
			this._gridHelper.AddColumn( "MaintainDate", "ά������",	null);
			this._gridHelper.AddColumn( "MaintainTime", "ά��ʱ��",	null);
			//melo zheng �����2006.12.26 ����ҳ����ת,��ѯ��ǰ�û������û���
			this._gridHelper.AddLinkColumn("UserGroup", "�û���",null);
		}

        public void BuildForSelectPage()
        {
            this._gridHelper.AddColumn("UserCode", "�û�����", null);
            this._gridHelper.AddColumn("UserName", "�û���", null);
            this._gridHelper.AddColumn("UserTelephone", "�绰����", null);
            this._gridHelper.AddColumn("UserEmail", "��������", null);
            this._gridHelper.AddColumn("UserDepartment", "����", null);
            this._gridHelper.AddColumn("DefaultOrgDesc", "Ĭ����֯", null);
            this._gridHelper.AddColumn("MaintainUser", "ά���û�", null);
            this._gridHelper.AddColumn("MaintainDate", "ά������", null);
            this._gridHelper.AddColumn("MaintainTime", "ά��ʱ��", null);
        }
	}

}
