// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using Microsoft.Win32;

	internal class FontSizeComboBox : ComboBox
	{
		private static readonly string[] fontSizes = new string[] { "1", "2", "3", "4", "5", "6", "7" };

		public FontSizeComboBox()
		{
			this.Text = "10";
			this.Font = new Font("Tahoma", 8f);

			this.Width = 40;
			this.DropDownWidth = 40;

			this.MaxDropDownItems = 12;

			this.PopulateFontSizeList();
		}

		private void PopulateFontSizeList()
		{
			this.Items.AddRange(fontSizes);
		}
	}
}
