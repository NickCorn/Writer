// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
// Contributed by: 
// Husein Choroomi (http://www.ManagedComponents.com)
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Collections;
	using System.Text;
	using System.Security.Cryptography;
	using System.IO;
	
	internal class FtpConfiguration
	{
		private IConfiguration configuration;
		private SymmetricAlgorithm symAlgorithm = new RijndaelManaged();
		private string _iv = "edwq7SgLQYYffT/RN3Y5cA==";
		private string _key = "9Ojb8DCkF6VxgBNq2i5c67dZ3+nJq/GBaGOTrPEthnI=";
		
		private string host;
		private string userName;
		private string password;
		private string directory;
		private string port;

		public IConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
			
			set
			{
				this.configuration = value;

				this.host = this.configuration["Host"];
				this.userName = this.configuration["UserName"];
				this.password = this.DecryptPassword(this.configuration["Password"]);
				this.directory = this.configuration["Directory"];
				this.port = this.configuration["Port"];
			}
		}

		public bool IsEmpty
		{
			get
			{
				return ((this.host == null) || (this.userName == null) || (this.directory == null) || (this.port == null));
			}
		}

		public string Host
		{
			get
			{
				return this.host;	
			}	
		
			set
			{
				this.host = value;
				this.UpdateConfiguation();
			}
		}

		public string UserName
		{
			get
			{
				return this.userName;	
			}	
		
			set
			{
				this.userName = value;
				this.UpdateConfiguation();
			}
		}

		public string Password
		{
			get
			{
				return this.password;
			}	
		
			set
			{
				this.password = value;
				this.UpdateConfiguation();
			}
		}

		public string Directory
		{
			get
			{
				return this.directory;
			}	
		
			set
			{
				this.directory = value;
				this.UpdateConfiguation();
			}
		}

		public string Port
		{
			get
			{
				return this.port;
			}	
		
			set
			{
				this.port = value;
				this.UpdateConfiguation();
			}
		}
		
		private void UpdateConfiguation()
		{
				this.configuration.Clear();
				this.configuration["Host"] = this.host;
				this.configuration["UserName"] = this.userName;
				this.configuration["Password"] = this.EncryptPassword(this.password);
				this.configuration["Directory"] = this.directory;
				this.configuration["Port"] = this.port;
		}

		private string EncryptPassword(string password) 
		{
			if (password != null)
			{
				ICryptoTransform encryptor =  symAlgorithm.CreateEncryptor(Convert.FromBase64String(_key), Convert.FromBase64String(_iv));
				ASCIIEncoding ascii = new ASCIIEncoding();
				byte[] passByte = ascii.GetBytes(password);
				byte[] cryptoByte = Crypto(encryptor, passByte);
				return Convert.ToBase64String(cryptoByte); 
			}
			
			return null;
		}

		private string DecryptPassword(string password) 
		{
			if (password != null)
			{
				ICryptoTransform decryptor =  symAlgorithm.CreateDecryptor(Convert.FromBase64String(_key), Convert.FromBase64String(_iv));
				byte[] cryptoByte = Convert.FromBase64String(password);
				byte[] passwordByte = Crypto(decryptor, cryptoByte);
				System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
				return ascii.GetString(passwordByte); 
			}
			
			return null;
		}

		private byte[] Crypto(ICryptoTransform op, byte[] input) 
		{
			MemoryStream memoryStream = new MemoryStream(); 
			CryptoStream cryptoStream = new CryptoStream(memoryStream, op, CryptoStreamMode.Write);
			cryptoStream.Write(input, 0, input.Length); 
			cryptoStream.Close();
			return memoryStream.ToArray();
		}
	}
}
