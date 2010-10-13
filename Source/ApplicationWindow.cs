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
	using Writer.Forms;

	internal class ApplicationWindow : Form
	{
		private CommandBarManager commandBarManager = new CommandBarManager();
		private CommandBar menuBar;	
		private CommandBar toolBar;
		private Panel view = new Panel();
		private StatusBar statusBar = new StatusBar();
		
		public ApplicationWindow()
		{
			this.Icon = IconResource.Application;
			this.Font = new Font("Tahoma", 8.25f);
			
			this.StartPosition = FormStartPosition.WindowsDefaultLocation;
			
			this.Size = new Size(800, 800);

			this.view.Dock = DockStyle.Fill;
			this.view.TabStop = false;
			this.Controls.Add(this.view);

			this.menuBar = new CommandBar(this.commandBarManager, CommandBarStyle.Menu);	
			this.toolBar = new CommandBar(this.commandBarManager, CommandBarStyle.ToolBar);

			this.commandBarManager.CommandBars.Add(this.menuBar);
			this.commandBarManager.CommandBars.Add(this.toolBar);
			this.Controls.Add(this.commandBarManager);

			this.Controls.Add(statusBar);
		}

		public CommandBar MenuBar
		{
			get { return this.menuBar; }
		}

		public CommandBar ToolBar
		{
			get { return this.toolBar; }
		}

		public StatusBar StatusBar
		{
			get { return this.statusBar; }
		}

		public Control View
		{
			get { return this.view; }
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (this.commandBarManager.PreProcessMessage(ref msg))
			{
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}