using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChicShop.Chic.WebServer
{
    public class WebServer
    {
        public int Port = 5050;

        private TcpListener listener;

        public WebServer(int port = 5050)
        {
            Port = port;
        }

        public void Start()
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();

            var thread = new Thread(new ThreadStart(StartListen));
            thread.Start();
        }

        public void StartListen()
        {
            while (true)
            {
                Socket socket = listener.AcceptSocket();

                if (socket.Connected)
                {
                    byte[] receive = new byte[1024];
                    socket.Receive(receive, receive.Length, 0);
                    string buffer = Encoding.ASCII.GetString(receive);

                    var message = "Hello World!";

                    SendHeader(buffer.Substring(buffer.IndexOf("HTTP", 1), 8), "text/plain", message.Length, "200 OK", ref socket);
                    Send(message, ref socket);
                }

                socket.Close();
            }
        }

        public void SendHeader(string httpVersion, string mimeType, int totalBytes, string statusCode, ref Socket socket)
        {
            string buffer = "";

            if (string.IsNullOrWhiteSpace(mimeType)) mimeType = "text/plain";

            buffer += httpVersion + statusCode + "\r\n";
            buffer += "Server: cx1193719-b\r\n";
            buffer += "Content-Type: " + mimeType + "\r\n";
            buffer += "Accept-Ranges: bytes\r\n";
            buffer += "Content-Length: " + totalBytes + "\r\n\r\n";

            Send(buffer, ref socket);
        }

        public void Send(string data, ref Socket socket) 
            => Send(Encoding.ASCII.GetBytes(data), ref socket);
        public void Send(byte[] data, ref Socket socket)
        {
            if (socket.Connected)
            {
                socket.Send(data, data.Length, 0);
            }
        }
    }
}
