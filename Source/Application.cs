// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Threading;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using System.Reflection;
	using Writer.Forms;
	using Writer.Html;
	
	internal sealed class Application : ICommandTarget
	{
		private ApplicationWindow applicationWindow;
		private FileHistory fileHistory;
		private FtpConfiguration ftpConfiguration;
		private CommandManager commandManager;
		private ConfigurationManager configurationManager;
		private Document document = null;

		[STAThread()]	
		public static void Main(string[] arguments)
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			new Application().Run();
		}

		public Application()
		{
			this.commandManager = new CommandManager();

			this.configurationManager = new ConfigurationManager();
			this.configurationManager.Load(this.GetType().Module.FullyQualifiedName);

			this.fileHistory = new FileHistory();
			this.fileHistory.Configuration = this.configurationManager["FileHistory"];

			this.ftpConfiguration = new FtpConfiguration();
			this.ftpConfiguration.Configuration = this.configurationManager["FtpConfiguration"];

			this.applicationWindow = new ApplicationWindow();
			this.applicationWindow.Closed += new EventHandler(this.ApplicationWindow_Closed);
			this.applicationWindow.Closing += new CancelEventHandler(this.ApplicationWindow_Closing);
			this.applicationWindow.Activated += new EventHandler(this.ApplicationWindowActivated);

			// Toolbar
			CommandBar toolBar = this.applicationWindow.ToolBar;

			this.commandManager.Add("File.New", toolBar.Items.AddButton(CommandBarImageResource.New, "&New", null, Keys.Control | Keys.N));
			this.commandManager.Add("File.Open", toolBar.Items.AddButton(CommandBarImageResource.Open, "&Open...", null, Keys.Control | Keys.O));
			this.commandManager.Add("File.Save", toolBar.Items.AddButton(CommandBarImageResource.Save, "&Save", null, Keys.Control | Keys.S));
			toolBar.Items.AddSeparator();
			this.commandManager.Add("Edit.Cut", toolBar.Items.AddButton(CommandBarImageResource.Cut, "Cu&t", null, Keys.Control | Keys.X));
			this.commandManager.Add("Edit.Copy", toolBar.Items.AddButton(CommandBarImageResource.Copy, "&Copy", null, Keys.Control | Keys.C));
			this.commandManager.Add("Edit.Paste", toolBar.Items.AddButton(CommandBarImageResource.Paste, "&Paste", null, Keys.Control | Keys.V));
			this.commandManager.Add("Edit.Delete", toolBar.Items.AddButton(CommandBarImageResource.Delete, "&Delete", null, Keys.Delete));
			toolBar.Items.AddSeparator();
			this.commandManager.Add("Edit.Undo", toolBar.Items.AddButton(CommandBarImageResource.Undo, "&Undo", null, Keys.Control | Keys.Z));
			this.commandManager.Add("Edit.Redo", toolBar.Items.AddButton(CommandBarImageResource.Redo, "&Redo", null, Keys.Control | Keys.Y));
			toolBar.Items.AddSeparator();
			this.commandManager.Add("Format.Font", toolBar.Items.AddComboBox("Font", new FontComboBox()));
			this.commandManager.Add("Format.FontSize", toolBar.Items.AddComboBox("Font Size", new FontSizeComboBox()));
			toolBar.Items.AddSeparator();
			this.commandManager.Add("Format.Bold", toolBar.Items.AddCheckBox(FormatResource.Bold, "&Bold", Keys.Control | Keys.B));
			this.commandManager.Add("Format.Italic", toolBar.Items.AddCheckBox(FormatResource.Italic, "&Italic", Keys.Control | Keys.I));
			this.commandManager.Add("Format.Underline", toolBar.Items.AddCheckBox(FormatResource.Underline, "U&nderline", Keys.Control | Keys.U));
			toolBar.Items.AddSeparator();
			this.commandManager.Add("Format.UnorderedList", toolBar.Items.AddCheckBox(FormatResource.UnorderedList, "&Bullets"));
			this.commandManager.Add("Format.OrderedList", toolBar.Items.AddCheckBox(FormatResource.OrderedList, "&Numbering"));
			this.commandManager.Add("Format.Unindent", toolBar.Items.AddButton(FormatResource.Unindent, "Unind&ent", null, Keys.Shift | Keys.Tab));
			this.commandManager.Add("Format.Indent", toolBar.Items.AddButton(FormatResource.Indent, "In&dent", null, Keys.Tab));
			toolBar.Items.AddSeparator();

			CommandBarMenu foreColorMenu = toolBar.Items.AddMenu(FormatResource.ForeColor, "Foreground Color");
			this.commandManager.Add("Format.ForeColor.Black", foreColorMenu.Items.AddButton(FormatResource.Black, "Blac&k", null));
			this.commandManager.Add("Format.ForeColor.Yellow", foreColorMenu.Items.AddButton(FormatResource.Yellow, "&Yellow", null));
			this.commandManager.Add("Format.ForeColor.Red", foreColorMenu.Items.AddButton(FormatResource.Red, "&Red", null));
			this.commandManager.Add("Format.ForeColor.Green", foreColorMenu.Items.AddButton(FormatResource.Green, "&Green", null));
			this.commandManager.Add("Format.ForeColor.Blue", foreColorMenu.Items.AddButton(FormatResource.Blue, "&Blue", null));
			this.commandManager.Add("Format.ForeColor.White", foreColorMenu.Items.AddButton(FormatResource.White, "&White", null));

			CommandBarMenu backColorMenu = toolBar.Items.AddMenu(FormatResource.BackColor, "Background Color");
			this.commandManager.Add("Format.BackColor.Black", backColorMenu.Items.AddButton(FormatResource.Black, "Blac&k", null));
			this.commandManager.Add("Format.BackColor.Yellow", backColorMenu.Items.AddButton(FormatResource.Yellow, "&Yellow", null));
			this.commandManager.Add("Format.BackColor.Red", backColorMenu.Items.AddButton(FormatResource.Red, "&Red", null));
			this.commandManager.Add("Format.BackColor.Green", backColorMenu.Items.AddButton(FormatResource.Green, "&Green", null));
			this.commandManager.Add("Format.BackColor.Blue", backColorMenu.Items.AddButton(FormatResource.Blue, "&Blue", null));
			this.commandManager.Add("Format.BackColor.White", backColorMenu.Items.AddButton(FormatResource.White, "&White", null));

			// Menu
			CommandBar menuBar = this.applicationWindow.MenuBar;
			
			CommandBarMenu fileMenu = menuBar.Items.AddMenu("&File");
			this.commandManager.Add("File.New", fileMenu.Items.AddButton(CommandBarImageResource.New, "&New", null, Keys.Control | Keys.N));
			this.commandManager.Add("File.Open", fileMenu.Items.AddButton(CommandBarImageResource.Open, "&Open...", null, Keys.Control | Keys.O));
			this.commandManager.Add("File.Save", fileMenu.Items.AddButton(CommandBarImageResource.Save, "&Save", null, Keys.Control | Keys.S));
			this.commandManager.Add("File.SaveAs", fileMenu.Items.AddButton("Save &As...", null));
			CommandBarMenu publishToWebMenu = fileMenu.Items.AddMenu("Publish to &Web");
			this.commandManager.Add("File.FtpSetting", publishToWebMenu.Items.AddButton("&Settings...", null));
			this.commandManager.Add("File.Publish", publishToWebMenu.Items.AddButton("&Publish", null));            
			fileMenu.Items.AddSeparator();
			this.commandManager.Add("File.PrintPreview", fileMenu.Items.AddButton(CommandBarImageResource.Preview, "Print Pre&view", null));
			this.commandManager.Add("File.Print", fileMenu.Items.AddButton(CommandBarImageResource.Print, "&Print", null, Keys.Control | Keys.P));
			fileMenu.Items.AddSeparator();
			CommandBarMenu fileHistoryMenu = fileMenu.Items.AddMenu("Recent &Files");
			this.commandManager.Add("File.History.0", fileHistoryMenu.Items.AddButton("&1", null));
			this.commandManager.Add("File.History.1", fileHistoryMenu.Items.AddButton("&2", null));
			this.commandManager.Add("File.History.2", fileHistoryMenu.Items.AddButton("&3", null));
			this.commandManager.Add("File.History.3", fileHistoryMenu.Items.AddButton("&4", null));
			fileMenu.Items.AddSeparator();
			this.commandManager.Add("Application.Exit", fileMenu.Items.AddButton("E&xit", null));
		
			CommandBarMenu editMenu = menuBar.Items.AddMenu("&Edit");
			this.commandManager.Add("Edit.Undo", editMenu.Items.AddButton(CommandBarImageResource.Undo, "&Undo", null, Keys.Control | Keys.Z));
			this.commandManager.Add("Edit.Redo", editMenu.Items.AddButton(CommandBarImageResource.Redo, "&Redo", null, Keys.Control | Keys.Y));
			editMenu.Items.AddSeparator();
			this.commandManager.Add("Edit.Cut", editMenu.Items.AddButton(CommandBarImageResource.Cut, "Cu&t", null, Keys.Control | Keys.X));
			this.commandManager.Add("Edit.Copy", editMenu.Items.AddButton(CommandBarImageResource.Copy, "&Copy", null, Keys.Control | Keys.C));
			this.commandManager.Add("Edit.Paste", editMenu.Items.AddButton(CommandBarImageResource.Paste, "&Paste", null, Keys.Control | Keys.V));
			this.commandManager.Add("Edit.Delete", editMenu.Items.AddButton(CommandBarImageResource.Delete, "&Delete", null, Keys.Delete));
			editMenu.Items.AddSeparator();
			this.commandManager.Add("Edit.SelectAll", editMenu.Items.AddButton("Select &All", null, Keys.Control | Keys.A));
			editMenu.Items.AddSeparator();
			this.commandManager.Add("Edit.Find", editMenu.Items.AddButton(CommandBarImageResource.Search, "&Find...", null, Keys.Control | Keys.F));
			this.commandManager.Add("Edit.FindNext", editMenu.Items.AddButton("Find &Next", null, Keys.F3));
			this.commandManager.Add("Edit.Replace", editMenu.Items.AddButton("&Replace...", null, Keys.Control | Keys.H));

			CommandBarMenu insertMenu = menuBar.Items.AddMenu("&Insert");
			this.commandManager.Add("Edit.InsertHyperlink", insertMenu.Items.AddButton(FormatResource.Hyperlink, "Insert &Hyperlink", null, Keys.Control | Keys.K));
			this.commandManager.Add("Edit.InsertPicture", insertMenu.Items.AddButton(FormatResource.Picture, "Insert &Picture...", null));
			this.commandManager.Add("Edit.InsertDateTime", insertMenu.Items.AddButton("Insert &Date and Time...", null));

			CommandBarMenu formatMenu = menuBar.Items.AddMenu("F&ormat");
			this.commandManager.Add("Format.Bold", formatMenu.Items.AddCheckBox(FormatResource.Bold, "&Bold", Keys.Control | Keys.B));
			this.commandManager.Add("Format.Italic", formatMenu.Items.AddCheckBox(FormatResource.Italic, "&Italic", Keys.Control | Keys.I));
			this.commandManager.Add("Format.Underline", formatMenu.Items.AddCheckBox(FormatResource.Underline, "U&nderline", Keys.Control | Keys.U));
			this.commandManager.Add("Format.Superscript", formatMenu.Items.AddCheckBox(FormatResource.Superscript, "&Superscript"));
			this.commandManager.Add("Format.Subscript", formatMenu.Items.AddCheckBox(FormatResource.Subscript, "Subscri&pt"));
			this.commandManager.Add("Format.Strikethrough", formatMenu.Items.AddCheckBox(FormatResource.Strikethrough, "Stri&ke"));
			formatMenu.Items.AddSeparator();
			this.commandManager.Add("Format.ForeColor", formatMenu.Items.AddButton(FormatResource.ForeColor, "&Fore Color", null));
			this.commandManager.Add("Format.BackColor", formatMenu.Items.AddButton(FormatResource.BackColor, "B&ack Color", null));
			formatMenu.Items.AddSeparator();
			this.commandManager.Add("Format.AlignLeft", formatMenu.Items.AddCheckBox(FormatResource.AlignLeft, "Align &Left"));
			this.commandManager.Add("Format.AlignCenter", formatMenu.Items.AddCheckBox(FormatResource.AlignCenter, "Align &Center"));
			this.commandManager.Add("Format.AlignRight", formatMenu.Items.AddCheckBox(FormatResource.AlignRight, "Align &Right"));
			formatMenu.Items.AddSeparator();
			this.commandManager.Add("Format.OrderedList", formatMenu.Items.AddCheckBox(FormatResource.OrderedList, "&Numbering"));
			this.commandManager.Add("Format.UnorderedList", formatMenu.Items.AddCheckBox(FormatResource.UnorderedList, "B&ullets"));
			this.commandManager.Add("Format.Unindent", formatMenu.Items.AddButton(FormatResource.Unindent, "Unind&ent", null, Keys.Shift | Keys.Tab));
			this.commandManager.Add("Format.Indent", formatMenu.Items.AddButton(FormatResource.Indent, "In&dent", null, Keys.Tab));

			CommandBarMenu helpMenu = menuBar.Items.AddMenu("&Help");
			this.commandManager.Add("Application.About", helpMenu.Items.AddButton("&About Writer...", null));

			this.commandManager.AddTarget(this);

			this.FileNew();

			CommandLine commandLine = new CommandLine();
			string[] fileNames = commandLine.GetArguments(string.Empty);
			if ((fileNames != null) && (fileNames.Length > 0))
			{
				this.LoadFile(fileNames[0]);
			}
		}

		public void Run()
		{
			System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(this.Application_ThreadException);
			System.Windows.Forms.Application.Run(this.applicationWindow);
		}

		private void ApplicationWindow_Closing(object sender, CancelEventArgs e)
		{
			if (!this.FileClose())
			{
				e.Cancel = true;
			}
		}

		private void ApplicationWindow_Closed(object sender, EventArgs e)
		{
			// Save configuration file
			this.configurationManager.Save(this.GetType().Module.FullyQualifiedName);
		}

		private void ApplicationWindowActivated(object sender, EventArgs e) {
			this.document.HtmlControl.Focus();
		}
		public bool Execute(CommandState commandState)
		{
			switch (commandState.CommandName)
			{
				case "File.New":
					this.FileNew();
					return true;

				case "File.Open":
					this.FileOpen();
					return true;

				case "File.Save":
					this.document.Save();
					return true;

				case "File.SaveAs":
					this.document.SaveAs();
					this.fileHistory.AddFile(this.document.Url); 
					return true;

				case "File.FtpSetting":
					this.PublishSetting();
					return true;

				case "File.Publish":
					this.Publish();
					return true;

				case "Application.Exit":
					this.applicationWindow.Close();
					return true;

				case "Application.About":
					this.About();
					return true;

				case "File.History.0":
					this.LoadFile(this.fileHistory[0]);
					return true;

				case "File.History.1":
					this.LoadFile(this.fileHistory[1]);
					return true;

				case "File.History.2":
					this.LoadFile(this.fileHistory[2]);
					return true;

				case "File.History.3":
					this.LoadFile(this.fileHistory[3]);
					return true;
			}

			return false;
		}

		public bool QueryStatus(CommandState commandState)
		{
			this.UpdateText();

			switch (commandState.CommandName)
			{
				case "File.New":
				case "File.Open":
				case "Application.Exit":
				case "Application.Feedback":
				case "Application.CheckForUpdates":
				case "Application.About":
					commandState.IsEnabled = true;
					return true;

				case "File.Save":
					commandState.IsEnabled = (this.document != null) && (this.document.IsDirty);
					return true;

				case "File.SaveAs":
					commandState.IsEnabled = (this.document != null);
					return true;

				case "File.FtpSetting":
					commandState.IsEnabled = true;
					return true;

				case "File.Publish":
					commandState.IsEnabled = (!this.ftpConfiguration.IsEmpty);
					return true;

				case "File.History.0":
					commandState.IsVisible = (this.fileHistory.Count > 0);
					commandState.Text = "&1 " + this.fileHistory[0];
					return true;

				case "File.History.1":
					commandState.IsVisible = (this.fileHistory.Count > 1);
					commandState.Text = "&2 " + this.fileHistory[1];
					return true;
				
				case "File.History.2":
					commandState.IsVisible = (this.fileHistory.Count > 2);
					commandState.Text = "&3 " + this.fileHistory[2];
					return true;

				case "File.History.3":
					commandState.IsVisible = (this.fileHistory.Count > 3);
					commandState.Text = "&4 " + this.fileHistory[3];
					return true;
			}

			return false;
		}

		private void UpdateText()
		{
			string text = "Lutz Roeder's Writer";
			if (this.document != null)
			{
				text += " - ";
				text += this.document.Url;
				
				if ( this.document.IsDirty ) {
					text += " *";
				}
			}

			this.applicationWindow.Text = text;
		}

		private void FileNew()
		{
			if (this.FileClose())
			{
				this.LoadFile(null);
			}
		}

		private void FileOpen()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Multiselect = false;
			dialog.Filter = Resource.GetString("HtmlFilter");
			if (dialog.ShowDialog() == DialogResult.OK)
			{
                this.FileClose();
                string url = dialog.FileName;
                this.LoadFile(url);
            }
		}

		private bool FileClose()
		{
			if (this.document != null)
			{
				if (this.document.IsDirty)
				{
					string message = "Do you want to save the changes to \'" + document.Url + "\'.";
					DialogResult dialogResult = MessageBox.Show(this.applicationWindow, message, Resource.GetString("ApplicationName"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
					switch (dialogResult)
					{
						case DialogResult.Yes:
							this.document.Save();
							break;

						case DialogResult.Cancel:
							return false;

						case DialogResult.No:
							break;
					}
				}

				this.document.Dispose();
				this.document = null;
			}

			
			if (this.applicationWindow.View.Controls.Count > 0)
			{
				Control control = this.applicationWindow.View.Controls[0];
				control.Parent = null;
				control.Dispose();
			}

			return true;
		}

		private bool LoadFile(string url)
		{
			if (this.FileClose())
			{
				this.document = new Document(this.commandManager, url);

				this.applicationWindow.View.Controls.Add(this.document.HtmlControl);
				this.commandManager.AddTarget(this.document);

				this.fileHistory.AddFile(url);

				return true;
			}

			return false;
		}

		private void About()
		{
			AboutDialog dialog = new AboutDialog();
			dialog.Run();
		}

		private void PublishSetting() 
		{
			FtpConfigurationDialog dialog = new FtpConfigurationDialog();
			if (!this.ftpConfiguration.IsEmpty) 
			{
				dialog.FtpServer = this.ftpConfiguration.Host;
				dialog.Directory = this.ftpConfiguration.Directory;
				dialog.Port = this.ftpConfiguration.Port;
				dialog.Username = this.ftpConfiguration.UserName;

				if ((this.ftpConfiguration.Password != null) || (this.ftpConfiguration.Password.Length != 0))
				{
					dialog.StorePassword = true;
					dialog.Password = this.ftpConfiguration.Password;
				}
			}
			
			if (dialog.Run()) 
			{
				this.ftpConfiguration.Host = dialog.FtpServer;
				this.ftpConfiguration.UserName = dialog.Username;
				this.ftpConfiguration.Password = dialog.Password;
				this.ftpConfiguration.Directory = dialog.Directory;
				this.ftpConfiguration.Port = dialog.Port;
			}
		}

        private void Publish() 
        {
			string password = this.ftpConfiguration.Password;
			
			if ((password == null) || (password.Length == 0))
			{
				FtpPasswordDialog dialog = new FtpPasswordDialog();
				if ( dialog.Run() ) 
				{
					password = dialog.Password;
				} 
				else 
				{
					return;
				}
			}

			try 
			{
				this.document.Save(); // TODO: What if the user don't like to save the file locally? This will be fixed in the next week!
				Cursor currentCursor = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;

				FtpConnection ff = new FtpConnection(this.ftpConfiguration.Host, this.ftpConfiguration.UserName, password, Int32.Parse(this.ftpConfiguration.Port));
				ff.ChangeDirectory(this.ftpConfiguration.Directory);
				ff.Upload(this.document.Url);
				ff.Disconnect();

				Cursor.Current = currentCursor;
				MessageBox.Show(this.applicationWindow, "File Publised Successfully.", Resource.GetString("ApplicationName"), MessageBoxButtons.OK, MessageBoxIcon.Information);
	
			} 
			catch(Exception exception) 
			{
				MessageBox.Show(this.applicationWindow, exception.Message, Resource.GetString("ApplicationName"));
			}			
        }

		private void Application_ThreadException(object sender, ThreadExceptionEventArgs t) 
		{
			// Restart
			try 
			{
				this.document.Save();
				string file = this.document.Url;

				this.applicationWindow.Close();

				Process process = new Process();
				process.StartInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
				process.StartInfo.Arguments = (File.Exists(file) ? ( "\"" + file + "\"") : string.Empty);
				process.Start();
			} 
			catch 
			{
			}
		}
	}
}