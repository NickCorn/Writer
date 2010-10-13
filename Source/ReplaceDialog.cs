// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// Contributed by: Husein Choroomi (http://www.ManagedComponents.com)
// ------------------------------------------------------------
namespace Writer
{
	using System.Windows.Forms;
	using System.Drawing;
	using System;
	using Writer.Html;

	internal class ReplaceDialog : Dialog 
	{
		TextBox replaceTextBox = new TextBox();
		CheckBox caseCheckBox = new CheckBox();
		CheckBox wholeCheckBox = new CheckBox();
		RadioButton upRadio = new RadioButton();
		TextBox searchTextBox = new TextBox();			
		Button findNextButton = new Button();
		Button replaceButton = new Button();
		Button replaceAllButton = new Button();	
		HtmlControl htmlControl;

		public ReplaceDialog(HtmlControl _htmlControl) {

			this.htmlControl = _htmlControl;

			this.ClientSize = new System.Drawing.Size(360, 122);
			this.Text = "Replace";

			Label findWhatLabel = new Label();
			findWhatLabel.FlatStyle = FlatStyle.System;
			findWhatLabel.Location = new Point(8, 8 + 4);
			findWhatLabel.Size = new Size(56, 16);
			findWhatLabel.TabIndex = 0;
			findWhatLabel.Text = "Fi&nd what:";
			this.Controls.Add(findWhatLabel);
			
			// this.searchTextBox.BorderStyle = BorderStyle.FixedSingle;
			this.searchTextBox.Location = new Point(84, 8);
			this.searchTextBox.Size = new Size(184, 20);
			this.searchTextBox.TabIndex = 1;
			this.searchTextBox.Text = "";
			this.searchTextBox.TextChanged += new EventHandler(this.TextChangeSearchTextBox);
			this.Controls.Add(searchTextBox);
			
			this.findNextButton.FlatStyle = FlatStyle.System;
			this.findNextButton.Location = new Point(276, 8);
			this.findNextButton.TabIndex = 9;
			this.findNextButton.Text = "&Find Next";
			this.findNextButton.Enabled = false;
			this.findNextButton.Click += new EventHandler(this.ClickFindNextButton);
			this.Controls.Add(findNextButton);
			
			Button cancelButton = new Button();			
			cancelButton.FlatStyle = FlatStyle.System;
			cancelButton.Location = new Point(276, 92);
			cancelButton.TabIndex = 12;
			cancelButton.Text = "Cancel";
			cancelButton.DialogResult = DialogResult.Cancel;
			this.Controls.Add(cancelButton);
			this.CancelButton = cancelButton;
			
			RadioButton downRadio = new RadioButton();
			downRadio.Checked = true;
			downRadio.FlatStyle = FlatStyle.System;
			downRadio.Location = new Point(48, 16);
			downRadio.Size = new Size(48, 16);
			downRadio.TabIndex = 7;
			downRadio.TabStop = true;
			downRadio.Text = "&Down";
			this.Controls.Add(downRadio);

            this.upRadio.FlatStyle = FlatStyle.System;
			this.upRadio.Location = new Point(8, 16);
			this.upRadio.Size = new System.Drawing.Size(40, 16);
			this.upRadio.TabIndex = 8;
			this.upRadio.Text = "&Up";
			this.Controls.Add(upRadio);

			GroupBox directionGroupBox = new GroupBox();
			directionGroupBox.Controls.Add(downRadio);
			directionGroupBox.Controls.Add(upRadio);
			directionGroupBox.FlatStyle = FlatStyle.System;
			directionGroupBox.Location = new Point(164, 60);
			directionGroupBox.Size = new Size(104, 40);
			directionGroupBox.TabIndex = 6;
			directionGroupBox.TabStop = false;
			directionGroupBox.Text = "Direction";
			this.Controls.Add(directionGroupBox);

			this.wholeCheckBox.FlatStyle = FlatStyle.System;
			this.wholeCheckBox.Location = new Point(8, 60 + 4);
			this.wholeCheckBox.Size = new Size(136, 16);
			this.wholeCheckBox.TabIndex = 4;
			this.wholeCheckBox.Text = "Match &whole word only";
			this.Controls.Add(wholeCheckBox);

			this.caseCheckBox.FlatStyle = FlatStyle.System;
			this.caseCheckBox.Location = new Point(8, 76 + 4);
			this.caseCheckBox.Size = new Size(128, 16);
			this.caseCheckBox.TabIndex = 5;
			this.caseCheckBox.Text = "Match &case";
			this.Controls.Add(caseCheckBox);
			
			Label replaceWithLabel = new Label();
			replaceWithLabel.FlatStyle = FlatStyle.System;
			replaceWithLabel.Location = new Point(8, 32 + 4);
			replaceWithLabel.Size = new Size(72, 16);
			replaceWithLabel.TabIndex = 2;
			replaceWithLabel.Text = "Re&place with:";
			this.Controls.Add(replaceWithLabel);
			
			// this.replaceTextBox.BorderStyle = BorderStyle.FixedSingle;
			this.replaceTextBox.Location = new Point(84, 32);
			this.replaceTextBox.Size = new Size(184, 20);
			this.replaceTextBox.TabIndex = 3;
			this.replaceTextBox.Text = "";
			this.Controls.Add(replaceTextBox);
			
			this.replaceAllButton.FlatStyle = FlatStyle.System;
			this.replaceAllButton.Location = new Point(276, 64);
			this.replaceAllButton.TabIndex = 11;
			this.replaceAllButton.Text = "Replace &All";
			this.replaceAllButton.Enabled = false;
			this.replaceAllButton.Click += new EventHandler(this.ClickReplaceAllButton);
			this.Controls.Add(replaceAllButton);
			
            this.replaceButton.FlatStyle = FlatStyle.System;
			this.replaceButton.Location = new Point(276, 36);
			this.replaceButton.TabIndex = 10;
			this.replaceButton.Text = "&Replace";
			this.replaceButton.Enabled = false;
			this.replaceButton.Click += new EventHandler(this.ClickReplaceButton);
			this.Controls.Add(replaceButton);

			this.searchTextBox.Focus();
		}
		
		private void TextChangeSearchTextBox(object sender, EventArgs e){
			if ( this.searchTextBox.Text != string.Empty ) {
				this.findNextButton.Enabled = true;
				this.replaceButton.Enabled = true;
				this.replaceAllButton.Enabled = true;
			}
			else {
				this.findNextButton.Enabled = false;
				this.replaceButton.Enabled = false;
				this.replaceAllButton.Enabled = false;
			}
		}

		private void ClickFindNextButton(object sender, EventArgs e) {
			if ( !this.htmlControl.Find(this.searchTextBox.Text, this.caseCheckBox.Checked, this.wholeCheckBox.Checked, this.upRadio.Checked) ) {
				MessageBox.Show(this, "Finished searching the document.", Resource.GetString("ApplicationName"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void ClickReplaceButton(object sender, EventArgs e) {
			if ( !this.htmlControl.Replace(this.searchTextBox.Text, this.replaceTextBox.Text, this.caseCheckBox.Checked, this.wholeCheckBox.Checked, this.upRadio.Checked) ) {
				MessageBox.Show(this, "Finished searching the document.", Resource.GetString("ApplicationName"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void ClickReplaceAllButton(object sender, EventArgs e) {
			while ( this.htmlControl.Replace(this.searchTextBox.Text, this.replaceTextBox.Text, this.caseCheckBox.Checked, this.wholeCheckBox.Checked, this.upRadio.Checked) ) {
			}
		}
	}
}
