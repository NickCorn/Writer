// ---------------------------------------------------------
// Windows Forms CommandBar Control
// Copyright (C) 2001-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ---------------------------------------------------------
namespace Writer.Forms
{
	using System.ComponentModel;

	[DesignTimeVisible(false), ToolboxItem(false)]
	public class CommandBarSeparator : CommandBarItem
	{
		public CommandBarSeparator() : base("-")
		{
		}
	}
}
