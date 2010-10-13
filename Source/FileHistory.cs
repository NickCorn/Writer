// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Collections;

	internal class FileHistory
	{
		private IConfiguration configuration;
		private ArrayList files = new ArrayList();

		public IConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
			
			set
			{
				this.configuration = value;	
			
				this.files.Clear();
				
				int index = 0;
				while (this.configuration[index.ToString()] != null)
				{
					this.files.Add(this.configuration[index.ToString()]);
					index++;
				}
			}	
		}

		public void AddFile(string url)
		{
			if ((url != null) && (url.Length != 0) && (!this.files.Contains(url)))
			{
				this.files.Insert(0, url);
				this.UpdateConfiguration();
			}
		}

		public int Count
		{
			get 
			{ 
				return this.files.Count; 
			}
		}

		public string this[int index]
		{
			get 
			{
				 return (index >= this.Count) ? string.Empty : (string)this.files[index]; 
			}
		}

		public void UpdateConfiguration()
		{
			this.configuration.Clear();			
			
			for (int i = 0; i < this.files.Count; i++)
			{
				this.configuration[i.ToString()] = (string) this.files[i];
			}
		}
	}
}