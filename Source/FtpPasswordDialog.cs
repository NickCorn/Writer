// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
// Contributed by: 
// Husein Choroomi (http://www.ManagedComponents.com)
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	internal class FtpPasswordDialog : Dialog 
	{
		private TextBox passwordTextBox;
				
		public FtpPasswordDialog() {

			this.Text = "Password";
			this.ClientSize = new Size(328, 74);
		
			Label descriptionLabel = new Label();
			descriptionLabel.Location = new Point(8, 8);
			descriptionLabel.Size = new Size(168, 23);
			descriptionLabel.Text = "Please enter your Password:";
			descriptionLabel.TextAlign = ContentAlignment.BottomLeft;
			descriptionLabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(descriptionLabel);

			this.passwordTextBox = new TextBox();
			this.passwordTextBox.Font = new Font("Wingdings", 8.25F);
			this.passwordTextBox.Location = new Point(8, 36);
			this.passwordTextBox.PasswordChar = 'l';
			this.passwordTextBox.Size = new Size(224, 20);
			this.passwordTextBox.Text = "";
			this.Controls.Add(this.passwordTextBox);

			Button okButton = new Button();
			okButton.DialogResult = DialogResult.OK;
			okButton.FlatStyle = FlatStyle.System;
			okButton.Location = new Point(244, 12);
			okButton.Text = "OK";
			okButton.FlatStyle = FlatStyle.System;
			this.Controls.Add(okButton);
			this.AcceptButton = okButton;
 
			Button cancelButton = new Button();
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.FlatStyle = FlatStyle.System;
			cancelButton.Location = new Point(244, 40);
			cancelButton.Text = "Cancel";
			cancelButton.FlatStyle = FlatStyle.System;
			this.Controls.Add(cancelButton);
			this.CancelButton = cancelButton;

            this.passwordTextBox.Focus();
		}

		public string Password {
			get { return this.passwordTextBox.Text; }
		}
	}
}
