using System;
using BenQGuru.eMES.Domain.LotDataCollect;
using UserControl;
using BenQGuru.eMES.Web.Helper;
using BenQGuru.eMES.Common.Domain;


namespace BenQGuru.eMES.LotDataCollect.Action
{
	/// <summary>
	/// ���ݲɼ�ģ��ӿ�
	/// ֻ�����ݲɼ��ڲ�ģ�����
	/// </summary>
	public interface IAction
	{
		Messages Execute(ActionEventArgs actionEventArgs);
	}

	// Added by Icyer 2005/10/28
	/// <summary>
	/// ��չ�����ݲɼ��ӿ�
	/// ��Execute������һ��������ActionCheckStatus�����Լ�¼Action�Ĳ���
	/// </summary>
	public interface IActionWithStatus : IAction
	{
		Messages Execute(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus);
	}
    public interface IActionWithStatusNew : IAction
    {
        Messages Execute(ActionEventArgs actionEventArgs, ActionCheckStatus actionCheckStatus, object[] OPBOMinno);
    }
	// Added end

}
