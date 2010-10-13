// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
// Contributed by: Husein Choroomi 
// http://www.ManagedComponents.com
// ------------------------------------------------------------
namespace Writer
{
	using System.Windows.Forms;
	using System.Drawing;
	using System;

	internal class FindDialog : Dialog 
	{
		TextBox searchTextBox = new TextBox();
		RadioButton upRadio = new RadioButton();
		CheckBox wholeCheckBox= new CheckBox();
		CheckBox caseCheckBox = new CheckBox();
		Button findNextButton = new Button();
	
		public FindDialog() {
			
			this.ClientSize = new System.Drawing.Size(344, 86);
			this.Text = "Find";
			
			Label findWhatLabel = new Label();
			findWhatLabel.FlatStyle = FlatStyle.System;
			findWhatLabel.Location = new Point(8, 8 + 4);
			findWhatLabel.Size = new Size(56, 16);
			findWhatLabel.TabIndex = 0;
			findWhatLabel.Text = "Fi&nd what:";
			this.Controls.Add(findWhatLabel);
			
			// this.searchTextBox.BorderStyle = BorderStyle.FixedSingle;
			this.searchTextBox.Location = new Point(64, 8);
			this.searchTextBox.Size = new Size(184, 20);
			this.searchTextBox.TabIndex = 1;
			this.searchTextBox.Text = "";
			this.searchTextBox.TextChanged += new EventHandler(this.TextChangeSearchTextBox);
			this.Controls.Add(this.searchTextBox);
			
			findNextButton.FlatStyle = FlatStyle.System;
			findNextButton.Location = new Point(256, 8);
			findNextButton.TabIndex = 6;
			findNextButton.Text = "&Find Next";
			findNextButton.DialogResult = DialogResult.OK;
			findNextButton.Enabled = false;
			this.Controls.Add(findNextButton);
			this.AcceptButton = findNextButton;
			
			Button cancelButton = new Button();
			cancelButton.FlatStyle = FlatStyle.System;
			cancelButton.Location = new Point(256, 36);
			cancelButton.TabIndex = 7;
			cancelButton.Text = "Cancel";
			cancelButton.DialogResult = DialogResult.Cancel;
			this.Controls.Add(cancelButton);
			this.CancelButton = cancelButton;
			
			this.upRadio.FlatStyle = FlatStyle.System;
			this.upRadio.Location = new Point(8, 16);
			this.upRadio.Size = new Size(40, 16);
			this.upRadio.TabIndex = 5;
			this.upRadio.Text = "&Up";
			this.Controls.Add(this.upRadio);
			
			RadioButton downRadio = new RadioButton();
			downRadio.Checked = true;
			downRadio.FlatStyle = FlatStyle.System;
			downRadio.Location = new Point(48, 16);
			downRadio.Size = new Size(48, 16);
			downRadio.TabIndex = 4;
			downRadio.TabStop = true;
			downRadio.Text = "&Down";
			this.Controls.Add(downRadio);
			
			GroupBox directionGroupBox = new GroupBox();
			directionGroupBox.Controls.Add(downRadio);
			directionGroupBox.Controls.Add(this.upRadio);
			directionGroupBox.FlatStyle = FlatStyle.System;
			directionGroupBox.Location = new Point(144, 32);
			directionGroupBox.Size = new Size(104, 40);
			directionGroupBox.TabIndex = 5;
			directionGroupBox.TabStop = false;
			directionGroupBox.Text = "Direction";
			this.Controls.Add(directionGroupBox);
			
			this.wholeCheckBox.FlatStyle = FlatStyle.System;
			this.wholeCheckBox.Location = new Point(8, 32 + 4);
			this.wholeCheckBox.Size = new Size(136, 16);
			this.wholeCheckBox.TabIndex = 2;
			this.wholeCheckBox.Text = "Match &whole word only";
			this.Controls.Add(this.wholeCheckBox);
			
			this.caseCheckBox.FlatStyle = FlatStyle.System;
			this.caseCheckBox.Location = new Point(8, 52 + 4);
			this.caseCheckBox.Size = new Size(128, 16);
			this.caseCheckBox.TabIndex = 3;
			this.caseCheckBox.Text = "Match &case";
			this.Controls.Add(this.caseCheckBox);

			this.searchTextBox.Focus();
		}
		
		public string SearchText {
			get { return this.searchTextBox.Text; }
			set { this.searchTextBox.Text = value; }
		}

		public bool IsUp {
			get { return this.upRadio.Checked; }
			set { this.upRadio.Checked = value; }
		}

		public bool IsWholeChecked {
			get { return this.wholeCheckBox.Checked; }
			set { this.wholeCheckBox.Checked = value; }
		}

		public bool IsCaseChecked {
			get { return this.caseCheckBox.Checked; }
			set { this.caseCheckBox.Checked = value; }
		}

		private void TextChangeSearchTextBox (object sender, EventArgs e) {
			if ( this.searchTextBox.Text != string.Empty ) {
				this.findNextButton.Enabled = true;
			}
			else {
				this.findNextButton.Enabled = false;
			}
		}
	}
}
