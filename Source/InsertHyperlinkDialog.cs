// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	internal class InsertHyperlinkDialog : Dialog
	{
		private TextBox urlTextBox = new TextBox();
		private TextBox descriptionTextBox = new TextBox();

		public InsertHyperlinkDialog()
		{
			this.ClientSize = new Size(404, 88);
			this.Text = "Insert Hyperlink";

			Button acceptButton = new Button();
			acceptButton.FlatStyle = FlatStyle.System;
			acceptButton.Location = new Point(244, 60);
			acceptButton.Size = new Size(75, 23);
			acceptButton.TabIndex = 3;
			acceptButton.Text = "OK";
			acceptButton.DialogResult = DialogResult.OK;
			this.Controls.Add(acceptButton);
			this.AcceptButton = acceptButton;

			Label urlLabel = new Label();
			urlLabel.Location = new Point(8, 36);
			urlLabel.Size = new Size(36, 15);
			urlLabel.TabIndex = 0;
			urlLabel.Text = "&URL:";
			urlLabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(urlLabel);

			this.urlTextBox.Location = new Point(84, 32);
			this.urlTextBox.Size = new Size(316, 20);
			this.urlTextBox.TabIndex = 2;
			this.Controls.Add(this.urlTextBox);

			Button cancelButton = new Button();
			cancelButton.FlatStyle = FlatStyle.System;
			cancelButton.Location = new Point(324, 60);
			cancelButton.Size = new Size(75, 23);
			cancelButton.TabIndex = 4;
			cancelButton.Text = "Cancel";
			cancelButton.DialogResult = DialogResult.Cancel;
			this.Controls.Add(cancelButton);
			this.CancelButton = cancelButton;

			this.descriptionTextBox.Location = new Point(84, 8);
			this.descriptionTextBox.Size = new Size(316, 20);
			this.descriptionTextBox.TabIndex = 1;
			this.descriptionTextBox.Text = "";
			this.Controls.Add(this.descriptionTextBox);

			Label descriptionLabel = new Label();
			descriptionLabel.Location = new Point(8, 12);
			descriptionLabel.Size = new Size(65, 16);
			descriptionLabel.TabIndex = 0;
			descriptionLabel.Text = "&Description:";
			descriptionLabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(descriptionLabel);

			this.urlTextBox.Focus();
		}

		public string Description
		{
			get { return this.descriptionTextBox.Text; }
			
			set 
			{ 
				if (value != null)
				{
					this.descriptionTextBox.Text = value; 
				}
			}
		}

		public string Url
		{
			get { return this.urlTextBox.Text; }
		}
	}
}
