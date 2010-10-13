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
	
	internal class InsertDateTimeDialog : Dialog 
	{
		private ListBox dateTimeFomatsListBox = new ListBox();
		
		public InsertDateTimeDialog() {
						
			this.ClientSize = new Size(280, 222);
			this.Text = "Date and Time";

			Label availableFormatlabel = new Label();
			availableFormatlabel.Location = new Point(8, 8);
			availableFormatlabel.Size = new Size(104, 16);
			availableFormatlabel.TabIndex = 0;
			availableFormatlabel.Text = "&Available Formats:";
			availableFormatlabel.FlatStyle = FlatStyle.System;
			this.Controls.Add(availableFormatlabel);
			
			Button acceptButton = new Button();
			acceptButton.FlatStyle = FlatStyle.System;
			acceptButton.Location = new Point(192, 24);
			acceptButton.DialogResult = DialogResult.OK;
			acceptButton.TabIndex = 2;
			acceptButton.Text = "OK";
			this.Controls.Add(acceptButton);
			this.AcceptButton = acceptButton;

			Button cancelButton = new Button();
			cancelButton.FlatStyle = FlatStyle.System;
			cancelButton.Location = new Point(192, 56);
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.TabIndex = 3;
			cancelButton.Text = "Cancel";
			this.Controls.Add(cancelButton);
			this.CancelButton = cancelButton;
			
			this.dateTimeFomatsListBox.Location = new System.Drawing.Point(8, 24);
			this.dateTimeFomatsListBox.Name = "DateTimeFomatsListBox";
			this.dateTimeFomatsListBox.Size = new System.Drawing.Size(176, 186);
			this.dateTimeFomatsListBox.TabIndex = 1;
			this.dateTimeFomatsListBox.DoubleClick += new EventHandler(this.DoubleClickDateTimeFormatListBox);
			this.Controls.Add(this.dateTimeFomatsListBox);
			
			this.AddDateTimeFormats();
			this.dateTimeFomatsListBox.SelectedIndex = 0;
			this.dateTimeFomatsListBox.Focus();
		}

		private void AddDateTimeFormats() {
			DateTime dateTimeNow = DateTime.Now;
		
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("M/d/yyyy"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("M/d/yy"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("MM/d/yy"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("MM/d/yyyy"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("yy/MM/d"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("yyyy-MM-d"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("d-MMM-yy"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("dddd, MMMM dd, yyyy"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("MMMM d, yyyy"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("dddd, dd MMMM, yyyy"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("d MMMM, yyyy"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("h:mm:ss tt"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("hh:mm:ss tt"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("h:mm:ss"));
			this.dateTimeFomatsListBox.Items.Add(dateTimeNow.ToString("hh:mm:ss"));
		}

		public string Format {
			get { return this.dateTimeFomatsListBox.SelectedItem.ToString(); }
		}

		private void DoubleClickDateTimeFormatListBox(object sender, EventArgs ea) {
			DialogResult = DialogResult.OK;
		}
	}
}
