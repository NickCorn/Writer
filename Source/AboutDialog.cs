// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Drawing;
	using System.Reflection;
	using System.Windows.Forms;
	using Writer.Forms;

	internal class AboutDialog : Dialog
	{
		public AboutDialog()
		{
			this.Text = "About";    	
			this.ClientSize = new Size(310, 200);

			PictureBox pictureBox = new PictureBox();
			pictureBox.Location = new Point(5, 5);
			pictureBox.Size = new Size(304, 88);
			pictureBox.Image = ImageResource.Application;
			this.Controls.Add(pictureBox);

			Label applicationLabel = new Label();
			applicationLabel.FlatStyle = FlatStyle.System;
			applicationLabel.Text = (this.GetType().Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute).Title + ", Version " + GetType().Assembly.GetName().Version;
			applicationLabel.Location = new Point(20,100);
			applicationLabel.Size = new Size(270, 16);
			this.Controls.Add(applicationLabel);

			Label messageLabel = new Label();
			messageLabel.FlatStyle = FlatStyle.System;
			messageLabel.Text = (this.GetType().Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute).Copyright;
			messageLabel.Location = new Point(20,118);
			messageLabel.Size = new Size(270, 32);
			this.Controls.Add(messageLabel);

			LinkLabel linkLabel = new LinkLabel();
			linkLabel.FlatStyle = FlatStyle.System;
			linkLabel.Location = new Point(20, 150);
			linkLabel.Size = new Size(200, 16);
			linkLabel.Text = Resource.GetString("Homepage");
			this.Controls.Add(linkLabel);
			
			Button acceptButton = new Button();
			acceptButton.FlatStyle = FlatStyle.System;
			acceptButton.Location = new Point(230, 170);
			acceptButton.Text = Resource.GetString("Ok");
			acceptButton.Size = new Size(75, 23);
			acceptButton.TabIndex = 0;
			acceptButton.DialogResult = DialogResult.OK;
			this.Controls.Add(acceptButton);
			
			this.AcceptButton = acceptButton;
			this.CancelButton = acceptButton;
		}
	}
}
