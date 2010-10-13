namespace Writer
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;

	internal class Configuration : IConfiguration
	{
		public event PropertyChangedEventHandler PropertyChanged;
	
		private Property[] properties = new Property[0];
	
		public void Clear()
		{
			for (int i = this.properties.Length - 1; i >= 0; i--)
			{
				string name = this.properties[i].Name;
				this[name] = null;
			}
		}

		public string this[string name]
		{
			get
			{
				for (int i = 0; i < this.properties.Length; i++)
				{
					if ((this.properties[i].Name != null) && (this.properties[i].Name == name))
					{
						return this.properties[i].Value;
					}
				}

				return null;
			}

			set
			{
				if (value != null)
				{
					for (int i = 0; i < this.properties.Length; i++)
					{
						if (name == this.properties[i].Name)
						{
							// Change value of existing property
							this.properties[i].Value = value;
							this.OnPropertyChanged(new PropertyChangedEventArgs(name));
							return;
						}
					}

					// Insert property
					Property[] newProperties = new Property[this.properties.Length + 1];
					Array.Copy(this.properties, 0, newProperties, 0, this.properties.Length);
					newProperties[this.properties.Length] = new Property(name, value);
					this.properties = newProperties;
					this.OnPropertyChanged(new PropertyChangedEventArgs(name));
				}
				else
				{
					for (int i = 0; i < this.properties.Length; i++)
					{
						if (name == this.properties[i].Name)
						{
							// Remove property
							Property[] newProperties = new Property[this.properties.Length - 1];
							Array.Copy(this.properties, 0, newProperties, 0, i);
							Array.Copy(this.properties, i + 1, newProperties, i, this.properties.Length - i - 1);
							this.properties = newProperties;
							this.OnPropertyChanged(new PropertyChangedEventArgs(name));
						}
					}					
				}
			}
		}

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		internal bool IsEmpty
		{
			get
			{
				return (this.properties.Length == 0);
			}
		}

		internal void Load(StreamReader reader)
		{
			this.Clear();

			int index = 0;
			while (reader.Peek() != -1)
			{
				long position = reader.BaseStream.Position;
				string line = reader.ReadLine();
				line = line.Trim();

				if ((line.Length > 0) && (!line.StartsWith("[")))
				{
					string propertyName = index.ToString();
					string propertyValue = line;

					int assignIndex = line.IndexOf("=");
					int quoteIndex = line.IndexOf("\"");
					if ((assignIndex != -1) && (assignIndex < quoteIndex))
					{
						propertyName = line.Substring(0, assignIndex);
						propertyValue = line.Substring(assignIndex + 1);
					}
					
					if (propertyValue.StartsWith("\""))
					{
						propertyValue = propertyValue.Substring(1);
					}

					if (propertyValue.EndsWith("\""))
					{
						propertyValue = propertyValue.Substring(0, propertyValue.Length - 1);
					}

					this[propertyName] = propertyValue;
				}
				else
				{
					reader.BaseStream.Position = position;
					break;
				}

				index++;
			}
		}

		internal void Save(StreamWriter writer)
		{
			bool isList = true;
			for (int i = 0; i < this.properties.Length; i++)
			{
				if (i.ToString() != this.properties[i].Name)
				{
					isList = false;
				}
			}

			for (int i = 0; i < this.properties.Length; i++)
			{
				if (!isList)
				{
					writer.Write(this.properties[i].Name);
					writer.Write("=");
				}

				writer.Write("\"");
				writer.Write(this.properties[i].Value);
				writer.Write("\"");
				writer.WriteLine();
			}
		}

		private class Property
		{
			public string Name;
			public string Value;

			public Property(string name, string value)
			{
				this.Name = name;
				this.Value = value;
			}
		}
	}
}

