using System;
using System.Windows.Forms;  
using BenQGuru.eMES.PDAClient.Command;

namespace BenQGuru.eMES.PDAClient.Menu
{
	/// <summary>
	/// IMenu ��ժҪ˵����
	/// </summary>
	public interface IMenu
	{
		object Key
		{
			get;
			set;
		}

		string Caption
		{
			get;
			set;
		}

		string Hint
		{
			get;
			set;
		}

		int Index
		{
			get;
			set;
		}

		int ImageIndex
		{
			get;
			set;
		}

		Shortcut  Shortcut
		{
			get;
			set;
		}

		IMenu[] SubMenus
		{
			get;
			set;		
		}

		ICommand  Command
		{
			get;
			set;
		}

		object MenuObject
		{
			get;
			set;
		}
	}
}
