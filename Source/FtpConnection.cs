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
	using System.Net;
	using System.IO;
	using System.Text;
	using System.Net.Sockets;

	internal class FtpConnection : IDisposable 
	{
		private string host;
		private string directory;
		private string userID;
		private string password;
		private string message;
		private string reply;
		private int port;
		private int bytes;
		private int retrievedValue;
		private Socket clientSocket;
		Byte[] buffer = new Byte[512];
		Encoding ascii = Encoding.ASCII;

		public FtpConnection(string host, string userID, string password, int port) {
			this.host = host;
			this.userID = userID;
			this.password = password;
			this.port = port;

			this.Connect();
		}

		public void Connect() {
			this.clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
			IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry(this.host).AddressList[0], this.port);

			try {
				this.clientSocket.Connect(ep);
			}
			catch {
				throw new IOException("Couldn't connect to remote server");
			}

			this.Reply();
			if(this.retrievedValue != 220) {
				this.Disconnect();
				throw new IOException(this.reply.Substring(4));
			}
			
			this.Command("USER " + this.userID);

			if( !(this.retrievedValue == 331 || this.retrievedValue == 230) ) {
				this.Dispose();
				throw new IOException(this.reply.Substring(4));
			}

			if( this.retrievedValue != 230 ) {
				this.Command("PASS " + this.password);
				if( !(this.retrievedValue == 230 || this.retrievedValue == 202) ) {
					this.Dispose();
					throw new IOException(this.reply.Substring(4));
				}
			}

			this.Command("TYPE I");
			if (this.retrievedValue != 200) {
				throw new IOException(this.reply.Substring(4));
			}
		}

		public void Upload(string fileName) {

			Socket cSocket = this.DataSocket();
			this.Command("STOR " + Path.GetFileName(fileName));

			if( !(this.retrievedValue == 125 || this.retrievedValue == 150) ) {
				throw new IOException(this.reply.Substring(4));
			}

			FileStream input = new FileStream(fileName, FileMode.Open);

			while ((this.bytes = input.Read(this.buffer, 0, this.buffer.Length)) > 0) {
				cSocket.Send(this.buffer, this.bytes, 0);
			}
			input.Close();

			if (cSocket.Connected) {
				cSocket.Close();
			}

			this.Reply();
			if( !(this.retrievedValue == 226 || this.retrievedValue == 250) ) {
				throw new IOException(this.reply.Substring(4));
			}
		}

		public void ChangeDirectory(string directory) {
			this.Command("CWD " + directory);
			if(this.retrievedValue != 250) {
				throw new IOException(this.reply.Substring(4));
			}
			this.directory = directory;
		}

		public void Dispose() {
			if(this.clientSocket != null) {
				this.clientSocket.Close();
				this.clientSocket = null;
			}
		}

		public void Disconnect() {
			if( this.clientSocket != null ) {
				this.Command("QUIT");
			}
			this.Dispose();
		}

		private void Reply() {
			this.message = "";
			this.reply = this.GetLine();
			this.retrievedValue = Int32.Parse(this.reply.Substring(0,3));
		}

		private string GetLine() {
			while(true) {
				this.bytes = this.clientSocket.Receive(this.buffer, this.buffer.Length, 0);
				this.message += this.ascii.GetString(this.buffer, 0, this.bytes);
				if(this.bytes < this.buffer.Length) {
					break;
				}
			}

			char[] seperator = {'\n'};
			string[] _message = this.message.Split(seperator);

			if(this.message.Length > 2) {
				this.message = _message[_message.Length-2];
			}
			else {
				this.message = _message[0];
			}

			if(!this.message.Substring(3,1).Equals(" ")) {
				return this.GetLine();
			}

			return this.message;
		}

		private void Command(String command) {
			Byte[] cmdBytes = Encoding.ASCII.GetBytes((command+"\r\n").ToCharArray());
			this.clientSocket.Send(cmdBytes, cmdBytes.Length, 0);
			this.Reply();
		}

		private Socket DataSocket() {

			this.Command("PASV");

			if(this.retrievedValue != 227) {
				throw new IOException(this.reply.Substring(4));
			}

			int index1 = this.reply.IndexOf('(');
			int index2 = this.reply.IndexOf(')');
			string ipData = this.reply.Substring(index1+1 ,index2-index1-1);
			int[] parts = new int[6];

			int len = ipData.Length;
			int partCount = 0;
			string buf = "";

			for (int i = 0; i < len && partCount <= 6; i++) {

				char ch = Char.Parse(ipData.Substring(i,1));
				if (Char.IsDigit(ch))
					buf += ch;
				else if (ch != ',') {
					throw new IOException("Malformed PASV reply: " + this.reply);
				}

				if (ch == ',' || i+1 == len) {

					try {
						parts[partCount++] = Int32.Parse(buf);
						buf = "";
					}
					catch {
						throw new IOException("Malformed PASV reply: " + this.reply);
					}
				}
			}

			string ipAddress = parts[0] + "."+ parts[1]+ "." + parts[2] + "." + parts[3];
			int port = (parts[4] << 8) + parts[5];

			Socket s = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
			IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry(ipAddress).AddressList[0], port);

			try {
				s.Connect(ep);
			}
			catch {
				throw new IOException("Can't connect to remote server");
			}
			return s;
		}
	}
}