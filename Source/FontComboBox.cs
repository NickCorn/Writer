// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms;
	using Microsoft.Win32;

    	internal class FontComboBox : ComboBox 
	{
	        public FontComboBox() 
		{
			this.Text = "Times New Roman";
			this.Font = new Font("Tahoma", 8f);

			this.Width = 125;
			this.DropDownWidth = 200;
			
			this.DrawMode = DrawMode.OwnerDrawVariable;
			this.MaxDropDownItems = 12;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.SystemEvents_InstalledFontsChanged(this, EventArgs.Empty);
			SystemEvents.InstalledFontsChanged += new EventHandler(this.SystemEvents_InstalledFontsChanged);
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.InstalledFontsChanged -= new EventHandler(this.SystemEvents_InstalledFontsChanged);
			base.OnHandleDestroyed(e);
		}

		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			if (e.Index > 0)
			{
				e.ItemHeight = 18;
			}

			base.OnMeasureItem(e);
		}

	        protected override void OnDrawItem(DrawItemEventArgs e) 
		{
			if (!this.Enabled)
			{
				e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
			}
			else
			{
				e.DrawBackground();

				string fontName = null;
				if (e.Index == -1)
				{
					// Text shown in the combobox itself
					fontName = this.Text;
				}
				else
				{
					fontName = (string)this.Items[e.Index];
				}

				Font font = null;
				if ((fontName != null) && (fontName.Length != 0))
				{
					try
					{
						FontFamily fontFamily = new FontFamily(fontName);
						FontStyle fontStyle = FontStyle.Regular;
						if (!fontFamily.IsStyleAvailable(fontStyle))
						{
							fontStyle = FontStyle.Italic;
							if (!fontFamily.IsStyleAvailable(fontStyle))
							{
								fontStyle = FontStyle.Bold;
								if (!fontFamily.IsStyleAvailable(fontStyle))
								{
									throw new NotSupportedException();
								}
							}
						}
												
						font = new Font(fontName, (float)((e.Bounds.Height - 2) / 1.2), fontStyle, GraphicsUnit.Pixel);
					}
					catch (Exception)
					{
					}
				}
	        
				Rectangle textBounds = new Rectangle(e.Bounds.Left + 2, e.Bounds.Top, e.Bounds.Width - 2, e.Bounds.Height);
				using (TextGraphics textGraphics = new TextGraphics(e.Graphics))
				{
					textGraphics.DrawText(fontName, textBounds.Location, (font != null) ? font : e.Font, e.ForeColor);
				}

				if (font != null)
				{
					font.Dispose();
				}
			}
		}

        	private void SystemEvents_InstalledFontsChanged(object sender, EventArgs e) 
		{
			string currentFont = this.Text;
			this.Items.Clear();

			string[] fontArray = null;
			Hashtable fontTable = new Hashtable();
			FontFamily[] fonts = FontFamily.Families;

			// If we are not doing only fixed width fonts, just add all font families
			for (int i = 0; i < fonts.Length; i++)
			{
				fontTable[fonts[i].Name.ToLower()] = fonts[i].Name;
			}

			// Copy the font names to an array and add them to the dropdown
			fontArray = new string[fontTable.Count];
			fontTable.Values.CopyTo(fontArray, 0);
			Array.Sort(fontArray);

			this.Items.AddRange(fontArray);

			if (currentFont.Length != 0)
			{
				this.Text = currentFont;
			}
        	}
	}
}
