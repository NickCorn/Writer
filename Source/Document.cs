// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Writer.Html;
	using Writer.Forms;

	internal class Document : ICommandTarget, IDisposable
	{
		private static int documentIndex = 1;
		
		private CommandManager commandManager;
		private string url;
		private string permanentUrl;
		private HtmlControl htmlControl;
		private Timer timer;

		string findSearchText = string.Empty;
		bool findSearchCase = false;
		bool findSearchWhole = false;
		bool findSearchDirection = false;

		public Document(CommandManager commandManager, string url)
		{
			this.url = url;
			this.permanentUrl = url;

			this.commandManager = commandManager;

			this.timer = new Timer();
			this.timer.Interval = 500;
			this.timer.Tick += new EventHandler(this.Timer_Tick);
			this.timer.Start();

			this.htmlControl = new HtmlControl();
			this.htmlControl.Dock = DockStyle.Fill;
			this.htmlControl.TabIndex = 0;
			this.htmlControl.IsDesignMode = true;

			if (this.url == null)
			{
				string tempPath = Path.GetTempFileName();
				if (File.Exists(tempPath))
				{
					File.Delete(tempPath);
				}

				Directory.CreateDirectory(tempPath);

				string tempFileName = Path.ChangeExtension("Document" + documentIndex, ".htm");
				documentIndex++;

				this.url = Path.Combine(tempPath, tempFileName);

				StreamWriter writer = new StreamWriter(this.url);
				writer.Write("<html><body></body></html>");
				writer.Close();
			}

			StreamReader reader = File.OpenText(this.url);
			this.htmlControl.LoadHtml(reader.ReadToEnd(), this.url);
			reader.Close();

			CommandBarContextMenu contextMenu = new CommandBarContextMenu();
			this.commandManager.Add("Edit.Undo", contextMenu.Items.AddButton(CommandBarImageResource.Undo, "&Undo", null, Keys.Control | Keys.Z));
			this.commandManager.Add("Edit.Redo", contextMenu.Items.AddButton(CommandBarImageResource.Redo, "&Redo", null, Keys.Control | Keys.Y));
			contextMenu.Items.AddSeparator();
			this.commandManager.Add("Edit.Cut", contextMenu.Items.AddButton(CommandBarImageResource.Cut, "Cu&t", null, Keys.Control | Keys.X));
			this.commandManager.Add("Edit.Copy", contextMenu.Items.AddButton(CommandBarImageResource.Copy, "&Copy", null, Keys.Control | Keys.C));
			this.commandManager.Add("Edit.Paste", contextMenu.Items.AddButton(CommandBarImageResource.Paste, "&Paste", null, Keys.Control | Keys.V));
			this.commandManager.Add("Edit.Delete", contextMenu.Items.AddButton(CommandBarImageResource.Delete, "&Delete", null, Keys.Delete));
			this.htmlControl.ContextMenu = contextMenu;
		}

		public void Dispose()
		{
			this.commandManager.RemoveTarget(this);

			this.timer.Stop();
			this.timer.Tick -= new EventHandler(this.Timer_Tick);

			this.htmlControl.Dispose();
			this.htmlControl = null;

			if (this.permanentUrl == null)
			{
				string tempPath = Path.GetDirectoryName(this.url);
				if (Directory.Exists(tempPath))
				{
					Directory.Delete(tempPath, true);
				}
			}
		}

		public HtmlControl HtmlControl
		{
			get { return this.htmlControl; }
		}

		public bool IsDirty
		{
			get { return this.htmlControl.IsDirty; }
		}

		public string Url
		{
			get { return this.url; }
		}

		public void Save()
		{
			if (this.permanentUrl != null)
			{
				this.SaveAs(this.permanentUrl);
			}
			else
			{
				this.SaveAs();
			}
		}

		public void SaveAs()
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filter = Resource.GetString("HtmlFilter");
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string url = dialog.FileName;
				this.SaveAs(url);
			}
		}

		private void SaveAs(string url)
		{
			Cursor currentCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			
			// TODO url != fileName

			// XHTML Formatting
			using (StreamWriter writer = new StreamWriter(url))
			{
				HtmlFormatter formatter = new HtmlFormatter();
				formatter.Format(this.htmlControl.SaveHtml(), writer);
			}

			// BUGFIX Special character corruption
			// insert file header bytes to resolve corruption
			FileStream fs = new FileStream(url, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			byte[] buffer = new byte[fs.Length];
			fs.Read(buffer,0,(int)fs.Length);
			fs.Close();
			//	if (!buffer[0].Equals( Convert.ToByte("EF", 16) ))  // 239
			//	{ // because this is always true remove the statement
			try 
			{
				int byteCount = 3;
				fs = new FileStream(url, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				fs.SetLength( buffer.Length + byteCount );
				byte[] characters = new byte[] { Convert.ToByte("EF", 16), 
												Convert.ToByte("BB", 16), 
												Convert.ToByte("BF", 16) };   //  239, 187, 191
				fs.Write(characters, 0, byteCount);
				fs.Write(buffer, 0, buffer.Length);
			}
			finally
			{
				if (fs != null) 
				{
					fs.Close();
				}
			}

            
			this.permanentUrl = url;
			this.url = url;

			Cursor.Current = currentCursor;
		}

		public bool Execute(CommandState commandState)
		{
			if (this.htmlControl.IsHandleCreated && this.htmlControl.IsReady)
			{
				switch (commandState.CommandName)
				{
					case "File.Print":
						this.htmlControl.Print();
						return true;
	
					case "File.PrintPreview":
						this.htmlControl.PrintPreview();
						return true;

					case "Edit.Undo":
						this.htmlControl.Undo();
						return true;

					case "Edit.Redo":
						this.htmlControl.Redo();
						return true;

					case "Edit.Cut":
						this.htmlControl.Cut();
						return true;

					case "Edit.Copy":
						this.htmlControl.Copy();
						return true;

					case "Edit.Paste":
						this.htmlControl.Paste();
						return true;

					case "Edit.Delete":
						this.htmlControl.Delete();
						return true;

					case "Edit.SelectAll":
						this.htmlControl.SelectAll();
						return true;

					case "Format.ForeColor.White":
						this.htmlControl.TextFormatting.ForeColor = Color.White;
						return true;

					case "Format.ForeColor.Red":
						this.htmlControl.TextFormatting.ForeColor = Color.Red;
						return true;

					case "Format.ForeColor.Green":
						this.htmlControl.TextFormatting.ForeColor = Color.FromArgb(0, 255, 0);
						return true;

					case "Format.ForeColor.Blue":
						this.htmlControl.TextFormatting.ForeColor = Color.Blue;
						return true;

					case "Format.ForeColor.Yellow":
						this.htmlControl.TextFormatting.ForeColor = Color.Yellow;
						return true;

					case "Format.ForeColor.Black":
						this.htmlControl.TextFormatting.ForeColor = Color.Black;
						return true;

					case "Format.ForeColor":
					{
						ColorDialog dialog = new ColorDialog();
						dialog.Color = this.htmlControl.TextFormatting.ForeColor;
						if (dialog.ShowDialog() == DialogResult.OK)
						{
							this.htmlControl.TextFormatting.ForeColor = dialog.Color;
						}
					}
						return true;

					case "Format.BackColor.Black":
						this.htmlControl.TextFormatting.BackColor = Color.Black;
						return true;

					case "Format.BackColor.Red":
						this.htmlControl.TextFormatting.BackColor = Color.Red;
						return true;

					case "Format.BackColor.Green":
						this.htmlControl.TextFormatting.BackColor = Color.FromArgb(0, 255, 0);
						return true;

					case "Format.BackColor.Blue":
						this.htmlControl.TextFormatting.BackColor = Color.Blue;
						return true;

					case "Format.BackColor.Yellow":
						this.htmlControl.TextFormatting.BackColor = Color.Yellow;
						return true;

					case "Format.BackColor.White":
						this.htmlControl.TextFormatting.BackColor = Color.White;
						return true;

					case "Format.BackColor":
					{
						ColorDialog dialog = new ColorDialog();
						dialog.Color = this.htmlControl.TextFormatting.BackColor;
						if (dialog.ShowDialog() == DialogResult.OK)
						{
							this.htmlControl.TextFormatting.BackColor = dialog.Color;
						}
					}
						return true;

					case "Format.Font":
						this.htmlControl.TextFormatting.FontName = (commandState as CommandComboBoxState).Value;
						return true;

					case "Format.FontSize":
						this.htmlControl.TextFormatting.FontSize = (HtmlFontSize)Int32.Parse((commandState as CommandComboBoxState).Value);
						return true;

					case "Format.Bold":
						this.htmlControl.TextFormatting.ToggleBold();
						return true;

					case "Format.Italic":
						this.htmlControl.TextFormatting.ToggleItalics();
						return true;

					case "Format.Underline":
						this.htmlControl.TextFormatting.ToggleUnderline();
						return true;

					case "Format.Strikethrough":
						this.htmlControl.TextFormatting.ToggleStrikethrough();
						return true;

					case "Format.Superscript":
						this.htmlControl.TextFormatting.ToggleSuperscript();
						return true;

					case "Format.Subscript":
						this.htmlControl.TextFormatting.ToggleSubscript();
						return true;

					case "Format.AlignLeft":
						this.htmlControl.TextFormatting.Alignment = HtmlAlignment.Left;
						return true;

					case "Format.AlignCenter":
						this.htmlControl.TextFormatting.Alignment = HtmlAlignment.Center;
						return true;

					case "Format.AlignRight":
						this.htmlControl.TextFormatting.Alignment = HtmlAlignment.Right;
						return true;

					case "Format.OrderedList":
						this.htmlControl.TextFormatting.HtmlFormat = HtmlFormat.OrderedList;
						return true;

					case "Format.UnorderedList":
						this.htmlControl.TextFormatting.HtmlFormat = HtmlFormat.UnorderedList;
						return true;

					case "Format.Indent":
						this.htmlControl.TextFormatting.Indent();
						return true;

					case "Format.Unindent":
						this.htmlControl.TextFormatting.Unindent();
						return true;

					case "Edit.InsertHyperlink":
						this.InsertHyperlink();
						return true;

					case "Edit.InsertPicture":
						this.InsertPicture();
						return true;

					case "Edit.InsertDateTime":
						this.InsertDateTime();
						return true;

					case "Edit.Find":
						this.Find();
						return true;

					case "Edit.FindNext":
						this.FindNext();
						return true;
					
					case "Edit.Replace":
						this.Replace();
						return true;
				}
			}

			return false;
		}

		public bool QueryStatus(CommandState commandState)
		{
			if (this.htmlControl.IsHandleCreated && this.htmlControl.IsReady)
			{
				CommandCheckBoxState checkBoxState = commandState as CommandCheckBoxState;

				switch (commandState.CommandName)
				{
					case "File.Print":
						commandState.IsEnabled = this.htmlControl.CanPrint;
						return true;
	
					case "File.PrintPreview":
						commandState.IsEnabled = this.htmlControl.CanPrintPreview;
						return true;

					case "Edit.Undo":
						commandState.IsEnabled = this.htmlControl.CanUndo;
						commandState.Text = "&Undo " + this.htmlControl.UndoDescription;
						return true;

					case "Edit.Redo":
						commandState.IsEnabled = this.htmlControl.CanRedo;
						commandState.Text = "&Redo " + this.htmlControl.RedoDescription;
						return true;

					case "Edit.Copy":
						commandState.IsEnabled = this.htmlControl.CanCopy;
						return true;

					case "Edit.Cut":
						commandState.IsEnabled = this.htmlControl.CanCut;
						return true;

					case "Edit.Paste":
						commandState.IsEnabled = this.htmlControl.CanPaste;
						return true;

					case "Edit.Delete":
						commandState.IsEnabled = this.htmlControl.CanDelete;
						return true;

					case "Edit.SelectAll":
						commandState.IsEnabled = this.htmlControl.CanSelectAll;
						return true;

					case "Edit.Find":
					case "Edit.FindNext":
					case "Edit.Replace":
						commandState.IsEnabled = true;
						return true;

					case "Edit.InsertHyperlink":
					case "Edit.InsertPicture":
					case "Edit.InsertDateTime":
						commandState.IsEnabled = this.htmlControl.CanInsertHtml;
						return true;

					case "Format.Font":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanSetFontName;
						(commandState as CommandComboBoxState).Value = this.htmlControl.TextFormatting.FontName;
						return true;

					case "Format.FontSize":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanSetFontSize;
						(commandState as CommandComboBoxState).Value = ((int)this.htmlControl.TextFormatting.FontSize).ToString();
						return true;

					case "Format.Bold":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanToggleBold;
						checkBoxState.IsChecked = this.htmlControl.TextFormatting.IsBold;
						return true;

					case "Format.Italic":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanToggleItalic;
						checkBoxState.IsChecked = this.htmlControl.TextFormatting.IsItalic;
						return true;

					case "Format.Underline":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanToggleUnderline;
						checkBoxState.IsChecked = this.htmlControl.TextFormatting.IsUnderline;
						return true;

					case "Format.Strikethrough":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanToggleStrikethrough;
						checkBoxState.IsChecked = this.htmlControl.TextFormatting.IsStrikethrough;
						return true;

					case "Format.Subscript":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanToggleSubscript;
						checkBoxState.IsChecked = this.htmlControl.TextFormatting.IsSubscript;
						return true;

					case "Format.Superscript":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanToggleSuperscript;
						checkBoxState.IsChecked = this.htmlControl.TextFormatting.IsSuperscript;
						return true;

					case "Format.AlignLeft":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanAlign(HtmlAlignment.Left);
						checkBoxState.IsChecked = (this.htmlControl.TextFormatting.Alignment == HtmlAlignment.Left);
						return true;

					case "Format.AlignRight":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanAlign(HtmlAlignment.Right);
						checkBoxState.IsChecked = (this.htmlControl.TextFormatting.Alignment == HtmlAlignment.Right);
						return true;

					case "Format.AlignCenter":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanAlign(HtmlAlignment.Center);
						checkBoxState.IsChecked = (this.htmlControl.TextFormatting.Alignment == HtmlAlignment.Center);
						return true;

					case "Format.OrderedList":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanSetHtmlFormat;
						checkBoxState.IsChecked = (this.htmlControl.TextFormatting.HtmlFormat == HtmlFormat.OrderedList);
						return true;

					case "Format.UnorderedList":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanSetHtmlFormat;
						checkBoxState.IsChecked = (this.htmlControl.TextFormatting.HtmlFormat == HtmlFormat.UnorderedList);
						return true;

					case "Format.Indent":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanIndent;
						return true;

					case "Format.Unindent":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanUnindent;
						return true;

					case "Format.ForeColor":
					case "Format.ForeColor.Black":
					case "Format.ForeColor.Yellow":
					case "Format.ForeColor.Red":
					case "Format.ForeColor.Green":
					case "Format.ForeColor.Blue":
					case "Format.ForeColor.White":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanSetForeColor;
						return true;

					case "Format.BackColor":
					case "Format.BackColor.Black":
					case "Format.BackColor.Yellow":
					case "Format.BackColor.Red":
					case "Format.BackColor.Green":
					case "Format.BackColor.Blue":
					case "Format.BackColor.White":
						commandState.IsEnabled = this.htmlControl.TextFormatting.CanSetBackColor;
						return true;
				}
			}

			return false;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			this.commandManager.QueryStatus();
		}

		private void InsertHyperlink() 
		{
			this.htmlControl.InsertHyperlink(null, null);
		}

		private void InsertPicture()
		{
			this.htmlControl.InsertImage();
		}

		private void InsertDateTime(){
			InsertDateTimeDialog dialog = new InsertDateTimeDialog();
			if ( dialog.Run() ) {
				this.htmlControl.InsertHtml(dialog.Format);
			}
		}

		private void Find() {
			FindDialog dialog = new FindDialog();
			if ( findSearchText != string.Empty ) {
				dialog.SearchText = this.findSearchText;
				dialog.IsCaseChecked = this.findSearchCase;
				dialog.IsWholeChecked = this.findSearchWhole;
				dialog.IsUp = this.findSearchDirection;
			}
			if ( dialog.Run() ) {
				this.findSearchText = dialog.SearchText;
				this.findSearchCase = dialog.IsCaseChecked;
				this.findSearchWhole = dialog.IsWholeChecked;
				this.findSearchDirection = dialog.IsUp;
				if ( !this.htmlControl.Find(this.findSearchText, this.findSearchCase, this.findSearchWhole, this.findSearchDirection) ) {
					MessageBox.Show("Finished searching the document.", Resource.GetString("ApplicationName"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
		}

		private void Replace() {
			ReplaceDialog dialog = new ReplaceDialog(this.htmlControl);
			dialog.ShowDialog();
		}

		private void FindNext() {
			if ( this.findSearchText != string.Empty ) {
				this.htmlControl.Find(this.findSearchText, this.findSearchCase, this.findSearchWhole, this.findSearchDirection);
			}
		}
	}
}
