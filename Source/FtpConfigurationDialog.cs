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
			
	internal class FtpConfigurationDialog : Dialog 
	{
		private Label passwordLabel;
		private Label descriptionLabel;
		private TextBox ftpServerTextBox;
		private TextBox usernameTextBox;
		private TextBox portTextBox;
		private TextBox passwordTextBox;
		private TextBox subdirectoryTextBox;
		private RadioButton askForPasswordRadioButton;
		private RadioButton storePasswordRadioButton;
		private Button okButton;
	
		public FtpConfigurationDialog() 
		{
			this.Text = "FTP Settings";
			this.ClientSize = new Size(296, 238);

			Label ftpServerLabel = new Label();
			ftpServerLabel.Location = new Point(8, 8 + 6);
			ftpServerLabel.Size = new Size(68, 23);
			ftpServerLabel.TabStop = false;
			ftpServerLabel.Text = "FTP Server:";
			ftpServerLabel.TextAlign = ContentAlignment.MiddleLeft;
			ftpServerLabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(ftpServerLabel);
			
			this.passwordLabel = new Label();
			this.passwordLabel.Enabled = false;
			this.passwordLabel.Location = new Point(8, 104 + 3);
			this.passwordLabel.Size = new Size(76, 23);
			this.passwordLabel.TabIndex = 1;
			this.passwordLabel.Text = "Password:";
			this.passwordLabel.TextAlign = ContentAlignment.MiddleLeft;
			this.passwordLabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(this.passwordLabel);
			
			this.askForPasswordRadioButton = new RadioButton();
			this.askForPasswordRadioButton.Checked = true;
			this.askForPasswordRadioButton.FlatStyle = FlatStyle.System;
			this.askForPasswordRadioButton.Location = new Point(8, 60);
			this.askForPasswordRadioButton.Size = new Size(228, 20);
			this.askForPasswordRadioButton.TabIndex = 9;
			this.askForPasswordRadioButton.TabStop = true;
			this.askForPasswordRadioButton.Text = "Ask for Password when Publishing";
			this.askForPasswordRadioButton.Enter += new System.EventHandler(this.ShowDescription);
			this.Controls.Add(this.askForPasswordRadioButton);
			
			this.storePasswordRadioButton = new RadioButton();
			this.storePasswordRadioButton.FlatStyle = FlatStyle.System;
			this.storePasswordRadioButton.Location = new Point(8, 80);
			this.storePasswordRadioButton.Size = new Size(232, 20);
			this.storePasswordRadioButton.TabIndex = 10;
			this.storePasswordRadioButton.Text = "Store Password (not secure)";
			this.storePasswordRadioButton.Enter += new System.EventHandler(this.ShowDescription);
			this.storePasswordRadioButton.CheckedChanged += new System.EventHandler(this.StorePasswordRadioButtonCheckedChanged);
			this.Controls.Add(this.storePasswordRadioButton);
			
			this.ftpServerTextBox = new TextBox();
			this.ftpServerTextBox.Location = new Point(80, 12);
			this.ftpServerTextBox.Size = new Size(132, 20);
			this.ftpServerTextBox.TabIndex = 4;
			this.ftpServerTextBox.Text = "";
			this.ftpServerTextBox.Enter += new System.EventHandler(this.ShowDescription);
			this.ftpServerTextBox.TextChanged += new EventHandler(this.TextBoxesTextChanged);
			this.Controls.Add(this.ftpServerTextBox);
			
			this.usernameTextBox = new TextBox();
			this.usernameTextBox.Location = new Point(80, 36);
			this.usernameTextBox.Size = new Size(132, 20);
			this.usernameTextBox.TabIndex = 5;
			this.usernameTextBox.Text = "";
			this.usernameTextBox.Enter += new System.EventHandler(this.ShowDescription);
			this.usernameTextBox.TextChanged += new EventHandler(this.TextBoxesTextChanged);
			this.Controls.Add(this.usernameTextBox);
			
			Label portLabel = new Label();
			portLabel.Location = new Point(220, 8 + 6);
			portLabel.Size = new Size(28, 23);
			portLabel.TabIndex = 7;
			portLabel.Text = "Port:";
			portLabel.TextAlign = ContentAlignment.MiddleLeft;
			portLabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(portLabel);

			this.portTextBox = new TextBox();
			this.portTextBox.Location = new Point(252, 12);
			this.portTextBox.Size = new Size(36, 20);
			this.portTextBox.TabIndex = 8;
			this.portTextBox.Text = "21";
			this.portTextBox.Enter += new System.EventHandler(this.ShowDescription);
			this.portTextBox.TextChanged += new EventHandler(this.TextBoxesTextChanged);
			this.Controls.Add(this.portTextBox);

			Label usernameLabel = new Label();
			usernameLabel.Location = new Point(8, 32 + 6);
			usernameLabel.Size = new Size(68, 23);
			usernameLabel.TabIndex = 10;
			usernameLabel.Text = "Username:";
			usernameLabel.TextAlign = ContentAlignment.MiddleLeft;
			usernameLabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(usernameLabel);
			
			Label subdirectoryLabel = new Label();
			subdirectoryLabel.Location = new Point(8, 128 + 3);
			subdirectoryLabel.Size = new Size(76, 23);
			subdirectoryLabel.TabIndex = 11;
			subdirectoryLabel.Text = "Subdirectory:";
			subdirectoryLabel.TextAlign = ContentAlignment.MiddleLeft;
			subdirectoryLabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(subdirectoryLabel);
			
			this.passwordTextBox = new TextBox();
			this.passwordTextBox.Enabled = false;
			this.passwordTextBox.Font = new Font("Wingdings", 8.25F);
			this.passwordTextBox.PasswordChar = 'l';
			this.passwordTextBox.Location = new Point(84, 104);
			this.passwordTextBox.Size = new Size(128, 20);
			this.passwordTextBox.TabIndex = 12;
			this.passwordTextBox.Text = "";
			this.passwordTextBox.Enter += new System.EventHandler(this.ShowDescription);
			this.passwordTextBox.TextChanged += new EventHandler(this.TextBoxesTextChanged);
			this.Controls.Add(this.passwordTextBox);
			
			this.subdirectoryTextBox = new TextBox();
			this.subdirectoryTextBox.Location = new Point(84, 128);
			this.subdirectoryTextBox.Size = new Size(128, 20);
			this.subdirectoryTextBox.TabIndex = 13;
			this.subdirectoryTextBox.Text = "";
			this.subdirectoryTextBox.Enter += new System.EventHandler(this.ShowDescription);
			this.subdirectoryTextBox.TextChanged += new EventHandler(this.TextBoxesTextChanged);
			this.Controls.Add(this.subdirectoryTextBox);
			
			this.descriptionLabel = new Label();
			this.descriptionLabel.BorderStyle = BorderStyle.FixedSingle;
			this.descriptionLabel.Location = new Point(4, 156);
			this.descriptionLabel.Size = new Size(288, 45);
			this.descriptionLabel.TabIndex = 14;
			this.descriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
			this.descriptionLabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(this.descriptionLabel);
			
			okButton = new Button();
			okButton.DialogResult = DialogResult.OK;
			okButton.FlatStyle = FlatStyle.System;
			okButton.Location = new Point(136, 208);
			okButton.TabIndex = 15;
			okButton.Text = "OK";
			okButton.Enabled = false;
			this.Controls.Add(okButton);
			this.AcceptButton = okButton;
			
			Button cancelButton = new Button();
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.FlatStyle = FlatStyle.System;
			cancelButton.Location = new Point(216, 208);
			cancelButton.TabIndex = 16;
			cancelButton.Text = "Cancel";
			this.Controls.Add(cancelButton);
			this.CancelButton = cancelButton;
			
			this.ftpServerTextBox.Focus();
		}

		private bool IsComplete() {
			if ( (this.ftpServerTextBox.Text != string.Empty) && (this.usernameTextBox.Text != string.Empty) && 
				(this.portTextBox.Text != string.Empty) ) {

				if ( this.passwordLabel.Enabled ) {
					return (this.passwordTextBox.Text != string.Empty) ? true : false;
				}

				return true;
			} else {
				return false;
			}
		}

		private void TextBoxesTextChanged(object sender, EventArgs e) {
			bool isComplete = IsComplete();
			if ( isComplete ) {
				this.okButton.Enabled = true;				
			} else {
				this.okButton.Enabled = false;
			}
		}

		private void StorePasswordRadioButtonCheckedChanged(object sender, System.EventArgs e) {
			if ( this.storePasswordRadioButton.Checked ) {
				this.passwordLabel.Enabled = true;
				this.passwordTextBox.Enabled = true;
			} else {
				this.passwordTextBox.Text = string.Empty;
				this.passwordLabel.Enabled = false;
				this.passwordTextBox.Enabled = false;
			}
			this.TextBoxesTextChanged(null, null);
		}

		private void ShowDescription(object sender, System.EventArgs e) {
			if ( sender == this.ftpServerTextBox ) {
				this.descriptionLabel.Text = "Type the name of the server, for example, " +
					"www.yourhost.com. You can also use an IP address like 127.0.0.1.";
			} else if ( sender == this.usernameTextBox ) {
				this.descriptionLabel.Text = "Type your server Username here.";
			} else if ( sender == this.portTextBox ) {
				this.descriptionLabel.Text = "Most FTP servers use port 21. If your server uses a different port, " +
					"specify that here.";
			} else if ( sender == this.passwordTextBox ) {
				this.descriptionLabel.Text = "Type your server Password here."; 
			} else if ( sender == this.subdirectoryTextBox ) {
				this.descriptionLabel.Text = "If you need to publish the file in a subdirectory " +
                    "Type the subdirectory here.";
			} else if ( sender == this.askForPasswordRadioButton ) {
				this.descriptionLabel.Text = "If you check this Radio Button, Writer will ask you for the FTP server password " +
					"every time you try to publish to your server.";
			} else if ( sender == this.storePasswordRadioButton ) {
				this.descriptionLabel.Text = "If you check this Radio Button, Writer will save your password in its config file, " +
					"which is not secure.";
			}
		}


		public string FtpServer {
			get {
				return this.ftpServerTextBox.Text;
			}

			set {
				this.ftpServerTextBox.Text = value;
			}
		}

		public string Username {
			get {
				return this.usernameTextBox.Text;
			}

			set {
				this.usernameTextBox.Text = value;
			}
		}

		public string Password {
			get {
				return this.passwordTextBox.Text;
			}

			set {
				this.passwordTextBox.Text = value;
			}
		}

		public string Directory {
			get {
				return this.subdirectoryTextBox.Text;
			}

			set {
				this.subdirectoryTextBox.Text = value;
			}
		}

		public string Port {
			get {
				return this.portTextBox.Text;
			}

			set {
				this.portTextBox.Text = value;
			}
		}

		public bool StorePassword {
			set {
				this.storePasswordRadioButton.Checked = value;
			}
		}
	}
}