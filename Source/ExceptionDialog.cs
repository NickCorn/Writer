namespace Writer
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	
	internal class ExceptionDialog : Dialog 
	{
	
		private TextBox descriptionTextBox;
		private string reportMessage = string.Empty;
		
		public ExceptionDialog() {

			this.Text = "Bug Report";
			this.ClientSize = new Size(352, 265);

			Label descriptionLabel = new Label();
			descriptionLabel.Location = new Point(8, 9);
			descriptionLabel.Size = new Size(336, 34);
			descriptionLabel.TabIndex = 0;
			descriptionLabel.Text = "Please tell us what you were doing when you got the error, if you are not sure leave" +
				" this text box blank and just press Send button.";
			this.Controls.Add(descriptionLabel);
			
			this.descriptionTextBox = new TextBox();
			this.descriptionTextBox.Location = new Point(8, 43);
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.descriptionTextBox.Size = new Size(336, 165);
			this.descriptionTextBox.TabIndex = 1;
			this.descriptionTextBox.Text = "";
			this.Controls.Add(this.descriptionTextBox);

			Button cancelButton = new Button();
			cancelButton.FlatStyle = FlatStyle.System;
			cancelButton.Location = new Point(268, 235);
			cancelButton.TabIndex = 3;
			cancelButton.Text = "Cancel";
			cancelButton.DialogResult = DialogResult.Cancel;
			this.Controls.Add(cancelButton);
			this.CancelButton = cancelButton;
			
		    Button sendButton = new Button();	
			sendButton.FlatStyle = FlatStyle.System;
			sendButton.Location = new Point(188, 235);
			sendButton.TabIndex = 2;
			sendButton.Text = "&Send";
			sendButton.DialogResult = DialogResult.OK;
			this.Controls.Add(sendButton);
						
			Writer.LinkLabel reportMessageLinkLabel = new Writer.LinkLabel();
			reportMessageLinkLabel.Location = new Point(8, 216);
			reportMessageLinkLabel.TabIndex = 4;
			reportMessageLinkLabel.TabStop = true;
			reportMessageLinkLabel.Text = "To see what you send to us Click here.";
			reportMessageLinkLabel.Click += new EventHandler(this.ReportMessageLinkLabelLinkClicked);
			this.Controls.Add(reportMessageLinkLabel);

			this.descriptionTextBox.Focus();
		}

		private void ReportMessageLinkLabelLinkClicked(object sender, EventArgs e) {
			MessageBox.Show(this.reportMessage, "Bug Description");
		}

		public string ReportMessage {
			set {
				this.reportMessage = value;
			}
		}

		public string UserDescription {
			get {
				return this.descriptionTextBox.Text;
			}
		}
	}
}
