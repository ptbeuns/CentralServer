using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CentralServer
{
    public class Server
    {
        public RailwayManager RailwayManager;
        private Socket listnerSocket;

        public Server()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 4337);

            listnerSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listnerSocket.Bind(localEndPoint);
                listnerSocket.Listen(20);
            }
            catch
            {
            }
        }

        public Socket AcceptClient()
        {
            if (!listnerSocket.IsBound)
            {
                return null;
            }

            Socket socket = null;

            try
            {
                socket = listnerSocket.Accept();
            }
            catch
            {
            }

            return socket;
        }
    }
}