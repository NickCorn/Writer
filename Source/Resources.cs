namespace Writer
{
	using System;
	using System.IO;
	using System.Drawing;

	internal sealed class FormatResource
	{
		private static Image[] images = null;

		private FormatResource()
		{
		}

		static FormatResource()
		{
			Stream stream = typeof(CommandBarImageResource).Assembly.GetManifestResourceStream("Writer.Format.png");

			Bitmap bitmap = new Bitmap(stream);
			bitmap.MakeTransparent(Color.FromArgb(255, 0, 255));
			int count = (int)(bitmap.Width / bitmap.Height);
			images = new Image[count];
			Rectangle rectangle = new Rectangle(0, 0, bitmap.Height, bitmap.Height);
			for (int i = 0; i < count; i++)
			{
				images[i] = bitmap.Clone(rectangle, bitmap.PixelFormat);
				rectangle.X += bitmap.Height;
			}
			
			stream.Close();
		}

		public static Image ForeColor     { get { return images[0]; } }
		public static Image BackColor     { get { return images[1]; } }

		public static Image Bold          { get { return images[2]; } }
		public static Image Italic        { get { return images[3]; } }
		public static Image Underline     { get { return images[4]; } }

		public static Image Superscript   { get { return images[5]; } }
		public static Image Subscript     { get { return images[6]; } }

		public static Image Strikethrough	{ get { return images[7]; } }

		public static Image AlignLeft     { get { return images[8]; } }
		public static Image AlignCenter   { get { return images[9]; } }
		public static Image AlignRight    { get { return images[10]; } }

		public static Image OrderedList   { get { return images[11]; } }
		public static Image UnorderedList { get { return images[12]; } }

		public static Image Unindent      { get { return images[13]; } }
		public static Image Indent        { get { return images[14]; } }

		public static Image Black         { get { return images[15]; } }
		public static Image Red           { get { return images[16]; } }
		public static Image Green         { get { return images[17]; } }
		public static Image Blue          { get { return images[18]; } }
		public static Image Yellow        { get { return images[19]; } }
		public static Image White         { get { return images[20]; } }

		public static Image Hyperlink     { get { return images[21]; } }
		public static Image Picture       { get { return images[22]; } }
	}

	internal sealed class CommandBarImageResource
	{
		private static Image[] images = null;

		private CommandBarImageResource()
		{
		}

		// ImageList.Images[int index] does not preserve alpha channel.
		static CommandBarImageResource()
		{
			Stream stream = typeof(CommandBarImageResource).Assembly.GetManifestResourceStream("Writer.CommandBar.png");

			Bitmap bitmap = new Bitmap(stream);
			int count = (int) (bitmap.Width / bitmap.Height);
			images = new Image[count];
			Rectangle rectangle = new Rectangle(0, 0, bitmap.Height, bitmap.Height);
			for (int i = 0; i < count; i++)
			{
				images[i] = bitmap.Clone(rectangle, bitmap.PixelFormat);
				rectangle.X += bitmap.Height;
			}
			
			stream.Close();
		}	

		public static Image New               { get { return images[0];  } }
		public static Image Open              { get { return images[1];  } }
		public static Image Save              { get { return images[2];  } }
		public static Image Cut               { get { return images[3];  } }
		public static Image Copy              { get { return images[4];  } }
		public static Image Paste             { get { return images[5];  } }
		public static Image Delete            { get { return images[6];  } }
		public static Image Properties        { get { return images[7];  } }
		public static Image Undo              { get { return images[8];  } }
		public static Image Redo              { get { return images[9];  } }
		public static Image Preview           { get { return images[10]; } }
		public static Image Print             { get { return images[11]; } }
		public static Image Search            { get { return images[12]; } }
		public static Image ReSearch          { get { return images[13]; } }
		public static Image Help              { get { return images[14]; } }
		public static Image ZoomIn            { get { return images[15]; } }
		public static Image ZoomOut           { get { return images[16]; } }
		public static Image Back              { get { return images[17]; } }
		public static Image Forward           { get { return images[18]; } }
		public static Image Favorites         { get { return images[19]; } }
		public static Image AddToFavorites    { get { return images[20]; } }
		public static Image Stop              { get { return images[21]; } }
		public static Image Refresh           { get { return images[22]; } }
		public static Image Home              { get { return images[23]; } }
		public static Image Edit              { get { return images[24]; } }
		public static Image Tools             { get { return images[25]; } }
		public static Image Tiles             { get { return images[26]; } }
		public static Image Icons             { get { return images[27]; } }
		public static Image List              { get { return images[28]; } }
		public static Image Details           { get { return images[29]; } }
		public static Image Pane              { get { return images[30]; } }
		public static Image Culture           { get { return images[31]; } }
		public static Image Languages         { get { return images[32]; } }
		public static Image History           { get { return images[33]; } }
		public static Image Mail              { get { return images[34]; } }
		public static Image Parent            { get { return images[35]; } }
		public static Image FolderProperties  { get { return images[36]; } }
	}

	internal sealed class IconResource
	{
		private IconResource()
		{
		}

		public static Icon Application
		{
			get { return new Icon(typeof(IconResource).Assembly.GetManifestResourceStream("Writer.Application.ico")); }	
		}	
	}

	internal sealed class ImageResource
	{
		private static Image application = null;

		private ImageResource()
		{
		}

		public static Image Application
		{
			get 
			{ 
				if (application == null)
				{
					Stream stream = typeof(ImageResource).Assembly.GetManifestResourceStream("Writer.Application.png");
					application = new Bitmap(stream);
					stream.Close();
				}
				
				return application;
			}	
		}	
	}
	
	internal sealed class Resource
	{
		private Resource()
		{
		}

		public static string GetString(string name)
		{
			switch (name)
			{
				case "ApplicationName":
					return "Writer";

				case "Homepage":
					return "http://www.lutzroeder.com";

				case "Ok":
					return "OK";

				case "Cancel":
					return "Cancel";

				case "HtmlFilter":
					return "HTML files (*.htm,*.html)|*.htm;*.html|All files (*.*)|*.*";

				case "PictureFilter":
					return "Picture files (*.jpg,*.png,*.bmp,*.gif)|*.jpg;*.png;*.bmp;*.gif|All files (*.*)|*.*";
			}
			
			throw new NotImplementedException();	
		}
	}
}
