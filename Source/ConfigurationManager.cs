namespace Writer
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Windows.Forms;
	using System.Xml;

	internal class ConfigurationManager : IConfigurationManager
	{
		private SortedList table = new SortedList();

		public IConfiguration this[string name]
		{
			get
			{
				if (!this.table.Contains(name))
				{
					this.table.Add(name, new Configuration());
				}

				return (IConfiguration)this.table[name];
			}
		}

		public void Load(string fileName)
		{
			fileName = Path.ChangeExtension(fileName, ".cfg");
			if (File.Exists(fileName))
			{
				using (Stream stream = File.OpenRead (fileName))
				{
					this.Load(stream);
				}
			}
		}

		public void Save(string fileName)
		{
			fileName = Path.ChangeExtension(fileName, ".cfg");

			Stream stream = null;
			try
			{
				stream = File.Create(fileName);
				this.Save(stream);
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
		}

		public void Load(Stream stream)
		{
			using (StreamReader reader = new StreamReader(stream))
			{
				while (reader.Peek() != -1)
				{
					string line = reader.ReadLine();
					line = line.Trim();
					if (line.Length > 0)
					{
						if ((line.StartsWith("[")) && (line.EndsWith("]")))
						{
							string name = line.Substring(1, line.Length - 2);
							Configuration configuration = (Configuration) this[name];
							configuration.Load(reader);
						}
					}
				}
			}
		}

		public void Save(Stream stream)
		{
			using (StreamWriter writer = new StreamWriter(stream))
			{
				IDictionaryEnumerator enumerator = this.table.GetEnumerator();
				while (enumerator.MoveNext())
				{
					Configuration configuration = (Configuration) enumerator.Value;
					if (!configuration.IsEmpty)
					{
						writer.WriteLine();
						writer.Write("[");
						writer.Write((string) enumerator.Key);
						writer.Write("]");
						writer.WriteLine();
						configuration.Save(writer);
					}
				}
			}
		}
	}
}
