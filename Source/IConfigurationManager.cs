namespace Writer
{
	using System;
	using System.IO;
	using System.Xml;

	internal interface IConfigurationManager
	{
		IConfiguration this[string name] { get; }
	}
}
