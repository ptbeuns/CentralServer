using System;
using System.Net;
using System.Net.Sockets;

namespace CentralServer
{
    public class Connection
    {
        private Socket connectionSocket;
        public string Message;
        

        public Connection(Socket socket)
        {
            if (socket == null)
            {
                throw new NullReferenceException("socket");
            }

            connectionSocket = socket;
        }

        public void ReceiveMessage()
        {
            //TODO
        }

        public void SendMessage()
        {
            //TODO
        }
    }
}