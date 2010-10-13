// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	internal class Dialog : Form
	{
		public Dialog()
		{
			this.Text = Resource.GetString("ApplicationName");
			this.Icon = null;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.Font = new Font("Tahoma", 8.25f);
			this.ControlBox = true;
			this.MaximizeBox = this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;      
		}


		public bool Run()
		{
			return (this.ShowDialog() == DialogResult.OK);
		}
	}
}
