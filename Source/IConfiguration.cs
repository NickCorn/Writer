namespace Writer
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Xml;

	internal interface IConfiguration
	{
		event PropertyChangedEventHandler PropertyChanged;
		string this[string name] { get; set; }
		void Clear();
	}
}
