// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Collections.Specialized;
	using System.ComponentModel;

	internal interface ICommandTarget
	{
		bool Execute(CommandState commandState);
		bool QueryStatus(CommandState commandState);
	}
}